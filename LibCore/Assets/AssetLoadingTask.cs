// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


using LughSharp.LibCore.Utils.Async;
using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Assets;

/// <summary>
/// Responsible for loading an asset through an <see cref="AssetLoader"/> based
/// on an <see cref="AssetDescriptor"/>.
/// </summary>
[PublicAPI]
public class AssetLoadingTask : IAsyncTask< object >
{
    public bool                     DependenciesLoaded { get; set; }
    public List< AssetDescriptor >? Dependencies       { get; set; }
    public AssetDescriptor          AssetDesc          { get; }
    public bool                     Cancel             { get; set; }
    public object?                  Asset              { get; set; }

    // ------------------------------------------------------------------------

    private readonly AsyncExecutor _executor;
    private readonly AssetLoader   _loader;
    private readonly AssetManager  _manager;
    private readonly long          _startTime;

    private volatile bool _asyncDone = false;

    private AsyncResult< object >? _depsFuture;
    private AsyncResult< object >? _loadFuture;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="AssetLoadingTask"/> class.
    /// </summary>
    /// <param name="manager">The asset manager responsible for managing assets.</param>
    /// <param name="assetDesc">The descriptor of the asset to be loaded.</param>
    /// <param name="loader">The loader used to load the asset.</param>
    /// <param name="threadPool">The asynchronous executor for handling loading tasks.</param>
    public AssetLoadingTask( AssetManager manager, AssetDescriptor assetDesc, AssetLoader loader, AsyncExecutor threadPool )
    {
        Logger.CheckPoint();

        _manager   = manager;
        _executor  = threadPool;
        AssetDesc  = assetDesc;
        _loader    = loader;
        _startTime = Logger.TraceLevel == Logger.LOG_DEBUG ? TimeUtils.NanoTime() : 0;
    }

    /// <summary>
    /// Loads parts of the asset asynchronously if the loader is
    /// an <see cref="AsynchronousAssetLoader{T,TP}"/>.
    /// </summary>
    public object? Call()
    {
        if ( Cancel )
        {
            return null;
        }

        var asyncLoader = ( AsynchronousAssetLoader< Type, AssetLoaderParameters >? ) _loader;

        if ( !DependenciesLoaded )
        {
            Dependencies = asyncLoader?.GetDependencies( AssetDesc.Filepath,
                                                         Resolve( asyncLoader, AssetDesc ),
                                                         AssetDesc.Parameters );

            if ( Dependencies != null )
            {
                RemoveDuplicates( Dependencies );

                _manager.InjectDependencies( AssetDesc.Filepath, Dependencies );
            }
            else
            {
                // if we have no dependencies, we load the async part of the task immediately.
                asyncLoader?.LoadSync( _manager, Resolve( asyncLoader, AssetDesc ), AssetDesc.Parameters! );

                _asyncDone = true;
            }
        }
        else
        {
            asyncLoader?.LoadSync( _manager,
                                   Resolve( asyncLoader, AssetDesc ),
                                   AssetDesc.Parameters! );

            _asyncDone = true;
        }

        return null;
    }

    /// <summary>
    /// Updates the loading of the asset. In case the asset is loaded with an
    /// <see cref="AsynchronousAssetLoader{T, TP}"/>, the loaders
    /// <see cref="AsynchronousAssetLoader{TAssetType,TParameters}.LoadAsync"/> method is first called on a
    /// worker thread. Once this method returns, the rest of the asset is loaded on the
    /// rendering thread via <see cref="AsynchronousAssetLoader{TAssetType,TParameters}.LoadAsync"/>.
    /// </summary>
    /// <returns> true in case the asset was fully loaded, false otherwise </returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public bool Update()
    {
        HandleAsyncLoader();

        return Asset != null;
    }

    /// <summary>
    /// Called when this task is the task that is currently being processed
    /// and it is unloaded.
    /// </summary>
    public void Unload()
    {
        if ( !_loader.IsSynchronous )
        {
            ( ( AsynchronousAssetLoader< Type, AssetLoaderParameters > ) _loader )
               .UnloadAsync( _manager,
                             Resolve( _loader, AssetDesc ),
                             AssetDesc.Parameters! );
        }
    }

