- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- DONE = Class finished but may not be fully 'CSharp-ified'
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- PRIORITY is 2D classes first
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- Restructure, where necessary, to cut the number of exceptions that can be thrown
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

CORE
----
    - DONE - AbstractGraphics
    - DONE - AbstractInput
    - DONE - ApplicationAdapter
    - DONE - Game
    - DONE - Gdx
    - DONE - IApplication
    - DONE - IApplicationListener
    - DONE - IApplicationLogger
    - DONE - IAudio
    - DONE - IFile
    - DONE - IGraphics
    - DONE - IInput
    - DONE - IInputProcessor
    - DONE - ILifecycleListener
    - DONE - INet
    - DONE - InputAdapter
    - DONE - InputEventQueue
    - DONE - IPreferences
    - DONE - IScreen
    - DONE - ScreenAdapter
    - DONE - Version

ASSETS
------
    -      - AssetDescriptor
    -      - AssetLoaderParameters
    -      - AssetLoadingTask
    -      - AssetManager
    -      - IAssetErrorListener
    -      - RefCountedContainer

ASSETS/LOADERS
--------------
    -      - AssetLoader
    -      - AsynchronousAssetLoader
    -      - BitmapFontLoader
    -      - CubemapLoader
    -      - I18NBundleLoader
    -      - IFileHandleResolver
    -      - ModelLoader
    -      - MusicLoader
    -      - ParticleEffectLoader
    -      - PixmapLoader
    -      - ShaderProgramLoader
    -      - SkinLoader
    -      - SoundLoader
    -      - SynchronousAssetLoader
    -      - TextureAtlasLoader
    -      - TextureLoader

ASSETS/LOADERS/RESOLVERS
------------------------
    -      - AbsoluteFileHandleResolver
    -      - ClasspathFileHandleResolver
    -      - ExternalFileHandleResolver
    -      - InternalFileHandleResolver
    -      - LocalFileHandleResolver
    -      - PrefixFileHandleResolver
    -      - ResolutionFileResolver

AUDIO
-----
    - DONE - IAudioDevice
    - DONE - IAudioRecorder
    - DONE - IMusic
    - DONE - ISound

FILES
-----
    -      - FileHandle
    -      - FileHandleStream

GRAPHICS
--------
    -      - BufferFormat               record class
    -      - Camera
    - DONE - Color
    -      - Colors
    -      - Cubemap
    -      - DisplayMode
    - DONE - FPSLogger
    -      - GLTexture
    -      - GraphicsType
    -      - ICubemapData
    -      - ICursor
    - DONE - IGL20
    - DONE - IGL30
    -      - ITextureData
    -      - Mesh
    -      - Monitor
    -      - OrthographicCamera
    -      - PerspectiveCamera
    -      - Pixmap
    -      - PixmapIO
    -      - Texture
    -      - TextureArray
    -      - TextureArrayData
    -      - TextureFilter
    -      - VertexAttribute
    -      - VertexAttributes

GRAPHICS/G2D
------------
    -      - Animation
    -      - BitmapFont
    -      - BitmapFontCache
    -      - CpuSpriteBatch
    -      - DistanceFieldFont
    -      - Gdx2DPixmap
    -      - GlyphLayout
    -      - IBatch
    -      - IPolygonBatch
    -      - NinePatch
    -      - ParticleEffect
    -      - ParticleEffectPool
    -      - ParticleEmitter
    -      - PixmapPacker
    -      - PixmapPacker10
    -      - PolygonRegion
    -      - PolygonRegionLoader
    -      - PolygonSprite
    -      - PolygonSpriteBatch
    -      - RepeatablePolygonSprite
    -      - Sprite
    -      - SpriteBatch
    -      - SpriteCache
    -      - TextureAtlas
    -      - TextureRegion

GRAPHICS/G3D
------------
    -      - 

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

GRAPHICS/PROFILING
------------------
    -      - GL20Interceptor
    -      - GL30Interceptor
    -      - GLErrorListener
    -      - GLInterceptor
    -      - GLProfiler

INPUT
-----
    -      - GestureDetector
    -      - RemoteInput
    -      - RemoteSender

MAPS
----
    -      - IImageResolver
    -      - IMapRenderer
    -      - Map
    -      - MapGroupLayer
    -      - MapLayer
    -      - MapLayers
    -      - MapObject
    -      - MapObjects
    -      - MapProperties

MAPS/OBJECTS
------------
    -      - CircleMapObject
    -      - EllipseMapObject
    -      - PolygonMapObject
    -      - PolylineMapObject
    -      - RectangleMapObject
    -      - TextureMapObject

