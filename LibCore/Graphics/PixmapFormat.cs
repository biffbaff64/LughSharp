﻿// ///////////////////////////////////////////////////////////////////////////////
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


using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Graphics;

[PublicAPI]
public class PixmapFormat
{
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

    // ------------------------------------------------------------------------
    
    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGdx2DPixmapFormat( Pixmap.Format format )
    {
        return format switch
        {
            Pixmap.Format.Alpha          => GDX_2D_FORMAT_ALPHA,
            Pixmap.Format.Intensity      => GDX_2D_FORMAT_ALPHA,
            Pixmap.Format.LuminanceAlpha => GDX_2D_FORMAT_LUMINANCE_ALPHA,
            Pixmap.Format.RGB565         => GDX_2D_FORMAT_RGB565,
            Pixmap.Format.RGBA4444       => GDX_2D_FORMAT_RGBA4444,
            Pixmap.Format.RGB888         => GDX_2D_FORMAT_RGB888,
            Pixmap.Format.RGBA8888       => GDX_2D_FORMAT_RGBA8888,

            var _ => GetDefault()
        };
        
        int GetDefault()
        {
            Logger.Divider( '=' );
            Logger.Debug( $"Unknown Format: {format}. Returning Default ( GDX_2D_FORMAT_RGBA8888 )"  );
            Logger.Divider( '=' );

            return GDX_2D_FORMAT_RGBA8888;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static Pixmap.Format FromGdx2DPixmapFormat( int format )
    {
        return format switch
        {
            GDX_2D_FORMAT_ALPHA           => Pixmap.Format.Alpha,
            GDX_2D_FORMAT_LUMINANCE_ALPHA => Pixmap.Format.LuminanceAlpha,
            GDX_2D_FORMAT_RGB565          => Pixmap.Format.RGB565,
            GDX_2D_FORMAT_RGBA4444        => Pixmap.Format.RGBA4444,
            GDX_2D_FORMAT_RGB888          => Pixmap.Format.RGB888,
            GDX_2D_FORMAT_RGBA8888        => Pixmap.Format.RGBA8888,

            var _ => GetDefault()
        };
        
        Pixmap.Format GetDefault()
        {
            Logger.Divider( '=' );
            Logger.Debug( $"Unknown Gdx2DPixmap Format: {format}. Returning Default ( RGBA8888 )"  );
            Logger.Divider( '=' );

            return Pixmap.Format.RGBA8888;
        }
    }

    /// <summary>
    /// Gets the number of bytes required for 1 pixel of the specified format.
    /// </summary>
    public static int Gdx2dBytesPerPixel( int format )
    {
        return format switch
        {
            GDX_2D_FORMAT_ALPHA => 1,
            GDX_2D_FORMAT_LUMINANCE_ALPHA
                or GDX_2D_FORMAT_RGB565
                or GDX_2D_FORMAT_RGBA4444 => 2,
            GDX_2D_FORMAT_RGB888   => 3,
            GDX_2D_FORMAT_RGBA8888 => 4,
            _                      => 4,
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int ToGLFormat( Pixmap.Format format )
    {
        var cformat = ToGdx2DPixmapFormat( format );
        
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
    public static int ToGLType( Pixmap.Format format )
    {
        var cformat = ToGdx2DPixmapFormat( format );
        
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
            GDX_2D_FORMAT_ALPHA           => "Alpha",
            GDX_2D_FORMAT_LUMINANCE_ALPHA => "Luminance Alpha",
            GDX_2D_FORMAT_RGB888          => "Rgb888",
            GDX_2D_FORMAT_RGBA8888        => "Rgba8888",
            GDX_2D_FORMAT_RGB565          => "Rgb565",
            GDX_2D_FORMAT_RGBA4444        => "Rgba4444",
            var _                         => "Unknown"
        };
    }
}