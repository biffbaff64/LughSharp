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

using System.Diagnostics;

using LibGDXSharp.DevFillers;

namespace LibGDXSharp.Input;

/// <summary>
/// Sends all inputs from touch, key, accelerometer and compass to a <see cref="RemoteInput"/>
/// at the given ip/port. Instantiate this and call SendUpdate() periodically.
/// </summary>
public class RemoteSender : IInputProcessor
{
    public const int Key_Down = 0;
    public const int Key_Up   = 1;
    public const int Key_Typed = 2;

    public const int Touch_Down    = 3;
    public const int Touch_Up      = 4;
    public const int Touch_Dragged = 5;

    public const int Accel   = 6;
    public const int Compass = 7;
    public const int Size    = 8;
    public const int Gyro    = 9;

    private DataOutputStream? _out;
    private bool              _connected = false;

    public RemoteSender( string ip, int port )
    {
        try
        {
            Socket socket = new Socket( ip, port );
            
            socket.SetTcpNoDelay( true );
            socket.SetSoTimeout( 3000 );
            
            _out = new DataOutputStream( socket.GetOutputStream() );
            _out.WriteBoolean( Gdx.Input.IsPeripheralAvailable( IInput.Peripheral.MultitouchScreen ) );
            _connected = true;
            
            Gdx.Input.SetInputProcessor( this );
        }
        catch ( Exception )
        {
            Gdx.App.Log( "RemoteSender", "couldn't connect to " + ip + ":" + port );
        }
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

        Debug.Assert( _out != null, nameof( _out ) + " != null" );

        try
        {
            _out.WriteInt( Accel );
            _out.WriteFloat( Gdx.Input.GetAccelerometerX() );
            _out.WriteFloat( Gdx.Input.GetAccelerometerY() );
            _out.WriteFloat( Gdx.Input.GetAccelerometerZ() );
            _out.WriteInt( Compass );
            _out.WriteFloat( Gdx.Input.GetAzimuth() );
            _out.WriteFloat( Gdx.Input.GetPitch() );
            _out.WriteFloat( Gdx.Input.GetRoll() );
            _out.WriteInt( Size );
            _out.WriteFloat( Gdx.Graphics.Width );
            _out.WriteFloat( Gdx.Graphics.Height );
            _out.WriteInt( Gyro );
            _out.WriteFloat( Gdx.Input.GetGyroscopeX() );
            _out.WriteFloat( Gdx.Input.GetGyroscopeY() );
            _out.WriteFloat( Gdx.Input.GetGyroscopeZ() );
        }
        catch ( Exception )
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

        Debug.Assert( _out != null, nameof( _out ) + " != null" );

        try
        {
            _out.WriteInt( Key_Down );
            _out.WriteInt( keycode );
        }
        catch ( Exception )
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
            _out?.WriteInt( Key_Up );
            _out?.WriteInt( keycode );
        }
        catch ( Exception )
        {
            lock ( this )
            {
                _connected = false;
            }
        }

        return false;
    }
    
    public virtual bool KeyTyped(char character)
    {
        lock (this)
        {
            if (!_connected)
            {
                return false;
            }
        }

        try
        {
            _out?.WriteInt(Key_Typed);
            _out?.WriteChar(character);
        }
        catch (Exception)
        {
            lock (this)
            {
                _connected = false;
            }
        }
        return false;
    }

    public bool TouchDown(int x, int y, int pointer, int button)
    {
        lock (this)
        {
            if (!_connected)
            {
                return false;
            }
        }

        Debug.Assert( _out != null, nameof( _out ) + " != null" );

        try
        {
            _out.WriteInt(Touch_Down);
            _out.WriteInt(x);
            _out.WriteInt(y);
            _out.WriteInt(pointer);
        }
        catch (Exception)
        {
            lock (this)
            {
                _connected = false;
            }
        }
        return false;
    }

    public bool TouchUp(int x, int y, int pointer, int button)
    {
        lock (this)
        {
            if (!_connected)
            {
                return false;
            }
        }

        Debug.Assert( _out != null, nameof( _out ) + " != null" );

        try
        {
            _out.WriteInt(Touch_Up);
            _out.WriteInt(x);
            _out.WriteInt(y);
            _out.WriteInt(pointer);
        }
        catch (Exception)
        {
            lock (this)
            {
                _connected = false;
            }
        }
        return false;
    }

    public bool TouchDragged(int x, int y, int pointer)
    {
        lock (this)
        {
            if (!_connected)
            {
                return false;
            }
        }

        Debug.Assert( _out != null, nameof( _out ) + " != null" );

        try
        {
            _out.WriteInt(Touch_Dragged);
            _out.WriteInt(x);
            _out.WriteInt(y);
            _out.WriteInt(pointer);
        }
        catch (Exception)
        {
            lock (this)
            {
                _connected = false;
            }
        }
        return false;
    }

    public bool MouseMoved(int x, int y)
    {
        return false;
    }

    public bool Scrolled(float amountX, float amountY)
    {
        return false;
    }

    public virtual bool Connected
    {
        get
        {
            lock (this)
            {
                return _connected;
            }
        }
    }
}