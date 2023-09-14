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

namespace LibGDXSharp.Graphics.FrameBuffers;

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

    // LibGDXSharp.Graphics.FrameBuffers.GLFrameBufferBuilder<LibGDXSharp.Graphics.FrameBuffers.GLFrameBuffer<LibGDXSharp.Graphics.GLTexture>>
    // LibGDXSharp.Graphics.FrameBuffers.GLFrameBufferBuilder<LibGDXSharp.Graphics.FrameBuffers.GLFrameBuffer<LibGDXSharp.Graphics.Texture>>
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
    public FrameBuffer( Pixmap.Format format, int width, int height, bool hasDepth, bool hasStencil = false )
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

        this.BufferBuilder = frameBufferBuilder;

        Build();
    }

    protected override Texture CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
    {
        var data = new GLOnlyTextureData
            (
            BufferBuilder.Width,
            BufferBuilder.Height,
            0,
            attachmentSpec.InternalFormat,
            attachmentSpec.Format,
            attachmentSpec.Type
            );

        var result = new Texture( data );

        result.SetFilter( TextureFilter.Linear, TextureFilter.Linear );
        result.SetWrap( TextureWrap.ClampToEdge, TextureWrap.ClampToEdge );

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
        Gdx.GL20.GLFramebufferTexture2D
            (
            IGL20.GL_FRAMEBUFFER, IGL20.GL_COLOR_ATTACHMENT0,
            IGL20.GL_TEXTURE_2D, texture.GetTextureObjectHandle(), 0
            );
    }

    /// <summary>
    /// See <see cref="GLFrameBuffer{T}.Unbind()"/>
    /// </summary>
    public new void Unbind()
    {
        base.Unbind();
    }
}
