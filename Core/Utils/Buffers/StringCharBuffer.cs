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

public class StringCharBuffer : CharBuffer
{
    public StringCharBuffer( string csq, int start, int end )
        : base( 0, 0, 0, 0 )
    {
    }

    /// <summary>
    /// Tells whether or not this buffer is <i>direct</i>.
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct </returns>
    public override bool IsDirect() => false;

    /// <summary>
    /// Relative <i>get</i> method.  Reads the char at this buffer's
    /// current position, and then increments the position.
    /// </summary>
    /// <returns>The char at the buffer's current position</returns>
    /// <exception cref="BufferUnderflowException">
    /// If the buffer's current position is not smaller than its limit
    /// </exception>
    public override char Get() => '\0';

    /// <summary>
    /// Relative <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// Writes the given char into this buffer at the current position, and then
    /// increments the position.
    /// </para>
    /// </summary>
    /// <param name="c">The char to be written</param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If this buffer's current position is not smaller than its limit
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    /// If this buffer is read-only, which it shouldn't be!````
    /// </exception>
    public override CharBuffer Put( char c ) => null;

    /// <summary>
    /// Absolute <i>get</i> method.  Reads the char at the given index.
    /// </summary>
    /// <param name="index">The index from which the char will be read</param>
    /// <returns> The char at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    public override char Get( int index ) => '\0';

    /// <summary>
    /// Absolute <i>get</i> method. Reads the char at the given index without
    /// any validation of the index.
    /// </summary>
    /// <param name="index">The index from which the char will be read</param>
    /// <returns>  The char at the given index </returns>
    public override char GetUnchecked( int index ) => '\0';

    /// <summary>
    /// Absolute <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// Writes the given char into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index">
    /// The index at which the char will be written
    /// </param>
    /// <param name="c"> The char value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">If this buffer is read-only </exception>
    public override CharBuffer Put( int index, char c ) => null;

    /// <summary>
    /// Compacts this buffer  <i>(optional operation)</i>.
    /// The chars between the buffer's current position and its limit, if any, are
    /// copied to the beginning of the buffer. That is, the char at index p = position()
    /// is copied to index zero, the char at index p + 1 is copied to index one, and so
    /// forth until the char at index limit() - 1 is copied to index n = limit() - 1 - p.
    /// The buffer's position is then set to n+1 and its limit is set to its capacity.
    /// The mark, if defined, is discarded.
    /// <para>
    /// The buffer's position is set to the number of chars copied, rather than to zero,
    /// so that an invocation of this method can be followed immediately by an invocation
    /// of another relative put method.
    /// </para>
    /// </summary>
    /// <returns> This Buffer </returns>
    public override CharBuffer Compact() => null;

    /// <summary>
    /// Creates a new char buffer whose content is a shared subsequence of
    /// this buffer's content.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of chars remaining in this buffer, and its mark
    /// will be undefined.  The new buffer will be direct if, and only if, this
    /// buffer is direct, and it will be read-only if, and only if, this buffer
    /// is read-only.
    /// </para>
    /// </summary>
    /// <returns>  The new char buffer </returns>
    public override CharBuffer Slice() => null;

    /// <summary>
    /// Creates a new char buffer that shares this buffer's content.
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
    /// <returns>  The new char buffer </returns>
    public override CharBuffer Duplicate() => null;

    /// <summary>
    /// Creates a new, read-only char buffer that shares this buffer's
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
    /// exactly the same way as the <see cref="duplicate duplicate"/> method.
    /// </para>
    /// </summary>
    /// <returns>  The new, read-only char buffer </returns>
    public override CharBuffer AsReadOnlyBuffer() => null;

    public override string ToString( int start, int end ) => null;

    /// <summary>
    /// Retrieves this buffer's byte order.
    /// <para>
    /// The byte order of a char buffer created by allocation or by wrapping an existing
    /// <tt>char</tt> array is the <see cref="ByteOrder.NATIVE_ORDER"/> of the underlying
    /// hardware.  The byte order of a char buffer created as a view of a byte buffer is
    /// that of the byte buffer at the moment that the view is created.
    /// </para>
    /// </summary>
    /// <returns> This buffer's byte order </returns>
    public override ByteOrder Order() => null;

    /// <summary>
    /// Creates a new character buffer that represents the specified subsequence
    /// of this buffer, relative to the current position.
    /// 
    /// <para> The new buffer will share this buffer's content; that is, if the
    /// content of this buffer is mutable then modifications to one buffer will
    /// cause the other to be modified.  The new buffer's capacity will be that
    /// of this buffer, its position will be
    /// <tt>position()</tt> + <tt>start</tt>, and its limit will be
    /// <tt>position()</tt> + <tt>end</tt>.  The new buffer will be
    /// direct if, and only if, this buffer is direct, and it will be read-only
    /// if, and only if, this buffer is read-only.  </para>
    /// </summary>
    /// <param name="start">
    ///         The index, relative to the current position, of the first
    ///         character in the subsequence; must be non-negative and no larger
    ///         than <tt>Remaining()</tt>
    /// </param>
    /// <param name="end">
    ///         The index, relative to the current position, of the character
    ///         following the last character in the subsequence; must be no
    ///         smaller than <tt>start</tt> and no larger than
    ///         <tt>Remaining()</tt>
    /// </param>
    /// <returns> The new character buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on <tt>start</tt> and <tt>end</tt> do not hold
    /// </exception>
    public override CharBuffer SubSequence( int start, int end ) => null;
}
