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

using LibGDXSharp.Maths;

namespace LibGDXSharp.Utils.Collections;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class ObjectSet<T>
{
    public static int TableSize( int capacity, float loadFactor )
    {
        if ( capacity < 0 )
        {
            throw new ArgumentException( "capacity must be >= 0: " + capacity );
        }
        
        var tableSize = MathUtils.NextPowerOfTwo( Math.Max( 2, ( int )Math.Ceiling( capacity / loadFactor ) ) );

        if ( tableSize > ( 1 << 30 ) )
        {
            throw new ArgumentException( "The required capacity is too large: " + capacity );
        }

        return tableSize;
    }
}