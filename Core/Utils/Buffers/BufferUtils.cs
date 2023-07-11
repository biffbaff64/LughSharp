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
public class BufferUtils
{
    private BufferUtils()
    {
    }

    public static ByteBuffer NewByteBuffer( int numBytes )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer;
    }

    public static CharBuffer NewCharBuffer( int numBytes )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer.AsCharBuffer();
    }

    public static ShortBuffer NewShortBuffer( int numBytes )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer.AsShortBuffer();
    }

    public static IntBuffer NewIntBuffer( int numBytes )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer.AsIntBuffer();
    }

    public static LongBuffer NewLongBuffer( int numBytes )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer.AsLongBuffer();
    }

    public static FloatBuffer NewFloatBuffer( int numFloats )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numFloats * 4 );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer.AsFloatBuffer();
    }

    public static DoubleBuffer NewDoubleBuffer( int numFloats )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numFloats * 4 );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer.AsDoubleBuffer();
    }

    public static byte Compare( byte x, byte y )
    {
        return ( byte )( x - y );
    }

    public static char Compare( char x, char y )
    {
        return ( char )( x - y );
    }

    public static int Compare( int x, int y )
    {
        return x - y;
    }

    public static float Compare( float x, float y )
    {
        return x - y;
    }
}