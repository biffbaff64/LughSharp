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


using System.Text;
using LughSharp.LibCore.Assets.Loaders;
using LughSharp.LibCore.Assets.Loaders.Resolvers;
using LughSharp.LibCore.Scenes.Scene2D.UI;
using LughSharp.LibCore.Utils.Collections.Extensions;
using Exception = System.Exception;

namespace LughSharp.LibCore.Assets;

/// <summary>
///     Loads and stores assets like textures, bitmapfonts, tile maps, sounds, music and so on.
/// </summary>
[PublicAPI]
public class AssetManager
{
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private readonly Dictionary< string, List< string > >?                          _assetDependencies = new();
    private readonly Dictionary< Type, Dictionary< string, IRefCountedContainer > > _assets            = new();
    private readonly Dictionary< string, Type? >                                    _assetTypes        = new();

    private readonly List< string >                                              _injected  = new();
    private readonly Dictionary< Type, Dictionary< string, AssetLoaderBase? >? > _loaders   = new();
    private readonly List< AssetDescriptor >                                     _loadQueue = new();
    private readonly Stack< AssetLoadingTask >                                   _tasks     = new();
    private          IAssetErrorListener?                                        _listener;

    private int _loaded;
    private int _peakTasks;
    private int _toLoad;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Creates a new AssetManager with all default loaders.
    /// </summary>
    public AssetManager() : this( new AbsoluteFileHandleResolver() )
    {
    }

    /// <summary>
    ///     Creates a new AssetManager with optionally all default loaders. If you don't add the
    ///     default loaders then you do have to manually add the loaders you need, including any
    ///     loaders they might depend on.
    /// </summary>
    /// <param name="resolver"></param>
    /// <param name="defaultLoaders">Whether to add the default loaders (default is true).</param>
    public AssetManager( IFileHandleResolver resolver, bool defaultLoaders = true )
    {
        if ( defaultLoaders )
        {
            //@formatter:off
            SetLoader( typeof(BitmapFont),      new BitmapFontLoader( resolver ) );
            SetLoader( typeof(Texture),         new TextureLoader( resolver ) );
            SetLoader( typeof(TextureAtlas),    new TextureAtlasLoader( resolver ) );
            SetLoader( typeof(Pixmap),          new PixmapLoader( resolver ) );
            SetLoader( typeof(Skin),            new SkinLoader( resolver ) );
            SetLoader( typeof(IMusic),          new MusicLoader( resolver ) );
            SetLoader( typeof(ISound),          new SoundLoader( resolver ) );
            SetLoader( typeof(Cubemap),         new CubemapLoader( resolver ) );
            SetLoader( typeof(ParticleEffect),  new ParticleEffectLoader( resolver ) );
            SetLoader( typeof(ShaderProgram),   new ShaderProgramLoader( resolver ) );
            SetLoader( typeof(PolygonRegion),   new PolygonRegionLoader( resolver ) );
            //@formatter:on
        }

        FileHandleResolver = resolver;
    }

    /// <summary>
    ///     Returns the <see cref="IFileHandleResolver" /> which this
    ///     AssetManager was loaded with.
    /// </summary>
    /// <returns>the file handle resolver which this AssetManager uses.</returns>
    public IFileHandleResolver FileHandleResolver { get; set; }

    /// <summary>
    ///     Returns the number of loaded assets.
    /// </summary>
    public int LoadedAssetsCount => _assetTypes.Count;

