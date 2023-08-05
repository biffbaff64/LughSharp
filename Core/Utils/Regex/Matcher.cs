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

[Obsolete]
public class Matcher : IMatchResult
{
    // The Pattern object that created this Matcher.
    public Pattern parentPattern;

    // The storage used by groups. They may contain invalid values
    // if a group was skipped during the matching.
    public int[] groups;
        
    // The range of string that last matched the pattern. If the last
    // match failed then first is -1; last initially holds 0 then it
    // holds the index of the end of the last match (which is where
    // the next search starts).
    public int first = -1;
    public int last  = 0;
        
    /// <summary>
    /// Storage used by nodes to tell what repetition they are on in
    /// a pattern, and where groups begin. The nodes themselves are stateless,
    /// so they rely on this field to hold state during a match.
    /// </summary>
    public int[] locals;

    /// <summary>
    /// bool indicating whether or not more input could change
    /// the results of the last match.
    /// 
    /// If hitEnd is true, and a match was found, then more input
    /// might cause a different match to be found.
    /// If hitEnd is true and a match was not found, then more
    /// input could cause a match to be found.
    /// If hitEnd is false and a match was found, then more input
    /// will not change the match.
    /// If hitEnd is false and a match was not found, then more
    /// input will not cause a match to be found.
    /// </summary>
    public bool HitEnd { get; set; }

    /// <summary>
    /// bool indicating whether or not more input could change
    /// a positive match into a negative one.
    /// 
    /// If requireEnd is true, and a match was found, then more
    /// input could cause the match to be lost.
    /// If requireEnd is false and a match was found, then more
    /// input might change the match but the match won't be lost.
    /// If a match was not found, then requireEnd has no meaning.
    /// </summary>
    public bool RequireEnd { get; set; }

    // The range within the sequence that is to be matched.
    // Anchors will match at these "hard" boundaries. Changing the
    // region changes these values.
    private int _regionStart;
    private int _regionEnd;

    // Lookbehind uses this value to ensure that the subexpression
    // match ends at the point where the lookbehind was encountered.
    private int _lookbehindTo;

    // The original string being matched.
    private string _text;

    // Matcher state used by the last node. NoAnchor is used when a
    // match does not have to consume all of the input. EndAnchor is
    // the mode used for matching all the input.
    private const int EndAnchor = 1;
    private const int NoAnchor  = 0;

    private int _acceptMode = NoAnchor;

    // The end index of what matched in the last match operation.
    private int _oldLast = -1;

    // The index of the last position appended in a substitution.
    private int _lastAppendPosition = 0;

    /// <summary>
    /// If transparentBounds is true then the boundaries of this
    /// matcher's region are transparent to lookahead, lookbehind,
    /// and boundary matching constructs that try to see beyond them.
    /// </summary>
    private bool _hasTransparentBounds = false;

    /// <summary>
    /// If anchoringBounds is true then the boundaries of this
    /// matcher's region match anchors such as ^ and $.
    /// </summary>
    private bool _hasAnchoringBounds = true;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="text"></param>
    [Obsolete]
    public Matcher( Pattern parent, string text )
    {
        parentPattern = parent;
        _text         = text;

        var parentGroupCount = Math.Max( parent.CapturingGroupCount, 10 );

        groups = new int[ parentGroupCount * 2 ];
        locals = new int[ parent.LocalCount ];

        Reset();
    }

    [Obsolete]
    public Pattern Pattern() => parentPattern;

    /// <summary>
    /// Returns the match state of this matcher as a MatchResult.
    /// The result is unaffected by subsequent operations performed
    /// upon this matcher.
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public IMatchResult ToMatchResult()
    {
        var result = new Matcher( parentPattern, _text )
        {
            first  = this.first,
            last   = this.last,
            groups = this.groups
        };

        return result;
    }

