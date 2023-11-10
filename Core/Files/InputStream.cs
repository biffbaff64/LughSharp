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

namespace LibGDXSharp.Core.Files;

/// <summary>
/// This abstract class is the superclass of all classes representing
/// an input stream of bytes.
/// <para>
/// Applications that need to define a subclass of <tt>InputStream</tt>
/// must always provide a method that returns the next byte of input.
/// </para>
/// </summary>
[PublicAPI]
public abstract class InputStream : ICloseable
{
    // MAX_SKIP_BUFFER_SIZE is used to determine the maximum
    // buffer size to use when skipping.
    private const int MAX_SKIP_BUFFER_SIZE = 2048;

    /// <summary>
    /// Reads the next byte of data from the input stream. The value byte is
    /// returned as an <tt>int</tt> in the range <tt>0</tt> to
    /// <tt>255</tt>. If no byte is available because the end of the stream
    /// has been reached, the value <tt>-1</tt> is returned. This method
    /// blocks until input data is available, the end of the stream is detected,
    /// or an exception is thrown.
    /// <para> A subclass must provide an implementation of this method. </para>
    /// </summary>
    /// <returns>
    /// the next byte of data, or <tt>-1</tt> if the end of the stream is reached.
    /// </returns>
    public abstract int Read();

    /// <summary>
    /// Reads some number of bytes from the input stream and stores them into
    /// the buffer array <tt>b</tt>. The number of bytes actually read is
    /// returned as an integer.  This method blocks until input data is
    /// available, end of file is detected, or an exception is thrown.
    /// <para>
    /// If the length of <tt>b</tt> is zero, then no bytes are read and
    /// <tt>0</tt> is returned; otherwise, there is an attempt to read at
    /// least one byte. If no byte is available because the stream is at the
    /// end of the file, the value <tt>-1</tt> is returned; otherwise, at
    /// least one byte is read and stored into <tt>b</tt>.
    /// </para>
    /// <para>
    /// The first byte read is stored into element <tt>b[0]</tt>, the
    /// next one into <tt>b[1]</tt>, and so on. The number of bytes read is,
    /// at most, equal to the length of <tt>b</tt>. Let <i>k</i> be the
    /// number of bytes actually read; these bytes will be stored in elements
    /// <tt>b[0]</tt> through <tt>b[</tt><i>k</i><tt>-1]</tt>, leaving elements
    /// <tt>b[</tt><i>k</i><tt>]</tt> through <tt>b[b.length-1]</tt> unaffected.
    /// </para>
    /// <para>
    /// The <tt>read(b)</tt> method for class <tt>InputStream</tt>
    /// has the same effect as: <tt> read(b, 0, b.length) </tt>
    /// </para>
    /// </summary>
    /// <param name="b"> the buffer into which the data is read. </param>
    /// <returns>
    /// the total number of bytes read into the buffer, or <tt>-1</tt> if there
    /// is no more data because the end of the stream has been reached.
    /// </returns>
    /// <exception cred="IOException">
    /// If the first byte cannot be read for any reason other than the end of the
    /// file, if the input stream has been closed, or if some other I/O error occurs.
    /// </exception>
    public virtual int Read( ref byte[] b )
    {
        return Read( b, 0, b.Length );
    }

