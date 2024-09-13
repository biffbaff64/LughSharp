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
using StbiSharp;

namespace LughSharp.LibCore.Graphics.G2D;

// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------

[PublicAPI]
public partial class Gdx2DPixmap : IDisposable
{
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

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

    public ByteBuffer?       PixmapBuffer { get; set; }
    public Gdx2dPixmapStruct PixmapDef    { get; set; }
    public uint              Width        { get; set; }
    public uint              Height       { get; set; }
    public uint              Format       { get; set; }

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

        LoadData( buffer, offset, len );

        PixmapBuffer = ByteBuffer.Wrap( PixmapDef.Pixels );

        if ( ( requestedFormat != 0 ) && ( requestedFormat != Format ) )
        {
            Convert( requestedFormat );
        }

        this.Width  = PixmapDef.Width;
        this.Height = PixmapDef.Height;
        this.Format = this.PixmapDef.Format;
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
    public Gdx2DPixmap( int width, int height, int format )
    {
        Logger.CheckPoint();

        this.PixmapDef = new Gdx2dPixmapStruct
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

    /// <summary>
    /// Loads the data in the supplied byte array into a <see cref="Gdx2dPixmapStruct"/>
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    /// <exception cref="IOException"></exception>
    private ByteBuffer LoadData( byte[] buffer, int offset, int len )
    {
        Logger.CheckPoint();

        var buf = new byte[ len ];

        //TODO: Once everything is working, implement a faster copy
        Array.Copy( buffer, offset, buf, 0, len );

        this.PixmapDef = Load( buf, len );

        var byteBuffer = ByteBuffer.Wrap( PixmapDef.Pixels );

        if ( byteBuffer == null )
        {
            throw new IOException( "Error loading pixmap" );
        }

        return byteBuffer;
    }

    private Gdx2dPixmapStruct Load( byte[] buffer, int len )
    {
        Logger.CheckPoint();

        var image = Stbi.LoadFromMemory( buffer, PixmapFormat.Gdx2dBytesPerPixel( ( int ) Format ) );

        var pixmapStruct = new Gdx2dPixmapStruct
        {
            Width  = ( uint ) image.Width,
            Height = ( uint ) image.Height,
            Format = ( uint ) image.NumChannels,
            Pixels = image.Data.ToArray()
        };

        return pixmapStruct;
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

        pixmap.SetBlend( GDX_2D_BLEND_NONE );
        pixmap.DrawPixmap( this, 0, 0, 0, 0, ( int ) Width, ( int ) Height );

//        Dispose();  // ??????

        this.Width        = pixmap.Width;
        this.Height       = pixmap.Height;
        this.Format       = pixmap.Format;
        this.PixmapDef    = pixmap.PixmapDef;
        this.PixmapBuffer = pixmap.PixmapBuffer;
    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="inStream"></param>
//    /// <param name="requestedFormat"></param>
//    /// <returns></returns>
//    public static Gdx2DPixmap? NewPixmap( StreamReader inStream, int requestedFormat )
//    {
//        Logger.CheckPoint();
//
//        try
//        {
//            return new Gdx2DPixmap( inStream, requestedFormat );
//        }
//        catch ( IOException e )
//        {
//            throw new GdxRuntimeException( e.Message );
//        }
//    }
//
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="width"></param>
//    /// <param name="height"></param>
//    /// <param name="format"></param>
//    /// <returns></returns>
//    /// <exception cref="GdxRuntimeException"></exception>
//    public static Gdx2DPixmap? NewPixmap( int width, int height, int format )
//    {
//        Logger.CheckPoint();
//
//        try
//        {
//            return new Gdx2DPixmap( width, height, format );
//        }
//        catch ( ArgumentException e )
//        {
//            throw new GdxRuntimeException( e.Message );
//        }
//    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Sets this pixmaps blending value.
    /// </summary>
    /// <param name="blend"></param>
    public void SetBlend( int blend )
    {
        this.PixmapDef = PixmapDef with
        {
            Blend = ( uint ) blend,
        };
    }

    //TODO: Why is this not a float?
    public void SetScale( int scale )
    {
        this.PixmapDef = PixmapDef with
        {
            Scale = ( uint ) scale,
        };
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
    // ------------------------------------------------------------------------
}