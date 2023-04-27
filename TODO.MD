- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
LIBGDX CSharp CONVERSION - ROUND 1
----------------------------------
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- STEP 1: Complete all conversions so that the code will build.
- STEP 2: Refactor, where possible and necessary, to take advantage of C# language features.
- STEP 3: Resolve ALL remaining TODOs.
- STEP 4: Ensure code is fully documented.
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- IP   = Conversion In Progress.
- DONE = Class finished but may not be fully 'CSharp-ified'
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- PRIORITY is 2D classes first
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- Restructure, where necessary, to cut the number of exceptions that can be thrown
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ROOT
----

    - DONE - Gdx                            Convert

CORE
----
    - DONE - AbstractGraphics               Convert
    - DONE - AbstractInput                  Convert
    - DONE - ApplicationAdapter             Convert
    - DONE - Game                           Convert
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
    - DONE - AssetLoadingTask               Convert
    - IP   - AssetManager                   Convert
    - DONE - IAssetErrorListener            Convert
    - DONE - RefCountedContainer            Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS/LOADERS
--------------
    - DONE - AssetLoader                    Convert
    - DONE - AsynchronousAssetLoader        Convert
    - IP   - BitmapFontLoader               Convert
    -      - CubemapLoader                  Convert
    -      - I18NBundleLoader               Convert
    -      - IFileHandleResolver            Convert
    -      - ModelLoader                    Convert
    - IP   - MusicLoader                    Convert
    -      - ParticleEffectLoader           Convert
    -      - PixmapLoader                   Convert
    -      - ShaderProgramLoader            Convert
    -      - SkinLoader                     Convert
    -      - SoundLoader                    Convert
    - DONE - SynchronousAssetLoader         Convert
    -      - TextureAtlasLoader             Convert
    - DONE - TextureLoader                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS/LOADERS/RESOLVERS
------------------------
    - DONE - AbsoluteFileHandleResolver
    - DONE - IFileInfoResolver
    - **** - ClasspathFileHandleResolver    Not Needed
    - **** - ExternalFileHandleResolver     Not Needed
    - **** - InternalFileHandleResolver     Not Needed
    - **** - LocalFileHandleResolver        Not Needed
    - **** - PrefixFileHandleResolver       Not Needed
    - **** - ResolutionFileResolver         Not Needed

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
    -      - File
    -      - FileHandle
    -      - FileHandleStream

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS
--------
    - DONE - BufferFormat                   Convert - record class
    - DONE - Camera                         Convert
    - DONE - Color                          Convert
    - DONE - Colors                         Convert
    -      - Cubemap                        Convert
    - DONE - DisplayMode                    Convert
    - DONE - FPSLogger                      Convert
    -      - GLTexture                      Convert
    - DONE - GraphicsType                   Convert
    -      - ICubemapData                   Convert
    - DONE - ICursor                        Convert
    - DONE - IGL20                          Convert
    - IP   - IGL30                          Convert
    - DONE - ITextureData                   Convert
    -      - Mesh                           Convert
    - DONE - Monitor                        Convert
    - IP   - OrthographicCamera             Convert
    -      - PerspectiveCamera              Convert
    - IP   - Pixmap                         Convert
    - IP   - PixmapIO                       Convert
    - IP   - Texture                        Convert
    -      - TextureArray                   Convert
    -      - TextureArrayData               Convert
    - DONE - TextureFilter                  Convert
    - DONE - TextureWrap                    Convert
    -      - VertexAttribute                Convert
    -      - VertexAttributes               Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G2D
