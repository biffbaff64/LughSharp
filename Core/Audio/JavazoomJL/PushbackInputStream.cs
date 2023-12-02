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

using LibGDXSharp.Core.Files;

namespace LibGDXSharp.Core.Audio.JavazoomJL;

/// <summary>
/// A <tt>PushbackInputStream</tt> adds functionality to another input stream, namely
/// the  ability to "push back" or "unread" one byte. This is useful in situations where
/// it is  convenient for a fragment of code to read an indefinite number of data bytes
/// that  are delimited by a particular byte value; after reading the terminating byte,
/// the  code fragment can "unread" it, so that the next read operation on the input stream
/// will reread the byte that was pushed back.
/// <para>
/// For example, bytes representing the characters constituting an identifier might be
/// terminated by a byte representing an  operator character;
/// </para>
/// <para>
/// a method whose job is to read just an identifier can read until it  sees the operator and
/// then push the operator back to be re-read.
/// </para>
/// </summary>
[PublicAPI]
public class PushbackInputStream : FilterInputStream
{
    protected byte[]? pushbackBuffer;

    // The position within the pushback buffer from which the next byte will be read.
    // When the buffer is empty, <tt>pos</tt> is equal to <tt>buf.length</tt>; when the
    // buffer is full, <tt>pos</tt> is equal to zero.
    protected int nextPos;

    /// <summary>
    /// Creates a PushbackInputStream with a pushback buffer of the specified size,
    /// and saves its argument, the input stream <tt>inStream</tt>, for later use.
    /// </summary>
    /// <param name="inStream"> the input stream from which bytes will be read. </param>
    /// <param name="size"> the size of the pushback buffer. </param>
    /// <exception cref="ArgumentException"> if <tt>size &lt;= 0</tt> </exception>
    public PushbackInputStream( InputStream inStream, int size = 1 )
        : base( inStream )
    {
        if ( size <= 0 )
        {
            throw new ArgumentException( "size <= 0" );
        }

        this.pushbackBuffer = new byte[ size ];
        this.nextPos        = size;
    }

    /// <summary>
    /// Reads the next byte of data from this input stream. The value byte is returned
    /// as an <tt>int</tt> in the range <tt>0</tt> to <tt>255</tt>. If no byte is available
    /// because the end of the stream has been reached, the value <tt>-1</tt> is returned.
    /// This method blocks until input data is available, the end of the stream is detected,
    /// or an exception is thrown.
    /// <para>
    /// This method returns the most recently pushed-back byte, if there is one, and
    /// otherwise calls the <tt>read</tt> method of its underlying input stream and
    /// returns whatever value that method returns.
    /// </para>
    /// </summary>
    /// <returns>
    /// the next byte of data, or <tt>-1</tt> if the end of the stream has been reached.
    /// </returns>
    /// <exception cref="IOException">
    /// if this input stream has been closed by invoking its <see cref="Close()"/>"
    /// method, or an I/O error occurs.
    /// </exception>
    public override int Read()
    {
        EnsureOpen();

        if ( nextPos < pushbackBuffer?.Length )
        {
            return pushbackBuffer[ nextPos++ ] & 0xff;
        }

        return base.Read();
    }

