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

using System.Runtime.Serialization;

namespace LibGDXSharp.Audio.MP3Sharp.Decoding;

/// <summary>
///     The DecoderException represents the class of errors that can occur when decoding MPEG audio.
/// </summary>
[Serializable]
public class DecoderException : Mp3SharpException
{
    public DecoderException( string message, Exception? inner )
        : base( message, inner ) => ErrorCode = DecoderErrors.UNKNOWN_ERROR;

    public DecoderException( int errorcode, Exception? inner )
        : this( GetErrorString( errorcode ), inner ) => ErrorCode = errorcode;

    protected DecoderException( SerializationInfo info, StreamingContext context ) : base( info, context ) => ErrorCode = info.GetInt32( "ErrorCode" );

    public int ErrorCode { get; set; }

    public override void GetObjectData( SerializationInfo info, StreamingContext context )
    {
        ArgumentNullException.ThrowIfNull( info );

        info.AddValue( "ErrorCode", ErrorCode );
        base.GetObjectData( info, context );
    }

    public static string GetErrorString( int errorcode ) =>

        // TODO: use resource file to map error codes to locale-sensitive strings. 
        $"Decoder errorcode {Convert.ToString( errorcode, 16 )}";
}
