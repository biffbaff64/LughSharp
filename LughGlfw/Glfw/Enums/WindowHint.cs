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

namespace LughGlfw.Glfw.Enums;

/// <summary>
/// Wrapper class for window hints that only accept certain values.
/// Is used to make it easier for developers to see what values are expected or allowed.
/// </summary>
public class WindowHint< T >( int hint )
{
    internal int Hint { get; } = hint;
}

/// <summary>
/// Window hints are set before <see cref="Glfw.CreateWindow" /> and affect how the window and its context should be created.
/// </summary>
public static class WindowHint
{
    /// <summary>
    /// Whether the windowed window will be resizable by the user.
    /// </summary>
    public static readonly WindowHint< bool > Resizable = new WindowHint< bool >( NativeGlfw.GLFW_RESIZABLE );

    /// <summary>
    /// Whether the windowed window will be initially visible.
    /// </summary>
    public static readonly WindowHint< bool > Visible = new WindowHint< bool >( NativeGlfw.GLFW_VISIBLE );

    /// <summary>
    /// Whether the windowed window will have window decorations such as a border, a close widget, etc.
    /// </summary>
    public static readonly WindowHint< bool > Decorated = new WindowHint< bool >( NativeGlfw.GLFW_DECORATED );

    /// <summary>
    /// Specifies whether the windowed mode window will be given input focus when created.
    /// </summary>
    public static readonly WindowHint< bool > Focused = new WindowHint< bool >( NativeGlfw.GLFW_FOCUSED );

    /// <summary>
    /// Specifies whether the full screen window will automatically iconify and restore the previous video mode on input focus loss.
    /// </summary>
    public static readonly WindowHint< bool > AutoIconify = new WindowHint< bool >( NativeGlfw.GLFW_AUTO_ICONIFY );

    /// <summary>
    /// Specifies whether the windowed mode window will be floating above other regular windows, also called topmost or always-on-top.
    /// </summary>
    public static readonly WindowHint< bool > Floating = new WindowHint< bool >( NativeGlfw.GLFW_FLOATING );

    /// <summary>
    /// Specifies whether the windowed mode window will be maximized when created.
    /// </summary>
    public static readonly WindowHint< bool > Maximized = new WindowHint< bool >( NativeGlfw.GLFW_MAXIMIZED );

    /// <summary>
    /// Specifies whether the cursor should be centered over newly created full screen windows.
    /// </summary>
    public static readonly WindowHint< bool > CenterCursor = new WindowHint< bool >( NativeGlfw.GLFW_CENTER_CURSOR );

    /// <summary>
    /// Specifies whether the window framebuffer will be transparent.
    /// </summary>
    public static readonly WindowHint< bool > TransparentFramebuffer = new WindowHint< bool >( NativeGlfw.GLFW_TRANSPARENT_FRAMEBUFFER );

    /// <summary>
    /// Specifies whether the window will be given input focus when <see cref="Glfw.ShowWindow" /> is called.
    /// </summary>
    public static readonly WindowHint< bool > FocusOnShow = new WindowHint< bool >( NativeGlfw.GLFW_FOCUS_ON_SHOW );

    /// <summary>
    /// Specifies whether the window content area should be resized based on the monitor content scale when the window is moved between monitors that use a different content scale; if enabled, window content scale will also be queried on creation of the window and whenever the window is moved to a monitor that has a different content scale from the current monitor.
    /// </summary>
    public static readonly WindowHint< bool > ScaleToMonitor = new WindowHint< bool >( NativeGlfw.GLFW_SCALE_TO_MONITOR );

    /// <summary>
    /// Specifies how many bits to use for the red component of the color buffer. Possible values are 0, 4 and 8.
    /// </summary>
    public static readonly WindowHint< int > RedBits = new WindowHint< int >( NativeGlfw.GLFW_RED_BITS );

