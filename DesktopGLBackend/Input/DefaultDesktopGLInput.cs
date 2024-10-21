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

using Corelib.LibCore.Core;
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Exceptions;

namespace DesktopGLBackend.Input;

[PublicAPI]
public class DefaultDesktopGLInput : AbstractInput, IDesktopGLInput
{
    private const int DEFAULT_MAX_POINTERS = 1;

    private readonly InputEventQueue  _eventQueue         = new();
    private readonly bool[]           _justPressedButtons = new bool[ 5 ];
    private readonly DesktopGLWindow? _window;

    private bool _justTouched;
    private char _lastCharacter;
    private int  _logicalMouseX;
    private int  _deltaX;
    private int  _deltaY;
    private int  _logicalMouseY;
    private int  _mousePressed;
    private int  _mouseX;
    private int  _mouseY;

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public DefaultDesktopGLInput( DesktopGLWindow? window )
    {
        ArgumentNullException.ThrowIfNull( window );

        _window = window;

        WindowHandleChanged( _window.GlfwWindow );
    }

    // ------------------------------------------------------------------------

    #region From IDesktopGLInput

    /// <inheritdoc />
    public void WindowHandleChanged( GLFW.Window? windowHandle )
    {
        ResetPollingStates();

        Glfw.SetKeyCallback( windowHandle, KeyCallback );
        Glfw.SetCharCallback( windowHandle, CharCallback );
        Glfw.SetMouseButtonCallback( windowHandle, MouseCallback );
        Glfw.SetScrollCallback( windowHandle, ScrollCallback );
        Glfw.SetCursorPosCallback( windowHandle, CursorPosCallback );
    }

    /// <inheritdoc />
    public void Update()
    {
        _eventQueue.Drain( InputProcessor );
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

    #endregion From IDesktopGLInput

    // ------------------------------------------------------------------------

    #region From Abstract Input

    /// <inheritdoc />
    public override int GetMaxPointers() => DEFAULT_MAX_POINTERS;

    /// <inheritdoc />
    public override int GetX( int pointer = 0 ) => pointer == 0 ? _mouseX : 0;

    /// <inheritdoc />
    public override int GetDeltaX( int pointer = 0 ) => pointer == 0 ? _deltaX : 0;

    /// <inheritdoc />
    public override int GetY( int pointer = 0 ) => pointer == 0 ? _mouseY : 0;

    /// <inheritdoc />
    public override int GetDeltaY( int pointer = 0 ) => pointer == 0 ? _deltaY : 0;

    /// <inheritdoc />
    public override bool IsTouched( int pointer = 0 )
    {
        if ( pointer == 0 )
        {
            GdxRuntimeException.ThrowIfNull( _window );

            return ( Glfw.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputState.Press )
                || ( Glfw.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputState.Press )
                || ( Glfw.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputState.Press )
                || ( Glfw.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputState.Press )
                || ( Glfw.GetMouseButton( _window.GlfwWindow, MouseButton.Button1 ) == InputState.Press );
        }

        return false;
    }

    /// <inheritdoc />
    public override bool JustTouched() => _justTouched;

    /// <inheritdoc />
    public override float GetPressure( int pointer = 0 ) => IsTouched( pointer ) ? 1 : 0;

    /// <inheritdoc />
    public override bool IsButtonPressed( int button )
    {
        return Glfw.GetMouseButton( _window!.GlfwWindow, TranslateToMouseButton( button ) )
            == InputState.Press;
    }

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
                                       IInput.OnscreenKeyboardType type = IInput.OnscreenKeyboardType.Default )
    {
        //FIXME: TextInput does nothing ( this fixme from Java/LibGdx )
        listener.Canceled();
    }

    /// <inheritdoc />
    public override long GetCurrentEventTime()
    {
        // queue sets its event time for each event dequeued/processed
        return _eventQueue.CurrentEventTime;
    }

    /// <inheritdoc />
    public override void SetCursorCaught( bool caught )
    {
        Glfw.SetInputMode( _window!.GlfwWindow, InputMode.Cursor, ( caught ? CursorMode.Disabled : CursorMode.Normal ) );
    }

    /// <inheritdoc />
    public override bool IsCursorCaught()
    {
        return Glfw.GetInputMode( _window!.GlfwWindow, InputMode.Cursor ) == CursorMode.Disabled;
    }