    /// <summary>
    /// Changes the Pattern that this Matcher uses to find matches
    /// with. This method causes this matcher to lose information
    /// about the groups of the last match that occurred. The matchers
    /// position in the input is maintained and its last append
    /// position is unaffected.
    /// </summary>
    [Obsolete]
    public Matcher UsePattern( Pattern? newPattern )
    {
        parentPattern = newPattern ?? throw new ArgumentException( "Pattern cannot be null" );

        // Reallocate state storage
        var parentGroupCount = Math.Max( newPattern.CapturingGroupCount, 10 );

        groups = new int[ parentGroupCount * 2 ];
        locals = new int[ newPattern.LocalCount ];

        for ( var i = 0; i < groups.Length; i++ )
        {
            groups[ i ] = -1;
        }

        for ( var i = 0; i < locals.Length; i++ )
        {
            locals[ i ] = -1;
        }

        return this;
    }

    /// <summary>
    /// Resets this matcher.
    /// <para></para>
    /// <para>
    /// Resetting a matcher discards all of its explicit state
    /// information and sets its append position to zero. The
    /// matcher's region is set to the default region, which is
    /// its entire character sequence.
    /// </para>
    /// <para>
    /// The anchoring and transparency of this matcher's region
    /// boundaries are unaffected.
    /// </para>
    /// </summary>
    [Obsolete]
    public Matcher Reset()
    {
        first    = -1;
        last     = 0;
        _oldLast = -1;

        for ( var i = 0; i < groups.Length; i++ )
        {
            groups[ i ] = -1;
        }

        for ( var i = 0; i < locals.Length; i++ )
        {
            locals[ i ] = -1;
        }

        _lastAppendPosition = 0;
        _regionStart        = 0;
        _regionEnd          = _text.Length;

        return this;
    }

    /// <summary>
    /// Resets this matcher with a new input sequence.
    /// <para></para>
    /// <para>
    /// Resetting a matcher discards all of its explicit state
    /// information and sets its append position to zero. The
    /// matcher's region is set to the default region, which is
    /// its entire character sequence.
    /// </para>
    /// <para>
    /// The anchoring and transparency of this matcher's region
    /// boundaries are unaffected.
    /// </para>
    /// </summary>
    [Obsolete]
    public Matcher Reset( string input )
    {
        _text = input;

        return Reset();
    }

    /// <summary>
    /// Returns the start index of the previous match.
    /// </summary>
    [Obsolete]
    public int Start()
    {
        if ( first < 0 ) throw new IllegalStateException( "No match available" );

        return first;
    }

    /// <summary>
    /// Returns the start index of the subsequence captured by the
    /// given group during the previous match operation.
    /// <para>
    /// Capturing groups are indexed from left to right, starting at one.
    /// Group zero denotes the entire pattern, so the expression <tt>m.start(0)</tt>
    /// is equivalent to <tt>m.start()</tt>.
    /// </para>
    /// </summary>
    /// <param name="group">
    /// The index of a capturing group in this matcher's pattern
    /// </param>
    /// <returns>
    /// The index of the first character captured by the group, or -1
    /// if the match was successful but the group itself did not match
    /// anything.
    /// </returns>
    /// <exception cref="IllegalStateException">
    /// If no match has yet been attempted, or if the previous match
    /// operation failed
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If there is no capturing group in the pattern with the
    /// given index.
    /// </exception>
    [Obsolete]
    public int Start( int group )
    {
        if ( first < 0 ) throw new IllegalStateException( "No match available" );

        if ( ( group < 0 ) || ( group > GroupCount() ) )
        {
            throw new IndexOutOfRangeException( "No group " + group );
        }

        return groups[ group * 2 ];
    }

    /// <summary>
    /// Returns the start index of the subsequence captured by the given
    /// named-capturing group during the previous match operation.
    /// </summary>
    /// <param name="name">
    /// The name of a named-capturing group in this matcher's pattern
    /// </param>
    /// <returns>
    /// The index of the first character captured by the group, or <tt>-1</tt>
    /// if the match was successful but the group itself did not match anything
    /// </returns>
    /// <exception cref="IllegalStateException">
    /// If no match has yet been attempted, or if the previous match operation failed
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If there is no capturing group in the pattern with the given name
    /// </exception>
    [Obsolete]
    public int Start( string name )
    {
        return groups[ GetMatchedGroupIndex( name ) * 2 ];
    }

