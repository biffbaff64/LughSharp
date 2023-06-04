using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class BaseDrawable : IDrawable
{
    public string? Name         { get; set; }
    public float   LeftWidth    { get; set; }
    public float   RightWidth   { get; set; }
    public float   TopHeight    { get; set; }
    public float   BottomHeight { get; set; }
    public float   MinWidth     { get; set; }
    public float   MinHeight    { get; set; }

    protected BaseDrawable()
    {
    }

    /// <summary>
    /// Creates a new empty drawable with the same sizing information as the specified drawable.
    /// </summary>
    public BaseDrawable( IDrawable? drawable )
    {
        ArgumentNullException.ThrowIfNull( drawable );

        if ( drawable is BaseDrawable baseDrawable ) Name = baseDrawable.Name;

        LeftWidth    = drawable.LeftWidth;
        RightWidth   = drawable.RightWidth;
        TopHeight    = drawable.TopHeight;
        BottomHeight = drawable.BottomHeight;
        MinWidth     = drawable.MinWidth;
        MinHeight    = drawable.MinHeight;
    }

    /// <summary>
    /// Draws this drawable at the specified bounds. The drawable should be tinted
    /// with <see cref="IBatch.GetColor"/>, possibly by mixing its own color. 
    /// </summary>
    public void Draw( IBatch batch, float x, float y, float width, float height )
    {
    }

    public void SetPadding( float topHeight, float leftWidth, float bottomHeight, float rightWidth )
    {
        TopHeight    = topHeight;
        LeftWidth    = leftWidth;
        BottomHeight = bottomHeight;
        RightWidth   = rightWidth;
    }
}
