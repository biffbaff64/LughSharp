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

using LibGDXSharp.Core.Utils.Collections;

namespace LibGDXSharp.Utils.Pooling;

/// <summary>
/// Stores a map of <see cref="Pool{T}"/>s by type for convenient static access.
/// </summary>
[PublicAPI]
public class Pools<T>
{
    private readonly static Dictionary< Type, Pool< T >? > TypePools = new();

    private Pools()
    {
    }

    /// <summary>
    /// Returns a new or existing pool for the specified type, stored in a Class
    /// to map. Note the max size is ignored if this is not the first time this
    /// pool has been requested. 
    /// </summary>
    public static Pool< T > Get( int max = 100 )
    {
        Pool< T >? pool = TypePools[ typeof( T ) ];

        if ( pool == null )
        {
            pool = new Pool< T >( 4, max );

            TypePools.Put( typeof( T ), pool );
        }

        return pool;
    }

    /// <summary>
    /// Sets an existing pool for the specified type, stored in a Class
    /// to <see cref="Pool{T}"/> map.
    /// </summary>
    public static void Set( Type type, Pool< T > pool )
    {
        TypePools[ type ] = pool;
    }

    /// <summary>
    /// </summary>
    public static T? Obtain() => Get().Obtain();

    /// <summary>
    /// </summary>
    public static void Free( T obj )
    {
        ArgumentNullException.ThrowIfNull( obj );

        TypePools[ typeof( T ) ]?.Free( obj );
    }

    /// <summary>
    /// Frees the specified objects from the pool.
    /// Null objects within the array are silently ignored.
    /// </summary>
    /// <param name="objects"></param>
    /// <param name="samePool">
    /// If true, objects don't need to be from the same pool but the
    /// pool must be looked up for each object.
    /// </param>
    public static void FreeAll( List< T? > objects, bool samePool = false )
    {
        ArgumentNullException.ThrowIfNull( objects );

        for ( int i = 0, n = objects.Count; i < n; i++ )
        {
            T? obj = objects[ i ];

            if ( ( obj == null ) || ( TypePools[ typeof( T ) ] == null ) )
            {
                continue;
            }

            TypePools[ typeof( T ) ]?.Free( obj );

            if ( !samePool )
            {
                TypePools[ typeof( T ) ] = null;
            }
        }
    }
}
