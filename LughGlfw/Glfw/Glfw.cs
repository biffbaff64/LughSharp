﻿// /////////////////////////////////////////////////////////////////////////////
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

using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;

using LughGlfw.Glfw.Enums;
using LughGlfw.Glfw.Structs;

using static LughGlfw.Glfw.NativeGlfw;

using ConnectionState = LughGlfw.Glfw.Enums.ConnectionState;

namespace LughGlfw.Glfw;

/// <summary>
/// A .NET wrapper around the native GLFW library with convenient and safe access
/// to the native functions.
/// </summary>
[PublicAPI]
public static class Glfw
{
    private static TM SetCallback< TM, TU >( ref TM? managedField,
                                             ref TU? nativeField,
                                             TM? newManaged,
                                             TU? newNative,
                                             Action< TU > setter ) where TM : Delegate where TU : Delegate
    {
        var oldManaged = managedField;
        managedField = newManaged!;

        if ( newManaged is null )
        {
            nativeField = null;
            setter( null );
        }
        else
        {
            nativeField = newNative;
            setter( newNative );
        }

        if ( oldManaged == null )
        {
            throw new InvalidOperationException( "The managed field is not set." );
        }
        
        return oldManaged;
    }

    private static T[] MarshalCopyArrayOf< T >( IntPtr ptr, int count ) where T : struct
    {
        if ( ptr == IntPtr.Zero )
        {
            return [ ];
        }

        var array = new T[ count ];

        for ( var i = 0; i < count; i++ )
        {
            array[ i ] = ( T )Marshal.PtrToStructure< T >( ptr + i * Marshal.SizeOf< T >() );
        }

        return array;
    }

    private static IntPtr MarshalManagedArrayToUnmanaged< T >( T[] array ) where T : struct
    {
        var ptr = Marshal.AllocHGlobal( Marshal.SizeOf< T >() * array.Length );

        for ( var i = 0; i < array.Length; i++ )
        {
            Marshal.StructureToPtr( array[ i ], ptr + i * Marshal.SizeOf< T >(), false );
        }

        return ptr;
    }

    private static void MarshalStringAndFree( Encoding enc, string str, Action< nint > withStr )
    {
        var  bytes   = enc.GetBytes( str );
        var pointer = Marshal.AllocHGlobal( bytes.Length + 1 );
        Marshal.Copy( bytes, 0, pointer, bytes.Length );
        Marshal.WriteByte( pointer, bytes.Length, 0 );
        withStr( pointer );
        Marshal.FreeHGlobal( pointer );
    }

    private static T MarshalStringAndFree< T >( Encoding enc, string str, Func< nint, T > withStr )
    {
        var  bytes   = enc.GetBytes( str );
        var pointer = Marshal.AllocHGlobal( bytes.Length + 1 );
        Marshal.Copy( bytes, 0, pointer, bytes.Length );
        Marshal.WriteByte( pointer, bytes.Length, 0 );
        var result = withStr( pointer );
        Marshal.FreeHGlobal( pointer );

        return result;
    }

    private static string CopyStringFromUnmanaged( nint ptr, Encoding encoding )
    {
        if ( ptr == IntPtr.Zero )
        {
            return string.Empty;
        }

        var length = 0;
        while ( Marshal.ReadByte( ptr, length ) != 0 )
        {
            length++;
        }

        var buffer = new byte[ length ];
        Marshal.Copy( ptr, buffer, 0, length );

        return encoding.GetString( buffer );
    }

    /// <inheritdoc cref="glfwInit"/>
    public static bool Init() => glfwInit() == GLFW_TRUE;

    /// <inheritdoc cref="glfwTerminate"/>
    public static void Terminate() => glfwTerminate();

    /// <inheritdoc cref="glfwInitHint"/>
    /// <param name="hint">The hint to set. Use the static fields in <see cref="InitHint"/>.</param>
    /// <param name="value">The value to set the hint to.</param>
    public static void InitHint< T >( InitHint< T > hint, T value ) where T : struct
    {
        var newValue = Convert.ToInt32( value );
        glfwInitHint( hint.Hint, newValue );
    }

    /// <inheritdoc cref="glfwInitAllocator"/>
    public static unsafe void InitAllocator( Allocator allocator )
    {
        if ( allocator is null )
        {
            glfwInitAllocator( null );

            return;
        }

        var allocatePtr   = Marshal.GetFunctionPointerForDelegate( allocator.Allocate );
        var reallocatePtr = Marshal.GetFunctionPointerForDelegate( allocator.Reallocate );
        var deallocatePtr = Marshal.GetFunctionPointerForDelegate( allocator.Deallocate );

        var glfwAllocator = new NativeGlfw.GLFWallocator
        {
            allocate   = allocatePtr,
            reallocate = reallocatePtr,
            deallocate = deallocatePtr,
            user       = allocator.User
        };

        var ptr = Marshal.AllocHGlobal( Marshal.SizeOf< GLFWallocator >() );
        Marshal.StructureToPtr( glfwAllocator, ptr, false );
        glfwInitAllocator( ( GLFWallocator* )ptr );
        Marshal.FreeHGlobal( ptr );
    }

    /// <inheritdoc cref="glfwGetVersion"/>
    public static void InitVulkanLoader( PFN_vkGetInstanceProcAddr loader ) => glfwInitVulkanLoader( loader );

    /// <inheritdoc cref="glfwGetVersion"/>
    public static unsafe void GetVersion( out int major, out int minor, out int revision )
    {
        int uMajor, uMinor, uRevision;
        glfwGetVersion( ( nint )( &uMajor ), ( nint )( &uMinor ), ( nint )( &uRevision ) );
        major    = uMajor;
        minor    = uMinor;
        revision = uRevision;
    }

    // The native function returns a statically allocated string, so we just do a copy here.
    /// <inheritdoc cref="glfwGetVersionString"/>
    public static string GetVersionString() => CopyStringFromUnmanaged( glfwGetVersionString(), Encoding.UTF8 );

