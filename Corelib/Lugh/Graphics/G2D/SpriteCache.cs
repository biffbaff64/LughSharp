// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using Corelib.Lugh.Graphics.GLUtils;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils.Collections;
using Corelib.Lugh.Utils.Exceptions;

using Matrix4 = Corelib.Lugh.Maths.Matrix4;

namespace Corelib.Lugh.Graphics.G2D;

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
/// can be changed. If the screen is <see cref="IApplicationListener.Resize"/>,
/// the SpriteCache's matrices must be updated. For example:
/// </para>
/// <code>
/// cache.GetProjectionMatrix().SetToOrtho2D(0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height);
/// </code>
/// <para>
/// Note that SpriteCache does not manage blending. You will need to enable blending
/// (<tt>Gdx.GL.GLEnable(IGL.GL_Blend);</tt>) and set the blend func as needed before
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
[PublicAPI]
public class SpriteCache
{
    // ========================================================================
    // ========================================================================

    private static readonly float[] _tempVertices = new float[ Sprite.VERTEX_SIZE * 6 ];

    private readonly List< Cache >   _caches         = new();
    private readonly Matrix4         _combinedMatrix = new();
    private readonly List< int >     _counts         = new( 8 );
    private readonly Mesh            _mesh;
    private readonly ShaderProgram?  _shader;
    private readonly List< Texture > _textures = new( 8 );

    private float  _colorPacked = Color.WhiteFloatBits;
    private Cache? _currentCache;

    // ========================================================================
    // ========================================================================

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
        _shader = shader;

        if ( useIndices && ( size > 8191 ) )
        {
            throw new ArgumentException( $"Can't have more than 8191 sprites per batch: {size}" );
        }

        _mesh = new Mesh( true,
                          size * ( useIndices ? 4 : 6 ),
                          useIndices ? size * 6 : 0,
                          new VertexAttribute( VertexAttributes.Usage.POSITION,
                                               2,
                                               ShaderProgram.POSITION_ATTRIBUTE ),
                          new VertexAttribute( VertexAttributes.Usage.COLOR_PACKED,
                                               4,
                                               ShaderProgram.COLOR_ATTRIBUTE ),
                          new VertexAttribute( VertexAttributes.Usage.TEXTURE_COORDINATES,
                                               2,
                                               ShaderProgram.TEXCOORD_ATTRIBUTE + "0" ) )
        {
            AutoBind = false,
        };

        if ( useIndices )
        {
            var   length  = size * 6;
            var   indices = new short[ length ];
            short j       = 0;

            for ( var i = 0; i < length; i += 6, j += 4 )
            {
                indices[ i + 0 ] = j;
                indices[ i + 1 ] = ( short ) ( j + 1 );
                indices[ i + 2 ] = ( short ) ( j + 2 );
                indices[ i + 3 ] = ( short ) ( j + 2 );
                indices[ i + 4 ] = ( short ) ( j + 3 );
                indices[ i + 5 ] = j;
            }

            _mesh.SetIndices( indices );
        }

