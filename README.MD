C# 2D Game Framework heavily inspired by the Java LibGDX Game Framework.

It is NOT a 100% conversion, some things may be different.

TODO List is below.

MAJOR OBSTACLES SO FAR:-

    - File Handling / Differences with Java
    - Listener / Observer pattern implementations

DISCLAIMER:- THE CODE QUALITY IN THIS PROJECT WILL IMPROVE OVER TIME. C# IS NEW TO ME.

The standard of the code here won't be great to start off with, but will hopefully improve as I
learn more about C#.

Project name is to change. Current candidates are:-

    - LughEngine
    - Lugh
    - LughGDX
    - SharpGDX

As far as I can see, I do not need to convert:-

    - Regex
    - Reflection
    - Concurrent ( AtomicInteger, AtomicQueue etc )
    - Async?
    - Network
    - Compression?

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

LIBGDX CSharp CONVERSION - ROUND 1
----------------------------------

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- STEP 1: Complete all conversions so that the code will build.
- STEP 2: Refactor, where possible and necessary, to take advantage of C# language features.
-         Some classes can become structs / records instead.
-         Some/Most Get / Set method declarations in interfaces could become Properties.
-         switch expressions instead of switch statements where appropriate.
-         switch expressions instead of if..if/else..else where appropriate.
-         Check methods to see if they can be virtual.
-         Use [PublicApi] where necessary.
-         Use sealed classes only where strictly necessary. Users should be able to extend library classes.
-         Use of virtual for base classes/methods and classes that are likely to be extended is essential.
- STEP 3: Resolve ALL remaining TODOs.
- STEP 4: Ensure code is fully documented.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- NO MAGIC NUMBERS!!!
-

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- SORT OUT VERSIONING!!!

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- IP = Conversion In Progress.
- DONE = Class finished but may not be fully 'CSharp-ified'

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

- PRIORITY is 2D classes first

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

AUDIO/MP3SHARP
--------------

    - DONE - Buffer16BitSterso
    - DONE - MP3SharpException
    - DONE - MP3Stream
    - DONE - SoundFormat

    Decoding
    --------

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

    Decoders
    --------

    - DONE - ASubband
    - DONE - IFrameDecoder
    - DONE - LayerIDecoder
    - DONE - LayerIIDecoder
    - DONE - LayerIIIDecoder

    Decoders/LayerI
    ---------------

    - DONE - SubbandLayer1
    - DONE - SubbandLayer1IntensityStereo
    - DONE - SubbandLayer1Stereo

    Decoders/LayerII
    ----------------

    - DONE - SubbandLayer2
    - DONE - SubbandLayer2IntensityStereo
    - DONE - SubbandLayer2Stereo

    Decoders/LayerIII
    -----------------

    - DONE - ChannelData
    - DONE - GranuleInfo
    - DONE - Layer3SideInfo
    - DONE - SBI
    - DONE - ScaleFactorData
    - DONE - ScaleFactorTable

    IO
    --

    - DONE - RandomAccessFileStream
    - DONE - RiffFile
    - DONE - WaveFile
    - DONE - WaveFileBuffer

    Support
    -------

    - DONE - SupportClass

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

FILES - DELETE CANDIDATE
------------------------

    TODO: Remove all this and properly integrate the C# file handling classes

    - IP   - BufferedStreamExtensions
    - IP   - ByteArrayInputStream
    - DONE - FileHandle
    - DONE - InputStream

    ------------------------------------------
    - java.io.BufferedReader                -> 
    - java.io.BufferedInputStream           -> BufferedStream ?
    - java.io.ByteArrayInputStream          -> 
    - java.io.ByteArrayOutputStream         -> 
    - java.io.DataInputStream               -> BinaryReader ?
    - java.io.DataOutputStream              -> BinaryWriter ?
    - java.io.File                          -> File ( static class )
    - java.io.FileHandle                    -> FileInfo + File + DirectoryInfo ?
    - java.io.FileInputStream               -> 
    - java.io.FileOutputStream              -> 
    - java.io.FilterInputStream             -> 
    - java.io.FilterOutputStream            -> 
    - java.io.DataOuput                     -> (Interface)
    - java.io.InputStream                   -> StreamReader ?
    - java.io.InputStreamReader             -> StreamReader ?
    - java.io.IOException                   -> System.IO.IOException
    - java.io.OutputStream                  -> StreamWriter ?
    - java.io.Reader                        -> StreamReader ?

    ------------------------------------------

    - Look into existing C# ZIP libraries

    - java.util.zip.CRC32                   -> 
    - java.util.zip.CheckedOutputStream     -> 
    - java.util.zip.Deflater                -> 
    - java.util.zip.DeflaterOutputStream    -> 
    - java.util.zip.InflaterInputStream     -> 

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

