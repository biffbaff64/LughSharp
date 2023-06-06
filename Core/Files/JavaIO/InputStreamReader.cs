using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Files;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class InputStreamReader : InputStream
{
    /// <summary>
    /// Creates an InputStreamReader that uses the named charset.
    /// </summary>
    /// <param name="istream">An InputStream</param>
    /// <param name="charset">The name of a supported Charset</param>
    ///
    /// @exception  UnsupportedEncodingException
    ///             If the named charset is not supported
    public InputStreamReader( InputStream istream, string charset ) : base()
    {
    }
}
