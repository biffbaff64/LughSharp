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
/// A contains some other input stream, which it uses as its basic source of
/// data, possibly transforming the data along the way or providing additional
/// functionality. The class itself simply overrides all methods of with versions
/// that pass all requests to the contained input stream. Subclasses of may
/// further override some of these methods and may also provide additional
/// methods and fields.
/// </summary>
[PublicAPI]
public class FilterInputStream : InputStream
{
    /// <summary>
    /// The input stream to be filtered.
    /// </summary>
    public InputStream? InputStream { get; set; }

    /// <summary>
    /// Creates a <tt>FilterInputStream</tt> by assigning the  argument <tt>in</tt>
    /// to the field <tt>this.in</tt> so as to remember it for later use.
    /// <param name="inStream">
    /// the underlying input stream, or <tt>null</tt> if this instance is to be
    /// created without an underlying stream.
    /// </param>
    /// </summary>
    protected FilterInputStream( InputStream? inStream )
    {
        ArgumentNullException.ThrowIfNull
            ( inStream, "Can't create a FilterInputSTream with a null In[putStream!" );

        this.InputStream = inStream;
    }

    /// <inheritdoc/>
    public override int Read()
    {
        return InputStream!.Read();
    }
    
    /// <summary>
    /// Reads up to <tt>byte.length</tt> bytes of data from this input stream
    /// into an array of bytes. This method blocks until some input is available.
    /// <para>
    /// This method simply performs the call <tt>read(b, 0, b.length)</tt> and
    /// returns the result. It is important that it does <i>not</i> do
    /// <tt>in.read(b)</tt> instead;
    /// certain subclasses of <tt>FilterInputStream</tt> depend on the implementation
    /// strategy actually used.
    /// </para>
    /// <param name="b"> the buffer into which the data is read. </param>
    /// </summary>
    /// <returns>
    /// the total number of bytes read into the buffer, or <tt>-1</tt> if there is no
    /// more data because the end of the stream has been reached.
    /// </returns>
    /// <exception cref="IOException"> if an I/O error occurs. </exception>
    public int Read( byte[] b )
    {
        return Read( b, 0, b.Length );
    }
}
