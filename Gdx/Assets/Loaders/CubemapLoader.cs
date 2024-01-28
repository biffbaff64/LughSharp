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

[PublicAPI]
public class CubemapLoader : AsynchronousAssetLoader< Cubemap, CubemapLoader.CubemapParameter >
{
    private CubemapLoaderInfo _loaderInfo = new()
    {
        filename    = "",
        cubemap     = null!,
        cubemapData = null!
    };

    /// <summary>
    ///     Creates a new CubmapLoader using the supplied resolver.
    /// </summary>
    public CubemapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    ///     Returns the assets this asset requires to be loaded first.
    ///     This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? parameter )
    {
        return default( List< AssetDescriptor > )!;
    }

    /// <summary>
    ///     Loads the non-OpenGL part of the asset and injects any dependencies of
    ///     the asset into the AssetManager.
    /// </summary>
    /// <param name="manager">The AssetManager to use.</param>
    /// <param name="fileName">The name of the asset to load.</param>
    /// <param name="file">The assets FileInfo object.</param>
    /// <param name="parameter">The AssetLoader parameters object.</param>
    public override void Load( AssetManager? manager,
                               string? fileName,
                               FileInfo? file,
                               CubemapParameter? parameter )
    {
        const bool GEN_MIP_MAPS = false;

        _loaderInfo.filename = fileName!;

        if ( parameter?.cubemapData == null )
        {
            _loaderInfo.cubemap = null;

            if ( parameter != null )
            {
                _loaderInfo.cubemap = ( ( CubemapParameter? )parameter )?.cubemap;
            }

            if ( fileName != null )
            {
                if ( fileName.Contains( ".ktx" ) || fileName.Contains( ".zktx" ) )
                {
                    _loaderInfo.cubemapData = new KtxTextureData( file, GEN_MIP_MAPS );
                }
            }
        }
        else
        {
            _loaderInfo.cubemapData = parameter.cubemapData;
            _loaderInfo.cubemap     = parameter.cubemap;
        }

        if ( !_loaderInfo.cubemapData!.IsPrepared )
        {
            _loaderInfo.cubemapData.Prepare();
        }
    }

    [PublicAPI]
    public struct CubemapLoaderInfo
    {
        public string?       filename;
        public Cubemap?      cubemap;
        public ICubemapData? cubemapData;
    }

    [PublicAPI]
    public class CubemapParameter : AssetLoaderParameters
    {
        public TextureFilter magFilter = TextureFilter.Nearest;
        public TextureFilter minFilter = TextureFilter.Nearest;
        public TextureWrap   wrapU     = TextureWrap.ClampToEdge;
        public TextureWrap   wrapV     = TextureWrap.ClampToEdge;

        // The texture to put the TextureData in, optional.
        public Cubemap? cubemap = null;

        // CubemapData for textures created on the fly, optional.
        // When set, all format and genMipMaps are ignored
        public ICubemapData? cubemapData = null;

        // the format of the final Texture. Uses the source images format if null
        public Pixmap.Format? format = null;
    }
}
