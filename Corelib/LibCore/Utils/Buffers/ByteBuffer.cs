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

using System.Buffers.Binary;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Utils.Buffers;

/// <summary>
/// A byte buffer.
/// This class defines six categories of operations upon byte buffers:
/// <li>
/// Absolute and relative <see cref="Get()"/> and <see cref="Put(byte)"/> methods
/// that read and write single bytes.
/// </li>
/// <li>
/// Relative <see cref="Get(byte[])"/> methods that transfer contiguous sequences of bytes
/// from this buffer into an array.
/// </li>
/// <li>
/// Relative <see cref="Put(byte[])"/> methods that transfer contiguous sequences  of bytes
/// from a byte array or some other byte buffer into this buffer.
/// </li>
/// <li>
/// Absolute and relative <see cref="GetChar()"/> and <see cref="PutChar(char)"/> methods that
/// read and write values of other primitive types, translating them to and from sequences of
/// bytes in a particular byte order.
/// </li>
/// <li>
/// Methods for creating view buffers, which allow a byte buffer to be viewed as a buffer containing
/// values of some other primitive type.
/// </li>
/// <li>
/// Methods for <see cref="Compact"/>ing, <see cref="Duplicate"/>ing, and <see cref="Slice"/>ing a
/// byte buffer.
/// </li>
/// <para>
/// Byte buffers can be created either by <see cref="Allocate"/>, which allocates space for the
/// buffers content, or by <see cref="Wrap(byte[])"/> an existing byte array into a buffer.
/// </para>
/// <b> Access to binary data </b>
/// <para>
/// This class defines methods for reading and writing values of all other primitive types, except
/// <c>bool</c>. Primitive values are translated to (or from) sequences of bytes according to the
/// buffer's current byte order, which may be retrieved and modified via the <see cref="Order()"/>
/// methods. Specific byte orders are represented by instances of the <see cref="ByteOrder"/> class.
/// The initial order of a byte buffer is always <see cref="ByteOrder.BigEndian"/>.
/// </para>
/// <para>
/// For access to heterogeneous binary data, that is, sequences of values of different types, this
/// class defines a family of absolute and relative <i>Get</i> and <i>Put</i> methods for each type.
/// For 32-bit floating-point values, for example, this class defines:
/// </para>
/// <code>
///     float GetFloat()
///     float GetFloat(int)
///     void PutFloat(float)
///     void PutFloat(int,float)
/// </code>
/// <para>
/// Corresponding methods are defined for the types <c>char</c>, <c>short</c>, <c>int</c>, <c>long</c>,
/// and <c>double</c>. The index parameters of the absolute <i>Get</i> and <i>Put</i> methods are in
/// terms of bytes rather than of the type being read or written.
/// </para>
/// <b> View Buffers. </b>
/// <para>
/// For access to homogeneous binary data, that is, sequences of values of the same type, this class
/// defines methods that can create <i>views</i> of a given byte buffer. A <i>view buffer</i> is simply
/// another buffer whose content is backed by the byte buffer. Changes to the byte buffer's content
/// will be visible in the view buffer, and vice versa; the two buffers' position, limit, and mark
/// values are independent.
/// </para>
/// <para>
/// View buffers have some important advantages over the families of type-specific <i>Get</i>
/// and <i>Put</i> methods described above:
/// <li>
/// A view buffer is indexed not in terms of bytes but rather in terms of the type-specific
/// size of its values.
/// </li>
/// <li>
/// A view buffer provides relative bulk <i>Get</i> and <i>Put</i> methods that can transfer
/// contiguous sequences of values between a buffer and an array or some other buffer of the
/// same type.
/// The byte order of a view buffer is fixed to be that of its byte buffer at the time that
/// the view is created.
/// </li>
/// </para>
/// <b> Invocation chaining </b>
/// <para>
/// Methods in this class that do not otherwise have a value to return are specified to return
/// the buffer upon which they are invoked. This allows method invocations to be chained.
/// </para>
/// <para>
/// The sequence of statements:-
/// </para>
/// <code>
///         bb.PutInt( 0xCAFEBABE );
///         bb.PutShort( 3 );
///         bb.PutShort( 45 );
///     </code>
/// can, for example, be replaced by the single statement
/// <code>
///     bb.PutInt( 0xCAFEBABE ).PutShort( 3 ).PutShort( 45 );
/// </code>
/// </summary>
[PublicAPI]
public abstract class ByteBuffer : Buffer
{
    public new byte[]? Hb { get; set; }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new buffer with the given mark, position, limit, capacity,
    /// backing array, and array offset
    /// </summary>
    protected ByteBuffer( int mark, int pos, int lim, int cap, byte[]? hb = null, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
        Hb          = hb ?? new byte[ cap ];
        Offset      = offset;
        IsBigEndian = !BitConverter.IsLittleEndian;

        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    /// <summary>
    /// Allocates a new direct byte buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its capacity,
    /// its mark will be undefined, each of its elements will be initialized to
    /// zero, and its byte order will be <see cref="ByteOrder.BigEndian"/>.
    /// Whether or not it has a <see cref="HasBackingArray"/> backing array is
    /// unspecified.
    /// </para>
    /// </summary>
    /// <param name="capacity"> The new buffer's capacity, in bytes. </param>
    /// <returns> The new byte buffer. </returns>
    /// <exception cref="ArgumentException">
    /// If the <see cref="capacity"/> is a negative integer
    /// </exception>
    public static ByteBuffer AllocateDirect( int capacity )
    {
        return Allocate( capacity );
    }

    /// <summary>
    /// Allocates a new byte buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its capacity, its
    /// mark will be undefined, and each of its elements will be initialized to zero.
    /// It will have a backing array and its <see cref="ArrayOffset"/> will be zero.
    /// </para>
    /// </summary>
    /// <param name="capacity"> The new buffer's capacity, in bytes </param>
    /// <returns> The new byte buffer </returns>
    /// <exception cref="ArgumentException">
    /// If the <c>capacity</c> is a negative integer
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
    /// capacity will be <c>array.length</c>, its position will be <c>offset</c>, its
    /// limit will be <c>offset + length</c>, and its mark will be undefined. Its
    /// backing array will be the given array, and its <see cref="ArrayOffset"/> will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back the new buffer </param>
    /// <param name="offset">
    /// The offset of the subarray to be used; must be non-negative and no larger
    /// than <c>array.length</c>. The new buffer's position will be set to this value.
    /// </param>
    /// <param name="length">
    /// The length of the subarray to be used; must be non-negative and no larger
    /// than <c>array.length - offset</c>. The new buffer's limit will be set to
    /// <c>offset + length</c>.
    /// </param>
    /// <returns> The new byte buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <c>offset</c> and <c>length</c> parameters do not hold
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
    /// capacity will be <c>array.length</c>, its position will be <c>offset</c>, its
    /// limit will be <c>offset + length</c>, and its mark will be undefined. Its
    /// backing array will be the given array, and its <see cref="ArrayOffset"/> will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back the new buffer </param>
    /// <returns> The new byte buffer </returns>
    public static ByteBuffer Wrap( byte[] array = null! )
    {
        return Wrap( array, 0, array.Length );
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// <para>
    /// This method transfers bytes from this buffer into the given destination
    /// array. If there are fewer bytes remaining in the buffer than are required
    /// to satisfy the request, that is, if <c>length &gt; remaining()</c>, then no
    /// bytes are transferred and a <see cref="GdxRuntimeException"/> is thrown.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <c>length</c> bytes from this buffer into the
    /// given array, starting at the current position of this buffer and at the given
    /// offset in the array. The position of this buffer is then incremented by <c>length</c>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form <c>src.get(dst, off, len)</c>
    /// has exactly the same effect as the loop
    /// <code>
    ///     for ( int i = off; i &lt; off + len; i++ )
    ///     {
    ///         dst[i] = src.get():
    ///     }
    /// </code>
    /// except that it first checks that there are sufficient bytes in
    /// this buffer and it is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="dst"> The array into which bytes are to be written </param>
    /// <param name="offset">
    /// The offset within the array of the first byte to be written; must be
    /// non-negative and no larger than <c>dst.length</c>
    /// </param>
    /// <param name="length">
    /// The maximum number of bytes to be written to the given array; must be
    /// non-negative and no larger than <c>dst.length - offset</c>
    /// </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than <c>length</c> bytes remaining in this buffer
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <c>offset</c> and <c>length</c> parameters do not hold.
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
    /// Transfers bytes from this buffer into the given destination buffer. An
    /// invocation of this method with the destination array <paramref name="dst"/>
    /// behaves in exactly the same way as invoking:-
    /// <code>
    ///     src.Get(dst, 0, dst.Length);
    /// </code>
    /// </summary>
    /// <param name="dst">The destination array.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there are fewer than <c> dst.Length </c> bytes remaining in this buffer.
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
        if ( IsReadOnly )
        {
            return this;
        }

        if ( src.Equals( this ) )
        {
            throw new ArgumentException( "Source buffer cannot be this buffer!" );
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
    /// Transfers the entire content of the given source byte array into this buffer.
    /// An invocation of this method with the source byte array <paramref name="src"/>
    /// behaves in exactly the same way as invoking <c>dst.Put(src, 0, src.Length)</c>.
    /// </summary>
    /// <param name="src">The source byte array.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="GdxRuntimeException">
    /// If there is insufficient space in this buffer, with appropriate message.
    /// </exception>
    public ByteBuffer Put( byte[] src )
    {
        return Put( src, 0, src.Length );
    }

    /// <summary>
    /// Returns an Integer value from the buffer at the current position.
    /// </summary>
    public int GetInt()
    {
        return IsBigEndian
            ? BinaryPrimitives.ReadInt32BigEndian( Hb.AsSpan( Position ) )
            : BinaryPrimitives.ReadInt32LittleEndian( Hb.AsSpan( Position ) );
    }

    /// <summary>
    /// Returns an Integer value from the buffer at the specified index.
    /// </summary>
    public int GetInt( int index )
    {
        return IsBigEndian
            ? BinaryPrimitives.ReadInt32BigEndian( Hb.AsSpan( index ) )
            : BinaryPrimitives.ReadInt32LittleEndian( Hb.AsSpan( index ) );
    }

    /// <summary>
    /// Puts the supplied Integer value into the buffer at the <see cref="Buffer.NextPutIndex()"/>.
    /// </summary>
    /// <param name="value"> The value to add. </param>
    /// <returns> This buffer for chaining. </returns>
    public ByteBuffer PutInt( int value )
    {
        if ( IsBigEndian )
        {
            BinaryPrimitives.WriteInt32BigEndian( Hb.AsSpan( NextPutIndex( sizeof( int ) ) ), value );
        }
        else
        {
            BinaryPrimitives.WriteInt32LittleEndian( Hb.AsSpan( NextPutIndex( sizeof( int ) ) ), value );
        }

        return this;
    }

    /// <summary>
    /// Puts the supplied Integer value into the buffer at the supplied index.
    /// </summary>
    /// <param name="index"> The required buffer index. </param>
    /// <param name="value"> The value to add. </param>
    /// <returns> This buffer for chaining. </returns>
    public ByteBuffer PutInt( int index, int value )
    {
        if ( IsBigEndian )
        {
            BinaryPrimitives.WriteInt32BigEndian( Hb.AsSpan( index ), value );
        }
        else
        {
            BinaryPrimitives.WriteInt32LittleEndian( Hb.AsSpan( index ), value );
        }

        return this;
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible byte array.
    /// If this method returns <c>true</c> then the <see cref="Array()"/> and
    /// <see cref="ArrayOffset()"/> methods may safely be invoked.
    /// </summary>
    /// <returns>
    /// <c>true</c> if, and only if, this buffer is backed by an array and is not read-only
    /// </returns>
    public override bool HasBackingArray()
    {
        return ( Hb != null ) && !IsReadOnly;
    }

    /// <summary>
    /// Returns the byte array that backs this buffer <i>(optional operation)</i>.
    /// Modifications to this buffer's content will cause the returned array's
    /// content to be modified, and vice versa.
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
        if ( IsReadOnly )
        {
            throw new GdxRuntimeException( "Buffer is Read Only!" );
        }

        return Hb!;
    }

    /// <summary>
    /// Returns the offset within this buffer's backing array of the first
    /// element of the buffer <i>(optional operation)</i>.
    /// <para>
    /// If this buffer is backed by an array then buffer position <i>p</i>
    /// corresponds to array index <i>p</i> + <c>arrayOffset()</c>.
    /// </para>
    /// <para>
    /// Invoke the <see cref="HasBackingArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.
    /// </para>
    /// </summary>
    /// <returns> The offset within this buffer's array of the first element of the buffer </returns>
    /// <exception cref="GdxRuntimeException">
    /// If this buffer is backed by an array but is read-only, or
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
    /// The bytes between the buffer's current position and its limit, if any,
    /// are copied to the beginning of the buffer. That is, the byte at index
    /// <c>p = Position()</c> is copied to index zero, the byte at index
    /// <c>p + 1</c> is copied to index one, and so forth until the byte at
    /// index <c>limit() - 1</c> is copied to index <c>n = Limit() - 1 - p</c>.
    /// The buffer's position is then set to <i>n+1</i> and its limit is set to
    /// its capacity. The mark, if defined, is discarded.
    /// </para>
    /// <para>
    /// The buffer's position is set to the number of bytes copied, rather than
    /// to zero, so that an invocation of this method can be followed immediately
    /// by an invocation of another relative <c>put</c> method.
    /// </para>
    /// <para>
    /// Invoke this method after writing data from a buffer in case the write was
    /// incomplete. The following loop, for example, copies bytes from one channel
    /// to another via the buffer <c>buf</c>:
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
    public virtual ByteBuffer Compact()
    {
        if ( IsReadOnly )
        {
            return this;
        }

        ValidateBackingArray();

        Array.Copy( Hb!, Ix( Position ), Hb!, Ix( 0 ), Remaining() );

        SetPosition( Remaining() );
        SetLimit( Capacity );
        DiscardMark();

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer Order( ByteOrder order )
    {
        IsBigEndian = order == ByteOrder.BigEndian;

        return this;
    }

    /// <inheritdoc />
    protected override void ValidateBackingArray()
    {
        if ( Hb == null )
        {
            throw new NullReferenceException( "Backing array is null!" );
        }
    }

    /// <summary>
    /// Returns the current hash code of this buffer.
    /// <para>
    /// The hash code of a byte buffer depends only upon its remaining elements;
    /// that is, upon the elements from <c>position()</c> up to, and including,
    /// the element at <c>limit()</c> - <c>1</c>.
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
    /// <c>true</c> if, and only if, this buffer is equal to the given object
    /// </returns>
    public override bool Equals( object? ob )
    {
        if ( this == ob )
        {
            return true;
        }

        if ( ob is not ByteBuffer other )
        {
            return false;
        }

        if ( Remaining() != other.Remaining() )
        {
            return false;
        }

        var p = Position;

        for ( int i = Limit - 1, j = other.Limit - 1; i >= p; i--, j-- )
        {
            if ( !Equals( Get( i ), other.Get( j ) ) )
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
    public int CompareTo( ByteBuffer other )
    {
        var n = Position + Math.Min( Remaining(), other.Remaining() );

        for ( int i = Position, j = other.Position; i < n; i++, j++ )
        {
            var cmp = NumberUtils.Compare( Get( i ), other.Get( j ) );

            if ( cmp != 0 )
            {
                return cmp;
            }
        }

        return Remaining() - other.Remaining();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Converts this byte buffer into a float buffer.
    /// </summary>
    /// <returns>A float buffer whose content is backed by the byte buffer.</returns>
    public FloatBuffer AsFloatBuffer()
    {
        GdxRuntimeException.ThrowIfNull( Hb );

        var floatCount = ( Hb.Length - Position ) / sizeof( float );
        var floatArray = new float[ floatCount ];

        for ( var i = 0; i < floatCount; i++ )
        {
            floatArray[ i ] = IsBigEndian
                ? BinaryPrimitives.ReadSingleBigEndian( Hb.AsSpan( Position + i * sizeof( float ) ) )
                : BinaryPrimitives.ReadSingleLittleEndian( Hb.AsSpan( Position + i * sizeof( float ) ) );
        }

        return FloatBuffer.Wrap( floatArray );
    }

    /// <summary>
    /// Converts this byte buffer into a short buffer.
    /// </summary>
    /// <returns>A new short buffer backed by this byte buffer.</returns>
    public ShortBuffer AsShortBuffer()
    {
        GdxRuntimeException.ThrowIfNull( Hb );

        var shortCount = ( Hb.Length - Position ) / sizeof( short );
        var shortArray = new short[ shortCount ];

        for ( var i = 0; i < shortCount; i++ )
        {
            shortArray[ i ] = IsBigEndian
                ? BinaryPrimitives.ReadInt16BigEndian( Hb.AsSpan( Position + i * sizeof( short ) ) )
                : BinaryPrimitives.ReadInt16LittleEndian( Hb.AsSpan( Position + i * sizeof( short ) ) );
        }

        return ShortBuffer.Wrap( shortArray );
    }

    /// <summary>
    /// Converts the current byte buffer into an equivalent integer buffer representation.
    /// </summary>
    /// <return>
    /// An integer buffer that shares the content of this byte buffer.
    /// </return>
    public IntBuffer AsIntBuffer()
    {
        GdxRuntimeException.ThrowIfNull( Hb );

        var intCount = ( Hb.Length - Position ) / sizeof( int );
        var intArray = new int[ intCount ];

        for ( var i = 0; i < intCount; i++ )
        {
            intArray[ i ] = IsBigEndian
                ? BinaryPrimitives.ReadInt32BigEndian( Hb.AsSpan( Position + i * sizeof( int ) ) )
                : BinaryPrimitives.ReadInt32LittleEndian( Hb.AsSpan( Position + i * sizeof( int ) ) );
        }

        return IntBuffer.Wrap( intArray );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <inheritdoc cref="IDisposable"/>>
    protected override void Dispose( bool disposing )
    {
        if ( !disposing ) return;
        if ( Hb == null ) return;

        Array.Clear( Hb );
        Hb = null;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region abstract methods

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
    /// Relative <c>get</c> method.  Reads the byte at this buffer's
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
    /// If <c>index</c> is negative or not smaller than the buffer's limit
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
    /// If <c>index</c> is negative or not smaller than the buffer's limit
    /// </exception>
    /// <exception cref="GdxRuntimeException"> If this buffer is read-only </exception>
    public abstract ByteBuffer Put( int index, byte b );

    #endregion abstract methods

    // ------------------------------------------------------------------------
}