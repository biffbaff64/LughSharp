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

namespace LughSharp.LibCore.Graphics.G2D;

public partial class Gdx2DPixmap
{
    public void Clear( PixmapDef pd, Color color )
    {
        Logger.CheckPoint();
        Logger.Debug( $"pd.Width: {pd.Width}" );
        Logger.Debug( $"pd.Height: {pd.Height}" );
        Logger.Debug( $"pd.Format: {pd.Format}" );
        Logger.Debug( $"pd.Color: {color} {color.ToIntBits()}" );
        
//        gdx2d_clear( pd, color.ColorToUInt() );
    }

    /// <summary>
    /// Gets the pixel at the specified X and Y coordinates.
    /// </summary>
    /// <param name="pd"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int GetPixel( PixmapDef pd, int x, int y )
    {
        return gdx2d_get_pixel( pd, x, y );
    }

    public void SetPixel( PixmapDef pd, int x, int y, Color color )
    {
        gdx2d_set_pixel( pd, x, y, color.ToIntBits() );
    }

    public void DrawLine( int x, int y, int x2, int y2, Color color )
    {
        gdx2d_draw_line( PixmapDef, x, y, x2, y2, color.ToIntBits() );
    }

    public void DrawRect( int x, int y, uint width, uint height, Color color )
    {
        gdx2d_draw_rect( PixmapDef, x, y, width, height, color.ToIntBits() );
    }

    public void DrawCircle( int x, int y, uint radius, Color color )
    {
        gdx2d_draw_circle( PixmapDef, x, y, radius, color.ToIntBits() );
    }

    public void FillRect( int x, int y, uint width, uint height, Color color )
    {
        gdx2d_fill_rect( PixmapDef, x, y, width, height, color.ToIntBits() );
    }

    public void FillCircle( int x, int y, uint radius, Color color )
    {
        gdx2d_fill_circle( PixmapDef, x, y, radius, color.ToIntBits() );
    }

    public void FillTriangle( int x1, int y1, int x2, int y2, int x3, int y3, Color color )
    {
        gdx2d_fill_triangle( PixmapDef, x1, y1, x2, y2, x3, y3, color.ToIntBits() );
    }

    public void DrawPixmap( Gdx2DPixmap src, int srcX, int srcY, int dstX, int dstY, int width, int height )
    {
        gdx2d_draw_pixmap( src.PixmapDef, PixmapDef, srcX, srcY, width, height, dstX, dstY, width, height );
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
        gdx2d_draw_pixmap( src.PixmapDef, PixmapDef, srcX, srcY, srcWidth, srcHeight, dstX, dstY, dstWidth, dstHeight );
    }

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
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

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
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

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
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

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern string gdx2d_get_failure_reason();

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_clear( PixmapDef pd, uint color );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern int gdx2d_get_pixel( PixmapDef pd, int x, int y );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_pixel( PixmapDef pd, int x, int y, uint color );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_blend( PixmapDef src, int blend );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_scale( PixmapDef src, int scale );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern uint gdx2d_bytes_per_pixel( uint format );

    [DllImport( "lib/gdx2d.dll", EntryPoint = "gdx2d_free", CallingConvention = CallingConvention.Cdecl )]
    private static extern void free( PixmapDef pixmap );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_line( PixmapDef pd, int x0, int y0, int x1, int y1, uint col );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_rect( PixmapDef pd, int x, int y, uint width, uint height, uint col );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_circle( PixmapDef pd, int x, int y, uint radius, uint col );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_rect( PixmapDef pd, int x, int y, uint width, uint height, uint col );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_circle( PixmapDef pd, int x0, int y0, uint radius, uint col );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_triangle( PixmapDef pd, int x1, int y1, int x2, int y2, int x3, int y3, uint col );

    [DllImport( "lib/gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_pixmap( PixmapDef src,
                                                  PixmapDef dst,
                                                  int srcX,
                                                  int srcY,
                                                  int srcWidth,
                                                  int srcHeight,
                                                  int dstX,
                                                  int dstY,
                                                  int dstWidth,
                                                  int dstHeight );
}