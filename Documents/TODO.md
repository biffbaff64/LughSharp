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

RULES TO FOLLOW
---------------

- Methods like **Dispose(), ToString(), Equals(), GetHashCode()** should be positioned at the
  END of source files.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- All uses of IRunnable.Runnable need checking and correcting.

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
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS
------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - AssetDescriptor
    - DONE - DONE - AssetLoaderParameters
    - DONE - DONE - AssetLoadingTask
    - DONE - DONE - AssetManager
    - DONE - DONE - IAssetErrorListener
    - DONE - DONE - RefCountedContainer

ASSETS/LOADERS
--------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - AssetLoader
    - DONE - DONE - AsynchronousAssetLoader
    - DONE - DONE - BitmapFontLoader
    - DONE - DONE - CubemapLoader
    - DONE - DONE - ModelLoader
    - DONE - DONE - MusicLoader
    - DONE - DONE - ParticleEffectLoader
    - DONE - DONE - PixmapLoader
    - DONE - DONE - ShaderProgramLoader
    - DONE - DONE - SkinLoader
    - DONE - DONE - SoundLoader
    - DONE - DONE - SynchronousAssetLoader
    - DONE - DONE - TextureAtlasLoader
    - DONE - DONE - TextureLoader

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
    - DONE - DONE - ResolutionFileResolver

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- **I'm currently considering ditching Lugh.Audio in favour of NAudio.**
- **Decision to be made asap.**

AUDIO
-----

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - IAudioDevice
    - DONE - DONE - IAudioRecorder
    - DONE - DONE - IMusic
    - DONE - DONE - ISound

AUDIO/MAPONUS
-------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Buffer16BitSterso
    - DONE - DONE - MP3SharpException
    - DONE - DONE - MP3Stream
    - DONE - DONE - SoundFormat

AUDIO/MAPONUS/DECODING
-----------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - AudioBase
    - DONE - DONE - BitReserve
    - DONE - DONE - Bitstream
    - DONE - DONE - BitstreamErrors
    - DONE - DONE - BitstreamException
    - DONE - DONE - CircularByteBuffer
    - DONE - DONE - Crc16
    - DONE - DONE - Decoder
    - DONE - DONE - DecoderParameters
    - DONE - DONE - DecoderErrors
    - DONE - DONE - DecoderException
    - DONE - IP   - Equalizer
    - DONE - IP   - Header
    - DONE - IP   - Huffman
    - DONE - DONE - OutputChannels
    - DONE - DONE - PushbackStream
    - DONE - DONE - SampleBuffer
    - DONE - IP   - SynthesisFilter

AUDIO/MAPONUS/DECODING/DECODERS
-----------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - ASubband
    - DONE - DONE - IFrameDecoder
    - DONE - DONE - LayerIDecoder
    - DONE - DONE - LayerIIDecoder
    - DONE - IP   - LayerIIIDecoder

AUDIO/MAPONUS/DECODING/DECODERS/LAYERI
---------------------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - SubbandLayer1
    - DONE - IP   - SubbandLayer1IntensityStereo
    - DONE - IP   - SubbandLayer1Stereo

AUDIO/MAPONUS/DECODING/DECODERS/LAYERII
----------------------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - SubbandLayer2
    - DONE - IP   - SubbandLayer2IntensityStereo
    - DONE - IP   - SubbandLayer2Stereo

AUDIO/MAPONUS/DECODING/DECODERS/LAYERIII
-----------------------------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - ChannelData
    - DONE - IP   - GranuleInfo
    - DONE - IP   - Layer3SideInfo
    - DONE - IP   - SBI
    - DONE - IP   - ScaleFactorData
    - DONE - IP   - ScaleFactorTable

AUDIO/MAPONUS/IO
-----------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - RandomAccessFileStream
    - DONE - IP   - RiffFile
    - DONE - IP   - WaveFile
    - DONE - IP   - WaveFileBuffer

AUDIO/MAPONUS/SUPPORT
----------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - SupportClass

AUDIO/OPENAL
------------

    CODE   DOCUMENT
    ----   --------
    - DONE - **** - AL
    - DONE - **** - ALC

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

FILES
-----

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - FileHandle
    - IP   - IP   - InputStream

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

