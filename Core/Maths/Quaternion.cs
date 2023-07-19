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

/// <summary>
/// A simple Quaternion class.
/// </summary>
public class Quaternion
{
    private readonly static Quaternion tmp1 = new( 0, 0, 0, 0 );
    private readonly static Quaternion tmp2 = new( 0, 0, 0, 0 );

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    /** Constructor, sets the four components of the quaternion.
	 * @param x The x-component
	 * @param y The y-component
	 * @param z The z-component
	 * @param w The w-component */
    public Quaternion( float x, float y, float z, float w )
    {
        this.Set( x, y, z, w );
    }

    public Quaternion()
    {
        Idt();
    }

    /** Constructor, sets the quaternion components from the given quaternion.
	 * 
	 * @param quaternion The quaternion to copy. */
    public Quaternion( Quaternion quaternion )
    {
        this.Set( quaternion );
    }

    /** Constructor, sets the quaternion from the given axis vector and the angle around that axis in degrees.
	 * 
	 * @param axis The axis
	 * @param angle The angle in degrees. */
    public Quaternion( Vector3 axis, float angle )
    {
        this.Set( axis, angle );
    }

    /** Sets the components of the quaternion
	 * @param x The x-component
	 * @param y The y-component
	 * @param z The z-component
	 * @param w The w-component
	 * @return This quaternion for chaining */
    public Quaternion Set( float x, float y, float z, float w )
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;

