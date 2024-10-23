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

using Corelib.LibCore.Utils;

namespace Corelib.LibCore.Input;

/// <summary>
/// <see cref="IInputProcessor"/>" implementation that detects gestures
/// (tap, long press, fling, pan, zoom, pinch) and hands them to a
/// <see cref="IGestureListener"/>.
/// </summary>
[PublicAPI]
public class GestureDetector : InputAdapter
{
    private readonly Vector2 _initialPointer1 = new();
    private readonly Vector2 _initialPointer2 = new();

    private readonly IGestureListener _listener;
    private readonly Vector2          _pointer1 = new();
    private readonly Vector2          _pointer2 = new();
    private readonly VelocityTracker  _tracker  = new();

    private bool  _inTapRectangle;
    private int   _lastTapButton;
    private int   _lastTapPointer;
    private long  _lastTapTime;
    private float _lastTapX;
    private float _lastTapY;

    private CancellationToken        _longPressCancellationToken;
    private bool                     _longPressFired;
    private float                    _longPressSeconds;
    private Task?                    _longPressTask;
    private CancellationTokenSource? _longPressTokenSource;
    private long                     _maxFlingDelay;
    private bool                     _panning;
    private bool                     _pinching;
    private int                      _tapCount;
    private long                     _tapCountInterval;
    private float                    _tapRectangleCenterX;
    private float                    _tapRectangleCenterY;
    private float                    _tapRectangleHeight;
    private float                    _tapRectangleWidth;
    private long                     _touchDownTime;

    /// <summary>
    /// Creates a new GestureDetector with default values: halfTapSquareSize=20,
    /// tapCountInterval=0.4f, longPressDuration=1.1f, maxFlingDelay=int.MaxValue.
    /// </summary>
    public GestureDetector( IGestureListener listener )
        : this( 20, 0.4f, 1.1f, int.MaxValue, listener )
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="halfTapSquareSize">
    /// half width in pixels of the square around an initial touch event, see
    /// <see cref="IGestureListener.Tap(float, float, int, int)"/>.
    /// </param>
    /// <param name="tapCountInterval">
    /// time in seconds that must pass for two touch down/up sequences to
    /// be detected as consecutive taps.
    /// </param>
    /// <param name="longPressDuration">
    /// time in seconds that must pass for the detector to fire a
    /// <see cref="IGestureListener.LongPress(float, float)"/> event.
    /// </param>
    /// <param name="maxFlingDelay">
    /// no fling event is fired when the time in seconds the finger was dragged is
    /// larger than this, see <see cref="IGestureListener.Fling(float, float, int)"/>
    /// </param>
    /// <param name="listener"></param>
    public GestureDetector( float halfTapSquareSize,
                            float tapCountInterval,
                            float longPressDuration,
                            float maxFlingDelay,
                            IGestureListener listener )
        : this( halfTapSquareSize,
                halfTapSquareSize,
                tapCountInterval,
                longPressDuration,
                maxFlingDelay,
                listener )
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="halfTapRectangleWidth">
    /// half width in pixels of the rectangle around an initial touch event,
    /// see <see cref="IGestureListener.Tap(float, float, int, int)"/>.
    /// </param>
    /// <param name="halfTapRectangleHeight">
    /// half height in pixels of the rectangle around an initial touch event,
    /// see <see cref="IGestureListener.Tap(float, float, int, int)"/>.
    /// </param>
    /// <param name="tapCountInterval">
    /// time in seconds that must pass for two touch down/up sequences to be
    /// detected as consecutive taps.
    /// </param>
    /// <param name="longPressDuration">
    /// time in seconds that must pass for the detector to fire a
    /// <see cref="IGestureListener.LongPress(float, float)"/> event.
    /// </param>
    /// <param name="maxFlingDelay">
    /// no fling event is fired when the time in seconds the finger was dragged
    /// is larger than this, see <see cref="IGestureListener.Fling(float, float, int)"/>
    /// </param>
    /// <param name="listener"></param>
    public GestureDetector( float halfTapRectangleWidth,
                            float halfTapRectangleHeight,
                            float tapCountInterval,
                            float longPressDuration,
                            float maxFlingDelay,
                            IGestureListener? listener )
    {
        _tapRectangleWidth  = halfTapRectangleWidth;
        _tapRectangleHeight = halfTapRectangleHeight;
        _tapCountInterval   = ( long ) ( tapCountInterval * 1000000000L );
        _longPressSeconds   = longPressDuration;
        _maxFlingDelay      = ( long ) ( maxFlingDelay * 1000000000L );
        _listener           = listener ?? throw new ArgumentException( "listener cannot be null." );

        _longPressTask        = null!;
        _longPressTokenSource = null!;
    }

