using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" )]
public abstract class DelegateAction : Action
{
    public Action? Action { get; set; }

    protected abstract bool Delegate( float delta );

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
        Pool< object >? pool = base.Pool;
        
        // Ensure this action can't be returned to the pool inside the delegate action.
        base.Pool = null;

        try
        {
            return Delegate( delta );
        }
        finally
        {
            base.Pool = pool;
        }
    }

    public new void Restart()
    {
        this.Action?.Restart();
    }

    public new void Reset()
    {
        base.Reset();
        this.Action = null;
    }

    public new Actor? Actor
    {
        get => base.Actor;
        set
        {
            if ( this.Action != null )
            {
                this.Action.Actor = value;
            }

            base.Actor = value;
        }
    }

    public new Actor? Target
    {
        get => base.Target;
        set
        {
            if ( this.Action != null )
            {
                this.Action.Target = value;
            }

            base.Target = value;
        }
    }

    public override string ToString()
    {
        return base.ToString() + ( this.Action == null ? "" : $"({this.Action})" );
    }
}
