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

namespace LughGlfw.Glfw;

[PublicAPI]
public class Gamepadstate
{
    /// <inheritdoc cref="NativeGlfw.GlfwGamepadState.Buttons"/>
    public byte[] Buttons { get; set; } = [ ];

    /// <inheritdoc cref="NativeGlfw.GlfwGamepadState.Axes"/>
    public float[] Axes { get; set; } = [ ];

    /// <summary>
    /// Get the state of a gamepad button.
    /// </summary>
    /// <param name="button">The button to check.</param>
    /// <returns>The state of the button.</returns>
    public InputState GetButtonState( GamepadButton button ) => ( InputState )Buttons[ ( int )button ];

    /// <summary>
    /// Get the state of a gamepad axis.
    /// </summary>
    /// <param name="axis">The axis to check.</param>
    /// <returns>The state of the axis.</returns>
    public float GetAxis( GamepadAxis axis ) => Axes[ ( int )axis ];
}

