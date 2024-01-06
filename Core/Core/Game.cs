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
///     An ApplicationListener that delegates to a Screen. This allows an application
///     to easily have multiple screens. Screens are not disposed automatically. You
///     must handle whether you want to keep screens around or dispose of them when
///     another screen is set.
/// </summary>
public abstract class Game : IApplicationListener
{
    private IScreen? _screen;

    /// <summary>
    ///     Sets the current screen. Screen.hide() is called on any old screen,
    ///     and Screen.show() is called on the new screen, if any.
    /// </summary>
    public IScreen? Screen
    {
        get => _screen;
        set
        {
            _screen?.Hide();

            _screen = value;

            if ( _screen != null )
            {
                _screen.Show();

                _screen.Resize( Gdx.Graphics.Width, Gdx.Graphics.Height );
            }
        }
    }

    /// <summary>
    ///     Called when the <see cref="IApplication" /> is first created.
    /// </summary>
    public virtual void Create()
    {
    }

    /// <summary>
    ///     Render the currently active screen.
    /// </summary>
    public virtual void Render() => Screen?.Render( Gdx.Graphics.DeltaTime );

    /// <summary>
    ///     Called when the <see cref="IApplication" /> is resized. This can
    ///     happen at any point during a non-paused state but will never happen
    ///     before a call to <see cref="Create" />
    /// </summary>
    /// <param name="width">The new width in pixels.</param>
    /// <param name="height">The new height in pixels.</param>
    public virtual void Resize( int width, int height ) => Screen?.Resize( width, height );

    /// <summary>
    ///     Called when the <see cref="IApplication" /> is paused, usually when
    ///     it's not active or visible on-screen. An Application is also
    ///     paused before it is destroyed.
    /// </summary>
    public virtual void Pause() => Screen?.Pause();

    /// <summary>
    ///     Called when the <see cref="IApplication" /> is resumed from a paused state,
    ///     usually when it regains focus.
    /// </summary>
    public virtual void Resume() => Screen?.Resume();

    /// <summary>
    ///     Called when the <see cref="IApplication" /> is destroyed.
    ///     Preceded by a call to <see cref="Pause" />.
    /// </summary>
    public virtual void Dispose()
    {
        Screen?.Hide();
        GC.SuppressFinalize( this );
    }
}
