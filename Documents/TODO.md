LIBGDX CSHARP CONVERSION - ROUND 1
----------------------------------

ALL CLASSES WILL BE UP FOR MODIFICATION FOLLOWING TESTING.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- STEP 1: Complete all conversions so that the code will build.
- STEP 2: Refactor, where possible and necessary, to take advantage of C# language features.
    - Some classes can become structs / records instead.
    - Some/Most Get / Set method declarations in interfaces could become Properties.
    - switch expressions instead of switch statements where appropriate.
    - switch expressions instead of if...if/else...else where appropriate.
    - Check methods to see if they can be virtual.
    - Check and/or correct visibility of classes/methods/properties etc.
    - Use sealed classes only where strictly necessary.
    - Use of virtual for base classes/methods and classes that are likely to be extended is essential.
    - Constantly look for opportunities to improve this code.
- STEP 3: Resolve ALL remaining TODOs.
- STEP 4: Ensure code is fully documented.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- All uses of IRunnable.Runnable need checking and correcting.
-

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- NO MAGIC NUMBERS!!!
- SORT OUT VERSIONING!!!
- PRIORITY is 2D classes first

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- There seems to be different namings for width/height etc. properties and methods. Make it more uniform
- Make more use of `<inheritdoc cref=""/>` or just `<inheritdoc/>`

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- IP = Conversion In Progress.
- DONE = Class finished but may not be fully 'CSHARP-ified'
- First column is for Code, Second column is for Documentation.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS
------

    TODO:   Remove SynchronousAssetLoader and modify AssetLoader / AsynchronousAssetLoader
            to be just one loading system.

    FOR NOW: Just use basic file loading until the rest is working, then work on a proper
             asset loading and management system. This will allow testing of everything else.
             ( Don't forget Maps.Tiled.Loaders... )

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - AssetDescriptor
    - IP   - IP   - AssetLoaderParameters
    - IP   - IP   - AssetLoadingTask
    - IP   - IP   - AssetManager
    - IP   - IP   - IAssetErrorListener
    - IP   - IP   - RefCountedContainer

ASSETS/LOADERS
--------------

    TODO:   I think most, if not all, of these classes could be modified or rewritten.
            As I've learned more about C# I've also learned more about Tasks and Delegates,
            I think these classes could/should use these.

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - AssetLoader
    - IP   - IP   - AsynchronousAssetLoader
    - IP   - IP   - BitmapFontLoader
    - IP   - IP   - CubemapLoader
    - IP   - IP   - ModelLoader
    - IP   - IP   - MusicLoader
    - IP   - IP   - ParticleEffectLoader
    - IP   - IP   - PixmapLoader
    - IP   - IP   - ShaderProgramLoader
    - IP   - IP   - SkinLoader
    - IP   - IP   - SoundLoader
    - IP   - IP   - SynchronousAssetLoader
    - IP   - IP   - TextureAtlasLoader
    - IP   - IP   - TextureLoader

ASSETS/LOADERS/RESOLVERS
------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - AbsoluteFileHandleResolver
    - DONE - DONE - ClasspathFileHandleResolver
    - DONE - DONE - ExternalFileHandleResolver
    - DONE - DONE - IFileHandleResolver
    - DONE - DONE - InternalFileHandleResolver
    - DONE - DONE - LocalFileHandleResolver
    - DONE - DONE - PrefixFileHandleResolver
    - DONE - IP   - ResolutionFileResolver

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

AUDIO
-----

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - IAudioDevice
    - DONE - DONE - IAudioRecorder
    - DONE - DONE - IMusic
    - DONE - DONE - ISound

AUDIO/MP3SHARP
--------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Buffer16BitSterso
    - DONE - DONE - MP3SharpException
    - DONE - DONE - MP3Stream
    - DONE - DONE - SoundFormat

AUDIO/MP3SHARP/DECODING
-----------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - AudioBase
    - DONE - DONE - BitReserve
    - DONE - DONE - Bitstream
    - DONE - DONE - BitstreamErrors
    - DONE - IP   - BitstreamException
    - DONE - IP   - CircularByteBuffer
    - DONE - DONE - Crc16
    - DONE - IP   - Decoder
    - DONE - IP   - DecoderParameters
    - DONE - IP   - DecoderErrors
    - DONE - IP   - DecoderException
    - DONE - IP   - Equalizer
    - DONE - IP   - Header
    - DONE - IP   - Huffman
    - DONE - IP   - OutputChannels
    - DONE - IP   - OutputChannelsEnum
    - DONE - IP   - PushbackStream
    - DONE - IP   - SampleBuffer
    - DONE - IP   - SynthesisFilter

AUDIO/MP3SHARP/DECODING/DECODERS
-----------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - ASubband
    - DONE - IP   - IFrameDecoder
    - DONE - IP   - LayerIDecoder
    - DONE - IP   - LayerIIDecoder
    - DONE - IP   - LayerIIIDecoder

AUDIO/MP3SHARP/DECODING/DECODERS/LAYERI
---------------------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - SubbandLayer1
    - DONE - IP   - SubbandLayer1IntensityStereo
    - DONE - IP   - SubbandLayer1Stereo

AUDIO/MP3SHARP/DECODING/DECODERS/LAYERII
----------------------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - SubbandLayer2
    - DONE - IP   - SubbandLayer2IntensityStereo
    - DONE - IP   - SubbandLayer2Stereo

AUDIO/MP3SHARP/DECODING/DECODERS/LAYERIII
-----------------------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - ChannelData
    - DONE - IP   - GranuleInfo
    - DONE - IP   - Layer3SideInfo
    - DONE - IP   - SBI
    - DONE - IP   - ScaleFactorData
    - DONE - IP   - ScaleFactorTable

AUDIO/MP3SHARP/IO
-----------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - RandomAccessFileStream
    - DONE - IP   - RiffFile
    - DONE - IP   - WaveFile
    - DONE - IP   - WaveFileBuffer

AUDIO/MP3SHARP/SUPPORT
----------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - SupportClass

AUDIO/OPENAL
------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - AL
    - DONE - IP   - ALC

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

CORE
----

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - AbstractGraphics
    - DONE - IP   - AbstractInput
    - DONE - DONE - ApplicationAdapter
    - DONE - DONE - Game
    - DONE - DONE - Gdx
    - DONE - DONE - GDXVersion
    - DONE - DONE - IApplication
    - DONE - DONE - IApplicationListener
    - DONE - DONE - IAudio
    - DONE - DONE - IFiles
    - DONE - IP   - IGraphics
    - DONE - IP   - IInput
    - DONE - IP   - IInputProcessor
    - DONE - DONE - ILifecycleListener
    - DONE - IP   - INet
    - DONE - DONE - InputAdapter
    - DONE - IP   - InputEventQueue
    - DONE - IP   - InputMultiplexer
    - DONE - DONE - IPreferences
    - DONE - DONE - IScreen
    - DONE - DONE - ScreenAdapter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS
--------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Camera
    - DONE - IP   - Color
    - DONE - IP   - Colors
    - DONE - IP   - Cubemap
    - DONE - IP   - FPSLogger
    - DONE - IP   - GLTexture
    - DONE - IP   - ICubemapData
    - DONE - IP   - ICursor
    - DONE - IP   - IDownloadPixmapResponseListener
    - DONE - IP   - ITextureArrayData
    - DONE - IP   - ITextureData
    - DONE - IP   - Mesh
    - DONE - IP   - OrthographicCamera
    - DONE - IP   - PerspectiveCamera
    - DONE - IP   - Pixmap
    - DONE - IP   - PixmapFormat
    - DONE - IP   - PixmapIO                            Tests needed
    - DONE - IP   - Texture
    - DONE - IP   - TextureArray
    - DONE - IP   - TextureFilter
    - DONE - IP   - TextureWrap
    - DONE - IP   - VertexAttribute
    - DONE - IP   - VertexAttributes

    - GameWindow ??
    - GraphicsDevice ??

GRAPHICS/FRAMEBUFFERS
---------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - FloatFrameBuffer
    - DONE - IP   - FloatFrameBufferBuilder
    - DONE - IP   - FrameBuffer
    - DONE - IP   - FrameBufferBuilder
    - DONE - IP   - FrameBufferCubemap
    - DONE - IP   - FrameBufferCubemapBuilder
    - DONE - IP   - FrameBufferRenderBufferAttachmentSpec
    - DONE - IP   - FrameBufferTextureAttachmentSpec
    - DONE - IP   - GLFrameBuffer
    - DONE - IP   - GLFrameBufferBuilder

    ** Restructure GLFrameBuffer.Build(), this method is too long.

GRAPHICS/G2D
------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Animation
    - DONE - IP   - AtlasRegion
    - DONE - IP   - AtlasSprite                     Done but I've spotted some possible updates
    - IP   - IP   - BitmapFont                      Much work needed!
    - IP   - IP   - BitmapFontCache                 Relies on the BitmapFont rewrite.
    - IP   - IP   - CpuSpriteBatch                  Some methods have too many parameters
    - DONE - IP   - DistanceFieldFont
    - DONE - IP   - Gdx2DPixmap
    - IP   - IP   - GlyphLayout                     Method SetText() is too big.
    - DONE - DONE - IBatch                          Some methods have too many parameters
    - DONE - DONE - IPolygonBatch
    - DONE - IP   - NinePatch
    - DONE - IP   - ParticleEffect
    - DONE - IP   - ParticleEffectPool
    - DONE - IP   - ParticleEmitter
    - IP   - IP   - PixmapPacker                    Strategy classes need completing
    - DONE - IP   - PixmapPackerIO
    - DONE - IP   - PolygonRegion
    - DONE - IP   - PolygonRegionLoader
    - DONE - IP   - PolygonSprite
    - DONE - IP   - PolygonSpriteBatch
    - DONE - IP   - RepeatablePolygonSprite
    - DONE - IP   - Sprite
    - DONE - IP   - SpriteBatch                     Some methods have too many parameters
    - DONE - IP   - SpriteCache
    - DONE - IP   - TextureAtlas
    - DONE - IP   - TextureAtlasData
    - DONE - IP   - TextureAtlasDataExtensions
    - DONE - IP   - TextureRegion

GRAPHICS/G3D
------------

    See Documents/TODO_G3D.MD

GRAPHICS/GLUTILS
----------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - ETC1
    - IP   - IP   - ETC1TextureData
    - IP   - IP   - FacedCubemapData
    - IP   - IP   - FileTextureArrayData
    - DONE - IP   - FileTextureData
    - DONE - IP   - FloatTextureData
    - DONE - IP   - GLOnlyTextureData
    - IP   - IP   - GLVersion
    - DONE - DONE - HdpiMode
    - DONE - DONE - HdpiUtils
    - IP   - IP   - IImmediateModeRenderer
    - DONE - DONE - IIndexData
    - DONE - IP   - IInstanceData
    - IP   - IP   - ImmediateModeRenderer20
    - IP   - IP   - IndexArray
    - IP   - IP   - IndexBufferObject
    - IP   - IP   - IndexBufferObjectSubData
    - IP   - IP   - InstanceBufferObject
    - IP   - IP   - InstanceBufferObjectSubData
    - DONE - DONE - IVertexData
    - IP   - IP   - KTXTTextureData
    - IP   - IP   - MipMapGenerator
    - DONE - IP   - MipMapTextureData
    - DONE - IP   - PixmapTextureData
    - DONE - IP   - ShaderProgram
    - DONE - IP   - ShapeRenderer
    - DONE - DONE - VertexArray
    - DONE - IP   - VertexBufferObject
    - IP   - IP   - VertexBufferObjectSubData
    - IP   - IP   - VertexBufferObjectWithVAO

    - The following do not need converting

GRAPHICS/OPENGL
---------------

    Remove the need for so much use of fixed() in code by implementing method overrides,
    and doing the work inside those methods instead.

    Work on reducing the amount of casting to uint.

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - GLBindings
    - IP   - IP   - GLSetup
    - IP   - IP   - IGL

GRAPHICS/PROFILING
------------------

    These are profiling classes only. If adding GL/Glfw breaks these update them later on.

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - BaseGLInterceptor
    -      -      - GLInterceptor       Replace GL20Interceptor & GL30Interceptor with this.
    - IP   - IP   - GL20Interceptor
    - IP   - IP   - GL30Interceptor
    - IP   - IP   - GLProfiler
    - IP   - IP   - IGLErrorListener

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

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - IImageResolver
    - DONE - IP   - IMapRenderer
    - DONE - IP   - Map
    - DONE - IP   - MapGroupLayer
    - DONE - IP   - MapLayer
    - DONE - IP   - MapLayers
    - DONE - IP   - MapObject
    - DONE - IP   - MapObjects
    - DONE - IP   - MapProperties

MAPS/OBJECTS
------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - CircleMapObject
    - DONE - DONE - EllipseMapObject
    - DONE - IP   - PolygonMapObject
    - DONE - DONE - PolylineMapObject
    - DONE - DONE - RectangleMapObject
    - DONE - IP   - TextureMapObject

MAPS/TILED
----------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - ITiledMapTile
    - DONE - IP   - TiledMap
    - DONE - IP   - TiledMapImageLayer
    - DONE - IP   - TiledMapTileLayer
    - DONE - IP   - TiledMapTileSet
    - DONE - IP   - TiledMapTileSets

MAPS/TILED/LOADERS
------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - AtlasTmxMapLoader
    - DONE - IP   - BaseTmxMapLoader
    - DONE - IP   - TmxMapLoader

MAPS/TILED/OBJECTS
------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - TiledMapTileMapObject

MAPS/TILED/RENDERERS
--------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - BatchTiledMapRenderer
    - DONE - IP   - HexagonalTiledMapRenderer
    - DONE - IP   - IsometricStaggeredTiledMapRenderer
    - DONE - IP   - IsometricTiledMapRenderer
    - DONE - IP   - ITiledMapRenderer
    - DONE - IP   - OrthoCachedTiledMapRenderer
    - DONE - IP   - OrthogonalTiledMapRenderer

MAPS/TILED/TILES
----------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - AnimatedTileMapTile
    - DONE - IP   - StaticTiledMapTile

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATHS
-----

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Affine2
    - DONE - IP   - Bezier
    - DONE - IP   - Bresenham2
    - DONE - IP   - BSpline
    - DONE - IP   - CatmullRomSpline
    - DONE - DONE - Circle
    - DONE - IP   - ConvexHull
    - DONE - IP   - CumulativeDistribution
    - IP   - IP   - DelaunayTriangulator        Unsure about method ComputeTriangles()
    - IP   - IP   - EarClippingTriangulator     Needs some testing
    - DONE - DONE - Ellipse
    - DONE - IP   - FloatCounter
    - DONE - IP   - Frustrum
    - DONE - IP   - GeometryUtils
    - DONE - IP   - GridPoint2
    - DONE - IP   - GridPoint3
    - DONE - IP   - Interpolation
    - DONE - IP   - Intersector
    - DONE - IP   - IPath
    - DONE - DONE - IShape2D
    - DONE - IP   - IVector
    - DONE - IP   - MathUtils
    - DONE - IP   - Matrix3
    - DONE - IP   - Matrix4
    - DONE - IP   - Number
    - DONE - IP   - NumberUtils
    - DONE - IP   - Plane
    - DONE - IP   - Polygon
    - DONE - IP   - Polyline
    - DONE - IP   - Quaternion
    - DONE - IP   - RandomXS128
    - DONE - IP   - RectangleShape
    - DONE - IP   - Vector2                        Convert, I prefer the way the LibGDX class works.
    - DONE - IP   - Vector3                        Convert, I prefer the way the LibGDX class works.
    - DONE - IP   - WindowedMean

MATH/COLLISION
--------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - BoundingBox
    - DONE - DONE - Ray
    - DONE - IP   - Segment
    - DONE - IP   - Sphere

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES/SCENE2D
--------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Action
    - DONE - DONE - Actor
    - DONE - DONE - Event
    - DONE - DONE - Group
    - DONE - DONE - IActor
    - DONE - DONE - InputEvent
    - DONE - DONE - Stage
    - DONE - DONE - Touchable

SCENES/SCENE2D/ACTIONS
----------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - Actions
    - DONE - IP   - AddAction
    - DONE - IP   - AddListenerAction
    - DONE - IP   - AfterAction
    - DONE - IP   - AlphaAction
    - DONE - IP   - ColorAction
    - DONE - IP   - CountdownEventAction
    - DONE - IP   - DelayAction
    - DONE - IP   - DelegateAction
    - DONE - IP   - EventAction
    - DONE - IP   - FloatAction
    - DONE - IP   - IntAction
    - DONE - IP   - LayoutAction
    - DONE - IP   - MoveByAction
    - DONE - IP   - MoveToAction
    - DONE - IP   - ParallelAction
    - DONE - IP   - RelativeTemporalAction
    - DONE - IP   - RemoveAction
    - DONE - IP   - RemoveActorAction
    - DONE - IP   - RemoveListenerAction
    - DONE - IP   - RepeatAction
    - DONE - IP   - RotateByAction
    - DONE - IP   - RotateToAction
    - DONE - IP   - RunnableAction
    - DONE - IP   - ScaleByAction
    - DONE - IP   - ScaleToAction
    - DONE - IP   - SequenceAction
    - DONE - IP   - SizeByAction
    - DONE - IP   - SizeToAction
    - DONE - DONE - TemporalAction
    - DONE - DONE - TimeScaleAction
    - DONE - DONE - TouchableAction
    - DONE - DONE - VisibleAction

SCENES/SCENE2D/LISTENERS
------------------------

    - Base classes to be used and extended as required

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - ActorGestureListener
    - IP   - IP   - ChangeListener
    - IP   - IP   - ClickListener
    - IP   - IP   - DragListener
    - IP   - IP   - DragScrollListener
    - IP   - IP   - FocusListener
    - IP   - IP   - IEventListener
    - IP   - IP   - InputListener

```
TODO: Use Lambdas for these, i.e.

AddListener( new ClickListener()
{
    // Clicked needs to be a Func<>
    Clicked = ( ev, x, y ) =>
    {
    }
} );
```

SCENES/SCENE2D/UI
-----------------

    TODO: I don't like the way Cell and Value classes are implemented.
          They seem confusing and are candidates for a rewrite.

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - Button
    - IP   - IP   - ButtonGroup
    - IP   - IP   - Cell
    - IP   - IP   - CheckBox
    - IP   - IP   - Container
    - IP   - IP   - Dialog
    - IP   - IP   - DialogChangeListener
    - IP   - IP   - DialogFocusListener
    - IP   - IP   - DialogInputListener
    - IP   - IP   - HorizontalGroup
    - IP   - IP   - Image
    - IP   - IP   - ImageButton
    - IP   - IP   - ImageTextButton
    - DONE - DONE - IOnScreenKeyboard
    - IP   - IP   - Label
    - IP   - IP   - ListBox
    - IP   - IP   - ParticleEffectActor
    - IP   - IP   - ProgressBar
    - IP   - IP   - ScrollPane
    - IP   - IP   - ScrollPaneListeners
    - DONE - IP   - SelectBox
    - IP   - IP   - Skin                    Needs Json updates
    - DONE - IP   - Slider
    - DONE - IP   - SplitPane
    - DONE - IP   - Stack
    - DONE - IP   - Table
    - DONE - IP   - TextArea
    - DONE - IP   - TextButton
    - DONE - IP   - TextField
    - DONE - IP   - TextTooltip
    - DONE - IP   - Tooltip
    - DONE - IP   - TooltipManager
    - DONE - IP   - Touchpad
    - DONE - IP   - Tree
    - DONE - IP   - Value
    - DONE - IP   - VerticalGroup
    - DONE - IP   - Widget
    - DONE - IP   - WidgetGroup
    - DONE - IP   - Window

SCENES/SCENE2D/UTILS
--------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - ArraySelection
    - IP   - IP   - BaseDrawable
    - IP   - IP   - DragAndDrop
    - IP   - IP   - ICullable
    - IP   - IP   - IDisableable
    - IP   - IP   - IDrawable
    - IP   - IP   - ILayout
    - IP   - IP   - ITransformDrawable
    - IP   - IP   - NinePatchDrawable
    - IP   - IP   - ScissorStack
    - IP   - IP   - Selection
    - IP   - IP   - SpriteDrawable
    - IP   - IP   - TextureRegionDrawable
    - IP   - IP   - TiledDrawable
    - IP   - IP   - UIUtils

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS
-----

    - Move Utils/Collections and Utils/Viewport out of Utils and into somewhere more appropriate.

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Align
    - DONE - IP   - BinaryHeap
    - DONE - DONE - Bits
    - DONE - IP   - ByteOrder                       Is this still needed?
    - DONE - IP   - Character                       C# System MUST already have this???
    - DONE - IP   - ComparableTimSort
    - DONE - IP   - DataInput                       Check
    - DONE - IP   - DataOutput                      Check
    - DONE - IP   - DataUtils                       Added Class
    - DONE - DONE - FloatConstants
    - DONE - DONE - GdxRuntimeException
    - DONE - IP   - GdxSystem
    - DONE - DONE - IClipboard                      Convert - Interface, clipboard handled in backends.                 
    - DONE - DONE - ICloseable
    - DONE - DONE - IReadable
    - DONE - IP   - IRunnable                       Done, but is it needed?
    - DONE - IP   - Logger                          Needs documenting.
    - DONE - IP   - PerformanceCounter              Check
    - DONE - IP   - PerformanceCounters             Check
    - DONE - IP   - PropertiesUtils                 Convert, but check if necessary first
    - DONE - IP   - QuadTreeFloat
    - DONE - IP   - QuickSelect
    - DONE - IP   - Scaling
    - DONE - IP   - ScreenUtils
    - DONE - IP   - Select                          Renamed Selector (See SortedSetExtensions.cs)
    - DONE - IP   - SortUtils
    - DONE - DONE - StringTokenizer
    - DONE - DONE - Timer
    - DONE - DONE - TimeUtils
    - DONE - IP   - TimSort

    - The following do not need converting

    - **** - Base64Coder                    Use System.Convert classes
    - **** - GdxNativesLoader
    - **** - I18NBundle
    - **** - LittleEndianInputStream
    - **** - PauseableThread
    - **** - Queue                          Use system.collection.generics.Queue<T>?
    - **** - SharedLibraryLoader
    - **** - SortedIntList                  Use SortedList<int>
    - **** - StreamUtils
    - **** - StringBuilder                  Use System.Text.StringBuilder
    - **** - TextFormatter

UTILS/BUFFERS
-------------

     CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Buffer
    - IP   - IP   - BufferUtils
    - DONE - DONE - ByteBuffer
    - DONE - IP   - CharBuffer
    - DONE - IP   - CircularByteBuffer
    - IP   - IP   - DirectByteBuffer
    - DONE - DONE - DoubleBuffer
    - DONE - IP   - FloatBuffer
    - IP   - IP   - IDirectBuffer
    - DONE - IP   - IntBuffer
    - IP   - IP   - LongBuffer
    - IP   - IP   - MappedByteBuffer
    - DONE - IP   - ShortBuffer
    - DONE - IP   - StringCharBuffer

    - IP   - IP   - HeapByteBuffer
    - IP   - IP   - HeapCharBuffer
    - DONE - DONE - HeapDoubleBuffer
    - IP   - IP   - HeapFloatBuffer
    - DONE - IP   - HeapIntBuffer
    - DONE - IP   - HeapShortBuffer

UTILS/COLLECTIONS
-----------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Array<T>                       Converted, but use List<T> for most cases.
    - DONE - DONE - CollectionsData
    - DONE - IP   - DelayedRemovalArray            Convert / Extend List<>
    - DONE - IP   - IdentityMap<K, V>              Convert / extend Dictionary< object, object > 
    - DONE - IP   - IPredicate
    - DONE - IP   - ObjectMap                      Converted, but use Dictionary< object, object >
    - DONE - IP   - PredicateIterable
    - DONE - IP   - PredicateIterator
    - DONE - IP   - SnapshotArray<T>

    - The following do not need converting

    - **** - ArrayMap                       -> Use Dictionary<K, V>
    - **** - ArrayIterable                  -> Use IEnumerable?
    - **** - ArrayIterator                  -> Use IEnumerator?
    - **** - BoolArray                      -> Use List< bool >
    - **** - ByteArray                      -> Use List< byte >
    - **** - CharArray                      -> Use List< char >
    - **** - FloatArray                     -> Use List< float >
    - **** - IntArray                       -> Use List< int >
    - **** - IntFloatMap                    -> Use Dictionary< int, float >
    - **** - IntIntMap                      -> Use Dictionary< int, int >
    - **** - IntMap                         -> Use Dictionary< int, V >
    - **** - IntSet                         -> Use List<int>
    - **** - LongArray                      -> Use List< long >
    - **** - LongMap                        -> Use Dictionary< long, V >
    - **** - ObjectFloatMap                 -> Use Dictionary< object, float >
    - **** - ObjectIntMap                   -> Use Dictionary< object, int >
    - **** - ObjectLongMap                  -> Use Dictionary< object, long >
    - **** - ObjectSet<T>                   -> Use List<T>
    - **** - OrderedMap<K, V>               -> Use SortedDictionary<K, V> ?
    - **** - OrderedSet<T>                  -> Use SortedSet<T>
    - **** - ShortArray                     -> Use List< short >

UTILS/COLLECTIONS/EXTENSIONS
----------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - DictionaryExtensions
    - DONE - DONE - ListExtensions

UTILS/JSON
----------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - AbstractWriter
    - IP   - IP   - Json
    - IP   - IP   - JsonReader
    - IP   - IP   - JsonValue               Iterator needs finishing
    - DONE - IP   - JsonWriter

UTILS/POOLING
-------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - FlushablePool
    - DONE - DONE - IPoolable
    - DONE - DONE - Pool
    - DONE - DONE - PooledLinkedList
    - DONE - DONE - Pools

UTILS/VIEWPORT
--------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - ExtendedViewport
    - DONE - DONE - FillViewport
    - DONE - DONE - FitViewport
    - DONE - DONE - ScalingViewport
    - DONE - DONE - ScreenViewport
    - DONE - DONE - StretchViewport
    - DONE - DONE - Viewport

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

BACKENDS/DESKTOPGL
------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - DesktopGLApplication
    - IP   - IP   - DesktopGLApplicationConfiguration
    - IP   - IP   - DesktopGLNativesLoader
    - IP   - IP   - DesktopGLNet
    - IP   - IP   - DesktopWindowCallbacks
    - IP   - IP   - IDesktopGLApplicationBase
    - IP   - IP   - Sync

BACKENDS/DESKTOPGL/AUDIO
------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - GdxSoundAudioRecorder
    - IP   - IP   - IGLAudio
    - IP   - IP   - Mp3
    - IP   - IP   - Ogg
    - IP   - IP   - OggInputStream
    - IP   - IP   - OpenALAudio
    - IP   - IP   - OpenALAudioDevice
    - IP   - IP   - OpenALMusic
    - IP   - IP   - OpenALSound
    - IP   - IP   - Wav

BACKENDS/DESKTOPGL/AUDIO/MOCK
-----------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - MockAudio
    - DONE - IP   - MockAudioDevice
    - DONE - IP   - MockAudioRecorder
    - DONE - IP   - MockMusic
    - DONE - IP   - MockSound

BACKENDS/DESKTOPGL/FILES
------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - DesktopGLFileHandle
    - IP   - IP   - DesktopGLFiles

BACKENDS/DESKTOPGL/GRAPHICS
---------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - DesktopGL20
    - IP   - IP   - DesktopGL30
    - IP   - IP   - DesktopGLGraphics
    - IP   - IP   - DesktopGLGraphics.DesktopGLDisplayMode   Delete candidate
    - IP   - IP   - DesktopGLGraphics.DesktopGLMonitor       Delete candidate

BACKENDS/DESKTOPGL/INPUT
------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - DefaultGLInput
    - IP   - IP   - IDesktopGLInput

BACKENDS/DESKTOPGL/UTILS
------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - DesktopGLApplicationLogger
    - IP   - IP   - DesktopGLClipboard
    - IP   - IP   - DesktopGLCursor
    - IP   - IP   - DesktopGLPreferences

BACKENDS/DESKTOPGL/WINDOW
-------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - DesktopGLWindow
    - IP   - IP   - DesktopGLWindowConfiguration
    - IP   - IP   - DesktopGLWindowAdapter
    - IP   - IP   - IDesktopGLWindowListener

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/BOX2D
----------------

    - It's most likely best to recommend the use of an already available C# port.
    - ( It really depends on how much of a glutton for punishment I am!!!! )

    eg:
    - Box2DSharp
    - Box2DX
    - Box2D.Net
    - Box2D.CSharp

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/GDX-TOOLS
--------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - FileProcessor

EXTENSIONS/GDX-TOOLS/IMAGEPACKER
--------------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - ImagePacker

EXTENSIONS/GDX-TOOLS/TILEDMAPPACKER
-----------------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - TileMapPacker
    -      -      - TiledMapPackerTest
    -      -      - TiledMapPackerTestRender
    -      -      - TileSetLayout

EXTENSIONS/GDX-TOOLS/TOOLS
--------------------------

EXTENSIONS/GDX-TOOLS/TOOLS/TEXTUREPACKER
----------------------------------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - ColorBleedEffect
    -      -      - TexturePacker

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

TESTS
-----

    - IP   - IP   - Core/Graphics/CameraTest
    -      -      - Core/Graphics/PixmapIOTest
    -      -      - Core/Graphics/SpriteTest
    - IP   - IP   - Core/Scene/Scene2D/StageTest

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

