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

using System.Runtime.Serialization;

using LibGDXSharp.Core.Utils.Collections.Extensions;

namespace LibGDXSharp.Utils.Collections;

/// <summary>
/// An unordered map that uses identity comparison for the object keys.
/// Null keys are not allowed.
/// No allocation is done except when growing the table size.
/// <para>
/// This class performs fast contains and remove (typically O(1), worst case O(n) but
/// that is rare in practice). Add may be slightly slower, depending on hash collisions.
/// Hashcodes are rehashed to reduce collisions and the need to resize. Load factors
/// greater than 0.91 greatly increase the chances to resize to the next higher POT size.
/// </para>
/// <para>
/// Unordered sets and maps are not designed to provide especially fast iteration.
/// Iteration is faster with OrderedSet and OrderedMap.
/// </para>
/// <para>
/// This implementation uses linear probing with the backward shift algorithm for removal.
/// Hashcodes are rehashed using Fibonacci hashing, instead of the more common power-of-two
/// mask, to better distribute poor hashCodes
/// </para>
/// <para>
/// (see <a href="https://probablydance.com/2018/06/16/fibonacci-hashing-the-optimization-that-the-world-forgot-or-a-better-alternative-to-integer-modulo/">
/// Malte Skarupke's blog post</a>). Linear probing continues to work even when all hashCodes
/// collide, just more slowly.
/// </para>
/// </summary>
public class IdentityMap<TK, TV> : ObjectMap< TK, TV > where TK : notnull
{
    private readonly ObjectIDGenerator _objectIDGenerator = new();

    private bool _firstTimeGen  = true;
    private bool _firstPlaceGen = true;

    /// <summary>
    /// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
    /// </summary>
    public IdentityMap()
    {
    }

    /// <summary>
    /// Creates a new map with a load factor of 0.8.
    /// </summary>
    /// <param name="initialCapacity">
    /// If not a power of two, it is increased to the next nearest power of two.
    /// </param>
    public IdentityMap( int initialCapacity ) : base( initialCapacity )
    {
    }

    /// <summary>
    /// Creates a new map with the specified initial capacity and load factor.
    /// This map will hold initialCapacity items before growing the backing table.
    /// </summary>
    /// <param name="initialCapacity">
    /// If not a power of two, it is increased to the next nearest power of two.
    /// </param>
    /// <param name="loadFactor"></param>
    public IdentityMap( int initialCapacity, float loadFactor )
        : base( initialCapacity, loadFactor )
    {
    }

    /// <summary>
    /// Creates a new map identical to the specified map.
    /// </summary>
    public IdentityMap( IdentityMap< TK, TV > map ) : base( map )
    {
    }

    protected new int Place( TK item )
    {
        var id = ( ulong )_objectIDGenerator.GetId( item, out _firstPlaceGen );

        return ( int )( ( id * 0x9E3779B97F4A7C15L ) >>> Shift );
    }

    public int LocateKey( TK key )
    {
        if ( key == null ) throw new ArgumentException( "key cannot be null." );

        TK?[] keytab = this.keyTable;

        for ( var i = Place( key );; i = ( i + 1 ) & Mask )
        {
            TK? other = keytab[ i ];

            if ( other == null ) return -( i + 1 ); // Empty space is available.
            if ( Equals( other, key ) ) return i;   // Same key was found.
        }
    }

    public int HashCode()
    {
        var   h        = Size;
        TK?[] keytab   = this.keyTable;
        TV?[] valuetab = this.valueTable;

        for ( int i = 0, n = keytab.Length; i < n; i++ )
        {
            TK? key = keytab[ i ];

            if ( key != null )
            {
                h += ( int )_objectIDGenerator.GetId( this, out _firstTimeGen );

                TV? value              = valuetab[ i ];
                if ( value != null ) h += value.GetHashCode();
            }
        }

        return h;
    }
}