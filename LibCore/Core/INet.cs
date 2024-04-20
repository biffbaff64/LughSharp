// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Network;
using Exception = System.Exception;

namespace LughSharp.LibCore.Core;

[PublicAPI]
public interface INet
{
    public enum Protocol
    {
        Tcp
    }

    public void SendHttpRequest( HttpRequest httpRequest, IHttpResponseListener httpResponseListener );

    public void CancelHttpRequest( HttpRequest httpRequest );

    public bool OpenUri( string uri );

    public IServerSocket? NewServerSocket( Protocol protocol, string hostname, int port, ServerSocketHints hints );

    public IServerSocket? NewServerSocket( Protocol protocol, int port, ServerSocketHints hints );

    public ISocket? NewClientSocket( Protocol protocol, string host, int port, SocketHints hints );

    [PublicAPI]
    public interface IHttpResponse
    {
        byte[] GetResult();

        string GetResultAsString();

        StreamReader GetResultAsStream();

        HttpStatus GetStatus();

        string GetHeader( string name );

        Dictionary< string, List< string > > GetHeaders();
    }

    /// <summary>
    /// </summary>
    [PublicAPI]
    public interface IHttpMethods
    {
        public const string HEAD   = "HEAD";
        public const string GET    = "GET";
        public const string POST   = "POST";
        public const string PUT    = "PUT";
        public const string PATCH  = "PATCH";
        public const string DELETE = "DELETE";
    }

    [PublicAPI]
    public interface IHttpResponseListener
    {
        void HandleHttpResponse( IHttpResponse httpResponse );

        void Failed( Exception t );

        void Cancelled();
    }

    [PublicAPI]
    public class HttpRequest : IPoolable
    {
        public string?       Url                { get; set; }
        public string?       HttpMethod         { get; set; }
        public int           TimeOut            { get; set; } = 0;
        public bool          IncludeCredentials { get; set; } = false;
        public StreamReader? ContentStream      { get; private set; }
        public long          ContentLength      { get; private set; }
        public string?       Content            { get; set; }

        private readonly Dictionary< string, string >? _headers;
        private          bool                          _followRedirects = true;

        public HttpRequest()
        {
            _headers = new Dictionary< string, string >();
        }

        public HttpRequest( string httpMethod ) : this()
        {
            HttpMethod = httpMethod;
        }

        public bool FollowRedirects
        {
            get => _followRedirects;
            set
            {
                if ( value || ( Gdx.App.AppType != IApplication.ApplicationType.WebGL ) )
                {
                    _followRedirects = value;
                }
                else
                {
                    throw new ArgumentException
                        ( "Following redirects can't be disabled using the GWT/WebGL backend!" );
                }
            }
        }

        public void Reset()
        {
            HttpMethod = null;
            Url        = null;
            _headers?.Clear();
            TimeOut = 0;

            Content       = null;
            ContentStream = null;
            ContentLength = 0;

            _followRedirects = true;
        }

        public Dictionary< string, string >? GetHeaders()
        {
            return _headers;
        }

        public void SetHeader( string name, string value )
        {
            if ( _headers != null )
            {
                _headers[ name ] = value;
            }
        }

        public void SetContent( StreamReader contentStream, long contentLength )
        {
            ContentStream = contentStream;
            ContentLength = contentLength;
        }
    }
}
