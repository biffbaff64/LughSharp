﻿// ///////////////////////////////////////////////////////////////////////////////
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

using Corelib.Lugh.Graphics.GLUtils;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Graphics.FrameBuffers;

/// <summary>
/// Encapsulates OpenGL ES 2.0 frame buffer objects. This is a simple helper
/// class which should cover most FBO uses. It will automatically create a
/// texture for the color attachment and a renderbuffer for the depth buffer.
/// You can get a hold of the texture by <see cref="GLFrameBuffer{T}.GetColorBufferTexture"/>.
/// This class will only work with OpenGL ES 2.0.
/// <para>
/// FrameBuffers are managed. In case of an OpenGL context loss, which only happens
/// on Android when a user switches to another application or receives an incoming
/// call, the framebuffer will be automatically recreated.
/// </para>
/// <para>
/// A FrameBuffer must be disposed if it is no longer needed.
/// </para>
/// </summary>
[PublicAPI]
public class FrameBuffer : GLFrameBuffer< Texture >
{
    public FrameBuffer()
    {
    }

    /// <summary>
    /// Creates a GLFrameBuffer from the specifications provided by bufferBuilder
    /// </summary>
    /// <param name="bufferBuilder"></param>
    public FrameBuffer( GLFrameBufferBuilder< GLFrameBuffer< GLTexture > > bufferBuilder )
        : base( bufferBuilder )
    {
    }

    /// <summary>
    /// Creates a new FrameBuffer having the given dimensions and potentially a
    /// depth and a stencil buffer attached.
    /// </summary>
    /// <param name="format">
    /// the format of the color buffer; according to the OpenGL ES 2.0 spec,
    /// only RGB565, RGBA4444 and RGB5_A1 are color-renderable.
    /// </param>
    /// <param name="width"> the width of the framebuffer in pixels </param>
    /// <param name="height"> the height of the framebuffer in pixels </param>
    /// <param name="hasDepth"> whether to attach a depth buffer </param>
    /// <param name="hasStencil"></param>
    /// <exception cref="GdxRuntimeException"> in case the FrameBuffer could not be created  </exception>
    public FrameBuffer( Pixmap.ColorFormat format, int width, int height, bool hasDepth, bool hasStencil = false )
    {
        var frameBufferBuilder = new FrameBufferBuilder( width, height );

        frameBufferBuilder.AddBasicColorTextureAttachment( format );

        if ( hasDepth )
        {
            frameBufferBuilder.AddBasicDepthRenderBuffer();
        }

        if ( hasStencil )
        {
            frameBufferBuilder.AddBasicStencilRenderBuffer();
        }

        BufferBuilder = frameBufferBuilder;

        BuildBuffer();
    }

    protected override Texture CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
    {
        var data = new GLOnlyTextureData( BufferBuilder.Width,
                                          BufferBuilder.Height,
                                          0,
                                          attachmentSpec.InternalFormat,
                                          attachmentSpec.Format,
                                          attachmentSpec.Type );

        var result = new Texture( data );

        result.SetFilter( Texture.TextureFilter.Linear, Texture.TextureFilter.Linear );
        result.SetWrap( Texture.TextureWrap.ClampToEdge, Texture.TextureWrap.ClampToEdge );

        return result;
    }

    /// <summary>
    /// Override this method in a derived class to dispose the
    /// backing texture as you like.
    /// </summary>
    protected override void DisposeColorTexture( Texture colorTexture )
    {
        colorTexture.Dispose();
    }

    /// <summary>
    /// Override this method in a derived class to attach the backing
    /// texture to the GL framebuffer object.
    /// </summary>
    protected override void AttachFrameBufferColorTexture( Texture texture )
    {
        GdxApi.Bindings.FramebufferTexture2D( IGL.GL_FRAMEBUFFER,
                                       IGL.GL_COLOR_ATTACHMENT0,
                                       IGL.GL_TEXTURE_2D,
                                       ( uint ) texture.GLTextureHandle,
                                       0 );
    }
}