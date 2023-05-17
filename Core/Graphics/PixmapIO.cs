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
    /// Sets the deflate compression level.
    /// Default is <see cref="Deflater.Default_Compression"/>
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
                writer.SetFlipY( flipY );
                writer.SetCompression( compression );
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

        private readonly static byte[] writeBuffer = new byte[ BufferSize ];
        private readonly static byte[] readBuffer  = new byte[ BufferSize ];

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

                lock ( writeBuffer )
                {
                    for ( var i = 0; i < iterations; i++ )
                    {
                        pixelBuf.Get( writeBuffer );
                        output.write( writeBuffer );
                    }

                    pixelBuf.Get( writeBuffer, 0, remainingBytes );
                    output.Write( writeBuffer, 0, remainingBytes );
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

                lock ( readBuffer )
                {
                    var readBytes = 0;

                    while ( ( readBytes = input.read( readBuffer ) ) > 0 )
                    {
                        pixelBuf.Put( readBuffer, 0, readBytes );
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

    /// <summary>
    /// </summary>
    /// <remarks>
    /// Paeth filter - a filtering algorithm used in the compression of PNG images
    /// </remarks>
    public sealed class PNG : IDisposable
    {
        private const int  Ihdr                = 0x49484452;
        private const int  Idat                = 0x49444154;
        private const int  Iend                = 0x49454E44;
        private const byte Color_ARGB          = 6;
        private const byte Compression_Deflate = 0;
        private const byte Filter_None         = 0;
        private const byte Interlace_None      = 0;
        private const byte Paeth               = 4;

        private readonly byte[] _signature = { 137, 80, 78, 71, 13, 10, 26, 10 };

        private readonly Deflater    _deflater;
        private readonly ChunkBuffer _buffer;

        private List< byte >? _lineOutBytes;
        private List< byte >? _curLineBytes;
        private List< byte >? _prevLineBytes;
        private bool          _flipY = true;
        private int           _lastLineLen;

        public PNG()
            : this( 128 * 128 )
        {
        }

        public PNG( int initialBufferSize )
        {
            _buffer   = new ChunkBuffer( initialBufferSize );
            _deflater = new Deflater();
        }

        /// <summary>
        /// If true, the resulting PNG is flipped vertically. Default is true.
        /// </summary>
        public void SetFlipY( bool flip )
        {
            this._flipY = flip;
        }

        /// <summary>
        /// Sets the deflate compression level.
        /// Default is <see cref="Deflater.Default_Compression"/>. 
        /// </summary>
        public void SetCompression( int level )
        {
            _deflater.SetLevel( level );
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
            _deflater.End();
        }

        /// <summary>
        /// Writes the pixmap to the stream without closing the stream.
        /// </summary>
        public void Write( OutputStream output, Pixmap pixmap )
        {
            var deflaterOutput = new DeflaterOutputStream( _buffer, _deflater );
            var dataOutput     = new DataOutputStream( output );

            dataOutput.write( _signature );

            _buffer.WriteInt( Ihdr );
            _buffer.WriteInt( pixmap.Width );
            _buffer.WriteInt( pixmap.Height );
            _buffer.WriteByte( 8 ); // 8 bits per component.
            _buffer.WriteByte( Color_ARGB );
            _buffer.WriteByte( Compression_Deflate );
            _buffer.WriteByte( Filter_None );
            _buffer.WriteByte( Interlace_None );
            _buffer.EndChunk( dataOutput );
            _buffer.WriteInt( Idat );

            _deflater.Reset();

            var lineLen = pixmap.Width * 4;

            byte[] lineOut  = default!;
            byte[] curLine  = default!;
            byte[] prevLine = default!;

            if ( _lineOutBytes == null )
            {
                lineOut  = ( _lineOutBytes  = new List< byte >( lineLen ) ).ToArray();
                curLine  = ( _curLineBytes  = new List< byte >( lineLen ) ).ToArray();
                prevLine = ( _prevLineBytes = new List< byte >( lineLen ) ).ToArray();
            }
            else
            {
                ArrayUtils.EnsureCapacity( ref lineOut, lineLen );  // lineOut  = lineOutBytes.EnsureCapacity( lineLen );
                ArrayUtils.EnsureCapacity( ref curLine, lineLen );  // curLine  = curLineBytes.EnsureCapacity( lineLen );
                ArrayUtils.EnsureCapacity( ref prevLine, lineLen ); // prevLine = prevLineBytes.EnsureCapacity( lineLen );

                for ( int i = 0, n = _lastLineLen; i < n; i++ )
                {
                    prevLine[ i ] = 0;
                }
            }

            _lastLineLen = lineLen;

            var oldPosition = pixmap.Pixels.Position;
            var isRgba8888  = pixmap.GetFormat() == Pixmap.Format.RGBA8888;

            for ( int y = 0, h = pixmap.Height; y < h; y++ )
            {
                var py = _flipY ? ( h - y - 1 ) : y;

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

                deflaterOutput.write( Paeth );
                deflaterOutput.write( lineOut, 0, lineLen );

                ( curLine, prevLine ) = ( prevLine, curLine );
            }

            pixmap.Pixels.Position = oldPosition;

            deflaterOutput.finish();
            _buffer.EndChunk( dataOutput );

            _buffer.WriteInt( Iend );
            _buffer.EndChunk( dataOutput );

            output.flush();
        }

        public sealed class ChunkBuffer : DataOutputStream
        {
            private ByteArrayOutputStream _buffer;
            private CRC32                 _crc;

            public ChunkBuffer( int initialSize )
                : this( new ByteArrayOutputStream( initialSize ), new CRC32() )
            {
            }
            
            private ChunkBuffer( ByteArrayOutputStream buffer, CRC32 crc )
                : base( new CheckedOutputStream( buffer, crc ) )
            {
                this._buffer = buffer;
                this._crc    = crc;
            }

            public void EndChunk( DataOutputStream target )
            {
                Flush();

                target.WriteInt( _buffer.size() - 4 );
                _buffer.WriteTo( target );

                target.WriteInt( ( int )_crc.getValue() );
                _buffer.Reset();

                _crc.Reset();
            }
        }
    }
}
