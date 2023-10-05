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
using LibGDXSharp.Core.Utils.Collections;

using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LibGDXSharp;

[PublicAPI]
public class GLApplication : IGLApplicationBase
{
    public GLApplicationConfiguration?         Config             { get; set; }
    public List< GLWindow >                    Windows            { get; set; } = new();
    public Dictionary< string, IPreferences >? Preferences        { get; set; }
    public List< IRunnable >                   Runnables          { get; set; } = new();
    public List< IRunnable >                   ExecutedRunnables  { get; set; } = new();
    public List< ILifecycleListener >          LifecycleListeners { get; set; } = new();
    public GLApplicationLogger                 ApplicationLogger  { get; set; }
    public int                                 LogLevel           { get; set; }
    public IApplication.ApplicationType        AppType            { get; set; }
    public IClipboard?                         Clipboard          { get; set; }

    public static GLVersion? GLVersion { get; set; }

    private static   GLFWCallbacks.ErrorCallback? _errorCallback   = null;
    private static   Callback?                    _glDebugCallback = null;
    private volatile GLWindow?                    _currentWindow   = null;
    private          Sync?                        _sync;

    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="listener"></param>
    /// <param name="config"></param>
    /// <exception cref="SystemException"></exception>
    /// <exception cref="GdxRuntimeException"></exception>
    public GLApplication( IApplicationListener listener, GLApplicationConfiguration config )
    {
        CreateApplication( listener, config );
    }

    public static void InitialiseGL()
    {
        if ( _errorCallback == null )
        {
            GLNativesLoader.Load();

            // TODO: Create error callback - See Java LibGDX

            GLFW.InitHint( InitHintBool.JoystickHatButtons, false );

            if ( GLFW.Init() )
            {
                throw new GdxRuntimeException( "Unable to initialise Glfw!" );
            }
        }
    }

    private void CreateApplication( IApplicationListener listener, GLApplicationConfiguration config )
    {
        InitialiseGL();

        ApplicationLogger = new GLApplicationLogger();

        config.Title ??= listener.GetType().Name;

        this.Config = config = GLApplicationConfiguration.Copy( config );

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
    }

    protected void CleanupWindows()
    {
    }

    protected void Cleanup()
    {
    }

    public void Debug( string tag, string message )
    {
        if ( LogLevel >= IApplication.LOG_DEBUG )
        {
            ApplicationLogger.Debug( tag, message );
        }
    }

    public void Debug( string tag, string message, Exception exception )
    {
        if ( LogLevel >= IApplication.LOG_DEBUG )
        {
            ApplicationLogger.Debug( tag, message, exception );
        }
    }

    public void Log( string tag, string message )
    {
        if ( LogLevel >= IApplication.LOG_INFO )
        {
            ApplicationLogger.Log( tag, message );
        }
    }

    public void Log( string tag, string message, Exception exception )
    {
        if ( LogLevel >= IApplication.LOG_INFO )
        {
            ApplicationLogger.Log( tag, message, exception );
        }
    }

    public void Error( string tag, string message )
    {
        if ( LogLevel >= IApplication.LOG_ERROR )
        {
            ApplicationLogger.Error( tag, message );
        }
    }

    public void Error( string tag, string message, Exception exception )
    {
        if ( LogLevel >= IApplication.LOG_ERROR )
        {
            ApplicationLogger!.Error( tag, message, exception );
        }
    }

    public IPreferences GetPreferences( string name )
    {
        if ( Preferences!.ContainsKey( name ) )
        {
            return Preferences.Get( name )!;
        }

        IPreferences prefs = new GLPreferences( name );

        Preferences.Put( name, prefs );

        return prefs;
    }

    public void CreateWindow( GLWindow window,
                              GLApplicationConfiguration config,
                              long sharedContext )
    {
    }

    public GLWindow CreateWindow( GLApplicationConfiguration config,
                                  IApplicationListener listener,
                                  long sharedContext )
    {
        var window = new GLWindow( listener, config, this );

        if ( sharedContext == 0 )
        {
            // the main window is created immediately
            CreateWindow( window, config, sharedContext );
        }
        else
        {
            // creation of additional windows is deferred to avoid GL context trouble
//            PostRunnable( new Runnable()
//            {
//                public void run ()
//                {
//                createWindow( window, config, sharedContext );
//                windows.add( window );
//            }
//            }
//            );
        }

        return window;
    }

    public IGLAudio CreateAudio( GLApplicationConfiguration config )
    {
        throw new NotImplementedException();
    }

    public IGLInput CreateInput( GLWindow window )
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

    public void PostRunnable( IRunnable runnable )
    {
    }

    public enum GLDebugMessageSeverity
    {
    }

    public static bool SetGLDebugMessageControl( GLDebugMessageSeverity severity, bool enabled )
    {
        return false;
    }

    /// <summary>
    /// </summary>
    private void InitiateGL()
    {
        GLFW.GetVersion( out var major, out var minor, out var revision );

        GLVersion = new GLVersion
            (
             IApplication.ApplicationType.Desktop,
             $"{major}.{minor}.{revision}",
             Gdx.GL20.GLGetString( IGL20.GL_VENDOR ),
             Gdx.GL20.GLGetString( IGL20.GL_RENDERER )
            );
    }
}
