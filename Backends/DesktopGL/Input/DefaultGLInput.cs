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

using LibGDXSharp.Backends.Desktop.Window;

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
        return ( pointer == 0 ) ? _mouseX : 0;
    }

    /// <inheritdoc />
    public override int GetDeltaX( int pointer = 0 )
    {
        return ( pointer == 0 ) ? _deltaX : 0;
    }

    /// <inheritdoc />
    public override int GetY( int pointer = 0 )
    {
        return ( pointer == 0 ) ? _mouseY : 0;
    }

    /// <inheritdoc />
    public override int GetDeltaY( int pointer = 0 )
    {
        return ( pointer == 0 ) ? _deltaY : 0;
    }

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
    public override bool IsButtonPressed( int button ) => Glfw.GetMouseButton( _window!.GlfwWindow,
                                                                               TranslateToMouseButton( button ) )
                                                       == InputState.Press;

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
    public override void SetCursorCaught( bool caught )
    {
        Glfw.SetInputMode( _window!.GlfwWindow,
                           InputMode.Cursor,
                           ( int )( caught ? CursorMode.Disabled : CursorMode.Normal ) );
    }

    /// <inheritdoc />
    public override bool IsCursorCaught() => Glfw.GetInputMode( _window!.GlfwWindow, InputMode.Cursor )
                                          == ( int )CursorMode.Disabled;

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

        Glfw.SetCursorPosition( _window!.GlfwWindow, x, y );
    }

    /// <inheritdoc />
    public void WindowHandleChanged( GLFW.Window windowHandle )
    {
        ResetPollingStates();

        Glfw.SetKeyCallback( windowHandle, KeyCallback );
        Glfw.SetCharCallback( windowHandle, CharCallback );
        Glfw.SetMouseButtonCallback( windowHandle, MouseCallback );
        Glfw.SetScrollCallback( windowHandle, ScrollCallback );
        Glfw.SetCursorPositionCallback( windowHandle, CursorPosCallback );
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

    public override bool IsPeripheralAvailable( IInput.Peripheral peripheral )
        => peripheral == IInput.Peripheral.HardwareKeyboard;

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

    private static MouseButton TranslateToMouseButton( int button )
    {
        return button switch
               {
                   0 => MouseButton.Button1,
                   1 => MouseButton.Button2,
                   2 => MouseButton.Button3,
                   3 => MouseButton.Button4,
                   4 => MouseButton.Button5,
                   5 => MouseButton.Button6,
                   6 => MouseButton.Button7,
                   7 => MouseButton.Button8,
                   _ => throw new GdxRuntimeException( $"Unknown MouseButton: {button}" )
               };
    }

    protected static char CharacterForKeyCode( int key )
    {
        return key switch
               {
                   IInput.Keys.BACKSPACE    => ( char )8,
                   IInput.Keys.TAB          => '\t',
                   IInput.Keys.FORWARD_DEL  => ( char )127,
                   IInput.Keys.NUMPAD_ENTER => '\n',
                   IInput.Keys.ENTER        => '\n',
                   _                        => ( char )0
               };
    }

    public static int GetGdxKeycode( Keys glKeycode )
    {
        return glKeycode switch
               {
                   Keys.Space          => IInput.Keys.SPACE,
                   Keys.Apostrophe     => IInput.Keys.APOSTROPHE,
                   Keys.Comma          => IInput.Keys.COMMA,
                   Keys.Minus          => IInput.Keys.MINUS,
                   Keys.Period         => IInput.Keys.PERIOD,
                   Keys.Slash          => IInput.Keys.SLASH,
                   Keys.Alpha0         => IInput.Keys.NUM_0,
                   Keys.Alpha1         => IInput.Keys.NUM_1,
                   Keys.Alpha2         => IInput.Keys.NUM_2,
                   Keys.Alpha3         => IInput.Keys.NUM_3,
                   Keys.Alpha4         => IInput.Keys.NUM_4,
                   Keys.Alpha5         => IInput.Keys.NUM_5,
                   Keys.Alpha6         => IInput.Keys.NUM_6,
                   Keys.Alpha7         => IInput.Keys.NUM_7,
                   Keys.Alpha8         => IInput.Keys.NUM_8,
                   Keys.Alpha9         => IInput.Keys.NUM_9,
                   Keys.SemiColon      => IInput.Keys.SEMICOLON,
                   Keys.Equal          => IInput.Keys.EQUALS_SIGN,
                   Keys.A              => IInput.Keys.A,
                   Keys.B              => IInput.Keys.B,
                   Keys.C              => IInput.Keys.C,
                   Keys.D              => IInput.Keys.D,
                   Keys.E              => IInput.Keys.E,
                   Keys.F              => IInput.Keys.F,
                   Keys.G              => IInput.Keys.G,
                   Keys.H              => IInput.Keys.H,
                   Keys.I              => IInput.Keys.I,
                   Keys.J              => IInput.Keys.J,
                   Keys.K              => IInput.Keys.K,
                   Keys.L              => IInput.Keys.L,
                   Keys.M              => IInput.Keys.M,
                   Keys.N              => IInput.Keys.N,
                   Keys.O              => IInput.Keys.O,
                   Keys.P              => IInput.Keys.P,
                   Keys.Q              => IInput.Keys.Q,
                   Keys.R              => IInput.Keys.R,
                   Keys.S              => IInput.Keys.S,
                   Keys.T              => IInput.Keys.T,
                   Keys.U              => IInput.Keys.U,
                   Keys.V              => IInput.Keys.V,
                   Keys.W              => IInput.Keys.W,
                   Keys.X              => IInput.Keys.X,
                   Keys.Y              => IInput.Keys.Y,
                   Keys.Z              => IInput.Keys.Z,
                   Keys.LeftBracket    => IInput.Keys.LEFT_BRACKET,
                   Keys.Backslash      => IInput.Keys.BACKSLASH,
                   Keys.RightBracket   => IInput.Keys.RIGHT_BRACKET,
                   Keys.GraveAccent    => IInput.Keys.GRAVE,
                   Keys.Unknown        => IInput.Keys.UNKNOWN,
                   Keys.Escape         => IInput.Keys.ESCAPE,
                   Keys.Enter          => IInput.Keys.ENTER,
                   Keys.Tab            => IInput.Keys.TAB,
                   Keys.Backspace      => IInput.Keys.BACKSPACE,
                   Keys.Insert         => IInput.Keys.INSERT,
                   Keys.Delete         => IInput.Keys.FORWARD_DEL,
                   Keys.Right          => IInput.Keys.RIGHT,
                   Keys.Left           => IInput.Keys.LEFT,
                   Keys.Down           => IInput.Keys.DOWN,
                   Keys.Up             => IInput.Keys.UP,
                   Keys.PageUp         => IInput.Keys.PAGE_UP,
                   Keys.PageDown       => IInput.Keys.PAGE_DOWN,
                   Keys.Home           => IInput.Keys.HOME,
                   Keys.End            => IInput.Keys.END,
                   Keys.CapsLock       => IInput.Keys.CAPS_LOCK,
                   Keys.ScrollLock     => IInput.Keys.SCROLL_LOCK,
                   Keys.PrintScreen    => IInput.Keys.PRINT_SCREEN,
                   Keys.Pause          => IInput.Keys.PAUSE,
                   Keys.F1             => IInput.Keys.F1,
                   Keys.F2             => IInput.Keys.F2,
                   Keys.F3             => IInput.Keys.F3,
                   Keys.F4             => IInput.Keys.F4,
                   Keys.F5             => IInput.Keys.F5,
                   Keys.F6             => IInput.Keys.F6,
                   Keys.F7             => IInput.Keys.F7,
                   Keys.F8             => IInput.Keys.F8,
                   Keys.F9             => IInput.Keys.F9,
                   Keys.F10            => IInput.Keys.F10,
                   Keys.F11            => IInput.Keys.F11,
                   Keys.F12            => IInput.Keys.F12,
                   Keys.F13            => IInput.Keys.F13,
                   Keys.F14            => IInput.Keys.F14,
                   Keys.F15            => IInput.Keys.F15,
                   Keys.F16            => IInput.Keys.F16,
                   Keys.F17            => IInput.Keys.F17,
                   Keys.F18            => IInput.Keys.F18,
                   Keys.F19            => IInput.Keys.F19,
                   Keys.F20            => IInput.Keys.F20,
                   Keys.F21            => IInput.Keys.F21,
                   Keys.F22            => IInput.Keys.F22,
                   Keys.F23            => IInput.Keys.F23,
                   Keys.F24            => IInput.Keys.F24,
                   Keys.F25            => IInput.Keys.UNKNOWN,
                   Keys.NumLock        => IInput.Keys.NUM_LOCK,
                   Keys.Numpad0        => IInput.Keys.NUMPAD_0,
                   Keys.Numpad1        => IInput.Keys.NUMPAD_1,
                   Keys.Numpad2        => IInput.Keys.NUMPAD_2,
                   Keys.Numpad3        => IInput.Keys.NUMPAD_3,
                   Keys.Numpad4        => IInput.Keys.NUMPAD_4,
                   Keys.Numpad5        => IInput.Keys.NUMPAD_5,
                   Keys.Numpad6        => IInput.Keys.NUMPAD_6,
                   Keys.Numpad7        => IInput.Keys.NUMPAD_7,
                   Keys.Numpad8        => IInput.Keys.NUMPAD_8,
                   Keys.Numpad9        => IInput.Keys.NUMPAD_9,
                   Keys.NumpadDecimal  => IInput.Keys.NUMPAD_DOT,
                   Keys.NumpadDivide   => IInput.Keys.NUMPAD_DIVIDE,
                   Keys.NumpadMultiply => IInput.Keys.NUMPAD_MULTIPLY,
                   Keys.NumpadSubtract => IInput.Keys.NUMPAD_SUBTRACT,
                   Keys.NumpadAdd      => IInput.Keys.NUMPAD_ADD,
                   Keys.NumpadEnter    => IInput.Keys.NUMPAD_ENTER,
                   Keys.NumpadEqual    => IInput.Keys.NUMPAD_EQUALS,
                   Keys.LeftShift      => IInput.Keys.SHIFT_LEFT,
                   Keys.LeftControl    => IInput.Keys.CONTROL_LEFT,
                   Keys.LeftAlt        => IInput.Keys.ALT_LEFT,
                   Keys.LeftSuper      => IInput.Keys.SYM,
                   Keys.RightShift     => IInput.Keys.SHIFT_RIGHT,
                   Keys.RightControl   => IInput.Keys.CONTROL_RIGHT,
                   Keys.RightAlt       => IInput.Keys.ALT_RIGHT,
                   Keys.RightSuper     => IInput.Keys.SYM,
                   Keys.Menu           => IInput.Keys.MENU,
                   _                   => IInput.Keys.UNKNOWN
               };
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing,
    ///     or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
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

    internal void KeyCallback( IntPtr window,
                               Keys key,
                               int scancode,
                               InputState action,
                               ModifierKeys mods )
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
                _lastCharacter = ( char )0;

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
        }
    }

    internal void CharCallback( IntPtr window, uint codepoint )
    {
        if ( ( codepoint & 0xff00 ) == 0xf700 )
        {
            return;
        }

        _lastCharacter = ( char )codepoint;
        _window?.Graphics.RequestRendering();
        _eventQueue.KeyTyped( ( char )codepoint, TimeUtils.NanoTime() );
    }

    internal void MouseCallback( IntPtr window, MouseButton button, InputState state, ModifierKeys mods )
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

    internal void ScrollCallback( IntPtr window, double x, double y )
    {
        _window?.Graphics.RequestRendering();
        _eventQueue.Scrolled( -( float )x, -( float )y, TimeUtils.NanoTime() );
    }

    internal void CursorPosCallback( IntPtr window, double x, double y )
    {
        _deltaX = ( int )x - _logicalMouseX;
        _deltaY = ( int )y - _logicalMouseY;
        _mouseX = _logicalMouseX = ( int )x;
        _mouseY = _logicalMouseY = ( int )y;

        if ( _window?.Config.HdpiMode == HdpiMode.Pixels )
        {
            // null check can be surpressed here because of above
            var xScale = _window.Graphics.BackBufferWidth / ( float )_window.Graphics.LogicalWidth;
            var yScale = _window.Graphics.BackBufferHeight / ( float )_window.Graphics.LogicalHeight;

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
