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

using Corelib.LibCore.Core;
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;
using DesktopGLBackend.Audio;
using DesktopGLBackend.Audio.Mock;
using DesktopGLBackend.Files;
using DesktopGLBackend.Input;
using DesktopGLBackend.Utils;
using DesktopGLBackend.Window;
using JetBrains.Annotations;
using Exception = System.Exception;
using Platform = Corelib.LibCore.Core.Platform;

namespace DesktopGLBackend.Core;

/// <summary>
/// Creates, and manages, an application to for Windows OpenGL backends. 
/// </summary>
[PublicAPI]
public class DesktopGLApplication : IDesktopGLApplicationBase
{
    #region public properties

    public DesktopGLApplicationConfiguration? Config             { get; set; }
    public Dictionary< string, IPreferences > Preferences        { get; set; }
    public List< DesktopGLWindow >            Windows            { get; set; } = [ ];
    public List< IRunnable.Runnable >         Runnables          { get; set; } = [ ];
    public List< IRunnable.Runnable >         ExecutedRunnables  { get; set; } = [ ];
    public List< ILifecycleListener >         LifecycleListeners { get; set; } = [ ];

    public IClipboard?   Clipboard  { get; set; }
    public IGLAudio?     Audio      { get; set; }
    public INet          Network    { get; set; }
    public IFiles        Files      { get; set; }
    public GLVersion?    GLVersion  { get; set; }
    public OpenGLProfile OGLProfile { get; private set; } = OpenGLProfile.CoreProfile;

    #endregion public properties

    // ========================================================================

    private const int FR_UNINITIALISED = -2;

    private static   GlfwErrorCallback? _errorCallback;
    private readonly Sync?              _sync;
    private          DesktopGLWindow    _desktopGLWindow;
    private          GLFW.Window?       _currentGLWindow;
    private          bool               _running         = true;
    private          bool               _glfwInitialised = false;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a new Desktop Gl Application using the provided <see cref="DesktopGLApplicationConfiguration"/>.
    /// </summary>
    /// <param name="listener"> The <see cref="IApplicationListener"/> to use. </param>
    /// <param name="config"> The <see cref="DesktopGLApplicationConfiguration"/> to use.</param>
    public DesktopGLApplication( IApplicationListener listener,
                                 DesktopGLApplicationConfiguration config )
    {
        // This MUST be the first call, so that the Logger class is initialised correctly.
        Gdx.Initialise( this );

        // Config.Title becomes the name of the ApplicationListener if
        // it has no value at this point.
        Config       =   DesktopGLApplicationConfiguration.Copy( config );
        Config.Title ??= listener.GetType().Name;

        // Initialise the persistant data manager
        Preferences = new Dictionary< string, IPreferences >();

        // Initialise the global environment shortcuts. 'Gdx.Audio', 'Gdx.Files', and 'Gdx.Net' are instances
        // of classes implementing IAudio, IFiles, and INet resprectively, and are used to access LughSharp
        // members 'Audio', 'Files', and 'Network' are instances of classes which extend the aforementioned
        // classes, and are used in backend code only.
        // Note: Gdx.Graphics is set later, during window creation.
        Audio     = CreateAudio( Config );
        Files     = new DesktopGLFiles();
        Network   = new DesktopGLNet( Config );
        Clipboard = new DesktopGLClipboard();

        Gdx.Audio = Audio;
        Gdx.Files = Files;
        Gdx.Net   = Network;
        Gdx.GL    = new GLBindings();

        _sync            = new Sync();
        _desktopGLWindow = null!;

        InitialiseGLFW();

        // Create the window(s)
        Windows.Add( CreateWindow( Config, listener, 0 ) );

//        OutputWindowsDebug();
    }

