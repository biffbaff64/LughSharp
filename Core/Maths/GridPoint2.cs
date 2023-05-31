using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maths;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class GridPoint2
{
    public int X { get; set; }
    public int Y { get; set; }

    /// <summary>
    /// Constructs a new 2D grid point.
    /// </summary>
    public GridPoint2()
    {
    }

    /// <summary>
    /// Constructs a new 2D grid point.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    public GridPoint2( int x, int y )
    {
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="point">The 2D grid point to make a copy of.</param>
    public GridPoint2( GridPoint2 point )
    {
        this.X = point.X;
        this.Y = point.Y;
    }

    /// <summary>
    /// Sets the coordinates of this 2D grid point to that of another.
    /// </summary>
    /// <param name="point"> The 2D grid point to copy the coordinates of.</param>
    /// <returns> this 2D grid point for chaining.</returns>
    public GridPoint2 Set( GridPoint2 point )
    {
        this.X = point.X;
        this.Y = point.Y;

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
        this.X = x;
        this.Y = y;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="other"> The other point</param>
    /// <returns> the squared distance between this point and the other point.</returns>
    public float Dst2( GridPoint2 other )
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
    public float Dst2( int x, int y )
    {
        var xd = x - this.X;
        var yd = y - this.Y;

        return ( xd * xd ) + ( yd * yd );
    }

    /// <summary>
    /// </summary>
    /// <param name="other"> The other point </param>
    /// <returns> the distance between this point and the other vector. </returns>
    public float Dst( GridPoint2 other )
    {
        var xd = other.X - X;
        var yd = other.Y - Y;

        return ( float )Math.Sqrt( ( xd * xd ) + ( yd * yd ) );
    }

    /// <summary>
    /// </summary>
    /// <param name="x"> The x-coordinate of the other point </param>
    /// <param name="y"> The y-coordinate of the other point </param>
    /// <returns> the distance between this point and the other point. </returns>
    public float Dst( int x, int y )
    {
        var xd = x - this.X;
        var yd = y - this.Y;

        return ( float )Math.Sqrt( ( xd * xd ) + ( yd * yd ) );
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
        this.X += x;
        this.Y += y;

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
        this.X -= x;
        this.Y -= y;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <returns> a copy of this grid point </returns>
    public GridPoint2 Cpy()
    {
        return new GridPoint2( this );
    }

    public new bool Equals( object? o )
    {
        if ( this == o ) return true;
            
        if ( ( o == null ) || ( o.GetType() != this.GetType() ) ) return false;
            
        var g = ( GridPoint2 )o;

        return ( this.X == g.X ) && ( this.Y == g.Y );
    }

    public int HashCode()
    {
        var prime = 53;

        var result = prime + this.X;
        result = ( prime * result ) + this.Y;

        return result;
    }

    public new string ToString() => $"({X},{Y})";
}