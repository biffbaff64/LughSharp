using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using LibGDXSharp.G2D;
using LibGDXSharp.Utils.Async;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Assets
{
    /// <summary>
    /// Loads and stores assets like textures, bitmapfonts, tile maps, sounds, music and so on.
    /// </summary>
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public sealed class AssetManager
    {
        /// <summary>
        /// </summary>
        private readonly Dictionary< Type, Dictionary< string, RefCountedContainer > > _assets =
            new Dictionary< Type, Dictionary< string, RefCountedContainer > >();

        /// <summary>
        /// </summary>
        private readonly Dictionary< string, List< string > >? _assetDependencies =
            new Dictionary< string, List< string > >();

        private readonly Stack< AssetLoadingTask >       _tasks      = new Stack< AssetLoadingTask >();
        private readonly Dictionary< Type, AssetLoader > _loaders    = new Dictionary< Type, AssetLoader >();
        private readonly Dictionary< string, Type? >     _assetTypes = new Dictionary< string, Type? >();
        private readonly List< AssetDescriptor >         _loadQueue  = new List< AssetDescriptor >();
        private readonly List< string >                  _injected   = new List< string >();

        private readonly AsyncExecutor?       _executor;
        private          IAssetErrorListener? _listener;

        private int _loaded;
        private int _toLoad;
        private int _peakTasks;

        private readonly IFileHandleResolver _resolver;

        internal Logger Log { get; }

        /// <summary>
        /// Creates a new AssetManager with all default loaders.
        ///</summary>
        public AssetManager() : this( new InternalFileHandleResolver() )
        {
        }

        /// <summary>
        /// Creates a new AssetManager with optionally all default loaders. If you don't add the
        /// default loaders then you do have to manually add the loaders you need, including any
        /// loaders they might depend on.
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
//                SetLoader( typeof(TextureAtlas),    new TextureAtlasLoader( resolver ) );
//                SetLoader( typeof(Pixmap),          new PixmapLoader( resolver ) );
//                SetLoader( typeof(Skin),            new SkinLoader( resolver ) );
//                SetLoader( typeof(IMusic),          new MusicLoader( resolver ) );
//                SetLoader( typeof(ISound),          new SoundLoader( resolver ) );

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
                Type? type = _assetTypes[ name ];

                if ( type == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                var assetsByType = _assets[ type ];

                if ( assetsByType == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                RefCountedContainer? assetContainer = assetsByType[ name ];

                if ( assetContainer == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                asset = ( T? )assetContainer.GetObject();

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
                var assetsByType = _assets[ type ];

                if ( assetsByType == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                RefCountedContainer? assetContainer = assetsByType[ name ];

                if ( assetContainer == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );

                asset = ( T? )assetContainer.GetObject();

                if ( asset == null ) throw new GdxRuntimeException( "Asset not loaded - " + name );
            }

            return asset;
        }

        /// <summary>
        /// </summary>
        /// <param name="assetDescriptor"></param>
        /// <returns></returns>
        public T Get<T>( AssetDescriptor assetDescriptor )
        {
            return Get< T >( assetDescriptor.FileName, assetDescriptor.Type );
        }

        /// <summary>
        /// Returns all assets matching the requested type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outArray"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List< T > GetAll<T>( Type type, List< T > outArray )
        {
            lock ( this )
            {
                var assetsByType = _assets[ type ];

                Debug.Assert( assetsByType != null, nameof( assetsByType ) + " != null" );

                foreach ( RefCountedContainer? asset in assetsByType.Values )
                {
                    var obj = asset.GetObject();

                    if ( obj != null )
                    {
                        outArray.Add( ( T )obj );
                    }
                }
            }

            return outArray;
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Contains( string fileName )
        {
            if ( _tasks.First().AssetDesc is { } assetDescriptor
                 && ( _tasks.Count > 0 )
                 && ( assetDescriptor.FileName.Equals( fileName ) ) )
            {
                return true;
            }

            foreach ( var t in _loadQueue )
            {
                if ( t.FileName.Equals( fileName ) )
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
        public bool Contains<T>( string fileName, Type type )
        {
            if ( _tasks.Count > 0 )
            {
                if ( _tasks.First().AssetDesc is { } assetDesc
                     && assetDesc.Type == type
                     && assetDesc.FileName.Equals( fileName ) )
                {
                    return true;
                }
            }

            foreach ( var assetDesc in _loadQueue )
            {
                if ( assetDesc.Type == type && assetDesc.FileName.Equals( fileName ) )
                {
                    return true;
                }
            }

            return IsLoaded< T >( fileName, type );
        }

        /// <summary>
        /// Removes the asset and all its dependencies, if they are not used by other assets.
        /// </summary>
        /// <param name="fileName"> the asset file name</param>
        public void Unload( string fileName )
        {
            // convert all windows path separators to unix style
            fileName = fileName.Replace( '\\', '/' );

            // check if it's currently processed (and the first element in the
            // stack, thus not a dependency) and cancel if necessary
            if ( _tasks.Count > 0 )
            {
                AssetLoadingTask currentTask = _tasks.First();

                if ( currentTask.AssetDesc.FileName.Equals( fileName ) )
                {
                    Log.Info( "Unload (from tasks): " + fileName );

                    currentTask.Cancel = true;
                    currentTask.Unload();

                    return;
                }
            }

            Type? type = _assetTypes[ fileName ];

            // check if it's in the queue
            var foundIndex = -1;

            for ( var i = 0; i < _loadQueue.Count; i++ )
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

                AssetDescriptor desc = _loadQueue[ foundIndex ];

                _loadQueue.RemoveAt( foundIndex );

                Log.Info( "Unload (from queue): " + fileName );

                // if the queued asset was already loaded, let the callback know it is available.
                if ( ( type != null ) && desc.Parameters is { LoadedCallBack: not null } )
                {
                    desc.Parameters.LoadedCallBack?.FinishedLoading( this, desc.FileName, desc.Type );
                }

                return;
            }

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );

            _assets.TryGetValue( type, out var assetRef );

            // if it is reference counted, decrement ref count and check if we can really get rid of it.
            if ( assetRef != null )
            {
                assetRef[ fileName ].RefCount--;
            }

            if ( assetRef?[ fileName ].RefCount <= 0 )
            {
                Log.Info( "Unload (dispose): " + fileName );

                if ( assetRef[ fileName ].GetObject() is IDisposable )
                {
                    ( ( IDisposable )assetRef[ fileName ].GetObject()! ).Dispose();
                }

                // remove the asset from the manager.
                _assetTypes.Remove( fileName );
                _assets[ type ].Remove( fileName );
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

            var assetsByType = _assets[ asset.GetType() ];

//            if ( assetsByType == null ) return false;

            foreach ( var fileName in assetsByType.Keys )
            {
                var otherAsset = ( T? )assetsByType[ fileName ].GetObject();

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
            if ( asset == null ) return null;

            foreach ( Type? assetType in _assets.Keys )
            {
                var assetsByType = _assets[ assetType ];

//                if ( assetsByType != null )
                {
                    foreach ( var fileName in assetsByType.Keys )
                    {
                        var otherAsset = ( T? )assetsByType[ fileName ].GetObject();

                        if ( ( otherAsset?.GetType() == asset.GetType() ) || asset.Equals( otherAsset ) )
                        {
                            return fileName;
                        }
                    }
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
            return fileName != null && _assetTypes.ContainsKey( fileName );
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">the file name of the asset</param>
        /// <param name="type"></param>
        /// <returns>whether the asset is loaded</returns>
        public bool IsLoaded<T>( string fileName, Type type )
        {
            var assetsByType = _assets[ type ];

            RefCountedContainer? assetContainer = assetsByType?[ fileName ];

            return assetContainer != null && assetContainer.GetObject() != null;
        }

        /// <summary>
        /// Returns the loader for the given type and the specified filename.
        /// </summary>
        /// <param name="type">The type of the loader to get</param>
        /// <param name="fileName">
        /// The filename of the asset to get a loader for, or null to get the default loader.
        /// </param>
        /// <returns>
        /// The loader capable of loading the type and filename, or null if none exists.
        /// </returns>
        public AssetLoader? GetLoader<T>( T? type, string? fileName = null )
        {
            AssetLoader loaders = this._loaders[ type!.GetType() ];

            if ( fileName == null ) return null;

            AssetLoader? result = null;

            var length = -1;

            foreach ( var entry in loaders.Entries() )
            {
                if ( ( entry.key.length() > length ) && fileName.EndsWith( entry.key ) )
                {
                    result = entry.value;
                    length = entry.key.length();
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the given asset to the loading queue of the AssetManager.
        /// </summary>
        /// <param name="fileName">
        /// the file name (interpretation depends on <see cref="AssetLoader"/>)
        /// </param>
        /// <param name="type">the type of the asset.</param>
        /// <param name="parameter"></param>
        public void Load( string fileName, Type? type, IAssetLoaderParameters parameter )
        {
            if ( type == null ) throw new NullReferenceException();

            AssetLoader? loader = GetLoader( type, fileName );

            if ( loader == null ) throw new GdxRuntimeException( "No loader for type: " + type.Name );

            if ( _loadQueue?.Count == 0 )
            {
                // reset stats
                _loaded    = 0;
                _toLoad    = 0;
                _peakTasks = 0;
            }

            // check if an asset with the same name but a different type has already been added.

            // check preload queue
            for ( var i = 0; i < _loadQueue?.Count; i++ )
            {
                var desc = _loadQueue[ i ];

                if ( desc.FileName.Equals( fileName ) && desc.Type != type )
                {
                    throw new GdxRuntimeException
                        (
                         $"Asset with name '{fileName}'"
                         + $"' already in preload queue, but has different type (expected: {type.Name}"
                         + ", found: {desc.Type?.Name})"
                        );
                }
            }

            // check task list
            for ( var i = 0; i < _tasks.Count; i++ )
            {
                AssetDescriptor desc = _tasks.ElementAt( i ).AssetDesc;

                if ( desc.FileName.Equals( fileName ) && desc.Type != type )
                {
                    throw new GdxRuntimeException
                        (
                         $"Asset with name '{fileName}'"
                         + $"' already in preload queue, but has different type (expected: {type.Name}"
                         + ", found: {desc.Type?.Name})"
                        );
                }
            }

            // check loaded assets
            Type? otherType = _assetTypes[ fileName ];

            if ( otherType != null && otherType != type )
            {
                throw new GdxRuntimeException
                    (
                     $"Asset with name '{fileName}' already loaded, but has"
                     + $"different type (expected: {type?.Name}, found: {otherType.Name})"
                    );
            }

            _toLoad++;

            var assetDesc = new AssetDescriptor
            {
                FileName   = fileName,
                Type       = type,
                Parameters = parameter!
            };

            _loadQueue?.Add( assetDesc );

            Log.Debug( "Queued: " + assetDesc );
        }

        /// <summary>
        /// Adds the given asset to the loading queue of the AssetManager.
        /// </summary>
        /// <param name="desc">the <see cref="AssetDescriptor"/></param>
        public void Load( AssetDescriptor desc )
        {
            Load( desc.FileName, desc.Type, desc.Parameters );
        }

        /// <summary>
        /// Updates the AssetManager for a single task. Returns if the current task is
        /// still being processed or there are no tasks, otherwise it finishes the current
        /// task and starts the next task.
        /// </summary>
        /// <returns>true if all loading is finished.</returns>
        public bool Update()
        {
            try
            {
                if ( _tasks.Count == 0 )
                {
                    // loop until we have a new task ready to be processed
                    while ( _loadQueue.Count != 0 && _tasks.Count == 0 )
                    {
                        NextTask();
                    }

                    // have we not found a task? We are done!
                    if ( _tasks.Count == 0 ) return true;
                }

                return UpdateTask() && _loadQueue.Count == 0 && _tasks.Count == 0;
            }
            catch ( Exception t )
            {
                HandleTaskError( t );

                return _loadQueue.Count == 0;
            }
        }

        /// <summary>
        /// Updates the AssetManager continuously for the specified number of milliseconds,
        /// yielding the CPU to the loading thread between updates. This may block for less
        /// time if all loading tasks are complete. This may block for more time if the portion
        /// of a single task that happens in the GL thread takes a long time.
        /// </summary>
        /// <returns> true if all loading is finished.</returns>
        public bool Update( int millis )
        {
            var endTime = TimeUtils.Millis() + millis;

            while ( true )
            {
                var done = Update();

                if ( done || TimeUtils.Millis() > endTime )
                {
                    return done;
                }

                ThreadUtils.Yield();
            }
        }

        /// <summary>
        /// Returns true when all assets are loaded. Can be called from any thread but
        /// note <see cref="Update()"/> or related methods must be called to process tasks. 
        /// </summary>
        public bool IsFinished()
        {
            return _loadQueue.Count == 0 && _tasks.Count == 0;
        }

        /// <summary>
        /// Blocks until all assets are loaded.
        /// </summary>
        public void FinishLoading()
        {
            Log.Debug( "Waiting for loading to complete..." );

            while ( !Update() )
            {
                ThreadUtils.Yield();
            }

            Log.Debug( "Loading complete." );
        }

        /// <summary>
        /// Blocks until the specified asset is loaded.
        /// </summary>
        /// <param name="assetDesc">the AssetDescriptor of the asset</param>
        public T FinishLoadingAsset<T>( AssetDescriptor assetDesc )
        {
            return FinishLoadingAsset< T >( assetDesc.FileName );
        }

        /// <summary>
        /// Blocks until the specified asset is loaded.
        /// </summary>
        /// <param name="fileName">
        /// the file name (interpretation depends on <see cref="AssetLoader"/>)
        /// </param>
        public T FinishLoadingAsset<T>( string fileName )
        {
            Log.Debug( "Waiting for asset to be loaded: " + fileName );

            while ( true )
            {
                lock ( this )
                {
                    Type? type = _assetTypes[ fileName ];

                    if ( type != null )
                    {
                        var assetsByType = _assets[ type ];

                        if ( assetsByType?[ fileName ] is { } assetContainer )
                        {
                            var asset = ( T? )assetContainer.GetObject();

                            if ( asset != null )
                            {
                                Log.Debug( "Asset loaded: " + fileName );

                                return asset;
                            }
                        }
                    }

                    Update();
                }

                ThreadUtils.Yield();
            }
        }

        public void InjectDependencies( string parentAssetFilename, IList< AssetDescriptor > dependendAssetDescs )
        {
            var injected = this._injected;

            foreach ( AssetDescriptor desc in dependendAssetDescs )
            {
                if ( injected.Contains( desc.FileName ) )
                {
                    continue; // Ignore subsequent dependencies if there are duplicates.
                }

                injected.Add( desc.FileName );

                InjectDependency( parentAssetFilename, desc );
            }

            injected.Clear();
        }

        private void InjectDependency( string parentAssetFilename, AssetDescriptor dependendAssetDesc )
        {
            // add the asset as a dependency of the parent asset
            var dependencies = _assetDependencies?[ parentAssetFilename ];

            if ( dependencies == null )
            {
                dependencies = new List< string >();

                _assetDependencies?.Put(parentAssetFilename, dependencies );
            }

            dependencies.Add( dependendAssetDesc.FileName );

            // if the asset is already loaded, increase its reference count.
            if ( IsLoaded( dependendAssetDesc.FileName ) )
            {
                Log.Debug( "Dependency already loaded: " + dependendAssetDesc );

                Type? type = _assetTypes[ dependendAssetDesc.FileName ];

                if ( type == null ) throw new GdxRuntimeException( "type cannot be null!" );

                var assetsByType = _assets[ type ];

                RefCountedContainer assetRef = assetsByType[ dependendAssetDesc.FileName ];

                assetRef.RefCount++;

                IncrementRefCountedDependencies( dependendAssetDesc.FileName );
            }
            else
            {
                // else add a new task for the asset.
                Log.Info( "Loading dependency: " + dependendAssetDesc );

                AddTask( dependendAssetDesc );
            }
        }

        /// <summary>
        /// Removes a task from the loadQueue and adds it to the task stack.
        /// If the asset is already loaded (which can happen if it was a
        /// dependency of a previously loaded asset) its reference count will
        /// be increased. 
        /// </summary>
        private void NextTask()
        {
            var assetDesc = _loadQueue.RemoveIndex( 0 );

            // if the asset not meant to be reloaded and is already loaded, increase its reference count
            if ( IsLoaded( assetDesc.FileName ) )
            {
                Log.Debug( "Already loaded: " + assetDesc );

                Type? type = _assetTypes[ assetDesc.FileName ];

                if ( type == null ) throw new GdxRuntimeException( "type cannot be null!" );

                var assetsByType = _assets[ type ];

                RefCountedContainer assetRef = assetsByType[ assetDesc.FileName ];

                assetRef.RefCount++;

                IncrementRefCountedDependencies( assetDesc.FileName );

//                if ( assetDesc.Parameters != null && assetDesc.Parameters.LoadedCallback != null )
                if ( assetDesc.Parameters is { LoadedCallBack: not null } )
                {
                    assetDesc.Parameters.LoadedCallBack.FinishedLoading
                        (
                         this,
                         assetDesc.FileName,
                         assetDesc.Type
                        );
                }

                _loaded++;
            }
            else
            {
                // else add a new task for the asset.
                Log.Info( "Loading: " + assetDesc );

                AddTask( assetDesc );
            }
        }


        /// <summary>
        /// Adds a <see cref="AssetLoadingTask"/> to the task stack for the given asset.
        /// </summary>
        private void AddTask( AssetDescriptor assetDesc )
        {
            var loader = GetLoader( assetDesc.Type, assetDesc.FileName );

            if ( loader == null )
            {
                throw new GdxRuntimeException( "No loader for type: {assetDesc.type}" );
            }

            _tasks.Push( new AssetLoadingTask( this, assetDesc, loader ) );

            _peakTasks++;
        }

        /// <summary>
        /// Adds an asset to this AssetManager.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="asset"></param>
        /// <typeparam name="T"></typeparam>
        private void AddAsset<T>( string fileName, Type type, T asset )
        {
            if ( asset == null ) throw new GdxRuntimeException( "No asset to add: null" );

            // add the asset to the filename lookup
            _assetTypes[ fileName ] = type;

            // add the asset to the type lookup
            var typeToAssets = _assets[ type ];

            if ( typeToAssets == null )
            {
                typeToAssets = new Dictionary< string, RefCountedContainer >();

                _assets[ type ] = typeToAssets;
            }

            typeToAssets[ fileName ] = new RefCountedContainer( asset );
        }

        /// <summary>
        /// Updates the current task on the top of the task stack.
        /// </summary>
        /// <returns>true if the asset is loaded or the task was cancelled.</returns>
        private bool UpdateTask()
        {
            var task = _tasks.Peek();

            var complete = true;

            try
            {
                complete = task.Cancel || task.Update();
            }
            catch ( SystemException ex )
            {
                task.Cancel = true;

                TaskFailed( task.AssetDesc, ex );
            }

            // if the task has been cancelled or has finished loading
            if ( complete )
            {
                // increase the number of loaded assets and pop the task from the stack
                if ( _tasks.Count == 1 )
                {
                    _loaded++;
                    _peakTasks = 0;
                }

                _tasks.Pop();

                if ( task.Cancel ) return true;

                AddAsset( task.AssetDesc.FileName, task.AssetDesc.Type, task.Asset );

                // otherwise, if a listener was found in the parameter invoke it
                if ( task.AssetDesc.Parameters is { LoadedCallBack: not null } )
                {
                    task.AssetDesc.Parameters.LoadedCallBack.FinishedLoading
                        (
                         this,
                         task.AssetDesc.FileName,
                         task.AssetDesc.Type
                        );
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Called when a task throws an exception during loading. The default implementation
        /// rethrows the exception. A subclass may supress the default implementation when
        /// loading assets where loading failure is recoverable.
        /// </summary>
        private void TaskFailed( AssetDescriptor assetDesc, System.Exception ex )
        {
            throw ex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        private void IncrementRefCountedDependencies( string parent )
        {
            var dependencies = _assetDependencies[ parent ];

            if ( dependencies == null ) return;

            foreach ( string dependency in dependencies )
            {
                Type? type = _assetTypes[ dependency ];

                if ( type == null ) throw new GdxRuntimeException( "type cannot be null!" );

                var assetsByType = _assets[ type ];

                RefCountedContainer assetRef = assetsByType[ dependency ];

                assetRef.RefCount++;

                IncrementRefCountedDependencies( dependency );
            }
        }

        /// <summary>
        /// Handles a runtime/loading error in <see cref="Update()"/> by optionally
        /// invoking the <see cref="IAssetErrorListener"/>.
        /// </summary>
        /// <param name="t"></param>
        private void HandleTaskError( Exception t )
        {
            Log.Error( "Error loading asset.", t );

            if ( _tasks.Count == 0 ) throw new GdxRuntimeException( t );

            // pop the faulty task from the stack
            AssetLoadingTask task      = _tasks.Pop();
            AssetDescriptor  assetDesc = task.AssetDesc;

            // remove all dependencies
            if ( task.DependenciesLoaded && task.Dependencies != null )
            {
                foreach ( var desc in task.Dependencies )
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
        /// Sets a new <see cref="AssetLoader"/> for the given type.
        /// </summary>
        /// <param name="type"> the type of the asset </param>
        /// <param name="loader"> the loader  </param>
        public void SetLoader( Type type, AssetLoader loader )
        {
            lock ( this )
            {
                SetLoader( type, null, loader );
            }
        }

        /// <summary>
        /// Sets a new <see cref="AssetLoader"/> for the given type.
        /// </summary>
        /// <param name="type"> the type of the asset </param>
        /// <param name="suffix">
        /// the suffix the filename must have for this loader to be used or null
        /// to specify the default loader.
        /// </param>
        /// <param name="loader"> the loader</param>
        public void SetLoader( Type type, string? suffix, AssetLoader loader )
        {
            lock ( this )
            {
                if ( type == null ) throw new ArgumentException( "type cannot be null." );
                if ( loader == null ) throw new ArgumentException( "loader cannot be null." );

                var loaders = this._loaders[ type ];

                _loaders.Put( type, loader );
            }
        }

        /// <returns>The number of loaded assets.</returns>
        public int LoadedAssetsCount => _assetTypes.Count;

        /// <returns>the number of currently queued assets.</returns>
        public int GetQueuedAssets() => _loadQueue.Count + _tasks.Count;

        /// <summary>
        /// </summary>
        /// <returns>the progress in percent of completion.</returns>
        public float GetProgress()
        {
            if ( _toLoad == 0 ) return 1;

            float fractionalLoaded = _loaded;

            if ( _peakTasks > 0 )
            {
                fractionalLoaded += ( ( _peakTasks - _tasks.Count ) / ( float )_peakTasks );
            }

            return Math.Min( 1, fractionalLoaded / _toLoad );
        }

        /// <summary>
        /// Sets an <see cref="IAssetErrorListener"/> to be invoked in case loading an asset failed.
        /// </summary>
        public void SetErrorListener( IAssetErrorListener listener )
        {
            this._listener = listener;
        }

        /// <summary>
        /// Disposes all assets in the manager and stops all asynchronous loading.
        /// </summary>
        public void Dispose()
        {
            Log.Debug( "Disposing." );

            Clear();

            _executor?.Dispose();
        }

        /// <summary>
        /// Clears and disposes all assets and the preloading queue.
        /// </summary>
        public void Clear()
        {
            _loadQueue.Clear();

            while ( !Update() )
            {
            }

            var dependencyCount = new Dictionary< string, int >();

            while ( _assetTypes.Count > 0 )
            {
                // for each asset, figure out how often it was referenced
                dependencyCount.Clear();

                var assets = _assetTypes.Keys.ToList();

                foreach ( var asset in assets )
                {
                    dependencyCount[ asset ] = 0;
                }

                foreach ( var asset in assets )
                {
                    var dependencies = _assetDependencies[ asset ];

                    foreach ( var dependency in dependencies )
                    {
                        var count = 0;

                        if ( dependencyCount.TryGetValue( dependency, out var value ) )
                        {
                            count = value == default ? 0 : value;
                        }

                        count++;
                        dependencyCount[ dependency ] = count;
                    }
                }

                // only dispose of assets that are root assets (not referenced)
                foreach ( var asset in assets )
                {
                    if ( dependencyCount.TryGetValue( asset, out var value ) == default )
                    {
                        Unload( asset );
                    }
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
        /// Returns the reference count of an asset.
        /// </summary>
        /// <param name="fileName"></param>
        public int GetReferenceCount( string fileName )
        {
            Type? type = _assetTypes[ fileName ];

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );

            if ( _assets == null ) throw new GdxRuntimeException( "_assets list is null!" );

            return _assets[ type ][ fileName ].RefCount;
        }

        /// <summary>
        /// Sets the reference count of an asset.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="refCount"></param>
        public void SetReferenceCount( string fileName, int refCount )
        {
            Type? type = _assetTypes[ fileName ];

            if ( type == null ) throw new GdxRuntimeException( "Asset not loaded: " + fileName );

            _assets[ type ][ fileName ].RefCount = refCount;
        }

        /// <summary>
        /// Returns a string containing ref count and dependency information for all assets.
        /// </summary>
        public string GetDiagnostics()
        {
            lock ( this )
            {
                var sb = new StringBuilder();

                foreach ( var fileName in _assetTypes.Keys )
                {
                    if ( sb.Length > 0 )
                    {
                        sb.Append( '\n' );
                    }

                    sb.Append( fileName );
                    sb.Append( ", " );

                    Type? type = _assetTypes[ fileName ];

                    if ( type == null )
                    {
                        sb.Append( "[ NULL type! ]" );
                    }
                    else
                    {
                        if ( _assets == null )
                        {
                            sb.Append( "NULL assets List!" );
                        }
                        else
                        {
                            var dependencies = _assetDependencies[ fileName ];

                            sb.Append( type.Name );

                            sb.Append( ", refs: " );
                            sb.Append( _assets[ type ][ fileName ].RefCount );

                            sb.Append( ", deps: [" );

                            foreach ( var dep in dependencies )
                            {
                                sb.Append( dep );
                                sb.Append( ',' );
                            }

                            sb.Append( ']' );
                        }
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
        public List< string > GetDependencies( string name )
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
