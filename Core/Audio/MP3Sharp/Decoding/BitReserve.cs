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

namespace LibGDXSharp.Audio.MP3Sharp;

/// <summary>
///     Implementation of Bit Reservoir for Layer III.
///     The implementation stores single bits as a word in the buffer. If a bit is set, the
///     corresponding word in the buffer will be non-zero. If a bit is clear, the corresponding
///     word is zero. Although this may seem waseful, this can be a factor of two quicker than
///     packing 8 bits to a byte and extracting.
/// </summary>

// TODO: there is no range checking, so buffer underflow or overflow can silently occur.
public class BitReserve
{
    /// <summary>
    ///     Size of the public buffer to store the reserved bits. Must be a power of 2. And
    ///     x8, as each bit is stored as a single entry.
    /// </summary>
    private const int BUFSIZE = 4096 * 8;

    /// <summary>
    ///     Mask that can be used to quickly implement the modulus operation on BUFSIZE.
    /// </summary>
    private const int BUFSIZE_MASK = BUFSIZE - 1;
    private int _bufByteIdx;

    private readonly int[] _buffer;
    private          int   _offset;
    private          int   _totbit;

    public BitReserve()
    {
        _buffer     = new int[ BUFSIZE ];
        _offset     = 0;
        _totbit     = 0;
        _bufByteIdx = 0;
    }

    /// <summary>
    ///     Return totbit Field.
    /// </summary>
    public int HssTell() => _totbit;

    /// <summary>
    ///     Read a number bits from the bit stream.
    /// </summary>
    public int ReadBits( int n )
    {
        _totbit += n;

        var val = 0;
        var pos = _bufByteIdx;

        if ( ( pos + n ) < BUFSIZE )
        {
            while ( n-- > 0 )
            {
                val <<= 1;
                val |=  _buffer[ pos++ ] != 0 ? 1 : 0;
            }
        }
        else
        {
            while ( n-- > 0 )
            {
                val <<= 1;
                val |=  _buffer[ pos ] != 0 ? 1 : 0;
                pos =   ( pos + 1 ) & BUFSIZE_MASK;
            }
        }

        _bufByteIdx = pos;

        return val;
    }

    /// <summary>
    ///     Read 1 bit from the bit stream.
    /// </summary>
    public int ReadOneBit()
    {
        _totbit++;

        var val = _buffer[ _bufByteIdx ];

        _bufByteIdx = ( _bufByteIdx + 1 ) & BUFSIZE_MASK;

        return val;
    }

    /// <summary>
    ///     Write 8 bits into the bit stream.
    /// </summary>
    public void PutBuffer( int val )
    {
        var ofs = _offset;

        _buffer[ ofs++ ] = val & 0x80;
        _buffer[ ofs++ ] = val & 0x40;
        _buffer[ ofs++ ] = val & 0x20;
        _buffer[ ofs++ ] = val & 0x10;
        _buffer[ ofs++ ] = val & 0x08;
        _buffer[ ofs++ ] = val & 0x04;
        _buffer[ ofs++ ] = val & 0x02;
        _buffer[ ofs++ ] = val & 0x01;

        _offset = ofs == BUFSIZE ? 0 : ofs;
    }

    /// <summary>
    ///     Rewind n bits in Stream.
    /// </summary>
    public void RewindStreamBits( int bitCount )
    {
        _totbit     -= bitCount;
        _bufByteIdx -= bitCount;

        if ( _bufByteIdx < 0 )
        {
            _bufByteIdx += BUFSIZE;
        }
    }

    /// <summary>
    ///     Rewind n bytes in Stream.
    /// </summary>
    public void RewindStreamBytes( int byteCount )
    {
        var bits = byteCount << 3;
        _totbit     -= bits;
        _bufByteIdx -= bits;

        if ( _bufByteIdx < 0 )
        {
            _bufByteIdx += BUFSIZE;
        }
    }
}
