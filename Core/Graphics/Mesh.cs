using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.GdxCore.Utils.Buffers;

namespace LibGDXSharp.Graphics;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class Mesh
{
    public enum VertexDataType
    {
        VertexArray,
        VertexBufferObject,
        VertexBufferObjectSubData,
        VertexBufferObjectWithVAO
    }

    internal readonly static IDictionary< IApplication, List< Mesh > > Meshes =
        new Dictionary< IApplication, List< Mesh > >();

    IVertexData   _vertices;
    IIndexData    _indices;
    IInstanceData _instances;
    bool          _autoBind = true;
    bool          _isVertexArray;
    bool          _isInstanced = false;

    private readonly ShortBuffer _shortBuffer = BufferUtils.NewShortBuffer( 100 );

    protected Mesh( IVertexData vertices, IIndexData indices, bool isVertexArray )
    {
        this._vertices      = vertices;
        this._indices       = indices;
        this._isVertexArray = isVertexArray;

        AddManagedMesh( Gdx.App, this );
    }

    /// <summary>
    /// Creates a new Mesh with the given attributes.
    /// </summary>
    /// <param name="isStatic"> whether this mesh is static or not. Allows for internal optimizations. </param>
    /// <param name="maxVertices"> the maximum number of vertices this mesh can hold </param>
    /// <param name="maxIndices"> the maximum number of indices this mesh can hold </param>
    /// <param name="attributes">
    /// the <see cref="VertexAttribute"/>s. Each vertex attribute defines one property
    /// of a vertex such as position, normal or texture coordinate
    /// </param>
    public Mesh( bool isStatic, int maxVertices, int maxIndices, params VertexAttribute[] attributes )
    {
        _vertices      = MakeVertexBuffer( isStatic, maxVertices, new VertexAttributes( attributes ) );
        _indices       = new IndexBufferObject( isStatic, maxIndices );
        _isVertexArray = false;

        AddManagedMesh( Gdx.App, this );
    }

    /// <summary>
    /// Creates a new Mesh with the given attributes.
    /// </summary>
    /// <param name="isStatic">whether this mesh is static or not. Allows for internal optimizations.</param>
    /// <param name="maxVertices">the maximum number of vertices this mesh can hold</param>
    /// <param name="maxIndices">the maximum number of indices this mesh can hold</param>
    /// <param name="attributes">
    /// the <see cref="VertexAttributes"/>. Each vertex attribute defines one property
    /// of a vertex such as position, normal or texture coordinate
    /// </param>
    public Mesh( bool isStatic, int maxVertices, int maxIndices, VertexAttributes attributes )
    {
        _vertices      = MakeVertexBuffer( isStatic, maxVertices, attributes );
        _indices       = new IndexBufferObject( isStatic, maxIndices );
        _isVertexArray = false;

        AddManagedMesh( Gdx.App, this );
    }

    /// <summary>
    /// Creates a new Mesh with the given attributes. Adds extra optimizations
    /// for dynamic (frequently modified) meshes.
    /// </summary>
    /// <param name="staticVertices">
    /// whether vertices of this mesh are static or not. Allows for internal optimizations.
    /// </param>
    /// <param name="staticIndices">
    /// whether indices of this mesh are static or not. Allows for internal optimizations.
    /// </param>
    /// <param name="maxVertices">
    /// the maximum number of vertices this mesh can hold
    /// </param>
    /// <param name="maxIndices">
    /// the maximum number of indices this mesh can hold
    /// </param>
    /// <param name="attributes">
    /// the <see cref="VertexAttributes"/>. Each vertex attribute defines one property
    /// of a vertex such as position, normal or texture coordinate
    /// </param>
    public Mesh( bool staticVertices,
                 bool staticIndices,
                 int maxVertices,
                 int maxIndices,
                 VertexAttributes attributes )
    {
        _vertices      = MakeVertexBuffer( staticVertices, maxVertices, attributes );
        _indices       = new IndexBufferObject( staticIndices, maxIndices );
        _isVertexArray = false;

        AddManagedMesh( Gdx.App, this );
    }

