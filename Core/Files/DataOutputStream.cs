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

namespace LibGDXSharp.Files;

/// <summary>
/// Wraps an existing <see cref="Stream"/> and writes typed data to it.
/// Typically, this stream can be read in by <see cref="DataInputStream"/>. Types that can be
/// written include byte, 16-bit short, 32-bit int, 32-bit float, 64-bit long,
/// 64-bit double, byte strings, and DataInput MUTF-8 encoded strings.
/// <para/>
/// Java's <see cref="DataOutputStream"/> is similar to .NET's <see cref="BinaryWriter"/>. However, it writes
/// in a modified UTF-8 format that is written in big-endian byte order.
/// This is a port of <see cref="DataOutputStream"/> that is fully compatible with Java's <see cref="DataInputStream"/>.
/// <para>
/// Usage Note: Always favor <see cref="BinaryWriter"/> over <see cref="DataOutputStream"/> unless you specifically need
/// the modified UTF-8 format and/or the <see cref="WriteUTF(string)"/> method.
/// </para>
/// </summary>
/// <seealso cref="DataInputStream"/>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class DataOutputStream : IDataOutput, IDisposable
{
    private readonly object _lock = new object();

    /// <summary>
    /// The number of bytes written out so far.
    /// </summary>
    protected int written;
    private readonly byte[] _buff;
    private readonly bool   _leaveOpen;
    private readonly Stream _output;

    /// <summary>
    /// Constructs a new <see cref="DataOutputStream"/> on the <see cref="Stream"/>
    /// <paramref name="output"/>. Note that data written by this stream is not in a human
    /// readable form but can be reconstructed by using a <see cref="DataInputStream"/>
    /// on the resulting output.
    /// </summary>
    /// <param name="output">The target stream for writing.</param>
    /// <param name="leaveOpen"><c>true</c> to leave the stream open after the <see cref="DataOutputStream"/> object is disposed; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="output"/> is <c>null</c>.</exception>
    /// <seealso cref="DataInputStream"/>
    public DataOutputStream( Stream output, bool leaveOpen = false )
    {
        this._output    = output ?? throw new ArgumentNullException( nameof( output ) );
        this._leaveOpen = leaveOpen;
        _buff           = new byte[ 8 ];
    }

    /// <summary>
    /// Flushes this stream to ensure all pending data is sent out to the target
    /// stream. This implementation then also flushes the target stream.
    /// </summary>
    /// <exception cref="IOException">If an error occurs attempting to flush this stream.</exception>
    protected void Flush() => _output.Flush();

    /// <summary>
    /// Gets the total number of bytes written to the target stream so far.
    /// </summary>
    public int Length
    {
        get
        {
            if ( written < 0 )
            {
                written = int.MaxValue;
            }

            return written;
        }
    }

    /// <summary>
    /// Writes <paramref name="count"/> bytes from the byte array <paramref name="buffer"/> starting at
    /// <paramref name="offset"/> to the target stream.
    /// </summary>
    /// <param name="buffer">The buffer to write to the target stream.</param>
    /// <param name="offset">The index of the first byte in <paramref name="buffer"/> to write.</param>
    /// <param name="count">The number of bytes from the <paramref name="buffer"/> to write.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is <c>null</c>.</exception>
    /// <seealso cref="DataInputStream.ReadFully(byte[])"/>
    /// <seealso cref="DataInputStream.ReadFully(byte[], int, int)"/>
    public virtual void Write( byte[] buffer, int offset, int count )
    {
        ArgumentNullException.ThrowIfNull( buffer );

        lock ( _lock )
        {
            _output.Write( buffer, offset, count );
            written += count;
        }
    }

    /// <summary>
    /// Writes a <see cref="byte"/> to the target stream. Only the least significant byte of
    /// the integer <paramref name="oneByte"/> is written.
    /// </summary>
    /// <param name="oneByte">The <see cref="byte"/> to write to the target stream.</param>
    public virtual void Write( int oneByte )
    {
        lock ( _lock )
        {
            _output.WriteByte( ( byte )oneByte );
            written++;
        }
    }

    /// <summary>
    /// Writes a <see cref="bool"/> to the target stream.
    /// </summary>
    /// <param name="value">The <see cref="bool"/> value to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <seealso cref="DataInputStream.ReadBoolean()"/>
    public void WriteBoolean( bool value )
    {
        lock ( _lock )
        {
            _output.WriteByte( ( byte )( value ? 1 : 0 ) );
            written++;
        }
    }

    /// <summary>
    /// Writes an 8-bit <see cref="byte"/> to the target stream. Only the least significant
    /// byte of the integer <paramref name="value"/> is written.
    /// </summary>
    /// <param name="value">The <see cref="byte"/> value to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <seealso cref="DataInputStream.ReadSByte()"/>
    /// <seealso cref="DataInputStream.ReadByte()"/>
    public void WriteByte( int value )
    {
        lock ( _lock )
        {
            _output.WriteByte( ( byte )value );
            written++;
        }
    }

    /// <summary>
    /// Writes the low order bytes from a string to the target stream.
    /// </summary>
    /// <param name="value">The string containing the bytes to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <c>null</c>.</exception>
    /// <seealso cref="DataInputStream.ReadFully(byte[])"/>
    /// <seealso cref="DataInputStream.ReadFully(byte[], int, int)"/>
    public void WriteBytes( string value )
    {
        ArgumentNullException.ThrowIfNull( value );

        lock ( _lock )
        {
            if ( value.Length == 0 )
            {
                return;
            }

            var bytes = new byte[ value.Length ];

            for ( var index = 0; index < value.Length; index++ )
            {
                bytes[ index ] = ( byte )value[ index ];
            }

            _output.Write( bytes, 0, bytes.Length );
            written += bytes.Length;
        }
    }

    /// <summary>
    /// Writes a 16-bit character to the target stream. Only the two lower bytes
    /// of the integer <paramref name="value"/> are written, with the higher one written
    /// are written, with the higher one written <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The character to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <seealso cref="DataInputStream.ReadChar()"/>
    public void WriteChar( int value )
    {
        lock ( _lock )
        {
            _buff[ 0 ] = ( byte )( value >> 8 );
            _buff[ 1 ] = ( byte )value;
            _output.Write( _buff, 0, 2 );
            written += 2;
        }
    }

    /// <summary>
    /// Writes the 16-bit characters contained in <paramref name="value"/> to the target
    /// stream.
    /// </summary>
    /// <param name="value">The string that contains the characters to write to this
    /// stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <c>null</c>.</exception>
    /// <seealso cref="DataInputStream.ReadChar()"/>
    public void WriteChars( string value )
    {
        ArgumentNullException.ThrowIfNull( value );

        lock ( _lock )
        {
            var newBytes = new byte[ value.Length * 2 ];

            for ( var index = 0; index < value.Length; index++ )
            {
                var newIndex = index == 0 ? index : index * 2;

                newBytes[ newIndex ]     = ( byte )( value[ index ] >> 8 );
                newBytes[ newIndex + 1 ] = ( byte )value[ index ];
            }

            _output.Write( newBytes, 0, newBytes.Length );
            written += newBytes.Length;
        }
    }

    /// <summary>
    /// Writes a 64-bit <see cref="double"/> to the target stream. The resulting output is the
    /// eight bytes resulting from calling <see cref="NumberUtils.DoubleToLongBits(double)"/>.
    /// </summary>
    /// <param name="value">The <see cref="double"/> to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <seealso cref="DataInputStream.ReadDouble()"/>
    public void WriteDouble( double value )
    {
        WriteLong( NumberUtils.DoubleToLongBits( value ) );
    }

    /// <summary>
    /// Writes a 32-bit <see cref="float"/> to the target stream. The resulting output is the
    /// four bytes resulting from calling <see cref="NumberUtils.FloatToIntBits(float)"/>.
    /// <para/>
    /// NOTE: This was writeFloat() in Java
    /// </summary>
    /// <param name="value">The <see cref="float"/> to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <seealso cref="DataInputStream.ReadFloat()"/>
    public void WriteFloat( float value )
    {
        WriteInt( NumberUtils.FloatToIntBits( value ) );
    }

    /// <summary>
    /// Writes a 32-bit <see cref="int"/> to the target stream. The resulting output is the
    /// four bytes, highest order first, of <paramref name="value"/>.
    /// <para/>
    /// </summary>
    /// <param name="value">The <see cref="int"/> to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <seealso cref="DataInputStream.ReadInt()"/>
    public void WriteInt( int value )
    {
        lock ( _lock )
        {
            _buff[ 0 ] = ( byte )( value >> 24 );
            _buff[ 1 ] = ( byte )( value >> 16 );
            _buff[ 2 ] = ( byte )( value >> 8 );
            _buff[ 3 ] = ( byte )value;
            _output.Write( _buff, 0, 4 );
            written += 4;
        }
    }

    /// <summary>
    /// Writes a 64-bit <see cref="long"/> to the target stream. The resulting output is the
    /// eight bytes, highest order first, of <paramref name="value"/>.
    /// <para/>
    /// </summary>
    /// <param name="value">The <see cref="long"/> to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <seealso cref="DataInputStream.ReadLong()"/>
    public void WriteLong( long value )
    {
        lock ( _lock )
        {
            _buff[ 0 ] = ( byte )( value >> 56 );
            _buff[ 1 ] = ( byte )( value >> 48 );
            _buff[ 2 ] = ( byte )( value >> 40 );
            _buff[ 3 ] = ( byte )( value >> 32 );
            _buff[ 4 ] = ( byte )( value >> 24 );
            _buff[ 5 ] = ( byte )( value >> 16 );
            _buff[ 6 ] = ( byte )( value >> 8 );
            _buff[ 7 ] = ( byte )value;
            _output.Write( _buff, 0, 8 );
            written += 8;
        }
    }

    private int WriteLongToBuffer( long value, byte[] buffer, int offset )
    {
        buffer[ offset++ ] = ( byte )( value >> 56 );
        buffer[ offset++ ] = ( byte )( value >> 48 );
        buffer[ offset++ ] = ( byte )( value >> 40 );
        buffer[ offset++ ] = ( byte )( value >> 32 );
        buffer[ offset++ ] = ( byte )( value >> 24 );
        buffer[ offset++ ] = ( byte )( value >> 16 );
        buffer[ offset++ ] = ( byte )( value >> 8 );
        buffer[ offset++ ] = ( byte )value;

        return offset;
    }

    /// <summary>
    /// Writes the specified 16-bit <see cref="short"/> to the target stream. Only the lower
    /// two bytes of the integer <paramref name="value"/> are written, with the higher one
    /// written first.
    /// <para/>
    /// </summary>
    /// <param name="value">The <see cref="short"/> to write to the target stream.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <seealso cref="DataInputStream.ReadShort()"/>
    public void WriteShort( int value )
    {
        lock ( _lock )
        {
            _buff[ 0 ] = ( byte )( value >> 8 );
            _buff[ 1 ] = ( byte )value;
            _output.Write( _buff, 0, 2 );
            written += 2;
        }
    }

    private int WriteShortToBuffer( int value, byte[] buffer, int offset )
    {
        buffer[ offset++ ] = ( byte )( value >> 8 );
        buffer[ offset++ ] = ( byte )value;

        return offset;
    }

    /// <summary>
    /// Writes the specified encoded in DataInput modified UTF-8 to this
    /// stream.
    /// </summary>
    /// <param name="value">The string to write to the target stream encoded in
    /// DataInput modified UTF-8.</param>
    /// <exception cref="IOException">If an error occurs while writing to the target stream.</exception>
    /// <exception cref="FormatException">If the encoded string is longer than 65535 bytes.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <c>null</c>.</exception>
    /// <seealso cref="DataInputStream.ReadUTF()"/>
    public void WriteUTF( string value )
    {
        ArgumentNullException.ThrowIfNull( value );

        var utfCount = CountUTFBytes( value );

        if ( utfCount > 65535 )
        {
            throw new GdxRuntimeException( $"utfCount is too long: {utfCount}" );
        }

        var buffer = new byte[ ( int )utfCount + 2 ];
        var offset = 0;

        offset = WriteShortToBuffer( ( int )utfCount, buffer, offset );
        offset = WriteUTFBytesToBuffer( value, buffer, offset );

        Write( buffer, 0, offset );
    }

    private long CountUTFBytes( string value )
    {
        int utfCount = 0, length = value.Length;

        for ( var i = 0; i < length; i++ )
        {
            int charValue = value[ i ];

            if ( charValue is > 0 and <= 127 )
            {
                utfCount++;
            }
            else if ( charValue <= 2047 )
            {
                utfCount += 2;
            }
            else
            {
                utfCount += 3;
            }
        }

        return utfCount;
    }

    private int WriteUTFBytesToBuffer( string value, byte[] buffer, int offset )
    {
        var length = value.Length;

        for ( var i = 0; i < length; i++ )
        {
            int charValue = value[ i ];

            if ( charValue is > 0 and <= 127 )
            {
                buffer[ offset++ ] = ( byte )charValue;
            }
            else if ( charValue <= 2047 )
            {
                buffer[ offset++ ] = ( byte )( 0xc0 | ( 0x1f & ( charValue >> 6 ) ) );
                buffer[ offset++ ] = ( byte )( 0x80 | ( 0x3f & charValue ) );
            }
            else
            {
                buffer[ offset++ ] = ( byte )( 0xe0 | ( 0x0f & ( charValue >> 12 ) ) );
                buffer[ offset++ ] = ( byte )( 0x80 | ( 0x3f & ( charValue >> 6 ) ) );
                buffer[ offset++ ] = ( byte )( 0x80 | ( 0x3f & charValue ) );
            }
        }

        return offset;
    }

    #region From FilterOutputStream

    /// <summary>
    /// Writes the entire contents of the byte array <paramref name="buffer"/> to this
    /// stream. This implementation writes the <paramref name="buffer"/> to the target
    /// stream.
    /// </summary>
    /// <param name="buffer">The buffer to be written.</param>
    /// <exception cref="IOException">If an I/O error occurs while writing to this stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is <c>null</c>.</exception>
    public void Write( byte[] buffer )
    {
        ArgumentNullException.ThrowIfNull( buffer );

        Write( buffer, 0, buffer.Length );
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( !_leaveOpen ) _output.Dispose();
        }
    }

    #endregion From FilterOutputStream

}