using LibGDXSharp.G2D;
using LibGDXSharp.Maths;

namespace LibGDXSharp.Utils
{
    /// <summary>
    /// Class with static helper methods related to currently bound OpenGL
    /// frame buffer, including access to the current OpenGL FrameBuffer.
    /// These methods can be used to get the entire screen content or a
    /// portion thereof.
    /// </summary>
    public class ScreenUtils
    {
        /// <summary>
        /// Clears the color buffers with the specified Color.
        /// </summary>
        /// <param name="color">Color to clear the color buffers with.</param>
        public static void Clear( Color color )
        {
            Clear( color.R, color.G, color.B, color.A );
        }

        /// <summary>
        /// Clears the color buffers and optionally the depth buffer.
        /// </summary>
        /// <param name="color">Color to clear the color buffers with.</param>
        /// <param name="clearDepth">Clears the depth buffer if true.</param>
        public static void Clear( Color color, bool clearDepth )
        {
            Clear( color.R, color.G, color.B, color.A, clearDepth );
        }

        /// <summary>
        /// Clears the color buffers and optionally the depth buffer.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="clearDepth"> Clears the depth buffer if true.</param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public static void Clear( float r, float g, float b, float a, bool clearDepth = false )
        {
            Gdx.GL.GLClearColor( r, g, b, a );

            var mask = IGL20.GL_Color_Buffer_Bit;

            if ( clearDepth )
            {
                mask |= IGL20.GL_Depth_Buffer_Bit;
            }

            Gdx.GL.GLClear( mask );
        }

        /// <summary>
        /// Returns the current framebuffer contents as a <see cref="TextureRegion"/> with
        /// a width and height equal to the current screen size.
        /// <p>
        /// The base <see cref="Texture"/> always has <see cref="MathUtils.NextPowerOfTwo"/>
        /// dimensions and RGBA8888 <see cref="Pixmap.Format"/>. It can be accessed via
        /// <see cref="TextureRegion.Texture"/>. The texture is not managed and has to be
        /// reloaded manually on a context loss.
        /// </p>
        /// <p>
        /// The returned TextureRegion is flipped along the Y axis by default. 
        /// </p>
        /// </summary>
        public static TextureRegion FrameBufferTexture
        {
            get
            {
                if ( Gdx.Graphics == null ) throw new NullReferenceException();
                
                var w = Gdx.Graphics.GetBackBufferWidth();
                var h = Gdx.Graphics.GetBackBufferHeight();

                return GetFrameBufferTexture( 0, 0, w, h );
            }
        }

        /// <summary>
        /// Returns a portion of the current framebuffer contents specified by x, y,
        /// width and height as a <see cref="TextureRegion"/> with the same dimensions.
        /// <para>
        /// The base <see cref="Texture"/> always has <see cref="MathUtils.NextPowerOfTwo"/>
        /// dimensions and RGBA8888 <see cref="Pixmap.Format"/>. It can be accessed via
        /// <see cref="TextureRegion.Texture"/>.
        /// </para>
        /// <para>
        /// This texture is not managed and has to be reloaded manually on a context loss.
        /// If the width and height specified are larger than the framebuffer dimensions,
        /// the Texture will be padded accordingly.
        /// </para>
        /// <para>
        /// Pixels that fall outside of the current screen will have RGBA values of 0.
        /// </para>
        /// </summary>
        /// <param name="x"> the x position of the framebuffer contents to capture </param>
        /// <param name="y"> the y position of the framebuffer contents to capture </param>
        /// <param name="w"> the width of the framebuffer contents to capture </param>
        /// <param name="h"> the height of the framebuffer contents to capture  </param>
        public static TextureRegion GetFrameBufferTexture( int x, int y, int w, int h )
        {
            var potW = MathUtils.NextPowerOfTwo( w );
            var potH = MathUtils.NextPowerOfTwo( h );

            Pixmap pixmap    = Pixmap.CreateFromFrameBuffer( x, y, w, h );
            var    potPixmap = new Pixmap( potW, potH, Pixmap.Format.RGBA8888 );

            potPixmap.SetBlending( Pixmap.Blending.None );
            potPixmap.DrawPixmap( pixmap, 0, 0 );

            var texture       = new Texture( potPixmap );
            var textureRegion = new TextureRegion( texture, 0, h, w, -h );

            potPixmap.Dispose();
            pixmap.Dispose();

            return textureRegion;
        }

        [Obsolete( "use <see cref=\"Pixmap.CreateFromFrameBuffer(int, int, int, int)\"/> instead." )]
        public static Pixmap GetFrameBufferPixmap( int x, int y, int w, int h )
        {
            return Pixmap.CreateFromFrameBuffer( x, y, w, h );
        }

        /// <summary>
        /// Returns the current framebuffer contents as a byte[] array with a length equal
        /// to screen width * height * 4. The byte[] will always contain RGBA8888 data.
        /// Because of differences in screen and image origins the framebuffer contents
        /// should be flipped along the Y axis if you intend save them to disk as a bitmap.
        /// Flipping is not a cheap operation, so use this functionality wisely.
        /// </summary>
        /// <param name="flipY"> whether to flip pixels along Y axis</param>
        public static byte[] GetFrameBufferPixels( bool flipY )
        {
            if ( Gdx.Graphics == null ) throw new NullReferenceException();
            
            var w = Gdx.Graphics.GetBackBufferWidth();
            var h = Gdx.Graphics.GetBackBufferHeight();

            return GetFrameBufferPixels( 0, 0, w, h, flipY );
        }

        /// <summary>
        /// Returns a portion of the current framebuffer contents specified by x, y, width and
        /// height, as a byte[] array with a length equal to the specified width * height * 4.
        /// The byte[] will always contain RGBA8888 data. If the width and height specified
        /// are larger than the framebuffer dimensions, the Texture will be padded accordingly.
        /// Pixels that fall outside of the current screen will have RGBA values of 0. Because
        /// of differences in screen and image origins the framebuffer contents should be flipped
        /// along the Y axis if you intend save them to disk as a bitmap.
        /// <p>
        /// Flipping is not a cheap operation, so use this functionality wisely.
        /// </p>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="flipY"> whether to flip pixels along Y axis  </param>
        public static byte[] GetFrameBufferPixels( int x, int y, int w, int h, bool flipY )
        {
            var numBytes = w * h * 4;

            Gdx.GL.GLPixelStorei( IGL20.GL_Pack_Alignment, 1 );
            
            Buffer pixels = Utils.BufferUtils.NewByteBuffer( numBytes );
            
            Gdx.GL.GLReadPixels( x, y, w, h, IGL20.GL_Rgba, IGL20.GL_Unsigned_Byte, pixels );

            var lines = new byte[ numBytes ];

            if ( flipY )
            {
                var numBytesPerLine = w * 4;

                for ( var i = 0; i < h; i++ )
                {
                    pixels.Position = ( ( h - i - 1 ) * numBytesPerLine );

                    pixels.Get( lines, i * numBytesPerLine, numBytesPerLine );
                }
            }
            else
            {
                pixels.Clear();

                pixels.Read( lines );
            }

            return lines;
        }
    }
}
