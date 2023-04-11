using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps
{
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
                return new TextureRegion( _assetManager.Get<Texture>( name, typeof(Texture)));
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

}
