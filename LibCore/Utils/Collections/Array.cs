// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using System.Text;

using LughSharp.LibCore.Maths;

namespace LughSharp.LibCore.Utils.Collections;

[PublicAPI]
public class Array<T>
{
    private PredicateIterable< T >? _predicateIEnumerable;

    /// <summary>
    ///     Creates a new Array with the specified initial capacity. Default is 16.
    /// </summary>
    /// <param name="ordered">
    ///     If false, methods that remove elements may change the order of other
    ///     elements in the array, which avoids a memory copy.
    /// </param>
    /// <param name="capacity">
    ///     The initial capacity.
    ///     Any elements added beyond this will cause the backing array to be grown.
    /// </param>
    public Array( bool ordered = true, int capacity = 16 )
    {
        Ordered = ordered;
        Items   = new T[ capacity ];
    }

    /// <summary>
    ///     Creates a new Array object from the supplied Array.
    /// </summary>
    /// <param name="array"> The array to copy. </param>
    public Array( Array< T > array )
        : this( array.Ordered, array.Items, 0, array.Size )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    public Array( T[] array ) : this( true, array, 0, array.Length )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="ordered">
    ///     If false, methods that remove elements may change the order of other
    ///     elements in the array, which avoids a memory copy.
    /// </param>
    /// <param name="array"></param>
    /// <param name="start"></param>
    /// <param name="count"></param>
    public Array( bool ordered, T[] array, int start, int count )
    {
        ArgumentNullException.ThrowIfNull( array );

        Ordered = ordered;
        Size    = count;
        Items   = new T[ Size ];

        Array.Copy( array, start, Items, 0, Size );
    }

