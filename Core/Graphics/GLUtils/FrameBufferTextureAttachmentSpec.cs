using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class FrameBufferTextureAttachmentSpec
{
    public int InternalFormat { get; }
    public int Format         { get; }
    public int Type           { get; }

    public bool IsFloat   { get; init; }
    public bool IsGpuOnly { get; init; }
    public bool IsDepth   { get; init; }
    public bool IsStencil { get; init; }

    public FrameBufferTextureAttachmentSpec( int internalformat, int format, int type )
    {
        this.InternalFormat = internalformat;
        this.Format         = format;
        this.Type           = type;
    }

    public bool IsColorTexture => ( !IsDepth && !IsStencil );
}