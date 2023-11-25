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

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class DesktopGLWindowConfiguration
{
    public int       WindowX            { get; private set; } = -1;
    public int       WindowY            { get; private set; } = -1;
    public int       WindowWidth        { get; private set; } = 640;
    public int       WindowHeight       { get; private set; } = 480;
    public int       WindowMinWidth     { get; private set; } = -1;
    public int       WindowMinHeight    { get; private set; } = -1;
    public int       WindowMaxWidth     { get; private set; } = -1;
    public int       WindowMaxHeight    { get; private set; } = -1;
    public FileType  WindowIconFileType { get; set; }
    public string[]? WindowIconPaths    { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="config"></param>
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
        InitialVisible         = config.InitialVisible;
        VSyncEnabled           = config.VSyncEnabled;
    }

    /// <summary>
    /// </summary>
    /// <param name="value">
    /// whether the window will be visible on creation. (default true)
    /// </param>
    public bool InitialVisible { get; set; } = true;

    /// <summary>
    /// Sets the app to use windowed mode.
    /// </summary>
    /// <param name="width"> the width of the window (default 640) </param>
    /// <param name="height"> the height of the window (default 480) </param>
    public void SetWindowedMode( int width, int height )
    {
        this.WindowWidth  = width;
        this.WindowHeight = height;
    }

    /// <summary>
    /// </summary>
    /// <param name="value">
    /// whether the windowed mode window is resizable (default true)
    /// </param>
    public bool WindowResizable { get; set; } = true;

    /// <summary>
    /// </summary>
    /// <param name="value">
    /// whether the windowed mode window is decorated, i.e. displaying
    /// the title bars (default true)
    /// </param>
    public bool WindowDecorated { get; set; } = true;

    /// <summary>
    /// </summary>
    /// <param name="value">
    /// whether the window starts maximized.
    /// Ignored if the window is full screen. (default false)
    /// </param>
    public bool WindowMaximized { get; set; } = false;

    /// <summary>
    /// </summary>
    /// <param name="value"> what monitor the window should maximize to. </param>
    public DesktopGLGraphics.GLMonitor? MaximizedMonitor { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="value">
    /// whether the window should automatically iconify and restore previous
    /// video mode on input focus loss. (default false).
    /// Does nothing in windowed mode.
    /// </param>
    public bool AutoIconify { get; set; } = false;

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
    /// Sets the icon that will be used in the window's title bar. Has no effect in
    /// macOS, which doesn't use window icons.
    /// </summary>
    /// <param name="filePaths">
    /// One or more internal image paths. Must be JPEG, PNG, or BMP format.
    /// The one closest to the system's desired size will be scaled. Good
    /// sizes include 16x16, 32x32 and 48x48.
    /// </param>
    public void SetWindowIcon( params string[] filePaths )
    {
        SetWindowIcon( FileType.Internal, filePaths );
    }

    /// <summary>
    /// Sets the icon that will be used in the window's title bar. Has no effect in macOS,
    /// which doesn't use window icons.
    /// </summary>
    /// <param name="fileType"> The type of file handle the paths are relative to. </param>
    /// <param name="filePaths">
    /// One or more image paths, relative to the given {@linkplain FileType}. Must be JPEG,
    /// PNG, or BMP format. The one closest to the system's desired size will be scaled.
    /// Good sizes include 16x16, 32x32 and 48x48.
    /// </param>
    public void SetWindowIcon( FileType fileType, params string[] filePaths )
    {
        WindowIconFileType = fileType;
        WindowIconPaths    = filePaths;
    }

    /// <summary>
    /// Sets the <see cref="IDesktopGLWindowListener"/>" which will be informed about
    /// iconficiation, focus loss and window close events.
    /// </summary>
    public IDesktopGLWindowListener? WindowListener { get; set; }

    /// <summary>
    /// Sets the app to use fullscreen mode. Use the static methods like
    /// <see cref="DesktopGLApplicationConfiguration.GetDisplayMode()"/> on this
    /// class to enumerate connected monitors and their fullscreen display modes.
    /// </summary>
    public DesktopGLGraphics.GLDisplayMode? FullscreenMode { get; set; }

    /// <summary>
    /// Sets the window title. Defaults to empty string.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Sets the initial background color. Defaults to black.
    /// </summary>
    public Color InitialBackgroundColor { get; set; } = Color.Black;

    /// <summary>
    /// Sets whether to use vsync. This setting can be changed anytime at runtime
    /// via <see cref="IGraphics.SetVSync(bool)"/>.
    /// <para>
    /// For multi-window applications, only one (the main) window should enable vsync.
    /// Otherwise, every window will wait for the vertical blank on swap individually,
    /// effectively cutting the frame rate to (refreshRate / numberOfWindows).
    /// </para>
    /// </summary>
    public bool VSyncEnabled { get; set; } = true;
}
