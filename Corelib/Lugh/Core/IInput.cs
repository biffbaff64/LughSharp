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


namespace Corelib.Lugh.Core;

/// <summary>
/// Interface to the input facilities. This allows polling the state of the keyboard, the
/// touch screen and the accelerometer. On some backends (desktop, gwt, etc) the touch
/// screen is replaced by mouse input. The accelerometer is of course not available on all
/// backends. Instead of polling for events, one can process all input events with an
/// InputProcessor. You can set the InputProcessor via the SetInputProcessor(InputProcessor)
/// method. It will be called before the ApplicationListener.render() method in each frame.
/// Keyboard keys are translated to the constants in Input.Keys transparently on all systems.
/// <para>
/// Do not use system specific key constants.
/// </para>
/// <para>
/// The class also offers methods to use (and test for the presence of) other input systems
/// like vibration, compass, on-screen keyboards, and cursor capture.
/// </para>
/// <para>
/// Support for simple input dialogs is also provided.
/// </para>
/// </summary>
[PublicAPI]
public interface IInput
{
    // ====================================================================
    // ====================================================================

    /// <summary>
    /// Keyboard Types
    /// </summary>
    [PublicAPI]
    public enum OnscreenKeyboardType
    {
        Default,
        NumberPad,
        PhonePad,
        Email,
        Password,
        Uri,
    }

    // ====================================================================
    // ====================================================================

    /// <summary>
    /// Screen orientation types.
    /// </summary>
    [PublicAPI]
    public enum Orientation
    {
        Landscape,
        Portrait,
    }

    // ====================================================================
    // ====================================================================

    /// <summary>
    /// Supported peripherals.
    /// </summary>
    [PublicAPI]
    public enum Peripheral
    {
        HardwareKeyboard,
        OnscreenKeyboard,
        MultitouchScreen,
        Accelerometer,
        Compass,
        Vibrator,
        Gyroscope,
        RotationVector,
        Pressure,
    }

    /// <summary>
    /// The currently set <see cref="IInputProcessor"/>.
    /// </summary>
    IInputProcessor? InputProcessor { get; set; }

    // ====================================================================
    // ====================================================================

	#region Mobile Devices

    float GetAccelerometerX();
    float GetAccelerometerY();
    float GetAccelerometerZ();
    float GetGyroscopeX();
    float GetGyroscopeY();
    float GetGyroscopeZ();
    float GetPressure( int pointer = 0 );
    float GetAzimuth();
    float GetPitch();
    float GetRoll();
    void SetOnscreenKeyboardVisible( bool visible );
    void SetOnscreenKeyboardVisible( bool visible, OnscreenKeyboardType type );
    void Vibrate( int milliseconds );
    void Vibrate( long[] pattern, int repeat );
    void CancelVibrate();
    void GetRotationMatrix( float[] matrix );
    bool IsTouched( int pointer = 0 );
    bool JustTouched();

	#endregion Mobile Devices

	// ========================================================================

    int GetMaxPointers();
    int GetX( int pointer = 0 );
    int GetDeltaX( int pointer = 0 );
    int GetY( int pointer = 0 );
    int GetDeltaY( int pointer = 0 );
    int GetRotation();

    bool IsButtonPressed( int button );
    bool IsButtonJustPressed( int button );
    bool IsKeyPressed( int key );
    bool IsKeyJustPressed( int key );
    bool IsPeripheralAvailable( Peripheral peripheral );

    void GetTextInput( ITextInputListener listener,
                       string title,
                       string text,
                       string hint,
                       OnscreenKeyboardType type );

    long GetCurrentEventTime();

    Orientation GetNativeOrientation();

	// ========================================================================

	#region catch keys

    bool IsCursorCaught();
    bool IsCatchKey( int keycode );
    bool IsCatchBackKey();
    bool IsCatchMenuKey();

