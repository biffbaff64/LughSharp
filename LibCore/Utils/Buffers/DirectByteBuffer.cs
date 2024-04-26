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


namespace LughSharp.LibCore.Utils.Buffers;

[PublicAPI]
public class DirectByteBuffer : MappedByteBuffer, IDirectBuffer
{
    public DirectByteBuffer( int capacity )
        : base( -1, 0, capacity, capacity )
    {
    }

    public object? AttachedObject { get; set; }

    /// <inheritdoc />
    public override long Address => 0;

    /// <summary>
    ///     Tells whether or not this buffer is <i>direct</i>.
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct </returns>
    public override bool IsDirect()
    {
        return true;
    }

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
    public override ByteBuffer Slice()
    {
        return null!;
    }

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
    public override ByteBuffer Duplicate()
    {
        return null!;
    }

    /// <summary>
    ///     Creates a new, read-only byte buffer that shares this buffer's
    ///     content.
    ///     <para>
    ///         The content of the new buffer will be that of this buffer.  Changes
    ///         to this buffer's content will be visible in the new buffer; the new
    ///         buffer itself, however, will be read-only and will not allow the shared
    ///         content to be modified.  The two buffers' position, limit, and mark
    ///         values will be independent.
    ///         <para>
    ///             The new buffer's capacity, limit, position, and mark values will be
    ///             identical to those of this buffer.
    ///             <para>
    ///                 If this buffer is itself read-only then this method behaves in
    ///                 exactly the same way as the {@link #duplicate duplicate} method.
    ///             </para>
    ///         </para>
    ///     </para>
    /// </summary>
    public override ByteBuffer AsReadOnlyBuffer()
    {
        return null!;
    }

    /// <summary>
    ///     Relative <i>get</i> method. Reads the byte at this buffer's current
    ///     position, and then increments the position.
    /// </summary>
    /// <returns> The byte at the buffer's current position </returns>
    public override byte Get()
    {
        return 0;
    }

    /// <summary>
    ///     Relative <i>put</i> method <i>(optional operation)</i>.
    ///     <para>
    ///         Writes the given byte into this buffer at the current
    ///         position, and then increments the position.
    ///     </para>
    ///     <param name="b"> The byte to be written </param>
    /// </summary>
    /// <returns> This buffer </returns>
    public override ByteBuffer Put( byte b )
    {
        return this;
    }

    /// <summary>
    ///     Absolute <i>get</i> method. Reads the byte at the given index.
    /// </summary>
    /// <param name="index"> The index from which the byte will be read </param>
    /// <returns> The byte at the given index </returns>
    public override byte Get( int index )
    {
        return 0;
    }

    /// <summary>
    ///     Absolute <i>put</i> method <i>(optional operation)</i>.
    ///     <para>
    ///         Writes the given byte into this buffer at the given index.
    ///     </para>
    /// </summary>
    /// <param name="index">The index at which the byte will be written</param>
    /// <param name="b"> The byte value to be written </param>
    /// <returns> This buffer </returns>
    public override ByteBuffer Put( int index, byte b )
    {
        return this;
    }

    /// <summary>
    ///     Compacts this buffer <i>(optional operation)</i>.
    ///     <para>
    ///         The bytes between the buffer's current position and its limit, if any,
    ///         are copied to the beginning of the buffer. That is, the byte at index
    ///         <i>p</i> = <tt>position()</tt> is copied to index zero, the byte at
    ///         index <i>p</i> + 1 is copied to index one, and so forth until the byte
    ///         at index <tt>limit()</tt> - 1 is copied to index
    ///         <i>n</i> = <tt>limit()</tt> - <tt>1</tt> - <i>p</i>.
    ///         The buffer's position is then set to <i>n+1</i> and its limit is set to
    ///         its capacity.  The mark, if defined, is discarded.
    ///         <para>
    ///             The buffer's position is set to the number of bytes copied,
    ///             * rather than to zero, so that an invocation of this method can be
    ///             * followed immediately by an invocation of another relative <i>put</i>
    ///             * method.
    ///         </para>
    ///         <para>
    ///             Invoke this method after writing data from a buffer in case the
    ///             write was incomplete.  The following loop, for example, copies bytes
    ///             from one channel to another via the buffer <tt>buf</tt>:
    ///             <code>
    ///             buf.clear();          // Prepare buffer for use
    ///             while (in.read(buf) >= 0 || buf.position != 0)
    ///             {
    ///                 buf.flip();
    ///                 out.write(buf);
    ///                 buf.compact();    // In case of partial write
    ///             }
    ///         </code>
    ///         </para>
    ///     </para>
    /// </summary>
    /// <returns> This buffer </returns>
    public override ByteBuffer Compact()
    {
        return null!;
    }

    public override char GetChar()
    {
        return '\0';
    }

    public override char GetChar( int index )
    {
        return '\0';
    }

