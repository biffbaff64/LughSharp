namespace LibGDXSharp.Maths
{
    /// <summary>
    /// Implementation of the Bezier curve.
    /// </summary>
    public class Bezier<T> : IPath where T : IVector< T >
    {
        /// <summary>
        /// Simple linear interpolation
        /// </summary>
        /// <param name="alist"> The <seealso cref="System.Collections.ArrayList"/> to set to the result.</param>
        /// <param name="t"> The location (ranging 0..1) on the line.</param>
        /// <param name="p0"> The start point.</param>
        /// <param name="p1"> The end point.</param>
        /// <param name="tmp"> A temporary vector to be used by the calculation.</param>
        /// <returns> The value specified by out for chaining </returns>
        public static T Linear<T2>( in T alist, in float t, in T p0, in T p1, in T tmp ) where T2 : List< T >
        {
            // B1(t) = p0 + (p1-p0)*t
            return alist.Set( p0 ).Scl( 1f - t ).Add( tmp.Set( p1 ).Scl( t ) );
        }
        
        /// <summary>
        /// Simple linear interpolation derivative
        /// </summary>
        /// <param name="vec"> The <seealso cref="System.Collections.ArrayList"/> to set to the result.</param>
        /// <param name="t"> The location (ranging 0..1) on the line.</param>
        /// <param name="p0"> The start point.</param>
        /// <param name="p1"> The end point.</param>
        /// <param name="tmp"> A temporary vector to be used by the calculation.</param>
        /// <returns> The value specified by out for chaining</returns>
        public static T LinearDerivative<T2>(in T vec, in float t, in T p0, in T p1, in T tmp) where T2 : List<T>
        {
            // B1'(t) = p1-p0
            return vec.Set(p1).Sub(p0);
        }
        
        /// <summary>
        /// Quadratic Bezier curve
        /// </summary>
        /// <param name="list"> The <seealso cref="System.Collections.ArrayList"/> to set to the result. </param>
        /// <param name="t"> The location (ranging 0..1) on the curve. </param>
        /// <param name="p0"> The first bezier point. </param>
        /// <param name="p1"> The second bezier point. </param>
        /// <param name="p2"> The third bezier point. </param>
        /// <param name="tmp"> A temporary vector to be used by the calculation. </param>
        /// <returns> The value specified by out for chaining  </returns>
        public static T Quadratic<T2>(in T list, in float t, in T p0, in T p1, in T p2, in T tmp) where T2 : List<T>
        {
            // B2(t) = (1 - t) * (1 - t) * p0 + 2 * (1-t) * t * p1 + t*t*p2
            var dt = 1f - t;
            return list.Set(p0).Scl(dt * dt).Add(tmp.Set(p1).Scl(2 * dt * t)).Add(tmp.Set(p2).Scl(t * t));
        }
        
        /// <summary>
        /// Quadratic Bezier curve derivative
        /// </summary>
        /// <param name="alist"> The <seealso cref="System.Collections.ArrayList"/> to set to the result. </param>
        /// <param name="t"> The location (ranging 0..1) on the curve. </param>
        /// <param name="p0"> The first bezier point. </param>
        /// <param name="p1"> The second bezier point. </param>
        /// <param name="p2"> The third bezier point. </param>
        /// <param name="tmp"> A temporary vector to be used by the calculation. </param>
        /// <returns> The value specified by out for chaining  </returns>
        public static T QuadraticDerivative<T2>(in T alist, in float t, in T p0, in T p1, in T p2, in T tmp) where T2 : List<T>
        {
            // B2'(t) = 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1)
            var dt = 1f - t;
            return alist.Set(p1).Sub(p0).Scl(2).Scl(1 - t).Add(tmp.Set(p2).Sub(p1).Scl(t).Scl(2));
        }
    }
}
