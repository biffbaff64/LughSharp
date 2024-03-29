﻿// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Utils.Collections.Extensions;

namespace LughSharp.LibCore.Maths;

/// <summary>
///     Implementation of the Bezier curve.
/// </summary>
public class Bezier<T> : IPath< T > where T : IVector< T >
{
    private T? _tmp  = default( T );
    private T? _tmp2 = default( T );
    private T? _tmp3 = default( T );

    /// <summary>
    /// </summary>
    public Bezier()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public Bezier( params T[] points )
    {
        Set( points );
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    public Bezier( in T[] points, in int offset, in int length )
    {
        Set( points, offset, length );
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    public Bezier( in List< T > points, in int offset, in int length )
    {
        Set( points, offset, length );
    }

    public List< T > Points { get; set; } = new();

    /// <summary>
    /// </summary>
    /// <param name="o"></param>
    /// <param name="t"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="o"></param>
    /// <param name="t"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Approximate( in T v )
    {
        // TODO: make a real approximate method
        T p1 = Points[ 0 ];
        T p2 = Points[ Points.Count - 1 ];
        T p3 = v;

        var l1Sqr = p1.Dst2( p2 );
        var l2Sqr = p3.Dst2( p2 );
        var l3Sqr = p3.Dst2( p1 );

        var l1 = ( float )Math.Sqrt( l1Sqr );
        var s  = ( ( l2Sqr + l1Sqr ) - l3Sqr ) / ( 2 * l1 );

        return MathUtils.Clamp( ( l1 - s ) / l1, 0f, 1f );
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Locate( T v )
    {
        // TODO implement a precise method
        return Approximate( v );
    }

    /// <summary>
    /// </summary>
    /// <param name="samples"></param>
    /// <returns></returns>
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

            ValueAt( _tmp3, i / ( ( float )samples - 1 ) );

            if ( i > 0 )
            {
                tempLength += _tmp2.Dst( _tmp3 );
            }
        }

        return tempLength;
    }

    /// <summary>
    ///     Simple linear interpolation
    /// </summary>
    /// <param name="alist"> The collection in which to set to the result.</param>
    /// <param name="t"> The location (ranging 0..1) on the line.</param>
    /// <param name="p0"> The start point.</param>
    /// <param name="p1"> The end point.</param>
    /// <param name="tmp"> A temporary vector to be used by the calculation.</param>
    /// <returns> The value specified by out for chaining </returns>
    public static T Linear( in T alist, in float t, in T p0, in T p1, in T? tmp )
    {
        // B1(t) = p0 + (p1 - p0) * t
        return alist.Set( p0 ).Scl( 1f - t ).Add( tmp!.Set( p1 ).Scl( t ) );
    }

    /// <summary>
    ///     Simple linear interpolation derivative
    /// </summary>
    /// <param name="vec"> The collection in which to set to the result.</param>
    /// <param name="t"> The location (ranging 0..1) on the line.</param>
    /// <param name="p0"> The start point.</param>
    /// <param name="p1"> The end point.</param>
    /// <param name="tmp"> A temporary vector to be used by the calculation.</param>
    /// <returns> The value specified by out for chaining</returns>
    public static T LinearDerivative( in T vec, in float t, in T p0, in T p1, in T? tmp )
    {
        // B1'(t) = p1 - p0
        return vec.Set( p1 ).Sub( p0 );
    }

    /// <summary>
    ///     Quadratic Bezier curve
    /// </summary>
    /// <param name="list"> The collection in which to set to the result.</param>
    /// <param name="t"> The location (ranging 0..1) on the curve. </param>
    /// <param name="p0"> The first bezier point. </param>
    /// <param name="p1"> The second bezier point. </param>
    /// <param name="p2"> The third bezier point. </param>
    /// <param name="tmp"> A temporary vector to be used by the calculation. </param>
    /// <returns> The value specified by out for chaining  </returns>
    public static T Quadratic( in T list, in float t, in T p0, in T p1, in T p2, in T? tmp )
    {
        // B2(t) = (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2
        var dt = 1f - t;

        return list.Set( p0 ).Scl( dt * dt )
                   .Add( tmp!.Set( p1 ).Scl( 2 * dt * t ) )
                   .Add( tmp.Set( p2 ).Scl( t * t ) );
    }

    /// <summary>
    ///     Quadratic Bezier curve derivative
    /// </summary>
    /// <param name="alist"> The collection in which to set to the result.</param>
    /// <param name="t"> The location (ranging 0..1) on the curve. </param>
    /// <param name="p0"> The first bezier point. </param>
    /// <param name="p1"> The second bezier point. </param>
    /// <param name="p2"> The third bezier point. </param>
    /// <param name="tmp"> A temporary vector to be used by the calculation. </param>
    /// <returns> The value specified by out for chaining  </returns>
    public static T QuadraticDerivative( in T alist, in float t, in T p0, in T p1, in T p2, in T? tmp )
    {
        // B2'(t) = 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1)
        //            var dt = 1f - t;
        return alist.Set( p1 ).Sub( p0 ).Scl( 2 ).Scl( 1 - t )
                    .Add( tmp!.Set( p2 ).Sub( p1 ).Scl( t ).Scl( 2 ) );
    }

    /// <summary>
    ///     Cubic Bezier curve
    /// </summary>
    /// <param name="alist"> The collection in which to set to the result.</param>
    /// <param name="t"> The location (ranging 0..1) on the curve.</param>
    /// <param name="p0"> The first bezier point. </param>
    /// <param name="p1"> The second bezier point. </param>
    /// <param name="p2"> The third bezier point. </param>
    /// <param name="p3"> The fourth bezier point. </param>
    /// <param name="tmp"> A temporary vector to be used by the calculation. </param>
    /// <returns> The value specified by out for chaining  </returns>
    public static T Cubic( in T alist, in float t, in T p0, in T p1, in T p2, in T p3, in T? tmp )
    {
        // B3(t) = (1-t) * (1-t) * (1-t) * p0 + 3 * (1-t) * (1-t) * t * p1 + 3 * (1-t) * t * t * p2 + t * t * t * p3
        var dt  = 1f - t;
        var dt2 = dt * dt;
        var t2  = t * t;

        return alist.Set( p0 ).Scl( dt2 * dt )
                    .Add( tmp!.Set( p1 ).Scl( 3 * dt2 * t ) )
                    .Add( tmp.Set( p2 ).Scl( 3 * dt * t2 ) )
                    .Add( tmp.Set( p3 ).Scl( t2 * t ) );
    }

    /// <summary>
    ///     Cubic Bezier curve derivative
    /// </summary>
    /// <param name="alist"> The collection in which to set to the result.</param>
    /// <param name="t"> The location (ranging 0..1) on the curve. </param>
    /// <param name="p0"> The first bezier point. </param>
    /// <param name="p1"> The second bezier point. </param>
    /// <param name="p2"> The third bezier point. </param>
    /// <param name="p3"> The fourth bezier point. </param>
    /// <param name="tmp"> A temporary vector to be used by the calculation. </param>
    /// <returns> The value specified by out for chaining  </returns>
    public static T CubicDerivative( in T alist, in float t, in T p0, in T p1, in T p2, in T p3, in T? tmp )
    {
        // B3'(t) = 3 * (1-t) * (1-t) * (p1 - p0) + 6 * (1 - t) * t * (p2 - p1) + 3 * t * t * (p3 - p2)
        var dt  = 1f - t;
        var dt2 = dt * dt;
        var t2  = t * t;

        return alist.Set( p1 ).Sub( p0 ).Scl( dt2 * 3 )
                    .Add( tmp!.Set( p2 ).Sub( p1 ).Scl( dt * t * 6 ) )
                    .Add( tmp.Set( p3 ).Sub( p2 ).Scl( t2 * 3 ) );
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public Bezier< T > Set( params T[] points )
    {
        return Set( points, 0, points.Length );
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
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

        Points.AddAll( points, offset, length );

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
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
        Points.AddAll( points, offset, length );

        return this;
    }
}