    private async Task SetLongPressTask()
    {
        _longPressTokenSource       ??= new CancellationTokenSource();
        _longPressCancellationToken =   _longPressTokenSource.Token;

        //@formatter:off
        _longPressTask = Task.Run( () =>
        {
            if ( !_longPressFired )
            {
                _longPressFired = _listener.LongPress( _pointer1.X, _pointer1.Y );
            }

            if ( _longPressCancellationToken.IsCancellationRequested )
            {
                _longPressCancellationToken.ThrowIfCancellationRequested();
            }
        }, _longPressCancellationToken );
        //@formatter:on

        try
        {
            await _longPressTask;
        }
        catch ( OperationCanceledException )
        {
            // Ignore
        }
        finally
        {
            _longPressTokenSource.Dispose();
            _longPressTokenSource = null;
        }
    }

    /// <summary>
    /// If _longPressTask is not null and is running, cancel it.
    /// </summary>
    private void CancelLongPressTask()
    {
        if ( _longPressTask is { Status: TaskStatus.Running } )
        {
            _longPressTokenSource?.Cancel();
        }
    }

    public override bool TouchDown( int x, int y, int pointer, int button )
    {
        return TouchDown( x, y, pointer, button );
    }

    public bool TouchDown( float x, float y, int pointer, int button )
    {
        if ( pointer > 1 )
        {
            return false;
        }

        if ( pointer == 0 )
        {
            _pointer1.Set( x, y );
            _touchDownTime = Gdx.Input.GetCurrentEventTime();
            _tracker.Start( x, y, _touchDownTime );

            if ( Gdx.Input.IsTouched( 1 ) )
            {
                // Start pinch.
                _inTapRectangle = false;
                _pinching       = true;
                _initialPointer1.Set( _pointer1 );
                _initialPointer2.Set( _pointer2 );

                CancelLongPressTask();
            }
            else
            {
                // Normal touch down.
                _inTapRectangle      = true;
                _pinching            = false;
                _longPressFired      = false;
                _tapRectangleCenterX = x;
                _tapRectangleCenterY = y;

                if ( _longPressTask?.Status != TaskStatus.Running ) //.IsScheduled() )
                {
//                    Timer.Schedule( _longPressTask, _longPressSeconds );
                    SetLongPressTask().ConfigureAwait( false );
                }
            }
        }
        else
        {
            // Start pinch.
            _pointer2.Set( x, y );
            _inTapRectangle = false;
            _pinching       = true;
            _initialPointer1.Set( _pointer1 );
            _initialPointer2.Set( _pointer2 );

            CancelLongPressTask();
        }

        return _listener.TouchDown( x, y, pointer, button );
    }

    public override bool TouchDragged( int x, int y, int pointer )
    {
        return TouchDragged( x, y, pointer );
    }

