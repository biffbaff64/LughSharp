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

using LibGDXSharp.Backends.Desktop;
using LibGDXSharp.Backends.Desktop.Audio;
using LibGDXSharp.Backends.Desktop.Audio.Mock;
using LibGDXSharp.Core.Files.Buffers;
using LibGDXSharp.Core.Utils.Collections;

using Sync = LibGDXSharp.Backends.Desktop.Sync;

namespace LibGDXSharp;

[PublicAPI]
public class DesktopGLApplication : IDesktopGLApplicationBase
{

    #region public properties

    public DesktopGLApplicationConfiguration?  Config             { get; set; }
    public List< DesktopGLWindow >             Windows            { get; set; } = new();
    public Dictionary< string, IPreferences >? Preferences        { get; set; }
    public List< Runnable >                    Runnables          { get; set; } = new();
    public List< Runnable >                    ExecutedRunnables  { get; set; } = new();
    public List< ILifecycleListener >          LifecycleListeners { get; set; } = new();
    public DesktopGLApplicationLogger?                ApplicationLogger  { get; set; }
    public int                                 LogLevel           { get; set; }
    public IClipboard?                         Clipboard          { get; set; }

    public static GLVersion? GLVersion { get; set; }

    public IApplication.ApplicationType AppType
    {
        get => IApplication.ApplicationType.Desktop;
        set { }
    }

    public IGraphics?            Graphics            => _currentWindow?.Graphics;
    public IApplicationListener? ApplicationListener => _currentWindow?.Listener;
    public IInput?               Input               => _currentWindow?.Input;
    public IGLAudio?             Audio               { get; set; } = null;
    public INet                  Net                 { get; set; }
    public IFiles                Files               { get; set; }

    #endregion public properties

    private static   GLFWCallbacks.ErrorCallback? _errorCallback = null;
    private volatile DesktopGLWindow?             _currentWindow = null;
    private          Sync?                        _sync          = null;
    private          bool                         _running       = true;

    // ------------------------------------------------------------------------

    public DesktopGLApplication( IApplicationListener listener,
                                 DesktopGLApplicationConfiguration config )
    {
        InitialiseGL();

        ApplicationLogger = new DesktopGLApplicationLogger();

        config.Title ??= listener.GetType().Name;

        this.Config = config = DesktopGLApplicationConfiguration.Copy( config );

        Gdx.App = this;

        if ( !config.DisableAudio )
        {
            try
            {
                this.Audio = CreateAudio( config );
            }
            catch ( System.Exception e )
            {
                Log( "GLApplication", "Couldn't initialize audio, disabling audio", e );

                this.Audio = new MockAudio();
            }
        }
        else
        {
            this.Audio = new MockAudio();
        }

        this.Files     = CreateFiles();
        this.Net       = new DesktopGLNet( config );
        this.Clipboard = new DesktopGLClipboard();
        this._sync     = new Sync();

        Gdx.Audio = this.Audio;
        Gdx.Files = this.Files;
        Gdx.Net   = this.Net;

        Windows.Add( CreateWindow( config, listener, 0 ) );

        try
        {
            Loop();
            CleanupWindows();
        }
        catch ( System.Exception e )
        {
            if ( e is SystemException exception )
            {
                throw exception;
            }
            else
            {
                throw new GdxRuntimeException( e );
            }
        }
        finally
        {
            Cleanup();
        }
    }

