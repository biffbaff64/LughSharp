using System.Diagnostics;

using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Utils
{
    /// <summary>
    /// Provides methods to sort arrays of objects.
    /// Sorting requires working memory and this class allows that memory to be reused
    /// to avoid allocation. The sorting is otherwise identical to the Arrays.sort
    /// methods (uses timsort).
    /// </summary>
    public class SortUtils
    {
        private readonly static SortUtils instance = new SortUtils();

        // ReSharper disable once ConvertToAutoProperty
        public static SortUtils Instance
        {
            [DebuggerStepThrough] get => instance;
        }

        private TimSort?           _timSort;
        private ComparableTimSort? _comparableTimSort;

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <typeparam name="T"></typeparam>
        public void Sort<T>( Array< T > a ) where T : IComparable
        {
            if ( _comparableTimSort == null )
            {
                _comparableTimSort = new ComparableTimSort();
            }

            _comparableTimSort.DoSort( a.Items, 0, a.Size );
        }

        /// <summary>
        /// The specified objects must implement <see cref="System.IComparable"/>.
        /// </summary>
        public void Sort( object[] a )
        {
            if ( _comparableTimSort == null )
            {
                _comparableTimSort = new ComparableTimSort();
            }

            _comparableTimSort.DoSort( a, 0, a.Length );
        }

        /// <summary>
        /// The specified objects must implement <see cref="System.IComparable"/>.
        /// </summary>
        public void Sort( object[]? a, int fromIndex, int toIndex )
        {
            if ( _comparableTimSort == null )
            {
                _comparableTimSort = new ComparableTimSort();
            }

            _comparableTimSort.DoSort( a, fromIndex, toIndex );
        }

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="c"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        public void Sort<T, T1>( Array< T > a, IComparer< T1 > c )
        {
            if ( _timSort == null )
            {
                _timSort = new TimSort();
            }

            _timSort.DoSort( a.Items, c, 0, a.Size );
        }

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="c"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        public void Sort<T, T1>( T[] a, IComparer< T1 > c )
        {
            if ( _timSort == null )
            {
                _timSort = new TimSort();
            }

            _timSort.DoSort( a, c, 0, a.Length );
        }

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="c"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        public void Sort<T, T1>( T[] a, IComparer< T1 > c, int fromIndex, int toIndex )
        {
            if ( _timSort == null )
            {
                _timSort = new TimSort();
            }

            _timSort.DoSort( a, c, fromIndex, toIndex );
        }
    }
}
