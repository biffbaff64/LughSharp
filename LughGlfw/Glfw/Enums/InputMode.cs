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
/// All possible input modes.
/// </summary>
public static class InputMode
{
  /// <summary>
  /// Input mode for cursors.
  /// </summary>
  public static readonly InputModeType<CursorMode> Cursor = new InputModeType<CursorMode>(NativeGlfw.GLFW_CURSOR);

  /// <summary>
  /// Input mode for sticky keys.
  /// </summary>
  public static readonly InputModeType<bool> StickyKeys = new InputModeType<bool>(NativeGlfw.GLFW_STICKY_KEYS);

  /// <summary>
  /// Input mode for sticky mouse buttons.
  /// </summary>
  public static readonly InputModeType<bool> StickyMouseButtons = new InputModeType<bool>(NativeGlfw.GLFW_STICKY_MOUSE_BUTTONS);

  /// <summary>
  /// Input mode for lock key mods.
  /// </summary>
  public static readonly InputModeType<bool> LockKeyMods = new InputModeType<bool>(NativeGlfw.GLFW_LOCK_KEY_MODS);

  /// <summary>
  /// Input mode for raw mouse motion.
  /// </summary>
  public static readonly InputModeType<bool> RawMouseMotion = new InputModeType<bool>(NativeGlfw.GLFW_RAW_MOUSE_MOTION);
}

/// <summary>
/// Wrapper class for window hints that only accept certain values.
/// Is used to make it easier for developers to see what values are expected or allowed.
/// </summary>
public class InputModeType<T>
{
  internal int Mode { get; }

  internal InputModeType(int mode)
  {
    Mode = mode;
  }
}