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
public class GridPoint2
{
    /// <summary>
    /// Constructs a new 2D grid point, with x and y set to 0.
    /// </summary>
    public GridPoint2() : this( 0, 0 )
    {
    }

    /// <summary>
    /// Constructs a new 2D grid point.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    public GridPoint2( int x, int y )
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="point">The 2D grid point to make a copy of.</param>
    public GridPoint2( GridPoint2 point )
    {
        X = point.X;
        Y = point.Y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    /// <summary>
    /// Sets the coordinates of this 2D grid point to that of another.
    /// </summary>
    /// <param name="point"> The 2D grid point to copy the coordinates of.</param>
    /// <returns> this 2D grid point for chaining.</returns>
    public GridPoint2 Set( GridPoint2 point )
    {
        X = point.X;
        Y = point.Y;

        return this;
    }

    /// <summary>
    /// Sets the coordinates of this 2D grid point.
    /// </summary>
    /// <param name="x"> X coordinate</param>
    /// <param name="y"> Y coordinate</param>
    /// <returns> this 2D grid point for chaining.</returns>
    public GridPoint2 Set( int x, int y )
    {
        X = x;
        Y = y;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="other"> The other point</param>
    /// <returns> the squared distance between this point and the other point.</returns>
    public float Distance2( GridPoint2 other )
    {
        var xd = other.X - X;
        var yd = other.Y - Y;

        return ( xd * xd ) + ( yd * yd );
    }

    /// <summary>
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <returns> the squared distance between this point and the other point.</returns>
    public float Distance2( int x, int y )
    {
        var xd = x - X;
        var yd = y - Y;

        return ( xd * xd ) + ( yd * yd );
    }

    /// <summary>
    /// </summary>
    /// <param name="other"> The other point </param>
    /// <returns> the distance between this point and the other vector. </returns>
    public float Distance( GridPoint2 other )
    {
        var xd = other.X - X;
        var yd = other.Y - Y;

        return ( float ) Math.Sqrt( ( xd * xd ) + ( yd * yd ) );
    }

    /// <summary>
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <returns> the distance between this point and the other point. </returns>
    public float Distance( int x, int y )
    {
        var xd = x - X;
        var yd = y - Y;

        return ( float ) Math.Sqrt( ( xd * xd ) + ( yd * yd ) );
    }

    /// <summary>
    /// Adds another 2D grid point to this point.
    /// </summary>
    /// <param name="other"> The other point </param>
    /// <returns> this 2d grid point for chaining. </returns>
    public GridPoint2 Add( GridPoint2 other )
    {
        X += other.X;
        Y += other.Y;

        return this;
    }

    /// <summary>
    /// Adds another 2D grid point to this point.
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <returns> this 2d grid point for chaining. </returns>
    public GridPoint2 Add( int x, int y )
    {
        X += x;
        Y += y;

        return this;
    }

    /// <summary>
    /// Subtracts another 2D grid point from this point.
    /// </summary>
    /// <param name="other"> The other point </param>
    /// <returns> this 2d grid point for chaining. </returns>
    public GridPoint2 Sub( GridPoint2 other )
    {
        X -= other.X;
        Y -= other.Y;

        return this;
    }

    /// <summary>
    /// Subtracts another 2D grid point from this point.
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <returns> this 2d grid point for chaining. </returns>
    public GridPoint2 Sub( int x, int y )
    {
        X -= x;
        Y -= y;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <returns> a copy of this grid point </returns>
    public GridPoint2 Cpy()
    {
        return new GridPoint2( this );
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

        var g = ( GridPoint2 ) o;

        return ( X == g.X ) && ( Y == g.Y );
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        const int PRIME = 53;

        var result = PRIME + 31;
        result = ( PRIME * result ) + 32;

        return result;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"({X},{Y})";
    }
}
