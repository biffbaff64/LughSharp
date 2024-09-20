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


using System.Runtime.CompilerServices;
using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Utils.Buffers;

/// <summary>
/// Class with static helper methods to increase the speed of array/direct
/// buffer and direct buffer/direct buffer transfers
/// </summary>
[PublicAPI]
public static class BufferUtils
{
    private readonly static List< ByteBuffer > _unsafeBuffers = new();

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

    /// <summary>
    /// Creates a new <see cref="DoubleBuffer"/> with the specified capacity.
    /// All elements will be initialised to zero.
    /// </summary>
    public static DoubleBuffer NewDoubleBuffer( int numFloats )
    {
        var buffer = DoubleBuffer.Allocate( numFloats * 4 );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer;
    }

    /// <summary>
    /// Creates a new <see cref="ByteBuffer"/> with the specified capacity.
    /// All elements will be initialised to zero.
    /// </summary>
    public static ByteBuffer NewByteBuffer( int numBytes, bool isUnsafe = false )
    {
        var buffer = isUnsafe
                         ? NewDisposableByteBuffer( numBytes )
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
    /// 
    /// </summary>
    /// <param name="numBytes"></param>
    /// <returns></returns>
    public static ByteBuffer NewDisposableByteBuffer( int numBytes )
    {
        //TODO:

        return ByteBuffer.Allocate( numBytes );
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

    /// <summary>
    /// Creates a new <see cref="CharBuffer"/> with the specified capacity.
    /// All elements will be initialised to zero.
    /// </summary>
    public static CharBuffer NewCharBuffer( int numBytes )
    {
        var buffer = CharBuffer.Allocate( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer;
    }

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

    /// <summary>
    /// Creates a new <see cref="LongBuffer"/> with the specified capacity.
    /// All elements will be initialised to zero.
    /// </summary>
    public static LongBuffer NewLongBuffer( int numBytes )
    {
        var buffer = LongBuffer.Allocate( numBytes );
        buffer.Order( ByteOrder.NativeOrder );

        return buffer;
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

        Copy( src, PositionInBytes( src ), dst, PositionInBytes( dst ), numBytes );
    }

    /// <summary>
    /// Copies the contents of src to dst, starting from src[srcOffset], copying numElements
    /// elements. The <see cref="Buffer"/> instance's <see cref="Buffer.Position()"/> is
    /// used to define the offset into the Buffer itself. The position and limit will stay
    /// the same.
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
            var _ => throw new GdxRuntimeException
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
            var _ => throw new GdxRuntimeException
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
            var _ => throw new GdxRuntimeException
                         ( $"Can't copy to a {dst.GetType().Name} instance" )
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
    }

    // ------------------------------------------------------------------------
    // CHAR
    // ------------------------------------------------------------------------

    public static char GetChar( ByteBuffer bb, int bi, bool bigEndian )
    {
        return bigEndian
                   ? MakeChar( bb.Hb![ ( byte ) bi ],
                               bb.Hb![ ( byte ) ( bi + 1 ) ] )

                   // ---------------------------------------
                   : MakeChar( bb.Hb![ ( byte ) ( bi + 1 ) ],
                               bb.Hb![ ( byte ) bi ] );
    }

    public static void PutChar( ByteBuffer bb, int bi, char x, bool bigEndian )
    {
        if ( bigEndian )
        {
            bb.Hb![ bi ]     = ( byte ) ( x >> 8 );
            bb.Hb![ bi + 1 ] = ( byte ) x;
        }

        bb.Hb![ bi ]     = ( byte ) x;
        bb.Hb![ bi + 1 ] = ( byte ) ( x >> 8 );
    }

    // ------------------------------------------------------------------------
    // SHORT
    // ------------------------------------------------------------------------

    public static short GetShort( ByteBuffer bb, int bi, bool bigEndian )
    {
        return bigEndian
                   ? MakeShort( bb.Hb![ ( byte ) bi ],
                                bb.Hb![ ( byte ) ( bi + 1 ) ] )

                   // ---------------------------------------
                   : MakeShort( bb.Hb![ ( byte ) ( bi + 1 ) ],
                                bb.Hb![ ( byte ) bi ] );
    }

    public static void PutShort( ByteBuffer bb, int bi, short x, bool bigEndian )
    {
        if ( bigEndian )
        {
            bb.Hb![ bi ]     = ( byte ) ( x >> 8 );
            bb.Hb![ bi + 1 ] = ( byte ) x;
        }

        bb.Hb![ bi ]     = ( byte ) x;
        bb.Hb![ bi + 1 ] = ( byte ) ( x >> 8 );
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
        return MakeInt( bb.Hb![ ( byte ) bi ],
                        bb.Hb![ ( byte ) ( bi + 1 ) ],
                        bb.Hb![ ( byte ) ( bi + 2 ) ],
                        bb.Hb![ ( byte ) ( bi + 3 ) ] );
    }

    private static int GetIntLong( ByteBuffer bb, int bi )
    {
        return MakeInt( bb.Hb![ ( byte ) ( bi + 3 ) ],
                        bb.Hb![ ( byte ) ( bi + 2 ) ],
                        bb.Hb![ ( byte ) ( bi + 1 ) ],
                        bb.Hb![ ( byte ) bi ] );
    }

    private static void PutIntByte( ByteBuffer bb, int bi, int x )
    {
        bb.Hb![ bi ]     = ( byte ) ( x >> 24 );
        bb.Hb![ bi + 1 ] = ( byte ) ( x >> 16 );
        bb.Hb![ bi + 2 ] = ( byte ) ( x >> 8 );
        bb.Hb![ bi + 3 ] = ( byte ) x;
    }

    private static void PutIntLong( ByteBuffer bb, int bi, int x )
    {
        bb.Hb![ bi ]     = ( byte ) x;
        bb.Hb![ bi + 1 ] = ( byte ) ( x >> 8 );
        bb.Hb![ bi + 2 ] = ( byte ) ( x >> 16 );
        bb.Hb![ bi + 3 ] = ( byte ) ( x >> 24 );
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
        return MakeLong( bb.Hb![ ( byte ) bi ],
                         bb.Hb![ ( byte ) ( bi + 1 ) ],
                         bb.Hb![ ( byte ) ( bi + 2 ) ],
                         bb.Hb![ ( byte ) ( bi + 3 ) ],
                         bb.Hb![ ( byte ) ( bi + 4 ) ],
                         bb.Hb![ ( byte ) ( bi + 5 ) ],
                         bb.Hb![ ( byte ) ( bi + 6 ) ],
                         bb.Hb![ ( byte ) ( bi + 7 ) ] );
    }

    private static long GetLongLong( ByteBuffer bb, int bi )
    {
        return MakeLong( bb.Hb![ ( byte ) ( bi + 7 ) ],
                         bb.Hb![ ( byte ) ( bi + 6 ) ],
                         bb.Hb![ ( byte ) ( bi + 5 ) ],
                         bb.Hb![ ( byte ) ( bi + 4 ) ],
                         bb.Hb![ ( byte ) ( bi + 3 ) ],
                         bb.Hb![ ( byte ) ( bi + 2 ) ],
                         bb.Hb![ ( byte ) ( bi + 1 ) ],
                         bb.Hb![ ( byte ) bi ] );
    }

    private static void PutLongByte( ByteBuffer bb, int bi, long x )
    {
        bb.Hb![ bi ]     = ( byte ) ( x >> 56 );
        bb.Hb![ bi + 1 ] = ( byte ) ( x >> 48 );
        bb.Hb![ bi + 2 ] = ( byte ) ( x >> 40 );
        bb.Hb![ bi + 3 ] = ( byte ) ( x >> 32 );
        bb.Hb![ bi + 4 ] = ( byte ) ( x >> 24 );
        bb.Hb![ bi + 5 ] = ( byte ) ( x >> 16 );
        bb.Hb![ bi + 6 ] = ( byte ) ( x >> 8 );
        bb.Hb![ bi + 7 ] = ( byte ) x;
    }

    private static void PutLongLong( ByteBuffer bb, int bi, long x )
    {
        bb.Hb![ bi ]     = ( byte ) x;
        bb.Hb![ bi + 1 ] = ( byte ) ( x >> 8 );
        bb.Hb![ bi + 2 ] = ( byte ) ( x >> 16 );
        bb.Hb![ bi + 3 ] = ( byte ) ( x >> 24 );
        bb.Hb![ bi + 4 ] = ( byte ) ( x >> 32 );
        bb.Hb![ bi + 5 ] = ( byte ) ( x >> 40 );
        bb.Hb![ bi + 6 ] = ( byte ) ( x >> 48 );
        bb.Hb![ bi + 7 ] = ( byte ) ( x >> 56 );
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
        bb.Hb![ bi ]    = ( byte ) ( x >> 56 );
        bb.Hb[ bi + 1 ] = ( byte ) ( x >> 48 );
        bb.Hb[ bi + 2 ] = ( byte ) ( x >> 40 );
        bb.Hb[ bi + 3 ] = ( byte ) ( x >> 32 );
        bb.Hb[ bi + 4 ] = ( byte ) ( x >> 24 );
        bb.Hb[ bi + 5 ] = ( byte ) ( x >> 16 );
        bb.Hb[ bi + 6 ] = ( byte ) ( x >> 8 );
        bb.Hb[ bi + 7 ] = ( byte ) x;
    }

    private static void PutDoubleLong( ByteBuffer bb, int bi, long x )
    {
        bb.Hb![ bi ]    = ( byte ) x;
        bb.Hb[ bi + 1 ] = ( byte ) ( x >> 8 );
        bb.Hb[ bi + 2 ] = ( byte ) ( x >> 16 );
        bb.Hb[ bi + 3 ] = ( byte ) ( x >> 24 );
        bb.Hb[ bi + 4 ] = ( byte ) ( x >> 32 );
        bb.Hb[ bi + 5 ] = ( byte ) ( x >> 40 );
        bb.Hb[ bi + 6 ] = ( byte ) ( x >> 48 );
        bb.Hb[ bi + 7 ] = ( byte ) ( x >> 56 );
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
        return ( char ) ( ( b1 << 8 ) | ( b0 & 0xff ) );
    }

    /// <summary>
    /// Converts the supplied 2 bytes into a short.
    /// </summary>
    /// <returns> The result. </returns>
    private static short MakeShort( byte b1, byte b0 )
    {
        return ( short ) ( ( b1 << 8 ) | ( b0 & 0xff ) );
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
             | b0 & 0xff;
    }

    /// <summary>
    /// Converts the supplied sequence of 8 bytes into a long.
    /// </summary>
    /// <returns> The result. </returns>
    private static long MakeLong( byte b7, byte b6, byte b5, byte b4, byte b3, byte b2, byte b1, byte b0 )
    {
        return ( ( long ) b7 << 56 )
             | ( ( ( long ) b6 & 0xff ) << 48 )
             | ( ( ( long ) b5 & 0xff ) << 40 )
             | ( ( ( long ) b4 & 0xff ) << 32 )
             | ( ( ( long ) b3 & 0xff ) << 24 )
             | ( ( ( long ) b2 & 0xff ) << 16 )
             | ( ( ( long ) b1 & 0xff ) << 8 )
             | ( long ) b0 & 0xff;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    //TODO: Should these be moved to NumberUtils or MathUtils??

    public static byte Compare( byte x, byte y )
    {
        return ( byte ) ( x - y );
    }

    public static char Compare( char x, char y )
    {
        return ( char ) ( x - y );
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