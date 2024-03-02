// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LibGDXSharp.LibCore.Assets.Loaders;

namespace LibGDXSharp.LibCore.Assets;

/// <summary>
///     Responsible for loading an asset through an <see cref="AssetLoaderBase" /> based
///     on an <see cref="AssetDescriptor" />.
/// </summary>
[PublicAPI]
public class AssetLoadingTask
{
    public bool                     DependenciesLoaded { get; set; }
    public AssetDescriptor          AssetDesc          { get; }
    public bool                     Cancel             { get; set; }
    public object?                  Asset              { get; set; }
    public List< AssetDescriptor >? Dependencies       { get; set; }

    // ------------------------------------------------------------------------

    private readonly AssetLoaderBase _loader;
    private readonly AssetManager    _manager;
    private volatile bool            _asyncDone = false;
    private volatile object?         _asset;
    private          long            _startTime;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="assetDesc"></param>
    /// <param name="loader"></param>
    public AssetLoadingTask( AssetManager manager,
                             AssetDescriptor assetDesc,
                             AssetLoaderBase loader )
    {
        this._manager   = manager;
        this.AssetDesc  = assetDesc;
        this._loader    = loader;
        this._startTime = manager.Log.Level == Logger.LOG_DEBUG ? TimeUtils.NanoTime() : 0;
    }

    // ------------------------------------------------------------------------

    /// <summary>
    ///     Loads parts of the asset asynchronously if the loader is
    ///     an <see cref="AsynchronousAssetLoader{T,TP}" />.
    /// </summary>
    public void Call()
    {
        if ( Cancel )
        {
            return;
        }

        var asyncLoader = ( AsynchronousAssetLoader? )_loader;

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
                // if we have no dependencies, we load the
                // async part of the task immediately.
                asyncLoader?.Load( _manager,
                                   AssetDesc.Filepath,
                                   Resolve( asyncLoader, AssetDesc ),
                                   AssetDesc.Parameters! );

                _asyncDone = true;
            }
        }
        else
        {
            asyncLoader?.Load( _manager,
                               AssetDesc.Filepath,
                               Resolve( asyncLoader, AssetDesc ),
                               AssetDesc.Parameters! );

            _asyncDone = true;
        }
    }

    /// <summary>
    ///     Updates the loading of the asset. In case the asset is loaded with an
    ///     <see cref="AsynchronousAssetLoader{T, TP}" />, the loaders
    ///     <see cref="AsynchronousAssetLoader{T, TP}.Load" /> method is first called on a
    ///     worker thread. Once this method returns, the rest of the asset is loaded on the
    ///     rendering thread via <see cref="AsynchronousAssetLoader{T, TP}.Load" />.
    /// </summary>
    /// <returns> true in case the asset was fully loaded, false otherwise </returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public bool Update()
    {
        HandleAsyncLoader();

        return Asset != null;
    }

    /// <summary>
    /// </summary>
    public void Unload()
    {
        if ( !_loader.IsSynchronous )
        {
            ( ( AsynchronousAssetLoader< Type, AssetLoaderParameters > )_loader )
               .Unload( _manager,
                        AssetDesc.Filepath,
                        Resolve( _loader, AssetDesc ),
                        AssetDesc.Parameters! );
        }
    }

    /// <summary>
    ///     Resolve the file and path names for the asset described in
    ///     the member <see cref="AssetDesc"/>
    /// </summary>
    /// <param name="loader"></param>
    /// <param name="assetDesc"></param>
    /// <returns></returns>
    private static FileInfo? Resolve( AssetLoaderBase? loader, AssetDescriptor? assetDesc )
    {
        if ( assetDesc is { File: null } descriptor )
        {
            if ( loader != null )
            {
                descriptor.File = loader.Resolve( descriptor.Filepath );
            }
        }

        return assetDesc?.File;
    }

    /// <summary>
    /// </summary>
    private void HandleAsyncLoader()
    {
//        if ( AssetDesc == null )
//        {
//            throw new GdxRuntimeException( "Unable to load asset: AssetDesc is null" );
//        }
//
//        var asyncLoader = ( AsynchronousAssetLoader? )_loader;
//
//        if ( asyncLoader == null )
//        {
//            throw new GdxRuntimeException( "asyncLoader is null" );
//        }
//
//        if ( !DependenciesLoaded )
//        {
//            if ( _depsFuture == null )
//            {
//                _depsFuture = AsyncExecutor.Submit( this );
//            }
//            else if ( _depsFuture.IsDone )
//            {
//                try
//                {
//                    _depsFuture.Get();
//                }
//                catch ( Exception e )
//                {
//                    throw new GdxRuntimeException( $"Couldn't load dependencies of asset: {AssetDesc.Filepath}", e );
//                }
//
//                DependenciesLoaded = true;
//
//                if ( _asyncDone )
//                {
//                    Asset = asyncLoader.Load( _manager,
//                                              AssetDesc.Filepath,
//                                              Resolve( _loader, AssetDesc ),
//                                              AssetDesc.Parameters! );
//                }
//            }
//        }
//        else if ( ( _loadFuture == null ) && !_asyncDone )
//        {
//            _loadFuture = AsyncExecutor.Submit( this );
//        }
//        else if ( _asyncDone )
//        {
//            Asset = asyncLoader.LoadSync( _manager,
//                                          AssetDesc.Filepath,
//                                          Resolve( _loader, AssetDesc ),
//                                          AssetDesc.Parameters! );
//        }
//        else if ( _loadFuture is { IsDone: true } )
//        {
//            try
//            {
//                _loadFuture.Get();
//            }
//            catch ( Exception e )
//            {
//                throw new GdxRuntimeException( $"Couldn't load asset: {AssetDesc.Filepath}", e );
//            }
//
//            Asset = asyncLoader.LoadSync( _manager,
//                                          AssetDesc.Filepath,
//                                          Resolve( _loader, AssetDesc ),
//                                          AssetDesc.Parameters! );
//        }
    }

    /// <summary>
    ///     Removes any duplicate assets from the referenced asset list.
    /// </summary>
    private static void RemoveDuplicates( List< AssetDescriptor > array )
    {
        for ( var i = 0; i < array.Count; ++i )
        {
            var  fn   = array[ i ].Filepath;
            Type type = array[ i ].AssetType;

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
