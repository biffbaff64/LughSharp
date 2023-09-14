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

namespace LibGDXSharp.Graphics.Profiling;

/// <summary>
/// Listener for GL errors detected by <see cref="GLProfiler"/>.
/// </summary>
/// <seealso cref="GLProfiler"/>
[PublicAPI]
public interface IGLErrorListener
{
    /// <summary>
    /// Put your error logging code here.
    /// </summary>
    /// <seealso cref="GLInterceptor.ResolveErrorNumber(int) "/>
    void OnError( int error );
}

/// <summary>
/// Listener that will log using Gdx.app.error GL error name and GL function.
/// </summary>
[PublicAPI]
public class GLLoggingListener : IGLErrorListener
{
    public void OnError( int error )
    {
        string? place = null;

//        try
//        {
//            StackTraceElement[] stack = Thread.CurrentThread.GetStackTrace();

//            for ( int i = 0; i < stack.Length; i++ )
//            {
//                if ( "check".Equals( stack[ i ].GetMethodName() ) )
//                {
//                    if ( ( i + 1 ) < stack.Length )
//                    {
//                        StackTraceElement glMethod = stack[ i + 1 ];

//                        place = glMethod.getMethodName();
//                    }

//                    break;
//                }
//            }
//        }
//        catch ( Exception ignored )
//        {
//        }

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
[PublicAPI]
class ThrowingListener : IGLErrorListener
{
    public void OnError( int error )
    {
        throw new GdxRuntimeException( "GLProfiler: Got GL error " + GLInterceptor.ResolveErrorNumber( error ) );
    }
}