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

using LibGDXSharp.Scenes.Scene2D.UI;

using Monitor = LibGDXSharp.Core.IGraphics.Monitor;

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class GLGraphics : AbstractGraphics, IDisposable
{
    public GLWindow?              Window       { get; set; }
    public IGraphics.BufferFormat BufferFormat { get; set; } = null!;

    private volatile bool _isContinuous = true;

    private long                  _lastFrameTime = -1;
    private long                  _frameId;
    private long                  _frameCounterStart = 0;
    private int                   _frames;
    private int                   _fps;
    private int                   _windowPosXBeforeFullscreen;
    private int                   _windowPosYBeforeFullscreen;
    private int                   _windowWidthBeforeFullscreen;
    private int                   _windowHeightBeforeFullscreen;
    private IGraphics.DisplayMode _displayModeBeforeFullscreen = null!;

    // ------------------------------------------------------------------------

    public unsafe void ResizeCallback( Window* windowHandle, int width, int height )
    {
        UpdateFramebufferInfo();

        if ( !Window!.ListenerInitialised )
        {
            return;
        }

        Window.MakeCurrent();

        Gdx.GL20.GLViewport( 0, 0, width, height );

        Window.Listener.Resize( Width, Height );
        Window.Listener.Render();

        GLFW.SwapBuffers( windowHandle );
    }

    // ------------------------------------------------------------------------

    public GLGraphics( GLWindow window )
    {
        this.Window = window;

        if ( window.Config.UseGL30 )
        {
            this.GL30 = new GL30();
            this.GL20 = this.GL30;
        }
        else
        {
            this.GL20 = new GL20();
            this.GL30 = null;
        }

        UpdateFramebufferInfo();
        InitiateGL();

        unsafe
        {
            GLFW.SetFramebufferSizeCallback( window.WindowHandle, ResizeCallback );
        }
    }

    private unsafe void UpdateFramebufferInfo()
    {
        if ( Window == null )
        {
            return;
        }

        GLFW.GetFramebufferSize( Window.WindowHandle, out var tmpWidth, out var tmpHeight );

        this.BackBufferWidth  = tmpWidth;
        this.BackBufferHeight = tmpHeight;

        GLFW.GetWindowSize( Window.WindowHandle, out tmpWidth, out tmpHeight );

        this.LogicalWidth  = tmpWidth;
        this.LogicalHeight = tmpHeight;

        BufferFormat = new IGraphics.BufferFormat()
        {
            R                = Window.Config.R,
            G                = Window.Config.G,
            B                = Window.Config.B,
            A                = Window.Config.A,
            Depth            = Window.Config.Depth,
            Stencil          = Window.Config.Stencil,
            Samples          = Window.Config.Samples,
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
        var vendorString   = Gdx.GL20.GLGetString( GL11.GL_VENDOR );
        var rendererString = string.Empty; //TODO: Gdx.GL20.GLGetString( GL11.GL_RENDERER );

        GLVersion = new GLVersion
            (
            IApplication.ApplicationType.Desktop,
            GLFW.GetVersionString(),
            vendorString,
            rendererString
            );

        if ( SupportsCubeMapSeamless() )
        {
            EnableCubeMapSeamless( true );
        }
    }

    /// <summary>
    /// Returns whether cubemap seamless feature is supported.
    /// </summary>
    public bool SupportsCubeMapSeamless()
    {
        return GLVersion.IsVersionEqualToOrHigher( 3, 2 )
            || SupportsExtension( "GL_ARB_seamless_cube_map" );
    }

    /// <summary>
    /// Enable or disable cubemap seamless feature. Default is true if supported.
    /// Should only be called if this feature is supported.
    /// <seealso cref="SupportsCubeMapSeamless()"/>
    /// </summary>
    /// <param name="enable"></param>
    public void EnableCubeMapSeamless( bool enable )
    {
        if ( enable )
        {
            Gdx.GL20.GLEnable( GL32.GL_TEXTURE_CUBE_MAP_SEAMLESS );
        }
        else
        {
            Gdx.GL20.GLDisable( GL32.GL_TEXTURE_CUBE_MAP_SEAMLESS );
        }
    }

    // ------------------------------------------------------------------------

    public override bool SupportsDisplayModeChange() => true;

    // ------------------------------------------------------------------------

    public override Monitor GetPrimaryMonitor()
    {
        return GLApplicationConfiguration.ToGLMonitor( GLFW.glfwGetPrimaryMonitor() );
    }

    public override Monitor GetMonitor()
    {
    }

    public override Monitor[] GetMonitors()
    {
    }

    public override IGraphics.DisplayMode[] GetDisplayModes()
    {
    }

    public override IGraphics.DisplayMode[] GetDisplayModes( Monitor monitor )
    {
    }

    public override IGraphics.DisplayMode GetDisplayMode()
    {
    }

    public override IGraphics.DisplayMode GetDisplayMode( Monitor monitor )
    {
    }

    public override bool SetFullscreenMode( IGraphics.DisplayMode displayMode )
    {
        return false;
    }

    public override bool SetWindowedMode( int width, int height )
    {
        return false;
    }

    // ------------------------------------------------------------------------

    public override void SetTitle( string title )
    {
    }

    public override void SetUndecorated( bool undecorated )
    {
    }

    public override void SetResizable( bool resizable )
    {
    }

    public override void SetVSync( bool vsync )
    {
    }

    public override void SetForegroundFps( int fps )
    {
    }

    public override bool SupportsExtension( string extension )
    {
    }

    public override void SetContinuousRendering( bool isContinuous )
    {
    }

    public override bool IsContinuousRendering()
    {
    }

    public override void RequestRendering()
    {
    }

    /// <summary>Whether the app is full screen or not.</summary>
    public override bool IsFullscreen()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="pixmap"></param>
    /// <param name="xHotspot"></param>
    /// <param name="yHotspot"></param>
    /// <returns></returns>
    public override ICursor NewCursor( Pixmap pixmap, int xHotspot, int yHotspot )
    {
    }

    /// <summary>
    /// Only viable on the lwjgl-backend and on the gwt-backend.
    /// Browsers that support cursor:url() and support the png format (the pixmap
    /// is converted to a data-url of type image/png) should also support custom
    /// cursors. Will set the mouse cursor image to the image represented by the
    /// Cursor. It is recommended to call this function in the main render thread,
    /// and maximum one time per frame.
    /// </summary>
    /// <param name="cursor">The mouse cursor as a <see cref="T:LibGDXSharp.Graphics.ICursor" /></param>
    public override void SetCursor( ICursor cursor )
    {
    }

    /// <summary>
    /// Sets one of the predefined <see cref="T:LibGDXSharp.Graphics.ICursor.SystemCursor" />s.
    /// </summary>
    /// <param name="systemCursor">The system cursor to use.</param>
    public override void SetSystemCursor( ICursor.SystemCursor systemCursor )
    {
        GLCursor.SetSystemCursor( Window.WindowHandle, systemCursor );
    }

    /// <summary>
    /// Returns whether OpenGL ES 3.0 is available.
    /// If it is you can get an instance of GL30 via GetGL30() to access
    /// OpenGL ES 3.0 functionality. Note that this functionality will
    /// only be available if you instructed the Application instance
    /// to use OpenGL ES 3.0!
    /// </summary>
    /// <returns>TRUE if available.</returns>
    public override bool IsGL30Available()
    {
        return GL30 != null;
    }

    public new int Width
    {
        get
        {
            if ( Window?.Config.HdpiMode == HdpiMode.Pixels )
            {
                return BackBufferWidth;
            }
            else
            {
                return LogicalWidth;
            }
        }
    }

    public new int Height
    {
        get
        {
            if ( Window?.Config.HdpiMode == HdpiMode.Pixels )
            {
                return BackBufferHeight;
            }
            else
            {
                return LogicalHeight;
            }
        }
    }

    // ------------------------------------------------------------------------

    public override int GetSafeInsetLeft() => 0;

    public override int GetSafeInsetTop() => 0;

    public override int GetSafeInsetBottom() => 0;

    public override int GetSafeInsetRight() => 0;

    public override long GetFrameId() => 0;

    public override int GetFramesPerSecond() => 0;

    public override GraphicsType GetGraphicsType() => GraphicsType.GL2;

    // ------------------------------------------------------------------------

    public override float GetPpiX() => GetPpcX() * 2.54f;

    public override float GetPpiY() => GetPpcY() * 2.54f;

    public override unsafe float GetPpcX()
    {
        GLFW.glfwGetMonitorPhysicalSize( GetMonitor(), out var tmp1, out var sizeX );

        return ( GetDisplayMode().Width / ( float )sizeX ) * 10;
    }

    public override unsafe float GetPpcY()
    {
        GLFW.glfwGetMonitorPhysicalSize( GetMonitor(), out var tmp1, out var sizeY );

        return ( GetDisplayMode().Height / ( float )sizeY ) * 10;
    }

    // ------------------------------------------------------------------------

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class GLDisplayMode : IGraphics.DisplayMode
    {
        public long MonitorHandle { get; set; }

        public GLDisplayMode( long monitor, int width, int height, int refreshRate, int bitsPerPixel )
            : base( width, height, refreshRate, bitsPerPixel )
        {
            this.MonitorHandle = monitor;
        }
    }

    public class GLMonitor : Monitor
    {
        public long MonitorHandle { get; set; }

        public GLMonitor( long monitor, int virtualX, int virtualY, string name )
            : base( virtualX, virtualY, name )
        {
            this.MonitorHandle = monitor;
        }
    }
}
