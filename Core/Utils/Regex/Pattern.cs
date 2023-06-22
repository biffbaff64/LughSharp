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

using System.Diagnostics.CodeAnalysis;
using System.Text;

using LibGDXSharp.Utils.Regex;

namespace LibGDXSharp.Utils.Regex;

/// <inheritdoc/>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed partial class Pattern : IPattern
{
    public const int Unix_Lines              = 0x01;
    public const int Case_Insensitive        = 0x02;
    public const int Comments                = 0x04;
    public const int Multiline               = 0x08;
    public const int Literal                 = 0x10;
    public const int Dotall                  = 0x20;
    public const int Unicode_Case            = 0x40;
    public const int Canon_Eq                = 0x80;
    public const int Unicode_Character_Class = 0x100;

    public const int Max_Reps    = 0x7fffffff;
    public const int Greedy      = 0;
    public const int Lazy        = 1;
    public const int Possessive  = 2;
    public const int Independant = 3;

    [NonSerialized] private volatile bool                      _compiled = false;
    [NonSerialized] private volatile Dictionary< string, int > _namedGroups;

    [NonSerialized] private string      _normalizedPattern;
    [NonSerialized] private int[]       _buffer;
    [NonSerialized] private GroupHead[] _groupNodes;
    [NonSerialized] private int[]       _temp;
    [NonSerialized] private int         _cursor;
    [NonSerialized] private int         _patternLength;
    [NonSerialized] private bool        _hasSupplementary;
    [NonSerialized] private string?     _pattern;

    public int CapturingGroupCount { get; set; }
    public int LocalCount          { get; set; }
    public int Flags               { get; set; }

    [NonSerialized] public Node root      = null!;
    [NonSerialized] public Node matchRoot = null!;

    static Node _accept     = new();
    static Node _lastAccept = new LastNode();

    public Pattern( string regex, int flags )
    {
        this._pattern = regex;
        this.Flags    = flags;

        // to use Unicode_Case if Unicode_Character_Class present
        if ( ( flags & Unicode_Character_Class ) != 0 )
        {
            this.Flags |= Unicode_Case;
        }

        // Reset group index count
        CapturingGroupCount = 1;
        LocalCount          = 0;

        if ( _pattern.Length > 0 )
        {
            try
            {
                Compile();
            }
            catch( StackOverflowException soe )
            {
                throw error( "Stack overflow during pattern compilation" );
            }
        }
        else
        {
            root      = new Start( _lastAccept );
            matchRoot = _lastAccept;
        }
    }

    public Pattern Compile( string regex )
    {
        return new Pattern( regex, 0 );
    }

    public Pattern Compile( string regex, int flags )
    {
        return new Pattern( regex, flags );
    }

    public string? GetPattern() => _pattern;

    public new string ToString()
    {
        return _pattern ?? "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Matcher Matcher( string input )
    {
        lock ( this )
        {
            if ( !_compiled )
            {
                Compile();
            }
        }

        var m = new Matcher( this, input );

        return m;
    }

    public bool Matches( string regex, string input )
    {
        Pattern p = Compile( regex );
        Matcher m = p.Matcher( input );

        return m.Matches();
    }

    public string[] Split( string input, int limit )
    {
        var     index        = 0;
        var     matchLimited = limit > 0;
        var     matchList    = new List< string >();
        Matcher m            = Matcher( input );

        // Add segments before each match found
        while ( m.Find() )
        {
            if ( !matchLimited || ( matchList.Count < ( limit - 1 ) ) )
            {
                if ( ( index == 0 ) && ( index == m.Start() ) && ( m.Start() == m.End() ) )
                {
                    // no empty leading Substring included for zero-width match
                    // at the beginning of the input char sequence.
                    continue;
                }

                var match = input.Substring( index, m.Start() - index ).ToString();
                matchList.Add( match );
                index = m.End();
            }
            else if ( matchList.Count == ( limit - 1 ) )
            {
                // last one
                var match = input.Substring( index, input.Length - index ).ToString();
                matchList.Add( match );
                index = m.End();
            }
        }

        // If no match was found, return this
        if ( index == 0 )
        {
            return new[] { input };
        }

        // Add remaining segment
        if ( !matchLimited || ( matchList.Count < limit ) )
        {
            matchList.Add( input.Substring( index, input.Length - index ).ToString() );
        }

        // Construct result
        var resultSize = matchList.Count;

        if ( limit == 0 )
        {
            while ( ( resultSize > 0 ) && matchList[ resultSize - 1 ].Equals( "" ) )
            {
                resultSize--;
            }
        }

        var result = new string[ resultSize ];

        return matchList.subList( 0, resultSize ).toArray( result );
    }

    public string[] Split( string input )
    {
        return Split( input, 0 );
    }

    public string Quote( string s )
    {
        var slashEIndex = s.IndexOf( "\\E", StringComparison.Ordinal );

        if ( slashEIndex == -1 ) return $"\\Q{s}\\E";

        var sb = new StringBuilder( s.Length * 2 );
        sb.Append( "\\Q" );

        var current = 0;

        while ( ( slashEIndex = s.IndexOf( "\\E", current, StringComparison.Ordinal ) ) != -1 )
        {
            sb.Append( s.Substring( current, slashEIndex ) );
            current = slashEIndex + 2;
            sb.Append( "\\E\\\\E\\Q" );
        }

        sb.Append( s.Substring( current, s.Length ) );
        sb.Append( "\\E" );

        return sb.ToString();
    }

    private void ReadObject( StreamReader s )
    {
        // Read in all fields
        s.DefaultReadObject();

        // Initialize counts
        CapturingGroupCount = 1;
        LocalCount          = 0;

        // if length > 0, the Pattern is lazily compiled
        lock ( this )
        {
            _compiled = false;
        }

        if ( _pattern?.Length == 0 )
        {
            root      = new Start( lastAccept );
            matchRoot = lastAccept;
            _compiled = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Normalize()
    {
        var inCharClass   = false;
        var lastCodePoint = -1;

        // Convert pattern into normalizedD form
        _normalizedPattern = Normalizer.normalize( _pattern, Normalizer.Form.NFD );
        _patternLength     = _normalizedPattern.Length;

        // Modify pattern to match canonical equivalences
        var newPattern = new StringBuilder( _patternLength );

        for ( var i = 0; i < _patternLength; )
        {
            int c = _normalizedPattern.CodePointAt( i );

            if ( ( CharHelper.GetCharCat( ( char )c ) == CharHelper.NonSpacingMark )
                 && ( lastCodePoint != -1 ) )
            {
                var sequenceBuffer = new StringBuilder();

                sequenceBuffer.AppendCodePoint( lastCodePoint );
                sequenceBuffer.AppendCodePoint( c );

                while ( CharHelper.GetCharCat( ( char )c ) == CharHelper.NonSpacingMark )
                {
                    i += CharHelper.CharCount( c );

                    if ( i >= _patternLength ) break;

                    c = CharHelper.CodePointAt( _normalizedPattern, i );

                    sequenceBuffer.AppendCodePoint( c );
                }

                var ea = ProduceEquivalentAlternation( sequenceBuffer.ToString() );

                newPattern.Length -= CharHelper.CharCount( lastCodePoint );
                newPattern.Append( "(?:" ).Append( ea ).Append( ")" );
            }
            else if ( ( c == '[' ) && ( lastCodePoint != '\\' ) )
            {
                i = NormalizeCharClass( newPattern, i );
            }
            else
            {
                newPattern.AppendCodePoint( c );
            }

            lastCodePoint = c;

            i += CharHelper.CharCount( c );
        }

        _normalizedPattern = newPattern.ToString();
    }

    private int NormalizeCharClass( StringBuilder newPattern, int i )
    {
        StringBuilder charClass     = new();
        StringBuilder eq            = null;
        var           lastCodePoint = -1;
        string        result;

        i++;

        if ( i == _normalizedPattern.Length ) throw Error( "Unclosed character class" );

        charClass.Append( "[" );

        while ( true )
        {
            var c = CharHelper.CodePointAt( _normalizedPattern, i );

            if ( ( c == ']' ) && ( lastCodePoint != '\\' ) )
            {
                charClass.Append( ( char )c );

                break;
            }
            else if ( CharHelper.GetCharCat( c ) == CharHelper.NonSpacingMark )
            {
                var sequenceBuffer = new StringBuilder();
                sequenceBuffer.AppendCodePoint( lastCodePoint );

                while ( CharHelper.GetCharCat( c ) == CharHelper.NonSpacingMark )
                {
                    sequenceBuffer.AppendCodePoint( c );
                    i += CharHelper.CharCount( c );

                    if ( i >= _normalizedPattern.Length ) break;

                    c = CharHelper.CodePointAt( _normalizedPattern, i );
                }

                var ea = ProduceEquivalentAlternation( sequenceBuffer.ToString() );

                charClass.Length -= CharHelper.CharCount( lastCodePoint );

                eq ??= new StringBuilder();

                eq.Append( '|' );
                eq.Append( ea );
            }
            else
            {
                charClass.AppendCodePoint( c );
                i++;
            }

            if ( i == _normalizedPattern.Length )
                throw Error( "Unclosed character class" );

            lastCodePoint = c;
        }

        if ( eq != null )
        {
            result = $"(?:{charClass}{eq})";
        }
        else
        {
            result = charClass.ToString();
        }

        newPattern.Append( result );

        return i;
    }

    private string ProduceEquivalentAlternation( string source )
    {
        int len = CountChars( source, 0, 1 );

        if ( source.Length == len ) return source; // source has one character.

        var baseStr        = source.Substring( 0, len );
        var combiningMarks = source.Substring( len );

        string[]      perms  = ProducePermutations( combiningMarks );
        StringBuilder result = new( source );

        // Add combined permutations
        for ( var x = 0; x < perms.Length; x++ )
        {
            var next = baseStr + perms[ x ];

            if ( x > 0 ) result.Append( "|" + next );

            next = ComposeOneStep( next );

            if ( next != null )
            {
                result.Append( "|" + ProduceEquivalentAlternation( next ) );
            }
        }

        return result.ToString();
    }

    private string[] ProducePermutations( string input )
    {
        if ( input.Length == CountChars( input, 0, 1 ) )
            return new string[] { input };

        if ( input.Length == CountChars( input, 0, 2 ) )
        {
            int c0 = CharHelper.codePointAt( input, 0 );
            int c1 = CharHelper.codePointAt( input, CharHelper.charCount( c0 ) );

            if ( getClass( c1 ) == getClass( c0 ) )
            {
                return new string[] { input };
            }

            string[] result = new string[ 2 ];
            result[ 0 ] = input;
            StringBuilder sb = new StringBuilder( 2 );
            sb.appendCodePoint( c1 );
            sb.appendCodePoint( c0 );
            result[ 1 ] = sb.toString();

            return result;
        }

        int length      = 1;
        int nCodePoints = countCodePoints( input );

        for ( int x = 1; x < nCodePoints; x++ )
            length = length * ( x + 1 );

        string[] temp = new string[ length ];

        int combClass[] = new int[ nCodePoints ];

        for ( int x = 0, i = 0; x < nCodePoints; x++ )
        {
            int c = CharHelper.codePointAt( input, i );
            combClass[ x ] =  getClass( c );
            i              += CharHelper.charCount( c );
        }

        // For each char, take it out and add the permutations
        // of the remaining chars
        int index = 0;
        int len;

        // offset maintains the index in code units.
        loop:

        for ( int x = 0, offset = 0; x < nCodePoints; x++, offset += len )
        {
            len = CountChars( input, offset, 1 );
            bool skip = false;

            for ( int y = x - 1; y >= 0; y-- )
            {
                if ( combClass[ y ] == combClass[ x ] )
                {
                    continue loop;
                }
            }

            StringBuilder sb         = new StringBuilder( input );
            string        otherChars = sb.Delete( offset, offset + len ).toString();
            string[]      subResult  = ProducePermutations( otherChars );

            string prefix = input.Substring( offset, offset + len );

            for ( int y = 0; y < subResult.Length; y++ )
                temp[ index++ ] = prefix + subResult[ y ];
        }

        string[] result = new string[ index ];

        for ( int x = 0; x < index; x++ )
            result[ x ] = temp[ x ];

        return result;
    }

    private string ComposeOneStep( string input )
    {
        System.Text.StringBuilder
        int    len                = CountChars( input, 0, 2 );
        string firstTwoCharacters = input.Substring( 0, len );
        string result             = Normalizer.normalize( firstTwoCharacters, Normalizer.Form.NFC );

        if ( result.Equals( firstTwoCharacters ) )
        {
            return null;
        }
        else
        {
            string remainder = input.Substring( len );

            return result + remainder;
        }
    }

    /// <summary>
    /// Preprocess any \Q...\E sequences in `temp', meta-quoting them.
    /// See the description of `quotemeta' in perlfunc(1).
    /// </summary>
    private void RemoveQEQuoting()
    {
        int pLen = patternLength;
        int i    = 0;

        while ( i < pLen - 1 )
        {
            if ( temp[ i ] != '\\' )
                i += 1;
            else if ( temp[ i + 1 ] != 'Q' )
                i += 2;
            else
                break;
        }

        if ( i >= pLen - 1 ) // No \Q sequence found
            return;

        int j = i;
        i += 2;
        int[] newtemp = new int[ j + 3 * ( pLen - i ) + 2 ];
        System.arraycopy( temp, 0, newtemp, 0, j );

        bool inQuote    = true;
        bool beginQuote = true;

        while ( i < pLen )
        {
            int c = temp[ i++ ];

            if ( !ASCII.isAscii( c ) || ASCII.isAlpha( c ) )
            {
                newtemp[ j++ ] = c;
            }
            else if ( ASCII.isDigit( c ) )
            {
                if ( beginQuote )
                {
                    /*
                     * A unicode escape \[0xu] could be before this quote,
                     * and we don't want this numeric char to processed as
                     * part of the escape.
                     */
                    newtemp[ j++ ] = '\\';
                    newtemp[ j++ ] = 'x';
                    newtemp[ j++ ] = '3';
                }

                newtemp[ j++ ] = c;
            }
            else if ( c != '\\' )
            {
                if ( inQuote ) newtemp[ j++ ] = '\\';
                newtemp[ j++ ] = c;
            }
            else if ( inQuote )
            {
                if ( temp[ i ] == 'E' )
                {
                    i++;
                    inQuote = false;
                }
                else
                {
                    newtemp[ j++ ] = '\\';
                    newtemp[ j++ ] = '\\';
                }
            }
            else
            {
                if ( temp[ i ] == 'Q' )
                {
                    i++;
                    inQuote    = true;
                    beginQuote = true;

                    continue;
                }
                else
                {
                    newtemp[ j++ ] = c;

                    if ( i != pLen )
                        newtemp[ j++ ] = temp[ i++ ];
                }
            }

            beginQuote = false;
        }

        patternLength = j;
        temp          = Arrays.copyOf( newtemp, j + 2 ); // double zero termination
    }

    public void Compile()
    {
    }

    public Dictionary< string, int > NamedGroups()
    {
    }

    /**
     * Used to print out a subtree of the Pattern to help with debugging.
     */
    private static void printObjectTree( Node node )
    {
        while ( node != null )
        {
            if ( node instanceof Prolog) {
                System.out.println( node );
                printObjectTree( ( ( Prolog )node ).loop );
                System.out.println( "**** end contents prolog loop" );
            } else if ( node instanceof Loop) {
                System.out.println( node );
                printObjectTree( ( ( Loop )node ).body );
                System.out.println( "**** end contents Loop body" );
            } else if ( node instanceof Curly) {
                System.out.println( node );
                printObjectTree( ( ( Curly )node ).atom );
                System.out.println( "**** end contents Curly body" );
            } else if ( node instanceof GroupCurly) {
                System.out.println( node );
                printObjectTree( ( ( GroupCurly )node ).atom );
                System.out.println( "**** end contents GroupCurly body" );
            } else if ( node instanceof GroupTail) {
                System.out.println( node );
                System.out.println( "Tail next is " + node.next );

                return;
            } else {
                System.out.println( node );
            }

            node = node.next;

            if ( node != null )
                System.out.

            println( "->next:" );

            if ( node == Pattern.accept )
            {
                System.out.println( "Accept Node" );
                node = null;
            }
        }
    }

    /**
     * Used to accumulate information about a subtree of the object graph
     * so that optimizations can be applied to the subtree.
     */
    static class TreeInfo
    {
        int  minLength;
        int  maxLength;
        bool maxValid;
        bool deterministic;

        TreeInfo()
        {
            reset();
        }

        void reset()
        {
            minLength     = 0;
            maxLength     = 0;
            maxValid      = true;
            deterministic = true;
        }
    }

    /**
     * Internal method used for handling all syntax errors. The pattern is
     * displayed with a pointer to aid in locating the syntax error.
     */
    private PatternSyntaxException error( String s )
    {
        return new PatternSyntaxException( s, normalizedPattern, cursor - 1 );
    }

    /**
     * Determines if there is any supplementary character or unpaired
     * surrogate in the specified range.
     */
    private bool findSupplementary( int start, int end )
    {
        for ( int i = start; i < end; i++ )
        {
            if ( isSupplementary( temp[ i ] ) )
                return true;
        }

        return false;
    }

    /**
     * Determines if the specified code point is a supplementary
     * character or unpaired surrogate.
     */
    private static bool isSupplementary( int ch )
    {
        return ch >= Character.MIN_SUPPLEMENTARY_CODE_POINT || Character.isSurrogate( ( char )ch );
    }

    /**
     *  The following methods handle the main parsing. They are sorted
     *  according to their precedence order, the lowest one first.
     */
    /**
     * The expression is parsed with branch nodes added for alternations.
     * This may be called recursively to parse sub expressions that may
     * contain alternations.
     */
    private Node expr( Node end )
    {
        Node   prev       = null;
        Node   firstTail  = null;
        Branch branch     = null;
        Node   branchConn = null;

        for ( ;; )
        {
            Node node     = sequence( end );
            Node nodeTail = root; //double return

            if ( prev == null )
            {
                prev      = node;
                firstTail = nodeTail;
            }
            else
            {
                // Branch
                if ( branchConn == null )
                {
                    branchConn      = new BranchConn();
                    branchConn.next = end;
                }

                if ( node == end )
                {
                    // if the node returned from sequence() is "end"
                    // we have an empty expr, set a null atom into
                    // the branch to indicate to go "next" directly.
                    node = null;
                }
                else
                {
                    // the "tail.next" of each atom goes to branchConn
                    nodeTail.next = branchConn;
                }

                if ( prev == branch )
                {
                    branch.add( node );
                }
                else
                {
                    if ( prev == end )
                    {
                        prev = null;
                    }
                    else
                    {
                        // replace the "end" with "branchConn" at its tail.next
                        // when put the "prev" into the branch as the first atom.
                        firstTail.next = branchConn;
                    }

                    prev = branch = new Branch( prev, node, branchConn );
                }
            }

            if ( peek() != '|' )
            {
                return prev;
            }

            next();
        }
    }

    @SuppressWarnings( "fallthrough" )

    /**
     * Parsing of sequences between alternations.
     */
    private Node sequence( Node end )
    {
        Node head = null;
        Node tail = null;
        Node node = null;
        LOOP:

        for ( ;; )
        {
            int ch = peek();

            switch ( ch )
            {
                case '(':
                    // Because group handles its own closure,
                    // we need to treat it differently
                    node = group0();

                    // Check for comment or flag group
                    if ( node == null )
                        continue;

                    if ( head == null )
                        head = node;
                    else
                        tail.next = node;

                    // Double return: Tail was returned in root
                    tail = root;

                    continue;

                case '[':
                    node = clazz( true );

                    break;

                case '\\':
                    ch = nextEscaped();

                    if ( ch == 'p' || ch == 'P' )
                    {
                        bool oneLetter = true;
                        bool comp      = ( ch == 'P' );
                        ch = next(); // Consume { if present

                        if ( ch != '{' )
                        {
                            unread();
                        }
                        else
                        {
                            oneLetter = false;
                        }

                        node = family( oneLetter, comp );
                    }
                    else
                    {
                        unread();
                        node = atom();
                    }

                    break;

                case '^':
                    next();

                    if ( has( MULTILINE ) )
                    {
                        if ( has( UNIX_LINES ) )
                            node = new UnixCaret();
                        else
                            node = new Caret();
                    }
                    else
                    {
                        node = new Begin();
                    }

                    break;

                case '$':
                    next();

                    if ( has( UNIX_LINES ) )
                        node = new UnixDollar( has( MULTILINE ) );
                    else
                        node = new Dollar( has( MULTILINE ) );

                    break;

                case '.':
                    next();

                    if ( has( DOTALL ) )
                    {
                        node = new All();
                    }
                    else
                    {
                        if ( has( UNIX_LINES ) )
                            node = new UnixDot();
                        else
                        {
                            node = new Dot();
                        }
                    }

                    break;

                case '|':
                case ')':
                    break LOOP;

                case ']': // Now interpreting dangling ] and } as literals
                case '}':
                    node = atom();

                    break;

                case '?':
                case '*':
                case '+':
                    next();

                    throw error( "Dangling meta character '" + ( ( char )ch ) + "'" );

                case 0:
                    if ( cursor >= patternLength )
                    {
                        break LOOP;
                    }

                // Fall through
                default:
                    node = atom();

                    break;
            }

            node = closure( node );

            if ( head == null )
            {
                head = tail = node;
            }
            else
            {
                tail.next = node;
                tail      = node;
            }
        }

        if ( head == null )
        {
            return end;
        }

        tail.next = end;
        root      = tail; //double return

        return head;
    }

    @SuppressWarnings( "fallthrough" )

    /**
     * Parse and add a new Single or Slice.
     */
    private Node atom()
    {
        int  first            = 0;
        int  prev             = -1;
        bool hasSupplementary = false;
        int  ch               = peek();

        for ( ;; )
        {
            switch ( ch )
            {
                case '*':
                case '+':
                case '?':
                case '{':
                    if ( first > 1 )
                    {
                        cursor = prev; // Unwind one character
                        first--;
                    }

                    break;

                case '$':
                case '.':
                case '^':
                case '(':
                case '[':
                case '|':
                case ')':
                    break;

                case '\\':
                    ch = nextEscaped();

                    if ( ch == 'p' || ch == 'P' )
                    {
                        // Property
                        if ( first > 0 )
                        {
                            // Slice is waiting; handle it first
                            unread();

                            break;
                        }
                        else
                        {
                            // No slice; just return the family node
                            bool comp      = ( ch == 'P' );
                            bool oneLetter = true;
                            ch = next(); // Consume { if present

                            if ( ch != '{' )
                                unread();
                            else
                                oneLetter = false;

                            return family( oneLetter, comp );
                        }
                    }

                    unread();
                    prev = cursor;
                    ch   = escape( false, first == 0, false );

                    if ( ch >= 0 )
                    {
                        append( ch, first );
                        first++;

                        if ( isSupplementary( ch ) )
                        {
                            hasSupplementary = true;
                        }

                        ch = peek();

                        continue;
                    }
                    else if ( first == 0 )
                    {
                        return root;
                    }

                    // Unwind meta escape sequence
                    cursor = prev;

                    break;

                case 0:
                    if ( cursor >= patternLength )
                    {
                        break;
                    }

                // Fall through
                default:
                    prev = cursor;
                    append( ch, first );
                    first++;

                    if ( isSupplementary( ch ) )
                    {
                        hasSupplementary = true;
                    }

                    ch = next();

                    continue;
            }

            break;
        }

        if ( first == 1 )
        {
            return newSingle( buffer[ 0 ] );
        }
        else
        {
            return newSlice( buffer, first, hasSupplementary );
        }
    }

    private void append( int ch, int len )
    {
        if ( len >= buffer.length )
        {
            int[] tmp = new int[ len + len ];
            System.arraycopy( buffer, 0, tmp, 0, len );
            buffer = tmp;
        }

        buffer[ len ] = ch;
    }

    /**
     * Parses a backref greedily, taking as many numbers as it
     * can. The first digit is always treated as a backref, but
     * multi digit numbers are only treated as a backref if at
     * least that many backrefs exist at this point in the regex.
     */
    private Node ref (int refNum) {
        bool done = false;

        while ( !done )
        {
            int ch = peek();

            switch ( ch )
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    int newRefNum = ( refNum * 10 ) + ( ch - '0' );

                    // Add another number if it doesn't make a group
                    // that doesn't exist
                    if ( capturingGroupCount - 1 < newRefNum )
                    {
                        done = true;

                        break;
                    }

                    refNum = newRefNum;
                    read();

                    break;

                default:
                    done = true;

                    break;
            }
        }

        if ( has( CASE_INSENSITIVE ) )
            return new CIBackRef( refNum, has( UNICODE_CASE ) );
        else
            return new BackRef( refNum );
    }

    /**
     * Parses an escape sequence to determine the actual value that needs
     * to be matched.
     * If -1 is returned and create was true a new object was added to the tree
     * to handle the escape sequence.
     * If the returned value is greater than zero, it is the value that
     * matches the escape sequence.
     */
    private int escape( bool inclass, bool create, bool isrange )
    {
        int ch = skip();

        switch ( ch )
        {
            case '0':
                return o();

            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                if ( inclass ) break;

                if ( create )
                {
                    root = ref ( ( ch - '0' ) );
                }

                return -1;

            case 'A':
                if ( inclass ) break;
                if ( create ) root = new Begin();

                return -1;

            case 'B':
                if ( inclass ) break;
                if ( create ) root = new Bound( Bound.NONE, has( UNICODE_CHARACTER_CLASS ) );

                return -1;

            case 'C':
                break;

            case 'D':
                if ( create )
                    root = has( UNICODE_CHARACTER_CLASS )
                        ? new Utype( UnicodeProp.DIGIT ).complement()
                        : new Ctype( ASCII.DIGIT ).complement();

                return -1;

            case 'E':
            case 'F':
                break;

            case 'G':
                if ( inclass ) break;
                if ( create ) root = new LastMatch();

                return -1;

            case 'H':
                if ( create ) root = new HorizWS().complement();

                return -1;

            case 'I':
            case 'J':
            case 'K':
            case 'L':
            case 'M':
            case 'N':
            case 'O':
            case 'P':
            case 'Q':
                break;

            case 'R':
                if ( inclass ) break;
                if ( create ) root = new LineEnding();

                return -1;

            case 'S':
                if ( create )
                    root = has( UNICODE_CHARACTER_CLASS )
                        ? new Utype( UnicodeProp.WHITE_SPACE ).complement()
                        : new Ctype( ASCII.SPACE ).complement();

                return -1;

            case 'T':
            case 'U':
                break;

            case 'V':
                if ( create ) root = new VertWS().complement();

                return -1;

            case 'W':
                if ( create )
                    root = has( UNICODE_CHARACTER_CLASS )
                        ? new Utype( UnicodeProp.WORD ).complement()
                        : new Ctype( ASCII.WORD ).complement();

                return -1;

            case 'X':
            case 'Y':
                break;

            case 'Z':
                if ( inclass ) break;

                if ( create )
                {
                    if ( has( UNIX_LINES ) )
                        root = new UnixDollar( false );
                    else
                        root = new Dollar( false );
                }

                return -1;

            case 'a':
                return '\007';

            case 'b':
                if ( inclass ) break;
                if ( create ) root = new Bound( Bound.BOTH, has( UNICODE_CHARACTER_CLASS ) );

                return -1;

            case 'c':
                return c();

            case 'd':
                if ( create )
                    root = has( UNICODE_CHARACTER_CLASS )
                        ? new Utype( UnicodeProp.DIGIT )
                        : new Ctype( ASCII.DIGIT );

                return -1;

            case 'e':
                return '\033';

            case 'f':
                return '\f';

            case 'g':
                break;

            case 'h':
                if ( create ) root = new HorizWS();

                return -1;

            case 'i':
            case 'j':
                break;

            case 'k':
                if ( inclass )
                    break;

                if ( read() != '<' )
                    throw error( "\\k is not followed by '<' for named capturing group" );

                String name = groupname( read() );

                if ( !namedGroups().containsKey( name ) )
                    throw error( "(named capturing group <" + name + "> does not exit" );

                if ( create )
                {
                    if ( has( CASE_INSENSITIVE ) )
                        root = new CIBackRef( namedGroups().get( name ), has( UNICODE_CASE ) );
                    else
                        root = new BackRef( namedGroups().get( name ) );
                }

                return -1;

            case 'l':
            case 'm':
                break;

            case 'n':
                return '\n';

            case 'o':
            case 'p':
            case 'q':
                break;

            case 'r':
                return '\r';

            case 's':
                if ( create )
                    root = has( UNICODE_CHARACTER_CLASS )
                        ? new Utype( UnicodeProp.WHITE_SPACE )
                        : new Ctype( ASCII.SPACE );

                return -1;

            case 't':
                return '\t';

            case 'u':
                return u();

            case 'v':
                // '\v' was implemented as VT/0x0B in releases < 1.8 (though
                // undocumented). In JDK8 '\v' is specified as a predefined
                // character class for all vertical whitespace characters.
                // So [-1, root=VertWS node] pair is returned (instead of a
                // single 0x0B). This breaks the range if '\v' is used as
                // the start or end value, such as [\v-...] or [...-\v], in
                // which a single definite value (0x0B) is expected. For
                // compatibility concern '\013'/0x0B is returned if isrange.
                if ( isrange )
                    return '\013';

                if ( create ) root = new VertWS();

                return -1;

            case 'w':
                if ( create )
                    root = has( UNICODE_CHARACTER_CLASS )
                        ? new Utype( UnicodeProp.WORD )
                        : new Ctype( ASCII.WORD );

                return -1;

            case 'x':
                return x();

            case 'y':
                break;

            case 'z':
                if ( inclass ) break;
                if ( create ) root = new End();

                return -1;

            default:
                return ch;
        }

        throw error( "Illegal/unsupported escape sequence" );
    }

    /**
     * Parse a character class, and return the node that matches it.
     *
     * Consumes a ] on the way out if consume is true. Usually consume
     * is true except for the case of [abc&&def] where def is a separate
     * right hand node with "understood" brackets.
     */
    private CharProperty clazz( bool consume )
    {
        CharProperty prev         = null;
        CharProperty node         = null;
        BitClass     bits         = new BitClass();
        bool         include      = true;
        bool         firstInClass = true;
        int          ch           = next();

        for ( ;; )
        {
            switch ( ch )
            {
                case '^':
                    // Negates if first char in a class, otherwise literal
                    if ( firstInClass )
                    {
                        if ( temp[ cursor - 1 ] != '[' )
                            break;

                        ch      = next();
                        include = !include;

                        continue;
                    }
                    else
                    {
                        // ^ not first in class, treat as literal
                        break;
                    }

                case '[':
                    firstInClass = false;
                    node         = clazz( true );

                    if ( prev == null )
                        prev = node;
                    else
                        prev = union( prev, node );

                    ch = peek();

                    continue;

                case '&':
                    firstInClass = false;
                    ch           = next();

                    if ( ch == '&' )
                    {
                        ch = next();
                        CharProperty rightNode = null;

                        while ( ch != ']' && ch != '&' )
                        {
                            if ( ch == '[' )
                            {
                                if ( rightNode == null )
                                    rightNode = clazz( true );
                                else
                                    rightNode = union( rightNode, clazz( true ) );
                            }
                            else
                            {
                                // abc&&def
                                unread();
                                rightNode = clazz( false );
                            }

                            ch = peek();
                        }

                        if ( rightNode != null )
                            node = rightNode;

                        if ( prev == null )
                        {
                            if ( rightNode == null )
                                throw error( "Bad class syntax" );
                            else
                                prev = rightNode;
                        }
                        else
                        {
                            prev = intersection( prev, node );
                        }
                    }
                    else
                    {
                        // treat as a literal &
                        unread();

                        break;
                    }

                    continue;

                case 0:
                    firstInClass = false;

                    if ( cursor >= patternLength )
                        throw error( "Unclosed character class" );

                    break;

                case ']':
                    firstInClass = false;

                    if ( prev != null )
                    {
                        if ( consume )
                            next();

                        return prev;
                    }

                    break;

                default:
                    firstInClass = false;

                    break;
            }

            node = range( bits );

            if ( include )
            {
                if ( prev == null )
                {
                    prev = node;
                }
                else
                {
                    if ( prev != node )
                        prev = union( prev, node );
                }
            }
            else
            {
                if ( prev == null )
                {
                    prev = node.complement();
                }
                else
                {
                    if ( prev != node )
                        prev = setDifference( prev, node );
                }
            }

            ch = peek();
        }
    }

    private CharProperty bitsOrSingle( BitClass bits, int ch )
    {
        /* Bits can only handle codepoints in [u+0000-u+00ff] range.
           Use "single" node instead of bits when dealing with unicode
           case folding for codepoints listed below.
           (1)Uppercase out of range: u+00ff, u+00b5
              toUpperCase(u+00ff) -> u+0178
              toUpperCase(u+00b5) -> u+039c
           (2)LatinSmallLetterLongS u+17f
              toUpperCase(u+017f) -> u+0053
           (3)LatinSmallLetterDotlessI u+131
              toUpperCase(u+0131) -> u+0049
           (4)LatinCapitalLetterIWithDotAbove u+0130
              toLowerCase(u+0130) -> u+0069
           (5)KelvinSign u+212a
              toLowerCase(u+212a) ==> u+006B
           (6)AngstromSign u+212b
              toLowerCase(u+212b) ==> u+00e5
        */
        int d;

        if ( ch < 256
             && !( has( CASE_INSENSITIVE )
                   && has( UNICODE_CASE )
                   && ( ch == 0xff
                        || ch == 0xb5
                        || ch == 0x49
                        || ch == 0x69
                        || //I and i
                        ch == 0x53
                        || ch == 0x73
                        || //S and s
                        ch == 0x4b
                        || ch == 0x6b
                        || //K and k
                        ch == 0xc5
                        || ch == 0xe5 ) ) ) //A+ring
            return bits.Add( ch, flags() );

        return newSingle( ch );
    }

    /**
     * Parse a single character or a character range in a character class
     * and return its representative node.
     */
    private CharProperty range( BitClass bits )
    {
        int ch = peek();

        if ( ch == '\\' )
        {
            ch = nextEscaped();

            if ( ch == 'p' || ch == 'P' )
            {
                // A property
                bool comp      = ( ch == 'P' );
                bool oneLetter = true;
                // Consume { if present
                ch = next();

                if ( ch != '{' )
                    unread();
                else
                    oneLetter = false;

                return family( oneLetter, comp );
            }
            else
            {
                // ordinary escape
                bool isrange = temp[ cursor + 1 ] == '-';
                unread();
                ch = escape( true, true, isrange );

                if ( ch == -1 )
                    return ( CharProperty )root;
            }
        }
        else
        {
            next();
        }

        if ( ch >= 0 )
        {
            if ( peek() == '-' )
            {
                int endRange = temp[ cursor + 1 ];

                if ( endRange == '[' )
                {
                    return bitsOrSingle( bits, ch );
                }

                if ( endRange != ']' )
                {
                    next();
                    int m = peek();

                    if ( m == '\\' )
                    {
                        m = escape( true, false, true );
                    }
                    else
                    {
                        next();
                    }

                    if ( m < ch )
                    {
                        throw error( "Illegal character range" );
                    }

                    if ( has( CASE_INSENSITIVE ) )
                        return caseInsensitiveRangeFor( ch, m );
                    else
                        return rangeFor( ch, m );
                }
            }

            return bitsOrSingle( bits, ch );
        }

        throw error( "Unexpected character '" + ( ( char )ch ) + "'" );
    }

    /**
     * Parses a Unicode character family and returns its representative node.
     */
    private CharProperty family( bool singleLetter,
                                 bool maybeComplement )
    {
        next();
        String       name;
        CharProperty node = null;

        if ( singleLetter )
        {
            int c = temp[ cursor ];

            if ( !Character.isSupplementaryCodePoint( c ) )
            {
                name = String.valueOf( ( char )c );
            }
            else
            {
                name = new String( temp, cursor, 1 );
            }

            read();
        }
        else
        {
            int i = cursor;
            mark( '}' );

            while ( read() != '}' )
            {
            }

            mark( '\000' );
            int j = cursor;

            if ( j > patternLength )
                throw error( "Unclosed character family" );

            if ( i + 1 >= j )
                throw error( "Empty character family" );

            name = new String( temp, i, j - i - 1 );
        }

        int i = name.indexOf( '=' );

        if ( i != -1 )
        {
            // property construct \p{name=value}
            String value = name.substring( i + 1 );
            name = name.substring( 0, i ).toLowerCase( Locale.ENGLISH );

            if ( "sc".equals( name ) || "script".equals( name ) )
            {
                node = unicodeScriptPropertyFor( value );
            }
            else if ( "blk".equals( name ) || "block".equals( name ) )
            {
                node = unicodeBlockPropertyFor( value );
            }
            else if ( "gc".equals( name ) || "general_category".equals( name ) )
            {
                node = charPropertyNodeFor( value );
            }
            else
            {
                throw error
                    (
                     "Unknown Unicode property {name=<"
                     + name
                     + ">, "
                     + "value=<"
                     + value
                     + ">}"
                    );
            }
        }
        else
        {
            if ( name.startsWith( "In" ) )
            {
                // \p{inBlockName}
                node = unicodeBlockPropertyFor( name.substring( 2 ) );
            }
            else if ( name.startsWith( "Is" ) )
            {
                // \p{isGeneralCategory} and \p{isScriptName}
                name = name.substring( 2 );
                UnicodeProp uprop = UnicodeProp.forName( name );

                if ( uprop != null )
                    node = new Utype( uprop );

                if ( node == null )
                    node = CharPropertyNames.charPropertyFor( name );

                if ( node == null )
                    node = unicodeScriptPropertyFor( name );
            }
            else
            {
                if ( has( UNICODE_CHARACTER_CLASS ) )
                {
                    UnicodeProp uprop = UnicodeProp.forPOSIXName( name );

                    if ( uprop != null )
                        node = new Utype( uprop );
                }

                if ( node == null )
                    node = charPropertyNodeFor( name );
            }
        }

        if ( maybeComplement )
        {
            if ( node instanceof Category || node instanceof Block)
            hasSupplementary = true;
            node             = node.complement();
        }

        return node;
    }


    /**
     * Returns a CharProperty matching all characters belong to
     * a UnicodeScript.
     */
    private CharProperty unicodeScriptPropertyFor( String name )
    {
        Character.UnicodeScript script;

        try
        {
            script = Character.UnicodeScript.forName( name );
        }
        catch ( IllegalArgumentException iae )
        {
            throw error( "Unknown character script name {" + name + "}" );
        }

        return new Script( script );
    }

    /**
     * Returns a CharProperty matching all characters in a UnicodeBlock.
     */
    private CharProperty unicodeBlockPropertyFor( String name )
    {
        Character.UnicodeBlock block;

        try
        {
            block = Character.UnicodeBlock.forName( name );
        }
        catch ( IllegalArgumentException iae )
        {
            throw error( "Unknown character block name {" + name + "}" );
        }

        return new Block( block );
    }

    /**
     * Returns a CharProperty matching all characters in a named property.
     */
    private CharProperty charPropertyNodeFor( String name )
    {
        CharProperty p = CharPropertyNames.charPropertyFor( name );

        if ( p == null )
            throw error( "Unknown character property name {" + name + "}" );

        return p;
    }

    /**
     * Parses and returns the name of a "named capturing group", the trailing
     * ">" is consumed after parsing.
     */
    private String groupname( int ch )
    {
        StringBuilder sb = new StringBuilder();
        sb.append( Character.toChars( ch ) );

        while ( ASCII.isLower( ch = read() ) || ASCII.isUpper( ch ) || ASCII.isDigit( ch ) )
        {
            sb.append( Character.toChars( ch ) );
        }

        if ( sb.length() == 0 )
            throw error( "named capturing group has 0 length name" );

        if ( ch != '>' )
            throw error( "named capturing group is missing trailing '>'" );

        return sb.toString();
    }

    /**
     * Parses a group and returns the head node of a set of nodes that process
     * the group. Sometimes a double return system is used where the tail is
     * returned in root.
     */
    private Node group0()
    {
        bool capturingGroup = false;
        Node head           = null;
        Node tail           = null;
        int  save           = flags;
        root = null;
        int ch = next();

        if ( ch == '?' )
        {
            ch = skip();

            switch ( ch )
            {
                case ':': //  (?:xxx) pure group
                    head      = createGroup( true );
                    tail      = root;
                    head.next = expr( tail );

                    break;

                case '=': // (?=xxx) and (?!xxx) lookahead
                case '!':
                    head      = createGroup( true );
                    tail      = root;
                    head.next = expr( tail );

                    if ( ch == '=' )
                    {
                        head = tail = new Pos( head );
                    }
                    else
                    {
                        head = tail = new Neg( head );
                    }

                    break;

                case '>': // (?>xxx)  independent group
                    head      = createGroup( true );
                    tail      = root;
                    head.next = expr( tail );
                    head      = tail = new Ques( head, INDEPENDENT );

                    break;

                case '<': // (?<xxx)  look behind
                    ch = read();

                    if ( ASCII.isLower( ch ) || ASCII.isUpper( ch ) )
                    {
                        // named captured group
                        String name = groupname( ch );

                        if ( namedGroups().containsKey( name ) )
                            throw error
                                (
                                 "Named capturing group <"
                                 + name
                                 + "> is already defined"
                                );

                        capturingGroup = true;
                        head           = createGroup( false );
                        tail           = root;
                        namedGroups().put( name, capturingGroupCount - 1 );
                        head.next = expr( tail );

                        break;
                    }

                    int start = cursor;
                    head      = createGroup( true );
                    tail      = root;
                    head.next = expr( tail );
                    tail.next = lookbehindEnd;
                    TreeInfo info = new TreeInfo();
                    head.study( info );

                    if ( info.maxValid == false )
                    {
                        throw error
                            (
                             "Look-behind group does not have "
                             + "an obvious maximum length"
                            );
                    }

                    bool hasSupplementary = findSupplementary( start, patternLength );

                    if ( ch == '=' )
                    {
                        head = tail = ( hasSupplementary
                            ? new BehindS
                                (
                                 head,
                                 info.maxLength,
                                 info.minLength
                                )
                            : new Behind
                                (
                                 head,
                                 info.maxLength,
                                 info.minLength
                                ) );
                    }
                    else if ( ch == '!' )
                    {
                        head = tail = ( hasSupplementary
                            ? new NotBehindS
                                (
                                 head,
                                 info.maxLength,
                                 info.minLength
                                )
                            : new NotBehind
                                (
                                 head,
                                 info.maxLength,
                                 info.minLength
                                ) );
                    }
                    else
                    {
                        throw error( "Unknown look-behind group" );
                    }

                    break;

                case '$':
                case '@':
                    throw error( "Unknown group type" );

                default: // (?xxx:) inlined match flags
                    unread();
                    addFlag();
                    ch = read();

                    if ( ch == ')' )
                    {
                        return null; // Inline modifier only
                    }

                    if ( ch != ':' )
                    {
                        throw error( "Unknown inline modifier" );
                    }

                    head      = createGroup( true );
                    tail      = root;
                    head.next = expr( tail );

                    break;
            }
        }
        else
        {
            // (xxx) a regular group
            capturingGroup = true;
            head           = createGroup( false );
            tail           = root;
            head.next      = expr( tail );
        }

        accept( ')', "Unclosed group" );
        flags = save;

        // Check for quantifiers
        Node node = closure( head );

        if ( node == head )
        {
            // No closure
            root = tail;

            return node; // Dual return
        }

        if ( head == tail )
        {
            // Zero length assertion
            root = node;

            return node; // Dual return
        }

        if ( node instanceof Ques) {
            Ques ques = ( Ques )node;

            if ( ques.type == POSSESSIVE )
            {
                root = node;

                return node;
            }

            tail.next = new BranchConn();
            tail      = tail.next;

            if ( ques.type == GREEDY )
            {
                head = new Branch( head, null, tail );
            }
            else
            {
                // Reluctant quantifier
                head = new Branch( null, head, tail );
            }

            root = tail;

            return head;
        } else if ( node instanceof Curly) {
            Curly curly = ( Curly )node;

            if ( curly.type == POSSESSIVE )
            {
                root = node;

                return node;
            }

            // Discover if the group is deterministic
            TreeInfo info = new TreeInfo();

            if ( head.study( info ) )
            {
                // Deterministic
                GroupTail temp = ( GroupTail )tail;

                head = root = new GroupCurly
                    (
                     head.next,
                     curly.cmin,
                     curly.cmax,
                     curly.type,
                     ( ( GroupTail )tail ).localIndex,
                     ( ( GroupTail )tail ).groupIndex,
                     capturingGroup
                    );

                return head;
            }
            else
            {
                // Non-deterministic
                int  temp = ( ( GroupHead )head ).localIndex;
                Loop loop;

                if ( curly.type == GREEDY )
                    loop = new Loop( this.localCount, temp );
                else // Reluctant Curly
                    loop = new LazyLoop( this.localCount, temp );

                Prolog prolog = new Prolog( loop );
                this.localCount += 1;
                loop.cmin       =  curly.cmin;
                loop.cmax       =  curly.cmax;
                loop.body       =  head;
                tail.next       =  loop;
                root            =  loop;

                return prolog; // Dual return
            }
        }

        throw error( "Internal logic error" );
    }

    /**
     * Create group head and tail nodes using double return. If the group is
     * created with anonymous true then it is a pure group and should not
     * affect group counting.
     */
    private Node createGroup( bool anonymous )
    {
        int localIndex = localCount++;
        int groupIndex = 0;

        if ( !anonymous )
            groupIndex = capturingGroupCount++;

        GroupHead head = new GroupHead( localIndex );
        root = new GroupTail( localIndex, groupIndex );

        if ( !anonymous && groupIndex < 10 )
            groupNodes[ groupIndex ] = head;

        return head;
    }

    /**
     * Parses inlined match flags and set them appropriately.
     */
    private void addFlag()
    {
        int ch = peek();

        for ( ;; )
        {
            switch ( ch )
            {
                case 'i':
                    flags |= CASE_INSENSITIVE;

                    break;

                case 'm':
                    flags |= MULTILINE;

                    break;

                case 's':
                    flags |= DOTALL;

                    break;

                case 'd':
                    flags |= UNIX_LINES;

                    break;

                case 'u':
                    flags |= UNICODE_CASE;

                    break;

                case 'c':
                    flags |= CANON_EQ;

                    break;

                case 'x':
                    flags |= COMMENTS;

                    break;

                case 'U':
                    flags |= ( UNICODE_CHARACTER_CLASS | UNICODE_CASE );

                    break;

                case '-': // subFlag then fall through
                    ch = next();
                    subFlag();

                default:
                    return;
            }

            ch = next();
        }
    }

    /**
     * Parses the second part of inlined match flags and turns off
     * flags appropriately.
     */
    private void subFlag()
    {
        int ch = peek();

        for ( ;; )
        {
            switch ( ch )
            {
                case 'i':
                    flags &= ~CASE_INSENSITIVE;

                    break;

                case 'm':
                    flags &= ~MULTILINE;

                    break;

                case 's':
                    flags &= ~DOTALL;

                    break;

                case 'd':
                    flags &= ~UNIX_LINES;

                    break;

                case 'u':
                    flags &= ~UNICODE_CASE;

                    break;

                case 'c':
                    flags &= ~CANON_EQ;

                    break;

                case 'x':
                    flags &= ~COMMENTS;

                    break;

                case 'U':
                    flags &= ~( UNICODE_CHARACTER_CLASS | UNICODE_CASE );

                default:
                    return;
            }

            ch = next();
        }
    }

    /**
     * Processes repetition. If the next character peeked is a quantifier
     * then new nodes must be appended to handle the repetition.
     * Prev could be a single or a group, so it could be a chain of nodes.
     */
    private Node closure( Node prev )
    {
        Node atom;
        int  ch = peek();

        switch ( ch )
        {
            case '?':
                ch = next();

                if ( ch == '?' )
                {
                    next();

                    return new Ques( prev, LAZY );
                }
                else if ( ch == '+' )
                {
                    next();

                    return new Ques( prev, POSSESSIVE );
                }

                return new Ques( prev, GREEDY );

            case '*':
                ch = next();

                if ( ch == '?' )
                {
                    next();

                    return new Curly( prev, 0, MAX_REPS, LAZY );
                }
                else if ( ch == '+' )
                {
                    next();

                    return new Curly( prev, 0, MAX_REPS, POSSESSIVE );
                }

                return new Curly( prev, 0, MAX_REPS, GREEDY );

            case '+':
                ch = next();

                if ( ch == '?' )
                {
                    next();

                    return new Curly( prev, 1, MAX_REPS, LAZY );
                }
                else if ( ch == '+' )
                {
                    next();

                    return new Curly( prev, 1, MAX_REPS, POSSESSIVE );
                }

                return new Curly( prev, 1, MAX_REPS, GREEDY );

            case '{':
                ch = temp[ cursor + 1 ];

                if ( ASCII.isDigit( ch ) )
                {
                    skip();
                    int cmin = 0;

                    do
                    {
                        cmin = cmin * 10 + ( ch - '0' );
                    }
                    while ( ASCII.isDigit( ch = read() ) );

                    int cmax = cmin;

                    if ( ch == ',' )
                    {
                        ch   = read();
                        cmax = MAX_REPS;

                        if ( ch != '}' )
                        {
                            cmax = 0;

                            while ( ASCII.isDigit( ch ) )
                            {
                                cmax = cmax * 10 + ( ch - '0' );
                                ch   = read();
                            }
                        }
                    }

                    if ( ch != '}' )
                        throw error( "Unclosed counted closure" );

                    if ( ( ( cmin ) | ( cmax ) | ( cmax - cmin ) ) < 0 )
                        throw error( "Illegal repetition range" );

                    Curly curly;
                    ch = peek();

                    if ( ch == '?' )
                    {
                        next();
                        curly = new Curly( prev, cmin, cmax, LAZY );
                    }
                    else if ( ch == '+' )
                    {
                        next();
                        curly = new Curly( prev, cmin, cmax, POSSESSIVE );
                    }
                    else
                    {
                        curly = new Curly( prev, cmin, cmax, GREEDY );
                    }

                    return curly;
                }
                else
                {
                    throw error( "Illegal repetition" );
                }

            default:
                return prev;
        }
    }

    /**
     *  Utility method for parsing control escape sequences.
     */
    private int c()
    {
        if ( cursor < patternLength )
        {
            return read() ^ 64;
        }

        throw error( "Illegal control escape sequence" );
    }

    /**
     *  Utility method for parsing octal escape sequences.
     */
    private int o()
    {
        int n = read();

        if ( ( ( n - '0' ) | ( '7' - n ) ) >= 0 )
        {
            int m = read();

            if ( ( ( m - '0' ) | ( '7' - m ) ) >= 0 )
            {
                int o = read();

                if ( ( ( ( o - '0' ) | ( '7' - o ) ) >= 0 ) && ( ( ( n - '0' ) | ( '3' - n ) ) >= 0 ) )
                {
                    return ( n - '0' ) * 64 + ( m - '0' ) * 8 + ( o - '0' );
                }

                unread();

                return ( n - '0' ) * 8 + ( m - '0' );
            }

            unread();

            return ( n - '0' );
        }

        throw error( "Illegal octal escape sequence" );
    }

    /**
     *  Utility method for parsing hexadecimal escape sequences.
     */
    private int x()
    {
        int n = read();

        if ( ASCII.isHexDigit( n ) )
        {
            int m = read();

            if ( ASCII.isHexDigit( m ) )
            {
                return ASCII.toDigit( n ) * 16 + ASCII.toDigit( m );
            }
        }
        else if ( n == '{' && ASCII.isHexDigit( peek() ) )
        {
            int ch = 0;

            while ( ASCII.isHexDigit( n = read() ) )
            {
                ch = ( ch << 4 ) + ASCII.toDigit( n );

                if ( ch > Character.MAX_CODE_POINT )
                    throw error( "Hexadecimal codepoint is too big" );
            }

            if ( n != '}' )
                throw error( "Unclosed hexadecimal escape sequence" );

            return ch;
        }

        throw error( "Illegal hexadecimal escape sequence" );
    }

    /**
     *  Utility method for parsing unicode escape sequences.
     */
    private int cursor()
    {
        return cursor;
    }

    private void setcursor( int pos )
    {
        cursor = pos;
    }

    private int uxxxx()
    {
        int n = 0;

        for ( int i = 0; i < 4; i++ )
        {
            int ch = read();

            if ( !ASCII.isHexDigit( ch ) )
            {
                throw error( "Illegal Unicode escape sequence" );
            }

            n = n * 16 + ASCII.toDigit( ch );
        }

        return n;
    }

    private int u()
    {
        int n = uxxxx();

        if ( Character.isHighSurrogate( ( char )n ) )
        {
            int cur = cursor();

            if ( read() == '\\' && read() == 'u' )
            {
                int n2 = uxxxx();

                if ( Character.isLowSurrogate( ( char )n2 ) )
                    return Character.toCodePoint( ( char )n, ( char )n2 );
            }

            setcursor( cur );
        }

        return n;
    }

    //
    // Utility methods for code point support
    //

    private static int countChars( CharSequence seq,
                                   int index,
                                   int lengthInCodePoints )
    {
        // optimization
        if ( lengthInCodePoints == 1 && !Character.isHighSurrogate( seq.charAt( index ) ) )
        {
            assert( index >= 0 && index < seq.length() );

            return 1;
        }

        int length = seq.length();
        int x      = index;

        if ( lengthInCodePoints >= 0 )
        {
            assert( index >= 0 && index < length );

            for ( int i = 0; x < length && i < lengthInCodePoints; i++ )
            {
                if ( Character.isHighSurrogate( seq.charAt( x++ ) ) )
                {
                    if ( x < length && Character.isLowSurrogate( seq.charAt( x ) ) )
                    {
                        x++;
                    }
                }
            }

            return x - index;
        }

        assert( index >= 0 && index <= length );

        if ( index == 0 )
        {
            return 0;
        }

        int len = -lengthInCodePoints;

        for ( int i = 0; x > 0 && i < len; i++ )
        {
            if ( Character.isLowSurrogate( seq.charAt( --x ) ) )
            {
                if ( x > 0 && Character.isHighSurrogate( seq.charAt( x - 1 ) ) )
                {
                    x--;
                }
            }
        }

        return index - x;
    }

    private static int countCodePoints( CharSequence seq )
    {
        int length = seq.length();
        int n      = 0;

        for ( int i = 0; i < length; )
        {
            n++;

            if ( Character.isHighSurrogate( seq.charAt( i++ ) ) )
            {
                if ( i < length && Character.isLowSurrogate( seq.charAt( i ) ) )
                {
                    i++;
                }
            }
        }

        return n;
    }
    
/**
 *  Returns a suitably optimized, single character matcher.
 */
private Pattern.CharProperty newSingle( int ch )
{
    if ( Has( Case_Insensitive ) )
    {
        int lower, upper;

        if ( Has( Unicode_Case ) )
        {
            upper = Character.toUpperCase( ch );
            lower = Character.toLowerCase( upper );

            if ( upper != lower )
                return new Pattern.SingleU( lower );
        }
        else if ( ASCII.IsAscii( ch ) )
        {
            lower = ASCII.ToLower( ch );
            upper = ASCII.ToUpper( ch );

            if ( lower != upper )
                return new Pattern.SingleI( lower, upper );
        }
    }

    if ( isSupplementary( ch ) )
    {
        return new Pattern.SingleS( ch ); // Match a given Unicode character
    }

    return new Single( ch ); // Match a given BMP character
}

/**
 *  Utility method for creating a string slice matcher.
 */
private Pattern.Node newSlice( int[] buf, int count, bool hasSupplementary )
{
    int[] tmp = new int[ count ];

    if ( has( CASE_INSENSITIVE ) )
    {
        if ( has( UNICODE_CASE ) )
        {
            for ( int i = 0; i < count; i++ )
            {
                tmp[ i ] = Character.toLowerCase
                    (
                     Character.toUpperCase( buf[ i ] )
                    );
            }

            return hasSupplementary ? new Pattern.SliceUS( tmp ) : new Pattern.SliceU( tmp );
        }

        for ( int i = 0; i < count; i++ )
        {
            tmp[ i ] = ASCII.toLower( buf[ i ] );
        }

        return hasSupplementary ? new Pattern.SliceIS( tmp ) : new Pattern.SliceI( tmp );
    }

    for ( int i = 0; i < count; i++ )
    {
        tmp[ i ] = buf[ i ];
    }

    return hasSupplementary ? new Pattern.SliceS( tmp ) : new Pattern.Slice( tmp );
}

private static bool inRange( int lower, int ch, int upper )
{
    return lower <= ch && ch <= upper;
}

/**
 * Returns node for matching characters within an explicit value range.
 */
private static Pattern.CharProperty rangeFor( int lower,
                                              int upper )
{
    return new Pattern.CharProperty()
    {
        bool isSatisfiedBy(int ch) {
        return inRange(lower, ch, upper);
    }};
}

/**
 * Returns node for matching characters within an explicit value
 * range in a case insensitive manner.
 */
private Pattern.CharProperty caseInsensitiveRangeFor( int lower,
                                                      int upper )
{
    if ( has( UNICODE_CASE ) )
        return new Pattern.CharProperty()
        {
            bool isSatisfiedBy(int ch) {
            if (inRange(lower, ch, upper))
            return true;
            int up = Character.toUpperCase(ch);
            return inRange(lower, up, upper) ||
            inRange(lower, Character.toLowerCase( up ), upper);
        }};

    return new Pattern.CharProperty()
    {
        bool isSatisfiedBy(int ch) {
        return inRange(lower, ch, upper) ||
        ASCII.isAscii(ch) &&
        (inRange(lower, ASCII.toUpper( ch ), upper) ||
        inRange(lower, ASCII.toLower( ch ), upper));
    }};
}

/**
     * Implements the Unicode category ALL and the dot metacharacter when
     * in dotall mode.
     */
static class All extends CharProperty {
bool isSatisfiedBy( int ch )
{
    return true;
}
}

/**
     * Node class for the dot metacharacter when dotall is not enabled.
     */
static class Dot extends CharProperty {
bool isSatisfiedBy( int ch )
{
    return ( ch != '\n'
             && ch != '\r'
             && ( ch | 1 ) != '\u2029'
             && ch != '\u0085' );
}
}

/**
     * Node class for the dot metacharacter when dotall is not enabled
     * but UNIX_LINES is enabled.
     */
static class UnixDot extends CharProperty {
bool isSatisfiedBy( int ch )
{
    return ch != '\n';
}
}

/**
     * The 0 or 1 quantifier. This one class implements all three types.
     */
static class Ques extends Node {
Pattern.Node atom;
int          type;

Ques( Pattern.Node node, int type )
{
    this.atom = node;
    this.type = type;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    switch ( type )
    {
        case GREEDY:
            return ( atom.match( matcher, i, seq ) && next.match( matcher, matcher.last, seq ) )
                   || next.match( matcher, i, seq );

        case LAZY:
            return next.match( matcher, i, seq )
                   || ( atom.match( matcher, i, seq ) && next.match( matcher, matcher.last, seq ) );

        case POSSESSIVE:
            if ( atom.match( matcher, i, seq ) ) i = matcher.last;

            return next.match( matcher, i, seq );

        default:
            return atom.match( matcher, i, seq ) && next.match( matcher, matcher.last, seq );
    }
}

bool study( Pattern.TreeInfo info )
{
    if ( type != INDEPENDENT )
    {
        int minL = info.minLength;
        atom.study( info );
        info.minLength     = minL;
        info.deterministic = false;

        return next.study( info );
    }
    else
    {
        atom.study( info );

        return next.study( info );
    }
}
}

/**
     * Handles the curly-brace style repetition with a specified minimum and
     * maximum occurrences. The * quantifier is handled as a special case.
     * This class handles the three types.
     */
static class Curly extends Node {
Pattern.Node atom;
int          type;
int          cmin;
int          cmax;

Curly( Pattern.Node node, int cmin, int cmax, int type )
{
    this.atom = node;
    this.type = type;
    this.cmin = cmin;
    this.cmax = cmax;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    int j;

    for ( j = 0; j < cmin; j++ )
    {
        if ( atom.match( matcher, i, seq ) )
        {
            i = matcher.last;

            continue;
        }

        return false;
    }

    if ( type == GREEDY )
        return match0( matcher, i, j, seq );
    else if ( type == LAZY )
        return match1( matcher, i, j, seq );
    else
        return match2( matcher, i, j, seq );
}

// Greedy match.
// i is the index to start matching at
// j is the number of atoms that have matched
bool match0( Matcher matcher, int i, int j, CharSequence seq )
{
    if ( j >= cmax )
    {
        // We have matched the maximum... continue with the rest of
        // the regular expression
        return next.match( matcher, i, seq );
    }

    int backLimit = j;

    while ( atom.match( matcher, i, seq ) )
    {
        // k is the length of this match
        int k = matcher.last - i;

        if ( k == 0 ) // Zero length match
            break;

        // Move up index and number matched
        i = matcher.last;
        j++;

        // We are greedy so match as many as we can
        while ( j < cmax )
        {
            if ( !atom.match( matcher, i, seq ) )
                break;

            if ( i + k != matcher.last )
            {
                if ( match0( matcher, matcher.last, j + 1, seq ) )
                    return true;

                break;
            }

            i += k;
            j++;
        }

        // Handle backing off if match fails
        while ( j >= backLimit )
        {
            if ( next.match( matcher, i, seq ) )
                return true;

            i -= k;
            j--;
        }

        return false;
    }

    return next.match( matcher, i, seq );
}

// Reluctant match. At this point, the minimum has been satisfied.
// i is the index to start matching at
// j is the number of atoms that have matched
bool match1( Matcher matcher, int i, int j, CharSequence seq )
{
    for ( ;; )
    {
        // Try finishing match without consuming any more
        if ( next.match( matcher, i, seq ) )
            return true;

        // At the maximum, no match found
        if ( j >= cmax )
            return false;

        // Okay, must try one more atom
        if ( !atom.match( matcher, i, seq ) )
            return false;

        // If we haven't moved forward then must break out
        if ( i == matcher.last )
            return false;

        // Move up index and number matched
        i = matcher.last;
        j++;
    }
}

bool match2( Matcher matcher, int i, int j, CharSequence seq )
{
    for ( ; j < cmax; j++ )
    {
        if ( !atom.match( matcher, i, seq ) )
            break;

        if ( i == matcher.last )
            break;

        i = matcher.last;
    }

    return next.match( matcher, i, seq );
}

bool study( Pattern.TreeInfo info )
{
    // Save original info
    int  minL = info.minLength;
    int  maxL = info.maxLength;
    bool maxV = info.maxValid;
    bool detm = info.deterministic;
    info.reset();

    atom.study( info );

    int temp = info.minLength * cmin + minL;

    if ( temp < minL )
    {
        temp = 0xFFFFFFF; // arbitrary large number
    }

    info.minLength = temp;

    if ( maxV & info.maxValid )
    {
        temp           = info.maxLength * cmax + maxL;
        info.maxLength = temp;

        if ( temp < maxL )
        {
            info.maxValid = false;
        }
    }
    else
    {
        info.maxValid = false;
    }

    if ( info.deterministic && cmin == cmax )
        info.deterministic = detm;
    else
        info.deterministic = false;

    return next.study( info );
}
}

/**
     * Handles the curly-brace style repetition with a specified minimum and
     * maximum occurrences in deterministic cases. This is an iterative
     * optimization over the Prolog and Loop system which would handle this
     * in a recursive way. The * quantifier is handled as a special case.
     * If capture is true then this class saves group settings and ensures
     * that groups are unset when backing off of a group match.
     */
static class GroupCurly extends Node {
Pattern.Node atom;
int          type;
int          cmin;
int          cmax;
int          localIndex;
int          groupIndex;
bool         capture;

GroupCurly( Pattern.Node node,
            int cmin,
            int cmax,
            int type,
            int local,
            int group,
            bool capture )
{
    this.atom       = node;
    this.type       = type;
    this.cmin       = cmin;
    this.cmax       = cmax;
    this.localIndex = local;
    this.groupIndex = group;
    this.capture    = capture;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    int[] groups = matcher.groups;
    int[] locals = matcher.locals;
    int   save0  = locals[ localIndex ];
    int   save1  = 0;
    int   save2  = 0;

    if ( capture )
    {
        save1 = groups[ groupIndex ];
        save2 = groups[ groupIndex + 1 ];
    }

    // Notify GroupTail there is no need to setup group info
    // because it will be set here
    locals[ localIndex ] = -1;

    bool ret = true;

    for ( int j = 0; j < cmin; j++ )
    {
        if ( atom.match( matcher, i, seq ) )
        {
            if ( capture )
            {
                groups[ groupIndex ]     = i;
                groups[ groupIndex + 1 ] = matcher.last;
            }

            i = matcher.last;
        }
        else
        {
            ret = false;

            break;
        }
    }

    if ( ret )
    {
        if ( type == GREEDY )
        {
            ret = match0( matcher, i, cmin, seq );
        }
        else if ( type == LAZY )
        {
            ret = match1( matcher, i, cmin, seq );
        }
        else
        {
            ret = match2( matcher, i, cmin, seq );
        }
    }

    if ( !ret )
    {
        locals[ localIndex ] = save0;

        if ( capture )
        {
            groups[ groupIndex ]     = save1;
            groups[ groupIndex + 1 ] = save2;
        }
    }

    return ret;
}

// Aggressive group match
bool match0( Matcher matcher, int i, int j, CharSequence seq )
{
    // don't back off passing the starting "j"
    int   min    = j;
    int[] groups = matcher.groups;
    int   save0  = 0;
    int   save1  = 0;

    if ( capture )
    {
        save0 = groups[ groupIndex ];
        save1 = groups[ groupIndex + 1 ];
    }

    for ( ;; )
    {
        if ( j >= cmax )
            break;

        if ( !atom.match( matcher, i, seq ) )
            break;

        int k = matcher.last - i;

        if ( k <= 0 )
        {
            if ( capture )
            {
                groups[ groupIndex ]     = i;
                groups[ groupIndex + 1 ] = i + k;
            }

            i = i + k;

            break;
        }

        for ( ;; )
        {
            if ( capture )
            {
                groups[ groupIndex ]     = i;
                groups[ groupIndex + 1 ] = i + k;
            }

            i = i + k;

            if ( ++j >= cmax )
                break;

            if ( !atom.match( matcher, i, seq ) )
                break;

            if ( i + k != matcher.last )
            {
                if ( match0( matcher, i, j, seq ) )
                    return true;

                break;
            }
        }

        while ( j > min )
        {
            if ( next.match( matcher, i, seq ) )
            {
                if ( capture )
                {
                    groups[ groupIndex + 1 ] = i;
                    groups[ groupIndex ]     = i - k;
                }

                return true;
            }

            // backing off
            i = i - k;

            if ( capture )
            {
                groups[ groupIndex + 1 ] = i;
                groups[ groupIndex ]     = i - k;
            }

            j--;

        }

        break;
    }

    if ( capture )
    {
        groups[ groupIndex ]     = save0;
        groups[ groupIndex + 1 ] = save1;
    }

    return next.match( matcher, i, seq );
}

// Reluctant matching
bool match1( Matcher matcher, int i, int j, CharSequence seq )
{
    for ( ;; )
    {
        if ( next.match( matcher, i, seq ) )
            return true;

        if ( j >= cmax )
            return false;

        if ( !atom.match( matcher, i, seq ) )
            return false;

        if ( i == matcher.last )
            return false;

        if ( capture )
        {
            matcher.groups[ groupIndex ]     = i;
            matcher.groups[ groupIndex + 1 ] = matcher.last;
        }

        i = matcher.last;
        j++;
    }
}

// Possessive matching
bool match2( Matcher matcher, int i, int j, CharSequence seq )
{
    for ( ; j < cmax; j++ )
    {
        if ( !atom.match( matcher, i, seq ) )
        {
            break;
        }

        if ( capture )
        {
            matcher.groups[ groupIndex ]     = i;
            matcher.groups[ groupIndex + 1 ] = matcher.last;
        }

        if ( i == matcher.last )
        {
            break;
        }

        i = matcher.last;
    }

    return next.match( matcher, i, seq );
}

bool study( Pattern.TreeInfo info )
{
    // Save original info
    int  minL = info.minLength;
    int  maxL = info.maxLength;
    bool maxV = info.maxValid;
    bool detm = info.deterministic;
    info.reset();

    atom.study( info );

    int temp = info.minLength * cmin + minL;

    if ( temp < minL )
    {
        temp = 0xFFFFFFF; // Arbitrary large number
    }

    info.minLength = temp;

    if ( maxV & info.maxValid )
    {
        temp           = info.maxLength * cmax + maxL;
        info.maxLength = temp;

        if ( temp < maxL )
        {
            info.maxValid = false;
        }
    }
    else
    {
        info.maxValid = false;
    }

    if ( info.deterministic && cmin == cmax )
    {
        info.deterministic = detm;
    }
    else
    {
        info.deterministic = false;
    }

    return next.study( info );
}
}

/**
     * A Guard node at the end of each atom node in a Branch. It
     * serves the purpose of chaining the "match" operation to
     * "next" but not the "study", so we can collect the TreeInfo
     * of each atom node without including the TreeInfo of the
     * "next".
     */
static class BranchConn extends Node {
BranchConn()
{
};

bool match( Matcher matcher, int i, CharSequence seq )
{
    return next.match( matcher, i, seq );
}

bool study( Pattern.TreeInfo info )
{
    return info.deterministic;
}
}

/**
     * Handles the branching of alternations. Note this is also used for
     * the ? quantifier to branch between the case where it matches once
     * and where it does not occur.
     */
static class Branch extends Node {
Pattern.Node[] atoms = new Pattern.Node[ 2 ];
int            size  = 2;
Pattern.Node   conn;

Branch( Pattern.Node first, Pattern.Node second, Pattern.Node branchConn )
{
    conn       = branchConn;
    atoms[ 0 ] = first;
    atoms[ 1 ] = second;
}

void add( Pattern.Node node )
{
    if ( size >= atoms.length )
    {
        Pattern.Node[] tmp = new Pattern.Node[ atoms.length * 2 ];
        System.arraycopy( atoms, 0, tmp, 0, atoms.length );
        atoms = tmp;
    }

    atoms[ size++ ] = node;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    for ( int n = 0; n < size; n++ )
    {
        if ( atoms[ n ] == null )
        {
            if ( conn.next.match( matcher, i, seq ) )
                return true;
        }
        else if ( atoms[ n ].match( matcher, i, seq ) )
        {
            return true;
        }
    }

    return false;
}

bool study( Pattern.TreeInfo info )
{
    int  minL = info.minLength;
    int  maxL = info.maxLength;
    bool maxV = info.maxValid;

    int minL2 = Integer.MAX_VALUE; //arbitrary large enough num
    int maxL2 = -1;

    for ( int n = 0; n < size; n++ )
    {
        info.reset();

        if ( atoms[ n ] != null )
            atoms[ n ].study( info );

        minL2 = Math.min( minL2, info.minLength );
        maxL2 = Math.max( maxL2, info.maxLength );
        maxV  = ( maxV & info.maxValid );
    }

    minL += minL2;
    maxL += maxL2;

    info.reset();
    conn.next.study( info );

    info.minLength     += minL;
    info.maxLength     += maxL;
    info.maxValid      &= maxV;
    info.deterministic =  false;

    return false;
}
}

/**
     * The GroupHead saves the location where the group begins in the locals
     * and restores them when the match is done.
     *
     * The matchRef is used when a reference to this group is accessed later
     * in the expression. The locals will have a negative value in them to
     * indicate that we do not want to unset the group if the reference
     * doesn't match.
     */
static class GroupHead extends Node {
int localIndex;

GroupHead( int localCount )
{
    localIndex = localCount;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    int save = matcher.locals[ localIndex ];
    matcher.locals[ localIndex ] = i;
    bool ret = next.match( matcher, i, seq );
    matcher.locals[ localIndex ] = save;

    return ret;
}

bool matchRef( Matcher matcher, int i, CharSequence seq )
{
    int save = matcher.locals[ localIndex ];
    matcher.locals[ localIndex ] = ~i; // HACK
    bool ret = next.match( matcher, i, seq );
    matcher.locals[ localIndex ] = save;

    return ret;
}
}

/**
     * Recursive reference to a group in the regular expression. It calls
     * matchRef because if the reference fails to match we would not unset
     * the group.
     */
static class GroupRef extends Node {
GroupHead head;

GroupRef( GroupHead head )
{
    this.head = head;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    return head.matchRef( matcher, i, seq )
           && next.match( matcher, matcher.last, seq );
}

bool study( Pattern.TreeInfo info )
{
    info.maxValid      = false;
    info.deterministic = false;

    return next.study( info );
}
}

/**
     * The GroupTail handles the setting of group beginning and ending
     * locations when groups are successfully matched. It must also be able to
     * unset groups that have to be backed off of.
     *
     * The GroupTail node is also used when a previous group is referenced,
     * and in that case no group information needs to be set.
     */
static class GroupTail extends Node {
int localIndex;
int groupIndex;

GroupTail( int localCount, int groupCount )
{
    localIndex = localCount;
    groupIndex = groupCount + groupCount;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    int tmp = matcher.locals[ localIndex ];

    if ( tmp >= 0 )
    {
        // This is the normal group case.
        // Save the group so we can unset it if it
        // backs off of a match.
        int groupStart = matcher.groups[ groupIndex ];
        int groupEnd   = matcher.groups[ groupIndex + 1 ];

        matcher.groups[ groupIndex ]     = tmp;
        matcher.groups[ groupIndex + 1 ] = i;

        if ( next.match( matcher, i, seq ) )
        {
            return true;
        }

        matcher.groups[ groupIndex ]     = groupStart;
        matcher.groups[ groupIndex + 1 ] = groupEnd;

        return false;
    }
    else
    {
        // This is a group reference case. We don't need to save any
        // group info because it isn't really a group.
        matcher.last = i;

        return true;
    }
}
}

/**
     * This sets up a loop to handle a recursive quantifier structure.
     */
static class Prolog extends Node {
Loop loop;

Prolog( Loop loop )
{
    this.loop = loop;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    return loop.matchInit( matcher, i, seq );
}

bool study( Pattern.TreeInfo info )
{
    return loop.study( info );
}
}

/**
     * Handles the repetition count for a greedy Curly. The matchInit
     * is called from the Prolog to save the index of where the group
     * beginning is stored. A zero length group check occurs in the
     * normal match but is skipped in the matchInit.
     */
static class Loop extends Node {
Pattern.Node body;
int          countIndex; // local count index in matcher locals
int          beginIndex; // group beginning index
int          cmin, cmax;

Loop( int countIndex, int beginIndex )
{
    this.countIndex = countIndex;
    this.beginIndex = beginIndex;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    // Avoid infinite loop in zero-length case.
    if ( i > matcher.locals[ beginIndex ] )
    {
        int count = matcher.locals[ countIndex ];

        // This block is for before we reach the minimum
        // iterations required for the loop to match
        if ( count < cmin )
        {
            matcher.locals[ countIndex ] = count + 1;
            bool b = body.match( matcher, i, seq );

            // If match failed we must backtrack, so
            // the loop count should NOT be incremented
            if ( !b )
                matcher.locals[ countIndex ] = count;

            // Return success or failure since we are under
            // minimum
            return b;
        }

        // This block is for after we have the minimum
        // iterations required for the loop to match
        if ( count < cmax )
        {
            matcher.locals[ countIndex ] = count + 1;
            bool b = body.match( matcher, i, seq );

            // If match failed we must backtrack, so
            // the loop count should NOT be incremented
            if ( !b )
                matcher.locals[ countIndex ] = count;
            else
                return true;
        }
    }

    return next.match( matcher, i, seq );
}

bool matchInit( Matcher matcher, int i, CharSequence seq )
{
    int  save = matcher.locals[ countIndex ];
    bool ret  = false;

    if ( 0 < cmin )
    {
        matcher.locals[ countIndex ] = 1;
        ret                          = body.match( matcher, i, seq );
    }
    else if ( 0 < cmax )
    {
        matcher.locals[ countIndex ] = 1;
        ret                          = body.match( matcher, i, seq );

        if ( ret == false )
            ret = next.match( matcher, i, seq );
    }
    else
    {
        ret = next.match( matcher, i, seq );
    }

    matcher.locals[ countIndex ] = save;

    return ret;
}

bool study( Pattern.TreeInfo info )
{
    info.maxValid      = false;
    info.deterministic = false;

    return false;
}
}

/**
     * Handles the repetition count for a reluctant Curly. The matchInit
     * is called from the Prolog to save the index of where the group
     * beginning is stored. A zero length group check occurs in the
     * normal match but is skipped in the matchInit.
     */
static class LazyLoop extends Loop {
LazyLoop( int countIndex, int beginIndex )
{
    super( countIndex, beginIndex );
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    // Check for zero length group
    if ( i > matcher.locals[ beginIndex ] )
    {
        int count = matcher.locals[ countIndex ];

        if ( count < cmin )
        {
            matcher.locals[ countIndex ] = count + 1;
            bool result = body.match( matcher, i, seq );

            // If match failed we must backtrack, so
            // the loop count should NOT be incremented
            if ( !result )
                matcher.locals[ countIndex ] = count;

            return result;
        }

        if ( next.match( matcher, i, seq ) )
            return true;

        if ( count < cmax )
        {
            matcher.locals[ countIndex ] = count + 1;
            bool result = body.match( matcher, i, seq );

            // If match failed we must backtrack, so
            // the loop count should NOT be incremented
            if ( !result )
                matcher.locals[ countIndex ] = count;

            return result;
        }

        return false;
    }

    return next.match( matcher, i, seq );
}

bool matchInit( Matcher matcher, int i, CharSequence seq )
{
    int  save = matcher.locals[ countIndex ];
    bool ret  = false;

    if ( 0 < cmin )
    {
        matcher.locals[ countIndex ] = 1;
        ret                          = body.match( matcher, i, seq );
    }
    else if ( next.match( matcher, i, seq ) )
    {
        ret = true;
    }
    else if ( 0 < cmax )
    {
        matcher.locals[ countIndex ] = 1;
        ret                          = body.match( matcher, i, seq );
    }

    matcher.locals[ countIndex ] = save;

    return ret;
}

bool study( Pattern.TreeInfo info )
{
    info.maxValid      = false;
    info.deterministic = false;

    return false;
}
}

/**
     * Refers to a group in the regular expression. Attempts to match
     * whatever the group referred to last matched.
     */
static class BackRef extends Node {
int groupIndex;

BackRef( int groupCount )
{
    super();
    groupIndex = groupCount + groupCount;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    int j = matcher.groups[ groupIndex ];
    int k = matcher.groups[ groupIndex + 1 ];

    int groupSize = k - j;

    // If the referenced group didn't match, neither can this
    if ( j < 0 )
        return false;

    // If there isn't enough input left no match
    if ( i + groupSize > matcher.to )
    {
        matcher.hitEnd = true;

        return false;
    }

    // Check each new char to make sure it matches what the group
    // referenced matched last time around
    for ( int index = 0; index < groupSize; index++ )
        if ( seq.charAt( i + index ) != seq.charAt( j + index ) )
            return false;

    return next.match( matcher, i + groupSize, seq );
}

bool study( Pattern.TreeInfo info )
{
    info.maxValid = false;

    return next.study( info );
}
}

static class CIBackRef extends Node {
int  groupIndex;
bool doUnicodeCase;

CIBackRef( int groupCount, bool doUnicodeCase )
{
    super();
    groupIndex         = groupCount + groupCount;
    this.doUnicodeCase = doUnicodeCase;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    int j = matcher.groups[ groupIndex ];
    int k = matcher.groups[ groupIndex + 1 ];

    int groupSize = k - j;

    // If the referenced group didn't match, neither can this
    if ( j < 0 )
        return false;

    // If there isn't enough input left no match
    if ( i + groupSize > matcher.to )
    {
        matcher.hitEnd = true;

        return false;
    }

    // Check each new char to make sure it matches what the group
    // referenced matched last time around
    int x = i;

    for ( int index = 0; index < groupSize; index++ )
    {
        int c1 = Character.codePointAt( seq, x );
        int c2 = Character.codePointAt( seq, j );

        if ( c1 != c2 )
        {
            if ( doUnicodeCase )
            {
                int cc1 = Character.toUpperCase( c1 );
                int cc2 = Character.toUpperCase( c2 );

                if ( cc1 != cc2 && Character.toLowerCase( cc1 ) != Character.toLowerCase( cc2 ) )
                    return false;
            }
            else
            {
                if ( ASCII.toLower( c1 ) != ASCII.toLower( c2 ) )
                    return false;
            }
        }

        x += Character.charCount( c1 );
        j += Character.charCount( c2 );
    }

    return next.match( matcher, i + groupSize, seq );
}

bool study( Pattern.TreeInfo info )
{
    info.maxValid = false;

    return next.study( info );
}
}

/**
     * Searches until the next instance of its atom. This is useful for
     * finding the atom efficiently without passing an instance of it
     * (greedy problem) and without a lot of wasted search time (reluctant
     * problem).
     */
static class First extends Node {
Pattern.Node atom;

First( Pattern.Node node )
{
    this.atom = BnM.optimize( node );
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    if ( atom instanceof BnM) {
        return atom.match( matcher, i, seq )
               && next.match( matcher, matcher.last, seq );
    }

    for ( ;; )
    {
        if ( i > matcher.to )
        {
            matcher.hitEnd = true;

            return false;
        }

        if ( atom.match( matcher, i, seq ) )
        {
            return next.match( matcher, matcher.last, seq );
        }

        i += countChars( seq, i, 1 );
        matcher.first++;
    }
}

bool study( Pattern.TreeInfo info )
{
    atom.study( info );
    info.maxValid      = false;
    info.deterministic = false;

    return next.study( info );
}
}

static class Conditional extends Node {
Pattern.Node cond, yes, not;

Conditional( Pattern.Node cond, Pattern.Node yes, Pattern.Node not )
{
    this.cond = cond;
    this.yes  = yes;
    this.not  = not;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    if ( cond.match( matcher, i, seq ) )
    {
        return yes.match( matcher, i, seq );
    }
    else
    {
        return not.match( matcher, i, seq );
    }
}

bool study( Pattern.TreeInfo info )
{
    int  minL = info.minLength;
    int  maxL = info.maxLength;
    bool maxV = info.maxValid;
    info.reset();
    yes.study( info );

    int  minL2 = info.minLength;
    int  maxL2 = info.maxLength;
    bool maxV2 = info.maxValid;
    info.reset();
    not.study( info );

    info.minLength     = minL + Math.min( minL2, info.minLength );
    info.maxLength     = maxL + Math.max( maxL2, info.maxLength );
    info.maxValid      = ( maxV & maxV2 & info.maxValid );
    info.deterministic = false;

    return next.study( info );
}
}

/**
     * Zero width positive lookahead.
     */
static class Pos extends Node {
Pattern.Node cond;

Pos( Pattern.Node cond )
{
    this.cond = cond;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    int  savedTo          = matcher.to;
    bool conditionMatched = false;

    // Relax transparent region boundaries for lookahead
    if ( matcher.transparentBounds )
        matcher.to = matcher.getTextLength();

    try
    {
        conditionMatched = cond.match( matcher, i, seq );
    }
    finally
    {
        // Reinstate region boundaries
        matcher.to = savedTo;
    }

    return conditionMatched && next.match( matcher, i, seq );
}
}

/**
     * Zero width negative lookahead.
     */
static class Neg extends Node {
Pattern.Node cond;

Neg( Pattern.Node cond )
{
    this.cond = cond;
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    int  savedTo          = matcher.to;
    bool conditionMatched = false;

    // Relax transparent region boundaries for lookahead
    if ( matcher.transparentBounds )
        matcher.to = matcher.getTextLength();

    try
    {
        if ( i < matcher.to )
        {
            conditionMatched = !cond.match( matcher, i, seq );
        }
        else
        {
            // If a negative lookahead succeeds then more input
            // could cause it to fail!
            matcher.requireEnd = true;
            conditionMatched   = !cond.match( matcher, i, seq );
        }
    }
    finally
    {
        // Reinstate region boundaries
        matcher.to = savedTo;
    }

    return conditionMatched && next.match( matcher, i, seq );
}
}
/**
 * For use with lookbehinds; matches the position where the lookbehind
 * was encountered.
 */
static Pattern.Node lookbehindEnd = new Pattern.Node()
{
    bool match(Matcher matcher, int i, CharSequence seq) {
    return i == matcher.lookbehindTo;
}

};

/**
     * Zero width positive lookbehind.
     */
static class Behind

extends Pattern.Node {
    Pattern.Node cond;
    int          rmax, rmin;
    Behind( Node cond, int rmax, int rmin ) {
        this.cond = cond;
        this.rmax = rmax;
        this.rmin = rmin;
    }

    bool match( Matcher matcher, int i, CharSequence seq )
    {
        int  savedFrom        = matcher.from;
        bool conditionMatched = false;
        int  startIndex       = ( !matcher.transparentBounds ) ? matcher.from : 0;
        int  from             = Math.max( i - rmax, startIndex );
        // Set end boundary
        int savedLBT = matcher.lookbehindTo;
        matcher.lookbehindTo = i;

        // Relax transparent region boundaries for lookbehind
        if ( matcher.transparentBounds )
            matcher.from = 0;

        for ( int j = i - rmin; !conditionMatched && j >= from; j-- )
        {
            conditionMatched = cond.match( matcher, j, seq );
        }

        matcher.from         = savedFrom;
        matcher.lookbehindTo = savedLBT;

        return conditionMatched && next.match( matcher, i, seq );
    }
}

/**
     * Zero width positive lookbehind, including supplementary
     * characters or unpaired surrogates.
     */
static class BehindS

extends Behind {
    BehindS( Node cond, int rmax, int rmin ) {
        super( cond, rmax, rmin );
    }

    bool match( Matcher matcher, int i, CharSequence seq )
    {
        int  rmaxChars        = countChars( seq, i, -rmax );
        int  rminChars        = countChars( seq, i, -rmin );
        int  savedFrom        = matcher.from;
        int  startIndex       = ( !matcher.transparentBounds ) ? matcher.from : 0;
        bool conditionMatched = false;
        int  from             = Math.max( i - rmaxChars, startIndex );
        // Set end boundary
        int savedLBT = matcher.lookbehindTo;
        matcher.lookbehindTo = i;

        // Relax transparent region boundaries for lookbehind
        if ( matcher.transparentBounds )
            matcher.from = 0;

        for ( int j = i - rminChars;
              !conditionMatched && j >= from;
              j -= j > from ? countChars( seq, j, -1 ) : 1 )
        {
            conditionMatched = cond.match( matcher, j, seq );
        }

        matcher.from         = savedFrom;
        matcher.lookbehindTo = savedLBT;

        return conditionMatched && next.match( matcher, i, seq );
    }
}

/**
     * Zero width negative lookbehind.
     */
static class NotBehind

extends Pattern.Node {
    Pattern.Node cond;
    int          rmax, rmin;
    NotBehind( Node cond, int rmax, int rmin ) {
        this.cond = cond;
        this.rmax = rmax;
        this.rmin = rmin;
    }

    bool match( Matcher matcher, int i, CharSequence seq )
    {
        int  savedLBT         = matcher.lookbehindTo;
        int  savedFrom        = matcher.from;
        bool conditionMatched = false;
        int  startIndex       = ( !matcher.transparentBounds ) ? matcher.from : 0;
        int  from             = Math.max( i - rmax, startIndex );
        matcher.lookbehindTo = i;

        // Relax transparent region boundaries for lookbehind
        if ( matcher.transparentBounds )
            matcher.from = 0;

        for ( int j = i - rmin; !conditionMatched && j >= from; j-- )
        {
            conditionMatched = cond.match( matcher, j, seq );
        }

        // Reinstate region boundaries
        matcher.from         = savedFrom;
        matcher.lookbehindTo = savedLBT;

        return !conditionMatched && next.match( matcher, i, seq );
    }
}

/**
     * Zero width negative lookbehind, including supplementary
     * characters or unpaired surrogates.
     */
static class NotBehindS

extends NotBehind {
    NotBehindS( Node cond, int rmax, int rmin ) {
        super( cond, rmax, rmin );
    }

    bool match( Matcher matcher, int i, CharSequence seq )
    {
        int  rmaxChars        = countChars( seq, i, -rmax );
        int  rminChars        = countChars( seq, i, -rmin );
        int  savedFrom        = matcher.from;
        int  savedLBT         = matcher.lookbehindTo;
        bool conditionMatched = false;
        int  startIndex       = ( !matcher.transparentBounds ) ? matcher.from : 0;
        int  from             = Math.max( i - rmaxChars, startIndex );
        matcher.lookbehindTo = i;

        // Relax transparent region boundaries for lookbehind
        if ( matcher.transparentBounds )
            matcher.from = 0;

        for ( int j = i - rminChars;
              !conditionMatched && j >= from;
              j -= j > from ? countChars( seq, j, -1 ) : 1 )
        {
            conditionMatched = cond.match( matcher, j, seq );
        }

        //Reinstate region boundaries
        matcher.from         = savedFrom;
        matcher.lookbehindTo = savedLBT;

        return !conditionMatched && next.match( matcher, i, seq );
    }
}

/**
 * Returns the set union of two CharProperty nodes.
 */
private static Pattern.CharProperty union( Pattern.CharProperty lhs,
                                           Pattern.CharProperty rhs )
{
    return new Pattern.CharProperty()
    {
        bool isSatisfiedBy(int ch) {
        return lhs.isSatisfiedBy(ch) || rhs.isSatisfiedBy(ch);
    }};
}

/**
 * Returns the set intersection of two CharProperty nodes.
 */
private static Pattern.CharProperty intersection( Pattern.CharProperty lhs,
                                                  Pattern.CharProperty rhs )
{
    return new Pattern.CharProperty()
    {
        bool isSatisfiedBy(int ch) {
        return lhs.isSatisfiedBy(ch) && rhs.isSatisfiedBy(ch);
    }};
}

/**
 * Returns the set difference of two CharProperty nodes.
 */
private static Pattern.CharProperty setDifference( Pattern.CharProperty lhs,
                                                   Pattern.CharProperty rhs )
{
    return new Pattern.CharProperty()
    {
        bool isSatisfiedBy(int ch) {
        return ! rhs.isSatisfiedBy(ch) && lhs.isSatisfiedBy(ch);
    }};
}

/**
     * Handles word boundaries. Includes a field to allow this one class to
     * deal with the different types of word boundaries we can match. The word
     * characters include underscores, letters, and digits. Non spacing marks
     * can are also part of a word if they have a base character, otherwise
     * they are ignored for purposes of finding word boundaries.
     */
static class Bound

extends Pattern.Node {
static int LEFT  = 0x1;
static int RIGHT = 0x2;
static int BOTH  = 0x3;
static int NONE  = 0x4;
int        type;
bool       useUWORD;

Bound( int n, bool useUWORD )
{
    type          = n;
    this.useUWORD = useUWORD;
}

bool isWord( int ch )
{
    return useUWORD ? UnicodeProp.WORD.is( ch )
        : ( ch == '_' || Character.isLetterOrDigit( ch ) );
}

int check( Matcher matcher, int i, CharSequence seq )
{
    int  ch;
    bool left       = false;
    int  startIndex = matcher.from;
    int  endIndex   = matcher.to;

    if ( matcher.transparentBounds )
    {
        startIndex = 0;
        endIndex   = matcher.getTextLength();
    }

    if ( i > startIndex )
    {
        ch = Character.codePointBefore( seq, i );

        left = ( isWord( ch )
                 || ( ( Character.getType( ch ) == Character.NON_SPACING_MARK )
                      && hasBaseCharacter( matcher, i - 1, seq ) ) );
    }

    bool right = false;

    if ( i < endIndex )
    {
        ch = Character.codePointAt( seq, i );

        right = ( isWord( ch )
                  || ( ( Character.getType( ch ) == Character.NON_SPACING_MARK )
                       && hasBaseCharacter( matcher, i, seq ) ) );
    }
    else
    {
        // Tried to access char past the end
        matcher.hitEnd = true;
        // The addition of another char could wreck a boundary
        matcher.requireEnd = true;
    }

    return ( ( left ^ right ) ? ( right ? LEFT : RIGHT ) : NONE );
}

bool match( Matcher matcher, int i, CharSequence seq )
{
    return ( check( matcher, i, seq ) & type ) > 0
           && next.match( matcher, i, seq );
}
}

/**
 * Non spacing marks only count as word characters in bounds calculations
 * if they have a base character.
 */
private static bool hasBaseCharacter( Matcher matcher,
                                      int i,
                                      CharSequence seq )
{
    int start = ( !matcher.transparentBounds ) ? matcher.from : 0;

    for ( int x = i; x >= start; x-- )
    {
        int ch = Character.codePointAt( seq, x );

        if ( Character.isLetterOrDigit( ch ) )
            return true;

        if ( Character.getType( ch ) == Character.NON_SPACING_MARK )
            continue;

        return false;
    }

    return false;
}

/**
     * Attempts to match a slice in the input using the Boyer-Moore string
     * matching algorithm. The algorithm is based on the idea that the
     * pattern can be shifted farther ahead in the search text if it is
     * matched right to left.
     * <p>
     * The pattern is compared to the input one character at a time, from
     * the rightmost character in the pattern to the left. If the characters
     * all match the pattern has been found. If a character does not match,
     * the pattern is shifted right a distance that is the maximum of two
     * functions, the bad character shift and the good suffix shift. This
     * shift moves the attempted match position through the input more
     * quickly than a naive one position at a time check.
     * <p>
     * The bad character shift is based on the character from the text that
     * did not match. If the character does not appear in the pattern, the
     * pattern can be shifted completely beyond the bad character. If the
     * character does occur in the pattern, the pattern can be shifted to
     * line the pattern up with the next occurrence of that character.
     * <p>
     * The good suffix shift is based on the idea that some subset on the right
     * side of the pattern has matched. When a bad character is found, the
     * pattern can be shifted right by the pattern length if the subset does
     * not occur again in pattern, or by the amount of distance to the
     * next occurrence of the subset in the pattern.
     *
     * Boyer-Moore search methods adapted from code by Amy Yu.
     */
static class BnM : Pattern.Node
{
    int[] buffer;
    int[] lastOcc;
    int[] optoSft;

    /**
         * Pre calculates arrays needed to generate the bad character
         * shift and the good suffix shift. Only the last seven bits
         * are used to see if chars match; This keeps the tables small
         * and covers the heavily used ASCII range, but occasionally
         * results in an aliased match for the bad character shift.
         */
    static Pattern.Node optimize( Pattern.Node node )
    {
        if ( !( node instanceof Slice)) {
            return node;
        }

        int[] src           = ( ( Pattern.Slice )node ).buffer;
        int   patternLength = src.length;

        // The BM algorithm requires a bit of overhead;
        // If the pattern is short don't use it, since
        // a shift larger than the pattern length cannot
        // be used anyway.
        if ( patternLength < 4 )
        {
            return node;
        }

        int   i, j, k;
        int[] lastOcc = new int[ 128 ];
        int[] optoSft = new int[ patternLength ];

        // Precalculate part of the bad character shift
        // It is a table for where in the pattern each
        // lower 7-bit value occurs
        for ( i = 0; i < patternLength; i++ )
        {
            lastOcc[ src[ i ] & 0x7F ] = i + 1;
        }

        // Precalculate the good suffix shift
        // i is the shift amount being considered
        NEXT:

        for ( i = patternLength; i > 0; i-- )
        {
            // j is the beginning index of suffix being considered
            for ( j = patternLength - 1; j >= i; j-- )
            {
                // Testing for good suffix
                if ( src[ j ] == src[ j - i ] )
                {
                    // src[j..len] is a good suffix
                    optoSft[ j - 1 ] = i;
                }
                else
                {
                    // No match. The array has already been
                    // filled up with correct values before.
                    continue NEXT;
                }
            }

            // This fills up the remaining of optoSft
            // any suffix can not have larger shift amount
            // then its sub-suffix. Why???
            while ( j > 0 )
            {
                optoSft[ --j ] = i;
            }
        }

        // Set the guard value because of unicode compression
        optoSft[ patternLength - 1 ] = 1;
        if ( node instanceof SliceS)

        return new BnMS( src, lastOcc, optoSft, node.next );
        return new BnM( src, lastOcc, optoSft, node.next );
    }

    BnM( int[] src, int[] lastOcc, int[] optoSft, Pattern.Node next )
    {
        this.buffer  = src;
        this.lastOcc = lastOcc;
        this.optoSft = optoSft;
        this.next    = next;
    }

    bool match( Matcher matcher, int i, CharSequence seq )
    {
        int[] src           = buffer;
        int   patternLength = src.length;
        int   last          = matcher.to - patternLength;

        // Loop over all possible match positions in text
        NEXT:

        while ( i <= last )
        {
            // Loop over pattern from right to left
            for ( int j = patternLength - 1; j >= 0; j-- )
            {
                int ch = seq.charAt( i + j );

                if ( ch != src[ j ] )
                {
                    // Shift search to the right by the maximum of the
                    // bad character shift and the good suffix shift
                    i += Math.max( j + 1 - lastOcc[ ch & 0x7F ], optoSft[ j ] );

                    continue NEXT;
                }
            }

            // Entire pattern matched starting at i
            matcher.first = i;
            bool ret = next.match( matcher, i + patternLength, seq );

            if ( ret )
            {
                matcher.first       = i;
                matcher.groups[ 0 ] = matcher.first;
                matcher.groups[ 1 ] = matcher.last;

                return true;
            }

            i++;
        }

        // BnM is only used as the leading node in the unanchored case,
        // and it replaced its Start() which always searches to the end
        // if it doesn't find what it's looking for, so hitEnd is true.
        matcher.hitEnd = true;

        return false;
    }

    bool study( Pattern.TreeInfo info )
    {
        info.minLength += buffer.length;
        info.maxValid  =  false;

        return next.study( info );
    }
}

/**
     * Supplementary support version of BnM(). Unpaired surrogates are
     * also handled by this class.
     */
static class BnMS

extends BnM {
    int lengthInChars;

    BnMS( int[] src, int[] lastOcc, int[] optoSft, Node next ) {
        super( src, lastOcc, optoSft, next );

        for ( int x = 0; x < buffer.length; x++ )
        {
            lengthInChars += Character.charCount( buffer[ x ] );
        }
    }

    bool match( Matcher matcher, int i, CharSequence seq )
    {
        int[] src           = buffer;
        int   patternLength = src.length;
        int   last          = matcher.to - lengthInChars;

        // Loop over all possible match positions in text
        NEXT:

        while ( i <= last )
        {
            // Loop over pattern from right to left
            int ch;

            for ( int j = countChars( seq, i, patternLength ), x = patternLength - 1;
                  j > 0;
                  j -= Character.charCount( ch ), x-- )
            {
                ch = Character.codePointBefore( seq, i + j );

                if ( ch != src[ x ] )
                {
                    // Shift search to the right by the maximum of the
                    // bad character shift and the good suffix shift
                    int n = Math.max( x + 1 - lastOcc[ ch & 0x7F ], optoSft[ x ] );
                    i += countChars( seq, i, n );

                    continue NEXT;
                }
            }

            // Entire pattern matched starting at i
            matcher.first = i;
            bool ret = next.match( matcher, i + lengthInChars, seq );

            if ( ret )
            {
                matcher.first       = i;
                matcher.groups[ 0 ] = matcher.first;
                matcher.groups[ 1 ] = matcher.last;

                return true;
            }

            i += CountChars( seq, i, 1 );
        }

        matcher.hitEnd = true;

        return false;
    }
}

///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////

/**
 *  This must be the very first initializer.
 */
static Pattern.Node accept = new Pattern.Node();

static Pattern.Node lastAccept = new Pattern.LastNode();

private static class CharPropertyNames
{

    static Pattern.CharProperty charPropertyFor( String name )
    {
        CharPropertyFactory m = map.get( name );

        return m == null ? null : m.make();
    }

    private static abstract class CharPropertyFactory
    {
        abstract Pattern.CharProperty make();
    }

    private static void defCategory( String name,
                                     int typeMask )
    {
        map.put
            ( name, new CharPropertyFactory()
            {
                CharProperty make() { return new Category(typeMask);
            }});
    }

    private static void defRange( String name,
                                  int lower,
                                  int upper )
    {
        map.put
            ( name, new CharPropertyFactory()
            {
                CharProperty make() { return rangeFor(lower, upper);
            }});
    }

    private static void defCtype( String name,
                                  int ctype )
    {
        map.put
            ( name, new CharPropertyFactory()
            {
                CharProperty make() { return new Ctype(ctype);
            }});
    }

    private static abstract class CloneableProperty

    extends CharProperty implements Cloneable
    {
        public CloneableProperty clone() {
        try {
        return (CloneableProperty) super.clone();
    } catch (CloneNotSupportedException e) {
        throw new AssertionError( e );
    }
}

}

private static void defClone( String name,
                              CloneableProperty p )
{
    map.put
        ( name, new CharPropertyFactory()
        {
            CharProperty make() { return p.clone();
        }});
}

private static HashMap< String, CharPropertyFactory > map
    = new HashMap<>();

static {
    // Unicode character property aliases, defined in
    // http://www.unicode.org/Public/UNIDATA/PropertyValueAliases.txt
    defCategory( "Cn", 1 << Character.UNASSIGNED );
    defCategory( "Lu", 1 << Character.UPPERCASE_LETTER );
    defCategory( "Ll", 1 << Character.LOWERCASE_LETTER );
    defCategory( "Lt", 1 << Character.TITLECASE_LETTER );
    defCategory( "Lm", 1 << Character.MODIFIER_LETTER );
    defCategory( "Lo", 1 << Character.OTHER_LETTER );
    defCategory( "Mn", 1 << Character.NON_SPACING_MARK );
    defCategory( "Me", 1 << Character.ENCLOSING_MARK );
    defCategory( "Mc", 1 << Character.COMBINING_SPACING_MARK );
    defCategory( "Nd", 1 << Character.DECIMAL_DIGIT_NUMBER );
    defCategory( "Nl", 1 << Character.LETTER_NUMBER );
    defCategory( "No", 1 << Character.OTHER_NUMBER );
    defCategory( "Zs", 1 << Character.SPACE_SEPARATOR );
    defCategory( "Zl", 1 << Character.LINE_SEPARATOR );
    defCategory( "Zp", 1 << Character.PARAGRAPH_SEPARATOR );
    defCategory( "Cc", 1 << Character.CONTROL );
    defCategory( "Cf", 1 << Character.FORMAT );
    defCategory( "Co", 1 << Character.PRIVATE_USE );
    defCategory( "Cs", 1 << Character.SURROGATE );
    defCategory( "Pd", 1 << Character.DASH_PUNCTUATION );
    defCategory( "Ps", 1 << Character.START_PUNCTUATION );
    defCategory( "Pe", 1 << Character.END_PUNCTUATION );
    defCategory( "Pc", 1 << Character.CONNECTOR_PUNCTUATION );
    defCategory( "Po", 1 << Character.OTHER_PUNCTUATION );
    defCategory( "Sm", 1 << Character.MATH_SYMBOL );
    defCategory( "Sc", 1 << Character.CURRENCY_SYMBOL );
    defCategory( "Sk", 1 << Character.MODIFIER_SYMBOL );
    defCategory( "So", 1 << Character.OTHER_SYMBOL );
    defCategory( "Pi", 1 << Character.INITIAL_QUOTE_PUNCTUATION );
    defCategory( "Pf", 1 << Character.FINAL_QUOTE_PUNCTUATION );

    defCategory
        (
         "L",
         ( ( 1 << Character.UPPERCASE_LETTER )
           | ( 1 << Character.LOWERCASE_LETTER )
           | ( 1 << Character.TITLECASE_LETTER )
           | ( 1 << Character.MODIFIER_LETTER )
           | ( 1 << Character.OTHER_LETTER ) )
        );

    defCategory
        (
         "M",
         ( ( 1 << Character.NON_SPACING_MARK )
           | ( 1 << Character.ENCLOSING_MARK )
           | ( 1 << Character.COMBINING_SPACING_MARK ) )
        );

    defCategory
        (
         "N",
         ( ( 1 << Character.DECIMAL_DIGIT_NUMBER )
           | ( 1 << Character.LETTER_NUMBER )
           | ( 1 << Character.OTHER_NUMBER ) )
        );

    defCategory
        (
         "Z",
         ( ( 1 << Character.SPACE_SEPARATOR )
           | ( 1 << Character.LINE_SEPARATOR )
           | ( 1 << Character.PARAGRAPH_SEPARATOR ) )
        );

    defCategory
        (
         "C",
         ( ( 1 << Character.CONTROL )
           | ( 1 << Character.FORMAT )
           | ( 1 << Character.PRIVATE_USE )
           | ( 1 << Character.SURROGATE ) )
        ); // Other

    defCategory
        (
         "P",
         ( ( 1 << Character.DASH_PUNCTUATION )
           | ( 1 << Character.START_PUNCTUATION )
           | ( 1 << Character.END_PUNCTUATION )
           | ( 1 << Character.CONNECTOR_PUNCTUATION )
           | ( 1 << Character.OTHER_PUNCTUATION )
           | ( 1 << Character.INITIAL_QUOTE_PUNCTUATION )
           | ( 1 << Character.FINAL_QUOTE_PUNCTUATION ) )
        );

    defCategory
        (
         "S",
         ( ( 1 << Character.MATH_SYMBOL )
           | ( 1 << Character.CURRENCY_SYMBOL )
           | ( 1 << Character.MODIFIER_SYMBOL )
           | ( 1 << Character.OTHER_SYMBOL ) )
        );

    defCategory
        (
         "LC",
         ( ( 1 << Character.UPPERCASE_LETTER )
           | ( 1 << Character.LOWERCASE_LETTER )
           | ( 1 << Character.TITLECASE_LETTER ) )
        );

    defCategory
        (
         "LD",
         ( ( 1 << Character.UPPERCASE_LETTER )
           | ( 1 << Character.LOWERCASE_LETTER )
           | ( 1 << Character.TITLECASE_LETTER )
           | ( 1 << Character.MODIFIER_LETTER )
           | ( 1 << Character.OTHER_LETTER )
           | ( 1 << Character.DECIMAL_DIGIT_NUMBER ) )
        );

    defRange( "L1", 0x00, 0xFF ); // Latin-1

    map.put
        ( "all", new CharPropertyFactory()
        {
            CharProperty make() { return new All();
        }});

    // Posix regular expression character classes, defined in
    // http://www.unix.org/onlinepubs/009695399/basedefs/xbd_chap09.html
    defRange( "ASCII", 0x00, 0x7F );    // ASCII
    defCtype( "Alnum", ASCII.ALNUM );   // Alphanumeric characters
    defCtype( "Alpha", ASCII.ALPHA );   // Alphabetic characters
    defCtype( "Blank", ASCII.BLANK );   // Space and tab characters
    defCtype( "Cntrl", ASCII.CNTRL );   // Control characters
    defRange( "Digit", '0', '9' );      // Numeric characters
    defCtype( "Graph", ASCII.GRAPH );   // printable and visible
    defRange( "Lower", 'a', 'z' );      // Lower-case alphabetic
    defRange( "Print", 0x20, 0x7E );    // Printable characters
    defCtype( "Punct", ASCII.PUNCT );   // Punctuation characters
    defCtype( "Space", ASCII.SPACE );   // Space characters
    defRange( "Upper", 'A', 'Z' );      // Upper-case alphabetic
    defCtype( "XDigit", ASCII.XDIGIT ); // hexadecimal digits

    // Java character properties, defined by methods in Character.java
    defClone
        ( "javaLowerCase", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isLowerCase(ch);
        }});

    defClone
        ( "javaUpperCase", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isUpperCase(ch);
        }});

    defClone
        ( "javaAlphabetic", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isAlphabetic(ch);
        }});

    defClone
        ( "javaIdeographic", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isIdeographic(ch);
        }});

    defClone
        ( "javaTitleCase", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isTitleCase(ch);
        }});

    defClone
        ( "javaDigit", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isDigit(ch);
        }});

    defClone
        ( "javaDefined", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isDefined(ch);
        }});

    defClone
        ( "javaLetter", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isLetter(ch);
        }});

    defClone
        ( "javaLetterOrDigit", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isLetterOrDigit(ch);
        }});

    defClone
        ( "javaJavaIdentifierStart", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isJavaIdentifierStart(ch);
        }});

    defClone
        ( "javaJavaIdentifierPart", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isJavaIdentifierPart(ch);
        }});

    defClone
        ( "javaUnicodeIdentifierStart", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isUnicodeIdentifierStart(ch);
        }});

    defClone
        ( "javaUnicodeIdentifierPart", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isUnicodeIdentifierPart(ch);
        }});

    defClone
        ( "javaIdentifierIgnorable", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isIdentifierIgnorable(ch);
        }});

    defClone
        ( "javaSpaceChar", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isSpaceChar(ch);
        }});

    defClone
        ( "javaWhitespace", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isWhitespace(ch);
        }});

    defClone
        ( "javaISOControl", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isISOControl(ch);
        }});

    defClone
        ( "javaMirrored", new CloneableProperty()
        {
            bool isSatisfiedBy(int ch) {
            return Character.isMirrored(ch);
        }});
}
}

