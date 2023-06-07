namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class ScaleByAction : RelativeTemporalAction
{
    public float AmountX { get; set; }
    public float AmountY { get; set; }

    protected override void UpdateRelative( float percentDelta )
    {
        Target?.ScaleBy( AmountX * percentDelta, AmountY * percentDelta );
    }

    public void SetAmount( float x, float y )
    {
        AmountX = x;
        AmountY = y;
    }

    public void SetAmount( float scale )
    {
        AmountX = scale;
        AmountY = scale;
    }
}