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


using LughSharp.LibCore.Utils.Buffers.HeapBuffers;
using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Utils.Buffers;

[PublicAPI]
public abstract class DoubleBuffer : Buffer, IComparable< DoubleBuffer >
{
    public new double[]? Hb        { get; set; }
    protected  int       Offset    { get; set; }
    protected  bool      BigEndian { get; set; } = true;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new DoubleBuffer.
    /// </summary>
    protected DoubleBuffer( int mark, int pos, int lim, int cap, double[]? hb = null, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
        Hb     = hb ?? new double[ cap ];
        Offset = offset;
    }

    /// <summary>
    /// Compares this buffer to another.
    /// <para>
    /// Two double buffers are compared by comparing their sequences of remaining
    /// elements lexicographically, without regard to the starting position of each
    /// sequence within its corresponding buffer. Pairs of <tt>double</tt> elements
    /// are compared as if by invoking <tt>double.==(double,double)</tt>, except that
    /// <tt>-0.0</tt> and <tt>0.0</tt> are considered to be equal. <tt>double.NaN</tt>
    /// is considered by this method to be equal to itself and greater than all other
    /// <tt>double</tt> values (including <tt>double.PositiveInfinity</tt>).
    /// </para>
    /// <para>
    /// A double buffer is not comparable to any other type of object.
    /// </para>
    /// </summary>
    /// <returns>
    /// A negative integer, zero, or a positive integer as this buffer
    /// is less than, equal to, or greater than the given buffer
    /// </returns>
    public int CompareTo( DoubleBuffer? that )
    {
        var n = Position + Math.Min( Remaining(), that!.Remaining() );

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
    /// Allocates a new double buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its capacity,
    /// its mark will be undefined, and each of its elements will be initialized
    /// to zero. It will have a backing <see cref="BackingArray"/>, and its <see cref="ArrayOffset"/>
    /// will be zero.
    /// </para>
    /// </summary>
    /// <param name="capacity"> The new buffer's capacity, in doubles. </param>
    /// <returns> The new double buffer </returns>
    /// <exception cref="ArgumentException"> If the capacity is a negative integer. </exception>
    public static DoubleBuffer Allocate( int capacity )
    {
        if ( capacity < 0 )
        {
            throw new ArgumentException();
        }

        return new HeapDoubleBuffer( capacity, capacity );
    }

    /// <summary>
    /// Wraps a double array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given double array; that is, modifications
    /// to the buffer will cause the array to be modified and vice versa. =The new buffer's
    /// capacity will be <tt>array.length</tt>, its position will be <tt>offset</tt>, its limit
    /// will be <tt>offset + length</tt>, and its mark will be undefined. Its
    /// <see cref="BackingArray"/> will be the given array, and its <see cref="ArrayOffset"/> will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back the new buffer. </param>
    /// <param name="offset">
    /// The offset of the subarray to be used; must be non-negative and
    /// no larger than <tt>array.length</tt>.  The new buffer's position
    /// will be set to this value.
    /// </param>
    /// <param name="length">
    /// The length of the subarray to be used;
    /// must be non-negative and no larger than
    /// <tt>array.length - offset</tt>.
    /// The new buffer's limit will be set to <tt>offset + length</tt>.
    /// </param>
    /// <returns> The new double buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt>
    /// parameters do not hold
    /// </exception>
    public static DoubleBuffer Wrap( double[] array, int offset, int length )
    {
        try
        {
            return new HeapDoubleBuffer( array, offset, length );
        }
        catch ( ArgumentException )
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Wraps a double array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given double array;
    /// that is, modifications to the buffer will cause the array to be modified
    /// and vice versa.  The new buffer's capacity and limit will be
    /// <tt>array.length</tt>, its position will be zero, and its mark will be
    /// undefined.  Its <see cref="BackingArray"/> backing array will be the
    /// given array, and its <see cref="ArrayOffset"/> will
    /// be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back this buffer </param>
    /// <returns> The new double buffer </returns>
    public static DoubleBuffer Wrap( double[] array )
    {
        return Wrap( array, 0, array.Length );
    }

    /// <summary>
    /// Creates a new double buffer whose content is a shared subsequence of
    /// this buffer's content.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of doubles remaining in this buffer, and its mark
    /// will be undefined.  The new buffer will be direct if, and only if, this
    /// buffer is direct, and it will be read-only if, and only if, this buffer
    /// is read-only.
    /// </para>
    /// </summary>
    /// <returns> The new double buffer </returns>
    public abstract DoubleBuffer Slice();

    /// <summary>
    /// Creates a new double buffer that shares this buffer's content.
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
    /// <returns> The new double buffer </returns>
    public abstract DoubleBuffer Duplicate();

    /// <summary>
    /// Creates a new, read-only double buffer that shares this buffer's
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
    /// exactly the same way as the <see cref="Duplicate"/> method.
    /// </para>
    /// </summary>
    /// <returns> The new, read-only double buffer </returns>
    public abstract DoubleBuffer asReadOnlyBuffer();

    /// <summary>
    /// Relative <i>get</i> method.  Reads the double at this buffer's
    /// current position, and then increments the position.
    /// </summary>
    /// <returns> The double at the buffer's current position </returns>
    public abstract double Get();

    /// <summary>
    /// Relative <i>put</i> method <i>(optional operation)</i>.
    /// <para>
    /// Writes the given double into this buffer at the current
    /// position, and then increments the position.
    /// </para>
    /// </summary>
    /// <param name="d"> The double to be written </param>
    /// <returns> This buffer </returns>
    public abstract DoubleBuffer Put( double d );

    /// <summary>
    /// Absolute <i>get</i> method.  Reads the double at the given index.
    /// </summary>
    /// <param name="index"> The index from which the double will be read </param>
    /// <returns> The double at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    public abstract double Get( int index );

    /// <summary>
    /// Absolute <i>put</i> method <i>(optional operation)</i>.
    /// <para>
    /// Writes the given double into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index"> The index at which the double will be written </param>
    /// <param name="d"> The double value to be written </param>
    /// <returns> This buffer </returns>
    public abstract DoubleBuffer Put( int index, double d );

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// <para>
    /// This method transfers doubles from this buffer into the given
    /// destination array.  If there are fewer doubles remaining in the
    /// buffer than are required to satisfy the request, that is, if
    /// <tt>length</tt> <tt>&gt;</tt> <tt>remaining()</tt>, then no
    /// doubles are transferred.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <tt>length</tt> doubles from this
    /// buffer into the given array, starting at the current position of this
    /// buffer and at the given offset in the array.  The position of this
    /// buffer is then incremented by <tt>length</tt>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form
    /// <tt>src.get(dst, off, len)</tt> has exactly the same effect as
    /// the loop
    /// </para>
    /// <code>
    ///         for (int i = off; i &lt; off + len; i++)
    ///         {
    ///              dst[ i ] = src.get():
    ///         }
    ///     </code>
    /// except that it first checks that there are sufficient doubles in
    /// this buffer and it is potentially much more efficient.
    /// </summary>
    /// <param name="dst">
    /// The array into which doubles are to be written
    /// </param>
    /// <param name="offset">
    /// The offset within the array of the first double to be
    /// written; must be non-negative and no larger than
    /// <tt>dst.length</tt>
    /// </param>
    /// <param name="length">
    /// The maximum number of doubles to be written to the given
    /// array; must be non-negative and no larger than
    /// <tt>dst.length - offset</tt>
    /// </param>
    /// <returns> This buffer </returns>
    public virtual DoubleBuffer Get( double[] dst, int offset, int length )
    {
        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer underflow!" );
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
    /// This method transfers doubles from this buffer into the given
    /// destination array.  An invocation of this method of the form
    /// <tt>src.get(a)</tt> behaves in exactly the same way as the invocation
    /// </para>
    /// <code>
    ///         src.get(a, 0, a.length)
    ///     </code>
    /// </summary>
    /// <param name="dst"> The destination array </param>
    /// <returns> This buffer </returns>
    public DoubleBuffer Get( double[] dst )
    {
        return Get( dst, 0, dst.Length );
    }

    /// <summary>
    /// Relative bulk <i>put</i> method <i>(optional operation)</i>.
    /// <para>
    /// This method transfers doubles into this buffer from the given source array.
    /// If there are more doubles to be copied from the array than remain in this
    /// buffer, that is, if <tt>length</tt> <tt>&gt;</tt> <tt>remaining()</tt>, then
    /// no doubles are transferred.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <i>n</i> = <tt>src.remaining()</tt> doubles
    /// from the given buffer into this buffer, starting at each buffer's current
    /// position. The positions of both buffers are then incremented by <i>n</i>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form <tt>dst.put(src)</tt>
    /// has exactly the same effect as the loop.
    /// </para>
    /// <code>
    ///         while (src.hasRemaining())
    ///         {
    ///             dst.Put(src.get());
    ///         }
    ///     </code>
    /// except that it first checks that there is sufficient space in this
    /// buffer and it is potentially much more efficient.
    /// </summary>
    /// <param name="src">
    /// The source buffer from which doubles are to be read;
    /// must not be this buffer
    /// </param>
    /// <returns> This buffer </returns>
    public virtual DoubleBuffer Put( DoubleBuffer src )
    {
        if ( src.Equals( this ) )
        {
            throw new ArgumentException();
        }

        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Readonly Buffer!" );
        }

        var n = src.Remaining();

        if ( n > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer underflow!" );
        }

        for ( var i = 0; i < n; i++ )
        {
            Put( src.Get() );
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>put</i> method <i>(optional operation)</i>.
    /// <para>
    /// This method transfers doubles into this buffer from the given source array.
    /// If there are more doubles to be copied from the array than remain in this
    /// buffer, that is, if <tt>length</tt> <tt>&gt;</tt> <tt>remaining()</tt>, then
    /// no doubles are transferred.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <tt>length</tt> doubles from the given array
    /// into this buffer, starting at the given offset in the array and at the current
    /// position of this buffer. The position of this buffer is then incremented by
    /// <tt>length</tt>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form <tt>dst.put(src, off, len)</tt>
    /// has exactly the same effect as the loop
    /// </para>
    /// <code>
    ///          for (int i = off; i &lt; off + len; i++)
    ///          {
    ///              dst.put(a[i]);
    ///          }
    ///     </code>
    /// except that it first checks that there is sufficient space in this
    /// buffer and it is potentially much more efficient.
    /// </summary>
    /// <param name="src">
    /// The array from which doubles are to be read
    /// </param>
    /// <param name="offset">
    /// The offset within the array of the first double to be read;
    /// must be non-negative and no larger than <tt>array.length</tt>
    /// </param>
    /// <param name="length">
    /// The number of doubles to be read from the given array;
    /// must be non-negative and no larger than
    /// <tt>array.length - offset</tt>
    /// </param>
    /// <returns> This buffer </returns>
    public virtual DoubleBuffer Put( double[] src, int offset, int length )
    {
        CheckBounds( offset, length, src.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer overflow!" );
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
    /// This method transfers the entire content of the given source double array into
    /// this buffer. An invocation of this method of the form <tt>dst.put(a)</tt> behaves
    /// in exactly the same way as the invocation
    /// </para>
    /// <code>
    ///         dst.put(a, 0, a.length)
    ///     </code>
    /// </summary>
    /// <param name="src"> The source array </param>
    /// <returns> This buffer </returns>
    public DoubleBuffer Put( double[] src )
    {
        return Put( src, 0, src.Length );
    }

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible double
    /// array.
    /// <para>
    /// If this method returns <tt>true</tt> then the <see cref="BackingArray"/>
    /// and <see cref="ArrayOffset()"/> methods may safely be invoked.
    /// </para>
    /// </summary>
    /// <returns>
    /// TRUE if, and only if, this buffer is backed by an array and is not read-only
    /// </returns>
    public override bool HasBackingArray()
    {
        return ( Hb != null ) && !IsReadOnly;
    }

    /// <summary>
    /// Returns the double array that backs this buffer <i>(optional operation)</i>.
    /// <para>
    /// Modifications to this buffer's content will cause the returned array's
    /// content to be modified, and vice versa.
    /// </para>
    /// <para>
    /// Invoke the <see cref="HasBackingArray"/> method before invoking this method in
    /// order to ensure that this buffer has an accessible backing array.
    /// </para>
    /// </summary>
    /// <returns>  The array that backs this buffer </returns>
    public new double[] BackingArray()
    {
        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Readonly Buffer!" );
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
    /// Invoke the <see cref="HasBackingArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.
    /// </para>
    /// </summary>
    /// <returns>
    /// The offset within this buffer's array of the first element of the buffer
    /// </returns>
    public override int ArrayOffset()
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Unsupported Operation!" );
        }

        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Readonly Buffer!" );
        }

        return Offset;
    }

    /// <summary>
    /// Compacts this buffer  <i>(optional operation)</i>.
    /// <para>
    /// The doubles between the buffer's current position and its limit,
    /// if any, are copied to the beginning of the buffer.  That is, the
    /// double at index <i>p</i> = <tt>position()</tt> is copied
    /// to index zero, the double at index <i>p</i> + 1 is copied
    /// to index one, and so forth until the double at index
    /// <tt>limit()</tt> - 1 is copied to index
    /// <i>n</i> = <tt>limit()</tt> - <tt>1</tt> - <i>p</i>.
    /// The buffer's position is then set to <i>n+1</i> and its limit is set to
    /// its capacity.  The mark, if defined, is discarded.
    /// </para>
    /// <para>
    /// The buffer's position is set to the number of doubles copied,
    /// rather than to zero, so that an invocation of this method can be
    /// followed immediately by an invocation of another relative <i>put</i>
    /// method.
    /// </para>
    /// </summary>
    /// <returns> This buffer </returns>
    public abstract DoubleBuffer Compact();

    /// <summary>
    /// Returns a string summarizing the state of this buffer.
    /// </summary>
    public override string ToString()
    {
        return $"{GetType().Name}[pos={Position} lim={Limit} cap={Capacity}]";
    }

    /// <summary>
    /// Returns the current hash code of this buffer.
    /// <para>
    /// The hash code of a double buffer depends only upon its remaining
    /// elements; that is, upon the elements from <tt>position()</tt> up to, and
    /// including, the element at <tt>limit()</tt> - <tt>1</tt>.
    /// </para>
    /// <para>
    /// Because buffer hash codes are content-dependent, it is inadvisable
    /// to use buffers as keys in hash maps or similar data structures unless it
    /// is known that their contents will not change.
    /// </para>
    /// </summary>
    /// <returns> The current hash code of this buffer </returns>
    public int HashCode()
    {
        var h = 1;
        var p = Position;

        for ( var i = Limit - 1; i >= p; i-- )
        {
            h = ( 31 * h ) + ( int ) Get( i );
        }

        return h;
    }

    /// <summary>
    /// Tells whether or not this buffer is equal to another object.
    /// <para>
    /// Two double buffers are equal if, and only if,
    /// </para>
    /// <li>
    ///     <para> They have the same element type, </para>
    /// </li>
    /// <li>
    ///     <para>
    ///     They have the same number of remaining elements, and
    ///     </para>
    /// </li>
    /// <li>
    ///     <para>
    ///     The two sequences of remaining elements, considered independently of
    ///     their starting positions, are pointwise equal. This method considers
    ///     two double elements <tt>a</tt> and <tt>b</tt> to be equal if
    ///     <code> (a == b) || (double.IsNaN(a) $amp;$amp; double.IsNaN(b))</code>.
    ///     The values <tt>-0.0</tt> and <tt>+0.0</tt> are considered to be equal,
    ///     unlike <tt>double.Equals(object)</tt>.
    ///     </para>
    /// </li>
    /// <para> A double buffer is not equal to any other type of object.  </para>
    /// </summary>
    /// <param name="ob"> The object to which this buffer is to be compared </param>
    /// <returns> TRUE if, and only if, this buffer is equal to the given object </returns>
    public override bool Equals( object? ob )
    {
        if ( this == ob )
        {
            return true;
        }

        if ( ob is not DoubleBuffer that )
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

    /// <inheritdoc />
    public override int GetHashCode()
    {
        const int PRIME = 31;

        var result = PRIME + NumberUtils.FloatToIntBits( 64.0f );
        result = ( PRIME * result ) + NumberUtils.FloatToIntBits( 128.0f );

        return result;
    }

    private static bool Equals( double x, double y )
    {
        return x.Equals( y ) || ( double.IsNaN( x ) && double.IsNaN( y ) );
    }

    private static int Compare( double x, double y )
    {
        return x < y             ? -1 :
               x > y             ? +1 :
               x.Equals( y )     ? 0 :
               double.IsNaN( x ) ? double.IsNaN( y ) ? 0 : +1 : -1;
    }

    /// <summary>
    /// Retrieves this buffer's byte order.
    /// <para>
    /// The byte order of a char buffer created by allocation or by wrapping an existing
    /// <tt>char</tt> array is the <see cref="ByteOrder.NativeOrder"/> of the underlying
    /// hardware.  The byte order of a char buffer created as a view of a byte buffer is
    /// that of the byte buffer at the moment that the view is created.
    /// </para>
    /// </summary>
    /// <returns> This buffer's byte order </returns>
    public virtual ByteOrder Order()
    {
        return BigEndian ? ByteOrder.BigEndian : ByteOrder.LittleEndian;
    }

    /// <summary>
    /// Modifies this buffer's byte order.
    /// </summary>
    /// <param name="order">
    /// The new byte order, either <see cref="ByteOrder.BigEndian"/>
    /// or <see cref="ByteOrder.LittleEndian"/>
    /// </param>
    /// <returns> This buffer </returns>
    public virtual DoubleBuffer Order( ByteOrder order )
    {
        BigEndian       = order == ByteOrder.BigEndian;
        NativeByteOrder = BigEndian == ( ByteOrder.NativeOrder == ByteOrder.BigEndian );

        return this;
    }
}