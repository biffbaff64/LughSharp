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


using LughSharp.LibCore.Utils.Buffers;

namespace LughSharp.LibCore.Graphics.G2D;

[PublicAPI]
public class Gdx2DPixmap : IDisposable
{
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
        PixelPtr = LoadData( NativeData, encodedData, offset, len );

        BasePtr = NativeData[ 0 ];
        Width   = ( int )NativeData[ 1 ];
        Height  = ( int )NativeData[ 2 ];
        Format  = ( int )NativeData[ 3 ];

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
        MemoryStream memoryStream = new( 1024 );
        StreamWriter writer       = new( memoryStream );

        int readBytes;

        while ( ( readBytes = inStream.Read() ) != -1 )
        {
            writer.Write( readBytes );
        }

        var buffer = memoryStream.ToArray();

        PixelPtr = LoadData( NativeData, buffer, 0, buffer.Length );

        BasePtr = NativeData[ 0 ];
        Width   = ( int )NativeData[ 1 ];
        Height  = ( int )NativeData[ 2 ];
        Format  = ( int )NativeData[ 3 ];

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
        PixelPtr = GetNewPixmap( NativeData, width, height, format );

        if ( PixelPtr == null )
        {
            throw new GdxRuntimeException( $"Unable to allocate memory for pixmap: "
                                         + $"{width} x {height}: {GetFormatString( format )}" );
        }

