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


namespace LughSharp.LibCore.Graphics.FrameBuffers;

/// <summary>
///     This is a <see cref="FrameBuffer" /> variant backed by a float texture.
/// </summary>
[PublicAPI]
public class FloatFrameBuffer : FrameBuffer
{
    public FloatFrameBuffer()
    {
    }

    /// <summary>
    ///     Creates a GLFrameBuffer from the specifications provided by bufferBuilder
    /// </summary>
    /// <param name="bufferBuilder"></param>
    public FloatFrameBuffer( GLFrameBufferBuilder< GLFrameBuffer< GLTexture > > bufferBuilder )
        : base( bufferBuilder )
    {
    }

    /// <summary>
    ///     Creates a new FrameBuffer with a float backing texture, having the given dimensions
    ///     and potentially a depth buffer attached.
    /// </summary>
    /// <param name="width"> the width of the framebuffer in pixels </param>
    /// <param name="height"> the height of the framebuffer in pixels </param>
    /// <param name="hasDepth"> whether to attach a depth buffer </param>
    /// <exception cref="GdxRuntimeException"> in case the FrameBuffer could not be created  </exception>
    public FloatFrameBuffer( int width, int height, bool hasDepth )
    {
        var bufferBuilder = new FrameBufferBuilder( width, height );

        bufferBuilder.AddFloatAttachment( IGL30.GL_RGBA32_F, IGL20.GL_RGBA, IGL20.GL_FLOAT, false );

        if ( hasDepth )
        {
            bufferBuilder.AddBasicDepthRenderBuffer();
        }

        BufferBuilder = bufferBuilder;

        Build();
    }

    protected override Texture CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
    {
        var data = new FloatTextureData( BufferBuilder.Width,
                                         BufferBuilder.Height,
                                         attachmentSpec.InternalFormat,
                                         attachmentSpec.Format,
                                         attachmentSpec.Type,
                                         attachmentSpec.IsGpuOnly );

        var result = new Texture( data );

        if ( Gdx.App.AppType == IApplication.ApplicationType.DesktopGL )
        {
            result.SetFilter( TextureFilter.Linear, TextureFilter.Linear );
        }
        else
        {
            // no filtering for float textures in OpenGL ES
            result.SetFilter( TextureFilter.Nearest, TextureFilter.Nearest );
        }

        result.SetWrap( TextureWrap.ClampToEdge, TextureWrap.ClampToEdge );

        return result;
    }
}