    /// <summary>
    /// Reads up to <tt>len</tt> bytes of data from the input stream into
    /// an array of bytes.  An attempt is made to read as many as
    /// <tt>len</tt> bytes, but a smaller number may be read.
    /// The number of bytes actually read is returned as an integer.
    /// <para> This method blocks until input data is available, end of file is
    /// detected, or an exception is thrown.
    /// </para>
    /// <para> If <tt>len</tt> is zero, then no bytes are read and
    /// <tt>0</tt> is returned; otherwise, there is an attempt to read at
    /// least one byte. If no byte is available because the stream is at end of
    /// file, the value <tt>-1</tt> is returned; otherwise, at least one
    /// byte is read and stored into <tt>b</tt>.
    /// </para>
    /// <para> The first byte read is stored into element <tt>b[off]</tt>, the
    /// next one into <tt>b[off+1]</tt>, and so on. The number of bytes read
    /// is, at most, equal to <tt>len</tt>. Let <i>k</i> be the number of
    /// bytes actually read; these bytes will be stored in elements
    /// <tt>b[off]</tt> through <tt>b[off+</tt><i>k</i><tt>-1]</tt>,
    /// leaving elements <tt>b[off+</tt><i>k</i><tt>]</tt> through
    /// <tt>b[off+len-1]</tt> unaffected.
    /// </para>
    /// <para> In every case, elements <tt>b[0]</tt> through
    /// <tt>b[off]</tt> and elements <tt>b[off+len]</tt> through
    /// <tt>b[b.length-1]</tt> are unaffected.
    /// </para>
    /// <para>
    /// The <tt>read(b,</tt> <tt>off,</tt> <tt>len)</tt> method
    /// for class <tt>InputStream</tt> simply calls the method
    /// <tt>read()</tt> repeatedly. If the first such call results in an
    /// <tt>IOException</tt>, that exception is returned from the call to
    /// the <tt>read(b,</tt> <tt>off,</tt> <tt>len)</tt> method.  If
    /// any subsequent call to <tt>read()</tt> results in a
    /// <tt>IOException</tt>, the exception is caught and treated as if it
    /// were end of file; the bytes read up to that point are stored into
    /// <tt>b</tt> and the number of bytes read before the exception
    /// occurred is returned. The default implementation of this method blocks
    /// until the requested amount of input data <tt>len</tt> has been read,
    /// end of file is detected, or an exception is thrown. Subclasses are encouraged
    /// to provide a more efficient implementation of this method.
    /// </para>
    /// <param name="b"> the buffer into which the data is read. </param>
    /// <param name="off">
    /// the start offset in array <tt>b</tt> at which the data is written.
    /// </param>
    /// <param name="len"> the maximum number of bytes to read. </param>
    /// </summary>
    /// <returns>
    /// the total number of bytes read into the buffer, or <tt>-1</tt> if there is
    /// no more data because the end of the stream has been reached.
    /// </returns>
    /// <exception cref="IOException">
    /// If the first byte cannot be read for any reason other than end of file, or
    /// if the input stream has been closed, or if some other I/O error occurs.
    /// </exception>
    /// <exception cref="NullReferenceException">If <tt>b</tt> is <tt>null</tt>.</exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>off</tt> is negative, <tt>len</tt> is negative, or <tt>len</tt> is
    /// greater than <tt>b.length - off</tt>
    /// </exception>
    public virtual int Read( byte[]? b, int off, int len )
    {
        ArgumentNullException.ThrowIfNull( b );

        if ( off < 0 || len < 0 || len > b.Length - off )
        {
            throw new IndexOutOfRangeException();
        }
        else if ( len == 0 )
        {
            return 0;
        }

        var c = Read();

        if ( c == -1 )
        {
            return -1;
        }

        b[ off ] = ( byte )c;

        var i = 1;

        try
        {
            for ( ; i < len; i++ )
            {
                c = Read();

                if ( c == -1 )
                {
                    break;
                }

                b[ off + i ] = ( byte )c;
            }
        }
        catch ( IOException )
        {
            // ignored
        }

        return i;
    }

    /// <summary>
    /// Skips over and discards <tt>n</tt> bytes of data from this input
    /// stream. The <tt>skip</tt> method may, for a variety of reasons, end
    /// up skipping over some smaller number of bytes, possibly <tt>0</tt>.
    /// This may result from any of a number of conditions; reaching end of file
    /// before <tt>n</tt> bytes have been skipped is only one possibility.
    /// The actual number of bytes skipped is returned. If {@code n} is
    /// negative, the {@code skip} method for class {@code InputStream} always
    /// returns 0, and no bytes are skipped. Subclasses may handle the negative
    /// value differently.
    /// <para>
    /// The <tt>skip</tt> method of this class creates a byte array and then
    /// repeatedly reads into it until <tt>n</tt> bytes have been read or the end
    /// of the stream has been reached. Subclasses are encouraged to provide a more
    /// efficient implementation of this method. For instance, the implementation
    /// may depend on the ability to seek.
    /// </para>
    /// <param name="n"> the number of bytes to be skipped. </param>
    /// </summary>
    /// <returns> the actual number of bytes skipped. </returns>
    /// <exception cref="IOException">
    /// if the stream does not support seek, or if some other I/O error occurs.
    /// </exception>
    public virtual long Skip( long n )
    {
        var remaining = n;

        if ( n <= 0 )
        {
            return 0;
        }

        var    size       = ( int )Math.Min( MAX_SKIP_BUFFER_SIZE, remaining );
        var skipBuffer = new byte[ size ];

        while ( remaining > 0 )
        {
            var nr = Read( skipBuffer, 0, ( int )Math.Min( size, remaining ) );

            if ( nr < 0 )
            {
                break;
            }

            remaining -= nr;
        }

        return n - remaining;
    }

