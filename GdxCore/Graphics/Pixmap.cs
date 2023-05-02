using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;

namespace LibGDXSharp.Graphics
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    [SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
    public class Pixmap : IDisposable
    {
        public enum Blending
        {
            None, SourceOver
        }

        public enum Filter
        {
            NearestNeighbour, BiLinear
        }

        public Pixmap( int potW, int potH, Format rgba8888 )
        {
            throw new NotImplementedException();
        }

        public Pixmap( FileInfo potW )
        {
            throw new NotImplementedException();
        }

        public enum Format
        {
            Alpha, Intensity, LuminanceAlpha, RGB565, RGBA4444, RGB888, RGBA8888
        }

        public static Pixmap CreateFromFrameBuffer( int i, int i1, int i2, int i3 )
        {
            throw new NotImplementedException();
        }

        public void SetBlending( object none )
        {
            throw new NotImplementedException();
        }

        public void DrawPixmap( Pixmap pixmap, int i, int i1 )
        {
            throw new NotImplementedException();
        }

        public static Pixmap.Format FormatFromString( string str )
        {
            str = str.ToLower();

            return str switch
                   {
                       "alpha"          => Pixmap.Format.Alpha,
                       "intensity"      => Pixmap.Format.Intensity,
                       "luminancealpha" => Pixmap.Format.LuminanceAlpha,
                       "rgb565"         => Pixmap.Format.RGB565,
                       "rgba4444"       => Pixmap.Format.RGBA4444,
                       "rgb888"         => Pixmap.Format.RGB888,
                       "rgba8888"       => Pixmap.Format.RGBA8888,
                       _                => throw new GdxRuntimeException( $"Unknown Format: {str}" )
                   };
        }

        private void Dispose( bool disposing )
        {
            if ( disposing )
            {
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }
    }

    public static class PixmapFormatExtensions
    {
        public static int ToGdx2DPixmapFormat( this Pixmap.Format format )
        {
            return format switch
                   {
                       Pixmap.Format.Alpha          => Gdx2DPixmap.Gdx2D_Format_Alpha,
                       Pixmap.Format.Intensity      => Gdx2DPixmap.Gdx2D_Format_Alpha,
                       Pixmap.Format.LuminanceAlpha => Gdx2DPixmap.Gdx2D_Format_Luminance_Alpha,
                       Pixmap.Format.RGB565         => Gdx2DPixmap.Gdx2D_Format_RGB565,
                       Pixmap.Format.RGBA4444       => Gdx2DPixmap.Gdx2D_Format_RGBA4444,
                       Pixmap.Format.RGB888         => Gdx2DPixmap.Gdx2D_Format_RGB888,
                       Pixmap.Format.RGBA8888       => Gdx2DPixmap.Gdx2D_Format_RGBA8888,
                       _                            => throw new GdxRuntimeException( $"Unknown Format: {format}" )
                   };
        }
    }
}
