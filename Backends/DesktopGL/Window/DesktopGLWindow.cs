// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.Backends.DesktopGL.Graphics;
using LughSharp.Backends.DesktopGL.Input;
using LughSharp.Backends.DesktopGL.Utils;
using LughSharp.LibCore.Utils.Collections.Extensions;
using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.Backends.DesktopGL.Window;

[PublicAPI]
public class DesktopGLWindow : IDisposable
{
    // ------------------------------------------------------------------------

    private readonly IDesktopGLApplicationBase  _application;
    private readonly List< IRunnable.Runnable > _executedRunnables = new();
    private readonly bool                       _iconified         = false;
    private readonly List< IRunnable.Runnable > _runnables         = new();
    private readonly Vector2                    _tmpV2             = new();

    private bool _requestRendering = false;

    // ------------------------------------------------------------------------

    public DesktopGLWindow( IApplicationListener listener,
                            DesktopGLApplicationConfiguration config,
                            IDesktopGLApplicationBase application )
    {
        Listener       = listener;
        WindowListener = config.WindowListener;
        Config         = config;
        _application   = application;
    }

    public GLFWWindow?                       GlfwWindow          { get; set; }
    public IDesktopGLWindowListener?         WindowListener      { get; set; }
    public IApplicationListener              Listener            { get; set; }
    public IDesktopGLInput                   Input               { get; set; } = null!;
    public DesktopGLGraphics                 Graphics            { get; set; } = null!;
    public DesktopGLApplicationConfiguration Config              { get; set; }
    public bool                              ListenerInitialised { get; set; } = false;

    // ------------------------------------------------------------------------

    public void Dispose()
    {
        Dispose( true );
    }

    /// <summary>
    ///     Creates a new Window and sets up the various related callbacks.
    /// </summary>
    public void Create( GLFWWindow window )
    {
        GlfwWindow = window;
        Input      = _application.CreateInput( this );
        Graphics   = new DesktopGLGraphics( this );

        //@formatter:off
        Glfw.SetWindowFocusCallback     ( window, DesktopWindowCallbacks.GdxFocusCallback );
        Glfw.SetWindowIconifyCallback   ( window, DesktopWindowCallbacks.GdxIconifyCallback );
        Glfw.SetWindowMaximizeCallback  ( window, DesktopWindowCallbacks.GdxMaximizeCallback );
        Glfw.SetWindowCloseCallback     ( window, DesktopWindowCallbacks.GdxWindowCloseCallback );
        Glfw.SetDropCallback            ( window, DesktopWindowCallbacks.GdxDropCallback );
        Glfw.SetWindowRefreshCallback   ( window, DesktopWindowCallbacks.GdxRefreshCallback );
        //@formatter:on

        WindowListener?.Created( this );
    }

