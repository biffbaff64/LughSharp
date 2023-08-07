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

using LibGDXSharp.GdxCore.Utils.Pooling;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Utils.Pooling;

/// <summary>
/// Stores a map of <see cref="Pool{T}"/>s (usually <see cref="ReflectionPool{T}"/>s)
/// by type for convenient static access.
/// </summary>
public class Pools<T>
{
    private readonly static Dictionary< Type, Pool< T >? > TYPE_POOLS = new();

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
        Pool< T >? pool = TYPE_POOLS[ typeof(T) ];

        if ( pool == null )
        {
            pool = new ReflectionPool< T >( 4, max );

            TYPE_POOLS.Put( typeof(T), pool );
        }

        return pool;
    }

    /// <summary>
    /// Sets an existing pool for the specified type, stored in a Class
    /// to <see cref="Pool{T}"/> map.
    /// </summary>
    public static void Set( Type type, Pool< T > pool )
    {
        TYPE_POOLS[ type ] = pool;
    }

    /// <summary>
    /// </summary>
    public static T Obtain() => Get().Obtain();

    /// <summary>
    /// </summary>
    public static void Free( T obj )
    {
        if ( obj == null ) throw new ArgumentException( "object cannot be null." );

        TYPE_POOLS[ typeof(T) ]?.Free( obj );
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
        if ( objects == null )
        {
            throw new ArgumentException( "objects cannot be null." );
        }

        for ( int i = 0, n = objects.Count; i < n; i++ )
        {
            T? obj = objects[ i ];

            if ( obj == null ) continue;

            if ( TYPE_POOLS[ typeof(T) ] == null ) continue;

            TYPE_POOLS[ typeof(T) ]?.Free( obj );

            if ( !samePool )
            {
                TYPE_POOLS[ typeof(T) ] = null;
            }
        }
    }
}