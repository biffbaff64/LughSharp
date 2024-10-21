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

using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Graphics.G2D;

/// <summary>
/// Defines a rectangular area of a texture. The coordinate system used has
/// its origin in the upper left corner with the x-axis pointing to the
/// right and the y axis pointing downwards.
/// </summary>
[PublicAPI]
public class TextureRegion
{
    // ------------------------------------------------------------------------

    private int   _regionHeight;
    private int   _regionWidth;
    private float _u;
    private float _u2;
    private float _v;
    private float _v2;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Constructs a region that cannot be used until a texture
    /// and texture coordinates are set.
    /// </summary>
    public TextureRegion()
    {
        Logger.Checkpoint();
    }

    /// <summary>
    /// Constructs a region the size of the specified texture.
    /// </summary>
    /// <param name="texture"></param>
    public TextureRegion( Texture texture )
    {
        Logger.Checkpoint();

        Texture = texture;

        SetRegion( 0, 0, texture.Width, texture.Height );
    }

    /// <param name="texture"></param>
    /// <param name="width">
    /// The width of the texture region. May be negative to flip the sprite when drawn.
    /// </param>
    /// <param name="height">
    /// The height of the texture region. May be negative to flip the sprite when drawn.
    /// </param>
    public TextureRegion( Texture texture, int width, int height )
        : this( texture, 0, 0, width, height )
    {
        Logger.Checkpoint();
    }

    /// <param name="texture"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width">
    /// The width of the texture region. May be negative to flip the sprite when drawn.
    /// </param>
    /// <param name="height">
    /// The height of the texture region. May be negative to flip the sprite when drawn.
    /// </param>
    public TextureRegion( Texture texture, int x, int y, int width, int height )
    {
        Logger.Checkpoint();

        Texture = texture;
        SetRegion( x, y, width, height );
    }

    /// <summary>
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <param name="u2"></param>
    /// <param name="v2"></param>
    public TextureRegion( Texture texture, float u, float v, float u2, float v2 )
    {
        Logger.Checkpoint();

        Texture = texture;
        SetRegionNV( u, v, u2, v2 );
    }

    /// <summary>
    /// </summary>
    /// <param name="region"></param>
    public TextureRegion( TextureRegion region )
    {
        Logger.Checkpoint();

        SetRegion( region );
    }

    /// <summary>
    /// </summary>
    /// <param name="region"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public TextureRegion( TextureRegion region, int x, int y, int width, int height )
    {
        Logger.Checkpoint();

        SetRegion( region, x, y, width, height );
    }

    public Texture Texture { get; set; } = null!;

    /// <summary>
    /// </summary>
    public virtual float U
    {
        get => _u;
        set
        {
            _u = value;

            RegionWidth = ( int ) Math.Round( Math.Abs( _u2 - _u ) * Texture.Width );
        }
    }

    /// <summary>
    /// </summary>
    public virtual float U2
    {
        get => _u2;
        set
        {
            _u2 = value;

            RegionWidth = ( int ) Math.Round( Math.Abs( _u2 - _u ) * Texture.Width );
        }
    }

    /// <summary>
    /// </summary>
    public virtual float V
    {
        get => _v;
        set
        {
            _v = value;

            RegionHeight = ( int ) Math.Round( Math.Abs( _v2 - _v ) * Texture.Height );
        }
    }

    /// <summary>
    /// </summary>
    public virtual float V2
    {
        get => _v2;
        set
        {
            _v2 = value;

            RegionHeight = ( int ) Math.Round( Math.Abs( _v2 - _v ) * Texture.Height );
        }
    }

    /// <summary>
    /// </summary>
    public int RegionX
    {
        get => ( int ) Math.Round( U * Texture.Width );
        set => U = value / ( float ) Texture.Width;
    }

    /// <summary>
    /// </summary>
    public int RegionY
    {
        get => ( int ) Math.Round( V * Texture.Height );
        set => V = value / ( float ) Texture.Height;
    }

    /// <summary>
    /// This TextureRegions Width property.
    /// </summary>
    public int RegionWidth
    {
        get => _regionWidth;
        set
        {
            if ( IsFlipX() )
            {
                U = U2 + ( value / ( float ) Texture.Width );
            }
            else
            {
                U2 = U + ( value / ( float ) Texture.Width );
            }

            _regionWidth = value;
        }
    }

