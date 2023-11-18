// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Input;

/** {@link InputProcessor} implementation that detects gestures (tap, long press, fling, pan, zoom, pinch) and hands them to a
 * {@link GestureListener}.
 * @author mzechner */
[PublicAPI]
public class GestureDetector : InputAdapter
{
    private float _tapRectangleWidth;
    private float _tapRectangleHeight;
    private long  _tapCountInterval;
    private float _longPressSeconds;
    private long  _maxFlingDelay;
    private bool  _inTapRectangle;
    private int   _tapCount;
    private long  _lastTapTime;
    private float _lastTapX;
    private float _lastTapY;
    private int   _lastTapButton;
    private int   _lastTapPointer;
    private bool  _longPressFired;
    private bool  _pinching;
    private bool  _panning;
    private float _tapRectangleCenterX;
    private float _tapRectangleCenterY;
    private long  _touchDownTime;

    private IGestureListener? _listener        = null;
    private VelocityTracker   _tracker         = new();
    private Vector2           _pointer1        = new();
    private Vector2           _pointer2        = new();
    private Vector2           _initialPointer1 = new();
    private Vector2           _initialPointer2 = new();

//private Task longPressTask = new Task()
//{
//    public void run()
//    {
//        if ( !longPressFired )
//        {
//            longPressFired = listener.longPress( pointer1.x, pointer1.y );
//        }
//    }
//};

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
    public GestureDetector( float halfTapSquareSize,
                            float tapCountInterval,
                            float longPressDuration,
                            float maxFlingDelay,
                            IGestureListener listener )
        : this( halfTapSquareSize, halfTapSquareSize, tapCountInterval, longPressDuration, maxFlingDelay, listener )
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
    /// @param maxFlingDelay no fling event is fired when the time in seconds the finger was dragged is larger than this, see
    ///           {@link GestureListener#fling(float, float, int)}
    /// </param>
    public GestureDetector( float halfTapRectangleWidth,
                            float halfTapRectangleHeight,
                            float tapCountInterval,
                            float longPressDuration,
                            float maxFlingDelay,
                            IGestureListener? listener )
    {
        this._tapRectangleWidth  = halfTapRectangleWidth;
        this._tapRectangleHeight = halfTapRectangleHeight;
        this._tapCountInterval   = ( long )( tapCountInterval * 1000000000l );
        this._longPressSeconds   = longPressDuration;
        this._maxFlingDelay      = ( long )( maxFlingDelay * 1000000000l );
        this._listener           = listener ?? throw new ArgumentException( "listener cannot be null." );
    }

    public bool touchDown( int x, int y, int pointer, int button )
    {
        return touchDown( ( float )x, ( float )y, pointer, button );
    }

    public bool touchDown( float x, float y, int pointer, int button )
    {
        if ( pointer > 1 )
        {
            return false;
        }

        if ( pointer == 0 )
        {
            pointer1.set( x, y );
            touchDownTime = Gdx.input.getCurrentEventTime();
            tracker.start( x, y, touchDownTime );

            if ( Gdx.input.isTouched( 1 ) )
            {
                // Start pinch.
                inTapRectangle = false;
                pinching       = true;
                initialPointer1.set( pointer1 );
                initialPointer2.set( pointer2 );
                longPressTask.cancel();
            }
            else
            {
                // Normal touch down.
                inTapRectangle      = true;
                pinching            = false;
                longPressFired      = false;
                tapRectangleCenterX = x;
                tapRectangleCenterY = y;

                if ( !longPressTask.isScheduled() )
                {
                    Timer.schedule( longPressTask, longPressSeconds );
                }
            }
        }
        else
        {
            // Start pinch.
            pointer2.set( x, y );
            inTapRectangle = false;
            pinching       = true;
            initialPointer1.set( pointer1 );
            initialPointer2.set( pointer2 );
            longPressTask.cancel();
        }

        return listener.touchDown( x, y, pointer, button );
    }

    public bool touchDragged( int x, int y, int pointer )
    {
        return touchDragged( ( float )x, ( float )y, pointer );
    }

    public bool touchDragged( float x, float y, int pointer )
    {
        if ( pointer > 1 )
        {
            return false;
        }

        if ( longPressFired )
        {
            return false;
        }

        if ( pointer == 0 )
        {
            pointer1.set( x, y );
        }
        else
        {
            pointer2.set( x, y );
        }

        // handle pinch zoom
        if ( pinching )
        {
            if ( listener != null )
            {
                bool result = listener.pinch( initialPointer1, initialPointer2, pointer1, pointer2 );

                return listener.zoom( initialPointer1.dst( initialPointer2 ), pointer1.dst( pointer2 ) ) || result;
            }

            return false;
        }

        // update tracker
        tracker.update( x, y, Gdx.input.getCurrentEventTime() );

        // check if we are still tapping.
        if ( inTapRectangle && !isWithinTapRectangle( x, y, tapRectangleCenterX, tapRectangleCenterY ) )
        {
            longPressTask.cancel();
            inTapRectangle = false;
        }

        // if we have left the tap square, we are panning
        if ( !inTapRectangle )
        {
            panning = true;

            return listener.pan( x, y, tracker.deltaX, tracker.deltaY );
        }

        return false;
    }

