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

using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils.Exceptions;
using Exception = System.Exception;

namespace LughSharp.Lugh.Utils.Buffers;

[PublicAPI]
public abstract class ShortBuffer : Buffer
{
    public new short[]? Hb { get; set; }

    // ========================================================================

    /// <summary>
    /// Creates a new buffer with the given mark, position, limit,
    /// capacity, backing array, and array offset
    /// </summary>
    protected ShortBuffer( int mark, int pos, int lim, int cap, short[]? hb = null, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
        Hb     = hb ?? new short[ cap ];
        Offset = offset;
    }

    /// <summary>
    /// Allocates a new short buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its
    /// capacity, its mark will be undefined, and each of its elements will be
    /// initialized to zero.  It will have a backing array (<see cref="List{T}"/>),
    /// and its <see cref="ArrayOffset"/> will be zero.
    /// </para>
    /// </summary>
    /// <param name="capacity"> The new buffer's capacity, in shorts </param>
    /// <returns> The new short buffer </returns>
    /// <exception cref="ArgumentException">
    /// If the <tt>capacity</tt> is a negative integer
    /// </exception>
    public static ShortBuffer Allocate( int capacity )
    {
        if ( capacity < 0 )
        {
            throw new ArgumentException();
        }

        return new HeapShortBuffer( capacity, capacity );
    }

