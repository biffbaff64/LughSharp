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

namespace LibGDXSharp.Utils.Buffers;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" )]
public abstract class ByteBuffer : Buffer
{
    protected byte[]? Hb     { get; set; }
    protected int     Offset { get; set; }

    private bool _bigEndian       = true;
    private bool _nativeByteOrder = ( Bits.ByteOrder == ByteOrder.BigEndian );

    protected ByteBuffer( int mark, int pos, int lim, int cap, byte[]? hb = null, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
        this.Hb     = hb;
        this.Offset = offset;
    }

    /// <summary>
    /// Allocates a new direct byte buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its capacity, its
    /// mark will be undefined, and each of its elements will be initialized to zero.
    /// Whether or not it has a backing array.
    /// </para>
    /// </summary>
    /// <param name="capacity"> The new buffer's capacity, in bytes </param>
    /// <returns> The new byte buffer </returns>
    /// <exception cref="ArgumentException">
    /// If the <tt>capacity</tt> is a negative integer
    /// </exception>
    public static ByteBuffer AllocateDirect( int capacity )
    {
        return new DirectByteBuffer( capacity );
    }

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
            throw new ArgumentException();
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
    /// The length of the subarray to be used;
    /// must be non-negative and no larger than <tt>array.length - offset</tt>.
    /// The new buffer's limit will be set to <tt>offset + length</tt>.
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

    public static ByteBuffer Wrap( byte[] array )
    {
        return Wrap( array, 0, array.Length );
    }

    public ByteBuffer Get( byte[] dst, int offset, int length )
    {
        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() )
        {
            throw new BufferUnderflowException();
        }

        var end = offset + length;

        for ( var i = offset; i < end; i++ )
        {
            dst[ i ] = Get();
        }

