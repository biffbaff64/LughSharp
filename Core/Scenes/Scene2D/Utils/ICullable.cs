using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Maths;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// Allows a parent to set the area that is visible on a child actor to allow the
/// child to cull when drawing itself. This must only be used for actors that are
/// not rotated or scaled.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public interface ICullable
{
    /// <summary>
    /// <param name="value"> The culling area in the child actor's coordinates. </param>
    /// </summary>
    RectangleShape CullingArea {set;}
}