        BasePtr = NativeData[ 0 ];
        Width   = ( int )NativeData[ 1 ];
        Height  = ( int )NativeData[ 2 ];
        Format  = ( int )NativeData[ 3 ];
    }

    /// <summary>
    /// </summary>
    /// <param name="pixelPtr"></param>
    /// <param name="nativeData"></param>
    public Gdx2DPixmap( ByteBuffer pixelPtr, long[] nativeData )
    {
        PixelPtr = pixelPtr;
        BasePtr  = nativeData[ 0 ];
        Width    = ( int )nativeData[ 1 ];
        Height   = ( int )nativeData[ 2 ];
        Format   = ( int )nativeData[ 3 ];
    }

    public long       BasePtr    { get; set; }                  // 
    public int        Format     { get; set; }                  // The actual pixmap format.
    public ByteBuffer PixelPtr   { get; set; }                  //
    public long[]     NativeData { get; set; } = new long[ 4 ]; //

    /// <summary>
    ///     Performs application-defined tasks associated with freeing,
    ///     releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
    }

    private ByteBuffer GetNewPixmap( long[] nativeData, int width, int height, int format )
    {
        return NewPixmap( nativeData, width, height, format );
    }

    private ByteBuffer LoadData( long[] nativeData, byte[] buffer, int offset, int len )
    {
        ByteBuffer ptr = Load( nativeData, buffer, offset, len );

        if ( ptr == null )
        {
            throw new IOException( $"Error loading pixmap: {GetFailureReason()}" );
        }

        return ptr;
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
                   GDX_2D_FORMAT_ALPHA           => IGL20.GL_ALPHA,
                   GDX_2D_FORMAT_LUMINANCE_ALPHA => IGL20.GL_LUMINANCE_ALPHA,
                   GDX_2D_FORMAT_RGB888          => IGL20.GL_RGB,
                   GDX_2D_FORMAT_RGB565          => IGL20.GL_RGB,
                   GDX_2D_FORMAT_RGBA8888        => IGL20.GL_RGBA,
                   GDX_2D_FORMAT_RGBA4444        => IGL20.GL_RGBA,
                   _                             => throw new GdxRuntimeException( $"unknown format: {format}" )
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
                   GDX_2D_FORMAT_ALPHA           => IGL20.GL_UNSIGNED_BYTE,
                   GDX_2D_FORMAT_LUMINANCE_ALPHA => IGL20.GL_UNSIGNED_BYTE,
                   GDX_2D_FORMAT_RGB888          => IGL20.GL_UNSIGNED_BYTE,
                   GDX_2D_FORMAT_RGBA8888        => IGL20.GL_UNSIGNED_BYTE,
                   GDX_2D_FORMAT_RGB565          => IGL20.GL_UNSIGNED_SHORT_5_6_5,
                   GDX_2D_FORMAT_RGBA4444        => IGL20.GL_UNSIGNED_SHORT_4_4_4_4,
                   _                             => throw new GdxRuntimeException( $"unknown format: {format}" )
               };
    }

    /// <summary>
    /// </summary>
    /// <param name="requestedFormat"></param>
    private void Convert( int requestedFormat )
    {
        Gdx2DPixmap pixmap = new( Width, Height, requestedFormat );

        pixmap.Blend = GDX_2D_BLEND_NONE;
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
        Clear( BasePtr, color );
    }

    public int GetPixel( int x, int y )
    {
        return GetPixel( BasePtr, x, y );
    }

    public void SetPixel( int x, int y, int color )
    {
        SetPixel( BasePtr, x, y, color );
    }

    public void DrawLine( int x, int y, int x2, int y2, int color )
    {
        DrawLine( BasePtr, x, y, x2, y2, color );
    }

    public void DrawRect( int x, int y, int width, int height, int color )
    {
        DrawRect( BasePtr, x, y, width, height, color );
    }

    public void DrawCircle( int x, int y, int radius, int color )
    {
        DrawCircle( BasePtr, x, y, radius, color );
    }

    public void FillRect( int x, int y, int width, int height, int color )
    {
        FillRect( BasePtr, x, y, width, height, color );
    }

    public void FillCircle( int x, int y, int radius, int color )
    {
        FillCircle( BasePtr, x, y, radius, color );
    }

    public void FillTriangle( int x1, int y1, int x2, int y2, int x3, int y3, int color )
    {
        FillTriangle( BasePtr, x1, y1, x2, y2, x3, y3, color );
    }

    public void DrawPixmap( Gdx2DPixmap src, int srcX, int srcY, int dstX, int dstY, int width, int height )
    {
        DrawPixmap( src.BasePtr, BasePtr, srcX, srcY, width, height, dstX, dstY, width, height );
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
        DrawPixmap( src.BasePtr, BasePtr, srcX, srcY, srcWidth, srcHeight, dstX, dstY, dstWidth, dstHeight );
    }

    public static Gdx2DPixmap? NewPixmap( StreamReader inStream, int requestedFormat )
    {
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

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    private static string GetFormatString( int format )
    {
        return format switch
               {
                   GDX_2D_FORMAT_ALPHA           => "Alpha",
                   GDX_2D_FORMAT_LUMINANCE_ALPHA => "Luminance alpha",
                   GDX_2D_FORMAT_RGB888          => "Rgb888",
                   GDX_2D_FORMAT_RGBA8888        => "Rgba8888",
                   GDX_2D_FORMAT_RGB565          => "Rgb565",
                   GDX_2D_FORMAT_RGBA4444        => "Rgba4444",
                   _                             => "Unknown"
               };
    }

    /// <summary>
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    // ########################################################################
    // Method stubs to be overridden in extending classes.
    // ########################################################################

    public virtual ByteBuffer Load( long[] nativeData, byte[] buffer, int offset, int len )
    {
        throw new NotImplementedException();
    }

    public virtual ByteBuffer NewPixmap( long[] nativeData, int width, int height, int format )
    {
        throw new NotImplementedException();
    }

    public virtual void Free( long pixmap )
    {
        throw new NotImplementedException();
    }

    public virtual void Clear( long pixmap, int color )
    {
        throw new NotImplementedException();
    }

    public virtual void SetPixel( long pixmap, int x, int y, int color )
    {
        throw new NotImplementedException();
    }

    public virtual int GetPixel( long pixmap, int x, int y )
    {
        throw new NotImplementedException();
    }

    public virtual void DrawLine( long pixmap, int x, int y, int x2, int y2, int color )
    {
        throw new NotImplementedException();
    }

    public virtual void DrawRect( long pixmap, int x, int y, int width, int height, int color )
    {
        throw new NotImplementedException();
    }

    public virtual void DrawCircle( long pixmap, int x, int y, int radius, int color )
    {
        throw new NotImplementedException();
    }

    public virtual void FillRect( long pixmap, int x, int y, int width, int height, int color )
    {
        throw new NotImplementedException();
    }

    public virtual void FillCircle( long pixmap, int x, int y, int radius, int color )
    {
        throw new NotImplementedException();
    }

    public virtual void FillTriangle( long pixmap, int x1, int y1, int x2, int y2, int x3, int y3, int color )
    {
        throw new NotImplementedException();
    }

    public virtual void DrawPixmap( long src,
                                    long dst,
                                    int srcX,
                                    int srcY,
                                    int srcWidth,
                                    int srcHeight,
                                    int dstX,
                                    int dstY,
                                    int dstWidth,
                                    int dstHeight )
    {
        throw new NotImplementedException();
    }

    public virtual void SetBlend( long src, int blend )
    {
        throw new NotImplementedException();
    }

    public virtual void SetScale( long src, int scale )
    {
        throw new NotImplementedException();
    }

    public virtual string GetFailureReason()
    {
        throw new NotImplementedException();
    }

    // ------------------------------------------------------------------------

    #region constants

    public const int GDX_2D_FORMAT_ALPHA           = 1;
    public const int GDX_2D_FORMAT_LUMINANCE_ALPHA = 2;
    public const int GDX_2D_FORMAT_RGB888          = 3;
    public const int GDX_2D_FORMAT_RGBA8888        = 4;
    public const int GDX_2D_FORMAT_RGB565          = 5;
    public const int GDX_2D_FORMAT_RGBA4444        = 6;
    public const int GDX_2D_SCALE_NEAREST          = 0;
    public const int GDX_2D_SCALE_LINEAR           = 1;
    public const int GDX_2D_BLEND_NONE             = 0;
    public const int GDX_2D_BLEND_SRC_OVER         = 1;

    #endregion constants

    // ------------------------------------------------------------------------

    #region properties

    public int Width  { get; set; }
    public int Height { get; set; }

    public int    GLInternalFormat => ToGLFormat( Format );
    public int    GLFormat         => ToGLFormat( Format );
    public int    GLType           => ToGLType( Format );
    public string FormatString     => GetFormatString( Format );

    public int Blend
    {
        set => SetBlend( BasePtr, value );
    }

    public int Scale
    {
        set => SetScale( BasePtr, value );
    }

    #endregion properties
}
