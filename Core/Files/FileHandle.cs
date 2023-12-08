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

[PublicAPI]
public class FileHandle
{
    public FileType Type     { get; set; }
    public FileInfo FileInfo { get; set; }

    /// <summary>
    /// </summary>
    protected FileHandle()
    {
        this.FileInfo = default( FileInfo )!;
        this.Type = default( FileType );
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    public FileHandle( string fileName )
    {
        this.FileInfo = new FileInfo( fileName );
        this.Type = FileType.Absolute;
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    public FileHandle( FileInfo file )
    {
        ArgumentNullException.ThrowIfNull( file );

        this.FileInfo = file;
        this.Type = FileType.Absolute;
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public FileHandle( string fileName, FileType type )
    {
        this.FileInfo = new FileInfo( fileName );
        this.Type = type;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual string Path() => this.FileInfo.FullName;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual string Name() => this.FileInfo.Name;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual string Extension() => this.FileInfo.Extension;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual string NameWithoutExtension()
    {
        return System.IO.Path.GetFileNameWithoutExtension( FileInfo.Name );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual string PathWithoutExtension()
    {
        var filePath = this.FileInfo.FullName;

        var dotIndex = filePath.LastIndexOf( ".", StringComparison.Ordinal );

        if ( dotIndex == -1 )
        {
            return filePath;
        }

        return filePath.Substring( 0, dotIndex );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public virtual FileStream Read()
    {
        if ( ( this.Type == FileType.Classpath )
          || ( ( this.Type == FileType.Internal ) && !this.FileInfo.Exists )
          || ( ( this.Type == FileType.Local ) && !this.FileInfo.Exists ) )
        {
            try
            {
                FileStream input = File.OpenRead( FileInfo.FullName );

                return input;
            }
            catch ( System.Exception ex )
            {
                throw new GdxRuntimeException( $"File not found: {FileInfo.Name} ({Type})", ex );
            }
        }

        return File.OpenRead( FileInfo.FullName );
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public virtual BufferedStream Read( int bufferSize )
    {
        return new BufferedStream( File.OpenRead( FileInfo.FullName ), bufferSize );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual StreamReader Reader() => new( Read() );

    /// <summary>
    /// </summary>
    /// <param name="charset"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public virtual StreamReader Reader( Encoding charset )
    {
        FileStream stream = Read();

        try
        {
            return new StreamReader( stream, charset );
        }
        catch ( System.Exception ex )
        {
            stream.Close();

            throw new GdxRuntimeException( $"Error reading file: {this}", ex );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public virtual BufferedStream Reader( int bufferSize )
    {
        return Read( bufferSize );
    }

    /// <summary>
    /// </summary>
    /// <param name="bufferSize"></param>
    /// <param name="charset"></param>
    /// <returns></returns>
    public virtual BufferedStream Reader( int bufferSize, Encoding charset )
    {
        return Reader( bufferSize );
    }

    /// <summary>
    /// </summary>
    /// <param name="charset"></param>
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
        catch ( System.Exception ex )
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
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
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
        catch ( System.Exception ex )
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
        if ( ( Type == FileType.Classpath )
          || ( ( Type == FileType.Internal ) && !FileInfo.Exists ) )
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
        catch ( System.Exception ex )
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
        if ( Type == FileType.Classpath )
        {
            throw new GdxRuntimeException( $"Cannot write to a classpath file: {FileInfo.Name}", null );
        }

        if ( Type == FileType.Internal )
        {
            throw new GdxRuntimeException( $"Cannot write to an internal file: {FileInfo.Name}", null );
        }

        try
        {
            return System.IO.File.OpenWrite( FileInfo.FullName );
        }
        catch ( System.Exception ex )
        {
            throw new GdxRuntimeException( $"Error Writing File: {FileInfo.Name} ({Type})", ex );
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
        catch ( System.Exception ex )
        {
            throw new GdxRuntimeException( $"Error stream writing to file: {FileInfo.Name} ({Type})", ex );
        }
        finally
        {
            output?.Close();
            input.Close();
        }
    }
}
