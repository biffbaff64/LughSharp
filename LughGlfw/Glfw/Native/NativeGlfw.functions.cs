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
using System.Runtime.InteropServices;

namespace LughGlfw.Glfw.Native;

[SuppressMessage( "ReSharper", "InconsistentNaming" )]
public static unsafe partial class NativeGlfw
{
    private const string LIBRARY_DLL = "glfw3";

    // ========================================================================

    private static DllLoader.GetProcAddressDelegate? __getProcAddressForGLFWFunction;

    private static T LoadFunction< T >( string name ) where T : Delegate
    {
        Console.WriteLine( $"Loading function {name}" );

        // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
        if ( __getProcAddressForGLFWFunction == null )
        {
            __getProcAddressForGLFWFunction = DllLoader.GetLoadFunctionPointerDelegate( LIBRARY_DLL );
        }

        return Marshal.GetDelegateForFunctionPointer< T >( __getProcAddressForGLFWFunction( name ) );
    }

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwInit();

    private static d_glfwInit p_glfwInit = LoadFunction< d_glfwInit >( "GlfwInit" );

    /// <summary>
    /// This function initializes the GLFW library. Before most GLFW functions can be used,
    /// GLFW must be initialized, and before an application terminates GLFW should be terminated
    /// in order to free any resources allocated during or after initialization.
    /// <para>
    /// If this function fails, it calls <see cref="NativeGlfw.glfwTerminate"/> before returning.
    /// If it succeeds, you should call <see cref="NativeGlfw.glfwTerminate"/> before the application
    /// exits.
    /// </para>
    /// <para>
    /// Additional calls to this function after successful initialization but before termination
    /// will return <see cref="NativeGlfw.GLFW_TRUE"/> immediately.
    /// </para>
    /// <para>
    /// The <see cref="NativeGlfw.GLFW_PLATFORM"/> init hint controls which platforms are considered
    /// during initialization. This also depends on which platforms the library was compiled to support.
    /// </para>
    /// <para>
    /// <b>Returns</b>
    /// <para>
    /// <see cref="NativeGlfw.GLFW_TRUE"/> if successful, or <see cref="NativeGlfw.GLFW_FALSE"/> if an
    /// <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.
    /// </para>
    /// </para>
    /// <para><b>Errors</b>
    /// <para>
    /// Possible errors include <see cref="NativeGlfw.GLFW_PLATFORM_UNAVAILABLE"/> and
    /// <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Remarks</b>
    /// <para>
    /// macOS: This function will change the current directory of the application to the
    /// <c>Contents/Resources</c> subdirectory of the application's bundle, if present. This can be
    /// disabled with the <see cref="NativeGlfw.GLFW_COCOA_CHDIR_RESOURCES"/> init hint.
    /// </para>
    /// <para>
    /// macOS: This function will create the main menu and dock icon for the application. If GLFW
    /// finds a <c>MainMenu.nib</c> it is loaded and assumed to contain a menu bar. Otherwise a
    /// minimal menu bar is created manually with common commands like Hide, Quit and About. The
    /// About entry opens a minimal about dialog with information from the application's bundle.
    /// The menu bar and dock icon can be disabled entirely with the <see cref="NativeGlfw.GLFW_COCOA_MENUBAR"/>
    /// init hint.
    /// </para>
    /// <para>
    /// Wayland, X11: If the library was compiled with support for both Wayland and X11, and the
    /// <see cref="NativeGlfw.GLFW_PLATFORM"/> init hint is set to <see cref="NativeGlfw.GLFW_ANY_PLATFORM"/>,
    /// the <c>XDG_SESSION_TYPE</c> environment variable affects which platform is picked. If the environment
    /// variable is not set, or is set to something other than <c>wayland</c> or <c>x11</c>, the regular
    /// detection mechanism will be used instead.
    /// </para>
    /// <para>
    /// X11: This function will set the <c>LC_CTYPE</c> category of the application locale according to the
    /// current environment if that category is still "C". This is because the "C" locale breaks Unicode
    /// text input.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Thread safety</b>
    /// <para>
    /// This function must only be called from the main thread.
    /// </para>
    /// </para>
    /// <para>
    /// <b>See also</b>
    /// <para>
    /// <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">Initialization and termination</see>
    /// </para>
    /// <para>
    /// <see cref="NativeGlfw.glfwInitHint"/>
    /// </para>
    /// <para>
    /// <see cref="NativeGlfw.glfwInitAllocator"/>
    /// </para>
    /// <para>
    /// <see cref="NativeGlfw.glfwTerminate"/>
    /// </para>
    /// </para>
    /// </summary>
    public static int GlfwInit() => p_glfwInit();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwTerminate();

    private static d_glfwTerminate p_glfwTerminate = LoadFunction< d_glfwTerminate >( "glfwTerminate" );

    /// <summary>
    /// This function destroys all remaining windows and cursors, restores any modified gamma
    /// ramps and frees any other allocated resources. Once this function is called, you must
    /// again call <see cref="GlfwInit"/> successfully before you will be able to use most
    /// GLFW functions.
    /// <para>
    /// If GLFW has been successfully initialized, this function should be called before the
    /// application exits. If initialization fails, there is no need to call this function, as
    /// it is called by <see cref="GlfwInit"/> before it returns failure.
    /// </para>
    /// <para>
    /// This function has no effect if GLFW is not initialized.
    /// </para>
    /// <para>
    /// <b>Errors</b>
    /// <para>
    /// Possible errors include <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Remarks</b>
    /// <para>
    /// This function may be called before <see cref="GlfwInit"/>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Warning</b>
    /// <para>
    /// The contexts of any remaining windows must not be current on any other thread when this
    /// function is called.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Reentrancy</b>
    /// <para>
    /// This function must not be called from a callback.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Thread safety</b>
    /// <para>
    /// This function must only be called from the main thread.
    /// </para>
    /// </para>
    /// <para>
    /// <b>See also</b>
    /// <para>
    /// <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">Initialization and termination</see>
    /// </para>
    /// <para>
    /// <see cref="GlfwInit"/>
    /// </para>
    /// </para>
    /// </summary>
    public static void glfwTerminate() => p_glfwTerminate();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwInitHint( int hint, int value );

    private static d_glfwInitHint p_glfwInitHint = LoadFunction< d_glfwInitHint >( "glfwInitHint" );

    /// <summary>
    /// This function sets hints for the next initialization of GLFW.
    /// <para>
    /// The values you set hints to are never reset by GLFW, but they only take effect during
    /// initialization. Once GLFW has been initialized, any values you set will be ignored until
    /// the library is terminated and initialized again.
    /// </para>
    /// <para>
    /// Some hints are platform specific. These may be set on any platform but they will only
    /// affect their specific platform. Other platforms will ignore them. Setting these hints
    /// requires no platform specific headers or functions.
    /// </para>
    /// <para>
    /// <b>Errors</b>
    /// <para>
    /// Possible errors include <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and
    /// <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Remarks</b>
    /// <para>
    /// This function may be called before <see cref="GlfwInit"/>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Thread safety</b>
    /// <para>
    /// This function must only be called from the main thread.
    /// </para>
    /// </para>
    /// <para>
    /// <b>See also</b>
    /// <para>
    /// init_hints
    /// </para>
    /// <para>
    /// <see cref="GlfwInit"/>
    /// </para>
    /// </para>
    /// </summary>
    public static void glfwInitHint( int hint, int value ) => p_glfwInitHint( hint, value );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwInitAllocator( GlfwAllocator* allocator );

    private static d_glfwInitAllocator p_glfwInitAllocator = LoadFunction< d_glfwInitAllocator >( "glfwInitAllocator" );

    /// <summary>
    /// To use the default allocator, call this function with a <c>NULL</c> argument.
    /// <para>
    /// If you specify an allocator struct, every member must be a valid function pointer. If any
    /// member is <c>NULL</c>, this function will emit <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>
    /// and the init allocator will be unchanged.
    /// </para>
    /// <para>
    /// The functions in the allocator must fulfil a number of requirements. See the documentation
    /// for <see cref="GlfwAllocatefun"/>, <see cref="GlfwReallocatefun"/> and <see cref="GlfwDeallocatefun"/>
    /// for details.
    /// </para>
    /// <para>
    /// <b>Errors</b>
    /// <para>
    /// Possible errors include <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Pointer lifetime</b>
    /// <para>
    /// The specified allocator is copied before this function returns.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#init_allocator">Custom heap memory allocator</see> </para> <para> <see cref="GlfwInit"/></para></para> <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    public static void glfwInitAllocator( GlfwAllocator* allocator ) => p_glfwInitAllocator( allocator );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwInitVulkanLoader( PFN_vkGetInstanceProcAddr loader );

    private static d_glfwInitVulkanLoader p_glfwInitVulkanLoader = LoadFunction< d_glfwInitVulkanLoader >( "glfwInitVulkanLoader" );

    /// <summary>
    /// <para>This function sets the <c>vkGetInstanceProcAddr</c> function that GLFW will use for all Vulkan related entry point queries.</para> <para>This feature is mostly useful on macOS, if your copy of the Vulkan loader is in a location where GLFW cannot find it through dynamic loading, or if you are still using the static library version of the loader.</para> <para>If set to <c>NULL</c>, GLFW will try to load the Vulkan loader dynamically by its standard name and get this function from there. This is the default behavior.</para> <para>The standard name of the loader is <c>vulkan-1.dll</c> on Windows, <c>libvulkan.so.1</c> on Linux and other Unix-like systems and <c>libvulkan.1.dylib</c> on macOS. If your code is also loading it via these names then you probably don't need to use this function.</para> <para>The function address you set is never reset by GLFW, but it only takes effect during initialization. Once GLFW has been initialized, any updates will be ignored until the library is terminated and initialized again.</para> <para><b>Loader function signature</b><para>PFN_vkVoidFunction vkGetInstanceProcAddr(VkInstance instance, const char* name) For more information about this function, see the <see href="https://www.glfw.org/docs/3.4/https://www.khronos.org/registry/vulkan/">Vulkan Registry</see>.</para></para> <para><b>Errors</b><para>None.</para></para> <para><b>Remarks</b><para>This function may be called before <see cref="GlfwInit"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/vulkan_guide.html#vulkan_loader">Finding the Vulkan loader</see> </para> <para> <see cref="GlfwInit"/></para></para> <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    public static void glfwInitVulkanLoader( PFN_vkGetInstanceProcAddr loader ) => p_glfwInitVulkanLoader( loader );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetVersion( IntPtr major, IntPtr minor, IntPtr rev );

    private static d_glfwGetVersion p_glfwGetVersion = LoadFunction< d_glfwGetVersion >( "glfwGetVersion" );

    /// <summary>
    /// <para>This function retrieves the major, minor and revision numbers of the GLFW library. It is intended for when you are using GLFW as a shared library and want to ensure that you are using the minimum required version.</para> <para>Any or all of the version arguments may be <c>NULL</c>.</para> <para><b>Errors</b><para>None.</para></para> <para><b>Remarks</b><para>This function may be called before <see cref="GlfwInit"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_version">Version management</see> </para> <para> <see cref="NativeGlfw.glfwGetVersionString"/></para></para> <para><b>Since</b><para>Added in version 1.0. </para></para>
    /// </summary>
    public static void glfwGetVersion( IntPtr major, IntPtr minor, IntPtr rev ) => p_glfwGetVersion( major, minor, rev );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetVersionString();

    private static d_glfwGetVersionString p_glfwGetVersionString = LoadFunction< d_glfwGetVersionString >( "glfwGetVersionString" );

    /// <summary>
    /// <para>This function returns the compile-time generated <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_version_string">version string</see> of the GLFW library binary. It describes the version, platforms, compiler and any platform or operating system specific compile-time options. It should not be confused with the OpenGL or OpenGL ES version string, queried with <c>glGetString</c>.</para> <para>Do not use the version string to parse the GLFW library version. The <see cref="NativeGlfw.glfwGetVersion"/> function provides the version of the running library binary in numerical format.</para> <para>Do not use the version string to parse what platforms are supported. The <see cref="NativeGlfw.glfwPlatformSupported"/> function lets you query platform support.</para> <para><b>Returns</b><para>The ASCII encoded GLFW version string.</para></para> <para><b>Errors</b><para>None.</para></para> <para><b>Remarks</b><para>This function may be called before <see cref="GlfwInit"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned string is static and compile-time generated.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_version">Version management</see> </para> <para> <see cref="NativeGlfw.glfwGetVersion"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static IntPtr glfwGetVersionString() => p_glfwGetVersionString();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetError( IntPtr description );

    private static d_glfwGetError p_glfwGetError = LoadFunction< d_glfwGetError >( "glfwGetError" );

    /// <summary>
    /// <para>This function returns and clears the <see href="https://www.glfw.org/docs/3.4/group__errors.html">error code</see> of the last error that occurred on the calling thread, and optionally a UTF-8 encoded human-readable description of it. If no error has occurred since the last call, it returns <see cref="NativeGlfw.GLFW_NO_ERROR"/> (zero) and the description pointer is set to <c>NULL</c>.</para> <para><b>Returns</b><para>The last error code for the calling thread, or <see cref="NativeGlfw.GLFW_NO_ERROR"/> (zero).</para></para> <para><b>Errors</b><para>None.</para></para> <para><b>Pointer lifetime</b><para>The returned string is allocated and freed by GLFW. You should not free it yourself. It is guaranteed to be valid only until the next error occurs or the library is terminated.</para></para> <para><b>Remarks</b><para>This function may be called before <see cref="GlfwInit"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">Error handling</see> </para> <para> <see cref="NativeGlfw.glfwSetErrorCallback"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static int glfwGetError( IntPtr description ) => p_glfwGetError( description );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetErrorCallback( IntPtr callback );

    private static d_glfwSetErrorCallback p_glfwSetErrorCallback = LoadFunction< d_glfwSetErrorCallback >( "glfwSetErrorCallback" );

    /// <summary>
    /// This function sets the error callback, which is called with an error code and a human-readable
    /// description each time a GLFW error occurs.
    /// <para>
    /// The error code is set before the callback is called. Calling <see cref="NativeGlfw.glfwGetError"/>
    /// from the error callback will return the same value as the error code argument.
    /// </para>
    /// <para>
    /// The error callback is called on the thread where the error occurred. If you are using GLFW
    /// from multiple threads, your error callback needs to be written accordingly.
    /// </para>
    /// <para>
    /// Because the description string may have been generated specifically for that error, it is
    /// not guaranteed to be valid after the callback has returned. If you wish to use it after the
    /// callback returns, you need to make a copy.
    /// </para>
    /// <para>
    /// Once set, the error callback remains set even after the library has been terminated.
    /// </para>
    /// <para>
    /// <b>Returns</b>
    /// <para>
    /// The previously set callback, or <c>NULL</c> if no callback was set.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Callback signature</b>
    /// <para>
    /// void callback_name(int error_code, const char* description) For more information about
    /// the callback parameters, see the
    /// <see href="https://www.glfw.org/docs/3.4/group__init.html#ga8184701785c096b3862a75cda1bf44a3">callback pointer type</see>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Errors</b>
    /// <para>
    /// None.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Remarks</b>
    /// <para>
    /// This function may be called before <see cref="GlfwInit"/>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Thread safety</b>
    /// <para>
    /// This function must only be called from the main thread.
    /// </para>
    /// </para>
    /// <para>
    /// <b>See also</b>
    /// <para>
    /// <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">Error handling</see>
    /// </para>
    /// <para>
    /// <see cref="NativeGlfw.glfwGetError"/>
    /// </para>
    /// </para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from
    /// being garbage collected. You must do that yourself to prevent potential crashes due to glfw
    /// calling a garbage collected delegate.
    /// </remarks>
    public static GlfwErrorfun glfwSetErrorCallback( GlfwErrorfun? callback )
    {
        System.IntPtr __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;

        System.IntPtr ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetErrorCallback( __callback_native );
        }

        System.GC.KeepAlive( __callback_native );

        GlfwErrorfun? ____callback_native_managed = null;

        if ( ____callback_native_retVal_native != default )
        {
            ____callback_native_managed = Marshal.PtrToStructure< GlfwErrorfun >( ____callback_native_retVal_native );
        }

        if ( ____callback_native_managed == null )
        {
            throw new NullReferenceException( "Failure to set Error Callback" );
        }

