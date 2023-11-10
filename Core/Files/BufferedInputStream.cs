///////////////////////////////////////////////////////////////////////////////
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
///////////////////////////////////////////////////////////////////////////////

using IndexOutOfRangeException = System.IndexOutOfRangeException;

namespace LibGDXSharp.Core.Files;

[PublicAPI]
public class BufferedInputStream : FilterInputStream
{
    private const int DEFAULT_BUFFER_SIZE = 8192;

    /// <summary>
    /// The maximum size of array to allocate. Some VMs reserve some header
    /// words in an array. Attempts to allocate larger arrays may result in
    /// an Out Of Memory Error.
    /// </summary>
    private const int MAX_BUFFER_SIZE = int.MaxValue - 8;

    /// <summary>
    /// The internal buffer array where the data is stored. When necessary,
    /// it may be replaced by another array of a different size.
    /// </summary>
    protected volatile byte[] buf;

    /// <summary>
    /// Atomic updater to provide compareAndSet for buf. This is necessary
    /// because closes can be asynchronous. We use nullness of buf[] as primary
    /// indicator that this stream is closed.
    /// </summary>
    //TODO: I don't know enough about C#/.Net to know if Atomic Reference classes are needed yet. Needs urgent resolution. 
    private static
        AtomicReferenceFieldUpdater< BufferedInputStream, byte[] > bufUpdater =
            AtomicReferenceFieldUpdater.NewUpdater( typeof( BufferedInputStream ), typeof( byte[] ), "buf" );

    /// <summary>
    /// The index one greater than the index of the last valid byte in the buffer.
    /// This value is always in the range <tt>0</tt> through <tt>buf.length</tt>;
    /// elements <tt>buf[0]</tt>  through <tt>buf[count-1]b</tt>contain buffered
    /// input data obtainedbfrom the underlying  input stream.
    /// </summary>
    protected int count;

    /// <summary>
    /// The current position in the buffer. This is the index of the next character
    /// to be read from the <tt>buf</tt> array.
    /// <para>
    /// This value is always in the range <tt>0</tt> through <tt>count</tt>. If it
    /// is less than <tt>count</tt>, then <tt>buf[pos]</tt> is the next byte to be
    /// supplied as input; if it is equal to <tt>count</tt>, then the next <tt>read</tt>
    /// or <tt>skip</tt> operation will require more bytes to be read from the contained
    /// input stream.
    /// </para>
    /// </summary>
    protected int pos;

    /// <summary>
    /// The value of the <tt>pos</tt> field at the time the last <tt>mark</tt> method
    /// was called.
    /// <para>
    /// This value is always in the range <tt>-1</tt> through <tt>pos</tt>. If there is
    /// no marked position in  the input stream, this field is <tt>-1</tt>. If there is
    /// a marked position in the input stream, then <tt>buf[markpos]</tt> is the first
    /// byte to be supplied as input after a <tt>reset</tt> operation. If <tt>markpos</tt>
    /// is not <tt>-1</tt>, then all bytes from positions <tt>buf[markpos]</tt> through
    /// <tt>buf[pos-1]</tt> must remain in the buffer array (though they may be moved to
    /// another place in the buffer array, with suitable adjustments to the values of
    /// <tt>count</tt>,  <tt>pos</tt>, and <tt>markpos</tt>); they may not be discarded
    /// unless and until the difference between <tt>pos</tt> and <tt>markpos</tt>
    /// exceeds <tt>marklimit</tt>.
    /// </para>
    /// </summary>
    protected int markpos = -1;

    /// <summary>
    /// The maximum read ahead allowed after a call to the <tt>mark</tt> method
    /// before subsequent calls to the <tt>reset</tt> method fail.
    /// Whenever the difference between <tt>pos</tt> and <tt>markpos</tt> exceeds
    /// <tt>marklimit</tt>, then the  mark may be dropped by setting <tt>markpos</tt>
    /// to <tt>-1</tt>.
    /// </summary>
    protected int marklimit;

