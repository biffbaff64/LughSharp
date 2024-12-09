// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
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

using System.Diagnostics.CodeAnalysis;

#pragma warning disable 1591

namespace LughGlfw.Glfw.Native;

/// <summary>
/// Native bindings to the GLFW library.
/// </summary>
[PublicAPI, SuppressMessage( "ReSharper", "RedundantOverflowCheckingContext" )]
public static unsafe partial class NativeGlfw
{
    /// <summary>
    /// One.
    /// </summary>
    public const int GLFW_TRUE = unchecked( ( int )1 );

    /// <summary>
    /// Zero.
    /// </summary>
    public const int GLFW_FALSE = unchecked( ( int )0 );

    public const int GLFW_HAT_CENTERED      = unchecked( ( int )0 );
    public const int GLFW_HAT_UP            = unchecked( ( int )1 );
    public const int GLFW_HAT_RIGHT         = unchecked( ( int )2 );
    public const int GLFW_HAT_DOWN          = unchecked( ( int )4 );
    public const int GLFW_HAT_LEFT          = unchecked( ( int )8 );
    public const int GLFW_HAT_RIGHT_UP      = unchecked( ( int )( GLFW_HAT_RIGHT | GLFW_HAT_UP ) );
    public const int GLFW_HAT_RIGHT_DOWN    = unchecked( ( int )( GLFW_HAT_RIGHT | GLFW_HAT_DOWN ) );
    public const int GLFW_HAT_LEFT_UP       = unchecked( ( int )( GLFW_HAT_LEFT | GLFW_HAT_UP ) );
    public const int GLFW_HAT_LEFT_DOWN     = unchecked( ( int )( GLFW_HAT_LEFT | GLFW_HAT_DOWN ) );
    public const int GLFW_KEY_UNKNOWN       = unchecked( ( int )-1 );
    public const int GLFW_KEY_SPACE         = unchecked( ( int )32 );
    public const int GLFW_KEY_APOSTROPHE    = unchecked( ( int )39 );
    public const int GLFW_KEY_COMMA         = unchecked( ( int )44 );
    public const int GLFW_KEY_MINUS         = unchecked( ( int )45 );
    public const int GLFW_KEY_PERIOD        = unchecked( ( int )46 );
    public const int GLFW_KEY_SLASH         = unchecked( ( int )47 );
    public const int GLFW_KEY_0             = unchecked( ( int )48 );
    public const int GLFW_KEY_1             = unchecked( ( int )49 );
    public const int GLFW_KEY_2             = unchecked( ( int )50 );
    public const int GLFW_KEY_3             = unchecked( ( int )51 );
    public const int GLFW_KEY_4             = unchecked( ( int )52 );
    public const int GLFW_KEY_5             = unchecked( ( int )53 );
    public const int GLFW_KEY_6             = unchecked( ( int )54 );
    public const int GLFW_KEY_7             = unchecked( ( int )55 );
    public const int GLFW_KEY_8             = unchecked( ( int )56 );
    public const int GLFW_KEY_9             = unchecked( ( int )57 );
    public const int GLFW_KEY_SEMICOLON     = unchecked( ( int )59 );
    public const int GLFW_KEY_EQUAL         = unchecked( ( int )61 );
    public const int GLFW_KEY_A             = unchecked( ( int )65 );
    public const int GLFW_KEY_B             = unchecked( ( int )66 );
    public const int GLFW_KEY_C             = unchecked( ( int )67 );
    public const int GLFW_KEY_D             = unchecked( ( int )68 );
    public const int GLFW_KEY_E             = unchecked( ( int )69 );
    public const int GLFW_KEY_F             = unchecked( ( int )70 );
    public const int GLFW_KEY_G             = unchecked( ( int )71 );
    public const int GLFW_KEY_H             = unchecked( ( int )72 );
    public const int GLFW_KEY_I             = unchecked( ( int )73 );
    public const int GLFW_KEY_J             = unchecked( ( int )74 );
    public const int GLFW_KEY_K             = unchecked( ( int )75 );
    public const int GLFW_KEY_L             = unchecked( ( int )76 );
    public const int GLFW_KEY_M             = unchecked( ( int )77 );
    public const int GLFW_KEY_N             = unchecked( ( int )78 );
    public const int GLFW_KEY_O             = unchecked( ( int )79 );
    public const int GLFW_KEY_P             = unchecked( ( int )80 );
    public const int GLFW_KEY_Q             = unchecked( ( int )81 );
    public const int GLFW_KEY_R             = unchecked( ( int )82 );
    public const int GLFW_KEY_S             = unchecked( ( int )83 );
    public const int GLFW_KEY_T             = unchecked( ( int )84 );
    public const int GLFW_KEY_U             = unchecked( ( int )85 );
    public const int GLFW_KEY_V             = unchecked( ( int )86 );
    public const int GLFW_KEY_W             = unchecked( ( int )87 );
    public const int GLFW_KEY_X             = unchecked( ( int )88 );
    public const int GLFW_KEY_Y             = unchecked( ( int )89 );
    public const int GLFW_KEY_Z             = unchecked( ( int )90 );
    public const int GLFW_KEY_LEFT_BRACKET  = unchecked( ( int )91 );
    public const int GLFW_KEY_BACKSLASH     = unchecked( ( int )92 );
    public const int GLFW_KEY_RIGHT_BRACKET = unchecked( ( int )93 );
    public const int GLFW_KEY_GRAVE_ACCENT  = unchecked( ( int )96 );
    public const int GLFW_KEY_WORLD_1       = unchecked( ( int )161 );
    public const int GLFW_KEY_WORLD_2       = unchecked( ( int )162 );
    public const int GLFW_KEY_ESCAPE        = unchecked( ( int )256 );
    public const int GLFW_KEY_ENTER         = unchecked( ( int )257 );
    public const int GLFW_KEY_TAB           = unchecked( ( int )258 );
    public const int GLFW_KEY_BACKSPACE     = unchecked( ( int )259 );
    public const int GLFW_KEY_INSERT        = unchecked( ( int )260 );
    public const int GLFW_KEY_DELETE        = unchecked( ( int )261 );
    public const int GLFW_KEY_RIGHT         = unchecked( ( int )262 );
    public const int GLFW_KEY_LEFT          = unchecked( ( int )263 );
    public const int GLFW_KEY_DOWN          = unchecked( ( int )264 );
    public const int GLFW_KEY_UP            = unchecked( ( int )265 );
    public const int GLFW_KEY_PAGE_UP       = unchecked( ( int )266 );
    public const int GLFW_KEY_PAGE_DOWN     = unchecked( ( int )267 );
    public const int GLFW_KEY_HOME          = unchecked( ( int )268 );
    public const int GLFW_KEY_END           = unchecked( ( int )269 );
    public const int GLFW_KEY_CAPS_LOCK     = unchecked( ( int )280 );
    public const int GLFW_KEY_SCROLL_LOCK   = unchecked( ( int )281 );
    public const int GLFW_KEY_NUM_LOCK      = unchecked( ( int )282 );
    public const int GLFW_KEY_PRINT_SCREEN  = unchecked( ( int )283 );
    public const int GLFW_KEY_PAUSE         = unchecked( ( int )284 );
    public const int GLFW_KEY_F1            = unchecked( ( int )290 );
    public const int GLFW_KEY_F2            = unchecked( ( int )291 );
    public const int GLFW_KEY_F3            = unchecked( ( int )292 );
    public const int GLFW_KEY_F4            = unchecked( ( int )293 );
    public const int GLFW_KEY_F5            = unchecked( ( int )294 );
    public const int GLFW_KEY_F6            = unchecked( ( int )295 );
    public const int GLFW_KEY_F7            = unchecked( ( int )296 );
    public const int GLFW_KEY_F8            = unchecked( ( int )297 );
    public const int GLFW_KEY_F9            = unchecked( ( int )298 );
    public const int GLFW_KEY_F10           = unchecked( ( int )299 );
    public const int GLFW_KEY_F11           = unchecked( ( int )300 );
    public const int GLFW_KEY_F12           = unchecked( ( int )301 );
    public const int GLFW_KEY_F13           = unchecked( ( int )302 );
    public const int GLFW_KEY_F14           = unchecked( ( int )303 );
    public const int GLFW_KEY_F15           = unchecked( ( int )304 );
    public const int GLFW_KEY_F16           = unchecked( ( int )305 );
    public const int GLFW_KEY_F17           = unchecked( ( int )306 );
    public const int GLFW_KEY_F18           = unchecked( ( int )307 );
    public const int GLFW_KEY_F19           = unchecked( ( int )308 );
    public const int GLFW_KEY_F20           = unchecked( ( int )309 );
    public const int GLFW_KEY_F21           = unchecked( ( int )310 );
    public const int GLFW_KEY_F22           = unchecked( ( int )311 );
    public const int GLFW_KEY_F23           = unchecked( ( int )312 );
    public const int GLFW_KEY_F24           = unchecked( ( int )313 );
    public const int GLFW_KEY_F25           = unchecked( ( int )314 );
    public const int GLFW_KEY_KP_0          = unchecked( ( int )320 );
    public const int GLFW_KEY_KP_1          = unchecked( ( int )321 );
    public const int GLFW_KEY_KP_2          = unchecked( ( int )322 );
    public const int GLFW_KEY_KP_3          = unchecked( ( int )323 );
    public const int GLFW_KEY_KP_4          = unchecked( ( int )324 );
    public const int GLFW_KEY_KP_5          = unchecked( ( int )325 );
    public const int GLFW_KEY_KP_6          = unchecked( ( int )326 );
    public const int GLFW_KEY_KP_7          = unchecked( ( int )327 );
    public const int GLFW_KEY_KP_8          = unchecked( ( int )328 );
    public const int GLFW_KEY_KP_9          = unchecked( ( int )329 );
    public const int GLFW_KEY_KP_DECIMAL    = unchecked( ( int )330 );
    public const int GLFW_KEY_KP_DIVIDE     = unchecked( ( int )331 );
    public const int GLFW_KEY_KP_MULTIPLY   = unchecked( ( int )332 );
    public const int GLFW_KEY_KP_SUBTRACT   = unchecked( ( int )333 );
    public const int GLFW_KEY_KP_ADD        = unchecked( ( int )334 );
    public const int GLFW_KEY_KP_ENTER      = unchecked( ( int )335 );
    public const int GLFW_KEY_KP_EQUAL      = unchecked( ( int )336 );
    public const int GLFW_KEY_LEFT_SHIFT    = unchecked( ( int )340 );
    public const int GLFW_KEY_LEFT_CONTROL  = unchecked( ( int )341 );
    public const int GLFW_KEY_LEFT_ALT      = unchecked( ( int )342 );
    public const int GLFW_KEY_LEFT_SUPER    = unchecked( ( int )343 );
    public const int GLFW_KEY_RIGHT_SHIFT   = unchecked( ( int )344 );
    public const int GLFW_KEY_RIGHT_CONTROL = unchecked( ( int )345 );
    public const int GLFW_KEY_RIGHT_ALT     = unchecked( ( int )346 );
    public const int GLFW_KEY_RIGHT_SUPER   = unchecked( ( int )347 );
    public const int GLFW_KEY_MENU          = unchecked( ( int )348 );
    public const int GLFW_KEY_LAST          = unchecked( ( int )GLFW_KEY_MENU );

