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
using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Graphics.Images;
using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Maps;

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
    /// A class that resolves images directly from a dictionary of textures.
    /// Implements the IImageResolver interface.
    /// </summary>
    [PublicAPI]
    public class DirectImageResolver : IImageResolver
    {
        // A dictionary that maps image names to Texture objects.
        private readonly Dictionary< string, Texture > _images;

        /// <summary>
        /// Initializes a new instance of the DirectImageResolver class with the
        /// specified dictionary of images.
        /// </summary>
        /// <param name="images">A dictionary mapping image names to Texture objects.</param>
        public DirectImageResolver( Dictionary< string, Texture > images )
        {
            _images = images ?? throw new ArgumentNullException( nameof( images ), "Images dictionary cannot be null" );
        }

        /// <summary>
        /// Retrieves the texture region for the specified image name.
        /// </summary>
        /// <param name="name">The name of the image to retrieve.</param>
        /// <returns>
        /// A <see cref="TextureRegion"/> object representing the texture region of the specified image.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the specified image name is not found in the dictionary.
        /// </exception>
        public TextureRegion GetImage( string name )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "Image name cannot be null or whitespace", nameof( name ) );
            }

            if ( !_images.TryGetValue( name, out var texture ) )
            {
                throw new KeyNotFoundException( $"The image with name '{name}' was not found in the images dictionary." );
            }

            return new TextureRegion( texture );
        }
    }

    /// <summary>
    /// Image Resolver for TextureRegions fetched via an <see cref="AssetManager"/>
    /// </summary>
    [PublicAPI]
    public class AssetManagerImageResolver : IImageResolver
    {
        private readonly AssetManager _assetManager;

        /// <summary>
        /// Initializes a new instance of the AssetManagerImageResolver class with the specified AssetManager.
        /// </summary>
        /// <param name="assetManager">The AssetManager used to manage and retrieve assets.</param>
        public AssetManagerImageResolver( AssetManager assetManager )
        {
            _assetManager = assetManager 
                         ?? throw new ArgumentNullException( nameof( assetManager ), "AssetManager cannot be null" );
        }

        /// <summary>
        /// Retrieves the texture region for the specified image name.
        /// </summary>
        /// <param name="name">The name of the image to retrieve.</param>
        /// <returns>
        /// A <see cref="TextureRegion"/> object representing the texture region of the specified image.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the specified image name is null or empty.
        /// </exception>
        /// <exception cref="AssetNotLoadedException">
        /// Thrown if the specified image is not loaded by the AssetManager.
        /// </exception>
        public TextureRegion GetImage( string name )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentNullException( nameof( name ), "Image name cannot be null or whitespace" );
            }

            if ( !_assetManager.IsLoaded( name, typeof( Texture ) ) )
            {
                throw new AssetNotLoadedException( $"The asset '{name}' is not loaded." );
            }

            var texture = _assetManager.Get( name ) as Texture;

            return new TextureRegion( texture! );
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
