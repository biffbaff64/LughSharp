// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace LibGDXSharp.LughGL;

[PublicAPI]
public class Glfw
{
    #region GLFW Constants
    
    public const int GLFW_VERSION_MAJOR            = 3;
    public const int GLFW_VERSION_MINOR            = 1;
    public const int GLFW_VERSION_REVISION         = 2;
    public const int GLFW_RELEASE                  = 0;
    public const int GLFW_PRESS                    = 1;
    public const int GLFW_REPEAT                   = 2;
    public const int GLFW_KEY_UNKNOWN              = -1;
    public const int GLFW_KEY_SPACE                = 32;
    public const int GLFW_KEY_APOSTROPHE           = 39;
    public const int GLFW_KEY_COMMA                = 44;
    public const int GLFW_KEY_MINUS                = 45;
    public const int GLFW_KEY_PERIOD               = 46;
    public const int GLFW_KEY_SLASH                = 47;
    public const int GLFW_KEY_0                    = 48;
    public const int GLFW_KEY_1                    = 49;
    public const int GLFW_KEY_2                    = 50;
    public const int GLFW_KEY_3                    = 51;
    public const int GLFW_KEY_4                    = 52;
    public const int GLFW_KEY_5                    = 53;
    public const int GLFW_KEY_6                    = 54;
    public const int GLFW_KEY_7                    = 55;
    public const int GLFW_KEY_8                    = 56;
    public const int GLFW_KEY_9                    = 57;
    public const int GLFW_KEY_SEMICOLON            = 59;
    public const int GLFW_KEY_EQUAL                = 61;
    public const int GLFW_KEY_A                    = 65;
    public const int GLFW_KEY_B                    = 66;
    public const int GLFW_KEY_C                    = 67;
    public const int GLFW_KEY_D                    = 68;
    public const int GLFW_KEY_E                    = 69;
    public const int GLFW_KEY_F                    = 70;
    public const int GLFW_KEY_G                    = 71;
    public const int GLFW_KEY_H                    = 72;
    public const int GLFW_KEY_I                    = 73;
    public const int GLFW_KEY_J                    = 74;
    public const int GLFW_KEY_K                    = 75;
    public const int GLFW_KEY_L                    = 76;
    public const int GLFW_KEY_M                    = 77;
    public const int GLFW_KEY_N                    = 78;
    public const int GLFW_KEY_O                    = 79;
    public const int GLFW_KEY_P                    = 80;
    public const int GLFW_KEY_Q                    = 81;
    public const int GLFW_KEY_R                    = 82;
    public const int GLFW_KEY_S                    = 83;
    public const int GLFW_KEY_T                    = 84;
    public const int GLFW_KEY_U                    = 85;
    public const int GLFW_KEY_V                    = 86;
    public const int GLFW_KEY_W                    = 87;
    public const int GLFW_KEY_X                    = 88;
    public const int GLFW_KEY_Y                    = 89;
    public const int GLFW_KEY_Z                    = 90;
    public const int GLFW_KEY_LEFT_BRACKET         = 91;
    public const int GLFW_KEY_BACKSLASH            = 92;
    public const int GLFW_KEY_RIGHT_BRACKET        = 93;
    public const int GLFW_KEY_GRAVE_ACCENT         = 96;
    public const int GLFW_KEY_WORLD_1              = 161;
    public const int GLFW_KEY_WORLD_2              = 162;
    public const int GLFW_KEY_ESCAPE               = 256;
    public const int GLFW_KEY_ENTER                = 257;
    public const int GLFW_KEY_TAB                  = 258;
    public const int GLFW_KEY_BACKSPACE            = 259;
    public const int GLFW_KEY_INSERT               = 260;
    public const int GLFW_KEY_DELETE               = 261;
    public const int GLFW_KEY_RIGHT                = 262;
    public const int GLFW_KEY_LEFT                 = 263;
    public const int GLFW_KEY_DOWN                 = 264;
    public const int GLFW_KEY_UP                   = 265;
    public const int GLFW_KEY_PAGE_UP              = 266;
    public const int GLFW_KEY_PAGE_DOWN            = 267;
    public const int GLFW_KEY_HOME                 = 268;
    public const int GLFW_KEY_END                  = 269;
    public const int GLFW_KEY_CAPS_LOCK            = 280;
    public const int GLFW_KEY_SCROLL_LOCK          = 281;
    public const int GLFW_KEY_NUM_LOCK             = 282;
    public const int GLFW_KEY_PRINT_SCREEN         = 283;
    public const int GLFW_KEY_PAUSE                = 284;
    public const int GLFW_KEY_F1                   = 290;
    public const int GLFW_KEY_F2                   = 291;
    public const int GLFW_KEY_F3                   = 292;
    public const int GLFW_KEY_F4                   = 293;
    public const int GLFW_KEY_F5                   = 294;
    public const int GLFW_KEY_F6                   = 295;
    public const int GLFW_KEY_F7                   = 296;
    public const int GLFW_KEY_F8                   = 297;
    public const int GLFW_KEY_F9                   = 298;
    public const int GLFW_KEY_F10                  = 299;
    public const int GLFW_KEY_F11                  = 300;
    public const int GLFW_KEY_F12                  = 301;
    public const int GLFW_KEY_F13                  = 302;
    public const int GLFW_KEY_F14                  = 303;
    public const int GLFW_KEY_F15                  = 304;
    public const int GLFW_KEY_F16                  = 305;
    public const int GLFW_KEY_F17                  = 306;
    public const int GLFW_KEY_F18                  = 307;
    public const int GLFW_KEY_F19                  = 308;
    public const int GLFW_KEY_F20                  = 309;
    public const int GLFW_KEY_F21                  = 310;
    public const int GLFW_KEY_F22                  = 311;
    public const int GLFW_KEY_F23                  = 312;
    public const int GLFW_KEY_F24                  = 313;
    public const int GLFW_KEY_F25                  = 314;
    public const int GLFW_KEY_KP_0                 = 320;
    public const int GLFW_KEY_KP_1                 = 321;
    public const int GLFW_KEY_KP_2                 = 322;
    public const int GLFW_KEY_KP_3                 = 323;
    public const int GLFW_KEY_KP_4                 = 324;
    public const int GLFW_KEY_KP_5                 = 325;
    public const int GLFW_KEY_KP_6                 = 326;
    public const int GLFW_KEY_KP_7                 = 327;
    public const int GLFW_KEY_KP_8                 = 328;
    public const int GLFW_KEY_KP_9                 = 329;
    public const int GLFW_KEY_KP_DECIMAL           = 330;
    public const int GLFW_KEY_KP_DIVIDE            = 331;
    public const int GLFW_KEY_KP_MULTIPLY          = 332;
    public const int GLFW_KEY_KP_SUBTRACT          = 333;
    public const int GLFW_KEY_KP_ADD               = 334;
    public const int GLFW_KEY_KP_ENTER             = 335;
    public const int GLFW_KEY_KP_EQUAL             = 336;
    public const int GLFW_KEY_LEFT_SHIFT           = 340;
    public const int GLFW_KEY_LEFT_CONTROL         = 341;
    public const int GLFW_KEY_LEFT_ALT             = 342;
    public const int GLFW_KEY_LEFT_SUPER           = 343;
    public const int GLFW_KEY_RIGHT_SHIFT          = 344;
    public const int GLFW_KEY_RIGHT_CONTROL        = 345;
    public const int GLFW_KEY_RIGHT_ALT            = 346;
    public const int GLFW_KEY_RIGHT_SUPER          = 347;
    public const int GLFW_KEY_MENU                 = 348;
    public const int GLFW_KEY_LAST                 = GLFW_KEY_MENU;
    public const int GLFW_MOD_SHIFT                = 0x0001;
    public const int GLFW_MOD_CONTROL              = 0x0002;
    public const int GLFW_MOD_ALT                  = 0x0004;
    public const int GLFW_MOD_SUPER                = 0x0008;
    public const int GLFW_MOUSE_BUTTON_1           = 0;
    public const int GLFW_MOUSE_BUTTON_2           = 1;
    public const int GLFW_MOUSE_BUTTON_3           = 2;
    public const int GLFW_MOUSE_BUTTON_4           = 3;
    public const int GLFW_MOUSE_BUTTON_5           = 4;
    public const int GLFW_MOUSE_BUTTON_6           = 5;
    public const int GLFW_MOUSE_BUTTON_7           = 6;
    public const int GLFW_MOUSE_BUTTON_8           = 7;
    public const int GLFW_MOUSE_BUTTON_LAST        = GLFW_MOUSE_BUTTON_8;
    public const int GLFW_MOUSE_BUTTON_LEFT        = GLFW_MOUSE_BUTTON_1;
    public const int GLFW_MOUSE_BUTTON_RIGHT       = GLFW_MOUSE_BUTTON_2;
    public const int GLFW_MOUSE_BUTTON_MIDDLE      = GLFW_MOUSE_BUTTON_3;
    public const int GLFW_JOYSTICK_1               = 0;
    public const int GLFW_JOYSTICK_2               = 1;
    public const int GLFW_JOYSTICK_3               = 2;
    public const int GLFW_JOYSTICK_4               = 3;
    public const int GLFW_JOYSTICK_5               = 4;
    public const int GLFW_JOYSTICK_6               = 5;
    public const int GLFW_JOYSTICK_7               = 6;
    public const int GLFW_JOYSTICK_8               = 7;
    public const int GLFW_JOYSTICK_9               = 8;
    public const int GLFW_JOYSTICK_10              = 9;
    public const int GLFW_JOYSTICK_11              = 10;
    public const int GLFW_JOYSTICK_12              = 11;
    public const int GLFW_JOYSTICK_13              = 12;
    public const int GLFW_JOYSTICK_14              = 13;
    public const int GLFW_JOYSTICK_15              = 14;
    public const int GLFW_JOYSTICK_16              = 15;
    public const int GLFW_JOYSTICK_LAST            = GLFW_JOYSTICK_16;
    public const int GLFW_TRUE                     = 1;
    public const int GLFW_FALSE                    = 0;
    public const int GLFW_MAXIMIZED                = 0x00020008;
    public const int GLFW_NOT_INITIALIZED          = 0x00010001;
    public const int GLFW_NO_CURRENT_CONTEXT       = 0x00010002;
    public const int GLFW_INVALID_ENUM             = 0x00010003;
    public const int GLFW_INVALID_VALUE            = 0x00010004;
    public const int GLFW_OUT_OF_MEMORY            = 0x00010005;
    public const int GLFW_API_UNAVAILABLE          = 0x00010006;
    public const int GLFW_VERSION_UNAVAILABLE      = 0x00010007;
    public const int GLFW_PLATFORM_ERROR           = 0x00010008;
    public const int GLFW_FORMAT_UNAVAILABLE       = 0x00010009;
    public const int GLFW_FOCUSED                  = 0x00020001;
    public const int GLFW_ICONIFIED                = 0x00020002;
    public const int GLFW_RESIZABLE                = 0x00020003;
    public const int GLFW_VISIBLE                  = 0x00020004;
    public const int GLFW_DECORATED                = 0x00020005;
    public const int GLFW_AUTO_ICONIFY             = 0x00020006;
    public const int GLFW_FLOATING                 = 0x00020007;
    public const int GLFW_RED_BITS                 = 0x00021001;
    public const int GLFW_GREEN_BITS               = 0x00021002;
    public const int GLFW_BLUE_BITS                = 0x00021003;
    public const int GLFW_ALPHA_BITS               = 0x00021004;
    public const int GLFW_DEPTH_BITS               = 0x00021005;
    public const int GLFW_STENCIL_BITS             = 0x00021006;
    public const int GLFW_ACCUM_RED_BITS           = 0x00021007;
    public const int GLFW_ACCUM_GREEN_BITS         = 0x00021008;
    public const int GLFW_ACCUM_BLUE_BITS          = 0x00021009;
    public const int GLFW_ACCUM_ALPHA_BITS         = 0x0002100A;
    public const int GLFW_AUX_BUFFERS              = 0x0002100B;
    public const int GLFW_STEREO                   = 0x0002100C;
    public const int GLFW_SAMPLES                  = 0x0002100D;
    public const int GLFW_SRGB_CAPABLE             = 0x0002100E;
    public const int GLFW_REFRESH_RATE             = 0x0002100F;
    public const int GLFW_DOUBLEBUFFER             = 0x00021010;
    public const int GLFW_CLIENT_API               = 0x00022001;
    public const int GLFW_CONTEXT_VERSION_MAJOR    = 0x00022002;
    public const int GLFW_CONTEXT_VERSION_MINOR    = 0x00022003;
    public const int GLFW_CONTEXT_REVISION         = 0x00022004;
    public const int GLFW_CONTEXT_ROBUSTNESS       = 0x00022005;
    public const int GLFW_OPENGL_FORWARD_COMPAT    = 0x00022006;
    public const int GLFW_OPENGL_DEBUG_CONTEXT     = 0x00022007;
    public const int GLFW_OPENGL_PROFILE           = 0x00022008;
    public const int GLFW_CONTEXT_RELEASE_BEHAVIOR = 0x00022009;
    public const int GLFW_OPENGL_API               = 0x00030001;
    public const int GLFW_OPENGL_ES_API            = 0x00030002;
    public const int GLFW_NO_ROBUSTNESS            = 0;
    public const int GLFW_NO_RESET_NOTIFICATION    = 0x00031001;
    public const int GLFW_LOSE_CONTEXT_ON_RESET    = 0x00031002;
    public const int GLFW_OPENGL_ANY_PROFILE       = 0;
    public const int GLFW_OPENGL_CORE_PROFILE      = 0x00032001;
    public const int GLFW_OPENGL_COMPAT_PROFILE    = 0x00032002;
    public const int GLFW_CURSOR                   = 0x00033001;
    public const int GLFW_STICKY_KEYS              = 0x00033002;
    public const int GLFW_STICKY_MOUSE_BUTTONS     = 0x00033003;
    public const int GLFW_CURSOR_NORMAL            = 0x00034001;
    public const int GLFW_CURSOR_HIDDEN            = 0x00034002;
    public const int GLFW_CURSOR_DISABLED          = 0x00034003;
    public const int GLFW_ANY_RELEASE_BEHAVIOR     = 0;
    public const int GLFW_RELEASE_BEHAVIOR_FLUSH   = 0x00035001;
    public const int GLFW_RELEASE_BEHAVIOR_NONE    = 0x00035002;
    public const int GLFW_ARROW_CURSOR             = 0x00036001;
    public const int GLFW_IBEAM_CURSOR             = 0x00036002;
    public const int GLFW_CROSSHAIR_CURSOR         = 0x00036003;
    public const int GLFW_HAND_CURSOR              = 0x00036004;
    public const int GLFW_HRESIZE_CURSOR           = 0x00036005;
    public const int GLFW_VRESIZE_CURSOR           = 0x00036006;
    public const int GLFW_CONNECTED                = 0x00040001;
    public const int GLFW_DISCONNECTED             = 0x00040002;

