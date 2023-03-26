- IP = In Progress
- - - - - - - - -
- DONE = Class finished but may not be fully 'CSharp-ified'
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- PRIORITY is 2D classes first
- - - - - - - - - - - - - - - -
- Restructure, where necessary, to cut the number of exceptions that can be thrown
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

CORE
----
    - DONE - AbstractGraphics
    - DONE - AbstractInput
    - DONE - Application
    - DONE - ApplicationAdapter
    - DONE - Game
    - DONE - Gdx
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
    - DONE - AssetDescriptor
    - DONE - AssetLoaderParameters
    - IP   - AssetLoadingTask
    - IP   - AssetManager
    - DONE - IAssetErrorListener
    - DONE - RefCountedContainer

ASSETS/LOADERS
--------------
    - IP   - AssetLoader
    - DONE - AsynchronousAssetLoader
    - IP   - BitmapFontLoader
    -      - CubemapLoader
    -      - I18NBundleLoader
    - DONE - IFileHandleResolver
    -      - ModelLoader
    -      - MusicLoader
    -      - ParticleEffectLoader
    -      - PixmapLoader
    -      - ShaderProgramLoader
    -      - SkinLoader
    -      - SoundLoader
    - DONE - SynchronousAssetLoader
    -      - TextureAtlasLoader
    - IP   - TextureLoader

ASSETS/LOADERS/RESOLVERS
------------------------
    - DONE - AbsoluteFileHandleResolver
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
    - DONE - FileHandle
    - DONE - FileHandleStream

GRAPHICS
--------
    - DONE - BufferFormat
    - DONE - Camera
    - DONE - Color
    - DONE - Colors
    -      - Cubemap
    - DONE - DisplayMode
    -      - FPSLogger
    -      - GLTexture
    - DONE - GraphicsType
    -      - ICubemapData
    - DONE - ICursor
    - IP   - IGL20
    -      - IGL30
    - IP   - ITextureData
    -      - Mesh
    - DONE - Monitor
    -      - OrthographicCamera
    -      - PerspectiveCamera
    - IP   - Pixmap
    -      - PixmapIO
    - IP   - Texture
    -      - TextureArray
    -      - TextureArrayData
    - DONE - TextureFilter
    -      - VertexAttribute
    -      - VertexAttributes

GRAPHICS/G2D
------------
    -      - Animation
    - IP   - BitmapFont
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
    - IP   - TextureRegion

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
    - IP   - IImageResolver
    - IP   - IMapRenderer
    - IP   - Map
    - DONE - MapGroupLayer
    - IP   - MapLayer
    - DONE - MapLayers
    - DONE - MapObject
    - DONE - MapObjects
    - DONE - MapProperties

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
    - IP   - ITiledMapTile
    - DONE - TiledMap
    - DONE - TiledMapImageLayer
    - DONE - TiledMapTileLayer
    - DONE - TiledMapTileSet
    - IP   - TiledMapTileSets

MAPS/TILED/LOADERS
------------------
    -      - AtlasTmxMapLoader
    - IP   - BaseTmxMapLoader
    - IP   - TmxMapLoader
    - ???? - TideMapLoader      Q: Should I support Tide?

MAPS/TILED/OBJECTS
------------------
    -      - TiledMapTileMapObject

MAPS/TILED/RENDERERS
--------------------
    - IP   - BatchTiledMapRenderer
    -      - HexagonalTiledMapRenderer
    -      - IsometricStaggeredTiledMapRenderer
    -      - IsometricTiledMapRenderer
    -      - ITiledMapRenderer
    -      - OrthoCachedTiledMapRenderer
    - IP   - OrthogonalTiledMapRenderer

MAPS/TILED/TILES
----------------
    -      - AnimatedTileMapTile
    -      - StaticTiledMapTile

MATH
----
    - DONE - Affine2
    - DONE - Bezier
    -      - Bresenham
    -      - BSpline
    -      - CatmullRomSpline
    - DONE - Circle
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
    - DONE - IPath
    - DONE - IShape2D
    - DONE - IVector
    - DONE - MathUtils
    - IP   - Matrix3
    - IP   - Matrix4
    -      - Plane
    -      - Polygon
    -      - Polyline
    -      - Quaternion
    -      - RandomXS128
    - DONE - Rectangle
    - DONE - Vector2
    - DONE - Vector3        ( Needs documentation comments )
    - DONE - WindowedMean

MATH/COLLISION
--------------
    - DONE - BoundingBox
    - DONE - Ray
    - DONE - Segment
    - DONE - Sphere

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
    - DONE - Action
    - IP   - Actor
    - DONE - Event
    -      - Group
    - DONE - IEventListener
    -      - InputEvent
    -      - InputListener
    - IP   - Stage
    - DONE - Touchable

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
    - IP   - ScissorStack
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
    - DONE - Bits
    - IP   - Buffer
    -      - BufferUtils
    -      - IClipboard
    -      - ComparableTimSort
    -      - DataBuffer
    -      - DataInput
    -      - DataOutput
    - DONE - FloatConsts
    -      - FlushablePool
    -      - GdxNativesLoader
    - DONE - GdxRuntimeException
    -      - I18NBundle
    - DONE - IllegalStateException
    -      - IntBuffer
    - IP   - IPoolable
    - DONE - IRunnable
    -      - LittleEndianInputStream
    -      - Logger
    - DONE - NumberUtils
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
    -      - ReflectionPool
    -      - Scaling
    -      - ScreenUtils
    - IP   - Selector (aka Libgdx Select)
    -      - SerializationException
    -      - SharedLibraryLoader
    -      - Sort
    -      - SortedIntList
    -      - StreamUtils
    - IP   - StringBuilder
    -      - TextFormatter
    -      - Timer
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
    - IP   - Array
    - IP   - ArrayIterable
    - IP   - ArrayIterator
    - IP   - ArrayMap
    -      - BooleanArray
    -      - ByteArray
    -      - CharArray
    - DONE - Collections
    - IP   - DelayedRemovalArray
    -      - FloatArray
    -      - IdentityMap
    -      - IntArray
    -      - IntFloatMap
    -      - IntIntMap
    -      - IntMap
    -      - IntSet
    - IP   - IPredicate
    -      - LongArray
    -      - LongMap
    -      - ObjectFloatMap
    -      - ObjectIntMap
    -      - ObjectLongMap
    - IP   - ObjectMap
    -      - ObjectSet
    -      - OrderedMap
    -      - OrderedSet
    - IP   - PredicateIterable
    - IP   - PredicateIterator
    -      - ShortArray
    - IP   - SnapshotArray

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

UTILS/VIEWPORT
--------------
    -      - ExtendedViewport
    -      - FillViewport
    -      - FitViewport
    -      - ScalingViewport
    -      - ScreenViewport
    -      - StretchViewport
    - DONE - Viewport

UTILS/XML
---------
    -      - XmlReader                  Use System.Xml.XmlReader instead
    -      - XmlWriter                  Use System.Xml.XmlWriter instead