    public override ByteBuffer PutChar( char value )
    {
        return null!;
    }

    public override ByteBuffer PutChar( int index, char value )
    {
        return null!;
    }

    public override CharBuffer AsCharBuffer()
    {
        return null!;
    }

    public override short GetShort()
    {
        return 0;
    }

    public override short GetShort( int index )
    {
        return 0;
    }

    public override ByteBuffer PutShort( short value )
    {
        return null!;
    }

    public override ByteBuffer PutShort( int index, short value )
    {
        return null!;
    }

    public override ShortBuffer AsShortBuffer()
    {
        return null!;
    }

    public override int GetInt()
    {
        return 0;
    }

    public override int GetInt( int index )
    {
        return 0;
    }

    public override ByteBuffer PutInt( int value )
    {
        return null!;
    }

    public override ByteBuffer PutInt( int index, int value )
    {
        return null!;
    }

    public override IntBuffer AsIntBuffer()
    {
        return null!;
    }

    public override long GetLong()
    {
        return 0;
    }

    public override long GetLong( int index )
    {
        return 0;
    }

    public override ByteBuffer PutLong( long value )
    {
        return null!;
    }

    public override ByteBuffer PutLong( int index, long value )
    {
        return null!;
    }

    public override LongBuffer AsLongBuffer()
    {
        return null!;
    }

    public override float GetFloat()
    {
        return 0;
    }

    public override float GetFloat( int index )
    {
        return 0;
    }

    public override ByteBuffer PutFloat( float value )
    {
        return null!;
    }

    public override ByteBuffer PutFloat( int index, float value )
    {
        return null!;
    }

    public override FloatBuffer AsFloatBuffer()
    {
        return null!;
    }

    /// <summary>
    ///     Relative <i>get</i> method for reading a double value.
    ///     <para>
    ///         Reads the next eight bytes at this buffer's current position,
    ///         composing them into a double value according to the current byte order,
    ///         and then increments the position by eight.
    ///     </para>
    /// </summary>
    /// <returns> The double value at the buffer's current position </returns>
    public override double GetDouble()
    {
        return 0;
    }

    /// <summary>
    ///     Absolute <i>get</i> method for reading a double value.
    ///     <para>
    ///         Reads eight bytes at the given index, composing them into a
    ///         double value according to the current byte order.
    ///     </para>
    /// </summary>
    /// <param name="index"> The index from which the bytes will be read </param>
    /// <returns> The double value at the given index </returns>
    public override double GetDouble( int index )
    {
        return 0;
    }

    /// <summary>
    ///     Relative <i>put</i> method for writing a double value <i>(optional operation)</i>.
    ///     <para>
    ///         Writes eight bytes containing the given double value, in the
    ///         current byte order, into this buffer at the current position, and then
    ///         increments the position by eight.
    ///     </para>
    /// </summary>
    /// <param name="value"> The double value to be written </param>
    /// <returns> This buffer </returns>
    public override ByteBuffer PutDouble( double value )
    {
        return this;
    }

    /// <summary>
    ///     Absolute <i>put</i> method for writing a double value
    ///     <i>(optional operation)</i>
    ///     <para>
    ///         Writes eight bytes containing the given double value, in the
    ///         current byte order, into this buffer at the given index.
    ///     </para>
    /// </summary>
    /// <param name="index"> The index at which the bytes will be written </param>
    /// <param name="value"> The double value to be written </param>
    /// <returns> This buffer </returns>
    public override ByteBuffer PutDouble( int index, double value )
    {
        return this;
    }

    /// <summary>
    ///     Creates a view of this byte buffer as a double buffer.
    ///     <para>
    ///         The content of the new buffer will start at this buffer's current
    ///         position.  Changes to this buffer's content will be visible in the new
    ///         buffer, and vice versa; the two buffers' position, limit, and mark
    ///         values will be independent.
    ///     </para>
    ///     <para>
    ///         The new buffer's position will be zero, its capacity and its limit
    ///         will be the number of bytes remaining in this buffer divided by
    ///         eight, and its mark will be undefined.  The new buffer will be direct
    ///         if, and only if, this buffer is direct, and it will be read-only if, and
    ///         only if, this buffer is read-only.
    ///     </para>
    /// </summary>
    /// <returns> A new double buffer </returns>
    public override DoubleBuffer AsDoubleBuffer()
    {
        return null!;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    internal class Deallocator
    {
        private long _address;
        private int  _capacity;
        private long _size;

        private Deallocator( long address, long size, int capacity )
        {
            Debug.Assert( address != 0 );

            _address  = address;
            _size     = size;
            _capacity = capacity;
        }

        public void Run()
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if ( _address == 0 )
            {
                return;
            }

            //            unsafe.freeMemory( address );
            _address = 0;

//            Bits.unreserveMemory( size, capacity );
        }
    }
}