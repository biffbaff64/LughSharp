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


namespace Corelib.Lugh.Maths;

/// <summary>
/// A point in a 3D grid, with integer x and y coordinates
/// </summary>
[PublicAPI]
public class GridPoint3
{
    /// <summary>
    /// Constructs a 3D grid point with all coordinates pointing to the origin (0, 0, 0).
    /// </summary>
    public GridPoint3() : this( 0, 0, 0 )
    {
    }

    /// <summary>
    /// Constructs a 3D grid point.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="z"> Z coordinate  </param>
    public GridPoint3( int x, int y, int z )
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="point"> The 3D grid point to make a copy of.  </param>
    public GridPoint3( GridPoint3 point )
    {
        X = point.X;
        Y = point.Y;
        Z = point.Z;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    /// <summary>
    /// Sets the coordinates of this 3D grid point to that of another.
    /// </summary>
    /// <param name="point">
    /// The 3D grid point to copy coordinates of.
    /// </param>
    /// <returns> this GridPoint3 for chaining.  </returns>
    public virtual GridPoint3 Set( GridPoint3 point )
    {
        X = point.X;
        Y = point.Y;
        Z = point.Z;

        return this;
    }

    /// <summary>
    /// Sets the coordinates of this GridPoint3D.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="z">
    /// Z coordinate
    /// </param>
    /// <returns> this GridPoint3D for chaining.  </returns>
    public virtual GridPoint3 Set( int x, int y, int z )
    {
        X = x;
        Y = y;
        Z = z;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="other"> The other point </param>
    /// <returns> the squared distance between this point and the other point. </returns>
    public virtual float Dst2( GridPoint3 other )
    {
        var xd = other.X - X;
        var yd = other.Y - Y;
        var zd = other.Z - Z;

        return ( xd * xd ) + ( yd * yd ) + ( zd * zd );
    }

    /// <summary>
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <param name="z"> The z-coordinate of the other point </param>
    /// <returns> the squared distance between this point and the other point. </returns>
    public virtual float Dst2( int x, int y, int z )
    {
        var xd = x - X;
        var yd = y - Y;
        var zd = z - Z;

        return ( xd * xd ) + ( yd * yd ) + ( zd * zd );
    }

    /// <summary>
    /// </summary>
    /// <param name="other"> The other point </param>
    /// <returns> the distance between this point and the other vector. </returns>
    public virtual float Dst( GridPoint3 other )
    {
        var xd = other.X - X;
        var yd = other.Y - Y;
        var zd = other.Z - Z;

        return ( float ) Math.Sqrt( ( xd * xd ) + ( yd * yd ) + ( zd * zd ) );
    }

    /// <summary>
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <param name="z"> The z-coordinate of the other point </param>
    /// <returns> the distance between this point and the other point. </returns>
    public virtual float Dst( int x, int y, int z )
    {
        var xd = x - X;
        var yd = y - Y;
        var zd = z - Z;

        return ( float ) Math.Sqrt( ( xd * xd ) + ( yd * yd ) + ( zd * zd ) );
    }

    /// <summary>
    /// Adds another 3D grid point to this point.
    /// </summary>
    /// <param name="other"> The other point </param>
    /// <returns> this 3d grid point for chaining. </returns>
    public virtual GridPoint3 Add( GridPoint3 other )
    {
        X += other.X;
        Y += other.Y;
        Z += other.Z;

        return this;
    }

    /// <summary>
    /// Adds another 3D grid point to this point.
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <param name="z"> The z-coordinate of the other point </param>
    /// <returns> this 3d grid point for chaining. </returns>
    public virtual GridPoint3 Add( int x, int y, int z )
    {
        X += x;
        Y += y;
        Z += z;

        return this;
    }

    /// <summary>
    /// Subtracts another 3D grid point from this point.
    /// </summary>
    /// <param name="other"> The other point </param>
    /// <returns> this 3d grid point for chaining. </returns>
    public virtual GridPoint3 Sub( GridPoint3 other )
    {
        X -= other.X;
        Y -= other.Y;
        Z -= other.Z;

        return this;
    }

    /// <summary>
    /// Subtracts another 3D grid point from this point.
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <param name="z"> The z-coordinate of the other point </param>
    /// <returns> this 3d grid point for chaining. </returns>
    public virtual GridPoint3 Sub( int x, int y, int z )
    {
        X -= x;
        Y -= y;
        Z -= z;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <returns> a copy of this grid point </returns>
    public virtual GridPoint3 Cpy()
    {
        return new GridPoint3( this );
    }

    /// <inheritdoc />
    public override bool Equals( object? o )
    {
        if ( this == o )
        {
            return true;
        }

        if ( ( o == null ) || ( o.GetType() != GetType() ) )
        {
            return false;
        }

        var g = ( GridPoint3 ) o;

        return ( X == g.X ) && ( Y == g.Y ) && ( Z == g.Z );
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        const int PRIME = 17;

        var result = PRIME + 31;
        result = ( PRIME * result ) + 33;
        result = ( PRIME * result ) + 33;

        return result;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}
