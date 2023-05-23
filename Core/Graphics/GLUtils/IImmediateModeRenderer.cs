using LibGDXSharp.Maths;

namespace LibGDXSharp.Graphics.GLUtils;

public interface IImmediateModeRenderer : IDisposable
{
    public void Begin( Matrix4 projModelView, int primitiveType );

    public void End();

    public void Flush();

    public void SetColor( Color color );

    public void SetColor( float r, float g, float b, float a );

    public void SetColor( float colorBits );

    public void TexCoord( float u, float v );

    public void Normal( float x, float y, float z );

    public void Vertex( float x, float y, float z );

    public int NumVertices { get; set; }

    public int MaxVertices { get; set; }
}