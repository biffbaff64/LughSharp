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

namespace LibGDXSharp.Utils.Regex;

/// <inheritdoc/>
[Obsolete]
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

    [Obsolete]
    public Pattern( string regex, int flags )
    {
//        this._pattern = regex;
//        this.Flags    = flags;

        // to use Unicode_Case if Unicode_Character_Class present
//        if ( ( flags & Unicode_Character_Class ) != 0 )
//        {
//            this.Flags |= Unicode_Case;
//        }

        // Reset group index count
//        CapturingGroupCount = 1;
//        LocalCount          = 0;

//        if ( _pattern.Length > 0 )
//        {
//            try
//            {
//                Compile();
//            }
//            catch ( StackOverflowException soe )
//            {
//                throw error( "Stack overflow during pattern compilation" );
//            }
//        }
//        else
//        {
//            root      = new Start( _lastAccept );
//            matchRoot = _lastAccept;
//        }
    }

    [Obsolete]
    public Pattern Compile( string regex )
    {
        return new Pattern( regex, 0 );
    }

    [Obsolete]
    public Pattern Compile( string regex, int flags )
    {
        return new Pattern( regex, flags );
    }

    [Obsolete]
    public string? GetPattern() => _pattern;

    [Obsolete]
    public new string ToString()
    {
        return _pattern ?? "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Obsolete]
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

    [Obsolete]
    public bool Matches( string regex, string input )
    {
        Pattern p = Compile( regex );
        Matcher m = p.Matcher( input );

        return m.Matches();
    }

    [Obsolete]
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

//        return matchList.subList( 0, resultSize ).toArray( result );
        return default!;
    }

    [Obsolete]
    public string[] Split( string input )
    {
        return Split( input, 0 );
    }

    [Obsolete]
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

    [Obsolete]
    private void ReadObject( StreamReader s )
    {
        // Read in all fields
//        s.DefaultReadObject();

        // Initialize counts
//        CapturingGroupCount = 1;
//        LocalCount          = 0;

        // if length > 0, the Pattern is lazily compiled
//        lock ( this )
//        {
//            _compiled = false;
//        }

//        if ( _pattern?.Length == 0 )
//        {
//            root      = new Start( _lastAccept );
//            matchRoot = _lastAccept;
//            _compiled = true;
//        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Obsolete]
    private void Normalize()
    {
//        var inCharClass   = false;
//        var lastCodePoint = -1;
//
//        // Convert pattern into normalizedD form
//        _normalizedPattern = Normalizer.normalize( _pattern, Normalizer.Form.NFD );
//        _patternLength     = _normalizedPattern.Length;
//
//        // Modify pattern to match canonical equivalences
//        var newPattern = new StringBuilder( _patternLength );
//
//        for ( var i = 0; i < _patternLength; )
//        {
//            int c = CharHelper.CodePointAt( _normalizedPattern, i );
//            
//            if ((char.GetUnicodeCategory( (char) c ) == UnicodeCategory.NonSpacingMark)
//                 && ( lastCodePoint != -1 ) )
//            {
//                var sequenceBuffer = new StringBuilder();
//
//                sequenceBuffer.AppendCodePoint( lastCodePoint );
//                sequenceBuffer.AppendCodePoint( c );
//
//                while ( char.GetUnicodeCategory( ( char )c ) == UnicodeCategory.NonSpacingMark )
//                {
//                    i += CharHelper.CharCount( c );
//
//                    if ( i >= _patternLength ) break;
//
//                    c = CharHelper.CodePointAt( _normalizedPattern, i );
//
//                    sequenceBuffer.AppendCodePoint( c );
//                }
//
//                var ea = ProduceEquivalentAlternation( sequenceBuffer.ToString() );
//
//                newPattern.Length -= CharHelper.CharCount( lastCodePoint );
//                newPattern.Append( "(?:" ).Append( ea ).Append( ')' );
//            }
//            else if ( ( c == '[' ) && ( lastCodePoint != '\\' ) )
//            {
//                i = NormalizeCharClass( newPattern, i );
//            }
//            else
//            {
//                newPattern.AppendCodePoint( c );
//            }
//
//            lastCodePoint = c;
//
//            i += CharHelper.CharCount( c );
//        }
//
//        _normalizedPattern = newPattern.ToString();
    }

    [Obsolete]
    private int NormalizeCharClass( StringBuilder newPattern, int i )
    {
        StringBuilder charClass     = new();
        StringBuilder eq            = null;
        var           lastCodePoint = -1;
        string        result;

        i++;

        if ( i == _normalizedPattern.Length )
            throw new PatternSyntaxException( "Unclosed character class", _normalizedPattern, 1 );
        
        charClass.Append( '[' );

        while ( true )
        {
            var c = CharHelper.CodePointAt( _normalizedPattern, i );

            if ( ( c == ']' ) && ( lastCodePoint != '\\' ) )
            {
                charClass.Append( ( char )c );

                break;
            }

            if ( char.GetUnicodeCategory( (char)c ) == UnicodeCategory.NonSpacingMark )
            {
                var sequenceBuffer = new StringBuilder();
//                sequenceBuffer.AppendCodePoint( lastCodePoint );

                while ( char.GetUnicodeCategory( (char)c ) == UnicodeCategory.NonSpacingMark )
                {
//                    sequenceBuffer.AppendCodePoint( c );
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
//                charClass.AppendCodePoint( c );
                i++;
            }

//            if ( i == _normalizedPattern.Length ) throw Error( "Unclosed character class" );

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

    [Obsolete]
    private string ProduceEquivalentAlternation( string source )
    {
//        int len = CountChars( source, 0, 1 );

//        if ( source.Length == len ) return source; // source has one character.

//        var baseStr        = source.Substring( 0, len );
//        var combiningMarks = source.Substring( len );

//        string[]      perms  = ProducePermutations( combiningMarks );
//        StringBuilder result = new( source );

        // Add combined permutations
//        for ( var x = 0; x < perms.Length; x++ )
//        {
//            var next = baseStr + perms[ x ];

//            if ( x > 0 ) result.Append( "|" + next );

//            next = ComposeOneStep( next );

//            if ( next != null )
//            {
//                result.Append( "|" + ProduceEquivalentAlternation( next ) );
//            }
//        }

//        return result.ToString();
        return "";
    }

    [Obsolete]
    private string[] ProducePermutations( string input )
    {
//        if ( input.Length == CountChars( input, 0, 1 ) )
//        {
//            return new string[] { input };
//        }

//        if ( input.Length == CountChars( input, 0, 2 ) )
//        {
//            int c0 = CharHelper.CodePointAt( input, 0 );
//            int c1 = CharHelper.CodePointAt( input, CharHelper.CharCount( c0 ) );

//            if ( getClass( c1 ) == getClass( c0 ) )
//            {
//                return new string[] { input };
//            }

//            string[] str = new string[ 2 ];
            
//            str[ 0 ] = input;
            
//            StringBuilder sb = new StringBuilder( 2 );
            
//            sb.appendCodePoint( c1 );
//            sb.appendCodePoint( c0 );
            
//            str[ 1 ] = sb.ToString();

//            return str;
//    }

//        int length      = 1;
//        int nCodePoints = countCodePoints( input );

//        for ( int x = 1; x < nCodePoints; x++ )
//        {
//            length = length * ( x + 1 );
//        }

//        string[] temp = new string[ length ];

//        int combClass[] = new int[ nCodePoints ];

//        for ( int x = 0, i = 0; x < nCodePoints; x++ )
//        {
//            int c = CharHelper.CodePointAt( input, i );
//            combClass[ x ] =  getClass( c );
//            i              += CharHelper.charCount( c );
//        }

        // For each char, take it out and add the permutations
        // of the remaining chars
//        int index = 0;
//        int len;

        // offset maintains the index in code units.
//        loop:

//        for ( int x = 0, offset = 0; x < nCodePoints; x++, offset += len )
//        {
//            len = CountChars( input, offset, 1 );
//            bool skip = false;

//            for ( int y = x - 1; y >= 0; y-- )
//            {
//                if ( combClass[ y ] == combClass[ x ] )
//                {
//                    goto loop;
//                }
//            }

//            StringBuilder sb         = new StringBuilder( input );
//            string        otherChars = sb.Delete( offset, offset + len ).toString();
//            string[]      subResult  = ProducePermutations( otherChars );

//            string prefix = input.Substring( offset, offset + len );

//            foreach ( var t in subResult )
//            {
//                _temp[ index++ ] = prefix + t;
//            }
//        }

//        string[] result = new string[ index ];

//        for ( int x = 0; x < index; x++ )
//        {
//            result[ x ] = _temp[ x ];
//        }

//        return result;
        return default!;
    }

    [Obsolete]
    private string? ComposeOneStep( string input )
    {
//        int    len                = CountChars( input, 0, 2 );
//        string firstTwoCharacters = input.Substring( 0, len );
//        string result             = Normalizer.normalize( firstTwoCharacters, Normalizer.Form.NFC );

//        if ( result.Equals( firstTwoCharacters ) )
//        {
//            return null;
//        }

//        string remainder = input.Substring( len );

//        return result + remainder;
        return "";
    }

    /// <summary>
    /// Preprocess any \Q...\E sequences in `temp', meta-quoting them.
    /// See the description of `quotemeta' in perlfunc(1).
    /// </summary>
    [Obsolete]
    private void RemoveQEQuoting()
    {
        int pLen = _patternLength;
        int i    = 0;

        while ( i < ( pLen - 1 ) )
        {
            if ( _temp[ i ] != '\\' )
            {
                i += 1;
            }
            else if ( _temp[ i + 1 ] != 'Q' )
            {
                i += 2;
            }
            else
            {
                break;
            }
        }

        if ( i >= ( pLen - 1 ) ) // No \Q sequence found
        {
            return;
        }

        int j = i;
        
        i += 2;
        
        int[] newtemp = new int[ j + ( 3 * ( pLen - i ) ) + 2 ];

        Array.Copy( _temp, 0, newtemp, 0, j );

        bool inQuote    = true;
        bool beginQuote = true;

//        while ( i < pLen )
//        {
//            int c = _temp[ i++ ];

//            if ( !ASCII.isAscii( c ) || ASCII.isAlpha( c ) )
//            {
//                newtemp[ j++ ] = c;
//            }
//            else if ( ASCII.isDigit( c ) )
//            {
//                if ( beginQuote )
//                {
                    /*
                     * A unicode escape \[0xu] could be before this quote,
                     * and we don't want this numeric char to processed as
                     * part of the escape.
                     */
//                    newtemp[ j++ ] = '\\';
//                    newtemp[ j++ ] = 'x';
//                    newtemp[ j++ ] = '3';
//                }

//                newtemp[ j++ ] = c;
//            }
//            else if ( c != '\\' )
//            {
//                if ( inQuote ) newtemp[ j++ ] = '\\';
//                newtemp[ j++ ] = c;
//            }
//            else if ( inQuote )
//            {
//                if ( _temp[ i ] == 'E' )
//                {
//                    i++;
//                    inQuote = false;
//                }
//                else
//                {
//                    newtemp[ j++ ] = '\\';
//                    newtemp[ j++ ] = '\\';
//                }
//            }
//            else
//            {
//                if ( _temp[ i ] == 'Q' )
//                {
//                    i++;
//                    inQuote    = true;
//                    beginQuote = true;

//                    continue;
//                }

//                newtemp[ j++ ] = c;

//                if ( i != pLen )
//                {
//                    newtemp[ j++ ] = _temp[ i++ ];
//                }
//            }

//            beginQuote = false;
//        }

//        _patternLength = j;
        
//        Array.Copy( newtemp, _temp, j + 2 ); // double zero termination
    }

    [Obsolete]
    public void Compile()
    {
    }

    [Obsolete]
    public Dictionary< string, int > NamedGroups()
    {
        return _namedGroups;
    }

    /**
     * Used to print out a subtree of the Pattern to help with debugging.
     */
    [Obsolete]
    private static void PrintObjectTree( Node node )
    {
        while ( node != null )
        {
            if ( node is Prolog )
            {
                Console.WriteLine(node);
                
//                printObjectTree( ( ( Prolog )node ).loop );
                
                Console.WriteLine( "**** end contents prolog loop" );
            }

            else if ( node is Loop )

            {
                Console.WriteLine( node );
//                printObjectTree( ( ( Loop )node ).body );
                Console.WriteLine( "**** end contents Loop body" );
            }

            else if ( node is Curly)

            {
                Console.WriteLine( node );
//                printObjectTree( ( ( Curly )node ).atom );
                Console.WriteLine( "**** end contents Curly body" );
            }

            else if ( node is GroupCurly)

            {
                Console.WriteLine( node );
//                printObjectTree( ( ( GroupCurly )node ).atom );
                Console.WriteLine( "**** end contents GroupCurly body" );
            }

            else if ( node is GroupTail)

            {
                Console.WriteLine( node );
                Console.WriteLine( "Tail next is " + node.next );

                return;
            }

            else

            {
                Console.WriteLine( node );
            }

            node = node.next;

            if ( node != null )
            {
                Console.WriteLine( "->next:" );
            }

//            if ( node == Pattern.accept )
//            {
//                Console.WriteLine( "Accept Node" );
//                node = null;
//            }
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    /**
     * Internal method used for handling all syntax errors. The pattern is
     * displayed with a pointer to aid in locating the syntax error.
     */
    [Obsolete]
    private PatternSyntaxException error( string s )
    {
//        return new PatternSyntaxException( s, normalizedPattern, _cursor - 1 );
        return default!;
    }

    /**
     * Determines if there is any supplementary character or unpaired
     * surrogate in the specified range.
     */
    [Obsolete]
    private bool findSupplementary( int start, int end )
    {
        for ( int i = start; i < end; i++ )
        {
            if ( isSupplementary( _temp[ i ] ) )
                return true;
        }

        return false;
    }

    /**
     * Determines if the specified code point is a supplementary
     * character or unpaired surrogate.
     */
    [Obsolete]
    private static bool isSupplementary( int ch )
    {
//        return ( ch >= Character.MIN_SUPPLEMENTARY_CODE_POINT ) || Character.isSurrogate( ( char )ch );
        return default!;
    }

    /*
     *  The following methods handle the main parsing. They are sorted
     *  according to their precedence order, the lowest one first.
     */

    /**
     * The expression is parsed with branch nodes added for alternations.
     * This may be called recursively to parse sub expressions that may
     * contain alternations.
     */
    [Obsolete]
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
//                    branchConn.next = end;
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
//                    nodeTail.next = branchConn;
                }

                if ( prev == branch )
                {
//                    branch.add( node );
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
//                        firstTail.next = branchConn;
                    }

//                    prev = branch = new Branch( prev, node, branchConn );
                }
            }

            if ( Peek() != '|' )
            {
                return prev;
            }

            Next();
        }
    }

    /**
     * Parsing of sequences between alternations.
     */
    [Obsolete]
    private Node sequence( Node end )
    {
        Node? head = null;
        Node? tail = null;

        LOOP:

        for ( ;; )
        {
            Node? node;
            int ch = Peek();

            switch ( ch )
            {
                case '(':
                    // Because group handles its own closure,
                    // we need to treat it differently
                    node = group0();

                    // Check for comment or flag group
                    if ( node == null ) continue;

                    if ( head == null )
                    {
                        head = node;
                    }
                    else
                    {
//                        tail.next = node;
                    }

                    // Double return: Tail was returned in root
                    tail = root;

                    continue;

                case '[':
                    node = clazz( true );

                    break;

                case '\\':
//                    ch = nextEscaped();

                    if ( ( ch == 'p' ) || ( ch == 'P' ) )
                    {
                        bool oneLetter = true;
                        bool comp      = ( ch == 'P' );
                        ch = Next(); // Consume { if present

                        if ( ch != '{' )
                        {
//                            unread();
                        }
                        else
                        {
                            oneLetter = false;
                        }

                        node = family( oneLetter, comp );
                    }
                    else
                    {
//                        unread();
                        node = atom();
                    }

                    break;

                case '^':
                    Next();

//                    if ( Has( MULTILINE ) )
//                    {
//                        if ( Has( UNIX_LINES ) )
//                        {
//                            node = new UnixCaret();
//                        }
//                        else
//                        {
//                            node = new Caret();
//                        }
//                    }
//                    else
//                    {
//                        node = new Begin();
//                    }

                    break;

                case '$':
                    Next();

//                    if ( Has( Unix_Lines ) )
//                    {
//                        node = new UnixDollar( Has( Multiline ) );
//                    }
//                    else
//                    {
//                        node = new Dollar( Has( Multiline ) );
//                    }

                    break;

                case '.':
                    Next();

//                    if ( Has( DOTALL ) )
//                    {
//                        node = new All();
//                    }
//                    else
//                    {
//                        if ( Has( UNIX_LINES ) )
//                        {
//                            node = new UnixDot();
//                        }
//                        else
//                        {
//                            node = new Dot();
//                        }
//                    }

                    break;

                case '|':
                case ')':
                    goto LOOP;

                case ']': // Now interpreting dangling ] and } as literals
                case '}':
                    node = atom();

                    break;

                case '?':
                case '*':
                case '+':
                    Next();

                    throw error( "Dangling meta character '" + ( ( char )ch ) + "'" );

                case 0:
                    if ( _cursor >= _patternLength ) goto LOOP;

                    break;
                // Fall through
                default:
                    node = atom();

                    break;
            }

//            node = closure( node );

//            if ( head == null )
//            {
//                head = tail = node;
//            }
//            else
//            {
//                tail.next = node;
//                tail      = node;
//            }
        }

        if ( head == null )
        {
            return end;
        }

//        tail.next = end;
        root      = tail; //double return

        return head;
    }

    /**
     * Parse and add a new Single or Slice.
     */
    [Obsolete]
    private Node atom()
    {
        int  first            = 0;
        int  prev             = -1;
        bool hasSupplementary = false;
        int  ch               = Peek();

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
                        _cursor = prev; // Unwind one character
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
//                    ch = nextEscaped();

//                    if ( ( ch == 'p' ) || ( ch == 'P' ) )
//                    {
                        // Property
//                        if ( first > 0 )
//                        {
                            // Slice is waiting; handle it first
//                            unread();

//                            break;
//                        }
//                        else
//                        {
                            // No slice; just return the family node
//                            bool comp      = ( ch == 'P' );
//                            bool oneLetter = true;
//                            ch = Next(); // Consume { if present

//                            if ( ch != '{' )
//                                unread();
//                            else
//                                oneLetter = false;

//                            return family( oneLetter, comp );
//                        }
//                    }

//                    unread();
//                    prev = _cursor;
//                    ch   = escape( false, first == 0, false );

//                    if ( ch >= 0 )
//                    {
//                        append( ch, first );
//                        first++;

//                        if ( isSupplementary( ch ) )
//                        {
//                            hasSupplementary = true;
//                        }

//                        ch = Peek();

//                        continue;
//                    }
//                    else if ( first == 0 )
//                    {
//                        return root;
//                    }

                    // Unwind meta escape sequence
                    _cursor = prev;

                    break;

                case 0:
                    if ( _cursor >= _patternLength )
                    {
                        break;
                    }
                    break;

                // Fall through
                default:
                    prev = _cursor;
                    append( ch, first );
                    first++;

                    if ( isSupplementary( ch ) )
                    {
                        hasSupplementary = true;
                    }

                    ch = Next();

                    continue;
            }

            break;
        }

