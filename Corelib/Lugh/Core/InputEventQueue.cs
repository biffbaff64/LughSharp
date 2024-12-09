// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Maths;

namespace Corelib.Lugh.Core;

/// <summary>
/// Queues events that are later passed to an <see cref="IInputProcessor"/>.
/// </summary>
[PublicAPI]
public class InputEventQueue
{
    #region constants

    private const int SKIP           = -1;
    private const int KEY_DOWN       = 0;
    private const int KEY_UP         = 1;
    private const int KEY_TYPED      = 2;
    private const int TOUCH_DOWN     = 3;
    private const int TOUCH_UP       = 4;
    private const int TOUCH_DRAGGED  = 5;
    private const int MOUSE_MOVED    = 6;
    private const int MOUSE_SCROLLED = 7;

    #endregion constants

    // ========================================================================

    public long CurrentEventTime { get; set; }

    // ========================================================================

    private readonly List< int > _processingQueue = new();
    private readonly List< int > _queue           = new();

    // ========================================================================

    /// <summary>
    /// Processes and drains the events in the queue using the specified input processor.
    /// </summary>
    /// <param name="processor">
    /// The input processor to handle the events. If null, the queue will be cleared without processing.
    /// </param>
    /// <exception cref="SystemException"> Thrown if an unknown event type is encountered. </exception>
    public void Drain( IInputProcessor? processor )
    {
        int[] processingArray;

        lock ( this )
        {
            if ( processor == null )
            {
                _queue.Clear();

                return;
            }

            _processingQueue.AddRange( _queue );
            _queue.Clear();

            processingArray = _processingQueue.ToArray();
        }

        var index = 0;
        var count = processingArray.Length;

        while ( index < count )
        {
            var eventType = processingArray[ index++ ];
            CurrentEventTime = ( ( long ) processingArray[ index++ ] << 32 ) | ( processingArray[ index++ ] & 0xFFFFFFFFL );

            switch ( eventType )
            {
                case SKIP:
                    index += processingArray[ index ];

                    break;

                case KEY_DOWN:
                    processor.KeyDown( processingArray[ index++ ] );

                    break;

                case KEY_UP:
                    processor.KeyUp( processingArray[ index++ ] );

                    break;

                case KEY_TYPED:
                    processor.KeyTyped( ( char ) processingArray[ index++ ] );

                    break;

                case TOUCH_DOWN:
                    processor.TouchDown( processingArray[ index++ ],
                                         processingArray[ index++ ],
                                         processingArray[ index++ ],
                                         processingArray[ index++ ] );

                    break;

                case TOUCH_UP:
                    processor.TouchUp( processingArray[ index++ ],
                                       processingArray[ index++ ],
                                       processingArray[ index++ ],
                                       processingArray[ index++ ] );

                    break;

                case TOUCH_DRAGGED:
                    processor.TouchDragged( processingArray[ index++ ],
                                            processingArray[ index++ ],
                                            processingArray[ index++ ] );

                    break;

                case MOUSE_MOVED:
                    processor.MouseMoved( processingArray[ index++ ], processingArray[ index++ ] );

                    break;

                case MOUSE_SCROLLED:
                    processor.Scrolled( NumberUtils.IntBitsToFloat( processingArray[ index++ ] ),
                                        NumberUtils.IntBitsToFloat( processingArray[ index++ ] ) );

                    break;

                default:
                    throw new SystemException( $"Unknown event type: {eventType}" );
            }
        }

        _processingQueue.Clear();
    }

