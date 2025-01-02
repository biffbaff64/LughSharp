// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils.Collections.DeleteCandidates;
using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Utils.Collections;

/// <summary>
/// A resizable, ordered or unordered array of objects. If unordered, this class
/// avoids a memory copy when removing elements (the last element is moved to the
/// removed element's position).
/// </summary>
/// <typeparam name="T"> The type of elements returned by the iterator. </typeparam>
[PublicAPI]
public class Array< T >
{
    public T[]  Items   { get; set; }
    public int  Size    { get; set; }
    public bool Ordered { get; set; }

    private PredicateIterable< T >? _predicateIEnumerable;

    // ========================================================================

    /// <summary>
    /// Creates a new Array with the specified initial capacity. Default is 16.
    /// </summary>
    /// <param name="ordered">
    /// If false, methods that remove elements may change the order of other elements
    /// in the array, which avoids a memory copy.
    /// </param>
    /// <param name="capacity">
    /// The initial capacity.  Any elements added beyond this will cause the backing
    /// array to be grown.
    /// </param>
    public Array( bool ordered = true, int capacity = 16 )
    {
        Ordered = ordered;
        Items   = new T[ capacity ];
    }

    /// <summary>
    /// Creates a new Array object from the supplied Array{T} object.
    /// </summary>
    public Array( Array< T > array )
        : this( array.Ordered, array.Items, 0, array.Size )
    {
    }

    /// <summary>
    /// Creates a new Array object from the supplied array[].
    /// </summary>
    public Array( T[] array )
        : this( true, array, 0, array.Length )
    {
    }

    /// <summary>
    /// Creates a new Array object from the supplied array[].
    /// </summary>
    /// <param name="ordered">
    /// If false, methods that remove elements may change the order of other
    /// elements in the array, which avoids a memory copy.
    /// </param>
    /// <param name="array"> The array[]  to use. </param>
    /// <param name="start"> The start index to start copyiong from. </param>
    /// <param name="count"> The number of elements to copy. </param>
    public Array( bool ordered, T[]? array, int start, int count )
    {
        ArgumentNullException.ThrowIfNull( array );

        Ordered = ordered;
        Size    = count;
        Items   = new T[ Size ];

        Array.Copy( array, start, Items, 0, Size );
    }

