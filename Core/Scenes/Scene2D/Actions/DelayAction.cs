namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class DelayAction : DelegateAction
{
    public float Duration { get; set; }
    public float Time     { get; set; }

    public DelayAction()
    {
    }

    public DelayAction( float duration )
    {
        this.Duration = duration;
    }

    protected override bool Delegate( float delta )
    {
        if ( Time < Duration )
        {
            Time += delta;

            if ( Time < Duration ) return false;
            
            delta = Time - Duration;
        }

        return ( Action == null ) || Action.Act( delta );
    }

    public void Finish()
    {
        Time = Duration;
    }

    public new void Restart()
    {
        base.Restart();
        
        Time = 0;
    }
}
