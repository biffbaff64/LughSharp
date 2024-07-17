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


using LughSharp.Backends.DesktopGL.Audio;
using LughSharp.Backends.DesktopGL.Audio.Mock;
using LughSharp.Backends.DesktopGL.Files;
using LughSharp.Backends.DesktopGL.Input;
using LughSharp.Backends.DesktopGL.Utils;
using LughSharp.Backends.DesktopGL.Window;
using LughSharp.LibCore.Utils.Collections.Extensions;
using LughSharp.LibCore.Utils.Exceptions;
using Exception = System.Exception;
using Monitor = DotGLFW.Monitor;
using Platform = LughSharp.LibCore.Core.Platform;

namespace LughSharp.Backends.DesktopGL;

[PublicAPI]
public class DesktopGLApplication : IDesktopGLApplicationBase, IDisposable
{
    #region public properties

    public DesktopGLApplicationConfiguration? Config             { get; set; }
    public Dictionary< string, IPreferences > Preferences        { get; set; }
    public List< DesktopGLWindow >            Windows            { get; set; } = new();
    public List< IRunnable.Runnable >         Runnables          { get; set; } = new();
    public List< IRunnable.Runnable >         ExecutedRunnables  { get; set; } = new();
    public List< ILifecycleListener >         LifecycleListeners { get; set; } = new();

    public IClipboard?    Clipboard     { get; set; }
    public IGLAudio?      Audio         { get; set; }
    public INet           Network       { get; set; }
    public IFiles         Files         { get; set; }
    public OpenGLProfile? OGLProfile    { get; set; }
    public bool           GLInitialised { get; private set; } = false;
    public GLVersion?     GLVersion     { get; set; }

    #endregion public properties

    // ------------------------------------------------------------------------

    public IApplicationListener GetApplicationListener() => _currentWindow.Listener;
    public IInput               GetInput()               => _currentWindow.Input;

    // ------------------------------------------------------------------------

    private const int FR_UNINITIALISED = -2;

    private static   GlfwErrorCallback? _errorCallback;
    private readonly Sync?              _sync;
    private volatile DesktopGLWindow    _currentWindow;
    private          bool               _running = true;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new Desktop Gl Application.
    /// <para>
    /// Uses the provided <see cref="DesktopGLApplicationConfiguration"/>.
    /// </para>
    /// </summary>
    /// <param name="listener"> The <see cref="IApplicationListener"/> to use. </param>
    /// <param name="config"> The <see cref="DesktopGLApplicationConfiguration"/> to use.</param>
    public DesktopGLApplication( IApplicationListener listener,
                                 DesktopGLApplicationConfiguration config )
    {
        // This MUST be the first call in this constructor
        Gdx.Initialise( this );

        Logger.CheckPoint();
        
        // Config.Title becomes the name of the ApplicationListener if
        // it has no value at this point.
        config.Title ??= listener.GetType().Name;
        Config       =   DesktopGLApplicationConfiguration.Copy( config );

        Preferences = new Dictionary< string, IPreferences >();

        // Initialise the global environment shortcuts. 'Gdx.Audio', 'Gdx.Files',
        // and 'Gdx.Net' are instances of classes implementing IAudio, IFiles, and
        // INet resprectively, and are used to access LughSharp members.
        // 'Audio', 'Files', and 'Network' are instances of classes which extend
        // the aforementioned classes, and are used in backend code only.
        Audio   = CreateAudio( config );
        Files   = new DesktopGLFiles();
        Network = new DesktopGLNet( config );

        Gdx.Audio = Audio;
        Gdx.Files = Files;
        Gdx.Net   = Network;
        Gdx.GL    = new GLBindings();

        // Gdx.Graphics is set later, on window creation as they are connected.

        Clipboard = new DesktopGLClipboard();

        _sync          = new Sync();
        _currentWindow = null!;

        // GLFW initialisation
        InitialiseGLFW();

        // Create the window(s)
        Windows.Add( CreateWindow( config, listener, 0 ) );
        
        Logger.Debug( " - Finished" );
    }