        return this;
    }

    /** Sets the quaternion components from the given quaternion.
	 * @param quaternion The quaternion.
	 * @return This quaternion for chaining. */
    public Quaternion Set( Quaternion quaternion )
    {
        return this.Set( quaternion.X, quaternion.Y, quaternion.Z, quaternion.W );
    }

    /** Sets the quaternion components from the given axis and angle around that axis.
	 * 
	 * @param axis The axis
	 * @param angle The angle in degrees
	 * @return This quaternion for chaining. */
    public Quaternion Set( Vector3 axis, float angle )
    {
        return SetFromAxis( axis.X, axis.Y, axis.Z, angle );
    }

    /** @return a copy of this quaternion */
    public Quaternion Cpy()
    {
        return new Quaternion( this );
    }

    /** @return the euclidean length of the specified quaternion */
    public static float Len( float x, float y, float z, float w )
    {
        return ( float )Math.Sqrt( ( x * x ) + ( y * y ) + ( z * z ) + ( w * w ) );
    }

    /** @return the euclidean length of this quaternion */
    public float Len()
    {
        return ( float )Math.Sqrt( ( X * X ) + ( Y * Y ) + ( Z * Z ) + ( W * W ) );
    }

    public new string ToString()
    {
        return "[" + X + "|" + Y + "|" + Z + "|" + W + "]";
    }

    /** Sets the quaternion to the given euler angles in degrees.
	 * @param yaw the rotation around the y axis in degrees
	 * @param pitch the rotation around the x axis in degrees
	 * @param roll the rotation around the z axis degrees
	 * @return this quaternion */
    public Quaternion SetEulerAngles( float yaw, float pitch, float roll )
    {
        return SetEulerAnglesRad
            (
            yaw * MathUtils.DegreesToRadians,
            pitch * MathUtils.DegreesToRadians,
            roll * MathUtils.DegreesToRadians
            );
    }

    /** Sets the quaternion to the given euler angles in radians.
	 * @param yaw the rotation around the y axis in radians
	 * @param pitch the rotation around the x axis in radians
	 * @param roll the rotation around the z axis in radians
	 * @return this quaternion */
    public Quaternion SetEulerAnglesRad( float yaw, float pitch, float roll )
    {
        var hr     = roll * 0.5f;
        var shr    = ( float )Math.Sin( hr );
        var chr    = ( float )Math.Cos( hr );
        var hp     = pitch * 0.5f;
        var shp    = ( float )Math.Sin( hp );
        var chp    = ( float )Math.Cos( hp );
        var hy     = yaw * 0.5f;
        var shy    = ( float )Math.Sin( hy );
        var chy    = ( float )Math.Cos( hy );
        var chyShp = chy * shp;
        var shyChp = shy * chp;
        var chyChp = chy * chp;
        var shyShp = shy * shp;

        // cos(yaw/2) * Sin(pitch/2) * cos(roll/2) + Sin(yaw/2) * cos(pitch/2) * Sin(roll/2)
        X = ( chyShp * chr ) + ( shyChp * shr );

        // Sin(yaw/2) * cos(pitch/2) * cos(roll/2) - cos(yaw/2) * Sin(pitch/2) * Sin(roll/2)
        Y = ( shyChp * chr ) - ( chyShp * shr );

        // cos(yaw/2) * cos(pitch/2) * Sin(roll/2) - Sin(yaw/2) * Sin(pitch/2) * cos(roll/2)
        Z = ( chyChp * shr ) - ( shyShp * chr );

        // cos(yaw/2) * cos(pitch/2) * cos(roll/2) + Sin(yaw/2) * Sin(pitch/2) * Sin(roll/2)
        W = ( chyChp * chr ) + ( shyShp * shr );

        return this;
    }

    /** Get the pole of the gimbal lock, if any.
	 * @return positive (+1) for north pole, negative (-1) for south pole, zero (0) when no gimbal lock */
    public int GetGimbalPole()
    {
        var t = ( Y * X ) + ( Z * W );

        return t > 0.499f ? 1 : ( t < -0.499f ? -1 : 0 );
    }

    /** Get the roll euler angle in radians, which is the rotation around the z axis. Requires that this quaternion is normalized.
	 * @return the rotation around the z axis in radians (between -PI and +PI) */
    public float GetRollRad()
    {
        var pole = GetGimbalPole();

        return pole == 0
            ? MathUtils.Atan2( 2f * ( ( W * Z ) + ( Y * X ) ), 1f - ( 2f * ( ( X * X ) + ( Z * Z ) ) ) )
            : ( float )pole
              * 2f
              * MathUtils.Atan2( Y, W );
    }

    /** Get the roll euler angle in degrees, which is the rotation around the z axis. Requires that this quaternion is normalized.
	 * @return the rotation around the z axis in degrees (between -180 and +180) */
    public float GetRoll()
    {
        return GetRollRad() * MathUtils.RadiansToDegrees;
    }

    /** Get the pitch euler angle in radians, which is the rotation around the x axis. Requires that this quaternion is normalized.
	 * @return the rotation around the x axis in radians (between -(PI/2) and +(PI/2)) */
    public float GetPitchRad()
    {
        var pole = GetGimbalPole();

        return pole == 0
            ? ( float )Math.Asin( MathUtils.Clamp( 2f * ( ( W * X ) - ( Z * Y ) ), -1f, 1f ) )
            : ( float )pole * MathUtils.PI * 0.5f;
    }

    /** Get the pitch euler angle in degrees, which is the rotation around the x axis. Requires that this quaternion is normalized.
	 * @return the rotation around the x axis in degrees (between -90 and +90) */
    public float GetPitch()
    {
        return GetPitchRad() * MathUtils.RadiansToDegrees;
    }

    /** Get the yaw euler angle in radians, which is the rotation around the y axis. Requires that this quaternion is normalized.
	 * @return the rotation around the y axis in radians (between -PI and +PI) */
    public float GetYawRad()
    {
        return GetGimbalPole() == 0
            ? MathUtils.Atan2( 2f * ( ( Y * W ) + ( X * Z ) ), 1f - ( 2f * ( ( Y * Y ) + ( X * X ) ) ) )
            : 0f;
    }

    /** Get the yaw euler angle in degrees, which is the rotation around the y axis. Requires that this quaternion is normalized.
	 * @return the rotation around the y axis in degrees (between -180 and +180) */
    public float GetYaw()
    {
        return GetYawRad() * MathUtils.RadiansToDegrees;
    }

    public static float Len2( float x, float y, float z, float w )
    {
        return ( x * x ) + ( y * y ) + ( z * z ) + ( w * w );
    }

    /** @return the length of this quaternion without square root */
    public float Len2()
    {
        return ( X * X ) + ( Y * Y ) + ( Z * Z ) + ( W * W );
    }

    /** Normalizes this quaternion to unit length
	 * @return the quaternion for chaining */
    public Quaternion Nor()
    {
        var len = Len2();

        if ( ( len != 0.0f ) && !MathUtils.IsEqual( len, 1f ) )
        {
            len = ( float )Math.Sqrt( len );

            W /= len;
            X /= len;
            Y /= len;
            Z /= len;
        }

        return this;
    }

    /// <summary>
    /// Conjugate the quaternion.
    /// </summary>
    /// <returns>This quaternion for chaining</returns>
    public Quaternion Conjugate()
    {
        X = -X;
        Y = -Y;
        Z = -Z;

        return this;
    }

    // TODO : this would better fit into the vector3 class
    /// <summary>
    /// Transforms the given vector using this quaternion
    /// </summary>
	/// <param name="v"> Vector to transform </param>
    public Vector3 Transform( Vector3 v )
    {
        tmp2.Set( this );
        tmp2.Conjugate();
        tmp2.MulLeft( tmp1.Set( v.X, v.Y, v.Z, 0 ) ).MulLeft( this );

        v.X = tmp2.X;
        v.Y = tmp2.Y;
        v.Z = tmp2.Z;

        return v;
    }

    /** Multiplies this quaternion with another one in the form of this = this * other
	 * 
	 * @param other Quaternion to multiply with
	 * @return This quaternion for chaining */
    public Quaternion Mul( Quaternion other )
    {
        var newX = ( ( this.W * other.X ) + ( this.X * other.W ) + ( this.Y * other.Z ) ) - ( this.Z * other.Y );
        var newY = ( ( this.W * other.Y ) + ( this.Y * other.W ) + ( this.Z * other.X ) ) - ( this.X * other.Z );
        var newZ = ( ( this.W * other.Z ) + ( this.Z * other.W ) + ( this.X * other.Y ) ) - ( this.Y * other.X );
        var newW = ( this.W * other.W ) - ( this.X * other.X ) - ( this.Y * other.Y ) - ( this.Z * other.Z );

        this.X = newX;
        this.Y = newY;
        this.Z = newZ;
        this.W = newW;

        return this;
    }

    /** Multiplies this quaternion with another one in the form of this = this * other
	 * 
	 * @param x the x component of the other quaternion to multiply with
	 * @param y the y component of the other quaternion to multiply with
	 * @param z the z component of the other quaternion to multiply with
	 * @param w the w component of the other quaternion to multiply with
	 * @return This quaternion for chaining */
    public Quaternion Mul( float x, float y, float z, float w )
    {
        var newX = ( ( this.W * x ) + ( this.X * w ) + ( this.Y * z ) ) - ( this.Z * y );
        var newY = ( ( this.W * y ) + ( this.Y * w ) + ( this.Z * x ) ) - ( this.X * z );
        var newZ = ( ( this.W * z ) + ( this.Z * w ) + ( this.X * y ) ) - ( this.Y * x );
        var newW = ( this.W * w ) - ( this.X * x ) - ( this.Y * y ) - ( this.Z * z );

        this.X = newX;
        this.Y = newY;
        this.Z = newZ;
        this.W = newW;

        return this;
    }

    /** Multiplies this quaternion with another one in the form of this = other * this
	 * 
	 * @param other Quaternion to multiply with
	 * @return This quaternion for chaining */
    public Quaternion MulLeft( Quaternion other )
    {
        var newX = ( ( other.W * this.X ) + ( other.X * this.W ) + ( other.Y * this.Z ) ) - ( other.Z * this.Y );
        var newY = ( ( other.W * this.Y ) + ( other.Y * this.W ) + ( other.Z * this.X ) ) - ( other.X * this.Z );
        var newZ = ( ( other.W * this.Z ) + ( other.Z * this.W ) + ( other.X * this.Y ) ) - ( other.Y * this.X );
        var newW = ( other.W * this.W ) - ( other.X * this.X ) - ( other.Y * this.Y ) - ( other.Z * this.Z );

        this.X = newX;
        this.Y = newY;
        this.Z = newZ;
        this.W = newW;

        return this;
    }

    /** Multiplies this quaternion with another one in the form of this = other * this
	 * 
	 * @param x the x component of the other quaternion to multiply with
	 * @param y the y component of the other quaternion to multiply with
	 * @param z the z component of the other quaternion to multiply with
	 * @param w the w component of the other quaternion to multiply with
	 * @return This quaternion for chaining */
    public Quaternion MulLeft( float x, float y, float z, float w )
    {
        var newX = ( ( w * this.X ) + ( x * this.W ) + ( y * this.Z ) ) - ( z * this.Y );
        var newY = ( ( w * this.Y ) + ( y * this.W ) + ( z * this.X ) ) - ( x * this.Z );
        var newZ = ( ( w * this.Z ) + ( z * this.W ) + ( x * this.Y ) ) - ( y * this.X );
        var newW = ( w * this.W ) - ( x * this.X ) - ( y * this.Y ) - ( z * this.Z );

        this.X = newX;
        this.Y = newY;
        this.Z = newZ;
        this.W = newW;

        return this;
    }

    /** Add the x,y,z,w components of the passed in quaternion to the ones of this quaternion */
    public Quaternion Add( Quaternion quaternion )
    {
        this.X += quaternion.X;
        this.Y += quaternion.Y;
        this.Z += quaternion.Z;
        this.W += quaternion.W;

        return this;
    }

    /** Add the x,y,z,w components of the passed in quaternion to the ones of this quaternion */
    public Quaternion Add( float qx, float qy, float qz, float qw )
    {
        this.X += qx;
        this.Y += qy;
        this.Z += qz;
        this.W += qw;

        return this;
    }

    // TODO : the matrix4 set(quaternion) doesnt set the last row+col of the matrix to 0,0,0,1 so... that's why there is this
