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

/// <summary>
///     Represents one of many application screens, such as a main menu,
///     a settings menu, the game screen and so on.
///     Note that Dispose() is not called automatically.
/// </summary>
public interface IScreen
{
    /// <summary>
    ///     Called when this screen becomes the current screen for
    ///     a <see cref="Game" />.
    /// </summary>
    void Show();

    /// <summary>
    ///     Called when the screen should render itself.
    /// </summary>
    /// <param name="delta"> The time in seconds since the last render. </param>
    void Render( float delta );

    /// <summary>
    /// </summary>
    /// <seealso cref="IApplicationListener.Resize(int, int)" />
    void Resize( int width, int height );

    /// <summary>
    /// </summary>
    /// <seealso cref="IApplicationListener.Pause()" />
    void Pause();

    /// <summary>
    /// </summary>
    /// <seealso cref="IApplicationListener.Resume()" />
    void Resume();

    /// <summary>
    ///     Called when this screen is no longer the current screen for
    ///     a <see cref="Game" />.
    /// </summary>
    void Hide();

    /// <summary>
    ///     Called when this screen should release all resources.
    /// </summary>
    void Dispose();
}