    /// <summary>
    /// The framework entry point. This passes control to <see cref="Loop()"/> and
    /// stays there until the app is finished. At this point <see cref="CleanupWindows"/>
    /// is called, followed by <see cref="Cleanup"/>.
    /// </summary>
    public void Run()
    {
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
        Logger.CheckPoint();
        
        List< DesktopGLWindow > closedWindows = new();

        while ( _running && ( Windows.Count > 0 ) )
        {
            Audio?.Update();

            var haveWindowsRendered = false;
            var targetFramerate     = FR_UNINITIALISED;

            closedWindows.Clear();

            // Update active windows
            foreach ( var window in Windows )
            {
                window.MakeCurrent();

                _currentWindow                    = window;
                _currentWindow.Graphics.GLVersion = GLVersion;
                
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

            Glfw.PollEvents();

            bool shouldRequestRendering;

            lock ( Runnables )
            {
                shouldRequestRendering = Runnables.Count > 0;

                ExecutedRunnables.Clear();
                ExecutedRunnables.AddAll( Runnables );

                Runnables.Clear();
            }

            // Handle all Runnables.
            foreach ( var runnable in ExecutedRunnables )
            {
                runnable();
            }

            if ( shouldRequestRendering )
            {
                // This section Must follow Runnables execution so changes done by
                // Runnables are reflected in the following render.
                foreach ( var window in Windows )
                {
                    if ( !window.Graphics.ContinuousRendering )
                    {
                        window.RequestRendering();
                    }
                }
            }

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
        }
    }

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
        Logger.CheckPoint();

        GdxRuntimeException.ThrowIfNull( Config );

        var appConfig = DesktopGLApplicationConfiguration.Copy( Config );

        appConfig.SetGLContextVersion();
        appConfig.SetWindowConfiguration( config );

        return CreateWindow( appConfig, listener, 0 );
    }

    /// <summary>
    /// </summary>
    /// <param name="config"></param>
    /// <param name="listener"></param>
    /// <param name="sharedContext"></param>
    /// <returns></returns>
    public DesktopGLWindow CreateWindow( DesktopGLApplicationConfiguration config,
                                         IApplicationListener listener,
                                         long sharedContext )
    {
        Logger.CheckPoint();

        var dlgWindow = new DesktopGLWindow( listener, config, this );

        if ( sharedContext == 0 )
        {
            Logger.CheckPoint();
            
            // the main window is created immediately
            CreateWindow( dlgWindow, config, 0 );
        }
        else
        {
            Logger.CheckPoint();

            // creation of additional windows is deferred to avoid GL context trouble
            PostRunnable( () =>
            {
                CreateWindow( dlgWindow, config, sharedContext );
                Windows.Add( dlgWindow );
            } );
        }

        Logger.Debug( " - Finished" );
        
        return dlgWindow;
    }

    /// <summary>
    /// </summary>
    /// <param name="dglWindow"></param>
    /// <param name="config"></param>
    /// <param name="sharedContext"></param>
    public void CreateWindow( DesktopGLWindow? dglWindow, DesktopGLApplicationConfiguration config, long sharedContext )
    {
        ArgumentNullException.ThrowIfNull( dglWindow );

        Logger.CheckPoint();

        var windowHandle = CreateGLFWWindow( config, sharedContext );

        dglWindow.Create( windowHandle );
        dglWindow.SetVisible( config.InitialVisibility );

        Logger.CheckPoint();

        for ( var i = 0; i < 2; i++ )
        {
            Gdx.GL.glClearColor( config.InitialBackgroundColor.R,
                                 config.InitialBackgroundColor.G,
                                 config.InitialBackgroundColor.B,
                                 config.InitialBackgroundColor.A );

            Gdx.GL.glClear( IGL.GL_COLOR_BUFFER_BIT );
            Glfw.SwapBuffers( windowHandle );
        }

        Logger.Debug( " - Finished" );
    }