    /// <inheritdoc />
    public override void SetCursorPosition( int x, int y )
    {
        if ( _window!.Config.HdpiMode == HdpiMode.Pixels )
        {
            var xScale = _window!.Graphics.LogicalWidth / ( float ) _window!.Graphics.BackBufferWidth;
            var yScale = _window!.Graphics.LogicalHeight / ( float ) _window!.Graphics.BackBufferHeight;

            x = ( int ) ( x * xScale );
            y = ( int ) ( y * yScale );
        }

        Glfw.SetCursorPos( _window!.GlfwWindow, x, y );
    }

    public override bool IsPeripheralAvailable( IInput.Peripheral peripheral )
    {
        return peripheral == IInput.Peripheral.HardwareKeyboard;
    }

    public override IInput.Orientation GetNativeOrientation()
    {
        return IInput.Orientation.Landscape;
    }

    private static MouseButton TranslateToMouseButton( int button )
    {
        return button switch
        {
            0     => MouseButton.Button1,
            1     => MouseButton.Button2,
            2     => MouseButton.Button3,
            3     => MouseButton.Button4,
            4     => MouseButton.Button5,
            5     => MouseButton.Button6,
            6     => MouseButton.Button7,
            7     => MouseButton.Button8,
            var _ => throw new GdxRuntimeException( $"Unknown MouseButton: {button}" )
        };
    }

    protected static char CharacterForKeyCode( int key )
    {
        return key switch
        {
            IInput.Keys.BACKSPACE    => ( char ) 8,
            IInput.Keys.TAB          => '\t',
            IInput.Keys.FORWARD_DEL  => ( char ) 127,
            IInput.Keys.NUMPAD_ENTER => '\n',
            IInput.Keys.ENTER        => '\n',
            var _                    => ( char ) 0
        };
    }

