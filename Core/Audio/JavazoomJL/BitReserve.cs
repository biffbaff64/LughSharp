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

namespace LibGDXSharp.Core.Audio.JavazoomJL;

[PublicAPI]
public class BitReserve
{
    // Size of the internal buffer to store the reserved bits. Must be a
    // power of 2. And x8, as each bit is stored as a single entry.
    private const int BUFSIZE = 4096 * 8;

    // Mask that can be used to quickly implement the modulus operation on BUFSIZE.
    private const int BUFSIZE_MASK = BUFSIZE - 1;

    public int TotBit { get; set; }

    private int   _offset     = 0;
    private int   _totbit     = 0;
    private int   _bufByteIdx = 0;
    private int[] _buf        = new int[ BUFSIZE ];

    /// <summary>
    /// Read a number bits from the bit stream.
    /// </summary>
    /// <param name="n"> the number of </param>
    public int HGetBits( int n )
    {
        _totbit += n;

        var val = 0;

        var pos = _bufByteIdx;

        if ( ( pos + n ) < BUFSIZE )
        {
            while ( n-- > 0 )
            {
                val <<= 1;
                val |=  _buf[ pos++ ] != 0 ? 1 : 0;
            }
        }
        else
        {
            while ( n-- > 0 )
            {
                val <<= 1;
                val |=  _buf[ pos ] != 0 ? 1 : 0;
                pos =   ( pos + 1 ) & BUFSIZE_MASK;
            }
        }

        _bufByteIdx = pos;

        return val;
    }

    /// <summary>
    /// Returns next bit from reserve.
    /// </summary>
    /// <returns> 0 if next bit is reset, or 1 if next bit is set. </returns>
    public int HGet1Bit()
    {
        _totbit++;
        
        var val = _buf[ _bufByteIdx ];
        
        _bufByteIdx = ( _bufByteIdx + 1 ) & BUFSIZE_MASK;

        return val;
    }

    /// <summary>
    /// Write 8 bits into the bit stream.
    /// </summary>
    public void HPutbuf( int val )
    {
        var ofs = _offset;
        
        _buf[ ofs++ ] = val & 0x80;
        _buf[ ofs++ ] = val & 0x40;
        _buf[ ofs++ ] = val & 0x20;
        _buf[ ofs++ ] = val & 0x10;
        _buf[ ofs++ ] = val & 0x08;
        _buf[ ofs++ ] = val & 0x04;
        _buf[ ofs++ ] = val & 0x02;
        _buf[ ofs++ ] = val & 0x01;

        _offset = ofs == BUFSIZE ? 0 : ofs;

    }

    /// <summary>
    /// Rewind n bits in Stream.
    /// </summary>
    public void RewindNbits( int n )
    {
        _totbit     -= n;
        _bufByteIdx -= n;

        if ( _bufByteIdx < 0 )
        {
            _bufByteIdx += BUFSIZE;
        }
    }

    /// <summary>
    /// Rewind n bytes in Stream.
    /// </summary>
    public void RewindNbytes( int n )
    {
        var bits = n << 3;

        _totbit     -= bits;
        _bufByteIdx -= bits;

        if ( _bufByteIdx < 0 )
        {
            _bufByteIdx += BUFSIZE;
        }
    }
}
