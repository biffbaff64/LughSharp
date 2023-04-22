namespace LibGDXSharp.Maths
{
    public class Quaternion
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector3 Transform( Vector3 vector3 )
        {
            return null;
        }

        public Matrix4 Set( Vector3 axis, float degrees )
        {
            throw new NotImplementedException();
        }

        public Matrix4 SetFromAxisRad( Vector3 axis, float radians )
        {
            throw new NotImplementedException();
        }

        public Matrix4 SetFromAxis( float axisX, float axisY, float axisZ, float degrees )
        {
            throw new NotImplementedException();
        }

        public Matrix4 SetFromAxisRad( float axisX, float axisY, float axisZ, float radians )
        {
            throw new NotImplementedException();
        }

        public Quaternion SetFromCross( Vector3 v1, Vector3 v2 )
        {
            throw new NotImplementedException();
        }

        public Matrix4 SetFromCross( float x1, float y1, float z1, float x2, float y2, float z2 )
        {
            throw new NotImplementedException();
        }

        public void SetEulerAngles( float yaw, float pitch, float roll )
        {
            throw new NotImplementedException();
        }

        public void SetEulerAnglesRad( float yaw, float pitch, float roll )
        {
            throw new NotImplementedException();
        }

        public Quaternion Slerp( Quaternion quat2, float f )
        {
            throw new NotImplementedException();
        }

        public void Nor()
        {
            throw new NotImplementedException();
        }

        public Vector3 Exp( float f )
        {
            throw new NotImplementedException();
        }

        public void Mul( Vector3 exp )
        {
            throw new NotImplementedException();
        }

        public Quaternion SetFromMatrix( bool normalizeAxes, Matrix4 matrix4 )
        {
            throw new NotImplementedException();
        }

        public Quaternion SetFromMatrix( Matrix4 normalizeAxes )
        {
            throw new NotImplementedException();
        }

        public void Set( Vector3 axis )
        {
            throw new NotImplementedException();
        }
    }
}

