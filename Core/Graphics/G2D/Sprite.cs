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

using LibGDXSharp.Maths;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Sprite : TextureRegion
{
    public readonly static int VertexSize = 2 + 1 + 2;
    public readonly static int SpriteSize = 4 * VertexSize;

    #region PrivateData

    private readonly Color _color = new(1, 1, 1, 1);

    private float           _x;
    private float           _y;
    private float           _width;
    private float           _height;
    private float           _originX;
    private float           _originY;
    private float           _rotation;
    private float           _scaleX = 1;
    private float           _scaleY = 1;
    private bool            _dirty  = true;
    private RectangleShape? _bounds;

    #endregion

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
        this.Texture = texture ?? throw new System.ArgumentException( "texture cannot be null." );

        SetRegion( srcX, srcY, srcWidth, srcHeight );
        SetColor( 1, 1, 1, 1 );
        SetSize( Math.Abs( srcWidth ), Math.Abs( srcHeight ) );
        SetOrigin( _width / 2, _height / 2 );
    }

    // Note the region is copied.
    /// <summary>
    /// Creates a sprite based on a specific TextureRegion.
    /// The new sprite's region is a copy of the parameter region - altering one
    /// does not affect the other 
    /// </summary>
    public Sprite( TextureRegion region )
    {
        SetRegion( region );
        SetColor( 1, 1, 1, 1 );
        SetSize( region.RegionWidth, region.RegionHeight );
        SetOrigin( _width / 2, _height / 2 );
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
        SetSize( Math.Abs( srcWidth ), Math.Abs( srcHeight ) );
        SetOrigin( _width / 2, _height / 2 );
    }

    /// <summary>
    /// Creates a sprite that is a copy in every way of the specified sprite.
    /// </summary>
    public Sprite( Sprite sprite )
    {
        Set( sprite );
    }

    #endregion

    /// <summary>
    /// Make this sprite a copy in every way of the specified sprite
    /// </summary>
    public void Set( Sprite sprite )
    {
        if ( sprite == null )
        {
            throw new System.ArgumentException( "sprite cannot be null." );
        }

        Array.Copy
            (
             sprite.Vertices,
             0,
             Vertices,
             0,
             SpriteSize
            );

        Texture      = sprite.Texture;
        U            = sprite.U;
        V            = sprite.V;
        U2           = sprite.U2;
        V2           = sprite.V2;
        _x           = sprite._x;
        _y           = sprite._y;
        _width       = sprite._width;
        _height      = sprite._height;
        RegionWidth  = sprite.RegionWidth;
        RegionHeight = sprite.RegionHeight;
        _originX     = sprite._originX;
        _originY     = sprite._originY;
        _rotation    = sprite._rotation;
        _scaleX      = sprite._scaleX;
        _scaleY      = sprite._scaleY;
        _dirty       = sprite._dirty;

        _color.Set( sprite._color );
    }

    /// <summary>
    /// Sets the position and size of the sprite when drawn, before scaling
    /// and rotation are applied. If origin, rotation, or scale are changed,
    /// it is slightly more efficient to set the bounds after those operations. 
    /// </summary>
    public virtual void SetBounds( float x, float y, float width, float height )
    {
        this._x      = x;
        this._y      = y;
        this._width  = width;
        this._height = height;

        if ( this._dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ( _scaleX is not 1 ) || ( _scaleY is not 1 ) )
        {
            this._dirty = true;

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
        this._width  = width;
        this._height = height;

        if ( this._dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ( _scaleX is not 1 ) || ( _scaleY is not 1 ) )
        {
            this._dirty = true;

            return;
        }

        var x2 = this._x + width;
        var y2 = this._y + height;

        Vertices[ IBatch.X1 ] = this._x;
        Vertices[ IBatch.Y1 ] = this._y;

        Vertices[ IBatch.X2 ] = this._x;
        Vertices[ IBatch.Y2 ] = y2;

        Vertices[ IBatch.X3 ] = x2;
        Vertices[ IBatch.Y3 ] = y2;

        Vertices[ IBatch.X4 ] = x2;
        Vertices[ IBatch.Y4 ] = this._y;
    }

    /// <summary>
    /// Sets the position where the sprite will be drawn. If origin, rotation, or scale are changed, it is slightly more efficient
    /// to set the position after those operations. If both position and size are to be changed, it is better to use
    /// <see cref="SetBounds(float, float, float, float)"/>. 
    /// </summary>
    public void SetPosition( float x, float y )
    {
        this._x = x;
        this._y = y;

        if ( this._dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ( _scaleX is not 1 ) || ( _scaleY is not 1 ) )
        {
            this._dirty = true;

            return;
        }

        var x2 = x + _width;
        var y2 = y + _height;

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
        SetPosition( x - this._originX, y - this._originY );
    }

    /// <summary>
    /// Sets the position so that the sprite is centered on (x, y)
    /// </summary>
    public void SetCenter( float x, float y )
    {
        SetPosition( x - ( Width / 2 ), y - ( Height / 2 ) );
    }

    /// <summary>
    /// Sets the x position relative to the current position where the sprite will
    /// be drawn. If origin, rotation, or scale are
    /// changed, it is slightly more efficient to translate after those operations. 
    /// </summary>
    public void TranslateX( float xAmount )
    {
        this.X += xAmount;

        if ( this._dirty )
        {
            return;
        }

        if ( ( this._rotation != 0 ) || ( this._scaleX is not 1 ) || ( this._scaleY is not 1 ) )
        {
            this._dirty = true;

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

        if ( this._dirty )
        {
            return;
        }

        if ( ( this._rotation != 0 ) || ( this._scaleX is not 1 ) || ( this._scaleY is not 1 ) )
        {
            this._dirty = true;

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

        if ( this._dirty )
        {
            return;
        }

        if ( ( this._rotation != 0 ) || ( this._scaleX is not 1 ) || ( this._scaleY is not 1 ) )
        {
            this._dirty = true;

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

        var color = tint.ToFloatBits();

        Vertices[ IBatch.C1 ] = color;
        Vertices[ IBatch.C2 ] = color;
        Vertices[ IBatch.C3 ] = color;
        Vertices[ IBatch.C4 ] = color;
    }

    public void SetColor( float r, float g, float b, float a )
    {
        Color.Set( r, g, b, a );

        var color = this.Color.ToFloatBits();

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
        this._originX = originX;
        this._originY = originY;
        this._dirty   = true;
    }

    /// <summary>
    /// Place origin in the center of the sprite
    /// </summary>
    public virtual void SetOriginCenter()
    {
        this._originX = Width / 2;
        this._originY = Height / 2;
        this._dirty   = true;
    }

    /// <summary>
    /// Sets the sprite's rotation in degrees relative to the current rotation.
    /// Rotation is centered on the origin set in <see cref="SetOrigin(float, float)"/> 
    /// </summary>
    public void Rotate( float degrees )
    {
        if ( degrees == 0 ) return;

        _rotation   += degrees;
        this._dirty =  true;
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
        this._scaleX = scaleXY;
        this._scaleY = scaleXY;
        this._dirty  = true;
    }

    /// <summary>
    /// Sets the sprite's scale for both X and Y. The sprite scales out from
    /// the origin. This will not affect the values returned by
    /// <see cref="Width"/> and <see cref="Height"/> 
    /// </summary>
    public void SetScale( float scaleX, float scaleY )
    {
        this._scaleX = scaleX;
        this._scaleY = scaleY;
        this._dirty  = true;
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
        this._scaleX += amount;
        this._scaleY += amount;
        this._dirty  =  true;
    }

    /// <summary>
    /// Returns the packed vertices, colors, and texture coordinates
    /// for this sprite.
    /// </summary>
    public float[] GetVertices()
    {
        if ( this._dirty )
        {
            this._dirty = false;

            var localX = -this._originX;
            var localY = -this._originY;

            var localX2      = localX + _width;
            var localY2      = localY + _height;
            var worldOriginX = this.X - localX;
            var worldOriginY = this.Y - localY;

            if ( ( this._scaleX is not 1 ) || ( this._scaleY is not 1 ) )
            {
                localX  *= this._scaleX;
                localY  *= this._scaleY;
                localX2 *= this._scaleX;
                localY2 *= this._scaleY;
            }

            if ( this._rotation != 0 )
            {
                var cos = MathUtils.CosDeg( this._rotation );
                var sin = MathUtils.SinDeg( this._rotation );

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
        batch.Draw( Texture, Vertices, 0, SpriteSize );
    }

    public void Draw( IBatch batch, float alphaModulation )
    {
        var oldAlpha = Alpha;

        Alpha = ( oldAlpha * alphaModulation );

        Draw( batch );

        Alpha = oldAlpha;
    }

    /// <summary>
    /// 
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

    // ----------------------------------------------------

    // The following four Setters are methods so as to
    // not interfere with inherited properties...

    public void SetU( float value )
    {
        base.U = value;

        Vertices[ IBatch.U1 ] = value;
        Vertices[ IBatch.U2 ] = value;
    }

    public void SetV( float value )
    {
        base.V = value;

        Vertices[ IBatch.V2 ] = value;
        Vertices[ IBatch.V3 ] = value;
    }

    public void SetU2( float value )
    {
        base.U2 = value;

        Vertices[ IBatch.U3 ] = value;
        Vertices[ IBatch.U4 ] = value;
    }

    public void SetV2( float value )
    {
        base.V2               = value;
        Vertices[ IBatch.V1 ] = value;
        Vertices[ IBatch.V4 ] = value;
    }

    // ----------------------------------------------------

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
    public virtual new void Flip( bool flipx, bool flipy )
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
    /// 
    /// </summary>
    /// <param name="xAmount"></param>
    /// <param name="yAmount"></param>
    public override void Scroll( float xAmount, float yAmount )
    {
        if ( xAmount != 0 )
        {
            var u  = ( Vertices[ IBatch.U1 ] + xAmount ) % 1;
            var u2 = u + ( _width / Texture.Width );

            this.U  = u;
            this.U2 = u2;

            Vertices[ IBatch.U1 ] = u;
            Vertices[ IBatch.U2 ] = u;
            Vertices[ IBatch.U3 ] = u2;
            Vertices[ IBatch.U4 ] = u2;
        }

        if ( yAmount != 0 )
        {
            var v  = ( Vertices[ IBatch.V2 ] + yAmount ) % 1;
            var v2 = v + ( _height / Texture.Height );

            this.V  = v;
            this.V2 = v2;

            Vertices[ IBatch.V1 ] = v2;
            Vertices[ IBatch.V2 ] = v;
            Vertices[ IBatch.V3 ] = v;
            Vertices[ IBatch.V4 ] = v2;
        }
    }

    #region Properties

    /// <summary>
    /// Sets the color of this sprite, expanding the alpha from 0-254 to 0-255.
    /// </summary>
    /// <see cref="Color.ToFloatBits()"/>
    public float PackedColor
    {
        set
        {
            Color.Abgr8888ToColor( this.Color, value );

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
    /// <returns> the bounding Rectangle  </returns>
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
        get => this.Color.A;
        set
        {
            _color.A = value;

            var color = this.Color.ToFloatBits();

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
            this._rotation = value;
            this._dirty    = true;
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

            Color color = this._color;

            color.R = ( intBits & 0xff ) / 255f;
            color.G = ( ( intBits >>> 8 ) & 0xff ) / 255f;
            color.B = ( ( intBits >>> 16 ) & 0xff ) / 255f;
            color.A = ( ( intBits >>> 24 ) & 0xff ) / 255f;

            return color;
        }
    }

    public float[] Vertices { get; set; } = null!;

    /// <returns> the width of the sprite, not accounting for scale. </returns>
    public float Width => _width;

    /// <returns> the height of the sprite, not accounting for scale. </returns>
    public float Height => _height;

    /// <summary>
    /// The origin influences <see cref="SetPosition(float, float)"/>,
    /// <see cref="Rotation"/> and the expansion direction of scaling
    /// <see cref="SetScale(float, float)"/> 
    /// </summary>
    public float OriginX => _originX;

    /// <summary>
    /// The origin influences <see cref="SetPosition(float, float)"/>,
    /// <see cref="Rotation"/> and the expansion direction of scaling
    /// <see cref="SetScale(float, float)"/> 
    /// </summary>
    public float OriginY => _originY;

    /// <summary>
    /// X scale of the sprite, independent of size set
    /// by <see cref="SetSize(float, float)"/>
    /// </summary>
    public float ScaleX => this._scaleX;

    /// <summary>
    /// Y scale of the sprite, independent of size set
    /// by <see cref="SetSize(float, float)"/>
    /// </summary>
    public float ScaleY => this._scaleY;

    /// <summary>
    /// Sets the x position where the sprite will be drawn. If origin, rotation,
    /// or scale are changed, it is slightly more efficient to set the position
    /// after those operations. If both position and size are to be changed, it
    /// is better to use <see cref="SetBounds(float, float, float, float)"/>. 
    /// </summary>
    public float X
    {
        get => this._x;
        set
        {
            this._x = value;

            if ( this._dirty ) return;

            if ( ( this._rotation != 0 ) || ( this._scaleX is not 1 ) || ( this._scaleY is not 1 ) )
            {
                this._dirty = true;

                return;
            }

            var x2 = value + this._width;

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
    public float Y
    {
        get => this._y;
        set
        {
            this._y = value;

            if ( this._dirty ) return;

            if ( ( this._rotation != 0 ) || ( this._scaleX is not 1 ) || ( this._scaleY is not 1 ) )
            {
                this._dirty = true;

                return;
            }

            var y2 = value + this._height;

            Vertices[ IBatch.Y1 ] = value;
            Vertices[ IBatch.Y2 ] = y2;
            Vertices[ IBatch.Y3 ] = y2;
            Vertices[ IBatch.Y4 ] = value;
        }
    }

    /// <summary>
    /// Sets the x position so that it is centered on the given x parameter
    /// </summary>
    public void SetCenterX( float value )
    {
        this.X = value - ( Width / 2 );
    }

    /// <summary>
    /// Sets the y position so that it is centered on the given y parameter
    /// </summary>
    public void SetCenterY( float value )
    {
        this.Y = value - ( Height / 2 );
    }

    #endregion

}
