namespace LibGDXSharp.Core
{
    /// <summary>
    /// Environment class holding references to the Application,
    /// Graphics, Audio, Files and Input instances.
    /// </summary>
    public static class Gdx
    {
        public static Application App      { get; set; } = null!;
        public static IGraphics   Graphics { get; set; } = null!;
        public static IAudio      Audio    { get; set; } = null!;
        public static IInput      Input    { get; set; } = null!;
        public static IFile       Files    { get; set; } = null!;
        public static INet        Net      { get; set; } = null!;
        public static IGL20       Gl       { get; set; } = null!;
        public static IGL20       Gl20     { get; set; } = null!;
        public static IGL30       Gl30     { get; set; } = null!;
    }
}
