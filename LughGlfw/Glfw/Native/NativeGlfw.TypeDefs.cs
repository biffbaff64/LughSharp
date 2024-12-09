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

using System.Runtime.InteropServices;
using System.Security;

namespace LughGlfw.Glfw.Native;

[SuppressUnmanagedCodeSecurity]
public static unsafe partial class NativeGlfw
{
    /// <summary>
    /// <para>Opaque monitor object.</para>
    /// <para><b>See also</b>
    /// <para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_object">Monitor objects</see>
    /// </para>
    /// </para>
    /// <para><b>Since</b>
    /// <para>Added in version 3.0. </para>
    /// </para>
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct GlfwMonitor
    {
    }

    /// <summary>
    /// <para>Opaque window object.</para>
    /// <para><b>See also</b>
    /// <para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_object">Window objects</see>
    /// </para>
    /// </para>
    /// <para><b>Since</b>
    /// <para>Added in version 3.0. </para>
    /// </para>
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct GlfwWindow
    {
    }

    /// <summary>
    /// <para>Opaque cursor object.</para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_object">Cursor objects</see></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct GlfwCursor
    {
    }

    /// <summary>
    /// <para>This is the function pointer type for memory allocation callbacks. A memory allocation callback function has the following signature: </para>void* function_name(size_t size, void* user) <para>This function must return either a memory block at least <c>size</c> bytes long, or <c>NULL</c> if allocation failed. Note that not all parts of GLFW handle allocation failures gracefully yet.</para> <para>This function must support being called during <see cref="NativeGlfw.glfwInit" /> but before the library is flagged as initialized, as well as during <see cref="NativeGlfw.glfwTerminate" /> after the library is no longer flagged as initialized.</para> <para>Any memory allocated via this function will be deallocated via the same allocator during library termination or earlier.</para> <para>Any memory allocated via this function must be suitably aligned for any object type. If you are using C99 or earlier, this alignment is platform-dependent but will be the same as what <c>malloc</c> provides. If you are using C11 or later, this is the value of <c>alignof(max_align_t)</c>.</para> <para>The size will always be greater than zero. Allocations of size zero are filtered out before reaching the custom allocator.</para> <para>If this function returns <c>NULL</c>, GLFW will emit <see cref="NativeGlfw.GLFW_OUT_OF_MEMORY" />.</para> <para>This function must not call any GLFW function.</para> <para><b>Returns</b><para>The address of the newly allocated memory block, or <c>NULL</c> if an error occurred.</para></para> <para><b>Pointer lifetime</b><para>The returned memory block must be valid at least until it is deallocated.</para></para> <para><b>Reentrancy</b><para>This function should not call any GLFW function.</para></para> <para><b>Thread safety</b><para>This function must support being called from any thread that calls GLFW functions.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#init_allocator">Custom heap memory allocator</see> </para> <para> <see cref="NativeGlfw.GLFWallocator" /></para></para> <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate IntPtr GlfwAllocatefun( nint size, IntPtr user );

    /// <summary>
    /// <para>This is the function pointer type for memory reallocation callbacks. A memory reallocation callback function has the following signature: </para>void* function_name(void* block, size_t size, void* user) <para>This function must return a memory block at least <c>size</c> bytes long, or <c>NULL</c> if allocation failed. Note that not all parts of GLFW handle allocation failures gracefully yet.</para> <para>This function must support being called during <see cref="NativeGlfw.glfwInit" /> but before the library is flagged as initialized, as well as during <see cref="NativeGlfw.glfwTerminate" /> after the library is no longer flagged as initialized.</para> <para>Any memory allocated via this function will be deallocated via the same allocator during library termination or earlier.</para> <para>Any memory allocated via this function must be suitably aligned for any object type. If you are using C99 or earlier, this alignment is platform-dependent but will be the same as what <c>realloc</c> provides. If you are using C11 or later, this is the value of <c>alignof(max_align_t)</c>.</para> <para>The block address will never be <c>NULL</c> and the size will always be greater than zero. Reallocations of a block to size zero are converted into deallocations before reaching the custom allocator. Reallocations of <c>NULL</c> to a non-zero size are converted into regular allocations before reaching the custom allocator.</para> <para>If this function returns <c>NULL</c>, GLFW will emit <see cref="NativeGlfw.GLFW_OUT_OF_MEMORY" />.</para> <para>This function must not call any GLFW function.</para> <para><b>Returns</b><para>The address of the newly allocated or resized memory block, or <c>NULL</c> if an error occurred.</para></para> <para><b>Pointer lifetime</b><para>The returned memory block must be valid at least until it is deallocated.</para></para> <para><b>Reentrancy</b><para>This function should not call any GLFW function.</para></para> <para><b>Thread safety</b><para>This function must support being called from any thread that calls GLFW functions.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#init_allocator">Custom heap memory allocator</see> </para> <para> <see cref="NativeGlfw.GLFWallocator" /></para></para> <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate IntPtr GlfwReallocatefun( IntPtr block, nint size, IntPtr user );

