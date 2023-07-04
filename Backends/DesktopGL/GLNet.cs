// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Network;

namespace LibGDXSharp.Backends.Desktop;

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