namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class TimeScaleAction : DelegateAction
{
    public float Scale { get; set; }

    protected override bool Delegate(float delta)
    {
        return ( base.Action == null ) || base.Action.Act(delta * Scale);
    }
}