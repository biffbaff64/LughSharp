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

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class CubemapLoader : AsynchronousAssetLoader< Cubemap, CubemapLoader.CubemapParameter >
{
    public struct CubemapLoaderInfo
    {
        public string?       filename;
        public Cubemap?      cubemap;
        public ICubemapData? cubemapData;
    };

    private CubemapLoaderInfo _loaderInfo = new()
    {
        filename    = "",
        cubemap     = null!,
        cubemapData = null!
    };

    /// <summary>
    /// Creates a new CubmapLoader using the supplied resolver.
    /// </summary>
    public CubemapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    /// Returns the assets this asset requires to be loaded first.
    /// This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters parameter )
    {
        return default( List< AssetDescriptor > )!;
    }

    /// <summary>
    /// Loads the non-OpenGL part of the asset and injects any dependencies of
    /// the asset into the AssetManager.
    /// </summary>
    /// <param name="manager">The AssetManager to use.</param>
    /// <param name="fileName">The name of the asset to load.</param>
    /// <param name="file">The assets FileInfo object.</param>
    /// <param name="parameter">The AssetLoader parameters object.</param>
    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    AssetLoaderParameters? parameter )
    {
        _loaderInfo.filename = fileName!;

        if ( ( ( CubemapParameter? )parameter )?.cubemapData == null )
        {
            var genMipMaps = false;

            _loaderInfo.cubemap = null;

            if ( parameter != null )
            {
                _loaderInfo.cubemap = ( ( CubemapParameter? )parameter )?.cubemap;
            }

            if ( fileName != null )
            {
                if ( fileName.Contains( ".ktx" ) || fileName.Contains( ".zktx" ) )
                {
                    _loaderInfo.cubemapData = new KtxTextureData( file, genMipMaps );
                }
            }
        }
        else
        {
            _loaderInfo.cubemapData = ( ( CubemapParameter? )parameter )?.cubemapData;
            _loaderInfo.cubemap     = ( ( CubemapParameter? )parameter )?.cubemap;
        }

        if ( !_loaderInfo.cubemapData!.Prepared )
        {
            _loaderInfo.cubemapData.Prepare();
        }
    }

    /// <summary>
    /// Loads the OpenGL part of the asset.
    /// </summary>
    /// <param name="manager">The AssetManager to use.</param>
    /// <param name="fileName">The name of the asset to load.</param>
    /// <param name="file">The assets FileInfo object.</param>
    /// <param name="parameter">The AssetLoader parameters object.</param>
    /// <returns></returns>
    public override Cubemap LoadSync( AssetManager? manager,
                                      string? fileName,
                                      FileInfo? file,
                                      AssetLoaderParameters? parameter )
    {
        Cubemap? cubemap = _loaderInfo.cubemap;

        if ( cubemap != null )
        {
            cubemap.Load( _loaderInfo.cubemapData! );
        }
        else
        {
            cubemap = new Cubemap( _loaderInfo.cubemapData );
        }

        if ( parameter != null )
        {
            cubemap.SetFilter
                (
                 ( ( CubemapParameter? )parameter )!.minFilter,
                 ( ( CubemapParameter? )parameter )!.magFilter
                );

            cubemap.SetWrap
                (
                 ( ( CubemapParameter? )parameter )!.wrapU,
                 ( ( CubemapParameter? )parameter )!.wrapV
                );
        }

        return cubemap;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    public sealed class CubemapParameter : AssetLoaderParameters
    {
        // the format of the final Texture. Uses the source images format if null
        public Pixmap.Format? format = null;

        // The texture to put the TextureData in, optional.
        public Cubemap? cubemap = null;

        // CubemapData for textures created on the fly, optional.
        // When set, all format and genMipMaps are ignored
        public ICubemapData?  cubemapData = null;
        public TextureFilter minFilter   = TextureFilter.Nearest;
        public TextureFilter magFilter   = TextureFilter.Nearest;
        public TextureWrap   wrapU       = TextureWrap.ClampToEdge;
        public TextureWrap   wrapV       = TextureWrap.ClampToEdge;
    }
}