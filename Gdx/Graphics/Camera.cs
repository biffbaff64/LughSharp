using System.Data;

using LibGDXSharp.Maths;
using LibGDXSharp.Maths.Collision;

namespace LibGDXSharp.Graphics
{
    public class Camera
    {
        public float    ViewportWidth  { get; set; }
        public float    ViewportHeight { get; set; }
        public Vector3? Position       { get; set; }

        public virtual Ray GetPickRay( float screenX, float screenY, float i, float screenY1, float screenWidth, float screenHeight )
        {
            throw new NotImplementedException();
        }

        public virtual void Project( Vector3 worldCoords )
        {
            throw new NotImplementedException();
        }

        public virtual void Project( Vector3 worldCoords, int screenX, int screenY, int screenWidth, int screenHeight )
        {
            throw new NotImplementedException();
        }

        public virtual void Unproject( Vector3 worldCoords )
        {
            throw new NotImplementedException();
        }

        public virtual void Unproject( Vector3 worldCoords, int screenX, int screenY, int screenWidth, int screenHeight )
        {
            throw new NotImplementedException();
        }

        public virtual void Update()
        {
        }
    }
}
