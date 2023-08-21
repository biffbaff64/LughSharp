// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Maths;
using LibGDXSharp.Utils;

namespace LibGDXSharp.G2D;

/** A PolygonSpriteBatch is used to Draw 2D polygons that reference a texture (region). The class will batch the drawing commands
 * and optimize them for processing by the GPU.
 * <p>
 * To Draw something with a PolygonSpriteBatch one has to first call the {@link PolygonSpriteBatch#begin()} method which will
 * setup appropriate render states. When you are done with drawing you have to call {@link PolygonSpriteBatch#end()} which will
 * actually Draw the things you specified.
 * </p>
 * <p>
 * All drawing commands of the PolygonSpriteBatch operate in screen coordinates. The screen coordinate system has an x-axis
 * pointing to the right, an y-axis pointing upwards and the origin is in the lower left corner of the screen. You can also
 * provide your own transformation and projection matrices if you so wish.
 * </p>
 * <p>
 * A PolygonSpriteBatch is managed. In case the OpenGL context is lost all OpenGL resources a PolygonSpriteBatch uses internally
 * get invalidated. A context is lost when a user switches to another application or receives an incoming call on Android. A
 * SpritPolygonSpriteBatcheBatch will be automatically reloaded after the OpenGL context is restored.
 * </p>
 * <p>
 * A PolygonSpriteBatch is a pretty heavy object so you should only ever have one in your program.
 * </p>
 * <p>
 * A PolygonSpriteBatch works with OpenGL ES 1.x and 2.0. In the case of a 2.0 context it will use its own custom shader to Draw
 * all provided sprites. You can set your own custom shader via {@link #setShader(ShaderProgram)}.
 * </p>
 * <p>
 * A PolygonSpriteBatch has to be disposed if it is no longer used.
 * </p>
 */
public sealed class PolygonSpriteBatch : IPolygonBatch
{
    private readonly Mesh           _mesh;
    private readonly float[]        _vertices;
    private readonly short[]        _triangles;
    private readonly Matrix4        _combinedMatrix = new Matrix4();
    private readonly ShaderProgram? _shader;
    private readonly bool           _ownsShader;
    private readonly Color          _color = new Color( 1, 1, 1, 1 );

    private Texture?       _lastTexture;
    private ShaderProgram? _customShader;
    private float          _invTexWidth  = 0;
    private float          _invTexHeight = 0;
    private float          _colorPacked  = Color.WhiteFloatBits;
    private bool           _blendingDisabled;
    private int            _vertexIndex;
    private int            _triangleIndex;

    /** Number of render calls since the last {@link #begin()}. **/
    public int renderCalls = 0;
    /** Number of rendering calls, ever. Will not be reset unless set manually. **/
    public int totalRenderCalls = 0;
    /** The maximum number of triangles rendered in one batch so far. **/
    public int maxTrianglesInBatch = 0;

    /** Constructs a PolygonSpriteBatch with the default shader, size vertices, and size * 2 triangles.
     * @param size The max number of vertices and number of triangles in a single batch. Max of 32767.
     * @see #PolygonSpriteBatch(int, int, ShaderProgram) */
    public PolygonSpriteBatch( int size )
        : this( size, size * 2, null )
    {
    }

    /** Constructs a PolygonSpriteBatch with the specified shader, size vertices and size * 2 triangles.
     * @param size The max number of vertices and number of triangles in a single batch. Max of 32767.
     * @see #PolygonSpriteBatch(int, int, ShaderProgram) */
    public PolygonSpriteBatch( int size, ShaderProgram? defaultShader = null )
        : this( size, size * 2, defaultShader )
    {
    }