    /// <summary>
    /// Framework Main Loop
    /// </summary>
    protected void Loop()
    {
        List< DesktopGLWindow > closedWindows = new();

        while ( _running && ( Windows.Count > 0 ) )
        {
            Audio?.Update();

            var haveWindowsRendered = false;

            closedWindows.Clear();

            var targetFramerate = -2;

            foreach ( DesktopGLWindow window in Windows )
            {
                window.MakeCurrent();

                _currentWindow = window;

                if ( targetFramerate == -2 )
                {
                    targetFramerate = window.Config.ForegroundFPS;
                }

                lock ( LifecycleListeners )
                {
                    haveWindowsRendered |= window.Update();
                }

                if ( window.ShouldClose() )
                {
                    closedWindows.Add( window );
                }
            }

            GLFW.PollEvents();

            bool shouldRequestRendering;

            lock ( Runnables )
            {
                shouldRequestRendering = Runnables.Count > 0;

                ExecutedRunnables.Clear();
                ExecutedRunnables.AddAll( Runnables );

                Runnables.Clear();
            }

            foreach ( Runnable runnable in ExecutedRunnables )
            {
                runnable();
            }

            if ( shouldRequestRendering )
            {
                // Must follow Runnables execution so changes done by
                // Runnables are reflected in the following render.
                foreach ( DesktopGLWindow window in Windows )
                {
                    if ( !window.Graphics.IsContinuousRendering() )
                    {
                        window.RequestRendering();
                    }
                }
            }

            foreach ( DesktopGLWindow closedWindow in closedWindows )
            {
                if ( Windows.Count == 1 )
                {
                    // Lifecycle listener methods have to be called before ApplicationListener
                    // methods. The application will be disposed when _all_ windows have been
                    // disposed, which is the case, when there is only 1 window left, which is
                    // in the process of being disposed.
                    for ( var i = LifecycleListeners.Count - 1; i >= 0; i-- )
                    {
                        ILifecycleListener l = LifecycleListeners[ i ];

                        l.Pause();
                        l.Dispose();
                    }

                    LifecycleListeners.Clear();
                }

                closedWindow.Dispose();

                Windows.Remove( closedWindow );
            }

            if ( !haveWindowsRendered )
            {
                // Sleep a few milliseconds in case no rendering was requested
                // with continuous rendering disabled.
                try
                {
                    Thread.Sleep( 1000 / Config!.IdleFPS );
                }
                catch ( ThreadInterruptedException )
                {
                    // ignore
                }
            }
            else if ( targetFramerate > 0 )
            {
                // sleep as needed to meet the target framerate
                _sync?.SyncFrameRate( targetFramerate );
            }
        }
    }

    protected void CleanupWindows()
    {
        lock ( LifecycleListeners )
        {
            foreach ( ILifecycleListener lifecycleListener in LifecycleListeners )
            {
                lifecycleListener.Pause();
                lifecycleListener.Dispose();
            }
        }

        foreach ( DesktopGLWindow window in Windows )
        {
            window.Dispose();
        }

        Windows.Clear();
    }

    protected void Cleanup()
    {
        DesktopGLCursor.DisposeSystemCursors();

        Audio?.Dispose();

        _errorCallback = null;

        GLFW.Terminate();
    }

    public void Debug( string tag, string message )
    {
        if ( LogLevel >= IApplication.LOG_DEBUG )
        {
            ApplicationLogger?.Debug( tag, message );
        }
    }

    public void Debug( string tag, string message, System.Exception exception )
    {
        if ( LogLevel >= IApplication.LOG_DEBUG )
        {
            ApplicationLogger?.Debug( tag, message, exception );
        }
    }

    public void Log( string tag, string message )
    {
        if ( LogLevel >= IApplication.LOG_INFO )
        {
            ApplicationLogger?.Log( tag, message );
        }
    }

    public void Log( string tag, string message, System.Exception exception )
    {
        if ( LogLevel >= IApplication.LOG_INFO )
        {
            ApplicationLogger?.Log( tag, message, exception );
        }
    }

    public void Error( string tag, string message )
    {
        if ( LogLevel >= IApplication.LOG_ERROR )
        {
            ApplicationLogger?.Error( tag, message );
        }
    }

    public void Error( string tag, string message, System.Exception exception )
    {
        if ( LogLevel >= IApplication.LOG_ERROR )
        {
            ApplicationLogger?.Error( tag, message, exception );
        }
    }

    public IPreferences GetPreferences( string name )
    {
        if ( Preferences!.ContainsKey( name ) )
        {
            return Preferences.Get( name );
        }

        IPreferences prefs = new DesktopGLPreferences( name );

        Preferences.Put( name, prefs );

        return prefs;
    }

    /// <summary>
    /// Creates a new <see cref="DesktopGLWindow"/> using the provided listener and
    /// <see cref="DesktopGLWindowConfiguration"/>.
    /// <para>
    /// This function only just instantiates a <see cref="DesktopGLWindow"/> and
    /// returns immediately. The actual window creation is postponed with
    /// <see cref="DesktopGLApplication.PostRunnable(Runnable)"/> until after all
    /// existing windows are updated.
    /// </para>
    /// </summary>
    public DesktopGLWindow NewWindow( IApplicationListener listener, DesktopGLWindowConfiguration config )
    {
        GdxRuntimeException.ThrowIfNull( Config );

        DesktopGLApplicationConfiguration appConfig = DesktopGLApplicationConfiguration.Copy( this.Config );

        appConfig.SetWindowConfiguration( config );

        return CreateWindow( appConfig, listener, 0 ); //TODO:
    }

