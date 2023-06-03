namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Adds a listener to the actor for a specific event type and does not complete
/// until <see cref="Handle"/> returns true.
/// </summary>
public abstract class EventAction<T> : Action, IEventListener where T : Event
{
    public bool Active     { get; set; }
    public T    EventClass { get; set; }
    public bool Result     { get; set; }

    protected EventAction( T eventClass )
    {
        this.EventClass = eventClass;
    }

    public new void Restart()
    {
        Result = false;
        Active = false;
    }

    public void SetTarget( Actor target )
    {
        base.Target?.RemoveListener( this );

        base.Target = target;

        base.Target?.AddListener( this );
    }

    /// <summary>
    /// Called when the specific type of event occurs on the actor.
    /// </summary>
    /// <returns>
    /// true if the event should be considered handled by <see cref="Event.Handle()"/>
    /// and this EventAction considered complete.
    /// </returns>
    public bool Handle( Event ev )
    {
        if ( !Active || ( ev.GetType() != EventClass.GetType() ) )
        {
            return false;
        }

        Result = HandleDelegate( ev );

        return Result;
    }

    // ReSharper disable once MemberCanBeProtected.Global
    public abstract bool HandleDelegate( Event ev );

    /// <summary>
    /// Updates the action based on time.
    /// Typically this is called each frame by <see cref="Actor"/>.
    /// </summary>
    /// <param name="delta">Time in seconds since the last frame.</param>
    /// <returns>
    /// true if the action is done. This method may continue to be called after
    /// the action is done.
    /// </returns>
    public override bool Act( float delta )
    {
        Active = true;

        return Result;
    }
}
