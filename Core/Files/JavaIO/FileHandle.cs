// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Text;

namespace LibGDXSharp.Files;

/// <summary>
/// Represents a file or directory on the filesystem, classpath, app storage or assets
/// directory. FileHandles are created via a Files instance. Because some of the file
/// types are backed by composite files and may be compressed (for example, if they are
/// found via the classpath), the methods for extracting a path() or file() may not be
/// appropriate for all types. Use the Reader or Stream methods here to hide these
/// dependencies from your platform independent code.
/// </summary>
// -------------------------------------------------------------------
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" )]
// -------------------------------------------------------------------
public class FileHandle
{
    public FileType    Type { get; set; }
    public FileStream? File { get; set; }

    /// <summary>
    /// Creates a new absolute FileHandle for the file name.
    /// </summary>
    public FileHandle( string fileName )
    {
        this.Type = FileType.Absolute;
        this.File = new FileStream( fileName, FileMode.Open );
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    public FileHandle( FileStream file )
    {
        this.Type = FileType.Absolute;
        this.File = file;
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    // ReSharper disable once MemberCanBeProtected.Global
    public FileHandle( string fileName, FileType type )
    {
        this.Type = type;
        this.File = new FileStream( fileName, FileMode.Open );
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="type"></param>
    public FileHandle( FileStream file, FileType type )
    {
        this.Type = type;
        this.File = file;
    }

    /// <summary>
    /// the path of the file as specified on construction,
    /// e.g. Gdx.IFile.Internal("dir/file.png") -> dir/file.png.
    /// Backward slashes will be replaced by forward slashes.
    /// </summary>
    /// <returns></returns>
    public string Path() => System.IO.Path.GetFullPath( File!.Name ).Replace( '\\', '/' );

    /// <summary>
    /// Returns the name of the file, without any parent paths.
    /// </summary>
    /// <returns></returns>
    public string? Name() => File?.Name;

    /// <summary>
    /// Returns the file extension (without the dot) or an empty
    /// string if the file name doesn't contain a dot.
    /// </summary>
    public string Extension()
    {
        var name = File?.Name;

        if ( name == null ) return "";

        var dotIndex = name.LastIndexOf( '.' );

        return dotIndex == -1 ? "" : name[ ( dotIndex + 1 ).. ];
    }

    /// <summary>
    /// Returns the name of the file, without parent paths or the extension.
    /// </summary>
    /// <returns></returns>
    public string NameWithoutExtension()
    {
        if ( File == null ) throw new GdxRuntimeException( $"File not found: {File}" );

        var fileName = this.File.Name;
        var dotIndex = fileName.LastIndexOf( ".", StringComparison.Ordinal );

        return dotIndex == -1 ? fileName : fileName[ ..dotIndex ];
    }

    /// <summary>
    /// Returns the path and filename without the extension.
    /// e.g. dir/dir2/file.png -> dir/dir2/file.
    /// Backward slashes will be returned as forward slashes.
    /// </summary>
    /// <returns></returns>
    public string PathWithoutExtension()
    {
        if ( File == null ) throw new GdxRuntimeException( $"File not found: {File}" );

        var filePath = System.IO.Path.GetFullPath( File.Name );
        var dotIndex = filePath.LastIndexOf( ".", StringComparison.Ordinal );

        return dotIndex == -1 ? filePath : filePath[ ..dotIndex ];
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public FileStream Read()
    {
        if ( File == null ) throw new GdxRuntimeException( $"File not found: {File}" );

        return new FileStream( File.Name, FileMode.OpenOrCreate );
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public BufferedStream Read( int bufferSize ) => new BufferedStream( this.Read(), bufferSize );

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public StreamReader Reader() => new StreamReader( Read() );

    /// <summary>
    /// </summary>
    /// <param name="charset"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public StreamReader Reader( Encoding charset )
    {
        FileStream stream = Read();

        try
        {
            return new StreamReader( stream, charset );
        }
        catch ( Exception ex )
        {
            stream.Close();

            throw new GdxRuntimeException( "Error reading file: " + this, ex );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public BufferedStream Reader( int bufferSize ) => Read( bufferSize );

    /// <summary>
    /// </summary>
    /// <param name="bufferSize"></param>
    /// <param name="charset"></param>
    /// <returns></returns>
    public BufferedStream Reader( int bufferSize, Encoding charset ) => Reader( bufferSize );

    /// <summary>
    /// </summary>
    /// <param name="charset"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public string ReadString( Encoding? charset = null )
    {
        var           output = new StringBuilder( EstimateLength() );
        StreamReader? reader = null;

        try
        {
            reader = charset == null ? new StreamReader( Read() ) : new StreamReader( Read(), charset );

            output.Append( reader.ReadToEnd() );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error reading layout file - " + this, ex );
        }
        finally
        {
            reader?.Close();
        }

        return output.ToString();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public byte[] ReadBytes()
    {
        FileStream input = Read();

        try
        {
            var buffer = new byte[ 16 * 1024 ];

            using var ms = new MemoryStream();

            int read;

            while ( ( read = input.Read( buffer, 0, buffer.Length ) ) > 0 )
            {
                ms.Write( buffer, 0, read );
            }

            return ms.ToArray();
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error reading file: " + this, ex );
        }
        finally
        {
            input.Close();
        }
    }

    private int EstimateLength()
    {
        var length = ( int )Length();

        return length != 0 ? length : 512;
    }

    /// <summary>
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public int ReadBytes( byte[] bytes, int offset, int size )
    {
        FileStream input    = Read();
        var        position = 0;

        try
        {
            while ( true )
            {
                var count = input.Read( bytes, offset + position, size - position );

                if ( count <= 0 )
                {
                    break;
                }

                position += count;
            }
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error Reading File: " + this, ex );
        }
        finally
        {
            input.Close();
        }

        return position - offset;
    }

//    /// <summary>
//    /// Attempts to memory map this file in READ_ONLY mode.
//    /// </summary>
//    public ByteBuffer Map() => Map( MapMode.ReadOnly );

//    /// <summary>
//    /// Attempts to memory map this file. Android files must not be compressed.
//    /// </summary>
//    /// <exception cref="GdxRuntimeException">
//    /// if this file handle represents a directory, doesn't exist, or could not
//    /// be read, or memory mapping fails, or is a <see cref="FileType.Classpath"/>
//    /// file.
//    /// </exception>
//    public ByteBuffer Map( FileChannel.MapMode mode )
//    {
//        if ( Type == FileType.Classpath )
//        {
//            throw new GdxRuntimeException( "Cannot map a classpath file: " + this );
//        }

//        RandomAccessFile raf = null;

//        try
//        {
//            raf = new RandomAccessFile( File, mode == MapMode.READ_ONLY ? "r" : "rw" );

//            FileChannel fileChannel = raf.getChannel();
//            ByteBuffer  map         = fileChannel.map( mode, 0, File?.Length );

//            map.Order( ByteOrder.NativeOrder() );

//            return map;
//        }
//        catch ( Exception ex )
//        {
//            throw new GdxRuntimeException( "Error memory mapping file: " + this + " (" + type + ")", ex );
//        }
//        finally
//        {
//            StreamUtils.CloseQuietly( raf );
//        }
//    }

    /// <summary>
    /// </summary>
    /// <param name="append"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public FileStream Write( bool append )
    {
        if ( File == null ) throw new GdxRuntimeException( " File is null! " );

        if ( Type == FileType.Classpath )
        {
            throw new GdxRuntimeException( "Cannot write to a classpath file: " + File, null );
        }

        if ( Type == FileType.Internal )
        {
            throw new GdxRuntimeException( "Cannot write to an internal file: " + File, null );
        }

        try
        {
            return System.IO.File.OpenWrite( File.Name );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error Writing File: " + File + " (" + Type + ")", ex );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="append"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public BufferedStream Write( bool append, int bufferSize )
    {
        var stream = Write( append );

        return new BufferedStream( stream, bufferSize );
    }

    /// <summary>
    /// </summary>
    /// <param name="input"></param>
    /// <param name="append"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public void Write( FileStream input, bool append )
    {
        FileStream? output = null;

        try
        {
            output = Write( append );
            output.CopyTo( input );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error stream writing to file: " + File + " (" + Type + ")", ex );
        }
        finally
        {
            output?.Close();
            input.Close();
        }
    }

    /// <summary>
    /// Returns a writer for writing to this file using the default charset.
    /// Parent directories will be created if necessary.
    /// </summary>
    /// <param name="append"></param>
    /// <param name="charset"></param>
    /// <returns></returns>
    public Writer Writer( bool append, string? charset = null )
    {
        if ( Type == FileType.Classpath )
            throw new GdxRuntimeException( "Cannot write to a classpath file: " + file );

        if ( Type == FileType.Internal )
            throw new GdxRuntimeException( "Cannot write to an internal file: " + file );

        Parent().mkdirs();

        try
        {
            FileOutputStream output = new FileOutputStream( file(), append );

            if ( charset == null )
            {
                return new OutputStreamWriter( output );
            }
            else
            {
                return new OutputStreamWriter( output, charset );
            }
        }
        catch ( IOException ex )
        {
            if ( File().isDirectory() )
            {
                throw new GdxRuntimeException( "Cannot open a stream to a directory: {File} ( {Type} )", ex );
            }

            throw new GdxRuntimeException( "Error writing file: {File} ( {Type} )", ex );
        }
    }

    /** Writes the specified string to the file using the default charset. Parent directories will be created if necessary.
	 * @param append If false, this file will be overwritten if it exists, otherwise it will be appended.
	 * @throws GdxRuntimeException if this file handle represents a directory, if it is a {@link FileType#Classpath} or
	 *            {@link FileType#Internal} file, or if it could not be written. */
    public void WriteString( string str, bool append )
    {
        writeString( str, append, null );
    }

    /** Writes the specified string to the file using the specified charset. Parent directories will be created if necessary.
	 * @param append If false, this file will be overwritten if it exists, otherwise it will be appended.
	 * @param charset May be null to use the default charset.
	 * @throws GdxRuntimeException if this file handle represents a directory, if it is a {@link FileType#Classpath} or
	 *            {@link FileType#Internal} file, or if it could not be written. */
    public void WriteString( string str, bool append, string charset )
    {
        Writer writer = null;

        try
        {
            writer = writer( append, charset );
            writer.write( str );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error writing file: " + file + " (" + type + ")", ex );
        }
        finally
        {
            StreamUtils.closeQuietly( writer );
        }
    }

    /** Writes the specified bytes to the file. Parent directories will be created if necessary.
	 * @param append If false, this file will be overwritten if it exists, otherwise it will be appended.
	 * @throws GdxRuntimeException if this file handle represents a directory, if it is a {@link FileType#Classpath} or
	 *            {@link FileType#Internal} file, or if it could not be written. */
    public void WriteBytes( byte[] bytes, bool append )
    {
        OutputStream output = write( append );

        try
        {
            output.write( bytes );
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( "Error writing file: " + file + " (" + type + ")", ex );
        }
        finally
        {
            StreamUtils.closeQuietly( output );
        }
    }

    /** Writes the specified bytes to the file. Parent directories will be created if necessary.
	 * @param append If false, this file will be overwritten if it exists, otherwise it will be appended.
	 * @throws GdxRuntimeException if this file handle represents a directory, if it is a {@link FileType#Classpath} or
	 *            {@link FileType#Internal} file, or if it could not be written. */
    public void WriteBytes( byte[] bytes, int offset, int length, bool append )
    {
        OutputStream output = write( append );

        try
        {
            output.write( bytes, offset, length );
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( "Error writing file: " + file + " (" + type + ")", ex );
        }
        finally
        {
            StreamUtils.closeQuietly( output );
        }
    }

    /** Returns the paths to the children of this directory. Returns an empty
      * list if this file handle represents a file and not a
	  * directory. On the desktop, an {@link FileType#Internal} handle to a directory on the classpath will return a zero length
	  * array.
	  * @throws GdxRuntimeException if this file is an {@link FileType#Classpath} file. */
    public FileHandle[] List()
    {
        if ( Type == FileType.Classpath )
        {
            throw new GdxRuntimeException( "Cannot list a classpath directory: " + File );
        }

        var relativePaths = Directory.GetFiles( System.IO.Path.GetFullPath( File!.Name ) );

        var handles = new FileHandle[ relativePaths.Length ];

        for ( int i = 0, n = relativePaths.Length; i < n; i++ )
        {
            handles[ i ] = Child( relativePaths[ i ] );
        }

        return handles;
    }

    /// <summary>
    /// Returns the paths to the children of this directory that satisfy the specified
    /// filter. Returns an empty list if this file handle represents a file and not a
    /// directory. On the desktop, an <see cref="FileType.Internal"/> handle to a
    /// directory on the classpath will return a zero length array.
    /// </summary>
    /// <param name="filter"> the <see cref="IFileFilter"/> to filter files. </param>
    public FileHandle[] List( IFileFilter filter )
    {
        if ( Type == FileType.Classpath )
        {
            throw new GdxRuntimeException( "Cannot list a classpath directory: " + File );
        }

        var relativePaths = Directory.GetFiles( System.IO.Path.GetFullPath( File!.Name ) );

        var handles = new FileHandle[ relativePaths.Length ];
        var count   = 0;

        for ( int i = 0, n = relativePaths.Length; i < n; i++ )
        {
            var        path  = relativePaths[ i ];
            FileHandle child = Child( path );

            if ( !filter.Accept( child.File ) ) continue;

            handles[ count ] = child;
            count++;
        }

        if ( count < relativePaths.Length )
        {
            var newHandles = new FileHandle[ count ];
            Array.Copy( handles, 0, newHandles, 0, count );
            handles = newHandles;
        }

        return handles;
    }

    /** Returns the paths to the children of this directory that satisfy the specified filter. Returns an empty list if this file
	  * handle represents a file and not a directory. On the desktop, an {@link FileType#Internal} handle to a directory on the
	  * classpath will return a zero length array.
	  * @param filter the {@link FilenameFilter} to filter files
	  * @throws GdxRuntimeException if this file is an {@link FileType#Classpath} file. */
    public FileHandle[] List( IFilenameFilter filter )
    {
        if ( type == FileType.Classpath ) throw new GdxRuntimeException( "Cannot list a classpath directory: " + file );
        File     file          = file();
        string[] relativePaths = file.list();

        if ( relativePaths == null ) return new FileHandle[ 0 ];
        FileHandle[] handles = new FileHandle[ relativePaths.length ];
        int          count   = 0;

        for ( int i = 0, n = relativePaths.length; i < n; i++ )
        {
            string path = relativePaths[ i ];

            if ( !filter.accept( file, path ) ) continue;
            handles[ count ] = child( path );
            count++;
        }

        if ( count < relativePaths.length )
        {
            FileHandle[] newHandles = new FileHandle[ count ];
            System.arraycopy( handles, 0, newHandles, 0, count );
            handles = newHandles;
        }

        return handles;
    }

    /** Returns the paths to the children of this directory with the specified suffix. Returns an empty list if this file handle
	 * represents a file and not a directory. On the desktop, an {@link FileType#Internal} handle to a directory on the classpath
	 * will return a zero length array.
	 * @throws GdxRuntimeException if this file is an {@link FileType#Classpath} file. */
    public FileHandle[] List( string suffix )
    {
        if ( type == FileType.Classpath ) throw new GdxRuntimeException( "Cannot list a classpath directory: " + file );
        string[] relativePaths = file().list();

        if ( relativePaths == null ) return new FileHandle[ 0 ];
        FileHandle[] handles = new FileHandle[ relativePaths.length ];
        int          count   = 0;

        for ( int i = 0, n = relativePaths.length; i < n; i++ )
        {
            string path = relativePaths[ i ];

            if ( !path.endsWith( suffix ) ) continue;
            handles[ count ] = child( path );
            count++;
        }

        if ( count < relativePaths.Length )
        {
            var newHandles = new FileHandle[ count ];
            Array.Copy( handles, 0, newHandles, 0, count );
            handles = newHandles;
        }

        return handles;
    }

    /// <summary>
    /// Returns true if this file is a directory.
    /// Always returns false for classpath files.
    /// On Android, an <see cref="FileType.Internal"/> handle to an empty
    /// directory will return false.
    /// On the desktop, an <see cref="FileType.Internal"/> handle to a
    /// directory on the classpath will return false.
    /// </summary>
    public bool IsDirectory()
    {
        if ( Type == FileType.Classpath ) return false;

        return Directory.Exists( File?.Name );
    }

    /// <summary>
    /// Returns a handle to the child with the specified name.
    /// </summary>
    public FileHandle Child( string? name )
    {
        ArgumentNullException.ThrowIfNull( name );

        if ( System.IO.Path.GetFullPath( File!.Name ).Length == 0 )
        {
            return new FileHandle( new FileStream( name, FileMode.Create ), Type );
        }

        // TODO: Not sure if this is right, check it!

        return new FileHandle( new FileStream( name, FileMode.Open ), Type );
    }

    /// <summary>
    /// Returns a handle to the sibling with the specified name.
    /// </summary>
    /// <exception cref="GdxRuntimeException">If this file is the root.</exception>
    public FileHandle Sibling( string name )
    {
        if ( System.IO.Path.GetFullPath( name ).Length == 0)
        {
            throw new GdxRuntimeException( "Cannot get the sibling of the root." );
        }

        return new FileHandle( new FileStream( name, FileMode.Open ), Type );
    }

    public FileHandle Parent()
    {
        FileStream parent = File.GetParentFile();

        if ( parent == null )
        {
            if ( type == FileType.Absolute )
            {
                parent = new File( "/" );
            }
            else
            {
                parent = new FileStream( "" );
            }
        }

        return new FileHandle( parent, Type );
    }

    /** @throws GdxRuntimeException if this file handle is a {@link FileType#Classpath} or {@link FileType#Internal} file. */
//    public void mkdirs()
//    {
//        if ( type == FileType.Classpath )
//            throw new GdxRuntimeException( "Cannot mkdirs with a classpath file: " + file );

//        if ( type == FileType.Internal )
//            throw new GdxRuntimeException( "Cannot mkdirs with an internal file: " + file );

//        file().mkdirs();
//    }

    /** Returns true if the file exists. On Android, a {@link FileType#Classpath} or {@link FileType#Internal} handle to a
	 * directory will always return false. Note that this can be very slow for internal files on Android! */
    public bool Exists()
    {
        switch ( type )
        {
            case Internal:
                if ( file().exists() ) return true;

            // Fall through.
            case Classpath:
                return FileHandle.class.getResource( "/" + file.getPath().replace( '\\', '/' ) ) != null;
        }

        return file().exists();
    }

    /** Deletes this file or empty directory and returns success. Will not delete a directory that has children.
	 * @throws GdxRuntimeException if this file handle is a {@link FileType#Classpath} or {@link FileType#Internal} file. */
    public bool Delete()
    {
        if ( type == FileType.Classpath ) throw new GdxRuntimeException( "Cannot delete a classpath file: " + file );
        if ( type == FileType.Internal ) throw new GdxRuntimeException( "Cannot delete an internal file: " + file );

        return file().delete();
    }

    /** Deletes this file or directory and all children, recursively.
	 * @throws GdxRuntimeException if this file handle is a {@link FileType#Classpath} or {@link FileType#Internal} file. */
    public bool DeleteDirectory()
    {
        if ( type == FileType.Classpath ) throw new GdxRuntimeException( "Cannot delete a classpath file: " + file );
        if ( type == FileType.Internal ) throw new GdxRuntimeException( "Cannot delete an internal file: " + file );

        return deleteDirectory( file() );
    }

    /** Deletes all children of this directory, recursively. Optionally preserving the folder structure.
	 * @throws GdxRuntimeException if this file handle is a {@link FileType#Classpath} or {@link FileType#Internal} file. */
    public void EmptyDirectory( bool preserveTree = false )
    {
        if ( type == FileType.Classpath ) throw new GdxRuntimeException( "Cannot delete a classpath file: " + file );
        if ( type == FileType.Internal ) throw new GdxRuntimeException( "Cannot delete an internal file: " + file );
        EmptyDirectory( file(), preserveTree );
    }

    /** Copies this file or directory to the specified file or directory. If this handle is a file, then 1) if the destination is a
	 * file, it is overwritten, or 2) if the destination is a directory, this file is copied into it, or 3) if the destination
	 * doesn't exist, {@link #mkdirs()} is called on the destination's parent and this file is copied into it with a new name. If
	 * this handle is a directory, then 1) if the destination is a file, GdxRuntimeException is thrown, or 2) if the destination is
	 * a directory, this directory is copied into it recursively, overwriting existing files, or 3) if the destination doesn't
	 * exist, {@link #mkdirs()} is called on the destination and this directory is copied into it recursively.
	 * @throws GdxRuntimeException if the destination file handle is a {@link FileType#Classpath} or {@link FileType#Internal}
	 *            file, or copying failed. */
    public void CopyTo( FileHandle dest )
    {
        if ( !isDirectory() )
        {
            if ( dest.isDirectory() ) dest = dest.child( name() );
            copyFile( this, dest );

            return;
        }

        if ( dest.exists() )
        {
            if ( !dest.isDirectory() )
                throw new GdxRuntimeException( "Destination exists but is not a directory: " + dest );
        }
        else
        {
            dest.mkdirs();

            if ( !dest.isDirectory() )
                throw new GdxRuntimeException( "Destination directory cannot be created: " + dest );
        }

        copyDirectory( this, dest.child( name() ) );
    }

    /** Moves this file to the specified file, overwriting the file if it already exists.
	 * @throws GdxRuntimeException if the source or destination file handle is a {@link FileType#Classpath} or
	 *            {@link FileType#Internal} file. */
    public void MoveTo( FileHandle dest )
    {
        switch ( type )
        {
            case Classpath:
                throw new GdxRuntimeException( "Cannot move a classpath file: " + file );

            case Internal:
                throw new GdxRuntimeException( "Cannot move an internal file: " + file );

            case Absolute:
            case External:
                // Try rename for efficiency and to change case on case-insensitive file systems.
                if ( file().renameTo( dest.file() ) ) return;

                break;
        }

        copyTo( dest );
        delete();
        if ( exists() && isDirectory() ) deleteDirectory();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public long Length()
    {
        if ( ( File != null )
             && ( ( Type == FileType.Classpath )
                  || ( ( Type == FileType.Internal ) && !System.IO.File.Exists( File.Name ) ) ) )
        {
            FileStream input  = Read();
            var        length = input.Length;

            input.Close();

            return length;
        }

        return File!.Length;
    }


    /** Returns the last modified time in milliseconds for this file. Zero is returned if the file doesn't exist. Zero is returned
	 * for {@link FileType#Classpath} files. On Android, zero is returned for {@link FileType#Internal} files. On the desktop, zero
	 * is returned for {@link FileType#Internal} files on the classpath. */
    public long LastModified()
    {
        return file().lastModified();
    }

    public bool Equals( Object obj )
    {
        if ( !( obj instanceof FileHandle)) return false;
        FileHandle             other = ( FileHandle )obj;

        return type == other.type && path().equals( other.path() );
    }

    public int HashCode()
    {
        int hash = 1;
        hash = hash * 37 + type.hashCode();
        hash = hash * 67 + path().hashCode();

        return hash;
    }

    public new string ToString()
    {
        return file.getPath().replace( '\\', '/' );
    }

    public static FileHandle TempFile( string prefix )
    {
        try
        {
            return new FileHandle( File.createTempFile( prefix, null ) );
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( "Unable to create temp file.", ex );
        }
    }

    public static FileHandle TempDirectory( string prefix )
    {
        try
        {
            File file = File.createTempFile( prefix, null );

            if ( !file.delete() ) throw new IOException( "Unable to delete temp file: " + file );
            if ( !file.mkdir() ) throw new IOException( "Unable to create temp directory: " + file );

            return new FileHandle( file );
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( "Unable to create temp file.", ex );
        }
    }

    private static void EmptyDirectory( FileStream file, bool preserveTree )
    {
        if ( System.IO.File.Exists( file.Name ) )
        {
            var fileNames = Directory.GetFiles( file.Name );

            if ( fileNames.Length > 0 )
            {
                var files = new FileStream[ fileNames.Length ];
                var ix    = 0;

                foreach ( var name in fileNames )
                {
                    files[ ix++ ] = new FileStream( name, FileMode.Open );
                }

                for ( int i = 0, n = files.Length; i < n; i++ )
                {
                    if ( Directory.Exists( files[ i ].Name ) )
                    {
                        Directory.Delete( files[ i ].Name );
                    }
                    else if ( preserveTree )
                    {
                        EmptyDirectory( files[ i ], true );
                    }
                    else
                    {
                        DeleteDirectory( files[ i ] );
                    }
                }
            }
        }
    }

    private static bool DeleteDirectory( FileStream file )
    {
        EmptyDirectory( file, false );

        return file.Delete();
    }

    private static void CopyFile( FileHandle source, FileHandle dest )
    {
        try
        {
            dest.Write( source.Read(), false );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException
                (
                 "Error copying source file: "
                 + source.File
                 + " ("
                 + source.Type
                 + ")\n" //
                 + "To destination: "
                 + dest.File
                 + " ("
                 + dest.Type
                 + ")",
                 ex
                );
        }
    }

    private static void CopyDirectory( FileHandle sourceDir, FileHandle destDir )
    {
        Directory.CreateDirectory( destDir.Path() );
        FileHandle[] files = sourceDir.List();

        for ( int i = 0, n = files.Length; i < n; i++ )
        {
            FileHandle srcFile  = files[ i ];
            FileHandle destFile = destDir.Child( srcFile.Name() );

            if ( System.IO.Directory.Exists( srcFile.Name() ) )
            {
                CopyDirectory( srcFile.Name()!, destFile.Name()!, true );
            }
            else
            {
                CopyFile( srcFile, destFile );
            }
        }
    }

    private static void CopyDirectory( string sourceDir, string destinationDir, bool recursive )
    {
        // Get information about the source directory
        var dir = new DirectoryInfo( sourceDir );

        // Check if the source directory exists
        if ( !dir.Exists )
        {
            throw new DirectoryNotFoundException( $"Source directory not found: {dir.FullName}" );
        }

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory( destinationDir );

        // Get the files in the source directory and copy to the destination directory
        foreach ( FileInfo file in dir.GetFiles() )
        {
            var targetFilePath = System.IO.Path.Combine( destinationDir, file.Name );
            file.CopyTo( targetFilePath );
        }

        // If recursive and copying subdirectories, recursively call this method
        if ( recursive )
        {
            foreach ( DirectoryInfo subDir in dirs )
            {
                var newDestinationDir = System.IO.Path.Combine( destinationDir, subDir.Name );
                CopyDirectory( subDir.FullName, newDestinationDir, true );
            }
        }
    }
}