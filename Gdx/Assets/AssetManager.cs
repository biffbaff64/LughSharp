using System.Text;

using LibGDXSharp.Core;
using LibGDXSharp.G2D;
using LibGDXSharp.Utils.Async;

namespace LibGDXSharp.Assets
{
    public sealed class AssetManager
    {
        private readonly Dictionary< Type, Dictionary< string, RefCountedContainer > >?                                _assets;
        private readonly Dictionary< string, Type >?                                                                   _assetTypes;
        private readonly Dictionary< string, List< string > >?                                                         _assetDependencies;
        private readonly List< string >?                                                                               _injected;
        private readonly Dictionary< Type, Dictionary< string, AssetLoader< Type, AssetLoaderParameters< Type > > > >? _loaders;
        private readonly List< AssetDescriptor< Type > >?                                                              _loadQueue;
        private readonly AsyncExecutor                                                                                 _executor;
        private          Stack< AssetLoadingTask< AssetLoaderParameters< Type > > >                                    _tasks;

        private IAssetErrorListener _listener;
        private int                 _loaded;
        private int                 _toLoad;
        private int                 _peakTasks;

        private readonly IFileHandleResolver _resolver;

        internal Logger Log { get; private set; }

        /// <summary>
        /// Creates a new AssetManager with all default loaders.
        ///</summary>
        public AssetManager() : this( new InternalFileHandleResolver() )
        {
        }

        /// <summary>
        /// Creates a new AssetManager with optionally all default loaders.
        /// If you don't add the default loaders then you do have to manually add
        /// the loaders you need, including any loaders they might depend on.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="defaultLoaders">Whether to add the default loaders (default is true).</param>
        public AssetManager( IFileHandleResolver resolver, bool defaultLoaders = true )
        {
            this.Log = new Logger( "AssetManager", IApplication.LogNone );

            if ( defaultLoaders )
            {
                //@formatter:off
                SetLoader( typeof(BitmapFont),      new BitmapFontLoader( resolver ) );
                SetLoader( typeof(Texture),         new TextureLoader( resolver ) );
//                SetLoader( typeof(IMusic),          new MusicLoader( resolver ) );
//                SetLoader( typeof(Pixmap),          new PixmapLoader( resolver ) );
//                SetLoader( typeof(ISound),          new SoundLoader( resolver ) );
//                SetLoader( typeof(TextureAtlas),    new TextureAtlasLoader( resolver ) );
//                SetLoader( typeof(Skin),            new SkinLoader( resolver ) );
//                SetLoader( typeof(ParticleEffect),  new ParticleEffectLoader( resolver ) );
//                SetLoader( typeof(PolygonRegion),   new PolygonRegionLoader( resolver ) );
//                SetLoader( typeof(I18NBundle),      new I18NBundleLoader( resolver ) );
//                SetLoader( typeof(ShaderProgram),   new ShaderProgramLoader( resolver ) );
//                SetLoader( typeof(Cubemap),         new CubemapLoader( resolver ) );
                //@formatter:on
            }

            this._resolver = resolver;
            this._executor = new AsyncExecutor( 1, "AssetManager" );
        }

        /// <summary>
        /// Returns the <see cref="IFileHandleResolver"/> for which this AssetManager was loaded with.
        /// </summary>
        /// <returns>the file handle resolver which this AssetManager uses.</returns>
        public IFileHandleResolver GetFileHandleResolver()
        {
            return _resolver;
        }

