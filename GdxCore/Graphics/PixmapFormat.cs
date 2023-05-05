using LibGDXSharp.G2D;

namespace LibGDXSharp.Graphics
{
    public static class PixmapFormat
    {
        /// <summary>
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public static int ToGdx2DPixmapFormat( Pixmap.Format format )
        {
            //@formatter:off
            return format switch
                   {
                       Pixmap.Format.Alpha          => Gdx2DPixmap.Gdx2D_Format_Alpha,
                       Pixmap.Format.Intensity      => Gdx2DPixmap.Gdx2D_Format_Alpha,
                       Pixmap.Format.LuminanceAlpha => Gdx2DPixmap.Gdx2D_Format_Luminance_Alpha,
                       Pixmap.Format.RGB565         => Gdx2DPixmap.Gdx2D_Format_RGB565,
                       Pixmap.Format.RGBA4444       => Gdx2DPixmap.Gdx2D_Format_RGBA4444,
                       Pixmap.Format.RGB888         => Gdx2DPixmap.Gdx2D_Format_RGB888,
                       Pixmap.Format.RGBA8888       => Gdx2DPixmap.Gdx2D_Format_RGBA8888,
                       
                       _ => throw new GdxRuntimeException( $"Unknown Format: {format}" )
                   };
            //@formatter:on
        }

        /// <summary>
        /// </summary>
        /// <param name="f"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public static Pixmap.Format FromGdx2DPixmapFormat( int format )
        {
            //@formatter:off
            return format switch
                   {
                       Gdx2DPixmap.Gdx2D_Format_Alpha           => Pixmap.Format.Alpha,
                       Gdx2DPixmap.Gdx2D_Format_Luminance_Alpha => Pixmap.Format.LuminanceAlpha,
                       Gdx2DPixmap.Gdx2D_Format_RGB565          => Pixmap.Format.RGB565,
                       Gdx2DPixmap.Gdx2D_Format_RGBA4444        => Pixmap.Format.RGBA4444,
                       Gdx2DPixmap.Gdx2D_Format_RGB888          => Pixmap.Format.RGB888,
                       Gdx2DPixmap.Gdx2D_Format_RGBA8888        => Pixmap.Format.RGBA8888,
                       
                       _ => throw new GdxRuntimeException( "Unknown Gdx2DPixmap Format: " + format )
                   };
            //@formatter:on
        }

        /// <summary>
        /// </summary>
        /// <param name="f"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static int ToGLFormat( Pixmap.Format format )
        {
            return Gdx2DPixmap.ToGLFormat( ToGdx2DPixmapFormat( format ) );
        }

        /// <summary>
        /// </summary>
        /// <param name="f"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static int ToGLType( Pixmap.Format format )
        {
            return Gdx2DPixmap.ToGLType( ToGdx2DPixmapFormat( format ) );
        }
    }
}
