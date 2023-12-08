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
    - DONE - ClasspathFileHandleResolver
    - DONE - ExternalFileHandleResolver
    - DONE - IFileHandleResolver
    - DONE - InternalFileHandleResolver
    - DONE - LocalFileHandleResolver
    - DONE - PrefixFileHandleResolver
    - DONE - ResolutionFileResolver

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

AUDIO/JAVAZOOMJL
----------------

    Could this be scrapped and MP3Sharp be used instead?

    - DONE - BitReserve
    - IP   - Bitstream
    - IP   - BitstreamException
    - IP   - Crc16
    - IP   - DecoderException
    - DONE - Header
    - IP   - HuffCodeTab
    - IP   - IFrameDecoder
    - IP   - LayerIDecoder
    - IP   - LayerIIDecoder
    - IP   - LayerIIIDecoder
    - IP   - Mp3Decoder
    - IP   - OutputBuffer
    - IP   - OutputChannels
    - IP   - PushbackInputStream
    - IP   - SynthesisFilter
    - IP   - SynthesisFilterData ( partial of SynthesisFilter )

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

BACKENDS/DESKTOPGL
------------------

    - IP   - DesktopGLApplication
    - DONE - DesktopGLApplicationConfiguration
    - IP   - DesktopGLNativesLoader
    - IP   - DesktopGLNet
    - IP   - DesktopWindowCallbacks
    - DONE - IDesktopGLApplicationBase
    - IP   - Sync

    Files
    -----

    - IP   - DesktopGLFileHandle
    - DONE - DesktopGLFiles

    Graphics
    --------

    - IP   - DesktopGL20
    - IP   - DesktopGL30
    - IP   - DesktopGLGraphics

    Input
    -----

    - DONE - DefaultGLInput
    - DONE - IDesktopGLInput

    Utils
    -----

    - IP   - DesktopGLApplicationLogger
    - IP   - DesktopGLClipboard
    - IP   - DesktopGLCursor
    - IP   - DesktopGLPreferences

    Window
    ------

    - IP   - DesktopGLWindow
    - IP   - DesktopGLWindowConfiguration
    - IP   - DesktopGLWindowAdapter
    - IP   - IDesktopGLWindowListener

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

BACKENDS/DESKTOPGL/AUDIO
------------------------

    - DONE - IGLAudio
    -      - Mp3
    -      - Ogg
    -      - OggInputStream
    -      - OpenALAudioDevice
    -      - OpenALGLAudio
    -      - OpenALMusic
    -      - OpenALSound
    -      - SoundAudioRecorder
    -      - Wav

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

BACKENDS/DESKTOPGL/AUDIO/MOCK
-----------------------------

    - DONE - MockAudio
    - DONE - MockAudioDevice
    - DONE - MockAudioRecorder
    - DONE - MockMusic
    - DONE - MockSound

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
            - DONE - DisplayModeDescriptor
            - DONE - MonitorDescriptor
            - DONE - BufferFormatDescriptor
    - DONE - IInput
            - DONE - Buttons
            - DONE - Keys
            - DONE - ITextInputListener
    - DONE - IInputProcessor                
    - DONE - ILifecycleListener             
    - DONE - INet
            - DONE - IHttpResponse
            - DONE - IHttpMethods
            - DONE - IHttpResponseListener
            - DONE - HttpRequest
    - DONE - InputAdapter                   
    - DONE - InputEventQueue                
    - DONE - IPreferences                   
    - DONE - IScreen                        
    - DONE - ScreenAdapter                  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/BOX2D
----------------

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/TOOLS
----------------

    - IP   - FileProcessor

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/TOOLS/GDX-TOOLS
--------------------------

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/TOOLS/GDX-TOOLS/TEXTUREPACKER
----------------------------------------

    -      - ColorBleedEffect
    -      - TexturePacker

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/TOOLS/IMAGEPACKER
----------------------------

    - IP   - ImagePacker

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/TOOLS/TILEDMAPPACKER
-------------------------------

    - IP   - TileMapPacker

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