------------
    -      - Animation                      Convert
    - IP   - BitmapFont                     Convert
    -      - BitmapFontCache                Convert
    -      - CpuSpriteBatch                 Convert
    -      - DistanceFieldFont              Convert
    -      - Gdx2DPixmap                    Convert
    -      - GlyphLayout                    Convert
    - IP   - IBatch                         Convert
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
    - IP   - SpriteBatch                    Convert
    -      - SpriteCache                    Convert
    - IP   - TextureAtlas                   Convert
    - IP   - TextureRegion                  Convert

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
    - IP   - ETC1TextureData
    -      - FacedCubemapData
    -      - FileTextureArrayData
    - IP   - FileTextureData
    -      - FloatFrameBuffer
    -      - FloatTextureData
    -      - FrameBuffer
    -      - FrameBufferCubemap
    -      - GLFrameBuffer
    -      - GLOnlyTextureData
    - IP   - GLVersion
    - IP   - HdpiMode
    - IP   - HdpiUtils
    -      - ImmediateModeRenderer
    -      - ImmediateModeRenderer20
    -      - IndexArray
    -      - IndexBufferObject
    -      - IndexBufferObjectSubData
    -      - IndexData
    -      - InstanceBufferObject
    -      - InstanceBufferObjectSubData
    -      - InstanceData
    - IP   - KTXTTextureData
    -      - MipMapGenerator
    -      - MipMapTextureData
    -      - PixmapTextureData
    -      - ShaderProgram
    - IP   - ShapeRenderer
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
    - DONE - IImageResolver                         Convert
    - DONE - IMapRenderer                           Convert
    - DONE - Map                                    Convert
    - DONE - MapGroupLayer                          Convert
    - DONE - MapLayer                               Convert
    - DONE - MapLayers                              Convert
    - DONE - MapObject                              Convert
    - DONE - MapObjects                             Convert
    - DONE - MapProperties                          Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/OBJECTS
------------
    - DONE - CircleMapObject                        Convert
    - DONE - EllipseMapObject                       Convert
    - DONE - PolygonMapObject                       Convert
    - DONE - PolylineMapObject                      Convert
    - DONE - RectangleMapObject                     Convert
    - DONE - TextureMapObject                       Convert

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
    - IP   - AtlasTmxMapLoader                      Convert
    - IP   - BaseTmxMapLoader                       Convert
    -      - TideMapLoader                          Q: Should I support Tide?
    - IP   - TmxMapLoader                           Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/OBJECTS
------------------
    - IP   - TiledMapTileMapObject                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/RENDERERS