    public bool TouchDragged( float x, float y, int pointer )
    {
        if ( pointer > 1 )
        {
            return false;
        }

        if ( _longPressFired )
        {
            return false;
        }

        if ( pointer == 0 )
        {
            _pointer1.Set( x, y );
        }
        else
        {
            _pointer2.Set( x, y );
        }

        // handle pinch zoom
        if ( _pinching )
        {
            var result = _listener.Pinch( _initialPointer1, _initialPointer2, _pointer1, _pointer2 );

            return _listener.Zoom( _initialPointer1.Distance( _initialPointer2 ),
                                   _pointer1.Distance( _pointer2 ) )
                || result;
        }

        // update tracker
        _tracker.Update( x, y, Gdx.Input.GetCurrentEventTime() );

        // check if we are still tapping.
        if ( _inTapRectangle && !IsWithinTapRectangle( x, y, _tapRectangleCenterX, _tapRectangleCenterY ) )
        {
            CancelLongPressTask();
            _inTapRectangle = false;
        }

        // if we have left the tap square, we are panning
        if ( !_inTapRectangle )
        {
            _panning = true;

            return _listener.Pan( x, y, _tracker.deltaX, _tracker.deltaY );
        }

        return false;
    }

    public override bool TouchUp( int x, int y, int pointer, int button )
    {
        return TouchUp( x, y, pointer, button );
    }

    public bool TouchUp( float x, float y, int pointer, int button )
    {
        if ( pointer > 1 )
        {
            return false;
        }

        // check if we are still tapping.
        if ( _inTapRectangle && !IsWithinTapRectangle( x, y, _tapRectangleCenterX, _tapRectangleCenterY ) )
        {
            _inTapRectangle = false;
        }

        var wasPanning = _panning;
        _panning = false;

        CancelLongPressTask();

        if ( _longPressFired )
        {
            return false;
        }

        if ( _inTapRectangle )
        {
            // handle taps
            if ( ( _lastTapButton != button )
              || ( _lastTapPointer != pointer )
              || ( ( TimeUtils.NanoTime() - _lastTapTime ) > _tapCountInterval )
              || !IsWithinTapRectangle( x, y, _lastTapX, _lastTapY ) )
            {
                _tapCount = 0;
            }

            _tapCount++;
            _lastTapTime    = TimeUtils.NanoTime();
            _lastTapX       = x;
            _lastTapY       = y;
            _lastTapButton  = button;
            _lastTapPointer = pointer;
            _touchDownTime  = 0;

            return _listener.Tap( x, y, _tapCount, button );
        }

        if ( _pinching )
        {
            // handle pinch end
            _pinching = false;
            _listener.PinchStop();
            _panning = true;

            // we are in pan mode again, reset velocity tracker
            if ( pointer == 0 )
            {
                // first pointer has lifted off, set up panning to use the second pointer...
                _tracker.Start( _pointer2.X, _pointer2.Y, Gdx.Input.GetCurrentEventTime() );
            }
            else
            {
                // second pointer has lifted off, set up panning to use the first pointer...
                _tracker.Start( _pointer1.X, _pointer1.Y, Gdx.Input.GetCurrentEventTime() );
            }

            return false;
        }

        // handle no longer panning
        var handled = false;

        if ( wasPanning && !_panning )
        {
            handled = _listener.PanStop( x, y, pointer, button );
        }

        // handle fling
        var time = Gdx.Input.GetCurrentEventTime();

        if ( ( time - _touchDownTime ) <= _maxFlingDelay )
        {
            _tracker.Update( x, y, time );

            handled = _listener.Fling( _tracker.GetVelocityX(), _tracker.GetVelocityY(), button )
                   || handled;
        }

        _touchDownTime = 0;

        return handled;
    }

    /// <summary>
    /// No further gesture events will be triggered for the
    /// current touch, if any.
    /// </summary>
    public void Cancel()
    {
        CancelLongPressTask();

        _longPressFired = true;
    }

    /// <summary>
    /// Returns whether the user touched the screen long enough
    /// to trigger a long press event.
    /// </summary>
    public bool IsLongPressed()
    {
        return IsLongPressed( _longPressSeconds );
    }

    /// <summary>
    /// returns whether the user touched the screen for as much
    /// or more than the given duration.
    /// </summary>
    public bool IsLongPressed( float duration )
    {
        if ( _touchDownTime == 0 )
        {
            return false;
        }

        return ( TimeUtils.NanoTime() - _touchDownTime ) > ( long ) ( duration * 1000000000L );
    }

    public bool IsPanning()
    {
        return _panning;
    }