    /// <summary>
    /// If this bit is set one or more Shift keys were held down.
    /// </summary>
    public const int GLFW_MOD_SHIFT = unchecked( ( int )0x0001 );

    /// <summary>
    /// If this bit is set one or more Control keys were held down.
    /// </summary>
    public const int GLFW_MOD_CONTROL = unchecked( ( int )0x0002 );

    /// <summary>
    /// If this bit is set one or more Alt keys were held down.
    /// </summary>
    public const int GLFW_MOD_ALT = unchecked( ( int )0x0004 );

    /// <summary>
    /// If this bit is set one or more Super keys were held down.
    /// </summary>
    public const int GLFW_MOD_SUPER = unchecked( ( int )0x0008 );

    /// <summary>
    /// If this bit is set the Caps Lock key is enabled.
    /// </summary>
    public const int GLFW_MOD_CAPS_LOCK = unchecked( ( int )0x0010 );

    /// <summary>
    /// If this bit is set the Num Lock key is enabled.
    /// </summary>
    public const int GLFW_MOD_NUM_LOCK = unchecked( ( int )0x0020 );

    public const int GLFW_MOUSE_BUTTON_1              = unchecked( ( int )0 );
    public const int GLFW_MOUSE_BUTTON_2              = unchecked( ( int )1 );
    public const int GLFW_MOUSE_BUTTON_3              = unchecked( ( int )2 );
    public const int GLFW_MOUSE_BUTTON_4              = unchecked( ( int )3 );
    public const int GLFW_MOUSE_BUTTON_5              = unchecked( ( int )4 );
    public const int GLFW_MOUSE_BUTTON_6              = unchecked( ( int )5 );
    public const int GLFW_MOUSE_BUTTON_7              = unchecked( ( int )6 );
    public const int GLFW_MOUSE_BUTTON_8              = unchecked( ( int )7 );
    public const int GLFW_MOUSE_BUTTON_LAST           = unchecked( ( int )GLFW_MOUSE_BUTTON_8 );
    public const int GLFW_MOUSE_BUTTON_LEFT           = unchecked( ( int )GLFW_MOUSE_BUTTON_1 );
    public const int GLFW_MOUSE_BUTTON_RIGHT          = unchecked( ( int )GLFW_MOUSE_BUTTON_2 );
    public const int GLFW_MOUSE_BUTTON_MIDDLE         = unchecked( ( int )GLFW_MOUSE_BUTTON_3 );
    public const int GLFW_JOYSTICK_1                  = unchecked( ( int )0 );
    public const int GLFW_JOYSTICK_2                  = unchecked( ( int )1 );
    public const int GLFW_JOYSTICK_3                  = unchecked( ( int )2 );
    public const int GLFW_JOYSTICK_4                  = unchecked( ( int )3 );
    public const int GLFW_JOYSTICK_5                  = unchecked( ( int )4 );
    public const int GLFW_JOYSTICK_6                  = unchecked( ( int )5 );
    public const int GLFW_JOYSTICK_7                  = unchecked( ( int )6 );
    public const int GLFW_JOYSTICK_8                  = unchecked( ( int )7 );
    public const int GLFW_JOYSTICK_9                  = unchecked( ( int )8 );
    public const int GLFW_JOYSTICK_10                 = unchecked( ( int )9 );
    public const int GLFW_JOYSTICK_11                 = unchecked( ( int )10 );
    public const int GLFW_JOYSTICK_12                 = unchecked( ( int )11 );
    public const int GLFW_JOYSTICK_13                 = unchecked( ( int )12 );
    public const int GLFW_JOYSTICK_14                 = unchecked( ( int )13 );
    public const int GLFW_JOYSTICK_15                 = unchecked( ( int )14 );
    public const int GLFW_JOYSTICK_16                 = unchecked( ( int )15 );
    public const int GLFW_JOYSTICK_LAST               = unchecked( ( int )GLFW_JOYSTICK_16 );
    public const int GLFW_GAMEPAD_BUTTON_A            = unchecked( ( int )0 );
    public const int GLFW_GAMEPAD_BUTTON_B            = unchecked( ( int )1 );
    public const int GLFW_GAMEPAD_BUTTON_X            = unchecked( ( int )2 );
    public const int GLFW_GAMEPAD_BUTTON_Y            = unchecked( ( int )3 );
    public const int GLFW_GAMEPAD_BUTTON_LEFT_BUMPER  = unchecked( ( int )4 );
    public const int GLFW_GAMEPAD_BUTTON_RIGHT_BUMPER = unchecked( ( int )5 );
    public const int GLFW_GAMEPAD_BUTTON_BACK         = unchecked( ( int )6 );
    public const int GLFW_GAMEPAD_BUTTON_START        = unchecked( ( int )7 );
    public const int GLFW_GAMEPAD_BUTTON_GUIDE        = unchecked( ( int )8 );
    public const int GLFW_GAMEPAD_BUTTON_LEFT_THUMB   = unchecked( ( int )9 );
    public const int GLFW_GAMEPAD_BUTTON_RIGHT_THUMB  = unchecked( ( int )10 );
    public const int GLFW_GAMEPAD_BUTTON_DPAD_UP      = unchecked( ( int )11 );
    public const int GLFW_GAMEPAD_BUTTON_DPAD_RIGHT   = unchecked( ( int )12 );
    public const int GLFW_GAMEPAD_BUTTON_DPAD_DOWN    = unchecked( ( int )13 );
    public const int GLFW_GAMEPAD_BUTTON_DPAD_LEFT    = unchecked( ( int )14 );
    public const int GLFW_GAMEPAD_BUTTON_LAST         = unchecked( ( int )GLFW_GAMEPAD_BUTTON_DPAD_LEFT );
    public const int GLFW_GAMEPAD_BUTTON_CROSS        = unchecked( ( int )GLFW_GAMEPAD_BUTTON_A );
    public const int GLFW_GAMEPAD_BUTTON_CIRCLE       = unchecked( ( int )GLFW_GAMEPAD_BUTTON_B );
    public const int GLFW_GAMEPAD_BUTTON_SQUARE       = unchecked( ( int )GLFW_GAMEPAD_BUTTON_X );
    public const int GLFW_GAMEPAD_BUTTON_TRIANGLE     = unchecked( ( int )GLFW_GAMEPAD_BUTTON_Y );
    public const int GLFW_GAMEPAD_AXIS_LEFT_X         = unchecked( ( int )0 );
    public const int GLFW_GAMEPAD_AXIS_LEFT_Y         = unchecked( ( int )1 );
    public const int GLFW_GAMEPAD_AXIS_RIGHT_X        = unchecked( ( int )2 );
    public const int GLFW_GAMEPAD_AXIS_RIGHT_Y        = unchecked( ( int )3 );
    public const int GLFW_GAMEPAD_AXIS_LEFT_TRIGGER   = unchecked( ( int )4 );
    public const int GLFW_GAMEPAD_AXIS_RIGHT_TRIGGER  = unchecked( ( int )5 );
    public const int GLFW_GAMEPAD_AXIS_LAST           = unchecked( ( int )GLFW_GAMEPAD_AXIS_RIGHT_TRIGGER );

