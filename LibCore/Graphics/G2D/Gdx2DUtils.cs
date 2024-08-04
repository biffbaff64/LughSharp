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

//TODO: There are many methods in here that could become properties.
//TODO: I am delaying this refactoring until I have this class working correctly.

[PublicAPI]
public class Gdx2DUtils
{
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct Gdx2dPixmap
    {
        public uint   Width  { get; set; }
        public uint   Height { get; set; }
        public uint   Format { get; set; }
        public uint   Blend  { get; set; }
        public uint   Scale  { get; set; }
        public byte[] Pixels { get; set; }
    }

    // ------------------------------------------------------------------------

    private static uint[] _lu4 = null!;
    private static uint[] _lu5 = null!;
    private static uint[] _lu6 = null!;

    // ------------------------------------------------------------------------

    public static void GenerateLookups()
    {
        _lu4 = new uint[ 16 ];
        _lu5 = new uint[ 32 ];
        _lu6 = new uint[ 64 ];

        for ( uint i = 0; i < 16; i++ )
        {
            _lu4[ i ] = ( uint ) ( i / 15.0f * 255 );
            _lu5[ i ] = ( uint ) ( i / 31.0f * 255 );
            _lu6[ i ] = ( uint ) ( i / 63.0f * 255 );
        }

        for ( uint i = 16; i < 32; i++ )
        {
            _lu5[ i ] = ( uint ) ( i / 31.0f * 255 );
            _lu6[ i ] = ( uint ) ( i / 63.0f * 255 );
        }

        for ( uint i = 32; i < 64; i++ )
        {
            _lu6[ i ] = ( uint ) ( i / 63.0f * 255 );
        }
    }

    private static uint ToFormat( uint format, uint color )
    {
        uint r;
        uint g;
        uint b;
        uint a;

        switch ( format )
        {
            case PixmapFormat.GDX_2D_FORMAT_ALPHA:
                return color & 0xff;

            case PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA:
                r = ( color & 0xff000000 ) >> 24;
                g = ( color & 0xff0000 ) >> 16;
                b = ( color & 0xff00 ) >> 8;
                a = color & 0xff;

                var l = ( ( uint ) ( 0.2126f * r + 0.7152f * g + 0.0722f * b ) & 0xff ) << 8;

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
                a = ( color & 0xff ) >> 4 & 0xf;

                return r | g | b | a;

            default:
                return 0;
        }
    }

    public static uint WeightRGBA8888( uint color, float weight )
    {
        var r = Math.Min( ( uint ) ( ( ( color & 0xff000000 ) >> 24 ) * weight ), 255 );
        var g = Math.Min( ( uint ) ( ( ( color & 0xff0000 ) >> 16 ) * weight ), 255 );
        var b = Math.Min( ( uint ) ( ( ( color & 0xff00 ) >> 8 ) * weight ), 255 );
        var a = Math.Min( ( uint ) ( ( color & 0xff ) * weight ), 255 );

        return ( r << 24 ) | ( g << 16 ) | ( b << 8 ) | a;
    }

    public static uint ToRGBA8888( uint format, uint color )
    {
        GenerateLookups();

        uint r;
        uint g;
        uint b;

        switch ( format )
        {
            case PixmapFormat.GDX_2D_FORMAT_ALPHA:
                return ( color & 0xff ) | 0xffffff00;

            case PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA:
                return ( ( color & 0xff00 ) << 16 ) | ( ( color & 0xff00 ) << 8 ) | ( color & 0xffff );

            case PixmapFormat.GDX_2D_FORMAT_RGB888:
                return ( color << 8 ) | 0x000000ff;

            case PixmapFormat.GDX_2D_FORMAT_RGBA8888:
                return color;

            case PixmapFormat.GDX_2D_FORMAT_RGB565:
                r = _lu5[ ( color & 0xf800 ) >> 11 ] << 24;
                g = _lu6[ ( color & 0x7e0 ) >> 5 ] << 16;
                b = _lu5[ ( color & 0x1f ) ] << 8;

                return r | g | b | 0xff;

            case PixmapFormat.GDX_2D_FORMAT_RGBA4444:
                r = _lu4[ ( color & 0xf000 ) >> 12 ] << 24;
                g = _lu4[ ( color & 0xf00 ) >> 8 ] << 16;
                b = _lu4[ ( color & 0xf0 ) >> 4 ] << 8;

                var a = _lu4[ ( color & 0xf ) ];

                return r | g | b | a;

            default:
                return 0;
        }
    }

