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
/// The string tokenizer class allows an application to break a string into tokens.
/// The <c>StringTokenizer</c> methods do not distinguish among identifiers,
/// numbers, and quoted strings, nor do they recognize and skip comments.
/// <para>
/// The set of delimiters (the characters that separate tokens) may be specified
/// either at creation time or on a per-token basis.
/// </para>
/// <para>
/// An instance of <c>StringTokenizer</c> behaves in one of two ways, depending
/// on whether it was created with the <c>returnDelims</c> flag having the value
/// <c>true</c> or <c>false</c>:
/// <para></para>
/// If the flag is <c>false</c>, delimiter characters serve to separate tokens.
/// A token is a maximal sequence of consecutive characters that are not delimiters.
/// If the flag is <c>true</c>, delimiter characters are themselves considered to
/// be tokens. A token is thus either one delimiter character, or a maximal sequence
/// of consecutive characters that are not delimiters.
/// </para>
/// <para>
/// A <c>StringTokenizer</c> object internally maintains a current position within
/// the string to be tokenized. Some operations advance this current position past
/// the characters processed.
/// </para>
/// <para>
/// A token is returned by taking a substring of the string that was used to create
/// the <c>StringTokenizer</c> object.
/// </para>
/// <para>
/// The following is one example of the use of the tokenizer. The code:
/// <code>
///     StringTokenizer st = new StringTokenizer( "this is a test" );
///     while ( st.HasMoreTokens() )
///     {
///         Console.WriteLine( st.NextToken() );
///     }
/// </code>
/// </para>
/// <para>
/// prints the following output:
/// <code>
///     this
///     is
///     a
///     test
/// </code>
/// </para>
/// <para>
/// <c>StringTokenizer</c> is a legacy class that is retained for compatibility
/// reasons although its use is discouraged in new code. It is recommended that
/// anyone seeking this functionality use the <c>split</c> method of <c>String</c>
/// instead.
/// </para>
/// <para>
/// The following example illustrates how the <c>String.split</c> method can be
/// used to break up a string into its basic tokens:
/// <code>
///         string[] result = "this is a test".split("\\s");
///         for ( int x=0; x&lt;result.Length; x++ )
///         {
///             Console.WriteLine( result[ x ] );
///         }
///         </code>
/// </para>
/// <para>
/// prints the following output:
/// <code>
///         this
///         is
///         a
///         test
///         </code>
/// </para>
/// </summary>
[PublicAPI]
public class StringTokenizer
{
    private readonly int    _maxPosition = 0;
    private readonly bool   _retDelims   = false;
    private readonly string _str;

    private int _currentPosition;

    /// <summary>
    /// When hasSurrogates is true, delimiters are converted to code points and
    /// isDelimiter(int) is used to determine if the given codepoint is a delimiter.
    /// </summary>
    private int[]? _delimiterCodePoints;

    private string? _delimiters;
    private bool    _delimsChanged;

    /// <summary>
    /// If delimiters include any surrogates (including surrogate pairs),
    /// <c>_hasSurrogates</c> is true and the tokenizer uses the different code
    /// path. This is because <c>string.IndexOf(int)</c> doesn't handle unpaired
    /// surrogates as a single character.
    /// </summary>
    private bool _hasSurrogates = false;

    /// <summary>
    /// maxDelimCodePoint stores the value of the delimiter character with the
    /// highest value. It is used to optimize the detection of delimiter
    /// characters.
    /// <para>
    /// It is unlikely to provide any optimization benefit in the hasSurrogates
    /// case because most string characters will be smaller than the limit, but
    /// we keep it so that the two code paths remain similar.
    /// </para>
    /// </summary>
    private int _maxDelimCodePoint;

    private int _newPosition;

