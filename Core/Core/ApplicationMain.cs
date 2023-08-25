// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Backends.Desktop;
using LibGDXSharp.Backends.Desktop.Audio;
using LibGDXSharp.Core.Utils.Collections;
using LibGDXSharp.Utils;

namespace LibGDXSharp.Core;

public class ApplicationMain : IApplication
{
    /// <inheritdoc cref="IApplication.LogLevel"/>
    public int LogLevel { get; set; }

    public IApplication.ApplicationType        AppType            { get; set; }
    public IClipboard?                         Clipboard          { get; set; }
    public ApplicationConfiguration?           Config             { get; set; }
    public List< GLWindow >                    Windows            { get; set; } = new();
    public Dictionary< string, IPreferences >? Preferences        { get; set; }
    public List< IRunnable >                   Runnables          { get; set; } = new();
    public List< IRunnable >                   ExecutedRunnables  { get; set; } = new();
    public List< ILifecycleListener >          LifecycleListeners { get; set; } = new();
    public ApplicationLogger                   ApplicationLogger  { get; set; } = new();

    public static GLVersion? GLVersion { get; set; }

    private static   GLFWCallbacks.ErrorCallback? _errorCallback   = null;
    private static   Callback?                    _glDebugCallback = null;
    private volatile GLWindow?                    _currentWindow   = null;
    private          Sync?                        _sync;

    // ------------------------------------------------------------------------

    public ApplicationMain( IApplicationListener listener, ApplicationConfiguration config )
    {
        CreateApplication( listener, config );
    }

    public void InitialiseGL()
    {
    }

    // ------------------------------------------------------------------------

    private void CreateApplication( IApplicationListener listener, ApplicationConfiguration config )
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

    // ------------------------------------------------------------------------

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

    // ------------------------------------------------------------------------

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
        throw new NotImplementedException();
    }

    public int GetVersion() => 0;

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

    public void Exit()
    {
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
    
    // ------------------------------------------------------------------------

    public void CreateWindow( GLWindow window, GLApplicationConfiguration config, long sharedContext )
    {
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
    
    // ------------------------------------------------------------------------

    public void AddLifecycleListener( ILifecycleListener listener )
    {
    }

    public void RemoveLifecycleListener( ILifecycleListener listener )
    {
    }

    public void PostRunnable( IRunnable runnable )
    {
    }

    // ------------------------------------------------------------------------

    public void Log( string tag, string message )
    {
    }

    public void Log( string tag, string message, Exception exception )
    {
    }

    public void Error( string tag, string message )
    {
    }

    public void Error( string tag, string message, Exception exception )
    {
    }

    public void Debug( string tag, string message )
    {
    }

    public void Debug( string tag, string message, Exception exception )
    {
    }

    public enum GLDebugMessageSeverity
    {
    }

    public static bool SetGLDebugMessageControl( GLDebugMessageSeverity severity, bool enabled )
    {
        return false;
    }
}
