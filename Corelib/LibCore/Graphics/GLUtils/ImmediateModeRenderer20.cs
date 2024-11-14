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

using Corelib.LibCore.Utils.Exceptions;
using Matrix4 = Corelib.LibCore.Maths.Matrix4;

namespace Corelib.LibCore.Graphics.GLUtils;

/// <summary>
/// Immediate mode rendering class for GLES 2.0. The renderer will allow you to
/// specify vertices on the fly and provides a default shader for (unlit) rendering.
/// </summary>
[PublicAPI]
public class ImmediateModeRenderer20 : IImmediateModeRenderer
{
    public int MaxVertices { get; set; }
    public int NumVertices { get; set; }

    // ========================================================================
    // ========================================================================

    private readonly int      _colorOffset;
    private readonly Mesh     _mesh;
    private readonly int      _normalOffset;
    private readonly int      _numTexCoords;
    private readonly Matrix4  _projModelView = new();
    private readonly string[] _shaderUniformNames;
    private readonly int      _texCoordOffset;
    private readonly int      _vertexSize;
    private readonly float[]  _vertices;

    private int            _numSetTexCoords;
    private bool           _ownsShader;
    private int            _primitiveType;
    private ShaderProgram? _shader;
    private int            _vertexIdx;

    // ========================================================================
    // ========================================================================

    public ImmediateModeRenderer20( bool hasNormals, bool hasColors, int numTexCoords )
        : this( 5000, hasNormals, hasColors, numTexCoords, CreateDefaultShader( hasNormals, hasColors, numTexCoords ) )
    {
        _ownsShader = true;
    }

    public ImmediateModeRenderer20( int maxVertices, bool hasNormals, bool hasColors, int numTexCoords )
        : this(
               maxVertices,
               hasNormals,
               hasColors,
               numTexCoords,
               CreateDefaultShader( hasNormals, hasColors, numTexCoords )
              )
    {
        _ownsShader = true;
    }

    public ImmediateModeRenderer20( int maxVertices, bool hasNormals, bool hasColors, int numTexCoords, ShaderProgram shader )
    {
        MaxVertices   = maxVertices;
        _numTexCoords = numTexCoords;
        _shader       = shader;

        VertexAttribute[] attribs = BuildVertexAttributes( hasNormals, hasColors, numTexCoords );
        _mesh = new Mesh( false, maxVertices, 0, attribs );

        _vertices   = new float[ maxVertices * ( _mesh.VertexAttributes.VertexSize / 4 ) ];
        _vertexSize = _mesh.VertexAttributes.VertexSize / 4;

        var attribute = _mesh.GetVertexAttribute( VertexAttributes.Usage.NORMAL );

        _normalOffset = attribute != null ? attribute.Offset / 4 : 0;

        attribute = _mesh.GetVertexAttribute( VertexAttributes.Usage.COLOR_PACKED );

        _colorOffset = attribute != null ? attribute.Offset / 4 : 0;

        attribute = _mesh.GetVertexAttribute( VertexAttributes.Usage.TEXTURE_COORDINATES );

        _texCoordOffset = attribute != null ? attribute.Offset / 4 : 0;

        _shaderUniformNames = new string[ numTexCoords ];

        for ( var i = 0; i < numTexCoords; i++ )
        {
            _shaderUniformNames[ i ] = "u_sampler" + i;
        }
    }

    public ShaderProgram? Shader
    {
        get => _shader;
        set
        {
            if ( _ownsShader )
            {
                _shader?.Dispose();
            }

            _shader     = value;
            _ownsShader = false;
        }
    }

    public void Begin( Matrix4 projModelView, int primitiveType )
    {
        _projModelView.Set( projModelView );
        _primitiveType = primitiveType;
    }

    public void End()
    {
        Flush();
    }

    public void SetColor( Color color )
    {
        _vertices[ _vertexIdx + _colorOffset ] = color.ToFloatBitsABGR();
    }

    public void SetColor( float r, float g, float b, float a )
    {
        _vertices[ _vertexIdx + _colorOffset ] = Color.ToFloatBitsABGR( r, g, b, a );
    }

    public void SetColor( float colorBits )
    {
        _vertices[ _vertexIdx + _colorOffset ] = colorBits;
    }

    public void TexCoord( float u, float v )
    {
        var idx = _vertexIdx + _texCoordOffset;

        _vertices[ idx + _numSetTexCoords ]     =  u;
        _vertices[ idx + _numSetTexCoords + 1 ] =  v;
        _numSetTexCoords                        += 2;
    }

    public void Normal( float x, float y, float z )
    {
        var idx = _vertexIdx + _normalOffset;

        _vertices[ idx ]     = x;
        _vertices[ idx + 1 ] = y;
        _vertices[ idx + 2 ] = z;
    }

    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void Vertex( float x, float y, float z )
    {
        _vertices[ _vertexIdx ]     = x;
        _vertices[ _vertexIdx + 1 ] = y;
        _vertices[ _vertexIdx + 2 ] = z;

        _numSetTexCoords =  0;
        _vertexIdx       += _vertexSize;
        NumVertices++;
    }

    /// <summary>
    /// </summary>
    public void Flush()
    {
        if ( NumVertices == 0 )
        {
            return;
        }

        _shader?.Bind();
        _shader?.SetUniformMatrix( "u_projModelView", _projModelView );

        for ( var i = 0; i < _numTexCoords; i++ )
        {
            _shader?.SetUniformi( _shaderUniformNames[ i ], i );
        }

        _mesh.SetVertices( _vertices, 0, _vertexIdx );
        _mesh.Render( _shader!, _primitiveType );

        _numSetTexCoords = 0;
        _vertexIdx       = 0;
        NumVertices      = 0;
    }

