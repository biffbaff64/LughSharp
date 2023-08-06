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

namespace LibGDXSharp.Utils.Compression;

/// <summary>
/// An interface representing a data checksum.
/// </summary>
public interface IChecksum
{
    /// <summary>
    /// Updates the current checksum with the specified byte.
    /// </summary>
    /// <param name="b"> the byte to update the checksum with </param>
    void Update( int b );

    /// <summary>
    /// Updates the current checksum with the specified array of bytes.
    /// </summary>
    /// <param name="b"> the byte array to update the checksum with </param>
    /// <param name="off"> the start offset of the data </param>
    /// <param name="len"> the number of bytes to use for the update </param>
    void Update( byte[] b, int off, int len );

    /// <summary>
    /// Returns the current checksum value.
    /// </summary>
    /// <returns> the current checksum value </returns>
    long Value { get; }

    /// <summary>
    /// Resets the checksum to its initial value.
    /// </summary>
    void Reset();
}