// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Runtime.Serialization;

namespace LibGDXSharp.Audio.MP3Sharp;

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
