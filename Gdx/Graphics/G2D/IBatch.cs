using LibGDXSharp.Maths;

namespace LibGDXSharp.G2D
{
    public interface IBatch : IDisposable
    {
        void Begin();
        void End();
        
        void SetProjectionMatrix( Matrix4 cameraCombined );
        Matrix4 GetTransformMatrix();
    }
}

