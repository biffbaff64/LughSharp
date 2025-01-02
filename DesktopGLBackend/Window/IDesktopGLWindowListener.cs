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

using LughSharp.Lugh.Core;
using DesktopGLBackend.Core;
using DesktopGLBackend.Graphics;
using JetBrains.Annotations;

namespace DesktopGLBackend.Window;

[PublicAPI]
public interface IDesktopGLWindowListener
{
    /// <summary>
    /// Called after the Glfw window is created. Before this callback is received, it's
    /// to use any <see cref="DesktopGLWindow"/> member functions which, for their part,
    /// involve calling Glfw functions.
    /// <para>
    /// For the main window, this is an immediate callback from inside
    /// <see
    ///     cref="DesktopGLApplication(IApplicationListener,DesktopGLApplicationConfiguration)"/>
    /// </para>
    /// </summary>
    /// <param name="window">the window instance.</param>
    void Created( DesktopGLWindow window );

    /// <summary>
    /// Called when the window is iconified (i.e. its minimize button was clicked),
    /// or when restored from the iconified state. When a window becomes iconified, its
    /// <see cref="IApplicationListener"/> will be paused, and when restored it will
    /// be resumed.
    /// </summary>
    /// <param name="isIconified">
    /// True if window is iconified, false if it leaves the iconified state
    /// </param>
    void Iconified( bool isIconified );

    /// <summary>
    /// Called when the window is maximized, or restored from the maximized state.
    /// </summary>
    /// <param name="isMaximized">
    /// true if window is maximized, false if it leaves the maximized state
    /// </param>
    void Maximized( bool isMaximized );

    /// <summary>
    /// Called when the window lost focus to another window. The window's
    /// <see cref="IApplicationListener"/> will continue to be called.
    /// </summary>
    void FocusLost();

    /// <summary>
    /// Called when the window gained focus.
    /// </summary>
    void FocusGained();

    /// <summary>
    /// Called when the user requested to close the window, e.g. clicking
    /// the close button or pressing the window closing keyboard shortcut.
    /// </summary>
    /// <returns> whether the window should actually close </returns>
    bool CloseRequested();

    /// <summary>
    /// Called when external files are dropped into the window, e.g from the Desktop.
    /// </summary>
    /// <param name="files"> array with absolute paths to the files </param>
    void FilesDropped( string[] files );

    /// <summary>
    /// Called when the window content is damaged and needs to be refreshed.
    /// When this occurs, <see cref="DesktopGLGraphics.RequestRendering"/> is automatically called.
    /// </summary>
    void RefreshRequested();
}
