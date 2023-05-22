using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Maths;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class ShapeRenderer : IDisposable
{
    public enum ShapeType
    {
        Point  = IGL20.GL_Points,
        Line   = IGL20.GL_Lines,
        Filled = IGL20.GL_Triangles
    }

    private IImmediateModeRenderer renderer;
    private bool                   matrixDirty      = false;
    private Matrix4                projectionMatrix = new Matrix4();
    private Matrix4                transformMatrix  = new Matrix4();
    private Matrix4                combinedMatrix   = new Matrix4();
    private Vector2                tmp              = new Vector2();
    private Color                  color            = new Color( 1, 1, 1, 1 );
    private ShapeType?             shapeType;
    private bool                   autoShapeType;
    private float                  defaultRectLineWidth = 0.75f;

    public ShapeRenderer()
        : this( 5000 )
    {
    }

    public ShapeRenderer( int maxVertices, ShaderProgram? defaultShader = null )
    {
        renderer = defaultShader == null
            ? new ImmediateModeRenderer20( maxVertices, false, true, 0 )
            : new ImmediateModeRenderer20( maxVertices, false, true, 0, defaultShader );

        projectionMatrix.SetToOrtho2D( 0, 0, Gdx.Graphics.Width, Gdx.Graphics.Height );
        matrixDirty = true;
    }

    public Color Color
    {
        get => color;
        set => color.Set( value );
    }

    /// <summary>
    /// Sets the color to be used by the next shapes drawn.
    /// </summary>
    public void SetColor( float r, float g, float b, float a )
    {
        this.color.Set( r, g, b, a );
    }

    public void UpdateMatrices()
    {
        matrixDirty = true;
    }

    public Matrix4 ProjectionMatrix
    {
        get => projectionMatrix;
        set
        {
            projectionMatrix = value;
            matrixDirty      = true;
        }
    }

    public Matrix4 TransformMatrix
    {
        get => transformMatrix;
        set
        {
            transformMatrix = value;
            matrixDirty     = true;
        }
    }

    /// <summary>
    /// Sets the transformation matrix to identity.
    /// </summary>
    public void Identity()
    {
        TransformMatrix.Idt();
        matrixDirty = true;
    }

    /// <summary>
    /// Multiplies the current transformation matrix by a translation matrix.
    /// </summary>
    public void Translate( float x, float y, float z )
    {
        TransformMatrix.Translate( x, y, z );
        matrixDirty = true;
    }

    /// <summary>
    /// Multiplies the current transformation matrix by a rotation matrix.
    /// </summary>
    public void Rotate( float axisX, float axisY, float axisZ, float degrees )
    {
        TransformMatrix.Rotate( axisX, axisY, axisZ, degrees );
        matrixDirty = true;
    }

    /// <summary>
    /// Multiplies the current transformation matrix by a scale matrix.
    /// </summary>
    public void Scale( float scaleX, float scaleY, float scaleZ )
    {
        TransformMatrix.Scale( scaleX, scaleY, scaleZ );
        matrixDirty = true;
    }

    /// <summary>
    /// If true, when drawing a shape cannot be performed with the current shape
    /// type, the batch is flushed and the shape type is changed automatically.
    /// This can increase the number of batch flushes if care is not taken to draw
    /// the same type of shapes together. Default is false.
    /// </summary>
    public void SetAutoShapeType( bool type )
    {
        this.autoShapeType = type;
    }

    /// <summary>
    /// Begins a new batch without specifying a shape type.
    /// </summary>
    /// <exception cref="IllegalStateException"> if <see cref="autoShapeType"/> is false.</exception>
    public void Begin()
    {
        if ( !autoShapeType )
        {
            throw new System.InvalidOperationException( "autoShapeType must be true to use this method." );
        }

        Begin( ShapeType.Line );
    }

    /// <summary>
    /// Starts a new batch of shapes. Shapes drawn within the batch will attempt
    /// to use the type specified. The call to this method must be paired with a
    /// call to <see cref="End()"/>.
    /// </summary>
    /// <seealso cref="SetAutoShapeType(bool) "/>
    public void Begin( ShapeType type )
    {
        if ( shapeType != null )
        {
            throw new System.InvalidOperationException( "Call end() before beginning a new shape batch." );
        }

        shapeType = type;

        if ( matrixDirty )
        {
            combinedMatrix.Set( projectionMatrix );
            Matrix4.Mul( combinedMatrix.val, transformMatrix.val );
            matrixDirty = false;
        }

        renderer.Begin( combinedMatrix, shapeType );
    }

    public void Set( ShapeType type )
    {
        if ( shapeType == type )
        {
            return;
        }

        if ( shapeType == null )
        {
            throw new System.InvalidOperationException( "begin must be called first." );
        }

        if ( !autoShapeType )
        {
            throw new System.InvalidOperationException( "autoShapeType must be enabled." );
        }

        End();

        Begin( type );
    }

    /// <summary>
    /// Draws a point using <see cref="ShapeType.Point"/>, <see cref="ShapeType.Line"/>
    /// or <see cref="ShapeType.Filled"/>.
    /// </summary>
    public void Point( float x, float y, float z )
    {
        if ( shapeType == ShapeType.Line )
        {
            var size = defaultRectLineWidth * 0.5f;

            Line( x - size, y - size, z, x + size, y + size, z );

            return;
        }
        else if ( shapeType == ShapeType.Filled )
        {
            var size = defaultRectLineWidth * 0.5f;

            Box
                (
                 x - size,
                 y - size,
                 z - size,
                 defaultRectLineWidth,
                 defaultRectLineWidth,
                 defaultRectLineWidth
                );

            return;
        }

        Check( ShapeType.Point, null, 1 );

        renderer.Color = color;
        renderer.Vertex( x, y, z );
    }

    /// <summary>
    /// Draws a line using <seealso cref="ShapeType.Line"/> or <seealso cref="ShapeType.Filled"/>.
    /// </summary>
    public void Line( float x, float y, float z, float x2, float y2, float z2 )
    {
        Line( x, y, z, x2, y2, z2, color, color );
    }

    public void Line( Vector3 v0, Vector3 v1 )
    {
        Line( v0.X, v0.Y, v0.Z, v1.X, v1.Y, v1.Z, color, color );
    }

    public void Line( float x, float y, float x2, float y2 )
    {
        Line( x, y, 0.0f, x2, y2, 0.0f, color, color );
    }

    public void Line( Vector2 v0, Vector2 v1 )
    {
        Line( v0.X, v0.Y, 0.0f, v1.X, v1.Y, 0.0f, color, color );
    }

    public void Line( float x, float y, float x2, float y2, Color c1, Color c2 )
    {
        Line( x, y, 0.0f, x2, y2, 0.0f, c1, c2 );
    }

    /// <summary>
    /// Draws a line using <see cref="ShapeType.Line"/> or <see cref="ShapeType.Filled"/>.
    /// The line is drawn with two colors interpolated between the start and end points.
    /// </summary>
    public void Line( float x, float y, float z, float x2, float y2, float z2, Color c1, Color c2 )
    {
        if ( shapeType == ShapeType.Filled )
        {
            RectLine( x, y, x2, y2, defaultRectLineWidth, c1, c2 );

            return;
        }

        Check( ShapeType.Line, null, 2 );

        renderer.SetColor( c1.R, c1.G, c1.B, c1.A );
        renderer.Vertex( x, y, z );
        renderer.SetColor( c2.R, c2.G, c2.B, c2.A );
        renderer.Vertex( x2, y2, z2 );
    }

    /** Draws a curve using {@link ShapeType#Line}. */
    public void Curve( float x1,
                       float y1,
                       float cx1,
                       float cy1,
                       float cx2,
                       float cy2,
                       float x2,
                       float y2,
                       int segments )
    {
        Check( ShapeType.Line, null, ( segments * 2 ) + 2 );

        var colorBits = color.ToFloatBits();

        // Algorithm from: http://www.antigrain.com/research/bezier_interpolation/index.html#PAGE_BEZIER_INTERPOLATION
        var subdivStep  = 1f / segments;
        var subdivStep2 = subdivStep * subdivStep;
        var subdivStep3 = subdivStep * subdivStep * subdivStep;

        var pre1 = 3 * subdivStep;
        var pre2 = 3 * subdivStep2;
        var pre4 = 6 * subdivStep2;
        var pre5 = 6 * subdivStep3;

        var tmp1X = ( x1 - ( cx1 * 2 ) ) + cx2;
        var tmp1Y = ( y1 - ( cy1 * 2 ) ) + cy2;

        var tmp2X = ( ( ( cx1 - cx2 ) * 3 ) - x1 ) + x2;
        var tmp2Y = ( ( ( cy1 - cy2 ) * 3 ) - y1 ) + y2;

        var fx = x1;
        var fy = y1;

        var dfx = ( ( cx1 - x1 ) * pre1 ) + ( tmp1X * pre2 ) + ( tmp2X * subdivStep3 );
        var dfy = ( ( cy1 - y1 ) * pre1 ) + ( tmp1Y * pre2 ) + ( tmp2Y * subdivStep3 );

        var ddfx = ( tmp1X * pre4 ) + ( tmp2X * pre5 );
        var ddfy = ( tmp1Y * pre4 ) + ( tmp2Y * pre5 );

        var dddfx = tmp2X * pre5;
        var dddfy = tmp2Y * pre5;

        while ( segments-- > 0 )
        {
            renderer.Color = colorBits;
            renderer.Vertex( fx, fy, 0 );

            fx   += dfx;
            fy   += dfy;
            dfx  += ddfx;
            dfy  += ddfy;
            ddfx += dddfx;
            ddfy += dddfy;

            renderer.Color = colorBits;
            renderer.Vertex( fx, fy, 0 );
        }

        renderer.Color = colorBits;
        renderer.Vertex( fx, fy, 0 );
        renderer.Color = colorBits;
        renderer.Vertex( x2, y2, 0 );
    }

    /// <summary>
    /// Draws a triangle in x/y plane using <seealso cref="ShapeType.Line"/>
    /// or <seealso cref="ShapeType.Filled"/>.
    /// </summary>
    public void Triangle( float x1, float y1, float x2, float y2, float x3, float y3 )
    {
        Check( ShapeType.Line, ShapeType.Filled, 6 );

        var colorBits = color.ToFloatBits();

        if ( shapeType == ShapeType.Line )
        {
            renderer.Color = colorBits;
            renderer.Vertex( x1, y1, 0 );
            renderer.Color = colorBits;
            renderer.Vertex( x2, y2, 0 );

            renderer.Color = colorBits;
            renderer.Vertex( x2, y2, 0 );
            renderer.Color = colorBits;
            renderer.Vertex( x3, y3, 0 );

            renderer.Color = colorBits;
            renderer.Vertex( x3, y3, 0 );
            renderer.Color = colorBits;
            renderer.Vertex( x1, y1, 0 );
        }
        else
        {
            renderer.Color = colorBits;
            renderer.Vertex( x1, y1, 0 );
            renderer.Color = colorBits;
            renderer.Vertex( x2, y2, 0 );
            renderer.Color = colorBits;
            renderer.Vertex( x3, y3, 0 );
        }
    }

    /// <summary>
    /// Draws a triangle in x/y plane with colored corners using <seealso cref="ShapeType.Line"/> or <seealso cref="ShapeType.Filled"/>. </summary>
    public virtual void Triangle( float x1,
                                  float y1,
                                  float x2,
                                  float y2,
                                  float x3,
                                  float y3,
                                  Color col1,
                                  Color col2,
                                  Color col3 )
    {
        check( ShapeType.Line, ShapeType.Filled, 6 );

        if ( shapeType == ShapeType.Line )
        {
            renderer.color( col1.r, col1.g, col1.b, col1.a );
            renderer.vertex( x1, y1, 0 );
            renderer.color( col2.r, col2.g, col2.b, col2.a );
            renderer.vertex( x2, y2, 0 );

            renderer.color( col2.r, col2.g, col2.b, col2.a );
            renderer.vertex( x2, y2, 0 );
            renderer.color( col3.r, col3.g, col3.b, col3.a );
            renderer.vertex( x3, y3, 0 );

            renderer.color( col3.r, col3.g, col3.b, col3.a );
            renderer.vertex( x3, y3, 0 );
            renderer.color( col1.r, col1.g, col1.b, col1.a );
            renderer.vertex( x1, y1, 0 );
        }
        else
        {
            renderer.color( col1.r, col1.g, col1.b, col1.a );
            renderer.vertex( x1, y1, 0 );
            renderer.color( col2.r, col2.g, col2.b, col2.a );
            renderer.vertex( x2, y2, 0 );
            renderer.color( col3.r, col3.g, col3.b, col3.a );
            renderer.vertex( x3, y3, 0 );
        }
    }
}