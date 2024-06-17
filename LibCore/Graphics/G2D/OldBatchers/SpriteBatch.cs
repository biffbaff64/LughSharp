// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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


using LughSharp.LibCore.Utils.Exceptions;
using Matrix4 = LughSharp.LibCore.Maths.Matrix4;

namespace LughSharp.LibCore.Graphics.G2D.OldBatchers;

[PublicAPI]
public class SpriteBatch : IBatch
{
    // ------------------------------------------------------------------------

    private const int MAX_VERTEX_INDEX = 32767;
    private const int MAX_SPRITES      = 8191;

    private readonly Color          _color          = new( 1, 1, 1, 1 );
    private readonly Matrix4        _combinedMatrix = new();
    private readonly Mesh           _mesh;
    private readonly bool           _ownsShader;
    private readonly ShaderProgram? _shader;
    private          ShaderProgram? _customShader;

    protected float ColorPacked = Color.WhiteFloatBits;
    protected int   Idx         = 0;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Constructs a new SpriteBatch with a size of 1000, one buffer,
    /// and the default shader.
    /// </summary>
    public SpriteBatch() : this( 1000 )
    {
    }

    /// <summary>
    /// Constructs a new SpriteBatch. Sets the projection matrix to an orthographic
    /// projection with y-axis point upwards, x-axis point to the right and the origin
    /// being in the bottom left corner of the screen. The projection will be pixel
    /// perfect with respect to the current screen resolution.
    /// <para>
    /// The defaultShader specifies the shader to use. Note that the names for uniforms
    /// for this default shader are different than the ones expect for shaders set with
    /// <see cref="Shader"/>. See <see cref="CreateDefaultShader()"/>.
    /// </para>
    /// </summary>
    /// <param name="size">
    /// The max number of sprites in a single batch. Max of 8191.
    /// </param>
    /// <param name="defaultShader">
    /// The default shader to use. This is not owned by the SpriteBatch and must
    /// be disposed separately.
    /// </param>
    protected SpriteBatch( int size, ShaderProgram? defaultShader = null )
    {
        // 32767 is max vertex index, so 32767 / 4 vertices per sprite = 8191 sprites max.
        if ( size > MAX_SPRITES )
        {
            throw new ArgumentException( "Can't have more than 8191 sprites per batch: " + size );
        }

        IsDrawing = false;

        var vertexDataType = Mesh.VertexDataType.VertexBufferObjectWithVAO;

        _mesh = new Mesh( vertexDataType,
                          false,
                          size * 4,
                          size * 6,
                          new VertexAttribute( VertexAttributes.Usage.POSITION,
                                               2,
                                               ShaderProgram.POSITION_ATTRIBUTE ),
                          new VertexAttribute( VertexAttributes.Usage.COLOR_PACKED,
                                               4,
                                               ShaderProgram.COLOR_ATTRIBUTE ),
                          new VertexAttribute( VertexAttributes.Usage.TEXTURE_COORDINATES,
                                               2,
                                               ShaderProgram.TEXCOORD_ATTRIBUTE + "0" ) );

        ProjectionMatrix.SetToOrtho2D( 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height );

        Vertices = new float[ size * Sprite.SPRITE_SIZE ];

        var   len     = size * 6;
        var   indices = new short[ len ];
        short j       = 0;

        for ( var i = 0; i < len; i += 6, j += 4 )
        {
            indices[ i ]     = j;
            indices[ i + 1 ] = ( short ) ( j + 1 );
            indices[ i + 2 ] = ( short ) ( j + 2 );
            indices[ i + 3 ] = ( short ) ( j + 2 );
            indices[ i + 4 ] = ( short ) ( j + 3 );
            indices[ i + 5 ] = j;
        }

        _mesh.SetIndices( indices );

        if ( defaultShader == null )
        {
            _shader     = CreateDefaultShader();
            _ownsShader = true;
        }
        else
        {
            _shader = defaultShader;
        }
    }

