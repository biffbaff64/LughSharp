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


namespace LughSharp.Lugh.Maths.Collision;

/// <summary>
/// Encapsulates an axis aligned bounding box represented by a minimum
/// and a maximum Vector. Additionally you can query for the bounding
/// box's center, dimensions and corner points.
/// </summary>
[Serializable, PublicAPI]
public class BoundingBox
{
    // ========================================================================

    private static readonly Vector3 _tmpVector = new();

    private readonly Vector3 _cnt = new();
    private readonly Vector3 _dim = new();

    // ========================================================================

    /// <summary>
    /// Constructs a new bounding box with the minimum and maximum vector set to zeros.
    /// </summary>
    public BoundingBox()
    {
        Clear();
    }

    /// <summary>
    /// Constructs a new bounding box from the given bounding box.
    /// </summary>
    /// <param name="bounds"> The bounding box to copy  </param>
    public BoundingBox( BoundingBox bounds )
    {
        Set( bounds );
    }

    /// <summary>
    /// Constructs the new bounding box using the given minimum and maximum vector.
    /// </summary>
    /// <param name="minimum"> The minimum vector </param>
    /// <param name="maximum"> The maximum vector  </param>
    public BoundingBox( Vector3 minimum, Vector3 maximum )
    {
        Set( minimum, maximum );
    }

    public Vector3 Max { get; set; } = new();
    public Vector3 Min { get; set; } = new();

    // ========================================================================

    public float CenterX => _cnt.X;
    public float CenterY => _cnt.Y;
    public float CenterZ => _cnt.Z;

    public float Width  => _dim.X;
    public float Height => _dim.Y;
    public float Depth  => _dim.Z;

    // ========================================================================

    /// <summary>
    /// Returns whether this bounding box is valid.
    /// This means that <see cref="Max"/> is greater than or equal to <see cref="Min"/>.
    /// </summary>
    /// <returns> True in case the bounding box is valid, false otherwise  </returns>
    public bool Valid => ( Min.X <= Max.X ) && ( Min.Y <= Max.Y ) && ( Min.Z <= Max.Z );

    /// <summary>
    /// Sets the given bounding box.
    /// </summary>
    /// <param name="bounds">The bounds.</param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Set( BoundingBox bounds )
    {
        return Set( bounds.Min, bounds.Max );
    }

    /// <summary>
    /// Sets the given minimum and maximum vector.
    /// </summary>
    /// <param name="minimum"> The minimum vector </param>
    /// <param name="maximum"> The maximum vector </param>
    /// <returns> This bounding box for chaining.  </returns>
    public BoundingBox Set( Vector3 minimum, Vector3 maximum )
    {
        Min.Set( minimum.X < maximum.X ? minimum.X : maximum.X,
                 minimum.Y < maximum.Y ? minimum.Y : maximum.Y,
                 minimum.Z < maximum.Z ? minimum.Z : maximum.Z );

        Max.Set( minimum.X > maximum.X ? minimum.X : maximum.X,
                 minimum.Y > maximum.Y ? minimum.Y : maximum.Y,
                 minimum.Z > maximum.Z ? minimum.Z : maximum.Z );

        _cnt.Set( Min ).Add( Max ).Scale( 0.5f );
        _dim.Set( Max ).Sub( Min );

        return this;
    }

    /// <summary>
    /// Sets the bounding box minimum and maximum vector from the given points.
    /// </summary>
    /// <param name="points"> The points. </param>
    /// <returns> This bounding box for chaining.  </returns>
    public BoundingBox Set( Vector3[] points )
    {
        ToInfinity();

        foreach ( var lPoint in points )
        {
            Extend( lPoint );
        }

        return this;
    }

    /// <summary>
    /// Sets the bounding box minimum and maximum vector from the given points.
    /// </summary>
    /// <param name="points"> The points. </param>
    /// <returns> This bounding box for chaining.  </returns>
    public BoundingBox Set( IEnumerable< Vector3 > points )
    {
        ToInfinity();

        foreach ( var lPoint in points )
        {
            Extend( lPoint );
        }

        return this;
    }

