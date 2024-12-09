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
using Corelib.Lugh.Utils.Exceptions;

using Matrix4 = Corelib.Lugh.Maths.Matrix4;

namespace Corelib.Lugh.Graphics.G2D;

/// <summary>
/// A PolygonSpriteBatch is used to Draw 2D polygons that reference a
/// texture (region). The class will batch the drawing commands and
/// optimize them for processing by the GPU.
/// <para>
/// To Draw something with a PolygonSpriteBatch one has to first call the
/// <see cref="PolygonSpriteBatch.Begin()"/> method which will setup appropriate
/// render states. When you are done with drawing you have to call
/// <see cref="PolygonSpriteBatch.End()"/> which will actually Draw the things
/// you specified.
/// </para>
/// <para>
/// All drawing commands of the PolygonSpriteBatch operate in screen coordinates.
/// The screen coordinate system has an x-axis pointing to the right, an y-axis
/// pointing upwards and the origin is in the lower left corner of the screen. You
/// can also provide your own transformation and projection matrices if you so wish.
/// A PolygonSpriteBatch is managed. In case the OpenGL context is lost all OpenGL
/// resources a PolygonSpriteBatch uses internally get invalidated. A context is lost
/// when a user switches to another application or receives an incoming call on Android.
/// A SpritPolygonSpriteBatcheBatch will be automatically reloaded after the OpenGL
/// context is restored.
/// </para>
/// <para>
/// A PolygonSpriteBatch is a pretty heavy object so you should only ever have one
/// in your program.
/// </para>
/// <para>
/// A PolygonSpriteBatch works with OpenGL ES 1.x and 2.0. In the case of a 2.0 context
/// it will use its own custom shader to Draw all provided sprites. You can set your own
/// custom shader via <see cref="Shader"/>.
/// </para>
/// <para>
/// A PolygonSpriteBatch has to be disposed if it is no longer used.
/// </para>
/// </summary>
[PublicAPI]
public class PolygonSpriteBatch : IPolygonBatch
{
    public int     BlendSrcFunc      { get; private set; } = IGL.GL_SRC_ALPHA;
    public int     BlendDstFunc      { get; private set; } = IGL.GL_ONE_MINUS_SRC_ALPHA;
    public int     BlendSrcFuncAlpha { get; private set; } = IGL.GL_SRC_ALPHA;
    public int     BlendDstFuncAlpha { get; private set; } = IGL.GL_ONE_MINUS_SRC_ALPHA;
    public Matrix4 ProjectionMatrix  { get; set; }         = new();
    public Matrix4 TransformMatrix   { get; set; }         = new();
    public bool    IsDrawing         { get; set; }

    // The maximum number of triangles rendered in one batch so far.
    public int MaxTrianglesInBatch { get; set; } = 0;

    // Number of render calls since the last call to Begin()
    public int RenderCalls { get; set; } = 0;

    // Number of rendering calls, ever. Will not be reset unless set manually.
    public int TotalRenderCalls { get; set; } = 0;

    // ========================================================================
    // ========================================================================

    private readonly Color          _color          = new( 1, 1, 1, 1 );
    private readonly Matrix4        _combinedMatrix = new();
    private readonly Mesh           _mesh;
    private readonly bool           _ownsShader;
    private readonly ShaderProgram? _shader;
    private readonly short[]        _triangles;
    private readonly float[]        _vertices;

    private bool           _blendingDisabled;
    private float          _colorPacked = Color.WhiteFloatBits;
    private ShaderProgram? _customShader;
    private float          _invTexHeight = 0;
    private float          _invTexWidth  = 0;
    private Texture?       _lastTexture;
    private int            _triangleIndex;
    private int            _vertexIndex;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Constructs a PolygonSpriteBatch with the default shader, size vertices,
    /// and size * 2 triangles.
    /// </summary>
    /// <param name="size">
    /// The max number of vertices and number of triangles in a single batch. Max of 32767.
    /// </param>
    public PolygonSpriteBatch( int size )
        : this( size, size * 2, null )
    {
    }

