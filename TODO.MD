- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- STEP 1: Complete all conversions so that the code will build.
- STEP 2: Refactor, where possible and necessary, to take advantage of C# language features.
- STEP 3: Resolve ALL remaining TODOs.
- STEP 4: Ensure code is fully documented.
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- DONE = Class finished but may not be fully 'CSharp-ified'
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- PRIORITY is 2D classes first
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- Restructure, where necessary, to cut the number of exceptions that can be thrown
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

CORE
----
    - DONE - AbstractGraphics               Convert
    - DONE - AbstractInput                  Convert
    - DONE - ApplicationAdapter             Convert
    - DONE - Game                           Convert
    - DONE - Gdx                            Convert
    - DONE - IApplication                   Convert
    - DONE - IApplicationListener           Convert
    - DONE - IApplicationLogger             Convert
    - DONE - IAudio                         Convert
    - DONE - IFile                          Convert
    - DONE - IGraphics                      Convert
    - DONE - IInput                         Convert
    - DONE - IInputProcessor                Convert
    - DONE - ILifecycleListener             Convert
    - DONE - INet                           Convert
    - DONE - InputAdapter                   Convert
    - DONE - InputEventQueue                Convert
    - DONE - IPreferences                   Convert
    - DONE - IScreen                        Convert
    - DONE - ScreenAdapter                  Convert
    - DONE - Version                        Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS
------
    - DONE - AssetDescriptor                Convert
    - DONE - AssetLoaderParameters          Convert
    -      - AssetLoadingTask               Convert
    -      - AssetManager                   Convert
    - DONE - IAssetErrorListener            Convert
    - DONE - RefCountedContainer            Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS/LOADERS
--------------
    -      - AssetLoader                    Convert
    -      - AsynchronousAssetLoader        Convert
    -      - BitmapFontLoader               Convert
    -      - CubemapLoader                  Convert
    -      - I18NBundleLoader               Convert
    -      - IFileHandleResolver            Convert
    -      - ModelLoader                    Convert
    -      - MusicLoader                    Convert
    -      - ParticleEffectLoader           Convert
    -      - PixmapLoader                   Convert
    -      - ShaderProgramLoader            Convert
    -      - SkinLoader                     Convert
    -      - SoundLoader                    Convert
    -      - SynchronousAssetLoader         Convert
    -      - TextureAtlasLoader             Convert
    -      - TextureLoader                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS/LOADERS/RESOLVERS
------------------------
    -      - AbsoluteFileHandleResolver
    -      - ClasspathFileHandleResolver
    -      - ExternalFileHandleResolver
    -      - InternalFileHandleResolver
    -      - LocalFileHandleResolver
    -      - PrefixFileHandleResolver
    -      - ResolutionFileResolver

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

AUDIO
-----
    - DONE - IAudioDevice                   Convert
    - DONE - IAudioRecorder                 Convert
    - DONE - IMusic                         Convert
    - DONE - ISound                         Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

FILES
-----
    -      - FileHandle
    -      - FileHandleStream

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS
--------
    -      - BufferFormat                   Convert - record class
    -      - Camera                         Convert
    - DONE - Color                          Convert
    -      - Colors                         Convert
    -      - Cubemap                        Convert
    -      - DisplayMode                    Convert
    - DONE - FPSLogger                      Convert
    -      - GLTexture                      Convert
    -      - GraphicsType                   Convert
    -      - ICubemapData                   Convert
    -      - ICursor                        Convert
    - DONE - IGL20                          Convert
    - DONE - IGL30                          Convert
    -      - ITextureData                   Convert
    -      - Mesh                           Convert
    -      - Monitor                        Convert
    -      - OrthographicCamera             Convert
    -      - PerspectiveCamera              Convert
    -      - Pixmap                         Convert
    -      - PixmapIO                       Convert
    -      - Texture                        Convert
    -      - TextureArray                   Convert
    -      - TextureArrayData               Convert
    -      - TextureFilter                  Convert
    -      - VertexAttribute                Convert
    -      - VertexAttributes               Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G2D
