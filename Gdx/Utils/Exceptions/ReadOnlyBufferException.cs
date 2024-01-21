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

namespace LibGDXSharp.Utils;

[PublicAPI]
public class ReadOnlyBufferException : Exception
{
    /// <summary>
    /// Throws a new ReadOnlyBufferException.
    /// </summary>
    /// <param name="message">
    /// A string holding an appropriate message.
    /// The obvious default message for this exception is "This Buffer is Read-Only!".
    /// </param>
    public ReadOnlyBufferException( string? message = "This Buffer is Read-Only!" )
        : base( message )
    {
    }

    public ReadOnlyBufferException( string? message, Exception? innerException )
        : base( message, innerException )
    {
    }
}
