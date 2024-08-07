﻿// ///////////////////////////////////////////////////////////////////////////////
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

[PublicAPI]
public class Gdx2DPixmap : IDisposable
{
    #region constants

    public const int GDX_2D_FORMAT_ALPHA           = 1;
    public const int GDX_2D_FORMAT_LUMINANCE_ALPHA = 2;
    public const int GDX_2D_FORMAT_RGB888          = 3;
    public const int GDX_2D_FORMAT_RGBA8888        = 4;
    public const int GDX_2D_FORMAT_RGB565          = 5;
    public const int GDX_2D_FORMAT_RGBA4444        = 6;
    public const int GDX_2D_SCALE_NEAREST          = 0;
    public const int GDX_2D_SCALE_LINEAR           = 1;
    public const int GDX_2D_SCALE_BILINEAR         = 1;
    public const int GDX_2D_BLEND_NONE             = 0;
    public const int GDX_2D_BLEND_SRC_OVER         = 1;

    #endregion constants

    // ------------------------------------------------------------------------

    #region properties

    public long       BasePtr    { get; set; }                  // 
    public int        Format     { get; set; }                  // The actual pixmap format.
    public ByteBuffer PixelPtr   { get; set; }                  //
    public long[]     NativeData { get; set; } = new long[ 4 ]; //
    public int        Width      { get; set; }                  //
    public int        Height     { get; set; }                  //

