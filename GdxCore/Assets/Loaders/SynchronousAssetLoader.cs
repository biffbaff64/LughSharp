using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Assets.Loaders
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public abstract class SynchronousAssetLoader<T, TP> : AssetLoader
    {
        protected SynchronousAssetLoader( IFileInfoResolver resolver ) : base( resolver )
        {
            IsSynchronous = true;
        }

        public abstract T? Load( AssetManager assetManager, string fileName, FileInfo? file, TP parameter );
    }
}
