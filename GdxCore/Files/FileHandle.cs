using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LibGDXSharp.Files;

/// <summary>
/// Represents a file or directory on the filesystem, classpath, app storage
/// or assets directory. FileHandles are created via a Files instance.
/// Because some of the file types are backed by composite files and may be
/// compressed (for example, if they are found via the classpath), the methods
/// for extracting a path() or file() may not be appropriate for all types. Use
/// the Reader or Stream methods here to hide these dependencies from your
/// platform independent code.
/// </summary>
// -------------------------------------------------------------------
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" )]
// -------------------------------------------------------------------
public class FileHandle
{
    public FileType Type { get; set; }
    public File?    File { get; set; }

    /// <summary>
    /// Creates a new absolute FileHandle for the file name.
    /// </summary>
    public FileHandle( string fileName )
    {
        this.Type = FileType.Absolute;
        this.File = new File( fileName );
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    public FileHandle( File file )
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
        this.File = new File( fileName );
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="type"></param>
    public FileHandle( File file, FileType type )
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
    public string? Path() => File?.GetPath().Replace( '\\', '/' );

    /// <summary>
    /// Returns the name of the file, without any parent paths.
    /// </summary>
    /// <returns></returns>
    public string? Name() => File?.GetName();

    /// <summary>
    /// Returns the file extension (without the dot) or an empty
    /// string if the file name doesn't contain a dot.
    /// </summary>
    public string Extension()
    {
        var name = File?.GetName();

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

        var fileName = this.File.GetName();
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

        var filePath = this.File.GetPath();
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

        if ( File.IsDirectory )
        {
            throw new GdxRuntimeException( "Cannot open a stream to a Directory!" );
        }

        return new FileStream( File.GetName(), FileMode.OpenOrCreate );
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

    /// <summary>
    /// Attempts to memory map this file in READ_ONLY mode.
    /// </summary>
    public ByteBuffer Map() => Map( MapMode.ReadOnly );

    /// <summary>
    /// Attempts to memory map this file. Android files must not be compressed.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// if this file handle represents a directory, doesn't exist, or could not
    /// be read, or memory mapping fails, or is a <see cref="FileType.Classpath"/>
    /// file.
    /// </exception>
    public ByteBuffer Map( FileChannel.MapMode mode )
    {
        if ( Type == FileType.Classpath )
        {
            throw new GdxRuntimeException( "Cannot map a classpath file: " + this );
        }

        RandomAccessFile raf = null;

        try
        {
            raf = new RandomAccessFile( File, mode == MapMode.READ_ONLY ? "r" : "rw" );

            FileChannel fileChannel = raf.getChannel();
            ByteBuffer  map         = fileChannel.map( mode, 0, File?.Length );

            map.Order( ByteOrder.NativeOrder() );

            return map;
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error memory mapping file: " + this + " (" + type + ")", ex );
        }
        finally
        {
            StreamUtils.CloseQuietly( raf );
        }
    }

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
            return System.IO.File.OpenWrite( File.FullName );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error Writing File: " + File + " (" + Type + ")", ex );
        }
    }

    public BufferedStream Write( bool append, int bufferSize )
    {
        var stream = Write( append );

        return new BufferedStream( stream, bufferSize );
    }

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


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public long Length()
    {
        if ( ( File != null )
             && ( ( Type == FileType.Classpath ) || ( ( Type == FileType.Internal ) && !this.File.Exists ) ) )
        {
            FileStream input  = Read();
            var        length = input.Length;

            input.Close();

            return length;
        }

        return File!.Length;
    }
}
