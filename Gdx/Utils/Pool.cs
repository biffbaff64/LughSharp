using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Utils
{
    public abstract class Pool<T>
    {
        // The maximum number of objects that will be pooled.
        public int Max  { get; set; }

        // The highest number of free objects. Can be reset any time.
        public int Peak { get; set; }

        private readonly Array< T > _freeObjects;

        /// <summary>
        /// Creates a pool with an initial capacity of 16 and no maximum.
        /// </summary>
        protected Pool() : this( 16 )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="initialCapacity">
        /// The initial size of the array supporting the pool. No objects are created/pre-allocated.
        /// </param>
        /// <param name="max">The maximum number of free objects to store in this pool.</param>
        protected Pool( int initialCapacity, int max = int.MaxValue )
        {
            _freeObjects = new Array< T >( false, initialCapacity );

            this.Max = max;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected abstract T? NewObject();

        /// <summary>
        /// Returns an object from this pool.
        /// The object may be new (from <see cref="NewObject"/>) or reused
        /// (previously <see cref="Free(T)"/> freed).
        /// </summary>
        public T? Obtain()
        {
            return _freeObjects.Size == 0 ? NewObject() : _freeObjects.Pop();

        }

        /// <summary>
        /// Puts the specified object in the pool, making it eligible to be returned by
        /// <see cref="Obtain"/>. If the pool already contains <see cref="Max"/> free objects,
        /// the specified object is discarded using <see cref="Discard"/> and not added to the pool.
        /// The pool does not check if an object is already freed, so the same object must not be
        /// freed multiple times.
        /// </summary>
        /// <param name="obj">The object to add to the pool.</param>
        /// <exception cref="ArgumentException"></exception>
        public void Free( T obj )
        {
            if ( obj == null )
            {
                throw new ArgumentException( "object cannot be null." );
            }

            if ( _freeObjects.Size < Max )
            {
                _freeObjects.Add( obj );

                Peak = Math.Max( Peak, _freeObjects.Size );

                Reset( obj );
            }
            else
            {
                Discard( obj );
            }
        }

        /// <summary>
        /// Adds the specified number of new free objects to the pool.
        /// Usually called early on as a pre-allocation mechanism but
        /// can be used at any time.
        /// </summary>
        /// <param name="size">The number of objects to be added.</param>
        public void Fill( int size )
        {
            for ( var i = 0; i < size; i++ )
            {
                if ( _freeObjects.Size < Max )
                {
                    _freeObjects.Add( NewObject() );
                }
            }

            Peak = Math.Max( Peak, _freeObjects.Size );
        }

        /// <summary>
        /// Called when an object is freed to clear the state of the object for possible
        /// later reuse. The default implementation calls IPoolable.Reset if the object is Poolable.
        /// </summary>
        protected void Reset( T obj )
        {
            if ( obj is IPoolable poolable )
            {
                poolable.Reset();
            }
        }

        /// <summary>
        /// Called when an object is discarded. This is the case when an object is
        /// freed, but the maximum capacity of the pool is reached, and when the
        /// pool is <see cref="Clear"/>ed.
        /// </summary>
        protected void Discard( T obj )
        {
        }

        /// <summary>
        /// Puts the specified objects in the pool. Null objects within the array
        /// are silently ignored. The pool does not check if an object is already
        /// freed, so the same object must not be freed multiple times.
        /// </summary>
        public void FreeAll( Array< T > objects )
        {
            if ( objects == null ) throw new ArgumentException( "objects cannot be null." );

            var max = this.Max;

            for ( int i = 0, n = objects.Size; i < n; i++ )
            {
                T obj = objects.Get( i );

                if ( obj == null ) continue;

                if ( this._freeObjects.Size < max )
                {
                    this._freeObjects.Add( obj );

                    Reset( obj );
                }
                else
                {
                    Discard( obj );
                }
            }

            Peak = Math.Max( Peak, this._freeObjects.Size );
        }

        /// <summary>
        /// Removes and discards all free objects from this pool.
        /// </summary>
        public void Clear()
        {
            for ( var i = 0; i < _freeObjects.Size; i++ )
            {
                T obj = _freeObjects.Pop();
                Discard( obj );
            }
        }

        /// <summary>
        /// The number of objects available to be obtained.
        /// </summary>
        public int GetFree()
        {
            return _freeObjects.Size;
        }
    }
}
