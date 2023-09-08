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

using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Core;

[PublicAPI]
public interface INet
{
    public interface IHttpResponse
    {
        byte[] GetResult();

        string GetResultAsString();

        StreamReader GetResultAsStream();

//        HttpStatus GetStatus();

        string GetHeader( string name );

        Dictionary< string, List< string > > GetHeaders();
    }

    /// <summary>
    /// </summary>
    public interface IHttpMethods
    {
        public const string HEAD   = "HEAD";
        public const string GET    = "GET";
        public const string POST   = "POST";
        public const string PUT    = "PUT";
        public const string PATCH  = "PATCH";
        public const string DELETE = "DELETE";
    }

    public interface IHttpResponseListener
    {
        void HandleHttpResponse( IHttpResponse httpResponse );

        void Failed( Exception t );

        void Cancelled();
    }

    public enum Protocol
    {
        Tcp
    }

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
            this._headers = new Dictionary< string, string >();
        }

        public HttpRequest( string httpMethod ) : this()
        {
            this.HttpMethod = httpMethod;
        }

        public Dictionary< string, string >? GetHeaders()
        {
            return _headers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetHeader( string name, string value )
        {
            if ( _headers != null )
            {
                _headers[ name ] = value;
            }
        }

        public bool FollowRedirects
        {
            get => _followRedirects;
            set
            {
                if ( value || ( Gdx.App.AppType != IApplication.ApplicationType.WebGL ) )
                {
                    this._followRedirects = value;
                }
                else
                {
                    throw new ArgumentException
                        ( "Following redirects can't be disabled using the GWT/WebGL backend!" );
                }
            }
        }

        public void SetContent( StreamReader contentStream, long contentLength )
        {
            this.ContentStream = contentStream;
            this.ContentLength = contentLength;
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
    }

    public void SendHttpRequest( HttpRequest httpRequest, IHttpResponseListener httpResponseListener );

    public void CancelHttpRequest( HttpRequest httpRequest );

    public bool OpenUri( string uri );

//    public IServerSocket NewServerSocket( Protocol protocol, string hostname, int port, ServerSocketHints hints );

//    public IServerSocket NewServerSocket( Protocol protocol, int port, ServerSocketHints hints );

//    public ISocket NewClientSocket( Protocol protocol, string host, int port, SocketHints hints );
}