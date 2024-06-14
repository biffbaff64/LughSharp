// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace LughSharp.LibCore.Files;

/// <summary>
/// Represents a file or directory on the filesystem. FileHandles are created via a
/// <see cref="File"/>s instance. Because some of the file types are backed by composite
/// files and may be compressed (for example, if they are in an Android .apk or are found
/// via the classpath), the methods for extracting a <b>Path</b> or <b>File</b> may not
/// be appropriate for all types.
/// <para>
/// Use the Reader or Stream methods here to hide these dependencies from your platform
/// independent code.
/// </para>
/// </summary>
[PublicAPI]
public class FileHandle
{
    public FileType Filetype { get; set; }
    public File     File     { get; set; }

    // ------------------------------------------------------------------------

    protected FileHandle()
    {
        this.File     = new File();
        this.Filetype = FileType.Absolute;
    }

    /// <summary>
    /// Creates a new absolute FileHandle for the file name. Use this for tools on the
    /// desktop that don't need any of the backends. Do not use this constructor for
    /// cross-platform developments. Use the <see cref="IFiles"/> interface instead.
    /// </summary>
    public FileHandle( string fileName, FileType type )
    {
        this.File     = new File( fileName );
        this.Filetype = type;
    }

    /// <summary>
    /// The name of the file, without any parent paths.
    /// </summary>
    public string Name => File.Name;

    /// <summary>
    /// The path of the file as specified on construction, e.g.
    /// <code>
    /// Gdx.Files.Internal("dir/file.png") -> dir/file.png.
    /// </code>
    /// <para>
    /// backward slashes will be replaced by forward slashes.
    /// </para>
    /// </summary>
    public string Path => File.Path.Replace( "\\", "/" );
}
