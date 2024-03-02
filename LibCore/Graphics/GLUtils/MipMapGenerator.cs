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


namespace LibGDXSharp.LibCore.Graphics.GLUtils;

public class MipMapGenerator
{
    private MipMapGenerator()
    {
    }

    public static bool UseHwMipMap { get; set; } = true;

    /// <summary>
    ///     Sets the image data of the <see cref="Texture" /> based on the <see cref="Pixmap" />.
    ///     The texture must be bound for this to work. If <code>disposePixmap</code> is true,
    ///     the pixmap will be disposed at the end of the method.
    /// </summary>
    /// <param name="pixmap"> the Pixmap </param>
    /// <param name="textureWidth"></param>
    /// <param name="textureHeight"></param>
    public static void GenerateMipMap( Pixmap pixmap, int textureWidth, int textureHeight )
        => GenerateMipMap( IGL20.GL_TEXTURE_2D, pixmap, textureWidth, textureHeight );

    /// <summary>
    ///     Sets the image data of the <see cref="Texture" /> based on the <see cref="Pixmap" />.
    ///     The texture must be bound for this to work. If <code>disposePixmap</code> is true,
    ///     the pixmap will be disposed at the end of the method.
    /// </summary>
    public static void GenerateMipMap( int target, Pixmap pixmap, int textureWidth, int textureHeight )
    {
        if ( !UseHwMipMap )
        {
            GenerateMipMapCPU( target, pixmap, textureWidth, textureHeight );

            return;
        }

        if ( ( Core.Gdx.App.AppType == IApplication.ApplicationType.Android )
          || ( Core.Gdx.App.AppType == IApplication.ApplicationType.WebGL )
          || ( Core.Gdx.App.AppType == IApplication.ApplicationType.IOS ) )
        {
            GenerateMipMapGLES20( target, pixmap );
        }
        else
        {
            GenerateMipMapDesktop( target, pixmap, textureWidth, textureHeight );
        }
    }

    private static void GenerateMipMapGLES20( int target, Pixmap pixmap )
    {
        Core.Gdx.GL.GLTexImage2D(
            target,
            0,
            pixmap.GLInternalFormat,
            pixmap.Width,
            pixmap.Height,
            0,
            pixmap.GLFormat,
            pixmap.GLType,
            pixmap.Pixels
            );

        Core.Gdx.GL20.GLGenerateMipmap( target );
    }

    private static void GenerateMipMapDesktop( int target, Pixmap pixmap, int textureWidth, int textureHeight )
    {
        if ( Core.Gdx.Graphics.SupportsExtension( "GL_ARB_framebuffer_object" )
          || Core.Gdx.Graphics.SupportsExtension( "GL_EXT_framebuffer_object" )
          || ( Core.Gdx.GL30 != null ) )
        {
            Core.Gdx.GL.GLTexImage2D(
                target,
                0,
                pixmap.GLInternalFormat,
                pixmap.Width,
                pixmap.Height,
                0,
                pixmap.GLFormat,
                pixmap.GLType,
                pixmap.Pixels
                );

            Core.Gdx.GL20.GLGenerateMipmap( target );
        }
        else
        {
            GenerateMipMapCPU( target, pixmap, textureWidth, textureHeight );
        }
    }

    private static void GenerateMipMapCPU( int target, Pixmap pixmap, int textureWidth, int textureHeight )
    {
        Core.Gdx.GL.GLTexImage2D(
            target,
            0,
            pixmap.GLInternalFormat,
            pixmap.Width,
            pixmap.Height,
            0,
            pixmap.GLFormat,
            pixmap.GLType,
            pixmap.Pixels
            );

        if ( ( Core.Gdx.GL20 == null ) && ( textureWidth != textureHeight ) )
        {
            throw new GdxRuntimeException( "texture width and height must be square when using mipmapping." );
        }

        var width  = pixmap.Width / 2;
        var height = pixmap.Height / 2;
        var level  = 1;

        while ( ( width > 0 ) && ( height > 0 ) )
        {
            var tmp = new Pixmap( width, height, pixmap.GetFormat() );

            tmp.Blending = Pixmap.BlendTypes.None;
            tmp.DrawPixmap( pixmap, 0, 0, pixmap.Width, pixmap.Height, 0, 0, width, height );

            if ( level > 1 )
            {
                pixmap.Dispose();
            }

            pixmap = tmp;

            Core.Gdx.GL.GLTexImage2D(
                target,
                level,
                pixmap.GLInternalFormat,
                pixmap.Width,
                pixmap.Height,
                0,
                pixmap.GLFormat,
                pixmap.GLType,
                pixmap.Pixels
                );

            width  = pixmap.Width / 2;
            height = pixmap.Height / 2;
            level++;
        }
    }
}
