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

using Corelib.LibCore.Input;

namespace Corelib.LibCore.Scenes.Scene2D.Listeners;

/// <summary>
/// Detects tap, long press, fling, pan, zoom, and pinch gestures on an actor.
/// If there is only a need to detect tap, use <see cref="ClickListener"/>.
/// </summary>
[PublicAPI]
public class ActorGestureListener : IEventListener
{
    private const float DEFAULT_HALF_TAP_SQUARE_SIZE = 20;
    private const float DEFAULT_TAP_COUNT_INTERVAL   = 0.4f;
    private const float DEFAULT_LONG_PRESS_DURATION  = 1.1f;
    private const float DEFAULT_MAX_FLING_DELAY      = int.MaxValue;

    private static readonly Vector2 _tmpCoords  = new();
    private static readonly Vector2 _tmpCoords2 = new();

    public ActorGestureDetector Detector        { get; set; }
    public Actor?               TouchDownTarget { get; set; }

    private Actor?      _actor;
    private InputEvent? _inputEvent;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Constructs a new GestureListener for Actors.
    /// </summary>
    /// <param name="halfTapSquareSize">
    /// Half width in pixels of the square around an initial touch event, see
    /// <see cref="ActorGestureDetector.Tap(float, float, int, int)"/>.
    /// </param>
    /// <param name="tapCountInterval">
    /// Time in seconds that must pass for two touch down/up sequences to be detected
    /// as consecutive taps.
    /// </param>
    /// <param name="longPressDuration">
    /// Time in seconds that must pass for the detector to fire a
    /// <see cref="ActorGestureDetector.LongPress(float, float)"/> event.
    /// </param>
    /// <param name="maxFlingDelay">
    /// No fling event is fired when the time in seconds the finger was dragged is larger
    /// than this, see <see cref="ActorGestureDetector.Fling(float, float, int)"/>.
    ///</param>
    public ActorGestureListener( float halfTapSquareSize = DEFAULT_HALF_TAP_SQUARE_SIZE,
                                 float tapCountInterval = DEFAULT_TAP_COUNT_INTERVAL,
                                 float longPressDuration = DEFAULT_LONG_PRESS_DURATION,
                                 float maxFlingDelay = DEFAULT_MAX_FLING_DELAY )
    {
        Detector = new ActorGestureDetector( halfTapSquareSize,
                                             tapCountInterval,
                                             longPressDuration,
                                             maxFlingDelay,
                                             this );
    }

    /// <summary>
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public virtual bool Handle( Event e )
    {
        if ( e is not InputEvent ev )
        {
            return false;
        }

        switch ( ev.Type )
        {
            case InputEvent.EventType.TouchDown:
            {
                _actor          = ev.ListenerActor;
                TouchDownTarget = ev.TargetActor;

                Detector.TouchDown( ev.StageX, ev.StageY, ev.Pointer, ev.Button );
                _actor?.StageToLocalCoordinates( _tmpCoords.Set( ev.StageX, ev.StageY ) );

                TouchDown( ev, _tmpCoords.X, _tmpCoords.Y, ev.Pointer, ev.Button );

                if ( ev.TouchFocus )
                {
                    ev.Stage?.AddTouchFocus( this,
                                             ev.ListenerActor,
                                             ev.TargetActor,
                                             ev.Pointer,
                                             ev.Button );
                }

                return true;
            }

            case InputEvent.EventType.TouchUp:
            {
                if ( ev.TouchFocusCancel )
                {
                    Detector.Reset();

                    return false;
                }

                _inputEvent = ev;
                _actor      = ev.ListenerActor;

                Detector.TouchUp( ev.StageX, ev.StageY, ev.Pointer, ev.Button );
                _actor?.StageToLocalCoordinates( _tmpCoords.Set( ev.StageX, ev.StageY ) );
                TouchUp( ev, _tmpCoords.X, _tmpCoords.Y, ev.Pointer, ev.Button );

                return true;
            }

            case InputEvent.EventType.TouchDragged:
            {
                _inputEvent = ev;
                _actor      = ev.ListenerActor;
                Detector.TouchDragged( ev.StageX, ev.StageY, ev.Pointer );

                return true;
            }

            default:
                break;
        }

        return false;
    }

    public virtual void TouchDown( InputEvent ev, float x, float y, int pointer, int button )
    {
    }

    public virtual void TouchUp( InputEvent ev, float x, float y, int pointer, int button )
    {
    }

    public virtual void Tap( InputEvent ev, float x, float y, int count, int button )
    {
    }

