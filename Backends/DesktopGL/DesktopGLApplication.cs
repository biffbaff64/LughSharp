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
using LibGDXSharp.Utils.Collections;

using Sync = LibGDXSharp.Backends.Desktop.Sync;

namespace LibGDXSharp;

public class DesktopGLApplication : IDesktopGLApplicationBase
{

    private const string TAG = "GLApplication";

    private static   GLFW.ErrorCallback? _errorCallback = null;
    private readonly Sync?               _sync          = null;
    private volatile DesktopGLWindow?    _currentWindow = null;
    private          bool                _running       = true;

    // ------------------------------------------------------------------------

    public DesktopGLApplication( IApplicationListener listener,
                                 DesktopGLApplicationConfiguration config )
    {
        InitialiseGL();

        ApplicationLogger = new DesktopGLApplicationLogger();

        config.Title ??= listener.GetType().Name;

        Config = config = DesktopGLApplicationConfiguration.Copy( config );

        Gdx.App = this;

        if ( !config.DisableAudio )
        {
            try
            {
                Audio = CreateAudio( config );
            }
            catch ( Exception e )
            {
                Log( TAG, "Couldn't initialize audio, disabling audio", e );

                Audio = new MockAudio();
            }
        }
        else
        {
            Audio = new MockAudio();
        }

        Files     = CreateFiles();
        Net       = new DesktopGLNet( config );
        Clipboard = new DesktopGLClipboard();
        _sync     = new Sync();

        Gdx.Audio = Audio;
        Gdx.Files = Files;
        Gdx.Net   = Net;

        Windows.Add( CreateWindow( config, listener, 0 ) );

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

    public void PostRunnable( Runnable runnable )
    {
        lock ( Runnables )
        {
            Runnables.Add( runnable );
        }
    }

    public IGLAudio CreateAudio( DesktopGLApplicationConfiguration config ) => new OpenALAudio( config.AudioDeviceSimultaneousSources,
                                                                                                config.AudioDeviceBufferCount,
                                                                                                config.AudioDeviceBufferSize );

    public IDesktopGLInput CreateInput( DesktopGLWindow window ) => new DefaultDesktopGLInput( window );

    public int GetVersion() => 0;

    public void Exit() => _running = false;

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

    /// <summary>
    ///     Framework Main Loop
    /// </summary>
    protected void Loop()
    {
        List< DesktopGLWindow > closedWindows = new();

        while ( _running && ( Windows.Count > 0 ) )
        {
            Audio?.Update();

            var haveWindowsRendered = false;
            var targetFramerate     = -2;

            closedWindows.Clear();

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

            Glfw.PollEvents();

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
                    if ( !window.Graphics.ContinuousRendering )
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

    /// <summary>
    ///     Cleans up, and disposes of, any windows that have been closed.
    /// </summary>
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

        Glfw.Terminate();
    }

    /// <summary>
    ///     Creates a new <see cref="DesktopGLWindow" /> using the provided listener and
    ///     <see cref="DesktopGLWindowConfiguration" />.
    ///     <para>
    ///         This function only just instantiates a <see cref="DesktopGLWindow" /> and
    ///         returns immediately. The actual window creation is postponed with
    ///         <see cref="DesktopGLApplication.PostRunnable(Runnable)" /> until after all
    ///         existing windows are updated.
    ///     </para>
    /// </summary>
    public DesktopGLWindow NewWindow( IApplicationListener listener, DesktopGLWindowConfiguration config )
    {
        GdxRuntimeException.ThrowIfNull( Config );

        DesktopGLApplicationConfiguration appConfig = DesktopGLApplicationConfiguration.Copy( Config );

        appConfig.SetWindowConfiguration( config );

        return CreateWindow( appConfig, listener, 0 );
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

        Glfw.MakeContextCurrent( window.GlfwWindow );

        return window;
    }

    public void CreateWindow( DesktopGLWindow window,
                              DesktopGLApplicationConfiguration config,
                              long sharedContext )
    {
        Window windowHandle = CreateGLFWWindow( config, window.GlfwWindow );

        window.Create( windowHandle );
        window.SetVisible( config.InitialVisible );

        for ( var i = 0; i < 2; i++ )
        {
            Gl.ClearColor( config.InitialBackgroundColor.R,
                           config.InitialBackgroundColor.G,
                           config.InitialBackgroundColor.B,
                           config.InitialBackgroundColor.A );

            Gl.Clear( ClearBufferMask.ColorBufferBit );
            Glfw.SwapBuffers( windowHandle );
        }
    }

    private GLFW.Window CreateGLFWWindow( DesktopGLApplicationConfiguration appConfig, Window sharedContextWindow )
    {
        Glfw.DefaultWindowHints();

        Glfw.WindowHint( Hint.Visible, false );
        Glfw.WindowHint( Hint.Resizable, appConfig.WindowResizable );
        Glfw.WindowHint( Hint.Maximized, appConfig.WindowMaximized );
        Glfw.WindowHint( Hint.AutoIconify, appConfig.AutoIconify );

        Glfw.WindowHint( Hint.RedBits, appConfig.R );
        Glfw.WindowHint( Hint.GreenBits, appConfig.G );
        Glfw.WindowHint( Hint.BlueBits, appConfig.B );
        Glfw.WindowHint( Hint.AlphaBits, appConfig.A );
        Glfw.WindowHint( Hint.StencilBits, appConfig.Stencil );
        Glfw.WindowHint( Hint.DepthBits, appConfig.Depth );
        Glfw.WindowHint( Hint.Samples, appConfig.Samples );

        if ( appConfig.UseGL30 )
        {
            Glfw.WindowHint( Hint.ContextVersionMajor, appConfig.Gles30ContextMajorVersion );
            Glfw.WindowHint( Hint.ContextVersionMinor, appConfig.Gles30ContextMinorVersion );

            if ( SharedLibraryLoader.IsMac )
            {
                // hints mandatory on OS X for GL 3.2+ context creation, but fail on Windows if the
                // WGL_ARB_create_context extension is not available
                // see: http://www.glfw.org/docs/latest/compat.html

                Glfw.WindowHint( Hint.OpenglForwardCompatible, true );
                Glfw.WindowHint( Hint.OpenglProfile, ( int )Gl.CONTEXT_CORE_PROFILE_BIT );
            }
        }

        if ( appConfig.TransparentFramebuffer )
        {
            Glfw.WindowHint( Hint.TransparentFramebuffer, true );
        }

        if ( appConfig.Debug )
        {
            Glfw.WindowHint( Hint.OpenglDebugContext, true );
        }

        GLFW.Window windowHandle;

        if ( appConfig.FullscreenMode != null )
        {
            Glfw.WindowHint( Hint.RefreshRate, appConfig.FullscreenMode.RefreshRate );

            windowHandle = Glfw.CreateWindow( appConfig.FullscreenMode.Width,
                                              appConfig.FullscreenMode.Height,
                                              appConfig.Title ?? "",
                                              appConfig.FullscreenMode.MonitorHandle,
                                              sharedContextWindow );
        }
        else
        {
            Glfw.WindowHint( Hint.Decorated, appConfig.WindowDecorated ? Gl.TRUE : Gl.FALSE );

            windowHandle = Glfw.CreateWindow( appConfig.WindowWidth,
                                              appConfig.WindowHeight,
                                              appConfig.Title ?? "",
                                              Monitor.None,
                                              Window.None );
        }

        if ( windowHandle == Window.None )
        {
            throw new GdxRuntimeException( "Couldn't create window" );
        }

        _currentWindow?.SetSizeLimits( appConfig.WindowMinWidth,
                                       appConfig.WindowMinHeight,
                                       appConfig.WindowMaxWidth,
                                       appConfig.WindowMaxHeight );

        if ( appConfig.FullscreenMode == null )
        {
            if ( appConfig is { WindowX: -1, WindowY: -1 } )
            {
                var windowWidth  = Math.Max( appConfig.WindowWidth, appConfig.WindowMinWidth );
                var windowHeight = Math.Max( appConfig.WindowHeight, appConfig.WindowMinHeight );

                if ( appConfig.WindowMaxWidth > -1 )
                {
                    windowWidth = Math.Min( windowWidth, appConfig.WindowMaxWidth );
                }

                if ( appConfig.WindowMaxHeight > -1 )
                {
                    windowHeight = Math.Min( windowHeight, appConfig.WindowMaxHeight );
                }

                GLFW.Monitor monitorHandle = Glfw.PrimaryMonitor;

                if ( appConfig is { WindowMaximized: true, MaximizedMonitor: not null } )
                {
                    monitorHandle = appConfig.MaximizedMonitor.MonitorHandle;
                }

                Glfw.GetMonitorWorkArea( monitorHandle.UserPointer,
                                         out var areaXPos,
                                         out var areaYPos,
                                         out var areaWidth,
                                         out var areaHeight );

                Glfw.SetWindowPosition( windowHandle,
                                        ( areaXPos + ( areaWidth / 2 ) ) - ( windowWidth / 2 ),
                                        ( areaYPos + ( areaHeight / 2 ) ) - ( windowHeight / 2 ) );
            }
            else
            {
                Glfw.SetWindowPosition( windowHandle, appConfig.WindowX, appConfig.WindowY );
            }

            if ( appConfig.WindowMaximized )
            {
                Glfw.MaximizeWindow( windowHandle );
            }
        }

        if ( appConfig.WindowIconPaths != null )
        {
            _currentWindow?.SetIcon( windowHandle, appConfig.WindowIconPaths, appConfig.WindowIconFileType );
        }

        Glfw.MakeContextCurrent( windowHandle );
        Glfw.SwapInterval( appConfig.VSyncEnabled ? 1 : 0 );

        //TODO: What do I do here????
//        Glfw.CreateCapabilities();

        InitiateGL();

        if ( !GLVersion!.IsVersionEqualToOrHigher( 2, 0 ) )
        {
            throw new GdxRuntimeException( $"OpenGL 2.0 or higher with the FBO extension is "
                                         + $"required. OpenGL version: {Gl.GetString( StringName.Version )}"
                                         + $"\n{GLVersion?.DebugVersionString()}" );
        }

        if ( !SupportsFBO() )
        {
            throw new GdxRuntimeException( $"OpenGL 2.0 or higher with the FBO extension is "
                                         + $"required. OpenGL version: {Gl.GetString( StringName.Version )}, "
                                         + $"FBO extension: false\n{GLVersion?.DebugVersionString()}" );
        }

        if ( appConfig.Debug )
        {
//            GlDebugCallback = GLFW.DebugMessageCallback( config.debugStream );
//            SetGLDebugMessageControl( GLDebugMessageSeverity.Notification, false );
        }

        return windowHandle;
    }

    private bool SupportsFBO()
    {
        GdxRuntimeException.ThrowIfNull( GLVersion );

        // FBO is in core since OpenGL 3.0,
        // see https://www.opengl.org/wiki/Framebuffer_Object
        return GLVersion.IsVersionEqualToOrHigher( 3, 0 )
            || Glfw.GetExtensionSupported( "GL_EXT_framebuffer_object" )
            || Glfw.GetExtensionSupported( "GL_ARB_framebuffer_object" );
    }

    public static void InitialiseGL()
    {
        if ( _errorCallback == null )
        {
            DesktopGLNativesLoader.Load();

            Glfw.SetErrorCallback( _errorCallback );
            Glfw.InitHint( Hint.JoystickHatButtons, false );

            if ( !Glfw.Init() )
            {
                throw new GdxRuntimeException( "Unable to initialise Glfw!" );
            }
        }
    }

    protected IFiles CreateFiles() => new DesktopGLFiles();

    /// <summary>
    /// </summary>
    private void InitiateGL()
    {
        Glfw.GetVersion( out var major, out var minor, out var revision );

        GLVersion = new GLVersion( IApplication.ApplicationType.Desktop,
                                   $"{major}.{minor}.{revision}",
                                   Gdx.GL20.GLGetString( IGL20.GL_VENDOR ),
                                   Gdx.GL20.GLGetString( IGL20.GL_RENDERER ) );
    }

    #region public properties

    public DesktopGLApplicationConfiguration?  Config             { get; set; }
    public List< DesktopGLWindow >             Windows            { get; set; } = new();
    public Dictionary< string, IPreferences >? Preferences        { get; set; }
    public List< Runnable >                    Runnables          { get; set; } = new();
    public List< Runnable >                    ExecutedRunnables  { get; set; } = new();
    public List< ILifecycleListener >          LifecycleListeners { get; set; } = new();
    public DesktopGLApplicationLogger?         ApplicationLogger  { get; set; }
    public int                                 LogLevel           { get; set; }
    public IClipboard?                         Clipboard          { get; set; }
    public GLVersion?                          GLVersion          { get; set; }

    public IGraphics?            Graphics            => _currentWindow?.Graphics;
    public IApplicationListener? ApplicationListener => _currentWindow?.Listener;
    public IInput?               Input               => _currentWindow?.Input;
    public IGLAudio?             Audio               { get; set; } = null;
    public INet                  Net                 { get; set; }
    public IFiles                Files               { get; set; }

    public IApplication.ApplicationType AppType
    {
        get => IApplication.ApplicationType.Desktop;
        set { }
    }

    #endregion public properties

    #region debug logging

    public void Debug( string tag, string message )
    {
        if ( LogLevel >= IApplication.LOG_DEBUG )
        {
            ApplicationLogger?.Debug( tag, message );
        }
    }

    public void Debug( string tag, string message, Exception exception )
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

    public void Log( string tag, string message, Exception exception )
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

    public void Error( string tag, string message, Exception exception )
    {
        if ( LogLevel >= IApplication.LOG_ERROR )
        {
            ApplicationLogger?.Error( tag, message, exception );
        }
    }

    #endregion debug logging

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region GLDebug specific

    //TODO: Unfinished, see GLDebugMessageSeverity below

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
    ///     Enables or disables GL debug messages for the specified severity level.
    ///     Returns false if the severity level could not be set (e.g. the NOTIFICATION
    ///     level is not supported by the ARB and AMD extensions).
    /// </summary>
    /// <seealso cref="DesktopGLApplicationConfiguration.EnableGLDebugOutput(bool, StreamWriter)" />
    public static bool SetGLDebugMessageControl( GLDebugMessageSeverity severity, bool enabled ) =>

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
        false;

    #endregion GLDebug specific

}
