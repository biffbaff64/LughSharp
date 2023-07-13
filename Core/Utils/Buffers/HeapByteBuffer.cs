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
public class HeapByteBuffer : ByteBuffer
{
    public HeapByteBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new byte[ cap ], 0 )
    {
    }

    public HeapByteBuffer( byte[]? buf, int off, int len )
        : base( -1, off, off + len, buf!.Length, buf, 0 )
    {
    }

    protected HeapByteBuffer( byte[]? buf, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap, buf, off )
    {
    }

    public override ByteBuffer Slice()
    {
        return new HeapByteBuffer( Hb, -1, 0, this.Remaining(), this.Remaining(), this.Position + Offset );
    }

    public override ByteBuffer Duplicate()
    {
        return new HeapByteBuffer( Hb, this.MarkValue(), this.Position, this.Limit, this.Capacity, Offset );
    }

    public override ByteBuffer AsReadOnlyBuffer()
    {
        throw new NotImplementedException();
        
//        return new HeapByteBufferR( Hb, this.MarkValue(), this.Position, this.Limit, this.Capacity, Offset );
    }

    public override byte Get() => 0;

    public override ByteBuffer Put( byte b ) => null;

    public override byte Get( int index ) => 0;

    public override ByteBuffer Put( int index, byte b ) => null;

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
    public override ByteBuffer Compact() => null;

    public override char GetChar() => '\0';

    public override ByteBuffer PutChar( char value ) => null;

    public override char GetChar( int index ) => '\0';

    public override ByteBuffer PutChar( int index, char value ) => null;

    public override CharBuffer AsCharBuffer() => null;

    public override short GetShort() => 0;

    public override ByteBuffer PutShort( short value ) => null;

    public override short GetShort( int index ) => 0;

    public override ByteBuffer PutShort( int index, short value ) => null;

    public override ShortBuffer AsShortBuffer() => null;

    public override int GetInt() => 0;

    public override ByteBuffer PutInt( int value ) => null;

    public override int GetInt( int index ) => 0;

    public override ByteBuffer PutInt( int index, int value ) => null;

    public override IntBuffer AsIntBuffer() => null;

    public override long GetLong() => 0;

    public override ByteBuffer PutLong( long value ) => null;

    public override long GetLong( int index ) => 0;

    public override ByteBuffer PutLong( int index, long value ) => null;

    public override LongBuffer AsLongBuffer() => null;

    public override float GetFloat() => 0;

    public override ByteBuffer PutFloat( float value ) => null;

    public override float GetFloat( int index ) => 0;

    public override ByteBuffer PutFloat( int index, float value ) => null;

    public override FloatBuffer AsFloatBuffer() => null;

    public override double GetDouble() => 0;

    public override ByteBuffer PutDouble( double value ) => null;

    public override double GetDouble( int index ) => 0;

    public override ByteBuffer PutDouble( int index, double value ) => null;

    public override DoubleBuffer AsDoubleBuffer() => null;

    /// <summary>
    /// Returns the array that backs this buffer  <i>(optional operation)</i>.
    /// <para>
    /// This method is intended to allow array-backed buffers to be passed to
    /// native code more efficiently. Concrete subclasses provide more strongly
    /// typed return values for this method.
    /// </para>
    /// <para>
    /// Modifications to this buffer's content will cause the returned array's
    /// content to be modified, and vice versa.
    /// </para>
    /// <para>
    /// Invoke the <see cref="Buffer.HasArray"/> method before invoking this method in
    /// order to ensure that this buffer has an accessible backing array.
    /// </para>
    /// </summary>
    /// <returns>  The array that backs this buffer </returns>
    public override object BackingArray() => null;

    /// <summary>
    /// Tells whether or not this buffer is <i>direct</i>.
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct </returns>
    public override bool IsDirect() => false;
}