    public void Reset()
    {
        _touchDownTime    = 0;
        _panning          = false;
        _inTapRectangle   = false;
        _tracker.lastTime = 0;
    }

    private bool IsWithinTapRectangle( float x, float y, float centerX, float centerY )
    {
        return ( Math.Abs( x - centerX ) < _tapRectangleWidth )
            && ( Math.Abs( y - centerY ) < _tapRectangleHeight );
    }

    /// <summary>
    /// The tap square will no longer be used for the current touch.
    /// </summary>
    public void InvalidateTapSquare()
    {
        _inTapRectangle = false;
    }

    public void SetTapSquareSize( float halfTapSquareSize )
    {
        SetTapRectangleSize( halfTapSquareSize, halfTapSquareSize );
    }

    public void SetTapRectangleSize( float halfTapRectangleWidth, float halfTapRectangleHeight )
    {
        _tapRectangleWidth  = halfTapRectangleWidth;
        _tapRectangleHeight = halfTapRectangleHeight;
    }

    public void SetTapCountInterval( float tapCountInterval )
    {
        _tapCountInterval = ( long ) ( tapCountInterval * 1000000000L );
    }

    public void SetLongPressSeconds( float longPressSeconds )
    {
        _longPressSeconds = longPressSeconds;
    }

    public void SetMaxFlingDelay( long maxFlingDelay )
    {
        _maxFlingDelay = maxFlingDelay;
    }

    /// <summary>
    /// Register an instance of this class with a <see cref="GestureDetector"/> to
    /// receive gestures such as taps, long presses, flings, panning or pinch zooming.
    /// Each method returns a bool indicating if the event should be handed to the
    /// next listener (false to hand it to the next listener, true otherwise).
    /// </summary>
    public interface IGestureListener
    {
        bool TouchDown( float x, float y, int pointer, int button );

        /// <summary>
        /// Called when a tap occured. A tap happens if a touch went down on the
        /// screen and was lifted again without moving outside of the tap square.
        /// The tap square is a rectangular area around the initial touch position
        /// as specified on construction time of the <see cref="GestureDetector"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"> the number of taps. </param>
        /// <param name="button"></param>
        bool Tap( float x, float y, int count, int button );

        bool LongPress( float x, float y );

        /// <summary>
        /// Called when the user dragged a finger over the screen and lifted it.
        /// Reports the last known velocity of the finger in pixels per second.
        /// </summary>
        /// <param name="velocityX"> velocity on x in seconds </param>
        /// <param name="velocityY"> velocity on y in seconds </param>
        /// <param name="button"></param>
        bool Fling( float velocityX, float velocityY, int button );

        /// <summary>
        /// Called when the user drags a finger over the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="deltaX"> the difference in pixels to the last drag event on x. </param>
        /// <param name="deltaY"> the difference in pixels to the last drag event on y. </param>
        bool Pan( float x, float y, float deltaX, float deltaY );

        /// <summary>
        /// Called when no longer panning.
        /// </summary>
        bool PanStop( float x, float y, int pointer, int button );

        /// <summary>
        /// Called when the user performs a pinch zoom gesture. The original
        /// distance is the distance in pixels when the gesture started.
        /// </summary>
        /// <param name="initialDistance">
        /// distance between fingers when the gesture started.
        /// </param>
        /// <param name="distance"> current distance between fingers. </param>
        bool Zoom( float initialDistance, float distance );

        /// <summary>
        /// Called when a user performs a pinch zoom gesture. Reports the initial
        /// positions of the two involved fingers and their current positions.
        /// </summary>
        /// <param name="initialPointer1"> </param>
        /// <param name="initialPointer2"> </param>
        /// <param name="pointer1"> </param>
        /// <param name="pointer2">  </param>
        bool Pinch( Vector2 initialPointer1, Vector2 initialPointer2, Vector2 pointer1, Vector2 pointer2 );

        /// <summary>
        /// Called when no longer pinching.
        /// </summary>
        void PinchStop();
    }

