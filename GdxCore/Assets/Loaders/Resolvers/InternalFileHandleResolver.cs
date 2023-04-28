namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public sealed class InternalFileHandleResolver : IFileHandleResolver
    {
        public FileInfo Resolve( string fileName )
        {
            return Gdx.Files.Internal( fileName );
        }
    }
}

