// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using System.IO.Hashing;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Exception = System.Exception;

namespace Corelib.LibCore.Graphics;

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
        Logger.Checkpoint();
        Logger.Debug( $"File {file.FullName}: File exists: {file.Exists}" );

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

        private static readonly byte[] _writeBuffer = new byte[ BUFFER_SIZE ];
        private static readonly byte[] _readBuffer  = new byte[ BUFFER_SIZE ];

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
                var output               = new BinaryWriter( deflaterOutputStream );

                output.Write( pixmap.Width );
                output.Write( pixmap.Height );
                output.Write( PixmapFormat.ToGdx2DFormat( pixmap.Format ) );

                var pixelBuf = pixmap.ByteBuffer;

                pixelBuf.Position = 0;
                pixelBuf.Limit     = pixelBuf.Capacity;

                var remainingBytes = pixelBuf.Capacity % BUFFER_SIZE;
                var iterations     = pixelBuf.Capacity / BUFFER_SIZE;

                lock ( _writeBuffer )
                {
                    for ( var i = 0; i < iterations; i++ )
                    {
                        pixelBuf.Get( _writeBuffer );
                        output.Write( _writeBuffer );
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
                var input    = new BinaryReader( new InflaterInputStream( file.OpenRead() ) );
                var width    = input.Read();
                var height   = input.Read();
                var format   = PixmapFormat.ToPixmapColorFormat( input.Read() );
                var pixmap   = new Pixmap( width, height, format );
                var pixelBuf = pixmap.ByteBuffer;

                pixelBuf.Position = 0;
                pixelBuf.Limit     = pixelBuf.Capacity;

                lock ( _readBuffer )
                {
                    int readBytes;

                    while ( ( readBytes = input.Read( _readBuffer ) ) > 0 )
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
        }
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Paeth filter - a filtering algorithm used in the compression of PNG images
    /// </summary>
    [PublicAPI]
    public class PNG : IDisposable
    {
        private const    int         IHDR                = 0x49484452;
        private const    int         IDAT                = 0x49444154;
        private const    int         IEND                = 0x49454E44;
        private const    byte        COLOR_ARGB          = 6;
        private const    byte        COMPRESSION_DEFLATE = 0;
        private const    byte        FILTER_NONE         = 0;
        private const    byte        INTERLACE_NONE      = 0;
        private const    byte        PAETH_FILTER        = 4;
        private readonly ChunkBuffer _buffer;
        private readonly Deflater    _deflater;

        private readonly byte[] _signature = [ 137, 80, 78, 71, 13, 10, 26, 10 ];

        // --------------------------------------------------------------------
        // --------------------------------------------------------------------

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
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _deflater.Finish();
        }

        /// <summary>
        /// Sets the deflate compression level.
        /// Default is <see cref="Deflater.DEFAULT_COMPRESSION"/>.
        /// </summary>
        public void SetCompression( int level )
        {
            _deflater.SetLevel( level );
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pixmap"></param>
        public void Write( FileInfo file, Pixmap pixmap )
        {
            Logger.Checkpoint();

            var output = new MemoryStream();

            try
            {
                Write( output, pixmap );

                Logger.Checkpoint();
            }
            catch ( IOException )
            {
            }
        }

        private int        _lastLineLen;
        private ByteArray? _curLineBytes;
        private ByteArray? _lineOutBytes;
        private ByteArray? _prevLineBytes;

        /// <summary>
        /// Writes the pixmap to the stream without closing the stream.
        /// </summary>
        public void Write( Stream output, Pixmap pixmap )
        {
            Logger.Checkpoint();
            
            var deflaterOutput = new DeflaterOutputStream( output /*_buffer*/, _deflater );
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
                _lineOutBytes  = new ByteArray( lineLen );
                _curLineBytes  = new ByteArray( lineLen );
                _prevLineBytes = new ByteArray( lineLen );

                lineOut  = new byte[ lineLen ];
                curLine  = new byte[ lineLen ];
                prevLine = new byte[ lineLen ];
            }
            else
            {
                _lineOutBytes.EnsureCapacity( lineLen );
                _curLineBytes!.EnsureCapacity( lineLen );
                _prevLineBytes!.EnsureCapacity( lineLen );

                lineOut  = _lineOutBytes.ToArray();
                curLine  = _curLineBytes.ToArray();
                prevLine = _prevLineBytes.ToArray();

                Array.Clear( prevLine, 0, _lastLineLen );
            }

            _lastLineLen = lineLen;

            var oldPosition = pixmap.ByteBuffer.Position;
            var isRgba8888  = pixmap.Format == Pixmap.ColorFormat.RGBA8888;

            for ( int y = 0, h = pixmap.Height; y < h; y++ )
            {
                var py = FlipY ? h - y - 1 : y;

                if ( isRgba8888 )
                {
                    pixmap.ByteBuffer.Position = py * lineLen;
                    pixmap.ByteBuffer.Get( curLine, 0, lineLen );
                }
                else
                {
                    for ( int px = 0, x = 0; px < pixmap.Width; px++ )
                    {
                        var pixel = pixmap.GetPixel( px, py );

                        curLine[ x++ ] = ( byte ) ( ( pixel >> 24 ) & 0xff );
                        curLine[ x++ ] = ( byte ) ( ( pixel >> 16 ) & 0xff );
                        curLine[ x++ ] = ( byte ) ( ( pixel >> 8 ) & 0xff );
                        curLine[ x++ ] = ( byte ) ( pixel & 0xff );
                    }
                }

                lineOut[ 0 ] = ( byte ) ( curLine[ 0 ] - prevLine[ 0 ] );
                lineOut[ 1 ] = ( byte ) ( curLine[ 1 ] - prevLine[ 1 ] );
                lineOut[ 2 ] = ( byte ) ( curLine[ 2 ] - prevLine[ 2 ] );
                lineOut[ 3 ] = ( byte ) ( curLine[ 3 ] - prevLine[ 3 ] );

                for ( var x = 4; x < lineLen; x++ )
                {
                    var a  = curLine[ x - 4 ] & 0xff;
                    var b  = prevLine[ x ] & 0xff;
                    var c  = prevLine[ x - 4 ] & 0xff;
                    var p  = ( a + b ) - c;
                    var pa = p - a;

                    if ( pa < 0 )
                    {
                        pa = -pa;
                    }

                    var pb = p - b;

                    if ( pb < 0 )
                    {
                        pb = -pb;
                    }

                    var pc = p - c;

                    if ( pc < 0 )
                    {
                        pc = -pc;
                    }

                    if ( ( pa <= pb ) && ( pa <= pc ) )
                    {
                        c = a;
                    }
                    else if ( pb <= pc )
                    {
                        c = b;
                    }

                    lineOut[ x ] = ( byte ) ( curLine[ x ] - c );
                }

                deflaterOutput.WriteByte( PAETH_FILTER );
                deflaterOutput.Write( lineOut, 0, lineLen );

                ( curLine, prevLine ) = ( prevLine, curLine );
            }

            pixmap.ByteBuffer.Position = oldPosition;

            deflaterOutput.Finish();
            _buffer.EndChunk( dataOutput );

            _buffer.Write( IEND );
            _buffer.EndChunk( dataOutput );

            output.Flush();
        }

        // --------------------------------------------------------------------

        [PublicAPI]
        public class ChunkBuffer : BinaryWriter
        {
            private readonly MemoryStream _buffer;
            private readonly Crc32        _crc;

            public ChunkBuffer( int initialSize )
                : this( new MemoryStream( initialSize ), new Crc32() )
            {
            }

            private ChunkBuffer( MemoryStream buffer, Crc32 crc )
                : base( new MemoryStream() /*new CheckedOutputStream( buffer, crc )*/ )
            {
                _buffer = buffer;
                _crc    = crc;
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
    }
}