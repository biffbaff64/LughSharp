namespace LibGDXSharp.Utils.Viewport;

/// <summary>
/// A ScalingViewport that uses <see cref="Scaling.Fill"/> so it keeps the aspect
/// ratio by scaling the world up to take the whole screen (some of the world may
/// be off screen).
/// </summary>
public class FillViewport : ScalingViewport
{
    /// <summary>
    /// Creates a new viewport using a new <see cref="OrthographicCamera"/>.
    /// </summary>
    public FillViewport(float worldWidth, float worldHeight)
        : base(Scaling.Fill, worldWidth, worldHeight)
    {
    }

    public FillViewport(float worldWidth, float worldHeight, Camera camera)
        : base(Scaling.Fill, worldWidth, worldHeight, camera)
    {
    }
}