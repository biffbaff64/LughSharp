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


using System.Drawing;
using LughSharp.LibCore.Utils.Exceptions;
using Matrix4 = LughSharp.LibCore.Maths.Matrix4;

namespace LughSharp.LibCore.Graphics.G2D;

[PublicAPI]
public class SpriteBatch : IBatch
{
    public bool    BlendingDisabled  { get; set; }         = false;
    public float   InvTexHeight      { get; set; }         = 0;
    public float   InvTexWidth       { get; set; }         = 0;
    public int     BlendSrcFunc      { get; private set; } = IGL.GL_SRC_ALPHA;
    public int     BlendDstFunc      { get; private set; } = IGL.GL_ONE_MINUS_SRC_ALPHA;
    public int     BlendSrcFuncAlpha { get; private set; } = IGL.GL_SRC_ALPHA;
    public int     BlendDstFuncAlpha { get; private set; } = IGL.GL_ONE_MINUS_SRC_ALPHA;
    public Matrix4 ProjectionMatrix  { get; }              = new();
    public Matrix4 TransformMatrix   { get; }              = new();
    public bool    IsDrawing         { get; set; }         = false;

    // Number of render calls since the last call to Begin()
    public int RenderCalls { get; set; } = 0;

    // Number of rendering calls, ever. Will not be reset unless set manually.
    public long TotalRenderCalls { get; set; } = 0;

    // The maximum number of sprites rendered in one batch so far.
    public int MaxSpritesInBatch { get; set; } = 0;

    // ------------------------------------------------------------------------

    protected Texture? LastTexture { get; set; } = null;
    protected float[]  Vertices    { get; set; }
    protected int      Idx         { get; set; } = 0;

    // ------------------------------------------------------------------------

    private const int MAX_VERTEX_INDEX = 32767;
    private const int MAX_SPRITES      = 8191;

    private readonly Color   _color          = new( 1, 1, 1, 1 );
    private readonly Matrix4 _combinedMatrix = new();
    private readonly bool    _ownsShader;

    private Mesh?          _mesh;
    private ShaderProgram? _shader;
    private Texture?       _lastSuccessfulTexture = null;
    private int            _nullTextureCount      = 0;
    private ShaderProgram? _customShader          = null;
    private float          _colorPacked           = Color.WhiteFloatBits;

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

