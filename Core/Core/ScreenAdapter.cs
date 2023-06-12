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
/// Convenience implementation of <see cref="IScreen"/>.
/// Derive from this and only override what you need.
/// </summary>
public class ScreenAdapter : IScreen
{
    public virtual void Show()
    {
    }

    public virtual void Render( float delta )
    {
    }

    public virtual void Resize( int width, int height )
    {
    }

    public virtual void Pause()
    {
    }

    public virtual void Resume()
    {
    }

    public virtual void Hide()
    {
    }

    public virtual void Dispose()
    {
    }
}