    /// <summary>
    /// </summary>
    /// <param name="config"></param>
    /// <param name="sharedContextWindow"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private GLFW.Window CreateGLFWWindow( DesktopGLApplicationConfiguration config, long sharedContextWindow )
    {
        Logger.CheckPoint();

        GLFW.Window? windowHandle;

        SetWindowHints( config );

        if ( config.FullscreenMode != null )
        {
            Logger.CheckPoint();

            Glfw.WindowHint( Hint.RefreshRate, config.FullscreenMode.RefreshRate );

            windowHandle = Glfw.CreateWindow( config.FullscreenMode.Width,
                                              config.FullscreenMode.Height,
                                              config.Title ?? "",
                                              config.FullscreenMode.MonitorHandle,
                                              GLFW.Window.NULL );

            Logger.CheckPoint();
        }
        else
        {
            Logger.CheckPoint();

            Logger.Debug( $"windowWidth : {config.WindowWidth}" );
            Logger.Debug( $"windowHeight: {config.WindowHeight}" );

            windowHandle = Glfw.CreateWindow( config.WindowWidth,
                                              config.WindowHeight,
                                              config.Title ?? "",
                                              Monitor.NULL,
                                              GLFW.Window.NULL );

            Logger.CheckPoint();
        }

        Logger.CheckPoint();

        DesktopGLWindow.SetSizeLimits( windowHandle ?? throw new GdxRuntimeException( "Couldn't create window!" ),
                                       config.WindowMinWidth,
                                       config.WindowMinHeight,
                                       config.WindowMaxWidth,
                                       config.WindowMaxHeight );

        if ( config.FullscreenMode == null )
        {
            if ( config is { WindowX: -1, WindowY: -1 } )
            {
                Logger.CheckPoint();

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

                Logger.CheckPoint();

                Glfw.GetMonitorWorkarea( monitorHandle,
                                         out var areaXPos,
                                         out var areaYPos,
                                         out var areaWidth,
                                         out var areaHeight );

                Logger.CheckPoint();

                Glfw.SetWindowPos( windowHandle,
                                   ( areaXPos + ( areaWidth / 2 ) ) - ( windowWidth / 2 ),
                                   ( areaYPos + ( areaHeight / 2 ) ) - ( windowHeight / 2 ) );
            }
            else
            {
                Logger.CheckPoint();

                Glfw.SetWindowPos( windowHandle, config.WindowX, config.WindowY );
            }

            if ( config.WindowMaximized )
            {
                Glfw.MaximizeWindow( windowHandle );
            }
        }

        if ( config.WindowIconPaths != null )
        {
            _currentWindow.SetIcon( windowHandle, config.WindowIconPaths, config.WindowIconFileType );
        }

        Logger.CheckPoint();

        Glfw.MakeContextCurrent( windowHandle );
        Glfw.SwapInterval( config.VSyncEnabled ? 1 : 0 );

        Logger.CheckPoint();

        InitialiseGL();

//        if ( config.Debug )
//        {
//            GlDebugCallback = Glfw.DebugMessageCallback( config.debugStream );
//            SetGLDebugMessageControl( GLDebugMessageSeverity.Notification, false );
//        }

        Logger.Debug( " - Finished" );

        return windowHandle;
    }

