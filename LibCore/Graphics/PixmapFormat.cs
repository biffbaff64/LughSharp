// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Graphics.G2D;

namespace LughSharp.LibCore.Graphics;

public static class PixmapFormat
{
    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGdx2DPixmapFormat( Pixmap.Format format )
    {
        return format switch
               {
                   Pixmap.Format.Alpha          => Gdx2DPixmap.GDX_2D_FORMAT_ALPHA,
                   Pixmap.Format.Intensity      => Gdx2DPixmap.GDX_2D_FORMAT_ALPHA,
                   Pixmap.Format.LuminanceAlpha => Gdx2DPixmap.GDX_2D_FORMAT_LUMINANCE_ALPHA,
                   Pixmap.Format.RGB565         => Gdx2DPixmap.GDX_2D_FORMAT_RGB565,
                   Pixmap.Format.RGBA4444       => Gdx2DPixmap.GDX_2D_FORMAT_RGBA4444,
                   Pixmap.Format.RGB888         => Gdx2DPixmap.GDX_2D_FORMAT_RGB888,
                   Pixmap.Format.RGBA8888       => Gdx2DPixmap.GDX_2D_FORMAT_RGBA8888,

                   _ => throw new GdxRuntimeException( $"Unknown Format: {format}" )
               };
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
                   Gdx2DPixmap.GDX_2D_FORMAT_ALPHA           => Pixmap.Format.Alpha,
                   Gdx2DPixmap.GDX_2D_FORMAT_LUMINANCE_ALPHA => Pixmap.Format.LuminanceAlpha,
                   Gdx2DPixmap.GDX_2D_FORMAT_RGB565          => Pixmap.Format.RGB565,
                   Gdx2DPixmap.GDX_2D_FORMAT_RGBA4444        => Pixmap.Format.RGBA4444,
                   Gdx2DPixmap.GDX_2D_FORMAT_RGB888          => Pixmap.Format.RGB888,
                   Gdx2DPixmap.GDX_2D_FORMAT_RGBA8888        => Pixmap.Format.RGBA8888,

                   _ => throw new GdxRuntimeException( "Unknown Gdx2DPixmap Format: " + format )
               };
    }

    //@formatter:on
    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int ToGLFormat( Pixmap.Format format )
    {
        return Gdx2DPixmap.ToGLFormat( ToGdx2DPixmapFormat( format ) );
    }

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int ToGLType( Pixmap.Format format )
    {
        return Gdx2DPixmap.ToGLType( ToGdx2DPixmapFormat( format ) );
    }
}
