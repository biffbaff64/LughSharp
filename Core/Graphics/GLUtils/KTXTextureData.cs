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
/// A KTXTextureData holds the data from a KTX (or zipped KTX file, aka ZKTX).
/// That is to say an OpenGL ready texture data. The KTX file format is just a
/// thin wrapper around OpenGL textures and therefore is compatible with most
/// OpenGL texture capabilities like texture compression, cubemapping, mipmapping,
/// etc.
/// <para>
/// For example, KTXTextureData can be used for <see cref="Texture"/> or
/// <see cref="Cubemap"/>.
/// </para>
/// </summary>
[PublicAPI]
public class KtxTextureData : ITextureData, ICubemapData
{
    // The file we are loading
    private FileInfo? _file;

    // KTX header (only available after preparing)
    private int _glType;
    private int _glTypeSize;
    private int _glFormat;
    private int _glInternalFormat;
    private int _glBaseInternalFormat;
    private int _pixelWidth  = -1;
    private int _pixelHeight = -1;
    private int _pixelDepth  = -1;
    private int _numberOfArrayElements;
    private int _numberOfFaces;
    private int _numberOfMipmapLevels;
    private int _imagePos;

    // KTX image data (only available after preparing and before consuming)
    private ByteBuffer? _compressedData;

    public KtxTextureData( FileInfo? file, bool useMipMaps )
    {
        this._file      = file;
        this.UseMipMaps = useMipMaps;
    }

    /// <returns> the <see cref="ITextureData.TextureDataType"/></returns>
    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Custom;

    /// <returns> whether the TextureData is prepared or not.</returns>
    public bool IsPrepared => _compressedData != null;

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ICubemapData.ConsumeCubemapData"/>.
    /// This method can be called from a non OpenGL thread and should thus not
    /// interact with OpenGL. 
    /// </summary>
    public void Prepare()
    {
        if ( _compressedData != null )
        {
            throw new GdxRuntimeException( "Already prepared" );
        }

        if ( _file == null )
        {
            throw new GdxRuntimeException( "Need a file to load from" );
        }

        // We support normal ktx files as well as 'zktx' which are gzip ktx
        // file with an int length at the beginning (like ETC1).
        if ( _file.Name.EndsWith( ".zktx" ) )
        {
            var buffer = new byte[ 1024 * 10 ];
            DataInputStream dataInputStream = null;

            try
            {
                dataInputStream = new DataInputStream( new BufferedInputStream( new GZIPInputStream( file.read() ) ) );
                int fileSize = dataInputStream.readInt();
                _compressedData = BufferUtils.NewUnsafeByteBuffer( fileSize );
                var readBytes = 0;

                while ( ( readBytes = dataInputStream.read( buffer ) ) != -1 )
                {
                    _compressedData.Put( buffer, 0, readBytes );
                }
                
                ( ( Buffer )_compressedData ).position( 0 );
                ( ( Buffer )_compressedData ).limit( _compressedData.Capacity );
            }
            catch ( Exception e )
            {
                throw new GdxRuntimeException( $"Couldn't load zktx file '{_file}'", e );
            }
            finally
            {
                dataInputStream.Close();
            }
        }
        else
        {
            _compressedData = ByteBuffer.Wrap( _file.ReadBytes() );
        }

        if ( _compressedData.Get() != 0x0AB ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x04B ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x054 ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x058 ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x020 ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x031 ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x031 ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x0BB ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x00D ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x00A ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x01A ) throw new GdxRuntimeException( "Invalid KTX Header" );
        if ( _compressedData.Get() != 0x00A ) throw new GdxRuntimeException( "Invalid KTX Header" );

        var endianTag = _compressedData.GetInt();

        if ( ( endianTag != 0x04030201 ) && ( endianTag != 0x01020304 ) )
        {
            throw new GdxRuntimeException( "Invalid KTX Header" );
        }

        if ( endianTag != 0x04030201 )
        {
            _compressedData.Order( _compressedData.Order() == ByteOrder.BigEndian
                                       ? ByteOrder.LittleEndian
                                       : ByteOrder.BigEndian );
        }

        glType                = compressedData.getInt();
        glTypeSize            = compressedData.getInt();
        glFormat              = compressedData.getInt();
        glInternalFormat      = compressedData.getInt();
        glBaseInternalFormat  = compressedData.getInt();
        pixelWidth            = compressedData.getInt();
        pixelHeight           = compressedData.getInt();
        pixelDepth            = compressedData.getInt();
        numberOfArrayElements = compressedData.getInt();
        numberOfFaces         = compressedData.getInt();
        numberOfMipmapLevels  = compressedData.getInt();

        if ( numberOfMipmapLevels == 0 )
        {
            numberOfMipmapLevels = 1;
            useMipMaps           = true;
        }

        int bytesOfKeyValueData = compressedData.getInt();
        imagePos = compressedData.position() + bytesOfKeyValueData;

        if ( !compressedData.isDirect() )
        {
            int pos = imagePos;

            for ( var level = 0; level < numberOfMipmapLevels; level++ )
            {
                int faceLodSize        = compressedData.getInt( pos );
                var faceLodSizeRounded = ( faceLodSize + 3 ) & ~3;
                pos += faceLodSizeRounded * numberOfFaces + 4;
            }

            ( ( Buffer )compressedData ).limit( pos );
            ( ( Buffer )compressedData ).position( 0 );
            ByteBuffer directBuffer = BufferUtils.newUnsafeByteBuffer( pos );
            directBuffer.order( compressedData.order() );
            directBuffer.put( compressedData );
            compressedData = directBuffer;
        }
    }

