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
    public        IDesktopGLWindowListener?         WindowListener      { get; set; }
    public unsafe Window*                           WindowHandle        { get; set; }
    public        IApplicationListener              Listener            { get; set; }
    public        IDesktopGLInput                   Input               { get; set; } = null!;
    public        DesktopGLGraphics                 Graphics            { get; set; } = null!;
    public        DesktopGLApplicationConfiguration Config              { get; set; }
    public        bool                              ListenerInitialised { get; set; } = false;

    private readonly IDesktopGLApplicationBase _application;
    private          IntBuffer                 _tmpBuffer;
    private          IntBuffer                 _tmpBuffer2;
    private          bool                      _iconified         = false;
    private          bool                      _requestRendering  = false;
    private readonly List< Runnable >          _runnables         = new();
    private readonly List< Runnable >          _executedRunnables = new();

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
    public unsafe void Create( Window* windowHandle )
    {
        this.WindowHandle = windowHandle;
        this.Input        = _application.CreateInput( this );
        this.Graphics     = new DesktopGLGraphics( this );

        GLFW.SetWindowFocusCallback( windowHandle, FocusCallback );
        GLFW.SetWindowIconifyCallback( windowHandle, IconifyCallback );
        GLFW.SetWindowMaximizeCallback( windowHandle, MaximizeCallback );
        GLFW.SetWindowCloseCallback( windowHandle, CloseCallback );
        GLFW.SetDropCallback( windowHandle, DropCallback );
        GLFW.SetWindowRefreshCallback( windowHandle, RefreshCallback );

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
                          || ( Graphics.IsContinuousRendering() ) );

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
            unsafe
            {
                Graphics.Update();
                Listener.Render();

                GLFW.SwapBuffers( WindowHandle );
            }
        }

        if ( !_iconified )
        {
            Input.PrepareNext();
        }

        return shouldRender;
    }

    private unsafe void WindowHandleChanged( Window* windowHandle )
    {
        this.WindowHandle = windowHandle;

        this.Input.WindowHandleChanged( windowHandle );
    }

    public unsafe void SetTitle( string title )
    {
        GLFW.SetWindowTitle( WindowHandle, title );
    }

    /// <summary>
    /// Sets minimum and maximum size limits for the window. If the window
    /// is full screen or not resizable, these limits are ignored. Use -1
    /// to indicate an unrestricted dimension.
    /// </summary>
    public unsafe void SetSizeLimits( int minWidth, int minHeight, int maxWidth, int maxHeight )
    {
        SetSizeLimits( WindowHandle, minWidth, minHeight, maxWidth, maxHeight );
    }

    private unsafe void SetSizeLimits( Window* windowHandle, int minWidth, int minHeight, int maxWidth, int maxHeight )
    {
        GLFW.SetWindowSizeLimits( windowHandle,
                                  minWidth > -1 ? minWidth : GLFW.DontCare,
                                  minHeight > -1 ? minHeight : GLFW.DontCare,
                                  maxWidth > -1 ? maxWidth : GLFW.DontCare,
                                  maxHeight > -1 ? maxHeight : GLFW.DontCare );
    }

    /// <summary>
    /// Sets the icon that will be used in the window's title bar. Has no effect in
    /// macOS, which doesn't use window icons.
    /// </summary>
    /// <param name="images"> One or more images. The one closest to the system's
    /// desired size will be scaled. Good sizes include 16x16, 32x32 and 48x48.
    /// Pixmap format <see cref="Pixmap.Format.RGBA8888"/>" is preferred so the
    /// images will not have to be copied and converted. The chosen image is copied,
    /// and the provided Pixmaps are not disposed.
    /// </param>
    public unsafe void SetIcon( params Pixmap[] images )
    {
        SetIcon( WindowHandle, images );
    }

    private void SetIcon( long windowHandle, String[] imagePaths, FileType imageFileType )
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

        unsafe
        {
            SetIcon( WindowHandle, pixmaps );
        }

        foreach ( Pixmap pixmap in pixmaps )
        {
            pixmap.Dispose();
        }
    }

    private unsafe void SetIcon( Window* windowHandle, Pixmap[] images )
    {
        if ( SharedLibraryLoader.IsMac )
        {
            return;
        }

        GLFWImage.Buffer buffer     = GLFWImage.malloc( images.Length );
        Pixmap?[]        tmpPixmaps = new Pixmap[ images.Length ];

        for ( int i = 0; i < images.Length; i++ )
        {
            Pixmap pixmap = images[ i ];

            if ( pixmap.GetFormat() != Pixmap.Format.RGBA8888 )
            {
                var rgba = new Pixmap( pixmap.Width, pixmap.Height, Pixmap.Format.RGBA8888 );

                rgba.Blending = Pixmap.BlendTypes.None;
                rgba.DrawPixmap( pixmap, 0, 0 );

                tmpPixmaps[ i ] = rgba;
                pixmap          = rgba;
            }

            GLFWImage icon = GLFWImage.malloc();

            icon.set( pixmap.Width, pixmap.Height, pixmap.Pixels );
            buffer.put( icon );

            icon.free();
        }

        buffer.position( 0 );

        GLFW.SetWindowIcon( windowHandle, buffer );

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

    public unsafe void MakeCurrent()
    {
        Gdx.Graphics = this.Graphics;
        Gdx.GL30     = this.Graphics.GL30;
        Gdx.GL20     = Gdx.GL30 != null ? Gdx.GL30! : Graphics.GL20!;
        Gdx.GL       = Gdx.GL30 != null ? Gdx.GL30 : Gdx.GL20;
        Gdx.Input    = this.Input;

        GLFW.MakeContextCurrent( WindowHandle );
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

        unsafe
        {
            GLFW.SetWindowFocusCallback( WindowHandle, null );
            GLFW.SetWindowIconifyCallback( WindowHandle, null );
            GLFW.SetWindowCloseCallback( WindowHandle, null );
            GLFW.SetDropCallback( WindowHandle, null );
            GLFW.DestroyWindow( WindowHandle );
        }

        focusCallback.free();
        iconifyCallback.free();
        maximizeCallback.free();
        closeCallback.free();
        dropCallback.free();
        refreshCallback.free();
    }
}
