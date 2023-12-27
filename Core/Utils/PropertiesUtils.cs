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

using System.Text;

using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Utils;

/// <summary>
/// A helper class that allows you to load and store key/value pairs of an
/// <see cref="Dictionary{TK,TV}"/> with the same line-oriented syntax supported
/// by <see cref="IPreferences"/>
/// </summary>
[PublicAPI]
public class PropertiesUtils
{
    private const int    NONE           = 0;
    private const int    SLASH          = 1;
    private const int    UNICODE        = 2;
    private const int    CONTINUE       = 3;
    private const int    KEY_DONE       = 4;
    private const int    IGNORE         = 5;
    private const string LINE_SEPARATOR = "\n";

    private PropertiesUtils()
    {
    }

    /// <summary>
    /// Adds to the specified <see cref="Dictionary{T,K}"/> the key/value pairs
    /// loaded from the <see cref="StreamReader"/> in a simple line-oriented format.
    /// <para>
    /// The input stream remains open after this method returns.
    /// </para>
    /// </summary>
    /// <param name="properties"> the map to be filled. </param>
    /// <param name="reader"> the input character stream reader. </param>
    /// <exception cref="IOException"> if an error occurred when reading from the input stream. </exception>
    /// <exception cref="ArgumentException"> if a malformed Unicode escape appears in the input.  </exception>
    public static void Load( Dictionary< string, string > properties, StreamReader reader )
    {
        ArgumentNullException.ThrowIfNull( properties );
        ArgumentNullException.ThrowIfNull( reader );

        var mode      = NONE;
        var unicode   = 0;
        var count     = 0;
        var buf       = new char[ 40 ];
        var offset    = 0;
        var keyLength = -1;
        var firstChar = true;

//        var br = new StreamReader( reader );

        while ( true )
        {
            var intVal = reader.Read();

            if ( intVal == -1 )
            {
                break;
            }

            var nextChar = ( char )intVal;

            if ( offset == buf.Length )
            {
                var newBuf = new char[ buf.Length * 2 ];

                Array.Copy( buf, 0, newBuf, 0, offset );

                buf = newBuf;
            }

            switch ( mode )
            {
                case UNICODE:
                {
                    var num   = char.GetNumericValue( nextChar.ToString(), 16 );
                    var digit = ( int )num;

                    switch ( digit )
                    {
                        case >= 0:
                        {
                            unicode = ( unicode << 4 ) + digit;

                            if ( ++count < 4 )
                            {
                                continue;
                            }

                            break;
                        }

                        default:
                        {
                            switch ( count )
                            {
                                case <= 4:
                                    throw new ArgumentException( "Invalid Unicode sequence: illegal character" );
                            }

                            break;
                        }
                    }

                    mode            = NONE;
                    buf[ offset++ ] = ( char )unicode;

                    if ( nextChar != '\n' )
                    {
                        continue;
                    }

                    break;
                }
            }

            switch ( mode )
            {
                case SLASH:
                    mode = NONE;

                    switch ( nextChar )
                    {
                        case '\r':
                            mode = CONTINUE; // Look for a following \n

                            continue;

                        case '\n':
                            mode = IGNORE; // Ignore whitespace on the next line

                            continue;

                        case 'b':
                            nextChar = '\b';

                            break;

                        case 'f':
                            nextChar = '\f';

                            break;

                        case 'n':
                            nextChar = '\n';

                            break;

                        case 'r':
                            nextChar = '\r';

                            break;

                        case 't':
                            nextChar = '\t';

                            break;

                        case 'u':
                            mode    = UNICODE;
                            unicode = count = 0;

                            continue;
                    }

                    break;

                default:
                {
                    switch ( nextChar )
                    {
                        case '#':
                        case '!':
                            switch ( firstChar )
                            {
                                case true:
                                {
                                    while ( true )
                                    {
                                        intVal = reader.Read();

                                        if ( intVal == -1 )
                                        {
                                            break;
                                        }

                                        nextChar = ( char )intVal;

                                        if ( nextChar is '\r' or '\n' )
                                        {
                                            break;
                                        }
                                    }

                                    continue;
                                }
                            }

                            break;

                        case '\n':
                        case '\r':
                            switch ( nextChar )
                            {
                                case '\n' when mode == CONTINUE:
                                    // Part of a \r\n sequence
                                    // Ignore whitespace on the next line
                                    mode = IGNORE;

                                    continue;
                            }

                            mode      = NONE;
                            firstChar = true;

                            // 'r'
                        {
                            switch ( offset )
                            {
                                case > 0:
                                case 0 when ( keyLength == 0 ):
                                {
                                    keyLength = keyLength switch
                                                {
                                                    -1 => offset,
                                                    _  => keyLength
                                                };

                                    var temp = new string( buf, 0, offset );

                                    properties.Put( temp.Substring( 0, keyLength ), temp.Substring( keyLength ) );

                                    break;
                                }
                            }

                            keyLength = -1;
                            offset    = 0;

                            continue;
                        }

                        case '\\':
                            keyLength = mode switch
                                        {
                                            KEY_DONE => offset,
                                            _        => keyLength
                                        };

                            mode = SLASH;

                            continue;

                        case ':':
                        case '=':
                            switch ( keyLength )
                            {
                                case -1:
                                    // if parsing the key
                                    mode      = NONE;
                                    keyLength = offset;

                                    continue;
                            }

                            break;
                    }

                    if ( char.IsWhiteSpace( nextChar ) )
                    {
                        mode = mode switch
                               {
                                   CONTINUE => IGNORE,
                                   _        => mode
                               };

                        if ( ( offset == 0 ) || ( offset == keyLength ) || ( mode == IGNORE ) )
                        {
                            continue;
                        }

                        switch ( keyLength )
                        {
                            case -1:
                                // if parsing the key
                                mode = KEY_DONE;

                                continue;
                        }
                    }

                    switch ( mode )
                    {
                        case IGNORE:
                        case CONTINUE:
                            mode = NONE;

                            break;
                    }

                    break;
                }
            }

            firstChar = false;

            switch ( mode )
            {
                case KEY_DONE:
                    keyLength = offset;
                    mode      = NONE;

                    break;
            }

            buf[ offset++ ] = nextChar;
        }

        switch ( mode )
        {
            case UNICODE when ( count <= 4 ):
                throw new ArgumentException( "Invalid Unicode sequence: expected format \\uxxxx" );
        }

        keyLength = keyLength switch
                    {
                        -1 when ( offset > 0 ) => offset,
                        _                      => keyLength
                    };

        switch ( keyLength )
        {
            case >= 0:
            {
                var temp  = new string( buf, 0, offset );
                var key   = temp.Substring( 0, keyLength );
                var value = temp.Substring( keyLength );

                switch ( mode )
                {
                    case SLASH:
                        value += "\u0000";

                        break;
                }

                properties.Put( key, value );

                break;
            }
        }
    }

