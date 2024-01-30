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

using System.Reflection;

using ErrorCode = OpenGL.ErrorCode;

namespace LibGDXSharp.Graphics.Profiling;

/// <summary>
///     Listener for GL errors detected by <see cref="GLProfiler" />.
/// </summary>
/// <seealso cref="GLProfiler" />
public interface IGLErrorListener
{
    /// <summary>
    ///     Put your error logging code here.
    /// </summary>
    /// <seealso cref="GLInterceptor.ResolveErrorNumber(ErrorCode) " />
    void OnError( ErrorCode error );
}

/// <summary>
///     Listener that will log using Gdx.app.error GL error name and GL function.
/// </summary>
public class GLLoggingListener : IGLErrorListener
{
    public void OnError( ErrorCode error )
    {
        string? place = null;

        try
        {
            var          stackTrace = new StackTrace();
            StackFrame[] frames     = stackTrace.GetFrames();

            for ( var i = 0; i < frames.Length; i++ )
            {
                if ( "check".Equals( frames[ i ].GetMethod()?.Name ) )
                {
                    if ( ( i + 1 ) < frames.Length )
                    {
                        MethodBase? glMethod = frames[ i + 1 ].GetMethod();
                        place = glMethod?.Name;
                    }

                    break;
                }
            }
        }
        catch ( System.Exception )
        {
            // ignored
        }

        Gdx.App.Error( "LoggingListener",
                       place != null
                           ? $"Error {GLInterceptor.ResolveErrorNumber( error )} from {place}"

                           // This will capture current stack trace for logging, if possible
                           : $"Error {GLInterceptor.ResolveErrorNumber( error )} at: {new System.Exception()}"
            );
    }
}

/// <summary>
///     Listener that will throw a GdxRuntimeException with error name.
/// </summary>
public class ThrowingListener : IGLErrorListener
{
    public void OnError( ErrorCode error )
    {
        throw new GdxRuntimeException( $"GLProfiler: Got GL error {GLInterceptor.ResolveErrorNumber( error )}" );
    }
}
