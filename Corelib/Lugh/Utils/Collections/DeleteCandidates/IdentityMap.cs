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


namespace Corelib.Lugh.Utils.Collections.DeleteCandidates;

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
/// (see
/// <a
///     href="https://probablydance.com/2018/06/16/fibonacci-hashing-the-optimization-that-the-world-forgot-or-a-better-alternative-to-integer-modulo/">
/// Malte Skarupke's blog post
/// </a>
/// ).
/// Linear probing continues to work even when all hashCodes collide, just more slowly.
/// </para>
/// </summary>
[PublicAPI, Obsolete( "Obsolete" )]
public class IdentityMap< TK, TV > : ObjectMap< TK, TV > where TK : notnull
{
    private readonly ObjectIDGenerator _objectIDGenerator = new();

    private bool _firstPlaceGen = true;

    // ========================================================================

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
    public IdentityMap( ObjectMap< TK, TV > map ) : base( map )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>

    //TODO: I have a feeling this method is badly named. Once I've worked out what it does I'll rename it.
    protected override int Place( TK item )
    {
        var id = ( ulong ) _objectIDGenerator.GetId( item, out _firstPlaceGen );

        return ( int ) ( ( id * 0x9E3779B97F4A7C15L ) >>> Shift );
    }

    public int LocateKey( TK key )
    {
        ArgumentNullException.ThrowIfNull( key );

        TK?[] keytab = KeyTable;

        for ( var i = Place( key );; i = ( i + 1 ) & Mask )
        {
            var other = keytab[ i ];

            if ( other == null )
            {
                // Empty space is available.
                return -( i + 1 );
            }

            if ( Equals( other, key ) )
            {
                // Same key was found.
                return i;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        const int PRIME = 53;

        var result = PRIME + 31;
        result = ( PRIME * result ) + 32;

        return result;
    }
}