    #endregion properties

    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="encodedData"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    /// <param name="requestedFormat"></param>
    /// <exception cref="IOException"></exception>
    public Gdx2DPixmap( byte[] encodedData, int offset, int len, int requestedFormat )
    {
        Logger.CheckPoint();

        PixelPtr = LoadData( encodedData, offset, len );

        BasePtr = NativeData[ 0 ];
        Width   = ( int ) NativeData[ 1 ];
        Height  = ( int ) NativeData[ 2 ];
        Format  = ( int ) NativeData[ 3 ];

        if ( ( requestedFormat != 0 ) && ( requestedFormat != Format ) )
        {
            Convert( requestedFormat );
        }
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

        PixelPtr = LoadData( buffer, 0, buffer.Length );

        BasePtr = NativeData[ 0 ];
        Width   = ( int ) NativeData[ 1 ];
        Height  = ( int ) NativeData[ 2 ];
        Format  = ( int ) NativeData[ 3 ];

        if ( ( requestedFormat != 0 ) && ( requestedFormat != Format ) )
        {
            Convert( requestedFormat );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="format"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public Gdx2DPixmap( int width, int height, int format )
    {
        Logger.CheckPoint();

        PixelPtr = GetNewPixmap( width, height, format );

        if ( PixelPtr == null )
        {
            throw new GdxRuntimeException( $"Unable to allocate memory for pixmap: "
                                         + $"{width} x {height}: {PixmapFormat.GetFormatString( format )}" );
        }

        BasePtr = NativeData[ 0 ];
        Width   = ( int ) NativeData[ 1 ];
        Height  = ( int ) NativeData[ 2 ];
        Format  = ( int ) NativeData[ 3 ];
    }

    /// <summary>
    /// </summary>
    /// <param name="pixelPtr"></param>
    /// <param name="nativeData"></param>
    public Gdx2DPixmap( ByteBuffer pixelPtr, long[] nativeData )
    {
        Logger.CheckPoint();

        PixelPtr = pixelPtr;
        BasePtr  = nativeData[ 0 ];
        Width    = ( int ) nativeData[ 1 ];
        Height   = ( int ) nativeData[ 2 ];
        Format   = ( int ) nativeData[ 3 ];
    }

    private ByteBuffer GetNewPixmap( int width, int height, int format )
    {
        Logger.CheckPoint();

        var pixmap = new Gdx2DUtils.Gdx2dPixmap
        {
            Width  = ( uint ) width,
            Height = ( uint ) height,
            Format = ( uint ) format,
            Blend  = GDX_2D_BLEND_SRC_OVER,
            Scale  = GDX_2D_SCALE_BILINEAR,
            Pixels = new byte[ width * height * PixmapFormat.Gdx2dBytesPerPixel( format ) ]
        };

        return BufferUtils.NewByteBuffer( pixmap.Pixels.Length );
    }

    /// <summary>
    /// Created because <see cref="Load"/>, being a virtual method, is unsafe
    /// to be called from constructors.
    /// </summary>
    private ByteBuffer LoadData( byte[] buffer, int offset, int len )
    {
        Logger.CheckPoint();

//        var ptr = Load( nativeData, buffer, offset, len );

//        if ( ptr == null )
        {
            throw new IOException( $"Error loading pixmap: {Gdx2DUtils.Gdx2dGetFailureReason()}" );
        }

//        return ptr;
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

        Gdx2DPixmap pixmap = new( Width, Height, requestedFormat );

//        pixmap.Blend = GDX_2D_BLEND_NONE;
        pixmap.DrawPixmap( this, 0, 0, 0, 0, Width, Height );

        Dispose();

        BasePtr    = pixmap.BasePtr;
        Format     = pixmap.Format;
        Height     = pixmap.Height;
        NativeData = pixmap.NativeData;
        PixelPtr   = pixmap.PixelPtr;
        Width      = pixmap.Width;
    }

    public void Clear( int color )
    {
//        Clear( BasePtr, color );
    }

    public int GetPixel( int x, int y )
    {
//        return GetPixel( BasePtr, x, y );
        return 0;
    }

    public void SetPixel( int x, int y, int color )
    {
//        SetPixel( BasePtr, x, y, color );
    }

    public void DrawLine( int x, int y, int x2, int y2, int color )
    {
//        DrawLine( BasePtr, x, y, x2, y2, color );
    }

    public void DrawRect( int x, int y, int width, int height, int color )
    {
//        DrawRect( BasePtr, x, y, width, height, color );
    }

    public void DrawCircle( int x, int y, int radius, int color )
    {
//        DrawCircle( BasePtr, x, y, radius, color );
    }

    public void FillRect( int x, int y, int width, int height, int color )
    {
//        FillRect( BasePtr, x, y, width, height, color );
    }

    public void FillCircle( int x, int y, int radius, int color )
    {
//        FillCircle( BasePtr, x, y, radius, color );
    }

    public void FillTriangle( int x1, int y1, int x2, int y2, int x3, int y3, int color )
    {
//        FillTriangle( BasePtr, x1, y1, x2, y2, x3, y3, color );
    }

    public void DrawPixmap( Gdx2DPixmap src, int srcX, int srcY, int dstX, int dstY, int width, int height )
    {
//        DrawPixmap( src.BasePtr, BasePtr, srcX, srcY, width, height, dstX, dstY, width, height );
    }

    public void DrawPixmap( Gdx2DPixmap src,
                            int srcX,
                            int srcY,
                            int srcWidth,
                            int srcHeight,
                            int dstX,
                            int dstY,
                            int dstWidth,
                            int dstHeight )
    {
//        DrawPixmap( src, srcX, srcY, srcWidth, srcHeight, dstX, dstY, dstWidth, dstHeight );
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

    /// <summary>
    /// Creates a new Gdx2DPixmap.
    /// </summary>
    /// <param name="width"> The width. </param>
    /// <param name="height"> The height. </param>
    /// <param name="format"> The pixmap format. </param>
    /// <returns></returns>
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

    public int    GLInternalFormat => ToGLFormat( Format );
    public int    GLFormat         => ToGLFormat( Format );
    public int    GLType           => ToGLType( Format );
    public string FormatString     => PixmapFormat.GetFormatString( Format );

//    public int Blend
//    {
//        set => Gdx2DUtils.Gdx2dSetBlend( BasePtr, value );
//    }

//    public int Scale
//    {
//        set => Gdx2DUtils.Gdx2dSetScale( BasePtr, value );
//    }


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
    /// Centralize all logic related to releasing unmanaged resources.
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