    /// <summary>
    /// Constructs a PolygonSpriteBatch with the specified shader, size vertices
    /// and size * 2 triangles.
    /// </summary>
    /// <param name="size">
    /// The max number of vertices and number of triangles in a single batch. Max of 32767.
    /// </param>
    /// <param name="defaultShader"></param>
    public PolygonSpriteBatch( int size, ShaderProgram? defaultShader = null )
        : this( size, size * 2, defaultShader )
    {
    }

    /// <summary>
    /// Constructs a new PolygonSpriteBatch. Sets the projection matrix to an orthographic
    /// projection with y-axis point upwards, x-axis point to the right and the origin
    /// being in the bottom left corner of the screen. The projection will be pixel perfect
    /// with respect to the current screen resolution.
    /// <para>
    /// The defaultShader specifies the shader to use. Note that the names for uniforms for
    /// this default shader are different than the ones expect for shaders set with
    /// <see cref="Shader"/>.
    /// </para>
    /// </summary>
    /// <param name="maxVertices"> The max number of vertices in a single batch. Max of 32767.</param>
    /// <param name="maxTriangles"> The max number of triangles in a single batch. </param>
    /// <param name="defaultShader">
    /// The default shader to use. This is not owned by the PolygonSpriteBatch and must
    /// be disposed separately. May be null to use the default shader.
    /// </param>
    public PolygonSpriteBatch( int maxVertices, int maxTriangles, ShaderProgram? defaultShader )
    {
        // 32767 is max vertex index.
        if ( maxVertices > 32767 )
        {
            throw new ArgumentException( "Can't have more than 32767 vertices per batch: " + maxVertices );
        }

        _mesh = new Mesh( Mesh.VertexDataType.VertexBufferObjectWithVAO,
                          false,
                          maxVertices,
                          maxTriangles * 3,
                          new VertexAttribute( VertexAttributes.Usage.POSITION, 2, ShaderProgram.POSITION_ATTRIBUTE ),
                          new VertexAttribute( VertexAttributes.Usage.COLOR_PACKED, 4, ShaderProgram.COLOR_ATTRIBUTE ),
                          new VertexAttribute( VertexAttributes.Usage.TEXTURE_COORDINATES, 2, ShaderProgram.TEXCOORD_ATTRIBUTE + "0" ) );

        _vertices  = new float[ maxVertices * Sprite.VERTEX_SIZE ];
        _triangles = new short[ maxTriangles * 3 ];

        if ( defaultShader == null )
        {
            _shader     = SpriteBatch.CreateDefaultShader();
            _ownsShader = true;
        }
        else
        {
            _shader = defaultShader;
        }

        ProjectionMatrix.SetToOrtho2D( 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height );
    }

    public void Begin()
    {
        if ( IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.end must be called before begin." );
        }

        RenderCalls = 0;

        Gdx.GL.glDepthMask( false );

        if ( _customShader != null )
        {
            _customShader.Bind();
        }
        else
        {
            _shader?.Bind();
        }

        SetupMatrices();

        IsDrawing = true;
    }