    void SetCursorPosition( int x, int y );
    void SetCatchKey( int keycode, bool addKey );
    void SetCatchBackKey( bool catchBack );
    void SetCatchMenuKey( bool catchMenu );
    void SetCursorCaught( bool caught );

	#endregion catch keys

    // ====================================================================
    // ====================================================================

    /// <summary>
    /// Mouse Buttons
    /// </summary>
    [PublicAPI]
    public static class Buttons
    {
        public const int LEFT    = 0;
        public const int RIGHT   = 1;
        public const int MIDDLE  = 2;
        public const int BACK    = 3;
        public const int FORWARD = 4;
    }

    // ====================================================================
    // ====================================================================

    /// <summary>
    /// Available Keys
    /// </summary>
    [PublicAPI]
    public static class Keys
    {
        public const int ANY_KEY             = -1;
        public const int NUM_0               = 7;
        public const int NUM_1               = 8;
        public const int NUM_2               = 9;
        public const int NUM_3               = 10;
        public const int NUM_4               = 11;
        public const int NUM_5               = 12;
        public const int NUM_6               = 13;
        public const int NUM_7               = 14;
        public const int NUM_8               = 15;
        public const int NUM_9               = 16;
        public const int A                   = 29;
        public const int ALT_LEFT            = 57;
        public const int ALT_RIGHT           = 58;
        public const int APOSTROPHE          = 75;
        public const int AT                  = 77;
        public const int B                   = 30;
        public const int BACK                = 4;
        public const int BACKSLASH           = 73;
        public const int C                   = 31;
        public const int CALL                = 5;
        public const int CAMERA              = 27;
        public const int CAPS_LOCK           = 115;
        public const int CLEAR               = 28;
        public const int COMMA               = 55;
        public const int D                   = 32;
        public const int DEL                 = 67;
        public const int BACKSPACE           = 67;
        public const int FORWARD_DEL         = 112;
        public const int DPAD_CENTER         = 23;
        public const int DPAD_DOWN           = 20;
        public const int DPAD_LEFT           = 21;
        public const int DPAD_RIGHT          = 22;
        public const int DPAD_UP             = 19;
        public const int CENTER              = 23;
        public const int DOWN                = 20;
        public const int LEFT                = 21;
        public const int RIGHT               = 22;
        public const int UP                  = 19;
        public const int E                   = 33;
        public const int ENDCALL             = 6;
        public const int ENTER               = 66;
        public const int ENVELOPE            = 65;
        public const int EQUALS_SIGN         = 70;
        public const int EXPLORER            = 64;
        public const int F                   = 34;
        public const int FOCUS               = 80;
        public const int G                   = 35;
        public const int GRAVE               = 68;
        public const int H                   = 36;
        public const int HEADSETHOOK         = 79;
        public const int HOME                = 3;
        public const int I                   = 37;
        public const int J                   = 38;
        public const int K                   = 39;
        public const int L                   = 40;
        public const int LEFT_BRACKET        = 71;
        public const int M                   = 41;
        public const int MEDIA_FAST_FORWARD  = 90;
        public const int MEDIA_NEXT          = 87;
        public const int MEDIA_PLAY_PAUSE    = 85;
        public const int MEDIA_PREVIOUS      = 88;
        public const int MEDIA_REWIND        = 89;
        public const int MEDIA_STOP          = 86;
        public const int MENU                = 82;
        public const int MINUS               = 69;
        public const int MUTE                = 91;
        public const int N                   = 42;
        public const int NOTIFICATION        = 83;
        public const int NUM                 = 78;
        public const int O                   = 43;
        public const int P                   = 44;
        public const int PAUSE               = 121; // aka break
        public const int PERIOD              = 56;
        public const int PLUS                = 81;
        public const int POUND               = 18;
        public const int POWER               = 26;
        public const int PRINT_SCREEN        = 120; // aka SYSRQ
        public const int Q                   = 45;
        public const int R                   = 46;
        public const int RIGHT_BRACKET       = 72;
        public const int S                   = 47;
        public const int SCROLL_LOCK         = 116;
        public const int SEARCH              = 84;
        public const int SEMICOLON           = 74;
        public const int SHIFT_LEFT          = 59;
        public const int SHIFT_RIGHT         = 60;
        public const int SLASH               = 76;
        public const int SOFT_LEFT           = 1;
        public const int SOFT_RIGHT          = 2;
        public const int SPACE               = 62;
        public const int STAR                = 17;
        public const int SYM                 = 63;
        public const int T                   = 48;
        public const int TAB                 = 61;
        public const int U                   = 49;
        public const int UNKNOWN             = 0;
        public const int V                   = 50;
        public const int VOLUME_DOWN         = 25;
        public const int VOLUME_UP           = 24;
        public const int W                   = 51;
        public const int X                   = 52;
        public const int Y                   = 53;
        public const int Z                   = 54;
        public const int META_ALT_LEFT_ON    = 16;
        public const int META_ALT_ON         = 2;
        public const int META_ALT_RIGHT_ON   = 32;
        public const int META_SHIFT_LEFT_ON  = 64;
        public const int META_SHIFT_ON       = 1;
        public const int META_SHIFT_RIGHT_ON = 128;
        public const int META_SYM_ON         = 4;
        public const int CONTROL_LEFT        = 129;
        public const int CONTROL_RIGHT       = 130;
        public const int ESCAPE              = 111;
        public const int END                 = 123;
        public const int INSERT              = 124;
        public const int PAGE_UP             = 92;
        public const int PAGE_DOWN           = 93;
        public const int PICTSYMBOLS         = 94;
        public const int SWITCH_CHARSET      = 95;
        public const int BUTTON_CIRCLE       = 255;
        public const int BUTTON_A            = 96;
        public const int BUTTON_B            = 97;
        public const int BUTTON_C            = 98;
        public const int BUTTON_X            = 99;
        public const int BUTTON_Y            = 100;
        public const int BUTTON_Z            = 101;
        public const int BUTTON_L1           = 102;
        public const int BUTTON_R1           = 103;
        public const int BUTTON_L2           = 104;
        public const int BUTTON_R2           = 105;
        public const int BUTTON_THUMBL       = 106;
        public const int BUTTON_THUMBR       = 107;
        public const int BUTTON_START        = 108;
        public const int BUTTON_SELECT       = 109;
        public const int BUTTON_MODE         = 110;

