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

using System.Runtime.InteropServices;

namespace DesktopGLBackend.Window;

public unsafe partial class DesktopGLWindow
{
    public void GdxFocusCallback( GLFW.Window windowHandle, bool focused )
    {
        if ( WindowListener != null )
        {
            if ( focused )
            {
                WindowListener.FocusGained();
            }
            else
            {
                WindowListener.FocusLost();
            }

            _focused = focused;
        }
    }

    public void GdxIconifyCallback( GLFW.Window windowHandle, bool iconified )
    {
        if ( WindowListener != null )
        {
            WindowListener.Iconified( iconified );
        }

        _iconified = iconified;

        if ( iconified )
        {
            ApplicationListener.Pause();
        }
        else
        {
            ApplicationListener.Resume();
        }
    }

    public void GdxMaximizeCallback( GLFW.Window windowHandle, bool maximized )
    {
        if ( WindowListener != null )
        {
            WindowListener.Maximized( maximized );
        }
    }

    public void GdxWindowCloseCallback( GLFW.Window windowHandle )
    {
        if ( WindowListener != null )
        {
            if ( !WindowListener.CloseRequested() )
            {
                Glfw.SetWindowShouldClose( windowHandle, false );
            }
        }
    }

    public void GdxDropCallback( GLFW.Window window, string[] paths )
    {
        var files = new string[ paths.Length ];

        Array.Copy( paths, 0, files, 0, paths.Length );

        if ( WindowListener != null )
        {
            WindowListener.FilesDropped( files );
        }
    }

    public void GdxRefreshCallback( GLFW.Window window )
    {
        if ( WindowListener != null )
        {
            WindowListener.RefreshRequested();
        }
    }
}