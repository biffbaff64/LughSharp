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

using Corelib.Lugh.Core;
using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.GLUtils;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Exceptions;

using DesktopGLBackend.Core;
using DesktopGLBackend.Utils;
using DesktopGLBackend.Window;

namespace DesktopGLBackend.Graphics;

[PublicAPI]
public class DesktopGLGraphics : AbstractGraphics, IDisposable
{
    public DesktopGLWindow? GLWindow { get; set; }

    // ========================================================================

    private IGraphics.DisplayMode? _displayModeBeforeFullscreen;

    private long _frameCounterStart = 0;
    private long _lastFrameTime     = -1;
    private long _frameId;
    private int  _frames;
    private int  _fps;
    private int  _windowPosXBeforeFullscreen;
    private int  _windowPosYBeforeFullscreen;
    private int  _windowWidthBeforeFullscreen;
    private int  _windowHeightBeforeFullscreen;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a new GLGraphics instance for Desktop backends, using the
    /// given <see cref="DesktopGLWindow"/> as the main window.
    /// </summary>
    public DesktopGLGraphics( DesktopGLWindow glWindow )
    {
        this.GLWindow = glWindow;

        UpdateFramebufferInfo();

        UpdateGLVersion();

        Glfw.SetWindowSizeCallback( GLWindow.GlfwWindow, ResizeCallback );
    }

    //@formatter:off
    
    /// <inheritdoc />
    public override int Width => GLWindow?.AppConfig.HdpiMode == HdpiMode.Pixels
                           ? BackBufferWidth
                           : LogicalWidth;

    /// <inheritdoc />
    public override int Height => GLWindow?.AppConfig.HdpiMode == HdpiMode.Pixels
                           ? BackBufferHeight
                           : LogicalHeight;
    
    //@formatter:on

    /// <summary>
    /// Whether the app is full screen or not.
    /// </summary>
    public override bool IsFullscreen
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( GLWindow );