CORE
----

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - AbstractGraphics
    - DONE - DONE - AbstractInput
    - DONE - DONE - ApplicationAdapter
    - DONE - DONE - Game
    - DONE - DONE - Gdx
    - DONE - DONE - GDXVersion
    - DONE - DONE - IApplication
    - DONE - DONE - IApplicationListener
    - DONE - DONE - IAudio
    - DONE - DONE - IFiles
    - DONE - DONE - IGraphics
    - DONE - DONE - IGraphics.BufferFormatDescriptor
    - DONE - DONE - IGraphics.DisplayMode
    - DONE - DONE - IGraphics.GdxMonitor
    - DONE - DONE - IInput
    - DONE - DONE - IInputProcessor
    - DONE - DONE - ILifecycleListener
    - DONE - DONE - INet
    - DONE - DONE - InputAdapter
    - DONE - DONE - InputEventQueue
    - DONE - DONE - InputMultiplexer
    - DONE - DONE - IPreferences
    - DONE - DONE - IScreen
    - DONE - DONE - Platform
    - DONE - DONE - ScreenAdapter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS
--------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - BMPFormatStructs
    - DONE - DONE - Color
    - DONE - DONE - Colors
    - DONE - DONE - Cubemap
    - DONE - DONE - FPSLogger
    - DONE - IP   - GLTexture
    - DONE - IP   - GraphicsBackend
    - DONE - DONE - GStructs
    - DONE - DONE - ICubemapData
    - DONE - DONE - ICursor
    - DONE - DONE - ITextureArrayData
    - DONE - DONE - ITextureData
    - DONE - IP   - Mesh
    - DONE - IP   - Pixel
    - DONE - DONE - Pixmap
    - DONE - IP   - PixmapFormat
    - DONE - IP   - PixmapFormatExtensions
    - DONE - IP   - PixmapIO                            Not working
    - DONE - IP   - PNGFormatStructs
    - DONE - IP   - Texture
    - DONE - IP   - TextureArray
    - DONE - DONE - TextureDataFactory
    - DONE - IP   - TextureFilter
    - DONE - IP   - TextureWrap
    - DONE - IP   - VertexAttribute
    - DONE - IP   - VertexAttributes

GRAPHICS/CAMERAS
----------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Camera
    - DONE - DONE - OrthographicCamera
    - DONE - DONE - PerspectiveCamera

GRAPHICS/FRAMEBUFFERS
---------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - FloatFrameBuffer
    - DONE - IP   - FloatFrameBufferBuilder
    - DONE - IP   - FrameBuffer
    - DONE - IP   - FrameBufferBuilder
    - DONE - IP   - FrameBufferCubemap
    - DONE - IP   - FrameBufferCubemapBuilder
    - DONE - IP   - FrameBufferRenderBufferAttachmentSpec
    - DONE - IP   - FrameBufferTextureAttachmentSpec
    - DONE - IP   - GLFrameBuffer
    - DONE - IP   - GLFrameBufferBuilder

GRAPHICS/G2D
------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Animation
    - DONE - IP   - AtlasRegion
    - DONE - IP   - AtlasSprite
    - IP   - IP   - BitmapFont                      Much work needed!
    - IP   - IP   - BitmapFontCache                 Relies on the BitmapFont rewrite.
    - DONE - IP   - DistanceFieldFont
    - IP   - IP   - Gdx2DPixmap
    - IP   - IP   - Gdx2DPixmapExtensions
    - IP   - IP   - Gdx2DUtils
    - DONE - IP   - GlyphLayout
    - DONE - IP   - NinePatch
    - DONE - IP   - ParticleEffect
    - DONE - IP   - ParticleEffectPool
    - DONE - IP   - ParticleEmitter
    - DONE - IP   - PixmapPacker
    - DONE - IP   - PixmapPackerIO
    - DONE - IP   - PolygonRegion
    - DONE - IP   - PolygonRegionLoader
    - DONE - IP   - PolygonSprite
    - DONE - IP   - RepeatablePolygonSprite
    - DONE - IP   - Sprite
    - DONE - IP   - SpriteCache
    - DONE - IP   - TextureAtlas
    - DONE - IP   - TextureAtlasData
    - DONE - IP   - TextureAtlasDataExtensions
    - DONE - IP   - TextureRegion

    Batching
    --------
    - IP   - IP   - CpuSpriteBatch                  Some methods have too many parameters
    - DONE - DONE - IBatch
    - IP   - IP   - IPolygonBatch                   Some methods have too many parameters
    - IP   - IP   - PolygonSpriteBatch              Some methods have too many parameters
    - IP   - IP   - SpriteBatch                     Some methods have too many parameters

