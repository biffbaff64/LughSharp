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
public class Sprite : TextureRegion
{
    public const int VERTEX_SIZE = 2 + 1 + 2;
    public const int SPRITE_SIZE = 4 * VERTEX_SIZE;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region PrivateData

    private readonly Color _color = new( 1, 1, 1, 1 );

    private RectangleShape? _bounds;
    private float           _x;
    private float           _y;
    private float           _rotation;
    private bool            _dirty = true;

    #endregion

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region Constructors

    /// <summary>
    /// Creates an uninitialized sprite.
    /// <para>
    /// The sprite will need a texture region and bounds set before it can be drawn.
    /// </para>
    /// </summary>
    public Sprite()
    {
        SetColor( 1, 1, 1, 1 );
    }

    /// <summary>
    /// Creates a sprite with width, height, and texture region
    /// equal to the size of the texture.
    /// </summary>
    public Sprite( Texture texture )
        : this( texture, 0, 0, texture.Width, texture.Height )
    {
    }

    /// <summary>
    /// Creates a sprite with width, height, and texture region equal to the
    /// specified size. The texture region's upper left corner will be 0,0.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="srcWidth">
    /// The width of the texture region.
    /// May be negative to flip the sprite when drawn.
    /// </param>
    /// <param name="srcHeight">
    /// The height of the texture region.
    /// May be negative to flip the sprite when drawn.
    /// </param>
    public Sprite( Texture texture, int srcWidth, int srcHeight )
        : this( texture, 0, 0, srcWidth, srcHeight )
    {
    }

    /// <summary>
    /// Creates a sprite with width, height, and texture region equal
    /// to the specified size.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="srcX"></param>
    /// <param name="srcY"></param>
    /// <param name="srcWidth">
    /// The width of the texture region.
    /// May be negative to flip the sprite when drawn.
    /// </param>
    /// <param name="srcHeight">
    /// The height of the texture region.
    /// May be negative to flip the sprite when drawn.
    /// </param>
    public Sprite( Texture? texture, int srcX, int srcY, int srcWidth, int srcHeight )
    {
        Texture = texture ?? throw new ArgumentException( "texture cannot be null." );

        SetRegion( srcX, srcY, srcWidth, srcHeight );
        SetColor( 1, 1, 1, 1 );

        SetSizeAndOrigin( Math.Abs( srcWidth ), Math.Abs( srcHeight ) );
    }

    /// <summary>
    /// Creates a sprite based on a specific TextureRegion.
    /// The new sprite's region is a copy of the parameter region - altering one
    /// does not affect the other.
    /// </summary>
    public Sprite( TextureRegion region )
    {
        SetRegion( region );
        SetColor( 1, 1, 1, 1 );

        SetSizeAndOrigin( region.RegionWidth, region.RegionHeight );
    }

    /// <summary>
    /// Creates a sprite with width, height, and texture region equal to the
    /// specified size, relative to specified sprite's texture region.
    /// </summary>
    /// <param name="region"></param>
    /// <param name="srcX"></param>
    /// <param name="srcY"></param>
    /// <param name="srcWidth">
    /// The width of the texture region.
    /// May be negative to flip the sprite when drawn.
    /// </param>
    /// <param name="srcHeight">
    /// The height of the texture region.
    /// May be negative to flip the sprite when drawn.
    /// </param>
    public Sprite( TextureRegion region, int srcX, int srcY, int srcWidth, int srcHeight )
    {
        SetRegion( region, srcX, srcY, srcWidth, srcHeight );
        SetColor( 1, 1, 1, 1 );

        SetSizeAndOrigin( Math.Abs( srcWidth ), Math.Abs( srcHeight ) );
    }

    /// <summary>
    /// Creates a sprite that is a copy in every way of the specified sprite.
    /// </summary>
    public Sprite( Sprite sprite )
    {
        Set( sprite );
    }

