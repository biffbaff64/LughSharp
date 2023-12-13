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

using LibGDXSharp.Backends.Desktop.Audio.MP3Sharp.Support;

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;

/// <summary>
/// MP3SharpException is the base class for all API-level
/// exceptions thrown by MP3Sharp. To facilitate conversion and
/// common handling of exceptions from other domains, the class
/// can delegate some functionality to a contained Throwable instance.
/// </summary>
[PublicAPI, Serializable]
public class Mp3SharpException : System.Exception
{
    public Mp3SharpException()
    {
    }

    public Mp3SharpException( string message )
        : base( message )
    {
    }

    public Mp3SharpException( string message, System.Exception? inner )
        : base( message, inner )
    {
    }

    protected Mp3SharpException( SerializationInfo info, StreamingContext context )
        : base( info, context )
    {
    }

    public void PrintStackTrace()
    {
        SupportClass.WriteStackTrace( this, Console.Error );
    }

    public void PrintStackTrace( StreamWriter ps )
    {
        if ( InnerException == null )
        {
            SupportClass.WriteStackTrace( this, ps );
        }
        else
        {
            SupportClass.WriteStackTrace( InnerException, Console.Error );
        }
    }
}
