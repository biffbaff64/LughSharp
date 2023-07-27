- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
LIBGDX CSharp CONVERSION - ROUND 1
----------------------------------
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- STEP 1: Complete all conversions so that the code will build.
- STEP 2: Refactor, where possible and necessary, to take advantage of C# language features.
-         Some classes can become structs / records instead.
-         Some/Most Get / Set method declarations in interfaces could become Properties.
-         switch expressions instead of switch statements where appropriate.
  -       Check methods to see if they can be virtual.
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
- Constantly look for opportunities to improve this code.
- There seems to be different namings for width/height etc properties and methods. Make it more uniform

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ROOT
----

    - DONE - Gdx                            
    -      - LibraryConstants

CORE
----

    - DONE - AbstractGraphics               
    - DONE - AbstractInput                  
    - DONE - ApplicationAdapter             
    - DONE - ApplicationBase                New
    - DONE - Game                           
    - DONE - GDXVersion                     
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS
------

    - DONE - AssetDescriptor                
    - DONE - AssetLoaderParameters          
    - DONE - AssetLoadingTask               
    - DONE - AssetManager                   
    - DONE - IAssetErrorListener            
    - DONE - RefCountedContainer            

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

ASSETS/LOADERS
--------------

    - DONE - AssetLoader                    
    - DONE - AsynchronousAssetLoader        
    - DONE - BitmapFontLoader               
    - DONE - CubemapLoader                  
    - DONE - I18NBundleLoader               
    - DONE - ModelLoader                    
    - DONE - MusicLoader                    
    - DONE - ParticleEffectLoader           
    - DONE - PixmapLoader                   
    - DONE - ShaderProgramLoader            
    - DONE - SkinLoader                     
    - DONE - SoundLoader                    
    - DONE - SynchronousAssetLoader         
    - DONE - TextureAtlasLoader             
    - DONE - TextureLoader                  

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

    - DONE - IAudioDevice                   
    - DONE - IAudioRecorder                 
    - DONE - IMusic                         
    - DONE - ISound                         

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

FILES
-----

    - IP   - BufferedReader
    - DONE - CloseableStreamReader
    - DONE - CloseableStreamWriter
    - DONE - DataInputStream
    - DONE - DataOutputStream
    - DONE - IDataInput
    - DONE - IDataOutput
    - DONE - Reader
    - DONE - WindowsFolderUtils
    - DONE - Writer
    -      - 

    - java.io.BufferedReader                -> 
    - java.io.BufferedInputStream           -> 
    - java.io.ByteArrayOutputStream         -> 
    - java.io.DataInputStream               -> BinaryReader
    - java.io.DataOutputStream              -> BinaryWriter
    - java.io.File                          -> 
    - java.io.FileHandle                    -> 
    - java.io.FileHandleStream              -> 
    - java.io.FilterOutputStream            -> 
    - java.io.DataOuput                     -> (Interface)
    - java.io.InputStream                   -> StreamReader
    - java.io.InputStreamReader             -> StreamReader
    - java.io.IOException                   -> System.IO.IOException
    - java.io.OutputStream                  -> StreamWriter
    - java.io.Reader                        -> 

    - Would MemoryStream cover all these?
    - java.nio.Buffer                       ->  
    - java.nio.BufferUtils                  ->  
    - java.nio.ByteBuffer                   ->  
    - java.nio.ByteBuffer                   -> 
    - java.nio.CharBuffer                   -> 
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

    - DONE - BufferFormat                       record class
    - DONE - Camera                             
    - DONE - Color                              
    - DONE - Colors                             
    - DONE - Cubemap                            
    - DONE - DisplayMode                        
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
    - DONE - Monitor                            
    - DONE - OrthographicCamera                 
    - DONE - PerspectiveCamera                  
    - DONE - Pixmap                             
    - IP   - PixmapIO                           
    - DONE - Texture                            
    - DONE - TextureArray                       
    - DONE - TextureFilter                      
    - DONE - TextureWrap                        
    - DONE - VertexAttribute                    
    - DONE - VertexAttributes                   

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G2D
------------

    - DONE - Animation                      
    - DONE - AtlasRegion                    
    - DONE - AtlasSprite                    
    - IP   - BitmapFont                     
    - IP   - BitmapFontCache                
    - DONE - CpuSpriteBatch                 
    -      - DistanceFieldFont              
    - DONE - Gdx2DPixmap                    
    - DONE - GlyphLayout                    
    - DONE - IBatch                         
    - DONE - IPolygonBatch                  
    - DONE - NinePatch                      
    - IP   - ParticleEffect                 
    -      - ParticleEffectPool             
    -      - ParticleEmitter                
    -      - PixmapPacker                   
    -      - PixmapPacker10                 
    -      - PolygonRegion                  
    -      - PolygonRegionLoader            
    -      - PolygonSprite                  
    -      - PolygonSpriteBatch             
    -      - RepeatablePolygonSprite        
    - DONE - Sprite                         
    - DONE - SpriteBatch                    
    - DONE - SpriteCache                    
    - DONE - TextureAtlas                   
    - DONE - TextureAtlasData               
    - DONE - TextureAtlasDataExtensions     
    - DONE - TextureRegion                  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/GLUTILS
