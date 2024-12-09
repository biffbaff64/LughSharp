// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils.Buffers;
using Corelib.Lugh.Utils.Exceptions;
using Exception = System.Exception;

namespace Corelib.Lugh.Graphics.GLUtils;

/// <summary>
/// Class for encoding and decoding ETC1 compressed images.
/// Also provides methods to add a PKM header.
/// </summary>
[PublicAPI]
public class ETC1
{
    /// <summary>
    /// The PKM header size in bytes
    /// </summary>
    public const int PKM_HEADER_SIZE = 16;

    public const int ETC1_RGB8_OES     = 0x00008d64;
    public const int RGB565_PIXEL_SIZE = 2;
    public const int RGB888_PIXEL_SIZE = 3;
    
    // ========================================================================

    /// <summary>
    /// Encodes the image via the ETC1 compression scheme.
    /// Only <see cref="Pixmap.ColorFormat.RGB565"/> and <see cref="Pixmap.ColorFormat.RGB888"/> are supported.
    /// </summary>
    /// <param name="pixmap"> the <see cref="Pixmap"/> </param>
    /// <returns> the <see cref="ETC1Data"/> </returns>
    public ETC1Data EncodeImage( Pixmap pixmap )
    {
        var pixelSize      = GetPixelSize( pixmap.Format );
        var compressedData = EncodeImage( pixmap.ByteBuffer!, 0, pixmap.Width, pixmap.Height, pixelSize );

//        BufferUtils.NewUnsafeByteBuffer( compressedData );

        return new ETC1Data( pixmap.Width, pixmap.Height, compressedData, 0, this );
    }

    /// <summary>
    /// Encodes the image via the ETC1 compression scheme.
    /// Only <see cref="Pixmap.ColorFormat.RGB565"/> and <see cref="Pixmap.ColorFormat.RGB888"/> are supported.
    /// Adds a PKM header in front of the compressed image data.
    /// </summary>
    /// <param name="pixmap"> the <see cref="Pixmap"/> </param>
    /// <returns> the <see cref="ETC1Data"/> </returns>
    public ETC1Data EncodeImagePKM( Pixmap pixmap )
    {
        var pixelSize      = GetPixelSize( pixmap.Format );
        var compressedData = EncodeImagePKM( pixmap.ByteBuffer, 0, pixmap.Width, pixmap.Height, pixelSize );

//        BufferUtils.NewUnsafeByteBuffer( compressedData );

        return new ETC1Data( pixmap.Width, pixmap.Height, compressedData, 16, this );
    }

    /// <summary>
    /// Takes ETC1 compressed image data and converts it to a <see cref="Pixmap.ColorFormat.RGB565"/> or
    /// <see cref="Pixmap.ColorFormat.RGB888"/> <see cref="Pixmap"/>.
    /// Does not modify the ByteBuffer's position or limit.
    /// </summary>
    /// <param name="etc1Data"> the <see cref="ETC1Data"/> instance </param>
    /// <param name="format"> either <see cref="Pixmap.ColorFormat.RGB565"/> or <see cref="Pixmap.ColorFormat.RGB888"/> </param>
    /// <returns> the Pixmap </returns>
    public Pixmap DecodeImage( ETC1Data? etc1Data, Pixmap.ColorFormat format )
    {
        ArgumentNullException.ThrowIfNull( etc1Data );

        int dataOffset;
        int width;
        int height;

        if ( etc1Data.HasPKMHeader() )
        {
            dataOffset = 16;
            width      = GetWidthPKM( etc1Data.CompressedData, 0 );
            height     = GetHeightPKM( etc1Data.CompressedData, 0 );
        }
        else
        {
            dataOffset = 0;
            width      = etc1Data.Width;
            height     = etc1Data.Height;
        }

        var pixelSize = GetPixelSize( format );
        var pixmap    = new Pixmap( width, height, format );

        DecodeImage( etc1Data.CompressedData, dataOffset, pixmap.ByteBuffer!, 0, width, height, pixelSize );

        return pixmap;
    }

