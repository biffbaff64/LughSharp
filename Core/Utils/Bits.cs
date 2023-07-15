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

namespace LibGDXSharp.Utils;

/// <summary>
/// A bitset, without size limitation, allows comparison via
/// bitwise operators to other bitfields.
/// </summary>
public class Bits
{
    private long[] _bits = { 0 };

    /// <summary>
    /// Creates a bit set whose initial size is large enough to explicitly
    /// represent bits with indices in the range 0 through nbits-1.
    /// </summary>
    /// <param name="nbits">the initial size of the bit set</param>
    public Bits( int nbits )
    {
        CheckCapacity( nbits >>> 6 );
    }

    /// <summary>
    /// </summary>
    /// <param name="index"> the index of the bit </param>
    /// <returns> whether the bit is set </returns>
    public bool Get( int index )
    {
        var word = index >>> 6;

        if ( word >= _bits.Length )
        {
            return false;
        }

        return ( _bits[ word ] & ( 1L << ( index & 0x3F ) ) ) != 0L;
    }

    /// <summary>
    /// Returns the bit at the given index and clears it in one go.
    /// </summary>
    /// <param name="index"> the index of the bit </param>
    /// <returns> whether the bit was set before invocation </returns>
    public bool GetAndClear( int index )
    {
        var word = index >>> 6;

        if ( word >= _bits.Length ) return false;

        var oldBits = _bits[ word ];

        _bits[ word ] &= ~( 1L << ( index & 0x3F ) );

        return _bits[ word ] != oldBits;
    }

    /// <summary>
    /// Returns the bit at the given index and sets it in one go.
    /// </summary>
    /// <param name="index">the index of the bit</param>
    /// <returns>whether the bit was set before invocation</returns>
    public bool GetAndSet( int index )
    {
        var word = index >>> 6;

        CheckCapacity( word );

        var oldBits = _bits[ word ];
        _bits[ word ] |= 1L << ( index & 0x3F );

        return _bits[ word ] == oldBits;
    }

    /// <summary>
    /// </summary>
    /// <param name="index"> the index of the bit to set </param>
    public void Set( int index )
    {
        var word = index >>> 6;

        CheckCapacity( word );

        _bits[ word ] |= 1L << ( index & 0x3F );
    }

    /// <summary>
    /// </summary>
    /// <param name="index"> the index of the bit to flip </param>
    public void Flip( int index )
    {
        var word = index >>> 6;

        CheckCapacity( word );

        _bits[ word ] ^= 1L << ( index & 0x3F );
    }

