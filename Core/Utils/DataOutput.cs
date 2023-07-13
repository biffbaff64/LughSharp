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
/// Extends <see cref="BinaryWriter"/> with additional convenience methods.
/// </summary>
public class DataOutput : BinaryWriter
{
    public DataOutput( StreamWriter output )
    {
    }

    /// <summary>
    /// Writes a 1-5 byte int.
    /// </summary>
    ///<param name="value"></param>
    /// <param name="optimizePositive"> If true, small positive numbers will be more efficient (1 byte) and small negative numbers will be
    ///           inefficient (5 bytes).  </param>
    public int WriteInt( int value, bool optimizePositive )
    {
        if ( !optimizePositive )
        {
            value = ( value << 1 ) ^ ( value >> 31 );
        }

        if ( ( value >>> 7 ) == 0 )
        {
            Write( ( sbyte )value );

            return 1;
        }

        Write( unchecked( ( sbyte )( ( value & 0x7F ) | 0x80 ) ) );

        if ( ( value >>> 14 ) == 0 )
        {
            Write( ( sbyte )( value >>> 7 ) );

            return 2;
        }

        Write( unchecked( ( sbyte )( ( value >>> 7 ) | 0x80 ) ) );

        if ( ( value >>> 21 ) == 0 )
        {
            Write( ( sbyte )( value >>> 14 ) );

            return 3;
        }

        Write( unchecked( ( sbyte )( ( value >>> 14 ) | 0x80 ) ) );

        if ( ( value >>> 28 ) == 0 )
        {
            Write( ( sbyte )( value >>> 21 ) );

            return 4;
        }

        Write( unchecked( ( sbyte )( ( value >>> 21 ) | 0x80 ) ) );
        Write( ( sbyte )( value >>> 28 ) );

        return 5;
    }

    /// <summary>
    /// Writes a length and then the string as UTF8. </summary>
    /// <param name="value"> May be null.  </param>
    public void WriteString( string? value )
    {
        if ( string.ReferenceEquals( value, null ) )
        {
            Write( 0 );

            return;
        }

        var charCount = value.Length;

        if ( charCount == 0 )
        {
            Write( (byte)1 );

            return;
        }

        WriteInt( charCount + 1, true );

        // Try to Write 8 bit chars.
        var charIndex = 0;

        for ( ; charIndex < charCount; charIndex++ )
        {
            int c = value[ charIndex ];

            if ( c > 127 )
            {
                break;
            }

            Write( ( sbyte )c );
        }

        if ( charIndex < charCount )
        {
            WriteStringSlow( value, charCount, charIndex );
        }
    }

    private void WriteStringSlow( string value, int charCount, int charIndex )
    {
        for ( ; charIndex < charCount; charIndex++ )
        {
            int c = value[ charIndex ];

            if ( c <= 0x007F )
            {
                Write( ( sbyte )c );
            }
            else if ( c > 0x07FF )
            {
                Write( unchecked( ( sbyte )( 0xE0 | ( ( c >> 12 ) & 0x0F ) ) ) );
                Write( unchecked( ( sbyte )( 0x80 | ( ( c >> 6 ) & 0x3F ) ) ) );
                Write( unchecked( ( sbyte )( 0x80 | ( c & 0x3F ) ) ) );
            }
            else
            {
                Write( unchecked( ( sbyte )( 0xC0 | ( ( c >> 6 ) & 0x1F ) ) ) );
                Write( unchecked( ( sbyte )( 0x80 | ( c & 0x3F ) ) ) );
            }
        }
    }
}
