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

public class File : IComparable< File >
{
    /// <summary>
    /// Compares the current instance with another object of the same type and
    /// returns an integer that indicates whether the current instance precedes,
    /// follows, or occurs in the same position in the sort order as the other
    /// object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared.
    /// The return value has these meanings:
    /// <list type="table">
    /// <listheader><term> Value</term><description> Meaning</description></listheader>
    ///     <item><term> Less than zero</term>
    ///     <description> This instance precedes <paramref name="other" /> in the sort order.</description>
    ///     </item>
    ///     <item><term> Zero</term>
    ///     <description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description>
    ///     </item>
    ///     <item><term> Greater than zero</term>
    ///     <description> This instance follows <paramref name="other" /> in the sort order.</description>
    ///     </item>
    /// </list>
    /// </returns>
    public int CompareTo( File? other ) => 0;
}
