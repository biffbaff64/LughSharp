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

namespace Corelib.Lugh.Audio.Maponus.Decoding;

/// <summary>
/// The DecoderException represents the class of errors that can occur when decoding MPEG audio.
/// </summary>
/// <remarks>
/// This exception is used to signal errors that occur specifically during the decoding of MPEG audio streams.
/// It extends from the <see cref="Mp3SharpException"/> class and adds additional context with an error code.
/// </remarks>
[Serializable, PublicAPI]
public class DecoderException : Mp3SharpException
{
    /// <summary>
    /// Gets or sets the error code associated with this exception.
    /// </summary>
    public int ErrorCode { get; set; }

    // ========================================================================

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoderException"/> class with a specified
    /// error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">
    /// The exception that is the cause of the current exception, or a null reference if no inner
    /// exception is specified.
    /// </param>
    public DecoderException( string message, Exception? inner )
        : base( message, inner )
    {
        ErrorCode = DecoderErrors.UNKNOWN_ERROR;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoderException"/> class with a specified
    /// error code and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="errorcode">The error code that explains the reason for the exception.</param>
    /// <param name="inner">
    /// The exception that is the cause of the current exception, or a null reference if no inner
    /// exception is specified.
    /// </param>
    public DecoderException( int errorcode, Exception? inner )
        : this( GetErrorString( errorcode ), inner )
    {
        ErrorCode = errorcode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DecoderException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected DecoderException( SerializationInfo info, StreamingContext context ) : base( info, context )
    {
        ErrorCode = info.GetInt32( "ErrorCode" );
    }

    /// <summary>
    /// Sets the <see cref="SerializationInfo"/> with information about the exception.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <exception cref="ArgumentNullException">Thrown when the info parameter is null.</exception>
    [Obsolete( "This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}" )]
    public override void GetObjectData( SerializationInfo info, StreamingContext context )
    {
        //TODO: This needs replacing!!

        ArgumentNullException.ThrowIfNull( info );

        info.AddValue( "ErrorCode", ErrorCode );
//        base.GetObjectData( info, context );
    }

    /// <summary>
    /// Returns the error message that corresponds to the specified error code.
    /// </summary>
    /// <param name="errorcode">The error code for which to get the error message.</param>
    /// <returns>A string that represents the error message.</returns>
    /// <remarks>
    /// This method can be extended to use resource files to provide locale-sensitive error messages.
    /// </remarks>
    public static string GetErrorString( int errorcode )
    {
        // TODO: use resource file to map error codes to locale-sensitive strings. 
        return $"Decoder errorcode {Convert.ToString( errorcode, 16 )}";
    }
}
