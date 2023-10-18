// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Collections;

namespace LibGDXSharp.Core.Utils.Collections;

[PublicAPI]
public class ObjectSet<T> : IEnumerable< T >
{
    public int  Size     { get; set; }
    public T[]? KeyTable { get; set; }

    private float _loadFactor;
    private int   _threshold;

    /// <summary>
    /// Used by <see cref="Place(Object)"/> to bit shift the upper bits of a
    /// <see cref="long"/> into a usable range (&gt;= 0 and &lt;= <see cref="mask"/>).
    /// The shift can be negative, which is convenient to match the number of
    /// bits in mask: if mask is a 7-bit number, a shift of -7 shifts the upper
    /// 7 bits into the lowest 7 positions.
    /// <para>
    /// <see cref="mask"/> can also be used to mask the low bits of a number,
    /// which may be faster for some hashcodes, if <see cref="Place(Object)"/>
    /// is overridden.
    /// </para>
    /// </summary>
    protected int shift;

    /// <summary>
    /// A bitmask used to confine hashcodes to the size of the table. Must
    /// be all 1 bits in its low positions, ie a power of two minus 1. If
    /// <see cref="Place(Object)"/>" is overriden, this can be used instead
    /// of <see cref="shift"/>" to isolate usable bits of a hash.
    /// </summary>
    protected int mask;

    private ObjectSetIterator _iterator1;
    private ObjectSetIterator _iterator2;

    /// <summary>
    /// Creates a new set with the specified initial capacity and load
    /// factor. This set will hold initialCapacity items before growing
    /// the backing table.
    /// </summary>
    /// <param name="initialCapacity">
    /// If not a power of two, it is increased to the next nearest power of two.
    /// Default value is 51.
    /// </param>
    /// <param name="loadFactor">Default value is 0.8f</param>
    public ObjectSet( int initialCapacity = 51, float loadFactor = 0.8f )
    {
        if ( ( loadFactor <= 0f ) || ( loadFactor >= 1f ) )
        {
            throw new ArgumentException( "loadFactor must be > 0 and < 1: " + loadFactor );
        }

        this._loadFactor = loadFactor;

        int tableSize = TableSize( initialCapacity, loadFactor );

        _threshold = ( int )( tableSize * loadFactor );
        mask       = tableSize - 1;
        shift      = ( int )long.LeadingZeroCount( mask );

        KeyTable = new T[ tableSize ];
    }

    /// <summary>
    /// Creates a new set identical to the specified set.
    /// </summary>
    public ObjectSet( ObjectSet< T > set )
        : this( ( int )( set.KeyTable!.Length * set._loadFactor ), set._loadFactor )
    {
        Array.Copy( set.KeyTable, 0, KeyTable!, 0, set.KeyTable.Length );
        Size = set.Size;
    }

    /// <summary>
    /// Returns an index greater than or equal to 0 and less than or equal to
    /// <see cref="mask"/> for the specified <paramref name="item"/>.
    /// </summary>
    /// <remarks>
    /// The default implementation uses Fibonacci hashing on the item's
    /// <see cref="object.GetHashCode"/>. The hash code is multiplied by a
    /// long constant (2 to the 64th, divided by the golden ratio) then the
    /// uppermost bits are shifted into the lowest positions to obtain an
    /// index in the desired range. Multiplication by a long may be slower
    /// than int but greatly improves rehashing, allowing even very poor hash
    /// codes, such as those that only differ in their upper bits, to be used
    /// without high collision rates.
    /// <para>
    /// Fibonacci hashing has increased collision rates when all or most hash
    /// codes are multiples of larger Fibonacci numbers.
    /// </para>
    /// </remarks>
    /// <param name="item">The item for which the index is computed.</param>
    /// <returns>
    /// An index greater than or equal to 0 and less than or equal to <see cref="mask"/>.
    /// </returns>
    /// <remarks>
    /// This method can be overridden to customize hashing. This may be useful,
    /// for example, in the unlikely event that most hash codes are Fibonacci
    /// numbers, if keys provide poor or incorrect hash codes, or to simplify
    /// hashing if keys provide high-quality hash codes and don't need Fibonacci
    /// hashing. The default implementation is:
    /// <para>
    /// <code>return item.GetHashCode() &amp; Mask;</code>
    /// </para>
    /// </remarks>
    protected int Place( T item )
    {
        return ( int )( ( ( ulong )item!.GetHashCode() * 0x9E3779B97F4A7C15L ) >>> shift );
    }

