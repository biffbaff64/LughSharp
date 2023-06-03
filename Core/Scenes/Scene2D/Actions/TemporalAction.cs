using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Maths;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class TemporalAction : Action
{
    public bool           Reverse       { get; set; }
    public float          Duration      { get; set; }
    public float          Time          { get; set; }
    public Interpolation? Interpolation { get; set; }

    private bool _began;
    private bool _complete;

    protected TemporalAction()
    {
    }

    protected TemporalAction( float duration )
    {
        this.Duration = duration;
    }

    protected TemporalAction( float duration, Interpolation? interpolation )
    {
        this.Duration      = duration;
        this.Interpolation = interpolation;
    }

    public override bool Act( float delta )
    {
        if ( _complete ) return true;

        Pool< object >? pool = base.Pool;

        // Ensure this action can't be returned to the pool while executing.
        base.Pool = null;

        try
        {
            if ( !_began )
            {
                Begin();
                _began = true;
            }

            Time      += delta;
            _complete =  Time >= Duration;

            var percent = _complete ? 1 : Time / Duration;

            if ( Interpolation != null )
            {
                percent = Interpolation.Apply( percent );
            }

            Update( Reverse ? 1 - percent : percent );

            if ( _complete ) End();

            return _complete;
        }
        finally
        {
            base.Pool = pool;
        }
    }

    /// <summary>
    /// Called the first time <see cref="Act(float)"/> is called. This is a good place
    /// to query the <see cref="Actor"/>'s starting state.
    /// </summary>
    protected static void Begin()
    {
    }

    /// <summary>
    /// Called the last time <see cref="Act(float)"/> is called.
    /// </summary>
    protected static void End()
    {
    }

    /// <summary>
    /// Called each frame.
    /// </summary>
    /// <param name="percent">
    /// The percentage of completion for this action, growing from 0 to 1 over the
    /// duration. If <see cref="Reverse"/> is true, this will shrink from 1 to 0.
    /// </param>
    protected abstract void Update( float percent );

    /** Skips to the end of the transition. */
    public void Finish()
    {
        Time = Duration;
    }

    public new void Restart()
    {
        Time      = 0;
        _began    = false;
        _complete = false;
    }

    protected new void Reset()
    {
        base.Reset();

        Reverse       = false;
        Interpolation = null;
    }

    /// <summary>
    /// Returns true after <see cref="Act(float)"/> has been called where time >= duration.
    /// </summary>
    public bool IsComplete() => _complete;
}
