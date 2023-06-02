namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Sets the alpha for an actor's color (or a specified color), from the
/// current alpha to the new alpha. Note this action transitions from the
/// alpha at the time the action starts to the specified alpha.
/// </summary>
public class AlphaAction : TemporalAction
{
    public float Alpha { get; set; }

    private float _start;

    protected new void Begin()
    {
        if ( Target == null )
        {
            throw new GdxRuntimeException( "Cannot begin with a null Target!" );
        }
        
        _start = Target.Color!.A;
    }

    protected override void Update( float percent )
    {
        if ( Target!.Color == null ) return;

        if ( percent == 0 )
        {
            Target.Color.A = _start;
        }
        else if ( percent is 1.0f )
        {
            Target.Color.A = Alpha;
        }
        else
        {
            Target.Color.A = ( _start + ( ( Alpha - _start ) * percent ) );
        }
    }
}
