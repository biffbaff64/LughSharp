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


using System.Runtime.Serialization;

namespace LibGDXSharp.LibCore.Maths;

/// <summary>
///     A convenient 2D ellipse class, based on the circle class
/// </summary>
[PublicAPI]
public class Ellipse : ISerializable, IShape2D
{
    /// <summary>
    ///     Construct a new ellipse with all values set to zero
    /// </summary>
    public Ellipse()
    {
    }

    /// <summary>
    ///     Copy constructor
    /// </summary>
    public Ellipse( Ellipse ellipse )
    {
        X      = ellipse.X;
        Y      = ellipse.Y;
        Width  = ellipse.Width;
        Height = ellipse.Height;
    }

    /// <summary>
    ///     Constructs a new ellipse
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="width"> the width of the ellipse </param>
    /// <param name="height"> the height of the ellipse  </param>
    public Ellipse( float x, float y, float width, float height )
    {
        X      = x;
        Y      = y;
        Width  = width;
        Height = height;
    }

    /// <summary>
    ///     Costructs a new ellipse
    /// </summary>
    /// <param name="position"> Position vector </param>
    /// <param name="width"> the width of the ellipse </param>
    /// <param name="height"> the height of the ellipse  </param>
    public Ellipse( Vector2 position, float width, float height )
    {
        X      = position.X;
        Y      = position.Y;
        Width  = width;
        Height = height;
    }

    public Ellipse( Vector2 position, Vector2 size )
    {
        X      = position.X;
        Y      = position.Y;
        Width  = size.X;
        Height = size.Y;
    }

    /// <summary>
    ///     Constructs a new <see cref="Ellipse" /> from the position and radius of
    ///     a <see cref="Circle" /> (since circles are special cases of ellipses).
    /// </summary>
    /// <param name="circle"> The circle to take the values of  </param>
    public Ellipse( Circle circle )
    {
        X      = circle.X;
        Y      = circle.Y;
        Width  = circle.Radius * 2f;
        Height = circle.Radius * 2f;
    }

    public float X      { get; set; }
    public float Y      { get; set; }
    public float Width  { get; set; }
    public float Height { get; set; }

    /// <summary>
    ///     Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" />
    ///     with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">
    ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.
    /// </param>
    /// <param name="context">
    ///     The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />)
    ///     for this serialization.
    /// </param>
    /// <exception cref="T:System.Security.SecurityException">
    ///     The caller does not have the required permission.
    /// </exception>
    public void GetObjectData( SerializationInfo info, StreamingContext context )
    {
    }

    /// <summary>
    ///     Checks whether or not this ellipse contains the given point.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y">
    ///     Y coordinate
    /// </param>
    /// <returns> true if this ellipse contains the given point; false otherwise.  </returns>
    public bool Contains( float x, float y )
    {
        x -= X;
        y -= Y;

        return ( ( ( x * x ) / ( Width * 0.5f * Width * 0.5f ) )
               + ( ( y * y ) / ( Height * 0.5f * Height * 0.5f ) ) )
            <= 1.0f;
    }

    /// <summary>
    ///     Checks whether or not this ellipse contains the given point.
    /// </summary>
    /// <param name="point">
    ///     Position vector
    /// </param>
    /// <returns> true if this ellipse contains the given point; false otherwise.  </returns>
    public bool Contains( Vector2 point ) => Contains( point.X, point.Y );

    /// <summary>
    ///     Sets a new position and size for this ellipse.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="width"> the width of the ellipse </param>
    /// <param name="height"> the height of the ellipse  </param>
    public void Set( float x, float y, float width, float height )
    {
        X      = x;
        Y      = y;
        Width  = width;
        Height = height;
    }

    /// <summary>
    ///     Sets a new position and size for this ellipse based upon another ellipse.
    /// </summary>
    /// <param name="ellipse"> The ellipse to copy the position and size of. </param>
    public void Set( Ellipse ellipse )
    {
        X      = ellipse.X;
        Y      = ellipse.Y;
        Width  = ellipse.Width;
        Height = ellipse.Height;
    }

    public void Set( Circle circle )
    {
        X      = circle.X;
        Y      = circle.Y;
        Width  = circle.Radius * 2f;
        Height = circle.Radius * 2f;
    }

    public void Set( Vector2 position, Vector2 size )
    {
        X      = position.X;
        Y      = position.Y;
        Width  = size.X;
        Height = size.Y;
    }

    /// <summary>
    ///     Sets the x and y-coordinates of ellipse center from a <see cref="Vector2" />.
    /// </summary>
    /// <param name="position"> The position vector </param>
    /// <returns> this ellipse for chaining  </returns>
    public Ellipse SetPosition( Vector2 position )
    {
        X = position.X;
        Y = position.Y;

        return this;
    }

    /// <summary>
    ///     Sets the x and y-coordinates of ellipse center
    /// </summary>
    /// <param name="x"> The x-coordinate </param>
    /// <param name="y"> The y-coordinate </param>
    /// <returns> this ellipse for chaining  </returns>
    public Ellipse SetPosition( float x, float y )
    {
        X = x;
        Y = y;

        return this;
    }

    /// <summary>
    ///     Sets the width and height of this ellipse
    /// </summary>
    /// <param name="width"> The width </param>
    /// <param name="height"> The height </param>
    /// <returns> this ellipse for chaining  </returns>
    public Ellipse SetSize( float width, float height )
    {
        Width  = width;
        Height = height;

        return this;
    }

    /// <returns>
    ///     The area of this <see cref="Ellipse" /> as <see cref="MathUtils.PI" />
    ///     <see cref="Ellipse.Width" /> * <see cref="Ellipse.Height" />
    /// </returns>
    public float Area() => ( MathUtils.PI * ( Width * Height ) ) / 4;

    /// <summary>
    ///     Approximates the circumference of this <see cref="Ellipse" />. Oddly enough,
    ///     the circumference of an ellipse is actually difficult to compute exactly.
    /// </summary>
    /// <returns>
    ///     The Ramanujan approximation to the circumference of an ellipse if one dimension
    ///     is at least three times longer than the other, else the simpler approximation
    /// </returns>
    public float Circumference()
    {
        var a = Width / 2;
        var b = Height / 2;

        if ( ( ( a * 3 ) > b ) || ( ( b * 3 ) > a ) )
        {
            // If one dimension is three times as long as the other...
            return ( float )( MathUtils.PI
                            * ( ( 3 * ( a + b ) )
                              - Math.Sqrt( ( ( 3 * a ) + b ) * ( a + ( 3 * b ) ) ) ) );
        }

        // We can use the simpler approximation, then
        return ( float )( MathUtils.PI2 * Math.Sqrt( ( ( a * a ) + ( b * b ) ) / 2 ) );
    }

    public new bool Equals( object? o )
    {
        if ( o == this )
        {
            return true;
        }

        if ( ( o == null ) || ( o.GetType() != GetType() ) )
        {
            return false;
        }

        var e = ( Ellipse )o;

        return X.Equals( e.X )
            && Y.Equals( e.Y )
            && Width.Equals( e.Width )
            && Height.Equals( e.Height );
    }

    public override int GetHashCode()
    {
        const int PRIME = 53;

        var result = PRIME + NumberUtils.FloatToRawIntBits( Height );
        result = ( PRIME * result ) + NumberUtils.FloatToRawIntBits( Width );
        result = ( PRIME * result ) + NumberUtils.FloatToRawIntBits( X );
        result = ( PRIME * result ) + NumberUtils.FloatToRawIntBits( Y );

        return result;
    }
}
