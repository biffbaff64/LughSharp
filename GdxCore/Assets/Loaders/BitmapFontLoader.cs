using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.UI;

namespace LibGDXSharp.Assets.Loaders
{
    /// <summary>
    /// </summary>
    public sealed class BitmapFontLoader
        : AsynchronousAssetLoader< BitmapFont, BitmapFontLoader.BitmapFontParameter >, IDisposable
    {
        private BitmapFont.BitmapFontData? _data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        public BitmapFontLoader( IFileInfoResolver resolver ) : base( resolver )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List< AssetDescriptor > GetDependencies( string fileName, FileInfo file, BitmapFontParameter? parameter )
        {
            var deps = new List< AssetDescriptor >();

            if ( parameter?.BitmapFontData != null )
            {
                _data = parameter.BitmapFontData;

                return deps;
            }

            _data = new BitmapFont.BitmapFontData( file, parameter != null && parameter.Flip );

            if ( parameter != null && parameter.AtlasName != null )
            {
                deps.Add( new AssetDescriptor( parameter.AtlasName, typeof(TextureAtlas) ) );
            }
            else
            {
                for ( var i = 0; i < _data.GetImagePaths().Length; i++ )
                {
                    var path     = _data.GetImagePath( i );
                    FileHandle? resolved = Resolve( path );

                    var textureParams = new TextureLoader.TextureParameter();

                    if ( parameter != null )
                    {
                        textureParams.GenMipMaps = parameter.GenMipMaps;
                        textureParams.MinFilter  = parameter.MinFilter;
                        textureParams.MagFilter  = parameter.MagFilter;
                    }

                    var descriptor = new AssetDescriptor< BitmapFont >( resolved, typeof(Texture), textureParams );
                    deps.Add( descriptor );
                }
            }

            return deps;
        }

        public class BitmapFontParameter : AssetLoaderParameters
        {
            /// <summary>
            /// Flips the font vertically if <tt>true</tt>. Defaults to <tt>false</tt>.
            /// </summary>
            internal bool Flip { get; set; } = false;

            /// <summary>
            /// Generates mipmaps for the font if <tt>true</tt>. Defaults to <tt>false</tt>.
            /// </summary>
            internal bool GenMipMaps { get; set; } = false;

            /// <summary>
            /// The <see cref="TextureFilter"/> to use when scaling down the <see cref="BitmapFont"/>.
            /// Defaults to <see cref="TextureFilter.Nearest"/>.
            /// </summary>
            internal TextureFilter MinFilter { get; set; } = TextureFilter.Nearest;

            /// <summary>
            /// The <see cref="TextureFilter"/> to use when scaling up the <see cref="BitmapFont"/>.
            /// Defaults to <see cref="TextureFilter.Nearest"/>.
            /// </summary>
            internal TextureFilter MagFilter { get; set; } = TextureFilter.Nearest;

            /// <summary>
            /// optional <see cref="BitmapFont.BitmapFontData"/> to be used instead of
            /// loading the <see cref="Texture"/> directly. Use this if your font is
            /// embedded in a <see cref="Skin"/>.
            /// </summary>
            internal BitmapFont.BitmapFontData? BitmapFontData { get; set; } = null;

            /// <summary>
            /// The name of the <see cref="TextureAtlas"/> to load the <see cref="BitmapFont"/>.
            /// if null, will look for a separate image.
            /// </summary>
            internal string? AtlasName { get; set; } = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        public override void LoadAsync( AssetManager manager,
                                        string fileName,
                                        FileHandle file,
                                        BitmapFontParameter parameter )
        {
        }

        public override BitmapFont LoadSync( AssetManager manager,
                                             string fileName,
                                             FileHandle file,
                                             BitmapFontParameter? parameter )
        {
            if ( parameter != null && parameter.AtlasName != null )
            {
                var atlas  = manager.Get( parameter.AtlasName, typeof(TextureAtlas) );
                var name   = file.Sibling( _data.imagePaths[ 0 ] ).nameWithoutExtension().toString();
                var region = atlas.FindRegion( name );

                if ( region == null )
                {
                    throw new GdxRuntimeException
                        (
                         "Could not find font region " + name + " in atlas " + parameter.AtlasName
                        );
                }

                return new BitmapFont( file, region );
            }
            else
            {
                var n    = _data.GetImagePaths().Length;
                var regs = new List<TextureRegion>( capacity: n );

                for ( var i = 0; i < n; i++ )
                {
                    regs.Add( new TextureRegion( manager.Get( _data.GetImagePath( i ), typeof(Texture))));
                }

                return new BitmapFont( _data, regs, true );
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Returns the assets this asset requires to be loaded first.
        /// This method may be called on a thread other than the GL thread.
        /// </summary>
        /// <param name="fileName">name of the asset to load</param>
        /// <param name="file">the resolved file to load</param>
        /// <param name="parameter">parameters for loading the asset</param>
        public override List< AssetDescriptor > GetDependencies( string? fileName, FileHandle? file, IAssetLoaderParameters parameter )
        {
            throw new NotImplementedException();
        }
    }
}
