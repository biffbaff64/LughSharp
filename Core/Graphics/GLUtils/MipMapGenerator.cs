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

namespace LibGDXSharp.Graphics.GLUtils;

[PublicAPI]
public class MipMapGenerator
{
    private MipMapGenerator()
    {
    }

    public static bool UseHwMipMap { get; set; } = true;

    /// <summary>
    /// Sets the image data of the <see cref="Texture"/> based on the <see cref="Pixmap"/>.
    /// The texture must be bound for this to work. If <code>disposePixmap</code> is true,
    /// the pixmap will be disposed at the end of the method.
    /// </summary>
    /// <param name="pixmap"> the Pixmap </param>
    /// <param name="textureWidth"></param>
    /// <param name="textureHeight"></param>
    public static void GenerateMipMap( Pixmap pixmap, int textureWidth, int textureHeight )
    {
        GenerateMipMap( IGL20.GL_TEXTURE_2D, pixmap, textureWidth, textureHeight );
    }

    /// <summary>
    /// Sets the image data of the <see cref="Texture"/> based on the <see cref="Pixmap"/>.
    /// The texture must be bound for this to work. If <code>disposePixmap</code> is true,
    /// the pixmap will be disposed at the end of the method.
    /// </summary>
    public static void GenerateMipMap( int target, Pixmap pixmap, int textureWidth, int textureHeight )
    {
        if ( !UseHwMipMap )
        {
            GenerateMipMapCPU( target, pixmap, textureWidth, textureHeight );

            return;
        }

        if ( ( Gdx.App.AppType == IApplication.ApplicationType.Android )
          || ( Gdx.App.AppType == IApplication.ApplicationType.WebGL )
          || ( Gdx.App.AppType == IApplication.ApplicationType.IOS ) )
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
        Gdx.GL.GLTexImage2D
            (
             target, 0, pixmap.GLInternalFormat, pixmap.Width, pixmap.Height, 0,
             pixmap.GLFormat, pixmap.GLType, pixmap.Pixels
            );

        Gdx.GL20.GLGenerateMipmap( target );
    }

    private static void GenerateMipMapDesktop( int target, Pixmap pixmap, int textureWidth, int textureHeight )
    {
        if ( Gdx.Graphics.SupportsExtension( "GL_ARB_framebuffer_object" )
          || Gdx.Graphics.SupportsExtension( "GL_EXT_framebuffer_object" )
          || ( Gdx.GL30 != null ) )
        {
            Gdx.GL.GLTexImage2D
                (
                 target, 0, pixmap.GLInternalFormat, pixmap.Width, pixmap.Height, 0,
                 pixmap.GLFormat, pixmap.GLType, pixmap.Pixels
                );

            Gdx.GL20.GLGenerateMipmap( target );
        }
        else
        {
            GenerateMipMapCPU( target, pixmap, textureWidth, textureHeight );
        }
    }

    private static void GenerateMipMapCPU( int target, Pixmap pixmap, int textureWidth, int textureHeight )
    {
        Gdx.GL.GLTexImage2D
            (
             target, 0, pixmap.GLInternalFormat, pixmap.Width, pixmap.Height, 0,
             pixmap.GLFormat, pixmap.GLType, pixmap.Pixels
            );

        if ( ( Gdx.GL20 == null ) && ( textureWidth != textureHeight ) )
        {
            throw new GdxRuntimeException( "texture width and height must be square when using mipmapping." );
        }

        var width  = pixmap.Width / 2;
        var height = pixmap.Height / 2;
        var level  = 1;

        while ( ( width > 0 ) && ( height > 0 ) )
        {
            var tmp = new Pixmap( width, height, pixmap.GetFormat() );

            tmp.Blend = Pixmap.Blending.None;
            tmp.DrawPixmap( pixmap, 0, 0, pixmap.Width, pixmap.Height, 0, 0, width, height );

            if ( level > 1 )
            {
                pixmap.Dispose();
            }
            
            pixmap = tmp;

            Gdx.GL.GLTexImage2D
                (
                 target, level, pixmap.GLInternalFormat, pixmap.Width, pixmap.Height, 0,
                 pixmap.GLFormat, pixmap.GLType, pixmap.Pixels
                );

            width  = pixmap.Width / 2;
            height = pixmap.Height / 2;
            level++;
        }
    }
}
