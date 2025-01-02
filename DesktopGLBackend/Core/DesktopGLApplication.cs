// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using System.Diagnostics.CodeAnalysis;

using LughSharp.Lugh.Core;
using LughSharp.Lugh.Graphics.GLUtils;
using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Graphics.Profiling;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Collections;
using LughSharp.Lugh.Utils.Exceptions;

using DesktopGLBackend.Audio;
using DesktopGLBackend.Audio.Mock;
using DesktopGLBackend.Files;
using DesktopGLBackend.Input;
using DesktopGLBackend.Utils;
using DesktopGLBackend.Window;

namespace DesktopGLBackend.Core;

/// <summary>
/// Creates, and manages, an application to for Windows OpenGL backends. 
/// </summary>
[PublicAPI]
public class DesktopGLApplication : IDesktopGLApplicationBase, IDisposable
{
    #region public properties

    public DesktopGLApplicationConfiguration? Config             { get; set; }
    public Dictionary< string, IPreferences > Preferences        { get; set; } = [ ];
    public List< DesktopGLWindow >            Windows            { get; set; } = [ ];
    public List< IRunnable.Runnable >         Runnables          { get; set; } = [ ];
    public List< IRunnable.Runnable >         ExecutedRunnables  { get; set; } = [ ];
    public List< ILifecycleListener >         LifecycleListeners { get; set; } = [ ];

    public IClipboard?   Clipboard  { get; set; }
    public IGLAudio?     Audio      { get; set; }
    public INet          Network    { get; set; }
    public IFiles        Files      { get; set; }
    public GLVersion?    GLVersion  { get; set; }
    public OpenGLProfile OGLProfile { get; set; }

    #endregion public properties

    // ========================================================================

    private const int FR_UNINITIALISED = -2;

    // ========================================================================

    private static   GlfwErrorCallback? _errorCallback;
    private readonly Sync?              _sync;
    private          DesktopGLWindow?   _currentWindow;
    private          bool               _running         = true;
    private          bool               _glfwInitialised = false;
    private          IPreferences       _prefs;

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
        // This MUST be the first call, so that the Logger and GdxApi.App global are
        // initialised correctly.
        GdxApi.Initialise( this );

        _prefs = GetPreferences( "desktopgl.lugh.engine.preferences" );
        _prefs.PutBool( "profiling", config.GLProfilingEnabled );
        _prefs.Flush();

        // This is set very early in this constructor to avoid any null references.
        if ( _prefs.GetBool( "profiling" ) )
        {
            Logger.Debug( "Profiling enabled" );
            GdxApi.Bindings = new GLInterceptor( new GLProfiler() );
        }
        else
        {
            Logger.Debug( "Profiling disabled" );
            GdxApi.Bindings = new GLBindings();
        }

        // Config.Title becomes the name of the ApplicationListener if it has no value at this point.
        Config       =   DesktopGLApplicationConfiguration.Copy( config );
        Config.Title ??= listener.GetType().Name;

        Config.SetOpenGLEmulation( DesktopGLApplicationConfiguration.GLEmulationType.GL30,
                                   GLUtils.DEFAULT_GL_MAJOR,
                                   GLUtils.DEFAULT_GL_MINOR );

        // Initialise the global environment shortcuts. 'GdxApi.Audio', 'GdxApi.Files', and 'GdxApi.Net'
        // are instances of classes implementing IAudio, IFiles, and INet resprectively, and are used to
        // access LughSharp members 'Audio', 'Files', and 'Network' are instances of classes which extend
        // the aforementioned// classes, and are used in backend code only.
        // Note: GdxApi.Graphics is set later, during window creation.
        Audio     = CreateAudio( Config );
        Files     = new DesktopGLFiles();
        Network   = new DesktopGLNet( Config );
        Clipboard = new DesktopGLClipboard();

        GdxApi.Audio = Audio;
        GdxApi.Files = Files;
        GdxApi.Net   = Network;

        _sync = new Sync();

        InitialiseGlfw();

