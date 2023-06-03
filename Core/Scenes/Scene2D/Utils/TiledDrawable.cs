using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class TiledDrawable : IDrawable
{
    public string Name         { get; set; } = "";
    public float  Scale        { get; set; }
    public float  LeftWidth    { get; set; }
    public float  RightWidth   { get; set; }
    public float  TopHeight    { get; set; }
    public float  BottomHeight { get; set; }
    public float  MinWidth     { get; set; }
    public float  MinHeight    { get; set; }

    public TiledDrawable( TextureRegion getRegion )
    {
    }

    /// <summary>
    /// Draws this drawable at the specified bounds. The drawable should be tinted
    /// with <seealso cref="IBatch.GetColor"/>, possibly by mixing its own color. 
    /// </summary>
    public void Draw( IBatch batch, float x, float y, float width, float height )
    {
    }
}
