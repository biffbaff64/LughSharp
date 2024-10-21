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

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

using Corelib.LibCore.Core;
using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Buffers;
using Corelib.LibCore.Utils.Collections;
using Buffer = Corelib.LibCore.Utils.Buffers.Buffer;
using Matrix3 = Corelib.LibCore.Maths.Matrix3;
using Matrix4 = Corelib.LibCore.Maths.Matrix4;

namespace Corelib.LibCore.Graphics.GLUtils;

/// <summary>
/// A shader program encapsulates a vertex and fragment shader pairlinked to
/// form a shader program. After construction a ShaderProgram can be used to
/// draw <see cref="Mesh"/>. To make the GPU use a specific ShaderProgram the
/// programs <see cref="Bind()"/> method must be used which effectively binds
/// the program. When a ShaderProgram is bound one can set uniforms, vertex
/// attributes and attributes as needed via the respective methods.
/// <para>
/// A ShaderProgram must be disposed via a call to <see cref="Dispose()"/>
/// when it is no longer needed
/// </para>
/// <para>
/// ShaderPrograms are managed. In case the OpenGL context is lost all shaders
/// get invalidated and have to be reloaded. Managed ShaderPrograms are
/// automatically reloaded when the OpenGL context is recreated so you don't
/// have to do this manually.
/// </para>
/// </summary>
[PublicAPI]
public class ShaderProgram
{
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

    private const int CACHED_NOT_FOUND = -1;
    private const int NOT_CACHED       = -2;

    // ------------------------------------------------------------------------
    
    /// <summary>
    /// flag indicating whether attributes & uniforms must be present at all times.
    /// </summary>
    public static readonly bool Pedantic = true;

    /// <summary>
    /// code that is always added to the vertex shader code, typically used to
    /// inject a #version line. Note that this is added as-is, you should include
    /// a newline (`\n`) if needed.
    /// </summary>
    public static readonly string PrependVertexCode = "";

    /// <summary>
    /// code that is always added to every fragment shader code, typically used
    /// to inject a #version line. Note that this is added as-is, you should
    /// include a newline (`\n`) if needed.
    /// </summary>
    public static readonly string PrependFragmentCode = "";
    
    // ------------------------------------------------------------------------

    public bool     IsCompiled           { get; set; }
    public string[] Attributes           { get; private set; } = null!;
    public string[] Uniforms             { get; private set; } = null!;
    public string   VertexShaderSource   { get; }
    public string   FragmentShaderSource { get; }
    public int      Handle               { get; private set; }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// the list of currently available shaders
    /// </summary>
    private static readonly Dictionary< IApplication, List< ShaderProgram >? > _shaders = new();

    private readonly Dictionary< string, int > _attributes     = new();
    private readonly Dictionary< string, int > _attributeSizes = new();
    private readonly Dictionary< string, int > _attributeTypes = new();
    private readonly Dictionary< string, int > _uniforms       = new();
    private readonly Dictionary< string, int > _uniformSizes   = new();
    private readonly Dictionary< string, int > _uniformTypes   = new();

    private int    _fragmentShaderHandle;
    private bool   _invalidated;
    private string _shaderLog = "";
    private int    _vertexShaderHandle;

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

        CompileShaders( vertexShader, fragmentShader );

