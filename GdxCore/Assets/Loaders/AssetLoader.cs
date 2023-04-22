using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Assets.Loaders
{
    /// <summary>
    /// Abstract base class for asset loaders.
    /// </summary>
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public abstract class AssetLoader
    {
        public IFileInfoResolver Resolver      { get; set; }
        public bool              IsSynchronous { get; protected init; } = false;

        /// <summary>
        /// Constructor, sets the FileHandleResolver to use to resolve the file
        /// associated with the asset name.
        /// </summary>
        protected AssetLoader( IFileInfoResolver resolver )
        {
            this.Resolver = resolver;
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileInfo Resolve( string fileName )
        {
            return Resolver.Resolve( fileName );
        }

        /// <summary>
        /// Returns the assets this asset requires to be loaded first.
        /// This method may be called on a thread other than the GL thread.
        /// </summary>
        /// <param name="fileName">name of the asset to load</param>
        /// <param name="file">the resolved file to load</param>
        /// <param name="parameter">parameters for loading the asset</param>
        public abstract List< AssetDescriptor > GetDependencies( string? fileName, FileInfo? file, IAssetLoaderParameters parameter );
    }
}
