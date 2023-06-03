using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class LayoutAction : Action
{
    public bool Enabled { get; set; }

    public void SetTarget( Actor actor )
    {
        if ( ( actor != null ) && ( actor is not ILayout ) )
        {
            throw new GdxRuntimeException( "Actor must implement layout: " + actor );
        }

        base.Target = actor;
    }

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
        if ( base.Target == null ) return false;
        
        ( ( ILayout )base.Target ).LayoutEnabled = Enabled;

        return true;
    }
}
