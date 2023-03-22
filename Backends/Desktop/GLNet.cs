using LibGDXSharp.Core;
using LibGDXSharp.Network;

namespace LibGDXSharp.Backends.Desktop
{
    public class GLNet : INet
    {
        public GLNet( GLApplicationConfiguration config )
        {
        }

        public void SendHttpRequest( INet.HttpRequest httpRequest, INet.IHttpResponseListener httpResponseListener )
        {
        }

        public void CancelHttpRequest( INet.HttpRequest httpRequest )
        {
        }

        public IServerSocket NewServerSocket( INet.Protocol protocol, string hostname, int port, ServerSocketHints hints )
        {
            return null;
        }

        public IServerSocket NewServerSocket( INet.Protocol protocol, int port, ServerSocketHints hints )
        {
            return null;
        }

        public ISocket NewClientSocket( INet.Protocol protocol, string host, int port, SocketHints hints )
        {
            return null;
        }

        public bool OpenUri( string uri )
        {
            return false;
        }
    }
}

