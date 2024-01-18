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
///     A point in a 3D grid, with integer x and y coordinates
/// </summary>
public class GridPoint3
{
    /// <summary>
    ///     Constructs a 3D grid point with all coordinates pointing to the origin (0, 0, 0).
    /// </summary>
    public GridPoint3()
    {
    }

    /// <summary>
    ///     Constructs a 3D grid point.
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
    ///     Copy constructor
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
    ///     Sets the coordinates of this 3D grid point to that of another.
    /// </summary>
    /// <param name="point">
    ///     The 3D grid point to copy coordinates of.
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
    ///     Sets the coordinates of this GridPoint3D.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="z">
    ///     Z coordinate
    /// </param>
    /// <returns> this GridPoint3D for chaining.  </returns>
    public virtual GridPoint3 Set( int x, int y, int z )
    {
        X = x;
        Y = y;
        Z = z;

        return this;
    }

    /// <param name="other"> The other point </param>
    /// <returns> the squared distance between this point and the other point. </returns>
    public virtual float Dst2( GridPoint3 other )
    {
        var xd = other.X - X;
        var yd = other.Y - Y;
        var zd = other.Z - Z;

        return ( xd * xd ) + ( yd * yd ) + ( zd * zd );
    }

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

    /// <param name="other"> The other point </param>
    /// <returns> the distance between this point and the other vector. </returns>
    public virtual float Dst( GridPoint3 other )
    {
        var xd = other.X - X;
        var yd = other.Y - Y;
        var zd = other.Z - Z;

        return ( float )Math.Sqrt( ( xd * xd ) + ( yd * yd ) + ( zd * zd ) );
    }

    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <param name="z"> The z-coordinate of the other point </param>
    /// <returns> the distance between this point and the other point. </returns>
    public virtual float Dst( int x, int y, int z )
    {
        var xd = x - X;
        var yd = y - Y;
        var zd = z - Z;

        return ( float )Math.Sqrt( ( xd * xd ) + ( yd * yd ) + ( zd * zd ) );
    }

    /// <summary>
    ///     Adds another 3D grid point to this point.
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
    ///     Adds another 3D grid point to this point.
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
    ///     Subtracts another 3D grid point from this point.
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
    ///     Subtracts another 3D grid point from this point.
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

    /// <returns> a copy of this grid point </returns>
    public virtual GridPoint3 Cpy() => new( this );

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

        var g = ( GridPoint3 )o;

        return ( X == g.X ) && ( Y == g.Y ) && ( Z == g.Z );
    }

    public override int GetHashCode()
    {
        const int prime = 17;

        var result = prime + X;
        result = ( prime * result ) + Y;
        result = ( prime * result ) + Z;

        return result;
    }

    public override string ToString() => $"({X}, {Y}, {Z})";
}
