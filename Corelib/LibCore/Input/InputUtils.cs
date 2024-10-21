// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using Corelib.LibCore.Core;
using Keys = Corelib.LibCore.Core.IInput.Keys;

namespace Corelib.LibCore.Input;

[PublicAPI]
public static class InputUtils
{
    /// <summary>
    /// Returns true if the left mouse button is pressed.
    /// </summary>
    public static bool MouseLeft()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.LEFT );
    }

    /// <summary>
    /// Returns true if the given keycode is the left mouse button.
    /// </summary>
    public static bool MouseLeft( int button )
    {
        return button == IInput.Buttons.LEFT;
    }

    /// <summary>
    /// Returns true if the right mouse button is pressed.
    /// </summary>
    public static bool MouseRight()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.RIGHT );
    }

    /// <summary>
    /// Returns true if the given keycode is the left mouse button.
    /// </summary>
    public static bool MouseRight( int button )
    {
        return button == IInput.Buttons.RIGHT;
    }

    /// <summary>
    /// Returns true if the middle mouse button is pressed.
    /// </summary>
    public static bool MouseMiddle()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.MIDDLE );
    }

    /// <summary>
    /// Returns true if the given keycode is the middle mouse button.
    /// </summary>
    public static bool MouseMiddle( int button )
    {
        return button == IInput.Buttons.MIDDLE;
    }

    /// <summary>
    /// Returns true if either of the SHIFT keys are pressed.
    /// </summary>
    public static bool ShiftKey()
    {
        return Gdx.Input.IsKeyPressed( Keys.SHIFT_LEFT ) || Gdx.Input.IsKeyPressed( Keys.SHIFT_RIGHT );
    }

    /// <summary>
    /// Returns true if the given keycode is a SHIFT key.
    /// </summary>
    public static bool ShiftKey( int keycode )
    {
        return keycode is Keys.SHIFT_LEFT or Keys.SHIFT_RIGHT;
    }

    /// <summary>
    /// Returns true if either of the CTRL keys are pressed, or SYM key on MacOS.
    /// </summary>
    public static bool CtrlKey()
    {
#if MACOS
        return Gdx.Input.isKeyPressed( Keys.SYM );
#else
        return Gdx.Input.IsKeyPressed( Keys.CONTROL_LEFT ) || Gdx.Input.IsKeyPressed( Keys.CONTROL_RIGHT );
#endif
    }

    /// <summary>
    /// Returns true if the given keycode is a CTRL key, or SYM key on MacOS.
    /// </summary>
    public static bool CtrlKey( int keycode )
    {
#if MACOS
        return keycode == Keys.SYM;
#else
        return keycode is Keys.CONTROL_LEFT or Keys.CONTROL_RIGHT;
#endif
    }

    /// <summary>
    /// Returns true if either of the ALT keys are pressed.
    /// </summary>
    public static bool AltKey()
    {
        return Gdx.Input.IsKeyPressed( Keys.ALT_LEFT ) || Gdx.Input.IsKeyPressed( Keys.ALT_RIGHT );
    }

    /// <summary>
    /// Returns true if the given keycode is an ALT key.
    /// </summary>
    public static bool AltKey( int keycode )
    {
        return keycode is Keys.ALT_LEFT or Keys.ALT_RIGHT;
    }
}
