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


namespace Corelib.LibCore.Utils;

[PublicAPI]
public class DataInput : BinaryReader
{
    private char[] _chars = new char[ 32 ];

    public DataInput( Stream input ) : base( input )
    {
    }

    /// <summary>
    /// Reads a 1-5 byte int.
    /// </summary>
    public int ReadInt( bool optimizePositive )
    {
        var b      = Read();
        var result = b & 0x7F;

        if ( ( b & 0x80 ) != 0 )
        {
            b      =  Read();
            result |= ( b & 0x7F ) << 7;

            if ( ( b & 0x80 ) != 0 )
            {
                b      =  Read();
                result |= ( b & 0x7F ) << 14;

                if ( ( b & 0x80 ) != 0 )
                {
                    b      =  Read();
                    result |= ( b & 0x7F ) << 21;

                    if ( ( b & 0x80 ) != 0 )
                    {
                        b      =  Read();
                        result |= ( b & 0x7F ) << 28;
                    }
                }
            }
        }

        return optimizePositive ? result : ( result >>> 1 ) ^ -( result & 1 );
    }

    /// <summary>
    /// Reads the length and string of UTF8 characters, or null.
    /// </summary>
    public string? ReadStringUTF8()
    {
        var charCount = ReadInt( true );

        if ( charCount == 0 )
        {
            return null;
        }

        if ( charCount == 1 )
        {
            return "";
        }

        charCount--;

        if ( _chars.Length < charCount )
        {
            _chars = new char[ charCount ];
        }

        // Try to read 7 bit ASCII chars.
        var charIndex = 0;
        var b         = 0;

        while ( charIndex < charCount )
        {
            b = Read();

            if ( b > 127 )
            {
                break;
            }

            _chars[ charIndex++ ] = ( char ) b;
        }

        // If a char was not ASCII, finish with slow path.
        if ( charIndex < charCount )
        {
            ReadUtf8Slow( charCount, charIndex, b );
        }

        return new string( _chars, 0, charCount );
    }

    /// <summary>
    /// </summary>
    /// <param name="charCount"></param>
    /// <param name="charIndex"></param>
    /// <param name="b"></param>
    private void ReadUtf8Slow( int charCount, int charIndex, int b )
    {
        while ( true )
        {
            switch ( b >> 4 )
            {
                case >= 0 and <= 7:
                {
                    _chars[ charIndex ] = ( char ) b;

                    break;
                }

                case 12 or 13:
                {
                    _chars[ charIndex ] = ( char ) ( ( ( b & 0x1F ) << 6 ) | ( Read() & 0x3F ) );

                    break;
                }

                case 14:
                {
                    _chars[ charIndex ] = ( char ) ( ( ( b & 0x0F ) << 12 )
                                                   | ( ( Read() & 0x3F ) << 6 )
                                                   | ( Read() & 0x3F ) );

                    break;
                }
            }

            if ( ++charIndex >= charCount )
            {
                break;
            }

            b = Read() & 0xFF;
        }
    }
}