    /// <inheritdoc cref="glfwGetError"/>
    public static ErrorCode GetError( out string description )
    {
        var descriptionPtr = IntPtr.Zero;
        int  error          = glfwGetError( descriptionPtr );
        description = CopyStringFromUnmanaged( descriptionPtr, Encoding.UTF8 );

        return ( ErrorCode )error;
    }

    private static GlfwErrorCallback _errorCallback;

    /// <inheritdoc cref="glfwSetErrorCallback" path="/summary"/>
    public static GlfwErrorCallback SetErrorCallback( GlfwErrorCallback callback )
    {
        return SetCallback(
                           ref _errorCallback,
                           ref _currentGLFWerrorfun,
                           callback,
                           ( int errorCode, IntPtr description ) => _errorCallback?.Invoke( ( ErrorCode )errorCode, CopyStringFromUnmanaged( description, Encoding.UTF8 ) ),
                           native => glfwSetErrorCallback( native )
                          );
    }

    /// <inheritdoc cref="glfwGetPlatform"/>
    public static Platform GetPlatform() => ( Platform )glfwGetPlatform();

    /// <inheritdoc cref="glfwPlatformSupported"/>
    public static bool PlatformSupported( Platform platform ) => glfwPlatformSupported( ( int )platform ) == GLFW_TRUE;

    /// <inheritdoc cref="glfwGetMonitors"/>
    public static unsafe Monitor[] GetMonitors()
    {
        var    count = 0;
        var ptr   = ( IntPtr )( void* )glfwGetMonitors( ( nint )( &count ) );

        if ( ptr == IntPtr.Zero )
            return Array.Empty< Monitor >();

        Monitor[] monitors = new Monitor[ count ];

        for ( var i = 0; i < count; i++ )
        {
            monitors[ i ] = new Monitor( Marshal.ReadIntPtr( ptr, i * IntPtr.Size ) );
        }

        return monitors;
    }

    /// <inheritdoc cref="glfwGetPrimaryMonitor"/>
    public static unsafe Monitor GetPrimaryMonitor()
    {
        var ptr = ( IntPtr )( void* )glfwGetPrimaryMonitor();

        return new Monitor( ptr );
    }

    /// <inheritdoc cref="glfwGetMonitorPos"/>
    public static unsafe void GetMonitorPos( Monitor monitor, out int x, out int y )
    {
        int uX, uY;
        glfwGetMonitorPos( monitor, ( nint )( &uX ), ( nint )( &uY ) );
        x = uX;
        y = uY;
    }

    /// <inheritdoc cref="glfwGetMonitorWorkarea"/>
    public static unsafe void GetMonitorWorkarea( Monitor monitor, out int x, out int y, out int width, out int height )
    {
        int uX, uY, uWidth, uHeight;
        glfwGetMonitorWorkarea( monitor, ( nint )( &uX ), ( nint )( &uY ), ( nint )( &uWidth ), ( nint )( &uHeight ) );
        x      = uX;
        y      = uY;
        width  = uWidth;
        height = uHeight;
    }

    /// <inheritdoc cref="glfwGetMonitorPhysicalSize"/>
    public static unsafe void GetMonitorPhysicalSize( Monitor monitor, out int width, out int height )
    {
        int uWidth, uHeight;
        glfwGetMonitorPhysicalSize( monitor, ( nint )( &uWidth ), ( nint )( &uHeight ) );
        width  = uWidth;
        height = uHeight;
    }

    /// <inheritdoc cref="glfwGetMonitorContentScale"/>
    public static unsafe void GetMonitorContentScale( Monitor monitor, out float xscale, out float yscale )
    {
        float uXscale, uYscale;
        glfwGetMonitorContentScale( monitor, ( nint )( &uXscale ), ( nint )( &uYscale ) );
        xscale = uXscale;
        yscale = uYscale;
    }

    // Name string pointer is allocated by GLFW and should not be freed by the caller.
    // We only copy the string here.
    /// <inheritdoc cref="glfwGetMonitorName"/>
    public static unsafe string GetMonitorName( Monitor monitor ) => CopyStringFromUnmanaged( glfwGetMonitorName( monitor ), Encoding.UTF8 );

    /// <inheritdoc cref="glfwSetMonitorUserPointer"/>
    public static unsafe void SetMonitorUserPointer( Monitor monitor, IntPtr pointer )
    {
        glfwSetMonitorUserPointer( monitor, pointer );
    }

    /// <inheritdoc cref="glfwGetMonitorUserPointer"/>
    public static unsafe IntPtr GetMonitorUserPointer( Monitor monitor )
    {
        return glfwGetMonitorUserPointer( monitor );
    }

    private static GlfwMonitorCallback _monitorCallback;

    /// <inheritdoc cref="glfwSetMonitorCallback" path="/summary"/>
    public static unsafe GlfwMonitorCallback SetMonitorCallback( GlfwMonitorCallback callback )
    {
        return SetCallback(
                           ref _monitorCallback,
                           ref _currentGLFWmonitorfun,
                           callback,
                           ( GLFWmonitor* monitor, int @event ) =>
                           {
                               var managedMonitor = new Monitor( ( IntPtr )monitor );
                               var managedEvent   = ( ConnectionState )@event;
                               _monitorCallback?.Invoke( managedMonitor, managedEvent );
                           },
                           native => glfwSetMonitorCallback( native )
                          );
    }

    /// <inheritdoc cref="glfwGetVideoModes"/>
    public static unsafe Vidmode[] GetVideoModes( Monitor monitor )
    {
        var    count = 0;
        var ptr   = ( IntPtr )( void* )glfwGetVideoModes( monitor, ( nint )( &count ) );

        if ( ptr == IntPtr.Zero )
            return Array.Empty< Vidmode >();

        var nativeModes = MarshalCopyArrayOf< GLFWvidmode >( ptr, count );

        return nativeModes.Select( nativeMode => new Vidmode
        {
            Width       = nativeMode.width,
            Height      = nativeMode.height,
            RedBits     = nativeMode.redBits,
            GreenBits   = nativeMode.greenBits,
            BlueBits    = nativeMode.blueBits,
            RefreshRate = nativeMode.refreshRate
        } ).ToArray();
    }

