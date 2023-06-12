- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
LIBGDX CSharp CONVERSION - ROUND 1
----------------------------------
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- STEP 1: Complete all conversions so that the code will build.
- STEP 2: Refactor, where possible and necessary, to take advantage of C# language features.
-         Some classes can become structs / records instead.
-         Some Get / Set method declarations in interfaces could become Properties.
-         switch expressions instead of switch statements where appropriate.
- STEP 3: Resolve ALL remaining TODOs.
- STEP 4: Ensure code is fully documented.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- SORT OUT VERSIONING!!!

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- ADD A LICENCE - MIT / APACHE ???

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- IP = Conversion In Progress.
- DONE = Class finished but may not be fully 'CSharp-ified'

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- PRIORITY is 2D classes first

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- Restructure, where necessary, to cut the number of exceptions that can be thrown

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- Constantly look for opportunities to improve this code.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ROOT
----

    - DONE - Gdx                            Convert

CORE
----

    - DONE - AbstractGraphics               Convert
    - DONE - AbstractInput                  Convert
    - DONE - ApplicationAdapter             Convert
    - DONE - ApplicationBase                New
    - DONE - Game                           Convert
    - DONE - GDXVersion                     Convert / Replace with C# Versioning?
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS
------

    - DONE - AssetDescriptor                Convert
    - DONE - AssetLoaderParameters          Convert
    - DONE - AssetLoadingTask               Convert
    - DONE - AssetManager                   Convert
    - DONE - IAssetErrorListener            Convert
    - DONE - RefCountedContainer            Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS/LOADERS
--------------

    - DONE - AssetLoader                    Convert
    - DONE - AsynchronousAssetLoader        Convert
    - DONE - BitmapFontLoader               Convert
    -      - CubemapLoader                  Convert
    -      - I18NBundleLoader               Convert
    -      - ModelLoader                    Convert
    - DONE - MusicLoader                    Convert
    -      - ParticleEffectLoader           Convert
    - DONE - PixmapLoader                   Convert
    - IP   - ShaderProgramLoader            Convert
    - DONE - SkinLoader                     Convert
    - DONE - SoundLoader                    Convert
    - DONE - SynchronousAssetLoader         Convert
    - DONE - TextureAtlasLoader             Convert
    - DONE - TextureLoader                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS/LOADERS/RESOLVERS
------------------------

    - DONE - AbsoluteFileHandleResolver
    - DONE - IFileHandleResolver
    - DONE - InternalFileHandleResolver
    -------------------------------------
    -      - ClasspathFileHandleResolver   
    -      - ExternalFileHandleResolver    
    -      - LocalFileHandleResolver       
    -      - PrefixFileHandleResolver      
    -      - ResolutionFileResolver        
    -------------------------------------

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

    - java.io.BufferedReader                -> 
    - java.io.BufferedInputStream           -> 
    - java.io.ByteArrayOutputStream         -> 
    - java.io.DataInputStream               -> BinaryReader
    - java.io.DataOutputStream              -> BinaryWriter
    - java.io.File                          -> 
    - java.io.FileHandle                    -> 
    - java.io.FileHandleStream              -> 
    - java.io.FilterOutputStream            -> 
    - java.io.InputStream                   -> StreamReader
    - java.io.InputStreamReader             -> StreamReader
    - java.io.IOException                   -> 
    - java.io.OutputStream                  -> StreamWriter
    - java.io.Reader                        -> 

    - Would MemoryStream cover all these?
    - java.nio.Buffer                       ->  
    - java.nio.BufferUtils                  ->  
    - java.nio.ByteBuffer                   ->  
    - java.nio.ByteBuffer                   -> 
    - java.nio.DataBuffer                   -> 
    - java.nio.DirectByteBuffer             -> 
    - java.nio.DoubleBuffer                 -> 
    - java.nio.FloatBuffer                  -> 
    - java.nio.HeapLongBuffer               -> 
    - java.nio.IntBuffer                    -> 
    - java.nio.LongBuffer                   -> 

    ------------------------------------------
    - Look into existing C# ZIP libraries
    - java.util.zip.CRC32                   -> 
    - java.util.zip.CheckedOutputStream     -> 
    - java.util.zip.Deflater                -> 
    - java.util.zip.DeflaterOutputStream    -> 
    - java.util.zip.InflaterInputStream     -> 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS
