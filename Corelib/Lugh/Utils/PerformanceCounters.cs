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


namespace Corelib.Lugh.Utils;

/// <summary>
/// Class to keep track of a group of <see cref="PerformanceCounter"/> instances.
/// </summary>
[PublicAPI]
public class PerformanceCounters
{
    /// <summary>
    /// The list of <see cref="PerformanceCounter"/>s to track.
    /// </summary>
    public List< PerformanceCounter > Counters { get; set; } = [ ];

    private const float NANO2_SECONDS = 1f / 1000000000.0f;

    private long _lastTick = 0L;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Adds a new <see cref="PerformanceCounter"/> to the <see cref="Counters"/> list.
    /// </summary>
    /// <param name="name"> The identifying name. </param>
    /// <param name="windowSize"></param>
    /// <returns> The newly created PerformanceCounter. </returns>
    public PerformanceCounter Add( in string name, in int windowSize )
    {
        var result = new PerformanceCounter( name, windowSize );

        Counters.Add( result );

        return result;
    }

    /// <summary>
    /// Adds a new <see cref="PerformanceCounter"/> to the <see cref="Counters"/> list.
    /// </summary>
    /// <param name="name"> The identifying name. </param>
    /// <returns> The newly created PerformanceCounter. </returns>
    public PerformanceCounter Add( in string name )
    {
        var result = new PerformanceCounter( name );

        Counters.Add( result );

        return result;
    }

    /// <summary>
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
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Tick( in float deltaTime )
    {
        foreach ( var t in Counters )
        {
            t.Tick( deltaTime );
        }
    }
}