    /// <summary>
    /// This is the function pointer type for memory deallocation callbacks. A memory
    /// deallocation callback function has the following signature:
    /// <code>
    /// void function_name(void* block, void* user)
    /// </code>
    /// <para>
    /// This function may deallocate the specified memory block. This memory block will
    /// have been allocated with the same allocator.
    /// </para>
    /// <para>
    /// This function must support being called during <see cref="NativeGlfw.glfwInit"/> but before the
    /// library is flagged as initialized, as well as during <see cref="NativeGlfw.glfwTerminate"/>
    /// after the library is no longer flagged as initialized.
    /// </para>
    /// <para>
    /// The block address will never be <b>NULL</b>. Deallocations of <b>NULL</b> are filtered out
    /// before reaching the custom allocator.
    /// </para>
    /// <para>
    /// If this function returns <b>NULL</b>, GLFW will emit <see cref="NativeGlfw.GLFW_OUT_OF_MEMORY"/>.
    /// </para>
    /// <para>This function must not call any GLFW function.</para>
    /// <para><b>Pointer lifetime</b>
    /// <para> The specified memory block will not be accessed by GLFW after this function is called.</para>
    /// </para>
    /// <para><b>Reentrancy</b>
    /// <para>This function should not call any GLFW function.</para>
    /// </para>
    /// <para><b>Thread safety</b>
    /// <para>This function must support being called from any thread that calls GLFW functions.</para>
    /// </para>
    /// <para><b>See also</b>
    /// <para>
    /// <see href="https://www.glfw.org/docs/3.4/intro_guide.html#init_allocator">Custom heap memory allocator</see>
    /// </para>
    /// <para> <see cref="NativeGlfw.GLFWallocator"/></para>
    /// </para>
    /// <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GlfwDeallocatefun( IntPtr block, IntPtr user );

    /// <summary>
    /// <para>This is the function pointer type for error callbacks. An error callback function has the following signature: </para>void callback_name(int error_code, const char* description) <para><b>Pointer lifetime</b><para>The error description string is valid until the callback function returns.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">Error handling</see> </para> <para> <see cref="NativeGlfw.glfwSetErrorCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWerrorfun( int errorCode, IntPtr description );

    /// <summary>
    /// <para>This is the function pointer type for window position callbacks. A window position callback function has the following signature: </para>void callback_name(<see cref="GlfwWindow" />* window, int xpos, int ypos) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_pos">Window position</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowPosCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWwindowposfun( GlfwWindow* window, int xpos, int ypos );

    /// <summary>
    /// <para>This is the function pointer type for window size callbacks. A window size callback function has the following signature: </para>void callback_name(<see cref="GlfwWindow" />* window, int width, int height) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_size">Window size</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowSizeCallback" /></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWwindowsizefun( GlfwWindow* window, int width, int height );

    /// <summary>
    /// <para>This is the function pointer type for window close callbacks. A window close callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_close">Window closing and close flag</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowCloseCallback" /></para></para> <para><b>Since</b><para>Added in version 2.5. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWwindowclosefun( GlfwWindow* window );

    /// <summary>
    /// <para>This is the function pointer type for window content refresh callbacks. A window content refresh callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window); <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_refresh">Window damage and refresh</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowRefreshCallback" /></para></para> <para><b>Since</b><para>Added in version 2.5. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWwindowrefreshfun( GlfwWindow* window );

    /// <summary>
    /// <para>This is the function pointer type for window focus callbacks. A window focus callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, int focused) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_focus">Window input focus</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowFocusCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWwindowfocusfun( GlfwWindow* window, int focused );

    /// <summary>
    /// <para>This is the function pointer type for window iconify callbacks. A window iconify callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, int iconified) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_iconify">Window iconification</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowIconifyCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWwindowiconifyfun( GlfwWindow* window, int iconified );

    /// <summary>
    /// <para>This is the function pointer type for window maximize callbacks. A window maximize callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, int maximized) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_maximize">Window maximization</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowMaximizeCallback" /></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWwindowmaximizefun( GlfwWindow* window, int maximized );

    /// <summary>
    /// <para>This is the function pointer type for framebuffer size callbacks. A framebuffer size callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, int width, int height) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_fbsize">Framebuffer size</see> </para> <para> <see cref="NativeGlfw.glfwSetFramebufferSizeCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWframebuffersizefun( GlfwWindow* window, int width, int height );