    /// <summary>
    /// No error has occurred.
    /// </summary>
    public const int GLFW_NO_ERROR = unchecked( ( int )0 );

    /// <summary>
    /// GLFW has not been initialized.
    /// </summary>
    public const int GLFW_NOT_INITIALIZED = unchecked( ( int )0x00010001 );

    /// <summary>
    /// No context is current for this thread.
    /// </summary>
    public const int GLFW_NO_CURRENT_CONTEXT = unchecked( ( int )0x00010002 );

    /// <summary>
    /// One of the arguments to the function was an invalid enum value.
    /// </summary>
    public const int GLFW_INVALID_ENUM = unchecked( ( int )0x00010003 );

    /// <summary>
    /// One of the arguments to the function was an invalid value.
    /// </summary>
    public const int GLFW_INVALID_VALUE = unchecked( ( int )0x00010004 );

    /// <summary>
    /// A memory allocation failed.
    /// </summary>
    public const int GLFW_OUT_OF_MEMORY = unchecked( ( int )0x00010005 );

    /// <summary>
    /// GLFW could not find support for the requested API on the system.
    /// </summary>
    public const int GLFW_API_UNAVAILABLE = unchecked( ( int )0x00010006 );

    /// <summary>
    /// The requested OpenGL or OpenGL ES version is not available.
    /// </summary>
    public const int GLFW_VERSION_UNAVAILABLE = unchecked( ( int )0x00010007 );

