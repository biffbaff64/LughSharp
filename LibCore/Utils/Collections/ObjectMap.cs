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

namespace LughSharp.LibCore.Utils.Collections;

/// <summary>
///     <para>An unordered map where the keys and values are objects.</para>
///     <para>Null keys are not allowed.</para>
///     <para>No allocation is done except when growing the table size.</para>
///     <para>
///         This class performs fast contains and remove (typically O(1), worst case O(n) but
///         that is rare in practice). Add may be slightly slower, depending on hash collisions.
///         Hashcodes are rehashed to reduce collisions and the need to resize. Load factors
///         greater than 0.91 greatly increase the chances to resize to the next higher POT size.
///         Unordered sets and maps are not designed to provide especially fast iteration.
///     </para>
///     <para>
///         This implementation uses linear probing with the backward shift algorithm for removal.
///         Hashcodes are rehashed using Fibonacci hashing, instead of the more common power-of-two
///         mask, to better distribute poor hashCodes (see Malte Skarupke's blog post). Linear
///         probing continues to work even when all hashCodes collide, just more slowly.
///     </para>
/// </summary>
[PublicAPI]
public class ObjectMap< TK, TV >
{
    // ------------------------------------------------------------------------

    private readonly   object _dummy = new();
    protected readonly float  LoadFactor;

    private   Entries? _entries1;
    private   Entries? _entries2;
    private   Keys?    _keys1;
    private   Keys?    _keys2;
    private   Values?  _values1;
    private   Values?  _values2;
    protected TK?[]    KeyTable;
    protected int      Threshold;
    protected TV?[]    ValueTable;

    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="initialCapacity"></param>
    /// <param name="loadFactor"></param>
    public ObjectMap( int initialCapacity = 51, float loadFactor = 0.8f )
    {
        if ( loadFactor is <= 0f or >= 1f )
        {
            throw new ArgumentException( "loadFactor must be > 0 and < 1: " + loadFactor );
        }

        LoadFactor = loadFactor;

        var tableSize = TableSize( initialCapacity, loadFactor );

        Threshold  = ( int ) ( tableSize * loadFactor );
        Mask       = tableSize - 1;
        Shift      = int.LeadingZeroCount( Mask );
        KeyTable   = new TK[ tableSize ];
        ValueTable = new TV[ tableSize ];
    }

    /// <summary>
    ///     Copy Constructor
    /// </summary>
    /// <param name="map">The ObjectMap to copy.</param>
    /// <exception cref="ArgumentException"></exception>
    protected ObjectMap( ObjectMap< TK, TV > map )
    {
        ArgumentNullException.ThrowIfNull( map );

        LoadFactor = map.LoadFactor;

        var tableSize = TableSize( ( int ) ( map.KeyTable.Length * map.LoadFactor ),
                                   LoadFactor );

        Threshold = ( int ) ( tableSize * LoadFactor );
        Mask      = tableSize - 1;
        Shift     = int.LeadingZeroCount( Mask );

        KeyTable   = new TK[ tableSize ];
        ValueTable = new TV[ tableSize ];

        Array.Copy( map.KeyTable, 0, KeyTable, 0, map.KeyTable.Length );
        Array.Copy( map.ValueTable, 0, ValueTable, 0, map.ValueTable.Length );

        Size = map.Size;
    }

    // ------------------------------------------------------------------------

    /// <summary>
    ///     Used by <see cref="Place" /> to bit shift the upper bits of a <b>long</b>
    ///     into a usable range (&gt;= 0 and &lt;= <see cref="Mask" />).
    ///     <para>
    ///         The shift can be negative, which is convenient to match the number of bits in
    ///         mask: if mask is a 7-bit number, a shift of -7 shifts the upper 7 bits into the
    ///         lowest 7 positions. This class sets the shift &gt; 32 and &lt; 64, which if used
    ///         with an int will still move the upper bits of an int to the lower bits.
    ///     </para>
    ///     <para>
    ///         <see cref="Mask" /> can also be used to mask the low bits of a number, which may
    ///         be faster for some hashcodes if <see cref="Place" /> is overridden.
    ///     </para>
    /// </summary>
    protected int Shift { get; set; }

