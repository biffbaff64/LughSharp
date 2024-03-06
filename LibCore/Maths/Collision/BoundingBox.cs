// ///////////////////////////////////////////////////////////////////////////////
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


namespace LibGDXSharp.LibCore.Maths.Collision;

/// <summary>
///     Encapsulates an axis aligned bounding box represented by a minimum
///     and a maximum Vector. Additionally you can query for the bounding
///     box's center, dimensions and corner points.
/// </summary>
[Serializable, PublicAPI]
public class BoundingBox
{
    private readonly static Vector3 TmpVector = new();

    private readonly Vector3 _cnt = new();
    private readonly Vector3 _dim = new();
    public readonly  Vector3 max  = new();

    public readonly Vector3 min = new();

    /// <summary>
    ///     Constructs a new bounding box with the minimum and maximum vector set to zeros.
    /// </summary>
    public BoundingBox()
    {
        Clear();
    }

    /// <summary>
    ///     Constructs a new bounding box from the given bounding box.
    /// </summary>
    /// <param name="bounds"> The bounding box to copy  </param>
    public BoundingBox( BoundingBox bounds )
    {
        Set( bounds );
    }

    /// <summary>
    ///     Constructs the new bounding box using the given minimum and maximum vector.
    /// </summary>
    /// <param name="minimum"> The minimum vector </param>
    /// <param name="maximum"> The maximum vector  </param>
    public BoundingBox( Vector3 minimum, Vector3 maximum )
    {
        Set( minimum, maximum );
    }

    public float CenterX => _cnt.X;
    public float CenterY => _cnt.Y;
    public float CenterZ => _cnt.Z;

    public float Width  => _dim.X;
    public float Height => _dim.Y;
    public float Depth  => _dim.Z;

    /// <summary>
    ///     Returns whether this bounding box is valid.
    ///     This means that <see cref="max" /> is greater than or equal to <see cref="min" />.
    /// </summary>
    /// <returns> True in case the bounding box is valid, false otherwise  </returns>
    public bool Valid => ( min.X <= max.X ) && ( min.Y <= max.Y ) && ( min.Z <= max.Z );

    /// <summary>
    ///     Sets the given bounding box.
    /// </summary>
    /// <param name="bounds">The bounds.</param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Set( BoundingBox bounds )
    {
        return Set( bounds.min, bounds.max );
    }

    /// <summary>
    ///     Sets the given minimum and maximum vector.
    /// </summary>
    /// <param name="minimum"> The minimum vector </param>
    /// <param name="maximum"> The maximum vector </param>
    /// <returns> This bounding box for chaining.  </returns>
    public BoundingBox Set( Vector3 minimum, Vector3 maximum )
    {
        min.Set(
            minimum.X < maximum.X ? minimum.X : maximum.X,
            minimum.Y < maximum.Y ? minimum.Y : maximum.Y,
            minimum.Z < maximum.Z ? minimum.Z : maximum.Z
            );

        max.Set(
            minimum.X > maximum.X ? minimum.X : maximum.X,
            minimum.Y > maximum.Y ? minimum.Y : maximum.Y,
            minimum.Z > maximum.Z ? minimum.Z : maximum.Z
            );

        _cnt.Set( min ).Add( max ).Scl( 0.5f );
        _dim.Set( max ).Sub( min );

        return this;
    }

    /// <summary>
    ///     Sets the bounding box minimum and maximum vector from the given points.
    /// </summary>
    /// <param name="points"> The points. </param>
    /// <returns> This bounding box for chaining.  </returns>
    public BoundingBox Set( Vector3[] points )
    {
        ToInfinity();

        foreach ( Vector3 lPoint in points )
        {
            Extend( lPoint );
        }

        return this;
    }

    /// <summary>
    ///     Sets the bounding box minimum and maximum vector from the given points.
    /// </summary>
    /// <param name="points"> The points. </param>
    /// <returns> This bounding box for chaining.  </returns>
    public BoundingBox Set( IEnumerable< Vector3 > points )
    {
        ToInfinity();

        foreach ( Vector3 lPoint in points )
        {
            Extend( lPoint );
        }

        return this;
    }

