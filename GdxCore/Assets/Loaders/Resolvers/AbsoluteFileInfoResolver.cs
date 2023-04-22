using LibGDXSharp.Core;

namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class AbsoluteFileInfoResolver : IFileInfoResolver
    {
        public FileInfo? Resolve( string fileName )
        {
            return Core.Gdx.Files.Absolute( fileName );
        }
    }
}

