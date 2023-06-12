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

using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps;

/// <summary>
/// esolves an image by a string, wrapper around a Map or AssetManager to load
/// maps either directly or via AssetManager.
/// </summary>
public interface IImageResolver
{
    /// <summary>
    /// Returns the <see cref="TextureRegion"/> for the given name,
    /// or null if texture doesn't exist.
    /// </summary>
    public TextureRegion GetImage( string name );

    /// <summary>
    /// </summary>
    public sealed class DirectImageResolver : IImageResolver
    {
        private readonly Dictionary< string, Texture > _images;

        internal DirectImageResolver( Dictionary< string, Texture > images )
        {
            this._images = images;
        }

        public TextureRegion GetImage( string name )
        {
            return new TextureRegion( _images[ name ] );
        }
    }

    /// <summary>
    /// </summary>
    public sealed class AssetManagerImageResolver : IImageResolver
    {
        private readonly AssetManager _assetManager;

        internal AssetManagerImageResolver( AssetManager assetManager )
        {
            this._assetManager = assetManager;
        }

        public TextureRegion GetImage( string name )
        {
            return new TextureRegion( _assetManager.Get< Texture >( name ) );
        }
    }

    /// <summary>
    /// </summary>
    public class TextureAtlasImageResolver : IImageResolver
    {
        private readonly TextureAtlas _atlas;

        public TextureAtlasImageResolver( TextureAtlas atlas )
        {
            this._atlas = atlas;
        }

        public TextureRegion GetImage( string name )
        {
            return _atlas.FindRegion( name );
        }
    }
}