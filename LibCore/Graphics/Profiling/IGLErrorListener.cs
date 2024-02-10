// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


using System.Reflection;

using LibGDXSharp.LibCore.Utils;

using Exception = System.Exception;

namespace LibGDXSharp.LibCore.Graphics.Profiling;

/// <summary>
///     Listener for GL errors detected by <see cref="GLProfiler" />.
/// </summary>
/// <seealso cref="GLProfiler" />
public interface IGLErrorListener
{
    /// <summary>
    ///     Put your error logging code here.
    /// </summary>
    /// <seealso cref="BaseGLInterceptor.ResolveErrorNumber(int) " />
    void OnError( int error );
}

/// <summary>
///     Listener that will log using Gdx.app.error GL error name and GL function.
/// </summary>
public class GLLoggingListener : IGLErrorListener
{
    public void OnError( int error )
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
        catch ( Exception )
        {
            // ignored
        }

        Core.Gdx.App.Error( "LoggingListener",
                            place != null
                                ? $"Error {BaseGLInterceptor.ResolveErrorNumber( error )} from {place}"

                                // This will capture current stack trace for logging, if possible
                                : $"Error {BaseGLInterceptor.ResolveErrorNumber( error )} at: {new Exception()}"
            );
    }
}

/// <summary>
///     Listener that will throw a GdxRuntimeException with error name.
/// </summary>
public class ThrowingListener : IGLErrorListener
{
    public void OnError( int error )
    {
        throw new GdxRuntimeException( $"GLProfiler: Got GL error {BaseGLInterceptor.ResolveErrorNumber( error )}" );
    }
}
