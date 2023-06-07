namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class RotateByAction : RelativeTemporalAction
{
    public float Amount { get; set; }

    protected override void UpdateRelative (float percentDelta)
    {
        Target?.RotateBy(Amount * percentDelta);
    }
}