        return ____callback_native_managed;
    }

    internal static GlfwErrorfun CurrentGlfwErrorfun = null!;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetPlatform();

    private static d_glfwGetPlatform p_glfwGetPlatform = LoadFunction< d_glfwGetPlatform >( "glfwGetPlatform" );

    /// <summary>
    /// <para>This function returns the platform that was selected during initialization. The returned value will be one of <see cref="NativeGlfw.GLFW_PLATFORM_WIN32"/>, <see cref="NativeGlfw.GLFW_PLATFORM_COCOA"/>, <see cref="NativeGlfw.GLFW_PLATFORM_WAYLAND"/>, <see cref="NativeGlfw.GLFW_PLATFORM_X11"/> or <see cref="NativeGlfw.GLFW_PLATFORM_NULL"/>.</para> <para><b>Returns</b><para>The currently selected platform, or zero if an error occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#platform">Runtime platform selection</see> </para> <para> <see cref="NativeGlfw.glfwPlatformSupported"/></para></para> <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    public static int glfwGetPlatform() => p_glfwGetPlatform();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwPlatformSupported( int platform );

    private static d_glfwPlatformSupported p_glfwPlatformSupported = LoadFunction< d_glfwPlatformSupported >( "glfwPlatformSupported" );

    /// <summary>
    /// <para>This function returns whether the library was compiled with support for the specified platform. The platform must be one of <see cref="NativeGlfw.GLFW_PLATFORM_WIN32"/>, <see cref="NativeGlfw.GLFW_PLATFORM_COCOA"/>, <see cref="NativeGlfw.GLFW_PLATFORM_WAYLAND"/>, <see cref="NativeGlfw.GLFW_PLATFORM_X11"/> or <see cref="NativeGlfw.GLFW_PLATFORM_NULL"/>.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if the platform is supported, or <see cref="NativeGlfw.GLFW_FALSE"/> otherwise.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Remarks</b><para>This function may be called before <see cref="GlfwInit"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/intro_guide.html#platform">Runtime platform selection</see> </para> <para> <see cref="NativeGlfw.glfwGetPlatform"/></para></para> <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    public static int glfwPlatformSupported( int platform ) => p_glfwPlatformSupported( platform );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwMonitor** d_glfwGetMonitors( IntPtr count );

    private static d_glfwGetMonitors p_glfwGetMonitors = LoadFunction< d_glfwGetMonitors >( "glfwGetMonitors" );

    /// <summary>
    /// <para>This function returns an array of handles for all currently connected monitors. The primary monitor is always first in the returned array. If no monitors were found, this function returns <c>NULL</c>.</para> <para><b>Returns</b><para>An array of monitor handles, or <c>NULL</c> if no monitors were found or if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned array is allocated and freed by GLFW. You should not free it yourself. It is guaranteed to be valid only until the monitor configuration changes or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_monitors">Retrieving monitors</see> </para> <para> <see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_event">Monitor configuration changes</see> </para> <para> <see cref="NativeGlfw.glfwGetPrimaryMonitor"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static GlfwMonitor** glfwGetMonitors( IntPtr count ) => p_glfwGetMonitors( count );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwMonitor* d_glfwGetPrimaryMonitor();

    private static d_glfwGetPrimaryMonitor p_glfwGetPrimaryMonitor = LoadFunction< d_glfwGetPrimaryMonitor >( "glfwGetPrimaryMonitor" );

    /// <summary>
    /// <para>This function returns the primary monitor. This is usually the monitor where elements like the task bar or global menu bar are located.</para> <para><b>Returns</b><para>The primary monitor, or <c>NULL</c> if no monitors were found or if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>Remarks</b><para>The primary monitor is always first in the array returned by <see cref="NativeGlfw.glfwGetMonitors"/>.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_monitors">Retrieving monitors</see> </para> <para> <see cref="NativeGlfw.glfwGetMonitors"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static GlfwMonitor* glfwGetPrimaryMonitor() => p_glfwGetPrimaryMonitor();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetMonitorPos( GlfwMonitor* monitor, IntPtr xpos, IntPtr ypos );

    private static d_glfwGetMonitorPos p_glfwGetMonitorPos = LoadFunction< d_glfwGetMonitorPos >( "glfwGetMonitorPos" );

    /// <summary>
    /// <para>This function returns the position, in screen coordinates, of the upper-left corner of the specified monitor.</para> <para>Any or all of the position arguments may be <c>NULL</c>. If an error occurs, all non-<c>NULL</c> position arguments will be set to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_properties">Monitor properties</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwGetMonitorPos( GlfwMonitor* monitor, IntPtr xpos, IntPtr ypos ) => p_glfwGetMonitorPos( monitor, xpos, ypos );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetMonitorWorkarea( GlfwMonitor* monitor, IntPtr xpos, IntPtr ypos, IntPtr width, IntPtr height );

    private static d_glfwGetMonitorWorkarea p_glfwGetMonitorWorkarea = LoadFunction< d_glfwGetMonitorWorkarea >( "glfwGetMonitorWorkarea" );

    /// <summary>
    /// <para>This function returns the position, in screen coordinates, of the upper-left corner of the work area of the specified monitor along with the work area size in screen coordinates. The work area is defined as the area of the monitor not occluded by the window system task bar where present. If no task bar exists then the work area is the monitor resolution in screen coordinates.</para> <para>Any or all of the position and size arguments may be <c>NULL</c>. If an error occurs, all non-<c>NULL</c> position and size arguments will be set to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_workarea">Work area</see></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwGetMonitorWorkarea( GlfwMonitor* monitor, IntPtr xpos, IntPtr ypos, IntPtr width, IntPtr height ) => p_glfwGetMonitorWorkarea( monitor, xpos, ypos, width, height );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetMonitorPhysicalSize( GlfwMonitor* monitor, IntPtr widthMM, IntPtr heightMM );

    private static d_glfwGetMonitorPhysicalSize p_glfwGetMonitorPhysicalSize = LoadFunction< d_glfwGetMonitorPhysicalSize >( "glfwGetMonitorPhysicalSize" );

    /// <summary>
    /// <para>This function returns the size, in millimetres, of the display area of the specified monitor.</para> <para>Some platforms do not provide accurate monitor size information, either because the monitor <see href="https://www.glfw.org/docs/3.4/https://en.wikipedia.org/wiki/Extended_display_identification_data">EDID</see> data is incorrect or because the driver does not report it accurately.</para> <para>Any or all of the size arguments may be <c>NULL</c>. If an error occurs, all non-<c>NULL</c> size arguments will be set to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Remarks</b><para>Windows: On Windows 8 and earlier the physical size is calculated from the current resolution and system DPI instead of querying the monitor EDID data.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_properties">Monitor properties</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwGetMonitorPhysicalSize( GlfwMonitor* monitor, IntPtr widthMM, IntPtr heightMM ) => p_glfwGetMonitorPhysicalSize( monitor, widthMM, heightMM );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetMonitorContentScale( GlfwMonitor* monitor, IntPtr xscale, IntPtr yscale );

    private static d_glfwGetMonitorContentScale p_glfwGetMonitorContentScale = LoadFunction< d_glfwGetMonitorContentScale >( "glfwGetMonitorContentScale" );

    /// <summary>
    /// <para>This function retrieves the content scale for the specified monitor. The content scale is the ratio between the current DPI and the platform's default DPI. This is especially important for text and any UI elements. If the pixel dimensions of your UI scaled by this look appropriate on your machine then it should appear at a reasonable size on other machines regardless of their DPI and scaling settings. This relies on the system DPI and scaling settings being somewhat correct.</para> <para>The content scale may depend on both the monitor resolution and pixel density and on user settings. It may be very different from the raw DPI calculated from the physical size and current resolution.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>Wayland: Fractional scaling information is not yet available for monitors, so this function only returns integer content scales.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_scale">Content scale</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowContentScale"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwGetMonitorContentScale( GlfwMonitor* monitor, IntPtr xscale, IntPtr yscale ) => p_glfwGetMonitorContentScale( monitor, xscale, yscale );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetMonitorName( GlfwMonitor* monitor );

    private static d_glfwGetMonitorName p_glfwGetMonitorName = LoadFunction< d_glfwGetMonitorName >( "glfwGetMonitorName" );

    /// <summary>
    /// <para>This function returns a human-readable name, encoded as UTF-8, of the specified monitor. The name typically reflects the make and model of the monitor and is not guaranteed to be unique among the connected monitors.</para> <para><b>Returns</b><para>The UTF-8 encoded name of the monitor, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned string is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified monitor is disconnected or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_properties">Monitor properties</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static IntPtr glfwGetMonitorName( GlfwMonitor* monitor ) => p_glfwGetMonitorName( monitor );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetMonitorUserPointer( GlfwMonitor* monitor, IntPtr pointer );

    private static d_glfwSetMonitorUserPointer p_glfwSetMonitorUserPointer = LoadFunction< d_glfwSetMonitorUserPointer >( "glfwSetMonitorUserPointer" );

    /// <summary>
    /// <para>This function sets the user-defined pointer of the specified monitor. The current value is retained until the monitor is disconnected. The initial value is <c>NULL</c>.</para> <para>This function may be called from the monitor callback, even for a monitor that is being disconnected.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Access is not synchronized.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_userptr">User pointer</see> </para> <para> <see cref="NativeGlfw.glfwGetMonitorUserPointer"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwSetMonitorUserPointer( GlfwMonitor* monitor, IntPtr pointer ) => p_glfwSetMonitorUserPointer( monitor, pointer );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetMonitorUserPointer( GlfwMonitor* monitor );

    private static d_glfwGetMonitorUserPointer p_glfwGetMonitorUserPointer = LoadFunction< d_glfwGetMonitorUserPointer >( "glfwGetMonitorUserPointer" );

    /// <summary>
    /// <para>This function returns the current value of the user-defined pointer of the specified monitor. The initial value is <c>NULL</c>.</para> <para>This function may be called from the monitor callback, even for a monitor that is being disconnected.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Access is not synchronized.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_userptr">User pointer</see> </para> <para> <see cref="NativeGlfw.glfwSetMonitorUserPointer"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static IntPtr glfwGetMonitorUserPointer( GlfwMonitor* monitor ) => p_glfwGetMonitorUserPointer( monitor );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetMonitorCallback( IntPtr callback );

    private static d_glfwSetMonitorCallback p_glfwSetMonitorCallback = LoadFunction< d_glfwSetMonitorCallback >( "glfwSetMonitorCallback" );

    // ========================================================================

    /// <summary>
    /// <para>This function sets the monitor configuration callback, or removes the currently set callback. This is called when a monitor is connected to or disconnected from the system.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwMonitor"/>* monitor, int event) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__monitor.html#gaabe16caca8dea952504dfdebdf4cd249">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_event">Monitor configuration changes</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwMonitorfun glfwSetMonitorCallback( GlfwMonitorfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwMonitorfun ____callback_native_managed;
        System.IntPtr  ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetMonitorCallback( __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwMonitorfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwMonitorfun CurrentGlfwMonitorfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwVideoMode* d_glfwGetVideoModes( GlfwMonitor* monitor, IntPtr count );

    private static d_glfwGetVideoModes p_glfwGetVideoModes = LoadFunction< d_glfwGetVideoModes >( "glfwGetVideoModes" );

    /// <summary>
    /// <para>This function returns an array of all video modes supported by the specified monitor. The returned array is sorted in ascending order, first by color bit depth (the sum of all channel depths), then by resolution area (the product of width and height), then resolution width and finally by refresh rate.</para> <para><b>Returns</b><para>An array of video modes, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned array is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified monitor is disconnected, this function is called again for that monitor or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_modes">Video modes</see> </para> <para> <see cref="NativeGlfw.glfwGetVideoMode"/></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Changed to return an array of modes for a specific monitor. </para></para>
    /// </summary>
    public static GlfwVideoMode* glfwGetVideoModes( GlfwMonitor* monitor, IntPtr count ) => p_glfwGetVideoModes( monitor, count );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwVideoMode* d_glfwGetVideoMode( GlfwMonitor* monitor );

    private static d_glfwGetVideoMode p_glfwGetVideoMode = LoadFunction< d_glfwGetVideoMode >( "glfwGetVideoMode" );

    /// <summary>
    /// <para>This function returns the current video mode of the specified monitor. If you have created a full screen window for that monitor, the return value will depend on whether that window is iconified.</para> <para><b>Returns</b><para>The current mode of the monitor, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned array is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified monitor is disconnected or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_modes">Video modes</see> </para> <para> <see cref="NativeGlfw.glfwGetVideoModes"/></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwGetDesktopMode</c>. </para></para>
    /// </summary>
    public static GlfwVideoMode* glfwGetVideoMode( GlfwMonitor* monitor ) => p_glfwGetVideoMode( monitor );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetGamma( GlfwMonitor* monitor, float gamma );

    private static d_glfwSetGamma p_glfwSetGamma = LoadFunction< d_glfwSetGamma >( "glfwSetGamma" );

    /// <summary>
    /// <para>This function generates an appropriately sized gamma ramp from the specified exponent and then calls <see cref="NativeGlfw.glfwSetGammaRamp"/> with it. The value must be a finite number greater than zero.</para> <para>The software controlled gamma ramp is applied in addition to the hardware gamma correction, which today is usually an approximation of sRGB gamma. This means that setting a perfectly linear ramp, or gamma 1.0, will produce the default (usually sRGB-like) behavior.</para> <para>For gamma correct rendering with OpenGL or OpenGL ES, see the <see cref="NativeGlfw.GLFW_SRGB_CAPABLE"/> hint.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Remarks</b><para>Wayland: Gamma handling is a privileged protocol, this function will thus never be implemented and emits <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_gamma">Gamma ramp</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwSetGamma( GlfwMonitor* monitor, float gamma ) => p_glfwSetGamma( monitor, gamma );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwGammaRamp* d_glfwGetGammaRamp( GlfwMonitor* monitor );

    private static d_glfwGetGammaRamp p_glfwGetGammaRamp = LoadFunction< d_glfwGetGammaRamp >( "glfwGetGammaRamp" );

    /// <summary>
    /// <para>This function returns the current gamma ramp of the specified monitor.</para> <para><b>Returns</b><para>The current gamma ramp, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Remarks</b><para>Wayland: Gamma handling is a privileged protocol, this function will thus never be implemented and emits <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> while returning <c>NULL</c>.</para></para> <para><b>Pointer lifetime</b><para>The returned structure and its arrays are allocated and freed by GLFW. You should not free them yourself. They are valid until the specified monitor is disconnected, this function is called again for that monitor or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_gamma">Gamma ramp</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static GlfwGammaRamp* glfwGetGammaRamp( GlfwMonitor* monitor ) => p_glfwGetGammaRamp( monitor );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetGammaRamp( GlfwMonitor* monitor, GlfwGammaRamp* ramp );

    private static d_glfwSetGammaRamp p_glfwSetGammaRamp = LoadFunction< d_glfwSetGammaRamp >( "glfwSetGammaRamp" );

    /// <summary>
    /// <para>This function sets the current gamma ramp for the specified monitor. The original gamma ramp for that monitor is saved by GLFW the first time this function is called and is restored by <see cref="NativeGlfw.glfwTerminate"/>.</para> <para>The software controlled gamma ramp is applied in addition to the hardware gamma correction, which today is usually an approximation of sRGB gamma. This means that setting a perfectly linear ramp, or gamma 1.0, will produce the default (usually sRGB-like) behavior.</para> <para>For gamma correct rendering with OpenGL or OpenGL ES, see the <see cref="NativeGlfw.GLFW_SRGB_CAPABLE"/> hint.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Remarks</b><para>The size of the specified gamma ramp should match the size of the current ramp for that monitor.</para> <para> Windows: The gamma ramp size must be 256.</para> <para> Wayland: Gamma handling is a privileged protocol, this function will thus never be implemented and emits <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>.</para></para> <para><b>Pointer lifetime</b><para>The specified gamma ramp is copied before this function returns.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_gamma">Gamma ramp</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwSetGammaRamp( GlfwMonitor* monitor, GlfwGammaRamp* ramp ) => p_glfwSetGammaRamp( monitor, ramp );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwDefaultWindowHints();

    private static d_glfwDefaultWindowHints p_glfwDefaultWindowHints = LoadFunction< d_glfwDefaultWindowHints >( "glfwDefaultWindowHints" );

    /// <summary>
    /// <para>This function resets all window hints to their <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hints_values">default values</see>.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hints">Window creation hints</see> </para> <para> <see cref="NativeGlfw.glfwWindowHint"/> </para> <para> <see cref="NativeGlfw.glfwWindowHintString"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwDefaultWindowHints() => p_glfwDefaultWindowHints();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwWindowHint( int hint, int value );

    private static d_glfwWindowHint p_glfwWindowHint = LoadFunction< d_glfwWindowHint >( "glfwWindowHint" );

    /// <summary>
    /// <para>This function sets hints for the next call to <see cref="NativeGlfw.glfwCreateWindow"/>. The hints, once set, retain their values until changed by a call to this function or <see cref="NativeGlfw.glfwDefaultWindowHints"/>, or until the library is terminated.</para> <para>Only integer value hints can be set with this function. String value hints are set with <see cref="NativeGlfw.glfwWindowHintString"/>.</para> <para>This function does not check whether the specified hint values are valid. If you set hints to invalid values this will instead be reported by the next call to <see cref="NativeGlfw.glfwCreateWindow"/>.</para> <para>Some hints are platform specific. These may be set on any platform but they will only affect their specific platform. Other platforms will ignore them. Setting these hints requires no platform specific headers or functions.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hints">Window creation hints</see> </para> <para> <see cref="NativeGlfw.glfwWindowHintString"/> </para> <para> <see cref="NativeGlfw.glfwDefaultWindowHints"/></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwOpenWindowHint</c>. </para></para>
    /// </summary>
    public static void glfwWindowHint( int hint, int value ) => p_glfwWindowHint( hint, value );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwWindowHintString( int hint, IntPtr value );

    private static d_glfwWindowHintString p_glfwWindowHintString = LoadFunction< d_glfwWindowHintString >( "glfwWindowHintString" );

    /// <summary>
    /// <para>This function sets hints for the next call to <see cref="NativeGlfw.glfwCreateWindow"/>. The hints, once set, retain their values until changed by a call to this function or <see cref="NativeGlfw.glfwDefaultWindowHints"/>, or until the library is terminated.</para> <para>Only string type hints can be set with this function. Integer value hints are set with <see cref="NativeGlfw.glfwWindowHint"/>.</para> <para>This function does not check whether the specified hint values are valid. If you set hints to invalid values this will instead be reported by the next call to <see cref="NativeGlfw.glfwCreateWindow"/>.</para> <para>Some hints are platform specific. These may be set on any platform but they will only affect their specific platform. Other platforms will ignore them. Setting these hints requires no platform specific headers or functions.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Pointer lifetime</b><para>The specified string is copied before this function returns.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hints">Window creation hints</see> </para> <para> <see cref="NativeGlfw.glfwWindowHint"/> </para> <para> <see cref="NativeGlfw.glfwDefaultWindowHints"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwWindowHintString( int hint, IntPtr value ) => p_glfwWindowHintString( hint, value );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwWindow* d_glfwCreateWindow( int width, int height, IntPtr title, GlfwMonitor* monitor, GlfwWindow* share );

    private static d_glfwCreateWindow p_glfwCreateWindow = LoadFunction< d_glfwCreateWindow >( "glfwCreateWindow" );

    /// <summary>
    /// <para>This function creates a window and its associated OpenGL or OpenGL ES context. Most of the options controlling how the window and its context should be created are specified with <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hints">window hints</see>.</para> <para>Successful creation does not change which context is current. Before you can use the newly created context, you need to <see href="https://www.glfw.org/docs/3.4/context_guide.html#context_current">make it current</see>. For information about the <c>share</c> parameter, see <see href="https://www.glfw.org/docs/3.4/context_guide.html#context_sharing">Context object sharing</see>.</para> <para>The created window, framebuffer and context may differ from what you requested, as not all parameters and hints are <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hints_hard">hard constraints</see>. This includes the size of the window, especially for full screen windows. To query the actual attributes of the created window, framebuffer and context, see <see cref="NativeGlfw.glfwGetWindowAttrib"/>, <see cref="NativeGlfw.glfwGetWindowSize"/> and <see cref="NativeGlfw.glfwGetFramebufferSize"/>.</para> <para>To create a full screen window, you need to specify the monitor the window will cover. If no monitor is specified, the window will be windowed mode. Unless you have a way for the user to choose a specific monitor, it is recommended that you pick the primary monitor. For more information on how to query connected monitors, see <see href="https://www.glfw.org/docs/3.4/monitor_guide.html#monitor_monitors">Retrieving monitors</see>.</para> <para>For full screen windows, the specified size becomes the resolution of the window's desired video mode. As long as a full screen window is not iconified, the supported video mode most closely matching the desired video mode is set for the specified monitor. For more information about full screen windows, including the creation of so called windowed full screen or borderless full screen windows, see <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_windowed_full_screen">"Windowed full screen" windows</see>.</para> <para>Once you have created the window, you can switch it between windowed and full screen mode with <see cref="NativeGlfw.glfwSetWindowMonitor"/>. This will not affect its OpenGL or OpenGL ES context.</para> <para>By default, newly created windows use the placement recommended by the window system. To create the window at a specific position, set the <see cref="NativeGlfw.GLFW_POSITION_X"/> and <see cref="NativeGlfw.GLFW_POSITION_Y"/> window hints before creation. To restore the default behavior, set either or both hints back to <see cref="NativeGlfw.GLFW_ANY_POSITION"/>.</para> <para>As long as at least one full screen window is not iconified, the screensaver is prohibited from starting.</para> <para>Window systems put limits on window sizes. Very large or very small window dimensions may be overridden by the window system on creation. Check the actual <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_size">size</see> after creation.</para> <para>The <see href="https://www.glfw.org/docs/3.4/window_guide.html#buffer_swap">swap interval</see> is not set during window creation and the initial value may vary depending on driver settings and defaults.</para> <para><b>Returns</b><para>The handle of the created window, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>, <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/>, <see cref="NativeGlfw.GLFW_VERSION_UNAVAILABLE"/>, <see cref="NativeGlfw.GLFW_FORMAT_UNAVAILABLE"/>, <see cref="NativeGlfw.GLFW_NO_WINDOW_CONTEXT"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>Windows: Window creation will fail if the Microsoft GDI software OpenGL implementation is the only one available.</para> <para> Windows: If the executable has an icon resource named <c>GLFW_ICON</c> it will be set as the initial icon for the window. If no such icon is present, the <c>IDI_APPLICATION</c> icon will be used instead. To set a different icon, see <see cref="NativeGlfw.glfwSetWindowIcon"/>.</para> <para> Windows: The context to share resources with must not be current on any other thread.</para> <para> macOS: The OS only supports core profile contexts for OpenGL versions 3.2 and later. Before creating an OpenGL context of version 3.2 or later you must set the <see cref="NativeGlfw.GLFW_OPENGL_PROFILE"/> hint accordingly. OpenGL 3.0 and 3.1 contexts are not supported at all on macOS.</para> <para> macOS: The GLFW window has no icon, as it is not a document window, but the dock icon will be the same as the application bundle's icon. For more information on bundles, see the <see href="https://www.glfw.org/docs/3.4/https://developer.apple.com/library/mac/documentation/CoreFoundation/Conceptual/CFBundles/">Bundle Programming Guide</see> in the Mac Developer Library.</para> <para> macOS: On OS X 10.10 and later the window frame will not be rendered at full resolution on Retina displays unless the <see cref="NativeGlfw.GLFW_SCALE_FRAMEBUFFER"/> hint is <see cref="NativeGlfw.GLFW_TRUE"/> and the <c>NSHighResolutionCapable</c> key is enabled in the application bundle's <c>Info.plist</c>. For more information, see <see href="https://www.glfw.org/docs/3.4/https://developer.apple.com/library/mac/documentation/GraphicsAnimation/Conceptual/HighResolutionOSX/Explained/Explained.html">High Resolution Guidelines for OS X</see> in the Mac Developer Library. The GLFW test and example programs use a custom <c>Info.plist</c> template for this, which can be found as <c>CMake/Info.plist.in</c> in the source tree.</para> <para> macOS: When activating frame autosaving with <see cref="NativeGlfw.GLFW_COCOA_FRAME_NAME"/>, the specified window size and position may be overridden by previously saved values.</para> <para> Wayland: GLFW uses <see href="https://www.glfw.org/docs/3.4/https://gitlab.freedesktop.org/libdecor/libdecor">libdecor</see> where available to create its window decorations. This in turn uses server-side XDG decorations where available and provides high quality client-side decorations on compositors like GNOME. If both XDG decorations and libdecor are unavailable, GLFW falls back to a very simple set of window decorations that only support moving, resizing and the window manager's right-click menu.</para> <para> X11: Some window managers will not respect the placement of initially hidden windows.</para> <para> X11: Due to the asynchronous nature of X11, it may take a moment for a window to reach its requested state. This means you may not be able to query the final size, position or other attributes directly after window creation.</para> <para> X11: The class part of the <c>WM_CLASS</c> window property will by default be set to the window title passed to this function. The instance part will use the contents of the <c>RESOURCE_NAME</c> environment variable, if present and not empty, or fall back to the window title. Set the <see cref="NativeGlfw.GLFW_X11_CLASS_NAME"/> and <see cref="NativeGlfw.GLFW_X11_INSTANCE_NAME"/> window hints to override this.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_creation">Window creation</see> </para> <para> <see cref="NativeGlfw.glfwDestroyWindow"/></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwOpenWindow</c>. </para></para>
    /// </summary>
    public static GlfwWindow* glfwCreateWindow( int width, int height, IntPtr title, GlfwMonitor* monitor, GlfwWindow* share ) => p_glfwCreateWindow( width, height, title, monitor, share );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwDestroyWindow( GlfwWindow* window );

    private static d_glfwDestroyWindow p_glfwDestroyWindow = LoadFunction< d_glfwDestroyWindow >( "glfwDestroyWindow" );

    /// <summary>
    /// <para>This function destroys the specified window and its context. On calling this function, no further callbacks will be called for that window.</para> <para>If the context of the specified window is current on the main thread, it is detached before being destroyed.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Note</b><para>The context of the specified window must not be current on any other thread when this function is called.</para></para> <para><b>Reentrancy</b><para>This function must not be called from a callback.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_creation">Window creation</see> </para> <para> <see cref="NativeGlfw.glfwCreateWindow"/></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwCloseWindow</c>. </para></para>
    /// </summary>
    public static void glfwDestroyWindow( GlfwWindow* window ) => p_glfwDestroyWindow( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwWindowShouldClose( GlfwWindow* window );

    private static d_glfwWindowShouldClose p_glfwWindowShouldClose = LoadFunction< d_glfwWindowShouldClose >( "glfwWindowShouldClose" );

    /// <summary>
    /// <para>This function returns the value of the close flag of the specified window.</para> <para><b>Returns</b><para>The value of the close flag.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Access is not synchronized.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_close">Window closing and close flag</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static int glfwWindowShouldClose( GlfwWindow* window ) => p_glfwWindowShouldClose( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowShouldClose( GlfwWindow* window, int value );

    private static d_glfwSetWindowShouldClose p_glfwSetWindowShouldClose = LoadFunction< d_glfwSetWindowShouldClose >( "glfwSetWindowShouldClose" );

    /// <summary>
    /// <para>This function sets the value of the close flag of the specified window. This can be used to override the user's attempt to close the window, or to signal that it should be closed.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Access is not synchronized.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_close">Window closing and close flag</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwSetWindowShouldClose( GlfwWindow* window, int value ) => p_glfwSetWindowShouldClose( window, value );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetWindowTitle( GlfwWindow* window );

    private static d_glfwGetWindowTitle p_glfwGetWindowTitle = LoadFunction< d_glfwGetWindowTitle >( "glfwGetWindowTitle" );

    /// <summary>
    /// <para>This function returns the window title, encoded as UTF-8, of the specified window. This is the title set previously by <see cref="NativeGlfw.glfwCreateWindow"/> or <see cref="NativeGlfw.glfwSetWindowTitle"/>.</para> <para><b>Returns</b><para>The UTF-8 encoded window title, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Remarks</b><para>The returned title is currently a copy of the title last set by <see cref="NativeGlfw.glfwCreateWindow"/> or <see cref="NativeGlfw.glfwSetWindowTitle"/>. It does not include any additional text which may be appended by the platform or another program.</para></para> <para><b>Pointer lifetime</b><para>The returned string is allocated and freed by GLFW. You should not free it yourself. It is valid until the next call to <see cref="NativeGlfw.glfwGetWindowTitle"/> or <see cref="NativeGlfw.glfwSetWindowTitle"/>, or until the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_title">Window title</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowTitle"/></para></para> <para><b>Since</b><para>Added in version 3.4. </para></para>
    /// </summary>
    public static IntPtr glfwGetWindowTitle( GlfwWindow* window ) => p_glfwGetWindowTitle( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowTitle( GlfwWindow* window, IntPtr title );

    private static d_glfwSetWindowTitle p_glfwSetWindowTitle = LoadFunction< d_glfwSetWindowTitle >( "glfwSetWindowTitle" );

    /// <summary>
    /// <para>This function sets the window title, encoded as UTF-8, of the specified window.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>macOS: The window title will not be updated until the next time you process events.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_title">Window title</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowTitle"/></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static void glfwSetWindowTitle( GlfwWindow* window, IntPtr title ) => p_glfwSetWindowTitle( window, title );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowIcon( GlfwWindow* window, int count, GlfwImage* images );

    private static d_glfwSetWindowIcon p_glfwSetWindowIcon = LoadFunction< d_glfwSetWindowIcon >( "glfwSetWindowIcon" );

    /// <summary>
    /// <para>This function sets the icon of the specified window. If passed an array of candidate images, those of or closest to the sizes desired by the system are selected. If no images are specified, the window reverts to its default icon.</para> <para>The pixels are 32-bit, little-endian, non-premultiplied RGBA, i.e. eight bits per channel with the red channel first. They are arranged canonically as packed sequential rows, starting from the top-left corner.</para> <para>The desired image sizes varies depending on platform and system settings. The selected images will be rescaled as needed. Good sizes include 16x16, 32x32 and 48x48.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Pointer lifetime</b><para>The specified image data is copied before this function returns.</para></para> <para><b>Remarks</b><para>macOS: Regular windows do not have icons on macOS. This function will emit <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>. The dock icon will be the same as the application bundle's icon. For more information on bundles, see the <see href="https://www.glfw.org/docs/3.4/https://developer.apple.com/library/mac/documentation/CoreFoundation/Conceptual/CFBundles/">Bundle Programming Guide</see> in the Mac Developer Library.</para> <para> Wayland: There is no existing protocol to change an icon, the window will thus inherit the one defined in the application's desktop file. This function will emit <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_icon">Window icon</see></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static void glfwSetWindowIcon( GlfwWindow* window, int count, GlfwImage* images ) => p_glfwSetWindowIcon( window, count, images );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetWindowPos( GlfwWindow* window, IntPtr xpos, IntPtr ypos );

    private static d_glfwGetWindowPos p_glfwGetWindowPos = LoadFunction< d_glfwGetWindowPos >( "glfwGetWindowPos" );

    /// <summary>
    /// <para>This function retrieves the position, in screen coordinates, of the upper-left corner of the content area of the specified window.</para> <para>Any or all of the position arguments may be <c>NULL</c>. If an error occurs, all non-<c>NULL</c> position arguments will be set to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Remarks</b><para>Wayland: There is no way for an application to retrieve the global position of its windows. This function will emit <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_pos">Window position</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowPos"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwGetWindowPos( GlfwWindow* window, IntPtr xpos, IntPtr ypos ) => p_glfwGetWindowPos( window, xpos, ypos );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowPos( GlfwWindow* window, int xpos, int ypos );

    private static d_glfwSetWindowPos p_glfwSetWindowPos = LoadFunction< d_glfwSetWindowPos >( "glfwSetWindowPos" );

    /// <summary>
    /// <para>This function sets the position, in screen coordinates, of the upper-left corner of the content area of the specified windowed mode window. If the window is a full screen window, this function does nothing.</para> <para>Do not use this function to move an already visible window unless you have very good reasons for doing so, as it will confuse and annoy the user.</para> <para>The window manager may put limits on what positions are allowed. GLFW cannot and should not override these limits.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Remarks</b><para>Wayland: There is no way for an application to set the global position of its windows. This function will emit <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_pos">Window position</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowPos"/></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static void glfwSetWindowPos( GlfwWindow* window, int xpos, int ypos ) => p_glfwSetWindowPos( window, xpos, ypos );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetWindowSize( GlfwWindow* window, IntPtr width, IntPtr height );

    private static d_glfwGetWindowSize p_glfwGetWindowSize = LoadFunction< d_glfwGetWindowSize >( "glfwGetWindowSize" );

    /// <summary>
    /// <para>This function retrieves the size, in screen coordinates, of the content area of the specified window. If you wish to retrieve the size of the framebuffer of the window in pixels, see <see cref="NativeGlfw.glfwGetFramebufferSize"/>.</para> <para>Any or all of the size arguments may be <c>NULL</c>. If an error occurs, all non-<c>NULL</c> size arguments will be set to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_size">Window size</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowSize"/></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static void glfwGetWindowSize( GlfwWindow* window, IntPtr width, IntPtr height ) => p_glfwGetWindowSize( window, width, height );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowSizeLimits( GlfwWindow* window, int minwidth, int minheight, int maxwidth, int maxheight );

    private static d_glfwSetWindowSizeLimits p_glfwSetWindowSizeLimits = LoadFunction< d_glfwSetWindowSizeLimits >( "glfwSetWindowSizeLimits" );

    /// <summary>
    /// <para>This function sets the size limits of the content area of the specified window. If the window is full screen, the size limits only take effect once it is made windowed. If the window is not resizable, this function does nothing.</para> <para>The size limits are applied immediately to a windowed mode window and may cause it to be resized.</para> <para>The maximum dimensions must be greater than or equal to the minimum dimensions and all must be greater than or equal to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>If you set size limits and an aspect ratio that conflict, the results are undefined.</para> <para> Wayland: The size limits will not be applied until the window is actually resized, either by the user or by the compositor.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_sizelimits">Window size limits</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowAspectRatio"/></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static void glfwSetWindowSizeLimits( GlfwWindow* window, int minwidth, int minheight, int maxwidth, int maxheight ) => p_glfwSetWindowSizeLimits( window, minwidth, minheight, maxwidth, maxheight );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowAspectRatio( GlfwWindow* window, int numer, int denom );

    private static d_glfwSetWindowAspectRatio p_glfwSetWindowAspectRatio = LoadFunction< d_glfwSetWindowAspectRatio >( "glfwSetWindowAspectRatio" );

    /// <summary>
    /// <para>This function sets the required aspect ratio of the content area of the specified window. If the window is full screen, the aspect ratio only takes effect once it is made windowed. If the window is not resizable, this function does nothing.</para> <para>The aspect ratio is specified as a numerator and a denominator and both values must be greater than zero. For example, the common 16:9 aspect ratio is specified as 16 and 9, respectively.</para> <para>If the numerator and denominator is set to <see cref="NativeGlfw.GLFW_DONT_CARE"/> then the aspect ratio limit is disabled.</para> <para>The aspect ratio is applied immediately to a windowed mode window and may cause it to be resized.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>If you set size limits and an aspect ratio that conflict, the results are undefined.</para> <para> Wayland: The aspect ratio will not be applied until the window is actually resized, either by the user or by the compositor.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_sizelimits">Window size limits</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowSizeLimits"/></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static void glfwSetWindowAspectRatio( GlfwWindow* window, int numer, int denom ) => p_glfwSetWindowAspectRatio( window, numer, denom );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowSize( GlfwWindow* window, int width, int height );

    private static d_glfwSetWindowSize p_glfwSetWindowSize = LoadFunction< d_glfwSetWindowSize >( "glfwSetWindowSize" );

    /// <summary>
    /// <para>This function sets the size, in screen coordinates, of the content area of the specified window.</para> <para>For full screen windows, this function updates the resolution of its desired video mode and switches to the video mode closest to it, without affecting the window's context. As the context is unaffected, the bit depths of the framebuffer remain unchanged.</para> <para>If you wish to update the refresh rate of the desired video mode in addition to its resolution, see <see cref="NativeGlfw.glfwSetWindowMonitor"/>.</para> <para>The window manager may put limits on what sizes are allowed. GLFW cannot and should not override these limits.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_size">Window size</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowSize"/> </para> <para> <see cref="NativeGlfw.glfwSetWindowMonitor"/></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static void glfwSetWindowSize( GlfwWindow* window, int width, int height ) => p_glfwSetWindowSize( window, width, height );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetFramebufferSize( GlfwWindow* window, IntPtr width, IntPtr height );

    private static d_glfwGetFramebufferSize p_glfwGetFramebufferSize = LoadFunction< d_glfwGetFramebufferSize >( "glfwGetFramebufferSize" );

    /// <summary>
    /// <para>This function retrieves the size, in pixels, of the framebuffer of the specified window. If you wish to retrieve the size of the window in screen coordinates, see <see cref="NativeGlfw.glfwGetWindowSize"/>.</para> <para>Any or all of the size arguments may be <c>NULL</c>. If an error occurs, all non-<c>NULL</c> size arguments will be set to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_fbsize">Framebuffer size</see> </para> <para> <see cref="NativeGlfw.glfwSetFramebufferSizeCallback"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwGetFramebufferSize( GlfwWindow* window, IntPtr width, IntPtr height ) => p_glfwGetFramebufferSize( window, width, height );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetWindowFrameSize( GlfwWindow* window, IntPtr left, IntPtr top, IntPtr right, IntPtr bottom );

    private static d_glfwGetWindowFrameSize p_glfwGetWindowFrameSize = LoadFunction< d_glfwGetWindowFrameSize >( "glfwGetWindowFrameSize" );

    /// <summary>
    /// <para>This function retrieves the size, in screen coordinates, of each edge of the frame of the specified window. This size includes the title bar, if the window has one. The size of the frame may vary depending on the <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hints_wnd">window-related hints</see> used to create it.</para> <para>Because this function retrieves the size of each window frame edge and not the offset along a particular coordinate axis, the retrieved values will always be zero or positive.</para> <para>Any or all of the size arguments may be <c>NULL</c>. If an error occurs, all non-<c>NULL</c> size arguments will be set to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_size">Window size</see></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    public static void glfwGetWindowFrameSize( GlfwWindow* window, IntPtr left, IntPtr top, IntPtr right, IntPtr bottom ) => p_glfwGetWindowFrameSize( window, left, top, right, bottom );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetWindowContentScale( GlfwWindow* window, IntPtr xscale, IntPtr yscale );

    private static d_glfwGetWindowContentScale p_glfwGetWindowContentScale = LoadFunction< d_glfwGetWindowContentScale >( "glfwGetWindowContentScale" );

    /// <summary>
    /// <para>This function retrieves the content scale for the specified window. The content scale is the ratio between the current DPI and the platform's default DPI. This is especially important for text and any UI elements. If the pixel dimensions of your UI scaled by this look appropriate on your machine then it should appear at a reasonable size on other machines regardless of their DPI and scaling settings. This relies on the system DPI and scaling settings being somewhat correct.</para> <para>On platforms where each monitors can have its own content scale, the window content scale will depend on which monitor the system considers the window to be on.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_scale">Window content scale</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowContentScaleCallback"/> </para> <para> <see cref="NativeGlfw.glfwGetMonitorContentScale"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwGetWindowContentScale( GlfwWindow* window, IntPtr xscale, IntPtr yscale ) => p_glfwGetWindowContentScale( window, xscale, yscale );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate float d_glfwGetWindowOpacity( GlfwWindow* window );

    private static d_glfwGetWindowOpacity p_glfwGetWindowOpacity = LoadFunction< d_glfwGetWindowOpacity >( "glfwGetWindowOpacity" );

    /// <summary>
    /// <para>This function returns the opacity of the window, including any decorations.</para> <para>The opacity (or alpha) value is a positive finite number between zero and one, where zero is fully transparent and one is fully opaque. If the system does not support whole window transparency, this function always returns one.</para> <para>The initial opacity value for newly created windows is one.</para> <para><b>Returns</b><para>The opacity value of the specified window.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_transparency">Window transparency</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowOpacity"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static float glfwGetWindowOpacity( GlfwWindow* window ) => p_glfwGetWindowOpacity( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowOpacity( GlfwWindow* window, float opacity );

    private static d_glfwSetWindowOpacity p_glfwSetWindowOpacity = LoadFunction< d_glfwSetWindowOpacity >( "glfwSetWindowOpacity" );

    /// <summary>
    /// <para>This function sets the opacity of the window, including any decorations.</para> <para>The opacity (or alpha) value is a positive finite number between zero and one, where zero is fully transparent and one is fully opaque.</para> <para>The initial opacity value for newly created windows is one.</para> <para>A window created with framebuffer transparency may not use whole window transparency. The results of doing this are undefined.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Remarks</b><para>Wayland: There is no way to set an opacity factor for a window. This function will emit <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_transparency">Window transparency</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowOpacity"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwSetWindowOpacity( GlfwWindow* window, float opacity ) => p_glfwSetWindowOpacity( window, opacity );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwIconifyWindow( GlfwWindow* window );

    private static d_glfwIconifyWindow p_glfwIconifyWindow = LoadFunction< d_glfwIconifyWindow >( "glfwIconifyWindow" );

    /// <summary>
    /// <para>This function iconifies (minimizes) the specified window if it was previously restored. If the window is already iconified, this function does nothing.</para> <para>If the specified window is a full screen window, GLFW restores the original video mode of the monitor. The window's desired video mode is set again when the window is restored.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>Wayland: Once a window is iconified, <see cref="NativeGlfw.glfwRestoreWindow"/> won’t be able to restore it. This is a design decision of the xdg-shell protocol.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_iconify">Window iconification</see> </para> <para> <see cref="NativeGlfw.glfwRestoreWindow"/> </para> <para> <see cref="NativeGlfw.glfwMaximizeWindow"/></para></para> <para><b>Since</b><para>Added in version 2.1. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static void glfwIconifyWindow( GlfwWindow* window ) => p_glfwIconifyWindow( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwRestoreWindow( GlfwWindow* window );

    private static d_glfwRestoreWindow p_glfwRestoreWindow = LoadFunction< d_glfwRestoreWindow >( "glfwRestoreWindow" );

    /// <summary>
    /// <para>This function restores the specified window if it was previously iconified (minimized) or maximized. If the window is already restored, this function does nothing.</para> <para>If the specified window is an iconified full screen window, its desired video mode is set again for its monitor when the window is restored.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_iconify">Window iconification</see> </para> <para> <see cref="NativeGlfw.glfwIconifyWindow"/> </para> <para> <see cref="NativeGlfw.glfwMaximizeWindow"/></para></para> <para><b>Since</b><para>Added in version 2.1. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static void glfwRestoreWindow( GlfwWindow* window ) => p_glfwRestoreWindow( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwMaximizeWindow( GlfwWindow* window );

    private static d_glfwMaximizeWindow p_glfwMaximizeWindow = LoadFunction< d_glfwMaximizeWindow >( "glfwMaximizeWindow" );

    /// <summary>
    /// <para>This function maximizes the specified window if it was previously not maximized. If the window is already maximized, this function does nothing.</para> <para>If the specified window is a full screen window, this function does nothing.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread Safety</b><para>This function may only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_iconify">Window iconification</see> </para> <para> <see cref="NativeGlfw.glfwIconifyWindow"/> </para> <para> <see cref="NativeGlfw.glfwRestoreWindow"/></para></para> <para><b>Since</b><para>Added in GLFW 3.2. </para></para>
    /// </summary>
    public static void glfwMaximizeWindow( GlfwWindow* window ) => p_glfwMaximizeWindow( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwShowWindow( GlfwWindow* window );

    private static d_glfwShowWindow p_glfwShowWindow = LoadFunction< d_glfwShowWindow >( "glfwShowWindow" );

    /// <summary>
    /// <para>This function makes the specified window visible if it was previously hidden. If the window is already visible or is in full screen mode, this function does nothing.</para> <para>By default, windowed mode windows are focused when shown Set the <see cref="NativeGlfw.GLFW_FOCUS_ON_SHOW"/> window hint to change this behavior for all newly created windows, or change the behavior for an existing window with <see cref="NativeGlfw.glfwSetWindowAttrib"/>.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>Wayland: Because Wayland wants every frame of the desktop to be complete, this function does not immediately make the window visible. Instead it will become visible the next time the window framebuffer is updated after this call.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hide">Window visibility</see> </para> <para> <see cref="NativeGlfw.glfwHideWindow"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwShowWindow( GlfwWindow* window ) => p_glfwShowWindow( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwHideWindow( GlfwWindow* window );

    private static d_glfwHideWindow p_glfwHideWindow = LoadFunction< d_glfwHideWindow >( "glfwHideWindow" );

    /// <summary>
    /// <para>This function hides the specified window if it was previously visible. If the window is already hidden or is in full screen mode, this function does nothing.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_hide">Window visibility</see> </para> <para> <see cref="NativeGlfw.glfwShowWindow"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwHideWindow( GlfwWindow* window ) => p_glfwHideWindow( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwFocusWindow( GlfwWindow* window );

    private static d_glfwFocusWindow p_glfwFocusWindow = LoadFunction< d_glfwFocusWindow >( "glfwFocusWindow" );

    /// <summary>
    /// <para>This function brings the specified window to front and sets input focus. The window should already be visible and not iconified.</para> <para>By default, both windowed and full screen mode windows are focused when initially created. Set the <see cref="NativeGlfw.GLFW_FOCUSED"/> to disable this behavior.</para> <para>Also by default, windowed mode windows are focused when shown with <see cref="NativeGlfw.glfwShowWindow"/>. Set the <see cref="NativeGlfw.GLFW_FOCUS_ON_SHOW"/> to disable this behavior.</para> <para>Do not use this function to steal focus from other applications unless you are certain that is what the user wants. Focus stealing can be extremely disruptive.</para> <para>For a less disruptive way of getting the user's attention, see <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_attention">attention requests</see>.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>Wayland: The compositor will likely ignore focus requests unless another window created by the same application already has input focus.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_focus">Window input focus</see> </para> <para> <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_attention">Window attention request</see></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static void glfwFocusWindow( GlfwWindow* window ) => p_glfwFocusWindow( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwRequestWindowAttention( GlfwWindow* window );

    private static d_glfwRequestWindowAttention p_glfwRequestWindowAttention = LoadFunction< d_glfwRequestWindowAttention >( "glfwRequestWindowAttention" );

    /// <summary>
    /// <para>This function requests user attention to the specified window. On platforms where this is not supported, attention is requested to the application as a whole.</para> <para>Once the user has given attention, usually by focusing the window or application, the system will end the request automatically.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>macOS: Attention is requested to the application as a whole, not the specific window.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_attention">Window attention request</see></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwRequestWindowAttention( GlfwWindow* window ) => p_glfwRequestWindowAttention( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwMonitor* d_glfwGetWindowMonitor( GlfwWindow* window );

    private static d_glfwGetWindowMonitor p_glfwGetWindowMonitor = LoadFunction< d_glfwGetWindowMonitor >( "glfwGetWindowMonitor" );

    /// <summary>
    /// <para>This function returns the handle of the monitor that the specified window is in full screen on.</para> <para><b>Returns</b><para>The monitor, or <c>NULL</c> if the window is in windowed mode or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_monitor">Window monitor</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowMonitor"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static GlfwMonitor* glfwGetWindowMonitor( GlfwWindow* window ) => p_glfwGetWindowMonitor( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowMonitor( GlfwWindow* window, GlfwMonitor* monitor, int xpos, int ypos, int width, int height, int refreshRate );

    private static d_glfwSetWindowMonitor p_glfwSetWindowMonitor = LoadFunction< d_glfwSetWindowMonitor >( "glfwSetWindowMonitor" );

    /// <summary>
    /// <para>This function sets the monitor that the window uses for full screen mode or, if the monitor is <c>NULL</c>, makes it windowed mode.</para> <para>When setting a monitor, this function updates the width, height and refresh rate of the desired video mode and switches to the video mode closest to it. The window position is ignored when setting a monitor.</para> <para>When the monitor is <c>NULL</c>, the position, width and height are used to place the window content area. The refresh rate is ignored when no monitor is specified.</para> <para>If you only wish to update the resolution of a full screen window or the size of a windowed mode window, see <see cref="NativeGlfw.glfwSetWindowSize"/>.</para> <para>When a window transitions from full screen to windowed mode, this function restores any previous window settings such as whether it is decorated, floating, resizable, has size or aspect ratio limits, etc.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>The OpenGL or OpenGL ES context will not be destroyed or otherwise affected by any resizing or mode switching, although you may need to update your viewport if the framebuffer size has changed.</para> <para> Wayland: The desired window position is ignored, as there is no way for an application to set this property.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_monitor">Window monitor</see> </para> <para> <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_full_screen">Full screen windows</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowMonitor"/> </para> <para> <see cref="NativeGlfw.glfwSetWindowSize"/></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static void glfwSetWindowMonitor( GlfwWindow* window, GlfwMonitor* monitor, int xpos, int ypos, int width, int height, int refreshRate ) => p_glfwSetWindowMonitor( window, monitor, xpos, ypos, width, height, refreshRate );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetWindowAttrib( GlfwWindow* window, int attrib );

    private static d_glfwGetWindowAttrib p_glfwGetWindowAttrib = LoadFunction< d_glfwGetWindowAttrib >( "glfwGetWindowAttrib" );

    /// <summary>
    /// <para>This function returns the value of an attribute of the specified window or its OpenGL or OpenGL ES context.</para> <para><b>Returns</b><para>The value of the attribute, or zero if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>Framebuffer related hints are not window attributes. See <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_attribs_fb">Framebuffer related attributes</see> for more information.</para> <para>Zero is a valid value for many window and context related attributes so you cannot use a return value of zero as an indication of errors. However, this function should not fail as long as it is passed valid arguments and the library has been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para> <para> Wayland: The Wayland protocol provides no way to check whether a window is iconfied, so <see cref="NativeGlfw.GLFW_ICONIFIED"/> always returns <see cref="NativeGlfw.GLFW_FALSE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_attribs">Window attributes</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowAttrib"/></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwGetWindowParam</c> and <c>glfwGetGLVersion</c>. </para></para>
    /// </summary>
    public static int glfwGetWindowAttrib( GlfwWindow* window, int attrib ) => p_glfwGetWindowAttrib( window, attrib );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowAttrib( GlfwWindow* window, int attrib, int value );

    private static d_glfwSetWindowAttrib p_glfwSetWindowAttrib = LoadFunction< d_glfwSetWindowAttrib >( "glfwSetWindowAttrib" );

    /// <summary>
    /// <para>This function sets the value of an attribute of the specified window.</para> <para>The supported attributes are <see cref="NativeGlfw.GLFW_DECORATED"/>, <see cref="NativeGlfw.GLFW_RESIZABLE"/>, <see cref="NativeGlfw.GLFW_FLOATING"/>, <see cref="NativeGlfw.GLFW_AUTO_ICONIFY"/> and <see cref="NativeGlfw.GLFW_FOCUS_ON_SHOW"/>. <see cref="NativeGlfw.GLFW_MOUSE_PASSTHROUGH"/></para> <para>Some of these attributes are ignored for full screen windows. The new value will take effect if the window is later made windowed.</para> <para>Some of these attributes are ignored for windowed mode windows. The new value will take effect if the window is later made full screen.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Remarks</b><para>Calling <see cref="NativeGlfw.glfwGetWindowAttrib"/> will always return the latest value, even if that value is ignored by the current mode of the window.</para> <para> Wayland: The <see cref="NativeGlfw.GLFW_FLOATING"/> window attribute is not supported. Setting this will emit <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_attribs">Window attributes</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowAttrib"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwSetWindowAttrib( GlfwWindow* window, int attrib, int value ) => p_glfwSetWindowAttrib( window, attrib, value );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetWindowUserPointer( GlfwWindow* window, IntPtr pointer );

    private static d_glfwSetWindowUserPointer p_glfwSetWindowUserPointer = LoadFunction< d_glfwSetWindowUserPointer >( "glfwSetWindowUserPointer" );

    /// <summary>
    /// <para>This function sets the user-defined pointer of the specified window. The current value is retained until the window is destroyed. The initial value is <c>NULL</c>.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Access is not synchronized.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_userptr">User pointer</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowUserPointer"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwSetWindowUserPointer( GlfwWindow* window, IntPtr pointer ) => p_glfwSetWindowUserPointer( window, pointer );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetWindowUserPointer( GlfwWindow* window );

    private static d_glfwGetWindowUserPointer p_glfwGetWindowUserPointer = LoadFunction< d_glfwGetWindowUserPointer >( "glfwGetWindowUserPointer" );

    /// <summary>
    /// <para>This function returns the current value of the user-defined pointer of the specified window. The initial value is <c>NULL</c>.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Access is not synchronized.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_userptr">User pointer</see> </para> <para> <see cref="NativeGlfw.glfwSetWindowUserPointer"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static IntPtr glfwGetWindowUserPointer( GlfwWindow* window ) => p_glfwGetWindowUserPointer( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetWindowPosCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetWindowPosCallback p_glfwSetWindowPosCallback = LoadFunction< d_glfwSetWindowPosCallback >( "glfwSetWindowPosCallback" );

    /// <summary>
    /// <para>This function sets the position callback of the specified window, which is called when the window is moved. The callback is provided with the position, in screen coordinates, of the upper-left corner of the content area of the window.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int xpos, int ypos) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#gabe287973a21a8f927cde4db06b8dcbe9">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Remarks</b><para>Wayland: This callback will never be called, as there is no way for an application to know its global position.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_pos">Window position</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwWindowposfun glfwSetWindowPosCallback( GlfwWindow* window, GlfwWindowposfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwWindowposfun ____callback_native_managed;
        System.IntPtr    ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetWindowPosCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwWindowposfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwWindowposfun CurrentGlfwWindowposfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetWindowSizeCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetWindowSizeCallback p_glfwSetWindowSizeCallback = LoadFunction< d_glfwSetWindowSizeCallback >( "glfwSetWindowSizeCallback" );

    /// <summary>
    /// <para>This function sets the size callback of the specified window, which is called when the window is resized. The callback is provided with the size, in screen coordinates, of the content area of the window.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int width, int height) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#gaec0282944bb810f6f3163ec02da90350">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_size">Window size</see></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter and return value. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwWindowsizefun glfwSetWindowSizeCallback( GlfwWindow* window, GlfwWindowsizefun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwWindowsizefun ____callback_native_managed;
        System.IntPtr     ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetWindowSizeCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwWindowsizefun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwWindowsizefun CurrentGlfwWindowsizefun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetWindowCloseCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetWindowCloseCallback p_glfwSetWindowCloseCallback = LoadFunction< d_glfwSetWindowCloseCallback >( "glfwSetWindowCloseCallback" );

    /// <summary>
    /// <para>This function sets the close callback of the specified window, which is called when the user attempts to close the window, for example by clicking the close widget in the title bar.</para> <para>The close flag is set before this callback is called, but you can modify it at any time with <see cref="NativeGlfw.glfwSetWindowShouldClose"/>.</para> <para>The close callback is not triggered by <see cref="NativeGlfw.glfwDestroyWindow"/>.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#gabf859b936d80961b7d39013a9694cc3e">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Remarks</b><para>macOS: Selecting Quit from the application menu will trigger the close callback for all windows.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_close">Window closing and close flag</see></para></para> <para><b>Since</b><para>Added in version 2.5. GLFW 3: Added window handle parameter and return value. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwWindowclosefun glfwSetWindowCloseCallback( GlfwWindow* window, GlfwWindowclosefun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwWindowclosefun ____callback_native_managed;
        System.IntPtr      ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetWindowCloseCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwWindowclosefun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwWindowclosefun CurrentGlfwWindowclosefun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetWindowRefreshCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetWindowRefreshCallback p_glfwSetWindowRefreshCallback = LoadFunction< d_glfwSetWindowRefreshCallback >( "glfwSetWindowRefreshCallback" );

    /// <summary>
    /// <para>This function sets the refresh callback of the specified window, which is called when the content area of the window needs to be redrawn, for example if the window has been exposed after having been covered by another window.</para> <para>On compositing window systems such as Aero, Compiz, Aqua or Wayland, where the window contents are saved off-screen, this callback may be called only very infrequently or never at all.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window); For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#ga431663a1427d2eb3a273bc398b6737b5">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_refresh">Window damage and refresh</see></para></para> <para><b>Since</b><para>Added in version 2.5. GLFW 3: Added window handle parameter and return value. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwWindowrefreshfun glfwSetWindowRefreshCallback( GlfwWindow* window, GlfwWindowrefreshfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwWindowrefreshfun ____callback_native_managed;
        System.IntPtr        ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetWindowRefreshCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwWindowrefreshfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwWindowrefreshfun CurrentGlfwWindowrefreshfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetWindowFocusCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetWindowFocusCallback p_glfwSetWindowFocusCallback = LoadFunction< d_glfwSetWindowFocusCallback >( "glfwSetWindowFocusCallback" );

    /// <summary>
    /// <para>This function sets the focus callback of the specified window, which is called when the window gains or loses input focus.</para> <para>After the focus callback is called for a window that lost input focus, synthetic key and mouse button release events will be generated for all such that had been pressed. For more information, see <see cref="NativeGlfw.glfwSetKeyCallback"/> and <see cref="NativeGlfw.glfwSetMouseButtonCallback"/>.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int focused) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#gabc58c47e9d93f6eb1862d615c3680f46">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_focus">Window input focus</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwWindowfocusfun glfwSetWindowFocusCallback( GlfwWindow* window, GlfwWindowfocusfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwWindowfocusfun ____callback_native_managed;
        System.IntPtr      ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetWindowFocusCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwWindowfocusfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwWindowfocusfun CurrentGlfwWindowfocusfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetWindowIconifyCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetWindowIconifyCallback p_glfwSetWindowIconifyCallback = LoadFunction< d_glfwSetWindowIconifyCallback >( "glfwSetWindowIconifyCallback" );

    /// <summary>
    /// <para>This function sets the iconification callback of the specified window, which is called when the window is iconified or restored.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int iconified) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#ga35c658cccba236f26e7adee0e25f6a4f">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_iconify">Window iconification</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwWindowiconifyfun glfwSetWindowIconifyCallback( GlfwWindow* window, GlfwWindowiconifyfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwWindowiconifyfun ____callback_native_managed;
        System.IntPtr        ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetWindowIconifyCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwWindowiconifyfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwWindowiconifyfun CurrentGlfwWindowiconifyfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetWindowMaximizeCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetWindowMaximizeCallback p_glfwSetWindowMaximizeCallback = LoadFunction< d_glfwSetWindowMaximizeCallback >( "glfwSetWindowMaximizeCallback" );

    /// <summary>
    /// <para>This function sets the maximization callback of the specified window, which is called when the window is maximized or restored.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int maximized) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#ga3017196fdaec33ac3e095765176c2a90">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_maximize">Window maximization</see></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwWindowMaximizefun glfwSetWindowMaximizeCallback( GlfwWindow* window, GlfwWindowMaximizefun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwWindowMaximizefun ____callback_native_managed;
        System.IntPtr         ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetWindowMaximizeCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwWindowMaximizefun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwWindowMaximizefun CurrentGlfwWindowMaximizefun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetFramebufferSizeCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetFramebufferSizeCallback p_glfwSetFramebufferSizeCallback = LoadFunction< d_glfwSetFramebufferSizeCallback >( "glfwSetFramebufferSizeCallback" );

    /// <summary>
    /// <para>This function sets the framebuffer resize callback of the specified window, which is called when the framebuffer of the specified window is resized.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int width, int height) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#gae18026e294dde685ed2e5f759533144d">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_fbsize">Framebuffer size</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwFrameBufferSizefun glfwSetFramebufferSizeCallback( GlfwWindow* window, GlfwFrameBufferSizefun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwFrameBufferSizefun ____callback_native_managed;
        System.IntPtr          ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetFramebufferSizeCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwFrameBufferSizefun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwFrameBufferSizefun CurrentGlfwFrameBufferSizefun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetWindowContentScaleCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetWindowContentScaleCallback p_glfwSetWindowContentScaleCallback = LoadFunction< d_glfwSetWindowContentScaleCallback >( "glfwSetWindowContentScaleCallback" );

    /// <summary>
    /// <para>This function sets the window content scale callback of the specified window, which is called when the content scale of the specified window changes.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, float xscale, float yscale) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__window.html#ga77f288a2d04bb3c77c7d9615d08cf70e">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#window_scale">Window content scale</see> </para> <para> <see cref="NativeGlfw.glfwGetWindowContentScale"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwWindowContentScalefun glfwSetWindowContentScaleCallback( GlfwWindow* window, GlfwWindowContentScalefun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwWindowContentScalefun ____callback_native_managed;
        System.IntPtr             ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetWindowContentScaleCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwWindowContentScalefun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwWindowContentScalefun CurrentGlfwWindowContentScalefun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwPollEvents();

    private static d_glfwPollEvents p_glfwPollEvents = LoadFunction< d_glfwPollEvents >( "glfwPollEvents" );

    /// <summary>
    /// <para>This function processes only those events that are already in the event queue and then returns immediately. Processing events will cause the window and input callbacks associated with those events to be called.</para> <para>On some platforms, a window move, resize or menu operation will cause event processing to block. This is due to how event processing is designed on those platforms. You can use the <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_refresh">window refresh callback</see> to redraw the contents of your window when necessary during such operations.</para> <para>Do not assume that callbacks you set will only be called in response to event processing functions like this one. While it is necessary to poll for events, window systems that require GLFW to register callbacks of its own can pass events to GLFW in response to many window system function calls. GLFW will pass those events on to the application callbacks before returning.</para> <para>Event processing is not required for joystick input to work.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Reentrancy</b><para>This function must not be called from a callback.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#events">Event processing</see> </para> <para> <see cref="NativeGlfw.glfwWaitEvents"/> </para> <para> <see cref="NativeGlfw.glfwWaitEventsTimeout"/></para></para> <para><b>Since</b><para>Added in version 1.0. </para></para>
    /// </summary>
    public static void glfwPollEvents() => p_glfwPollEvents();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwWaitEvents();

    private static d_glfwWaitEvents p_glfwWaitEvents = LoadFunction< d_glfwWaitEvents >( "glfwWaitEvents" );

    /// <summary>
    /// <para>This function puts the calling thread to sleep until at least one event is available in the event queue. Once one or more events are available, it behaves exactly like <see cref="NativeGlfw.glfwPollEvents"/>, i.e. the events in the queue are processed and the function then returns immediately. Processing events will cause the window and input callbacks associated with those events to be called.</para> <para>Since not all events are associated with callbacks, this function may return without a callback having been called even if you are monitoring all callbacks.</para> <para>On some platforms, a window move, resize or menu operation will cause event processing to block. This is due to how event processing is designed on those platforms. You can use the <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_refresh">window refresh callback</see> to redraw the contents of your window when necessary during such operations.</para> <para>Do not assume that callbacks you set will only be called in response to event processing functions like this one. While it is necessary to poll for events, window systems that require GLFW to register callbacks of its own can pass events to GLFW in response to many window system function calls. GLFW will pass those events on to the application callbacks before returning.</para> <para>Event processing is not required for joystick input to work.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Reentrancy</b><para>This function must not be called from a callback.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#events">Event processing</see> </para> <para> <see cref="NativeGlfw.glfwPollEvents"/> </para> <para> <see cref="NativeGlfw.glfwWaitEventsTimeout"/></para></para> <para><b>Since</b><para>Added in version 2.5. </para></para>
    /// </summary>
    public static void glfwWaitEvents() => p_glfwWaitEvents();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwWaitEventsTimeout( double timeout );

    private static d_glfwWaitEventsTimeout p_glfwWaitEventsTimeout = LoadFunction< d_glfwWaitEventsTimeout >( "glfwWaitEventsTimeout" );

    /// <summary>
    /// <para>This function puts the calling thread to sleep until at least one event is available in the event queue, or until the specified timeout is reached. If one or more events are available, it behaves exactly like <see cref="NativeGlfw.glfwPollEvents"/>, i.e. the events in the queue are processed and the function then returns immediately. Processing events will cause the window and input callbacks associated with those events to be called.</para> <para>The timeout value must be a positive finite number.</para> <para>Since not all events are associated with callbacks, this function may return without a callback having been called even if you are monitoring all callbacks.</para> <para>On some platforms, a window move, resize or menu operation will cause event processing to block. This is due to how event processing is designed on those platforms. You can use the <see href="https://www.glfw.org/docs/3.4/window_guide.html#window_refresh">window refresh callback</see> to redraw the contents of your window when necessary during such operations.</para> <para>Do not assume that callbacks you set will only be called in response to event processing functions like this one. While it is necessary to poll for events, window systems that require GLFW to register callbacks of its own can pass events to GLFW in response to many window system function calls. GLFW will pass those events on to the application callbacks before returning.</para> <para>Event processing is not required for joystick input to work.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Reentrancy</b><para>This function must not be called from a callback.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#events">Event processing</see> </para> <para> <see cref="NativeGlfw.glfwPollEvents"/> </para> <para> <see cref="NativeGlfw.glfwWaitEvents"/></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static void glfwWaitEventsTimeout( double timeout ) => p_glfwWaitEventsTimeout( timeout );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwPostEmptyEvent();

    private static d_glfwPostEmptyEvent p_glfwPostEmptyEvent = LoadFunction< d_glfwPostEmptyEvent >( "glfwPostEmptyEvent" );

    /// <summary>
    /// <para>This function posts an empty event from the current thread to the event queue, causing <see cref="NativeGlfw.glfwWaitEvents"/> or <see cref="NativeGlfw.glfwWaitEventsTimeout"/> to return.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#events">Event processing</see> </para> <para> <see cref="NativeGlfw.glfwWaitEvents"/> </para> <para> <see cref="NativeGlfw.glfwWaitEventsTimeout"/></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    public static void glfwPostEmptyEvent() => p_glfwPostEmptyEvent();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetInputMode( GlfwWindow* window, int mode );

    private static d_glfwGetInputMode p_glfwGetInputMode = LoadFunction< d_glfwGetInputMode >( "glfwGetInputMode" );

    /// <summary>
    /// <para>This function returns the value of an input option for the specified window. The mode must be one of <see cref="NativeGlfw.GLFW_CURSOR"/>, <see cref="NativeGlfw.GLFW_STICKY_KEYS"/>, <see cref="NativeGlfw.GLFW_STICKY_MOUSE_BUTTONS"/>, <see cref="NativeGlfw.GLFW_LOCK_KEY_MODS"/> or <see cref="NativeGlfw.GLFW_RAW_MOUSE_MOTION"/>.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see cref="NativeGlfw.glfwSetInputMode"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static int glfwGetInputMode( GlfwWindow* window, int mode ) => p_glfwGetInputMode( window, mode );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetInputMode( GlfwWindow* window, int mode, int value );

    private static d_glfwSetInputMode p_glfwSetInputMode = LoadFunction< d_glfwSetInputMode >( "glfwSetInputMode" );

    /// <summary>
    /// <para>This function sets an input mode option for the specified window. The mode must be one of <see cref="NativeGlfw.GLFW_CURSOR"/>, <see cref="NativeGlfw.GLFW_STICKY_KEYS"/>, <see cref="NativeGlfw.GLFW_STICKY_MOUSE_BUTTONS"/>, <see cref="NativeGlfw.GLFW_LOCK_KEY_MODS"/> or <see cref="NativeGlfw.GLFW_RAW_MOUSE_MOTION"/>.</para> <para>If the mode is <see cref="NativeGlfw.GLFW_CURSOR"/>, the value must be one of the following cursor modes:</para> <see cref="NativeGlfw.GLFW_CURSOR_NORMAL"/> makes the cursor visible and behaving normally. <see cref="NativeGlfw.GLFW_CURSOR_HIDDEN"/> makes the cursor invisible when it is over the content area of the window but does not restrict the cursor from leaving. <see cref="NativeGlfw.GLFW_CURSOR_DISABLED"/> hides and grabs the cursor, providing virtual and unlimited cursor movement. This is useful for implementing for example 3D camera controls. <see cref="NativeGlfw.GLFW_CURSOR_CAPTURED"/> makes the cursor visible and confines it to the content area of the window. <para>If the mode is <see cref="NativeGlfw.GLFW_STICKY_KEYS"/>, the value must be either <see cref="NativeGlfw.GLFW_TRUE"/> to enable sticky keys, or <see cref="NativeGlfw.GLFW_FALSE"/> to disable it. If sticky keys are enabled, a key press will ensure that <see cref="NativeGlfw.glfwGetKey"/> returns <see cref="NativeGlfw.GLFW_PRESS"/> the next time it is called even if the key had been released before the call. This is useful when you are only interested in whether keys have been pressed but not when or in which order.</para> <para>If the mode is <see cref="NativeGlfw.GLFW_STICKY_MOUSE_BUTTONS"/>, the value must be either <see cref="NativeGlfw.GLFW_TRUE"/> to enable sticky mouse buttons, or <see cref="NativeGlfw.GLFW_FALSE"/> to disable it. If sticky mouse buttons are enabled, a mouse button press will ensure that <see cref="NativeGlfw.glfwGetMouseButton"/> returns <see cref="NativeGlfw.GLFW_PRESS"/> the next time it is called even if the mouse button had been released before the call. This is useful when you are only interested in whether mouse buttons have been pressed but not when or in which order.</para> <para>If the mode is <see cref="NativeGlfw.GLFW_LOCK_KEY_MODS"/>, the value must be either <see cref="NativeGlfw.GLFW_TRUE"/> to enable lock key modifier bits, or <see cref="NativeGlfw.GLFW_FALSE"/> to disable them. If enabled, callbacks that receive modifier bits will also have the <see cref="NativeGlfw.GLFW_MOD_CAPS_LOCK"/> bit set when the event was generated with Caps Lock on, and the <see cref="NativeGlfw.GLFW_MOD_NUM_LOCK"/> bit when Num Lock was on.</para> <para>If the mode is <see cref="NativeGlfw.GLFW_RAW_MOUSE_MOTION"/>, the value must be either <see cref="NativeGlfw.GLFW_TRUE"/> to enable raw (unscaled and unaccelerated) mouse motion when the cursor is disabled, or <see cref="NativeGlfw.GLFW_FALSE"/> to disable it. If raw motion is not supported, attempting to set this will emit <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>. Call <see cref="NativeGlfw.glfwRawMouseMotionSupported"/> to check for support.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see above).</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see cref="NativeGlfw.glfwGetInputMode"/></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwEnable</c> and <c>glfwDisable</c>. </para></para>
    /// </summary>
    public static void glfwSetInputMode( GlfwWindow* window, int mode, int value ) => p_glfwSetInputMode( window, mode, value );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwRawMouseMotionSupported();

    private static d_glfwRawMouseMotionSupported p_glfwRawMouseMotionSupported = LoadFunction< d_glfwRawMouseMotionSupported >( "glfwRawMouseMotionSupported" );

    /// <summary>
    /// <para>This function returns whether raw mouse motion is supported on the current system. This status does not change after GLFW has been initialized so you only need to check this once. If you attempt to enable raw motion on a system that does not support it, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> will be emitted.</para> <para>Raw mouse motion is closer to the actual motion of the mouse across a surface. It is not affected by the scaling and acceleration applied to the motion of the desktop cursor. That processing is suitable for a cursor while raw motion is better for controlling for example a 3D camera. Because of this, raw mouse motion is only provided when the cursor is disabled.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if raw mouse motion is supported on the current machine, or <see cref="NativeGlfw.GLFW_FALSE"/> otherwise.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#raw_mouse_motion">Raw mouse motion</see> </para> <para> <see cref="NativeGlfw.glfwSetInputMode"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static int glfwRawMouseMotionSupported() => p_glfwRawMouseMotionSupported();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetKeyName( int key, int scancode );

    private static d_glfwGetKeyName p_glfwGetKeyName = LoadFunction< d_glfwGetKeyName >( "glfwGetKeyName" );

    /// <summary>
    /// <para>This function returns the name of the specified printable key, encoded as UTF-8. This is typically the character that key would produce without any modifier keys, intended for displaying key bindings to the user. For dead keys, it is typically the diacritic it would add to a character.</para> <para>Do not use this function for <see href="https://www.glfw.org/docs/3.4/input_guide.html#input_char">text input</see>. You will break text input for many languages even if it happens to work for yours.</para> <para>If the key is <see cref="NativeGlfw.GLFW_KEY_UNKNOWN"/>, the scancode is used to identify the key, otherwise the scancode is ignored. If you specify a non-printable key, or <see cref="NativeGlfw.GLFW_KEY_UNKNOWN"/> and a scancode that maps to a non-printable key, this function returns <c>NULL</c> but does not emit an error.</para> <para>This behavior allows you to always pass in the arguments in the <see href="https://www.glfw.org/docs/3.4/input_guide.html#input_key">key callback</see> without modification.</para> <para>The printable keys are:</para> <see cref="NativeGlfw.GLFW_KEY_APOSTROPHE"/> <see cref="NativeGlfw.GLFW_KEY_COMMA"/> <see cref="NativeGlfw.GLFW_KEY_MINUS"/> <see cref="NativeGlfw.GLFW_KEY_PERIOD"/> <see cref="NativeGlfw.GLFW_KEY_SLASH"/> <see cref="NativeGlfw.GLFW_KEY_SEMICOLON"/> <see cref="NativeGlfw.GLFW_KEY_EQUAL"/> <see cref="NativeGlfw.GLFW_KEY_LEFT_BRACKET"/> <see cref="NativeGlfw.GLFW_KEY_RIGHT_BRACKET"/> <see cref="NativeGlfw.GLFW_KEY_BACKSLASH"/> <see cref="NativeGlfw.GLFW_KEY_WORLD_1"/> <see cref="NativeGlfw.GLFW_KEY_WORLD_2"/> <see cref="NativeGlfw.GLFW_KEY_0"/> to <see cref="NativeGlfw.GLFW_KEY_9"/> <see cref="NativeGlfw.GLFW_KEY_A"/> to <see cref="NativeGlfw.GLFW_KEY_Z"/> <see cref="NativeGlfw.GLFW_KEY_KP_0"/> to <see cref="NativeGlfw.GLFW_KEY_KP_9"/> <see cref="NativeGlfw.GLFW_KEY_KP_DECIMAL"/> <see cref="NativeGlfw.GLFW_KEY_KP_DIVIDE"/> <see cref="NativeGlfw.GLFW_KEY_KP_MULTIPLY"/> <see cref="NativeGlfw.GLFW_KEY_KP_SUBTRACT"/> <see cref="NativeGlfw.GLFW_KEY_KP_ADD"/> <see cref="NativeGlfw.GLFW_KEY_KP_EQUAL"/> <para>Names for printable keys depend on keyboard layout, while names for non-printable keys are the same across layouts but depend on the application language and should be localized along with other user interface text.</para> <para><b>Returns</b><para>The UTF-8 encoded, layout-specific name of the key, or <c>NULL</c>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>The contents of the returned string may change when a keyboard layout change event is received.</para></para> <para><b>Pointer lifetime</b><para>The returned string is allocated and freed by GLFW. You should not free it yourself. It is valid until the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_key_name">Key names</see></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static IntPtr glfwGetKeyName( int key, int scancode ) => p_glfwGetKeyName( key, scancode );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetKeyScancode( int key );

    private static d_glfwGetKeyScancode p_glfwGetKeyScancode = LoadFunction< d_glfwGetKeyScancode >( "glfwGetKeyScancode" );

    /// <summary>
    /// <para>This function returns the platform-specific scancode of the specified key.</para> <para>If the specified <see href="https://www.glfw.org/docs/3.4/group__keys.html">key token</see> corresponds to a physical key not supported on the current platform then this method will return <c>-1</c>. Calling this function with anything other than a key token will return <c>-1</c> and generate a <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> error.</para> <para><b>Returns</b><para>The platform-specific scancode for the key, or <c>-1</c> if the key is not supported on the current platform or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_key">Key input</see></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static int glfwGetKeyScancode( int key ) => p_glfwGetKeyScancode( key );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetKey( GlfwWindow* window, int key );

    private static d_glfwGetKey p_glfwGetKey = LoadFunction< d_glfwGetKey >( "glfwGetKey" );

    /// <summary>
    /// <para>This function returns the last state reported for the specified key to the specified window. The returned state is one of <see cref="NativeGlfw.GLFW_PRESS"/> or <see cref="NativeGlfw.GLFW_RELEASE"/>. The action <see cref="NativeGlfw.GLFW_REPEAT"/> is only reported to the key callback.</para> <para>If the <see cref="NativeGlfw.GLFW_STICKY_KEYS"/> input mode is enabled, this function returns <see cref="NativeGlfw.GLFW_PRESS"/> the first time you call it for a key that was pressed, even if that key has already been released.</para> <para>The key functions deal with physical keys, with <see href="https://www.glfw.org/docs/3.4/group__keys.html">key tokens</see> named after their use on the standard US keyboard layout. If you want to input text, use the Unicode character callback instead.</para> <para>The <see href="https://www.glfw.org/docs/3.4/group__mods.html">modifier key bit masks</see> are not key tokens and cannot be used with this function.</para> <para>Do not use this function to implement <see href="https://www.glfw.org/docs/3.4/input_guide.html#input_char">text input</see>.</para> <para><b>Returns</b><para>One of <see cref="NativeGlfw.GLFW_PRESS"/> or <see cref="NativeGlfw.GLFW_RELEASE"/>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_key">Key input</see></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static int glfwGetKey( GlfwWindow* window, int key ) => p_glfwGetKey( window, key );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetMouseButton( GlfwWindow* window, int button );

    private static d_glfwGetMouseButton p_glfwGetMouseButton = LoadFunction< d_glfwGetMouseButton >( "glfwGetMouseButton" );

    /// <summary>
    /// <para>This function returns the last state reported for the specified mouse button to the specified window. The returned state is one of <see cref="NativeGlfw.GLFW_PRESS"/> or <see cref="NativeGlfw.GLFW_RELEASE"/>.</para> <para>If the <see cref="NativeGlfw.GLFW_STICKY_MOUSE_BUTTONS"/> input mode is enabled, this function returns <see cref="NativeGlfw.GLFW_PRESS"/> the first time you call it for a mouse button that was pressed, even if that mouse button has already been released.</para> <para><b>Returns</b><para>One of <see cref="NativeGlfw.GLFW_PRESS"/> or <see cref="NativeGlfw.GLFW_RELEASE"/>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_mouse_button">Mouse button input</see></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static int glfwGetMouseButton( GlfwWindow* window, int button ) => p_glfwGetMouseButton( window, button );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwGetCursorPos( GlfwWindow* window, IntPtr xpos, IntPtr ypos );

    private static d_glfwGetCursorPos p_glfwGetCursorPos = LoadFunction< d_glfwGetCursorPos >( "glfwGetCursorPos" );

    /// <summary>
    /// <para>This function returns the position of the cursor, in screen coordinates, relative to the upper-left corner of the content area of the specified window.</para> <para>If the cursor is disabled (with <see cref="NativeGlfw.GLFW_CURSOR_DISABLED"/>) then the cursor position is unbounded and limited only by the minimum and maximum values of a <c>double</c>.</para> <para>The coordinate can be converted to their integer equivalents with the <c>floor</c> function. Casting directly to an integer type works for positive coordinates, but fails for negative ones.</para> <para>Any or all of the position arguments may be <c>NULL</c>. If an error occurs, all non-<c>NULL</c> position arguments will be set to zero.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_pos">Cursor position</see> </para> <para> <see cref="NativeGlfw.glfwSetCursorPos"/></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwGetMousePos</c>. </para></para>
    /// </summary>
    public static void glfwGetCursorPos( GlfwWindow* window, IntPtr xpos, IntPtr ypos ) => p_glfwGetCursorPos( window, xpos, ypos );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetCursorPos( GlfwWindow* window, double xpos, double ypos );

    private static d_glfwSetCursorPos p_glfwSetCursorPos = LoadFunction< d_glfwSetCursorPos >( "glfwSetCursorPos" );

    /// <summary>
    /// <para>This function sets the position, in screen coordinates, of the cursor relative to the upper-left corner of the content area of the specified window. The window must have input focus. If the window does not have input focus when this function is called, it fails silently.</para> <para>Do not use this function to implement things like camera controls. GLFW already provides the <see cref="NativeGlfw.GLFW_CURSOR_DISABLED"/> cursor mode that hides the cursor, transparently re-centers it and provides unconstrained cursor motion. See <see cref="NativeGlfw.glfwSetInputMode"/> for more information.</para> <para>If the cursor mode is <see cref="NativeGlfw.GLFW_CURSOR_DISABLED"/> then the cursor position is unconstrained and limited only by the minimum and maximum values of a <c>double</c>.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/> (see remarks).</para></para> <para><b>Remarks</b><para>Wayland: This function will only work when the cursor mode is <see cref="NativeGlfw.GLFW_CURSOR_DISABLED"/>, otherwise it will emit <see cref="NativeGlfw.GLFW_FEATURE_UNAVAILABLE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_pos">Cursor position</see> </para> <para> <see cref="NativeGlfw.glfwGetCursorPos"/></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwSetMousePos</c>. </para></para>
    /// </summary>
    public static void glfwSetCursorPos( GlfwWindow* window, double xpos, double ypos ) => p_glfwSetCursorPos( window, xpos, ypos );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwCursor* d_glfwCreateCursor( GlfwImage* image, int xhot, int yhot );

    private static d_glfwCreateCursor p_glfwCreateCursor = LoadFunction< d_glfwCreateCursor >( "glfwCreateCursor" );

    /// <summary>
    /// <para>Creates a new custom cursor image that can be set for a window with <see cref="NativeGlfw.glfwSetCursor"/>. The cursor can be destroyed with <see cref="NativeGlfw.glfwDestroyCursor"/>. Any remaining cursors are destroyed by <see cref="NativeGlfw.glfwTerminate"/>.</para> <para>The pixels are 32-bit, little-endian, non-premultiplied RGBA, i.e. eight bits per channel with the red channel first. They are arranged canonically as packed sequential rows, starting from the top-left corner.</para> <para>The cursor hotspot is specified in pixels, relative to the upper-left corner of the cursor image. Like all other coordinate systems in GLFW, the X-axis points to the right and the Y-axis points down.</para> <para><b>Returns</b><para>The handle of the created cursor, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Pointer lifetime</b><para>The specified image data is copied before this function returns.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_object">Cursor objects</see> </para> <para> <see cref="NativeGlfw.glfwDestroyCursor"/> </para> <para> <see cref="NativeGlfw.glfwCreateStandardCursor"/></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    public static GlfwCursor* glfwCreateCursor( GlfwImage* image, int xhot, int yhot ) => p_glfwCreateCursor( image, xhot, yhot );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwCursor* d_glfwCreateStandardCursor( int shape );

    private static d_glfwCreateStandardCursor p_glfwCreateStandardCursor = LoadFunction< d_glfwCreateStandardCursor >( "glfwCreateStandardCursor" );

    /// <summary>
    /// <para>Returns a cursor with a standard shape, that can be set for a window with <see cref="NativeGlfw.glfwSetCursor"/>. The images for these cursors come from the system cursor theme and their exact appearance will vary between platforms.</para> <para>Most of these shapes are guaranteed to exist on every supported platform but a few may not be present. See the table below for details.</para> <para>1) This uses a private system API and may fail in the future.</para> <para>2) This uses a newer standard that not all cursor themes support.</para> <para>If the requested shape is not available, this function emits a <see cref="NativeGlfw.GLFW_CURSOR_UNAVAILABLE"/> error and returns <c>NULL</c>.</para> <para><b>Returns</b><para>A new cursor ready to use or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>, <see cref="NativeGlfw.GLFW_CURSOR_UNAVAILABLE"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_standard">Standard cursor creation</see> </para> <para> <see cref="NativeGlfw.glfwCreateCursor"/></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    public static GlfwCursor* glfwCreateStandardCursor( int shape ) => p_glfwCreateStandardCursor( shape );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwDestroyCursor( GlfwCursor* cursor );

    private static d_glfwDestroyCursor p_glfwDestroyCursor = LoadFunction< d_glfwDestroyCursor >( "glfwDestroyCursor" );

    /// <summary>
    /// <para>This function destroys a cursor previously created with <see cref="NativeGlfw.glfwCreateCursor"/>. Any remaining cursors will be destroyed by <see cref="NativeGlfw.glfwTerminate"/>.</para> <para>If the specified cursor is current for any window, that window will be reverted to the default cursor. This does not affect the cursor mode.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Reentrancy</b><para>This function must not be called from a callback.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_object">Cursor objects</see> </para> <para> <see cref="NativeGlfw.glfwCreateCursor"/></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    public static void glfwDestroyCursor( GlfwCursor* cursor ) => p_glfwDestroyCursor( cursor );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetCursor( GlfwWindow* window, GlfwCursor* cursor );

    private static d_glfwSetCursor p_glfwSetCursor = LoadFunction< d_glfwSetCursor >( "glfwSetCursor" );

    /// <summary>
    /// <para>This function sets the cursor image to be used when the cursor is over the content area of the specified window. The set cursor will only be visible when the <see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_mode">cursor mode</see> of the window is <see cref="NativeGlfw.GLFW_CURSOR_NORMAL"/>.</para> <para>On some platforms, the set cursor may not be visible unless the window also has input focus.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_object">Cursor objects</see></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    public static void glfwSetCursor( GlfwWindow* window, GlfwCursor* cursor ) => p_glfwSetCursor( window, cursor );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetKeyCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetKeyCallback p_glfwSetKeyCallback = LoadFunction< d_glfwSetKeyCallback >( "glfwSetKeyCallback" );

    /// <summary>
    /// <para>This function sets the key callback of the specified window, which is called when a key is pressed, repeated or released.</para> <para>The key functions deal with physical keys, with layout independent <see href="https://www.glfw.org/docs/3.4/group__keys.html">key tokens</see> named after their values in the standard US keyboard layout. If you want to input text, use the <see href="https://www.glfw.org/docs/3.4/group__input.html#gab25c4a220fd8f5717718dbc487828996">character callback</see> instead.</para> <para>When a window loses input focus, it will generate synthetic key release events for all pressed keys with associated key tokens. You can tell these events from user-generated events by the fact that the synthetic ones are generated after the focus loss event has been processed, i.e. after the <see href="https://www.glfw.org/docs/3.4/group__window.html#gac2d83c4a10f071baf841f6730528e66c">window focus callback</see> has been called.</para> <para>The scancode of a key is specific to that platform or sometimes even to that machine. Scancodes are intended to allow users to bind keys that don't have a GLFW key token. Such keys have <c>key</c> set to <see cref="NativeGlfw.GLFW_KEY_UNKNOWN"/>, their state is not saved and so it cannot be queried with <see cref="NativeGlfw.glfwGetKey"/>.</para> <para>Sometimes GLFW needs to generate synthetic key events, in which case the scancode may be zero.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int key, int scancode, int action, int mods) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#ga5bd751b27b90f865d2ea613533f0453c">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_key">Key input</see></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter and return value. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwKeyfun glfwSetKeyCallback( GlfwWindow* window, GlfwKeyfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwKeyfun    ____callback_native_managed;
        System.IntPtr ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetKeyCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwKeyfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwKeyfun CurrentGlfwKeyfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetCharCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetCharCallback p_glfwSetCharCallback = LoadFunction< d_glfwSetCharCallback >( "glfwSetCharCallback" );

    /// <summary>
    /// <para>This function sets the character callback of the specified window, which is called when a Unicode character is input.</para> <para>The character callback is intended for Unicode text input. As it deals with characters, it is keyboard layout dependent, whereas the <see href="https://www.glfw.org/docs/3.4/group__input.html#ga1caf18159767e761185e49a3be019f8d">key callback</see> is not. Characters do not map 1:1 to physical keys, as a key may produce zero, one or more characters. If you want to know whether a specific physical key was pressed or released, see the key callback instead.</para> <para>The character callback behaves as system text input normally does and will not be called if modifier keys are held down that would prevent normal text input on that platform, for example a Super (Command) key on macOS or Alt key on Windows.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, unsigned int codepoint) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#ga1ab90a55cf3f58639b893c0f4118cb6e">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_char">Text input</see></para></para> <para><b>Since</b><para>Added in version 2.4. GLFW 3: Added window handle parameter and return value. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwCharfun glfwSetCharCallback( GlfwWindow* window, GlfwCharfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwCharfun   ____callback_native_managed;
        System.IntPtr ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetCharCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwCharfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwCharfun CurrentGlfwCharfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetCharModsCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetCharModsCallback p_glfwSetCharModsCallback = LoadFunction< d_glfwSetCharModsCallback >( "glfwSetCharModsCallback" );

    /// <summary>
    /// <para>This function sets the character with modifiers callback of the specified window, which is called when a Unicode character is input regardless of what modifier keys are used.</para> <para>The character with modifiers callback is intended for implementing custom Unicode character input. For regular Unicode text input, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#gab25c4a220fd8f5717718dbc487828996">character callback</see>. Like the character callback, the character with modifiers callback deals with characters and is keyboard layout dependent. Characters do not map 1:1 to physical keys, as a key may produce zero, one or more characters. If you want to know whether a specific physical key was pressed or released, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#ga1caf18159767e761185e49a3be019f8d">key callback</see> instead.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, unsigned int codepoint, int mods) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#gac3cf64f90b6219c05ac7b7822d5a4b8f">function pointer type</see>.</para></para> <para><b><see href="https://www.glfw.org/docs/3.4/deprecated.html#_deprecated000002">Deprecated:</see></b><para>Scheduled for removal in version 4.0.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_char">Text input</see></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwCharmodsfun glfwSetCharModsCallback( GlfwWindow* window, GlfwCharmodsfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwCharmodsfun ____callback_native_managed;
        System.IntPtr   ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetCharModsCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwCharmodsfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwCharmodsfun CurrentGlfwCharmodsfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetMouseButtonCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetMouseButtonCallback p_glfwSetMouseButtonCallback = LoadFunction< d_glfwSetMouseButtonCallback >( "glfwSetMouseButtonCallback" );

    /// <summary>
    /// <para>This function sets the mouse button callback of the specified window, which is called when a mouse button is pressed or released.</para> <para>When a window loses input focus, it will generate synthetic mouse button release events for all pressed mouse buttons. You can tell these events from user-generated events by the fact that the synthetic ones are generated after the focus loss event has been processed, i.e. after the <see href="https://www.glfw.org/docs/3.4/group__window.html#gac2d83c4a10f071baf841f6730528e66c">window focus callback</see> has been called.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int button, int action, int mods) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#ga0184dcb59f6d85d735503dcaae809727">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#input_mouse_button">Mouse button input</see></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter and return value. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwMouseButtonfun glfwSetMouseButtonCallback( GlfwWindow* window, GlfwMouseButtonfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwMouseButtonfun ____callback_native_managed;
        System.IntPtr      ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetMouseButtonCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwMouseButtonfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwMouseButtonfun CurrentGlfwMouseButtonfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetCursorPosCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetCursorPosCallback p_glfwSetCursorPosCallback = LoadFunction< d_glfwSetCursorPosCallback >( "glfwSetCursorPosCallback" );

    /// <summary>
    /// <para>This function sets the cursor position callback of the specified window, which is called when the cursor is moved. The callback is provided with the position, in screen coordinates, relative to the upper-left corner of the content area of the window.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, double xpos, double ypos); For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#gad6fae41b3ac2e4209aaa87b596c57f68">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_pos">Cursor position</see></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwSetMousePosCallback</c>. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwCursorposfun glfwSetCursorPosCallback( GlfwWindow* window, GlfwCursorposfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwCursorposfun ____callback_native_managed;
        System.IntPtr    ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetCursorPosCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwCursorposfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwCursorposfun CurrentGlfwCursorPosfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetCursorEnterCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetCursorEnterCallback p_glfwSetCursorEnterCallback = LoadFunction< d_glfwSetCursorEnterCallback >( "glfwSetCursorEnterCallback" );

    /// <summary>
    /// <para>This function sets the cursor boundary crossing callback of the specified window, which is called when the cursor enters or leaves the content area of the window.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int entered) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#gaa93dc4818ac9ab32532909d53a337cbe">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#cursor_enter">Cursor enter/leave events</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwCursorenterfun glfwSetCursorEnterCallback( GlfwWindow* window, GlfwCursorenterfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwCursorenterfun ____callback_native_managed;
        System.IntPtr      ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetCursorEnterCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwCursorenterfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwCursorenterfun CurrentGlfwCursorEnterfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetScrollCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetScrollCallback p_glfwSetScrollCallback = LoadFunction< d_glfwSetScrollCallback >( "glfwSetScrollCallback" );

    /// <summary>
    /// <para>This function sets the scroll callback of the specified window, which is called when a scrolling device is used, such as a mouse wheel or scrolling area of a touchpad.</para> <para>The scroll callback receives all scrolling input, like that from a mouse wheel or a touchpad scrolling area.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, double xoffset, double yoffset) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#gaf656112c33de3efdb227fa58f0134cf5">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#scrolling">Scroll input</see></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwSetMouseWheelCallback</c>. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being garbage collected. You must do that yourself to prevent potential crashes due to glfw calling a garbage collected delegate.
    /// </remarks>
    public static GlfwScrollfun glfwSetScrollCallback( GlfwWindow* window, GlfwScrollfun callback )
    {
        System.IntPtr __callback_native;
        __callback_native = callback != null ? Marshal.GetFunctionPointerForDelegate( callback ) : default;
        GlfwScrollfun ____callback_native_managed;
        System.IntPtr ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetScrollCallback( window, __callback_native );
        }
        System.GC.KeepAlive( __callback_native );
        ____callback_native_managed = ____callback_native_retVal_native != default ? Marshal.PtrToStructure< GlfwScrollfun >( ____callback_native_retVal_native ) : null;

        return ____callback_native_managed;
    }

    internal static GlfwScrollfun CurrentGlfwScrollfun;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetDropCallback( GlfwWindow* window, IntPtr callback );

    private static d_glfwSetDropCallback p_glfwSetDropCallback = LoadFunction< d_glfwSetDropCallback >( "glfwSetDropCallback" );

    /// <summary>
    /// <para>This function sets the path drop callback of the specified window, which is called when one or more dragged paths are dropped on the window.</para> <para>Because the path array and its strings may have been generated specifically for that event, they are not guaranteed to be valid after the callback has returned. If you wish to use them after the callback returns, you need to make a deep copy.</para> <para><b>Returns</b><para>The previously set callback, or <c>NULL</c> if no callback was set or the library had not been <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.</para></para> <para><b>Callback signature</b><para>void function_name(<see cref="GlfwWindow"/>* window, int path_count, const char* paths[]) For more information about the callback parameters, see the <see href="https://www.glfw.org/docs/3.4/group__input.html#gaaba73c3274062c18723b7f05862d94b2">function pointer type</see>.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#path_drop">Path drop input</see></para></para> <para><b>Since</b><para>Added in version 3.1. </para></para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being
    /// garbage collected. You must do that yourself to prevent potential crashes due to glfw calling
    /// a garbage collected delegate.
    /// </remarks>
    public static GlfwDropfun glfwSetDropCallback( GlfwWindow* window, GlfwDropfun callback )
    {
        var __callback_native = Marshal.GetFunctionPointerForDelegate( callback );

        System.IntPtr ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetDropCallback( window, __callback_native );
        }

        System.GC.KeepAlive( __callback_native );

        var ____callback_native_managed = ____callback_native_retVal_native != default
            ? Marshal.PtrToStructure< GlfwDropfun >( ____callback_native_retVal_native )
            : null;

        return ____callback_native_managed!;
    }

    internal static GlfwDropfun CurrentGlfwDropfun = null!;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwJoystickPresent( int jid );

    private static d_glfwJoystickPresent p_glfwJoystickPresent = LoadFunction< d_glfwJoystickPresent >( "glfwJoystickPresent" );

    /// <summary>
    /// <para>This function returns whether the specified joystick is present.</para> <para>There is no need to call this function before other functions that accept a joystick ID, as they all check for presence before performing any other work.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if the joystick is present, or <see cref="NativeGlfw.GLFW_FALSE"/> otherwise.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick">Joystick input</see></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwGetJoystickParam</c>. </para></para>
    /// </summary>
    public static int glfwJoystickPresent( int jid ) => p_glfwJoystickPresent( jid );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetJoystickAxes( int jid, IntPtr count );

    private static d_glfwGetJoystickAxes p_glfwGetJoystickAxes = LoadFunction< d_glfwGetJoystickAxes >( "glfwGetJoystickAxes" );

    /// <summary>
    /// <para>This function returns the values of all axes of the specified joystick. Each element in the array is a value between -1.0 and 1.0.</para> <para>If the specified joystick is not present this function will return <c>NULL</c> but will not generate an error. This can be used instead of first calling <see cref="NativeGlfw.glfwJoystickPresent"/>.</para> <para><b>Returns</b><para>An array of axis values, or <c>NULL</c> if the joystick is not present or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned array is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified joystick is disconnected or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick_axis">Joystick axis states</see></para></para> <para><b>Since</b><para>Added in version 3.0. Replaces <c>glfwGetJoystickPos</c>. </para></para>
    /// </summary>
    public static IntPtr glfwGetJoystickAxes( int jid, IntPtr count ) => p_glfwGetJoystickAxes( jid, count );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetJoystickButtons( int jid, IntPtr count );

    private static d_glfwGetJoystickButtons p_glfwGetJoystickButtons = LoadFunction< d_glfwGetJoystickButtons >( "glfwGetJoystickButtons" );

    /// <summary>
    /// <para>This function returns the state of all buttons of the specified joystick. Each element in the array is either <see cref="NativeGlfw.GLFW_PRESS"/> or <see cref="NativeGlfw.GLFW_RELEASE"/>.</para> <para>For backward compatibility with earlier versions that did not have <see cref="NativeGlfw.glfwGetJoystickHats"/>, the button array also includes all hats, each represented as four buttons. The hats are in the same order as returned by glfwGetJoystickHats and are in the order up, right, down and left. To disable these extra buttons, set the <see cref="NativeGlfw.GLFW_JOYSTICK_HAT_BUTTONS"/> init hint before initialization.</para> <para>If the specified joystick is not present this function will return <c>NULL</c> but will not generate an error. This can be used instead of first calling <see cref="NativeGlfw.glfwJoystickPresent"/>.</para> <para><b>Returns</b><para>An array of button states, or <c>NULL</c> if the joystick is not present or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned array is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified joystick is disconnected or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick_button">Joystick button states</see></para></para> <para><b>Since</b><para>Added in version 2.2. GLFW 3: Changed to return a dynamic array. </para></para>
    /// </summary>
    public static IntPtr glfwGetJoystickButtons( int jid, IntPtr count ) => p_glfwGetJoystickButtons( jid, count );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetJoystickHats( int jid, IntPtr count );

    private static d_glfwGetJoystickHats p_glfwGetJoystickHats = LoadFunction< d_glfwGetJoystickHats >( "glfwGetJoystickHats" );

    /// <summary>
    /// <para>This function returns the state of all hats of the specified joystick. Each element in the array is one of the following values:</para> <para>The diagonal directions are bitwise combinations of the primary (up, right, down and left) directions and you can test for these individually by ANDing it with the corresponding direction.</para> if (hats[2] &amp; <see cref="NativeGlfw.GLFW_HAT_RIGHT"/>) { // State of hat 2 could be right-up, right or right-down } <para>If the specified joystick is not present this function will return <c>NULL</c> but will not generate an error. This can be used instead of first calling <see cref="NativeGlfw.glfwJoystickPresent"/>.</para> <para><b>Returns</b><para>An array of hat states, or <c>NULL</c> if the joystick is not present or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned array is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified joystick is disconnected, this function is called again for that joystick or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick_hat">Joystick hat states</see></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static IntPtr glfwGetJoystickHats( int jid, IntPtr count ) => p_glfwGetJoystickHats( jid, count );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetJoystickName( int jid );

    private static d_glfwGetJoystickName p_glfwGetJoystickName = LoadFunction< d_glfwGetJoystickName >( "glfwGetJoystickName" );

    /// <summary>
    /// <para>This function returns the name, encoded as UTF-8, of the specified joystick. The returned string is allocated and freed by GLFW. You should not free it yourself.</para> <para>If the specified joystick is not present this function will return <c>NULL</c> but will not generate an error. This can be used instead of first calling <see cref="NativeGlfw.glfwJoystickPresent"/>.</para> <para><b>Returns</b><para>The UTF-8 encoded name of the joystick, or <c>NULL</c> if the joystick is not present or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned string is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified joystick is disconnected or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick_name">Joystick name</see></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static IntPtr glfwGetJoystickName( int jid ) => p_glfwGetJoystickName( jid );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetJoystickGUID( int jid );

    private static d_glfwGetJoystickGUID p_glfwGetJoystickGUID = LoadFunction< d_glfwGetJoystickGUID >( "glfwGetJoystickGUID" );

    /// <summary>
    /// <para>This function returns the SDL compatible GUID, as a UTF-8 encoded hexadecimal string, of the specified joystick. The returned string is allocated and freed by GLFW. You should not free it yourself.</para> <para>The GUID is what connects a joystick to a gamepad mapping. A connected joystick will always have a GUID even if there is no gamepad mapping assigned to it.</para> <para>If the specified joystick is not present this function will return <c>NULL</c> but will not generate an error. This can be used instead of first calling <see cref="NativeGlfw.glfwJoystickPresent"/>.</para> <para>The GUID uses the format introduced in SDL 2.0.5. This GUID tries to uniquely identify the make and model of a joystick but does not identify a specific unit, e.g. all wired Xbox 360 controllers will have the same GUID on that platform. The GUID for a unit may vary between platforms depending on what hardware information the platform specific APIs provide.</para> <para><b>Returns</b><para>The UTF-8 encoded GUID of the joystick, or <c>NULL</c> if the joystick is not present or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_INVALID_ENUM"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned string is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified joystick is disconnected or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#gamepad">Gamepad input</see></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static IntPtr glfwGetJoystickGUID( int jid ) => p_glfwGetJoystickGUID( jid );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetJoystickUserPointer( int jid, IntPtr pointer );

    private static d_glfwSetJoystickUserPointer p_glfwSetJoystickUserPointer = LoadFunction< d_glfwSetJoystickUserPointer >( "glfwSetJoystickUserPointer" );

    /// <summary>
    /// <para>This function sets the user-defined pointer of the specified joystick. The current value is retained until the joystick is disconnected. The initial value is <c>NULL</c>.</para> <para>This function may be called from the joystick callback, even for a joystick that is being disconnected.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Access is not synchronized.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick_userptr">Joystick user pointer</see> </para> <para> <see cref="NativeGlfw.glfwGetJoystickUserPointer"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static void glfwSetJoystickUserPointer( int jid, IntPtr pointer ) => p_glfwSetJoystickUserPointer( jid, pointer );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetJoystickUserPointer( int jid );

    private static d_glfwGetJoystickUserPointer p_glfwGetJoystickUserPointer = LoadFunction< d_glfwGetJoystickUserPointer >( "glfwGetJoystickUserPointer" );

    /// <summary>
    /// <para>This function returns the current value of the user-defined pointer of the specified joystick. The initial value is <c>NULL</c>.</para> <para>This function may be called from the joystick callback, even for a joystick that is being disconnected.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Access is not synchronized.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick_userptr">Joystick user pointer</see> </para> <para> <see cref="NativeGlfw.glfwSetJoystickUserPointer"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static IntPtr glfwGetJoystickUserPointer( int jid ) => p_glfwGetJoystickUserPointer( jid );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwJoystickIsGamepad( int jid );

    private static d_glfwJoystickIsGamepad p_glfwJoystickIsGamepad = LoadFunction< d_glfwJoystickIsGamepad >( "glfwJoystickIsGamepad" );

    /// <summary>
    /// <para>This function returns whether the specified joystick is both present and has a gamepad mapping.</para> <para>If the specified joystick is present but does not have a gamepad mapping this function will return <see cref="NativeGlfw.GLFW_FALSE"/> but will not generate an error. Call <see cref="NativeGlfw.glfwJoystickPresent"/> to check if a joystick is present regardless of whether it has a mapping.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if a joystick is both present and has a gamepad mapping, or <see cref="NativeGlfw.GLFW_FALSE"/> otherwise.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#gamepad">Gamepad input</see> </para> <para> <see cref="NativeGlfw.glfwGetGamepadState"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static int glfwJoystickIsGamepad( int jid ) => p_glfwJoystickIsGamepad( jid );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwSetJoystickCallback( IntPtr callback );

    private static d_glfwSetJoystickCallback p_glfwSetJoystickCallback = LoadFunction< d_glfwSetJoystickCallback >( "glfwSetJoystickCallback" );

    /// <summary>
    /// This function sets the joystick configuration callback, or removes the currently set callback.
    /// This is called when a joystick is connected to or disconnected from the system.
    /// <para>
    /// For joystick connection and disconnection events to be delivered on all platforms, you need
    /// to call one of the <see href="https://www.glfw.org/docs/3.4/input_guide.html#events">event processing</see>
    /// functions. Joystick disconnection may also be detected and the callback called by joystick functions.
    /// The function will then return whatever it returns if the joystick is not present.
    /// </para>
    /// <para>
    /// <b>Returns</b>
    /// <para>
    /// The previously set callback, or <c>NULL</c> if no callback was set or the library had not been
    /// <see href="https://www.glfw.org/docs/3.4/intro_guide.html#intro_init">initialized</see>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Callback signature</b>
    /// <para>
    /// void function_name(int jid, int event). For more information about the callback parameters, see
    /// the <see href="https://www.glfw.org/docs/3.4/group__input.html#gaa21ad5986ae9a26077a40142efb56243">function pointer type</see>
    /// </para>
    /// </para>
    /// <para>
    /// <b>Errors</b>
    /// <para>
    /// Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.
    /// </para>
    /// </para>
    /// <para>
    /// <b>Thread safety</b>
    /// <para>
    /// This function must only be called from the main thread.
    /// </para>
    /// </para>
    /// <para>
    /// <b>See also</b>
    /// <para>
    /// <see href="https://www.glfw.org/docs/3.4/input_guide.html#joystick_event">Joystick configuration changes</see>
    /// </para>
    /// </para>
    /// </summary>
    /// <remarks>
    /// WARNING! Be aware that the supplied callback is NOT cached in any way to prevent it from being
    /// garbage collected. You must do that yourself to prevent potential crashes due to glfw calling
    /// a garbage collected delegate.
    /// </remarks>
    public static GlfwJoystickfun glfwSetJoystickCallback( GlfwJoystickfun callback )
    {
        var __callback_native = Marshal.GetFunctionPointerForDelegate( callback );

        System.IntPtr ____callback_native_retVal_native;
        {
            ____callback_native_retVal_native = p_glfwSetJoystickCallback( __callback_native );
        }

        System.GC.KeepAlive( __callback_native );

        var ____callback_native_managed = ( ____callback_native_retVal_native != default )
            ? Marshal.PtrToStructure< GlfwJoystickfun >( ____callback_native_retVal_native )
            : null;

        return ____callback_native_managed!;
    }

    internal static GlfwJoystickfun CurrentGlfwJoystickfun = null!;

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwUpdateGamepadMappings( IntPtr @string );

    private static d_glfwUpdateGamepadMappings p_glfwUpdateGamepadMappings = LoadFunction< d_glfwUpdateGamepadMappings >( "glfwUpdateGamepadMappings" );

    /// <summary>
    /// <para>This function parses the specified ASCII encoded string and updates the internal list with any gamepad mappings it finds. This string may contain either a single gamepad mapping or many mappings separated by newlines. The parser supports the full format of the <c>gamecontrollerdb.txt</c> source file including empty lines and comments.</para> <para>See <see href="https://www.glfw.org/docs/3.4/input_guide.html#gamepad_mapping">Gamepad mappings</see> for a description of the format.</para> <para>If there is already a gamepad mapping for a given GUID in the internal list, it will be replaced by the one passed to this function. If the library is terminated and re-initialized the internal list will revert to the built-in default.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if successful, or <see cref="NativeGlfw.GLFW_FALSE"/> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#gamepad">Gamepad input</see> </para> <para> <see cref="NativeGlfw.glfwJoystickIsGamepad"/> </para> <para> <see cref="NativeGlfw.glfwGetGamepadName"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static int glfwUpdateGamepadMappings( IntPtr @string ) => p_glfwUpdateGamepadMappings( @string );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetGamepadName( int jid );

    private static d_glfwGetGamepadName p_glfwGetGamepadName = LoadFunction< d_glfwGetGamepadName >( "glfwGetGamepadName" );

    /// <summary>
    /// <para>This function returns the human-readable name of the gamepad from the gamepad mapping assigned to the specified joystick.</para> <para>If the specified joystick is not present or does not have a gamepad mapping this function will return <c>NULL</c> but will not generate an error. Call <see cref="NativeGlfw.glfwJoystickPresent"/> to check whether it is present regardless of whether it has a mapping.</para> <para><b>Returns</b><para>The UTF-8 encoded name of the gamepad, or <c>NULL</c> if the joystick is not present, does not have a mapping or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned string is allocated and freed by GLFW. You should not free it yourself. It is valid until the specified joystick is disconnected, the gamepad mappings are updated or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#gamepad">Gamepad input</see> </para> <para> <see cref="NativeGlfw.glfwJoystickIsGamepad"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static IntPtr glfwGetGamepadName( int jid ) => p_glfwGetGamepadName( jid );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetGamepadState( int jid, GlfwGamepadState* state );

    private static d_glfwGetGamepadState p_glfwGetGamepadState = LoadFunction< d_glfwGetGamepadState >( "glfwGetGamepadState" );

    /// <summary>
    /// <para>This function retrieves the state of the specified joystick remapped to an Xbox-like gamepad.</para> <para>If the specified joystick is not present or does not have a gamepad mapping this function will return <see cref="NativeGlfw.GLFW_FALSE"/> but will not generate an error. Call <see cref="NativeGlfw.glfwJoystickPresent"/> to check whether it is present regardless of whether it has a mapping.</para> <para>The Guide button may not be available for input as it is often hooked by the system or the Steam client.</para> <para>Not all devices have all the buttons or axes provided by <see cref="GlfwGamepadState"/>. Unavailable buttons and axes will always report <see cref="NativeGlfw.GLFW_RELEASE"/> and 0.0 respectively.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if successful, or <see cref="NativeGlfw.GLFW_FALSE"/> if no joystick is connected, it has no gamepad mapping or an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_ENUM"/>.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#gamepad">Gamepad input</see> </para> <para> <see cref="NativeGlfw.glfwUpdateGamepadMappings"/> </para> <para> <see cref="NativeGlfw.glfwJoystickIsGamepad"/></para></para> <para><b>Since</b><para>Added in version 3.3. </para></para>
    /// </summary>
    public static int glfwGetGamepadState( int jid, GlfwGamepadState* state ) => p_glfwGetGamepadState( jid, state );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetClipboardString( GlfwWindow* window, IntPtr @string );

    private static d_glfwSetClipboardString p_glfwSetClipboardString = LoadFunction< d_glfwSetClipboardString >( "glfwSetClipboardString" );

    /// <summary>
    /// <para>This function sets the system clipboard to the specified, UTF-8 encoded string.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>Windows: The clipboard on Windows has a single global lock for reading and writing. GLFW tries to acquire it a few times, which is almost always enough. If it cannot acquire the lock then this function emits <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and returns. It is safe to try this multiple times.</para></para> <para><b>Pointer lifetime</b><para>The specified string is copied before this function returns.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#clipboard">Clipboard input and output</see> </para> <para> <see cref="NativeGlfw.glfwGetClipboardString"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwSetClipboardString( GlfwWindow* window, IntPtr @string ) => p_glfwSetClipboardString( window, @string );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetClipboardString( GlfwWindow* window );

    private static d_glfwGetClipboardString p_glfwGetClipboardString = LoadFunction< d_glfwGetClipboardString >( "glfwGetClipboardString" );

    /// <summary>
    /// <para>This function returns the contents of the system clipboard, if it contains or is convertible to a UTF-8 encoded string. If the clipboard is empty or if its contents cannot be converted, <c>NULL</c> is returned and a <see cref="NativeGlfw.GLFW_FORMAT_UNAVAILABLE"/> error is generated.</para> <para><b>Returns</b><para>The contents of the clipboard as a UTF-8 encoded string, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_FORMAT_UNAVAILABLE"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>Windows: The clipboard on Windows has a single global lock for reading and writing. GLFW tries to acquire it a few times, which is almost always enough. If it cannot acquire the lock then this function emits <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and returns. It is safe to try this multiple times.</para></para> <para><b>Pointer lifetime</b><para>The returned string is allocated and freed by GLFW. You should not free it yourself. It is valid until the next call to <see cref="NativeGlfw.glfwGetClipboardString"/> or <see cref="NativeGlfw.glfwSetClipboardString"/>, or until the library is terminated.</para></para> <para><b>Thread safety</b><para>This function must only be called from the main thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#clipboard">Clipboard input and output</see> </para> <para> <see cref="NativeGlfw.glfwSetClipboardString"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static IntPtr glfwGetClipboardString( GlfwWindow* window ) => p_glfwGetClipboardString( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate double d_glfwGetTime();

    private static d_glfwGetTime p_glfwGetTime = LoadFunction< d_glfwGetTime >( "glfwGetTime" );

    /// <summary>
    /// <para>This function returns the current GLFW time, in seconds. Unless the time has been set using <see cref="NativeGlfw.glfwSetTime"/> it measures time elapsed since GLFW was initialized.</para> <para>This function and <see cref="NativeGlfw.glfwSetTime"/> are helper functions on top of <see cref="NativeGlfw.glfwGetTimerFrequency"/> and <see cref="NativeGlfw.glfwGetTimerValue"/>.</para> <para>The resolution of the timer is system dependent, but is usually on the order of a few micro- or nanoseconds. It uses the highest-resolution monotonic time source on each operating system.</para> <para><b>Returns</b><para>The current time, in seconds, or zero if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Reading and writing of the internal base time is not atomic, so it needs to be externally synchronized with calls to <see cref="NativeGlfw.glfwSetTime"/>.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#time">Time input</see></para></para> <para><b>Since</b><para>Added in version 1.0. </para></para>
    /// </summary>
    public static double glfwGetTime() => p_glfwGetTime();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSetTime( double time );

    private static d_glfwSetTime p_glfwSetTime = LoadFunction< d_glfwSetTime >( "glfwSetTime" );

    /// <summary>
    /// <para>This function sets the current GLFW time, in seconds. The value must be a positive finite number less than or equal to 18446744073.0, which is approximately 584.5 years.</para> <para>This function and <see cref="NativeGlfw.glfwGetTime"/> are helper functions on top of <see cref="NativeGlfw.glfwGetTimerFrequency"/> and <see cref="NativeGlfw.glfwGetTimerValue"/>.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_INVALID_VALUE"/>.</para></para> <para><b>Remarks</b><para>The upper limit of GLFW time is calculated as floor((264 - 1) / 109) and is due to implementations storing nanoseconds in 64 bits. The limit may be increased in the future.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. Reading and writing of the internal base time is not atomic, so it needs to be externally synchronized with calls to <see cref="NativeGlfw.glfwGetTime"/>.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#time">Time input</see></para></para> <para><b>Since</b><para>Added in version 2.2. </para></para>
    /// </summary>
    public static void glfwSetTime( double time ) => p_glfwSetTime( time );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate ulong d_glfwGetTimerValue();

    private static d_glfwGetTimerValue p_glfwGetTimerValue = LoadFunction< d_glfwGetTimerValue >( "glfwGetTimerValue" );

    /// <summary>
    /// <para>This function returns the current value of the raw timer, measured in 1&#160;/&#160;frequency seconds. To get the frequency, call <see cref="NativeGlfw.glfwGetTimerFrequency"/>.</para> <para><b>Returns</b><para>The value of the timer, or zero if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#time">Time input</see> </para> <para> <see cref="NativeGlfw.glfwGetTimerFrequency"/></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static ulong glfwGetTimerValue() => p_glfwGetTimerValue();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate ulong d_glfwGetTimerFrequency();

    private static d_glfwGetTimerFrequency p_glfwGetTimerFrequency = LoadFunction< d_glfwGetTimerFrequency >( "glfwGetTimerFrequency" );

    /// <summary>
    /// <para>This function returns the frequency, in Hz, of the raw timer.</para> <para><b>Returns</b><para>The frequency of the timer, in Hz, or zero if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/input_guide.html#time">Time input</see> </para> <para> <see cref="NativeGlfw.glfwGetTimerValue"/></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static ulong glfwGetTimerFrequency() => p_glfwGetTimerFrequency();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwMakeContextCurrent( GlfwWindow* window );

    private static d_glfwMakeContextCurrent p_glfwMakeContextCurrent = LoadFunction< d_glfwMakeContextCurrent >( "glfwMakeContextCurrent" );

    /// <summary>
    /// <para>This function makes the OpenGL or OpenGL ES context of the specified window current on the calling thread. It can also detach the current context from the calling thread without making a new one current by passing in <c>NULL</c>.</para> <para>A context must only be made current on a single thread at a time and each thread can have only a single current context at a time. Making a context current detaches any previously current context on the calling thread.</para> <para>When moving a context between threads, you must detach it (make it non-current) on the old thread before making it current on the new one.</para> <para>By default, making a context non-current implicitly forces a pipeline flush. On machines that support <c>GL_KHR_context_flush_control</c>, you can control whether a context performs this flush by setting the <see cref="NativeGlfw.GLFW_CONTEXT_RELEASE_BEHAVIOR"/> hint.</para> <para>The specified window must have an OpenGL or OpenGL ES context. Specifying a window without a context will generate a <see cref="NativeGlfw.GLFW_NO_WINDOW_CONTEXT"/> error.</para> <para><b>Remarks</b><para>If the previously current context was created via a different context creation API than the one passed to this function, GLFW will still detach the previous one from its API before making the new one current.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_NO_WINDOW_CONTEXT"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/context_guide.html#context_current">Current context</see> </para> <para> <see cref="NativeGlfw.glfwGetCurrentContext"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static void glfwMakeContextCurrent( GlfwWindow* window ) => p_glfwMakeContextCurrent( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate GlfwWindow* d_glfwGetCurrentContext();

    private static d_glfwGetCurrentContext p_glfwGetCurrentContext = LoadFunction< d_glfwGetCurrentContext >( "glfwGetCurrentContext" );

    /// <summary>
    /// <para>This function returns the window whose OpenGL or OpenGL ES context is current on the calling thread.</para> <para><b>Returns</b><para>The window whose context is current, or <c>NULL</c> if no window's context is current.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/context_guide.html#context_current">Current context</see> </para> <para> <see cref="NativeGlfw.glfwMakeContextCurrent"/></para></para> <para><b>Since</b><para>Added in version 3.0. </para></para>
    /// </summary>
    public static GlfwWindow* glfwGetCurrentContext() => p_glfwGetCurrentContext();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSwapBuffers( GlfwWindow* window );

    private static d_glfwSwapBuffers p_glfwSwapBuffers = LoadFunction< d_glfwSwapBuffers >( "glfwSwapBuffers" );

    /// <summary>
    /// <para>This function swaps the front and back buffers of the specified window when rendering with OpenGL or OpenGL ES. If the swap interval is greater than zero, the GPU driver waits the specified number of screen updates before swapping the buffers.</para> <para>The specified window must have an OpenGL or OpenGL ES context. Specifying a window without a context will generate a <see cref="NativeGlfw.GLFW_NO_WINDOW_CONTEXT"/> error.</para> <para>This function does not apply to Vulkan. If you are rendering with Vulkan, see <c>vkQueuePresentKHR</c> instead.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_NO_WINDOW_CONTEXT"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>EGL: The context of the specified window must be current on the calling thread.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#buffer_swap">Buffer swapping</see> </para> <para> <see cref="NativeGlfw.glfwSwapInterval"/></para></para> <para><b>Since</b><para>Added in version 1.0. GLFW 3: Added window handle parameter. </para></para>
    /// </summary>
    public static void glfwSwapBuffers( GlfwWindow* window ) => p_glfwSwapBuffers( window );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate void d_glfwSwapInterval( int interval );

    private static d_glfwSwapInterval p_glfwSwapInterval = LoadFunction< d_glfwSwapInterval >( "glfwSwapInterval" );

    /// <summary>
    /// <para>This function sets the swap interval for the current OpenGL or OpenGL ES context, i.e. the number of screen updates to wait from the time <see cref="NativeGlfw.glfwSwapBuffers"/> was called before swapping the buffers and returning. This is sometimes called vertical synchronization, vertical retrace synchronization or just vsync.</para> <para>A context that supports either of the <c>WGL_EXT_swap_control_tear</c> and <c>GLX_EXT_swap_control_tear</c> extensions also accepts negative swap intervals, which allows the driver to swap immediately even if a frame arrives a little bit late. You can check for these extensions with <see cref="NativeGlfw.glfwExtensionSupported"/>.</para> <para>A context must be current on the calling thread. Calling this function without a current context will cause a <see cref="NativeGlfw.GLFW_NO_CURRENT_CONTEXT"/> error.</para> <para>This function does not apply to Vulkan. If you are rendering with Vulkan, see the present mode of your swapchain instead.</para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_NO_CURRENT_CONTEXT"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>This function is not called during context creation, leaving the swap interval set to whatever is the default for that API. This is done because some swap interval extensions used by GLFW do not allow the swap interval to be reset to zero once it has been set to a non-zero value.</para> <para>Some GPU drivers do not honor the requested swap interval, either because of a user setting that overrides the application's request or due to bugs in the driver.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/window_guide.html#buffer_swap">Buffer swapping</see> </para> <para> <see cref="NativeGlfw.glfwSwapBuffers"/></para></para> <para><b>Since</b><para>Added in version 1.0. </para></para>
    /// </summary>
    public static void glfwSwapInterval( int interval ) => p_glfwSwapInterval( interval );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwExtensionSupported( IntPtr extension );

    private static d_glfwExtensionSupported p_glfwExtensionSupported = LoadFunction< d_glfwExtensionSupported >( "glfwExtensionSupported" );

    /// <summary>
    /// <para>This function returns whether the specified <see href="https://www.glfw.org/docs/3.4/context_guide.html#context_glext">API extension</see> is supported by the current OpenGL or OpenGL ES context. It searches both for client API extension and context creation API extensions.</para> <para>A context must be current on the calling thread. Calling this function without a current context will cause a <see cref="NativeGlfw.GLFW_NO_CURRENT_CONTEXT"/> error.</para> <para>As this functions retrieves and searches one or more extension strings each call, it is recommended that you cache its results if it is going to be used frequently. The extension strings will not change during the lifetime of a context, so there is no danger in doing this.</para> <para>This function does not apply to Vulkan. If you are using Vulkan, see <see cref="NativeGlfw.glfwGetRequiredInstanceExtensions"/>, <c>vkEnumerateInstanceExtensionProperties</c> and <c>vkEnumerateDeviceExtensionProperties</c> instead.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if the extension is available, or <see cref="NativeGlfw.GLFW_FALSE"/> otherwise.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_NO_CURRENT_CONTEXT"/>, <see cref="NativeGlfw.GLFW_INVALID_VALUE"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/context_guide.html#context_glext">OpenGL and OpenGL ES extensions</see> </para> <para> <see cref="NativeGlfw.glfwGetProcAddress"/></para></para> <para><b>Since</b><para>Added in version 1.0. </para></para>
    /// </summary>
    public static int glfwExtensionSupported( IntPtr extension ) => p_glfwExtensionSupported( extension );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetProcAddress( IntPtr procname );

    private static d_glfwGetProcAddress p_glfwGetProcAddress = LoadFunction< d_glfwGetProcAddress >( "glfwGetProcAddress" );

    /// <summary>
    /// <para>This function returns the address of the specified OpenGL or OpenGL ES <see href="https://www.glfw.org/docs/3.4/context_guide.html#context_glext">core or extension function</see>, if it is supported by the current context.</para> <para>A context must be current on the calling thread. Calling this function without a current context will cause a <see cref="NativeGlfw.GLFW_NO_CURRENT_CONTEXT"/> error.</para> <para>This function does not apply to Vulkan. If you are rendering with Vulkan, see <see cref="NativeGlfw.glfwGetInstanceProcAddress"/>, <c>vkGetInstanceProcAddr</c> and <c>vkGetDeviceProcAddr</c> instead.</para> <para><b>Returns</b><para>The address of the function, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_NO_CURRENT_CONTEXT"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>The address of a given function is not guaranteed to be the same between contexts.</para> <para>This function may return a non-<c>NULL</c> address despite the associated version or extension not being available. Always check the context version or extension string first.</para></para> <para><b>Pointer lifetime</b><para>The returned function pointer is valid until the context is destroyed or the library is terminated.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/context_guide.html#context_glext">OpenGL and OpenGL ES extensions</see> </para> <para> <see cref="NativeGlfw.glfwExtensionSupported"/></para></para> <para><b>Since</b><para>Added in version 1.0. </para></para>
    /// </summary>
    public static IntPtr glfwGetProcAddress( IntPtr procname ) => p_glfwGetProcAddress( procname );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwVulkanSupported();

    private static d_glfwVulkanSupported p_glfwVulkanSupported = LoadFunction< d_glfwVulkanSupported >( "glfwVulkanSupported" );

    /// <summary>
    /// <para>This function returns whether the Vulkan loader and any minimally functional ICD have been found.</para> <para>The availability of a Vulkan loader and even an ICD does not by itself guarantee that surface creation or even instance creation is possible. Call <see cref="NativeGlfw.glfwGetRequiredInstanceExtensions"/> to check whether the extensions necessary for Vulkan surface creation are available and <see cref="NativeGlfw.glfwGetPhysicalDevicePresentationSupport"/> to check whether a queue family of a physical device supports image presentation.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if Vulkan is minimally available, or <see cref="NativeGlfw.GLFW_FALSE"/> otherwise.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/vulkan_guide.html#vulkan_support">Querying for Vulkan support</see></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static int glfwVulkanSupported() => p_glfwVulkanSupported();

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetRequiredInstanceExtensions( IntPtr count );

    private static d_glfwGetRequiredInstanceExtensions p_glfwGetRequiredInstanceExtensions = LoadFunction< d_glfwGetRequiredInstanceExtensions >( "glfwGetRequiredInstanceExtensions" );

    /// <summary>
    /// <para>This function returns an array of names of Vulkan instance extensions required by GLFW for creating Vulkan surfaces for GLFW windows. If successful, the list will always contain <c>VK_KHR_surface</c>, so if you don't require any additional extensions you can pass this list directly to the <c>VkInstanceCreateInfo</c> struct.</para> <para>If Vulkan is not available on the machine, this function returns <c>NULL</c> and generates a <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/> error. Call <see cref="NativeGlfw.glfwVulkanSupported"/> to check whether Vulkan is at least minimally available.</para> <para>If Vulkan is available but no set of extensions allowing window surface creation was found, this function returns <c>NULL</c>. You may still use Vulkan for off-screen rendering and compute work.</para> <para><b>Returns</b><para>An array of ASCII encoded extension names, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/>.</para></para> <para><b>Remarks</b><para>Additional extensions may be required by future versions of GLFW. You should check if any extensions you wish to enable are already in the returned array, as it is an error to specify an extension more than once in the <c>VkInstanceCreateInfo</c> struct.</para></para> <para><b>Pointer lifetime</b><para>The returned array is allocated and freed by GLFW. You should not free it yourself. It is guaranteed to be valid only until the library is terminated.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/vulkan_guide.html#vulkan_ext">Querying required Vulkan extensions</see> </para> <para> <see cref="NativeGlfw.glfwCreateWindowSurface"/></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static IntPtr glfwGetRequiredInstanceExtensions( IntPtr count ) => p_glfwGetRequiredInstanceExtensions( count );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate IntPtr d_glfwGetInstanceProcAddress( IntPtr instance, IntPtr procname );

    private static d_glfwGetInstanceProcAddress p_glfwGetInstanceProcAddress = LoadFunction< d_glfwGetInstanceProcAddress >( "glfwGetInstanceProcAddress" );

    /// <summary>
    /// <para>This function returns the address of the specified Vulkan core or extension function for the specified instance. If instance is set to <c>NULL</c> it can return any function exported from the Vulkan loader, including at least the following functions:</para> <c>vkEnumerateInstanceExtensionProperties</c> <c>vkEnumerateInstanceLayerProperties</c> <c>vkCreateInstance</c> <c>vkGetInstanceProcAddr</c> <para>If Vulkan is not available on the machine, this function returns <c>NULL</c> and generates a <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/> error. Call <see cref="NativeGlfw.glfwVulkanSupported"/> to check whether Vulkan is at least minimally available.</para> <para>This function is equivalent to calling <c>vkGetInstanceProcAddr</c> with a platform-specific query of the Vulkan loader as a fallback.</para> <para><b>Returns</b><para>The address of the function, or <c>NULL</c> if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/> and <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/>.</para></para> <para><b>Pointer lifetime</b><para>The returned function pointer is valid until the library is terminated.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/vulkan_guide.html#vulkan_proc">Querying Vulkan function pointers</see></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static IntPtr glfwGetInstanceProcAddress( IntPtr instance, IntPtr procname ) => p_glfwGetInstanceProcAddress( instance, procname );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwGetPhysicalDevicePresentationSupport( IntPtr instance, IntPtr device, uint queuefamily );

    private static d_glfwGetPhysicalDevicePresentationSupport p_glfwGetPhysicalDevicePresentationSupport = LoadFunction< d_glfwGetPhysicalDevicePresentationSupport >( "glfwGetPhysicalDevicePresentationSupport" );

    /// <summary>
    /// <para>This function returns whether the specified queue family of the specified physical device supports presentation to the platform GLFW was built for.</para> <para>If Vulkan or the required window surface creation instance extensions are not available on the machine, or if the specified instance was not created with the required extensions, this function returns <see cref="NativeGlfw.GLFW_FALSE"/> and generates a <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/> error. Call <see cref="NativeGlfw.glfwVulkanSupported"/> to check whether Vulkan is at least minimally available and <see cref="NativeGlfw.glfwGetRequiredInstanceExtensions"/> to check what instance extensions are required.</para> <para><b>Returns</b><para><see cref="NativeGlfw.GLFW_TRUE"/> if the queue family supports presentation, or <see cref="NativeGlfw.GLFW_FALSE"/> otherwise.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/> and <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/>.</para></para> <para><b>Remarks</b><para>macOS: This function currently always returns <see cref="NativeGlfw.GLFW_TRUE"/>, as the <c>VK_MVK_macos_surface</c> and <c>VK_EXT_metal_surface</c> extensions do not provide a <c>vkGetPhysicalDevice*PresentationSupport</c> type function.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. For synchronization details of Vulkan objects, see the Vulkan specification.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/vulkan_guide.html#vulkan_present">Querying for Vulkan presentation support</see></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static int glfwGetPhysicalDevicePresentationSupport( IntPtr instance, IntPtr device, uint queuefamily ) => p_glfwGetPhysicalDevicePresentationSupport( instance, device, queuefamily );

    // ========================================================================

    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    private delegate int d_glfwCreateWindowSurface( IntPtr instance, GlfwWindow* window, IntPtr allocator, IntPtr surface );

    private static d_glfwCreateWindowSurface p_glfwCreateWindowSurface = LoadFunction< d_glfwCreateWindowSurface >( "glfwCreateWindowSurface" );

    /// <summary>
    /// <para>This function creates a Vulkan surface for the specified window.</para> <para>If the Vulkan loader or at least one minimally functional ICD were not found, this function returns <c>VK_ERROR_INITIALIZATION_FAILED</c> and generates a <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/> error. Call <see cref="NativeGlfw.glfwVulkanSupported"/> to check whether Vulkan is at least minimally available.</para> <para>If the required window surface creation instance extensions are not available or if the specified instance was not created with these extensions enabled, this function returns <c>VK_ERROR_EXTENSION_NOT_PRESENT</c> and generates a <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/> error. Call <see cref="NativeGlfw.glfwGetRequiredInstanceExtensions"/> to check what instance extensions are required.</para> <para>The window surface cannot be shared with another API so the window must have been created with the <see href="https://www.glfw.org/docs/3.4/window_guide.html#GLFW_CLIENT_API_attrib">client api hint</see> set to <see cref="NativeGlfw.GLFW_NO_API"/> otherwise it generates a <see cref="NativeGlfw.GLFW_INVALID_VALUE"/> error and returns <c>VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</c>.</para> <para>The window surface must be destroyed before the specified Vulkan instance. It is the responsibility of the caller to destroy the window surface. GLFW does not destroy it for you. Call <c>vkDestroySurfaceKHR</c> to destroy the surface.</para> <para><b>Returns</b><para><c>VK_SUCCESS</c> if successful, or a Vulkan error code if an <see href="https://www.glfw.org/docs/3.4/intro_guide.html#error_handling">error</see> occurred.</para></para> <para><b>Errors</b><para>Possible errors include <see cref="NativeGlfw.GLFW_NOT_INITIALIZED"/>, <see cref="NativeGlfw.GLFW_API_UNAVAILABLE"/>, <see cref="NativeGlfw.GLFW_PLATFORM_ERROR"/> and <see cref="NativeGlfw.GLFW_INVALID_VALUE"/></para></para> <para><b>Remarks</b><para>If an error occurs before the creation call is made, GLFW returns the Vulkan error code most appropriate for the error. Appropriate use of <see cref="NativeGlfw.glfwVulkanSupported"/> and <see cref="NativeGlfw.glfwGetRequiredInstanceExtensions"/> should eliminate almost all occurrences of these errors.</para> <para> macOS: GLFW prefers the <c>VK_EXT_metal_surface</c> extension, with the <c>VK_MVK_macos_surface</c> extension as a fallback. The name of the selected extension, if any, is included in the array returned by <see cref="NativeGlfw.glfwGetRequiredInstanceExtensions"/>.</para> <para> macOS: This function creates and sets a <c>CAMetalLayer</c> instance for the window content view, which is required for MoltenVK to function.</para> <para> X11: By default GLFW prefers the <c>VK_KHR_xcb_surface</c> extension, with the <c>VK_KHR_xlib_surface</c> extension as a fallback. You can make <c>VK_KHR_xlib_surface</c> the preferred extension by setting the <see cref="NativeGlfw.GLFW_X11_XCB_VULKAN_SURFACE"/> init hint. The name of the selected extension, if any, is included in the array returned by <see cref="NativeGlfw.glfwGetRequiredInstanceExtensions"/>.</para></para> <para><b>Thread safety</b><para>This function may be called from any thread. For synchronization details of Vulkan objects, see the Vulkan specification.</para></para> <para><b>See also</b><para><see href="https://www.glfw.org/docs/3.4/vulkan_guide.html#vulkan_surface">Creating a Vulkan window surface</see> </para> <para> <see cref="NativeGlfw.glfwGetRequiredInstanceExtensions"/></para></para> <para><b>Since</b><para>Added in version 3.2. </para></para>
    /// </summary>
    public static int glfwCreateWindowSurface( IntPtr instance, GlfwWindow* window, IntPtr allocator, IntPtr surface ) => p_glfwCreateWindowSurface( instance, window, allocator, surface );
}