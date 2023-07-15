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

using LibGDXSharp.Maths;

namespace LibGDXSharp.Core;

public class InputEventQueue
{
    private const int Skip           = -1;
    private const int Key_Down       = 0;
    private const int Key_Up         = 1;
    private const int Key_Typed      = 2;
    private const int Touch_Down     = 3;
    private const int Touch_Up       = 4;
    private const int Touch_Dragged  = 5;
    private const int Mouse_Moved    = 6;
    private const int Mouse_Scrolled = 7;

    private readonly List< int > _queue           = new List< int >();
    private readonly List< int > _processingQueue = new List< int >();

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
            CurrentEventTime = ( ( long )q[ i++ ] << 32 ) | ( q[ i++ ] & 0xFFFFFFFFL );

            switch ( type )
            {
                case Skip:
                    i += q[ i ];

                    break;

                case Key_Down:
                    processor.KeyDown( q[ i++ ] );

                    break;

                case Key_Up:
                    processor.KeyUp( q[ i++ ] );

                    break;

                case Key_Typed:
                    processor.KeyTyped( ( char )q[ i++ ] );

                    break;

                case Touch_Down:
                    processor.TouchDown( q[ i++ ], q[ i++ ], q[ i++ ], q[ i++ ] );

                    break;

                case Touch_Up:
                    processor.TouchUp( q[ i++ ], q[ i++ ], q[ i++ ], q[ i++ ] );

                    break;

                case Touch_Dragged:
                    processor.TouchDragged( q[ i++ ], q[ i++ ], q[ i++ ] );

                    break;

                case Mouse_Moved:
                    processor.MouseMoved( q[ i++ ], q[ i++ ] );

                    break;

                case Mouse_Scrolled:
                    processor.Scrolled
                        ( NumberUtils.IntBitsToFloat( q[ i++ ] ), NumberUtils.IntBitsToFloat( q[ i++ ] ) );

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

                if ( type == nextType ) return i;

                i += 3;

                switch ( type )
                {
                    case Skip:
                        i += q[ i ];

                        break;

                    case Key_Down:
                        i++;

                        break;

                    case Key_Up:
                        i++;

                        break;

                    case Key_Typed:
                        i++;

                        break;

                    case Touch_Down:
                        i += 4;

                        break;

                    case Touch_Up:
                        i += 4;

                        break;

                    case Touch_Dragged:
                        i += 3;

                        break;

                    case Mouse_Moved:
                        i += 2;

                        break;

                    case Mouse_Scrolled:
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
        _queue.Add( ( int )( time >> 32 ) );
        _queue.Add( ( int )time );
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
            _queue.Add( Key_Down );
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
            _queue.Add( Key_Up );

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
            _queue.Add( Key_Typed );
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
            _queue.Add( Touch_Down );

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
            _queue.Add( Touch_Up );

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
            for ( var i = Next( Touch_Dragged, 0 ); i >= 0; i = Next( Touch_Dragged, i + 6 ) )
            {
                if ( _queue[ i + 5 ] == pointer )
                {
                    _queue[ i ]     = Skip;
                    _queue[ i + 3 ] = 3;
                }
            }

            _queue.Add( Touch_Dragged );

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
            for ( var i = Next( Mouse_Moved, 0 ); i >= 0; i = Next( Mouse_Moved, i + 5 ) )
            {
                _queue[ i ]     = Skip;
                _queue[ i + 3 ] = 2;
            }

            _queue.Add( Mouse_Moved );

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
            _queue.Add( Mouse_Scrolled );

            QueueTime( time );

            _queue.Add( NumberUtils.FloatToIntBits( amountX ) );
            _queue.Add( NumberUtils.FloatToIntBits( amountY ) );
        }

        return false;
    }
}