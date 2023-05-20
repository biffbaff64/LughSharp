using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Maths;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class ShapeRenderer : IDisposable
{
    public enum ShapeType
    {
        Point  = IGL20.GL_Points,
        Line   = IGL20.GL_Lines,
        Filled = IGL20.GL_Triangles
    }

    public void Begin()
    {
    }

    public void End()
    {
    }
        
    public void Set( ShapeType line )
    {
    }

    public void SetColor( object getDebugColor )
    {
    }

    public void Rect( float f, float f1, float originX, float originY, float width, float height, float scaleX, float scaleY, float rotation )
    {
    }

    public void Flush()
    {
    }

    public void SetAutoShapeType( bool b )
    {
    }

    public bool IsDrawing()
    {
        return false;
    }

    public Matrix4 GetTransformMatrix()
    {
        throw new NotImplementedException();
    }

    public void SetProjectionMatrix( Matrix4? cameraCombined )
    {
    }

    public void Dispose()
    {
    }

    public void SetTransformMatrix( Matrix4 transform )
    {
        throw new NotImplementedException();
    }
}