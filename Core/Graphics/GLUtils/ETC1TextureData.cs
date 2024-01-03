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

namespace LibGDXSharp.Graphics.GLUtils;

[PublicAPI]
public class ETC1TextureData : ITextureData
{
    private FileInfo?      _file;
    private ETC1.ETC1Data? _data;

    public ETC1TextureData( FileInfo file, bool useMipMaps = false )
    {
        this._file      = file;
        this.UseMipMaps = useMipMaps;
    }

    public ETC1TextureData( ETC1.ETC1Data encodedImage, bool useMipMaps )
    {
        this._data      = encodedImage;
        this.UseMipMaps = useMipMaps;
    }

    /// <inheritdoc/>
    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Custom;

    /// <inheritdoc/>
    public bool IsPrepared { get; set; }

    /// <inheritdoc/>
    public void Prepare()
    {
        if ( IsPrepared )
        {
            throw new GdxRuntimeException( "Already prepared" );
        }

        if ( ( _file == null ) && ( _data == null ) )
        {
            throw new GdxRuntimeException( "Can only load once from ETC1Data" );
        }

        if ( _file != null )
        {
            _data = new ETC1.ETC1Data( _file );
        }

        if ( _data == null )
        {
            throw new GdxRuntimeException( "No data to prepare!" );
        }
        
        Width      = _data.Width;
        Height     = _data.Height;
        IsPrepared = true;
    }

    /// <inheritdoc/>
    public Pixmap ConsumePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    /// <inheritdoc/>
    public bool DisposePixmap()
    {
        throw new GdxRuntimeException( "This TextureData implementation does not return a Pixmap" );
    }

    /// <inheritdoc/>
    public void ConsumeCustomData( TextureTarget target )
    {
        if ( !IsPrepared )
        {
            throw new GdxRuntimeException( "Call prepare() before calling consumeCompressedData()" );
        }

        if ( _data is null )
        {
            throw new GdxRuntimeException( "No data to consume!" );
        }
            
        if ( !Gdx.Graphics.SupportsExtension( "GL_OES_compressed_ETC1_RGB8_texture" ) )
        {
            Pixmap pixmap = ETC1.DecodeImage( _data, Pixmap.Format.RGB565 );

            Gdx.GL.GLTexImage2D
                (
                 target, 0, pixmap.GLInternalFormat, pixmap.Width, pixmap.Height, 0,
                 pixmap.GLFormat, pixmap.GLType, pixmap.Pixels
                );

            if ( UseMipMaps )
            {
                MipMapGenerator.GenerateMipMap( target, pixmap, pixmap.Width, pixmap.Height );
            }

            pixmap.Dispose();
            UseMipMaps = false;
        }
        else
        {
            Gdx.GL.GLCompressedTexImage2D
                (
                 target,
                 0,
                 ETC1.ETC1_RGB8_OES,
                 Width, Height,
                 0,
                 _data.CompressedData!.Capacity - _data.DataOffset,
                 _data.CompressedData
                );

            if ( UseMipMaps )
            {
                Gdx.GL20.GLGenerateMipmap( IGL20.GL_TEXTURE_2D );
            }
        }

        _data.Dispose();
        
        _data       = null;
        IsPrepared = false;
    }

    /// <inheritdoc/>
    public int Width { get; set; }

    /// <inheritdoc/>
    public int Height { get; set; }

    /// <inheritdoc/>
    public bool UseMipMaps { get; set; } = false;

    /// <inheritdoc/>
    public Pixmap.Format GetFormat() => Pixmap.Format.Alpha;

    /// <inheritdoc/>
    public bool IsManaged() => false;
}