    /// <summary>
    /// A platform-specific error occurred that does not match any of the more specific categories.
    /// </summary>
    public const int GLFW_PLATFORM_ERROR = unchecked( ( int )0x00010008 );

    /// <summary>
    /// The requested format is not supported or available.
    /// </summary>
    public const int GLFW_FORMAT_UNAVAILABLE = unchecked( ( int )0x00010009 );

    /// <summary>
    /// The specified window does not have an OpenGL or OpenGL ES context.
    /// </summary>
    public const int GLFW_NO_WINDOW_CONTEXT = unchecked( ( int )0x0001000A );

    /// <summary>
    /// The specified cursor shape is not available.
    /// </summary>
    public const int GLFW_CURSOR_UNAVAILABLE = unchecked( ( int )0x0001000B );

    /// <summary>
    /// The requested feature is not provided by the platform.
    /// </summary>
    public const int GLFW_FEATURE_UNAVAILABLE = unchecked( ( int )0x0001000C );

    /// <summary>
    /// The requested feature is not implemented for the platform.
    /// </summary>
    public const int GLFW_FEATURE_UNIMPLEMENTED = unchecked( ( int )0x0001000D );

    /// <summary>
    /// Platform unavailable or no matching platform was found.
    /// </summary>
    public const int GLFW_PLATFORM_UNAVAILABLE = unchecked( ( int )0x0001000E );