GRAPHICS/G3D
------------

    See Documents/TODO_G3D.MD

GRAPHICS/GLUTILS
----------------

    CODE   DOCUMENT
    ----   --------
    - IP   - IP   - ETC1
    - IP   - IP   - ETC1TextureData
    - IP   - IP   - FacedCubemapData              Width / Height are incorrect
    - IP   - IP   - FileTextureArrayData
    - DONE - IP   - FileTextureData
    - DONE - IP   - FloatTextureData
    - DONE - IP   - GLOnlyTextureData
    - DONE - IP   - GLVersion
    - DONE - DONE - HdpiMode
    - DONE - DONE - HdpiUtils
    - DONE - IP   - IImmediateModeRenderer
    - DONE - DONE - IIndexData
    - DONE - IP   - IInstanceData
    - DONE - IP   - ImmediateModeRenderer20
    - DONE - IP   - IndexArray
    - DONE - DONE - IndexBufferObject
    - IP   - IP   - IndexBufferObjectSubData
    - IP   - IP   - InstanceBufferObject
    - IP   - IP   - InstanceBufferObjectSubData
    - DONE - DONE - IVertexData
    - DONE - IP   - KTXTTextureData
    - DONE - IP   - MipMapGenerator
    - DONE - IP   - MipMapTextureData
    - DONE - IP   - PixmapTextureData
    - DONE - IP   - ShaderProgram
    - DONE - IP   - ShapeRenderer
    - DONE - DONE - VertexArray
    - DONE - DONE - VertexBufferObject
    - DONE - IP   - VertexBufferObjectSubData
    - DONE - IP   - VertexBufferObjectWithVAO

    - The following do not need converting

GRAPHICS/OPENGL
---------------

    Remove the need for so much use of fixed() in code by implementing method overrides,
    and doing the work inside those methods instead?

    Work on reducing the amount of casting to uint.

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - GLBindings
    - DONE - DONE - IGL
    - DONE - DONE - IGLBindings

GRAPHICS/PROFILING
------------------

    These are profiling classes only. If adding GL/Glfw breaks these update them later on.

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - BaseGLInterceptor
    - DONE - DONE - GLInterceptor
    - DONE - DONE - GLProfiler          
    - DONE - DONE - IGLErrorListener

GRAPHICS/VIEWPORT
-----------------

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
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

INPUT
-----

    -      -      - GestureDetector
    - DONE - DONE - InputUtils
    -      -      - RemoteInput
    -      -      - RemoteSender

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS
----

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - IImageResolver
    - DONE - DONE - IMapRenderer
    - DONE - DONE - Map
    - DONE - DONE - MapGroupLayer
    - DONE - DONE - MapLayer
    - DONE - DONE - MapLayers
    - DONE - DONE - MapObject
    - DONE - DONE - MapObjects
    - DONE - DONE - MapProperties

MAPS/OBJECTS
------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - CircleMapObject
    - DONE - DONE - EllipseMapObject
    - DONE - DONE - PolygonMapObject
    - DONE - DONE - PolylineMapObject
    - DONE - DONE - RectangleMapObject
    - DONE - DONE - TextureMapObject

MAPS/TILED
----------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - ITiledMapTile
    - DONE - DONE - TiledMap
    - DONE - DONE - TiledMapImageLayer
    - DONE - DONE - TiledMapTileLayer
    - DONE - DONE - TiledMapTileSet
    - DONE - DONE - TiledMapTileSets

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

    All Classes in this section could be made more efficient.

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
    - DONE - DONE - AnimatedTileMapTile
    - DONE - DONE - StaticTiledMapTile

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATHS
-----

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Affine2
    - DONE - DONE - Bezier
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
    - DONE - DONE - Point2D
    - DONE - IP   - Polygon
    - DONE - IP   - Polyline
    - DONE - IP   - Quaternion
    - DONE - IP   - RandomXS128
    - DONE - IP   - RectangleShape
    - DONE - DONE - SimpleVectors
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
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES/SCENE2D
--------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Action
    - DONE - IP   - Actor
    - DONE - IP   - Event
    - DONE - IP   - Group
    - DONE - DONE - IActor
    - DONE - DONE - InputEvent
    - DONE - IP   - Stage
    - DONE - DONE - Touchable

