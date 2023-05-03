namespace LibGDXSharp.Utils.Regex
{
    public class Matcher : IMatchResult
    {
        // The Pattern object that created this Matcher.
        public Pattern parentPattern;

        // The storage used by groups. They may contain invalid values
        // if a group was skipped during the matching.
        public int[] groups;

        // The range within the sequence that is to be matched.
        // Anchors will match at these "hard" boundaries. Changing the
        // region changes these values.
        private int _from;
        private int _to;

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

        private int acceptMode = NoAnchor;

        // The range of string that last matched the pattern. If the last
        // match failed then first is -1; last initially holds 0 then it
        // holds the index of the end of the last match (which is where
        // the next search starts).
        public int first = -1;
        public int last  = 0;

        // The end index of what matched in the last match operation.
        private int _oldLast = -1;

        // The index of the last position appended in a substitution.
        private int _lastAppendPosition = 0;

        /// <summary>
        /// Storage used by nodes to tell what repetition they are on in
        /// a pattern, and where groups begin. The nodes themselves are stateless,
        /// so they rely on this field to hold state during a match.
        /// </summary>
        public int[] locals;

        /// <summary>
        /// Boolean indicating whether or not more input could change
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
        private bool _hitEnd;

        /// <summary>
        /// Boolean indicating whether or not more input could change
        /// a positive match into a negative one.
        /// 
        /// If requireEnd is true, and a match was found, then more
        /// input could cause the match to be lost.
        /// If requireEnd is false and a match was found, then more
        /// input might change the match but the match won't be lost.
        /// If a match was not found, then requireEnd has no meaning.
        /// </summary>
        private bool _requireEnd;

        /// <summary>
        /// If transparentBounds is true then the boundaries of this
        /// matcher's region are transparent to lookahead, lookbehind,
        /// and boundary matching constructs that try to see beyond them.
        /// </summary>
        private bool _transparentBounds = false;

        /// <summary>
        /// If anchoringBounds is true then the boundaries of this
        /// matcher's region match anchors such as ^ and $.
        /// </summary>
        private bool _anchoringBounds = true;

        public Matcher( Pattern parent, string text )
        {
            parentPattern = parent;
            _text         = text;

            var parentGroupCount = Math.Max( parent.CapturingGroupCount, 10 );

            groups = new int[ parentGroupCount * 2 ];
            locals = new int[ parent.LocalCount ];

            Reset();
        }

        /// <summary>
        /// Returns the match state of this matcher as a MatchResult.
        /// The result is unaffected by subsequent operations performed
        /// upon this matcher.
        /// </summary>
        /// <returns></returns>
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
        public Matcher UsePattern( Pattern? newPattern )
        {
            parentPattern = newPattern ?? throw new ArgumentException( "Pattern cannot be null" );

            // Reallocate state storage
            int parentGroupCount = Math.Max( newPattern.CapturingGroupCount, 10 );
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
            _from               = 0;
            _to                 = GetTextLength();

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
        public Matcher Reset( string input )
        {
            _text = input;

            return Reset();
        }

        /// <summary>
        /// Returns the start index of the previous match.
        /// </summary>
        public int Start()
        {
            if ( first < 0 )
            {
                throw new IllegalStateException( "No match available" );
            }

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
        public int Start( int group )
        {
            if ( first < 0 )
            {
                throw new IllegalStateException( "No match available" );
            }

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
        public virtual int Start( string name )
        {
            return groups[ GetMatchedGroupIndex( name ) * 2 ];
        }

        /// <summary>
        /// Returns the offset after the last character matched.
        /// </summary>
        public int End()
        {
            if ( first < 0 )
            {
                throw new IllegalStateException( "No match available" );
            }

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
        public int End( int group )
        {
            if ( first < 0 )
            {
                throw new IllegalStateException( "No match available" );
            }

            if ( ( group < 0 ) || ( group > GroupCount() ) )
            {
                throw new IndexOutOfRangeException( "No group " + group );
            }

            return groups[ ( group * 2 ) + 1 ];
        }
    }
}