    /// <inheritdoc cref="glfwGetVideoMode"/>
    public static unsafe Vidmode GetVideoMode( Monitor monitor )
    {
        GLFWvidmode* nativeMode = glfwGetVideoMode( monitor );

        return new Vidmode
        {
            Width       = nativeMode -> width,
            Height      = nativeMode -> height,
            RedBits     = nativeMode -> redBits,
            GreenBits   = nativeMode -> greenBits,
            BlueBits    = nativeMode -> blueBits,
            RefreshRate = nativeMode -> refreshRate
        };
    }

    /// <inheritdoc cref="glfwSetGamma"/>
    public static unsafe void SetGamma( Monitor monitor, float gamma )
    {
        glfwSetGamma( monitor, gamma );
    }

    /// <inheritdoc cref="glfwGetGammaRamp"/>
    public static unsafe Gammaramp GetGammaRamp( Monitor monitor )
    {
        GLFWgammaramp* gammaramp = glfwGetGammaRamp( monitor );
        var            size      = gammaramp -> size;

        var red   = MarshalCopyArrayOf< ushort >( gammaramp -> red, ( int )size );
        var green = MarshalCopyArrayOf< ushort >( gammaramp -> green, ( int )size );
        var blue  = MarshalCopyArrayOf< ushort >( gammaramp -> blue, ( int )size );

        return new Gammaramp
        {
            Red   = red,
            Green = green,
            Blue  = blue
        };
    }

    /// <inheritdoc cref="glfwSetGammaRamp"/>
    public static unsafe void SetGammaRamp( Monitor monitor, Gammaramp ramp )
    {
        var redPtr   = MarshalManagedArrayToUnmanaged( ramp.Red );
        var greenPtr = MarshalManagedArrayToUnmanaged( ramp.Green );
        var bluePtr  = MarshalManagedArrayToUnmanaged( ramp.Blue );

        var unmanagedRamp = new GLFWgammaramp
        {
            red   = redPtr,
            green = greenPtr,
            blue  = bluePtr,
            size  = ( uint )ramp.Red.Length
        };

        glfwSetGammaRamp( monitor, &unmanagedRamp );

        // ^ copies the ramp, so we can free the memory now
        Marshal.FreeHGlobal( redPtr );
        Marshal.FreeHGlobal( greenPtr );
        Marshal.FreeHGlobal( bluePtr );
    }

    /// <inheritdoc cref="glfwDefaultWindowHints"/>
    public static void DefaultWindowHints() => glfwDefaultWindowHints();

    /// <inheritdoc cref="glfwWindowHint"/>
    /// <param name="hint">The hint to set. Use the static fields in <see cref="WindowHint"/>.</param>
    /// <param name="value">The value to set the hint to.</param>
    public static void WindowHint< T >( WindowHint< T > hint, T value ) where T : struct
    {
        var newValue = Convert.ToInt32( value );
        glfwWindowHint( hint.Hint, newValue );
    }

    /// <inheritdoc cref="glfwWindowHintString"/>
    public static void WindowHintString( WindowHint< string > hint, string value ) =>
        MarshalStringAndFree( Encoding.UTF8, value, ptr => glfwWindowHintString( hint.Hint, ptr ) );

    /// <inheritdoc cref="glfwCreateWindow"/>
    public static unsafe Window CreateWindow( int width, int height, string title, Monitor monitor, Window share ) =>
        MarshalStringAndFree( Encoding.UTF8, title, titlePtr =>
        {
            var windowPtr = glfwCreateWindow( width, height, titlePtr, monitor, share );

            return new Window( ( nint )( void* )windowPtr );
        } );

    /// <inheritdoc cref="glfwDestroyWindow"/>
    public static unsafe void DestroyWindow( Window window ) => glfwDestroyWindow( window );

    /// <inheritdoc cref="glfwWindowShouldClose"/>
    public static unsafe bool WindowShouldClose( Window window ) => glfwWindowShouldClose( window ) == GLFW_TRUE;

    /// <inheritdoc cref="glfwSetWindowShouldClose"/>
    public static unsafe void SetWindowShouldClose( Window window, bool value ) => glfwSetWindowShouldClose( window, value ? GLFW_TRUE : GLFW_FALSE );

    /// <inheritdoc cref="glfwGetWindowTitle"/>
    public static unsafe string GetWindowTitle( Window window ) => CopyStringFromUnmanaged( glfwGetWindowTitle( window ), Encoding.UTF8 );

    /// <inheritdoc cref="glfwSetWindowTitle"/>
    public static unsafe void SetWindowTitle( Window window, string title ) =>
        MarshalStringAndFree( Encoding.UTF8, title, titlePtr => glfwSetWindowTitle( window, titlePtr ) );

    /// <inheritdoc cref = "glfwSetWindowIcon" />
    public static unsafe void SetWindowIcon( Window window, MediaTypeNames.Image[] images )
    {
        if ( images.Length == 0 )
        {
            glfwSetWindowIcon( window, 0, ( GLFWimage* )IntPtr.Zero );

            return;
        }

        var nativeImages = images.Select( image => new GLFWimage
        {
            width  = image.Width,
            height = image.Height,
            pixels = MarshalManagedArrayToUnmanaged( image.Pixels )
        } ).ToArray();

        var nativeImagesPtr = MarshalManagedArrayToUnmanaged( nativeImages );
        glfwSetWindowIcon( window, images.Length, ( GLFWimage* )nativeImagesPtr );
        Marshal.FreeHGlobal( nativeImagesPtr );
    }

    /// <inheritdoc cref="glfwGetWindowPos"/>
    public static unsafe void GetWindowPos( Window window, out int x, out int y )
    {
        int uX, uY;
        glfwGetWindowPos( window, ( nint )( &uX ), ( nint )( &uY ) );
        x = uX;
        y = uY;
    }

    /// <inheritdoc cref="glfwSetWindowPos"/>
    public static unsafe void SetWindowPos( Window window, int x, int y ) =>
        glfwSetWindowPos( window, x, y );

