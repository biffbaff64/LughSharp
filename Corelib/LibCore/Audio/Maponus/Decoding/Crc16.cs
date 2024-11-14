// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Audio.Maponus.Support;

namespace Corelib.LibCore.Audio.Maponus.Decoding;

/// <summary>
/// 16-Bit CRC checksum
/// </summary>
[PublicAPI]
public class Crc16
{
    private static readonly short _polynomial;

    private short _crc = ( short ) SupportClass.Identity( 0xFFFF );

    // ========================================================================

    static Crc16()
    {
        _polynomial = ( short ) SupportClass.Identity( 0x8005 );
    }

    /// <summary>
    /// Feed a bitstring to the crc calculation (length between 0 and 32, not inclusive).
    /// </summary>
    public void AddBits( int bitstring, int length )
    {
        var bitmask = 1 << ( length - 1 );

        do
        {
            if ( ( ( _crc & 0x8000 ) == 0 ) ^ ( ( bitstring & bitmask ) == 0 ) )
            {
                _crc <<= 1;
                _crc ^=  _polynomial;
            }
            else
            {
                _crc <<= 1;
            }
        }
        while ( ( bitmask = SupportClass.URShift( bitmask, 1 ) ) != 0 );
    }

    /// <summary>
    /// Return the calculated checksum. Erase it for next calls to add_bits().
    /// </summary>
    public short Checksum()
    {
        var sum = _crc;

        _crc = ( short ) SupportClass.Identity( 0xFFFF );

        return sum;
    }
}
