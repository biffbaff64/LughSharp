// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Graphics.G2D;

namespace LibGDXSharp.Graphics;

public static class PixmapFormat
{
    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static int ToGdx2DPixmapFormat( Pixmap.Format format ) =>

        //@formatter:off
        format switch
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

    //@formatter:on
    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static Pixmap.Format FromGdx2DPixmapFormat( int format ) =>

        //@formatter:off
        format switch
                                                                       {
                                                                           Gdx2DPixmap.GDX_2D_FORMAT_ALPHA           => Pixmap.Format.Alpha,
                                                                           Gdx2DPixmap.GDX_2D_FORMAT_LUMINANCE_ALPHA => Pixmap.Format.LuminanceAlpha,
                                                                           Gdx2DPixmap.GDX_2D_FORMAT_RGB565          => Pixmap.Format.RGB565,
                                                                           Gdx2DPixmap.GDX_2D_FORMAT_RGBA4444        => Pixmap.Format.RGBA4444,
                                                                           Gdx2DPixmap.GDX_2D_FORMAT_RGB888          => Pixmap.Format.RGB888,
                                                                           Gdx2DPixmap.GDX_2D_FORMAT_RGBA8888        => Pixmap.Format.RGBA8888,
                       
                                                                           _ => throw new GdxRuntimeException( "Unknown Gdx2DPixmap Format: " + format )
                                                                       };

    //@formatter:on
    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int ToGLFormat( Pixmap.Format format ) => Gdx2DPixmap.ToGLFormat( ToGdx2DPixmapFormat( format ) );

    /// <summary>
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int ToGLType( Pixmap.Format format ) => Gdx2DPixmap.ToGLType( ToGdx2DPixmapFormat( format ) );
}
