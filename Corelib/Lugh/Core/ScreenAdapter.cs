// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace Corelib.Lugh.Core;

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

    /// <inheritdoc cref="IScreen.Update"/>
    public virtual void Update( float delta )
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

    /// <inheritdoc cref="IScreen.Close"/>
    public virtual void Close()
    {
    }
}
