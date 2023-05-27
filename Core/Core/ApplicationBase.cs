using System.Diagnostics.CodeAnalysis;

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