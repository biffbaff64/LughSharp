// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Exceptions;
using Exception = System.Exception;

namespace Corelib.Lugh.Maths;

/// <summary>
/// Encapsulates a 2D vector. Allows chaining methods by returning a reference to itself
/// </summary>
[PublicAPI]
public class Vector2 : IVector< Vector2 >
{
    public static readonly Vector2 XDefault = new( 1, 0 );
    public static readonly Vector2 YDefault = new( 0, 1 );
    public static readonly Vector2 Zero     = new( 0, 0 );

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Constructs a new vector at (0,0)
    /// </summary>
    public Vector2() : this( 0, 0 )
    {
    }

    /// <summary>
    /// Constructs a vector with the given components
    /// </summary>
    /// <param name="x">The x-component.</param>
    /// <param name="y">The x-component.</param>
    public Vector2( float x, float y )
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Constructs a vector from the given vector
    /// </summary>
    /// <param name="v">The Vector</param>
    public Vector2( Vector2 v )
    {
        Set( v );
    }

    /// <summary>
    /// The X-Component of this vector.
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// The Y-Component of this vector.
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// Returns a copy of this vector.
    /// </summary>
    public Vector2 Cpy()
    {
        return new Vector2( this );
    }

    public float Len()
    {
        return ( float ) Math.Sqrt( ( X * X ) + ( Y * Y ) );
    }

    public float Len2()
    {
        return ( X * X ) + ( Y * Y );
    }

    public Vector2 Set( Vector2 v )
    {
        X = v.X;
        Y = v.Y;

        return this;
    }