----------------

    - IP   - ETC1                                   Find alternative / Has native
    - IP   - ETC1TextureData                        Find alternative / Has native
    - DONE - FacedCubemapData
    - IP   - FileTextureArrayData
    - IP   - FileTextureData
    - IP   - FloatFrameBuffer                       Inheritance order correct
    - DONE - FloatFrameBufferBuilder                Inheritance order correct
    - DONE - FloatTextureData
    - DONE - FrameBuffer                            Inheritance order correct
    - DONE - FrameBufferBuilder                     Inheritance order correct
    -      - FrameBufferCubemap
    - DONE - FrameBufferCubemapBuilder
    - DONE - FrameBufferRenderBufferAttachmentSpec
    - DONE - FrameBufferTextureAttachmentSpec
    - DONE - GLFrameBuffer                          Inheritance order correct
    - DONE - GLFrameBufferBuilder
    - DONE - GLOnlyTextureData
    - DONE - GLVersion
    - DONE - HdpiMode
    - DONE - HdpiUtils
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
    - DONE - ShaderProgram
    - DONE - ShapeRenderer

        The following classes have a lot in common.
        Perhaps create a base class they all extend from?

    - DONE - VertexArray
    - DONE - VertexBufferObject
    - DONE - VertexBufferObjectSubData
    - DONE - VertexBufferObjectWithVAO

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

    - DONE - IImageResolver                         
    - DONE - IMapRenderer                           
    - DONE - Map                                    
    - DONE - MapGroupLayer                          
    - DONE - MapLayer                               
    - DONE - MapLayers                              
    - DONE - MapObject                              
    - DONE - MapObjects                             
    - DONE - MapProperties                          

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/OBJECTS
------------

    - DONE - CircleMapObject                        
    - DONE - EllipseMapObject                       
    - DONE - PolygonMapObject                       
    - DONE - PolylineMapObject                      
    - DONE - RectangleMapObject                     
    - DONE - TextureMapObject                       

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED
----------

    - DONE - ITiledMapTile                          
    - DONE - TiledMap                               
    - DONE - TiledMapImageLayer                     
    - DONE - TiledMapTileLayer                      
    - DONE - TiledMapTileSet                        
    - DONE - TiledMapTileSets                       

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/LOADERS
------------------

    - IP   - AtlasTmxMapLoader                      
    - IP   - BaseTmxMapLoader                       
    -      - TideMapLoader                          Q: Should I support Tide?
    - IP   - TmxMapLoader                           

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/OBJECTS
------------------

    - DONE - TiledMapTileMapObject                  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/RENDERERS
