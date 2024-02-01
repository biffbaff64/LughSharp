// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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

using System.Text;

using LibGDXSharp.Gdx.Core;
using LibGDXSharp.Gdx.Utils;
using LibGDXSharp.Gdx.Utils.Buffers;
using LibGDXSharp.Gdx.Utils.Collections.Extensions;

using Buffer = LibGDXSharp.Gdx.Utils.Buffers.Buffer;
using Matrix3 = LibGDXSharp.Gdx.Maths.Matrix3;
using Matrix4 = LibGDXSharp.Gdx.Maths.Matrix4;

namespace LibGDXSharp.Gdx.Graphics.GLUtils;

/// <summary>
///     <para>
///         A shader program encapsulates a vertex and fragment shader pair
///         linked to form a shader program.
///     </para>
///     <para>
///         After construction a ShaderProgram can be used to draw <see cref="Mesh" />.
///         To make the GPU use a specific ShaderProgram the programs <see cref="Bind()" />
///         method must be used which effectively binds the program.
///     </para>
///     <para>
///         When a ShaderProgram is bound one can set uniforms, vertex attributes and
///         attributes as needed via the respective methods.
///     </para>
///     <para>
///         A ShaderProgram must be disposed via a call to <see cref="Dispose()" />
///         when it is no longer needed
///     </para>
///     <para>
///         ShaderPrograms are managed. In case the OpenGL context is lost all shaders
///         get invalidated and have to be reloaded. Managed ShaderPrograms are
///         automatically reloaded when the OpenGL context is recreated so you don't
///         have to do this manually.
///     </para>
/// </summary>
public class ShaderProgram
{
    private const int CACHED_NOT_FOUND = -1;
    private const int NOT_CACHED       = -2;

    /// <summary>
    ///     flag indicating whether attributes & uniforms must be present
    ///     at all times.
    /// </summary>
    public readonly static bool Pedantic = true;

    /// <summary>
    ///     code that is always added to the vertex shader code, typically used to
    ///     inject a #version line. Note that this is added as-is, you should include
    ///     a newline (`\n`) if needed.
    /// </summary>
    public readonly static string PrependVertexCode = "";

    /// <summary>
    ///     code that is always added to every fragment shader code, typically used
    ///     to inject a #version line. Note that this is added as-is, you should
    ///     include a newline (`\n`) if needed.
    /// </summary>
    public readonly static string PrependFragmentCode = "";

    /// <summary>
    ///     the list of currently available shaders
    /// </summary>
    private readonly static Dictionary< IApplication, List< ShaderProgram > > Shaders = new();

    /// <summary>
    ///     attribute lookup
    /// </summary>
    private readonly Dictionary< string, int > _attributes = new();
    private readonly Dictionary< string, int > _attributeSizes = new();
    private readonly Dictionary< string, int > _attributeTypes = new();

    private readonly FloatBuffer _matrix;

    private readonly IntBuffer _parameters = BufferUtils.NewIntBuffer( 1 );
    private readonly IntBuffer _progType   = BufferUtils.NewIntBuffer( 1 );

    /// <summary>
    ///     uniform lookup
    /// </summary>
    private readonly Dictionary< string, int > _uniforms = new();
    private readonly Dictionary< string, int > _uniformSizes = new();
    private readonly Dictionary< string, int > _uniformTypes = new();
    private          int                       _fragmentShaderHandle;
    private          bool                      _invalidated;

    /// <summary>
    ///     the log
    /// </summary>
    private string _log = "";

