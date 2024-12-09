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
/// All window attributes that can be queried or set.
/// </summary>
public static class WindowAttrib
{
  /// <summary>
  /// Window focus attribute
  /// </summary>
  public static readonly WindowAttribType<bool> Focused = new WindowAttribType<bool>(NativeGlfw.GLFW_FOCUSED);

  /// <summary>
  /// Window iconification attribute
  /// </summary>
  public static readonly WindowAttribType<bool> Iconified = new WindowAttribType<bool>(NativeGlfw.GLFW_ICONIFIED);

  /// <summary>
  /// Window maximized attribute
  /// </summary>
  public static readonly WindowAttribType<bool> Maximized = new WindowAttribType<bool>(NativeGlfw.GLFW_MAXIMIZED);

  /// <summary>
  /// If the window is hovered or not. Can only be queried.
  /// </summary>
  public static readonly WindowAttribType<bool> Hovered = new WindowAttribType<bool>(NativeGlfw.GLFW_HOVERED);

  /// <summary>
  /// If the window is visible or not
  /// </summary>
  public static readonly WindowAttribType<bool> Visible = new WindowAttribType<bool>(NativeGlfw.GLFW_VISIBLE);

  /// <summary>
  /// If the window is resizable or not
  /// </summary>
  public static readonly WindowAttribType<bool> Resizable = new WindowAttribType<bool>(NativeGlfw.GLFW_RESIZABLE);

  /// <summary>
  /// If the window has decorations or not
  /// </summary>
  public static readonly WindowAttribType<bool> Decorated = new WindowAttribType<bool>(NativeGlfw.GLFW_DECORATED);

  /// <summary>
  /// If the window has automatic iconification enabled or not
  /// </summary>
  public static readonly WindowAttribType<bool> AutoIconify = new WindowAttribType<bool>(NativeGlfw.GLFW_AUTO_ICONIFY);

  /// <summary>
  /// If the window is floating or not
  /// </summary>
  public static readonly WindowAttribType<bool> Floating = new WindowAttribType<bool>(NativeGlfw.GLFW_FLOATING);

  /// <summary>
  /// If the window has a transparent framebuffer or not
  /// </summary>
  public static readonly WindowAttribType<bool> TransparentFramebuffer = new WindowAttribType<bool>(NativeGlfw.GLFW_TRANSPARENT_FRAMEBUFFER);

  /// <summary>
  /// If the window has focus on show or not
  /// </summary>
  public static readonly WindowAttribType<bool> FocusOnShow = new WindowAttribType<bool>(NativeGlfw.GLFW_FOCUS_ON_SHOW);

  /// <summary>
  /// Which client API the window is using
  /// </summary>
  public static readonly WindowAttribType<ClientAPI> ClientAPI = new WindowAttribType<ClientAPI>(NativeGlfw.GLFW_CLIENT_API);

  /// <summary>
  /// The context creation API used to create the window
  /// </summary>
  public static readonly WindowAttribType<ContextCreationAPI> ContextCreationAPI = new WindowAttribType<ContextCreationAPI>(NativeGlfw.GLFW_CONTEXT_CREATION_API);

  /// <summary>
  /// The major version of the current context
  /// </summary>
  public static readonly WindowAttribType<int> ContextVersionMajor = new WindowAttribType<int>(NativeGlfw.GLFW_CONTEXT_VERSION_MAJOR);

  /// <summary>
  /// The minor version of the current context
  /// </summary>
  public static readonly WindowAttribType<int> ContextVersionMinor = new WindowAttribType<int>(NativeGlfw.GLFW_CONTEXT_VERSION_MINOR);

  /// <summary>
  /// The revision of the current context
  /// </summary>
  public static readonly WindowAttribType<int> ContextRevision = new WindowAttribType<int>(NativeGlfw.GLFW_CONTEXT_REVISION);

  /// <summary>
  /// If the OpenGL context is forward compatible or not
  /// </summary>
  public static readonly WindowAttribType<bool> OpenGLForwardCompat = new WindowAttribType<bool>(NativeGlfw.GLFW_OPENGL_FORWARD_COMPAT);

  /// <summary>
  /// If the OpenGL context has debug errors enabled or not
  /// </summary>
  public static readonly WindowAttribType<bool> OpenGLDebugContext = new WindowAttribType<bool>(NativeGlfw.GLFW_OPENGL_DEBUG_CONTEXT);

  /// <summary>
  /// The OpenGL profile used by the context
  /// </summary>
  public static readonly WindowAttribType<OpenGLProfile> OpenGLProfile = new WindowAttribType<OpenGLProfile>(NativeGlfw.GLFW_OPENGL_PROFILE);

  /// <summary>
  /// Which context release behavior is used
  /// </summary>
  public static readonly WindowAttribType<ContextReleaseBehaviour> ContextReleaseBehaviour = new WindowAttribType<ContextReleaseBehaviour>(NativeGlfw.GLFW_CONTEXT_RELEASE_BEHAVIOR);

  /// <summary>
  /// If the context is no error or not
  /// </summary>
  public static readonly WindowAttribType<bool> ContextNoError = new WindowAttribType<bool>(NativeGlfw.GLFW_CONTEXT_NO_ERROR);

  /// <summary>
  /// The context robustness
  /// </summary>
  public static readonly WindowAttribType<ContextRobustness> ContextRobustness = new WindowAttribType<ContextRobustness>(NativeGlfw.GLFW_CONTEXT_ROBUSTNESS);
}

/// <summary>
/// Wrapper class for window hints that only accept certain values.
/// Is used to make it easier for developers to see what values are expected or allowed.
/// </summary>
public class WindowAttribType<T>
{
  internal int Attribute { get; }

  internal WindowAttribType(int attribute)
  {
    Attribute = attribute;
  }
}