    /// <summary>
    /// Gets the pixel size for the given <see cref="Pixmap.ColorFormat"/>, which must be
    /// one of <see cref="Pixmap.ColorFormat.RGB565"/> or <see cref="Pixmap.ColorFormat.RGB888"/>.
    /// </summary>
    private int GetPixelSize( Pixmap.ColorFormat? format )
    {
        if ( format == Pixmap.ColorFormat.RGB565 )
        {
            return RGB565_PIXEL_SIZE;
        }

        if ( format == Pixmap.ColorFormat.RGB888 )
        {
            return RGB888_PIXEL_SIZE;
        }

        throw new GdxRuntimeException( "Can only handle RGB565 or RGB888 images" );
    }

    // ========================================================================
    // Abstract Methods

    /// <summary>
    /// Returns the number of bytes needed to store the compressed data.
    /// </summary>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels </param>
    public virtual int GetCompressedDataSize( int width, int height )
    {
        throw new NotImplementedException();
    }

//    {
//            return etc1_get_encoded_data_size(width, height);
//    }

    /// <summary>
    /// Writes a PKM header to the <see cref="ByteBuffer"/>. Does not modify the
    /// position or limit of the ByteBuffer.
    /// </summary>
    /// <param name="header"> the direct native order <see cref="ByteBuffer"/> </param>
    /// <param name="offset"> the offset to the header in bytes</param>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels </param>
    public virtual void FormatHeader( ByteBuffer header, int offset, int width, int height )
    {
        throw new NotImplementedException();
    }

//    {
//            etc1_pkm_format_header((etc1_byte*)header + offset, width, height);
//    }

    /// <summary>
    /// Returns the width stored in the PKM header.
    /// </summary>
    /// <param name="header">
    /// direct native order <see cref="ByteBuffer"/> holding the PKM header
    /// </param>
    /// <param name="offset">
    /// the offset in bytes to the PKM header from the ByteBuffer's start
    /// </param>
    public virtual int GetWidthPKM( ByteBuffer? header, int offset )
    {
        throw new NotImplementedException();
    }

//    {
//            return etc1_pkm_get_width((etc1_byte*)header + offset);
//    }

    /// <summary>
    /// Returns the height stored in the PKM header
    /// </summary>
    /// <param name="header"> direct native order {@link ByteBuffer} holding the PKM header </param>
    /// <param name="offset"> the offset in bytes to the PKM header from the ByteBuffer's start </param>
    public virtual int GetHeightPKM( ByteBuffer? header, int offset )
    {
        throw new NotImplementedException();
    }

//    {
//            return etc1_pkm_get_height((etc1_byte*)header + offset);
//    }

    /// <summary>
    /// Returns <b>true</b> if the header if valid PKM.
    /// </summary>
    /// <param name="header"> direct native order <see cref="ByteBuffer"/> holding the PKM header. </param>
    /// <param name="offset"> the offset in bytes to the PKM header from the ByteBuffer's start. </param>
    public virtual bool IsValidPKM( ByteBuffer header, int offset )
    {
        throw new NotImplementedException();
    }

//    {
//            return etc1_pkm_is_valid((etc1_byte*)header + offset) != 0?true:false;
//    }