    private int _vertexShaderHandle;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="vertexShader"></param>
    /// <param name="fragmentShader"></param>
    /// <exception cref="ArgumentException"></exception>
    public ShaderProgram( string vertexShader, string fragmentShader )
    {
        ArgumentNullException.ThrowIfNull( vertexShader );
        ArgumentNullException.ThrowIfNull( fragmentShader );

        if ( !string.IsNullOrEmpty( PrependVertexCode ) )
        {
            vertexShader = PrependVertexCode + vertexShader;
        }

        if ( !string.IsNullOrEmpty( PrependFragmentCode ) )
        {
            fragmentShader = PrependFragmentCode + fragmentShader;
        }

        VertexShaderSource   = vertexShader;
        FragmentShaderSource = fragmentShader;
        _matrix              = BufferUtils.NewFloatBuffer( 16 );

        CompileShaders( vertexShader, fragmentShader );

        if ( IsCompiled )
        {
            FetchAttributes();
            FetchUniforms();

            AddManagedShader( Core.Gdx.App, this );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="vertexShader"></param>
    /// <param name="fragmentShader"></param>
    public ShaderProgram( FileInfo vertexShader, FileInfo fragmentShader )
        : this( File.ReadAllText( vertexShader.Name ), File.ReadAllText( fragmentShader.Name ) )
    {
    }

//    internal readonly static IntBuffer Intbuf = BufferUtils.NewIntBuffer( 1 );

    /// <returns>
    ///     the log info for the shader compilation and program linking stage.
    ///     The shader needs to be bound for this method to have an effect.
    /// </returns>
    public string Log
    {
        get
        {
            if ( IsCompiled )
            {
                // Gdx.gl20.glGetProgramiv(program, IGL20.GL_INFO_LOG_LENGTH, intbuf);
                // int infoLogLength = intbuf.get(0);
                // if (infoLogLength > 1) {
                _log = Core.Gdx.GL20.GLGetProgramInfoLog( Handle );

                // }
            }

            return _log;
        }
    }

    /// <summary>
    ///     Loads and compiles the shaders, creates a new program and links the shaders.
    /// </summary>
    /// <param name="vertexShader"> </param>
    /// <param name="fragmentShader">  </param>
    private void CompileShaders( string vertexShader, string fragmentShader )
    {
        _vertexShaderHandle   = LoadShader( IGL20.GL_VERTEX_SHADER, vertexShader );
        _fragmentShaderHandle = LoadShader( IGL20.GL_FRAGMENT_SHADER, fragmentShader );

        if ( ( _vertexShaderHandle == -1 ) || ( _fragmentShaderHandle == -1 ) )
        {
            IsCompiled = false;

            return;
        }

        Handle = LinkProgram( CreateProgram() );

        if ( Handle == -1 )
        {
            IsCompiled = false;

            return;
        }

        IsCompiled = true;
    }

    private int LoadShader( int type, string source )
    {
        IntBuffer intbuf = BufferUtils.NewIntBuffer( 1 );

        var shader = Core.Gdx.GL20.GLCreateShader( type );

        if ( shader == 0 )
        {
            return -1;
        }

        Core.Gdx.GL20.GLShaderSource( shader, source );
        Core.Gdx.GL20.GLCompileShader( shader );
        Core.Gdx.GL20.GLGetShaderiv( shader, IGL20.GL_COMPILE_STATUS, intbuf );

        var compiled = intbuf.Get( 0 );

        if ( compiled == 0 )
        {
// gl.glGetShaderiv(shader, IGL20.GL_INFO_LOG_LENGTH, intbuf);
// int infoLogLength = intbuf.get(0);
// if (infoLogLength > 1) {
            var infoLog = Core.Gdx.GL20.GLGetShaderInfoLog( shader );
            _log += type == IGL20.GL_VERTEX_SHADER ? "Vertex shader\n" : "Fragment shader:\n";
            _log += infoLog;

// }
            return -1;
        }

        return shader;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    protected static int CreateProgram()
    {
        var program = Core.Gdx.GL20.GLCreateProgram();

        return program != 0 ? program : -1;
    }

    private int LinkProgram( int program )
    {
        if ( program == -1 )
        {
            return -1;
        }

        Core.Gdx.GL20.GLAttachShader( program, _vertexShaderHandle );
        Core.Gdx.GL20.GLAttachShader( program, _fragmentShaderHandle );
        Core.Gdx.GL20.GLLinkProgram( program );

        ByteBuffer tmp = ByteBuffer.Allocate( 4 );
        tmp.Order( ByteOrder.NativeOrder );
        IntBuffer intbuf = tmp.AsIntBuffer();

        Core.Gdx.GL20.GLGetProgramiv( program, IGL20.GL_LINK_STATUS, intbuf );

        var linked = intbuf.Get( 0 );

        if ( linked == 0 )
        {
// Gdx.gl20.glGetProgramiv(program, IGL20.GL_INFO_LOG_LENGTH, intbuf);
// int infoLogLength = intbuf.get(0);
// if (infoLogLength > 1) {
            _log = Core.Gdx.GL20.GLGetProgramInfoLog( program );

// }
            return -1;
        }

        return program;
    }

    private int FetchAttributeLocation( string name )
    {
        // -2 == not yet cached
        // -1 == cached but not found
        int location;

        if ( ( location = _attributes.Get( name, NOT_CACHED ) ) == NOT_CACHED )
        {
            location            = Core.Gdx.GL20.GLGetAttribLocation( Handle, name );
            _attributes[ name ] = location;
        }

        return location;
    }

    private int FetchUniformLocation( string name ) => FetchUniformLocation( name, Pedantic );

    public int FetchUniformLocation( string name, bool pedant )
    {
        // -2 == not yet cached
        // -1 == cached but not found
        int location;

        if ( ( location = _uniforms.Get( name, NOT_CACHED ) ) == NOT_CACHED )
        {
            location = Core.Gdx.GL20.GLGetUniformLocation( Handle, name );

            if ( ( location == CACHED_NOT_FOUND ) && pedant )
            {
                if ( IsCompiled )
                {
                    throw new ArgumentException( "No uniform with name '" + name + "' in shader" );
                }

                throw new InvalidOperationException( "An attempted fetch uniform from uncompiled shader \n" + Log );
            }

            _uniforms[ name ] = location;
        }

        return location;
    }

    /// <summary>
    ///     Sets the uniform with the given name. The <see cref="ShaderProgram" />
    ///     must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value"> the value  </param>
    public void SetUniformi( string name, int value )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform1I( FetchUniformLocation( name ), value );
    }

    public void SetUniformi( int location, int value )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform1I( location, value );
    }

    /// <summary>
    ///     Sets the uniform with the given name. The <see cref="ShaderProgram" />
    ///     must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="count"> the first value </param>
    /// <param name="value"> the second value </param>
    public void SetUniformi( string name, int count, int value )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform2I( FetchUniformLocation( name ), count, value );
    }

