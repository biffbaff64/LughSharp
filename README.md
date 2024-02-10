![lughlogo.png](Resources%2Flughlogo.png)
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

C# 2D Game Framework heavily inspired by the Java LibGDX Game Framework.
This is an OpenSource project, enabled, with thanks, by Jetbrains Community
Support OpenSource Licensing https://jb.gg/OpenSourceSupport

It is NOT a 100% conversion, some things may be different.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

![red7logo_small.png](Resources%2Fred7logo_small.png)   ![jb_beam.png](Resources%2Fjb_beam_small.png)  ![OSI_Standard_Logo_100X130.png](Resources%2FOSI_Standard_Logo_100X130.png)

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

LIBGDX CSHARP CONVERSION - ROUND 1
----------------------------------

ALL CLASSES WILL BE UP FOR MODIFICATION FOLLOWING TESTING.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- STEP 1: Complete all conversions so that the code will build.
- STEP 2: Refactor, where possible and necessary, to take advantage of C# language features.
    - Some classes can become structs / records instead.
    - Some/Most Get / Set method declarations in interfaces could become Properties.
    - switch expressions instead of switch statements where appropriate.
    - switch expressions instead of if..if/else..else where appropriate.
    - Check methods to see if they can be virtual.
    - Use [PublicApi] where necessary.
    - Use sealed classes only where strictly necessary. Users should be able to extend library classes.
    - Use of virtual for base classes/methods and classes that are likely to be extended is essential.
    - Constantly look for opportunities to improve this code.
