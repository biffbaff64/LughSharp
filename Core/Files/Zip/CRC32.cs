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

using System.Diagnostics;

using LibGDXSharp.Utils.Compression;

namespace LibGDXSharp.Files.Zip;

/// <summary>
/// A class that can be used to compute the <tt>CRC-32</tt> of a data stream.
/// <para>
/// Passing a <tt>null</tt> argument to a method in this class will cause
/// a <see cref="NullReferenceException"/> to be thrown.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class CRC32 : IChecksum
{
    private int _crc;

    public CRC32()
    {
    }

    /**
     * Updates the CRC-32 checksum with the specified byte (the low
     * eight bits of the argument b).
     *
     * @param b the byte to update the checksum with
     */
    public void Update( int b )
    {
        _crc = Update( _crc, b );
    }

    /**
     * Updates the CRC-32 checksum with the specified array of bytes.
     *
     * @throws  ArrayIndexOutOfBoundsException
     *          if {@code off} is negative, or {@code len} is negative,
     *          or {@code off+len} is greater than the length of the
     *          array {@code b}
     */
    public void Update( byte[] b, int off, int len )
    {
        ArgumentNullException.ThrowIfNull( b );

        if ( ( off < 0 ) || ( len < 0 ) || ( off > ( b.Length - len ) ) )
        {
            throw new IndexOutOfRangeException();
        }

        _crc = UpdateBytes( _crc, b, off, len );
    }

    /**
     * Updates the CRC-32 checksum with the specified array of bytes.
     *
     * @param b the array of bytes to update the checksum with
     */
    public void Update( byte[] b )
    {
        _crc = UpdateBytes( _crc, b, 0, b.Length );
    }

    /**
     * Updates the checksum with the bytes from the specified buffer.
     *
     * The checksum is updated using
     * buffer.{@link java.nio.Buffer#remaining() remaining()}
     * bytes starting at
     * buffer.{@link java.nio.Buffer#position() position()}
     * Upon return, the buffer's position will
     * be updated to its limit; its limit will not have been changed.
     *
     * @param buffer the ByteBuffer to update the checksum with
     * @since 1.8
     */
    public void Update( ByteBuffer buffer )
    {
        var pos   = buffer.Position;
        var limit = buffer.Limit;

        Debug.Assert( pos <= limit );

        var rem = limit - pos;

        if ( rem <= 0 ) return;

        if ( buffer is IDirectBuffer )
        {
            _crc = UpdateByteBuffer( _crc, ( ( IDirectBuffer )buffer ).Address(), pos, rem );
        }
        else if ( buffer.HasArray() )
        {
            _crc = UpdateBytes( _crc, buffer.Array(), pos + buffer.ArrayOffset(), rem );
        }
        else
        {
            var b = new byte[ rem ];

            buffer.Get( b );
            _crc = UpdateBytes( _crc, b, 0, b.Length );
        }

        buffer.Position = limit;
    }

    /**
     * Resets CRC-32 to initial value.
     */
    public void reset()
    {
        _crc = 0;
    }

    /**
     * Returns CRC-32 value.
     */
    public long GetValue()
    {
        return ( _crc & 0xffffffffL );
    }

    public static int Update( int crc, int b );
    private static int UpdateBytes( int crc, byte[] b, int off, int len );
    private static int UpdateByteBuffer( int adler, long addr, int off, int len );
}
