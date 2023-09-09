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

using System.Text;

using LibGDXSharp.Maths;

namespace LibGDXSharp.Utils;

/// <summary>
/// Class to keep track of the time and load (percentage of total time) a
/// specific task takes. Call start() just before starting the task and
/// stop() right after. You can do this multiple times if required.
/// <p>
/// Every render or update call tick() to update the values. The time
/// FloatCounter provides access to the minimum, maximum, average,
/// total and current time (in seconds) the task takes. Likewise for
/// the load value, which is the percentage of the total time.
/// </p>
/// </summary>
[PublicAPI]
public class PerformanceCounter
{
    private const float NANO2_SECONDS = 1f / 1000000000.0f;

    private long _startTime = 0L;
    private long _lastTick  = 0L;

    /// <summary>
    /// The time value of this counter (seconds)
    /// </summary>
    public FloatCounter Time { get; set; }

    /// <summary>
    /// The load value of this counter
    /// </summary>
    public FloatCounter Load { get; set; }

    /// <summary>
    /// The name of this counter
    /// </summary>
    public string Name { get; set; }

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

    public PerformanceCounter( in string name ) : this( name, 5 )
    {
    }

    public PerformanceCounter( in string name, in int windowSize )
    {
        this.Name = name;
        this.Time = new FloatCounter( windowSize );
        this.Load = new FloatCounter( 1 );
    }

    /// <summary>
    /// Updates the time and load counters and resets the time.
    /// Call <seealso cref="Start()"/> to begin a new count. The values are only
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
    /// Call <seealso cref="Start()"/> to begin a new count.
    /// </summary>
    /// <param name="delta"> The time since the last call to this method</param>
    public void Tick( in float delta )
    {
        if ( !Valid )
        {
            Gdx.App.Error
                (
                 "PerformanceCounter",
                 "Invalid data, check if you called PerformanceCounter#stop()"
                );

            return;
        }

        Time.Put( Current );

        var currentLoad = delta == 0f ? 0f : Current / delta;

        Load.Put( ( delta > 1f ) ? currentLoad : ( delta * currentLoad ) + ( ( 1f - delta ) * Load.Latest ) );

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

    /// <summary>
    /// </summary>
    public override string ToString()
    {
        var sb = new StringBuilder();

        return ToString( sb ).ToString();
    }

    /// <summary>
    /// Creates a string in the form of "name [time: value, load: value]"
    /// </summary>
    public StringBuilder ToString( in StringBuilder sb )
    {
        sb.Append( Name )
            .Append( ": [time: " )
            .Append( Time.Value )
            .Append( ", load: " )
            .Append( Load.Value )
            .Append( ']' );

        return sb;
    }
}