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

using LibGDXSharp.Backends.DesktopGL;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class DesktopGLWindow : IDisposable
{
    public IDesktopGLWindowListener?         WindowListener      { get; set; }
    public GLFW.Window                       GlfwWindow          { get; set; }
    public IApplicationListener              Listener            { get; set; }
    public IDesktopGLInput                   Input               { get; set; } = null!;
    public DesktopGLGraphics                 Graphics            { get; set; } = null!;
    public DesktopGLApplicationConfiguration Config              { get; set; }
    public bool                              ListenerInitialised { get; set; } = false;

    private          bool                      _iconified         = false;
    private          bool                      _requestRendering  = false;
    private readonly Vector2                   _tmpV2             = new();
    private readonly List< Runnable >          _runnables         = new();
    private readonly List< Runnable >          _executedRunnables = new();
    private readonly IDesktopGLApplicationBase _application;

    // ------------------------------------------------------------------------

    public DesktopGLWindow( IApplicationListener listener,
                            DesktopGLApplicationConfiguration config,
                            IDesktopGLApplicationBase application )
    {
        this.Listener       = listener;
        this.WindowListener = config.WindowListener;
        this.Config         = config;
        this._application   = application;
    }

    /// <summary>
    /// Creates a new Window and sets up the various related callbacks.
    /// </summary>
    public void Create( GLFW.Window window )
    {
        this.GlfwWindow = window;
        this.Input      = _application.CreateInput( this );
        this.Graphics   = new DesktopGLGraphics( this );

        //@formatter:off
        Glfw.SetWindowFocusCallback     ( window, DesktopWindowCallbacks.GdxFocusCallback );
        Glfw.SetWindowIconifyCallback   ( window, DesktopWindowCallbacks.GdxIconifyCallback );
        Glfw.SetWindowMaximizeCallback  ( window, DesktopWindowCallbacks.GdxMaximizeCallback );
        Glfw.SetCloseCallback           ( window, DesktopWindowCallbacks.GdxWindowCloseCallback );
        Glfw.SetDropCallback            ( window, DesktopWindowCallbacks.GdxDropCallback );
        Glfw.SetWindowRefreshCallback   ( window, DesktopWindowCallbacks.GdxRefreshCallback );
        //@formatter:on

        if ( WindowListener != null )
        {
            WindowListener.Created( this );
        }
    }

    /// <summary>
    /// Post a <see cref="Runnable"/> to this window's event queue. Use this if
    /// you access statics like <see cref="Gdx.Graphics"/> in your runnable instead
    /// of <see cref="DesktopGLApplication.PostRunnable(Runnable)"/>".
    /// </summary>
    public void PostRunnable( Runnable runnable )
    {
        lock ( _runnables )
        {
            _runnables.Add( runnable );
        }
    }

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

        foreach ( Runnable runnable in _executedRunnables )
        {
            runnable();
        }

        var shouldRender = ( ( _executedRunnables.Count > 0 )
                          || ( Graphics.ContinuousRendering ) );

        _executedRunnables.Clear();

        if ( !_iconified )
        {
            Input.Update();
        }

        lock ( this )
        {
            shouldRender      |= ( _requestRendering && !_iconified );
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

    private void WindowHandleChanged( GLFW.Window windowHandle )
    {
        this.GlfwWindow = windowHandle;

        this.Input.WindowHandleChanged( windowHandle );
    }

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
        SetSizeLimits( GlfwWindow, minWidth, minHeight, maxWidth, maxHeight );
    }

    /// <summary>
    /// Sets minimum and maximum size limits for the window. If the window
    /// is full screen or not resizable, these limits are ignored. Use -1
    /// to indicate an unrestricted dimension.
    /// </summary>
    public void SetSizeLimits( GLFW.Window windowHandle, int minWidth, int minHeight, int maxWidth, int maxHeight )
    {
        Glfw.SetWindowSizeLimits( windowHandle,
                                  minWidth > -1 ? minWidth : GL.GL_DONT_CARE,
                                  minHeight > -1 ? minHeight : GL.GL_DONT_CARE,
                                  maxWidth > -1 ? maxWidth : GL.GL_DONT_CARE,
                                  maxHeight > -1 ? maxHeight : GL.GL_DONT_CARE );
    }

    /// <summary>
    /// Sets the icon that will be used in the window's title bar. Has no effect in macOS,
    /// which doesn't use window icons.
    /// </summary>
    /// <param name="images">
    /// One or more images. The one closest to the system's desired size will be scaled.
    /// Good sizes include 16x16, 32x32 and 48x48. Pixmap format <see cref="Pixmap.Format.RGBA8888"/>
    /// is preferred so the images will not have to be copied and converted. The chosen image
    /// is copied, and the provided Pixmaps are not disposed.
    /// </param>
    public void SetIcon( params Pixmap[] images )
    {
        SetIcon( GlfwWindow, images );
    }

    public void SetIcon( GLFW.Window window, String[] imagePaths, FileType imageFileType )
    {
        if ( SharedLibraryLoader.IsMac )
        {
            return;
        }

        var pixmaps = new Pixmap[ imagePaths.Length ];

        for ( var i = 0; i < imagePaths.Length; i++ )
        {
            pixmaps[ i ] = new Pixmap( Gdx.Files.GetFileHandle( imagePaths[ i ], imageFileType ) );
        }

        SetIcon( window, pixmaps );

        foreach ( Pixmap pixmap in pixmaps )
        {
            pixmap.Dispose();
        }
    }

    //TODO:
    private void SetIcon( GLFW.Window window, Pixmap[] images )
    {
        if ( SharedLibraryLoader.IsMac )
        {
            return;
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
        Gdx.Graphics = this.Graphics;
        Gdx.GL30     = this.Graphics.GL30;
        Gdx.GL20     = Gdx.GL30 != null ? Gdx.GL30 : Graphics.GL20!;
        Gdx.GL       = Gdx.GL30 != null ? Gdx.GL30 : Gdx.GL20;
        Gdx.Input    = this.Input;

        Glfw.MakeContextCurrent( GlfwWindow );
    }

    public void RequestRendering()
    {
        lock ( this )
        {
            this._requestRendering = true;
        }
    }

    public bool ShouldClose() => false;

    public void SetPosition( int x, int y )
    {
        Glfw.SetWindowPosition( GlfwWindow, x, y );
    }

    /// <summary>
    /// Gets the current window position in logical coordinates. All monitors span a
    /// virtual surface together. The coordinates are relative to the first monitor
    /// in the virtual surface.
    /// </summary>
    /// <returns>A Vector2 holding the window X and Y.</returns>
    public Vector2 GetPosition()
    {
        Glfw.GetWindowPosition( GlfwWindow, out var xPos, out var yPos );

        return _tmpV2.Set( xPos, yPos );
    }

    /// <summary>
    /// Return the window X position in logical coordinates. All monitors span a virtual
    /// surface together. The coordinates are relative to the first monitor in the
    /// virtual surface.
    /// </summary>
    public int GetPositionX() => ( int )GetPosition().X;

    /// <summary>
    /// Return the window Y position in logical coordinates. All monitors span a virtual
    /// surface together. The coordinates are relative to the first monitor in the
    /// virtual surface.
    /// </summary>
    public int GetPositionY() => ( int )GetPosition().Y;

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

    /// <summary>
    /// Closes this window and pauses and disposes the associated <see cref="IApplicationListener"/>.
    /// </summary>
    public void CloseWindow()
    {
        Glfw.SetWindowShouldClose( GlfwWindow, true );
    }

    /// <summary>
    /// Minimizes (iconifies) the window. Iconified windows do not call
    /// their <see cref="IApplicationListener"/> until the window is restored.
    /// </summary>
    public void IconifyWindow()
    {
        Glfw.IconifyWindow( GlfwWindow );
    }

    /// <summary>
    /// De-minimizes (de-iconifies) and de-maximizes the window.
    /// </summary>
    public void RestoreWindow()
    {
        Glfw.RestoreWindow( GlfwWindow );
    }

    /// <summary>
    /// Maximizes the window.
    /// </summary>
    public void MaximizeWindow()
    {
        Glfw.MaximizeWindow( GlfwWindow );
    }

    /// <summary>
    /// Brings the window to front and sets input focus. The window should
    /// already be visible and not iconified.
    /// </summary>
    public void FocusWindow()
    {
        Glfw.FocusWindow( GlfwWindow );
    }

    public void Dispose()
    {
        Listener.Pause();
        Listener.Dispose();
        DesktopGLCursor.Dispose( this );
        Graphics.Dispose();
        Input.Dispose();

        Glfw.SetWindowFocusCallback( GlfwWindow, null );
        Glfw.SetWindowIconifyCallback( GlfwWindow, null );
        Glfw.SetCloseCallback( GlfwWindow, null );
        Glfw.SetDropCallback( GlfwWindow, null );
        Glfw.DestroyWindow( GlfwWindow );
    }
}
