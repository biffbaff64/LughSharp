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

using System.Runtime.Serialization;

namespace LibGDXSharp.Maths;

/// <summary>
/// A convenient 2D ellipse class, based on the circle class
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class Ellipse : ISerializable, IShape2D
{
    public float X      { get; set; }
    public float Y      { get; set; }
    public float Width  { get; set; }
    public float Height { get; set; }

    /// <summary>
    /// Construct a new ellipse with all values set to zero
    /// </summary>
    public Ellipse()
    {
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    public Ellipse( Ellipse ellipse )
    {
        this.X      = ellipse.X;
        this.Y      = ellipse.Y;
        this.Width  = ellipse.Width;
        this.Height = ellipse.Height;
    }

    /// <summary>
    /// Constructs a new ellipse
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="width"> the width of the ellipse </param>
    /// <param name="height"> the height of the ellipse  </param>
    public Ellipse( float x, float y, float width, float height )
    {
        this.X      = x;
        this.Y      = y;
        this.Width  = width;
        this.Height = height;
    }

    /// <summary>
    /// Costructs a new ellipse
    /// </summary>
    /// <param name="position"> Position vector </param>
    /// <param name="width"> the width of the ellipse </param>
    /// <param name="height"> the height of the ellipse  </param>
    public Ellipse( Vector2 position, float width, float height )
    {
        this.X      = position.X;
        this.Y      = position.Y;
        this.Width  = width;
        this.Height = height;
    }

    public Ellipse( Vector2 position, Vector2 size )
    {
        this.X      = position.X;
        this.Y      = position.Y;
        this.Width  = size.X;
        this.Height = size.Y;
    }

    /// <summary>
    /// Constructs a new <see cref="Ellipse"/> from the position and radius of
    /// a <see cref="Circle"/> (since circles are special cases of ellipses).
    /// </summary>
    /// <param name="circle"> The circle to take the values of  </param>
    public Ellipse( Circle circle )
    {
        this.X      = circle.X;
        this.Y      = circle.Y;
        this.Width  = circle.Radius * 2f;
        this.Height = circle.Radius * 2f;
    }

    /// <summary>
    /// Checks whether or not this ellipse contains the given point.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate
    /// </param>
    /// <returns> true if this ellipse contains the given point; false otherwise.  </returns>
    public bool Contains( float x, float y )
    {
        x = x - this.X;
        y = y - this.Y;

        return ( ( ( x * x ) / ( Width * 0.5f * Width * 0.5f ) ) + ( ( y * y ) / ( Height * 0.5f * Height * 0.5f ) ) ) <= 1.0f;
    }

    /// <summary>
    /// Checks whether or not this ellipse contains the given point.
    /// </summary>
    /// <param name="point"> Position vector
    /// </param>
    /// <returns> true if this ellipse contains the given point; false otherwise.  </returns>
    public bool Contains( Vector2 point )
    {
        return Contains( point.X, point.Y );
    }

    /// <summary>
    /// Sets a new position and size for this ellipse.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="width"> the width of the ellipse </param>
    /// <param name="height"> the height of the ellipse  </param>
    public void Set( float x, float y, float width, float height )
    {
        this.X      = x;
        this.Y      = y;
        this.Width  = width;
        this.Height = height;
    }

    /// <summary>
    /// Sets a new position and size for this ellipse based upon another ellipse.
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
        this.X      = circle.X;
        this.Y      = circle.Y;
        this.Width  = circle.Radius * 2f;
        this.Height = circle.Radius * 2f;
    }

    public void Set( Vector2 position, Vector2 size )
    {
        this.X      = position.X;
        this.Y      = position.Y;
        this.Width  = size.X;
        this.Height = size.Y;
    }

    /// <summary>
    /// Sets the x and y-coordinates of ellipse center from a <see cref="Vector2"/>.
    /// </summary>
    /// <param name="position"> The position vector </param>
    /// <returns> this ellipse for chaining  </returns>
    public Ellipse SetPosition( Vector2 position )
    {
        this.X = position.X;
        this.Y = position.Y;

        return this;
    }

    /// <summary>
    /// Sets the x and y-coordinates of ellipse center
    /// </summary>
    /// <param name="x"> The x-coordinate </param>
    /// <param name="y"> The y-coordinate </param>
    /// <returns> this ellipse for chaining  </returns>
    public Ellipse SetPosition( float x, float y )
    {
        this.X = x;
        this.Y = y;

        return this;
    }

    /// <summary>
    /// Sets the width and height of this ellipse
    /// </summary>
    /// <param name="width"> The width </param>
    /// <param name="height"> The height </param>
    /// <returns> this ellipse for chaining  </returns>
    public Ellipse SetSize( float width, float height )
    {
        this.Width  = width;
        this.Height = height;

        return this;
    }

    /// <returns>
    /// The area of this <see cref="Ellipse"/> as <see cref="MathUtils.PI"/>
    /// * <see cref="Ellipse.Width"/> * <seealso cref="Ellipse.Height"/>
    /// </returns>
    public float Area()
    {
        return ( MathUtils.PI * ( this.Width * this.Height ) ) / 4;
    }

    /// <summary>
    /// Approximates the circumference of this <see cref="Ellipse"/>. Oddly enough,
    /// the circumference of an ellipse is actually difficult to compute exactly.
    /// </summary>
    /// <returns>
    /// The Ramanujan approximation to the circumference of an ellipse if one dimension
    /// is at least three times longer than the other, else the simpler approximation
    /// </returns>
    public float Circumference()
    {
        var a = this.Width / 2;
        var b = this.Height / 2;

        if ( ( ( a * 3 ) > b ) || ( ( b * 3 ) > a ) )
        {
            // If one dimension is three times as long as the other...
            return ( float )( MathUtils.PI * ( ( 3 * ( a + b ) ) 
                                               - Math.Sqrt( ( ( 3 * a ) + b ) * ( a + ( 3 * b ) ) ) ) );
        }
        else
        {
            // We can use the simpler approximation, then
            return ( float )( MathUtils.PI2 * Math.Sqrt( ( ( a * a ) + ( b * b ) ) / 2 ) );
        }
    }

    public new bool Equals( object? o )
    {
        if ( o == this ) return true;
        if ( ( o == null ) || ( o.GetType() != this.GetType() ) ) return false;

        var e = ( Ellipse )o;

        return ( this.X.Equals(e.X) )
               && ( this.Y.Equals(e.Y) )
               && ( this.Width.Equals(e.Width) )
               && ( this.Height.Equals(e.Height) );
    }

    public int HashCode()
    {
        var prime = 53;

        var result = prime + NumberUtils.FloatToRawIntBits( this.Height );
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( this.Width );
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( this.X );
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( this.Y );

        return result;
    }

    /// <summary>
    /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" />
    /// with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">
    /// The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.
    /// </param>
    /// <param name="context">
    /// The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />)
    /// for this serialization.
    /// </param>
    /// <exception cref="T:System.Security.SecurityException">
    /// The caller does not have the required permission.
    /// </exception>
    public void GetObjectData( SerializationInfo info, StreamingContext context )
    {
    }
}
