using LibGDXSharp.Core;

namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class AbsoluteFileInfoResolver : IFileInfoResolver
    {
        public FileInfo? Resolve( string fileName )
        {
            return Gdx.Files.Absolute( fileName );
        }
    }
}

