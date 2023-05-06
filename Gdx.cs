using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp;

/// <summary>
/// Environment class holding references to the Application,
/// Graphics, Audio, Files and Input instances.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public static class Gdx
{
    public static bool DevMode { get; set; } = false;
    public static bool GodMode { get; set; } = false;

    [AllowNull] public static IApplication App      { get; set; }
    [AllowNull] public static IGraphics    Graphics { get; set; }
    [AllowNull] public static IAudio       Audio    { get; set; }
    [AllowNull] public static IInput       Input    { get; set; }
    [AllowNull] public static IFiles       Files    { get; set; }
    [AllowNull] public static INet         Net      { get; set; }
    [AllowNull] public static IGL20        GL       { get; set; }
    [AllowNull] public static IGL20        GL20     { get; set; }
    [AllowNull] public static IGL30        GL30     { get; set; }
}