    /// <inheritdoc cref="glfwGetWindowSize"/>
    public static unsafe void GetWindowSize( Window window, out int width, out int height )
    {
        int uWidth, uHeight;
        glfwGetWindowSize( window, ( nint )( &uWidth ), ( nint )( &uHeight ) );
        width  = uWidth;
        height = uHeight;
    }

    /// <inheritdoc cref="glfwSetWindowSize"/>
    public static unsafe void SetWindowSizeLimits( Window window, int minWidth, int minHeight, int maxWidth, int maxHeight ) =>
        glfwSetWindowSizeLimits( window, minWidth, minHeight, maxWidth, maxHeight );

    /// <inheritdoc cref="glfwSetWindowAspectRatio"/>
    public static unsafe void SetWindowAspectRatio( Window window, int numerator, int denominator ) =>
        glfwSetWindowAspectRatio( window, numerator, denominator );

    /// <inheritdoc cref="glfwSetWindowSize"/>
    public static unsafe void SetWindowSize( Window window, int width, int height ) =>
        glfwSetWindowSize( window, width, height );

    /// <inheritdoc cref="glfwGetFramebufferSize"/>
    public static unsafe void GetFramebufferSize( Window window, out int width, out int height )
    {
        int uWidth, uHeight;
        glfwGetFramebufferSize( window, ( nint )( &uWidth ), ( nint )( &uHeight ) );
        width  = uWidth;
        height = uHeight;
    }

    /// <inheritdoc cref="glfwGetWindowFrameSize"/>
    public static unsafe void GetWindowFrameSize( Window window, out int left, out int top, out int right, out int bottom )
    {
        int uLeft, uTop, uRight, uBottom;
        glfwGetWindowFrameSize( window, ( nint )( &uLeft ), ( nint )( &uTop ), ( nint )( &uRight ), ( nint )( &uBottom ) );
        left   = uLeft;
        top    = uTop;
        right  = uRight;
        bottom = uBottom;
    }

    /// <inheritdoc cref="glfwGetWindowContentScale"/>
    public static unsafe void GetWindowContentScale( Window window, out float xScale, out float yScale )
    {
        float uXScale, uYScale;
        glfwGetWindowContentScale( window, ( nint )( &uXScale ), ( nint )( &uYScale ) );
        xScale = uXScale;
        yScale = uYScale;
    }

    /// <inheritdoc cref="glfwGetWindowOpacity"/>
    public static unsafe float GetWindowOpacity( Window window ) =>
        glfwGetWindowOpacity( window );

    /// <inheritdoc cref="glfwSetWindowOpacity"/>
    public static unsafe void SetWindowOpacity( Window window, float opacity ) =>
        glfwSetWindowOpacity( window, opacity );

    /// <inheritdoc cref="glfwIconifyWindow"/>
    public static unsafe void IconifyWindow( Window window ) =>
        glfwIconifyWindow( window );

    /// <inheritdoc cref="glfwRestoreWindow"/>
    public static unsafe void RestoreWindow( Window window ) =>
        glfwRestoreWindow( window );

    /// <inheritdoc cref="glfwMaximizeWindow"/>
    public static unsafe void MaximizeWindow( Window window ) =>
        glfwMaximizeWindow( window );

    /// <inheritdoc cref="glfwShowWindow"/>
    public static unsafe void ShowWindow( Window window ) =>
        glfwShowWindow( window );

    /// <inheritdoc cref="glfwHideWindow"/>
    public static unsafe void HideWindow( Window window ) =>
        glfwHideWindow( window );

    /// <inheritdoc cref="glfwFocusWindow"/>
    public static unsafe void FocusWindow( Window window ) =>
        glfwFocusWindow( window );

    /// <inheritdoc cref="glfwRequestWindowAttention"/>
    public static unsafe void RequestWindowAttention( Window window ) =>
        glfwRequestWindowAttention( window );

    /// <inheritdoc cref="glfwGetWindowMonitor"/>
    public static unsafe Monitor GetWindowMonitor( Window window ) =>
        new Monitor( ( nint )glfwGetWindowMonitor( window ) );

    /// <inheritdoc cref="glfwSetWindowMonitor"/>
    public static unsafe void SetWindowMonitor( Window window, Monitor monitor, int x, int y, int width, int height, int refreshRate ) =>
        glfwSetWindowMonitor( window, monitor, x, y, width, height, refreshRate );

    /// <inheritdoc cref="glfwGetWindowAttrib"/>
    public static unsafe T GetWindowAttrib< T >( Window window, WindowAttribType< T > attrib ) where T : struct
    {
        int value = glfwGetWindowAttrib( window, attrib.Attribute );

        return ( T )Convert.ChangeType( value, typeof( T ) );
    }

    /// <inheritdoc cref="glfwSetWindowAttrib"/>
    public static unsafe void SetWindowAttrib< T >( Window window, WindowAttribType< T > attrib, T value ) where T : struct
    {
        var intValue = Convert.ToInt32( value );
        glfwSetWindowAttrib( window, attrib.Attribute, intValue );
    }

    /// <inheritdoc cref="glfwSetWindowUserPointer"/>
    public static unsafe void SetWindowUserPointer( Window window, IntPtr pointer ) =>
        glfwSetWindowUserPointer( window, pointer );

    /// <inheritdoc cref="glfwGetWindowUserPointer"/>
    public static unsafe IntPtr GetWindowUserPointer( Window window ) =>
        glfwGetWindowUserPointer( window );

    private static GlfwWindowPosCallback _windowPosCallback;

    /// <inheritdoc cref="glfwSetWindowPosCallback" path="/summary"/>
    public static unsafe GlfwWindowPosCallback SetWindowPosCallback( Window window, GlfwWindowPosCallback callback ) =>
        SetCallback(
                    ref _windowPosCallback,
                    ref _currentGLFWwindowposfun,
                    callback,
                    ( GLFWwindow* w, int x, int y ) => _windowPosCallback?.Invoke( new Window( ( nint )w ), x, y ),
                    native => glfwSetWindowPosCallback( window, native )
                   );