    public static Func< byte[], uint > GetPixelFuncPtr( uint format )
    {
        return format switch
        {
            PixmapFormat.GDX_2D_FORMAT_ALPHA           => GetPixelAlpha,
            PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA => GetPixelLuminanceAlpha,
            PixmapFormat.GDX_2D_FORMAT_RGB888          => GetPixelRGB888,
            PixmapFormat.GDX_2D_FORMAT_RGBA8888        => GetPixelRGBA8888,
            PixmapFormat.GDX_2D_FORMAT_RGB565          => GetPixelRGB565,
            PixmapFormat.GDX_2D_FORMAT_RGBA4444        => GetPixelRGBA4444,
            var _                                      => GetPixelAlpha,
        };
    }

    public static Action< byte[], uint > SetPixelFuncPtr( uint format )
    {
        return format switch
        {
            PixmapFormat.GDX_2D_FORMAT_ALPHA           => SetPixelAlpha,
            PixmapFormat.GDX_2D_FORMAT_LUMINANCE_ALPHA => SetPixelLuminanceAlpha,
            PixmapFormat.GDX_2D_FORMAT_RGB888          => SetPixelRGB888,
            PixmapFormat.GDX_2D_FORMAT_RGBA8888        => SetPixelRGBA8888,
            PixmapFormat.GDX_2D_FORMAT_RGB565          => SetPixelRGB565,
            PixmapFormat.GDX_2D_FORMAT_RGBA4444        => SetPixelRGBA4444,
            var _                                      => SetPixelAlpha,
        };
    }

    public static uint Gdx2dBlend( uint src, uint dst )
    {
        var srcA = src & 0xff;

        if ( srcA == 0 )
        {
            return dst;
        }

        var srcB = ( src >> 8 ) & 0xff;
        var srcG = ( src >> 16 ) & 0xff;
        var srcR = ( src >> 24 ) & 0xff;

        var dstA = dst & 0xff;
        var dstB = ( dst >> 8 ) & 0xff;
        var dstG = ( dst >> 16 ) & 0xff;
        var dstR = ( dst >> 24 ) & 0xff;

        dstA -= ( dstA * srcA ) / 255;

        var a = dstA + srcA;

        dstR = ( dstR * dstA + srcR * srcA ) / a;
        dstG = ( dstG * dstA + srcG * srcA ) / a;
        dstB = ( dstB * dstA + srcB * srcA ) / a;

        return ( dstR << 24 ) | ( dstG << 16 ) | ( dstB << 8 ) | a;
    }

    public static Gdx2dPixmap? Gdx2dLoad( byte[] buffer, uint len )
    {
        var pixels = new byte[ len ];

        LoadImageFromMemory( buffer, len, out var width, out var height, out var format, ref pixels );

        var pixmap = new Gdx2dPixmap
        {
            Width  = ( uint ) width,
            Height = ( uint ) height,
            Format = ( uint ) format,
            Blend  = PixmapFormat.GDX_2D_BLEND_SRC_OVER,
            Scale  = PixmapFormat.GDX_2D_SCALE_BILINEAR,
            Pixels = pixels
        };

        return pixmap;
    }

    private static void LoadImageFromMemory( byte[] buffer, uint len, out int w, out int h, out int f, ref byte[] bytes )
    {
        //TODO: Set up data correctly

        for ( var i = 0; i < len; i++ )
        {
            bytes[ i ] = buffer[ i ];
        }

        w = 0;
        h = 0;
        f = PixmapFormat.GDX_2D_FORMAT_RGBA8888;
    }