/**
 * Creates a predicate which can be used to match a string.
 *
 * @return  The predicate which can be used for matching on a string
 * @since   1.8
 */
public Predicate< String > asPredicate()
{
    return s -> matcher( s ).find();
}

/**
 * Creates a stream from the given input sequence around matches of this
 * pattern.
 *
 * <p> The stream returned by this method contains each substring of the
 * input sequence that is terminated by another subsequence that matches
 * this pattern or is terminated by the end of the input sequence.  The
 * substrings in the stream are in the order in which they occur in the
 * input. Trailing empty strings will be discarded and not encountered in
 * the stream.
 *
 * <p> If this pattern does not match any subsequence of the input then
 * the resulting stream has just one element, namely the input sequence in
 * string form.
 *
 * <p> When there is a positive-width match at the beginning of the input
 * sequence then an empty leading substring is included at the beginning
 * of the stream. A zero-width match at the beginning however never produces
 * such empty leading substring.
 *
 * <p> If the input sequence is mutable, it must remain constant during the
 * execution of the terminal stream operation.  Otherwise, the result of the
 * terminal stream operation is undefined.
 *
 * @param   input
 *          The character sequence to be split
 *
 * @return  The stream of strings computed by splitting the input
 *          around matches of this pattern
 * @see     #split(CharSequence)
 * @since   1.8
 */
