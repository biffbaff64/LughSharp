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
/// Convenience implementation of <see cref="IApplicationListener"/>.
/// Derive from this and only override what you need.
/// </summary>
[PublicAPI]
public class ApplicationAdapter : IApplicationListener, IDisposable
{
    /// <summary>
    /// Called when the <see cref="IApplication"/> is first created.
    /// </summary>
    public virtual void Create()
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> is resized. This can
    /// happen at any point during a non-paused state but will never happen
    /// before a call to <see cref="IApplicationListener.Create"/>
    /// </summary>
    /// <param name="width">The new width in pixels.</param>
    /// <param name="height">The new height in pixels.</param>
    public virtual void Resize( int width, int height )
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> should draw itself.
    /// </summary>
    public virtual void Render()
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> is paused, usually when
    /// it's not active or visible on-screen. An Application is also
    /// paused before it is destroyed.
    /// </summary>
    public virtual void Pause()
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> is resumed from a paused state,
    /// usually when it regains focus.
    /// </summary>
    public virtual void Resume()
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> is destroyed.
    /// Preceded by a call to <see cref="IApplicationListener.Pause"/>.
    /// </summary>
    public virtual void Dispose()
    {
    }
}
