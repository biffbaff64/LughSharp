// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace Corelib.Lugh.Maths;

[PublicAPI]
public class RandomXS128 : Random
{
    /// <summary>
    /// Normalization constant for double.
    /// </summary>
    private const double NORM_DOUBLE = 1.0 / ( 1L << 53 );

    /// <summary>
    /// Normalization constant for float.
    /// </summary>
    private const double NORM_FLOAT = 1.0 / ( 1L << 24 );

    /// <summary>
    /// The first half of the internal state of this pseudo-random number generator.
    /// </summary>
    private long _seed0;

    /// <summary>
    /// The second half of the internal state of this pseudo-random number generator.
    /// </summary>
    private long _seed1;

    /// <summary>
    /// Creates a new random number generator. This constructor sets the seed of the
    /// random number generator to a value very likely to be distinct from any other
    /// invocation of this constructor.
    /// <para>
    /// This implementation creates a <see cref="System.Random"/> instance to generate the initial seed.
    /// </para>
    /// </summary>
    public RandomXS128() : base( new Random().Next() )
    {
    }

    /// <summary>
    /// Creates a new random number generator using a single long seed.
    /// </summary>
    /// <param name="seed"> the initial seed  </param>
    public RandomXS128( int seed ) : base( seed )
    {
    }

    /// <summary>
    /// Creates a new random number generator using two long seeds.
    /// </summary>
    /// <param name="seed0"> the first part of the initial seed </param>
    /// <param name="seed1"> the second part of the initial seed  </param>
    public RandomXS128( long seed0, long seed1 )
    {
        SetState( seed0, seed1 );
    }

    /// <summary>
    /// Sets the internal seed of this generator based on the given value.
    /// <para>
    /// The given seed is passed twice through a hash function. This way,
    /// if the user passes a small value we avoid the short irregular
    /// transient associated with states having a very small number of bits set.
    /// </para>
    /// </summary>
    /// <param name="value">
    /// a nonzero seed for this generator (if zero, the generator will be seeded
    /// with <see cref="long.MinValue"/>).
    /// </param>
    public long Seed
    {
        set
        {
            var seed0 = MurmurHash3( value == 0 ? long.MinValue : value );
            SetState( seed0, MurmurHash3( seed0 ) );
        }
    }

    /// <summary>
    /// Returns the next pseudo-random, uniformly distributed long value
    /// from this random number generator's sequence.
    /// <para>
    /// Subclasses should override this, as this is used by all other methods.
    /// </para>
    /// </summary>
    public long NextLong()
    {
        var s1 = _seed0;
        var s0 = _seed1;

        _seed0 = s0;

        s1 ^= s1 << 23;

        return ( _seed1 = s1 ^ s0 ^ ( s1 >>> 17 ) ^ ( s0 >>> 26 ) ) + s0;
    }

    /// <summary>
    /// This protected method is final because, contrary to the superclass,
    /// it's not used anymore by the other methods.
    /// </summary>
    public override int Next( int bits )
    {
        return ( int ) ( NextLong() & ( ( 1L << bits ) - 1 ) );
    }

    /// <summary>
    /// Returns the next pseudo-random, uniformly distributed int value from
    /// this random number generator's sequence.
    /// <para>
    /// This implementation uses <see cref="NextLong()"/> internally.
    /// </para>
    /// </summary>
    public int NextInt()
    {
        return ( int ) NextLong();
    }

    /// <summary>
    /// Returns a pseudo-random, uniformly distributed int value between 0
    /// (inclusive) and the specified value (exclusive), drawn from this
    /// random number generator's sequence.
    /// <para>
    /// This implementation uses <see cref="NextLong()"/> internally.
    /// </para>
    /// </summary>
    /// <param name="n"> the positive bound on the random number to be returned. </param>
    /// <returns>
    /// the next pseudo-random int value between 0 (inclusive) and n (exclusive).
    /// </returns>
    public int NextInt( in int n )
    {
        return ( int ) NextLong( n );
    }

    /// <summary>
    /// Returns a pseudo-random, uniformly distributed long value between 0
    /// (inclusive) and the specified value (exclusive), drawn from this
    /// random number generator's sequence. The algorithm used to generate
    /// the value guarantees that the result is uniform, provided that the
    /// sequence of 64-bit values produced by this generator is.
    /// <para>
    /// This implementation uses <see cref="NextLong()"/> internally.
    /// </para>
    /// </summary>
    /// <param name="n"> the positive bound on the random number to be returned.</param>
    /// <returns>
    /// the next pseudo-random long value between 0 (inclusive) and n (exclusive).
    /// </returns>
    public long NextLong( in long n )
    {
        if ( n <= 0 )
        {
            throw new ArgumentException( "n must be positive" );
        }

        for ( ;; )
        {
            var bits  = NextLong() >>> 1;
            var value = bits % n;

            if ( ( ( bits - value ) + ( n - 1 ) ) >= 0 )
            {
                return value;
            }
        }
    }

    /// <summary>
    /// Returns a pseudo-random, uniformly distributed double value between
    /// 0.0 and 1.0 from this random number generator's sequence.
    /// <para>
    /// This implementation uses <see cref="NextLong()"/> internally.
    /// </para>
    /// </summary>
    public override double NextDouble()
    {
        return ( NextLong() >>> 11 ) * NORM_DOUBLE;
    }

    /// <summary>
    /// Returns a pseudo-random, uniformly distributed {@code float} value
    /// between 0.0 and 1.0 from this random number generator's sequence.
    /// <para>
    /// This implementation uses <see cref="NextLong()"/> internally.
    /// </para>
    /// </summary>
    public float NextFloat()
    {
        return ( float ) ( ( NextLong() >> 40 ) * NORM_FLOAT );
    }

    /// <summary>
    /// Returns a pseudo-random, uniformly distributed bool value from
    /// this random number generator's sequence.
    /// <para>
    /// This implementation uses <see cref="NextLong()"/> internally.
    /// </para>
    /// </summary>
    public bool Nextbool()
    {
        return ( NextLong() & 1 ) != 0;
    }

    /// <summary>
    /// Generates random bytes and places them into a user-supplied byte array.
    /// The number of random bytes produced is equal to the length of the byte
    /// array.
    /// <para>
    /// This implementation uses <see cref="NextLong()"/> internally.
    /// </para>
    /// </summary>
    public void NextBytes( in sbyte[] bytes )
    {
        var i = bytes.Length;

        while ( i != 0 )
        {
            var n = i < 8 ? i : 8;

            for ( var bits = NextLong(); n-- != 0; bits >>= 8 )
            {
                bytes[ --i ] = ( sbyte ) bits;
            }
        }
    }

    /// <summary>
    /// Sets the internal state of this generator.
    /// </summary>
    /// <param name="seed0"> the first part of the internal state </param>
    /// <param name="seed1"> the second part of the internal state  </param>
    public void SetState( in long seed0, in long seed1 )
    {
        _seed0 = seed0;
        _seed1 = seed1;
    }

    /// <summary>
    /// Returns the internal seeds to allow state saving.
    /// </summary>
    /// <param name="seed">must be 0 or 1, designating which of the 2 long seeds to return</param>
    /// <returns> the internal seed that can be used in setState </returns>
    public virtual long GetState( int seed )
    {
        return seed == 0 ? _seed0 : _seed1;
    }

    private static long MurmurHash3( long x )
    {
        x ^= x >>> 33;
        x *= unchecked( ( long ) 0xff51afd7ed558ccdL );
        x ^= x >>> 33;
        x *= unchecked( ( long ) 0xc4ceb9fe1a85ec53L );
        x ^= x >>> 33;

        return x;
    }
}
