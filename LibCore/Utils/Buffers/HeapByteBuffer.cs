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
public class HeapByteBuffer : ByteBuffer
{
    public HeapByteBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new byte[ cap ] )
    {
    }

    public HeapByteBuffer( byte[]? buf, int off, int len )
        : base( -1, off, off + len, buf!.Length, buf )
    {
    }

    protected HeapByteBuffer( byte[]? buf, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap, buf, off )
    {
    }

    /// <inheritdoc cref="ByteBuffer.IsDirect()" />
    public override bool IsDirect()
    {
        return false;
    }

    public override ByteBuffer Slice()
    {
        return new HeapByteBuffer( Hb, -1, 0, Remaining(), Remaining(), Position + Offset );
    }

    public override ByteBuffer Duplicate()
    {
        return new HeapByteBuffer( Hb, MarkValue(), Position, Limit, Capacity, Offset );
    }

    public override ByteBuffer AsReadOnlyBuffer()
    {
        throw new NotImplementedException();
    }

    //        return new HeapByteBufferR( Hb, this.MarkValue(), this.Position, this.Limit, this.Capacity, Offset );
    // ------------------------------------------------------------------------

    public override byte Get()
    {
        throw new NotImplementedException();
    }

    public override byte Get( int index )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer Put( byte b )
    {
        throw new NotImplementedException();
    }

    public override ByteBuffer Put( int index, byte b )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ByteBuffer.Compact()" />
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