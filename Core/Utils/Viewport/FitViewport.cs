namespace LibGDXSharp.Utils.Viewport;

/// <summary>
/// A ScalingViewport that uses <see cref="Scaling.Fit"/> so it keeps the aspect
/// ratio by scaling the world up to fit the screen, adding black bars (letterboxing)
/// for the remaining space.
/// </summary>
public class FitViewport : ScalingViewport
{
    /// <summary>
    /// Creates a new viewport using a new <see cref="OrthographicCamera"/>.
    /// </summary>
    public FitViewport(float worldWidth, float worldHeight)
        : base(Scaling.Fit, worldWidth, worldHeight)
    {
    }

    public FitViewport(float worldWidth, float worldHeight, Camera camera)
        : base(Scaling.Fit, worldWidth, worldHeight, camera)
    {
    }
}