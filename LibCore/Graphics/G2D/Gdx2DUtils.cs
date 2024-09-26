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
    /// Clear the pd defined in the supplied <see cref="PixmapDataType"/>,
    /// setting it to the supplied Color.
    /// </summary>
    /// <param name="pd"> The NativePixmapDef. </param>
    /// <param name="color"> The Color. </param>
    public void Clear( PixmapDataType pd, Color color )
    {
        Logger.CheckPoint();

        var size = ( uint ) ( pd.Width * pd.Height * PixmapFormat.Gdx2dBytesPerPixel( ( int ) pd.Format ) );

        Logger.Debug( $"size: {size}, color: {color.R}, {color.G}, {color.B}, {color.A}" );

        switch ( pd.Format )
        {
            case PixmapFormat.GDX_2D_FORMAT_ALPHA:
                clear_alpha( pd, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA:
                clear_luminance_alpha( pd, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_RGB888:
                clear_RGB888( pd, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_RGBA8888:
                clear_RGBA8888( pd, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_RGB565:
                clear_RGB565( pd, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_RGBA4444:
                clear_RGBA4444( pd, color, size );
                break;

            default:
                break;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    internal void clear_alpha( PixmapDataType pd, Color color, uint size )
    {
//        int pixels = pixmap->width * pixmap->height;
//        memset((void*)pixmap->pixels, col, pixels);
    }

    internal void clear_luminance_alpha( PixmapDataType pd, Color color, uint size )
    {
//        int             pixels = pixmap->width * pixmap->height;
//        unsigned short* ptr    = (unsigned short*)pixmap->pixels;
//        unsigned short  l      = (col & 0xff) << 8 | (col >> 8);
//
//        for (; pixels > 0; pixels--)
//        {
//            *ptr = l;
//            ptr++;
//        }
    }

    internal void clear_RGB888( PixmapDataType pd, Color color, uint size )
    {
//        int            pixels = pixmap->width * pixmap->height;
//        unsigned char* ptr    = (unsigned char*)pixmap->pixels;
//        unsigned char  r      = (col & 0xff0000) >> 16;
//        unsigned char  g      = (col & 0xff00) >> 8;
//        unsigned char  b      = (col & 0xff);
//
//        for (; pixels > 0; pixels--)
//        {
//            *ptr = r;
//            ptr++;
//            *ptr = g;
//            ptr++;
//            *ptr = b;
//            ptr++;
//        }
    }

    internal void clear_RGBA8888( PixmapDataType pd, Color color, uint size )
    {
//        int           pixels = pixmap->width * pixmap->height;
//        uint32_t*     ptr    = (uint32_t*)pixmap->pixels;
//        unsigned char r      = (col & 0xff000000) >> 24;
//        unsigned char g      = (col & 0xff0000) >> 16;
//        unsigned char b      = (col & 0xff00) >> 8;
//        unsigned char a      = (col & 0xff);
//        col = (a << 24) | (b << 16) | (g << 8) | r;
//
//        for (; pixels > 0; pixels--)
//        {
//            *ptr = col;
//            ptr++;
//        }

//        var r = ( color.ToIntBits() & 0xff000000 ) >> 24;
//        var g = ( color.ToIntBits() & 0x00ff0000 ) >> 16;
//        var b = ( color.ToIntBits() & 0x0000ff00 ) >> 8;
//        var a = ( color.ToIntBits() & 0x000000ff );

//        var col = ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | r;

        var col = ToFormat( PixmapFormat.GDX_2D_FORMAT_RGB888, Color.RGBA8888( color ) );

        Logger.Debug( $"col: {col}"  );
        Logger.Debug( $"byte 0: {( col & 0x000000ff )}" );
        Logger.Debug( $"byte 1: {( col & 0x0000ff00 ) >> 8}" );
        Logger.Debug( $"byte 2: {( col & 0x00ff0000 ) >> 16}" );
        Logger.Debug( $"byte 3: {( col & 0xff000000 ) >> 24}" );

        pd.Pixels = Enumerable.Repeat( ( byte ) 255, ( int ) size ).ToArray();
    }

    internal void clear_RGB565( PixmapDataType pd, Color color, uint size )
    {
//        int             pixels = pixmap->width * pixmap->height;
//        unsigned short* ptr    = (unsigned short*)pixmap->pixels;
//        unsigned short  l      = col & 0xffff;
//
//        for (; pixels > 0; pixels--)
//        {
//            *ptr = l;
//            ptr++;
//        }
    }

    internal void clear_RGBA4444( PixmapDataType pd, Color color, uint size )
    {
//        int             pixels = pixmap->width * pixmap->height;
//        unsigned short* ptr    = (unsigned short*)pixmap->pixels;
//        unsigned short  l      = col & 0xffff;
//
//        for (; pixels > 0; pixels--)
//        {
//            *ptr = l;
//            ptr++;
//        }
/*
        var col    = ( byte ) color.ToIntBits();
        var offset = 0;

        fixed ( byte* ptr = pd.Pixels )
        {
            for ( ; pixels > 0; pixels-- )
            {
                *( ptr + offset ) = col;
                offset++;
            }
        }
*/
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private uint ToFormat( uint format, uint color )
    {
        uint r, g, b, a;

        switch ( format )
        {
            case PixmapFormat.GDX_2D_FORMAT_ALPHA:
                return color & 0xff;

            case PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA:
                r = ( color & 0xff000000 ) >> 24;
                g = ( color & 0xff0000 ) >> 16;
                b = ( color & 0xff00 ) >> 8;
                a = ( color & 0xff );
                var l = ( ( uint ) ( ( 0.2126f * r ) + ( 0.7152 * g ) + ( 0.0722 * b ) ) & 0xff ) << 8;
                return ( l & 0xffffff00 ) | a;

            case PixmapFormat.GDX_2D_FORMAT_RGB888:
                return color >> 8;

            case PixmapFormat.GDX_2D_FORMAT_RGBA8888:
                return color;

            case PixmapFormat.GDX_2D_FORMAT_RGB565:
                r = ( ( ( color & 0xff000000 ) >> 27 ) << 11 ) & 0xf800;
                g = ( ( ( color & 0xff0000 ) >> 18 ) << 5 ) & 0x7e0;
                b = ( ( color & 0xff00 ) >> 11 ) & 0x1f;
                return r | g | b;

            case PixmapFormat.GDX_2D_FORMAT_RGBA4444:
                r = ( ( ( color & 0xff000000 ) >> 28 ) << 12 ) & 0xf000;
                g = ( ( ( color & 0xff0000 ) >> 20 ) << 8 ) & 0xf00;
                b = ( ( ( color & 0xff00 ) >> 12 ) << 4 ) & 0xf0;
                a = ( ( color & 0xff ) >> 4 ) & 0xf;
                return r | g | b | a;

            default:
                return 0;
        }
    }
}