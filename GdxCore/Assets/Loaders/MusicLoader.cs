namespace LibGDXSharp.Assets.Loaders
{
    public class MusicLoader
        : AsynchronousAssetLoader< Texture, TextureLoader.TextureParameter >, IDisposable
    {
        public MusicLoader( IFileHandleResolver resolver ) : base( resolver )
        {
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

        /// <summary>
        /// Loads the non-OpenGL part of the asset and injects any dependencies of
        /// the asset into the AssetManager.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        public override void LoadAsync( AssetManager? manager, string? fileName, FileHandle? file, TextureLoader.TextureParameter? parameter )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the OpenGL part of the asset.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override Texture LoadSync( AssetManager? manager, string? fileName, FileHandle? file, TextureLoader.TextureParameter? parameter )
        {
            throw new NotImplementedException();
        }
    }
}