    /// <summary>
    /// Uploads the pixel data for the 6 faces of the cube to the OpenGL ES texture.
    /// The caller must bind an OpenGL ES texture. A call to <see cref="ICubemapData.Prepare"/>
    /// must preceed a call to this method. Any internal data structures created
    /// in <see cref="ICubemapData.Prepare"/> should be disposed of here. 
    /// </summary>
    public void ConsumeCubemapData()
    {
    }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ITextureData.ConsumePixmap"/> or
    /// <see cref="ITextureData.ConsumeCustomData"/>. This method can be called from a non
    /// OpenGL thread and should thus not interact with OpenGL. 
    /// </summary>
    void ITextureData.Prepare()
    {
    }

    /// <summary>
    /// Returns the <see cref="Pixmap"/> for upload by Texture.
    /// <para>
    /// A call to <see cref="ITextureData.Prepare"/> must precede a call to this method. Any
    /// internal data structures created in <see cref="ITextureData.Prepare"/> should be
    /// disposed of here.
    /// </para>
    /// </summary>
    /// <returns> the pixmap.</returns>
    public Pixmap ConsumePixmap()
    {
    }

    /// <returns>
    /// whether the caller of <see cref="ITextureData.ConsumePixmap"/> should dispose the
    /// Pixmap returned by <see cref="ITextureData.ConsumePixmap"/>
    /// </returns>
    public bool DisposePixmap() => false;

    /// <summary>
    /// Uploads the pixel data to the OpenGL ES texture. The caller must bind an
    /// OpenGL ES texture. A call to <see cref="ITextureData.Prepare"/> must preceed a call
    /// to this method.
    /// <para>
    /// Any internal data structures created in <see cref="ITextureData.Prepare"/> should be
    /// disposed of here. 
    /// </para>
    /// </summary>
    public void ConsumeCustomData( int target )
    {
    }

    /// <returns> the width of the pixel data </returns>
    public int Width { get; set; }

    /// <returns> the height of the pixel data </returns>
    public int Height { get; set; }

    /// <summary>
    /// Returns true if this implementation can cope with a EGL context loss.
    /// </summary>
    public bool Managed { get; set; }

    /// <returns> the <see cref="Pixmap.Format"/> of the pixel data </returns>
    public Pixmap.Format GetFormat() => Pixmap.Format.Alpha;

    /// <returns> whether to generate mipmaps or not. </returns>
    public bool UseMipMaps { get; set; }

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool IsManaged() => false;
}
