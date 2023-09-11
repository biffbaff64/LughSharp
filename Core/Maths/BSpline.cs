// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Maths;

[PublicAPI]
public class BSpline<T> : IPath< T > where T : IVector< T >
{
    private const float D6 = 1f / 6f;

    public T[]?        ControlPoints { get; set; }
    public List< T >? Knots         { get; set; }
    public int        Degree        { get; set; }
    public bool       Continuous    { get; set; }
    public int        SpanCount     { get; set; }

    private T? _tmp;
    private T? _tmp2;
    private T? _tmp3;

    public BSpline()
    {
    }

    public BSpline( T[] controlPoints, int degree, bool continuous )
    {
        Set( controlPoints, degree, continuous );
    }

    /// <summary>
    /// Calculates the cubic b-spline value for the given position (t).
    /// </summary>
    /// <param name="output"> The Vector to set to the result. </param>
    /// <param name="t"> The position (0&lt;=t&lt;=1) on the spline </param>
    /// <param name="points"> The control points </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of out  </returns>
    public static T Cubic( T output, float t, T[] points, bool continuous, T tmp )
    {
        var n = continuous ? points.Length : points.Length - 3;
        var u = t * n;
        var i = ( t >= 1f ) ? ( n - 1 ) : ( int )u;

        u -= i;

        return Cubic( output, i, u, points, continuous, tmp );
    }

    /// <summary>
    /// Calculates the cubic b-spline derivative for the given position (t).
    /// </summary>
    /// <param name="output"> The Vector to set to the result. </param>
    /// <param name="t"> The position (0&lt;=t&lt;=1) on the spline </param>
    /// <param name="points"> The control points </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of out  </returns>
    public static T CubicDerivative( T output, float t, T[] points, bool continuous, T tmp )
    {
        var n = continuous ? points.Length : points.Length - 3;
        var u = t * n;
        var i = ( t >= 1f ) ? ( n - 1 ) : ( int )u;

        u -= i;

        return Cubic( output, i, u, points, continuous, tmp );
    }

    /// <summary>
    /// Calculates the cubic b-spline value for the given span (i) at the given position (u).
    /// </summary>
    /// <param name="output"> The Vector to set to the result. </param>
    /// <param name="i">
    /// The span (0&lt;=i&lt;spanCount) spanCount = continuous ? points.length : points.length - 3 (cubic degree)
    /// </param>
    /// <param name="u"> The position (0&lt;=u&lt;=1) on the span </param>
    /// <param name="points"> The control points </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of out  </returns>
    public static T Cubic( T output, int i, float u, T[] points, bool continuous, T tmp )
    {
        var n  = points.Length;
        var dt = 1f - u;
        var t2 = u * u;
        var t3 = t2 * u;

        output.Set( points[ i ] ).Scl( ( ( ( 3f * t3 ) - ( 6f * t2 ) ) + 4f ) * D6 );

        if ( continuous || ( i > 0 ) )
        {
            output.Add
                (
                 tmp.Set( points[ ( ( n + i ) - 1 ) % n ] )
                    .Scl( dt * dt * dt * D6 )
                );
        }

        if ( continuous || ( i < ( n - 1 ) ) )
        {
            output.Add
                (
                 tmp.Set( points[ ( i + 1 ) % n ] )
                    .Scl( ( ( -3f * t3 ) + ( 3f * t2 ) + ( 3f * u ) + 1f ) * D6 )
                );
        }

        if ( continuous || ( i < ( n - 2 ) ) )
        {
            output.Add( tmp.Set( points[ ( i + 2 ) % n ] ).Scl( t3 * D6 ) );
        }

        return output;
    }

    /// <summary>
    /// Calculates the cubic b-spline derivative for the given span (i) at the given position (u).
    /// </summary>
    /// <param name="output"> The Vector to set to the result. </param>
    /// <param name="i">
    /// The span (0&lt;=i&lt;spanCount) spanCount = continuous ? points.length : points.length - 3 (cubic degree)
    /// </param>
    /// <param name="u"> The position (0&lt;=u&lt;=1) on the span </param>
    /// <param name="points"> The control points </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of out  </returns>
    public static T CubicDerivative( T output, int i, float u, T[] points, bool continuous, T tmp )
    {
        var n  = points.Length;
        var dt = 1f - u;
        var t2 = u * u;

        output.Set( points[ i ] ).Scl( ( 1.5f * t2 ) - ( 2 * u ) );

        if ( continuous || ( i > 0 ) )
        {
            output.Add
                (
                 tmp.Set( points[ ( ( n + i ) - 1 ) % n ] )
                    .Scl( -0.5f * dt * dt )
                );
        }

        if ( continuous || ( i < ( n - 1 ) ) )
        {
            output.Add
                (
                 tmp.Set( points[ ( i + 1 ) % n ] )
                    .Scl( ( -1.5f * t2 ) + u + 0.5f )
                );
        }

        if ( continuous || ( i < ( n - 2 ) ) )
        {
            output.Add( tmp.Set( points[ ( i + 2 ) % n ] ).Scl( 0.5f * t2 ) );
        }

        return output;
    }