FILES
-----

    TODO: Remove all this and properly integrate the C# file handling classes

    - IP   - BufferedStreamExtensions
    - IP   - ByteArrayInputStream
    - DONE - FileHandle
    - DONE - InputStream

    ------------------------------------------
    - java.io.BufferedReader                -> 
    - java.io.BufferedInputStream           -> BufferedStream
    - java.io.ByteArrayInputStream          -> 
    - java.io.ByteArrayOutputStream         -> 
    - java.io.DataInputStream               -> BinaryReader
    - java.io.DataOutputStream              -> BinaryWriter
    - java.io.File                          -> 
    - java.io.FileHandle                    -> FileInfo ?
    - java.io.FileInputStream               -> 
    - java.io.FileOutputStream              -> 
    - java.io.FilterInputStream             -> 
    - java.io.FilterOutputStream            -> 
    - java.io.DataOuput                     -> (Interface)
    - java.io.InputStream                   -> StreamReader
    - java.io.InputStreamReader             -> StreamReader
    - java.io.IOException                   -> System.IO.IOException
    - java.io.OutputStream                  -> StreamWriter
    - java.io.Reader                        -> 

    ------------------------------------------

    - Look into existing C# ZIP libraries

    - java.util.zip.CRC32                   -> 
    - java.util.zip.CheckedOutputStream     -> 
    - java.util.zip.Deflater                -> 
    - java.util.zip.DeflaterOutputStream    -> 
    - java.util.zip.InflaterInputStream     -> 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

FILES/BUFFERS
-------------

    - DONE - Buffer
    - DONE - BufferUtils
    - DONE - ByteBuffer
    - DONE - CharBuffer
    - IP   - DirectByteBuffer
    - IP   - DoubleBuffer
    - DONE - FloatBuffer
    - IP   - HeapByteBuffer
    - IP   - HeapCharBuffer
    - IP   - HeapFloatBuffer
    - IP   - HeapShortBuffer
    - IP   _ IDirectBuffer
    - IP   - IntBuffer
    - IP   - LongBuffer
    - IP   - MappedByteBuffer
    - DONE - ShortBuffer
    - DONE - StringCharBuffer

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS
--------

    - DONE - Camera                             
    - IP   - Color                              
    - DONE - Colors                             
    - DONE - Cubemap
            - DONE - CubemapSide
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
            - DONE - TextureArrayDataFactory
    - DONE - ITextureData             
            - DONE - Factory
    - DONE - Mesh                               
    - DONE - OrthographicCamera                 
    - DONE - PerspectiveCamera                  
    - DONE - Pixmap              
            - DONE - PixmapFormatExtensions
    - DONE - PixmapFormat                             
    - DONE - PixmapIO
            - DONE - CIM
            - DONE - PNG
                    - DONE - ChunkBuffer
    - DONE - Texture                            
    - DONE - TextureArray                       
    - DONE - TextureFilter                      
    - DONE - TextureWrap                        
    - DONE - VertexAttribute                    
    - DONE - VertexAttributes
            - DONE - Usage

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
            - DONE - Glyph
            - DONE - BitmapFontData
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
            - DONE - PooledEffect
    - DONE - ParticleEmitter           
            - DONE - Particle
            - DONE - ParticleValue
            - DONE - NumericValue
            - DONE - RangedNumericValue
            - DONE - ScaledNumericValue
            - DONE - IndependentScaledNumericValue
            - DONE - GradientColorValue
            - DONE - ParticleSpawnShapeValue
    - DONE - PixmapPacker
            - DONE - Page
            - DONE - GuillotineStrategy
            - DONE - SkylineStrategy
            - DONE - PixmapPackerRectangle
            - DONE - IPackStrategy
    - DONE - PixmapPacker10       
            - DONE - ImageFormat
            - DONE - SaveParameters
    - DONE - PolygonRegion                  
    - DONE - PolygonRegionLoader
            - DONE - PolygonRegionParameters
    - DONE - PolygonSprite                  
    - DONE - PolygonSpriteBatch             
    - DONE - RepeatablePolygonSprite        
    - DONE - Sprite                         
    - DONE - SpriteBatch                    
    - DONE - SpriteCache
            - DONE - Cache
    - DONE - TextureAtlas                   
    - DONE - TextureAtlasData   
            - DONE - IField
            - DONE - Page
            - DONE - Region
    - DONE - TextureAtlasDataExtensions
            - DONE - ComparatorAnonymousInner
            - DONE - PageFieldParse
            - DONE - PageFieldFormat
            - DONE - PageFieldFilter
            - DONE - PageFieldRepeat
            - DONE - PageFieldPma
            - DONE - RegionFieldXY
            - DONE - RegionFieldSize
            - DONE - RegionFieldBounds
            - DONE - RegionFieldOffset
            - DONE - RegionFieldOrig
            - DONE - RegionFieldOffsets
            - DONE - RegionFieldRotate
            - DONE - RegionFieldIndex
    - DONE - TextureRegion                  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D