SCENES/SCENE2D/ACTIONS
----------------------

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - Actions
    - DONE - IP   - AddAction
    - DONE - IP   - AddListenerAction
    - DONE - IP   - AfterAction
    - DONE - DONE - AlphaAction
    - DONE - IP   - ColorAction
    - DONE - IP   - CountdownEventAction
    - DONE - IP   - DelayAction
    - DONE - IP   - DelegateAction
    - DONE - IP   - EventAction
    - DONE - DONE - FloatAction
    - DONE - DONE - IntAction
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
    - DONE - DONE - SizeByAction
    - DONE - DONE - SizeToAction
    - DONE - DONE - TemporalAction
    - DONE - DONE - TimeScaleAction
    - DONE - DONE - TouchableAction
    - DONE - DONE - VisibleAction

SCENES/SCENE2D/LISTENERS
------------------------

    - Base classes to be used and extended as required

    CODE   DOCUMENT
    ----   --------
    - DONE - IP   - ActorGestureListener
    - DONE - DONE - ChangeListener
    - DONE - IP   - ClickListener
    - DONE - IP   - DragListener
    - DONE - IP   - DragScrollListener
    - DONE - IP   - FocusListener
    - DONE - DONE - IEventListener
    - DONE - DONE - InputListener

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
    - DONE - IP   - Button
    - DONE - IP   - ButtonGroup
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
    - DONE - DONE - ArraySelection
    - DONE - DONE - BaseDrawable
    - DONE - IP   - DragAndDrop
    - DONE - DONE - ICullable
    - DONE - IP   - IDisableable
    - DONE - DONE - IDrawable
    - DONE - DONE - ILayout
    - DONE - DONE - ITransformDrawable
    - DONE - IP   - NinePatchDrawable
    - DONE - IP   - ScissorStack
    - DONE - IP   - Selection
    - DONE - IP   - SpriteDrawable
    - DONE - DONE - TextureRegionDrawable
    - DONE - IP   - TiledDrawable

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS
-----

    - Move Utils/Collections and Utils/Viewport out of Utils and into somewhere more appropriate.

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Align
    - DONE - DONE - BinaryHeap
    - DONE - DONE - Bits
    - DONE - DONE - ByteOrder                       Is this still needed? Possible Delete Candidate?
    - DONE - DONE - Character                       C# System MUST already have this???
    - DONE - IP   - ComparableTimSort
    - DONE - IP   - DataInput                       Check
    - DONE - IP   - DataOutput                      Check
    - DONE - DONE - DataUtils                       Added Class
    - DONE - DONE - FloatConstants
    - DONE - DONE - IClipboard                      Convert - Interface, clipboard handled in backends.
    - DONE - DONE - ICloseable
    - DONE - DONE - IManageable
    - DONE - DONE - IReadable
    - DONE - DONE - IResetable
    - DONE - DONE - IRunnable                       Done, but is it needed?
    - DONE - DONE - Logger                          Make the actual message writing virtual
    - DONE - DONE - PerformanceCounter              Check
    - DONE - IP   - PerformanceCounters             Check
    - DONE - DONE - PropertiesUtils                 Convert, but check if necessary
    - DONE - IP   - QuadTreeFloat
    - DONE - IP   - QuickSelect
    - DONE - IP   - Scaling
    - DONE - DONE - ScreenUtils
    - DONE - IP   - Selector
    - DONE - IP   - SingletonBase<T>
    - DONE - DONE - SortUtils                       Do I still need this?
    - DONE - DONE - StringTokenizer
    - DONE - IP   - Timer
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

    I only really need the buffers that LibGDX uses, and I OUGHT to be finding alternatives.

     CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Buffer
    - IP   - IP   - BufferUtils
    - DONE - DONE - ByteBuffer
    - DONE - DONE - FloatBuffer
    - IP   - IP   - GdxBufferUtils
    - DONE - DONE - IntBuffer
    - DONE - IP   - ShortBuffer

    - **** - **** - CharBuffer                  Not needed yet
    - **** - **** - CircularByteBuffer          Not needed yet
    - **** - **** - DoubleBuffer                Not needed yet
    - **** - **** - LongBuffer                  Not needed yet
    - **** - **** - MappedByteBuffer            Not needed yet
    - **** - **** - StringCharBuffer            Not needed yet