        return this;
    }

    public ByteBuffer Get( byte[] dst )
    {
        return Get( dst, 0, dst.Length );
    }

    public ByteBuffer Put( ByteBuffer src )
    {
        if ( src == this )
        {
            throw new ArgumentException();
        }

        if ( IsReadOnly )
        {
            throw new ReadOnlyBufferException();
        }

        int n = src.Remaining();

        if ( n > Remaining() )
        {
            throw new BufferOverflowException();
        }

        for ( var i = 0; i < n; i++ )
        {
            Put( src.Get() );
        }

        return this;
    }

    public ByteBuffer Put( byte[] src, int offset, int length )
    {
        CheckBounds( offset, length, src.Length );

        if ( length > Remaining() )
        {
            throw new BufferOverflowException();
        }

        var end = offset + length;

        for ( var i = offset; i < end; i++ )
        {
            Put( src[ i ] );
        }

        return this;
    }

    public ByteBuffer Put( byte[] src )
    {
        return Put( src, 0, src.Length );
    }

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
    public override bool HasArray()
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
    /// Invoke the <see cref="HasArray"/> method before invoking this method in
    /// order to ensure that this buffer has an accessible backing array.
    /// </para>
    /// </summary>
    /// <returns>  The array that backs this buffer </returns>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="UnsupportedOperationException">
    ///          If this buffer is not backed by an accessible array
    /// </exception>
    public byte[] Array()
    {
        if ( Hb == null )
        {
            throw new UnsupportedOperationException();
        }

        if ( IsReadOnly )
        {
            throw new ReadOnlyBufferException();
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
    /// Invoke the <seealso cref="HasArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.
    /// </para>
    /// </summary>
    /// <returns>
    /// The offset within this buffer's array of the first element of the buffer </returns>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="UnsupportedOperationException">
    ///          If this buffer is not backed by an accessible array
    /// </exception>
    public override int ArrayOffset()
    {
        if ( Hb == null )
        {
            throw new UnsupportedOperationException();
        }

        if ( IsReadOnly )
        {
            throw new ReadOnlyBufferException();
        }

        return Offset;
    }

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
    /// <returns>  The current hash code of this buffer </returns>
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
    /// </para>
    ///   <li><para> They have the same element type,  </para></li>
    ///   <li><para>
    ///   They have the same number of remaining elements, and
    ///   </para></li>
    ///   <li><para>
    ///   The two sequences of remaining elements, considered independently of
    ///   their starting positions, are pointwise equal.
    ///   </para></li>
    /// <para> A byte buffer is not equal to any other type of object. </para>
    /// </summary>
    /// <param name="ob"> The object to which this buffer is to be compared </param>
    /// <returns>
    /// <tt>true</tt> if, and only if, this buffer is equal to the given object
    /// </returns>
    public override bool Equals( object ob )
    {
        if ( this == ob )
        {
            return true;
        }

        if ( !( ob is ByteBuffer that ) )
        {
            return false;
        }

        if ( this.Remaining() != that.Remaining() )
        {
            return false;
        }

        var p = this.Position;

        for ( int i = this.Limit - 1, j = that.Limit - 1; i >= p; i--, j-- )
        {
            if ( !Equals( this.Get( i ), that.Get( j ) ) )
            {
                return false;
            }
        }

        return true;
    }

    private static bool Equals( byte x, byte y )
    {
        return x == y;
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
        var n = this.Position + Math.Min( this.Remaining(), that.Remaining() );

        for ( int i = this.Position, j = that.Position; i < n; i++, j++ )
        {
            var cmp = BufferUtils.Compare( this.Get( i ), that.Get( j ) );

            if ( cmp != 0 )
            {
                return cmp;
            }
        }

        return this.Remaining() - that.Remaining();
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
        return _bigEndian ? ByteOrder.BigEndian : ByteOrder.LittleEndian;
    }

    /// <summary>
    /// Modifies this buffer's byte order.
    /// </summary>
    /// <param name="bo">
    /// The new byte order, either <see cref="ByteOrder.BigEndian"/>
    /// or <see cref="ByteOrder.LittleEndian"/>
    /// </param>
    /// <returns> This buffer </returns>
    public ByteBuffer Order( ByteOrder bo )
    {
        _bigEndian = ( bo == ByteOrder.BigEndian );

        _nativeByteOrder = ( _bigEndian == ( Bits.ByteOrder == ByteOrder.BigEndian ) );

        return this;
    }

    // ------------------------------------------------------------------------

    #region abstract methods

    public abstract ByteBuffer Slice();

    public abstract ByteBuffer Duplicate();

    public abstract ByteBuffer AsReadOnlyBuffer();

    public abstract byte Get();

    public abstract ByteBuffer Put( byte b );

    public abstract byte Get( int index );

    public abstract ByteBuffer Put( int index, byte b );

    /// <summary>
    /// Compacts this buffer <i>(optional operation)</i>.
    /// <para>
    /// The bytes between the buffer's current position and its limit, if any,
    /// are copied to the beginning of the buffer. That is, the byte at index
    /// <i>p</i> = <tt>position()</tt> is copied to index zero, the byte at index
    /// <i>p</i> + 1 is copied to index one, and so forth until the byte at index
    /// <tt>limit()</tt> - 1 is copied to index <i><tt>n = limit() - 1 - p</tt></i>.
    /// The buffer's position is then set to <i>n+1</i> and its limit is set to
    /// its capacity. The mark, if defined, is discarded.
    /// </para>
    /// <para> The buffer's position is set to the number of bytes copied,
    /// rather than to zero, so that an invocation of this method can be
    /// followed immediately by an invocation of another relative <i>put</i>
    /// method.
    /// </para>
    /// <para>
    /// Invoke this method after writing data from a buffer in case the write
    /// was incomplete. The following loop, for example, copies bytes from one
    /// channel to another via the buffer <tt>buf</tt>:
    /// 
    /// <code>
    ///   buf.clear();          // Prepare buffer for use
    ///   while (in.read(buf) >= 0 || buf.position != 0)
    ///   {
    ///       buf.flip();
    ///       out.write(buf);
    ///       buf.compact();    // In case of partial write
    ///   }
    /// </code>
    /// </para>
    /// </summary>
    /// <returns> This buffer </returns>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public abstract ByteBuffer Compact();

    #endregion abstract methods

    // ------------------------------------------------------------------------

    #region Unchecked accessors, for use by ByteBufferAs-X-Buffer classes

    public abstract char GetChar();

    public abstract ByteBuffer PutChar( char value );

    public abstract char GetChar( int index );

    public abstract ByteBuffer PutChar( int index, char value );

    public abstract CharBuffer AsCharBuffer();

    public abstract short GetShort();

    public abstract ByteBuffer PutShort( short value );

    public abstract short GetShort( int index );

    public abstract ByteBuffer PutShort( int index, short value );

    public abstract ShortBuffer AsShortBuffer();

    public abstract int GetInt();

    public abstract ByteBuffer PutInt( int value );

    public abstract int GetInt( int index );

    public abstract ByteBuffer PutInt( int index, int value );

    public abstract IntBuffer AsIntBuffer();

    public abstract long GetLong();

    public abstract ByteBuffer PutLong( long value );

    public abstract long GetLong( int index );

    public abstract ByteBuffer PutLong( int index, long value );

    public abstract LongBuffer AsLongBuffer();

    public abstract float GetFloat();

    public abstract ByteBuffer PutFloat( float value );

    public abstract float GetFloat( int index );

    public abstract ByteBuffer PutFloat( int index, float value );

    public abstract FloatBuffer AsFloatBuffer();

    public abstract double GetDouble();

    public abstract ByteBuffer PutDouble( double value );

    public abstract double GetDouble( int index );

    public abstract ByteBuffer PutDouble( int index, double value );

    public abstract DoubleBuffer AsDoubleBuffer();

    #endregion Unchecked accessors, for use by ByteBufferAs-X-Buffer classes

    // ------------------------------------------------------------------------
}