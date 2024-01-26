// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Utils.Buffers;

[PublicAPI]
public class HeapFloatBuffer : FloatBuffer
{
    public HeapFloatBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new float[ cap ] )
    {
    }

    public HeapFloatBuffer( float[] buf, int off, int len )
        : base( -1, off, off + len, buf.Length, buf )
    {
    }

    protected HeapFloatBuffer( float[] buf, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap, buf, off )
    {
    }

    /// <summary>
    ///     Returns the array that backs this buffer <i>(optional operation)</i>.
    ///     <para>
    ///         This method is intended to allow array-backed buffers to be passed to
    ///         native code more efficiently. Concrete subclasses provide more strongly
    ///         typed return values for this method.
    ///     </para>
    ///     <para>
    ///         Modifications to this buffer's content will cause the returned array's
    ///         content to be modified, and vice versa.
    ///     </para>
    ///     <para>
    ///         Invoke the <see cref="Buffer.HasArray" /> method before invoking this method in
    ///         order to ensure that this buffer has an accessible backing array.
    ///     </para>
    /// </summary>
    /// <returns>  The array that backs this buffer </returns>
    public override object BackingArray() => null;

    /// <summary>
    ///     Tells whether or not this buffer is <i>direct</i>.
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct </returns>
    public override bool IsDirect() => false;

    /// <summary>
    ///     Creates a new float buffer whose content is a shared subsequence of
    ///     this buffer's content.
    ///     <para>
    ///         The content of the new buffer will start at this buffer's current
    ///         position. Changes to this buffer's content will be visible in the new
    ///         buffer, and vice versa; the two buffers' position, limit, and mark
    ///         values will be independent.
    ///     </para>
    ///     <para>
    ///         The new buffer's position will be zero, its capacity and its limit
    ///         will be the number of floats remaining in this buffer, and its mark
    ///         will be undefined. The new buffer will be direct if, and only if, this
    ///         buffer is direct, and it will be read-only if, and only if, this buffer
    ///         is read-only.
    ///     </para>
    /// </summary>
    /// <returns> The new float buffer </returns>
    public override FloatBuffer Slice() => null;

    /// <summary>
    ///     Creates a new float buffer that shares this buffer's content.
    ///     <para>
    ///         The content of the new buffer will be that of this buffer.  Changes
    ///         to this buffer's content will be visible in the new buffer, and vice
    ///         versa; the two buffers' position, limit, and mark values will be
    ///         independent.
    ///     </para>
    ///     <para>
    ///         The new buffer's capacity, limit, position, and mark values will be
    ///         identical to those of this buffer.  The new buffer will be direct if,
    ///         and only if, this buffer is direct, and it will be read-only if, and
    ///         only if, this buffer is read-only.
    ///     </para>
    /// </summary>
    /// <returns> The new float buffer </returns>
    public override FloatBuffer Duplicate() => null;

    /// <summary>
    ///     Creates a new, read-only float buffer that shares this buffer's
    ///     content.
    ///     <para>
    ///         The content of the new buffer will be that of this buffer.  Changes
    ///         to this buffer's content will be visible in the new buffer; the new
    ///         buffer itself, however, will be read-only and will not allow the shared
    ///         content to be modified.  The two buffers' position, limit, and mark
    ///         values will be independent.
    ///     </para>
    ///     <para>
    ///         The new buffer's capacity, limit, position, and mark values will be
    ///         identical to those of this buffer.
    ///     </para>
    ///     <para>
    ///         If this buffer is itself read-only then this method behaves in
    ///         exactly the same way as the {@link #duplicate duplicate} method.
    ///     </para>
    /// </summary>
    /// <returns> The new, read-only float buffer </returns>
    public override FloatBuffer asReadOnlyBuffer() => null;

    /// <summary>
    ///     Relative <tt>get</tt> method. Reads the float at this buffers
    ///     current position, and then increments the position.
    /// </summary>
    /// <returns> The float at the buffer's current position </returns>
    /// <exception cref="GdxRuntimeException">
    ///     If the buffer's current position is not smaller than its limit
    /// </exception>
    public override float Get() => 0;

    /// <summary>
    ///     Relative <tt>put</tt> method; <tt>(optional operation)</tt>.
    ///     <para>
    ///         Writes the given float into this buffer at the current
    ///         position, and then increments the position.
    ///     </para>
    /// </summary>
    /// <param name="f"> The float to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">
    ///     If this buffer's current position is not smaller than its limit
    /// </exception>
    /// <exception cref="GdxRuntimeException">
    ///     If this buffer is read-only
    /// </exception>
    public override FloatBuffer Put( float f ) => null;

    /// <summary>
    ///     Absolute <tt>get</tt> method. Reads the float at the given index.
    /// </summary>
    /// <param name="index"> The index from which the float will be read </param>
    /// <returns> The float at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///     If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    public override float Get( int index ) => 0;

    /// <summary>
    ///     Absolute <tt>put</tt> method  <tt>(optional operation)</tt>.
    ///     <para>
    ///         Writes the given float into this buffer at the given index.
    ///     </para>
    /// </summary>
    /// <param name="index"> The index at which the float will be written </param>
    /// <param name="f"> The float value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///     If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    /// <exception cref="GdxRuntimeException"> If this buffer is read-only </exception>
    public override FloatBuffer Put( int index, float f ) => null;

    /// <summary>
    ///     Compacts this buffer  <i>(optional operation)</i>.
    ///     <para>
    ///         The floats between the buffer's current position and its limit,
    ///         if any, are copied to the beginning of the buffer. That is, the
    ///         float at index <tt>p = Position</tt> is copied to index zero, the
    ///         float at index <tt>p + 1|</tt> is copied to index one, and so forth
    ///         until the float at index <tt>Limit - 1</tt> is copied to index
    ///         <tt>n = Limit - 1 - p</tt>.
    ///         The buffer's position is then set to <i>n+1</i> and its limit is set to
    ///         its capacity.  The mark, if defined, is discarded.
    ///     </para>
    ///     <para>
    ///         The buffer's position is set to the number of floats copied, rather than
    ///         to zero, so that an invocation of this method can be followed immediately
    ///         by an invocation of another relative <tt>put</tt>
    ///         method.
    ///     </para>
    /// </summary>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">If this buffer is read-only</exception>
    public override FloatBuffer Compact() => null;

    /// <summary>
    ///     Retrieves this buffer's byte order.
    ///     <para>
    ///         The byte order of a float buffer created by allocation or by wrapping
    ///         an existing <tt>float</tt> array is the <see cref="ByteOrder.NativeOrder" />"
    ///         of the underlying hardware.
    ///     </para>
    /// </summary>
    /// <returns> This buffer's byte order </returns>
    public override ByteOrder Order() => null;
}