    /// <summary>
    ///     Gets the centre of the bounding box and places it into the supplied
    ///     Vector3. The modified vector is returned but can be thrown away.
    /// </summary>
    /// <param name="vec3">
    ///     The <see cref="Vector3" /> to receive the center of the bounding box.
    /// </param>
    /// <returns>The vector specified with the vec3 argument.</returns>
    public Vector3 GetCenter( Vector3 vec3 )
    {
        return vec3.Set( _cnt );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner000( in Vector3 vec3 )
    {
        return vec3.Set( min.X, min.Y, min.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner001( in Vector3 vec3 )
    {
        return vec3.Set( min.X, min.Y, max.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner010( in Vector3 vec3 )
    {
        return vec3.Set( min.X, max.Y, min.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner011( in Vector3 vec3 )
    {
        return vec3.Set( min.X, max.Y, max.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner100( in Vector3 vec3 )
    {
        return vec3.Set( max.X, min.Y, min.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner101( in Vector3 vec3 )
    {
        return vec3.Set( max.X, min.Y, max.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner110( in Vector3 vec3 )
    {
        return vec3.Set( max.X, max.Y, min.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner111( in Vector3 vec3 )
    {
        return vec3.Set( max.X, max.Y, max.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3">
    ///     The <see cref="Vector3" /> to receive the dimensions of this bounding box on all three axis.
    /// </param>
    /// <returns> The vector specified with the vec3 argument</returns>
    public Vector3 GetDimensions( in Vector3 vec3 )
    {
        return vec3.Set( _dim );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"> The <see cref="Vector3" /> to receive the minimum values.</param>
    /// <returns> The vector specified with the out argument</returns>
    public Vector3 GetMin( in Vector3 vec3 )
    {
        return vec3.Set( min );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3">The <see cref="Vector3" /> to receive the maximum values.</param>
    /// <returns> The vector specified with the out argument</returns>
    public Vector3 GetMax( in Vector3 vec3 )
    {
        return vec3.Set( max );
    }

    /// <summary>
    ///     Sets the minimum and maximum vector to positive and negative infinity.
    /// </summary>
    /// <returns> This bounding box for chaining.  </returns>
    public BoundingBox ToInfinity()
    {
        min.Set( float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity );
        max.Set( float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity );

        _cnt.Set( 0, 0, 0 );
        _dim.Set( 0, 0, 0 );

        return this;
    }

    /// <summary>
    ///     Extends the bounding box to incorporate the given <see cref="Vector3" />.
    /// </summary>
    /// <param name="point"> The vector</param>
    /// <returns> This bounding box for chaining.</returns>
    public BoundingBox Extend( Vector3 point )
    {
        return Set(
            min.Set( Min( min.X, point.X ), Min( min.Y, point.Y ), Min( min.Z, point.Z ) ),
            max.Set( Max( max.X, point.X ), Max( max.Y, point.Y ), Max( max.Z, point.Z ) )
            );
    }

    /// <summary>
    ///     Sets the minimum and maximum vector to zeros.
    /// </summary>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Clear()
    {
        return Set( min.Set( 0, 0, 0 ), max.Set( 0, 0, 0 ) );
    }

    /// <summary>
    ///     Extends this bounding box by the given bounding box.
    /// </summary>
    /// <param name="aBounds">The bounding box</param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Extend( BoundingBox aBounds )
    {
        return Set(
            min.Set( Min( min.X, aBounds.min.X ), Min( min.Y, aBounds.min.Y ), Min( min.Z, aBounds.min.Z ) ),
            max.Set( Max( max.X, aBounds.max.X ), Max( max.Y, aBounds.max.Y ), Max( max.Z, aBounds.max.Z ) )
            );
    }

    /// <summary>
    ///     Extends this bounding box by the given sphere.
    /// </summary>
    /// <param name="center">Sphere center</param>
    /// <param name="radius">Sphere radius</param>
    /// <returns> This bounding box for chaining.</returns>
    public BoundingBox Extend( Vector3 center, float radius )
    {
        return Set(
            min.Set( Min( min.X, center.X - radius ), Min( min.Y, center.Y - radius ), Min( min.Z, center.Z - radius ) ),
            max.Set( Max( max.X, center.X + radius ), Max( max.Y, center.Y + radius ), Max( max.Z, center.Z + radius ) )
            );
    }

    /// <summary>
    ///     Extends this bounding box by the given transformed bounding box.
    /// </summary>
    /// <param name="bounds">The bounding box</param>
    /// <param name="transform">
    ///     The transformation matrix to apply to bounds, before using it
    ///     to extend this bounding box.
    /// </param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Extend( BoundingBox bounds, Matrix4 transform )
    {
        Extend( TmpVector.Set( bounds.min.X, bounds.min.Y, bounds.min.Z ).Mul( transform ) );
        Extend( TmpVector.Set( bounds.min.X, bounds.min.Y, bounds.max.Z ).Mul( transform ) );
        Extend( TmpVector.Set( bounds.min.X, bounds.max.Y, bounds.min.Z ).Mul( transform ) );
        Extend( TmpVector.Set( bounds.min.X, bounds.max.Y, bounds.max.Z ).Mul( transform ) );
        Extend( TmpVector.Set( bounds.max.X, bounds.min.Y, bounds.min.Z ).Mul( transform ) );
        Extend( TmpVector.Set( bounds.max.X, bounds.min.Y, bounds.max.Z ).Mul( transform ) );
        Extend( TmpVector.Set( bounds.max.X, bounds.max.Y, bounds.min.Z ).Mul( transform ) );
        Extend( TmpVector.Set( bounds.max.X, bounds.max.Y, bounds.max.Z ).Mul( transform ) );

        return this;
    }

    /// <summary>
    ///     Multiplies the bounding box by the given matrix. This is achieved by
    ///     multiplying the 8 corner points and then calculating the minimum and
    ///     maximum vectors from the transformed points.
    /// </summary>
    /// <param name="transform">The matrix</param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Multiply( Matrix4 transform )
    {
        float x0 = min.X,
              y0 = min.Y,
              z0 = min.Z,
              x1 = max.X,
              y1 = max.Y,
              z1 = max.Z;

        ToInfinity();

        Extend( TmpVector.Set( x0, y0, z0 ).Mul( transform ) );
        Extend( TmpVector.Set( x0, y0, z1 ).Mul( transform ) );
        Extend( TmpVector.Set( x0, y1, z0 ).Mul( transform ) );
        Extend( TmpVector.Set( x0, y1, z1 ).Mul( transform ) );
        Extend( TmpVector.Set( x1, y0, z0 ).Mul( transform ) );
        Extend( TmpVector.Set( x1, y0, z1 ).Mul( transform ) );
        Extend( TmpVector.Set( x1, y1, z0 ).Mul( transform ) );
        Extend( TmpVector.Set( x1, y1, z1 ).Mul( transform ) );

        return this;
    }

    /// <summary>
    ///     Returns whether the given bounding box is contained in this bounding box.
    /// </summary>
    /// <param name="b">The bounding box</param>
    /// <returns>Whether the given bounding box is contained</returns>
    public bool Contains( BoundingBox b )
    {
        return !Valid
            || ( ( min.X <= b.min.X )
              && ( min.Y <= b.min.Y )
              && ( min.Z <= b.min.Z )
              && ( max.X >= b.max.X )
              && ( max.Y >= b.max.Y )
              && ( max.Z >= b.max.Z ) );
    }

    /// <summary>
    ///     Returns whether the given bounding box is intersecting this
    ///     bounding box (at least one point in).
    /// </summary>
    /// <param name="b">The bounding box</param>
    /// <returns>Whether the given bounding box is intersected</returns>
    public bool Intersects( BoundingBox b )
    {
        if ( !Valid )
        {
            return false;
        }

        // test using SAT (separating axis theorem)

        var lx   = Math.Abs( _cnt.X - b._cnt.X );
        var sumx = ( _dim.X / 2.0f ) + ( b._dim.X / 2.0f );

        var ly   = Math.Abs( _cnt.Y - b._cnt.Y );
        var sumy = ( _dim.Y / 2.0f ) + ( b._dim.Y / 2.0f );

        var lz   = Math.Abs( _cnt.Z - b._cnt.Z );
        var sumz = ( _dim.Z / 2.0f ) + ( b._dim.Z / 2.0f );

        return ( lx <= sumx ) && ( ly <= sumy ) && ( lz <= sumz );
    }

    /// <summary>
    ///     Returns whether the given vector is contained in this bounding box.
    /// </summary>
    /// <param name="v"> The vector </param>
    /// <returns> Whether the vector is contained or not.  </returns>
    public bool Contains( Vector3 v )
    {
        return ( min.X <= v.X )
            && ( max.X >= v.X )
            && ( min.Y <= v.Y )
            && ( max.Y >= v.Y )
            && ( min.Z <= v.Z )
            && ( max.Z >= v.Z );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[ {min} | {max} ]";
    }

    /// <summary>
    ///     Extends the bounding box by the given vector.
    /// </summary>
    /// <param name="x">The x-coordinate</param>
    /// <param name="y">The y-coordinate</param>
    /// <param name="z">The z-coordinate</param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Extend( float x, float y, float z )
    {
        return Set(
            min.Set( Min( min.X, x ), Min( min.Y, y ), Min( min.Z, z ) ),
            max.Set( Max( max.X, x ), Max( max.Y, y ), Max( max.Z, z ) )
            );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Min( in float a, in float b )
    {
        return a > b ? b : a;
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Max( in float a, in float b )
    {
        return a > b ? a : b;
    }
}
