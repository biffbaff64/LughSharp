using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS0184
namespace LibGDXSharp.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class BufferUtils
{
    public static ByteBuffer NewByteBuffer( int capacity )
    {
        return ByteBuffer.AllocateDirect( capacity );
    }

    public static IntBuffer NewIntBuffer( int capacity )
    {
        return IntBuffer.AllocateDirect( capacity );
    }

    public static FloatBuffer NewFloatBuffer( int capacity )
    {
        throw new NotImplementedException();
    }

    public static void Copy( Buffer src, Buffer dst, int numElements )
    {
        var numBytes = ElementsToBytes( src, numElements );

        dst.SetLimit( dst.Position + BytesToElements( dst, numBytes ) );

        CopyBufferToBuffer( src, PositionInBytes( src ), dst, PositionInBytes( dst ), numBytes );
    }

    private static int PositionInBytes( Buffer dst )
    {
        if ( dst is ByteBuffer )
        {
            return dst.Position;
        }
        else if ( dst is IntBuffer )
        {
            return dst.Position << 2;
        }
        else if ( dst is FloatBuffer )
        {
            return dst.Position << 2;
        }
        else if ( dst is DoubleBuffer )
        {
            return dst.Position << 3;
        }
// To be implemented...
//        else if ( dst is ShortBuffer )
//        {
//            return dst.Position << 1;
//        }
//        else if ( dst is CharBuffer )
//        {
//            return dst.Position << 1;
//        }
//        else if ( dst is LongBuffer )
//        {
//            return dst.Position << 3;
//        }
        else
        {
            throw new GdxRuntimeException( "Can't copy to a " + dst.GetType().FullName + " instance" );
        }
    }

    private static int BytesToElements( Buffer dst, int bytes )
    {
        if ( dst is ByteBuffer )
        {
            return bytes;
        }
        else if ( dst is IntBuffer )
        {
            return bytes >>> 2;
        }
        else if ( dst is FloatBuffer )
        {
            return bytes >>> 2;
        }
        else if ( dst is DoubleBuffer )
        {
            return bytes >>> 3;
        }
// To be implemented...
//        else if ( dst is ShortBuffer )
//        {
//            return bytes >>> 1;
//        }
//        else if ( dst is CharBuffer )
//        {
//            return bytes >>> 1;
//        }
//        else if ( dst is LongBuffer )
//        {
//            return bytes >>> 3;
//        }
        else
        {
            throw new GdxRuntimeException( "Can't copy to a " + dst.GetType().FullName + " instance" );
        }
    }

    private static int ElementsToBytes( Buffer dst, int elements )
    {
        if ( dst is ByteBuffer )
        {
            return elements;
        }
        else if ( dst is IntBuffer )
        {
            return elements << 2;
        }
        else if ( dst is FloatBuffer )
        {
            return elements << 2;
        }
        else if ( dst is DoubleBuffer )
        {
            return elements << 3;
        }
// To be implemented...
//        else if ( dst is ShortBuffer )
//        {
//            return elements << 1;
//        }
//        else if ( dst is CharBuffer )
//        {
//            return elements << 1;
//        }
//        else if ( dst is LongBuffer )
//        {
//            return elements << 3;
//        }
        else
        {
            throw new GdxRuntimeException( "Can't copy to a " + dst.GetType().FullName + " instance" );
        }
    }
}