//        if ( first == 1 )
//        {
//            return newSingle( buffer[ 0 ] );
//        }
//        else
//        {
//            return newSlice( buffer, first, hasSupplementary );
//        }

        return default!;
    }

    [Obsolete]
    private void append( int ch, int len )
    {
//        if ( len >= buffer.length )
//        {
//            int[] tmp = new int[ len + len ];
//            System.arraycopy( buffer, 0, tmp, 0, len );
//            buffer = tmp;
//        }

//        buffer[ len ] = ch;
    }

    /**
     * Parses a backref greedily, taking as many numbers as it
     * can. The first digit is always treated as a backref, but
     * multi digit numbers are only treated as a backref if at
     * least that many backrefs exist at this point in the regex.
     */
    [Obsolete]
    private Node backref( int refNum )
    {
        bool done = false;

        while ( !done )
        {
            int ch = Peek();

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
//                    if ( ( capturingGroupCount - 1 ) < newRefNum )
//                    {
//                        done = true;

//                        break;
//                    }

//                    refNum = newRefNum;
//                    read();

                    break;

                default:
                    done = true;

                    break;
            }
        }

//        if ( Has( CASE_INSENSITIVE ) )
//            return new CIBackRef( refNum, Has( UNICODE_CASE ) );
//        else
//            return new BackRef( refNum );
        return default!;
    }

    /**
     * Parses an escape sequence to determine the actual value that needs
     * to be matched.
     * If -1 is returned and create was true a new object was added to the tree
     * to handle the escape sequence.
     * If the returned value is greater than zero, it is the value that
     * matches the escape sequence.
     */
    [Obsolete]
    private int escape( bool inclass, bool create, bool isrange )
    {
//        int ch = skip();

//        switch ( ch )
//        {
//            case '0':
//                return o();

//            case '1':
//            case '2':
//            case '3':
//            case '4':
//            case '5':
//            case '6':
//            case '7':
//            case '8':
//            case '9':
//                if ( inclass ) break;

//                if ( create )
//                {
//                    root = backref( ( ch - '0' ) );
//                }

//                return -1;

//            case 'A':
//                if ( inclass ) break;
//                if ( create ) root = new Begin();

//                return -1;

//            case 'B':
//                if ( inclass ) break;
//                if ( create ) root = new Bound( Bound.NONE, Has( UNICODE_CHARACTER_CLASS ) );

//                return -1;

//            case 'C':
//                break;

//            case 'D':
//                if ( create )
//                    root = Has( UNICODE_CHARACTER_CLASS )
//                        ? new Utype( UnicodeProp.DIGIT ).complement()
//                        : new Ctype( ASCII.DIGIT ).complement();

//                return -1;

//            case 'E':
//            case 'F':
//                break;

//            case 'G':
//                if ( inclass ) break;
//                if ( create ) root = new LastMatch();

//                return -1;

//            case 'H':
//                if ( create ) root = new HorizWS().complement();

//                return -1;

//            case 'I':
//            case 'J':
//            case 'K':
//            case 'L':
//            case 'M':
//            case 'N':
//            case 'O':
//            case 'P':
//            case 'Q':
//                break;

//            case 'R':
//                if ( inclass ) break;
//                if ( create ) root = new LineEnding();

//                return -1;

//            case 'S':
//                if ( create )
//                    root = Has( UNICODE_CHARACTER_CLASS )
//                        ? new Utype( UnicodeProp.WHITE_SPACE ).complement()
//                        : new Ctype( ASCII.SPACE ).complement();

//                return -1;

//            case 'T':
//            case 'U':
//                break;

//            case 'V':
//                if ( create ) root = new VertWS().complement();

//                return -1;

//            case 'W':
//                if ( create )
//                    root = Has( UNICODE_CHARACTER_CLASS )
//                        ? new Utype( UnicodeProp.WORD ).complement()
//                        : new Ctype( ASCII.WORD ).complement();

//                return -1;

//            case 'X':
//            case 'Y':
//                break;

//            case 'Z':
//                if ( inclass ) break;

//                if ( create )
//                {
//                    if ( Has( UNIX_LINES ) )
//                    {
//                        root = new UnixDollar( false );
//                    }
//                    else
//                    {
//                        root = new Dollar( false );
//                    }
//                }

//                return -1;

//            case 'a':
//                return '\007';

//            case 'b':
//                if ( inclass ) break;
//                if ( create ) root = new Bound( Bound.BOTH, Has( UNICODE_CHARACTER_CLASS ) );

//                return -1;

//            case 'c':
//                return c();

//            case 'd':
//                if ( create )
//                {
//                    root = Has( UNICODE_CHARACTER_CLASS )
//                        ? new Utype( UnicodeProp.DIGIT )
//                        : new Ctype( ASCII.DIGIT );
//                }

//                return -1;

//            case 'e':
//                return '\033';

//            case 'f':
//                return '\f';

//            case 'g':
//                break;

//            case 'h':
//                if ( create ) root = new HorizWS();

//                return -1;

//            case 'i':
//            case 'j':
//                break;

//            case 'k':
//                if ( inclass ) break;

//                if ( read() != '<' )
//                {
//                    throw error( "\\k is not followed by '<' for named capturing group" );
//                }

//                string name = groupname( read() );

//                if ( !namedGroups().containsKey( name ) )
//                {
//                    throw error( "(named capturing group <" + name + "> does not exit" );
//                }

//                if ( create )
//                {
//                    if ( Has( CASE_INSENSITIVE ) )
//                    {
//                        root = new CIBackRef( namedGroups().get( name ), Has( UNICODE_CASE ) );
//                    }
//                    else
//                    {
//                        root = new BackRef( namedGroups().get( name ) );
//                    }
//                }

//                return -1;

//            case 'l':
//            case 'm':
//                break;

//            case 'n':
//                return '\n';

//            case 'o':
//            case 'p':
//            case 'q':
//                break;

//            case 'r':
//                return '\r';

//            case 's':
//                if ( create )
//                    root = Has( UNICODE_CHARACTER_CLASS )
//                        ? new Utype( UnicodeProp.WHITE_SPACE )
//                        : new Ctype( ASCII.SPACE );

//                return -1;

//            case 't':
//                return '\t';

//            case 'u':
//                return u();

//            case 'v':
                // '\v' was implemented as VT/0x0B in releases < 1.8 (though
                // undocumented). In JDK8 '\v' is specified as a predefined
                // character class for all vertical whitespace characters.
                // So [-1, root=VertWS node] pair is returned (instead of a
                // single 0x0B). This breaks the range if '\v' is used as
                // the start or end value, such as [\v-...] or [...-\v], in
                // which a single definite value (0x0B) is expected. For
                // compatibility concern '\013'/0x0B is returned if isrange.
//                if ( isrange )
//                    return '\013';

//                if ( create ) root = new VertWS();

//                return -1;

//            case 'w':
//                if ( create )
//                    root = Has( UNICODE_CHARACTER_CLASS )
//                        ? new Utype( UnicodeProp.WORD )
//                        : new Ctype( ASCII.WORD );

//                return -1;

//            case 'x':
//                return x();

//            case 'y':
//                break;

//            case 'z':
//                if ( inclass ) break;
//                if ( create ) root = new End();

//                return -1;

//            default:
//                return ch;
//        }

//        throw error( "Illegal/unsupported escape sequence" );
        return -1;
    }

    /**
     * Parse a character class, and return the node that matches it.
     *
     * Consumes a ] on the way out if consume is true. Usually consume
     * is true except for the case of [abc&&def] where def is a separate
     * right hand node with "understood" brackets.
     */
    [Obsolete]
    private CharProperty clazz( bool consume )
    {
        CharProperty prev         = null;
        CharProperty node         = null;
        BitClass     bits         = new BitClass();
        bool         include      = true;
        bool         firstInClass = true;
        int          ch           = Next();

        for ( ;; )
        {
            switch ( ch )
            {
                case '^':
                    // Negates if first char in a class, otherwise literal
                    if ( firstInClass )
                    {
                        if ( _temp[ _cursor - 1 ] != '[' )
                            break;

                        ch      = Next();
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
//                    else
//                        prev = union( prev, node );

                    ch = Peek();

                    continue;

                case '&':
                    firstInClass = false;
                    ch           = Next();

                    if ( ch == '&' )
                    {
                        ch = Next();
                        CharProperty rightNode = null;

                        while ( ( ch != ']' ) && ( ch != '&' ) )
                        {
                            if ( ch == '[' )
                            {
                                if ( rightNode == null )
                                    rightNode = clazz( true );
//                                else
//                                    rightNode = union( rightNode, clazz( true ) );
                            }
                            else
                            {
                                // abc&&def
//                                unread();
                                rightNode = clazz( false );
                            }

                            ch = Peek();
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
//                        else
//                        {
//                            prev = intersection( prev, node );
//                        }
                    }
                    else
                    {
                        // treat as a literal &
//                        unread();

                        break;
                    }

                    continue;

                case 0:
                    firstInClass = false;

                    if ( _cursor >= _patternLength )
                        throw error( "Unclosed character class" );

                    break;

                case ']':
                    firstInClass = false;

                    if ( prev != null )
                    {
                        if ( consume )
                            Next();

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
//                    if ( prev != node )
//                        prev = union( prev, node );
                }
            }
            else
            {
//                if ( prev == null )
//                {
//                    prev = node.complement();
//                }
//                else
//                {
//                    if ( prev != node )
//                        prev = setDifference( prev, node );
//                }
            }

            ch = Peek();
        }
    }

    [Obsolete]
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

//        if ( ( ch < 256 )
//             && !( Has( CASE_INSENSITIVE )
//                   && Has( UNICODE_CASE )
//                   && ( ( ch == 0xff )
//                        || ( ch == 0xb5 )
//                        || ( ch == 0x49 )
//                        || ( ch == 0x69 )
//                        || //I and i
//                        ( ch == 0x53 )
//                        || ( ch == 0x73 )
//                        || //S and s
//                        ( ch == 0x4b )
//                        || ( ch == 0x6b )
//                        || //K and k
//                        ( ch == 0xc5 )
//                        || ( ch == 0xe5 ) ) ) ) //A+ring
//            return bits.Add( ch, flags() );

        return newSingle( ch );
    }

    /**
     * Parse a single character or a character range in a character class
     * and return its representative node.
     */
    [Obsolete]
    private CharProperty range( BitClass bits )
    {
        int ch = Peek();

//        if ( ch == '\\' )
//        {
//            ch = nextEscaped();

//            if ( ( ch == 'p' ) || ( ch == 'P' ) )
//            {
                // A property
//                bool comp      = ( ch == 'P' );
//                bool oneLetter = true;
                // Consume { if present
//                ch = Next();

//                if ( ch != '{' )
//                    unread();
//                else
//                    oneLetter = false;

//                return family( oneLetter, comp );
//            }
//            else
//            {
                // ordinary escape
//                bool isrange = _temp[ _cursor + 1 ] == '-';
//                unread();
//                ch = escape( true, true, isrange );

//                if ( ch == -1 )
//                    return ( CharProperty )root;
//            }
//        }
//        else
//        {
//            Next();
//        }

//        if ( ch >= 0 )
//        {
//            if ( Peek() == '-' )
//            {
//                int endRange = _temp[ _cursor + 1 ];

//                if ( endRange == '[' )
//                {
//                    return bitsOrSingle( bits, ch );
//                }

//                if ( endRange != ']' )
//                {
//                    Next();
//                    int m = Peek();

//                    if ( m == '\\' )
//                    {
//                        m = escape( true, false, true );
//                    }
//                    else
//                    {
//                        Next();
//                    }

//                    if ( m < ch )
//                    {
//                        throw error( "Illegal character range" );
//                    }

//                    if ( Has( CASE_INSENSITIVE ) )
//                        return caseInsensitiveRangeFor( ch, m );
//                    else
//                        return rangeFor( ch, m );
//                }
//            }

//            return bitsOrSingle( bits, ch );
//        }

//        throw error( "Unexpected character '" + ( ( char )ch ) + "'" );

        return default!;
    }

    /**
     * Parses a Unicode character family and returns its representative node.
     */
    [Obsolete]
    private CharProperty family( bool singleLetter,
                                 bool maybeComplement )
    {
        Next();
        string       name;
        CharProperty node = null;

//        if ( singleLetter )
//        {
//            int c = _temp[ _cursor ];

//            if ( !Character.isSupplementaryCodePoint( c ) )
//            {
//                name = string.valueOf( ( char )c );
//            }
//            else
//            {
//                name = new string( temp, _cursor, 1 );
//            }

//            read();
//        }
//        else
//        {
//            int i = _cursor;
//            mark( '}' );

//            while ( read() != '}' )
//            {
//            }

//            mark( '\000' );
//            int j = _cursor;

//            if ( j > _patternLength )
//                throw error( "Unclosed character family" );

//            if ( ( i + 1 ) >= j )
//                throw error( "Empty character family" );

//            name = new string( temp, i, j - i - 1 );
//        }

//        int i = name.indexOf( '=' );

//        if ( i != -1 )
//        {
            // property construct \p{name=value}
//            string value = name.substring( i + 1 );
//            name = name.substring( 0, i ).toLowerCase( Locale.ENGLISH );

//            if ( "sc".equals( name ) || "script".equals( name ) )
//            {
//                node = unicodeScriptPropertyFor( value );
//            }
//            else if ( "blk".equals( name ) || "block".equals( name ) )
//            {
//                node = unicodeBlockPropertyFor( value );
//            }
//            else if ( "gc".equals( name ) || "general_category".equals( name ) )
//            {
//                node = charPropertyNodeFor( value );
//            }
//            else
//            {
//                throw error
//                    (
//                     "Unknown Unicode property {name=<"
//                     + name
//                     + ">, "
//                     + "value=<"
//                     + value
//                     + ">}"
//                    );
//            }
//        }
//        else
//        {
//            if ( name.startsWith( "In" ) )
//            {
                // \p{inBlockName}
//                node = unicodeBlockPropertyFor( name.substring( 2 ) );
//            }
//            else if ( name.startsWith( "Is" ) )
//            {
                // \p{isGeneralCategory} and \p{isScriptName}
//                name = name.substring( 2 );
//                UnicodeProp uprop = UnicodeProp.forName( name );

//                if ( uprop != null )
//                    node = new Utype( uprop );

//                if ( node == null )
//                    node = CharPropertyNames.charPropertyFor( name );

//                if ( node == null )
//                    node = unicodeScriptPropertyFor( name );
//            }
//            else
//            {
//                if ( Has( UNICODE_CHARACTER_CLASS ) )
//                {
//                    UnicodeProp uprop = UnicodeProp.forPOSIXName( name );

//                    if ( uprop != null )
//                        node = new Utype( uprop );
//                }

//                if ( node == null )
//                    node = charPropertyNodeFor( name );
//            }
//        }

//        if ( maybeComplement )
//        {
//            if ( node is Category || node is Block)
//            hasSupplementary = true;
//            node             = node.complement();
//        }

//        return node;
        return default!;
    }


    /**
     * Returns a CharProperty matching all characters belong to
     * a UnicodeScript.
     */
    [Obsolete]
    private CharProperty unicodeScriptPropertyFor( string name )
    {
//        Character.UnicodeScript script;

//        try
//        {
//            script = Character.UnicodeScript.forName( name );
//        }
//        catch ( IllegalArgumentException iae )
//        {
//            throw error( "Unknown character script name {" + name + "}" );
//        }

//        return new Script( script );
        return default!;
    }

    /**
     * Returns a CharProperty matching all characters in a UnicodeBlock.
     */
    [Obsolete]
    private CharProperty unicodeBlockPropertyFor( string name )
    {
//        Character.UnicodeBlock block;

//        try
//        {
//            block = Character.UnicodeBlock.forName( name );
//        }
//        catch ( IllegalArgumentException iae )
//        {
//            throw error( "Unknown character block name {" + name + "}" );
//        }

//        return new Block( block );
        return default!;
    }

    /**
     * Returns a CharProperty matching all characters in a named property.
     */
    [Obsolete]
    private CharProperty charPropertyNodeFor( string name )
    {
//        CharProperty p = CharPropertyNames.charPropertyFor( name );

//        if ( p == null )
//            throw error( "Unknown character property name {" + name + "}" );

//        return p;
        return default!;
    }

    /**
     * Parses and returns the name of a "named capturing group", the trailing
     * ">" is consumed after parsing.
     */
    [Obsolete]
    private string groupname( int ch )
    {
        StringBuilder sb = new StringBuilder();
//        sb.append( Character.toChars( ch ) );

//        while ( ASCII.isLower( ch = read() ) || ASCII.isUpper( ch ) || ASCII.isDigit( ch ) )
//        {
//            sb.append( Character.toChars( ch ) );
//        }

//        if ( sb.length() == 0 )
//            throw error( "named capturing group has 0 length name" );

//        if ( ch != '>' )
//            throw error( "named capturing group is missing trailing '>'" );

        return sb.ToString();
    }

    /**
     * Parses a group and returns the head node of a set of nodes that process
     * the group. Sometimes a double return system is used where the tail is
     * returned in root.
     */
    [Obsolete]
    private Node group0()
    {
/*
        bool capturingGroup = false;
        Node head           = null;
        Node tail           = null;
        int  save           = flags;
        root = null;
        int ch = Next();

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
                        string name = groupname( ch );

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

                    int start = _cursor;
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

                    bool hasSupplementary = findSupplementary( start, _patternLength );

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

        if ( node is Ques) {
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
        } else if ( node is Curly) {
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
*/

        throw error( "Internal logic error" );
    }

    /**
     * Create group head and tail nodes using double return. If the group is
     * created with anonymous true then it is a pure group and should not
     * affect group counting.
     */
    [Obsolete]
    private Node createGroup( bool anonymous )
    {
//        int localIndex = localCount++;
//        int groupIndex = 0;

//        if ( !anonymous )
//            groupIndex = capturingGroupCount++;

//        GroupHead head = new GroupHead( localIndex );
//        root = new GroupTail( localIndex, groupIndex );

//        if ( !anonymous && ( groupIndex < 10 ) )
//            groupNodes[ groupIndex ] = head;

//        return head;
        return default!;
    }

    /**
     * Parses inlined match flags and set them appropriately.
     */
    [Obsolete]
    private void addFlag()
    {
//        int ch = Peek();

//        for ( ;; )
//        {
//            switch ( ch )
//            {
//                case 'i':
//                    flags |= CASE_INSENSITIVE;

//                    break;

//                case 'm':
//                    flags |= MULTILINE;

//                    break;

//                case 's':
//                    flags |= DOTALL;

//                    break;

//                case 'd':
//                    flags |= UNIX_LINES;

//                    break;

//                case 'u':
//                    flags |= UNICODE_CASE;

//                    break;

//                case 'c':
//                    flags |= CANON_EQ;

//                    break;

//                case 'x':
//                    flags |= COMMENTS;

//                    break;

//                case 'U':
//                    flags |= ( UNICODE_CHARACTER_CLASS | UNICODE_CASE );

//                    break;

//                case '-': // subFlag then fall through
//                    ch = Next();
//                    subFlag();

//                default:
//                    return;
//            }

//            ch = Next();
//        }
    }

    /**
     * Parses the second part of inlined match flags and turns off
     * flags appropriately.
     */
    [Obsolete]
    private void subFlag()
    {
//        int ch = Peek();

//        for ( ;; )
//        {
//            switch ( ch )
//            {
//                case 'i':
//                    flags &= ~CASE_INSENSITIVE;

//                    break;

//                case 'm':
//                    flags &= ~MULTILINE;

//                    break;

//                case 's':
//                    flags &= ~DOTALL;

//                    break;

//                case 'd':
//                    flags &= ~UNIX_LINES;

//                    break;

//                case 'u':
//                    flags &= ~UNICODE_CASE;

//                    break;

//                case 'c':
//                    flags &= ~CANON_EQ;

//                    break;

//                case 'x':
//                    flags &= ~COMMENTS;

//                    break;

//                case 'U':
//                    flags &= ~( UNICODE_CHARACTER_CLASS | UNICODE_CASE );

//                default:
//                    return;
//            }

//            ch = Next();
//        }
    }

    /**
     * Processes repetition. If the next character peeked is a quantifier
     * then new nodes must be appended to handle the repetition.
     * Prev could be a single or a group, so it could be a chain of nodes.
     */
    [Obsolete]
    private Node closure( Node prev )
    {
//        Node atom;
//        int  ch = Peek();

//        switch ( ch )
//        {
//            case '?':
//                ch = Next();

//                if ( ch == '?' )
//                {
//                    Next();

//                    return new Ques( prev, LAZY );
//                }
//                else if ( ch == '+' )
//                {
//                    Next();

//                    return new Ques( prev, POSSESSIVE );
//                }

//                return new Ques( prev, GREEDY );

//            case '*':
//                ch = Next();

//                if ( ch == '?' )
//                {
//                    Next();

//                    return new Curly( prev, 0, MAX_REPS, LAZY );
//                }
//                else if ( ch == '+' )
//                {
//                    Next();

//                    return new Curly( prev, 0, MAX_REPS, POSSESSIVE );
//                }

//                return new Curly( prev, 0, MAX_REPS, GREEDY );

//            case '+':
//                ch = Next();

//                if ( ch == '?' )
//                {
//                    Next();

//                    return new Curly( prev, 1, MAX_REPS, LAZY );
//                }
//                else if ( ch == '+' )
//                {
//                    Next();

//                    return new Curly( prev, 1, MAX_REPS, POSSESSIVE );
//                }

//                return new Curly( prev, 1, MAX_REPS, GREEDY );

//            case '{':
//                ch = _temp[ _cursor + 1 ];

//                if ( ASCII.isDigit( ch ) )
//                {
//                    skip();
//                    int cmin = 0;

//                    do
//                    {
//                        cmin = ( cmin * 10 ) + ( ch - '0' );
//                    }
//                    while ( ASCII.isDigit( ch = read() ) );

//                    int cmax = cmin;

//                    if ( ch == ',' )
//                    {
//                        ch   = read();
//                        cmax = MAX_REPS;

//                        if ( ch != '}' )
//                        {
//                            cmax = 0;

//                            while ( ASCII.isDigit( ch ) )
//                            {
//                                cmax = ( cmax * 10 ) + ( ch - '0' );
//                                ch   = read();
//                            }
//                        }
//                    }

//                    if ( ch != '}' )
//                        throw error( "Unclosed counted closure" );

//                    if ( ( ( cmin ) | ( cmax ) | ( cmax - cmin ) ) < 0 )
//                        throw error( "Illegal repetition range" );

//                    Curly curly;
//                    ch = Peek();

//                    if ( ch == '?' )
//                    {
//                        Next();
//                        curly = new Curly( prev, cmin, cmax, LAZY );
//                    }
//                    else if ( ch == '+' )
//                    {
//                        Next();
//                        curly = new Curly( prev, cmin, cmax, POSSESSIVE );
//                    }
//                    else
//                    {
//                        curly = new Curly( prev, cmin, cmax, GREEDY );
//                    }

//                    return curly;
//                }
//                else
//                {
//                    throw error( "Illegal repetition" );
//                }

//            default:
//                return prev;
//        }
        return default!;
    }

    /**
     *  Utility method for parsing control escape sequences.
     */
    [Obsolete]
    private int c()
    {
//        if ( _cursor < _patternLength )
//        {
//            return read() ^ 64;
//        }

        throw error( "Illegal control escape sequence" );
    }

    /**
     *  Utility method for parsing octal escape sequences.
     */
    [Obsolete]
    private int o()
    {
//        int n = read();

//        if ( ( ( n - '0' ) | ( '7' - n ) ) >= 0 )
//        {
//            int m = read();

//            if ( ( ( m - '0' ) | ( '7' - m ) ) >= 0 )
//            {
//                int o = read();

//                if ( ( ( ( o - '0' ) | ( '7' - o ) ) >= 0 ) && ( ( ( n - '0' ) | ( '3' - n ) ) >= 0 ) )
//                {
//                    return ( ( n - '0' ) * 64 ) + ( ( m - '0' ) * 8 ) + ( o - '0' );
//                }

//                unread();

//                return ( ( n - '0' ) * 8 ) + ( m - '0' );
//            }

//            unread();

//            return ( n - '0' );
//        }

        throw error( "Illegal octal escape sequence" );
    }

    /**
     *  Utility method for parsing hexadecimal escape sequences.
     */
    [Obsolete]
    private int x()
    {
//        int n = read();

//        if ( ASCII.IsHexDigit( n ) )
//        {
//            int m = read();

//            if ( ASCII.IsHexDigit( m ) )
//            {
//                return ( ASCII.ToDigit( n ) * 16 ) + ASCII.ToDigit( m );
//            }
//        }
//        else if ( ( n == '{' ) && ASCII.IsHexDigit( Peek() ) )
//        {
//            int ch = 0;

//            while ( ASCII.IsHexDigit( n = read() ) )
//            {
//                ch = ( ch << 4 ) + ASCII.ToDigit( n );

//                if ( ch > Character.MAX_CODE_POINT )
//                    throw error( "Hexadecimal codepoint is too big" );
//            }

//            if ( n != '}' )
//                throw error( "Unclosed hexadecimal escape sequence" );

//            return ch;
//    }

        throw error( "Illegal hexadecimal escape sequence" );
    }

    /**
     *  Utility method for parsing unicode escape sequences.
     */
    [Obsolete]
    private int Cursor()
    {
        return _cursor;
    }

    [Obsolete]
    private void setcursor( int pos )
    {
        _cursor = pos;
    }

    [Obsolete]
    private int uxxxx()
    {
        int n = 0;

//        for ( int i = 0; i < 4; i++ )
//        {
//            int ch = read();

//            if ( !ASCII.IsHexDigit( ch ) )
//            {
//                throw error( "Illegal Unicode escape sequence" );
//            }

//            n = ( n * 16 ) + ASCII.ToDigit( ch );
//        }

        return n;
    }

    [Obsolete]
    private int u()
    {
        int n = uxxxx();

//        if ( CharHelper.isHighSurrogate( ( char )n ) )
//        {
//            int cur = Cursor();

//            if ( ( read() == '\\' ) && ( read() == 'u' ) )
//            {
//                int n2 = uxxxx();

//                if ( Character.isLowSurrogate( ( char )n2 ) )
//                    return Character.toCodePoint( ( char )n, ( char )n2 );
//            }

//            setcursor( cur );
//        }

        return n;
    }

    //
    // Utility methods for code point support
    //

    [Obsolete]
    private static int countChars( string seq,
                                   int index,
                                   int lengthInCodePoints )
    {
        // optimization
//        if ( ( lengthInCodePoints == 1 ) && !Character.isHighSurrogate( seq.charAt( index ) ) )
//        {
//            assert( ( index >= 0 ) && ( index < seq.length() ) );

//            return 1;
//        }

//        int length = seq.length();
//        int x      = index;

//        if ( lengthInCodePoints >= 0 )
//        {
//            assert( ( index >= 0 ) && ( index < length ) );

//            for ( int i = 0; ( x < length ) && ( i < lengthInCodePoints ); i++ )
//            {
//                if ( Character.isHighSurrogate( seq.charAt( x++ ) ) )
//                {
//                    if ( ( x < length ) && Character.isLowSurrogate( seq.charAt( x ) ) )
//                    {
//                        x++;
//                    }
//                }
//            }

//            return x - index;
//        }

//        assert( ( index >= 0 ) && ( index <= length ) );

//        if ( index == 0 )
//        {
//            return 0;
//        }

//        int len = -lengthInCodePoints;

//        for ( int i = 0; ( x > 0 ) && ( i < len ); i++ )
//        {
//            if ( Character.isLowSurrogate( seq.charAt( --x ) ) )
//            {
//                if ( ( x > 0 ) && Character.isHighSurrogate( seq.charAt( x - 1 ) ) )
//                {
//                    x--;
//                }
//            }
//        }

//        return index - x;
        return -1;
    }

    [Obsolete]
    private static int countCodePoints( string seq )
    {
//        int length = seq.length();
        int n      = 0;

//        for ( int i = 0; i < length; )
//        {
//            n++;

//            if ( Character.isHighSurrogate( seq.charAt( i++ ) ) )
//            {
//                if ( ( i < length ) && Character.isLowSurrogate( seq.charAt( i ) ) )
//                {
//                    i++;
//                }
//            }
//        }

        return n;
    }

    /**
 *  Returns a suitably optimized, single character matcher.
 */
    [Obsolete]
    private Pattern.CharProperty newSingle( int ch )
    {
        if ( Has( Case_Insensitive ) )
        {
            int lower, upper;

//            if ( Has( Unicode_Case ) )
//            {
//                upper = Character.toUpperCase( ch );
//                lower = Character.toLowerCase( upper );

//                if ( upper != lower )
//                    return new Pattern.SingleU( lower );
//            }
//            else if ( ASCII.IsAscii( ch ) )
//            {
//                lower = ASCII.ToLower( ch );
//                upper = ASCII.ToUpper( ch );

//                if ( lower != upper )
//                    return new Pattern.SingleI( lower, upper );
//            }
        }

//        if ( isSupplementary( ch ) )
//        {
//            return new Pattern.SingleS( ch ); // Match a given Unicode character
//        }

//        return new Single( ch ); // Match a given BMP character
        return default!;
    }

    /**
 *  Utility method for creating a string slice matcher.
 */
    [Obsolete]
    private Pattern.Node newSlice( int[] buf, int count, bool hasSupplementary )
    {
//        int[] tmp = new int[ count ];

//        if ( Has( Case_Insensitive ) )
//        {
//            if ( Has( Unicode_Case ) )
//            {
//                for ( int i = 0; i < count; i++ )
//                {
//                    tmp[ i ] = Character.toLowerCase( Character.toUpperCase( buf[ i ] ) );
//                }

//                return hasSupplementary ? new SliceUS( tmp ) : new SliceU( tmp );
//            }

//            for ( int i = 0; i < count; i++ )
//            {
//                tmp[ i ] = ASCII.ToLower( buf[ i ] );
//            }

//            return hasSupplementary ? new SliceIS( tmp ) : new SliceI( tmp );
//        }

//        for ( int i = 0; i < count; i++ )
//        {
//            tmp[ i ] = buf[ i ];
//        }

//        return hasSupplementary ? new SliceS( tmp ) : new Slice( tmp );
        return default!;
    }

    [Obsolete]
    private static bool InRange( int lower, int ch, int upper )
    {
        return ( lower <= ch ) && ( ch <= upper );
    }
}

// ----------------------------------------------------------------------------
