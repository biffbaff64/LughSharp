using LibGDXSharp.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// A drawable knows how to draw itself at a given rectangular size. It provides
/// padding sizes and a minimum size so that other code can determine how to size
/// and position content.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Draws this drawable at the specified bounds. The drawable should be tinted
    /// with <see cref="IBatch.GetColor"/>, possibly by mixing its own color. 
    /// </summary>
    void Draw( IBatch batch, float x, float y, float width, float height );

    float LeftWidth { get; set; }

    float RightWidth { get; set; }

    float TopHeight { get; set; }

    float BottomHeight { get; set; }

    float MinWidth { get; set; }

    float MinHeight { get; set; }
}