    /// <summary>
    /// <para>This is the function pointer type for window content scale callbacks. A window content scale callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, float xscale, float yscale) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_scale">Window content scale</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowContentScaleCallback" /></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWwindowcontentscalefun( GlfwWindow* window, float xscale, float yscale );

    /// <summary>
    /// <para>This is the function pointer type for mouse button callback functions. A mouse button callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, int button, int action, int mods) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_mouse_button">Mouse button input</see> </para> <para> <see cref="NativeGlfw.glfwSetMouseButtonCallback" /></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle and modifier mask parameters. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWmousebuttonfun( GlfwWindow* window, int button, int action, int mods );

    /// <summary>
    /// <para>This is the function pointer type for cursor position callbacks. A cursor position callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, double xpos, double ypos); <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_pos">Cursor position</see> </para> <para> <see cref="NativeGlfw.glfwSetCursorPosCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>GLFWmouseposfun</c>. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWcursorposfun( GlfwWindow* window, double xpos, double ypos );

    /// <summary>
    /// <para>This is the function pointer type for cursor enter/leave callbacks. A cursor enter/leave callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, int entered) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_enter">Cursor enter/leave events</see> </para> <para> <see cref="NativeGlfw.glfwSetCursorEnterCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWcursorenterfun( GlfwWindow* window, int entered );

    /// <summary>
    /// <para>This is the function pointer type for scroll callbacks. A scroll callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, double xoffset, double yoffset) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#scrolling">Scroll input</see> </para> <para> <see cref="NativeGlfw.glfwSetScrollCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>GLFWmousewheelfun</c>. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWscrollfun( GlfwWindow* window, double xoffset, double yoffset );

    /// <summary>
    /// <para>This is the function pointer type for keyboard key callbacks. A keyboard key callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, int key, int scancode, int action, int mods) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_key">Key input</see> </para> <para> <see cref="NativeGlfw.glfwSetKeyCallback" /></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle, scancode and modifier mask parameters. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWkeyfun( GlfwWindow* window, int key, int scancode, int action, int mods );

    /// <summary>
    /// <para>This is the function pointer type for Unicode character callbacks. A Unicode character callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, unsigned int codepoint) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_char">Text input</see> </para> <para> <see cref="NativeGlfw.glfwSetCharCallback" /></para></para> <para><b>Since</b><para>Added in version 2.4. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWcharfun( GlfwWindow* window, uint codepoint );

    /// <summary>
    /// <para>This is the function pointer type for Unicode character with modifiers callbacks. It is called for each input character, regardless of what modifier keys are held down. A Unicode character with modifiers callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, unsigned int codepoint, int mods) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_char">Text input</see> </para> <para> <see cref="NativeGlfw.glfwSetCharModsCallback" /></para></para> <para><b><see href="https://www.glfw.org/docs/3.4/deprecated.html#_deprecated000001">Deprecated:</see></b><para>Scheduled for removal in version 4.0.</para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWcharmodsfun( GlfwWindow* window, uint codepoint, int mods );

    /// <summary>
    /// <para>This is the function pointer type for path drop callbacks. A path drop callback function has the following signature: </para>void function_name(<see cref="GlfwWindow" />* window, int path_count, const char* paths[]) <para><b>Pointer lifetime</b><para>The path array and its strings are valid until the callback function returns.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#path_drop">Path drop input</see> </para> <para> <see cref="NativeGlfw.glfwSetDropCallback" /></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWdropfun( GlfwWindow* window, int path_count, IntPtr paths );

    /// <summary>
    /// <para>This is the function pointer type for monitor configuration callbacks. A monitor callback function has the following signature: </para>void function_name(<see cref="GlfwMonitor" />* monitor, int event) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_event">Monitor configuration changes</see> </para> <para> <see cref="NativeGlfw.glfwSetMonitorCallback" /></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWmonitorfun( GlfwMonitor* monitor, int @event );

    /// <summary>
    /// <para>This is the function pointer type for joystick configuration callbacks. A joystick configuration callback function has the following signature: </para>void function_name(int jid, int event) <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick_event">Joystick configuration changes</see> </para> <para> <see cref="NativeGlfw.glfwSetJoystickCallback" /></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate void GLFWjoystickfun( int jid, int @event );