    /// <summary>
    /// Creates a <tt>BufferedInputStream</tt> with the specified buffer size,
    /// and saves its  argument, the input stream <tt>in</tt>, for later use.
    /// An internal buffer array of length  <tt>size</tt> is created and stored
    /// in <tt>buf</tt>.
    /// </summary>
    /// <param name="inStream"> the underlying input stream. </param>
    /// <param name="size"> the buffer size. </param>
    /// <exception cref="ArgumentException"> if size &lt;= 0. </exception>
    public BufferedInputStream( InputStream? inStream, int size = DEFAULT_BUFFER_SIZE )
        : base( inStream )
    {
        if ( size <= 0 )
        {
            throw new ArgumentException( "Buffer size <= 0" );
        }

        buf = new byte[ size ];
    }

    /// <summary>
    /// Fills the buffer with more data, taking into account shuffling and other
    /// tricks for dealing with marks. Assumes that it is being called by a method.
    /// This method also assumes that all data has already been read in, hence
    /// pos > count.
    /// </summary>
    private void Fill()
    {
        var buffer = GetBufferIfOpen();

        if ( markpos < 0 )
        {
            pos = 0; // no mark: throw away the buffer
        }
        else if ( pos >= buffer.Length ) // no room left in buffer
        {
            if ( markpos > 0 )
            {
                // can throw away early part of the buffer
                var sz = pos - markpos;

                Array.Copy( buffer, markpos, buffer, 0, sz );

                pos     = sz;
                markpos = 0;
            }
            else if ( buffer.Length >= marklimit )
            {
                markpos = -1; // buffer got too big, invalidate mark
                pos     = 0;  // drop buffer contents
            }
            else if ( buffer.Length >= MAX_BUFFER_SIZE )
            {
                throw new OutOfMemoryException( "Required array size too large" );
            }
            else
            {
                // grow buffer
                var nsz = ( pos <= ( MAX_BUFFER_SIZE - pos ) ) ? pos * 2 : MAX_BUFFER_SIZE;

                if ( nsz > marklimit )
                {
                    nsz = marklimit;
                }

                var nbuf = new byte[ nsz ];

                Array.Copy( buffer, 0, nbuf, 0, pos );

                if ( !bufUpdater.CompareAndSet( this, buffer, nbuf ) )
                {
                    // Can't replace buf if there was an async close.
                    // Note: This would need to be changed if fill()
                    // is ever made accessible to multiple threads.
                    // But for now, the only way CAS can fail is via close.
                    // assert buf == null;
                    throw new IOException( "Stream closed" );
                }

                buffer = nbuf;
            }
        }

        count = pos;
        var n = GetInputStreamIfOpen().Read( buffer, pos, buffer.Length - pos );

        if ( n > 0 )
        {
            count = n + pos;
        }
    }

    /// <inheritdoc/>
    public override int Read()
    {
        if ( pos >= count )
        {
            Fill();

            if ( pos >= count )
            {
                return -1;
            }
        }

        return GetBufferIfOpen()[ pos++ ] & 0xff;
    }

    /// <summary>
    /// Read characters into a portion of an array, reading from the underlying
    /// stream at most once if necessary.
    /// </summary>
    private int Read1( byte[]? b, int off, int len )
    {
        var avail = count - pos;

        if ( avail <= 0 )
        {
            // If the requested length is at least as large as the buffer, and
            // if there is no mark/reset activity, do not bother to copy the
            // bytes into the local buffer.  In this way buffered streams will
            // cascade harmlessly.
            if ( ( len >= GetBufferIfOpen().Length ) && ( markpos < 0 ) )
            {
                return GetInputStreamIfOpen().Read( b, off, len );
            }

            Fill();
            
            avail = count - pos;

            if ( avail <= 0 )
            {
                return -1;
            }
        }

        var cnt = ( avail < len ) ? avail : len;

        Array.Copy( GetBufferIfOpen(), pos, b!, off, cnt );
        
        pos += cnt;

        return cnt;
    }

