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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Utils.Buffers;

[PublicAPI]
public class HeapFloatBuffer : FloatBuffer
{
    public HeapFloatBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new float[ cap ] )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    public HeapFloatBuffer( float[] buf, int off, int len )
        : base( -1, off, off + len, buf.Length, buf )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    protected HeapFloatBuffer( float[] buf, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap, buf, off )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
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
    public override FloatBuffer Slice()
    {
        return new HeapFloatBuffer( Hb!, -1, 0, Remaining(), Remaining(), Position + Offset );
    }

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
    public override FloatBuffer Duplicate()
    {
        return new HeapFloatBuffer( Hb!, Mark, Position, Limit, Capacity, Offset );
    }

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
    public override FloatBuffer asReadOnlyBuffer()
    {
        return new HeapFloatBufferR( Hb!, Mark, Position, Limit, Capacity, Offset );
    }

    /// <summary>
    /// Relative <tt>get</tt> method. Reads the float at this buffers
    /// current position, and then increments the position.
    /// </summary>
    /// <returns> The float at the buffer's current position </returns>
    /// <exception cref="GdxRuntimeException">
    /// If the buffer's current position is not smaller than its limit
    /// </exception>
    public override float Get()
    {
        return 0;
    }

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
    public override FloatBuffer Put( float f )
    {
        return null!;
    }

    /// <summary>
    /// Absolute <tt>get</tt> method. Reads the float at the given index.
    /// </summary>
    /// <param name="index"> The index from which the float will be read </param>
    /// <returns> The float at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    public override float Get( int index )
    {
        return 0;
    }

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
    public override FloatBuffer Put( int index, float f )
    {
        return null!;
    }

    /// <summary>
    /// Compacts this buffer  <i>(optional operation)</i>.
    /// <para>
    /// The floats between the buffer's current position and its limit,
    /// if any, are copied to the beginning of the buffer. That is, the
    /// float at index <tt>p = Position</tt> is copied to index zero, the
    /// float at index <tt>p + 1|</tt> is copied to index one, and so forth
    /// until the float at index <tt>Limit - 1</tt> is copied to index
    /// <tt>n = Limit - 1 - p</tt>.
    /// The buffer's position is then set to <i>n+1</i> and its limit is set to
    /// its capacity.  The mark, if defined, is discarded.
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
    public override FloatBuffer Compact()
    {
        return null!;
    }

    /// <summary>
    /// Retrieves this buffer's byte order.
    /// <para>
    /// The byte order of a float buffer created by allocation or by wrapping
    /// an existing <tt>float</tt> array is the <see cref="ByteOrder.NativeOrder"/>"
    /// of the underlying hardware.
    /// </para>
    /// </summary>
    /// <returns> This buffer's byte order </returns>
    public override ByteOrder Order()
    {
        return null!;
    }
}
