// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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

/// <summary>
/// A byte buffer.
/// <br></br>
/// <para>
/// This class defines six categories of operations upon byte buffers:
/// <li>
/// Absolute and relative <see cref="Get()"/> and <see cref="Put(byte)"/> methods
/// that read and write single bytes.
/// </li>
/// <li>
/// Relative <see cref="Get(byte[])"/> methods that transfer contiguous sequences
/// of bytes from this buffer into an array.
/// </li>
/// <li>
/// Relative <see cref="Put(byte[])"/> methods that transfer contiguous sequences
/// of bytes from a byte array or some other byte buffer into this buffer.
/// </li>
/// <li>
/// Absolute and relative <see cref="GetChar()"/> and <see cref="PutChar(char)"/>
/// methods that read and write values of other primitive types, translating them
/// to and from sequences of bytes in a particular byte order.
/// </li>
/// <li>
/// Methods for creating view buffers, which allow a byte buffer to be viewed
/// as a buffer containing values of some other primitive type.
/// </li>
/// <li>
/// Methods for <see cref="Compact"/>ing, <see cref="Duplicate"/>ing, and
/// <see cref="Slice"/>ing a byte buffer.
/// </li>
/// </para>
/// <br></br>
/// <para>
/// Byte buffers can be created either by <see cref="Allocate"/>, which allocates
/// space for the buffers content, or by <see cref="Wrap(byte[])"/> an existing
/// byte array into a buffer.
/// </para>
/// <br></br>
/// <h2> Access to binary data </h2>
/// <para>
/// This class defines methods for reading and writing values of all other primitive
/// types, except <tt>bool</tt>. Primitive values are translated to (or from) sequences
/// of bytes according to the buffer's current byte order, which may be retrieved and
/// modified via the <see cref="Order()"/> methods. Specific byte orders are represented
/// by instances of the <see cref="ByteOrder"/> class. The initial order of a byte buffer
/// is always <see cref="ByteOrder.BigEndian"/>.
/// </para>
/// <br></br>
/// <para>
/// For access to heterogeneous binary data, that is, sequences of values of different types,
/// this class defines a family of absolute and relative <i>Get</i> and <i>Put</i> methods
/// for each type. For 32-bit floating-point values, for example, this class defines:
/// <code>
///         float GetFloat()
///         float GetFloat(int)
///         void PutFloat(float)
///         void PutFloat(int,float)
///     </code>
/// </para>
/// <para>
/// Corresponding methods are defined for the types <tt>char</tt>, <tt>short</tt>,
/// <tt>int</tt>, <tt>long</tt>, and <tt>double</tt>. The index parameters of the
/// absolute <i>Get</i> and <i>Put</i> methods are in terms of bytes rather than of
/// the type being read or written.
/// </para>
/// <br></br>
/// <h2> View Buffers. </h2>
/// <para>
/// For access to homogeneous binary data, that is, sequences of values of the same type,
/// this class defines methods that can create <i>views</i> of a given byte buffer. A
/// <i>view buffer</i> is simply another buffer whose content is backed by the byte buffer.
/// Changes to the byte buffer's content will be visible in the view buffer, and vice versa;
/// the two buffers' position, limit, and mark values are independent.
/// <br></br>
/// <para>
/// The <see cref="AsFloatBuffer()"/> method, for example, creates an instance of the
/// <see cref="FloatBuffer"/> class that is backed by the byte buffer upon which the
/// method is invoked.
/// </para>
/// <para>
/// Corresponding view-creation methods are defined for the types <tt>char</tt>,
/// <tt>short</tt>, <tt>int</tt>, <tt>long</tt>, and <tt>double</tt>.
/// </para>
/// </para>
/// <br></br>
/// <para>
/// View buffers have some3 important advantages over the families of type-specific <i>Get</i>
/// and <i>Put</i> methods described above:
/// <br></br>
/// <li>
/// A view buffer is indexed not in terms of bytes but rather in terms of the type-specific
/// size of its values.
/// </li>
/// <li>
/// A view buffer provides relative bulk <i>Get</i> and <i>Put</i> methods that can transfer
/// contiguous sequences of values between a buffer and an array or some other buffer of the
/// same type.
/// </li>
/// </para>
/// <para>
/// The byte order of a view buffer is fixed to be that of its byte buffer at the time that
/// the view is created.
/// </para>
/// <h2> Invocation chaining </h2>
/// <para>
/// Methods in this class that do not otherwise have a value to return are specified to return
/// the buffer upon which they are invoked. This allows method invocations to be chained.
/// <br></br>
/// <para>
/// The sequence of statements:-
/// <code>
///         bb.PutInt(0xCAFEBABE);
///         bb.PutShort(3);
///         bb.PutShort(45);
///     </code>
/// can, for example, be replaced by the single statement
/// <code>
///         bb.PutInt(0xCAFEBABE).PutShort(3).PutShort(45);
///     </code>
/// </para>
/// </para>
/// </summary>
[PublicAPI]
public abstract class ByteBuffer : Buffer
{
    protected bool NativeByteOrder = Bits.ByteOrder == ByteOrder.BigEndian;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new buffer with the given mark, position, limit, capacity,
    /// backing array, and array offset
    /// </summary>
    protected ByteBuffer( int mark, int pos, int lim, int cap, byte[]? hb = null, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
        Hb     = hb;
        Offset = offset;
    }