    public bool touchUp( int x, int y, int pointer, int button )
    {
        return touchUp( ( float )x, ( float )y, pointer, button );
    }

    public bool touchUp( float x, float y, int pointer, int button )
    {
        if ( pointer > 1 )
        {
            return false;
        }

        // check if we are still tapping.
        if ( inTapRectangle && !isWithinTapRectangle( x, y, tapRectangleCenterX, tapRectangleCenterY ) )
        {
            inTapRectangle = false;
        }

        bool wasPanning = panning;
        panning = false;

        longPressTask.cancel();

        if ( longPressFired )
        {
            return false;
        }

        if ( inTapRectangle )
        {
            // handle taps
            if ( lastTapButton != button
              || lastTapPointer != pointer
              || TimeUtils.nanoTime() - lastTapTime > tapCountInterval
              || !isWithinTapRectangle( x, y, lastTapX, lastTapY ) )
            {
                tapCount = 0;
            }

            tapCount++;
            lastTapTime    = TimeUtils.nanoTime();
            lastTapX       = x;
            lastTapY       = y;
            lastTapButton  = button;
            lastTapPointer = pointer;
            touchDownTime  = 0;

            return listener.tap( x, y, tapCount, button );
        }

        if ( pinching )
        {
            // handle pinch end
            pinching = false;
            listener.pinchStop();
            panning = true;

            // we are in pan mode again, reset velocity tracker
            if ( pointer == 0 )
            {
                // first pointer has lifted off, set up panning to use the second pointer...
                tracker.start( pointer2.x, pointer2.y, Gdx.input.getCurrentEventTime() );
            }
            else
            {
                // second pointer has lifted off, set up panning to use the first pointer...
                tracker.start( pointer1.x, pointer1.y, Gdx.input.getCurrentEventTime() );
            }

            return false;
        }

        // handle no longer panning
        bool handled = false;

        if ( wasPanning && !panning )
        {
            handled = listener.panStop( x, y, pointer, button );
        }

        // handle fling
        long time = Gdx.input.getCurrentEventTime();

        if ( time - touchDownTime <= maxFlingDelay )
        {
            tracker.update( x, y, time );
            handled = listener.fling( tracker.getVelocityX(), tracker.getVelocityY(), button ) || handled;
        }

        touchDownTime = 0;

        return handled;
    }

    /** No further gesture events will be triggered for the current touch, if any. */
    public void cancel()
    {
        longPressTask.cancel();
        longPressFired = true;
    }

    /** @return whether the user touched the screen long enough to trigger a long press event. */
    public bool isLongPressed()
    {
        return isLongPressed( longPressSeconds );
    }

    /** @param duration
     * @return whether the user touched the screen for as much or more than the given duration. */
    public bool isLongPressed( float duration )
    {
        if ( touchDownTime == 0 )
        {
            return false;
        }

        return TimeUtils.nanoTime() - touchDownTime > ( long )( duration * 1000000000l );
    }

    public bool isPanning()
    {
        return panning;
    }

    public void reset()
    {
        touchDownTime    = 0;
        panning          = false;
        inTapRectangle   = false;
        tracker.lastTime = 0;
    }

    private bool isWithinTapRectangle( float x, float y, float centerX, float centerY )
    {
        return Math.abs( x - centerX ) < tapRectangleWidth && Math.abs( y - centerY ) < tapRectangleHeight;
    }

    /** The tap square will no longer be used for the current touch. */
    public void invalidateTapSquare()
    {
        inTapRectangle = false;
    }

    public void setTapSquareSize( float halfTapSquareSize )
    {
        setTapRectangleSize( halfTapSquareSize, halfTapSquareSize );
    }

    public void setTapRectangleSize( float halfTapRectangleWidth, float halfTapRectangleHeight )
    {
        this.tapRectangleWidth  = halfTapRectangleWidth;
        this.tapRectangleHeight = halfTapRectangleHeight;
    }

    /** @param tapCountInterval time in seconds that must pass for two touch down/up sequences to be detected as consecutive
     *           taps. */
    public void setTapCountInterval( float tapCountInterval )
    {
        this.tapCountInterval = ( long )( tapCountInterval * 1000000000l );
    }

    public void setLongPressSeconds( float longPressSeconds )
    {
        this.longPressSeconds = longPressSeconds;
    }

    public void setMaxFlingDelay( long maxFlingDelay )
    {
        this.maxFlingDelay = maxFlingDelay;
    }