    /// <summary>
    /// </summary>
    public T Get< T >( string name )
    {
        T? asset;

        lock ( this )
        {
            var type = _assetTypes[ name ];

            if ( type == null ) throw new GdxRuntimeException( $"Asset not loaded - {name}" );

            Dictionary< string, IRefCountedContainer > assetsByType = _assets[ type ];

            if ( assetsByType == null ) throw new GdxRuntimeException( $"Asset not loaded - {name}" );

            var assetContainer = assetsByType[ name ];

            if ( assetContainer == null ) throw new GdxRuntimeException( $"Asset not loaded - {name}" );

            asset = ( T? ) assetContainer.Asset;

            if ( asset == null ) throw new GdxRuntimeException( $"Asset not loaded - {name}" );
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
    public T Get< T >( string name, Type type )
    {
        lock ( this )
        {
            Dictionary< string, IRefCountedContainer >? assetsByType = _assets[ type ];

            if ( assetsByType == null ) throw new GdxRuntimeException( $"Asset not loaded - {name}" );

            var assetContainer = assetsByType[ name ];

            if ( assetContainer == null ) throw new GdxRuntimeException( $"Asset not loaded - {name}" );

            var asset = ( T? ) assetContainer.Asset;

            if ( asset == null ) throw new GdxRuntimeException( $"Asset not loaded - {name}" );

            return asset;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="assetDescriptor"></param>
    /// <returns></returns>
    public T Get< T >( AssetDescriptor assetDescriptor )
    {
        if ( assetDescriptor.Filepath == null ) throw new GdxRuntimeException( "Filepath is Null!" );

        return Get< T >( assetDescriptor.Filepath, assetDescriptor.AssetType );
    }

    /// <summary>
    ///     Returns all assets matching the requested type.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="outArray"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List< T > GetAll< T >( Type type, List< T > outArray )
    {
        lock ( this )
        {
            Dictionary< string, IRefCountedContainer > assetsByType = _assets[ type ];

            foreach ( var asset in assetsByType.Values )
            {
                var obj = asset.Asset;

                outArray.Add( ( T ) obj! );
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
          && assetDescriptor.Filepath.Equals( fileName ) )
            return true;

        foreach ( var t in _loadQueue )
            if ( t.Filepath.Equals( fileName ) )
                return true;

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
            if ( _tasks.First().AssetDesc is { } assetDesc
              && ( assetDesc.AssetType == type )
              && assetDesc.Filepath.Equals( fileName ) )
                return true;
        }

        foreach ( var assetDesc in _loadQueue )
            if ( ( assetDesc.AssetType == type )
              && assetDesc.Filepath.Equals( fileName ) )
                return true;

        return IsLoaded( fileName, type );
    }

    /// <summary>
    ///     Removes the asset and all its dependencies, IF they are not used by other assets.
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
            if ( _tasks.First().AssetDesc.Filepath == fileName )
            {
                Logger.Debug( $"Unload (from tasks): {fileName}" );

                _tasks.First().Cancel = true;
                _tasks.First().Unload();

                return;
            }
        }

        var type = _assetTypes[ fileName ];

        // check if it's in the queue
        var foundIndex = -1;

        for ( var i = 0; i < _loadQueue.Count; i++ )
            if ( _loadQueue[ i ].Filepath.Equals( fileName ) )
            {
                foundIndex = i;

                break;
            }

        if ( foundIndex != -1 )
        {
            _toLoad--;

            var desc = _loadQueue[ foundIndex ];

            _loadQueue.RemoveAt( foundIndex );

            Logger.Debug( $"Unload (from queue): {fileName}" );

            // if the queued asset was already loaded, let the callback know it is available.
            if ( ( type != null ) && desc.Parameters is { LoadedCallback: not null } )
                desc.Parameters.LoadedCallback?.FinishedLoading( this, desc.Filepath, desc.AssetType );

            return;
        }

        if ( type == null ) throw new GdxRuntimeException( $"Asset not loaded: {fileName}" );

        _assets.TryGetValue( type, out Dictionary< string, IRefCountedContainer >? assetRef );

        // if it is reference counted, decrement ref count and check if we can really get rid of it.
        if ( assetRef != null )
        {
            assetRef[ fileName ].RefCount--;

            if ( assetRef[ fileName ].RefCount <= 0 )
            {
                Logger.Debug( $"Unload (dispose): {fileName}" );

                if ( assetRef[ fileName ].Asset is IDisposable disposable ) disposable.Dispose();

                // remove the asset from the manager.
                _assetTypes.Remove( fileName );
                _assets[ type ].Remove( fileName );
            }
            else
                Logger.Debug( $"Unload (decrement): {fileName}" );

            // remove any dependencies (or just decrement their ref count).
            List< string >? dependencies = _assetDependencies?[ fileName ];

            if ( dependencies != null )
            {
                foreach ( var dependency in dependencies )
                    if ( IsLoaded( dependency ) )
                    {
                        //TODO: Refactor to remove recursiveness
                        Unload( dependency );
                    }
            }

            // remove dependencies if ref count < 0
            if ( assetRef[ fileName ].RefCount <= 0 ) _assetDependencies?.Remove( fileName );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="asset">the asset</param>
    /// <returns>whether the asset is contained in this manager</returns>
    public bool ContainsAsset< T >( T asset )
    {
        if ( asset == null ) return false;

        Dictionary< string, IRefCountedContainer > assetsByType = _assets[ asset.GetType() ];

        foreach ( var fileName in assetsByType.Keys )
        {
            var otherAsset = ( T? ) assetsByType[ fileName ].Asset;

            if ( ( otherAsset?.GetType() == asset.GetType() ) || asset.Equals( otherAsset ) ) return true;
        }

        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="asset">the asset</param>
    /// <returns>the filename of the asset or null</returns>
    public string? GetAssetFileName< T >( T asset )
    {
        if ( asset == null ) return null;

        foreach ( var assetType in _assets.Keys )
        {
            Dictionary< string, IRefCountedContainer > assetsByType = _assets[ assetType ];

            foreach ( var fileName in assetsByType.Keys )
            {
                var otherAsset = ( T? ) assetsByType[ fileName ].Asset;

                if ( ( otherAsset?.GetType() == asset.GetType() ) || asset.Equals( otherAsset ) ) return fileName;
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
        return ( fileName != null ) && _assetTypes.ContainsKey( fileName );
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName">the file name of the asset</param>
    /// <param name="type"></param>
    /// <returns>whether the asset is loaded</returns>
    public bool IsLoaded( string fileName, Type type )
    {
        return _assets[ type ][ fileName ].Asset != null;
    }

    /// <summary>
    ///     Returns the loader for the given type and the specified filename.
    /// </summary>
    /// <param name="type">The type of the loader to get</param>
    /// <param name="fileName">
    ///     The filename of the asset to get a loader for, or null to get the default loader.
    /// </param>
    /// <returns>
    ///     The loader capable of loading the type and filename, or null if none exists.
    /// </returns>
    public AssetLoaderBase? GetLoader( Type? type, string? fileName = null )
    {
        if ( ( type == null ) || ( _loaders[ type ] == null ) || ( _loaders[ type ]?.Count < 1 ) ) return null;

        if ( fileName == null ) return _loaders[ type ]?[ "" ];

        AssetLoaderBase? result = null;

        var len = -1;

        foreach ( KeyValuePair< string, AssetLoaderBase? > entry in _loaders[ type ]! )
        {
            if ( ( entry.Key.Length > len ) && fileName.EndsWith( entry.Key ) ) result = entry.Value;

            len = entry.Key.Length;
        }

        return result;
    }

    /// <summary>
    ///     Adds the given asset to the loading queue of the AssetManager.
    /// </summary>
    /// <param name="fileName">
    ///     the file name (interpretation depends on <see cref="AssetLoaderBase" />)
    /// </param>
    /// <param name="type">the type of the asset.</param>
    /// <param name="parameter"></param>
    public void Load( string? fileName, Type? type, AssetLoaderParameters? parameter )
    {
        ArgumentNullException.ThrowIfNull( fileName, "Filename not specified!" );

        var loader = GetLoader( type, fileName );

        if ( loader == null ) throw new GdxRuntimeException( $"No loader for type: {type?.Name}" );

        if ( _loadQueue.Count == 0 )
        {
            // reset stats
            _loaded    = 0;
            _toLoad    = 0;
            _peakTasks = 0;
        }

        // check if an asset with the same name but a
        // different type has already been added.

        // check preload queue
        foreach ( var desc in _loadQueue )
            if ( ( desc.Filepath == fileName ) && ( desc.AssetType != type ) )
            {
                throw new GdxRuntimeException
                    ( $"Asset with name '{fileName}' already in preload queue, but has different type (expected: {type?.Name}, found: {desc.AssetType.Name})" );
            }

        // check task list
        for ( var i = 0; i < _tasks.Count; i++ )
        {
            var desc = _tasks.ElementAt( i ).AssetDesc;

            if ( ( desc.Filepath == fileName ) && ( desc.AssetType != type ) )
            {
                throw new GdxRuntimeException
                    ( $"Asset with name '{fileName}' already in preload queue, but has different type (expected: {type?.Name}, found: {desc.AssetType.Name})" );
            }
        }

        // check loaded assets
        var otherType = _assetTypes[ fileName ];

        if ( ( otherType != null ) && ( otherType != type ) )
        {
            throw new GdxRuntimeException
                ( $"Asset with name '{fileName}' already loaded, but has different type (expected: {type?.Name}, found: {otherType.Name})" );
        }

        _toLoad++;

        var assetDesc = new AssetDescriptor
        {
            Filepath   = fileName,
            AssetType  = type!,
            Parameters = parameter
        };

        _loadQueue.Add( assetDesc );

        Logger.Debug( $"Queued: {assetDesc}" );
    }

    /// <summary>
    ///     Adds the given asset to the loading queue of the AssetManager.
    /// </summary>
    /// <param name="desc">the <see cref="AssetDescriptor" /></param>
    public void Load( AssetDescriptor desc )
    {
        ArgumentNullException.ThrowIfNull( desc );

        Load( desc.Filepath, desc.AssetType, desc.Parameters );
    }

    /// <summary>
    ///     Updates the AssetManager for a single task. Returns if the current task is
    ///     still being processed or there are no tasks, otherwise it finishes the current
    ///     task and starts the next task.
    /// </summary>
    /// <returns>true if all loading is finished.</returns>
    public bool Update()
    {
        try
        {
            if ( _tasks.Count == 0 )
            {
                // loop until we have a new task ready to be processed
                while ( ( _loadQueue.Count != 0 ) && ( _tasks.Count == 0 ) ) NextTask();

                // have we not found a task? We are done!
                if ( _tasks.Count == 0 ) return true;
            }

            return UpdateTask() && ( _loadQueue.Count == 0 ) && ( _tasks.Count == 0 );
        }
        catch ( Exception t )
        {
            HandleTaskError( t );

            return _loadQueue.Count == 0;
        }
    }

    /// <summary>
    ///     Updates the AssetManager continuously for the specified number of milliseconds,
    ///     yielding the CPU to the loading thread between updates. This may block for less
    ///     time if all loading tasks are complete. This may block for more time if the portion
    ///     of a single task that happens in the GL thread takes a long time.
    /// </summary>
    /// <returns> true if all loading is finished.</returns>
    public bool Update( int millis )
    {
        var endTime = TimeUtils.Millis() + millis;

        while ( true )
        {
            var done = Update();

            if ( done || ( TimeUtils.Millis() > endTime ) ) return done;
        }
    }

    /// <summary>
    ///     Returns true when all assets are loaded. Can be called from any thread but
    ///     note <see cref="Update()" /> or related methods must be called to process tasks.
    /// </summary>
    public bool IsFinished()
    {
        return ( _loadQueue.Count == 0 ) && ( _tasks.Count == 0 );
    }

    /// <summary>
    ///     Blocks until all assets are loaded.
    /// </summary>
    public void FinishLoading()
    {
        Logger.Debug( "Waiting for loading to complete..." );

        while ( !Update() )
        {
            //
        }

        Logger.Debug( "Loading complete." );
    }

    /// <summary>
    ///     Blocks until the specified asset is loaded.
    /// </summary>
    /// <param name="assetDesc">the AssetDescriptor of the asset</param>
    public T FinishLoadingAsset< T >( AssetDescriptor assetDesc )
    {
        return FinishLoadingAsset< T >( assetDesc.Filepath );
    }

    /// <summary>
    ///     Blocks until the specified asset is loaded.
    /// </summary>
    /// <param name="fileName">
    ///     the file name (interpretation depends on <see cref="AssetLoaderBase" />)
    /// </param>
    public T FinishLoadingAsset< T >( string? fileName )
    {
        Debug.Assert( fileName != null, $"{nameof( fileName )} is null" );

        Logger.Debug( $"Waiting for asset to be loaded: {fileName}" );

        while ( true )
            lock ( this )
            {
                var type = _assetTypes[ fileName ];

                if ( type != null )
                {
                    Dictionary< string, IRefCountedContainer > assetsByType = _assets[ type ];

                    if ( assetsByType[ fileName ] is { } assetContainer )
                    {
                        var asset = ( T? ) assetContainer.Asset;

                        if ( asset != null )
                        {
                            Logger.Debug( $"Asset loaded: {fileName}" );

                            return asset;
                        }
                    }
                }

                Update();
            }
    }

    /// <summary>
    /// </summary>
    /// <param name="parentAssetFilename"></param>
    /// <param name="dependendAssetDescs"></param>
    public void InjectDependencies( string? parentAssetFilename, List< AssetDescriptor > dependendAssetDescs )
    {
        ArgumentNullException.ThrowIfNull( parentAssetFilename );

        foreach ( var desc in dependendAssetDescs )
        {
            Debug.Assert( desc.Filepath != null, "desc.FilePath is null" );

            if ( _injected.Contains( desc.Filepath ) )
            {
                // Ignore subsequent dependencies if there are duplicates.
                continue;
            }

            _injected.Add( desc.Filepath );

            InjectDependency( parentAssetFilename, desc );
        }

        _injected.Clear();
        _injected.EnsureCapacity( 32 );
    }

    /// <summary>
    /// </summary>
    /// <param name="parentAssetFilename"></param>
    /// <param name="dependendAssetDesc"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public void InjectDependency( string parentAssetFilename, AssetDescriptor dependendAssetDesc )
    {
        // add the asset as a dependency of the parent asset
        List< string >? dependencies = _assetDependencies?[ parentAssetFilename ];

        if ( dependencies == null )
        {
            dependencies = new List< string >();

            _assetDependencies?.Put( parentAssetFilename, dependencies );
        }

        _assetDependencies?[ parentAssetFilename ].Add( dependendAssetDesc.Filepath );

        // if the asset is already loaded, increase its reference count.
        if ( IsLoaded( dependendAssetDesc.Filepath ) )
        {
            Logger.Debug( $"Dependency already loaded: {dependendAssetDesc}" );

            var type = _assetTypes[ dependendAssetDesc.Filepath ];

            if ( type == null ) throw new GdxRuntimeException( "type cannot be null!" );

            _assets[ type ][ dependendAssetDesc.Filepath ].RefCount++;

            IncrementRefCountedDependencies( dependendAssetDesc.Filepath );
        }
        else
        {
            // else add a new task for the asset.
            Logger.Debug( $"Loading dependency: {dependendAssetDesc}" );

            AddTask( dependendAssetDesc );
        }
    }

    /// <summary>
    ///     Removes a task from the loadQueue and adds it to the task stack.
    ///     If the asset is already loaded (which can happen if it was a
    ///     dependency of a previously loaded asset) its reference count will
    ///     be increased.
    /// </summary>
    public void NextTask()
    {
        var assetDesc = _loadQueue.RemoveIndex( 0 );

        // if the asset is not meant to be reloaded and is already
        // loaded, increase its reference count
        if ( IsLoaded( assetDesc.Filepath ) )
        {
            Logger.Debug( $"Already loaded: {assetDesc}" );

            var type = _assetTypes[ assetDesc.Filepath ];

            if ( type == null ) throw new GdxRuntimeException( "type cannot be null!" );

            _assets[ type ][ assetDesc.Filepath ].RefCount++;

            IncrementRefCountedDependencies( assetDesc.Filepath );

            assetDesc.Parameters?.LoadedCallback?.FinishedLoading( this,
                                                                   assetDesc.Filepath,
                                                                   assetDesc.AssetType );

            _loaded++;
        }
        else
        {
            // else add a new task for the asset.
            Logger.Debug( $"Loading: {assetDesc}" );

            AddTask( assetDesc );
        }
    }

    /// <summary>
    ///     Adds a <see cref="AssetLoadingTask" /> to the task stack for the given asset.
    /// </summary>
    public void AddTask( AssetDescriptor assetDesc )
    {
        var loader = GetLoader( assetDesc.AssetType, assetDesc.Filepath );

        if ( loader == null ) throw new GdxRuntimeException( $"No loader for type: {assetDesc.AssetType}" );

        _tasks.Push( new AssetLoadingTask( this, assetDesc, loader ) );

        _peakTasks++;
    }

    /// <summary>
    ///     Adds an asset to this AssetManager.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    /// <param name="asset"></param>
    /// <typeparam name="T"></typeparam>
    public void AddAsset< T >( string fileName, Type type, T asset )
    {
        if ( asset == null ) throw new GdxRuntimeException( "No asset to add: null" );

        // add the asset to the filename lookup
        _assetTypes[ fileName ]     = type;
        _assets[ type ][ fileName ] = new RefCountedContainer( asset );
    }

    /// <summary>
    ///     Updates the current task on the top of the task stack.
    /// </summary>
    /// <returns>true if the asset is loaded or the task was cancelled.</returns>
    public bool UpdateTask()
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

            if ( task.AssetDesc.Filepath == null ) throw new GdxRuntimeException( "task.AssetDesc.FilePath cannot be null!" );

            AddAsset( task.AssetDesc.Filepath, task.AssetDesc.AssetType, task.Asset );

            // otherwise, if a listener was found in the parameter invoke it
            task.AssetDesc.Parameters?.LoadedCallback?.FinishedLoading(
                                                                       this,
                                                                       task.AssetDesc.Filepath,
                                                                       task.AssetDesc.AssetType
                                                                      );

            return true;
        }

        return false;
    }

    /// <summary>
    ///     Called when a task throws an exception during loading. The default implementation
    ///     rethrows the exception and does not use the <tt>assetDesc</tt> parameter.
    ///     A subclass may supress the default implementation when loading assets where loading
    ///     failure is recoverable.
    /// </summary>
    public virtual void TaskFailed( AssetDescriptor assetDesc, Exception ex )
    {
        throw ex;
    }

    /// <summary>
    /// </summary>
    /// <param name="parent"></param>
    public void IncrementRefCountedDependencies( string parent )
    {
        if ( _assetDependencies?[ parent ] == null ) return;

        foreach ( var dependency in _assetDependencies[ parent ] )
        {
            var type = _assetTypes[ dependency ];

            if ( type == null ) throw new GdxRuntimeException( "type cannot be null!" );

            _assets[ type ][ dependency ].RefCount++;

            // TODO: Refactor to remove recursiveness
            IncrementRefCountedDependencies( dependency );
        }
    }

    /// <summary>
    ///     Handles a runtime/loading error in <see cref="Update()" /> by optionally
    ///     invoking the <see cref="IAssetErrorListener" />.
    /// </summary>
    /// <param name="t"></param>
    public void HandleTaskError( Exception t )
    {
        Logger.Error( $"Error loading asset: {t}" );

        if ( _tasks.Count == 0 ) throw new GdxRuntimeException( t );

        // pop the faulty task from the stack
        var task      = _tasks.Pop();
        var assetDesc = task.AssetDesc;

        // remove all dependencies if dependences are loaded and
        // those dependencies actually exist...
        if ( task is { DependenciesLoaded: true, Dependencies: not null } )
        {
            foreach ( var desc in task.Dependencies ) Unload( desc.Filepath );
        }

        // clear the rest of the stack
        _tasks.Clear();

        // inform the listener that something bad happened
        if ( _listener != null )
            _listener.Error( assetDesc, t );
        else
            throw new GdxRuntimeException( t );
    }

    /// <summary>
    ///     Sets a new <see cref="AssetLoaderBase" /> for the given type.
    /// </summary>
    /// <param name="type"> the type of the asset </param>
    /// <param name="loader"> the loader  </param>
    public void SetLoader( Type type, AssetLoaderBase loader )
    {
        lock ( this )
        {
            SetLoader( type, null, loader );
        }
    }

    /// <summary>
    ///     Sets a new <see cref="AssetLoaderBase" /> for the given type.
    /// </summary>
    /// <param name="type"> the type of the asset </param>
    /// <param name="suffix">
    ///     the suffix the filename must have for this loader to be used or null
    ///     to specify the default loader.
    /// </param>
    /// <param name="loader"> the loader</param>
    public void SetLoader( Type type, string? suffix, AssetLoaderBase loader )
    {
        lock ( this )
        {
            ArgumentNullException.ThrowIfNull( type );
            ArgumentNullException.ThrowIfNull( loader );

            if ( _loaders[ type ] == null ) _loaders.Put( type, new Dictionary< string, AssetLoaderBase? >() );

            _loaders[ type ]?.Put( suffix ?? "", loader );

            Logger.Debug( $"Loader set: {type.Name} -> {loader.GetType().Name}" );
        }
    }

    /// <summary>
    ///     Returns the number of currently queued assets.
    /// </summary>
    public int GetQueuedAssets()
    {
        return _loadQueue.Count + _tasks.Count;
    }

    /// <summary>
    ///     Returns the progress in percent of completion.
    /// </summary>
    public float GetProgress()
    {
        if ( _toLoad == 0 ) return 1f;

        float fractionalLoaded = _loaded;

        if ( _peakTasks > 0 ) fractionalLoaded += ( _peakTasks - _tasks.Count ) / ( float ) _peakTasks;

        return Math.Min( 1f, fractionalLoaded / _toLoad );
    }

    /// <summary>
    ///     Sets an <see cref="IAssetErrorListener" /> to be invoked in case loading an asset failed.
    /// </summary>
    public void SetErrorListener( IAssetErrorListener listener )
    {
        _listener = listener;
    }

    /// <summary>
    ///     Disposes all assets in the manager and stops all asynchronous loading.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing ) Clear();
    }

    /// <summary>
    ///     Clears and disposes all assets and the preloading queue.
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

            List< string > assets = _assetTypes.Keys.ToList();

            foreach ( var asset in assets ) dependencyCount[ asset ] = 0;

            foreach ( var asset in assets )
            {
                List< string >? dependencies = _assetDependencies?[ asset ];

                if ( dependencies != null )
                {
                    foreach ( var dependency in dependencies )
                    {
                        var count = 0;

                        if ( dependencyCount.TryGetValue( dependency, out var value ) ) count = value == default( int ) ? 0 : value;

                        count++;
                        dependencyCount[ dependency ] = count;
                    }
                }
            }

            // only dispose of assets that are root assets (not referenced)
            foreach ( var asset in assets )
                if ( dependencyCount.TryGetValue( asset, out _ ) == default( bool ) )
                    Unload( asset );
        }

        _assets.Clear();
        _assetTypes.Clear();
        _assetDependencies?.Clear();
        _loaded    = 0;
        _toLoad    = 0;
        _peakTasks = 0;
        _loadQueue.Clear();
        _tasks.Clear();
    }

    /// <summary>
    ///     Returns the reference count of an asset.
    /// </summary>
    /// <param name="fileName"></param>
    public int GetReferenceCount( string fileName )
    {
        var type = _assetTypes[ fileName ];

        if ( type == null ) throw new GdxRuntimeException( $"Asset not loaded: {fileName}" );

        if ( _assets == null ) throw new GdxRuntimeException( "_assets list is null!" );

        return _assets[ type ][ fileName ].RefCount;
    }

    /// <summary>
    ///     Sets the reference count of an asset.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="refCount"></param>
    public void SetReferenceCount( string fileName, int refCount )
    {
        var type = _assetTypes[ fileName ];

        if ( type == null ) throw new GdxRuntimeException( $"Asset not loaded: {fileName}" );

        _assets[ type ][ fileName ].RefCount = refCount;
    }

    /// <summary>
    ///     Returns a string containing ref count and dependency information for all assets.
    /// </summary>
    public string GetDiagnostics()
    {
        lock ( this )
        {
            var sb = new StringBuilder();

            foreach ( var fileName in _assetTypes.Keys )
            {
                if ( sb.Length > 0 ) sb.Append( '\n' );

                sb.Append( fileName );
                sb.Append( ", " );

                var type = _assetTypes[ fileName ];

                if ( type == null )
                    sb.Append( "[ NULL type! ]" );
                else
                {
                    if ( _assets == null )
                        sb.Append( "NULL assets List!" );
                    else
                    {
                        List< string >? dependencies = _assetDependencies?[ fileName ];

                        if ( dependencies != null )
                        {
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
    public IEnumerable< string >? GetDependencies( string name )
    {
        lock ( this )
        {
            return _assetDependencies?[ name ];
        }
    }

    /// <summary>
    ///     Returns the asset type for the given asset name.
    /// </summary>
    /// <param name="name"> String holding the asset name. </param>
    /// <returns> The asset type. </returns>
    public Type? GetAssetType( string name )
    {
        lock ( this )
        {
            return _assetTypes[ name ];
        }
    }
}