    // Number of render calls since the last call to Begin()
    public int RenderCalls { get; set; } = 0;

    // Number of rendering calls, ever. Will not be reset unless set manually.
    public int TotalRenderCalls { get; set; } = 0;

    // The maximum number of sprites rendered in one batch so far.
    public int MaxSpritesInBatch { get; set; } = 0;

    public bool  BlendingDisabled { get; set; } = false;
    public float InvTexHeight     { get; set; } = 0;
    public float InvTexWidth      { get; set; } = 0;

    protected Texture? LastTexture       { get; set; }
    protected float[]  Vertices          { get; set; }
    public    int      BlendSrcFunc      { get; private set; } = IGL.GL_SRC_ALPHA;
    public    int      BlendDstFunc      { get; private set; } = IGL.GL_ONE_MINUS_SRC_ALPHA;
    public    int      BlendSrcFuncAlpha { get; private set; } = IGL.GL_SRC_ALPHA;
    public    int      BlendDstFuncAlpha { get; private set; } = IGL.GL_ONE_MINUS_SRC_ALPHA;
    public    Matrix4  ProjectionMatrix  { get; }              = new();
    public    Matrix4  TransformMatrix   { get; }              = new();
    public    bool     IsDrawing         { get; set; }

    public void Begin()
    {
        if ( IsDrawing )
        {
            throw new InvalidOperationException( "SpriteBatch.End() must be called before Begin()" );
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
            throw new InvalidOperationException( "SpriteBatch.begin must be called before end." );
        }

        if ( Idx > 0 )
        {
            Flush();
        }

        LastTexture = null;
        IsDrawing   = false;

        Gdx.GL.glDepthMask( true );

        if ( !BlendingDisabled )
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
            ColorPacked = value.ToFloatBits();
        }
    }

    public void SetColor( float r, float g, float b, float a )
    {
        _color.Set( r, g, b, a );
        ColorPacked = _color.ToFloatBits();
    }

    public float PackedColor
    {
        set
        {
            var color = Color;

            Color.ABGR8888ToColor( ref color, value );
            ColorPacked = value;

            Color = color;
        }
        get => ColorPacked;
    }

    public virtual void Draw( Texture texture,
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
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "SpriteBatch.begin must be called before draw." );
        }

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( ( Math.Abs( scaleX - 1 ) > 0 ) || ( Math.Abs( scaleY - 1 ) > 0 ) )
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

        var u     = srcX * InvTexWidth;
        var v     = ( srcY + srcHeight ) * InvTexHeight;
        var u2    = ( srcX + srcWidth ) * InvTexWidth;
        var v2    = srcY * InvTexHeight;
        var color = ColorPacked;

        if ( flipX )
        {
            ( u, u2 ) = ( u2, u );
        }

        if ( flipY )
        {
            ( v, v2 ) = ( v2, v );
        }

        Vertices[ Idx ]     = x1;
        Vertices[ Idx + 1 ] = y1;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x2;
        Vertices[ Idx + 6 ] = y2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = x3;
        Vertices[ Idx + 11 ] = y3;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = x4;
        Vertices[ Idx + 16 ] = y4;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx = Idx + 20;
    }

    public virtual void Draw( Texture texture,
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
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        var u     = srcX * InvTexWidth;
        var v     = ( srcY + srcHeight ) * InvTexHeight;
        var u2    = ( srcX + srcWidth ) * InvTexWidth;
        var v2    = srcY * InvTexHeight;
        var fx2   = x + width;
        var fy2   = y + height;
        var color = ColorPacked;

        if ( flipX )
        {
            ( u, u2 ) = ( u2, u );
        }

        if ( flipY )
        {
            ( v, v2 ) = ( v2, v );
        }

        Vertices[ Idx ]     = x;
        Vertices[ Idx + 1 ] = y;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x;
        Vertices[ Idx + 6 ] = fy2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = fx2;
        Vertices[ Idx + 11 ] = fy2;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = fx2;
        Vertices[ Idx + 16 ] = y;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx = Idx + 20;
    }

    public virtual void Draw( Texture texture, float x, float y, int srcX, int srcY, int srcWidth, int srcHeight )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        var u   = srcX * InvTexWidth;
        var v   = ( srcY + srcHeight ) * InvTexHeight;
        var u2  = ( srcX + srcWidth ) * InvTexWidth;
        var v2  = srcY * InvTexHeight;
        var fx2 = x + srcWidth;
        var fy2 = y + srcHeight;

        var color = ColorPacked;

        Vertices[ Idx ]     = x;
        Vertices[ Idx + 1 ] = y;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x;
        Vertices[ Idx + 6 ] = fy2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = fx2;
        Vertices[ Idx + 11 ] = fy2;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = fx2;
        Vertices[ Idx + 16 ] = y;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx = Idx + 20;
    }

    public virtual void Draw( Texture texture,
                              float x,
                              float y,
                              float width,
                              float height,
                              float u,
                              float v,
                              float u2,
                              float v2 )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        var fx2   = x + width;
        var fy2   = y + height;
        var color = ColorPacked;

        Vertices[ Idx ]     = x;
        Vertices[ Idx + 1 ] = y;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x;
        Vertices[ Idx + 6 ] = fy2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = fx2;
        Vertices[ Idx + 11 ] = fy2;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = fx2;
        Vertices[ Idx + 16 ] = y;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx = Idx + 20;
    }

    public virtual void Draw( Texture texture, float x, float y )
    {
        Draw( texture, x, y, texture.Width, texture.Height );
    }

    public virtual void Draw( Texture texture, float x, float y, float width, float height )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        var fx2   = x + width;
        var fy2   = y + height;
        var color = ColorPacked;

        const float U  = 0;
        const float V  = 1;
        const float U2 = 1;
        const float V2 = 0;

        Vertices[ Idx ]     = x;
        Vertices[ Idx + 1 ] = y;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = U;
        Vertices[ Idx + 4 ] = V;

        Vertices[ Idx + 5 ] = x;
        Vertices[ Idx + 6 ] = fy2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = U;
        Vertices[ Idx + 9 ] = V2;

        Vertices[ Idx + 10 ] = fx2;
        Vertices[ Idx + 11 ] = fy2;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = U2;
        Vertices[ Idx + 14 ] = V2;

        Vertices[ Idx + 15 ] = fx2;
        Vertices[ Idx + 16 ] = y;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = U2;
        Vertices[ Idx + 19 ] = V;

        Idx = Idx + 20;
    }

    public virtual void Draw( Texture texture, float[] spriteVertices, int offset, int count )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        var verticesLength    = Vertices.Length;
        var remainingVertices = verticesLength;

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else
        {
            remainingVertices -= Idx;

            if ( remainingVertices == 0 )
            {
                Flush();
                remainingVertices = verticesLength;
            }
        }

        var copyCount = Math.Min( remainingVertices, count );

        Array.Copy(
                   spriteVertices,
                   offset,
                   Vertices,
                   Idx,
                   copyCount
                  );

        Idx   += copyCount;
        count -= copyCount;

        while ( count > 0 )
        {
            offset += copyCount;

            Flush();

            copyCount = Math.Min( verticesLength, count );

            Array.Copy(
                       spriteVertices,
                       offset,
                       Vertices,
                       0,
                       copyCount
                      );

            Idx   += copyCount;
            count -= copyCount;
        }
    }

    public virtual void Draw( TextureRegion region, float x, float y )
    {
        Draw( region, x, y, region.RegionWidth, region.RegionHeight );
    }

    public virtual void Draw( TextureRegion region, float x, float y, float width, float height )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        var texture = region.Texture;

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        var fx2 = x + width;
        var fy2 = y + height;
        var u   = region.U;
        var v   = region.V2;
        var u2  = region.U2;
        var v2  = region.V;

        var color = ColorPacked;

        Vertices[ Idx ]     = x;
        Vertices[ Idx + 1 ] = y;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x;
        Vertices[ Idx + 6 ] = fy2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = fx2;
        Vertices[ Idx + 11 ] = fy2;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = fx2;
        Vertices[ Idx + 16 ] = y;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx = Idx + 20;
    }

    public virtual void Draw( TextureRegion region,
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
            throw new InvalidOperationException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        var texture = region.Texture;

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( ( Math.Abs( scaleX - 1 ) > 0 ) || ( Math.Abs( scaleY - 1 ) > 0 ) )
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

        var color = ColorPacked;

        Vertices[ Idx ]     = x1;
        Vertices[ Idx + 1 ] = y1;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x2;
        Vertices[ Idx + 6 ] = y2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = x3;
        Vertices[ Idx + 11 ] = y3;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = x4;
        Vertices[ Idx + 16 ] = y4;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx = Idx + 20;
    }

    public virtual void Draw( TextureRegion region,
                              float x,
                              float y,
                              float originX,
                              float originY,
                              float width,
                              float height,
                              float scaleX,
                              float scaleY,
                              float rotation,
                              bool clockwise )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        var texture = region.Texture;

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( ( Math.Abs( scaleX - 1 ) > 0 ) || ( Math.Abs( scaleY - 1 ) > 0 ) )
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

        float u1, v1, u2, v2, u3, v3, u4, v4;

        if ( clockwise )
        {
            u1 = region.U2;
            v1 = region.V2;
            u2 = region.U;
            v2 = region.V2;
            u3 = region.U;
            v3 = region.V;
            u4 = region.U2;
            v4 = region.V;
        }
        else
        {
            u1 = region.U;
            v1 = region.V;
            u2 = region.U2;
            v2 = region.V;
            u3 = region.U2;
            v3 = region.V2;
            u4 = region.U;
            v4 = region.V2;
        }

        var color = ColorPacked;

        Vertices[ Idx ]     = x1;
        Vertices[ Idx + 1 ] = y1;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = u1;
        Vertices[ Idx + 4 ] = v1;

        Vertices[ Idx + 5 ] = x2;
        Vertices[ Idx + 6 ] = y2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = u2;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = x3;
        Vertices[ Idx + 11 ] = y3;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = u3;
        Vertices[ Idx + 14 ] = v3;

        Vertices[ Idx + 15 ] = x4;
        Vertices[ Idx + 16 ] = y4;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = u4;
        Vertices[ Idx + 19 ] = v4;

        Idx = Idx + 20;
    }

    public virtual void Draw( TextureRegion region, float width, float height, Affine2 transform )
    {
        if ( !IsDrawing )
        {
            throw new GdxRuntimeException( "SpriteBatch.Begin() must be called before Draw()" );
        }

        if ( region.Texture != LastTexture )
        {
            SwitchTexture( region.Texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        // construct corner points
        var x1 = transform.m02;
        var y1 = transform.m12;
        var x2 = ( transform.m01 * height ) + transform.m02;
        var y2 = ( transform.m11 * height ) + transform.m12;
        var x3 = ( transform.m00 * width ) + ( transform.m01 * height ) + transform.m02;
        var y3 = ( transform.m10 * width ) + ( transform.m11 * height ) + transform.m12;
        var x4 = ( transform.m00 * width ) + transform.m02;
        var y4 = ( transform.m10 * width ) + transform.m12;

        var u     = region.U;
        var v     = region.V2;
        var u2    = region.U2;
        var v2    = region.V;
        var color = ColorPacked;

        Vertices[ Idx ]     = x1;
        Vertices[ Idx + 1 ] = y1;
        Vertices[ Idx + 2 ] = color;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x2;
        Vertices[ Idx + 6 ] = y2;
        Vertices[ Idx + 7 ] = color;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = x3;
        Vertices[ Idx + 11 ] = y3;
        Vertices[ Idx + 12 ] = color;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = x4;
        Vertices[ Idx + 16 ] = y4;
        Vertices[ Idx + 17 ] = color;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx = Idx + 20;
    }

    /// <summary>
    /// </summary>
    public void Flush()
    {
        if ( Idx == 0 )
        {
            return;
        }

        RenderCalls++;
        TotalRenderCalls++;

        var spritesInBatch = Idx / 20;

        if ( spritesInBatch > MaxSpritesInBatch )
        {
            MaxSpritesInBatch = spritesInBatch;
        }

        var count = spritesInBatch * 6;

        LastTexture?.Bind();

        _mesh.SetVertices( Vertices, 0, Idx );
        _mesh.IndicesBuffer.Position = 0;
        _mesh.IndicesBuffer.Limit    = count;

        if ( BlendingDisabled )
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

        _mesh.Render( _customShader ?? _shader, IGL.GL_TRIANGLES, 0, count );

        Idx = 0;
    }

    public void DisableBlending()
    {
        if ( BlendingDisabled )
        {
            return;
        }

        Flush();
        BlendingDisabled = true;
    }

    public void EnableBlending()
    {
        if ( !BlendingDisabled )
        {
            return;
        }

        Flush();
        BlendingDisabled = false;
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

    public virtual void SetTransformMatrix( Matrix4 transform )
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

    /// <summary>
    /// Returns a new instance of the default shader used by SpriteBatch
    /// for GL2 when no shader is specified.
    /// </summary>
    public static ShaderProgram CreateDefaultShader()
    {
        const string VERTEX_SHADER = "attribute vec4 "
                                   + ShaderProgram.POSITION_ATTRIBUTE
                                   + ";\n"
                                   + "attribute vec4 "
                                   + ShaderProgram.COLOR_ATTRIBUTE
                                   + ";\n"
                                   + "attribute vec2 "
                                   + ShaderProgram.TEXCOORD_ATTRIBUTE
                                   + "0;\n"
                                   + "uniform mat4 u_projTrans;\n"
                                   + "varying vec4 v_color;\n"
                                   + "varying vec2 v_texCoords;\n"
                                   + "\n"
                                   + "void main()\n"
                                   + "{\n"
                                   + "   v_color = "
                                   + ShaderProgram.COLOR_ATTRIBUTE
                                   + ";\n"
                                   + "   v_color.a = v_color.a * (255.0/254.0);\n"
                                   + "   v_texCoords = "
                                   + ShaderProgram.TEXCOORD_ATTRIBUTE
                                   + "0;\n"
                                   + "   gl_Position =  u_projTrans * "
                                   + ShaderProgram.POSITION_ATTRIBUTE
                                   + ";\n"
                                   + "}\n";

        const string FRAGMENT_SHADER = "#ifdef GL_ES\n"
                                     + "#define LOWP lowp\n"
                                     + "precision mediump float;\n"
                                     + "#else\n"
                                     + "#define LOWP \n"
                                     + "#endif\n"
                                     + "varying LOWP vec4 v_color;\n"
                                     + "varying vec2 v_texCoords;\n"
                                     + "uniform sampler2D u_texture;\n"
                                     + "void main()\n"
                                     + "{\n"
                                     + "  gl_FragColor = v_color * texture2D(u_texture, v_texCoords);\n"
                                     + "}";

        var shader = new ShaderProgram( VERTEX_SHADER, FRAGMENT_SHADER );

        if ( !shader.IsCompiled )
        {
            throw new ArgumentException( "Error compiling shader: " + shader.Log );
        }

        return shader;
    }

    protected void SetupMatrices()
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

    protected void SwitchTexture( Texture texture )
    {
        Flush();
        LastTexture  = texture;
        InvTexWidth  = 1.0f / texture.Width;
        InvTexHeight = 1.0f / texture.Height;
    }
}
