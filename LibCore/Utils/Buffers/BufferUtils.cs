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


namespace LibGDXSharp.LibCore.Utils.Buffers;

/// <summary>
///     Class with static helper methods to increase the speed of array/direct
///     buffer and direct buffer/direct buffer transfers
/// </summary>
[PublicAPI]
public static class BufferUtils
{
    private static List< ByteBuffer > _unsafeBuffers   = new();
    private static int                _allocatedUnsafe = 0;

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

    public static byte Compare( byte x, byte y ) => ( byte )( x - y );

    public static char Compare( char x, char y ) => ( char )( x - y );

    public static int Compare( int x, int y ) => x - y;

    public static float Compare( float x, float y ) => x - y;

    /// <summary>
    ///     Copies the contents of src to dst, starting from the current position of src,
    ///     copying numElements elements (using the data type of src, no matter the datatype
    ///     of dst). The dst <see cref="Buffer.Position" /> is used as the writing offset.
    ///     The position of both Buffers will stay the same. The limit of the src Buffer will
    ///     stay the same. The limit of the dst Buffer will be set to dst.Position + numElements,
    ///     where numElements are translated to the number of elements appropriate for the dst
    ///     Buffer data type.
    ///     <b>
    ///         The Buffers must be direct Buffers with native byte order.
    ///         No error checking is performed
    ///     </b>
    ///     .
    /// </summary>
    /// <param name="src"> the source Buffer. </param>
    /// <param name="dst"> the destination Buffer. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( Buffer src, Buffer dst, int numElements )
    {
        var numBytes = ElementsToBytes( src, numElements );

        dst.Limit = dst.Position + BytesToElements( dst, numBytes );

        Copy( src, PositionInBytes( src ), dst, PositionInBytes( dst ), numBytes );
    }

    /// <summary>
    ///     Copies the contents of src to dst, starting from src[srcOffset],
    ///     copying numElements elements. The <see cref="Buffer" /> instance's
    ///     <see cref="Buffer.Position()" /> is used to define the offset into
    ///     the Buffer itself. The position and limit will stay the same.
    ///     <para>
    ///         The Buffer must be a direct Buffer with native byte order. No error
    ///         checking is performed
    ///     </para>
    ///     .
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
        return dst switch
               {
                   ByteBuffer                 => dst.Position,
                   ShortBuffer or CharBuffer  => dst.Position << 1,
                   IntBuffer or FloatBuffer   => dst.Position << 2,
                   LongBuffer or DoubleBuffer => dst.Position << 3,
                   _                          => throw new GdxRuntimeException
                       ( $"Can't get position for {dst.GetType().Name} instance" )
               };
    }

    /// <summary>
    /// </summary>
    /// <param name="dst"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private static int BytesToElements( Buffer dst, int bytes )
    {
        return dst switch
               {
                   ByteBuffer                 => bytes,
                   ShortBuffer or CharBuffer  => bytes >>> 1,
                   IntBuffer or FloatBuffer   => bytes >>> 2,
                   LongBuffer or DoubleBuffer => bytes >>> 3,
                   _                          => throw new GdxRuntimeException
                       ( $"Can't copy to a {dst.GetType().Name} instance" )
               };
    }

    /// <summary>
    /// </summary>
    /// <param name="dst"></param>
    /// <param name="elements"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    private static int ElementsToBytes( Buffer dst, int elements )
    {
        return dst switch
               {
                   ByteBuffer                 => elements,
                   ShortBuffer or CharBuffer  => elements << 1,
                   IntBuffer or FloatBuffer   => elements << 2,
                   LongBuffer or DoubleBuffer => elements << 3,
                   _                          => throw new GdxRuntimeException
                       ( $"Can't copy to a {dst.GetType().Name} instance" )
               };
    }

    public static void DisposeUnsafeByteBuffer( ByteBuffer byteBuffer )
    {
        // TODO:
    }
}