    public static int GetGdxKeycode( Key glKeycode )
    {
        return glKeycode switch
        {
            Key.Space        => IInput.Keys.SPACE,
            Key.Apostrophe   => IInput.Keys.APOSTROPHE,
            Key.Comma        => IInput.Keys.COMMA,
            Key.Minus        => IInput.Keys.MINUS,
            Key.Period       => IInput.Keys.PERIOD,
            Key.Slash        => IInput.Keys.SLASH,
            Key.D0           => IInput.Keys.NUM_0,
            Key.D1           => IInput.Keys.NUM_1,
            Key.D2           => IInput.Keys.NUM_2,
            Key.D3           => IInput.Keys.NUM_3,
            Key.D4           => IInput.Keys.NUM_4,
            Key.D5           => IInput.Keys.NUM_5,
            Key.D6           => IInput.Keys.NUM_6,
            Key.D7           => IInput.Keys.NUM_7,
            Key.D8           => IInput.Keys.NUM_8,
            Key.D9           => IInput.Keys.NUM_9,
            Key.Semicolon    => IInput.Keys.SEMICOLON,
            Key.Equal        => IInput.Keys.EQUALS_SIGN,
            Key.A            => IInput.Keys.A,
            Key.B            => IInput.Keys.B,
            Key.C            => IInput.Keys.C,
            Key.D            => IInput.Keys.D,
            Key.E            => IInput.Keys.E,
            Key.F            => IInput.Keys.F,
            Key.G            => IInput.Keys.G,
            Key.H            => IInput.Keys.H,
            Key.I            => IInput.Keys.I,
            Key.J            => IInput.Keys.J,
            Key.K            => IInput.Keys.K,
            Key.L            => IInput.Keys.L,
            Key.M            => IInput.Keys.M,
            Key.N            => IInput.Keys.N,
            Key.O            => IInput.Keys.O,
            Key.P            => IInput.Keys.P,
            Key.Q            => IInput.Keys.Q,
            Key.R            => IInput.Keys.R,
            Key.S            => IInput.Keys.S,
            Key.T            => IInput.Keys.T,
            Key.U            => IInput.Keys.U,
            Key.V            => IInput.Keys.V,
            Key.W            => IInput.Keys.W,
            Key.X            => IInput.Keys.X,
            Key.Y            => IInput.Keys.Y,
            Key.Z            => IInput.Keys.Z,
            Key.LeftBracket  => IInput.Keys.LEFT_BRACKET,
            Key.Backslash    => IInput.Keys.BACKSLASH,
            Key.RightBracket => IInput.Keys.RIGHT_BRACKET,
            Key.GraveAccent  => IInput.Keys.GRAVE,

//            Key.Unknown      => IInput.Keys.UNKNOWN,
            Key.Escape       => IInput.Keys.ESCAPE,
            Key.Enter        => IInput.Keys.ENTER,
            Key.Tab          => IInput.Keys.TAB,
            Key.Backspace    => IInput.Keys.BACKSPACE,
            Key.Insert       => IInput.Keys.INSERT,
            Key.Delete       => IInput.Keys.FORWARD_DEL,
            Key.Right        => IInput.Keys.RIGHT,
            Key.Left         => IInput.Keys.LEFT,
            Key.Down         => IInput.Keys.DOWN,
            Key.Up           => IInput.Keys.UP,
            Key.PageUp       => IInput.Keys.PAGE_UP,
            Key.PageDown     => IInput.Keys.PAGE_DOWN,
            Key.Home         => IInput.Keys.HOME,
            Key.End          => IInput.Keys.END,
            Key.CapsLock     => IInput.Keys.CAPS_LOCK,
            Key.ScrollLock   => IInput.Keys.SCROLL_LOCK,
            Key.PrintScreen  => IInput.Keys.PRINT_SCREEN,
            Key.Pause        => IInput.Keys.PAUSE,
            Key.F1           => IInput.Keys.F1,
            Key.F2           => IInput.Keys.F2,
            Key.F3           => IInput.Keys.F3,
            Key.F4           => IInput.Keys.F4,
            Key.F5           => IInput.Keys.F5,
            Key.F6           => IInput.Keys.F6,
            Key.F7           => IInput.Keys.F7,
            Key.F8           => IInput.Keys.F8,
            Key.F9           => IInput.Keys.F9,
            Key.F10          => IInput.Keys.F10,
            Key.F11          => IInput.Keys.F11,
            Key.F12          => IInput.Keys.F12,
            Key.F13          => IInput.Keys.F13,
            Key.F14          => IInput.Keys.F14,
            Key.F15          => IInput.Keys.F15,
            Key.F16          => IInput.Keys.F16,
            Key.F17          => IInput.Keys.F17,
            Key.F18          => IInput.Keys.F18,
            Key.F19          => IInput.Keys.F19,
            Key.F20          => IInput.Keys.F20,
            Key.F21          => IInput.Keys.F21,
            Key.F22          => IInput.Keys.F22,
            Key.F23          => IInput.Keys.F23,
            Key.F24          => IInput.Keys.F24,
            Key.F25          => IInput.Keys.UNKNOWN,
            Key.NumLock      => IInput.Keys.NUM_LOCK,
            Key.Kp0          => IInput.Keys.NUMPAD_0,
            Key.Kp1          => IInput.Keys.NUMPAD_1,
            Key.Kp2          => IInput.Keys.NUMPAD_2,
            Key.Kp3          => IInput.Keys.NUMPAD_3,
            Key.Kp4          => IInput.Keys.NUMPAD_4,
            Key.Kp5          => IInput.Keys.NUMPAD_5,
            Key.Kp6          => IInput.Keys.NUMPAD_6,
            Key.Kp7          => IInput.Keys.NUMPAD_7,
            Key.Kp8          => IInput.Keys.NUMPAD_8,
            Key.Kp9          => IInput.Keys.NUMPAD_9,
            Key.KpDecimal    => IInput.Keys.NUMPAD_DOT,
            Key.KpDivide     => IInput.Keys.NUMPAD_DIVIDE,
            Key.KpMultiply   => IInput.Keys.NUMPAD_MULTIPLY,
            Key.KpSubtract   => IInput.Keys.NUMPAD_SUBTRACT,
            Key.KpAdd        => IInput.Keys.NUMPAD_ADD,
            Key.KpEnter      => IInput.Keys.NUMPAD_ENTER,
            Key.KpEqual      => IInput.Keys.NUMPAD_EQUALS,
            Key.LeftShift    => IInput.Keys.SHIFT_LEFT,
            Key.LeftControl  => IInput.Keys.CONTROL_LEFT,
            Key.LeftAlt      => IInput.Keys.ALT_LEFT,
            Key.LeftSuper    => IInput.Keys.SYM,
            Key.RightShift   => IInput.Keys.SHIFT_RIGHT,
            Key.RightControl => IInput.Keys.CONTROL_RIGHT,
            Key.RightAlt     => IInput.Keys.ALT_RIGHT,
            Key.RightSuper   => IInput.Keys.SYM,
            Key.Menu         => IInput.Keys.MENU,
            var _            => IInput.Keys.UNKNOWN
        };
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose( true );
    }

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    // ------------------------------------------------------------------------
    // Callbacks
    // ------------------------------------------------------------------------

