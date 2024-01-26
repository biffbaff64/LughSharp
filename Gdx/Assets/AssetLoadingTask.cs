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

namespace LibGDXSharp.Assets;

/// <summary>
/// Responsible for loading an asset through an <see cref="AssetLoader"/> based
/// on an <see cref="AssetDescriptor"/>.
/// </summary>
[PublicAPI]
public class AssetLoadingTask
{
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private readonly AssetLoader  _loader;
    private readonly AssetManager _manager;

    private volatile bool _asyncDone = false;

    private long _startTime;

    public bool            DependenciesLoaded { get; set; }
    public AssetDescriptor AssetDesc          { get; }
    public bool            Cancel             { get; set; }
    public object?         Asset              { get; set; }

    public List< AssetDescriptor >? dependencies;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="assetDesc"></param>
    /// <param name="loader"></param>
    public AssetLoadingTask( AssetManager manager,
                             AssetDescriptor assetDesc,
                             AssetLoader loader )
    {
        _manager  = manager;
        AssetDesc = assetDesc;
        _loader   = loader;

        _startTime = ( manager.Log.Level == Logger.LOG_DEBUG ) ? TimeUtils.NanoTime() : 0;
    }

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

        if ( _loader is AsynchronousAssetLoader asyncLoader )
        {
            if ( !DependenciesLoaded )
            {
                dependencies = asyncLoader.GetDependencies( AssetDesc.Filepath,
                                                            Resolve( asyncLoader, AssetDesc ),
                                                            AssetDesc.Parameters );

                if ( dependencies != null )
                {
                    RemoveDuplicates( dependencies );

                    _manager.InjectDependencies( AssetDesc.Filepath, dependencies );
                }
                else
                {
                    // if we have no dependencies, we load the
                    // async part of the task immediately.
                    asyncLoader.LoadAsync( _manager,
                                           AssetDesc.Filepath,
                                           Resolve( _loader, AssetDesc ),
                                           AssetDesc.Parameters! );

                    _asyncDone = true;
                }
            }
            else
            {
                asyncLoader.LoadAsync( _manager,
                                       AssetDesc.Filepath,
                                       Resolve( _loader, AssetDesc ),
                                       AssetDesc.Parameters! );

                _asyncDone = true;
            }
        }
    }

    /// <summary>
    ///     Updates the loading of the asset. In case the asset is loaded with an
    ///     <see cref="AsynchronousAssetLoader{T, TP}" />, the loaders
    ///     <see cref="AsynchronousAssetLoader{T, TP}.LoadAsync" /> method is first called on a
    ///     worker thread. Once this method returns, the rest of the asset is loaded on the
    ///     rendering thread via <see cref="AsynchronousAssetLoader{T, TP}.LoadSync" />.
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
    /// </summary>
    public void Unload()
    {
        if ( !_loader.IsSynchronous )
        {
            ( ( AsynchronousAssetLoader< Type, AssetLoaderParameters > )_loader )
               .UnloadAsync( _manager,
                             AssetDesc.Filepath,
                             Resolve( _loader, AssetDesc ),
                             AssetDesc.Parameters! );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="loader"></param>
    /// <param name="assetDesc"></param>
    /// <returns></returns>
    private static FileInfo? Resolve( AssetLoader? loader, AssetDescriptor? assetDesc )
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

            dependencies = syncLoader.GetDependencies( AssetDesc.Filepath,
                                                       Resolve( _loader, AssetDesc ),
                                                       AssetDesc.Parameters );

            if ( dependencies == null )
            {
                Asset = syncLoader.Load( _manager,
                                         AssetDesc.Filepath,
                                         Resolve( _loader, AssetDesc ),
                                         AssetDesc.Parameters! );

                return;
            }

            RemoveDuplicates( dependencies );
            _manager.InjectDependencies( AssetDesc.Filepath, dependencies );
        }
        else
        {
            Asset = syncLoader.Load( _manager,
                                     AssetDesc.Filepath,
                                     Resolve( _loader, AssetDesc ),
                                     AssetDesc.Parameters! );
        }
    }

    /// <summary>
    /// </summary>
    private async void HandleAsyncLoader()
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
                _depsFuture = AsyncExecutor.Submit( this );
            }
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

                if ( _asyncDone )
                {
                    Asset = asyncLoader.LoadSync( _manager,
                                                  AssetDesc.Filepath,
                                                  Resolve( _loader, AssetDesc ),
                                                  AssetDesc.Parameters! );
                }
            }
        }
        else if ( ( _loadFuture == null ) && !_asyncDone )
        {
            _loadFuture = AsyncExecutor.Submit( this );
        }
        else if ( _asyncDone )
        {
            Asset = asyncLoader.LoadSync( _manager,
                                          AssetDesc.Filepath,
                                          Resolve( _loader, AssetDesc ),
                                          AssetDesc.Parameters! );
        }
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
                                          AssetDesc.Filepath,
                                          Resolve( _loader, AssetDesc ),
                                          AssetDesc.Parameters! );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
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
