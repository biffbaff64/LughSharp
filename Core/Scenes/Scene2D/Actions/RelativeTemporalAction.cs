namespace LibGDXSharp.Scenes.Scene2D.Actions;

public abstract class RelativeTemporalAction : TemporalAction
{
    private float _lastPercent;

    protected new void Begin()
    {
        _lastPercent = 0;
    }

    protected override void Update( float percent )
    {
        UpdateRelative( percent - _lastPercent );
        _lastPercent = percent;
    }

    protected abstract void UpdateRelative( float percentDelta );
}