    /// <summary>
    /// Finds the next index of the specified event type starting from the given index.
    /// </summary>
    /// <param name="nextType"> The event type to search for. </param>
    /// <param name="i"> The index to start searching from. </param>
    /// <returns>
    /// The index of the next occurrence of the specified event type, or -1 if not found.
    /// </returns>
    /// <exception cref="SystemException"> Thrown if an unknown event type is encountered. </exception>
    private int Next( int nextType, int i )
    {
        lock ( this )
        {
            var q = _queue.ToArray();

            for ( var n = _queue.Count; i < n; )
            {
                var type = q[ i ];

                if ( type == nextType )
                {
                    return i;
                }

                i += 3;

                switch ( type )
                {
                    case SKIP:
                    {
                        i += q[ i ];

                        break;
                    }

                    case KEY_DOWN:
                    case KEY_UP:
                    case KEY_TYPED:
                    {
                        i++;

                        break;
                    }

                    case TOUCH_DOWN:
                    case TOUCH_UP:
                    {
                        i += 4;

                        break;
                    }

                    case TOUCH_DRAGGED:
                    {
                        i += 3;

                        break;
                    }

                    case MOUSE_MOVED:
                    case MOUSE_SCROLLED:
                    {
                        i += 2;

                        break;
                    }

                    default:
                    {
                        throw new SystemException();
                    }
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Adds the specified time to the queue.
    /// </summary>
    /// <param name="time">The time to add to the queue.</param>
    private void QueueTime( long time )
    {
        _queue.Add( ( int ) ( time >> 32 ) );
        _queue.Add( ( int ) time );
    }

    /// <summary>
    /// Queues a key down event with the specified keycode and time.
    /// </summary>
    /// <param name="keycode"> The keycode of the key down event. </param>
    /// <param name="time"> The time the event occurred. </param>
    /// <returns> Always returns false. </returns>
    public bool KeyDown( int keycode, long time )
    {
        lock ( this )
        {
            _queue.Add( KEY_DOWN );
            QueueTime( time );
            _queue.Add( keycode );
        }

        return false;
    }

    /// <summary>
    /// Queues a key up event with the specified keycode and time.
    /// </summary>
    /// <param name="keycode"> The keycode of the key up event. </param>
    /// <param name="time"> The time the event occurred. </param>
    /// <returns> Always returns false. </returns>
    public bool KeyUp( int keycode, long time )
    {
        lock ( this )
        {
            _queue.Add( KEY_UP );
            QueueTime( time );
            _queue.Add( keycode );
        }

        return false;
    }

    /// <summary>
    /// Queues a key typed event with the specified character and time.
    /// </summary>
    /// <param name="character"> The character of the key typed event. </param>
    /// <param name="time"> The time the event occurred. </param>
    /// <returns> Always returns false. </returns>
    public bool KeyTyped( char character, long time )
    {
        lock ( this )
        {
            _queue.Add( KEY_TYPED );
            QueueTime( time );
            _queue.Add( character );
        }

        return false;
    }

    /// <summary>
    /// Queues a touch down event with the specified parameters.
    /// </summary>
    /// <param name="screenX"> The x-coordinate of the touch. </param>
    /// <param name="screenY"> The y-coordinate of the touch. </param>
    /// <param name="pointer"> The pointer index. </param>
    /// <param name="button"> The button pressed. </param>
    /// <param name="time"> The time the event occurred. </param>
    /// <returns> Always returns false. </returns>
    public bool TouchDown( int screenX, int screenY, int pointer, int button, long time )
    {
        lock ( this )
        {
            _queue.Add( TOUCH_DOWN );
            QueueTime( time );
            _queue.Add( screenX );
            _queue.Add( screenY );
            _queue.Add( pointer );
            _queue.Add( button );
        }

        return false;
    }

    /// <summary>
    /// Queues a touch up event with the specified parameters.
    /// </summary>
    /// <param name="screenX"> The x-coordinate of the touch. </param>
    /// <param name="screenY"> The y-coordinate of the touch. </param>
    /// <param name="pointer"> The pointer index. </param>
    /// <param name="button"> The button released. </param>
    /// <param name="time"> The time the event occurred. </param>
    /// <returns> Always returns false. </returns>
    public bool TouchUp( int screenX, int screenY, int pointer, int button, long time )
    {
        lock ( this )
        {
            _queue.Add( TOUCH_UP );
            
            QueueTime( time );
            
            _queue.Add( screenX );
            _queue.Add( screenY );
            _queue.Add( pointer );
            _queue.Add( button );
        }

        return false;
    }

    /// <summary>
    /// Queues a touch dragged event with the specified parameters, skipping any previously
    /// queued touch dragged events for the same pointer.
    /// </summary>
    /// <param name="screenX"> The x-coordinate of the drag. </param>
    /// <param name="screenY"> The y-coordinate of the drag. </param>
    /// <param name="pointer"> The pointer index. </param>
    /// <param name="time"> The time the event occurred. </param>
    /// <returns> Always returns false. </returns>
    public bool TouchDragged( int screenX, int screenY, int pointer, long time )
    {
        lock ( this )
        {
            // Skip any queued touch dragged events for the same pointer.
            for ( var i = Next( TOUCH_DRAGGED, 0 ); i >= 0; i = Next( TOUCH_DRAGGED, i + 6 ) )
            {
                if ( _queue[ i + 5 ] == pointer )
                {
                    _queue[ i ]     = SKIP;
                    _queue[ i + 3 ] = 3;
                }
            }

            _queue.Add( TOUCH_DRAGGED ); 
            
            QueueTime( time );
            
            _queue.Add( screenX );
            _queue.Add( screenY );
            _queue.Add( pointer );
        }

        return false;
    }

    /// <summary>
    /// Queues a mouse moved event with the specified parameters, skipping any previously
    /// queued mouse moved events.
    /// </summary>
    /// <param name="screenX"> The x-coordinate of the mouse. </param>
    /// <param name="screenY"> The y-coordinate of the mouse. </param>
    /// <param name="time"> The time the event occurred. </param>
    /// <returns> Always returns false. </returns>
    public bool MouseMoved( int screenX, int screenY, long time )
    {
        lock ( this )
        {
            // Skip any queued mouse moved events.
            for ( var i = Next( MOUSE_MOVED, 0 ); i >= 0; i = Next( MOUSE_MOVED, i + 5 ) )
            {
                _queue[ i ]     = SKIP;
                _queue[ i + 3 ] = 2;
            }

            _queue.Add( MOUSE_MOVED );
            
            QueueTime( time );
            
            _queue.Add( screenX );
            _queue.Add( screenY );
        }

        return false;
    }

    /// <summary>
    /// Queues a mouse scrolled event with the specified parameters.
    /// </summary>
    /// <param name="amountX">The horizontal scroll amount.</param>
    /// <param name="amountY">The vertical scroll amount.</param>
    /// <param name="time">The time the event occurred.</param>
    /// <returns>Always returns false.</returns>
    public bool Scrolled( float amountX, float amountY, long time )
    {
        lock ( this )
        {
            _queue.Add( MOUSE_SCROLLED );
            
            QueueTime( time );
            
            _queue.Add( NumberUtils.FloatToIntBits( amountX ) );
            _queue.Add( NumberUtils.FloatToIntBits( amountY ) );
        }

        return false;
    }
}