FILES/BUFFERS - DELETE CANDIDATE
--------------------------------

    - See if these can be replaced by any C# System classes

    - DONE - Buffer
    - IP   - BufferUtils
    - DONE - ByteBuffer
    - DONE - CharBuffer
    - IP   - CircularByteBuffer
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

    - Not sure about these. Am I supporting mobile? Probably?

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

    - DONE - AtlasTmxMapLoader 
    - DONE - BaseTmxMapLoader
    - DONE - TmxMapLoader
    - **** - TideMapLoader

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
    - DONE - IActor
    - DONE - InputEvent
    - DONE - Stage
    - DONE - Touchable
    - DONE - TouchFocus

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
i.e.    AddListener( new ClickListener()
        {
            // Clicked needs to be a Func<>
            Clicked = ( ev, x, y ) =>
            {
            }
        } );

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
    - IP   _ ScrollPaneListeners
    - IP   - SelectBox
    - DONE - Skin                           
    - DONE - Slider                         
    -      - SplitPane                      
    - DONE - Stack                          
    - DONE - Table                          
    - DONE - TextArea                       
    - DONE - TextButton                     
    - DONE - TextField      Check that _blinkTask and _KeyRepeatTask are disposed of properly                      
    -      - TextTooltip                    
    -      - TooltipManager                 
    -      - Touchpad                       
    - DONE - Tree
    - DONE - Value                          
    - DONE - ValueExtensions
    - DONE - VerticalGroup                  
    - DONE - Widget                         
    - DONE - WidgetGroup                    
    - DONE - Window                         

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
    - IP   - SharedLibraryLoader.SystemHelpers
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

    - DONE - Array<T>                       Converted, but use List<T> for most cases.
    - DONE - CollectionsData
    - DONE - DelayedRemovalArray            Convert / Extend List<>
    - IP   - IdentityMap<K, V>              Convert / extend Dictionary< object, object > 
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

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

UTILS/EXCEPTIONS
----------------

    These need more documentation!

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

    - TODO: Switch to System.Xml

    - IP   - XmlReader          Some 'unreachable code' fixes needed
    - DONE - XmlReaderData
    - DONE - XmlWriter

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

TESTS/CORE/GRAPHICS
-------------------

    - IP   - CameraTest

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

    - DONE - DesktopGLFileHandle
    - DONE - DesktopGLFiles

    Graphics
    --------

    - IP   - DesktopGL20
    - IP   - DesktopGL30
    - IP   - DesktopGLGraphics
    - IP   - DesktopGLGraphics.DesktopGLDisplayMode   Delete candidate
    - IP   - DesktopGLGraphics.DesktopGLMonitor       Delete candidate

    Input
    -----

    - DONE - DefaultGLInput
    - DONE - IDesktopGLInput

    Utils
    -----

    - IP   - DesktopGLApplicationLogger
    - IP   - DesktopGLClipboard
    - DONE - DesktopGLCursor
    - IP   - DesktopGLPreferences

    Window
    ------

    - IP   - DesktopGLWindow
    - DONE - DesktopGLWindowConfiguration
    - DONE - DesktopGLWindowAdapter
    - DONE - IDesktopGLWindowListener

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

EXTENSIONS/BOX2D
----------------

    - It's most likely best to recommend the use of an already available C# port.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/GDX-TOOLS
--------------------

    - IP   - FileProcessor

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/GDX-TOOLS/TILEDMAPPACKER
-----------------------------------

    - IP   - TileMapPacker
    -      - TiledMapPackerTest
    -      - TiledMapPackerTestRender
    -      - TileSetLayout

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/GDX-TOOLS/TOOLS
--------------------------

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/GDX-TOOLS/TOOLS/TEXTUREPACKER
----------------------------------------

    -      - ColorBleedEffect
    -      - TexturePacker

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

EXTENSIONS/GDX-TOOLS/IMAGEPACKER
--------------------------------

    - IP   - ImagePacker

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

