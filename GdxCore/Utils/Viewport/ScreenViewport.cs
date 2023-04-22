namespace LibGDXSharp.Utils.Viewport
{
    /// <summary>
    /// A viewport where the world size is based on the size of the screen.
    /// By default 1 world unit == 1 screen pixel, but this ratio can be
    /// changed by modifying <see cref="UnitsPerPixel"/>.
    /// </summary>
    public class ScreenViewport : Viewport
    {
        public float UnitsPerPixel { get; set; } = 1;

        /// <summary>
        /// Creates a new viewport using a new <see cref="OrthographicCamera"/>. </summary>
        public ScreenViewport() : this( new OrthographicCamera() )
        {
        }

        public ScreenViewport( Camera camera )
        {
            Camera = camera;
        }

        public virtual new void Update( int screenWidth, int screenHeight, bool centerCamera )
        {
            SetScreenBounds( 0, 0, screenWidth, screenHeight );
            SetWorldSize( screenWidth * UnitsPerPixel, screenHeight * UnitsPerPixel );
            Apply( centerCamera );
        }
    }
}