    /// <summary>
    /// Specifies how many bits to use for the green component of the color buffer. Possible values are 0, 4 and 8.
    /// </summary>
    public static readonly WindowHint< int > GreenBits = new WindowHint< int >( NativeGlfw.GLFW_GREEN_BITS );

    /// <summary>
    /// Specifies how many bits to use for the blue component of the color buffer. Possible values are 0, 4 and 8.
    /// </summary>
    public static readonly WindowHint< int > BlueBits = new WindowHint< int >( NativeGlfw.GLFW_BLUE_BITS );

    /// <summary>
    /// Specifies how many bits to use for the alpha component of the color buffer. Possible values are 0, 4 and 8.
    /// </summary>
    public static readonly WindowHint< int > AlphaBits = new WindowHint< int >( NativeGlfw.GLFW_ALPHA_BITS );

    /// <summary>
    /// Specifies how many bits to use for the depth buffer. Possible values are 0, 16 and 24.
    /// </summary>
    public static readonly WindowHint< int > DepthBits = new WindowHint< int >( NativeGlfw.GLFW_DEPTH_BITS );

    /// <summary>
    /// Specifies how many bits to use for the stencil buffer. Possible values are 0 and 8.
    /// </summary>
    public static readonly WindowHint< int > StencilBits = new WindowHint< int >( NativeGlfw.GLFW_STENCIL_BITS );

    /// <summary>
    /// Specifies how many bits to use for the red component of the accumulation buffer. Possible values are 0, 4 and 8.
    /// </summary>
    public static readonly WindowHint< int > AccumRedBits = new WindowHint< int >( NativeGlfw.GLFW_ACCUM_RED_BITS );

    /// <summary>
    /// Specifies how many bits to use for the green component of the accumulation buffer. Possible values are 0, 4 and 8.
    /// </summary>
    public static readonly WindowHint< int > AccumGreenBits = new WindowHint< int >( NativeGlfw.GLFW_ACCUM_GREEN_BITS );

    /// <summary>
    /// Specifies how many bits to use for the blue component of the accumulation buffer. Possible values are 0, 4 and 8.
    /// </summary>
    public static readonly WindowHint< int > AccumBlueBits = new WindowHint< int >( NativeGlfw.GLFW_ACCUM_BLUE_BITS );

    /// <summary>
    /// Specifies how many bits to use for the alpha component of the accumulation buffer. Possible values are 0, 4 and 8.
    /// </summary>
    public static readonly WindowHint< int > AccumAlphaBits = new WindowHint< int >( NativeGlfw.GLFW_ACCUM_ALPHA_BITS );

    /// <summary>
    /// Specifies the number of auxiliary buffers to use. Possible values are 0 and integers between 1 and 4, inclusive.
    /// </summary>
    public static readonly WindowHint< int > AuxBuffers = new WindowHint< int >( NativeGlfw.GLFW_AUX_BUFFERS );

    /// <summary>
    /// Specifies how many samples to use for multisampling. Possible values are integers between 0 and 16, inclusive.
    /// </summary>
    public static readonly WindowHint< int > Samples = new WindowHint< int >( NativeGlfw.GLFW_SAMPLES );

    /// <summary>
    /// Specifies the desired refresh rate for full screen windows.
    /// </summary>
    public static readonly WindowHint< int > RefreshRate = new WindowHint< int >( NativeGlfw.GLFW_REFRESH_RATE );

    /// <summary>
    /// Specifies whether to use stereoscopic rendering. This is a hard constraint.
    /// </summary>
    public static readonly WindowHint< bool > Stereo = new WindowHint< bool >( NativeGlfw.GLFW_STEREO );

    /// <summary>
    /// Specifies whether the framebuffer should be sRGB capable. 
    /// </summary>
    public static readonly WindowHint< bool > SRGBCapable = new WindowHint< bool >( NativeGlfw.GLFW_SRGB_CAPABLE );