    #endregion

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Helper method for constructors which allows calls to virtual
    /// methods which cannot be called from constructors.
    /// </summary>
    /// <param name="srcWidth"></param>
    /// <param name="srcHeight"></param>
    private void SetSizeAndOrigin( int srcWidth, int srcHeight )
    {
        SetSize( srcWidth, srcHeight );
        SetOrigin( Width / 2, Height / 2 );
    }

    /// <summary>
    /// Make this sprite a copy in every way of the specified sprite
    /// </summary>
    /// <summary>
    /// Sets the properties of this Sprite to match those of the provided Sprite.
    /// </summary>
    /// <param name="sprite">The Sprite whose properties will be copied.</param>
    /// <exception cref="ArgumentNullException">Thrown if the provided sprite is null.</exception>
    public void Set( Sprite sprite )
    {
        ArgumentNullException.ThrowIfNull( sprite, nameof( sprite ) );

        try
        {
            // Copy vertices array
            Array.Copy( sprite.Vertices, 0, Vertices, 0, SPRITE_SIZE );
        }
        catch ( ArgumentException ex )
        {
            throw new InvalidOperationException( "Failed to copy vertices array.", ex );
        }

        // Assign properties
        Texture      = sprite.Texture;
        U            = sprite.U;
        V            = sprite.V;
        U2           = sprite.U2;
        V2           = sprite.V2;
        _x           = sprite._x;
        _y           = sprite._y;
        Width        = sprite.Width;
        Height       = sprite.Height;
        RegionWidth  = sprite.RegionWidth;
        RegionHeight = sprite.RegionHeight;
        OriginX      = sprite.OriginX;
        OriginY      = sprite.OriginY;
        _rotation    = sprite._rotation;
        ScaleX       = sprite.ScaleX;
        ScaleY       = sprite.ScaleY;
        _dirty       = sprite._dirty;

        // Copy color
        _color.Set( sprite._color );
    }

    /// <summary>
    /// Sets the position and size of the sprite when drawn, before scaling
    /// and rotation are applied. If origin, rotation, or scale are changed,
    /// it is slightly more efficient to set the bounds after those operations.
    /// </summary>
    public virtual void SetBounds( float x, float y, float width, float height )
    {
        _x     = x;
        _y     = y;
        Width  = width;
        Height = height;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ScaleX is not 1 || ScaleY is not 1 )
        {
            _dirty = true;

            return;
        }

        var x2 = x + width;
        var y2 = y + height;

        Vertices[ IBatch.X1 ] = x;
        Vertices[ IBatch.Y1 ] = y;

        Vertices[ IBatch.X2 ] = x;
        Vertices[ IBatch.Y2 ] = y2;

        Vertices[ IBatch.X3 ] = x2;
        Vertices[ IBatch.Y3 ] = y2;

