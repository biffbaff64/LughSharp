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

using Corelib.Lugh.Utils.Collections;

namespace Corelib.Lugh.Utils.Pooling;

/// <summary>
/// Stores a map of <see cref="Pool{T}"/>s by type for convenient static access.
/// </summary>
[PublicAPI]
public static class Pools< T >
{
    private static readonly Dictionary< Type, Pool< T >? > _typePools = new();

    // ========================================================================
    
    /// <summary>
    /// Returns a new or existing pool for the specified type, stored in a Class
    /// to map. Note the max size is ignored if this is not the first time this
    /// pool has been requested.
    /// </summary>
    public static Pool< T > Get( int max = 100 )
    {
        var pool = _typePools[ typeof( T ) ];

        if ( pool == null )
        {
            pool = new Pool< T >( 4, max );

            _typePools.Put( typeof( T ), pool );
        }

        return pool;
    }

    /// <summary>
    /// Sets an existing pool for the specified type, stored in a Class
    /// to <see cref="Pool{T}"/> map.
    /// </summary>
    public static void Set( Type type, Pool< T > pool )
    {
        _typePools[ type ] = pool;
    }

    /// <inheritdoc cref="Pool{T}.Obtain()"/>
    public static T? Obtain()
    {
        return Get().Obtain();
    }

    /// <inheritdoc cref="Pool{T}.Free(T)"/>
    public static void Free( T? obj )
    {
        ArgumentNullException.ThrowIfNull( obj );

        _typePools[ typeof( T ) ]?.Free( obj );
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
    public static void FreeAll( List< T > objects, bool samePool = false )
    {
        ArgumentNullException.ThrowIfNull( objects );

        for ( int i = 0, n = objects.Count; i < n; i++ )
        {
            var obj = objects[ i ];

            if ( ( obj == null ) || ( _typePools[ typeof( T ) ] == null ) )
            {
                continue;
            }

            _typePools[ typeof( T ) ]?.Free( obj );

            if ( !samePool )
            {
                _typePools[ typeof( T ) ] = null;
            }
        }
    }
}