    /** Constructs a new PolygonSpriteBatch. Sets the projection matrix to an orthographic projection with y-axis point upwards,
     * x-axis point to the right and the origin being in the bottom left corner of the screen. The projection will be pixel perfect
     * with respect to the current screen resolution.
     * <para>
     * The defaultShader specifies the shader to use. Note that the names for uniforms for this default shader are different than
     * the ones expect for shaders set with {@link #setShader(ShaderProgram)}. See {@link SpriteBatch#createDefaultShader()}.
     * </para>
     * @param maxVertices The max number of vertices in a single batch. Max of 32767.
     * @param maxTriangles The max number of triangles in a single batch.
     * @param defaultShader The default shader to use. This is not owned by the PolygonSpriteBatch and must be disposed separately.
     *           May be null to use the default shader. */
    public PolygonSpriteBatch( int maxVertices, int maxTriangles, ShaderProgram? defaultShader )
    {
        // 32767 is max vertex index.
        if ( maxVertices > 32767 )
        {
            throw new ArgumentException( "Can't have more than 32767 vertices per batch: " + maxVertices );
        }

        var vertexDataType = Mesh.VertexDataType.VertexArray;

        if ( Gdx.Graphics.IsGL30Available() )
        {
            vertexDataType = Mesh.VertexDataType.VertexBufferObjectWithVAO;
        }

        _mesh = new Mesh
            (
            vertexDataType, false, maxVertices, maxTriangles * 3,
            new VertexAttribute( VertexAttributes.Usage.POSITION, 2, ShaderProgram.POSITION_ATTRIBUTE ),
            new VertexAttribute( VertexAttributes.Usage.COLOR_PACKED, 4, ShaderProgram.COLOR_ATTRIBUTE ),
            new VertexAttribute( VertexAttributes.Usage.TEXTURE_COORDINATES, 2, ShaderProgram.TEXCOORD_ATTRIBUTE + "0" )
            );

        _vertices  = new float[ maxVertices * Sprite.VertexSize ];
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
            throw new IllegalStateException( "PolygonSpriteBatch.end must be called before begin." );
        }

        renderCalls = 0;

