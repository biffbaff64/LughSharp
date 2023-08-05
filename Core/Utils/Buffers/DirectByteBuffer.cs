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
public class DirectByteBuffer : MappedByteBuffer
{
    public DirectByteBuffer( int capacity )
        : base( -1, 0, capacity, capacity )
    {
    }

    public override object BackingArray() => null;

    public override bool IsDirect() => false;

    public override ByteBuffer Slice() => null;

    public override ByteBuffer Duplicate() => null;

    public override ByteBuffer AsReadOnlyBuffer() => null;

    public override byte Get() => 0;

    public override ByteBuffer Put( byte b ) => null;

    public override byte Get( int index ) => 0;

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

    public override DoubleBuffer AsDoubleBuffer() => null;
}
