using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class FrameBufferCubemapBuilder : GLFrameBufferBuilder< FrameBufferCubemap >
{
    public FrameBufferCubemapBuilder( int width, int height )
        : base( width, height )
    {
    }

    public override FrameBufferCubemap Build()
    {
        return new FrameBufferCubemap( this );
    }
}
