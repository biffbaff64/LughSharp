// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Utils.Collections;

/// <summary>
/// A resizable, ordered or unordered byte array. If unordered, this class
/// avoids a memory copy when removing elements (the last element is moved
/// to the removed element's position).
/// </summary>
[PublicAPI]
public class ByteArray
{
    public byte[] Items   { get; set; }
    public int    Size    { get; set; }
    public bool   Ordered { get; set; }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates an ordered array with a capacity of 16.
    /// </summary>
    public ByteArray( int capacity )
        : this( true, capacity )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="ordered">
    /// If false, methods that remove elements may change the order of other
    /// elements in the array, which avoids a memory copy.
    /// </param>
    /// <param name="capacity">
    /// Any elements added beyond this will cause the backing array to be grown.
    /// </param>
    public ByteArray( bool ordered = true, int capacity = 16 )
    {
        this.Ordered = ordered;
        Items        = new byte[ capacity ];
    }

    /// <summary>
    /// Creates a new array containing the elements in the specific array. The new
    /// array will be ordered if the specific array is ordered. The capacity is set
    /// to the number of elements, so any subsequent elements added will cause the
    /// backing array to be grown.
    /// </summary>
    public ByteArray( ByteArray array )
    {
        this.Ordered = array.Ordered;
        Size         = array.Size;
        Items        = new byte[ Size ];

        Array.Copy( array.Items, 0, Items, 0, Size );
    }

    /// <summary>
    /// Creates a new ordered array containing the elements in the specified array.
    /// The capacity is set to the number of elements, so any subsequent elements
    /// added will cause the backing array to be grown.
    /// </summary>
    public ByteArray( byte[] array )
        : this( true, array, 0, array.Length )
    {
    }

    /// <summary>
    /// Creates a new array containing the elements in the specified array. The
    /// capacity is set to the number of elements, so any subsequent elements
    /// added will cause the backing array to be grown.
    /// </summary>
    /// <param name="ordered">
    /// If false, methods that remove elements may change the order of other
    /// elements in the array, which avoids a memory copy
    /// </param>
    /// <param name="array"></param>
    /// <param name="startIndex"></param>
    /// <param name="count"></param>
    public ByteArray( bool ordered, byte[] array, int startIndex, int count )
        : this( ordered, count )
    {
        Size = count;
        Array.Copy( array, startIndex, Items, 0, count );
    }

    public void Add( byte value )
    {
        var items = this.Items;

        if ( Size == items.Length )
        {
            items = Resize( Math.Max( 8, ( int ) ( Size * 1.75f ) ) );
        }

        items[ Size++ ] = value;
    }

    public void Add( byte value1, byte value2 )
    {
        var items = this.Items;

        if ( ( Size + 1 ) >= items.Length )
        {
            items = Resize( Math.Max( 8, ( int ) ( Size * 1.75f ) ) );
        }

        items[ Size ]     =  value1;
        items[ Size + 1 ] =  value2;
        Size              += 2;
    }

    public void Add( byte value1, byte value2, byte value3 )
    {
        var items = this.Items;

        if ( ( Size + 2 ) >= items.Length )
        {
            items = Resize( Math.Max( 8, ( int ) ( Size * 1.75f ) ) );
        }

        items[ Size ]     =  value1;
        items[ Size + 1 ] =  value2;
        items[ Size + 2 ] =  value3;
        Size              += 3;
    }

    public void Add( byte value1, byte value2, byte value3, byte value4 )
    {
        var items = this.Items;

        if ( ( Size + 3 ) >= items.Length )
        {
            items = Resize( Math.Max( 8, ( int ) ( Size * 1.8f ) ) ); // 1.75 isn't enough when size=5.
        }

        items[ Size ]     =  value1;
        items[ Size + 1 ] =  value2;
        items[ Size + 2 ] =  value3;
        items[ Size + 3 ] =  value4;
        Size              += 4;
    }