--------------------
    - IP   - BatchTiledMapRenderer                  Convert
    -      - HexagonalTiledMapRenderer              Convert
    -      - IsometricStaggeredTiledMapRenderer     Convert
    -      - IsometricTiledMapRenderer              Convert
    - DONE - ITiledMapRenderer                      Convert
    -      - OrthoCachedTiledMapRenderer            Convert
    - IP   - OrthogonalTiledMapRenderer             Convert

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
    - DONE - Bezier
    - DONE - Bresenham2
    -      - BSpline
    -      - CatmullRomSpline
    - DONE - Circle
    -      - ConvexHull
    -      - CumulativeDistribution
    -      - DelaunayTriangulator
    -      - EarClippingTriangulator
    -      - Ellipse
    - DONE - FloatCounter
    - DONE - Frustrum
    -      - GeometryUtils
    - DONE - GridPoint2
    -      - GridPoint3
    -      - Interpolation
    -      - Intersector
    - DONE - IPath
    - DONE - IShape2D
    - DONE - IVector
    - DONE - MathUtils
    -      - Matrix3                Convert
    -      - Matrix4                Convert
    - DONE - Number
    - DONE - Plane
    -      - Polygon
    -      - Polyline
    -      - Quaternion
    - DONE - RandomXS128
    - DONE - RectangleShape
    - DONE - Vector2                Convert, I prefer the way the LibGDX class works.
    - DONE - Vector3                Convert, I prefer the way the LibGDX class works.
    - DONE - WindowedMean

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATH/COLLISION
--------------
    - DONE - BoundingBox            Convert     Struct? / Record?
    - DONE - Ray                    Convert
    - DONE - Segment                Convert
    - DONE - Sphere                 Convert

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
    - IP - Action                         Convert
    - IP   - Actor                          Convert
    - IP - Event                          Convert
    - IP   - Group                          Convert
    - DONE - IEventListener                 Convert
    - DONE - InputEvent                     Convert
    -      - InputListener                  Convert
    - IP   - Stage                          Convert
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
    - IP   - Table                          Convert
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
    - IP   - WidgetGroup                    Convert
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
    - DONE - IClipboard                     Convert - Interface, actual clipboard handled in backend.                 

    Misc
    ----
    - DONE - Align                          Convert
    -      - Base64Coder                    Use System.Convert classes??
    -      - BinaryHeap
    - DONE - Bits                           Convert
    -      - ComparableTimSort              Convert
    -      - DataInput                      Check
    -      - DataOutput                     Check
    - DONE - DataUtils                      Added Class
    - DONE - FloatConsts                    Done, but is it needed?
    -      - GdxNativesLoader
    -      - I18NBundle                     Check
    - DONE - IRunnable                      Done, but is it needed?
    -      - LittleEndianInputStream        Check
    - DONE - Logger                         Convert
    - DONE - NumberUtils                    Convert
    -      - PauseableThread
    - DONE - PerformanceCounter             Check
    - DONE - PerformanceCounters            Check
    -      - PropertiesUtils                Convert, but check first
    -      - QuadTreeFloat
    -      - Queue
    -      - QuickSelect
    - DONE - Scaling
    - IP   - ScreenUtils                    Convert
    - IP   - Select                         Renamed Selector
    -      - SharedLibraryLoader            Check
    - DONE - SortUtils
    -      - SortedIntList
    -      - StreamUtils
    - **** - StringBuilder                  Use System.Text.StringBuilder
    - **** - TextFormatter                  Remove, look into C# localisation
    - DONE - Timer                          Use System.Timer instead??
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
    - **** - FloatArray                     List< float >
    - **** - LongArray                      List< long >
    - **** - ShortArray                     List< short >

    - DONE - DelayedRemovalArray            Convert / Extend List<>
    - DONE - SnapshotArray                  Convert / extend List<T>

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
    - **** - CRC                            N0
    - **** - ICodeProgress                  N0
    - **** - LzmaAdapted                    N0

    UTILS/COMPRESSION/LZ
    --------------------
            - **** - BinTree                No
            - **** - InWindow               No
            - **** - OutWindow              No
    
    UTILS/COMPRESSION/LZMA
    ----------------------
            - **** - Base                   No
            - **** - Decoder                No
            - **** - Encoder                No
    
    UTILS/COMPRESSION/RANGECODER
    ----------------------------
            - **** - BitTreeDecoder         No
            - **** - BitTreeEncoder         No
            - **** - Decoder                No
            - **** - Encoder                No

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/EXCEPTIONS
----------------
    - DONE - AssetNotLoadedException        Convert
    - DONE - BufferOverflowException        Convert
    - DONE - BufferUnderflowException       Convert
    - DONE - GdxRuntimeException            Convert
    - DONE - IllegalStateException          Convert
    - DONE - NoSuchElementException         Convert
    - DONE - NumberFormatException          Convert
    - DONE - ReadOnlyBufferException        Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/JSON
----------
    -      - IBaseJsonReader                Convert
    -      - Json                           Convert
    -      - JsonReader                     Convert
    -      - JsonValue                      Convert
    -      - JsonWriter                     Convert
    -      - UBJsonReader                   Convert
    -      - UBJsonWriter                   Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/POOLING
-------------
    - DONE - FlushablePool                  Convert
    - DONE - IPoolable                      Convert
    - DONE - Pool                           Convert
    - DONE - PooledLinkedList               Convert
    - DONE - Pools                          Convert
    - DONE - ReflectionPool                 Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/REFLECT
-------------
    - **** - Annotation                     No
    - **** - ArrayReflection                No
    - **** - ClassReflection                No
    - **** - Constructor                    No
    - **** - Field                          No
    - **** - Method                         No
    - **** - ReflectionException            No
    - **** - ReflectionPool                 No

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
    -      - XmlReader                      Convert
    -      - XmlWriter                      Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

