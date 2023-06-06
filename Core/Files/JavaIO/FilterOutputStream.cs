namespace LibGDXSharp.Files;

/**
 * This class is the superclass of all classes that filter output
 * streams. These streams sit on top of an already existing output
 * stream (the <i>underlying</i> output stream) which it uses as its
 * basic sink of data, but possibly transforming the data along the
 * way or providing additional functionality.
 * <p>
 * The class <tt>FilterOutputStream</tt> itself simply overrides
 * all methods of <tt>OutputStream</tt> with versions that pass
 * all requests to the underlying output stream. Subclasses of
 * <tt>FilterOutputStream</tt> may further override some of these
 * methods as well as provide additional methods and fields.
 * </p>
 */
public class FilterOutputStream : OutputStream
{
    /// <summary>
    /// The underlying output stream to be filtered.
    /// </summary>
    protected readonly OutputStream outputStream;

    /// <summary>
    /// Creates an output stream filter built on top of the specified
    /// underlying output stream.
    /// </summary>
    /// <param name="ostream">
    /// the underlying output stream to be assigned to the field <tt>this.out</tt>
    /// for later use, or <tt>null</tt> if this instance is to be created without
    /// an underlying stream.
    /// </param>
    public FilterOutputStream( OutputStream ostream )
    {
        this.outputStream = ostream;
    }

    /// <summary>
    /// Writes the specified <tt>byte</tt> to this output stream.
    /// <para>
    /// The <tt>write</tt> method of <tt>FilterOutputStream</tt> calls the
    /// <tt>write</tt> method of its underlying output stream, that is, it
    /// performs <tt>out.write(b)</tt>.
    /// </para>
    /// <para>
    /// Implements the abstract <tt>Write</tt> method of <tt>OutputStream</tt>.
    /// </para>
    /// </summary>
    /// <param name="b">the <tt>byte</tt>. </param>
    /// <exception cref="IOException">if an I/O error occurs. </exception>
    public new void Write( int b )
    {
        outputStream.Write( b );
    }

    /// <summary>
    /// Writes <tt>b.length</tt> bytes to this output stream.
    /// <para>
    /// The <tt>write</tt> method of <tt>FilterOutputStream</tt>
    /// calls its <tt>write</tt> method of three arguments with the
    /// arguments <tt>b</tt>, <tt>0</tt>, and
    /// <tt>b.length</tt>.
    /// </para>
    /// <para>
    /// Note that this method does not call the one-argument
    /// <tt>write</tt> method of its underlying stream with the single
    /// argument <tt>b</tt>.
    /// 
    /// </para>
    /// </summary>
    /// <param name="b">   the data to be written. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    /// <seealso cref="FilterOutputStream.Write(byte[], int, int)"/>
    public void Write( byte[] b )
    {
        Write( b, 0, b.Length );
    }

    /// <summary>
    /// Writes <tt>len</tt> bytes from the specified <tt>byte</tt> array starting
    /// at offset <tt>off</tt> to this output stream.
    /// <para>
    /// The <tt>write</tt> method of <tt>FilterOutputStream</tt> calls the <tt>Write</tt>
    /// method of one argument on each <tt>byte</tt> to output.
    /// </para>
    /// <para>
    /// Note that this method does not call the <tt>Write</tt> method of its underlying
    /// input stream with the same arguments. Subclasses of <tt>FilterOutputStream</tt>
    /// should provide a more efficient implementation of this method.
    /// </para>
    /// </summary>
    /// <param name="b"> the data. </param>
    /// <param name="off"> the start offset in the data. </param>
    /// <param name="len"> the number of bytes to write. </param>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    /// <seealso cref="FilterOutputStream.Write(int)"/>
    public void Write( byte[] b, int off, int len )
    {
        if ( ( off | len | ( b.Length - ( len + off ) ) | ( off + len ) ) < 0 )
        {
            throw new IndexOutOfRangeException();
        }

        for ( int i = 0; i < len; i++ )
        {
            Write( b[ off + i ] );
        }
    }

    /// <summary>
    /// Flushes this output stream and forces any buffered output bytes to be
    /// written out to the stream.
    /// <para>
    /// The <tt>flush</tt> method of <tt>FilterOutputStream</tt> calls the <tt>flush</tt>
    /// method of its underlying output stream.
    /// </para>
    /// </summary>
    /// <exception cref="IOException">  if an I/O error occurs. </exception>
    /// <seealso cref="FilterOutputStream.outputStream"/>
    public new void Flush()
    {
        outputStream.Flush();
    }

    /// <summary>
    /// Closes this output stream and releases any system resources associated
    /// with the stream.
    /// <para>
    /// The <tt>close</tt> method of <tt>FilterOutputStream</tt> calls its <tt>flush</tt>
    /// method, and then calls the <tt>close</tt> method of its underlying output stream.
    /// </para> </summary>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    /// <seealso cref="FilterOutputStream.Flush()"/>
    /// <seealso cref="FilterOutputStream.outputStream"/>
    public void Close()
    {
        using OutputStream ostream = outputStream;

        Flush();
    }
}

