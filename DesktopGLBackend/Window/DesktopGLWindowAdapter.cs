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

using JetBrains.Annotations;

namespace DesktopGLBackend.Window;

/// <summary>
/// Convenience implementation of <see cref="IDesktopGLWindowListener"/>.
/// Derive from this class and only override the methods you are interested in.
/// </summary>
[PublicAPI]
public class DesktopGLWindowAdapter : IDesktopGLWindowListener
{
    /// <inheritdoc />
    public virtual void Created( DesktopGLWindow window )
    {
    }

    /// <inheritdoc />
    public virtual void Iconified( bool isIconified )
    {
    }

    /// <inheritdoc />
    public virtual void Maximized( bool isMaximized )
    {
    }

    /// <inheritdoc />
    public virtual void FocusLost()
    {
    }

    /// <inheritdoc />
    public virtual void FocusGained()
    {
    }

    /// <inheritdoc />
    public virtual bool CloseRequested()
    {
        return false;
    }

    /// <inheritdoc />
    public virtual void FilesDropped( string[] files )
    {
    }

    /// <inheritdoc />
    public virtual void RefreshRequested()
    {
    }
}
