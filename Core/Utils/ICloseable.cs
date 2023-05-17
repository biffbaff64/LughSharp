using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils;

/// <summary>
/// A Closeable is a source or destination of data that can be closed.
/// The close method is invoked to release resources that the object
/// is holding (such as open files).
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public interface ICloseable
{
    public void Close();
}
