// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LughSharp.LibCore.Core;

[PublicAPI]
public class InputEventQueue
{
    private const int SKIP           = -1;
    private const int KEY_DOWN       = 0;
    private const int KEY_UP         = 1;
    private const int KEY_TYPED      = 2;
    private const int TOUCH_DOWN     = 3;
    private const int TOUCH_UP       = 4;
    private const int TOUCH_DRAGGED  = 5;
    private const int MOUSE_MOVED    = 6;
    private const int MOUSE_SCROLLED = 7;

    private readonly List< int > _processingQueue = new();

    private readonly List< int > _queue = new();

    public long CurrentEventTime { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="processor"></param>
    /// <exception cref="SystemException"></exception>
    public void Drain( IInputProcessor? processor )
    {
        lock ( this )
        {
            if ( processor == null )
            {
                _queue.Clear();

                return;
            }

            _processingQueue.AddRange( _queue );
            _queue.Clear();
        }

        var q = _processingQueue.ToArray();

        for ( int i = 0, n = _processingQueue.Count; i < n; )
        {
            var type = q[ i++ ];
            CurrentEventTime = ( ( long ) q[ i++ ] << 32 ) | ( q[ i++ ] & 0xFFFFFFFFL );

            switch ( type )
            {
                case SKIP:
                    i += q[ i ];

                    break;

                case KEY_DOWN:
                    processor.KeyDown( q[ i++ ] );

                    break;

                case KEY_UP:
                    processor.KeyUp( q[ i++ ] );

                    break;

                case KEY_TYPED:
                    processor.KeyTyped( ( char ) q[ i++ ] );

                    break;

                case TOUCH_DOWN:
                    processor.TouchDown( q[ i++ ], q[ i++ ], q[ i++ ], q[ i++ ] );

                    break;

                case TOUCH_UP:
                    processor.TouchUp( q[ i++ ], q[ i++ ], q[ i++ ], q[ i++ ] );

                    break;

                case TOUCH_DRAGGED:
                    processor.TouchDragged( q[ i++ ], q[ i++ ], q[ i++ ] );

                    break;

                case MOUSE_MOVED:
                    processor.MouseMoved( q[ i++ ], q[ i++ ] );

                    break;

                case MOUSE_SCROLLED:
                    processor.Scrolled( NumberUtils.IntBitsToFloat( q[ i++ ] ), NumberUtils.IntBitsToFloat( q[ i++ ] ) );

                    break;

                default:
                    throw new SystemException();
            }
        }

        _processingQueue.Clear();
    }

    /// <summary>
    /// </summary>
    /// <param name="nextType"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    /// <exception cref="SystemException"></exception>
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
                        i += q[ i ];

                        break;

                    case KEY_DOWN:
                    case KEY_UP:
                    case KEY_TYPED:
                        i++;

                        break;

                    case TOUCH_DOWN:
                    case TOUCH_UP:
                        i += 4;

                        break;

                    case TOUCH_DRAGGED:
                        i += 3;

                        break;

                    case MOUSE_MOVED:
                    case MOUSE_SCROLLED:
                        i += 2;

                        break;

                    default:
                        throw new SystemException();
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// </summary>
    /// <param name="time"></param>
    private void QueueTime( long time )
    {
        _queue.Add( ( int ) ( time >> 32 ) );
        _queue.Add( ( int ) time );
    }

    /// <summary>
    /// </summary>
    /// <param name="keycode"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="keycode"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="character"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="pointer"></param>
    /// <param name="button"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="pointer"></param>
    /// <param name="button"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="pointer"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="amountX"></param>
    /// <param name="amountY"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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