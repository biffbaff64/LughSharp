using LibGDXSharp.Utils.Async;

namespace LibGDXSharp.Assets
{
    internal class AssetLoadingTask<T> : IAsyncTask
    {
        private AssetManager                                 _manager;
        private AssetLoader< T, AssetLoaderParameters< T > > _loader;
        private AsyncExecutor                                _executor;
        private long                                         _startTime;

        private volatile List< AssetDescriptor< T > >? _dependencies;
        private volatile AsyncResult?                  _depsFuture;
        private volatile AsyncResult?                  _loadFuture;
        private volatile object?                       _asset;

        private volatile bool _asyncDone;
        private volatile bool _dependenciesLoaded;

        /// <summary>
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="assetDesc"></param>
        /// <param name="loader"></param>
        /// <param name="threadPool"></param>
        public AssetLoadingTask( AssetManager manager,
                                 AssetDescriptor< T > assetDesc,
                                 AssetLoader< T, AssetLoaderParameters< T > > loader,
                                 AsyncExecutor threadPool )
        {
            this._manager   = manager;
            this.AssetDesc  = assetDesc;
            this._loader    = loader;
            this._executor  = threadPool;
            this._startTime = manager.Log.Level == Logger.LogDebug ? TimeUtils.NanoTime() : 0;
        }

        public AssetDescriptor< T > AssetDesc { get; set; }

        public bool Cancel { get; set; }

        public virtual void Call()
        {
            if ( Cancel ) return;

            var asyncLoader = ( AsynchronousAssetLoader< T, AssetLoaderParameters< T > > )_loader;

            if ( !_dependenciesLoaded )
            {
                _dependencies = asyncLoader.GetDependencies
                    (
                     AssetDesc.FileName,
                     Resolve( _loader, AssetDesc ),
                     AssetDesc.Parameters
                    );

                if ( _dependencies != null )
                {
                    RemoveDuplicates( _dependencies );
                    _manager.InjectDependencies( AssetDesc.FileName, _dependencies );
                }
                else
                {
                    // if we have no dependencies, we load the async part of the task immediately.
                    asyncLoader.LoadAsync
                        (
                         _manager,
                         AssetDesc.FileName,
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
                     AssetDesc.FileName,
                     Resolve( _loader, AssetDesc ),
                     AssetDesc.Parameters
                    );

                _asyncDone = true;
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
            if ( _loader.IsSynchronous )
            {
                HandleSyncLoader();
            }
            else
            {
                HandleAsyncLoader();
            }

            return _asset != null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unload()
        {
            if ( !_loader.IsSynchronous )
            {
                ( ( AsynchronousAssetLoader< T, AssetLoaderParameters< T > > )_loader ).UnloadAsync
                    (
                     _manager,
                     AssetDesc.FileName,
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
        private FileHandle? Resolve( AssetLoader< T, AssetLoaderParameters< T > > loader,
                                     AssetDescriptor< T > assetDesc )
        {
            if ( AssetDesc.File == null )
            {
                AssetDesc.File = loader.Resolve( assetDesc.FileName );
            }

            return assetDesc.File;
        }

        /// <summary>
        /// </summary>
        private void HandleSyncLoader()
        {
            var syncLoader = ( SynchronousAssetLoader< T, AssetLoaderParameters< T > > )_loader;

            if ( !_dependenciesLoaded )
            {
                _dependenciesLoaded = true;

                _dependencies = syncLoader.GetDependencies
                    (
                     AssetDesc.FileName,
                     Resolve( _loader, AssetDesc ),
                     AssetDesc.Parameters
                    );

                if ( _dependencies == null )
                {
                    _asset = syncLoader.Load
                        (
                         _manager,
                         AssetDesc.FileName,
                         Resolve( _loader, AssetDesc ),
                         AssetDesc.Parameters
                        );

                    return;
                }

                RemoveDuplicates( _dependencies );
                _manager.InjectDependencies( AssetDesc.FileName, _dependencies );
            }
            else
            {
                _asset = syncLoader.Load
                    (
                     _manager,
                     AssetDesc.FileName,
                     Resolve( _loader, AssetDesc ),
                     AssetDesc.Parameters
                    );
            }
        }

        /// <summary>
        /// </summary>
        private void HandleAsyncLoader()
        {
            var asyncLoader = ( AsynchronousAssetLoader< T, AssetLoaderParameters< T > > )_loader;

            if ( !_dependenciesLoaded )
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
                    catch ( Exception e )
                    {
                        throw new GdxRuntimeException
                            ( "Couldn't load dependencies of asset: " + AssetDesc.FileName, e );
                    }

                    _dependenciesLoaded = true;

                    if ( _asyncDone )
                    {
                        _asset = asyncLoader.LoadSync
                            (
                             _manager,
                             AssetDesc.FileName,
                             Resolve( _loader, AssetDesc ),
                             AssetDesc.Parameters
                            );
                    }
                }
            }
            else if ( _loadFuture == null && !_asyncDone )
            {
                _loadFuture = _executor.Submit( this );
            }
            else if ( _asyncDone )
            {
                _asset = asyncLoader.LoadSync
                    (
                     _manager,
                     AssetDesc.FileName,
                     Resolve( _loader, AssetDesc ),
                     AssetDesc.Parameters
                    );
            }
            else if ( _loadFuture.IsDone() )
            {
                try
                {
                    _loadFuture.Get();
                }
                catch ( Exception e )
                {
                    throw new GdxRuntimeException( "Couldn't load asset: " + AssetDesc.FileName, e );
                }

                _asset = asyncLoader.LoadSync
                    (
                     _manager,
                     AssetDesc.FileName,
                     Resolve( _loader, AssetDesc ),
                     AssetDesc.Parameters
                    );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        private void RemoveDuplicates( List< AssetDescriptor< T > > array )
        {
            for ( var i = 0; i < array.Count; ++i )
            {
                string fn   = array[ i ].FileName;
                Type   type = array[ i ].Type;

                for ( var j = array.Count - 1; j > i; --j )
                {
                    if ( type == array[ j ].Type && fn.Equals( array[ j ].FileName ) )
                    {
                        array.RemoveAt( j );
                    }
                }
            }
        }
    }
}
