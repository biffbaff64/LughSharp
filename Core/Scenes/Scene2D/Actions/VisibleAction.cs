namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class VisibleAction : Action
{
    public bool Visible { get; set; }

    /// <summary>
    /// Updates the action based on time.
    /// Typically this is called each frame by <see cref="Action.Actor"/>.
    /// </summary>
    /// <param name="delta">Time in seconds since the last frame.</param>
    /// <returns>
    /// true if the action is done. This method may continue to be called after
    /// the action is done.
    /// </returns>
    public override bool Act( float delta )
    {
        if ( Target == null ) return false;
        
        Target.Visible = Visible;

        return true;
    }
}