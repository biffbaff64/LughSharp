using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// A drawable that supports scale and rotation.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public interface ITransformDrawable : IDrawable
{
    void Draw( IBatch batch,
               float x,
               float y,
               float originX,
               float originY,
               float width,
               float height,
               float scaleX,
               float scaleY,
               float rotation );
}