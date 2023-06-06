using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Files;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class InputStream : ICloseable
{
    public void Close()
    {
    }
}
