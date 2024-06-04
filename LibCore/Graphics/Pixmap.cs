// ///////////////////////////////////////////////////////////////////////////////
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


using LughSharp.LibCore.Utils.Exceptions;
using Exception = System.Exception;

namespace LughSharp.LibCore.Graphics;

/// <summary>
/// A Pixmap represents an image in memory. It has a width and height expressed
/// in pixels as well as a <see cref="Format"/> specifying the number and order
/// of color components per pixel.
/// <para>
/// Coordinates of pixels are specified with respect to the top left corner of
/// the image, with the x-axis pointing to the right and the y-axis pointing
/// downwards.
/// </para>
/// <para>
/// By default all methods use blending. You can disable blending by setting it
/// to <see cref="BlendTypes.None"/>, which may reduce blitting time by ~30%.
/// </para>
/// <para>
/// The <see cref="DrawPixmap(Pixmap, int, int, int, int, int, int, int, int)"/> method
/// will scale and stretch the source image to a target image. There either nearest
/// neighbour or bilinear filtering can be used.
/// </para>
/// </summary>
[PublicAPI]
public class Pixmap : IDisposable
{
    private int _color = 0;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new Pixmap instance with the given width, height and format.
    /// </summary>
    /// <param name="width">The width in pixels.</param>
    /// <param name="height">The height in pixels.</param>
    /// <param name="format">The <see cref="Pixmap.Format"/></param>
    public Pixmap( int width, int height, Format format )
    {
        GDX2DPixmap = new Gdx2DPixmap( width, height, PixmapFormat.ToGdx2DPixmapFormat( format ) );
        SetColor( 0, 0, 0, 0 );
        FillWithCurrentColor();
    }

    /// <summary>
    /// Creates a new Pixmap instance from the given encoded image data.
    /// <para>
    /// The image can be encoded as JPEG, PNG or BMP.
    /// </para>
    /// </summary>
    public Pixmap( byte[] encodedData, int offset, int len )
    {
        try
        {
            GDX2DPixmap = new Gdx2DPixmap( encodedData, offset, len, 0 );
        }
        catch ( IOException e )
        {
            throw new GdxRuntimeException( "Couldn't load pixmap from image data", e );
        }
    }

    /// <summary>
    /// Creates a new Pixmap instance from the given file.
    /// <para>
    /// The file must be a Png, Jpeg or Bitmap. Paletted formats are not supported.
    /// </para>
    /// </summary>
    /// <param name="file"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public Pixmap( FileInfo file )
    {
        var fs = file.Open( FileMode.Open );

        try
        {
            var bytes = new byte[ fs.Length ];

            if ( fs.Read( bytes, 0, bytes.Length ) == 0 )
            {
                throw new FileLoadException( $"Error reading from {file.Name}: No data found." );
            }

            GDX2DPixmap = new Gdx2DPixmap( bytes, 0, bytes.Length, 0 );
        }
        catch ( Exception e )
        {
            throw new GdxRuntimeException( $"Couldn't load file:  {file.Name}", e );
        }
    }

    /// <summary>
    /// Creates a new Pixmap from the supplied <see cref="Gdx2DPixmap"/>.
    /// </summary>
    public Pixmap( Gdx2DPixmap pixmap )
    {
        GDX2DPixmap = pixmap;
    }

    public Gdx2DPixmap GDX2DPixmap { get; set; }
    public bool        IsDisposed  { get; set; } = false;

    // ----------------------------------------------------------

    /// <summary>
    /// Returns the width of the Pixmap in pixels.
    /// </summary>
    public int Width => GDX2DPixmap.Width;

    /// <summary>
    /// Returns the height of the Pixmap in pixels.
    /// </summary>
    public int Height => GDX2DPixmap.Height;

    /// <summary>
    /// Returns the OpenGL ES format of this Pixmap.
    /// </summary>
    /// <returns> one of GL_ALPHA, GL_RGB, GL_RGBA, GL_LUMINANCE, or GL_LUMINANCE_ALPHA.</returns>
    public int GLFormat => GDX2DPixmap.GLFormat;

    /// <summary>
    /// Returns the OpenGL ES internal format of this Pixmap.
    /// </summary>
    /// <returns> one of GL_ALPHA, GL_RGB, GL_RGBA, GL_LUMINANCE, or GL_LUMINANCE_ALPHA.</returns>
    public int GLInternalFormat => GDX2DPixmap.GLInternalFormat;

