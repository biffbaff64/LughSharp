// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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


using Exception = System.Exception;

namespace Corelib.LibCore.Audio.Maponus;

/// <summary>
/// MP3SharpException is the base class for all API-level exceptions
/// thrown by MP3Sharp.
/// </summary>
[Serializable, PublicAPI]
public class Mp3SharpException : ApplicationException
{
    /// <summary>
    /// Initializes a new Mp3SharpException with a specified error message.
    /// </summary>
    /// <param name="message"> The message that describes the error. </param>
    public Mp3SharpException( string message ) : base( message )
    {
    }

    /// <summary>
    /// Initializes a new Mp3SharpException with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="exception">
    /// The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.
    /// </param>
    public Mp3SharpException( string message, Exception? exception )
        : base( message, exception )
    {
    }

    /// <summary>
    /// Initializes a new MP3SharpException with serialized data.
    /// </summary>
    /// <param name="info">
    /// The <see cref="SerializationInfo"/> that holds the serialized object data about
    /// the exception being thrown.
    /// </param>
    /// <param name="context">
    /// The <see cref="StreamingContext"/> that contains contextual information about
    /// the source or destination.
    /// </param>
    protected Mp3SharpException( SerializationInfo info, StreamingContext context )
        //: base( info, context )
    {
    }
}