    /** Register an instance of this class with a {@link GestureDetector} to receive gestures such as taps, long presses, flings,
     * panning or pinch zooming. Each method returns a bool indicating if the event should be handed to the next listener (false
     * to hand it to the next listener, true otherwise).
     * @author mzechner */
    [PublicAPI]
    public interface IGestureListener
    {
        /** @see InputProcessor#touchDown(int, int, int, int) */
        bool TouchDown( float x, float y, int pointer, int button );

        /** Called when a tap occured. A tap happens if a touch went down on the screen and was lifted again without moving outside
         * of the tap square. The tap square is a rectangular area around the initial touch position as specified on construction
         * time of the {@link GestureDetector}.
         * @param count the number of taps. */
        bool Tap( float x, float y, int count, int button );

        bool LongPress( float x, float y );

        /** Called when the user dragged a finger over the screen and lifted it. Reports the last known velocity of the finger in
         * pixels per second.
         * @param velocityX velocity on x in seconds
         * @param velocityY velocity on y in seconds */
        bool Fling( float velocityX, float velocityY, int button );

        /** Called when the user drags a finger over the screen.
         * @param deltaX the difference in pixels to the last drag event on x.
         * @param deltaY the difference in pixels to the last drag event on y. */
        bool Pan( float x, float y, float deltaX, float deltaY );

        /** Called when no longer panning. */
        bool PanStop( float x, float y, int pointer, int button );

        /** Called when the user performs a pinch zoom gesture. The original distance is the distance in pixels when the gesture
         * started.
         * @param initialDistance distance between fingers when the gesture started.
         * @param distance current distance between fingers. */
        bool Zoom( float initialDistance, float distance );

        /** Called when a user performs a pinch zoom gesture. Reports the initial positions of the two involved fingers and their
         * current positions.
         * @param initialPointer1
         * @param initialPointer2
         * @param pointer1
         * @param pointer2 */
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
    [PublicAPI]
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

        public bool Pinch( Vector2 initialPointer1, Vector2 initialPointer2, Vector2 pointer1, Vector2 pointer2 )
        {
            return false;
        }

        public void PinchStop()
        {
        }
    }

    [PublicAPI]
    public class VelocityTracker
    {
        int     sampleSize = 10;
        float   lastX,  lastY;
        float   deltaX, deltaY;
        long    lastTime;
        int     numSamples;
        float[] meanX    = new float[ sampleSize ];
        float[] meanY    = new float[ sampleSize ];
        long[]  meanTime = new long[ sampleSize ];

        public void start( float x, float y, long timeStamp )
        {
            lastX      = x;
            lastY      = y;
            deltaX     = 0;
            deltaY     = 0;
            numSamples = 0;

            for ( int i = 0; i < sampleSize; i++ )
            {
                meanX[ i ]    = 0;
                meanY[ i ]    = 0;
                meanTime[ i ] = 0;
            }

            lastTime = timeStamp;
        }

        public void update( float x, float y, long currTime )
        {
            deltaX = x - lastX;
            deltaY = y - lastY;
            lastX  = x;
            lastY  = y;
            long deltaTime = currTime - lastTime;
            lastTime = currTime;
            int index = numSamples % sampleSize;
            meanX[ index ]    = deltaX;
            meanY[ index ]    = deltaY;
            meanTime[ index ] = deltaTime;
            numSamples++;
        }

        public float getVelocityX()
        {
            float meanX    = getAverage( this.meanX, numSamples );
            float meanTime = getAverage( this.meanTime, numSamples ) / 1000000000.0f;

            if ( meanTime == 0 )
            {
                return 0;
            }

            return meanX / meanTime;
        }

        public float getVelocityY()
        {
            float meanY    = getAverage( this.meanY, numSamples );
            float meanTime = getAverage( this.meanTime, numSamples ) / 1000000000.0f;

            if ( meanTime == 0 )
            {
                return 0;
            }

            return meanY / meanTime;
        }

        private float getAverage( float[] values, int numSamples )
        {
            numSamples = Math.Min( sampleSize, numSamples );
            float sum = 0;

            for ( int i = 0; i < numSamples; i++ )
            {
                sum += values[ i ];
            }

            return sum / numSamples;
        }

        private long getAverage( long[] values, int numSamples )
        {
            numSamples = Math.Min( sampleSize, numSamples );
            long sum = 0;

            for ( int i = 0; i < numSamples; i++ )
            {
                sum += values[ i ];
            }

            if ( numSamples == 0 )
            {
                return 0;
            }

            return sum / numSamples;
        }

        private float getSum( float[] values, int numSamples )
        {
            numSamples = Math.Min( sampleSize, numSamples );
            float sum = 0;

            for ( int i = 0; i < numSamples; i++ )
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
