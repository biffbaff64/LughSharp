- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
LIBGDX CSharp CONVERSION - ROUND 1
----------------------------------

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- STEP 1: Complete all conversions so that the code will build.
- STEP 2: Refactor, where possible and necessary, to take advantage of C# language features.
-         Some classes can become structs / records instead.
-         Some/Most Get / Set method declarations in interfaces could become Properties.
-         switch expressions instead of switch statements where appropriate.
-         Check methods to see if they can be virtual.
-         Use [PublicApi] where necessary.
-         Use [UsedImplicitly] where necessary.
-         Use sealed classes only where strictly necessary. Users should be able to extend library classes.
- STEP 3: Resolve ALL remaining TODOs.
- STEP 4: Ensure code is fully documented.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- NO MAGIC NUMBERS!!!
- 
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- SORT OUT VERSIONING!!!

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- IP   = Conversion In Progress.
- DONE = Class finished but may not be fully 'CSharp-ified'

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- PRIORITY is 2D classes first

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- Restructure, where necessary, to cut the number of exceptions that can be thrown
- Constantly look for opportunities to improve this code.
- There seems to be different namings for width/height etc properties and methods. Make it more uniform
- Make more use of <inheritdoc cref=""/>> or just <inheritdoc/>
- 
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
    - **** - I18NBundleLoader               
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

FILES
-----

    - java.io.BufferedReader                -> 
    - java.io.BufferedInputStream           -> BufferedStream
    - java.io.ByteArrayOutputStream         -> 
    - java.io.DataInputStream               -> BinaryReader
    - java.io.DataOutputStream              -> BinaryWriter
    - java.io.File                          -> 
    - java.io.FileHandle                    -> 
    - java.io.FileInputStream               -> 
    - java.io.FileOutputStream              -> 
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
    - DONE - IGL31                              
    - DONE - IGL32                              
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
    - IP   - PolygonRegionLoader            
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/GLUTILS
----------------

    - **** - ETC1                                   Should I support this format?
    - **** - ETC1TextureData                        Should I support this format?
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/PROFILING
------------------

    - DONE - GL20Interceptor
    - DONE - GL30Interceptor
    - DONE - GLInterceptor
    - DONE - GLProfiler
    - DONE - IGLErrorListener

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

INPUT
-----

    Are these needed?

    -      - GestureDetector
    -      - RemoteInput
    -      - RemoteSender       Needs rework for compatibility with system.io.sockets.

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

    - DONE - AtlasTmxMapLoader                      Check for nullability issues 
    - DONE - BaseTmxMapLoader                       Check for nullability issues
    - DONE - TmxMapLoader                           Check for nullability issues

    - **** - TideMapLoader                          Should I support Tide?

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
    -      - DelaunayTriangulator
    -      - EarClippingTriangulator
    - DONE - Ellipse
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

NETWORK - DELETE CANDIDATE
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

SCENES/LISTENERS
----------------

    The Java Listeners implemented in LibGDX need replacing by C# Observers

    -      - ActorGestureListener           
    -      - ChangeListener                 
    -      - ClickListener                  
    -      - DragListener                   
    -      - DragScrollListener             
    -      - FocusListener                  
    -      - IEventListener                 
    -      - InputListener                  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES/SCENE2D
