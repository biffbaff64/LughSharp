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

    public static IApplication App      { get; set; }
    public static IGraphics    Graphics { get; set; }
    public static IAudio       Audio    { get; set; }
    public static IInput       Input    { get; set; }
    public static IFiles       Files    { get; set; }
    public static INet         Net      { get; set; }
    public static IGL20        GL       { get; set; }
    public static IGL20        GL20     { get; set; }
    public static IGL30        GL30     { get; set; }
}

public abstract class AppBase : IApplication
{
}