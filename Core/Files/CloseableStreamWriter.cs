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

namespace LibGDXSharp.Core.Files;

public class CloseableStreamWriter : StreamWriter, ICloseable
{

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using UTF-8 encoding and the default buffer size.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    public CloseableStreamWriter( Stream stream ) : base( stream )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and the default buffer size.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public CloseableStreamWriter( Stream stream, Encoding encoding ) : base( stream, encoding )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and buffer size.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="bufferSize">The buffer size, in bytes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public CloseableStreamWriter( Stream stream, Encoding encoding, int bufferSize ) : base
        ( stream, encoding, bufferSize )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and buffer size, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="bufferSize">The buffer size, in bytes.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.StreamWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public CloseableStreamWriter( Stream stream, Encoding? encoding = null, int bufferSize = -1,
                                  bool leaveOpen = false ) : base( stream, encoding, bufferSize, leaveOpen )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the default encoding and buffer size.</summary>
    /// <param name="path">The complete file path to write to. <paramref name="path" /> can be a file name.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is an empty string ("").
    /// -or-
    /// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public CloseableStreamWriter( string path ) : base( path )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the default encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="append">
    /// <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is empty.
    /// -or-
    /// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public CloseableStreamWriter( string path, bool append ) : base( path, append )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the specified encoding and default buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="append">
    /// <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is empty.
    /// -or-
    /// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public CloseableStreamWriter( string path, bool append, Encoding encoding ) : base( path, append, encoding )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file on the specified path, using the specified encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="append">
    /// <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="bufferSize">The buffer size, in bytes.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is an empty string ("").
    /// -or-
    /// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    public CloseableStreamWriter( string path, bool append, Encoding encoding, int bufferSize ) : base
        ( path, append, encoding, bufferSize )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file, using the default encoding, and configured with the specified <see cref="T:System.IO.FileStreamOptions" /> object.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="options">An object that specifies the configuration options for the underlying <see cref="T:System.IO.FileStream" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="options" /> is <see langword="null" />
    /// .</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public CloseableStreamWriter( string path, FileStreamOptions options ) : base( path, options )
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file, using the specified encoding, and configured with the specified <see cref="T:System.IO.FileStreamOptions" /> object.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="options">An object that specifies the configuration options for the underlying <see cref="T:System.IO.FileStream" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="options" /> is <see langword="null" />
    /// .</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public CloseableStreamWriter( string path, Encoding encoding, FileStreamOptions options ) : base
        ( path, encoding, options )
    {
    }
}