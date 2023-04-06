using LibGDXSharp.Utils.Async;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Assets
{
    internal class AssetLoadingTask<T> : IAsyncTask
    {
        private AssetManager        _manager;
        private AssetLoader< T, T > _loader;
        private AsyncExecutor       _executor;
        private long                _startTime;

        private volatile List< AssetDescriptor< T > > _dependencies;
        private volatile AsyncResult                  _depsFuture;
        private volatile AsyncResult                  _loadFuture;
        private volatile object                       _asset;

        private volatile bool _asyncDone;
        private volatile bool _dependenciesLoaded;
        private volatile bool _cancel;

        /// <summary>
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="assetDesc"></param>
        /// <param name="loader"></param>
        /// <param name="threadPool"></param>
        public AssetLoadingTask( AssetManager manager,
                                 AssetDescriptor< T > assetDesc,
                                 AssetLoader< T, T > loader,
                                 AsyncExecutor threadPool )
        {
            this._manager        = manager;
            this.AssetDescriptor = assetDesc;
            this._loader         = loader;
            this._executor       = threadPool;
            this._startTime      = manager.Log.GetLevel() == Logger.LogDebug ? TimeUtils.NanoTime() : 0;
        }

        public AssetDescriptor< T > AssetDescriptor { get; set; }

        public bool Cancel
        {
            get => _cancel;
            set => _cancel = value;
        }
        
        public bool Call()
        {
            return false;
        }

        public bool Update()
        {
            return false;
        }

        public void Unload()
        {
        }

        private FileHandle Resolve( AssetLoader< T, T > loader, AssetDescriptor< T > assetDesc )
        {
            return new FileHandle( string.Empty );
        }

        private void HandleSyncLoader()
        {
        }

        private void HandleAsyncLoader()
        {
        }

        private void RemoveDuplicates( List< AssetDescriptor< T > > array )
        {
        }
    }
}