    private VertexAttribute[] BuildVertexAttributes( bool hasNormals, bool hasColor, int numTexCoords )
    {
        var attribs = new List< VertexAttribute >
        {
            new( VertexAttributes.Usage.POSITION, 3, ShaderProgram.POSITION_ATTRIBUTE ),
        };

        if ( hasNormals )
        {
            attribs.Add( new VertexAttribute( VertexAttributes.Usage.NORMAL,
                                              3,
                                              ShaderProgram.NORMAL_ATTRIBUTE ) );
        }

        if ( hasColor )
        {
            attribs.Add( new VertexAttribute( VertexAttributes.Usage.COLOR_PACKED,
                                              4,
                                              ShaderProgram.COLOR_ATTRIBUTE ) );
        }

        for ( var i = 0; i < numTexCoords; i++ )
        {
            attribs.Add( new VertexAttribute( VertexAttributes.Usage.TEXTURE_COORDINATES,
                                              2,
                                              ShaderProgram.TEXCOORD_ATTRIBUTE + i ) );
        }

        var array = new VertexAttribute[ attribs.Count ];

        for ( var i = 0; i < attribs.Count; i++ )
        {
            array[ i ] = attribs[ i ];
        }

        return array;
    }

    /// <summary>
    /// </summary>
    /// <param name="hasNormals"></param>
    /// <param name="hasColors"></param>
    /// <param name="numTexCoords"></param>
    /// <returns></returns>
    private static string CreateVertexShader( bool hasNormals, bool hasColors, int numTexCoords )
    {
        var shader = "attribute vec4 "
                   + ShaderProgram.POSITION_ATTRIBUTE
                   + ";\n"
                   + ( hasNormals ? "attribute vec3 " + ShaderProgram.NORMAL_ATTRIBUTE + ";\n" : "" )
                   + ( hasColors ? "attribute vec4 " + ShaderProgram.COLOR_ATTRIBUTE + ";\n" : "" );

        for ( var i = 0; i < numTexCoords; i++ )
        {
            shader += "attribute vec2 " + ShaderProgram.TEXCOORD_ATTRIBUTE + i + ";\n";
        }

        shader += "uniform mat4 u_projModelView;\n" //
                + ( hasColors ? "varying vec4 v_col;\n" : "" );

        for ( var i = 0; i < numTexCoords; i++ )
        {
            shader += "varying vec2 v_tex" + i + ";\n";
        }

        shader += "void main() {\n"
                + "   gl_Position = u_projModelView * "
                + ShaderProgram.POSITION_ATTRIBUTE
                + ";\n";

        if ( hasColors )
        {
            shader += "   v_col = "
                    + ShaderProgram.COLOR_ATTRIBUTE
                    + ";\n"
                    + "   v_col.a *= 255.0 / 254.0;\n";
        }

        for ( var i = 0; i < numTexCoords; i++ )
        {
            shader += "   v_tex" + i + " = " + ShaderProgram.TEXCOORD_ATTRIBUTE + i + ";\n";
        }

        shader += "   gl_PointSize = 1.0;\n" + "}\n";

        return shader;
    }

    /// <summary>
    /// </summary>
    /// <param name="hasColors"></param>
    /// <param name="numTexCoords"></param>
    /// <returns></returns>
    private static string CreateFragmentShader( bool hasColors, int numTexCoords )
    {
        var shader = "#ifdef GL_ES\n" + "precision mediump float;\n" + "#endif\n";

        if ( hasColors )
        {
            shader += "varying vec4 v_col;\n";
        }

        for ( var i = 0; i < numTexCoords; i++ )
        {
            shader += "varying vec2 v_tex" + i + ";\n";
            shader += "uniform sampler2D u_sampler" + i + ";\n";
        }

        shader += "void main() {\n   gl_FragColor = " + ( hasColors ? "v_col" : "vec4(1, 1, 1, 1)" );

        if ( numTexCoords > 0 )
        {
            shader += " * ";
        }

        for ( var i = 0; i < numTexCoords; i++ )
        {
            if ( i == ( numTexCoords - 1 ) )
            {
                shader += " texture2D(u_sampler" + i + ",  v_tex" + i + ")";
            }
            else
            {
                shader += " texture2D(u_sampler" + i + ",  v_tex" + i + ") *";
            }
        }

        shader += ";\n}";

        return shader;
    }

    /// <summary>
    /// Returns a new instance of the default shader used by SpriteBatch
    /// when no shader is specified.
    /// </summary>
    public static ShaderProgram CreateDefaultShader( bool hasNormals, bool hasColors, int numTexCoords )
    {
        var vertexShader   = CreateVertexShader( hasNormals, hasColors, numTexCoords );
        var fragmentShader = CreateFragmentShader( hasColors, numTexCoords );
        var program        = new ShaderProgram( vertexShader, fragmentShader );

        if ( !program.IsCompiled )
        {
            throw new GdxRuntimeException( "Error compiling shader: " + program.ShaderLog );
        }

        return program;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if ( _ownsShader && ( _shader != null ) )
        {
            _shader.Dispose();
        }

        _mesh.Dispose();
    }
}