    /// <summary>
    /// Writes the key/value pairs of the specified <see cref="Dictionary{T,K}"/> to
    /// the output character stream in a simple line-oriented format.
    /// <para>
    /// Every entry in the <tt>Dictionary</tt> is written out, one per line. For each
    /// entry the key string is written, then an <tt>ASCII =</tt>, then the associated
    /// element string. For the key, all space characters are written with a preceding
    /// <tt>\ (back-slash)</tt> character. For the element, leading space characters,
    /// but not embedded or trailing space characters, are written with a preceding
    /// <tt>\ (back-slash)</tt> character. The key and element characters <tt>#</tt>, <tt>!</tt>,
    /// <tt>=</tt>, and <tt>:</tt> are written with a preceding backslash to ensure that
    /// they are properly loaded.
    /// </para>
    /// <para>
    /// After the entries have been written, the output stream is flushed. The output
    /// stream remains open after this method returns.
    /// </para>
    /// </summary>
    /// <param name="properties"> the Dictionary. </param>
    /// <param name="writer"> an output character stream writer. </param>
    /// <param name="comment"> an optional comment to be written, or null. </param>
    /// <exception cref="IOException">
    /// if writing this property list to the specified output stream throws an <tt>IOException</tt>.
    /// </exception>
    /// <exception cref="NullReferenceException"> if <code>writer</code> is null.</exception>
    public static void Store( Dictionary< string, string > properties, StreamWriter writer, string comment )
    {
        StoreImpl( properties, writer, comment, false );
    }

