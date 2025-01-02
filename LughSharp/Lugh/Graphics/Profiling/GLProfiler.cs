// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Graphics.OpenGL;
using LughSharp.Lugh.Maths;

namespace LughSharp.Lugh.Graphics.Profiling;

/// <summary>
/// When enabled, collects statistics about GL calls and checks for GL errors.
/// Enabling will wrap GdxApi.GL* instances with delegate classes which provide
/// described functionality and route GL calls to the actual GL instances.
/// </summary>
[PublicAPI]
public class GLProfiler
{
    public IGLErrorListener  ErrorListener     { get; set; }
    public bool              Enabled           { get; set; } = false;
    public BaseGLInterceptor BaseGLInterceptor { get; set; }

    private IGLBindings _glBindingsStore;

    // ========================================================================

    /// <summary>
    /// Create a new instance of GLProfiler to monitor a <see cref="IGraphics"/>
    /// instance's gl calls
    /// </summary>
    public GLProfiler()
    {
        BaseGLInterceptor = new GLInterceptor( this );
        ErrorListener     = new GLLoggingListener();

        _glBindingsStore = GdxApi.Bindings;
    }

    // ========================================================================

    /// <summary>
    /// Will reset the statistical information which has been collected so far.
    /// This should be called after every frame.
    /// Error listener is kept as it is.
    /// </summary>
    public void Reset()
    {
        BaseGLInterceptor.Reset();
    }

    /// <summary>
    /// Enables profiling by replacing the <tt>GL</tt> instance with a profiling one.
    /// </summary>
    public void Enable()
    {
        if ( Enabled )
        {
            return;
        }

        GdxApi.Graphics.GL = ( IGLBindings )BaseGLInterceptor;

        Enabled = true;
    }

    /// <summary>
    /// Disables profiling by resetting the <tt>GL</tt> instances with the original ones.
    /// </summary>
    public void Disable()
    {
        if ( !Enabled )
        {
            return;
        }

        GdxApi.Graphics.GL = _glBindingsStore;

        Enabled = false;
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Returns the total gl calls made since the last reset
    /// </summary>
    public int Calls => BaseGLInterceptor.Calls;

    /// <summary>
    /// Returns the total amount of texture bindings made since the last reset
    /// </summary>
    public int TextureBindings => BaseGLInterceptor.TextureBindings;

    /// <summary>
    /// Returns the total amount of draw calls made since the last reset
    /// </summary>
    public int DrawCalls => BaseGLInterceptor.DrawCalls;

    /// <summary>
    /// </summary>
    /// <returns>
    /// the total amount of shader switches made since the last reset
    /// </returns>
    public int ShaderSwitches => BaseGLInterceptor.ShaderSwitches;

    /// <summary>
    /// Returns <see cref="FloatCounter"/> containing information about rendered
    /// vertices since the last reset.
    /// </summary>
    public FloatCounter VertexCount => BaseGLInterceptor.VertexCount;
}