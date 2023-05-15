using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Maths;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Sprite : TextureRegion
{
    public readonly static int VertexSize = 2 + 1 + 2;
    public readonly static int SpriteSize = 4 * VertexSize;

    public float[] Vertices { get; set; }

    private Color          _color = new(1, 1, 1, 1);
    private float          _x;
    private float          _y;
    private float          _width;
    private float          _height;
    private float          _originX;
    private float          _originY;
    private float          _rotation;
    private float          _scaleX = 1;
    private float          _scaleY = 1;
    private bool           _dirty  = true;
    private RectangleShape _bounds;

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
    public void SetBounds( float x, float y, float width, float height )
    {
        this._x      = x;
        this._y      = y;
        this._width  = width;
        this._height = height;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ( _scaleX is not 1 ) || ( _scaleY is not 1 ) )
        {
            _dirty = true;

            return;
        }

        var   x2       = x + width;
        var   y2       = y + height;
        
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
    public void SetSize( float width, float height )
    {
        this._width  = width;
        this._height = height;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ( _scaleX is not 1 ) || ( _scaleY is not 1 ) )
        {
            _dirty = true;

            return;
        }

        var   x2       = _x + width;
        var   y2       = _y + height;
        Vertices[ X1 ] = x;
        Vertices[ Y1 ] = y;

        Vertices[ X2 ] = x;
        Vertices[ Y2 ] = y2;

        Vertices[ X3 ] = x2;
        Vertices[ Y3 ] = y2;

        Vertices[ X4 ] = x2;
        Vertices[ Y4 ] = y;
    }

    /// <summary>
    /// Sets the position where the sprite will be drawn. If origin, rotation, or scale are changed, it is slightly more efficient
    /// to set the position after those operations. If both position and size are to be changed, it is better to use
    /// <see cref="setBounds(float, float, float, float)"/>. 
    /// </summary>
    public void SetPosition( float x, float y )
    {
        this._x = x;
        this._y = y;

        if ( _dirty )
        {
            return;
        }

        if ( ( _rotation != 0 ) || ( _scaleX != 1 ) || ( _scaleY != 1 ) )
        {
            _dirty = true;

            return;
        }

        var x2 = x + _width;
        var y2 = y + _height;

        Vertices[ X1 ] = x;
        Vertices[ Y1 ] = y;

        Vertices[ X2 ] = x;
        Vertices[ Y2 ] = y2;

        Vertices[ X3 ] = x2;
        Vertices[ Y3 ] = y2;

        Vertices[ X4 ] = x2;
        Vertices[ Y4 ] = y;
    }

    /// <summary>
    /// Sets the position where the sprite will be drawn, relative to its current origin.
    /// </summary>
    public void SetOriginBasedPosition( float x, float y )
    {
        SetPosition( x - this._originX, y - this._originY );
    }

    /// <summary>
    /// Sets the x position where the sprite will be drawn. If origin, rotation, or scale are changed, it is slightly more efficient
    /// to set the position after those operations. If both position and size are to be changed, it is better to use
    /// <see cref="setBounds(float, float, float, float)"/>. 
    /// </summary>
    public float X
    {
        set
        {
            this.x = value;

            if ( _dirty )
            {
                return;
            }

            if ( ( rotation != 0 ) || ( scaleX != 1 ) || ( scaleY != 1 ) )
            {
                _dirty = true;

                return;
            }

            var   x2       = value + width;
            Vertices[ X1 ] = value;
            Vertices[ X2 ] = value;
            Vertices[ X3 ] = x2;
            Vertices[ X4 ] = x2;
        }
    }

    /// <summary>
    /// Sets the y position where the sprite will be drawn. If origin, rotation, or scale are changed, it is slightly more efficient
    /// to set the position after those operations. If both position and size are to be changed, it is better to use
    /// <see cref="setBounds(float, float, float, float)"/>. 
    /// </summary>
    public float Y
    {
        set
        {
            this.y = value;

            if ( _dirty )
            {
                return;
            }

            if ( ( rotation != 0 ) || ( scaleX != 1 ) || ( scaleY != 1 ) )
            {
                _dirty = true;

                return;
            }

            var   y2       = value + height;
            Vertices[ Y1 ] = value;
            Vertices[ Y2 ] = y2;
            Vertices[ Y3 ] = y2;
            Vertices[ Y4 ] = value;
        }
    }

    /// <summary>
    /// Sets the x position so that it is centered on the given x parameter </summary>
    public float CenterX
    {
        set { X = value - ( width / 2 ); }
    }

    /// <summary>
    /// Sets the y position so that it is centered on the given y parameter </summary>
    public float CenterY
    {
        set { Y = value - ( height / 2 ); }
    }

    /// <summary>
    /// Sets the position so that the sprite is centered on (x, y) </summary>
    public void SetCenter( float x, float y )
    {
        setPosition( x - ( width / 2 ), y - ( height / 2 ) );
    }

    /// <summary>
    /// Sets the x position relative to the current position where the sprite will be drawn. If origin, rotation, or scale are
    /// changed, it is slightly more efficient to translate after those operations. 
    /// </summary>
    public void TranslateX( float xAmount )
    {
        this.x += xAmount;

        if ( _dirty )
        {
            return;
        }

        if ( ( rotation != 0 ) || ( scaleX != 1 ) || ( scaleY != 1 ) )
        {
            _dirty = true;

            return;
        }

        Vertices[ X1 ] += xAmount;
        Vertices[ X2 ] += xAmount;
        Vertices[ X3 ] += xAmount;
        Vertices[ X4 ] += xAmount;
    }

    /// <summary>
    /// Sets the y position relative to the current position where the sprite will be drawn. If origin, rotation, or scale are
    /// changed, it is slightly more efficient to translate after those operations. 
    /// </summary>
    public void TranslateY( float yAmount )
    {
        y += yAmount;

        if ( _dirty )
        {
            return;
        }

        if ( ( rotation != 0 ) || ( scaleX != 1 ) || ( scaleY != 1 ) )
        {
            _dirty = true;

            return;
        }

        Vertices[ Y1 ] += yAmount;
        Vertices[ Y2 ] += yAmount;
        Vertices[ Y3 ] += yAmount;
        Vertices[ Y4 ] += yAmount;
    }

    /// <summary>
    /// Sets the position relative to the current position where the sprite will be drawn. If origin, rotation, or scale are
    /// changed, it is slightly more efficient to translate after those operations. 
    /// </summary>
    public void Translate( float xAmount, float yAmount )
    {
        x += xAmount;
        y += yAmount;

        if ( _dirty )
        {
            return;
        }

        if ( ( rotation != 0 ) || ( scaleX != 1 ) || ( scaleY != 1 ) )
        {
            _dirty = true;

            return;
        }

        Vertices[ X1 ] += xAmount;
        Vertices[ Y1 ] += yAmount;

        Vertices[ X2 ] += xAmount;
        Vertices[ Y2 ] += yAmount;

        Vertices[ X3 ] += xAmount;
        Vertices[ Y3 ] += yAmount;

        Vertices[ X4 ] += xAmount;
        Vertices[ Y4 ] += yAmount;
    }

    /// <summary>
    /// Sets the color used to tint this sprite. Default is <see cref="Color.WHITE"/>. </summary>
    public void setColor( Color tint )
    {
        color.set( tint );
        float   color    = tint.toFloatBits();
        Vertices[ C1 ] = color;
        Vertices[ C2 ] = color;
        Vertices[ C3 ] = color;
        Vertices[ C4 ] = color;
    }

    /// <summary>
    /// Sets the alpha portion of the color used to tint this sprite. </summary>
    public float Alpha
    {
        set
        {
            color.a = value;
            float color = this.color.toFloatBits();
            Vertices[ C1 ] = color;
            Vertices[ C2 ] = color;
            Vertices[ C3 ] = color;
            Vertices[ C4 ] = color;
        }
    }

    /// <see cref=".setColor(Color) "/>
    public void SetColor( float r, float g, float b, float a )
    {
        color.set( r, g, b, a );
        float   color    = this.color.toFloatBits();
        Vertices[ C1 ] = color;
        Vertices[ C2 ] = color;
        Vertices[ C3 ] = color;
        Vertices[ C4 ] = color;
    }

    /// <summary>
    /// Sets the color of this sprite, expanding the alpha from 0-254 to 0-255. </summary>
    /// <see cref=".setColor(Color)"/>
    /// <see cref="Color.toFloatBits() "/>
    public float PackedColor
    {
        set
        {
            Color.abgr8888ToColor( color, value );
            Vertices[ C1 ] = value;
            Vertices[ C2 ] = value;
            Vertices[ C3 ] = value;
            Vertices[ C4 ] = value;
        }
    }

    /// <summary>
    /// Sets the origin in relation to the sprite's position for scaling and rotation. </summary>
    public void SetOrigin( float originX, float originY )
    {
        this.originX = originX;
        this.originY = originY;
        _dirty        = true;
    }

    /// <summary>
    /// Place origin in the center of the sprite </summary>
    public void SetOriginCenter()
    {
        this.originX = width / 2;
        this.originY = height / 2;
        _dirty        = true;
    }

    /// <summary>
    /// Sets the rotation of the sprite in degrees. Rotation is centered on the origin set in <see cref="setOrigin(float, float)"/> </summary>
    public float Rotation
    {
        set
        {
            this.rotation = value;
            _dirty         = true;
        }
        get { return rotation; }
    }

    /// <summary>
    /// Sets the sprite's rotation in degrees relative to the current rotation. Rotation is centered on the origin set in
    /// <see cref="setOrigin(float, float)"/> 
    /// </summary>
    public void Rotate( float degrees )
    {
        if ( degrees == 0 )
        {
            return;
        }

        rotation += degrees;
        _dirty    =  true;
    }

    /// <summary>
    /// Rotates this sprite 90 degrees in-place by rotating the texture coordinates. This rotation is unaffected by
    /// <see cref="setRotation(float)"/> and <see cref="rotate(float)"/>. 
    /// </summary>
    public void Rotate90( bool clockwise )
    {
        if ( clockwise )
        {
            var temp = Vertices[ V1 ];
            Vertices[ V1 ] = Vertices[ V4 ];
            Vertices[ V4 ] = Vertices[ V3 ];
            Vertices[ V3 ] = Vertices[ V2 ];
            Vertices[ V2 ] = temp;

            temp           = Vertices[ U1 ];
            Vertices[ U1 ] = Vertices[ U4 ];
            Vertices[ U4 ] = Vertices[ U3 ];
            Vertices[ U3 ] = Vertices[ U2 ];
            Vertices[ U2 ] = temp;
        }
        else
        {
            var temp = Vertices[ V1 ];
            Vertices[ V1 ] = Vertices[ V2 ];
            Vertices[ V2 ] = Vertices[ V3 ];
            Vertices[ V3 ] = Vertices[ V4 ];
            Vertices[ V4 ] = temp;

            temp           = Vertices[ U1 ];
            Vertices[ U1 ] = Vertices[ U2 ];
            Vertices[ U2 ] = Vertices[ U3 ];
            Vertices[ U3 ] = Vertices[ U4 ];
            Vertices[ U4 ] = temp;
        }
    }

    /// <summary>
    /// Sets the sprite's scale for both X and Y uniformly. The sprite scales out from the origin. This will not affect the values
    /// returned by <see cref="getWidth()"/> and <see cref="getHeight()"/> 
    /// </summary>
    public void setScale( float scaleXY )
    {
        this.scaleX = scaleXY;
        this.scaleY = scaleXY;
        _dirty       = true;
    }

    /// <summary>
    /// Sets the sprite's scale for both X and Y. The sprite scales out from
    /// the origin. This will not affect the values returned by
    /// <see cref="getWidth()"/> and <see cref="getHeight()"/> 
    /// </summary>
    public void SetScale( float scaleX, float scaleY )
    {
        this._scaleX = scaleX;
        this._scaleY = scaleY;
        this._dirty  = true;
    }

    /// <summary>
    /// Sets the sprite's scale relative to the current scale. for example: original scale 2 -> sprite.scale(4) -> final scale 6.
    /// The sprite scales out from the origin. This will not affect the values returned by <see cref="getWidth()"/> and
    /// <see cref="getHeight()"/> 
    /// </summary>
    public void Scale( float amount )
    {
        this._scaleX += amount;
        this._scaleY += amount;
        this._dirty  =  true;
    }

    /// <summary>
    /// Returns the packed vertices, colors, and texture coordinates for this sprite. </summary>
    public float[] GetVertices()
    {
        get
        {
            if ( _dirty )
            {
                _dirty = false;

                float[] vertices     = this.vertices;
                float   localX       = -originX;
                float   localY       = -originY;
                var   localX2      = localX + width;
                var   localY2      = localY + height;
                var   worldOriginX = this.x - localX;
                var   worldOriginY = this.y - localY;

                if ( ( scaleX != 1 ) || ( scaleY != 1 ) )
                {
                    localX  *= scaleX;
                    localY  *= scaleY;
                    localX2 *= scaleX;
                    localY2 *= scaleY;
                }

                if ( rotation != 0 )
                {
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float cos = MathUtils.cosDeg(rotation);
                    float cos = MathUtils.cosDeg( rotation );
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float sin = MathUtils.sinDeg(rotation);
                    float sin = MathUtils.sinDeg( rotation );
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float localXCos = localX * cos;
                    var localXCos = localX * cos;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float localXSin = localX * sin;
                    var localXSin = localX * sin;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float localYCos = localY * cos;
                    var localYCos = localY * cos;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float localYSin = localY * sin;
                    var localYSin = localY * sin;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float localX2Cos = localX2 * cos;
                    var localX2Cos = localX2 * cos;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float localX2Sin = localX2 * sin;
                    var localX2Sin = localX2 * sin;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float localY2Cos = localY2 * cos;
                    var localY2Cos = localY2 * cos;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float localY2Sin = localY2 * sin;
                    var localY2Sin = localY2 * sin;

                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float x1 = localXCos - localYSin + worldOriginX;
                    var x1 = ( localXCos - localYSin ) + worldOriginX;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float y1 = localYCos + localXSin + worldOriginY;
                    var y1 = localYCos + localXSin + worldOriginY;
                    vertices[ X1 ] = x1;
                    vertices[ Y1 ] = y1;

                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float x2 = localXCos - localY2Sin + worldOriginX;
                    var x2 = ( localXCos - localY2Sin ) + worldOriginX;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float y2 = localY2Cos + localXSin + worldOriginY;
                    var y2 = localY2Cos + localXSin + worldOriginY;
                    vertices[ X2 ] = x2;
                    vertices[ Y2 ] = y2;

                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float x3 = localX2Cos - localY2Sin + worldOriginX;
                    var x3 = ( localX2Cos - localY2Sin ) + worldOriginX;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float y3 = localY2Cos + localX2Sin + worldOriginY;
                    var y3 = localY2Cos + localX2Sin + worldOriginY;
                    vertices[ X3 ] = x3;
                    vertices[ Y3 ] = y3;

                    vertices[ X4 ] = x1 + ( x3 - x2 );
                    vertices[ Y4 ] = y3 - ( y2 - y1 );
                }
                else
                {
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float x1 = localX + worldOriginX;
                    var x1 = localX + worldOriginX;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float y1 = localY + worldOriginY;
                    var y1 = localY + worldOriginY;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float x2 = localX2 + worldOriginX;
                    var x2 = localX2 + worldOriginX;
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final float y2 = localY2 + worldOriginY;
                    var y2 = localY2 + worldOriginY;

                    vertices[ X1 ] = x1;
                    vertices[ Y1 ] = y1;

                    vertices[ X2 ] = x1;
                    vertices[ Y2 ] = y2;

                    vertices[ X3 ] = x2;
                    vertices[ Y3 ] = y2;

                    vertices[ X4 ] = x2;
                    vertices[ Y4 ] = y1;
                }
            }

            return vertices;
        }
    }

    /// <summary>
    /// Returns the bounding axis aligned <see cref="Rectangle"/> that bounds this sprite. The rectangles x and y coordinates describe its
    /// bottom left corner. If you change the position or size of the sprite, you have to fetch the triangle again for it to be
    /// recomputed.
    /// </summary>
    /// <returns> the bounding Rectangle  </returns>
    public RectangleShape BoundingRectangle
    {
        get
        {
            var minx = Vertices[ X1 ];
            var miny = Vertices[ Y1 ];
            var maxx = Vertices[ X1 ];
            var maxy = Vertices[ Y1 ];

            minx = minx > Vertices[ X2 ] ? Vertices[ X2 ] : minx;
            minx = minx > Vertices[ X3 ] ? Vertices[ X3 ] : minx;
            minx = minx > Vertices[ X4 ] ? Vertices[ X4 ] : minx;

            maxx = maxx < Vertices[ X2 ] ? Vertices[ X2 ] : maxx;
            maxx = maxx < Vertices[ X3 ] ? Vertices[ X3 ] : maxx;
            maxx = maxx < Vertices[ X4 ] ? Vertices[ X4 ] : maxx;

            miny = miny > Vertices[ Y2 ] ? Vertices[ Y2 ] : miny;
            miny = miny > Vertices[ Y3 ] ? Vertices[ Y3 ] : miny;
            miny = miny > Vertices[ Y4 ] ? Vertices[ Y4 ] : miny;

            maxy = maxy < Vertices[ Y2 ] ? Vertices[ Y2 ] : maxy;
            maxy = maxy < Vertices[ Y3 ] ? Vertices[ Y3 ] : maxy;
            maxy = maxy < Vertices[ Y4 ] ? Vertices[ Y4 ] : maxy;

            if ( bounds == null )
            {
                bounds = new RectangleShape();
            }

            bounds.x      = minx;
            bounds.y      = miny;
            bounds.width  = maxx - minx;
            bounds.height = maxy - miny;

            return bounds;
        }
    }

    public void Draw( IBatch batch )
    {
        batch.Draw( Texture, Vertices, 0, SpriteSize );
    }

    public void Draw( Batch batch, float alphaModulation )
    {
        float oldAlpha = Color.a;
        setAlpha( oldAlpha * alphaModulation );
        Draw( batch );
        setAlpha( oldAlpha );
    }

    public float X
    {
        get { return x; }
    }

    public float Y
    {
        get { return y; }
    }

    /// <returns> the width of the sprite, not accounting for scale. </returns>
    public float Width
    {
        get { return width; }
    }

    /// <returns> the height of the sprite, not accounting for scale. </returns>
    public float Height
    {
        get { return height; }
    }

    /// <summary>
    /// The origin influences <see cref="setPosition(float, float)"/>, <see cref="setRotation(float)"/> and the expansion direction of scaling
    /// <see cref="setScale(float, float)"/> 
    /// </summary>
    public float OriginX
    {
        get { return originX; }
    }

    /// <summary>
    /// The origin influences <see cref="setPosition(float, float)"/>, <see cref="setRotation(float)"/> and the expansion direction of scaling
    /// <see cref="setScale(float, float)"/> 
    /// </summary>
    public float OriginY
    {
        get { return originY; }
    }

    /// <summary>
    /// X scale of the sprite, independent of size set by <see cref="setSize(float, float)"/> </summary>
    public float ScaleX
    {
        get { return scaleX; }
    }

    /// <summary>
    /// Y scale of the sprite, independent of size set by <see cref="setSize(float, float)"/> </summary>
    public float ScaleY
    {
        get { return scaleY; }
    }

    /// <summary>
    /// Returns the color of this sprite. If the returned instance is manipulated, <see cref="setColor(Color)"/> must be called
    /// afterward. 
    /// </summary>
    public Color Color
    {
        get
        {
            int   intBits = NumberUtils.floatToIntColor( vertices[ C1 ] );
            Color color   = this.color;
            color.r = ( intBits & 0xff ) / 255f;
            color.g = ( ( ( int )( ( uint )intBits >> 8 ) ) & 0xff ) / 255f;
            color.b = ( ( ( int )( ( uint )intBits >> 16 ) ) & 0xff ) / 255f;
            color.a = ( ( ( int )( ( uint )intBits >> 24 ) ) & 0xff ) / 255f;

            return color;
        }
    }

    public void SetRegion( float u, float v, float u2, float v2 )
    {
        base.SetRegion( u, v, u2, v2 );

        float[] vertices = Sprite.this.vertices;
        vertices[ U1 ] = u;
        vertices[ V1 ] = v2;

        vertices[ U2 ] = u;
        vertices[ V2 ] = v;

        vertices[ U3 ] = u2;
        vertices[ V3 ] = v;

        vertices[ U4 ] = u2;
        vertices[ V4 ] = v2;
    }

    public float U
    {
        set
        {
            base.U         = value;
            vertices[ U1 ] = value;
            vertices[ U2 ] = value;
        }
    }

    public float V
    {
        set
        {
            base.V         = value;
            vertices[ V2 ] = value;
            vertices[ V3 ] = value;
        }
    }

    public float U2
    {
        set
        {
            base.U2        = value;
            vertices[ U3 ] = value;
            vertices[ U4 ] = value;
        }
    }

    public float V2
    {
        set
        {
            base.V2        = value;
            vertices[ V1 ] = value;
            vertices[ V4 ] = value;
        }
    }

    /// <summary>
    /// Set the sprite's flip state regardless of current condition </summary>
    /// <param name="x"> the desired horizontal flip state </param>
    /// <param name="y"> the desired vertical flip state  </param>
    public void SetFlip( bool x, bool y )
    {
        var performX = false;
        var performY = false;

        if ( isFlipX() != x )
        {
            performX = true;
        }

        if ( isFlipY() != y )
        {
            performY = true;
        }

        Flip( performX, performY );
    }

    /// <summary>
    /// boolean parameters x,y are not setting a state, but performing a flip </summary>
    /// <param name="x"> perform horizontal flip </param>
    /// <param name="y"> perform vertical flip  </param>
    public void Flip( bool x, bool y )
    {
        base.Flip( x, y );
        float[] vertices = Sprite.this.vertices;

        if ( x )
        {
            var temp = vertices[ U1 ];
            vertices[ U1 ] = vertices[ U3 ];
            vertices[ U3 ] = temp;
            temp           = vertices[ U2 ];
            vertices[ U2 ] = vertices[ U4 ];
            vertices[ U4 ] = temp;
        }

        if ( y )
        {
            var temp = vertices[ V1 ];
            vertices[ V1 ] = vertices[ V3 ];
            vertices[ V3 ] = temp;
            temp           = vertices[ V2 ];
            vertices[ V2 ] = vertices[ V4 ];
            vertices[ V4 ] = temp;
        }
    }

    public void Scroll( float xAmount, float yAmount )
    {
        float[] vertices = Sprite.this.vertices;

        if ( xAmount != 0 )
        {
            var u  = ( vertices[ U1 ] + xAmount ) % 1;
            var u2 = u + ( width / texture.getWidth() );
            this.u         = u;
            this.u2        = u2;
            vertices[ U1 ] = u;
            vertices[ U2 ] = u;
            vertices[ U3 ] = u2;
            vertices[ U4 ] = u2;
        }

        if ( yAmount != 0 )
        {
            var v  = ( Vertices[ V2 ] + yAmount ) % 1;
            var v2 = v + ( height / texture.getHeight() );
            this.v         = v;
            this.v2        = v2;
            Vertices[ V1 ] = v2;
            Vertices[ V2 ] = v;
            Vertices[ V3 ] = v;
            Vertices[ V4 ] = v2;
        }
    }
}