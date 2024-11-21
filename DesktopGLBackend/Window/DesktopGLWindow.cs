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

using System;
using System.Collections.Generic;

using Corelib.LibCore.Core;
using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;

using DesktopGLBackend.Core;
using DesktopGLBackend.Graphics;
using DesktopGLBackend.Input;
using DesktopGLBackend.Utils;

using JetBrains.Annotations;

using Platform = Corelib.LibCore.Core.Platform;

namespace DesktopGLBackend.Window;

/// <summary>
/// Wrapper/Manager class for a <see cref="GLFW.Window"/>.
/// </summary>
[PublicAPI]
public class DesktopGLWindow : IDisposable
{
    public GLFW.Window?                      GlfwWindow          { get; set; }
    public IDesktopGLWindowListener?         WindowListener      { get; set; }
    public IApplicationListener              Listener            { get; set; }
    public IDesktopGLInput                   Input               { get; set; } = null!;
    public DesktopGLApplicationConfiguration AppConfig           { get; set; }
    public DesktopGLGraphics                 Graphics            { get; set; } = null!;
    public bool                              ListenerInitialised { get; set; } = false;

    /// <summary>
    /// Return the window X position in logical coordinates. All monitors span a virtual
    /// surface together. The coordinates are relative to the first monitor in the
    /// virtual surface.
    /// </summary>
    public int PositionX => ( int )GetPosition().X;

    /// <summary>
    /// Return the window Y position in logical coordinates. All monitors span a virtual
    /// surface together. The coordinates are relative to the first monitor in the
    /// virtual surface.
    /// </summary>
    public int PositionY => ( int )GetPosition().Y;

    // ========================================================================

    private readonly List< IRunnable.Runnable > _executedRunnables = [ ];
    private readonly bool                       _iconified         = false;
    private readonly List< IRunnable.Runnable > _runnables         = [ ];
    private readonly Vector2                    _tmpV2             = new();

    private bool _requestRendering = false;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a new DesktopGLWindow instance, using the given <see cref="IApplicationListener"/>,
    /// <see cref="DesktopGLApplicationConfiguration"/>, and attaching it to the given
    /// <see cref="IDesktopGLApplicationBase"/>.
    /// </summary>
    public DesktopGLWindow( IApplicationListener listener,
                            DesktopGLApplicationConfiguration config,
                            IDesktopGLApplicationBase application )
    {
        Logger.Checkpoint();

        Listener       = listener;
        WindowListener = config.WindowListener;
        AppConfig      = DesktopGLApplicationConfiguration.Copy( config );
    }

    /// <summary>
    /// 
    /// </summary>
    public void Initialise( GLFW.Window? window, IDesktopGLApplicationBase app )
    {
        Logger.Checkpoint();
        Logger.Debug( $"window: {window?.GetHandle()}, app: {app}" );

        this.GlfwWindow         = window;
        this.Input              = app.CreateInput( this );
        this.Graphics           = new DesktopGLGraphics( this );
        this.Graphics.GLVersion = app.GLVersion;

        Logger.Checkpoint();

        //@formatter:off
        Glfw.SetWindowFocusCallback     ( window, DesktopWindowCallbacks.GdxFocusCallback );
        Glfw.SetWindowIconifyCallback   ( window, DesktopWindowCallbacks.GdxIconifyCallback );
        Glfw.SetWindowMaximizeCallback  ( window, DesktopWindowCallbacks.GdxMaximizeCallback );
        Glfw.SetWindowCloseCallback     ( window, DesktopWindowCallbacks.GdxWindowCloseCallback );
        Glfw.SetDropCallback            ( window, DesktopWindowCallbacks.GdxDropCallback );
        Glfw.SetWindowRefreshCallback   ( window, DesktopWindowCallbacks.GdxRefreshCallback );
        //@formatter:on

        Logger.Checkpoint();

        WindowListener?.Created( this );

        Logger.Checkpoint();
    }

    /// <summary>
    /// Update this window.
    /// </summary>
    /// <returns> True if the window should render itself. </returns>
    public bool Update()
    {
        if ( !ListenerInitialised )
        {
            InitialiseListener();
        }

        lock ( _runnables )
        {
            foreach ( var runnable in _runnables )
            {
                _executedRunnables.Add( runnable );
            }

            _runnables.Clear();
        }

        foreach ( var runnable in _executedRunnables )
        {
            runnable();
        }

        var shouldRender = ( _executedRunnables.Count > 0 ) || Graphics.ContinuousRendering;

        _executedRunnables.Clear();

        if ( !_iconified )
        {
            Input.Update();
        }

        lock ( this )
        {
            shouldRender      |= _requestRendering && !_iconified;
            _requestRendering =  false;
        }

        if ( shouldRender )
        {
            Graphics.Update();
            Listener.Update();
            Listener.Render();

            Glfw.SwapBuffers( GlfwWindow );
        }

        if ( !_iconified )
        {
            Input.PrepareNext();
        }

        return shouldRender;
    }

