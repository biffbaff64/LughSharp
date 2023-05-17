namespace LibGDXSharp.Assets;

public interface IAssetLoaderParameters
{
    /// <summary>
    /// Callback interface that will be invoked when the <see cref="AssetManager"/> loaded an asset.
    /// </summary>
    public interface ILoadedCallback
    {
        public void FinishedLoading( AssetManager assetManager, string fileName, Type? type );
    }

    public ILoadedCallback? LoadedCallback { get; set; }
}

public class AssetLoaderParameters : IAssetLoaderParameters
{
    public IAssetLoaderParameters.ILoadedCallback? LoadedCallback { get; set; }
}