    /// <summary>
    /// Returns the index of the key if already present, else -(index + 1)
    /// for the next empty index.
    /// <para>
    /// This can be overridden in this pacakge to compare for equality
    /// differently than <see cref="object.Equals(object)"/>.
    /// </para>
    /// </summary>
    private int LocateKey( T key )
    {
        ArgumentNullException.ThrowIfNull( key );

        GdxRuntimeException.ThrowIfNull( KeyTable, "KeyTable cannot be null." );

        for ( var i = Place( key );; i = ( i + 1 ) & mask )
        {
            T other = this.KeyTable[ i ];

            if ( other == null )
            {
                // Empty space is available.
                return -( i + 1 );
            }

            if ( other.Equals( key ) )
            {
                // Same key was found.
                return i;
            }
        }
    }

    /// <summary>
    /// Returns true if the key was not already in the set. If this set
    /// already contains the key, the call leaves the set unchanged and
    /// returns false.
    /// </summary>
    public bool Add( T key )
    {
        GdxRuntimeException.ThrowIfNull( KeyTable, "KeyTable cannot be null." );

        var i = LocateKey( key );

        if ( i >= 0 )
        {
            // Existing key was found.
            return false;
        }

        i = -( i + 1 ); // Empty space was found.

        KeyTable[ i ] = key;

        if ( ++Size >= _threshold )
        {
            resize( KeyTable.Length << 1 );
        }

        return true;
    }

    public void AddAll( List< T > array )
    {
        AddAll( array.ToArray(), 0, array.Count );
    }

    public void AddAll( List< T > array, int offset, int length )
    {
        if ( ( offset + length ) > array.Count )
        {
            throw new ArgumentException
                ( $"offset + length must be <= size: {offset} + {length} <= {array.Count}" );
        }

        AddAll( array.ToArray(), offset, length );
    }

    public bool AddAll( params T[] array )
    {
        return AddAll( array, 0, array.Length );
    }

    public bool AddAll( T[] array, int offset, int length )
    {
        ensureCapacity( length );

        var oldSize = Size;

        for ( int i = offset, n = i + length; i < n; i++ )
        {
            Add( array[ i ] );
        }

        return oldSize != Size;
    }

    public void AddAll( ObjectSet< T > set )
    {
        ensureCapacity( set.Size );

        var n = set.KeyTable?.Length;

        for ( var i = 0; i < n; i++ )
        {
            T key = set.KeyTable![ i ];

            if ( key != null )
            {
                Add( key );
            }
        }
    }

    /// <summary>
    /// Skips checks for existing keys, doesn't increment size.
    /// </summary>
    private void AddResize( T key )
    {
        for ( var i = Place( key );; i = ( i + 1 ) & mask )
        {
            if ( this.KeyTable![ i ] == null )
            {
                this.KeyTable[ i ] = key;

                return;
            }
        }
    }

    /// <summary>
    /// Returns true if the key was removed.
    /// </summary>
    public bool Remove( T key )
    {
        GdxRuntimeException.ThrowIfNull( KeyTable, "KeyTable cannot be null." );

        var i = LocateKey( key );

        if ( i < 0 )
        {
            return false;
        }

        var next = ( i + 1 ) & mask;

        while ( ( key = this.KeyTable[ next ] ) != null )
        {
            var placement = Place( key );

            if ( ( ( next - placement ) & this.mask ) > ( ( i - placement ) & this.mask ) )
            {
                this.KeyTable[ i ] = key;

                i = next;
            }

            next = ( next + 1 ) & mask;
        }

        this.KeyTable[ i ] = default( T )!;
        Size--;

        return true;
    }

    /// <summary>
    /// Returns true if the set has one or more items.
    /// </summary>
    public bool NotEmpty() => Size > 0;

    /// <summary>
    /// Returns true if the set is empty.
    /// </summary>
    public bool IsEmpty() => Size == 0;

