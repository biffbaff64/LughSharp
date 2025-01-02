// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Assets;
using LughSharp.Lugh.Graphics.G3D.Models.Data;
using LughSharp.Lugh.Graphics.Images;

namespace LughSharp.Lugh.Graphics.G3D.Utils;

/// <summary>
/// Used by <see cref="Model"/> to load textures from <see cref="ModelData"/>.
/// </summary>
[PublicAPI]
public interface ITextureProvider
{
    Texture Load( string fileName );

    [PublicAPI]
    public class FileTextureProvider : ITextureProvider
    {
        private readonly Texture.TextureFilter _magFilter;
        private readonly Texture.TextureFilter _minFilter;
        private readonly bool                  _useMipMaps;
        private readonly Texture.TextureWrap   _uWrap;
        private readonly Texture.TextureWrap   _vWrap;

        public FileTextureProvider()
        {
            _minFilter  = _magFilter = Texture.TextureFilter.Linear;
            _uWrap      = _vWrap     = Texture.TextureWrap.Repeat;
            _useMipMaps = false;
        }

        public FileTextureProvider( Texture.TextureFilter minFilter,
                                    Texture.TextureFilter magFilter,
                                    Texture.TextureWrap uWrap,
                                    Texture.TextureWrap vWrap,
                                    bool useMipMaps )
        {
            _minFilter  = minFilter;
            _magFilter  = magFilter;
            _uWrap      = uWrap;
            _vWrap      = vWrap;
            _useMipMaps = useMipMaps;
        }

        public Texture Load( string fileName )
        {
            var result = new Texture( GdxApi.Files.Internal( fileName ).File, _useMipMaps );
            result.SetFilter( _minFilter, _magFilter );
            result.SetWrap( _uWrap, _vWrap );

            return result;
        }
    }

    public class AssetTextureProvider : ITextureProvider
    {
        public AssetTextureProvider( AssetManager? assetManager )
        {
            AssetManager = assetManager;
        }

        private AssetManager? AssetManager { get; }

        public Texture Load( string fileName )
        {
            if ( AssetManager == null )
            {
                throw new NullReferenceException();
            }

            if ( AssetManager.Get( fileName ) is not Texture texture )
            {
                throw new NullReferenceException( $"Loaded texture {fileName} is null!" );
            }

            return texture;
        }
    }
}
