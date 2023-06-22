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
using System.Diagnostics.CodeAnalysis;
using System.Text;

using LibGDXSharp.Maths;

namespace LibGDXSharp.Utils.Collections.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" )]
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class GdxArray<T>
{
    public T[]  Items   { get; private set; }
    public int  Size    { get; private set; }
    public bool Ordered { get; private set; }

    private IEnumerable< T >?       _iterable;
    private PredicateIterable< T >? _predicateIEnumerable;

    /// <summary>
    /// Creates a new Array with the specified initial capacity.
    /// </summary>
    /// <param name="ordered">
    /// If false, methods that remove elements may change the order of other
    /// elements in the array, which avoids a memory copy.
    /// </param>
    /// <param name="capacity">
    /// The initial capacity.
    /// Any elements added beyond this will cause the backing array to be grown.
    /// </param>
    public GdxArray( bool ordered = true, int capacity = 16 )
    {
        Ordered = ordered;
        Items   = new T[ capacity ];
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    public GdxArray( GdxArray< T > array )
    {
        if ( array == null ) throw new GdxRuntimeException( "array cannot be null!" );
        if ( array.Items == null ) throw new GdxRuntimeException( "array cannot be null!" );

        Ordered = array.Ordered;
        Size    = array.Size;
        Items   = new T[ Size ];

        Array.Copy( array.Items, 0, Items, 0, Size );
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    public GdxArray( T[] array ) : this( true, array, 0, array.Length )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="ordered"></param>
    /// <param name="array"></param>
    /// <param name="start"></param>
    /// <param name="count"></param>
    public GdxArray( bool ordered, T[] array, int start, int count )
    {
        Ordered = ordered;
        Size    = count;
        Items   = new T[ Size ];

        Array.Copy( array, start, Items, 0, Size );
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public void Add( T value )
    {
        if ( Size == Items.Length )
        {
            Items = Resize( Math.Max( 8, ( int )( Size * 1.75f ) ) );
        }

        Items[ Size++ ] = value;
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    public void AddAll( GdxArray< T > array )
    {
        AddAll( array, 0, array.Size );
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    public void AddAll( params T[] array )
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
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void AddAll( GdxArray< T > array, int start, int count )
    {
        if ( ( start + count ) > array.Size )
        {
            throw new ArgumentOutOfRangeException
                ( "start + count must be <= size - " + start + " + " + count + " <= " + array.Size );
        }

        AddAll( array.Items, start, count );
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    /// <param name="start"></param>
    /// <param name="count"></param>
    public void AddAll( T?[] array, int start, int count )
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
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public T Get( int index )
    {
        if ( index >= Size )
        {
            throw new ArgumentOutOfRangeException( "index can't be >= size - " + index + " >= " + Size );
        }

        return Items[ index ];
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Set( int index, T value )
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
    public void Insert( int index, T value )
    {
        if ( index > Size )
        {
            throw new ArgumentOutOfRangeException( "index can't be > size - " + index + " > " + Size );
        }

        if ( Items == null ) throw new GdxRuntimeException( "Items cannot be null!" );

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

        if ( Items == null ) throw new GdxRuntimeException( "Items cannot be null!" );

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
    public void Swap( int first, int second )
    {
        if ( first >= Size )
        {
            throw new ArgumentOutOfRangeException( "first can't be >= size - " + first + " >= " + Size );
        }

        if ( second >= Size )
        {
            throw new ArgumentOutOfRangeException( "second can't be >= size - " + second + " >= " + Size );
        }

        if ( Items == null ) throw new GdxRuntimeException( "Items cannot be null!" );

        ( Items[ first ], Items[ second ] ) = ( Items[ second ], Items[ first ] );
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains( T value )
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

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int IndexOf( T? value )
    {
        if ( Items == null ) throw new GdxRuntimeException( "Items cannot be null!" );

        if ( value == null )
        {
            for ( int i = 0, n = Size; i < n; i++ )
            {
                if ( Items[ i ]!.Equals( value ) )
                {
                    return i;
                }
            }
        }
        else
        {
            for ( int i = 0, n = Size; i < n; i++ )
            {
                if ( value.Equals( Items[ i ] ) )
                {
                    return i;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int LastIndexOf( T? value )
    {
        if ( value != null )
        {
            for ( var i = Size - 1; i >= 0; i-- )
            {
                if ( value.Equals( Items[ i ] ) )
                {
                    return i;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void RemoveRange( int start, int end )
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
            var lastIndex = this.Size - 1;

            for ( var i = 0; i < count; i++ )
            {
                Items[ start + i ] = Items[ lastIndex - i ];
            }
        }

        Size -= count;
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public bool RemoveAll( GdxArray< T > array )
    {
        var size      = this.Size;
        var startSize = size;

        for ( int i = 0, n = array.Size; i < n; i++ )
        {
            T item = array.Get( i );

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
    /// Removes and returns the last item.
    /// </summary>
    /// <exception cref="NullReferenceException">Thrown if the array size is zero.</exception>
    public T Pop()
    {
        if ( Size == 0 )
        {
            throw new IndexOutOfRangeException( "Array is empty." );
        }

        --Size;

        T item = Items[ Size ];

        Items[ Size ] = default!;

        return item;
    }

    /// <summary>
    /// Returns the last item in the array.
    /// </summary>
    /// <exception cref="NullReferenceException">Thrown if the array size is zero.</exception>
    public T Peek()
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
        if ( Size == 0 ) throw new NullReferenceException( "Array is empty." );

        return Items[ 0 ];
    }

    /// <summary>
    /// </summary>
    public void Clear()
    {
        Array.Clear( Items );
        
        Size = 0;
    }

    /// <summary>
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
            Resize( System.Math.Max( 8, newSize ) );
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
            Resize( System.Math.Max( 8, sizeNeeded ) );
        }

        return Items;
    }

    /// <summary>
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns></returns>
    protected T[] Resize( int newSize )
    {
        var newItems = ( T[] )Array.CreateInstance( Items.GetType(), newSize );

        Array.Copy( Items, 0, newItems, 0, System.Math.Min( Size, newItems.Length ) );

        this.Items = newItems;

        return newItems;
    }

    /// <summary>
    /// </summary>
    public void Sort()
    {
        SortUtils.Instance.Sort< T >( Items, 0, Size );
    }

    /// <summary>
    /// </summary>
    /// <param name="comparator"></param>
    public void Sort( IComparer< object > comparator )
    {
        SortUtils.Instance.Sort( Items, comparator, 0, Size );
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

        return Selector< T >.Instance().Select( Items, comparator, kthLowest, Size );
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

        return Selector< T >.Instance().SelectIndex( Items, comparator, kthLowest, Size );
    }

    /// <summary>
    /// Rearrange this array in reverse order.
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
    /// Shuffle this array.
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
    /// 
    /// </summary>
    /// <param name="newSize"></param>
    public void Truncate( int newSize )
    {
        if ( Size <= newSize ) return;

        for ( var i = newSize; i < Size; i++ )
        {
            Items[ i ] = default( T )!;
        }

        Size = newSize;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public T? Random()
    {
        if ( Size == 0 )
        {
            return default( T );
        }

        return Items[ MathUtils.Random( 0, Size - 1 ) ];
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public T[] ToArray()
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
    public T[] ToArray( Type type )
    {
        var result = ( T[] )Array.CreateInstance( type, Size );

        Array.Copy( Items, 0, result, 0, Size );

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public new int GetHashCode()
    {
        if ( !Ordered ) return base.GetHashCode();

        var h = 1;

        for ( int i = 0, n = Size; i < n; i++ )
        {
            h *= 31;

            T item = Items[ i ];

            if ( item != null )
            {
                h += item.GetHashCode();
            }
        }

        return h;
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public new bool Equals( object obj )
    {
        if ( obj == this ) return true;

        if ( !Ordered ) return false;

        var array = ( GdxArray< T > )obj;

        if ( !array.Ordered ) return false;

        var n = Size;

        if ( n != array.Size ) return false;

        for ( var i = 0; i < n; i++ )
        {
            T o1 = this.Items[ i ];
            T o2 = array.Items[ i ];

            if ( !( o1?.Equals( o2 ) ?? ( o2 == null ) ) ) return false;
        }

        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerator< T > GetEnumerator()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public new string ToString()
    {
        if ( Size == 0 ) return "[]";

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
    public string ToString( string separator )
    {
        if ( Size == 0 ) return "";

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