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


using LibGDXSharp.Backends.DesktopGL.Utils;
using LibGDXSharp.Backends.DesktopGL.Window;
using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Utils;
using LibGDXSharp.LibCore.Utils.Buffers;

namespace LibGDXSharp.Backends.DesktopGL.Graphics;

using BufferFormatDescriptor = IGraphics.BufferFormatDescriptor;

[PublicAPI]
public class DesktopGLGraphics : AbstractGraphics, IDisposable
{
    public DesktopGLWindow?       GLWindow               { get; set; }
    public BufferFormatDescriptor BufferFormatDescriptor { get; set; } = null!;

    // ------------------------------------------------------------------------

    private readonly IntBuffer                       _tmpBuffer                   = BufferUtils.NewIntBuffer( 1 );
    private readonly IntBuffer                       _tmpBuffer2                  = BufferUtils.NewIntBuffer( 1 );
    private          IGraphics.DisplayModeDescriptor _displayModeBeforeFullscreen = null!;
    private          int                             _fps;
    private          long                            _frameCounterStart = 0;
    private          long                            _frameId;
    private          int                             _frames;

    private long      _lastFrameTime = -1;
    private IntBuffer _tmpBuffer3    = BufferUtils.NewIntBuffer( 1 );
    private IntBuffer _tmpBuffer4    = BufferUtils.NewIntBuffer( 1 );

    private int _tmpInt  = 0;
    private int _tmpInt2 = 0;
    private int _windowHeightBeforeFullscreen;
    private int _windowPosXBeforeFullscreen;
    private int _windowPosYBeforeFullscreen;
    private int _windowWidthBeforeFullscreen;

    // ------------------------------------------------------------------------

    public DesktopGLGraphics( DesktopGLWindow glWindow )
    {
        this.GLWindow = glWindow;

        if ( glWindow.Config.UseGL30 )
        {
            GL30 = new DesktopGL30();
            GL20 = GL30;
        }
        else
        {
            GL20 = new DesktopGL20();
            GL30 = null;
        }

        UpdateFramebufferInfo();
        InitiateGL();

        Glfw.SetWindowSizeCallback( this.GLWindow.GlfwWindow, ResizeCallback );
    }

    public new int Width => this.GLWindow?.Config.HdpiMode == HdpiMode.Pixels
        ? BackBufferWidth
        : LogicalWidth;

    public new int Height => this.GLWindow?.Config.HdpiMode == HdpiMode.Pixels
        ? BackBufferHeight
        : LogicalHeight;

    // ------------------------------------------------------------------------

    public void ResizeCallback( GLFWWindow windowHandle, int width, int height )
    {
        UpdateFramebufferInfo();

        if ( !GLWindow!.ListenerInitialised )
        {
            return;
        }

        GLWindow.MakeCurrent();

        Gdx.GL20.GLViewport( 0, 0, width, height );

        GLWindow.Listener.Resize( Width, Height );
        GLWindow.Listener.Render();
    }

    private void UpdateFramebufferInfo()
    {
        if ( GLWindow == null )
        {
            return;
        }

        Glfw.GetFramebufferSize( GLWindow.GlfwWindow, out var tmpWidth, out var tmpHeight );

        BackBufferWidth  = tmpWidth;
        BackBufferHeight = tmpHeight;

        Glfw.GetWindowSize( GLWindow.GlfwWindow, out tmpWidth, out tmpHeight );

        LogicalWidth  = tmpWidth;
        LogicalHeight = tmpHeight;

        BufferFormatDescriptor = new BufferFormatDescriptor
        {
            R                = GLWindow.Config.R,
            G                = GLWindow.Config.G,
            B                = GLWindow.Config.B,
            A                = GLWindow.Config.A,
            Depth            = GLWindow.Config.Depth,
            Stencil          = GLWindow.Config.Stencil,
            Samples          = GLWindow.Config.Samples,
            CoverageSampling = false
        };
    }

    public void Update()
    {
        var time = TimeUtils.NanoTime();

        if ( _lastFrameTime == -1 )
        {
            _lastFrameTime = time;
        }

        DeltaTime      = ( time - _lastFrameTime ) / 1000000000.0f;
        _lastFrameTime = time;

        if ( ( time - _frameCounterStart ) >= 1000000000 )
        {
            _fps               = _frames;
            _frames            = 0;
            _frameCounterStart = time;
        }

        _frames++;
        _frameId++;
    }

