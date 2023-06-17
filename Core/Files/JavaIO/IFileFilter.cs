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

public interface IFileFilter
{
    /// <summary>
    /// Tests whether or not the specified abstract pathname should be
    /// included in a pathname list.
    /// </summary>
    /// <param name="pathname"> The abstract pathname to be tested. </param>
    /// <returns>
    /// <tt>true</tt> if and only if <tt>pathname</tt> should be included
    /// </returns>
    bool Accept( FileStream? pathname );
}