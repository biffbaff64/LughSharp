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


using ConnectionState = LughGlfw.Glfw.Enums.ConnectionState;

namespace LughGlfw.Glfw;

/// <inheritdoc cref="NativeGlfw.GlfwErrorfun" />
public delegate void GlfwErrorCallback(ErrorCode errorCode, string description);

/// <inheritdoc cref="NativeGlfw.GlfwMonitorfun" />
public delegate void GlfwMonitorCallback(Monitor monitor, ConnectionState @event);

/// <inheritdoc cref="NativeGlfw.GlfwWindowposfun" />
public delegate void GlfwWindowPosCallback(Window window, int x, int y);

/// <inheritdoc cref="NativeGlfw.GlfwWindowsizefun" />
public delegate void GlfwWindowSizeCallback(Window window, int width, int height);

/// <inheritdoc cref="NativeGlfw.GlfwWindowclosefun" />
public delegate void GlfwWindowCloseCallback(Window window);

/// <inheritdoc cref="NativeGlfw.GlfwWindowrefreshfun" />
public delegate void GlfwWindowRefreshCallback(Window window);

/// <inheritdoc cref="NativeGlfw.GlfwWindowfocusfun" />
public delegate void GlfwWindowFocusCallback(Window window, bool focused);

/// <inheritdoc cref="NativeGlfw.GlfwWindowiconifyfun" />
public delegate void GlfwWindowIconifyCallback(Window window, bool iconified);

/// <inheritdoc cref="NativeGlfw.GlfwWindowMaximizefun" />
public delegate void GlfwWindowMaximizeCallback(Window window, bool maximized);

/// <inheritdoc cref="NativeGlfw.GlfwFrameBufferSizefun" />
public delegate void GlfwFramebufferSizeCallback(Window window, int width, int height);

/// <inheritdoc cref="NativeGlfw.GlfwWindowContentScalefun" />
public delegate void GlfwWindowContentScaleCallback(Window window, float xScale, float yScale);

/// <inheritdoc cref="NativeGlfw.GlfwKeyfun" />
public delegate void GlfwKeyCallback(Window window, Key key, int scancode, InputState action, ModifierKey mods);

/// <inheritdoc cref="NativeGlfw.GlfwCharfun" />
public delegate void GlfwCharCallback(Window window, uint codepoint);

/// <inheritdoc cref="NativeGlfw.GlfwCharmodsfun" />
public delegate void GlfwCharModsCallback(Window window, uint codepoint, ModifierKey mods);

/// <inheritdoc cref="NativeGlfw.GlfwMouseButtonfun" />
public delegate void GlfwMouseButtonCallback(Window window, MouseButton button, InputState action, ModifierKey mods);

/// <inheritdoc cref="NativeGlfw.GlfwCursorposfun" />
public delegate void GlfwCursorPosCallback(Window window, double x, double y);

/// <inheritdoc cref="NativeGlfw.GlfwCursorenterfun" />
public delegate void GlfwCursorEnterCallback(Window window, bool entered);

/// <inheritdoc cref="NativeGlfw.GlfwScrollfun" />
public delegate void GlfwScrollCallback(Window window, double xoffset, double yoffset);

/// <inheritdoc cref="NativeGlfw.GlfwDropfun" />
public delegate void GlfwDropCallback(Window window, string[] paths);

/// <inheritdoc cref="NativeGlfw.GlfwJoystickfun" />
public delegate void GlfwJoystickCallback(Joystick joystick, ConnectionState @event);


