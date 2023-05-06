using LibGDXSharp.GdxCore.Utils.Pooling;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Utils.Pooling;

/// <summary>
/// Stores a map of <see cref="Pool{T}"/>s (usually <see cref="ReflectionPool{T}"/>s)
/// by type for convenient static access.
/// </summary>
public class Pools<T> where T : Type
{
    private readonly static Dictionary< Type, Pool<T>? > typePools = new Dictionary< Type, Pool<T>? >();

    private Pools()
    {
    }

    /// <summary>
    /// Returns a new or existing pool for the specified type, stored in a Class
    /// to map. Note the max size is ignored if this is not the first time this
    /// pool has been requested. 
    /// </summary>
    public static Pool<T> Get( int max = 100 )
    {
        var pool = typePools[ typeof(T) ];

        if ( pool == null )
        {
            pool = new ReflectionPool< T >( 4, max );
                
            typePools.Put( typeof(T), pool );
        }

        return pool;
    }

    /// <summary>
    /// Sets an existing pool for the specified type, stored in a Class
    /// to <see cref="Pool{T}"/> map.
    /// </summary>
    public static void Set( Type type, Pool<T> pool )
    {
        typePools[ type ] = pool;
    }

    /// <summary>
    /// </summary>
    public static T Obtain()
    {
        return Get().Obtain();
    }

    /// <summary>
    /// </summary>
    public static void Free( T obj )
    {
        if ( obj == null ) throw new ArgumentException( "object cannot be null." );

        var pool = typePools.Get( obj );

        // Ignore freeing an object that was never retained.

        pool?.Free( obj );
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
    public static void FreeAll( List<T?> objects, bool samePool = false )
    {
        if ( objects == null )
        {
            throw new ArgumentException( "objects cannot be null." );
        }

        Pool<T>? pool = null;

        for ( int i = 0, n = objects.Count; i < n; i++ )
        {
            T? obj = objects[ i ];

            if ( obj == null ) continue;

            if ( pool == null )
            {
                pool = typePools[ obj ];

                // Ignore freeing an object that was never retained.
                if ( pool == null ) continue;
            }

            pool.Free( obj );

            if ( !samePool )
            {
                pool = null;
            }
        }
    }
}