using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maths;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public interface IPath<T>
{
    T DerivativeAt( in T outParam, in float t );

    /// <summary>
    /// </summary>
    /// <returns>The value of the path at t where <tt>0 &lt;= t &lt;= 1.</tt></returns>
    T ValueAt( in T value, in float t );

    /// <summary>
    /// </summary>
    /// <returns>
    /// The approximated value (between 0 and 1) on the path which is closest
    /// to the specified value. Note that the implementation of this method
    /// might be optimized for speed against precision.
    /// See <see cref="Locate"/> for a more precise (but more
    /// intensive) method.
    /// </returns>
    float Approximate( in T v );

    /// <summary>
    /// </summary>
    /// <returns>
    /// The precise location (between 0 and 1) on the path which is closest
    /// to the specified value. Note that the implementation of this method
    /// might be CPU intensive. see <see cref="Approximate"/> for a
    /// faster (but less precise) method.
    /// </returns>
    float Locate( T v );

    /// <summary>
    /// </summary>
    /// <param name="samples">
    /// The amount of divisions used to approximate length. Higher values
    /// will produce more precise results but will be more CPU intensive.
    /// </param>
    /// <returns>
    /// An approximated length of the spline through sampling the curve and
    /// accumulating the euclidean distances between the sample points.
    /// </returns>
    float ApproxLength( int samples );
}