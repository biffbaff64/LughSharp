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
///   Specifies the platform type (rendering backend) to request when using OpenGL ES and EGL via ANGLE.
/// </summary>
public enum Platform
{
  /// <summary>
  /// No platform specified.
  /// </summary>
  AnyPlatform = NativeGlfw.GLFW_ANY_PLATFORM,

  /// <summary>
  /// Windows platform.
  /// </summary>
  Win32 = NativeGlfw.GLFW_PLATFORM_WIN32,

  /// <summary>
  /// macOS platform.
  /// </summary>
  Cocoa = NativeGlfw.GLFW_PLATFORM_COCOA,

  /// <summary>
  /// Linux platform.
  /// </summary>
  Wayland = NativeGlfw.GLFW_PLATFORM_WAYLAND,

  /// <summary>
  /// Linux platform.
  /// </summary>
  X11 = NativeGlfw.GLFW_PLATFORM_X11,

  /// <summary>
  /// Null platform.
  /// </summary>
  Null = NativeGlfw.GLFW_PLATFORM_NULL
}