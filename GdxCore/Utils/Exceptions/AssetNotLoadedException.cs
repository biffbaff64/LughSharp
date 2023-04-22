namespace LibGDXSharp.Utils
{
    public class AssetNotLoadedException : Exception
    {
        public Type                   Type   { get; private set; }
        public string                 Path   { get; private set; }
        public IAssetLoaderParameters Params { get; private set; }

        public AssetNotLoadedException( Type type, string path, IAssetLoaderParameters param  )
            : base( $"Asset not loaded: '{path}' ({type})" )
        {
            this.Type   = type;
            this.Path   = path;
            this.Params = param;
        }

        public AssetNotLoadedException( AssetDescriptor desc )
            : this( desc.Type, desc.FilePath, desc.Parameters )
        {
        }
    }
}

