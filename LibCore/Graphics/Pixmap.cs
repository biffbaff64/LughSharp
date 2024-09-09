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


using LughSharp.LibCore.Utils.Exceptions;
using Exception = System.Exception;

namespace LughSharp.LibCore.Graphics;

/// <summary>
/// A Pixmap represents an image in memory. It has a width and height expressed
/// in pixels as well as a <see cref="ColorFormat"/> specifying the number and order
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
/// will scale and stretch the source image to a target image. In this case either nearest
/// neighbour or bilinear filtering can be used.
/// </para>
/// </summary>
[PublicAPI]
public class Pixmap : IDisposable
{
    public bool        IsDisposed { get; set; } = false;       // 
    public int         Scale      { get; set; }                // 
    public Color       Color      { get; set; } = Color.Clear; // 
    public Gdx2DPixmap PixelData  { get; set; }                // 

    /// <summary>
    /// Sets the type of <see cref="BlendTypes"/> to be used for all operations.
    /// Default is <see cref="BlendTypes.SourceOver"/>.
    /// </summary>
    public BlendTypes Blending { get; set; } = BlendTypes.SourceOver;

    // ------------------------------------------------------------------------

    private Filter _filter = Filter.BiLinear;
    private int    _format;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new Pixmap instance with the given width, height and format.
    /// </summary>
    /// <param name="width">The width in pixels.</param>
    /// <param name="height">The height in pixels.</param>
    /// <param name="format">The <see cref="ColorFormat"/></param>
    public Pixmap( int width, int height, Pixmap.ColorFormat? format )
    {
        Logger.CheckPoint();
        Logger.Debug( $"width: {width}, height: {height}, format: {format}" );

        PixelData = new Gdx2DPixmap( width, height, PixmapFormat.ToGdx2DPixmapFormat( format ) );

        this.Width  = ( int ) PixelData.Width;
        this.Height = ( int ) PixelData.Height;
        this.Format = PixmapFormat.FromGdx2DPixmapFormat( ( int ) PixelData.Format );

        Logger.Debug( $"Width: {this.Width}, Height: {this.Height}, Format: {this.Format}" );

        SetColor( Graphics.Color.Red );
        FillWithCurrentColor();

        Logger.CheckPoint();
    }

