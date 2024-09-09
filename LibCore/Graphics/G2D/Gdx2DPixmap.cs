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

namespace LughSharp.LibCore.Graphics.G2D;

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------
// ----------------------------------------------------------------------------

[PublicAPI]
public partial class Gdx2DPixmap : IDisposable
{
    #region constants

    public const int GDX_2D_FORMAT_ALPHA           = 1;
    public const int GDX_2D_FORMAT_LUMINANCE_ALPHA = 2;
    public const int GDX_2D_FORMAT_RGB888          = 3;
    public const int GDX_2D_FORMAT_RGBA8888        = 4;
    public const int GDX_2D_FORMAT_RGB565          = 5;
    public const int GDX_2D_FORMAT_RGBA4444        = 6;

    public const int GDX_2D_SCALE_NEAREST  = 0;
    public const int GDX_2D_SCALE_LINEAR   = 1;
    public const int GDX_2D_SCALE_BILINEAR = 1;
    public const int GDX_2D_BLEND_NONE     = 0;
    public const int GDX_2D_BLEND_SRC_OVER = 1;

    public const int DEFAULT_FORMAT = GDX_2D_FORMAT_RGBA8888;
    public const int DEFAULT_BLEND  = GDX_2D_BLEND_SRC_OVER;
    public const int DEFAULT_SCALE  = GDX_2D_SCALE_BILINEAR;

    #endregion constants

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region properties

    public ByteBuffer   PixmapBuffer { get; set; }
    public gdx2d_pixmap PixmapDef    { get; set; }
    public uint         Width        { get; set; }
    public uint         Height       { get; set; }
    public uint         Format       { get; set; }

    #endregion properties

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region constructors

    /// <summary>
    /// Creates a new Gdx2DPixmap instance.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <param name="requestedFormat"></param>
    /// <exception cref="IOException"></exception>
    public Gdx2DPixmap( byte[] buffer, int offset, int len, int requestedFormat )
    {
        Logger.CheckPoint();
        Logger.Debug( $"buffer length  : {buffer.Length}" );
        Logger.Debug( $"offset         : {offset}" );
        Logger.Debug( $"requestedFormat: {requestedFormat}" );

        LoadData( buffer, offset, len );

//        this.PixmapDef = new PixmapDef
//        {
//            //Note:
//            // Members width, height, and format need to be set
//            // correctly when gdx2d_load is working correctly
//            Width  = 165,
//            Height = 111,
//            Format = ( uint ) ( ( uint ) requestedFormat == 0 ? GDX_2D_FORMAT_RGBA8888 : requestedFormat ),
//            Scale  = DEFAULT_SCALE,
//            Blend  = DEFAULT_BLEND,
//            Pixels = buffer,
//        };

        Logger.CheckPoint();
        Logger.Debug( $"NPixmapDef       : {PixmapDef.Width} x {PixmapDef.Height}" );
        Logger.Debug( $"NPixmapDef.Format: {PixmapDef.Format}" );

        PixmapBuffer = ByteBuffer.Wrap( PixmapDef.Pixels );

        Logger.CheckPoint();

        if ( ( requestedFormat != 0 ) && ( requestedFormat != Format ) )
        {
            Convert( requestedFormat );
        }

        this.Width  = PixmapDef.Width;
        this.Height = PixmapDef.Height;
        this.Format = this.PixmapDef.Format;

        Logger.CheckPoint();
    }

    /// <summary>
    /// </summary>
    /// <param name="encodedData"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <param name="requestedFormat"></param>
    public Gdx2DPixmap( ByteBuffer encodedData, int offset, int len, int requestedFormat )
    {
        //TODO:
    }

    /// <summary>
    /// </summary>
    /// <param name="inStream"></param>
    /// <param name="requestedFormat"></param>
    /// <exception cref="IOException"></exception>
    public Gdx2DPixmap( StreamReader inStream, int requestedFormat )
    {
        Logger.CheckPoint();

        MemoryStream memoryStream = new( 1024 );
        StreamWriter writer       = new( memoryStream );

        int readBytes;

        while ( ( readBytes = inStream.Read() ) != -1 )
        {
            writer.Write( readBytes );
        }

        var buffer = memoryStream.ToArray();

        LoadData( buffer, 0, buffer.Length );

        if ( ( requestedFormat != 0 ) && ( requestedFormat != Format ) )
        {
            Convert( requestedFormat );
        }

        this.Width  = PixmapDef.Width;
        this.Height = PixmapDef.Height;
        this.Format = this.PixmapDef.Format;

        PixmapBuffer = ByteBuffer.Wrap( PixmapDef.Pixels );
    }

