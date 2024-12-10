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
/// Wrapping enum for GamepadButton.
/// </summary>
[PublicAPI]
public enum GamepadButton
{
    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_A" />
    A = NativeGlfw.GLFW_GAMEPAD_BUTTON_A,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_B" />
    B = NativeGlfw.GLFW_GAMEPAD_BUTTON_B,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_X" />
    X = NativeGlfw.GLFW_GAMEPAD_BUTTON_X,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_Y" />
    Y = NativeGlfw.GLFW_GAMEPAD_BUTTON_Y,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_LEFT_BUMPER" />
    LeftBumper = NativeGlfw.GLFW_GAMEPAD_BUTTON_LEFT_BUMPER,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_RIGHT_BUMPER" />
    RightBumper = NativeGlfw.GLFW_GAMEPAD_BUTTON_RIGHT_BUMPER,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_BACK" />
    Back = NativeGlfw.GLFW_GAMEPAD_BUTTON_BACK,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_START" />
    Start = NativeGlfw.GLFW_GAMEPAD_BUTTON_START,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_GUIDE" />
    Guide = NativeGlfw.GLFW_GAMEPAD_BUTTON_GUIDE,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_LEFT_THUMB" />
    LeftThumb = NativeGlfw.GLFW_GAMEPAD_BUTTON_LEFT_THUMB,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_RIGHT_THUMB" />
    RightThumb = NativeGlfw.GLFW_GAMEPAD_BUTTON_RIGHT_THUMB,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_DPAD_UP" />
    DpadUp = NativeGlfw.GLFW_GAMEPAD_BUTTON_DPAD_UP,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_DPAD_RIGHT" />
    DpadRight = NativeGlfw.GLFW_GAMEPAD_BUTTON_DPAD_RIGHT,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_DPAD_DOWN" />
    DpadDown = NativeGlfw.GLFW_GAMEPAD_BUTTON_DPAD_DOWN,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_DPAD_LEFT" />
    DpadLeft = NativeGlfw.GLFW_GAMEPAD_BUTTON_DPAD_LEFT,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_LAST" />
    Last = NativeGlfw.GLFW_GAMEPAD_BUTTON_LAST,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_CROSS" />
    Cross = NativeGlfw.GLFW_GAMEPAD_BUTTON_CROSS,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_CIRCLE" />
    Circle = NativeGlfw.GLFW_GAMEPAD_BUTTON_CIRCLE,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_SQUARE" />
    Square = NativeGlfw.GLFW_GAMEPAD_BUTTON_SQUARE,

    /// <inheritdoc cref="NativeGlfw.GLFW_GAMEPAD_BUTTON_TRIANGLE" />
    Triangle = NativeGlfw.GLFW_GAMEPAD_BUTTON_TRIANGLE,
}