        ProjectionMatrix.SetToOrtho2D( 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height );
    }

    // ========================================================================
    // ========================================================================

    public int     RenderCallsSinceBegin { get; set; } = 0;
    public int     TotalRenderCalls      { get; set; } = 0;
    public Color   Color                 { get; set; } = new( 1, 1, 1, 1 );
    public Matrix4 ProjectionMatrix      { get; set; } = new();
    public Matrix4 TransformMatrix       { get; set; } = new();
    public bool    IsDrawing             { get; private set; }

    /// <summary>
    /// The color of this sprite cache, expanding the alpha from 0-254 to 0-255.
    /// </summary>
    public float PackedColor
    {
        get => _colorPacked;
        set
        {
            var color = Color;
            Color.ABGR8888ToColor( ref color, value );

            _colorPacked = value;

            Color = color;
        }
    }

    /// Sets the color used to tint images when they are added to the
    /// SpriteCache. Default is
    /// <see cref="Graphics.Color.White"/>
    /// .
    public void SetColor( Color tint )
    {
        Color.Set( tint );
        PackedColor = tint.ToFloatBitsABGR();
    }

    /// <summary>
    /// </summary>
    /// <see cref="SetColor(Corelib.Lugh.Graphics.Color)"/>
    public void SetColor( float r, float g, float b, float a )
    {
        Color.Set( r, g, b, a );
        PackedColor = Color.ToFloatBitsABGR();
    }

    /// <summary>
    /// Starts the definition of a new cache, allowing the add and
    /// <see cref="EndCache()"/> methods to be called.
    /// </summary>
    public void BeginCache()
    {
        if ( IsDrawing )
        {
            throw new GdxRuntimeException( "end must be called before beginCache" );
        }

        if ( _currentCache != null )
        {
            throw new GdxRuntimeException( "endCache must be called before begin." );
        }

        _currentCache = new Cache( _caches.Count, _mesh.GetVerticesBuffer().Limit );
        _caches.Add( _currentCache );
        _mesh.GetVerticesBuffer().Compact();
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
        if ( IsDrawing )
        {
            throw new GdxRuntimeException( "end must be called before beginCache" );
        }

        if ( _currentCache != null )
        {
            throw new GdxRuntimeException( "endCache must be called before begin." );
        }

        if ( cacheID == ( _caches.Count - 1 ) )
        {
            var oldCache = _caches.RemoveIndex( cacheID );
            _mesh.GetVerticesBuffer().Limit = oldCache.Offset;
            BeginCache();

            return;
        }

        _currentCache                      = _caches[ cacheID ];
        _mesh.GetVerticesBuffer().Position = _currentCache.Offset;
    }

    /// <summary>
    /// Ends the definition of a cache, returning the cache ID to be
    /// used with <see cref="Draw(int)"/>.
    /// </summary>
    public int EndCache()
    {
        if ( _currentCache == null )
        {
            throw new GdxRuntimeException( "beginCache must be called before endCache." );
        }

        var cache      = _currentCache;
        var cacheCount = _mesh.GetVerticesBuffer().Position - cache.Offset;

        if ( cache.Textures == null )
        {
            // New cache.
            cache.MaxCount     = cacheCount;
            cache.TextureCount = _textures.Count;
            cache.Textures     = _textures.ToArray();
            cache.Counts       = new int[ cache.TextureCount ];

            for ( int i = 0, n = _counts.Count; i < n; i++ )
            {
                cache.Counts[ i ] = _counts[ i ];
            }

            _mesh.GetVerticesBuffer().Flip();
        }
        else
        {
            // Redefine existing cache.
            if ( cacheCount > cache.MaxCount )
            {
                throw new GdxRuntimeException( $"If a cache is not the last created, it cannot be redefined"
                                             + $"with more entries than when it was first created: "
                                             + $"{cacheCount} ({cache.MaxCount} max)" );
            }

            cache.TextureCount = _textures.Count;

            if ( cache.Textures.Length < cache.TextureCount )
            {
                cache.Textures = new Texture[ cache.TextureCount ];
            }

            for ( int i = 0, n = cache.TextureCount; i < n; i++ )
            {
                cache.Textures[ i ] = _textures[ i ];
            }

            if ( cache.Counts?.Length < cache.TextureCount )
            {
                cache.Counts = new int[ cache.TextureCount ];
            }

            if ( cache.Counts != null )
            {
                for ( int i = 0, n = cache.TextureCount; i < n; i++ )
                {
                    cache.Counts[ i ] = _counts[ i ];
                }
            }

            var vertices = _mesh.GetVerticesBuffer();
            vertices.Position = 0;
            var lastCache = _caches[ _caches.Count - 1 ];
            vertices.Limit = lastCache.Offset + lastCache.MaxCount;
        }

        _currentCache = null!;
        _textures.Clear();
        _counts.Clear();

        return cache.ID;
    }

    /// <summary>
    /// Invalidates all cache IDs and resets the SpriteCache so new caches can be added.
    /// </summary>
    public void Clear()
    {
        _caches.Clear();
        _mesh.GetVerticesBuffer().Clear().Flip();
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
        {
            throw new GdxRuntimeException( "beginCache must be called before add." );
        }

        var verticesPerImage = _mesh.NumIndices > 0 ? 4 : 6;
        var count            = ( length / ( verticesPerImage * Sprite.VERTEX_SIZE ) ) * 6;
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

        _mesh.GetVerticesBuffer().Put( vertices, offset, length );
    }

    /// <summary>
    /// Adds the specified texture to the cache.
    /// </summary>
    public void Add( Texture texture, float x, float y )
    {
        var fx2 = x + texture.Width;
        var fy2 = y + texture.Height;

        _tempVertices[ 0 ] = x;
        _tempVertices[ 1 ] = y;
        _tempVertices[ 2 ] = PackedColor;
        _tempVertices[ 3 ] = 0;
        _tempVertices[ 4 ] = 1;

        _tempVertices[ 5 ] = x;
        _tempVertices[ 6 ] = fy2;
        _tempVertices[ 7 ] = PackedColor;
        _tempVertices[ 8 ] = 0;
        _tempVertices[ 9 ] = 0;

        _tempVertices[ 10 ] = fx2;
        _tempVertices[ 11 ] = fy2;
        _tempVertices[ 12 ] = PackedColor;
        _tempVertices[ 13 ] = 1;
        _tempVertices[ 14 ] = 0;

        if ( _mesh.NumIndices > 0 )
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = y;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = 1;
            _tempVertices[ 19 ] = 1;

            Add( texture, _tempVertices, 0, 20 );
        }
        else
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = fy2;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = 1;
            _tempVertices[ 19 ] = 0;

            _tempVertices[ 20 ] = fx2;
            _tempVertices[ 21 ] = y;
            _tempVertices[ 22 ] = PackedColor;
            _tempVertices[ 23 ] = 1;
            _tempVertices[ 24 ] = 1;

            _tempVertices[ 25 ] = x;
            _tempVertices[ 26 ] = y;
            _tempVertices[ 27 ] = PackedColor;
            _tempVertices[ 28 ] = 0;
            _tempVertices[ 29 ] = 1;

            Add( texture, _tempVertices, 0, 30 );
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

        _tempVertices[ 0 ] = x;
        _tempVertices[ 1 ] = y;
        _tempVertices[ 2 ] = color;
        _tempVertices[ 3 ] = u;
        _tempVertices[ 4 ] = v;

        _tempVertices[ 5 ] = x;
        _tempVertices[ 6 ] = fy2;
        _tempVertices[ 7 ] = color;
        _tempVertices[ 8 ] = u;
        _tempVertices[ 9 ] = v2;

        _tempVertices[ 10 ] = fx2;
        _tempVertices[ 11 ] = fy2;
        _tempVertices[ 12 ] = color;
        _tempVertices[ 13 ] = u2;
        _tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = y;
            _tempVertices[ 17 ] = color;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v;

            Add( texture, _tempVertices, 0, 20 );
        }
        else
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = fy2;
            _tempVertices[ 17 ] = color;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v2;

            _tempVertices[ 20 ] = fx2;
            _tempVertices[ 21 ] = y;
            _tempVertices[ 22 ] = color;
            _tempVertices[ 23 ] = u2;
            _tempVertices[ 24 ] = v;

            _tempVertices[ 25 ] = x;
            _tempVertices[ 26 ] = y;
            _tempVertices[ 27 ] = color;
            _tempVertices[ 28 ] = u;
            _tempVertices[ 29 ] = v;

            Add( texture, _tempVertices, 0, 30 );
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

        _tempVertices[ 0 ] = x;
        _tempVertices[ 1 ] = y;
        _tempVertices[ 2 ] = PackedColor;
        _tempVertices[ 3 ] = u;
        _tempVertices[ 4 ] = v;

        _tempVertices[ 5 ] = x;
        _tempVertices[ 6 ] = fy2;
        _tempVertices[ 7 ] = PackedColor;
        _tempVertices[ 8 ] = u;
        _tempVertices[ 9 ] = v2;

        _tempVertices[ 10 ] = fx2;
        _tempVertices[ 11 ] = fy2;
        _tempVertices[ 12 ] = PackedColor;
        _tempVertices[ 13 ] = u2;
        _tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = y;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v;

            Add( texture, _tempVertices, 0, 20 );
        }
        else
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = fy2;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v2;

            _tempVertices[ 20 ] = fx2;
            _tempVertices[ 21 ] = y;
            _tempVertices[ 22 ] = PackedColor;
            _tempVertices[ 23 ] = u2;
            _tempVertices[ 24 ] = v;

            _tempVertices[ 25 ] = x;
            _tempVertices[ 26 ] = y;
            _tempVertices[ 27 ] = PackedColor;
            _tempVertices[ 28 ] = u;
            _tempVertices[ 29 ] = v;

            Add( texture, _tempVertices, 0, 30 );
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

        _tempVertices[ 0 ] = x;
        _tempVertices[ 1 ] = y;
        _tempVertices[ 2 ] = PackedColor;
        _tempVertices[ 3 ] = u;
        _tempVertices[ 4 ] = v;

        _tempVertices[ 5 ] = x;
        _tempVertices[ 6 ] = fy2;
        _tempVertices[ 7 ] = PackedColor;
        _tempVertices[ 8 ] = u;
        _tempVertices[ 9 ] = v2;

        _tempVertices[ 10 ] = fx2;
        _tempVertices[ 11 ] = fy2;
        _tempVertices[ 12 ] = PackedColor;
        _tempVertices[ 13 ] = u2;
        _tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = y;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v;

            Add( texture, _tempVertices, 0, 20 );
        }
        else
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = fy2;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v2;

            _tempVertices[ 20 ] = fx2;
            _tempVertices[ 21 ] = y;
            _tempVertices[ 22 ] = PackedColor;
            _tempVertices[ 23 ] = u2;
            _tempVertices[ 24 ] = v;

            _tempVertices[ 25 ] = x;
            _tempVertices[ 26 ] = y;
            _tempVertices[ 27 ] = PackedColor;
            _tempVertices[ 28 ] = u;
            _tempVertices[ 29 ] = v;

            Add( texture, _tempVertices, 0, 30 );
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
        if ( scaleX is not 1 || scaleY is not 1 )
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

        if ( flipX )
        {
            ( u, u2 ) = ( u2, u );
        }

        if ( flipY )
        {
            ( v, v2 ) = ( v2, v );
        }

        _tempVertices[ 0 ] = x1;
        _tempVertices[ 1 ] = y1;
        _tempVertices[ 2 ] = PackedColor;
        _tempVertices[ 3 ] = u;
        _tempVertices[ 4 ] = v;

        _tempVertices[ 5 ] = x2;
        _tempVertices[ 6 ] = y2;
        _tempVertices[ 7 ] = PackedColor;
        _tempVertices[ 8 ] = u;
        _tempVertices[ 9 ] = v2;

        _tempVertices[ 10 ] = x3;
        _tempVertices[ 11 ] = y3;
        _tempVertices[ 12 ] = PackedColor;
        _tempVertices[ 13 ] = u2;
        _tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            _tempVertices[ 15 ] = x4;
            _tempVertices[ 16 ] = y4;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v;

            Add( texture, _tempVertices, 0, 20 );
        }
        else
        {
            _tempVertices[ 15 ] = x3;
            _tempVertices[ 16 ] = y3;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v2;

            _tempVertices[ 20 ] = x4;
            _tempVertices[ 21 ] = y4;
            _tempVertices[ 22 ] = PackedColor;
            _tempVertices[ 23 ] = u2;
            _tempVertices[ 24 ] = v;

            _tempVertices[ 25 ] = x1;
            _tempVertices[ 26 ] = y1;
            _tempVertices[ 27 ] = PackedColor;
            _tempVertices[ 28 ] = u;
            _tempVertices[ 29 ] = v;

            Add( texture, _tempVertices, 0, 30 );
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

        _tempVertices[ 0 ] = x;
        _tempVertices[ 1 ] = y;
        _tempVertices[ 2 ] = PackedColor;
        _tempVertices[ 3 ] = u;
        _tempVertices[ 4 ] = v;

        _tempVertices[ 5 ] = x;
        _tempVertices[ 6 ] = fy2;
        _tempVertices[ 7 ] = PackedColor;
        _tempVertices[ 8 ] = u;
        _tempVertices[ 9 ] = v2;

        _tempVertices[ 10 ] = fx2;
        _tempVertices[ 11 ] = fy2;
        _tempVertices[ 12 ] = PackedColor;
        _tempVertices[ 13 ] = u2;
        _tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = y;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v;

            Add( region.Texture, _tempVertices, 0, 20 );
        }
        else
        {
            _tempVertices[ 15 ] = fx2;
            _tempVertices[ 16 ] = fy2;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v2;

            _tempVertices[ 20 ] = fx2;
            _tempVertices[ 21 ] = y;
            _tempVertices[ 22 ] = PackedColor;
            _tempVertices[ 23 ] = u2;
            _tempVertices[ 24 ] = v;

            _tempVertices[ 25 ] = x;
            _tempVertices[ 26 ] = y;
            _tempVertices[ 27 ] = PackedColor;
            _tempVertices[ 28 ] = u;
            _tempVertices[ 29 ] = v;

            Add( region.Texture, _tempVertices, 0, 30 );
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
        if ( scaleX is not 1 || scaleY is not 1 )
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

        _tempVertices[ 0 ] = x1;
        _tempVertices[ 1 ] = y1;
        _tempVertices[ 2 ] = PackedColor;
        _tempVertices[ 3 ] = u;
        _tempVertices[ 4 ] = v;

        _tempVertices[ 5 ] = x2;
        _tempVertices[ 6 ] = y2;
        _tempVertices[ 7 ] = PackedColor;
        _tempVertices[ 8 ] = u;
        _tempVertices[ 9 ] = v2;

        _tempVertices[ 10 ] = x3;
        _tempVertices[ 11 ] = y3;
        _tempVertices[ 12 ] = PackedColor;
        _tempVertices[ 13 ] = u2;
        _tempVertices[ 14 ] = v2;

        if ( _mesh.NumIndices > 0 )
        {
            _tempVertices[ 15 ] = x4;
            _tempVertices[ 16 ] = y4;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v;

            Add( region.Texture, _tempVertices, 0, 20 );
        }
        else
        {
            _tempVertices[ 15 ] = x3;
            _tempVertices[ 16 ] = y3;
            _tempVertices[ 17 ] = PackedColor;
            _tempVertices[ 18 ] = u2;
            _tempVertices[ 19 ] = v2;

            _tempVertices[ 20 ] = x4;
            _tempVertices[ 21 ] = y4;
            _tempVertices[ 22 ] = PackedColor;
            _tempVertices[ 23 ] = u2;
            _tempVertices[ 24 ] = v;

            _tempVertices[ 25 ] = x1;
            _tempVertices[ 26 ] = y1;
            _tempVertices[ 27 ] = PackedColor;
            _tempVertices[ 28 ] = u;
            _tempVertices[ 29 ] = v;

            Add( region.Texture, _tempVertices, 0, 30 );
        }
    }

    /// <summary>
    /// Adds the specified region to the cache.
    /// </summary>
    public void Add( Sprite sprite )
    {
        if ( _mesh.NumIndices > 0 )
        {
            Add( sprite.Texture, sprite.Vertices, 0, Sprite.SPRITE_SIZE );

            return;
        }

        Array.Copy( sprite.Vertices, 0, _tempVertices, 0, 3 * Sprite.VERTEX_SIZE ); // temp0,1,2=sprite0,1,2

        Array.Copy( sprite.Vertices,
                    2 * Sprite.VERTEX_SIZE,
                    _tempVertices,
                    3 * Sprite.VERTEX_SIZE,
                    Sprite.VERTEX_SIZE ); // temp3=sprite2

        Array.Copy( sprite.Vertices,
                    3 * Sprite.VERTEX_SIZE,
                    _tempVertices,
                    4 * Sprite.VERTEX_SIZE,
                    Sprite.VERTEX_SIZE ); // temp4=sprite3

        Array.Copy( sprite.Vertices, 0, _tempVertices, 5 * Sprite.VERTEX_SIZE, Sprite.VERTEX_SIZE ); // temp5=sprite0

        Add( sprite.Texture, _tempVertices, 0, 30 );
    }

    /// <summary>
    /// Prepares the OpenGL state for SpriteCache rendering.
    /// </summary>
    public void Begin()
    {
        if ( IsDrawing )
        {
            throw new GdxRuntimeException( "end must be called before begin." );
        }

        if ( _currentCache != null )
        {
            throw new GdxRuntimeException( "endCache must be called before begin" );
        }

        RenderCallsSinceBegin = 0;
        _combinedMatrix.Set( ProjectionMatrix ).Mul( TransformMatrix );

        Gdx.GL.glDepthMask( false );

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

        IsDrawing = true;
    }

    /// <summary>
    /// Completes rendering for this SpriteCache.
    /// </summary>
    public void End()
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "begin must be called before end." );
        }

        IsDrawing = false;

        Gdx.GL.glDepthMask( true );

        _mesh.Unbind( CustomShader ?? _shader );
    }

    /// <summary>
    /// Draws all the images defined for the specified cache ID.
    /// </summary>
    public void Draw( int cacheID )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "SpriteCache.begin must be called before draw." );
        }

        var cache = _caches[ cacheID ];

        var verticesPerImage = _mesh.NumIndices > 0 ? 4 : 6;
        var offset           = ( cache.Offset / ( verticesPerImage * Sprite.VERTEX_SIZE ) ) * 6;
        var counts           = cache.Counts;
        var textureCount     = cache.TextureCount;

        Texture[]? textures = cache.Textures;

        for ( var i = 0; i < textureCount; i++ )
        {
            textures?[ i ].Bind();

            _mesh.Render( CustomShader ?? _shader, IGL.GL_TRIANGLES, offset, counts![ i ] );

            offset += counts[ i ];
        }

        RenderCallsSinceBegin += textureCount;
        TotalRenderCalls      += textureCount;
    }

    /// <summary>
    /// Draws a subset of images defined for the specified cache ID.
    /// </summary>
    /// <param name="cacheID"></param>
    /// <param name="offset"> The first image to render. </param>
    /// <param name="length"> The number of images from the first image (inclusive) to render. </param>
    public void Draw( int cacheID, int offset, int length )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "SpriteCache.begin must be called before draw." );
        }

        var cache = _caches[ cacheID ];

        var verticesPerImage = _mesh.NumIndices > 0 ? 4 : 6;

        offset =  ( ( cache.Offset / ( verticesPerImage * Sprite.VERTEX_SIZE ) ) * 6 ) + ( offset * 6 );
        length *= 6;

        Texture[]? textures = cache.Textures;

        var counts       = cache.Counts;
        var textureCount = cache.TextureCount;

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

            _mesh.Render( CustomShader ?? _shader, IGL.GL_TRIANGLES, offset, count );

            offset += count;
        }

        RenderCallsSinceBegin += cache.TextureCount;
        TotalRenderCalls      += textureCount;
    }

    /// <summary>
    /// Releases all resources held by this SpriteCache.
    /// </summary>
    public void Dispose()
    {
        _mesh.Dispose();
        _shader?.Dispose();
    }

    // ========================================================================
    // ========================================================================

    private sealed class Cache
    {
        internal readonly int    ID;
        internal readonly int    Offset;
        internal          int[]? Counts;

        internal int        MaxCount;
        internal int        TextureCount;
        internal Texture[]? Textures;

        internal Cache( int id, int offset )
        {
            ID     = id;
            Offset = offset;
        }
    }

    // ========================================================================
    // ========================================================================

    #region shaders

    private static ShaderProgram CreateDefaultShader()
    {
        const string VERTEX_SHADER = "in vec4 "
                                   + ShaderProgram.POSITION_ATTRIBUTE
                                   + ";\n" //
                                   + "in vec4 "
                                   + ShaderProgram.COLOR_ATTRIBUTE
                                   + ";\n" //
                                   + "in vec2 "
                                   + ShaderProgram.TEXCOORD_ATTRIBUTE
                                   + "0;\n"                                   //
                                   + "uniform mat4 u_projectionViewMatrix;\n" //
                                   + "out vec4 v_color;\n"                //
                                   + "out vec2 v_texCoords;\n"            //
                                   + "\n"                                     //
                                   + "void main()\n"                          //
                                   + "{\n"                                    //
                                   + "   v_color = "
                                   + ShaderProgram.COLOR_ATTRIBUTE
                                   + ";\n"                                         //
                                   + "   v_color.a = v_color.a * (255.0/254.0);\n" //
                                   + "   v_texCoords = "
                                   + ShaderProgram.TEXCOORD_ATTRIBUTE
                                   + "0;\n" //
                                   + "   gl_Position =  u_projectionViewMatrix * "
                                   + ShaderProgram.POSITION_ATTRIBUTE
                                   + ";\n" //
                                   + "}\n";

        const string FRAGMENT_SHADER = "#ifdef GL_ES\n"
                                     + "#define LOWP lowp\n"
                                     + "precision mediump float;\n"
                                     + "#endif\n"
                                     + "in vec4 v_color;\n"
                                     + "in vec2 v_texCoords;\n"
                                     + "uniform sampler2D u_texture;\n"
                                     + "void main()\n"
                                     + "{\n"                                                             //
                                     + "  vec4 fragColor = v_color * texture(u_texture, v_texCoords);\n" //
                                     + "}";

        var shader = new ShaderProgram( VERTEX_SHADER, FRAGMENT_SHADER );

        if ( !shader.IsCompiled )
        {
            throw new ArgumentException( "Error compiling shader: " + shader.ShaderLog );
        }

        return shader;
    }

    //TODO: Update this documentation, this is GL not GLES
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

    #endregion shaders
}
