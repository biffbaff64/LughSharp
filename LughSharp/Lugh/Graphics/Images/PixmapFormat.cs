// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Graphics.Images;

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

    // ========================================================================

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
            var _                             => throw new GdxRuntimeException( $"unknown format: {format}" ),
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
            var _                         => throw new GdxRuntimeException( $"unknown format: {format}" ),
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
            var _                         => 4,
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
            var _                         => throw new GdxRuntimeException( $"unknown format: {format}" ),
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
            var _                         => throw new GdxRuntimeException( $"unknown format: {format}" ),
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
            var _                         => $"Unknown format: {format}",
        };
    }

    public static string GetGLFormatName( int format )
    {
        return format switch
        {
            IGL.GL_ALPHA           => "IGL.GL_ALPHA",
            IGL.GL_LUMINANCE_ALPHA => "IGL.GL_LUMINANCE_ALPHA",
            IGL.GL_RGB             => "IGL.GL_RGB",
            IGL.GL_RGBA            => "IGL.GL_RGBA",
            var _                  => $"Unknown format: {format}",
        };
    }

    public static string GetGLTypeName( int format )
    {
        return format switch
        {
            IGL.GL_UNSIGNED_BYTE          => "IGL.GL_UNSIGNED_BYTE",
            IGL.GL_UNSIGNED_SHORT_5_6_5   => "IGL.GL_UNSIGNED_SHORT_5_6_5",
            IGL.GL_UNSIGNED_SHORT_4_4_4_4 => "IGL.GL_UNSIGNED_SHORT_4_4_4_4",
            var _                         => $"Unknown format: {format}",
        };
    }
}