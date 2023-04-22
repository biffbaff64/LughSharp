namespace LibGDXSharp.Assets
{
    public interface IAssetErrorListener
    {
        public void Error( AssetDescriptor asset, Exception throwable );
    }
}