    private static GlfwWindowSizeCallback _windowSizeCallback;

    /// <inheritdoc cref="glfwSetWindowSizeCallback" path="/summary"/>
    public static unsafe GlfwWindowSizeCallback SetWindowSizeCallback( Window window, GlfwWindowSizeCallback callback ) =>
        SetCallback(
                    ref _windowSizeCallback,
                    ref _currentGLFWwindowsizefun,
                    callback,
                    ( GLFWwindow* w, int width, int height ) => _windowSizeCallback?.Invoke( new Window( ( nint )w ), width, height ),
                    native => glfwSetWindowSizeCallback( window, native )
                   );

    private static GlfwWindowCloseCallback _windowCloseCallback;

    /// <inheritdoc cref="glfwSetWindowCloseCallback" path="/summary"/>
    public static unsafe GlfwWindowCloseCallback SetWindowCloseCallback( Window window, GlfwWindowCloseCallback callback ) =>
        SetCallback(
                    ref _windowCloseCallback,
                    ref _currentGLFWwindowclosefun,
                    callback,
                    ( GLFWwindow* w ) => _windowCloseCallback?.Invoke( new Window( ( nint )w ) ),
                    native => glfwSetWindowCloseCallback( window, native )
                   );

    private static GlfwWindowRefreshCallback _windowRefreshCallback;

    /// <inheritdoc cref="glfwSetWindowRefreshCallback" path="/summary"/>
    public static unsafe GlfwWindowRefreshCallback SetWindowRefreshCallback( Window window, GlfwWindowRefreshCallback callback ) =>
        SetCallback(
                    ref _windowRefreshCallback,
                    ref _currentGLFWwindowrefreshfun,
                    callback,
                    ( GLFWwindow* w ) => _windowRefreshCallback?.Invoke( new Window( ( nint )w ) ),
                    native => glfwSetWindowRefreshCallback( window, native )
                   );

    private static GlfwWindowFocusCallback _windowFocusCallback;

    /// <inheritdoc cref="glfwSetWindowFocusCallback" path="/summary"/>
    public static unsafe GlfwWindowFocusCallback SetWindowFocusCallback( Window window, GlfwWindowFocusCallback callback ) =>
        SetCallback(
                    ref _windowFocusCallback,
                    ref _currentGLFWwindowfocusfun,
                    callback,
                    ( GLFWwindow* w, int focused ) => _windowFocusCallback?.Invoke( new Window( ( nint )w ), focused == GLFW_TRUE ),
                    native => glfwSetWindowFocusCallback( window, native )
                   );

    private static GlfwWindowIconifyCallback _windowIconifyCallback;

    /// <inheritdoc cref="glfwSetWindowIconifyCallback" path="/summary"/>
    public static unsafe GlfwWindowIconifyCallback SetWindowIconifyCallback( Window window, GlfwWindowIconifyCallback callback ) =>
        SetCallback(
                    ref _windowIconifyCallback,
                    ref _currentGLFWwindowiconifyfun,
                    callback,
                    ( GLFWwindow* w, int iconified ) => _windowIconifyCallback?.Invoke( new Window( ( nint )w ), iconified == GLFW_TRUE ),
                    native => glfwSetWindowIconifyCallback( window, native )
                   );

    private static GlfwWindowMaximizeCallback _windowMaximizeCallback;

    /// <inheritdoc cref="glfwSetWindowMaximizeCallback" path="/summary"/>
    public static unsafe GlfwWindowMaximizeCallback SetWindowMaximizeCallback( Window window, GlfwWindowMaximizeCallback callback ) =>
        SetCallback(
                    ref _windowMaximizeCallback,
                    ref _currentGLFWwindowmaximizefun,
                    callback,
                    ( GLFWwindow* w, int maximized ) => _windowMaximizeCallback?.Invoke( new Window( ( nint )w ), maximized == GLFW_TRUE ),
                    native => glfwSetWindowMaximizeCallback( window, native )
                   );

    private static GlfwFramebufferSizeCallback _framebufferSizeCallback;

    /// <inheritdoc cref="glfwSetFramebufferSizeCallback" path="/summary"/>
    public static unsafe GlfwFramebufferSizeCallback SetFramebufferSizeCallback( Window window, GlfwFramebufferSizeCallback callback ) =>
        SetCallback(
                    ref _framebufferSizeCallback,
                    ref _currentGLFWframebuffersizefun,
                    callback,
                    ( GLFWwindow* w, int width, int height ) => _framebufferSizeCallback?.Invoke( new Window( ( nint )w ), width, height ),
                    native => glfwSetFramebufferSizeCallback( window, native )
                   );

    private static GlfwWindowContentScaleCallback _windowContentScaleCallback;

    /// <inheritdoc cref="glfwSetWindowContentScaleCallback" path="/summary"/>
    public static unsafe GlfwWindowContentScaleCallback SetWindowContentScaleCallback( Window window, GlfwWindowContentScaleCallback callback ) =>
        SetCallback(
                    ref _windowContentScaleCallback,
                    ref _currentGLFWwindowcontentscalefun,
                    callback,
                    ( GLFWwindow* w, float xScale, float yScale ) => _windowContentScaleCallback?.Invoke( new Window( ( nint )w ), xScale, yScale ),
                    native => glfwSetWindowContentScaleCallback( window, native )
                   );

    /// <inheritdoc cref="glfwPollEvents"/>
    public static void PollEvents() => glfwPollEvents();

    /// <inheritdoc cref="glfwWaitEvents"/>
    public static void WaitEvents() => glfwWaitEvents();

    /// <inheritdoc cref="glfwWaitEventsTimeout"/>
    public static void WaitEventsTimeout( double timeout ) => glfwWaitEventsTimeout( timeout );

    /// <inheritdoc cref="glfwPostEmptyEvent"/>
    public static void PostEmptyEvent() => glfwPostEmptyEvent();

