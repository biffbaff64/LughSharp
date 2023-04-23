using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Utils.Collections
{
    /// <summary>
    /// An array that allows modification during iteration. Guarantees that
    /// array entries provided by begin() between indexes 0 and size at the
    /// time begin was called will not be modified until end() is called. If
    /// modification of the SnapshotArray occurs between begin/end, the backing
    /// array is copied prior to the modification, ensuring that the backing
    /// array that was returned by begin() is unaffected. To avoid allocation,
    /// an attempt is made to reuse any extra array created as a result of this
    /// copy on subsequent copies.
    /// <para>
    /// Note that SnapshotArray is not for thread safety, only for modification
    /// during iteration.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public sealed class SnapshotArray<T> : List< T >
    {
        private T[]? _snapshot;
        private T[]? _recycled;
        private int  _snapshots;

        public SnapshotArray( int capacity = 0 ) : base( capacity )
        {
        }

        public SnapshotArray( List< T > array ) : base( array )
        {
        }

        public SnapshotArray( T[] array ) : base( array )
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public T?[] Begin()
        {
            if ( _snapshot == null )
            {
                throw new NullReferenceException( "Snapshot array should not be NULL in Begin()" );
            }

            Modified();

            CopyTo( _snapshot );

            _snapshots++;

            return ToArray();
        }

        /// <summary>
        /// </summary>
        public void End()
        {
            _snapshots = System.Math.Max( 0, _snapshots - 1 );

            if ( _snapshot != base.ToArray() && _snapshots == 0 )
            {
                // The backing array was copied, keep around the old array.
                _recycled = _snapshot;

                if ( _recycled != null )
                {
                    for ( int i = 0, n = _recycled.Length; i < n; i++ )
                    {
                        _recycled[ i ] = default!;
                    }
                }
            }

            _snapshot = null!;
        }

        /// <summary>
        /// </summary>
        private void Modified()
        {
            if ( _snapshot != base.ToArray() ) return;

            // Snapshot is in use, copy backing array to recycled array or create new backing array.
            if ( _recycled?.Length >= Count )
            {
                // Copy the contents of items[] to recycled
                for ( var i = 0; i < Count; i++ )
                {
                    _recycled[ i ] = base[ i ];
                }
                
                // 'recycled' now references nothing 
                _recycled = default!;
            }
            else
            {
                this.Resize( base.Count );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set( int index, T value )
        {
            Modified();
            this[ index ] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public new void Insert( int index, T value )
        {
            Modified();
            base.Insert( index, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        public new void InsertRange( int index, IEnumerable< T > collection )
        {
            Modified();
            base.InsertRange( index, collection );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public new bool Remove( T value )
        {
            Modified();

            return base.Remove( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new T RemoveAt( int index )
        {
            Modified();

            return this.RemoveIndex( index );
        }

        /// <summary>
        /// Removes a range of elements from the array.
        /// </summary>
        /// <param name="start">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public new void RemoveRange( int start, int count )
        {
            Modified();
            base.RemoveRange( start, count );
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the
        /// specified predicate.
        /// </summary>
        /// <param name="match">
        /// The Predicate delegate that defines the conditions of the elements to remove.
        /// </param>
        /// <returns>The number of elements removed from the array</returns>
        public new int RemoveAll( Predicate< T > match )
        {
            Modified();

            return base.RemoveAll( match );
        }
    }
}
