using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics.GLUtils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class FloatFrameBufferBuilder : GLFrameBufferBuilder< FloatFrameBuffer >
{
    public FloatFrameBufferBuilder( int width, int height )
        : base( width, height )
    {
    }

    public override FloatFrameBuffer Build()
    {
        return new FloatFrameBuffer( this );
    }
}