    public void SetUniformi( int location, int count, int value )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform2I( location, count, value );
    }

    /// <summary>
    ///     Sets the uniform with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value </param>
    public void SetUniformi( string name, int value1, int value2, int value3 )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform3I( FetchUniformLocation( name ), value1, value2, value3 );
    }

    public void SetUniformi( int location, int x, int y, int z )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform3I( location, x, y, z );
    }

    /// <summary>
    ///     Sets the uniform with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value"> the value  </param>
    public void SetUniformf( string name, float value )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform1F( FetchUniformLocation( name ), value );
    }

    public void SetUniformf( int location, int value )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform1F( location, value );
    }

    /// <summary>
    ///     Sets the uniform with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value  </param>
    public void SetUniformf( string name, float value1, float value2 )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform2F( FetchUniformLocation( name ), value1, value2 );
    }

    public void SetUniformf( int location, int value1, int value2 )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform2F( location, value1, value2 );
    }

    /// <summary>
    ///     Sets the uniform with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value  </param>
    public void SetUniformf( string name, float value1, float value2, float value3 )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform3F( FetchUniformLocation( name ), value1, value2, value3 );
    }

    public void SetUniformf( int location, float value1, float value2, float value3 )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform3F( location, value1, value2, value3 );
    }

    /// <summary>
    ///     Sets the uniform with the given name. The <see cref="ShaderProgram" />
    ///     must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    public void SetUniformf( string name, float x, float y, float z, float w )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform4F( FetchUniformLocation( name ), x, y, z, w );
    }

    public void SetUniformf( int location, float x, float y, float z, float w )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform4F( location, x, y, z, w );
    }

    public void SetUniform1Fv( string name, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform1Fv( FetchUniformLocation( name ), length, values, offset );
    }

    public void SetUniform1Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform1Fv( location, length, values, offset );
    }

    public void SetUniform2Fv( string name, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform2Fv( FetchUniformLocation( name ), length / 2, values, offset );
    }

    public void SetUniform2Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform2Fv( location, length / 2, values, offset );
    }

    public void SetUniform3Fv( string name, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform3Fv( FetchUniformLocation( name ), length / 3, values, offset );
    }

    public void SetUniform3Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform3Fv( location, length / 3, values, offset );
    }

    public void SetUniform4Fv( string name, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform4Fv( FetchUniformLocation( name ), length / 4, values, offset );
    }

    public void SetUniform4Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform4Fv( location, length / 4, values, offset );
    }

    /// <summary>
    ///     Sets the uniform matrix with the given name. The <see cref="ShaderProgram" />
    ///     must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix  </param>
    public void SetUniformMatrix( string name, Matrix4 matrix ) => SetUniformMatrix( name, matrix, false );

    /// <summary>
    ///     Sets the uniform matrix with the given name. The <see cref="ShaderProgram" />
    ///     must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix </param>
    /// <param name="transpose"> whether the matrix should be transposed  </param>
    public void SetUniformMatrix( string name, Matrix4 matrix, bool transpose ) => SetUniformMatrix( FetchUniformLocation( name ), matrix, transpose );

    public void SetUniformMatrix( int location, Matrix4 matrix ) => SetUniformMatrix( location, matrix, false );

    public void SetUniformMatrix( int location, Matrix4 matrix, bool transpose )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniformMatrix4Fv( location, 1, transpose, matrix.val, 0 );
    }

    /// <summary>
    ///     Sets the uniform matrix with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix  </param>
    public void SetUniformMatrix( string name, Matrix3 matrix ) => SetUniformMatrix( name, matrix, false );

    /// <summary>
    ///     Sets the uniform matrix with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix </param>
    /// <param name="transpose"> whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix( string name, Matrix3 matrix, bool transpose ) => SetUniformMatrix( FetchUniformLocation( name ), matrix, transpose );

    public void SetUniformMatrix( int location, Matrix3 matrix ) => SetUniformMatrix( location, matrix, false );

    public void SetUniformMatrix( int location, Matrix3 matrix, bool transpose )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniformMatrix3Fv( location, 1, transpose, matrix.val, 0 );
    }

    /// <summary>
    ///     Sets an array of uniform matrices with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name">the name of the uniform </param>
    /// <param name="buffer">buffer containing the matrix data </param>
    /// <param name="count"></param>
    /// <param name="transpose">whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix3Fv( string name, FloatBuffer buffer, int count, bool transpose )
    {
        CheckManaged();
        buffer.Position = 0;
        Core.Gdx.GL20.GLUniformMatrix3Fv( FetchUniformLocation( name ), count, transpose, buffer );
    }

    /// <summary>
    ///     Sets an array of uniform matrices with the given name. The <see cref="ShaderProgram" /> must be bound for this to
    ///     work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="buffer"> buffer containing the matrix data </param>
    /// <param name="count"></param>
    /// <param name="transpose"> whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix4Fv( string name, FloatBuffer buffer, int count, bool transpose )
    {
        CheckManaged();
        buffer.Position = 0;
        Core.Gdx.GL20.GLUniformMatrix4Fv( FetchUniformLocation( name ), count, transpose, buffer );
    }

    public void SetUniformMatrix4Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniformMatrix4Fv( location, length / 16, false, values, offset );
    }

    public void SetUniformMatrix4Fv( string name, float[] values, int offset, int length )
        => SetUniformMatrix4Fv( FetchUniformLocation( name ), values, offset, length );

    /// <summary>
    ///     Sets the uniform with the given name. The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> x and y as the first and second values respectively  </param>
    public void SetUniformf( string name, Vector2 values )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform2F( FetchUniformLocation( name ), values.X, values.Y );
    }

    public void SetUniformf( int location, Vector2 values )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform2F( location, values.X, values.Y );
    }

    /// <summary>
    ///     Sets the uniform with the given name. The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> x, y and z as the first, second and third values respectively  </param>
    public void SetUniformf( string name, Vector3 values )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform3F( FetchUniformLocation( name ), values.X, values.Y, values.Z );
    }

    public void SetUniformf( int location, Vector3 values )
    {
        CheckManaged();
        Core.Gdx.GL20.GLUniform3F( location, values.X, values.Y, values.Z );
    }

    /// <summary>
    ///     Sets the uniform with the given name. The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> r, g, b and a as the first through fourth values respectively  </param>
    public void SetUniformf( string name, Color values ) => Core.Gdx.GL20.GLUniform4F( FetchUniformLocation( name ), values.R, values.G, values.B, values.A );

    public void SetUniformf( int location, Color values ) => Core.Gdx.GL20.GLUniform4F( location, values.R, values.G, values.B, values.A );

    /// <summary>
    ///     Sets the vertex attribute with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="size">
    ///     The number of components, must be >= 1 and &lt;= 4.
    /// </param>
    /// <param name="type">
    ///     The type, must be one of IGL20.GL_Byte, IGL20.GL_Unsigned_Byte, IGL20.GL_Short,
    ///     IGL20.GL_Unsigned_Short, IGL20.GL_Fixed, or IGL20.GL_Float.
    ///     <para>GL_F will not work on the desktop.</para>
    /// </param>
    /// <param name="normalize">
    ///     Whether fixed point data should be normalized. Will not work on the desktop.
    /// </param>
    /// <param name="stride">The stride in bytes between successive attributes.</param>
    /// <param name="buffer">The buffer containing the vertex attributes.</param>
    public void SetVertexAttribute( string name, int size, int type, bool normalize, int stride, Buffer buffer )
    {
        CheckManaged();

        var location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Core.Gdx.GL20.GLVertexAttribPointer( location, size, type, normalize, stride, buffer );
    }

    public void SetVertexAttribute( int location, int size, int type, bool normalize, int stride, Buffer buffer )
    {
        CheckManaged();
        Core.Gdx.GL20.GLVertexAttribPointer( location, size, type, normalize, stride, buffer );
    }

    /// <summary>
    ///     Sets the vertex attribute with the given name.
    ///     The <see cref="ShaderProgram" /> must be bound for this to work.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="size">The number of components, must be >= 1 and &lt;= 4.</param>
    /// <param name="type">
    ///     The type, must be one of IGL20.GL_Byte, IGL20.GL_Unsigned_Byte, IGL20.GL_Short,
    ///     IGL20.GL_Unsigned_Short, IGL20.GL_Fixed, or IGL20.GL_Float.
    ///     <para>GL_Fixed will not work on the desktop.</para>
    /// </param>
    /// <param name="normalize">
    ///     Whether fixed point data should be normalized. Will not work on the desktop.
    /// </param>
    /// <param name="stride">The stride in bytes between successive attributes.</param>
    /// <param name="offset">
    ///     Byte offset into the vertex buffer object bound to IGL20.GL_Array_Buffer.
    /// </param>
    public void SetVertexAttribute( string name, int size, int type, bool normalize, int stride, int offset )
    {
        CheckManaged();

        var location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Core.Gdx.GL20.GLVertexAttribPointer( location, size, type, normalize, stride, offset );
    }

    public void SetVertexAttribute( int location, int size, int type, bool normalize, int stride, int offset )
    {
        CheckManaged();
        Core.Gdx.GL20.GLVertexAttribPointer( location, size, type, normalize, stride, offset );
    }

    public void Bind()
    {
        CheckManaged();
        Core.Gdx.GL20.GLUseProgram( Handle );
    }

    /// <summary>
    ///     Disposes all resources associated with this shader.
    ///     Must be called when the shader is no longer used.
    /// </summary>
    public void Dispose()
    {
        Core.Gdx.GL20.GLUseProgram( 0 );
        Core.Gdx.GL20.GLDeleteShader( _vertexShaderHandle );
        Core.Gdx.GL20.GLDeleteShader( _fragmentShaderHandle );
        Core.Gdx.GL20.GLDeleteProgram( Handle );

        Shaders.Get( Core.Gdx.App ).Remove( this );
    }

    /// <summary>
    ///     Disables the vertex attribute with the given name
    /// </summary>
    /// <param name="name"> the vertex attribute name  </param>
    public void DisableVertexAttribute( string name )
    {
        CheckManaged();

        var location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Core.Gdx.GL20.GLDisableVertexAttribArray( location );
    }

    public void DisableVertexAttribute( int location )
    {
        CheckManaged();
        Core.Gdx.GL20.GLDisableVertexAttribArray( location );
    }

    /// <summary>
    ///     Enables the vertex attribute with the given name
    /// </summary>
    /// <param name="name"> the vertex attribute name  </param>
    public void EnableVertexAttribute( string name )
    {
        CheckManaged();

        var location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Core.Gdx.GL20.GLEnableVertexAttribArray( location );
    }

    public void EnableVertexAttribute( int location )
    {
        CheckManaged();
        Core.Gdx.GL20.GLEnableVertexAttribArray( location );
    }

    private void CheckManaged()
    {
        if ( _invalidated )
        {
            CompileShaders( VertexShaderSource, FragmentShaderSource );
            _invalidated = false;
        }
    }

    private void AddManagedShader( IApplication app, ShaderProgram shaderProgram )
    {
        List< ShaderProgram > managedResources = Shaders.Get( app );

        managedResources.Add( shaderProgram );
        Shaders.Put( app, managedResources );
    }

    /// <summary>
    ///     Invalidates all shaders so the next time they are used new
    ///     handles are generated.
    /// </summary>
    /// <param name="app">  </param>
    public static void InvalidateAllShaderPrograms( IApplication app )
    {
        List< ShaderProgram > shaderArray = Shaders.Get( app );

        foreach ( ShaderProgram sp in shaderArray )
        {
            sp._invalidated = true;
            sp.CheckManaged();
        }
    }

    public static void ClearAllShaderPrograms( IApplication app ) => Shaders.Remove( app );

    /// <summary>
    ///     Sets the given attribute
    /// </summary>
    /// <param name="name"> the name of the attribute </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value </param>
    /// <param name="value4"> the fourth value  </param>
    public void SetAttributef( string name, float value1, float value2, float value3, float value4 )
        => Core.Gdx.GL20.GLVertexAttrib4F( FetchAttributeLocation( name ), value1, value2, value3, value4 );

    /// <summary>
    /// </summary>
    private void FetchUniforms()
    {
        _parameters.Clear();

        Core.Gdx.GL20.GLGetProgramiv( Handle, IGL20.GL_ACTIVE_UNIFORMS, _parameters );

        var numUniforms = _parameters.Get( 0 );

        Uniforms = new string[ numUniforms ];

        for ( var i = 0; i < numUniforms; i++ )
        {
            _parameters.Clear();
            _parameters.Put( 0, 1 );

            _progType.Clear();

            var name = Core.Gdx.GL20.GLGetActiveUniform( Handle, i, _parameters, _progType );

            var location = Core.Gdx.GL20.GLGetUniformLocation( Handle, name );

            _uniforms[ name ]     = location;
            _uniformTypes[ name ] = _progType.Get( 0 );
            _uniformSizes[ name ] = _parameters.Get( 0 );
            Uniforms[ i ]         = name;
        }
    }

    /// <summary>
    /// </summary>
    private void FetchAttributes()
    {
        _parameters.Clear();

        Core.Gdx.GL20.GLGetProgramiv( Handle, IGL20.GL_ACTIVE_ATTRIBUTES, _parameters );

        var numAttributes = _parameters.Get( 0 );

        Attributes = new string[ numAttributes ];

        for ( var i = 0; i < numAttributes; i++ )
        {
            _parameters.Clear();
            _parameters.Put( 0, 1 );

            _progType.Clear();

            var name     = Core.Gdx.GL20.GLGetActiveAttrib( Handle, i, _parameters, _progType );
            var location = Core.Gdx.GL20.GLGetAttribLocation( Handle, name );

            _attributes[ name ]     = location;
            _attributeTypes[ name ] = _progType.Get( 0 );
            _attributeSizes[ name ] = _parameters.Get( 0 );
            Attributes[ i ]         = name;
        }
    }

    /// <param name="name"> the name of the attribute </param>
    /// <returns> whether the attribute is available in the shader  </returns>
    public bool HasAttribute( string name ) => _attributes.ContainsKey( name );

    /// <param name="name"> the name of the attribute </param>
    /// <returns>
    ///     the type of the attribute, one of <see cref="IGL20.GL_FLOAT" />,
    ///     <see cref="IGL20.GL_FLOAT_VEC2" /> etc.
    /// </returns>
    public int GetAttributeType( string name ) => _attributeTypes.GetValueOrDefault( name, 0 );

    /// <param name="name"> the name of the attribute </param>
    /// <returns> the location of the attribute or -1.  </returns>
    public int GetAttributeLocation( string name ) => _attributes.GetValueOrDefault( name, 0 );

    /// <param name="name"> the name of the attribute </param>
    /// <returns> the size of the attribute or 0.</returns>
    public int GetAttributeSize( string name ) => _attributeSizes.GetValueOrDefault( name, 0 );

    /// <param name="name"> the name of the uniform.</param>
    /// <returns> whether the uniform is available in the shader.</returns>
    public bool HasUniform( string name ) => _uniforms.ContainsKey( name );

    /// <param name="name"> the name of the uniform </param>
    /// <returns>
    ///     the type of the uniform, one of <see cref="IGL20.GL_FLOAT" />,
    ///     <see cref="IGL20.GL_FLOAT_VEC2" /> etc.
    /// </returns>
    public int GetUniformType( string name ) => _uniformTypes.GetValueOrDefault( name, 0 );

    /// <param name="name"> the name of the uniform </param>
    /// <returns> the location of the uniform or -1.</returns>
    public int GetUniformLocation( string name ) => _uniforms.GetValueOrDefault( name, -1 );

    /// <param name="name">The name of the uniform</param>
    /// <returns> the size of the uniform or 0.</returns>
    public int GetUniformSize( string name ) => _uniformSizes.GetValueOrDefault( name, 0 );

    #region default attribute names

    public const string POSITION_ATTRIBUTE   = "a_position";
    public const string NORMAL_ATTRIBUTE     = "a_normal";
    public const string COLOR_ATTRIBUTE      = "a_color";
    public const string TEXCOORD_ATTRIBUTE   = "a_texCoord";
    public const string TANGENT_ATTRIBUTE    = "a_tangent";
    public const string BINORMAL_ATTRIBUTE   = "a_binormal";
    public const string BONEWEIGHT_ATTRIBUTE = "a_boneWeight";

    #endregion default attribute names

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region properties

    public static string ManagedStatus
    {
        get
        {
            var builder = new StringBuilder( "Managed shaders/app: { " );

            foreach ( IApplication app in Shaders.Keys )
            {
                builder.Append( Shaders[ app ].Count );
                builder.Append( ' ' );
            }

            builder.Append( '}' );

            return builder.ToString();
        }
    }

    public bool IsCompiled { get; set; }

    public static int NumManagedShaderPrograms => Shaders[ Core.Gdx.App ].Count;

    public string[] Attributes { get; private set; } = null!;

    public string[] Uniforms { get; private set; } = null!;

    public string VertexShaderSource { get; }

    public string FragmentShaderSource { get; }

    public int Handle { get; private set; }

    #endregion properties
}
