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

using System.Diagnostics;

namespace LibGDXSharp.Utils;

/// <summary>
/// Provides methods to sort arrays of objects.
/// Sorting requires working memory and this class allows that memory to be reused
/// to avoid allocation. The sorting is otherwise identical to the Arrays.sort
/// methods (uses timsort).
/// </summary>
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class SortUtils
{
    public static SortUtils Instance { [DebuggerStepThrough] get; } = new();

    private TimSort< object >? _timSort;
    private ComparableTimSort< object >? _comparableTimSort;

    public void Sort<T>( List< T > a ) where T : IComparable< T >
    {
        _comparableTimSort ??= new ComparableTimSort< object >();
        _comparableTimSort.DoSort( a.ToArray() as object[], 0, a.Count );
    }

    public void Sort( object[] a )
    {
        _comparableTimSort ??= new ComparableTimSort< object >();
        _comparableTimSort.DoSort(a, 0, a.Length);
    }

    public void Sort( object[] a, int fromIndex, int toIndex )
    {
        _comparableTimSort ??= new ComparableTimSort< object >();
        _comparableTimSort.DoSort( a, fromIndex, toIndex );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <typeparam name="T"></typeparam>
    public void Sort<T>( List< T > a, IComparer< object > c )
    {
        _timSort ??= new TimSort< object >();

        _timSort.DoSort( a.Cast< object >().ToArray(), c, 0, a.Count );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <typeparam name="T"></typeparam>
    public void Sort<T>( T[] a, IComparer< object > c )
    {
        _timSort ??= new TimSort< object >();

        _timSort.DoSort( a.Cast< object >().ToArray(), c, 0, a.Length );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    /// <typeparam name="T"></typeparam>
    public void Sort<T>( T[] a, IComparer< object > c, int fromIndex, int toIndex )
    {
        _timSort ??= new TimSort< object >();

        _timSort.DoSort( a.Cast< object >().ToArray(), c, fromIndex, toIndex );
    }
}