    /// <summary>
    /// Specifies whether the framebuffer should be double buffered. You nearly always want to use double buffering. This is a hard constraint.
    /// </summary>
    public static readonly WindowHint< bool > DoubleBuffer = new WindowHint< bool >( NativeGlfw.GLFW_DOUBLEBUFFER );

    /// <summary>
    /// Specifies the client API to create the context for. Possible values are <see cref="ClientAPI.OpenGLAPI" />, <see cref="ClientAPI.OpenGLESAPI" /> and <see cref="ClientAPI.NoAPI" />.
    /// </summary>
    public static readonly WindowHint< ClientAPI > ClientAPI = new WindowHint< ClientAPI >( NativeGlfw.GLFW_CLIENT_API );

    /// <summary>
    /// Specifies the context creation API to use. Possible values are <see cref="ContextCreationAPI.NativeContextAPI" />, <see cref="ContextCreationAPI.EGLContextAPI" />, and <see cref="ContextCreationAPI.OSMesaContextAPI" />.
    /// </summary>
    public static readonly WindowHint< ContextCreationAPI > ContextCreationAPI = new WindowHint< ContextCreationAPI >( NativeGlfw.GLFW_CONTEXT_CREATION_API );

    /// <summary>
    /// Specifies the client API major version that the created context must be compatible with. The exact behavior of this hint depends on the requested client API.
    /// </summary>
    public static readonly WindowHint< int > ContextVersionMajor = new WindowHint< int >( NativeGlfw.GLFW_CONTEXT_VERSION_MAJOR );

    /// <summary>
    /// Specifies the client API minor version that the created context must be compatible with. The exact behavior of this hint depends on the requested client API.
    /// </summary>
    public static readonly WindowHint< int > ContextVersionMinor = new WindowHint< int >( NativeGlfw.GLFW_CONTEXT_VERSION_MINOR );

    /// <summary>
    /// Specifies the client API revision number that the created context must be compatible with. The exact behavior of this hint depends on the requested client API.
    /// </summary>
    public static readonly WindowHint< ContextRobustness > ContextRobustness = new WindowHint< ContextRobustness >( NativeGlfw.GLFW_CONTEXT_ROBUSTNESS );

    /// <summary>
    /// Specifies the release behavior to be used by the context. Possible values are <see cref="ContextReleaseBehaviour.AnyReleaseBehaviour" />, <see cref="ContextReleaseBehaviour.ReleaseBehaviourFlush" /> and <see cref="ContextReleaseBehaviour.ReleaseBehaviourNone" />.
    /// </summary>
    public static readonly WindowHint< ContextReleaseBehaviour > ContextReleaseBehaviour = new WindowHint< ContextReleaseBehaviour >( NativeGlfw.GLFW_CONTEXT_RELEASE_BEHAVIOR );

    /// <summary>
    /// Specifies whether to use OpenGL forward compatibility. If set to <c>true</c>, then no deprecated functionality will be supported by the created context. Possible values are <c>true</c> and <c>false</c>.
    /// </summary>
    public static readonly WindowHint< bool > OpenGLForwardCompat = new WindowHint< bool >( NativeGlfw.GLFW_OPENGL_FORWARD_COMPAT );

    /// <summary>
    /// Specifies whether to use OpenGL debug context. If set to <c>true</c>, then debug messages will be generated by the context. Possible values are <c>true</c> and <c>false</c>.
    /// </summary>
    public static readonly WindowHint< bool > OpenGLDebugContext = new WindowHint< bool >( NativeGlfw.GLFW_OPENGL_DEBUG_CONTEXT );

    /// <summary>
    /// Specifies the OpenGL profile used by the context. Possible values are <see cref="OpenGLProfile.AnyProfile" />, <see cref="OpenGLProfile.CoreProfile" /> and <see cref="OpenGLProfile.CompatProfile" />.
    /// </summary>
    public static readonly WindowHint< OpenGLProfile > OpenGLProfile = new WindowHint< OpenGLProfile >( NativeGlfw.GLFW_OPENGL_PROFILE );