    private void InitiateGL()
    {
        var vendorString   = Gdx.GL20.GLGetString( IGL20.GL_VENDOR );
        var rendererString = Gdx.GL20.GLGetString( IGL20.GL_RENDERER );

        GLVersion = new GLVersion( IApplication.ApplicationType.Desktop,
                                   Glfw.GetVersionString(),
                                   vendorString,
                                   rendererString );

        if ( SupportsCubeMapSeamless() )
        {
            EnableCubeMapSeamless( true );
        }
    }

    /// <summary>
    ///     Returns whether cubemap seamless feature is supported.
    /// </summary>
    public bool SupportsCubeMapSeamless() => GLVersion.IsVersionEqualToOrHigher( 3, 2 )
                                          || SupportsExtension( "GL_ARB_seamless_cube_map" );

    /// <summary>
    ///     Enable or disable cubemap seamless feature. Default is true if supported.
    ///     Should only be called if this feature is supported.
    ///     <seealso cref="SupportsCubeMapSeamless()" />
    /// </summary>
    /// <param name="enable"></param>
    public void EnableCubeMapSeamless( bool enable )
    {
        if ( SupportsCubeMapSeamless() )
        {
            if ( enable )
            {
                GL.glEnable( GL.GL_TEXTURE_CUBE_MAP_SEAMLESS );
                
                Gdx.GL20.GLEnable( GL.GL_TEXTURE_CUBE_MAP_SEAMLESS );
            }
            else
            {
                Gdx.GL20.GLDisable( GL.GL_TEXTURE_CUBE_MAP_SEAMLESS );
            }
        }
    }

    // ------------------------------------------------------------------------

    public override bool SupportsDisplayModeChange() => true;

    // ------------------------------------------------------------------------

    public override IGraphics.MonitorDescriptor GetPrimaryMonitor() => DesktopGLApplicationConfiguration.ToGLMonitor( Glfw.PrimaryMonitor );

    public override IGraphics.MonitorDescriptor GetMonitor()
    {
        IGraphics.MonitorDescriptor[] monitors = GetMonitors();
        IGraphics.MonitorDescriptor   result   = monitors[ 0 ];

        Glfw.GetWindowPosition( GLWindow!.GlfwWindow, out _tmpInt, out _tmpInt2 );

        var windowX = _tmpBuffer.Get( 0 );
        var windowY = _tmpBuffer2.Get( 0 );

        Glfw.GetWindowSize( GLWindow.GlfwWindow, out var windowWidth, out var windowHeight );

        var bestOverlap = 0;

        foreach ( IGraphics.MonitorDescriptor monitor in monitors )
        {
            IGraphics.DisplayModeDescriptor mode = GetDisplayMode( monitor );

            var overlap = Math.Max( 0,
                                    Math.Min( windowX + windowWidth, monitor.VirtualX + mode.Width )
                                  - Math.Max( windowX, monitor.VirtualX ) )
                        * Math.Max( 0,
                                    Math.Min( windowY + windowHeight, monitor.VirtualY + mode.Height )
                                  - Math.Max( windowY, monitor.VirtualY ) );

            if ( bestOverlap < overlap )
            {
                bestOverlap = overlap;
                result      = monitor;
            }
        }

        return result;
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override bool SetWindowedMode( int width, int height ) => false;

    public override void SetTitle( string title )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow, "GLWindow == null" );

