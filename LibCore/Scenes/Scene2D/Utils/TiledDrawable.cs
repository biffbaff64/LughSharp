﻿// ///////////////////////////////////////////////////////////////////////////////
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


using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Graphics.G2D;

namespace LughSharp.LibCore.Scenes.Scene2D.Utils;

/// <summary>
///     Draws a <see cref="TextureRegion" /> repeatedly to fill the area,
///     instead of stretching it.
/// </summary>
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

    public Color Color { get; set; } = new( 1, 1, 1, 1 );

    public float Scale { set; get; } = 1;

    public override void Draw( IBatch batch, float x, float y, float width, float height )
    {
        TextureRegion? region = Region;

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
        throw new NotSupportedException();
    }

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
