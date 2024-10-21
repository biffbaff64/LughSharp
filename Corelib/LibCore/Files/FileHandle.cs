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

using Corelib.LibCore.Core;

namespace Corelib.LibCore.Files;

/// <summary>
/// A FileInfo container which contains a <see cref="FileInfo"/> instance
/// and a <see cref="PathTypes"/> instance. The <c>PathTypes</c> instance
/// defaults to <see cref="PathTypes.Internal"/>.
/// </summary>
[PublicAPI]
public class FileHandle
{
    public PathTypes PathType { get; set; }
    public FileInfo  File     { get; set; }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Create a new FileHandle with a default <see cref="FileInfo"/> and
    /// PathType set to <see cref="PathTypes.Internal"/>
    /// </summary>
    protected FileHandle()
    {
        this.File     = new FileInfo( "" );
        this.PathType = PathTypes.Internal;
    }

    /// <summary>
    /// Create a new FileHandle with a <see cref="FileInfo"/> using the supplied
    /// filename, and PathType set to <see cref="PathTypes.Internal"/>
    /// </summary>
    /// <param name="fileName"></param>
    public FileHandle( string fileName )
    {
        this.File     = new FileInfo( fileName );
        this.PathType = PathTypes.Internal;
    }

    /// <summary>
    /// Creates a new FileHandle using the supplied <see cref="FileInfo"/>
    /// </summary>
    /// <param name="file"> The FileInfo instance. </param>
    public FileHandle( FileInfo file )
    {
        this.File     = file;
        this.PathType = PathTypes.Internal;
    }

    /// <summary>
    /// Creates a new FileHandle using the supplied fileName string,
    /// and <see cref="PathTypes"/> value.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public FileHandle( string fileName, PathTypes type )
    {
        this.File     = new FileInfo( fileName );
        this.PathType = type;
    }

    /// <summary>
    /// Creates a new FileHandle using the supplied FileInfo instance,
    /// and <see cref="PathTypes"/> value.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="type"></param>
    public FileHandle( FileInfo file, PathTypes type )
    {
        this.File     = file;
        this.PathType = type;
    }

    // ------------------------------------------------------------------------

    #region helpers

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
    public string Extension() => Path.GetExtension( File.Name ).TrimStart( '.' );

    /// <summary>
    /// Return the name of the file, without parent paths or the extension.
    /// </summary>
    public string NameWithoutExtension() => Path.GetFileNameWithoutExtension( File.Name );

    /// <summary>
    /// Returns the path and filename without the extension,
    /// <para>
    /// e.g. dir/dir2/file.png -> dir/dir2/file.
    /// </para>
    /// Backward slashes will be returned as forward slashes.
    /// </summary>
    public string PathWithoutExtension() => Path.GetFileNameWithoutExtension( File.FullName );

    #endregion helpers

    // ------------------------------------------------------------------------

    #region unimplemented

    // Not yet implemented from Java::LibGDX. Not all will be implemented.

//    public  File                File();
//    public  InputStream         Read();
//    public  BufferedInputStream Read( int bufferSize );
//    public  Reader              Reader();
//    public  Reader              Reader( string charset );
//    public  BufferedReader      Reader( int bufferSize );
//    public  BufferedReader      Reader( int bufferSize, string charset );
//    public  string              ReadString();
//    public  string              ReadString( string charset );
//    public  byte[]              ReadBytes();
//    private int                 EstimateLength();
//    public  int                 ReadBytes( byte[] bytes, int offset, int size );
//    public  ByteBuffer          Map();
//    public  ByteBuffer          Map( FileChannel.MapMode mode );
//    public  OutputStream        Write( bool append );
//    public  OutputStream        Write( bool append, int bufferSize );
//    public  void                Write( InputStream input, bool append );
//    public  Writer              Writer( bool append );
//    public  Writer              Writer( bool append, string charset );
//    public  void                WriteString( string string, bool append);
//    public  void                WriteString( string string, bool append, string charset);
//    public  void                WriteBytes( byte[] bytes, bool append );
//    public  void                WriteBytes( byte[] bytes, int offset, int length, bool append );
//    public  FileHandle[]        List();
//    public  FileHandle[]        List( FileFilter filter );
//    public  FileHandle[]        List( FilenameFilter filter );
//    public  FileHandle[]        List( string suffix );
//    public  bool                IsDirectory();
//    public  FileHandle          Child( string name );
//    public  FileHandle          Sibling( string name );
//    public  FileHandle          Parent();
//    public  void                Mkdirs();
//    public  bool                Exists();
//    public  bool                Delete();
//    public  bool                DeleteDirectory();
//    public  void                EmptyDirectory();
//    public  void                EmptyDirectory( bool preserveTree );
//    public  void                CopyTo( FileHandle dest );
//    public  void                MoveTo( FileHandle dest );
//    public  long                Length();
//    public  long                LastModified();
//    public  bool                Equals( object obj );
//    public  int                 HashCode();
//    public  string              ToString();
//    public  FileHandle          TempFile( string prefix );
//    public  FileHandle          TempDirectory( string prefix );
//    public  void                EmptyDirectory( File file, bool preserveTree );
//    private bool                DeleteDirectory( File file );
//    private void                CopyFile( FileHandle source, FileHandle dest );
//    private void                CopyDirectory( FileHandle sourceDir, FileHandle destDir );

    #endregion unimplemented
}