    /// <summary>
    /// Reads bytes from this byte-input stream into the specified byte array,
    /// starting at the given offset.
    /// <para>
    /// This method implements the general contract of the corresponding
    /// <see cref="InputStream.Read(byte[], int, int)"/> method of
    /// the <see cref="InputStream"/> class. As an additional convenience, it
    /// attempts to read as many bytes as possible by repeatedly invoking the
    /// <tt>read</tt> method of the underlying stream. This iterated <tt>read</tt>
    /// continues until one of the following conditions becomes true:
    /// </para>
    /// <para>
    ///   <li> The specified number of bytes have been read, </li>
    ///   <li>
    ///   The <tt>read</tt> method of the underlying stream returns
    ///   <tt>-1</tt>, indicating end-of-file, or
    ///   </li>
    ///   <li>
    ///   The <tt>available</tt> method of the underlying stream
    ///   returns zero, indicating that further input requests would block.
    ///   </li>
    /// </para>
    /// <para>
    /// If the first <tt>read</tt> on the underlying stream returns <tt>-1</tt> to
    /// indicate end-of-file then this method returns <tt>-1</tt>.  Otherwise this
    /// method returns the number of bytes actually read.
    /// </para>
    /// <para>
    /// Subclasses of this class are encouraged, but not required, to attempt to
    /// read as many bytes as possible in the same fashion.
    /// </para>
    /// </summary>
    /// <param name="b"> destination buffer. </param>
    /// <param name="off"> offset at which to start storing bytes. </param>
    /// <param name="len"> maximum number of bytes to read. </param>
    /// <returns>
    /// the number of bytes read, or <tt>-1</tt> if the end of the stream has been reached.
    /// </returns>
    /// <exception cref="IOException">
    /// if this input stream has been closed by invoking its {@link #close()} method,
    /// or an I/O error occurs.
    /// </exception>
    public override int Read( byte[]? b, int off, int len )
    {
        GetBufferIfOpen(); // Check for closed stream

        if ( ( off | len | ( off + len ) | ( b?.Length - ( off + len ) ) ) < 0 )
        {
            throw new IndexOutOfRangeException();
        }
        else if ( len == 0 )
        {
            return 0;
        }

        var n = 0;

        for ( ;; )
        {
            var nread = Read1( b, off + n, len - n );

            if ( nread <= 0 )
            {
                return ( n == 0 ) ? nread : n;
            }

            n += nread;

            if ( n >= len )
            {
                return n;
            }

            // if not closed but no bytes available, return
            if ( ( inputStream != null ) && ( inputStream.Available() <= 0 ) )
            {
                return n;
            }
        }
    }

    /// <summary>
    /// See the general contract of the <tt>skip</tt> method of <tt>InputStream</tt>.
    /// </summary>
    /// <exception cref="IOException">
    /// if the stream does not support seek, or if this input stream has been closed by
    /// invoking its {@link #close()} method, or an I/O error occurs.
    /// </exception>
    public override long Skip( long n )
    {
        GetBufferIfOpen(); // Check for closed stream

        if ( n <= 0 )
        {
            return 0;
        }

        var avail = count - pos;

        if ( avail <= 0 )
        {
            // If no mark position set then don't keep in buffer
            if ( markpos < 0 )
            {
                return GetInputStreamIfOpen().Skip( n );
            }

            // Fill in buffer to save bytes for reset
            Fill();

            avail = count - pos;

            if ( avail <= 0 )
            {
                return 0;
            }
        }

        var skipped = ( avail < n ) ? avail : ( int )n;

        pos += skipped;

        return skipped;
    }

