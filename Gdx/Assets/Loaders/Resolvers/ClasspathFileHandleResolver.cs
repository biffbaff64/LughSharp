using LibGDXSharp.Core;

namespace LibGDXSharp.Assets.Loaders.Resolvers
{
    public class ClasspathFileHandleResolver : IFileHandleResolver
    {
        public FileHandle? Resolve( String fileName )
        {
            return Gdx.Files.Classpath( fileName );
        }
    }
}
