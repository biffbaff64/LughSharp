using System.Diagnostics.CodeAnalysis;
using System.Text;

using LibGDXSharp.GdxCore.Utils.Buffers;

namespace LibGDXSharp.Files;

[SuppressMessage( "ReSharper", "UnassignedGetOnlyAutoProperty" )]
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class ByteBuffer : Buffer, IComparable< ByteBuffer >
{
    private readonly byte[] _hb;
    private readonly int    _offset;

    // Valid only for heap buffers.
    public bool isReadOnly;

    protected ByteBuffer()
    {
        _hb = null!;
        isReadOnly = false;S
    }
    
    /// <summary>
    /// Creates a new buffer with the given mark, position, limit, capacity,
    /// backing array, and array offset
    /// </summary>
    protected ByteBuffer( int mark, int pos, int lim, int cap, byte[] hb = null!, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
        this._hb     = hb;
        this._offset = offset;
    }

    /// <summary>
    /// Allocates a new direct byte buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its capacity, its
    /// mark will be undefined, and each of its elements will be initialized to zero.
    /// Whether or not it has a backing array is unspecified.
    /// </para>
    /// </summary>
    /// <param name="capacity">The new buffer's capacity, in bytes</param>
    /// <returns>The new byte buffer</returns>
    /// <exception cref="ArgumentException">
    /// If the <tt>capacity</tt> is a negative integer.
    /// </exception>
    public static ByteBuffer AllocateDirect( int capacity )
    {
        return new DirectByteBuffer( capacity );
    }

    /// <summary>
    /// Allocates a new byte buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its capacity,
    /// its mark will be undefined, and each of its elements will be initialized
    /// to zero.  It will have a backing array, and its <see cref="ArrayOffset()"/>
    /// will be zero.
    /// </para>
    /// </summary>
    /// <param name="capacity">The new buffer's capacity, in bytes</param>
    /// <returns>The new byte buffer</returns>
    /// <exception cref="ArgumentException">
    /// If the <tt>capacity</tt> is a negative integer
    /// </exception>
    public static ByteBuffer Allocate( int capacity )
    {
        if ( capacity < 0 )
        {
            throw new System.ArgumentException();
        }

        return new HeapByteBuffer( capacity, capacity );
    }

    /// <summary>
    /// Wraps a byte array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given byte array;
    /// that is, modifications to the buffer will cause the array to be modified
    /// and vice versa.  The new buffer's capacity will be
    /// <tt>array.length</tt>, its position will be <tt>offset</tt>, its limit
    /// will be <tt>offset + length</tt>, and its mark will be undefined.  Its
    /// backing array will be the given array, and its array offset will be zero.
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
    /// <returns>The new byte buffer</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt> parameters
    /// do not hold.
    /// </exception>
    public static ByteBuffer Wrap( byte[] array, int offset, int length )
    {
        try
        {
            return new HeapByteBuffer( array, offset, length );
        }
        catch ( System.ArgumentException )
        {
            throw new System.IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Wraps a byte array into a buffer.
    /// 
    /// <para>
    /// The new buffer will be backed by the given byte array; that is,
    /// modifications to the buffer will cause the array to be modified
    /// and vice versa.  The new buffer's capacity and limit will be
    /// <tt>array.length</tt>, its position will be zero, and its mark will be
    /// undefined.  Its backing array will be the given array, and its array
    /// offset will be zero.
    /// </para>
    /// </summary>
    /// <param name="array">The array that will back this buffer</param>
    /// <returns>The new byte buffer.</returns>
    public static ByteBuffer Wrap( byte[] array )
    {
        return Wrap( array, 0, array.Length );
    }

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
    /// will be undefined. The new buffer will be direct if, and only if, this
    /// buffer is direct, and it will be read-only if, and only if, this buffer
    /// is read-only.
    /// </para>
    /// </summary>
    /// <returns>The new byte buffer.</returns>
    public abstract ByteBuffer Slice();

    /// <summary>
    /// Creates a new byte buffer that shares this buffer's content.
    /// <para>
    /// The content of the new buffer will be that of this buffer. Changes
    /// to this buffer's content will be visible in the new buffer, and vice
    /// versa; the two buffers' position, limit, and mark values will be
    /// independent.
    /// </para>
    /// <para>
    /// The new buffer's capacity, limit, position, and mark values will be
    /// identical to those of this buffer. The new buffer will be direct if,
    /// and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns>The new byte buffer.</returns>
    public abstract ByteBuffer Duplicate();

    /// <summary>
    /// Creates a new, read-only byte buffer that shares this buffer's
    /// content.
    /// <para>
    /// The content of the new buffer will be that of this buffer. Changes
    /// to this buffer's content will be visible in the new buffer; the new
    /// buffer itself, however, will be read-only and will not allow the shared
    /// content to be modified. The two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's capacity, limit, position, and mark values will be
    /// identical to those of this buffer.
    /// </para>
    /// <para>
    /// If this buffer is itself read-only then this method behaves in exactly
    /// the same way as the <see cref="Duplicate"/> method.
    /// </para>
    /// </summary>
    /// <returns>The new, read-only byte buffer.</returns>
    public abstract ByteBuffer AsReadOnlyBuffer();

    /// <summary>
    /// Relative <tt>get</tt> method. Reads the byte at this buffer's
    /// current position, and then increments the position.
    /// </summary>
    /// <returns>The byte at the buffer's current position.</returns>
    /// <exception cref="BufferUnderflowException">
    /// If the buffer's current position is not smaller than its limit.
    /// </exception>
    public abstract byte Get();

    /// <summary>
    /// Relative <tt>put</tt> method. Writes the given byte into this
    /// buffer at the current position, and then increments the position.
    /// </summary>
    /// <param name="b">The byte to be written.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="BufferOverflowException">
    /// If this buffer's current position is not smaller than its limit
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    /// If this buffer is read-only.
    /// </exception>
    protected abstract ByteBuffer Put( byte b );

    /// <summary>
    /// Absolute <tt>get</tt> method. Reads the byte at the given index.
    /// </summary>
    /// <param name="index">
    /// The index from which the byte will be read
    /// </param>
    /// <returns>The byte at the given index.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit.
    /// </exception>
    public abstract byte Get( int index );

    /// <summary>
    /// Absolute <tt>put</tt> method. Writes the given byte into this
    /// buffer at the given index.
    /// </summary>
    /// <param name="index">
    /// The index at which the byte will be written
    /// </param>
    /// <param name="b">The byte value to be written.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit.
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only.</exception>
    public abstract ByteBuffer Put( int index, byte b );

    // -- Bulk get operations --

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// 
    /// <para> This method transfers bytes from this buffer into the given
    /// destination array.  If there are fewer bytes remaining in the
    /// buffer than are required to satisfy the request, that is, if
    /// <tt>length</tt> <tt>&gt;</tt> <tt>remaining()</tt>, then no
    /// bytes are transferred and a <see cref="BufferUnderflowException"/> is
    /// thrown.
    /// 
    /// </para>
    /// <para> Otherwise, this method copies <tt>length</tt> bytes from this
    /// buffer into the given array, starting at the current position of this
    /// buffer and at the given offset in the array.  The position of this
    /// buffer is then incremented by <tt>length</tt>.
    /// 
    /// </para>
    /// <para> In other words, an invocation of this method of the form
    /// <tt>src.get(dst, off, len)</tt> has exactly the same effect as
    /// the loop
    /// 
    /// <pre>{@code
    ///     for (int i = off; i < off + len; i++)
    ///         dst[i] = src.get():
    /// }</pre>
    /// 
    /// except that it first checks that there are sufficient bytes in
    /// this buffer and it is potentially much more efficient.
    /// 
    /// </para>
    /// </summary>
    /// <param name="dst">
    ///         The array into which bytes are to be written
    /// </param>
    /// <param name="offset">
    ///         The offset within the array of the first byte to be
    ///         written; must be non-negative and no larger than
    ///         <tt>dst.length</tt>
    /// </param>
    /// <param name="length">
    ///         The maximum number of bytes to be written to the given
    ///         array; must be non-negative and no larger than
    ///         <tt>dst.length - offset</tt>
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferUnderflowException">
    ///          If there are fewer than <tt>length</tt> bytes
    ///          remaining in this buffer
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    ///          If the preconditions on the <tt>offset</tt> and <tt>length</tt>
    ///          parameters do not hold </exception>
    public virtual ByteBuffer Get( byte[] dst, int offset, int length )
    {
        checkBounds( offset, length, dst.Length );

        if ( length > remaining() )
        {
            throw new BufferUnderflowException();
        }

        int end = offset + length;

        for ( int i = offset; i < end; i++ )
        {
            dst[ i ] = get();
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// 
    /// <para> This method transfers bytes from this buffer into the given
    /// destination array.  An invocation of this method of the form
    /// <tt>src.get(a)</tt> behaves in exactly the same way as the invocation
    /// 
    /// <pre>
    ///     src.get(a, 0, a.length) </pre>
    /// 
    /// </para>
    /// </summary>
    /// <param name="dst">
    ///          The destination array
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferUnderflowException">
    ///          If there are fewer than <tt>length</tt> bytes
    ///          remaining in this buffer </exception>
    public virtual ByteBuffer Get( byte[] dst )
    {
        return Get( dst, 0, dst.Length );
    }


    // -- Bulk put operations --

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// 
    /// <para> This method transfers the bytes remaining in the given source
    /// buffer into this buffer.  If there are more bytes remaining in the
    /// source buffer than in this buffer, that is, if
    /// <tt>src.remaining()</tt> <tt>&gt;</tt> <tt>remaining()</tt>,
    /// then no bytes are transferred and a {@link
    /// BufferOverflowException} is thrown.
    /// 
    /// </para>
    /// <para> Otherwise, this method copies
    /// <i>n</i> = <tt>src.remaining()</tt> bytes from the given
    /// buffer into this buffer, starting at each buffer's current position.
    /// The positions of both buffers are then incremented by <i>n</i>.
    /// 
    /// </para>
    /// <para> In other words, an invocation of this method of the form
    /// <tt>dst.put(src)</tt> has exactly the same effect as the loop
    /// 
    /// <pre>
    ///     while (src.hasRemaining())
    ///         dst.put(src.get()); </pre>
    /// 
    /// except that it first checks that there is sufficient space in this
    /// buffer and it is potentially much more efficient.
    /// 
    /// </para>
    /// </summary>
    /// <param name="src">
    ///         The source buffer from which bytes are to be read;
    ///         must not be this buffer
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferOverflowException">
    ///          If there is insufficient space in this buffer
    ///          for the remaining bytes in the source buffer
    /// </exception>
    /// <exception cref="ArgumentException">
    ///          If the source buffer is this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public virtual ByteBuffer Put( ByteBuffer src )
    {
        if ( src == this )
        {
            throw new System.ArgumentException();
        }

        if ( isReadOnly() )
        {
            throw new ReadOnlyBufferException();
        }

        int n = src.remaining();

        if ( n > remaining() )
        {
            throw new BufferOverflowException();
        }

        for ( int i = 0; i < n; i++ )
        {
            Put( src.get() );
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// 
    /// <para> This method transfers bytes into this buffer from the given
    /// source array.  If there are more bytes to be copied from the array
    /// than remain in this buffer, that is, if
    /// <tt>length</tt> <tt>&gt;</tt> <tt>remaining()</tt>, then no
    /// bytes are transferred and a <see cref="BufferOverflowException"/> is
    /// thrown.
    /// 
    /// </para>
    /// <para> Otherwise, this method copies <tt>length</tt> bytes from the
    /// given array into this buffer, starting at the given offset in the array
    /// and at the current position of this buffer.  The position of this buffer
    /// is then incremented by <tt>length</tt>.
    /// 
    /// </para>
    /// <para> In other words, an invocation of this method of the form
    /// <tt>dst.put(src, off, len)</tt> has exactly the same effect as
    /// the loop
    /// 
    /// <pre>{@code
    ///     for (int i = off; i < off + len; i++)
    ///         dst.put(a[i]);
    /// }</pre>
    /// 
    /// except that it first checks that there is sufficient space in this
    /// buffer and it is potentially much more efficient.
    /// 
    /// </para>
    /// </summary>
    /// <param name="src">
    ///         The array from which bytes are to be read
    /// </param>
    /// <param name="offset">
    ///         The offset within the array of the first byte to be read;
    ///         must be non-negative and no larger than <tt>array.length</tt>
    /// </param>
    /// <param name="length">
    ///         The number of bytes to be read from the given array;
    ///         must be non-negative and no larger than
    ///         <tt>array.length - offset</tt>
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferOverflowException">
    ///          If there is insufficient space in this buffer
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    ///          If the preconditions on the <tt>offset</tt> and <tt>length</tt>
    ///          parameters do not hold
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public virtual ByteBuffer Put( byte[] src, int offset, int length )
    {
        checkBounds( offset, length, src.Length );

        if ( length > remaining() )
        {
            throw new BufferOverflowException();
        }

        int end = offset + length;

        for ( int i = offset; i < end; i++ )
        {
            this.Put( src[ i ] );
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// 
    /// <para> This method transfers the entire content of the given source
    /// byte array into this buffer.  An invocation of this method of the
    /// form <tt>dst.put(a)</tt> behaves in exactly the same way as the
    /// invocation
    /// 
    /// <pre>
    ///     dst.put(a, 0, a.length) </pre>
    /// 
    /// </para>
    /// </summary>
    /// <param name="src">
    ///          The source array
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferOverflowException">
    ///          If there is insufficient space in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public ByteBuffer Put( byte[] src )
    {
        return Put( src, 0, src.Length );
    }

    // -- Other stuff --

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible byte
    /// array.
    /// 
    /// <para> If this method returns <tt>true</tt> then the <see cref="array() array"/>
    /// and <see cref="arrayOffset() arrayOffset"/> methods may safely be invoked.
    /// </para>
    /// </summary>
    /// <returns>  <tt>true</tt> if, and only if, this buffer
    ///          is backed by an array and is not read-only </returns>
    public bool HasArray()
    {
        return ( _hb != null ) && !isReadOnly;
    }

    /// <summary>
    /// Returns the byte array that backs this
    /// buffer  <i>(optional operation)</i>.
    /// 
    /// <para> Modifications to this buffer's content will cause the returned
    /// array's content to be modified, and vice versa.
    /// 
    /// </para>
    /// <para> Invoke the <see cref="hasArray hasArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.  </para>
    /// </summary>
    /// <returns>  The array that backs this buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="UnsupportedOperationException">
    ///          If this buffer is not backed by an accessible array </exception>
    public byte[] Array()
    {
        if ( _hb == null )
        {
            throw new System.NotSupportedException();
        }

        if ( isReadOnly )
        {
            throw new ReadOnlyBufferException();
        }

        return _hb;
    }

    /// <summary>
    /// Returns the offset within this buffer's backing array of the first
    /// element of the buffer  <i>(optional operation)</i>.
    /// 
    /// <para> If this buffer is backed by an array then buffer position <i>p</i>
    /// corresponds to array index <i>p</i> + <tt>arrayOffset()</tt>.
    /// 
    /// </para>
    /// <para> Invoke the <see cref="hasArray hasArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.  </para>
    /// </summary>
    /// <returns>  The offset within this buffer's array
    ///          of the first element of the buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="UnsupportedOperationException">
    ///          If this buffer is not backed by an accessible array </exception>
    public int ArrayOffset()
    {
        if ( _hb == null )
        {
            throw new System.NotSupportedException();
        }

        if ( isReadOnly )
        {
            throw new ReadOnlyBufferException();
        }

        return _offset;
    }

    /// <summary>
    /// Compacts this buffer  <i>(optional operation)</i>.
    /// 
    /// <para> The bytes between the buffer's current position and its limit,
    /// if any, are copied to the beginning of the buffer.  That is, the
    /// byte at index <i>p</i> = <tt>position()</tt> is copied
    /// to index zero, the byte at index <i>p</i> + 1 is copied
    /// to index one, and so forth until the byte at index
    /// <tt>limit()</tt> - 1 is copied to index
    /// <i>n</i> = <tt>limit()</tt> - <tt>1</tt> - <i>p</i>.
    /// The buffer's position is then set to <i>n+1</i> and its limit is set to
    /// its capacity.  The mark, if defined, is discarded.
    /// 
    /// </para>
    /// <para> The buffer's position is set to the number of bytes copied,
    /// rather than to zero, so that an invocation of this method can be
    /// followed immediately by an invocation of another relative <i>put</i>
    /// method. </para>
    /// 
    /// 
    /// 
    /// <para> Invoke this method after writing data from a buffer in case the
    /// write was incomplete.  The following loop, for example, copies bytes
    /// from one channel to another via the buffer <tt>buf</tt>:
    /// 
    /// <blockquote><pre>{@code
    ///   buf.clear();          // Prepare buffer for use
    ///   while (in.read(buf) >= 0 || buf.position != 0) {
    ///       buf.flip();
    ///       out.write(buf);
    ///       buf.compact();    // In case of partial write
    ///   }
    /// }</pre></blockquote>
    /// 
    /// 
    /// 
    /// </para>
    /// </summary>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer Compact();

    /// <summary>
    /// Tells whether or not this byte buffer is direct.
    /// </summary>
    /// <returns>  <tt>true</tt> if, and only if, this buffer is direct </returns>
    public abstract bool Direct { get; }

    /// <summary>
    /// Returns a string summarizing the state of this buffer.
    /// </summary>
    /// <returns>  A summary string </returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        sb.Append( this.GetType().FullName );
        sb.Append( "[pos=" );
        sb.Append( position() );
        sb.Append( " lim=" );
        sb.Append( limit() );
        sb.Append( " cap=" );
        sb.Append( capacity() );
        sb.Append( "]" );

        return sb.ToString();
    }


    /// <summary>
    /// Returns the current hash code of this buffer.
    /// 
    /// <para> The hash code of a byte buffer depends only upon its remaining
    /// elements; that is, upon the elements from <tt>position()</tt> up to, and
    /// including, the element at <tt>limit()</tt> - <tt>1</tt>.
    /// 
    /// </para>
    /// <para> Because buffer hash codes are content-dependent, it is inadvisable
    /// to use buffers as keys in hash maps or similar data structures unless it
    /// is known that their contents will not change.  </para>
    /// </summary>
    /// <returns>  The current hash code of this buffer </returns>
    public override int GetHashCode()
    {
        int h = 1;
        int p = position();

        for ( int i = limit() - 1; i >= p; i-- )
        {


            h = ( 31 * h ) + ( int )get( i );
        }

        return h;
    }

    /// <summary>
    /// Tells whether or not this buffer is equal to another object.
    /// <para>
    /// Two byte buffers are equal if, and only if,
    /// </para>
    ///   <para> 1. They have the same element type.</para>
    ///   <para> 2. They have the same number of remaining elements.</para>
    ///   <para> 3. The two sequences of remaining elements, considered
    ///             independently of their starting positions, are pointwise equal.
    ///   </para>
    /// <para> A byte buffer is not equal to any other type of object.</para>
    /// </summary>
    /// <param name="ob"> The object to which this buffer is to be compared.</param>
    /// <returns>
    /// <tt>true</tt> if, and only if, this buffer is equal to the given object.
    /// </returns>
    public override bool Equals( object? ob )
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

        for ( int i = this.Limit - 1, j = that.Limit - 1; i >= this.Position; i--, j-- )
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
    /// <para> Two byte buffers are compared by comparing their sequences of
    /// remaining elements lexicographically, without regard to the starting
    /// position of each sequence within its corresponding buffer.
    /// Pairs of byte elements are compared as if by invoking
    /// <see cref="Byte.CompareTo(byte)"/>.
    /// </para>
    /// <para> A byte buffer is not comparable to any other type of object. </para>
    /// </summary>
    /// <returns>
    /// A negative integer, zero, or a positive integer as this buffer
    /// is less than, equal to, or greater than the given buffer
    /// </returns>
    public virtual int CompareTo( ByteBuffer? that )
    {
        ArgumentNullException.ThrowIfNull( that );
        
        int n = this.Position + Math.Min( this.Remaining(), that.Remaining() );

        for ( int i = this.Position, j = that.Position; i < n; i++, j++ )
        {
            int cmp = Compare( this.Get( i ), that.Get( j ) );

            if ( cmp != 0 )
            {
                return cmp;
            }
        }

        return this.Remaining() - that.Remaining();
    }

    private static int Compare( byte byte1, byte byte2 )
    {
        return byte1.CompareTo( byte2 );
    }

    // -- Other char stuff --

    // -- Other byte stuff: Access to binary data --

    internal bool bigEndian       = true;
    internal bool nativeByteOrder = ( Bits.ByteOrder == ByteOrder.BigEndian );

    /// <summary>
    /// Retrieves this buffer's byte order.
    /// <para>
    /// The byte order is used when reading or writing multibyte values, and
    /// when creating buffers that are views of this byte buffer. The order of
    /// a newly-created byte buffer is always <see cref="ByteOrder.BigEndian"/>
    /// </para>
    /// </summary>
    /// <returns>  This buffer's byte order </returns>
    public ByteOrder Order()
    {
        return bigEndian ? ByteOrder.BigEndian : ByteOrder.LittleEndian;
    }

    /// <summary>
    /// Modifies this buffer's byte order.
    /// </summary>
    /// <param name="bo">
    ///         The new byte order,
    ///         either <see cref="ByteOrder.BigEndian"/>
    ///         or <see cref="ByteOrder.LittleEndian"/>
    /// </param>
    /// <returns>  This buffer </returns>
    public ByteBuffer Order( ByteOrder bo )
    {
        bigEndian       = ( bo == ByteOrder.BigEndian );
        nativeByteOrder = ( bigEndian == ( Bits.ByteOrder == ByteOrder.BigEndian ) );

        return this;
    }

    /// <summary>
    /// Relative <i>get</i> method for reading a char value.
    /// 
    /// <para> Reads the next two bytes at this buffer's current position,
    /// composing them into a char value according to the current byte order,
    /// and then increments the position by two.  </para>
    /// </summary>
    /// <returns>  The char value at the buffer's current position
    /// </returns>
    /// <exception cref="BufferUnderflowException">
    ///          If there are fewer than two bytes
    ///          remaining in this buffer </exception>
    public abstract char Char { get; }

    /// <summary>
    /// Relative <i>put</i> method for writing a char
    /// value  <i>(optional operation)</i>.
    /// 
    /// <para> Writes two bytes containing the given char value, in the
    /// current byte order, into this buffer at the current position, and then
    /// increments the position by two.  </para>
    /// </summary>
    /// <param name="value">
    ///         The char value to be written
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferOverflowException">
    ///          If there are fewer than two bytes
    ///          remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer PutChar( char value );

    /// <summary>
    /// Absolute <i>get</i> method for reading a char value.
    /// 
    /// <para> Reads two bytes at the given index, composing them into a
    /// char value according to the current byte order.  </para>
    /// </summary>
    /// <param name="index">
    ///         The index from which the bytes will be read
    /// </param>
    /// <returns>  The char value at the given index
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>index</tt> is negative
    ///          or not smaller than the buffer's limit,
    ///          minus one </exception>
    public abstract char GetChar( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing a char
    /// value  <i>(optional operation)</i>.
    /// 
    /// <para> Writes two bytes containing the given char value, in the
    /// current byte order, into this buffer at the given index.  </para>
    /// </summary>
    /// <param name="index">
    ///         The index at which the bytes will be written
    /// </param>
    /// <param name="value">
    ///         The char value to be written
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>index</tt> is negative
    ///          or not smaller than the buffer's limit,
    ///          minus one
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer PutChar( int index, char value );

    /// <summary>
    /// Creates a view of this byte buffer as a char buffer.
    /// 
    /// <para> The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// 
    /// </para>
    /// <para> The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// two, and its mark will be undefined.  The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.  </para>
    /// </summary>
    /// <returns>  A new char buffer </returns>
    public abstract CharBuffer AsCharBuffer();


    /// <summary>
    /// Relative <i>get</i> method for reading a short value.
    /// 
    /// <para> Reads the next two bytes at this buffer's current position,
    /// composing them into a short value according to the current byte order,
    /// and then increments the position by two.  </para>
    /// </summary>
    /// <returns>  The short value at the buffer's current position
    /// </returns>
    /// <exception cref="BufferUnderflowException">
    ///          If there are fewer than two bytes
    ///          remaining in this buffer </exception>
    public abstract short Short { get; }

    /// <summary>
    /// Relative <i>put</i> method for writing a short
    /// value  <i>(optional operation)</i>.
    /// 
    /// <para> Writes two bytes containing the given short value, in the
    /// current byte order, into this buffer at the current position, and then
    /// increments the position by two.  </para>
    /// </summary>
    /// <param name="value">
    ///         The short value to be written
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferOverflowException">
    ///          If there are fewer than two bytes
    ///          remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer PutShort( short value );

    /// <summary>
    /// Absolute <i>get</i> method for reading a short value.
    /// 
    /// <para> Reads two bytes at the given index, composing them into a
    /// short value according to the current byte order.  </para>
    /// </summary>
    /// <param name="index">
    ///         The index from which the bytes will be read
    /// </param>
    /// <returns>  The short value at the given index
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>index</tt> is negative
    ///          or not smaller than the buffer's limit,
    ///          minus one </exception>
    public abstract short GetShort( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing a short
    /// value  <i>(optional operation)</i>.
    /// 
    /// <para> Writes two bytes containing the given short value, in the
    /// current byte order, into this buffer at the given index.  </para>
    /// </summary>
    /// <param name="index">
    ///         The index at which the bytes will be written
    /// </param>
    /// <param name="value">
    ///         The short value to be written
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>index</tt> is negative
    ///          or not smaller than the buffer's limit,
    ///          minus one
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer PutShort( int index, short value );

    /// <summary>
    /// Creates a view of this byte buffer as a short buffer.
    /// 
    /// <para> The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// 
    /// </para>
    /// <para> The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// two, and its mark will be undefined.  The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.  </para>
    /// </summary>
    /// <returns>  A new short buffer </returns>
    public abstract ShortBuffer AsShortBuffer();


    /// <summary>
    /// Relative <i>get</i> method for reading an int value.
    /// 
    /// <para> Reads the next four bytes at this buffer's current position,
    /// composing them into an int value according to the current byte order,
    /// and then increments the position by four.  </para>
    /// </summary>
    /// <returns>  The int value at the buffer's current position
    /// </returns>
    /// <exception cref="BufferUnderflowException">
    ///          If there are fewer than four bytes
    ///          remaining in this buffer </exception>
    public abstract int Int { get; }

    /// <summary>
    /// Relative <i>put</i> method for writing an int
    /// value  <i>(optional operation)</i>.
    /// 
    /// <para> Writes four bytes containing the given int value, in the
    /// current byte order, into this buffer at the current position, and then
    /// increments the position by four.  </para>
    /// </summary>
    /// <param name="value">
    ///         The int value to be written
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferOverflowException">
    ///          If there are fewer than four bytes
    ///          remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer PutInt( int value );

    /// <summary>
    /// Absolute <i>get</i> method for reading an int value.
    /// 
    /// <para> Reads four bytes at the given index, composing them into a
    /// int value according to the current byte order.  </para>
    /// </summary>
    /// <param name="index">
    ///         The index from which the bytes will be read
    /// </param>
    /// <returns>  The int value at the given index
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>index</tt> is negative
    ///          or not smaller than the buffer's limit,
    ///          minus three </exception>
    public abstract int GetInt( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing an int
    /// value  <i>(optional operation)</i>.
    /// 
    /// <para> Writes four bytes containing the given int value, in the
    /// current byte order, into this buffer at the given index.  </para>
    /// </summary>
    /// <param name="index">
    ///         The index at which the bytes will be written
    /// </param>
    /// <param name="value">
    ///         The int value to be written
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>index</tt> is negative
    ///          or not smaller than the buffer's limit,
    ///          minus three
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer PutInt( int index, int value );

    /// <summary>
    /// Creates a view of this byte buffer as an int buffer.
    /// 
    /// <para> The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// 
    /// </para>
    /// <para> The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// four, and its mark will be undefined.  The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.  </para>
    /// </summary>
    /// <returns>  A new int buffer </returns>
    public abstract IntBuffer AsIntBuffer();


    /// <summary>
    /// Relative <i>get</i> method for reading a long value.
    /// 
    /// <para> Reads the next eight bytes at this buffer's current position,
    /// composing them into a long value according to the current byte order,
    /// and then increments the position by eight.  </para>
    /// </summary>
    /// <returns>  The long value at the buffer's current position
    /// </returns>
    /// <exception cref="BufferUnderflowException">
    ///          If there are fewer than eight bytes
    ///          remaining in this buffer </exception>
    public abstract long Long { get; }

    /// <summary>
    /// Relative <i>put</i> method for writing a long
    /// value  <i>(optional operation)</i>.
    /// 
    /// <para> Writes eight bytes containing the given long value, in the
    /// current byte order, into this buffer at the current position, and then
    /// increments the position by eight.  </para>
    /// </summary>
    /// <param name="value">
    ///         The long value to be written
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="BufferOverflowException">
    ///          If there are fewer than eight bytes
    ///          remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer PutLong( long value );

    /// <summary>
    /// Absolute <i>get</i> method for reading a long value.
    /// 
    /// <para> Reads eight bytes at the given index, composing them into a
    /// long value according to the current byte order.  </para>
    /// </summary>
    /// <param name="index">
    ///         The index from which the bytes will be read
    /// </param>
    /// <returns>  The long value at the given index
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>index</tt> is negative
    ///          or not smaller than the buffer's limit,
    ///          minus seven </exception>
    public abstract long GetLong( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing a long
    /// value  <i>(optional operation)</i>.
    /// 
    /// <para> Writes eight bytes containing the given long value, in the
    /// current byte order, into this buffer at the given index.  </para>
    /// </summary>
    /// <param name="index">
    ///         The index at which the bytes will be written
    /// </param>
    /// <param name="value">
    ///         The long value to be written
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>index</tt> is negative
    ///          or not smaller than the buffer's limit,
    ///          minus seven
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is read-only </exception>
    public abstract ByteBuffer PutLong( int index, long value );

    /// <summary>
    /// Creates a view of this byte buffer as a long buffer.
    /// 
    /// <para> The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// 
    /// </para>
    /// <para> The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// eight, and its mark will be undefined.  The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.  </para>
    /// </summary>
    /// <returns>  A new long buffer </returns>
    public abstract LongBuffer AsLongBuffer();


    /// <summary>
    /// Relative <i>get</i> method for reading a float value.
    /// 
    /// <para> Reads the next four bytes at this buffer's current position,
    /// composing them into a float value according to the current byte order,
    /// and then increments the position by four.  </para>
    /// </summary>
    /// <returns>  The float value at the buffer's current position
    /// </returns>
    /// <exception cref="BufferUnderflowException">
    ///          If there are fewer than four bytes
    ///          remaining in this buffer </exception>
    public abstract float Float { get; }

    /// <summary>
    /// Relative <i>put</i> method for writing a float value  <i>(optional operation)</i>.
    /// <para>
    /// Writes four bytes containing the given float value, in the
    /// current byte order, into this buffer at the current position, and then
    /// increments the position by four.
    /// </para>
    /// </summary>
    /// <param name="value"> The float value to be written </param>
    /// <returns>  This buffer </returns>
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
    /// <param name="index"> The index from which the bytes will be read </param>
    /// <returns>  The float value at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus 3
    /// </exception>
    public abstract float GetFloat( int index );

    /// <summary>
    /// Absolute <i>put</i> method for writing a float value <i>(optional operation)</i>.
    /// <para>
    /// Writes four bytes containing the given float value, in the
    /// current byte order, into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index"> The index at which the bytes will be written </param>
    /// <param name="value"> The float value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit, minus 3
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
    /// <returns>  A new float buffer </returns>
    public abstract FloatBuffer AsFloatBuffer();


    /// <summary>
    /// Relative <i>get</i> method for reading a double value.
    /// 
    /// <para> Reads the next eight bytes at this buffer's current position,
    /// composing them into a double value according to the current byte order,
    /// and then increments the position by eight.  </para>
    /// </summary>
    /// <returns>  The double value at the buffer's current position </returns>
    /// <exception cref="BufferUnderflowException">
    /// If there are fewer than eight bytes remaining in this buffer
    /// </exception>
    public abstract double Double { get; }

    /// <summary>
    /// Relative <i>put</i> method for writing a double value <i>(optional operation)</i>.
    /// <para>
    /// Writes eight bytes containing the given double value, in the current
    /// byte order, into this buffer at the current position, and then increments
    /// the position by eight.
    /// </para>
    /// </summary>
    /// <param name="value"> The double value to be written </param>
    /// <returns>  This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If there are fewer than eight bytes remaining in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">If this buffer is read-only </exception>
    public abstract ByteBuffer PutDouble( double value );

    /// <summary>
    /// Absolute <tt>get</tt> method for reading a double value.
    /// <para>
    /// Reads eight bytes at the given index, composing them into a
    /// double value according to the current byte order.
    /// </para>
    /// </summary>
    /// <param name="index">
    /// The index from which the bytes will be read.
    /// </param>
    /// <returns>The double value at the given index.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit,
    /// minus seven.
    /// </exception>
    public abstract double GetDouble( int index );

    /// <summary>
    /// Absolute <tt>put</tt> method for writing a double value.
    /// <para>
    /// Writes eight bytes containing the given double value, in the
    /// current byte order, into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index">The index at which the bytes will be written.</param>
    /// <param name="value">The double value to be written.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit,
    /// minus seven
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    /// If this buffer is read-only.
    /// </exception>
    public abstract ByteBuffer PutDouble( int index, double value );

    /// <summary>
    /// Creates a view of this byte buffer as a double buffer.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position. Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of bytes remaining in this buffer divided by
    /// eight, and its mark will be undefined. The new buffer will be direct
    /// if, and only if, this buffer is direct, and it will be read-only if,
    /// and only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns>A new double buffer.</returns>
    public abstract DoubleBuffer AsDoubleBuffer();
}
