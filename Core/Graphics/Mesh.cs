using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.GdxCore.Utils.Buffers;

namespace LibGDXSharp.Graphics;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class Mesh
{
    private readonly ShortBuffer _shortBuffer = BufferUtils.NewShortBuffer( 100 );

    public Mesh( VertexDataType vertexDataType, bool b, int size, int i, VertexAttribute vertexAttribute, VertexAttribute vertexAttribute1, VertexAttribute vertexAttribute2 )
    {
    }

    public Mesh( bool b, int size, int i, VertexAttribute vertexAttribute, VertexAttribute vertexAttribute1, VertexAttribute vertexAttribute2 )
    {
    }

    public enum VertexDataType
    {
        VertexArray,
        VertexBufferObject,
        VertexBufferObjectSubData,
        VertexBufferObjectWithVAO
    }

    public void SetIndices( short[] indices )
    {
    }

    public void Render( ShaderProgram? customShader, int glTriangles, int i, int count )
    {
    }

    public void SetVertices( float[] vertices, int i, int idx )
    {
    }

    public void SetAutoBind( bool b )
    {
    }

    public void Bind( ShaderProgram? customShader )
    {
    }

    public void Unbind( ShaderProgram? customShader )
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
    
    public void Dispose()
    {
    }
}