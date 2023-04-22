using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Core
{
    /// <summary>
    /// Environment class holding references to the Application,
    /// Graphics, Audio, Files and Input instances.
    /// </summary>
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public static class Gdx
    {
        public static bool DevMode { get; set; } = false;
        public static bool GodMode { get; set; } = false;

        public static IApplication App      { get; set; } = null!;
        public static IGraphics    Graphics { get; set; } = null!;
        public static IAudio       Audio    { get; set; } = null!;
        public static IInput       Input    { get; set; } = null!;
        public static IFiles       Files    { get; set; } = null!;
        public static INet         Net      { get; set; } = null!;
        public static IGL20        Gl       { get; set; } = null!;
        public static IGL20        Gl20     { get; set; } = null!;
        public static IGL30        Gl30     { get; set; } = null!;
    }
}