--------

    - DONE - BufferFormat                       Convert - record class
    - DONE - Camera                             Convert
    - DONE - Color                              Convert
    - DONE - Colors                             Convert
    -      - Cubemap                            Convert
    - DONE - DisplayMode                        Convert
    - DONE - FPSLogger                          Convert
    - DONE - GLTexture                          Convert
    - DONE - GraphicsType                       Convert
    - DONE - ICubemapData                       Convert
    - DONE - ICursor                            Convert
    - DONE - IDownloadPixmapResponseListener    Convert
    - DONE - IGL20                              Convert
    - DONE - IGL30                              Convert
    - DONE - ITextureArrayData                  Convert
    - DONE - ITextureData                       Convert
    - DONE - Mesh                               Convert
    - DONE - Monitor                            Convert
    - DONE - OrthographicCamera                 Convert
    - DONE - PerspectiveCamera                  Convert
    - DONE - Pixmap                             Convert
    - IP   - PixmapIO                           Convert
    - DONE - Texture                            Convert
    - DONE - TextureArray                       Convert
    - DONE - TextureFilter                      Convert
    - DONE - TextureWrap                        Convert
    - DONE - VertexAttribute                    Convert
    - DONE - VertexAttributes                   Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G2D
------------

    - DONE - Animation                      Convert
    - DONE - AtlasRegion                    Convert
    - DONE - AtlasSprite                    Convert
    - DONE - BitmapFont                     Convert
    - IP   - BitmapFontCache                Convert
    - DONE - CpuSpriteBatch                 Convert
    -      - DistanceFieldFont              Convert
    - DONE - Gdx2DPixmap                    Convert
    - DONE - GlyphLayout                    Convert
    - DONE - IBatch                         Convert
    - DONE - IPolygonBatch                  Convert
    - DONE - NinePatch                      Convert
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
    - DONE - Sprite                         Convert
    - DONE - SpriteBatch                    Convert
    - DONE - SpriteCache                    Convert
    - DONE - TextureAtlas                   Convert
    - IP   - TextureAtlasData               Convert
    - DONE - TextureAtlasDataExtensions     Convert
    - DONE - TextureRegion                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D
------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/GLUTILS
----------------

    - IP   - ETC1
    - IP   - ETC1TextureData
    -      - FacedCubemapData
    - IP   - FileTextureArrayData
    - IP   - FileTextureData
    -      - FloatFrameBuffer
    - DONE - FloatFrameBufferBuilder                Move from GLFrameBuffer
    -      - FloatTextureData
    - DONE - FrameBuffer
    - DONE - FrameBufferBuilder                     Move from GLFrameBuffer
    -      - FrameBufferCubemap
    - DONE - FrameBufferCubemapBuilder              Move from GLFrameBuffer
    - DONE - FrameBufferRenderBufferAttachmentSpec  Move from GLFrameBuffer
    - DONE - FrameBufferTextureAttachmentSpec       Move from GLFrameBuffer
    - DONE - GLFrameBuffer
    - DONE - GLFrameBufferBuilder                   Move from GLFrameBuffer
    - DONE - GLOnlyTextureData
    - IP   - GLVersion
    - DONE - HdpiMode
    - IP   - HdpiUtils
    - DONE - IImmediateModeRenderer
    - DONE - IIndexData
    - DONE - IInstanceData
    - DONE - ImmediateModeRenderer20
    - DONE - IndexArray
    -      - IndexBufferObject
    -      - IndexBufferObjectSubData
    -      - InstanceBufferObject
    -      - InstanceBufferObjectSubData
    - DONE - IVertexData
    - IP   - KTXTTextureData
    -      - MipMapGenerator
    -      - MipMapTextureData
    - DONE - PixmapTextureData
    - IP   - ShaderProgram
    - DONE - ShapeRenderer
    -      - VertexArray
    -      - VertexBufferObject
    -      - VertexBufferObjectSubData
    -      - VertexBufferObjectWithVAO

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/PROFILING
------------------

    - IP   - GL20Interceptor
    - IP   - GL30Interceptor
    - DONE - GLInterceptor
    - IP   - GLProfiler
    - IP   - IGLErrorListener

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

