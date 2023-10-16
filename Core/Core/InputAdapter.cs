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
/// An adapter class for <see cref="IInputProcessor"/>.
/// You can derive from this and only override what you are interested in.
/// </summary>
[PublicAPI]
public class InputAdapter : IInputProcessor
{
    /// <inheritdoc/>
    public virtual bool KeyDown( int keycode ) => false;

    /// <inheritdoc/>
    public virtual bool KeyUp( int keycode ) => false;

    /// <inheritdoc/>
    public virtual bool KeyTyped( char character ) => false;

    /// <inheritdoc/>
    public virtual bool TouchDown( int screenX, int screenY, int pointer, int button ) => false;

    /// <inheritdoc/>
    public virtual bool TouchUp( int screenX, int screenY, int pointer, int button ) => false;

    /// <inheritdoc/>
    public virtual bool TouchDragged( int screenX, int screenY, int pointer ) => false;

    /// <inheritdoc/>
    public virtual bool MouseMoved( int screenX, int screenY ) => false;

    /// <inheritdoc/>
    public virtual bool Scrolled( float amountX, float amountY ) => false;
}