    public static Gdx2dPixmap Gdx2dNew( int width, int height, int format )
    {
        var pixmap = new Gdx2dPixmap
        {
            Width  = ( uint ) width,
            Height = ( uint ) height,
            Format = ( uint ) format,
            Blend  = PixmapFormat.GDX_2D_BLEND_SRC_OVER,
            Scale  = PixmapFormat.GDX_2D_SCALE_BILINEAR,
            Pixels = new byte[ width * height * PixmapFormat.Gdx2dBytesPerPixel( format ) ]
        };

        return pixmap;
    }

    // ------------------------------------------------------------------------

    public static void SetPixelAlpha( byte[] pixelAddr, uint color )
    {
        pixelAddr[ 0 ] = ( byte ) ( color & 0xff );
    }

    public static void SetPixelLuminanceAlpha( byte[] pixelAddr, uint color )
    {
        BitConverter.GetBytes( ( ushort ) color ).CopyTo( pixelAddr, 0 );
    }

    public static void SetPixelRGB888( byte[] pixelAddr, uint color )
    {
        pixelAddr[ 0 ] = ( byte ) ( ( color & 0xff0000 ) >> 16 );
        pixelAddr[ 1 ] = ( byte ) ( ( color & 0xff00 ) >> 8 );
        pixelAddr[ 2 ] = ( byte ) ( color & 0xff );
    }

    public static void SetPixelRGBA8888( byte[] pixelAddr, uint color )
    {
        pixelAddr[ 0 ] = ( byte ) ( ( color & 0xff000000 ) >> 24 );
        pixelAddr[ 1 ] = ( byte ) ( ( color & 0xff0000 ) >> 16 );
        pixelAddr[ 2 ] = ( byte ) ( ( color & 0xff00 ) >> 8 );
        pixelAddr[ 3 ] = ( byte ) ( color & 0xff );
    }

    public static void SetPixelRGB565( byte[] pixelAddr, uint color )
    {
        BitConverter.GetBytes( ( ushort ) color ).CopyTo( pixelAddr, 0 );
    }

    public static void SetPixelRGBA4444( byte[] pixelAddr, uint color )
    {
        BitConverter.GetBytes( ( ushort ) color ).CopyTo( pixelAddr, 0 );
    }

    public static uint GetPixelAlpha( byte[] pixelAddr )
    {
        return pixelAddr[ 0 ];
    }

    public static uint GetPixelLuminanceAlpha( byte[] pixelAddr )
    {
        return ( ( uint ) pixelAddr[ 0 ] << 8 ) | pixelAddr[ 1 ];
    }

    public static uint GetPixelRGB888( byte[] pixelAddr )
    {
        return ( ( uint ) pixelAddr[ 0 ] << 16 ) | ( ( uint ) pixelAddr[ 1 ] << 8 ) | pixelAddr[ 2 ];
    }

    public static uint GetPixelRGBA8888( byte[] pixelAddr )
    {
        return ( ( uint ) pixelAddr[ 0 ] << 24 )
             | ( ( uint ) pixelAddr[ 1 ] << 16 )
             | ( ( uint ) pixelAddr[ 2 ] << 8 )
             | pixelAddr[ 3 ];
    }

    public static uint GetPixelRGB565( byte[] pixelAddr )
    {
        return BitConverter.ToUInt16( pixelAddr, 0 );
    }

    public static uint GetPixelRGBA4444( byte[] pixelAddr )
    {
        return BitConverter.ToUInt16( pixelAddr, 0 );
    }

    public static void Gdx2dFree( Gdx2dPixmap pixmap )
    {
        //TODO: In C#, garbage collection handles this, so no need to free memory manually
        pixmap.Pixels = null!;
    }

    public static void Gdx2dSetBlend( Gdx2dPixmap pixmap, uint blend )
    {
        pixmap.Blend = blend;
    }

    public static void Gdx2dSetScale( Gdx2dPixmap pixmap, uint scale )
    {
        pixmap.Scale = scale;
    }

    public static string Gdx2dGetFailureReason()
    {
        //TODO: Replace with an actual method to retrieve the error reason from the image library
        return "Failure reason not implemented.";
    }

    public static void ClearAlpha( Gdx2dPixmap pixmap, uint col )
    {
        Array.Fill( pixmap.Pixels, ( byte ) col );
    }
}