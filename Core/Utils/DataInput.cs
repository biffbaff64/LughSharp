namespace LibGDXSharp.Utils;

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

        switch ( charCount )
        {
            case 0:
                return null;

            case 1:
                return "";
        }

        charCount--;

        if ( _chars.Length < charCount )
        {
            _chars = new char[ charCount ];
        }

        // Try to read 7 bit ASCII chars.
        int charIndex = 0;
        int b         = 0;

        while ( charIndex < charCount )
        {
            b = Read();

            if ( b > 127 )
            {
                break;
            }

            _chars[ charIndex++ ] = ( char )b;
        }

        // If a char was not ASCII, finish with slow path.
        if ( charIndex < charCount )
        {
            ReadUtf8Slow( charCount, charIndex, b );
        }

        return new string( _chars, 0, charCount );
    }

    private void ReadUtf8Slow( int charCount, int charIndex, int b )
    {
        while ( true )
        {
            switch ( b >> 4 )
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    _chars[ charIndex ] = ( char )b;

                    break;

                case 12:
                case 13:
                    _chars[ charIndex ] = ( char )( ( ( b & 0x1F ) << 6 ) | ( Read() & 0x3F ) );

                    break;

                case 14:
                    _chars[ charIndex ] = ( char )( ( ( b & 0x0F ) << 12 ) 
                                                    | ( ( Read() & 0x3F ) << 6 ) 
                                                    | ( Read() & 0x3F ) );

                    break;
            }

            if ( ++charIndex >= charCount )
            {
                break;
            }

            b = Read() & 0xFF;
        }
    }
}
