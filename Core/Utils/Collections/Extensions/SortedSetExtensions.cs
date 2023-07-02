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

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Core.Utils.Collections.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public static class SortedSetExtensions
{
    public static void Clear<T>( this SortedSet< T > ss, int maximumCapacity )
    {
        throw new NotImplementedException();
    }

    public static void AddAll<T>( this SortedSet< T > ss, SortedSet< T > array )
    {
        throw new NotImplementedException();
    }
}

