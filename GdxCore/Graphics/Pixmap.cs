using System.Diagnostics.CodeAnalysis;

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
}