        Glfw.SetWindowTitle( GLWindow.GlfwWindow, title );
    }

    public override void SetUndecorated( bool undecorated )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow, "GLWindow == null" );

        GLWindow.Config.WindowDecorated = !undecorated;

        Glfw.SetWindowAttribute( GLWindow.GlfwWindow, WindowAttribute.Decorated, undecorated );
    }

    public override void SetResizable( bool resizable )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow, "GLWindow == null" );

        GLWindow.Config.WindowResizable = resizable;

        Glfw.SetWindowAttribute( GLWindow.GlfwWindow, WindowAttribute.Resizable, resizable );
    }

    public override void SetVSync( bool vsync )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow, "GLWindow == null" );

        GLWindow.Config.VSyncEnabled = vsync;

        Glfw.SwapInterval( vsync ? 1 : 0 );
    }

    public override void SetForegroundFps( int fps )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow, "GLWindow == null" );

        GLWindow.Config.ForegroundFPS = fps;
    }

    public override bool SupportsExtension( string extension ) => Glfw.GetExtensionSupported( extension );

    public override void RequestRendering() => GLWindow?.RequestRendering();

    /// <summary>
    ///     Whether the app is full screen or not.
    ///     #
    /// </summary>
    public override bool IsFullscreen()
    {
        GdxRuntimeException.ThrowIfNull( GLWindow, "GLWindow == null" );

        return Glfw.GetWindowMonitor( GLWindow.GlfwWindow ) != Monitor.None;
    }

    /// <summary>
    /// </summary>
    /// <param name="pixmap"></param>
    /// <param name="xHotspot"></param>
    /// <param name="yHotspot"></param>
    /// <returns></returns>
    public override ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot ) => new DesktopGLCursor( GLWindow!, pixmap, xHotspot, yHotspot );

    /// <summary>
    ///     Browsers that support cursor:url() and support the png format (the pixmap
    ///     is converted to a data-url of type image/png) should also support custom
    ///     cursors. Will set the mouse cursor image to the image represented by the
    ///     Cursor. It is recommended to call this function in the main render thread,
    ///     and maximum one time per frame.
    /// </summary>
    /// <param name="cursor">
    ///     The mouse cursor as a <see cref="T:LibGDXSharp.LibCore.Graphics.ICursor" />
    /// </param>
    public override void SetCursor( ICursor cursor )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow, "GLWindow == null" );

        Glfw.SetCursor( GLWindow.GlfwWindow, ( ( DesktopGLCursor )cursor ).GLFWCursor );
    }

    /// <summary>
    ///     Sets one of the predefined <see cref="T:LibGDXSharp.LibCore.Graphics.ICursor.SystemCursor" />s.
    /// </summary>
    /// <param name="systemCursor">The system cursor to use.</param>
    public override void SetSystemCursor( ICursor.SystemCursor systemCursor )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow, "GLWindow == null" );

        DesktopGLCursor.SetSystemCursor( GLWindow.GlfwWindow, systemCursor );
    }

    /// <summary>
    ///     Returns whether OpenGL ES 3.0 is available.
    ///     If it is you can get an instance of GL30 via GetGL30() to access
    ///     OpenGL ES 3.0 functionality. Note that this functionality will
    ///     only be available if you instructed the Application instance
    ///     to use OpenGL ES 3.0!
    /// </summary>
    /// 3r
    /// <returns>TRUE if available.</returns>
    public override bool IsGL30Available() => GL30 != null;

    // ------------------------------------------------------------------------
    //TODO:

    public override int  GetSafeInsetLeft()   => 0;
    public override int  GetSafeInsetTop()    => 0;
    public override int  GetSafeInsetBottom() => 0;
    public override int  GetSafeInsetRight()  => 0;
    public override long GetFrameId()         => _frameId;
    public override int  GetFramesPerSecond() => _fps;

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override IGraphics.MonitorDescriptor[] GetMonitors()
    {
        Monitor[] glfwMonitors = Glfw.Monitors;
        var       monitors     = new IGraphics.MonitorDescriptor[ glfwMonitors.Length ];

        for ( var i = 0; i < Glfw.Monitors.Length; i++ )
        {
            monitors[ i ] = DesktopGLApplicationConfiguration.ToGLMonitor( glfwMonitors[ i ] );
        }

        return monitors;
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override IGraphics.DisplayModeDescriptor[] GetDisplayModes() => DesktopGLApplicationConfiguration.GetDisplayModes();

    /// <inheritdoc />
    public override IGraphics.DisplayModeDescriptor[] GetDisplayModes( IGraphics.MonitorDescriptor monitor )
        => DesktopGLApplicationConfiguration.GetDisplayModes( monitor.MonitorHandle );

    /// <inheritdoc />
    public override IGraphics.DisplayModeDescriptor GetDisplayMode() => DesktopGLApplicationConfiguration.GetDisplayMode( GetMonitor().MonitorHandle );

    /// <inheritdoc />
    public override IGraphics.DisplayModeDescriptor GetDisplayMode( IGraphics.MonitorDescriptor monitor )
        => DesktopGLApplicationConfiguration.GetDisplayMode( monitor.MonitorHandle );

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override bool SetFullscreenMode( IGraphics.DisplayModeDescriptor displayMode )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow );

        GLWindow.Input.ResetPollingStates();

        var newMode = ( DesktopGLDisplayMode )displayMode;

        if ( IsFullscreen() )
        {
            var currentMode = ( DesktopGLDisplayMode )GetDisplayMode();

            if ( ( currentMode.MonitorHandle == newMode.MonitorHandle )
              && ( currentMode.RefreshRate == newMode.RefreshRate ) )
            {
                // same monitor and refresh rate
                Glfw.SetWindowSize( GLWindow.GlfwWindow, newMode.Width, newMode.Height );
            }
            else
            {
                // different monitor and/or refresh rate
                Glfw.SetWindowMonitor( GLWindow.GlfwWindow,
                                       newMode.MonitorHandle,
                                       0,
                                       0,
                                       newMode.Width,
                                       newMode.Height,
                                       newMode.RefreshRate );
            }
        }
        else
        {
            // store window position so we can restore it when switching
            // from fullscreen to windowed later
            StoreCurrentWindowPositionAndDisplayMode();

            // switch from windowed to fullscreen
            Glfw.SetWindowMonitor( GLWindow.GlfwWindow,
                                   newMode.MonitorHandle,
                                   0,
                                   0,
                                   newMode.Width,
                                   newMode.Height,
                                   newMode.RefreshRate );
        }

        UpdateFramebufferInfo();

        SetVSync( GLWindow!.Config.VSyncEnabled );

        return true;
    }

    // ------------------------------------------------------------------------

    public override GLVersion.GLType GetGraphicsType() => GLVersion.GLType.GL20;

    public override float GetPpiX() => GetPpcX() * 2.54f;

    public override float GetPpiY() => GetPpcY() * 2.54f;

    public override float GetPpcX()
    {
        Glfw.GetMonitorPhysicalSize( GetMonitor().MonitorHandle, out var sizeX, out var sizeY ); //TODO:

        return ( GetDisplayMode().Width / ( float )sizeX ) * 10;
    }

    public override float GetPpcY()
    {
        Glfw.GetMonitorPhysicalSize( GetMonitor().MonitorHandle, out var sizeX, out var sizeY ); //TODO:

        return ( GetDisplayMode().Height / ( float )sizeY ) * 10;
    }

    // ------------------------------------------------------------------------

    private void StoreCurrentWindowPositionAndDisplayMode()
    {
        if ( GLWindow == null )
        {
            return;
        }

        _windowPosXBeforeFullscreen   = GLWindow.GetPositionX();
        _windowPosYBeforeFullscreen   = GLWindow.GetPositionY();
        _windowWidthBeforeFullscreen  = LogicalWidth;
        _windowHeightBeforeFullscreen = LogicalHeight;
        _displayModeBeforeFullscreen  = GetDisplayMode();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class DesktopGLDisplayMode : IGraphics.DisplayModeDescriptor
    {
        public DesktopGLDisplayMode( GLFWMonitor monitor,
                                     int width,
                                     int height,
                                     int refreshRate,
                                     int bitsPerPixel )
            : base( width, height, refreshRate, bitsPerPixel ) => MonitorHandle = monitor;

        public Monitor MonitorHandle { get; set; }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class DesktopGLMonitor : IGraphics.MonitorDescriptor
    {
        public DesktopGLMonitor( GLFWMonitor monitor,
                                 int virtualX,
                                 int virtualY,
                                 string name )
            : base( virtualX, virtualY, name ) => MonitorHandle = monitor;

        public GLFWMonitor MonitorHandle { get; private set; }
    }

    // ------------------------------------------------------------------------

    #region IDisposable implementation

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            //TODO:
        }
    }

    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    #endregion IDisposable implementation
}