        Vertices[ IBatch.X4 ] = x2;
        Vertices[ IBatch.Y4 ] = y;
    }

    /// <summary>
    /// Sets the size of the sprite when drawn, before scaling and rotation are
    /// applied. If origin, rotation, or scale are changed, it is slightly more
    /// efficient to set the size after those operations.
    /// <para>
    /// If both position and size are to be changed, it is better to use
    /// <see cref="SetBounds(float, float, float, float)"/>.
    /// </para>
    /// </summary>
    public virtual void SetSize( float width, float height )
    {
        Width  = width;
        Height = height;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ScaleX is not 1 || ScaleY is not 1 )
        {
            _dirty = true;

            return;
        }

        var x2 = _x + width;
        var y2 = _y + height;

        Vertices[ IBatch.X1 ] = _x;
        Vertices[ IBatch.Y1 ] = _y;

        Vertices[ IBatch.X2 ] = _x;
        Vertices[ IBatch.Y2 ] = y2;

        Vertices[ IBatch.X3 ] = x2;
        Vertices[ IBatch.Y3 ] = y2;

        Vertices[ IBatch.X4 ] = x2;
        Vertices[ IBatch.Y4 ] = _y;
    }

    /// <summary>
    /// Sets the position where the sprite will be drawn. If origin, rotation, or scale are changed, it is slightly more
    /// efficient
    /// to set the position after those operations. If both position and size are to be changed, it is better to use
    /// <see cref="SetBounds(float, float, float, float)"/>.
    /// </summary>
    public void SetPosition( float x, float y )
    {
        _x = x;
        _y = y;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ScaleX is not 1 || ScaleY is not 1 )
        {
            _dirty = true;

            return;
        }

        var x2 = x + Width;
        var y2 = y + Height;

        Vertices[ IBatch.X1 ] = x;
        Vertices[ IBatch.Y1 ] = y;

        Vertices[ IBatch.X2 ] = x;
        Vertices[ IBatch.Y2 ] = y2;

        Vertices[ IBatch.X3 ] = x2;
        Vertices[ IBatch.Y3 ] = y2;

        Vertices[ IBatch.X4 ] = x2;
        Vertices[ IBatch.Y4 ] = y;
    }

    /// <summary>
    /// Sets the position where the sprite will be drawn, relative to its current origin.
    /// </summary>
    public void SetOriginBasedPosition( float x, float y )
    {
        SetPosition( x - OriginX, y - OriginY );
    }

    /// <summary>
    /// Sets the position so that the sprite is centered on (x, y)
    /// </summary>
    public void SetCenter( float x, float y )
    {
        SetPosition( x - ( Width / 2 ), y - ( Height / 2 ) );
    }

    /// <summary>
    /// Sets the x position so that it is centered on the given x parameter
    /// </summary>
    public void SetCenterX( float value )
    {
        X = value - ( Width / 2 );
    }

    /// <summary>
    /// Sets the y position so that it is centered on the given y parameter
    /// </summary>
    public void SetCenterY( float value )
    {
        Y = value - ( Height / 2 );
    }

    /// <summary>
    /// Sets the x position relative to the current position where the sprite will
    /// be drawn. If origin, rotation, or scale are
    /// changed, it is slightly more efficient to translate after those operations.
    /// </summary>
    public void TranslateX( float xAmount )
    {
        X += xAmount;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ScaleX is not 1 || ScaleY is not 1 )
        {
            _dirty = true;

            return;
        }

        Vertices[ IBatch.X1 ] += xAmount;
        Vertices[ IBatch.X2 ] += xAmount;
        Vertices[ IBatch.X3 ] += xAmount;
        Vertices[ IBatch.X4 ] += xAmount;
    }

    /// <summary>
    /// Sets the y position relative to the current position where the sprite will
    /// be drawn. If origin, rotation, or scale are changed, it is slightly more
    /// efficient to translate after those operations.
    /// </summary>
    public void TranslateY( float yAmount )
    {
        Y += yAmount;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ScaleX is not 1 || ScaleY is not 1 )
        {
            _dirty = true;

            return;
        }

        Vertices[ IBatch.Y1 ] += yAmount;
        Vertices[ IBatch.Y2 ] += yAmount;
        Vertices[ IBatch.Y3 ] += yAmount;
        Vertices[ IBatch.Y4 ] += yAmount;
    }

    /// <summary>
    /// Sets the position relative to the current position where the sprite will
    /// be drawn. If origin, rotation, or scale are changed, it is slightly more
    /// efficient to translate after those operations.
    /// </summary>
    public void Translate( float xAmount, float yAmount )
    {
        X += xAmount;
        Y += yAmount;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ScaleX is not 1 || ScaleY is not 1 )
        {
            _dirty = true;

            return;
        }

        Vertices[ IBatch.X1 ] += xAmount;
        Vertices[ IBatch.Y1 ] += yAmount;

        Vertices[ IBatch.X2 ] += xAmount;
        Vertices[ IBatch.Y2 ] += yAmount;

        Vertices[ IBatch.X3 ] += xAmount;
        Vertices[ IBatch.Y3 ] += yAmount;

        Vertices[ IBatch.X4 ] += xAmount;
        Vertices[ IBatch.Y4 ] += yAmount;
    }

    /// <summary>
    /// Sets the color used to tint this sprite.
    /// Default is <see cref="Graphics.Color.White"/>.
    /// </summary>
    public void SetColor( Color tint )
    {
        Color.Set( tint );

        var color = tint.ToFloatBitsABGR();

        Vertices[ IBatch.C1 ] = color;
        Vertices[ IBatch.C2 ] = color;
        Vertices[ IBatch.C3 ] = color;
        Vertices[ IBatch.C4 ] = color;
    }

    /// <summary>
    /// Sets the color used to tint this sprite.
    /// </summary>
    public void SetColor( float r, float g, float b, float a )
    {
        Color.Set( r, g, b, a );

        var color = Color.ToFloatBitsABGR();

        Vertices[ IBatch.C1 ] = color;
        Vertices[ IBatch.C2 ] = color;
        Vertices[ IBatch.C3 ] = color;
        Vertices[ IBatch.C4 ] = color;
    }

    /// <summary>
    /// Sets the origin in relation to the sprite's position for scaling and rotation.
    /// </summary>
    public virtual void SetOrigin( float originX, float originY )
    {
        OriginX = originX;
        OriginY = originY;
        _dirty  = true;
    }

    /// <summary>
    /// Place origin in the center of the sprite
    /// </summary>
    public virtual void SetOriginCenter()
    {
        OriginX = Width / 2;
        OriginY = Height / 2;
        _dirty  = true;
    }

    /// <summary>
    /// Sets the sprite's rotation in degrees relative to the current rotation.
    /// Rotation is centered on the origin set in <see cref="SetOrigin(float, float)"/>
    /// </summary>
    public void Rotate( float degrees )
    {
        if ( degrees == 0 )
        {
            return;
        }

        _rotation += degrees;
        _dirty    =  true;
    }

    /// <summary>
    /// Rotates this sprite 90 degrees in-place by rotating the texture coordinates.
    /// This rotation is unaffected by <see cref="Rotation"/> and
    /// <see cref="Rotate(float)"/>.
    /// </summary>
    public virtual void Rotate90( bool clockwise )
    {
        if ( clockwise )
        {
            var temp = Vertices[ IBatch.V1 ];

            Vertices[ IBatch.V1 ] = Vertices[ IBatch.V4 ];
            Vertices[ IBatch.V4 ] = Vertices[ IBatch.V3 ];
            Vertices[ IBatch.V3 ] = Vertices[ IBatch.V2 ];
            Vertices[ IBatch.V2 ] = temp;

            temp = Vertices[ IBatch.U1 ];

            Vertices[ IBatch.U1 ] = Vertices[ IBatch.U4 ];
            Vertices[ IBatch.U4 ] = Vertices[ IBatch.U3 ];
            Vertices[ IBatch.U3 ] = Vertices[ IBatch.U2 ];
            Vertices[ IBatch.U2 ] = temp;
        }
        else
        {
            var temp = Vertices[ IBatch.V1 ];

            Vertices[ IBatch.V1 ] = Vertices[ IBatch.V2 ];
            Vertices[ IBatch.V2 ] = Vertices[ IBatch.V3 ];
            Vertices[ IBatch.V3 ] = Vertices[ IBatch.V4 ];
            Vertices[ IBatch.V4 ] = temp;

            temp = Vertices[ IBatch.U1 ];

            Vertices[ IBatch.U1 ] = Vertices[ IBatch.U2 ];
            Vertices[ IBatch.U2 ] = Vertices[ IBatch.U3 ];
            Vertices[ IBatch.U3 ] = Vertices[ IBatch.U4 ];
            Vertices[ IBatch.U4 ] = temp;
        }
    }

    /// <summary>
    /// Sets the sprite's scale for both X and Y uniformly. The sprite
    /// scales out from the origin. This will not affect the values
    /// returned by <see cref="Width"/> and <see cref="Height"/>
    /// </summary>
    public void SetScale( float scaleXY )
    {
        ScaleX = scaleXY;
        ScaleY = scaleXY;
        _dirty = true;
    }

    /// <summary>
    /// Sets the sprite's scale for both X and Y. The sprite scales out from
    /// the origin. This will not affect the values returned by
    /// <see cref="Width"/> and <see cref="Height"/>
    /// </summary>
    public void SetScale( float scaleX, float scaleY )
    {
        ScaleX = scaleX;
        ScaleY = scaleY;
        _dirty = true;
    }

    /// <summary>
    /// Sets the sprite's scale relative to the current scale. for example:
    /// original scale 2 -> sprite.scale(4) -> final scale 6.
    /// <para>
    /// The sprite scales out from the origin. This will not affect the values
    /// returned by <see cref="Width"/> and <see cref="Height"/>
    /// </para>
    /// </summary>
    public void AddScale( float amount )
    {
        ScaleX += amount;
        ScaleY += amount;
        _dirty =  true;
    }

    /// <summary>
    /// Returns the packed vertices, colors, and texture coordinates
    /// for this sprite.
    /// </summary>
    public float[] GetVertices()
    {
        if ( _dirty )
        {
            _dirty = false;

            var localX = -OriginX;
            var localY = -OriginY;

            var localX2      = localX + Width;
            var localY2      = localY + Height;
            var worldOriginX = X - localX;
            var worldOriginY = Y - localY;

            if ( ScaleX is not 1 || ScaleY is not 1 )
            {
                localX  *= ScaleX;
                localY  *= ScaleY;
                localX2 *= ScaleX;
                localY2 *= ScaleY;
            }

            if ( _rotation != 0 )
            {
                var cos = MathUtils.CosDeg( _rotation );
                var sin = MathUtils.SinDeg( _rotation );

                var localXCos  = localX * cos;
                var localXSin  = localX * sin;
                var localYCos  = localY * cos;
                var localYSin  = localY * sin;
                var localX2Cos = localX2 * cos;
                var localX2Sin = localX2 * sin;
                var localY2Cos = localY2 * cos;
                var localY2Sin = localY2 * sin;

                var x1 = ( localXCos - localYSin ) + worldOriginX;
                var y1 = localYCos + localXSin + worldOriginY;

                Vertices[ IBatch.X1 ] = x1;
                Vertices[ IBatch.Y1 ] = y1;

                var x2 = ( localXCos - localY2Sin ) + worldOriginX;
                var y2 = localY2Cos + localXSin + worldOriginY;

                Vertices[ IBatch.X2 ] = x2;
                Vertices[ IBatch.Y2 ] = y2;

                var x3 = ( localX2Cos - localY2Sin ) + worldOriginX;
                var y3 = localY2Cos + localX2Sin + worldOriginY;

                Vertices[ IBatch.X3 ] = x3;
                Vertices[ IBatch.Y3 ] = y3;

                Vertices[ IBatch.X4 ] = x1 + ( x3 - x2 );
                Vertices[ IBatch.Y4 ] = y3 - ( y2 - y1 );
            }
            else
            {
                var x1 = localX + worldOriginX;
                var y1 = localY + worldOriginY;
                var x2 = localX2 + worldOriginX;
                var y2 = localY2 + worldOriginY;

                Vertices[ IBatch.X1 ] = x1;
                Vertices[ IBatch.Y1 ] = y1;

                Vertices[ IBatch.X2 ] = x1;
                Vertices[ IBatch.Y2 ] = y2;

                Vertices[ IBatch.X3 ] = x2;
                Vertices[ IBatch.Y3 ] = y2;

                Vertices[ IBatch.X4 ] = x2;
                Vertices[ IBatch.Y4 ] = y1;
            }
        }

        return Vertices;
    }

    public void Draw( IBatch batch )
    {
        batch.Draw( Texture, Vertices, 0, SPRITE_SIZE );
    }

    public void Draw( IBatch batch, float alphaModulation )
    {
        var oldAlpha = Alpha;

        Alpha = oldAlpha * alphaModulation;

        Draw( batch );

        Alpha = oldAlpha;
    }

    /// <summary>
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <param name="u2"></param>
    /// <param name="v2"></param>
    public override void SetRegion( float u, float v, float u2, float v2 )
    {
        base.SetRegion( u, v, u2, v2 );

        Vertices[ IBatch.U1 ] = u;
        Vertices[ IBatch.V1 ] = v2;

        Vertices[ IBatch.U2 ] = u;
        Vertices[ IBatch.V2 ] = v;

        Vertices[ IBatch.U3 ] = u2;
        Vertices[ IBatch.V3 ] = v;

        Vertices[ IBatch.U4 ] = u2;
        Vertices[ IBatch.V4 ] = v2;
    }

    /// <summary>
    /// Set the sprite's flip state regardless of current condition.
    /// </summary>
    /// <param name="flipx"> the desired horizontal flip state </param>
    /// <param name="flipy"> the desired vertical flip state  </param>
    public void SetFlip( bool flipx, bool flipy )
    {
        var performX = false;
        var performY = false;

        if ( IsFlipX() != flipx )
        {
            performX = true;
        }

        if ( IsFlipY() != flipy )
        {
            performY = true;
        }

        Flip( performX, performY );
    }

    /// <summary>
    /// </summary>
    /// <param name="flipx"> perform horizontal flip </param>
    /// <param name="flipy"> perform vertical flip  </param>
    public override void Flip( bool flipx, bool flipy )
    {
        base.Flip( flipx, flipy );

        if ( flipx )
        {
            var temp = Vertices[ IBatch.U1 ];

            Vertices[ IBatch.U1 ] = Vertices[ IBatch.U3 ];
            Vertices[ IBatch.U3 ] = temp;

            temp = Vertices[ IBatch.U2 ];

            Vertices[ IBatch.U2 ] = Vertices[ IBatch.U4 ];
            Vertices[ IBatch.U4 ] = temp;
        }

        if ( flipy )
        {
            var temp = Vertices[ IBatch.V1 ];

            Vertices[ IBatch.V1 ] = Vertices[ IBatch.V3 ];
            Vertices[ IBatch.V3 ] = temp;

            temp = Vertices[ IBatch.V2 ];

            Vertices[ IBatch.V2 ] = Vertices[ IBatch.V4 ];
            Vertices[ IBatch.V4 ] = temp;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="xAmount"></param>
    /// <param name="yAmount"></param>
    public override void Scroll( float xAmount, float yAmount )
    {
        if ( xAmount != 0 )
        {
            var u  = ( Vertices[ IBatch.U1 ] + xAmount ) % 1;
            var u2 = u + ( Width / Texture.Width );

            U  = u;
            U2 = u2;

            Vertices[ IBatch.U1 ] = u;
            Vertices[ IBatch.U2 ] = u;
            Vertices[ IBatch.U3 ] = u2;
            Vertices[ IBatch.U4 ] = u2;
        }

        if ( yAmount != 0 )
        {
            var v  = ( Vertices[ IBatch.V2 ] + yAmount ) % 1;
            var v2 = v + ( Height / Texture.Height );

            V  = v;
            V2 = v2;

            Vertices[ IBatch.V1 ] = v2;
            Vertices[ IBatch.V2 ] = v;
            Vertices[ IBatch.V3 ] = v;
            Vertices[ IBatch.V4 ] = v2;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region Properties

    public override float U
    {
        get => base.U;
        set
        {
            base.U = value;

            Vertices[ IBatch.U1 ] = value;
            Vertices[ IBatch.U2 ] = value;
        }
    }

    public override float V
    {
        get => base.V;
        set
        {
            base.V = value;

            Vertices[ IBatch.V2 ] = value;
            Vertices[ IBatch.V3 ] = value;
        }
    }

    public override float U2
    {
        get => base.U2;
        set
        {
            base.U2 = value;

            Vertices[ IBatch.U3 ] = value;
            Vertices[ IBatch.U4 ] = value;
        }
    }

    public override float V2
    {
        get => base.V2;
        set
        {
            base.V2 = value;

            Vertices[ IBatch.V1 ] = value;
            Vertices[ IBatch.V4 ] = value;
        }
    }

    /// <summary>
    /// Sets the color of this sprite, expanding the alpha from 0-254 to 0-255.
    /// </summary>
    /// <see cref="Graphics.Color.ToFloatBitsABGR()"/>
    public float PackedColor
    {
        set
        {
            var color = Color;

            Color.ABGR8888ToColor( ref color, value );

            Vertices[ IBatch.C1 ] = value;
            Vertices[ IBatch.C2 ] = value;
            Vertices[ IBatch.C3 ] = value;
            Vertices[ IBatch.C4 ] = value;
        }
    }

    /// <summary>
    /// Returns the bounding axis aligned <see cref="RectangleShape"/> that
    /// bounds this sprite. The rectangles x and y coordinates describe its
    /// bottom left corner. If you change the position or size of the sprite,
    /// you must fetch the triangle again for it to be recomputed.
    /// </summary>
    public RectangleShape BoundingRectangle
    {
        get
        {
            var minx = Vertices[ IBatch.X1 ];
            var miny = Vertices[ IBatch.Y1 ];
            var maxx = Vertices[ IBatch.X1 ];
            var maxy = Vertices[ IBatch.Y1 ];

            minx = minx > Vertices[ IBatch.X2 ] ? Vertices[ IBatch.X2 ] : minx;
            minx = minx > Vertices[ IBatch.X3 ] ? Vertices[ IBatch.X3 ] : minx;
            minx = minx > Vertices[ IBatch.X4 ] ? Vertices[ IBatch.X4 ] : minx;

            maxx = maxx < Vertices[ IBatch.X2 ] ? Vertices[ IBatch.X2 ] : maxx;
            maxx = maxx < Vertices[ IBatch.X3 ] ? Vertices[ IBatch.X3 ] : maxx;
            maxx = maxx < Vertices[ IBatch.X4 ] ? Vertices[ IBatch.X4 ] : maxx;

            miny = miny > Vertices[ IBatch.Y2 ] ? Vertices[ IBatch.Y2 ] : miny;
            miny = miny > Vertices[ IBatch.Y3 ] ? Vertices[ IBatch.Y3 ] : miny;
            miny = miny > Vertices[ IBatch.Y4 ] ? Vertices[ IBatch.Y4 ] : miny;

            maxy = maxy < Vertices[ IBatch.Y2 ] ? Vertices[ IBatch.Y2 ] : maxy;
            maxy = maxy < Vertices[ IBatch.Y3 ] ? Vertices[ IBatch.Y3 ] : maxy;
            maxy = maxy < Vertices[ IBatch.Y4 ] ? Vertices[ IBatch.Y4 ] : maxy;

            _bounds ??= new RectangleShape();

            _bounds.X      = minx;
            _bounds.Y      = miny;
            _bounds.Width  = maxx - minx;
            _bounds.Height = maxy - miny;

            return _bounds;
        }
    }

    /// <summary>
    /// Sets the alpha portion of the color used to tint this sprite.
    /// </summary>
    public float Alpha
    {
        get => Color.A;
        set
        {
            _color.A = value;

            var color = Color.ToFloatBitsABGR();

            Vertices[ IBatch.C1 ] = color;
            Vertices[ IBatch.C2 ] = color;
            Vertices[ IBatch.C3 ] = color;
            Vertices[ IBatch.C4 ] = color;
        }
    }

    /// <summary>
    /// Sets the rotation of the sprite in degrees. Rotation is centered on the
    /// origin set in <see cref="SetOrigin(float, float)"/>
    /// </summary>
    public float Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            _dirty    = true;
        }
    }

    /// <summary>
    /// Returns the color of this sprite. If the returned instance is
    /// manipulated, <see cref="SetColor(float,float,float,float)"/> must be
    /// called afterward.
    /// </summary>
    public Color Color
    {
        get
        {
            var intBits = NumberUtils.FloatToIntColor( Vertices[ IBatch.C1 ] );

            var color = _color;

            color.R = ( intBits & 0xff ) / 255f;
            color.G = ( ( intBits >>> 8 ) & 0xff ) / 255f;
            color.B = ( ( intBits >>> 16 ) & 0xff ) / 255f;
            color.A = ( ( intBits >>> 24 ) & 0xff ) / 255f;

            return color;
        }
    }

    public float[] Vertices { get; set; } = null!;

    /// <returns> the width of the sprite, not accounting for scale. </returns>
    public float Width { get; set; }

    /// <returns> the height of the sprite, not accounting for scale. </returns>
    public float Height { get; set; }

    /// <summary>
    /// The origin influences <see cref="SetPosition(float, float)"/>,
    /// <see cref="Rotation"/> and the expansion direction of scaling
    /// <see cref="SetScale(float, float)"/>
    /// </summary>
    public float OriginX { get; set; }

    /// <summary>
    /// The origin influences <see cref="SetPosition(float, float)"/>,
    /// <see cref="Rotation"/> and the expansion direction of scaling
    /// <see cref="SetScale(float, float)"/>
    /// </summary>
    public float OriginY { get; private set; }

    /// <summary>
    /// X scale of the sprite, independent of size set
    /// by <see cref="SetSize(float, float)"/>
    /// </summary>
    public float ScaleX { get; private set; } = 1;

    /// <summary>
    /// Y scale of the sprite, independent of size set
    /// by <see cref="SetSize(float, float)"/>
    /// </summary>
    public float ScaleY { get; private set; } = 1;

    /// <summary>
    /// Sets the x position where the sprite will be drawn. If origin, rotation,
    /// or scale are changed, it is slightly more efficient to set the position
    /// after those operations. If both position and size are to be changed, it
    /// is better to use <see cref="SetBounds(float, float, float, float)"/>.
    /// </summary>
    public virtual float X
    {
        get => _x;
        set
        {
            _x = value;

            if ( _dirty )
            {
                return;
            }

            if ( ( _rotation != 0 ) || ScaleX is not 1 || ScaleY is not 1 )
            {
                _dirty = true;

                return;
            }

            var x2 = value + Width;

            Vertices[ IBatch.X1 ] = value;
            Vertices[ IBatch.X2 ] = value;
            Vertices[ IBatch.X3 ] = x2;
            Vertices[ IBatch.X4 ] = x2;
        }
    }

    /// <summary>
    /// Sets the y position where the sprite will be drawn. If origin, rotation,
    /// or scale are changed, it is slightly more efficient to set the position
    /// after those operations. If both position and size are to be changed, it
    /// is better to use <see cref="SetBounds(float, float, float, float)"/>.
    /// </summary>
    public virtual float Y
    {
        get => _y;
        set
        {
            _y = value;

            if ( _dirty )
            {
                return;
            }

            if ( ( _rotation != 0 ) || ScaleX is not 1 || ScaleY is not 1 )
            {
                _dirty = true;

                return;
            }

            var y2 = value + Height;

            Vertices[ IBatch.Y1 ] = value;
            Vertices[ IBatch.Y2 ] = y2;
            Vertices[ IBatch.Y3 ] = y2;
            Vertices[ IBatch.Y4 ] = value;
        }
    }

    #endregion properties
}