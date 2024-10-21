// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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


namespace DesktopGLBackend.Core;

/// <summary>
/// A highly accurate sync method that continually adapts to the system
/// it runs on to provide reliable results.
/// </summary>
[PublicAPI]
public class Sync
{
    // number of nano seconds in a second.
    private const long NANOS_IN_SECOND = 1000L * 1000L * 1000L;

    // for calculating the averages the previous sleep/yield times are stored.
    private readonly RunningAvg _sleepDurations = new( 10 );
    private readonly RunningAvg _yieldDurations = new( 10 );

    // whether the initialisation code has run.
    private bool _initialised = false;

    // The time to sleep/yield until the next frame.
    private long _nextFrame = 0;

    // ------------------------------------------------------------------------

    /// <summary>
    /// An accurate sync method that will attempt to run at a constant frame rate.
    /// It should be called once every frame.
    /// </summary>
    /// <param name="fps"> the desired frame rate, in frames per second. </param>
    public void SyncFrameRate( int fps )
    {
        if ( fps <= 0 )
        {
            return;
        }

        if ( !_initialised )
        {
            Initialise();
        }

        try
        {
            long t1;

            var t0 = GetTime();

            while ( ( _nextFrame - t0 ) > _sleepDurations.Average )
            {
                Thread.Sleep( 1 );
                _sleepDurations.Add( t1 = GetTime() - t0 );
                t0 = t1;
            }

            _sleepDurations.DampenForLowResTicker();

            t0 = GetTime();

            while ( ( _nextFrame - t0 ) > _yieldDurations.Average )
            {
                Thread.Yield();
                _yieldDurations.Add( t1 = GetTime() - t0 );
                t0 = t1;
            }
        }
        catch ( ThreadInterruptedException )
        {
        }

        _nextFrame = Math.Max( _nextFrame + ( NANOS_IN_SECOND / fps ), GetTime() );
    }

    /// <summary>
    /// This method will initialise the sync method by setting initial values for
    /// sleepDurations/ yieldDurations and nextFrame. If running on windows it will
    /// start the sleep timer fix.
    /// </summary>
    private void Initialise()
    {
        _initialised = true;

        _sleepDurations.Init( 1000 * 1000 );

        _yieldDurations.Init( ( int ) ( -( GetTime() - GetTime() ) * 1.333 ) );

        _nextFrame = GetTime();

        var osName = System.Environment.OSVersion.Platform.ToString();

        if ( osName.StartsWith( "Win" ) )
        {
            var timerAccuracyThread = new Thread( () =>
            {
                try
                {
                    Thread.Sleep( int.MaxValue );
                }
                catch ( ThreadInterruptedException )
                {
                }
            } )
            {
                Name         = "C# Timer",
                IsBackground = true
            };

            timerAccuracyThread.Start();
        }
    }

    /// <summary>
    /// Get the system time in nano seconds.
    /// </summary>
    private long GetTime()
    {
        return ( long ) Glfw.GetTime() * NANOS_IN_SECOND;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private sealed class RunningAvg( int slotCount )
    {
        private const    long   DAMPEN_THRESHOLD = 10 * 1000L * 1000L;
        private const    float  DAMPEN_FACTOR    = 0.9f;
        private readonly long[] _slots           = new long[ slotCount ];
        private          int    _offset          = 0;

        public long Average
        {
            get
            {
                long sum = 0;

                foreach ( var t in _slots )
                {
                    sum += t;
                }

                return sum / _slots.Length;
            }
        }

        public void Init( long value )
        {
            while ( _offset < _slots.Length )
            {
                _slots[ _offset++ ] = value;
            }
        }

        public void Add( long value )
        {
            _slots[ _offset++ % _slots.Length ] =  value;
            _offset                             %= _slots.Length;
        }

        public void DampenForLowResTicker()
        {
            if ( Average > DAMPEN_THRESHOLD )
            {
                for ( var i = 0; i < _slots.Length; i++ )
                {
                    _slots[ i ] = ( long ) ( _slots[ i ] * DAMPEN_FACTOR );
                }
            }
        }
    }
}
