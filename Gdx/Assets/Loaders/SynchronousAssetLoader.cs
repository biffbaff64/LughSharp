namespace LibGDXSharp.Assets.Loaders
{
    public abstract class SynchronousAssetLoader<T, TP> : AssetLoader< T, TP > where TP : AssetLoaderParameters< T >
    {
        protected SynchronousAssetLoader( IFileHandleResolver resolver ) : base( resolver )
        {
            IsSynchronous = true;
        }

        public abstract T Load( AssetManager assetManager, string fileName, FileHandle file, TP parameter );
    }
}