    /// <summary>
    /// Returns an estimate of the number of bytes that can be read (or
    /// skipped over) from this input stream without blocking by the next
    /// invocation of a method for this input stream. The next invocation
    /// might be the same thread or another thread.  A single read or skip of this
    /// many bytes will not block, but may read or skip fewer bytes.
    /// <para>
    /// Note that while some implementations of {@code InputStream} will return
    /// the total number of bytes in the stream, many will not.  It is
    /// never correct to use the return value of this method to allocate
    /// a buffer intended to hold all data in this stream.
    /// </para>
    /// <para>
    /// A subclass' implementation of this method may choose to throw an
    /// <see cref="IOException"/> if this input stream has been closed by
    /// invoking the <see cref="Close()"/>" method.
    /// </para>
    /// <para>
    /// The {@code available} method for class {@code InputStream} always
    /// returns {@code 0}.
    /// </para>
    /// <para> This method should be overridden by subclasses. </para>
    /// </summary>
    /// <returns>
    /// an estimate of the number of bytes that can be read (or skipped
    /// over) from this input stream without blocking or 0 when it reaches
    /// the end of the input stream.
    /// </returns>
    public virtual int Available()
    {
        return 0;
    }

    /// <summary>
    /// Closes this input stream and releases any system resources associated
    /// with the stream.
    /// <para>
    /// The <tt>close</tt> method of <tt>InputStream</tt> does nothing.
    /// </para>
    /// </summary>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public virtual void Close()
    {
    }

    /// <summary>
    /// Marks the current position in this input stream. A subsequent call to
    /// the <tt>reset</tt> method repositions this stream at the last marked
    /// position so that subsequent reads re-read the same bytes.
    /// <para>
    /// The <tt>readlimit</tt> arguments tells this input stream to allow that
    /// many bytes to be read before the mark position gets invalidated.
    /// </para>
    /// <para>
    /// The general contract of <tt>mark</tt> is that, if the method <tt>markSupported</tt>
    /// returns <tt>true</tt>, the stream somehow remembers all the bytes read after
    /// the call to <tt>mark</tt> and stands ready to supply those same bytes again
    /// if and whenever the method <tt>reset</tt> is called.  However, the stream is
    /// not required to remember any data at all if more than <tt>readlimit</tt> bytes
    /// are read from the stream before <tt>reset</tt> is called.
    /// </para>
    /// <para> Marking a closed stream should not have any effect on the stream. </para>
    /// <para> The <tt>mark</tt> method of <tt>InputStream</tt> does nothing. </para>
    /// </summary>
    /// <param name="readlimit">
    /// the maximum limit of bytes that can be read before the mark position becomes invalid.
    /// </param>
    public virtual void Mark( int readlimit )
    {
    }

    /// <summary>
    /// Repositions this stream to the position at the time the <tt>mark</tt>
    /// method was last called on this input stream.
    /// <para>
    /// The general contract of <tt>reset</tt> is:
    /// <li>
    /// If the method <tt>markSupported</tt> returns <tt>true</tt>, then:
    /// <li>
    /// If the method <tt>mark</tt> has not been called since
    /// the stream was created, or the number of bytes read from the stream
    /// since <tt>mark</tt> was last called is larger than the argument
    /// to <tt>mark</tt> at that last call, then an
    /// <tt>IOException</tt> might be thrown.
    /// </li>
    /// <li>
    /// If such an <tt>IOException</tt> is not thrown, then the
    /// stream is reset to a state such that all the bytes read since the
    /// most recent call to <tt>mark</tt> (or since the start of the
    /// file, if <tt>mark</tt> has not been called) will be resupplied
    /// to subsequent callers of the <tt>read</tt> method, followed by
    /// any bytes that otherwise would have been the next input data as of
    /// the time of the call to <tt>reset</tt>.
    /// </li>
    /// <li> If the method <tt>markSupported</tt> returns <tt>false</tt>, then: </li>
    /// <li> The call to <tt>reset</tt> may throw an <tt>IOException</tt>. </li>
    /// <li>
    /// If an <tt>IOException</tt> is not thrown, then the stream
    /// is reset to a fixed state that depends on the particular type of the
    /// input stream and how it was created. The bytes that will be supplied
    /// to subsequent callers of the <tt>read</tt> method depend on the
    /// particular type of the input stream.
    /// </li>
    /// </li>
    /// </para>
    /// <para>
    /// The method <tt>reset</tt> for class <tt>InputStream</tt> does nothing except
    /// throw an <tt>IOException</tt>.
    /// </para>
    /// </summary>
    /// <exception cref="IOException">
    /// if this stream has not been marked or if the mark has been invalidated.
    /// </exception>
    public virtual void Reset()
    {
        throw new IOException( "mark/reset not supported" );

    }

    /// <summary>
    /// Tests if this input stream supports the <tt>mark</tt> and <tt>reset</tt>
    /// methods. Whether or not <tt>mark</tt> and <tt>reset</tt> are supported is
    /// an invariant property of a particular input stream instance.
    /// The <tt>markSupported</tt> method of <tt>InputStream</tt> returns <tt>false</tt>.
    /// </summary>
    /// <returns>
    /// <tt>true</tt> if this stream instance supports the mark and reset methods;
    /// <tt>false</tt> otherwise.
    /// </returns>
    public virtual bool MarkSupported()
    {
        return false;
    }
}
