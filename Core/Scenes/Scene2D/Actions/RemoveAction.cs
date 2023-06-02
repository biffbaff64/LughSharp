namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class RemoveAction : Action
{
    public Action? Action { get; set; }

    /// <summary>
    /// Updates the action based on time.
    /// Typically this is called each frame by <see cref="Scene2D.Action.Actor"/>.
    /// </summary>
    /// <param name="delta">Time in seconds since the last frame.</param>
    /// <returns>
    /// true if the action is done. This method may continue to be called after
    /// the action is done.
    /// </returns>
    public override bool Act( float delta ) => false;
}