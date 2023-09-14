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

namespace LibGDXSharp.Utils.Collections;

[PublicAPI]
public class CollectionsData
{
    /// <summary>
    /// When true, <see cref="IEnumerator{T}"/> for collections will allocate a new
    /// iterator for each invocation. When false, the iterator is reused and nested
    /// use will throw an exception.
    /// <p>
    /// Default is false.
    /// </p> 
    /// </summary>
    public static bool AllocateIterators { get; set; }
}