            return Glfw.GetWindowMonitor( GLWindow.GlfwWindow ) != null;
        }
    }

    /// <inheritdoc />
    public override GraphicsBackend.BackendType GraphicsType => GraphicsBackend.BackendType.OpenGles; //TODO

    /// <inheritdoc />
    public override bool SupportsDisplayModeChange() => true;

    /// <summary>
    /// </summary>
    /// <param name="windowHandle"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void ResizeCallback( GLFW.Window windowHandle, int width, int height )
    {
        UpdateFramebufferInfo();

        if ( !GLWindow!.ListenerInitialised )
        {
            return;
        }

        GLWindow.MakeCurrent();

        Gdx.GL.glViewport( 0, 0, width, height );

        GLWindow.Listener.Update();
        GLWindow.Listener.Resize( Width, Height );
        GLWindow.Listener.Render();
    }

    /// <summary>
    /// </summary>
    private void UpdateFramebufferInfo()
    {
        if ( ( GLWindow == null ) || ( GLWindow.GlfwWindow == null ) )
        {
            return;
        }

        Glfw.GetFramebufferSize( GLWindow.GlfwWindow, out var tmpWidth, out var tmpHeight );
        
        BackBufferWidth = tmpWidth;
        BackBufferHeight = tmpHeight;
        
        Glfw.GetWindowSize( GLWindow.GlfwWindow, out tmpWidth, out tmpHeight );

        LogicalWidth = tmpWidth;
        LogicalHeight = tmpHeight;
        
        BufferFormat = new IGraphics.BufferFormatDescriptor
        {
            R                = GLWindow.AppConfig.Red,
            G                = GLWindow.AppConfig.Green,
            B                = GLWindow.AppConfig.Blue,
            A                = GLWindow.AppConfig.Alpha,
            Depth            = GLWindow.AppConfig.Depth,
            Stencil          = GLWindow.AppConfig.Stencil,
            Samples          = GLWindow.AppConfig.Samples,
            CoverageSampling = false,
        };
    }

    /// <summary>
    /// </summary>
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

    /// <summary>
    /// Returns whether cubemap seamless feature is supported.
    /// </summary>
    public bool SupportsCubeMapSeamless()
    {
        return /*( bool )GLVersion!.IsVersionEqualToOrHigher( 3, 2 )
               ||*/ SupportsExtension( "GL_ARB_seamless_cube_map" );
    }

    /// <summary>
    /// Enable or disable cubemap seamless feature. Default is true if supported.
    /// Should only be called if this feature is supported.
    /// </summary>
    /// <param name="enable"></param>
    public void EnableCubeMapSeamless( bool enable )
    {
        if ( SupportsCubeMapSeamless() )
        {
            if ( enable )
            {
                Gdx.GL.glEnable( IGL.GL_TEXTURE_CUBE_MAP_SEAMLESS );
            }
            else
            {
                Gdx.GL.glDisable( IGL.GL_TEXTURE_CUBE_MAP_SEAMLESS );
            }
        }
    }

    /// <inheritdoc />
    public override bool SetWindowedMode( int width, int height )
    {
        GLWindow?.Input.ResetPollingStates();

        var monitor = Glfw.GetPrimaryMonitor();

        if ( !IsFullscreen )
        {
            if ( ( width != LogicalWidth ) || ( height != LogicalHeight ) )
            {
                //Center window
                Glfw.GetMonitorWorkarea( monitor, out var x, out var y, out var w, out var h );
                Glfw.SetWindowPos( GLWindow?.GlfwWindow, x, y );

                GLWindow?.SetPosition( x + ( ( w - width ) / 2 ), y + ( ( h - height ) / 2 ) );
            }

            Glfw.SetWindowSize( GLWindow?.GlfwWindow, width, height );
        }
        else
        {
            if ( _displayModeBeforeFullscreen == null )
            {
                BackupCurrentWindow();
            }

            if ( ( width != _windowWidthBeforeFullscreen ) || ( height != _windowHeightBeforeFullscreen ) )
            {
                Glfw.GetMonitorWorkarea( monitor, out var x, out var y, out var w, out var h );

                Glfw.SetWindowMonitor( GLWindow?.GlfwWindow,
                                       monitor,
                                       x + ( ( w - width ) / 2 ),
                                       y + ( ( h - height ) / 2 ),
                                       width,
                                       height,
                                       _displayModeBeforeFullscreen!.RefreshRate );
            }
            else
            {
                Glfw.SetWindowMonitor( GLWindow?.GlfwWindow,
                                       monitor,
                                       _windowPosXBeforeFullscreen,
                                       _windowPosYBeforeFullscreen,
                                       width,
                                       height,
                                       _displayModeBeforeFullscreen!.RefreshRate );
            }
        }

        UpdateFramebufferInfo();

        return true;
    }

    /// <inheritdoc />
    public override void SetUndecorated( bool undecorated )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow );

        GLWindow.AppConfig.WindowDecorated = !undecorated;

        Glfw.SetWindowAttrib( GLWindow.GlfwWindow, WindowAttrib.Decorated, undecorated );
    }

    /// <inheritdoc />
    public override void SetResizable( bool resizable )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow );

        GLWindow.AppConfig.WindowResizable = resizable;

        Glfw.SetWindowAttrib( GLWindow.GlfwWindow, WindowAttrib.Resizable, resizable );
    }

    /// <inheritdoc />
    public override void SetVSync( bool vsync )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow );

        GLWindow.AppConfig.VSyncEnabled = vsync;

        Glfw.SwapInterval( vsync ? 1 : 0 );
    }

    /// <inheritdoc />
    public override void SetForegroundFps( int fps )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow );

        GLWindow.AppConfig.ForegroundFPS = fps;
    }

    /// <inheritdoc />
    public override bool SupportsExtension( string extension )
    {
        return Glfw.ExtensionSupported( extension );
    }

    /// <inheritdoc />
    public override void RequestRendering()
    {
        GLWindow?.RequestRendering();
    }

    /// <inheritdoc />
    public override ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot )
    {
        return new DesktopGLCursor( GLWindow!, pixmap, xHotspot, yHotspot );
    }

    /// <summary>
    /// Browsers that support cursor:url() and support the png format (the pixmap is
    /// converted to a data-url of type image/png) should also support custom cursors.
    /// Will set the mouse cursor image to the image represented by the Cursor. It is
    /// recommended to call this function in the main render thread, and maximum one
    /// time per frame.
    /// </summary>
    /// <param name="cursor">
    /// The mouse cursor as a <see cref="ICursor"/>
    /// </param>
    public override void SetCursor( ICursor cursor )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow );

        Glfw.SetCursor( GLWindow.GlfwWindow, ( ( DesktopGLCursor )cursor ).GLFWCursor );
    }

    /// <summary>
    /// Sets one of the predefined <see cref="ICursor.SystemCursor"/>s.
    /// </summary>
    /// <param name="systemCursor">The system cursor to use.</param>
    public override void SetSystemCursor( ICursor.SystemCursor systemCursor )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow?.GlfwWindow );

        DesktopGLCursor.SetSystemCursor( GLWindow.GlfwWindow, systemCursor );
    }

    // ========================================================================

    /// <inheritdoc />
    public override IGraphics.DisplayMode[] GetDisplayModes()
    {
        return DesktopGLApplicationConfiguration.GetDisplayModes( Glfw.GetPrimaryMonitor() );
    }

    /// <inheritdoc />
    public override IGraphics.DisplayMode[] GetDisplayModes( GLFW.Monitor monitor )
    {
        return DesktopGLApplicationConfiguration.GetDisplayModes( monitor );
    }

    /// <inheritdoc />
    public override IGraphics.DisplayMode GetDisplayMode()
    {
        return DesktopGLApplicationConfiguration.GetDisplayMode( Glfw.GetPrimaryMonitor() );
    }

    /// <inheritdoc />
    public override IGraphics.DisplayMode GetDisplayMode( GLFW.Monitor monitor )
    {
        return DesktopGLApplicationConfiguration.GetDisplayMode( monitor );
    }

    /// <inheritdoc />
    public override bool SetFullscreenMode( IGraphics.DisplayMode displayMode )
    {
        GdxRuntimeException.ThrowIfNull( GLWindow );

        GLWindow.Input.ResetPollingStates();

        var newMode = ( DesktopGLDisplayMode )displayMode;

        if ( IsFullscreen )
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
            BackupCurrentWindow();

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

        SetVSync( GLWindow!.AppConfig.VSyncEnabled );

        return true;
    }

    /// <inheritdoc />
    public override (float X, float Y) GetPpiXY()
    {
        return ( GetPpcXY().X * 2.54f, GetPpcXY().Y * 2.54f );
    }

    /// <inheritdoc />
    public override (float X, float Y) GetPpcXY()
    {
        Glfw.GetMonitorPhysicalSize( Glfw.GetPrimaryMonitor(), out var sizeX, out var sizeY );

        return ( ( GetDisplayMode().Width / ( float )sizeX ) * 10,
            ( GetDisplayMode().Height / ( float )sizeY ) * 10 );
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Makes a backup of the current windows position and display mode.
    /// </summary>
    private void BackupCurrentWindow()
    {
        if ( GLWindow == null )
        {
            return;
        }

        _windowPosXBeforeFullscreen   = GLWindow.PositionX;
        _windowPosYBeforeFullscreen   = GLWindow.PositionY;
        _windowWidthBeforeFullscreen  = LogicalWidth;
        _windowHeightBeforeFullscreen = LogicalHeight;
        _displayModeBeforeFullscreen  = GetDisplayMode();
    }

    private void UpdateGLVersion()
    {
//        var vendorString   = Gdx.GL.glGetString( IGL.GL_VENDOR );
//        var rendererString = Gdx.GL.glGetString( IGL.GL_RENDERER );

//        GLVersion = new GLVersion( Platform.ApplicationType.WindowsGL,
//                                   vendorString,
//                                   rendererString );

//TODO:
//        EnableCubeMapSeamless( true );
    }

    // ========================================================================

    /// <inheritdoc />
    public override int GetSafeInsetLeft() => 0;

    /// <inheritdoc />
    public override int GetSafeInsetTop() => 0;

    /// <inheritdoc />
    public override int GetSafeInsetBottom() => 0;

    /// <inheritdoc />
    public override int GetSafeInsetRight() => 0;

    /// <inheritdoc />
    public override long GetFrameID() => _frameId;

    /// <inheritdoc />
    public override int GetFramesPerSecond() => _fps;

    // ========================================================================

    /// <summary>
    /// Describes a Display Mode for a <see cref="GLFW.Monitor"/>
    /// </summary>
    [PublicAPI]
    public class DesktopGLDisplayMode : IGraphics.DisplayMode
    {
        /// <summary>
        /// The <see cref="GLFW.Monitor"/> this <see cref="IGraphics.DisplayMode"/> applies to.
        /// </summary>
        public GLFW.Monitor MonitorHandle { get; set; }

        /// <summary>
        /// Creates a new Display Mode and its properties.
        /// </summary>
        /// <param name="monitor"> The target monitor. </param>
        /// <param name="width"> Monitor display width. </param>
        /// <param name="height"> Monior display height. </param>
        /// <param name="refreshRate"> The refresh rate. </param>
        /// <param name="bitsPerPixel"> The bits per pixel. </param>
        public DesktopGLDisplayMode( GLFW.Monitor monitor, int width, int height, int refreshRate, int bitsPerPixel )
            : base( width, height, refreshRate, bitsPerPixel )
        {
            MonitorHandle = monitor;
        }
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Wrapper for a <see cref="GLFW.Monitor"/> which adds virtual X & Y, plus a name.
    /// Virtual positions are for multiple monitors.
    /// </summary>
    [PublicAPI]
    public class DesktopGLMonitor : IGraphics.GdxMonitor
    {
        /// <summary>
        /// The <see cref="GLFW.Monitor"/>.
        /// </summary>
        public GLFW.Monitor MonitorHandle { get; private set; }

        public DesktopGLMonitor( GLFW.Monitor monitor, int virtualX, int virtualY, string name )
            : base( virtualX, virtualY, name )
        {
            MonitorHandle = monitor;
        }
    }

    // ========================================================================
    // ========================================================================

    #region IDisposable implementation

    protected static void Dispose( bool disposing )
    {
        if ( disposing )
        {
            //TODO:
        }
    }

    public void Dispose()
    {
        Dispose( true );
    }

    #endregion IDisposable implementation
}