    /// <summary>
    /// Returns the OpenGL ES type of this Pixmap.
    /// </summary>
    /// <returns> one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_4_4_4_4 </returns>
    public int GLType => GDX2DPixmap.GLType;

    /// <summary>
    /// Returns the direct ByteBuffer holding the pixel data. For the format Alpha each
    /// value is encoded as a byte.
    /// <para>
    /// For the format LuminanceAlpha the luminance is the first byte and the alpha is
    /// the second byte of the pixel.
    /// </para>
    /// <para>
    /// For the formats RGB888 and RGBA8888 the color components are stored in a single
    /// byte each in the order red, green, blue (alpha).
    /// </para>
    /// <para>
    /// For the formats RGB565 and RGBA4444 the pixel colors are stored in shorts in
    /// machine dependent order.
    /// </para>
    /// </summary>
    /// <returns> the direct <see cref="ByteBuffer"/> holding the pixel data.  </returns>
    public ByteBuffer Pixels
    {
        get
        {
            if ( IsDisposed )
            {
                throw new GdxRuntimeException( "Pixmap already disposed" );
            }

            return GDX2DPixmap.PixelPtr;
        }
        set
        {
            var dst = GDX2DPixmap.PixelPtr;

            BufferUtils.Copy( value, dst, dst.Limit );
        }
    }

    /// <summary>
    /// Downloads an image from http(s) url and passes it as a Pixmap to the
    /// specified <see cref="IDownloadPixmapResponseListener"/>.
    /// </summary>
    /// <param name="url">http url to download the image from.</param>
    /// <param name="responseListener">the listener to call once the image is available as a Pixmap</param>
    public static void DownloadFromUrl( string url, IDownloadPixmapResponseListener responseListener )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the color for drawing operations.
    /// </summary>
    /// <param name="color"> the color, encoded as RGBA8888  </param>
    public void SetColor( int color )
    {
        _color = color;
    }

    /// <summary>
    /// Sets the color for drawing operations.
    /// </summary>
    /// <param name="r"> The red component. </param>
    /// <param name="g"> The green component. </param>
    /// <param name="b"> The blue component. </param>
    /// <param name="a"> The alpha component.  </param>
    public void SetColor( float r, float g, float b, float a )
    {
        _color = Color.RGBA8888( r, g, b, a );
    }

    /// <summary>
    /// Sets the color for drawing operations.
    /// </summary>
    /// <param name="color"> The color.</param>
    public void SetColor( Color color )
    {
        _color = Color.RGBA8888( color.R, color.G, color.B, color.A );
    }

    /// <summary>
    /// Fills the complete bitmap with the currently set color.
    /// </summary>
    public void FillWithCurrentColor()
    {
        GDX2DPixmap.Clear( _color );
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
        GDX2DPixmap.DrawLine( x, y, x2, y2, _color );
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
        GDX2DPixmap.DrawRect( x, y, width, height, _color );
    }

    /// <summary>
    /// Draws an area from another Pixmap to this Pixmap.
    /// </summary>
    /// <param name="pixmap"> The other Pixmap </param>
    /// <param name="x"> The target x-coordinate (top left corner) </param>
    /// <param name="y"> The target y-coordinate (top left corner)  </param>
    public void DrawPixmap( Pixmap pixmap, int x, int y )
    {
        DrawPixmap( pixmap, x, y, 0, 0, pixmap.Width, pixmap.Height );
    }

    /// <summary>
    /// Draws an area from another Pixmap to this Pixmap.
    /// </summary>
    /// <param name="pixmap"> The other Pixmap </param>
    /// <param name="x"> The target x-coordinate (top left corner) </param>
    /// <param name="y"> The target y-coordinate (top left corner) </param>
    /// <param name="srcx"> The source x-coordinate (top left corner) </param>
    /// <param name="srcy"> The source y-coordinate (top left corner); </param>
    /// <param name="srcWidth"> The width of the area from the other Pixmap in pixels </param>
    /// <param name="srcHeight"> The height of the area from the other Pixmap in pixels  </param>
    public void DrawPixmap( Pixmap pixmap, int x, int y, int srcx, int srcy, int srcWidth, int srcHeight )
    {
        ArgumentNullException.ThrowIfNull( nameof( pixmap ), "Source Pixmap cannot be null." );

        try
        {
            GDX2DPixmap.DrawPixmap( pixmap.GDX2DPixmap, srcx, srcy, x, y, srcWidth, srcHeight );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error occurred while drawing the pixmap.", ex );
        }
    }

