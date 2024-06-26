﻿// ///////////////////////////////////////////////////////////////////////////////
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

namespace LughSharp.Backends.DesktopGL;

[PublicAPI]
public class DesktopGLApplication : IDesktopGLApplicationBase
{
    #region public properties

    public DesktopGLApplicationConfiguration?  Config             { get; set; }
    public List< DesktopGLWindow >             Windows            { get; set; } = new();
    public Dictionary< string, IPreferences >? Preferences        { get; set; }
    public List< IRunnable.Runnable >          Runnables          { get; set; } = new();
    public List< IRunnable.Runnable >          ExecutedRunnables  { get; set; } = new();
    public List< ILifecycleListener >          LifecycleListeners { get; set; } = new();

    public int         LogLevel  { get; set; }
    public IClipboard? Clipboard { get; set; }
    public GLVersion?  GLVersion { get; set; }
    public IGLAudio?   Audio     { get; set; } = null;
    public INet        Network   { get; set; }
    public IFiles      Files     { get; set; }

    public IGraphics?            Graphics            => _currentWindow?.Graphics;
    public IApplicationListener? ApplicationListener => _currentWindow?.Listener;
    public IInput?               Input               => _currentWindow?.Input;

    public Platform.ApplicationType AppType
    {
        get => Platform.ApplicationType.WindowsGL;
        set { }
    }

    #endregion public properties

    // ------------------------------------------------------------------------

    private const int FR_UNINITIALISED = -2;

    private static   GlfwErrorCallback? _errorCallback = null;
    private readonly Sync?              _sync          = null;
    private volatile DesktopGLWindow?   _currentWindow = null;
    private          bool               _running       = true;

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
        InitialiseGL();

        config.Title ??= listener.GetType().Name;

        Config = config = DesktopGLApplicationConfiguration.Copy( config );

        Gdx.App = this;

        Audio     = CreateAudio( config );
        Files     = new DesktopGLFiles();
        Network   = new DesktopGLNet( config );
        Clipboard = new DesktopGLClipboard();
        _sync     = new Sync();

        Gdx.Audio = Audio;
        Gdx.Files = Files;
        Gdx.Net   = Network;

        Windows.Add( CreateWindow( config, listener, 0 ) );

        Run();
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

                _currentWindow = window;

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

            foreach ( var closedWindow in closedWindows )
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
        if ( Preferences!.ContainsKey( name ) )
        {
            return Preferences.Get( name );
        }

        IPreferences prefs = new DesktopGLPreferences( name );

        Preferences.Put( name, prefs );

        return prefs;
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

    /// <summary>
    /// </summary>
    /// <param name="window"></param>
    /// <param name="config"></param>
    /// <param name="sharedContext"></param>
    public void CreateWindow( DesktopGLWindow window, DesktopGLApplicationConfiguration config, long sharedContext )
    {
        ArgumentNullException.ThrowIfNull( window );
        ArgumentNullException.ThrowIfNull( window.GlfwWindow );

        var windowHandle = CreateGLFWWindow( config, window.GlfwWindow );

        window.Create( windowHandle );
        window.SetVisible( config.InitialVisible );

        for ( var i = 0; i < 2; i++ )
        {
            Gdx.GL.glClearColor( config.InitialBackgroundColor.R,
                                 config.InitialBackgroundColor.G,
                                 config.InitialBackgroundColor.B,
                                 config.InitialBackgroundColor.A );

            Gdx.GL.glClear( IGL.GL_COLOR_BUFFER_BIT );
            Glfw.SwapBuffers( windowHandle );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="appConfig"></param>
    /// <param name="sharedContextWindow"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private GLFW.Window CreateGLFWWindow( DesktopGLApplicationConfiguration appConfig,
                                          GLFW.Window sharedContextWindow )
    {
        SetWindowHints( appConfig );

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
            Glfw.WindowHint( Hint.Decorated, appConfig.WindowDecorated );

            windowHandle = Glfw.CreateWindow( appConfig.WindowWidth,
                                              appConfig.WindowHeight,
                                              appConfig.Title ?? "",
                                              Monitor.NULL,
                                              GLFW.Window.NULL );
        }

        if ( windowHandle == GLFW.Window.NULL )
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

                var monitorHandle = Glfw.GetPrimaryMonitor();

                if ( appConfig is { WindowMaximized: true, MaximizedMonitor: not null } )
                {
                    monitorHandle = appConfig.MaximizedMonitor.MonitorHandle;
                }

                Glfw.GetMonitorWorkarea( monitorHandle,
                                         out var areaXPos,
                                         out var areaYPos,
                                         out var areaWidth,
                                         out var areaHeight );

                Glfw.SetWindowPos( windowHandle,
                                   ( areaXPos + ( areaWidth / 2 ) ) - ( windowWidth / 2 ),
                                   ( areaYPos + ( areaHeight / 2 ) ) - ( windowHeight / 2 ) );
            }
            else
            {
                Glfw.SetWindowPos( windowHandle, appConfig.WindowX, appConfig.WindowY );
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

        InitGLVersion();

//        if ( appConfig.Debug )
//        {
//            GlDebugCallback = GLFW.DebugMessageCallback( config.debugStream );
//            SetGLDebugMessageControl( GLDebugMessageSeverity.Notification, false );
//        }

        return windowHandle;
    }

