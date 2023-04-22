using System.Text;

namespace LibGDXSharp.Utils
{
    public class PerformanceCounters
    {
        private const float Nano2Seconds = 1f / 1000000000.0f;

        public readonly List< PerformanceCounter > counters  = new List< PerformanceCounter >();

        private long _lastTick = 0L;

        public PerformanceCounter Add( in string name, in int windowSize )
        {
            var result = new PerformanceCounter( name, windowSize );

            counters.Add( result );

            return result;
        }

        public PerformanceCounter Add( in string name )
        {
            var result = new PerformanceCounter( name );

            counters.Add( result );

            return result;
        }

        public void Tick()
        {
            long t = TimeUtils.NanoTime();

            if ( _lastTick > 0L )
            {
                Tick( ( t - _lastTick ) * Nano2Seconds );
            }

            _lastTick = t;
        }

        public void Tick( in float deltaTime )
        {
            foreach ( PerformanceCounter t in counters )
            {
                t.Tick( deltaTime );
            }
        }

        public StringBuilder ToString( in StringBuilder sb )
        {
            sb.Length = 0;

            for ( var i = 0; i < counters.Count; i++ )
            {
                if ( i != 0 )
                {
                    sb.Append( "; " );
                }

                counters[ i ].ToString( sb );
            }

            return sb;
        }
    }
}
