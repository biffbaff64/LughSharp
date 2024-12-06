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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Utils.Buffers;

/// <summary>
/// A float buffer.
/// <para>
/// This class defines four categories of operations upon float buffers:
/// </para>
/// <para>
/// Absolute and relative <see cref="Get(int)"/> and <see cref="Put(float[],int,int)"/>
/// methods that read and write single floats;
/// </para>
/// <para>
/// Relative bulk <see cref="Get(int)"/> methods that transfer contiguous
/// sequences of floats from this buffer into an array; and
/// </para>
/// <para>
/// Relative bulk <see cref="Put(float[],int,int)"/> methods that transfer
/// contiguous sequences of floats from a float array or some other float
/// buffer into this buffer; and
/// </para>
/// <para>
/// Methods for <see cref="Compact"/>-ing, <see cref="Duplicate"/>-ing, and
/// <see cref="Slice"/>-ing a float buffer.
/// </para>
/// <para>
/// Float buffers can be created either by <tt>allocation</tt>, which allocates
/// space for the buffer's content, by <tt>wrapping</tt> an existing float array
/// into a buffer, or by creating a
/// <a href="ByteBuffer.html.Views">
///     <tt>view</tt>
/// </a>
/// of an existing byte buffer.
/// </para>
/// <para>
/// Like a byte buffer, a float buffer is either <tt>direct</tt> or <tt>non-direct</tt>
/// </para>
/// <para>
/// A float buffer created via the <tt>wrap</tt> methods of this class will
/// be non-direct.  A float buffer created as a view of a byte buffer will
/// be direct if, and only if, the byte buffer itself is direct.  Whether or not
/// a float buffer is direct may be determined by checking the <see cref="Buffer.IsDirect()"/>
/// method.
/// </para>
/// <para>
/// Methods in this class that do not otherwise have a value to return are specified
/// to return the buffer upon which they are invoked. This allows method invocations
/// to be chained.
/// </para>
/// </summary>
[PublicAPI]
public abstract class FloatBuffer : Buffer
{
    public new float[]? Hb { get; set; }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatBuffer"/> class with the
    /// specified parameters.
    /// </summary>
    /// <param name="mark"> The mark position. </param>
    /// <param name="pos"> The current position. </param>
    /// <param name="lim"> The limit of the buffer. </param>
    /// <param name="cap"> The capacity of the buffer. </param>
    /// <param name="hb"> The backing array, if any. Default is null. </param>
    /// <param name="offset">The offset within the backing array. Default is 0.</param>
    protected FloatBuffer( int mark, int pos, int lim, int cap, float[]? hb = null, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
        Hb     = hb ?? new float[ cap ];
        Offset = offset;

        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatBuffer"/> class with the
    /// specified parameters and no backing array.
    /// </summary>
    /// <param name="mark"> The mark position. </param>
    /// <param name="pos"> The current position. </param>
    /// <param name="lim"> The limit of the buffer. </param>
    /// <param name="cap"> The capacity of the buffer. </param>
    protected FloatBuffer( int mark, int pos, int lim, int cap )
        : this( mark, pos, lim, cap, null )
    {
        Offset = 0;

        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    /// <summary>
    /// Allocates a new float buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its
    /// capacity, its mark will be undefined, and each of its elements will be
    /// initialized to zero. It will have a backing array, and its array offset
    /// will be zero.
    /// </para>
    /// </summary>
    /// <param name="capacity"> The new buffer's capacity, in floats </param>
    /// <returns> The new float buffer </returns>
    /// <exception cref="ArgumentException">
    /// If the <tt>capacity</tt> is a negative integer
    /// </exception>
    public static FloatBuffer Allocate( int capacity )
    {
        if ( capacity < 0 )
        {
            throw new ArgumentException( "Capacity should be >= 0" );
        }

        return new HeapFloatBuffer( capacity, capacity );
    }

    /// <summary>
    /// Wraps a float array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given float array;
    /// that is, modifications to the buffer will cause the array to be modified
    /// and vice versa.  The new buffer's capacity will be <tt>array.length</tt>,
    /// its position will be <tt>offset</tt>, its limit will be <tt>offset + length</tt>,
    /// and its mark will be undefined. Its backing array will be the given array, and
    /// its array offset will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back the new buffer </param>
    /// <param name="offset">
    /// The offset of the subarray to be used; must be non-negative and no larger
    /// than <tt>array.length</tt>. The new buffer's position will be set to this value.
    /// </param>
    /// <param name="length">
    /// The length of the subarray to be used; must be non-negative and no larger than
    /// <tt>array.length - offset</tt>. The new buffer's limit will be set to
    /// <tt>offset + length</tt>.
    /// </param>
    /// <returns> The new float buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt>
    /// parameters do not hold
    /// </exception>
    public static FloatBuffer Wrap( float[] array, int offset, int length )
    {
        try
        {
            return new HeapFloatBuffer( array, offset, length );
        }
        catch ( ArgumentException )
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Wraps a float array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given float array; that is,
    /// modifications to the buffer will cause the array to be modified
    /// and vice versa.  The new buffer's capacity and limit will be
    /// <tt>array.length</tt>, its position will be zero, and its mark will be
    /// undefined.  Its backing array will be the given array, and its array
    /// offset will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back this buffer </param>
    /// <returns> The new float buffer </returns>
    public static FloatBuffer Wrap( float[] array )
    {
        return Wrap( array, 0, array.Length );
    }

    /// <summary>
    /// Creates a new float buffer whose content is a shared subsequence of
    /// this buffer's content.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position. Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of floats remaining in this buffer, and its mark
    /// will be undefined. The new buffer will be direct if, and only if, this
    /// buffer is direct, and it will be read-only if, and only if, this buffer
    /// is read-only.
    /// </para>
    /// </summary>
    /// <returns> The new float buffer </returns>
    public abstract FloatBuffer Slice();

    /// <summary>
    /// Creates a new float buffer that shares this buffer's content.
    /// <para>
    /// The content of the new buffer will be that of this buffer.  Changes
    /// to this buffer's content will be visible in the new buffer, and vice
    /// versa; the two buffers' position, limit, and mark values will be
    /// independent.
    /// </para>
    /// <para>
    /// The new buffer's capacity, limit, position, and mark values will be
    /// identical to those of this buffer.  The new buffer will be direct if,
    /// and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns> The new float buffer </returns>
    public abstract FloatBuffer Duplicate();

    /// <summary>
    /// Creates a new, read-only float buffer that shares this buffer's
    /// content.
    /// <para>
    /// The content of the new buffer will be that of this buffer.  Changes
    /// to this buffer's content will be visible in the new buffer; the new
    /// buffer itself, however, will be read-only and will not allow the shared
    /// content to be modified.  The two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's capacity, limit, position, and mark values will be
    /// identical to those of this buffer.
    /// </para>
    /// <para>
    /// If this buffer is itself read-only then this method behaves in
    /// exactly the same way as the {@link #duplicate duplicate} method.
    /// </para>
    /// </summary>
    /// <returns> The new, read-only float buffer </returns>
    public abstract FloatBuffer asReadOnlyBuffer();

    // ========================================================================
    // -- Singleton get/put methods --

    /// <summary>
    /// Relative <tt>get</tt> method. Reads the float at this buffers
    /// current position, and then increments the position.
    /// </summary>
    /// <returns> The float at the buffer's current position </returns>
    /// <exception cref="GdxRuntimeException">
    /// If the buffer's current position is not smaller than its limit
    /// </exception>
    public abstract float Get();

    /// <summary>
    /// Relative <tt>put</tt> method; <tt>(optional operation)</tt>.
    /// <para>
    /// Writes the given float into this buffer at the current
    /// position, and then increments the position.
    /// </para>
    /// </summary>
    /// <param name="f"> The float to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer's current position is not smaller than its limit
    /// </exception>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is read-only
    /// </exception>
    public abstract FloatBuffer Put( float f );

    /// <summary>
    /// Absolute <tt>get</tt> method. Reads the float at the given index.
    /// </summary>
    /// <param name="index"> The index from which the float will be read </param>
    /// <returns> The float at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    public abstract float Get( int index );

    /// <summary>
    /// Absolute <tt>put</tt> method  <tt>(optional operation)</tt>.
    /// <para>
    /// Writes the given float into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index"> The index at which the float will be written </param>
    /// <param name="f"> The float value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    /// <exception cref="GdxRuntimeException"> If this buffer is read-only </exception>
    public abstract FloatBuffer Put( int index, float f );

    // ========================================================================
    // -- Bulk get operations --

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// <para>
    /// This method transfers floats from this buffer into the given
    /// destination array. If there are fewer floats remaining in the
    /// buffer than are required to satisfy the request, that is, if
    /// <tt>length &gt; Remaining()</tt>, then no floats are transferred
    /// and a <see cref="GdxRuntimeException"/> is thrown.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <tt>length</tt> floats from this
    /// buffer into the given array, starting at the current position of this
    /// buffer and at the given offset in the array.  The position of this
    /// buffer is then incremented by <tt>length</tt>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form
    /// <tt>src.Get(dst, off, len)</tt> has exactly the same effect as
    /// the loop:
    /// <code>
    ///         for (int i = off; i &lt; off + len; i++)
    ///         {
    ///             dst[i] = src.Get():
    ///         }
    ///         </code>
    /// </para>
    /// <para>
    /// except that it first checks that there are sufficient floats in
    /// this buffer and it is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="dst">
    /// The array into which floats are to be written
    /// </param>
    /// <param name="offset">
    /// The offset within the array of the first float to be written; must
    /// be non-negative and no larger than <tt>dst.Length</tt>
    /// </param>
    /// <param name="length">
    /// The maximum number of floats to be written to the given array; must
    /// be non-negative and no larger than <tt>dst.Length - offset</tt>
    /// </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than <tt>length</tt> floats remaining in this buffer
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt> parameters do not hold
    /// </exception>
    public FloatBuffer Get( float[] dst, int offset, int length )
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
    /// Relative bulk <tt>get</tt> method.
    /// <para>
    /// This method transfers floats from this buffer into the given
    /// destination array. An invocation of this method of the form
    /// <tt>src.get(a)</tt> behaves in exactly the same way as the invocation
    /// </para>
    /// <code>
    ///     src.get(a, 0, a.length)
    /// </code>
    /// </summary>
    /// <param name="dst"> The destination array </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than <tt>length</tt> floats remaining in this buffer
    /// </exception>
    public FloatBuffer Get( float[] dst )
    {
        return Get( dst, 0, dst.Length );
    }

    // ========================================================================
    // -- Bulk put operations --

    /// <summary>
    /// Relative bulk <tt>put</tt> method (optional operation).
    /// This method transfers the floats remaining in the given source buffer into this buffer.
    /// If there are more floats remaining in the source buffer than in this buffer, that is,
    /// if <tt>src.Remaining() &gt; this.Remaining()</tt>, then no floats are transferred, and a
    /// <see cref="GdxRuntimeException"/> is thrown.
    /// <para>
    /// Otherwise, this method copies <tt>n = src.Remaining()</tt> floats from the given buffer
    /// into this buffer, starting at each buffer's current position. The positions of both buffers
    /// are then incremented by <tt>n</tt>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form <tt>dst.Put(src)</tt> has exactly
    /// the same effect as the loop:
    /// <code>
    ///         while (src.HasRemaining)
    ///         {
    ///             dst.Put(src.Get());
    ///         }
    /// </code>
    /// except that it first checks that there is sufficient space in this buffer and is potentially
    /// much more efficient.
    /// </para>
    /// </summary>
    /// <param name="src">
    /// The source buffer from which floats are to be read; must not be this buffer.
    /// </param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer for the remaining floats in the source buffer.
    /// </exception>
    /// <exception cref="ArgumentException"> If the source buffer is this buffer. </exception>
    /// <exception cref="GdxRuntimeException"> If this buffer is read-only. </exception>
    public FloatBuffer Put( FloatBuffer src )
    {
        if ( IsReadOnly )
        {
            return this;
        }

        if ( src.Equals( this ) )
        {
            throw new ArgumentException( "src cannot be this buffer!" );
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
    /// Relative bulk <i>put</i> method (optional operation).
    /// </summary>
    /// <remarks>
    /// This method transfers floats into this buffer from the given source array. If
    /// there are more floats to be copied from the array than remain in this buffer,
    /// that is, if <tt>length &gt; this.Remaining()</tt>, then no floats are transferred,
    /// and a <see cref="GdxRuntimeException"/> is thrown.
    /// <para>
    /// Otherwise, this method copies <tt>length</tt> floats from the given array into this
    /// buffer, starting at the given offset in the array and at the current position of this
    /// buffer. The position of this buffer is then incremented by <tt>length</tt>.
    /// </para>
    /// In other words, an invocation of this method of the form <tt>dst.Put(src, off, len)</tt>
    /// has exactly the same effect as the loop:
    /// <code>
    ///     for (int i = off; i &lt; off + len; i++)
    ///     {
    ///         dst.Put(a[i]);
    ///     }
    /// </code>
    /// except that it first checks that there is sufficient space in this buffer and is
    /// potentially much more efficient.
    /// </remarks>
    /// <param name="src">The array from which floats are to be read.</param>
    /// <param name="offset">
    /// The offset within the array of the first float to be read; must be non-negative and
    /// no larger than <paramref name="src"/>.Length.
    /// </param>
    /// <param name="length">
    /// The number of floats to be read from the given array; must be non-negative
    /// and no larger than <paramref name="src"/>.Length - <paramref name="offset"/>.
    /// </param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer, or if the buffer is read-only.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <paramref name="offset"/> and <paramref name="length"/>
    /// parameters do not hold.
    /// </exception>
    public FloatBuffer Put( float[] src, int offset, int length )
    {
        if ( IsReadOnly )
        {
            return this;
        }

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
    /// Relative bulk <i>put</i> method (optional operation).
    /// </summary>
    /// <remarks>
    /// This method transfers the entire content of the given source float
    /// array into this buffer. An invocation of this method with the source
    /// float array <paramref name="src"/> behaves in exactly the same way as
    /// invoking <tt>dst.Put(src, 0, src.Length)</tt>.
    /// </remarks>
    /// <param name="src">The source array.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer.
    /// </exception>
    /// <exception cref="GdxRuntimeException">If this buffer is read-only.</exception>
    public FloatBuffer Put( float[] src )
    {
        return Put( src, 0, src.Length );
    }

    // ========================================================================
    // -- Other stuff --

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible float array.
    /// </summary>
    /// <remarks>
    /// If this method returns <tt>true</tt>, then the <see cref="BackingArray"/> and
    /// <see cref="ArrayOffset"/> methods may safely be invoked.
    /// </remarks>
    /// <returns>
    /// <tt>true</tt> if, and only if, this buffer is backed by an array and is not read-only.
    /// </returns>
    public override bool HasBackingArray()
    {
        return ( Hb != null ) && !IsReadOnly;
    }

    /// <summary>
    /// Returns the float array that backs this buffer (optional operation).
    /// </summary>
    /// <remarks>
    /// Modifications to this buffer's content will cause the returned array's
    /// content to be modified, and vice versa. Invoke the <see cref="HasBackingArray"/>
    /// method before invoking this method to ensure that this buffer has an
    /// accessible backing array.
    /// </remarks>
    /// <returns> The array that backs this buffer. </returns>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is backed by an array but is read-only.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// If this buffer is not backed by an accessible array.
    /// </exception>
    public new float[] BackingArray()
    {
        return Hb!;
    }

    /// <summary>
    /// Returns the offset within this buffer's backing array of the first
    /// element of the buffer <tt>(optional operation)</tt>.
    /// <para>
    /// If this buffer is backed by an array then buffer position <i>p</i>
    /// corresponds to array index <tt>p + ArrayOffset()</tt>.
    /// </para>
    /// <para>
    /// Invoke the <see cref="HasBackingArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.
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
        ValidateBackingArray();

        return Offset;
    }

    /// <summary>
    /// Compacts this buffer  <i>(optional operation)</i>.
    /// <para>
    /// The floats between the buffer's current position and its limit, if any,
    /// are copied to the beginning of the buffer. That is, the float at index
    /// <c>p = Position</c> is copied to index zero, the float at index <c>p + 1|</c>
    /// is copied to index one, and so forth until the float at index <c>Limit - 1</c>
    /// is copied to index <c>n = Limit - 1 - p</c>. The buffer's position is then
    /// set to <c>n + 1</c> and its limit is set to its capacity. The mark, if defined,
    /// is discarded.
    /// </para>
    /// <para>
    /// The buffer's position is set to the number of floats copied, rather than
    /// to zero, so that an invocation of this method can be followed immediately
    /// by an invocation of another relative <tt>put</tt>
    /// method.
    /// </para>
    /// </summary>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">If this buffer is read-only</exception>
    public abstract FloatBuffer Compact();

    /// <summary>
    /// Returns the current hash code of this buffer.
    /// <para>
    /// The hash code of a float buffer depends only upon its remaining
    /// elements; that is, upon the elements from <tt>Position</tt> up to, and
    /// including, the element at <tt>Limit - 1</tt>.
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
        var h = 31 + ( int )Get( 0 );

        h = ( 31 * h ) + ( int )Get( 1 );
        h = ( 31 * h ) + ( int )Get( 2 );
        h = ( 31 * h ) + ( int )Get( 3 );

        return h;
    }

    /// <summary>
    /// Tells whether or not this buffer is equal to another object.
    /// <para>
    /// Two float buffers are equal if, and only if,
    /// <li> They have the same element type, </li>
    /// <li> They have the same number of remaining elements, and, </li>
    /// <li>
    /// The two sequences of remaining elements, considered independently of their
    /// starting positions, are pointwise equal. This method considers two float
    /// elements <tt>a</tt> and <tt>b</tt> to be equal if:-
    /// <code>
    ///     <tt>(a == b) || (Float.isNaN(a) &amp;&amp; Float.isNaN(b))</tt>.
    /// </code>
    /// The values <tt>-0.0</tt> and <tt>+0.0</tt> are considered to be
    /// equal, unlike <see cref="float.Equals(object)"/>.
    /// </li>
    /// </para>
    /// <para>
    /// A float buffer is not equal to any other type of object.
    /// </para>
    /// </summary>
    /// <param name="ob"> The object to which this buffer is to be compared </param>
    /// <returns>
    /// <tt>true</tt> if, and only if, this buffer is equal to the given object
    /// </returns>
    public override bool Equals( object? ob )
    {
        if ( this == ob )
        {
            return true;
        }

        if ( ob is not FloatBuffer floatBuffer )
        {
            return false;
        }

        if ( Remaining != floatBuffer.Remaining )
        {
            return false;
        }

        var p = Position;

        for ( int i = Limit - 1, j = floatBuffer.Limit - 1; i >= p; i--, j-- )
        {
            if ( !Equals( Get( i ), floatBuffer.Get( j ) ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Tells whether or not float <tt>a</tt> is equal to float <tt>b</tt>.
    /// </summary>
    private static bool Equals( float a, float b )
    {
        return a.Equals( b ) || ( float.IsNaN( a ) && float.IsNaN( b ) );
    }

    /// <summary>
    /// Compares this buffer to another.
    /// <para>
    /// Two float buffers are compared by comparing their sequences of remaining
    /// elements lexicographically, without regard to the starting position of each
    /// sequence within its corresponding buffer. Pairs of float elements are compared
    /// as if by invoking <see cref="float.CompareTo(float)"/>, except that <tt>-0.0</tt>
    /// and <tt>0.0</tt> are considered to be equal.
    /// </para>
    /// <para>
    /// <tt>Float.NaN</tt> is considered by this method to be equal to itself and greater
    /// than all other <tt>float</tt> values (including <tt>float.PositiveInfinity</tt>).
    /// </para>
    /// <para>
    /// A float buffer is not comparable to any other type of object.
    /// </para>
    /// </summary>
    /// <returns>
    /// A negative integer, zero, or a positive integer as this buffer
    /// is less than, equal to, or greater than the given buffer
    /// </returns>
    public int CompareTo( FloatBuffer that )
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
    /// Compare two float values, returning -1 if the first is less than the second,
    /// +1 if the first is greater than the second, and 0 if they are equal. It also
    /// handles NaN (Not a Number) values by considering them equal to each other but
    /// less than any non-NaN number.
    /// </summary>
    private static int Compare( float x, float y )
    {
        if ( x < y ) return -1;

        if ( x > y ) return 1;

        if ( x.Equals( y ) ) return 0;

        if ( float.IsNaN( x ) )
        {
            return float.IsNaN( y ) ? 0 : 1;
        }

        return -1;
    }

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
}