    /// <inheritdoc cref="glfwGetInputMode"/>
    public static unsafe T GetInputMode< T >( Window window, InputModeType< T > mode ) where T : Enum
    {
        int modeAsInt   = mode.Mode;
        int currentMode = glfwGetInputMode( window, modeAsInt );

        return ( T )Enum.ToObject( typeof( T ), currentMode );
    }

    /// <inheritdoc cref="glfwSetInputMode"/>
    public static unsafe void SetInputMode< T >( Window window, InputModeType< T > mode, T value ) where T : Enum
    {
        int modeAsInt  = mode.Mode;
        var valueAsInt = Convert.ToInt32( value );
        glfwSetInputMode( window, modeAsInt, valueAsInt );
    }

    /// <inheritdoc cref="glfwRawMouseMotionSupported"/>
    public static bool RawMouseMotionSupported() => glfwRawMouseMotionSupported() == GLFW_TRUE;

    /// <inheritdoc cref="glfwGetKeyName"/>
    public static unsafe string GetKeyName( Key key, int scancode ) =>
        CopyStringFromUnmanaged( glfwGetKeyName( ( int )key, scancode ), Encoding.UTF8 );

    /// <inheritdoc cref="glfwGetKeyScancode"/>
    public static unsafe int GetKeyScancode( Key key ) => glfwGetKeyScancode( ( int )key );

    /// <inheritdoc cref="glfwGetKey"/>
    public static unsafe InputState GetKey( Window window, Key key ) => ( InputState )glfwGetKey( window, ( int )key );

    /// <inheritdoc cref="glfwGetMouseButton"/>
    public static unsafe InputState GetMouseButton( Window window, MouseButton button ) => ( InputState )glfwGetMouseButton( window, ( int )button );

    /// <inheritdoc cref="glfwGetCursorPos"/>
    public static unsafe void GetCursorPos( Window window, out double xpos, out double ypos )
    {
        double uXpos, uYpos;
        glfwGetCursorPos( window, ( nint )( &uXpos ), ( nint )( &uYpos ) );
        xpos = uXpos;
        ypos = uYpos;
    }

    /// <inheritdoc cref="glfwSetCursorPos"/>
    public static unsafe void SetCursorPos( Window window, double xpos, double ypos ) =>
        glfwSetCursorPos( window, xpos, ypos );

    /// <inheritdoc cref="glfwCreateCursor"/>
    public static unsafe Cursor CreateCursor( MediaTypeNames.Image image, int xhot, int yhot )
    {
        var nativeImage = new GLFWimage
        {
            width  = image.Width,
            height = image.Height,
            pixels = MarshalManagedArrayToUnmanaged( image.Pixels )
        };

        var cursor = glfwCreateCursor( &nativeImage, xhot, yhot );
        Marshal.FreeHGlobal( nativeImage.pixels );

        return new Cursor( ( nint )cursor );
    }

    /// <inheritdoc cref="glfwCreateStandardCursor"/>
    public static unsafe Cursor CreateStandardCursor( CursorShape shape )
    {
        var cursor = glfwCreateStandardCursor( ( int )shape );

        return new Cursor( ( nint )cursor );
    }

    /// <inheritdoc cref="glfwDestroyCursor"/>
    public static unsafe void DestroyCursor( Cursor cursor ) =>
        glfwDestroyCursor( cursor );

    /// <inheritdoc cref="glfwSetCursor"/>
    public static unsafe void SetCursor( Window window, Cursor cursor ) =>
        glfwSetCursor( window, cursor );

    private static GlfwKeyCallback _keyCallback;

    /// <inheritdoc cref="glfwSetKeyCallback" path="/summary"/>
    public static unsafe GlfwKeyCallback SetKeyCallback( Window window, GlfwKeyCallback callback ) =>
        SetCallback(
                    ref _keyCallback,
                    ref _currentGLFWkeyfun,
                    callback,
                    ( GLFWwindow* w, int key, int scancode, int action, int mods ) =>
                    {
                        var managedWindow   = new Window( ( nint )w );
                        var managedKey      = ( Key )key;
                        var managedScancode = scancode;
                        var managedAction   = ( InputState )action;
                        var managedMods     = ( ModifierKey )mods;
                        _keyCallback?.Invoke( managedWindow, managedKey, managedScancode, managedAction, managedMods );
                    },
                    native => glfwSetKeyCallback( window, native )
                   );

    private static GlfwCharCallback _charCallback;

    /// <inheritdoc cref="glfwSetCharCallback" path="/summary"/>
    public static unsafe GlfwCharCallback SetCharCallback( Window window, GlfwCharCallback callback ) =>
        SetCallback(
                    ref _charCallback,
                    ref _currentGLFWcharfun,
                    callback,
                    ( GLFWwindow* w, uint codepoint ) =>
                    {
                        var managedWindow    = new Window( ( nint )w );
                        var managedCodepoint = codepoint;
                        _charCallback?.Invoke( managedWindow, managedCodepoint );
                    },
                    native => glfwSetCharCallback( window, native )
                   );

    private static GlfwCharModsCallback _charModsCallback;

    /// <inheritdoc cref="glfwSetCharModsCallback" path="/summary"/>
    public static unsafe GlfwCharModsCallback SetCharModsCallback( Window window, GlfwCharModsCallback callback ) =>
        SetCallback(
                    ref _charModsCallback,
                    ref _currentGLFWcharmodsfun,
                    callback,
                    ( GLFWwindow* w, uint codepoint, int mods ) =>
                    {
                        var managedWindow    = new Window( ( nint )w );
                        var managedCodepoint = codepoint;
                        var managedMods      = ( ModifierKey )mods;
                        _charModsCallback?.Invoke( managedWindow, managedCodepoint, managedMods );
                    },
                    native => glfwSetCharModsCallback( window, native )
                   );

    private static GlfwMouseButtonCallback _mouseButtonCallback;

