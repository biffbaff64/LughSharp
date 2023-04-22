namespace LibGDXSharp.Utils.Viewport
{
    /// <summary>
    /// A ScalingViewport that uses <seealso cref="Scaling.Stretch"/> so it does not
    /// keep the aspect ratio, the world is scaled to take the whole screen.
    /// </summary>
    public class StretchViewport : ScalingViewport
    {
        /// <summary>
        /// Creates a new viewport using a new <seealso cref="OrthographicCamera"/>. </summary>
        public StretchViewport( float worldWidth, float worldHeight )
            : base( Scaling.Stretch, worldWidth, worldHeight )
        {
        }

        public StretchViewport( float worldWidth, float worldHeight, Camera camera )
            : base( Scaling.Stretch, worldWidth, worldHeight, camera )
        {
        }
    }
}
