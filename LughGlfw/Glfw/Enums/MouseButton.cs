// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
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
/// Wrapping enum for MouseButton.
/// </summary>
[PublicAPI]
public enum MouseButton
{
    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_1" />
    Button1 = NativeGlfw.GLFW_MOUSE_BUTTON_1,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_2" />
    Button2 = NativeGlfw.GLFW_MOUSE_BUTTON_2,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_3" />
    Button3 = NativeGlfw.GLFW_MOUSE_BUTTON_3,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_4" />
    Button4 = NativeGlfw.GLFW_MOUSE_BUTTON_4,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_5" />
    Button5 = NativeGlfw.GLFW_MOUSE_BUTTON_5,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_6" />
    Button6 = NativeGlfw.GLFW_MOUSE_BUTTON_6,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_7" />
    Button7 = NativeGlfw.GLFW_MOUSE_BUTTON_7,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_8" />
    Button8 = NativeGlfw.GLFW_MOUSE_BUTTON_8,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_LAST" />
    ButtonLast = NativeGlfw.GLFW_MOUSE_BUTTON_LAST,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_LEFT" />
    ButtonLeft = NativeGlfw.GLFW_MOUSE_BUTTON_LEFT,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_RIGHT" />
    ButtonRight = NativeGlfw.GLFW_MOUSE_BUTTON_RIGHT,

    /// <inheritdoc cref="NativeGlfw.GLFW_MOUSE_BUTTON_MIDDLE" />
    ButtonMiddle = NativeGlfw.GLFW_MOUSE_BUTTON_MIDDLE,
}