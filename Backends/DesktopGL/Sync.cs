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

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class Sync
{
    private const long NANOS_IN_SECOND = 1000L * 1000L * 1000L;

    private readonly RunningAvg _sleepDurations = new( 10 );
    private readonly RunningAvg _yieldDurations = new( 10 );

    private long _nextFrame   = 0;
    private bool _initialised = false;

    public Sync()
    {
    }

    public void SyncFrameRate( int fps )
    {
        if ( fps <= 0 ) return;
        if ( !_initialised ) Initialise();

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
            var timerAccuracyThread = new Thread
                (
                () =>
                {
                    try
                    {
                        Thread.Sleep( Timeout.Infinite );
                    }
                    catch ( ThreadInterruptedException )
                    {
                    }
                }
                )
                {
                    Name         = "C# Timer",
                    IsBackground = true
                };

            timerAccuracyThread.Start();
        }
    }

    private long GetTime() => ( DateTime.UtcNow.Ticks * 100 );

    private class RunningAvg
    {
        private readonly long[] _slots;
        private          int    _offset;

        private const long  DAMPEN_THRESHOLD = 10 * 1000L * 1000L;
        private const float DAMPEN_FACTOR    = 0.9f;

        public RunningAvg( int slotCount )
        {
            _slots  = new long[ slotCount ];
            _offset = 0;
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