namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class SizeByAction : RelativeTemporalAction
{
    public float AmountWidth  { get; set; }
    public float AmountHeight { get; set; }

    protected override void UpdateRelative( float percentDelta )
    {
        Target?.SizeBy( AmountWidth * percentDelta, AmountHeight * percentDelta );
    }

    public void SetAmount( float width, float height )
    {
        AmountWidth  = width;
        AmountHeight = height;
    }
}