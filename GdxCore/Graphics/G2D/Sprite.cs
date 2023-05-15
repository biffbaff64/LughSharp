using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Sprite
{
    public readonly static int VertexSize = 2 + 1 + 2;
    public readonly static int SpriteSize = 4 * VertexSize;

    public Texture Texture  { get; set; }
    public float[] Vertices { get; set; }

    public Sprite( AtlasRegion region )
    {
    }

    public void SetBounds( int i, int i1, int regionRegionHeight, int regionRegionWidth )
    {
    }

    public void Rotate90( bool b )
    {
    }
}