UTILS/BUFFERS/BYTEBUFFERAS
--------------------------

     CODE   DOCUMENT
    ----   --------
    - **** - **** - ByteBufferAsCharBufferB     Not needed yet
    - **** - **** - ByteBufferAsCharBufferL     Not needed yet
    - **** - **** - ByteBufferAsDoubleBufferB   Not needed yet
    - **** - **** - ByteBufferAsDoubleBufferL   Not needed yet
    - **** - **** - ByteBufferAsFloatBufferB    Not needed yet
    - **** - **** - ByteBufferAsFloatBufferL    Not needed yet
    - **** - **** - ByteBufferAsIntBufferB      Not needed yet
    - **** - **** - ByteBufferAsIntBufferL      Not needed yet
    - **** - **** - ByteBufferAsLongBufferB     Not needed yet
    - **** - **** - ByteBufferAsLongBufferL     Not needed yet
    - **** - **** - ByteBufferAsShortBufferB    Not needed yet
    - **** - **** - ByteBufferAsShortBufferL    Not needed yet

UTILS/BUFFERS/DIRECTBUFFERS
---------------------------

     CODE   DOCUMENT
    ----   --------
    - **** - **** - DirectByteBuffer            Not needed yet

UTILS/BUFFERS/HEAPBUFFERS
-------------------------

     CODE   DOCUMENT
    ----   --------
    - IP   - IP   - HeapByteBuffer
    - **** - **** - HeapCharBuffer              Not needed yet
    - **** - **** - HeapDoubleBuffer            Not needed yet
    - IP   - IP   - HeapFloatBuffer
    - IP   - IP   - HeapIntBuffer
    - IP   - IP   - HeapShortBuffer
    - IP   - IP   - HeapByteBufferR
    - **** - **** - HeapCharBufferR             Not needed yet
    - **** - **** - HeapDoubleBufferR           Not needed yet
    - IP   - IP   - HeapFloatBufferR
    - IP   - IP   - HeapIntBufferR
    - IP   - IP   - HeapShortBufferR

UTILS/COLLECTIONS
-----------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - Array<T>                       Converted, but use List<T> for most cases.
    - DONE - IP   - ByteArray                      List< byte > should be fine for most cases.
    - DONE - DONE - DictionaryExtensions
    - DONE - DONE - DelayedRemovalList             Convert / Extend List<>
    - DONE - DONE - ListExtensions
    - DONE - DONE - ObjectMap                      Converted, but use Dictionary< object, object >
    - DONE - DONE - SnapshotArray<T>

    - Work on removing these

    - **** - IPredicate
    - **** - PredicateIterable
    - **** - PredicateIterator

    - The following do not need converting

    - **** - ArrayMap                       -> Use Dictionary<K, V>
    - **** - ArrayIterable                  -> Use IEnumerable?
    - **** - ArrayIterator                  -> Use IEnumerator?
    - **** - BoolArray                      -> Use List< bool >
    - **** - CharArray                      -> Use List< char >
    - **** - CollectionsData                -> ***** Not needed *****
    - **** - FloatArray                     -> Use List< float >
    - **** - IdentityMap<K, V>              -> ***** Not needed *****
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

UTILS/EXCEPTIONS
----------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - AssetNotLoadedException
    - DONE - DONE - BufferOverflowException
    - DONE - DONE - BufferUnderflowException
    - DONE - DONE - GdxRuntimeException
    - DONE - DONE - ReadOnlyBufferException

UTILS/POOLING
-------------

    CODE   DOCUMENT
    ----   --------
    - DONE - DONE - FlushablePool
    - DONE - DONE - IPoolable
    - DONE - DONE - Pool
    - DONE - DONE - PooledLinkedList
    - DONE - DONE - Pools

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
