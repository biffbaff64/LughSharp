using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LibGDXSharp.Utils.Regex;

/// <inheritdoc/>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Pattern : IPattern
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

    [NonSerialized] private volatile bool                      _compiled = false;
    [NonSerialized] private volatile Dictionary< string, int > _namedGroups;

    [NonSerialized] private string      _normalizedPattern;
    [NonSerialized] private int[]       _buffer;
    [NonSerialized] private GroupHead[] _groupNodes;
    [NonSerialized] private int[]       _temp;
    [NonSerialized] private int         _cursor;
    [NonSerialized] private int         _patternLength;
    [NonSerialized] private bool        _hasSupplementary;

    public int     CapturingGroupCount { get; set; }
    public int     LocalCount          { get; set; }
    public string? pattern;
    public int     flags;

    [NonSerialized] public Node root      = null!;
    [NonSerialized] public Node matchRoot = null!;

    public static Pattern Compile( string regex )
    {
        return new Pattern( regex, 0 );
    }

    public static Pattern Compile( string regex, int flags )
    {
        return new Pattern( regex, flags );
    }

    public Dictionary< string, int > NamedGroups()
    {
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

    public static bool Matches( string regex, string input )
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

    public static string Quote( string s )
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

        if ( pattern?.Length == 0 )
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
        _normalizedPattern = Normalizer.normalize( pattern, Normalizer.Form.NFD );
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
        StringBuilder result = new(source);

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
        int    len                = CountChars( input, 0, 2 );
        string firstTwoCharacters = input.Substring( 0, len );
        string result             = Normalizer.normalize( firstTwoCharacters, Normalizer.Form.NFC );

        if ( result.Equals( firstTwoCharacters ) )
            return null;
        else
        {
            string remainder = input.Substring( len );

            return result + remainder;
        }
    }

    public new string ToString()
    {
        return pattern ?? "";
    }

    #region CompanionClasses

    /// <summary>
    /// Base class for all node classes. Subclasses should override the match()
    /// method as appropriate. This class is an accepting node, so its match()
    /// always returns true.
    /// </summary>
    internal class Node : object
    {
        internal readonly Node? next;

        internal Node()
        {
            next = Pattern.accept;
        }

        /// <summary>
        /// This method implements the classic accept node.
        /// </summary>
        internal virtual bool Match( Matcher matcher, int i, string seq )
        {
            matcher.last        = i;
            matcher.groups[ 0 ] = matcher.first;
            matcher.groups[ 1 ] = matcher.last;

            return true;
        }

        /// <summary>
        /// This method is good for all zero length assertions.
        /// </summary>
        internal virtual bool Study( TreeInfo info )
        {
            if ( next != null )
            {
                return next.Study( info );
            }
            else
            {
                return info.deterministic;
            }
        }
    }

    public class TreeInfo
    {
    }
    
    #endregion
}