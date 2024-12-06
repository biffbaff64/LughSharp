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


namespace Corelib.Lugh.Utils;

/// <summary>
/// Extends <see cref="BinaryWriter"/> with additional convenience methods.
/// </summary>
[PublicAPI]
public class DataOutput : BinaryWriter
{
    /// <summary>
    /// Constructs a new DataOuput instance with the given output stream.
    /// </summary>
    /// <param name="output"></param>
    public DataOutput( Stream output ) : base( output )
    {
    }

    /// <summary>
    /// Writes a 1-5 byte int.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="optimizePositive">
    /// If true, small positive numbers will be more efficient (1 byte) and
    /// small negative numbers will be inefficient (5 bytes).
    /// </param>
    public void WriteInt( int value, bool optimizePositive )
    {
        if ( !optimizePositive )
        {
            value = ( value << 1 ) ^ ( value >> 31 );
        }

        if ( ( value >>> 7 ) == 0 )
        {
            Write( ( sbyte ) value );
        }

        Write( unchecked( ( sbyte ) ( ( value & 0x7F ) | 0x80 ) ) );

        if ( ( value >>> 14 ) == 0 )
        {
            Write( ( sbyte ) ( value >>> 7 ) );
        }

        Write( unchecked( ( sbyte ) ( ( value >>> 7 ) | 0x80 ) ) );

        if ( ( value >>> 21 ) == 0 )
        {
            Write( ( sbyte ) ( value >>> 14 ) );
        }

        Write( unchecked( ( sbyte ) ( ( value >>> 14 ) | 0x80 ) ) );

        if ( ( value >>> 28 ) == 0 )
        {
            Write( ( sbyte ) ( value >>> 21 ) );
        }

        Write( unchecked( ( sbyte ) ( ( value >>> 21 ) | 0x80 ) ) );
        Write( ( sbyte ) ( value >>> 28 ) );
    }

    /// <summary>
    /// Writes a length and then the string as UTF8.
    /// </summary>
    /// <param name="value"> May be null.  </param>
    public void WriteString( string? value )
    {
        if ( value == null )
        {
            Write( 0 );

            return;
        }

        var charCount = value.Length;

        if ( charCount == 0 )
        {
            Write( ( byte ) 1 );

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

            Write( ( sbyte ) c );
        }

        if ( charIndex < charCount )
        {
            WriteStringSlow( value, charCount, charIndex );
        }
    }

    /// <summary>
    /// Writes a string as UTF8 beyond the first characters that were handled as 8-bit characters.
    /// </summary>
    /// <param name="value">The string to write.</param>
    /// <param name="charCount">The total number of characters in the string.</param>
    /// <param name="charIndex">The index from which to start writing remaining characters.</param>
    public void WriteStringSlow( string value, int charCount, int charIndex )
    {
        for ( ; charIndex < charCount; charIndex++ )
        {
            int c = value[ charIndex ];

            switch ( c )
            {
                case <= 0x007F:
                {
                    Write( ( sbyte ) c );

                    break;
                }

                case > 0x07FF:
                {
                    Write( unchecked( ( sbyte ) ( 0xE0 | ( ( c >> 12 ) & 0x0F ) ) ) );
                    Write( unchecked( ( sbyte ) ( 0x80 | ( ( c >> 6 ) & 0x3F ) ) ) );
                    Write( unchecked( ( sbyte ) ( 0x80 | ( c & 0x3F ) ) ) );

                    break;
                }

                default:
                {
                    Write( unchecked( ( sbyte ) ( 0xC0 | ( ( c >> 6 ) & 0x1F ) ) ) );
                    Write( unchecked( ( sbyte ) ( 0x80 | ( c & 0x3F ) ) ) );

                    break;
                }
            }
        }
    }
}
