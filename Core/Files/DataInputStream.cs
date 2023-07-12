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
/// Wraps an existing <see cref="Stream"/> and reads typed data from it.
/// Typically, this stream has been written by a DataOutputStream. Types that can
/// be read include byte, 16-bit short, 32-bit int, 32-bit float, 64-bit long,
/// 64-bit double, byte strings, and strings encoded in <see cref="IDataInput"/>
/// modified UTF-8 (MUTF-8 Encoding).
/// <para/>
/// Java's <see cref="DataInputStream"/> is similar to .NET's <see cref="BinaryReader"/>.
/// However, it reads using a modified UTF-8 format (MUTF-8 Encoding) and also reads bytes
/// in big endian order. This is a port of <see cref="DataInputStream"/> that is fully
/// compatible with Java's <see cref="DataOutputStream"/>.
/// <para>
/// Usage Note: Always favor <see cref="BinaryReader"/> over <see cref="DataInputStream"/>
/// unless you specifically need the modified UTF-8 format and/or the <see cref="ReadUTF()"/>
/// or <see cref="ReadUTF(IDataInput)"/> methods.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class DataInputStream : IDataInput, IDisposable
{
    private readonly Stream  _input;
    private readonly bool    _leaveOpen;
    private readonly byte[]  _buff;
    private          char[]? _lineBuffer;

    /// <summary>
    /// Constructs a new <see cref="DataInputStream"/> on the <see cref="Stream"/> <paramref name="input"/>. All
    /// reads are then filtered through this stream. Note that data read by this
    /// stream is not in a human readable format and was most likely created by a
    /// <see cref="DataOutputStream"/>.
    /// </summary>
    /// <param name="input">the source <see cref="Stream"/> the filter reads from.</param>
    /// <param name="leaveOpen"><c>true</c> to leave the stream open after the <see cref="DataInputStream"/> object is disposed; otherwise, <c>false</c>.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="input"/> is <c>null</c>.</exception>
    /// <seealso cref="DataOutputStream"/>
    public DataInputStream( Stream input, bool leaveOpen = false )
    {
        this._input     = input ?? throw new ArgumentNullException( nameof( input ) );
        this._leaveOpen = leaveOpen;
        _buff           = new byte[ 8 ];
    }

    // J2N: This was required by some tests. It will only work for seekable streams,
    // but since "buffered" streams in Java are equivalent, this should work similarly.
    // Non-buffered streams always return 0, anyway.
    /// <summary>
    /// Gets the number of bytes available to read. If the <see cref="Stream.CanSeek"/> property 
    /// of the underlying stream is <c>false</c>, zero will be returned. 
    /// </summary>
    public int Available => !_input.CanSeek ? 0 : ( int )( _input.Length - _input.Position );

    /// <summary>
    /// Reads bytes from this stream into the byte array <paramref name="buffer"/>. Returns
    /// the number of bytes that have been read.
    /// <para/>
    /// IMPORTANT: This method has .NET semantics. That is, it returns <c>0</c> rather than <c>-1</c> when
    /// the end of the stream is reached.
    /// </summary>
    /// <param name="buffer">The buffer to read bytes into.</param>
    /// <returns>The number of bytes read into <paramref name="buffer"/>. This might be less
    /// than the number of bytes requested if that many bytes are not available, or it might
    /// be zero if the end of the stream is reached.</returns>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is <c>null</c>.</exception>
    /// <see cref="IDataOutput.Write(byte[])"/>
    /// <see cref="IDataOutput.Write(byte[], int, int)"/>
    public int Read( byte[] buffer )
    {
        ArgumentNullException.ThrowIfNull( buffer );

        return _input.Read( buffer, 0, buffer.Length );
    }

    /// <summary>
    /// Reads at most <paramref name="length"/> bytes from this stream and stores them in
    /// the byte array <paramref name="buffer"/> starting at <paramref name="offset"/>. Returns the
    /// number of bytes that have been read or zero if no bytes have been read and
    /// the end of the stream has been reached.
    /// <para/>
    /// IMPORTANT: This method has .NET semantics. That is, it returns <c>0</c> rather than <c>-1</c> when
    /// the end of the stream is reached.
    /// </summary>
    /// <param name="buffer">The byte array in which to store the bytes read.</param>
    /// <param name="offset">The initial position in <paramref name="buffer"/> to store the bytes
    /// read from this stream.</param>
    /// <param name="length">the maximum number of bytes to store in <paramref name="buffer"/>.</param>
    /// <returns>The number of bytes read into <paramref name="buffer"/>. This might be less
    /// than the number of bytes requested if that many bytes are not available, or it might
    /// be zero if the end of the stream is reached.</returns>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is <c>null</c>.</exception>
    /// <see cref="IDataOutput.Write(byte[])"/>
    /// <see cref="IDataOutput.Write(byte[], int, int)"/>
    public int Read( byte[] buffer, int offset, int length )
    {
        ArgumentNullException.ThrowIfNull( buffer );

        return _input.Read( buffer, offset, length );
    }

    /// <summary>
    /// Reads a boolean from this stream.
    /// </summary>
    /// <returns>The next boolean value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before one byte
    /// has been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteBoolean(bool)"/>
    public bool ReadBoolean()
    {
        var temp = _input.ReadByte();

        if ( temp < 0 )
        {
            throw new EndOfStreamException();
        }

        return ( temp != 0 );
    }

    /// <summary>
    /// Reads an 8-bit byte value from this stream.
    /// <para/>
    /// </summary>
    /// <returns>The next byte value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before one byte
    /// has been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteByte(int)"/>
    public int ReadByte()
    {
        var temp = _input.ReadByte();

        if ( temp < 0 )
        {
            throw new EndOfStreamException();
        }

        return temp;
    }

    private int ReadToBuff( int count )
    {
        var offset = 0;

        while ( offset < count )
        {
            var bytesRead = _input.Read( _buff, offset, count - offset );

            if ( bytesRead <= 0 ) return bytesRead;
            offset += bytesRead;
        }

        return offset;
    }

    /// <summary>
    /// Reads a 16-bit character value from this stream.
    /// </summary>
    /// <returns>The next <see cref="char"/> value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before two bytes
    /// have been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteChar(int)"/>
    public char ReadChar()
    {
        if ( ReadToBuff( 2 ) <= 0 )
        {
            throw new EndOfStreamException();
        }

        return ( char )( ( ( _buff[ 0 ] & 0xff ) << 8 ) | ( _buff[ 1 ] & 0xff ) );
    }

    /// <summary>
    /// Reads a 64-bit <see cref="double"/> value from this stream.
    /// </summary>
    /// <returns>The next <see cref="double"/> value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before eight bytes
    /// have been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteDouble(double)"/>
    public double ReadDouble()
    {
        return NumberUtils.LongBitsToDouble( ReadLong() );
    }

    /// <summary>
    /// Reads a 32-bit <see cref="float"/> value from this stream.
    /// <para/>
    /// </summary>
    /// <returns>The next <see cref="float"/> value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before four bytes
    /// have been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteFloat(float)"/>
    public float ReadFloat()
    {
        return NumberUtils.IntBitsToFloat( ReadInt() );
    }

    /// <summary>
    /// Reads bytes from this stream into the byte array <paramref name="buffer"/>. This
    /// method will block until <c><paramref name="buffer"/>.Length</c> number of bytes have been
    /// read.
    /// </summary>
    /// <param name="buffer">Buffer to read bytes into.</param>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before enough bytes
    /// have been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is <c>null</c>.</exception>
    /// <seealso cref="IDataOutput.Write(byte[])"/>
    /// <seealso cref="IDataOutput.Write(byte[], int, int)"/>
    public void ReadFully( byte[] buffer )
    {
        ArgumentNullException.ThrowIfNull( buffer );

        ReadFully( buffer, 0, buffer.Length );
    }

    /// <summary>
    /// Reads bytes from this stream and stores them in the byte array <paramref name="buffer"/>
    /// starting at the position <paramref name="offset"/>. This method blocks until
    /// <paramref name="length"/> bytes have been read. If <paramref name="length"/> is zero, then this
    /// method returns without reading any bytes.
    /// </summary>
    /// <param name="buffer">The byte array into which the data is read.</param>
    /// <param name="offset">The offset in <paramref name="buffer"/> from where to store the bytes read.</param>
    /// <param name="length">The maximum number of bytes to read.</param>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before enough bytes
    /// have been read.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> plus <paramref name="length"/> indicates a position not within this instance.
    /// <para/>
    /// -or-
    /// <para/>
    /// <paramref name="offset"/> or <paramref name="length"/> is less than zero.
    /// </exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is <c>null</c>.</exception>
    /// <seealso cref="IDataOutput.Write(byte[])"/>
    /// <seealso cref="IDataOutput.Write(byte[], int, int)"/>
    public void ReadFully( byte[] buffer, int offset, int length )
    {
        ArgumentNullException.ThrowIfNull( buffer );
        
        if ( length < 0 ) throw new ArgumentOutOfRangeException( 
            nameof( length ), @"Non-negative number required." );

        if ( length == 0 ) return;

        if ( offset < 0 ) throw new ArgumentOutOfRangeException(
            nameof( offset ), @"Non-negative number required." );

        if ( offset > ( buffer.Length - length ) ) throw new ArgumentOutOfRangeException(
            nameof( length ), @"Index and length must refer to a location within the string." );

        while ( length > 0 )
        {
            var result = _input.Read( buffer, offset, length );

            if ( result <= 0 )
                throw new EndOfStreamException();

            offset += result;
            length -= result;
        }
    }

    /// <summary>
    /// Reads a 32-bit integer value from this stream.
    /// <para/>
    /// </summary>
    /// <returns>The next <see cref="int"/> value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before four bytes
    /// have been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteInt(int)"/>
    public int ReadInt()
    {
        if ( ReadToBuff( 4 ) <= 0 )
        {
            throw new EndOfStreamException();
        }

        return ( ( _buff[ 0 ] & 0xff ) << 24 )
               | ( ( _buff[ 1 ] & 0xff ) << 16 )
               | ( ( _buff[ 2 ] & 0xff ) << 8 )
               | ( _buff[ 3 ] & 0xff );
    }

    /// <summary>
    /// Returns a string that contains the next line of text available from the
    /// source stream. A line is represented by zero or more characters followed
    /// by <c>'\n'</c>, <c>'\r'</c>, <c>"\r\n"</c> or the end of the stream.
    /// The string does not include the newline sequence.
    /// </summary>
    /// <returns>The contents of the line or <c>null</c> if no characters were
    /// read before the end of the source stream has been reached.</returns>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    [Obsolete( "Use BufferedReader" )]
    public string? ReadLine()
    {
        var buf = _lineBuffer ??= new char[ 128 ];

        var room   = buf.Length;
        var offset = 0;
        int c;

        while ( true )
        {
            switch ( c = _input.ReadByte() )
            {
                case -1:
                case '\n':
                    goto loop;

                case '\r':
                    var c2 = _input.ReadByte();

                    if ( ( c2 != '\n' ) && ( c2 != -1 ) )
                    {
                        using var reader = new StreamReader( _input );

                        c2 = reader.Peek();

                        // http://stackoverflow.com/a/8021738/181087
                        //if (!(in is PushbackInputStream)) {
                        //    this.in = new PushbackInputStream(in);
                        //}
                        //((PushbackInputStream)in).unread(c2);
                    }

                    goto loop;

                default:
                    if ( --room < 0 )
                    {
                        buf  = new char[ offset + 128 ];
                        room = buf.Length - offset - 1;
                        System.Array.Copy( _lineBuffer!, 0, buf, 0, offset );
                        _lineBuffer = buf;
                    }

                    buf[ offset++ ] = ( char )c;

                    break;
            }
        }

        loop:

        if ( ( c == -1 ) && ( offset == 0 ) )
        {
            return null;
        }

        return new string( buf, 0, offset );
    }

    /// <summary>
    /// Reads a 64-bit <see cref="long"/> value from this stream.
    /// <para/>
    /// </summary>
    /// <returns>The next <see cref="long"/> value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before four eight
    /// have been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteLong(long)"/>
    public long ReadLong()
    {
        if ( ReadToBuff( 8 ) <= 0 )
        {
            throw new EndOfStreamException();
        }

        var i1 = ( ( _buff[ 0 ] & 0xff ) << 24 )
                 | ( ( _buff[ 1 ] & 0xff ) << 16 )
                 | ( ( _buff[ 2 ] & 0xff ) << 8 )
                 | ( _buff[ 3 ] & 0xff );

        var i2 = ( ( _buff[ 4 ] & 0xff ) << 24 )
                 | ( ( _buff[ 5 ] & 0xff ) << 16 )
                 | ( ( _buff[ 6 ] & 0xff ) << 8 )
                 | ( _buff[ 7 ] & 0xff );

        return ( ( i1 & 0xffffffffL ) << 32 ) | ( i2 & 0xffffffffL );
    }

    /// <summary>
    /// Reads a 16-bit <see cref="short"/> value from this stream.
    /// <para/>
    /// </summary>
    /// <returns>The next <see cref="short"/> value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before two bytes
    /// have been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteShort(int)"/>
    public short ReadShort()
    {
        if ( ReadToBuff( 2 ) <= 0 )
        {
            throw new EndOfStreamException();
        }

        return ( short )( ( ( _buff[ 0 ] & 0xff ) << 8 ) | ( _buff[ 1 ] & 0xff ) );
    }

    /// <summary>
    /// Reads an unsigned 8-bit <see cref="byte"/> value from this stream and returns it as an
    /// <see cref="int"/>.
    /// <para/>
    /// </summary>
    /// <returns>The next unsigned <see cref="byte"/> value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before one byte
    /// has been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteByte(int)"/>
    public int ReadUnsignedByte()
    {
        var temp = _input.ReadByte();

        if ( temp < 0 )
        {
            throw new EndOfStreamException();
        }

        return temp;
    }

    /// <summary>
    /// Reads a 16-bit <see cref="ushort"/> value from this stream and returns it as an
    /// <see cref="int"/>.
    /// <para/>
    /// </summary>
    /// <returns>The next <see cref="ushort"/> value from the source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before two bytes
    /// have been read.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteShort(int)"/>
    public int ReadUnsignedShort()
    {
        if ( ReadToBuff( 2 ) <= 0 )
        {
            throw new EndOfStreamException();
        }

        return ( ushort )( ( ( _buff[ 0 ] & 0xff ) << 8 ) | ( _buff[ 1 ] & 0xff ) );
    }

    /// <summary>
    /// Reads an string encoded in DataInput modified UTF-8 from this
    /// stream.
    /// </summary>
    /// <returns>The next DataInput MUTF-8 encoded string read from the
    /// source stream.</returns>
    /// <exception cref="EndOfStreamException">If the end of the stream is reached before the read
    /// request can be satisfied.</exception>
    /// <exception cref="IOException">If a problem occurs while reading from this stream.</exception>
    /// <seealso cref="IDataOutput.WriteUTF(string)"/>
    public string ReadUTF()
    {
        return DecodeUTF( ReadUnsignedShort() );
    }

    private string DecodeUTF( int utfSize )
    {
        return DecodeUTF( utfSize, this );
    }

    private static string DecodeUTF( int utfSize, IDataInput @in )
    {
        var buf  = new byte[ utfSize ];
        var @out = new char[ utfSize ];
        @in.ReadFully( buf, 0, utfSize );

        return ConvertUTF8WithBuf( buf, @out, 0, utfSize );
    }

    /// <summary>
    /// Reads a string encoded in DataInput modified UTF-8 from the
    /// <see cref="IDataInput"/> stream <paramref name="input"/>.
    /// </summary>
    /// <param name="input">The input stream to read from.</param>
    /// <returns>The next DataInput modified UTF-8 encoded string from the source stream.</returns>
    /// <exception cref="IOException">If a problem occurs while reading from the stream.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="input"/> is <c>null</c>.</exception>
    /// <seealso cref="DataOutputStream.WriteUTF(string)"/>
    public static string ReadUTF( IDataInput input )
    {
        ArgumentNullException.ThrowIfNull( input );

        return DecodeUTF( input.ReadUnsignedShort(), input );
    }

    private static string ConvertUTF8WithBuf( byte[] buf, char[] output, int offset, int utfSize )
    {
        var count = 0;
        var s = 0;

        while ( count < utfSize )
        {
            if ( ( output[ s ] = ( char )buf[ offset + count++ ] ) < '\u0080' )
            {
                s++;
            }
            else
            {
                int a;

                if ( ( ( a = output[ s ] ) & 0xe0 ) == 0xc0 )
                {
                    if ( count >= utfSize )
                    {
                        throw new FormatException( $"Second byte at {count} does not match UTF8 Specification." );
                    }

                    int b = buf[ count++ ];

                    if ( ( b & 0xC0 ) != 0x80 )
                    {
                        throw new FormatException( $"Second byte at {count - 1} does not match UTF8 Specification." );
                    }

                    output[ s++ ] = ( char )( ( ( a & 0x1F ) << 6 ) | ( b & 0x3F ) );
                }
                else if ( ( a & 0xf0 ) == 0xe0 )
                {
                    if ( ( count + 1 ) >= utfSize )
                    {
                        throw new FormatException( $"Third byte at {count+1} does not match UTF8 Specification." );
                    }

                    int b = buf[ count++ ];
                    int c = buf[ count++ ];

                    if ( ( ( b & 0xC0 ) != 0x80 ) || ( ( c & 0xC0 ) != 0x80 ) )
                    {
                        throw new FormatException( $"Second or third byte at {count - 2} does not match UTF8 Specification." );
                    }

                    output[ s++ ] = ( char )( ( ( a & 0x0F ) << 12 ) | ( ( b & 0x3F ) << 6 ) | ( c & 0x3F ) );
                }
                else
                {
                    throw new FormatException( $"Input at {count - 1} does not match UTF8 Specification." );
                }
            }
        }

        return new string( output, 0, s );
    }

    /// <summary>
    /// Skips <paramref name="count"/> number of bytes in this stream. Subsequent 
    /// <see cref="Read(byte[])"/>s will not return these bytes.
    /// <para/>
    /// This method will not throw an <see cref="EndOfStreamException"/> if the end of the
    /// input is reached before <paramref name="count"/> bytes where skipped.
    /// </summary>
    /// <param name="count">The number of bytes to skip.</param>
    /// <returns>The number of bytes actually skipped.</returns>
    /// <exception cref="IOException">If a problem occurs while skipping.</exception>
    public int SkipBytes( int count )
    {
        var skipped = 0;
        int skip;

        while ( ( skipped < count ) && ( ( skip = Skip( _input, count - skipped ) ) > 0 ) )
        {
            skipped += skip;
        }

        if ( skipped < 0 )
        {
            throw new EndOfStreamException();
        }

        return skipped;
    }

    /// <summary>
    /// Helper method for SkipBytes, since Position and Seek do not work on
    /// non-seekable streams.
    /// </summary>
    private static int Skip( Stream stream, int n )
    {
        var total = 0;

        while ( ( total < n ) && ( stream.ReadByte() > -1 ) )
        {
            total++;
        }

        return total;
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="DataInputStream"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="DataInputStream"/> class and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( !_leaveOpen )
                _input.Dispose();
        }
    }

    #region From FilterInputStream

    /// <summary>
    /// Reads a single byte from the stream and returns it as an integer
    /// in the range from 0 to 255. Returns -1 if the end of this stream has been
    /// reached.
    /// </summary>
    /// <returns>The byte read or -1 if the end of the filtered stream has been reached.</returns>
    /// <exception cref="IOException">If the stream is disposed or another <see cref="IOException"/> occurs.</exception>
    public int Read() => _input.ReadByte();

    #endregion

}