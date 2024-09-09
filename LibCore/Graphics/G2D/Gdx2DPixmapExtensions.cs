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
    /// <summary>
    /// Gets the pixel at the specified X and Y coordinates.
    /// </summary>
    /// <param name="pd"> The <see cref="gdx2d_pixmap"/> holding the pixmap info. </param>
    /// <param name="x"> X co-ordinate. </param>
    /// <param name="y"> Y co-ordinate. </param>
    /// <returns></returns>
    public int GetPixel( gdx2d_pixmap pd, int x, int y )
    {
        return gdx2d_get_pixel( pd, x, y );
    }

    public void SetPixel( gdx2d_pixmap pd, int x, int y, Color color )
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

    /// <summary>
    /// Simple pixmap struct holding the pixel data, the dimensions and the
    /// format of the pixmap. The format is one of the GDX_2D_FORMAT_XXX constants.
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]

    // ReSharper disable once InconsistentNaming
    public unsafe struct gdx2d_pixmap
    {
        public uint   Width  { get; set; }
        public uint   Height { get; set; }
        public uint   Format { get; set; }
        public uint   Blend  { get; set; }
        public uint   Scale  { get; set; }
        public byte[] Pixels { get; set; }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
//TODO: Convert all of these to C#

    private const string DLL_PATH = "lib/gdx2d.dll";

    [DllImport( DLL_PATH, SetLastError = true )]
    private static extern gdx2d_pixmap gdx2d_load( byte[] buffer, int len );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern gdx2d_pixmap gdx2d_new( int width, int height, int format );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern string gdx2d_get_failure_reason();

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_clear( gdx2d_pixmap pd, uint color );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern int gdx2d_get_pixel( gdx2d_pixmap pd, int x, int y );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_pixel( gdx2d_pixmap pd, int x, int y, uint color );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_blend( gdx2d_pixmap src, int blend );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_set_scale( gdx2d_pixmap src, int scale );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern uint gdx2d_bytes_per_pixel( uint format );

    [DllImport( DLL_PATH, EntryPoint = "gdx2d_free", CallingConvention = CallingConvention.Cdecl )]
    private static extern void free( gdx2d_pixmap pixmap );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_line( gdx2d_pixmap pd, int x0, int y0, int x1, int y1, uint col );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_rect( gdx2d_pixmap pd, int x, int y, uint width, uint height, uint col );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_circle( gdx2d_pixmap pd, int x, int y, uint radius, uint col );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_rect( gdx2d_pixmap pd, int x, int y, uint width, uint height, uint col );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_circle( gdx2d_pixmap pd, int x0, int y0, uint radius, uint col );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_fill_triangle( gdx2d_pixmap pd, int x1, int y1, int x2, int y2, int x3, int y3, uint col );

    [DllImport( DLL_PATH, CallingConvention = CallingConvention.Cdecl )]
    private static extern void gdx2d_draw_pixmap( gdx2d_pixmap src,
                                                  gdx2d_pixmap dst,
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