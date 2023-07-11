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

using LibGDXSharp.Utils.Buffers;

namespace LibGDXSharp.Files;

/// <summary>
/// Abstract class for reading character streams.
/// <para>
/// The only methods that a subclass must implement are <b>Read(char[], int, int)</b> and
/// <b>Close()</b>. Most subclasses, however, will override some of the methods defined
/// here in order to provide higher efficiency, additional functionality, or both.
/// </para>
/// </summary>
public abstract class Reader : ICloseable
{
    /// <summary>
    /// The object used to synchronize operations on this stream. For efficiency, a
    /// character-stream object may use an object other than itself to protect critical
    /// sections. A subclass should therefore use the object in this field rather than
    /// <tt>this</tt> or a synchronized method.
    /// </summary>
    protected object Lockobj { get; set; }
        
    /// <summary>
    /// Creates a new character-stream reader whose critical sections will
    /// synchronize on the reader itself.
    /// </summary>
    protected Reader()
    {
        this.Lockobj = this;
    }

    /// <summary>
    /// Creates a new character-stream reader whose critical sections will
    /// synchronize on the given object.
    /// </summary>
    /// <param name="lockobj"> The Object to synchronize on. </param>
    protected Reader( object? lockobj )
    {
        this.Lockobj = lockobj ?? throw new NullReferenceException();
    }

    /// <summary>
    /// Attempts to read characters into the specified character buffer.
    /// The buffer is used as a repository of characters as-is: the only
    /// changes made are the results of a put operation. No flipping or
    /// rewinding of the buffer is performed.
    /// </summary>
    /// <param name="target"> the buffer to read characters into </param>
    /// <returns>
    /// The number of characters added to the buffer, or -1 if this source
    /// of characters is at its end
    /// </returns>
    /// <exception cref="IOException"> if an I/O error occurs </exception>
    /// <exception cref="NullReferenceException"> if target is null </exception>
    /// <exception cref="ReadOnlyBufferException"> if target is a read only buffer </exception>
    public int Read( CharBuffer target )
    {
        var len  = target.Remaining();
        var cbuf = new char[ len ];
        var n    = Read( cbuf, 0, len );

        if ( n > 0 ) target.Put( cbuf, 0, n );

        return n;
    }

    /// <summary>
    /// Reads a single character.  This method will block until a character is
    /// available, an I/O error occurs, or the end of the stream is reached.
    /// Subclasses that intend to support efficient single-character input
    /// should override this method.
    /// </summary>
    /// <returns>
    /// The character read, as an integer in the range 0 to 65535 (<i>0x00-0xffff</i>),
    /// or -1 if the end of the stream has been reached
    /// </returns>
    /// <exception cref="IOException"> If an I/O error occurs </exception>
    public int Read()
    {
        var cb = new char[ 1 ];

        if ( Read( cb, 0, 1 ) == -1 )
        {
            return -1;
        }

        return cb[ 0 ];
    }

    /// <summary>
    /// Reads characters into an array.  This method will block until some input
    /// is available, an I/O error occurs, or the end of the stream is reached.
    /// </summary>
    /// <param name="cbuf"> Destination buffer </param>
    /// <returns>
    /// The number of characters read, or -1 if the end of the stream has been reached
    /// </returns>
    /// <exception cref="IOException"> If an I/O error occurs </exception>
    public int Read( char[] cbuf )
    {
        return Read( cbuf, 0, cbuf.Length );
    }

    /// <summary>
    /// Reads characters into a portion of an array.  This method will block
    /// until some input is available, an I/O error occurs, or the end of the
    /// stream is reached.
    /// </summary>
    /// <param name="cbuf"> Destination buffer </param>
    /// <param name="off"> Offset at which to start storing characters </param>
    /// <param name="len"> Maximum number of characters to read </param>
    ///
    /// <returns>
    /// The number of characters read, or -1 if the end of the stream has been reached
    /// </returns>
    /// <exception cref="IOException"> If an I/O error occurs </exception>
    protected abstract int Read( char[] cbuf, int off, int len );

    private const int     MaxSkipBufferSize = 8192;
    private       char[]? _skipBuffer       = null;

    /// <summary>
    /// Skips characters.  This method will block until some characters are
    /// available, an I/O error occurs, or the end of the stream is reached.
    /// </summary>
    /// <param name="n"> The number of characters to skip </param>
    /// <returns> The number of characters actually skipped </returns>
    /// <exception cref="ArgumentException"> If <b>n</b> is negative. </exception>
    /// <exception cref="IOException"> If an I/O error occurs </exception>
    public long Skip( long n )
    {
        if ( n < 0L ) throw new ArgumentException( "skip value is negative" );

        var nn = ( int )Math.Min( n, MaxSkipBufferSize );

        lock ( Lockobj )

        {
            if ( ( _skipBuffer == null ) || ( _skipBuffer.Length < nn ) )
            {
                _skipBuffer = new char[ nn ];
            }

            var r = n;

            while ( r > 0 )
            {
                var nc = Read( _skipBuffer, 0, ( int )Math.Min( r, nn ) );

                if ( nc == -1 ) break;

                r -= nc;
            }

            return n - r;
        }
    }

    /// <summary>
    /// Tells whether this stream is ready to be read.
    /// </summary>
    /// <returns>
    /// True if the next read() is guaranteed not to block for input, false
    /// otherwise.  Note that returning false does not guarantee that the
    /// next read will block.
    /// </returns>
    /// <exception cref="IOException"> If an I/O error occurs </exception>
    public bool Ready()
    {
        return false;
    }

    /// <summary>
    /// Tells whether this stream supports the mark() operation. The default
    /// implementation always returns false. Subclasses should override this method.
    /// </summary>
    /// <returns>true if and only if this stream supports the mark operation.</returns>
    public bool MarkSupported()
    {
        return false;
    }

    /// <summary>
    /// Marks the present position in the stream. Subsequent calls to reset()
    /// will attempt to reposition the stream to this point. Not all
    /// character-input streams support the mark() operation.
    /// </summary>
    /// <param name="readAheadLimit">
    /// Limit on the number of characters that may be read while still preserving
    /// the mark. After reading this many characters, attempting to reset the stream
    /// may fail.
    /// </param>
    /// <exception cref="IOException">
    /// If the stream does not support mark(), or if some other I/O error occurs
    /// </exception>
    public void Mark( int readAheadLimit )
    {
        throw new IOException( "mark() not supported" );
    }

    /// <summary>
    /// Resets the stream.  If the stream has been marked, then attempt to
    /// reposition it at the mark.  If the stream has not been marked, then
    /// attempt to reset it in some way appropriate to the particular stream,
    /// for example by repositioning it to its starting point.  Not all
    /// character-input streams support the reset() operation, and some support
    /// reset() without supporting mark().
    /// </summary>
    /// <exception cref="IOException">
    /// If the stream has not been marked, if the mark has been invalidated,
    /// if the stream does not support reset(), or if some other I/O error occurs
    /// </exception>
    public void Reset()
    {
        throw new IOException( "reset() not supported" );
    }

    /// <summary>
    /// Closes the stream and releases any system resources associated with
    /// it.  Once the stream has been closed, further read(), ready(),
    /// mark(), reset(), or skip() invocations will throw an IOException.
    /// Closing a previously closed stream has no effect.
    /// </summary>
    public void Close()
    {
    }
}