--------------

    - DONE - Action                         
    - DONE - Actor                          
    - DONE - Event                          
    - DONE - Group                          
    - DONE - InputEvent                     
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

    NOTE: I don't like the way Cell and Value classes are implemented.
          They seem confusing and are candidates for a rewrite.

    - IP   - Button                         
    - DONE - ButtonGroup                    
    - DONE - Cell                           
    - DONE - CheckBox                       
    - DONE - Container                      
    - IP   - Dialog                         
    - IP   - HorizontalGroup                
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
    - IP   - Tree                           Genewrics have become confused, come back later
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

    - DONE - ArraySelection                 
    - DONE - BaseDrawable                   
    - IP   - DragAndDrop                    
    - DONE - ICullable                      
    - DONE - IDisableable                   
    - DONE - IDrawable                      
    - DONE - ILayout                        
    - DONE - ITransformDrawable             
    - DONE - NinePatchDrawable              
    - DONE - ScissorStack                   
    - IP   - Selection                      
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
    - DONE - CharHelper
    - DONE - ComparableTimSort              
    - DONE - DataInput                      Check
    - DONE - DataOutput                     Check
    - DONE - DataUtils                      Added Class
    - DONE - FloatConsts                    Done, but is it needed?
    - DONE - ICloseable                     
    - DONE - IClipboard                     Convert - Interface, clipboard handled in backends.                 
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
    - DONE - SortUtils
    - DONE - StringTokenizer
    - DONE - Timer                          Use System.Timer instead??
    - DONE - TimeUtils
    - DONE - TimSort
    - DONE - Trace                          Addition

    -      - GdxNativesLoader               Check - Remove this??
    -      - LittleEndianInputStream        Check - Remove this??
    -      - PauseableThread                Check - Remove this??
    -      - SharedLibraryLoader            Check - Remove this??
    -      - StreamUtils                    Check - Remove this??

    - **** - Base64Coder                    Use System.Convert classes
    - **** - I18NBundle                     Remove, look into C# localisation
    - **** - Queue                          Use system.collection.generics.Queue<T>?
    - **** - SortedIntList                  Use SortedList<int>
    - **** - StringBuilder                  Use System.Text.StringBuilder
    - **** - TextFormatter                  Remove, look into C# localisation

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/BUFFERS
-------------

    IMPORTANT! - Establish whether or not I actually need these.

    - DONE - Buffer
    - IP   - BufferUtils
    - DONE - ByteBuffer
    - DONE - CharBuffer
    - DONE - FloatBuffer
    - DONE - StringCharBuffer

    Does C# have these classes already?
        - Could MemoryStream be used?

    - IP   - DirectByteBuffer
    - IP   - DoubleBuffer
    - IP   - HeapByteBuffer
    - IP   - HeapCharBuffer
    - IP   _ IDirectBuffer
    - IP   - IntBuffer
    - IP   - LongBuffer
    - IP   - MappedByteBuffer
    - IP   - ShortBuffer

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/COLLECTIONS
-----------------

    - DONE - Array<T>                       Converted, but use List<T>.
    - DONE - CollectionsData
    - DONE - DelayedRemovalArray            Convert / Extend List<>
    - DONE - IdentityMap<K, V>              Convert / extend Dictionary< object, object > 
    - DONE - SnapshotArray<T>               Insert the necessary bits from Array<T> / remove dependency

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
    - **** - IPredicate
    - **** - LongArray                      Use List< long >
    - **** - LongMap                        Use Dictionary< long, V >
    - **** - ObjectFloatMap                 Use Dictionary< object, float >
    - **** - ObjectIntMap                   Use Dictionary< object, int >
    - **** - ObjectLongMap                  Use Dictionary< object, long >
    - **** - ObjectMap                      Use Dictionary< object, object >
    - **** - ObjectSet<T>                   Use List<T>
    - **** - OrderedMap<K, V>               Use SortedDictionary<K, V> ?
    - **** - OrderedSet<T>                  Use SortedSet<T>
    - **** - PredicateIterable
    - **** - PredicateIterator
    - **** - ShortArray                     Use List< short >

    ----------------------------------------------------------------------------------

    - DONE - DictionaryExtensions
    - DONE - ListExtensions

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
    - DONE _ MemberNullException
    - DONE - NoSuchElementException         
    - DONE - NumberFormatException          
    - DONE - ReadOnlyBufferException        
    - DONE - UnsupportedEncodingException   
    - DONE - UnsupportedOperationException  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/POOLING
-------------

    - DONE - FlushablePool                  
    - DONE - IPoolable                      
    - DONE - Pool                           
    - DONE - PooledLinkedList               
    - DONE - Pools                          

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

    - DONE - XmlReader
    - DONE - XmlReaderData
    - DONE - XmlWriter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
