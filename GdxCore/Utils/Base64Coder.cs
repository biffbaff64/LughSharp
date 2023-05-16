namespace LibGDXSharp.Utils;

public class Base64Coder
{
    public class CharMap
    {
        protected char[] EncodingMap { get; set; } = new char[ 64 ];
        protected byte[] DecodingMap { get; set; } = new byte[ 128 ];

        public CharMap( char char63, char char64 )
        {
            int i = 0;

            for ( char c = 'A'; c <= 'Z'; c++ )
            {
                EncodingMap[ i++ ] = c;
            }

            for ( char c = 'a'; c <= 'z'; c++ )
            {
                EncodingMap[ i++ ] = c;
            }

            for ( char c = '0'; c <= '9'; c++ )
            {
                EncodingMap[ i++ ] = c;
            }

            EncodingMap[ i++ ] = char63;
            EncodingMap[ i++ ] = char64;

            for ( i = 0; i < DecodingMap.Length; i++ )
            {
                DecodingMap[ i ] = -1;
            }

            for ( i = 0; i < 64; i++ )
            {
                DecodingMap[ EncodingMap[ i ] ] = ( byte )i;
            }
        }
    }

    // The line separator string of the operating system.
    private const string SystemLineSeparator = "\n";

    public readonly static CharMap RegularMap = new CharMap( '+', '/' ), UrlsafeMap = new CharMap( '-', '_' );

    /// <summary>
    /// Encodes a string into Base64 format, optionally using URL-safe encoding instead of the "regular" Base64 encoding.
    /// No blanks or line breaks are inserted. </summary>
    /// <param name="s"> A String to be encoded. </param>
    /// <param name="useUrlsafeEncoding"> If true, this encodes the result with an alternate URL-safe set of characters. </param>
    /// <returns> A String containing the Base64 encoded data.  </returns>
    public static string EncodeString( string s, bool useUrlsafeEncoding = false )
    {
        try
        {
            return new string
                ( encode( s.GetBytes( Encoding.UTF8 ), useUrlsafeEncoding ? urlsafeMap.encodingMap : regularMap.encodingMap ) );
        }
        catch ( UnsupportedEncodingException )
        {
            // shouldn't ever happen; only needed because we specify an encoding with a String
            return "";
        }
    }

    /// <summary>
    /// Encodes a byte array into Base64 format and breaks the output into lines of 76 characters. This method is compatible with
    /// <code>sun.misc.BASE64Encoder.encodeBuffer(byte[])</code>. </summary>
    /// <param name="in"> An array containing the data bytes to be encoded. </param>
    /// <returns> A String containing the Base64 encoded data, broken into lines.  </returns>
    public static string EncodeLines( sbyte[] @in )
    {
        return encodeLines( @in, 0, @in.Length, 76, systemLineSeparator, regularMap.encodingMap );
    }

    public static string EncodeLines( sbyte[] @in, int iOff, int iLen, int lineLen, string lineSeparator, CharMap charMap )
    {
        return encodeLines( @in, iOff, iLen, lineLen, lineSeparator, charMap.encodingMap );
    }

    /// <summary>
    /// Encodes a byte array into Base64 format and breaks the output into lines. </summary>
    /// <param name="in"> An array containing the data bytes to be encoded. </param>
    /// <param name="iOff"> Offset of the first byte in <code>in</code> to be processed. </param>
    /// <param name="iLen"> Number of bytes to be processed in <code>in</code>, starting at <code>iOff</code>. </param>
    /// <param name="lineLen"> Line length for the output data. Should be a multiple of 4. </param>
    /// <param name="lineSeparator"> The line separator to be used to separate the output lines. </param>
    /// <param name="charMap"> char map to use </param>
    /// <returns> A String containing the Base64 encoded data, broken into lines.  </returns>
    public static string EncodeLines( sbyte[] @in, int iOff, int iLen, int lineLen, string lineSeparator, char[] charMap )
    {
        int blockLen = ( lineLen * 3 ) / 4;

        if ( blockLen <= 0 )
        {
            throw new System.ArgumentException();
        }

        int           lines  = ( iLen + blockLen - 1 ) / blockLen;
        int           bufLen = ( ( iLen + 2 ) / 3 ) * 4 + lines * lineSeparator.Length;
        StringBuilder buf    = new StringBuilder( bufLen );
        int           ip     = 0;

        while ( ip < iLen )
        {
            int l = Math.Min( iLen - ip, blockLen );
            buf.Append( Encode( @in, iOff + ip, l, charMap ) );
            buf.Append( lineSeparator );
            ip += l;
        }

        return buf.ToString();
    }

    /// <summary>
    /// Encodes a byte array into Base64 format. No blanks or line breaks are inserted in the output. </summary>
    /// <param name="in"> An array containing the data bytes to be encoded. </param>
    /// <returns> A character array containing the Base64 encoded data.  </returns>
    public static char[] Encode( sbyte[] @in )
    {
        return encode( @in, regularMap.encodingMap );
    }

    public static char[] Encode( sbyte[] @in, CharMap charMap )
    {
        return Encode( @in, 0, @in.Length, charMap );
    }

    public static char[] Encode( sbyte[] @in, char[] charMap )
    {
        return Encode( @in, 0, @in.Length, charMap );
    }

    /// <summary>
    /// Encodes a byte array into Base64 format. No blanks or line breaks are inserted in the output. </summary>
    /// <param name="in"> An array containing the data bytes to be encoded. </param>
    /// <param name="iLen"> Number of bytes to process in <code>in</code>. </param>
    /// <returns> A character array containing the Base64 encoded data.  </returns>
    public static char[] Encode( sbyte[] @in, int iLen )
    {
        return Encode( @in, 0, iLen, regularMap.encodingMap );
    }

    public static char[] Encode( sbyte[] @in, int iOff, int iLen, CharMap charMap )
    {
        return Encode( @in, iOff, iLen, charMap.encodingMap );
    }
}
