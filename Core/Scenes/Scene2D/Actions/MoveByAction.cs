namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class MoveByAction : RelativeTemporalAction
{
    public float AmountX { get; set; }
    public float AmountY { get; set; }

    protected void UpdateRelative( float percentDelta )
    {
        Target?.MoveBy( AmountX * percentDelta, AmountY * percentDelta );
    }

    public void SetAmount( float x, float y )
    {
        this.AmountX = x;
        this.AmountY = y;
    }
}