    /// <summary>
    /// Decodes the compressed image data to RGB565 or RGB888 pixel data. Does not
    /// modify the position or limit of the <see cref="ByteBuffer"/> instances.
    /// </summary>
    /// <param name="compressedData">
    /// the compressed image data in a direct native order {@link ByteBuffer}
    /// </param>
    /// <param name="offset"> the offset in bytes to the image data from the start of the buffer </param>
    /// <param name="decodedData">
    /// the decoded data in a direct native order ByteBuffer, must hold width * height * pixelSize bytes.
    /// </param>
    /// <param name="offsetDec"> the offset in bytes to the decoded image data. </param>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels </param>
    /// <param name="pixelSize"> the pixel size, either 2 (RBG565) or 3 (RGB888) </param>
    public virtual void DecodeImage( ByteBuffer? compressedData,
                                      int offset,
                                      ByteBuffer decodedData,
                                      int offsetDec,
                                      int width,
                                      int height,
                                      int pixelSize )
    {
        throw new NotImplementedException();
    }

//    {
//            etc1_decode_image((etc1_byte*)compressedData + offset, (etc1_byte*)decodedData + offsetDec, width, height, pixelSize, width * pixelSize);
//    }

    /// <summary>
    /// Encodes the image data given as RGB565 or RGB888. Does not modify the
    /// position or limit of the <see cref="ByteBuffer"/>.
    /// </summary>
    /// <param name="imageData"> the image data in a direct native order <see cref="ByteBuffer"/> </param>
    /// <param name="offset"> the offset in bytes to the image data from the start of the buffer </param>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels </param>
    /// <param name="pixelSize"> the pixel size, either 2 (RGB565) or 3 (RGB888) </param>
    /// <returns> a new direct native order ByteBuffer containing the compressed image data </returns>
    public virtual ByteBuffer EncodeImage( ByteBuffer imageData, int offset, int width, int height, int pixelSize )
    {
        throw new NotImplementedException();
    }

//    {
//            int compressedSize = etc1_get_encoded_data_size(width, height);
//            etc1_byte* compressedData = (etc1_byte*)malloc(compressedSize);
//            etc1_encode_image((etc1_byte*)imageData + offset, width, height, pixelSize, width * pixelSize, compressedData);
//            return env->NewDirectByteBuffer(compressedData, compressedSize);
//    }