    private static void StoreImpl( Dictionary< string, string > properties,
                                   StreamWriter writer,
                                   string? comment,
                                   bool escapeUnicode )
    {
        if ( comment != null )
        {
            WriteComment( writer, comment );
        }

        writer.Write( "#" );
        writer.Write( DateTime.Now.ToString( "ddd MM/dd/yyyy h:mm tt" ) );
        writer.Write( LINE_SEPARATOR );

        var sb = new StringBuilder( 200 );

        foreach ( KeyValuePair< string, string > entry in properties )
        {
            DumpString( sb, entry.Key, true, escapeUnicode );
            sb.Append( '=' );
            DumpString( sb, entry.Value, false, escapeUnicode );
            writer.Write( LINE_SEPARATOR );
            writer.Write( sb.ToString() );
            sb.Clear();
        }

        writer.Flush();
    }

    private static void DumpString( StringBuilder outBuffer, string str, bool escapeSpace, bool escapeUnicode )
    {
        var len = str.Length;

        for ( var i = 0; i < len; i++ )
        {
            var ch = str[ i ];

            // Handle common case first
            if ( ( ch > 61 ) && ( ch < 127 ) )
            {
                outBuffer.Append( ch == '\\' ? "\\\\" : ch );

                continue;
            }

            switch ( ch )
            {
                case ' ':
                    if ( ( i == 0 ) || escapeSpace )
                    {
                        outBuffer.Append( "\\ " );
                    }
                    else
                    {
                        outBuffer.Append( ch );
                    }

                    break;

                case '\n':
                    outBuffer.Append( "\\n" );

                    break;

                case '\r':
                    outBuffer.Append( "\\r" );

                    break;

                case '\t':
                    outBuffer.Append( "\\t" );

                    break;

                case '\f':
                    outBuffer.Append( "\\f" );

                    break;

                case '=': // Fall through
                case ':': // Fall through
                case '#': // Fall through
                case '!':
                    outBuffer.Append( '\\' ).Append( ch );

                    break;

                default:
                    switch ( ( ( ch < 0x0020 ) || ( ch > 0x007e ) ) & escapeUnicode )
                    {
                        case true:
                        {
                            var hex = ( ( int )ch ).ToString( "X" );

                            outBuffer.Append( "\\u" );

                            for ( var j = 0; j < ( 4 - hex.Length ); j++ )
                            {
                                outBuffer.Append( '0' );
                            }

                            outBuffer.Append( hex );

                            break;
                        }

                        default:
                            outBuffer.Append( ch );

                            break;
                    }

                    break;
            }
        }
    }

    private static void WriteComment( StreamWriter writer, string comment )
    {
        writer.Write( "#" );

        var len       = comment.Length;
        var curIndex  = 0;
        var lastIndex = 0;

        while ( curIndex < len )
        {
            var c = comment[ curIndex ];

            switch ( c )
            {
                case > '\u00ff':
                case '\n':
                case '\r':
                {
                    if ( lastIndex != curIndex )
                    {
                        writer.Write( comment.Substring( lastIndex, curIndex ) );
                    }

                    switch ( c )
                    {
                        case > '\u00ff':
                        {
                            var hex = ( ( int )c ).ToString( "X" );

                            writer.Write( "\\u" );

                            for ( var j = 0; j < ( 4 - hex.Length ); j++ )
                            {
                                writer.Write( '0' );
                            }

                            writer.Write( hex );

                            break;
                        }

                        default:
                        {
                            writer.Write( LINE_SEPARATOR );

                            switch ( c )
                            {
                                case '\r' when ( curIndex != ( len - 1 ) ) && ( comment[ curIndex + 1 ] == '\n' ):
                                    curIndex++;

                                    break;
                            }

                            if ( ( curIndex == ( len - 1 ) )
                              || ( ( comment[ curIndex + 1 ] != '#' ) && ( comment[ curIndex + 1 ] != '!' ) ) )
                            {
                                writer.Write( "#" );
                            }

                            break;
                        }
                    }

                    lastIndex = curIndex + 1;

                    break;
                }
            }

            curIndex++;
        }

        if ( lastIndex != curIndex )
        {
            writer.Write( comment.Substring( lastIndex, curIndex ) );
        }

        writer.Write( LINE_SEPARATOR );
    }
}