        _mesh = new Mesh( Mesh.VertexDataType.VertexBufferObjectWithVAO,
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
                                               $"{ShaderProgram.TEXCOORD_ATTRIBUTE}0" ) );

        ProjectionMatrix.SetToOrtho2D( 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height );

        Vertices = new float[ size * Sprite.SPRITE_SIZE ];

        var len     = size * 6;
        var indices = new short[ len ];

        for ( short i = 0, j = 0; i < len; i += 6, j += 4 )
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

        if ( IsBlendingEnabled )
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
            _colorPacked = value.ToFloatBits();
        }
    }

    public void SetColor( float r, float g, float b, float a )
    {
        _color.Set( r, g, b, a );
        _colorPacked = _color.ToFloatBits();
    }

    public float PackedColor
    {
        set
        {
            var color = Color;

            Color.ABGR8888ToColor( ref color, value );
            _colorPacked = value;

            Color = color;
        }
        get => _colorPacked;
    }

    public virtual void Draw( Texture? texture,
                              GRect region,
                              Point2D origin,
                              Point2D scale,
                              float rotation,
                              GRect src,
                              bool flipX,
                              bool flipY )
    {
        Validate( texture );
        
        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = region.X + origin.X;
        var worldOriginY = region.Y + origin.Y;
        var fx           = -origin.X;
        var fy           = -origin.Y;
        var fx2          = region.Width - origin.X;
        var fy2          = region.Height - origin.Y;

        // scale
        if ( ( scale.X != 1 ) || ( scale.Y != 1 ) )
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

        var u  = src.X * InvTexWidth;
        var v  = ( src.Y + src.Height ) * InvTexHeight;
        var u2 = ( src.X + src.Width ) * InvTexWidth;
        var v2 = src.Y * InvTexHeight;

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
        Vertices[ Idx + 2 ] = PackedColor;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x2;
        Vertices[ Idx + 6 ] = y2;
        Vertices[ Idx + 7 ] = PackedColor;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = x3;
        Vertices[ Idx + 11 ] = y3;
        Vertices[ Idx + 12 ] = PackedColor;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = x4;
        Vertices[ Idx + 16 ] = y4;
        Vertices[ Idx + 17 ] = PackedColor;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx += 20;
    }

    public virtual void Draw( Texture? texture,
                              GRect region,
                              GRect src,
                              bool flipX,
                              bool flipY )
    {
        Validate( texture );

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        var u   = src.X * InvTexWidth;
        var v   = ( src.Y + src.Height ) * InvTexHeight;
        var u2  = ( src.X + src.Width ) * InvTexWidth;
        var v2  = src.Y * InvTexHeight;
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

        Vertices[ Idx ]     = region.X;
        Vertices[ Idx + 1 ] = region.Y;
        Vertices[ Idx + 2 ] = PackedColor;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = region.X;
        Vertices[ Idx + 6 ] = fy2;
        Vertices[ Idx + 7 ] = PackedColor;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = fx2;
        Vertices[ Idx + 11 ] = fy2;
        Vertices[ Idx + 12 ] = PackedColor;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = fx2;
        Vertices[ Idx + 16 ] = region.Y;
        Vertices[ Idx + 17 ] = PackedColor;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx += 20;
    }

    public virtual void Draw( Texture? texture, float x, float y, GRect src )
    {
        Validate( texture );

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        var u   = src.X * InvTexWidth;
        var v   = ( src.Y + src.Height ) * InvTexHeight;
        var u2  = ( src.X + src.Width ) * InvTexWidth;
        var v2  = src.Y * InvTexHeight;
        var fx2 = x + src.Width;
        var fy2 = y + src.Height;

        Vertices[ Idx ]     = x;
        Vertices[ Idx + 1 ] = y;
        Vertices[ Idx + 2 ] = PackedColor;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = x;
        Vertices[ Idx + 6 ] = fy2;
        Vertices[ Idx + 7 ] = PackedColor;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = fx2;
        Vertices[ Idx + 11 ] = fy2;
        Vertices[ Idx + 12 ] = PackedColor;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = fx2;
        Vertices[ Idx + 16 ] = y;
        Vertices[ Idx + 17 ] = PackedColor;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx += 20;
    }

    public virtual void Draw( Texture? texture,
                              GRect region,
                              float u,
                              float v,
                              float u2,
                              float v2 )
    {
        Validate( texture );

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        var fx2 = region.X + region.Width;
        var fy2 = region.Y + region.Height;

        Vertices[ this.Idx ]     = region.X;
        Vertices[ this.Idx + 1 ] = region.Y;
        Vertices[ this.Idx + 2 ] = this.PackedColor;
        Vertices[ this.Idx + 3 ] = u;
        Vertices[ this.Idx + 4 ] = v;

        Vertices[ this.Idx + 5 ] = region.X;
        Vertices[ this.Idx + 6 ] = fy2;
        Vertices[ this.Idx + 7 ] = this.PackedColor;
        Vertices[ this.Idx + 8 ] = u;
        Vertices[ this.Idx + 9 ] = v2;

        Vertices[ this.Idx + 10 ] = fx2;
        Vertices[ this.Idx + 11 ] = fy2;
        Vertices[ this.Idx + 12 ] = this.PackedColor;
        Vertices[ this.Idx + 13 ] = u2;
        Vertices[ this.Idx + 14 ] = v2;

        Vertices[ this.Idx + 15 ] = fx2;
        Vertices[ this.Idx + 16 ] = region.Y;
        Vertices[ this.Idx + 17 ] = this.PackedColor;
        Vertices[ this.Idx + 18 ] = u2;
        Vertices[ this.Idx + 19 ] = v;

        this.Idx += 20;
    }

    /// <summary>
    /// Draw the given <see cref="Texture"/> at the given X and Y coordinates.
    /// </summary>
    /// <param name="texture"> The texture. </param>
    /// <param name="x"> X coordinate in pixels. </param>
    /// <param name="y"> Y coordinate in pixels. </param>
    public virtual void Draw( Texture? texture, float x, float y )
    {
        Validate( texture );

        Draw( texture, x, y, texture!.Width, texture.Height );
    }

    public virtual void Draw( Texture? texture, float locX, float locY, int width, int height )
    {
        Validate( texture );

        var vertices = this.Vertices;
        
        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == vertices.Length )
        {
            Flush();
        }

        var fx2 = locX + width;
        var fy2 = locY + height;

        const float U  = 0;
        const float V  = 1;
        const float U2 = 1;
        const float V2 = 0;

        vertices[ this.Idx ]     = locX;
        vertices[ this.Idx + 1 ] = locY;
        vertices[ this.Idx + 2 ] = this.PackedColor;
        vertices[ this.Idx + 3 ] = U;
        vertices[ this.Idx + 4 ] = V;

        vertices[ this.Idx + 5 ] = locX;
        vertices[ this.Idx + 6 ] = fy2;
        vertices[ this.Idx + 7 ] = this.PackedColor;
        vertices[ this.Idx + 8 ] = U;
        vertices[ this.Idx + 9 ] = V2;

        vertices[ this.Idx + 10 ] = fx2;
        vertices[ this.Idx + 11 ] = fy2;
        vertices[ this.Idx + 12 ] = this.PackedColor;
        vertices[ this.Idx + 13 ] = U2;
        vertices[ this.Idx + 14 ] = V2;

        vertices[ this.Idx + 15 ] = fx2;
        vertices[ this.Idx + 16 ] = locY;
        vertices[ this.Idx + 17 ] = this.PackedColor;
        vertices[ this.Idx + 18 ] = U2;
        vertices[ this.Idx + 19 ] = V;

        this.Idx += 20;
    }

    public virtual void Draw( Texture? texture, float[] spriteVertices, int offset, int count )
    {
        Validate( texture );

        var vertices          = this.Vertices;
        var verticesLength    = vertices.Length;
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

        Array.Copy( spriteVertices, offset, vertices, Idx, copyCount );

        Idx   += copyCount;
        count -= copyCount;

        while ( count > 0 )
        {
            offset += copyCount;

            Flush();

            copyCount = Math.Min( verticesLength, count );

            Array.Copy( spriteVertices, offset, vertices, 0, copyCount );

            Idx   += copyCount;
            count -= copyCount;
        }
    }

    public virtual void Draw( TextureRegion? region, float x, float y )
    {
        Validate( region );

        Draw( region, x, y, region!.RegionWidth, region.RegionHeight );
    }

    public virtual void Draw( TextureRegion? region, float x, float y, float width, float height )
    {
        Validate( region );

        var vertices = this.Vertices;
        var texture  = region!.Texture;

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == vertices.Length )
        {
            Flush();
        }

        var fx2 = x + width;
        var fy2 = y + height;
        var u   = region.U;
        var v   = region.V2;
        var u2  = region.U2;
        var v2  = region.V;

        vertices[ Idx ]     = x;
        vertices[ Idx + 1 ] = y;
        vertices[ Idx + 2 ] = PackedColor;
        vertices[ Idx + 3 ] = u;
        vertices[ Idx + 4 ] = v;

        vertices[ Idx + 5 ] = x;
        vertices[ Idx + 6 ] = fy2;
        vertices[ Idx + 7 ] = PackedColor;
        vertices[ Idx + 8 ] = u;
        vertices[ Idx + 9 ] = v2;

        vertices[ Idx + 10 ] = fx2;
        vertices[ Idx + 11 ] = fy2;
        vertices[ Idx + 12 ] = PackedColor;
        vertices[ Idx + 13 ] = u2;
        vertices[ Idx + 14 ] = v2;

        vertices[ Idx + 15 ] = fx2;
        vertices[ Idx + 16 ] = y;
        vertices[ Idx + 17 ] = PackedColor;
        vertices[ Idx + 18 ] = u2;
        vertices[ Idx + 19 ] = v;

        Idx += 20;
    }

    public virtual void Draw( TextureRegion? textureRegion,
                              GRect region,
                              Point2D origin,
                              Point2D scale,
                              float rotation )
    {
        Validate( textureRegion );

        var vertices = this.Vertices;
        var texture  = textureRegion!.Texture;

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == vertices.Length )
        {
            Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = region.X + origin.X;
        var worldOriginY = region.Y + origin.Y;
        var fx           = -origin.X;
        var fy           = -origin.Y;
        var fx2          = region.Width - origin.X;
        var fy2          = region.Height - origin.Y;

        // scale
        if ( ( Math.Abs( scale.X - 1 ) > 0 ) || ( Math.Abs( scale.Y - 1 ) > 0 ) )
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

        vertices[ Idx ]     = x1;
        vertices[ Idx + 1 ] = y1;
        vertices[ Idx + 2 ] = PackedColor;
        vertices[ Idx + 3 ] = u;
        vertices[ Idx + 4 ] = v;

        vertices[ Idx + 5 ] = x2;
        vertices[ Idx + 6 ] = y2;
        vertices[ Idx + 7 ] = PackedColor;
        vertices[ Idx + 8 ] = u;
        vertices[ Idx + 9 ] = v2;

        vertices[ Idx + 10 ] = x3;
        vertices[ Idx + 11 ] = y3;
        vertices[ Idx + 12 ] = PackedColor;
        vertices[ Idx + 13 ] = u2;
        vertices[ Idx + 14 ] = v2;

        vertices[ Idx + 15 ] = x4;
        vertices[ Idx + 16 ] = y4;
        vertices[ Idx + 17 ] = PackedColor;
        vertices[ Idx + 18 ] = u2;
        vertices[ Idx + 19 ] = v;

        Idx += 20;
    }

    public virtual void Draw( TextureRegion? textureRegion,
                              GRect region,
                              Point2D origin,
                              Point2D scale,
                              float rotation,
                              bool clockwise )
    {
        Validate( textureRegion );

        var vertices = this.Vertices;
        var texture  = textureRegion?.Texture;

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == vertices.Length )
        {
            Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = region.X + origin.X;
        var worldOriginY = region.Y + origin.Y;
        var fx           = -origin.X;
        var fy           = -origin.Y;
        var fx2          = region.Width - origin.X;
        var fy2          = region.Height - origin.Y;

        // scale
        if ( ( Math.Abs( scale.X - 1 ) > 0 ) || ( Math.Abs( scale.Y - 1 ) > 0 ) )
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
            u1 = textureRegion!.U2;
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
            u1 = textureRegion!.U;
            v1 = textureRegion.V;
            u2 = textureRegion.U2;
            v2 = textureRegion.V;
            u3 = textureRegion.U2;
            v3 = textureRegion.V2;
            u4 = textureRegion.U;
            v4 = textureRegion.V2;
        }

        vertices[ Idx ]     = x1;
        vertices[ Idx + 1 ] = y1;
        vertices[ Idx + 2 ] = PackedColor;
        vertices[ Idx + 3 ] = u1;
        vertices[ Idx + 4 ] = v1;

        vertices[ Idx + 5 ] = x2;
        vertices[ Idx + 6 ] = y2;
        vertices[ Idx + 7 ] = PackedColor;
        vertices[ Idx + 8 ] = u2;
        vertices[ Idx + 9 ] = v2;

        vertices[ Idx + 10 ] = x3;
        vertices[ Idx + 11 ] = y3;
        vertices[ Idx + 12 ] = PackedColor;
        vertices[ Idx + 13 ] = u3;
        vertices[ Idx + 14 ] = v3;

        vertices[ Idx + 15 ] = x4;
        vertices[ Idx + 16 ] = y4;
        vertices[ Idx + 17 ] = PackedColor;
        vertices[ Idx + 18 ] = u4;
        vertices[ Idx + 19 ] = v4;

        Idx += 20;
    }

    public virtual void Draw( TextureRegion? region, float width, float height, Affine2 transform )
    {
        Validate( region );

        var vertices = this.Vertices;
        
        if ( region?.Texture != LastTexture )
        {
            SwitchTexture( region?.Texture );
        }
        else if ( Idx == vertices.Length )
        {
            Flush();
        }

        // construct corner points
        var x1 = transform.M02;
        var y1 = transform.M12;
        var x2 = ( transform.M01 * height ) + transform.M02;
        var y2 = ( transform.M11 * height ) + transform.M12;
        var x3 = ( transform.M00 * width ) + ( transform.M01 * height ) + transform.M02;
        var y3 = ( transform.M10 * width ) + ( transform.M11 * height ) + transform.M12;
        var x4 = ( transform.M00 * width ) + transform.M02;
        var y4 = ( transform.M10 * width ) + transform.M12;

        var u  = region!.U;
        var v  = region.V2;
        var u2 = region.U2;
        var v2 = region.V;

        vertices[ Idx ]     = x1;
        vertices[ Idx + 1 ] = y1;
        vertices[ Idx + 2 ] = PackedColor;
        vertices[ Idx + 3 ] = u;
        vertices[ Idx + 4 ] = v;

        vertices[ Idx + 5 ] = x2;
        vertices[ Idx + 6 ] = y2;
        vertices[ Idx + 7 ] = PackedColor;
        vertices[ Idx + 8 ] = u;
        vertices[ Idx + 9 ] = v2;

        vertices[ Idx + 10 ] = x3;
        vertices[ Idx + 11 ] = y3;
        vertices[ Idx + 12 ] = PackedColor;
        vertices[ Idx + 13 ] = u2;
        vertices[ Idx + 14 ] = v2;

        vertices[ Idx + 15 ] = x4;
        vertices[ Idx + 16 ] = y4;
        vertices[ Idx + 17 ] = PackedColor;
        vertices[ Idx + 18 ] = u2;
        vertices[ Idx + 19 ] = v;

        Idx += 20;
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

        if ( LastTexture == null )
        {
            _nullTextureCount++;
            
            Logger.Error( $"Attempt to flush with null texture. " +
                          $"This batch will be skipped. " +
                          $"Null texture count: {_nullTextureCount}. " +
                          $"Last successful texture: {_lastSuccessfulTexture?.ToString() ?? "None"}" );

            Idx = 0;
            return;
        }

        LastTexture?.Bind();

        if ( _mesh == null )
        {
            Logger.Error( "Mesh is NULL" );

            Idx = 0;
            return;
        }

        _mesh?.SetVertices( Vertices, 0, Idx );
        _mesh!.IndicesBuffer.Position = 0;
        _mesh!.IndicesBuffer.Limit    = count;

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
        _mesh?.Dispose();

        if ( _ownsShader && ( _shader != null ) )
        {
            _shader.Dispose();
        }

        _mesh         = null;
        _shader       = null;
        _customShader = null;
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
            throw new GdxRuntimeException( "Error compiling shader: " + shader.ShaderLog );
        }

        return shader;
    }

    protected void SetupMatrices()
    {
        _combinedMatrix.Set( ProjectionMatrix ).Mul( TransformMatrix );

        if ( _customShader != null )
        {
            if ( !_customShader.IsCompiled )
            {
                Logger.Debug( "Custom Shader is not compiled." );
            }

            _customShader.SetUniformMatrix( "u_projTrans", _combinedMatrix );
            _customShader.SetUniformi( "u_texture", 0 );
        }
        else
        {
            if ( _shader is { IsCompiled: false } )
            {
                Logger.Debug( "Custom Shader is not compiled." );
            }

            _shader?.SetUniformMatrix( "u_projTrans", _combinedMatrix );
            _shader?.SetUniformi( "u_texture", 0 );
        }
    }

    protected void SwitchTexture( Texture? texture )
    {
        if ( texture == null )
        {
            return;
        }

        Flush();

        LastTexture            = texture;
        _lastSuccessfulTexture = texture;
        InvTexWidth            = 1.0f / texture.Width;
        InvTexHeight           = 1.0f / texture.Height;
    }

    // ------------------------------------------------------------------------
    
    private bool IsBlendingEnabled => !BlendingDisabled;

    private void Validate( Texture? texture )
    {
        ArgumentNullException.ThrowIfNull( texture );

        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "Begin() must be called before Draw()." );
        }
    }

    private void Validate( TextureRegion? region )
    {
        ArgumentNullException.ThrowIfNull( region );

        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "Begin() must be called before Draw()." );
        }
    }
}