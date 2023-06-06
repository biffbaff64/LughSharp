using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.DevFillers;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class DefaultFileSystem
{
    public static FileSystem? FileSystem { get; set; }
}