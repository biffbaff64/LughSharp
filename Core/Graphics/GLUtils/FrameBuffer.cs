using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// Encapsulates OpenGL ES 2.0 frame buffer objects. This is a simple helper
/// class which should cover most FBO uses. It will automatically create a
/// texture for the color attachment and a renderbuffer for the depth buffer.
/// You can get a hold of the texture by <see cref="GetColorBufferTexture()"/>.
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
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class FrameBuffer : GLFrameBuffer< Texture >
{
    private FrameBuffer()
    {
    }

    /// <summary>
    /// Creates a GLFrameBuffer from the specifications provided by bufferBuilder
    /// </summary>
    /// <param name="bufferBuilder">
    ///  </param>
    protected FrameBuffer( GLFrameBufferBuilder< GLFrameBuffer< Texture > > bufferBuilder )
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
        FrameBufferBuilder frameBufferBuilder = new(width, height);
        frameBufferBuilder.AddBasicColorTextureAttachment( format );

        if ( hasDepth )
        {
            frameBufferBuilder.AddBasicDepthRenderBuffer();
        }

        if ( hasStencil )
        {
            frameBufferBuilder.AddBasicStencilRenderBuffer();
        }

        this.bufferBuilder = frameBufferBuilder;

        Build();
    }

    protected Texture CreateTexture( FrameBufferTextureAttachmentSpec attachmentSpec )
    {
        var data = new GLOnlyTextureData
            (
             bufferBuilder.width,
             bufferBuilder.height,
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

    protected void DisposeColorTexture( Texture colorTexture )
    {
        colorTexture.Dispose();
    }

    protected void AttachFrameBufferColorTexture( Texture texture )
    {
        Gdx.GL20.GLFramebufferTexture2D
            (
             IGL20.GL_Framebuffer,
             IGL20.GL_Color_Attachment0,
             IGL20.GL_Texture_2D,
             texture.GLHandle,
             0
            );
    }

    /// <summary>
    /// See <see cref="GLFrameBuffer.Unbind()"/>
    /// </summary>
    public static void Unbind()
    {
        GLFrameBuffer<Texture>.Unbind();
    }
}