- STEP 3: Resolve ALL remaining TODOs.
- STEP 4: Ensure code is fully documented.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- NO MAGIC NUMBERS!!!
- SORT OUT VERSIONING!!!
- PRIORITY is 2D classes first

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- There seems to be different namings for width/height etc properties and methods. Make it more uniform
- Make more use of <inheritdoc cref=""/>> or just <inheritdoc/>

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- IP = Conversion In Progress.
- DONE = Class finished but may not be fully 'CSHARP-ified'

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS
------

    TODO:   Remove SynchronousAssetLoader and modify AssetLoader / AsynchronousAssetLoader
            to be just one loading system.
    FOR NOW: Just use basic file loading until the rest is working, then work on a proper
             asset loading and management system. This will allow testing of everything else.
             ( Don't forget Maps.Tiled.Loaders... )

    - IP   - AssetDescriptor
    - IP   - AssetLoaderParameters
    - IP   - AssetLoadingTask
    - IP   - AssetManager
    - IP   - IAssetErrorListener
    - IP   - RefCountedContainer

    ASSETS/LOADERS
    --------------

    TODO:   I think most, if not all, of these classes could be modified or rewritten.
            As I've learned more about C# I've also learned more about Tasks and Delegates,
            I think these classes could/should use these.

    - IP   - AssetLoader
    - IP   - AsynchronousAssetLoader
    - IP   - BitmapFontLoader
    - IP   - CubemapLoader
    - IP   - ModelLoader
    - IP   - MusicLoader
    - IP   - ParticleEffectLoader
    - IP   - PixmapLoader
    - IP   - ShaderProgramLoader
    - IP   - SkinLoader
    - IP   - SoundLoader
    - IP   - SynchronousAssetLoader
    - IP   - TextureAtlasLoader
    - IP   - TextureLoader

    ASSETS/LOADERS/RESOLVERS
    ------------------------

    - IP   - AbsoluteFileHandleResolver
    - IP   - ClasspathFileHandleResolver
    - IP   - ExternalFileHandleResolver
    - IP   - IFileHandleResolver
    - IP   - InternalFileHandleResolver
    - IP   - LocalFileHandleResolver
    - IP   - PrefixFileHandleResolver
    - IP   - ResolutionFileResolver

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

AUDIO
-----

    - DONE - IAudioDevice
    - DONE - IAudioRecorder
    - DONE - IMusic
    - DONE - ISound

    AUDIO/MP3SHARP
    --------------

    - DONE - Buffer16BitSterso
    - DONE - MP3SharpException
    - DONE - MP3Stream
    - DONE - SoundFormat

    AUDIO/MP3SHARP/DECODING
    -----------------------

    - DONE - AudioBase
    - DONE - BitReserve
    - DONE - Bitstream
    - DONE - BitstreamErrors
    - DONE - BitstreamException
    - DONE - CircularByteBuffer
    - DONE - Crc16
    - DONE - Decoder
    - DONE - Decode.Parameters
    - DONE - DecoderErrors
    - DONE - DecoderException
    - DONE - Equalizer
    - DONE - Header
    - DONE - Huffman
    - DONE - OuputChannels
    - DONE - OutputChannelsEnum
    - DONE - PushbackStream
    - DONE - SampleBuffer
    - DONE - SynthesisFilter

    AUDIO/MP3SHARP/DECODERS
    -----------------------

    - DONE - ASubband
    - DONE - IFrameDecoder
    - DONE - LayerIDecoder
    - DONE - LayerIIDecoder
    - DONE - LayerIIIDecoder

    AUDIO/MP3SHARP/DECODERS/LAYERI
    ------------------------------

    - DONE - SubbandLayer1
    - DONE - SubbandLayer1IntensityStereo
    - DONE - SubbandLayer1Stereo

    AUDIO/MP3SHARP/DECODERS/LAYERII
    -------------------------------

    - DONE - SubbandLayer2
    - DONE - SubbandLayer2IntensityStereo
    - DONE - SubbandLayer2Stereo

    AUDIO/MP3SHARP/DECODERS/LAYERIII
    --------------------------------

    - DONE - ChannelData
    - DONE - GranuleInfo
    - DONE - Layer3SideInfo
    - DONE - SBI
    - DONE - ScaleFactorData
    - DONE - ScaleFactorTable

    AUDIO/MP3SHARP/IO
    -----------------

    - DONE - RandomAccessFileStream
    - DONE - RiffFile
    - DONE - WaveFile
    - DONE - WaveFileBuffer

    AUDIO/MP3SHARP/SUPPORT
    ----------------------

    - DONE - SupportClass

    AUDIO/OPENAL
    ------------

    - DONE - AL
    - DONE - ALC

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

CORE
----

    - DONE - AbstractGraphics
    - DONE - AbstractInput
    - DONE - ApplicationAdapter
    - DONE - Game
    - DONE - Gdx
    - DONE - GDXVersion
    - DONE - IApplication
    - DONE - IApplicationListener
    - DONE - IApplicationLogger
    - DONE - IAudio
    - DONE - IFile
    - DONE - IGraphics
    - DONE - IGraphics.DisplayModeDescriptor
    - DONE - IGraphics.MonitorDescriptor
    - DONE - IGraphics.BufferFormatDescriptor
    - DONE - IInput
    - DONE - IInput.Buttons
    - DONE - IInput.Keys
    - DONE - IInput.ITextInputListener
    - DONE - IInputProcessor
    - DONE - ILifecycleListener
    - DONE - INet
    - DONE - INet.IHttpResponse
    - DONE - INet.IHttpMethods
    - DONE - INet.IHttpResponseListener
    - DONE - INet.HttpRequest
    - DONE - InputAdapter
    - DONE - InputEventQueue
    - DONE - IPreferences
    - DONE - IScreen
    - DONE - ScreenAdapter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS
--------

    - DONE - Camera
    - DONE - Color
    - DONE - Colors
    - DONE - Cubemap
    - DONE - FPSLogger
    - DONE - GLTexture
    - DONE - GraphicsType
    - DONE - ICubemapData
    - DONE - ICursor
    - DONE - IDownloadPixmapResponseListener
    - DONE - IGL20
    - DONE - IGL30
    - DONE - ITextureArrayData
    - DONE - ITextureData
    - DONE - Mesh
    - DONE - OrthographicCamera
    - DONE - PerspectiveCamera                  
    - DONE - Pixmap              
    - DONE - PixmapFormat                             
    - DONE - PixmapIO
    - DONE - Texture                            
    - DONE - TextureArray                       
    - DONE - TextureFilter                      
    - DONE - TextureWrap                        
    - DONE - VertexAttribute                    
    - DONE - VertexAttributes

    GRAPHICS/FRAMEBUFFERS
    ---------------------

    - DONE - FloatFrameBuffer
    - DONE - FloatFrameBufferBuilder
    - DONE - FrameBuffer
    - DONE - FrameBufferBuilder
    - DONE - FrameBufferCubemap
    - DONE - FrameBufferCubemapBuilder
    - DONE - FrameBufferRenderBufferAttachmentSpec
    - DONE - FrameBufferTextureAttachmentSpec
    - DONE - GLFrameBuffer
    - DONE - GLFrameBufferBuilder

    ** Restructure GLFrameBuffer.Build(), this method is too long.

    GRAPHICS/G2D
    ------------

    - DONE - Animation                      
    - DONE - AtlasRegion                    
    - DONE - AtlasSprite                    
    - DONE - BitmapFont 
    - DONE - BitmapFontCache                
    - DONE - CpuSpriteBatch                 
    - DONE - DistanceFieldFont              
    - DONE - Gdx2DPixmap                    
    - DONE - GlyphLayout                    
    - DONE - IBatch                         
    - DONE - IPolygonBatch                  
    - DONE - NinePatch                      
    - DONE - ParticleEffect                 
    - DONE - ParticleEffectPool        
    - DONE - ParticleEmitter           
    - DONE - PixmapPacker
    - DONE - PixmapPacker10       
    - DONE - PolygonRegion                  
    - DONE - PolygonRegionLoader
    - DONE - PolygonSprite                  
    - DONE - PolygonSpriteBatch             
    - DONE - RepeatablePolygonSprite        
    - DONE - Sprite                         
    - DONE - SpriteBatch                    
    - DONE - SpriteCache
    - DONE - TextureAtlas                   
    - DONE - TextureAtlasData   
    - DONE - TextureAtlasDataExtensions
    - DONE - TextureRegion                  

    GRAPHICS/G3D
    ------------

    See Documents/TODO_G3D.MD

    GRAPHICS/GLUTILS
    ----------------

    - DONE - FacedCubemapData
    - DONE - FileTextureArrayData
    - DONE - FileTextureData
    - DONE - FloatTextureData
    - DONE - GLOnlyTextureData
    - DONE - GLVersion
    - DONE - HdpiMode
    - DONE - HdpiUtils
    - DONE - IImmediateModeRenderer
    - DONE - IIndexData
    - DONE - IInstanceData
    - DONE - ImmediateModeRenderer20
    - DONE - IndexArray
    - DONE - IndexBufferObject
    - DONE - IndexBufferObjectSubData
    - DONE - InstanceBufferObject
    - DONE - InstanceBufferObjectSubData
    - DONE - IVertexData
    - DONE - KTXTTextureData
    - DONE - MipMapGenerator
    - DONE - MipMapTextureData
    - DONE - PixmapTextureData
    - DONE - ShaderProgram
    - DONE - ShapeRenderer

        The following classes have a lot in common.
        Perhaps create a base class they all extend from?

    - DONE - VertexArray
    - DONE - VertexBufferObject
    - DONE - VertexBufferObjectSubData
    - DONE - VertexBufferObjectWithVAO

    - The following do not need converting

    - **** - ETC1
    - **** - ETC1TextureData

    GRAPHICS/PROFILING
    ------------------

    These are profiling classes only. If adding GL/Glfw breaks these update them later on.

    - DONE - BaseGLInterceptor
    - DONE - GL20Interceptor
    - DONE - GL30Interceptor
    - DONE - GLProfiler
    - DONE - IGLErrorListener

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

INPUT
-----

    - Not sure about these. Am I supporting mobile?

    -      - GestureDetector
    -      - RemoteInput
    -      - RemoteSender

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS
----

    - DONE - IImageResolver                         
    - DONE - IMapRenderer                           
    - DONE - Map                                    
    - DONE - MapGroupLayer                          
    - DONE - MapLayer                               
    - DONE - MapLayers                              
    - DONE - MapObject                              
    - DONE - MapObjects                             
    - DONE - MapProperties                          

    MAPS/OBJECTS
    ------------

    - DONE - CircleMapObject                        
    - DONE - EllipseMapObject                       
    - DONE - PolygonMapObject                       
    - DONE - PolylineMapObject                      
    - DONE - RectangleMapObject                     
    - DONE - TextureMapObject                       

    MAPS/TILED
    ----------

    - DONE - ITiledMapTile                          
    - DONE - TiledMap                               
    - DONE - TiledMapImageLayer                     
    - DONE - TiledMapTileLayer                      
    - DONE - TiledMapTileSet                        
    - DONE - TiledMapTileSets                       

    MAPS/TILED/LOADERS
    ------------------

    - DONE - AtlasTmxMapLoader 
    - DONE - BaseTmxMapLoader
    - DONE - TmxMapLoader

    MAPS/TILED/OBJECTS
    ------------------

    - DONE - TiledMapTileMapObject                  

    MAPS/TILED/RENDERERS
    --------------------

    - DONE - BatchTiledMapRenderer                  
    - DONE - HexagonalTiledMapRenderer              
    - DONE - IsometricStaggeredTiledMapRenderer     
    - DONE - IsometricTiledMapRenderer              
    - DONE - ITiledMapRenderer                      
    - DONE - OrthoCachedTiledMapRenderer            
    - DONE - OrthogonalTiledMapRenderer             

    MAPS/TILED/TILES
    ----------------

    - DONE - AnimatedTileMapTile                    
    - DONE - StaticTiledMapTile                     

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATHS
-----

    - DONE - Affine2
    - DONE - Bezier
    - DONE - Bresenham2
    - DONE - BSpline
    - DONE - CatmullRomSpline
    - DONE - Circle
    - DONE - ConvexHull
    - DONE - CumulativeDistribution
    - DONE - DelaunayTriangulator
    - DONE - EarClippingTriangulator
    - DONE - Ellipse
    - DONE - FloatConsts
    - DONE - FloatCounter
    - DONE - Frustrum
    - DONE - GeometryUtils
    - DONE - GridPoint2
    - DONE - GridPoint3
    - DONE - Interpolation
    - DONE - Intersector
    - DONE - IPath
    - DONE - IShape2D
    - DONE - IVector
    - DONE - MathUtils
    - DONE - Matrix3                        
    - DONE - Matrix4                        
    - DONE - Number
    - DONE - NumberUtils                    
    - DONE - Plane
    - DONE - Polygon
    - DONE - Polyline
    - DONE - Quaternion
    - DONE - RandomXS128
    - DONE - RectangleShape
    - DONE - Vector2                        Convert, I prefer the way the LibGDX class works.
    - DONE - Vector3                        Convert, I prefer the way the LibGDX class works.
    - DONE - WindowedMean

    MATH/COLLISION
    --------------

    - DONE - BoundingBox
    - DONE - Ray
    - DONE - Segment
    - DONE - Sphere

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES/SCENE2D
--------------

    - DONE - Action
    - DONE - Actor
    - DONE - Event
    - DONE - Group
    - DONE - IActor
    - DONE - InputEvent
    - DONE - Stage
    - DONE - Touchable
    - DONE - TouchFocus

    SCENES/SCENE2D/ACTIONS
    ----------------------

    - DONE - Actions                        
    - DONE - AddAction                      
    - DONE - AddListenerAction              
    - DONE - AfterAction                    
    - DONE - AlphaAction                    
    - DONE - ColorAction                    
    - DONE - CountdownEventAction           
    - DONE - DelayAction                    
    - DONE - DelegateAction                 
    - DONE - EventAction                    
    - DONE - FloatAction                    
    - DONE - IntAction                      
    - DONE - LayoutAction                   
    - DONE - MoveByAction                   
    - DONE - MoveToAction                   
    - DONE - ParallelAction                 
    - DONE - RelativeTemporalAction         
    - DONE - RemoveAction                   
    - DONE - RemoveActorAction              
    - DONE - RemoveListenerAction           
    - DONE - RepeatAction                   
    - DONE - RotateByAction                 
    - DONE - RotateToAction                 
    - DONE - RunnableAction                 
    - DONE - ScaleByAction                  
    - DONE - ScaleToAction                  
    - DONE - SequenceAction                 
    - DONE - SizeByAction                   
    - DONE - SizeToAction                   
    - DONE - TemporalAction                 
    - DONE - TimeScaleAction                
    - DONE - TouchableAction                
    - DONE - VisibleAction                  

    SCENES/SCENE2D/LISTENERS
    ------------------------

    - Base classes to be used and extended as required

    - IP   - ActorGestureListener           
    - IP   - ChangeListener                 
    - IP   - ClickListener                  
    - IP   - DragListener                   
    - IP   - DragScrollListener             
    - IP   - FocusListener                  
    - IP   - IEventListener                 
    - IP   - InputListener                  

TODO: Use Lambdas for these...
i.e. AddListener( new ClickListener()
{
    // Clicked needs to be a Func<>
    Clicked = ( ev, x, y ) =>
    {
    }
} );

    SCENES/SCENE2D/UI
    -----------------

    TODO: I don't like the way Cell and Value classes are implemented.
          They seem confusing and are candidates for a rewrite.

    - DONE - Button
    - DONE - ButtonGroup
    - DONE - Cell
    - DONE - CheckBox
    - DONE - Container
    - DONE - Dialog
    - DONE - DialogChangeListener
    - DONE - DialogFocusListener
    - DONE - DialogInputListener
    - DONE - HorizontalGroup
    - DONE - Image
    - DONE - ImageButton
    - DONE - ImageTextButton
    - DONE - Label
    - DONE - ListBox
    - DONE - ParticleEffectActor            
    - DONE - ProgressBar
    - DONE - ScrollPane
    - DONE - ScrollPaneListeners
    - DONE - SelectBox
    - DONE - Skin                           
    - DONE - Slider                         
    - DONE - SplitPane                      
    - DONE - Stack                          
    - DONE - Table                          
    - DONE - TextArea                       
    - DONE - TextButton
    - DONE - TextField                      
    - DONE - Tooltip
    - DONE - TextTooltip
    - DONE - TooltipManager                 
    - DONE - Touchpad                       
    - DONE - Tree
    - DONE - Value                          
    - DONE - ValueExtensions
    - DONE - VerticalGroup                  
    - DONE - Widget                         
    - DONE - WidgetGroup                    
    - DONE - Window                         

    SCENES/SCENE2D/UTILS
    --------------------

    - DONE - ArraySelection
    - DONE - BaseDrawable
    - DONE - DragAndDrop
    - DONE - ICullable
    - DONE - IDisableable
    - DONE - IDrawable
    - DONE - ILayout
    - DONE - ITransformDrawable
    - DONE - NinePatchDrawable
    - DONE - ScissorStack
    - DONE - Selection
    - DONE - SpriteDrawable
    - DONE - TextureRegionDrawable
    - DONE - TiledDrawable
    - DONE - UIUtils

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS
-----

    - DONE - Align
    - DONE - BinaryHeap
    - DONE - Bits
    - DONE - Character
    - DONE - ComparableTimSort
    - DONE - DataInput                      Check
    - DONE - DataOutput                     Check
    - DONE - DataUtils                      Added Class
    - DONE - FloatConstants
    - DONE - GdxRuntimeException
    - DONE - IClipboard                     Convert - Interface, clipboard handled in backends.                 
    - DONE - ICloseable
    - DONE - IReadable
    - DONE - IRunnable                      Done, but is it needed?
    - DONE - Logger                         
    - DONE - PerformanceCounter             Check
    - DONE - PerformanceCounters            Check
    - DONE - PropertiesUtils                Convert, but check if necessary first
    - DONE - QuadTreeFloat
    - DONE - QuickSelect
    - DONE - Scaling
    - DONE - ScreenUtils
    - DONE - Select                         Renamed Selector (See SortedSetExtensions.cs)
    - DONE - SharedLibraryLoader
    - DONE - SharedLibraryLoader.SystemHelpers
    - DONE - SortUtils
    - DONE - StringTokenizer
    - DONE - Timer                          
    - DONE - TimeUtils
    - DONE - TimSort
    - DONE - Trace                          Addition

    - The following do not need converting

    - **** - Base64Coder                    Use System.Convert classes
    - **** - GdxNativesLoader
    - **** - I18NBundle
    - **** - LittleEndianInputStream
    - **** - PauseableThread
    - **** - Queue                          Use system.collection.generics.Queue<T>?
    - **** - SortedIntList                  Use SortedList<int>
    - **** - StreamUtils
    - **** - StringBuilder                  Use System.Text.StringBuilder
    - **** - TextFormatter

    UTILS/BUFFERS
    -------------

    - IP   - Buffer
    - IP   - BufferUtils
    - IP   - ByteBuffer
    - IP   - CharBuffer
    - IP   - CircularByteBuffer
    - IP   - DirectByteBuffer
    - IP   - DoubleBuffer
    - IP   - FloatBuffer
    - IP   - HeapByteBuffer
    - IP   - HeapCharBuffer
    - IP   - HeapFloatBuffer
    - IP   - HeapShortBuffer
    - IP   - IDirectBuffer
    - IP   - IntBuffer
    - IP   - LongBuffer
    - IP   - MappedByteBuffer
    - IP   - ShortBuffer
    - IP   - StringCharBuffer

    UTILS/COLLECTIONS
    -----------------

    - DONE - Array<T>                       Converted, but use List<T> for most cases.
    - DONE - CollectionsData
    - DONE - DelayedRemovalArray            Convert / Extend List<>
    - DONE - IdentityMap<K, V>              Convert / extend Dictionary< object, object > 
    - DONE - IPredicate
    - DONE - ObjectMap                      Converted, but use Dictionary< object, object >
    - DONE - PredicateIterable
    - DONE - PredicateIterator
    - DONE - SnapshotArray<T>

    ----------------------------------------------------------------------------------

    - DONE - DictionaryExtensions
    - DONE - ListExtensions

    ----------------------------------------------------------------------------------

    - The following do not need converting

    - **** - ArrayMap                       Use Dictionary<K, V>
    - **** - ArrayIterable                  Use IEnumerable?
    - **** - ArrayIterator                  Use IEnumerator?
    - **** - BoolArray                      Use List< bool >
    - **** - ByteArray                      Use List< byte >
    - **** - CharArray                      Use List< char >
    - **** - FloatArray                     Use List< float >
    - **** - IntArray                       Use List< int >
    - **** - IntFloatMap                    Use Dictionary< int, float >
    - **** - IntIntMap                      Use Dictionary< int, int >
    - **** - IntMap                         Use Dictionary< int, V >
    - **** - IntSet                         Use List<int>
    - **** - LongArray                      Use List< long >
    - **** - LongMap                        Use Dictionary< long, V >
    - **** - ObjectFloatMap                 Use Dictionary< object, float >
    - **** - ObjectIntMap                   Use Dictionary< object, int >
    - **** - ObjectLongMap                  Use Dictionary< object, long >
    - **** - ObjectSet<T>                   Use List<T>
    - **** - OrderedMap<K, V>               Use SortedDictionary<K, V> ?
    - **** - OrderedSet<T>                  Use SortedSet<T>
    - **** - ShortArray                     Use List< short >

    UTILS/POOLING
    -------------

    - DONE - FlushablePool                  
    - DONE - IPoolable                      
    - DONE - Pool                           
    - DONE - PooledLinkedList               
    - DONE - Pools                          

    UTILS/VIEWPORT
    --------------

    - DONE - ExtendedViewport               
    - DONE - FillViewport                   
    - DONE - FitViewport                    
    - DONE - ScalingViewport                
    - DONE - ScreenViewport                 
    - DONE - StretchViewport                
    - DONE - Viewport                       

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

BACKENDS/DESKTOPGL
------------------

    - IP   - DesktopGLApplication
    - IP   - DesktopGLApplicationConfiguration
    - IP   - DesktopGLNativesLoader
    - IP   - DesktopGLNet
    - IP   - DesktopWindowCallbacks
    - IP   - IDesktopGLApplicationBase
    - IP   - Sync

    BACKENDS/DESKTOPGL/FILES
    ------------------------

    - IP   - DesktopGLFileHandle
    - IP   - DesktopGLFiles

    BACKENDS/DESKTOPGL/GRAPHICS
    ---------------------------

    - IP   - DesktopGL20
    - IP   - DesktopGL30
    - IP   - DesktopGLGraphics
    - IP   - DesktopGLGraphics.DesktopGLDisplayMode   Delete candidate
    - IP   - DesktopGLGraphics.DesktopGLMonitor       Delete candidate

    BACKENDS/DESKTOPGL/INPUT
    ------------------------

    - IP   - DefaultGLInput
    - IP   - IDesktopGLInput

    BACKENDS/DESKTOPGL/UTILS
    ------------------------

    - IP   - DesktopGLApplicationLogger
    - IP   - DesktopGLClipboard
    - IP   - DesktopGLCursor
    - IP   - DesktopGLPreferences

    BACKENDS/DESKTOPGL/WINDOW
    -------------------------

    - IP   - DesktopGLWindow
    - IP   - DesktopGLWindowConfiguration
    - IP   - DesktopGLWindowAdapter
    - IP   - IDesktopGLWindowListener

    BACKENDS/DESKTOPGL/AUDIO
    ------------------------

    - IP   - GdxSoundAudioRecorder
    - DONE - IGLAudio
    - IP   - Mp3
    - IP   - Ogg
    - IP   - OggInputStream
    - IP   - OpenALAudio
    - IP   - OpenALAudioDevice
    - IP   - OpenALMusic
    - IP   - OpenALSound
    - IP   - Wav

    BACKENDS/DESKTOPGL/AUDIO/MOCK
    -----------------------------

    - DONE - MockAudio
    - DONE - MockAudioDevice
    - DONE - MockAudioRecorder
    - DONE - MockMusic
    - DONE - MockSound

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/BOX2D
----------------

    - It's most likely best to recommend the use of an already available C# port.
    - ( It really depends on how much of a glutton for punishment you are!!!! )

    eg:
    - Box2DSharp
    - Box2DX
    - Box2D.Net
    - Box2D.CSharp

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/GDX-TOOLS
--------------------

    - IP   - FileProcessor

    EXTENSIONS/GDX-TOOLS/IMAGEPACKER
    --------------------------------

    - IP   - ImagePacker

    EXTENSIONS/GDX-TOOLS/TILEDMAPPACKER
    -----------------------------------

    - IP   - TileMapPacker
    -      - TiledMapPackerTest
    -      - TiledMapPackerTestRender
    -      - TileSetLayout

    EXTENSIONS/GDX-TOOLS/TOOLS
    --------------------------

    EXTENSIONS/GDX-TOOLS/TOOLS/TEXTUREPACKER
    ----------------------------------------

    - IP   - ColorBleedEffect
    -      - TexturePacker

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

TESTS
-----

    - IP   - Core/Graphics/CameraTest
    -      - Core/Graphics/SpriteTest
    - IP   - Core/Scene/Scene2D/StageTest

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