        public const int NUMPAD_0 = 144;
        public const int NUMPAD_1 = 145;
        public const int NUMPAD_2 = 146;
        public const int NUMPAD_3 = 147;
        public const int NUMPAD_4 = 148;
        public const int NUMPAD_5 = 149;
        public const int NUMPAD_6 = 150;
        public const int NUMPAD_7 = 151;
        public const int NUMPAD_8 = 152;
        public const int NUMPAD_9 = 153;

        public const int NUMPAD_DIVIDE      = 154;
        public const int NUMPAD_MULTIPLY    = 155;
        public const int NUMPAD_SUBTRACT    = 156;
        public const int NUMPAD_ADD         = 157;
        public const int NUMPAD_DOT         = 158;
        public const int NUMPAD_COMMA       = 159;
        public const int NUMPAD_ENTER       = 160;
        public const int NUMPAD_EQUALS      = 161;
        public const int NUMPAD_LEFT_PAREN  = 162;
        public const int NUMPAD_RIGHT_PAREN = 163;
        public const int NUM_LOCK           = 143;

        public const int COLON = 243;
        public const int F1    = 131;
        public const int F2    = 132;
        public const int F3    = 133;
        public const int F4    = 134;
        public const int F5    = 135;
        public const int F6    = 136;
        public const int F7    = 137;
        public const int F8    = 138;
        public const int F9    = 139;
        public const int F10   = 140;
        public const int F11   = 141;
        public const int F12   = 142;
        public const int F13   = 183;
        public const int F14   = 184;
        public const int F15   = 185;
        public const int F16   = 186;
        public const int F17   = 187;
        public const int F18   = 188;
        public const int F19   = 189;
        public const int F20   = 190;
        public const int F21   = 191;
        public const int F22   = 192;
        public const int F23   = 193;
        public const int F24   = 194;

