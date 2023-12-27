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

using System.IO.Compression;

using LibGDXSharp.Files.Buffers;

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
    public bool IsPrepared
    {
        get => _compressedData != null;
        set { }
    }

//    /// <summary>
//    /// Prepares the TextureData for a call to <see cref="ITextureData.ConsumePixmap"/> or
//    /// <see cref="ITextureData.ConsumeCustomData"/>. This method can be called from a non
//    /// OpenGL thread and should thus not interact with OpenGL. 
//    /// </summary>
//    public void Prepare()
//    {
//    }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ICubemapData.ConsumeCubemapData"/>.
    /// This method can be called from a non OpenGL thread and should thus not
    /// interact with OpenGL. 
    /// </summary>
    public void Prepare()
    {
        if ( _compressedData != null )
        {
            throw new GdxRuntimeException( "Already prepared!" );
        }

        if ( _file == null )
        {
            throw new GdxRuntimeException( "Source file not specified!" );
        }

        // We support normal ktx files as well as 'zktx' which are gzip ktx
        // file with an int length at the beginning (like ETC1).
        if ( _file.Name.EndsWith( ".zktx" ) )
        {
            var           buffer          = new byte[ 1024 * 10 ];
            BinaryReader? dataInputStream = null;

            try
            {
                dataInputStream = new BinaryReader
                    ( new BufferedStream( new GZipStream( _file.OpenRead(), CompressionMode.Decompress ) ) );

                var fileSize = dataInputStream.ReadInt32();

                _compressedData = BufferUtils.NewByteBuffer( fileSize );

                int readBytes;

                while ( ( readBytes = dataInputStream.Read( buffer ) ) != -1 )
                {
                    _compressedData.Put( buffer, 0, readBytes );
                }

                _compressedData.Position = 0;
                _compressedData.Limit    = _compressedData.Capacity;
            }
            catch ( System.Exception e )
            {
                throw new GdxRuntimeException( $"Couldn't load zktx file '{_file}'", e );
            }
            finally
            {
                dataInputStream?.Close();
            }
        }
        else
        {
            _compressedData = ByteBuffer.Wrap( File.ReadAllBytes( _file.Name ) );
        }

        // ReSharper disable EnforceIfStatementBraces
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

        // ReSharper restore EnforceIfStatementBraces

        var endianTag = _compressedData.GetInt();

        if ( ( endianTag != 0x04030201 ) && ( endianTag != 0x01020304 ) )
        {
            throw new GdxRuntimeException( "Invalid KTX Header" );
        }

        if ( endianTag != 0x04030201 )
        {
            _compressedData.Order
                (
                 _compressedData.Order() == ByteOrder.BigEndian
                     ? ByteOrder.LittleEndian
                     : ByteOrder.BigEndian
                );
        }

        _glType                = _compressedData.GetInt();
        _glTypeSize            = _compressedData.GetInt();
        _glFormat              = _compressedData.GetInt();
        _glInternalFormat      = _compressedData.GetInt();
        _glBaseInternalFormat  = _compressedData.GetInt();
        _pixelWidth            = _compressedData.GetInt();
        _pixelHeight           = _compressedData.GetInt();
        _pixelDepth            = _compressedData.GetInt();
        _numberOfArrayElements = _compressedData.GetInt();
        _numberOfFaces         = _compressedData.GetInt();
        _numberOfMipmapLevels  = _compressedData.GetInt();

        if ( _numberOfMipmapLevels == 0 )
        {
            _numberOfMipmapLevels = 1;
            UseMipMaps            = true;
        }

        var bytesOfKeyValueData = _compressedData.GetInt();

        _imagePos = _compressedData.Position + bytesOfKeyValueData;

        if ( !_compressedData.IsDirect() )
        {
            var pos = _imagePos;

            for ( var level = 0; level < _numberOfMipmapLevels; level++ )
            {
                var faceLodSize        = _compressedData.GetInt( pos );
                var faceLodSizeRounded = ( faceLodSize + 3 ) & ~3;
                pos += ( faceLodSizeRounded * _numberOfFaces ) + 4;
            }

            _compressedData.Limit    = pos;
            _compressedData.Position = 0;

            ByteBuffer directBuffer = BufferUtils.NewByteBuffer( pos );
            directBuffer.Order( _compressedData.Order() );
            directBuffer.Put( _compressedData );

            _compressedData = directBuffer;
        }
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
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    /// <returns>
    /// whether the caller of <see cref="ITextureData.ConsumePixmap"/> should dispose the
    /// Pixmap returned by <see cref="ITextureData.ConsumePixmap"/>
    /// </returns>
    public bool DisposePixmap() => false;

    /// <summary>
    /// Uploads the pixel data for the 6 faces of the cube to the OpenGL ES texture.
    /// The caller must bind an OpenGL ES texture. A call to <see cref="ICubemapData.Prepare"/>
    /// must preceed a call to this method. Any internal data structures created
    /// in <see cref="ICubemapData.Prepare"/> should be disposed of here. 
    /// </summary>
    public void ConsumeCubemapData()
    {
        ConsumeCustomData( IGL20.GL_TEXTURE_CUBE_MAP );
    }

    private const int GL_TEXTURE_1D           = 0x1234;
    private const int GL_TEXTURE_3D           = 0x1234;
    private const int GL_TEXTURE_1D_ARRAY_EXT = 0x1234;
    private const int GL_TEXTURE_2D_ARRAY_EXT = 0x1234;

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
        if ( _compressedData == null )
        {
            throw new GdxRuntimeException( "Call prepare() before calling consumeCompressedData()" );
        }

        IntBuffer buffer = BufferUtils.NewIntBuffer( 16 );

        // Check OpenGL type and format, detect compressed data format (no type & format)
        var compressed = false;

        if ( ( _glType == 0 ) || ( _glFormat == 0 ) )
        {
            if ( ( _glType + _glFormat ) != 0 )
            {
                throw new GdxRuntimeException( "either both or none of glType, glFormat must be zero" );
            }

            compressed = true;
        }

        // find OpenGL texture target and dimensions
        var textureDimensions = 1;
        var glTarget          = GL_TEXTURE_1D;

        if ( _pixelHeight > 0 )
        {
            textureDimensions = 2;
            glTarget          = IGL20.GL_TEXTURE_2D;
        }

        if ( _pixelDepth > 0 )
        {
            textureDimensions = 3;
            glTarget          = GL_TEXTURE_3D;
        }

        if ( _numberOfFaces == 6 )
        {
            if ( textureDimensions == 2 )
            {
                glTarget = IGL20.GL_TEXTURE_CUBE_MAP;
            }
            else
            {
                throw new GdxRuntimeException( "cube map needs 2D faces" );
            }
        }
        else if ( _numberOfFaces != 1 )
        {
            throw new GdxRuntimeException( "numberOfFaces must be either 1 or 6" );
        }

        if ( _numberOfArrayElements > 0 )
        {
            if ( glTarget == GL_TEXTURE_1D )
            {
                glTarget = GL_TEXTURE_1D_ARRAY_EXT;
            }
            else if ( glTarget == IGL20.GL_TEXTURE_2D )
            {
                glTarget = GL_TEXTURE_2D_ARRAY_EXT;
            }
            else
            {
                throw new GdxRuntimeException( "No API for 3D and cube arrays yet" );
            }

            textureDimensions++;
        }

        if ( glTarget == 0x1234 )
        {
            throw new GdxRuntimeException
                ( "Unsupported texture format (only 2D texture are supported in LibGdx for the time being)" );
        }

        var singleFace = -1;

        if ( ( _numberOfFaces == 6 ) && ( target != IGL20.GL_TEXTURE_CUBE_MAP ) )
        {
            // Load a single face of the cube (should be avoided since the data is unloaded afterwards)
            if ( !( target is >= IGL20.GL_TEXTURE_CUBE_MAP_POSITIVE_X
                              and <= IGL20.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z ) )
            {
                throw new GdxRuntimeException
                    (
                     "You must specify either GL_TEXTURE_CUBE_MAP to bind all 6 faces of the"
                   + "cube or the requested face GL_TEXTURE_CUBE_MAP_POSITIVE_X and followings."
                    );
            }

            singleFace = target - IGL20.GL_TEXTURE_CUBE_MAP_POSITIVE_X;
            target     = IGL20.GL_TEXTURE_CUBE_MAP_POSITIVE_X;
        }
        else if ( ( _numberOfFaces == 6 ) && ( target == IGL20.GL_TEXTURE_CUBE_MAP ) )
        {
            // Load the 6 faces
            target = IGL20.GL_TEXTURE_CUBE_MAP_POSITIVE_X;
        }
        else
        {
            // Load normal texture
            if ( ( target != glTarget )
              && !( ( IGL20.GL_TEXTURE_CUBE_MAP_POSITIVE_X <= target )
                 && ( target <= IGL20.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z )
                 && ( target == IGL20.GL_TEXTURE_2D ) ) )
            {
                throw new GdxRuntimeException
                    (
                     "Invalid target requested : 0x"
                   + target.ToString( "X" )
                   + ", expecting : 0x"
                   + glTarget.ToString( "X" )
                    );
            }
        }

        // KTX files require an unpack alignment of 4
        Gdx.GL.GLGetIntegerv( IGL20.GL_UNPACK_ALIGNMENT, buffer );

        var previousUnpackAlignment = buffer.Get( 0 );

        if ( previousUnpackAlignment != 4 )
        {
            Gdx.GL.GLPixelStorei( IGL20.GL_UNPACK_ALIGNMENT, 4 );
        }

        var glInternalFormat = this._glInternalFormat;
        var glFormat         = this._glFormat;
        var pos              = _imagePos;

        for ( var level = 0; level < _numberOfMipmapLevels; level++ )
        {
            var pixelWidth  = Math.Max( 1, this._pixelWidth >> level );
            var pixelHeight = Math.Max( 1, this._pixelHeight >> level );
            var pixelDepth  = Math.Max( 1, this._pixelDepth >> level );

            _compressedData.Position = pos;

            var faceLodSize        = _compressedData.GetInt();
            var faceLodSizeRounded = ( faceLodSize + 3 ) & ~3;

            pos += 4;

            for ( var face = 0; face < _numberOfFaces; face++ )
            {
                _compressedData.Position = pos;

                pos += faceLodSizeRounded;

                if ( ( singleFace != -1 ) && ( singleFace != face ) )
                {
                    continue;
                }

                ByteBuffer data = _compressedData.Slice();
                data.Limit = faceLodSizeRounded;

                if ( textureDimensions == 1 )
                {
                    // if (compressed)
                    // Gdx.gl.glCompressedTexImage1D(target + face, level, glInternalFormat, pixelWidth, 0, faceLodSize,
                    // data);
                    // else
                    // Gdx.gl.glTexImage1D(target + face, level, glInternalFormat, pixelWidth, 0, glFormat, glType, data);
                }
                else if ( textureDimensions == 2 )
                {
                    if ( _numberOfArrayElements > 0 )
                    {
                        pixelHeight = _numberOfArrayElements;
                    }

                    if ( compressed )
                    {
                        if ( glInternalFormat == ETC1.ETC1_RGB8_OES )
                        {
                            if ( !Gdx.Graphics.SupportsExtension( "GL_OES_compressed_ETC1_RGB8_texture" ) )
                            {
                                var    etcData = new ETC1.ETC1Data( pixelWidth, pixelHeight, data, 0 );
                                Pixmap pixmap  = ETC1.DecodeImage( etcData, Pixmap.Format.RGB888 );

                                Gdx.GL.GLTexImage2D
                                    (
                                     target + face,
                                     level,
                                     pixmap.GLInternalFormat,
                                     pixmap.Width, pixmap.Height,
                                     0,
                                     pixmap.GLFormat, pixmap.GLType, pixmap.Pixels
                                    );

                                pixmap.Dispose();
                            }
                            else
                            {
                                Gdx.GL.GLCompressedTexImage2D
                                    (
                                     target + face, level, glInternalFormat, pixelWidth, pixelHeight, 0,
                                     faceLodSize, data
                                    );
                            }
                        }
                        else
                        {
                            // Try to load (no software unpacking fallback)
                            Gdx.GL.GLCompressedTexImage2D
                                (
                                 target + face, level, glInternalFormat, pixelWidth, pixelHeight, 0,
                                 faceLodSize, data
                                );
                        }
                    }
                    else
                    {
                        Gdx.GL.GLTexImage2D
                            (
                             target + face,
                             level,
                             glInternalFormat,
                             pixelWidth,
                             pixelHeight,
                             0,
                             glFormat,
                             _glType,
                             data
                            );
                    }
                }
                else if ( textureDimensions == 3 )
                {
                    if ( _numberOfArrayElements > 0 )
                    {
                        pixelDepth = _numberOfArrayElements;
                    }

                    // if (compressed)
                    // Gdx.gl.glCompressedTexImage3D(target + face, level, glInternalFormat, pixelWidth, pixelHeight, pixelDepth, 0,
                    // faceLodSize, data);
                    // else
                    // Gdx.gl.glTexImage3D(target + face, level, glInternalFormat, pixelWidth, pixelHeight, pixelDepth, 0, glFormat,
                    // glType, data);
                }
            }
        }

        if ( previousUnpackAlignment != 4 )
        {
            Gdx.GL.GLPixelStorei( IGL20.GL_UNPACK_ALIGNMENT, previousUnpackAlignment );
        }

        if ( UseMipMaps )
        {
            Gdx.GL.GLGenerateMipmap( target );
        }

        // dispose data once transfered to GPU
        DisposePreparedData();
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

    public void DisposePreparedData()
    {
        if ( _compressedData != null )
        {
            BufferUtils.DisposeUnsafeByteBuffer( _compressedData );
        }

        _compressedData = null;
    }
}
