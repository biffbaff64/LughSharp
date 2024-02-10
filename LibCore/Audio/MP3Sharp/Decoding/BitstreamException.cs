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


using System.Runtime.Serialization;

using Exception = System.Exception;

namespace LibGDXSharp.LibCore.Audio.MP3Sharp.Decoding;

/// <summary>
///     Instances of BitstreamException are thrown
///     when operations on a Bitstream fail.
///     <p>
///         The exception provides details of the exception condition
///         in two ways:
///         <ol>
///             <li>
///                 as an error-code describing the nature of the error
///             </li>
///             <br></br>
///             <li>
///                 as the Throwable instance, if any, that was thrown
///                 indicating that an exceptional condition has occurred.
///             </li>
///         </ol>
///     </p>
/// </summary>
[Serializable]
public class BitstreamException : Mp3SharpException
{
    public BitstreamException( string message, Exception? inner = null )
        : base( message, inner ) => ErrorCode = BitstreamErrors.UNKNOWN_ERROR;

    public BitstreamException( int errorcode, Exception? inner = null )
        : this( GetErrorString( errorcode ), inner )
    {
        ErrorCode = BitstreamErrors.UNKNOWN_ERROR;
        ErrorCode = errorcode;
    }

    protected BitstreamException( SerializationInfo info, StreamingContext context )
        : base( info, context ) => ErrorCode = info.GetInt32( "ErrorCode" );

    public int ErrorCode { get; set; }

    public override void GetObjectData( SerializationInfo info, StreamingContext context )
    {
        ArgumentNullException.ThrowIfNull( info );

        info.AddValue( "ErrorCode", ErrorCode );
        base.GetObjectData( info, context );
    }

    public static string GetErrorString( int errorcode ) => $"Bitstream errorcode {Convert.ToString( errorcode, 16 )}";
}
