// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace LughSharp.LibCore.Graphics;

public partial class Pixelmap
{
    public void SetColor( int color )
    {
    }

    public void SetColor( int red, int green, int blue, int alpha )
    {
    }

    public void SetColor( Color color )
    {
    }

    public void FillWithCurrentColor()
    {
    }

    /// <summary>
    /// Draws a line between the given coordinates using the currently set color.
    /// </summary>
    /// <param name="x"> The x-coodinate of the first point </param>
    /// <param name="y"> The y-coordinate of the first point </param>
    /// <param name="x2"> The x-coordinate of the second point </param>
    /// <param name="y2"> The y-coordinate of the second point  </param>
    public void DrawLine( int x, int y, int x2, int y2 )
    {
//        PixelData.DrawLine( x, y, x2, y2, this.Color );
    }

    /// <summary>
    /// Draws a rectangle outline starting at x, y extending by width to the right
    /// and by height downwards (y-axis points downwards) using the current color.
    /// </summary>
    /// <param name="x"> The x coordinate </param>
    /// <param name="y"> The y coordinate </param>
    /// <param name="width"> The width in pixels </param>
    /// <param name="height"> The height in pixels  </param>
    public void DrawRectangle( int x, int y, int width, int height )
    {
//        PixelData.DrawRect( x, y, width, height, this.Color );
    }

    /// <summary>
    /// Draws an area from another Pixelmap to this Pixelmap.
    /// </summary>
    /// <param name="pixmap"> The other Pixelmap </param>
    /// <param name="x"> The target x-coordinate (top left corner) </param>
    /// <param name="y"> The target y-coordinate (top left corner)  </param>
    public void DrawPixmap( Pixelmap pixmap, int x, int y )
    {
//        DrawPixmap( pixmap, x, y, 0, 0, pixmap.Width, pixmap.Height );
    }

    /// <summary>
    /// Draws an area from another Pixelmap to this Pixelmap.
    /// </summary>
    /// <param name="pixmap"> The other Pixelmap </param>
    /// <param name="x"> The target x-coordinate (top left corner) </param>
    /// <param name="y"> The target y-coordinate (top left corner) </param>
    /// <param name="srcx"> The source x-coordinate (top left corner) </param>
    /// <param name="srcy"> The source y-coordinate (top left corner); </param>
    /// <param name="srcWidth"> The width of the area from the other Pixelmap in pixels </param>
    /// <param name="srcHeight"> The height of the area from the other Pixelmap in pixels  </param>
    public void DrawPixmap( Pixelmap pixmap, int x, int y, int srcx, int srcy, int srcWidth, int srcHeight )
    {
//        ArgumentNullException.ThrowIfNull( nameof( pixmap ), "Source Pixelmap cannot be null." );

//        try
//        {
//            PixelData.DrawPixmap( pixmap.PixelData, srcx, srcy, x, y, srcWidth, srcHeight );
//        }
//        catch ( Exception ex )
//        {
//            throw new GdxRuntimeException( "Error occurred while drawing the pixmap.", ex );
//        }
    }

    /// <summary>
    /// Draws an area from another Pixelmap to this Pixelmap. This will automatically
    /// scale and stretch the source image to the specified target rectangle.
    /// <para>
    /// Use <see cref="Pixelmap.Filter"/> property to specify the type of filtering to
    /// be used (NearestNeighbour or Bilinear).
    /// </para>
    /// </summary>
    /// <param name="pixmap"> The other Pixelmap </param>
    /// <param name="srcx"> The source x-coordinate (top left corner) </param>
    /// <param name="srcy"> The source y-coordinate (top left corner); </param>
    /// <param name="srcWidth"> The width of the area from the other Pixelmap in pixels </param>
    /// <param name="srcHeight"> The height of the area from the other Pixelmap in pixels </param>
    /// <param name="dstx"> The target x-coordinate (top left corner) </param>
    /// <param name="dsty"> The target y-coordinate (top left corner) </param>
    /// <param name="dstWidth"> The target width </param>
    /// <param name="dstHeight"> the target height  </param>
    public void DrawPixmap( Pixelmap pixmap, int srcx, int srcy, int srcWidth, int srcHeight, int dstx, int dsty, int dstWidth, int dstHeight )
    {
//        ArgumentNullException.ThrowIfNull( nameof( pixmap ), "Source Pixelmap cannot be null." );

//        try
//        {
//            PixelData.DrawPixmap( pixmap.PixelData, srcx, srcy, srcWidth, srcHeight, dstx, dsty, dstWidth, dstHeight );
//        }
//        catch ( Exception ex )
//        {
//            throw new GdxRuntimeException( "Error occurred while drawing the pixmap.", ex );
//        }
    }

