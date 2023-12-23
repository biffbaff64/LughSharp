// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Collections;

namespace LibGDXSharp.Utils.Collections;

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
[PublicAPI]
public class SnapshotArray<T> : Array< T >, IEnumerable< T >
{
    private T[]? _snapshot;
    private T[]? _recycled;
    private int  _snapshotCount;

    private IEnumerable< T >? _iterable;

    public SnapshotArray( int capacity = 0 )
        : this( true, capacity )
    {
    }

    public SnapshotArray( SnapshotArray< T > array )
    {
        ArgumentNullException.ThrowIfNull( array );
        ArgumentNullException.ThrowIfNull( array.Items );

        Ordered = array.Ordered;
        Size    = array.Size;
        Items   = new T[ Size ];

        Array.Copy( array.Items, 0, Items, 0, Size );
    }

    public SnapshotArray( bool ordered = true, int capacity = 16 )
    {
        Ordered = ordered;
        Items   = new T[ capacity ];
    }

    public SnapshotArray( bool ordered, T[] array, int startIndex, int count )
    {
        Ordered = ordered;
        Size    = count;
        Items   = new T[ count ];

        Array.Copy( array, startIndex, Items, 0, count );
    }

    /// <summary>
    /// Takes a snapshot of the current array state and then
    /// returns the array. 
    /// </summary>
    /// <returns></returns>
    public T?[] Begin()
    {
        Modified();

        _snapshot = ToArray();

        _snapshotCount++;

        return ToArray();
    }

    /// <summary>
    /// </summary>
    public void End()
    {
        _snapshotCount = Math.Max( 0, _snapshotCount - 1 );

        if ( ( _snapshot != ToArray() ) && ( _snapshotCount == 0 ) )
        {
            // The backing array was copied, keep around the old array.
            _recycled = _snapshot;

            if ( _recycled != null )
            {
                for ( int i = 0, n = _recycled.Length; i < n; i++ )
                {
                    _recycled[ i ] = default( T )!;
                }
            }
        }

        _snapshot = null!;
    }

    /// <summary>
    /// </summary>
    public void Modified()
    {
        if ( _snapshot != ToArray() )
        {
            return;
        }

        // Snapshot is in use, copy backing array to recycled
        // array or create new backing array.
        if ( _recycled?.Length >= Size )
        {
            // Copy the contents of items[] to recycled
            for ( var i = 0; i < Size; i++ )
            {
                _recycled[ i ] = Items[ i ];
            }

            // 'recycled' now references nothing 
            _recycled = null;
        }
        else
        {
            this.Resize( Size );
        }
    }

