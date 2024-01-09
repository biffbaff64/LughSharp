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
///     When enabled, collects statistics about GL calls and checks for GL errors.
///     Enabling will wrap Gdx.GL* instances with delegate classes which provide
///     described functionality and route GL calls to the actual GL instances.
/// </summary>
/// <seealso cref="GL20Interceptor" />
/// <seealso cref="GL30Interceptor" />
public class GLProfiler
{
    /// <summary>
    ///     Create a new instance of GLProfiler to monitor a <see cref="IGraphics" />
    ///     instance's gl calls
    /// </summary>
    /// <param name="graphics"> instance to monitor with this instance.</param>
    public GLProfiler( IGraphics graphics )
    {
        Graphics = graphics;

        if ( graphics.IsGL30Available() )
        {
            Interceptor = new GL30Interceptor( this, graphics.GL30! );
        }
        else
        {
            Interceptor = new GL20Interceptor( this, graphics.GL20! );
        }

        Listener = new GLLoggingListener();
    }

    public IGLErrorListener Listener    { get; set; }
    public bool             Enabled     { get; set; } = false;
    public IGraphics        Graphics    { get; set; }
    public GLInterceptor    Interceptor { get; set; }

    /// <summary>
    ///     Returns the total gl calls made since the last reset
    /// </summary>
    public int Calls => Interceptor.Calls;

    /// <summary>
    ///     Returns the total amount of texture bindings made since the last reset
    /// </summary>
    public int TextureBindings => Interceptor.TextureBindings;

    /// <summary>
    ///     Returns the total amount of draw calls made since the last reset
    /// </summary>
    public int DrawCalls => Interceptor.DrawCalls;

    /// <summary>
    /// </summary>
    /// <returns>
    ///     the total amount of shader switches made since the last reset
    /// </returns>
    public int ShaderSwitches => Interceptor.ShaderSwitches;

    /// <summary>
    ///     Returns <see cref="FloatCounter" /> containing information about rendered
    ///     vertices since the last reset.
    /// </summary>
    public FloatCounter VertexCount => Interceptor.VertexCount;

    /// <summary>
    ///     Enables profiling by replacing the <tt>GL20</tt> and <tt>GL30</tt>
    ///     instances with profiling ones.
    /// </summary>
    public void Enable()
    {
        if ( Enabled )
        {
            return;
        }

        if ( Graphics.IsGL30Available() )
        {
            Graphics.GL30 = ( IGL30 )Interceptor;
        }
        else
        {
            Graphics.GL20 = Interceptor;
        }

        Enabled = true;
    }

    /// <summary>
    ///     Disables profiling by resetting the <tt>GL20</tt> and <tt>GL30</tt>
    ///     instances with the original ones.
    /// </summary>
    public void Disable()
    {
        if ( !Enabled )
        {
            return;
        }

        if ( Graphics.GL30 != null )
        {
            Graphics.GL30 = ( ( GL30Interceptor )Graphics.GL30! ).GL30;
        }
        else
        {
            Graphics.GL20 = ( ( GL20Interceptor )Graphics.GL20! ).GL20;
        }

        Enabled = false;
    }

    /// <summary>
    ///     Will reset the statistical information which has been collected so far.
    ///     This should be called after every frame.
    ///     Error listener is kept as it is.
    /// </summary>
    public void Reset() => Interceptor.Reset();
}
