namespace LibGDXSharp.Assets.Loaders
{
    public class SoundLoader : AsynchronousAssetLoader
    {
        public SoundLoader( IFileHandleResolver resolver ) : base( resolver )
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