    /// <summary>
    /// Post a <see cref="IRunnable.Runnable"/> to this window's event queue. Use this if
    /// you access statics like <see cref="Gdx.Graphics"/> in your runnable instead
    /// of <see cref="DesktopGLApplication.PostRunnable(IRunnable.Runnable)"/>".
    /// </summary>
    public void PostRunnable( IRunnable.Runnable runnable )
    {
        lock ( _runnables )
        {
            _runnables.Add( runnable );
        }
    }

    /// <summary>
    /// Makes this the currently active window.
    /// </summary>
    public void MakeCurrent()
    {
        Gdx.Graphics = Graphics;
        Gdx.Input    = Input;

        Glfw.MakeContextCurrent( GlfwWindow );
    }

    /// <summary>
    /// Reguest the window to be drawn.
    /// </summary>
    public void RequestRendering()
    {
        lock ( this )
        {
            _requestRendering = true;
        }
    }

    /// <summary>
    /// Returns <b>true</b> if this window should close. It establishes this
    /// via <see cref="Glfw.WindowShouldClose(Window)"/>
    /// </summary>
    /// <returns></returns>
    public bool ShouldClose()
    {
        return Glfw.WindowShouldClose( GlfwWindow );
    }

    /// <inheritdoc cref="Glfw.SetWindowPos(Window,int,int)"/>
    public void SetPosition( int x, int y )
    {
        Glfw.SetWindowPos( GlfwWindow, x, y );
    }

    /// <summary>
    /// Gets the current window position in logical coordinates. All monitors span a
    /// virtual surface together. The coordinates are relative to the first monitor
    /// in the virtual surface.
    /// </summary>
    /// <returns>A Vector2 holding the window X and Y.</returns>
    public Vector2 GetPosition()
    {
        Glfw.GetWindowPos( GlfwWindow, out var xPos, out var yPos );

        return _tmpV2.Set( xPos, yPos );
    }

    /// <summary>
    /// Sets the visibility of the window.
    /// Invisible windows will still call their <see cref="IApplicationListener"/>
    /// </summary>
    public void SetVisible( bool visible )
    {
        if ( visible )
        {
            Glfw.ShowWindow( GlfwWindow );
        }
        else
        {
            Glfw.HideWindow( GlfwWindow );
        }
    }

    // ========================================================================

    /// <summary>
    /// Closes this window and pauses and disposes the associated <see cref="IApplicationListener"/>.
    /// This function sets the value of the close flag of the specified window. This can be used to
    /// override the user's attempt to close the window, or to signal that it should be closed.
    /// </summary>
    public void CloseWindow() => Glfw.SetWindowShouldClose( GlfwWindow, true );

    /// <summary>
    /// Minimizes (iconifies) the window. Iconified windows do not call their
    /// <see cref="IApplicationListener"/> until the window is restored.
    /// </summary>
    public void IconifyWindow() => Glfw.IconifyWindow( GlfwWindow );

    /// <summary>
    /// This function restores the specified window if it was previously iconified
    /// (minimized) or maximized. If the window is already restored, this function
    /// does nothing. If the specified window is a full screen window, the resolution
    /// chosen for the window is restored on the selected monitor.
    /// </summary>
    public void RestoreWindow() => Glfw.RestoreWindow( GlfwWindow );

    /// <summary>
    /// This function maximizes the specified window if it was previously not maximized.
    /// If the window is already maximized, this function does nothing. If the specified
    /// window is a full screen window, this function does nothing.
    /// </summary>
    public void MaximizeWindow() => Glfw.MaximizeWindow( GlfwWindow );

    /// <summary>
    /// Brings the window to front and sets input focus. The window should
    /// already be visible and not iconified.
    /// </summary>
    public void FocusWindow() => Glfw.FocusWindow( GlfwWindow );

    // ========================================================================

    /// <summary>
    /// Sets the windows title.
    /// </summary>
    /// <param name="title"> String holding the Title text. </param>
    public void SetTitle( string title )
    {
        Glfw.SetWindowTitle( GlfwWindow, title );
    }

    /// <summary>
    /// Sets minimum and maximum size limits for the window. If the window
    /// is full screen or not resizable, these limits are ignored. Use -1
    /// to indicate an unrestricted dimension.
    /// </summary>
    public void SetSizeLimits( int minWidth, int minHeight, int maxWidth, int maxHeight )
    {
        SetSizeLimits( GlfwWindow!, minWidth, minHeight, maxWidth, maxHeight );
    }