    /// <summary>
    /// Draws an area from another Pixmap to this Pixmap. This will automatically
    /// scale and stretch the source image to the specified target rectangle.
    /// <para>
    /// Use <see cref="Pixmap.Filter"/> property to specify the type of filtering to
    /// be used (NearestNeighbour or Bilinear).
    /// </para>
    /// </summary>
    /// <param name="pixmap"> The other Pixmap </param>
    /// <param name="srcx"> The source x-coordinate (top left corner) </param>
    /// <param name="srcy"> The source y-coordinate (top left corner); </param>
    /// <param name="srcWidth"> The width of the area from the other Pixmap in pixels </param>
    /// <param name="srcHeight"> The height of the area from the other Pixmap in pixels </param>
    /// <param name="dstx"> The target x-coordinate (top left corner) </param>
    /// <param name="dsty"> The target y-coordinate (top left corner) </param>
    /// <param name="dstWidth"> The target width </param>
    /// <param name="dstHeight"> the target height  </param>
    public void DrawPixmap( Pixmap pixmap, int srcx, int srcy, int srcWidth, int srcHeight, int dstx, int dsty, int dstWidth, int dstHeight )
    {
        ArgumentNullException.ThrowIfNull( nameof( pixmap ), "Source Pixmap cannot be null." );

        try
        {
            GDX2DPixmap.DrawPixmap( pixmap.GDX2DPixmap, srcx, srcy, srcWidth, srcHeight, dstx, dsty, dstWidth, dstHeight );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error occurred while drawing the pixmap.", ex );
        }
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
        GDX2DPixmap.FillRect( x, y, width, height, _color );
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
        GDX2DPixmap.DrawCircle( x, y, radius, _color );
    }

    /// <summary>
    /// Fills a circle with the center at x,y and a radius using the current color.
    /// </summary>
    /// <param name="x"> The x-coordinate of the center </param>
    /// <param name="y"> The y-coordinate of the center </param>
    /// <param name="radius"> The radius in pixels  </param>
    public void FillCircle( int x, int y, int radius )
    {
        GDX2DPixmap.FillCircle( x, y, radius, _color );
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
        GDX2DPixmap.FillTriangle( x1, y1, x2, y2, x3, y3, _color );
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
        return GDX2DPixmap.GetPixel( x, y );
    }

    /// <summary>
    /// Draws a pixel at the given location with the current color.
    /// </summary>
    /// <param name="x"> the x-coordinate </param>
    /// <param name="y"> the y-coordinate  </param>
    public void DrawPixel( int x, int y )
    {
        GDX2DPixmap.SetPixel( x, y, _color );
    }

    /// <summary>
    /// Draws a pixel at the given location with the given color.
    /// </summary>
    /// <param name="x"> the x-coordinate </param>
    /// <param name="y"> the y-coordinate </param>
    /// <param name="color"> the color in RGBA8888 format.  </param>
    public void DrawPixel( int x, int y, int color )
    {
        GDX2DPixmap.SetPixel( x, y, color );
    }

    /// <returns> the <see cref="Pixmap.Format"/> of this Pixmap. </returns>
    public Format GetFormat()
    {
        return PixmapFormat.FromGdx2DPixmapFormat( GDX2DPixmap.Format );
    }

    /// <summary>
    /// Creates a Pixmap from a part of the current framebuffer.
    /// </summary>
    /// <param name="x">Framebuffer region x</param>
    /// <param name="y">Framebuffer region y</param>
    /// <param name="width">Framebuffer region width</param>
    /// <param name="height">Framebuffer region height</param>
    /// <returns>The new Pixmap.</returns>
    public static unsafe Pixmap CreateFromFrameBuffer( int x, int y, int width, int height )
    {
        Gdx.GL.glPixelStorei( IGL.GL_PACK_ALIGNMENT, 1 );

        Pixmap pixmap = new( width, height, Format.RGBA8888 );

        fixed ( void* ptr = &pixmap.Pixels.BackingArray()[ 0 ] )
        {
            Gdx.GL.glReadPixels( x, y, width, height, IGL.GL_RGBA, IGL.GL_UNSIGNED_BYTE, ptr );
        }

        return pixmap;
    }