INPUT
-----

    -      - GestureDetector
    -      - RemoteInput
    - DONE - RemoteSender       Needs rework for compatibility with system.io.sockets.

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

    - DONE - TiledMapTileMapObject                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/RENDERERS
--------------------

    - DONE - BatchTiledMapRenderer                  Convert
    - DONE - HexagonalTiledMapRenderer              Convert
    -      - IsometricStaggeredTiledMapRenderer     Convert
    - DONE - IsometricTiledMapRenderer              Convert
    - DONE - ITiledMapRenderer                      Convert
    -      - OrthoCachedTiledMapRenderer            Convert
    - DONE - OrthogonalTiledMapRenderer             Convert

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
    - DONE - BSpline
    -      - CatmullRomSpline
    - DONE - Circle
    - IP   - ConvexHull
    - DONE - CumulativeDistribution
    -      - DelaunayTriangulator
    -      - EarClippingTriangulator
    - DONE - Ellipse
    - DONE - FloatCounter
    - DONE - Frustrum
    - DONE - GeometryUtils
    - DONE - GridPoint2
    - DONE - GridPoint3
    -      - Interpolation
    -      - Intersector
    - DONE - IPath
    - DONE - IShape2D
    - DONE - IVector
    - DONE - MathUtils
    -      - Matrix3                Convert
    - IP   - Matrix4                Convert
    - DONE - Number
    - DONE - Plane
    - DONE - Polygon
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

    - DONE - Action                         Convert
    - DONE - Actor                          Convert
    - DONE - Event                          Convert
    - DONE - Group                          Convert
    - DONE - IEventListener                 Convert
    - DONE - InputEvent                     Convert
    - DONE - InputListener                  Convert
    - DONE - Stage                          Convert
    - DONE - Touchable                      Convert
    - DONE - TouchFocus                     Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/ACTIONS
----------------------

    - DONE - Actions                        Convert
    - DONE - AddAction                      Convert
    - DONE - AddListenerAction              Convert
    - DONE - AfterAction                    Convert
    - DONE - AlphaAction                    Convert
    - DONE - ColorAction                    Convert
    - DONE - CountdownEventAction           Convert
    - DONE - DelayAction                    Convert
    - DONE - DelegateAction                 Convert
    - DONE - EventAction                    Convert
    - DONE - FloatAction                    Convert
    - DONE - IntAction                      Convert
    - DONE - LayoutAction                   Convert
    - DONE - MoveByAction                   Convert
    - DONE - MoveToAction                   Convert
    - DONE - ParallelAction                 Convert
    - DONE - RelativeTemporalAction         Convert
    - DONE - RemoveAction                   Convert
    - DONE - RemoveActorAction              Convert
    - DONE - RemoveListenerAction           Convert
    - DONE - RepeatAction                   Convert
    - DONE - RotateByAction                 Convert
    - DONE - RotateToAction                 Convert
    - DONE - RunnableAction                 Convert
    - DONE - ScaleByAction                  Convert
    - DONE - ScaleToAction                  Convert
    - DONE - SequenceAction                 Convert
    - DONE - SizeByAction                   Convert
    - DONE - SizeToAction                   Convert
    - DONE - TemporalAction                 Convert
    - DONE - TimeScaleAction                Convert
    - DONE - TouchableAction                Convert
    - DONE - VisibleAction                  Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/UI
