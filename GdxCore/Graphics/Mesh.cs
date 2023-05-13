using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.GdxCore.Utils.Buffers;

namespace LibGDXSharp.Graphics;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Mesh
{
    public Mesh( VertexDataType vertexDataType, bool b, int size, int i, VertexAttribute vertexAttribute, VertexAttribute vertexAttribute1, VertexAttribute vertexAttribute2 )
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

    public void Dispose()
    {
    }

    public void Render( ShaderProgram? customShader, int glTriangles, int i, int count )
    {
    }

    public void SetVertices( float[] vertices, int i, int idx )
    {
    }

    private readonly ShortBuffer _shortBuffer = BufferUtils.NewShortBuffer( 100 );
    
    public ShortBuffer GetIndicesBuffer()
    {
        return _shortBuffer;
    }
}