    public override void Add( T value )
    {
        Modified();

        if ( Size == Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int )( Size * 1.75f ) ) );
        }

        Items[ Size++ ] = value;
    }

    /// <summary>
    /// Copy 'count' items from the supplied array to this array,
    /// starting from position 'start'.
    /// </summary>
    /// <param name="array">The array of items to add.</param>
    /// <param name="start">The start index.</param>
    /// <param name="count">The number of items to copy.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void AddAll( SnapshotArray< T > array, int start, int count )
    {
        if ( ( start + count ) > array.Size )
        {
            throw new ArgumentOutOfRangeException
                ( $@"start + count must be <= size - {start} + {count} <= {array.Size}" );
        }

        Modified();

        AddAll( array.Items, start, count );
    }

    /// <summary>
    /// Copy 'count' items from the supplied array to this array,
    /// starting from position 'start'.
    /// </summary>
    /// <param name="array">The array of items to add.</param>
    /// <param name="start">The start index.</param>
    /// <param name="count">The number of items to copy.</param>
    public override void AddAll( T?[] array, int start, int count )
    {
        Modified();

        var sizeNeeded = Size + count;

        if ( sizeNeeded > Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int )( sizeNeeded * 1.75f ) ) );
        }

        Array.Copy( array, start, Items, Size, count );

        Size += count;
    }

    public override T Get( int index )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( $@"index can't be >= size - {index} >= {Size}" );
        }

        return Items[ index ];
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public override void Set( int index, T value )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( $@"index can't be >= size - {index} >= {Size}" );
        }

        Modified();

        Items[ index ] = value;
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public override void Insert( int index, T value )
    {
        if ( index > Size )
        {
            throw new ArgumentOutOfRangeException( $@"index can't be >= size - {index} >= {Size}" );
        }

        if ( Items == null )
        {
            throw new GdxRuntimeException( "Items cannot be null!" );
        }

        Modified();

        if ( Size == Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int )( Size * 1.75f ) ) );
        }

        if ( Ordered )
        {
            Array.Copy( Items, index, Items, index + 1, Size - index );
        }
        else
        {
            Items[ Size ] = Items[ index ];
        }

        Size++;
        Items[ index ] = value;
    }

    public override void Swap( int first, int second )
    {
        Modified();

        ( Items[ first ], Items[ second ] ) = ( Items[ second ], Items[ first ] );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Remove( T value )
    {
        Modified();

        for ( int i = 0, n = Size; i < n; i++ )
        {
            if ( value!.Equals( Items[ i ] ) )
            {
                RemoveAt( i );

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T RemoveAt( int index )
    {
        Modified();

        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( $@"index can't be >= size - {index} >= {Size}" );
        }

        T value = Items[ index ];

        Size--;

        if ( Ordered )
        {
            Array.Copy( Items, index + 1, Items, index, Size - index );
        }
        else
        {
            Items[ index ] = Items[ Size ];
        }

        Items[ Size ] = default( T )!;

        return value;
    }

    /// <summary>
    /// Removes a range of elements from the array.
    /// </summary>
    /// <param name="start">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="end">The ending index of the range.</param>
    public override void RemoveRange( int start, int end )
    {
        Modified();

        if ( end >= Size )
        {
            throw new ArgumentOutOfRangeException( $@"end can't be >= size - {end} >= {Size}" );
        }

        if ( start > end )
        {
            throw new ArgumentOutOfRangeException( $@"start can't be > end - {start} > {end}" );
        }

        var count = ( end - start ) + 1;

        if ( Ordered )
        {
            Array.Copy( Items, start + count, Items, start, Size - ( start + count ) );
        }
        else
        {
            var lastIndex = this.Size - 1;

            for ( var i = 0; i < count; i++ )
            {
                Items[ start + i ] = Items[ lastIndex - i ];
            }
        }

        Size -= count;
    }

    /// <summary>
    /// Removes all the elements that match the conditions defined by the
    /// specified predicate.
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public bool RemoveAll( SnapshotArray< T > array )
    {
        Modified();

        var size      = this.Size;
        var startSize = size;

        for ( int i = 0, n = array.Size; i < n; i++ )
        {
            T item = array.Get( i );

            for ( var ii = 0; ii < size; ii++ )
            {
                if ( item!.Equals( Items[ ii ] ) )
                {
                    RemoveAt( ii );
                    size--;

                    break;
                }
            }
        }

        return size != startSize;
    }

    /// <summary>
    /// Returns the index of first occurrence of value in the array,
    /// or -1 if no such value exists.
    /// </summary>
    /// <param name="value"> May be null. </param>
    /// <returns>
    /// An index of first occurrence of value in array or -1 if no such value exists
    /// </returns>
    public override int IndexOf( T? value )
    {
        for ( int i = 0, n = Size; i < n; i++ )
        {
            if ( ( value != null ) && value.Equals( Items[ i ] ) )
            {
                return i;
            }
        }

        return -1;
    }

    public override bool Contains( T? value )
    {
        var i = Size - 1;

        while ( i >= 0 )
        {
            if ( Items[ i-- ]!.Equals( value ) )
            {
                return true;
            }
        }

        return false;
    }

    public override T Peek()
    {
        if ( Size == 0 )
        {
            throw new NullReferenceException( "Array is empty." );
        }

        return Items[ Size - 1 ];
    }

    public override T Pop()
    {
        Modified();

        if ( Size == 0 )
        {
            throw new IndexOutOfRangeException( "Array is empty." );
        }

        --Size;

        T item = Items[ Size ];

        Items[ Size ] = default( T )!;

        return item;
    }

    public override void Clear()
    {
        Array.Clear( Items );

        Size = 0;
    }

    protected override T[] Resize( int newSize )
    {
        var newItems = ( T[] )Array.CreateInstance( Items.GetType(), newSize );

        Array.Copy( Items, 0, newItems, 0, System.Math.Min( Size, newItems.Length ) );

        this.Items = newItems;

        return newItems;
    }

    public override T[] ToArray()
    {
        Type? memberInfo = Items.GetType().BaseType;

        return memberInfo != null
            ? ToArray( memberInfo )
            : ( T[] )Array.CreateInstance( Items.GetType(), Size );
    }

    public override T[] ToArray( Type type )
    {
        var result = ( T[] )Array.CreateInstance( type, Size );

        Array.Copy( Items, 0, result, 0, Size );

        return result;
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals( object? obj )
    {
        if ( obj == this )
        {
            return true;
        }

        if ( !Ordered )
        {
            return false;
        }

        var array = ( SnapshotArray< T >? )obj;

        if ( array is not { Ordered: true } )
        {
            return false;
        }

        var n = Size;

        if ( n != array.Size )
        {
            return false;
        }

        for ( var i = 0; i < n; i++ )
        {
            T obj1 = this.Items[ i ];
            T obj2 = array.Items[ i ];

            if ( !( obj1?.Equals( obj2 ) ?? ( obj2 == null ) ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator< T > GetEnumerator()
    {
        return new SnapshotEnumerator< T >( Items );
    }
}

[PublicAPI]
public class SnapshotEnumerator<T> : IEnumerator< T >
{
    private int _position = -1;
    private T[] _array;

    public SnapshotEnumerator( T[] array )
    {
        this._array = array;
    }

    /// <inheritdoc />
    public bool MoveNext()
    {
        _position++;

        return _position < _array.Length;
    }

    /// <inheritdoc />
    public void Reset()
    {
    }

    /// <inheritdoc />
    public T Current
    {
        get
        {
            try
            {
                return _array[ _position ];
            }
            catch ( IndexOutOfRangeException )
            {
                throw new InvalidOperationException();
            }
            catch ( NullReferenceException )
            {
                Gdx.App.Error( "SnapshotArrayEnumerator", "NullReference encountered!" );

                throw;
            }
        }
    }

    /// <inheritdoc />
    object IEnumerator.Current => Current!;

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