    /// <inheritdoc cref="glfwSetMouseButtonCallback" path="/summary"/>
    public static unsafe GlfwMouseButtonCallback SetMouseButtonCallback( Window window, GlfwMouseButtonCallback callback ) =>
        SetCallback(
                    ref _mouseButtonCallback,
                    ref _currentGLFWmousebuttonfun,
                    callback,
                    ( GLFWwindow* w, int button, int action, int mods ) =>
                    {
                        var managedWindow = new Window( ( nint )w );
                        var managedButton = ( MouseButton )button;
                        var managedAction = ( InputState )action;
                        var managedMods   = ( ModifierKey )mods;
                        _mouseButtonCallback?.Invoke( managedWindow, managedButton, managedAction, managedMods );
                    },
                    native => glfwSetMouseButtonCallback( window, native )
                   );

    private static GlfwCursorPosCallback _cursorPosCallback;

    /// <inheritdoc cref="glfwSetCursorPosCallback" path="/summary"/> 
    public static unsafe GlfwCursorPosCallback SetCursorPosCallback( Window window, GlfwCursorPosCallback callback ) =>
        SetCallback(
                    ref _cursorPosCallback,
                    ref _currentGLFWcursorposfun,
                    callback,
                    ( GLFWwindow* w, double xpos, double ypos ) =>
                    {
                        var managedWindow = new Window( ( nint )w );
                        var managedXpos   = xpos;
                        var managedYpos   = ypos;
                        _cursorPosCallback?.Invoke( managedWindow, managedXpos, managedYpos );
                    },
                    native => glfwSetCursorPosCallback( window, native )
                   );

    private static GlfwCursorEnterCallback _cursorEnterCallback;

    /// <inheritdoc cref="glfwSetCursorEnterCallback" path="/summary"/>
    public static unsafe GlfwCursorEnterCallback SetCursorEnterCallback( Window window, GlfwCursorEnterCallback callback ) =>
        SetCallback(
                    ref _cursorEnterCallback,
                    ref _currentGLFWcursorenterfun,
                    callback,
                    ( GLFWwindow* w, int entered ) =>
                    {
                        var managedWindow  = new Window( ( nint )w );
                        var managedEntered = entered == GLFW_TRUE;
                        _cursorEnterCallback?.Invoke( managedWindow, managedEntered );
                    },
                    native => glfwSetCursorEnterCallback( window, native )
                   );

    private static GlfwScrollCallback _scrollCallback;

    /// <inheritdoc cref="glfwSetScrollCallback" path="/summary"/>
    public static unsafe GlfwScrollCallback SetScrollCallback( Window window, GlfwScrollCallback callback ) =>
        SetCallback(
                    ref _scrollCallback,
                    ref _currentGLFWscrollfun,
                    callback,
                    ( GLFWwindow* w, double xoffset, double yoffset ) =>
                    {
                        var managedWindow  = new Window( ( nint )w );
                        var managedXoffset = xoffset;
                        var managedYoffset = yoffset;
                        _scrollCallback?.Invoke( managedWindow, managedXoffset, managedYoffset );
                    },
                    native => glfwSetScrollCallback( window, native )
                   );

    private static GlfwDropCallback _dropCallback;

    /// <inheritdoc cref="glfwSetDropCallback" path="/summary"/>
    public static unsafe GlfwDropCallback SetDropCallback( Window window, GlfwDropCallback callback ) =>
        SetCallback(
                    ref _dropCallback,
                    ref _currentGLFWdropfun,
                    callback,
                    ( GLFWwindow* w, int count, IntPtr paths ) =>
                    {
                        var managedWindow = new Window( ( nint )w );
                        var managedPaths  = new string[ count ];

                        for ( var i = 0; i < count; i++ )
                        {
                            var ptr = Marshal.ReadIntPtr( paths, i * IntPtr.Size );
                            managedPaths[ i ] = CopyStringFromUnmanaged( ptr, Encoding.UTF8 );
                        }

                        _dropCallback?.Invoke( managedWindow, managedPaths );
                    },
                    native => glfwSetDropCallback( window, native )
                   );

    /// <inheritdoc cref="glfwJoystickPresent"/>
    public static bool JoystickPresent( Joystick jid ) => glfwJoystickPresent( ( int )jid ) == GLFW_TRUE;

    /// <inheritdoc cref="glfwGetJoystickAxes"/>
    public static unsafe float[] GetJoystickAxes( Joystick jid )
    {
        var count = 0;
        var axes  = glfwGetJoystickAxes( ( int )jid, ( nint )( &count ) );

        return MarshalCopyArrayOf< float >( axes, count );
    }

    /// <inheritdoc cref="glfwGetJoystickButtons"/>
    public static unsafe byte[] GetJoystickButtons( Joystick jid )
    {
        var count   = 0;
        var buttons = glfwGetJoystickButtons( ( int )jid, ( nint )( &count ) );

        return MarshalCopyArrayOf< byte >( buttons, count );
    }

    /// <inheritdoc cref="glfwGetJoystickHats"/>
    public static unsafe byte[] GetJoystickHats( Joystick jid )
    {
        var count = 0;
        var hats  = glfwGetJoystickHats( ( int )jid, ( nint )( &count ) );

        return MarshalCopyArrayOf< byte >( hats, count );
    }

    /// <inheritdoc cref="glfwGetJoystickName"/>
    public static unsafe string GetJoystickName( Joystick jid ) =>
        CopyStringFromUnmanaged( glfwGetJoystickName( ( int )jid ), Encoding.UTF8 );

    /// <inheritdoc cref="glfwGetJoystickGUID"/>
    public static unsafe string GetJoystickGUID( Joystick jid ) =>
        CopyStringFromUnmanaged( glfwGetJoystickGUID( ( int )jid ), Encoding.UTF8 );

    /// <inheritdoc cref="glfwSetJoystickUserPointer"/>
    public static unsafe void SetJoystickUserPointer( Joystick jid, IntPtr pointer ) =>
        glfwSetJoystickUserPointer( ( int )jid, pointer );

    /// <inheritdoc cref="glfwGetJoystickUserPointer"/>
    public static unsafe IntPtr GetJoystickUserPointer( Joystick jid ) =>
        glfwGetJoystickUserPointer( ( int )jid );

