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

using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Maths;

/// <summary>
/// Implementation of the Bezier curve.
/// </summary>
/// <typeparam name="T">The type of the points on the Bezier curve, which must implement IVector&lt;T&gt;.</typeparam>
[PublicAPI]
public class Bezier< T > : IPath< T > where T : IVector< T >
{
    /// <summary>
    /// Gets or sets the control points of the Bezier curve.
    /// </summary>
    public List< T > Points { get; set; } = [ ];

    private T? _tmp  = default( T );
    private T? _tmp2 = default( T );
    private T? _tmp3 = default( T );

    // ------------------------------------------------------------------------
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Bezier{T}"/> class.
    /// </summary>
    public Bezier()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bezier{T}"/> class with the specified points.
    /// </summary>
    /// <param name="points">The control points of the Bezier curve.</param>
    public Bezier( params T[] points )
    {
        Set( points );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bezier{T}"/> class with the specified points.
    /// </summary>
    /// <param name="points">The control points of the Bezier curve.</param>
    /// <param name="offset">The offset in the array of points at which to start.</param>
    /// <param name="length">The number of points to use.</param>
    public Bezier( in T[] points, in int offset, in int length )
    {
        Set( points, offset, length );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bezier{T}"/> class with the specified points.
    /// </summary>
    /// <param name="points">The control points of the Bezier curve.</param>
    /// <param name="offset">The offset in the list of points at which to start.</param>
    /// <param name="length">The number of points to use.</param>
    public Bezier( in List< T > points, in int offset, in int length )
    {
        Set( points, offset, length );
    }

    /// <summary>
    /// Calculates the value of the Bezier curve at the specified parameter t.
    /// </summary>
    /// <param name="o">The vector to store the result.</param>
    /// <param name="t">The parameter along the curve, ranging from 0 to 1.</param>
    /// <returns>The vector at the specified parameter t.</returns>
    public T ValueAt( in T o, in float t )
    {
        var n = Points.Count;

        if ( n == 2 )
        {
            Linear( o, t, Points[ 0 ], Points[ 1 ], _tmp );
        }
        else if ( n == 3 )
        {
            Quadratic( o, t, Points[ 0 ], Points[ 1 ], Points[ 2 ], _tmp );
        }
        else if ( n == 4 )
        {
            Cubic( o, t, Points[ 0 ], Points[ 1 ], Points[ 2 ], Points[ 3 ], _tmp );
        }

        return o;
    }

    /// <summary>
    /// Calculates the derivative of the Bezier curve at the specified parameter t.
    /// </summary>
    /// <param name="o">The vector to store the result.</param>
    /// <param name="t">The parameter along the curve, ranging from 0 to 1.</param>
    /// <returns>The derivative at the specified parameter t.</returns>
    public T DerivativeAt( in T o, in float t )
    {
        var n = Points.Count;

        if ( n == 2 )
        {
            LinearDerivative( o, t, Points[ 0 ], Points[ 1 ], _tmp );
        }
        else if ( n == 3 )
        {
            QuadraticDerivative( o, t, Points[ 0 ], Points[ 1 ], Points[ 2 ], _tmp );
        }
        else if ( n == 4 )
        {
            CubicDerivative( o, t, Points[ 0 ], Points[ 1 ], Points[ 2 ], Points[ 3 ], _tmp );
        }

        return o;
    }

    /// <summary>
    /// Approximates the position on the curve closest to the specified vector.
    /// </summary>
    /// <param name="v">The vector to approximate.</param>
    /// <returns>The parameter t along the curve closest to the vector.</returns>
    public float Approximate( in T v )
    {
        // TODO: make a real approximate method
        var p1 = Points[ 0 ];
        var p2 = Points[ Points.Count - 1 ];
        var p3 = v;

        var l1Sqr = p1.Distance2( p2 );
        var l2Sqr = p3.Distance2( p2 );
        var l3Sqr = p3.Distance2( p1 );

        var l1 = ( float ) Math.Sqrt( l1Sqr );
        var s  = ( ( l2Sqr + l1Sqr ) - l3Sqr ) / ( 2 * l1 );

        return MathUtils.Clamp( ( l1 - s ) / l1, 0f, 1f );
    }

    /// <summary>
    /// Locates the position on the curve closest to the specified vector.
    /// </summary>
    /// <param name="v">The vector to locate.</param>
    /// <returns>The parameter t along the curve closest to the vector.</returns>
    public float Locate( T v )
    {
        // TODO implement a precise method
        return Approximate( v );
    }

    /// <summary>
    /// Approximates the length of the Bezier curve using a specified number of samples.
    /// </summary>
    /// <param name="samples">The number of samples to use.</param>
    /// <returns>The approximate length of the curve.</returns>
    public float ApproxLength( int samples )
    {
        float tempLength = 0;

        if ( _tmp2 == null )
        {
            return tempLength;
        }

        if ( _tmp3 == null )
        {
            return tempLength;
        }

        for ( var i = 0; i < samples; ++i )
        {
            _tmp2.Set( _tmp3 );

            ValueAt( _tmp3, i / ( ( float ) samples - 1 ) );

            if ( i > 0 )
            {
                tempLength += _tmp2.Distance( _tmp3 );
            }
        }

        return tempLength;
    }

    /// <summary>
    /// Sets the control points of the Bezier curve.
    /// </summary>
    /// <param name="points">The control points of the Bezier curve.</param>
    /// <returns>The current instance for chaining.</returns>
    public Bezier< T > Set( params T[] points )
    {
        return Set( points, 0, points.Length );
    }

    /// <summary>
    /// Sets the control points of the Bezier curve with the specified offset and length.
    /// </summary>
    /// <param name="points">The control points of the Bezier curve.</param>
    /// <param name="offset">The offset in the array of points at which to start.</param>
    /// <param name="length">The number of points to use.</param>
    /// <returns>The current instance for chaining.</returns>
    /// <exception cref="GdxRuntimeException">Thrown if the length is not between 2 and 4.</exception>
    public Bezier< T > Set( in T[] points, in int offset, in int length )
    {
        if ( length is < 2 or > 4 )
        {
            throw new GdxRuntimeException( "Only first, second and third degree Bezier curves are supported." );
        }

        _tmp  ??= points[ 0 ].Cpy();
        _tmp2 ??= points[ 0 ].Cpy();
        _tmp3 ??= points[ 0 ].Cpy();

        Points.Clear();

        Points.AddRange( points.Skip( offset ).Take( length ) );

        return this;
    }

    /// <summary>
    /// Sets the control points of the Bezier curve with the specified offset and length.
    /// </summary>
    /// <param name="points">The control points of the Bezier curve.</param>
    /// <param name="offset">The offset in the list of points at which to start.</param>
    /// <param name="length">The number of points to use.</param>
    /// <returns>The current instance for chaining.</returns>
    /// <exception cref="GdxRuntimeException">Thrown if the length is not between 2 and 4.</exception>
    public Bezier< T > Set( in List< T > points, in int offset, in int length )
    {
        if ( length is < 2 or > 4 )
        {
            throw new GdxRuntimeException( "Only first, second and third degree Bezier curves are supported." );
        }

        _tmp  ??= points[ 0 ].Cpy();
        _tmp2 ??= points[ 0 ].Cpy();
        _tmp3 ??= points[ 0 ].Cpy();

        Points.Clear();
        Points.AddRange( points.Skip( offset ).Take( length ) );

        return this;
    }

    /// <summary>
    /// Simple linear interpolation.
    /// </summary>
    /// <param name="alist">The vector to store the result.</param>
    /// <param name="t">The parameter along the line, ranging from 0 to 1.</param>
    /// <param name="p0">The start point.</param>
    /// <param name="p1">The end point.</param>
    /// <param name="tmp">A temporary vector to be used by the calculation.</param>
    /// <returns>The interpolated value.</returns>
    public static T Linear( in T alist, in float t, in T p0, in T p1, in T? tmp )
    {
        // B1(t) = p0 + (p1 - p0) * t
        return alist.Set( p0 ).Scale( 1f - t ).Add( tmp!.Set( p1 ).Scale( t ) );
    }

    /// <summary>
    /// Simple linear interpolation derivative.
    /// </summary>
    /// <param name="vec">The vector to store the result.</param>
    /// <param name="t">The parameter along the line, ranging from 0 to 1.</param>
    /// <param name="p0">The start point.</param>
    /// <param name="p1">The end point.</param>
    /// <param name="tmp">A temporary vector to be used by the calculation.</param>
    /// <returns>The derivative value.</returns>
    public static T LinearDerivative( in T vec, in float t, in T p0, in T p1, in T? tmp )
    {
        // B1'(t) = p1 - p0
        return vec.Set( p1 ).Sub( p0 );
    }

    /// <summary>
    /// Quadratic Bezier curve interpolation.
    /// </summary>
    /// <param name="list">The vector to store the result.</param>
    /// <param name="t">The parameter along the curve, ranging from 0 to 1.</param>
    /// <param name="p0">The first control point.</param>
    /// <param name="p1">The second control point.</param>
    /// <param name="p2">The third control point.</param>
    /// <param name="tmp">A temporary vector to be used by the calculation.</param>
    /// <returns>The interpolated value.</returns>
    public static T Quadratic( in T list, in float t, in T p0, in T p1, in T p2, in T? tmp )
    {
        // B2(t) = (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2
        var dt = 1f - t;

        return list.Set( p0 ).Scale( dt * dt )
                   .Add( tmp!.Set( p1 ).Scale( 2 * dt * t ) )
                   .Add( tmp.Set( p2 ).Scale( t * t ) );
    }

    /// <summary>
    /// Quadratic Bezier curve derivative.
    /// </summary>
    /// <param name="alist">The vector to store the result.</param>
    /// <param name="t">The parameter along the curve, ranging from 0 to 1.</param>
    /// <param name="p0">The first control point.</param>
    /// <param name="p1">The second control point.</param>
    /// <param name="p2">The third control point.</param>
    /// <param name="tmp">A temporary vector to be used by the calculation.</param>
    /// <returns>The derivative value.</returns>
    public static T QuadraticDerivative( in T alist, in float t, in T p0, in T p1, in T p2, in T? tmp )
    {
        // B2'(t) = 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1)
        return alist.Set( p1 ).Sub( p0 ).Scale( 2 ).Scale( 1 - t )
                    .Add( tmp!.Set( p2 ).Sub( p1 ).Scale( t ).Scale( 2 ) );
    }

    /// <summary>
    /// Cubic Bezier curve interpolation.
    /// </summary>
    /// <param name="alist">The vector to store the result.</param>
    /// <param name="t">The parameter along the curve, ranging from 0 to 1.</param>
    /// <param name="p0">The first control point.</param>
    /// <param name="p1">The second control point.</param>
    /// <param name="p2">The third control point.</param>
    /// <param name="p3">The fourth control point.</param>
    /// <param name="tmp">A temporary vector to be used by the calculation.</param>
    /// <returns>The interpolated value.</returns>
    public static T Cubic( in T alist, in float t, in T p0, in T p1, in T p2, in T p3, in T? tmp )
    {
        // B3(t) = (1-t) * (1-t) * (1-t) * p0 + 3 * (1-t) * (1-t) * t * p1 + 3 * (1-t) * t * t * p2 + t * t * t * p3
        var dt  = 1f - t;
        var dt2 = dt * dt;
        var t2  = t * t;

        return alist.Set( p0 ).Scale( dt * dt2 )
                    .Add( tmp!.Set( p1 ).Scale( 3 * dt2 * t ) )
                    .Add( tmp.Set( p2 ).Scale( 3 * dt * t2 ) )
                    .Add( tmp.Set( p3 ).Scale( t * t2 ) );
    }

    /// <summary>
    /// Cubic Bezier curve derivative.
    /// </summary>
    /// <param name="alist">The vector to store the result.</param>
    /// <param name="t">The parameter along the curve, ranging from 0 to 1.</param>
    /// <param name="p0">The first control point.</param>
    /// <param name="p1">The second control point.</param>
    /// <param name="p2">The third control point.</param>
    /// <param name="p3">The fourth control point.</param>
    /// <param name="tmp">A temporary vector to be used by the calculation.</param>
    /// <returns>The derivative value.</returns>
    public static T CubicDerivative( in T alist, in float t, in T p0, in T p1, in T p2, in T p3, in T? tmp )
    {
        // B3'(t) = 3 * (1-t) * (1-t) * (p1 - p0) + 6 * (1 - t) * t * (p2 - p1) + 3 * t * t * (p3 - p2)
        var dt  = 1f - t;
        var dt2 = dt * dt;
        var t2  = t * t;

        return alist.Set( p1 ).Sub( p0 ).Scale( dt2 * 3 )
                    .Add( tmp!.Set( p2 ).Sub( p1 ).Scale( dt * t * 6 ) )
                    .Add( tmp.Set( p3 ).Sub( p2 ).Scale( t2 * 3 ) );
    }
}
