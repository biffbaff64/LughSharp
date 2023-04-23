using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public class BufferUnderflowException : Exception
    {
        public BufferUnderflowException() : base()
        {
        }
        
        public BufferUnderflowException( string message ) : base( message )
        {
        }

        public BufferUnderflowException( Exception e ) : this( "", e )
        {
        }

        public BufferUnderflowException( string message, Exception? exception ) : base( message, exception )
        {
        }
    }
}

