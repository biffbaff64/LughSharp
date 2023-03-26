namespace LibGDXSharp.Utils
{
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

        /// <param name="index"> the index of the bit </param>
        /// <returns> whether the bit is set </returns>
        public virtual bool Get( int index )
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
        public virtual bool GetAndClear( int index )
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
        public virtual bool GetAndSet( int index )
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
        public virtual void Set( int index )
        {
            var word = index >>> 6;

            CheckCapacity( word );

            _bits[ word ] |= 1L << ( index & 0x3F );
        }

        /// <summary>
        /// </summary>
        /// <param name="index"> the index of the bit to flip </param>
        public virtual void Flip( int index )
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
        public virtual void Clear( int index )
        {
            var word = index >>> 6;

            if ( word >= _bits.Length ) return;

            _bits[ word ] &= ~( 1L << ( index & 0x3F ) );
        }

        /// <summary>
        /// Clears the entire bitset
        /// </summary>
        public virtual void Clear()
        {
            Array.Fill( _bits, 0 );
        }

        /// <summary>
        /// </summary>
        /// <returns> the number of bits currently stored, <b>not</b> the highset set bit!</returns>
        public virtual int NumBits()
        {
            return _bits.length << 6;
        }

        /// <summary>
        /// Returns the "logical size" of this bitset: the index of the highest set bit in the bitset plus one. Returns zero if the
        /// bitset contains no set bits.
        /// </summary>
        /// <returns> the logical size of this bitset  </returns>
        public virtual int Length()
        {
            long[] bits = this.bits;

            for ( int word = bits.Length - 1; word >= 0; --word )
            {
                long bitsAtWord = bits[ word ];

                if ( bitsAtWord != 0 )
                {
                    for ( int bit = 63; bit >= 0; --bit )
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

        /// <returns> true if this bitset contains at least one bit set to true </returns>
        public virtual bool NotEmpty()
        {
            return !Empty;
        }

        /// <returns> true if this bitset contains no bits that are set to true </returns>
        public virtual bool Empty
        {
            get
            {
                long[] bits   = this.bits;
                int    length = bits.Length;

                for ( int i = 0; i < length; i++ )
                {
                    if ( bits[ i ] != 0L )
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Returns the index of the first bit that is set to true that occurs on or after the specified starting index. If no such bit
        /// exists then -1 is returned. 
        /// </summary>
        public virtual int NextSetBit( int fromIndex )
        {
            long[] bits       = this.bits;
            int    word       = ( int )( ( uint )fromIndex >> 6 );
            int    bitsLength = bits.Length;

            if ( word >= bitsLength )
            {
                return -1;
            }

            long bitsAtWord = bits[ word ];

            if ( bitsAtWord != 0 )
            {
                for ( int i = fromIndex & 0x3f; i < 64; i++ )
                {
                    if ( ( bitsAtWord & ( 1L << ( i & 0x3F ) ) ) != 0L )
                    {
                        return ( word << 6 ) + i;
                    }
                }
            }

            for ( word++; word < bitsLength; word++ )
            {
                if ( word != 0 )
                {
                    bitsAtWord = bits[ word ];

                    if ( bitsAtWord != 0 )
                    {
                        for ( int i = 0; i < 64; i++ )
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
        /// Returns the index of the first bit that is set to false that occurs on or after the specified starting index. </summary>
        public virtual int NextClearBit( int fromIndex )
        {
            long[] bits       = this.bits;
            int    word       = ( int )( ( uint )fromIndex >> 6 );
            int    bitsLength = bits.Length;

            if ( word >= bitsLength )
            {
                return bits.Length << 6;
            }

            long bitsAtWord = bits[ word ];

            for ( int i = fromIndex & 0x3f; i < 64; i++ )
            {
                if ( ( bitsAtWord & ( 1L << ( i & 0x3F ) ) ) == 0L )
                {
                    return ( word << 6 ) + i;
                }
            }

            for ( word++; word < bitsLength; word++ )
            {
                if ( word == 0 )
                {
                    return word << 6;
                }

                bitsAtWord = bits[ word ];

                for ( int i = 0; i < 64; i++ )
                {
                    if ( ( bitsAtWord & ( 1L << ( i & 0x3F ) ) ) == 0L )
                    {
                        return ( word << 6 ) + i;
                    }
                }
            }

            return bits.Length << 6;
        }
    }
}
