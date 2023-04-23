using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maths
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    [SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
    public class Plane
    {
        /// <summary>
        /// Enum specifying on which side a point lies respective to the plane and
        /// it's normal. <see cref="PlaneSide.Front"/> is the side to which the
        /// normal points.
        /// </summary>
        public enum PlaneSide
        {
            OnPlane,
            Back,
            Front
        }

        private readonly Vector3 _normal = new Vector3();
        private          float   _dto    = 0;

        /// <summary>
        /// Constructs a new plane with all values set to 0
        /// </summary>
        public Plane()
        {
        }

        /// <summary>
        /// Constructs a new plane based on the normal and distance to the origin.
        /// </summary>
        /// <param name="normal">The plane normal</param>
        /// <param name="dto">The distance to the origin</param>
        public Plane( Vector3 normal, float dto )
        {
            this._normal.Set( normal ).Nor();
            this._dto = dto;
        }

        /// <summary>
        /// Constructs a new plane based on the normal and a point on the plane.
        /// </summary>
        /// <param name="normal"> The normal </param>
        /// <param name="point"> The point on the plane  </param>
        public Plane( Vector3 normal, Vector3 point )
        {
            this._normal.Set( normal ).Nor();
            this._dto = -this._normal.Dot( point );
        }

        /// <summary>
        /// Constructs a new plane out of the three given points that are considered to
        /// be on the plane. The normal is calculated via a cross product between
        /// ( point1 - point2 ) x ( point2 - point3 )
        /// </summary>
        /// <param name="point1"> The first point </param>
        /// <param name="point2"> The second point </param>
        /// <param name="point3"> The third point  </param>
        public Plane( Vector3 point1, Vector3 point2, Vector3 point3 )
        {
            Set( point1, point2, point3 );
        }

        /// <summary>
        /// Sets the plane normal and distance to the origin based on the three given
        /// points which are considered to be on the plane. The normal is calculated
        /// via a cross product between ( point1 - point2 ) x ( point2 - point3 )
        /// </summary>
        /// <param name="point1"> </param>
        /// <param name="point2"> </param>
        /// <param name="point3">  </param>
        public void Set( Vector3 point1, Vector3 point2, Vector3 point3 )
        {
            _normal.Set( point1 )
                .Sub( point2 )
                .Crs( point2.X - point3.X, point2.Y - point3.Y, point2.Z - point3.Z )
                .Nor();

            _dto = -point1.Dot( _normal );
        }

        /// <summary>
        /// Sets the plane normal and distance
        /// </summary>
        /// <param name="nx"> normal x-component </param>
        /// <param name="ny"> normal y-component </param>
        /// <param name="nz"> normal z-component </param>
        /// <param name="d"> distance to origin  </param>
        public void Set( float nx, float ny, float nz, float d )
        {
            _normal.Set( nx, ny, nz );
            this._dto = d;
        }

        /// <summary>
        /// Calculates the shortest signed distance between the plane and the given point.
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>the shortest signed distance between the plane and the point</returns>
        public float Distance( Vector3 point )
        {
            return _normal.Dot( point ) + _dto;
        }

        /// <summary>
        /// Returns on which side the given point lies relative to the plane and its
        /// normal. PlaneSide.Front refers to the side the plane normal points to.
        /// </summary>
        /// <param name="point"> The point </param>
        /// <returns> The side the point lies relative to the plane  </returns>
        public PlaneSide TestPoint( Vector3 point )
        {
            var dist = _normal.Dot( point ) + _dto;

            if ( dist == 0 )
            {
                return PlaneSide.OnPlane;
            }
            else if ( dist < 0 )
            {
                return PlaneSide.Back;
            }
            else
            {
                return PlaneSide.Front;
            }
        }

        /// <summary>
        /// Returns on which side the given point lies relative to the plane and its
        /// normal. PlaneSide.Front refers to the side the plane normal points to.
        /// </summary>
        /// <param name="x"> </param>
        /// <param name="y"> </param>
        /// <param name="z"> </param>
        /// <returns> The side the point lies relative to the plane  </returns>
        public PlaneSide TestPoint( float x, float y, float z )
        {
            var dist = _normal.Dot( x, y, z ) + _dto;

            if ( dist == 0 )
            {
                return PlaneSide.OnPlane;
            }
            else if ( dist < 0 )
            {
                return PlaneSide.Back;
            }
            else
            {
                return PlaneSide.Front;
            }
        }

        /// <summary>
        /// Returns whether the plane is facing the direction vector. Think of the
        /// direction vector as the direction a camera looks in. This method will
        /// return true if the front side of the plane determined by its normal
        /// faces the camera.
        /// </summary>
        /// <param name="direction"> the direction </param>
        /// <returns> whether the plane is front facing  </returns>
        public bool IsFrontFacing( Vector3 direction )
        {
            return _normal.Dot( direction ) <= 0;
        }

        /// <returns> The normal </returns>
        public Vector3 Normal => _normal;

        /// <summary>
        /// </summary>
        /// <returns>The distance to the origin</returns>
        public float DistanceToOrigin => _dto;

        /// <summary>
        /// Sets the plane to the given point and normal.
        /// </summary>
        /// <param name="point">the point on the plane</param>
        /// <param name="norm">the normal of the plane</param>
        public void Set( Vector3 point, Vector3 norm )
        {
            this._normal.Set( norm );
            _dto = -point.Dot( norm );
        }

        /// <summary>
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="pointZ"></param>
        /// <param name="norX"></param>
        /// <param name="norY"></param>
        /// <param name="norZ"></param>
        public void Set( float pointX, float pointY, float pointZ, float norX, float norY, float norZ )
        {
            this._normal.Set( norX, norY, norZ );
            _dto = -( pointX * norX + pointY * norY + pointZ * norZ );
        }

        /// <summary>
        /// Sets this plane from the given plane
        /// </summary>
        /// <param name="plane"> the plane  </param>
        public void Set( Plane plane )
        {
            this._normal.Set( plane._normal );
            this._dto = plane._dto;
        }

        public override string ToString()
        {
            return _normal.ToString() + ", " + _dto;
        }
    }
}
