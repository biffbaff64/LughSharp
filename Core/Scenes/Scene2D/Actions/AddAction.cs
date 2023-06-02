namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Adds an action to an actor.
/// </summary>
public sealed class AddAction : Action
{
    public Action? Action { get; set; }

    public override bool Act(float delta)
    {
        Target?.AddAction( Action );
        return true;
    }

    public new void Restart()
    {
        Action?.Restart();
    }

    public new void Reset()
    {
        base.Reset();
        Action = null;
    }
}

