namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Removes an actor from the stage.
/// </summary>
public class RemoveActorAction : Action
{
    private bool _removed;

    public override bool Act(float delta)
    {
        if (!_removed)
        {
            _removed = true;
            Target?.Remove();
        }
        
        return true;
    }

    public new void Restart()
    {
        _removed = false;
    }
}