    /// <summary>
    /// This byte buffer's backing array.
    /// </summary>
    public byte[]? Hb { get; set; }

    // ------------------------------------------------------------------------

    protected int  Offset    { get; set; }
    protected bool BigEndian { get; set; } = true;

    /// <summary>
    /// Allocates a new byte buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its capacity, its
    /// mark will be undefined, and each of its elements will be initialized to zero.
    /// It will have a backing array and its <see cref="ArrayOffset"/> will be zero.
    /// </para>
    /// </summary>
    /// <param name="capacity">The new buffer's capacity, in bytes</param>
    /// <returns> The new byte buffer </returns>
    /// <exception cref="ArgumentException">
    /// If the <tt>capacity</tt> is a negative integer
    /// </exception>
    public static ByteBuffer Allocate( int capacity )
    {
        if ( capacity < 0 )
        {
            throw new ArgumentException( $"capacity should not be less than zero! : {capacity}" );
        }

        return new HeapByteBuffer( capacity, capacity );
    }

    /// <summary>
    /// Wraps a byte array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given byte array; that is, modifications
    /// to the buffer will cause the array to be modified and vice versa. The new buffer's
    /// capacity will be <tt>array.length</tt>, its position will be <tt>offset</tt>, its
    /// limit will be <tt>offset + length</tt>, and its mark will be undefined. Its
    /// backing array will be the given array, and its <see cref="ArrayOffset"/> will be zero.
    /// </para>
    /// </summary>
    /// <param name="array">The array that will back the new buffer</param>
    /// <param name="offset">
    /// The offset of the subarray to be used; must be non-negative and no larger
    /// than <tt>array.length</tt>. The new buffer's position will be set to this value.
    /// </param>
    /// <param name="length">
    /// The length of the subarray to be used; must be non-negative and no larger
    /// than <tt>array.length - offset</tt>. The new buffer's limit will be set to
    /// <tt>offset + length</tt>.
    /// </param>
    /// <returns> The new byte buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt> parameters do not hold
    /// </exception>
    public static ByteBuffer Wrap( byte[] array, int offset, int length )
    {
        try
        {
            return new HeapByteBuffer( array, offset, length );
        }
        catch ( ArgumentException )
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Wraps a byte array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given byte array; that is, modifications
    /// to the buffer will cause the array to be modified and vice versa. The new buffer's
    /// capacity will be <tt>array.length</tt>, its position will be <tt>offset</tt>, its
    /// limit will be <tt>offset + length</tt>, and its mark will be undefined. Its
    /// backing array will be the given array, and its <see cref="ArrayOffset"/> will be zero.
    /// </para>
    /// </summary>
    /// <param name="array">The array that will back the new buffer</param>
    /// <returns> The new byte buffer </returns>
    public static ByteBuffer Wrap( byte[] array )
    {
        return Wrap( array, 0, array.Length );
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// <para>
    /// This method transfers bytes from this buffer into the given destination
    /// array. If there are fewer bytes remaining in the buffer than are required
    /// to satisfy the request, that is, if <tt>length &gt; remaining()</tt>, then no
    /// bytes are transferred and a <see cref="GdxRuntimeException"/> is thrown.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <tt>length</tt> bytes from this buffer into the
    /// given array, starting at the current position of this buffer and at the given
    /// offset in the array. The position of this buffer is then incremented by <tt>length</tt>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form <tt>src.get(dst, off, len)</tt>
    /// has exactly the same effect as the loop
    /// <code>
    ///             for ( int i = off; i &lt; off + len; i++ )
    ///             {
    ///                 dst[i] = src.get():
    ///             }
    ///         </code>
    /// except that it first checks that there are sufficient bytes in
    /// this buffer and it is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="dst"> The array into which bytes are to be written </param>
    /// <param name="offset">
    /// The offset within the array of the first byte to be written; must be
    /// non-negative and no larger than <tt>dst.length</tt>
    /// </param>
    /// <param name="length">
    /// The maximum number of bytes to be written to the given array; must be
    /// non-negative and no larger than <tt>dst.length - offset</tt>
    /// </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than <tt>length</tt> bytes remaining in this buffer
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt> parameters do not hold.
    /// </exception>
    public virtual ByteBuffer Get( byte[] dst, int offset, int length )
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
    /// Transfers bytes from this buffer into the given destination array. An
    /// invocation of this method with the destination array <paramref name="dst"/>
    /// behaves in exactly the same way as invoking:-
    /// <code>
    ///     src.Get(dst, 0, dst.Length);
    ///     </code>
    /// </summary>
    /// <param name="dst">The destination array.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than <tt> dst.Length </tt> bytes remaining in this buffer.
    /// </exception>
    public ByteBuffer Get( byte[] dst )
    {
        return Get( dst, 0, dst.Length );
    }

    /// <summary>
    /// Transfers the bytes remaining in the given source buffer into this buffer.
    /// If there are more bytes remaining in the source buffer than in this buffer,
    /// no bytes are transferred, and a <see cref="GdxRuntimeException"/> is thrown.
    /// </summary>
    /// <param name="src">
    /// The source buffer from which bytes are to be read; must not be this buffer.
    /// </param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer for the remaining bytes in the
    /// source buffer, or if this buffer is read-only.
    /// </exception>
    /// <exception cref="ArgumentException">If the source buffer is this buffer.</exception>
    public virtual ByteBuffer Put( ByteBuffer src )
    {
        if ( src.Equals( this ) )
        {
            throw new ArgumentException( "Source buffer cannot be this buffer!" );
        }

        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Buffer is readonly!" );
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
    /// Transfers bytes into this buffer from the given source array. If there are
    /// more bytes to be copied from the array than remain in this buffer, no bytes
    /// are transferred, and a <see cref="GdxRuntimeException"/> is thrown.
    /// </summary>
    /// <param name="src">The array from which bytes are to be read.</param>
    /// <param name="offset">
    /// The offset within the array of the first byte to be read; must be non-negative
    /// and no larger than <paramref name="src"/>.Length.
    /// </param>
    /// <param name="length">
    /// The number of bytes to be read from the given array; must be non-negative
    /// and no larger than <paramref name="src"/>.Length - <paramref name="offset"/>.
    /// </param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer, or if this buffer is read-only.
    /// ( With appropriate message ).
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <paramref name="offset"/> and <paramref name="length"/>
    /// parameters do not hold.
    /// </exception>
    public virtual ByteBuffer Put( byte[] src, int offset, int length )
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
    /// Transfers the entire content of the given source byte array into this buffer.
    /// An invocation of this method with the source byte array <paramref name="src"/>
    /// behaves in exactly the same way as invoking <c>dst.Put(src, 0, src.Length)</c>.
    /// </summary>
    /// <param name="src">The source byte array.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer.
    /// </exception>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer, or if this buffer is read-only.
    /// ( With appropriate message ).
    /// </exception>
    public ByteBuffer Put( byte[] src )
    {
        return Put( src, 0, src.Length );
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible byte array.
    /// <para>
    /// If this method returns <tt>true</tt> then the <see cref="Array()"/> and
    /// <see cref="ArrayOffset()"/> methods may safely be invoked.
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
    /// Returns the byte array that backs this buffer <i>(optional operation)</i>.
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
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is backed by an array but is read-only, or
    /// If this buffer is not backed by an accessible array
    /// </exception>
    public new byte[] BackingArray()
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Backing array is null!" );
        }

        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Buffer is Read Only!" );
        }

        return Hb;
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
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is backed by an array but is read-only, or
    /// If this buffer is not backed by an accessible array
    /// </exception>
    public override int ArrayOffset()
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Backing array is null!" );
        }

        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Buffer is Read Only!" );
        }

