namespace LibGDXSharp.Utils
{
    /// <summary>
    /// A simple linked list that pools its nodes.
    /// </summary>
    public class PooledLinkedList<T>
    {
        private record Item<TT>
        {
            public TT?         payload;
            public Item< TT >? next;
            public Item< TT >? prev;
        }

        private Item< T >? _head;
        private Item< T >? _tail;
        private Item< T >? _iter;
        private Item< T >? _curr;
        private int        _size = 0;

        private readonly Pool< Item< T > >? _pool;

        public PooledLinkedList( int maxPoolSize )
        {
            this._pool = new Pool< Item< T > >( 16, maxPoolSize )
            {
                NewObject = NewObjectImplementation
            };
        }

        /** Adds the specified object to the end of the list regardless of iteration status */
        public void Add( T obj)
        {
            var item = _pool?.Obtain();
        
            item.payload = obj;
            item.next    = null;
            item.prev    = null;

            if ( _head == null )
            {
                _head = item;
                _tail = item;
                
                _size++;

                return;
            }

            item.prev = _tail;
            _tail.next = item;
            _tail      = item;
            _size++;
        }

        private Item< T > NewObjectImplementation()
        {
            return new Item< T >();
        }
    }
}