    #endregion GLFW Constants

    #region Callback Delegates
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GLFWerrorCallback( int errorcode, string description );

    #endregion Callback Delegates
    
    public struct GLFWImage
    {
        public int    width;
        public int    height;
        public IntPtr pixels;
    }

    #region GLFW Commands
    
    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern IntPtr glfwGetWindowUserPointer( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwGetVersion( ref int major, ref int minor, ref int rev );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwGetMonitorPos( IntPtr monitor, ref int xpos, ref int ypos );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwGetMonitorPhysicalSize( IntPtr monitor, ref int width, ref int height );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwGetWindowPos( IntPtr window, ref int xpos, ref int ypos );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwGetWindowSize( IntPtr window, ref int width, ref int height );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwGetFramebufferSize( IntPtr window, ref int width, ref int height );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwGetWindowFrameSize( IntPtr window, ref int left, ref int top, ref int right, ref int bottom );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwGetCursorPos( IntPtr window, ref double xpos, ref double ypos );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetGamma( IntPtr monitor, float gamma );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetGammaRamp( IntPtr monitor, long ramp );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetWindowShouldClose( IntPtr window, int value );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetWindowTitle( IntPtr window, string title );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetWindowPos( IntPtr window, int xpos, int ypos );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetWindowSize( IntPtr window, int width, int height );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetWindowUserPointer( IntPtr window, IntPtr pointer );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetCursorPos( IntPtr window, double xpos, double ypos );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetCursor( IntPtr window, long cursor );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetClipboardString( IntPtr window, string @string );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetTime( double time );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetInputMode( IntPtr window, int mode, int value );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwTerminate();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwDefaultWindowHints();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwWindowHint( int target, int hint );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwDestroyWindow( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwIconifyWindow( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwRestoreWindow( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwShowWindow( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwHideWindow( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwPollEvents();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwWaitEvents();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwPostEmptyEvent();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwDestroyCursor( long cursor );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwMakeContextCurrent( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSwapBuffers( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSwapInterval( int interval );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern int glfwInit();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern int glfwWindowShouldClose( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern int glfwGetWindowAttrib( IntPtr window, int attrib );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern int glfwGetInputMode( IntPtr window, int mode );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern int glfwGetKey( IntPtr window, int key );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern int glfwGetMouseButton( IntPtr window, int button );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern int glfwJoystickPresent( int joy );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern int glfwExtensionSupported( string extension );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern IntPtr glfwGetJoystickButtons( int joy, out int count );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern string glfwGetVersionString();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern string glfwGetMonitorName( IntPtr monitor );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern string glfwGetJoystickName( int joy );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern string glfwGetClipboardString( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern long[] glfwGetVideoModes( IntPtr monitor, ref int count );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern long[] glfwGetMonitors( int count );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern long glfwGetVideoMode( IntPtr monitor );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern long glfwGetGammaRamp( IntPtr monitor );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern IntPtr glfwGetPrimaryMonitor();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern IntPtr glfwCreateWindow( int width, int height, byte[] title, IntPtr monitor, IntPtr share );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern IntPtr glfwGetWindowMonitor( IntPtr window );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern long glfwCreateCursor( long image, int xhot, int yhot );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern long glfwCreateStandardCursor( int shape );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern long glfwGetCurrentContext();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern long glfwGetProcAddress( string procname );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern IntPtr glfwGetJoystickAxes( int joy, out int count );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern IntPtr glfwGetJoystickHats( int joy, out int count );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern double glfwGetTime();

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern void glfwSetWindowIcon( IntPtr window, int count, GLFWImage[] images );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWerrorCallback glfwSetErrorCallback( GLFWerrorCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWmonitorCallback glfwSetMonitorCallback( GLFWmonitorCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWwindowposCallback glfwSetWindowPosCallback( IntPtr window, GLFWwindowposCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWwindowsizeCallback glfwSetWindowSizeCallback( IntPtr window, GLFWwindowsizeCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWwindowcloseCallback glfwSetWindowCloseCallback( IntPtr window, GLFWwindowcloseCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWwindowrefreshCallback glfwSetWindowRefreshCallback( IntPtr window, GLFWwindowrefreshCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWwindowfocusCallback glfwSetWindowFocusCallback( IntPtr window, GLFWwindowfocusCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWwindowiconifyCallback glfwSetWindowIconifyCallback( IntPtr window, GLFWwindowiconifyCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWframebuffersizeCallback glfwSetFramebufferSizeCallback( IntPtr window, GLFWframebuffersizeCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWkeyCallback glfwSetKeyCallback( IntPtr window, GLFWkeyCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWcharCallback glfwSetCharCallback( IntPtr window, GLFWcharCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWcharmodsCallback glfwSetCharModsCallback( IntPtr window, GLFWcharmodsCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWmousebuttonCallback glfwSetMouseButtonCallback( IntPtr window, GLFWmousebuttonCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWcursorposCallback glfwSetCursorPosCallback( IntPtr window, GLFWcursorposCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWcursorenterCallback glfwSetCursorEnterCallback( IntPtr window, GLFWcursorenterCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWscrollCallback glfwSetScrollCallback( IntPtr window, GLFWscrollCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWdropCallback glfwSetDropCallback( IntPtr window, GLFWdropCallback cb );

    [DllImport( Glg.LIBGLFW, CallingConvention = CallingConvention.Cdecl )]
    public static extern GLFWjoystickCallback glfwSetJoystickCallback( GLFWjoystickCallback cb );

    #endregion GLFW Commands
}