    /// <summary>
    /// Calculates the n-degree b-spline value for the given position (t).
    /// </summary>
    /// <param name="output"> The Vector to set to the result. </param>
    /// <param name="t"> The position (0&lt;=t&lt;=1) on the spline </param>
    /// <param name="points"> The control points </param>
    /// <param name="degree"> The degree of the b-spline </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of out  </returns>
    public static T Calculate( T output, float t, T[] points, int degree, bool continuous, T tmp )
    {
        var n = continuous ? points.Length : points.Length - degree;
        var u = t * n;
        var i = ( t >= 1f ) ? ( n - 1 ) : ( int )u;

        u -= i;

        return Calculate( output, i, u, points, degree, continuous, tmp );
    }

    /// <summary>
    /// Calculates the n-degree b-spline derivative for the given position (t).
    /// </summary>
    /// <param name="output"> The Vector to set to the result. </param>
    /// <param name="t"> The position (0&lt;=t&lt;=1) on the spline </param>
    /// <param name="points"> The control points </param>
    /// <param name="degree"> The degree of the b-spline </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of out  </returns>
    public static T Derivative( T output, float t, T[] points, int degree, bool continuous, T tmp )
    {
        var n = continuous ? points.Length : points.Length - degree;
        var u = t * n;
        var i = ( t >= 1f ) ? ( n - 1 ) : ( int )u;

        u -= i;

        return Derivative( output, i, u, points, degree, continuous, tmp );
    }

    /// <summary>
    /// Calculates the n-degree b-spline value for the given span (i) at the given position (u).
    /// </summary>
    /// <param name="output"> The Vector to set to the result. </param>
    /// <param name="i">
    /// The span (0&lt;=i&lt;spanCount) spanCount = continuous ? points.length : points.length - degree
    /// </param>
    /// <param name="u"> The position (0&lt;=u&lt;=1) on the span </param>
    /// <param name="points"> The control points </param>
    /// <param name="degree"> The degree of the b-spline, only 3 is supported </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of out  </returns>
    public static T Calculate( T output, int i, float u, T[] points, int degree, bool continuous, T tmp )
    {
        if ( degree == 3 )
        {
            return Cubic( output, i, u, points, continuous, tmp );
        }

        throw new System.ArgumentException();
    }

    /// <summary>
    /// Calculates the n-degree b-spline derivative for the given span (i) at the given position (u). </summary>
    /// <param name="output"> The Vector to set to the result. </param>
    /// <param name="i">
    /// The span (0&lt;=i&lt;spanCount) spanCount = continuous ? points.length : points.length - degree
    /// </param>
    /// <param name="u"> The position (0&lt;=u&lt;=1) on the span </param>
    /// <param name="points"> The control points </param>
    /// <param name="degree"> The degree of the b-spline, only 3 is supported </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of out  </returns>
    public static T Derivative( T output, int i, float u, T[] points, int degree, bool continuous, T tmp )
    {
        if ( degree == 3 )
        {
            return CubicDerivative( output, i, u, points, continuous, tmp );
        }

        throw new System.ArgumentException();
    }

    public BSpline< T > Set( T[] controlPoints, int degree, bool continuous )
    {
        _tmp  ??= controlPoints[ 0 ].Cpy();
        _tmp2 ??= controlPoints[ 0 ].Cpy();
        _tmp3 ??= controlPoints[ 0 ].Cpy();

        this.ControlPoints = controlPoints;
        this.Degree        = degree;
        this.Continuous    = continuous;
        this.SpanCount     = continuous ? controlPoints.Length : controlPoints.Length - degree;

        if ( Knots == null )
        {
            Knots = new List< T >( SpanCount );
        }
        else
        {
            Knots.Clear();
            Knots.EnsureCapacity( SpanCount );
        }

        for ( var i = 0; i < SpanCount; i++ )
        {
            Knots.Add
                (
                 Calculate
                     (
                      controlPoints[ 0 ].Cpy(),
                      continuous ? i : ( int )( i + ( 0.5f * degree ) ),
                      0f,
                      controlPoints,
                      degree,
                      continuous,
                      _tmp
                     )
                );
        }

        return this;
    }