    /// <summary>
    /// Gets the centre of the bounding box and places it into the supplied
    /// Vector3. The modified vector is returned but can be thrown away.
    /// </summary>
    /// <param name="vec3">
    /// The <see cref="Vector3"/> to receive the center of the bounding box.
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
        return vec3.Set( Min.X, Min.Y, Min.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner001( in Vector3 vec3 )
    {
        return vec3.Set( Min.X, Min.Y, Max.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner010( in Vector3 vec3 )
    {
        return vec3.Set( Min.X, Max.Y, Min.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner011( in Vector3 vec3 )
    {
        return vec3.Set( Min.X, Max.Y, Max.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner100( in Vector3 vec3 )
    {
        return vec3.Set( Max.X, Min.Y, Min.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner101( in Vector3 vec3 )
    {
        return vec3.Set( Max.X, Min.Y, Max.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner110( in Vector3 vec3 )
    {
        return vec3.Set( Max.X, Max.Y, Min.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public Vector3 GetCorner111( in Vector3 vec3 )
    {
        return vec3.Set( Max.X, Max.Y, Max.Z );
    }

    /// <summary>
    /// </summary>
    /// <param name="vec3">
    /// The <see cref="Vector3"/> to receive the dimensions of this bounding box on all three axis.
    /// </param>
    /// <returns> The vector specified with the vec3 argument</returns>
    public Vector3 GetDimensions( in Vector3 vec3 )
    {
        return vec3.Set( _dim );
    }

    /// <summary>
    /// Gets the minimum values into a <see cref="Vector3"/>.
    /// </summary>
    /// <param name="vec3"> The <see cref="Vector3"/> to receive the minimum values.</param>
    /// <returns> The vector specified with the out argument</returns>
    public Vector3 GetMin( in Vector3 vec3 )
    {
        return vec3.Set( Min );
    }

    /// <summary>
    /// Gets the maximum values into a <see cref="Vector3"/>.
    /// </summary>
    /// <param name="vec3">The <see cref="Vector3"/> to receive the maximum values.</param>
    /// <returns> The vector specified with the out argument</returns>
    public Vector3 GetMax( in Vector3 vec3 )
    {
        return vec3.Set( Max );
    }

    /// <summary>
    /// Sets the minimum and maximum vector to positive and negative infinity.
    /// </summary>
    /// <returns> This bounding box for chaining.  </returns>
    public BoundingBox ToInfinity()
    {
        Min.Set( float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity );
        Max.Set( float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity );

        _cnt.Set( 0, 0, 0 );
        _dim.Set( 0, 0, 0 );

        return this;
    }

    /// <summary>
    /// Extends the bounding box to incorporate the given <see cref="Vector3"/>.
    /// </summary>
    /// <param name="point"> The vector</param>
    /// <returns> This bounding box for chaining.</returns>
    public BoundingBox Extend( Vector3 point )
    {
        return Set( Min.Set( Math.Min( Min.X, point.X ),
                             Math.Min( Min.Y, point.Y ),
                             Math.Min( Min.Z, point.Z ) ),
                    Max.Set( Math.Max( Max.X, point.X ),
                             Math.Max( Max.Y, point.Y ),
                             Math.Max( Max.Z, point.Z ) ) );
    }

    /// <summary>
    /// Sets the minimum and maximum vector to zeros.
    /// </summary>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Clear()
    {
        return Set( Min.Set( 0, 0, 0 ), Max.Set( 0, 0, 0 ) );
    }

    /// <summary>
    /// Extends this bounding box by the given bounding box.
    /// </summary>
    /// <param name="aBounds">The bounding box</param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Extend( BoundingBox aBounds )
    {
        return Set( Min.Set( Math.Min( Min.X, aBounds.Min.X ),
                             Math.Min( Min.Y, aBounds.Min.Y ),
                             Math.Min( Min.Z, aBounds.Min.Z ) ),
                    Max.Set( Math.Max( Max.X, aBounds.Max.X ),
                             Math.Max( Max.Y, aBounds.Max.Y ),
                             Math.Max( Max.Z, aBounds.Max.Z ) ) );
    }

    /// <summary>
    /// Extends this bounding box by the given sphere.
    /// </summary>
    /// <param name="center">Sphere center</param>
    /// <param name="radius">Sphere radius</param>
    /// <returns> This bounding box for chaining.</returns>
    public BoundingBox Extend( Vector3 center, float radius )
    {
        return Set( Min.Set( Math.Min( Min.X, center.X - radius ),
                             Math.Min( Min.Y, center.Y - radius ),
                             Math.Min( Min.Z, center.Z - radius ) ),
                    Max.Set( Math.Max( Max.X, center.X + radius ),
                             Math.Max( Max.Y, center.Y + radius ),
                             Math.Max( Max.Z, center.Z + radius ) ) );
    }

    /// <summary>
    /// Extends this bounding box by the given transformed bounding box.
    /// </summary>
    /// <param name="bounds">The bounding box</param>
    /// <param name="transform">
    /// The transformation matrix to apply to bounds, before using it
    /// to extend this bounding box.
    /// </param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Extend( BoundingBox bounds, Matrix4 transform )
    {
        Extend( _tmpVector.Set( bounds.Min.X, bounds.Min.Y, bounds.Min.Z ).Mul( transform ) );
        Extend( _tmpVector.Set( bounds.Min.X, bounds.Min.Y, bounds.Max.Z ).Mul( transform ) );
        Extend( _tmpVector.Set( bounds.Min.X, bounds.Max.Y, bounds.Min.Z ).Mul( transform ) );
        Extend( _tmpVector.Set( bounds.Min.X, bounds.Max.Y, bounds.Max.Z ).Mul( transform ) );
        Extend( _tmpVector.Set( bounds.Max.X, bounds.Min.Y, bounds.Min.Z ).Mul( transform ) );
        Extend( _tmpVector.Set( bounds.Max.X, bounds.Min.Y, bounds.Max.Z ).Mul( transform ) );
        Extend( _tmpVector.Set( bounds.Max.X, bounds.Max.Y, bounds.Min.Z ).Mul( transform ) );
        Extend( _tmpVector.Set( bounds.Max.X, bounds.Max.Y, bounds.Max.Z ).Mul( transform ) );

        return this;
    }

    /// <summary>
    /// Multiplies the bounding box by the given matrix. This is achieved by
    /// multiplying the 8 corner points and then calculating the minimum and
    /// maximum vectors from the transformed points.
    /// </summary>
    /// <param name="transform">The matrix</param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Multiply( Matrix4 transform )
    {
        var x0 = Min.X;
        var y0 = Min.Y;
        var z0 = Min.Z;
        var x1 = Max.X;
        var y1 = Max.Y;
        var z1 = Max.Z;

        ToInfinity();

        Extend( _tmpVector.Set( x0, y0, z0 ).Mul( transform ) );
        Extend( _tmpVector.Set( x0, y0, z1 ).Mul( transform ) );
        Extend( _tmpVector.Set( x0, y1, z0 ).Mul( transform ) );
        Extend( _tmpVector.Set( x0, y1, z1 ).Mul( transform ) );
        Extend( _tmpVector.Set( x1, y0, z0 ).Mul( transform ) );
        Extend( _tmpVector.Set( x1, y0, z1 ).Mul( transform ) );
        Extend( _tmpVector.Set( x1, y1, z0 ).Mul( transform ) );
        Extend( _tmpVector.Set( x1, y1, z1 ).Mul( transform ) );

        return this;
    }

    /// <summary>
    /// Returns whether the given bounding box is contained in this bounding box.
    /// </summary>
    /// <param name="b">The bounding box</param>
    /// <returns>Whether the given bounding box is contained</returns>
    public bool Contains( BoundingBox b )
    {
        return !Valid
            || ( ( Min.X <= b.Min.X )
              && ( Min.Y <= b.Min.Y )
              && ( Min.Z <= b.Min.Z )
              && ( Max.X >= b.Max.X )
              && ( Max.Y >= b.Max.Y )
              && ( Max.Z >= b.Max.Z ) );
    }

    /// <summary>
    /// Returns whether the given bounding box is intersecting this
    /// bounding box (at least one point in).
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
    /// Returns whether the given vector is contained in this bounding box.
    /// </summary>
    /// <param name="v"> The vector </param>
    /// <returns> Whether the vector is contained or not.  </returns>
    public bool Contains( Vector3 v )
    {
        return ( Min.X <= v.X )
            && ( Max.X >= v.X )
            && ( Min.Y <= v.Y )
            && ( Max.Y >= v.Y )
            && ( Min.Z <= v.Z )
            && ( Max.Z >= v.Z );
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[ {Min} | {Max} ]";
    }

    /// <summary>
    /// Extends the bounding box by the given vector.
    /// </summary>
    /// <param name="x">The x-coordinate</param>
    /// <param name="y">The y-coordinate</param>
    /// <param name="z">The z-coordinate</param>
    /// <returns>This bounding box for chaining.</returns>
    public BoundingBox Extend( float x, float y, float z )
    {
        return Set( Min.Set( Math.Min( Min.X, x ), Math.Min( Min.Y, y ), Math.Min( Min.Z, z ) ),
                    Max.Set( Math.Max( Max.X, x ), Math.Max( Max.Y, y ), Math.Max( Max.Z, z ) ) );
    }
}