        Windows.Add( CreateWindow( Config, listener, 0 ) );
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// The entry point for running code using this framework. At this point at least one window
    /// will have been created, Glfw will have been set up, and the framework properly initialised.
    /// <para>
    /// This passes control to <see cref="Loop()"/> and stays there until the app is finished. At
    /// this point <see cref="CleanupWindows"/> is called, followed by <see cref="Cleanup"/>.
    /// </para>
    /// </summary>
    public void Run()
    {
        try
        {
            Loop();
            CleanupWindows();
        }
        catch ( System.Exception e )
        {
            throw ( e is SystemException ) ? e : new GdxRuntimeException( e );
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
        Logger.Debug( "Entering framework loop" );

        List< DesktopGLWindow > closedWindows = [ ];

        while ( _running && ( Windows.Count > 0 ) )
        {
            Logger.Checkpoint();
            Glfw.PollEvents();
            Logger.Checkpoint();

            var haveWindowsRendered = false;
            var targetFramerate     = FR_UNINITIALISED;

            closedWindows.Clear();

            // Update active windows. SwapBuffers is called in window.Update().
            foreach ( var window in Windows )
            {
                Logger.Checkpoint();
                window.MakeCurrent();

                _currentWindow = window;

                if ( targetFramerate == FR_UNINITIALISED )
                {
                    targetFramerate = window.AppConfig.ForegroundFPS;
                }

                Logger.Checkpoint();
                lock ( LifecycleListeners )
                {
                    haveWindowsRendered |= window.Update();
                }
                Logger.Checkpoint();

                if ( window.ShouldClose() )
                {
                    closedWindows.Add( window );
                }
                Logger.Checkpoint();
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

        Logger.Debug( "Ending framework loop" );
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
    public DesktopGLWindow NewWindow( IApplicationListener listener, DesktopGLWindowConfiguration windowConfig )
    {
        GdxRuntimeException.ThrowIfNull( Config );

        Config.SetWindowConfiguration( windowConfig );

        return CreateWindow( Config, listener, 0 );
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
        var dglWindow = new DesktopGLWindow( listener, config, this );

        if ( sharedContext == 0 )
        {
            // the main window is created immediately
            dglWindow = CreateWindow( dglWindow, config, 0 );
        }
        else
        {
            // creation of additional windows is deferred to avoid GL context trouble
            PostRunnable( () =>
            {
                dglWindow = CreateWindow( dglWindow, config, sharedContext );
                Windows.Add( dglWindow );
            } );
        }

        return dglWindow;
    }

    /// <summary>
    /// </summary>
    /// <param name="dglWindow"></param>
    /// <param name="config"></param>
    /// <param name="sharedContext"></param>
    public DesktopGLWindow CreateWindow( DesktopGLWindow? dglWindow,
                                         DesktopGLApplicationConfiguration config,
                                         long sharedContext )
    {
        ArgumentNullException.ThrowIfNull( dglWindow );

        var windowHandle = CreateGlfwWindow( config, sharedContext );

        dglWindow.Create( windowHandle );
        dglWindow.SetVisible( config.InitialVisibility );

        for ( var i = 0; i < 2; i++ )
        {
            GdxApi.Bindings.ClearColor( config.InitialBackgroundColor.R,
                                        config.InitialBackgroundColor.G,
                                        config.InitialBackgroundColor.B,
                                        config.InitialBackgroundColor.A );

            GdxApi.Bindings.Clear( IGL.GL_COLOR_BUFFER_BIT );
            Glfw.SwapBuffers( windowHandle );
        }

        // the call above to CreateGlfwWindow switches the OpenGL context to the
        // newly created window, ensure that the invariant "currentWindow is the
        // window with the current active OpenGL context" holds
        _currentWindow?.MakeCurrent();

        return dglWindow;
    }

    /// <summary>
    /// </summary>
    /// <param name="config"></param>
    /// <param name="sharedContextWindow"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private GLFW.Window CreateGlfwWindow( DesktopGLApplicationConfiguration config, long sharedContextWindow )
    {
        SetWindowHints( config );

        GLFW.Window? windowHandle;

        if ( config.FullscreenMode != null )
        {
            // Create a fullscreen window

            Glfw.WindowHint( WindowHint.RefreshRate, config.FullscreenMode.RefreshRate );

            windowHandle = Glfw.CreateWindow( config.FullscreenMode.Width,
                                              config.FullscreenMode.Height,
                                              config.Title ?? "",
                                              config.FullscreenMode.MonitorHandle,
                                              GLFW.Window.NULL );
        }
        else
        {
            // Create a 'windowed' window

            windowHandle = Glfw.CreateWindow( config.WindowWidth,
                                              config.WindowHeight,
                                              config.Title ?? "",
                                              GLFW.Monitor.NULL,
                                              GLFW.Window.NULL );
        }

        if ( windowHandle.Equals( null ) )
        {
            throw new NullReferenceException( "Failed to create window!" );
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

                var monitorHandle = Glfw.GetPrimaryMonitor();

                if ( config is { WindowMaximized: true, MaximizedMonitor: not null } )
                {
                    monitorHandle = config.MaximizedMonitor.MonitorHandle;
                }

                Glfw.GetMonitorWorkarea( monitorHandle, out var areaX, out var areaY, out var areaW, out var areaH );

                Glfw.SetWindowPos( windowHandle,
                                   ( areaX + ( areaW / 2 ) ) - ( windowWidth / 2 ),
                                   ( areaY + ( areaH / 2 ) ) - ( windowHeight / 2 ) );
            }
            else
            {
                Glfw.SetWindowPos( windowHandle, config.WindowX, config.WindowY );
            }

            if ( config.WindowMaximized )
            {
                Glfw.MaximizeWindow( windowHandle );
            }
        }

        if ( config.WindowIconPaths != null )
        {
            DesktopGLWindow.SetIcon( windowHandle, config.WindowIconPaths, config.WindowIconFileType );
        }

        Glfw.MakeContextCurrent( windowHandle );

        Glfw.SwapInterval( config.VSyncEnabled ? 1 : 0 );

        GLUtils.CreateCapabilities(); // Needed??

        InitGLVersion( windowHandle );

        if ( config.Debug )
        {
            GdxApi.Bindings.Enable( IGL.GL_DEBUG_OUTPUT );

            unsafe
            {
                GdxApi.Bindings.DebugMessageCallback( GdxApi.Bindings.MessageCallback, null );
            }
            
//            GlDebugCallback = Glfw.DebugMessageCallback( config.debugStream );
//            SetGLDebugMessageControl( GLDebugMessageSeverity.Notification, false );
        }

        return windowHandle;
    }
    
    #endregion window creation handlers

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <param name="windowHandle"></param>
    private void InitGLVersion( GLFW.Window windowHandle )
    {
        OGLProfile = GLUtils.DEFAULT_OPENGL_PROFILE;

        Glfw.GetVersion( out var glMajor, out var glMinor, out var revision );

        Glfw.WindowHint( WindowHint.ClientAPI, GLUtils.DEFAULT_CLIENT_API );
        Glfw.WindowHint( WindowHint.OpenGLProfile, OGLProfile );
        Glfw.WindowHint( WindowHint.ContextVersionMajor, glMajor );
        Glfw.WindowHint( WindowHint.ContextVersionMinor, glMinor );

        Logger.Debug( $"Glfw: {glMajor}.{glMinor}.{revision} : glProfile: {OGLProfile}" );

        Glfw.MakeContextCurrent( windowHandle );

        Logger.Debug( $"OGLVersion: {GdxApi.Bindings.GetOpenGLVersion().major}.{GdxApi.Bindings.GetOpenGLVersion().minor}" );

        GLVersion = new GLVersion( LughSharp.Lugh.Core.Platform.ApplicationType.WindowsGL );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public void InitialiseGlfw()
    {
        try
        {
            if ( !_glfwInitialised )
            {
                DesktopGLNativesLoader.Load();

                ErrorCallback();

                Glfw.SetErrorCallback( _errorCallback );
                Glfw.InitHint( InitHint.JoystickHatButtons, false );

                if ( !Glfw.Init() )
                {
                    Glfw.GetError( out var error );

                    Logger.Debug( $"Failed to initialise Glfw: {error}" );

                    System.Environment.Exit( 1 );
                }

                Logger.Debug( $"Success: Glfw Version: {Glfw.GetVersionString()}", true );

                _glfwInitialised = true;
            }
        }
        catch ( System.Exception e )
        {
            throw new ApplicationException( $"Failure in InitialiseGLFW() : {e}" );
        }
    }

    private static void ErrorCallback()
    {
        _errorCallback = ( error, description ) =>
        {
            Logger.Error( $"ErrorCode: {error}, {description}" );

            if ( error == ErrorCode.InvalidEnum )
            {
                
            }
        };
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
            catch ( System.Exception e )
            {
                Logger.Debug( $"Couldn't initialize audio, disabling audio: {e}" );
                audio = new MockAudio();
            }
        }
        else
        {
            Logger.Debug( "Audio is disabled in Config, using MockAudio instead." );

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

        Logger.Debug( $"Creating new Preferences file: {name}" );

        IPreferences prefs = new DesktopGLPreferences( name );

        Preferences.Put( name, prefs );

        return prefs;
    }

    /// <inheritdoc />
    public LughSharp.Lugh.Core.Platform.ApplicationType AppType
    {
        get => LughSharp.Lugh.Core.Platform.ApplicationType.WindowsGL;
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
    /// Schedule an exit from the application. On android, this will cause a call to
    /// Pause() and Dispose() at the next opportunity. It will not immediately finish
    /// your application. On iOS this should be avoided in production as it breaks
    /// Apples guidelines
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

//        if ( config.GLEmulation == DesktopGLApplicationConfiguration.GLEmulationType.GL30
//             || config.GLEmulation == DesktopGLApplicationConfiguration.GLEmulationType.GL31
//             || config.GLEmulation == DesktopGLApplicationConfiguration.GLEmulationType.GL32 )
        {
            Glfw.WindowHint( WindowHint.ContextVersionMajor, config.GLContextMajorVersion );
            Glfw.WindowHint( WindowHint.ContextVersionMinor, config.GLContextMinorVersion );

//            if ( Platform.IsMac )
            {
                // hints mandatory on OS X for GL 3.2+ context creation, but fail on Windows if the
                // WGL_ARB_create_context extension is not available
                // see: http://www.glfw.org/docs/latest/compat.html
                Glfw.WindowHint( WindowHint.OpenGLForwardCompat, true );
                Glfw.WindowHint( WindowHint.OpenGLProfile, GLUtils.DEFAULT_OPENGL_PROFILE );
            }
        }

        Glfw.WindowHint( WindowHint.DoubleBuffer, true );

        if ( config.TransparentFramebuffer )
        {
            Glfw.WindowHint( WindowHint.TransparentFramebuffer, true );
        }

        if ( config.Debug )
        {
            Logger.Debug( "Setting OpenGL Debug Context" );

            Glfw.WindowHint( WindowHint.OpenGLDebugContext, true );
        }
    }

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

    // ========================================================================
    // ========================================================================

    ~DesktopGLApplication()
    {
        Dispose( false );
    }

    /// <inheritdoc/>
    /// <remarks> Calls Dispose(bool) with <b>true</b>. </remarks>
    /// <remarks> GC.SuppressFinalize is called in Dispose(bool). </remarks>
    [SuppressMessage( "Usage", "CA1816:Dispose methods should call SuppressFinalize" )]
    public void Dispose()
    {
        Dispose( true );
    }

    public void Dispose( bool disposing )
    {
        if ( disposing )
        {
            // Release managed resources here
        }

        GC.SuppressFinalize( this );
    }
}