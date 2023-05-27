using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;

namespace LibGDXSharp.Scenes.Scene2D.UI;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class Skin
{
    public Dictionary< Type, Dictionary< string, object > > Resources { get; set; }

    public TextureAtlas Atlas { get; set; }
    public float        Scale { get; set; } = 1;

    public Skin( TextureAtlas atlas )
    {
    }

    public void Load( FileInfo? file )
    {
    }

    public void Add( string key, object value )
    {
    }
}