    public void End()
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before end." );
        }

        if ( _vertexIndex > 0 )
        {
            Flush();
        }

        _lastTexture = null;
        IsDrawing    = false;

        Gdx.GL.glDepthMask( true );

        if ( IsBlendingEnabled() )
        {
            Gdx.GL.glDisable( IGL.GL_BLEND );
        }
    }

    public Color Color
    {
        get => _color;
        set
        {
            _color.Set( value );
            _colorPacked = value.ToFloatBitsABGR();
        }
    }

    public void SetColor( float r, float g, float b, float a )
    {
        _color.Set( r, g, b, a );
        _colorPacked = _color.ToFloatBitsABGR();
    }

    public float ColorPackedABGR
    {
        get => _colorPacked;
        set
        {
            var color = Color;

            Color.ABGR8888ToColor( ref color, value );
            _colorPacked = value;
        }
    }

    /// <summary>
    /// Draws the supplied <see cref="PolygonRegion"/> at the given corrdinates.
    /// </summary>
    /// <param name="region"> The Polygon Region to draw </param>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <exception cref="GdxRuntimeException"></exception>
    public void Draw( PolygonRegion region, float x, float y )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( region.Region.Texture != _lastTexture )
        {
            SwitchTexture( region.Region.Texture );
        }
        else if ( ( ( _triangleIndex + region.Triangles.Length ) > _triangles.Length )
               || ( ( _vertexIndex + ( ( region.Vertices?.Length * Sprite.VERTEX_SIZE ) / 2 ) ) > _vertices.Length ) )
        {
            Flush();
        }

        foreach ( var t in region.Triangles )
        {
            _triangles[ _triangleIndex++ ] = ( short ) ( t + ( _vertexIndex / Sprite.VERTEX_SIZE ) );
        }

        for ( var i = 0; i < region.Vertices?.Length; i += 2 )
        {
            _vertices[ _vertexIndex++ ] = region.Vertices[ i ] + x;
            _vertices[ _vertexIndex++ ] = region.Vertices[ i + 1 ] + y;
            _vertices[ _vertexIndex++ ] = _colorPacked;
            _vertices[ _vertexIndex++ ] = region.TextureCoords[ i ];
            _vertices[ _vertexIndex++ ] = region.TextureCoords[ i + 1 ];
        }
    }

    public void Draw( PolygonRegion region, float x, float y, float width, float height )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( region.Region.Texture != _lastTexture )
        {
            SwitchTexture( region.Region.Texture );
        }
        else if ( ( ( _triangleIndex + region.Triangles.Length ) > _triangles.Length )
               || ( ( _vertexIndex + ( ( region.Vertices?.Length * Sprite.VERTEX_SIZE ) / 2 ) ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        for ( int i = 0, n = region.Triangles.Length; i < n; i++ )
        {
            _triangles[ _triangleIndex++ ] = ( short ) ( region.Triangles[ i ] + startVertex );
        }

        var sX = width / region.Region.RegionWidth;
        var sY = height / region.Region.RegionHeight;

        for ( var i = 0; i < region.Vertices?.Length; i += 2 )
        {
            _vertices[ _vertexIndex++ ] = ( region.Vertices[ i ] * sX ) + x;
            _vertices[ _vertexIndex++ ] = ( region.Vertices[ i + 1 ] * sY ) + y;
            _vertices[ _vertexIndex++ ] = _colorPacked;
            _vertices[ _vertexIndex++ ] = region.TextureCoords[ i ];
            _vertices[ _vertexIndex++ ] = region.TextureCoords[ i + 1 ];
        }
    }

    public void Draw( PolygonRegion region,
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
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( region.Region.Texture != _lastTexture )
        {
            SwitchTexture( region.Region.Texture );
        }
        else if ( ( ( _triangleIndex + region.Triangles.Length ) > _triangles.Length )
               || ( ( _vertexIndex + ( ( region.Vertices?.Length * Sprite.VERTEX_SIZE ) / 2 ) ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        foreach ( var triangle in region.Triangles )
        {
            _triangles[ _triangleIndex++ ] = ( short ) ( triangle + startVertex );
        }

        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var sX           = width / region.Region.RegionWidth;
        var sY           = height / region.Region.RegionHeight;
        var cos          = MathUtils.CosDeg( rotation );
        var sin          = MathUtils.SinDeg( rotation );

        for ( var i = 0; i < region.Vertices?.Length; i += 2 )
        {
            var fx = ( ( region.Vertices[ i ] * sX ) - originX ) * scaleX;
            var fy = ( ( region.Vertices[ i + 1 ] * sY ) - originY ) * scaleY;

            _vertices[ _vertexIndex++ ] = ( ( cos * fx ) - ( sin * fy ) ) + worldOriginX;
            _vertices[ _vertexIndex++ ] = ( sin * fx ) + ( cos * fy ) + worldOriginY;
            _vertices[ _vertexIndex++ ] = _colorPacked;
            _vertices[ _vertexIndex++ ] = region.TextureCoords[ i ];
            _vertices[ _vertexIndex++ ] = region.TextureCoords[ i + 1 ];
        }
    }

    public void Draw( Texture texture,
                      float[] polygonVertices,
                      int verticesOffset,
                      int verticesCount,
                      short[] polygonTriangles,
                      int trianglesOffset,
                      int trianglesCount )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + trianglesCount ) > _triangles.Length )
               || ( ( _vertexIndex + verticesCount ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        for ( int i = trianglesOffset, n = i + trianglesCount; i < n; i++ )
        {
            _triangles[ _triangleIndex++ ] = ( short ) ( polygonTriangles[ i ] + startVertex );
        }

        Array.Copy( polygonVertices, verticesOffset, _vertices, _vertexIndex, verticesCount );
        _vertexIndex += verticesCount;
    }

    public void Draw( Texture texture,
                      GRect region,
                      Point2D origin,
                      Point2D scale,
                      float rotation,
                      GRect src,
                      bool flipX,
                      bool flipY )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        // bottom left and top right corner points relative to origin
        var worldOriginX = region.X + origin.X;
        var worldOriginY = region.Y + origin.Y;
        var fx           = -origin.X;
        var fy           = -origin.Y;
        var fx2          = region.Width - origin.X;
        var fy2          = region.Height - origin.Y;

        // scale
        if ( !scale.X.Equals( 1 ) || !scale.Y.Equals( 1 ) )
        {
            fx  *= scale.X;
            fy  *= scale.Y;
            fx2 *= scale.X;
            fy2 *= scale.Y;
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

        var u  = src.X * _invTexWidth;
        var v  = ( src.Y + src.Height ) * _invTexHeight;
        var u2 = ( src.X + src.Width ) * _invTexWidth;
        var v2 = src.Y * _invTexHeight;

        if ( flipX )
        {
            ( u, u2 ) = ( u2, u );
        }

        if ( flipY )
        {
            ( v, v2 ) = ( v2, v );
        }

        _vertices[ _vertexIndex++ ] = x1;
        _vertices[ _vertexIndex++ ] = y1;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v;

        _vertices[ _vertexIndex++ ] = x2;
        _vertices[ _vertexIndex++ ] = y2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = x3;
        _vertices[ _vertexIndex++ ] = y3;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = x4;
        _vertices[ _vertexIndex++ ] = y4;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v;
    }

    public void Draw( Texture texture,
                      GRect region,
                      GRect src,
                      bool flipX,
                      bool flipY )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        var u   = src.X * _invTexWidth;
        var v   = ( src.Y + src.Height ) * _invTexHeight;
        var u2  = ( src.X + src.Width ) * _invTexWidth;
        var v2  = src.Y * _invTexHeight;
        var fx2 = region.X + region.Width;
        var fy2 = region.Y + region.Height;

        if ( flipX )
        {
            ( u, u2 ) = ( u2, u );
        }

        if ( flipY )
        {
            ( v, v2 ) = ( v2, v );
        }

        _vertices[ _vertexIndex++ ] = region.X;
        _vertices[ _vertexIndex++ ] = region.Y;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v;

        _vertices[ _vertexIndex++ ] = region.X;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = region.Y;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v;
    }

    public void Draw( Texture texture, float x, float y, GRect src )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        var u   = src.X * _invTexWidth;
        var v   = ( src.Y + src.Height ) * _invTexHeight;
        var u2  = ( src.X + src.Width ) * _invTexWidth;
        var v2  = src.Y * _invTexHeight;
        var fx2 = x + src.Width;
        var fy2 = y + src.Height;

        _vertices[ _vertexIndex++ ] = x;
        _vertices[ _vertexIndex++ ] = y;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v;

        _vertices[ _vertexIndex++ ] = x;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = y;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v;
    }

    public void Draw( Texture texture, GRect region, float u, float v, float u2, float v2 )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        var fx2 = region.X + region.Width;
        var fy2 = region.Y + region.Height;

        _vertices[ _vertexIndex++ ] = region.X;
        _vertices[ _vertexIndex++ ] = region.Y;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v;

        _vertices[ _vertexIndex++ ] = region.X;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = region.X;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v;
    }

    public void Draw( Texture texture, float x, float y )
    {
        Draw( texture, x, y, texture.Width, texture.Height );
    }

    public void Draw( Texture texture, float posX, float posY, int width, int height )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        var   fx2 = posX + width;
        var   fy2 = posY + height;
        float u   = 0;
        float v   = 1;
        float u2  = 1;
        float v2  = 0;

        _vertices[ _vertexIndex++ ] = posX;
        _vertices[ _vertexIndex++ ] = posY;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v;

        _vertices[ _vertexIndex++ ] = posX;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = posY;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v;
    }

    public void Draw( Texture texture, float[] spriteVertices, int offset, int count )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangleCount = ( count / Sprite.SPRITE_SIZE ) * 6;
        int batch;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
            batch = Math.Min( Math.Min( count, _vertices.Length - ( _vertices.Length % Sprite.SPRITE_SIZE ) ), ( _triangles.Length / 6 ) * Sprite.SPRITE_SIZE );
            triangleCount = ( batch / Sprite.SPRITE_SIZE ) * 6;
        }
        else if ( ( ( _triangleIndex + triangleCount ) > _triangles.Length )
               || ( ( _vertexIndex + count ) > _vertices.Length ) )
        {
            Flush();
            batch = Math.Min( Math.Min( count, _vertices.Length - ( _vertices.Length % Sprite.SPRITE_SIZE ) ), ( _triangles.Length / 6 ) * Sprite.SPRITE_SIZE );
            triangleCount = ( batch / Sprite.SPRITE_SIZE ) * 6;
        }
        else
        {
            batch = count;
        }

        var vertex = ( short ) ( _vertexIndex / Sprite.VERTEX_SIZE );

        for ( var n = _triangleIndex + triangleCount; _triangleIndex < n; _triangleIndex += 6, vertex += 4 )
        {
            _triangles[ _triangleIndex ]     = vertex;
            _triangles[ _triangleIndex + 1 ] = ( short ) ( vertex + 1 );
            _triangles[ _triangleIndex + 2 ] = ( short ) ( vertex + 2 );
            _triangles[ _triangleIndex + 3 ] = ( short ) ( vertex + 2 );
            _triangles[ _triangleIndex + 4 ] = ( short ) ( vertex + 3 );
            _triangles[ _triangleIndex + 5 ] = vertex;
        }

        var vertexIndex   = _vertexIndex;
        var triangleIndex = _triangleIndex;

        while ( true )
        {
            Array.Copy( spriteVertices, offset, _vertices, vertexIndex, batch );
            _vertexIndex   =  vertexIndex + batch;
            _triangleIndex =  triangleIndex;
            count          -= batch;

            if ( count == 0 )
            {
                break;
            }

            offset += batch;
            Flush();
            vertexIndex = 0;

            if ( batch > count )
            {
                batch         = Math.Min( count, ( _triangles.Length / 6 ) * Sprite.SPRITE_SIZE );
                triangleIndex = ( batch / Sprite.SPRITE_SIZE ) * 6;
            }
        }
    }

    public void Draw( TextureRegion region, float x, float y )
    {
        Draw( region, x, y, region.RegionWidth, region.RegionHeight );
    }

    public void Draw( TextureRegion region, float x, float y, float width, float height )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( region.Texture != _lastTexture )
        {
            SwitchTexture( region.Texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) ) //
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        var fx2 = x + width;
        var fy2 = y + height;
        var u   = region.U;
        var v   = region.V2;
        var u2  = region.U2;
        var v2  = region.V;

        _vertices[ _vertexIndex++ ] = x;
        _vertices[ _vertexIndex++ ] = y;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v;

        _vertices[ _vertexIndex++ ] = x;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = fy2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = fx2;
        _vertices[ _vertexIndex++ ] = y;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v;
    }

    public void Draw( TextureRegion textureRegion,
                      GRect region,
                      Point2D origin,
                      Point2D scale,
                      float rotation )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( textureRegion.Texture != _lastTexture )
        {
            SwitchTexture( textureRegion.Texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) ) //
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        // bottom left and top right corner points relative to origin
        var worldOriginX = region.X + origin.X;
        var worldOriginY = region.Y + origin.Y;
        var fx           = -origin.X;
        var fy           = -origin.Y;
        var fx2          = region.Width - origin.X;
        var fy2          = region.Height - origin.Y;

        // scale
        if ( !scale.X.Equals( 1 ) || !scale.Y.Equals( 1 ) )
        {
            fx  *= scale.X;
            fy  *= scale.Y;
            fx2 *= scale.X;
            fy2 *= scale.Y;
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

        var u  = textureRegion.U;
        var v  = textureRegion.V2;
        var u2 = textureRegion.U2;
        var v2 = textureRegion.V;

        _vertices[ _vertexIndex++ ] = x1;
        _vertices[ _vertexIndex++ ] = y1;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v;

        _vertices[ _vertexIndex++ ] = x2;
        _vertices[ _vertexIndex++ ] = y2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = x3;
        _vertices[ _vertexIndex++ ] = y3;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = x4;
        _vertices[ _vertexIndex++ ] = y4;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v;
    }

    public void Draw( TextureRegion textureRegion,
                      GRect region,
                      Point2D origin,
                      Point2D scale,
                      float rotation,
                      bool clockwise )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( textureRegion.Texture != _lastTexture )
        {
            SwitchTexture( textureRegion.Texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        // bottom left and top right corner points relative to origin
        var worldOriginX = region.X + origin.X;
        var worldOriginY = region.Y + origin.Y;
        var fx           = -origin.X;
        var fy           = -origin.Y;
        var fx2          = region.Width - origin.X;
        var fy2          = region.Height - origin.Y;

        // scale
        if ( !scale.X.Equals( 1 ) || !scale.Y.Equals( 1 ) )
        {
            fx  *= scale.X;
            fy  *= scale.Y;
            fx2 *= scale.X;
            fy2 *= scale.Y;
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

        float u1, v1, u2, v2, u3, v3, u4, v4;

        if ( clockwise )
        {
            u1 = textureRegion.U2;
            v1 = textureRegion.V2;
            u2 = textureRegion.U;
            v2 = textureRegion.V2;
            u3 = textureRegion.U;
            v3 = textureRegion.V;
            u4 = textureRegion.U2;
            v4 = textureRegion.V;
        }
        else
        {
            u1 = textureRegion.U;
            v1 = textureRegion.V;
            u2 = textureRegion.U2;
            v2 = textureRegion.V;
            u3 = textureRegion.U2;
            v3 = textureRegion.V2;
            u4 = textureRegion.U;
            v4 = textureRegion.V2;
        }

        _vertices[ _vertexIndex++ ] = x1;
        _vertices[ _vertexIndex++ ] = y1;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u1;
        _vertices[ _vertexIndex++ ] = v1;

        _vertices[ _vertexIndex++ ] = x2;
        _vertices[ _vertexIndex++ ] = y2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = x3;
        _vertices[ _vertexIndex++ ] = y3;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u3;
        _vertices[ _vertexIndex++ ] = v3;

        _vertices[ _vertexIndex++ ] = x4;
        _vertices[ _vertexIndex++ ] = y4;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u4;
        _vertices[ _vertexIndex++ ] = v4;
    }

    public void Draw( TextureRegion region, float width, float height, Affine2 transform )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        if ( region.Texture != _lastTexture )
        {
            SwitchTexture( region.Texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > _triangles.Length )
               || ( ( _vertexIndex + Sprite.SPRITE_SIZE ) > _vertices.Length ) )
        {
            Flush();
        }

        var startVertex = _vertexIndex / Sprite.VERTEX_SIZE;

        _triangles[ _triangleIndex++ ] = ( short ) startVertex;
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 1 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 2 );
        _triangles[ _triangleIndex++ ] = ( short ) ( startVertex + 3 );
        _triangles[ _triangleIndex++ ] = ( short ) startVertex;

        // construct corner points
        var x1 = transform.M02;
        var y1 = transform.M12;
        var x2 = ( transform.M01 * height ) + transform.M02;
        var y2 = ( transform.M11 * height ) + transform.M12;
        var x3 = ( transform.M00 * width ) + ( transform.M01 * height ) + transform.M02;
        var y3 = ( transform.M10 * width ) + ( transform.M11 * height ) + transform.M12;
        var x4 = ( transform.M00 * width ) + transform.M02;
        var y4 = ( transform.M10 * width ) + transform.M12;

        var u  = region.U;
        var v  = region.V2;
        var u2 = region.U2;
        var v2 = region.V;

        _vertices[ _vertexIndex++ ] = x1;
        _vertices[ _vertexIndex++ ] = y1;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v;

        _vertices[ _vertexIndex++ ] = x2;
        _vertices[ _vertexIndex++ ] = y2;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = x3;
        _vertices[ _vertexIndex++ ] = y3;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v2;

        _vertices[ _vertexIndex++ ] = x4;
        _vertices[ _vertexIndex++ ] = y4;
        _vertices[ _vertexIndex++ ] = _colorPacked;
        _vertices[ _vertexIndex++ ] = u2;
        _vertices[ _vertexIndex++ ] = v;
    }

    public void Flush()
    {
        if ( _vertexIndex == 0 )
        {
            return;
        }

        RenderCalls++;
        TotalRenderCalls++;

        var trianglesInBatch = _triangleIndex;

        if ( trianglesInBatch > MaxTrianglesInBatch )
        {
            MaxTrianglesInBatch = trianglesInBatch;
        }

        _lastTexture?.Bind();

        var mesh = _mesh;

        mesh.SetVertices( _vertices, 0, _vertexIndex );
        mesh.SetIndices( _triangles, 0, trianglesInBatch );

        if ( _blendingDisabled )
        {
            Gdx.GL.glDisable( IGL.GL_BLEND );
        }
        else
        {
            Gdx.GL.glEnable( IGL.GL_BLEND );

            if ( BlendSrcFunc != -1 )
            {
                Gdx.GL.glBlendFuncSeparate( BlendSrcFunc, BlendDstFunc, BlendSrcFuncAlpha, BlendDstFuncAlpha );
            }
        }

        mesh.Render( _customShader ?? _shader, IGL.GL_TRIANGLES, 0, trianglesInBatch );

        _vertexIndex   = 0;
        _triangleIndex = 0;
    }

    public void DisableBlending()
    {
        Flush();
        _blendingDisabled = true;
    }

    public void EnableBlending()
    {
        Flush();
        _blendingDisabled = false;
    }

    public void SetBlendFunction( int srcFunc, int dstFunc )
    {
        SetBlendFunctionSeparate( srcFunc, dstFunc, srcFunc, dstFunc );
    }

    public void SetBlendFunctionSeparate( int srcFuncColor, int dstFuncColor, int srcFuncAlpha, int dstFuncAlpha )
    {
        if ( ( BlendSrcFunc == srcFuncColor )
          && ( BlendDstFunc == dstFuncColor )
          && ( BlendSrcFuncAlpha == srcFuncAlpha )
          && ( BlendDstFuncAlpha == dstFuncAlpha ) )
        {
            return;
        }

        Flush();

        BlendSrcFunc      = srcFuncColor;
        BlendDstFunc      = dstFuncColor;
        BlendSrcFuncAlpha = srcFuncAlpha;
        BlendDstFuncAlpha = dstFuncAlpha;
    }

    public void Dispose()
    {
        _mesh.Dispose();

        if ( _ownsShader && ( _shader != null ) )
        {
            _shader.Dispose();
        }
    }

    public void SetProjectionMatrix( Matrix4 projection )
    {
        if ( IsDrawing )
        {
            Flush();
        }

        ProjectionMatrix.Set( projection );

        if ( IsDrawing )
        {
            SetupMatrices();
        }
    }

    public void SetTransformMatrix( Matrix4 transform )
    {
        if ( IsDrawing )
        {
            Flush();
        }

        TransformMatrix.Set( transform );

        if ( IsDrawing )
        {
            SetupMatrices();
        }
    }

    public ShaderProgram? Shader
    {
        get => _customShader ?? _shader;
        set
        {
            if ( IsDrawing )
            {
                Flush();
            }

            _customShader = value;

            if ( IsDrawing )
            {
                if ( _customShader != null )
                {
                    _customShader.Bind();
                }
                else
                {
                    _shader?.Bind();
                }

                SetupMatrices();
            }
        }
    }

    public void SetupMatrices()
    {
        _combinedMatrix.Set( ProjectionMatrix ).Mul( TransformMatrix );

        if ( _customShader != null )
        {
            _customShader.SetUniformMatrix( "u_projTrans", _combinedMatrix );
            _customShader.SetUniformi( "u_texture", 0 );
        }
        else
        {
            _shader?.SetUniformMatrix( "u_projTrans", _combinedMatrix );
            _shader?.SetUniformi( "u_texture", 0 );
        }
    }

    private void SwitchTexture( Texture texture )
    {
        Flush();
        _lastTexture  = texture;
        _invTexWidth  = 1.0f / texture.Width;
        _invTexHeight = 1.0f / texture.Height;
    }

    public bool IsBlendingEnabled()
    {
        return !_blendingDisabled;
    }
}