    /// <summary>
    ///     Post a <see cref="IRunnable.Runnable" /> to this window's event queue. Use this if
    ///     you access statics like <see cref="Gdx.Graphics" /> in your runnable instead
    ///     of <see cref="DesktopGLApplication.PostRunnable(IRunnable.Runnable)" />".
    /// </summary>
    public void PostRunnable( IRunnable.Runnable runnable )
    {
        lock ( _runnables )
        {
            _runnables.Add( runnable );
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public bool Update()
    {
        if ( !ListenerInitialised )
        {
            InitialiseListener();
        }

        lock ( _runnables )
        {
            _executedRunnables.AddAll( _runnables );
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
            Listener.Render();

            Glfw.SwapBuffers( GlfwWindow );
        }

        if ( !_iconified )
        {
            Input.PrepareNext();
        }

        return shouldRender;
    }

    public void SetTitle( string title )
    {
        Glfw.SetWindowTitle( GlfwWindow, title );
    }

    /// <summary>
    ///     Sets minimum and maximum size limits for the window. If the window
    ///     is full screen or not resizable, these limits are ignored. Use -1
    ///     to indicate an unrestricted dimension.
    /// </summary>
    public void SetSizeLimits( int minWidth, int minHeight, int maxWidth, int maxHeight )
    {
        GdxRuntimeException.ThrowIfNull( GlfwWindow );

        SetSizeLimits( GlfwWindow, minWidth, minHeight, maxWidth, maxHeight );
    }

    /// <summary>
    ///     Sets minimum and maximum size limits for the window. If the window
    ///     is full screen or not resizable, these limits are ignored. Use -1
    ///     to indicate an unrestricted dimension.
    /// </summary>
    public void SetSizeLimits( GLFWWindow windowHandle, int minWidth, int minHeight, int maxWidth, int maxHeight )
    {
        Glfw.SetWindowSizeLimits( windowHandle, minWidth, minHeight, maxWidth, maxHeight );
    }

    /// <summary>
    ///     Sets the icon that will be used in the window's title bar. Has no effect in macOS,
    ///     which doesn't use window icons.
    /// </summary>
    /// <param name="images">
    ///     One or more images. The one closest to the system's desired size will be scaled.
    ///     Good sizes include 16x16, 32x32 and 48x48. Pixmap format <see cref="Pixmap.Format.RGBA8888" />
    ///     is preferred so the images will not have to be copied and converted. The chosen image
    ///     is copied, and the provided Pixmaps are not disposed.
    /// </param>
    public void SetIcon( params Pixmap[] images )
    {
        if ( GlfwWindow != null )
        {
            SetIcon( GlfwWindow, images );
        }
    }

    public void SetIcon( GLFWWindow window, string[] imagePaths, FileType imageFileType )
    {
        if ( Platform.IsMac )
        {
            return;
        }

        var pixmaps = new Pixmap[ imagePaths.Length ];

        for ( var i = 0; i < imagePaths.Length; i++ )
        {
            pixmaps[ i ] = new Pixmap( Gdx.Files.GetFileHandle( imagePaths[ i ], imageFileType ) );
        }

        SetIcon( window, pixmaps );

        foreach ( var pixmap in pixmaps )
        {
            pixmap.Dispose();
        }
    }

    //TODO:
    private void SetIcon( GLFWWindow window, Pixmap[] images )
    {
        if ( Platform.IsMac )
        {
        }


//        GLFW.Image Buffer buffer = GLFWImage.malloc( images.length );
//
//        Pixmap?[] tmpPixmaps = new Pixmap[ images.Length ];
//
//        for ( int i = 0; i < images.Length; i++ )
//        {
//            Pixmap pixmap = images[ i ];
//
//            if ( pixmap.GetFormat() != Pixmap.Format.RGBA8888 )
//            {
//                var rgba = new Pixmap( pixmap.Width, pixmap.Height, Pixmap.Format.RGBA8888 );
//
//                rgba.Blending = Pixmap.BlendTypes.None;
//                rgba.DrawPixmap( pixmap, 0, 0 );
//
//                tmpPixmaps[ i ] = rgba;
//                pixmap          = rgba;
//            }
//
//            GLFW.Image icon = new( pixmap.Width,
//                                   pixmap.Height,
//                                   pixmap.
//                );
//
//            buffer.put( icon );
//
//            icon.free();
//        }
//
//        buffer.position( 0 );
//        Glfw.SetWindowIcon( window, buffer );
//
//        buffer.free();
//
//        foreach ( Pixmap? pixmap in tmpPixmaps )
//        {
//            if ( pixmap != null )
//            {
//                pixmap.Dispose();
//            }
//        }
    }

    private void InitialiseListener()
    {
        if ( !ListenerInitialised )
        {
            Listener.Create();
            Listener.Resize( Graphics.Width, Graphics.Height );
            ListenerInitialised = true;
        }
    }

    public void MakeCurrent()
    {
        Gdx.Graphics = Graphics;
        Gdx.GL30     = Graphics.GL30;
        Gdx.GL20     = Gdx.GL30 != null ? Gdx.GL30 : Graphics.GL20!;
        Gdx.GL       = Gdx.GL30 != null ? Gdx.GL30 : Gdx.GL20;
        Gdx.Input    = Input;

        Glfw.MakeContextCurrent( GlfwWindow );
    }

    public void RequestRendering()
    {
        lock ( this )
        {
            _requestRendering = true;
        }
    }

    public bool ShouldClose()
    {
        //TODO:
        return false;
    }

    public void SetPosition( int x, int y )
    {
        Glfw.SetWindowPos( GlfwWindow, x, y );
    }

    /// <summary>
    ///     Gets the current window position in logical coordinates. All monitors span a
    ///     virtual surface together. The coordinates are relative to the first monitor
    ///     in the virtual surface.
    /// </summary>
    /// <returns>A Vector2 holding the window X and Y.</returns>
    public Vector2 GetPosition()
    {
        Glfw.GetWindowPos( GlfwWindow, out var xPos, out var yPos );

        return _tmpV2.Set( xPos, yPos );
    }

    /// <summary>
    ///     Return the window X position in logical coordinates. All monitors span a virtual
    ///     surface together. The coordinates are relative to the first monitor in the
    ///     virtual surface.
    /// </summary>
    public int GetPositionX()
    {
        return ( int ) GetPosition().X;
    }

    /// <summary>
    ///     Return the window Y position in logical coordinates. All monitors span a virtual
    ///     surface together. The coordinates are relative to the first monitor in the
    ///     virtual surface.
    /// </summary>
    public int GetPositionY()
    {
        return ( int ) GetPosition().Y;
    }

    /// <summary>
    ///     Sets the visibility of the window.
    ///     Invisible windows will still call their <see cref="IApplicationListener" />
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

    // ------------------------------------------------------------------------

    /// <summary>
    ///     Closes this window and pauses and disposes the associated <see cref="IApplicationListener" />.
    /// </summary>
    public void CloseWindow()
    {
        Glfw.SetWindowShouldClose( GlfwWindow, true );
    }

    /// <summary>
    ///     Minimizes (iconifies) the window. Iconified windows do not call
    ///     their <see cref="IApplicationListener" /> until the window is restored.
    /// </summary>
    public void IconifyWindow()
    {
        Glfw.IconifyWindow( GlfwWindow );
    }

    /// <summary>
    ///     De-minimizes (de-iconifies) and de-maximizes the window.
    /// </summary>
    public void RestoreWindow()
    {
        Glfw.RestoreWindow( GlfwWindow );
    }

    /// <summary>
    ///     Maximizes the window.
    /// </summary>
    public void MaximizeWindow()
    {
        Glfw.MaximizeWindow( GlfwWindow );
    }

    /// <summary>
    ///     Brings the window to front and sets input focus. The window should
    ///     already be visible and not iconified.
    /// </summary>
    public void FocusWindow()
    {
        Glfw.FocusWindow( GlfwWindow );
    }

    public void Dispose( bool disposing )
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
}