------------

    -      - Attribute
    -      - AttributesGroup
    -      - Environment
    -      - Material
    -      - Model
    -      - ModelBatch
    -      - ModelCache
    -      - ModelInstance
    -      - Renderable
    -      - RenderableProvider
    -      - Shader

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/ATTRIBUTES
-----------------------

    -      - BlendingAttribute
    -      - ColorAttribute
    -      - CubemapAttribute
    -      - DepthTestAttribute
    -      - DirectionalLightsAttribute
    -      - FloatAttribute
    -      - IntAttribute
    -      - PointLightsAttribute
    -      - SpotLightsAttribute
    -      - TextureAttribute

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/DECALS
-------------------

    -      - CameraGroupStrategy
    -      - Decal
    -      - DecalBatch
    -      - DecalMaterial
    -      - IGroupPlug
    -      - IGroupStrategy
    -      - PluggableGroupStrategy
    -      - SimpleOrthoGroupStrategy

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/ENV
----------------

    -      - AmbientCubemap
    -      - BaseLight
    -      - DirectionalLight
    -      - DirectionalShadowLight
    -      - IShadowMap
    -      - PointLight
    -      - SpericalHarmonics
    -      - SpotLight

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/LOADER
-------------------

    -      - G3ModelLoader
    -      - ObjLoader

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/MODELS
-------------------

    -      - Attribute
    -      - MeshPart
    -      - Node
    -      - NodeAnimation
    -      - NodeKeyframe
    -      - NodePart

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/MODELS/DATA
------------------------

    -      - ModelAnimation
    -      - ModelData
    -      - ModelMaterial
    -      - ModelMesh
    -      - ModelMeshPart
    -      - ModelNode
    -      - ModelNodeAnimation
    -      - ModelNodeKeyframe
    -      - ModelNodePart
    -      - ModelTexture

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/PARTICLES
----------------------

    -      - ParallelArray
    -      - ParticleChannels
    -      - ParticleController
    -      - ParticleControllerComponent
    -      - ParticleEffect
    -      - ParticleEffectLoader
    -      - ParticleShader
    -      - ParticleSorter
    -      - ParticleSystem
    -      - ResourceData

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/PARTICLES/BATCHES
------------------------------

    -      - BillboardParticleBatch
    -      - BufferedParticleBatch
    -      - IParticleBatch
    -      - ModelInstanceParticleBatch
    -      - PointSpriteParticleBatch

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/PARTICLES/EMITTERS
-------------------------------

    -      - Emitter
    -      - RegulatEmitter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/PARTICLES/INFLUENCERS
