using LibGDXSharp.Maths;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// An action that has a int, whose value is transitioned over time.
/// </summary>
public class IntAction : TemporalAction
{
    public int Start    { get; set; }
    public int EndValue { get; set; }
    public int Value    { get; set; }

    /// <summary>
    /// Creates a IntAction that transitions from 0 to 1.
    /// </summary>
    public IntAction()
    {
        Start    = 0;
        EndValue = 1;
    }

    /// <summary>
    /// Creates a IntAction that transitions from start to end.
    /// </summary>
    public IntAction( int start, int end )
    {
        this.Start    = start;
        this.EndValue = end;
    }

    /// <summary>
    /// Creates a IntAction that transitions from start to end.
    /// </summary>
    public IntAction( int start, int end, float duration ) : base( duration )
    {
        this.Start    = start;
        this.EndValue = end;
    }

    /// <summary>
    /// Creates a IntAction that transitions from start to end.
    /// </summary>
    public IntAction( int start, int end, float duration, Interpolation interpolation )
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
            Value = ( int )( Start + ( ( EndValue - Start ) * percent ) );
        }
    }
}
