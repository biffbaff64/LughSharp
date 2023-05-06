using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics;

using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class TextureWrap
{
    public readonly static TextureWrap MirroredRepeat = new TextureWrap( "MirroredRepeat", InnerEnum.MirroredRepeat, IGL20.GL_Mirrored_Repeat );
    public readonly static TextureWrap ClampToEdge    = new TextureWrap( "ClampToEdge", InnerEnum.ClampToEdge, IGL20.GL_Clamp_To_Edge );
    public readonly static TextureWrap Repeat         = new TextureWrap( "Repeat", InnerEnum.Repeat, IGL20.GL_Repeat );

    private readonly static List< TextureWrap > valueList = new List< TextureWrap >();

    static TextureWrap()
    {
        valueList.Add( MirroredRepeat );
        valueList.Add( ClampToEdge );
        valueList.Add( Repeat );
    }

    public enum InnerEnum
    {
        MirroredRepeat,
        ClampToEdge,
        Repeat
    }

    public readonly  InnerEnum innerEnumValue;
    private readonly string    _nameValue;
    private static   int       _nextOrdinal = 0;

    public TextureWrap( string name, InnerEnum innerEnum, int glEnum )
    {
        this.GLEnum = glEnum;

        _nameValue     = name;
        OrdinalValue   = _nextOrdinal++;
        innerEnumValue = innerEnum;
    }

    public int GLEnum       { get; }
    public int OrdinalValue { get; set; }

    public static TextureWrap[] Values() => valueList.ToArray();

    public static TextureWrap ValueOf( string name )
    {
        foreach ( TextureWrap enumInstance in TextureWrap.valueList )
        {
            if ( enumInstance._nameValue == name )
            {
                return enumInstance;
            }
        }

        throw new System.ArgumentException( name );
    }

    public override string ToString()
    {
        return _nameValue;
    }
}