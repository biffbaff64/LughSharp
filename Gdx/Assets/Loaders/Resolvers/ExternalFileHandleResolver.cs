using LibGDXSharp.Core;

namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class ExternalFileHandleResolver : IFileHandleResolver
    {
        public FileHandle? Resolve( string fileName )
        {
            return Gdx.Files.External( fileName );
        }
    }
}

