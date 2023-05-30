using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class FrameBufferRenderBufferAttachmentSpec
{
    public int InternalFormat { get; set; }

    public FrameBufferRenderBufferAttachmentSpec( int internalFormat )
    {
        this.InternalFormat = internalFormat;
    }
}