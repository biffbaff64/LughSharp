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

/// <summary>
/// A helper class that allows you to load and store key/value pairs of an
/// <see cref="Dictionary{TK,TV}"/> with the same line-oriented syntax supported
/// by <see cref="IPreferences"/>
/// </summary>
[PublicAPI]
public static class PropertiesUtils
{
    private const int    NONE           = 0;
    private const int    SLASH          = 1;
    private const int    UNICODE        = 2;
    private const int    CONTINUE       = 3;
    private const int    KEY_DONE       = 4;
    private const int    IGNORE         = 5;
    private const string LINE_SEPARATOR = "\n";

    /// <summary>
    /// Loads properties from the specified <see cref="StreamReader"/> into the provided dictionary.
    /// </summary>
    /// <param name="properties">The dictionary to load properties into.</param>
    /// <param name="reader">The reader to read the properties from.</param>
    /// <exception cref="ArgumentNullException">Thrown if properties or reader is null.</exception>
    /// <exception cref="ArgumentException">Thrown if an invalid Unicode sequence is encountered.</exception>
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

        while ( true )
        {
            var intVal = reader.Read();

            if ( intVal == -1 ) break;

            var nextChar = ( char ) intVal;

            if ( offset == buf.Length )
            {
                var newBuf = new char[ buf.Length * 2 ];
                Array.Copy( buf, 0, newBuf, 0, offset );
                buf = newBuf;
            }

            switch ( mode )
            {
                case UNICODE:
                    var num   = char.GetNumericValue( nextChar.ToString(), 16 );
                    var digit = ( int ) num;

                    if ( digit >= 0 )
                    {
                        unicode = ( unicode << 4 ) + digit;

                        if ( ++count < 4 ) continue;
                    }
                    else if ( count <= 4 )
                    {
                        throw new ArgumentException( "Invalid Unicode sequence: illegal character" );
                    }

                    mode            = NONE;
                    buf[ offset++ ] = ( char ) unicode;

                    if ( nextChar != '\n' ) continue;

                    break;
            }

            switch ( mode )
            {
                case SLASH:
                    mode = NONE;

                    switch ( nextChar )
                    {
                        case '\r':
                            mode = CONTINUE;

                            continue;

                        case '\n':
                            mode = IGNORE;

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
                    switch ( nextChar )
                    {
                        case '#':
                        case '!':
                            if ( firstChar )
                            {
                                while ( true )
                                {
                                    intVal = reader.Read();

                                    if ( intVal == -1 ) break;

                                    nextChar = ( char ) intVal;

                                    if ( nextChar is '\r' or '\n' )
                                    {
                                        break;
                                    }
                                }

                                continue;
                            }

                            break;

                        case '\n':
                        case '\r':
                            if ( ( nextChar == '\n' ) && ( mode == CONTINUE ) )
                            {
                                mode = IGNORE;

                                continue;
                            }

                            mode      = NONE;
                            firstChar = true;

                            if ( ( offset > 0 ) || ( keyLength == 0 ) )
                            {
                                keyLength = keyLength == -1 ? offset : keyLength;
                                var temp = new string( buf, 0, offset );
                                properties[ temp.Substring( 0, keyLength ) ] = temp.Substring( keyLength );
                            }

                            keyLength = -1;
                            offset    = 0;

                            continue;

                        case '\\':
                            keyLength = mode == KEY_DONE ? offset : keyLength;
                            mode      = SLASH;

                            continue;

                        case ':':
                        case '=':
                            if ( keyLength == -1 )
                            {
                                mode      = NONE;
                                keyLength = offset;

                                continue;
                            }

                            break;
                    }

                    if ( char.IsWhiteSpace( nextChar ) )
                    {
                        mode = mode == CONTINUE ? IGNORE : mode;

                        if ( ( offset == 0 ) || ( offset == keyLength ) || ( mode == IGNORE ) ) continue;

                        if ( keyLength == -1 )
                        {
                            mode = KEY_DONE;

                            continue;
                        }
                    }

                    mode = ( mode is IGNORE or CONTINUE ) ? NONE : mode;

                    break;
            }

            firstChar = false;

            if ( mode == KEY_DONE )
            {
                keyLength = offset;
                mode      = NONE;
            }

            buf[ offset++ ] = nextChar;
        }

        if ( ( mode == UNICODE ) && ( count <= 4 ) )
        {
            throw new ArgumentException( "Invalid Unicode sequence: expected format \\uxxxx" );
        }

        keyLength = ( keyLength == -1 ) && ( offset > 0 ) ? offset : keyLength;

        if ( keyLength >= 0 )
        {
            var temp  = new string( buf, 0, offset );
            var key   = temp.Substring( 0, keyLength );
            var value = temp.Substring( keyLength );

            if ( mode == SLASH )
            {
                value += "\0";
            }

            properties[ key ] = value;
        }
    }