    /// <inheritdoc cref="glfwJoystickIsGamepad"/>
    public static bool JoystickIsGamepad( Joystick jid ) => glfwJoystickIsGamepad( ( int )jid ) == GLFW_TRUE;

    private static GlfwJoystickCallback _joystickCallback;

    /// <inheritdoc cref="glfwSetJoystickCallback" path="/summary"/>
    public static unsafe GlfwJoystickCallback SetJoystickCallback( GlfwJoystickCallback callback ) =>
        SetCallback(
                    ref _joystickCallback,
                    ref _currentGLFWjoystickfun,
                    callback,
                    ( int jid, int @event ) =>
                    {
                        var managedJid   = ( Joystick )jid;
                        var managedEvent = ( ConnectionState )@event;
                        _joystickCallback?.Invoke( managedJid, managedEvent );
                    },
                    native => glfwSetJoystickCallback( native )
                   );

    /// <inheritdoc cref="glfwUpdateGamepadMappings"/>
    public static unsafe bool UpdateGamepadMappings( string newMappings ) =>
        MarshalStringAndFree( Encoding.UTF8, newMappings, mappingsPtr =>
                                  glfwUpdateGamepadMappings( mappingsPtr ) == GLFW_TRUE );

    /// <inheritdoc cref="glfwGetGamepadName"/>
    public static unsafe string GetGamepadName( Joystick jid ) =>
        CopyStringFromUnmanaged( glfwGetGamepadName( ( int )jid ), Encoding.UTF8 );

    /// <inheritdoc cref="glfwGetGamepadState"/>
    public static unsafe bool GetGamepadState( Joystick jid, out Gamepadstate state )
    {
        GLFWgamepadstate nativeState;
        bool             result = glfwGetGamepadState( ( int )jid, &nativeState ) == GLFW_TRUE;
        
        state = new Gamepadstate
        {
            Buttons = MarshalCopyArrayOf< byte >( ( nint )nativeState.buttons, GLFW_GAMEPAD_BUTTON_LAST + 1 ),
            Axes    = MarshalCopyArrayOf< float >( ( nint )nativeState.axes, GLFW_GAMEPAD_AXIS_LAST + 1 )
        };

        return result;
    }

    /// <inheritdoc cref="glfwSetClipboardString"/>
    public static unsafe void SetClipboardString( Window window, string @string ) =>
        MarshalStringAndFree( Encoding.UTF8, @string, strPtr => glfwSetClipboardString( window, strPtr ) );

    /// <inheritdoc cref="glfwGetClipboardString"/>
    public static unsafe string GetClipboardString( Window window ) =>
        CopyStringFromUnmanaged( glfwGetClipboardString( window ), Encoding.UTF8 );

    /// <inheritdoc cref="glfwGetTime"/>
    public static double GetTime() => glfwGetTime();

    /// <inheritdoc cref="glfwSetTime"/>
    public static void SetTime( double time ) => glfwSetTime( time );

    /// <inheritdoc cref="glfwGetTimerValue"/>
    public static ulong GetTimerValue() => glfwGetTimerValue();

    /// <inheritdoc cref="glfwGetTimerFrequency"/>
    public static ulong GetTimerFrequency() => glfwGetTimerFrequency();

    /// <inheritdoc cref="glfwMakeContextCurrent"/>
    public static unsafe void MakeContextCurrent( Window window ) => glfwMakeContextCurrent( window );

    /// <inheritdoc cref="glfwGetCurrentContext"/>
    public static unsafe Window GetCurrentContext() => new Window( ( nint )glfwGetCurrentContext() );

    /// <inheritdoc cref="glfwSwapBuffers"/>
    public static unsafe void SwapBuffers( Window window ) => glfwSwapBuffers( window );

    /// <inheritdoc cref="glfwSwapInterval"/>
    public static void SwapInterval( int interval ) => glfwSwapInterval( interval );

    /// <inheritdoc cref="glfwExtensionSupported"/>
    public static unsafe bool ExtensionSupported( string extension ) =>
        MarshalStringAndFree( Encoding.UTF8, extension, extPtr => glfwExtensionSupported( extPtr ) == GLFW_TRUE );

    /// <inheritdoc cref="glfwGetProcAddress"/>
    public static IntPtr GetProcAddress( string procname ) =>
        MarshalStringAndFree( Encoding.UTF8, procname, procnamePtr =>
                                  glfwGetProcAddress( procnamePtr ) );

    /// <inheritdoc cref="glfwVulkanSupported"/>
    public static bool VulkanSupported() => glfwVulkanSupported() == GLFW_TRUE;

    /// <inheritdoc cref="glfwGetRequiredInstanceExtensions"/>
    public static unsafe string[] GetRequiredInstanceExtensions()
    {
        var count          = 0;
        var extensions     = glfwGetRequiredInstanceExtensions( ( nint )( &count ) );
        var managedStrings = new string[ count ];

        for ( var i = 0; i < count; i++ )
        {
            var ptr = Marshal.ReadIntPtr( extensions, i * IntPtr.Size );
            managedStrings[ i ] = CopyStringFromUnmanaged( ptr, Encoding.UTF8 );
        }

        return managedStrings;
    }

    /// <inheritdoc cref="glfwGetInstanceProcAddress"/>
    public static IntPtr GetInstanceProcAddress( IntPtr instance, string procname ) =>
        MarshalStringAndFree( Encoding.UTF8, procname, procnamePtr =>
                                  glfwGetInstanceProcAddress( instance, procnamePtr ) );

    /// <inheritdoc cref="glfwGetPhysicalDevicePresentationSupport"/>
    public static bool GetPhysicalDevicePresentationSupport( IntPtr instance, IntPtr device, uint queuefamily ) =>
        glfwGetPhysicalDevicePresentationSupport( instance, device, queuefamily ) == GLFW_TRUE;

    /// <inheritdoc cref="glfwCreateWindowSurface"/>
    public static unsafe int CreateWindowSurface( IntPtr instance, Window window, IntPtr allocator, IntPtr surface ) =>
        glfwCreateWindowSurface( instance, window, allocator, surface );
}