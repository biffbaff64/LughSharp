using System.Text;

using LibGDXSharp.Maths;

using Buffer = Silk.NET.OpenGLES.Buffer;

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
public class ShaderProgram
{
    /// <summary>
    /// default name for position attributes.
    /// </summary>
    public const string PositionAttribute = "a_position";

    /// <summary>
    /// default name for normal attributes.
    /// </summary>
    public const string NormalAttribute = "a_normal";

    /// <summary>
    /// default name for color attributes.
    /// </summary>
    public const string ColorAttribute = "a_color";

    /// <summary>
    /// default name for texcoords attributes, append texture unit number.
    /// </summary>
    public const string TexcoordAttribute = "a_texCoord";

    /// <summary>
    /// default name for tangent attribute
    /// </summary>
    public const string TangentAttribute = "a_tangent";

    /// <summary>
    /// default name for binormal attribute
    /// </summary>
    public const string BinormalAttribute = "a_binormal";

    /// <summary>
    /// default name for boneweight attribute
    /// </summary>
    public const string BoneweightAttribute = "a_boneWeight";

    /// <summary>
    /// flag indicating whether attributes & uniforms must be present
    /// at all times.
    /// </summary>
    public static bool pedantic = true;

    /// <summary>
    /// code that is always added to the vertex shader code, typically used to
    /// inject a #version line. Note that this is added as-is, you should include
    /// a newline (`\n`) if needed. 
    /// </summary>
    public static string prependVertexCode = "";

    /// <summary>
    /// code that is always added to every fragment shader code, typically used
    /// to inject a #version line. Note that this is added as-is, you should
    /// include a newline (`\n`) if needed. 
    /// </summary>
    public static string prependFragmentCode = "";

    /// <summary>
    /// the list of currently available shaders * </summary>
    private readonly static Dictionary< IApplication, List< ShaderProgram > > shaders = new();

    /// <summary>
    /// the log * </summary>
    private string _log = "";

    /// <summary>whether this program compiled successfully</summary>
    private bool _isCompiled;

    /// <summary>uniform lookup</summary>
    private readonly Dictionary< string, int > _uniforms = new();
    private readonly Dictionary< string, int > _uniformTypes = new();
    private readonly Dictionary< string, int > _uniformSizes = new();

    private string[] _uniformNames;

    /// <summary>
    /// attribute lookup
    /// </summary>
    private readonly Dictionary< string, int > _attributes = new();
    private readonly Dictionary< string, int > _attributeTypes = new();
    private readonly Dictionary< string, int > _attributeSizes = new();

    /// <summary>
    /// Attribute names.
    /// </summary>
    private string[] _attributeNames;

    /// <summary>
    /// Program handle.
    /// </summary>
    private int _program;

    private int _vertexShaderHandle;

    private int _fragmentShaderHandle;

    private readonly FloatBuffer _matrix;

    private readonly string _vertexShaderSource;

    private readonly string _fragmentShaderSource;

    public bool invalidated;

    private int _refCount = 0;

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

        if ( !string.IsNullOrEmpty( prependVertexCode ) )
        {
            vertexShader = prependVertexCode + vertexShader;
        }

        if ( !string.IsNullOrEmpty( prependFragmentCode ) )
        {
            fragmentShader = prependFragmentCode + fragmentShader;
        }

        this._vertexShaderSource   = vertexShader;
        this._fragmentShaderSource = fragmentShader;
        this._matrix               = BufferUtils.NewFloatBuffer( 16 );

        CompileShaders( vertexShader, fragmentShader );

