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

using System.Diagnostics;

namespace LibGDXSharp.Utils.Buffers;

public sealed class DirectByteBuffer : MappedByteBuffer
{
    private readonly static bool Unaligned = Bits.Unaligned();
    private readonly static bool Unsafe    = Bits.Unsafe();

    public DirectByteBuffer( int capacity )
        : base( -1, 0, capacity, capacity )
    {
    }

    public DirectByteBuffer( long addr, int cap, Object ob )
        : base( -1, 0, cap, cap )
    {
        address = addr;
        cleaner = null;
        att     = ob;
    }

    public DirectByteBuffer( IDirectBuffer db, // package-private
                      int mark,
                      int pos,
                      int lim,
                      int cap,
                      int off )
        : base( mark, pos, lim, cap )
    {
        address = db.address() + off;

        cleaner = null;
        att     = db;
    }

    public override object BackingArray() => null;

    public override bool IsDirect() => true;

    public override ByteBuffer Slice()
    {
        var pos = this.Position;
        var lim = this.Limit;

        Debug.Assert( pos <= lim );

        var rem = ( pos <= lim ? lim - pos : 0 );
        var off = ( pos << 0 );

        Debug.Assert( off >= 0 );

        return new DirectByteBuffer( this, -1, 0, rem, rem, off );
    }

    public override ByteBuffer Duplicate() => null;

    public override ByteBuffer AsReadOnlyBuffer() => null;

    protected override byte Get() => 0;

    protected override ByteBuffer Put( byte b ) => null;

    protected override byte Get( int index ) => 0;

    public override ByteBuffer Put( int index, byte b ) => null;

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

    public override DoubleBuffer AsDoubleBuffer()
    {
        var off = this.Position;
        var lim = this.Limit;

        Debug.Assert( off <= lim );

        var rem = ( off <= lim ? lim - off : 0 );

        var size = rem >> 3;

        if ( !Unaligned && ( ( address + off ) % ( 1 << 3 ) != 0 ) )
        {
            return ( bigEndian
                ? ( DoubleBuffer )( new ByteBufferAsDoubleBufferB
                    (
                    this,
                    -1,
                    0,
                    size,
                    size,
                    off
                    ) )
                : ( DoubleBuffer )( new ByteBufferAsDoubleBufferL
                    (
                    this,
                    -1,
                    0,
                    size,
                    size,
                    off
                    ) ) );
        }
        else
        {
            return ( nativeByteOrder
                ? ( DoubleBuffer )( new DirectDoubleBufferU
                    (
                    this,
                    -1,
                    0,
                    size,
                    size,
                    off
                    ) )
                : ( DoubleBuffer )( new DirectDoubleBufferS
                    (
                    this,
                    -1,
                    0,
                    size,
                    size,
                    off
                    ) ) );
        }
    }
}