    /// <summary>
    /// <para>This describes a single video mode.</para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_modes">Video modes</see> </para> <para> <see cref="NativeGlfw.glfwGetVideoMode" /> </para> <para> <see cref="NativeGlfw.glfwGetVideoModes" /></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added refresh rate member. </para></para>
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct GLFWvidmode
    {
        /// <summary>
        /// <para>The width, in screen coordinates, of the video mode. </para>
        /// </summary>
        public int width;

        /// <summary>
        /// <para>The height, in screen coordinates, of the video mode. </para>
        /// </summary>
        public int height;

        /// <summary>
        /// <para>The bit depth of the red channel of the video mode. </para>
        /// </summary>
        public int redBits;

        /// <summary>
        /// <para>The bit depth of the green channel of the video mode. </para>
        /// </summary>
        public int greenBits;

        /// <summary>
        /// <para>The bit depth of the blue channel of the video mode. </para>
        /// </summary>
        public int blueBits;

        /// <summary>
        /// <para>The refresh rate, in Hz, of the video mode. </para>
        /// </summary>
        public int refreshRate;
    }

    /// <summary>
    /// <para>This describes the gamma ramp for a monitor.</para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_gamma">Gamma ramp</see> </para> <para> <see cref="NativeGlfw.glfwGetGammaRamp" /> </para> <para> <see cref="NativeGlfw.glfwSetGammaRamp" /></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct GLFWgammaramp
    {
        /// <summary>
        /// <para>An array of value describing the response of the red channel. </para>
        /// </summary>
        public IntPtr red;

        /// <summary>
        /// <para>An array of value describing the response of the green channel. </para>
        /// </summary>
        public IntPtr green;

        /// <summary>
        /// <para>An array of value describing the response of the blue channel. </para>
        /// </summary>
        public IntPtr blue;

        /// <summary>
        /// <para>The number of elements in each array. </para>
        /// </summary>
        public uint size;
    }

    /// <summary>
    /// <para>This describes a single 2D image. See the documentation for each related function what the expected pixel format is.</para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_custom">Custom cursor creation</see> </para> <para> <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_icon">Window icon</see></para></para> <para><b>Since</b><para>Added in version 2.1. GLFW 3: Removed format and bytes-per-pixel members. </para></para>
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct GLFWimage
    {
        /// <summary>
        /// <para>The width, in pixels, of this image. </para>
        /// </summary>
        public int width;

        /// <summary>
        /// <para>The height, in pixels, of this image. </para>
        /// </summary>
        public int height;

        /// <summary>
        /// <para>The pixel data of this image, arranged left-to-right, top-to-bottom. </para>
        /// </summary>
        public IntPtr pixels;
    }

    /// <summary>
    /// <para>This describes the input state of a gamepad.</para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#gamepad">Gamepad input</see> </para> <para> <see cref="NativeGlfw.glfwGetGamepadState" /></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct GLFWgamepadstate
    {
        /// <summary>
        /// <para>The states of each <see href="https://www.glfw.org/docs/3.4/group__gamepad__buttons.html">gamepad button</see>, <see cref="NativeGlfw.GLFW_PRESS" /> or <see cref="NativeGlfw.GLFW_RELEASE" />. </para>
        /// </summary>
        public fixed byte buttons[ 15 ];

        /// <summary>
        /// <para>The states of each <see href="https://www.glfw.org/docs/3.4/group__gamepad__axes.html">gamepad axis</see>, in the range -1.0 to 1.0 inclusive. </para>
        /// </summary>
        public fixed float axes[ 6 ];
    }

    /// <summary>
    /// <para>This describes a custom heap memory allocator for GLFW. To set an allocator, pass it to <see cref="NativeGlfw.glfwInitAllocator" /> before initializing the library.</para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#init_allocator">Custom heap memory allocator</see> </para> <para> <see cref="NativeGlfw.glfwInitAllocator" /></para></para> <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public unsafe struct GLFWallocator
    {
        /// <summary>
        /// <para>The memory allocation function. See <see cref="GlfwAllocatefun" /> for details about allocation function. </para>
        /// </summary>
        public IntPtr allocate;

        /// <summary>
        /// <para>The memory reallocation function. See <see cref="GlfwReallocatefun" /> for details about reallocation function. </para>
        /// </summary>
        public IntPtr reallocate;

        /// <summary>
        /// <para>The memory deallocation function. See <see cref="GlfwDeallocatefun" /> for details about deallocation function. </para>
        /// </summary>
        public IntPtr deallocate;

        /// <summary>
        /// <para>The user pointer for this custom allocator. This value will be passed to the allocator functions. </para>
        /// </summary>
        public IntPtr user;
    }

    /// <summary>
    /// The function pointer type for the instance-level Vulkan function vkGetInstanceProcAddr.
    /// </summary>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate IntPtr PFN_vkGetInstanceProcAddr( IntPtr instance, IntPtr pName );
}