MAPS/TILED
----------
    -      - ITiledMapTile
    -      - TiledMap
    -      - TiledMapImageLayer
    -      - TiledMapTileLayer
    -      - TiledMapTileSet
    -      - TiledMapTileSets

MAPS/TILED/LOADERS
------------------
    -      - AtlasTmxMapLoader
    -      - BaseTmxMapLoader
    -      - TmxMapLoader
    - ???? - TideMapLoader      Q: Should I support Tide?

MAPS/TILED/OBJECTS
------------------
    -      - TiledMapTileMapObject

MAPS/TILED/RENDERERS
--------------------
    -      - BatchTiledMapRenderer
    -      - HexagonalTiledMapRenderer
    -      - IsometricStaggeredTiledMapRenderer
    -      - IsometricTiledMapRenderer
    -      - ITiledMapRenderer
    -      - OrthoCachedTiledMapRenderer
    -      - OrthogonalTiledMapRenderer

MAPS/TILED/TILES
----------------
    -      - AnimatedTileMapTile
    -      - StaticTiledMapTile

MATH
----
    -      - Affine2
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
    -      - Matrix3
    -      - Matrix4
    -      - Plane
    -      - Polygon
    -      - Polyline
    -      - Quaternion
    -      - RandomXS128
    -      - Rectangle
    -      - Vector2                make struct
    -      - Vector3                make struct
    -      - WindowedMean

MATH/COLLISION
--------------
    -      - BoundingBox
    -      - Ray
    -      - Segment
    -      - Sphere

NETWORK
-------
    -      - HttpParametersUtils
    -      - HttpRequestBuilder
    -      - HttpStatus
    -      - IHttpRequestHeader
    -      - IHttpResponseHeader
    -      - IServerSocket
    -      - ISocket
    -      - NetJavaImpl
    -      - NetJavaServerSocketImpl
    -      - NetJavaSocketImpl
    -      - ServerSocketHints
    -      - SocketHints

SCENES.SCENE2D
--------------
    -      - Action
    -      - Actor
    -      - Event
    -      - Group
    -      - IEventListener
    -      - InputEvent
    -      - InputListener
    -      - Stage
    -      - Touchable

SCENES.SCENE2D/ACTIONS
----------------------
    -      - Actions
    -      - AddAction
    -      - AddListenerAction
    -      - AfterAction
    -      - AlphaAction
    -      - ColorAction
    -      - CountdownEventAction
    -      - DelayAction
    -      - DelegateAction
    -      - EventAction
    -      - FloatAction
    -      - IntAction
    -      - LayoutAction
    -      - MoveByAction
    -      - MoveToAction
    -      - ParallelAction
    -      - RelativeTemporalAction
    -      - RemoveAction
    -      - RemoveActorAction
    -      - RemoveListenerAction
    -      - RepeatAction
    -      - RotateByAction
    -      - RotateToAction
    -      - RunnableAction
    -      - ScaleByAction
    -      - ScaleToAction
    -      - SequenceAction
    -      - SizeByAction
    -      - SizeToAction
    -      - TemporalAction
    -      - TimeScaleAction
    -      - TouchableAction
    -      - VisibleAction

SCENES.SCENE2D/UI
-----------------
    -      - Button
    -      - ButtonGroup
    -      - Cell
    -      - CheckBox
    -      - Container
    -      - Dialog
    -      - HorizontalGroup
    -      - Image
    -      - ImageButton
    -      - ImageTextButton
    -      - Label
    -      - List
    -      - ParticleEffectActor
    -      - ProgressBar
    -      - ScrollPane
    -      - SelectBox
    -      - Skin
    -      - Slider
    -      - SplitPane
    -      - Stack
    -      - Table
    -      - TextArea
    -      - TextButton
    -      - TextField
    -      - TextTooltip
    -      - TooltipManager
    -      - Touchpad
    -      - Tree
    -      - Value
    -      - VerticalGroup
    -      - Widget
    -      - WidgetGroup
    -      - Window

SCENES.SCENE2D/UTILS
--------------------
    -      - ActorGestureListener
    -      - ArraySelection
    -      - BaseDrawable
    -      - ChangeListener
    -      - ClickListener
    -      - DragAndDrop
    -      - DragListener
    -      - DragScrollListener
    -      - FocusListener
    -      - ICullable
    -      - IDisableable
    -      - IDrawable
    -      - ILayout
    -      - ITransformDrawable
    -      - NinePatchDrawable
    -      - ScissorStack
    -      - Selection
    -      - SpriteDrawable
    -      - TextureRegionDrawable
    -      - TiledDrawable
    -      - UIUtils

