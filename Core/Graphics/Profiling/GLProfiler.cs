using LibGDXSharp.Backends.Desktop;
using LibGDXSharp.Maths;

namespace LibGDXSharp.Graphics.Profiling;

/// <summary>
/// When enabled, collects statistics about GL calls and checks for GL errors.
/// Enabling will wrap Gdx.GL* instances with delegate classes which provide
/// described functionality and route GL calls to the actual GL instances.
/// </summary>
/// <seealso cref="GL20Interceptor"/>
/// <seealso cref="GL30Interceptor"/>
public class GLProfiler
{
    public IGLErrorListener Listener { get; set; }
    public bool             Enabled  { get; set; } = false;

    private IGraphics     _graphics;
    private GLInterceptor _glInterceptor;

    /// <summary>
    /// Create a new instance of GLProfiler to monitor a <see cref="IGraphics"/>
    /// instance's gl calls
    /// </summary>
    /// <param name="graphics"> instance to monitor with this instance.</param>
    public GLProfiler( IGraphics graphics )
    {
        this._graphics = graphics;

        if ( graphics.IsGL30Available() )
        {
            _glInterceptor = new GL30Interceptor( this, graphics.GetGL30() );
        }
        else
        {
            _glInterceptor = new GL20Interceptor( this, graphics.GetGL20() );
        }

        Listener = IGLErrorListener.LOGGING_LISTENER;
    }

    /// <summary>
    /// Enables profiling by replacing the <tt>GL20</tt> and <tt>GL30</tt>
    /// instances with profiling ones.
    /// </summary>
    public void Enable()
    {
        if ( Enabled )
        {
            return;
        }

        GL30 gl30 = _graphics.getGL30();

        if ( gl30 != null )
        {
            _graphics.setGL30( ( GL30 )_glInterceptor );
        }
        else
        {
            _graphics.setGL20( _glInterceptor );
        }

        Enabled = true;
    }

    /// <summary>
    /// Disables profiling by resetting the {@code GL20} and {@code GL30} instances with the original ones. </summary>
    public void Disable()
    {
        if ( !Enabled )
        {
            return;
        }

        GL30 gl30 = _graphics.getGL30();

        if ( gl30 != null )
        {
            _graphics.setGL30( ( ( GL30Interceptor )_graphics.getGL30() ).gl30 );
        }
        else
        {
            _graphics.setGL20( ( ( GL20Interceptor )_graphics.getGL20() ).gl20 );
        }

        Enabled = false;
    }

    /// <summary>
    /// Returns the total gl calls made since the last reset
    /// </summary>
    public int Calls => _glInterceptor.GetCalls();

    /// <summary>
    /// Returns the total amount of texture bindings made since the last reset
    /// </summary>
    public int TextureBindings => _glInterceptor.GetTextureBindings();

    /// <summary>
    /// Returns the total amount of draw calls made since the last reset
    /// </summary>
    public int DrawCalls => _glInterceptor.GetDrawCalls();

    /// 
    /// <returns> the total amount of shader switches made since the last reset </returns>
    public int ShaderSwitches => _glInterceptor.ShaderSwitches;

    /// <summary>
    /// Returns <see cref="FloatCounter"/> containing information about rendered
    /// vertices since the last reset.
    /// </summary>
    public FloatCounter VertexCount => _glInterceptor.VertexCount;

    /// <summary>
    /// Will reset the statistical information which has been collected so far.
    /// This should be called after every frame.
    /// Error listener is kept as it is. 
    /// </summary>
    public void Reset() => _glInterceptor.Reset();
}