    /// <summary>
    /// </summary>
    /// <param name="len"></param>
    private void CheckCapacity( int len )
    {
        if ( len >= _bits.Length )
        {
            var newBits = new long[ len + 1 ];

            Array.Copy( _bits, 0, newBits, 0, _bits.Length );

            _bits = newBits;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="index"> the index of the bit to clear </param>
    public void Clear( int index )
    {
        var word = index >>> 6;

        if ( word >= _bits.Length ) return;

        _bits[ word ] &= ~( 1L << ( index & 0x3F ) );
    }

    /// <summary>
    /// Clears the entire bitset
    /// </summary>
    public void Clear()
    {
        Array.Fill( _bits, 0 );
    }

    /// <summary>
    /// </summary>
    /// <returns> the number of bits currently stored, <b>not</b> the highset set bit!</returns>
    public int NumBits()
    {
        return _bits.Length << 6;
    }

    /// <summary>
    /// Returns the "logical size" of this bitset: the index of the highest set bit in the bitset plus one. Returns zero if the
    /// bitset contains no set bits.
    /// </summary>
    /// <returns> the logical size of this bitset  </returns>
    public int Length()
    {
        for ( var word = _bits.Length - 1; word >= 0; --word )
        {
            var bitsAtWord = _bits[ word ];

            if ( bitsAtWord != 0 )
            {
                for ( var bit = 63; bit >= 0; --bit )
                {
                    if ( ( bitsAtWord & ( 1L << ( bit & 0x3F ) ) ) != 0L )
                    {
                        return ( word << 6 ) + bit + 1;
                    }
                }
            }
        }

        return 0;
    }

    /// <summary>
    /// </summary>
    /// <returns> true if this bitset contains at least one bit set to true </returns>
    public bool NotEmpty()
    {
        return !Empty;
    }

    /// <summary>
    /// </summary>
    /// <returns> true if this bitset contains no bits that are set to true </returns>
    public bool Empty
    {
        get
        {
            foreach ( var t in _bits )
            {
                if ( t != 0L )
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Returns the index of the first bit that is set to true that occurs on
    /// or after the specified starting index. If no such bit exists then -1
    /// is returned. 
    /// </summary>
    public int NextSetBit( int fromIndex )
    {
        var word = fromIndex >>> 6;

        if ( word >= _bits.Length )
        {
            return -1;
        }

        var bitsAtWord = _bits[ word ];

        if ( bitsAtWord != 0 )
        {
            for ( var i = fromIndex & 0x3f; i < 64; i++ )
            {
                if ( ( bitsAtWord & ( 1L << ( i & 0x3F ) ) ) != 0L )
                {
                    return ( word << 6 ) + i;
                }
            }
        }

        for ( word++; word < _bits.Length; word++ )
        {
            if ( word != 0 )
            {
                bitsAtWord = _bits[ word ];

                if ( bitsAtWord != 0 )
                {
                    for ( var i = 0; i < 64; i++ )
                    {
                        if ( ( bitsAtWord & ( 1L << ( i & 0x3F ) ) ) != 0L )
                        {
                            return ( word << 6 ) + i;
                        }
                    }
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns the index of the first bit that is set to false that occurs on
    /// or after the specified starting index.
    /// </summary>
    public int NextClearBit( int fromIndex )
    {
        var word = fromIndex >>> 6;

        if ( word >= _bits.Length )
        {
            return _bits.Length << 6;
        }

        var bitsAtWord = _bits[ word ];

        for ( var i = fromIndex & 0x3f; i < 64; i++ )
        {
            if ( ( bitsAtWord & ( 1L << ( i & 0x3F ) ) ) == 0L )
            {
                return ( word << 6 ) + i;
            }
        }

        for ( word++; word < _bits.Length; word++ )
        {
            if ( word == 0 )
            {
                return word << 6;
            }

            bitsAtWord = _bits[ word ];

            for ( var i = 0; i < 64; i++ )
            {
                if ( ( bitsAtWord & ( 1L << ( i & 0x3F ) ) ) == 0L )
                {
                    return ( word << 6 ) + i;
                }
            }
        }

        return _bits.Length << 6;
    }

    /// <summary>
    /// Performs a logical <b>AND</b> of this target bit set with the argument
    /// bit set. This bit set is modified so that each bit in it has the value
    /// true if and only if it both initially had the value true and the
    /// corresponding bit in the bit set argument also had the value true.
    /// </summary>
    /// <param name="other"> a bit set  </param>
    public void And( Bits other )
    {
        var commonWords = Math.Min( _bits.Length, other._bits.Length );

        for ( var i = 0; commonWords > i; i++ )
        {
            _bits[ i ] &= other._bits[ i ];
        }

        if ( _bits.Length > commonWords )
        {
            for ( int i = commonWords, s = _bits.Length; s > i; i++ )
            {
                _bits[ i ] = 0L;
            }
        }
    }

    /// <summary>
    /// Clears all of the bits in this bit set whose corresponding bit is
    /// set in the specified bit set.
    /// </summary>
    /// <param name="other"> a bit set  </param>
    public void AndNot( Bits other )
    {
        for ( int i = 0, j = _bits.Length, k = other._bits.Length; ( i < j ) && ( i < k ); i++ )
        {
            _bits[ i ] &= ~other._bits[ i ];
        }
    }

    /// <summary>
    /// Performs a logical <b>OR</b> of this bit set with the bit set argument.
    /// This bit set is modified so that a bit in it has the value true if and
    /// only if it either already had the value true or the corresponding bit
    /// in the bit set argument has the value true.
    /// </summary>
    /// <param name="other"> a bit set  </param>
    public void Or( Bits other )
    {
        var commonWords = Math.Min( _bits.Length, other._bits.Length );

        for ( var i = 0; commonWords > i; i++ )
        {
            _bits[ i ] |= other._bits[ i ];
        }

        if ( commonWords < other._bits.Length )
        {
            CheckCapacity( other._bits.Length );

            for ( int i = commonWords, s = other._bits.Length; s > i; i++ )
            {
                _bits[ i ] = other._bits[ i ];
            }
        }
    }

    /// <summary>
    /// Performs a logical <b>XOR</b> of this bit set with the bit set argument.
    /// This bit set is modified so that a bit in it has the value true if and
    /// only if one of the following statements holds:
    /// <ul>
    /// <li>
    /// The bit initially has the value true, and the corresponding bit
    /// in the argument has the value false.
    /// </li>
    /// <li>
    /// The bit initially has the value false, and the corresponding bit
    /// in the argument has the value true.
    /// </li>
    /// </ul>
    /// </summary>
    /// <param name="other">  </param>
    public void Xor( Bits other )
    {
        var commonWords = Math.Min( _bits.Length, other._bits.Length );

        for ( var i = 0; commonWords > i; i++ )
        {
            _bits[ i ] ^= other._bits[ i ];
        }

        if ( commonWords < other._bits.Length )
        {
            CheckCapacity( other._bits.Length );

            for ( int i = commonWords, s = other._bits.Length; s > i; i++ )
            {
                _bits[ i ] = other._bits[ i ];
            }
        }
    }

    /// <summary>
    /// Returns true if the specified BitSet has any bits set to true that are
    /// also set to true in this BitSet.
    /// </summary>
    /// <param name="other"> a bit set </param>
    /// <returns>bool indicating whether this bit set intersects the specified bit set</returns>
    public bool Intersects( Bits other )
    {
        var bits      = this._bits;
        var otherBits = other._bits;

        for ( var i = Math.Min( bits.Length, otherBits.Length ) - 1; i >= 0; i-- )
        {
            if ( ( bits[ i ] & otherBits[ i ] ) != 0 )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if this bit set is a super set of the specified set, i.e. it
    /// has all bits set to true that are also set to true in the specified BitSet.
    /// </summary>
    /// <param name="other"> a bit set </param>
    /// <returns>
    /// bool indicating whether this bit set is a super set of the specified set
    /// </returns>
    public bool ContainsAll( Bits other )
    {
        var bits            = this._bits;
        var otherBits       = other._bits;
        var otherBitsLength = otherBits.Length;
        var bitsLength      = bits.Length;

        for ( var i = bitsLength; i < otherBitsLength; i++ )
        {
            if ( otherBits[ i ] != 0 )
            {
                return false;
            }
        }

        for ( var i = Math.Min( bitsLength, otherBitsLength ) - 1; i >= 0; i-- )
        {
            if ( ( bits[ i ] & otherBits[ i ] ) != otherBits[ i ] )
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        const int prime = 73;

        var hash = prime + Gdx.App.GetVersion().GetHashCode();
        hash = ( prime * hash ) + Gdx.App.GetHashCode();

        return hash!;
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals( object? obj )
    {
        if ( this == obj ) return true;

        if ( obj == null ) return false;

        if ( this.GetType() != obj.GetType() ) return false;

        var other     = ( Bits )obj;
        var otherBits = other._bits;

        var commonWords = Math.Min( _bits.Length, otherBits.Length );

        for ( var i = 0; commonWords > i; i++ )
        {
            if ( _bits[ i ] != otherBits[ i ] )
            {
                return false;
            }
        }

        if ( _bits.Length == otherBits.Length )
        {
            return true;
        }

        return Length() == other.Length();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    #region statics

    private static ByteOrder? _byteOrder;

    public static ByteOrder ByteOrder
    {
        get
        {
            if ( _byteOrder == null )
            {
                throw new GdxRuntimeException( "Unknown Byte Order!" );
            }

            return _byteOrder;
        }
        set => _byteOrder = value;
    }

    #endregion statics

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
}