    /// <summary>
    /// Returns the pixel format from a valid named string.
    /// </summary>
    public static Format FormatFromString( string str )
    {
        str = str.ToLower();

        return str switch
        {
            "alpha"          => Format.Alpha,
            "intensity"      => Format.Intensity,
            "luminancealpha" => Format.LuminanceAlpha,
            "rgb565"         => Format.RGB565,
            "rgba4444"       => Format.RGBA4444,
            "rgb888"         => Format.RGB888,
            "rgba8888"       => Format.RGBA8888,
            var _            => throw new GdxRuntimeException( $"Unknown Format: {str}" )
        };
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Response listener for <see cref="Pixmap.DownloadFromUrl(String, IDownloadPixmapResponseListener)"/>
    /// </summary>
    [PublicAPI]
    public interface IDownloadPixmapResponseListener
    {
        /// <summary>
        /// Called on the render thread when image was downloaded successfully.
        /// </summary>
        void DownloadComplete( Pixmap pixmap );

        /// <summary>
        /// Called when image download failed. This might get called on a background thread.
        /// </summary>
        void DownloadFailed( Exception e );
    }

    // ------------------------------------------------------------------------

    #region dispose pattern

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( !IsDisposed );
    }

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            GDX2DPixmap.Dispose();
            IsDisposed = true;
        }
    }

    #endregion dispose pattern

    // ------------------------------------------------------------------------

    #region PixmapEnums

    /// <summary>
    /// Blending functions to be set with <see cref="Pixmap.Blending"/>.
    /// </summary>
    public enum BlendTypes
    {
        None,
        SourceOver
    }

    /// <summary>
    /// Filters to be used with <see cref="DrawPixmap(Pixmap, int, int, int, int, int, int, int, int)"/>.
    /// </summary>
    public enum Filter
    {
        NearestNeighbour,
        BiLinear
    }

    /// <summary>
    /// Available Pixmap pixel formats.
    /// </summary>
    public enum Format
    {
        Alpha,
        Intensity,
        LuminanceAlpha,
        RGB565,
        RGBA4444,
        RGB888,
        RGBA8888
    }

    #endregion

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region PublicProperties

    private BlendTypes _blend  = BlendTypes.SourceOver;
    private Filter     _filter = Filter.BiLinear;

    /// <summary>
    /// Sets the type of <see cref="BlendTypes"/> to be used for all operations.
    /// Default is <see cref="BlendTypes.SourceOver"/>.
    /// </summary>
    public BlendTypes Blending
    {
        get => _blend;
        set
        {
            _blend            = value;
            GDX2DPixmap.Blend = _blend == BlendTypes.None ? 0 : 1;
        }
    }

    /// <summary>
    /// Sets the type of interpolation <see cref="BlendTypes"/> to be used in
    /// conjunction with <see cref="DrawPixmap(Pixmap, int, int, int, int, int, int, int, int)"/>.
    /// </summary>
    public Filter FilterValue
    {
        get => _filter;
        set
        {
            _filter = value;

            GDX2DPixmap.Scale = _filter == Filter.NearestNeighbour
                                    ? Gdx2DPixmap.GDX_2D_SCALE_NEAREST
                                    : Gdx2DPixmap.GDX_2D_SCALE_LINEAR;
        }
    }

    #endregion
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

[PublicAPI]
public static class PixmapFormatExtensions
{
    public static int ToGLType( this Pixmap.Format format )
    {
        return Gdx2DPixmap.ToGLType( ToGdx2DPixmapFormat( format ) );
    }

    public static int ToGLFormat( this Pixmap.Format format )
    {
        return Gdx2DPixmap.ToGLFormat( ToGdx2DPixmapFormat( format ) );
    }

    public static int ToGdx2DPixmapFormat( this Pixmap.Format format )
    {
        return format switch
        {
            Pixmap.Format.Alpha          => Gdx2DPixmap.GDX_2D_FORMAT_ALPHA,
            Pixmap.Format.Intensity      => Gdx2DPixmap.GDX_2D_FORMAT_ALPHA,
            Pixmap.Format.LuminanceAlpha => Gdx2DPixmap.GDX_2D_FORMAT_LUMINANCE_ALPHA,
            Pixmap.Format.RGB565         => Gdx2DPixmap.GDX_2D_FORMAT_RGB565,
            Pixmap.Format.RGBA4444       => Gdx2DPixmap.GDX_2D_FORMAT_RGBA4444,
            Pixmap.Format.RGB888         => Gdx2DPixmap.GDX_2D_FORMAT_RGB888,
            Pixmap.Format.RGBA8888       => Gdx2DPixmap.GDX_2D_FORMAT_RGBA8888,

            var _ => throw new GdxRuntimeException( $"Unknown format: {format}" )
        };
    }
}