    /// <summary>
    /// Creates a new Gdx2DPixmap object with the given width, height, and pixel format.
    /// </summary>
    /// <param name="width"> Width in pixels. </param>
    /// <param name="height"> Height in pixels. </param>
    /// <param name="format"> The requested GDX_2D_FORMAT_xxx color format. </param>
    /// <exception cref="GdxRuntimeException"></exception>
    public unsafe Gdx2DPixmap( int width, int height, int format )
    {
        Logger.CheckPoint();
        Logger.Debug( $"width: {width}, height: {height}, format: {format}" );

        this.PixmapDef = new gdx2d_pixmap
        {
            Width  = ( uint ) width,
            Height = ( uint ) height,
            Format = ( uint ) format,
            Blend  = GDX_2D_BLEND_SRC_OVER,
            Scale  = GDX_2D_SCALE_BILINEAR,
            Pixels = new byte[ width * height * PixmapFormat.Gdx2dBytesPerPixel( format ) ]
        };

        PixmapBuffer = ByteBuffer.Wrap( PixmapDef.Pixels );

        this.Width  = this.PixmapDef.Width;
        this.Height = this.PixmapDef.Height;
        this.Format = this.PixmapDef.Format;

        if ( PixmapBuffer == null )
        {
            throw new GdxRuntimeException( $"Unable to allocate memory for pixmap: "
                                         + $"{width} x {height}: {PixmapFormat.GetFormatString( format )}" );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="pixelPtr"></param>
    /// <param name="data"></param>
    public Gdx2DPixmap( ByteBuffer pixelPtr, byte[] data )
    {
        Logger.CheckPoint();

        //TODO:
    }

    #endregion constructors

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private ByteBuffer LoadData( byte[] buffer, int offset, int len )
    {
        Logger.CheckPoint();

        var buf = new byte[ len ];

        Array.Copy( buffer, offset, buf, 0, len );

        this.PixmapDef = gdx2d_load( buf, len );

        Logger.CheckPoint();

        var byteBuffer = ByteBuffer.Wrap( PixmapDef.Pixels );

        if ( byteBuffer == null )
        {
            throw new IOException( "Error loading pixmap" );
        }

        return byteBuffer;
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGLFormat( int format )
    {
        return format switch
        {
            GDX_2D_FORMAT_ALPHA           => IGL.GL_ALPHA,
            GDX_2D_FORMAT_LUMINANCE_ALPHA => IGL.GL_LUMINANCE_ALPHA,
            GDX_2D_FORMAT_RGB888          => IGL.GL_RGB,
            GDX_2D_FORMAT_RGB565          => IGL.GL_RGB,
            GDX_2D_FORMAT_RGBA8888        => IGL.GL_RGBA,
            GDX_2D_FORMAT_RGBA4444        => IGL.GL_RGBA,
            var _                         => throw new GdxRuntimeException( $"unknown format: {format}" )
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGLType( int format )
    {
        return format switch
        {
            GDX_2D_FORMAT_ALPHA           => IGL.GL_UNSIGNED_BYTE,
            GDX_2D_FORMAT_LUMINANCE_ALPHA => IGL.GL_UNSIGNED_BYTE,
            GDX_2D_FORMAT_RGB888          => IGL.GL_UNSIGNED_BYTE,
            GDX_2D_FORMAT_RGBA8888        => IGL.GL_UNSIGNED_BYTE,
            GDX_2D_FORMAT_RGB565          => IGL.GL_UNSIGNED_SHORT_5_6_5,
            GDX_2D_FORMAT_RGBA4444        => IGL.GL_UNSIGNED_SHORT_4_4_4_4,
            var _                         => throw new GdxRuntimeException( $"unknown format: {format}" )
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="requestedFormat"></param>
    private void Convert( int requestedFormat )
    {
        Logger.CheckPoint();

        var pixmap = new Gdx2DPixmap( ( int ) Width, ( int ) Height, requestedFormat );

        pixmap.Blend = GDX_2D_BLEND_NONE;
        pixmap.DrawPixmap( this, 0, 0, 0, 0, ( int ) Width, ( int ) Height );

        Dispose();

        this.Width        = pixmap.Width;
        this.Height       = pixmap.Height;
        this.Format       = pixmap.Format;
        this.PixmapDef    = pixmap.PixmapDef;
        this.PixmapBuffer = pixmap.PixmapBuffer;
    }

    public static Gdx2DPixmap? NewPixmap( StreamReader inStream, int requestedFormat )
    {
        Logger.CheckPoint();

        try
        {
            return new Gdx2DPixmap( inStream, requestedFormat );
        }
        catch ( IOException e )
        {
            Logger.Error( e.Message );

            return null;
        }
    }

    public static Gdx2DPixmap? NewPixmap( int width, int height, int format )
    {
        Logger.CheckPoint();

        try
        {
            return new Gdx2DPixmap( width, height, format );
        }
        catch ( ArgumentException e )
        {
            Logger.Error( e.Message );

            return null;
        }
    }

    // ------------------------------------------------------------------------

    public int Blend
    {
        set => gdx2d_set_blend( PixmapDef, value );
    }

    public int Scale
    {
        set => gdx2d_set_scale( PixmapDef, value );
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
    }

    /// <summary>
    /// Centralise all logic related to releasing unmanaged resources.
    /// </summary>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    // ------------------------------------------------------------------------
}