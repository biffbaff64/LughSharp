// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using Corelib.LibCore.Core;
using Corelib.LibCore.Utils;

namespace Corelib.LibCore.Scenes.Scene2D.Listeners;

/// <summary>
/// Detects mouse over, mouse or finger touch presses, and clicks on an actor.
/// A touch must go down over the actor and is considered pressed as long as
/// it is over the actor or within the <see cref="TapSquareSize"/>.
/// This behavior makes it easier to press buttons on a touch interface when
/// the initial touch happens near the edge of the actor. Double clicks can be
/// detected using <see cref="TapCount"/>. Any touch (not just the first) will
/// trigger this listener. While pressed, other touch downs are ignored.
/// </summary>
[PublicAPI]
public class ClickListener : InputListener
{
    /// <summary>
    /// Time in seconds <see cref="VisualPressed"/> reports true after
    /// a press resulting in a click is released.
    /// </summary>
    public const float VISUAL_PRESSED_DURATION = 0.1f;

    private bool _cancelled;
    private long _lastTapTime;
    private bool _over;
    private long _tapCountInterval = ( long ) ( 0.4f * 1000000000L );
    private long _visualPressedTime;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new ClickListener.
    /// Sets the button to listen for, all other buttons are ignored.
    /// Default is <see cref="IInput.Buttons.LEFT"/>. Use -1 for any button.
    /// </summary>
    public ClickListener( int button = IInput.Buttons.LEFT )
    {
        Button = button;
    }

    public float TouchDownX     { get; set; } = -1;
    public float TouchDownY     { get; set; } = -1;
    public int   PressedPointer { get; set; } = -1;
    public int   PressedButton  { get; set; } = -1;
    public int   Button         { get; set; }
    public bool  Pressed        { get; set; }
    public float TapSquareSize  { get; set; } = 14;
    public int   TapCount       { get; set; }

    /// <summary>
    /// Returns true if a touch is over the actor or within the tap square or
    /// has been very recently. This allows the UI to show a press and release
    /// that was so fast it occurred within a single frame.
    /// </summary>
    public bool VisualPressed
    {
        get
        {
            if ( Pressed )
            {
                return true;
            }

            if ( _visualPressedTime <= 0 )
            {
                return false;
            }

            if ( _visualPressedTime > TimeUtils.Millis() )
            {
                return true;
            }

            _visualPressedTime = 0;

            return false;
        }
        set
        {
            if ( value )
            {
                _visualPressedTime = TimeUtils.Millis() + ( long ) ( VISUAL_PRESSED_DURATION * 1000 );
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
    public bool Over => _over || Pressed;

    /// <summary>
    /// Sets the button to listen for, all other buttons are ignored.
    /// Default is <see cref="IInput.Buttons.LEFT"/>.
    /// Use -1 for any button.
    /// </summary>
    public long TapCountInterval
    {
        get => _tapCountInterval;
        set => _tapCountInterval = value * 1000000000L;
    }

    /// <inheritdoc />
    public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
    {
        if ( Pressed )
        {
            return false;
        }

        if ( ( pointer == 0 ) && ( Button != -1 ) && ( button != Button ) )
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

    /// <inheritdoc />
    public override void TouchDragged( InputEvent? ev, float x, float y, int pointer )
    {
        if ( ( pointer != PressedPointer ) || _cancelled )
        {
            return;
        }

        Pressed = IsOver( ev?.ListenerActor, x, y );

        if ( !Pressed )
        {
            // Once outside the tap square, don't use the tap square anymore.
            InvalidateTapSquare();
        }
    }

    /// <inheritdoc />
    public override void TouchUp( InputEvent? ev, float x, float y, int pointer, int button )
    {
        if ( pointer == PressedPointer )
        {
            if ( !_cancelled )
            {
                var touchUpOver = IsOver( ev?.ListenerActor, x, y );

                // Ignore touch up if the wrong mouse button.
                if ( touchUpOver && ( pointer == 0 ) && ( Button != -1 ) && ( button != Button ) )
                {
                    touchUpOver = false;
                }

                if ( touchUpOver )
                {
                    var time = TimeUtils.NanoTime();

                    if ( ( time - _lastTapTime ) > TapCountInterval )
                    {
                        TapCount = 0;
                    }

                    TapCount++;
                    _lastTapTime = time;

                    OnClicked( ev!, x, y );
                }
            }

            Pressed        = false;
            PressedPointer = -1;
            PressedButton  = -1;
            _cancelled     = false;
        }
    }

    /// <inheritdoc />
    public override void Enter( InputEvent? ev, float x, float y, int pointer, Actor? fromActor )
    {
        if ( ( pointer == -1 ) && !_cancelled )
        {
            _over = true;
        }
    }

    /// <inheritdoc />
    public override void Exit( InputEvent? ev, float x, float y, int pointer, Actor? toActor )
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
        if ( PressedPointer == -1 )
        {
            return;
        }

        _cancelled = true;
        Pressed    = false;
    }

    /// <summary>
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public virtual void OnClicked( InputEvent ev, float x, float y )
    {
    }

    /// <summary>
    /// Returns true if the specified position is over the specified
    /// actor or within the tap square.
    /// </summary>
    public bool IsOver( Actor? actor, float x, float y )
    {
        var hit = actor?.Hit( x, y, true );

        if ( ( hit == null ) || !hit.IsDescendantOf( actor ) )
        {
            return InTapSquare( x, y );
        }

        return true;
    }

    /// <summary>
    /// Returns true if the supplied x and y coordinates are within the tap square.
    /// </summary>
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
}
