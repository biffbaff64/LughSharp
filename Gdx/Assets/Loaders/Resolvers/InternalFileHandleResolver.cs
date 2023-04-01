using LibGDXSharp.Core;

namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class InternalFileHandleResolver : IFileHandleResolver
    {
        public FileHandle? Resolve( string fileName )
        {
            return Gdx.Files?.Internal( fileName );
        }
    }
}

