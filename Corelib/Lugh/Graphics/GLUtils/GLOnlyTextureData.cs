// ///////////////////////////////////////////////////////////////////////////////
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

using Corelib.Lugh.Graphics.FrameBuffers;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Graphics.GLUtils;

/// <summary>
/// A <see cref="ITextureData"/> implementation which should be used to create
/// GL only textures.
/// This TextureData fits perfectly for <see cref="FrameBuffer"/>s.
/// The data is not managed.
/// </summary>
[PublicAPI]
public class GLOnlyTextureData : ITextureData
{
    public int                MipLevel       { get; set; } = 0;
    public int                InternalFormat { get; set; }
    public Pixmap.ColorFormat Format         { get; set; }
    public int                Type           { get; set; }
    public int                Width          { get; set; } = 0;
    public int                Height         { get; set; } = 0;
    public bool               IsPrepared     { get; set; } = false;
    public bool               UseMipMaps     { get; set; }

    // ========================================================================

    /// <summary>
    /// See <a href="https://www.khronos.org/opengles/sdk/docs/man/xhtml/glTexImage2D.xml">glTexImage2D</a>
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="mipMapLevel"></param>
    /// <param name="internalFormat">
    /// Specifies the internal format of the texture. Must be one of the following symbolic constants:
    /// <see cref="IGL.GL_ALPHA"/>, <see cref="IGL.GL_LUMINANCE"/>, <see cref="IGL.GL_LUMINANCE_ALPHA"/>,
    /// <see cref="IGL.GL_RGB"/>, <see cref="IGL.GL_RGBA"/>.
    /// </param>
    /// <param name="format">
    /// Specifies the format of the texel data. Must match internalformat.
    /// The following symbolic values are accepted:
    /// <see cref="IGL.GL_ALPHA"/>, <see cref="IGL.GL_RGB"/>, <see cref="IGL.GL_RGBA"/>,
    /// <see cref="IGL.GL_LUMINANCE"/>, and <see cref="IGL.GL_LUMINANCE_ALPHA"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the texel data. The following symbolic values are accepted:
    /// <see cref="IGL.GL_UNSIGNED_BYTE"/>, <see cref="IGL.GL_UNSIGNED_SHORT_5_6_5"/>,
    /// <see cref="IGL.GL_UNSIGNED_SHORT_4_4_4_4"/>, and <see cref="IGL.GL_UNSIGNED_SHORT_5_5_5_1"/>.
    /// </param>
    public GLOnlyTextureData( int width,
                              int height,
                              int mipMapLevel,
                              int internalFormat,
                              int format,
                              int type )
    {
        Width          = width;
        Height         = height;
        MipLevel       = mipMapLevel;
        InternalFormat = internalFormat;
        Format         = PixmapFormat.ToPixmapColorFormat( format );
        Type           = type;
    }

    public void Prepare()
    {
        if ( IsPrepared )
        {
            throw new GdxRuntimeException( "Already prepared" );
        }

        IsPrepared = true;
    }

    public unsafe void ConsumeCustomData( int target )
    {
        Gdx.GL.glTexImage2D( target,
                             MipLevel,
                             InternalFormat,
                             Width,
                             Height,
                             0,
                             PixmapFormat.ToGdx2DFormat( Format ),
                             Type,
                             null! );
    }

    /// <summary>
    /// Returns the <see cref="Pixmap.ColorFormat"/> for this GLOnlyTextureData object.
    /// </summary>
    public Pixmap.ColorFormat GetFormat()
    {
        return Pixmap.ColorFormat.RGBA8888;
    }

    /// <summary>
    /// GLOnlyTextureData objects are not Managed.
    /// </summary>
    public bool IsManaged
    {
        get => false;
        set { }
    }

    /// <summary>
    /// Returns the <see cref="ITextureData.TextureType"/> for this Texture Data.
    /// </summary>
    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Custom;

    // ========================================================================
    // ========================================================================

    public Pixmap ConsumePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    public bool ShouldDisposePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }
}