-----------------

    - IP   - Button                         Convert
    -      - ButtonGroup                    Convert
    - DONE - Cell                           Convert
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
    - DONE - Value                          Convert
    - DONE - ValueExtensions                New Class
    -      - VerticalGroup                  Convert
    -      - Widget                         Convert
    - DONE - WidgetGroup                    Convert
    -      - Window                         Convert

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/UTILS
--------------------

    -      - ActorGestureListener           Convert
    -      - ArraySelection                 Convert
    - DONE - BaseDrawable                   Convert
    - DONE - ChangeListener                 Convert
    - DONE - ClickListener                  Convert
    -      - DragAndDrop                    Convert
    -      - DragListener                   Convert
    -      - DragScrollListener             Convert
    -      - FocusListener                  Convert
    - DONE - ICullable                      Convert
    - DONE - IDisableable                   Convert
    - DONE - IDrawable                      Convert
    - DONE - ILayout                        Convert
    - DONE - ITransformDrawable             Convert
    - DONE - NinePatchDrawable              Convert
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

    Misc
    ----
    - DONE - Align                          Convert
    ----------------------------------------------------------------------------------
    - **** - Base64Coder                    Use System.Convert classes
    ----------------------------------------------------------------------------------
    - DONE - BinaryHeap
    - DONE - Bits                           Convert
    - DONE - CharHelper                     Might be added to.
    ----------------------------------------------------------------------------------
    - **** - ComparableTimSort              Not Needed
    ----------------------------------------------------------------------------------
    - DONE - DataInput                      Check
    -      - DataOutput                     Check
    - DONE - DataUtils                      Added Class
    - DONE - FloatConsts                    Done, but is it needed?
    -      - GdxNativesLoader
    -      - I18NBundle                     Check
    - DONE - ICloseable                     Convert
    - DONE - IClipboard                     Convert - Interface, actual clipboard handled in backend.                 
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
    - IP   - ScreenUtils                    Needs ByteBuffer.Read implementing
    - IP   - Select                         Renamed Selector
    -      - SharedLibraryLoader            Check
    - DONE - SortUtils
    -      - SortedIntList
    -      - StreamUtils
    - DONE - StringTokenizer
    ----------------------------------------------------------------------------------
    - **** - StringBuilder                  Use System.Text.StringBuilder
    - **** - TextFormatter                  Remove, look into C# localisation
    ----------------------------------------------------------------------------------
    - DONE - Timer                          Use System.Timer instead??
    ----------------------------------------------------------------------------------
    - DONE - TimeUtils
    - DONE - TimSort
    -      - Trace                          Addition

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

    - **** - AtomicInteger                  No
    - **** - AtomicQueue                    No
    - **** - AtomicReferenceArray           No

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
    - **** - BoolArray                      List< bool >
    - **** - ByteArray                      List< byte >
    - **** - CharArray                      List< char >
    - **** - IntArray                       List< int >
    - **** - FloatArray                     List< float >
    - **** - LongArray                      List< long >
    - **** - ShortArray                     List< short >

    - DONE - DelayedRemovalArray            Convert / Extend List<>
    - DONE - SnapshotArray                  Convert / extend List<T>

    Extensions
    ----------
    - IP   - DictionaryExtensions
    - DONE - GdxArray
    - IP   - ListExtensions
    -      - ListQueue
    -      - ListStack
    -      - ObjectMap
    -      - ObjectSet

    Maps
    ----
    - **** - ArrayMap                       Replace with Dictionary<K, V>
    - **** - IntFloatMap                    Replace with Dictionary< int, float >
    - **** - IntIntMap                      Replace with Dictionary< int, int >
    - **** - IntMap                         Replace with Dictionary< int, V >
    - **** - LongMap                        Replace with Dictionary< long, V >
    - **** - ObjectFloatMap                 Replace with Dictionary< object, float >
    - **** - ObjectIntMap                   Replace with Dictionary< object, int >
    - **** - ObjectLongMap                  Replace with Dictionary< object, long >
    - **** - ObjectMap                      Replace with Dictionary< object, object >

    - IP   - IdentityMap<K, V>              Convert / extend Dictionary< object, object > 
    - IP   - OrderedMap<K, V>               Convert / extend Dictionary< object, object >

    Sets
    ----
    - **** - IntSet                         Replace with List<int>
    - **** - ObjectSet<T>                   Replace with List<T>
    - IP   - OrderedSet<T>                  Convert / extend List<T>
                                            
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

    - **** - CRC
    - **** - IChecksum
    - **** - ICodeProgress
    - **** - LzmaAdapted

    UTILS/COMPRESSION/LZ
    --------------------
            - **** - BinTree
            - **** - InWindow
            - **** - OutWindow
    
    UTILS/COMPRESSION/LZMA
    ----------------------
            - **** - Base
            - **** - Decoder
            - **** - Encoder
    
    UTILS/COMPRESSION/RANGECODER
    ----------------------------
            - **** - BitTreeDecoder
            - **** - BitTreeEncoder
            - **** - Decoder
            - **** - Encoder

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

    - **** - IBaseJsonReader
    - **** - Json
    - **** - JsonReader
    - **** - JsonValue
    - **** - JsonWriter
    - **** - UBJsonReader
    - **** - UBJsonWriter

    using an existing Json library would be better, but this could be
    converted at a later date.

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