    /// <summary>
    /// Constructs a string tokenizer for the specified string. All characters
    /// in the <c>delim</c> argument are the delimitersfor separating tokens.
    /// <para>
    /// If the <c>returnDelims</c> flag is <c>true</c>, then the delimiter
    /// characters are also returned as tokens. Each delimiter is returned as
    /// a string of length one. If the flag is <c>false</c>, the delimiter
    /// characters are skipped and only serve as separators between tokens.
    /// </para>
    /// <para>
    /// Note that if <c>delim</c> is <c>null</c>, this constructor does
    /// not throw an exception. However, trying to invoke other methods on the
    /// resulting <c>StringTokenizer</c> may result in a <c>NullReferenceException</c>.
    /// </para>
    /// </summary>
    /// <param name="str">a string to be parsed.</param>
    /// <param name="delim">the delimiters.</param>
    /// <param name="returnDelims">
    /// flag indicating whether to return the delimiters as tokens.
    /// </param>
    public StringTokenizer( string str, string delim = " \t\n\r\f", bool returnDelims = false )
    {
        _currentPosition = 0;
        _newPosition     = -1;
        _delimsChanged   = false;
        _str             = str;
        _maxPosition     = str.Length;
        _delimiters      = delim;
        _retDelims       = returnDelims;

        SetMaxDelimCodePoint();
    }

    /// <summary>
    /// Set maxDelimCodePoint to the highest char in the delimiter set.
    /// </summary>
    private void SetMaxDelimCodePoint()
    {
        if ( _delimiters == null )
        {
            _maxDelimCodePoint = 0;

            return;
        }

        var m     = 0;
        var count = 0;
        int c;

        for ( var i = 0; i < _delimiters.Length; i += char.IsSurrogatePair( _delimiters, i ) ? 2 : 1 )
        {
            c = _delimiters[ i ];

            if ( c is >= Character.MIN_HIGH_SURROGATE and <= Character.MAX_LOW_SURROGATE )
            {
                c = char.ConvertToUtf32( _delimiters, i );

                _hasSurrogates = true;
            }

            if ( m < c )
            {
                m = c;
            }

            count++;
        }

        _maxDelimCodePoint = m;

        if ( _hasSurrogates )
        {
            _delimiterCodePoints = new int[ count ];

            for ( int i = 0, j = 0; i < count; i++, j += char.IsSurrogatePair( _delimiters, i ) ? 2 : 1 )
            {
                c = char.ConvertToUtf32( _delimiters, j );

                _delimiterCodePoints[ i ] = c;
            }
        }
    }

    /// <summary>
    /// Skips delimiters starting from the specified position. If retDelims
    /// is false, returns the index of the first non-delimiter character at or
    /// after startPos. If retDelims is true, startPos is returned.
    /// </summary>
    private int SkipDelimiters( int startPos )
    {
        if ( _delimiters == null )
        {
            throw new NullReferenceException();
        }

        var position = startPos;

        while ( !_retDelims && ( position < _maxPosition ) )
        {
            if ( !_hasSurrogates )
            {
                var c = _str[ position ];

                if ( ( c > _maxDelimCodePoint ) || ( _delimiters.IndexOf( c ) < 0 ) )
                {
                    break;
                }

                position++;
            }
            else
            {
                var c = char.ConvertToUtf32( _str, position );

                if ( ( c > _maxDelimCodePoint ) || !IsDelimiter( c ) )
                {
                    break;
                }

                position += Character.CharCount( c );
            }
        }

        return position;
    }

