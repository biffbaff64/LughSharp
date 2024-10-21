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

using Corelib.LibCore.Maths;

namespace Corelib.LibCore.Graphics.G2D;

[PublicAPI]
public class PolygonSprite
{
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private readonly RectangleShape _bounds = new();

    private bool     _dirty;
    private float[]? _vertices;
    private float    _x;
    private float    _y;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public PolygonSprite( PolygonRegion region )
    {
        SetRegion( region );
        SetSize( region.Region.RegionWidth, region.Region.RegionHeight );
        SetOrigin( Width / 2, Height / 2 );
    }

    /// <summary>
    /// Creates a sprite that is a copy in every way of the specified sprite.
    /// </summary>
    public PolygonSprite( PolygonSprite sprite )
    {
        Set( sprite );
    }

    public PolygonRegion? Region   { get; private set; }
    public float          Width    { get; set; }
    public float          Height   { get; set; }
    public float          OriginX  { get; set; }
    public float          OriginY  { get; set; }
    public float          ScaleX   { get; set; } = 1f;
    public float          ScaleY   { get; set; } = 1f;
    public float          Rotation { get; set; }

    /// <summary>
    /// Sets the x position where the sprite will be drawn. If origin, rotation,
    /// or scale are changed, it is slightly more efficient to set the position
    /// after those operations. If both position and size are to be changed, it
    /// is better to use <see cref="SetBounds(float, float, float, float)"/>.
    /// </summary>
    public float X
    {
        get => _x;
        set => TranslateX( value );
    }

    /// <summary>
    /// Sets the y position where the sprite will be drawn. If origin, rotation,
    /// or scale are changed, it is slightly more efficient to set the position
    /// after those operations. If both position and size are to be changed, it
    /// is better to use <see cref="SetBounds(float, float, float, float)"/>.
    /// </summary>
    public float Y
    {
        get => _y;
        set => TranslateY( value );
    }

    /// <summary>
    /// Returns the color of this sprite. Modifying the returned color
    /// will have unexpected effects unless <see cref="SetColor(Corelib.LibCore.Graphics.Color)"/> or
    /// <see cref="SetColor(float, float, float, float)"/> is subsequently
    /// called before drawing this sprite.
    /// </summary>
    public Color Color { get; } = new( 1f, 1f, 1f, 1f );

    public void Set( PolygonSprite sprite )
    {
        ArgumentNullException.ThrowIfNull( sprite );

        SetRegion( sprite.Region );

        _x       = sprite._x;
        _y       = sprite._y;
        Width    = sprite.Width;
        Height   = sprite.Height;
        OriginX  = sprite.OriginX;
        OriginY  = sprite.OriginY;
        Rotation = sprite.Rotation;
        ScaleX   = sprite.ScaleX;
        ScaleY   = sprite.ScaleY;

        Color.Set( sprite.Color );
    }

    /// <summary>
    /// Sets the position and size of the sprite when drawn, before scaling
    /// and rotation are applied. If origin, rotation, or scale are changed,
    /// it is slightly more efficient to set the bounds after those operations.
    /// </summary>
    public void SetBounds( float x, float y, float width, float height )
    {
        _x     = x;
        _y     = y;
        Width  = width;
        Height = height;

        _dirty = true;
    }

    /// <summary>
    /// Sets the size of the sprite when drawn, before scaling and rotation
    /// are applied. If origin, rotation, or scale are changed, it is slightly
    /// more efficient to set the size after those operations. If both position
    /// and size are to be changed, it is better to use
    /// <see cref="SetBounds(float, float, float, float)"/>
    /// </summary>
    public void SetSize( float width, float height )
    {
        Width  = width;
        Height = height;

        _dirty = true;
    }

    /// <summary>
    /// Sets the position where the sprite will be drawn. If origin, rotation,
    /// or scale are changed, it is slightly more efficient to set the position
    /// after those operations. If both position and size are to be changed,
    /// it is better to use <see cref="SetBounds(float, float, float, float)"/>
    /// </summary>
    public void SetPosition( float x, float y )
    {
        Translate( x - _x, y - _y );
    }