    /// <summary>
    /// Adds the specified value to this array.
    /// </summary>
    public virtual void Add( T value )
    {
        if ( Size == Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int ) ( Size * 1.75f ) ) );
        }

        Items[ Size++ ] = value;
    }

    /// <summary>
    /// Adds all items in the supplied array to this array.
    /// </summary>
    public virtual void AddAll( List< T > array )
    {
        AddAll( array, 0, array.Count );
    }

    /// <summary>
    /// Copy items from the supplied array to this array,
    /// starting from position 0.
    /// </summary>
    /// <param name="array">The array of items to add.</param>
    public virtual void AddAll( params T[] array )
    {
        AddAll( array, 0, array.Length );
    }

    /// <summary>
    /// Copy 'count' items from the supplied array to this array,
    /// starting from position 'start'.
    /// </summary>
    /// <param name="array">The array of items to add.</param>
    /// <param name="start">The start index.</param>
    /// <param name="count">The number of items to copy.</param>
    public virtual void AddAll( List< T > array, int start, int count )
    {
        ArgumentNullException.ThrowIfNull( array );

        if ( ( start + count ) > array.Count )
        {
            throw new ArgumentOutOfRangeException
                ( $"start + count must be <= size - {start} + {count} <= {array.Count}" );
        }

        AddAll( array.ToArray(), start, count );
    }

    /// <summary>
    /// Copy 'count' items from the supplied array to this array,
    /// starting from position 'start'.
    /// </summary>
    /// <param name="array">The array of items to add.</param>
    /// <param name="start">The start index.</param>
    /// <param name="count">The number of items to copy.</param>
    public virtual void AddAll( T?[] array, int start, int count )
    {
        var sizeNeeded = Size + count;

        if ( sizeNeeded > Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int ) ( sizeNeeded * 1.75f ) ) );
        }

        Array.Copy( array, start, Items, Size, count );

        Size += count;
    }

    /// <summary>
    /// Gets the array item at the specified index.
    /// </summary>
    /// <param name="index"> The index into the array. Must be in the range 0 - Size-1. </param>
    /// <returns></returns>
    public virtual T GetAt( int index )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( nameof( index ), $"index can't be >= size - {index} >= {Size}" );
        }

        if ( index < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( index ), "index cannot be less than 0." );
        }

        return Items[ index ];
    }

    /// <summary>
    /// Sets the elementn at the specified index to the supplied value.
    /// </summary>
    /// <param name="index"> The index into the Array. </param>
    /// <param name="value"> The new value. </param>
    public virtual void Set( int index, T value )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( "index can't be >= size - " + index + " >= " + Size );
        }

        Items[ index ] = value;
    }

    /// <summary>
    /// Inserts the supplied value into the array at the specified index.
    /// </summary>
    /// <param name="index"> The index to insert the value. </param>
    /// <param name="value"> The value top insert. </param>
    public virtual void Insert( int index, T value )
    {
        if ( index > Size )
        {
            throw new ArgumentOutOfRangeException( "index can't be > size - " + index + " > " + Size );
        }

        if ( Items == null )
        {
            throw new GdxRuntimeException( "Items cannot be null!" );
        }

        if ( Size == Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int ) ( Size * 1.75f ) ) );
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

    /// <summary>
    /// Inserts room for <paramref name="count"/> elements into the array
    /// starting at position <paramref name="index"/>.
    /// </summary>
    public virtual void InsertRange( int index, int count )
    {
        if ( index > Size )
        {
            throw new IndexOutOfRangeException( "index can't be > size - " + index + " > " + Size );
        }

        if ( Items == null )
        {
            throw new GdxRuntimeException( "Items cannot be null!" );
        }

        var sizeNeeded = Size + count;

        if ( sizeNeeded > Items.Length )
        {
            Items = Resize( Math.Max( Math.Max( 8, sizeNeeded ), ( int ) ( Size * 1.75f ) ) );
        }

        Array.Copy( Items, index, Items, index + count, Size - index );

        Size = sizeNeeded;
    }

    /// <summary>
    /// Swaps the elements at positions <paramref name="first"/> and
    /// <paramref name="second"/>
    /// </summary>
    /// <param name="first"> Array index of the first element. </param>
    /// <param name="second"> Array index of the second element. </param>
    public virtual void Swap( int first, int second )
    {
        if ( first >= Size )
        {
            throw new ArgumentOutOfRangeException( "first can't be >= size - " + first + " >= " + Size );
        }

        if ( second >= Size )
        {
            throw new ArgumentOutOfRangeException( "second can't be >= size - " + second + " >= " + Size );
        }

        if ( Items == null )
        {
            throw new GdxRuntimeException( "Items cannot be null!" );
        }

        ( Items[ first ], Items[ second ] ) = ( Items[ second ], Items[ first ] );
    }

    /// <summary>
    /// Returns TRUE if this array contains the requested value.
    /// </summary>
    public virtual bool Contains( T? value )
    {
        return ( Size != 0 ) && ( IndexOf( value ) >= 0 );
    }

    /// <summary>
    /// Returns the index of the first occurance of the requested value in this array.
    /// </summary>
    public virtual int IndexOf( T? value )
    {
        if ( Items == null )
        {
            throw new GdxRuntimeException( "Items cannot be null!" );
        }

        return Array.IndexOf( Items, value, 0, Size );
    }

    /// <summary>
    /// Returns the index of the last occurance of the requested value in this array.
    /// </summary>
    /// <param name="value"> The value to find. </param>
    /// <returns> The above mentione3d index. </returns>
    public virtual int LastIndexOf( T? value )
    {
        return Array.LastIndexOf( Items, value );
    }

    /// <summary>
    /// Removes the first occurance of the requested value from this array.
    /// </summary>
    /// <param name="value"> The value to remove. </param>
    /// <returns> True if successful. </returns>
    public virtual bool RemoveValue( T value )
    {
        for ( int i = 0, n = Size; i < n; i++ )
        {
            if ( ( value != null ) && value.Equals( Items[ i ] ) )
            {
                RemoveIndex( i );

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes, and returns, the item found at the specified index from this array.
    /// </summary>
    /// <param name="index"> The array index. </param>
    /// <returns> The item being removed. </returns>
    /// <exception cref="ArgumentOutOfRangeException"> If the supplied index is out of range. </exception>
    public virtual T RemoveIndex( int index )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( "index can't be >= size - " + index + " >= " + Size );
        }

        var value = Items[ index ];

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
    /// Remove all items from this array between, and including, the specified start and end points.
    /// </summary>
    /// <param name="start"> The start index. </param>
    /// <param name="end"> The end index. </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public virtual void RemoveRange( int start, int end )
    {
        if ( end >= Size )
        {
            throw new ArgumentOutOfRangeException( "end can't be >= size - " + end + " >= " + Size );
        }

        if ( start > end )
        {
            throw new ArgumentOutOfRangeException( "start can't be > end - " + start + " > " + end );
        }

        var count = ( end - start ) + 1;

        if ( Ordered )
        {
            Array.Copy( Items, start + count, Items, start, Size - ( start + count ) );
        }
        else
        {
            var lastIndex = Size - 1;

            for ( var i = 0; i < count; i++ )
            {
                Items[ start + i ] = Items[ lastIndex - i ];
            }
        }

        Size -= count;
    }

    /// <summary>
    /// Removes all items from this array that match items in the supplied
    /// array at the same index.
    /// </summary>
    /// <returns> <b>True</b> if successful. </returns>
    public virtual bool RemoveAll( Array< T > array )
    {
        var size      = Size;
        var startSize = size;

        for ( int i = 0, n = array.Size; i < n; i++ )
        {
            var item = array.GetAt( i );

            for ( var ii = 0; ii < size; ii++ )
            {
                if ( ( item != null ) && item.Equals( Items[ ii ] ) )
                {
                    RemoveIndex( ii );
                    size--;

                    break;
                }
            }
        }

        return size != startSize;
    }

    /// <summary>
    /// Removes and returns the last item.
    /// </summary>
    /// <exception cref="NullReferenceException"> If the array size is zero. </exception>
    public virtual T Pop()
    {
        if ( Size == 0 )
        {
            throw new IndexOutOfRangeException( "Array is empty." );
        }

        --Size;

        var item = Items[ Size ];

        Items[ Size ] = default( T )!;

        return item;
    }

    /// <summary>
    /// Returns the last item in the array.
    /// </summary>
    /// <exception cref="NullReferenceException">Thrown if the array size is zero.</exception>
    public virtual T Peek()
    {
        if ( Size == 0 )
        {
            throw new NullReferenceException( "Array is empty." );
        }

        return Items[ Size - 1 ];
    }

    /// <summary>
    /// Returns the first item in the array.
    /// </summary>
    /// <exception cref="NullReferenceException">Thrown if the array size is zero.</exception>
    public T First()
    {
        if ( Size == 0 )
        {
            throw new NullReferenceException( "Array is empty." );
        }

        return Items[ 0 ];
    }

    /// <summary>
    /// Clears this array.
    /// </summary>
    public virtual void Clear()
    {
        Array.Clear( Items );

        Size = 0;
    }

    /// <summary>
    /// Shrinks the backing array for this Array to the specified size.
    /// All items beyond the new size are lost.
    /// </summary>
    /// <returns> The resized backing array. </returns>
    public virtual T?[] Shrink()
    {
        if ( Items.Length != Size )
        {
            Resize( Size );
        }

        return Items;
    }

    /// <summary>
    /// Sets the size of this array to the new specified value.
    /// </summary>
    /// <returns> The resized backing array. </returns>
    public virtual T?[] SetSize( int newSize )
    {
        Truncate( newSize );

        if ( newSize > Items.Length )
        {
            Resize( Math.Max( 8, newSize ) );
        }

        Size = newSize;

        return Items;
    }

    /// <summary>
    /// Ensures the backing array has enough room for the requested additional capacity.
    /// </summary>
    /// <returns> The resized backing array. </returns>
    public virtual T?[] EnsureCapacity( int additionalCapacity )
    {
        var sizeNeeded = Size + additionalCapacity;

        if ( sizeNeeded > Items.Length )
        {
            Resize( Math.Max( 8, sizeNeeded ) );
        }

        return Items;
    }

    /// <summary>
    /// Resize the backing array to the given new size.
    /// </summary>
    /// <param name="newSize"> The new backing array size. </param>
    /// <returns> The resized backing array. </returns>
    protected virtual T[] Resize( int newSize )
    {
        var newItems = ( T[] ) Array.CreateInstance( Items.GetType(), newSize );

        Array.Copy( Items, 0, newItems, 0, Math.Min( Size, newItems.Length ) );

        Items = newItems;

        return newItems;
    }

    /// <summary>
    /// Sort the backing array, using the default comparer.
    /// </summary>
    public virtual void Sort()
    {
        SortUtils.Sort( Items, 0, Size );
    }

    /// <summary>
    /// Sort the backing array, using the specified comparer.
    /// </summary>
    /// <param name="comparator"> The comparer to use. </param>
    public virtual void Sort( IComparer< T > comparator )
    {
        SortUtils.Sort( Items, comparator, 0, Size );
    }

    /// <summary>
    /// Selects the kth ranked item from the backing array, using the specified
    /// comparer.
    /// </summary>
    /// <param name="comparator"> The comparer to use. </param>
    /// <param name="kthLowest"> The rank. </param>
    /// <returns> The selected item. </returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public virtual T SelectRanked( IComparer< T > comparator, int kthLowest )
    {
        if ( kthLowest < 1 )
        {
            throw new GdxRuntimeException( "nth_lowest must be greater than 0, 1 = first, 2 = second..." );
        }

        return Selector< T >.Instance.Select( Items, comparator, kthLowest, Size );
    }

    /// <summary>
    /// Selects the Kth lowest ranked object using the specified comparator.
    /// </summary>
    /// <param name="comparator"> Used for comparison. </param>
    /// <param name="kthLowest">
    /// Rank of desired object according to comparison, n is based on ordinal numbers, not
    /// array indices. for min value use 1, for max value use size of array, using 0 results
    /// in runtime exception.
    /// </param>
    /// <returns> The index of the Nth lowest ranked object. </returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public virtual int SelectRankedIndex( IComparer< T > comparator, int kthLowest )
    {
        if ( kthLowest < 1 )
        {
            throw new GdxRuntimeException( "nth_lowest must be greater than 0, 1 = first, 2 = second..." );
        }

        return Selector< T >.Instance.SelectIndex( Items, comparator, kthLowest, Size );
    }

    /// <summary>
    /// Rearrange this array in reverse order.
    /// </summary>
    public virtual void Reverse()
    {
        for ( int i = 0, lastIndex = Size - 1, n = Size / 2; i < n; i++ )
        {
            var ii = lastIndex - i;

            ( Items[ i ], Items[ ii ] ) = ( Items[ ii ], Items[ i ] );
        }
    }

    /// <summary>
    /// Shuffle this array.
    /// </summary>
    public virtual void Shuffle()
    {
        for ( var i = Size - 1; i >= 0; i-- )
        {
            var ii = MathUtils.Random( i );

            ( Items[ i ], Items[ ii ] ) = ( Items[ ii ], Items[ i ] );
        }
    }

    /// <summary>
    /// Returns an iterable for the selected items in the array. Remove is
    /// supported, but not between hasNext() and next().
    /// <para>
    /// If Collections.allocateIterators is false, the same iterable instance
    /// is returned each time this method is called. Use the Predicate.
    /// PredicateIterable constructor for nested or multithreaded iteration.
    /// </para>
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual IEnumerable< T > Select( IPredicate< T > predicate )
    {
        if ( Items == null )
        {
            throw new NullReferenceException( "Items cannot be null at this point" );
        }

        if ( _predicateIEnumerable == null )
        {
            _predicateIEnumerable = new PredicateIterable< T >( Items, predicate );
        }
        else
        {
            _predicateIEnumerable.Set( Items, predicate );
        }

        return _predicateIEnumerable;
    }

    /// <summary>
    /// Truncates this array to the specified new size, if the new size
    /// is smaller than the current size.
    /// </summary>
    public virtual void Truncate( int newSize )
    {
        if ( Size <= newSize )
        {
            return;
        }

        for ( var i = newSize; i < Size; i++ )
        {
            Items[ i ] = default( T )!;
        }

        Size = newSize;
    }

    /// <summary>
    /// Returns a random element from the array.
    /// </summary>
    [MustUseReturnValue]
    public virtual T? Random()
    {
        return Size == 0 ? default( T ) : Items[ MathUtils.Random( 0, Size - 1 ) ];
    }

    /// <summary>
    /// Returns the <see cref="Items"/> backing array as a one-dimensional array
    /// </summary>
    /// <returns> The new array. </returns>
    [MustUseReturnValue]
    public virtual T[] ToArray()
    {
        var memberInfo = Items.GetType().BaseType;

        return memberInfo != null
                   ? ToArray( memberInfo )
                   : ( T[] ) Array.CreateInstance( Items.GetType(), Size );
    }

    /// <summary>
    /// Returns the <see cref="Items"/> backing array as a one-dimensional array
    /// of the specified Type.
    /// </summary>
    /// <param name="type"> The <see cref="Type"/> of the array to create. </param>
    /// <returns> The new array. </returns>
    [MustUseReturnValue]
    public virtual T[] ToArray( Type type )
    {
        var result = ( T[] ) Array.CreateInstance( type, Size );

        Array.Copy( Items, 0, result, 0, Size );

        return result;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var h = 31 * GetType().GetHashCode();
        h *= 67 + 689696484;

        return h;
    }

    /// <inheritdoc />
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

        var array = ( Array< T > ) obj!;

        if ( !array.Ordered )
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
            if ( Items[ i ]!.Equals( array.Items[ i ] ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if ( Size == 0 )
        {
            return "[]";
        }

        var buffer = new StringBuilder( 32 );

        buffer.Append( '[' );
        buffer.Append( Items[ 0 ] );

        for ( var i = 1; i < Size; i++ )
        {
            buffer.Append( ", " );
            buffer.Append( Items[ i ] );
        }

        buffer.Append( ']' );

        return buffer.ToString();
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <param name="separator"> Character(s) to use as seperators between items. </param>
    /// <returns> A string that represents the current object. </returns>
    public virtual string ToString( string separator )
    {
        if ( Size == 0 )
        {
            return "";
        }

        var buffer = new StringBuilder( 32 );

        buffer.Append( Items[ 0 ] );

        for ( var i = 1; i < Size; i++ )
        {
            buffer.Append( separator );
            buffer.Append( Items[ i ] );
        }

        return buffer.ToString();
    }
}
