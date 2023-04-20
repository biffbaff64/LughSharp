namespace LibGDXSharp.Assets
{
    public interface IAssetErrorListener
    {
        public void Error<T>( AssetDescriptor asset, Exception throwable );
    }
}