    public void KeyCallback( GLFW.Window window, Key key, int scancode, InputState action, ModifierKey mods )
    {
        int gdxKey;

        switch ( action )
        {
            case InputState.Press:
            {
                gdxKey = GetGdxKeycode( key );

                _eventQueue.KeyDown( gdxKey, TimeUtils.NanoTime() );

                PressedKeyCount++;
                KeyJustPressed            = true;
                PressedKeys[ gdxKey ]     = true;
                JustPressedKeys[ gdxKey ] = true;

                _window?.Graphics.RequestRendering();
                _lastCharacter = ( char ) 0;

                var character = CharacterForKeyCode( gdxKey );

                if ( character != 0 )
                {
                    CharCallback( window, character );
                }

                break;
            }

            case InputState.Release:
            {
                gdxKey = GetGdxKeycode( key );

                PressedKeyCount--;
                PressedKeys[ gdxKey ] = false;

                _window?.Graphics.RequestRendering();

                _eventQueue.KeyUp( gdxKey, TimeUtils.NanoTime() );

                break;
            }

            case InputState.Repeat:
            {
                if ( _lastCharacter != 0 )
                {
                    _window?.Graphics.RequestRendering();

                    _eventQueue.KeyTyped( _lastCharacter, TimeUtils.NanoTime() );
                }

                break;
            }

            default:
            {
                break;
            }
        }
    }

    public void CharCallback( GLFW.Window window, uint codepoint )
    {
        if ( ( codepoint & 0xff00 ) == 0xf700 )
        {
            return;
        }

        _lastCharacter = ( char ) codepoint;
        _window?.Graphics.RequestRendering();
        _eventQueue.KeyTyped( ( char ) codepoint, TimeUtils.NanoTime() );
    }

    public void MouseCallback( GLFW.Window window, MouseButton button, InputState state, ModifierKey mods )
    {
        var gdxButton = button switch
        {
            MouseButton.ButtonLeft   => IInput.Buttons.LEFT,
            MouseButton.ButtonRight  => IInput.Buttons.RIGHT,
            MouseButton.ButtonMiddle => IInput.Buttons.MIDDLE,
            MouseButton.Button4      => IInput.Buttons.BACK,
            MouseButton.Button5      => IInput.Buttons.FORWARD,
            var _                    => -1
        };


        if ( Enum.IsDefined( typeof( MouseButton ), button ) && ( gdxButton == -1 ) )
        {
            return;
        }

        var time = TimeUtils.NanoTime();

        if ( state == InputState.Press )
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
        _eventQueue.Scrolled( -( float ) x, -( float ) y, TimeUtils.NanoTime() );
    }

    public void CursorPosCallback( GLFW.Window window, double x, double y )
    {
        _deltaX = ( int ) x - _logicalMouseX;
        _deltaY = ( int ) y - _logicalMouseY;
        _mouseX = _logicalMouseX = ( int ) x;
        _mouseY = _logicalMouseY = ( int ) y;

        if ( _window?.Config.HdpiMode == HdpiMode.Pixels )
        {
            // null check can be surpressed here because of above
            var xScale = _window.Graphics.BackBufferWidth / ( float ) _window.Graphics.LogicalWidth;
            var yScale = _window.Graphics.BackBufferHeight / ( float ) _window.Graphics.LogicalHeight;

            _deltaX = ( int ) ( _deltaX * xScale );
            _deltaY = ( int ) ( _deltaY * yScale );
            _mouseX = ( int ) ( _mouseX * xScale );
            _mouseY = ( int ) ( _mouseY * yScale );
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

    // ------------------------------------------------------------------------
    // Stubs
    // ------------------------------------------------------------------------

    public override float GetAccelerometerX() => 0;

    public override float GetAccelerometerY() => 0;

    public override float GetAccelerometerZ() => 0;

    public override int GetRotation() => 0;

    public override float GetAzimuth() => 0;

    public override float GetPitch() => 0;

    public override float GetRoll() => 0;

    public override float GetGyroscopeX() => 0;

    public override float GetGyroscopeY() => 0;

    public override float GetGyroscopeZ() => 0;

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

    #endregion From Abstract Input
}