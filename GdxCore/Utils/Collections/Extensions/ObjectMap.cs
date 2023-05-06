using System.Collections;
using System.Text;

namespace LibGDXSharp.Utils.Collections.Extensions;

/// <summary>
/// <p>An unordered map where the keys and values are objects.</p>
/// <p>Null keys are not allowed.</p>
/// <p>No allocation is done except when growing the table size.</p>
/// <p>
/// This class performs fast contains and remove (typically O(1), worst case O(n) but
/// that is rare in practice). Add may be slightly slower, depending on hash collisions.
/// Hashcodes are rehashed to reduce collisions and the need to resize. Load factors
/// greater than 0.91 greatly increase the chances to resize to the next higher POT size.
/// Unordered sets and maps are not designed to provide especially fast iteration.
/// </p>
/// <p>Iteration is faster with OrderedSet and OrderedMap.</p>
/// <p>
/// This implementation uses linear probing with the backward shift algorithm for removal.
/// Hashcodes are rehashed using Fibonacci hashing, instead of the more common power-of-two
/// mask, to better distribute poor hashCodes (see Malte Skarupke's blog post). Linear
/// probing continues to work even when all hashCodes collide, just more slowly.
/// </p>
/// </summary>
public class ObjectMap<TK, TV> : IEnumerable< ObjectMap< TK, TV >.Entry< TK, TV > >
{
    private TK?[] _keyTable;
    private TV?[] _valueTable;

    private readonly float _loadFactor;
    private          int   _threshold;

    private readonly object _dummy = new object();

    /// <summary>
    /// Used by <see cref="Place"/> to bit shift the upper bits of a <b>long</b>
    /// into a usable range (&gt;= 0 and &lt;= <see cref="_mask"/>).
    /// <p>
    /// The shift can be negative, which is convenient to match the number of bits in
    /// mask: if mask is a 7-bit number, a shift of -7 shifts the upper 7 bits into the
    /// lowest 7 positions. This class sets the shift &gt; 32 and &lt; 64, which if used
    /// with an int will still move the upper bits of an int to the lower bits.
    /// </p>
    /// <p>
    /// <see cref="_mask"/> can also be used to mask the low bits of a number, which may
    /// be faster for some hashcodes if <see cref="Place"/> is overridden.
    /// </p>
    /// </summary>
    private int _shift;

    /// <summary>
    /// A bitmask used to confine hashcodes to the size of the table. Must be all
    /// 1 bits in its low positions, ie a power of two minus 1.
    /// If <see cref="Place"/> is overriden, this can be used instead of <see cref="_shift"/>
    /// to isolate usable bits of a hash.
    /// </summary>
    private int _mask;

    [NonSerialized] private Entries< TK, TV >? _entries1;
    [NonSerialized] private Entries< TK, TV >? _entries2;
    [NonSerialized] private Values< TV >?      _values1;
    [NonSerialized] private Values< TV >?      _values2;
    [NonSerialized] private Keys< TK >?        _keys1;
    [NonSerialized] private Keys< TK >?        _keys2;

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

        this._loadFactor = loadFactor;

        var tableSize = ArrayUtils.TableSize( initialCapacity, loadFactor );

