// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Utils.Regex;

public partial class Pattern
{
    /// <summary>
    /// Indicates whether a particular flag is set or not.
    /// </summary>
    private bool Has( int f )
    {
        return ( Flags & f ) != 0;
    }

    /// <summary>
    /// Match next character, signal error if failed.
    /// </summary>
    private void Accept( int ch, string s )
    {
        var testChar = _temp[ _cursor++ ];

        if ( Has( Comments ) )
        {
            testChar = ParsePastWhitespace( testChar );
        }

        if ( ch != testChar )
        {
            throw error( s );
        }
    }

    /// <summary>
    /// Mark the end of pattern with a specific character.
    /// </summary>
    private void Mark( int c )
    {
        _temp[ _patternLength ] = c;
    }

    /// <summary>
    /// Peek the next character, and do not advance the _cursor.
    /// </summary>
    private int Peek()
    {
        var ch = _temp[ _cursor ];

        if ( Has( Comments ) )
        {
            ch = PeekPastWhitespace( ch );
        }

        return ch;
    }

    /// <summary>
    /// Read the next character, and advance the _cursor by one.
    /// </summary>
    private int Read()
    {
        var ch = _temp[ _cursor++ ];

        if ( Has( Comments ) )
        {
            ch = ParsePastWhitespace( ch );
        }

        return ch;
    }

    /// <summary>
    /// Read the next character, and advance the _cursor by one,
    /// ignoring the <see cref="Comments"/> setting
    /// </summary>
    private int ReadEscaped()
    {
        var ch = _temp[ _cursor++ ];

        return ch;
    }

    /// <summary>
    /// Advance the _cursor by one, and peek the next character.
    /// </summary>
    private int Next()
    {
        var ch = _temp[ ++_cursor ];

        if ( Has( Comments ) )
        {
            ch = PeekPastWhitespace( ch );
        }

        return ch;
    }

    /// <summary>
    /// Advance the _cursor by one, and peek the next character,
    /// ignoring the <see cref="Comments"/> setting
    /// </summary>
    private int NextEscaped()
    {
        var ch = _temp[ ++_cursor ];

        return ch;
    }

    /// <summary>
    /// If in xmode peek past whitespace and comments.
    /// </summary>
    private int PeekPastWhitespace( int ch )
    {
        while ( ASCII.IsSpace( ch ) || ( ch == '#' ) )
        {
            while ( ASCII.IsSpace( ch ) )
            {
                ch = _temp[ ++_cursor ];
            }

            if ( ch == '#' )
            {
                ch = PeekPastLine();
            }
        }

        return ch;
    }

    /// <summary>
    /// If in xmode parse past whitespace and comments.
    /// </summary>
    private int ParsePastWhitespace( int ch )
    {
        while ( ASCII.IsSpace( ch ) || ( ch == '#' ) )
        {
            while ( ASCII.IsSpace( ch ) )
            {
                ch = _temp[ _cursor++ ];
            }

            if ( ch == '#' )
            {
                ch = ParsePastLine();
            }
        }

        return ch;
    }

    /// <summary>
    /// xmode parse past comment to end of line.
    /// </summary>
    private int ParsePastLine()
    {
        var ch = _temp[ _cursor++ ];

        while ( ( ch != 0 ) && !IsLineSeparator( ch ) )
        {
            ch = _temp[ _cursor++ ];
        }

        if ( ( ch == 0 ) && ( _cursor > _patternLength ) )
        {
            _cursor = _patternLength;
            ch     = _temp[ _cursor++ ];
        }

        return ch;
    }

    /// <summary>
    /// xmode peek past comment to end of line.
    /// </summary>
    private int PeekPastLine()
    {
        var ch = _temp[ ++_cursor ];

        while ( ( ch != 0 ) && !IsLineSeparator( ch ) )
        {
            ch = _temp[ ++_cursor ];
        }

        if ( ( ch == 0 ) && ( _cursor > _patternLength ) )
        {
            _cursor = _patternLength;
            ch     = _temp[ _cursor ];
        }

        return ch;
    }

    /// <summary>
    /// Determines if character is a line separator in the current mode
    /// </summary>
    private bool IsLineSeparator( int ch )
    {
        if ( Has( Unix_Lines ) )
        {
            return ch == '\n';
        }

        return ( ( ch == '\n' ) || ( ch == '\r' ) || ( ( ch | 1 ) == '\u2029' ) || ( ch == '\u0085' ) );
    }

    /// <summary>
    /// Read the character after the next one, and advance the _cursor by two.
    /// </summary>
    private int Skip()
    {
        var i  = _cursor;
        var ch = _temp[ i + 1 ];

        _cursor = i + 2;

        return ch;
    }

    /// <summary>
    /// Unread one next character, and retreat _cursor by one.
    /// </summary>
    private void Unread()
    {
        _cursor--;
    }
}