using LibGDXSharp.Maths;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.G2D;

/// <summary>
/// Draws 2D images, optimized for geometry that does not change. Sprites and/or
/// textures are cached and given an ID, which can later be used for _drawing.
/// The size, color, and texture region for each cached image cannot be modified.
/// This information is stored in video memory and does not have to be sent to the
/// GPU each time it is drawn.
/// <para>
/// To cache Sprites or Textures, first call <see cref="BeginCache()"/>, then call
/// the appropriate add method to define the images. To complete the cache,
/// call <see cref="EndCache"/> and store the returned cache ID.
/// </para>
/// <para>
/// To draw with SpriteCache, first call <see cref="Begin()"/>, then call
/// <see cref="Draw(int)"/> with a cache ID. When SpriteCache _drawing is complete,
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
    private          bool          _drawing;
    private readonly List< Cache > _caches = new();

    private readonly Matrix4        _combinedMatrix = new Matrix4();
    private readonly ShaderProgram? _shader;

    private          Cache?          _currentCache;
    private readonly List< Texture > _textures = new(8);
    private readonly List< int >     _counts   = new(8);

    private float _colorPacked = Color.WhiteFloatBits;

    /// <summary>
    /// Number of render calls since the last <see cref="Begin"/>.
    /// </summary>
    public int renderCalls = 0;

    /// <summary>
    /// Number of rendering calls, ever. Will not be reset unless set manually.
    /// </summary>
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

    /// <summary>
    /// Creates a cache with the specified size and OpenGL ES 2.0 shader.
    /// </summary>
    /// <param name="size">
    /// The maximum number of images this cache can hold. The memory required
    /// to hold the images is allocated up front. Max of 8191 if indices are used.
    /// </param>
    /// <param name="shader"></param>
    /// <param name="useIndices">If true, indexed geometry will be used.</param>
    public SpriteCache( int size, ShaderProgram shader, bool useIndices )
    {
        this._shader = shader;

        if ( useIndices && ( size > 8191 ) )
            throw new ArgumentException( "Can't have more than 8191 sprites per batch: " + size );

        _mesh = new Mesh
            (
             true,
             size * ( useIndices ? 4 : 6 ),
             useIndices ? size * 6 : 0,
             new VertexAttribute
                 (
                  VertexAttributes.Usage.Position,
                  2,
                  ShaderProgram.PositionAttribute
                 ),
             new VertexAttribute
                 (
                  VertexAttributes.Usage.ColorPacked,
                  4,
                  ShaderProgram.ColorAttribute
                 ),
             new VertexAttribute
                 (
                  VertexAttributes.Usage.TextureCoordinates,
                  2,
                  ShaderProgram.TexcoordAttribute + "0"
                 )
            )
            {
                AutoBind = false
            };

        if ( useIndices )
        {
            var   length  = size * 6;
            var   indices = new short[ length ];
            short j       = 0;

            for ( var i = 0; i < length; i += 6, j += 4 )
            {
                indices[ i + 0 ] = j;
                indices[ i + 1 ] = ( short )( j + 1 );
                indices[ i + 2 ] = ( short )( j + 2 );
                indices[ i + 3 ] = ( short )( j + 2 );
                indices[ i + 4 ] = ( short )( j + 3 );
                indices[ i + 5 ] = j;
            }

            _mesh.SetIndices( indices );
        }

        ProjectionMatrix.SetToOrtho2D( 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height );
    }

    /// Sets the color used to tint images when they are added to the
    /// SpriteCache. Default is <see cref="Color.White"/>.
    public void SetColor( Color tint )
    {
        Color.Set( tint );
        PackedColor = tint.ToFloatBits();
    }

    /// <summary>
    /// </summary>
    /// <see cref="SetColor(LibGDXSharp.Graphics.Color)"/>
    public void SetColor( float r, float g, float b, float a )
    {
        Color.Set( r, g, b, a );
        PackedColor = Color.ToFloatBits();
    }

    public Color Color { get; } = new Color( 1, 1, 1, 1 );

    /// <summary>
    /// The color of this sprite cache, expanding the alpha from 0-254 to 0-255.
    /// </summary>
    public float PackedColor
    {
        get => _colorPacked;
        set
        {
            Graphics.Color.Abgr8888ToColor( Color, value );
            _colorPacked = value;
        }
    }

    /// <summary>
    /// Starts the definition of a new cache, allowing the add and
    /// <see cref="EndCache()"/> methods to be called.
    /// </summary>
    public void BeginCache()
    {
        if ( _drawing )
            throw new IllegalStateException( "end must be called before beginCache" );

        if ( _currentCache != null )
            throw new IllegalStateException( "endCache must be called before begin." );

        _currentCache = new Cache( _caches.Count, _mesh.VerticesBuffer.Limit );
        _caches.Add( _currentCache );
        _mesh.VerticesBuffer.Compact();
    }

    /// <summary>
    /// Starts the redefinition of an existing cache, allowing the add and
    /// <see cref="EndCache()"/> methods to be called. If this is not the
    /// last cache created, it cannot have more entries added to it than when
    /// it was first created. To do that, use <see cref="Clear()"/> and then
    /// <see cref="Begin()"/>.
    /// </summary>
    public void BeginCache( int cacheID )
    {
        if ( _drawing )
            throw new IllegalStateException( "end must be called before beginCache" );

        if ( _currentCache != null )
            throw new IllegalStateException( "endCache must be called before begin." );

        if ( cacheID == ( _caches.Count - 1 ) )
        {
            Cache oldCache = _caches.RemoveIndex( cacheID );
            _mesh.VerticesBuffer.Limit = oldCache.offset;
            BeginCache();

            return;
        }

        _currentCache                 = _caches[ cacheID ];
        _mesh.VerticesBuffer.Position = _currentCache.offset;
    }

    /// <summary>
    /// Ends the definition of a cache, returning the cache ID to be
    /// used with <see cref="Draw(int)"/>.
    /// </summary>
    public int EndCache()
    {
        if ( _currentCache == null )
            throw new IllegalStateException( "beginCache must be called before endCache." );

        Cache cache      = _currentCache;
        var   cacheCount = _mesh.VerticesBuffer.Position - cache.offset;

        if ( cache.textures == null )
        {
            // New cache.
            cache.maxCount     = cacheCount;
            cache.textureCount = _textures.Count;
            cache.textures     = _textures.ToArray();
            cache.counts       = new int[ cache.textureCount ];

            for ( int i = 0, n = _counts.Count; i < n; i++ )
            {
                cache.counts[ i ] = _counts[ i ];
            }

            _mesh.VerticesBuffer.Flip();
        }
        else
        {
            // Redefine existing cache.
            if ( cacheCount > cache.maxCount )
            {
                throw new GdxRuntimeException
                    (
                     $"If a cache is not the last created, it cannot be redefined"
                     + $"with more entries than when it was first created: "
                     + $"{cacheCount} ({cache.maxCount} max)"
                    );
            }

            cache.textureCount = _textures.Count;

            if ( cache.textures.Length < cache.textureCount )
            {
                cache.textures = new Texture[ cache.textureCount ];
            }

            for ( int i = 0, n = cache.textureCount; i < n; i++ )
            {
                cache.textures[ i ] = _textures[ i ];
            }

            if ( cache.counts?.Length < cache.textureCount )
            {
                cache.counts = new int[ cache.textureCount ];
            }

            if ( cache.counts != null )
            {
                for ( int i = 0, n = cache.textureCount; i < n; i++ )
                {
                    cache.counts[ i ] = _counts[ i ];
                }
            }

            FloatBuffer vertices = _mesh.VerticesBuffer;
            vertices.Position = 0;
            Cache lastCache = _caches[ _caches.Count - 1 ];
            vertices.Limit = ( lastCache.offset + lastCache.maxCount );
        }

        _currentCache = null!;
        _textures.Clear();
        _counts.Clear();

        return cache.id;
    }

    /// <summary>
    /// Invalidates all cache IDs and resets the SpriteCache so new _caches can be added.
    /// </summary>
    public void Clear()
    {
        _caches.Clear();
        _mesh.VerticesBuffer.Clear().Flip();
    }

    /// <summary>
    /// Adds the specified vertices to the cache. Each vertex should have 5
    /// elements, one for each of the attributes: x, y, color, u, and v. If
    /// indexed geometry is used, each image should be specified as 4 vertices,
    /// otherwise each image should be specified as 6 vertices.
    /// </summary>
    public void Add( Texture texture, float[] vertices, int offset, int length )
    {
        if ( _currentCache == null )
            throw new IllegalStateException( "beginCache must be called before add." );

        var verticesPerImage = _mesh.NumIndices > 0 ? 4 : 6;
        var count            = ( length / ( verticesPerImage * Sprite.VertexSize ) ) * 6;
        var lastIndex        = _textures.Count - 1;

        if ( ( lastIndex < 0 ) || ( _textures[ lastIndex ] != texture ) )
        {
            _textures.Add( texture );
            _counts.Add( count );
        }
        else
        {
            _counts[ lastIndex ] += count;
        }

        _mesh.VerticesBuffer.Put( vertices, offset, length );
    }

    /// <summary>
    /// Adds the specified texture to the cache.
    /// </summary>
    public void Add( Texture texture, float x, float y )
    {
        var fx2 = x + texture.Width;
        var fy2 = y + texture.Height;

        tempVertices[ 0 ] = x;
        tempVertices[ 1 ] = y;
        tempVertices[ 2 ] = PackedColor;
        tempVertices[ 3 ] = 0;
        tempVertices[ 4 ] = 1;

        tempVertices[ 5 ] = x;
        tempVertices[ 6 ] = fy2;
        tempVertices[ 7 ] = PackedColor;
        tempVertices[ 8 ] = 0;
        tempVertices[ 9 ] = 0;

        tempVertices[ 10 ] = fx2;
        tempVertices[ 11 ] = fy2;
        tempVertices[ 12 ] = PackedColor;
        tempVertices[ 13 ] = 1;
        tempVertices[ 14 ] = 0;

        if ( _mesh.NumIndices > 0 )
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = y;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = 1;
            tempVertices[ 19 ] = 1;

            Add( texture, tempVertices, 0, 20 );
        }
        else
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = fy2;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = 1;
            tempVertices[ 19 ] = 0;

            tempVertices[ 20 ] = fx2;
            tempVertices[ 21 ] = y;
            tempVertices[ 22 ] = PackedColor;
            tempVertices[ 23 ] = 1;
            tempVertices[ 24 ] = 1;

            tempVertices[ 25 ] = x;
            tempVertices[ 26 ] = y;
            tempVertices[ 27 ] = PackedColor;
            tempVertices[ 28 ] = 0;
            tempVertices[ 29 ] = 1;

            Add( texture, tempVertices, 0, 30 );
        }
    }

    /// <summary>
    /// Adds the specified texture to the cache.
    /// </summary>
    public void Add( Texture texture,
                     float x,
                     float y,
                     int srcWidth,
                     int srcHeight,
                     float u,
                     float v,
                     float u2,
                     float v2,
                     float color )
    {
        var fx2 = x + srcWidth;
        var fy2 = y + srcHeight;

        tempVertices[ 0 ] = x;
        tempVertices[ 1 ] = y;
        tempVertices[ 2 ] = color;
        tempVertices[ 3 ] = u;
        tempVertices[ 4 ] = v;

        tempVertices[ 5 ] = x;
        tempVertices[ 6 ] = fy2;
        tempVertices[ 7 ] = color;
        tempVertices[ 8 ] = u;
        tempVertices[ 9 ] = v2;

        tempVertices[ 10 ] = fx2;
        tempVertices[ 11 ] = fy2;
        tempVertices[ 12 ] = color;
        tempVertices[ 13 ] = u2;
        tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = y;
            tempVertices[ 17 ] = color;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v;

            Add( texture, tempVertices, 0, 20 );
        }
        else
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = fy2;
            tempVertices[ 17 ] = color;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v2;

            tempVertices[ 20 ] = fx2;
            tempVertices[ 21 ] = y;
            tempVertices[ 22 ] = color;
            tempVertices[ 23 ] = u2;
            tempVertices[ 24 ] = v;

            tempVertices[ 25 ] = x;
            tempVertices[ 26 ] = y;
            tempVertices[ 27 ] = color;
            tempVertices[ 28 ] = u;
            tempVertices[ 29 ] = v;

            Add( texture, tempVertices, 0, 30 );
        }
    }

    /// <summary>
    /// Adds the specified texture to the cache.
    /// </summary>
    public void Add( Texture texture, float x, float y, int srcX, int srcY, int srcWidth, int srcHeight )
    {
        var invTexWidth  = 1.0f / texture.Width;
        var invTexHeight = 1.0f / texture.Height;

        var u   = srcX * invTexWidth;
        var v   = ( srcY + srcHeight ) * invTexHeight;
        var u2  = ( srcX + srcWidth ) * invTexWidth;
        var v2  = srcY * invTexHeight;
        var fx2 = x + srcWidth;
        var fy2 = y + srcHeight;

        tempVertices[ 0 ] = x;
        tempVertices[ 1 ] = y;
        tempVertices[ 2 ] = PackedColor;
        tempVertices[ 3 ] = u;
        tempVertices[ 4 ] = v;

        tempVertices[ 5 ] = x;
        tempVertices[ 6 ] = fy2;
        tempVertices[ 7 ] = PackedColor;
        tempVertices[ 8 ] = u;
        tempVertices[ 9 ] = v2;

        tempVertices[ 10 ] = fx2;
        tempVertices[ 11 ] = fy2;
        tempVertices[ 12 ] = PackedColor;
        tempVertices[ 13 ] = u2;
        tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = y;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v;

            Add( texture, tempVertices, 0, 20 );
        }
        else
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = fy2;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v2;

            tempVertices[ 20 ] = fx2;
            tempVertices[ 21 ] = y;
            tempVertices[ 22 ] = PackedColor;
            tempVertices[ 23 ] = u2;
            tempVertices[ 24 ] = v;

            tempVertices[ 25 ] = x;
            tempVertices[ 26 ] = y;
            tempVertices[ 27 ] = PackedColor;
            tempVertices[ 28 ] = u;
            tempVertices[ 29 ] = v;

            Add( texture, tempVertices, 0, 30 );
        }
    }

    /// <summary>
    /// Adds the specified texture to the cache.
    /// </summary>
    public void Add( Texture texture,
                     float x,
                     float y,
                     float width,
                     float height,
                     int srcX,
                     int srcY,
                     int srcWidth,
                     int srcHeight,
                     bool flipX,
                     bool flipY )
    {

        var invTexWidth  = 1.0f / texture.Width;
        var invTexHeight = 1.0f / texture.Height;
        var u            = srcX * invTexWidth;
        var v            = ( srcY + srcHeight ) * invTexHeight;
        var u2           = ( srcX + srcWidth ) * invTexWidth;
        var v2           = srcY * invTexHeight;

        var fx2 = x + width;
        var fy2 = y + height;

        if ( flipX )
        {
            ( u, u2 ) = ( u2, u );
        }

        if ( flipY )
        {
            ( v, v2 ) = ( v2, v );
        }

        tempVertices[ 0 ] = x;
        tempVertices[ 1 ] = y;
        tempVertices[ 2 ] = PackedColor;
        tempVertices[ 3 ] = u;
        tempVertices[ 4 ] = v;

        tempVertices[ 5 ] = x;
        tempVertices[ 6 ] = fy2;
        tempVertices[ 7 ] = PackedColor;
        tempVertices[ 8 ] = u;
        tempVertices[ 9 ] = v2;

        tempVertices[ 10 ] = fx2;
        tempVertices[ 11 ] = fy2;
        tempVertices[ 12 ] = PackedColor;
        tempVertices[ 13 ] = u2;
        tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = y;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v;

            Add( texture, tempVertices, 0, 20 );
        }
        else
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = fy2;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v2;

            tempVertices[ 20 ] = fx2;
            tempVertices[ 21 ] = y;
            tempVertices[ 22 ] = PackedColor;
            tempVertices[ 23 ] = u2;
            tempVertices[ 24 ] = v;

            tempVertices[ 25 ] = x;
            tempVertices[ 26 ] = y;
            tempVertices[ 27 ] = PackedColor;
            tempVertices[ 28 ] = u;
            tempVertices[ 29 ] = v;

            Add( texture, tempVertices, 0, 30 );
        }
    }

    /// <summary>
    /// Adds the specified texture to the cache.
    /// </summary>
    public void Add( Texture texture,
                     float x,
                     float y,
                     float originX,
                     float originY,
                     float width,
                     float height,
                     float scaleX,
                     float scaleY,
                     float rotation,
                     int srcX,
                     int srcY,
                     int srcWidth,
                     int srcHeight,
                     bool flipX,
                     bool flipY )
    {

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;

        var fx  = -originX;
        var fy  = -originY;
        var fx2 = width - originX;
        var fy2 = height - originY;

        // scale
        if ( ( scaleX is not 1 ) || ( scaleY is not 1 ) )
        {
            fx  *= scaleX;
            fy  *= scaleY;
            fx2 *= scaleX;
            fy2 *= scaleY;
        }

        // construct corner points, start from top left and go counter clockwise
        var p1X = fx;
        var p1Y = fy;
        var p2X = fx;
        var p2Y = fy2;
        var p3X = fx2;
        var p3Y = fy2;
        var p4X = fx2;
        var p4Y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if ( rotation != 0 )
        {
            var cos = MathUtils.CosDeg( rotation );
            var sin = MathUtils.SinDeg( rotation );

            x1 = ( cos * p1X ) - ( sin * p1Y );
            y1 = ( sin * p1X ) + ( cos * p1Y );

            x2 = ( cos * p2X ) - ( sin * p2Y );
            y2 = ( sin * p2X ) + ( cos * p2Y );

            x3 = ( cos * p3X ) - ( sin * p3Y );
            y3 = ( sin * p3X ) + ( cos * p3Y );

            x4 = x1 + ( x3 - x2 );
            y4 = y3 - ( y2 - y1 );
        }
        else
        {
            x1 = p1X;
            y1 = p1Y;

            x2 = p2X;
            y2 = p2Y;

            x3 = p3X;
            y3 = p3Y;

            x4 = p4X;
            y4 = p4Y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        var invTexWidth  = 1.0f / texture.Width;
        var invTexHeight = 1.0f / texture.Height;

        var u  = srcX * invTexWidth;
        var v  = ( srcY + srcHeight ) * invTexHeight;
        var u2 = ( srcX + srcWidth ) * invTexWidth;
        var v2 = srcY * invTexHeight;

        if ( flipX ) ( u, u2 ) = ( u2, u );

        if ( flipY ) ( v, v2 ) = ( v2, v );

        tempVertices[ 0 ] = x1;
        tempVertices[ 1 ] = y1;
        tempVertices[ 2 ] = PackedColor;
        tempVertices[ 3 ] = u;
        tempVertices[ 4 ] = v;

        tempVertices[ 5 ] = x2;
        tempVertices[ 6 ] = y2;
        tempVertices[ 7 ] = PackedColor;
        tempVertices[ 8 ] = u;
        tempVertices[ 9 ] = v2;

        tempVertices[ 10 ] = x3;
        tempVertices[ 11 ] = y3;
        tempVertices[ 12 ] = PackedColor;
        tempVertices[ 13 ] = u2;
        tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            tempVertices[ 15 ] = x4;
            tempVertices[ 16 ] = y4;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v;

            Add( texture, tempVertices, 0, 20 );
        }
        else
        {
            tempVertices[ 15 ] = x3;
            tempVertices[ 16 ] = y3;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v2;

            tempVertices[ 20 ] = x4;
            tempVertices[ 21 ] = y4;
            tempVertices[ 22 ] = PackedColor;
            tempVertices[ 23 ] = u2;
            tempVertices[ 24 ] = v;

            tempVertices[ 25 ] = x1;
            tempVertices[ 26 ] = y1;
            tempVertices[ 27 ] = PackedColor;
            tempVertices[ 28 ] = u;
            tempVertices[ 29 ] = v;

            Add( texture, tempVertices, 0, 30 );
        }
    }

    /// <summary>
    /// Adds the specified region to the cache.
    /// </summary>
    public void Add( TextureRegion region, float x, float y )
    {
        Add( region, x, y, region.RegionWidth, region.RegionHeight );
    }

    /// <summary>
    /// Adds the specified region to the cache.
    /// </summary>
    public void Add( TextureRegion region, float x, float y, float width, float height )
    {
        var fx2 = x + width;
        var fy2 = y + height;
        var u   = region.U;
        var v   = region.V2;
        var u2  = region.U2;
        var v2  = region.V;

        tempVertices[ 0 ] = x;
        tempVertices[ 1 ] = y;
        tempVertices[ 2 ] = PackedColor;
        tempVertices[ 3 ] = u;
        tempVertices[ 4 ] = v;

        tempVertices[ 5 ] = x;
        tempVertices[ 6 ] = fy2;
        tempVertices[ 7 ] = PackedColor;
        tempVertices[ 8 ] = u;
        tempVertices[ 9 ] = v2;

        tempVertices[ 10 ] = fx2;
        tempVertices[ 11 ] = fy2;
        tempVertices[ 12 ] = PackedColor;
        tempVertices[ 13 ] = u2;
        tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = y;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v;

            Add( region.Texture, tempVertices, 0, 20 );
        }
        else
        {
            tempVertices[ 15 ] = fx2;
            tempVertices[ 16 ] = fy2;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v2;

            tempVertices[ 20 ] = fx2;
            tempVertices[ 21 ] = y;
            tempVertices[ 22 ] = PackedColor;
            tempVertices[ 23 ] = u2;
            tempVertices[ 24 ] = v;

            tempVertices[ 25 ] = x;
            tempVertices[ 26 ] = y;
            tempVertices[ 27 ] = PackedColor;
            tempVertices[ 28 ] = u;
            tempVertices[ 29 ] = v;

            Add( region.Texture, tempVertices, 0, 30 );
        }
    }

    /// <summary>
    /// Adds the specified region to the cache.
    /// </summary>
    public void Add( TextureRegion region,
                     float x,
                     float y,
                     float originX,
                     float originY,
                     float width,
                     float height,
                     float scaleX,
                     float scaleY,
                     float rotation )
    {

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( ( scaleX is not 1 ) || ( scaleY is not 1 ) )
        {
            fx  *= scaleX;
            fy  *= scaleY;
            fx2 *= scaleX;
            fy2 *= scaleY;
        }

        // construct corner points, start from top left and go counter clockwise
        var p1X = fx;
        var p1Y = fy;
        var p2X = fx;
        var p2Y = fy2;
        var p3X = fx2;
        var p3Y = fy2;
        var p4X = fx2;
        var p4Y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if ( rotation != 0 )
        {
            var cos = MathUtils.CosDeg( rotation );
            var sin = MathUtils.SinDeg( rotation );

            x1 = ( cos * p1X ) - ( sin * p1Y );
            y1 = ( sin * p1X ) + ( cos * p1Y );

            x2 = ( cos * p2X ) - ( sin * p2Y );
            y2 = ( sin * p2X ) + ( cos * p2Y );

            x3 = ( cos * p3X ) - ( sin * p3Y );
            y3 = ( sin * p3X ) + ( cos * p3Y );

            x4 = x1 + ( x3 - x2 );
            y4 = y3 - ( y2 - y1 );
        }
        else
        {
            x1 = p1X;
            y1 = p1Y;

            x2 = p2X;
            y2 = p2Y;

            x3 = p3X;
            y3 = p3Y;

            x4 = p4X;
            y4 = p4Y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        var u  = region.U;
        var v  = region.V2;
        var u2 = region.U2;
        var v2 = region.V;

        tempVertices[ 0 ] = x1;
        tempVertices[ 1 ] = y1;
        tempVertices[ 2 ] = PackedColor;
        tempVertices[ 3 ] = u;
        tempVertices[ 4 ] = v;

        tempVertices[ 5 ] = x2;
        tempVertices[ 6 ] = y2;
        tempVertices[ 7 ] = PackedColor;
        tempVertices[ 8 ] = u;
        tempVertices[ 9 ] = v2;

        tempVertices[ 10 ] = x3;
        tempVertices[ 11 ] = y3;
        tempVertices[ 12 ] = PackedColor;
        tempVertices[ 13 ] = u2;
        tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            tempVertices[ 15 ] = x4;
            tempVertices[ 16 ] = y4;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v;
            Add( region.Texture, tempVertices, 0, 20 );
        }
        else
        {
            tempVertices[ 15 ] = x3;
            tempVertices[ 16 ] = y3;
            tempVertices[ 17 ] = PackedColor;
            tempVertices[ 18 ] = u2;
            tempVertices[ 19 ] = v2;

            tempVertices[ 20 ] = x4;
            tempVertices[ 21 ] = y4;
            tempVertices[ 22 ] = PackedColor;
            tempVertices[ 23 ] = u2;
            tempVertices[ 24 ] = v;

            tempVertices[ 25 ] = x1;
            tempVertices[ 26 ] = y1;
            tempVertices[ 27 ] = PackedColor;
            tempVertices[ 28 ] = u;
            tempVertices[ 29 ] = v;

            Add( region.Texture, tempVertices, 0, 30 );
        }
    }

    /// <summary>
    /// Adds the specified region to the cache.
    /// </summary>
    public void Add( Sprite sprite )
    {
        if ( _mesh.NumIndices > 0 )
        {
            Add( sprite.Texture, sprite.Vertices, 0, Sprite.SpriteSize );

            return;
        }

        Array.Copy( sprite.Vertices, 0, tempVertices, 0, 3 * Sprite.VertexSize ); // temp0,1,2=sprite0,1,2

        Array.Copy
            (
             sprite.Vertices,
             2 * Sprite.VertexSize,
             tempVertices,
             3 * Sprite.VertexSize,
             Sprite.VertexSize
            ); // temp3=sprite2

        Array.Copy
            (
             sprite.Vertices,
             3 * Sprite.VertexSize,
             tempVertices,
             4 * Sprite.VertexSize,
             Sprite.VertexSize
            ); // temp4=sprite3

        Array.Copy( sprite.Vertices, 0, tempVertices, 5 * Sprite.VertexSize, Sprite.VertexSize ); // temp5=sprite0

        Add( sprite.Texture, tempVertices, 0, 30 );
    }

    /// <summary>
    /// Prepares the OpenGL state for SpriteCache rendering.
    /// </summary>
    public void Begin()
    {
        if ( _drawing )
            throw new IllegalStateException( "end must be called before begin." );

        if ( _currentCache != null )
            throw new IllegalStateException( "endCache must be called before begin" );

        renderCalls = 0;
        _combinedMatrix.Set( ProjectionMatrix ).Mul( TransformMatrix );

        Gdx.GL20.GLDepthMask( false );

        if ( CustomShader != null )
        {
            CustomShader.Bind();
            CustomShader.SetUniformMatrix( "u_proj", ProjectionMatrix );
            CustomShader.SetUniformMatrix( "u_trans", TransformMatrix );
            CustomShader.SetUniformMatrix( "u_projTrans", _combinedMatrix );
            CustomShader.SetUniformi( "u_texture", 0 );

            _mesh.Bind( CustomShader );
        }
        else
        {
            if ( _shader != null )
            {
                _shader.Bind();
                _shader.SetUniformMatrix( "u_projectionViewMatrix", _combinedMatrix );
                _shader.SetUniformi( "u_texture", 0 );
                _mesh.Bind( _shader );
            }
        }

        _drawing = true;
    }

    /// <summary>
    /// Completes rendering for this SpriteCache.
    /// </summary>
    public void End()
    {
        if ( !_drawing )
            throw new IllegalStateException( "begin must be called before end." );

        _drawing = false;

        Gdx.GL20.GLDepthMask( true );

        _mesh.Unbind( CustomShader ?? _shader );
    }

    /// <summary>
    /// Draws all the images defined for the specified cache ID.
    /// </summary>
    public void Draw( int cacheID )
    {
        if ( !_drawing )
            throw new IllegalStateException( "SpriteCache.begin must be called before draw." );

        Cache cache = _caches[ cacheID ];

        var verticesPerImage = _mesh.NumIndices > 0 ? 4 : 6;
        var offset           = ( cache.offset / ( verticesPerImage * Sprite.VertexSize ) ) * 6;
        var counts           = cache.counts;
        var textureCount     = cache.textureCount;

        Texture[]? textures = cache.textures;

        for ( var i = 0; i < textureCount; i++ )
        {
            textures?[ i ].Bind();

            _mesh.Render( CustomShader ?? _shader, IGL20.GL_Triangles, offset, counts![ i ] );

            offset += counts[ i ];
        }

        renderCalls      += textureCount;
        totalRenderCalls += textureCount;
    }

    /// <summary>
    /// Draws a subset of images defined for the specified cache ID.
    /// </summary>
    /// <param name="cacheID"></param>
    /// <param name="offset"> The first image to render. </param>
    /// <param name="length">
    /// The number of images from the first image (inclusive) to render.
    /// </param>
    public void Draw( int cacheID, int offset, int length )
    {
        if ( !_drawing )
            throw new IllegalStateException( "SpriteCache.begin must be called before draw." );

        Cache cache = _caches[ cacheID ];

        var verticesPerImage = _mesh.NumIndices > 0 ? 4 : 6;

        offset =  ( ( cache.offset / ( verticesPerImage * Sprite.VertexSize ) ) * 6 ) + ( offset * 6 );
        length *= 6;
        Texture[]? textures = cache.textures;

        var counts       = cache.counts;
        var textureCount = cache.textureCount;

        for ( var i = 0; i < textureCount; i++ )
        {
            textures?[ i ].Bind();
            var count = counts![ i ];

            if ( count > length )
            {
                i     = textureCount;
                count = length;
            }
            else
            {
                length -= count;
            }

            _mesh.Render( CustomShader ?? _shader, IGL20.GL_Triangles, offset, count );

            offset += count;
        }

        renderCalls      += cache.textureCount;
        totalRenderCalls += textureCount;
    }

    public Matrix4 ProjectionMatrix { get; init; } = new();

    public Matrix4 TransformMatrix { get; init; } = new();

    private static ShaderProgram CreateDefaultShader()
    {
        var vertexShader =
            "attribute vec4 "
            + ShaderProgram.PositionAttribute
            + ";\n" //
            + "attribute vec4 "
            + ShaderProgram.ColorAttribute
            + ";\n" //
            + "attribute vec2 "
            + ShaderProgram.TexcoordAttribute
            + "0;\n"                                   //
            + "uniform mat4 u_projectionViewMatrix;\n" //
            + "varying vec4 v_color;\n"                //
            + "varying vec2 v_texCoords;\n"            //
            + "\n"                                     //
            + "void main()\n"                          //
            + "{\n"                                    //
            + "   v_color = "
            + ShaderProgram.ColorAttribute
            + ";\n"                                         //
            + "   v_color.a = v_color.a * (255.0/254.0);\n" //
            + "   v_texCoords = "
            + ShaderProgram.TexcoordAttribute
            + "0;\n" //
            + "   gl_Position =  u_projectionViewMatrix * "
            + ShaderProgram.PositionAttribute
            + ";\n" //
            + "}\n";

        var fragmentShader =
            "#ifdef GL_ES\n"                                                    //
            + "precision mediump float;\n"                                      //
            + "#endif\n"                                                        //
            + "varying vec4 v_color;\n"                                         //
            + "varying vec2 v_texCoords;\n"                                     //
            + "uniform sampler2D u_texture;\n"                                  //
            + "void main()\n"                                                   //
            + "{\n"                                                             //
            + "  gl_FragColor = v_color * texture2D(u_texture, v_texCoords);\n" //
            + "}";

        var shader = new ShaderProgram( vertexShader, fragmentShader );

        if ( !shader.IsCompiled )
            throw new ArgumentException( "Error compiling shader: " + shader.Log );

        return shader;
    }

    /// <summary>
    /// Sets the shader to be used in a GLES 2.0 environment. Vertex position
    /// attribute is called "a_position", the texture coordinates attribute is
    /// called called "a_texCoords", the color attribute is called "a_color".
    /// The projection matrix is uploaded via a mat4 uniform called "u_proj",
    /// the transform matrix is uploaded via a uniform called "u_trans", the combined
    /// transform and projection matrx is is uploaded via a mat4 uniform called
    /// "u_projTrans". The texture sampler is passed via a uniform called "u_texture".
    /// Call this method with a null argument to use the default shader.
    /// </summary>
    public ShaderProgram? CustomShader { get; set; }

    public bool IsDrawing()
    {
        return _drawing;
    }

    /// <summary>
    /// Releases all resources held by this SpriteCache.
    /// </summary>
    public void Dispose()
    {
        _mesh.Dispose();
        _shader?.Dispose();
    }

    private sealed class Cache
    {
        internal readonly int id;
        internal readonly int offset;

        internal int        maxCount;
        internal int        textureCount;
        internal Texture[]? textures;
        internal int[]?     counts;

        internal Cache( int id, int offset )
        {
            this.id     = id;
            this.offset = offset;
        }
    }
}