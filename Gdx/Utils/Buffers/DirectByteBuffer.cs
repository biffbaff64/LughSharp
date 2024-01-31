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


namespace LibGDXSharp.Gdx.Utils.Buffers;

[PublicAPI]
public class DirectByteBuffer : ByteBuffer
{
    public DirectByteBuffer( int capacity )
        : base( -1, 0, capacity, capacity )
    {
    }

    /// <summary>
    ///     Tells whether or not this buffer is <i>direct</i>.
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct </returns>
    public override bool IsDirect() => true;

    /// <summary>
    ///     Creates a new byte buffer whose content is a shared subsequence of
    ///     this buffer's content.
    ///     <p>
    ///         The content of the new buffer will start at this buffer's current
    ///         position.  Changes to this buffer's content will be visible in the new
    ///         buffer, and vice versa; the two buffers' position, limit, and mark
    ///         values will be independent.
    ///     </p>
    ///     <p>
    ///         The new buffer's position will be zero, its capacity and its limit
    ///         will be the number of bytes remaining in this buffer, and its mark
    ///         will be undefined.  The new buffer will be direct if, and only if, this
    ///         buffer is direct, and it will be read-only if, and only if, this buffer
    ///         is read-only.
    ///     </p>
    /// </summary>
    /// <returns> The new byte buffer </returns>
    public override ByteBuffer Slice() => null;

    /// <summary>
    ///     Creates a new byte buffer that shares this buffer's content.
    ///     <p>
    ///         The content of the new buffer will be that of this buffer.  Changes
    ///         to this buffer's content will be visible in the new buffer, and vice
    ///         versa; the two buffers' position, limit, and mark values will be
    ///         independent.
    ///     </p>
    ///     <p>
    ///         The new buffer's capacity, limit, position, and mark values will be
    ///         identical to those of this buffer.  The new buffer will be direct if,
    ///         and only if, this buffer is direct, and it will be read-only if, and
    ///         only if, this buffer is read-only.
    ///     </p>
    /// </summary>
    /// <returns> The new byte buffer. </returns>
    public override ByteBuffer Duplicate() => null;

    /**
     * Creates a new, read-only byte buffer that shares this buffer's
     * content.
     * <p>
     *     The content of the new buffer will be that of this buffer.  Changes
     *     to this buffer's content will be visible in the new buffer; the new
     *     buffer itself, however, will be read-only and will not allow the shared
     *     content to be modified.  The two buffers' position, limit, and mark
     *     values will be independent.
     *     <p>
     *         The new buffer's capacity, limit, position, and mark values will be
     *         identical to those of this buffer.
     *         <p>
     *             If this buffer is itself read-only then this method behaves in
     *             exactly the same way as the {@link #duplicate duplicate} method.
     *         </p>
     *         @return  The new, read-only byte buffer
     */
    public override ByteBuffer AsReadOnlyBuffer() => null;

    /**
     * Relative
     * <i>get</i>
     * method.  Reads the byte at this buffer's
     * current position, and then increments the position.
     * 
     * @return  The byte at the buffer's current position
     * 
     * @throws  BufferUnderflowException
     * If the buffer's current position is not smaller than its limit
     */
    public override byte Get() => 0;

    /**
     * Relative
     * <i>put</i>
     * method
     * <i>(optional operation)</i>
     * .
     * <p>
     *     Writes the given byte into this buffer at the current
     *     position, and then increments the position.
     * </p>
     * @param  b
     * The byte to be written
     * 
     * @return  This buffer
     * 
     * @throws  BufferOverflowException
     * If this buffer's current position is not smaller than its limit
     * 
     * @throws  GdxRuntimeException( "Buffer is Read Only!" )
     * If this buffer is read-only
     */
    public override ByteBuffer Put( byte b ) => null;

    /**
     * Absolute
     * <i>get</i>
     * method.  Reads the byte at the given
     * index.
     * 
     * @param  index
     * The index from which the byte will be read
     * 
     * @return  The byte at the given index
     * 
     * @throws  IndexOutOfBoundsException
     * If
     * <tt>index</tt>
     * is negative
     * or not smaller than the buffer's limit
     */
    public override byte Get( int index ) => 0;

    /**
     * Absolute
     * <i>put</i>
     * method
     * <i>(optional operation)</i>
     * .
     * <p>
     *     Writes the given byte into this buffer at the given
     *     index.
     * </p>
     * @param  index
     * The index at which the byte will be written
     * 
     * @param  b
     * The byte value to be written
     * 
     * @return  This buffer
     * 
     * @throws  IndexOutOfBoundsException
     * If
     * <tt>index</tt>
     * is negative
     * or not smaller than the buffer's limit
     * 
     * @throws  GdxRuntimeException( "Buffer is Read Only!" )
     * If this buffer is read-only
     */
    public override ByteBuffer Put( int index, byte b ) => null;

