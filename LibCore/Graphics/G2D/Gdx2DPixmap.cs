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

using LughSharp.LibCore.Utils.Buffers.HeapBuffers;
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

    public const int DEFAULT_FORMAT = GDX_2D_FORMAT_RGBA8888;
    public const int DEFAULT_BLEND  = GDX_2D_BLEND_SRC_OVER;
    public const int DEFAULT_SCALE  = GDX_2D_SCALE_BILINEAR;

    #endregion constants

    // ------------------------------------------------------------------------

    #region properties

    public ByteBuffer? PixelPtr { get; set; }

    public PixmapDef PixmapDef { get; set; }

//    public byte[]      NativeData { get; set; } = new byte[ 4 ];
    public int  Width   { get; set; }
    public int  Height  { get; set; }
    public int  Format  { get; set; }
    public long BasePtr { get; set; }

    #endregion properties

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
        Logger.Debug( $"buffer length: {buffer.Length}" );
        Logger.Debug( $"offset: {offset}" );
        Logger.Debug( $"requestedFormat: {requestedFormat}" );

        LoadData( buffer, offset, buffer.Length );

        Logger.CheckPoint();

        if ( ( requestedFormat != 0 ) && ( requestedFormat != Format ) )
        {
            Convert( requestedFormat );
        }

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
        Logger.Debug( $"width: {width}, height: {height}, format: {format}" );

        this.PixmapDef = Gdx2DNew( width, height, format );

        Logger.CheckPoint();

        PixelPtr = ByteBuffer.Wrap( this.PixmapDef.Pixels );

        this.Width  = this.PixmapDef.Width;
        this.Height = this.PixmapDef.Height;
        this.Format = this.PixmapDef.Format;

        if ( PixelPtr == null )
        {
            throw new GdxRuntimeException( $"Unable to allocate memory for pixmap: "
                                         + $"{width} x {height}: {PixmapFormat.GetFormatString( format )}" );
        }

        Logger.CheckPoint();
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

    #endregion constructors

    private ByteBuffer LoadData( byte[] buffer, int offset, int len )
    {
        Logger.CheckPoint();
        Logger.Debug( $"buffer length: {buffer.Length}" );
        Logger.Debug( $"offset       : {offset}" );
        Logger.Debug( $"len          : {len}" );

        var ptr = Load( buffer, offset, len );

        Logger.CheckPoint();

//        Logger.Debug( $"NativeData: {NativeData[ 0 ]}, {NativeData[ 1 ]}, {NativeData[ 2 ]}, {NativeData[ 3 ]}" );

        if ( PixelPtr == null )
        {
            Logger.CheckPoint();

            throw new IOException( "Error loading pixmap" );
        }

        Logger.CheckPoint();

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

        var pixmap = new Gdx2DPixmap( Width, Height, requestedFormat );

        pixmap.Blend = GDX_2D_BLEND_NONE;
        pixmap.DrawPixmap( this, 0, 0, 0, 0, Width, Height );

        Dispose();

        this.BasePtr   = pixmap.BasePtr;
        this.Format    = pixmap.Format;
        this.Height    = pixmap.Height;
        this.PixmapDef = pixmap.PixmapDef;

//        this.NativeData = pixmap.NativeData;
        this.PixelPtr = pixmap.PixelPtr;
        this.Width    = pixmap.Width;
    }

    public void Clear( PixmapDef pd, int color )
    {
        gdx2d_clear( pd, color );
    }

    public int GetPixel( PixmapDef pd, int x, int y )
    {
        return gdx2d_get_pixel( pd, x, y );
    }

    public void SetPixel( PixmapDef pd, int x, int y, int color )
    {
        gdx2d_set_pixel( pd, x, y, color );
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
//        gdx_draw_pixmap( src, srcX, srcY, srcWidth, srcHeight, dstX, dstY, dstWidth, dstHeight );
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

    private PixmapDef Gdx2DNew( int width, int height, int format )
    {
        var pmd = new PixmapDef
        {
            Width  = width,
            Height = height,
            Format = format,
            Blend  = GDX_2D_BLEND_SRC_OVER,
            Scale  = GDX_2D_SCALE_BILINEAR
        };

        pmd.Pixels = new byte[ pmd.Width * pmd.Height * PixmapFormat.Gdx2dBytesPerPixel( format ) ];

        return pmd;
    }

    // ------------------------------------------------------------------------

    public int    GLInternalFormat => ToGLFormat( Format );
    public int    GLFormat         => ToGLFormat( Format );
    public int    GLType           => ToGLType( Format );
    public string FormatString     => PixmapFormat.GetFormatString( Format );

    public int Blend { get; set; }

//TODO:
//    {
//        set => gdx2d_set_blend( Def, value );
//    }

    public int Scale { get; set; }

//TODO:
//    {
//        set => gdx2d_set_scale( Def, value );
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

    private ByteBuffer Load( byte[] buffer, int offset, int len )
    {
        return new HeapByteBuffer( buffer, offset, len );
    }

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern ByteBuffer gdx2d_load( byte[] nativeData, byte[] buffer, int offset, int len );
/*
    const unsigned char* p_buffer = (const unsigned char*)env->GetPrimitiveArrayCritical(buffer, 0);
    gdx2d_pixmap* pixmap = gdx2d_load(p_buffer + offset, len);
    env->ReleasePrimitiveArrayCritical(buffer, (char*)p_buffer, 0);

    if(pixmap==0)
        return 0;

    jobject pixel_buffer = env->NewDirectByteBuffer((void*)pixmap->pixels, pixmap->width * pixmap->height * gdx2d_bytes_per_pixel(pixmap->format));
    jlong* p_native_data = (jlong*)env->GetPrimitiveArrayCritical(nativeData, 0);
    p_native_data[0] = (jlong)pixmap;
    p_native_data[1] = pixmap->width;
    p_native_data[2] = pixmap->height;
    p_native_data[3] = pixmap->format;
    env->ReleasePrimitiveArrayCritical(nativeData, p_native_data, 0);

    return pixel_buffer;
 */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern ByteBuffer loadByteBuffer( long[] nativeData, ByteBuffer buffer, int offset, int len );
    /*
        if(buffer==0)
            return 0;

        const unsigned char* p_buffer = (const unsigned char*)env->GetDirectBufferAddress(buffer);
        gdx2d_pixmap* pixmap = gdx2d_load(p_buffer + offset, len);

        if(pixmap==0)
            return 0;

        jobject pixel_buffer = env->NewDirectByteBuffer((void*)pixmap->pixels, pixmap->width * pixmap->height * gdx2d_bytes_per_pixel(pixmap->format));
        jlong* p_native_data = (jlong*)env->GetPrimitiveArrayCritical(nativeData, 0);
        p_native_data[0] = (jlong)pixmap;
        p_native_data[1] = pixmap->width;
        p_native_data[2] = pixmap->height;
        p_native_data[3] = pixmap->format;
        env->ReleasePrimitiveArrayCritical(nativeData, p_native_data, 0);

        return pixel_buffer;
     */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern PixmapDef gdx2d_new( int width, int height, int format );
    /*
        gdx2d_pixmap* pixmap = gdx2d_new(width, height, format);
        if(pixmap==0)
            return 0;

        jobject pixel_buffer = env->NewDirectByteBuffer((void*)pixmap->pixels, pixmap->width * pixmap->height * gdx2d_bytes_per_pixel(pixmap->format));
        jlong* p_native_data = (jlong*)env->GetPrimitiveArrayCritical(nativeData, 0);
        p_native_data[0] = (jlong)pixmap;
        p_native_data[1] = pixmap->width;
        p_native_data[2] = pixmap->height;
        p_native_data[3] = pixmap->format;
        env->ReleasePrimitiveArrayCritical(nativeData, p_native_data, 0);

        return pixel_buffer;
     */

    // ------------------------------------------------------------------------

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern string gdx2d_get_failure_reason();
    /* return env->NewStringUTF(gdx2d_get_failure_reason()); */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_clear( PixmapDef pd, int color );
    /* gdx2d_clear((gdx2d_pixmap*)pixmap, color); */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern int gdx2d_get_pixel( PixmapDef pd, int x, int y );
    /* return gdx2d_get_pixel((gdx2d_pixmap*)pixmap, x, y); */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_pixel( PixmapDef pd, int x, int y, int color );
    /* gdx2d_set_pixel((gdx2d_pixmap*)pixmap, x, y, color); */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_blend( PixmapDef src, int blend );
    /* gdx2d_set_blend((gdx2d_pixmap*)src, blend); */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_scale( PixmapDef src, int scale );
    /* gdx2d_set_scale((gdx2d_pixmap*)src, scale); */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern uint gdx2d_bytes_per_pixel( uint format );
    /* */

    [DllImport( "Libs/gdx2d.dll", EntryPoint = "gdx2d_free", CallingConvention = CallingConvention.Cdecl )]
    private static extern void free( PixmapDef pixmap );
    /* gdx2d_free((gdx2d_pixmap*)pixmap); */

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_line( PixmapDef pd, int x0, int y0, int x1, int y1, uint col );

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_rect( PixmapDef pixmap, int x, int y, uint width, uint height, uint col );

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_circle(const gdx2d_pixmap* pixmap, int32_t x, int32_t y, uint32_t radius, uint32_t col);

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    gdx2d_fill_rect
        
    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    gdx2d_fill_circle

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    gdx2d_fill_triangle

    [DllImport( "Libs/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx_draw_pixmap( PixmapDef src,
                                                long dst,
                                                int srcX,
                                                int srcY,
                                                int srcWidth,
                                                int srcHeight,
                                                int dstX,
                                                int dstY,
                                                int dstWidth,
                                                int dstHeight );
    /* gdx2d_draw_pixmap((gdx2d_pixmap*)src, (gdx2d_pixmap*)dst, srcX, srcY, srcWidth, srcHeight, dstX, dstY, dstWidth, dstHeight); */
}

// ----------------------------------------------------------------------------

[PublicAPI, StructLayout( LayoutKind.Sequential )]
public struct PixmapDef
{
    public int    Width  { get; set; }
    public int    Height { get; set; }
    public int    Format { get; set; }
    public int    Blend  { get; set; }
    public int    Scale  { get; set; }
    public byte[] Pixels { get; set; }
}