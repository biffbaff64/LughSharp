// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Utils.Buffers;

/// <summary>
/// Class with static helper methods to increase the speed of array/direct
/// buffer and direct buffer/direct buffer transfers
/// </summary>
[PublicAPI]
public static partial class BufferUtils
{
    private static readonly List< ByteBuffer > _unsafeBuffers = [ ];

    private static int _allocatedUnsafe = 0;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new <see cref="FloatBuffer"/> with the specified capacity.
    /// All elements will be initialised to zero.
    /// </summary>
    public static FloatBuffer NewFloatBuffer( int numFloats )
    {
        var buffer = FloatBuffer.Allocate( numFloats * 4 );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer;
    }

//    /// <summary>
//    /// Creates a new <see cref="DoubleBuffer"/> with the specified capacity.
//    /// All elements will be initialised to zero.
//    /// </summary>
//    public static DoubleBuffer NewDoubleBuffer( int numFloats )
//    {
//        var buffer = DoubleBuffer.Allocate( numFloats * 4 );
//        buffer.Order( ByteOrder.NativeOrder );
//
//        return buffer;
//    }

    /// <summary>
    /// Creates a new <see cref="ByteBuffer"/> with the specified capacity.
    /// All elements will be initialised to zero.
    /// </summary>
    public static ByteBuffer NewByteBuffer( int numBytes, bool isUnsafe = false )
    {
        var buffer = isUnsafe
            ? throw new NotImplementedException( "Disposable Byte Buffers not implemented!" ) /* NewDisposableByteBufferJni( numBytes ) */
            : ByteBuffer.Allocate( numBytes );

        buffer.Order( ByteOrder.NativeOrder );

        if ( isUnsafe )
        {
            _allocatedUnsafe += numBytes;

            lock ( _unsafeBuffers )
            {
                _unsafeBuffers.Add( buffer );
            }
        }

        return buffer;
    }

