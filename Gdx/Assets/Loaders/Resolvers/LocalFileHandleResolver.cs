using LibGDXSharp.Core;

namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class LocalFileHandleResolver : IFileHandleResolver
    {
        public FileHandle? Resolve( string fileName )
        {
            return Gdx.Files?.Local( fileName );
        }
    }
}

