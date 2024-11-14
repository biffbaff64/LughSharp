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

namespace Corelib.LibCore.Audio.Maponus.Decoding;

/// <summary>
/// Instances of BitstreamException are thrown
/// when operations on a Bitstream fail.
/// <para>
/// The exception provides details of the exception condition
/// in two ways:
/// <li>
/// as an error-code describing the nature of the error
/// </li>
/// <li>
/// as the Throwable instance, if any, that was thrown
/// indicating that an exceptional condition has occurred.
/// </li>
/// </para>
/// </summary>
[Serializable, PublicAPI]
public class BitstreamException : Mp3SharpException
{
    // ========================================================================

    /// <summary>
    /// Initializes a new Mp3SharpException with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// </param>
    /// <param name="inner">
    /// The exception that is the cause of the current exception, or a null reference
    /// if no inner exception is specified.
    /// </param>
    public BitstreamException( string message, Exception? inner = null )
        : base( message, inner )
    {
        ErrorCode = BitstreamErrors.UNKNOWN_ERROR;
    }

    /// <summary>
    /// Initializes a new Mp3SharpException with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="errorcode">
    /// The errorcode which is used to extract the message that explains the
    /// reason for the exception.
    /// </param>
    /// <param name="inner">
    /// The exception that is the cause of the current exception, or a null reference
    /// if no inner exception is specified.
    /// </param>
    public BitstreamException( int errorcode, Exception? inner = null )
        : this( GetErrorString( errorcode ), inner )
    {
        ErrorCode = errorcode;
    }

    /// <summary>
    /// Initializes a new BitstreamException with serialized data.
    /// </summary>
    /// <param name="info">
    /// The <see cref="SerializationInfo"/> that holds the serialized object
    /// data about the exception being thrown.
    /// </param>
    /// <param name="context">
    /// The <see cref="StreamingContext"/> that contains contextual information
    /// about the source or destination.
    /// </param>
    protected BitstreamException( SerializationInfo info, StreamingContext context )
        : base( info, context )
    {
        ErrorCode = info.GetInt32( "ErrorCode" );
    }

    public int ErrorCode { get; set; }

    /// <inheritdoc />
    [Obsolete( "This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}" )]
    public override void GetObjectData( SerializationInfo info, StreamingContext context )
    {
        //TODO: This needs replacing!!
        
        ArgumentNullException.ThrowIfNull( info );

        info.AddValue( "ErrorCode", ErrorCode );
        base.GetObjectData( info, context );
    }

    /// <summary>
    /// Returns the error code as a string.
    /// </summary>
    /// <param name="errorcode"> The errorcode. </param>
    /// <returns></returns>
    public static string GetErrorString( int errorcode )
    {
        return $"Bitstream errorcode {Convert.ToString( errorcode, 16 )}";
    }
}