    public void AddAll( ByteArray array )
    {
        AddAll( array.Items, 0, array.Size );
    }

    public void AddAll( ByteArray array, int offset, int length )
    {
        if ( ( offset + length ) > array.Size )
        {
            throw new ArgumentException( $"offset + length must be <= size: {offset} + {length} <= {array.Size}" );
        }
        
        AddAll( array.Items, offset, length );
    }

    public void AddAll( params byte[] array )
    {
        AddAll( array, 0, array.Length );
    }

    public void AddAll( byte[] array, int offset, int length )
    {
        var items      = this.Items;
        var sizeNeeded = Size + length;

        if ( sizeNeeded > items.Length )
        {
            items = Resize( Math.Max( Math.Max( 8, sizeNeeded ), ( int ) ( Size * 1.75f ) ) );
        }

        Array.Copy( array, offset, items, Size, length );

        Size += length;
    }

    public byte Get( int index )
    {
        if ( index >= Size )
        {
            throw new IndexOutOfRangeException( $"index can't be >= size: {index} >= {Size}" );
        }

        return Items[ index ];
    }

    public void Set( int index, byte value )
    {
        if ( index >= Size )
        {
            throw new IndexOutOfRangeException( $"index can't be >= size: {index} >= {Size}" );
        }

        Items[ index ] = value;
    }

    public void Incr( int index, byte value )
    {
        if ( index >= Size )
        {
            throw new IndexOutOfRangeException( $"index can't be >= size: {index} >= {Size}" );
        }

        Items[ index ] += value;
    }

    public void Incr( byte value )
    {
        var items = this.Items;

        for ( int i = 0, n = Size; i < n; i++ )
        {
            items[ i ] += value;
        }
    }

    public void Mul( int index, byte value )
    {
        if ( index >= Size )
        {
            throw new IndexOutOfRangeException( $"index can't be >= size: {index} >= {Size}" );
        }

        Items[ index ] *= value;
    }

    public void Mul( byte value )
    {
        var items = this.Items;

        for ( int i = 0, n = Size; i < n; i++ )
        {
            items[ i ] *= value;
        }
    }

    public void Insert( int index, byte value )
    {
        if ( index > Size )
        {
            throw new IndexOutOfRangeException( $"index can't be > size: {index} > {Size}" );
        }

        var items = this.Items;

        if ( Size == items.Length )
        {
            items = Resize( Math.Max( 8, ( int ) ( Size * 1.75f ) ) );
        }

        if ( Ordered )
        {
            Array.Copy( items, index, items, index + 1, Size - index );
        }
        else
        {
            items[ Size ] = items[ index ];
        }

        Size++;
        items[ index ] = value;
    }

    /// <summary>
    /// Inserts the specified number of items at the specified index. The new items
    /// will have values equal to the values at those indices before the insertion.
    /// </summary>
    public void InsertRange( int index, int count )
    {
        if ( index > Size )
        {
            throw new IndexOutOfRangeException( $"index can't be > size: {index} > {Size}" );
        }

        var sizeNeeded = Size + count;

        if ( sizeNeeded > Items.Length )
        {
            Items = Resize( Math.Max( Math.Max( 8, sizeNeeded ), ( int ) ( Size * 1.75f ) ) );
        }

        Array.Copy( Items, index, Items, index + count, Size - index );

        Size = sizeNeeded;
    }

    public void Swap( int first, int second )
    {
        if ( first >= Size )
        {
            throw new IndexOutOfRangeException( $"first can't be >= size: {first} >= {Size}" );
        }

        if ( second >= Size )
        {
            throw new IndexOutOfRangeException( $"second can't be >= size: {second} >= {Size}" );
        }

        var items = this.Items;
        
        ( items[ first ], items[ second ] ) = ( items[ second ], items[ first ] );
    }

    public bool Contains( byte value )
    {
        var i     = Size - 1;
        var items = this.Items;
        
        while ( i >= 0 )
        {
            if ( items[ i-- ] == value )
            {
                return true;
            }
        }

        return false;
    }