    /// <summary>
    /// If true is returned, additional gestures will not be triggered. No ev is
    /// provided because this ev is triggered by time passing, not by an InputEvent.
    /// </summary>
    public virtual bool LongPress( Actor actor, float x, float y )
    {
        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="velocityX"></param>
    /// <param name="velocityY"></param>
    /// <param name="button"></param>
    public virtual void Fling( InputEvent ev, float velocityX, float velocityY, int button )
    {
    }

    /// <summary>
    /// The delta is the difference in stage coordinates since the last pan.
    /// </summary>
    public virtual void Pan( InputEvent ev, float x, float y, float deltaX, float deltaY )
    {
    }

    public virtual void PanStop( InputEvent ev, float x, float y, int pointer, int button )
    {
    }

    public virtual void Zoom( InputEvent ev, float initialDistance, float distance )
    {
    }

    public virtual void Pinch( InputEvent ev,
                               Vector2 initialPointer1,
                               Vector2 initialPointer2,
                               Vector2 pointer1,
                               Vector2 pointer2 )
    {
    }

    // ------------------------------------------------------------------------

    [PublicAPI]
    public class ActorGestureDetector : GestureDetector
    {
        private readonly Vector2              _initialPointer1 = new();
        private readonly Vector2              _initialPointer2 = new();
        private readonly ActorGestureListener _parent;
        private readonly Vector2              _pointer1 = new();
        private readonly Vector2              _pointer2 = new();

        public ActorGestureDetector( float halfTapSquareSize,
                                     float tapCountInterval,
                                     float longPressDuration,
                                     float maxFlingDelay,
                                     ActorGestureListener parent )
            : base( halfTapSquareSize,
                    tapCountInterval,
                    longPressDuration,
                    maxFlingDelay,
                    new GestureAdapter() )
        {
            _parent = parent;
        }

        public bool Tap( float stageX, float stageY, int count, int button )
        {
            _parent._actor?.StageToLocalCoordinates( _tmpCoords.Set( stageX, stageY ) );

            _parent.Tap( _parent._inputEvent!, _tmpCoords.X, _tmpCoords.Y, count, button );

            return true;
        }

        public bool LongPress( float stageX, float stageY )
        {
            _parent._actor?.StageToLocalCoordinates( _tmpCoords.Set( stageX, stageY ) );

            return _parent.LongPress( _parent._actor!, _tmpCoords.X, _tmpCoords.Y );
        }

        public bool Fling( float velocityX, float velocityY, int button )
        {
            StageToLocalAmount( _tmpCoords.Set( velocityX, velocityY ) );
            _parent.Fling( _parent._inputEvent!, _tmpCoords.X, _tmpCoords.Y, button );

            return true;
        }

        public bool Pan( float stageX, float stageY, float deltaX, float deltaY )
        {
            StageToLocalAmount( _tmpCoords.Set( deltaX, deltaY ) );

            deltaX = _tmpCoords.X;
            deltaY = _tmpCoords.Y;

            _parent._actor!.StageToLocalCoordinates( _tmpCoords.Set( stageX, stageY ) );
            _parent.Pan( _parent._inputEvent!, _tmpCoords.X, _tmpCoords.Y, deltaX, deltaY );

            return true;
        }

        public bool PanStop( float stageX, float stageY, int pointer, int button )
        {
            _parent._actor!.StageToLocalCoordinates( _tmpCoords.Set( stageX, stageY ) );
            _parent.PanStop( _parent._inputEvent!, _tmpCoords.X, _tmpCoords.Y, pointer, button );

            return true;
        }

        public bool Zoom( float initialDistance, float distance )
        {
            _parent.Zoom( _parent._inputEvent!, initialDistance, distance );

            return true;
        }

        public bool Pinch( Vector2 stageInitialPointer1,
                           Vector2 stageInitialPointer2,
                           Vector2 stagePointer1,
                           Vector2 stagePointer2 )
        {
            _parent._actor!.StageToLocalCoordinates( _initialPointer1.Set( stageInitialPointer1 ) );
            _parent._actor!.StageToLocalCoordinates( _initialPointer2.Set( stageInitialPointer2 ) );
            _parent._actor!.StageToLocalCoordinates( _pointer1.Set( stagePointer1 ) );
            _parent._actor!.StageToLocalCoordinates( _pointer2.Set( stagePointer2 ) );

            _parent.Pinch( _parent._inputEvent!, _initialPointer1, _initialPointer2, _pointer1, _pointer2 );

            return true;
        }

        private void StageToLocalAmount( Vector2 amount )
        {
            _parent._actor!.StageToLocalCoordinates( amount );
            amount.Sub( _parent._actor.StageToLocalCoordinates( _tmpCoords2.Set( 0, 0 ) ) );
        }
    }
}