    /// <summary>
    /// Handles the loading of assets using a synchronous asset loader.
    /// It manages dependencies and ensures they are loaded before the main asset.
    /// </summary>
    private void HandleSyncLoader()
    {
        var syncLoader = ( SynchronousAssetLoader< Type, AssetLoaderParameters > ) _loader;

        if ( !DependenciesLoaded )
        {
            DependenciesLoaded = true;
            Dependencies       = syncLoader.GetDependencies( AssetDesc.Filepath, Resolve( _loader, AssetDesc ), AssetDesc.Parameters );

            if ( Dependencies == null )
            {
                Asset = syncLoader.Load( _manager, Resolve( _loader, AssetDesc )!, AssetDesc.Parameters! );

                return;
            }

            RemoveDuplicates( Dependencies );

            _manager.InjectDependencies( AssetDesc.Filepath, Dependencies );
        }
        else
        {
            Asset = syncLoader.Load( _manager, Resolve( _loader, AssetDesc )!, AssetDesc.Parameters! );
        }
    }

    /// <summary>
    /// Handles the loading of assets using an asynchronous asset loader.
    /// Manages the asynchronous loading process, including dependency
    /// loading and main asset loading.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// Thrown if the asset description or asynchronous loader is null, or if there
    /// is an error loading dependencies or the asset.
    /// </exception>
    private void HandleAsyncLoader()
    {
        if ( AssetDesc == null )
        {
            throw new GdxRuntimeException( "Unable to load asset: AssetDesc is null" );
        }

        var asyncLoader = ( AsynchronousAssetLoader< Type, AssetLoaderParameters >? ) _loader;

        if ( asyncLoader == null )
        {
            throw new GdxRuntimeException( "asyncLoader is null" );
        }

        // Check if dependencies have been loaded.
        if ( !DependenciesLoaded )
        {
            // If the future task for loading dependencies is not started, submit it to the executor.
            if ( _depsFuture == null )
            {
                _depsFuture = _executor.Submit( this );
            }

            // If the future task is done, process the dependencies.
            else if ( _depsFuture.IsDone )
            {
                try
                {
                    _depsFuture.Get();
                }
                catch ( Exception e )
                {
                    throw new GdxRuntimeException( $"Couldn't load dependencies of asset: {AssetDesc.Filepath}", e );
                }

                DependenciesLoaded = true;

                // If asynchronous loading is done, load the asset synchronously.
                if ( _asyncDone )
                {
                    Asset = asyncLoader.LoadSync( _manager,
                                                  Resolve( _loader, AssetDesc ),
                                                  AssetDesc.Parameters! );
                }
            }
        }

        // If dependencies are loaded and the loading future task is not started
        // and async loading is not done, submit it to the executor.
        else if ( ( _loadFuture == null ) && !_asyncDone )
        {
            _loadFuture = _executor.Submit( this );
        }

        // If asynchronous loading is done, load the asset synchronously.
        else if ( _asyncDone )
        {
            Asset = asyncLoader.LoadSync( _manager,
                                          Resolve( _loader, AssetDesc ),
                                          AssetDesc.Parameters! );
        }

        // If the loading future task is done, process the loaded asset.
        else if ( _loadFuture is { IsDone: true } )
        {
            try
            {
                _loadFuture.Get();
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( $"Couldn't load asset: {AssetDesc.Filepath}", e );
            }

            Asset = asyncLoader.LoadSync( _manager,
                                          Resolve( _loader, AssetDesc ),
                                          AssetDesc.Parameters! );
        }
    }

    /// <summary>
    /// Resolves the file path of an asset descriptor using the provided asset loader.
    /// </summary>
    /// <param name="loader">The asset loader used to resolve the file path.</param>
    /// <param name="assetDesc">The asset descriptor containing the file path to be resolved.</param>
    /// <returns>
    /// The resolved <see cref="FileInfo"/> object, or null if the resolution
    /// fails or the asset descriptor is null.
    /// </returns>
    private static FileInfo? Resolve( AssetLoader? loader, AssetDescriptor? assetDesc )
    {
        // If the asset descriptor is not null and its File property is null, resolve the file path.
        if ( assetDesc is { File: null } descriptor )
        {
            // Use the loader to resolve the file path if the loader is not null.
            if ( loader != null )
            {
                descriptor.File = loader.Resolve( descriptor.Filepath );
            }
        }

        // Return the resolved file or null if resolution did not occur.
        return assetDesc?.File;
    }

    /// <summary>
    /// Removes any duplicate assets from the referenced asset list.
    /// </summary>
    private static void RemoveDuplicates( List< AssetDescriptor > array )
    {
        for ( var i = 0; i < array.Count; ++i )
        {
            var fn   = array[ i ].Filepath;
            var type = array[ i ].AssetType;

            for ( var j = array.Count - 1; j > i; --j )
            {
                if ( ( type == array[ j ].AssetType ) && fn.Equals( array[ j ].Filepath ) )
                {
                    array.RemoveAt( j );
                }
            }
        }
    }
}