    /// <summary>
    /// Returns the offset after the last character matched.
    /// </summary>
    [Obsolete]
    public int End()
    {
        if ( first < 0 ) throw new IllegalStateException( "No match available" );

        return last;
    }

    /// <summary>
    /// Returns the offset after the last character of the subsequence
    /// captured by the given group during the previous match operation.
    /// </summary>
    /// <param name="group">
    /// The index of a capturing group in this matcher's pattern
    /// </param>
    /// <returns>
    /// The offset after the last character captured by the group, or <tt>-1</tt>
    /// if the match was successful but the group itself did not match anything
    /// </returns>
    /// <exception cref="IllegalStateException">
    /// If no match has yet been attempted, or if the previous match operation failed
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If there is no capturing group in the pattern with the given index
    /// </exception>
    [Obsolete]
    public int End( int group )
    {
        if ( first < 0 ) throw new IllegalStateException( "No match available" );

        if ( ( group < 0 ) || ( group > GroupCount() ) )
        {
            throw new IndexOutOfRangeException( "No group " + group );
        }

        return groups[ ( group * 2 ) + 1 ];
    }

    /// <summary>
    /// Returns the offset after the last character of the subsequence
    /// captured by the given group during the previous match operation.
    /// </summary>
    /// <param name="name">
    /// The name of a named-capturing group in this matcher's pattern.
    /// </param>
    /// <returns>
    /// The offset after the last character captured by the group, or <tt>-1</tt>
    /// if the match was successful but the group itself did not match anything
    /// </returns>
    /// <exception cref="IllegalStateException">
    /// If no match has yet been attempted, or if the previous match operation failed
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If there is no capturing group in the pattern with the given name.
    /// </exception>
    [Obsolete]
    public int End( string name )
    {
        return groups[ ( GetMatchedGroupIndex( name ) * 2 ) + 1 ];
    }

    /// <inheritdoc />
    [Obsolete]
    public string? Group() => Group( 0 );

