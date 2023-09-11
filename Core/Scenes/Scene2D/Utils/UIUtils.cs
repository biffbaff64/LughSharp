// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using Keys = LibGDXSharp.Core.IInput.Keys;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

[PublicAPI]
public static class UIUtils
{
    public static bool Left()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.LEFT );
    }

    public static bool Left( int button )
    {
        return button == IInput.Buttons.LEFT;
    }

    public static bool Right()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.RIGHT );
    }

    public static bool Right( int button )
    {
        return button == IInput.Buttons.RIGHT;
    }

    public static bool Middle()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.MIDDLE );
    }

    public static bool Middle( int button )
    {
        return button == IInput.Buttons.MIDDLE;
    }

    public static bool Shift()
    {
        return Gdx.Input.IsKeyPressed( IInput.Keys.SHIFT_LEFT ) || Gdx.Input.IsKeyPressed( IInput.Keys.SHIFT_RIGHT );
    }

    public static bool Shift( int keycode )
    {
        return keycode is Keys.SHIFT_LEFT or Keys.SHIFT_RIGHT;
    }

    public static bool Ctrl()
    {
        #if MACOS
        return Gdx.Input.isKeyPressed( Keys.SYM );
        #else
        return Gdx.Input.IsKeyPressed( Keys.CONTROL_LEFT ) || Gdx.Input.IsKeyPressed( Keys.CONTROL_RIGHT );
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

    public static bool Alt()
    {
        return Gdx.Input.IsKeyPressed( Keys.ALT_LEFT ) || Gdx.Input.IsKeyPressed( Keys.ALT_RIGHT );
    }

    public static bool Alt( int keycode )
    {
        return keycode is Keys.ALT_LEFT or Keys.ALT_RIGHT;
    }
}
