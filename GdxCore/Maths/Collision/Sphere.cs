namespace LibGDXSharp.Maths.Collision
{
    /// <summary>
    /// Encapsulates a 3D sphere with a center and a radius
    /// </summary>
    [Serializable]
    public class Sphere
    {
        public readonly float   radius; // the radius of the sphere
        public readonly Vector3 center; // the center of the sphere

        private readonly static float _PI_4_3 = MathUtils.PI * 4f / 3f;

        /// <summary>
        /// Constructs a sphere with the given center and radius </summary>
        /// <param name="center"> The center </param>
        /// <param name="radius"> The radius  </param>
        public Sphere( Vector3 center, float radius )
        {
            this.center = new Vector3( center );
            this.radius = radius;
        }

        /// <summary>
        /// </summary>
        /// <param name="sphere"> the other sphere </param>
        /// <returns> whether this and the other sphere overlap  </returns>
        public virtual bool Overlaps( Sphere sphere )
        {
            return center.Dst2( sphere.center ) < ( radius + sphere.radius ) * ( radius + sphere.radius );
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            const int prime = 71;

            var result = prime + this.center.GetHashCode();
            result = prime * result + NumberUtils.FloatToRawIntBits( this.radius );

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals( object? o )
        {
            if ( this == o ) return true;

            if ( o == null || o.GetType() != this.GetType() ) return false;

            var s = ( Sphere )o;

            return MathUtils.IsEqual( this.radius, radius ) && this.center.Equals( s.center );
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual float Volume()
        {
            return _PI_4_3 * this.radius * this.radius * this.radius;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual float SurfaceArea()
        {
            return 4 * MathUtils.PI * this.radius * this.radius;
        }
    }
}
