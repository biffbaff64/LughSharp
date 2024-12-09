// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using Corelib.Lugh.Core;
using Corelib.Lugh.Network;
using JetBrains.Annotations;

namespace DesktopGLBackend.Core;

[PublicAPI]
public class DesktopGLNet : INet
{
    public DesktopGLNet( DesktopGLApplicationConfiguration config )
    {
    }

    public void SendHttpRequest( INet.HttpRequest httpRequest,
                                 INet.IHttpResponseListener httpResponseListener )
    {
    }

    public void CancelHttpRequest( INet.HttpRequest httpRequest )
    {
    }

    public IServerSocket? NewServerSocket( INet.Protocol protocol,
                                           string hostname,
                                           int port,
                                           ServerSocketHints hints )
    {
        return null;
    }

    public IServerSocket? NewServerSocket( INet.Protocol protocol,
                                           int port,
                                           ServerSocketHints hints )
    {
        return null;
    }

    public ISocket? NewClientSocket( INet.Protocol protocol,
                                     string host,
                                     int port,
                                     SocketHints hints )
    {
        return null;
    }

    public bool OpenUri( string uri )
    {
        //        if ( SharedLibraryLoader.IsMac )
        //        {
        //            try
        //            {
        //                FileManager.OpenURL( uri );
        //
        //                return true;
        //            }
        //            catch ( IOException e )
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                Desktop.GetDesktop().Browse( new URI( uri ) );
        //
        //                return true;
        //            }
        //            catch ( System.Exception )
        //            {
        //                return false;
        //            }
        //        }
        return false;
    }
}
