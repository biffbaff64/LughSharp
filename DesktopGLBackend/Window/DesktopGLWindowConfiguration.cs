// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Core;
using Corelib.LibCore.Graphics;
using DesktopGLBackend.Core;

namespace DesktopGLBackend.Window;

[PublicAPI]
public class DesktopGLWindowConfiguration
{
    public int       WindowX            { get; set; } = -1;
    public int       WindowY            { get; set; } = -1;
    public int       WindowWidth        { get; set; } = 640;
    public int       WindowHeight       { get; set; } = 480;
    public int       WindowMinWidth     { get; set; } = -1;
    public int       WindowMinHeight    { get; set; } = -1;
    public int       WindowMaxWidth     { get; set; } = -1;
    public int       WindowMaxHeight    { get; set; } = -1;
    public PathTypes WindowIconFileType { get; set; }
    public string[]? WindowIconPaths    { get; set; }

    public DesktopGLGraphics.DesktopGLMonitor? MaximizedMonitor { get; set; }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// whether the window will be visible on creation. (default true)
    /// </summary>
    public bool InitialVisibility { get; set; } = true;

    /// <summary>
    /// Sets the <see cref="IDesktopGLWindowListener"/> which will be informed about
    /// iconficiation, focus loss and window close events.
    /// </summary>
    public IDesktopGLWindowListener? WindowListener { get; set; }

    /// <summary>
    /// Sets the app to use fullscreen mode.
    /// <para>
    /// Use the static methods like <see cref="DesktopGLApplicationConfiguration.GetDisplayMode()"/>
    /// on this class to enumerate connected monitors and their fullscreen display modes.
    /// </para>
    /// </summary>
    public DesktopGLGraphics.DesktopGLDisplayMode? FullscreenMode { get; set; }

    /// <summary>
    /// Sets whether to use vsync.
    /// <para>
    /// This setting can be changed anytime at runtime via <see cref="IGraphics.SetVSync(bool)"/>.
    /// </para>
    /// <para>
    /// For multi-window applications, only one (the main) window should enable vsync. Otherwise,
    /// every window will wait for the vertical blank on swap individually, effectively cutting
    /// the frame rate to (refreshRate / numberOfWindows).
    /// </para>
    /// </summary>
    public bool VSyncEnabled { get; set; } = true;

    /// <summary>
    /// whether the windowed mode window is resizable (default true)
    /// </summary>
    public bool WindowResizable { get; set; } = true;

    /// <summary>
    /// whether the windowed mode window is decorated, i.e. displaying the title bars.
    /// (default true)
    /// </summary>
    public bool WindowDecorated { get; set; } = true;

    /// <summary>
    /// whether the window starts maximized. Ignored if the window is full screen.
    /// (default false)
    /// </summary>
    public bool WindowMaximized { get; set; } = false;

    /// <summary>
    /// whether the window should automatically iconify and restore previous video mode
    /// on input focus loss. (default false). Does nothing in windowed mode.
    /// (default false)
    /// </summary>
    public bool AutoIconify { get; set; } = false;

    /// <summary>
    /// Sets the initial background color. Defaults to black.
    /// </summary>
    public Color InitialBackgroundColor { get; set; } = Color.Black;

    /// <summary>
    /// Sets the window title. Defaults to empty string.
    /// </summary>
    public string? Title { get; set; } = "";

    /// <summary>
    /// Sets this windows configuration settings.
    /// </summary>
    /// <param name="config">
    /// The window configuration data from which to initialise this window config.
    /// </param>
    public void SetWindowConfiguration( DesktopGLWindowConfiguration config )
    {
        WindowX            = config.WindowX;
        WindowY            = config.WindowY;
        WindowWidth        = config.WindowWidth;
        WindowHeight       = config.WindowHeight;
        WindowMinWidth     = config.WindowMinWidth;
        WindowMinHeight    = config.WindowMinHeight;
        WindowMaxWidth     = config.WindowMaxWidth;
        WindowMaxHeight    = config.WindowMaxHeight;
        WindowResizable    = config.WindowResizable;
        WindowDecorated    = config.WindowDecorated;
        WindowMaximized    = config.WindowMaximized;
        MaximizedMonitor   = config.MaximizedMonitor;
        AutoIconify        = config.AutoIconify;
        WindowIconFileType = config.WindowIconFileType;

        if ( config.WindowIconPaths != null )
        {
            WindowIconPaths = new string[ config.WindowIconPaths.Length ];

            Array.Copy( config.WindowIconPaths, WindowIconPaths, config.WindowIconPaths.Length );
        }

        WindowListener         = config.WindowListener;
        FullscreenMode         = config.FullscreenMode;
        Title                  = config.Title;
        InitialBackgroundColor = config.InitialBackgroundColor;
        InitialVisibility      = config.InitialVisibility;
        VSyncEnabled           = config.VSyncEnabled;
    }

    /// <summary>
    /// Sets the icon that will be used in the window's title bar. Has no effect in
    /// macOS, which doesn't use window icons.
    /// </summary>
    /// <param name="filePaths">
    /// One or more internal image paths. Must be JPEG, PNG, or BMP format. The one
    /// closest to the system's desired size will be scaled. Good sizes include 16x16,
    /// 32x32 and 48x48.
    /// </param>
    public void SetWindowIcon( params string[] filePaths )
    {
        SetWindowIcon( PathTypes.Internal, filePaths );
    }

    /// <summary>
    /// Sets the app to use windowed mode.
    /// </summary>
    /// <param name="width"> the width of the window (default 640) </param>
    /// <param name="height">the height of the window (default 480) </param>
    public void SetWindowedMode( int width, int height )
    {
        WindowWidth  = width;
        WindowHeight = height;
    }

    /// <summary>
    /// Sets the position of the window in windowed mode.
    /// Default -1 for both coordinates for centered on primary monitor.
    /// </summary>
    public void SetWindowPosition( int x, int y )
    {
        WindowX = x;
        WindowY = y;
    }

    /// <summary>
    /// Sets minimum and maximum size limits for the window. If the window is full
    /// screen or not resizable, these limits are ignored. The default for all four
    /// parameters is -1, which means unrestricted.
    /// </summary>
    public void SetWindowSizeLimits( int minWidth, int minHeight, int maxWidth, int maxHeight )
    {
        WindowMinWidth  = minWidth;
        WindowMinHeight = minHeight;
        WindowMaxWidth  = maxWidth;
        WindowMaxHeight = maxHeight;
    }

    /// <summary>
    /// Sets the icon that will be used in the window's title bar.Has no effect in macOS,
    /// which doesn't use window icons.
    /// </summary>
    /// <param name="fileType"> The type of file handle the paths are relative to. </param>
    /// <param name="filePaths">
    /// One or more image paths, relative to the given <see cref="PathTypes"/>. Must be JPEG,
    /// PNG, or BMP format. The one closest to the system's desired size will be scaled.
    /// Good sizes include 16x16, 32x32 and 48x48.
    /// </param>
    public void SetWindowIcon( PathTypes fileType, params string[] filePaths )
    {
        WindowIconFileType = fileType;
        WindowIconPaths    = filePaths;
    }
}