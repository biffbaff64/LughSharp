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

using Corelib.Lugh.Maths;

namespace Corelib.Lugh.Utils;

/// <summary>
/// Class to keep track of the time and load (percentage of total time) a specific task
/// takes. Call start() just before starting the task and stop() right after. You can do
/// this multiple times if required.
/// <para>
/// Every render or update call tick() to update the values. The time FloatCounter provides
/// access to the minimum, maximum, average, total and current time (in seconds) the task
/// takes. Likewise for the load value, which is the percentage of the total time.
/// </para>
/// </summary>
[PublicAPI]
public class PerformanceCounter
{
    private const float NANO2_SECONDS = 1f / 1000000000.0f;

    private long _lastTick  = 0L;
    private long _startTime = 0L;

    // ========================================================================

    /// <summary>
    /// Creates a new PerformanceCounter instance, giving it the siupplied name.
    /// </summary>
    public PerformanceCounter( in string name ) : this( name, 5 )
    {
    }

    /// <summary>
    /// Creates a new PerformanceCounter instance, giving it the supplied name.
    /// </summary>
    public PerformanceCounter( in string name, in int windowSize )
    {
        Name = name;
        Time = new FloatCounter( windowSize );
        Load = new FloatCounter( 1 );
    }

    /// <summary>
    /// The time value of this counter (seconds)
    /// </summary>
    public FloatCounter Time { get; }

    /// <summary>
    /// The load value of this counter
    /// </summary>
    public FloatCounter Load { get; }

    /// <summary>
    /// The name of this counter
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The current value in seconds, you can manually increase this using your own
    /// timing mechanism if needed, if you do so, you also need to update <see cref="Valid"/>.
    /// </summary>
    public float Current { get; set; } = 0f;

    /// <summary>
    /// Flag to indicate that the current value is valid, you need to set this to true
    /// if using your own timing mechanism
    /// </summary>
    public bool Valid { get; set; } = false;

    /// <summary>
    /// Updates the time and load counters and resets the time.
    /// Call <see cref="Start()"/> to begin a new count. The values are only
    /// valid after at least two calls to this method.
    /// </summary>
    public void Tick()
    {
        var t = TimeUtils.NanoTime();

        if ( _lastTick > 0L )
        {
            Tick( ( t - _lastTick ) * NANO2_SECONDS );
        }

        _lastTick = t;
    }

    /// <summary>
    /// Updates the time and load counters and resets the time.
    /// Call <see cref="Start()"/> to begin a new count.
    /// </summary>
    /// <param name="delta"> The time since the last call to this method</param>
    public void Tick( in float delta )
    {
        if ( !Valid )
        {
            Logger.Error( "Invalid data, check if you called PerformanceCounter#stop()" );

            return;
        }

        Time.Put( Current );

        var currentLoad = delta == 0f ? 0f : Current / delta;

        Load.Put( delta > 1f ? currentLoad : ( delta * currentLoad ) + ( ( 1f - delta ) * Load.Latest ) );

        Current = 0f;
        Valid   = false;
    }

    /// <summary>
    /// Start counting, call this method just before performing the task you
    /// want to keep track of. Call <see cref="Stop()"/> when done.
    /// </summary>
    public void Start()
    {
        _startTime = TimeUtils.NanoTime();
        Valid      = false;
    }

    /// <summary>
    /// Stop counting, call this method right after you performed the task you
    /// want to keep track of. Call <see cref="Start()"/> again when you perform
    /// more of that task.
    /// </summary>
    public void Stop()
    {
        if ( _startTime > 0L )
        {
            Current    += ( TimeUtils.NanoTime() - _startTime ) * NANO2_SECONDS;
            _startTime =  0L;
            Valid      =  true;
        }
    }

    /// <summary>
    /// Resets this performance counter to its defaults values.
    /// </summary>
    public void Reset()
    {
        Time.Reset();
        Load.Reset();

        _startTime = 0L;
        _lastTick  = 0L;
        Current    = 0f;
        Valid      = false;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Name}: [time: {Time.Value}, load: {Load.Value}]";
    }
}
