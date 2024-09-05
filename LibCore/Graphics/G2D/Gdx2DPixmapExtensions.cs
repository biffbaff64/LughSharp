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
    public void Clear( NativePixmapDef pd, Color color )
    {
        Logger.CheckPoint();
        Logger.Debug( $"pd.Width: {pd.Width}" );
        Logger.Debug( $"pd.Height: {pd.Height}" );
        Logger.Debug( $"pd.Format: {pd.Format}" );
        Logger.Debug( $"pd.Color: {color} {color.ToIntBits()}" );
        
        gdx2d_clear( pd, color.ToIntBits() );
    }

    /// <summary>
    /// Gets the pixel at the specified X and Y coordinates.
    /// </summary>
    /// <param name="pd"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int GetPixel( NativePixmapDef pd, int x, int y )
    {
        return gdx2d_get_pixel( pd, x, y );
    }

    public void SetPixel( NativePixmapDef pd, int x, int y, Color color )
    {
        gdx2d_set_pixel( pd, x, y, color.ToIntBits() );
    }

    public void DrawLine( int x, int y, int x2, int y2, Color color )
    {
        gdx2d_draw_line( NativePixmapDef, x, y, x2, y2, color.ToIntBits() );
    }

    public void DrawRect( int x, int y, uint width, uint height, Color color )
    {
        gdx2d_draw_rect( NativePixmapDef, x, y, width, height, color.ToIntBits() );
    }

    public void DrawCircle( int x, int y, uint radius, Color color )
    {
        gdx2d_draw_circle( NativePixmapDef, x, y, radius, color.ToIntBits() );
    }

    public void FillRect( int x, int y, uint width, uint height, Color color )
    {
        gdx2d_fill_rect( NativePixmapDef, x, y, width, height, color.ToIntBits() );
    }

    public void FillCircle( int x, int y, uint radius, Color color )
    {
        gdx2d_fill_circle( NativePixmapDef, x, y, radius, color.ToIntBits() );
    }

    public void FillTriangle( int x1, int y1, int x2, int y2, int x3, int y3, Color color )
    {
        gdx2d_fill_triangle( NativePixmapDef, x1, y1, x2, y2, x3, y3, color.ToIntBits() );
    }

    public void DrawPixmap( Gdx2DPixmap src, int srcX, int srcY, int dstX, int dstY, int width, int height )
    {
        gdx2d_draw_pixmap( src.NativePixmapDef, NativePixmapDef, srcX, srcY, width, height, dstX, dstY, width, height );
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
        gdx2d_draw_pixmap( src.NativePixmapDef, NativePixmapDef, srcX, srcY, srcWidth, srcHeight, dstX, dstY, dstWidth, dstHeight );
    }

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------
//TODO: Convert all of these to C#
    
    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern NativePixmapDef gdx2d_load( byte[] buffer, int len );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern NativePixmapDef gdx2d_new( int width, int height, int format );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern string gdx2d_get_failure_reason();

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_clear( NativePixmapDef pd, uint color );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern int gdx2d_get_pixel( NativePixmapDef pd, int x, int y );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_pixel( NativePixmapDef pd, int x, int y, uint color );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_blend( NativePixmapDef src, int blend );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_scale( NativePixmapDef src, int scale );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern uint gdx2d_bytes_per_pixel( uint format );

    [DllImport( "lib\\gdx2d.dll", EntryPoint = "gdx2d_free", CallingConvention = CallingConvention.Cdecl )]
    private static extern void free( NativePixmapDef pixmap );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_line( NativePixmapDef pd, int x0, int y0, int x1, int y1, uint col );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_rect( NativePixmapDef pd, int x, int y, uint width, uint height, uint col );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_circle( NativePixmapDef pd, int x, int y, uint radius, uint col );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_rect( NativePixmapDef pd, int x, int y, uint width, uint height, uint col );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_circle( NativePixmapDef pd, int x0, int y0, uint radius, uint col );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_triangle( NativePixmapDef pd, int x1, int y1, int x2, int y2, int x3, int y3, uint col );

    [DllImport( "lib\\gdx2d.dll", CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_pixmap( NativePixmapDef src,
                                                  NativePixmapDef dst,
                                                  int srcX,
                                                  int srcY,
                                                  int srcWidth,
                                                  int srcHeight,
                                                  int dstX,
                                                  int dstY,
                                                  int dstWidth,
                                                  int dstHeight );
    
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
}