    /// <summary>
    /// This TextureRegions Height property.
    /// </summary>
    public int RegionHeight
    {
        get => _regionHeight;
        set
        {
            if ( IsFlipY() )
            {
                V = V2 + ( value / ( float ) Texture.Height );
            }
            else
            {
                V2 = V + ( value / ( float ) Texture.Height );
            }

            _regionHeight = value;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="texture"></param>
    public void SetRegion( Texture texture )
    {
        Texture = texture;
        SetRegion( 0, 0, texture.Width, texture.Height );
    }

    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public void SetRegion( int x, int y, int width, int height )
    {
        if ( Texture == null )
        {
            throw new GdxRuntimeException( "Texture cannot be null" );
        }

        var invTexWidth  = 1f / Texture.Width;
        var invTexHeight = 1f / Texture.Height;

        SetRegion( x * invTexWidth, y * invTexHeight, ( x + width ) * invTexWidth, ( y + height ) * invTexHeight );

        RegionWidth  = Math.Abs( width );
        RegionHeight = Math.Abs( height );
    }

    /// <summary>
    /// Non-Virtual version of <see cref="SetRegion( float, float, float, float )"/>,
    /// enabling this to be called from constructors.
    /// </summary>
    private void SetRegionNV( float u, float v, float u2, float v2 )
    {
        SetRegion( u, v, u2, v2 );
    }

    /// <summary>
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <param name="u2"></param>
    /// <param name="v2"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public virtual void SetRegion( float u, float v, float u2, float v2 )
    {
        if ( Texture == null )
        {
            throw new GdxRuntimeException( "Texture cannot be null" );
        }

        var texWidth  = Texture.Width;
        var texHeight = Texture.Height;

        this.RegionWidth  = ( int ) Math.Round( Math.Abs( u2 - u ) * texWidth );
        this.RegionHeight = ( int ) Math.Round( Math.Abs( v2 - v ) * texHeight );

        // For a 1x1 region, adjust UVs toward pixel center to avoid filtering
        // artifacts on AMD GPUs when drawing very stretched.
        if ( ( this.RegionWidth == 1 ) && ( this.RegionHeight == 1 ) )
        {
            var adjustX = 0.25f / texWidth;

            u  += adjustX;
            u2 -= adjustX;

            var adjustY = 0.25f / texHeight;

            v  += adjustY;
            v2 -= adjustY;
        }

        this.U  = u;
        this.V  = v;
        this.U2 = u2;
        this.V2 = v2;
    }

    /// <summary>
    /// </summary>
    /// <param name="region"></param>
    public void SetRegion( TextureRegion region )
    {
        Texture = region.Texture;
        SetRegion( region.U, region.V, region.U2, region.V2 );
    }

    /// <summary>
    /// </summary>
    /// <param name="region"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetRegion( TextureRegion region, int x, int y, int width, int height )
    {
        Texture = region.Texture;
        SetRegion( region.RegionX + x, region.RegionY + y, width, height );
    }

    /// <summary>
    /// Flips this TextureRegion horizontally, vertically, or both.
    /// </summary>
    /// <param name="x"> TRUE to flip horizontally. </param>
    /// <param name="y"> TRUE to flip vertically. </param>
    public virtual void Flip( bool x, bool y )
    {
        if ( x )
        {
            ( U, U2 ) = ( U2, U );
        }

        if ( y )
        {
            ( V, V2 ) = ( V2, V );
        }
    }

    /// <summary>
    /// Returns true if this TextureRegion is flipped horizontally.
    /// </summary>
    public virtual bool IsFlipX()
    {
        return U > U2;
    }

    /// <summary>
    /// Returns true if this TextureRegion is flipped vertically.
    /// </summary>
    public virtual bool IsFlipY()
    {
        return V > V2;
    }

    /// <summary>
    /// Offsets the region relative to the current region. Generally the region's
    /// size should be the entire size of the texture in the direction(s) it is
    /// scrolled.
    /// </summary>
    /// <param name="xAmount">The percentage to offset horizontally.</param>
    /// <param name="yAmount">
    /// The percentage to offset vertically.
    /// This is done in texture space, so up is negative.
    /// </param>
    public virtual void Scroll( float xAmount, float yAmount )
    {
        if ( xAmount != 0 )
        {
            var width = ( U2 - U ) * Texture.Width;

            U  = ( U + xAmount ) % 1;
            U2 = U + ( width / Texture.Width );
        }

        if ( yAmount != 0 )
        {
            var height = ( V2 - V ) * Texture.Height;

            V  = ( V + yAmount ) % 1;
            V2 = V + ( height / Texture.Height );
        }
    }

    /// <summary>
    /// Helper function to create tiles out of this TextureRegion starting from the
    /// top left corner going to the right and ending at the bottom right corner.
    /// Only complete tiles will be returned so if the region's width or height are
    /// not a multiple of the tile width and height not all of the region will be
    /// used. This will not work on texture regions returned form a TextureAtlas that
    /// either have whitespace removed or where flipped before the region is split.
    /// </summary>
    /// <param name="tileWidth">Required tile's width in pixels.</param>
    /// <param name="tileHeight">Required tile's height in pixels.</param>
    /// <returns>A 2D array of TextureRegions index by [row, column].</returns>
    public TextureRegion[ , ] Split( int tileWidth, int tileHeight )
    {
        var x      = RegionX;
        var y      = RegionY;
        var width  = RegionWidth;
        var height = RegionHeight;

        var rows = height / tileHeight;
        var cols = width / tileWidth;

        var startX = x;
        var tiles  = new TextureRegion[ rows, cols ];

        for ( var row = 0; row < rows; row++, y += tileHeight )
        {
            x = startX;

            for ( var col = 0; col < cols; col++, x += tileWidth )
            {
                tiles[ row, col ] = new TextureRegion( Texture, x, y, tileWidth, tileHeight );
            }
        }

        return tiles;
    }

    /// <summary>
    /// Helper function to create tiles out of the given Texture starting from the
    /// top left corner going to the right and ending at the bottom right corner.
    /// Only complete tiles will be returned so if the texture's width or height are
    /// not a multiple of the tile width and height not all of the texture will be used.
    /// </summary>
    /// <param name="texture">The texture to split.</param>
    /// <param name="tileWidth">Required tile's width in pixels.</param>
    /// <param name="tileHeight">Required tile's height in pixels.</param>
    /// <returns>A 2D array of TextureRegions index by [row, column].</returns>
    public static TextureRegion[ , ] Split( Texture texture, int tileWidth, int tileHeight )
    {
        var region = new TextureRegion( texture );

        return region.Split( tileWidth, tileHeight );
    }
}