------------
    -      - Animation                      Convert
    -      - BitmapFont                     Convert
    -      - BitmapFontCache                Convert
    -      - CpuSpriteBatch                 Convert
    -      - DistanceFieldFont              Convert
    -      - Gdx2DPixmap                    Convert
    -      - GlyphLayout                    Convert
    -      - IBatch                         Convert
    -      - IPolygonBatch                  Convert
    -      - NinePatch                      Convert
    -      - ParticleEffect                 Convert
    -      - ParticleEffectPool             Convert
    -      - ParticleEmitter                Convert
    -      - PixmapPacker                   Convert
    -      - PixmapPacker10                 Convert
    -      - PolygonRegion                  Convert
    -      - PolygonRegionLoader            Convert
    -      - PolygonSprite                  Convert
    -      - PolygonSpriteBatch             Convert
    -      - RepeatablePolygonSprite        Convert
    -      - Sprite                         Convert
    -      - SpriteBatch                    Convert
    -      - SpriteCache                    Convert
    -      - TextureAtlas                   Convert
    -      - TextureRegion                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D
------------
    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/GLUTILS
----------------
    -      - ETC1
    -      - ETC1TextureData
    -      - FacedCubemapData
    -      - FileTextureArrayData
    -      - FileTextureData
    -      - FloatFrameBuffer
    -      - FloatTextureData
    -      - FrameBuffer
    -      - FrameBufferCubemap
    -      - GLFrameBuffer
    -      - GLOnlyTextureData
    -      - GLVersion
    -      - HdpiMode
    -      - HdpiUtils
    -      - ImmediateModeRenderer
    -      - ImmediateModeRenderer20
    -      - IndexArray
    -      - IndexBufferObject
    -      - IndexBufferObjectSubData
    -      - IndexData
    -      - InstanceBufferObject
    -      - InstanceBufferObjectSubData
    -      - InstanceData
    -      - KTXTTextureData
    -      - MipMapGenerator
    -      - MipMapTextureData
    -      - PixmapTextureData
    -      - ShaderProgram
    -      - ShapeRenderer
    -      - VertexArray
    -      - VertexBufferObject
    -      - VertexBufferObjectSubData
    -      - VertexBufferObjectWithVAO
    -      - VertexData

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/PROFILING
------------------
    -      - GL20Interceptor
    -      - GL30Interceptor
    -      - GLErrorListener
    -      - GLInterceptor
    -      - GLProfiler

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

INPUT
-----
    -      - GestureDetector
    -      - RemoteInput
    -      - RemoteSender

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS
----
    -      - IImageResolver                         Convert
    - DONE - IMapRenderer                           Convert
    - DONE - Map                                    Convert
    - DONE - MapGroupLayer                          Convert
    - DONE - MapLayer                               Convert
    - DONE - MapLayers                              Convert
    - DONE - MapObject                              Convert
    -      - MapObjects                             Convert
    -      - MapProperties                          Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/OBJECTS
------------
    -      - CircleMapObject                        Convert
    -      - EllipseMapObject                       Convert
    -      - PolygonMapObject                       Convert
    -      - PolylineMapObject                      Convert
    -      - RectangleMapObject                     Convert
    -      - TextureMapObject                       Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED
----------
    - DONE - ITiledMapTile                          Convert
    - DONE - TiledMap                               Convert
    - DONE - TiledMapImageLayer                     Convert
    - DONE - TiledMapTileLayer                      Convert
    - DONE - TiledMapTileSet                        Convert
    - DONE - TiledMapTileSets                       Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/LOADERS
------------------
    -      - AtlasTmxMapLoader                      Convert
    -      - BaseTmxMapLoader                       Convert
    -      - TmxMapLoader                           Convert
    -      - TideMapLoader                          Q: Should I support Tide?

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/OBJECTS
------------------
    -      - TiledMapTileMapObject                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/RENDERERS
--------------------
    - DONE - BatchTiledMapRenderer                  Convert
    -      - HexagonalTiledMapRenderer              Convert
    -      - IsometricStaggeredTiledMapRenderer     Convert
    -      - IsometricTiledMapRenderer              Convert
    - DONE - ITiledMapRenderer                      Convert
    -      - OrthoCachedTiledMapRenderer            Convert
    -      - OrthogonalTiledMapRenderer             Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/TILES
