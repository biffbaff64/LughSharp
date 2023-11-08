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

namespace LibGDXSharp.Core.Audio.JavazoomJL;

/// <summary>
/// Instances of <tt>BitstreamException</tt> are thrown when operations
/// on a <tt>Bitstream</tt> fail.
/// <para>
/// The exception provides details of the exception condition in two ways:
/// <li>as an error-code describing the nature of the error</li>
/// <li>
/// as the <tt>Exception</tt> instance, if any, that was thrown indicating
/// that an exceptional condition has occurred.
/// </li>
/// </para>
/// </summary>
[PublicAPI]
public class BitstreamException : Exception
{
    public int Errorcode { get; private set; } = Bitstream.UNKNOWN_ERROR;

    public BitstreamException( string msg, Exception? t = null )
        : base( msg, t )
    {
    }

    public BitstreamException( int errorcode, Exception? t = null )
        : this( GetErrorString( errorcode ), t )
    {
        this.Errorcode = errorcode;
    }

    // REVIEW: use resource bundle to map error codes to locale-sensitive strings.
    private static string GetErrorString( int errorcode ) => $@"Bitstream errorcode {errorcode}";
}