UTILS/REGEX
-----------

    - RE-EVALUATE WHETHER THIS IS NEEDED...
    - REALLY OUGHT TO BE USING C# REGEX!
    - (Although I would like to convert it eventually...)

    - DONE - ASCII                          Convert
    - DONE - IMatchResult                   Convert
    - IP   - Matcher                        Convert
    - IP   - Pattern                        Convert
    - DONE - PatternSyntaxException         Convert
    - DONE - UnicodeProp                    Convert

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

    - Use System.Xml?

    - **** - XmlReader
    - **** - XmlWriter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / BOX2D
------------------

    - Use Box2DSharp?

    -      - Body
    -      - BodyDef
    -      - Box2D
    -      - Box2DDebugRenderer
    -      - ChainShape
    -      - CircleShape
    -      - Contact
    -      - ContactFilter
    -      - ContactImpulse
    -      - IContactListener
    -      - IDestructionListener
    -      - EdgeShape
    -      - Filter
    -      - Fixture
    -      - FixtureDef
    -      - Joint
    -      - JointDef
    -      - Manifold
    -      - MassData
    -      - PolygonShape
    -      - IQueryCallback
    -      - IRayCastCallback
    -      - Shape
    -      - Transform
    -      - World
    -      - WorldManifold

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / BOX2D / GRAPHICS
-----------------------------

    -      - ParticleEmitterBox2D

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / BOX2D / JOINTS
---------------------------

    -      - DistanceJoint
    -      - DistanceJointDef
    -      - FictionJoint
    -      - FrictionJointDef
    -      - GearJoint
    -      - GearJointDef
    -      - MotorJoint
    -      - MotorJointDef
    -      - MouseJoint
    -      - MouseJointDef
    -      - PrismaticJoint
    -      - PrismaticJointDef
    -      - PulleyJoint
    -      - PulleyJointDef
    -      - RevoluteJoint
    -      - RevoluteJointDef
    -      - RopeJoint
    -      - RopeJointDef
    -      - WeldJoint
    -      - WeldJointDef
    -      - WheelJoint
    -      - WheelJointDef

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/ TOOLS
-----------------

    -      - FileProcessor

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TILEDMAPPACKER
-----------------------------------

    -      - TiledMapPacker
    -      - TiledMapPackerTest
    -      - TiledMapPackerTestRender
    -      - TileSetLayout

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / BMFONT
-----------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / DISTANCEFIELD
------------------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / ETC1
---------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / FLAME
----------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / HEADERS
------------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / HIERO
----------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / IMAGEPACKER
----------------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / KTX
--------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / PARTICLEEDITOR
-------------------------------------------

    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS / TOOLS / TOOLS / TEXTUREPACKER
------------------------------------------

    -      - ColorBleedEffect
    -      - GridPacker
    -      - ImageProcessor
    -      - MaxRectsPacker
    -      - TexturePacker
    -      - TexturePackerFileProcessor
    -      - TexturePackerTest
    -      - TexturePackerUpscaleTest
    -      - TextureUnpacker
