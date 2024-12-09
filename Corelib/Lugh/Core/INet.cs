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

using Corelib.Lugh.Network;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Pooling;
using Exception = System.Exception;

namespace Corelib.Lugh.Core;

[PublicAPI]
public interface INet
{
    /// <summary>
    /// Represents the supported network protocols.
    /// </summary>
    [PublicAPI]
    public enum Protocol
    {
        Tcp,
    }

    /// <summary>
    /// Sends an HTTP request.
    /// </summary>
    /// <param name="httpRequest">The HTTP request to be sent.</param>
    /// <param name="httpResponseListener">The listener to handle the HTTP response.</param>
    public void SendHttpRequest( HttpRequest httpRequest, IHttpResponseListener httpResponseListener );

    /// <summary>
    /// Cancels an HTTP request.
    /// </summary>
    /// <param name="httpRequest">The HTTP request to be cancelled.</param>
    public void CancelHttpRequest( HttpRequest httpRequest );

    /// <summary>
    /// Opens a URI.
    /// </summary>
    /// <param name="uri">The URI to be opened.</param>
    /// <returns>True if the URI was successfully opened; otherwise, false.</returns>
    public bool OpenUri( string uri );

    /// <summary>
    /// Creates a new server socket.
    /// </summary>
    /// <param name="protocol">The protocol to be used by the server socket.</param>
    /// <param name="hostname">The hostname to bind the server socket to.</param>
    /// <param name="port">The port to bind the server socket to.</param>
    /// <param name="hints">Hints to customize the server socket behavior.</param>
    /// <returns>A new server socket, or null if the creation failed.</returns>
    public IServerSocket? NewServerSocket( Protocol protocol, string hostname, int port, ServerSocketHints hints );

    /// <summary>
    /// Creates a new server socket.
    /// </summary>
    /// <param name="protocol">The protocol to be used by the server socket.</param>
    /// <param name="port">The port to bind the server socket to.</param>
    /// <param name="hints">Hints to customize the server socket behavior.</param>
    /// <returns>A new server socket, or null if the creation failed.</returns>
    public IServerSocket? NewServerSocket( Protocol protocol, int port, ServerSocketHints hints );

    /// <summary>
    /// Creates a new client socket.
    /// </summary>
    /// <param name="protocol">The protocol to be used by the client socket.</param>
    /// <param name="host">The host to connect to.</param>
    /// <param name="port">The port to connect to.</param>
    /// <param name="hints">Hints to customize the client socket behavior.</param>
    /// <returns>A new client socket, or null if the creation failed.</returns>
    public ISocket? NewClientSocket( Protocol protocol, string host, int port, SocketHints hints );

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Represents an HTTP response.
    /// </summary>
    [PublicAPI]
    public interface IHttpResponse
    {
        /// <summary>
        /// Gets the result of the HTTP response as a byte array.
        /// </summary>
        /// <returns>The result as a byte array.</returns>
        byte[] GetResult();

        /// <summary>
        /// Gets the result of the HTTP response as a string.
        /// </summary>
        /// <returns>The result as a string.</returns>
        string GetResultAsString();

        /// <summary>
        /// Gets the result of the HTTP response as a stream.
        /// </summary>
        /// <returns>The result as a stream.</returns>
        StreamReader GetResultAsStream();

        /// <summary>
        /// Gets the status of the HTTP response.
        /// </summary>
        /// <returns>The status of the HTTP response.</returns>
        HttpStatus GetStatus();

        /// <summary>
        /// Gets the value of a specific header from the HTTP response.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <returns>The value of the header.</returns>
        string GetHeader( string name );

        /// <summary>
        /// Gets all the headers of the HTTP response.
        /// </summary>
        /// <returns>A dictionary containing all the headers.</returns>
        Dictionary< string, List< string > > GetHeaders();
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Represents HTTP methods.
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

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Represents a listener for HTTP responses.
    /// </summary>
    [PublicAPI]
    public interface IHttpResponseListener
    {
        /// <summary>
        /// Handles the HTTP response.
        /// </summary>
        /// <param name="httpResponse"> The HTTP response to handle. </param>
        void HandleHttpResponse( IHttpResponse httpResponse );

        /// <summary>
        /// Handles a failure in the HTTP request.
        /// </summary>
        /// <param name="t"> The exception that occurred. </param>
        void Failed( Exception t );

        /// <summary>
        /// Handles the cancellation of the HTTP request.
        /// </summary>
        void Cancelled();
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Represents an HTTP request.
    /// </summary>
    [PublicAPI]
    public class HttpRequest : IResetable
    {
        private readonly Dictionary< string, string >? _headers;
        private          bool                          _followRedirects = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="httpMethod"> The HTTP method for the request. </param>
        public HttpRequest( string? httpMethod = null )
        {
            _headers   = new Dictionary< string, string >();
            HttpMethod = httpMethod;
        }

        /// <summary>
        /// Gets or sets the URL for the HTTP request.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method for the request.
        /// </summary>
        public string? HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the timeout for the request in milliseconds.
        /// </summary>
        public int TimeOut { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether to include credentials in the request.
        /// </summary>
        public bool IncludeCredentials { get; set; } = false;

        /// <summary>
        /// Gets the content stream of the HTTP request.
        /// </summary>
        public StreamReader? ContentStream { get; private set; }

        /// <summary>
        /// Gets the content length of the HTTP request.
        /// </summary>
        public long ContentLength { get; private set; }

        /// <summary>
        /// Gets or sets the content of the HTTP request.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to follow redirects.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if setting follow redirects to false on WebGL backend.</exception>
        public bool FollowRedirects
        {
            get => _followRedirects;
            set
            {
                if ( value || ( Gdx.App.AppType != Platform.ApplicationType.WebGL ) )
                {
                    _followRedirects = value;
                }
                else
                {
                    throw new ArgumentException( "Following redirects can't be disabled using the GWT/WebGL backend!" );
                }
            }
        }

        /// <summary>
        /// Resets the HTTP request to its initial state.
        /// </summary>
        public void Reset()
        {
            HttpMethod       = null;
            Url              = null;
            TimeOut          = 0;
            Content          = null;
            ContentStream    = null;
            ContentLength    = 0;
            _followRedirects = true;
            _headers?.Clear();
        }

        /// <summary>
        /// Gets the headers of the HTTP request.
        /// </summary>
        /// <returns>A dictionary containing the headers.</returns>
        public Dictionary< string, string >? GetHeaders()
        {
            return _headers;
        }

        /// <summary>
        /// Sets a header for the HTTP request.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        public void SetHeader( string name, string value )
        {
            if ( _headers != null )
            {
                _headers[ name ] = value;
            }
        }
    }
}