    public DesktopGLWindow CreateWindow( DesktopGLApplicationConfiguration config,
                                         IApplicationListener listener,
                                         long sharedContext )
    {
        var window = new DesktopGLWindow( listener, config, this );

        if ( sharedContext == 0 )
        {
            // the main window is created immediately
            CreateWindow( window, config, sharedContext );
        }
        else
        {
            // creation of additional windows is deferred to avoid GL context trouble
            PostRunnable( () =>
            {
                CreateWindow( window, config, sharedContext );
                Windows.Add( window );
            } );
        }

        unsafe
        {
            GLFW.MakeContextCurrent( window.WindowHandle );
        }

        return window;
    }

    public void CreateWindow( DesktopGLWindow window,
                              DesktopGLApplicationConfiguration config,
                              long sharedContext )
    {
    }

    private long CreateGlfwWindow( DesktopGLApplicationConfiguration config, long sharedContextWindow )
    {
        GLFW.DefaultWindowHints();
        GLFW.WindowHint( GLFW.GLFW_VISIBLE, GLFW.GLFW_FALSE );
        GLFW.WindowHint( GLFW.GLFW_RESIZABLE, config.windowResizable ? GLFW.GLFW_TRUE : GLFW.GLFW_FALSE );
        GLFW.WindowHint( GLFW.GLFW_MAXIMIZED, config.windowMaximized ? GLFW.GLFW_TRUE : GLFW.GLFW_FALSE );
        GLFW.WindowHint( GLFW.GLFW_AUTO_ICONIFY, config.autoIconify ? GLFW.GLFW_TRUE : GLFW.GLFW_FALSE );

        GLFW.WindowHint( GLFW.GLFW_RED_BITS, config.r );
        GLFW.WindowHint( GLFW.GLFW_GREEN_BITS, config.g );
        GLFW.WindowHint( GLFW.GLFW_BLUE_BITS, config.b );
        GLFW.WindowHint( GLFW.GLFW_ALPHA_BITS, config.a );
        GLFW.WindowHint( GLFW.GLFW_STENCIL_BITS, config.stencil );
        GLFW.WindowHint( GLFW.GLFW_DEPTH_BITS, config.depth );
        GLFW.WindowHint( GLFW.GLFW_SAMPLES, config.samples );

        if ( config.UseGL30 )
        {
            GLFW.WindowHint( GLFW.GLFW_CONTEXT_VERSION_MAJOR, config.gles30ContextMajorVersion );
            GLFW.WindowHint( GLFW.GLFW_CONTEXT_VERSION_MINOR, config.gles30ContextMinorVersion );

            if ( SharedLibraryLoader.IsMac )
            {
                // hints mandatory on OS X for GL 3.2+ context creation, but fail on Windows if the
                // WGL_ARB_create_context extension is not available
                // see: http://www.glfw.org/docs/latest/compat.html
                GLFW.WindowHint( GLFW.GLFW_OPENGL_FORWARD_COMPAT, GLFW.GLFW_TRUE );
                GLFW.WindowHint( GLFW.GLFW_OPENGL_PROFILE, GLFW.GLFW_OPENGL_CORE_PROFILE );
            }
        }

        if ( config.TransparentFramebuffer )
        {
            GLFW.WindowHint( GLFW.GLFW_TRANSPARENT_FRAMEBUFFER, GLFW.GLFW_TRUE );
        }

        if ( config.Debug )
        {
            GLFW.WindowHint( GLFW.GLFW_OPENGL_DEBUG_CONTEXT, GLFW.GLFW_TRUE );
        }

        long windowHandle = 0;

        if ( config.FullscreenMode != null )
        {
            GLFW.WindowHint( GLFW.GLFW_REFRESH_RATE, config.FullscreenMode.RefreshRate );

            windowHandle = GLFW.CreateWindow( config.FullscreenMode.Width,
                                              config.FullscreenMode.Height,
                                              config.Title,
                                              config.FullscreenMode.MonitorHandle,
                                              sharedContextWindow );
        }
        else
        {
            GLFW.WindowHint( GLFW.GLFW_DECORATED, config.WindowDecorated ? GLFW.GLFW_TRUE : GLFW.GLFW_FALSE );

            windowHandle = GLFW.CreateWindow( config.WindowWidth,
                                              config.WindowHeight,
                                              config.Title,
                                              0,
                                              sharedContextWindow );
        }

        if ( windowHandle == 0 )
        {
            throw new GdxRuntimeException( "Couldn't create window" );
        }

        DesktopGLWindow.SetSizeLimits( windowHandle,
                                       config.WindowMinWidth,
                                       config.WindowMinHeight,
                                       config.WindowMaxWidth,
                                       config.WindowMaxHeight );

        if ( config.FullscreenMode == null )
        {
            if ( config is { WindowX: -1, WindowY: -1 } )
            {
                var windowWidth  = Math.Max( config.WindowWidth, config.WindowMinWidth );
                var windowHeight = Math.Max( config.WindowHeight, config.WindowMinHeight );

                if ( config.WindowMaxWidth > -1 )
                {
                    windowWidth = Math.Min( windowWidth, config.WindowMaxWidth );
                }

                if ( config.WindowMaxHeight > -1 )
                {
                    windowHeight = Math.Min( windowHeight, config.WindowMaxHeight );
                }

                long monitorHandle = GLFW.GetPrimaryMonitor();

                if ( config.WindowMaximized && ( config.MaximizedMonitor != null ) )
                {
                    monitorHandle = config.MaximizedMonitor.monitorHandle;
                }

                IntBuffer areaXPos   = BufferUtils.NewIntBuffer( 1 );
                IntBuffer areaYPos   = BufferUtils.NewIntBuffer( 1 );
                IntBuffer areaWidth  = BufferUtils.NewIntBuffer( 1 );
                IntBuffer areaHeight = BufferUtils.NewIntBuffer( 1 );

                GLFW.GetMonitorWorkarea( monitorHandle,
                                         areaXPos,
                                         areaYPos,
                                         areaWidth,
                                         areaHeight );

                GLFW.glfwSetWindowPos( windowHandle,
                                       areaXPos.get( 0 ) + areaWidth.get( 0 ) / 2 - windowWidth / 2,
                                       areaYPos.get( 0 ) + areaHeight.get( 0 ) / 2 - windowHeight / 2 );
            }
            else
            {
                GLFW.glfwSetWindowPos( windowHandle, config.windowX, config.windowY );
            }

            if ( config.windowMaximized )
            {
                GLFW.glfwMaximizeWindow( windowHandle );
            }
        }

        if ( config.windowIconPaths != null )
        {
            DesktopGLWindow.SetIcon( windowHandle, config.windowIconPaths, config.windowIconFileType );
        }

        GLFW.MakeContextCurrent( windowHandle );
        GLFW.SwapInterval( config.VSyncEnabled ? 1 : 0 );

        GL.CreateCapabilities();

        InitiateGL();

        if ( !GLVersion!.IsVersionEqualToOrHigher( 2, 0 ) )
        {
            throw new GdxRuntimeException( "OpenGL 2.0 or higher with the FBO extension is required. OpenGL version: "
                                         + GL11.glGetString( GL11.GL_VERSION )
                                         + "\n"
                                         + GLVersion?.DebugVersionString() );
        }

        if ( !SupportsFBO() )
        {
//            throw new GdxRuntimeException( "OpenGL 2.0 or higher with the FBO extension is required. OpenGL version: "
//                                         + GL.GLGetString( GL11.GL_VERSION )
//                                         + ", FBO extension: false\n"
//                                         + GLVersion?.DebugVersionString() );
        }

        if ( config.Debug )
        {
//            glDebugCallback = GLUtil.setupDebugMessageCallback( config.debugStream );
//            SetGLDebugMessageControl( GLDebugMessageSeverity.Notification, false );
        }

        return windowHandle;
    }

