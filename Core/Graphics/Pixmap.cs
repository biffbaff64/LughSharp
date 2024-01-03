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

using LibGDXSharp.Files.Buffers;
using LibGDXSharp.Graphics.G2D;

namespace LibGDXSharp.Graphics;

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
    // ----------------------------------------------------------

    #region PixmapEnums

    public enum BlendTypes
    {
        None,
        SourceOver
    }

    public enum Filter
    {
        NearestNeighbour,
        BiLinear
    }

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

    // ----------------------------------------------------------

    public readonly Gdx2DPixmap gdx2DPixmap;
    public          bool        isDisposed = false;

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
            this._blend       = value;
            gdx2DPixmap.Blend = _blend == BlendTypes.None ? 0 : 1;
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
            this._filter = value;

            gdx2DPixmap.Scale =
                (
                _filter == Filter.NearestNeighbour
                    ? Gdx2DPixmap.GDX_2D_SCALE_NEAREST
                    : Gdx2DPixmap.GDX_2D_SCALE_LINEAR
                );
        }
    }

    #endregion

    private int _color = 0;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    /// <summary>
    /// Creates a new Pixmap instance with the given width, height and format.
    /// </summary>
    /// <param name="width">The width in pixels.</param>
    /// <param name="height">The height in pixels.</param>
    /// <param name="format">The <see cref="Pixmap.Format"/></param>
    public Pixmap( int width, int height, Format format )
    {
        gdx2DPixmap = new Gdx2DPixmap( width, height, PixmapFormat.ToGdx2DPixmapFormat( format ) );
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
            gdx2DPixmap = new Gdx2DPixmap( encodedData, offset, len, 0 );
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
        FileStream fs = file.Open( FileMode.Open );

        try
        {
            var bytes = new byte[ fs.Length ];

            if ( ( fs.Read( bytes, 0, bytes.Length ) ) == 0 )
            {
                throw new FileLoadException( $"Error reading from {file.Name}: No data found." );
            }

            gdx2DPixmap = new Gdx2DPixmap( bytes, 0, bytes.Length, 0 );
        }
        catch ( System.Exception e )
        {
            throw new GdxRuntimeException( $"Couldn't load file:  {file.Name}", e );
        }
    }

    /// <summary>
    /// Creates a new Pixmap from the supplied <see cref="Gdx2DPixmap"/>.
    /// </summary>
    public Pixmap( Gdx2DPixmap pixmap )
    {
        this.gdx2DPixmap = pixmap;
    }

    /// <summary>
    /// Downloads an image from http(s) url and passes it as a Pixmap to the
    /// specified <see cref="IDownloadPixmapResponseListener"/>.
    /// </summary>
    /// <param name="url">http url to download the image from.</param>
    /// <param name="responseListener">the listener to call once the image is available as a Pixmap</param>
    public static void DownloadFromUrl( string url, IDownloadPixmapResponseListener responseListener )
    {
        //TODO:
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the color for drawing operations.
    /// </summary>
    /// <param name="color"> the color, encoded as RGBA8888  </param>
    public void SetColor( int color ) => this._color = color;

    /// <summary>
    /// Sets the color for drawing operations.
    /// </summary>
    /// <param name="r"> The red component. </param>
    /// <param name="g"> The green component. </param>
    /// <param name="b"> The blue component. </param>
    /// <param name="a"> The alpha component.  </param>
    public void SetColor( float r, float g, float b, float a ) => _color = Color.RGBA8888( r, g, b, a );

    /// <summary>
    /// Sets the color for drawing operations.
    /// </summary>
    /// <param name="color"> The color.</param>
    public void SetColor( Color color ) => this._color = Color.RGBA8888( color.R, color.G, color.B, color.A );

    /// <summary>
    /// Fills the complete bitmap with the currently set color.
    /// </summary>
    public void FillWithCurrentColor() => gdx2DPixmap.Clear( _color );

    /// <summary>
    /// Draws a line between the given coordinates using the currently set color.
    /// </summary>
    /// <param name="x"> The x-coodinate of the first point </param>
    /// <param name="y"> The y-coordinate of the first point </param>
    /// <param name="x2"> The x-coordinate of the second point </param>
    /// <param name="y2"> The y-coordinate of the second point  </param>
    public void DrawLine( int x, int y, int x2, int y2 )
    {
        gdx2DPixmap.DrawLine( x, y, x2, y2, _color );
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
        gdx2DPixmap.DrawRect( x, y, width, height, _color );
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
        this.gdx2DPixmap.DrawPixmap( pixmap.gdx2DPixmap, srcx, srcy, x, y, srcWidth, srcHeight );
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
    public void DrawPixmap( Pixmap pixmap,
                            int srcx,
                            int srcy,
                            int srcWidth,
                            int srcHeight,
                            int dstx,
                            int dsty,
                            int dstWidth,
                            int dstHeight )
    {
        this.gdx2DPixmap.DrawPixmap(
            pixmap.gdx2DPixmap,
            srcx,
            srcy,
            srcWidth,
            srcHeight,
            dstx,
            dsty,
            dstWidth,
            dstHeight
            );
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
        gdx2DPixmap.FillRect( x, y, width, height, _color );
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
        gdx2DPixmap.DrawCircle( x, y, radius, _color );
    }

    /// <summary>
    /// Fills a circle with the center at x,y and a radius using the current color.
    /// </summary>
    /// <param name="x"> The x-coordinate of the center </param>
    /// <param name="y"> The y-coordinate of the center </param>
    /// <param name="radius"> The radius in pixels  </param>
    public void FillCircle( int x, int y, int radius )
    {
        gdx2DPixmap.FillCircle( x, y, radius, _color );
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
        gdx2DPixmap.FillTriangle( x1, y1, x2, y2, x3, y3, _color );
    }

    /// <summary>
    /// Returns the 32-bit RGBA8888 value of the pixel at x, y.
    /// For Alpha formats the RGB components will be one.
    /// </summary>
    /// <param name="x"> The x-coordinate </param>
    /// <param name="y"> The y-coordinate </param>
    /// <returns> The pixel color in RGBA8888 format.  </returns>
    public int GetPixel( int x, int y ) => gdx2DPixmap.GetPixel( x, y );

    /// <summary>
    /// Draws a pixel at the given location with the current color.
    /// </summary>
    /// <param name="x"> the x-coordinate </param>
    /// <param name="y"> the y-coordinate  </param>
    public void DrawPixel( int x, int y )
    {
        gdx2DPixmap.SetPixel( x, y, _color );
    }

    /// <summary>
    /// Draws a pixel at the given location with the given color.
    /// </summary>
    /// <param name="x"> the x-coordinate </param>
    /// <param name="y"> the y-coordinate </param>
    /// <param name="color"> the color in RGBA8888 format.  </param>
    public void DrawPixel( int x, int y, int color )
    {
        gdx2DPixmap.SetPixel( x, y, color );
    }

    /// <returns> The width of the Pixmap in pixels. </returns>
    public int Width => gdx2DPixmap.Width;

    /// <returns> The height of the Pixmap in pixels. </returns>
    public int Height => gdx2DPixmap.Height;

    /// <summary>
    /// Returns the OpenGL ES format of this Pixmap.
    /// </summary>
    /// <returns> one of GL_ALPHA, GL_RGB, GL_RGBA, GL_LUMINANCE, or GL_LUMINANCE_ALPHA.</returns>
    public PixelFormat GLFormat => gdx2DPixmap.GLFormat;

    /// <summary>
    /// Returns the OpenGL ES internal format of this Pixmap.
    /// </summary>
    /// <returns> one of GL_ALPHA, GL_RGB, GL_RGBA, GL_LUMINANCE, or GL_LUMINANCE_ALPHA.</returns>
    public InternalFormat GLInternalFormat => gdx2DPixmap.GLInternalFormat;

    /// <summary>
    /// Returns the OpenGL ES type of this Pixmap.
    /// </summary>
    /// <returns> one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_4_4_4_4 </returns>
    public PixelType GLType => gdx2DPixmap.GLType;

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
    public IntPtr Pixels
    {
        get
        {
            if ( isDisposed )
            {
                throw new GdxRuntimeException( "Pixmap already disposed" );
            }

            return gdx2DPixmap.pixelPtr;
        }
        set
        {
            ByteBuffer dst = gdx2DPixmap.pixelPtr;

            BufferUtils.Copy( value, dst, dst.Limit );
        }
    }

    /// <returns> the <see cref="Pixmap.Format"/> of this Pixmap. </returns>
    public Format GetFormat() => PixmapFormat.FromGdx2DPixmapFormat( gdx2DPixmap.format );

    // #############################################################

    /// <summary>
    /// Creates a Pixmap from a part of the current framebuffer.
    /// </summary>
    /// <param name="x">Framebuffer region x</param>
    /// <param name="y">Framebuffer region y</param>
    /// <param name="width">Framebuffer region width</param>
    /// <param name="height">Framebuffer region height</param>
    /// <returns>The new Pixmap.</returns>
    public static Pixmap CreateFromFrameBuffer( int x, int y, int width, int height )
    {
        Gdx.GL.GLPixelStorei( IGL20.GL_PACK_ALIGNMENT, 1 );

        Pixmap pixmap = new( width, height, Pixmap.Format.RGBA8888 );

        ByteBuffer pixels = pixmap.Pixels;

        Gdx.GL.GLReadPixels( x, y, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels );

        return pixmap;
    }
    
    public static Pixmap.Format FormatFromString( string str )
    {
        str = str.ToLower();

        return str switch
               {
                   "alpha"          => Pixmap.Format.Alpha,
                   "intensity"      => Pixmap.Format.Intensity,
                   "luminancealpha" => Pixmap.Format.LuminanceAlpha,
                   "rgb565"         => Pixmap.Format.RGB565,
                   "rgba4444"       => Pixmap.Format.RGBA4444,
                   "rgb888"         => Pixmap.Format.RGB888,
                   "rgba8888"       => Pixmap.Format.RGBA8888,
                   _                => throw new GdxRuntimeException( $"Unknown Format: {str}" )
               };
    }

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            gdx2DPixmap.Dispose();
            isDisposed = true;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( !isDisposed );
        GC.SuppressFinalize( this );
    }
}

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
        if ( format == Pixmap.Format.Alpha )
        {
            return Gdx2DPixmap.GDX_2D_FORMAT_ALPHA;
        }

        if ( format == Pixmap.Format.Intensity )
        {
            return Gdx2DPixmap.GDX_2D_FORMAT_ALPHA;
        }

        if ( format == Pixmap.Format.LuminanceAlpha )
        {
            return Gdx2DPixmap.GDX_2D_FORMAT_LUMINANCE_ALPHA;
        }

        if ( format == Pixmap.Format.RGB565 )
        {
            return Gdx2DPixmap.GDX_2D_FORMAT_RGB565;
        }

        if ( format == Pixmap.Format.RGBA4444 )
        {
            return Gdx2DPixmap.GDX_2D_FORMAT_RGBA4444;
        }

        if ( format == Pixmap.Format.RGB888 )
        {
            return Gdx2DPixmap.GDX_2D_FORMAT_RGB888;
        }

        if ( format == Pixmap.Format.RGBA8888 )
        {
            return Gdx2DPixmap.GDX_2D_FORMAT_RGBA8888;
        }

        throw new GdxRuntimeException( "Unknown Format: " + format );
    }
}
