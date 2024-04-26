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


namespace LughSharp.LibCore.Graphics.Profiling;

/// <summary>
///     When enabled, collects statistics about GL calls and checks for GL errors.
///     Enabling will wrap Gdx.GL* instances with delegate classes which provide
///     described functionality and route GL calls to the actual GL instances.
/// </summary>
/// <seealso cref="GL20Interceptor" />
/// <seealso cref="GL30Interceptor" />
[PublicAPI]
public class GLProfiler
{
    public IGLErrorListener  Listener    { get; set; }
    public bool              Enabled     { get; set; } = false;
    public IGraphics         Graphics    { get; set; }
    public BaseGLInterceptor Interceptor { get; set; }

    // ------------------------------------------------------------------------

    /// <summary>
    ///     Create a new instance of GLProfiler to monitor a <see cref="IGraphics" />
    ///     instance's gl calls
    /// </summary>
    /// <param name="graphics"> instance to monitor with this instance.</param>
    public GLProfiler( IGraphics graphics )
    {
        Graphics = graphics;

        Interceptor = new GLInterceptor( this );

//        if ( graphics.IsGL30Available() )
//        {
//            Interceptor = new GL30Interceptor( this, graphics.GL30! );
//        }
//        else
//        {
//            Interceptor = new GL20Interceptor( this, graphics.GL20! );
//        }

        Listener = new GLLoggingListener();
    }

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
    ///     Will reset the statistical information which has been collected so far.
    ///     This should be called after every frame.
    ///     Error listener is kept as it is.
    /// </summary>
    public void Reset()
    {
        Interceptor.Reset();
    }

    // ------------------------------------------------------------------------

    /// <summary>
    ///     Enables profiling by replacing the <tt>GL20</tt> and <tt>GL30</tt>
    ///     instances with profiling ones.
    /// </summary>
    public void Enable()
    {
        if ( Enabled ) return;

//        if ( Graphics.IsGL30Available() )
//        {
//            Graphics.GL30 = ( IGL30 ) Interceptor;
//        }
//        else
//        {
//            Graphics.GL20 = Interceptor;
//        }

        Enabled = true;
    }

    /// <summary>
    ///     Disables profiling by resetting the <tt>GL20</tt> and <tt>GL30</tt>
    ///     instances with the original ones.
    /// </summary>
    public void Disable()
    {
        if ( !Enabled ) return;

//        if ( Graphics.GL30 != null )
//        {
//            Graphics.GL30 = ( ( GL30Interceptor ) Graphics.GL30! ).GL30;
//        }
//        else
//        {
//            Graphics.GL20 = ( ( GL20Interceptor ) Graphics.GL20! ).GL20;
//        }

        Enabled = false;
    }
}