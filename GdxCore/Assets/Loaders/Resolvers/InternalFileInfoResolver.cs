namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public sealed class InternalFileInfoResolver : IFileInfoResolver
    {
        public FileInfo Resolve( string fileName )
        {
            return Gdx.Files.Internal( fileName );
        }
    }
}

