using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils.Collections.Extensions
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public static class ListExtensions
    {
        public static T[] Resize<T>( this IList< T > ts, int newSize )
        {
            var newItems = new T[ newSize ];
            
            Array.Copy( ts.ToArray(), newItems, newSize  );

//            if ( newSize < ts. )
            
//            if ( newSize < ts.Capacity )
//            {
//                ts.RemoveRange
//            }
                
//            T[] items    = this.items;
//            T[] newItems = ( T[] )ArrayReflection.newInstance( items.getClass().getComponentType(), newSize );
//            System.arraycopy( items, 0, newItems, 0, Math.min( size, newItems.length ) );
//            this.items = newItems;

            return newItems;
        }

        /// <summary>
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List< T > With<T>( T t )
        {
            var list = new List< T >
            {
                t
            };

            return list;
        }
        
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>( this IList< T > ts )
        {
            var count = ts.Count;
            var last  = count - 1;

            var random = new Random();

            for ( var i = 0; i < last; ++i )
            {
                var r = random.Next( i, count );

                ( ts[ i ], ts[ r ] ) = ( ts[ r ], ts[ i ] );
            }
        }

        /// <summary>
        /// Removes and returns the last item in the list.
        /// </summary>
        /// <param name="ts"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="IllegalStateException"></exception>
        public static T Pop<T>( this IList< T > ts )
        {
            if ( ts.Count == 0 ) throw new IllegalStateException( "Array is empty." );

            T item = ts[ ^1 ];

            ts.RemoveAt( ts.Count - 1 );

            return item;
        }

        /// <summary>
        /// Removes and returns the item at the specified index.
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RemoveIndex<T>( this IList< T > ts, int index )
        {
            if ( index >= ts.Count )
            {
                throw new IndexOutOfRangeException( "index can't be >= size: " + index + " >= " + ts.Count );
            }

            T value = ts[ index ];

            ts.RemoveAt( index );

            return value;
        }
    }
}
