
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

- There seems to be different namings for width/height etc properties and methods. Make it more uniform
- Make more use of `<inheritdoc cref=""/>` or just `<inheritdoc/>`

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

    - IP   - AbstractGraphics
    - IP   - AbstractInput
    - DONE - ApplicationAdapter
    - DONE - Game
    - DONE - Gdx
    - IP   - GDXVersion
    - DONE - IApplication
    - DONE - IApplicationListener
    - DONE - IAudio
    - DONE - IFile
    - DONE - IGraphics
    - DONE - IInput
    - DONE - IInputProcessor
    - DONE - ILifecycleListener
    - IP   - INet
    - DONE - InputAdapter
    - DONE - InputEventQueue
    - IP   - InputMultiplexer
    - DONE - IPreferences
    - DONE - IScreen
    - DONE - ScreenAdapter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS
--------

    - NOTE: I am not sure how to proceed with implementing OpenGL within this project
    -       right now. Java LibGDX uses LWJGL3, there is not current C# OpenGL implementation
    -       that does things in the same way ie. using ByterBuffer instead of byte*, that kind
    -       of thing. It means a big rewrite of many of the graphics classes.
    -       I might need some guidance, or a lightbulb moment to continue.

    - What if I switched to DirectX or used SDL2?

    - DONE - Camera
    - DONE - Color
    - DONE - Colors
    - DONE - Cubemap
    - DONE - FPSLogger
    - IP   - GLTexture
    - DONE - ICubemapData
    - DONE - ICursor
    - DONE - IDownloadPixmapResponseListener
    - IP   - IGL20
    - IP   - IGL30
    - DONE - ITextureArrayData
    - DONE - ITextureData
    - DONE - Mesh
    - DONE - OrthographicCamera
    - DONE - PerspectiveCamera                  
    - IP   - Pixmap              
    - IP   - PixmapFormat                             
    - IP   - PixmapIO
    - IP   - Texture                            
    - IP   - TextureArray                       
    - IP   - TextureFilter                      
    - DONE - TextureWrap                        
    - DONE - VertexAttribute                    
    - DONE - VertexAttributes

    - GameWindow ??
    - GraphicsDevice ??

    GRAPHICS/FRAMEBUFFERS
    ---------------------

    - IP   - FloatFrameBuffer
    - IP   - FloatFrameBufferBuilder
    - IP   - FrameBuffer
    - IP   - FrameBufferBuilder
    - IP   - FrameBufferCubemap
    - IP   - FrameBufferCubemapBuilder
    - IP   - FrameBufferRenderBufferAttachmentSpec
    - IP   - FrameBufferTextureAttachmentSpec
    - IP   - GLFrameBuffer
    - IP   - GLFrameBufferBuilder

    ** Restructure GLFrameBuffer.Build(), this method is too long.

    GRAPHICS/G2D
    ------------

    - IP   - Animation                      
    - IP   - AtlasRegion                    
    - IP   - AtlasSprite                    
    - IP   - BitmapFont 
    - IP   - BitmapFontCache                
    - IP   - CpuSpriteBatch                 
    - IP   - DistanceFieldFont              
    - IP   - Gdx2DPixmap                    
    - IP   - GlyphLayout                    
    - IP   - IBatch                         
    - IP   - IPolygonBatch                  
    - IP   - NinePatch                      
    - IP   - ParticleEffect                 
    - IP   - ParticleEffectPool        
    - IP   - ParticleEmitter           
    - IP   - PixmapPacker
    - IP   - PixmapPacker10       
    - IP   - PolygonRegion                  
    - IP   - PolygonRegionLoader
    - IP   - PolygonSprite                  
    - IP   - PolygonSpriteBatch             
    - IP   - RepeatablePolygonSprite        
    - IP   - Sprite                         
    - IP   - SpriteBatch                    
    - IP   - SpriteCache
    - IP   - TextureAtlas                   
    - IP   - TextureAtlasData   
    - IP   - TextureAtlasDataExtensions
    - IP   - TextureRegion                  

    GRAPHICS/G3D
    ------------

    See Documents/TODO_G3D.MD

    GRAPHICS/GLUTILS
    ----------------

    - IP   - FacedCubemapData
    - IP   - FileTextureArrayData
    - IP   - FileTextureData
    - IP   - FloatTextureData
    - IP   - GLOnlyTextureData
    - IP   - GLVersion
    - IP   - HdpiMode
    - IP   - HdpiUtils
    - IP   - IImmediateModeRenderer
    - IP   - IIndexData
    - IP   - IInstanceData
    - IP   - ImmediateModeRenderer20
    - IP   - IndexArray
    - IP   - IndexBufferObject
    - IP   - IndexBufferObjectSubData
    - IP   - InstanceBufferObject
    - IP   - InstanceBufferObjectSubData
    - IP   - IVertexData
    - IP   - KTXTTextureData
    - IP   - MipMapGenerator
    - IP   - MipMapTextureData
    - IP   - PixmapTextureData
    - IP   - ShaderProgram
    - IP   - ShapeRenderer

        The following classes have a lot in common.
        Perhaps create a base class they all extend from?

    - IP   - VertexArray
    - IP   - VertexBufferObject
    - IP   - VertexBufferObjectSubData
    - IP   - VertexBufferObjectWithVAO

    - The following do not need converting

    - **** - ETC1
    - **** - ETC1TextureData

    GRAPHICS/OPENGL
    ---------------

    - IP   - GL

    GRAPHICS/PROFILING
    ------------------

    These are profiling classes only. If adding GL/Glfw breaks these update them later on.

    - IP   - BaseGLInterceptor
    - IP   - GL20Interceptor
    - IP   - GL30Interceptor
    - IP   - GLProfiler
    - IP   - IGLErrorListener

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

    - IP   - IImageResolver                         
    - IP   - IMapRenderer                           
    - IP   - Map                                    
    - IP   - MapGroupLayer                          
    - IP   - MapLayer                               
    - IP   - MapLayers                              
    - IP   - MapObject                              
    - IP   - MapObjects                             
    - IP   - MapProperties                          

    MAPS/OBJECTS
    ------------

    - IP   - CircleMapObject                        
    - IP   - EllipseMapObject                       
    - IP   - PolygonMapObject                       
    - IP   - PolylineMapObject                      
    - IP   - RectangleMapObject                     
    - IP   - TextureMapObject                       

    MAPS/TILED
    ----------

    - IP   - ITiledMapTile                          
    - IP   - TiledMap                               
    - IP   - TiledMapImageLayer                     
    - IP   - TiledMapTileLayer                      
    - IP   - TiledMapTileSet                        
    - IP   - TiledMapTileSets                       

    MAPS/TILED/LOADERS
    ------------------

    - IP   - AtlasTmxMapLoader 
    - IP   - BaseTmxMapLoader
    - IP   - TmxMapLoader

    MAPS/TILED/OBJECTS
    ------------------

    - IP   - TiledMapTileMapObject                  

    MAPS/TILED/RENDERERS
    --------------------

    - IP   - BatchTiledMapRenderer                  
    - IP   - HexagonalTiledMapRenderer              
    - IP   - IsometricStaggeredTiledMapRenderer     
    - IP   - IsometricTiledMapRenderer              
    - IP   - ITiledMapRenderer                      
    - IP   - OrthoCachedTiledMapRenderer            
    - IP   - OrthogonalTiledMapRenderer             

    MAPS/TILED/TILES
    ----------------

    - IP   - AnimatedTileMapTile                    
    - IP   - StaticTiledMapTile                     

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATHS
-----

    - IP   - Affine2
    - IP   - Bezier
    - IP   - Bresenham2
    - IP   - BSpline
    - IP   - CatmullRomSpline
    - IP   - Circle
    - IP   - ConvexHull
    - IP   - CumulativeDistribution
    - IP   - DelaunayTriangulator
    - IP   - EarClippingTriangulator
    - IP   - Ellipse
    - IP   - FloatConsts
    - IP   - FloatCounter
    - IP   - Frustrum
    - IP   - GeometryUtils
    - IP   - GridPoint2
    - IP   - GridPoint3
    - IP   - Interpolation
    - IP   - Intersector
    - IP   - IPath
    - IP   - IShape2D
    - IP   - IVector
    - IP   - MathUtils
    - IP   - Matrix3                        
    - IP   - Matrix4                        
    - IP   - Number
    - IP   - NumberUtils                    
    - IP   - Plane
    - IP   - Polygon
    - IP   - Polyline
    - IP   - Quaternion
    - IP   - RandomXS128
    - IP   - RectangleShape
    - IP   - Vector2                        Convert, I prefer the way the LibGDX class works.
    - IP   - Vector3                        Convert, I prefer the way the LibGDX class works.
    - IP   - WindowedMean

    MATH/COLLISION
    --------------

    - IP   - BoundingBox
    - IP   - Ray
    - IP   - Segment
    - IP   - Sphere

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES/SCENE2D
--------------

    - IP   - Action
    - IP   - Actor
    - IP   - Event
    - IP   - Group
    - IP   - IActor
    - IP   - InputEvent
    - IP   - Stage
    - IP   - Touchable
    - IP   - TouchFocus

    SCENES/SCENE2D/ACTIONS
    ----------------------

    - IP   - Actions                        
    - IP   - AddAction                      
    - IP   - AddListenerAction              
    - IP   - AfterAction                    
    - IP   - AlphaAction                    
    - IP   - ColorAction                    
    - IP   - CountdownEventAction           
    - IP   - DelayAction                    
    - IP   - DelegateAction                 
    - IP   - EventAction                    
    - IP   - FloatAction                    
    - IP   - IntAction                      
    - IP   - LayoutAction                   
    - IP   - MoveByAction                   
    - IP   - MoveToAction                   
    - IP   - ParallelAction                 
    - IP   - RelativeTemporalAction         
    - IP   - RemoveAction                   
    - IP   - RemoveActorAction              
    - IP   - RemoveListenerAction           
    - IP   - RepeatAction                   
    - IP   - RotateByAction                 
    - IP   - RotateToAction                 
    - IP   - RunnableAction                 
    - IP   - ScaleByAction                  
    - IP   - ScaleToAction                  
    - IP   - SequenceAction                 
    - IP   - SizeByAction                   
    - IP   - SizeToAction                   
    - IP   - TemporalAction                 
    - IP   - TimeScaleAction                
    - IP   - TouchableAction                
    - IP   - VisibleAction                  

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

    - IP   - Button
    - IP   - ButtonGroup
    - IP   - Cell
    - IP   - CheckBox
    - IP   - Container
    - IP   - Dialog
    - IP   - DialogChangeListener
    - IP   - DialogFocusListener
    - IP   - DialogInputListener
    - IP   - HorizontalGroup
    - IP   - Image
    - IP   - ImageButton
    - IP   - ImageTextButton
    - IP   - Label
    - IP   - ListBox
    - IP   - ParticleEffectActor            
    - IP   - ProgressBar
    - IP   - ScrollPane
    - IP   - ScrollPaneListeners
    - IP   - SelectBox
    - IP   - Skin                           
    - IP   - Slider                         
    - IP   - SplitPane                      
    - IP   - Stack                          
    - IP   - Table                          
    - IP   - TextArea                       
    - IP   - TextButton
    - IP   - TextField                      
    - IP   - Tooltip
    - IP   - TextTooltip
    - IP   - TooltipManager                 
    - IP   - Touchpad                       
    - IP   - Tree
    - IP   - Value                          
    - IP   - ValueExtensions
    - IP   - VerticalGroup                  
    - IP   - Widget                         
    - IP   - WidgetGroup                    
    - IP   - Window                         

    SCENES/SCENE2D/UTILS
    --------------------

    - IP   - ArraySelection
    - IP   - BaseDrawable
    - IP   - DragAndDrop
    - IP   - ICullable
    - IP   - IDisableable
    - IP   - IDrawable
    - IP   - ILayout
    - IP   - ITransformDrawable
    - IP   - NinePatchDrawable
    - IP   - ScissorStack
    - IP   - Selection
    - IP   - SpriteDrawable
    - IP   - TextureRegionDrawable
    - IP   - TiledDrawable
    - IP   - UIUtils

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS
-----

    - Move Utils/Collections and Utils/Viewport out of Utils and into somewhere more appropriate.

    - DONE - Align
    - DONE - BinaryHeap
    - DONE - Bits
    - DONE - ByteOrder
    - DONE - Character
    - DONE - ComparableTimSort
    - DONE - DataInput                      Check
    - DONE - DataOutput                     Check
    - DONE - DataUtils                      Added Class
    - DONE - FloatConstants
    - DONE - GdxRuntimeException
    - DONE - GdxSystem
    - DONE - IClipboard                     Convert - Interface, clipboard handled in backends.                 
    - DONE - ICloseable
    - DONE - IReadable
    - DONE - IRunnable                      Done, but is it needed?
    - IP   - Logger                         Needs documenting.
    - IP   - PerformanceCounter             Check
    - IP   - PerformanceCounters            Check
    - IP   - PropertiesUtils                Convert, but check if necessary first
    - DONE - QuadTreeFloat
    - DONE - QuickSelect
    - IP   - Scaling
    - DONE - ScreenUtils
    - DONE - Select                         Renamed Selector (See SortedSetExtensions.cs)
    - DONE - SortUtils
    - DONE - StringTokenizer
    - DONE - Timer                          
    - DONE - TimeUtils
    - DONE - TimSort

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

    - IP   - Array<T>                       Converted, but use List<T> for most cases.
    - IP   - CollectionsData
    - IP   - DelayedRemovalArray            Convert / Extend List<>
    - IP   - IdentityMap<K, V>              Convert / extend Dictionary< object, object > 
    - IP   - IPredicate
    - IP   - ObjectMap                      Converted, but use Dictionary< object, object >
    - IP   - PredicateIterable
    - IP   - PredicateIterator
    - IP   - SnapshotArray<T>

    ----------------------------------------------------------------------------------

    - IP   - DictionaryExtensions
    - IP   - ListExtensions

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

    - IP   - FlushablePool                  
    - IP   - IPoolable                      
    - IP   - Pool                           
    - IP   - PooledLinkedList               
    - IP   - Pools                          

    UTILS/VIEWPORT
    --------------

    - IP   - ExtendedViewport               
    - IP   - FillViewport                   
    - IP   - FitViewport                    
    - IP   - ScalingViewport                
    - IP   - ScreenViewport                 
    - IP   - StretchViewport                
    - IP   - Viewport                       

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

    BACKENDS/DESKTOPGL/AUDIO
    ------------------------

    - IP   - GdxSoundAudioRecorder
    - IP   - IGLAudio
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

