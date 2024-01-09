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

public class Vector3 : IVector< Vector3 >
{
    public readonly static  Vector3 XDefault = new( 1, 0, 0 );
    public readonly static  Vector3 YDefault = new( 0, 1, 0 );
    public readonly static  Vector3 ZDefault = new( 0, 0, 1 );
    public readonly static  Vector3 Zero     = new( 0, 0, 0 );
    private readonly static Matrix4 TmpMat   = new();

    /// <summary>
    ///     Default constructor.
    ///     Sets X, Y, and Z to zero.
    /// </summary>
    public Vector3() : this( 0, 0, 0 )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3( float x, float y, float z ) => Set( x, y, z );

    /// <summary>
    /// </summary>
    /// <param name="vector"></param>
    public Vector3( Vector3 vector ) => Set( vector );

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
    public Vector3( float[] values ) => Set( values[ 0 ], values[ 1 ], values[ 2 ] );

    /// <summary>
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="z"></param>
    public Vector3( Vector2 vector, float z ) => Set( vector.X, vector.Y, z );

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Vector3 Set( Vector3 vector ) => Set( vector.X, vector.Y, vector.Z );

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public Vector3 SetToRandomDirection()
    {
        var u = MathUtils.Random();
        var v = MathUtils.Random();

        var theta = MathUtils.PI2 * u;                     // azimuthal angle
        var phi   = ( float )Math.Acos( ( 2f * v ) - 1f ); // polar angle

        return SetFromSpherical( theta, phi );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public Vector3 Cpy() => new( this );

    /// <summary>
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public Vector3 Add( Vector3 vector ) => Add( vector.X, vector.Y, vector.Z );

    public Vector3 Sub( Vector3 vec ) => Sub( vec.X, vec.Y, vec.Z );

    public Vector3 Scl( float scalar ) => Set( X * scalar, Y * scalar, Z * scalar );

    public Vector3 Scl( Vector3 other ) => Set( X * other.X, Y * other.Y, Z * other.Z );

    public Vector3 MulAdd( Vector3 vec, float scalar )
    {
        X += vec.X * scalar;
        Y += vec.Y * scalar;
        Z += vec.Z * scalar;

        return this;
    }

    public Vector3 MulAdd( Vector3 vec, Vector3 mulVec )
    {
        X += vec.X * mulVec.X;
        Y += vec.Y * mulVec.Y;
        Z += vec.Z * mulVec.Z;

        return this;
    }

    public float Len() => ( float )Math.Sqrt( ( X * X ) + ( Y * Y ) + ( Z * Z ) );

    public float Len2() => ( X * X ) + ( Y * Y ) + ( Z * Z );

    public float Dst( Vector3 vector )
    {
        var a = vector.X - X;
        var b = vector.Y - Y;
        var c = vector.Z - Z;

        return ( float )Math.Sqrt( ( a * a ) + ( b * b ) + ( c * c ) );
    }

    public float Dst2( Vector3 point )
    {
        var a = point.X - X;
        var b = point.Y - Y;
        var c = point.Z - Z;

        return ( a * a ) + ( b * b ) + ( c * c );
    }

    public Vector3 Nor()
    {
        var len2 = Len2();

        if ( ( len2 == 0f ) || MathUtils.IsEqual( len2, 1f ) )
        {
            return this;
        }

        return Scl( 1f / ( float )Math.Sqrt( len2 ) );
    }

    public float Dot( Vector3 vector ) => ( X * vector.X ) + ( Y * vector.Y ) + ( Z * vector.Z );

    public bool IsUnit() => IsUnit( 0.000000001f );

    public bool IsUnit( float margin ) => Math.Abs( Len2() - 1f ) < margin;

    public bool IsZero() => ( X == 0 ) && ( Y == 0 ) && ( Z == 0 );

    public bool IsZero( float margin ) => Len2() < margin;

    public bool IsOnLine( Vector3 other, float epsilon )
        => Len2( ( Y * other.Z ) - ( Z * other.Y ), ( Z * other.X ) - ( X * other.Z ), ( X * other.Y ) - ( Y * other.X ) ) <= epsilon;

    public bool IsOnLine( Vector3 other ) => Len2( ( Y * other.Z ) - ( Z * other.Y ), ( Z * other.X ) - ( X * other.Z ), ( X * other.Y ) - ( Y * other.X ) )
                                          <= MathUtils.FLOAT_ROUNDING_ERROR;

    public bool IsCollinear( Vector3 other, float epsilon ) => IsOnLine( other, epsilon ) && HasSameDirection( other );

    public bool IsCollinear( Vector3 other ) => IsOnLine( other ) && HasSameDirection( other );

    public bool IsCollinearOpposite( Vector3 other, float epsilon ) => IsOnLine( other, epsilon ) && HasOppositeDirection( other );

    public bool IsCollinearOpposite( Vector3 other ) => IsOnLine( other ) && HasOppositeDirection( other );

    public bool IsPerpendicular( Vector3 vector ) => MathUtils.IsZero( Dot( vector ) );

    public bool IsPerpendicular( Vector3 vector, float epsilon ) => MathUtils.IsZero( Dot( vector ), epsilon );

    public bool HasSameDirection( Vector3 vector ) => Dot( vector ) > 0;

    public bool HasOppositeDirection( Vector3 vector ) => Dot( vector ) < 0;

    public Vector3 Lerp( Vector3 target, float alpha )
    {
        X += alpha * ( target.X - X );
        Y += alpha * ( target.Y - Y );
        Z += alpha * ( target.Z - Z );

        return this;
    }

    public Vector3 Interpolate( Vector3 target, float alpha, IInterpolation interpolator ) => Lerp( target, interpolator.Apply( 0f, 1f, alpha ) );

    public Vector3 Limit( float limit ) => Limit2( limit * limit );

    public Vector3 Limit2( float limit2 )
    {
        var len2 = Len2();

        if ( len2 > limit2 )
        {
            Scl( ( float )Math.Sqrt( limit2 / len2 ) );
        }

        return this;
    }

    public Vector3 SetLength( float len ) => SetLength2( len * len );

    public Vector3 SetLength2( float len2 )
    {
        var oldLen2 = Len2();

        return ( oldLen2 == 0 ) || MathUtils.IsEqual( oldLen2, len2 ) ? this : Scl( ( float )Math.Sqrt( len2 / oldLen2 ) );
    }

    /// <summary>
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public Vector3 Clamp( float min, float max )
    {
        var len2 = Len2();

        if ( len2 == 0f )
        {
            return this;
        }

        var max2 = max * max;

        if ( len2 > max2 )
        {
            return Scl( ( float )Math.Sqrt( max2 / len2 ) );
        }

        var min2 = min * min;

        return len2 < min2 ? Scl( ( float )Math.Sqrt( min2 / len2 ) ) : this;
    }

    /// <summary>
    /// </summary>
    /// <param name="other"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool EpsilonEquals( Vector3? other, float epsilon = MathUtils.FLOAT_ROUNDING_ERROR )
    {
        if ( other == null )
        {
            return false;
        }

        if ( Math.Abs( other.X - X ) > epsilon )
        {
            return false;
        }

        if ( Math.Abs( other.Y - Y ) > epsilon )
        {
            return false;
        }

        if ( Math.Abs( other.Z - Z ) > epsilon )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public Vector3 SetZero()
    {
        X = 0;
        Y = 0;
        Z = 0;

        return this;
    }

    public Vector3 Set( float x, float y, float z )
    {
        X = x;
        Y = y;
        Z = z;

        return this;
    }

    public Vector3 Set( float[] values ) => Set( values[ 0 ], values[ 1 ], values[ 2 ] );

    public Vector3 Set( Vector2 vector, float z ) => Set( vector.X, vector.Y, z );

    /// <summary>
    /// </summary>
    /// <param name="azimuthalAngle"></param>
    /// <param name="polarAngle"></param>
    /// <returns></returns>
    public Vector3 SetFromSpherical( float azimuthalAngle, float polarAngle )
    {
        var cosPolar = MathUtils.Cos( polarAngle );
        var sinPolar = MathUtils.Sin( polarAngle );

        var cosAzim = MathUtils.Cos( azimuthalAngle );
        var sinAzim = MathUtils.Sin( azimuthalAngle );

        return Set( cosAzim * sinPolar, sinAzim * sinPolar, cosPolar );
    }

    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Vector3 Add( float x, float y, float z ) => Set( X + x, Y + y, Z + z );

    public Vector3 Add( float values ) => Set( X + values, Y + values, Z + values );

    public Vector3 Sub( float x, float y, float z ) => Set( X - x, Y - y, Z - z );

    public Vector3 Sub( float value ) => Set( X - value, Y - value, Z - value );

    public Vector3 Scl( float vx, float vy, float vz ) => Set( X * vx, Y * vy, Z * vz );

    public static float Len( float x, float y, float z ) => ( float )Math.Sqrt( ( x * x ) + ( y * y ) + ( z * z ) );

    public static float Len2( float x, float y, float z ) => ( x * x ) + ( y * y ) + ( z * z );

    public bool Idt( Vector3 vector ) => MathUtils.IsEqual( X, vector.X )
                                      && MathUtils.IsEqual( Y, vector.Y )
                                      && MathUtils.IsEqual( Z, vector.Z );

    public static float Dst( float x1, float y1, float z1, float x2, float y2, float z2 )
    {
        var a = x2 - x1;
        var b = y2 - y1;
        var c = z2 - z1;

        return ( float )Math.Sqrt( ( a * a ) + ( b * b ) + ( c * c ) );
    }

    public float Dst( float x, float y, float z )
    {
        var a = x - X;
        var b = y - Y;
        var c = z - Z;

        return ( float )Math.Sqrt( ( a * a ) + ( b * b ) + ( c * c ) );
    }

    public static float Dst2( float x1, float y1, float z1, float x2, float y2, float z2 )
    {
        var a = x2 - x1;
        var b = y2 - y1;
        var c = z2 - z1;

        return ( a * a ) + ( b * b ) + ( c * c );
    }

    public float Dst2( float x, float y, float z )
    {
        var a = x - X;
        var b = y - Y;
        var c = z - Z;

        return ( a * a ) + ( b * b ) + ( c * c );
    }

    public static float Dot( float x1, float y1, float z1, float x2, float y2, float z2 ) => ( x1 * x2 ) + ( y1 * y2 ) + ( z1 * z2 );

    public float Dot( float x, float y, float z ) => ( X * x ) + ( Y * y ) + ( Z * z );

    public Vector3 Crs( Vector3 vector )
        => Set( ( Y * vector.Z ) - ( Z * vector.Y ), ( Z * vector.X ) - ( X * vector.Z ), ( X * vector.Y ) - ( Y * vector.X ) );

    public Vector3 Crs( float x, float y, float z ) => Set( ( Y * z ) - ( Z * y ), ( Z * x ) - ( X * z ), ( X * y ) - ( Y * x ) );

    public Vector3 Mul4X3( float[] matrix ) => Set(
        ( X * matrix[ 0 ] ) + ( Y * matrix[ 3 ] ) + ( Z * matrix[ 6 ] ) + matrix[ 9 ],
        ( X * matrix[ 1 ] )
      + ( Y * matrix[ 4 ] )
      + ( Z * matrix[ 7 ] )
      + matrix[ 10 ],
        ( X * matrix[ 2 ] ) + ( Y * matrix[ 5 ] ) + ( Z * matrix[ 8 ] ) + matrix[ 11 ]
        );

    public Vector3 Mul( Matrix4 matrix )
    {
        var lMat = matrix.val;

        return Set(
            ( X * lMat[ Matrix4.M00 ] ) + ( Y * lMat[ Matrix4.M01 ] ) + ( Z * lMat[ Matrix4.M02 ] ) + lMat[ Matrix4.M03 ],
            ( X
            * lMat[ Matrix4.M10 ] )
          + ( Y * lMat[ Matrix4.M11 ] )
          + ( Z * lMat[ Matrix4.M12 ] )
          + lMat[ Matrix4.M13 ],
            ( X * lMat[ Matrix4.M20 ] )
          + ( Y
            * lMat[ Matrix4.M21 ] )
          + ( Z * lMat[ Matrix4.M22 ] )
          + lMat[ Matrix4.M23 ]
            );
    }

    public Vector3 TraMul( Matrix4 matrix )
    {
        var lMat = matrix.val;

        return Set(
            ( X * lMat[ Matrix4.M00 ] ) + ( Y * lMat[ Matrix4.M10 ] ) + ( Z * lMat[ Matrix4.M20 ] ) + lMat[ Matrix4.M30 ],
            ( X
            * lMat[ Matrix4.M01 ] )
          + ( Y * lMat[ Matrix4.M11 ] )
          + ( Z * lMat[ Matrix4.M21 ] )
          + lMat[ Matrix4.M31 ],
            ( X * lMat[ Matrix4.M02 ] )
          + ( Y
            * lMat[ Matrix4.M12 ] )
          + ( Z * lMat[ Matrix4.M22 ] )
          + lMat[ Matrix4.M32 ]
            );
    }

    public Vector3 Mul( Matrix3 matrix )
    {
        var lMat = matrix.val;

        return Set(
            ( X * lMat[ Matrix3.M00 ] ) + ( Y * lMat[ Matrix3.M01 ] ) + ( Z * lMat[ Matrix3.M02 ] ),
            ( X * lMat[ Matrix3.M10 ] )
          + ( Y
            * lMat[ Matrix3.M11 ] )
          + ( Z * lMat[ Matrix3.M12 ] ),
            ( X * lMat[ Matrix3.M20 ] ) + ( Y * lMat[ Matrix3.M21 ] ) + ( Z * lMat[ Matrix3.M22 ] )
            );
    }

    public Vector3 TraMul( Matrix3 matrix )
    {
        var lMat = matrix.val;

        return Set(
            ( X * lMat[ Matrix3.M00 ] ) + ( Y * lMat[ Matrix3.M10 ] ) + ( Z * lMat[ Matrix3.M20 ] ),
            ( X * lMat[ Matrix3.M01 ] )
          + ( Y
            * lMat[ Matrix3.M11 ] )
          + ( Z * lMat[ Matrix3.M21 ] ),
            ( X * lMat[ Matrix3.M02 ] ) + ( Y * lMat[ Matrix3.M12 ] ) + ( Z * lMat[ Matrix3.M22 ] )
            );
    }

    public Vector3 Mul( Quaternion quat ) => quat.Transform( this );

    public Vector3 Prj( Matrix4 matrix )
    {
        var lMat = matrix.val;
        var lW   = 1f / ( ( X * lMat[ Matrix4.M30 ] ) + ( Y * lMat[ Matrix4.M31 ] ) + ( Z * lMat[ Matrix4.M32 ] ) + lMat[ Matrix4.M33 ] );

        return Set(
            ( ( X * lMat[ Matrix4.M00 ] ) + ( Y * lMat[ Matrix4.M01 ] ) + ( Z * lMat[ Matrix4.M02 ] ) + lMat[ Matrix4.M03 ] ) * lW,
            ( ( X
              * lMat[ Matrix4.M10 ] )
            + ( Y * lMat[ Matrix4.M11 ] )
            + ( Z * lMat[ Matrix4.M12 ] )
            + lMat[ Matrix4.M13 ] )
          * lW,
            ( ( X * lMat[ Matrix4.M20 ] ) + ( Y * lMat[ Matrix4.M21 ] ) + ( Z * lMat[ Matrix4.M22 ] ) + lMat[ Matrix4.M23 ] ) * lW
            );
    }

    public Vector3 Rot( Matrix4 matrix )
    {
        var lMat = matrix.val;

        return Set(
            ( X * lMat[ Matrix4.M00 ] ) + ( Y * lMat[ Matrix4.M01 ] ) + ( Z * lMat[ Matrix4.M02 ] ),
            ( X * lMat[ Matrix4.M10 ] )
          + ( Y
            * lMat[ Matrix4.M11 ] )
          + ( Z * lMat[ Matrix4.M12 ] ),
            ( X * lMat[ Matrix4.M20 ] ) + ( Y * lMat[ Matrix4.M21 ] ) + ( Z * lMat[ Matrix4.M22 ] )
            );
    }

    public Vector3 Unrotate( Matrix4 matrix )
    {
        var lMat = matrix.val;

        return Set(
            ( X * lMat[ Matrix4.M00 ] ) + ( Y * lMat[ Matrix4.M10 ] ) + ( Z * lMat[ Matrix4.M20 ] ),
            ( X * lMat[ Matrix4.M01 ] )
          + ( Y
            * lMat[ Matrix4.M11 ] )
          + ( Z * lMat[ Matrix4.M21 ] ),
            ( X * lMat[ Matrix4.M02 ] ) + ( Y * lMat[ Matrix4.M12 ] ) + ( Z * lMat[ Matrix4.M22 ] )
            );
    }

    public Vector3 Untransform( Matrix4 matrix )
    {
        var lMat = matrix.val;

        X -= lMat[ Matrix4.M03 ];
        Y -= lMat[ Matrix4.M03 ];
        Z -= lMat[ Matrix4.M03 ];

        return Set(
            ( X * lMat[ Matrix4.M00 ] ) + ( Y * lMat[ Matrix4.M10 ] ) + ( Z * lMat[ Matrix4.M20 ] ),
            ( X * lMat[ Matrix4.M01 ] )
          + ( Y
            * lMat[ Matrix4.M11 ] )
          + ( Z * lMat[ Matrix4.M21 ] ),
            ( X * lMat[ Matrix4.M02 ] ) + ( Y * lMat[ Matrix4.M12 ] ) + ( Z * lMat[ Matrix4.M22 ] )
            );
    }

    public Vector3 Rotate( float degrees, float axisX, float axisY, float axisZ ) => Mul( TmpMat.SetToRotation( axisX, axisY, axisZ, degrees ) );

    public Vector3 RotateRad( float radians, float axisX, float axisY, float axisZ ) => Mul( TmpMat.SetToRotationRad( axisX, axisY, axisZ, radians ) );

    public Vector3 Rotate( Vector3 axis, float degrees )
    {
        TmpMat.SetToRotation( axis, degrees );

        return Mul( TmpMat );
    }

    public Vector3 RotateRad( Vector3 axis, float radians )
    {
        TmpMat.SetToRotationRad( axis, radians );

        return Mul( TmpMat );
    }

    public Vector3 Slerp( Vector3 target, float alpha )
    {
        var dot = Dot( target );

        // If the inputs are too close for comfort, simply linearly interpolate.
        if ( ( dot > 0.9995 ) || ( dot < -0.9995 ) )
        {
            return Lerp( target, alpha );
        }

        // theta0 = angle between input vectors
        var theta0 = ( float )Math.Acos( dot );

        // theta = angle between this vector and result
        var theta = theta0 * alpha;

        var st = ( float )Math.Sin( theta );
        var tx = target.X - ( X * dot );
        var ty = target.Y - ( Y * dot );
        var tz = target.Z - ( Z * dot );
        var l2 = ( tx * tx ) + ( ty * ty ) + ( tz * tz );
        var dl = st * ( l2 < 0.0001f ? 1f : 1f / ( float )Math.Sqrt( l2 ) );

        return Scl( ( float )Math.Cos( theta ) ).Add( tx * dl, ty * dl, tz * dl ).Nor();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public new int GetHashCode()
    {
        var prime = 31;

        var result = prime + NumberUtils.FloatToIntBits( X );
        result = ( prime * result ) + NumberUtils.FloatToIntBits( Y );
        result = ( prime * result ) + NumberUtils.FloatToIntBits( Z );

        return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public new bool Equals( object? obj )
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

        var other = ( Vector3 )obj;

        if ( NumberUtils.FloatToIntBits( X ) != NumberUtils.FloatToIntBits( other.X ) )
        {
            return false;
        }

        if ( NumberUtils.FloatToIntBits( Y ) != NumberUtils.FloatToIntBits( other.Y ) )
        {
            return false;
        }

        return NumberUtils.FloatToIntBits( Z ) == NumberUtils.FloatToIntBits( other.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public bool EpsilonEquals( float x, float y, float z, float epsilon = MathUtils.FLOAT_ROUNDING_ERROR )
    {
        if ( Math.Abs( x - X ) > epsilon )
        {
            return false;
        }

        if ( Math.Abs( y - Y ) > epsilon )
        {
            return false;
        }

        if ( Math.Abs( z - Z ) > epsilon )
        {
            return false;
        }

        return true;
    }

    public override string ToString() => "(" + X + "," + Y + "," + Z + ")";

    public Vector3 FromString( string v )
    {
        var s0 = v.IndexOf( ',', 1 );
        var s1 = v.IndexOf( ',', s0 + 1 );

        if ( ( s0 != -1 ) && ( s1 != -1 ) && ( v[ 0 ] == '(' ) && ( v[ ^1 ] == ')' ) )
        {
            try
            {
                var x = float.Parse( v.Substring( 1, s0 ) );
                var y = float.Parse( v.Substring( s0 + 1, s1 ) );
                var z = float.Parse( v.Substring( s1 + 1, v.Length - 1 ) );

                return Set( x, y, z );
            }
            catch ( NumberFormatException )
            {
                // Throw a GdxRuntimeException
            }
        }

        throw new GdxRuntimeException( "Malformed Vector3: " + v );
    }
}
