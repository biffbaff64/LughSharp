using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.DevFillers;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Graphics;

/// <summary>
/// Write Pixmaps to various formats.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class PixmapIO
{
    /// <summary>
    /// Writes the <see cref="Pixmap"/> to the given file using a custom compression
    /// scheme. First three integers define the width, height and format, remaining
    /// bytes are zlib compressed pixels. To be able to load the Pixmap to a Texture,
    /// use ".cim" as the file suffix.
    /// <para>
    /// Throws a GdxRuntimeException if the Pixmap couldn't be written to the file.
    /// </para>
    /// </summary>
    /// <param name="file">The file to write the Pixmap to.</param>
    /// <param name="pixmap"></param>
    public static void WriteCIM( FileInfo file, Pixmap pixmap )
    {
        CIM.Write( file, pixmap );
    }

    /// <summary>
    /// Reads the <see cref="Pixmap"/> from the given file, assuming the Pixmap was
    /// written with the <see cref="PixmapIO.WriteCIM(FileInfo, Pixmap)"/> method.
    /// <para>
    /// Throws a GdxRuntimeException in case the file couldn't be read.
    /// </para>
    /// </summary>
    /// <param name="file"> the file to read the Pixmap from  </param>
    public static Pixmap ReadCIM( FileInfo file )
    {
        return CIM.Read( file );
    }

    /// <summary>
    /// Writes the pixmap as a PNG. See <see cref="PNG"/> to write out multiple PNGs
    /// with minimal allocation.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="pixmap"></param>
    /// <param name="compression">
    /// Sets the deflate compression level. Default is <see cref="Deflater.Default_Compression"/>
    /// </param>
    /// <param name="flipY">Flips the Pixmap vertically if true</param>
    public static void WritePNG( FileInfo file,
                                 Pixmap pixmap,
                                 int compression = Deflater.Default_Compression,
                                 bool flipY = false )
    {
        try
        {
            // Guess at deflated size.
            var writer = new PNG( ( int )( pixmap.Width * pixmap.Height * 1.5f ) );

            try
            {
                writer.setFlipY( flipY );
                writer.setCompression( compression );
                writer.Write( file, pixmap );
            }
            finally
            {
                writer.Dispose();
            }
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( "Error writing PNG: " + file, ex );
        }
    }

    private class CIM
    {
        private const int BufferSize = 32000;

        private readonly static byte[] _writeBuffer = new byte[ BufferSize ];
        private readonly static byte[] _readBuffer  = new byte[ BufferSize ];

        public static void Write( FileInfo file, Pixmap pixmap )
        {
            DataOutputStream output = null;

            try
            {
                // long start = System.nanoTime();
                DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream( file.write( false ) );

                output = new DataOutputStream( deflaterOutputStream );
                output.writeInt( pixmap.Width );
                output.writeInt( pixmap.Height );
                output.writeInt( PixmapFormat.ToGdx2DPixmapFormat( pixmap.GetFormat() ) );

                ByteBuffer pixelBuf = pixmap.Pixels;
                pixelBuf.Position = 0;
                pixelBuf.Limit    = pixelBuf.Capacity;

                var remainingBytes = pixelBuf.Capacity % BufferSize;
                var iterations     = pixelBuf.Capacity / BufferSize;

                lock ( _writeBuffer )
                {
                    for ( var i = 0; i < iterations; i++ )
                    {
                        pixelBuf.Get( _writeBuffer );
                        output.write( _writeBuffer );
                    }

                    pixelBuf.Get( _writeBuffer, 0, remainingBytes );
                    output.Write( _writeBuffer, 0, remainingBytes );
                }

                pixelBuf.Position = 0;
                pixelBuf.Limit    = pixelBuf.Capacity;
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( "Couldn't write Pixmap to file '" + file + "'", e );
            }
            finally
            {
                StreamUtils.CloseQuietly( output );
            }
        }

        public static Pixmap Read( FileInfo file )
        {
            DataInputStream input = null;

            try
            {
                // long start = System.nanoTime();
                input = new DataInputStream( new InflaterInputStream( new BufferedInputStream( file.Read() ) ) );

                int width  = input.readInt();
                int height = input.readInt();

                Pixmap.Format format = PixmapFormat.FromGdx2DPixmapFormat( input.readInt() );

                var pixmap = new Pixmap( width, height, format );

                ByteBuffer pixelBuf = pixmap.Pixels;
                pixelBuf.Position = 0;
                pixelBuf.Limit    = pixelBuf.Capacity;

                lock ( _readBuffer )
                {
                    var readBytes = 0;

                    while ( ( readBytes = input.read( _readBuffer ) ) > 0 )
                    {
                        pixelBuf.Put( _readBuffer, 0, readBytes );
                    }
                }

                pixelBuf.Position = 0;
                pixelBuf.Limit    = pixelBuf.Capacity;

                return pixmap;
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( "Couldn't read Pixmap from file '" + file + "'", e );
            }
            finally
            {
                StreamUtils.CloseQuietly( input );
            }
        }
    }

    public class PNG : IDisposable
    {
        private const int  IHDR                = 0x49484452;
        private const int  IDAT                = 0x49444154;
        private const int  IEND                = 0x49454E44;
        private const byte COLOR_ARGB          = 6;
        private const byte COMPRESSION_DEFLATE = 0;
        private const byte FILTER_NONE         = 0;
        private const byte INTERLACE_NONE      = 0;
        private const byte PAETH               = 4;

        private readonly byte[] SIGNATURE = { 137, 80, 78, 71, 13, 10, 26, 10 };

        private readonly Deflater    deflater;
        private readonly ChunkBuffer buffer;

        private List< byte >? lineOutBytes;
        private List< byte >? curLineBytes;
        private List< byte >? prevLineBytes;
        private bool          flipY = true;
        private int           lastLineLen;

        public PNG()
            : this( 128 * 128 )
        {
        }

        public PNG( int initialBufferSize )
        {
            buffer   = new ChunkBuffer( initialBufferSize );
            deflater = new Deflater();
        }

        /// <summary>
        /// If true, the resulting PNG is flipped vertically. Default is true.
        /// </summary>
        public void setFlipY( bool flip )
        {
            this.flipY = flip;
        }

        /// <summary>
        /// Sets the deflate compression level.
        /// Default is <see cref="Deflater.Default_Compression"/>. 
        /// </summary>
        public void setCompression( int level )
        {
            deflater.SetLevel( level );
        }

        public void Write( FileInfo file, Pixmap pixmap )
        {
            OutputStream output = file.Write( false );

            try
            {
                Write( output, pixmap );
            }
            finally
            {
                StreamUtils.CloseQuietly( output );
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Writes the pixmap to the stream without closing the stream.
        /// </summary>
        public void Write( OutputStream output, Pixmap pixmap )
        {
            var deflaterOutput = new DeflaterOutputStream( buffer, deflater );
            var dataOutput     = new DataOutputStream( output );

            dataOutput.write( SIGNATURE );

            buffer.WriteInt( IHDR );
            buffer.WriteInt( pixmap.Width );
            buffer.WriteInt( pixmap.Height );
            buffer.WriteByte( 8 ); // 8 bits per component.
            buffer.WriteByte( COLOR_ARGB );
            buffer.WriteByte( COMPRESSION_DEFLATE );
            buffer.WriteByte( FILTER_NONE );
            buffer.WriteByte( INTERLACE_NONE );
            buffer.EndChunk( dataOutput );
            buffer.WriteInt( IDAT );

            deflater.Reset();

            var lineLen = pixmap.Width * 4;

            byte[] lineOut  = default!;
            byte[] curLine  = default!;
            byte[] prevLine = default!;

            if ( lineOutBytes == null )
            {
                lineOut  = ( lineOutBytes  = new List< byte >( lineLen ) ).ToArray();
                curLine  = ( curLineBytes  = new List< byte >( lineLen ) ).ToArray();
                prevLine = ( prevLineBytes = new List< byte >( lineLen ) ).ToArray();
            }
            else
            {
                ArrayUtils.EnsureCapacity( ref lineOut, lineLen );
                ArrayUtils.EnsureCapacity( ref curLine, lineLen );
                ArrayUtils.EnsureCapacity( ref prevLine, lineLen );

//                lineOut  = lineOutBytes.EnsureCapacity( lineLen );
//                curLine  = curLineBytes.EnsureCapacity( lineLen );
//                prevLine = prevLineBytes.EnsureCapacity( lineLen );

                for ( int i = 0, n = lastLineLen; i < n; i++ )
                {
                    prevLine[ i ] = 0;
                }
            }

            lastLineLen = lineLen;

            var oldPosition = pixmap.Pixels.Position;
            var isRgba8888  = pixmap.GetFormat() == Pixmap.Format.RGBA8888;

            for ( int y = 0, h = pixmap.Height; y < h; y++ )
            {
                var py = flipY ? ( h - y - 1 ) : y;

                if ( isRgba8888 )
                {
                    pixmap.Pixels.Position = ( py * lineLen );
                    pixmap.Pixels.Get( curLine, 0, lineLen );
                }
                else
                {
                    for ( int px = 0, x = 0; px < pixmap.Width; px++ )
                    {
                        var pixel = pixmap.GetPixel( px, py );

                        curLine[ x++ ] = ( byte )( ( pixel >> 24 ) & 0xff );
                        curLine[ x++ ] = ( byte )( ( pixel >> 16 ) & 0xff );
                        curLine[ x++ ] = ( byte )( ( pixel >> 8 ) & 0xff );
                        curLine[ x++ ] = ( byte )( pixel & 0xff );
                    }
                }

                lineOut[ 0 ] = ( byte )( curLine[ 0 ] - prevLine[ 0 ] );
                lineOut[ 1 ] = ( byte )( curLine[ 1 ] - prevLine[ 1 ] );
                lineOut[ 2 ] = ( byte )( curLine[ 2 ] - prevLine[ 2 ] );
                lineOut[ 3 ] = ( byte )( curLine[ 3 ] - prevLine[ 3 ] );

                for ( var x = 4; x < lineLen; x++ )
                {
                    var a  = curLine[ x - 4 ] & 0xff;
                    var b  = prevLine[ x ] & 0xff;
                    var c  = prevLine[ x - 4 ] & 0xff;
                    var p  = ( a + b ) - c;
                    var pa = p - a;

                    if ( pa < 0 ) pa = -pa;

                    var pb = p - b;

                    if ( pb < 0 ) pb = -pb;

                    var pc = p - c;

                    if ( pc < 0 ) pc = -pc;

                    if ( ( pa <= pb ) && ( pa <= pc ) )
                    {
                        c = a;
                    }
                    else if ( pb <= pc )
                    {
                        c = b;
                    }

                    lineOut[ x ] = ( byte )( curLine[ x ] - c );
                }

                deflaterOutput.write( PAETH );
                deflaterOutput.write( lineOut, 0, lineLen );

                ( curLine, prevLine ) = ( prevLine, curLine );
            }

            pixmap.Pixels.Position = oldPosition;

            deflaterOutput.finish();
            buffer.EndChunk( dataOutput );

            buffer.WriteInt( IEND );
            buffer.EndChunk( dataOutput );

            output.flush();
        }

        public void dispose()
        {
            deflater.End();
        }

        public sealed class ChunkBuffer : DataOutputStream
        {
            private ByteArrayOutputStream buffer;
            private CRC32                 crc;

            public ChunkBuffer( int initialSize )
                : this( new ByteArrayOutputStream( initialSize ), new CRC32() )
            {
            }
            
            private ChunkBuffer( ByteArrayOutputStream buffer, CRC32 crc )
                : base( new CheckedOutputStream( buffer, crc ) )
            {
                this.buffer = buffer;
                this.crc    = crc;
            }

            public void EndChunk( DataOutputStream target )
            {
                Flush();

                target.WriteInt( buffer.size() - 4 );
                buffer.WriteTo( target );

                target.WriteInt( ( int )crc.getValue() );
                buffer.Reset();

                crc.Reset();
            }
        }
    }
}
