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

namespace LibGDXSharp.Files.Buffers;

public class HeapShortBuffer : ShortBuffer
{
    public HeapShortBuffer( int capacity, int limit )
    {
    }

    public HeapShortBuffer( short[] capacity, int offset, int length )
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
    ///     Creates a new short buffer whose content is a shared subsequence of
    ///     this buffer's content.
    ///     <para>
    ///         The content of the new buffer will start at this buffer's current
    ///         position.  Changes to this buffer's content will be visible in the new
    ///         buffer, and vice versa; the two buffers' position, limit, and mark
    ///         values will be independent.
    ///     </para>
    ///     <para>
    ///         The new buffer's position will be zero, its capacity and its limit
    ///         will be the number of shorts remaining in this buffer, and its mark
    ///         will be undefined.  The new buffer will be direct if, and only if, this
    ///         buffer is direct, and it will be read-only if, and only if, this buffer
    ///         is read-only.
    ///     </para>
    /// </summary>
    /// @return  The new short buffer
    public override ShortBuffer Slice() => null;

    /// <summary>
    ///     Creates a new short buffer that shares this buffer's content.
    /// </summary>
    /// <remarks>
    ///     The content of the new buffer will be that of this buffer.
    ///     Changes to this buffer's content will be visible in the new buffer,
    ///     and vice versa; the two buffers' position, limit, and mark values
    ///     will be independent.
    /// </remarks>
    /// <returns>The new short buffer.</returns>
    public override ShortBuffer Duplicate() => null;

    /// <summary>
    ///     Creates a new, read-only short buffer that shares this buffer's content.
    ///     The content of the new buffer will be that of this buffer. Changes to this
    ///     buffer's content will be visible in the new buffer; the new buffer itself,
    ///     however, will be read-only and will not allow the shared content to be
    ///     modified. The two buffers' position, limit, and mark values will be independent.
    /// </summary>
    /// <returns>The new, read-only short buffer.</returns>
    public override ShortBuffer AsReadOnlyBuffer() => null;

    /// <summary>
    ///     Relative <i>get</i> method. Reads the short at this buffer's current position,
    ///     and then increments the position.
    /// </summary>
    /// <returns>The short at the buffer's current position.</returns>
    /// <exception cref="BufferUnderflowException">
    ///     If the buffer's current position is not smaller than its limit.
    /// </exception>
    public override short Get() => 0;

    /// <summary>
    ///     Relative <i>put</i> method <i>(optional operation)</i>.
    /// </summary>
    /// <param name="s">The short to be written.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="BufferOverflowException">
    ///     If this buffer's current position is not smaller than its limit.
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">If this buffer is read-only.</exception>
    public override ShortBuffer Put( short s ) => null;

    /// <summary>
    ///     Absolute <i>get</i> method. Reads the short at the given index.
    /// </summary>
    /// <param name="index">The index from which the short will be read.</param>
    /// <returns>The short at the given index.</returns>
    /// <exception cref="IndexOutOfRangeException">
    ///     If <paramref name="index" /> is negative or not smaller than the buffer's limit.
    /// </exception>
    public override short Get( int index ) => 0;

    /// <summary>
    ///     Absolute <i>put</i> method <i>(optional operation)</i>.
    /// </summary>
    /// <remarks>
    ///     <para> Writes the given short into this buffer at the given index. </para>
    /// </remarks>
    /// <param name="index">The index at which the short will be written.</param>
    /// <param name="s">The short value to be written.</param>
    /// <returns>This buffer.</returns>
    /// <exception cref="IndexOutOfRangeException">
    ///     If <paramref name="index" /> is negative or not smaller than the buffer's limit.
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">If this buffer is read-only.</exception>
    public override ShortBuffer Put( int index, short s ) => null;

    /// <summary>
    ///     Compacts this buffer  <i>(optional operation)</i>.
    ///     <para>
    ///         The shorts between the buffer's current position and its limit,
    ///         if any, are copied to the beginning of the buffer. That is, the
    ///         short at index <tt><i>p</i> = position()</tt> is copied to index
    ///         zero, the short at index <i>p</i> + 1 is copied to index one, and
    ///         so forth until the short at index <tt>limit()</tt> - 1 is copied to
    ///         index <i>n</i> = <tt>limit()</tt> - <tt>1</tt> - <i>p</i>.
    ///     </para>
    ///     <para>
    ///         The buffer's position is then set to <i>n+1</i> and its limit is set
    ///         to its capacity. The mark, if defined, is discarded.
    ///     </para>
    ///     <para>
    ///         The buffer's position is set to the number of shorts copied,
    ///         rather than to zero, so that an invocation of this method can be
    ///         followed immediately by an invocation of another relative <i>put</i>
    ///         method
    ///     </para>
    /// </summary>
    /// <returns> This buffer </returns>
    /// <exception cref="ReadOnlyBufferException">If this buffer is read-only</exception>
    public override ShortBuffer Compact() => null;

    /// <summary>
    ///     Retrieves this buffer's byte order.
    ///     <para>
    ///         The byte order of a short buffer created by allocation or by wrapping an
    ///         existing <tt>short</tt> array is the <see cref="ByteOrder.NativeOrder" />
    ///         of the underlying hardware. The byte order of a short buffer created as a
    ///         view of a byte buffer is that of the byte buffer at the moment that the
    ///         view is created.
    ///     </para>
    /// </summary>
    /// <returns> This buffer's byte order </returns>
    public override ByteOrder Order() => null;
}