--------------------

    - DONE - BatchTiledMapRenderer                  
    - DONE - HexagonalTiledMapRenderer              
    -      - IsometricStaggeredTiledMapRenderer     
    - DONE - IsometricTiledMapRenderer              
    - DONE - ITiledMapRenderer                      
    -      - OrthoCachedTiledMapRenderer            
    - DONE - OrthogonalTiledMapRenderer             

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MAPS/TILED/TILES
----------------

    - DONE - AnimatedTileMapTile                    
    - DONE - StaticTiledMapTile                     

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
    - DONE - ConvexHull
    - DONE - CumulativeDistribution
    -      - DelaunayTriangulator
    -      - EarClippingTriangulator
    - DONE - Ellipse
    - DONE - FloatCounter
    - DONE - Frustrum
    - DONE - GeometryUtils
    - DONE - GridPoint2
    - DONE - GridPoint3
    - IP   - Interpolation
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
    - DONE - Quaternion                     Documentation needs completing
    - DONE - RandomXS128
    - DONE - RectangleShape
    - DONE - Vector2                        Convert, I prefer the way the LibGDX class works.
    - DONE - Vector3                        Convert, I prefer the way the LibGDX class works.
    - DONE - WindowedMean

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATH/COLLISION
--------------

    - DONE - BoundingBox            Struct? / Record?
    - DONE - Ray                    
    - DONE - Segment                
    - DONE - Sphere                 

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

    - DONE - Action                         
    - DONE - Actor                          
    - DONE - Event                          
    - DONE - Group                          
    - DONE - IEventListener                 
    - DONE - InputEvent                     
    - DONE - InputListener                  
    - DONE - Stage                          
    - DONE - Touchable                      
    - DONE - TouchFocus                     

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/ACTIONS
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/UI
-----------------

    - DONE - Button                         
    - DONE - ButtonGroup                    
    - DONE - Cell                           
    - DONE - CheckBox                       
    - DONE - Container                      
    -      - Dialog                         
    -      - HorizontalGroup                
    - DONE - Image                          
    -      - ImageButton                    
    -      - ImageTextButton                
    - DONE - Label                          
    -      - List                           
    -      - ParticleEffectActor            
    - DONE - ProgressBar                    
    -      - ScrollPane                     
    -      - SelectBox                      
    - DONE - Skin                           
    -      - Slider                         
    -      - SplitPane                      
    - DONE - Stack                          
    - DONE - Table                          
    -      - TextArea                       
    -      - TextButton                     
    -      - TextField                      
    -      - TextTooltip                    
    -      - TooltipManager                 
    -      - Touchpad                       
    -      - Tree                           
    - DONE - Value                          
    - DONE - ValueExtensions                New Class
    - DONE - VerticalGroup                  
    - DONE - Widget                         
    - DONE - WidgetGroup                    
    - DONE - Window                         

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/UTILS
--------------------

    -      - ActorGestureListener           
    -      - ArraySelection                 
    - DONE - BaseDrawable                   
    - DONE - ChangeListener                 
    - DONE - ClickListener                  
    -      - DragAndDrop                    
    -      - DragListener                   
    -      - DragScrollListener             
    -      - FocusListener                  
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
    ----------------------------------------------------------------------------------
    - **** - Base64Coder                    Use System.Convert classes
    ----------------------------------------------------------------------------------
    - DONE - BinaryHeap
    - DONE - Bits                           
    ----------------------------------------------------------------------------------
    - DONE - CharHelper                     Might be added to.
    ----------------------------------------------------------------------------------
    - DONE - ComparableTimSort              
    - DONE - DataInput                      Check
    -      - DataOutput                     Check
    - DONE - DataUtils                      Added Class
    - DONE - FloatConsts                    Done, but is it needed?
    -      - GdxNativesLoader
    -      - I18NBundle                     Check
    - DONE - ICloseable                     
    - DONE - IClipboard                     Convert - Interface, actual clipboard handled in backends.                 
    - DONE - IRunnable                      Done, but is it needed?
    -      - LittleEndianInputStream        Check
    - DONE - Logger                         
    -      - PauseableThread
    - DONE - PerformanceCounter             Check
    - DONE - PerformanceCounters            Check
    - DONE - PropertiesUtils                Convert, but check if necessary first
    -      - QuadTreeFloat
    -      - Queue
    - DONE - QuickSelect
    - DONE - Scaling
    - DONE - ScreenUtils                    Needs ByteBuffer.Read implementing
    - DONE - Select                         Renamed Selector
    -      - SharedLibraryLoader            Check
    - DONE - SortUtils
    - **** - SortedIntList                  Use SortedList<int>
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
    - DONE - Trace                          Addition

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/ASYNC
-----------

     later, purely for education purposes.

    - **** - AsyncExecutor
    - **** - AsyncResult|
    - **** - IAsyncTask
    - **** - ThreadUtils

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/BUFFERS
-------------

    - DONE - Buffer
    - IP   - BufferUtils
    - DONE - ByteBuffer
    - IP   - CharBuffer
    - IP   - DirectByteBuffer
    - IP   - DoubleBuffer
    - IP   - FloatBuffer
    - IP   - HeapByteBuffer
    - IP   - HeapCharBuffer
    - IP   - IntBuffer
    - IP   - LongBuffer
    - IP   - MappedByteBuffer
    - IP   - ShortBuffer
    - IP   - StringCharBuffer
    -      - 
    -      - 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/COLLECTIONS