    public int IndexOf( byte value )
    {
        var items = this.Items;
        
        for ( int i = 0, n = Size; i < n; i++ )
        {
            if ( items[ i ] == value )
            {
                return i;
            }
        }

        return -1;
    }

    public int LastIndexOf( byte value )
    {
        var items = this.Items;
        
        for ( var i = Size - 1; i >= 0; i-- )
        {
            if ( items[ i ] == value )
            {
                return i;
            }
        }

        return -1;
    }

    public bool RemoveValue( byte value )
    {
        var items = this.Items;

        for ( int i = 0, n = Size; i < n; i++ )
        {
            if ( items[ i ] == value )
            {
                RemoveIndex( i );
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes and returns the item at the specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public int RemoveIndex( int index )
    {
        if ( index >= Size )
        {
            throw new IndexOutOfRangeException( $"index can't be >= size: {index} >= {Size}" );
        }

        var items = this.Items;
        int value = items[ index ];

        Size--;

        if ( Ordered )
        {
            Array.Copy( items, index + 1, items, index, Size - index );
        }
        else
        {
            items[ index ] = items[ Size ];
        }

        return value;
    }

    /// <summary>
    /// Removes the items between the specified indices, inclusive.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public void RemoveRange( int start, int end )
    {
        var n = Size;

        if ( end >= n )
        {
            throw new IndexOutOfRangeException( $"end can't be >= size: {end} >= {Size}" );
        }

        if ( start > end )
        {
            throw new IndexOutOfRangeException( $"start can't be > end: {start} > {end}" );
        }

        int count = ( end - start ) + 1, lastIndex = n - count;

        if ( Ordered )
        {
            Array.Copy( Items, start + count, Items, start, n - ( start + count ) );
        }
        else
        {
            var i = Math.Max( lastIndex, end + 1 );
            Array.Copy( Items, i, Items, start, n - i );
        }

        Size = n - count;
    }

    /// <summary>
    /// Removes from this array all of elements contained in the specified array.
    /// </summary>
    /// <param name="array"></param>
    /// <returns> true if this array was modified. </returns>
    public bool RemoveAll( ByteArray array )
    {
        var size      = this.Size;
        var startSize = size;
        var items     = this.Items;

        for ( int i = 0, n = array.Size; i < n; i++ )
        {
            int item = array.Get( i );

            for ( var ii = 0; ii < size; ii++ )
            {
                if ( item == items[ ii ] )
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
    public byte Pop() => Items[ --Size ];

    /// <summary>
    /// Returns the last item.
    /// </summary>
    public byte Peek() => Items[ Size - 1 ];

    /// <summary>
    /// Returns the first item.
    /// </summary>
    public byte First()
    {
        if ( Size == 0 )
        {
            throw new GdxRuntimeException( "Array is empty." );
        }
        
        return Items[ 0 ];
    }

    /// <summary>
    /// Returns true if the array has one or more items.
    /// </summary>
    public bool NotEmpty() => Size > 0;

    /// <summary>
    /// Returns true if the array is empty.
    /// </summary>
    public bool IsEmpty() => Size == 0;

    public void Clear() => Size = 0;

    /// <summary>
    /// Reduces the size of the backing array to the size of the actual items.
    /// This is useful to release memory when many items have been removed, or
    /// if it is known that more items will not be added.
    /// </summary>
    public byte[] Shrink()
    {
        if ( Items.Length != Size ) Resize( Size );
        return Items;
    }

    /// <summary>
    /// Increases the size of the backing array to accommodate the specified
    /// number of additional items. Useful before adding many items to avoid
    /// multiple backing array resizes.
    /// </summary>
    public byte[] EnsureCapacity( int additionalCapacity )
    {
        if ( additionalCapacity < 0 )
        {
            throw new ArgumentException( $"additionalCapacity must be >= 0: {additionalCapacity}" );
        }
        
        var sizeNeeded = Size + additionalCapacity;

        if ( sizeNeeded > Items.Length )
        {
            Resize( Math.Max( Math.Max( 8, sizeNeeded ), ( int ) ( Size * 1.75f ) ) );
        }
        
        return Items;
    }

    /// <summary>
    /// Sets the array size, leaving any values beyond the current size undefined.
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public byte[] SetSize( int newSize )
    {
        if ( newSize < 0 ) throw new ArgumentException( $"newSize must be >= 0: {newSize}" );
        if ( newSize > Items.Length ) Resize( Math.Max( 8, newSize ) );

        Size = newSize;

        return Items;
    }

    protected byte[] Resize( int newSize )
    {
        var newItems = new byte[ newSize ];
        var items    = this.Items;

        Array.Copy( items, 0, newItems, 0, Math.Min( Size, newItems.Length ) );

        this.Items = newItems;

        return newItems;
    }

    public void Sort()
    {
        Array.Sort( Items, 0, Size );
    }

    public void Reverse()
    {
        var items = this.Items;

        for ( int i = 0, lastIndex = Size - 1, n = Size / 2; i < n; i++ )
        {
            var ii = lastIndex - i;

            ( items[ i ], items[ ii ] ) = ( items[ ii ], items[ i ] );
        }
    }

    public void Shuffle()
    {
        var items = this.Items;

        for ( var i = Size - 1; i >= 0; i-- )
        {
            var ii = MathUtils.Random( i );

            ( items[ i ], items[ ii ] ) = ( items[ ii ], items[ i ] );
        }
    }

    /// <summary>
    /// Reduces the size of the array to the specified size. If the array is
    /// already smaller than the specified size, no action is taken.
    /// </summary>
    public void Truncate( int newSize )
    {
        if ( Size > newSize ) Size = newSize;
    }

    /// <summary>
    /// Returns a Random item from the array, or zero if the array is empty.
    /// </summary>
    /// <returns></returns>
    public byte Random()
    {
        return Size == 0 ? ( byte ) 0 : Items[ MathUtils.Random( 0, Size - 1 ) ];
    }

    public byte[] ToArray()
    {
        var array = new byte[ Size ];
        Array.Copy( Items, 0, array, 0, Size );
        return array;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var items = this.Items;
        var h     = 1;

        for ( int i = 0, n = Size; i < n; i++ )
        {
            h = ( h * 31 ) + items[ i ];
        }

        return h;
    }

    /// <summary>
    /// Returns false if either array is unordered.
    /// </summary>
    public override bool Equals( object? obj )
    {
        if ( obj == this ) return true;
        if ( !Ordered ) return false;

        if ( obj is not ByteArray array ) return false;

        if ( !array.Ordered ) return false;

        var n = Size;

        if ( n != array.Size ) return false;

        var items1 = this.Items;
        var items2 = array.Items;

        for ( var i = 0; i < n; i++ )
        {
            if ( items1[ i ] != items2[ i ] )
                return false;
        }

        return true;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if ( Size == 0 ) return "[]";

        var items  = this.Items;
        var buffer = new StringBuilder( 32 );

        buffer.Append( '[' );
        buffer.Append( items[ 0 ] );

        for ( var i = 1; i < Size; i++ )
        {
            buffer.Append( ", " );
            buffer.Append( items[ i ] );
        }

        buffer.Append( ']' );

        return buffer.ToString();
    }

    public string ToString( string separator )
    {
        if ( Size == 0 ) return "";

        var items  = this.Items;
        var buffer = new StringBuilder( 32 );

        buffer.Append( items[ 0 ] );

        for ( var i = 1; i < Size; i++ )
        {
            buffer.Append( separator );
            buffer.Append( items[ i ] );
        }

        return buffer.ToString();
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static ByteArray With( params byte[] array )
    {
        return new ByteArray( array );
    }
}