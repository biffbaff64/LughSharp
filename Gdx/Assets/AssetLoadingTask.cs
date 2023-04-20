using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Utils.Async;

namespace LibGDXSharp.Assets
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public sealed class AssetLoadingTask
    {
        private readonly AssetManager    _manager;
        private readonly AssetLoader     _loader;

        private volatile bool _asyncStarted = false;
        private volatile bool _asyncDone    = false;

        private volatile bool                     _dependenciesStarted;
        private volatile bool                     _dependenciesLoaded;
        private volatile List< AssetDescriptor >? _dependencies;

        private long _startTime;

        public AssetDescriptor AssetDesc { get; set; }
        public bool            Cancel    { get; set; }
        public object?         Asset     { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="assetDesc"></param>
        /// <param name="loader"></param>
        /// <param name="threadPool"></param>
        public AssetLoadingTask( AssetManager manager, AssetDescriptor assetDesc, AssetLoader loader )
        {
            this._manager   = manager;
            this.AssetDesc  = assetDesc;
            this._loader    = loader;
            this._startTime = manager.Log.Level == Logger.LogDebug ? TimeUtils.NanoTime() : 0;
        }

        /// <summary>
        /// Loads parts of the asset asynchronously if the loader is
        /// an <see cref="AsynchronousAssetLoader{T,TP}"/>.
        /// </summary>
        public void Call()
        {
            if ( Cancel ) return;

            var asyncLoader = _loader as AsynchronousAssetLoader;

            if ( !_dependenciesLoaded )
            {
                _dependencies = asyncLoader?.GetDependencies
                    (
                     AssetDesc.FileName,
                     Resolve( asyncLoader, AssetDesc ),
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
                    asyncLoader?.LoadAsync
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
                asyncLoader?.LoadAsync
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
        public void Unload<T>()
        {
            // if ( _loader != null && !_loader.IsSynchronous )
            if ( _loader is { IsSynchronous: false } )
            {
                ( ( AsynchronousAssetLoader< T, AssetLoaderParameters > )_loader ).UnloadAsync
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
        private FileHandle? Resolve( AssetLoader? loader, AssetDescriptor? assetDesc )
        {
            if ( assetDesc is { File: null } )
            {
                if ( loader != null )
                {
                    assetDesc.File = loader.Resolve( assetDesc.FileName );
                }
            }

            return assetDesc?.File;
        }

        /// <summary>
        /// </summary>
        private void HandleSyncLoader()
        {
            var syncLoader = ( SynchronousAssetLoader< T, IAssetLoaderParameters >? )_loader;

            if ( syncLoader == null ) return;

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
                    Asset = syncLoader.Load
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
                Asset = syncLoader.Load
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
            if ( AssetDesc == null ) throw new GdxRuntimeException( "Unable to load asset: AssetDesc is null" );

            var asyncLoader = ( AsynchronousAssetLoader< T, AssetLoaderParameters< T > >? )_loader;

            if ( asyncLoader == null ) throw new GdxRuntimeException( "asyncLoader is null" );

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
                        Asset = asyncLoader.LoadSync
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
                Asset = asyncLoader.LoadSync
                    (
                     _manager,
                     AssetDesc.FileName,
                     Resolve( _loader, AssetDesc ),
                     AssetDesc.Parameters
                    );
            }
            else if ( _loadFuture != null && _loadFuture.IsDone() )
            {
                try
                {
                    _loadFuture.Get();
                }
                catch ( Exception e )
                {
                    throw new GdxRuntimeException( "Couldn't load asset: " + AssetDesc.FileName, e );
                }

                Asset = asyncLoader.LoadSync
                    (
                     _manager,
                     AssetDesc.FileName,
                     Resolve( _loader, AssetDesc ),
                     AssetDesc.Parameters
                    );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        private static void RemoveDuplicates( IList< AssetDescriptor > array )
        {
            for ( var i = 0; i < array.Count; ++i )
            {
                var  fn   = array[ i ].FileName;
                Type type = array[ i ].Type;

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
