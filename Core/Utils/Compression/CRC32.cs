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

using LibGDXSharp.Utils.Buffers;
using LibGDXSharp.Utils.Compression;

namespace LibGDXSharp.Utils;

/// <summary>
/// A class that can be used to compute the CRC-32 of a data stream.
/// <para>
/// Passing a <b>null</b> argument to a method in this class will cause
/// a <see cref="NullReferenceException"/> to be thrown.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class CRC32 : IChecksum
{
    private int _crc;

    /// <summary>
    /// Creates a new CRC32 object.
    /// </summary>
    public CRC32()
    {
    }

    /// <summary>
    /// Updates the CRC-32 checksum with the specified byte (the low
    /// eight bits of the argument b).
    /// </summary>
    /// <param name="b"> the byte to update the checksum with </param>
    public void Update( int b )
    {
        _crc = Update( _crc, b );
    }

    /// <summary>
    /// Updates the CRC-32 checksum with the specified array of bytes.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">
    /// if <tt>off</tt> is negative, or <tt>len</tt> is negative, or <tt>off+len</tt>
    /// is greater than the length of the array <tt>b</tt>
    /// </exception>
    public void Update( byte[] b, int off, int len )
    {
        ArgumentNullException.ThrowIfNull( b );

        if ( ( off < 0 ) || ( len < 0 ) || ( off > ( b.Length - len ) ) )
        {
            throw new System.IndexOutOfRangeException();
        }

        _crc = UpdateBytes( _crc, b, off, len );
    }

    /// <summary>
    /// Updates the CRC-32 checksum with the specified array of bytes.
    /// </summary>
    /// <param name="b"> the array of bytes to update the checksum with </param>
    public void Update( byte[] b )
    {
        _crc = UpdateBytes( _crc, b, 0, b.Length );
    }

    /// <summary>
    /// Updates the checksum with the bytes from the specified buffer.
    /// <para>    
    /// The checksum is updated using <see cref="Buffers.Buffer.Remaining"/>
    /// bytes starting at <see cref="Buffers.Buffer.Position"/>
    /// Upon return, the buffer's position will be updated to its limit;
    /// its limit will not have been changed.
    /// </para>    
    /// </summary>
    /// <param name="buffer"> the ByteBuffer to update the checksum with</param>
    public void Update( ByteBuffer buffer )
    {
        var pos   = buffer.Position;
        var limit = buffer.Limit;

        Debug.Assert( ( pos <= limit ) );

        var rem = limit - pos;

        if ( rem <= 0 ) return;

//        if ( buffer is IDirectBuffer )
//        {
//            _crc = UpdateByteBuffer( _crc, ( ( IDirectBuffer )buffer ).Address(), pos, rem );
//        }
//        else
        {
            if ( buffer.HasArray() )
            {
                _crc = UpdateBytes( _crc, buffer.Array(), pos + buffer.ArrayOffset(), rem );
            }
            else
            {
                var b = new byte[ rem ];

                buffer.Get( b );

                _crc = UpdateBytes( _crc, b, 0, b.Length );
            }
        }

        buffer.Position = limit;
    }

    /// <summary>
    /// Resets CRC-32 to initial value.
    /// </summary>
    public void Reset()
    {
        _crc = 0;
    }

    /// <summary>
    /// Returns CRC-32 value.
    /// </summary>
    public long Value => _crc & 0xffffffffL;

    private int Update( int crc, int b )
    {
        throw new NotImplementedException();
    }
    
    private int UpdateBytes( int crc, byte[] b, int off, int len )
    {
        throw new NotImplementedException();
    }
    
    private int UpdateByteBuffer( int adler, long addr, int off, int len )
    {
        throw new NotImplementedException();
    }
}
