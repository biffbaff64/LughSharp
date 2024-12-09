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

using System.Data;

using LughGlfw.Glfw.Enums;

using ConnectionState = LughGlfw.Glfw.Enums.ConnectionState;

namespace LughGlfw.Glfw;

/// <inheritdoc cref="NativeGlfw.GLFWerrorfun" />
public delegate void GlfwErrorCallback(ErrorCode errorCode, string description);

/// <inheritdoc cref="NativeGlfw.GLFWmonitorfun" />
public delegate void GlfwMonitorCallback(Monitor monitor, ConnectionState @event);

/// <inheritdoc cref="NativeGlfw.GLFWwindowposfun" />
public delegate void GlfwWindowPosCallback(Window window, int x, int y);

/// <inheritdoc cref="NativeGlfw.GLFWwindowsizefun" />
public delegate void GlfwWindowSizeCallback(Window window, int width, int height);

/// <inheritdoc cref="NativeGlfw.GLFWwindowclosefun" />
public delegate void GlfwWindowCloseCallback(Window window);

/// <inheritdoc cref="NativeGlfw.GLFWwindowrefreshfun" />
public delegate void GlfwWindowRefreshCallback(Window window);

/// <inheritdoc cref="NativeGlfw.GLFWwindowfocusfun" />
public delegate void GlfwWindowFocusCallback(Window window, bool focused);

/// <inheritdoc cref="NativeGlfw.GLFWwindowiconifyfun" />
public delegate void GlfwWindowIconifyCallback(Window window, bool iconified);

/// <inheritdoc cref="NativeGlfw.GLFWwindowmaximizefun" />
public delegate void GlfwWindowMaximizeCallback(Window window, bool maximized);

/// <inheritdoc cref="NativeGlfw.GLFWframebuffersizefun" />
public delegate void GlfwFramebufferSizeCallback(Window window, int width, int height);

/// <inheritdoc cref="NativeGlfw.GLFWwindowcontentscalefun" />
public delegate void GlfwWindowContentScaleCallback(Window window, float xScale, float yScale);

/// <inheritdoc cref="NativeGlfw.GLFWkeyfun" />
public delegate void GlfwKeyCallback(Window window, Key key, int scancode, InputState action, ModifierKey mods);

/// <inheritdoc cref="NativeGlfw.GLFWcharfun" />
public delegate void GlfwCharCallback(Window window, uint codepoint);

/// <inheritdoc cref="NativeGlfw.GLFWcharmodsfun" />
public delegate void GlfwCharModsCallback(Window window, uint codepoint, ModifierKey mods);

/// <inheritdoc cref="NativeGlfw.GLFWmousebuttonfun" />
public delegate void GlfwMouseButtonCallback(Window window, MouseButton button, InputState action, ModifierKey mods);

/// <inheritdoc cref="NativeGlfw.GLFWcursorposfun" />
public delegate void GlfwCursorPosCallback(Window window, double x, double y);

/// <inheritdoc cref="NativeGlfw.GLFWcursorenterfun" />
public delegate void GlfwCursorEnterCallback(Window window, bool entered);

/// <inheritdoc cref="NativeGlfw.GLFWscrollfun" />
public delegate void GlfwScrollCallback(Window window, double xoffset, double yoffset);

/// <inheritdoc cref="NativeGlfw.GLFWdropfun" />
public delegate void GlfwDropCallback(Window window, string[] paths);

/// <inheritdoc cref="NativeGlfw.GLFWjoystickfun" />
public delegate void GlfwJoystickCallback(Joystick joystick, ConnectionState @event);


