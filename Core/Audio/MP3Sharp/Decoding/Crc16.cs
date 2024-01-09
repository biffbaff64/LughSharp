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

using LibGDXSharp.Audio.MP3Sharp.Support;

namespace LibGDXSharp.Audio.MP3Sharp;

/// <summary>
///     16-Bit CRC checksum
/// </summary>
public class Crc16
{
    private readonly static short Polynomial;

    private short _crc = ( short )SupportClass.Identity( 0xFFFF );

    static Crc16() => Polynomial = ( short )SupportClass.Identity( 0x8005 );

    /// <summary>
    ///     Feed a bitstring to the crc calculation (length between 0 and 32, not inclusive).
    /// </summary>
    public void AddBits( int bitstring, int length )
    {
        var bitmask = 1 << ( length - 1 );

        do
        {
            if ( ( ( _crc & 0x8000 ) == 0 ) ^ ( ( bitstring & bitmask ) == 0 ) )
            {
                _crc <<= 1;
                _crc ^=  Polynomial;
            }
            else
            {
                _crc <<= 1;
            }
        }
        while ( ( bitmask = SupportClass.URShift( bitmask, 1 ) ) != 0 );
    }

    /// <summary>
    ///     Return the calculated checksum.
    ///     Erase it for next calls to add_bits().
    /// </summary>
    public short Checksum()
    {
        var sum = _crc;

        _crc = ( short )SupportClass.Identity( 0xFFFF );

        return sum;
    }
}
