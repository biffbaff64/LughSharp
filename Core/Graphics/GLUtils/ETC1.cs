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

using LibGDXSharp.Utils.Buffers;

namespace LibGDXSharp.Graphics.GLUtils;

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
    public const int ETC1_RGB8_OES = 0x00008d64;

    /// <summary>
    /// Class for storing ETC1 compressed image data.
    /// </summary>
    public class ETC1Data : IDisposable
    {
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
        public ByteBuffer? CompressedData { get; set; }

        /// <summary>
        /// the offset in bytes to the actual compressed data.
        /// Might be 16 if this contains a PKM header, 0 otherwise.
        /// </summary>
        public int DataOffset { get; set; }

        public ETC1Data( int width, int height, ByteBuffer? compressedData, int dataOffset )
        {
            this.Width          = width;
            this.Height         = height;
            this.CompressedData = compressedData;
            this.DataOffset     = dataOffset;

            CheckNPOT();
        }

        public ETC1Data( FileInfo pkmFile )
        {
//            var              buffer = new sbyte[ 1024 * 10 ];
//            DataInputStream? input  = null;

//            try
//            {
//                input = new DataInputStream( new BufferedInputStream( new GZIPInputStream( pkmFile.Read() ) ) );

//                int fileSize = input.ReadInt();

//                CompressedData = BufferUtils.NewUnsafeByteBuffer( fileSize );
//                int readBytes;

//                while ( ( readBytes = input.Read( buffer ) ) != -1 )
//                {
//                    CompressedData.Put( buffer, 0, readBytes );
//                }

//                CompressedData.Position = 0;
//                CompressedData.Limit    = CompressedData.Capacity;
//            }
//            catch ( Exception e )
//            {
//                throw new GdxRuntimeException( "Couldn't load pkm file '" + pkmFile + "'", e );
//            }
//            finally
//            {
//                StreamUtils.CloseQuietly( input );
//            }

//            Width      = GetWidthPKM( CompressedData, 0 );
//            Height     = GetHeightPKM( CompressedData, 0 );
//            DataOffset = PkmHeaderSize;

//            CompressedData.Position = DataOffset;
//            CheckNPOT();
        }

        private void CheckNPOT()
        {
            if ( !MathUtils.IsPowerOfTwo( Width ) || !MathUtils.IsPowerOfTwo( Height ) )
            {
                Console.WriteLine
                    ( "ETC1Data " + "warning: non-power-of-two ETC1 textures may crash the driver of PowerVR GPUs" );
            }
        }

        /// <summary>
        /// </summary>
        /// <returns> whether this ETC1Data has a PKM header </returns>
        public bool HasPKMHeader() => DataOffset == 16;

        /// <summary>
        /// Writes the ETC1Data with a PKM header to the given file.
        /// </summary>
        /// <param name="file"> the file.  </param>
        public void Write( FileInfo file )
        {
//            DataOutputStream? write        = null;
//            sbyte[]           buffer       = new sbyte[ 10 * 1024 ];
//            int               writtenBytes = 0;

//            CompressedData.Position = 0;
//            CompressedData.Limit    = CompressedData.Capacity;

//            try
//            {
//                write = new DataOutputStream( new GZIPOutputStream( file.Write( false ) ) );
//                write.WriteInt( CompressedData.Capacity );

//                while ( writtenBytes != CompressedData.Capacity )
//                {
//                    int bytesToWrite = Math.Min( CompressedData.Remaining(), buffer.Length );

//                    CompressedData.Get( buffer, 0, bytesToWrite );

//                    write.Write( buffer, 0, bytesToWrite );
//                    writtenBytes += bytesToWrite;
//                }
//            }
//            catch ( Exception e )
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

        /// <summary>
        /// Releases the native resources of the ETC1Data instance.
        /// </summary>
        public void Dispose()
        {
            BufferUtils.DisposeUnsafeByteBuffer( CompressedData );
        }

        public override string ToString()
        {
            if ( HasPKMHeader() )
            {
                return ( ETC1.IsValidPKM( CompressedData, 0 ) ? "valid" : "invalid" )
                       + " pkm ["
                       + ETC1.GetWidthPKM( CompressedData, 0 )
                       + "x"
                       + ETC1.GetHeightPKM( CompressedData, 0 )
                       + "], compressed: "
                       + ( CompressedData.Capacity - ETC1.PKM_HEADER_SIZE );
            }
            else
            {
                return "raw ["
                       + Width
                       + "x"
                       + Height
                       + "], compressed: "
                       + ( CompressedData.Capacity - ETC1.PKM_HEADER_SIZE );
            }
        }
    }

    private static int GetPixelSize( Pixmap.Format format )
    {
        if ( format == Pixmap.Format.RGB565 ) return 2;
        if ( format == Pixmap.Format.RGB888 ) return 3;

        throw new GdxRuntimeException( "Can only handle RGB565 or RGB888 images" );
    }

    /// <summary>
    /// Encodes the image via the ETC1 compression scheme. Only <see cref="Pixmap.Format.RGB565"/>
    /// and <see cref="Pixmap.Format.RGB888"/> are supported.
    /// </summary>
    /// <param name="pixmap"> the <see cref="Pixmap"/> </param>
    /// <returns> the <see cref="ETC1Data"/> </returns>
    public static ETC1Data EncodeImage( Pixmap pixmap )
    {
        var         pixelSize      = GetPixelSize( pixmap.GetFormat() );
        ByteBuffer? compressedData = EncodeImage( pixmap.Pixels, 0, pixmap.Width, pixmap.Height, pixelSize );

        BufferUtils.NewUnsafeByteBuffer( compressedData );

        return new ETC1Data( pixmap.Width, pixmap.Height, compressedData, 0 );
    }

    /// <summary>
    /// Encodes the image via the ETC1 compression scheme. Only <see cref="Pixmap.Format.RGB565"/> and
    /// <see cref="Pixmap.Format.RGB888"/> are supported. Adds a PKM header in front of the compressed
    /// image data.
    /// </summary>
    /// <param name="pixmap"> the <see cref="Pixmap"/> </param>
    /// <returns> the <see cref="ETC1Data"/> </returns>
    public static ETC1Data EncodeImagePKM( Pixmap pixmap )
    {
        var         pixelSize      = GetPixelSize( pixmap.GetFormat() );
        ByteBuffer? compressedData = EncodeImagePKM( pixmap.Pixels, 0, pixmap.Width, pixmap.Height, pixelSize );

        BufferUtils.NewUnsafeByteBuffer( compressedData );

        return new ETC1Data( pixmap.Width, pixmap.Height, compressedData, 16 );
    }

    /// <summary>
    /// Takes ETC1 compressed image data and converts it to a <see cref="Pixmap.Format.RGB565"/> or
    /// <see cref="Pixmap.Format.RGB888"/> <see cref="Pixmap"/>.
    /// Does not modify the ByteBuffer's position or limit.
    /// </summary>
    /// <param name="etc1Data"> the <see cref="ETC1Data"/> instance </param>
    /// <param name="format"> either <see cref="Pixmap.Format.RGB565"/> or <see cref="Pixmap.Format.RGB888"/> </param>
    /// <returns> the Pixmap </returns>
    public static Pixmap DecodeImage( ETC1Data etc1Data, Pixmap.Format format )
    {
        var dataOffset = 0;
        var width      = 0;
        var height     = 0;

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

        var    pixelSize = GetPixelSize( format );
        var pixmap    = new Pixmap( width, height, format );

        DecodeImage( etc1Data.CompressedData, dataOffset, pixmap.Pixels, 0, width, height, pixelSize );

        return pixmap;
    }

    // @off
    /*JNI
    #include <etc1/etc1_utils.h>
    #include <stdlib.h>
     */

    /// <summary>
    /// </summary>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels </param>
    /// <returns> the number of bytes needed to store the compressed data </returns>
    public static /*native*/ int GetCompressedDataSize( int width, int height )
    {
//            return etc1_get_encoded_data_size(width, height);

        return 0;
    }

    /** Writes a PKM header to the {@link ByteBuffer}. Does not modify the position or limit of the ByteBuffer.
	 * @param header the direct native order {@link ByteBuffer}
	 * @param offset the offset to the header in bytes
	 * @param width the width in pixels
	 * @param height the height in pixels */
    public static /*native*/ void FormatHeader( ByteBuffer header, int offset, int width, int height )
    {
        /*
            etc1_pkm_format_header((etc1_byte*)header + offset, width, height);
        */
    }

    /** @param header direct native order {@link ByteBuffer} holding the PKM header
	 * @param offset the offset in bytes to the PKM header from the ByteBuffer's start
	 * @return the width stored in the PKM header */
    private static /*native*/ int GetWidthPKM( ByteBuffer header, int offset )
    {
        /*
            return etc1_pkm_get_width((etc1_byte*)header + offset);
        */

        return 0;
    }

    /** @param header direct native order {@link ByteBuffer} holding the PKM header
	 * @param offset the offset in bytes to the PKM header from the ByteBuffer's start
	 * @return the height stored in the PKM header */
    private static /*native*/ int GetHeightPKM( ByteBuffer header, int offset )
    {
        /*
            return etc1_pkm_get_height((etc1_byte*)header + offset);
        */

        return 0;
    }

    /** @param header direct native order {@link ByteBuffer} holding the PKM header
	 * @param offset the offset in bytes to the PKM header from the ByteBuffer's start
	 * @return the width stored in the PKM header */
    private static /*native*/ bool IsValidPKM( ByteBuffer header, int offset )
    {
        /*
            return etc1_pkm_is_valid((etc1_byte*)header + offset) != 0?true:false;
        */

        return false;
    }

    /** Decodes the compressed image data to RGB565 or RGB888 pixel data. Does not modify the position or limit of the
	 * {@link ByteBuffer} instances.
	 * @param compressedData the compressed image data in a direct native order {@link ByteBuffer}
	 * @param offset the offset in bytes to the image data from the start of the buffer
	 * @param decodedData the decoded data in a direct native order ByteBuffer, must hold width * height * pixelSize bytes.
	 * @param offsetDec the offset in bytes to the decoded image data.
	 * @param width the width in pixels
	 * @param height the height in pixels
	 * @param pixelSize the pixel size, either 2 (RBG565) or 3 (RGB888) */
    private static /*native*/ void DecodeImage( ByteBuffer compressedData, int offset, ByteBuffer decodedData,
                                                int offsetDec, int width, int height, int pixelSize )
    {
        /*
            etc1_decode_image((etc1_byte*)compressedData + offset, (etc1_byte*)decodedData + offsetDec, width, height, pixelSize, width * pixelSize);
        */
    }

    /** Encodes the image data given as RGB565 or RGB888. Does not modify the position or limit of the {@link ByteBuffer}.
	 * @param imageData the image data in a direct native order {@link ByteBuffer}
	 * @param offset the offset in bytes to the image data from the start of the buffer
	 * @param width the width in pixels
	 * @param height the height in pixels
	 * @param pixelSize the pixel size, either 2 (RGB565) or 3 (RGB888)
	 * @return a new direct native order ByteBuffer containing the compressed image data */
    private static /*native*/ ByteBuffer? EncodeImage( ByteBuffer imageData, int offset, int width, int height, int pixelSize )
    {
        /*
            int compressedSize = etc1_get_encoded_data_size(width, height);
            etc1_byte* compressedData = (etc1_byte*)malloc(compressedSize);
            etc1_encode_image((etc1_byte*)imageData + offset, width, height, pixelSize, width * pixelSize, compressedData);
            return env->NewDirectByteBuffer(compressedData, compressedSize);
        */

        return default;
    }

    /** Encodes the image data given as RGB565 or RGB888. Does not modify the position or limit of the {@link ByteBuffer}.
	 * @param imageData the image data in a direct native order {@link ByteBuffer}
	 * @param offset the offset in bytes to the image data from the start of the buffer
	 * @param width the width in pixels
	 * @param height the height in pixels
	 * @param pixelSize the pixel size, either 2 (RGB565) or 3 (RGB888)
	 * @return a new direct native order ByteBuffer containing the compressed image data */
    private static /*native*/ ByteBuffer? EncodeImagePKM( ByteBuffer imageData, int offset, int width, int height, int pixelSize )
    {
        /*
            int compressedSize = etc1_get_encoded_data_size(width, height);
            etc1_byte* compressed = (etc1_byte*)malloc(compressedSize + ETC_PKM_HEADER_SIZE);
            etc1_pkm_format_header(compressed, width, height);
            etc1_encode_image((etc1_byte*)imageData + offset, width, height, pixelSize, width * pixelSize, compressed + ETC_PKM_HEADER_SIZE);
            return env->NewDirectByteBuffer(compressed, compressedSize + ETC_PKM_HEADER_SIZE);
        */

        return default;
    }
}