    /// <summary>
    /// Reads up to <tt>len</tt> bytes of data from this input stream into an array of
    /// bytes.  This method first reads any pushed-back bytes; after that, if fewer than
    /// <tt>len</tt> bytes have been read then it reads from the underlying input stream.
    /// If <tt>len</tt> is not zero, the method blocks until at least 1 byte of input is
    /// available; otherwise, no bytes are read and <tt>0</tt> is returned.
    /// </summary>
    /// <param name="buffer"> the buffer into which the data is read. </param>
    /// <param name="offset"> the start offset in the destination array <tt>b</tt> </param>
    /// <param name="len"> the maximum number of bytes read. </param>
    /// <returns>
    /// the total number of bytes read into the buffer, or <tt>-1</tt> if there is no more
    /// data because the end of the stream has been reached.
    /// </returns>
    /// <exception cref="NullReferenceException"> If <tt>b</tt> is <tt>null</tt>. </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>offset</tt> is negative, <tt>len</tt> is negative, or <tt>len</tt> is greater
    /// than <tt>buffer.Length - offset</tt>
    /// </exception>
    /// <exception cref="IOException">
    /// if this input stream has been closed by invoking its <see cref="Close()"/> method,
    /// or an I/O error occurs.
    /// </exception>
    public override int Read( byte[]? buffer, int offset, int len )
    {
        EnsureOpen();

        if ( buffer == null )
        {
            throw new NullReferenceException();
        }
        else if ( ( offset < 0 ) || ( len < 0 ) || ( len > ( buffer.Length - offset ) ) )
        {
            throw new IndexOutOfRangeException();
        }
        else if ( len == 0 )
        {
            return 0;
        }

        var avail = pushbackBuffer!.Length - nextPos;

        if ( avail > 0 )
        {
            if ( len < avail )
            {
                avail = len;
            }

            Array.Copy( pushbackBuffer!, nextPos, buffer, offset, avail );

            nextPos += avail;
            offset  += avail;
            len     -= avail;
        }

        if ( len > 0 )
        {
            len = base.Read( buffer, offset, len );

            if ( len == -1 )
            {
                return avail == 0 ? -1 : avail;
            }

            return avail + len;
        }

        return avail;
    }

    /// <summary>
    /// Pushes back a byte by copying it to the front of the pushback buffer. After this
    /// method returns, the next byte to be read will have the value <tt>(byte)b</tt>.
    /// </summary>
    /// <param name="b">
    /// the <tt>int</tt> value whose low-order byte is to be pushed back. </param>
    /// <exception cref="IOException">
    /// If there is not enough room in the pushback buffer for the byte, or this input
    /// stream has been closed by invoking its <see cref="Close()"/>" method.
    /// </exception>
    public void Unread( int b )
    {
        EnsureOpen();

        if ( nextPos == 0 )
        {
            throw new IOException( "Push back buffer is full" );
        }

        pushbackBuffer![ --nextPos ] = ( byte )b;
    }

    /// <summary>
    /// Pushes back a portion of an array of bytes by copying it to the front of the
    /// pushback buffer.  After this method returns, the next byte to be read will have
    /// the value <tt>b[off]</tt>, the byte after that will have the value <tt>b[off+1]</tt>,
    /// and so forth.
    /// </summary>
    /// <param name="b"> the byte array to push back. </param>
    /// <param name="off"> the start offset of the data. </param>
    /// <param name="len"> the number of bytes to push back. </param>
    /// <exception cref="IOException">
    /// If there is not enough room in the pushback buffer for the specified number of bytes,
    /// or this input stream has been closed by invoking its <see cref="Close()"/> method.
    /// </exception>
    public void Unread( byte[] b, int off, int len )
    {
        EnsureOpen();

        if ( len > nextPos )
        {
            throw new IOException( "Push back buffer is full" );
        }

        nextPos -= len;

        Array.Copy( b, off, pushbackBuffer!, nextPos, len );
    }

    /// <summary>
    /// Pushes back an array of bytes by copying it to the front of the pushback buffer.
    /// After this method returns, the next byte to be read will have the value <tt>b[0]</tt>,
    /// the byte after that will have the value <tt>b[1]</tt>, and so forth.
    /// </summary>
    /// <param name="b"> the byte array to push back </param>
    /// <exception cref="IOException">
    /// If there is not enough room in the pushback buffer for the specified number of bytes,
    /// or this input stream has been closed by invoking its <see cref="Close()"/> method.
    /// </exception>
    public void Unread( byte[] b )
    {
        Unread( b, 0, b.Length );
    }

