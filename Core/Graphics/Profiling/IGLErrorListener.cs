namespace LibGDXSharp.Graphics.Profiling;

/// <summary>
/// Listener for GL errors detected by <see cref="GLProfiler"/>.
/// </summary>
/// <seealso cref="GLProfiler"/>
public interface IGLErrorListener
{
    /// <summary>
    /// Put your error logging code here.
    /// </summary>
    /// <seealso cref="GLInterceptor.ResolveErrorNumber(int) "/>
    void OnError( int error );
}

// Basic implementations

/// <summary>
/// Listener that will log using Gdx.app.error GL error name and GL function.
/// </summary>
class LoggingListener : IGLErrorListener
{
    public void OnError( int error )
    {
        string? place = null;

        try
        {
            StackTraceElement[] stack = Thread.CurrentThread.GetStackTrace();

            for ( int i = 0; i < stack.Length; i++ )
            {
                if ( "check".Equals( stack[ i ].GetMethodName() ) )
                {
                    if ( ( i + 1 ) < stack.Length )
                    {
                        StackTraceElement glMethod = stack[ i + 1 ];

                        place = glMethod.getMethodName();
                    }

                    break;
                }
            }
        }
        catch ( Exception ignored )
        {
        }

        if ( place != null )
        {
            Gdx.App.Error( "LoggingListener", "Error " + GLInterceptor.ResolveErrorNumber( error ) + " from " + place );
        }
        else
        {
            // This will capture current stack trace for logging, if possible
            Gdx.App.Error( "LoggingListener", "Error " + GLInterceptor.ResolveErrorNumber( error ) + " at: ", new Exception() );
        }
    }
}

/// <summary>
/// Listener that will throw a GdxRuntimeException with error name.
/// </summary>
class ThrowingListener : IGLErrorListener
{
    public void OnError( int error )
    {
        throw new GdxRuntimeException( "GLProfiler: Got GL error " + GLInterceptor.ResolveErrorNumber( error ) );
    }
}