    /// <summary>
    /// Sets the x position relative to the current position where the sprite
    /// will be drawn. If origin, rotation, or scale are changed, it is slightly
    /// more efficient to translate after those operations.
    /// </summary>
    public void TranslateX( float xAmount )
    {
        _x += xAmount;

        if ( _dirty )
        {
            return;
        }

        for ( var i = 0; i < _vertices?.Length; i += Sprite.VERTEX_SIZE )
        {
            _vertices[ i ] += xAmount;
        }
    }

    /// <summary>
    /// Sets the y position relative to the current position where the sprite
    /// will be drawn. If origin, rotation, or scale are changed, it is slightly
    /// more efficient to translate after those operations.
    /// </summary>
    public void TranslateY( float yAmount )
    {
        _y += yAmount;

        if ( _dirty )
        {
            return;
        }

        for ( var i = 1; i < _vertices?.Length; i += Sprite.VERTEX_SIZE )
        {
            _vertices[ i ] += yAmount;
        }
    }

    /// <summary>
    /// Sets the position relative to the current position where the sprite
    /// will be drawn. If origin, rotation, or scale are changed, it is
    /// slightly more efficient to translate after those operations.
    /// </summary>
    public void Translate( float xAmount, float yAmount )
    {
        _x += xAmount;
        _y += yAmount;

        if ( _dirty )
        {
            return;
        }

        for ( var i = 0; i < _vertices?.Length; i += Sprite.VERTEX_SIZE )
        {
            _vertices[ i ]     += xAmount;
            _vertices[ i + 1 ] += yAmount;
        }
    }

    public void SetColor( Color tint )
    {
        Color.Set( tint );

        var color = tint.ToFloatBitsABGR();

        for ( var i = 2; i < _vertices?.Length; i += Sprite.VERTEX_SIZE )
        {
            _vertices[ i ] = color;
        }
    }

    public void SetColor( float r, float g, float b, float a )
    {
        Color.Set( r, g, b, a );

        for ( var i = 2; i < _vertices?.Length; i += Sprite.VERTEX_SIZE )
        {
            _vertices[ i ] = Color.ToFloatBitsABGR();
        }
    }

    /// <summary>
    /// Sets the origin in relation to the sprite's position for scaling and rotation.
    /// </summary>
    public void SetOrigin( float originX, float originY )
    {
        OriginX = originX;
        OriginY = originY;
        _dirty  = true;
    }

    public void SetRotation( float degrees )
    {
        Rotation = degrees;
        _dirty   = true;
    }

    /// <summary>
    /// Sets the sprite's rotation relative to the current rotation.
    /// </summary>
    public void Rotate( float degrees )
    {
        Rotation += degrees;
        _dirty   =  true;
    }

    public void SetScale( float scaleXY )
    {
        ScaleX = scaleXY;
        ScaleY = scaleXY;
        _dirty = true;
    }

    public void SetScale( float scaleX, float scaleY )
    {
        ScaleX = scaleX;
        ScaleY = scaleY;
        _dirty = true;
    }

    /// <summary>
    /// Sets the sprite's scale relative to the current scale.
    /// </summary>
    public void Scale( float amount )
    {
        ScaleX += amount;
        ScaleY += amount;
        _dirty =  true;
    }