UTILS
-----
    - DONE - Align
    -      - Base64Coder
    -      - BinaryHeap
    -      - Bits
    -      - Buffer
    -      - BufferUtils
    -      - IClipboard
    -      - ComparableTimSort
    -      - DataBuffer
    -      - DataInput
    -      - DataOutput
    - DONE - FloatConsts                Doner, but is it needed?
    -      - FlushablePool
    -      - GdxNativesLoader
    -      - I18NBundle
    -      - IllegalStateException
    -      - IntBuffer
    -      - IPoolable
    -      - IRunnable
    -      - LittleEndianInputStream
    -      - Logger
    -      - NumberUtils
    -      - PauseableThread
    -      - PerformanceCounter
    -      - PerformanceCounters
    -      - Pool
    -      - PooledLinkedList
    -      - Pools
    -      - PropertiesUtils
    -      - QuadTreeFloat
    -      - Queue
    -      - QuickSelect
    -      - Scaling
    -      - ScreenUtils
    -      - Selector (aka Libgdx Select)
    -      - SerializationException
    -      - SharedLibraryLoader
    -      - SortUtils
    -      - SortedIntList
    -      - StreamUtils
    -      - StringBuilder
    -      - TextFormatter
    -      - Timer                  Use System.Timer instead??
    -      - TimeUtils
    -      - TimSort

UTILS/ASYNC
-----------
    -      - AsyncExecutor
    -      - AsyncResult
    -      - IAsyncTask
    -      - ThreadUtils

UTILS/ATOMIC
------------
    -      - AtomicInteger
    -      - AtomicQueue
    -      - AtomicReferenceArray

UTILS/COLLECTIONS
-----------------
    -      - Array              Just use List<>?
    -      - ArrayIterable      Needed??
    -      - ArrayIterator      Needed??
    - DONE - ArrayMap                   Dictionary<K, V>
    - DONE - BooleanArray               List< bool >
    - DONE - ByteArray                  List< byte >
    - DONE - CharArray                  List< char >
    -      - Collections
    -      - DelayedRemovalArray
    - DONE - FloatArray                 List< float >
    -      - IdentityMap
    - DONE - IntArray                   List< int >
    - DONE - IntFloatMap                Dictionary< int, float >
    - DONE - IntIntMap                  Dictionary< int, int >
    - DONE - IntMap                     Dictionary< int, V >
    -      - IntSet
    -      - IPredicate
    - DONE - LongArray                  List< long >
    - DONE - LongMap                    Dictionary< long, V >
    -      - ObjectFloatMap
    -      - ObjectIntMap
    -      - ObjectLongMap
    -      - ObjectMap
    -      - ObjectSet
    -      - OrderedMap
    -      - OrderedSet
    -      - PredicateIterable
    -      - PredicateIterator
    -      - ShortArray                 List< short >
    -      - SnapshotArray

UTILS/COMPRESSION
-----------------
    -      - CRC                        Needed??
    -      - ICodeProgress              Needed??
    -      - LzmaAdapted                Needed??

    UTILS/COMPRESSION/LZ
    --------------------
            -      - BinTree            Needed??
            -      - InWindow           Needed??
            -      - OutWindow          Needed??
    
    UTILS/COMPRESSION/LZMA
    ----------------------
            -      - Base               Needed??
            -      - Decoder            Needed??
            -      - Encoder            Needed??
    
    UTILS/COMPRESSION/RANGECODER
    ----------------------------
            -      - BitTreeDecoder     Needed??
            -      - BitTreeEncoder     Needed??
            -      - Decoder            Needed??
            -      - Encoder            Needed??

UTILS/EXCEPTIONS
----------------
    - Done - GdxRuntimeException
    - Done - IllegalStateException
    - Done - NoSuchElementException
    - Done - NumberFormatException
    - Done - SerializationException

UTILS/JSON
----------
    -      - IBaseJsonReader            Needed??
    -      - Json                       Needed??
    -      - JsonReader                 Needed??
    -      - JsonValue                  Needed??
    -      - JsonWriter                 Needed??
    -      - UBJsonReader               Needed??
    -      - UBJsonWriter               Needed??

UTILS/REFLECT
-------------
    -      - Annotation                 Needed??
    -      - ArrayReflection            Needed??
    -      - ClassReflection            Needed??
    -      - Constructor                Needed??
    -      - Field                      Needed??
    -      - Method                     Needed??
    -      - ReflectionException        Needed??
    -      - ReflectionPool             Needed??

UTILS/VIEWPORT
--------------
    -      - ExtendedViewport
    -      - FillViewport
    -      - FitViewport
    -      - ScalingViewport
    -      - ScreenViewport
    -      - StretchViewport
    -      - Viewport

UTILS/XML
---------
    -      - XmlReader                  Use System.Xml.XmlReader instead
    -      - XmlWriter                  Use System.Xml.XmlWriter instead