    public T ValueAt( in T output, in float t )
    {
        var n = SpanCount;
        var u = t * n;
        var i = ( t >= 1f ) ? ( n - 1 ) : ( int )u;

        u -= i;

        return ValueAt( output, i, u );
    }

    /// <returns> The value of the spline at position u of the specified span </returns>
    protected virtual T ValueAt( T output, int span, float u )
    {
        return Calculate
            (
             output,
             Continuous ? span : ( span + ( int )( Degree * 0.5f ) ),
             u,
             ControlPoints!,
             Degree,
             Continuous,
             _tmp!
            );
    }

    public virtual T DerivativeAt( in T output, in float t )
    {
        var n = SpanCount;
        var u = t * n;
        var i = ( t >= 1f ) ? ( n - 1 ) : ( int )u;
        
        u -= i;

        return DerivativeAt( output, i, u );
    }

    /// <returns> The derivative of the spline at position u of the specified span </returns>
    protected virtual T DerivativeAt( T output, int span, float u )
    {
        return Derivative
            (
             output,
             Continuous ? span : ( span + ( int )( Degree * 0.5f ) ),
             u,
             ControlPoints!,
             Degree,
             Continuous,
             _tmp!
            );
    }

    /// <returns> The span closest to the specified value </returns>
    protected virtual int Nearest( T input )
    {
        return Nearest( input, 0, SpanCount );
    }

    /// <returns> The span closest to the specified value, restricting to the specified spans. </returns>
    protected virtual int Nearest( T input, int start, int count )
    {
        while ( start < 0 )
        {
            start += SpanCount;
        }

        var result = start % SpanCount;
        var dst    = input.Dst2( Knots![ result ] );

        for ( var i = 1; i < count; i++ )
        {
            var idx = ( start + i ) % SpanCount;
            var d   = input.Dst2( Knots[ idx ] );

            if ( d < dst )
            {
                dst    = d;
                result = idx;
            }
        }

        return result;
    }

    public virtual float Approximate( in T v )
    {
        return Approximate( v, Nearest( v ) );
    }

    public virtual float Approximate( T input, int start, int count )
    {
        return Approximate( input, Nearest( input, start, count ) );
    }

    protected virtual float Approximate( T input, int near )
    {
        var n = near;

        T nearest  = Knots![ n ];
        T previous = Knots[ n > 0 ? n - 1 : SpanCount - 1 ];
        T next     = Knots[ ( n + 1 ) % SpanCount ];

        var dstPrev2 = input.Dst2( previous );
        var dstNext2 = input.Dst2( next );
        T   p1;
        T   p2;
        T   p3;

        if ( dstNext2 < dstPrev2 )
        {
            p1 = nearest;
            p2 = next;
            p3 = input;
        }
        else
        {
            p1 = previous;
            p2 = nearest;
            p3 = input;
            n  = n > 0 ? n - 1 : SpanCount - 1;
        }

        var l1Sqr = p1.Dst2( p2 );
        var l2Sqr = p3.Dst2( p2 );
        var l3Sqr = p3.Dst2( p1 );
        var l1    = ( float )Math.Sqrt( l1Sqr );
        var s     = ( ( l2Sqr + l1Sqr ) - l3Sqr ) / ( 2 * l1 );
        var u     = MathUtils.Clamp( ( l1 - s ) / l1, 0f, 1f );

        return ( n + u ) / SpanCount;
    }

    public float Locate( T v )
    {
        // TODO Add a precise method
        return Approximate( v );
    }

    public float ApproxLength( int samples )
    {
        float tempLength = 0;

        for ( var i = 0; i < samples; ++i )
        {
            if ( _tmp3 != null )
            {
                _tmp2?.Set( _tmp3 );
                
                ValueAt( _tmp3, ( i ) / ( ( float )samples - 1 ) );

                if ( ( _tmp2 != null ) && ( i > 0 ) )
                {
                    tempLength += _tmp2.Dst( _tmp3 );
                }
            }

        }

        return tempLength;
    }
}