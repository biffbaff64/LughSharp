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
/// Wrapper class for initialization hints.
/// </summary>
public class InitHint<T>(int hint)
{
  internal int Hint { get; } = hint;
}

/// <summary>
/// Initialization hints are set before <see cref="Glfw.Init" /> and affect how the library behaves until termination.
/// Read more here <see href="https://www.glfw.org/docs/latest/intro_guide.html#init_hints" />.
/// </summary>
public class InitHint
{
  // SHARED INIT HINTS

  /// <summary>
  /// Specifies the platform to use for windowing and input. Possible values are <see cref="Platform.AnyPlatform" />, <see cref="Platform.Win32" />, <see cref="Platform.Cocoa" />, <see cref="Platform.Wayland" />, <see cref="Platform.X11" />, <see cref="Platform.Null" />.
  /// Default value: <see cref="Platform.AnyPlatform" />.
  /// </summary>
  public static readonly InitHint<Platform> Platform = new InitHint<Platform>(NativeGlfw.GLFW_PLATFORM);

  /// <summary>
  /// Specifies whether to also expose joystick hats as buttons, for compatibility with earlier versions of GLFW that did not have glfwGetJoystickHats. Possible values are true and false.
  /// Default value: <see langword="true"/>.
  /// </summary>
  public static readonly InitHint<bool> JoystickHatButtons = new InitHint<bool>(NativeGlfw.GLFW_JOYSTICK_HAT_BUTTONS);

  /// <summary>
  /// Specifies the platform type (rendering backend) to request when using OpenGL ES and EGL via ANGLE.
  /// </summary>
  public static readonly InitHint<AnglePlatform> AnglePlatformType = new InitHint<AnglePlatform>(NativeGlfw.GLFW_ANGLE_PLATFORM_TYPE);

  // MACOS SPECIFIC INIT HINTS

  /// <summary>
  /// Specifies whether to set the current directory to the application to the Contents/Resources subdirectory of the application's bundle, if present. Possible values are true and false.
  /// Default value: <see langword="true"/>.
  /// </summary>
  public static readonly InitHint<bool> CocoaChdirResources = new InitHint<bool>(NativeGlfw.GLFW_COCOA_CHDIR_RESOURCES);

  /// <summary>
  /// Specifies whether to create a basic menu bar, either from a nib or manually, when the first window is created, which is when AppKit is initialized. Possible values are true and false.
  /// Default value: <see langword="true"/>.
  /// </summary>
  public static readonly InitHint<bool> CocoaMenubar = new InitHint<bool>(NativeGlfw.GLFW_COCOA_MENUBAR);

  // WAYLAND SPECIFIC INIT HINTS

  /// <summary>
  /// Specifies whether to use libdecor for window decorations where available.
  /// Default value: <see cref="WaylandLibDecor.PreferLibDecor"/>.
  /// </summary>
  public static readonly InitHint<WaylandLibDecor> WaylandLibDecor = new InitHint<WaylandLibDecor>(NativeGlfw.GLFW_WAYLAND_LIBDECOR);

  // X11 SPECIFIC INIT HINTS

  /// <summary>
  /// Specifies whether to prefer the VK_KHR_xcb_surface extension for creating Vulkan surfaces, or whether to use the VK_KHR_xlib_surface extension. Possible values are true and false.
  /// Default value: <see langword="true"/>.
  /// </summary>
  public static readonly InitHint<bool> X11XCBVulkanSurface = new InitHint<bool>(NativeGlfw.GLFW_X11_XCB_VULKAN_SURFACE);
}