    public void PostRunnable( Runnable runnable )
    {
        lock ( Runnables )
        {
            Runnables.Add( runnable );
        }
    }

    private static bool SupportsFBO()
    {
        GdxRuntimeException.ThrowIfNull( GLVersion );

        // FBO is in core since OpenGL 3.0,
        // see https://www.opengl.org/wiki/Framebuffer_Object
        return GLVersion.IsVersionEqualToOrHigher( 3, 0 )
            || GLFW.ExtensionSupported( "GL_EXT_framebuffer_object" )
            || GLFW.ExtensionSupported( "GL_ARB_framebuffer_object" );
    }

    public static void InitialiseGL()
    {
        if ( _errorCallback == null )
        {
            DesktopGLNativesLoader.Load();

            GLFW.SetErrorCallback( _errorCallback );
            GLFW.InitHint( InitHintBool.JoystickHatButtons, false );

            if ( GLFW.Init() )
            {
                throw new GdxRuntimeException( "Unable to initialise Glfw!" );
            }
        }
    }

    public IGLAudio CreateAudio( DesktopGLApplicationConfiguration config )
    {
        throw new NotImplementedException();
    }

    public IDesktopGLInput CreateInput( DesktopGLWindow window )
    {
        throw new NotImplementedException();
    }

    protected IFiles CreateFiles()
    {
        return new DesktopGLFiles();
    }

