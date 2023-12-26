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

namespace LibGDXSharp.Core.Files.Buffers;

/// <summary>
/// Class with static helper methods to increase the speed of array/direct
/// buffer and direct buffer/direct buffer transfers
/// </summary>
[PublicAPI]
public class BufferUtils
{
    private static List< ByteBuffer > _unsafeBuffers   = new();
    private static int                _allocatedUnsafe = 0;

    private BufferUtils()
    {
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

    public static ByteBuffer NewByteBuffer( int numBytes )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer;
    }

    public static ShortBuffer NewShortBuffer( int numBytes )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer.AsShortBuffer();
    }

    public static CharBuffer NewCharBuffer( int numBytes )
    {
        ByteBuffer buffer = ByteBuffer.AllocateDirect( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer.AsCharBuffer();
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

    /// <summary>
    /// Copies the contents of src to dst, starting from the current position of src,
    /// copying numElements elements (using the data type of src, no matter the datatype
    /// of dst). The dst <see cref="Buffer.Position"/> is used as the writing offset.
    /// The position of both Buffers will stay the same. The limit of the src Buffer will
    /// stay the same. The limit of the dst Buffer will be set to dst.Position + numElements,
    /// where numElements are translated to the number of elements appropriate for the dst
    /// Buffer data type. <b>The Buffers must be direct Buffers with native byte order.
    /// No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source Buffer. </param>
    /// <param name="dst"> the destination Buffer. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( Buffer src, Buffer dst, int numElements )
    {
        var numBytes = ElementsToBytes( src, numElements );

        dst.Limit = ( dst.Position + BytesToElements( dst, numBytes ) );

        Copy( src, PositionInBytes( src ), dst, PositionInBytes( dst ), numBytes );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset],
    /// copying numElements elements. The <see cref="Buffer"/> instance's
    /// <see cref="Buffer.Position()"/> is used to define the offset into
    /// the Buffer itself. The position and limit will stay the same.
    /// <para>
    /// The Buffer must be a direct Buffer with native byte order. No error
    /// checking is performed
    /// </para>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    public static void Copy( float[] src, int srcOffset, int numElements, Buffer dst )
    {
        //TODO:
    }

    public static void Copy( short[] src, int offset, Buffer dst, int numElements )
    {
        //TODO:
    }

    public static void Copy( float[] src, Buffer dst, int offset, int numElements )
    {
        //TODO:
    }

    private static void Copy( Buffer src, int srcOffset, Buffer dst, int dstOffset, int numBytes )
    {
        //TODO:
    }

    /// <summary>
    /// </summary>
    /// <param name="dst"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private static int PositionInBytes( Buffer dst )
    {
        if ( dst is ByteBuffer )
        {
            return dst.Position;
        }
        
        if ( dst is ShortBuffer or CharBuffer )
        {
            return dst.Position << 1;
        }
        
        if ( dst is IntBuffer or FloatBuffer )
        {
            return dst.Position << 2;
        }
        
        if ( dst is LongBuffer or DoubleBuffer )
        {
            return dst.Position << 3;
        }

        throw new GdxRuntimeException( $"Can't get position for {dst.GetType().Name} instance" );
    }

    /// <summary>
    /// </summary>
    /// <param name="dst"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private static int BytesToElements( Buffer dst, int bytes )
    {
        if ( dst is ByteBuffer )
        {
            return bytes;
        }
        
        if ( dst is ShortBuffer or CharBuffer )
        {
            return bytes >>> 1;
        }
        
        if ( dst is IntBuffer or FloatBuffer )
        {
            return bytes >>> 2;
        }
        
        if ( dst is LongBuffer or DoubleBuffer )
        {
            return bytes >>> 3;
        }

        throw new GdxRuntimeException( $"Can't copy to a {dst.GetType().Name} instance" );
    }

    /// <summary>
    /// </summary>
    /// <param name="dst"></param>
    /// <param name="elements"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private static int ElementsToBytes( Buffer dst, int elements )
    {
        if ( dst is ByteBuffer )
        {
            return elements;
        }
        
        if ( dst is ShortBuffer or CharBuffer )
        {
            return elements << 1;
        }
        
        if ( dst is IntBuffer or FloatBuffer )
        {
            return elements << 2;
        }
        
        if ( dst is LongBuffer or DoubleBuffer )
        {
            return elements << 3;
        }

        throw new GdxRuntimeException( $"Can't copy to a {dst.GetType().Name} instance" );
    }

    public static void DisposeUnsafeByteBuffer( ByteBuffer byteBuffer )
    {
        // TODO:
    }
}
