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

namespace LibGDXSharp;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class GLApplication : GLApplicationBase
{
    public GLApplicationConfiguration?        Config             { get; set; }
    public List< GLWindow >                   Windows            { get; set; } = new List< GLWindow >();
    public GLClipboard?                       Clipboard          { get; set; }
    public ObjectMap< string, IPreferences >? Preferences        { get; set; }
    public List< IRunnable >                  Runnables          { get; set; } = new List< IRunnable >();
    public List< IRunnable >                  ExecutedRunnables  { get; set; } = new List< IRunnable >();
    public List< ILifecycleListener >         LifecycleListeners { get; set; } = new List< ILifecycleListener >();

    public static GLVersion? GLVersion { get; set; }

    private static   GlfwCallbacks.ErrorCallback? errorCallback    = null;
    private static   Callback?                    _glDebugCallback = null;
    private volatile GLWindow?                    _currentWindow   = null;
    private          Sync?                        _sync;

    public static void InitialiseGL()
    {
        if ( errorCallback == null )
        {
            GLNativesLoader.Load();

            // TODO: Create error callback

            Glfw.GetApi().InitHint( InitHint.JoystickHatButtons, false );

            if ( Glfw.GetApi().Init() )
            {
                throw new GdxRuntimeException( "Unable to initialise Glfw!" );
            }
        }
    }

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

    // ----- ----- -----
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
                this.Audio = CreateAudio( config );
            }
            catch ( Exception e )
            {
                Log( "Lwjgl3Application", "Couldn't initialize audio, disabling audio", e );

                this.Audio = new MockAudio();
            }
        }
        else
        {
            this.Audio = new MockAudio();
        }

        this.Files     = CreateFiles();
        this.Net       = new GLNet( config );
        this.Clipboard = new GLClipboard();
        this._sync     = new Sync();

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
            if ( e is SystemException )
            {
                throw ( SystemException )e;
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

    public override void Debug( string tag, string message )
    {
        if ( LogLevel >= LogDebug )
        {
            ApplicationLogger!.Debug( tag, message );
        }
    }

    public override void Debug( string tag, string message, Exception exception )
    {
        if ( LogLevel >= LogDebug )
        {
            ApplicationLogger!.Debug( tag, message, exception );
        }
    }

    public override void Log( string tag, string message )
    {
        if ( LogLevel >= LogInfo )
        {
            ApplicationLogger!.Log( tag, message );
        }
    }

    public override void Log( string tag, string message, Exception exception )
    {
        if ( LogLevel >= LogInfo )
        {
            ApplicationLogger!.Log( tag, message, exception );
        }
    }

    public override void Error( string tag, string message )
    {
        if ( LogLevel >= LogError )
        {
            ApplicationLogger!.Error( tag, message );
        }
    }

    public override void Error( string tag, string message, Exception exception )
    {
        if ( LogLevel >= LogError )
        {
            ApplicationLogger!.Error( tag, message, exception );
        }
    }

    public override IPreferences? GetPreferences( string name )
    {
        if ( Preferences!.ContainsKey( name ) ) return Preferences.Get( name );

        IPreferences prefs = new GLPreferences( name );

        Preferences.Put( name, prefs );

        return prefs;
    }

    public GLWindow CreateWindow( GLApplicationConfiguration config, IApplicationListener listener, long sharedContext )
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
            postRunnable
                ( new Runnable()
                {
 


                    public void run ()
                    {
                    createWindow( window, config, sharedContext );
                    windows.add( window );
                }

            }
            );
        }

        return window;
    }

    public override IGLAudio CreateAudio( GLApplicationConfiguration config )
    {
        throw new NotImplementedException();
    }

    public override IGLInput CreateInput( GLWindow window )
    {
        throw new NotImplementedException();
    }

    protected IFiles CreateFiles()
    {
        return new GLFiles();
    }

    public override int GetVersion()
    {
        return 0;
    }

    public override IApplication.ApplicationType Type { get; set; }

    public override void Exit()
    {
    }

    public override void AddLifecycleListener( ILifecycleListener listener )
    {
    }

    public override void RemoveLifecycleListener( ILifecycleListener listener )
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
        Glfw.GetApi().GetVersion( out var major, out var minor, out var revision );

        GLVersion = new GLVersion
            (
             IApplication.ApplicationType.Desktop,
             $"{major}.{minor}.{revision}",
             Gdx.GL20.GLGetString( IGL20.GL_Vendor ),
             Gdx.GL20.GLGetString( IGL20.GL_Renderer )
            );
    }
}