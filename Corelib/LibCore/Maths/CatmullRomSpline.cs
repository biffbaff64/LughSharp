// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


namespace Corelib.LibCore.Maths;

[PublicAPI]
public class CatmullRomSpline< T > : IPath< T > where T : IVector< T >
{
    // ========================================================================

    private T? _tmp;
    private T? _tmp2;
    private T? _tmp3;

    // ========================================================================

    public CatmullRomSpline()
    {
    }

    public CatmullRomSpline( T[] controlPoints, bool continuous )
    {
        Set( controlPoints, continuous );
    }

    public T[]  ControlPoints { get; set; } = default( T[] )!;
    public bool Continuous    { get; set; }
    public int  SpanCount     { get; set; }

    public T ValueAt( in T outp, in float t )
    {
        var n = SpanCount;
        var u = t * n;
        var i = t >= 1f ? n - 1 : ( int ) u;
        u -= i;

        return ValueAt( outp, i, u );
    }

    public T DerivativeAt( in T outp, in float t )
    {
        var n = SpanCount;
        var u = t * n;
        var i = t >= 1f ? n - 1 : ( int ) u;
        u -= i;

        return DerivativeAt( outp, i, u );
    }

    public float Approximate( in T v )
    {
        return Approximate( v, Nearest( v ) );
    }

    public float Locate( T v )
    {
        return Approximate( v );
    }

    public float ApproxLength( int samples )
    {
        float tempLength = 0;

        if ( ( _tmp2 != null ) && ( _tmp3 != null ) )
        {
            for ( var i = 0; i < samples; ++i )
            {
                _tmp2.Set( _tmp3 );
                ValueAt( _tmp3, i / ( ( float ) samples - 1 ) );

                if ( i > 0 )
                {
                    tempLength += _tmp2.Distance( _tmp3 );
                }
            }
        }

        return tempLength;
    }

