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
    }
}

