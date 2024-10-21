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
/// Encapsulates a 2D polygon defined by it's vertices relative to an origin point (default of 0, 0).
/// </summary>
[PublicAPI]
public class Polygon : IShape2D
{
    // ------------------------------------------------------------------------

    private RectangleShape? _bounds;
    private bool            _dirty = true;
    private float[]?        _localVertices;
    private float[]?        _worldVertices;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Constructs a new polygon with no vertices.
    /// </summary>
    public Polygon()
    {
        _localVertices = [ ];
    }

    /// <summary>
    /// Constructs a new polygon from a float array of parts of vertex points.
    /// </summary>
    /// <param name="vertices">
    /// an array where every even element represents the horizontal part of a point,
    /// and the following element representing the vertical part
    /// </param>
    /// <exception cref="ArgumentException">
    /// if less than 6 elements, representing 3 points, are provided
    /// </exception>
    public Polygon( float[]? vertices )
    {
        if ( vertices?.Length < 6 )
        {
            throw new ArgumentException( "polygons must contain at least 3 points." );
        }

        _localVertices = vertices;
    }

    public float X        { get; set; }
    public float Y        { get; set; }
    public float OriginX  { get; set; }
    public float OriginY  { get; set; }
    public float Rotation { get; set; }
    public float ScaleX   { get; set; } = 1;
    public float ScaleY   { get; set; } = 1;

    /// <summary>
    /// Calculates and returns the vertices of the polygon after scaling, rotation,
    /// and positional translations have been applied, as they are position within
    /// the world.
    /// </summary>
    /// <returns> vertices scaled, rotated, and offset by the polygon position.</returns>
    public float[]? TransformedVertices
    {
        get
        {
            if ( !_dirty )
            {
                return _worldVertices;
            }

            _dirty = false;

            if ( ( _worldVertices == null ) || ( _worldVertices.Length != _localVertices?.Length ) )
            {
                _worldVertices = new float[ _localVertices!.Length ];
            }

            var scale = ScaleX is not 1 || ScaleY is not 1;
            var cos   = MathUtils.CosDeg( Rotation );
            var sin   = MathUtils.SinDeg( Rotation );

            for ( int i = 0, n = _localVertices.Length; i < n; i += 2 )
            {
                var x = _localVertices[ i ] - OriginX;
                var y = _localVertices[ i + 1 ] - OriginY;

                // scale if needed
                if ( scale )
                {
                    x *= ScaleX;
                    y *= ScaleY;
                }

                // rotate if needed
                if ( Rotation != 0 )
                {
                    var oldX = x;

                    x = ( cos * x ) - ( sin * y );
                    y = ( sin * oldX ) + ( cos * y );
                }

                _worldVertices[ i ]     = X + x + OriginX;
                _worldVertices[ i + 1 ] = Y + y + OriginY;
            }

            return _worldVertices;
        }
    }

    /// <summary>
    /// 1. Returns the polygon's local vertices without scaling or rotation and
    /// without being offset by the polygon position.
    /// <para>
    /// 2. Sets the polygon's local vertices relative to the origin point, without
    /// any scaling, rotating or translations being applied.
    /// </para>
    /// </summary>
    /// <param name="value">
    /// float array where every even element represents the x-coordinate of a
    /// vertex, and the proceeding element representing the y-coordinate.
    /// </param>
    /// <exception cref="ArgumentException">
    /// if less than 6 elements, representing 3 points, are provided
    /// </exception>
    public float[]? Vertices
    {
        get => _localVertices;
        set
        {
            if ( value?.Length < 6 )
            {
                throw new ArgumentException( "polygons must contain at least 3 points." );
            }

            _localVertices = value;
            _dirty         = true;
        }
    }

    /// <summary>
    /// Sets the polygon to be rotated by the supplied degrees.
    /// </summary>
    public float SetRotation
    {
        set
        {
            Rotation = value;
            _dirty   = true;
        }
    }

    /// <summary>
    /// Sets the polygon's world vertices to be recalculated when getting
    /// the <see cref="TransformedVertices"/> Property.
    /// </summary>

    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    public bool Dirty
    {
        get => _dirty;
        set => _dirty = value;
    }

