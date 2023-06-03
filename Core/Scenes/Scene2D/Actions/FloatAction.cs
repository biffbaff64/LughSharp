using LibGDXSharp.Maths;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// An action that has a float, whose value is transitioned over time.
/// </summary>
public class FloatAction : TemporalAction
{
    public float Start    { get; set; }
    public float EndValue { get; set; }
    public float Value    { get; set; }

    /// <summary>
    /// Creates a FloatAction that transitions from 0 to 1.
    /// </summary>
    public FloatAction()
    {
        Start    = 0;
        EndValue = 1;
    }

    /// <summary>
    /// Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end )
    {
        this.Start    = start;
        this.EndValue = end;
    }

    /// <summary>
    /// Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end, float duration ) : base( duration )
    {
        this.Start    = start;
        this.EndValue = end;
    }

    /// <summary>
    /// Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end, float duration, Interpolation interpolation )
        : base( duration, interpolation )
    {
        this.Start    = start;
        this.EndValue = end;
    }

    protected new void Begin()
    {
        Value = Start;
    }

    protected override void Update( float percent )
    {
        if ( percent == 0 )
        {
            Value = Start;
        }
        else if ( percent is 1.0f )
        {
            Value = EndValue;
        }
        else
        {
            Value = Start + ( ( EndValue - Start ) * percent );
        }
    }
}