    /// <summary>
    /// Wraps a short array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given short array;
    /// that is, modifications to the buffer will cause the array to be modified
    /// and vice versa.  The new buffer's capacity will be
    /// <tt>array.length</tt>, its position will be <tt>offset</tt>, its limit
    /// will be <tt>offset + length</tt>, and its mark will be undefined.  Its
    /// backing array will be the given array, and its array offset will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back the new buffer </param>
    /// <param name="offset">
    /// The offset of the subarray to be used; must be non-negative and
    /// no larger than <tt>array.length</tt>. The new buffer's position
    /// will be set to this value.
    /// </param>
    /// <param name="length">
    /// The length of the subarray to be used; must be non-negative and no larger
    /// than <tt>array.length - offset</tt>. The new buffer's limit will be set to
    /// <tt>offset + length</tt>.
    /// </param>
    /// <returns> The new short buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt> parameters do not hold
    /// </exception>
    public static ShortBuffer Wrap( short[] array, int offset, int length )
    {
        try
        {
            return new HeapShortBuffer( array, offset, length );
        }
        catch ( Exception )
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Wraps a short array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given short array; that is, modifications
    /// to the buffer will cause the array to be modified and vice versa. The new buffer's
    /// capacity and limit will be <tt>array.length</tt>, its position will be zero, and
    /// its mark will be undefined.  Its backing array will be the given array, and its
    /// array offset will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back this buffer </param>
    /// <returns> The new short buffer </returns>
    public static ShortBuffer Wrap( short[] array )
    {
        return Wrap( array, 0, array.Length );
    }

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// <para>
    /// This method transfers shorts from this buffer into the given destination
    /// array. If there are fewer shorts remaining in the buffer than are required
    /// to satisfy the request, that is if <paramref name="length"/> is greater than
    /// <see cref="Buffer.Remaining()"/>, then no shorts are transferred, and a
    /// <see cref="GdxRuntimeException"/> is thrown.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <paramref name="length"/> shorts from this buffer
    /// into the given array, starting at the current position of this buffer and at the
    /// given offset in the array. The position of this buffer is then incremented by
    /// <paramref name="length"/>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form <tt>src.get(dst, off, len)</tt>
    /// has exactly the same effect as the loop
    /// <code>
    ///     for (int i = off; i &lt; off + len; i++)
    ///     {
    ///         dst[i] = src.get();
    ///     }
    /// </code>
    /// except that it first checks that there are sufficient shorts in this buffer and it
    /// is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="dst">The array into which shorts are to be written.</param>
    /// <param name="offset">
    /// The offset within the array of the first short to be written; must be non-negative
    /// and no larger than <see cref="Array.Length"/>.
    /// </param>
    /// <param name="length">
    /// The maximum number of shorts to be written to the given array; must be non-negative
    /// and no larger than <c>dst.Length - offset</c>.
    /// </param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than <paramref name="length"/> shorts remaining in this buffer.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <paramref name="offset"/> and <paramref name="length"/>
    /// parameters do not hold.
    /// </exception>
    public virtual ShortBuffer Get( short[] dst, int offset, int length )
    {
        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Underflow!" );
        }

        var end = offset + length;

        for ( var i = offset; i < end; i++ )
        {
            dst[ i ] = Get();
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// <para>
    /// This method transfers shorts from this buffer into the given destination
    /// array. An invocation of this method of the form <tt>src.get(a)</tt> behaves
    /// in exactly the same way as the invocation
    /// <code>
    ///     src.get(a, 0, a.Length);
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="dst">
    /// The destination array.
    /// </param>
    /// <returns> This buffer. </returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than <paramref name="dst.length"/> shorts
    /// remaining in this buffer.
    /// </exception>
    public virtual ShortBuffer Get( short[] dst )
    {
        return Get( dst, 0, dst.Length );
    }

    /// <summary>
    /// Relative bulk <i>put</i> method <i>(optional operation)</i>.
    /// <para>
    /// This method transfers the shorts remaining in the given source
    /// buffer into this buffer. If there are more shorts remaining in the
    /// source buffer than in this buffer, that is, if src.remaining() > remaining(),
    /// then no shorts are transferred and a BufferOverflowException is thrown.
    /// </para>
    /// <para>
    /// Otherwise, this method copies n = src.remaining() shorts from the given
    /// buffer into this buffer, starting at each buffer's current position.
    /// The positions of both buffers are then incremented by n.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form dst.put(src) has
    /// exactly the same effect as the loop
    /// </para>
    /// <code>
    ///     while (src.hasRemaining())
    ///     {
    ///         dst.put(src.get());
    ///     }
    /// </code>
    /// <para>
    /// except that it first checks that there is sufficient space in this
    /// buffer and it is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="src">
    /// The source buffer from which shorts are to be read;
    /// must not be this buffer.
    /// </param>
    /// <returns> This buffer. </returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer for the remaining shorts in the source buffer.
    /// </exception>
    /// <exception cref="ArgumentException">If the source buffer is this buffer.</exception>
    /// <exception cref="GdxRuntimeException">If this buffer is read-only.</exception>
    public virtual ShortBuffer Put( ShortBuffer src )
    {
        if ( Equals( src, this ) )
        {
            throw new ArgumentException();
        }

        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Buffer is Read Only!" );
        }

        var n = src.Remaining();

        if ( n > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Overflow!" );
        }

        for ( var i = 0; i < n; i++ )
        {
            Put( src.Get() );
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// This method transfers shorts into this buffer from the given
    /// source array.  If there are more shorts to be copied from the array
    /// than remain in this buffer, that is, if
    /// <tt>length</tt> <tt>&gt;</tt> <tt>remaining()</tt>, then no
    /// shorts are transferred and a {@link BufferOverflowException} is
    /// thrown.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <tt>length</tt> shorts from the
    /// given array into this buffer, starting at the given offset in the array
    /// and at the current position of this buffer.  The position of this buffer
    /// is then incremented by <tt>length</tt>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form
    /// <tt>dst.put(src, off, len)</tt> has exactly the same effect as
    /// the loop
    /// <code>
    ///     for (int i = off; i &lt; off + len; i++)
    ///     {
    ///         dst.put( a[ i ] );
    ///     }
    /// </code>
    /// except that it first checks that there is sufficient space in this
    /// buffer and it is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="src">The array from which shorts are to be read</param>
    /// <param name="offset">
    /// The offset within the array of the first short to be read;
    /// must be non-negative and no larger than <tt>array.length</tt>
    /// </param>
    /// <param name="length">
    /// The number of shorts to be read from the given array;
    /// must be non-negative and no larger than
    /// <tt>array.length - offset</tt>
    /// </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt>
    /// parameters do not hold
    /// </exception>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is read-only
    /// </exception>
    public virtual ShortBuffer Put( short[] src, int offset, int length )
    {
        CheckBounds( offset, length, src.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Overflow!" );
        }

        var end = offset + length;

        for ( var i = offset; i < end; i++ )
        {
            Put( src[ i ] );
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// This method transfers the entire content of the given source
    /// short array into this buffer.  An invocation of this method of the
    /// form <tt>dst.put(a)</tt> behaves in exactly the same way as the
    /// invocation :-
    /// <code> dst.Put(a, 0, a.length) </code>
    /// </para>
    /// </summary>
    /// <param name="src"> The source array </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer
    /// </exception>
    /// <exception crewf="GdxRuntimeException">
    /// If this buffer is read-only
    /// </exception>
    public virtual ShortBuffer Put( short[] src )
    {
        return Put( src, 0, src.Length );
    }

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible short
    /// array.
    /// <para>
    /// If this method returns <tt>true</tt> then the <see cref="BackingArray"/>
    /// and <see cref="ArrayOffset()"/> methods may safely be invoked.
    /// </para>
    /// </summary>
    /// <returns>
    /// <tt>true</tt> if, and only if, this buffer is backed by an array and is not read-only
    /// </returns>
    public override bool HasBackingArray()
    {
        return ( Hb != null ) && !IsReadOnly;
    }

    /// <summary>
    /// Returns the short array that backs this buffer <i>(optional operation)</i>.
    /// <para>
    /// Modifications to this buffer's content will cause the returned
    /// array's content to be modified, and vice versa.
    /// </para>
    /// <para>
    /// Invoke the {@link #hasArray hasArray} method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.
    /// </para>
    /// </summary>
    /// <returns> The array that backs this buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is not backed by an accessible array
    /// </exception>
    public new short[] BackingArray()
    {
        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Buffer is Read Only!" );
        }

        return Hb!;
    }

    /// <summary>
    /// Returns the offset within this buffer's backing array of the first
    /// element of the buffer  <i>(optional operation)</i>.
    /// <para>
    /// If this buffer is backed by an array then buffer position <i>p</i>
    /// corresponds to array index <i>p</i> + <tt>arrayOffset()</tt>.
    /// </para>
    /// <para>
    /// Invoke the <see cref="HasBackingArray"/> method before invoking this method
    /// in order to ensure that this buffer has an accessible backing array.
    /// </para>
    /// </summary>
    /// <returns>
    /// The offset within this buffer's array of the first element of the buffer
    /// </returns>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is not backed by an accessible array
    /// </exception>
    public override int ArrayOffset()
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Backing array os null." );
        }

        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Buffer is Read Only!" );
        }

        return Offset;
    }

    // ========================================================================

    #region abstract methods

    /// <summary>
    /// Creates a new short buffer whose content is a shared subsequence of
    /// this buffer's content.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of shorts remaining in this buffer, and its mark
    /// will be undefined.  The new buffer will be direct if, and only if, this
    /// buffer is direct, and it will be read-only if, and only if, this buffer
    /// is read-only.
    /// </para>
    /// </summary>
    /// @return  The new short buffer
    public abstract ShortBuffer Slice();

    /// <summary>
    /// Creates a new short buffer that shares this buffer's content.
    /// </summary>
    /// <remarks>
    /// The content of the new buffer will be that of this buffer.
    /// Changes to this buffer's content will be visible in the new buffer,
    /// and vice versa; the two buffers' position, limit, and mark values
    /// will be independent.
    /// </remarks>
    /// <returns>The new short buffer.</returns>
    public abstract ShortBuffer Duplicate();

    /// <summary>
    /// Creates a new, read-only short buffer that shares this buffer's content.
    /// The content of the new buffer will be that of this buffer. Changes to this
    /// buffer's content will be visible in the new buffer; the new buffer itself,
    /// however, will be read-only and will not allow the shared content to be
    /// modified. The two buffers' position, limit, and mark values will be independent.
    /// </summary>
    /// <returns>The new, read-only short buffer.</returns>
    public abstract ShortBuffer AsReadOnlyBuffer();

    /// <summary>
    /// Relative <i>get</i> method. Reads the short at this buffer's current position,
    /// and then increments the position.
    /// </summary>
    /// <returns>The short at the buffer's current position.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If the buffer's current position is not smaller than its limit.
    /// </exception>
    public abstract short Get();

    /// <summary>
    /// Relative <i>put</i> method <i>(optional operation)</i>.
    /// </summary>
    /// <param name="s">The short to be written.</param>
    /// <returns> This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer's current position is not smaller than its limit.
    /// </exception>
    /// <exception cref="GdxRuntimeException">If this buffer is read-only.</exception>
    public abstract ShortBuffer Put( short s );

    /// <summary>
    /// Absolute <i>get</i> method. Reads the short at the given index.
    /// </summary>
    /// <param name="index">The index from which the short will be read.</param>
    /// <returns>The short at the given index.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="index"/> is negative or not smaller than the buffer's limit.
    /// </exception>
    public abstract short Get( int index );

    /// <summary>
    /// Absolute <i>put</i> method <i>(optional operation)</i>.
    /// </summary>
    /// <remarks>
    ///     <para> Writes the given short into this buffer at the given index. </para>
    /// </remarks>
    /// <param name="index">The index at which the short will be written.</param>
    /// <param name="s">The short value to be written.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="index"/> is negative or not smaller than the buffer's limit.
    /// </exception>
    /// <exception cref="GdxRuntimeException">If this buffer is read-only.</exception>
    public abstract ShortBuffer Put( int index, short s );
    
    /// <summary>
    /// Compacts this buffer  <i>(optional operation)</i>.
    /// <para>
    /// The shorts between the buffer's current position and its limit, if any, are copied
    /// to the beginning of the buffer. That is, the short at index <tt><i>p</i> = position()</tt>
    /// is copied to index zero, the short at index <i>p</i> + 1 is copied to index one, and
    /// so forth until the short at index <tt>limit()</tt> - 1 is copied to
    /// index <i>n</i> = <tt>limit()</tt> - <tt>1</tt> - <i>p</i>.
    /// </para>
    /// <para>
    /// The buffer's position is then set to <i>n+1</i> and its limit is set
    /// to its capacity. The mark, if defined, is discarded.
    /// </para>
    /// <para>
    /// The buffer's position is set to the number of shorts copied,
    /// rather than to zero, so that an invocation of this method can be
    /// followed immediately by an invocation of another relative <i>put</i>
    /// method
    /// </para>
    /// </summary>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">If this buffer is read-only</exception>
    public abstract ShortBuffer Compact();

    #endregion abstract methods
    
    // ========================================================================

    /// <summary>
    /// Compares this buffer to another.
    /// <para>
    /// Two short buffers are compared by comparing their sequences of remaining
    /// elements lexicographically, without regard to the starting position of each
    /// sequence within its corresponding buffer. Pairs of <tt>short</tt> elements are
    /// compared as if by invoking <see cref="Compare(short,short)"/>.
    /// </para>
    /// <para>
    /// A short buffer is not comparable to any other type of object.
    /// </para>
    /// </summary>
    /// <returns>
    /// A negative integer, zero, or a positive integer as this buffer is less than,
    /// equal to, or greater than the given buffer
    /// </returns>
    public int CompareTo( ShortBuffer that )
    {
        var n = Position + Math.Min( Remaining(), that.Remaining() );

        for ( int i = Position, j = that.Position; i < n; i++, j++ )
        {
            var cmp = Compare( Get( i ), that.Get( j ) );

            if ( cmp != 0 )
            {
                return cmp;
            }
        }

        return Remaining() - that.Remaining();
    }

    /// <summary>
    /// Compares two values.
    /// <li>If x and y are equal, will return 0. </li>
    /// <li>If x is greater than y, will return &gt; 0. </li>
    /// <li>If x is less than y, will return &lt; 0. </li>
    /// </summary>
    private static int Compare( short x, short y )
    {
        return x - y;
    }

    // ========================================================================
    
    /// <inheritdoc />
    protected override void ValidateBackingArray()
    {
        if ( Hb == null )
        {
            throw new NullReferenceException( "Backing array is null!" );
        }
    }

    // ========================================================================
    // ========================================================================

    /// <inheritdoc cref="IDisposable"/>>
    protected override void Dispose( bool disposing )
    {
        if ( !disposing ) return;
        if ( Hb == null ) return;

        Array.Clear( Hb );
        Hb = null;
    }
    
    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Returns the current hash code of this buffer.
    /// <para>
    /// The hash code of a short buffer depends only upon its remaining
    /// elements; that is, upon the elements from <tt>position()</tt> up to,
    /// and including, the element at <tt>limit()</tt> - <tt>1</tt>.
    /// </para>
    /// <para>
    /// Because buffer hash codes are content-dependent, it is inadvisable
    /// to use buffers as keys in hash maps or similar data structures unless it
    /// is known that their contents will not change.
    /// </para>
    /// </summary>
    /// <returns> The current hash code of this buffer </returns>
    public override int GetHashCode()
    {
        const int PRIME = 31;

        var result = PRIME + NumberUtils.FloatToIntBits( 64.0f );
        result = ( PRIME * result ) + NumberUtils.FloatToIntBits( 128.0f );

        return result;
    }

    /// <summary>
    /// Tells whether or not this buffer is equal to another object.
    /// <para>
    /// Two short buffers are equal if, and only if,
    /// <li>They have the same element type,</li>
    /// <li>They have the same number of remaining elements, and</li>
    /// <li>
    /// The two sequences of remaining elements, considered
    /// independently of their starting positions, are pointwise equal.
    /// </li>
    /// </para>
    /// <para>
    /// A short buffer is not equal to any other type of object.
    /// </para>
    /// </summary>
    /// <param name="ob"> The object to which this buffer is to be compared </param>
    /// <returns>
    /// <tt>true</tt> if, and only if, this buffer is equal to the given object
    /// </returns>
    public override bool Equals( object? ob )
    {
        if ( Equals( this, ob ) )
        {
            return true;
        }

        if ( ob is not ShortBuffer that )
        {
            return false;
        }

        if ( Remaining() != that.Remaining() )
        {
            return false;
        }

        var p = Position;

        for ( int i = Limit - 1, j = that.Limit - 1; i >= p; i--, j-- )
        {
            if ( !Equals( Get( i ), that.Get( j ) ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns TRUE if <paramref name="x"/> is equal to <paramref name="y"/>.
    /// </summary>
    private static bool Equals( short x, short y )
    {
        return x == y;
    }
}
