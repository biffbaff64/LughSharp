// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


namespace Corelib.Lugh.Utils;

/// <summary>
/// Provides methods to sort arrays of objects.
/// <para>
/// Sorting requires working memory and this class allows that memory to be reused
/// to avoid allocation. The sorting is otherwise identical to the Arrays.sort
/// methods (uses timsort).
/// </para>
/// </summary>
[PublicAPI]
public class SortUtils
{
    /// <summary>
    /// Sorts the elements of the specified list using the default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <param name="a">The list to sort.</param>
    public static void Sort< T >( List< T > a ) where T : IComparable< T >
    {
        var comparableTimSort = new ComparableTimSort< T >();
        comparableTimSort.DoSort( a.ToArray(), 0, a.Count );
    }

    /// <summary>
    /// Sorts the elements of the specified array using the default comparer.
    /// </summary>
    /// <param name="a">The array to sort.</param>
    public static void Sort( object[] a )
    {
        var comparableTimSort = new ComparableTimSort< object >();
        comparableTimSort.DoSort( a, 0, a.Length );
    }

    /// <summary>
    /// Sorts the elements of the specified range in the array using the default comparer.
    /// </summary>
    /// <param name="a">The array to sort.</param>
    /// <param name="fromIndex">The index of the first element (inclusive) to be sorted.</param>
    /// <param name="toIndex">The index of the last element (exclusive) to be sorted.</param>
    public static void Sort( object[] a, int fromIndex, int toIndex )
    {
        var comparableTimSort = new ComparableTimSort< object >();
        comparableTimSort.DoSort( a, fromIndex, toIndex );
    }

    /// <summary>
    /// Sorts the elements of the specified list using the specified comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <param name="a">The list to sort.</param>
    /// <param name="c">The comparer to use.</param>
    public static void Sort< T >( List< T > a, IComparer< T > c )
    {
        var timSort = new TimSort< T >();
        timSort.DoSort( a.ToArray(), c, 0, a.Count );
    }

    /// <summary>
    /// Sorts the elements of the specified array using the specified comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="a">The array to sort.</param>
    /// <param name="c">The comparer to use.</param>
    public static void Sort< T >( T[] a, IComparer< T > c )
    {
        var timSort = new TimSort< T >();
        timSort.DoSort( a, c, 0, a.Length );
    }

    /// <summary>
    /// Sorts the elements of the specified range in the array using the specified comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="a">The array to sort.</param>
    /// <param name="c">The comparer to use.</param>
    /// <param name="fromIndex">The index of the first element (inclusive) to be sorted.</param>
    /// <param name="toIndex">The index of the last element (exclusive) to be sorted.</param>
    public static void Sort< T >( T[] a, IComparer< T > c, int fromIndex, int toIndex )
    {
        var timSort = new TimSort< T >();
        timSort.DoSort( a, c, fromIndex, toIndex );
    }

    /// <summary>
    /// Sorts the elements of the specified range in the array using the default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="a">The array to sort.</param>
    /// <param name="from">The index of the first element (inclusive) to be sorted.</param>
    /// <param name="to">The index of the last element (exclusive) to be sorted.</param>
    public static void Sort< T >( T[] a, int from, int to )
    {
        var comparableTimSort = new ComparableTimSort< T >();
        comparableTimSort.DoSort( a, from, to );
    }
}
