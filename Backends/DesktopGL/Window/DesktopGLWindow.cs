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

using LibGDXSharp.Core.Files.Buffers;
using LibGDXSharp.Core.Utils.Collections;

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class DesktopGLWindow : IDisposable
{
    public IDesktopGLWindowListener?         WindowListener      { get; set; }
    public GLFW.Window                       GlfwWindow        { get; set; }
    public IApplicationListener              Listener            { get; set; }
    public IDesktopGLInput                   Input               { get; set; } = null!;
    public DesktopGLGraphics                 Graphics            { get; set; } = null!;
    public DesktopGLApplicationConfiguration Config              { get; set; }
    public bool                              ListenerInitialised { get; set; } = false;

    private          IntBuffer                 _tmpBuffer;
    private          IntBuffer                 _tmpBuffer2;
    private          bool                      _iconified         = false;
    private          bool                      _requestRendering  = false;
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
        this._tmpBuffer     = BufferUtils.NewIntBuffer( 1 );
        this._tmpBuffer2    = BufferUtils.NewIntBuffer( 1 );
    }

    /// <summary>
    /// </summary>
    /// <param name="windowHandle"></param>
    public void Create( GLFW.Window windowHandle )
    {
        this.GlfwWindow = windowHandle;
        this.Input        = _application.CreateInput( this );
        this.Graphics     = new DesktopGLGraphics( this );

        Glfw.SetWindowFocusCallback( windowHandle, FocusCallback );
        Glfw.SetWindowIconifyCallback( windowHandle, IconifyCallback );
        Glfw.SetWindowMaximizeCallback( windowHandle, MaximizeCallback );
        Glfw.SetDropCallback( windowHandle, DropCallback );
        Glfw.SetWindowRefreshCallback( windowHandle, RefreshCallback );

        if ( WindowListener != null )
        {
            WindowListener.Created( this );
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

    private void SetIcon( GLFW.Window window, String[] imagePaths, FileType imageFileType )
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

    private void SetIcon( GLFW.Window window, Pixmap[] images )
    {
        if ( SharedLibraryLoader.IsMac )
        {
            return;
        }

        GLFWImage.Buffer buffer     = GLFWImage.malloc( images.length );
        Pixmap?[]         tmpPixmaps = new Pixmap[ images.length ];

        for ( int i = 0; i < images.length; i++ )
        {
            Pixmap pixmap = images[ i ];

            if ( pixmap.getFormat() != Pixmap.Format.RGBA8888 )
            {
                Pixmap rgba = new Pixmap( pixmap.getWidth(), pixmap.getHeight(), Pixmap.Format.RGBA8888 );
                rgba.setBlending( Pixmap.Blending.None );
                rgba.drawPixmap( pixmap, 0, 0 );
                tmpPixmaps[ i ] = rgba;
                pixmap          = rgba;
            }

            GLFWImage icon = GLFWImage.malloc();
            icon.set( pixmap.getWidth(), pixmap.getHeight(), pixmap.getPixels() );
            buffer.put( icon );

            icon.free();
        }

        buffer.position( 0 );
        Glfw.SetWindowIcon( windowHandle, buffer );

        buffer.free();
        foreach ( Pixmap? pixmap in tmpPixmaps )
        {
            if ( pixmap != null )
            {
                pixmap.Dispose();
            }
        }
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
        Gdx.GL20     = Gdx.GL30 != null ? Gdx.GL30! : Graphics.GL20!;
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

    public void Dispose()
    {
        Listener.Pause();
        Listener.Dispose();
        DesktopGLCursor.Dispose( this );
        Graphics.Dispose();
        Input.Dispose();

        GLFW.SetWindowFocusCallback( GlfwWindow, null );
        GLFW.SetWindowIconifyCallback( GlfwWindow, null );
        GLFW.SetWindowCloseCallback( GlfwWindow, null );
        GLFW.SetDropCallback( GlfwWindow, null );
        GLFW.DestroyWindow( GlfwWindow );

        focusCallback.free();
        iconifyCallback.free();
        maximizeCallback.free();
        closeCallback.free();
        dropCallback.free();
        refreshCallback.free();
    }
}