    /// <summary>
    /// Creates a new Pixmap instance from the given encoded image data.
    /// The image can be encoded as JPEG, PNG or BMP.
    /// </summary>
    public Pixmap( byte[] encodedData, int offset, int len )
    {
        Logger.CheckPoint();
        Logger.Debug( $"encodedData: {encodedData}, offset: {offset}, len: {len}" );

        try
        {
            PixelData = new Gdx2DPixmap( encodedData, offset, len, 0 );

            Width       = ( int ) PixelData.Width;
            Height      = ( int ) PixelData.Height;
            this.Format = PixmapFormat.FromGdx2DPixmapFormat( ( int ) PixelData.Format );
        }
        catch ( IOException e )
        {
            throw new GdxRuntimeException( "Couldn't load pixmap from image data", e );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="encodedData"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public Pixmap( ByteBuffer encodedData, int offset, int len )
    {
        if ( !encodedData.IsDirect() )
        {
            throw new GdxRuntimeException( "Couldn't load pixmap from non-direct ByteBuffer" );
        }

        try
        {
            PixelData = new Gdx2DPixmap( encodedData, offset, len, 0 );

            Width       = ( int ) PixelData.Width;
            Height      = ( int ) PixelData.Height;
            this.Format = PixmapFormat.FromGdx2DPixmapFormat( ( int ) PixelData.Format );
        }
        catch ( IOException e )
        {
            throw new GdxRuntimeException( "Couldn't load pixmap from image data", e );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="encodedData"></param>
    public Pixmap( ByteBuffer encodedData )
        : this( encodedData, encodedData.Position, encodedData.Remaining() )
    {
    }

    /// <summary>
    /// Creates a new Pixmap instance from the given file. The file must be a Png,
    /// Jpeg or Bitmap. Paletted formats are not supported.
    /// </summary>
    /// <param name="file"> The file. </param>
    /// <exception cref="GdxRuntimeException">
    /// Thrown if there were errors loading or reading the file.
    /// </exception>
    public Pixmap( FileInfo file )
    {
        Logger.CheckPoint();
        Logger.Debug( $"File: {file.FullName}" );
        if ( File.Exists( file.FullName ) )
        {
            Logger.Debug( "Image file found" );
        }

        try
        {
            Logger.CheckPoint();

            var data = File.ReadAllBytes( file.FullName );

            Logger.CheckPoint();

            PixelData = new Gdx2DPixmap( data, 0, data.Length, 0 );

            Logger.CheckPoint();

            Width  = ( int ) PixelData.Width;
            Height = ( int ) PixelData.Height;
            Logger.CheckPoint();
            this.Format = PixmapFormat.FromGdx2DPixmapFormat( ( int ) PixelData.Format );

            Logger.CheckPoint();
            Logger.Debug( $"{PixelData.Width} x {PixelData.Height}, {PixelData.Format}" );
        }
        catch ( Exception e )
        {
            throw new GdxRuntimeException( $"Couldn't load file:  {file.FullName}", e );
        }

        Logger.CheckPoint();
    }

    /// <summary>
    /// </summary>
    /// <param name="gdx2DPixmap"></param>
    public Pixmap( Gdx2DPixmap gdx2DPixmap )
    {
        this.PixelData = gdx2DPixmap;

        Width       = ( int ) PixelData.Width;
        Height      = ( int ) PixelData.Height;
        this.Format = PixmapFormat.FromGdx2DPixmapFormat( ( int ) PixelData.Format );
    }

    // ----------------------------------------------------------

    /// <summary>
    /// Returns the width of the Pixmap in pixels.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Returns the height of the Pixmap in pixels.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Returns the OpenGL ES format of this Pixmap.
    /// </summary>
    /// <returns> one of GL_ALPHA, GL_RGB, GL_RGBA, GL_LUMINANCE, or GL_LUMINANCE_ALPHA.</returns>
    public int GLFormat => ( int ) PixelData.Format;

    /// <summary>
    /// Returns the OpenGL ES internal format of this Pixmap.
    /// </summary>
    /// <returns> one of GL_ALPHA, GL_RGB, GL_RGBA, GL_LUMINANCE, or GL_LUMINANCE_ALPHA.</returns>
    public int GLInternalFormat => Gdx2DPixmap.ToGLFormat( ( int ) PixelData.Format );

    /// <summary>
    /// Returns the OpenGL ES type of this Pixmap.
    /// </summary>
    /// <returns> one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT_5_6_5, GL_UNSIGNED_SHORT_4_4_4_4 </returns>
    public int GLType => Gdx2DPixmap.ToGLType( ( int ) PixelData.Format );

    /// <summary>
    /// Returns the byte[] array holding the pixel data. For the format Alpha each
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
    public ByteBuffer? Pixels
    {
        get
        {
            if ( IsDisposed )
            {
                throw new GdxRuntimeException( "Pixmap already disposed" );
            }

            return PixelData.PixmapBuffer;
        }

        set => PixelData.PixmapBuffer = value!;
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
    public void SetColor( Color color )
    {
        this.Color = color;
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
        this.Color = new Color( r, g, b, a );
    }

    /// <summary>
    /// Fills the complete bitmap with the currently set color.
    /// </summary>
    public void FillWithCurrentColor()
    {
        PixelData.Clear( PixelData.PixmapDef, this.Color );
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
        PixelData.DrawLine( x, y, x2, y2, this.Color );
    }

    /// <summary>
    /// Draws a rectangle outline starting at x, y extending by width to the right
    /// and by height downwards (y-axis points downwards) using the current color.
    /// </summary>
    /// <param name="x"> The x coordinate </param>
    /// <param name="y"> The y coordinate </param>
    /// <param name="width"> The width in pixels </param>
    /// <param name="height"> The height in pixels  </param>
    public void DrawRectangle( int x, int y, uint width, uint height )
    {
        PixelData.DrawRect( x, y, width, height, this.Color );
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
            PixelData.DrawPixmap( pixmap.PixelData, srcx, srcy, x, y, srcWidth, srcHeight );
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
            PixelData.DrawPixmap( pixmap.PixelData, srcx, srcy, srcWidth, srcHeight, dstx, dsty, dstWidth, dstHeight );
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
    public void FillRectangle( int x, int y, uint width, uint height )
    {
        PixelData.FillRect( x, y, width, height, this.Color );
    }

    /// <summary>
    /// Draws a circle outline with the center at x,y and a radius using the
    /// current color and stroke width.
    /// </summary>
    /// <param name="x"> The x-coordinate of the center </param>
    /// <param name="y"> The y-coordinate of the center </param>
    /// <param name="radius"> The radius in pixels  </param>
    public void DrawCircle( int x, int y, uint radius )
    {
        PixelData.DrawCircle( x, y, radius, this.Color );
    }

    /// <summary>
    /// Fills a circle with the center at x,y and a radius using the current color.
    /// </summary>
    /// <param name="x"> The x-coordinate of the center </param>
    /// <param name="y"> The y-coordinate of the center </param>
    /// <param name="radius"> The radius in pixels </param>
    public void FillCircle( int x, int y, uint radius )
    {
        PixelData.FillCircle( x, y, radius, this.Color );
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
        PixelData.FillTriangle( x1, y1, x2, y2, x3, y3, this.Color );
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
        return PixelData.GetPixel( PixelData.PixmapDef, x, y );
    }

    /// <summary>
    /// Draws a pixel at the given location with the current color.
    /// </summary>
    /// <param name="x"> the x-coordinate </param>
    /// <param name="y"> the y-coordinate </param>
    public void DrawPixel( int x, int y )
    {
        PixelData.SetPixel( PixelData.PixmapDef, x, y, this.Color );
    }

    /// <summary>
    /// Draws a pixel at the given location with the given color.
    /// </summary>
    /// <param name="x"> the x-coordinate </param>
    /// <param name="y"> the y-coordinate </param>
    /// <param name="color"> the color in RGBA8888 format. </param>
    public void DrawPixel( int x, int y, Color color )
    {
        PixelData.SetPixel( PixelData.PixmapDef, x, y, color );
    }

    /// <summary>
    /// Returns the <see cref="ColorFormat"/> of this Pixmap.
    /// </summary>
    public Pixmap.ColorFormat? Format
    {
        get => PixmapFormat.FromGdx2DPixmapFormat( _format != 0
                                                       ? _format
                                                       : PixmapFormat.GDX_2D_FORMAT_RGB888 );
        set => _format = PixmapFormat.ToGdx2DPixmapFormat( value );
    }

    /// <summary>
    /// Creates a Pixmap from a part of the current framebuffer.
    /// </summary>
    /// <param name="x"> Framebuffer region x </param>
    /// <param name="y"> Framebuffer region y </param>
    /// <param name="width"> Framebuffer region width </param>
    /// <param name="height"> Framebuffer region height </param>
    /// <returns>The new Pixmap.</returns>
    public static unsafe Pixmap CreateFromFrameBuffer( int x, int y, int width, int height )
    {
        Gdx.GL.glPixelStorei( IGL.GL_PACK_ALIGNMENT, 1 );

        Pixmap pixmap = new( width, height, Pixmap.ColorFormat.RGBA8888 );

        fixed ( void* ptr = &pixmap.Pixels!.BackingArray()[ 0 ] )
        {
            Gdx.GL.glReadPixels( x, y, width, height, IGL.GL_RGBA, IGL.GL_UNSIGNED_BYTE, ptr );
        }

        return pixmap;
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

            Scale = _filter == Filter.NearestNeighbour
                        ? Gdx2DPixmap.GDX_2D_SCALE_NEAREST
                        : Gdx2DPixmap.GDX_2D_SCALE_LINEAR;
        }
    }

    /// <summary>
    /// Returns the pixel format from a valid named string.
    /// </summary>
    public static ColorFormat FormatFromString( string str )
    {
        str = str.ToLower();

        return str switch
        {
            "alpha"          => ColorFormat.Alpha,
            "intensity"      => ColorFormat.Intensity,
            "luminancealpha" => ColorFormat.LuminanceAlpha,
            "rgb565"         => ColorFormat.RGB565,
            "rgba4444"       => ColorFormat.RGBA4444,
            "rgb888"         => ColorFormat.RGB888,
            "rgba8888"       => ColorFormat.RGBA8888,
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
            //TODO:
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
    public enum ColorFormat
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
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------