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

namespace LibGDXSharp.Core.Utils.Collections;

/// <summary>
/// A <see cref="ObjectSet"/> that also stores keys in an <see cref="List"/> using the
/// insertion order. Null keys are not allowed. No allocation is done except when growing
/// the table size.
/// <para>
/// Iteration is ordered and faster than an unordered set. Keys can also be accessed and
/// the order changed using <see cref="OrderedItems()"/>. There is some additional overhead
/// for put and remove. When used for faster iteration versus ObjectSet and the order does
/// not actually matter, copying during remove can be greatly reduced by setting <see cref="Array.ordered"/>
/// to false for <see cref="OrderedSet.orderedItems()"/>.
/// </para>
/// <para>
/// This class performs fast contains (typically O(1), worst case O(n) but that is rare in practice). Remove is somewhat slower due
/// to <see cref="orderedItems()"/>. Add may be slightly slower, depending on hash collisions. Hashcodes are rehashed to reduce
/// collisions and the need to resize. Load factors greater than 0.91 greatly increase the chances to resize to the next higher POT
/// size.
/// </para>
/// <para>
/// Unordered sets and maps are not designed to provide especially fast iteration.
/// Iteration is faster with OrderedSet and OrderedMap.
/// </para>
/// <para>
/// This implementation uses linear probing with the backward shift algorithm for removal.
/// Hashcodes are rehashed using Fibonacci hashing, instead of the more common power-of-two
/// mask, to better distribute poor hashCodes
/// (see <a href="https://probablydance.com/2018/06/16/fibonacci-hashing-the-optimization-that-the-world-forgot-or-a-better-alternative-to-integer-modulo/">
/// Malte Skarupke's blog post</a>). Linear probing continues to work even when all hashCodes collide, just more slowly.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class OrderedSet<T>
{
}

