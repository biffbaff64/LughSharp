using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Utils
{
    public class Pools
    {
        private static ObjectMap< Type, Pool<T> > _typePools = new ObjectMap< Type, Pool<T> >();

        private Pools()
        {
        }

        /// <summary>
        /// Returns a new or existing pool for the specified type, stored in a Class
        /// to map. Note the max size is ignored if this is not the first time this
        /// pool has been requested. 
        /// </summary>
        public static Pool< T > Get<T>( Type type, int max )
        {
            Pool< T > pool = _typePools.Get( type );

            if ( pool == null )
            {
                pool = new ReflectionPool<T>( type, 4, max );
                
                _typePools.Put( type, pool );
            }

            return pool;
        }

        /// <summary>
        /// Returns a new or existing pool for the specified type, stored in a Class
        /// map. The max size of the pool used is 100. 
        /// </summary>
        public static Pool< T > Get<T>( Type type )
        {
            return Get( type, 100 );
        }

        /// <summary>
        /// Sets an existing pool for the specified type, stored in a Class to <seealso cref="Pool"/> map. </summary>
        public static void Set<T>( Type type, Pool< T > pool )
        {
            _typePools.Put( type, pool );
        }

        /// <summary>
        /// </summary>
        public static T Obtain<T>( Type type )
        {
            return Get( type ).Obtain();
        }

        /// <summary>
        /// </summary>
        public static void Free( object obj )
        {
            if ( obj == null )
            {
                throw new System.ArgumentException( "object cannot be null." );
            }

            var pool = _typePools.Get( obj.GetType() );

            if ( pool == null )
            {
                return; // Ignore freeing an object that was never retained.
            }

            pool.Free( obj );
        }

        /// <summary>
        /// Frees the specified objects from the <seealso cref="get(Class) pool"/>.
        /// Null objects within the array are silently ignored.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="samePool">
        /// If true, objects don't need to be from the same pool but the
        /// pool must be looked up for each object.
        /// </param>
        public static void FreeAll( Array objects, bool samePool = false )
        {
            if ( objects == null )
            {
                throw new System.ArgumentException( "objects cannot be null." );
            }

            Pool<T>? pool = null;

            for ( int i = 0, n = objects.Size; i < n; i++ )
            {
                object obj = objects.Get( i );

                if ( obj == null )
                {
                    continue;
                }

                if ( pool == null )
                {
                    pool = _typePools.Get( obj.GetType() );

                    if ( pool == null )
                    {
                        continue; // Ignore freeing an object that was never retained.
                    }
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
