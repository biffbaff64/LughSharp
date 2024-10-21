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

using Corelib.LibCore.Maths;
using Corelib.LibCore.Scenes.Scene2D.UI;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils;
using Timer = Corelib.LibCore.Utils.Timer;
using Utils_Timer = Corelib.LibCore.Utils.Timer;

namespace Corelib.LibCore.Scenes.Scene2D.Listeners;

/// <summary>
/// Causes a scroll pane to scroll when a drag goes outside the bounds of the scroll pane.
/// Attach the listener to the actor which will cause scrolling when dragged, usually the
/// scroll pane or the scroll pane's actor.
/// <para>
/// If <see cref="ScrollPane.SetFlickScroll(bool)"/> is true, the scroll pane must have
/// <see cref="ScrollPane.CancelTouchFocus"/> false. When a drag starts that should drag
/// rather than flick scroll, cancel the scroll pane's touch focus using:-
/// <code>
///             Stage.CancelTouchFocus(scrollPane);
///         </code>
/// .
/// In this case the drag scroll listener must not be attached to the scroll pane, else
/// it would also lose touch focus. Instead it can be attached to the scroll pane's actor.
/// </para>
/// <para>
/// If using drag and drop, <see cref="DragAndDrop.CancelTouchFocus"/> must be false.
/// </para>
/// </summary>
[PublicAPI]
public class DragScrollListener : DragListener
{
    private static readonly Vector2             _tmpCoords     = new();
    private readonly        Interpolation.ExpIn _interpolation = Interpolation.Exp5In;
    private readonly        ScrollPane          _scrollPane;

    private float      _maxSpeed  = 75;
    private float      _minSpeed  = 15;
    private float      _padBottom = 0;
    private float      _padTop    = 0;
    private long       _rampTime  = 1750;
    private ScrollDown _scrollDown;
    private ScrollUp   _scrollUp;
    private long       _startTime = 0;
    private float      _tickSecs  = 0.05f;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// This class listens for drag events and handles scrolling for a ScrollPane.
    /// </summary>
    public DragScrollListener( ScrollPane scroll )
    {
        _scrollPane = scroll;

        _scrollUp   = new ScrollUp( this, scroll );
        _scrollDown = new ScrollDown( this, scroll );
    }

    /// <summary>
    /// Configures the scrolling speed and timing for the drag scroll listener.
    /// </summary>
    /// <param name="minSpeedPixels">The minimum speed in pixels per second.</param>
    /// <param name="maxSpeedPixels">The maximum speed in pixels per second.</param>
    /// <param name="tickSecs">The interval in seconds at which the scroll position is updated.</param>
    /// <param name="rampSecs">The time in seconds it takes to ramp up to the maximum speed.</param>
    public virtual void Setup( float minSpeedPixels, float maxSpeedPixels, float tickSecs, float rampSecs )
    {
        _minSpeed = minSpeedPixels;
        _maxSpeed = maxSpeedPixels;
        _tickSecs = tickSecs;
        _rampTime = ( long ) ( rampSecs * 1000 );
    }

    /// <inheritdoc />
    public override void Drag( InputEvent ev, float x, float y, int pointer )
    {
        ev.ListenerActor?.LocalToActorCoordinates( _scrollPane, _tmpCoords.Set( x, y ) );

        if ( IsAbove( _tmpCoords.Y ) )
        {
            _scrollDown.Cancel();

            if ( !_scrollUp.IsScheduled() )
            {
                _startTime = TimeUtils.Millis();
                Timer.Schedule( _scrollUp, _tickSecs, _tickSecs );
            }

            return;
        }

        if ( IsBelow( _tmpCoords.Y ) )
        {
            _scrollUp.Cancel();

            if ( !_scrollDown.IsScheduled() )
            {
                _startTime = TimeUtils.Millis();
                Timer.Schedule( _scrollDown, _tickSecs, _tickSecs );
            }

            return;
        }

        // Don't call DragStop() from here because that can be overridden
        // and problems may arise...
        // Better safe than sorry, etc.
        _scrollUp.Cancel();
        _scrollDown.Cancel();
    }

    /// <inheritdoc />
    public override void DragStop( InputEvent ev, float x, float y, int pointer )
    {
        _scrollUp.Cancel();
        _scrollDown.Cancel();
    }

    /// <summary>
    /// Sets the top and bottom padding amounts.
    /// </summary>
    /// <param name="padtop"> The Top padding. </param>
    /// <param name="padbottom"> The Bottom padding. </param>
    public void SetPadding( float padtop, float padbottom )
    {
        _padTop    = padtop;
        _padBottom = padbottom;
    }

    /// <summary>
    /// Returns true if the provided Y value is above the scrollpane.
    /// </summary>
    protected bool IsAbove( float y )
    {
        return y >= ( _scrollPane.Height - _padTop );
    }

    /// <summary>
    /// Returns true if the provided Y value is below the scrollpane.
    /// </summary>
    protected bool IsBelow( float y )
    {
        return y < _padBottom;
    }

    /// <summary>
    /// Sets the ScrollPane Y Scroll to the provided value.
    /// </summary>
    protected void SetScroll( float y )
    {
        _scrollPane.SetScrollY( y );
    }

    /// <summary>
    /// Calculates the current scroll speed in pixels.
    /// </summary>
    /// <returns>
    /// The current scroll speed in pixels based on the interpolation between minimum and maximum speed.
    /// </returns>
    private float GetScrollPixels()
    {
        return _interpolation.Apply( _minSpeed,
                                     _maxSpeed,
                                     Math.Min( 1, ( TimeUtils.Millis() - _startTime ) / ( float ) _rampTime ) );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class ScrollUp( DragScrollListener dsl, ScrollPane scroll ) : Utils_Timer.Task
    {
        public override void Run()
        {
            dsl.SetScroll( scroll.GetVisualScrollY() - dsl.GetScrollPixels() );
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class ScrollDown( DragScrollListener dsl, ScrollPane scroll ) : Utils_Timer.Task
    {
        public override void Run()
        {
            dsl.SetScroll( scroll.GetVisualScrollY() + dsl.GetScrollPixels() );
        }
    }
}

// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------
// ----------------------------------------------------------------------------
