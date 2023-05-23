using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Assets.Loaders.Resolvers;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class AbsoluteFileHandleResolver : IFileHandleResolver
{
    public FileInfo Resolve( string fileName )
    {
        return Gdx.Files.Absolute( fileName );
    }
}