    /// <summary>
    ///     Adds the specified value to this array.
    /// </summary>
    /// <param name="value"></param>
    public virtual void Add( T value )
    {
        if ( Size == Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int )( Size * 1.75f ) ) );
        }

        Items[ Size++ ] = value;
    }

    /// <summary>
    ///     Adds all items in the supplied array to this array.
    /// </summary>
    /// <param name="array"></param>
    public void AddAll( Array< T > array )
    {
        AddAll( array, 0, array.Size );
    }

    /// <summary>
    ///     Copy items from the supplied array to this array,
    ///     starting from position 0.
    /// </summary>
    /// <param name="array">The array of items to add.</param>
    public void AddAll( params T[] array )
    {
        AddAll( array, 0, array.Length );
    }

    /// <summary>
    ///     Copy 'count' items from the supplied array to this array,
    ///     starting from position 'start'.
    /// </summary>
    /// <param name="array">The array of items to add.</param>
    /// <param name="start">The start index.</param>
    /// <param name="count">The number of items to copy.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void AddAll( Array< T > array, int start, int count )
    {
        ArgumentNullException.ThrowIfNull( array );

        if ( ( start + count ) > array.Size )
        {
            throw new ArgumentOutOfRangeException
                ( $"start + count must be <= size - {start} + {count} <= {array.Size}" );
        }

        AddAll( array.Items, start, count );
    }

    /// <summary>
    ///     Copy 'count' items from the supplied array to this array,
    ///     starting from position 'start'.
    /// </summary>
    /// <param name="array">The array of items to add.</param>
    /// <param name="start">The start index.</param>
    /// <param name="count">The number of items to copy.</param>
    public virtual void AddAll( T?[] array, int start, int count )
    {
        var sizeNeeded = Size + count;

        if ( sizeNeeded > Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int )( sizeNeeded * 1.75f ) ) );
        }

        Array.Copy( array, start, Items, Size, count );

        Size += count;
    }

    /// <summary>
    ///     Gets the array item at the specified index.
    /// </summary>
    /// <param name="index"> The index into the array. Must be in the range 0 - Size-1. </param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public virtual T GetAt( int index )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( nameof( index ), $@"index can't be >= size - {index} >= {Size}" );
        }

        if ( index < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( index ), @"index cannot be less than 0." );
        }

        return Items[ index ];
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public virtual void Set( int index, T value )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( "index can't be >= size - " + index + " >= " + Size );
        }

        Items[ index ] = value;
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public void InsertRange( int index, int count )
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
            Items = Resize( Math.Max( Math.Max( 8, sizeNeeded ), ( int )( Size * 1.75f ) ) );
        }

        Array.Copy( Items, index, Items, index + count, Size - index );

        Size = sizeNeeded;
    }

    /// <summary>
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    ///     Returns TRUE if this array contains the requested value.
    /// </summary>
    public virtual bool Contains( T? value )
    {
        return ( Size != 0 ) && ( IndexOf( value ) >= 0 );
    }

    /// <summary>
    ///     Returns the index of the first occurance of the requested value in this array.
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
    ///     Returns the index of the last occurance of the requested value in this array.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int LastIndexOf( T? value )
    {
        return Array.LastIndexOf( Items, value );
    }

    /// <summary>
    ///     Removes the first occurance of the requested value from this array.
    /// </summary>
    /// <param name="value"> The value to remove. </param>
    /// <returns> True if successful. </returns>
    public bool RemoveValue( T value )
    {
        for ( int i = 0, n = Size; i < n; i++ )
        {
            if ( value!.Equals( Items[ i ] ) )
            {
                RemoveIndex( i );

                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Removes, and returns, the item found at the specified index from this array.
    /// </summary>
    /// <param name="index"> The array index. </param>
    /// <returns> The item being removed. </returns>
    /// <exception cref="ArgumentOutOfRangeException"> If the supplied index is out of range. </exception>
    public T RemoveIndex( int index )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( "index can't be >= size - " + index + " >= " + Size );
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
    ///     Remove all items from this array between, and including, the specified start and end points.
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
    ///     Removes all items from this array that match items in the supplied
    ///     array at the same index.
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public bool RemoveAll( Array< T > array )
    {
        var size      = Size;
        var startSize = size;

        for ( int i = 0, n = array.Size; i < n; i++ )
        {
            T item = array.GetAt( i );

            for ( var ii = 0; ii < size; ii++ )
            {
                if ( item!.Equals( Items[ ii ] ) )
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
    ///     Removes and returns the last item.
    /// </summary>
    /// <exception cref="NullReferenceException"> If the array size is zero. </exception>
    public virtual T Pop()
    {
        if ( Size == 0 )
        {
            throw new IndexOutOfRangeException( "Array is empty." );
        }

        --Size;

        T item = Items[ Size ];

        Items[ Size ] = default( T )!;

        return item;
    }

    /// <summary>
    ///     Returns the last item in the array.
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
    ///     Returns the first item in the array.
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
    /// </summary>
    public virtual void Clear()
    {
        Array.Clear( Items );

        Size = 0;
    }

    /// <summary>
    ///     Shrinks the backing array for this Array to the specified size.
    ///     All items beyond the new size are lost.
    /// </summary>
    /// <returns></returns>
    public T?[] Shrink()
    {
        if ( Items.Length != Size )
        {
            Resize( Size );
        }

        return Items;
    }

    /// <summary>
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns></returns>
    public T?[] SetSize( int newSize )
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
    /// </summary>
    /// <param name="additionalCapacity"></param>
    /// <returns></returns>
    public T?[] EnsureCapacity( int additionalCapacity )
    {
        var sizeNeeded = Size + additionalCapacity;

        if ( sizeNeeded > Items.Length )
        {
            Resize( Math.Max( 8, sizeNeeded ) );
        }

        return Items;
    }

    /// <summary>
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns></returns>
    protected virtual T[] Resize( int newSize )
    {
        var newItems = ( T[] )Array.CreateInstance( Items.GetType(), newSize );

        Array.Copy( Items, 0, newItems, 0, Math.Min( Size, newItems.Length ) );

        Items = newItems;

        return newItems;
    }

    /// <summary>
    /// </summary>
    public void Sort()
    {
        SortUtils.Sort( Items, 0, Size );
    }

    /// <summary>
    /// </summary>
    /// <param name="comparator"></param>
    public void Sort( IComparer< T > comparator )
    {
        SortUtils.Sort( Items, comparator, 0, Size );
    }

    /// <summary>
    /// </summary>
    /// <param name="comparator"></param>
    /// <param name="kthLowest"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public T SelectRanked( IComparer< T > comparator, int kthLowest )
    {
        if ( kthLowest < 1 )
        {
            throw new GdxRuntimeException( "nth_lowest must be greater than 0, 1 = first, 2 = second..." );
        }

        return Selector< T >.Instance.Select( Items, comparator, kthLowest, Size );
    }

    /// <summary>
    /// </summary>
    /// <param name="comparator"></param>
    /// <param name="kthLowest"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public int SelectRankedIndex( IComparer< T > comparator, int kthLowest )
    {
        if ( kthLowest < 1 )
        {
            throw new GdxRuntimeException( "nth_lowest must be greater than 0, 1 = first, 2 = second..." );
        }

        return Selector< T >.Instance.SelectIndex( Items, comparator, kthLowest, Size );
    }

    /// <summary>
    ///     Rearrange this array in reverse order.
    /// </summary>
    public void Reverse()
    {
        for ( int i = 0, lastIndex = Size - 1, n = Size / 2; i < n; i++ )
        {
            var ii = lastIndex - i;

            ( Items[ i ], Items[ ii ] ) = ( Items[ ii ], Items[ i ] );
        }
    }

    /// <summary>
    ///     Shuffle this array.
    /// </summary>
    public void Shuffle()
    {
        for ( var i = Size - 1; i >= 0; i-- )
        {
            var ii = MathUtils.Random( i );

            ( Items[ i ], Items[ ii ] ) = ( Items[ ii ], Items[ i ] );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public IEnumerable< T > Select( IPredicate< T > predicate )
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
    /// </summary>
    /// <param name="newSize"></param>
    public void Truncate( int newSize )
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
    ///     Returns a random element from the array.
    /// </summary>
    [MustUseReturnValue]
    public T? Random()
    {
        return Size == 0 ? default( T ) : Items[ MathUtils.Random( 0, Size - 1 ) ];
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [MustUseReturnValue]
    public virtual T[] ToArray()
    {
        Type? memberInfo = Items.GetType().BaseType;

        return memberInfo != null
            ? ToArray( memberInfo )
            : ( T[] )Array.CreateInstance( Items.GetType(), Size );
    }

    /// <summary>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [MustUseReturnValue]
    public virtual T[] ToArray( Type type )
    {
        var result = ( T[] )Array.CreateInstance( type, Size );

        Array.Copy( Items, 0, result, 0, Size );

        return result;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        var h = 31 * GetType().GetHashCode();
        h *= 67 + 689696484;

        return h;
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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

        var array = ( Array< T > )obj!;

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
            T o1 = Items[ i ];
            T o2 = array.Items[ i ];

            if ( !( o1?.Equals( o2 ) ?? ( o2 == null ) ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="separator"></param>
    /// <returns></returns>
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

    #region properties

    public T[]  Items   { get; set; }
    public int  Size    { get; set; }
    public bool Ordered { get; set; }

    #endregion properties
}