    /// <summary>
    /// The entry point for running code using this framework. At this point at least one window will have been
    /// created, GLFW will have been set up, and the framework properly initialised.
    /// <para>
    /// This passes control to <see cref="Loop()"/> and stays there until the app is finished. At this point
    /// <see cref="CleanupWindows"/> is called, followed by <see cref="Cleanup"/>.
    /// </para>
    /// </summary>
    public void Run()
    {
        Logger.Checkpoint( true, true );

        try
        {
            Loop();
            CleanupWindows();
        }
        catch ( Exception e )
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
    /// Framework Main Loop.
    /// </summary>
    protected void Loop()
    {
        Logger.Checkpoint();
        Logger.Debug( $"_running: {_running} Number of windows: {Windows.Count}" );
        Logger.Debug( "Entering framework loop" );

        List< DesktopGLWindow > closedWindows = [ ];

        while ( _running && ( Windows.Count > 0 ) )
        {
            Glfw.PollEvents();

            var haveWindowsRendered = false;
            var targetFramerate     = FR_UNINITIALISED;

            closedWindows.Clear();

            // Update active windows
            foreach ( var window in Windows )
            {
                window.MakeCurrent();

                _desktopGLWindow                    = window;
                _desktopGLWindow.Graphics.GLVersion = GLVersion;

                if ( targetFramerate == FR_UNINITIALISED )
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

            bool shouldRequestRendering;

            lock ( Runnables )
            {
                shouldRequestRendering = Runnables.Count > 0;

                ExecutedRunnables.Clear();

                foreach ( var runnable in Runnables )
                {
                    ExecutedRunnables.Add( runnable );
                }

                Runnables.Clear();
            }

            // Handle all Runnables.
            foreach ( var runnable in ExecutedRunnables )
            {
                runnable();
            }

            if ( shouldRequestRendering )
            {
                // This section Must follow Runnables execution so changes made by
                // Runnables are reflected in the following render.
                foreach ( var window in Windows )
                {
                    if ( !window.Graphics.ContinuousRendering )
                    {
                        window.RequestRendering();
                    }
                }
            }

            // Tidy up any closed windows
            foreach ( var window in closedWindows )
            {
                if ( Windows.Count == 1 )
                {
                    // Lifecycle listener methods have to be called before ApplicationListener
                    // methods. The application will be disposed when _all_ windows have been
                    // disposed, which is the case, when there is only 1 window left, which is
                    // in the process of being disposed.
                    for ( var i = LifecycleListeners.Count - 1; i >= 0; i-- )
                    {
                        var l = LifecycleListeners[ i ];

                        l.Pause();
                        l.Dispose();
                    }

                    LifecycleListeners.Clear();
                }

                window.Dispose();
                Windows.Remove( window );
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

            Audio?.Update();
        }
    }

    // ========================================================================
    // ========================================================================

    #region window creation handlers

    /// <summary>
    /// Creates a new <see cref="DesktopGLWindow"/> using the provided listener and
    /// <see cref="DesktopGLWindowConfiguration"/>.
    /// <para>
    /// This function only instantiates a <see cref="DesktopGLWindow"/> and
    /// returns immediately. The actual window creation is postponed with
    /// <see cref="DesktopGLApplication.PostRunnable(IRunnable.Runnable)"/> until after all
    /// existing windows are updated.
    /// </para>
    /// </summary>
    public DesktopGLWindow NewWindow( IApplicationListener listener, DesktopGLWindowConfiguration config )
    {
        GdxRuntimeException.ThrowIfNull( Config );

        var appConfig = DesktopGLApplicationConfiguration.Copy( Config );

        appConfig.SetWindowConfiguration( config );

        return CreateWindow( appConfig, listener, 0 );
    }

    /// <summary>
    /// Creates a new <see cref="DesktopGLWindow"/> using the 
    /// </summary>
    /// <param name="config"></param>
    /// <param name="listener"></param>
    /// <param name="sharedContext"></param>
    /// <returns></returns>
    public DesktopGLWindow CreateWindow( DesktopGLApplicationConfiguration config,
                                         IApplicationListener listener,
                                         long sharedContext )
    {
        // Create the manager for the main window
        var dlgWindow = new DesktopGLWindow( listener, config, this );

        Logger.Debug( $"sharedContext = {sharedContext}" );

        if ( sharedContext == 0 )
        {
            // the main window is created immediately
            dlgWindow = CreateWindow( dlgWindow, config, 0 );
        }
        else
        {
            // creation of additional windows is deferred to avoid GL context trouble
            PostRunnable( () =>
            {
                dlgWindow = CreateWindow( dlgWindow, config, sharedContext );
                Windows.Add( dlgWindow );
            } );
        }

        Logger.Debug( $"{dlgWindow.Config.WindowWidth}, {dlgWindow.Config.WindowHeight}" );

        return dlgWindow;
    }

    /// <summary>
    /// </summary>
    /// <param name="dglWindow"></param>
    /// <param name="config"></param>
    /// <param name="sharedContext"></param>
    public DesktopGLWindow CreateWindow( DesktopGLWindow? dglWindow, DesktopGLApplicationConfiguration config, long sharedContext )
    {
        ArgumentNullException.ThrowIfNull( dglWindow );

        Logger.Checkpoint();

        var windowHandle = CreateGLFWWindow( config, sharedContext );

        Logger.Checkpoint();

        dglWindow.Initialise( windowHandle, this );
        dglWindow.SetVisible( config.InitialVisibility );

        Logger.Checkpoint();

        for ( var i = 0; i < 2; i++ )
        {
            Gdx.GL.glClearColor( config.InitialBackgroundColor.R,
                                 config.InitialBackgroundColor.G,
                                 config.InitialBackgroundColor.B,
                                 config.InitialBackgroundColor.A );

            Gdx.GL.glClear( IGL.GL_COLOR_BUFFER_BIT );
            Glfw.SwapBuffers( windowHandle );
        }

        Logger.Checkpoint();

        return dglWindow;
    }

    /// <summary>
    /// </summary>
    /// <param name="config"></param>
    /// <param name="sharedContextWindow"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private GLFW.Window CreateGLFWWindow( DesktopGLApplicationConfiguration config, long sharedContextWindow )
    {
        Logger.Checkpoint();

        SetWindowHints( config );

        GLFW.Monitor monitor;

        if ( config.FullscreenMode != null )
        {
            // Create a fullscreen window
            
            Glfw.WindowHint( WindowHint.RefreshRate, config.FullscreenMode.RefreshRate );

            Logger.Debug( "Fullscreen" );
            Logger.Debug( $"windowWidth : {config.FullscreenMode.Width}" );
            Logger.Debug( $"windowHeight: {config.FullscreenMode.Height}" );

            monitor = config.FullscreenMode.MonitorHandle;

            var mode = Glfw.GetVideoMode( monitor );

            _currentGLWindow = Glfw.CreateWindow( mode.Width,
                                                  mode.Height,
                                                  config.Title ?? "",
                                                  monitor,
                                                  GLFW.Window.NULL );
        }
        else
        {
            // Create a 'windowed' window
            
            Logger.Debug( "Windowed" );
            Logger.Debug( $"windowWidth : {config.WindowWidth}" );
            Logger.Debug( $"windowHeight: {config.WindowHeight}" );

            monitor = Glfw.GetPrimaryMonitor();

            _currentGLWindow = Glfw.CreateWindow( config.WindowWidth,
                                                  config.WindowHeight,
                                                  config.Title ?? "",
                                                  monitor,
                                                  GLFW.Window.NULL );
        }

        Logger.Checkpoint();

        if ( _currentGLWindow == null )
        {
            throw new NullReferenceException( "Failed to create window!" );
        }

        Glfw.MakeContextCurrent( _currentGLWindow );

        Logger.Checkpoint();

//        Gdx.GL.Import( Glfw.GetProcAddress );
        
        Logger.Checkpoint();

        Logger.Debug( $"config.WindowMinWidth  : {config.WindowMinWidth}" );
        Logger.Debug( $"config.WindowMinHeight : {config.WindowMinHeight}" );
        Logger.Debug( $"config.WindowMaxWidth  : {config.WindowMaxWidth}" );
        Logger.Debug( $"config.WindowMaxHeight : {config.WindowMaxHeight}" );

//        DesktopGLWindow.SetSizeLimits( _currentGLWindow,
//                                       config.WindowMinWidth,
//                                       config.WindowMinHeight,
//                                       config.WindowMaxWidth,
//                                       config.WindowMaxHeight );

        if ( config.FullscreenMode == null )
        {
            Logger.Checkpoint();

            if ( config is { WindowX: -1, WindowY: -1 } )
            {
                Logger.Checkpoint();

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

                var monitorHandle = Glfw.GetPrimaryMonitor();

                if ( config is { WindowMaximized: true, MaximizedMonitor: not null } )
                {
                    monitorHandle = config.MaximizedMonitor.MonitorHandle;
                }

                Logger.Checkpoint();
                Logger.Debug( $"monitorHandle : {monitorHandle}" );
                Logger.Debug( $"windowHandle  : {_currentGLWindow}" );
                Logger.Debug( $"config        : {config}" );
                Logger.Debug( $"windowWidth   : {windowWidth}" );
                Logger.Debug( $"windowHeight  : {windowHeight}" );

                Glfw.GetMonitorWorkarea( monitorHandle, out var areaX, out var areaY, out var areaW, out var areaH );

                Logger.Debug( $"areaXPos   : {areaX}" );
                Logger.Debug( $"areaYPos   : {areaY}" );
                Logger.Debug( $"areaWidth  : {areaW}" );
                Logger.Debug( $"areaHeight : {areaH}" );

                Glfw.SetWindowPos( _currentGLWindow,
                                   ( areaX + ( areaW / 2 ) ) - ( windowWidth / 2 ),
                                   ( areaY + ( areaH / 2 ) ) - ( windowHeight / 2 ) );
            }
            else
            {
                Logger.Checkpoint();

                Logger.Debug( $"windowHandle  : {_currentGLWindow}" );
                Logger.Debug( $"config        : {config}" );
                Logger.Debug( $"config.WindowX: {config.WindowX}" );
                Logger.Debug( $"config.WindowY: {config.WindowY}" );

                Glfw.SetWindowPos( _currentGLWindow, config.WindowX, config.WindowY );
            }

            if ( config.WindowMaximized )
            {
                Glfw.MaximizeWindow( _currentGLWindow );
            }
        }

        if ( config.WindowIconPaths != null )
        {
            _desktopGLWindow.SetIcon( _currentGLWindow, config.WindowIconPaths, config.WindowIconFileType );
        }

        Glfw.SwapInterval( config.VSyncEnabled ? 1 : 0 );

        InitGLVersion();

        if ( config.Debug )
        {
//TODO:
//            GlDebugCallback = Glfw.DebugMessageCallback( config.debugStream );
//            SetGLDebugMessageControl( GLDebugMessageSeverity.Notification, false );
        }

        return _currentGLWindow;
    }

    #endregion window creation handlers

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    private unsafe void InitGLVersion()
    {
        Logger.Checkpoint();

        // Retrieve OpenGL version
        Glfw.GetVersion( out var glMajor, out var glMinor, out var revision );

        Logger.Debug( $"GLFW : {glMajor}.{glMinor}.{revision} : glProfile: {OpenGLProfile.CoreProfile}" );

        // Set the client API to use OpenGL.
        Glfw.WindowHint( WindowHint.ClientAPI, ClientAPI.OpenGLAPI );

        // Set the OpenGL context version based on the retrieved major and minor version numbers.
        Glfw.WindowHint( WindowHint.ContextVersionMajor, glMajor );
        Glfw.WindowHint( WindowHint.ContextVersionMinor, glMinor );

        // Determine the OpenGL profile to use based on the profile string retrieved.
        OGLProfile = OpenGLProfile.CoreProfile; // Use the core profile.

        GLVersion = new GLVersion( Platform.ApplicationType.WindowsGL,
                                   null,   //Gdx.GL.glGetString( IGL.GL_VENDOR ),
                                   null ); //Gdx.GL.glGetString( IGL.GL_RENDERER ) );

        Logger.Checkpoint();

        // Set the flag indicating that OpenGL has been initialized.
        _glfwInitialised = true;
    }

    /// <summary>
    /// Sets up an OpenGL context using GLFW, retrieves OpenGL version and profile information
    /// from the project settings, and initializes the OpenGL environment accordingly. 
    /// </summary>
    /// <exception cref="GdxRuntimeException"> Thrown if any problems occurred with GL initialisation. </exception>
    public void InitialiseGLFW()
    {
        try
        {
            Logger.Checkpoint();

            if ( !_glfwInitialised )
            {
                DesktopGLNativesLoader.Load(); // This may no longer be needed

                _errorCallback = ErrorCallback;
                Glfw.SetErrorCallback( _errorCallback );
                Glfw.InitHint( InitHint.JoystickHatButtons, false );

                if ( !Glfw.Init() )
                {
                    Glfw.GetError( out var error );

                    Logger.Divider();
                    Logger.Debug( "Failed to initialise GLFW" );
                    Logger.Debug( error );
                    Logger.Divider();
                    
                    System.Environment.Exit( 1 );
                }

                Logger.Debug( $"Success: GLFW Version: {Glfw.GetVersionString()}", true );

                _glfwInitialised = true;
            }
        }
        catch ( Exception e )
        {
            throw new ApplicationException( $"Failure in InitialiseGLFW() : {e}" );
        }
    }

    private static void ErrorCallback( ErrorCode error, string description )
    {
        Logger.Error( $"ErrorCode: {error}, {description}" );
    }

    // ========================================================================

    /// <inheritdoc />
    public IGLAudio CreateAudio( DesktopGLApplicationConfiguration config )
    {
        IGLAudio audio;

        if ( !config.DisableAudio )
        {
            try
            {
                audio = new OpenALAudio( config.AudioDeviceSimultaneousSources,
                                         config.AudioDeviceBufferCount,
                                         config.AudioDeviceBufferSize );
            }
            catch ( Exception e )
            {
                Logger.Debug( $"Couldn't initialize audio, disabling audio: {e}" );

                audio = new MockAudio();
            }
        }
        else
        {
            audio = new MockAudio();
        }

        return audio;
    }

    /// <summary>
    /// Cleans up, and disposes of, any windows that have been closed.
    /// </summary>
    protected void CleanupWindows()
    {
        lock ( LifecycleListeners )
        {
            foreach ( var lifecycleListener in LifecycleListeners )
            {
                lifecycleListener.Pause();
                lifecycleListener.Dispose();
            }
        }

        foreach ( var window in Windows )
        {
            window.Dispose();
        }

        Windows.Clear();
    }

    /// <summary>
    /// Cleanup everything before shutdown.
    /// </summary>
    protected void Cleanup()
    {
        DesktopGLCursor.DisposeSystemCursors();

        Audio?.Dispose();

        _errorCallback = null;

        Glfw.Terminate();
    }

    /// <inheritdoc />
    public IPreferences GetPreferences( string name )
    {
        if ( Preferences.ContainsKey( name ) )
        {
            return Preferences.Get( name )!;
        }

        IPreferences prefs = new DesktopGLPreferences( name );

        Preferences.Put( name, prefs );

        return prefs;
    }

    /// <inheritdoc />
    public Platform.ApplicationType AppType
    {
        get => Platform.ApplicationType.WindowsGL;
        set { }
    }

    /// <inheritdoc />
    public void PostRunnable( IRunnable.Runnable runnable )
    {
        lock ( Runnables )
        {
            Runnables.Add( runnable );
        }
    }

    /// <inheritdoc />
    public virtual IDesktopGLInput CreateInput( DesktopGLWindow window )
    {
        return new DefaultDesktopGLInput( window );
    }

    /// <summary>
    /// Returns the Android API level on Android, the major OS version on iOS (5, 6, 7, ..),
    /// or 0 on the desktop.
    /// </summary>
    public virtual int GetVersion() => 0;

    /// <summary>
    /// Schedule an exit from the application. On android, this will cause a call to Pause()
    /// and Dispose() some time in the future. It will not immediately finish your application.
    /// On iOS this should be avoided in production as it breaks Apples guidelines
    /// </summary>
    public virtual void Exit()
    {
        _running = false;
    }

    /// <inheritdoc />
    public void AddLifecycleListener( ILifecycleListener listener )
    {
        lock ( LifecycleListeners )
        {
            LifecycleListeners.Add( listener );
        }
    }

    /// <inheritdoc />
    public void RemoveLifecycleListener( ILifecycleListener listener )
    {
        lock ( LifecycleListeners )
        {
            LifecycleListeners.Remove( listener );
        }
    }

    /// <summary>
    /// Initialise the main Window <see cref="WindowHint"/>s. Some Hints may be set
    /// elsewhere, but this is where most are initialised.
    /// </summary>
    /// <param name="config"> The current <see cref="DesktopGLApplicationConfiguration"/>. </param>
    private static void SetWindowHints( DesktopGLApplicationConfiguration config )
    {
        ArgumentNullException.ThrowIfNull( config );

        Glfw.DefaultWindowHints();

        Glfw.WindowHint( WindowHint.Visible, config.InitialVisibility );
        Glfw.WindowHint( WindowHint.Resizable, config.WindowResizable );
        Glfw.WindowHint( WindowHint.Maximized, config.WindowMaximized );
        Glfw.WindowHint( WindowHint.AutoIconify, config.AutoIconify );
        Glfw.WindowHint( WindowHint.Decorated, config.WindowDecorated );

        Glfw.WindowHint( WindowHint.RedBits, config.Red );
        Glfw.WindowHint( WindowHint.GreenBits, config.Green );
        Glfw.WindowHint( WindowHint.BlueBits, config.Blue );
        Glfw.WindowHint( WindowHint.AlphaBits, config.Alpha );
        Glfw.WindowHint( WindowHint.StencilBits, config.Stencil );
        Glfw.WindowHint( WindowHint.DepthBits, config.Depth );
        Glfw.WindowHint( WindowHint.Samples, config.Samples );

        Glfw.WindowHint( WindowHint.ClientAPI, ClientAPI.OpenGLAPI );
        Glfw.WindowHint( WindowHint.ContextVersionMajor, config.GLContextMajorVersion );
        Glfw.WindowHint( WindowHint.ContextVersionMinor, config.GLContextMinorVersion );
        Glfw.WindowHint( WindowHint.OpenGLProfile, OpenGLProfile.CoreProfile );
        Glfw.WindowHint( WindowHint.OpenGLForwardCompat, true );

        if ( config.TransparentFramebuffer )
        {
            Glfw.WindowHint( WindowHint.TransparentFramebuffer, true );
        }

        if ( config.Debug )
        {
            Glfw.WindowHint( WindowHint.OpenGLDebugContext, true );
        }
    }

    // ========================================================================

    public IApplicationListener GetApplicationListener() => _desktopGLWindow.Listener;
    public IInput GetInput() => _desktopGLWindow.Input;

    // ========================================================================
    // ========================================================================
    // ========================================================================
    // ========================================================================

    #region GLDebug specific

    [PublicAPI]
    public struct Gldms( int gl43, int khr, int arb, int amd )
    {
        public int GL43 = gl43;
        public int Khr  = khr;
        public int Arb  = arb;
        public int Amd  = amd;
    }

    [PublicAPI]
    public record GLDebugMessageSeverity
    {
        public Gldms High = new( IGL.GL_DEBUG_SEVERITY_HIGH,
                                 -1,   //KHRDebug.GL_DEBUG_SEVERITY_HIGH,
                                 -1,   //ARBDebugOutput.GL_DEBUG_SEVERITY_HIGH_ARB,
                                 -1 ); //AMDDebugOutput.GL_DEBUG_SEVERITY_HIGH_AMD );

        public Gldms Medium = new( IGL.GL_DEBUG_SEVERITY_MEDIUM,
                                   -1,   //KHRDebug.GL_DEBUG_SEVERITY_MEDIUM,
                                   -1,   //ARBDebugOutput.GL_DEBUG_SEVERITY_MEDIUM_ARB,
                                   -1 ); //AMDDebugOutput.GL_DEBUG_SEVERITY_MEDIUM_AMD );

        public Gldms Low = new( IGL.GL_DEBUG_SEVERITY_LOW,
                                -1,   //KHRDebug.GL_DEBUG_SEVERITY_LOW,
                                -1,   //ARBDebugOutput.GL_DEBUG_SEVERITY_LOW_ARB,
                                -1 ); //AMDDebugOutput.GL_DEBUG_SEVERITY_LOW_AMD );

        public Gldms Notification = new( IGL.GL_DEBUG_SEVERITY_NOTIFICATION,
                                         -1, //KHRDebug.GL_DEBUG_SEVERITY_NOTIFICATION,
                                         -1,
                                         -1 );
    }

//    /// <summary>
//    /// Enables or disables GL debug messages for the specified severity level.
//    /// Returns false if the severity level could not be set (e.g. the NOTIFICATION
//    /// level is not supported by the ARB and AMD extensions).
//    /// </summary>
    public static bool SetGLDebugMessageControl( GLDebugMessageSeverity severity, bool enabled )
    {
//        GLCapabilities caps = GL.GetCapabilities();
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

    #endregion GLDebug specific
}