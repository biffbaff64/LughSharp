using LibGDXSharp.Core;

namespace LibGDXSharp.Graphics;

/// <summary>
/// A simple helper class to log the frames per seconds achieved.
/// Just invoke the Log() method in your rendering method. The output
/// will be logged once per second.
/// </summary>
public class FPSLogger
{
    private          long _startTime;
    private readonly int  _bound;

    public FPSLogger( int bound = int.MaxValue )
    {
        this._bound     = bound;
        this._startTime = TimeUtils.NanoTime();
    }

    /// <summary>
    /// Logs the current frames per second to the console. 
    /// </summary>
    public void Log()
    {
        var nanoTime = TimeUtils.NanoTime();

        if ( ( nanoTime - _startTime ) > 1000000000 ) // 1,000,000,000ns == one second
        {
            var fps = Gdx.Graphics?.GetFramesPerSecond();

            if ( fps < _bound )
            {
                Gdx.App.Log( "FPSLogger", "fps: " + fps );

                _startTime = nanoTime;
            }
        }
    }
}