    public CatmullRomSpline< T > Set( T[] controlPoints, bool continuous )
    {
        _tmp  ??= controlPoints[ 0 ].Cpy();
        _tmp2 ??= controlPoints[ 0 ].Cpy();
        _tmp3 ??= controlPoints[ 0 ].Cpy();

        ControlPoints = controlPoints;
        Continuous    = continuous;
        SpanCount     = continuous ? controlPoints.Length : controlPoints.Length - 3;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <returns> The value of the spline at position u of the specified span. </returns>
    public T ValueAt( T outp, int span, float u )
    {
        return Calculate( outp,
                          Continuous ? span : span + 1,
                          u,
                          ControlPoints,
                          Continuous,
                          _tmp );
    }

    /// <summary>
    /// </summary>
    /// <returns> The derivative of the spline at position u of the specified span </returns>
    public T DerivativeAt( T outp, int span, float u )
    {
        return Derivative( outp,
                           Continuous ? span : span + 1,
                           u,
                           ControlPoints,
                           Continuous,
                           _tmp );
    }

    /// <summary>
    /// </summary>
    /// <returns> The span closest to the specified value. </returns>
    public int Nearest( T inp )
    {
        return Nearest( inp, 0, SpanCount );
    }

    /// <summary>
    /// </summary>
    /// <returns>
    /// The span closest to the specified value, restricting to the specified spans.
    /// </returns>
    public int Nearest( T inp, int start, int count )
    {
        while ( start < 0 )
        {
            start += SpanCount;
        }

        var result = start % SpanCount;
        var dst    = inp.Distance2( ControlPoints[ result ] );

        for ( var i = 1; i < count; i++ )
        {
            var idx = ( start + i ) % SpanCount;
            var d   = inp.Distance2( ControlPoints[ idx ] );

            if ( d < dst )
            {
                dst    = d;
                result = idx;
            }
        }

        return result;
    }

    public float Approximate( in T inp, int start, int count )
    {
        return Approximate( inp, Nearest( inp, start, count ) );
    }

    public float Approximate( in T inp, int near )
    {
        var n        = near;
        var nearest  = ControlPoints[ n ];
        var previous = ControlPoints[ n > 0 ? n - 1 : SpanCount - 1 ];
        var next     = ControlPoints[ ( n + 1 ) % SpanCount ];
        var dstPrev2 = inp.Distance2( previous );
        var dstNext2 = inp.Distance2( next );

        T p1, p2, p3;

        if ( dstNext2 < dstPrev2 )
        {
            p1 = nearest;
            p2 = next;
            p3 = inp;
        }
        else
        {
            p1 = previous;
            p2 = nearest;
            p3 = inp;
            n  = n > 0 ? n - 1 : SpanCount - 1;
        }

        var l1Sqr = p1.Distance2( p2 );
        var l2Sqr = p3.Distance2( p2 );
        var l3Sqr = p3.Distance2( p1 );
        var l1    = ( float ) Math.Sqrt( l1Sqr );
        var s     = ( ( l2Sqr + l1Sqr ) - l3Sqr ) / ( 2f * l1 );
        var u     = MathUtils.Clamp( ( l1 - s ) / l1, 0f, 1f );

        return ( n + u ) / SpanCount;
    }

    /// <summary>
    /// Calculates the catmullrom value for the given position (t).
    /// </summary>
    /// <param name="outvec"> The Vector to Set to the result. </param>
    /// <param name="t"> The position (0 &lt;= t &lt;= 1) on the spline </param>
    /// <param name="points"> The control points </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns></returns>
    public static T Calculate( T outvec, float t, T[] points, bool continuous, T tmp )
    {
        var n = continuous ? points.Length : points.Length - 3;
        var u = t * n;
        var i = t >= 1f ? n - 1 : ( int ) u;

        u -= i;

        return Calculate( outvec, i, u, points, continuous, tmp );
    }

    /// <summary>
    /// Calculates the catmullrom value for the given span (i) at the given position (u).
    /// </summary>
    /// <param name="outp"> The Vector to Set to the result. </param>
    /// <param name="i">
    /// The span (0&lt;=i&lt;spanCount) spanCount = continuous ? points.Length : points.Length - degree
    /// </param>
    /// <param name="u"> The position (0 &lt;= u &lt;= 1) on the span </param>
    /// <param name="points"> The control points </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of outp </returns>
    public static T Calculate( T outp,
                               int i,
                               float u,
                               T[] points,
                               bool continuous,
                               T? tmp )
    {
        var n  = points.Length;
        var u2 = u * u;

        var u3 = u2 * u;
        outp.Set( points[ i ] ).Scale( ( ( 1.5f * u3 ) - ( 2.5f * u2 ) ) + 1.0f );

        if ( ( tmp != null ) && ( continuous || ( i > 0 ) ) )
        {
            outp.Add( tmp.Set( points[ ( ( n + i ) - 1 ) % n ] ).Scale( ( ( -0.5f * u3 ) + u2 ) - ( 0.5f * u ) ) );
        }

        if ( ( tmp != null ) && ( continuous || ( i < ( n - 1 ) ) ) )
        {
            outp.Add( tmp.Set( points[ ( i + 1 ) % n ] ).Scale( ( -1.5f * u3 ) + ( 2f * u2 ) + ( 0.5f * u ) ) );
        }

        if ( ( tmp != null ) && ( continuous || ( i < ( n - 2 ) ) ) )
        {
            outp.Add( tmp.Set( points[ ( i + 2 ) % n ] ).Scale( ( 0.5f * u3 ) - ( 0.5f * u2 ) ) );
        }

        return outp;
    }

    /// <summary>
    /// Calculates the derivative of the catmullrom spline for the given position (t).
    /// </summary>
    /// <param name="outp"> The Vector to Set to the result. </param>
    /// <param name="t"> The position (0&lt;=t&lt;=1) on the spline </param>
    /// <param name="points"> The control points </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of outp </returns>
    public static T Derivative( T outp,
                                float t,
                                T[] points,
                                bool continuous,
                                T tmp )
    {
        var n = continuous ? points.Length : points.Length - 3;
        var u = t * n;
        var i = t >= 1f ? n - 1 : ( int ) u;
        u -= i;

        return Derivative( outp, i, u, points, continuous, tmp );
    }

    /// <summary>
    /// Calculates the derivative of the catmullrom spline for the given
    /// span (i) at the given position (u).
    /// </summary>
    /// <param name="outp"> The Vector to Set to the result. </param>
    /// <param name="i">
    /// The span (0&lt;=i&lt;spanCount) spanCount = continuous ? points.Length : points.Length - degree
    /// </param>
    /// <param name="u"> The position (0&lt;=u&lt;=1) on the span </param>
    /// <param name="points"> The control points </param>
    /// <param name="continuous"> If true the b-spline restarts at 0 when reaching 1 </param>
    /// <param name="tmp"> A temporary vector used for the calculation </param>
    /// <returns> The value of outp </returns>
    public static T Derivative( T outp,
                                int i,
                                float u,
                                T[] points,
                                bool continuous,
                                T? tmp )
    {
        /*
         * catmull'(u) = 0.5 *((-p0 + p2) + 2 * (2*p0 - 5*p1 + 4*p2 - p3) * u + 3 * (-p0 + 3*p1 - 3*p2 + p3) * u * u)
         */
        var n = points.Length;

        var u2 = u * u;

        // float u3 = u2 * u;
        outp.Set( points[ i ] ).Scale( ( -u * 5 ) + ( u2 * 4.5f ) );

        if ( ( tmp != null ) && ( continuous || ( i > 0 ) ) )
        {
            outp.Add( tmp.Set( points[ ( ( n + i ) - 1 ) % n ] )
                         .Scale( ( -0.5f + ( u * 2 ) ) - ( u2 * 1.5f ) ) );
        }

        if ( ( tmp != null ) && ( continuous || ( i < ( n - 1 ) ) ) )
        {
            outp.Add( tmp.Set( points[ ( i + 1 ) % n ] )
                         .Scale( ( 0.5f + ( u * 4 ) ) - ( u2 * 4.5f ) ) );
        }

        if ( ( tmp != null ) && ( continuous || ( i < ( n - 2 ) ) ) )
        {
            outp.Add( tmp.Set( points[ ( i + 2 ) % n ] )
                         .Scale( -u + ( u2 * 1.5f ) ) );
        }

        return outp;
    }
}