    /// <summary>
    /// Sets up an OpenGL context using GLFW, retrieves OpenGL version and profile information
    /// from the project settings, and initializes the OpenGL environment accordingly. 
    /// </summary>
    /// <remarks> 14.07.2024 - Merged with InitGLVersion(). </remarks>
    public unsafe void InitialiseGL()
    {
        Logger.CheckPoint();

        // Retrieve OpenGL version
        Glfw.GetVersion( out var glMajor, out var glMinor, out var revision );

        Logger.Debug( $"glMajor  : {glMajor}" );
        Logger.Debug( $"glMinor  : {glMinor}" );
        Logger.Debug( $"Revision : {revision}" );

        // Retrieve the OpenGL profile from the ',csproj' file
        var glProfile = Gdx.GL.GetProjectOpenGLProfile();

        Logger.Debug( $"glProfile: {glProfile}" );

        // Set the client API to use OpenGL.
        Glfw.WindowHint( Hint.ClientAPI, ClientAPI.OpenGLAPI );

        Logger.CheckPoint();

        // Set the OpenGL context version based on the retrieved major and minor version numbers.
        Glfw.WindowHint( Hint.ContextVersionMajor, glMajor );
        Glfw.WindowHint( Hint.ContextVersionMinor, glMinor );

        Logger.CheckPoint();

        // Determine the OpenGL profile to use based on the profile string retrieved.
        OGLProfile = glProfile switch
        {
            "CORE"   => OpenGLProfile.CoreProfile,   // Use the core profile.
            "COMPAT" => OpenGLProfile.CompatProfile, // Use the compatibility profile.
            var _    => throw new Exception( "Invalid OpenGL profile!" )
        };

        Logger.CheckPoint();

        GLVersion = new GLVersion( Platform.ApplicationType.WindowsGL,
                                   $"{glMajor}.{glMinor}.{revision}",
                                   null,   //Gdx.GL.glGetString( IGL.GL_VENDOR ),
                                   null ); //Gdx.GL.glGetString( IGL.GL_RENDERER ) );

        Logger.CheckPoint();

        if ( !GLVersion.IsVersionEqualToOrHigher( 2, 0 ) || !SupportsFBO() )
        {
            var (major, minor) = Gdx.GL.GetProjectOpenGLVersion();

            throw new GdxRuntimeException( $"OpenGL 2.0 or higher with the FBO extension is required. "
                                         + $"OpenGL version: {major}.{minor}"
                                         + $"\n{GLVersion.DebugVersionString()}" );
        }

        Logger.CheckPoint();

        Gdx.GL.Import( Glfw.GetProcAddress );

        Logger.CheckPoint();

        // Set the flag indicating that OpenGL has been initialized.
        GLInitialised = true;
    }

    /// <summary>
    /// Handle Glfw setup and initialisation.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// Thrown if any problems occurred with GL initialisation.
    /// </exception>
    public static void InitialiseGLFW()
    {
        Logger.CheckPoint();

        try
        {
            if ( _errorCallback == null )
            {
                DesktopGLNativesLoader.Load();

                _errorCallback = ErrorCallback;

                Glfw.SetErrorCallback( _errorCallback );
                Glfw.InitHint( InitHint.JoystickHatButtons, false );

                if ( !Glfw.Init() )
                {
                    Console.Error.WriteLine( "Failed to initialise GLFW" );
                    System.Environment.Exit( 1 );
                }
            }
        }
        catch ( Exception e )
        {
            throw new ApplicationException( $"Failure in InitialiseGL() : {e}" );
        }

        Logger.Debug( " - Finished" );

        return;

        // --------------------------------------------------------------------

        void ErrorCallback( ErrorCode error, string description )
        {
            Logger.Error( $"ErrorCode: {error}, {description}" );
        }

        // --------------------------------------------------------------------
    }

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
            return Preferences.Get( name );
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

    /// <inheritdoc />
    public virtual int GetVersion()
    {
        return 0;
    }

    /// <inheritdoc />
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
    /// Initialise the main Window <see cref="Hint"/>s. Some Hints may be set
    /// elsewhere, but this is where most are initialised.
    /// </summary>
    /// <param name="config"> The current <see cref="DesktopGLApplicationConfiguration"/>. </param>
    private void SetWindowHints( DesktopGLApplicationConfiguration config )
    {
        Logger.CheckPoint();

        Glfw.DefaultWindowHints();

        Glfw.WindowHint( Hint.Visible, false );
        Glfw.WindowHint( Hint.Resizable, config.WindowResizable );
        Glfw.WindowHint( Hint.Maximized, config.WindowMaximized );
        Glfw.WindowHint( Hint.AutoIconify, config.AutoIconify );
        Glfw.WindowHint( Hint.Decorated, config.WindowDecorated );

        Glfw.WindowHint( Hint.RedBits, config.Red );
        Glfw.WindowHint( Hint.GreenBits, config.Green );
        Glfw.WindowHint( Hint.BlueBits, config.Blue );
        Glfw.WindowHint( Hint.AlphaBits, config.Alpha );
        Glfw.WindowHint( Hint.StencilBits, config.Stencil );
        Glfw.WindowHint( Hint.DepthBits, config.Depth );
        Glfw.WindowHint( Hint.Samples, config.Samples );

        Glfw.WindowHint( Hint.ContextVersionMajor, config.GLContextMajorVersion );
        Glfw.WindowHint( Hint.ContextVersionMinor, config.GLContextMinorVersion );

        Glfw.WindowHint( Hint.ClientAPI, ClientAPI.OpenGLAPI );

        if ( Platform.IsMac )
        {
            // hints mandatory on OS X for GL 3.2+ context creation, but fail on Windows if the
            // WGL_ARB_create_context extension is not available
            // see: http://www.glfw.org/docs/latest/compat.html

            Glfw.WindowHint( Hint.OpenGLForwardCompat, true );
        }

        Logger.CheckPoint();

        if ( config.TransparentFramebuffer )
        {
            Glfw.WindowHint( Hint.TransparentFramebuffer, true );
        }

        if ( config.Debug )
        {
            Glfw.WindowHint( Hint.OpenGLDebugContext, true );
        }

        Logger.Debug( " - Finished" );
    }

