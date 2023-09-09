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

using System.IO.Hashing;

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using LibGDXSharp.Utils;
using LibGDXSharp.Utils.Buffers;

namespace LibGDXSharp.Graphics;

/// <summary>
/// Write Pixmaps to various formats.
/// </summary>
[PublicAPI]
public static class PixmapIO
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
    /// Default is <see cref="Deflater.DEFAULT_COMPRESSION"/>
    /// </param>
    /// <param name="flipY">Flips the Pixmap vertically if true</param>
    public static void WritePNG( FileInfo file,
                                 Pixmap pixmap,
                                 int compression = Deflater.DEFAULT_COMPRESSION,
                                 bool flipY = false )
    {
        try
        {
            // Guess at deflated size.
            var writer = new PNG( ( int )( pixmap.Width * pixmap.Height * 1.5f ) );

            try
            {
                writer.FlipY = flipY;
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

    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    private static class CIM
    {
        private const int BUFFER_SIZE = 32000;

        private readonly static byte[] WriteBuffer = new byte[ BUFFER_SIZE ];
        private readonly static byte[] ReadBuffer  = new byte[ BUFFER_SIZE ];

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pixmap"></param>
        /// <exception cref="GdxRuntimeException"></exception>
        public static void Write( FileInfo file, Pixmap pixmap )
        {
            try
            {
                var deflaterOutputStream = new DeflaterOutputStream( file.OpenWrite() );
                var output = new BinaryWriter( deflaterOutputStream );

                output.Write( pixmap.Width );
                output.Write( pixmap.Height );
                output.Write( PixmapFormat.ToGdx2DPixmapFormat( pixmap.GetFormat() ) );

                ByteBuffer pixelBuf = pixmap.Pixels;

                pixelBuf.Position = 0;
                pixelBuf.Limit    = pixelBuf.Capacity;

                var remainingBytes = pixelBuf.Capacity % BUFFER_SIZE;
                var iterations     = pixelBuf.Capacity / BUFFER_SIZE;

                lock ( WriteBuffer )
                {
                    for ( var i = 0; i < iterations; i++ )
                    {
                        pixelBuf.Get( WriteBuffer );
                        output.Write( WriteBuffer );
                    }

                    pixelBuf.Get( WriteBuffer, 0, remainingBytes );
                    output.Write( WriteBuffer, 0, remainingBytes );
                }

                pixelBuf.Position = 0;
                pixelBuf.Limit    = pixelBuf.Capacity;
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( "Couldn't write Pixmap to file '" + file + "'", e );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="GdxRuntimeException"></exception>
        public static Pixmap Read( FileInfo file )
        {
            try
            {
                // long start = System.nanoTime();
                var input = new BinaryReader( new InflaterInputStream( file.OpenRead() ) );

                var width  = input.Read();
                var height = input.Read();

                Pixmap.Format format = PixmapFormat.FromGdx2DPixmapFormat( input.Read() );

                var pixmap = new Pixmap( width, height, format );

                ByteBuffer pixelBuf = pixmap.Pixels;
                pixelBuf.Position = 0;
                pixelBuf.Limit    = pixelBuf.Capacity;

                lock ( ReadBuffer )
                {
                    int readBytes;

                    while ( ( readBytes = input.Read( ReadBuffer ) ) > 0 )
                    {
                        pixelBuf.Put( ReadBuffer, 0, readBytes );
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
        }
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Paeth filter - a filtering algorithm used in the compression of PNG images
    /// </summary>
    [PublicAPI]
    public class PNG : IDisposable
    {
        private const int  IHDR                = 0x49484452;
        private const int  IDAT                = 0x49444154;
        private const int  IEND                = 0x49454E44;
        private const byte COLOR_ARGB          = 6;
        private const byte COMPRESSION_DEFLATE = 0;
        private const byte FILTER_NONE         = 0;
        private const byte INTERLACE_NONE      = 0;
        private const byte PAETH_FILTER        = 4;

        private readonly byte[]      _signature = { 137, 80, 78, 71, 13, 10, 26, 10 };
        private readonly Deflater    _deflater;
        private readonly ChunkBuffer _buffer;

        private List< byte >? _lineOutBytes;
        private List< byte >? _curLineBytes;
        private List< byte >? _prevLineBytes;
        private int           _lastLineLen;

        /// <summary>
        /// </summary>
        /// <param name="initialBufferSize"></param>
        public PNG( int initialBufferSize = 128 * 128 )
        {
            _buffer   = new ChunkBuffer( initialBufferSize );
            _deflater = new Deflater();
        }

        /// <summary>
        /// If true, the resulting PNG is flipped vertically. Default is true.
        /// </summary>
        public bool FlipY { get; set; }

        /// <summary>
        /// Sets the deflate compression level.
        /// Default is <see cref="Deflater.DEFAULT_COMPRESSION"/>. 
        /// </summary>
        public void SetCompression( int level ) => _deflater.SetLevel( level );

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pixmap"></param>
        public void Write( FileInfo file, Pixmap pixmap )
        {
            var output = new MemoryStream();

            try
            {
                Write( output, pixmap );
            }
            catch ( IOException )
            {
            }
        }

        /// <summary>
        /// Writes the pixmap to the stream without closing the stream.
        /// </summary>
        public void Write( Stream output, Pixmap pixmap )
        {
            var deflaterOutput = new DeflaterOutputStream( output/*_buffer*/, _deflater );
            var dataOutput     = new BinaryWriter( output );

            dataOutput.Write( _signature );

            _buffer.Write( IHDR );
            _buffer.Write( pixmap.Width );
            _buffer.Write( pixmap.Height );       // 
            _buffer.Write( 8 );                   // 8 bits per component.
            _buffer.Write( COLOR_ARGB );          // 
            _buffer.Write( COMPRESSION_DEFLATE ); // 
            _buffer.Write( FILTER_NONE );
            _buffer.Write( INTERLACE_NONE );
            _buffer.EndChunk( dataOutput );
            _buffer.Write( IDAT );

            _deflater.Reset();

            var lineLen = pixmap.Width * 4;

            byte[] lineOut;
            byte[] curLine;
            byte[] prevLine;

            if ( _lineOutBytes == null )
            {
                lineOut  = ( _lineOutBytes  = new List< byte >( lineLen ) ).ToArray();
                curLine  = ( _curLineBytes  = new List< byte >( lineLen ) ).ToArray();
                prevLine = ( _prevLineBytes = new List< byte >( lineLen ) ).ToArray();
            }
            else
            {
                _lineOutBytes.EnsureCapacity( lineLen );
                _curLineBytes!.EnsureCapacity( lineLen );
                _prevLineBytes!.EnsureCapacity( lineLen );

                lineOut  = _lineOutBytes.ToArray();
                curLine  = _curLineBytes.ToArray();
                prevLine = _prevLineBytes.ToArray();

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
                var py = FlipY ? ( h - y - 1 ) : y;

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

                deflaterOutput.Write( new ReadOnlySpan< byte >( PAETH_FILTER ) );
                deflaterOutput.Write( lineOut, 0, lineLen );

                ( curLine, prevLine ) = ( prevLine, curLine );
            }

            pixmap.Pixels.Position = oldPosition;

            deflaterOutput.Finish();
            _buffer.EndChunk( dataOutput );

            _buffer.Write( IEND );
            _buffer.EndChunk( dataOutput );

            output.Flush();
        }

        // --------------------------------------------------------------------

        public class ChunkBuffer : BinaryWriter
        {
            private readonly MemoryStream _buffer;
            private readonly Crc32        _crc;

            public ChunkBuffer( int initialSize )
                : this( new MemoryStream( initialSize ), new Crc32() )
            {
            }

            private ChunkBuffer( MemoryStream buffer, Crc32 crc )
                : base( new MemoryStream()/*new CheckedOutputStream( buffer, crc )*/ )
            {
                this._buffer = buffer;
                this._crc    = crc;
            }

            public void EndChunk( BinaryWriter target )
            {
                Flush();

                target.Write( _buffer.Length - 4 );
                target.Write( _buffer.ToArray() );
                target.Write( _crc.GetCurrentHash() );

                _buffer.Position = 0;
                _crc.Reset();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _deflater.Finish();
        }
    }
}