    /// <summary>
    /// Returns an axis-aligned bounding box of this polygon.
    /// Note the returned RectangleShape is cached in this polygon, and will
    /// be reused if this Polygon is changed.
    /// </summary>
    /// <returns> this polygon's bounding box <see cref="RectangleShape"/>  </returns>
    public RectangleShape BoundingRectangle
    {
        get
        {
            var vertices = TransformedVertices;

            var minX = vertices![ 0 ];
            var minY = vertices[ 1 ];
            var maxX = vertices[ 0 ];
            var maxY = vertices[ 1 ];

            var numFloats = vertices.Length;

            for ( var i = 2; i < numFloats; i += 2 )
            {
                minX = minX > vertices[ i ] ? vertices[ i ] : minX;
                minY = minY > vertices[ i + 1 ] ? vertices[ i + 1 ] : minY;
                maxX = maxX < vertices[ i ] ? vertices[ i ] : maxX;
                maxY = maxY < vertices[ i + 1 ] ? vertices[ i + 1 ] : maxY;
            }

            _bounds ??= new RectangleShape();

            _bounds.X      = minX;
            _bounds.Y      = minY;
            _bounds.Width  = maxX - minX;
            _bounds.Height = maxY - minY;

            return _bounds;
        }
    }

    /// <summary>
    /// Returns whether an x, y pair is contained within the polygon.
    /// </summary>
    public bool Contains( float x, float y )
    {
        var numFloats  = TransformedVertices!.Length;
        var intersects = 0;

        for ( var i = 0; i < numFloats; i += 2 )
        {
            var x1 = TransformedVertices[ i ];
            var y1 = TransformedVertices[ i + 1 ];
            var x2 = TransformedVertices[ ( i + 2 ) % numFloats ];
            var y2 = TransformedVertices[ ( i + 3 ) % numFloats ];

            if ( ( ( ( y1 <= y ) && ( y < y2 ) ) || ( ( y2 <= y ) && ( y < y1 ) ) )
              && ( x < ( ( ( ( x2 - x1 ) / ( y2 - y1 ) ) * ( y - y1 ) ) + x1 ) ) )
            {
                intersects++;
            }
        }

        return ( intersects & 1 ) == 1;
    }

    /// <summary>
    /// Returns whether an x, y pair from the supplied Vector2 is
    /// contained within the polygon.
    /// </summary>
    public bool Contains( Vector2 point )
    {
        return Contains( point.X, point.Y );
    }

    /// <summary>
    /// Sets the origin point to which all of the polygon's local vertices are relative to.
    /// </summary>
    public void SetOrigin( float originX, float originY )
    {
        OriginX = originX;
        OriginY = originY;
        _dirty  = true;
    }

    /// <summary>
    /// Sets the polygon's position within the world.
    /// </summary>
    public void SetPosition( float x, float y )
    {
        X      = x;
        Y      = y;
        _dirty = true;
    }

    /// <summary>
    /// Translates the polygon's position by the specified horizontal and vertical amounts.
    /// </summary>
    public void Translate( float x, float y )
    {
        X      += x;
        Y      += y;
        _dirty =  true;
    }

    /// <summary>
    /// Applies additional rotation to the polygon by the supplied degrees.
    /// </summary>
    public void Rotate( float degrees )
    {
        Rotation += degrees;
        _dirty   =  true;
    }

    /// <summary>
    /// Sets the amount of scaling to be applied to the polygon.
    /// </summary>
    public void SetScale( float scaleX, float scaleY )
    {
        ScaleX = scaleX;
        ScaleY = scaleY;
        _dirty = true;
    }

    /// <summary>
    /// Applies additional scaling to the polygon by the supplied amount.
    /// </summary>
    public void Scale( float amount )
    {
        ScaleX += amount;
        ScaleY += amount;
        _dirty =  true;
    }

    /// <summary>
    /// Returns the area contained within the polygon.
    /// </summary>
    public float Area()
    {
        return GeometryUtils.PolygonArea( TransformedVertices!, 0, TransformedVertices!.Length );
    }
}