        _threshold  = ( int )( tableSize * loadFactor );
        _mask       = tableSize - 1;
        _shift      = int.LeadingZeroCount( _mask );
        _keyTable   = new TK[ tableSize ];
        _valueTable = new TV[ tableSize ];
    }

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="map">The ObjectMap to copy.</param>
    /// <exception cref="ArgumentException"></exception>
    public ObjectMap( ObjectMap< TK, TV > map )
    {
        if ( map == null ) throw new ArgumentException( "supplied map is null!" );

        if ( map._valueTable == null )
        {
            throw new ArgumentException( "supplied map._valuetable is null!" );
        }

        this._loadFactor = map._loadFactor;

        var tableSize = ArrayUtils.TableSize
            (
             ( int )( map._keyTable.Length * map._loadFactor ),
             _loadFactor
            );

        _threshold = ( int )( tableSize * _loadFactor );
        _mask      = tableSize - 1;
        _shift     = int.LeadingZeroCount( _mask );

        _keyTable   = new TK[ tableSize ];
        _valueTable = new TV[ tableSize ];

        Array.Copy( map._keyTable, 0, _keyTable, 0, map._keyTable.Length );
        Array.Copy( map._valueTable, 0, _valueTable, 0, map._valueTable.Length );

        this.Size = map.Size;
    }

    /// <summary>
    /// Returns the size of this ObjectMap
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Returns an index greater than or equal to 0 and less than or equal
    /// to <see cref="_mask"/> for the specified <tt>item</tt>.
    /// </summary>
    /// <param name="item"></param>
    public int Place( TK item )
    {
        if ( item == null )
        {
            throw new ArgumentException( "item cannot be null!" );
        }

        return ( int )( ( ( ulong )item.GetHashCode() * 0x9E3779B97F4A7C15L ) >>> _shift );
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private int LocateKey( TK key )
    {
        if ( key == null ) throw new ArgumentException( "key cannot be null." );

        if ( _keyTable == null ) throw new NullReferenceException( "_keyTable is null" );

        for ( var i = Place( key );; i = ( i + 1 ) & _mask )
        {
            TK? other = _keyTable[ i ];

            if ( other == null ) return -( i + 1 ); // Empty space is available.

            if ( other.Equals( key ) ) return i; // Same key was found.
        }
    }

    /// <summary>
    /// Replaces the value associated with the specified key, and returns the old value.
    /// If the key is not found, the value is added at the end of the map and null is returned.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public TV? Put( TK key, TV? value )
    {
        if ( _keyTable == null ) throw new NullReferenceException( "Put(): _keyTable is null" );
        if ( _valueTable == null ) throw new NullReferenceException( "Put(): _valueTable is null" );

        var i = LocateKey( key );

        if ( i >= 0 )
        {
            // Existing key was found.
            TV? oldValue = _valueTable[ i ];
            _valueTable[ i ] = value;

            return oldValue;
        }

        i = -( i + 1 ); // Empty space was found.

        _keyTable[ i ]   = key;
        _valueTable[ i ] = value;

        if ( ++Size >= _threshold ) Resize( _keyTable.Length << 1 );

        return default;
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    public void PutAll( ObjectMap< TK, TV > map )
    {
        EnsureCapacity( map.Size );

        for ( int i = 0, n = _keyTable.Length; i < n; i++ )
        {
            TK? key = _keyTable[ i ];

            if ( key != null ) Put( key, _valueTable[ i ] );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="newSize"></param>
    void Resize( int newSize )
    {
        var oldCapacity = _keyTable.Length;
        _threshold = ( int )( newSize * _loadFactor );

        _mask  = newSize - 1;
        _shift = int.LeadingZeroCount( _mask );

        var oldKeyTable   = _keyTable;
        var oldValueTable = _valueTable;

        _keyTable   = new TK[ newSize ];
        _valueTable = new TV[ newSize ];

        if ( Size > 0 )
        {
            for ( var i = 0; i < oldCapacity; i++ )
            {
                TK? key = oldKeyTable[ i ];

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
    /// Doesn't increment Size. This method is actually a utility
    /// method for <see cref="Resize"/>
    /// </remarks>
    /// <param name="key"></param>
    /// <param name="value"></param>
    private void PutResize( TK key, TV? value )
    {
        for ( var i = Place( key );; i = ( i + 1 ) & _mask )
        {
            if ( _keyTable[ i ] == null )
            {
                _keyTable[ i ]   = key;
                _valueTable[ i ] = value;

                return;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="TT"></typeparam>
    /// <returns></returns>
    public TV? Get<TT>( TT key ) where TT : TK
    {
        var i = LocateKey( key );

        return ( i < 0 ) ? default : _valueTable[ i ];
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public TV? Get( TK key, TV? defaultValue )
    {
        var i = LocateKey( key );

        return i < 0 ? defaultValue : _valueTable[ i ];
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TV? Remove( TK key )
    {
        if ( _keyTable == null ) throw new NullReferenceException( "Remove(): _keyTable is null" );
        if ( _valueTable == null ) throw new NullReferenceException( "Remove(): _valueTable is null" );

        var i = LocateKey( key );

        if ( i < 0 ) return default;

        TV? oldValue = _valueTable[ i ];

        var next = ( i + 1 ) & _mask;

        while ( _keyTable[ next ] is { } lkey )
        {
            var placement = Place( lkey );

            if ( ( ( next - placement ) & _mask ) > ( ( i - placement ) & _mask ) )
            {
                _keyTable[ i ]   = key;
                _valueTable[ i ] = _valueTable[ next ];

                i = next;
            }

            next = ( next + 1 ) & _mask;
        }

        _keyTable[ i ]   = default( TK );
        _valueTable[ i ] = default( TV );

        Size--;

        return oldValue;
    }

    /// <summary>
    /// </summary>
    /// <param name="maximumCapacity"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Shrink( int maximumCapacity )
    {
        if ( maximumCapacity < 0 )
        {
            throw new ArgumentException( "maximumCapacity must be >= 0: " + maximumCapacity );
        }

        var tableSize = ArrayUtils.TableSize( maximumCapacity, _loadFactor );

        if ( _keyTable.Length > tableSize )
        {
            Resize( tableSize );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="maximumCapacity"></param>
    public void Clear( int maximumCapacity )
    {
        var tableSize = ArrayUtils.TableSize( maximumCapacity, _loadFactor );

        if ( _keyTable.Length <= tableSize )
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
        if ( Size == 0 ) return;

        Size = 0;

        Array.Clear( _keyTable );
        Array.Clear( _valueTable );
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
            for ( var i = _valueTable.Length - 1; i >= 0; i-- )
            {
                if ( _keyTable[ i ] != null && _valueTable[ i ] == null )
                {
                    return true;
                }
            }
        }
        else
        {
            for ( var i = _valueTable.Length - 1; i >= 0; i-- )
            {
                if ( value.Equals( _valueTable[ i ] ) )
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
    /// Returns the key for the specified value, or null if it is not in the map.
    /// Note this traverses the entire map and compares every value, which may be
    /// an expensive operation.
    /// </summary>
    public TK? FindKey( object? value )
    {
        if ( value == null )
        {
            for ( var i = _valueTable.Length - 1; i >= 0; i-- )
            {
                if ( _keyTable[ i ] != null && _valueTable[ i ] == null )
                {
                    return _keyTable[ i ];
                }
            }
        }
        else
        {
            for ( var i = _valueTable.Length - 1; i >= 0; i-- )
            {
                if ( value.Equals( _valueTable[ i ] ) )
                {
                    return _keyTable[ i ];
                }
            }
        }

        return default;
    }

    /// <summary>
    /// Increases the size of the backing array to accommodate the specified number
    /// of additional items / loadFactor. Useful before adding many items to avoid
    /// multiple backing array resizes.
    /// </summary>
    public void EnsureCapacity( int additionalCapacity )
    {
        var tableSize = ArrayUtils.TableSize( Size + additionalCapacity, _loadFactor );

        if ( _keyTable.Length < tableSize )
        {
            Resize( tableSize );
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public new int GetHashCode()
    {
        var h = Size;

        for ( int i = 0, n = _keyTable.Length; i < n; i++ )
        {
            TK? key = _keyTable[ i ];

            if ( key != null )
            {
                h += key.GetHashCode();

                TV? value = _valueTable[ i ];

                if ( value != null )
                {
                    h += value.GetHashCode();
                }
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

        if ( obj is not ObjectMap< TK, TV > other )
        {
            return false;
        }

        if ( other.Size != Size ) return false;

        for ( int i = 0, n = _keyTable.Length; i < n; i++ )
        {
            TK? key = _keyTable[ i ];

            if ( key != null )
            {
                TV? value = _valueTable[ i ];

                if ( value == null )
                {
                    if ( other.Get( key, ( TV? )_dummy ) != null ) return false;
                }
                else
                {
                    if ( !value.Equals( other.Get( key ) ) ) return false;
                }
            }
        }

        return true;
    }

    public string ToString( string separator )
    {
        return ToString( separator, false );
    }

    public new string ToString()
    {
        return ToString( ", ", true );
    }

    protected string ToString( string separator, bool braces )
    {
        if ( Size == 0 ) return braces ? "{}" : "";

        var buffer = new StringBuilder( 32 );

        if ( braces ) buffer.Append( '{' );

        var i = _keyTable.Length;

        while ( i-- > 0 )
        {
            TK? key = _keyTable[ i ];

            if ( key == null ) continue;

            buffer.Append( key );
            buffer.Append( '=' );

            TV? value = _valueTable[ i ];

            buffer.Append( value );

            break;
        }

        while ( i-- > 0 )
        {
            TK? key = _keyTable[ i ];

            if ( key == null ) continue;

            buffer.Append( separator );
            buffer.Append( key );
            buffer.Append( '=' );

            TV? value = _valueTable[ i ];

            if ( Equals( value, this ) )
            {
                buffer.Append( "(this)" );
            }
            else
            {
                buffer.Append( value );
            }
        }

        if ( braces ) buffer.Append( '}' );

        return buffer.ToString();
    }

    /// <summary>
    /// Helper method.
    /// </summary>
    /// <returns>TRUE if Size is greater than zero.</returns>
    public bool NotEmpty()
    {
        return Size > 0;
    }

    /// <summary>
    /// Helper method.
    /// </summary>
    /// <returns>TRUE if Size is zero.</returns>
    public bool IsEmpty()
    {
        return Size == 0;
    }

    /// <summary>
    /// Returns an iterator for the entries in the map. Remove is supported.
    /// <p>
    /// If Collections.allocateIterators is false, the same iterator instance
    /// is returned each time this method is called. Use the ObjectMap.Entries
    /// constructor for nested or multithreaded iteration.
    /// </p>
    /// </summary>
    /// <returns></returns>
    public Entries< TK, TV > GetEntries()
    {
    }

    /// <summary>
    /// Returns an iterator for the values in the map. Remove is supported.
    /// <p>
    /// If Collections.allocateIterators is false, the same iterator instance is
    /// returned each time this method is called. Use the ObjectMap.Values
    /// constructor for nested or multithreaded iteration.
    /// </p>
    /// </summary>
    /// <returns></returns>
    public Values< TV > GetValues()
    {
    }
        
    /// <summary>
    /// Returns an iterator for the keys in the map. Remove is supported.
    /// <p>
    /// If Collections.allocateIterators is false, the same iterator instance
    /// is returned each time this method is called. Use the ObjectMap.Keys
    /// constructor for nested or multithreaded iteration.
    /// </p>
    /// </summary>
    /// <returns></returns>
    public Keys< TK > GetKeys()
    {
        if ( CollectionsData.AllocateIterators ) return new Keys< TK >( this );

        if ( _keys1 == null )
        {
            _keys1 = new Keys< TK >( this );
            _keys2 = new Keys< TK >( this );
        }

        if ( !_keys1.valid )
        {
            _keys1.Reset();
            _keys1.valid = true;
            _keys2.valid = false;

            return _keys1;
        }

        _keys2.Reset();
        _keys2.valid = true;
        _keys1.valid = false;

        return _keys2;
    }

    public IEnumerator< Entry< TK, TV > > GetEnumerator()
    {
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TKe"></typeparam>
    /// <typeparam name="TVe"></typeparam>
    public class Entry<TKe, TVe>
    {
        public TKe? key;
        public TVe? value;

        public override string ToString()
        {
            return key + " = " + value;
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TKm"></typeparam>
    /// <typeparam name="TVm"></typeparam>
    /// <typeparam name="TI"></typeparam>
    public abstract class MapIterator<TKm, TVm, TI> : IEnumerable< TI >, IEnumerator< TI >
    {
        protected readonly ObjectMap< TKm, TVm > map;

        public bool valid = true;

        protected MapIterator( ObjectMap< TKm, TVm > map )
        {
            this.map = map;
            Reset();
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        /// The collection was modified after the enumerator was created.
        /// </exception>
        /// <returns>
        /// <see langword="true" />if the enumerator was successfully advanced to the next element;
        /// <see langword="false" />if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the enumerator to its initial position,
        /// which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        /// The collection was modified after the enumerator was created.
        /// </exception>
        public void Reset()
        {
        }

        /// <summary>
        /// Gets the element in the collection at the current position
        /// of the enumerator.
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the enumerator.
        /// </returns>
        public TI Current { get; }

        /// <summary>
        /// Gets the element in the collection at the current position
        /// of the enumerator.
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the enumerator.
        /// </returns>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator< TI > GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be
        /// used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TKe"></typeparam>
    /// <typeparam name="TVe"></typeparam>
    public sealed class Entries<TKe, TVe> : MapIterator< TKe, TVe, Entry< TKe, TVe > >
    {
        /// <summary>
        /// </summary>
        /// <param name="map"></param>
        public Entries( ObjectMap< TKe, TVe > map ) : base( map )
        {
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TVv"></typeparam>
    public sealed class Values<TVv> : MapIterator< object, TVv, TVv >
    {
        /// <summary>
        /// </summary>
        /// <param name="map"></param>
        public Values( ObjectMap< object, TVv > map ) : base( map )
        {
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TKk"></typeparam>
    public sealed class Keys<TKk> : MapIterator< TKk, object, TKk >
    {
        public Keys( ObjectMap< TKk, object > map ) : base( map )
        {
        }
    }
}
