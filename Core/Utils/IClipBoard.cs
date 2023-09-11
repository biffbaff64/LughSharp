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

/// <summary>
/// A very simple clipboard interface for text content.
/// </summary>
[PublicAPI]
public interface IClipboard
{
    /// <summary>
    /// Check if the clipboard has contents.
    /// </summary>
    /// <returns> true, if the clipboard has contents</returns>
    bool HasContents();

    /// <summary>
    /// The current content of the clipboard if it contains text
    /// </summary>
    /// <returns> the clipboard content or null  </returns>
    string Contents { get; set; }
}
