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

using LibGDXSharp.Backends.Desktop.Audio;
using LibGDXSharp.Utils;

namespace LibGDXSharp.Backends.Desktop;

public abstract class GLApplicationBase : IApplication
{
    public abstract IGLAudio CreateAudio( GLApplicationConfiguration config );

    public abstract IGLInput CreateInput( GLWindow window );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Getter and Setter for the log level.
    /// LogNone will mute all log output.
    /// LogError will only let error messages through.
    /// LogInfo will let all non-debug messages through.
    /// LogDebug will let all messages through.
    /// </summary>
    public int LogLevel { get; set; }

    public IApplication.ApplicationType AppType   { get; set; }
    public IClipboard?                  Clipboard { get; set; }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public abstract void         Log( string tag, string message );
    public abstract void         Log( string tag, string message, Exception exception );
    public abstract void         Error( string tag, string message );
    public abstract void         Error( string tag, string message, Exception exception );
    public abstract void         Debug( string tag, string message );
    public abstract void         Debug( string tag, string message, Exception exception );
    public abstract int          GetVersion();
    public abstract IPreferences GetPreferences( string name );
    public abstract void         Exit();
    public abstract void         AddLifecycleListener( ILifecycleListener listener );
    public abstract void         RemoveLifecycleListener( ILifecycleListener listener );
    public abstract void         PostRunnable( IRunnable runnable );
}