public Stream< String > splitAsStream( CharSequence input )
{
    class MatcherIterator implements Iterator < String > {
private Matcher matcher;
// The start position of the next sub-sequence of input
// when current == input.length there are no more elements
private int current;
// null if the next element, if any, needs to obtained
private String nextElement;
// > 0 if there are N next empty elements
private int emptyElementCount;

MatcherIterator() {
    this.matcher = matcher( input );
}

public String next()
{
    if ( !hasNext() )
        throw new NoSuchElementException();

    if ( emptyElementCount == 0 )
    {
        String n = nextElement;
        nextElement = null;

        return n;
    }
    else
    {
        emptyElementCount--;

        return "";
    }
}

public bool hasNext()
{
    if ( nextElement != null || emptyElementCount > 0 ) return true;

    if ( current == input.length() ) return false;

    // Consume the next matching element
    // Count sequence of matching empty elements
    while ( matcher.find() )
    {
        nextElement = input.subSequence( current, matcher.start() ).toString();
        current     = matcher.end();

        if ( !nextElement.isEmpty() )
        {
            return true;
        }

        if ( current > 0 )
        {
            // no empty leading substring for zero-width
            // match at the beginning of the input
            emptyElementCount++;
        }
    }

    // Consume last matching element
    nextElement = input.subSequence( current, input.length() ).toString();
    current     = input.length();

    if ( !nextElement.isEmpty() )
    {
        return true;
    }

    // Ignore a terminal sequence of matching empty elements
    emptyElementCount = 0;
    nextElement       = null;

    return false;
}
}
return StreamSupport.stream(Spliterators.spliteratorUnknownSize(
new MatcherIterator(), Spliterator.ORDERED | Spliterator.NONNULL), false);
}
}