    /// <summary>
    /// Input focus window hint and attribute.
    /// </summary>
    public const int GLFW_FOCUSED = unchecked( ( int )0x00020001 );

    /// <summary>
    /// Window iconification window attribute.
    /// </summary>
    public const int GLFW_ICONIFIED = unchecked( ( int )0x00020002 );

    /// <summary>
    /// Window resize-ability window hint and attribute.
    /// </summary>
    public const int GLFW_RESIZABLE = unchecked( ( int )0x00020003 );

    /// <summary>
    /// Window visibility window hint and attribute.
    /// </summary>
    public const int GLFW_VISIBLE = unchecked( ( int )0x00020004 );

    /// <summary>
    /// Window decoration window hint and attribute.
    /// </summary>
    public const int GLFW_DECORATED = unchecked( ( int )0x00020005 );

    /// <summary>
    /// Window auto-iconification window hint and attribute.
    /// </summary>
    public const int GLFW_AUTO_ICONIFY = unchecked( ( int )0x00020006 );

    /// <summary>
    /// Window decoration window hint and attribute.
    /// </summary>
    public const int GLFW_FLOATING = unchecked( ( int )0x00020007 );

    /// <summary>
    /// Window maximization window hint and attribute.
    /// </summary>
    public const int GLFW_MAXIMIZED = unchecked( ( int )0x00020008 );

    /// <summary>
    /// Cursor centering window hint.
    /// </summary>
    public const int GLFW_CENTER_CURSOR = unchecked( ( int )0x00020009 );

    /// <summary>
    /// Window framebuffer transparency hint and attribute.
    /// </summary>
    public const int GLFW_TRANSPARENT_FRAMEBUFFER = unchecked( ( int )0x0002000A );

    /// <summary>
    /// Mouse cursor hover window attribute.
    /// </summary>
    public const int GLFW_HOVERED = unchecked( ( int )0x0002000B );

    /// <summary>
    /// Input focus on calling show window hint and attribute.
    /// </summary>
    public const int GLFW_FOCUS_ON_SHOW = unchecked( ( int )0x0002000C );

    /// <summary>
    /// Mouse input transparency window hint and attribute.
    /// </summary>
    public const int GLFW_MOUSE_PASSTHROUGH = unchecked( ( int )0x0002000D );

    /// <summary>
    /// Initial position x-coordinate window hint.
    /// </summary>
    public const int GLFW_POSITION_X = unchecked( ( int )0x0002000E );

