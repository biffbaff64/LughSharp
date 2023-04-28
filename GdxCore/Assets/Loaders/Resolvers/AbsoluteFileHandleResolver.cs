using LibGDXSharp.Core;

namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class AbsoluteFileHandleResolver : IFileHandleResolver
    {
        public FileInfo? Resolve( string fileName )
        {
            return Gdx.Files.Absolute( fileName );
        }
    }
}

