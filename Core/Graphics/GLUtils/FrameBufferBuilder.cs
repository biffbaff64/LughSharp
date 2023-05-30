using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class FrameBufferBuilder : GLFrameBufferBuilder< FrameBuffer >
{
    public FrameBufferBuilder( int width, int height )
        : base( width, height )
    {
    }

    public override FrameBuffer Build()
    {
        return new FrameBuffer( this );
    }
}