    /// <summary>
    /// Returns an estimate of the number of bytes that can be read (or skipped over) from
    /// this input stream without blocking by the next invocation of a method for this input
    /// stream. The next invocation might be the same thread or another thread. A single read
    /// or skip of this many bytes will not block, but may read or skip fewer bytes.
    /// <para>
    /// The method returns the sum of the number of bytes that have been pushed back and the
    /// value returned by <see cref="FilterInputStream.Available"/>".
    /// </para>
    /// </summary>
    /// <returns>
    /// the number of bytes that can be read (or skipped over) from the input stream without blocking.
    /// </returns>
    /// <exception cref="IOException">
    /// if this input stream has been closed by invoking its <see cref="Close()"/> method,
    /// or an I/O error occurs.
    /// </exception>
    public override int Available()
    {
        EnsureOpen();

        var n     = ( pushbackBuffer?.Length - nextPos );
        var avail = base.Available();

        return ( int )( n > ( int.MaxValue - avail ) ? int.MaxValue : n + avail )!;
    }

    /// <summary>
    /// Skips over and discards <tt>n</tt> bytes of data from this input stream. The
    /// <tt>skip</tt> method may, for a variety of reasons, end up skipping over some
    /// smaller number of bytes, possibly zero. If <tt>n</tt> is negative, no bytes are skipped.
    /// <para>
    /// The <tt>skip</tt> method of <tt>PushbackInputStream</tt> first skips over the bytes
    /// in the pushback buffer, if any. It then calls the <tt>skip</tt> method of the underlying
    /// input stream if more bytes need to be skipped. The actual number of bytes skipped
    /// is returned.
    /// </para>
    /// </summary>
    public override int Skip( int n )
    {
        EnsureOpen();

        if ( n <= 0 )
        {
            return 0;
        }

        var pskip = ( pushbackBuffer!.Length - nextPos );

        if ( pskip > 0 )
        {
            if ( n < pskip )
            {
                pskip = n;
            }

            nextPos += pskip;
            n       -= pskip;
        }

        if ( n > 0 )
        {
            pskip += base.Skip( n );
        }

        return pskip;
    }

    /// Tests if this input stream supports the <tt>mark</tt> and <tt>reset</tt> methods,
    /// which it does not.
    ///`<summary></summary>
    /// <returns>
    /// <tt>false</tt>, since this class does not support the <tt>mark</tt> and <tt>reset</tt> methods.
    /// </returns>
    public override bool MarkSupported()
    {
        return false;
    }

    /// <summary>
    /// Marks the current position in this input stream.
    /// <para>
    /// The <tt>mark</tt> method of <tt>PushbackInputStream</tt> does nothing.
    /// </para>
    /// </summary>
    /// <param name="readlimit">
    /// the maximum limit of bytes that can be read before the mark position becomes invalid.
    /// </param>
    public override void Mark( int readlimit )
    {
    }

    /// <summary>
    /// Repositions this stream to the position at the time the <tt>mark</tt> method
    /// was last called on this input stream.
    /// <para>
    /// The method <tt>reset</tt> for class <tt>PushbackInputStream</tt> does nothing
    /// except throw an <tt>IOException</tt>.
    /// </para>
    /// </summary>
    /// <exception cref="IOException"> if this method is invoked. </exception>
    public override void Reset()
    {
        throw new IOException( "mark/reset not supported" );
    }

    /// <summary>
    /// Closes this input stream and releases any system resources associated with the
    /// stream. Once the stream has been closed, further Read(), Unread(), Available(),
    /// Reset(), or Skip() invocations will throw an IOException.
    /// Closing a previously closed stream has no effect.
    /// </summary>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public override void Close()
    {
        if ( InputStream == null )
        {
            return;
        }

        InputStream.Close();
        InputStream    = null;
        pushbackBuffer = null;
    }

    /// <summary>
    /// Check to make sure that this stream has not been closed
    /// </summary>
    private void EnsureOpen()
    {
        if ( InputStream == null )
        {
            throw new IOException( "Stream closed" );
        }
    }
}