    /// <summary>
    /// Specifies whether to use full resolution framebuffers on Retina displays.
    /// </summary>
    public static readonly WindowHint< bool > CocoaRetinaFramebuffer = new WindowHint< bool >( NativeGlfw.GLFW_COCOA_RETINA_FRAMEBUFFER );

    /// <summary>
    /// Specifies the UTF-8 encoded name to use for autosaving the window frame, or <c>null</c> for no autosave. This is only supported on OS X.
    /// </summary>
    public static readonly WindowHint< string > CocoaFrameName = new WindowHint< string >( NativeGlfw.GLFW_COCOA_FRAME_NAME );

    /// <summary>
    /// Specifies whether to enable automatic graphics switching, i.e. to allow the system to choose the integrated GPU for the OpenGL context and move it between GPUs if necessary or whether to force it to always run on the discrete GPU. This is only supported on OS X.
    /// </summary>
    public static readonly WindowHint< bool > CocoaGraphicsSwitching = new WindowHint< bool >( NativeGlfw.GLFW_COCOA_GRAPHICS_SWITCHING );

    /// <summary>
    /// Specifies the desired ASCII encoded class part of the ICCCM class hint for the window. This is only supported on X11.
    /// </summary>
    public static readonly WindowHint< string > X11ClassName = new WindowHint< string >( NativeGlfw.GLFW_X11_CLASS_NAME );

    /// <summary>
    /// Specifies the desired ASCII encoded instance part of the ICCCM class hint for the window. This is only supported on X11.
    /// </summary>
    public static readonly WindowHint< string > X11InstanceName = new WindowHint< string >( NativeGlfw.GLFW_X11_INSTANCE_NAME );

    /// <summary>
    /// Specifies the access to the window menu. Possible values are <c>true</c> and <c>false</c>.
    /// </summary>
    public static readonly WindowHint< bool > Win32KeyboardMenu = new WindowHint< bool >( NativeGlfw.GLFW_WIN32_KEYBOARD_MENU );

    /// <summary>
    /// Specifies whether the parent process show command should be applied to the child process. Windows specific.
    /// </summary>
    public static readonly WindowHint< bool > Win32ShowDefault = new WindowHint< bool >( NativeGlfw.GLFW_WIN32_SHOWDEFAULT );

    /// <summary>
    /// Allows specification of the Wayland app_id.
    /// </summary>
    public static readonly WindowHint< string > WaylandAppId = new WindowHint< string >( NativeGlfw.GLFW_WAYLAND_APP_ID );

    /// <summary>
    /// The window frame will not be rendered at full resolution on Retina displays unless this hint is set to <c>true</c>.
    /// </summary>
    public static readonly WindowHint< bool > ScaleFramebuffer = new WindowHint< bool >( NativeGlfw.GLFW_SCALE_FRAMEBUFFER );

    /// <summary>
    /// Specifies the desired initial window X position.
    /// </summary>
    public static readonly WindowHint< uint > PositionX = new WindowHint< uint >( NativeGlfw.GLFW_POSITION_X );

    /// <summary>
    /// Specifies the desired initial window Y position.
    /// </summary>
    public static readonly WindowHint< uint > PositionY = new WindowHint< uint >( NativeGlfw.GLFW_POSITION_Y );

    /// <summary>
    /// specifies whether the window is transparent to mouse input, letting any mouse events pass through to whatever window is behind it. This can be set before creation with the GLFW_MOUSE_PASSTHROUGH window hint or after with glfwSetWindowAttrib. This is only supported for undecorated windows. Decorated windows with this enabled will behave differently between platforms.
    /// </summary>
    public static readonly WindowHint< bool > MousePassthrough = new WindowHint< bool >( NativeGlfw.GLFW_MOUSE_PASSTHROUGH );
}