    /// <summary>
    /// Initialise the main Window <see cref="Hint"/>s. Some Hints may be set
    /// elsewhere, but this is where most are initialised.
    /// </summary>
    /// <param name="appConfig"> The current <see cref="DesktopGLApplicationConfiguration"/>. </param>
    private void SetWindowHints( DesktopGLApplicationConfiguration appConfig )
    {
        Glfw.DefaultWindowHints();

        Glfw.WindowHint( Hint.Visible, false );
        Glfw.WindowHint( Hint.Resizable, appConfig.WindowResizable );
        Glfw.WindowHint( Hint.Maximized, appConfig.WindowMaximized );
        Glfw.WindowHint( Hint.AutoIconify, appConfig.AutoIconify );

        Glfw.WindowHint( Hint.RedBits, appConfig.Red );
        Glfw.WindowHint( Hint.GreenBits, appConfig.Green );
        Glfw.WindowHint( Hint.BlueBits, appConfig.Blue );
        Glfw.WindowHint( Hint.AlphaBits, appConfig.Alpha );
        Glfw.WindowHint( Hint.StencilBits, appConfig.Stencil );
        Glfw.WindowHint( Hint.DepthBits, appConfig.Depth );
        Glfw.WindowHint( Hint.Samples, appConfig.Samples );

        Glfw.WindowHint( Hint.ContextVersionMajor, appConfig.GLContextMajorVersion );
        Glfw.WindowHint( Hint.ContextVersionMinor, appConfig.GLContextMinorVersion );

        if ( Platform.IsMac )
        {
            // hints mandatory on OS X for GL 3.2+ context creation, but fail on Windows if the
            // WGL_ARB_create_context extension is not available
            // see: http://www.glfw.org/docs/latest/compat.html

            Glfw.WindowHint( Hint.OpenGLForwardCompat, true );
            Glfw.WindowHint( Hint.OpenGLProfile, OpenGLProfile.CoreProfile );
        }

        if ( appConfig.TransparentFramebuffer )
        {
            Glfw.WindowHint( Hint.TransparentFramebuffer, true );
        }

        if ( appConfig.Debug )
        {
            Glfw.WindowHint( Hint.OpenGLDebugContext, true );
        }
    }
    
    /// <summary>
    /// Initialises <see cref="GLVersion"/>.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// If GLVersion is less than 2.0 or FBOs are not supported.
    /// </exception>
    private void InitGLVersion()
    {
        Glfw.GetVersion( out var majorv, out var minorv, out var revision );

        unsafe
        {
            GLVersion = new GLVersion( Platform.ApplicationType.WindowsGL,
                                       $"{majorv}.{minorv}.{revision}",
                                       Gdx.GL.glGetString( IGL.GL_VENDOR )->ToString(),
                                       Gdx.GL.glGetString( IGL.GL_RENDERER )->ToString() );
        }
        
        if ( !GLVersion!.IsVersionEqualToOrHigher( 2, 0 ) || !SupportsFBO() )
        {
            var (major, minor) = Gdx.GL.GetProjectOpenGLVersion();

            throw new GdxRuntimeException( $"OpenGL 2.0 or higher with the FBO extension is required. "
                                         + $"OpenGL version: {major}.{minor}"
                                         + $"\n{GLVersion?.DebugVersionString()}" );
        }
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
        return GLVersion.IsVersionEqualToOrHigher( 3, 0 )
            || Glfw.ExtensionSupported( "GL_EXT_framebuffer_object" )
            || Glfw.ExtensionSupported( "GL_ARB_framebuffer_object" );
    }

    /// <summary>
    /// Handle Glfw setup and initialisation.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// Thrown if any problems occurred with GL initialisation.
    /// </exception>
    public static void InitialiseGL()
    {
        if ( _errorCallback == null )
        {
            DesktopGLNativesLoader.Load();

            Glfw.SetErrorCallback( _errorCallback );
            Glfw.InitHint( InitHint.JoystickHatButtons, false );

            if ( !Glfw.Init() )
            {
                throw new GdxRuntimeException( "Unable to initialise Glfw!" );
            }
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region GLDebug specific

    //TODO: Unfinished, see GLDebugMessageSeverity below

    [PublicAPI]
    public struct Gldms
    {
        public int GL43;
        public int Khr;
        public int Arb;
        public int Amd;

        public Gldms( int gl43, int khr, int arb, int amd )
        {
            this.GL43 = gl43;
            this.Khr  = khr;
            this.Arb  = arb;
            this.Amd  = amd;
        }
    }

    public record GLDebugMessageSeverity
    {
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
//        public Gldms Notification = new( GL43.GL_DEBUG_SEVERITY_NOTIFICATION,
//                                         KHRDebug.GL_DEBUG_SEVERITY_NOTIFICATION,
//                                         -1,
//                                         -1 );
    }

    /// <summary>
    /// Enables or disables GL debug messages for the specified severity level.
    /// Returns false if the severity level could not be set (e.g. the NOTIFICATION
    /// level is not supported by the ARB and AMD extensions).
    /// </summary>
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
