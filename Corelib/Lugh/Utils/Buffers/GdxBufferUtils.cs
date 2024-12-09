// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using Corelib.Lugh.Maths;

namespace Corelib.Lugh.Utils.Buffers;

public partial class BufferUtils
{
    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used
    /// to define the offset into the Buffer itself. The position will stay the same, the limit
    /// will be set to position + numElements. <b>The Buffer must be a direct Buffer with extern
    /// byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( byte[] src, int srcOffset, Buffer dst, int numElements )
    {
        dst.Limit = ( dst.Position + BytesToElements( dst, numElements ) );
        
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used to
    /// define the offset into the Buffer itself. The position will stay the same, the limit
    /// will be set to position + numElements. <b>The Buffer must be a direct Buffer with extern
    /// byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( short[] src, int srcOffset, Buffer dst, int numElements )
    {
        dst.Limit = ( dst.Position + BytesToElements( dst, numElements << 1 ) );

        Array.Copy( src, srcOffset, dst.BackingArray(), PositionInBytes( dst ), numElements << 1 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used
    /// to define the offset into the Buffer itself. The position and limit will stay the same.
    /// <b>The Buffer must be a direct Buffer with extern byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    public static void Copy( char[] src, int srcOffset, int numElements, Buffer dst )
    {
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 1 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used
    /// to define the offset into the Buffer itself. The position and limit will stay the same.
    /// <b>The Buffer must be a direct Buffer with extern byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    public static void Copy( int[] src, int srcOffset, int numElements, Buffer dst )
    {
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 2 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used
    /// to define the offset into the Buffer itself. The position and limit will stay the same.
    /// <b>The Buffer must be a direct Buffer with extern byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    public static void Copy( long[] src, int srcOffset, int numElements, Buffer dst )
    {
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 3 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used to
    /// define the offset into the Buffer itself. The position and limit will stay the same.
    /// <b>The Buffer must be a direct Buffer with extern byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    public static void Copy( double[] src, int srcOffset, int numElements, Buffer dst )
    {
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 3 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used to
    /// define the offset into the Buffer itself. The position will stay the same, the limit
    /// will be set to position + numElements. <b>The Buffer must be a direct Buffer with
    /// extern byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( char[] src, int srcOffset, Buffer dst, int numElements )
    {
        dst.Limit = ( dst.Position + BytesToElements( dst, numElements << 1 ) );
        
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 1 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used to
    /// define the offset into the Buffer itself. The position will stay the same, the limit
    /// will be set to position + numElements. <b>The Buffer must be a direct Buffer with extern
    /// byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( int[] src, int srcOffset, Buffer dst, int numElements )
    {
        dst.Limit = ( dst.Position + BytesToElements( dst, numElements << 2 ) );
        
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 2 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used to
    /// define the offset into the Buffer itself. The position will stay the same, the limit
    /// will be set to position + numElements. <b>The Buffer must be a direct Buffer with extern
    /// byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( long[] src, int srcOffset, Buffer dst, int numElements )
    {
        dst.Limit = ( dst.Position + BytesToElements( dst, numElements << 3 ) );
        
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 3 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used to
    /// define the offset into the Buffer itself. The position will stay the same, the limit
    /// will be set to position + numElements. <b>The Buffer must be a direct Buffer with extern
    /// byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( float[] src, int srcOffset, Buffer dst, int numElements )
    {
        dst.Limit = ( dst.Position + BytesToElements( dst, numElements << 2 ) );
        
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 2 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position"/> is used to
    /// define the offset into the Buffer itself. The position will stay the same, the limit
    /// will be set to position + numElements. <b>The Buffer must be a direct Buffer with
    /// extern byte order. No error checking is performed</b>.
    /// </summary>
    /// <param name="src"> the source array. </param>
    /// <param name="srcOffset"> the offset into the source array. </param>
    /// <param name="dst"> the destination Buffer, its position is used as an offset. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( double[] src, int srcOffset, Buffer dst, int numElements )
    {
        dst.Limit = ( dst.Position + BytesToElements( dst, numElements << 3 ) );
        
        Array.Copy( src, srcOffset, dst.Hb, PositionInBytes( dst ), numElements << 3 );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from the current position of src,
    /// copying numElements elements (using the data type of src, no matter the datatype
    /// of dst). The dst <see cref="Buffer.Position"/> is used as the writing offset.
    /// The position of both Buffers will stay the same. The limit of the src Buffer will
    /// stay the same. The limit of the dst Buffer will be set to dst.Position + numElements,
    /// where numElements are translated to the number of elements appropriate for the dst
    /// Buffer data type.
    /// </summary>
    /// <param name="src"> the source Buffer. </param>
    /// <param name="dst"> the destination Buffer. </param>
    /// <param name="numElements"> the number of elements to copy. </param>
    public static void Copy( Buffer src, Buffer dst, int numElements )
    {
        var numBytes = ElementsToBytes( src, numElements );

        dst.Limit = dst.Position + BytesToElements( dst, numBytes );

        Array.Copy( src.BackingArray(),
                        PositionInBytes( src ),
                        dst.BackingArray(),
                        PositionInBytes( dst ),
                        numBytes );

        Array.Copy( src.Hb, PositionInBytes( src ), dst.Hb, PositionInBytes( dst ), numBytes );
    }

    /// <summary>
    /// Multiply float vector components within the buffer with the specified matrix. The
    /// <see cref="Buffer.Position"/> is used as the offset.
    /// </summary>
    /// <param name="data"> The buffer to transform. </param>
    /// <param name="dimensions">
    /// The number of components of the vector (2 for xy, 3 for xyz or 4 for xyzw)
    /// </param>
    /// <param name="strideInBytes">
    /// The offset between the first and the second vector to transform.
    /// </param>
    /// <param name="count"> The number of vectors to transform </param>
    /// <param name="matrix"> The matrix to multiply the vector with </param>
    public static void Transform( Buffer data, int dimensions, int strideInBytes, int count, Matrix4 matrix )
    {
        Transform( data, dimensions, strideInBytes, count, matrix, 0 );
    }

    /// <summary>
    /// Multiply float vector components within the buffer with the specified matrix. The
    /// <see cref="Buffer.Position"/> is used as the
    /// offset.
    /// </summary>
    /// <param name="data"> The buffer to transform. </param>
    /// <param name="dimensions">
    /// The number of components of the vector (2 for xy, 3 for xyz or 4 for xyzw).
    /// </param>
    /// <param name="strideInBytes">
    /// The offset between the first and the second vector to transform.
    /// </param>
    /// <param name="count"> The number of vectors to transform </param>
    /// <param name="matrix"> The matrix to multiply the vector with </param>
    public static void Transform( float[] data, int dimensions, int strideInBytes, int count, Matrix4 matrix )
    {
        Transform( data, dimensions, strideInBytes, count, matrix, 0 );
    }

    /// <summary>
    /// Multiply float vector components within the buffer with the specified matrix. The specified
    /// offset value is added to the <see cref="Buffer.Position"/> and used as the offset.
    /// </summary>
    /// <param name="data"> The buffer to transform. </param>
    /// <param name="dimensions">
    /// The number of components of the vector (2 for xy, 3 for xyz or 4 for xyzw).
    /// </param>
    /// <param name="strideInBytes">
    /// The offset between the first and the second vector to transform.
    /// </param>
    /// <param name="count"> The number of vectors to transform </param>
    /// <param name="matrix"> The matrix to multiply the vector with </param>
    /// <param name="offset">
    /// The offset within the buffer (in bytes relative to the current position) to the vector.
    /// </param>
    public static void Transform( Buffer data, int dimensions, int strideInBytes, int count, Matrix4 matrix, int offset )
    {
        //TODO:
//        switch ( dimensions )
//        {
//            case 4:
//                TransformV4M4Jni( data, strideInBytes, count, matrix.Val, PositionInBytes( data ) + offset );
//                break;
//            case 3:
//                TransformV3M4Jni( data, strideInBytes, count, matrix.Val, PositionInBytes( data ) + offset );
//                break;
//            case 2:
//                TransformV2M4Jni( data, strideInBytes, count, matrix.Val, PositionInBytes( data ) + offset );
//                break;
//            default:
//                throw new ArgumentException();
//        }
    }

    /// <summary>
    /// Multiply float vector components within the buffer with the specified matrix. The specified
    /// offset value is added to the <see cref="Buffer.Position"/> and used as the offset.
    /// </summary>
    /// <param name="data"> The buffer to transform. </param>
    /// <param name="dimensions"> The number of components of the vector (2 for xy, 3 for xyz or 4 for xyzw) </param>
    /// <param name="strideInBytes"> The offset between the first and the second vector to transform </param>
    /// <param name="count"> The number of vectors to transform </param>
    /// <param name="matrix"> The matrix to multiply the vector with </param>
    /// <param name="offset">
    /// The offset within the buffer (in bytes relative to the current position) to the vector.
    /// </param>
    public static void Transform( float[] data, int dimensions, int strideInBytes, int count, Matrix4 matrix, int offset )
    {
        //TODO:
//        switch ( dimensions )
//        {
//            case 4:
//                TransformV4M4Jni( data, strideInBytes, count, matrix.Val, offset );
//                break;
//            case 3:
//                TransformV3M4Jni( data, strideInBytes, count, matrix.Val, offset );
//                break;
//            case 2:
//                TransformV2M4Jni( data, strideInBytes, count, matrix.Val, offset );
//                break;
//            default:
//                throw new ArgumentException();
//        }
    }

    /// <summary>
    /// Multiply float vector components within the buffer with the specified matrix.
    /// The <see cref="Buffer.Position"/> is used as the offset.
    /// </summary>
    /// <param name="data"> The buffer to transform. </param>
    /// <param name="dimensions"> The number of components (x, y, z) of the vector (2 for xy or 3 for xyz) </param>
    /// <param name="strideInBytes"> The offset between the first and the second vector to transform </param>
    /// <param name="count"> The number of vectors to transform </param>
    /// <param name="matrix"> The matrix to multiply the vector with </param>
    public static void Transform( Buffer data, int dimensions, int strideInBytes, int count, Matrix3 matrix )
    {
        Transform( data, dimensions, strideInBytes, count, matrix, 0 );
    }

    /// <summary>
    /// Multiply float vector components within the buffer with the specified matrix.
    /// The <see cref="Buffer.Position"/> is used as the offset.
    /// </summary>
    /// <param name="data"> The buffer to transform. </param>
    /// <param name="dimensions"> The number of components (x, y, z) of the vector (2 for xy or 3 for xyz) </param>
    /// <param name="strideInBytes"> The offset between the first and the second vector to transform </param>
    /// <param name="count"> The number of vectors to transform </param>
    /// <param name="matrix"> The matrix to multiply the vector with </param>
    public static void Transform( float[] data, int dimensions, int strideInBytes, int count, Matrix3 matrix )
    {
        Transform( data, dimensions, strideInBytes, count, matrix, 0 );
    }

    /// <summary>
    /// Multiply float vector components within the buffer with the specified matrix. The specified
    /// offset value is added to the <see cref="Buffer.Position"/> and used as the offset.
    /// </summary>
    /// <param name="data"> The buffer to transform. </param>
    /// <param name="dimensions"> The number of components (x, y, z) of the vector (2 for xy or 3 for xyz) </param>
    /// <param name="strideInBytes"> The offset between the first and the second vector to transform </param>
    /// <param name="count"> The number of vectors to transform </param>
    /// <param name="matrix"> The matrix to multiply the vector with, </param>
    /// <param name="offset"> The offset within the buffer (in bytes relative to the current position) to the vector </param>
    public static void Transform( Buffer data, int dimensions, int strideInBytes, int count, Matrix3 matrix, int offset )
    {
        //TODO:
//        switch ( dimensions )
//        {
//            case 3:
//                TransformV3M3Jni( data, strideInBytes, count, matrix.Val, PositionInBytes( data ) + offset );
//                break;
//            case 2:
//                TransformV2M3Jni( data, strideInBytes, count, matrix.Val, PositionInBytes( data ) + offset );
//                break;
//            default:
//                throw new ArgumentException();
//        }
    }

    /// <summary>
    /// Multiply float vector components within the buffer with the specified matrix. The specified
    /// offset value is added to the <see cref="Buffer.Position"/> and used as the offset.
    /// </summary>
    /// <param name="data"> The buffer to transform. </param>
    /// <param name="dimensions"> The number of components (x, y, z) of the vector (2 for xy or 3 for xyz) </param>
    /// <param name="strideInBytes"> The offset between the first and the second vector to transform </param>
    /// <param name="count"> The number of vectors to transform </param>
    /// <param name="matrix"> The matrix to multiply the vector with, </param>
    /// <param name="offset"> The offset within the buffer (in bytes relative to the current position) to the vector </param>
    public static void Transform( float[] data, int dimensions, int strideInBytes, int count, Matrix3 matrix, int offset )
    {
        //TODO:
//        switch ( dimensions )
//        {
//            case 3:
//                TransformV3M3Jni( data, strideInBytes, count, matrix.Val, offset );
//                break;
//            case 2:
//                TransformV2M3Jni( data, strideInBytes, count, matrix.Val, offset );
//                break;
//            default:
//                throw new ArgumentException();
//        }
    }

//    public static long FindFloats( Buffer vertex, int strideInBytes, Buffer vertices, int numVertices )
//    {
//        return FindJni( vertex, PositionInBytes( vertex ), strideInBytes, vertices, PositionInBytes( vertices ), numVertices );
//    }

//    public static long FindFloats( float[] vertex, int strideInBytes, Buffer vertices, int numVertices )
//    {
//        return FindJni( vertex, 0, strideInBytes, vertices, PositionInBytes( vertices ), numVertices );
//    }

//    public static long FindFloats( Buffer vertex, int strideInBytes, float[] vertices, int numVertices )
//    {
//        return FindJni( vertex, PositionInBytes( vertex ), strideInBytes, vertices, 0, numVertices );
//    }

//    public static long FindFloats( float[] vertex, int strideInBytes, float[] vertices, int numVertices )
//    {
//        return FindJni( vertex, 0, strideInBytes, vertices, 0, numVertices );
//    }

//    public static long FindFloats( Buffer vertex, int strideInBytes, Buffer vertices, int numVertices, float epsilon )
//    {
//        return FindJni( vertex, PositionInBytes( vertex ), strideInBytes, vertices, PositionInBytes( vertices ), numVertices, epsilon );
//    }

//    public static long FindFloats( float[] vertex, int strideInBytes, Buffer vertices, int numVertices, float epsilon )
//    {
//        return FindJni( vertex, 0, strideInBytes, vertices, PositionInBytes( vertices ), numVertices, epsilon );
//    }

//    public static long FindFloats( Buffer vertex, int strideInBytes, float[] vertices, int numVertices, float epsilon )
//    {
//        return FindJni( vertex, PositionInBytes( vertex ), strideInBytes, vertices, 0, numVertices, epsilon );
//    }

//    public static long FindFloats( float[] vertex, int strideInBytes, float[] vertices, int numVertices, float epsilon )
//    {
//        return FindJni( vertex, 0, strideInBytes, vertices, 0, numVertices, epsilon );
//    }

//    public static bool IsUnsafeByteBuffer( ByteBuffer buffer )
//    {
//        lock ( _unsafeBuffers )
//        {
//            return _unsafeBuffers.Contains( buffer );
//        }
//    }

//    /// <summary>
//    /// Allocates a new direct ByteBuffer from extern heap memory using the extern
//    /// byte order. Needs to be disposed with <see cref="DisposeUnsafeByteBuffer(ByteBuffer)"/>.
//    /// </summary>
//    public static ByteBuffer NewUnsafeByteBuffer( int numBytes )
//    {
//        var buffer = NewDisposableByteBufferJni( numBytes );
//        buffer.Order( ByteOrder.NativeOrder );
//
//        _allocatedUnsafe += numBytes;
//
//        lock ( _unsafeBuffers )
//        {
//            _unsafeBuffers.Add( buffer );
//        }
//
//        return buffer;
//    }

//    /// <summary>
//    /// Returns the address of the Buffer, it assumes it is an unsafe buffer.
//    /// </summary>
//    /// <param name="buffer"> The Buffer to ask the address for. </param>
//    /// <returns> the address of the Buffer. </returns>
//    public static long GetUnsafeBufferAddress( Buffer buffer )
//    {
//        return GetBufferAddressJni( buffer ) + buffer.Position;
//    }

//    /// <summary>
//    /// Registers the given ByteBuffer as an unsafe ByteBuffer. The ByteBuffer must have
//    /// been allocated in extern code, pointing to a memory region allocated via malloc.
//    /// Needs to be disposed with <see cref="DisposeUnsafeByteBuffer(ByteBuffer)"/>.
//    /// </summary>
//    /// <param name="buffer"> The <see cref="ByteBuffer"/> to register. </param>
//    /// <returns> The ByteBuffer passed to the method. </returns>
//    public static ByteBuffer NewUnsafeByteBuffer( ByteBuffer buffer )
//    {
//        _allocatedUnsafe += buffer.Capacity;
//
//        lock ( _unsafeBuffers )
//        {
//            _unsafeBuffers.Add( buffer );
//        }
//
//        return buffer;
//    }

//    /// <summary>
//    /// Returns the number of bytes allocated with <see cref="NewUnsafeByteBuffer"/>
//    /// </summary>
//    public static int GetAllocatedBytesUnsafe()
//    {
//        return _allocatedUnsafe;
//    }

    // ========================================================================
    // ========================================================================

    #region Jni equivalent methods

//    private static extern void FreeMemoryJni( ByteBuffer buffer );
//    /* free(buffer); */

//    private static extern ByteBuffer NewDisposableByteBufferJni( int numBytes );
//    /* return env->NewDirectByteBuffer((char*)malloc(numBytes), numBytes); */

//    private static extern long GetBufferAddressJni( Buffer buffer );
//    /* return (jlong) buffer; */

//    private static extern void ClearJni( ByteBuffer buffer, int numBytes );
//    /* memset(buffer, 0, numBytes); */

//    private static extern void Array.Copy( float[] src, Buffer dst, int numFloats, int offset );
//    /* memcpy(dst, src + offset, numFloats << 2 ); */

//    private static extern void Array.Copy( byte[] src, int srcOffset, Buffer dst, int dstOffset, int numBytes );
//    /* memcpy(dst + dstOffset, src + srcOffset, numBytes); */

//    private static extern void Array.Copy( char[] src, int srcOffset, Buffer dst, int dstOffset, int numBytes );
//    /* memcpy(dst + dstOffset, src + srcOffset, numBytes); */

//    private static extern void Array.Copy( short[] src, int srcOffset, Buffer dst, int dstOffset, int numBytes );
//    /* memcpy(dst + dstOffset, src + srcOffset, numBytes); */

//    private static extern void Array.Copy( int[] src, int srcOffset, Buffer dst, int dstOffset, int numBytes );
//    /* memcpy(dst + dstOffset, src + srcOffset, numBytes); */

//    private static extern void Array.Copy( long[] src, int srcOffset, Buffer dst, int dstOffset, int numBytes );
//    /* memcpy(dst + dstOffset, src + srcOffset, numBytes); */

//    private static extern void Array.Copy( float[] src, int srcOffset, Buffer dst, int dstOffset, int numBytes );
//    /* memcpy(dst + dstOffset, src + srcOffset, numBytes); */

//    private static extern void Array.Copy( double[] src, int srcOffset, Buffer dst, int dstOffset, int numBytes );
//    /* memcpy(dst + dstOffset, src + srcOffset, numBytes); */

//    private static extern void Array.Copy( Buffer src, int srcOffset, Buffer dst, int dstOffset, int numBytes );
//    /* memcpy(dst + dstOffset, src + srcOffset, numBytes); */

//    private static extern void TransformV4M4Jni( Buffer data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<4, 4>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV4M4Jni( float[] data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<4, 4>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV3M4Jni( Buffer data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<3, 4>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV3M4Jni( float[] data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<3, 4>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV2M4Jni( Buffer data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<2, 4>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV2M4Jni( float[] data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<2, 4>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV3M3Jni( Buffer data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<3, 3>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV3M3Jni( float[] data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<3, 3>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV2M3Jni( Buffer data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<2, 3>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern void TransformV2M3Jni( float[] data, int strideInBytes, int count, float[] matrix, int offsetInBytes );
//    /* transform<2, 3>((float*)data, strideInBytes / 4, count, (float*)matrix, offsetInBytes / 4); */

//    private static extern long FindJni( Buffer vertex, int vertexOffsetInBytes, int strideInBytes, Buffer vertices, int verticesOffsetInBytes, int numVertices );
//    /* return find((float *)&vertex[vertexOffsetInBytes / 4], (unsigned int)(strideInBytes / 4), (float*)&vertices[verticesOffsetInBytes / 4], (unsigned int)numVertices); */

//    private static extern long FindJni( float[] vertex, int vertexOffsetInBytes, int strideInBytes, Buffer vertices, int verticesOffsetInBytes, int numVertices );
//    /* return find((float *)&vertex[vertexOffsetInBytes / 4], (unsigned int)(strideInBytes / 4), (float*)&vertices[verticesOffsetInBytes / 4], (unsigned int)numVertices); */

//    private static extern long FindJni( Buffer vertex, int vertexOffsetInBytes, int strideInBytes, float[] vertices, int verticesOffsetInBytes, int numVertices );
//    /* return find((float *)&vertex[vertexOffsetInBytes / 4], (unsigned int)(strideInBytes / 4), (float*)&vertices[verticesOffsetInBytes / 4], (unsigned int)numVertices); */

//    private static extern long FindJni( float[] vertex, int vertexOffsetInBytes, int strideInBytes, float[] vertices, int verticesOffsetInBytes, int numVertices );
//    /* return find((float *)&vertex[vertexOffsetInBytes / 4], (unsigned int)(strideInBytes / 4), (float*)&vertices[verticesOffsetInBytes / 4], (unsigned int)numVertices); */

//    private static extern long FindJni( Buffer vertex, int vertexOffsetInBytes, int strideInBytes, Buffer vertices, int verticesOffsetInBytes, int numVertices, float epsilon );
//    /* return find((float *)&vertex[vertexOffsetInBytes / 4], (unsigned int)(strideInBytes / 4), (float*)&vertices[verticesOffsetInBytes / 4], (unsigned int)numVertices, epsilon); */

//    private static extern long FindJni( float[] vertex, int vertexOffsetInBytes, int strideInBytes, Buffer vertices, int verticesOffsetInBytes, int numVertices, float epsilon );
//    /* return find((float *)&vertex[vertexOffsetInBytes / 4], (unsigned int)(strideInBytes / 4), (float*)&vertices[verticesOffsetInBytes / 4], (unsigned int)numVertices, epsilon); */

//    private static extern long FindJni( Buffer vertex, int vertexOffsetInBytes, int strideInBytes, float[] vertices, int verticesOffsetInBytes, int numVertices, float epsilon );
//    /* return find((float *)&vertex[vertexOffsetInBytes / 4], (unsigned int)(strideInBytes / 4), (float*)&vertices[verticesOffsetInBytes / 4], (unsigned int)numVertices, epsilon); */

//    private static extern long FindJni( float[] vertex, int vertexOffsetInBytes, int strideInBytes, float[] vertices, int verticesOffsetInBytes, int numVertices, float epsilon );
//    /* return find((float *)&vertex[vertexOffsetInBytes / 4], (unsigned int)(strideInBytes / 4), (float*)&vertices[verticesOffsetInBytes / 4], (unsigned int)numVertices, epsilon); */

    #endregion Jni equivalent methods
}