        public const int MAX_KEYCODE = 255;

        private static readonly List< string > _keyNames = new();

        // ====================================================================

        static Keys()
        {
            for ( var i = 0; i <= MAX_KEYCODE; i++ )
            {
                var name = ToString( i );

                if ( ( name != null ) && ( _keyNames != null ) )
                {
                    _keyNames[ i ] = name;
                }
            }
        }

        public static string? ToString( int keycode )
        {
            if ( keycode < 0 )
            {
                throw new ArgumentException( "keycode cannot be < 0 - " + keycode );
            }

            if ( keycode > MAX_KEYCODE )
            {
                throw new ArgumentException( "keycode cannot be > MaxKeycode - " + keycode );
            }

            var str = keycode switch
            {
                UNKNOWN            => "Unknown",
                SOFT_LEFT          => "Soft Left",
                SOFT_RIGHT         => "Soft Right",
                HOME               => "Home",
                BACK               => "Back",
                CALL               => "Call",
                ENDCALL            => "End Call",
                NUM_0              => "0",
                NUM_1              => "1",
                NUM_2              => "2",
                NUM_3              => "3",
                NUM_4              => "4",
                NUM_5              => "5",
                NUM_6              => "6",
                NUM_7              => "7",
                NUM_8              => "8",
                NUM_9              => "9",
                STAR               => "*",
                POUND              => "#",
                UP                 => "Up",
                DOWN               => "Down",
                LEFT               => "Left",
                RIGHT              => "Right",
                CENTER             => "Center",
                VOLUME_UP          => "Volume Up",
                VOLUME_DOWN        => "Volume Down",
                POWER              => "Power",
                CAMERA             => "Camera",
                CLEAR              => "Clear",
                A                  => "A",
                B                  => "B",
                C                  => "C",
                D                  => "D",
                E                  => "E",
                F                  => "F",
                G                  => "G",
                H                  => "H",
                I                  => "I",
                J                  => "J",
                K                  => "K",
                L                  => "L",
                M                  => "M",
                N                  => "N",
                O                  => "O",
                P                  => "P",
                Q                  => "Q",
                R                  => "R",
                S                  => "S",
                T                  => "T",
                U                  => "U",
                V                  => "V",
                W                  => "W",
                X                  => "X",
                Y                  => "Y",
                Z                  => "Z",
                COMMA              => ",",
                PERIOD             => ".",
                ALT_LEFT           => "L-Alt",
                ALT_RIGHT          => "R-Alt",
                SHIFT_LEFT         => "L-Shift",
                SHIFT_RIGHT        => "R-Shift",
                TAB                => "Tab",
                SPACE              => "Space",
                SYM                => "SYM",
                EXPLORER           => "Explorer",
                ENVELOPE           => "Envelope",
                ENTER              => "Enter",
                DEL                => "Delete", // also BACKSPACE
                GRAVE              => "`",
                MINUS              => "-",
                EQUALS_SIGN        => "=",
                LEFT_BRACKET       => "[",
                RIGHT_BRACKET      => "]",
                BACKSLASH          => "\\",
                SEMICOLON          => ";",
                APOSTROPHE         => "'",
                SLASH              => "/",
                AT                 => "@",
                NUM                => "Num",
                HEADSETHOOK        => "Headset Hook",
                FOCUS              => "Focus",
                PLUS               => "Plus",
                MENU               => "Menu",
                NOTIFICATION       => "Notification",
                SEARCH             => "Search",
                MEDIA_PLAY_PAUSE   => "Play/Pause",
                MEDIA_STOP         => "Stop Media",
                MEDIA_NEXT         => "Next Media",
                MEDIA_PREVIOUS     => "Prev Media",
                MEDIA_REWIND       => "Rewind",
                MEDIA_FAST_FORWARD => "Fast Forward",
                MUTE               => "Mute",
                PAGE_UP            => "Page Up",
                PAGE_DOWN          => "Page Down",
                PICTSYMBOLS        => "PICTSYMBOLS",
                SWITCH_CHARSET     => "SWITCH_CHARSET",
                BUTTON_A           => "A Button",
                BUTTON_B           => "B Button",
                BUTTON_C           => "C Button",
                BUTTON_X           => "X Button",
                BUTTON_Y           => "Y Button",
                BUTTON_Z           => "Z Button",
                BUTTON_L1          => "L1 Button",
                BUTTON_R1          => "R1 Button",
                BUTTON_L2          => "L2 Button",
                BUTTON_R2          => "R2 Button",
                BUTTON_THUMBL      => "Left Thumb",
                BUTTON_THUMBR      => "Right Thumb",
                BUTTON_START       => "Start",
                BUTTON_SELECT      => "Select",
                BUTTON_MODE        => "Button Mode",
                FORWARD_DEL        => "Forward Delete",
                CONTROL_LEFT       => "L-Ctrl",
                CONTROL_RIGHT      => "R-Ctrl",
                ESCAPE             => "Escape",
                END                => "End",
                INSERT             => "Insert",
                NUMPAD_0           => "Numpad 0",
                NUMPAD_1           => "Numpad 1",
                NUMPAD_2           => "Numpad 2",
                NUMPAD_3           => "Numpad 3",
                NUMPAD_4           => "Numpad 4",
                NUMPAD_5           => "Numpad 5",
                NUMPAD_6           => "Numpad 6",
                NUMPAD_7           => "Numpad 7",
                NUMPAD_8           => "Numpad 8",
                NUMPAD_9           => "Numpad 9",
                COLON              => ":",
                F1                 => "F1",
                F2                 => "F2",
                F3                 => "F3",
                F4                 => "F4",
                F5                 => "F5",
                F6                 => "F6",
                F7                 => "F7",
                F8                 => "F8",
                F9                 => "F9",
                F10                => "F10",
                F11                => "F11",
                F12                => "F12",
                F13                => "F13",
                F14                => "F14",
                F15                => "F15",
                F16                => "F16",
                F17                => "F17",
                F18                => "F18",
                F19                => "F19",
                F20                => "F20",
                F21                => "F21",
                F22                => "F22",
                F23                => "F23",
                F24                => "F24",
                NUMPAD_DIVIDE      => "Num /",
                NUMPAD_MULTIPLY    => "Num *",
                NUMPAD_SUBTRACT    => "Num -",
                NUMPAD_ADD         => "Num +",
                NUMPAD_DOT         => "Num .",
                NUMPAD_COMMA       => "Num ,",
                NUMPAD_ENTER       => "Num Enter",
                NUMPAD_EQUALS      => "Num =",
                NUMPAD_LEFT_PAREN  => "Num (",
                NUMPAD_RIGHT_PAREN => "Num )",
                NUM_LOCK           => "Num Lock",
                CAPS_LOCK          => "Caps Lock",
                SCROLL_LOCK        => "Scroll Lock",
                PAUSE              => "Pause",
                PRINT_SCREEN       => "Print",
                var _              => null,
            };

            return str;
        }

        public static int ValueOf( string keyname )
        {
            return _keyNames.IndexOf( keyname );
        }
    }

    // ====================================================================
    // ====================================================================

    /// <summary>
    /// Interface describing a listener for text input.
    /// </summary>
    [PublicAPI]
    public interface ITextInputListener
    {
        void Input( string text );
        void Canceled();
    }
}