    /// <summary>
    /// Fills a rectangle starting at x, y extending by width to the right and by
    /// height downwards (y-axis points downwards) using the current color.
    /// </summary>
    /// <param name="x"> The x coordinate </param>
    /// <param name="y"> The y coordinate </param>
    /// <param name="width"> The width in pixels </param>
    /// <param name="height"> The height in pixels  </param>
    public void FillRectangle( int x, int y, int width, int height )
    {
//        PixelData.FillRect( x, y, width, height, this.Color );
    }

    /// <summary>
    /// Draws a circle outline with the center at x,y and a radius using the
    /// current color and stroke width.
    /// </summary>
    /// <param name="x"> The x-coordinate of the center </param>
    /// <param name="y"> The y-coordinate of the center </param>
    /// <param name="radius"> The radius in pixels  </param>
    public void DrawCircle( int x, int y, int radius )
    {
//        PixelData.DrawCircle( x, y, radius, this.Color );
    }

    /// <summary>
    /// Fills a circle with the center at x,y and a radius using the current color.
    /// </summary>
    /// <param name="x"> The x-coordinate of the center </param>
    /// <param name="y"> The y-coordinate of the center </param>
    /// <param name="radius"> The radius in pixels  </param>
    public void FillCircle( int x, int y, int radius )
    {
//        PixelData.FillCircle( x, y, radius, this.Color );
    }

    /// <summary>
    /// Fills a triangle with vertices at x1,y1 and x2,y2 and x3,y3 using the current color.
    /// </summary>
    /// <param name="x1"> The x-coordinate of vertex 1 </param>
    /// <param name="y1"> The y-coordinate of vertex 1 </param>
    /// <param name="x2"> The x-coordinate of vertex 2 </param>
    /// <param name="y2"> The y-coordinate of vertex 2 </param>
    /// <param name="x3"> The x-coordinate of vertex 3 </param>
    /// <param name="y3"> The y-coordinate of vertex 3  </param>
    public void FillTriangle( int x1, int y1, int x2, int y2, int x3, int y3 )
    {
//        PixelData.FillTriangle( x1, y1, x2, y2, x3, y3, this.Color );
    }

    /// <summary>
    /// Returns the 32-bit RGBA8888 value of the pixel at x, y.
    /// For Alpha formats the RGB components will be one.
    /// </summary>
    /// <param name="x"> The x-coordinate </param>
    /// <param name="y"> The y-coordinate </param>
    /// <returns> The pixel color in RGBA8888 format.  </returns>
    public int GetPixel( int x, int y )
    {
        return Pixels[ x, y ].Color.ToIntBits();
    }

    /// <summary>
    /// Draws a pixel at the given location with the current color.
    /// </summary>
    /// <param name="x"> the x-coordinate </param>
    /// <param name="y"> the y-coordinate  </param>
    public void DrawPixel( int x, int y )
    {
//        PixelData.SetPixel( x, y, this.Color );
    }

    /// <summary>
    /// Draws a pixel at the given location with the given color.
    /// </summary>
    /// <param name="x"> the x-coordinate </param>
    /// <param name="y"> the y-coordinate </param>
    /// <param name="color"> the color in RGBA8888 format.  </param>
    public void DrawPixel( int x, int y, int color )
    {
//        PixelData.SetPixel( x, y, color );
    }

    /// <returns> the <see cref="Pixelmap.Format"/> of this Pixelmap. </returns>
    public Pixmap.Format GetFormat()
    {
        return PixmapFormat.FromGdx2DPixmapFormat( PixFormat );
    }

    /// <summary>
    /// Creates a Pixelmap from a part of the current framebuffer.
    /// </summary>
    /// <param name="x">Framebuffer region x</param>
    /// <param name="y">Framebuffer region y</param>
    /// <param name="width">Framebuffer region width</param>
    /// <param name="height">Framebuffer region height</param>
    /// <returns>The new Pixelmap.</returns>
    public static unsafe Pixelmap CreateFromFrameBuffer( int x, int y, int width, int height )
    {
        Gdx.GL.glPixelStorei( IGL.GL_PACK_ALIGNMENT, 1 );

        Pixelmap pixmap = new( width, height, Pixmap.Format.RGBA8888 );

//        fixed ( void* ptr = &pixmap.Pixels.BackingArray()[ 0 ] )
//        {
//            Gdx.GL.glReadPixels( x, y, width, height, IGL.GL_RGBA, IGL.GL_UNSIGNED_BYTE, ptr );
//        }

        return pixmap;
    }

    /// <summary>
    /// Sets the type of interpolation <see cref="BlendTypes"/> to be used in
    /// conjunction with <see cref="DrawPixmap(Pixelmap, int, int, int, int, int, int, int, int)"/>.
    /// </summary>
    public Filter FilterValue
    {
        get => _filter;
        set
        {
            _filter = value;

            Scale = _filter == Filter.NearestNeighbour
                        ? Gdx2DPixmap.GDX_2D_SCALE_NEAREST
                        : Gdx2DPixmap.GDX_2D_SCALE_LINEAR;
        }
    }
}
