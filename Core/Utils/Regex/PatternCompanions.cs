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
using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils.Regex;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed partial class Pattern
{
     #region CompanionClasses

    /// <summary>
    /// Base class for all node classes. Subclasses should override the match()
    /// method as appropriate. This class is an accepting node, so its match()
    /// always returns true.
    /// </summary>
    public class Node : object
    {
        public readonly Node? next;

        public Node()
        {
            next = Pattern.Accept;
        }

        /// <summary>
        /// This method implements the classic accept node.
        /// </summary>
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
        public bool Study( TreeInfo info )
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

    public class LastNode : Node
    {
    }

    public class TreeInfo
    {
    }

    /**
     *  Creates a bit vector for matching Latin-1 values. A normal BitClass
     *  never matches values above Latin-1, and a complemented BitClass always
     *  matches values above Latin-1.
     */
    public class BitClass : BmpCharProperty
    {
        private bool[] _bits;

        public BitClass()
        {
            _bits = new bool[ 256 ];
        }

        public BitClass( bool[] bits )
        {
            this._bits = bits;
        }

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
                    _bits[ Character.toLowerCase( c ) ] = true;
                    _bits[ Character.toUpperCase( c ) ] = true;
                }
            }

            _bits[ c ] = true;

            return this;
        }

        bool isSatisfiedBy( int ch )
        {
            return ch < 256 && _bits[ ch ];
        }
    }

    public class Start : Node
    {
    }

    public class StartS : Start
    {
    }

    public class Begin : Node
    {
    }

    public class End : Node
    {
    }

    public class Caret : Node
    {
    }

    public class UnixCaret : Node
    {
    }

    public class LastMatch : Node
    {
    }

    public class Dollar : Node
    {
    }

    public class UnixDollar : Node
    {
    }

    public class LineEnding : Node
    {
    }

    public class CharProperty : Node
    {
    }

    public class BmpCharProperty : CharProperty
    {
    }

    public class SingleS : CharProperty
    {
    }

    public class Single : BmpCharProperty
    {
    }

    public class SingleI : BmpCharProperty
    {
    }

    public class SingleU : CharProperty
    {
    }

    public class Block : CharProperty
    {
    }

    public class Script : CharProperty
    {
    }

    public class Category : CharProperty
    {
    }

    public class UType : CharProperty
    {
    }

    public class CType : BmpCharProperty
    {
    }

    public class VertWS : BmpCharProperty
    {
    }

    public class HorizWS : BmpCharProperty
    {
    }

    public class SliceNode : Node
    {
    }

    public class Slice : SliceNode
    {
    }

    public class SliceI : SliceNode
    {
    }

    public class SliceU : SliceNode
    {
    }

    public class SliceS : SliceNode
    {
    }

    public class SliceIS : SliceNode
    {
    }

    public class SliceUS : SliceIS
    {
    }

    public class All : CharProperty
    {
    }

    public class Dot : CharProperty
    {
    }

    public class UnixDot : CharProperty
    {
    }

    public class Ques : Node
    {
    }

    public class Curly : Node
    {
    }

    public class GroupCurly : Node
    {
    }

    public class BranchConn : Node
    {
    }

    public class Branch : Node
    {
    }

    public class GroupHead : Node
    {
    }

    public class GroupRef : Node
    {
    }

    public class GroupTail : Node
    {
    }

    public class Prolog : Node
    {
    }

    public class Loop : Node
    {
    }

    public class LazyLoop : Node
    {
    }

    public class BackRef : Node
    {
    }

    public class CIBackRef : Node
    {
    }

    public class First : Node
    {
    }

    public class Conditional : Node
    {
    }

    public class Pos : Node
    {
    }

    public class Neg : Node
    {
    }

    public class Behind : Node
    {
    }

    public class BehindS : Behind
    {
    }

    public class NotBehind : Node
    {
    }

    public class NotBehindS : NotBehind
    {
    }

    public class Bound : Node
    {
    }

    public class BnM : Node
    {
    }

    public class BnMS : BnM
    {
    }

    public class CharPropertyNames
    {
    }

        #endregion

}