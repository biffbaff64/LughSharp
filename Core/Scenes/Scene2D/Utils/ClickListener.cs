namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// Detects mouse over, mouse or finger touch presses, and clicks on an actor.
/// A touch must go down over the actor and is considered pressed as long as
/// it is over the actor or within the <see cref="TapSquareSize"/>.
/// This behavior makes it easier to press buttons on a touch interface when
/// the initial touch happens near the edge of the actor. Double clicks can be
/// detected using <see cref="TapCount"/>. Any touch (not just the first) will
/// trigger this listener. While pressed, other touch downs are ignored.
/// </summary>
public class ClickListener : InputListener
{
    /// <summary>
    /// Time in seconds <see cref="VisualPressed"/> reports true after a press
    /// resulting in a click is released.
    /// </summary>
    public readonly static float VisualPressedDuration = 0.1f;

    public float TouchDownX       { get; set; } = -1;
    public float TouchDownY       { get; set; } = -1;
    public int   PressedPointer   { get; set; } = -1;
    public int   PressedButton    { get; set; } = -1;
    public int   Button           { get; set; }
    public bool  Pressed          { get; set; }
    public float TapSquareSize    { get; set; } = 14;
    public int   TapCount         { get; set; }

    private bool _over;
    private bool _cancelled;
    private long _visualPressedTime;
    private long _lastTapTime;
    private long _tapCountInterval = ( long )( 0.4f * 1000000000L );

    /// <summary>
    /// Create a listener where <see cref="Clicked(InputEvent, float, float)"/> is
    /// only called for left clicks.
    /// </summary>
    /// <see cref="ClickListener(int)"/>
    public ClickListener()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public ClickListener( int button )
    {
        this.Button = button;
    }

    public new bool TouchDown( InputEvent ev, float x, float y, int pointer, int button )
    {
        if ( Pressed ) return false;

        if ( ( pointer == 0 ) && ( this.Button != -1 ) && ( button != this.Button ) )
        {
            return false;
        }

        Pressed        = true;
        PressedPointer = pointer;
        PressedButton  = button;
        TouchDownX     = x;
        TouchDownY     = y;
        VisualPressed  = true;

        return true;
    }

    public new void TouchDragged( InputEvent ev, float x, float y, int pointer )
    {
        if ( ( pointer != PressedPointer ) || _cancelled ) return;

        Pressed = IsOver( ev.ListenerActor, x, y );

        if ( !Pressed )
        {
            // Once outside the tap square, don't use the tap square anymore.
            InvalidateTapSquare();
        }
    }

    public new void TouchUp( InputEvent ev, float x, float y, int pointer, int button )
    {
        if ( pointer == PressedPointer )
        {
            if ( !_cancelled )
            {
                var touchUpOver = IsOver( ev.ListenerActor, x, y );

                // Ignore touch up if the wrong mouse button.
                if ( touchUpOver && ( pointer == 0 ) && ( this.Button != -1 ) && ( button != this.Button ) )
                {
                    touchUpOver = false;
                }

                if ( touchUpOver )
                {
                    long time = TimeUtils.NanoTime();

                    if ( ( time - _lastTapTime ) > TapCountInterval )
                    {
                        TapCount = 0;
                    }

                    TapCount++;
                    _lastTapTime = time;

                    Clicked( ev, x, y );
                }
            }

            Pressed        = false;
            PressedPointer = -1;
            PressedButton  = -1;
            _cancelled     = false;
        }
    }

    public new void Enter( InputEvent ev, float x, float y, int pointer, Actor fromActor )
    {
        if ( ( pointer == -1 ) && !_cancelled )
        {
            _over = true;
        }
    }

    public new void Exit( InputEvent ev, float x, float y, int pointer, Actor toActor )
    {
        if ( ( pointer == -1 ) && !_cancelled )
        {
            _over = false;
        }
    }

    /// <summary>
    /// If a touch down is being monitored, the drag and touch up events are
    /// ignored until the next touch up.
    /// </summary>
    public virtual void Cancel()
    {
        if ( PressedPointer == -1 ) return;

        _cancelled = true;
        Pressed    = false;
    }

    public void Clicked( InputEvent ev, float x, float y )
    {
    }

    /// <summary>
    /// Returns true if the specified position is over the specified
    /// actor or within the tap square.
    /// </summary>
    public bool IsOver( Actor? actor, float x, float y )
    {
        Actor? hit = actor?.Hit( x, y, true );

        if ( ( hit == null ) || !hit.IsDescendantOf( actor ) )
        {
            return InTapSquare( x, y );
        }

        return true;
    }

    public bool InTapSquare( float x, float y )
    {
        if ( TouchDownX.Equals( -1 ) && TouchDownY.Equals( -1 ) )
        {
            return false;
        }

        return ( Math.Abs( x - TouchDownX ) < TapSquareSize )
               && ( Math.Abs( y - TouchDownY ) < TapSquareSize );
    }

    /// <summary>
    /// Returns true if a touch is within the tap square.
    /// </summary>
    public bool InTapSquare()
    {
        return !TouchDownX.Equals( -1 );
    }

    /// <summary>
    /// The tap square will no longer be used for the current touch.
    /// </summary>
    public void InvalidateTapSquare()
    {
        TouchDownX = -1;
        TouchDownY = -1;
    }

    /// <summary>
    /// Returns true if a touch is over the actor or within the tap square or
    /// has been very recently. This allows the UI to show a press and release
    /// that was so fast it occurred within a single frame. 
    /// </summary>
    public bool VisualPressed
    {
        get
        {
            if ( Pressed ) return true;

            if ( _visualPressedTime <= 0 ) return false;

            if ( _visualPressedTime > TimeUtils.Millis() ) return true;

            _visualPressedTime = 0;

            return false;
        }
        set
        {
            if ( value )
            {
                _visualPressedTime = TimeUtils.Millis() + ( long )( VisualPressedDuration * 1000 );
            }
            else
            {
                _visualPressedTime = 0;
            }
        }
    }

    /// <summary>
    /// Returns true if the mouse or touch is over the actor or pressed
    /// and within the tap square.
    /// </summary>
    public bool Over => ( _over || Pressed );
    
    /// <summary>
    /// Sets the button to listen for, all other buttons are ignored.
    /// Default is <see cref="IInput.Buttons.Left"/>.
    /// Use -1 for any button.
    /// </summary>
    public long TapCountInterval
    {
        get => _tapCountInterval;
        set => _tapCountInterval = ( value * 1000000000L );
    }
}