    /// <summary>
    /// Encodes the image data given as RGB565 or RGB888. Does not modify
    /// the position or limit of the <see cref="ByteBuffer"/>.
    /// </summary>
    /// <param name="imageData"> the image data in a direct native order <see cref="ByteBuffer"/> </param>
    /// <param name="offset"> the offset in bytes to the image data from the start of the buffer </param>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels </param>
    /// <param name="pixelSize"> the pixel size, either 2 (RGB565) or 3 (RGB888) </param>
    /// <returns> a new direct native order ByteBuffer containing the compressed image data </returns>
    public virtual ByteBuffer EncodeImagePKM( ByteBuffer? imageData, int offset, int width, int height, int pixelSize )
    {
        throw new NotImplementedException();
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Class for storing ETC1 compressed image data.
    /// </summary>
    [PublicAPI]
    public class ETC1Data : IDisposable
    {
        private readonly ETC1 _etc1;

        public ETC1Data( int width, int height, ByteBuffer compressedData, int dataOffset, ETC1 etc1 )
        {
            _etc1          = etc1;
            Width          = width;
            Height         = height;
            CompressedData = compressedData;
            DataOffset     = dataOffset;

            CheckNPOT();
        }

        public ETC1Data( FileInfo pkmFile, ETC1 etc )
        {
            var           buffer = new byte[ 1024 * 10 ];
            BinaryReader? input  = null;

            _etc1 = etc;

            try
            {
                var zipStream      = new GZipStream( pkmFile.OpenRead(), CompressionMode.Decompress );
                var bufferedStream = new BufferedStream( zipStream );

                input = new BinaryReader( bufferedStream );

                var fileSize = input.ReadInt32();

                CompressedData = BufferUtils.NewByteBuffer( fileSize, isUnsafe: false );

                int readBytes;

                while ( ( readBytes = input.Read( buffer ) ) != -1 )
                {
                    CompressedData.Put( buffer, 0, readBytes );
                }

                CompressedData.Position = 0;
                CompressedData.Limit    = CompressedData.Capacity;
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( "Couldn't load pkm file '" + pkmFile + "'", e );
            }
            finally
            {
                input?.Close();
            }

            Width      = _etc1.GetWidthPKM( CompressedData, 0 );
            Height     = _etc1.GetHeightPKM( CompressedData, 0 );
            DataOffset = PKM_HEADER_SIZE;

            CompressedData.Position = DataOffset;
            CheckNPOT();
        }

        /// <summary>
        /// the width in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// the height in pixels.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// the optional PKM header and compressed image data.
        /// </summary>
        public ByteBuffer CompressedData { get; set; }

        /// <summary>
        /// the offset in bytes to the actual compressed data.
        /// Might be 16 if this contains a PKM header, 0 otherwise.
        /// </summary>
        public int DataOffset { get; set; }

        /// <summary>
        /// Returns whether this ETC1Data has a PKM header.
        /// </summary>
        public bool HasPKMHeader() => DataOffset == 16;

        private void CheckNPOT()
        {
            if ( !MathUtils.IsPowerOfTwo( Width ) || !MathUtils.IsPowerOfTwo( Height ) )
            {
                Console.WriteLine( "ETC1Data warning: non-power-of-two ETC1textures may crash the driver of PowerVR GPUs" );
            }
        }

        /// <summary>
        /// Writes the ETC1Data with a PKM header to the given file.
        /// </summary>
        /// <param name="file"> the file. </param>
        public void Write( FileInfo file )
        {
//            DataOutputStream? write        = null;
//            var               buffer       = new sbyte[ 10 * 1024 ];
//            var               writtenBytes = 0;

//            CompressedData.Position = 0;
//            CompressedData.Limit    = CompressedData.Capacity;

//            try
//            {
//                write = new DataOutputStream( new GZIPOutputStream( file.Write( false ) ) );
//                write.WriteInt( CompressedData.Capacity );

//                while ( writtenBytes != CompressedData.Capacity )
//                {
//                    var bytesToWrite = Math.Min( CompressedData.Remaining(), buffer.Length );

//                    CompressedData.Get( buffer, 0, bytesToWrite );

//                    write.Write( buffer, 0, bytesToWrite );
//                    writtenBytes += bytesToWrite;
//                }
//            }
//            catch ( System.Exception e )
//            {
//                throw new GdxRuntimeException( "Couldn't write PKM file to '" + file + "'", e );
//            }
//            finally
//            {
//                StreamUtils.CloseQuietly( write );
//            }

//            CompressedData.Position = DataOffset;
//            CompressedData.Limit    = CompressedData.Capacity;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if ( HasPKMHeader() )
            {
                return $"{( _etc1.IsValidPKM( CompressedData, 0 ) ? "valid" : "invalid" )} "
                     + $"pkm [{_etc1.GetWidthPKM( CompressedData, 0 )}x{_etc1.GetHeightPKM( CompressedData, 0 )}], "
                     + $"compressed: {CompressedData.Capacity - PKM_HEADER_SIZE}";
            }

            return $"raw [{Width}x{Height}], compressed: {CompressedData.Capacity - PKM_HEADER_SIZE}";
        }

        /// <summary>
        /// Releases the native resources of the ETC1Data instance.
        /// </summary>
        public void Dispose()
        {
            BufferUtils.DisposeUnsafeByteBuffer( CompressedData );
        }
    }

//    {
//            int compressedSize = etc1_get_encoded_data_size(width, height);
//            etc1_byte* compressed = (etc1_byte*)malloc(compressedSize + ETC_PKM_HEADER_SIZE);
//            etc1_pkm_format_header(compressed, width, height);
//            etc1_encode_image((etc1_byte*)imageData + offset, width, height, pixelSize, width * pixelSize, compressed + ETC_PKM_HEADER_SIZE);
//            return env->NewDirectByteBuffer(compressed, compressedSize + ETC_PKM_HEADER_SIZE);
//    }
}
