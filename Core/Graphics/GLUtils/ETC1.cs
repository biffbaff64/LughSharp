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

using LibGDXSharp.Maths;

using Buffer = Silk.NET.OpenGLES.Buffer;

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
/// Class for encoding and decoding ETC1 compressed images.
/// Also provides methods to add a PKM header.
/// </summary>
public class ETC1
{
    /// <summary>
    /// The PKM header size in bytes
    /// </summary>
    public const int PkmHeaderSize = 16;
    public const int Etc1Rgb8Oes = 0x00008d64;

    /// <summary>
    /// Class for storing ETC1 compressed image data.
    /// @author mzechner 
    /// </summary>
    public sealed class ETC1Data : IDisposable
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
        public ByteBuffer CompressedData { get; set; }

        /// <summary>
        /// the offset in bytes to the actual compressed data.
        /// Might be 16 if this contains a PKM header, 0 otherwise.
        /// </summary>
        public int DataOffset { get; set; }

        public ETC1Data( int width, int height, ByteBuffer compressedData, int dataOffset )
        {
            this.Width          = width;
            this.Height         = height;
            this.CompressedData = compressedData;
            this.DataOffset     = dataOffset;
            
            CheckNPOT();
        }

        public ETC1Data( FileHandle pkmFile )
        {
            var         buffer = new sbyte[ 1024 * 10 ];
            DataInputStream input  = null;

            try
            {
                input = new DataInputStream( new BufferedInputStream( new GZIPInputStream( pkmFile.Read() ) ) );
                
                int fileSize = input.ReadInt();
                
                CompressedData = BufferUtils.NewUnsafeByteBuffer( fileSize );
                int readBytes;

                while ( ( readBytes = input.Read( buffer ) ) != -1 )
                {
                    CompressedData.Put( buffer, 0, readBytes );
                }

                CompressedData.Position = 0;
                CompressedData.Limit = CompressedData.Capacity;
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( "Couldn't load pkm file '" + pkmFile + "'", e );
            }
            finally
            {
                StreamUtils.CloseQuietly( input );
            }

            Width      = GetWidthPKM( CompressedData, 0 );
            Height     = GetHeightPKM( CompressedData, 0 );
            DataOffset = PkmHeaderSize;
            
            CompressedData.Position = DataOffset;
            CheckNPOT();
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
        public void Write(FileHandle file)
        {
            DataOutputStream write        = null;
            sbyte[]          buffer       = new sbyte[10 * 1024];
            int              writtenBytes = 0;
            
            CompressedData.Position = 0;
            CompressedData.Limit = CompressedData.Capacity;

            try
            {
                write = new DataOutputStream(new GZIPOutputStream( file.Write( false ) ) );
                write.WriteInt( CompressedData.Capacity );
                
                while (writtenBytes != CompressedData.Capacity )
                {
                    int bytesToWrite = Math.Min( CompressedData.Remaining(), buffer.Length );

                    CompressedData.Get( buffer, 0, bytesToWrite );

                    write.Write(buffer, 0, bytesToWrite);
                    writtenBytes += bytesToWrite;
                }
            }
            catch (Exception e)
            {
                throw new GdxRuntimeException("Couldn't write PKM file to '" + file + "'", e);
            }
            finally
            {
                StreamUtils.CloseQuietly( write );
            }
            
            CompressedData.Position = DataOffset;
            CompressedData.Limit = CompressedData.Capacity;
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
                return (ETC1.IsValidPKM( CompressedData, 0 ) ? "valid" : "invalid")
                       + " pkm [" + ETC1.GetWidthPKM( CompressedData, 0 ) + "x"
                       + ETC1.GetHeightPKM( CompressedData, 0 ) + "], compressed: "
                       + ( CompressedData.Capacity - ETC1.PkmHeaderSize );
            }
            else
            {
                return "raw [" + Width + "x" + Height + "], compressed: "
                       + ( CompressedData.Capacity - ETC1.PkmHeaderSize );
            }
        }
    }
}