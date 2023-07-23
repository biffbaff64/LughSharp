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

using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Utils;

/// <summary>
/// A helper class that allows you to load and store key/value pairs of an
/// <see cref="Dictionary{TK,TV}"/> with the same line-oriented syntax supported
/// by <see cref="IPreferences"/>
/// </summary>
public class PropertiesUtils
{
    private const int    None           = 0;
    private const int    Slash          = 1;
    private const int    Unicode        = 2;
    private const int    Continue       = 3;
    private const int    Key_Done       = 4;
    private const int    Ignore         = 5;
    private const string Line_Separator = "\n";

    private PropertiesUtils()
    {
    }

    /// <summary>
    /// Adds to the specified <see cref="Dictionary{T,K}"/> the key/value pairs
    /// loaded from the <see cref="Reader"/> in a simple line-oriented format.
    /// <para>
    /// The input stream remains open after this method returns.
    /// </para>
    /// </summary>
    /// <param name="properties"> the map to be filled. </param>
    /// <param name="reader"> the input character stream reader. </param>
    /// <exception cref="IOException"> if an error occurred when reading from the input stream. </exception>
    /// <exception cref="ArgumentException"> if a malformed Unicode escape appears in the input.  </exception>
    public static void Load( Dictionary< string, string > properties, Reader reader )
    {
        ArgumentNullException.ThrowIfNull( properties );
        ArgumentNullException.ThrowIfNull( reader );

        var mode      = None;
        var unicode   = 0;
        var count     = 0;
        var buf       = new char[ 40 ];
        var offset    = 0;
        var keyLength = -1;
        var firstChar = true;

        BufferedReader br = new BufferedReader( reader );

        while ( true )
        {
            int intVal = br.Read();

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

            if ( mode == Unicode )
            {
                var num = char.GetNumericValue( nextChar.ToString(), 16 );
                var digit = ( int )num;
                
                if ( digit >= 0 )
                {
                    unicode = ( unicode << 4 ) + digit;

                    if ( ++count < 4 )
                    {
                        continue;
                    }
                }
                else if ( count <= 4 )
                {
                    throw new ArgumentException( "Invalid Unicode sequence: illegal character" );
                }

                mode            = None;
                buf[ offset++ ] = ( char )unicode;

                if ( nextChar != '\n' )
                {
                    continue;
                }
            }

            if ( mode == Slash )
            {
                mode = None;

                switch ( nextChar )
                {
                    case '\r':
                        mode = Continue; // Look for a following \n

                        continue;

                    case '\n':
                        mode = Ignore; // Ignore whitespace on the next line

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
                        mode    = Unicode;
                        unicode = count = 0;

                        continue;
                }
            }
            else
            {
                switch ( nextChar )
                {
                    case '#':
                    case '!':
                        if ( firstChar )
                        {
                            while ( true )
                            {
                                intVal = br.Read();

                                if ( intVal == -1 )
                                {
                                    break;
                                }

                                nextChar = ( char )intVal;

                                if ( ( nextChar == '\r' ) || ( nextChar == '\n' ) )
                                {
                                    break;
                                }
                            }

                            continue;
                        }

                        break;

                    case '\n':
                    case '\r':
                        if ( nextChar == '\n' )
                        {
                            if ( mode == Continue )
                            {
                                // Part of a \r\n sequence
                                // Ignore whitespace on the next line
                                mode = Ignore;

                                continue;
                            }
                        }

                        mode      = None;
                        firstChar = true;

                        // 'r'
                        {
                            if ( ( offset > 0 ) || ( ( offset == 0 ) && ( keyLength == 0 ) ) )
                            {
                                if ( keyLength == -1 )
                                {
                                    keyLength = offset;
                                }

                                var temp = new string( buf, 0, offset );

                                properties.Put( temp.Substring( 0, keyLength ), temp.Substring( keyLength ) );
                            }

                            keyLength = -1;
                            offset    = 0;

                            continue;
                        }

                    case '\\':
                        if ( mode == Key_Done )
                        {
                            keyLength = offset;
                        }

                        mode = Slash;

                        continue;

                    case ':':
                    case '=':
                        if ( keyLength == -1 )
                        {
                            // if parsing the key
                            mode      = None;
                            keyLength = offset;

                            continue;
                        }

                        break;
                }

                // if (Character.isWhitespace(nextChar)) { <-- not supported by GWT; replaced with isSpace.
                // TODO: ^^
                if ( char.IsWhiteSpace( nextChar ) )
                {
                    if ( mode == Continue )
                    {
                        mode = Ignore;
                    }

                    // if key length == 0 or value length == 0
                    if ( ( offset == 0 ) || ( offset == keyLength ) || ( mode == Ignore ) )
                    {
                        continue;
                    }

                    if ( keyLength == -1 )
                    {
                        // if parsing the key
                        mode = Key_Done;

                        continue;
                    }
                }

                if ( ( mode == Ignore ) || ( mode == Continue ) )
                {
                    mode = None;
                }
            }

            firstChar = false;

            if ( mode == Key_Done )
            {
                keyLength = offset;
                mode      = None;
            }

            buf[ offset++ ] = nextChar;
        }

        if ( ( mode == Unicode ) && ( count <= 4 ) )
        {
            throw new ArgumentException( "Invalid Unicode sequence: expected format \\uxxxx" );
        }

        if ( ( keyLength == -1 ) && ( offset > 0 ) )
        {
            keyLength = offset;
        }

        if ( keyLength >= 0 )
        {
            var temp  = new string( buf, 0, offset );
            var key   = temp.Substring( 0, keyLength );
            var value = temp.Substring( keyLength );

            if ( mode == Slash )
            {
                value += "\u0000";
            }

            properties.Put( key, value );
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
    public static void Store( Dictionary< string, string > properties, Writer writer, string comment )
    {
        StoreImpl( properties, writer, comment, false );
    }

    private static void StoreImpl( Dictionary< string, string > properties, Writer writer,
                                   string? comment, bool escapeUnicode )
    {
        if ( comment != null )
        {
            WriteComment( writer, comment );
        }

        writer.Write( "#" );
        writer.Write( DateTime.Now.ToString( "ddd MM/dd/yyyy h:mm tt" ) );
        writer.Write( Line_Separator );

        var sb = new StringBuilder( 200 );

        foreach ( KeyValuePair< string, string > entry in properties )
        {
            DumpString( sb, entry.Key, true, escapeUnicode );
            sb.Append( '=' );
            DumpString( sb, entry.Value, false, escapeUnicode );
            writer.Write( Line_Separator );
            writer.Write( sb.ToString() );
            sb.Clear();
        }

        writer.Flush();
    }

    private static void DumpString( StringBuilder outBuffer, string str, bool escapeSpace, bool escapeUnicode )
    {
        int len = str.Length;

        for ( var i = 0; i < len; i++ )
        {
            char ch = str[ i ];

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
                    if ( ( ( ch < 0x0020 ) || ( ch > 0x007e ) ) & escapeUnicode )
                    {
                        var hex = ( ( int )ch ).ToString( "X" );

                        outBuffer.Append( "\\u" );

                        for ( var j = 0; j < ( 4 - hex.Length ); j++ )
                        {
                            outBuffer.Append( '0' );
                        }

                        outBuffer.Append( hex );
                    }
                    else
                    {
                        outBuffer.Append( ch );
                    }

                    break;
            }
        }
    }

    private static void WriteComment( Writer writer, string comment )
    {
        writer.Write( "#" );

        var len       = comment.Length;
        var curIndex  = 0;
        var lastIndex = 0;

        while ( curIndex < len )
        {
            var c = comment[ curIndex ];

            if ( ( c > '\u00ff' ) || ( c == '\n' ) || ( c == '\r' ) )
            {
                if ( lastIndex != curIndex )
                {
                    writer.Write( comment.Substring( lastIndex, curIndex ) );
                }

                if ( c > '\u00ff' )
                {
                    var hex = ( ( int )c ).ToString( "X" );

                    writer.Write( "\\u" );

                    for ( var j = 0; j < ( 4 - hex.Length ); j++ )
                    {
                        writer.Write( '0' );
                    }

                    writer.Write( hex );
                }
                else
                {
                    writer.Write( Line_Separator );

                    if ( ( c == '\r' ) && ( curIndex != ( len - 1 ) ) && ( comment[ curIndex + 1 ] == '\n' ) )
                    {
                        curIndex++;
                    }

                    if ( ( curIndex == ( len - 1 ) )
                         || ( ( comment[ curIndex + 1 ] != '#' ) && ( comment[ curIndex + 1 ] != '!' ) ) )
                    {
                        writer.Write( "#" );
                    }
                }

                lastIndex = curIndex + 1;
            }

            curIndex++;
        }

        if ( lastIndex != curIndex ) writer.Write( comment.Substring( lastIndex, curIndex ) );

        writer.Write( Line_Separator );
    }
}