        /// <summary>
        ///
        /// </summary>
        public T Get<T>( string name )
        {
            T? asset;

            lock ( this )
            {
                Type? type = _assetTypes?[ name ];

                if ( type == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                var assetsByType = _assets?[ type ];

                if ( assetsByType == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                var assetContainer = assetsByType[ name ];

                if ( assetContainer == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                asset = ( T? )assetContainer.Object;

                if ( asset == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );
            }

            return asset;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public T Get<T>( string name, Type type )
        {
            T? asset;

            lock ( this )
            {
                var assetsByType = _assets?[ type ];

                if ( assetsByType == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                RefCountedContainer assetContainer = assetsByType[ name ];

                if ( assetContainer == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                asset = ( T? )assetContainer.Object;

                if ( asset == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );
            }

            return asset;
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outArray"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List< T > GetAll<T>( Type type, List< T > outArray )
        {
            lock ( this )
            {
                var assetsByType = _assets?[ type ];

                if ( assetsByType != null )
                {
                    foreach ( RefCountedContainer? asset in assetsByType.Values )
                    {
                        if ( asset?.Object != null )
                        {
                            outArray.Add( ( T )asset.Object );
                        }
                    }
                }
            }

            return outArray;
        }

        /// <summary>
        /// </summary>
        /// <param name="assetDescriptor"></param>
        /// <returns></returns>
        public T Get<T>( AssetDescriptor< T > assetDescriptor )
        {
            return Get< T >( assetDescriptor.FileName, assetDescriptor.Type );
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Contains( string fileName )
        {
            if ( ( _tasks.Count > 0 )
                 && ( _tasks.First().AssetDescriptor.FileName.Equals( fileName ) ) )
            {
                return true;
            }

            for ( var i = 0; i < _loadQueue?.Count; i++ )
            {
                if ( _loadQueue[ i ].FileName.Equals( fileName ) )
                {
                    return true;
                }
            }

            return IsLoaded( fileName );
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Contains( string fileName, Type type )
        {
            if ( _tasks.Count > 0 )
            {
                var assetDesc = _tasks.First().AssetDescriptor;

                if ( assetDesc.Type == type && assetDesc.FileName.Equals( fileName ) ) return true;
            }

            for ( int i = 0; i < _loadQueue?.Count; i++ )
            {
                var assetDesc = _loadQueue[ i ];

                if ( assetDesc.Type == type && assetDesc.FileName.Equals( fileName ) ) return true;
            }

            return IsLoaded( fileName, type );
        }

        /// <summary>
        /// Removes the asset and all its dependencies, if they are not used by other assets.
        /// </summary>
        /// <param name="fileName"></param> the file name
        public void Unload( string fileName )
        {
            // convert all windows path separators to unix style
            fileName = fileName.Replace( '\\', '/' );

            // check if it's currently processed (and the first element in the
            // stack, thus not a dependency) and cancel if necessary
            if ( _tasks.Count > 0 )
            {
                var currentTask = _tasks.First();

                if ( currentTask.AssetDescriptor.FileName.Equals( fileName ) )
                {
                    Log.Info( "Unload (from tasks): " + fileName );

                    currentTask.Cancel = true;

                    currentTask.Unload();

                    return;
                }
            }

            Type? type = _assetTypes?[ fileName ];

            // check if it's in the queue
            var foundIndex = -1;

            for ( var i = 0; i < _loadQueue?.Count; i++ )
            {
                if ( _loadQueue[ i ].FileName.Equals( fileName ) )
                {
                    foundIndex = i;

                    break;
                }
            }

            if ( foundIndex != -1 )
            {
                _toLoad--;

                var desc = _loadQueue?[ foundIndex ];
                _loadQueue?.RemoveAt( foundIndex );

                Log.Info( "Unload (from queue): " + fileName );

                // if the queued asset was already loaded, let the callback know it is available.
                if ( ( type != null ) && ( desc?.Parameters != null ) && ( desc.Parameters.LoadedCallback != null ) )
                {
                    desc.Parameters.LoadedCallback.FinishedLoading( this, desc.FileName, desc.Type );
                }

                return;
            }

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );

            Dictionary< string, RefCountedContainer >? assetRef = null;

            _assets?.TryGetValue( type, out assetRef );

            // if it is reference counted, decrement ref count and check if we can really get rid of it.
            if ( assetRef != null )
            {
                assetRef[ fileName ].RefCount--;
            }

            if ( assetRef?[ fileName ].RefCount <= 0 )
            {
                Log.Info( "Unload (dispose): " + fileName );

                // if it is disposable dispose it
                if ( assetRef[ fileName ].Object is IDisposable )
                {
                    ( ( IDisposable )assetRef[ fileName ].Object ).Dispose();
                }

                // remove the asset from the manager.
                _assetTypes?.Remove( fileName );
                _assets?[ type ].Remove( fileName );
            }
            else
            {
                Log.Info( "Unload (decrement): " + fileName );
            }

            // remove any dependencies (or just decrement their ref count).
            var dependencies = _assetDependencies?[ fileName ];

            if ( dependencies != null )
            {
                foreach ( var dependency in dependencies )
                {
                    if ( IsLoaded( dependency ) ) Unload( dependency );
                }
            }

            // remove dependencies if ref count < 0
            if ( assetRef?[ fileName ].RefCount <= 0 )
            {
                _assetDependencies?.Remove( fileName );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="asset">the asset</param>
        /// <returns>whether the asset is contained in this manager</returns>
        public bool ContainsAsset<T>( T asset )
        {
            if ( asset == null ) return false;

            var assetsByType = _assets?[ asset.GetType() ];

            if ( assetsByType == null ) return false;

            foreach ( var fileName in assetsByType.Keys )
            {
                var otherAsset = ( T )assetsByType[ fileName ].Object;

                if ( otherAsset?.GetType() == asset.GetType() || asset.Equals( otherAsset ) ) return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="asset">the asset</param>
        /// <returns>the filename of the asset or null</returns>
        public string? GetAssetFileName<T>( T asset )
        {
            if ( _assets == null ) return null;
            if ( asset == null ) return null;

            foreach ( Type assetType in _assets.Keys )
            {
                var assetsByType = _assets[ assetType ];

                foreach ( var fileName in assetsByType.Keys )
                {
                    var otherAsset = ( T )assetsByType[ fileName ].Object;

                    if ( otherAsset?.GetType() == asset.GetType() || asset.Equals( otherAsset ) ) return fileName;
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">the file name of the asset</param>
	    /// <returns>whether the asset is loaded</returns>
        public bool IsLoaded( string? fileName )
        {
            if ( _assetTypes == null ) return false;
            
            return fileName != null && _assetTypes.ContainsKey( fileName );

        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">the file name of the asset</param>
        /// <param name="type"></param>
        /// <returns>whether the asset is loaded</returns>
        public bool IsLoaded( string fileName, Type type )
        {
            var assetsByType = _assets?[ type ];

            if ( assetsByType == null ) return false;
            
            RefCountedContainer assetContainer = assetsByType[ fileName ];

            return assetContainer?.Object != null;
        }

        /// <summary>
        /// Returns the default loader for the given type.
        /// </summary>
        /// <param name="type">The type of the loader to get</param>
        /// <returns>The loader capable of loading the type, or null if none exists.</returns>
        public AssetLoader<T, TP> GetLoader<T, TP>( T type) where TP : AssetLoaderParameters<T>
        {
            return GetLoader( type, null );
        }

        /// <summary>
        /// Returns the loader for the given type and the specified filename. If no loader
        /// exists for the specific filename, the default loader for that type is returned.
        /// </summary>
	    /// <param name="type">The type of the loader to get</param>
	    /// <param name="fileName">The filename of the asset to get a loader for, or null to get the default loader
	    /// @return The loader capable of loading the type and filename, or null if none exists
        public AssetLoader<T, TP> GetLoader<T, TP>( T type, string fileName) where TP : AssetLoaderParameters<T>
        {
            Dictionary< string, AssetLoader<T, TP> > loaders = this._loaders[ type.GetType() ];

            if ( loaders == null || loaders.size < 1 ) return null;
            if ( fileName == null ) return loaders.get( "" );

            AssetLoader result = null;

            int l = -1;

            foreach ( ObjectMap.Entry< string, AssetLoader > entry in loaders.entries() )
            {
                if ( entry.key.length() > l && FileName.endsWith( entry.key ) )
                {
                    result = entry.value;
                    l      = entry.key.length();
                }
            }

            return result;
        }

        /** Adds the given asset to the loading queue of the AssetManager.
    	  * @param fileName the file name (interpretation depends on {@link AssetLoader})
    	  * @param type the type of the asset.
          */
            public void Load( string fileName, Type type )
        {
            Load( fileName, type, null );
        }

        /** Adds the given asset to the loading queue of the AssetManager.
	 * @param fileName the file name (interpretation depends on {@link AssetLoader})
	 * @param type the type of the asset.
	 * @param parameter parameters for the AssetLoader. */
        public void Load( string fileName, Class< T > type, AssetLoaderParameters< T > parameter )
        {
            AssetLoader loader = getLoader( type, fileName );

            if ( loader == null )
                throw new GdxRuntimeException( "No loader for type: " + ClassReflection.getSimpleName( type ) );

            // reset stats
            if ( _loadQueue.size == 0 )
            {
                _loaded    = 0;
                _toLoad    = 0;
                _peakTasks = 0;
            }

            // check if an asset with the same name but a different type has already been added.

            // check preload queue
            for ( int i = 0; i < _loadQueue.size; i++ )
            {
                AssetDescriptor desc = _loadQueue.get( i );

                if ( desc.fileName.equals( fileName ) && !desc.type.equals( type ) )
                    throw new GdxRuntimeException
                        (
                         "Asset with name '"
                         + fileName
                         + "' already in preload queue, but has different type (expected: "
                         + ClassReflection.getSimpleName( type )
                         + ", found: "
                         + ClassReflection.getSimpleName( desc.type )
                         + ")"
                        );
            }

            // check task list
            for ( int i = 0; i < _tasks.size(); i++ )
            {
                AssetDescriptor desc = _tasks.get( i ).assetDesc;

                if ( desc.fileName.equals( fileName ) && !desc.type.equals( type ) )
                    throw new GdxRuntimeException
                        (
                         "Asset with name '"
                         + fileName
                         + "' already in task list, but has different type (expected: "
                         + ClassReflection.getSimpleName( type )
                         + ", found: "
                         + ClassReflection.getSimpleName( desc.type )
                         + ")"
                        );
            }

            // check loaded assets
            Class otherType = _assetTypes.get( fileName );

            if ( otherType != null && !otherType.equals( type ) )
                throw new GdxRuntimeException
                    (
                     "Asset with name '"
                     + fileName
                     + "' already loaded, but has different type (expected: "
                     + ClassReflection.getSimpleName( type )
                     + ", found: "
                     + ClassReflection.getSimpleName( otherType )
                     + ")"
                    );

            _toLoad++;
            AssetDescriptor assetDesc = new AssetDescriptor( fileName, type, parameter );
            _loadQueue.add( assetDesc );
            Log.Debug( "Queued: " + assetDesc );
        }

        /** Adds the given asset to the loading queue of the AssetManager.
	 * @param desc the {@link AssetDescriptor} */
        public void Load( AssetDescriptor desc )
        {
            Load( desc.fileName, desc.type, desc.params );
        }

        /** Updates the AssetManager for a single task. Returns if the current task is still being processed or there are no tasks,
	 * otherwise it finishes the current task and starts the next task.
	 * @return true if all loading is finished. */
        public bool Update()
        {
            try
            {
                if ( _tasks.size() == 0 )
                {
                    // loop until we have a new task ready to be processed
                    while ( _loadQueue.size != 0 && _tasks.size() == 0 )
                        NextTask();

                    // have we not found a task? We are done!
                    if ( _tasks.size() == 0 ) return true;
                }

                return UpdateTask() && _loadQueue.size == 0 && _tasks.size() == 0;
            }
            catch ( Throwable t )
            {
                HandleTaskError( t );

                return _loadQueue.size == 0;
            }
        }

        /** Updates the AssetManager continuously for the specified number of milliseconds, yielding the CPU to the loading thread
	 * between updates. This may block for less time if all loading tasks are complete. This may block for more time if the portion
	 * of a single task that happens in the GL thread takes a long time.
	 * @return true if all loading is finished. */
        public bool Update( int millis )
        {
            long endTime = TimeUtils.millis() + millis;

            while ( true )
            {
                bool done = Update();

                if ( done || TimeUtils.millis() > endTime ) return done;
                ThreadUtils.yield();
            }
        }

        /** Returns true when all assets are loaded. Can be called from any thread but note {@link #update()} or related methods must
	 * be called to process tasks. */
        public bool IsFinished()
        {
            return _loadQueue.size == 0 && _tasks.size() == 0;
        }

        /** Blocks until all assets are loaded. */
        public void FinishLoading()
        {
            Log.Debug( "Waiting for loading to complete..." );

            while ( !Update() )
                ThreadUtils.yield();

            Log.Debug( "Loading complete." );
        }

        /** Blocks until the specified asset is loaded.
	 * @param assetDesc the AssetDescriptor of the asset */
        public <T> T FinishLoadingAsset( AssetDescriptor assetDesc )
        {
            return finishLoadingAsset( assetDesc.fileName );
        }

        /** Blocks until the specified asset is loaded.
	 * @param fileName the file name (interpretation depends on {@link AssetLoader}) */
        public <T> T FinishLoadingAsset( string fileName )
        {
            Log.Debug( "Waiting for asset to be loaded: " + fileName );

            while ( true )
            {
                synchronized( this ) {
                    Class< T > type = _assetTypes.get( fileName );

                    if ( type != null )
                    {
                        ObjectMap< string, RefCountedContainer > assetsByType = _assets.get( type );

                        if ( assetsByType != null )
                        {
                            RefCountedContainer assetContainer = assetsByType.get( fileName );

                            if ( assetContainer != null )
                            {
                                T asset = assetContainer.GetObject( type );

                                if ( asset != null )
                                {
                                    Log.Debug( "Asset loaded: " + fileName );

                                    return asset;
                                }
                            }
                        }
                    }

                    Update();
                }

                ThreadUtils.yield();
            }
        }

        void InjectDependencies( string parentAssetFilename, Array< AssetDescriptor > dependendAssetDescs )
        {
            ObjectSet< string > injected = this._injected;
            for ( AssetDescriptor desc :
            dependendAssetDescs) {
                if ( injected.contains( desc.fileName ) ) continue; // Ignore subsequent dependencies if there are duplicates.
                injected.add( desc.fileName );
                InjectDependency( parentAssetFilename, desc );
            }

            injected.clear( 32 );
        }

        private void InjectDependency( string parentAssetFilename, AssetDescriptor dependendAssetDesc )
        {
            // add the asset as a dependency of the parent asset
            Array< string > dependencies = _assetDependencies.get( parentAssetFilename );

            if ( dependencies == null )
            {
                dependencies = new Array();
                _assetDependencies.put( parentAssetFilename, dependencies );
            }

            dependencies.add( dependendAssetDesc.fileName );

            // if the asset is already loaded, increase its reference count.
            if ( isLoaded( dependendAssetDesc.fileName ) )
            {
                Log.Debug( "Dependency already loaded: " + dependendAssetDesc );
                Class               type     = _assetTypes.get( dependendAssetDesc.fileName );
                RefCountedContainer assetRef = _assets.get( type ).get( dependendAssetDesc.fileName );
                assetRef.IncRefCount();
                IncrementRefCountedDependencies( dependendAssetDesc.fileName );
            }
            else
            {
                // else add a new task for the asset.
                Log.Info( "Loading dependency: " + dependendAssetDesc );
                AddTask( dependendAssetDesc );
            }
        }

        /** Removes a task from the loadQueue and adds it to the task stack. If the asset is already loaded (which can happen if it was
	 * a dependency of a previously loaded asset) its reference count will be increased. */
        private void NextTask()
        {
            AssetDescriptor assetDesc = _loadQueue.removeIndex( 0 );

            // if the asset not meant to be reloaded and is already loaded, increase its reference count
            if ( isLoaded( assetDesc.fileName ) )
            {
                Log.Debug( "Already loaded: " + assetDesc );
                Class               type     = _assetTypes.get( assetDesc.fileName );
                RefCountedContainer assetRef = _assets.get( type ).get( assetDesc.fileName );
                assetRef.IncRefCount();
                IncrementRefCountedDependencies( assetDesc.fileName );
                if ( assetDesc.params != null && assetDesc.params.loadedCallback != null)
                assetDesc.params.loadedCallback.finishedLoading( this, assetDesc.fileName, assetDesc.type );
                _loaded++;
            }
            else
            {
                // else add a new task for the asset.
                Log.Info( "Loading: " + assetDesc );
                AddTask( assetDesc );
            }
        }

        /** Adds a {@link AssetLoadingTask} to the task stack for the given asset. */
        private void AddTask( AssetDescriptor assetDesc )
        {
            AssetLoader loader = getLoader( assetDesc.type, assetDesc.fileName );

            if ( loader == null )
                throw new GdxRuntimeException( "No loader for type: " + ClassReflection.getSimpleName( assetDesc.type ) );

            _tasks.push( new AssetLoadingTask( this, assetDesc, loader, _executor ) );
            _peakTasks++;
        }

        /** Adds an asset to this AssetManager */
        private <T> void AddAsset( final string _fileName, _class< T > type, T Asset) {
            // add the asset to the filename lookup
            _assetTypes.put( fileName, type );

            // add the asset to the type lookup
            ObjectMap< string, RefCountedContainer > typeToAssets = _assets.get( type );

            if ( typeToAssets == null )
            {
                typeToAssets = new ObjectMap< string, RefCountedContainer >();
                _assets.put( type, typeToAssets );
            }

            typeToAssets.put( fileName, new RefCountedContainer( asset ) );
        }

        /** Updates the current task on the top of the task stack.
	 * @return true if the asset is loaded or the task was cancelled. */
        private bool UpdateTask()
        {
            AssetLoadingTask task = _tasks.peek();

            bool complete = true;

            try
            {
                complete = task.cancel || task.update();
            }
            catch ( RuntimeException ex )
            {
                task.cancel = true;
                TaskFailed( task.assetDesc, ex );
            }

            // if the task has been cancelled or has finished loading
            if ( complete )
            {
                // increase the number of loaded assets and pop the task from the stack
                if ( _tasks.size() == 1 )
                {
                    _loaded++;
                    _peakTasks = 0;
                }

                _tasks.pop();

                if ( task.cancel ) return true;

                AddAsset( task.assetDesc.fileName, task.assetDesc.type, task.asset );

                // otherwise, if a listener was found in the parameter invoke it
                if ( task.assetDesc.params != null && task.assetDesc.params.loadedCallback != null)
                task.assetDesc.params.loadedCallback.finishedLoading( this, task.assetDesc.fileName, task.assetDesc.type );

                long endTime = TimeUtils.nanoTime();
                Log.Debug( "Loaded: " + ( endTime - task.startTime ) / 1000000f + "ms " + task.assetDesc );

                return true;
            }

            return false;
        }

        /** Called when a task throws an exception during loading. The default implementation rethrows the exception. A subclass may
	 * supress the default implementation when loading assets where loading failure is recoverable. */
        private void TaskFailed( AssetDescriptor assetDesc, RuntimeException ex )
        {
            throw ex;
        }

        private void IncrementRefCountedDependencies( string parent )
        {
            Array< string > dependencies = _assetDependencies.get( parent );

            if ( dependencies == null ) return;

            for ( string dependency :
            dependencies) {
                Class               type     = _assetTypes.get( dependency );
                RefCountedContainer assetRef = _assets.get( type ).get( dependency );
                assetRef.IncRefCount();
                IncrementRefCountedDependencies( dependency );
            }
        }

        /** Handles a runtime/loading error in {@link #update()} by optionally invoking the {@link AssetErrorListener}.
	 * @param t */
        private void HandleTaskError( Throwable t )
        {
            Log.error( "Error loading asset.", t );

            if ( _tasks.isEmpty() ) throw new GdxRuntimeException( t );

            // pop the faulty task from the stack
            AssetLoadingTask task      = _tasks.pop();
            AssetDescriptor  assetDesc = task.assetDesc;

            // remove all dependencies
            if ( task.dependenciesLoaded && task.dependencies != null )
            {
                foreach ( AssetDescriptor desc in task.dependencies )
                {
                    Unload( desc.fileName );
                }
            }

            // clear the rest of the stack
            _tasks.Clear();

            // inform the listener that something bad happened
            if ( _listener != null )
            {
                _listener.Error( assetDesc, t );
            }
            else
            {
                throw new GdxRuntimeException( t );
            }
        }

        /// <summary>
        /// Sets a new <see cref="AssetLoader{T,TP}"/> for the given type.
        /// </summary>
        /// <param name="type"> the type of the asset </param>
        /// <param name="loader"> the loader  </param>
        public void SetLoader<T, TP>( Type type, AssetLoader< T, TP > loader ) where TP : AssetLoaderParameters< T >
        {
            lock ( this )
            {
                SetLoader( type, null, loader );
            }
        }

        /// <summary>
        /// Sets a new <see cref="AssetLoader{T,TP}"/> for the given type.
        /// </summary>
        /// <param name="type"> the type of the asset </param>
        /// <param name="suffix">
        /// the suffix the filename must have for this loader to be used or null
        /// to specify the default loader.
        /// </param>
        /// <param name="loader"> the loader  </param>
        public void SetLoader<T, P>( Type type, string suffix, AssetLoader< T, P > loader ) where P : AssetLoaderParameters< T >
        {
            lock ( this )
            {
                if ( type == null ) throw new ArgumentException( "type cannot be null." );
                if ( loader == null ) throw new ArgumentException( "loader cannot be null." );

                Log.Debug( "Loader set: " + type.Name + " -> " + loader.GetType().Name );

                ObjectMap< string, AssetLoader > loaders = this._loaders.Get( type );

                if ( loaders == null )
                {
                    this.loaders.put( type, loaders = new ObjectMap< string, AssetLoader >() );
                }

                loaders.put( string.ReferenceEquals( suffix, null ) ? "" : suffix, loader );
            }
        }

        /** @return the number of loaded assets */
        public int GetLoadedAssetsCount()
        {
            return _assetTypes.Count;
        }

        /** @return the number of currently queued assets */
        public int GetQueuedAssets()
        {
            return _loadQueue.size + _tasks.size();
        }

        /** @return the progress in percent of completion. */
        public float GetProgress()
        {
            if ( _toLoad == 0 ) return 1;
            float fractionalLoaded = _loaded;

            if ( _peakTasks > 0 )
            {
                fractionalLoaded += ( ( _peakTasks - _tasks.size() ) / ( float )_peakTasks );
            }

            return Math.min( 1, fractionalLoaded / _toLoad );
        }

        /** Sets an {@link AssetErrorListener} to be invoked in case loading an asset failed.
	 * @param listener the listener or null */
        public void SetErrorListener( AssetErrorListener listener )
        {
            this._listener = listener;
        }

        /** Disposes all assets in the manager and stops all asynchronous loading. */
        @Override

        public void Dispose()
        {
            Log.Debug( "Disposing." );
            Clear();
            _executor.dispose();
        }

        /** Clears and disposes all assets and the preloading queue. */
        public void Clear()
        {
            _loadQueue.Clear();

            while ( !Update() )
                ;

            ObjectIntMap< string > dependencyCount = new ObjectIntMap< string >();

            while ( _assetTypes.size > 0 )
            {
                // for each asset, figure out how often it was referenced
                dependencyCount.Clear();
                Array< string > assets = _assetTypes.keys().toArray();
                for ( string asset :
                assets)
                dependencyCount.put( Asset, 0 );

                for ( string asset :
                assets) {
                    Array< string > dependencies = _assetDependencies.get( Asset );

                    if ( dependencies == null ) continue;
                    for ( string dependency :
                    dependencies) {
                        int count = dependencyCount.get( dependency, 0 );
                        count++;
                        dependencyCount.put( dependency, count );
                    }
                }

                // only dispose of assets that are root assets (not referenced)
                foreach ( string asset in assets )
                {
                    if ( dependencyCount.get( Asset, 0 ) == 0 ) Unload( Asset );
                }
            }

            this._assets.Clear();
            this._assetTypes.Clear();
            this._assetDependencies.Clear();
            this._loaded    = 0;
            this._toLoad    = 0;
            this._peakTasks = 0;
            this._loadQueue.Clear();
            this._tasks.Clear();
        }

        /// <summary>
        /// Returns the <see cref="Logger"/> used by the <see cref="AssetManager"/>
        /// </summary>
        public Logger GetLogger()
        {
            return Log;
        }

        public void SetLogger( Logger logger )
        {
            Log = logger;
        }

        /** Returns the reference count of an asset.
	 * @param fileName */
        public int GetReferenceCount( string fileName )
        {
            Class type = _assetTypes.get( fileName );

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );

            return _assets.get( type ).get( fileName ).getRefCount();
        }

        /// <summary>
        /// Sets the reference count of an asset.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="refCount"></param>
        public void SetReferenceCount( string fileName, int refCount )
        {
            Type type = _assetTypes.get( fileName );

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );

            _assets.get( type ).get( fileName ).setRefCount( refCount );
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// a string containing ref count and dependency information for all assets.
        /// </returns>
        public string GetDiagnostics()
        {
            lock ( this )
            {
                var sb = new StringBuilder( 256 );

                foreach ( var fileName in _assetTypes.Keys )
                {
                    if ( sb.Length > 0 )
                    {
                        sb.Append( '\n' );
                    }

                    sb.Append( FileName );
                    sb.Append( ", " );

                    Type type = _assetTypes[ fileName ];

                    RefCountedContainer assetRef = _assets.Values[ fileName ];

                    List< string > dependencies = _assetDependencies.Get( FileName );

                    sb.Append( ClassReflection.GetSimpleName( type ) );

                    sb.Append( ", refs: " );
                    sb.Append( assetRef.GetRefCount() );

                    if ( dependencies != null )
                    {
                        sb.Append( ", deps: [" );

                        foreach ( string dep in dependencies )
                        {
                            sb.Append( dep );
                            sb.Append( ',' );
                        }

                        sb.Append( ']' );
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public List< string > GetAssetNames()
        {
            lock ( this )
            {
                return _assetTypes.Keys.ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List< string >? GetDependencies( string name )
        {
            lock ( this )
            {
                var array = _assetDependencies[ name ];

                return array;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Type? GetAssetType( string name )
        {
            lock ( this )
            {
                Type? type = _assetTypes[ name ];

                return type;
            }
        }
    }
}
