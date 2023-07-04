// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Graphics.G3D.Models.Data;

namespace LibGDXSharp.Graphics.G3D.Utils;

/// <summary>
/// Used by <see cref="Model"/> to load textures from <see cref="ModelData"/>.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public interface ITextureProvider
{
    Texture Load( string fileName );

    public class FileTextureProvider : ITextureProvider
    {
        private readonly TextureFilter _minFilter;
        private readonly TextureFilter _magFilter;
        private readonly TextureWrap   _uWrap;
        private readonly TextureWrap   _vWrap;
        private readonly bool          _useMipMaps;

        public FileTextureProvider()
        {
            _minFilter  = _magFilter = TextureFilter.Linear;
            _uWrap      = _vWrap     = TextureWrap.Repeat;
            _useMipMaps = false;
        }

        public FileTextureProvider( TextureFilter minFilter, TextureFilter magFilter,
                                    TextureWrap uWrap, TextureWrap vWrap, bool useMipMaps )
        {
            this._minFilter  = minFilter;
            this._magFilter  = magFilter;
            this._uWrap      = uWrap;
            this._vWrap      = vWrap;
            this._useMipMaps = useMipMaps;
        }

        public Texture Load( string fileName )
        {
            var result = new Texture( Gdx.Files.Internal( fileName ), _useMipMaps );
            result.SetFilter( _minFilter, _magFilter );
            result.SetWrap( _uWrap, _vWrap );

            return result;
        }
    }

    public class AssetTextureProvider : ITextureProvider
    {
        public AssetManager? AssetManager { get; set; }

        public AssetTextureProvider( AssetManager? assetManager )
        {
            this.AssetManager = assetManager;
        }

        public Texture Load( string fileName )
        {
            if ( AssetManager == null ) throw new NullReferenceException();
            
            return AssetManager.Get< Texture >( fileName );
        }
    }
}
