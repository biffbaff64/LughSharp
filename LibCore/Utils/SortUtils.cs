// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LughSharp.LibCore.Utils;

/// <summary>
/// Provides methods to sort arrays of objects.
/// Sorting requires working memory and this class allows that memory to be reused
/// to avoid allocation. The sorting is otherwise identical to the Arrays.sort
/// methods (uses timsort).
/// </summary>
[PublicAPI]
public class SortUtils
{
    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <typeparam name="T"></typeparam>
    public static void Sort< T >( List< T > a ) where T : IComparable< T >
    {
        var comparableTimSort = new ComparableTimSort< T >();
        comparableTimSort.DoSort( a.ToArray(), 0, a.Count );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    public static void Sort( object[] a )
    {
        var comparableTimSort = new ComparableTimSort< object >();
        comparableTimSort.DoSort( a, 0, a.Length );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    public static void Sort( object[] a, int fromIndex, int toIndex )
    {
        var comparableTimSort = new ComparableTimSort< object >();
        comparableTimSort.DoSort( a, fromIndex, toIndex );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <typeparam name="T"></typeparam>
    public static void Sort< T >( List< T > a, IComparer< T > c )
    {
        var timSort = new TimSort< T >();

        timSort.DoSort( a.ToArray(), c, 0, a.Count );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <typeparam name="T"></typeparam>
    public static void Sort< T >( T[] a, IComparer< T > c )
    {
        var timSort = new TimSort< T >();

        timSort.DoSort( a.ToArray(), c, 0, a.Length );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    /// <typeparam name="T"></typeparam>
    public static void Sort< T >( T[] a, IComparer< T > c, int fromIndex, int toIndex )
    {
        var timSort = new TimSort< T >();

        timSort.DoSort( a.ToArray(), c, fromIndex, toIndex );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <typeparam name="T"></typeparam>
    public static void Sort< T >( T[] a, int from, int to )
    {
        var comparableTimSort = new ComparableTimSort< T >();

        comparableTimSort.DoSort( a, from, to );
    }
}