    /// <summary>
    /// Returns the packed vertices, colors, and texture coordinates for this sprite.
    /// </summary>
    public float[]? GetVertices()
    {
        if ( !_dirty || ( _vertices == null ) || ( Region == null ) )
        {
            return _vertices;
        }

        _dirty = false;

        var originX  = OriginX;
        var originY  = OriginY;
        var scaleX   = ScaleX;
        var scaleY   = ScaleY;
        var vertices = _vertices;

        var regionVertices = Region.Vertices;

        var region = Region;

        var worldOriginX = _x + originX;
        var worldOriginY = _y + originY;
        var sX           = Width / region.Region.RegionWidth;
        var sY           = Height / region.Region.RegionHeight;
        var cos          = MathUtils.CosDeg( Rotation );
        var sin          = MathUtils.SinDeg( Rotation );

        for ( int i = 0, v = 0, n = regionVertices!.Length; i < n; i += 2, v += 5 )
        {
            var fx = ( ( regionVertices[ i ] * sX ) - originX ) * scaleX;
            var fy = ( ( regionVertices[ i + 1 ] * sY ) - originY ) * scaleY;
            vertices[ v ]     = ( ( cos * fx ) - ( sin * fy ) ) + worldOriginX;
            vertices[ v + 1 ] = ( sin * fx ) + ( cos * fy ) + worldOriginY;
        }

        return vertices;
    }

    /// <summary>
    /// Returns the bounding axis aligned <see cref="RectangleShape"/> that bounds
    /// this sprite. The rectangles x and y coordinates describe its bottom left
    /// corner. If you change the position or size of the sprite, you have to fetch
    /// the triangle again for it to be recomputed.
    /// </summary>
    /// <returns> the bounding Rectangle </returns>
    public RectangleShape GetBoundingRectangle()
    {
        var vertices = GetVertices();

        if ( vertices == null )
        {
            throw new NullReferenceException();
        }

        var minx = vertices[ 0 ];
        var miny = vertices[ 1 ];
        var maxx = vertices[ 0 ];
        var maxy = vertices[ 1 ];

        for ( var i = 5; i < vertices.Length; i += 5 )
        {
            var x = vertices[ i ];
            var y = vertices[ i + 1 ];

            minx = minx > x ? x : minx;
            maxx = maxx < x ? x : maxx;
            miny = miny > y ? y : miny;
            maxy = maxy < y ? y : maxy;
        }

        _bounds.X      = minx;
        _bounds.Y      = miny;
        _bounds.Width  = maxx - minx;
        _bounds.Height = maxy - miny;

        return _bounds;
    }

    public void Draw( PolygonSpriteBatch spriteBatch )
    {
        if ( Region == null )
        {
            return;
        }

        spriteBatch.Draw(
                         Region.Region.Texture,
                         GetVertices()!,
                         0,
                         _vertices!.Length,
                         Region.Triangles,
                         0,
                         Region.Triangles.Length
                        );
    }

    public void Draw( PolygonSpriteBatch spriteBatch, float alphaModulation )
    {
        var color    = Color;
        var oldAlpha = color.A;

        color.A *= alphaModulation;
        SetColor( color );

        Draw( spriteBatch );

        color.A = oldAlpha;
        SetColor( color );
    }

    /// <summary>
    /// Returns the actual color used in the vertices of this sprite. Modifying the
    /// returned color will have unexpected effects unless <see cref="SetColor(Corelib.LibCore.Graphics.Color)"/>
    /// or <see cref="SetColor(float, float, float, float)"/> is subsequently called
    /// before drawing this sprite.
    /// </summary>
    public Color GetPackedColor()
    {
        var color = Color;

        if ( _vertices != null )
        {
            Color.ABGR8888ToColor( ref color, _vertices[ 2 ] );
        }

        return color;
    }

    public void SetRegion( PolygonRegion? region )
    {
        if ( region == null )
        {
            return;
        }

        Region = region;

        var regionVertices = region.Vertices;
        var textureCoords  = region.TextureCoords;

        var verticesLength = ( regionVertices!.Length / 2 ) * 5;

        if ( ( _vertices == null ) || ( _vertices.Length != verticesLength ) )
        {
            _vertices = new float[ verticesLength ];
        }

        // Set the color and UVs in this sprite's vertices.
        for ( int i = 0, v = 2; v < verticesLength; i += 2, v += 5 )
        {
            _vertices[ v ]     = Color.ToFloatBitsABGR();
            _vertices[ v + 1 ] = textureCoords[ i ];
            _vertices[ v + 2 ] = textureCoords[ i + 1 ];
        }

        _dirty = true;
    }
}
