using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LibGDXSharp.Utils;

public class GdxRuntimeException : Exception
{
    public GdxRuntimeException( string message ) : base( message )
    {
    }

    public GdxRuntimeException( Exception e ) : this( "", e )
    {
    }
        
    public GdxRuntimeException( string message, Exception? exception )
        : base( message, exception )
    {
    }
}
