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

namespace Corelib.LibCore.Graphics.G2D;

public partial class Gdx2DPixmap
{
    /// <summary>
    /// Clear the pd defined in the supplied <see cref="PixmapDataType"/>,
    /// setting it to the supplied Color.
    /// </summary>
    /// <param name="color"> The Color. </param>
    public void Clear( Color color )
    {
        var size = ( uint ) ( _pixmapDataType.Width
                            * _pixmapDataType.Height
                            * PixmapFormat.Gdx2dBytesPerPixel( ( int ) _pixmapDataType.Format ) );

        switch ( _pixmapDataType.Format )
        {
            case PixmapFormat.GDX_2D_FORMAT_ALPHA:
                clear_alpha( _pixmapDataType, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA:
                clear_luminance_alpha( _pixmapDataType, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_RGB888:
                clear_RGB888( _pixmapDataType, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_RGBA8888:
                clear_RGBA8888( _pixmapDataType, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_RGB565:
                clear_RGB565( _pixmapDataType, color, size );
                break;

            case PixmapFormat.GDX_2D_FORMAT_RGBA4444:
                clear_RGBA4444( _pixmapDataType, color, size );
                break;

            default:
                break;
        }

        Array.Copy( _pixmapDataType.Pixels, PixmapBuffer.Hb!, _pixmapDataType.Pixels.Length );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    internal static void clear_alpha( PixmapDataType pd, Color color, uint size )
    {
//        int pixels = pixmap->width * pixmap->height;
//        memset((void*)pixmap->pixels, col, pixels);
    }

    internal static void clear_luminance_alpha( PixmapDataType pd, Color color, uint size )
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

    internal static void clear_RGB888( PixmapDataType pd, Color color, uint size )
    {
        var col = Color.RGB888( color );
        var b   = ( byte ) ( ( col & 0x0000ff00 ) >> 8 );
        var g   = ( byte ) ( ( col & 0x00ff0000 ) >> 16 );
        var r   = ( byte ) ( ( col & 0xff000000 ) >> 24 );

        for ( var pixel = 0; pixel < size; )
        {
            pd.Pixels[ pixel++ ] = b;
            pd.Pixels[ pixel++ ] = g;
            pd.Pixels[ pixel++ ] = r;
        }
    }

    internal static void clear_RGBA8888( PixmapDataType pd, Color color, uint size )
    {
        var col  = Color.RGBA8888( color );
        var a    = ( byte ) ( ( col & 0x000000ff ) );
        var b    = ( byte ) ( ( col & 0x0000ff00 ) >> 8 );
        var g    = ( byte ) ( ( col & 0x00ff0000 ) >> 16 );
        var r    = ( byte ) ( ( col & 0xff000000 ) >> 24 );

        for ( var pixel = 0; pixel < size; )
        {
            pd.Pixels[ pixel++ ] = a;
            pd.Pixels[ pixel++ ] = b;
            pd.Pixels[ pixel++ ] = g;
            pd.Pixels[ pixel++ ] = r;
        }
    }

    internal static void clear_RGB565( PixmapDataType pd, Color color, uint size )
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
        
        var col  = Color.RGB565( color );
    }

    internal static void clear_RGBA4444( PixmapDataType pd, Color color, uint size )
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
        
        var col  = Color.RGBA4444( color );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private static uint ToFormat( uint format, uint color )
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