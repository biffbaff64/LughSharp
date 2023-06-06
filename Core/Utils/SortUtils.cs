using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils;

/// <summary>
/// Provides methods to sort arrays of objects.
/// Sorting requires working memory and this class allows that memory to be reused
/// to avoid allocation. The sorting is otherwise identical to the Arrays.sort
/// methods (uses timsort).
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class SortUtils
{
    private readonly static SortUtils instance = new SortUtils();

    // ReSharper disable once ConvertToAutoProperty
    public static SortUtils Instance
    {
        [DebuggerStepThrough]
        get => instance;
    }

    private TimSort<object>?   _timSort;

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <typeparam name="T"></typeparam>
    public void Sort<T>( List< T > a, IComparer< object > c )
    {
        _timSort ??= new TimSort< object >();

        _timSort.DoSort( a.Cast<object>().ToArray(), c, 0, a.Count );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public void Sort<T, T1>( T[] a, IComparer< object > c )
    {
        _timSort ??= new TimSort< object >();

        _timSort.DoSort( a.Cast<object>().ToArray(), c, 0, a.Length );
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    /// <typeparam name="T"></typeparam>
    public void Sort<T>( IEnumerable< T > a, IComparer< object > c, int fromIndex, int toIndex )
    {
        _timSort ??= new TimSort< object >();

        _timSort.DoSort( a.Cast<object>().ToArray(), c, fromIndex, toIndex );
    }
}