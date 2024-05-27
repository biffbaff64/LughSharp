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


namespace LughSharp.LibCore.Maps;

/// <summary>
/// Resolves an image by a string, wrapper around a Map or AssetManager to load
/// maps either directly or via AssetManager.
/// </summary>
[PublicAPI]
public interface IImageResolver
{
    /// <summary>
    /// Returns the <see cref="TextureRegion"/> for the given name,
    /// or null if texture doesn't exist.
    /// </summary>
    public TextureRegion? GetImage( string name );

    /// <summary>
    /// </summary>
    [PublicAPI]
    public class DirectImageResolver : IImageResolver
    {
        private readonly Dictionary< string, Texture > _images;

        internal DirectImageResolver( Dictionary< string, Texture > images )
        {
            _images = images;
        }

        public TextureRegion GetImage( string name )
        {
            return new TextureRegion( _images[ name ] );
        }
    }

    /// <summary>
    /// Image Resolver for TextureRegions fetched via an <see cref="AssetManager"/>
    /// </summary>
    [PublicAPI]
    public class AssetManagerImageResolver : IImageResolver
    {
        private readonly AssetManager _assetManager;

        internal AssetManagerImageResolver( AssetManager assetManager )
        {
            _assetManager = assetManager;
        }

        public TextureRegion GetImage( string name )
        {
            return new TextureRegion( _assetManager.Get< Texture >( name ) );
        }
    }

    /// <summary>
    /// Resolver for TextureAtlas regions.
    /// Provides a means of accessing TextureRegions in the atlas.
    /// </summary>
    [PublicAPI]
    public class TextureAtlasImageResolver : IImageResolver
    {
        private readonly TextureAtlas _atlas;

        /// <summary>
        /// Constructor.
        /// Creates an Image Resolver for the supplied TextureAtlas.
        /// </summary>
        /// <param name="atlas"></param>
        public TextureAtlasImageResolver( TextureAtlas atlas )
        {
            _atlas = atlas;
        }

        /// <summary>
        /// Gets the TextureRegion that matches the supplied name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TextureRegion? GetImage( string name )
        {
            return _atlas.FindRegion( name );
        }
    }
}
