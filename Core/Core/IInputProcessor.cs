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

namespace LibGDXSharp.Core;

[PublicAPI]
public interface IInputProcessor
{
    /// <summary>
    /// Called when a key is pressed.
    /// </summary>
    /// <param name="keycode">One of the constants in <see cref="IInput.Keys"/></param>
    /// <returns>TRUE if the input was processed.</returns>
    bool KeyDown( int keycode );

    /// <summary>
    /// Called when a key is released.
    /// </summary>
    /// <param name="keycode">One of the constants in <see cref="IInput.Keys"/></param>
    /// <returns>TRUE if the input was processed.</returns>
    bool KeyUp( int keycode );

    /// <summary>
    /// Called when a key was typed
    /// </summary>
    /// <param name="character"></param>
    /// <returns>TRUE if the input was processed.</returns>
    bool KeyTyped( char character );

    /// <summary>
    /// Called when the screen was touched or a mouse button was pressed.
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="pointer"></param>
    /// <param name="button"></param>
    /// <returns>TRUE if the input was processed.</returns>
    bool TouchDown( int screenX, int screenY, int pointer, int button );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="pointer"></param>
    /// <param name="button"></param>
    /// <returns>TRUE if the input was processed.</returns>
    bool TouchUp( int screenX, int screenY, int pointer, int button );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="pointer"></param>
    /// <returns>TRUE if the input was processed.</returns>
    bool TouchDragged( int screenX, int screenY, int pointer );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <returns>TRUE if the input was processed.</returns>
    bool MouseMoved( int screenX, int screenY );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amountX"></param>
    /// <param name="amountY"></param>
    /// <returns>TRUE if the input was processed.</returns>
    bool Scrolled( float amountX, float amountY );
}