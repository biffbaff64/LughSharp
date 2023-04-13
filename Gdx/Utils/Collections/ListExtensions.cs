namespace LibGDXSharp.Utils.Collections
{
    public static class ListExtensions
    {
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
    }
}
