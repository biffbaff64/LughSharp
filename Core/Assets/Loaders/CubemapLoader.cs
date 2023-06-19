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

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Assets.Loaders;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class CubemapLoader : AsynchronousAssetLoader< Cubemap, CubemapLoader.CubemapParameter >
{
    public CubemapLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    public class CubemapParameter : AssetLoaderParameters
    {
        // the format of the final Texture. Uses the source images format if null
        public Pixmap.Format? format = null;

        // The texture to put the TextureData in, optional.
        public Cubemap? cubemap = null;

        // CubemapData for textures created on the fly, optional.
        // When set, all format and genMipMaps are ignored
        public ICubemapData? cubemapData = null;
        public TextureFilter minFilter   = TextureFilter.Nearest;
        public TextureFilter magFilter   = TextureFilter.Nearest;
        public TextureWrap   wrapU       = TextureWrap.ClampToEdge;
        public TextureWrap   wrapV       = TextureWrap.ClampToEdge;
    }
}