    /// <summary>
    /// Creates a new Mesh with the given attributes. This is an expert method
    /// with no error checking. Use at your own risk.
    /// </summary>
    /// <param name="type">the <see cref="VertexDataType"/> to be used, VBO or VA.</param>
    /// <param name="isStatic">whether this mesh is static or not. Allows for internal optimizations.</param>
    /// <param name="maxVertices">the maximum number of vertices this mesh can hold</param>
    /// <param name="maxIndices">the maximum number of indices this mesh can hold</param>
    /// <param name="attributes">
    /// the <see cref="VertexAttribute"/>s. Each vertex attribute defines one property
    /// of a vertex such as position, normal or texture coordinate
    /// </param>
    public Mesh( VertexDataType type,
                 bool isStatic,
                 int maxVertices,
                 int maxIndices,
                 params VertexAttribute[] attributes )
        : this( type, isStatic, maxVertices, maxIndices, new VertexAttributes( attributes ) )
    {
    }

    /// <summary>
    /// Creates a new Mesh with the given attributes. This is an expert method
    /// with no error checking. Use at your own risk.
    /// </summary>
    /// <param name="type">the <see cref="VertexDataType"/> to be used, VBO or VA.</param>
    /// <param name="isStatic">whether this mesh is static or not. Allows for internal optimizations.</param>
    /// <param name="maxVertices">the maximum number of vertices this mesh can hold</param>
    /// <param name="maxIndices">the maximum number of indices this mesh can hold</param>
    /// <param name="attributes">the <see cref="VertexAttributes"/>.</param>
    public Mesh( VertexDataType type, bool isStatic, int maxVertices, int maxIndices, VertexAttributes attributes )
    {
        switch ( type )
        {
            case VertexDataType.VertexBufferObject:
                _vertices      = new VertexBufferObject( isStatic, maxVertices, attributes );
                _indices       = new IndexBufferObject( isStatic, maxIndices );
                _isVertexArray = false;

                break;

            case VertexDataType.VertexBufferObjectSubData:
                _vertices      = new VertexBufferObjectSubData( isStatic, maxVertices, attributes );
                _indices       = new IndexBufferObjectSubData( isStatic, maxIndices );
                _isVertexArray = false;

                break;

            case VertexDataType.VertexBufferObjectWithVAO:
                _vertices      = new VertexBufferObjectWithVAO( isStatic, maxVertices, attributes );
                _indices       = new IndexBufferObjectSubData( isStatic, maxIndices );
                _isVertexArray = false;

                break;

            case VertexDataType.VertexArray:
            default:
                _vertices      = new VertexArray( maxVertices, attributes );
                _indices       = new IndexArray( maxIndices );
                _isVertexArray = true;

                break;
        }

        AddManagedMesh( Gdx.App, this );
    }

    private IVertexData MakeVertexBuffer( bool isStatic, int maxVertices, VertexAttributes vertexAttributes )
    {
        if ( Gdx.GL30 != null )
        {
            return new VertexBufferObjectWithVAO( isStatic, maxVertices, vertexAttributes );
        }
        else
        {
            return new VertexBufferObject( isStatic, maxVertices, vertexAttributes );
        }
    }

    public void SetIndices( short[] indices )
    {
    }

    public void Render( ShaderProgram? customShader, int glTriangles, int i, int count )
    {
    }

    public void Render( ShaderProgram? customShader, int glTriangles )
    {
    }

    public void SetVertices( float[] vertices, int i, int idx )
    {
    }

    public void SetAutoBind( bool bind )
    {
    }

    public void Bind( ShaderProgram? shader )
    {
    }

    public void Unbind( ShaderProgram? shader )
    {
    }

    public ShortBuffer GetIndicesBuffer()
    {
        return _shortBuffer;
    }

    public FloatBuffer GetVerticesBuffer()
    {
        return null!;
    }

    public int GetNumIndices()
    {
        return 0;
    }

    public VertexAttributes GetVertexAttributes()
    {
        return _vertices.Attributes;
    }

    public VertexAttribute? GetVertexAttribute( int usage )
    {
        VertexAttributes attributes = _vertices.getAttributes();
        var              len        = attributes.Size;

        for ( var i = 0; i < len; i++ )
        {
            if ( attributes.Get( i ).usage == usage )
            {
                return attributes.Get( i );
            }
        }

        return null;
    }

    public void Dispose()
    {
    }
}