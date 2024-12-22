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

using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils.Buffers;

namespace Corelib.Lugh.Utils;

/// <summary>
/// Class with static helper methods related to currently bound OpenGL frame buffer,
/// including access to the current OpenGL FrameBuffer. These methods can be used to
/// get the entire screen content or a portion thereof.
/// </summary>
[PublicAPI]
public class ScreenUtils
{
    /// <summary>
    /// Returns the current framebuffer contents as a <see cref="TextureRegion"/> with
    /// a width and height equal to the current screen size.
    /// <para>
    /// The base <see cref="Texture"/> always has <see cref="MathUtils.NextPowerOfTwo"/>
    /// dimensions and RGBA8888 <see cref="Pixmap.ColorFormat"/>. It can be accessed via
    /// <see cref="TextureRegion.Texture"/>. The texture is not managed and has to be
    /// reloaded manually on a context loss.
    /// </para>
    /// <para>
    /// The returned TextureRegion is flipped along the Y axis by default.
    /// </para>
    /// </summary>
    public static TextureRegion FrameBufferTexture
    {
        get
        {
            if ( GdxApi.Graphics == null )
            {
                throw new NullReferenceException();
            }

            var w = GdxApi.Graphics.BackBufferWidth;
            var h = GdxApi.Graphics.BackBufferHeight;

            return GetFrameBufferTexture( 0, 0, w, h );
        }
    }

    /// <summary>
    /// Clears the color buffers and optionally the depth buffer.
    /// </summary>
    /// <param name="color">Color to clear the color buffers with.</param>
    /// <param name="clearDepth">Clears the depth buffer if true.</param>
    public static void Clear( Color color, bool clearDepth = false )
    {
        Clear( color.R, color.G, color.B, color.A, clearDepth );
    }

    /// <summary>
    /// Clears the color buffers and optionally the depth buffer.
    /// </summary>
    /// <param name="a"> Alpha component. </param>
    /// <param name="clearDepth"> Clears the depth buffer if true.</param>
    /// <param name="r"> Red component. </param>
    /// <param name="g"> Green component. </param>
    /// <param name="b"> Blue component. </param>
    public static void Clear( float r, float g, float b, float a, bool clearDepth = false )
    {
        GdxApi.Bindings.ClearColor( r, g, b, a );

        var mask = ( uint ) IGL.GL_COLOR_BUFFER_BIT;

        if ( clearDepth )
        {
            mask |= IGL.GL_DEPTH_BUFFER_BIT;
        }

        GdxApi.Bindings.Clear( mask );
    }

    /// <summary>
    /// Returns a portion of the current framebuffer contents specified by x, y,
    /// width and height as a <see cref="TextureRegion"/> with the same dimensions.
    /// <para>
    /// The base <see cref="Texture"/> always has <see cref="MathUtils.NextPowerOfTwo"/>
    /// dimensions and RGBA8888 <see cref="Pixmap.ColorFormat"/>. It can be accessed via
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
        var potW      = MathUtils.NextPowerOfTwo( w );
        var potH      = MathUtils.NextPowerOfTwo( h );
        var pixmap    = Pixmap.CreateFromFrameBuffer( x, y, w, h );
        var potPixmap = new Pixmap( potW, potH, Pixmap.ColorFormat.RGBA8888 );

        potPixmap.Blending = Pixmap.BlendTypes.None;
        potPixmap.DrawPixmap( pixmap, 0, 0 );

        var texture       = new Texture( potPixmap );
        var textureRegion = new TextureRegion( texture, 0, h, w, -h );

        potPixmap.Dispose();
        pixmap.Dispose();

        return textureRegion;
    }

    /// <inheritdoc cref="Pixmap.CreateFromFrameBuffer"/> 
    public static Pixmap GetFrameBufferPixmap( int x, int y, int w, int h )
    {
        return Pixmap.CreateFromFrameBuffer( x, y, w, h );
    }

    /// <summary>
    /// Returns the current framebuffer contents as a byte[] array with a length equal
    /// to screen width * height * 4. The byte[] will always contain RGBA8888 data.
    /// Because of differences in screen and image origins the framebuffer contents should
    /// be flipped along the Y axis if you intend save them to disk as a bitmap. Flipping
    /// is not a cheap operation, so use this functionality wisely.
    /// </summary>
    /// <param name="flipY"> whether to flip pixels along Y axis</param>
    public static byte[] GetFrameBufferPixels( bool flipY )
    {
        if ( GdxApi.Graphics == null )
        {
            throw new NullReferenceException();
        }

        var w = GdxApi.Graphics.BackBufferWidth;
        var h = GdxApi.Graphics.BackBufferHeight;

        return GetFrameBufferPixels( 0, 0, w, h, flipY );
    }

    /// <summary>
    /// Returns a portion of the current framebuffer contents specified by x, y, width and height,
    /// as a byte[] array with a length equal to the specified width * height * 4. The byte[] will
    /// always contain RGBA8888 data. If the width and height specified are larger than the framebuffer
    /// dimensions, the Texture will be padded accordingly. Pixels that fall outside of the current
    /// screen will have RGBA values of 0. Because of differences in screen and image origins the
    /// framebuffer contents should be flipped along the Y axis if you intend save them to disk as
    /// a bitmap.
    /// <para>
    /// Flipping is not a cheap operation, so use this functionality wisely.
    /// </para>
    /// </summary>
    /// <param name="x"> Portion X position. </param>
    /// <param name="y"> Portion Y position. </param>
    /// <param name="w"> Portion Width. </param>
    /// <param name="h"> Portion Height. </param>
    /// <param name="flipY"> whether to flip pixels along Y axis  </param>
    public static unsafe byte[] GetFrameBufferPixels( int x, int y, int w, int h, bool flipY )
    {
        var numBytes = w * h * 4;

        GdxApi.Bindings.PixelStorei( IGL.GL_PACK_ALIGNMENT, 1 );

        var pixels = BufferUtils.NewByteBuffer( numBytes );
        
        fixed ( void* ptr = &pixels.Hb!.ToArray()[ 0 ] )
        {
            GdxApi.Bindings.ReadPixels( x, y, w, h, IGL.GL_RGBA, IGL.GL_UNSIGNED_BYTE, ptr );
        }

        var lines = new byte[ numBytes ];

        if ( flipY )
        {
            var numBytesPerLine = w * 4;

            for ( var i = 0; i < h; i++ )
            {
                pixels.Position = ( h - i - 1 ) * numBytesPerLine;

                pixels.Get( lines, i * numBytesPerLine, numBytesPerLine );
            }
        }
        else
        {
            pixels.Clear();

            pixels.Get( lines );
        }

        return lines;
    }
}
