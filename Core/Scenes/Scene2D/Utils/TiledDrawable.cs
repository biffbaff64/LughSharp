using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class TiledDrawable : BaseDrawable
{
    public float Scale { get; set; }

    public TiledDrawable( TextureRegion getRegion )
    {
    }
}