    /**
     * * Compacts this buffer
     * <i>(optional operation)</i>
     * .
     * *
     * *
     * <p>
     *     The bytes between the buffer's current position and its limit,
     *     * if any, are copied to the beginning of the buffer.  That is, the
     *     * byte at index <i>p</i> = <tt>position()</tt> is copied
     *     * to index zero, the byte at index <i>p</i> + 1 is copied
     *     * to index one, and so forth until the byte at index
     *     * <tt>limit()</tt> - 1 is copied to index
     *     * <i>n</i> = <tt>limit()</tt> - <tt>1</tt> - <i>p</i>.
     *     * The buffer's position is then set to <i>n+1</i> and its limit is set to
     *     * its capacity.  The mark, if defined, is discarded.
     *     *
     *     *
     *     <p>
     *         The buffer's position is set to the number of bytes copied,
     *         * rather than to zero, so that an invocation of this method can be
     *         * followed immediately by an invocation of another relative <i>put</i>
     *         * method.
     *     </p>
     *     *
     *     *
     *     *
     *     <p>
     *         Invoke this method after writing data from a buffer in case the
     *         * write was incomplete.  The following loop, for example, copies bytes
     *         * from one channel to another via the buffer <tt>buf</tt>:
     *         *
     *         *
     *         <blockquote>
     *             <pre>
     *                 {@code
     *                 *   buf.clear();          // Prepare buffer for use
     *                 *   while (in.read(buf) >= 0 || buf.position != 0) {
     *                 *       buf.flip();
     *                 *       out.write(buf);
     *                 *       buf.compact();    // In case of partial write
     *                 *   }
     *                 * }
     *             </pre>
     *         </blockquote>
     *         *
     *         *
     *         * @return  This buffer
     *         *
     *         * @throws  GdxRuntimeException( "Buffer is Read Only!" )
     *         *          If this buffer is read-only
     */
    public override ByteBuffer Compact() => null;

    public override char GetChar() => '\0';

    public override char GetChar( int index ) => '\0';

    public override ByteBuffer PutChar( char value ) => null;

    public override ByteBuffer PutChar( int index, char value ) => null;

    public override CharBuffer AsCharBuffer() => null;

    public override short GetShort() => 0;

    public override short GetShort( int index ) => 0;

    public override ByteBuffer PutShort( short value ) => null;

    public override ByteBuffer PutShort( int index, short value ) => null;

    public override ShortBuffer AsShortBuffer() => null;

    public override int GetInt() => 0;

    public override int GetInt( int index ) => 0;

    public override ByteBuffer PutInt( int value ) => null;

    public override ByteBuffer PutInt( int index, int value ) => null;

    public override IntBuffer AsIntBuffer() => null;

    public override long GetLong() => 0;

    public override long GetLong( int index ) => 0;

    public override ByteBuffer PutLong( long value ) => null;

    public override ByteBuffer PutLong( int index, long value ) => null;

    public override LongBuffer AsLongBuffer() => null;

    public override float GetFloat() => 0;

    public override float GetFloat( int index ) => 0;

    public override ByteBuffer PutFloat( float value ) => null;

    public override ByteBuffer PutFloat( int index, float value ) => null;

    public override FloatBuffer AsFloatBuffer() => null;

    /**
     * Relative
     * <i>get</i>
     * method for reading a double value.
     * <p>
     *     Reads the next eight bytes at this buffer's current position,
     *     composing them into a double value according to the current byte order,
     *     and then increments the position by eight.
     * </p>
     * @return  The double value at the buffer's current position
     * 
     * @throws  BufferUnderflowException
     * If there are fewer than eight bytes
     * remaining in this buffer
     */
    public override double GetDouble() => 0;

    /**
     * Absolute
     * <i>get</i>
     * method for reading a double value.
     * <p>
     *     Reads eight bytes at the given index, composing them into a
     *     double value according to the current byte order.
     * </p>
     * @param  index
     * The index from which the bytes will be read
     * 
     * @return  The double value at the given index
     * 
     * @throws  IndexOutOfBoundsException
     * If
     * <tt>index</tt>
     * is negative
     * or not smaller than the buffer's limit,
     * minus seven
     */
    public override double GetDouble( int index ) => 0;

    /**
     * Relative
     * <i>put</i>
     * method for writing a double
     * value
     * <i>(optional operation)</i>
     * .
     * <p>
     *     Writes eight bytes containing the given double value, in the
     *     current byte order, into this buffer at the current position, and then
     *     increments the position by eight.
     * </p>
     * @param  value
     * The double value to be written
     * 
     * @return  This buffer
     * 
     * @throws  BufferOverflowException
     * If there are fewer than eight bytes
     * remaining in this buffer
     * 
     * @throws  GdxRuntimeException( "Buffer is Read Only!" )
     * If this buffer is read-only
     */
    public override ByteBuffer PutDouble( double value ) => null;

    /**
     * Absolute
     * <i>put</i>
     * method for writing a double
     * value
     * <i>(optional operation)</i>
     * .
     * <p>
     *     Writes eight bytes containing the given double value, in the
     *     current byte order, into this buffer at the given index.
     * </p>
     * @param  index
     * The index at which the bytes will be written
     * 
     * @param  value
     * The double value to be written
     * 
     * @return  This buffer
     * 
     * @throws  IndexOutOfBoundsException
     * If
     * <tt>index</tt>
     * is negative
     * or not smaller than the buffer's limit,
     * minus seven
     * 
     * @throws  GdxRuntimeException( "Buffer is Read Only!" )
     * If this buffer is read-only
     */
    public override ByteBuffer PutDouble( int index, double value ) => null;

    /**
     * Creates a view of this byte buffer as a double buffer.
     * <p>
     *     The content of the new buffer will start at this buffer's current
     *     position.  Changes to this buffer's content will be visible in the new
     *     buffer, and vice versa; the two buffers' position, limit, and mark
     *     values will be independent.
     * </p>
     * <p>
     *     The new buffer's position will be zero, its capacity and its limit
     *     will be the number of bytes remaining in this buffer divided by
     *     eight, and its mark will be undefined.  The new buffer will be direct
     *     if, and only if, this buffer is direct, and it will be read-only if, and
     *     only if, this buffer is read-only.
     * </p>
     * @return  A new double buffer
     */
    public override DoubleBuffer AsDoubleBuffer() => null;
}
