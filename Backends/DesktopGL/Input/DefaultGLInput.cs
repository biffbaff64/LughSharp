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

namespace LibGDXSharp.Backends.Desktop.Input;

public class DefaultDesktopGLInput : AbstractInput, IDesktopGLInput
{
    private readonly InputEventQueue  _eventQueue         = new();
    private readonly bool[]           _justPressedButtons = new bool[ 5 ];
    private readonly DesktopGLWindow? _window;
    private          int              _deltaX;
    private          int              _deltaY;
    private          IInputProcessor? _inputProcessor;
    private          bool             _justTouched;
    private          char             _lastCharacter;
    private          int              _logicalMouseX;

    private int _logicalMouseY;
    private int _mousePressed;

    private int _mouseX;
    private int _mouseY;

    /// <inheritdoc />
    public DefaultDesktopGLInput( DesktopGLWindow? window )
    {
        ArgumentNullException.ThrowIfNull( window );

        _window = window;

        WindowHandleChanged( _window.GlfwWindow );
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

    /// <inheritdoc />
    public void Update() => _eventQueue.Drain( _inputProcessor );

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
    public override int GetMaxPointers() => 1;

    /// <inheritdoc />
    public override int GetX( int pointer = 0 )
    {
        if ( pointer == 0 )
        {
            return _mouseX;
        }

        return 0;
    }

    /// <inheritdoc />
    public override int GetDeltaX( int pointer = 0 )
    {
        if ( pointer == 0 )
        {
            return _deltaX;
        }

        return 0;
    }

    /// <inheritdoc />
    public override int GetY( int pointer = 0 )
    {
        if ( pointer == 0 )
        {
            return _mouseY;
        }

        return 0;
    }

    /// <inheritdoc />
    public override int GetDeltaY( int pointer = 0 )
    {
        if ( pointer == 0 )
        {
            return _deltaX;
        }

        return 0;
    }

    /// <inheritdoc />
    public override bool IsTouched( int pointer = 0 )
    {
        if ( pointer == 0 )
        {
            GdxRuntimeException.ThrowIfNull( _window );

            return ( GLFW.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputAction.Press )
                || ( GLFW.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputAction.Press )
                || ( GLFW.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputAction.Press )
                || ( GLFW.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputAction.Press )
                || ( GLFW.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputAction.Press );
        }

        return false;
    }

    /// <inheritdoc />
    public override bool JustTouched() => _justTouched;

    /// <inheritdoc />
    public override float GetPressure( int pointer = 0 ) => IsTouched( pointer ) ? 1 : 0;

    /// <inheritdoc />
    public override bool IsButtonPressed( int button ) => GLFW.GetMouseButton( _window!.GlfwWindow,
                                                                               TranslateToMouseButton( button ) )
                                                       == InputAction.Press;

    /// <inheritdoc />
    public override bool IsButtonJustPressed( int button )
    {
        if ( ( button < 0 ) || ( button >= _justPressedButtons.Length ) )
        {
            return false;
        }

        return _justPressedButtons[ button ];
    }

    /// <inheritdoc />
    public override void GetTextInput( IInput.ITextInputListener listener,
                                       string title,
                                       string text,
                                       string hint,
                                       IInput.OnscreenKeyboardType type = IInput.OnscreenKeyboardType.Default ) =>

        //FIXME: TextInput does nothing ( this fixme from Java/LibGdx )
        listener.Canceled();

    /// <inheritdoc />
    public override long GetCurrentEventTime() =>

        // queue sets its event time for each event dequeued/processed
        _eventQueue.CurrentEventTime;

    /// <inheritdoc />
    public override void SetCursorCaught( bool caught ) => GLFW.SetInputMode( _window!.GlfwWindow,
                                                                              CursorStateAttribute.Cursor,
                                                                              caught
                                                                                  ? CursorModeValue.CursorDisabled
                                                                                  : CursorModeValue.CursorNormal );

    /// <inheritdoc />
    public override bool IsCursorCaught() => GLFW.GetInputMode( _window!.GlfwWindow, CursorStateAttribute.Cursor )
                                          == CursorModeValue.CursorDisabled;

    /// <inheritdoc />
    public override void SetCursorPosition( int x, int y )
    {
        if ( _window!.Config.HdpiMode == HdpiMode.Pixels )
        {
            var xScale = _window!.Graphics.LogicalWidth / ( float )_window!.Graphics.BackBufferWidth;
            var yScale = _window!.Graphics.LogicalHeight / ( float )_window!.Graphics.BackBufferHeight;

            x = ( int )( x * xScale );
            y = ( int )( y * yScale );
        }

        GLFW.SetCursorPos( _window!.GlfwWindow, x, y );
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing,
    ///     or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public void WindowHandleChanged( GLFW.Window windowHandle )
    {
        ResetPollingStates();

        GLFW.SetKeyCallback( windowHandle, KeyCallback );
        GLFW.SetCharCallback( windowHandle, CharCallback );
        GLFW.SetMouseButtonCallback( windowHandle, MouseCallback );
        GLFW.SetScrollCallback( windowHandle, ScrollCallback );
        GLFW.SetCursorPosCallback( windowHandle, CursorPosCallback );
    }

    // ------------------------------------------------------------------------
    // Stubs
    // ------------------------------------------------------------------------

    public override float GetAccelerometerX() => 0;
    public override float GetAccelerometerY() => 0;
    public override float GetAccelerometerZ() => 0;
    public override int   GetRotation()       => 0;
    public override float GetAzimuth()        => 0;
    public override float GetPitch()          => 0;
    public override float GetRoll()           => 0;
    public override float GetGyroscopeX()     => 0;
    public override float GetGyroscopeY()     => 0;
    public override float GetGyroscopeZ()     => 0;

    public override bool IsPeripheralAvailable( IInput.Peripheral peripheral ) => peripheral == IInput.Peripheral.HardwareKeyboard;

    public override IInput.Orientation GetNativeOrientation() => IInput.Orientation.Landscape;

    public override void SetOnscreenKeyboardVisible( bool visible )
    {
    }

    public override void SetOnscreenKeyboardVisible( bool visible, IInput.OnscreenKeyboardType type )
    {
    }

    public override void Vibrate( int milliseconds )
    {
    }

    public override void Vibrate( long[] pattern, int repeat )
    {
    }

    public override void CancelVibrate()
    {
    }

    public override void GetRotationMatrix( float[] matrix )
    {
    }

    private MouseButton TranslateToMouseButton( int button ) => button switch
                                                                {
                                                                    0 => MouseButton.Button1,
                                                                    1 => MouseButton.Button2,
                                                                    2 => MouseButton.Button3,
                                                                    3 => MouseButton.Button4,
                                                                    4 => MouseButton.Button5,
                                                                    5 => MouseButton.Button6,
                                                                    6 => MouseButton.Button7,
                                                                    7 => MouseButton.Button8,
                                                                    _ => MouseButton.Last
                                                                };

    protected char CharacterForKeyCode( int key )
    {
        switch ( key )
        {
            case IInput.Keys.BACKSPACE:
                return ( char )8;

            case IInput.Keys.TAB:
                return '\t';

            case IInput.Keys.FORWARD_DEL:
                return ( char )127;

            case IInput.Keys.NUMPAD_ENTER:
            case IInput.Keys.ENTER:
                return '\n';
        }

        return ( char )0;
    }

    public int GetGdxKeycode( Keys glKeycode )
    {
        switch ( glKeycode )
        {
            case Keys.Space:
                return IInput.Keys.SPACE;

            case Keys.Apostrophe:
                return IInput.Keys.APOSTROPHE;

            case Keys.Comma:
                return IInput.Keys.COMMA;

            case Keys.Minus:
                return IInput.Keys.MINUS;

            case Keys.Period:
                return IInput.Keys.PERIOD;

            case Keys.Slash:
                return IInput.Keys.SLASH;

            case Keys.D0:
                return IInput.Keys.NUM_0;

            case Keys.D1:
                return IInput.Keys.NUM_1;

            case Keys.D2:
                return IInput.Keys.NUM_2;

            case Keys.D3:
                return IInput.Keys.NUM_3;

            case Keys.D4:
                return IInput.Keys.NUM_4;

            case Keys.D5:
                return IInput.Keys.NUM_5;

            case Keys.D6:
                return IInput.Keys.NUM_6;

            case Keys.D7:
                return IInput.Keys.NUM_7;

            case Keys.D8:
                return IInput.Keys.NUM_8;

            case Keys.D9:
                return IInput.Keys.NUM_9;

            case Keys.Semicolon:
                return IInput.Keys.SEMICOLON;

            case Keys.Equal:
                return IInput.Keys.EQUALS_SIGN;

            case Keys.A:
                return IInput.Keys.A;

            case Keys.B:
                return IInput.Keys.B;

            case Keys.C:
                return IInput.Keys.C;

            case Keys.D:
                return IInput.Keys.D;

            case Keys.E:
                return IInput.Keys.E;

            case Keys.F:
                return IInput.Keys.F;

            case Keys.G:
                return IInput.Keys.G;

            case Keys.H:
                return IInput.Keys.H;

            case Keys.I:
                return IInput.Keys.I;

            case Keys.J:
                return IInput.Keys.J;

            case Keys.K:
                return IInput.Keys.K;

            case Keys.L:
                return IInput.Keys.L;

            case Keys.M:
                return IInput.Keys.M;

            case Keys.N:
                return IInput.Keys.N;

            case Keys.O:
                return IInput.Keys.O;

            case Keys.P:
                return IInput.Keys.P;

            case Keys.Q:
                return IInput.Keys.Q;

            case Keys.R:
                return IInput.Keys.R;

            case Keys.S:
                return IInput.Keys.S;

            case Keys.T:
                return IInput.Keys.T;

            case Keys.U:
                return IInput.Keys.U;

            case Keys.V:
                return IInput.Keys.V;

            case Keys.W:
                return IInput.Keys.W;

            case Keys.X:
                return IInput.Keys.X;

            case Keys.Y:
                return IInput.Keys.Y;

            case Keys.Z:
                return IInput.Keys.Z;

            case Keys.LeftBracket:
                return IInput.Keys.LEFT_BRACKET;

            case Keys.Backslash:
                return IInput.Keys.BACKSLASH;

            case Keys.RightBracket:
                return IInput.Keys.RIGHT_BRACKET;

            case Keys.GraveAccent:
                return IInput.Keys.GRAVE;

            case Keys.Unknown:
                return IInput.Keys.UNKNOWN;

            case Keys.Escape:
                return IInput.Keys.ESCAPE;

            case Keys.Enter:
                return IInput.Keys.ENTER;

            case Keys.Tab:
                return IInput.Keys.TAB;

            case Keys.Backspace:
                return IInput.Keys.BACKSPACE;

            case Keys.Insert:
                return IInput.Keys.INSERT;

            case Keys.Delete:
                return IInput.Keys.FORWARD_DEL;

            case Keys.Right:
                return IInput.Keys.RIGHT;

            case Keys.Left:
                return IInput.Keys.LEFT;

            case Keys.Down:
                return IInput.Keys.DOWN;

            case Keys.Up:
                return IInput.Keys.UP;

            case Keys.PageUp:
                return IInput.Keys.PAGE_UP;

            case Keys.PageDown:
                return IInput.Keys.PAGE_DOWN;

            case Keys.Home:
                return IInput.Keys.HOME;

            case Keys.End:
                return IInput.Keys.END;

            case Keys.CapsLock:
                return IInput.Keys.CAPS_LOCK;

            case Keys.ScrollLock:
                return IInput.Keys.SCROLL_LOCK;

            case Keys.PrintScreen:
                return IInput.Keys.PRINT_SCREEN;

            case Keys.Pause:
                return IInput.Keys.PAUSE;

            case Keys.F1:
                return IInput.Keys.F1;

            case Keys.F2:
                return IInput.Keys.F2;

            case Keys.F3:
                return IInput.Keys.F3;

            case Keys.F4:
                return IInput.Keys.F4;

            case Keys.F5:
                return IInput.Keys.F5;

            case Keys.F6:
                return IInput.Keys.F6;

            case Keys.F7:
                return IInput.Keys.F7;

            case Keys.F8:
                return IInput.Keys.F8;

            case Keys.F9:
                return IInput.Keys.F9;

            case Keys.F10:
                return IInput.Keys.F10;

            case Keys.F11:
                return IInput.Keys.F11;

            case Keys.F12:
                return IInput.Keys.F12;

            case Keys.F13:
                return IInput.Keys.F13;

            case Keys.F14:
                return IInput.Keys.F14;

            case Keys.F15:
                return IInput.Keys.F15;

            case Keys.F16:
                return IInput.Keys.F16;

            case Keys.F17:
                return IInput.Keys.F17;

            case Keys.F18:
                return IInput.Keys.F18;

            case Keys.F19:
                return IInput.Keys.F19;

            case Keys.F20:
                return IInput.Keys.F20;

            case Keys.F21:
                return IInput.Keys.F21;

            case Keys.F22:
                return IInput.Keys.F22;

            case Keys.F23:
                return IInput.Keys.F23;

            case Keys.F24:
                return IInput.Keys.F24;

            case Keys.F25:
                return IInput.Keys.UNKNOWN;

            case Keys.NumLock:
                return IInput.Keys.NUM_LOCK;

            case Keys.KeyPad0:
                return IInput.Keys.NUMPAD_0;

            case Keys.KeyPad1:
                return IInput.Keys.NUMPAD_1;

            case Keys.KeyPad2:
                return IInput.Keys.NUMPAD_2;

            case Keys.KeyPad3:
                return IInput.Keys.NUMPAD_3;

            case Keys.KeyPad4:
                return IInput.Keys.NUMPAD_4;

            case Keys.KeyPad5:
                return IInput.Keys.NUMPAD_5;

            case Keys.KeyPad6:
                return IInput.Keys.NUMPAD_6;

            case Keys.KeyPad7:
                return IInput.Keys.NUMPAD_7;

            case Keys.KeyPad8:
                return IInput.Keys.NUMPAD_8;

            case Keys.KeyPad9:
                return IInput.Keys.NUMPAD_9;

            case Keys.KeyPadDecimal:
                return IInput.Keys.NUMPAD_DOT;

            case Keys.KeyPadDivide:
                return IInput.Keys.NUMPAD_DIVIDE;

            case Keys.KeyPadMultiply:
                return IInput.Keys.NUMPAD_MULTIPLY;

            case Keys.KeyPadSubtract:
                return IInput.Keys.NUMPAD_SUBTRACT;

            case Keys.KeyPadAdd:
                return IInput.Keys.NUMPAD_ADD;

            case Keys.KeyPadEnter:
                return IInput.Keys.NUMPAD_ENTER;

            case Keys.KeyPadEqual:
                return IInput.Keys.NUMPAD_EQUALS;

            case Keys.LeftShift:
                return IInput.Keys.SHIFT_LEFT;

            case Keys.LeftControl:
                return IInput.Keys.CONTROL_LEFT;

            case Keys.LeftAlt:
                return IInput.Keys.ALT_LEFT;

            case Keys.LeftSuper:
                return IInput.Keys.SYM;

            case Keys.RightShift:
                return IInput.Keys.SHIFT_RIGHT;

            case Keys.RightControl:
                return IInput.Keys.CONTROL_RIGHT;

            case Keys.RightAlt:
                return IInput.Keys.ALT_RIGHT;

            case Keys.RightSuper:
                return IInput.Keys.SYM;

            case Keys.Menu:
                return IInput.Keys.MENU;

            default:
                return IInput.Keys.UNKNOWN;
        }
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing,
    ///     or resetting unmanaged resources.
    /// </summary>
    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    // ------------------------------------------------------------------------
    // Callbacks
    // ------------------------------------------------------------------------

    private void KeyCallback( GLFW.Window window,
                              Keys key,
                              int scancode,
                              InputAction action,
                              KeyModifiers mods )
    {
        int gdxKey;

        switch ( action )
        {
            case InputAction.Press:
            {
                gdxKey = GetGdxKeycode( key );

                _eventQueue.KeyDown( gdxKey, TimeUtils.NanoTime() );

                PressedKeyCount++;
                KeyJustPressed            = true;
                PressedKeys[ gdxKey ]     = true;
                JustPressedKeys[ gdxKey ] = true;

                _window?.Graphics.RequestRendering();
                _lastCharacter = ( char )0;

                var character = CharacterForKeyCode( gdxKey );

                if ( character != 0 )
                {
                    CharCallback( window, character );
                }

                break;
            }

            case InputAction.Release:
            {
                gdxKey = GetGdxKeycode( key );

                PressedKeyCount--;
                PressedKeys[ gdxKey ] = false;

                _window?.Graphics.RequestRendering();

                _eventQueue.KeyUp( gdxKey, TimeUtils.NanoTime() );

                break;
            }

            case InputAction.Repeat:
            {
                if ( _lastCharacter != 0 )
                {
                    _window?.Graphics.RequestRendering();

                    _eventQueue.KeyTyped( _lastCharacter, TimeUtils.NanoTime() );
                }

                break;
            }
        }
    }

    private void CharCallback( GLFW.Window window, uint codepoint )
    {
        if ( ( codepoint & 0xff00 ) == 0xf700 )
        {
            return;
        }

        _lastCharacter = ( char )codepoint;
        _window?.Graphics.RequestRendering();
        _eventQueue.KeyTyped( ( char )codepoint, TimeUtils.NanoTime() );
    }

    private void MouseCallback( GLFW.Window window, MouseButton button, InputAction action, KeyModifiers mods )
    {
        var gdxButton = button switch
                        {
                            MouseButton.Left    => IInput.Buttons.LEFT,
                            MouseButton.Right   => IInput.Buttons.RIGHT,
                            MouseButton.Middle  => IInput.Buttons.MIDDLE,
                            MouseButton.Button4 => IInput.Buttons.BACK,
                            MouseButton.Button5 => IInput.Buttons.FORWARD,
                            _                   => -1
                        };


        if ( Enum.IsDefined( typeof( MouseButton ), button ) && ( gdxButton == -1 ) )
        {
            return;
        }

        var time = TimeUtils.NanoTime();

        if ( action == InputAction.Press )
        {
            _mousePressed++;
            _justTouched                     = true;
            _justPressedButtons[ gdxButton ] = true;

            _window?.Graphics.RequestRendering();
            _eventQueue.TouchDown( _mouseX, _mouseY, 0, gdxButton, time );
        }
        else
        {
            _mousePressed = Math.Max( 0, _mousePressed - 1 );

            _window?.Graphics.RequestRendering();
            _eventQueue.TouchUp( _mouseX, _mouseY, 0, gdxButton, time );
        }
    }

    public void ScrollCallback( GLFW.Window window, double x, double y )
    {
        _window?.Graphics.RequestRendering();
        _eventQueue.Scrolled( -( float )x, -( float )y, TimeUtils.NanoTime() );
    }

    public void CursorPosCallback( GLFW.Window window, double x, double y )
    {
        _deltaX = ( int )x - _logicalMouseX;
        _deltaY = ( int )y - _logicalMouseY;
        _mouseX = _logicalMouseX = ( int )x;
        _mouseY = _logicalMouseY = ( int )y;

        if ( _window?.Config.HdpiMode == HdpiMode.Pixels )
        {
            // null check can be surpressed here because of above
            var xScale = _window!.Graphics.BackBufferWidth / ( float )_window!.Graphics.LogicalWidth;
            var yScale = _window!.Graphics.BackBufferHeight / ( float )_window!.Graphics.LogicalHeight;

            _deltaX = ( int )( _deltaX * xScale );
            _deltaY = ( int )( _deltaY * yScale );
            _mouseX = ( int )( _mouseX * xScale );
            _mouseY = ( int )( _mouseY * yScale );
        }

        _window?.Graphics.RequestRendering();

        if ( _mousePressed > 0 )
        {
            _eventQueue.TouchDragged( _mouseX, _mouseY, 0, TimeUtils.NanoTime() );
        }
        else
        {
            _eventQueue.MouseMoved( _mouseX, _mouseY, TimeUtils.NanoTime() );
        }
    }
}