    /// <summary>
    /// Returns <b>true</b> if <b><i>F</i></b>rame<b><i>B</i></b>uffer <b><i>O</i></b>bjects
    /// are supported. 
    /// </summary>
    private bool SupportsFBO()
    {
        GdxRuntimeException.ThrowIfNull( GLVersion );

        // FBO is in core since OpenGL 3.0,
        // see https://www.opengl.org/wiki/Framebuffer_Object
        return GLVersion!.IsVersionEqualToOrHigher( 3, 0 )
            || Glfw.ExtensionSupported( "GL_EXT_framebuffer_object" )
            || Glfw.ExtensionSupported( "GL_ARB_framebuffer_object" );
    }

    // ------------------------------------------------------------------------

    public void Dispose()
    {
    }

    // ------------------------------------------------------------------------

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

//    #region GLDebug specific

//    [PublicAPI]
//    public struct Gldms
//    {
//        public int GL43;
//        public int Khr;
//        public int Arb;
//        public int Amd;

//        public Gldms( int gl43, int khr, int arb, int amd )
//        {
//            this.GL43 = gl43;
//            this.Khr  = khr;
//            this.Arb  = arb;
//            this.Amd  = amd;
//        }
//    }

//    public record GLDebugMessageSeverity
//    {
//        public Gldms High = new( GL43.GL_DEBUG_SEVERITY_HIGH,
//                                 KHRDebug.GL_DEBUG_SEVERITY_HIGH,
//                                 ARBDebugOutput.GL_DEBUG_SEVERITY_HIGH_ARB,
//                                 AMDDebugOutput.GL_DEBUG_SEVERITY_HIGH_AMD );
//
//        public Gldms Medium = new( GL43.GL_DEBUG_SEVERITY_MEDIUM,
//                                   KHRDebug.GL_DEBUG_SEVERITY_MEDIUM,
//                                   ARBDebugOutput.GL_DEBUG_SEVERITY_MEDIUM_ARB,
//                                   AMDDebugOutput.GL_DEBUG_SEVERITY_MEDIUM_AMD );
//        
//        public Gldms Low = new( GL43.GL_DEBUG_SEVERITY_LOW,
//                                KHRDebug.GL_DEBUG_SEVERITY_LOW,
//                                ARBDebugOutput.GL_DEBUG_SEVERITY_LOW_ARB,
//                                AMDDebugOutput.GL_DEBUG_SEVERITY_LOW_AMD );
//        
//        public Gldms Notification = new( IGL.GL_DEBUG_SEVERITY_NOTIFICATION,
//                                         KHRDebug.GL_DEBUG_SEVERITY_NOTIFICATION,
//                                         -1,
//                                         -1 );
//    }

//    /// <summary>
//    /// Enables or disables GL debug messages for the specified severity level.
//    /// Returns false if the severity level could not be set (e.g. the NOTIFICATION
//    /// level is not supported by the ARB and AMD extensions).
//    /// </summary>
//    public static bool SetGLDebugMessageControl( GLDebugMessageSeverity severity, bool enabled )
//    {
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
//        return false;
//    }

//    #endregion GLDebug specific
}