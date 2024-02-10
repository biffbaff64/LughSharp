// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LibGDXSharp.LibCore.Core;

using Keys = LibGDXSharp.LibCore.Core.IInput.Keys;

namespace LibGDXSharp.LibCore.Scenes.Scene2D.Utils;

public static class UIUtils
{
    public static bool Left() => Core.Gdx.Input.IsButtonPressed( IInput.Buttons.LEFT );

    public static bool Left( int button ) => button == IInput.Buttons.LEFT;

    public static bool Right() => Core.Gdx.Input.IsButtonPressed( IInput.Buttons.RIGHT );

    public static bool Right( int button ) => button == IInput.Buttons.RIGHT;

    public static bool Middle() => Core.Gdx.Input.IsButtonPressed( IInput.Buttons.MIDDLE );

    public static bool Middle( int button ) => button == IInput.Buttons.MIDDLE;

    public static bool Shift() => Core.Gdx.Input.IsKeyPressed( Keys.SHIFT_LEFT ) || Core.Gdx.Input.IsKeyPressed( Keys.SHIFT_RIGHT );

    public static bool Shift( int keycode ) => keycode is Keys.SHIFT_LEFT or Keys.SHIFT_RIGHT;

    public static bool Ctrl()
    {
        #if MACOS
        return Gdx.Input.isKeyPressed( Keys.SYM );
        #else
        return Core.Gdx.Input.IsKeyPressed( Keys.CONTROL_LEFT ) || Core.Gdx.Input.IsKeyPressed( Keys.CONTROL_RIGHT );
        #endif
    }

    public static bool Ctrl( int keycode )
    {
        #if MACOS
        return keycode == Keys.SYM;
        #else
        return keycode is Keys.CONTROL_LEFT or Keys.CONTROL_RIGHT;
        #endif
    }

    public static bool Alt() => Core.Gdx.Input.IsKeyPressed( Keys.ALT_LEFT ) || Core.Gdx.Input.IsKeyPressed( Keys.ALT_RIGHT );

    public static bool Alt( int keycode ) => keycode is Keys.ALT_LEFT or Keys.ALT_RIGHT;
}
