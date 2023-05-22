namespace LibGDXSharp.Scenes.Scene2D.Utils;

public abstract class ChangeListener : IEventListener
{
    public bool Handle( Event ev )
    {
        if ( ev is not ChangeEvent changeEvent ) return false;

        Changed( changeEvent, changeEvent.TargetActor );

        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="actor">
    /// The event target, which is the actor that emitted the change event.
    /// </param>
    protected abstract void Changed( ChangeEvent ev, Actor? actor);

    /// <summary>
    /// Fired when something in an actor has changed. This is a generic event, exactly
    /// what changed in an actor will vary.
    /// </summary>
    protected class ChangeEvent : Event
    {
    }
}