----------------------------------

    -      - ColorInfluencer
    -      - DynamicsInfluencer
    -      - DynamicsModifier
    -      - Influencer
    -      - ModelInfluencer
    -      - ParticleControllerFinalizerInfluencer
    -      - ParticleControllerInfluencer
    -      - RegionInfluencer
    -      - ScaleInfluencer
    -      - SimpleInfluencer
    -      - SpawnInfluencer

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/PARTICLES/RENDERERS
--------------------------------

    -      - BillboardControllerRenderData
    -      - BillboardRenderer
    -      - ModelInstanceControllerRenderData
    -      - ModelInstanceRenderer
    -      - ParticleControllerControllerRenderer
    -      - ParticleControllerRenderData
    -      - ParticleControllerRenderer
    -      - PointSpriteControllerRenderData
    -      - PointSpriteRenderer

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/PARTICLES/VALUES
-----------------------------

    -      - CylinderSpawnShapeValue
    -      - EllipseSpawnShapeValue
    -      - GradientColorValue
    -      - LineSpawnShapeValue
    -      - MeshSpawnShapeValue
    -      - NumericValue
    -      - ParticleValue
    -      - PointSpawnShapeValue
    -      - PrimitiveSpawnShapeValue
    -      - RangedNumericValue
    -      - RectangleSpawnShapeValue
    -      - ScaledNumericValue
    -      - SpawnShapeValue
    -      - UnweightedMeshSpawnShapeValue
    -      - WeightMeshSpawnShapeValue

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/SHADERS
--------------------

    -      - BaseShader
    -      - DefaultShader
    -      - DepthShader

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/UTILS
------------------

    -      - AnimationController
    -      - BaseAnimationController
    -      - BaseShaderProvider
    -      - CameraInputController
    -      - DefaultRenderableSorter
    -      - DefaultShaderProvider
    -      - DefaultTextureBinder
    -      - DepthShaderProvider
    -      - F|irstPersonCameraController
    -      - IMeshPartBuilder
    -      - IRenderableSorter
    -      - IShaderProvider
    -      - ITextureBinder
    -      - ITextureProvider
    -      - MeshBuilder
    -      - ModelBuilder
    -      - RenderContext
    -      - ShapeCache
    -      - TextureDescriptor

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

GRAPHICS/G3D/UTILS/SHAPEBUILDERS
--------------------------------

    -      - ArrowShapeBuilder
    -      - BaseShapeBuilder
    -      - BoxShapeBuilder
    -      - CapsuleShapeBuilder
    -      - ConeShapeBuilder
    -      - CylinderShapeBuilder
    -      - EllipseShapeBuilder
    -      - FrustrumShapeBuilder
    -      - PatchShapeBuilder
    -      - RenderableShapeBuilder
    -      - SphereShapeBuilder

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
    - DONE - IsometricStaggeredTiledMapRenderer     
    - DONE - IsometricTiledMapRenderer              
    - DONE - ITiledMapRenderer                      
    - DONE - OrthoCachedTiledMapRenderer            
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MATH/COLLISION
--------------

    - DONE - BoundingBox
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

SCENES/SCENE2D/LISTENERS
------------------------

    - IP   - ActorGestureListener           
    - IP   - ChangeListener                 
    - IP   - ClickListener                  
    - IP   - DragListener                   
    - IP   - DragScrollListener             
    - IP   - FocusListener                  
    - IP   - IEventListener                 
    - IP   - InputListener                  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

SCENES.SCENE2D/UI
-----------------

    TODO: I don't like the way Cell and Value classes are implemented.
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
    - DONE - ScrollPane        
    - IP   _ ScrollPaneListeners
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
    - IP   - Tree                           Generics have become confused, come back later
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
    - IP   - SharedLibraryLoader
            - IP   - SystemHelpers
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/ASYNC - DELETE CANDIDATE
-----------

    - **** - AsyncExecutor
    - **** - ASyncResult
    - **** - IAsyncTask
    - **** - ThreadUtils

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/COLLECTIONS
-----------------

    - DONE - Array<T>                       Converted, but use List<T>.
    - DONE - CollectionsData
    - DONE - DelayedRemovalArray            Convert / Extend List<>
    - DONE - IdentityMap<K, V>              Convert / extend Dictionary< object, object > 
    - DONE - ObjectMap                      Converted, but use Dictionary< object, object >
    - DONE - PredicateIterable
    - DONE - PredicateIterator
    - DONE - SnapshotArray<T>

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
    - **** - IPredicate                     ????
    - **** - LongArray                      Use List< long >
    - **** - LongMap                        Use Dictionary< long, V >
    - **** - ObjectFloatMap                 Use Dictionary< object, float >
    - **** - ObjectIntMap                   Use Dictionary< object, int >
    - **** - ObjectLongMap                  Use Dictionary< object, long >
    - **** - ObjectSet<T>                   Use List<T>
    - **** - OrderedMap<K, V>               Use SortedDictionary<K, V> ?
    - **** - OrderedSet<T>                  Use SortedSet<T>
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
    - DONE - InvalidTypeException       
    - DONE - MemberNullException
    - DONE - MissingResourceException
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
