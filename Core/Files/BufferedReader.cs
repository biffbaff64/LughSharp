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

using System.Text;

namespace LibGDXSharp.Files;

/// <summary>
/// Reads text from a character-input stream, buffering characters so as to
/// provide for the efficient reading of characters, arrays, and lines.
///
/// <p>
/// The buffer size may be specified, or the default size may be used. The
/// default is large enough for most purposes.
/// </p>
/// 
/// <p>
/// In general, each read request made of a Reader causes a corresponding
/// read request to be made of the underlying character or byte stream.  It is
/// therefore advisable to wrap a BufferedReader around any Reader whose read()
/// operations may be costly, such as FileReaders and InputStreamReaders.  For
/// example,
/// </p>
///
/// <code>
/// BufferedReader _reader = new BufferedReader(new FileReader("foo.in"));
/// </code>
///
/// <p>
/// will buffer the input from the specified file.  Without buffering, each
/// invocation of read() or readLine() could cause bytes to be read from the
/// file, converted into characters, and then returned, which can be very
/// inefficient.
/// </p>
///
/// <p>
/// Programs that use DataInputStreams for textual input can be localized by
/// replacing each DataInputStream with an appropriate BufferedReader.
/// </p>
/// </summary>
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class BufferedReader : Reader
{
    private const int DEFAULT_CHAR_BUFFER_SIZE     = 8192;
    private const int DEFAULT_EXPECTED_LINE_LENGTH = 80;
    private const int INVALIDATED               = -2;
    private const int UNMARKED                  = -1;

    private Reader? _reader;
    private char[]  _cb;
    private int     _nChars;
    private int     _nextChar;
    private int     _markedChar = UNMARKED;

    // Valid only when _markedChar > 0 
    private int _readAheadLimit = 0;

    // If the next character is a line feed, skip it
    private bool _skipLF = false;

    // The _skipLF flag when the mark was set 
    private bool _markedSkipLF = false;

    /// <summary>
    /// Creates a buffering character-input stream that uses an input buffer of
    /// the specified size.
    /// </summary>
    /// <param name="reader"> A Reader </param>
    /// <param name="sz"> Input-buffer size </param>
    /// <exception cref="ArgumentException"> If sz &lt;= 0 </exception>
    public BufferedReader( Reader reader, int sz = DEFAULT_CHAR_BUFFER_SIZE ) : base( reader )
    {
        if ( sz <= 0 ) throw new ArgumentException( "Buffer size <= 0" );

        this._reader = reader;

        _cb       = new char[ sz ];
        _nextChar = _nChars = 0;
    }

    /// <summary>
    /// Checks to make sure that the stream has not been closed
    /// </summary>
    private void EnsureOpen()
    {
        if ( _reader == null )
        {
            throw new IOException( "Stream closed" );
        }
    }

    /// <summary>
    /// Fills the input buffer, taking the mark into account if it is valid.
    /// </summary>
    private void Fill()
    {
        int dst;

        if ( _markedChar <= UNMARKED )
        {
            // No mark
            dst = 0;
        }
        else
        {
            // Marked
            var delta = _nextChar - _markedChar;

            if ( delta >= _readAheadLimit )
            {
                // Gone past read-ahead limit: Invalidate mark
                _markedChar     = INVALIDATED;
                _readAheadLimit = 0;
                dst             = 0;
            }
            else
            {
                if ( _readAheadLimit <= _cb.Length )
                {
                    // Shuffle in the current buffer 
                    Array.Copy( _cb, _markedChar, _cb, 0, delta );
                    _markedChar = 0;
                    dst         = delta;
                }
                else
                {
                    // Reallocate buffer to accommodate read-ahead limit
                    var ncb = new char[ _readAheadLimit ];

                    Array.Copy( _cb, _markedChar, ncb, 0, delta );
                    _cb         = ncb;
                    _markedChar = 0;
                    dst         = delta;
                }

                _nextChar = _nChars = delta;
            }
        }

        int n;

        do
        {
            n = _reader!.Read( _cb, dst, _cb.Length - dst );
        }
        while ( n == 0 );

        if ( n > 0 )
        {
            _nChars   = dst + n;
            _nextChar = dst;
        }
    }

    /// <summary>
    /// Reads a single character.
    /// </summary>
    /// <returns>
    /// The character read, as an integer in the range 0 to 65535 (<tt>0x00-0xffff</tt>),
    /// or -1 if the end of the stream has been reached
    /// </returns>
    /// <exception cref="IOException"> If an I/O error occurs </exception>
    public new int Read()
    {
        lock ( base.Lockobj )
        {
            EnsureOpen();

            for ( ;; )
            {
                if ( _nextChar >= _nChars )
                {
                    Fill();

                    if ( _nextChar >= _nChars )
                    {
                        return -1;
                    }
                }

                if ( _skipLF )
                {
                    _skipLF = false;

                    if ( _cb[ _nextChar ] == '\n' )
                    {
                        _nextChar++;

                        continue;
                    }
                }

                return _cb[ _nextChar++ ];
            }
        }
    }

    /// <summary>
    /// Reads characters into a portion of an array, reading from the underlying
    /// stream if necessary.
    /// </summary>
    private int Read1( char[] cbuf, int offset, int len )
    {
        if ( _nextChar >= _nChars )
        {
            // If the requested length is at least as large as the buffer, and
            // if there is no mark/reset activity, and if line feeds are not
            // being skipped, do not bother to copy the characters into the
            // local buffer.  In this way buffered streams will cascade
            // harmlessly.
            if ( ( len >= _cb.Length ) && ( _markedChar <= UNMARKED ) && !_skipLF )
            {
                return _reader!.Read( cbuf, offset, len );
            }

            Fill();
        }

        if ( _nextChar >= _nChars )
        {
            return -1;
        }

        if ( _skipLF )
        {
            _skipLF = false;

            if ( _cb[ _nextChar ] == '\n' )
            {
                _nextChar++;

                if ( _nextChar >= _nChars )
                {
                    Fill();
                }

                if ( _nextChar >= _nChars )
                {
                    return -1;
                }
            }
        }

        var n = Math.Min( len, _nChars - _nextChar );

        Array.Copy( _cb, _nextChar, cbuf, offset, n );
        _nextChar += n;

        return n;
    }

	/// <summary>
	/// Reads characters into a portion of an array.
	/// 
	/// <para>
	/// This method implements the general contract of the corresponding
	/// <tt><seealso cref="Reader.Read(char[], int, int)"/></tt> method of the
	/// <tt><seealso cref="Reader"/></tt> class. As an additional convenience, it
	/// attempts to read as many characters as possible by repeatedly invoking
	/// the <tt>read</tt> method of the underlying stream. This iterated
	/// <tt>read</tt> continues until one of the following conditions becomes
	/// true:
	/// </para>
	/// <ul>
	/// 
	///   <li> The specified number of characters have been read, </li>
	/// 
	///   <li> The <tt>read</tt> method of the underlying stream returns
	///   <tt>-1</tt>, indicating end-of-file, or
	///   </li>
	/// 
	///   <li>
	///   The <tt>ready</tt> method of the underlying stream
	///   returns <tt>false</tt>, indicating that further input requests
	///   would block.
	///   </li>
	/// 
	/// </ul>
	/// If the first <tt>read</tt> on the underlying stream returns
	/// <tt>-1</tt> to indicate end-of-file then this method returns
	/// <tt>-1</tt>.  Otherwise this method returns the number of characters
	/// actually read.
	/// 
	/// <para>
	/// Subclasses of this class are encouraged, but not required, to
	/// attempt to read as many characters as possible in the same fashion.
	/// </para>
	/// 
	/// <para>
	/// Ordinarily this method takes characters from this stream's character
	/// buffer, filling it from the underlying stream as necessary.  If,
	/// however, the buffer is empty, the mark is not valid, and the requested
	/// length is at least as large as the buffer, then this method will read
	/// characters directly from the underlying stream into the given array.
	/// Thus redundant <tt>BufferedReader</tt>s will not copy data
	/// unnecessarily.
	/// </para>
	/// </summary>
	/// <param name="cbuf"> Destination buffer </param>
	/// <param name="offset"> Offset at which to start storing characters </param>
	/// <param name="len"> Maximum number of characters to read
	/// </param>
	/// <returns>
	/// The number of characters read, or -1 if the end of the stream has been reached
	/// </returns>
	/// <exception cref="IOException"> If an I/O error occurs </exception>
    public override int Read( char[] cbuf, int offset, int len )
    {
        lock ( Lockobj )
        {
            EnsureOpen();

            if ( ( offset < 0 )
                 || ( offset > cbuf.Length )
                 || ( len < 0 )
                 || ( ( offset + len ) > cbuf.Length )
                 || ( ( offset + len ) < 0 ) )
            {
                throw new IndexOutOfRangeException();
            }
            else if ( len == 0 )
            {
                return 0;
            }

            var n = Read1( cbuf, offset, len );

            if ( n <= 0 ) return n;

            while ( ( n < len ) && _reader!.Ready() )
            {
                var n1 = Read1( cbuf, offset + n, len - n );

                if ( n1 <= 0 ) break;
                n += n1;
            }

            return n;
        }
    }

    /// <summary>
    /// Reads a line of text.  A line is considered to be terminated by any one
    /// of a line feed ('\n'), a carriage return ('\r'), or a carriage return
    /// followed immediately by a linefeed.
    /// </summary>
    ///
    /// <param name="ignoreLF"> If true, the next '\n' will be skipped </param>
    /// <returns>
    /// A string containing the contents of the line, not including
    /// any line-termination characters, or null if the end of the
    /// stream has been reached
    /// </returns>
    private string? ReadLine( bool ignoreLF = false )
    {
        StringBuilder? s = null;

        lock ( Lockobj )
        {
            EnsureOpen();

            var omitLF = ignoreLF || _skipLF;

            for ( ;; )
            {
                if ( _nextChar >= _nChars )
                {
                    Fill();
                }

                if ( _nextChar >= _nChars )
                {
                    // EOF
                    if ( s is { Length: > 0 } )
                    {
                        return s.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }

                var eol = false;
                var c   = default( char );
                int i;

                // Skip a leftover '\n', if necessary
                if ( omitLF && ( _cb[ _nextChar ] == '\n' ) )
                {
                    _nextChar++;
                }

                _skipLF = false;
                omitLF  = false;

                charLoop:

                for ( i = _nextChar; i < _nChars; i++ )
                {
                    c = _cb[ i ];

                    if ( ( c == '\n' ) || ( c == '\r' ) )
                    {
                        eol = true;

                        goto charLoop;
                    }
                }

                var startChar = _nextChar;
                _nextChar = i;

                if ( eol )
                {
                    string str;

                    if ( s == null )
                    {
                        str = new string( _cb, startChar, i - startChar );
                    }
                    else
                    {
                        s.Append( _cb, startChar, i - startChar );
                        str = s.ToString();
                    }

                    _nextChar++;

                    if ( c == '\r' )
                    {
                        _skipLF = true;
                    }

                    return str;
                }

                s ??= new StringBuilder( DEFAULT_EXPECTED_LINE_LENGTH );

                s.Append( _cb, startChar, i - startChar );
            }
        }
    }

    /// <summary>
    /// Skips characters.
    /// </summary>
    ///
    /// <param name="n"> The number of characters to skip </param>
    /// <returns> The number of characters actually skipped </returns>
    ///
    /// <exception cref="ArgumentException"> If <tt>n</tt> is negative. </exception>
    /// <exception cref="IOException"> If an I/O error occurs </exception>
    public new long Skip( long n )
    {
        if ( n < 0L )
        {
            throw new ArgumentException( "skip value is negative" );
        }

        lock ( Lockobj )
        {
            EnsureOpen();

            var r = n;

            while ( r > 0 )
            {
                if ( _nextChar >= _nChars ) Fill();

                if ( _nextChar >= _nChars ) /* EOF */
                    break;

                if ( _skipLF )
                {
                    _skipLF = false;

                    if ( _cb[ _nextChar ] == '\n' )
                    {
                        _nextChar++;
                    }
                }

                long d = _nChars - _nextChar;

                if ( r <= d )
                {
                    _nextChar += ( int )r;
                    r         =  0;

                    break;
                }
                else
                {
                    r         -= d;
                    _nextChar =  _nChars;
                }
            }

            return n - r;
        }
    }

    /// <summary>
    /// Tells whether this stream is ready to be read.  A buffered character
    /// stream is ready if the buffer is not empty, or if the underlying
    /// character stream is ready.
    /// </summary>
    public new bool Ready()
    {
        lock ( Lockobj )
        {
            EnsureOpen();

            // If newline needs to be skipped and the next char to be read
            // is a newline character, then just skip it right away.
            if ( _skipLF )
            {
                // Note that in.ready() will return true if and only if the next
                // read on the stream will not block.
                if ( ( _nextChar >= _nChars ) && _reader!.Ready() )
                {
                    Fill();
                }

                if ( _nextChar < _nChars )
                {
                    if ( _cb[ _nextChar ] == '\n' ) _nextChar++;

                    _skipLF = false;
                }
            }

            return ( _nextChar < _nChars ) || _reader!.Ready();
        }
    }

    /// <summary>
    /// Tells whether this stream supports the mark() operation, which it does.
    /// </summary>
    public new bool MarkSupported => true;

    /// <summary>
    /// Marks the present position in the stream.  Subsequent calls to reset()
    /// will attempt to reposition the stream to this point.
    /// </summary>
    /// <param name="readAheadLimit">
    /// Limit on the number of characters that may be read while still preserving
    /// the mark. An attempt to reset the stream after reading characters up to
    /// this limit or beyond may fail. A limit value larger than the size of the
    /// input buffer will cause a new buffer to be allocated whose size is no
    /// smaller than limit. Therefore large values should be used with care.
    /// </param>
    /// <exception cref="ArgumentException">  If readAheadLimit &lt; 0 </exception>
    public new void Mark( int readAheadLimit )
    {
        if ( readAheadLimit < 0 )
        {
            throw new ArgumentException( "Read-ahead limit < 0" );
        }

        lock ( Lockobj )
        {
            EnsureOpen();
            this._readAheadLimit = readAheadLimit;
            _markedChar          = _nextChar;
            _markedSkipLF        = _skipLF;
        }
    }

    /// <summary>
    /// Resets the stream to the most recent mark.
    /// </summary>
    /// <exception cref="IOException">
    /// If the stream has never been marked, or if the mark has been invalidated
    /// </exception>
    public new void Reset()
    {
        lock ( Lockobj )
        {
            EnsureOpen();

            if ( _markedChar < 0 )
            {
                throw new IOException
                    (
                    ( _markedChar == INVALIDATED )
                        ? "Mark invalid"
                        : "Stream not marked"
                    );
            }

            _nextChar = _markedChar;
            _skipLF   = _markedSkipLF;
        }
    }

    public new void Close()
    {
        lock ( Lockobj )
        {
            if ( _reader == null ) return;

            try
            {
                _reader.Close();
            }
            finally
            {
                _reader = null;
                _cb     = null!;
            }
        }
    }

    //TODO: Update this documentation to match the converted method.
    
    /// <summary>
    /// Returns a <b>Stream</b>, the elements of which are lines read from
    /// this BufferedReader. The <see cref="Stream"/> is lazily populated,
    /// i.e., read only occurs during the <tt>terminal stream operation</tt>.
    /// 
    /// <para>
    /// The reader must not be operated on during the execution of the terminal
    /// stream operation. Otherwise, the result of the terminal stream operation
    /// is undefined.
    /// </para>
    /// 
    /// <para>
    /// After execution of the terminal stream operation there are no guarantees
    /// that the reader will be at a specific position from which to read the next
    /// character or line.
    /// </para>
    /// 
    /// <para>
    /// If an <see cref="IOException"/> is thrown when accessing the underlying
    /// BufferedReader, it is wrapped in an <see cref="UncheckedIOException"/>
    /// which will be thrown from the Stream method that caused the read to take
    /// place. This method will return a Stream if invoked on a BufferedReader
    /// that is closed.
    /// </para>
    ///
    /// <para>
    /// Any operation on that stream that requires reading from the BufferedReader
    /// after it is closed, will cause an UncheckedIOException to be thrown.
    /// </para>
    /// 
    /// </summary>
    /// <returns>
    /// a {@code Stream<string>} providing the lines of text described by this {@code BufferedReader}
    public IEnumerable< string? >? Lines()
    {
        string? nextLine = null;

        bool HasNext()
        {
            if (nextLine != null)
            {
                return true;
            }
            else
            {
                try
                {
                    nextLine = ReadLine();
                    
                    return (nextLine != null);
                }
                catch (IOException e)
                {
//                    throw new UncheckedIOException( e );
                    throw new IOException();
                }
            }
        }

        string? Next()
        {
            if (( nextLine != null ) || HasNext())
            {
                var line = nextLine;
                
                nextLine = null;

                return line;
            }
            else
            {
                throw new NoSuchElementException();
            }
        }

        while (HasNext())
        {
            yield return Next();
        }
    }
}