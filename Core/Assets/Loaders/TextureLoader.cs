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

namespace LibGDXSharp.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> for <see cref="Texture"/> instances.
/// The pixel data is loaded asynchronously. The texture is then created on the
/// rendering thread, synchronously. Passing a <see cref="TextureParameter"/>
/// to <see cref="AssetManager"/>.Load() allows one to specify parameters as
/// can be passed to the various Texture constructors, e.g. filtering, whether
/// to generate mipmaps and so on.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class TextureLoader : AsynchronousAssetLoader, IDisposable
{
    public class TextureLoaderInfo
    {
        public string?       Filename { get; set; }
        public ITextureData? Data     { get; set; }
        public Texture?      Texture  { get; set; }
    }

    private readonly TextureLoaderInfo _loaderInfo;

    public TextureLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        _loaderInfo = new TextureLoaderInfo();
    }

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    AssetLoaderParameters parameter )
    {
        _loaderInfo.Filename = fileName;

        if ( ( ( TextureParameter )parameter ).TextureData == null )
        {
            Pixmap.Format? format     = null;
            var            genMipMaps = false;

            _loaderInfo.Texture = null;

            if ( parameter != null )
            {
                format              = ( ( TextureParameter )parameter ).Format;
                genMipMaps          = ( ( TextureParameter )parameter ).GenMipMaps;
                _loaderInfo.Texture = ( ( TextureParameter )parameter ).Texture;
            }

            _loaderInfo.Data = ITextureData.Factory.LoadFromFile( file, format, genMipMaps );
        }
        else
        {
            _loaderInfo.Data    = ( ( TextureParameter )parameter ).TextureData;
            _loaderInfo.Texture = ( ( TextureParameter )parameter ).Texture;
        }

        if ( _loaderInfo.Data is { IsPrepared: false } )
        {
            _loaderInfo.Data.Prepare();
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override Texture LoadSync( AssetManager? manager,
                                      string? fileName,
                                      FileInfo? file,
                                      AssetLoaderParameters parameter )
    {
        Texture? texture = _loaderInfo.Texture;

        if ( texture != null )
        {
            texture.Load( _loaderInfo.Data );
        }
        else
        {
            texture = new Texture( _loaderInfo.Data );
        }

        texture.SetFilter
            (
             ( ( TextureParameter )parameter ).MinFilter,
             ( ( TextureParameter )parameter ).MagFilter
            );

        texture.SetWrap
            (
             ( ( TextureParameter )parameter ).WrapU,
             ( ( TextureParameter )parameter ).WrapV
            );

        return texture;
    }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters parameter )
    {
        return null!;
    }

    /// <summary>
    /// </summary>
    [SuppressMessage( "ReSharper", "PropertyCanBeMadeInitOnly.Global" )]
    public class TextureParameter : AssetLoaderParameters
    {
        /// <summary>
        /// the format of the final Texture. Uses the source images format if null
        /// </summary>
        public Pixmap.Format? Format { get; set; } = null;

        /// <summary>
        /// whether to generate mipmaps
        /// </summary>
        public bool GenMipMaps { get; set; } = false;

        /// <summary>
        /// The texture to put the <see cref="TextureData"/> in, optional.
        /// </summary>
        public Texture? Texture { get; set; } = null;

        /// <summary>
        /// TextureData for textures created on the fly, optional. When set, all format and genMipMaps are ignored
        /// </summary>
        public ITextureData? TextureData { get; set; } = null;

        public TextureFilter MinFilter { get; set; } = TextureFilter.Nearest;
        public TextureFilter MagFilter { get; set; } = TextureFilter.Nearest;
        public TextureWrap   WrapU     { get; set; } = TextureWrap.ClampToEdge;
        public TextureWrap   WrapV     { get; set; } = TextureWrap.ClampToEdge;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }
}