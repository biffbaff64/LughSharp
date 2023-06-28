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

public static class UIUtils
{
    public static bool Left()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.Left );
    }

    public static bool Left( int button )
    {
        return button == IInput.Buttons.Left;
    }

    public static bool Right()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.Right );
    }

    public static bool Right( int button )
    {
        return button == IInput.Buttons.Right;
    }

    public static bool Middle()
    {
        return Gdx.Input.IsButtonPressed( IInput.Buttons.Middle );
    }

    public static bool Middle( int button )
    {
        return button == IInput.Buttons.Middle;
    }

    public static bool Shift()
    {
        return Gdx.Input.IsKeyPressed( IInput.Keys.Shift_Left ) || Gdx.Input.IsKeyPressed( IInput.Keys.Shift_Right );
    }

    public static bool Shift( int keycode )
    {
        return keycode is Keys.Shift_Left or Keys.Shift_Right;
    }

    public static bool Ctrl()
    {
        #if MACOS
            return Gdx.Input.isKeyPressed( Keys.SYM );
        #else        
            return Gdx.Input.IsKeyPressed( Keys.Control_Left ) || Gdx.Input.IsKeyPressed( Keys.Control_Right );
        #endif
    }

    public static bool Ctrl( int keycode )
    {
        #if MACOS
            return keycode == Keys.SYM;
        #else
            return keycode is Keys.Control_Left or Keys.Control_Right;
        #endif
    }

    public static bool Alt()
    {
        return Gdx.Input.IsKeyPressed( Keys.Alt_Left ) || Gdx.Input.IsKeyPressed( Keys.Alt_Right );
    }

    public static bool Alt( int keycode )
    {
        return keycode is Keys.Alt_Left or Keys.Alt_Right;
    }
}