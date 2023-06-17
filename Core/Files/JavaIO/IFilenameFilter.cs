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

namespace LibGDXSharp.Files;

/// <summary>
/// Instances of classes that implement this interface are used to filter filenames.
/// </summary>
public interface IFilenameFilter
{
    /// <summary>
    /// Tests if a specified file should be included in a file list.
    /// </summary>
    /// <param name="dir"> the directory in which the file was found. </param>
    /// <param name="name"> the name of the file. </param>
    /// <returns> <tt>true</tt>
    /// if and only if the name should beb included in the file list;
    /// <tt>false</tt> otherwise.
    /// </returns>
    bool Accept( FileStream dir, string name );
}