        if ( IsCompiled )
        {
            FetchAttributes();
            FetchUniforms();

            AddManagedShader( Gdx.App, this );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="vertexShader"></param>
    /// <param name="fragmentShader"></param>
    public ShaderProgram( FileSystemInfo vertexShader, FileSystemInfo fragmentShader )
        : this( File.ReadAllText( vertexShader.Name ), File.ReadAllText( fragmentShader.Name ) )
    {
    }

    /// <returns>
    /// the log info for the shader compilation and program linking stage.
    /// The shader needs to be bound for this method to have an effect.
    /// </returns>
    public unsafe string ShaderLog
    {
        get
        {
            if ( IsCompiled )
            {
                var length = stackalloc int[ 1 ];

                Gdx.GL.glGetProgramiv( ( uint ) Handle, IGL.GL_INFO_LOG_LENGTH, length );

                _shaderLog = Gdx.GL.glGetProgramInfoLog( ( uint ) Handle, *length );
            }

            return _shaderLog;
        }
    }

    /// <summary>
    /// Loads and compiles the shaders, creates a new program and links the shaders.
    /// </summary>
    /// <param name="vertexShader"> </param>
    /// <param name="fragmentShader">  </param>
    private void CompileShaders( string vertexShader, string fragmentShader )
    {
        _vertexShaderHandle   = LoadShader( IGL.GL_VERTEX_SHADER, vertexShader );
        _fragmentShaderHandle = LoadShader( IGL.GL_FRAGMENT_SHADER, fragmentShader );

        if ( ( _vertexShaderHandle == -1 ) || ( _fragmentShaderHandle == -1 ) )
        {
            IsCompiled = false;

            return;
        }

        Handle = LinkProgram( CreateProgram() );

        IsCompiled = ( Handle != -1 );
    }

    private unsafe int LoadShader( int shaderType, string source )
    {
        var shader = Gdx.GL.glCreateShader( shaderType );

        if ( shader == 0 )
        {
            return -1;
        }

        Gdx.GL.glShaderSource( shader, source );
        Gdx.GL.glCompileShader( shader );

        var status = stackalloc int[ 1 ];

        Gdx.GL.glGetShaderiv( shader, IGL.GL_COMPILE_STATUS, status );

        if ( *status == IGL.GL_FALSE )
        {
            var length = stackalloc int[ 1 ];

            Gdx.GL.glGetShaderiv( shader, IGL.GL_INFO_LOG_LENGTH, length );

            var infoLog = Gdx.GL.glGetShaderInfoLog( shader, *length );

            Gdx.GL.glDeleteShader( shader );

            _shaderLog += shaderType == IGL.GL_VERTEX_SHADER ? "Vertex shader\n" : "Fragment shader:\n";
            _shaderLog += infoLog;

            throw new Exception( $"Failed to loader shader {shader}: {infoLog}" );
        }

        return ( int ) shader;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    protected static int CreateProgram()
    {
        var program = ( int ) Gdx.GL.glCreateProgram();

        return program != 0 ? program : -1;
    }

    private unsafe int LinkProgram( int program )
    {
        if ( program == -1 )
        {
            return -1;
        }

        Gdx.GL.glAttachShader( ( uint ) program, ( uint ) _vertexShaderHandle );
        Gdx.GL.glAttachShader( ( uint ) program, ( uint ) _fragmentShaderHandle );
        Gdx.GL.glLinkProgram( ( uint ) program );

        var status = stackalloc int[ 1 ];

        Gdx.GL.glGetProgramiv( ( uint ) program, IGL.GL_LINK_STATUS, status );

        if ( *status == IGL.GL_FALSE )
        {
            var length = stackalloc int[ 1 ];

            Gdx.GL.glGetProgramiv( ( uint ) program, IGL.GL_INFO_LOG_LENGTH, length );

            _shaderLog = Gdx.GL.glGetProgramInfoLog( ( uint ) program, *length );

            throw new Exception( $"Failed to link shader program {program}: {_shaderLog}" );
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
            location            = Gdx.GL.glGetAttribLocation( ( uint ) Handle, name );
            _attributes[ name ] = location;
        }

        return location;
    }

    private int FetchUniformLocation( string name )
    {
        return FetchUniformLocation( name, Pedantic );
    }

    public int FetchUniformLocation( string name, bool pedant )
    {
        // -2 == not yet cached
        // -1 == cached but not found
        int location;

        if ( ( location = _uniforms.Get( name, NOT_CACHED ) ) == NOT_CACHED )
        {
            location = Gdx.GL.glGetUniformLocation( ( uint ) Handle, name );

            if ( ( location == CACHED_NOT_FOUND ) && pedant )
            {
                if ( IsCompiled )
                {
                    throw new ArgumentException( "No uniform with name '" + name + "' in shader" );
                }

                throw new InvalidOperationException( "An attempted fetch uniform from uncompiled shader \n" + ShaderLog );
            }

            _uniforms[ name ] = location;
        }

        return location;
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value"> the value  </param>
    public void SetUniformi( string name, int value )
    {
        CheckManaged();
        Gdx.GL.glUniform1i( FetchUniformLocation( name ), value );
    }

    public void SetUniformi( int location, int value )
    {
        CheckManaged();
        Gdx.GL.glUniform1i( location, value );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="count"> the first value </param>
    /// <param name="value"> the second value </param>
    public void SetUniformi( string name, int count, int value )
    {
        CheckManaged();
        Gdx.GL.glUniform2i( FetchUniformLocation( name ), count, value );
    }

    public void SetUniformi( int location, int count, int value )
    {
        CheckManaged();
        Gdx.GL.glUniform2i( location, count, value );
    }

    /// <summary>
    /// Sets the uniform with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value </param>
    public void SetUniformi( string name, int value1, int value2, int value3 )
    {
        CheckManaged();
        Gdx.GL.glUniform3i( FetchUniformLocation( name ), value1, value2, value3 );
    }

    public void SetUniformi( int location, int x, int y, int z )
    {
        CheckManaged();
        Gdx.GL.glUniform3i( location, x, y, z );
    }

    /// <summary>
    /// Sets the uniform with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value"> the value  </param>
    public void SetUniformf( string name, float value )
    {
        CheckManaged();
        Gdx.GL.glUniform1f( FetchUniformLocation( name ), value );
    }

    public void SetUniformf( int location, int value )
    {
        CheckManaged();
        Gdx.GL.glUniform1f( location, value );
    }

    /// <summary>
    /// Sets the uniform with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value  </param>
    public void SetUniformf( string name, float value1, float value2 )
    {
        CheckManaged();
        Gdx.GL.glUniform2f( FetchUniformLocation( name ), value1, value2 );
    }

    public void SetUniformf( int location, int value1, int value2 )
    {
        CheckManaged();
        Gdx.GL.glUniform2f( location, value1, value2 );
    }

    /// <summary>
    /// Sets the uniform with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value  </param>
    public void SetUniformf( string name, float value1, float value2, float value3 )
    {
        CheckManaged();
        Gdx.GL.glUniform3f( FetchUniformLocation( name ), value1, value2, value3 );
    }

    public void SetUniformf( int location, float value1, float value2, float value3 )
    {
        CheckManaged();
        Gdx.GL.glUniform3f( location, value1, value2, value3 );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    public void SetUniformf( string name, float x, float y, float z, float w )
    {
        CheckManaged();
        Gdx.GL.glUniform4f( FetchUniformLocation( name ), x, y, z, w );
    }

    public void SetUniformf( int location, float x, float y, float z, float w )
    {
        CheckManaged();
        Gdx.GL.glUniform4f( location, x, y, z, w );
    }

    public void SetUniform1Fv( string name, params float[] value )
    {
        CheckManaged();
        Gdx.GL.glUniform1fv( FetchUniformLocation( name ), value );
    }

    public void SetUniform1Fv( int location, params float[] value )
    {
        CheckManaged();
        Gdx.GL.glUniform1fv( location, value );
    }

    public void SetUniform2Fv( string name, params float[] values )
    {
        CheckManaged();
        Gdx.GL.glUniform2fv( FetchUniformLocation( name ), values );
    }

    public void SetUniform2Fv( int location, params float[] values )
    {
        CheckManaged();
        Gdx.GL.glUniform2fv( location, values );
    }

    public void SetUniform3Fv( string name, params float[] values )
    {
        CheckManaged();
        Gdx.GL.glUniform3fv( FetchUniformLocation( name ), values );
    }

    public void SetUniform3Fv( int location, params float[] values )
    {
        CheckManaged();
        Gdx.GL.glUniform3fv( location, values );
    }

    public void SetUniform4Fv( string name, params float[] values )
    {
        CheckManaged();
        Gdx.GL.glUniform4fv( FetchUniformLocation( name ), values );
    }

    public void SetUniform4Fv( int location, params float[] values )
    {
        CheckManaged();
        Gdx.GL.glUniform4fv( location, values );
    }

    /// <summary>
    /// Sets the uniform matrix with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix  </param>
    public void SetUniformMatrix( string name, Matrix4 matrix )
    {
        SetUniformMatrix( name, matrix, false );
    }

    /// <summary>
    /// Sets the uniform matrix with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix </param>
    /// <param name="transpose"> whether the matrix should be transposed  </param>
    public void SetUniformMatrix( string name, Matrix4 matrix, bool transpose )
    {
        SetUniformMatrix( FetchUniformLocation( name ), matrix, transpose );
    }

    public void SetUniformMatrix( int location, Matrix4 matrix )
    {
        SetUniformMatrix( location, matrix, false );
    }

    public void SetUniformMatrix( int location, Matrix4 matrix, bool transpose )
    {
        CheckManaged();
        Gdx.GL.glUniformMatrix4fv( location, transpose, matrix.Val );
    }

    /// <summary>
    /// Sets the uniform matrix with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix  </param>
    public void SetUniformMatrix( string name, Matrix3 matrix )
    {
        SetUniformMatrix( name, matrix, false );
    }

    /// <summary>
    /// Sets the uniform matrix with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix </param>
    /// <param name="transpose"> whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix( string name, Matrix3 matrix, bool transpose )
    {
        SetUniformMatrix( FetchUniformLocation( name ), matrix, transpose );
    }

    public void SetUniformMatrix( int location, Matrix3 matrix )
    {
        SetUniformMatrix( location, matrix, false );
    }

    public void SetUniformMatrix( int location, Matrix3 matrix, bool transpose )
    {
        CheckManaged();
        Gdx.GL.glUniformMatrix3fv( location, transpose, matrix.Val );
    }

    /// <summary>
    /// Sets an array of uniform matrices with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name">the name of the uniform </param>
    /// <param name="buffer">buffer containing the matrix data </param>
    /// <param name="count"></param>
    /// <param name="transpose">whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix3Fv( string name, FloatBuffer buffer, int count, bool transpose )
    {
        CheckManaged();
        buffer.Position = 0;

        unsafe
        {
            fixed ( float* ptr = &( buffer ).BackingArray()[ 0 ] )
            {
                Gdx.GL.glUniformMatrix3fv( FetchUniformLocation( name ), count, transpose, ptr );
            }
        }
    }

    /// <summary>
    /// Sets an array of uniform matrices with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="buffer"> buffer containing the matrix data </param>
    /// <param name="count"></param>
    /// <param name="transpose"> whether the uniform matrix should be transposed  </param>
    public unsafe void SetUniformMatrix4Fv( string name, FloatBuffer buffer, int count, bool transpose )
    {
        CheckManaged();
        buffer.Position = 0;

        fixed ( float* ptr = &buffer.BackingArray()[ 0 ] )
        {
            Gdx.GL.glUniformMatrix4fv( FetchUniformLocation( name ), count, transpose, ptr );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>
    /// <param name="values"></param>
    public void SetUniformMatrix4Fv( int location, params float[] values )
    {
        CheckManaged();
        Gdx.GL.glUniformMatrix4fv( location, false, values );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="values"></param>
    public void SetUniformMatrix4Fv( string name, params float[] values )
    {
        SetUniformMatrix4Fv( FetchUniformLocation( name ), values );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> x and y as the first and second values respectively  </param>
    public void SetUniformf( string name, Vector2 values )
    {
        CheckManaged();
        Gdx.GL.glUniform2f( FetchUniformLocation( name ), values.X, values.Y );
    }

    public void SetUniformf( int location, Vector2 values )
    {
        CheckManaged();
        Gdx.GL.glUniform2f( location, values.X, values.Y );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> x, y and z as the first, second and third values respectively  </param>
    public void SetUniformf( string name, Vector3 values )
    {
        CheckManaged();
        Gdx.GL.glUniform3f( FetchUniformLocation( name ), values.X, values.Y, values.Z );
    }

    public void SetUniformf( int location, Vector3 values )
    {
        CheckManaged();
        Gdx.GL.glUniform3f( location, values.X, values.Y, values.Z );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> r, g, b and a as the first through fourth values respectively  </param>
    public void SetUniformf( string name, Color values )
    {
        Gdx.GL.glUniform4f( FetchUniformLocation( name ), values.R, values.G, values.B, values.A );
    }

    public void SetUniformf( int location, Color values )
    {
        Gdx.GL.glUniform4f( location, values.R, values.G, values.B, values.A );
    }

    /// <summary>
    /// Sets the vertex attribute with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="size">
    /// The number of components, must be >= 1 and &lt;= 4.
    /// </param>
    /// <param name="type">
    /// The type, must be one of IGL.GL_Byte, IGL.GL_Unsigned_Byte, IGL.GL_Short,
    /// IGL.GL_Unsigned_Short, IGL.GL_Fixed, or IGL.GL_Float.
    /// <para>GL_F will not work on the desktop.</para>
    /// </param>
    /// <param name="normalize">
    /// Whether fixed point data should be normalized. Will not work on the desktop.
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

        unsafe
        {
            fixed ( void* ptr = &buffer.BackingArray()[ 0 ] )
            {
                Gdx.GL.glVertexAttribPointer( ( uint ) location, size, type, normalize, stride, ptr );
            }
        }
    }

    public void SetVertexAttribute( int location, int size, int type, bool normalize, int stride, Buffer buffer )
    {
        CheckManaged();

        unsafe
        {
            fixed ( void* ptr = &buffer.BackingArray()[ 0 ] )
            {
                Gdx.GL.glVertexAttribPointer( ( uint ) location, size, type, normalize, stride, ptr );
            }
        }
    }

    /// <summary>
    /// Sets the vertex attribute with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="size">The number of components, must be >= 1 and &lt;= 4.</param>
    /// <param name="type">
    /// The type, must be one of IGL.GL_Byte, IGL.GL_Unsigned_Byte, IGL.GL_Short,
    /// IGL.GL_Unsigned_Short, IGL.GL_Fixed, or IGL.GL_Float.
    /// <para>GL_Fixed will not work on the desktop.</para>
    /// </param>
    /// <param name="normalize">
    /// Whether fixed point data should be normalized. Will not work on the desktop.
    /// </param>
    /// <param name="stride">The stride in bytes between successive attributes.</param>
    /// <param name="offset">
    /// Byte offset into the vertex buffer object bound to IGL.GL_Array_Buffer.
    /// </param>
    public void SetVertexAttribute( string name, int size, int type, bool normalize, int stride, int offset )
    {
        CheckManaged();

        var location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Gdx.GL.glVertexAttribPointer( ( uint ) location, size, type, normalize, stride, ( uint ) offset );
    }

    public void SetVertexAttribute( int location, int size, int type, bool normalize, int stride, int offset )
    {
        CheckManaged();
        Gdx.GL.glVertexAttribPointer( ( uint ) location, size, type, normalize, stride, ( uint ) offset );
    }

    public void Bind()
    {
        CheckManaged();
        Gdx.GL.glUseProgram( ( uint ) Handle );
    }

    /// <summary>
    /// Disposes all resources associated with this shader.
    /// Must be called when the shader is no longer used.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL.glUseProgram( 0 );
        Gdx.GL.glDeleteShader( ( uint ) _vertexShaderHandle );
        Gdx.GL.glDeleteShader( ( uint ) _fragmentShaderHandle );
        Gdx.GL.glDeleteProgram( ( uint ) Handle );

        _shaders[ Gdx.App ]?.Remove( this );
    }

    /// <summary>
    /// Disables the vertex attribute with the given name
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

        Gdx.GL.glDisableVertexAttribArray( ( uint ) location );
    }

    public void DisableVertexAttribute( int location )
    {
        CheckManaged();
        Gdx.GL.glDisableVertexAttribArray( ( uint ) location );
    }

    /// <summary>
    /// Enables the vertex attribute with the given name
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

        Gdx.GL.glEnableVertexAttribArray( ( uint ) location );
    }

    public void EnableVertexAttribute( int location )
    {
        CheckManaged();
        Gdx.GL.glEnableVertexAttribArray( ( uint ) location );
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
        Logger.Checkpoint();

        List< ShaderProgram >? managedResources;

        if ( !_shaders.ContainsKey( app ) || ( _shaders[ app ] == null ) )
        {
            managedResources = new List< ShaderProgram >();
        }
        else
        {
            managedResources = _shaders[ app ];
        }

        managedResources?.Add( shaderProgram );

        _shaders.Put( app, managedResources );
    }

    /// <summary>
    /// Invalidates all shaders so the next time they are used new
    /// handles are generated.
    /// </summary>
    /// <param name="app">  </param>
    public static void InvalidateAllShaderPrograms( IApplication app )
    {
        List< ShaderProgram >? shaderArray;

        if ( !_shaders.ContainsKey( app ) || ( _shaders[ app ] == null ) )
        {
            shaderArray = new List< ShaderProgram >();
        }
        else
        {
            shaderArray = _shaders[ app ];
        }
        
        foreach ( var sp in shaderArray! )
        {
            sp._invalidated = true;
            sp.CheckManaged();
        }
    }

    public static void ClearAllShaderPrograms( IApplication app )
    {
        _shaders.Remove( app );
    }

    /// <summary>
    /// Sets the given attribute
    /// </summary>
    /// <param name="name"> the name of the attribute </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value </param>
    /// <param name="value4"> the fourth value  </param>
    public void SetAttributef( string name, float value1, float value2, float value3, float value4 )
    {
        Gdx.GL.glVertexAttrib4f( ( uint ) FetchAttributeLocation( name ), value1, value2, value3, value4 );
    }

    /// <summary>
    /// </summary>
    private unsafe void FetchUniforms()
    {
        var numUniforms = stackalloc int[ 1 ];

        Gdx.GL.glGetProgramiv( ( uint ) Handle, IGL.GL_ACTIVE_UNIFORMS, numUniforms );

        Uniforms = new string[ *numUniforms ];

        for ( uint i = 0; i < *numUniforms; i++ )
        {
            var name = Gdx.GL.glGetActiveUniform( ( uint ) Handle,
                                                  i,
                                                  IGL.GL_ACTIVE_UNIFORM_MAX_LENGTH,
                                                  out var size,
                                                  out var type );

            var location = Gdx.GL.glGetUniformLocation( ( uint ) Handle, name );

            _uniforms[ name ]     = location;
            _uniformSizes[ name ] = size;
            _uniformTypes[ name ] = type;
            Uniforms[ i ]         = name;
        }
    }

    /// <summary>
    /// </summary>
    private unsafe void FetchAttributes()
    {
        var numAttributes = stackalloc int[ 1 ];

        Gdx.GL.glGetProgramiv( ( uint ) Handle, IGL.GL_ACTIVE_ATTRIBUTES, numAttributes );

        Attributes = new string[ *numAttributes ];

        for ( var index = 0; index < *numAttributes; index++ )
        {
            var name = Gdx.GL.glGetActiveAttrib( ( uint ) Handle,
                                                 ( uint ) index,
                                                 IGL.GL_ACTIVE_ATTRIBUTE_MAX_LENGTH,
                                                 out var size,
                                                 out var type );

            var location = Gdx.GL.glGetAttribLocation( ( uint ) Handle, name );

            _attributes[ name ]     = location;
            _attributeSizes[ name ] = size;
            _attributeTypes[ name ] = type;
            Attributes[ index ]     = name;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="name"> the name of the attribute </param>
    /// <returns> whether the attribute is available in the shader  </returns>
    public bool HasAttribute( string name )
    {
        return _attributes.ContainsKey( name );
    }

    /// <param name="name"> the name of the attribute </param>
    /// <returns>
    /// the type of the attribute, one of <see cref="IGL.GL_FLOAT"/>,
    /// <see cref="IGL.GL_FLOAT_VEC2"/> etc.
    /// </returns>
    public int GetAttributeType( string name )
    {
        return _attributeTypes.GetValueOrDefault( name, 0 );
    }

    /// <param name="name"> the name of the attribute </param>
    /// <returns> the location of the attribute or -1.  </returns>
    public int GetAttributeLocation( string name )
    {
        return _attributes.GetValueOrDefault( name, 0 );
    }

    /// <param name="name"> the name of the attribute </param>
    /// <returns> the size of the attribute or 0.</returns>
    public int GetAttributeSize( string name )
    {
        return _attributeSizes.GetValueOrDefault( name, 0 );
    }

    /// <param name="name"> the name of the uniform.</param>
    /// <returns> whether the uniform is available in the shader.</returns>
    public bool HasUniform( string name )
    {
        return _uniforms.ContainsKey( name );
    }

    /// <param name="name"> the name of the uniform </param>
    /// <returns>
    /// the type of the uniform, one of <see cref="IGL.GL_FLOAT"/>,
    /// <see cref="IGL.GL_FLOAT_VEC2"/> etc.
    /// </returns>
    public int GetUniformType( string name )
    {
        return _uniformTypes.GetValueOrDefault( name, 0 );
    }

    /// <param name="name"> the name of the uniform </param>
    /// <returns> the location of the uniform or -1.</returns>
    public int GetUniformLocation( string name )
    {
        return _uniforms.GetValueOrDefault( name, -1 );
    }

    /// <param name="name">The name of the uniform</param>
    /// <returns> the size of the uniform or 0.</returns>
    public int GetUniformSize( string name )
    {
        return _uniformSizes.GetValueOrDefault( name, 0 );
    }

    public static string ManagedStatus
    {
        get
        {
            var builder = new StringBuilder( "Managed shaders/app: { " );

            foreach ( var app in _shaders.Keys )
            {
                builder.Append( _shaders[ app ]?.Count );
                builder.Append( ' ' );
            }

            builder.Append( '}' );

            return builder.ToString();
        }
    }

    public static int NumManagedShaderPrograms => _shaders[ Gdx.App ]!.Count;
}