        Gdx.GL.GLDepthMask( false );

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
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before end." );
        }

        if ( _vertexIndex > 0 )
        {
            Flush();
        }

        _lastTexture = null;
        IsDrawing    = false;

        Gdx.GL.GLDepthMask( true );

        if ( ISBlendingEnabled() )
        {
            Gdx.GL.GLDisable( IGL20.GL_BLEND );
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
        get => _colorPacked;
        set
        {
            Color.Abgr8888ToColor( _color, value );
            _colorPacked = value;
        }
    }

    public void Draw( PolygonRegion region, float x, float y )
    {
        if ( !IsDrawing )
        {
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var     triangles             = this._triangles;
        short[] regionTriangles       = region.Triangles;
        var     regionTrianglesLength = regionTriangles.Length;
        float[] regionVertices        = region.Vertices;
        var     regionVerticesLength  = regionVertices.Length;

        Texture texture = region.Region.Texture;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + regionTrianglesLength ) > triangles.Length )
               || ( ( _vertexIndex + ( ( regionVerticesLength * Sprite.VertexSize ) / 2 ) ) > _vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var vertexIndex   = this._vertexIndex;
        var startVertex   = vertexIndex / Sprite.VertexSize;

        for ( var i = 0; i < regionTrianglesLength; i++ )
        {
            triangles[ triangleIndex++ ] = ( short )( regionTriangles[ i ] + startVertex );
        }

        this._triangleIndex = triangleIndex;

        var     vertices      = this._vertices;
        var     color         = this._colorPacked;
        float[] textureCoords = region.TextureCoords;

        for ( var i = 0; i < regionVerticesLength; i += 2 )
        {
            vertices[ vertexIndex++ ] = regionVertices[ i ] + x;
            vertices[ vertexIndex++ ] = regionVertices[ i + 1 ] + y;
            vertices[ vertexIndex++ ] = color;
            vertices[ vertexIndex++ ] = textureCoords[ i ];
            vertices[ vertexIndex++ ] = textureCoords[ i + 1 ];
        }

        this._vertexIndex = vertexIndex;
    }

    public void Draw( PolygonRegion region, float x, float y, float width, float height )
    {
        if ( !IsDrawing )
        {
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var           triangles             = this._triangles;
        var           regionTriangles       = region.Triangles;
        var           regionTrianglesLength = regionTriangles.Length;
        var           regionVertices        = region.Vertices;
        var           regionVerticesLength  = regionVertices.Length;
        TextureRegion textureRegion         = region.Region;

        Texture texture = textureRegion.Texture;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + regionTrianglesLength ) > triangles.Length )
               || ( ( _vertexIndex + ( ( regionVerticesLength * Sprite.VertexSize ) / 2 ) ) > _vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var vertexIndex   = this._vertexIndex;
        var startVertex   = vertexIndex / Sprite.VertexSize;

        for ( int i = 0, n = regionTriangles.Length; i < n; i++ )
        {
            triangles[ triangleIndex++ ] = ( short )( regionTriangles[ i ] + startVertex );
        }

        this._triangleIndex = triangleIndex;

        var     vertices      = this._vertices;
        var     color         = this._colorPacked;
        float[] textureCoords = region.TextureCoords;
        var     sX            = width / textureRegion.RegionWidth;
        var     sY            = height / textureRegion.RegionHeight;

        for ( var i = 0; i < regionVerticesLength; i += 2 )
        {
            vertices[ vertexIndex++ ] = ( regionVertices[ i ] * sX ) + x;
            vertices[ vertexIndex++ ] = ( regionVertices[ i + 1 ] * sY ) + y;
            vertices[ vertexIndex++ ] = color;
            vertices[ vertexIndex++ ] = textureCoords[ i ];
            vertices[ vertexIndex++ ] = textureCoords[ i + 1 ];
        }

        this._vertexIndex = vertexIndex;
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
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var           triangles             = this._triangles;
        short[]       regionTriangles       = region.Triangles;
        var           regionTrianglesLength = regionTriangles.Length;
        float[]       regionVertices        = region.Vertices;
        var           regionVerticesLength  = regionVertices.Length;
        TextureRegion textureRegion         = region.Region;

        Texture texture = textureRegion.Texture;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + regionTrianglesLength ) > triangles.Length )
               || ( ( _vertexIndex + ( ( regionVerticesLength * Sprite.VertexSize ) / 2 ) ) > _vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var vertexIndex   = this._vertexIndex;
        var startVertex   = vertexIndex / Sprite.VertexSize;

        for ( var i = 0; i < regionTrianglesLength; i++ )
        {
            triangles[ triangleIndex++ ] = ( short )( regionTriangles[ i ] + startVertex );
        }

        this._triangleIndex = triangleIndex;

        var vertices      = this._vertices;
        var color         = this._colorPacked;
        var textureCoords = region.TextureCoords;

        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var sX           = width / textureRegion.RegionWidth;
        var sY           = height / textureRegion.RegionHeight;
        var cos          = MathUtils.CosDeg( rotation );
        var sin          = MathUtils.SinDeg( rotation );

        for ( var i = 0; i < regionVerticesLength; i += 2 )
        {
            var fx = ( ( regionVertices[ i ] * sX ) - originX ) * scaleX;
            var fy = ( ( regionVertices[ i + 1 ] * sY ) - originY ) * scaleY;

            vertices[ vertexIndex++ ] = ( ( cos * fx ) - ( sin * fy ) ) + worldOriginX;
            vertices[ vertexIndex++ ] = ( sin * fx ) + ( cos * fy ) + worldOriginY;
            vertices[ vertexIndex++ ] = color;
            vertices[ vertexIndex++ ] = textureCoords[ i ];
            vertices[ vertexIndex++ ] = textureCoords[ i + 1 ];
        }

        this._vertexIndex = vertexIndex;
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
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + trianglesCount ) > triangles.Length )
               || ( ( _vertexIndex + verticesCount ) > vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var vertexIndex   = this._vertexIndex;
        var startVertex   = vertexIndex / Sprite.VertexSize;

        for ( int i = trianglesOffset, n = i + trianglesCount; i < n; i++ )
        {
            triangles[ triangleIndex++ ] = ( short )( polygonTriangles[ i ] + startVertex );
        }

        this._triangleIndex = triangleIndex;

        Array.Copy( polygonVertices, verticesOffset, vertices, vertexIndex, verticesCount );
        this._vertexIndex += verticesCount;
    }

    public void Draw( Texture texture,
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
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length )
               || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( !scaleX.Equals( 1 ) || !scaleY.Equals( 1 ) )
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

        var u  = srcX * _invTexWidth;
        var v  = ( srcY + srcHeight ) * _invTexHeight;
        var u2 = ( srcX + srcWidth ) * _invTexWidth;
        var v2 = srcY * _invTexHeight;

        if ( flipX )
        {
            ( u, u2 ) = ( u2, u );
        }

        if ( flipY )
        {
            ( v, v2 ) = ( v2, v );
        }

        var color = this._colorPacked;
        var idx   = this._vertexIndex;
        vertices[ idx++ ] = x1;
        vertices[ idx++ ] = y1;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v;

        vertices[ idx++ ] = x2;
        vertices[ idx++ ] = y2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = x3;
        vertices[ idx++ ] = y3;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = x4;
        vertices[ idx++ ] = y4;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v;
        this._vertexIndex = idx;
    }

    public void Draw( Texture texture,
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
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length )
               || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        var u   = srcX * _invTexWidth;
        var v   = ( srcY + srcHeight ) * _invTexHeight;
        var u2  = ( srcX + srcWidth ) * _invTexWidth;
        var v2  = srcY * _invTexHeight;
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

        var color = this._colorPacked;
        var idx   = this._vertexIndex;
        vertices[ idx++ ] = x;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v;

        vertices[ idx++ ] = x;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v;
        this._vertexIndex = idx;
    }

    public void Draw( Texture texture, float x, float y, int srcX, int srcY, int srcWidth, int srcHeight )
    {
        if ( !IsDrawing )
        {
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length )
               || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        var u   = srcX * _invTexWidth;
        var v   = ( srcY + srcHeight ) * _invTexHeight;
        var u2  = ( srcX + srcWidth ) * _invTexWidth;
        var v2  = srcY * _invTexHeight;
        var fx2 = x + srcWidth;
        var fy2 = y + srcHeight;

        var color = this._colorPacked;
        var idx   = this._vertexIndex;
        vertices[ idx++ ] = x;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v;

        vertices[ idx++ ] = x;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v;
        this._vertexIndex = idx;
    }

    public void Draw( Texture texture, float x, float y, float width, float height, float u, float v, float u2, float v2 )
    {
        if ( !IsDrawing )
        {
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length )
               || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        var fx2 = x + width;
        var fy2 = y + height;

        var color = this._colorPacked;
        var idx   = this._vertexIndex;
        vertices[ idx++ ] = x;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v;

        vertices[ idx++ ] = x;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v;
        this._vertexIndex = idx;
    }

    public void Draw( Texture texture, float x, float y )
    {
        Draw( texture, x, y, texture.Width, texture.Height );
    }

    public void Draw( Texture texture, float x, float y, float width, float height )
    {
        if ( !IsDrawing )
        {
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length )
               || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        var   fx2 = x + width;
        var   fy2 = y + height;
        float u   = 0;
        float v   = 1;
        float u2  = 1;
        float v2  = 0;

        var color = this._colorPacked;
        var idx   = this._vertexIndex;
        vertices[ idx++ ] = x;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v;

        vertices[ idx++ ] = x;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v;
        this._vertexIndex = idx;
    }

    public void Draw( Texture texture, float[] spriteVertices, int offset, int count )
    {
        if ( !IsDrawing )
        {
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        var triangleCount = ( count / Sprite.SpriteSize ) * 6;
        int batch;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
            batch = Math.Min( Math.Min( count, vertices.Length - ( vertices.Length % Sprite.SpriteSize ) ), ( triangles.Length / 6 ) * Sprite.SpriteSize );
            triangleCount = ( batch / Sprite.SpriteSize ) * 6;
        }
        else if ( ( ( _triangleIndex + triangleCount ) > triangles.Length )
               || ( ( _vertexIndex + count ) > vertices.Length ) )
        {
            Flush();
            batch = Math.Min( Math.Min( count, vertices.Length - ( vertices.Length % Sprite.SpriteSize ) ), ( triangles.Length / 6 ) * Sprite.SpriteSize );
            triangleCount = ( batch / Sprite.SpriteSize ) * 6;
        }
        else
        {
            batch = count;
        }

        var vertexIndex   = this._vertexIndex;
        var vertex        = ( short )( vertexIndex / Sprite.VertexSize );
        var triangleIndex = this._triangleIndex;

        for ( var n = triangleIndex + triangleCount; triangleIndex < n; triangleIndex += 6, vertex += 4 )
        {
            triangles[ triangleIndex ]     = vertex;
            triangles[ triangleIndex + 1 ] = ( short )( vertex + 1 );
            triangles[ triangleIndex + 2 ] = ( short )( vertex + 2 );
            triangles[ triangleIndex + 3 ] = ( short )( vertex + 2 );
            triangles[ triangleIndex + 4 ] = ( short )( vertex + 3 );
            triangles[ triangleIndex + 5 ] = vertex;
        }

        while ( true )
        {
            Array.Copy( spriteVertices, offset, vertices, vertexIndex, batch );
            this._vertexIndex   =  vertexIndex + batch;
            this._triangleIndex =  triangleIndex;
            count               -= batch;

            if ( count == 0 )
            {
                break;
            }

            offset += batch;
            Flush();
            vertexIndex = 0;

            if ( batch > count )
            {
                batch         = Math.Min( count, ( triangles.Length / 6 ) * Sprite.SpriteSize );
                triangleIndex = ( batch / Sprite.SpriteSize ) * 6;
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
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        Texture texture = region.Texture;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length ) || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) ) //
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        var fx2 = x + width;
        var fy2 = y + height;
        var u   = region.U;
        var v   = region.V2;
        var u2  = region.U2;
        var v2  = region.V;

        var color = this._colorPacked;
        var idx   = this._vertexIndex;
        vertices[ idx++ ] = x;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v;

        vertices[ idx++ ] = x;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = fy2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = fx2;
        vertices[ idx++ ] = y;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v;
        this._vertexIndex = idx;
    }

    public void Draw( TextureRegion region,
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
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        Texture texture = region.Texture;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length ) || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) ) //
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( !scaleX.Equals( 1 ) || !scaleY.Equals( 1 ) )
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

        var color = this._colorPacked;
        var idx   = this._vertexIndex;
        vertices[ idx++ ] = x1;
        vertices[ idx++ ] = y1;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v;

        vertices[ idx++ ] = x2;
        vertices[ idx++ ] = y2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = x3;
        vertices[ idx++ ] = y3;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = x4;
        vertices[ idx++ ] = y4;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v;
        this._vertexIndex = idx;
    }

    public void Draw( TextureRegion region,
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
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        Texture texture = region.Texture;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length )
               || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( !scaleX.Equals( 1 ) || !scaleY.Equals( 1 ) )
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

        var color = this._colorPacked;
        var idx   = this._vertexIndex;

        vertices[ idx++ ] = x1;
        vertices[ idx++ ] = y1;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u1;
        vertices[ idx++ ] = v1;

        vertices[ idx++ ] = x2;
        vertices[ idx++ ] = y2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = x3;
        vertices[ idx++ ] = y3;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u3;
        vertices[ idx++ ] = v3;

        vertices[ idx++ ] = x4;
        vertices[ idx++ ] = y4;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u4;
        vertices[ idx++ ] = v4;
        this._vertexIndex = idx;
    }

    public void Draw( TextureRegion region, float width, float height, Affine2 transform )
    {
        if ( !IsDrawing )
        {
            throw new IllegalStateException( "PolygonSpriteBatch.begin must be called before Draw." );
        }

        var triangles = this._triangles;
        var vertices  = this._vertices;

        Texture texture = region.Texture;

        if ( texture != _lastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( ( ( _triangleIndex + 6 ) > triangles.Length )
               || ( ( _vertexIndex + Sprite.SpriteSize ) > vertices.Length ) )
        {
            Flush();
        }

        var triangleIndex = this._triangleIndex;
        var startVertex   = _vertexIndex / Sprite.VertexSize;
        triangles[ triangleIndex++ ] = ( short )startVertex;
        triangles[ triangleIndex++ ] = ( short )( startVertex + 1 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 2 );
        triangles[ triangleIndex++ ] = ( short )( startVertex + 3 );
        triangles[ triangleIndex++ ] = ( short )startVertex;
        this._triangleIndex          = triangleIndex;

        // construct corner points
        var x1 = transform.m02;
        var y1 = transform.m12;
        var x2 = ( transform.m01 * height ) + transform.m02;
        var y2 = ( transform.m11 * height ) + transform.m12;
        var x3 = ( transform.m00 * width ) + ( transform.m01 * height ) + transform.m02;
        var y3 = ( transform.m10 * width ) + ( transform.m11 * height ) + transform.m12;
        var x4 = ( transform.m00 * width ) + transform.m02;
        var y4 = ( transform.m10 * width ) + transform.m12;

        var u  = region.U;
        var v  = region.V2;
        var u2 = region.U2;
        var v2 = region.V;

        var color = this._colorPacked;
        var idx   = _vertexIndex;
        vertices[ idx++ ] = x1;
        vertices[ idx++ ] = y1;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v;

        vertices[ idx++ ] = x2;
        vertices[ idx++ ] = y2;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = x3;
        vertices[ idx++ ] = y3;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v2;

        vertices[ idx++ ] = x4;
        vertices[ idx++ ] = y4;
        vertices[ idx++ ] = color;
        vertices[ idx++ ] = u2;
        vertices[ idx++ ] = v;
        _vertexIndex      = idx;
    }

    public void Flush()
    {
        if ( _vertexIndex == 0 )
        {
            return;
        }

        renderCalls++;
        totalRenderCalls++;
        var trianglesInBatch = _triangleIndex;

        if ( trianglesInBatch > maxTrianglesInBatch )
        {
            maxTrianglesInBatch = trianglesInBatch;
        }

        _lastTexture?.Bind();
        Mesh mesh = this._mesh;
        mesh.SetVertices( _vertices, 0, _vertexIndex );
        mesh.SetIndices( _triangles, 0, trianglesInBatch );

        if ( _blendingDisabled )
        {
            Gdx.GL.GLDisable( IGL20.GL_BLEND );
        }
        else
        {
            Gdx.GL.GLEnable( IGL20.GL_BLEND );

            if ( BlendSrcFunc != -1 )
            {
                Gdx.GL.GLBlendFuncSeparate( BlendSrcFunc, BlendDstFunc, BlendSrcFuncAlpha, BlendDstFuncAlpha );
            }
        }

        mesh.Render( _customShader ?? _shader, IGL20.GL_TRIANGLES, 0, trianglesInBatch );

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
                    this._shader?.Bind();
                }

                SetupMatrices();
            }
        }
    }

    public bool ISBlendingEnabled()
    {
        return !_blendingDisabled;
    }

    public int BlendSrcFunc { get; private set; } = IGL20.GL_SRC_ALPHA;

    public int BlendDstFunc { get; private set; } = IGL20.GL_ONE_MINUS_SRC_ALPHA;

    public int BlendSrcFuncAlpha { get; private set; } = IGL20.GL_SRC_ALPHA;

    public int BlendDstFuncAlpha { get; private set; } = IGL20.GL_ONE_MINUS_SRC_ALPHA;

    public Matrix4 ProjectionMatrix { get; set; } = new Matrix4();

    public Matrix4 TransformMatrix { get; set; } = new Matrix4();

    public bool IsDrawing { get; set; }
}
