// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Utils.Async;

namespace LibGDXSharp.Assets;

[PublicAPI]
public class AssetLoadingTask
{
    public bool                     DependenciesLoaded { get; set; }
    public AssetDescriptor          AssetDesc          { get; set; }
    public bool                     Cancel             { get; set; }
    public object?                  Asset              { get; set; }

    public List< AssetDescriptor >? dependencies;

    private readonly AssetManager _manager;
    private readonly AssetLoader  _loader;

    private volatile bool          _asyncDone = false;
    private volatile AsyncResult?  _depsFuture;
    private volatile AsyncResult?  _loadFuture;
    private volatile AsyncExecutor _executor;

    private long _startTime;

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="assetDesc"></param>
    /// <param name="loader"></param>
    /// <param name="threadPool"></param>
    public AssetLoadingTask( AssetManager manager,
                             AssetDescriptor assetDesc,
                             AssetLoader loader,
                             AsyncExecutor threadPool )
    {
        _manager   = manager;
        AssetDesc  = assetDesc;
        _loader    = loader;
        _executor  = threadPool;
        _startTime = manager.Log.Level == Logger.LOG_DEBUG ? TimeUtils.NanoTime() : 0;
    }

    /// <summary>
    /// Loads parts of the asset asynchronously if the loader is
    /// an <see cref="AsynchronousAssetLoader{T,TP}"/>.
    /// </summary>
    public void Call()
    {
        if ( Cancel )
        {
            return;
        }

        if ( _loader is AsynchronousAssetLoader asyncLoader )
        {
            if ( !DependenciesLoaded )
            {
                dependencies = asyncLoader.GetDependencies
                    (
                    AssetDesc.FilePath ?? "",
                    Resolve( asyncLoader, AssetDesc ),
                    AssetDesc.Parameters
                    );

                if ( dependencies != null )
                {
                    RemoveDuplicates( dependencies );
                    _manager.InjectDependencies( AssetDesc.FilePath, dependencies );
                }
                else
                {
                    // if we have no dependencies, we load the
                    // async part of the task immediately.
                    asyncLoader.LoadAsync
                        (
                        _manager,
                        AssetDesc.FilePath,
                        Resolve( _loader, AssetDesc ),
                        AssetDesc.Parameters
                        );

                    _asyncDone = true;
                }
            }
            else
            {
                asyncLoader.LoadAsync
                    (
                    _manager,
                    AssetDesc.FilePath,
                    Resolve( _loader, AssetDesc ),
                    AssetDesc.Parameters
                    );

                _asyncDone = true;
            }
        }
    }

    /// <summary>
    /// Updates the loading of the asset. In case the asset is loaded with an
    /// <see cref="AsynchronousAssetLoader{T, TP}"/>, the loaders
    /// <see cref="AsynchronousAssetLoader{T, TP}.LoadAsync"/> method is first called on a
    /// worker thread. Once this method returns, the rest of the asset is loaded on the
    /// rendering thread via <see cref="AsynchronousAssetLoader{T, TP}.LoadSync"/>.
    /// </summary>
    /// <returns> true in case the asset was fully loaded, false otherwise </returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public bool Update()
    {
        // if ( _loader != null && _loader.IsSynchronous )
        if ( _loader is { IsSynchronous: true } )
        {
            HandleSyncLoader();
        }
        else
        {
            HandleAsyncLoader();
        }

        return Asset != null;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Unload()
    {
        if ( !_loader.IsSynchronous )
        {
            ( ( AsynchronousAssetLoader< Type, AssetLoaderParameters > )_loader ).UnloadAsync
                (
                _manager,
                AssetDesc.FilePath,
                Resolve( _loader, AssetDesc ),
                AssetDesc.Parameters
                );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="loader"></param>
    /// <param name="assetDesc"></param>
    /// <returns></returns>
    private FileInfo? Resolve( AssetLoader? loader, AssetDescriptor? assetDesc )
    {
        if ( assetDesc is { File: null } descriptor )
        {
            if ( loader != null )
            {
                descriptor.File = loader.Resolve( descriptor.FilePath );
            }
        }

        return assetDesc?.File;
    }

    /// <summary>
    /// </summary>
    private void HandleSyncLoader()
    {
        var syncLoader = ( SynchronousAssetLoader< Type, AssetLoaderParameters >? )_loader;

        if ( syncLoader == null )
        {
            return;
        }

        if ( !DependenciesLoaded )
        {
            DependenciesLoaded = true;

            dependencies = syncLoader.GetDependencies
                (
                AssetDesc.FilePath,
                Resolve( _loader, AssetDesc ),
                AssetDesc.Parameters
                );

            if ( dependencies == null )
            {
                Asset = syncLoader.Load
                    (
                    _manager,
                    AssetDesc.FilePath,
                    Resolve( _loader, AssetDesc ),
                    AssetDesc.Parameters
                    );

                return;
            }

            RemoveDuplicates( dependencies );
            _manager.InjectDependencies( AssetDesc.FilePath, dependencies );
        }
        else
        {
            Asset = syncLoader.Load
                (
                _manager,
                AssetDesc.FilePath,
                Resolve( _loader, AssetDesc ),
                AssetDesc.Parameters
                );
        }
    }

    /// <summary>
    /// </summary>
    private void HandleAsyncLoader()
    {
        if ( AssetDesc == null )
        {
            throw new GdxRuntimeException( "Unable to load asset: AssetDesc is null" );
        }

        var asyncLoader = ( AsynchronousAssetLoader? )_loader;

        if ( asyncLoader == null )
        {
            throw new GdxRuntimeException( "asyncLoader is null" );
        }

        if ( !DependenciesLoaded )
        {
            if ( _depsFuture == null )
            {
                _depsFuture = _executor.Submit( this );
            }
            else if ( _depsFuture.IsDone() )
            {
                try
                {
                    _depsFuture.Get();
                }
                catch ( System.Exception e )
                {
                    throw new GdxRuntimeException
                        ( $"Couldn't load dependencies of asset: {AssetDesc.FilePath}", e );
                }

                DependenciesLoaded = true;

                if ( _asyncDone )
                {
                    Asset = asyncLoader.LoadSync
                        (
                        _manager,
                        AssetDesc.FilePath,
                        Resolve( _loader, AssetDesc ),
                        AssetDesc.Parameters
                        );
                }
            }
        }
        else if ( ( _loadFuture == null ) && !_asyncDone )
        {
            _loadFuture = _executor.Submit( this );
        }
        else if ( _asyncDone )
        {
            Asset = asyncLoader.LoadSync
                (
                _manager,
                AssetDesc.FilePath,
                Resolve( _loader, AssetDesc ),
                AssetDesc.Parameters
                );
        }
        else if ( ( _loadFuture != null ) && _loadFuture.IsDone() )
        {
            try
            {
                _loadFuture.Get();
            }
            catch ( System.Exception e )
            {
                throw new GdxRuntimeException( $"Couldn't load asset: {AssetDesc.FilePath}", e );
            }

            Asset = asyncLoader.LoadSync
                (
                _manager,
                AssetDesc.FilePath,
                Resolve( _loader, AssetDesc ),
                AssetDesc.Parameters
                );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    private static void RemoveDuplicates( List< AssetDescriptor > array )
    {
        for ( var i = 0; i < array.Count; ++i )
        {
            var  fn   = array[ i ].FilePath;
            Type type = array[ i ].Type;

            for ( var j = array.Count - 1; j > i; --j )
            {
                if ( ( type == array[ j ].Type ) && fn.Equals( array[ j ].FilePath ) )
                {
                    array.RemoveAt( j );
                }
            }
        }
    }
}
