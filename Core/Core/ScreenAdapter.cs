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
/// Convenience implementation of <see cref="IScreen"/>. Derive from this and
/// only override what you need.
/// <para>
/// Using this means that any app screens use only what they need and don't
/// duplicate unnecessary code. 
/// </para>
/// </summary>
[PublicAPI]
public class ScreenAdapter : IScreen
{
    /// <inheritdoc cref="IScreen.Show"/>
    public virtual void Show()
    {
    }

    /// <inheritdoc cref="IScreen.Render"/>
    public virtual void Render( float delta )
    {
    }

    /// <inheritdoc cref="IScreen.Resize"/>
    public virtual void Resize( int width, int height )
    {
    }

    /// <inheritdoc cref="IScreen.Pause"/>
    public virtual void Pause()
    {
    }

    /// <inheritdoc cref="IScreen.Resume"/>
    public virtual void Resume()
    {
    }

    /// <inheritdoc cref="IScreen.Hide"/>
    public virtual void Hide()
    {
    }

    /// <inheritdoc cref="IScreen.Dispose"/>
    public virtual void Dispose()
    {
    }
}