    /// <summary>
    /// Returns an estimate of the number of bytes that can be read (or skipped over)
    /// from this input stream without blocking by the next invocation of a method for
    /// this input stream. The next invocation might be the same thread or another
    /// thread. A single read or skip of this many bytes will not block, but may read
    /// or skip fewer bytes.
    /// <para>
    /// This method returns the sum of the number of bytes remaining to be read in
    /// the buffer (<tt>count - pos</tt>) and the result of calling the
    /// FilterInputStream.inputStream.Available().
    /// </para>
    /// </summary>
    /// <returns>
    /// an estimate of the number of bytes that can be read (or skipped over) from this
    /// input stream without blocking.
    /// </returns>
    /// <exception cref="IOException">
    /// if this input stream has been closed by invoking its <see cref="Close()"/> method,
    /// or an I/O error occurs.
    /// </exception>
    public override int Available()
    {
        var n     = count - pos;
        var avail = GetInputStreamIfOpen().Available();

        return n > ( int.MaxValue - avail ) ? int.MaxValue : n + avail;
    }

    /// <summary>
    /// See the general contract of the <tt>mark</tt> method of <tt>InputStream</tt>.
    /// </summary>
    /// <param name="readlimit">
    /// the maximum limit of bytes that can be read before the mark position becomes invalid.
    /// </param>
    public override void Mark( int readlimit )
    {
        marklimit = readlimit;
        markpos   = pos;
    }

    /// <summary>
    /// See the general contract of the <tt>reset</tt> method of <tt>InputStream</tt>.
    /// <para>
    /// If <tt>markpos</tt> is <tt>-1</tt> (no mark has been set or the mark has been
    /// invalidated), an <tt>IOException</tt> is thrown. Otherwise, <tt>pos</tt> is
    /// set equal to <tt>markpos</tt>.
    /// </para>
    /// </summary>
    /// <exception cref="IOException">
    /// if this stream has not been marked or, if the mark has been invalidated, or
    /// the stream has been closed by invoking its <see cref="Close()"/> method, or
    /// an I/O error occurs.
    /// </exception>
    public override void Reset()
    {
        GetBufferIfOpen(); // Cause exception if closed

        if ( markpos < 0 )
        {
            throw new IOException( "Resetting to invalid mark" );
        }

        pos = markpos;
    }

    /// <summary>
    /// Tests if this input stream supports the <tt>mark</tt> and <tt>reset</tt>
    /// methods. The <tt>markSupported</tt> method of <tt>BufferedInputStream</tt>
    /// returns <tt>true</tt>.
    /// </summary>
    /// <returns>
    /// a <tt>bool</tt> indicating if this stream type supports the <tt>mark</tt>
    /// and <tt>reset</tt> methods.
    /// </returns>
    public override bool MarkSupported()
    {
        return true;
    }

    /// <summary>
    /// Closes this input stream and releases any system resources associated with
    /// the stream. Once the stream has been closed, further read(), available(),
    /// reset(), or skip() invocations will throw an IOException.
    /// Closing a previously closed stream has no effect.
    /// </summary>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public override void Close()
    {
        while ( buf is { } buffer )
        {
            if ( bufUpdater.CompareAndSet( this, buffer, null ) )
            {
                InputStream? input = inputStream;
                inputStream = null;

                if ( input != null )
                {
                    input.Close();
                }

                return;
            }
        }
    }

    /// <summary>
    /// Check to make sure that underlying input stream has not been
    /// nulled out due to close; if not return it;
    /// </summary>
    private InputStream GetInputStreamIfOpen()
    {
        InputStream? input = inputStream;

        if ( input == null )
        {
            throw new IOException( "Stream closed" );
        }

        return input;
    }

    /// <summary>
    /// Check to make sure that buffer has not been nulled out due to
    /// close; if not return it;
    /// </summary>
    private byte[] GetBufferIfOpen()
    {
        var buffer = buf;

        if ( buffer == null )
        {
            throw new IOException( "Stream closed" );
        }

        return buffer;
    }
}