-----------------

    Note: C# has IEnumerable and IEnumerator interfaces...

    - DONE - Array<T>                       Converted, but use List<T> so can be deprecated
    - DONE - CollectionsData
    - DONE - DelayedRemovalArray            Convert / Extend List<>
    - DONE - IdentityMap<K, V>              Convert / extend Dictionary< object, object > 
    - **** - IPredicate
    -      - ListQueue                      ?????
    -      - ListStack                      ?????
    - DONE - ObjectMap                      Use Dictionary< object, object > for most instances.
    - IP   - ObjectSet<T>                   Use List<T>
    - **** - OrderedSet<T>                  Use SortedSet<T> ?
    - **** - PredicateIterable
    - **** - PredicateIterator
    - DONE - SnapshotArray                  Convert / extend List<T>


    - **** - Array                          Use List<>
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
    - **** - OrderedMap<K, V>               Use SortedDictionary<K, V> ?
    - **** - ShortArray                     Use List< short >

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/COLLECTIONS/EXTENSIONS
----------------------------

    - IP   - DictionaryExtensions
    - IP   - ListExtensions
    - IP   - SortedSetExtensions

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/COMPRESSION
-----------------

    Convert later, purely for education purposes.

    - **** - CRC
    - **** - IChecksum
    - **** - ICodeProgress
    - **** - LzmaAdapted

UTILS/COMPRESSION/LZ
--------------------

    Convert later, purely for education purposes.

    - **** - BinTree
    - **** - InWindow
    - **** - OutWindow

UTILS/COMPRESSION/LZMA
----------------------

    Convert later, purely for education purposes.

    - **** - Base
    - **** - Decoder
    - **** - Encoder

UTILS/COMPRESSION/RANGECODER
----------------------------

    Convert later, purely for education purposes.

    - **** - BitTreeDecoder
    - **** - BitTreeEncoder
    - **** - Decoder
    - **** - Encoder

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/CONCURRENT
----------------

    Convert later, purely for education purposes.

    - **** - AtomicInteger
    - **** - AtomicQueue
    - **** - AtomicReferenceArray

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/EXCEPTIONS
----------------

    - DONE - AssetNotLoadedException        
    - DONE - BufferOverflowException        
    - DONE - BufferUnderflowException       
    - DONE - GdxRuntimeException            
    - DONE - IllegalStateException          
    - DONE - InvalidMarkException           
    - DONE - NoSuchElementException         
    - DONE - NumberFormatException          
    - DONE - ReadOnlyBufferException        
    - DONE - UnsupportedEncodingException   
    - DONE - UnsupportedOperationException  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/JSON
----------

    Use an existing Json library, but this could be converted at a later date.

    - **** - IBaseJsonReader
    - **** - Json
    - **** - JsonReader
    - **** - JsonValue
    - **** - JsonWriter
    - **** - UBJsonReader
    - **** - UBJsonWriter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/POOLING
-------------

    - DONE - FlushablePool                  
    - DONE - IPoolable                      
    - DONE - Pool                           
    - DONE - PooledLinkedList               
    - DONE - Pools                          
    - DONE - ReflectionPool                 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/REFLECT
-------------

    Convert later, purely for education purposes.

    - **** - Annotation
    - **** - ArrayReflection
    - **** - ClassReflection
    - **** - Constructor
    - **** - Field
    - **** - Method
    - **** - ReflectionException
    - **** - ReflectionPool

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/REGEX
-----------

    USE C# REGEX LIBS, THEY'RE MUCH EASIER!!

    - **** - ASCII
    - **** - IMatchResult
    - **** - Matcher
    - **** - Pattern
    - **** - PatternCompanions
    - **** - PatternHelper
    - **** - PatternSyntaxException
    - **** - UnicodeProp

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

UTILS/XML
---------

    Use C# XML Libraries, but convert these, eventually, anyway.

    - DONE - XmlReader
    - DONE - XmlWriter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