    public Vector2 Sub( Vector2 v )
    {
        X -= v.X;
        Y -= v.Y;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public Vector2 Nor()
    {
        var len = Len();

        if ( len != 0 )
        {
            X /= len;
            Y /= len;
        }

        return this;
    }

    public Vector2 Add( Vector2 v )
    {
        X += v.X;
        Y += v.Y;

        return this;
    }

    public float Dot( Vector2 v )
    {
        return ( X * v.X ) + ( Y * v.Y );
    }

    public Vector2 Scale( float scalar )
    {
        X *= scalar;
        Y *= scalar;

        return this;
    }

    public Vector2 Scale( Vector2 v )
    {
        X *= v.X;
        Y *= v.Y;

        return this;
    }

    public Vector2 MulAdd( Vector2 vec, float scalar )
    {
        X += vec.X * scalar;
        Y += vec.Y * scalar;

        return this;
    }

    public Vector2 MulAdd( Vector2 vec, Vector2 mulVec )
    {
        X += vec.X * mulVec.X;
        Y += vec.Y * mulVec.Y;

        return this;
    }

    public float Distance( Vector2 v )
    {
        var xD = v.X - X;
        var yD = v.Y - Y;

        return ( float ) Math.Sqrt( ( xD * xD ) + ( yD * yD ) );
    }

    public float Distance2( Vector2 v )
    {
        var xD = v.X - X;
        var yD = v.Y - Y;

        return ( xD * xD ) + ( yD * yD );
    }

    public Vector2 Limit( float limit )
    {
        return Limit2( limit * limit );
    }

    public Vector2 Limit2( float limit2 )
    {
        var len2 = Len2();

        return len2 > limit2 ? Scale( ( float ) Math.Sqrt( limit2 / len2 ) ) : this;
    }

    public Vector2 Clamp( float min, float max )
    {
        var len2 = Len2();

        if ( len2 == 0f )
        {
            return this;
        }

        var max2 = max * max;

        if ( len2 > max2 )
        {
            return Scale( ( float ) Math.Sqrt( max2 / len2 ) );
        }

        var min2 = min * min;

        return len2 < min2 ? Scale( ( float ) Math.Sqrt( min2 / len2 ) ) : this;
    }

    public Vector2 SetLength( float len )
    {
        return SetLength2( len * len );
    }

    public Vector2 SetLength2( float len2 )
    {
        var oldLen2 = Len2();

        return ( oldLen2 == 0 ) || MathUtils.IsEqual( oldLen2, len2 ) ? this : Scale( ( float ) Math.Sqrt( len2 / oldLen2 ) );
    }

    public Vector2 Lerp( Vector2 target, float alpha )
    {
        var invAlpha = 1.0f - alpha;
        X = ( X * invAlpha ) + ( target.X * alpha );
        Y = ( Y * invAlpha ) + ( target.Y * alpha );

        return this;
    }

    public Vector2 Interpolate( Vector2 target, float alpha, IInterpolation interpolation )
    {
        return Lerp( target, interpolation.Apply( alpha ) );
    }

    public Vector2 SetToRandomDirection()
    {
        var theta = MathUtils.Random( 0f, MathUtils.PI2 );

        return Set( MathUtils.Cos( theta ), MathUtils.Sin( theta ) );
    }

    public bool EpsilonEquals( Vector2? other, float epsilon = FloatConstants.FLOAT_TOLERANCE )
    {
        if ( other == null )
        {
            return false;
        }

        if ( Math.Abs( other.X - X ) > epsilon )
        {
            return false;
        }

        return !( Math.Abs( other.Y - Y ) > epsilon );
    }

    public bool IsUnit( float margin = FloatConstants.FLOAT_TOLERANCE )
    {
        return Math.Abs( Len2() - 1f ) < margin;
    }

    public bool IsZero()
    {
        return ( X == 0 ) && ( Y == 0 );
    }

    public bool IsZero( float margin )
    {
        return Len2() < margin;
    }

    public bool IsOnLine( Vector2 other )
    {
        return MathUtils.IsZero( ( X * other.Y ) - ( Y * other.X ) );
    }

    public bool IsOnLine( Vector2 other, float epsilon )
    {
        return MathUtils.IsZero( ( X * other.Y ) - ( Y * other.X ), epsilon );
    }

    public bool IsCollinear( Vector2 other, float epsilon )
    {
        return IsOnLine( other, epsilon ) && ( Dot( other ) > 0f );
    }

    public bool IsCollinear( Vector2 other )
    {
        return IsOnLine( other ) && ( Dot( other ) > 0f );
    }

    public bool IsCollinearOpposite( Vector2 other, float epsilon )
    {
        return IsOnLine( other, epsilon ) && ( Dot( other ) < 0f );
    }

    public bool IsCollinearOpposite( Vector2 other )
    {
        return IsOnLine( other ) && ( Dot( other ) < 0f );
    }

    public bool IsPerpendicular( Vector2 vector )
    {
        return MathUtils.IsZero( Dot( vector ) );
    }

    public bool IsPerpendicular( Vector2 vector, float epsilon )
    {
        return MathUtils.IsZero( Dot( vector ), epsilon );
    }

    public bool HasSameDirection( Vector2 vector )
    {
        return Dot( vector ) > 0;
    }

    public bool HasOppositeDirection( Vector2 vector )
    {
        return Dot( vector ) < 0;
    }

    public Vector2 SetZero()
    {
        X = 0;
        Y = 0;

        return this;
    }

    public static float Len( float x, float y )
    {
        return ( float ) Math.Sqrt( ( x * x ) + ( y * y ) );
    }

    public static float Len2( float x, float y )
    {
        return ( x * x ) + ( y * y );
    }

    /// <summary>
    /// Sets the components of this vector
    /// </summary>
    /// <param name="x">The x-component</param>
    /// <param name="y">The y-component</param>
    /// <returns>This vector for chaining</returns>
    public Vector2 Set( float x, float y )
    {
        X = x;
        Y = y;

        return this;
    }

    /// <summary>
    /// Substracts the other vector from this vector.
    /// </summary>
    /// <param name="x">The x-component of the other vector</param>
    /// <param name="y">The y-component of the other vector</param>
    /// <returns>This vector for chaining</returns>
    public Vector2 Sub( float x, float y )
    {
        X -= x;
        Y -= y;

        return this;
    }

    /// <summary>
    /// Adds the given components to this vector
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <returns>This vector for chaining.</returns>
    public Vector2 Add( float x, float y )
    {
        X += x;
        Y += y;

        return this;
    }

    public static float Dot( float x1, float y1, float x2, float y2 )
    {
        return ( x1 * x2 ) + ( y1 * y2 );
    }

    public float Dot( float ox, float oy )
    {
        return ( X * ox ) + ( Y * oy );
    }

    public Vector2 Scl( float x, float y )
    {
        X *= x;
        Y *= y;

        return this;
    }

    public static float Dst( float x1, float y1, float x2, float y2 )
    {
        var xD = x2 - x1;
        var yD = y2 - y1;

        return ( float ) Math.Sqrt( ( xD * xD ) + ( yD * yD ) );
    }

    public float Dst( float x, float y )
    {
        var xD = x - X;
        var yD = y - Y;

        return ( float ) Math.Sqrt( ( xD * xD ) + ( yD * yD ) );
    }

    public static float Dst2( float x1, float y1, float x2, float y2 )
    {
        var xD = x2 - x1;
        var yD = y2 - y1;

        return ( xD * xD ) + ( yD * yD );
    }

    public float Dst2( float x, float y )
    {
        var xD = x - X;
        var yD = y - Y;

        return ( xD * xD ) + ( yD * yD );
    }

    public override string ToString()
    {
        return "(" + X + "," + Y + ")";
    }

    public Vector2 Fromstring( string v )
    {
        var s = v.IndexOf( ',', 1 );

        // Note - v[ ^1 ] is equivalent to v[ v.Length-1 ].
        if ( ( s != -1 ) && ( v[ 0 ] == '(' ) && ( v[ ^1 ] == ')' ) )
        {
            try
            {
                var x = float.Parse( v.Substring( 1, s ) );
                var y = float.Parse( v.Substring( s + 1, v.Length - 1 ) );

                return Set( x, y );
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( ex.Message );
            }
        }

        throw new GdxRuntimeException( "Malformed Vector2: " + v );
    }

    public Vector2 Mul( Matrix3 mat )
    {
        var x = ( X * mat.Val[ 0 ] ) + ( Y * mat.Val[ 3 ] ) + mat.Val[ 6 ];
        var y = ( X * mat.Val[ 1 ] ) + ( Y * mat.Val[ 4 ] ) + mat.Val[ 7 ];

        X = x;
        Y = y;

        return this;
    }

    public float Crs( Vector2 v )
    {
        return ( X * v.Y ) - ( Y * v.X );
    }

    public float Crs( float x, float y )
    {
        return ( X * y ) - ( Y * x );
    }

    public float Angle()
    {
        var angle = ( float ) Math.Atan2( Y, X ) * MathUtils.RADIANS_TO_DEGREES;

        if ( angle < 0 )
        {
            angle += 360;
        }

        return angle;
    }

    public float Angle( Vector2 reference )
    {
        return ( float ) Math.Atan2( Crs( reference ), Dot( reference ) ) * MathUtils.RADIANS_TO_DEGREES;
    }

    public float AngleDeg()
    {
        var angle = ( float ) Math.Atan2( Y, X ) * MathUtils.RADIANS_TO_DEGREES;

        if ( angle < 0 )
        {
            angle += 360;
        }

        return angle;
    }

    public float AngleDeg( Vector2 reference )
    {
        var angle = ( float ) Math.Atan2( reference.Crs( this ), reference.Dot( this ) ) * MathUtils.RADIANS_TO_DEGREES;

        if ( angle < 0 )
        {
            angle += 360;
        }

        return angle;
    }

    public float AngleRad()
    {
        return ( float ) Math.Atan2( Y, X );
    }

    public float AngleRad( Vector2 reference )
    {
        return ( float ) Math.Atan2( reference.Crs( this ), reference.Dot( this ) );
    }

    public Vector2 SetAngle( float degrees )
    {
        return SetAngleRad( degrees * MathUtils.DEGREES_TO_RADIANS );
    }

    public Vector2 SetAngleDeg( float degrees )
    {
        return SetAngleRad( degrees * MathUtils.DEGREES_TO_RADIANS );
    }

    public Vector2 SetAngleRad( float radians )
    {
        Set( Len(), 0f );
        RotateRad( radians );

        return this;
    }

    public Vector2 Rotate( float degrees )
    {
        return RotateRad( degrees * MathUtils.DEGREES_TO_RADIANS );
    }

    public Vector2 RotateAround( Vector2 reference, float degrees )
    {
        return Sub( reference ).RotateDeg( degrees ).Add( reference );
    }

    public Vector2 RotateDeg( float degrees )
    {
        return RotateRad( degrees * MathUtils.DEGREES_TO_RADIANS );
    }

    public Vector2 RotateRad( float radians )
    {
        var cos = ( float ) Math.Cos( radians );
        var sin = ( float ) Math.Sin( radians );

        var newX = ( X * cos ) - ( Y * sin );
        var newY = ( X * sin ) + ( Y * cos );

        X = newX;
        Y = newY;

        return this;
    }

    public Vector2 RotateAroundDeg( Vector2 reference, float degrees )
    {
        return Sub( reference ).RotateDeg( degrees ).Add( reference );
    }

    public Vector2 RotateAroundRad( Vector2 reference, float radians )
    {
        return Sub( reference ).RotateRad( radians ).Add( reference );
    }

    public Vector2 Rotate90( int dir )
    {
        var x = X;

        if ( dir >= 0 )
        {
            X = -Y;
            Y = x;
        }
        else
        {
            X = Y;
            Y = -x;
        }

        return this;
    }

    public override int GetHashCode()
    {
        const int PRIME = 31;

        var result = PRIME + NumberUtils.FloatToIntBits( X );
        result = ( PRIME * result ) + NumberUtils.FloatToIntBits( Y );

        return result;
    }

    public override bool Equals( object? obj )
    {
        if ( this == obj )
        {
            return true;
        }

        if ( obj == null )
        {
            return false;
        }

        if ( GetType() != obj.GetType() )
        {
            return false;
        }

        var other = ( Vector2 ) obj;

        if ( NumberUtils.FloatToIntBits( X ) != NumberUtils.FloatToIntBits( other.X ) )
        {
            return false;
        }

        return NumberUtils.FloatToIntBits( Y ) == NumberUtils.FloatToIntBits( other.Y );
    }

    public bool EpsilonEquals( float x, float y, float epsilon )
    {
        if ( Math.Abs( x - X ) > epsilon )
        {
            return false;
        }

        return !( Math.Abs( y - Y ) > epsilon );
    }

    public bool EpsilonEquals( float x, float y )
    {
        return EpsilonEquals( x, y, FloatConstants.FLOAT_TOLERANCE );
    }
}