    /// <summary>
    /// Reduces the size of the backing arrays to be the specified
    /// capacity / loadFactor, or less. If the capacity is already less,
    /// nothing is done.
    /// <para>
    /// If the set contains more items than the specified capacity, the
    /// next highest power of two capacity is used instead.
    /// </para>
    /// </summary>
    public void Shrink( int maximumCapacity )
    {
        if ( maximumCapacity < 0 )
        {
            throw new ArgumentException( "maximumCapacity must be >= 0: " + maximumCapacity );
        }

        int tableSize = TableSize( maximumCapacity, _loadFactor );

        if ( KeyTable?.Length > tableSize )
        {
            resize( tableSize );
        }
    }

    /** Clears the set and reduces the size of the backing arrays to be the specified capacity / loadFactor, if they are larger.
     * The reduction is done by allocating new arrays, though for large arrays this can be faster than clearing the existing
     * array. */
    public void clear( int maximumCapacity )
    {
        int tableSize = tableSize( maximumCapacity, loadFactor );

        if ( keyTable.length <= tableSize )
        {
            clear();

            return;
        }

        size = 0;
        resize( tableSize );
    }

    /** Clears the set, leaving the backing arrays at the current capacity. When the capacity is high and the population is low,
     * iteration can be unnecessarily slow. {@link #clear(int)} can be used to reduce the capacity. */
    public void clear()
    {
        if ( size == 0 )
        {
            return;
        }

        size = 0;
        Arrays.fill( keyTable, null );
    }

    public boolean contains( T key )
    {
        return locateKey( key ) >= 0;
    }

    public @Null T get( T key )
    {
        int i = locateKey( key );

        return i < 0 ? null : keyTable[ i ];
    }

    public T first()
    {
        T[] keyTable = this.keyTable;

        for ( int i = 0, n = keyTable.length; i < n; i++ )
        {
            if ( keyTable[ i ] != null )
            {
                return keyTable[ i ];
            }
        }

        throw new IllegalStateException( "ObjectSet is empty." );
    }

    /** Increases the size of the backing array to accommodate the specified number of additional items / loadFactor. Useful before
     * adding many items to avoid multiple backing array resizes. */
    public void ensureCapacity( int additionalCapacity )
    {
        int tableSize = tableSize( size + additionalCapacity, loadFactor );
        if ( keyTable.length < tableSize )
        {
            resize( tableSize );
        }
    }

    private void resize( int newSize )
    {
        int oldCapacity = keyTable.length;
        threshold = ( int )( newSize * loadFactor );
        mask      = newSize - 1;
        shift     = Long.numberOfLeadingZeros( mask );
        T[] oldKeyTable = keyTable;

        keyTable = ( T[] )( new Object[ newSize ] );

        if ( size > 0 )
        {
            for ( var i = 0; i < oldCapacity; i++ )
            {
                T key = oldKeyTable[ i ];
                if ( key != null )
                {
                    addResize( key );
                }
            }
        }
    }

    public int hashCode()
    {
        int h        = size;
        T[] keyTable = this.keyTable;

        for ( int i = 0, n = keyTable.length; i < n; i++ )
        {
            T key                = keyTable[ i ];
            if ( key != null )
            {
                h += key.hashCode();
            }
        }

        return h;
    }

    public boolean equals( Object obj )
    {
        if ( !( obj instanceof ObjectSet)) return false;
        var                    other = ( ObjectSet )obj;

        if ( other.size != size )
        {
            return false;
        }

        T[] keyTable = this.keyTable;

        for ( int i = 0, n = keyTable.length; i < n; i++ )
        {
            if ( keyTable[ i ] != null && !other.contains( keyTable[ i ] ) )
            {
                return false;
            }
        }

        return true;
    }

    public String toString()
    {
        return '{' + toString( ", " ) + '}';
    }

    public String toString( String separator )
    {
        if ( size == 0 )
        {
            return "";
        }

        java.lang.StringBuilder buffer   = new java.lang.StringBuilder( 32 );
        T[]                     keyTable = this.keyTable;
        int                     i        = keyTable.length;

        while ( i-- > 0 )
        {
            T key = keyTable[ i ];

            if ( key == null )
            {
                continue;
            }

            buffer.append( key == this ? "(this)" : key );

            break;
        }

        while ( i-- > 0 )
        {
            T key = keyTable[ i ];

            if ( key == null )
            {
                continue;
            }

            buffer.append( separator );
            buffer.append( key == this ? "(this)" : key );
        }

        return buffer.toString();
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator< T > GetEnumerator()
    {
        yield break;
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can
    /// be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
