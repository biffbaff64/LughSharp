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

using System.Runtime.CompilerServices;
using System.Security.Principal;

using LibGDXSharp.Backends.Desktop;
using LibGDXSharp.Backends.Desktop.Audio;
using LibGDXSharp.Backends.Desktop.Audio.Mock;
using LibGDXSharp.Core.Utils.Collections;

using Sync = LibGDXSharp.Backends.Desktop.Sync;

namespace LibGDXSharp;

[PublicAPI]
public class DesktopGLApplication : IGLApplicationBase
{

    #region public properties

    public DesktopGLApplicationConfiguration?  Config             { get; set; }
    public List< DesktopGLWindow >             Windows            { get; set; } = new();
    public Dictionary< string, IPreferences >? Preferences        { get; set; }
    public List< Runnable >                    Runnables          { get; set; } = new();
    public List< Runnable >                    ExecutedRunnables  { get; set; } = new();
    public List< ILifecycleListener >          LifecycleListeners { get; set; } = new();
    public GLApplicationLogger?                ApplicationLogger  { get; set; }
    public int                                 LogLevel           { get; set; }
    public IApplication.ApplicationType        AppType            { get; set; }
    public IClipboard?                         Clipboard          { get; set; }

    public static GLVersion? GLVersion { get; set; }

    #endregion public properties

    private static   GLFWCallbacks.ErrorCallback? _errorCallback = null;
    private volatile DesktopGLWindow?             _currentWindow = null;
    private          IGLAudio?                    _audio         = null;
    private          Sync?                        _sync          = null;
    private          bool                         _running       = true;

    // ------------------------------------------------------------------------

    public DesktopGLApplication( IApplicationListener listener,
                                 DesktopGLApplicationConfiguration config )
    {
        CreateApplication( listener, config );
    }

    private void CreateApplication( IApplicationListener listener,
                                    DesktopGLApplicationConfiguration config )
    {
        InitialiseGL();

        ApplicationLogger = new GLApplicationLogger();

        config.Title ??= listener.GetType().Name;

        this.Config = config = DesktopGLApplicationConfiguration.Copy( config );

        Gdx.App = this;

        if ( !config.DisableAudio )
        {
            try
            {
                Gdx.Audio = CreateAudio( config );
            }
            catch ( Exception e )
            {
                Log( "GLApplication", "Couldn't initialize audio, disabling audio", e );

                Gdx.Audio = new MockAudio();
            }
        }
        else
        {
            Gdx.Audio = new MockAudio();
        }

        Gdx.Files      = CreateFiles();
        Gdx.Net        = new GLNet( config );
        this.Clipboard = new GLClipboard();
        this._sync     = new Sync();

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

    /// <summary>
    /// Framework Main Loop
    /// </summary>
    protected void Loop()
    {
        List< DesktopGLWindow > closedWindows = new();

        while ( _running && ( Windows.Count > 0 ) )
        {
            _audio?.Update();

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
                _sync?.SyncFrameRate( targetFramerate ); // sleep as needed to meet the target framerate
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

        _audio?.Dispose();

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

    public IPreferences GetPreferences( string name )
    {
        if ( Preferences!.ContainsKey( name ) )
        {
            return Preferences.Get( name );
        }

        IPreferences prefs = new GLPreferences( name );

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
    public unsafe DesktopGLWindow NewWindow( IApplicationListener listener, DesktopGLWindowConfiguration config )
    {
        GdxRuntimeException.ThrowIfNull( Config );

        DesktopGLApplicationConfiguration appConfig = DesktopGLApplicationConfiguration.Copy( this.Config );

        appConfig.SetWindowConfiguration( config );

        return CreateWindow( appConfig, listener, this.Windows[ 0 ].WindowHandle );
    }

    public void CreateWindow( DesktopGLWindow window,
                              DesktopGLApplicationConfiguration config,
                              long sharedContext )
    {
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

        return window;
    }

    public void PostRunnable( Runnable runnable )
    {
        lock ( Runnables )
        {
            Runnables.Add( runnable );
        }
    }

    public static void InitialiseGL()
    {
        if ( _errorCallback == null )
        {
            GLNativesLoader.Load();

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

    public IGLInput CreateInput( DesktopGLWindow window )
    {
        throw new NotImplementedException();
    }

    protected IFiles CreateFiles()
    {
        return new GLFiles();
    }

    public int GetVersion()
    {
        return 0;
    }

    public void Exit()
    {
    }

    public void AddLifecycleListener( ILifecycleListener listener )
    {
    }

    public void RemoveLifecycleListener( ILifecycleListener listener )
    {
    }

    public enum GLDebugMessageSeverity
    {
    }

    /// <summary>
    /// Enables or disables GL debug messages for the specified severity level.
    /// Returns false if the severity level could not be set (e.g. the NOTIFICATION
    /// level is not supported by the ARB and AMD extensions).
    /// </summary>
    /// <seealso cref="DesktopGLApplicationConfiguration.EnableGLDebugOutput(bool, StreamWriter)"/>
    public static bool SetGLDebugMessageControl( GLDebugMessageSeverity severity, bool enabled )
    {
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