----------------
    - DONE - AnimatedTileMapTile                    Convert
    - DONE - StaticTiledMapTile                     Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATH
----
    - DONE - Affine2
    -      - Bezier
    -      - Bresenham
    -      - BSpline
    -      - CatmullRomSpline
    -      - Circle
    -      - ConvexHull
    -      - CumulativeDistribution
    -      - DelaunayTriangulator
    -      - EarClippingTriangulator
    -      - Ellipse
    -      - FloatCounter
    -      - Frustrum
    -      - GeometryUtils
    -      - GridPoint2
    -      - GridPoint3
    -      - Interpolation
    -      - Intersector
    -      - IPath
    -      - IShape2D
    -      - IVector
    -      - MathUtils
    -      - Matrix3                Convert
    -      - Matrix4                Convert
    -      - Plane
    -      - Polygon
    -      - Polyline
    -      - Quaternion
    -      - RandomXS128
    -      - RectangleShape
    -      - Vector2                Convert, I prefer the way the LibGDX class works.
    -      - Vector3                Convert, I prefer the way the LibGDX class works.
    -      - WindowedMean

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATH/COLLISION
--------------
    -      - BoundingBox            Convert     Struct? / Record?
    -      - Ray                    Convert
    -      - Segment                Convert
    -      - Sphere                 Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

NETWORK
-------

    Note:- All this is probably already supported by C# System

    - **** - HttpParametersUtils
    - **** - HttpRequestBuilder
    - **** - HttpStatus
    - **** - IHttpRequestHeader
    - **** - IHttpResponseHeader
    - **** - IServerSocket
    - **** - ISocket
    - **** - NetJavaImpl
    - **** - NetJavaServerSocketImpl
    - **** - NetJavaSocketImpl
    - **** - ServerSocketHints
    - **** - SocketHints

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D
--------------
    - DONE - Action                         Convert
    -      - Actor                          Convert
    - DONE - Event                          Convert
    -      - Group                          Convert
    -      - IEventListener                 Convert
    -      - InputEvent                     Convert
    -      - InputListener                  Convert
    -      - Stage                          Convert
    -      - Touchable                      Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/ACTIONS
----------------------
    -      - Actions                        Convert
    -      - AddAction                      Convert
    -      - AddListenerAction              Convert
    -      - AfterAction                    Convert
    -      - AlphaAction                    Convert
    -      - ColorAction                    Convert
    -      - CountdownEventAction           Convert
    -      - DelayAction                    Convert
    -      - DelegateAction                 Convert
    -      - EventAction                    Convert
    -      - FloatAction                    Convert
    -      - IntAction                      Convert
    -      - LayoutAction                   Convert
    -      - MoveByAction                   Convert
    -      - MoveToAction                   Convert
    -      - ParallelAction                 Convert
    -      - RelativeTemporalAction         Convert
    -      - RemoveAction                   Convert
    -      - RemoveActorAction              Convert
    -      - RemoveListenerAction           Convert
    -      - RepeatAction                   Convert
    -      - RotateByAction                 Convert
    -      - RotateToAction                 Convert
    -      - RunnableAction                 Convert
    -      - ScaleByAction                  Convert
    -      - ScaleToAction                  Convert
    -      - SequenceAction                 Convert
    -      - SizeByAction                   Convert
    -      - SizeToAction                   Convert
    -      - TemporalAction                 Convert
    -      - TimeScaleAction                Convert
    -      - TouchableAction                Convert
    -      - VisibleAction                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/UI