    /// <summary>
    /// Initial position y-coordinate window hint.
    /// </summary>
    public const int GLFW_POSITION_Y = unchecked( ( int )0x0002000F );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_RED_BITS = unchecked( ( int )0x00021001 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_GREEN_BITS = unchecked( ( int )0x00021002 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_BLUE_BITS = unchecked( ( int )0x00021003 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_ALPHA_BITS = unchecked( ( int )0x00021004 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_DEPTH_BITS = unchecked( ( int )0x00021005 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_STENCIL_BITS = unchecked( ( int )0x00021006 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_ACCUM_RED_BITS = unchecked( ( int )0x00021007 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_ACCUM_GREEN_BITS = unchecked( ( int )0x00021008 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_ACCUM_BLUE_BITS = unchecked( ( int )0x00021009 );

    /// <summary>
    /// Framebuffer bit depth hint.
    /// </summary>
    public const int GLFW_ACCUM_ALPHA_BITS = unchecked( ( int )0x0002100A );

    /// <summary>
    /// Framebuffer auxiliary buffer hint.
    /// </summary>
    public const int GLFW_AUX_BUFFERS = unchecked( ( int )0x0002100B );

    /// <summary>
    /// OpenGL stereoscopic rendering hint.
    /// </summary>
    public const int GLFW_STEREO = unchecked( ( int )0x0002100C );

    /// <summary>
    /// Framebuffer MSAA samples hint.
    /// </summary>
    public const int GLFW_SAMPLES = unchecked( ( int )0x0002100D );

    /// <summary>
    /// Framebuffer sRGB hint.
    /// </summary>
    public const int GLFW_SRGB_CAPABLE = unchecked( ( int )0x0002100E );

    /// <summary>
    /// Monitor refresh rate hint.
    /// </summary>
    public const int GLFW_REFRESH_RATE = unchecked( ( int )0x0002100F );

    /// <summary>
    /// Framebuffer double buffering hint and attribute.
    /// </summary>
    public const int GLFW_DOUBLEBUFFER = unchecked( ( int )0x00021010 );

    /// <summary>
    /// Context client API hint and attribute.
    /// </summary>
    public const int GLFW_CLIENT_API = unchecked( ( int )0x00022001 );

    /// <summary>
    /// Context client API major version hint and attribute.
    /// </summary>
    public const int GLFW_CONTEXT_VERSION_MAJOR = unchecked( ( int )0x00022002 );

    /// <summary>
    /// Context client API minor version hint and attribute.
    /// </summary>
    public const int GLFW_CONTEXT_VERSION_MINOR = unchecked( ( int )0x00022003 );

    /// <summary>
    /// Context client API revision number attribute.
    /// </summary>
    public const int GLFW_CONTEXT_REVISION = unchecked( ( int )0x00022004 );

    /// <summary>
    /// Context robustness hint and attribute.
    /// </summary>
    public const int GLFW_CONTEXT_ROBUSTNESS = unchecked( ( int )0x00022005 );

    /// <summary>
    /// OpenGL forward-compatibility hint and attribute.
    /// </summary>
    public const int GLFW_OPENGL_FORWARD_COMPAT = unchecked( ( int )0x00022006 );

    /// <summary>
    /// Debug mode context hint and attribute.
    /// </summary>
    public const int GLFW_CONTEXT_DEBUG = unchecked( ( int )0x00022007 );

    /// <summary>
    /// Legacy name for compatibility.
    /// </summary>
    public const int GLFW_OPENGL_DEBUG_CONTEXT = unchecked( ( int )GLFW_CONTEXT_DEBUG );

    /// <summary>
    /// OpenGL profile hint and attribute.
    /// </summary>
    public const int GLFW_OPENGL_PROFILE = unchecked( ( int )0x00022008 );

    /// <summary>
    /// Context flush-on-release hint and attribute.
    /// </summary>
    public const int GLFW_CONTEXT_RELEASE_BEHAVIOR = unchecked( ( int )0x00022009 );

    /// <summary>
    /// Context error suppression hint and attribute.
    /// </summary>
    public const int GLFW_CONTEXT_NO_ERROR = unchecked( ( int )0x0002200A );

    /// <summary>
    /// Context creation API hint and attribute.
    /// </summary>
    public const int GLFW_CONTEXT_CREATION_API = unchecked( ( int )0x0002200B );

    /// <summary>
    /// Window content area scaling window window hint.
    /// </summary>
    public const int GLFW_SCALE_TO_MONITOR = unchecked( ( int )0x0002200C );

    /// <summary>
    /// Window framebuffer scaling window hint.
    /// </summary>
    public const int GLFW_SCALE_FRAMEBUFFER = unchecked( ( int )0x0002200D );

    /// <summary>
    /// Legacy name for compatibility.
    /// </summary>
    public const int GLFW_COCOA_RETINA_FRAMEBUFFER = unchecked( ( int )0x00023001 );

    /// <summary>
    /// macOS specific window hint.
    /// </summary>
    public const int GLFW_COCOA_FRAME_NAME = unchecked( ( int )0x00023002 );

    /// <summary>
    /// macOS specific window hint.
    /// </summary>
    public const int GLFW_COCOA_GRAPHICS_SWITCHING = unchecked( ( int )0x00023003 );

    /// <summary>
    /// X11 specific window hint.
    /// </summary>
    public const int GLFW_X11_CLASS_NAME = unchecked( ( int )0x00024001 );

    /// <summary>
    /// X11 specific window hint.
    /// </summary>
    public const int GLFW_X11_INSTANCE_NAME = unchecked( ( int )0x00024002 );

    public const int GLFW_WIN32_KEYBOARD_MENU = unchecked( ( int )0x00025001 );

    /// <summary>
    /// Win32 specific window hint.
    /// </summary>
    public const int GLFW_WIN32_SHOWDEFAULT = unchecked( ( int )0x00025002 );

    /// <summary>
    /// Wayland specific window hint.
    /// </summary>
    public const int GLFW_WAYLAND_APP_ID = unchecked( ( int )0x00026001 );

    public const int GLFW_NO_API                       = unchecked( ( int )0 );
    public const int GLFW_OPENGL_API                   = unchecked( ( int )0x00030001 );
    public const int GLFW_OPENGL_ES_API                = unchecked( ( int )0x00030002 );
    public const int GLFW_NO_ROBUSTNESS                = unchecked( ( int )0 );
    public const int GLFW_NO_RESET_NOTIFICATION        = unchecked( ( int )0x00031001 );
    public const int GLFW_LOSE_CONTEXT_ON_RESET        = unchecked( ( int )0x00031002 );
    public const int GLFW_OPENGL_ANY_PROFILE           = unchecked( ( int )0 );
    public const int GLFW_OPENGL_CORE_PROFILE          = unchecked( ( int )0x00032001 );
    public const int GLFW_OPENGL_COMPAT_PROFILE        = unchecked( ( int )0x00032002 );
    public const int GLFW_CURSOR                       = unchecked( ( int )0x00033001 );
    public const int GLFW_STICKY_KEYS                  = unchecked( ( int )0x00033002 );
    public const int GLFW_STICKY_MOUSE_BUTTONS         = unchecked( ( int )0x00033003 );
    public const int GLFW_LOCK_KEY_MODS                = unchecked( ( int )0x00033004 );
    public const int GLFW_RAW_MOUSE_MOTION             = unchecked( ( int )0x00033005 );
    public const int GLFW_CURSOR_NORMAL                = unchecked( ( int )0x00034001 );
    public const int GLFW_CURSOR_HIDDEN                = unchecked( ( int )0x00034002 );
    public const int GLFW_CURSOR_DISABLED              = unchecked( ( int )0x00034003 );
    public const int GLFW_CURSOR_CAPTURED              = unchecked( ( int )0x00034004 );
    public const int GLFW_ANY_RELEASE_BEHAVIOR         = unchecked( ( int )0 );
    public const int GLFW_RELEASE_BEHAVIOR_FLUSH       = unchecked( ( int )0x00035001 );
    public const int GLFW_RELEASE_BEHAVIOR_NONE        = unchecked( ( int )0x00035002 );
    public const int GLFW_NATIVE_CONTEXT_API           = unchecked( ( int )0x00036001 );
    public const int GLFW_EGL_CONTEXT_API              = unchecked( ( int )0x00036002 );
    public const int GLFW_OSMESA_CONTEXT_API           = unchecked( ( int )0x00036003 );
    public const int GLFW_ANGLE_PLATFORM_TYPE_NONE     = unchecked( ( int )0x00037001 );
    public const int GLFW_ANGLE_PLATFORM_TYPE_OPENGL   = unchecked( ( int )0x00037002 );
    public const int GLFW_ANGLE_PLATFORM_TYPE_OPENGLES = unchecked( ( int )0x00037003 );
    // ReSharper disable once InconsistentNaming
    public const int GLFW_ANGLE_PLATFORM_TYPE_D3D9     = unchecked( ( int )0x00037004 );
    // ReSharper disable once InconsistentNaming
    public const int GLFW_ANGLE_PLATFORM_TYPE_D3D11    = unchecked( ( int )0x00037005 );
    public const int GLFW_ANGLE_PLATFORM_TYPE_VULKAN   = unchecked( ( int )0x00037007 );
    public const int GLFW_ANGLE_PLATFORM_TYPE_METAL    = unchecked( ( int )0x00037008 );
    public const int GLFW_WAYLAND_PREFER_LIBDECOR      = unchecked( ( int )0x00038001 );
    public const int GLFW_WAYLAND_DISABLE_LIBDECOR     = unchecked( ( int )0x00038002 );
    public const int GLFW_ANY_POSITION                 = unchecked( ( int )0x80000000 );

    /// <summary>
    /// The regular arrow cursor shape.
    /// </summary>
    public const int GLFW_ARROW_CURSOR = unchecked( ( int )0x00036001 );

    /// <summary>
    /// The text input I-beam cursor shape.
    /// </summary>
    public const int GLFW_IBEAM_CURSOR = unchecked( ( int )0x00036002 );

    /// <summary>
    /// The crosshair cursor shape.
    /// </summary>
    public const int GLFW_CROSSHAIR_CURSOR = unchecked( ( int )0x00036003 );

    /// <summary>
    /// The pointing hand cursor shape.
    /// </summary>
    public const int GLFW_POINTING_HAND_CURSOR = unchecked( ( int )0x00036004 );

    /// <summary>
    /// The horizontal resize/move arrow shape.
    /// </summary>
    public const int GLFW_RESIZE_EW_CURSOR = unchecked( ( int )0x00036005 );

    /// <summary>
    /// The vertical resize/move arrow shape.
    /// </summary>
    public const int GLFW_RESIZE_NS_CURSOR = unchecked( ( int )0x00036006 );

    /// <summary>
    /// The top-left to bottom-right diagonal resize/move arrow shape.
    /// </summary>
    public const int GLFW_RESIZE_NWSE_CURSOR = unchecked( ( int )0x00036007 );

    /// <summary>
    /// The top-right to bottom-left diagonal resize/move arrow shape.
    /// </summary>
    public const int GLFW_RESIZE_NESW_CURSOR = unchecked( ( int )0x00036008 );

    /// <summary>
    /// The omni-directional resize/move cursor shape.
    /// </summary>
    public const int GLFW_RESIZE_ALL_CURSOR = unchecked( ( int )0x00036009 );

    /// <summary>
    /// The operation-not-allowed shape.
    /// </summary>
    public const int GLFW_NOT_ALLOWED_CURSOR = unchecked( ( int )0x0003600A );

    /// <summary>
    /// Legacy name for compatibility.
    /// </summary>
    public const int GLFW_HRESIZE_CURSOR = unchecked( ( int )GLFW_RESIZE_EW_CURSOR );

    /// <summary>
    /// Legacy name for compatibility.
    /// </summary>
    public const int GLFW_VRESIZE_CURSOR = unchecked( ( int )GLFW_RESIZE_NS_CURSOR );

    /// <summary>
    /// Legacy name for compatibility.
    /// </summary>
    public const int GLFW_HAND_CURSOR = unchecked( ( int )GLFW_POINTING_HAND_CURSOR );

    public const int GLFW_CONNECTED    = unchecked( ( int )0x00040001 );
    public const int GLFW_DISCONNECTED = unchecked( ( int )0x00040002 );

    /// <summary>
    /// Joystick hat buttons init hint.
    /// </summary>
    public const int GLFW_JOYSTICK_HAT_BUTTONS = unchecked( ( int )0x00050001 );

    /// <summary>
    /// ANGLE rendering backend init hint.
    /// </summary>
    public const int GLFW_ANGLE_PLATFORM_TYPE = unchecked( ( int )0x00050002 );

    /// <summary>
    /// Platform selection init hint.
    /// </summary>
    public const int GLFW_PLATFORM = unchecked( ( int )0x00050003 );

    /// <summary>
    /// macOS specific init hint.
    /// </summary>
    public const int GLFW_COCOA_CHDIR_RESOURCES = unchecked( ( int )0x00051001 );

    /// <summary>
    /// macOS specific init hint.
    /// </summary>
    public const int GLFW_COCOA_MENUBAR = unchecked( ( int )0x00051002 );

    /// <summary>
    /// X11 specific init hint.
    /// </summary>
    public const int GLFW_X11_XCB_VULKAN_SURFACE = unchecked( ( int )0x00052001 );

    /// <summary>
    /// Wayland specific init hint.
    /// </summary>
    public const int GLFW_WAYLAND_LIBDECOR = unchecked( ( int )0x00053001 );

    /// <summary>
    /// Hint value that enables automatic platform selection.
    /// </summary>
    public const int GLFW_ANY_PLATFORM = unchecked( ( int )0x00060000 );

    public const int GLFW_PLATFORM_WIN32   = unchecked( ( int )0x00060001 );
    public const int GLFW_PLATFORM_COCOA   = unchecked( ( int )0x00060002 );
    public const int GLFW_PLATFORM_WAYLAND = unchecked( ( int )0x00060003 );
    public const int GLFW_PLATFORM_X11     = unchecked( ( int )0x00060004 );
    public const int GLFW_PLATFORM_NULL    = unchecked( ( int )0x00060005 );
    public const int GLFW_DONT_CARE        = unchecked( ( int )-1 );

    /// <summary>
    /// The major version number of the GLFW header.
    /// </summary>
    public const int GLFW_VERSION_MAJOR = unchecked( ( int )3 );

    /// <summary>
    /// The minor version number of the GLFW header.
    /// </summary>
    public const int GLFW_VERSION_MINOR = unchecked( ( int )4 );

    /// <summary>
    /// The revision number of the GLFW header.
    /// </summary>
    public const int GLFW_VERSION_REVISION = unchecked( ( int )0 );

    /// <summary>
    /// The key or mouse button was released.
    /// </summary>
    public const int GLFW_RELEASE = unchecked( ( int )0 );

    /// <summary>
    /// The key or mouse button was pressed.
    /// </summary>
    public const int GLFW_PRESS = unchecked( ( int )1 );

    /// <summary>
    /// The key was held down until it repeated.
    /// </summary>
    public const int GLFW_REPEAT = unchecked( ( int )2 );
}
#pragma warning restore 1591