    /// <summary>
    /// Sets minimum and maximum size limits for the given window. If the window
    /// is full screen or not resizable, these limits are ignored.
    /// Use -1 to indicate an unrestricted dimension.
    /// </summary>
    /// <param name="handle"> The window. </param>
    /// <param name="minWidth"> The minimum window width. </param>
    /// <param name="minHeight"> The minimum window height. </param>
    /// <param name="maxWidth"> The maximum window width. </param>
    /// <param name="maxHeight"> The maximum window height. </param>
    public static void SetSizeLimits( GLFW.Window handle, int minWidth, int minHeight, int maxWidth, int maxHeight )
    {
        Glfw.SetWindowSizeLimits( handle,
                                  minWidth > -1 ? minWidth : -1,
                                  minHeight > -1 ? minHeight : -1,
                                  maxWidth > -1 ? maxWidth : -1,
                                  maxHeight > -1 ? maxHeight : -1 );
    }

    /// <summary>
    /// Sets the icon that will be used in the window's title bar. Has no effect in macOS,
    /// which doesn't use window icons.
    /// </summary>
    /// <param name="images">
    /// One or more images. The one closest to the system's desired size will be scaled.
    /// Good sizes include 16x16, 32x32 and 48x48. Pixmap format <see cref="Pixmap.ColorFormat.RGBA8888"/>
    /// is preferred so the images will not have to be copied and converted. <b>The chosen image
    /// is copied, and the provided Pixmaps are not disposed.</b>
    /// </param>
    public void SetIcon( params Pixmap[] images )
    {
        if ( GlfwWindow != null )
        {
            SetIcon( GlfwWindow, images );
        }
    }

    /// <summary>
    /// Sets the icon that will be used in the window's title bar. Has no effect in macOS,
    /// which doesn't use window icons.
    /// </summary>
    public static void SetIcon( GLFW.Window window, string[] imagePaths, PathTypes imageFileType )
    {
        if ( Platform.IsMac )
        {
            return;
        }

        var pixmaps = new Pixmap[ imagePaths.Length ];

        for ( var i = 0; i < imagePaths.Length; i++ )
        {
            pixmaps[ i ] = new Pixmap( Gdx.Files.GetFileHandle( imagePaths[ i ], imageFileType ).File );
        }

        SetIcon( window, pixmaps );

        foreach ( var pixmap in pixmaps )
        {
            pixmap.Dispose();
        }
    }

    /// <summary>
    /// Sets the icon that will be used in the window's title bar. Has no effect in macOS,
    /// which doesn't use window icons.
    /// </summary>
    /// <param name="window"> The applicable window. </param>
    /// <param name="images">
    /// One or more images. The one closest to the system's desired size will be scaled.
    /// Good sizes include 16x16, 32x32 and 48x48. Pixmap format <see cref="Pixmap.ColorFormat.RGBA8888"/>
    /// is preferred so the images will not have to be copied and converted. <b>The chosen image
    /// is copied, and the provided Pixmaps are not disposed.</b>
    /// </param>
    public static void SetIcon( GLFW.Window window, Pixmap[] images )
    {
        Logger.Checkpoint();

        if ( Platform.IsMac )
        {
            return;
        }

        List< GLFW.Image > buffer = new( images.Length );

        Pixmap?[] tmpPixmaps = new Pixmap[ images.Length ];

        for ( var i = 0; i < images.Length; i++ )
        {
            if ( images[ i ].Format != Pixmap.ColorFormat.RGBA8888 )
            {
                var rgba = new Pixmap( images[ i ].Width, images[ i ].Height, Pixmap.ColorFormat.RGBA8888 );

                rgba.Blending = Pixmap.BlendTypes.None;
                rgba.DrawPixmap( images[ i ], 0, 0 );

                tmpPixmaps[ i ] = rgba;
            }

            GLFW.Image icon = new()
            {
                Width  = images[ i ].Width,
                Height = images[ i ].Height,
                Pixels = images[ i ].PixelData,
            };

            buffer.Add( icon );
        }

        Glfw.SetWindowIcon( window, buffer.ToArray() );

        foreach ( var pixmap in tmpPixmaps )
        {
            pixmap?.Dispose();
        }
    }

    /// <summary>
    /// Initialises the <see cref="IApplicationListener"/>.
    /// </summary>
    private void InitialiseListener()
    {
        if ( !ListenerInitialised )
        {
            Listener.Create();
            Listener.Resize( Graphics.Width, Graphics.Height );
            ListenerInitialised = true;
        }
    }

    // ========================================================================
    // ========================================================================

    #region dispose pattern

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose( true );
    }

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            Listener.Pause();
            Listener.Dispose();
            DesktopGLCursor.Dispose( this );
            Graphics.Dispose();
            Input.Dispose();

            Glfw.SetWindowFocusCallback( GlfwWindow, null );
            Glfw.SetWindowIconifyCallback( GlfwWindow, null );
            Glfw.SetWindowCloseCallback( GlfwWindow, null );
            Glfw.SetDropCallback( GlfwWindow, null );

            Glfw.DestroyWindow( GlfwWindow );
        }
    }

    #endregion dispose pattern
}