    /// <summary>
    /// Stores the properties from the dictionary to the specified <see cref="StreamWriter"/>.
    /// </summary>
    /// <param name="properties">The dictionary containing properties to store.</param>
    /// <param name="writer">The writer to write the properties to.</param>
    /// <param name="comment">An optional comment to include at the top of the output.</param>
    /// <param name="escapeUnicode">Whether to escape non-ASCII Unicode characters.</param>
    public static void Store( Dictionary< string, string > properties, StreamWriter writer, string? comment, bool escapeUnicode = false )
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

    /// <summary>
    /// Converts a string to a form suitable for writing to a properties file, escaping necessary characters.
    /// </summary>
    /// <param name="outBuffer">The buffer to write the escaped string to.</param>
    /// <param name="str">The string to escape.</param>
    /// <param name="escapeSpace">Whether to escape spaces.</param>
    /// <param name="escapeUnicode">Whether to escape non-ASCII Unicode characters.</param>
    private static void DumpString( StringBuilder outBuffer, string str, bool escapeSpace, bool escapeUnicode )
    {
        var len = str.Length;

        for ( var i = 0; i < len; i++ )
        {
            var ch = str[ i ];

            if ( ( ch > 61 ) && ( ch < 127 ) )
            {
                outBuffer.Append( ch == '\\' ? @"\\" : ch );

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

                case '=':
                case ':':
                case '#':
                case '!':
                    outBuffer.Append( '\\' ).Append( ch );

                    break;

                default:
                    if ( escapeUnicode && ( ( ch < 0x0020 ) || ( ch > 0x007e ) ) )
                    {
                        var hex = ( ( int ) ch ).ToString( "X" );
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

    /// <summary>
    /// Writes a comment to the specified <see cref="StreamWriter"/>.
    /// </summary>
    /// <param name="writer">The writer to write the comment to.</param>
    /// <param name="comment">The comment to write.</param>
    private static void WriteComment( StreamWriter writer, string comment )
    {
        writer.Write( "#" );

        var len       = comment.Length;
        var curIndex  = 0;
        var lastIndex = 0;

        while ( curIndex < len )
        {
            var c = comment[ curIndex ];

            if ( c is > '\u00ff' or '\n' or '\r' )
            {
                if ( lastIndex != curIndex )
                {
                    writer.Write( comment.Substring( lastIndex, curIndex ) );
                }

                if ( c > '\u00ff' )
                {
                    var hex = ( ( int ) c ).ToString( "X" );
                    writer.Write( "\\u" );

                    for ( var j = 0; j < ( 4 - hex.Length ); j++ )
                    {
                        writer.Write( '0' );
                    }

                    writer.Write( hex );
                }
                else
                {
                    writer.Write( LINE_SEPARATOR );

                    if ( ( c == '\r' ) && ( curIndex != ( len - 1 ) ) && ( comment[ curIndex + 1 ] == '\n' ) )
                    {
                        curIndex++;
                    }

                    if ( ( curIndex == ( len - 1 ) ) || ( ( comment[ curIndex + 1 ] != '#' ) && ( comment[ curIndex + 1 ] != '!' ) ) )
                    {
                        writer.Write( "#" );
                    }
                }

                lastIndex = curIndex + 1;
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