        if ( _isCompiled )
        {
            fetchAttributes();
            fetchUniforms();
            AddManagedShader( Gdx.App, this );
        }
    }

    public ShaderProgram( FileHandle vertexShader, FileHandle fragmentShader ) : this
        ( vertexShader.readString(), fragmentShader.readString() )
    {
    }

    /// <summary>
    /// Loads and compiles the shaders, creates a new program and links the shaders.
    /// </summary>
    /// <param name="vertexShader"> </param>
    /// <param name="fragmentShader">  </param>
    private void CompileShaders( string vertexShader, string fragmentShader )
    {
        vertexShaderHandle   = LoadShader( IGL20.GL_VERTEX_SHADER, vertexShader );
        fragmentShaderHandle = LoadShader( IGL20.GL_FRAGMENT_SHADER, fragmentShader );

        if ( vertexShaderHandle == -1 || fragmentShaderHandle == -1 )
        {
            isCompiled = false;

            return;
        }

        program = LinkProgram( CreateProgram() );

        if ( program == -1 )
        {
            isCompiled = false;

            return;
        }

        isCompiled = true;
    }

    private int LoadShader( int type, string source )
    {
        IGL20     gl     = Gdx.gl20;
        IntBuffer intbuf = BufferUtils.newIntBuffer( 1 );

        int shader = gl.glCreateShader( type );

        if ( shader == 0 )
        {
            return -1;
        }

        gl.glShaderSource( shader, source );
        gl.glCompileShader( shader );
        gl.glGetShaderiv( shader, IGL20.GL_COMPILE_STATUS, intbuf );

        int compiled = intbuf.get( 0 );

        if ( compiled == 0 )
        {
// gl.glGetShaderiv(shader, IGL20.GL_INFO_LOG_LENGTH, intbuf);
// int infoLogLength = intbuf.get(0);
// if (infoLogLength > 1) {
            string infoLog = gl.glGetShaderInfoLog( shader );
            log += type == IGL20.GL_VERTEX_SHADER ? "Vertex shader\n" : "Fragment shader:\n";
            log += infoLog;

// }
            return -1;
        }

        return shader;
    }

    protected int CreateProgram()
    {
        IGL20 gl      = Gdx.gl20;
        int   program = gl.glCreateProgram();

        return program != 0 ? program : -1;
    }

    private int LinkProgram( int program )
    {
        IGL20 gl = Gdx.gl20;

        if ( program == -1 )
        {
            return -1;
        }

        gl.glAttachShader( program, vertexShaderHandle );
        gl.glAttachShader( program, fragmentShaderHandle );
        gl.glLinkProgram( program );

        ByteBuffer tmp = ByteBuffer.allocateDirect( 4 );
        tmp.order( ByteOrder.nativeOrder() );
        IntBuffer intbuf = tmp.asIntBuffer();

        gl.glGetProgramiv( program, IGL20.GL_LINK_STATUS, intbuf );
        int linked = intbuf.get( 0 );

        if ( linked == 0 )
        {
// Gdx.gl20.glGetProgramiv(program, IGL20.GL_INFO_LOG_LENGTH, intbuf);
// int infoLogLength = intbuf.get(0);
// if (infoLogLength > 1) {
            log = Gdx.gl20.glGetProgramInfoLog( program );

// }
            return -1;
        }

        return program;
    }

    internal static readonly IntBuffer Intbuf = BufferUtils.newIntBuffer( 1 );

    /// <returns> the log info for the shader compilation and program linking stage. The shader needs to be bound for this method to
    ///         have an effect.  </returns>
    public string Log
    {
        get
        {
            if ( isCompiled )
            {
                // Gdx.gl20.glGetProgramiv(program, IGL20.GL_INFO_LOG_LENGTH, intbuf);
                // int infoLogLength = intbuf.get(0);
                // if (infoLogLength > 1) {
                log = Gdx.gl20.glGetProgramInfoLog( program );

                // }
                return log;
            }
            else
            {
                return log;
            }
        }
    }

    /// <returns> whether this ShaderProgram compiled successfully. </returns>
    public bool Compiled => isCompiled;

    private int FetchAttributeLocation( string name )
    {
        IGL20 gl = Gdx.gl20;
        // -2 == not yet cached
        // -1 == cached but not found
        int location;

        if ( ( location = attributes.get( name, -2 ) ) == -2 )
        {
            location = gl.glGetAttribLocation( program, name );
            attributes.put( name, location );
        }

        return location;
    }

    private int FetchUniformLocation( string name )
    {
        return FetchUniformLocation( name, pedantic );
    }

    public int FetchUniformLocation( string name, bool pedantic )
    {
        // -2 == not yet cached
        // -1 == cached but not found
        int location;

        if ( ( location = uniforms.get( name, -2 ) ) == -2 )
        {
            location = Gdx.gl20.glGetUniformLocation( program, name );

            if ( location == -1 && pedantic )
            {
                if ( isCompiled )
                {
                    throw new System.ArgumentException( "No uniform with name '" + name + "' in shader" );
                }

                throw new System.InvalidOperationException( "An attempted fetch uniform from uncompiled shader \n" + Log );
            }

            uniforms.put( name, location );
        }

        return location;
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
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
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
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
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/>
    /// must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value </param>
    /// <param name="value4"> the fourth value  </param>
    public void SetUniformi( string name, int value1, int value2, int value3, int value4 )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform4i( location, value1, value2, value3, value4 );
    }

    public void SetUniformi( int location, int value1, int value2, int value3, int value4 )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform4i( location, value1, value2, value3, value4 );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value"> the value  </param>
    public void SetUniformf( string name, float value )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform1f( location, value );
    }

    public void SetUniformf( int location, float value )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform1f( location, value );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value  </param>
    public void SetUniformf( string name, float value1, float value2 )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform2f( location, value1, value2 );
    }

    public void SetUniformf( int location, float value1, float value2 )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform2f( location, value1, value2 );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value  </param>
    public void SetUniformf( string name, float value1, float value2, float value3 )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform3f( location, value1, value2, value3 );
    }

    public void SetUniformf( int location, float value1, float value2, float value3 )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform3f( location, value1, value2, value3 );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="value1"> the first value </param>
    /// <param name="value2"> the second value </param>
    /// <param name="value3"> the third value </param>
    /// <param name="value4"> the fourth value  </param>
    public void SetUniformf( string name, float value1, float value2, float value3, float value4 )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform4f( location, value1, value2, value3, value4 );
    }

    public void SetUniformf( int location, float value1, float value2, float value3, float value4 )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform4f( location, value1, value2, value3, value4 );
    }

    public void SetUniform1fv( string name, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform1fv( location, length, values, offset );
    }

    public void SetUniform1fv( int location, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform1fv( location, length, values, offset );
    }

    public void SetUniform2fv( string name, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform2fv( location, length / 2, values, offset );
    }

    public void SetUniform2fv( int location, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform2fv( location, length / 2, values, offset );
    }

    public void SetUniform3fv( string name, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform3fv( location, length / 3, values, offset );
    }

    public void SetUniform3fv( int location, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform3fv( location, length / 3, values, offset );
    }

    public void SetUniform4fv( string name, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchUniformLocation( name );
        gl.glUniform4fv( location, length / 4, values, offset );
    }

    public void SetUniform4fv( int location, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniform4fv( location, length / 4, values, offset );
    }

    /// <summary>
    /// Sets the uniform matrix with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix  </param>
    public void SetUniformMatrix( string name, Matrix4 matrix )
    {
        SetUniformMatrix( name, matrix, false );
    }

    /// <summary>
    /// Sets the uniform matrix with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix </param>
    /// <param name="transpose"> whether the matrix should be transposed  </param>
    public void SetUniformMatrix( string name, Matrix4 matrix, bool transpose )
    {
        setUniformMatrix( fetchUniformLocation( name ), matrix, transpose );
    }

    public void SetUniformMatrix( int location, Matrix4 matrix )
    {
        SetUniformMatrix( location, matrix, false );
    }

    public void SetUniformMatrix( int location, Matrix4 matrix, bool transpose )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniformMatrix4fv( location, 1, transpose, matrix.val, 0 );
    }

    /// <summary>
    /// Sets the uniform matrix with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix  </param>
    public void SetUniformMatrix( string name, Matrix3 matrix )
    {
        SetUniformMatrix( name, matrix, false );
    }

    /// <summary>
    /// Sets the uniform matrix with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="matrix"> the matrix </param>
    /// <param name="transpose"> whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix( string name, Matrix3 matrix, bool transpose )
    {
        setUniformMatrix( fetchUniformLocation( name ), matrix, transpose );
    }

    public void SetUniformMatrix( int location, Matrix3 matrix )
    {
        SetUniformMatrix( location, matrix, false );
    }

    public void SetUniformMatrix( int location, Matrix3 matrix, bool transpose )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniformMatrix3fv( location, 1, transpose, matrix.val, 0 );
    }

    /// <summary>
    /// Sets an array of uniform matrices with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="buffer"> buffer containing the matrix data </param>
    /// <param name="transpose"> whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix3fv( string name, FloatBuffer buffer, int count, bool transpose )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        ( ( Buffer )buffer ).position( 0 );
        int location = fetchUniformLocation( name );
        gl.glUniformMatrix3fv( location, count, transpose, buffer );
    }

    /// <summary>
    /// Sets an array of uniform matrices with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="buffer"> buffer containing the matrix data </param>
    /// <param name="transpose"> whether the uniform matrix should be transposed  </param>
    public void SetUniformMatrix4fv( string name, FloatBuffer buffer, int count, bool transpose )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        ( ( Buffer )buffer ).position( 0 );
        int location = fetchUniformLocation( name );
        gl.glUniformMatrix4fv( location, count, transpose, buffer );
    }

    public void SetUniformMatrix4fv( int location, float[] values, int offset, int length )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glUniformMatrix4fv( location, length / 16, false, values, offset );
    }

    public void SetUniformMatrix4fv( string name, float[] values, int offset, int length )
    {
        setUniformMatrix4fv( fetchUniformLocation( name ), values, offset, length );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> x and y as the first and second values respectively  </param>
    public void SetUniformf( string name, Vector2 values )
    {
        setUniformf( name, values.x, values.y );
    }

    public void SetUniformf( int location, Vector2 values )
    {
        setUniformf( location, values.x, values.y );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> x, y and z as the first, second and third values respectively  </param>
    public void SetUniformf( string name, Vector3 values )
    {
        setUniformf( name, values.x, values.y, values.z );
    }

    public void SetUniformf( int location, Vector3 values )
    {
        setUniformf( location, values.x, values.y, values.z );
    }

    /// <summary>
    /// Sets the uniform with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the name of the uniform </param>
    /// <param name="values"> r, g, b and a as the first through fourth values respectively  </param>
    public void SetUniformf( string name, Color values )
    {
        setUniformf( name, values.r, values.g, values.b, values.a );
    }

    public void SetUniformf( int location, Color values )
    {
        setUniformf( location, values.r, values.g, values.b, values.a );
    }

    /// <summary>
    /// Sets the vertex attribute with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the attribute name </param>
    /// <param name="size"> the number of components, must be >= 1 and <= 4 </param>
    /// <param name="type"> the type, must be one of IGL20.GL_BYTE, IGL20.GL_UNSIGNED_BYTE, IGL20.GL_SHORT,
    ///           IGL20.GL_UNSIGNED_SHORT,IGL20.GL_FIXED, or IGL20.GL_FLOAT. GL_FIXED will not work on the desktop </param>
    /// <param name="normalize"> whether fixed point data should be normalized. Will not work on the desktop </param>
    /// <param name="stride"> the stride in bytes between successive attributes </param>
    /// <param name="buffer"> the buffer containing the vertex attributes.  </param>
    public void SetVertexAttribute( string name, int size, int type, bool normalize, int stride, Buffer buffer )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        int location = fetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        gl.glVertexAttribPointer( location, size, type, normalize, stride, buffer );
    }

    public void SetVertexAttribute( int location, int size, int type, bool normalize, int stride, Buffer buffer )
    {
        IGL20 gl = Gdx.gl20;
        checkManaged();
        gl.glVertexAttribPointer( location, size, type, normalize, stride, buffer );
    }

    /// <summary>
    /// Sets the vertex attribute with the given name. The <see cref="ShaderProgram"/> must be bound for this to work.
    /// </summary>
    /// <param name="name"> the attribute name </param>
    /// <param name="size"> the number of components, must be >= 1 and <= 4 </param>
    /// <param name="type"> the type, must be one of IGL20.GL_BYTE, IGL20.GL_UNSIGNED_BYTE, IGL20.GL_SHORT,
    ///           IGL20.GL_UNSIGNED_SHORT,IGL20.GL_FIXED, or IGL20.GL_FLOAT. GL_FIXED will not work on the desktop </param>
    /// <param name="normalize"> whether fixed point data should be normalized. Will not work on the desktop </param>
    /// <param name="stride"> the stride in bytes between successive attributes </param>
    /// <param name="offset"> byte offset into the vertex buffer object bound to IGL20.GL_ARRAY_BUFFER.  </param>
    public void SetVertexAttribute( string name, int size, int type, bool normalize, int stride, int offset )
    {
        IGL20 gl = Gdx.gl20;
        CheckManaged();
        int location = fetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        gl.glVertexAttribPointer( location, size, type, normalize, stride, offset );
    }

    public void SetVertexAttribute( int location, int size, int type, bool normalize, int stride, int offset )
    {
        IGL20 gl = Gdx.gl20;
        CheckManaged();
        gl.glVertexAttribPointer( location, size, type, normalize, stride, offset );
    }

    /// @deprecated use <see cref="bind()"/> instead, this method will be remove in future version 
    [Obsolete( "use <see cref=\"bind()\"/> instead, this method will be remove in future version" )]
    public void Begin()
    {
        Bind();
    }

    public void Bind()
    {
        IGL20 gl = Gdx.gl20;
        CheckManaged();
        gl.glUseProgram( program );
    }

    /// @deprecated no longer necessary, this method will be remove in future version 
    [Obsolete( "no longer necessary, this method will be remove in future version" )]
    public void End()
    {
    }

    /// <summary>
    /// Disposes all resources associated with this shader. Must be called when the shader is no longer used. </summary>
    public void Dispose()
    {
        IGL20 gl = Gdx.gl20;
        gl.glUseProgram( 0 );
        gl.glDeleteShader( vertexShaderHandle );
        gl.glDeleteShader( fragmentShaderHandle );
        gl.glDeleteProgram( program );

        if ( shaders.get( Gdx.app ) != null )
        {
            shaders.get( Gdx.app ).removeValue( this, true );
        }
    }

    /// <summary>
    /// Disables the vertex attribute with the given name
    /// </summary>
    /// <param name="name"> the vertex attribute name  </param>
    public void DisableVertexAttribute( string name )
    {
        IGL20 gl = Gdx.gl20;
        CheckManaged();
        int location = fetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        gl.glDisableVertexAttribArray( location );
    }

    public void DisableVertexAttribute( int location )
    {
        IGL20 gl = Gdx.gl20;
        CheckManaged();
        gl.glDisableVertexAttribArray( location );
    }

    /// <summary>
    /// Enables the vertex attribute with the given name
    /// </summary>
    /// <param name="name"> the vertex attribute name  </param>
    public void EnableVertexAttribute( string name )
    {
        IGL20 gl = Gdx.gl20;
        CheckManaged();
        int location = fetchAttributeLocation( name );

        if ( location == -1 )
        {
            return;
        }

        gl.glEnableVertexAttribArray( location );
    }

    public void EnableVertexAttribute( int location )
    {
        IGL20 gl = Gdx.gl20;
        CheckManaged();
        gl.glEnableVertexAttribArray( location );
    }

    private void CheckManaged()
    {
        if ( invalidated )
        {
            compileShaders( vertexShaderSource, fragmentShaderSource );
            invalidated = false;
        }
    }

    private void AddManagedShader( IApplication app, ShaderProgram shaderProgram )
    {
        List< ShaderProgram > managedResources = shaders.get( app );

        if ( managedResources == null )
        {
            managedResources = new List< ShaderProgram >();
        }

        managedResources.add( shaderProgram );
        shaders.put( app, managedResources );
    }
    using System.Text;

    /// <summary>
    /// Invalidates all shaders so the next time they are used new handles are generated </summary>
    /// <param name="app">  </param>
    public static void InvalidateAllShaderPrograms( IApplication app )
    {
        if ( Gdx.gl20 == null )
        {
            return;
        }

        List< ShaderProgram > shaderArray = shaders.get( app );

        if ( shaderArray == null )
        {
            return;
        }

        foreach ( ShaderProgram sp in shaderArray )
        {
            sp.invalidated = true;
            sp.CheckManaged();
        }
    }

    public static void ClearAllShaderPrograms( IApplication app )
    {
        shaders.Remove( app );
    }

    public static string ManagedStatus
    {
        get
        {
            var builder = new StringBuilder( "Managed shaders/app: { " );

            foreach ( IApplication app in shaders.Keys )
            {
                builder.Append( shaders[ app ].Count );
                builder.Append( ' ' );
            }

            builder.Append( '}' );

            return builder.ToString();
        }
    }

    /// <returns> the number of managed shader programs currently loaded </returns>
    public static int NumManagedShaderPrograms => shaders[ Gdx.App ].Count;

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
        Gdx.GL20.GLVertexAttrib4F( FetchAttributeLocation( name ),
                                   value1, value2, value3, value4 );
    }

    private readonly IntBuffer _parameters = BufferUtils.NewIntBuffer( 1 );
    private readonly IntBuffer _progType   = BufferUtils.NewIntBuffer( 1 );

    /// <summary>
    /// </summary>
    private void FetchUniforms()
    {
        _parameters.Clear();

        Gdx.GL20.GLGetProgramiv( _program, IGL20.GL_Active_Uniforms, _parameters );

        int numUniforms = _parameters.Get( 0 );

        _uniformNames = new string[ numUniforms ];

        for ( int i = 0; i < numUniforms; i++ )
        {
            _parameters.Clear();
            _parameters.Put( 0, 1 );

            _progType.Clear();

            var name = Gdx.GL20.GLGetActiveUniform( _program, i, _parameters, _progType );

            var location = Gdx.GL20.GLGetUniformLocation( _program, name );

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

        Gdx.GL20.GLGetProgramiv( _program, IGL20.GL_Active_Attributes, _parameters );

        int numAttributes = _parameters.Get( 0 );

        _attributeNames = new string[ numAttributes ];

        for ( int i = 0; i < numAttributes; i++ )
        {
            _parameters.Clear();
            _parameters.Put( 0, 1 );

            _progType.Clear();

            var name     = Gdx.GL20.GLGetActiveAttrib( _program, i, _parameters, _progType );
            var location = Gdx.GL20.GLGetAttribLocation( _program, name );

            _attributes[ name ]     = location;
            _attributeTypes[ name ] = _progType.Get( 0 ) );
            _attributeSizes[ name ] = _parameters.Get( 0 ) );
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
    /// the type of the attribute, one of <see cref="IGL20.GL_Float"/>,
    /// <see cref="IGL20.GL_Float_Vec2"/> etc.
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
    /// the type of the uniform, one of <see cref="IGL20.GL_Float"/>,
    /// <see cref="IGL20.GL_Float_Vec2"/> etc.
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

    /// <returns> the attributes </returns>
    public string[] Attributes => _attributeNames;

    /// <returns> the uniforms </returns>
    public string[] Uniforms => _uniformNames;

    /// <returns> the source of the vertex shader </returns>
    public string VertexShaderSource => _vertexShaderSource;

    /// <returns> the source of the fragment shader </returns>
    public string FragmentShaderSource => _fragmentShaderSource;

    /// <returns> the handle of the shader program </returns>
    public int Handle => _program;
}
