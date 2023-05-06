namespace LibGDXSharp.Assets.Loaders;

public class PixmapParameter
{
}
    
public class PixmapLoader : AsynchronousAssetLoader
{
    public PixmapLoader( IFileHandleResolver resolver) : base( resolver )
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