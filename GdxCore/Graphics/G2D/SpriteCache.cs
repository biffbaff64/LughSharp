using LibGDXSharp.Maths;

namespace LibGDXSharp.G2D;

/// <summary>
/// Draws 2D images, optimized for geometry that does not change. Sprites and/or
/// textures are cached and given an ID, which can later be used for drawing.
/// The size, color, and texture region for each cached image cannot be modified.
/// This information is stored in video memory and does not have to be sent to the
/// GPU each time it is drawn.
/// <para>
/// To cache Sprites or Textures, first call <see cref="BeginCache"/>, then call
/// the appropriate add method to define the images. To complete the cache,
/// call <see cref="EndCache"/> and store the returned cache ID.
/// </para>
/// <para>
/// To draw with SpriteCache, first call <see cref="Begin()"/>, then call
/// <see cref="Draw(int)"/> with a cache ID. When SpriteCache drawing is complete,
/// call <see cref="End()"/>.
/// </para>
/// <para>
/// By default, SpriteCache draws using screen coordinates and uses an x-axis
/// pointing to the right, an y-axis pointing upwards and the origin is the bottom
/// left corner of the screen. The default transformation and projection matrices
/// can be changed. If the screen is <see cref="IApplicationListener.Resize(int, int)"/>,
/// the SpriteCache's matrices must be updated. For example:
/// </para>
/// <code>cache.GetProjectionMatrix().SetToOrtho2D(0, 0, Gdx.Graphics.GetWidth(), Gdx.Graphics.GetHeight());</code>
/// <para>
/// Note that SpriteCache does not manage blending. You will need to enable blending
/// (<tt>Gdx.GL.GLEnable(IGL20.GL_Blend);</tt>) and set the blend func as needed before
/// or between calls to <see cref="Draw(int)"/>.
/// </para>
/// <para>
/// SpriteCache is managed. If the OpenGL context is lost and the restored, all OpenGL
/// resources a SpriteCache uses internally are restored.
/// </para>
/// <para>
/// SpriteCache is a reasonably heavyweight object. Typically only one instance should
/// be used for an entire application.
/// </para>
/// <para>
/// SpriteCache works with OpenGL ES 1.x and 2.0. For 2.0, it uses its own custom shader
/// to draw.
/// </para>
/// <para>
/// SpriteCache must be disposed once it is no longer needed.
/// </para> 
/// </summary>
public class SpriteCache
{
    private readonly static float[] tempVertices = new float[ Sprite.VertexSize * 6 ];

    private readonly Mesh          _mesh;
    private readonly Matrix4       _transformMatrix  = new Matrix4();
    private readonly Matrix4       _projectionMatrix = new Matrix4();
    private          bool          _drawing;
    private          List< Cache > _caches = new();

    private readonly Matrix4       _combinedMatrix = new Matrix4();
    private readonly ShaderProgram _shader;

    private          Cache           _currentCache;
    private readonly List< Texture > _textures = new(8);
    private readonly List< int >     _counts   = new(8);

    private readonly Color _color       = new Color( 1, 1, 1, 1 );
    private          float _colorPacked = Color.WhiteFloatBits;

    private ShaderProgram? _customShader = null;

    /** Number of render calls since the last {@link #begin()}. **/
    public int renderCalls = 0;

    /** Number of rendering calls, ever. Will not be reset unless set manually. **/
    public int totalRenderCalls = 0;

    /// <summary>
    /// Creates a cache that uses indexed geometry and can contain up to 1000 images.
    /// </summary>
    public SpriteCache() : this( 1000, false )
    {
    }

    /// <summary>
    /// Creates a cache with the specified size, using a default shader if
    /// OpenGL ES 2.0 is being used.
    /// </summary>
    /// <param name="size">
    /// The maximum number of images this cache can hold. The memory required
    /// to hold the images is allocated up front. Max of 8191 if indices.
    /// </param>
    /// <param name="useIndices">If true, indexed geometry will be used.</param>
    public SpriteCache( int size, bool useIndices )
        : this( size, CreateDefaultShader(), useIndices )
    {
    }

    private class Cache
    {
        protected int       id;
        protected int       offset;
        protected int       maxCount;
        protected int       textureCount;
        protected Texture[] textures;
        protected int[]     counts;

        public Cache( int id, int offset )
        {
            this.id     = id;
            this.offset = offset;
        }
    }
}