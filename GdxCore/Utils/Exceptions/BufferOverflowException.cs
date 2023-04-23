using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public class BufferOverflowException : Exception
    {
        public BufferOverflowException() : base()
        {
        }
        
        public BufferOverflowException( string message ) : base( message )
        {
        }

        public BufferOverflowException( Exception e ) : this( "", e )
        {
        }

        public BufferOverflowException( string message, Exception? exception ) : base( message, exception )
        {
        }
    }
}

