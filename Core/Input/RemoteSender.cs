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

/// <summary>
/// Sends all inputs from touch, key, accelerometer and compass to
/// a <see cref="RemoteInput"/> at the given ip/port. Instantiate
/// this and call SendUpdate() periodically.
/// </summary>
[PublicAPI]
public class RemoteSender : IInputProcessor
{
    public const int KEY_DOWN  = 0;
    public const int KEY_UP    = 1;
    public const int KEY_TYPED = 2;

    public const int TOUCH_DOWN    = 3;
    public const int TOUCH_UP      = 4;
    public const int TOUCH_DRAGGED = 5;

    public const int ACCEL   = 6;
    public const int COMPASS = 7;
    public const int SIZE    = 8;
    public const int GYRO    = 9;

    private BinaryWriter? _out;
    private bool          _connected = false;

    public RemoteSender( string ip, int port )
    {
//        try
//        {
//            Socket socket = new Socket( ip, port );

//            _out = new DataOutputStream( socket.GetOutputStream() );
//            _out.WriteBoolean( Gdx.Input.IsPeripheralAvailable( IInput.Peripheral.MultitouchScreen ) );

//            _out = new BinaryWriter()
//            _connected = true;

//            Gdx.Input.SetInputProcessor( this );
//        }
//        catch ( System.Exception )
//        {
//            Gdx.App.Log( "RemoteSender", "couldn't connect to " + ip + ":" + port );
//        }
    }

    public void SendUpdate()
    {
        lock ( this )
        {
            if ( !_connected )
            {
                return;
            }
        }

        Debug.Assert( _out != null, nameof( _out ) + " is null" );

        try
        {
            _out.Write( ACCEL );
            _out.Write( Gdx.Input.GetAccelerometerX() );
            _out.Write( Gdx.Input.GetAccelerometerY() );
            _out.Write( Gdx.Input.GetAccelerometerZ() );
            _out.Write( COMPASS );
            _out.Write( Gdx.Input.GetAzimuth() );
            _out.Write( Gdx.Input.GetPitch() );
            _out.Write( Gdx.Input.GetRoll() );
            _out.Write( SIZE );
            _out.Write( Gdx.Graphics.Width );
            _out.Write( Gdx.Graphics.Height );
            _out.Write( GYRO );
            _out.Write( Gdx.Input.GetGyroscopeX() );
            _out.Write( Gdx.Input.GetGyroscopeY() );
            _out.Write( Gdx.Input.GetGyroscopeZ() );
        }
        catch ( System.Exception )
        {
            _out       = null;
            _connected = false;
        }
    }

    public bool KeyDown( int keycode )
    {
        lock ( this )
        {
            if ( !_connected )
            {
                return false;
            }
        }

        Debug.Assert( _out != null, nameof( _out ) + " is null" );

        try
        {
            _out.Write( KEY_DOWN );
            _out.Write( keycode );
        }
        catch ( System.Exception )
        {
            lock ( this )
            {
                _connected = false;
            }
        }

        return false;
    }

    public bool KeyUp( int keycode )
    {
        lock ( this )
        {
            if ( !_connected )
            {
                return false;
            }
        }

        try
        {
            _out?.Write( KEY_UP );
            _out?.Write( keycode );
        }
        catch ( System.Exception )
        {
            lock ( this )
            {
                _connected = false;
            }
        }

        return false;
    }

    public virtual bool KeyTyped( char character )
    {
        lock ( this )
        {
            if ( !_connected )
            {
                return false;
            }
        }

        try
        {
            _out?.Write( KEY_TYPED );
            _out?.Write( character );
        }
        catch ( System.Exception )
        {
            lock ( this )
            {
                _connected = false;
            }
        }

        return false;
    }

    public bool TouchDown( int x, int y, int pointer, int button )
    {
        lock ( this )
        {
            if ( !_connected )
            {
                return false;
            }
        }

        Debug.Assert( _out != null, nameof( _out ) + " is null" );

        try
        {
            _out.Write( TOUCH_DOWN );
            _out.Write( x );
            _out.Write( y );
            _out.Write( pointer );
        }
        catch ( System.Exception )
        {
            lock ( this )
            {
                _connected = false;
            }
        }

        return false;
    }

    public bool TouchUp( int x, int y, int pointer, int button )
    {
        lock ( this )
        {
            if ( !_connected )
            {
                return false;
            }
        }

        Debug.Assert( _out != null, nameof( _out ) + " is null" );

        try
        {
            _out.Write( TOUCH_UP );
            _out.Write( x );
            _out.Write( y );
            _out.Write( pointer );
        }
        catch ( System.Exception )
        {
            lock ( this )
            {
                _connected = false;
            }
        }

        return false;
    }

    public bool TouchDragged( int x, int y, int pointer )
    {
        lock ( this )
        {
            if ( !_connected )
            {
                return false;
            }
        }

        Debug.Assert( _out != null, nameof( _out ) + " is null" );

        try
        {
            _out.Write( TOUCH_DRAGGED );
            _out.Write( x );
            _out.Write( y );
            _out.Write( pointer );
        }
        catch ( System.Exception )
        {
            lock ( this )
            {
                _connected = false;
            }
        }

        return false;
    }

    public bool MouseMoved( int x, int y )
    {
        return false;
    }

    public bool Scrolled( float amountX, float amountY )
    {
        return false;
    }

    public virtual bool Connected
    {
        get
        {
            lock ( this )
            {
                return _connected;
            }
        }
    }
}
