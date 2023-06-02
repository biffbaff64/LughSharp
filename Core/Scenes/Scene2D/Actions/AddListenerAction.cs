namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Adds a listener to an actor.
/// </summary>
public sealed class AddListenerAction : Action
{
    public IEventListener? Listener { get; set; }
    public bool            Capture  { get; set; }

    public override bool Act( float delta )
    {
        if ( Capture )
        {
            Target?.AddCaptureListener( Listener! );
        }
        else
        {
            Target?.AddListener( Listener! );
        }

        return true;
    }

    public new void Reset()
    {
        base.Reset();
        Listener = null;
    }
}
