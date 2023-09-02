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

/** DirectByteBuffer, DirectReadWriteByteBuffer and DirectReadOnlyHeapByteBuffer compose the implementation of direct byte buffers.
 * <p>
 * DirectByteBuffer implements all the shared readonly methods and is extended by the other two classes.
 * </p>
 * <p>
 * All methods are marked for runtime performance.
 * </p> */
abstract class DirectByteBuffer : BaseByteBuffer, HasArrayBufferView
{
    private readonly List< int > _byteArray;

    DirectByteBuffer( int capacity )
        : this( ArrayBufferNative.create( capacity ), capacity, 0 )
    {
    }

    DirectByteBuffer( ArrayBuffer buf )
        : this( buf, buf.byteLength(), 0 )
    {
    }

    DirectByteBuffer( ArrayBuffer buffer, int capacity, int offset )
        : base( capacity )
    {
        _byteArray = Int8ArrayNative.create( buffer, offset, capacity );
    }

    public ArrayBufferView GetTypedArray()
    {
        return _byteArray;
    }

    public int GetElementSize()
    {
        return 1;
    }

    /*
     * Override ByteBuffer.get(byte[], int, int) to improve performance.
     *
     * (non-Javadoc)
     *
     * @see java.nio.ByteBuffer#get(byte[], int, int)
     */
    public ByteBuffer Get( byte[] dest, int off, int len )
    {
        int length = dest.Length;

        if ( off < 0 || len < 0 || ( long )off + ( long )len > length )
        {
            throw new IndexOutOfRangeException();
        }

        if ( len > Remaining() )
        {
            throw new BufferUnderflowException();
        }

        for ( int i = 0; i < len; i++ )
        {
            dest[ i + off ] = get( position + i );
        }

        position += len;

        return this;
    }

    public byte get()
    {

// if (position == limit) {
// throw new BufferUnderflowException();
// }
        return ( byte )_byteArray.get( position++ );

    }

    public byte get( int index )
    {
// if (index < 0 || index >= limit) {
// throw new IndexOutOfBoundsException();
// }
        return ( byte )_byteArray.get( index );
    }

    public double getDouble()
    {
        return Numbers.longBitsToDouble( getLong() );
    }

    public double getDouble( int index )
    {
        return Numbers.longBitsToDouble( getLong( index ) );
    }

    public float getFloat()
    {
        return Numbers.intBitsToFloat( getInt() );
    }

    public float getFloat( int index )
    {
        return Numbers.intBitsToFloat( getInt( index ) );
    }

    public int getInt()
    {
        int newPosition = position + 4;

// if (newPosition > limit) {
// throw new BufferUnderflowException();
// }
        int result = loadInt( position );
        position = newPosition;

        return result;
    }

    public int getInt( int index )
    {
// if (index < 0 || index + 4 > limit) {
// throw new IndexOutOfBoundsException();
// }
        return loadInt( index );
    }

    public long getLong()
    {
        int newPosition = position + 8;

// if (newPosition > limit) {
// throw new BufferUnderflowException();
// }
        long result = loadLong( position );
        position = newPosition;

        return result;
    }

    public long getLong( int index )
    {
// if (index < 0 || index + 8 > limit) {
// throw new IndexOutOfBoundsException();
// }
        return loadLong( index );
    }

    public short getShort()
    {
        int newPosition = position + 2;

// if (newPosition > limit) {
// throw new BufferUnderflowException();
// }
        short result = loadShort( position );
        position = newPosition;

        return result;
    }

    public short getShort( int index )
    {
// if (index < 0 || index + 2 > limit) {
// throw new IndexOutOfBoundsException();
// }
        return loadShort( index );
    }

    public boolean isDirect()
    {
        return false;
    }

    protected int loadInt( int baseOffset )
    {
        int bytes = 0;

        if ( order == Endianness.BIG_ENDIAN )
        {
            for ( int i = 0; i < 4; i++ )
            {
                bytes = bytes << 8;
                bytes = bytes | ( _byteArray.get( baseOffset + i ) & 0xFF );
            }
        }
        else
        {
            for ( int i = 3; i >= 0; i-- )
            {
                bytes = bytes << 8;
                bytes = bytes | ( _byteArray.get( baseOffset + i ) & 0xFF );
            }
        }

        return bytes;
    }

    protected long loadLong( int baseOffset )
    {
        long bytes = 0;

        if ( order == Endianness.BIG_ENDIAN )
        {
            for ( int i = 0; i < 8; i++ )
            {
                bytes = bytes << 8;
                bytes = bytes | ( _byteArray.get( baseOffset + i ) & 0xFF );
            }
        }
        else
        {
            for ( int i = 7; i >= 0; i-- )
            {
                bytes = bytes << 8;
                bytes = bytes | ( _byteArray.get( baseOffset + i ) & 0xFF );
            }
        }

        return bytes;
    }

    protected short loadShort( int baseOffset )
    {
        short bytes = 0;

        if ( order == Endianness.BIG_ENDIAN )
        {
            bytes =  ( short )( _byteArray.get( baseOffset ) << 8 );
            bytes |= ( _byteArray.get( baseOffset + 1 ) & 0xFF );
        }
        else
        {
            bytes =  ( short )( _byteArray.get( baseOffset + 1 ) << 8 );
            bytes |= ( _byteArray.get( baseOffset ) & 0xFF );
        }

        return bytes;
    }

    protected void store( int baseOffset, int value )
    {
        if ( order == Endianness.BIG_ENDIAN )
        {
            for ( int i = 3; i >= 0; i-- )
            {
                _byteArray.set( baseOffset + i, ( byte )( value & 0xFF ) );
                value = value >> 8;
            }
        }
        else
        {
            for ( int i = 0; i <= 3; i++ )
            {
                _byteArray.set( baseOffset + i, ( byte )( value & 0xFF ) );
                value = value >> 8;
            }
        }
    }

    protected void store( int baseOffset, long value )
    {
        if ( order == Endianness.BIG_ENDIAN )
        {
            for ( int i = 7; i >= 0; i-- )
            {
                _byteArray.set( baseOffset + i, ( byte )( value & 0xFF ) );
                value = value >> 8;
            }
        }
        else
        {
            for ( int i = 0; i <= 7; i++ )
            {
                _byteArray.set( baseOffset + i, ( byte )( value & 0xFF ) );
                value = value >> 8;
            }
        }
    }

    protected void store( int baseOffset, short value )
    {
        if ( order == Endianness.BIG_ENDIAN )
        {
            _byteArray.set( baseOffset, ( byte )( ( value >> 8 ) & 0xFF ) );
            _byteArray.set( baseOffset + 1, ( byte )( value & 0xFF ) );
        }
        else
        {
            _byteArray.set( baseOffset + 1, ( byte )( ( value >> 8 ) & 0xFF ) );
            _byteArray.set( baseOffset, ( byte )( value & 0xFF ) );
        }
    }
}
