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

using System.Collections;

namespace LibGDXSharp.Utils.Collections;

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
[PublicAPI]
public class PredicateIterable<T> : IEnumerable< T >
{
    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    [PublicAPI]
    public PredicateIterable( IEnumerable< T > enumerable, IPredicate< T > predicate )
    {
        Enumerable = enumerable;
        Predicate  = predicate;
    }

    public IEnumerable< T >        Enumerable { get; set; }
    public IPredicate< T >         Predicate  { get; set; }
    public PredicateIterator< T >? Enumerator { get; set; } = null;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerator< T > GetEnumerator() => Enumerable.GetEnumerator();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    [PublicAPI]
    public void Set( IEnumerable< T > enumerable, IPredicate< T > predicate )
    {
        Enumerable = enumerable;
        Predicate  = predicate;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerator< T > Iterator()
    {
        if ( Enumerator == null )
        {
            Enumerator = new PredicateIterator< T >( Enumerable.GetEnumerator(), Predicate );
        }
        else
        {
            Enumerator.Set( Enumerable.GetEnumerator(), Predicate );
        }

        return Enumerator;
    }
}
