using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Utils.Reflect;

namespace LibGDXSharp.Utils
{
    /// <summary>
    /// Stores a map of <see cref="Pool{T}"/>s (usually <see ref="ReflectionPools"/>) by
    /// type for convenient static access.
    /// </summary>
    public class Pools
    {
        private readonly static ObjectMap< Type, Pool<Type> > typePools = new ObjectMap< Type, Pool<Type> >();

        private Pools()
        {
        }

        /// <summary>
        /// Returns a new or existing pool for the specified type, stored in a Class
        /// to map. Note the max size is ignored if this is not the first time this
        /// pool has been requested. 
        /// </summary>
        public static Pool<T> Get<T>( Type type, int max = 100 )
        {
            var pool = typePools.Get<T>( type );

            if ( pool == null )
            {
                pool = new ReflectionPool<T>( type, 4, max );
                
                typePools.Put( type, pool );
            }

            return pool;
        }

        /// <summary>
        /// Sets an existing pool for the specified type, stored in a Class
        /// to <see cref="Pool{T}"/> map.
        /// </summary>
        public static void Set( Type type, Pool< T > pool )
        {
            typePools.Put( type, pool );
        }

        /// <summary>
        /// </summary>
        public static object? Obtain( Type type )
        {
            return Get( type ).Obtain();
        }

        /// <summary>
        /// </summary>
        public static void Free( object obj )
        {
            if ( obj == null ) throw new ArgumentException( "object cannot be null." );

            Pool<T>? pool = typePools.Get( obj.GetType() );

            // Ignore freeing an object that was never retained.
            if ( pool == null ) return;

            pool.Free( ( T )obj );
        }

        /// <summary>
        /// Frees the specified objects from the <see cref="Get() pool"/>.
        /// Null objects within the array are silently ignored.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="samePool">
        /// If true, objects don't need to be from the same pool but the
        /// pool must be looked up for each object.
        /// </param>
        public static void FreeAll( Array<T> objects, bool samePool = false )
        {
            if ( objects == null )
            {
                throw new ArgumentException( "objects cannot be null." );
            }

            Pool<T>? pool = null;

            for ( int i = 0, n = objects.Size; i < n; i++ )
            {
                T obj = objects.Get( i );

                if ( obj == null ) continue;

                if ( pool == null )
                {
                    pool = typePools.Get( obj.GetType() );

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
}