    /// <summary>
    /// Derrive from this if you only want to implement a subset
    /// of <see cref="IGestureListener"/>.
    /// </summary>
    public class GestureAdapter : IGestureListener
    {
        public bool TouchDown( float x, float y, int pointer, int button )
        {
            return false;
        }

        public bool Tap( float x, float y, int count, int button )
        {
            return false;
        }

        public bool LongPress( float x, float y )
        {
            return false;
        }

        public bool Fling( float velocityX, float velocityY, int button )
        {
            return false;
        }

        public bool Pan( float x, float y, float deltaX, float deltaY )
        {
            return false;
        }

        public bool PanStop( float x, float y, int pointer, int button )
        {
            return false;
        }

        public bool Zoom( float initialDistance, float distance )
        {
            return false;
        }

        public bool Pinch( Vector2 initialPointer1,
                           Vector2 initialPointer2,
                           Vector2 pointer1,
                           Vector2 pointer2 )
        {
            return false;
        }

        public void PinchStop()
        {
        }
    }


    public class VelocityTracker
    {
        private readonly long[]  _meanTime;
        private readonly float[] _meanX;
        private readonly float[] _meanY;

        private readonly int   _sampleSize;
        private          float _lastX;
        private          float _lastY;
        private          int   _numSamples;
        public           float deltaX;
        public           float deltaY;
        public           long  lastTime;

        public VelocityTracker()
        {
            _sampleSize = 10;

            _meanX    = new float[ _sampleSize ];
            _meanY    = new float[ _sampleSize ];
            _meanTime = new long[ _sampleSize ];
        }

        public void Start( float x, float y, long timeStamp )
        {
            _lastX      = x;
            _lastY      = y;
            deltaX      = 0;
            deltaY      = 0;
            _numSamples = 0;

            for ( var i = 0; i < _sampleSize; i++ )
            {
                _meanX[ i ]    = 0;
                _meanY[ i ]    = 0;
                _meanTime[ i ] = 0;
            }

            lastTime = timeStamp;
        }

        public void Update( float x, float y, long currTime )
        {
            deltaX = x - _lastX;
            deltaY = y - _lastY;
            _lastX = x;
            _lastY = y;

            var deltaTime = currTime - lastTime;

            lastTime = currTime;

            var index = _numSamples % _sampleSize;

            _meanX[ index ]    = deltaX;
            _meanY[ index ]    = deltaY;
            _meanTime[ index ] = deltaTime;
            _numSamples++;
        }

        public float GetVelocityX()
        {
            var meanX    = GetAverage( _meanX, _numSamples );
            var meanTime = GetAverage( _meanTime, _numSamples ) / 1000000000.0f;

            if ( meanTime == 0 )
            {
                return 0;
            }

            return meanX / meanTime;
        }

        public float GetVelocityY()
        {
            var meanY    = GetAverage( _meanY, _numSamples );
            var meanTime = GetAverage( _meanTime, _numSamples ) / 1000000000.0f;

            if ( meanTime == 0 )
            {
                return 0;
            }

            return meanY / meanTime;
        }

        private float GetAverage( float[] values, int numSamples )
        {
            numSamples = Math.Min( _sampleSize, numSamples );
            float sum = 0;

            for ( var i = 0; i < numSamples; i++ )
            {
                sum += values[ i ];
            }

            return sum / numSamples;
        }

        private long GetAverage( long[] values, int numSamples )
        {
            numSamples = Math.Min( _sampleSize, numSamples );
            long sum = 0;

            for ( var i = 0; i < numSamples; i++ )
            {
                sum += values[ i ];
            }

            if ( numSamples == 0 )
            {
                return 0;
            }

            return sum / numSamples;
        }

        private float GetSum( float[] values, int numSamples )
        {
            numSamples = Math.Min( _sampleSize, numSamples );
            float sum = 0;

            for ( var i = 0; i < numSamples; i++ )
            {
                sum += values[ i ];
            }

            if ( numSamples == 0 )
            {
                return 0;
            }

            return sum;
        }
    }
}
