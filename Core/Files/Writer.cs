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

namespace LibGDXSharp.Core.Files;

/// <summary>
/// Abstract class for writing to character streams.  The only methods that a
/// subclass must implement are write(char[], int, int), flush(), and close().
/// Most subclasses, however, will override some of the methods defined here in
/// order to provide higher efficiency, additional functionality, or both.
/// </summary>
public abstract class Writer : ICloseable
{
    /// <summary>
    /// Temporary buffer used to hold writes of strings and single characters
    /// </summary>
    private char[]? _writeBuffer;

    /// <summary>
    /// Size of writeBuffer, must be >= 1
    /// </summary>
    private const int WriteBufferSize = 1024;

    /// <summary>
    /// The object used to synchronize operations on this stream.  For
    /// efficiency, a character-stream object may use an object other than
    /// itself to protect critical sections.  A subclass should therefore use
    /// the object in this field rather than <tt>this</tt> or a synchronized
    /// method.
    /// </summary>
    protected readonly object lockObject;

    /// <summary>
    /// Creates a new character-stream writer whose critical sections will
    /// synchronize on the writer itself.
    /// </summary>
    protected Writer()
    {
        this.lockObject = this;
    }

    /// <summary>
    /// Creates a new character-stream writer whose critical sections will
    /// synchronize on the given object.
    /// </summary>
    /// <param name="lockobj"> Object to synchronize on </param>
    protected Writer( object lockobj )
    {
        ArgumentNullException.ThrowIfNull( lockobj );

        this.lockObject = lockobj;
    }

    /// <summary>
    /// Writes a single character.  The character to be written is contained in
    /// the 16 low-order bits of the given integer value; the 16 high-order bits
    /// are ignored.
    /// <para>
    /// Subclasses that intend to support efficient single-character output
    /// should override this method.
    /// </para>
    /// </summary>
    /// <param name="c"> int specifying a character to be written </param>
    public void Write( int c )
    {
        lock ( lockObject )
        {
            _writeBuffer ??= new char[ WriteBufferSize ];

            _writeBuffer[ 0 ] = ( char )c;
            Write( _writeBuffer, 0, 1 );
        }
    }

    /// <summary>
    /// Writes an array of characters.
    /// </summary>
    /// <param name="charbuf"> Array of characters to be written </param>
    public void Write( char[] charbuf ) => Write( charbuf, 0, charbuf.Length );

    /// <summary>
    /// Writes a portion of an array of characters.
    /// </summary>
    /// <param name="cbuf">Array of characters</param>
    /// <param name="off">Offset from which to start writing characters</param>
    /// <param name="len">Number of characters to write</param>
    public abstract void Write( char[] cbuf, int off, int len );

    /// <summary>
    /// Writes a string.
    /// </summary>
    /// <param name="str"> String to be written </param>
    public void Write( string str )
    {
        Write( str, 0, str.Length );
    }

    /// <summary>
    /// Writes a portion of a string.
    /// </summary>
    /// <param name="str"> A String </param>
    /// <param name="off"> Offset from which to start writing characters </param>
    /// <param name="len"> Number of characters to write </param>
    /// <exception cref="IndexOutOfRangeException">
    ///          If <tt>off</tt> is negative, or <tt>len</tt> is negative,
    ///          or <tt>off+len</tt> is negative or greater than the length
    ///          of the given string
    /// </exception>
    public void Write( string str, int off, int len )
    {
        lock ( lockObject )
        {
            char[] cbuf;

            if ( len <= WriteBufferSize )
            {
                _writeBuffer ??= new char[ WriteBufferSize ];

                cbuf = _writeBuffer;
            }
            else
            {
                // Don't permanently allocate very large buffers.
                cbuf = new char[ len ];
            }

            str.CopyTo( off, cbuf, 0, ( off + len ) );

            Write( cbuf, 0, len );
        }
    }

    /// <summary>
    /// Appends the specified character sequence to this writer.
    /// <para>
    /// An invocation of this method of the form <b>out.append(csq)</b> behaves in
    /// exactly the same way as the invocation <b> out.write(csq.toString()) </b>
    /// </para>
    /// <para>
    /// Depending on the specification of <b>ToString</b> for the character sequence
    /// <b>csq</b>, the entire sequence may not be appended. For instance, invoking
    /// the <b>ToString</b> method of a character buffer will return a subsequence
    /// whose content depends upon the buffer's position and limit.
    /// </para>
    /// </summary>
    /// <param name="csq">
    /// The character sequence to append. If <b>csq</b> is <b>null</b>, then the
    /// four characters <b>"null"</b> are appended to this writer.
    /// </param>
    /// <returns> This writer </returns>
    public Writer Append( string? csq )
    {
        Write( csq ?? "null" );

        return this;
    }

    /// <summary>
    /// Appends a subsequence of the specified character sequence to this writer.
    /// <para>
    /// An invocation of this method of the form <b>out.append(csq, start, end)</b> when
    /// <b>csq</b> is not <b>null</b> behaves in exactly the same way as the invocation
    /// <b>out.Write(csq.SubSequence(start, end).ToString())</b>
    /// </para>
    /// </summary>
    /// <param name="csq">
    /// The character sequence from which a subsequence will be appended. If <b>csq</b>
    /// is <b>null</b>, then characters will be appended as if <b>csq</b> contained the
    /// four characters <b>"null"</b>.
    /// </param>
    /// <param name="start"> The index of the first character in the subsequence </param>
    /// <param name="end">
    /// The index of the character following the last character in the subsequence
    /// </param>
    /// <returns> This writer </returns>
    public Writer Append( string? csq, int start, int end )
    {
        var cs = csq ?? "null";

        Write( cs.Substring( start, end ) );

        return this;
    }

    /// <summary>
    /// Appends the specified character to this writer.
    /// <para>
    /// An invocation of this method of the form <b>out.append(c)</b> behaves in
    /// exactly the same way as the invocation <b> out.write(c) </b>
    /// </para>
    /// </summary>
    /// <param name="c"> The 16-bit character to append </param>
    /// <returns>  This writer </returns>
    public Writer Append( char c )
    {
        Write(c);

        return this;
    }

    /// <summary>
    /// Flushes the stream.  If the stream has saved any characters from the
    /// various write() methods in a buffer, write them immediately to their
    /// intended destination.  Then, if that destination is another character or
    /// byte stream, flush it.  Thus one flush() invocation will flush all the
    /// buffers in a chain of Writers and OutputStreams.
    /// <para>
    /// If the intended destination of this stream is an abstraction provided
    /// by the underlying operating system, for example a file, then flushing the
    /// stream guarantees only that bytes previously written to the stream are
    /// passed to the operating system for writing; it does not guarantee that
    /// they are actually written to a physical device such as a disk drive.
    /// </para>
    /// </summary>
    public abstract void Flush();

    /// <summary>
    /// Closes the stream, flushing it first. Once the stream has been closed,
    /// further write() or flush() invocations will cause an IOException to be
    /// thrown. Closing a previously closed stream has no effect.
    /// </summary>
    public virtual void Close()
    {
    }
}
