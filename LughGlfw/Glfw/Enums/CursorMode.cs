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
/// Input modes for a cursor
/// </summary>
public enum CursorMode
{
  /// <summary>
  ///     Makes the cursor visible and behaving normally.
  /// </summary>
  Normal = NativeGlfw.GLFW_CURSOR_NORMAL,

  /// <summary>
  ///     Makes the cursor invisible when it is over the client area of the window but does not restrict the cursor from leaving.
  /// </summary>
  Hidden = NativeGlfw.GLFW_CURSOR_HIDDEN,

  /// <summary>
  ///     Hides and grabs the cursor, providing virtual and unlimited cursor movement. This is useful for implementing for example 3D camera controls.
  /// </summary>
  Disabled = NativeGlfw.GLFW_CURSOR_DISABLED,

  /// <summary>
  ///    Captures the cursor, makes it visible and confines it to the content area of the window.
  /// </summary>
  Captured = NativeGlfw.GLFW_CURSOR_CAPTURED
}