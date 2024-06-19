// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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
/// <see cref="FileInfo"/>s instance. Because some of the file types are backed by composite
/// files and may be compressed (for example, if they are in an Android .apk or are found
/// via the classpath), the methods for extracting a <b>Path</b> or <b>FileInfo</b> may not
/// be appropriate for all types.
/// <para>
/// Use the Reader or Stream methods here to hide these dependencies from your platform
/// independent code.
/// </para>
/// </summary>
[PublicAPI]
public class FileHandle
{
    public PathTypes PathType { get; set; }
    public FileInfo  File     { get; set; }

    // ------------------------------------------------------------------------

    protected FileHandle()
    {
        this.File     = null!;
        this.PathType = PathTypes.Absolute;
    }
    
    /// <summary>
    /// Creates a new absolute FileHandle for the file name. Use this for tools on
    /// the desktop that don't need any of the backends. Do not use this constructor
    /// for cross-platform developments. Use the <see cref="IFiles"/> interface instead.
    /// </summary>
    /// <param name="fileName"> the filename. </param>
    public FileHandle( string fileName )
    {
        this.File     = new FileInfo( fileName );
        this.PathType = PathTypes.Absolute;
    }

    /// <summary>
    /// Creates a new absolute FileHandle for the File. Use this for tools on the desktop
    /// that don't need any of the backends. Do not use this constructor for cross-platform
    /// developments. Use the <see cref="IFiles"/> interface instead.
    /// </summary>
    /// <param name="file"> the file. </param>
    public FileHandle( FileInfo file )
    {
        this.File     = file;
        this.PathType = PathTypes.Absolute;
    }

    /// <summary>
    /// Creates a new absolute FileHandle for the file name. Use this for tools on the
    /// desktop that don't need any of the backends. Do not use this constructor for
    /// cross-platform developments. Use the <see cref="IFiles"/> interface instead.
    /// </summary>
    public FileHandle( string fileName, PathTypes type )
    {
        this.File     = new FileInfo( fileName );
        this.PathType = type;
    }

    /// <summary>
    /// Creates a new absolute FileHandle for the file name. Use this for tools on the
    /// desktop that don't need any of the backends. Do not use this constructor for
    /// cross-platform developments. Use the <see cref="IFiles"/> interface instead.
    /// </summary>
    public FileHandle( FileInfo file, PathTypes type )
    {
        this.File     = file;
        this.PathType = type;
    }

    /// <summary>
    /// The name of the file, without any parent paths.
    /// </summary>
    public string FileName => File.Name;

    /// <summary>
    /// The path of the file as specified on construction, e.g.
    /// <code>
    /// Gdx.Files.Internal("dir/file.png") -> dir/file.png.
    /// </code>
    /// <para>
    /// backward slashes will be replaced by forward slashes.
    /// </para>
    /// </summary>
    public string FilePath => File.FullName.Replace( "\\", "/" );

    /// <summary>
    /// Returns the file extension (without the dot) or an empty string if the
    /// file name doesn't contain a dot.
    /// </summary>
    public string Extension()
    {
        return Path.GetExtension( File.Name ).TrimStart( '.' );
    }

    /// <summary>
    /// Return the name of the file, without parent paths or the extension.
    /// </summary>
    public string NameWithoutExtension()
    {
        return Path.GetFileNameWithoutExtension( File.Name );
    }

    /// <summary>
    /// Returns the path and filename without the extension,
    /// <para>
    /// e.g. dir/dir2/file.png -> dir/dir2/file.
    /// </para>
    /// Backward slashes will be returned as forward slashes.
    /// </summary>
    public string PathWithoutExtension()
    {
        return Path.GetFileNameWithoutExtension( File.FullName );
    }
}
