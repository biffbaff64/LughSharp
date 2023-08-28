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

using JetBrains.Annotations;

namespace LibGDXSharp.Utils.Buffers;

[PublicAPI]
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

    /// <inheritdoc cref="ByteBuffer.IsDirect()"/>
    public override bool IsDirect() => false;

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

    // ------------------------------------------------------------------------

    protected override byte Get()
    {
        throw new NotImplementedException();
    }

    protected override byte Get( int index )
    {
        throw new NotImplementedException();
    }

    protected override ByteBuffer Put( byte b )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer Put( int index, byte b )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ByteBuffer.Compact()"/>
    public override ByteBuffer Compact()
    {
        throw new NotImplementedException();
    }

    public override char GetChar()
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutChar( char value )
    {
        throw new NotImplementedException();
    }

    public override char GetChar( int index )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutChar( int index, char value )
    {
        throw new NotImplementedException();
    }

    public override CharBuffer AsCharBuffer()
    {
        throw new NotImplementedException();
    }

    public override short GetShort()
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutShort( short value )
    {
        throw new NotImplementedException();
    }

    public override short GetShort( int index )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutShort( int index, short value )
    {
        throw new NotImplementedException();
    }

    public override ShortBuffer AsShortBuffer()
    {
        throw new NotImplementedException();
    }

    public override int GetInt()
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutInt( int value )
    {
        throw new NotImplementedException();
    }

    public override int GetInt( int index )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutInt( int index, int value )
    {
        throw new NotImplementedException();
    }

    public override IntBuffer AsIntBuffer()
    {
        throw new NotImplementedException();
    }

    public override long GetLong()
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutLong( long value )
    {
        throw new NotImplementedException();
    }

    public override long GetLong( int index )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutLong( int index, long value )
    {
        throw new NotImplementedException();
    }

    public override LongBuffer AsLongBuffer()
    {
        throw new NotImplementedException();
    }

    public override float GetFloat()
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutFloat( float value )
    {
        throw new NotImplementedException();
    }

    public override float GetFloat( int index )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutFloat( int index, float value )
    {
        throw new NotImplementedException();
    }

    public override FloatBuffer AsFloatBuffer()
    {
        throw new NotImplementedException();
    }

    public override double GetDouble()
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutDouble( double value )
    {
        throw new NotImplementedException();
    }

    public override double GetDouble( int index )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer PutDouble( int index, double value )
    {
        throw new NotImplementedException();
    }

    public override DoubleBuffer AsDoubleBuffer()
    {
        throw new NotImplementedException();
    }
}
