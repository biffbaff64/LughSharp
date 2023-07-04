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

namespace LibGDXSharp.Core;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassWithVirtualMembersNeverInherited.Global" )]
public class ApplicationBase : IApplication
{
    /// <summary>
    /// Getter and Setter for the log level.
    /// LogNone will mute all log output.
    /// LogError will only let error messages through.
    /// LogInfo will let all non-debug messages through.
    /// LogDebug will let all messages through.
    /// </summary>
    public virtual int LogLevel { get; set; }

    public virtual IApplication.ApplicationType AppType { get; set; }

    public virtual int GetVersion() => throw new NotImplementedException();

    public virtual IPreferences GetPreferences( string name ) => throw new NotImplementedException();

    public virtual IClipboard GetClipBoard() => throw new NotImplementedException();

    public virtual void Log( string tag, string message )
    {
        throw new NotImplementedException();
    }

    public virtual void Log( string tag, string message, Exception exception )
    {
        throw new NotImplementedException();
    }

    public virtual void Error( string tag, string message )
    {
        throw new NotImplementedException();
    }

    public virtual void Error( string tag, string message, Exception exception )
    {
        throw new NotImplementedException();
    }

    public virtual void Debug( string tag, string message )
    {
        throw new NotImplementedException();
    }

    public virtual void Debug( string tag, string message, Exception exception )
    {
        throw new NotImplementedException();
    }

    public virtual void Exit()
    {
        throw new NotImplementedException();
    }

    public virtual void AddLifecycleListener( ILifecycleListener listener )
    {
        throw new NotImplementedException();
    }

    public virtual void RemoveLifecycleListener( ILifecycleListener listener )
    {
        throw new NotImplementedException();
    }

    public virtual void PostRunnable( IRunnable runnable )
    {
        throw new NotImplementedException();
    }
}