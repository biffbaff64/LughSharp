using LibGDXSharp.G2D;

namespace LibGDXSharp.Assets.Loaders
{
    /// <summary>
    /// AssetLoader to load TextureAtlas instances.
    /// Passing a <see cref="TextureAtlasParameter"/> to
    /// <see cref="AssetManager.Load(String, Type, IAssetLoaderParameters)"/>
    /// allows to specify whether the atlas regions should be flipped on the
    /// y-axis or not.
    /// </summary>
    public class TextureAtlasLoader : SynchronousAssetLoader< TextureAtlas, TextureAtlasParameter >
    {
        private TextureAtlasData? _data;

        public TextureAtlasLoader( IFileHandleResolver resolver )
            : base( resolver )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="assetManager"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override TextureAtlas Load( AssetManager assetManager,
                                           string fileName,
                                           FileInfo? file,
                                           TextureAtlasParameter parameter )
        {
            foreach ( TextureAtlasData.Page page in _data.GetPages() )
            {
                Texture texture = assetManager.Get( page.textureFile.Path().ReplaceAll( "\\\\", "/" ), typeof(Texture) );
                page.texture = texture;
            }

            var atlas = new TextureAtlas( _data );
            _data = null;

            return atlas;
        }

        public List< AssetDescriptor > GetDependencies( string fileName,
                                                        FileInfo atlasFile,
                                                        TextureAtlasParameter? parameter )
        {
            DirectoryInfo? imgDir = atlasFile.Directory;

            if ( parameter != null )
            {
                _data = new TextureAtlasData( atlasFile, imgDir, parameter.flip );
            }
            else
            {
                _data = new TextureAtlasData( atlasFile, imgDir, false );
            }

            var dependencies = new List< AssetDescriptor >();

            foreach ( TextureAtlasData.Page page in _data.GetPages() )
            {
                var tparams = new TextureLoader.TextureParameter
                {
                    Format     = page.Format,
                    GenMipMaps = page.UseMipMaps,
                    MinFilter  = page.MinFilter,
                    MagFilter  = page.MagFilter
                };

                dependencies.Add( new AssetDescriptor( page.textureFile, typeof(Texture), tparams ) );
            }

            return dependencies;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }

    public class TextureAtlasParameter : AssetLoaderParameters
    {
        // whether to flip the texture atlas vertically.
        public bool flip = false;

        public TextureAtlasParameter()
        {
        }

        public TextureAtlasParameter( bool flip )
        {
            this.flip = flip;
        }
    }
}