    /// <inheritdoc/>
    [Obsolete]
    public string? Group( int group )
    {
        if ( first < 0 ) throw new IllegalStateException( "No match found" );

        if ( ( group < 0 ) || ( group > GroupCount() ) )
        {
            throw new IndexOutOfRangeException( "No group " + group );
        }

        if ( ( groups[ group * 2 ] == -1 ) || ( groups[ ( group * 2 ) + 1 ] == -1 ) )
        {
            return null;
        }

        return GetSubSequence( groups[ group * 2 ], groups[ ( group * 2 ) + 1 ] );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [Obsolete]
    public string? Group( string name )
    {
        var group = GetMatchedGroupIndex( name );

        if ( ( groups[ group * 2 ] == -1 ) || ( groups[ ( group * 2 ) + 1 ] == -1 ) )
        {
            return null;
        }

        return GetSubSequence( groups[ group * 2 ], groups[ ( group * 2 ) + 1 ] );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public int GroupCount() => parentPattern.CapturingGroupCount - 1;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public bool Matches() => Match( _regionStart, EndAnchor );

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public bool Find()
    {
        var nextSearchIndex = last;

        if ( nextSearchIndex == first ) nextSearchIndex++;

        // If next search starts before region, start it at region
        if ( nextSearchIndex < _regionStart ) nextSearchIndex = _regionStart;

        // If next search starts beyond region then it fails
        if ( nextSearchIndex > _regionEnd )
        {
            for ( var i = 0; i < groups.Length; i++ )
            {
                groups[ i ] = -1;
            }

            return false;
        }

        return Search( nextSearchIndex );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    [Obsolete]
    public bool Find( int start )
    {
        var limit = _text.Length;

        if ( ( start < 0 ) || ( start > limit ) )
        {
            throw new IndexOutOfRangeException( "Illegal start index" );
        }

        Reset();

        return Search( start );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public bool LookingAt() => Match( _regionStart, NoAnchor );

    [Obsolete]
    public static string QuoteReplacement( string s )
    {
        if ( ( !s.Contains( '\\' ) ) && ( !s.Contains( '$' ) ) )
        {
            return s;
        }

        var sb = new StringBuilder();

        foreach ( var c in s )
        {
            if ( c is '\\' or '$' )
            {
                sb.Append( '\\' );
            }

            sb.Append( c );
        }

        return sb.ToString();
    }

    [Obsolete]
    public Matcher AppendReplacement( StringBuilder sb, string replacement )
    {
        // If no match, return error
        if ( first < 0 ) throw new IllegalStateException( "No match available" );

        // Process substitution string to replace group references with groups
        var cursor = 0;
        var result = new StringBuilder();

        while ( cursor < replacement.Length )
        {
            var nextChar = replacement[ cursor ];

            if ( nextChar == '\\' )
            {
                cursor++;

                if ( cursor == replacement.Length )
                {
                    throw new ArgumentException( "character to be escaped is missing" );
                }

                nextChar = replacement[ cursor ];

                result.Append( nextChar );

                cursor++;
            }
            else if ( nextChar == '$' )
            {
                // Skip past $
                cursor++;

                // Throw IAE if this "$" is the last character in replacement
                if ( cursor == replacement.Length )
                {
                    throw new ArgumentException( "Illegal group reference: group index is missing" );
                }

                nextChar = replacement[ cursor ];

                int refNum;

                if ( nextChar == '{' )
                {
                    cursor++;

                    var gsb = new StringBuilder();

                    while ( cursor < replacement.Length )
                    {
                        nextChar = replacement[ cursor ];

                        if ( ASCII.IsLower( nextChar ) || ASCII.IsUpper( nextChar ) || ASCII.IsDigit( nextChar ) )
                        {
                            gsb.Append( nextChar );

                            cursor++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if ( gsb.Length == 0 )
                    {
                        throw new ArgumentException( "named capturing group has 0 length name" );
                    }

                    if ( nextChar != '}' )
                    {
                        throw new ArgumentException( "named capturing group is missing trailing '}'" );
                    }

                    var gname = gsb.ToString();

                    if ( ASCII.IsDigit( gname[ 0 ] ) )
                    {
                        throw new ArgumentException( $"capturing group name {gname} starts with digit character" );
                    }

                    if ( !parentPattern.NamedGroups().ContainsKey( gname ) )
                    {
                        throw new ArgumentException( $"No group with name {gname}" );
                    }

                    refNum = parentPattern.NamedGroups()[ gname ];
                    cursor++;
                }
                else
                {
                    // The first number is always a group
                    refNum = nextChar - '0';

                    if ( refNum is < 0 or > 9 )
                    {
                        throw new ArgumentException( "Illegal group reference" );
                    }

                    cursor++;

                    // Capture the largest legal group string
                    var done = false;

                    while ( !done )
                    {
                        if ( cursor >= replacement.Length ) break;

                        var nextDigit = replacement[ cursor ] - '0';

                        if ( nextDigit is < 0 or > 9 ) break;

                        var newRefNum = ( refNum * 10 ) + nextDigit;

                        if ( GroupCount() < newRefNum )
                        {
                            done = true;
                        }
                        else
                        {
                            refNum = newRefNum;
                            cursor++;
                        }
                    }
                }

                // Append group
                if ( ( Start( refNum ) != -1 ) && ( End( refNum ) != -1 ) )
                {
                    result.Append( _text, Start( refNum ), End( refNum ) );
                }
            }
            else
            {
                result.Append( nextChar );
                cursor++;
            }
        }

        // Append the intervening text
        sb.Append( _text, _lastAppendPosition, first );

        // Append the match substitution
        sb.Append( result );

        _lastAppendPosition = last;

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sb"></param>
    /// <returns></returns>
    [Obsolete]
    public StringBuilder AppendTail( StringBuilder sb )
    {
        sb.Append( _text, _lastAppendPosition, _text.Length );

        return sb;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="replacement"></param>
    /// <returns></returns>
    [Obsolete]
    public string ReplaceAll( string replacement )
    {
        Reset();

        var result = Find();

        if ( result )
        {
            var sb = new StringBuilder();

            do
            {
                AppendReplacement( sb, replacement );

                result = Find();
            }
            while ( result );

            AppendTail( sb );

            return sb.ToString();
        }

        return _text;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="replacement"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    [Obsolete]
    public string ReplaceFirst( string replacement )
    {
        if ( replacement == null ) throw new NullReferenceException( "replacement" );

        Reset();

        if ( !Find() ) return _text;

        var sb = new StringBuilder();

        AppendReplacement( sb, replacement );
        AppendTail( sb );

        return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    [Obsolete]
    public Matcher Region( int start, int end )
    {
        if ( ( start < 0 ) || ( start > _text.Length ) ) throw new IndexOutOfRangeException( "start" );

        if ( ( end < 0 ) || ( end > _text.Length ) ) throw new IndexOutOfRangeException( "end" );

        if ( start > end ) throw new IndexOutOfRangeException( "start > end" );

        Reset();

        _regionStart = start;
        _regionEnd   = end;

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    [Obsolete]
    public Matcher UseTransparentBounds( bool b )
    {
        _hasTransparentBounds = b;

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    [Obsolete]
    public Matcher UseAnchoringBounds( bool b )
    {
        _hasAnchoringBounds = b;

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    [Obsolete]
    private bool Search( int from )
    {
        this.HitEnd     = false;
        this.RequireEnd = false;
        _regionStart    = from < 0 ? 0 : from;
        this.first      = from;
        this._oldLast   = _oldLast < 0 ? from : _oldLast;

        for ( var i = 0; i < groups.Length; i++ )
        {
            groups[ i ] = -1;
        }

        _acceptMode = NoAnchor;

        bool result = parentPattern.root.Match( this, from, _text );

        if ( !result )
        {
            this.first = -1;
        }

        this._oldLast = this.last;

        return result;
    }

    /// <summary>
    /// Initiates a search for an anchored match to a Pattern within
    /// the given bounds. The groups are filled with default values
    /// and the match of the root of the state machine is called. The
    /// state machine will hold the state of the match as it proceeds
    /// in this matcher.
    /// </summary>
    [Obsolete]
    private bool Match( int from, int anchor )
    {
        this.HitEnd     = false;
        this.RequireEnd = false;

        from = from < 0 ? 0 : from;

        this.first    = from;
        this._oldLast = _oldLast < 0 ? from : _oldLast;

        for ( var i = 0; i < groups.Length; i++ )
        {
            groups[ i ] = -1;
        }

        _acceptMode = anchor;

        bool result = parentPattern.matchRoot.Match( this, from, _text );

        if ( !result )
        {
            this.first = -1;
        }

        this._oldLast = this.last;

        return result;
    }

    /// <summary>
    /// Returns the group index of the matched capturing group.
    /// </summary>
    [Obsolete]
    private int GetMatchedGroupIndex( string name )
    {
        if ( name == null ) throw new GdxRuntimeException( "Group Name must not be null!" );

        if ( first < 0 ) throw new IllegalStateException( "No match found" );

        if ( !parentPattern.NamedGroups().ContainsKey( name ) )
        {
            throw new ArgumentException( "No group with name <" + name + ">" );
        }

        return parentPattern.NamedGroups()[ name ];
    }

    /// <summary>
    /// Generates a String from this Matcher's input in the specified range.
    /// </summary>
    /// <param name="beginIndex">the beginning index, inclusive</param>
    /// <param name="endIndex">the ending index, exclusive</param>
    /// <returns>A String generated from this Matcher's input</returns>
    [Obsolete]
    private string GetSubSequence( int beginIndex, int endIndex )
    {
        return _text.Substring( beginIndex, endIndex - beginIndex );
    }

    [Obsolete]
    public new string ToString()
    {
        var sb = new StringBuilder();

        sb.Append( "Matcher" );
        sb.Append( $"[pattern={Pattern()}" );
        sb.Append( " region=" );
        sb.Append( $"{_regionStart},{_regionEnd}" );
        sb.Append( " lastmatch=" );

        if ( ( first >= 0 ) && ( Group() != null ) )
        {
            sb.Append( Group() );
        }

        sb.Append( ']' );

        return sb.ToString();
    }
}
