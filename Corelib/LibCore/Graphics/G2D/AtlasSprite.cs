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


namespace Corelib.LibCore.Graphics.G2D;

/// <summary>
/// A sprite that, if whitespace was stripped from the region when it was packed,
/// is automatically positioned as if whitespace had not been stripped. */
/// </summary>
[PublicAPI]
public class AtlasSprite : Sprite
{
    public AtlasRegion Region          { get; set; }
    public float       OriginalOffsetX { get; set; }
    public float       OriginalOffsetY { get; set; }

    // ========================================================================
    // ========================================================================
    
    /// <summary>
    /// Creates a new AtlasSprite using the atlas region from the
    /// supplied AtlasSprite.
    /// </summary>
    public AtlasSprite( AtlasSprite sprite )
    {
        Region          = sprite.Region;
        OriginalOffsetX = sprite.OriginalOffsetX;
        OriginalOffsetY = sprite.OriginalOffsetY;

        Set( sprite );
    }

    /// <summary>
    /// Creates a new AtlasSprite using the supplied atlas region.
    /// </summary>
    public AtlasSprite( AtlasRegion region )
    {
        Region = new AtlasRegion( region );

        Init( region );
    }

    /// <summary>
    /// Setup method designed to be called from <tt>AtlasSprite(AtlasRegion)</tt>
    /// to get around the issue of calling virtual methods from a constructor.
    /// </summary>
    private void Init( AtlasRegion region )
    {
        OriginalOffsetX = region.OffsetX;
        OriginalOffsetY = region.OffsetY;

        SetRegion( region );

        SetOrigin( region.OriginalWidth / 2f, region.OriginalHeight / 2f );

        var width  = region.RegionWidth;
        var height = region.RegionHeight;

        if ( region.Rotate )
        {
            base.Rotate90( true );
            base.SetBounds( region.OffsetX, region.OffsetY, height, width );
        }
        else
        {
            base.SetBounds( region.OffsetX, region.OffsetY, width, height );
        }

        SetColor( 1, 1, 1, 1 );
    }

    /// <inheritdoc />
    public override float X
    {
        get => base.X - Region.OffsetX;
        set => base.X = value + Region.OffsetX;
    }

    /// <inheritdoc />
    public override float Y
    {
        get => base.Y - Region.OffsetY;
        set => base.Y = value + Region.OffsetY;
    }

    /// <inheritdoc />
    public override void SetBounds( float x, float y, float width, float height )
    {
        var widthRatio  = width / Region.OriginalWidth;
        var heightRatio = height / Region.OriginalHeight;

        Region.OffsetX = OriginalOffsetX * widthRatio;
        Region.OffsetY = OriginalOffsetY * heightRatio;

        var packedWidth  = Region.Rotate ? Region.PackedHeight : Region.PackedWidth;
        var packedHeight = Region.Rotate ? Region.PackedWidth : Region.PackedHeight;

        base.SetBounds(
                       x + Region.OffsetX,
                       y + Region.OffsetY,
                       packedWidth * widthRatio,
                       packedHeight * heightRatio
                      );
    }

    /// <inheritdoc />
    public override void SetSize( float width, float height )
    {
        SetBounds( X, Y, width, height );
    }

    /// <inheritdoc />
    public override void SetOrigin( float originX, float originY )
    {
        base.SetOrigin( originX - Region.OffsetX, originY - Region.OffsetY );
    }

    /// <inheritdoc />
    public override void SetOriginCenter()
    {
        base.SetOrigin( ( Width / 2 ) - Region.OffsetX, ( Height / 2 ) - Region.OffsetY );
    }

    /// <inheritdoc />
    public override void Flip( bool x, bool y )
    {
        // Flip texture.
        if ( Region.Rotate )
        {
            base.Flip( y, x );
        }
        else
        {
            base.Flip( x, y );
        }

        var oldOriginX = GetOriginX();
        var oldOriginY = GetOriginY();
        var oldOffsetX = Region.OffsetX;
        var oldOffsetY = Region.OffsetY;

        var widthRatio  = GetWidthRatio();
        var heightRatio = GetHeightRatio();

        Region.OffsetX = OriginalOffsetX;
        Region.OffsetY = OriginalOffsetY;

        Region.Flip( x, y ); // Updates x and y offsets.

        OriginalOffsetX =  Region.OffsetX;
        OriginalOffsetY =  Region.OffsetY;
        Region.OffsetX  *= widthRatio;
        Region.OffsetY  *= heightRatio;

        // Update position and origin with new offsets.
        Translate( Region.OffsetX - oldOffsetX, Region.OffsetY - oldOffsetY );
        SetOrigin( oldOriginX, oldOriginY );
    }

    /// <inheritdoc />
    public override void Rotate90( bool clockwise )
    {
        // Rotate texture.
        base.Rotate90( clockwise );

        var oldOriginX = GetOriginX();
        var oldOriginY = GetOriginY();
        var oldOffsetX = Region.OffsetX;
        var oldOffsetY = Region.OffsetY;

        var widthRatio  = GetWidthRatio();
        var heightRatio = GetHeightRatio();

        if ( clockwise )
        {
            Region.OffsetX = oldOffsetY;

            Region.OffsetY = ( Region.OriginalHeight * heightRatio )
                           - oldOffsetX
                           - ( Region.PackedWidth * widthRatio );
        }
        else
        {
            Region.OffsetY = oldOffsetX;

            Region.OffsetX = ( Region.OriginalWidth * widthRatio )
                           - oldOffsetY
                           - ( Region.PackedHeight * heightRatio );
        }

        // Update position and origin with new offsets.
        Translate( Region.OffsetX - oldOffsetX, Region.OffsetY - oldOffsetY );
        SetOrigin( oldOriginX, oldOriginY );
    }

    public float GetOriginX()
    {
        return OriginX + Region.OffsetX;
    }

    public float GetOriginY()
    {
        return OriginY + Region.OffsetY;
    }

    public float GetWidth()
    {
        return ( Width / Region.RotatedPackedWidth ) * Region.OriginalWidth;
    }

    public float GetHeight()
    {
        return ( Height / Region.RotatedPackedHeight ) * Region.OriginalHeight;
    }

    public float GetWidthRatio()
    {
        return Width / Region.RotatedPackedWidth;
    }

    public float GetHeightRatio()
    {
        return Height / Region.RotatedPackedHeight;
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        return Region.ToString();
    }
}
