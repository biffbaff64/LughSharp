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

public class CloseableStreamReader : StreamReader, ICloseable
{

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified stream.
    /// </summary>
    /// <param name="stream">The stream to be read.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    public CloseableStreamReader( Stream stream ) : base( stream )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified stream, with the specified byte order mark detection option.
    /// </summary>
    /// <param name="stream">The stream to be read.</param>
    /// <param name="detectEncodingFromByteOrderMarks">
    /// Indicates whether to look for byte order marks at the beginning of the file.
    /// </param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    public CloseableStreamReader( Stream stream, bool detectEncodingFromByteOrderMarks )
        : base( stream, detectEncodingFromByteOrderMarks )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified stream, with the specified character encoding.
    /// </summary>
    /// <param name="stream">The stream to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    public CloseableStreamReader( Stream stream, Encoding encoding )
        : base( stream, encoding )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified stream, with the specified character encoding and byte order
    /// mark detection option.
    /// </summary>
    /// <param name="stream">The stream to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">
    /// Indicates whether to look for byte order marks at the beginning of the file.
    /// </param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    public CloseableStreamReader( Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks )
        : base( stream, encoding, detectEncodingFromByteOrderMarks )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified stream, with the specified character encoding, byte order mark
    /// detection option, and buffer size.
    /// </summary>
    /// <param name="stream">The stream to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">
    /// Indicates whether to look for byte order marks at the beginning of the file.
    /// </param>
    /// <param name="bufferSize">The minimum buffer size.</param>
    /// <exception cref="T:System.ArgumentException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is less than or equal to zero.</exception>
    public CloseableStreamReader( Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks,
                                  int bufferSize )
        : base( stream, encoding, detectEncodingFromByteOrderMarks, bufferSize )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified stream based on the specified character encoding, byte order
    /// mark detection option, and buffer size, and optionally leaves the stream open.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">
    /// <see langword="true" />
    /// to look for byte order marks at the beginning of the file; otherwise, <see langword="false" />.
    /// </param>
    /// <param name="bufferSize">The minimum buffer size.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" />
    /// to leave the stream open after the <see cref="T:System.IO.StreamReader" /> object
    /// is disposed; otherwise, <see langword="false" />.
    /// </param>
    public CloseableStreamReader( Stream stream, Encoding? encoding = null,
                                  bool detectEncodingFromByteOrderMarks = true, int bufferSize = -1,
                                  bool leaveOpen = false )
        : base( stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified file name.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// The specified path is invalid, such as being on an unmapped drive.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" />
    /// includes an incorrect or invalid syntax for file name, directory name, or volume label.
    /// </exception>
    public CloseableStreamReader( string path ) : base( path )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified file name, with the specified byte order mark detection option.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="detectEncodingFromByteOrderMarks">
    /// Indicates whether to look for byte order marks at the beginning of the file.
    /// </param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// The specified path is invalid, such as being on an unmapped drive.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" />
    /// includes an incorrect or invalid syntax for file name, directory name, or volume label.
    /// </exception>
    public CloseableStreamReader( string path, bool detectEncodingFromByteOrderMarks )
        : base( path, detectEncodingFromByteOrderMarks )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class
    /// for the specified file path, using the default encoding, enabling detection of
    /// byte order marks at the beginning of the file, and configured with the specified
    /// <see cref="T:System.IO.FileStreamOptions" /> object.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="options">
    /// An object that specifies the configuration options for the underlying
    /// <see cref="T:System.IO.FileStream" />.
    /// </param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="stream" /> is not readable.
    /// -or-
    ///           <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="options" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// The specified path is invalid, such as being on an unmapped drive.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" />
    /// includes an incorrect or invalid syntax for file name, directory name, or volume label.
    /// </exception>
    public CloseableStreamReader( string path, FileStreamOptions options ) : base( path, options )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for
    /// the specified file name, with the specified character encoding.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// The specified path is invalid, such as being on an unmapped drive.
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" />
    /// includes an incorrect or invalid syntax for file name, directory name, or volume label.
    /// </exception>
    public CloseableStreamReader( string path, Encoding encoding ) : base( path, encoding )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified character encoding and byte order mark detection option.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    public CloseableStreamReader( string path, Encoding encoding, bool detectEncodingFromByteOrderMarks )
        : base( path, encoding, detectEncodingFromByteOrderMarks )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified character encoding, byte order mark detection option, and buffer size.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <param name="bufferSize">The minimum buffer size, in number of 16-bit characters.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="buffersize" /> is less than or equal to zero.</exception>
    public CloseableStreamReader( string path, Encoding encoding, bool detectEncodingFromByteOrderMarks,
                                  int bufferSize )
        : base( path, encoding, detectEncodingFromByteOrderMarks, bufferSize )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file path, with the specified character encoding, byte order mark detection option, and configured with the specified <see cref="T:System.IO.FileStreamOptions" /> object.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <param name="options">An object that specifies the configuration options for the underlying <see cref="T:System.IO.FileStream" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="stream" /> is not readable.
    ///           -or-
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> or <paramref name="options" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    public CloseableStreamReader( string path, Encoding encoding, bool detectEncodingFromByteOrderMarks,
                                  FileStreamOptions options )
        : base( path, encoding, detectEncodingFromByteOrderMarks, options )
    {
    }
}