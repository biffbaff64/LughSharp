namespace LibGDXSharp.Assets.Loaders.Resolvers;

/// <summary>
/// An interface for classes that can map a file name to a FileInfo.
/// Used to allow the AssetManager to load files from anywhere, or
/// implement caching strategies.
/// </summary>
public interface IFileHandleResolver
{
    public FileInfo Resolve( string fileName );
}