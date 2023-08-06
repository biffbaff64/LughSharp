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

namespace LibGDXSharp.Utils;

public class PerformanceCounters
{
    private const float NANO2_SECONDS = 1f / 1000000000.0f;

    public readonly List< PerformanceCounter > counters = new();

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
            Tick( ( t - _lastTick ) * NANO2_SECONDS );
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