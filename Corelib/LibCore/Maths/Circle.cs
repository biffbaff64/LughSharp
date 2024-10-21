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

/// <summary>
/// A convenient 2D Circle class
/// </summary>
[PublicAPI]
public class Circle : IShape2D
{
    // ------------------------------------------------------------------------

    /// <summary>
    /// Constructs a new circle with all values set to zero
    /// </summary>
    public Circle() : this( 0, 0, 0 )
    {
    }

    /// <summary>
    /// Constructs a new circle with the given X and Y coordinates and the given radius.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="radius"> The radius of the circle  </param>
    public Circle( float x, float y, float radius )
    {
        X      = x;
        Y      = y;
        Radius = radius;
    }

    /// <summary>
    /// Constructs a new circle using a given <see cref="Vector2"/> that contains
    /// the desired X and Y coordinates, and a given radius.
    /// </summary>
    /// <param name="position"> The position <see cref="Vector2"/>. </param>
    /// <param name="radius"> The radius  </param>
    public Circle( Vector2 position, float radius )
    {
        X      = position.X;
        Y      = position.Y;
        Radius = radius;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="circle"> The circle to construct a copy of. </param>
    public Circle( Circle circle )
    {
        X      = circle.X;
        Y      = circle.Y;
        Radius = circle.Radius;
    }

    /// <summary>
    /// Creates a new <see cref="Circle"/> in terms of its center and a point on its edge.
    /// </summary>
    /// <param name="center"> The center of the new circle </param>
    /// <param name="edge"> Any point on the edge of the given circle </param>
    public Circle( Vector2 center, Vector2 edge )
    {
        X      = center.X;
        Y      = center.Y;
        Radius = Vector2.Len( center.X - edge.X, center.Y - edge.Y );
    }

    public float X      { get; set; }
    public float Y      { get; set; }
    public float Radius { get; set; }

    /// <summary>
    /// Checks whether or not this circle contains a given point.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <returns> true if this circle contains the given point.  </returns>
    public bool Contains( float x, float y )
    {
        x = X - x;
        y = Y - y;

        return ( ( x * x ) + ( y * y ) ) <= ( Radius * Radius );
    }

    /// <summary>
    /// Checks whether or not this circle contains a given point.
    /// </summary>
    /// <param name="point">
    /// The <see cref="Vector2"/> that contains the point coordinates.
    /// </param>
    /// <returns> true if this circle contains this point; false otherwise.  </returns>
    public bool Contains( Vector2 point )
    {
        var dx = X - point.X;
        var dy = Y - point.Y;

        return ( ( dx * dx ) + ( dy * dy ) ) <= ( Radius * Radius );
    }

    /// <summary>
    /// Sets a new location and radius for this circle.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="radius"> Circle radius  </param>
    public void Set( float x, float y, float radius )
    {
        X      = x;
        Y      = y;
        Radius = radius;
    }

    /// <summary>
    /// Sets a new location and radius for this circle.
    /// </summary>
    /// <param name="position"> Position <see cref="Vector2"/> for this circle. </param>
    /// <param name="radius"> Circle radius  </param>
    public void Set( Vector2 position, float radius )
    {
        X      = position.X;
        Y      = position.Y;
        Radius = radius;
    }

    /// <summary>
    /// Sets a new location and radius for this circle, based upon another circle.
    /// </summary>
    /// <param name="circle"> The circle to copy the position and radius of.  </param>
    public void Set( Circle circle )
    {
        X      = circle.X;
        Y      = circle.Y;
        Radius = circle.Radius;
    }

    /// <summary>
    /// Sets this <see cref="Circle"/>'s values in terms of its center and a point on its edge.
    /// </summary>
    /// <param name="center"> The new center of the circle </param>
    /// <param name="edge"> Any point on the edge of the given circle  </param>
    public void Set( Vector2 center, Vector2 edge )
    {
        X      = center.X;
        Y      = center.Y;
        Radius = Vector2.Len( center.X - edge.X, center.Y - edge.Y );
    }

    /// <summary>
    /// Sets the x and y-coordinates of circle center from vector
    /// </summary>
    /// <param name="position"> The position vector  </param>
    public void SetPosition( Vector2 position )
    {
        X = position.X;
        Y = position.Y;
    }

    /// <summary>
    /// Sets the x and y-coordinates of circle center
    /// </summary>
    /// <param name="x"> The x-coordinate </param>
    /// <param name="y"> The y-coordinate  </param>
    public void SetPosition( float x, float y )
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Returns TRUE if this circle contains the supplied reference circle.
    /// </summary>
    /// <param name="c"> the other <see cref="Circle"/> </param>
    public bool Contains( Circle c )
    {
        var radiusDiff = Radius - c.Radius;

        if ( radiusDiff < 0f )
        {
            return false; // Can't contain bigger circle
        }

        var dx        = X - c.X;
        var dy        = Y - c.Y;
        var dst       = ( dx * dx ) + ( dy * dy );
        var radiusSum = Radius + c.Radius;

        return !( ( radiusDiff * radiusDiff ) < dst ) && ( dst < ( radiusSum * radiusSum ) );
    }

    /// <summary>
    /// Returns TRUE if this circle overlaps the supplied reference circle.
    /// </summary>
    /// <param name="c"> the other <see cref="Circle"/> </param>
    public bool Overlaps( Circle c )
    {
        var dx        = X - c.X;
        var dy        = Y - c.Y;
        var distance  = ( dx * dx ) + ( dy * dy );
        var radiusSum = Radius + c.Radius;

        return distance < ( radiusSum * radiusSum );
    }

    /// <summary>
    /// Returns a <see cref="string"/> representation of this
    /// <see cref="Circle"/> of the form <tt>x,y,radius</tt>.
    /// </summary>
    public override string ToString()
    {
        return $"{X},{Y},{Radius}";
    }

    /// <summary>
    /// Returns the circumference of this circle:-
    /// (as 2 * <see cref="MathUtils.PI2"/>) * <code>radius</code>
    /// </summary>
    public float Circumference()
    {
        return Radius * MathUtils.PI2;
    }

    /// <summary>
    /// Returns the area of this circle:-
    /// (as <see cref="MathUtils.PI"/> * radius * radius).
    /// </summary>
    public float Area()
    {
        return Radius * Radius * MathUtils.PI;
    }

    /// <inheritdoc />
    public override bool Equals( object? o )
    {
        if ( o == this )
        {
            return true;
        }

        if ( ( o == null ) || ( o.GetType() != GetType() ) )
        {
            return false;
        }

        var c = ( Circle ) o;

        return MathUtils.IsEqual( X, c.X )
            || MathUtils.IsEqual( Y, c.Y )
            || MathUtils.IsEqual( Radius, c.Radius );
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        const int PRIME = 41;

        var result = PRIME + 43;
        result = ( PRIME * result ) + 45;
        result = ( PRIME * result ) + 47;

        return result;
    }
}
