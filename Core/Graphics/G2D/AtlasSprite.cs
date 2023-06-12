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

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.G2D;

/// <summary>
/// A sprite that, if whitespace was stripped from the region when it was packed,
/// is automatically positioned as if whitespace had not been stripped. */
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class AtlasSprite : Sprite
{
    public AtlasRegion Region          { get; set; }
    public float       OriginalOffsetX { get; set; }
    public float       OriginalOffsetY { get; set; }

    public AtlasSprite( AtlasRegion region )
    {
        this.Region = new AtlasRegion( region );

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

    public AtlasSprite( AtlasSprite sprite )
    {
        Region               = sprite.Region;
        this.OriginalOffsetX = sprite.OriginalOffsetX;
        this.OriginalOffsetY = sprite.OriginalOffsetY;

        Set( sprite );
    }

    public new void SetPosition( float x, float y )
    {
        base.SetPosition( x + Region.OffsetX, y + Region.OffsetY );
    }

    public void SetX( float x )
    {
        base.X = x + Region.OffsetX;
    }

    public void SetY( float y )
    {
        base.Y = y + Region.OffsetY;
    }

    public new void SetBounds( float x, float y, float width, float height )
    {
        var widthRatio  = width / Region.OriginalWidth;
        var heightRatio = height / Region.OriginalHeight;

        Region.OffsetX = OriginalOffsetX * widthRatio;
        Region.OffsetY = OriginalOffsetY * heightRatio;

        int packedWidth  = Region.Rotate ? Region.PackedHeight : Region.PackedWidth;
        int packedHeight = Region.Rotate ? Region.PackedWidth : Region.PackedHeight;

        base.SetBounds
            (
             x + Region.OffsetX,
             y + Region.OffsetY,
             packedWidth * widthRatio,
             packedHeight * heightRatio
            );
    }

    public new void SetSize( float width, float height )
    {
        SetBounds( GetX(), GetY(), width, height );
    }

    public new void SetOrigin( float originX, float originY )
    {
        base.SetOrigin( originX - Region.OffsetX, originY - Region.OffsetY );
    }

    public new void SetOriginCenter()
    {
        base.SetOrigin( ( Width / 2 ) - Region.OffsetX, ( Height / 2 ) - Region.OffsetY );
    }

    public new void Flip( bool x, bool y )
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

    public new void Rotate90( bool clockwise )
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

    public float GetX()
    {
        return base.X - Region.OffsetX;
    }

    public float GetY()
    {
        return base.Y - Region.OffsetY;
    }

    public float GetOriginX()
    {
        return base.OriginX + Region.OffsetX;
    }

    public float GetOriginY()
    {
        return base.OriginY + Region.OffsetY;
    }

    public float GetWidth()
    {
        return ( base.Width / Region.RotatedPackedWidth ) * Region.OriginalWidth;
    }

    public float GetHeight()
    {
        return ( base.Height / Region.RotatedPackedHeight ) * Region.OriginalHeight;
    }

    public float GetWidthRatio()
    {
        return base.Width / Region.RotatedPackedWidth;
    }

    public float GetHeightRatio()
    {
        return base.Height / Region.RotatedPackedHeight;
    }

    public new string? ToString()
    {
        return Region.ToString();
    }
}