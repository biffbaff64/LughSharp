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

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class DefaultDesktopGLInput : AbstractInput, IDesktopGLInput
{
    private DesktopGLWindow? _window;
    private IInputProcessor? _inputProcessor;
    private InputEventQueue  _eventQueue = new();

    private int    _mouseX;
    private int    _mouseY;
    private int    _mousePressed;
    private int    _deltaX;
    private int    _deltaY;
    private bool   _justTouched;
    private bool[] _justPressedButtons = new bool[ 5 ];
    private char   _lastCharacter;

    /// <inheritdoc />
    public DefaultDesktopGLInput( DesktopGLWindow? window )
    {
        ArgumentNullException.ThrowIfNull( window );

        this._window = window;

        unsafe
        {
            WindowHandleChanged( _window.WindowHandle );
        }
    }

    /// <inheritdoc />
    public void Update()
    {
        _eventQueue.Drain( _inputProcessor );
    }

    /// <inheritdoc />
    public override bool IsPeripheralAvailable( IInput.Peripheral peripheral )
    {
        return peripheral == IInput.Peripheral.HardwareKeyboard;
    }

    /// <inheritdoc />
    public override IInput.Orientation GetNativeOrientation()
    {
        return IInput.Orientation.Landscape;
    }

    /// <inheritdoc />
    public override void SetCursorCaught( bool caught )
    {
        unsafe
        {
            Glfw.SetInputMode( _window!.WindowHandle,
                               GLFW.GLFW_CURSOR,
                               caught
                                   ? GLFW.GLFW_CURSOR_DISABLED
                                   : GLFW.GLFW_CURSOR_NORMAL );
        }
    }

    /// <inheritdoc />
    public override bool IsCursorCaught()
    {
        return false;
    }

    /// <inheritdoc />
    public override void SetCursorPosition( int x, int y )
    {
    }

    /// <inheritdoc />
    public override void GetTextInput( IInput.ITextInputListener listener, string title, string text, string hint )
    {
    }

    /// <inheritdoc />
    public override void GetTextInput( IInput.ITextInputListener listener, string title, string text, string hint, IInput.OnscreenKeyboardType type )
    {
    }

    /// <inheritdoc />
    public override void GetRotationMatrix( float[] matrix )
    {
    }

    /// <inheritdoc />
    public override long GetCurrentEventTime()
    {
        return 0;
    }

    /// <inheritdoc />
    public unsafe void WindowHandleChanged( Window* windowHandle )
    {
    }

    /// <inheritdoc />
    public void PrepareNext()
    {
        if ( _justTouched )
        {
            _justTouched = false;

            Array.Fill( _justPressedButtons, false );
        }

        if ( KeyJustPressed )
        {
            KeyJustPressed = false;

            Array.Fill( JustPressedKeys, false );
        }

        _deltaX = 0;
        _deltaY = 0;
    }

    /// <inheritdoc />
    public void ResetPollingStates()
    {
        _justTouched   = false;
        KeyJustPressed = false;

        Array.Fill( JustPressedKeys, false );
        Array.Fill( _justPressedButtons, false );

        _eventQueue.Drain( null );
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    // ------------------------------------------------------------------------
    // Callbacks
    // ------------------------------------------------------------------------

    private GLFW.KeyCallback _keyCallback = KeyCallback;

    private static void KeyCallback( IntPtr window, Keys key, int scancode, InputState state, ModifierKeys mods )
    {
    }

    private GLFW.CharCallback _charCallback = CharCallback;

    private static void CharCallback( IntPtr window, uint codepoint )
    {
    }

    private GLFW.MouseCallback _mouseCallback = MouseCallback;

    private static void MouseCallback( IntPtr window, double x, double y )
    {
    }

    // ------------------------------------------------------------------------
    // Stubs
    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override float GetAccelerometerX()
    {
        return 0;
    }

    /// <inheritdoc />
    public override float GetAccelerometerY()
    {
        return 0;
    }

    /// <inheritdoc />
    public override float GetAccelerometerZ()
    {
        return 0;
    }

    /// <inheritdoc />
    public override float GetGyroscopeX()
    {
        return 0;
    }

    /// <inheritdoc />
    public override float GetGyroscopeY()
    {
        return 0;
    }

    /// <inheritdoc />
    public override float GetGyroscopeZ()
    {
        return 0;
    }

    /// <inheritdoc />
    public override int GetMaxPointers()
    {
        return 0;
    }

    /// <inheritdoc />
    public override int GetX( int pointer = 0 )
    {
        return 0;
    }

    /// <inheritdoc />
    public override int GetDeltaX( int pointer = 0 )
    {
        return 0;
    }

    /// <inheritdoc />
    public override int GetY( int pointer = 0 )
    {
        return 0;
    }

    /// <inheritdoc />
    public override int GetDeltaY( int pointer = 0 )
    {
        return 0;
    }

    /// <inheritdoc />
    public override bool IsTouched( int pointer = 0 )
    {
        return false;
    }

    /// <inheritdoc />
    public override bool JustTouched()
    {
        return false;
    }

    /// <inheritdoc />
    public override float GetPressure( int pointer = 0 )
    {
        return 0;
    }

    /// <inheritdoc />
    public override bool IsButtonPressed( int button )
    {
        return false;
    }

    /// <inheritdoc />
    public override bool IsButtonJustPressed( int button )
    {
        return false;
    }

    /// <inheritdoc />
    public override void SetInputProcessor( IInputProcessor processor )
    {
    }

    /// <inheritdoc />
    public override IInputProcessor? GetInputProcessor()
    {
        return null;
    }

    /// <inheritdoc />
    public override int GetRotation()
    {
        return 0;
    }

    /// <inheritdoc />
    public override float GetAzimuth()
    {
        return 0;
    }

    /// <inheritdoc />
    public override float GetPitch()
    {
        return 0;
    }

    /// <inheritdoc />
    public override float GetRoll()
    {
        return 0;
    }

    /// <inheritdoc />
    public override void SetOnscreenKeyboardVisible( bool visible )
    {
    }

    /// <inheritdoc />
    public override void SetOnscreenKeyboardVisible( bool visible, IInput.OnscreenKeyboardType type )
    {
    }

    /// <inheritdoc />
    public override void Vibrate( int milliseconds )
    {
    }

    /// <inheritdoc />
    public override void Vibrate( long[] pattern, int repeat )
    {
    }

    /// <inheritdoc />
    public override void CancelVibrate()
    {
    }
}