    /// <summary>
    /// Skips ahead from startPos and returns the index of the next delimiter
    /// character encountered, or maxPosition if no such delimiter is found.
    /// </summary>
    private int ScanToken( int startPos )
    {
        var position = startPos;

        while ( position < _maxPosition )
        {
            if ( !_hasSurrogates )
            {
                var c = _str[ position ];

                if ( ( c <= _maxDelimCodePoint ) && ( _delimiters?.IndexOf( c ) >= 0 ) )
                {
                    break;
                }

                position++;
            }
            else
            {
                var c = char.ConvertToUtf32( _str, position );

                if ( ( c <= _maxDelimCodePoint ) && IsDelimiter( c ) )
                {
                    break;
                }

                position += Character.CharCount( c );
            }
        }

        if ( _retDelims && ( startPos == position ) )
        {
            if ( !_hasSurrogates )
            {
                var c = _str[ position ];

                if ( ( c <= _maxDelimCodePoint ) && ( _delimiters?.IndexOf( c ) >= 0 ) )
                {
                    position++;
                }
            }
            else
            {
                var c = char.ConvertToUtf32( _str, position );

                if ( ( c <= _maxDelimCodePoint ) && IsDelimiter( c ) )
                {
                    position += Character.CharCount( c );
                }
            }
        }

        return position;
    }

    /// <summary>
    /// Returns TRUE if the supplied codepoint is a delimiter.
    /// </summary>
    private bool IsDelimiter( int codePoint )
    {
        if ( _delimiterCodePoints == null )
        {
            return false;
        }

        foreach ( var t in _delimiterCodePoints )
        {
            if ( t == codePoint )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Tests if there are more tokens available from this tokenizer's string.
    /// If this method returns <c>true</c>, then a subsequent call to
    /// <c>nextToken</c> with no argument will successfully return a token.
    /// </summary>
    /// <returns>
    /// <c>true</c> if and only if there is at least one token in the string
    /// after the current position; <c>false</c> otherwise.
    /// </returns>
    public bool HasMoreTokens()
    {
        // Temporarily store this position and use it in the following
        // nextToken() method only if the delimiters haven't been changed in
        // that nextToken() invocation.

        _newPosition = SkipDelimiters( _currentPosition );

        return _newPosition < _maxPosition;
    }

    /// <summary>
    /// Returns the next token from this string tokenizer.
    /// </summary>
    /// <returns>the next token from this string tokenizer.</returns>
    public string NextToken()
    {
        // If next position already computed in hasMoreElements() and
        // delimiters have changed between the computation and this invocation,
        // then use the computed value.

        _currentPosition = ( _newPosition >= 0 ) && !_delimsChanged
                               ? _newPosition
                               : SkipDelimiters( _currentPosition );

        // Reset these anyway
        _delimsChanged = false;
        _newPosition   = -1;

        if ( _currentPosition >= _maxPosition )
        {
            throw new IndexOutOfRangeException( $"{_currentPosition} > {_maxPosition}" );
        }

        var start = _currentPosition;

        _currentPosition = ScanToken( _currentPosition );

        return _str.Substring( start, _currentPosition - start );
    }

    /// <summary>
    /// Returns the next token in this string tokenizer's string. First,
    /// the set of characters considered to be delimiters by this
    /// <c>StringTokenizer</c> object is changed to be the characters in
    /// the string <c>delim</c>. Then the next token in the string
    /// after the current position is returned. The current position is
    /// advanced beyond the recognized token.  The new delimiter set
    /// remains the default after this call.
    /// </summary>
    /// <param name="delim">the new delimiters.</param>
    /// <returns>the next token, after switching to the new delimiter set.</returns>
    public string NextToken( string delim )
    {
        _delimiters = delim;

        // delimiter string specified, so set the appropriate flag.
        _delimsChanged = true;

        SetMaxDelimCodePoint();

        return NextToken();
    }

    /// <summary>
    /// Calculates the number of times that this tokenizer's <c>NextToken</c>
    /// method can be called before it generates an exception. The current
    /// position is not advanced.
    /// </summary>
    /// <returns>
    /// the number of tokens remaining in the string using the current delimiter set.
    /// </returns>
    public int CountTokens()
    {
        var count   = 0;
        var currpos = _currentPosition;

        while ( currpos < _maxPosition )
        {
            currpos = SkipDelimiters( currpos );

            if ( currpos >= _maxPosition )
            {
                break;
            }

            currpos = ScanToken( currpos );
            count++;
        }

        return count;
    }
}
