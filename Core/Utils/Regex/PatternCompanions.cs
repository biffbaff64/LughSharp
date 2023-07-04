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

using System.Diagnostics;

namespace LibGDXSharp.Utils.Regex;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed partial class Pattern
{

     #region CompanionClasses

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Base class for all node classes. Subclasses should override the match()
    /// method as appropriate. This class is an accepting node, so its match()
    /// always returns true.
    /// </summary>
    [Obsolete]
    public class Node : object
    {
        public readonly Node? next;

        [Obsolete]
        public Node()
        {
//            next = Pattern.Accept;
        }

        /// <summary>
        /// This method implements the classic accept node.
        /// </summary>
        [Obsolete]
        public bool Match( Matcher matcher, int i, string seq )
        {
            matcher.last        = i;
            matcher.groups[ 0 ] = matcher.first;
            matcher.groups[ 1 ] = matcher.last;

            return true;
        }

        /// <summary>
        /// This method is good for all zero length assertions.
        /// </summary>
        [Obsolete]
        public bool Study( TreeInfo info )
        {
            if ( next != null )
            {
                return next.Study( info );
            }

            return info.Deterministic;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class LastNode : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Used to accumulate information about a subtree of the object graph
    /// so that optimizations can be applied to the subtree.
    /// </summary>
    [Obsolete]
    public class TreeInfo
    {
        public int  MinLength     { get; set; }
        public int  MaxLength     { get; set; }
        public bool MaxValid      { get; set; }
        public bool Deterministic { get; set; }

        [Obsolete]
        public TreeInfo()
        {
            Reset();
        }

        [Obsolete]
        public void Reset()
        {
            MinLength     = 0;
            MaxLength     = 0;
            MaxValid      = true;
            Deterministic = true;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a bit vector for matching Latin-1 values. A normal BitClass
    /// never matches values above Latin-1, and a complemented BitClass always
    /// matches values above Latin-1.
    /// </summary>
    [Obsolete]
    public class BitClass : BmpCharProperty
    {
        private bool[] _bits;

        [Obsolete]
        public BitClass()
        {
            _bits = new bool[ 256 ];
        }

        [Obsolete]
        public BitClass( bool[] bits )
        {
            this._bits = bits;
        }

        [Obsolete]
        public BitClass Add( int c, int flags )
        {
            Debug.Assert( c is >= 0 and <= 255 );

            if ( ( flags & Case_Insensitive ) != 0 )
            {
                if ( ASCII.IsAscii( c ) )
                {
                    _bits[ ASCII.ToUpper( c ) ] = true;
                    _bits[ ASCII.ToLower( c ) ] = true;
                }
                else if ( ( flags & Unicode_Case ) != 0 )
                {
                    _bits[ char.ToLower( (char)c ) ] = true;
                    _bits[ char.ToUpper( (char)c ) ] = true;
                }
            }

            _bits[ c ] = true;

            return this;
        }

        [Obsolete]
        public bool IsSatisfiedBy( int ch )
        {
            return ( ch < 256 ) && _bits[ ch ];
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Start : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class StartS : Start
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Begin : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class End : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Caret : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class UnixCaret : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class LastMatch : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Dollar : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class UnixDollar : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class LineEnding : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class CharProperty : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class BmpCharProperty : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SingleS : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Single : BmpCharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SingleI : BmpCharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SingleU : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Block : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Script : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Category : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class UType : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class CType : BmpCharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class VertWS : BmpCharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class HorizWS : BmpCharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SliceNode : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Slice : SliceNode
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SliceI : SliceNode
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SliceU : SliceNode
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SliceS : SliceNode
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SliceIS : SliceNode
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class SliceUS : SliceIS
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class All : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Dot : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class UnixDot : CharProperty
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Ques : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Curly : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class GroupCurly : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class BranchConn : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Branch : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// The GroupHead saves the location where the group begins in the locals and
    /// restores them when the match is done. The matchRef is used when a reference
    /// to this group is accessed later in the expression. The locals will have a
    /// negative value in them to indicate that we do not want to unset the group
    /// if the reference doesn't match.
    /// </summary>
    [Obsolete]
    public class GroupHead : Node
    {
        private int _localIndex;

        [Obsolete]
        public GroupHead( int localCount )
        {
            _localIndex = localCount;
        }

        [Obsolete]
        public new bool Match( Matcher matcher, int i, string seq )
        {
            int save = matcher.locals[ _localIndex ];

            matcher.locals[ _localIndex ] = i;

            bool ret = next.Match( matcher, i, seq );

            matcher.locals[ _localIndex ] = save;

            return ret;
        }

        [Obsolete]
        public bool MatchRef( Matcher matcher, int i, string seq )
        {
            int save = matcher.locals[ _localIndex ];

            matcher.locals[ _localIndex ] = ~i; // HACK

            bool ret = next.Match( matcher, i, seq );

            matcher.locals[ _localIndex ] = save;

            return ret;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class GroupRef : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class GroupTail : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Prolog : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Loop : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class LazyLoop : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class BackRef : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class CIBackRef : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class First : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Conditional : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Pos : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Neg : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Behind : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class BehindS : Behind
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class NotBehind : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class NotBehindS : NotBehind
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class Bound : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class BnM : Node
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class BnMS : BnM
    {
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [Obsolete]
    public class CharPropertyNames
    {
    }

        #endregion

}