-----------------
    -      - Button                         Convert
    -      - ButtonGroup                    Convert
    -      - Cell                           Convert
    -      - CheckBox                       Convert
    -      - Container                      Convert
    -      - Dialog                         Convert
    -      - HorizontalGroup                Convert
    -      - Image                          Convert
    -      - ImageButton                    Convert
    -      - ImageTextButton                Convert
    -      - Label                          Convert
    -      - List                           Convert
    -      - ParticleEffectActor            Convert
    -      - ProgressBar                    Convert
    -      - ScrollPane                     Convert
    -      - SelectBox                      Convert
    -      - Skin                           Convert
    -      - Slider                         Convert
    -      - SplitPane                      Convert
    -      - Stack                          Convert
    -      - Table                          Convert
    -      - TextArea                       Convert
    -      - TextButton                     Convert
    -      - TextField                      Convert
    -      - TextTooltip                    Convert
    -      - TooltipManager                 Convert
    -      - Touchpad                       Convert
    -      - Tree                           Convert
    -      - Value                          Convert
    -      - VerticalGroup                  Convert
    -      - Widget                         Convert
    -      - WidgetGroup                    Convert
    -      - Window                         Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/UTILS
--------------------
    -      - ActorGestureListener           Convert
    -      - ArraySelection                 Convert
    -      - BaseDrawable                   Convert
    -      - ChangeListener                 Convert
    -      - ClickListener                  Convert
    -      - DragAndDrop                    Convert
    -      - DragListener                   Convert
    -      - DragScrollListener             Convert
    -      - FocusListener                  Convert
    -      - ICullable                      Convert
    -      - IDisableable                   Convert
    -      - IDrawable                      Convert
    -      - ILayout                        Convert
    -      - ITransformDrawable             Convert
    -      - NinePatchDrawable              Convert
    -      - ScissorStack                   Convert
    -      - Selection                      Convert
    -      - SpriteDrawable                 Convert
    -      - TextureRegionDrawable          Convert
    -      - TiledDrawable                  Convert
    -      - UIUtils                        Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS
-----

    Buffers
    -------
    -      - Buffer                         Needed??
    -      - BufferUtils                    Needed??
    -      - DataBuffer                     Needed??
    -      - IntBuffer                      Needed??

    Clipboard
    ---------
    -      - IClipboard                     Convert - Interface, actual clipboard handled in backend.                 

    Pools
    -----
    - DONE - FlushablePool                  Convert
    - DONE - IPoolable                      Convert
    - DONE - Pool                           Convert
    - DONE - PooledLinkedList               Convert
    -      - Pools                          Convert

    Misc
    ----
    - DONE - Align                          Convert
    -      - Base64Coder                    Use System.Convert classes??
    -      - BinaryHeap
    - DONE - Bits                           Convert
    -      - ComparableTimSort              Convert
    -      - DataInput                      Check
    -      - DataOutput                     Check
    - DONE - FloatConsts                    Done, but is it needed?
    -      - GdxNativesLoader
    -      - I18NBundle                     Check
    -      - IRunnable
    -      - LittleEndianInputStream        Check
    - DONE - Logger                         Convert
    - DONE - NumberUtils                    Convert
    -      - PauseableThread
    -      - PerformanceCounter             Check
    -      - PerformanceCounters            Check
    -      - PropertiesUtils                Convert, but check first
    -      - QuadTreeFloat
    -      - Queue
    -      - QuickSelect
    - DONE - Scaling
    -      - ScreenUtils                    Convert
    -      - Select                     
    -      - SharedLibraryLoader            Check
    -      - SortUtils
    -      - SortedIntList
    -      - StreamUtils
    - **** - StringBuilder                  Use System.Text.StringBuilder
    -      - TextFormatter                  Remove, look in to C# localisation
    -      - Timer                          Use System.Timer instead??
    - DONE - TimeUtils
    - DONE - TimSort

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/ASYNC
-----------
    - **** - AsyncExecutor                  Needed??
    - **** - AsyncResult                    Needed??
    - **** - IAsyncTask                     Needed??
    - **** - ThreadUtils                    Needed??

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/ATOMIC
------------
    - **** - AtomicInteger                  Needed??
    - **** - AtomicQueue                    Needed??
    - **** - AtomicReferenceArray           Needed??

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/COLLECTIONS
-----------------

    Note: C# has IEnumerable and IEnumerator interfaces...

    Arrays
    ------
    - **** - Array                          List<>
    - **** - ArrayIterable                  Use IEnumerable?
    - **** - ArrayIterator                  Use IEnumerator?
    - **** - BooleanArray                   List< bool >
    - **** - ByteArray                      List< byte >
    - **** - CharArray                      List< char >
    - **** - IntArray                       List< int >
    -      - DelayedRemovalArray            Extend List<>
    - **** - FloatArray                     List< float >
    - **** - LongArray                      List< long >
    - **** - ShortArray                     List< short >
    -      - SnapshotArray                  Convert / extend List<T>

    Maps
    ----
    - **** - ArrayMap                       Replace with Dictionary<K, V>
    - **** - IdentityMap                    Convert / extend Dictionary< object, object > 
    - **** - IntFloatMap                    Replace with Dictionary< int, float >
    - **** - IntIntMap                      Replace with Dictionary< int, int >
    - **** - IntMap                         Replace with Dictionary< int, V >
    - **** - LongMap                        Replace with Dictionary< long, V >
    - **** - ObjectFloatMap                 Replace with Dictionary< object, float >
    - **** - ObjectIntMap                   Replace with Dictionary< object, int >
    - **** - ObjectLongMap                  Replace with Dictionary< object, long >
    - **** - ObjectMap                      Replace with Dictionary< object, object >
    - **** - OrderedMap                     Convert / extend Dictionary< object, object >

    Sets
    ----
    - **** - IntSet                         Replace with List<int>
    - **** - ObjectSet<T>                   Replace with List<T>
    - **** - OrderedSet<T>                  Convert / extend List<T>
                                            
    Misc                                    
    ----                                    
    - **** - Collections                    Needed??
    - **** - IPredicate                     Needed??
    - **** - PredicateIterable              Needed??
    - **** - PredicateIterator              Needed??

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                            
UTILS/COMPRESSION
-----------------
    - **** - CRC                            Needed??
    - **** - ICodeProgress                  Needed??
    - **** - LzmaAdapted                    Needed??

    UTILS/COMPRESSION/LZ
    --------------------
            - **** - BinTree                Needed??
            - **** - InWindow               Needed??
            - **** - OutWindow              Needed??
    
    UTILS/COMPRESSION/LZMA
    ----------------------
            - **** - Base                   Needed??
            - **** - Decoder                Needed??
            - **** - Encoder                Needed??
    
    UTILS/COMPRESSION/RANGECODER
    ----------------------------
            - **** - BitTreeDecoder         Needed??
            - **** - BitTreeEncoder         Needed??
            - **** - Decoder                Needed??
            - **** - Encoder                Needed??

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/EXCEPTIONS
----------------
    - DONE - GdxRuntimeException            Convert
    - DONE - IllegalStateException          Convert
    - DONE - NoSuchElementException         Convert
    - DONE - NumberFormatException          Convert
    - DONE - SerializationException         Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/JSON
----------
    - **** - IBaseJsonReader                Needed??
    - **** - Json                           Needed??
    - **** - JsonReader                     Needed??
    - **** - JsonValue                      Needed??
    - **** - JsonWriter                     Needed??
    - **** - UBJsonReader                   Needed??
    - **** - UBJsonWriter                   Needed??

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/REFLECT
-------------
    - **** - Annotation                     Needed??
    - **** - ArrayReflection                Needed??
    - **** - ClassReflection                Needed??
    - **** - Constructor                    Needed??
    - **** - Field                          Needed??
    - **** - Method                         Needed??
    - **** - ReflectionException            Needed??
    - **** - ReflectionPool                 Needed??

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/VIEWPORT
--------------
    - DONE - ExtendedViewport               Convert
    - DONE - FillViewport                   Convert
    - DONE - FitViewport                    Convert
    - DONE - ScalingViewport                Convert
    - DONE - ScreenViewport                 Convert
    - DONE - StretchViewport                Convert
    - DONE - Viewport                       Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/XML
---------
    -      - XmlReader                      Use System.Xml.XmlReader instead
    -      - XmlWriter                      Use System.Xml.XmlWriter instead

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

