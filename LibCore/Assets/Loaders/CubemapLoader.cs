// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Assets.Loaders.Resolvers;
using LughSharp.LibCore.Graphics;

namespace LughSharp.LibCore.Assets.Loaders;

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


    internal struct CubemapLoaderInfo
    {
        public string?       filename;
        public Cubemap?      cubemap;
        public ICubemapData? cubemapData;
    }

    [PublicAPI]
    public class CubemapParameter : AssetLoaderParameters
    {
        // The texture to put the TextureData in, optional.
        public Cubemap? cubemap = null;

        // CubemapData for textures created on the fly, optional.
        // When set, all format and genMipMaps are ignored
        public ICubemapData? cubemapData = null;

        // the format of the final Texture. Uses the source images format if null
        public Pixmap.Format? format    = null;
        public TextureFilter  magFilter = TextureFilter.Nearest;
        public TextureFilter  minFilter = TextureFilter.Nearest;
        public TextureWrap    wrapU     = TextureWrap.ClampToEdge;
        public TextureWrap    wrapV     = TextureWrap.ClampToEdge;
    }
}
