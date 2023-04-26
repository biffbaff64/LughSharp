namespace LibGDXSharp.Utils.Atomic
{
    /// <summary>
    /// A queue that allows one thread to call <see cref="Put(object)"/> and another
    /// thread to call <see cref="Poll()"/>. Multiple threads must not call these methods.
    /// </summary>
    public class AtomicQueue<T>
    {
        private readonly AtomicInteger             _writeIndex = new AtomicInteger();
        private readonly AtomicInteger             _readIndex  = new AtomicInteger();
        private readonly AtomicReferenceArray< T > _queue;

        /// <summary>
        /// </summary>
        /// <param name="capacity"></param>
        public AtomicQueue( int capacity )
        {
            _queue = new AtomicReferenceArray<T>( capacity );
        }

        /// <summary>
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        private int Next( int idx )
        {
            return ( idx + 1 ) % _queue;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Put( T value )
        {
            int write = _writeIndex.Get();
            int read  = _readIndex.Get();
            int next  = Next( write );

            if ( next == read )
            {
                return false;
            }

            _queue.Set( write, value );
            _writeIndex.Set( next );

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual T? Poll()
        {
            int read  = _readIndex.Get();
            int write = _writeIndex.Get();

            if ( read == write )
            {
                return default( T );
            }

            T value = _queue.Get( read );
            _readIndex.Set( Next( read ) );

            return value;
        }
    }
}
