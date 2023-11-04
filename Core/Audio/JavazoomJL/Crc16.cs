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
public class Crc16
{
    private static int _polynomial = 0x8005;
    private        int _crc        = 0xFFFF;

    /// <summary>
    /// Feed a bitstring to the crc calculation (0 &lt; length &lt;= 32).
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
        
        while ( ( bitmask >>>= 1 ) != 0 );
    }

    /// <summary>
    /// Return the calculated checksum. Erase it for next calls to add_bits().
    /// </summary>
    public int Checksum()
    {
        var sum = _crc;

        _crc = 0xFFFF;

        return sum;
    }
}