    public int GetVersion()
    {
        return 0;
    }

    public void Exit()
    {
        _running = false;
    }

    public void AddLifecycleListener( ILifecycleListener listener )
    {
        lock ( LifecycleListeners )
        {
            LifecycleListeners.Add( listener );
        }
    }

    public void RemoveLifecycleListener( ILifecycleListener listener )
    {
        lock ( LifecycleListeners )
        {
            LifecycleListeners.Remove( listener );
        }
    }

    [PublicAPI]
    public struct Gldms
    {
        public int gl43;
        public int khr;
        public int arb;
        public int amd;

        public Gldms( int gl43, int khr, int arb, int amd )
        {
            this.gl43 = gl43;
            this.khr  = khr;
            this.arb  = arb;
            this.amd  = amd;
        }
    }

    [PublicAPI]
    public record GLDebugMessageSeverity
    {
//        public Gldms High = new(
//            GL43.GL_DEBUG_SEVERITY_HIGH,
//            KHRDebug.GL_DEBUG_SEVERITY_HIGH,
//            ARBDebugOutput.GL_DEBUG_SEVERITY_HIGH_ARB,
//            AMDDebugOutput.GL_DEBUG_SEVERITY_HIGH_AMD );
//        public Gldms Medium = new(
//            GL43.GL_DEBUG_SEVERITY_MEDIUM,
//            KHRDebug.GL_DEBUG_SEVERITY_MEDIUM,
//            ARBDebugOutput.GL_DEBUG_SEVERITY_MEDIUM_ARB,
//            AMDDebugOutput.GL_DEBUG_SEVERITY_MEDIUM_AMD );
//        public Gldms Low = new(
//            GL43.GL_DEBUG_SEVERITY_LOW,
//            KHRDebug.GL_DEBUG_SEVERITY_LOW,
//            ARBDebugOutput.GL_DEBUG_SEVERITY_LOW_ARB,
//            AMDDebugOutput.GL_DEBUG_SEVERITY_LOW_AMD );
//        public Gldms Notification = new(
//            GL43.GL_DEBUG_SEVERITY_NOTIFICATION,
//            KHRDebug.GL_DEBUG_SEVERITY_NOTIFICATION,
//            -1,
//            -1 );
    }

    /// <summary>
    /// Enables or disables GL debug messages for the specified severity level.
    /// Returns false if the severity level could not be set (e.g. the NOTIFICATION
    /// level is not supported by the ARB and AMD extensions).
    /// </summary>
    /// <seealso cref="DesktopGLApplicationConfiguration.EnableGLDebugOutput(bool, StreamWriter)"/>
    public static bool SetGLDebugMessageControl( GLDebugMessageSeverity severity, bool enabled )
    {
//        GLCapabilities caps         = GL.GetCapabilities();
//        const int      GL_DONT_CARE = 0x1100; // not defined anywhere yet
//
//        if ( caps.OpenGL43 )
//        {
//            GL43.glDebugMessageControl( GL_DONT_CARE, GL_DONT_CARE, severity.gl43, ( IntBuffer )null, enabled );
//
//            return true;
//        }
//
//        if ( caps.GL_KHR_debug )
//        {
//            KHRDebug.glDebugMessageControl( GL_DONT_CARE, GL_DONT_CARE, severity.khr, ( IntBuffer )null, enabled );
//
//            return true;
//        }
//
//        if ( caps.GL_ARB_debug_output && severity.arb != -1 )
//        {
//            ARBDebugOutput.glDebugMessageControlARB( GL_DONT_CARE, GL_DONT_CARE, severity.arb, ( IntBuffer )null, enabled );
//
//            return true;
//        }
//
//        if ( caps.GL_AMD_debug_output && severity.amd != -1 )
//        {
//            AMDDebugOutput.glDebugMessageEnableAMD( GL_DONT_CARE, severity.amd, ( IntBuffer )null, enabled );
//
//            return true;
//        }

        return false;
    }

    /// <summary>
    /// </summary>
    private void InitiateGL()
    {
        GLFW.GetVersion( out var major, out var minor, out var revision );

        GLVersion = new GLVersion(
            IApplication.ApplicationType.Desktop,
            $"{major}.{minor}.{revision}",
            Gdx.GL20.GLGetString( IGL20.GL_VENDOR ),
            Gdx.GL20.GLGetString( IGL20.GL_RENDERER )
            );
    }
}