        return Offset;
    }

    /// <summary>
    /// Compacts this buffer  <i>(optional operation)</i>.
    /// <para>
    /// The bytes between the buffer's current position and its limit, if any,
    /// are copied to the beginning of the buffer. That is, the byte at index
    /// <tt>p = Position()</tt> is copied to index zero, the byte at index
    /// <tt>p + 1</tt> is copied to index one, and so forth until the byte at
    /// index <tt>limit() - 1</tt> is copied to index <tt>n = Limit() - 1 - p</tt>.
    /// The buffer's position is then set to <i>n+1</i> and its limit is set to
    /// its capacity. The mark, if defined, is discarded.
    /// </para>
    /// <para>
    /// The buffer's position is set to the number of bytes copied, rather than
    /// to zero, so that an invocation of this method can be followed immediately
    /// by an invocation of another relative <tt>put</tt> method.
    /// </para>
    /// <para>
    /// Invoke this method after writing data from a buffer in case the write was
    /// incomplete. The following loop, for example, copies bytes from one channel
    /// to another via the buffer <tt>buf</tt>:
    /// <code>
    ///   buf.clear();          // Prepare buffer for use
    ///   while( in.read(buf) >= 0 || buf.position != 0 )
    ///   {
    ///       buf.flip();
    ///       out.write(buf);
    ///       buf.compact();    // In case of partial write
    ///   }
    /// </code>
    /// </para>
    /// </summary>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException"> If this buffer is read-only </exception>
    public abstract ByteBuffer Compact();

    /// <summary>
    /// Returns a string summarizing the state of this buffer.
    /// </summary>
    /// <returns> A summary string </returns>
    public override string ToString()
    {
        return $"{GetType().Name} [pos={Position} lim={Limit} cap={Capacity}]";
    }

    /// <summary>
    /// Returns the current hash code of this buffer.
    /// <para>
    /// The hash code of a byte buffer depends only upon its remaining elements;
    /// that is, upon the elements from <tt>position()</tt> up to, and including,
    /// the element at <tt>limit()</tt> - <tt>1</tt>.
    /// </para>
    /// <para>
    /// Because buffer hash codes are content-dependent, it is inadvisable to use
    /// buffers as keys in hash maps or similar data structures unless it is known
    /// that their contents will not change.
    /// </para>
    /// </summary>
    /// <returns> The current hash code of this buffer </returns>
    public int HashCode()
    {
        var h = 1;
        var p = Position;

        for ( var i = Limit - 1; i >= p; i-- )
        {
            h = ( 31 * h ) + Get( i );
        }

        return h;
    }

    /// <summary>
    /// Tells whether or not this buffer is equal to another object.
    /// <para>
    /// Two byte buffers are equal if, and only if,
    /// <li>They have the same element type,</li>
    /// <li>They have the same number of remaining elements, and,</li>
    /// <li>
    /// The two sequences of remaining elements, considered independently
    /// of their starting positions, are pointwise equal.
    /// </li>
    /// </para>
    /// <para> A byte buffer is not equal to any other type of object. </para>
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

        if ( ob is not ByteBuffer that )
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
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode();
    }

    /// <summary>
    /// Compares this buffer to another.
    /// <para>
    /// Two byte buffers are compared by comparing their sequences of remaining
    /// elements lexicographically, without regard to the starting position of
    /// each sequence within its corresponding buffer.
    /// </para>
    /// <para>
    /// A byte buffer is not comparable to any other type of object.
    /// </para>
    /// </summary>
    /// <returns>
    /// A negative integer, zero, or a positive integer as this buffer is less than, equal
    /// to, or greater than the given buffer
    /// </returns>
    public int CompareTo( ByteBuffer that )
    {
        var n = Position + Math.Min( Remaining(), that.Remaining() );

        for ( int i = Position, j = that.Position; i < n; i++, j++ )
        {
            var cmp = BufferUtils.Compare( Get( i ), that.Get( j ) );

            if ( cmp != 0 )
            {
                return cmp;
            }
        }

        return Remaining() - that.Remaining();
    }

    /// <summary>
    /// Retrieves this buffer's byte order.
    /// <para>
    /// The byte order is used when reading or writing multibyte values, and
    /// when creating buffers that are views of this byte buffer. The order of
    /// a newly-created byte buffer is always <see cref="ByteOrder.BigEndian"/>.
    /// </para>
    /// </summary>
    /// <returns> This buffer's byte order </returns>
    public ByteOrder Order()
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
    public ByteBuffer Order( ByteOrder order )
    {
        BigEndian       = order == ByteOrder.BigEndian;
        NativeByteOrder = BigEndian == ( Bits.ByteOrder == ByteOrder.BigEndian );

        return this;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region abstract methods

    /// <summary>
    /// Helper method for creating an index value using the supplied int
    /// value and any other valid value, depending upon implementation.
    /// </summary>
    /// <returns> The calculated index. </returns>
    protected abstract int Ix( int i );

    /// <summary>
    /// Creates a new byte buffer whose content is a shared subsequence of
    /// this buffer's content.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer, and its mark
    /// will be undefined.  The new buffer will be direct if, and only if, this
    /// buffer is direct, and it will be read-only if, and only if, this buffer
    /// is read-only.
    /// </para>
    /// </summary>
    /// <returns> The new byte buffer </returns>
    public abstract ByteBuffer Slice();

    /// <summary>
    /// Creates a new byte buffer that shares this buffer's content.
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
    /// <returns> The new byte buffer. </returns>
    public abstract ByteBuffer Duplicate();

    /// <summary>
    /// Creates a new, read-only byte buffer that shares this buffer's
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
    /// <returns> The new, read-only byte buffer </returns>
    public abstract ByteBuffer AsReadOnlyBuffer();


    // -- Singleton get/put methods --

    /// <summary>
    /// Relative <tt>get</tt> method.  Reads the byte at this buffer's
    /// current position, and then increments the position.
    /// </summary>
    /// <returns> The byte at the buffer's current position </returns>
    /// <exception cref="GdxRuntimeException">
    /// If the buffer's current position is not smaller than its limit
    /// </exception>
    public abstract byte Get();

    /// <summary>
    /// Relative <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// Writes the given byte into this buffer at the current
    /// position, and then increments the position.
    /// </para>
    /// </summary>
    /// <param name="b"> The byte to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer's current position is not smaller than its limit,
    /// or if this buffer is read-only
    /// </exception>
    public abstract ByteBuffer Put( byte b );

    /// <summary>
    /// Absolute <i>get</i> method.  Reads the byte at the given index.
    /// </summary>
    /// <param name="index">"> The index from which the byte will be read </param>
    /// <returns> The byte at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    public abstract byte Get( int index );

    /// <summary>
    /// Absolute <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// Writes the given byte into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index">"> The index at which the byte will be written </param>
    /// <param name="b"> The byte value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    /// <exception cref="GdxRuntimeException"> If this buffer is read-only </exception>
    public abstract ByteBuffer Put( int index, byte b );

    #endregion abstract methods

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region Abstract Methods, for use by ByteBufferAs-X-Buffer classes

    /// <summary>
    /// Relative <i>get</i> method for reading a char value.
    /// <para>
    /// Reads the next two bytes at this buffer's current position, composing them into a
    /// char value according to the current byte order, and then increments the position by two.
    /// </para>
    /// </summary>
    /// <returns> The char value at the buffer's current position </returns>
    /// <exception cref="BufferUnderflowException">
    /// If there are fewer than two bytes remaining in this buffer
    /// </exception>
    public abstract char GetChar();

    /// <summary>
    /// Relative <i>put</i> method for writing a char value  <i>(optional operation)</i>.
    /// <para>
    /// Writes two bytes containing the given char value, in the current byte order, into
    /// this buffer at the current position, and then increments the position by two.
    /// </para>
    /// </summary>
    /// <param name="value"> The char value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If there are fewer than two bytes remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutChar( char value );

    /// <summary>
    /// Absolute <i>get</i> method for reading a char value.
    /// <para>
    /// Reads two bytes at the given index, composing them into a char value according
    /// to the current byte order.
    /// </para>
    /// </summary>
    /// <param name="index"> The index from which the bytes will be read </param>
    /// <returns> The char value at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus one
    /// </exception>
    public abstract char GetChar( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing a char value  <i>(optional operation)</i>.
    /// <para>
    /// Writes two bytes containing the given char value, in the current byte order, into
    /// this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index"> The index at which the bytes will be written </param>
    /// <param name="value"> The char value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus one
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutChar( int index, char value );

    /// <summary>
    /// Creates a view of this byte buffer as a char buffer.
    /// <para>
    /// The content of the new buffer will start at this buffers current position. Changes to
    /// this buffer's content will be visible in the new buffer, and vice versa; the two buffers'
    /// position, limit, and mark values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit will be the number of
    /// bytes remaining in this buffer divided by two, and its mark will be undefined. The new
    /// buffer will be direct if, and only if, this buffer is direct, and it will be read-only
    /// if, and only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns> A new char buffer </returns>
    public abstract CharBuffer AsCharBuffer();

    /// <summary>
    /// Relative <i>get</i> method for reading a short value.
    /// <para>
    /// Reads the next two bytes at this buffer's current position,
    /// composing them into a short value according to the current byte order,
    /// and then increments the position by two.
    /// </para>
    /// </summary>
    /// <returns> The short value at the buffer's current position </returns>
    /// <exception cref="BufferUnderflowException">
    /// If there are fewer than two bytes remaining in this buffer
    /// </exception>
    public abstract short GetShort();

    /// <summary>
    /// Relative <i>put</i> method for writing a short value  <i>(optional operation)</i>.
    /// <para>
    /// Writes two bytes containing the given short value, in the
    /// current byte order, into this buffer at the current position, and then
    /// increments the position by two.
    /// </para>
    /// </summary>
    /// <param name="value"> The short value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If there are fewer than two bytes remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutShort( short value );

    /// <summary>
    /// Absolute <i>get</i> method for reading a short value.
    /// <para>
    /// Reads two bytes at the given index, composing them into a
    /// short value according to the current byte order.
    /// </para>
    /// </summary>
    /// <param name="index"> The index from which the bytes will be read </param>
    /// <returns> The short value at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus one
    /// </exception>
    public abstract short GetShort( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing a short value  <i>(optional operation)</i>.
    /// <para>
    /// Writes two bytes containing the given short value, in the
    /// current byte order, into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index"> The index at which the bytes will be written </param>
    /// <param name="value"> The short value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus one
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutShort( int index, short value );

    /// <summary>
    /// Creates a view of this byte buffer as a short buffer.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position. Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// two, and its mark will be undefined.  The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns> A new short buffer </returns>
    public abstract ShortBuffer AsShortBuffer();


    /// <summary>
    /// Relative <i>get</i> method for reading an int value.
    /// <para>
    /// Reads the next four bytes at this buffer's current position,
    /// composing them into an int value according to the current byte order,
    /// and then increments the position by four.
    /// </para>
    /// </summary>
    /// <returns> The int value at the buffer's current position </returns>
    /// <exception cref="BufferUnderflowException">
    /// If there are fewer than four bytes remaining in this buffer
    /// </exception>
    public abstract int GetInt();

    /// <summary>
    /// Relative <i>put</i> method for writing an int value  <i>(optional operation)</i>.
    /// <para>
    /// Writes four bytes containing the given int value, in the
    /// current byte order, into this buffer at the current position, and then
    /// increments the position by four.
    /// </para>
    /// </summary>
    /// <param name="value"> The int value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If there are fewer than four bytes remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutInt( int value );

    /// <summary>
    /// Absolute <i>get</i> method for reading an int value.
    /// <para>
    /// Reads four bytes at the given index, composing them into a
    /// int value according to the current byte order.
    /// </para>
    /// </summary>
    /// <param name="index"> The index from which the bytes will be read </param>
    /// <returns> The int value at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus three
    /// </exception>
    public abstract int GetInt( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing an int value  <i>(optional operation)</i>.
    /// <para>
    /// Writes four bytes containing the given int value, in the
    /// current byte order, into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index"> The index at which the bytes will be written </param>
    /// <param name="value"> The int value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus three
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutInt( int index, int value );

    /// <summary>
    /// Creates a view of this byte buffer as an int buffer.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// four, and its mark will be undefined.  The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns> A new int buffer </returns>
    public abstract IntBuffer AsIntBuffer();

    /// <summary>
    /// Relative <i>get</i> method for reading a long value.
    /// <para>
    /// Reads the next eight bytes at this buffer's current position, composing them
    /// into a long value according to the current byte order, and then increments
    /// the position by eight.
    /// </para>
    /// </summary>
    /// <returns> The long value at the buffer's current position </returns>
    /// <exception cref="BufferUnderflowException">
    /// If there are fewer than eight bytes remaining in this buffer
    /// </exception>
    public abstract long GetLong();

    /// <summary>
    /// Relative <i>put</i> method for writing a long value  <i>(optional operation)</i>.
    /// <para>
    /// Writes eight bytes containing the given long value, in the current byte order,
    /// into this buffer at the current position, and then increments the position by eight.
    /// </para>
    /// </summary>
    /// <param name="value"> The long value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If there are fewer than eight bytes remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutLong( long value );

    /// <summary>
    /// Absolute <i>get</i> method for reading a long value.
    /// <para>
    /// Reads eight bytes at the given index, composing them into a
    /// long value according to the current byte order.
    /// </para>
    /// </summary>
    /// <param name="index"> The index from which the bytes will be read </param>
    /// <returns> The long value at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus seven
    /// </exception>
    public abstract long GetLong( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing a long
    /// value  <i>(optional operation)</i>.
    /// <para>
    /// Writes eight bytes containing the given long value, in the
    /// current byte order, into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index"> The index at which the bytes will be written </param>
    /// <param name="value"> The long value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus seven
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutLong( int index, long value );

    /// <summary>
    /// Creates a view of this byte buffer as a long buffer.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// eight, and its mark will be undefined.  The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns> A new long buffer </returns>
    public abstract LongBuffer AsLongBuffer();

    /// <summary>
    /// Relative <i>get</i> method for reading a float value.
    /// <para>
    /// Reads the next four bytes at this buffer's current position, composing them
    /// into a float value according to the current byte order, and then increments
    /// the position by four.
    /// </para>
    /// </summary>
    /// <returns> The float value at the buffer's current position </returns>
    /// <exception cref="BufferUnderflowException">
    /// If there are fewer than four bytes remaining in this buffer
    /// </exception>
    public abstract float GetFloat();

    /// <summary>
    /// Relative <i>put</i> method for writing a float value  <i>(optional operation)</i>.
    /// <para>
    /// Writes four bytes containing the given float value, in the current byte order,
    /// into this buffer at the current position, and then increments the position by four.
    /// </para>
    /// </summary>
    /// <param name="value">"> The float value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If there are fewer than four bytes remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutFloat( float value );

    /// <summary>
    /// Absolute <i>get</i> method for reading a float value.
    /// <para>
    /// Reads four bytes at the given index, composing them into a
    /// float value according to the current byte order.
    /// </para>
    /// </summary>
    /// <param name="index">"> The index from which the bytes will be read </param>
    /// <returns> The float value at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus three
    /// </exception>
    public abstract float GetFloat( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing a float value  <i>(optional operation)</i>.
    /// <para>
    /// Writes four bytes containing the given float value, in the
    /// current byte order, into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index">"> The index at which the bytes will be written </param>
    /// <param name="value">"> The float value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus three
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer PutFloat( int index, float value );

    /// <summary>
    /// Creates a view of this byte buffer as a float buffer.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// four, and its mark will be undefined.  The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns> A new float buffer </returns>
    public abstract FloatBuffer AsFloatBuffer();

    /// <summary>
    /// Reads the next eight bytes at this buffer's current position, composing them
    /// into a double value according to the current byte order, and then increments
    /// the position by eight.
    /// </summary>
    /// <returns>The double value at the buffer's current position.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than eight bytes remaining in this buffer.
    /// </exception>
    public abstract double GetDouble();

    /// <summary>
    /// Reads eight bytes at the given index, composing them into a double value
    /// according to the current byte order.
    /// </summary>
    /// <param name="index">">The index from which the bytes will be read.</param>
    /// <returns>The double value at the given index.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="index"/> is negative or not smaller than the buffer's limit, minus seven.
    /// </exception>
    public abstract double GetDouble( int index );

    /// <summary>
    /// Absolute <tt>get</tt> method for reading a double value.
    /// Writes eight bytes containing the given double value, in the current byte
    /// order, into this buffer at the current position, and then increments the
    /// position by eight.
    /// </summary>
    /// <param name="value">">The double value to be written.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than eight bytes remaining in this buffer.
    /// </exception>
    /// <exception cref="GdxRuntimeException">If this buffer is read-only.</exception>
    public abstract ByteBuffer PutDouble( double value );

    /// <summary>
    /// Absolute <tt>put</tt> method for writing a double value <tt>(optional operation)</tt>.
    /// <para>
    /// Writes eight bytes containing the given double value, in the current byte order,
    /// into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index">"> The index at which the bytes will be written </param>
    /// <param name="value">"> The double value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus 7
    /// </exception>
    /// <exception cref="GdxRuntimeException">
    /// if this buffer is read-only
    /// </exception>
    public abstract ByteBuffer PutDouble( int index, double value );

    /// <summary>
    /// Creates a view of this byte buffer as a double buffer.
    /// <para>
    /// The content of the new buffer will start at this buffer's current position.
    /// Changes to this buffer's content will be visible in the new buffer, and vice
    /// versa; the two buffers' position, limit, and mark values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit will be the
    /// number of bytes remaining in this buffer divided by eight, and its mark will be
    /// undefined. The new buffer will be direct if, and only if, this buffer is direct,
    /// and it will be read-only if, and only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns> A new double buffer. </returns>
    public abstract DoubleBuffer AsDoubleBuffer();

    #endregion Abstract Methods, for use by ByteBufferAs-X-Buffer classes

    // ------------------------------------------------------------------------
}