    /// <summary>
    ///     A bitmask used to confine hashcodes to the size of the table. Must be all
    ///     1 bits in its low positions, ie a power of two minus 1.
    ///     If <see cref="Place" /> is overriden, this can be used instead of <see cref="Shift" />
    ///     to isolate usable bits of a hash.
    /// </summary>
    protected int Mask { get; set; }

    /// <summary>
    ///     Returns the size of this ObjectMap
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    ///     Returns an index between 0 and <see cref="Mask" /> for the specified <c>item</c>.
    ///     <para>
    ///         The default implementation uses Fibonacci hashing based on the <c>item.GetHashCode()</c>.
    ///         The hash code is multiplied by a constant (2 to the 64th, divided by the golden ratio),
    ///         and the uppermost bits are shifted to obtain an index within the desired range.
    ///         This method can handle even poor hash codes, preventing high collision rates.
    ///         However, it may have increased collision rates when most hash codes are multiples
    ///         of larger Fibonacci numbers.
    ///     </para>
    ///     <para>
    ///         For more details, see
    ///         <a
    ///             href="https://probablydance.com/2018/06/16/fibonacci-hashing-the-optimization-that-the-world-forgot-or-a-better-alternative-to-integer-modulo/">
    ///             Malte
    ///             Skarupke's blog post
    ///         </a>
    ///         .
    ///     </para>
    ///     <para>
    ///         You can override this method to customize hashing. This might be useful, for instance,
    ///         in cases where most hash codes are Fibonacci numbers, when keys have poor or incorrect
    ///         hash codes, or when high-quality hash codes negate the need for Fibonacci hashing.
    ///         Example: <c>return item.GetHashCode() &amp; Mask;</c>
    ///     </para>
    /// </summary>
    /// <param name="item">The item to calculate the index for.</param>
    protected virtual int Place( TK item )
    {
        ArgumentNullException.ThrowIfNull( item );

        return ( int ) ( ( ( ulong ) item.GetHashCode() * 0x9E3779B97F4A7C15L ) >>> Shift );
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private int LocateKey( TK key )
    {
        ArgumentNullException.ThrowIfNull( key );

        if ( KeyTable == null )
        {
            throw new NullReferenceException( "_keyTable is null" );
        }

        for ( var i = Place( key ); /*..*/; i = ( i + 1 ) & Mask )
        {
            var other = KeyTable[ i ];

            if ( other == null )
            {
                return -( i + 1 ); // Empty space is available.
            }

            if ( other.Equals( key ) )
            {
                return i; // Same key was found.
            }
        }
    }

    /// <summary>
    ///     Replaces the value associated with the specified key, and returns the old value.
    ///     If the key is not found, the value is added at the end of the map and null is returned.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public TV? Put( TK key, TV? value )
    {
        ArgumentNullException.ThrowIfNull( KeyTable );
        ArgumentNullException.ThrowIfNull( ValueTable );

        var i = LocateKey( key );

        if ( i >= 0 )
        {
            // Existing key was found.
            var oldValue = ValueTable[ i ];
            ValueTable[ i ] = value;

            return oldValue;
        }

        i = -( i + 1 ); // Empty space was found.

        KeyTable[ i ]   = key;
        ValueTable[ i ] = value;

        if ( ++Size >= Threshold )
        {
            Resize( KeyTable.Length << 1 );
        }

        return default( TV? );
    }

    /// <summary>
    ///     Copies all key-value pairs from the specified <paramref name="map" />
    ///     into the current collection.
    /// </summary>
    /// <param name="map">The source map containing the key-value pairs to copy.</param>
    /// <remarks>
    ///     <para>
    ///         This method ensures that the current collection has sufficient capacity
    ///         to accommodate the key-value pairs from the <paramref name="map" />. Then,
    ///         it iterates through the key table, copying each non-null key along with
    ///         its associated value from the <paramref name="map" /> into the current
    ///         collection using the <see cref="Put" /> method.
    ///     </para>
    /// </remarks>
    /// <seealso cref="Put" />
    /// <typeparam name="TK">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TV">The type of the values in the collection.</typeparam>
    public void PutAll( ObjectMap< TK, TV > map )
    {
        EnsureCapacity( map.Size );

        for ( int i = 0, n = KeyTable.Length; i < n; i++ )
        {
            var key = KeyTable[ i ];

            if ( key != null )
            {
                Put( key, map.ValueTable[ i ] );
            }
        }
    }

    /// <summary>
    ///     Retrieves the value associated with the specified <paramref name="key" />
    ///     from the collection.
    /// </summary>
    /// <param name="key">The key to look up.</param>
    /// <returns>
    ///     The value associated with the <paramref name="key" /> if found; otherwise null.
    /// </returns>
    /// <remarks>
    ///     This method searches the collection for the given <paramref name="key" />
    ///     using the <see cref="LocateKey" /> method. If the key is found, the associated
    ///     value is returned; otherwise, <c>null</c> is returned.
    /// </remarks>
    /// <seealso cref="LocateKey" />
    /// <typeparam name="TT">The type of the key to look up.</typeparam>
    /// <typeparam name="TK">The type constraint for the key type.</typeparam>
    /// <typeparam name="TV">The type of the value to retrieve.</typeparam>
    public TV? Get< TT >( TT key ) where TT : TK
    {
        var i = LocateKey( key );

        return i < 0 ? default( TV? ) : ValueTable[ i ];
    }

    /// <summary>
    ///     Retrieves the value associated with the specified <paramref name="key" /> from the collection,
    ///     or returns the provided <paramref name="defaultValue" /> if the key is not found.
    /// </summary>
    /// <param name="key">The key to look up.</param>
    /// <param name="defaultValue">The value to return if the key is not found.</param>
    /// <returns>
    ///     The value associated with the <paramref name="key" /> if found; otherwise, the
    ///     <paramref name="defaultValue" /> is returned.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This method searches the collection for the given <paramref name="key" /> using
    ///         the <see cref="LocateKey" /> method. If the key is found, the associated value is
    ///         returned; otherwise, the provided <paramref name="defaultValue" /> is returned.
    ///     </para>
    /// </remarks>
    /// <seealso cref="LocateKey" />
    /// <typeparam name="TK">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TV">The type of the values in the collection.</typeparam>
    public TV? Get( TK key, TV? defaultValue )
    {
        var i = LocateKey( key );

        return i < 0 ? defaultValue : ValueTable[ i ];
    }

    /// <summary>
    ///     Helper method.
    /// </summary>
    /// <returns>TRUE if Size is greater than zero.</returns>
    public bool NotEmpty()
    {
        return Size > 0;
    }

    /// <summary>
    ///     Helper method.
    /// </summary>
    /// <returns>TRUE if Size is zero.</returns>
    public bool IsEmpty()
    {
        return Size == 0;
    }

    /// <summary>
    ///     Shrinks the map to the specified maximum capacity.
    /// </summary>
    /// <param name="maximumCapacity"> The new maximum capacity. </param>
    public void Shrink( int maximumCapacity )
    {
        if ( maximumCapacity < 0 )
        {
            throw new ArgumentException( "maximumCapacity must be >= 0: " + maximumCapacity );
        }

        var tableSize = TableSize( maximumCapacity, LoadFactor );

        if ( KeyTable.Length > tableSize )
        {
            Resize( tableSize );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="maximumCapacity"></param>
    public void Clear( int maximumCapacity )
    {
        var tableSize = TableSize( maximumCapacity, LoadFactor );

        if ( KeyTable.Length <= tableSize )
        {
            Clear();

            return;
        }

        Size = 0;

        Resize( tableSize );
    }

    /// <summary>
    /// </summary>
    public void Clear()
    {
        if ( Size == 0 )
        {
            return;
        }

        Size = 0;

        Array.Clear( KeyTable );
        Array.Clear( ValueTable );
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="identity"></param>
    /// <returns></returns>
    public bool ContainsValue( object? value, bool identity )
    {
        if ( value == null )
        {
            for ( var i = ValueTable.Length - 1; i >= 0; i-- )
            {
                if ( ( KeyTable[ i ] != null ) && ( ValueTable[ i ] == null ) )
                {
                    return true;
                }
            }
        }
        else
        {
            for ( var i = ValueTable.Length - 1; i >= 0; i-- )
            {
                if ( value.Equals( ValueTable[ i ] ) )
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey( TK key )
    {
        return LocateKey( key ) >= 0;
    }

    /// <summary>
    ///     Returns the key for the specified value, or null if it is not in the map.
    ///     Note this traverses the entire map and compares every value, which may be
    ///     an expensive operation.
    /// </summary>
    public TK? FindKey( object? value )
    {
        if ( value == null )
        {
            for ( var i = ValueTable.Length - 1; i >= 0; i-- )
            {
                if ( ( KeyTable[ i ] != null ) && ( ValueTable[ i ] == null ) )
                {
                    return KeyTable[ i ];
                }
            }
        }
        else
        {
            for ( var i = ValueTable.Length - 1; i >= 0; i-- )
            {
                if ( value.Equals( ValueTable[ i ] ) )
                {
                    return KeyTable[ i ];
                }
            }
        }

        return default( TK? );
    }

    /// <summary>
    ///     Increases the size of the backing array to accommodate the specified number
    ///     of additional items / loadFactor. Useful before adding many items to avoid
    ///     multiple backing array resizes.
    /// </summary>
    public void EnsureCapacity( int additionalCapacity )
    {
        var tableSize = TableSize( Size + additionalCapacity, LoadFactor );

        if ( KeyTable.Length < tableSize )
        {
            Resize( tableSize );
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        const int PRIME = 28;

        var result = PRIME + 34;
        result = ( PRIME * result ) + 43;

        return result;
    }

    public int TableSize( int capacity, float lf )
    {
        if ( capacity < 0 )
        {
            throw new ArgumentException( "capacity must be >= 0: " + capacity );
        }

        var tableSize = MathUtils.NextPowerOfTwo( Math.Max( 2, ( int ) Math.Ceiling( capacity / lf ) ) );

        if ( tableSize > ( 1 << 30 ) )
        {
            throw new ArgumentException( "The required capacity is too large: " + capacity );
        }

        return tableSize;
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public new bool Equals( object obj )
    {
        if ( obj == this )
        {
            return true;
        }

        if ( obj is not ObjectMap< TK, TV > other )
        {
            return false;
        }

        if ( other.Size != Size )
        {
            return false;
        }

        for ( int i = 0, n = KeyTable.Length; i < n; i++ )
        {
            var key = KeyTable[ i ];

            if ( key != null )
            {
                var value = ValueTable[ i ];

                if ( value == null )
                {
                    if ( other.Get( key, ( TV? ) _dummy ) != null )
                    {
                        return false;
                    }
                }
                else
                {
                    if ( !value.Equals( other.Get( key ) ) )
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public string ToString( string separator )
    {
        return ToString( separator, false );
    }

    public override string ToString()
    {
        return ToString( ", ", true );
    }

    protected string ToString( string separator, bool braces )
    {
        if ( Size == 0 )
        {
            return braces ? "{}" : "";
        }

        var buffer = new StringBuilder( 32 );

        if ( braces )
        {
            buffer.Append( '{' );
        }

        var i = KeyTable.Length;

        while ( i-- > 0 )
        {
            var key = KeyTable[ i ];

            if ( key == null )
            {
                continue;
            }

            buffer.Append( key );
            buffer.Append( '=' );

            var value = ValueTable[ i ];

            buffer.Append( value );

            break;
        }

        while ( i-- > 0 )
        {
            var key = KeyTable[ i ];

            if ( key == null )
            {
                continue;
            }

            buffer.Append( separator );
            buffer.Append( key );
            buffer.Append( '=' );

            var value = ValueTable[ i ];

            if ( Equals( value, this ) )
            {
                buffer.Append( "(this)" );
            }
            else
            {
                buffer.Append( value );
            }
        }

        if ( braces )
        {
            buffer.Append( '}' );
        }

        return buffer.ToString();
    }

    /// <summary>
    /// </summary>
    /// <param name="newSize"></param>
    public void Resize( int newSize )
    {
        var oldCapacity = KeyTable.Length;

        Threshold = ( int ) ( newSize * LoadFactor );
        Mask      = newSize - 1;
        Shift     = int.LeadingZeroCount( Mask );

        TK?[] oldKeyTable   = KeyTable;
        TV?[] oldValueTable = ValueTable;

        KeyTable   = new TK[ newSize ];
        ValueTable = new TV[ newSize ];

        if ( Size > 0 )
        {
            for ( var i = 0; i < oldCapacity; i++ )
            {
                var key = oldKeyTable[ i ];

                if ( key != null )
                {
                    PutResize( key, oldValueTable[ i ] );
                }
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <remarks>Skips checks for existing keys.</remarks>
    /// <remarks>
    ///     Doesn't increment Size. This method is actually a utility
    ///     method for <see cref="Resize" />
    /// </remarks>
    /// <param name="key"></param>
    /// <param name="value"></param>
    private void PutResize( TK key, TV? value )
    {
        for ( var i = Place( key );; i = ( i + 1 ) & Mask )
        {
            if ( KeyTable[ i ] == null )
            {
                KeyTable[ i ]   = key;
                ValueTable[ i ] = value;

                return;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TV? Remove( TK key )
    {
        if ( KeyTable == null )
        {
            throw new NullReferenceException( "Remove(): _keyTable is null" );
        }

        if ( ValueTable == null )
        {
            throw new NullReferenceException( "Remove(): _valueTable is null" );
        }

        var i = LocateKey( key );

        if ( i < 0 )
        {
            return default( TV? );
        }

        var oldValue = ValueTable[ i ];

        var next = ( i + 1 ) & Mask;

        while ( KeyTable[ next ] is { } lkey )
        {
            var placement = Place( lkey );

            if ( ( ( next - placement ) & Mask ) > ( ( i - placement ) & Mask ) )
            {
                KeyTable[ i ]   = key;
                ValueTable[ i ] = ValueTable[ next ];

                i = next;
            }

            next = ( next + 1 ) & Mask;
        }

        KeyTable[ i ]   = default( TK );
        ValueTable[ i ] = default( TV );

        Size--;

        return oldValue;
    }

    /// <summary>
    ///     Returns an iterator for the entries in the map. Remove is supported.
    ///     <para>
    ///         If Collections.allocateIterators is false, the same iterator instance
    ///         is returned each time this method is called. Use the ObjectMap.Entries
    ///         constructor for nested or multithreaded iteration.
    ///     </para>
    /// </summary>
    /// <returns></returns>
    public Entries GetEntries()
    {
        if ( CollectionsData.AllocateIterators )
        {
            return new Entries( this );
        }

        if ( _entries1 == null )
        {
            _entries1 = new Entries( this );
            _entries2 = new Entries( this );
        }

        Debug.Assert( _entries2 != null, nameof( _entries2 ) + " != null" );

        if ( !_entries1.Valid )
        {
            _entries1.Reset();
            _entries1.Valid = true;
            _entries2.Valid = false;

            return _entries1;
        }

        _entries2.Reset();
        _entries2.Valid = true;
        _entries1.Valid = false;

        return _entries2;
    }

    /// <summary>
    ///     Returns an iterator for the values in the map. Remove is supported.
    ///     <para>
    ///         If Collections.allocateIterators is false, the same iterator instance is
    ///         returned each time this method is called. Use the ObjectMap.Values
    ///         constructor for nested or multithreaded iteration.
    ///     </para>
    /// </summary>
    /// <returns></returns>
    public Values GetValues()
    {
        if ( CollectionsData.AllocateIterators )
        {
            return new Values( this );
        }

        if ( _values1 == null )
        {
            _values1 = new Values( this );
            _values2 = new Values( this );
        }

        Debug.Assert( _values1 != null, nameof( _values1 ) + " != null" );
        Debug.Assert( _values2 != null, nameof( _values2 ) + " != null" );

        if ( !_values1.Valid )
        {
            _values1.Reset();
            _values1.Valid = true;
            _values2.Valid = false;

            return _values1;
        }

        _values2.Reset();
        _values2.Valid = true;
        _values1.Valid = false;

        return _values2;
    }

    /// <summary>
    ///     Returns an iterator for the keys in the map. Remove is supported.
    ///     <para>
    ///         If Collections.allocateIterators is false, the same iterator instance
    ///         is returned each time this method is called. Use the ObjectMap.Keys
    ///         constructor for nested or multithreaded iteration.
    ///     </para>
    /// </summary>
    /// <returns></returns>
    public Keys GetKeys()
    {
        if ( CollectionsData.AllocateIterators )
        {
            return new Keys( this );
        }

        if ( _keys1 == null )
        {
            _keys1 = new Keys( this );
            _keys2 = new Keys( this );
        }

        Debug.Assert( _keys1 != null, nameof( _keys1 ) + " != null" );
        Debug.Assert( _keys2 != null, nameof( _keys2 ) + " != null" );

        if ( !_keys1.Valid )
        {
            _keys1.Reset();
            _keys1.Valid = true;
            _keys2.Valid = false;

            return _keys1;
        }

        _keys2.Reset();
        _keys2.Valid = true;
        _keys1.Valid = false;

        return _keys2;
    }

    protected Entries GetIterator()
    {
        return GetEntries();
    }

    /// <summary>
    /// </summary>
    [PublicAPI]
    public class Entry
    {
        public TK? Key;
        public TV? Value;

        public Entry()
        {
        }

        public Entry( TK? k, TV? v )
        {
            Key   = k;
            Value = v;
        }

        public override string ToString()
        {
            return Key + " = " + Value;
        }
    }

    /// <summary>
    /// </summary>
    [PublicAPI]
    public abstract class MapIterator
    {
        protected readonly ObjectMap< TK, TV > Map;

        protected int CurrentIndex = -1;
        protected int NextIndex    = -1;

        protected MapIterator( ObjectMap< TK, TV > map )
        {
            Map = map;

            Reset();
        }

        public    bool Valid   { get; set; } = true;
        protected bool HasNext { get; set; }

        public void Reset()
        {
            CurrentIndex = -1;
            NextIndex    = -1;

            FindNextIndex();
        }

        protected void FindNextIndex()
        {
            for ( var n = Map.KeyTable.Length; ++NextIndex < n; )
            {
                if ( Map.KeyTable[ NextIndex ] != null )
                {
                    HasNext = true;

                    return;
                }
            }

            HasNext = false;
        }

        public void Remove()
        {
            var i = CurrentIndex;

            if ( i < 0 )
            {
                throw new GdxRuntimeException( "Next() must be called before Remove()." );
            }

            var mask = Map.Mask;
            var next = ( i + 1 ) & mask;

            while ( Map.KeyTable[ next ] is { } key )
            {
                var placement = Map.Place( key );

                if ( ( ( next - placement ) & mask ) > ( ( i - placement ) & mask ) )
                {
                    Map.KeyTable[ i ]   = key;
                    Map.ValueTable[ i ] = Map.ValueTable[ next ];
                    i                   = next;
                }

                next = ( next + 1 ) & mask;
            }

            Map.KeyTable[ i ]   = default( TK? )!;
            Map.ValueTable[ i ] = default( TV? );

            Map.Size--;

            if ( i != CurrentIndex )
            {
                --NextIndex;
            }

            CurrentIndex = -1;
        }
    }

    /// <summary>
    /// </summary>
    [PublicAPI]
    public class Entries : MapIterator
    {
        private readonly Entry _entry = new();

        /// <summary>
        /// </summary>
        /// <param name="map"></param>
        public Entries( ObjectMap< TK, TV > map ) : base( map )
        {
        }

        /// <summary>
        ///     Note the same entry instance is returned each time this method is called.
        /// </summary>
        public Entry Next()
        {
            if ( !HasNext )
            {
                throw new GdxRuntimeException( "HasNext : false!" );
            }

            if ( !Valid )
            {
                throw new GdxRuntimeException( "#iterator() cannot be used nested." );
            }

            _entry.Key   = Map.KeyTable[ NextIndex ];
            _entry.Value = Map.ValueTable[ NextIndex ];
            CurrentIndex = NextIndex;

            FindNextIndex();

            return _entry;
        }

        public Entries Iterator()
        {
            return this;
        }
    }

    /// <summary>
    /// </summary>
    [PublicAPI]
    public class Values : MapIterator
    {
        /// <summary>
        /// </summary>
        /// <param name="map"></param>
        public Values( ObjectMap< TK, TV > map )
            : base( map )
        {
        }

        public TV? Next()
        {
            if ( !HasNext )
            {
                throw new GdxRuntimeException( "HasNext : false!" );
            }

            if ( !Valid )
            {
                throw new GdxRuntimeException( "#iterator() cannot be used nested." );
            }

            var value = Map.ValueTable[ NextIndex ];

            CurrentIndex = NextIndex;

            FindNextIndex();

            return value;
        }

        public Values Iterator()
        {
            return this;
        }

        /// <summary>
        ///     Returns a new array containing the remaining values.
        /// </summary>
        public List< TV > ToArray()
        {
            return ToArray( new List< TV >( Map.Size ) );
        }

        /// <summary>
        ///     Adds the remaining values to the array.
        /// </summary>
        public List< TV > ToArray( List< TV > array )
        {
            while ( HasNext )
            {
                array.Add( Next()! );
            }

            return array;
        }
    }

    /// <summary>
    /// </summary>
    [PublicAPI]
    public class Keys : MapIterator
    {
        public Keys( ObjectMap< TK, TV > map )
            : base( map )
        {
        }

        public TK Next()
        {
            if ( !HasNext )
            {
                throw new GdxRuntimeException( "HasNext : false!" );
            }

            if ( !Valid )
            {
                throw new GdxRuntimeException( "#iterator() cannot be used nested." );
            }

            var key = Map.KeyTable[ NextIndex ]!;

            CurrentIndex = NextIndex;

            FindNextIndex();

            return key;
        }

        public Keys Iterator()
        {
            return this;
        }

        /// <summary>
        ///     Returns a new array containing the remaining keys.
        /// </summary>
        public List< TK > ToArray()
        {
            return ToArray( new List< TK >( Map.Size ) );
        }

        /// <summary>
        ///     Adds the remaining keys to the array.
        /// </summary>
        public List< TK > ToArray( List< TK > array )
        {
            while ( HasNext )
            {
                array.Add( Next() );
            }

            return array;
        }
    }
}