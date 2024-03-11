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


namespace LibGDXSharp.Backends.DesktopGL;

[PublicAPI]
public class Sync
{
    private const long NANOS_IN_SECOND = 1000L * 1000L * 1000L;

    private readonly RunningAvg _sleepDurations = new( 10 );
    private readonly RunningAvg _yieldDurations = new( 10 );
    private          bool       _initialised    = false;

    private long _nextFrame = 0;

    public Sync()
    {
    }

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

    private void Initialise()
    {
        _initialised = true;

        _sleepDurations.Init( 1000 * 1000 );
        _yieldDurations.Init( ( int )( -( GetTime() - GetTime() ) * 1.333 ) );

        _nextFrame = GetTime();

        var osName = Environment.OSVersion.Platform.ToString();

        if ( osName.StartsWith( "Win" ) )
        {
            var timerAccuracyThread = new Thread( () =>
                {
                    try
                    {
                        Thread.Sleep( Timeout.Infinite );
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

    private long GetTime()
    {
        return DateTime.UtcNow.Ticks * 100;
    }

    private sealed class RunningAvg
    {
        private const    long   DAMPEN_THRESHOLD = 10 * 1000L * 1000L;
        private const    float  DAMPEN_FACTOR    = 0.9f;
        private readonly long[] _slots;
        private          int    _offset;

        public RunningAvg( int slotCount )
        {
            _slots  = new long[ slotCount ];
            _offset = 0;
        }

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
                    _slots[ i ] = ( long )( _slots[ i ] * DAMPEN_FACTOR );
                }
            }
        }
    }
}
