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

[PublicAPI]
public class FileHandle
{
    public FileInfo FileInfo { get; set; }
    public FileType FileType { get; set; }

    /// <summary>
    ///     Creates a new absolute FileHandle for the file name. Use this for tools
    ///     on the desktop that don't need any of the backends.
    /// </summary>
    public FileHandle( string fileName )
    {
        FileInfo = new FileInfo( fileName );
        FileType = FileType.Absolute;
    }

    /// <summary>
    ///     Creates a new absolute FileHandle for the File. Use this for tools on the
    ///     desktop that don't need any of the backends.
    /// </summary>
    public FileHandle( FileInfo file )
    {
        ArgumentNullException.ThrowIfNull( file );

        FileInfo = file;
        FileType = FileType.Absolute;
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public FileHandle( string fileName, FileType type )
    {
        FileInfo = new FileInfo( fileName );
        FileType = type;
    }

    /// <summary>
    ///     The path of the file as specified on construction,
    ///     <para>
    ///         e.g. Gdx.files.internal("dir/file.png") -> dir/file.png.
    ///     </para>
    ///     <para>
    ///         Backward slashes will be replaced by forward slashes.
    ///     </para>
    /// </summary>
    public virtual string Path() => FileInfo.FullName;

    /// <summary>
    ///     The name of the file, without any parent paths.
    /// </summary>
    public virtual string Name() => FileInfo.Name;

    /// <summary>
    ///     Returns the file extension (without the dot) or an empty string
    ///     if the file name doesn't contain a dot.
    /// </summary>
    public virtual string Extension() => FileInfo.Extension;

    /// <summary>
    ///     The name of the file, without parent paths or the extension.
    /// </summary>
    public virtual string NameWithoutExtension()
    {
        return System.IO.Path.GetFileNameWithoutExtension( FileInfo.Name );
    }

    /// <summary>
    ///     The path and filename without the extension, e.g. dir/dir2/file.png -> dir/dir2/file.
    ///     Backward slashes will be returned as forward slashes.
    /// </summary>
    public virtual string PathWithoutExtension()
    {
        var filePath = FileInfo.FullName;

        var dotIndex = filePath.LastIndexOf( ".", StringComparison.Ordinal );

        return dotIndex == -1 ? filePath : filePath.Substring( 0, dotIndex );
    }

    /// <summary>
    ///     Returns a stream for reading this file as bytes.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    ///     if the file handle represents a directory, doesn't exist, or could not be read.
    /// </exception>
    public virtual FileStream Read()
    {
        if ( ( FileType == FileType.Classpath )
          || ( ( FileType == FileType.Internal ) && !FileInfo.Exists )
          || ( ( FileType == FileType.Local ) && !FileInfo.Exists ) )
        {
            try
            {
                FileStream input = File.OpenRead( FileInfo.FullName );

                return input;
            }
            catch ( Exception ex )
            {
                throw new GdxRuntimeException( $"File not found: {FileInfo.Name} ({FileType})", ex );
            }
        }

        return File.OpenRead( FileInfo.FullName );
    }

    /// <summary>
    ///     Returns a stream for reading this file as bytes.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    ///     if the file handle represents a directory, doesn't exist, or could not be read.
    /// </exception>
    public virtual BufferedStream Read( int bufferSize )
    {
        return new BufferedStream( File.OpenRead( FileInfo.FullName ), bufferSize );
    }

    /// <summary>
    ///     Returns a reader for reading this file as characters the platform's default charset.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    ///     if the file handle represents a directory, doesn't exist, or could not be read.
    /// </exception>
    public virtual StreamReader Reader() => new( Read() );

    /// <summary>
    /// </summary>
    /// <param name="charset"></param>
    /// <exception cref="GdxRuntimeException">
    ///     if the file handle represents a directory, doesn't exist, or could not be read.
    /// </exception>
    public virtual StreamReader Reader( Encoding charset )
    {
        FileStream stream = Read();

        try
        {
            return new StreamReader( stream, charset );
        }
        catch ( Exception ex )
        {
            stream.Close();

            throw new GdxRuntimeException( $"Error reading file: {this}", ex );
        }
    }

    /// <summary>
    ///     Returns a buffered reader for reading this file as characters
    ///     using the platform's default charset.
    /// </summary>
    public virtual BufferedStream Reader( int bufferSize ) => Read( bufferSize );

    /// <summary>
    ///     Returns a buffered reader for reading this file as characters.
    /// </summary>
    public virtual BufferedStream Reader( int bufferSize, Encoding charset ) => Reader( bufferSize );

    /// <summary>
    ///     Reads the entire file into a string using the specified charset.
    /// </summary>
    /// <param name="charset">If null, the default charset is used.</param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public virtual string ReadString( Encoding? charset = null )
    {
        var           output = new StringBuilder( EstimateLength() );
        StreamReader? reader = null;

        try
        {
            reader = charset == null
                ? new StreamReader( Read() )
                : new StreamReader( Read(), charset );

            output.Append( reader.ReadToEnd() );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( $"Error reading layout file - {this}", ex );
        }
        finally
        {
            reader?.Close();
        }

        return output.ToString();
    }

    /// <summary>
    ///     Reads the entire file into a byte array.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    ///     if the file handle represents a directory, doesn't exist, or could not be read.
    /// </exception>
    public virtual byte[] ReadBytes()
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
            throw new GdxRuntimeException( $"Error reading file: {this}", ex );
        }
        finally
        {
            input.Close();
        }
    }

    private int EstimateLength() => ( int )Length() != 0 ? ( int )Length() : 512;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual long Length()
    {
        if ( ( FileType == FileType.Classpath )
          || ( ( FileType == FileType.Internal ) && !FileInfo.Exists ) )
        {
            FileStream input  = Read();
            var        length = input.Length;

            input.Close();

            return length;
        }

        return FileInfo.Length;
    }

    /// <summary>
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public virtual int ReadBytes( byte[] bytes, int offset, int size )
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
            throw new GdxRuntimeException( $"Error Reading File: {this}", ex );
        }
        finally
        {
            input.Close();
        }

        return position - offset;
    }

    /// <summary>
    /// </summary>
    /// <param name="append"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public virtual FileStream Write( bool append )
    {
        if ( FileType == FileType.Classpath )
        {
            throw new GdxRuntimeException( $"Cannot write to a classpath file: {FileInfo.Name}", null );
        }

        if ( FileType == FileType.Internal )
        {
            throw new GdxRuntimeException( $"Cannot write to an internal file: {FileInfo.Name}", null );
        }

        try
        {
            return File.OpenWrite( FileInfo.FullName );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( $"Error Writing File: {FileInfo.Name} ({FileType})", ex );
        }
    }

    public virtual BufferedStream Write( bool append, int bufferSize )
    {
        FileStream stream = Write( append );

        return new BufferedStream( stream, bufferSize );
    }

    public virtual void Write( FileStream input, bool append )
    {
        FileStream? output = null;

        try
        {
            output = Write( append );

            output.CopyTo( input );
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( $"Error stream writing to file: {FileInfo.Name} ({FileType})", ex );
        }
        finally
        {
            output?.Close();
            input.Close();
        }
    }
}
