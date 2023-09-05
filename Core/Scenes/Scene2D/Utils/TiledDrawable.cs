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

using LibGDXSharp.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// Draws a <see cref="TextureRegion"/> repeatedly to fill the area,
/// instead of stretching it.
/// </summary>
[PublicAPI]
public class TiledDrawable : TextureRegionDrawable
{
    public TiledDrawable( TextureRegion region )
        : base( region )
    {
    }

    public TiledDrawable( TextureRegionDrawable drawable )
        : base( drawable )
    {
    }

    public override void Draw( IBatch batch, float x, float y, float width, float height )
    {
        TextureRegion? region = this.Region;

        if ( region != null )
        {
            var oldColor = batch.PackedColor;

            batch.Color = Color.Mul( batch.Color );

            var regionWidth  = region.RegionWidth * Scale;
            var regionHeight = region.RegionHeight * Scale;
            var fullX        = ( int )( width / regionWidth );
            var fullY        = ( int )( height / regionHeight );
            var remainingX   = width - ( regionWidth * fullX );
            var remainingY   = height - ( regionHeight * fullY );
            var startX       = x;
            var startY       = y;

            for ( var i = 0; i < fullX; i++ )
            {
                y = startY;

                for ( var ii = 0; ii < fullY; ii++ )
                {
                    batch.Draw( region, x, y, regionWidth, regionHeight );
                    y += regionHeight;
                }

                x += regionWidth;
            }

            Texture texture = region.Texture;

            var u  = region.U;
            var v2 = region.V2;

            if ( remainingX > 0 )
            {
                // Right edge.
                var u2 = u + ( remainingX / ( texture.Width * Scale ) );
                var v  = region.V;

                y = startY;

                for ( var ii = 0; ii < fullY; ii++ )
                {
                    batch.Draw( texture, x, y, remainingX, regionHeight, u, v2, u2, v );
                    y += regionHeight;
                }

                // Upper right corner.
                if ( remainingY > 0 )
                {
                    v = v2 - ( remainingY / ( texture.Height * Scale ) );
                    batch.Draw( texture, x, y, remainingX, remainingY, u, v2, u2, v );
                }
            }

            if ( remainingY > 0 )
            {
                // Top edge.
                var u2 = region.U2;
                var v  = v2 - ( remainingY / ( texture.Height * Scale ) );

                x = startX;

                for ( var i = 0; i < fullX; i++ )
                {
                    batch.Draw( texture, x, y, regionWidth, remainingY, u, v2, u2, v );
                    x += regionWidth;
                }
            }

            batch.PackedColor = oldColor;
        }
    }

    public override void Draw( IBatch batch,
                          float x,
                          float y,
                          float originX,
                          float originY,
                          float width,
                          float height,
                          float scaleX,
                          float scaleY,
                          float rotation )
    {
        throw new System.NotSupportedException();
    }

    public Color Color { get; set; } = new( 1, 1, 1, 1 );

    public float Scale { set; get; } = 1;

    public override TiledDrawable Tint( Color tint )
    {
        var drawable = new TiledDrawable( this );

        drawable.Color.Set( tint );
        drawable.LeftWidth    = LeftWidth;
        drawable.RightWidth   = RightWidth;
        drawable.TopHeight    = TopHeight;
        drawable.BottomHeight = BottomHeight;

        return drawable;
    }
}