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

using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Graphics;

[PublicAPI]
public class PixmapFormat
{
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

    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGdx2DFormat( Pixmap.ColorFormat? format )
    {
        return format switch
        {
            Pixmap.ColorFormat.Alpha          => GDX_2D_FORMAT_ALPHA,
            Pixmap.ColorFormat.Intensity      => GDX_2D_FORMAT_ALPHA,
            Pixmap.ColorFormat.LuminanceAlpha => GDX_2D_FORMAT_LUMINANCE_ALPHA,
            Pixmap.ColorFormat.RGB565         => GDX_2D_FORMAT_RGB565,
            Pixmap.ColorFormat.RGBA4444       => GDX_2D_FORMAT_RGBA4444,
            Pixmap.ColorFormat.RGB888         => GDX_2D_FORMAT_RGB888,
            Pixmap.ColorFormat.RGBA8888       => GDX_2D_FORMAT_RGBA8888,
            var _                             => throw new GdxRuntimeException( $"unknown format: {format}" )
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static Pixmap.ColorFormat ToPixmapColorFormat( int format )
    {
        return format switch
        {
            GDX_2D_FORMAT_ALPHA           => Pixmap.ColorFormat.Alpha,
            GDX_2D_FORMAT_LUMINANCE_ALPHA => Pixmap.ColorFormat.LuminanceAlpha,
            GDX_2D_FORMAT_RGB888          => Pixmap.ColorFormat.RGB888,
            GDX_2D_FORMAT_RGBA8888        => Pixmap.ColorFormat.RGBA8888,
            GDX_2D_FORMAT_RGB565          => Pixmap.ColorFormat.RGB565,
            GDX_2D_FORMAT_RGBA4444        => Pixmap.ColorFormat.RGBA4444,
            var _                         => throw new GdxRuntimeException( $"unknown format: {format}" )
        };
    }

    /// <summary>
    /// Gets the number of bytes required for 1 pixel of the specified format.
    /// </summary>
    public static int Gdx2dBytesPerPixel( int format )
    {
        return format switch
        {
            GDX_2D_FORMAT_ALPHA           => 1,
            GDX_2D_FORMAT_LUMINANCE_ALPHA => 2,
            GDX_2D_FORMAT_RGB565          => 2,
            GDX_2D_FORMAT_RGBA4444        => 2,
            GDX_2D_FORMAT_RGB888          => 3,
            GDX_2D_FORMAT_RGBA8888        => 4,
            _                             => 4,
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int ToGLFormat( Pixmap.ColorFormat format )
    {
        var cformat = ToGdx2DFormat( format );

        return cformat switch
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
    public static int ToGLType( Pixmap.ColorFormat format )
    {
        var cformat = ToGdx2DFormat( format );

        return cformat switch
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
    /// <param name="format"></param>
    /// <returns></returns>
    public static string GetFormatString( int format )
    {
        return format switch
        {
            GDX_2D_FORMAT_ALPHA           => "GDX_2D_FORMAT_ALPHA",
            GDX_2D_FORMAT_LUMINANCE_ALPHA => "GDX_2D_FORMAT_LUMINANCE_ALPHA",
            GDX_2D_FORMAT_RGB888          => "GDX_2D_FORMAT_RGB888",
            GDX_2D_FORMAT_RGBA8888        => "GDX_2D_FORMAT_RGBA8888",
            GDX_2D_FORMAT_RGB565          => "GDX_2D_FORMAT_RGB565",
            GDX_2D_FORMAT_RGBA4444        => "GDX_2D_FORMAT_RGBA4444",
            var _                         => "Unknown"
        };
    }

    /// <summary>
    /// Extracts the <c>Width</c> and <c>Height</c> from a PNG file.
    /// </summary>
    /// <remarks>
    /// Adapted from code obtained elsewhere. I'm not sure of where, and will credit
    /// the original author when I have corrected this.
    /// </remarks>
    public static ( int width, int height ) GetPNGWidthHeight( FileInfo file )
    {
        if ( file.Extension.ToLower() != ".png" )
        {
            throw new GdxRuntimeException( $"PNG files ONLY!: ({file.Name})" );
        }

        var br = new BinaryReader( File.OpenRead( file.Name ) );

        br.BaseStream.Position = 16;

        var widthbytes  = new byte[ sizeof( int ) ];
        var heightbytes = new byte[ sizeof( int ) ];

        for ( var i = 0; i < sizeof( int ); i++ )
        {
            widthbytes[ sizeof( int ) - 1 - i ] = br.ReadByte();
        }

        for ( var i = 0; i < sizeof( int ); i++ )
        {
            heightbytes[ sizeof( int ) - 1 - i ] = br.ReadByte();
        }

        return ( BitConverter.ToInt32( widthbytes, 0 ),
                 BitConverter.ToInt32( heightbytes, 0 ) );
    }
}