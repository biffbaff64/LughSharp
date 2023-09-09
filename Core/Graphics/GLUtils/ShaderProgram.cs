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

using System.Text;

using LibGDXSharp.Core.Utils.Collections;
using LibGDXSharp.Maths;
using LibGDXSharp.Utils;
using LibGDXSharp.Utils.Buffers;

using Buffer = LibGDXSharp.Utils.Buffers.Buffer;

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// <para>
/// A shader program encapsulates a vertex and fragment shader pair
/// linked to form a shader program.
/// </para>
/// <para>
/// After construction a ShaderProgram can be used to draw <see cref="Mesh"/>.
/// To make the GPU use a specific ShaderProgram the programs <see cref="Bind()"/>
/// method must be used which effectively binds the program.
/// </para>
/// <para>
/// When a ShaderProgram is bound one can set uniforms, vertex attributes and
/// attributes as needed via the respective methods.
/// </para>
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
    /// <summary>
    /// default name for position attributes.
    /// </summary>
    public const string POSITION_ATTRIBUTE = "a_position";

    /// <summary>
    /// default name for normal attributes.
    /// </summary>
    public const string NORMAL_ATTRIBUTE = "a_normal";

    /// <summary>
    /// default name for color attributes.
    /// </summary>
    public const string COLOR_ATTRIBUTE = "a_color";

    /// <summary>
    /// default name for texcoords attributes, append texture unit number.
    /// </summary>
    public const string TEXCOORD_ATTRIBUTE = "a_texCoord";

    /// <summary>
    /// default name for tangent attribute
    /// </summary>
    public const string TANGENT_ATTRIBUTE = "a_tangent";

    /// <summary>
    /// default name for binormal attribute
    /// </summary>
    public const string BINORMAL_ATTRIBUTE = "a_binormal";

    /// <summary>
    /// default name for boneweight attribute
    /// </summary>
    public const string BONEWEIGHT_ATTRIBUTE = "a_boneWeight";

    /// <summary>
    /// flag indicating whether attributes & uniforms must be present
    /// at all times.
    /// </summary>
    public readonly static bool Pedantic = true;

    /// <summary>
    /// code that is always added to the vertex shader code, typically used to
    /// inject a #version line. Note that this is added as-is, you should include
    /// a newline (`\n`) if needed. 
    /// </summary>
    public readonly static string PrependVertexCode = "";

    /// <summary>
    /// code that is always added to every fragment shader code, typically used
    /// to inject a #version line. Note that this is added as-is, you should
    /// include a newline (`\n`) if needed. 
    /// </summary>
    public readonly static string PrependFragmentCode = "";

    /// <summary>
    /// the list of currently available shaders
    /// </summary>
    private readonly static Dictionary< IApplication, List< ShaderProgram > > Shaders = new();

    /// <summary>
    /// the log
    /// </summary>
    private string _log = "";

    /// <summary>
    /// uniform lookup
    /// </summary>
    private readonly Dictionary< string, int > _uniforms = new();
    private readonly Dictionary< string, int > _uniformTypes = new();
    private readonly Dictionary< string, int > _uniformSizes = new();
    private          string[]                  _uniformNames = null!;

    /// <summary>
    /// attribute lookup
    /// </summary>
    private readonly Dictionary< string, int > _attributes = new();
    private readonly Dictionary< string, int > _attributeTypes = new();
    private readonly Dictionary< string, int > _attributeSizes = new();
    private          string[]                  _attributeNames = null!;

    private int _programHandle;
    private int _vertexShaderHandle;
    private int _fragmentShaderHandle;

    private readonly FloatBuffer _matrix;
    private readonly string      _vertexShaderSource;
    private readonly string      _fragmentShaderSource;

    public bool invalidated;

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

    /// <summary>whether this program compiled successfully</summary>
    public bool IsCompiled { get; set; }

    /// <returns> the number of managed shader programs currently loaded </returns>
    public static int NumManagedShaderPrograms => Shaders[ Gdx.App ].Count;

    public string[] Attributes => _attributeNames;

    public string[] Uniforms => _uniformNames;

    public string VertexShaderSource => _vertexShaderSource;

    public string FragmentShaderSource => _fragmentShaderSource;

    public int Handle => _programHandle;
    
    #endregion properties

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexShader"></param>
    /// <param name="fragmentShader"></param>
    /// <exception cref="ArgumentException"></exception>
    public ShaderProgram( string vertexShader, string fragmentShader )
    {
        if ( string.ReferenceEquals( vertexShader, null ) )
        {
            throw new System.ArgumentException( "vertex shader must not be null" );
        }

        if ( string.ReferenceEquals( fragmentShader, null ) )
        {
            throw new System.ArgumentException( "fragment shader must not be null" );
        }

        if ( !string.IsNullOrEmpty( PrependVertexCode ) )
        {
            vertexShader = PrependVertexCode + vertexShader;
        }

        if ( !string.IsNullOrEmpty( PrependFragmentCode ) )
        {
            fragmentShader = PrependFragmentCode + fragmentShader;
        }

        this._vertexShaderSource   = vertexShader;
        this._fragmentShaderSource = fragmentShader;
        this._matrix               = BufferUtils.NewFloatBuffer( 16 );

        CompileShaders( vertexShader, fragmentShader );

        if ( IsCompiled )
        {
            FetchAttributes();
            FetchUniforms();

            AddManagedShader( Gdx.App, this );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexShader"></param>
    /// <param name="fragmentShader"></param>
    public ShaderProgram( FileInfo vertexShader, FileInfo fragmentShader )
        : this( "", "" ) //vertexShader.ReadString(), fragmentShader.ReadString() )
    {
        //TODO:
    }

    /// <summary>
    /// Loads and compiles the shaders, creates a new program and links the shaders.
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

        _programHandle = LinkProgram( CreateProgram() );

        if ( _programHandle == -1 )
        {
            IsCompiled = false;

            return;
        }

        IsCompiled = true;
    }

    private int LoadShader( int type, string source )
    {
        IntBuffer intbuf = BufferUtils.NewIntBuffer( 1 );

        int shader = Gdx.GL20.GLCreateShader( type );

        if ( shader == 0 )
        {
            return -1;
        }

        Gdx.GL20.GLShaderSource( shader, source );
        Gdx.GL20.GLCompileShader( shader );
        Gdx.GL20.GLGetShaderiv( shader, IGL20.GL_COMPILE_STATUS, intbuf );

        var compiled = intbuf.Get( 0 );

        if ( compiled == 0 )
        {
// gl.glGetShaderiv(shader, IGL20.GL_INFO_LOG_LENGTH, intbuf);
// int infoLogLength = intbuf.get(0);
// if (infoLogLength > 1) {
            string infoLog = Gdx.GL20.GLGetShaderInfoLog( shader );
            _log += type == IGL20.GL_VERTEX_SHADER ? "Vertex shader\n" : "Fragment shader:\n";
            _log += infoLog;

// }
            return -1;
        }

        return shader;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected int CreateProgram()
    {
        var program = Gdx.GL20.GLCreateProgram();

        return program != 0 ? program : -1;
    }

    private int LinkProgram( int program )
    {
        if ( program == -1 )
        {
            return -1;
        }

        Gdx.GL20.GLAttachShader( program, _vertexShaderHandle );
        Gdx.GL20.GLAttachShader( program, _fragmentShaderHandle );
        Gdx.GL20.GLLinkProgram( program );

        ByteBuffer tmp = ByteBuffer.AllocateDirect( 4 );
        tmp.Order( ByteOrder.NativeOrder );
        IntBuffer intbuf = tmp.AsIntBuffer();

        Gdx.GL20.GLGetProgramiv( program, IGL20.GL_LINK_STATUS, intbuf );

        var linked = intbuf.Get( 0 );

        if ( linked == 0 )
        {
// Gdx.gl20.glGetProgramiv(program, IGL20.GL_INFO_LOG_LENGTH, intbuf);
// int infoLogLength = intbuf.get(0);
// if (infoLogLength > 1) {
            _log = Gdx.GL20.GLGetProgramInfoLog( program );

// }
            return -1;
        }

        return program;
    }

//    internal readonly static IntBuffer Intbuf = BufferUtils.NewIntBuffer( 1 );

    /// <returns>
    /// the log info for the shader compilation and program linking stage.
    /// The shader needs to be bound for this method to have an effect.
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
                _log = Gdx.GL20.GLGetProgramInfoLog( _programHandle );

                // }
            }

            return _log;
        }
    }

    private int FetchAttributeLocation( string name )
    {
        // -2 == not yet cached
        // -1 == cached but not found
        int location;

        if ( ( location = _attributes.Get( name, -2 ) ) == -2 )
        {
            location            = Gdx.GL20.GLGetAttribLocation( _programHandle, name );
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

        if ( ( location = _uniforms.Get( name, -2 ) ) == -2 )
        {
            location = Gdx.GL20.GLGetUniformLocation( _programHandle, name );

            if ( ( location == -1 ) && pedant )
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
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value"> the value  </param>
    public void SetUniformi( string name, int value )
    {
        CheckManaged();
        Gdx.GL20.GLUniform1I( FetchUniformLocation( name ), value );
    }

    public void SetUniformi( int location, int value )
    {
        CheckManaged();
        Gdx.GL20.GLUniform1I( location, value );
    }

    /// <summary>
    /// Sets the uniform with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value  </param>
    public void SetUniformi( string name, int value1, int value2 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform2I( FetchUniformLocation( name ), value1, value2 );
    }

    public void SetUniformi( int location, int value1, int value2 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform2I( location, value1, value2 );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value  </param>
    public void SetUniformi( string name, int value1, int value2, int value3 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform3I( FetchUniformLocation( name ), value1, value2, value3 );
    }

    public void SetUniformi( int location, int value1, int value2, int value3 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform3I( location, value1, value2, value3 );
    }

    /// <summary>
    /// Sets the uniform with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value </param>
    /// <param name="value4"> the fourth value  </param>
    public void SetUniformi( string name, int value1, int value2, int value3, int value4 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform4I( FetchUniformLocation( name ), value1, value2, value3, value4 );
    }

    public void SetUniformi( int location, int value1, int value2, int value3, int value4 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform4I( location, value1, value2, value3, value4 );
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
        Gdx.GL20.GLUniform1F( FetchUniformLocation( name ), value );
    }

    public void SetUniformf( int location, float value )
    {
        CheckManaged();
        Gdx.GL20.GLUniform1F( location, value );
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
        Gdx.GL20.GLUniform2F( FetchUniformLocation( name ), value1, value2 );
    }

    public void SetUniformf( int location, float value1, float value2 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform2F( location, value1, value2 );
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
        Gdx.GL20.GLUniform3F( FetchUniformLocation( name ), value1, value2, value3 );
    }

    public void SetUniformf( int location, float value1, float value2, float value3 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform3F( location, value1, value2, value3 );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value </param>
    /// <param name="value4"> the fourth value  </param>
    public void SetUniformf( string name, float value1, float value2, float value3, float value4 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform4F( FetchUniformLocation( name ), value1, value2, value3, value4 );
    }

    public void SetUniformf( int location, float value1, float value2, float value3, float value4 )
    {
        CheckManaged();
        Gdx.GL20.GLUniform4F( location, value1, value2, value3, value4 );
    }

    public void SetUniform1Fv( string name, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniform1Fv( FetchUniformLocation( name ), length, values, offset );
    }

    public void SetUniform1Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniform1Fv( location, length, values, offset );
    }

    public void SetUniform2Fv( string name, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniform2Fv( FetchUniformLocation( name ), length / 2, values, offset );
    }

    public void SetUniform2Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniform2Fv( location, length / 2, values, offset );
    }

    public void SetUniform3Fv( string name, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniform3Fv( FetchUniformLocation( name ), length / 3, values, offset );
    }

    public void SetUniform3Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniform3Fv( location, length / 3, values, offset );
    }

    public void SetUniform4Fv( string name, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniform4Fv( FetchUniformLocation( name ), length / 4, values, offset );
    }

    public void SetUniform4Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniform4Fv( location, length / 4, values, offset );
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
        Gdx.GL20.GLUniformMatrix4Fv( location, 1, transpose, matrix.val, 0 );
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
        Gdx.GL20.GLUniformMatrix3Fv( location, 1, transpose, matrix.val, 0 );
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
        Gdx.GL20.GLUniformMatrix3Fv( FetchUniformLocation( name ), count, transpose, buffer );
    }

    /// <summary>
    /// Sets an array of uniform matrices with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="buffer"> buffer containing the matrix data </param>
    /// <param name="count"></param>
    /// <param name="transpose"> whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix4Fv( string name, FloatBuffer buffer, int count, bool transpose )
    {
        CheckManaged();
        buffer.Position = 0;
        Gdx.GL20.GLUniformMatrix4Fv( FetchUniformLocation( name ), count, transpose, buffer );
    }

    public void SetUniformMatrix4Fv( int location, float[] values, int offset, int length )
    {
        CheckManaged();
        Gdx.GL20.GLUniformMatrix4Fv( location, length / 16, false, values, offset );
    }

    public void SetUniformMatrix4Fv( string name, float[] values, int offset, int length )
    {
        SetUniformMatrix4Fv( FetchUniformLocation( name ), values, offset, length );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> x and y as the first and second values respectively  </param>
    public void SetUniformf( string name, Vector2 values )
    {
        SetUniformf( name, values.X, values.Y );
    }

    public void SetUniformf( int location, Vector2 values )
    {
        SetUniformf( location, values.X, values.Y );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> x, y and z as the first, second and third values respectively  </param>
    public void SetUniformf( string name, Vector3 values )
    {
        SetUniformf( name, values.X, values.Y, values.Z );
    }

    public void SetUniformf( int location, Vector3 values )
    {
        SetUniformf( location, values.X, values.Y, values.Z );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> r, g, b and a as the first through fourth values respectively  </param>
    public void SetUniformf( string name, Color values )
    {
        SetUniformf( name, values.R, values.G, values.B, values.A );
    }

    public void SetUniformf( int location, Color values )
    {
        SetUniformf( location, values.R, values.G, values.B, values.A );
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
    /// The type, must be one of IGL20.GL_Byte, IGL20.GL_Unsigned_Byte, IGL20.GL_Short,
    /// IGL20.GL_Unsigned_Short, IGL20.GL_Fixed, or IGL20.GL_Float.
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

        int location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Gdx.GL20.GLVertexAttribPointer( location, size, type, normalize, stride, buffer );
    }

    public void SetVertexAttribute( int location, int size, int type, bool normalize, int stride, Buffer buffer )
    {
        CheckManaged();
        Gdx.GL20.GLVertexAttribPointer( location, size, type, normalize, stride, buffer );
    }

    /// <summary>
    /// Sets the vertex attribute with the given name.
    /// The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="size">The number of components, must be >= 1 and &lt;= 4.</param>
    /// <param name="type">
    /// The type, must be one of IGL20.GL_Byte, IGL20.GL_Unsigned_Byte, IGL20.GL_Short,
    /// IGL20.GL_Unsigned_Short, IGL20.GL_Fixed, or IGL20.GL_Float.
    /// <para>GL_Fixed will not work on the desktop.</para>
    /// </param>
    /// <param name="normalize">
    /// Whether fixed point data should be normalized. Will not work on the desktop.
    /// </param>
    /// <param name="stride">The stride in bytes between successive attributes.</param>
    /// <param name="offset">
    /// Byte offset into the vertex buffer object bound to IGL20.GL_Array_Buffer.
    /// </param>
    public void SetVertexAttribute( string name, int size, int type, bool normalize, int stride, int offset )
    {
        CheckManaged();

        int location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Gdx.GL20.GLVertexAttribPointer( location, size, type, normalize, stride, offset );
    }

    public void SetVertexAttribute( int location, int size, int type, bool normalize, int stride, int offset )
    {
        CheckManaged();
        Gdx.GL20.GLVertexAttribPointer( location, size, type, normalize, stride, offset );
    }

    [Obsolete( "use <see cref=\"bind()\"/> instead, this method will be remove in future version" )]
    public void Begin()
    {
        Bind();
    }

    public void Bind()
    {
        CheckManaged();
        Gdx.GL20.GLUseProgram( _programHandle );
    }

    [Obsolete( "no longer necessary, this method will be remove in future version" )]
    public void End()
    {
    }

    /// <summary>
    /// Disposes all resources associated with this shader.
    /// Must be called when the shader is no longer used.
    /// </summary>
    public void Dispose()
    {
        Gdx.GL20.GLUseProgram( 0 );
        Gdx.GL20.GLDeleteShader( _vertexShaderHandle );
        Gdx.GL20.GLDeleteShader( _fragmentShaderHandle );
        Gdx.GL20.GLDeleteProgram( _programHandle );

        Shaders.Get( Gdx.App ).Remove( this );
    }

    /// <summary>
    /// Disables the vertex attribute with the given name
    /// </summary>
    /// <param name="name"> the vertex attribute name  </param>
    public void DisableVertexAttribute( string name )
    {
        CheckManaged();

        int location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Gdx.GL20.GLDisableVertexAttribArray( location );
    }

    public void DisableVertexAttribute( int location )
    {
        CheckManaged();
        Gdx.GL20.GLDisableVertexAttribArray( location );
    }

    /// <summary>
    /// Enables the vertex attribute with the given name
    /// </summary>
    /// <param name="name"> the vertex attribute name  </param>
    public void EnableVertexAttribute( string name )
    {
        CheckManaged();

        int location = FetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        Gdx.GL20.GLEnableVertexAttribArray( location );
    }

    public void EnableVertexAttribute( int location )
    {
        CheckManaged();
        Gdx.GL20.GLEnableVertexAttribArray( location );
    }

    private void CheckManaged()
    {
        if ( invalidated )
        {
            CompileShaders( _vertexShaderSource, _fragmentShaderSource );
            invalidated = false;
        }
    }

    private void AddManagedShader( IApplication app, ShaderProgram shaderProgram )
    {
        List< ShaderProgram > managedResources = Shaders.Get( app );

        managedResources.Add( shaderProgram );
        Shaders.Put( app, managedResources );
    }

    /// <summary>
    /// Invalidates all shaders so the next time they are used new
    /// handles are generated.
    /// </summary>
    /// <param name="app">  </param>
    public static void InvalidateAllShaderPrograms( IApplication app )
    {
        List< ShaderProgram > shaderArray = Shaders.Get( app );

        foreach ( ShaderProgram sp in shaderArray )
        {
            sp.invalidated = true;
            sp.CheckManaged();
        }
    }

    public static void ClearAllShaderPrograms( IApplication app )
    {
        Shaders.Remove( app );
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
        Gdx.GL20.GLVertexAttrib4F
            (
             FetchAttributeLocation( name ),
             value1,
             value2,
             value3,
             value4
            );
    }

    private readonly IntBuffer _parameters = BufferUtils.NewIntBuffer( 1 );
    private readonly IntBuffer _progType   = BufferUtils.NewIntBuffer( 1 );

    /// <summary>
    /// </summary>
    private void FetchUniforms()
    {
        _parameters.Clear();

        Gdx.GL20.GLGetProgramiv( _programHandle, IGL20.GL_ACTIVE_UNIFORMS, _parameters );

        int numUniforms = _parameters.Get( 0 );

        _uniformNames = new string[ numUniforms ];

        for ( int i = 0; i < numUniforms; i++ )
        {
            _parameters.Clear();
            _parameters.Put( 0, 1 );

            _progType.Clear();

            var name = Gdx.GL20.GLGetActiveUniform( _programHandle, i, _parameters, _progType );

            var location = Gdx.GL20.GLGetUniformLocation( _programHandle, name );

            _uniforms[ name ]     = location;
            _uniformTypes[ name ] = _progType.Get( 0 );
            _uniformSizes[ name ] = _parameters.Get( 0 );
            _uniformNames[ i ]    = name;
        }
    }

    /// <summary>
    /// </summary>
    private void FetchAttributes()
    {
        _parameters.Clear();

        Gdx.GL20.GLGetProgramiv( _programHandle, IGL20.GL_ACTIVE_ATTRIBUTES, _parameters );

        int numAttributes = _parameters.Get( 0 );

        _attributeNames = new string[ numAttributes ];

        for ( int i = 0; i < numAttributes; i++ )
        {
            _parameters.Clear();
            _parameters.Put( 0, 1 );

            _progType.Clear();

            var name     = Gdx.GL20.GLGetActiveAttrib( _programHandle, i, _parameters, _progType );
            var location = Gdx.GL20.GLGetAttribLocation( _programHandle, name );

            _attributes[ name ]     = location;
            _attributeTypes[ name ] = _progType.Get( 0 );
            _attributeSizes[ name ] = _parameters.Get( 0 );
            _attributeNames[ i ]    = name;
        }
    }

    /// <param name="name"> the name of the attribute </param>
    /// <returns> whether the attribute is available in the shader  </returns>
    public bool HasAttribute( string name )
    {
        return _attributes.ContainsKey( name );
    }

    /// <param name="name"> the name of the attribute </param>
    /// <returns>
    /// the type of the attribute, one of <see cref="IGL20.GL_FLOAT"/>,
    /// <see cref="IGL20.GL_FLOAT_VEC2"/> etc.
    /// </returns>
    public int GetAttributeType( string name )
    {
        return _attributeTypes.TryGetValue( name, out var type ) ? type : 0;
    }

    /// <param name="name"> the name of the attribute </param>
    /// <returns> the location of the attribute or -1.  </returns>
    public int GetAttributeLocation( string name )
    {
        return _attributes.TryGetValue( name, out var location ) ? location : 0;
    }

    /// <param name="name"> the name of the attribute </param>
    /// <returns> the size of the attribute or 0.</returns>
    public int GetAttributeSize( string name )
    {
        return _attributeSizes.TryGetValue( name, out var size ) ? size : 0;
    }

    /// <param name="name"> the name of the uniform.</param>
    /// <returns> whether the uniform is available in the shader.</returns>
    public bool HasUniform( string name )
    {
        return _uniforms.ContainsKey( name );
    }

    /// <param name="name"> the name of the uniform </param>
    /// <returns>
    /// the type of the uniform, one of <see cref="IGL20.GL_FLOAT"/>,
    /// <see cref="IGL20.GL_FLOAT_VEC2"/> etc.
    /// </returns>
    public int GetUniformType( string name )
    {
        return _uniformTypes.TryGetValue( name, out var type ) ? type : 0;
    }

    /// <param name="name"> the name of the uniform </param>
    /// <returns> the location of the uniform or -1.</returns>
    public int GetUniformLocation( string name )
    {
        return _uniforms.TryGetValue( name, out var location ) ? location : -1;
    }

    /// <param name="name">The name of the uniform</param>
    /// <returns> the size of the uniform or 0.</returns>
    public int GetUniformSize( string name )
    {
        return _uniformSizes.TryGetValue( name, out var size ) ? size : 0;
    }
}