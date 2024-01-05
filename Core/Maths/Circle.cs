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
/// </summary>
public class Circle : IShape2D
{

    /// <summary>
    ///     Constructs a new circle with all values set to zero
    /// </summary>
    public Circle() : this( 0, 0, 0 )
    {
    }

    /// <summary>
    ///     Constructs a new circle with the given X and Y coordinates and the given radius.
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
    ///     Constructs a new circle using a given <see cref="Vector2" /> that contains the desired X and Y coordinates, and a
    ///     given radius.
    /// </summary>
    /// <param name="position"> The position <see cref="Vector2" />. </param>
    /// <param name="radius"> The radius  </param>
    public Circle( Vector2 position, float radius )
    {
        X      = position.X;
        Y      = position.Y;
        Radius = radius;
    }

    /// <summary>
    ///     Copy constructor
    /// </summary>
    /// <param name="circle"> The circle to construct a copy of.  </param>
    public Circle( Circle circle )
    {
        X      = circle.X;
        Y      = circle.Y;
        Radius = circle.Radius;
    }

    /// <summary>
    ///     Creates a new <see cref="Circle" /> in terms of its center and a point on its edge.
    /// </summary>
    /// <param name="center"> The center of the new circle </param>
    /// <param name="edge"> Any point on the edge of the given circle  </param>
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
    ///     Checks whether or not this circle contains a given point.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y">
    ///     Y coordinate
    /// </param>
    /// <returns> true if this circle contains the given point.  </returns>
    public bool Contains( float x, float y )
    {
        x = X - x;
        y = Y - y;

        return ( ( x * x ) + ( y * y ) ) <= ( Radius * Radius );
    }

    /// <summary>
    ///     Checks whether or not this circle contains a given point.
    /// </summary>
    /// <param name="point">
    ///     The <see cref="Vector2" /> that contains the point coordinates.
    /// </param>
    /// <returns> true if this circle contains this point; false otherwise.  </returns>
    public bool Contains( Vector2 point )
    {
        var dx = X - point.X;
        var dy = Y - point.Y;

        return ( ( dx * dx ) + ( dy * dy ) ) <= ( Radius * Radius );
    }

    /// <summary>
    ///     Sets a new location and radius for this circle.
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
    ///     Sets a new location and radius for this circle.
    /// </summary>
    /// <param name="position"> Position <see cref="Vector2" /> for this circle. </param>
    /// <param name="radius"> Circle radius  </param>
    public void Set( Vector2 position, float radius )
    {
        X      = position.X;
        Y      = position.Y;
        Radius = radius;
    }

    /// <summary>
    ///     Sets a new location and radius for this circle, based upon another circle.
    /// </summary>
    /// <param name="circle"> The circle to copy the position and radius of.  </param>
    public void Set( Circle circle )
    {
        X      = circle.X;
        Y      = circle.Y;
        Radius = circle.Radius;
    }

    /// <summary>
    ///     Sets this <see cref="Circle" />'s values in terms of its center and a point on its edge.
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
    ///     Sets the x and y-coordinates of circle center from vector
    /// </summary>
    /// <param name="position"> The position vector  </param>
    public void SetPosition( Vector2 position )
    {
        X = position.X;
        Y = position.Y;
    }

    /// <summary>
    ///     Sets the x and y-coordinates of circle center
    /// </summary>
    /// <param name="x"> The x-coordinate </param>
    /// <param name="y"> The y-coordinate  </param>
    public void SetPosition( float x, float y )
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// </summary>
    /// <param name="c"> the other <see cref="Circle" /> </param>
    /// <returns> whether this circle contains the other circle.  </returns>
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
    /// </summary>
    /// <param name="c"> the other <see cref="Circle" /> </param>
    /// <returns> whether this circle overlaps the other circle.  </returns>
    public bool Overlaps( Circle c )
    {
        var dx        = X - c.X;
        var dy        = Y - c.Y;
        var distance  = ( dx * dx ) + ( dy * dy );
        var radiusSum = Radius + c.Radius;

        return distance < ( radiusSum * radiusSum );
    }

    /// <summary>
    ///     Returns a <see cref="string" /> representation of this
    ///     <see cref="Circle" /> of the form <code>x,y,radius</code>.
    /// </summary>
    public override string ToString() => X + "," + Y + "," + Radius;

    /// <summary>
    /// </summary>
    /// <returns>
    ///     The circumference of this circle:-
    ///     (as 2 * <see cref="MathUtils.PI2" />) * <code>radius</code>
    /// </returns>
    public float Circumference() => Radius * MathUtils.PI2;

    /// <summary>
    /// </summary>
    /// <returns>
    ///     The area of this circle (as <see cref="MathUtils.PI" /> * radius * radius).
    /// </returns>
    public float Area() => Radius * Radius * MathUtils.PI;

    /// <summary>
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
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

        var c = ( Circle )o;

        return MathUtils.IsEqual( X, c.X )
            || MathUtils.IsEqual( Y, c.Y )
            || MathUtils.IsEqual( Radius, c.Radius );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        const int prime = 41;

        var result = 1;

        result = prime + NumberUtils.FloatToRawIntBits( Radius );
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( X );
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( Y );

        return result;
    }
}
