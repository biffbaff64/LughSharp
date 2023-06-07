using System.Diagnostics;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class RemoveListenerAction : Action
{
    public IEventListener? Listener { get; set; }
    public bool            Capture  { get; set; }

    public override bool Act( float delta )
    {
        Debug.Assert( Listener != null, nameof( Listener ) + " != null" );

        if ( Capture )
        {
            Target?.RemoveCaptureListener( Listener );
        }
        else
        {
            Target?.RemoveListener( Listener );
        }

        return true;
    }

    public new void Reset()
    {
        base.Reset();
        Listener = null;
    }
}