    /// <summary>
    /// Creates a new <see cref="ShortBuffer"/> with the specified capacity.
    /// All elements will be initialised to zero.
    /// </summary>
    public static ShortBuffer NewShortBuffer( int numBytes )
    {
        var buffer = ShortBuffer.Allocate( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer;
    }

//    /// <summary>
//    /// Creates a new <see cref="CharBuffer"/> with the specified capacity.
//    /// All elements will be initialised to zero.
//    /// </summary>
//    public static CharBuffer NewCharBuffer( int numBytes )
//    {
//        var buffer = CharBuffer.Allocate( numBytes );
//        buffer.Order( ByteOrder.NativeOrder );
//
//        return buffer;
//    }

    /// <summary>
    /// Creates a new <see cref="IntBuffer"/> with the specified capacity.
    /// All elements will be initialised to zero.
    /// </summary>
    public static IntBuffer NewIntBuffer( int numBytes )
    {
        var buffer = IntBuffer.Allocate( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer;
    }

//    /// <summary>
//    /// Creates a new <see cref="LongBuffer"/> with the specified capacity.
//    /// All elements will be initialised to zero.
//    /// </summary>
//    public static LongBuffer NewLongBuffer( int numBytes )
//    {
//        var buffer = LongBuffer.Allocate( numBytes );
//        buffer.Order( ByteOrder.NativeOrder );
//
//        return buffer;
//    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position()"/> is
    /// used to define the offset into the Buffer itself. The position and limit will stay
    /// the same.
    /// </summary>
    /// <param name="src"> The source array. </param>
    /// <param name="srcOffset"> The offset into the source array. </param>
    /// <param name="numElements"> The number of elements to copy. </param>
    /// <param name="dst"> The destination Buffer, its position is used as an offset. </param>
    public static void Copy( float[] src, int srcOffset, int numElements, Buffer dst )
    {
//        CopyJni( src, srcOffset, dst, PositionInBytes( dst ), numElements << 2 );
    }

    /// <summary>
    /// Copies numFloats floats from src starting at offset to dst. Dst is assumed to
    /// be a direct buffer. The method will crash if that is not the case. The position
    /// and limit of the buffer are ignored, the copy is placed at position 0 in the
    /// buffer. After the copying process the position of the buffer is set to 0 and its
    /// limit set to numFloats * 4 if it is a ByteBuffer and numFloats if it is a
    /// FloatBuffer. In case the Buffer is neither a ByteBuffer nor a FloatBuffer the
    /// limit is not set. This is an expert method, use at your own risk.
    /// </summary>
    /// <param name="src"> The source array </param>
    /// <param name="dst"> The destination buffer, has to be a direct Buffer </param>
    /// <param name="numElements"> The number of floats to copy </param>
    /// <param name="offset"> The offset in src to start copying from </param>
    public static void Copy( float[] src, Buffer dst, int numElements, int offset )
    {
        dst.Limit = dst switch
        {
            ByteBuffer  => numElements << 2,
            FloatBuffer => numElements,
            _           => dst.Limit,
        };

        Array.Copy( src, offset, dst.Hb, 0, numElements );

//        CopyJni( src, dst, numElements, offset );

        dst.Position = 0;
    }

    /// <summary>
    /// Returns the current position in bytes of the specified buffer.
    /// </summary>
    /// <param name="dst">The buffer to get the position from.</param>
    /// <returns>The position of the buffer in bytes.</returns>
    /// <exception cref="GdxRuntimeException">Thrown if the buffer type is unsupported.</exception>
    private static int PositionInBytes( Buffer dst )
    {
        return dst switch
        {
            ByteBuffer               => dst.Position,
            ShortBuffer              => dst.Position << 1,
            IntBuffer or FloatBuffer => dst.Position << 2,
            var _ => throw new GdxRuntimeException
                ( $"Can't get position for {dst.GetType().Name} instance" ),
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
            ByteBuffer               => bytes,
            ShortBuffer              => bytes >>> 1,
            IntBuffer or FloatBuffer => bytes >>> 2,
            var _ => throw new GdxRuntimeException
                ( $"Can't copy to a {dst.GetType().Name} instance" ),
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
            ByteBuffer               => elements,
            ShortBuffer              => elements << 1,
            IntBuffer or FloatBuffer => elements << 2,
            var _ => throw new GdxRuntimeException
                ( $"Can't copy to a {dst.GetType().Name} instance" ),
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void DisposeUnsafeByteBuffer( ByteBuffer buffer )
    {
        var size = buffer.Capacity;

        lock ( _unsafeBuffers )
        {
            if ( !_unsafeBuffers.Remove( buffer ) )
            {
                throw new ArgumentException
                    ( "buffer not allocated with NewUnsafeByteBuffer, or is already disposed" );
            }
        }

        _allocatedUnsafe -= size;
        
        buffer.Dispose();
    }

    // ------------------------------------------------------------------------
    // CHAR
    // ------------------------------------------------------------------------

    public static char GetChar( ByteBuffer bb, int bi, bool bigEndian )
    {
        return bigEndian
            ? MakeChar( bb.Hb![ ( byte )bi ],
                bb.Hb![ ( byte )( bi + 1 ) ] )

            // ---------------------------------------
            : MakeChar( bb.Hb![ ( byte )( bi + 1 ) ],
                bb.Hb![ ( byte )bi ] );
    }

    public static void PutChar( ByteBuffer bb, int bi, char x, bool bigEndian )
    {
        if ( bigEndian )
        {
            bb.Hb![ bi ]     = ( byte )( x >> 8 );
            bb.Hb![ bi + 1 ] = ( byte )x;
        }

        bb.Hb![ bi ]     = ( byte )x;
        bb.Hb![ bi + 1 ] = ( byte )( x >> 8 );
    }

    // ------------------------------------------------------------------------
    // SHORT
    // ------------------------------------------------------------------------

    public static short GetShort( ByteBuffer bb, int bi, bool bigEndian )
    {
        return bigEndian
            ? MakeShort( bb.Hb![ ( byte )bi ],
                bb.Hb![ ( byte )( bi + 1 ) ] )

            // ---------------------------------------
            : MakeShort( bb.Hb![ ( byte )( bi + 1 ) ],
                bb.Hb![ ( byte )bi ] );
    }

    public static void PutShort( ByteBuffer bb, int bi, short x, bool bigEndian )
    {
        if ( bigEndian )
        {
            bb.Hb![ bi ]     = ( byte )( x >> 8 );
            bb.Hb![ bi + 1 ] = ( byte )x;
        }

        bb.Hb![ bi ]     = ( byte )x;
        bb.Hb![ bi + 1 ] = ( byte )( x >> 8 );
    }

    // ------------------------------------------------------------------------
    // INT
    // ------------------------------------------------------------------------

    public static int GetInt( ByteBuffer bb, int bi, bool bigEndian )
    {
        return bigEndian ? GetIntByte( bb, bi ) : GetIntLong( bb, bi );
    }

    public static void PutInt( ByteBuffer bb, int bi, int x, bool bigEndian )
    {
        if ( bigEndian )
        {
            PutIntByte( bb, bi, x );
        }

        PutIntLong( bb, bi, x );
    }

    private static int GetIntByte( ByteBuffer bb, int bi )
    {
        return MakeInt( bb.Hb![ ( byte )bi ],
            bb.Hb![ ( byte )( bi + 1 ) ],
            bb.Hb![ ( byte )( bi + 2 ) ],
            bb.Hb![ ( byte )( bi + 3 ) ] );
    }

    private static int GetIntLong( ByteBuffer bb, int bi )
    {
        return MakeInt( bb.Hb![ ( byte )( bi + 3 ) ],
            bb.Hb![ ( byte )( bi + 2 ) ],
            bb.Hb![ ( byte )( bi + 1 ) ],
            bb.Hb![ ( byte )bi ] );
    }

    private static void PutIntByte( ByteBuffer bb, int bi, int x )
    {
        bb.Hb![ bi ]     = ( byte )( x >> 24 );
        bb.Hb![ bi + 1 ] = ( byte )( x >> 16 );
        bb.Hb![ bi + 2 ] = ( byte )( x >> 8 );
        bb.Hb![ bi + 3 ] = ( byte )x;
    }

    private static void PutIntLong( ByteBuffer bb, int bi, int x )
    {
        bb.Hb![ bi ]     = ( byte )x;
        bb.Hb![ bi + 1 ] = ( byte )( x >> 8 );
        bb.Hb![ bi + 2 ] = ( byte )( x >> 16 );
        bb.Hb![ bi + 3 ] = ( byte )( x >> 24 );
    }

    // ------------------------------------------------------------------------
    // LONG
    // ------------------------------------------------------------------------

    public static long GetLong( ByteBuffer bb, int bi, bool bigEndian )
    {
        return bigEndian ? GetLongByte( bb, bi ) : GetLongLong( bb, bi );
    }

    public static void PutLong( ByteBuffer bb, int bi, long x, bool bigEndian )
    {
        if ( bigEndian )
        {
            PutLongByte( bb, bi, x );
        }

        PutLongLong( bb, bi, x );
    }

    private static long GetLongByte( ByteBuffer bb, int bi )
    {
        return MakeLong( bb.Hb![ ( byte )bi ],
            bb.Hb![ ( byte )( bi + 1 ) ],
            bb.Hb![ ( byte )( bi + 2 ) ],
            bb.Hb![ ( byte )( bi + 3 ) ],
            bb.Hb![ ( byte )( bi + 4 ) ],
            bb.Hb![ ( byte )( bi + 5 ) ],
            bb.Hb![ ( byte )( bi + 6 ) ],
            bb.Hb![ ( byte )( bi + 7 ) ] );
    }

    private static long GetLongLong( ByteBuffer bb, int bi )
    {
        return MakeLong( bb.Hb![ ( byte )( bi + 7 ) ],
            bb.Hb![ ( byte )( bi + 6 ) ],
            bb.Hb![ ( byte )( bi + 5 ) ],
            bb.Hb![ ( byte )( bi + 4 ) ],
            bb.Hb![ ( byte )( bi + 3 ) ],
            bb.Hb![ ( byte )( bi + 2 ) ],
            bb.Hb![ ( byte )( bi + 1 ) ],
            bb.Hb![ ( byte )bi ] );
    }

    private static void PutLongByte( ByteBuffer bb, int bi, long x )
    {
        bb.Hb![ bi ]     = ( byte )( x >> 56 );
        bb.Hb![ bi + 1 ] = ( byte )( x >> 48 );
        bb.Hb![ bi + 2 ] = ( byte )( x >> 40 );
        bb.Hb![ bi + 3 ] = ( byte )( x >> 32 );
        bb.Hb![ bi + 4 ] = ( byte )( x >> 24 );
        bb.Hb![ bi + 5 ] = ( byte )( x >> 16 );
        bb.Hb![ bi + 6 ] = ( byte )( x >> 8 );
        bb.Hb![ bi + 7 ] = ( byte )x;
    }

    private static void PutLongLong( ByteBuffer bb, int bi, long x )
    {
        bb.Hb![ bi ]     = ( byte )x;
        bb.Hb![ bi + 1 ] = ( byte )( x >> 8 );
        bb.Hb![ bi + 2 ] = ( byte )( x >> 16 );
        bb.Hb![ bi + 3 ] = ( byte )( x >> 24 );
        bb.Hb![ bi + 4 ] = ( byte )( x >> 32 );
        bb.Hb![ bi + 5 ] = ( byte )( x >> 40 );
        bb.Hb![ bi + 6 ] = ( byte )( x >> 48 );
        bb.Hb![ bi + 7 ] = ( byte )( x >> 56 );
    }

    // ------------------------------------------------------------------------
    // DOUBLE
    // ------------------------------------------------------------------------

    public static double GetDouble( ByteBuffer bb, int bi, bool bigEndian )
    {
        return bigEndian ? GetDoubleByte( bb, bi ) : GetDoubleLong( bb, bi );
    }

    public static void PutDouble( ByteBuffer bb, int bi, double x, bool bigEndian )
    {
        if ( bigEndian )
        {
            PutDoubleByte( bb, bi, BitConverter.DoubleToInt64Bits( x ) );
        }

        PutDoubleLong( bb, bi, BitConverter.DoubleToInt64Bits( x ) );
    }

    private static double GetDoubleByte( ByteBuffer bb, int bi )
    {
        return BitConverter.DoubleToInt64Bits( MakeLong( bb.Hb![ bi ],
            bb.Hb[ bi + 1 ],
            bb.Hb[ bi + 2 ],
            bb.Hb[ bi + 3 ],
            bb.Hb[ bi + 4 ],
            bb.Hb[ bi + 5 ],
            bb.Hb[ bi + 6 ],
            bb.Hb[ bi + 7 ] ) );
    }

    private static double GetDoubleLong( ByteBuffer bb, int bi )
    {
        return BitConverter.DoubleToInt64Bits( MakeLong( bb.Hb![ bi + 7 ],
            bb.Hb[ bi + 6 ],
            bb.Hb[ bi + 5 ],
            bb.Hb[ bi + 4 ],
            bb.Hb[ bi + 3 ],
            bb.Hb[ bi + 2 ],
            bb.Hb[ bi + 1 ],
            bb.Hb[ bi ] ) );
    }

    private static void PutDoubleByte( ByteBuffer bb, int bi, long x )
    {
        bb.Hb![ bi ]    = ( byte )( x >> 56 );
        bb.Hb[ bi + 1 ] = ( byte )( x >> 48 );
        bb.Hb[ bi + 2 ] = ( byte )( x >> 40 );
        bb.Hb[ bi + 3 ] = ( byte )( x >> 32 );
        bb.Hb[ bi + 4 ] = ( byte )( x >> 24 );
        bb.Hb[ bi + 5 ] = ( byte )( x >> 16 );
        bb.Hb[ bi + 6 ] = ( byte )( x >> 8 );
        bb.Hb[ bi + 7 ] = ( byte )x;
    }

    private static void PutDoubleLong( ByteBuffer bb, int bi, long x )
    {
        bb.Hb![ bi ]    = ( byte )x;
        bb.Hb[ bi + 1 ] = ( byte )( x >> 8 );
        bb.Hb[ bi + 2 ] = ( byte )( x >> 16 );
        bb.Hb[ bi + 3 ] = ( byte )( x >> 24 );
        bb.Hb[ bi + 4 ] = ( byte )( x >> 32 );
        bb.Hb[ bi + 5 ] = ( byte )( x >> 40 );
        bb.Hb[ bi + 6 ] = ( byte )( x >> 48 );
        bb.Hb[ bi + 7 ] = ( byte )( x >> 56 );
    }

    // ------------------------------------------------------------------------
    // FLOAT
    // ------------------------------------------------------------------------

    public static float GetFloat( ByteBuffer bb, int bi, bool bigEndian )
    {
        return bigEndian ? GetFloatByte( bb, bi ) : GetFloatLong( bb, bi );
    }

    public static void PutFloat( ByteBuffer bb, int bi, float x, bool bigEndian )
    {
        if ( bigEndian )
        {
            PutFloatByte( bb, bi, x );
        }

        PutFloatLong( bb, bi, x );
    }

    private static float GetFloatByte( ByteBuffer bb, int bi )
    {
        return BitConverter.Int32BitsToSingle( GetIntByte( bb, bi ) );
    }

    private static float GetFloatLong( ByteBuffer bb, int bi )
    {
        return BitConverter.Int32BitsToSingle( GetIntLong( bb, bi ) );
    }

    private static void PutFloatByte( ByteBuffer bb, int bi, float x )
    {
        PutIntByte( bb, bi, BitConverter.SingleToInt32Bits( x ) );
    }

    private static void PutFloatLong( ByteBuffer bb, int bi, float x )
    {
        PutIntLong( bb, bi, BitConverter.SingleToInt32Bits( x ) );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Converts the supplied 2 bytes into a char.
    /// </summary>
    /// <returns> The result. </returns>
    private static char MakeChar( byte b1, byte b0 )
    {
        return ( char )( ( b1 << 8 ) | ( b0 & 0xff ) );
    }

    /// <summary>
    /// Converts the supplied 2 bytes into a short.
    /// </summary>
    /// <returns> The result. </returns>
    private static short MakeShort( byte b1, byte b0 )
    {
        return ( short )( ( b1 << 8 ) | ( b0 & 0xff ) );
    }

    /// <summary>
    /// Converts the supplied sequence of 4 bytes into an int.
    /// </summary>
    /// <returns> The result. </returns>
    private static int MakeInt( byte b3, byte b2, byte b1, byte b0 )
    {
        return ( b3 << 24 )
               | ( ( b2 & 0xff ) << 16 )
               | ( ( b1 & 0xff ) << 8 )
               | ( b0 & 0xff );
    }

    /// <summary>
    /// Converts the supplied sequence of 8 bytes into a long.
    /// </summary>
    /// <returns> The result. </returns>
    private static long MakeLong( byte b7, byte b6, byte b5, byte b4, byte b3, byte b2, byte b1, byte b0 )
    {
        return ( ( long )b7 << 56 )
               | ( ( ( long )b6 & 0xff ) << 48 )
               | ( ( ( long )b5 & 0xff ) << 40 )
               | ( ( ( long )b4 & 0xff ) << 32 )
               | ( ( ( long )b3 & 0xff ) << 24 )
               | ( ( ( long )b2 & 0xff ) << 16 )
               | ( ( ( long )b1 & 0xff ) << 8 )
               | ( ( long )b0 & 0xff );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
}