// method
    /** Fills a 4x4 matrix with the rotation matrix represented by this quaternion.
	 * 
	 * @param matrix Matrix to fill */
    public void ToMatrix( float[] matrix )
    {
        var xx = X * X;
        var xy = X * Y;
        var xz = X * Z;
        var xw = X * W;
        var yy = Y * Y;
        var yz = Y * Z;
        var yw = Y * W;
        var zz = Z * Z;
        var zw = Z * W;

        // Set matrix from quaternion
        matrix[ Matrix4.M00 ] = 1 - ( 2 * ( yy + zz ) );
        matrix[ Matrix4.M01 ] = 2 * ( xy - zw );
        matrix[ Matrix4.M02 ] = 2 * ( xz + yw );
        matrix[ Matrix4.M03 ] = 0;
        matrix[ Matrix4.M10 ] = 2 * ( xy + zw );
        matrix[ Matrix4.M11 ] = 1 - ( 2 * ( xx + zz ) );
        matrix[ Matrix4.M12 ] = 2 * ( yz - xw );
        matrix[ Matrix4.M13 ] = 0;
        matrix[ Matrix4.M20 ] = 2 * ( xz - yw );
        matrix[ Matrix4.M21 ] = 2 * ( yz + xw );
        matrix[ Matrix4.M22 ] = 1 - ( 2 * ( xx + yy ) );
        matrix[ Matrix4.M23 ] = 0;
        matrix[ Matrix4.M30 ] = 0;
        matrix[ Matrix4.M31 ] = 0;
        matrix[ Matrix4.M32 ] = 0;
        matrix[ Matrix4.M33 ] = 1;
    }

    /** Sets the quaternion to an identity Quaternion
	 * @return this quaternion for chaining */
    public Quaternion Idt()
    {
        return this.Set( 0, 0, 0, 1 );
    }

    /** @return If this quaternion is an identity Quaternion */
    public bool ISIdentity()
    {
        return MathUtils.IsZero( X )
               && MathUtils.IsZero( Y )
               && MathUtils.IsZero( Z )
               && MathUtils.IsEqual( W, 1f );
    }

    /** @return If this quaternion is an identity Quaternion */
    public bool ISIdentity( float tolerance )
    {
        return MathUtils.IsZero( X, tolerance )
               && MathUtils.IsZero( Y, tolerance )
               && MathUtils.IsZero( Z, tolerance )
               && MathUtils.IsEqual( W, 1f, tolerance );
    }

    // todo : the setFromAxis(v3,float) method should replace the set(v3,float) method
    /** Sets the quaternion components from the given axis and angle around that axis.
	 * 
	 * @param axis The axis
	 * @param degrees The angle in degrees
	 * @return This quaternion for chaining. */
    public Quaternion SetFromAxis( Vector3 axis, float degrees )
    {
        return SetFromAxis( axis.X, axis.Y, axis.Z, degrees );
    }

    /** Sets the quaternion components from the given axis and angle around that axis.
	 * 
	 * @param axis The axis
	 * @param radians The angle in radians
	 * @return This quaternion for chaining. */
    public Quaternion SetFromAxisRad( Vector3 axis, float radians )
    {
        return SetFromAxisRad( axis.X, axis.Y, axis.Z, radians );
    }

    /** Sets the quaternion components from the given axis and angle around that axis.
	 * @param x X direction of the axis
	 * @param y Y direction of the axis
	 * @param z Z direction of the axis
	 * @param degrees The angle in degrees
	 * @return This quaternion for chaining. */
    public Quaternion SetFromAxis( float x, float y, float z, float degrees )
    {
        return SetFromAxisRad( x, y, z, degrees * MathUtils.DegreesToRadians );
    }

    /** Sets the quaternion components from the given axis and angle around that axis.
	 * @param x X direction of the axis
	 * @param y Y direction of the axis
	 * @param z Z direction of the axis
	 * @param radians The angle in radians
	 * @return This quaternion for chaining. */
    public Quaternion SetFromAxisRad( float x, float y, float z, float radians )
    {
        var d = Vector3.Len( x, y, z );

        if ( d == 0f ) return Idt();

        d = 1f / d;

        var lAng = radians < 0 ? MathUtils.PI2 - ( -radians % MathUtils.PI2 ) : radians % MathUtils.PI2;
        var lSin = ( float )Math.Sin( lAng / 2 );
        var lCos = ( float )Math.Cos( lAng / 2 );

        return this.Set( d * x * lSin, d * y * lSin, d * z * lSin, lCos ).Nor();
    }

    /** Sets the Quaternion from the given matrix, optionally removing any scaling. */
    public Quaternion SetFromMatrix( bool normalizeAxes, Matrix4 matrix )
    {
        return SetFromAxes
            (
            normalizeAxes, matrix.val[ Matrix4.M00 ], matrix.val[ Matrix4.M01 ], matrix.val[ Matrix4.M02 ],
            matrix.val[ Matrix4.M10 ], matrix.val[ Matrix4.M11 ], matrix.val[ Matrix4.M12 ], matrix.val[ Matrix4.M20 ],
            matrix.val[ Matrix4.M21 ], matrix.val[ Matrix4.M22 ]
            );
    }

    /** Sets the Quaternion from the given rotation matrix, which must not contain scaling. */
    public Quaternion SetFromMatrix( Matrix4 matrix )
    {
        return SetFromMatrix( false, matrix );
    }

    /** Sets the Quaternion from the given matrix, optionally removing any scaling. */
    public Quaternion SetFromMatrix( bool normalizeAxes, Matrix3 matrix )
    {
        return SetFromAxes
            (
            normalizeAxes, matrix.val[ Matrix3.M00 ], matrix.val[ Matrix3.M01 ], matrix.val[ Matrix3.M02 ],
            matrix.val[ Matrix3.M10 ], matrix.val[ Matrix3.M11 ], matrix.val[ Matrix3.M12 ], matrix.val[ Matrix3.M20 ],
            matrix.val[ Matrix3.M21 ], matrix.val[ Matrix3.M22 ]
            );
    }

    /** Sets the Quaternion from the given rotation matrix, which must not contain scaling. */
    public Quaternion SetFromMatrix( Matrix3 matrix )
    {
        return SetFromMatrix( false, matrix );
    }

    /** <p>
	 * Sets the Quaternion from the given x-, y- and z-axis which have to be orthonormal.
	 * </p>
	 * 
	 * <p>
	 * Taken from Bones framework for JPCT, see http://www.aptalkarga.com/bones/ which in turn took it from Graphics Gem code at
	 * ftp://ftp.cis.upenn.edu/pub/graphics/shoemake/quatut.ps.Z.
	 * </p>
	 * 
	 * @param xx x-axis x-coordinate
	 * @param xy x-axis y-coordinate
	 * @param xz x-axis z-coordinate
	 * @param yx y-axis x-coordinate
	 * @param yy y-axis y-coordinate
	 * @param yz y-axis z-coordinate
	 * @param zx z-axis x-coordinate
	 * @param zy z-axis y-coordinate
	 * @param zz z-axis z-coordinate */
    public Quaternion SetFromAxes( float xx, float xy, float xz, float yx, float yy,
                                   float yz, float zx, float zy, float zz )
    {
        return SetFromAxes( false, xx, xy, xz, yx, yy, yz, zx, zy, zz );
    }

    /** <p>
	 * Sets the Quaternion from the given x-, y- and z-axis.
	 * </p>
	 * 
	 * <p>
	 * Taken from Bones framework for JPCT, see http://www.aptalkarga.com/bones/ which in turn took it from Graphics Gem code at
	 * ftp://ftp.cis.upenn.edu/pub/graphics/shoemake/quatut.ps.Z.
	 * </p>
	 * 
	 * @param normalizeAxes whether to normalize the axes (necessary when they contain scaling)
	 * @param xx x-axis x-coordinate
	 * @param xy x-axis y-coordinate
	 * @param xz x-axis z-coordinate
	 * @param yx y-axis x-coordinate
	 * @param yy y-axis y-coordinate
	 * @param yz y-axis z-coordinate
	 * @param zx z-axis x-coordinate
	 * @param zy z-axis y-coordinate
	 * @param zz z-axis z-coordinate */
    public Quaternion SetFromAxes( bool normalizeAxes, float xx, float xy, float xz,
                                   float yx, float yy, float yz,
                                   float zx, float zy, float zz )
    {
        if ( normalizeAxes )
        {
            var lx = 1f / Vector3.Len( xx, xy, xz );
            var ly = 1f / Vector3.Len( yx, yy, yz );
            var lz = 1f / Vector3.Len( zx, zy, zz );

            xx *= lx;
            xy *= lx;
            xz *= lx;
            yx *= ly;
            yy *= ly;
            yz *= ly;
            zx *= lz;
            zy *= lz;
            zz *= lz;
        }

        // the trace is the sum of the diagonal elements; see
        // http://mathworld.wolfram.com/MatrixTrace.html
        var t = xx + yy + zz;

        // we protect the division by s by ensuring that s>=1
        if ( t >= 0 )
        {
            // |w| >= .5
            var s = ( float )Math.Sqrt( t + 1 ); // |s|>=1 ...

            W = 0.5f * s;
            s = 0.5f / s; // so this division isn't bad
            X = ( zy - yz ) * s;
            Y = ( xz - zx ) * s;
            Z = ( yx - xy ) * s;
        }
        else if ( ( xx > yy ) && ( xx > zz ) )
        {
            var s = ( float )Math.Sqrt( ( 1.0f + xx ) - yy - zz ); // |s|>=1

            X = s * 0.5f; // |x| >= .5
            s = 0.5f / s;
            Y = ( yx + xy ) * s;
            Z = ( xz + zx ) * s;
            W = ( zy - yz ) * s;
        }
        else if ( yy > zz )
        {
            var s = ( float )Math.Sqrt( ( 1.0f + yy ) - xx - zz ); // |s|>=1

            Y = s * 0.5f; // |y| >= .5
            s = 0.5f / s;
            X = ( yx + xy ) * s;
            Z = ( zy + yz ) * s;
            W = ( xz - zx ) * s;
        }
        else
        {
            var s = ( float )Math.Sqrt( ( 1.0 + zz ) - xx - yy ); // |s|>=1

            Z = s * 0.5f; // |z| >= .5
            s = 0.5f / s;
            X = ( xz + zx ) * s;
            Y = ( zy + yz ) * s;
            W = ( yx - xy ) * s;
        }

        return this;
    }

    /** Set this quaternion to the rotation between two vectors.
	 * @param v1 The base vector, which should be normalized.
	 * @param v2 The target vector, which should be normalized.
	 * @return This quaternion for chaining */
    public Quaternion SetFromCross( Vector3 v1, Vector3 v2 )
    {
        var dot   = MathUtils.Clamp( v1.Dot( v2 ), -1f, 1f );
        var angle = ( float )Math.Acos( dot );

        return SetFromAxisRad
            (
            ( v1.Y * v2.Z ) - ( v1.Z * v2.Y ), ( v1.Z * v2.X ) - ( v1.X * v2.Z ),
            ( v1.X * v2.Y ) - ( v1.Y * v2.X ), angle
            );
    }

    /** Set this quaternion to the rotation between two vectors.
	 * @param x1 The base vectors x value, which should be normalized.
	 * @param y1 The base vectors y value, which should be normalized.
	 * @param z1 The base vectors z value, which should be normalized.
	 * @param x2 The target vector x value, which should be normalized.
	 * @param y2 The target vector y value, which should be normalized.
	 * @param z2 The target vector z value, which should be normalized.
	 * @return This quaternion for chaining */
    public Quaternion SetFromCross( float x1, float y1, float z1, float x2, float y2, float z2 )
    {
        var dot   = MathUtils.Clamp( Vector3.Dot( x1, y1, z1, x2, y2, z2 ), -1f, 1f );
        var angle = ( float )Math.Acos( dot );

        return SetFromAxisRad( ( y1 * z2 ) - ( z1 * y2 ), ( z1 * x2 ) - ( x1 * z2 ), ( x1 * y2 ) - ( y1 * x2 ), angle );
    }

    /** Spherical linear interpolation between this quaternion and the other quaternion, based on the alpha value in the range
	 * [0,1]. Taken from Bones framework for JPCT, see http://www.aptalkarga.com/bones/
	 * @param end the end quaternion
	 * @param alpha alpha in the range [0,1]
	 * @return this quaternion for chaining */
    public Quaternion Slerp( Quaternion end, float alpha )
    {
        var d      = ( this.X * end.X ) + ( this.Y * end.Y ) + ( this.Z * end.Z ) + ( this.W * end.W );
        var absDot = d < 0.0f ? -d : d;

        // Set the first and second scale for the interpolation
        var scale0 = 1f - alpha;
        var scale1 = alpha;

        // Check if the angle between the 2 quaternions was big enough to
        // warrant such calculations
        if ( ( 1 - absDot ) > 0.1 )
        {
            // Get the angle between the 2 quaternions,
            // and then store the Sin() of that angle
            var angle       = ( float )Math.Acos( absDot );
            var invSinTheta = 1f / ( float )Math.Sin( angle );

            // Calculate the scale for q1 and q2, according to the angle and
            // it's sine value
            scale0 = ( ( float )Math.Sin( ( 1f - alpha ) * angle ) * invSinTheta );
            scale1 = ( ( float )Math.Sin( ( alpha * angle ) ) * invSinTheta );
        }

        if ( d < 0.0f ) scale1 = -scale1;

        // Calculate the x, y, z and w values for the quaternion by using a
        // special form of linear interpolation for quaternions.
        X = ( scale0 * X ) + ( scale1 * end.X );
        Y = ( scale0 * Y ) + ( scale1 * end.Y );
        Z = ( scale0 * Z ) + ( scale1 * end.Z );
        W = ( scale0 * W ) + ( scale1 * end.W );

        // Return the interpolated quaternion
        return this;
    }

    /** Spherical linearly interpolates multiple quaternions and stores the result in this Quaternion. Will not destroy the data
	 * previously inside the elements of q. result = (q_1^w_1)*(q_2^w_2)* ... *(q_n^w_n) where w_i=1/n.
	 * @param q List of quaternions
	 * @return This quaternion for chaining */
    public Quaternion Slerp( Quaternion[] q )
    {
        // Calculate exponents and multiply everything from left to right
        var w = 1.0f / q.Length;

        Set( q[ 0 ] ).Exp( w );

        for ( var i = 1; i < q.Length; i++ )
        {
            Mul( tmp1.Set( q[ i ] ).Exp( w ) );
        }

        Nor();

        return this;
    }

    /** Spherical linearly interpolates multiple quaternions by the given weights and stores the result in this Quaternion. Will not
	 * destroy the data previously inside the elements of q or w. result = (q_1^w_1)*(q_2^w_2)* ... *(q_n^w_n) where the sum of w_i
	 * is 1. Lists must be equal in length.
	 * @param q List of quaternions
	 * @param w List of weights
	 * @return This quaternion for chaining */
    public Quaternion Slerp( Quaternion[] q, float[] w )
    {
        // Calculate exponents and multiply everything from left to right
        Set( q[ 0 ] ).Exp( w[ 0 ] );

        for ( var i = 1; i < q.Length; i++ )
        {
            Mul( tmp1.Set( q[ i ] ).Exp( w[ i ] ) );
        }

        Nor();

        return this;
    }

    /** Calculates (this quaternion)^alpha where alpha is a real number and stores the result in this quaternion. See
	 * http://en.wikipedia.org/wiki/Quaternion#Exponential.2C_logarithm.2C_and_power
	 * @param alpha Exponent
	 * @return This quaternion for chaining */
    public Quaternion Exp( float alpha )
    {
        // Calculate |q|^alpha
        float norm    = Len();
        var   normExp = ( float )Math.Pow( norm, alpha );

        // Calculate theta
        var theta = ( float )Math.Acos( W / norm );

        // Calculate coefficient of basis elements
        float coeff = 0;

        // If theta is small enough, use the limit of Sin(alpha*theta) / Sin(theta) instead of actual value
        if ( Math.Abs( theta ) < 0.001 )
        {
            coeff = ( normExp * alpha ) / norm;
        }
        else
        {
            coeff = ( float )( ( normExp * Math.Sin( alpha * theta ) ) / ( norm * Math.Sin( theta ) ) );
        }

        // Write results
        W =  ( float )( normExp * Math.Cos( alpha * theta ) );
        X *= coeff;
        Y *= coeff;
        Z *= coeff;

        // Fix any possible discrepancies
        Nor();

        return this;
    }

    public int HashCode()
    {
        var prime  = 31;
        var result = 1;

        result = prime + NumberUtils.FloatToRawIntBits( W );
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( X );
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( Y );
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( Z );

        return result;
    }

    public new bool Equals( object? obj )
    {
        if ( this == obj ) return true;

        if ( obj == null ) return false;

        if ( obj is not Quaternion quaternion )
        {
            return false;
        }

        return ( NumberUtils.FloatToRawIntBits( W ) == NumberUtils.FloatToRawIntBits( quaternion.W ) )
            && ( NumberUtils.FloatToRawIntBits( X ) == NumberUtils.FloatToRawIntBits( quaternion.X ) )
            && ( NumberUtils.FloatToRawIntBits( Y ) == NumberUtils.FloatToRawIntBits( quaternion.Y ) )
            && ( NumberUtils.FloatToRawIntBits( Z ) == NumberUtils.FloatToRawIntBits( quaternion.Z ) );
    }

    /** Get the dot product between the two quaternions (commutative).
	 * @param x1 the x component of the first quaternion
	 * @param y1 the y component of the first quaternion
	 * @param z1 the z component of the first quaternion
	 * @param w1 the w component of the first quaternion
	 * @param x2 the x component of the second quaternion
	 * @param y2 the y component of the second quaternion
	 * @param z2 the z component of the second quaternion
	 * @param w2 the w component of the second quaternion
	 * @return the dot product between the first and second quaternion. */
    public static float Dot( float x1, float y1, float z1, float w1,
                             float x2, float y2, float z2, float w2 )
    {
        return ( x1 * x2 ) + ( y1 * y2 ) + ( z1 * z2 ) + ( w1 * w2 );
    }

    /** Get the dot product between this and the other quaternion (commutative).
	 * @param other the other quaternion.
	 * @return the dot product of this and the other quaternion. */
    public float Dot( Quaternion other )
    {
        return ( this.X * other.X ) + ( this.Y * other.Y ) + ( this.Z * other.Z ) + ( this.W * other.W );
    }

    /** Get the dot product between this and the other quaternion (commutative).
	 * @param x the x component of the other quaternion
	 * @param y the y component of the other quaternion
	 * @param z the z component of the other quaternion
	 * @param w the w component of the other quaternion
	 * @return the dot product of this and the other quaternion. */
    public float Dot( float x, float y, float z, float w )
    {
        return ( this.X * x ) + ( this.Y * y ) + ( this.Z * z ) + ( this.W * w );
    }

    /** Multiplies the components of this quaternion with the given scalar.
	 * @param scalar the scalar.
	 * @return this quaternion for chaining. */
    public Quaternion Mul( float scalar )
    {
        this.X *= scalar;
        this.Y *= scalar;
        this.Z *= scalar;
        this.W *= scalar;

        return this;
    }

    /// <summary>
    /// Get the axis angle representation of the rotation in degrees. The supplied vector
    /// will receive the axis (x, y and z values) of the rotation and the value returned
    /// is the angle in degrees around that axis. Note that this method will alter the
	/// supplied vector, the existing value of the vector is ignored.
	/// <para>
	/// This will normalize this quaternion if needed. The received axis is a unit vector.
	/// However, if this is an identity quaternion (no rotation), then the length of the
	/// axis may be zero.
	/// </para>
    /// </summary>
    /// <param name="axis"> vector which will receive the axis </param>
	/// <returns> the angle in degrees </returns>
    public float GetAxisAngle( Vector3 axis )
    {
        return GetAxisAngleRad( axis ) * MathUtils.RadiansToDegrees;
    }

    /// Get the axis-angle representation of the rotation in radians. The supplied vector
    /// will receive the axis (x, y and z values) of the rotation and the value returned
    /// is the angle in radians around that axis. Note that this method will alter the
	/// supplied vector, the existing value of the vector is ignored.
	/// <para>
	/// This will normalize this quaternion if needed. The received axis is a unit vector.
	/// However, if this is an identity quaternion (no rotation), then the length of the
	/// axis may be zero.
	/// </para>
	/// <param name="axis"> vector which will receive the axis </param>
	/// <returns> the angle in radians </returns>
    public float GetAxisAngleRad( Vector3 axis )
    {
        // if w>1 Acos and Sqrt will produce errors, this cant happen if quaternion is normalised
        if ( this.W > 1 ) this.Nor();

        var angle = ( float )( 2.0 * Math.Acos( this.W ) );

        // assuming quaternion normalised then w is less than 1, so term always positive.
        var s = Math.Sqrt( 1 - ( this.W * this.W ) );

        if ( s < MathUtils.Float_Rounding_Error )
        {
            // test to avoid divide by zero, s is always positive due to Sqrt
            // if s close to zero then direction of axis not important
            // ( if it is important that axis is normalised then replace with x=1; y=z=0 )
            axis.X = this.X;
            axis.Y = this.Y;
            axis.Z = this.Z;
        }
        else
        {
            axis.X = ( float )( this.X / s ); // normalise axis
            axis.Y = ( float )( this.Y / s );
            axis.Z = ( float )( this.Z / s );
        }

        return angle;
    }

    /// <summary>
    /// Get the angle in radians of the rotation this quaternion represents.
    /// Does not normalize the quaternion.
    /// <para>
    /// Use <see cref="GetAxisAngleRad(Vector3)"/> to get both the axis and
    /// the angle of this rotation.
	/// </para>
    /// <para>
    /// Use <see cref="GetAngleAroundRad(Vector3)"/> to get the angle around
    /// a specific axis.
    /// </para>
    /// </summary>
	/// <returns> the angle in radians of the rotation </returns>
    public float GetAngleRad()
    {
        return ( float )( 2.0 * Math.Acos( ( this.W > 1 ) ? ( this.W / Len() ) : this.W ) );
    }

    /// Get the angle in degrees of the rotation this quaternion represents. Use {@link #getAxisAngle(Vector3)} to get both the axis
	/// and the angle of this rotation. Use {@link #getAngleAround(Vector3)} to get the angle around a specific axis.
	/// @return the angle in degrees of the rotation */
    public float GetAngle()
    {
        return GetAngleRad() * MathUtils.RadiansToDegrees;
    }

    /// <summary>
    /// Get the swing rotation and twist rotation for the specified axis. The twist
    /// rotation represents the rotation around the specified axis. The swing rotation
    /// represents the rotation of the specified axis itself, which is the rotation
    /// around an axis perpendicular to the specified axis.
    /// <para>
    /// The swing and twist rotation can be used to reconstruct the original
    /// quaternion: this = swing * twist
    /// </para>
    /// </summary>
    /// <param name="axisX">
    /// the X component of the normalized axis for which to get the swing and twist rotation
    /// </param>
    /// <param name="axisY">
    /// the Y component of the normalized axis for which to get the swing and twist rotation
    /// </param>
    /// <param name="axisZ">
    /// the Z component of the normalized axis for which to get the swing and twist rotation
    /// </param>
    /// <param name="swing">
    /// will receive the swing rotation: the rotation around an axis perpendicular to the specified axis
    /// </param>
    /// <param name="twist">
    /// will receive the twist rotation: the rotation around the specified axis
    /// </param>
    public void GetSwingTwist( float axisX, float axisY, float axisZ, ref Quaternion swing, ref Quaternion twist )
    {
        float d = Vector3.Dot( this.X, this.Y, this.Z, axisX, axisY, axisZ );

        twist.Set( axisX * d, axisY * d, axisZ * d, this.W ).Nor();
        
        if ( d < 0 ) twist.Mul( -1f );
        
        swing.Set( twist ).Conjugate().MulLeft( this );
    }

    /// <summary>
    /// Get the swing rotation and twist rotation for the specified axis. The twist
    /// rotation represents the rotation around the specified axis. The swing rotation
    /// represents the rotation of the specified axis itself, which is the rotation
    /// around an axis perpendicular to the specified axis.
    /// <para>
    /// The swing and twist rotation can be used to reconstruct the original
    /// quaternion: this = swing * twist
    /// </para>
    /// </summary>
    /// <param name="axis">
    /// the normalized axis for which to get the swing and twist rotation
    /// </param>
    /// <param name="swing">
    /// will receive the swing rotation: the rotation around an axis perpendicular
    /// to the specified axis
    /// </param>
    /// <param name="twist">
    /// will receive the twist rotation: the rotation around the specified axis
    /// </param>
    public void GetSwingTwist( Vector3 axis, ref Quaternion swing, ref Quaternion twist )
    {
        GetSwingTwist( axis.X, axis.Y, axis.Z, ref swing, ref twist );
    }

    /// <summary>
    /// Get the angle in radians of the rotation around the specified axis.
    /// The axis must be normalized.
    /// </summary>
	/// <param name="axisX"> the x component of the normalized axis for which to get the angle </param>
	/// <param name="axisY"> the y component of the normalized axis for which to get the angle </param>
	/// <param name="axisZ"> the z component of the normalized axis for which to get the angle </param>
	/// <returns>
	/// the angle in radians of the rotation around the specified axis
	/// </returns>
    public float GetAngleAroundRad( float axisX, float axisY, float axisZ )
    {
        var d  = Vector3.Dot( this.X, this.Y, this.Z, axisX, axisY, axisZ );
        var   l2 = Len2( axisX * d, axisY * d, axisZ * d, this.W );

        return MathUtils.IsZero( l2 )
            ? 0f
            : ( float )( 2.0
                         * Math.Acos
                             (
                             MathUtils.Clamp
                                 (
                                 ( float )( ( d < 0 ? -this.W : this.W ) / Math.Sqrt( l2 ) ), -1f, 1f
                                 )
                             ) );
    }

    /// <summary>
    /// Get the angle in radians of the rotation around the specified axis.
    /// The axis must be normalized.
    /// </summary>
	/// <param name="axis"> the normalized axis for which to get the angle </param>
	/// <returns> the angle in radians of the rotation around the specified axis </returns>
    public float GetAngleAroundRad( Vector3 axis )
    {
        return GetAngleAroundRad( axis.X, axis.Y, axis.Z );
    }

    /// <summary>
    /// Get the angle in degrees of the rotation around the specified axis. The axis must be normalized.
    /// </summary>
	/// <param name="axisX"> the x component of the normalized axis for which to get the angle </param>
	/// <param name="axisY"> the y component of the normalized axis for which to get the angle </param>
	/// <param name="axisZ"> the z component of the normalized axis for which to get the angle </param>
	/// <returns> the angle in degrees of the rotation around the specified axis </returns>
    public float GetAngleAround( float axisX, float axisY, float axisZ )
    {
        return GetAngleAroundRad( axisX, axisY, axisZ ) * MathUtils.RadiansToDegrees;
    }

    /// <summary>
    /// Get the angle in degrees of the rotation around the specified axis. The axis must be normalized.
    /// </summary>
	/// <param name="axis"> the normalized axis for which to get the angle </param>
	/// <returns> the angle in degrees of the rotation around the specified axis </returns>
    public float GetAngleAround( Vector3 axis )
    {
        return GetAngleAround( axis.X, axis.Y, axis.Z );
    }
}