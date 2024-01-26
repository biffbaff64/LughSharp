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

using LibGDXSharp.Utils.Buffers;

namespace LibGDXSharp.Utils;

/// <summary>
///     A <tt>Readable</tt> is a source of characters. Characters from a <tt>Readable</tt>
///     are made available to callers of the read method via a <see cref="CharBuffer" />.
/// </summary>
[PublicAPI]
public interface IReadable
{
    /// <summary>
    ///     Attempts to read characters into the specified character buffer.
    ///     The buffer is used as a repository of characters as-is: the only
    ///     changes made are the results of a put operation. No flipping or
    ///     rewinding of the buffer is performed.
    /// </summary>
    /// <param name="cb"> the buffer to read characters into </param>
    /// <returns>
    ///     The number of {@code char} values added to the buffer,
    ///     or -1 if this source of characters is at its end
    /// </returns>
    int Read( CharBuffer cb );
}
