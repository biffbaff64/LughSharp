// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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
/// This is a near duplicate of <see cref="TimSort{T}"/>, modified for use with arrays
/// of objects that implement <see cref="IComparable"/>, instead of using explicit
/// comparators.
/// <para>
/// If you are using an optimizing VM, you may find that ComparableTimSort offers no
/// performance benefit over TimSort in conjunction with a comparator that simply
/// returns <see cref="IComparable.CompareTo"/>. If this is the case, you are better
/// off deleting ComparableTimSort to eliminate the code duplication.
/// </para>
/// </summary>
[PublicAPI]
public class ComparableTimSort< T >
{
    /// <summary>
    /// This is the minimum sized sequence that will be merged. Shorter sequences will
    /// be lengthened by calling binarySort. If the entire array is less than this length,
    /// no merges will be performed.
    /// <para>
    /// This constant should be a power of two. It was 64 in Tim Peter's C implementation,
    /// but 32 was empirically determined to work better in this implementation. In the
    /// unlikely event that you set this constant to be a number that's not a power of two,
    /// you'll need to change the <see cref="MinRunLength"/> computation.
    /// </para>
    /// <para>
    /// If you decrease this constant, you must change the stackLen computation in the
    /// TimSort constructor, or you risk an ArrayOutOfBounds exception. See listsort.txt
    /// for a discussion of the minimum stack length required as a function of the length
    /// of the array being sorted and the minimum merge sequence length.
    /// </para>
    /// </summary>
    private const int MIN_MERGE = 32;

    // When we get into galloping mode, we stay there until both runs
    // win less often than MIN_GALLOP consecutive times.
    private const int MIN_GALLOP = 7;

    // Maximum initial size of tmp array, which is used for merging. The array can grow
    // to accommodate demand. Unlike Tim's original C version, we do not allocate this
    // much storage when sorting smaller arrays. This change was required for performance.
    private const int INITIAL_TMP_STORAGE_LENGTH = 256;

    // Asserts have been placed in if-statements for performace.
    // To enable them, set this field to true.
    // If you modify this class, please do test the asserts!
    [UsedImplicitly]
    private const bool ALLOW_ASSERTS = false; // true;

    private readonly int[] _runBase;
    private readonly int[] _runLen;

    // The array being sorted.
    private T[]? _a;

    // This controls when we get *into* galloping mode. It is initialized to Min_Gallop.
    // The mergeLo and mergeHi methods nudge it higher for random data, and lower for
    // highly structured data.
    private int _minGallop = MIN_GALLOP;

    // A stack of pending runs yet to be merged. Run i starts at address base[i] and
    // extends for len[i] elements. It's always true (so long as the indices are in
    // bounds) that:
    // 
    // runBase[i] + runLen[i] == runBase[i + 1]
    // 
    // so we could cut the storage for this, but it's a minor amount, and keeping all
    // the info explicit simplifies the code.
    private int _stackSize = 0; // Number of pending runs on stack

    // Temp storage for merges.
    private T?[] _tmp;
    private int  _tmpCount;

    public ComparableTimSort()
    {
        _tmp     = new T[ INITIAL_TMP_STORAGE_LENGTH ];
        _runBase = new int[ 40 ];
        _runLen  = new int[ 40 ];
    }

    /// <summary>
    /// Creates a TimSort instance to maintain the state of an ongoing sort.
    /// </summary>
    /// <param name="a"> the array to be sorted  </param>
    private ComparableTimSort( T[] a )
    {
        _a = a;

        // Allocate temp storage (which may be increased later if necessary)
        var len = a.Length;

        var newArray = new T[ len < ( 2 * INITIAL_TMP_STORAGE_LENGTH )
                                  ? len >>> 1
                                  : INITIAL_TMP_STORAGE_LENGTH ];

        _tmp = newArray;

        // Allocate runs-to-be-merged stack (which cannot be expanded).
        // The "magic numbers" in the computation below must be changed if Min_Merge is
        // decreased. See the Min_Merge declaration above for more information.
        var stackLen = len < 120 ? 5 : len < 1542 ? 10 : len < 119151 ? 19 : 40;

        _runBase = new int[ stackLen ];
        _runLen  = new int[ stackLen ];
    }

    public void DoSort( T[]? a, int lo, int hi )
    {
        ArgumentNullException.ThrowIfNull( a );

        _stackSize = 0;
        RangeCheck( a.Length, lo, hi );

        var nRemaining = hi - lo;

        if ( nRemaining < 2 )
        {
            return; // Arrays of size 0 and 1 are always sorted
        }

        // If array is small, do a "mini-TimSort" with no merges
        if ( nRemaining < MIN_MERGE )
        {
            var initRunLen = CountRunAndMakeAscending( a, lo, hi );

            BinarySort( a, lo, hi, lo + initRunLen );

            return;
        }

        _a        = a;
        _tmpCount = 0;

        // March over the array once, left to right, finding natural runs, extending
        // short natural runs to minRun elements, and merging runs to maintain stack
        // invariant.
        var minRun = MinRunLength( nRemaining );

        do
        {
            // Identify next run
            var runLen = CountRunAndMakeAscending( a, lo, hi );

            // If run is short, extend to min(minRun, nRemaining)
            if ( runLen < minRun )
            {
                var force = nRemaining <= minRun ? nRemaining : minRun;
                BinarySort( a, lo, lo + force, lo + runLen );
                runLen = force;
            }

            // Push run onto pending-run stack, and maybe merge
            PushRun( lo, runLen );
            MergeCollapse();

            // Advance to find next run
            lo         += runLen;
            nRemaining -= runLen;
        }
        while ( nRemaining != 0 );

#if ALLOW_ASSERTS
        DebugAssert( lo == hi );
#endif

        // Merge all remaining runs to complete sort
        MergeForceCollapse();

#if ALLOW_ASSERTS
        DebugAssert( _stackSize == 1 );
#endif

        _a = null;

        for ( int i = 0, n = _tmpCount; i < n; i++ )
        {
            _tmp[ i ] = default( T? );
        }
    }

    public static void Sort( T[] a )
    {
        Sort( a, 0, a.Length );
    }

    public static void Sort( T[] a, int lo, int hi )
    {
        RangeCheck( a.Length, lo, hi );
        var nRemaining = hi - lo;

        if ( nRemaining < 2 )
        {
            return; // Arrays of size 0 and 1 are always sorted
        }

        // If array is small, do a "mini-TimSort" with no merges
        if ( nRemaining < MIN_MERGE )
        {
            var initRunLen = CountRunAndMakeAscending( a, lo, hi );
            BinarySort( a, lo, hi, lo + initRunLen );

            return;
        }

        // March over the array once, left to right, finding natural runs, extending short
        // natural runs to minRun elements, and merging runs to maintain stack invariant. 
        var ts     = new ComparableTimSort< T >( a );
        var minRun = MinRunLength( nRemaining );

        do
        {
            // Identify next run
            var runLen = CountRunAndMakeAscending( a, lo, hi );

            // If run is short, extend to min(minRun, nRemaining)
            if ( runLen < minRun )
            {
                var force = nRemaining <= minRun ? nRemaining : minRun;

                BinarySort( a, lo, lo + force, lo + runLen );
                runLen = force;
            }

            // Push run onto pending-run stack, and maybe merge
            ts.PushRun( lo, runLen );
            ts.MergeCollapse();

            // Advance to find next run
            lo         += runLen;
            nRemaining -= runLen;
        }
        while ( nRemaining != 0 );

#if ALLOW_ASSERTS
        DebugAssert( lo == hi );
#endif

        // Merge all remaining runs to complete sort
        ts.MergeForceCollapse();

#if ALLOW_ASSERTS
        DebugAssert( ts._stackSize == 1 );
#endif
    }

    /// <summary>
    /// Sorts the specified portion of the specified array using a binary insertion sort.
    /// This is the best method for sorting small numbers of elements. It requires O(n log n)
    /// compares, but O(n^2) data movement (worst case).
    /// If the initial part of the specified range is already sorted, this method can take
    /// advantage of it: the method assumes that the elements from index <tt>lo</tt>, inclusive,
    /// to <tt>start</tt>, exclusive are already sorted.
    /// </summary>
    /// <param name="a"> the array in which a range is to be sorted </param>
    /// <param name="lo"> the index of the first element in the range to be sorted </param>
    /// <param name="hi"> the index after the last element in the range to be sorted </param>
    /// <param name="start">
    /// the index of the first element in the range that is not already known to be sorted
    /// <tt>lo &lt;= start &lt;= hi</tt>
    /// </param>
    private static void BinarySort( T[] a, int lo, int hi, int start )
    {
#if ALLOW_ASSERTS
        DebugAssert( ( lo <= start ) && ( start <= hi ) );
#endif

        if ( start == lo )
        {
            start++;
        }

        for ( ; start < hi; start++ )
        {
            var pivot = ( IComparable< T > ) a[ start ]!;

            // Set left (and right) to the index where a[start] (pivot) belongs
            var left  = lo;
            var right = start;

#if ALLOW_ASSERTS
            DebugAssert( left <= right );
#endif

            // Invariants: pivot >= all in [lo, left). pivot < all in [right, start).
            while ( left < right )
            {
                var mid = ( left + right ) >>> 1;

                if ( pivot?.CompareTo( a[ mid ] ) < 0 )
                {
                    right = mid;
                }
                else
                {
                    left = mid + 1;
                }
            }

#if ALLOW_ASSERTS
            DebugAssert( left == right );
#endif

            // The invariants still hold: pivot >= all in [lo, left) and pivot < all in
            // [left, start), so pivot belongs at left. Note that if there are elements
            // equal to pivot, left points to the first slot after them -- that's why
            // this sort is stable.

            // Slide elements over to make room to make room for pivot.
            var n = start - left; // The number of elements to move

            if ( n is 1 or 2 )
            {
                if ( n == 2 )
                {
                    a[ left + 2 ] = a[ left + 1 ];
                }

                a[ left + 1 ] = a[ left ];
            }
            else
            {
                Array.Copy( a, left, a, left + 1, n );
            }

            a[ left ] = ( T ) pivot!;
        }
    }

    /// <summary>
    /// Returns the length of the run beginning at the specified position in the specified
    /// array and reverses the run if it is descending (ensuring that the run will always
    /// be ascending when the method returns).
    /// <para>
    /// A run is the longest ascending sequence with:
    /// </para>
    /// <para>
    ///     <tt>
    ///     a[lo] &lt;= a[lo + 1] &lt;= a[lo + 2] &lt;= ...
    ///     </tt>
    /// </para>
    /// <para>
    /// or the longest descending sequence with:
    /// </para>
    /// <para>
    ///     <tt>
    ///     a[lo] > a[lo + 1] > a[lo + 2] > ...
    ///     </tt>
    /// </para>
    /// <para></para>
    /// For its intended use in a stable mergesort, the strictness of the definition of
    /// descending is needed so that the call can safely reverse a descending sequence
    /// without violating stability.
    /// </summary>
    /// <param name="a"> the array in which a run is to be counted and possibly reversed </param>
    /// <param name="lo"> index of the first element in the run </param>
    /// <param name="hi">
    /// index after the last element that may be contained in the run. It is required
    /// that <tt>lo &lt; hi</tt>.
    /// </param>
    /// <returns> the length of the run beginning at the specified position in the specified array  </returns>
    private static int CountRunAndMakeAscending( T[] a, int lo, int hi )
    {
        ArgumentNullException.ThrowIfNull( a );

#if ALLOW_ASSERTS
        DebugAssert( lo < hi );
#endif

        var runHi = lo + 1;

        if ( runHi == hi )
        {
            return 1;
        }

        // Find end of run, and reverse range if descending
        if ( ( ( IComparable< T > ) a[ runHi++ ]! ).CompareTo( a[ lo ] ) < 0 )
        {
            // Descending
            while ( ( runHi < hi )
                 && ( ( ( IComparable< T > ) a[ runHi ]! ).CompareTo( a[ runHi - 1 ] ) < 0 ) )
            {
                runHi++;
            }

            ReverseRange( a, lo, runHi );
        }
        else
        {
            // Ascending
            while ( ( runHi < hi )
                 && ( ( ( IComparable< T > ) a[ runHi ]! ).CompareTo( a[ runHi - 1 ] ) >= 0 ) )
            {
                runHi++;
            }
        }

        return runHi - lo;
    }

    /// <summary>
    /// Reverse the specified range of the specified array.
    /// </summary>
    /// <param name="a"> the array in which a range is to be reversed </param>
    /// <param name="lo"> the index of the first element in the range to be reversed </param>
    /// <param name="hi"> the index after the last element in the range to be reversed  </param>
    private static void ReverseRange( T[] a, int lo, int hi )
    {
        hi--;

        while ( lo < hi )
        {
            var t = a[ lo ];
            a[ lo++ ] = a[ hi ];
            a[ hi-- ] = t;
        }
    }

    /// <summary>
    /// Returns the minimum acceptable run length for an array of the specified length.
    /// Natural runs shorter than this will be extended with <see cref="BinarySort"/>.
    /// <para>
    /// Roughly speaking, the computation is:
    /// </para>
    /// <para>
    /// If n &lt; Min_Merge, return n (it's too small to bother with fancy stuff).
    /// Else if n is an exact power of 2, return Min_Merge/2. Else return an int k,
    /// Min_Merge/2 &lt;= k &lt;= Min_Merge, such that n/k is close to, but strictly
    /// less than, an exact power of 2.
    /// </para>
    /// </summary>
    /// <param name="n"> the length of the array to be sorted </param>
    /// <returns> the length of the minimum run to be merged  </returns>
    private static int MinRunLength( int n )
    {
#if ALLOW_ASSERTS
        DebugAssert( n >= 0 );
#endif

        var r = 0; // Becomes 1 if any 1 bits are shifted off

        while ( n >= MIN_MERGE )
        {
            r |=  n & 1;
            n >>= 1;
        }

        return n + r;
    }

    /// <summary>
    /// Pushes the specified run onto the pending-run stack.
    /// </summary>
    /// <param name="runBase"> index of the first element in the run </param>
    /// <param name="runLen"> the number of elements in the run  </param>
    private void PushRun( int runBase, int runLen )
    {
        _runBase[ _stackSize ] = runBase;
        _runLen[ _stackSize ]  = runLen;

        _stackSize++;
    }

    /// <summary>
    /// Examines the stack of runs waiting to be merged and merges adjacent runs until
    /// the stack invariants are reestablished:
    /// <para>
    ///     <tt>
    ///     1. runLen[i - 3] > runLen[i - 2] + runLen[i - 1] 2. runLen[i - 2] > runLen[i - 1]
    ///     </tt>
    /// </para>
    /// <para>
    /// This method is called each time a new run is pushed onto the stack, so the
    /// invariants are guaranteed to hold for i &lt; stackSize upon entry to the method.
    /// </para>
    /// </summary>
    private void MergeCollapse()
    {
        while ( _stackSize > 1 )
        {
            var n = _stackSize - 2;

            if ( ( n > 0 ) && ( _runLen[ n - 1 ] <= ( _runLen[ n ] + _runLen[ n + 1 ] ) ) )
            {
                if ( _runLen[ n - 1 ] < _runLen[ n + 1 ] )
                {
                    n--;
                }

                MergeAt( n );
            }
            else if ( _runLen[ n ] <= _runLen[ n + 1 ] )
            {
                MergeAt( n );
            }
            else
            {
                break; // Invariant is established
            }
        }
    }

    /// <summary>
    /// Merges all runs on the stack until only one remains.
    /// This method is called once, to complete the sort.
    /// </summary>
    private void MergeForceCollapse()
    {
        while ( _stackSize > 1 )
        {
            var n = _stackSize - 2;

            if ( ( n > 0 ) && ( _runLen[ n - 1 ] < _runLen[ n + 1 ] ) )
            {
                n--;
            }

            MergeAt( n );
        }
    }

    /// <summary>
    /// Merges the two runs at stack indices i and i+1. Run i must be the penultimate or
    /// antepenultimate run on the stack. In other words, i must be equal to stackSize-2
    /// or stackSize-3.
    /// </summary>
    /// <param name="i"> stack index of the first of the two runs to merge  </param>
    private void MergeAt( int i )
    {
#if ALLOW_ASSERTS
        DebugAssert( _stackSize >= 2 );
        DebugAssert( i >= 0 );
        DebugAssert( ( i == ( _stackSize - 2 ) ) || ( i == ( _stackSize - 3 ) ) );
#endif

        var base1 = _runBase[ i ];
        var len1  = _runLen[ i ];
        var base2 = _runBase[ i + 1 ];
        var len2  = _runLen[ i + 1 ];

#if ALLOW_ASSERTS
        DebugAssert( ( len1 > 0 ) && ( len2 > 0 ) );
        DebugAssert( ( base1 + len1 ) == base2 );
#endif

        // Record the length of the combined runs; if i is the 3rd-last run now, also
        // slide over the last run (which isn't involved in this merge). The current
        // run (i+1) goes away in any case.
        _runLen[ i ] = len1 + len2;

        if ( i == ( _stackSize - 3 ) )
        {
            _runBase[ i + 1 ] = _runBase[ i + 2 ];
            _runLen[ i + 1 ]  = _runLen[ i + 2 ];
        }

        _stackSize--;

#if ALLOW_ASSERTS
        DebugAssert( _a != null, nameof( _a ) + " != null" );
#endif

        // Find where the first element of run2 goes in run1. Prior elements
        // in run1 can be ignored (because they're already in place).
        var k = GallopRight( ( IComparable< T > ) _a![ base2 ]!, _a, base1, len1, 0 );

#if ALLOW_ASSERTS
        DebugAssert( k >= 0 );
#endif

        base1 += k;
        len1  -= k;

        if ( len1 == 0 )
        {
            return;
        }

        // Find where the last element of run1 goes in run2. Subsequent elements in run2
        // can be ignored (because they're already in place).
        len2 = GallopLeft( ( IComparable< T > ) _a[ ( base1 + len1 ) - 1 ]!, _a, base2, len2, len2 - 1 );

#if ALLOW_ASSERTS
        DebugAssert( len2 >= 0 );
#endif

        if ( len2 == 0 )
        {
            return;
        }

        // Merge remaining runs, using tmp array with min(len1, len2) elements
        if ( len1 <= len2 )
        {
            MergeLo( base1, len1, base2, len2 );
        }
        else
        {
            MergeHi( base1, len1, base2, len2 );
        }
    }

    /// <summary>
    /// Locates the position at which to insert the specified key into the specified
    /// sorted range; if the range contains an element equal to key, returns the index
    /// of the leftmost equal element.
    /// </summary>
    /// <param name="key"> the key whose insertion point to search for </param>
    /// <param name="a"> the array in which to search </param>
    /// <param name="first"> the index of the first element in the range </param>
    /// <param name="len"> the length of the range; must be > 0 </param>
    /// <param name="hint">
    /// the index at which to begin the search, 0 &lt;= hint &amp;lt; n. The closer hint
    /// is to the result, the faster this method will run.
    /// </param>
    /// <returns>
    /// the int k, 0 &lt;= k &lt;= n such that a[b + k - 1] &lt; key &lt;= a[b + k],
    /// pretending that a[b - 1] is minus infinity and a[b + n] is infinity. In other words,
    /// key belongs at index b + k; or in other words, the first k elements of a should
    /// precede key, and the last n - k should follow it.
    /// </returns>
    private static int GallopLeft( IComparable< T > key, T[] a, int first, int len, int hint )
    {
#if ALLOW_ASSERTS
        DebugAssert( ( len > 0 ) && ( hint >= 0 ) && ( hint < len ) );
#endif

        var lastOfs = 0;
        var ofs     = 1;

        if ( key.CompareTo( a[ first + hint ] ) > 0 )
        {
            // Gallop right until a[first+hint+lastOfs] < key <= a[first+hint+ofs]
            var maxOfs = len - hint;

            while ( ( ofs < maxOfs ) && ( key.CompareTo( a[ first + hint + ofs ] ) > 0 ) )
            {
                lastOfs = ofs;
                ofs     = ( ofs << 1 ) + 1;

                if ( ofs <= 0 ) // int overflow
                {
                    ofs = maxOfs;
                }
            }

            if ( ofs > maxOfs )
            {
                ofs = maxOfs;
            }

            // Make offsets relative to first
            lastOfs += hint;
            ofs     += hint;
        }
        else
        {
            // key <= a[first + hint]
            // Gallop left until a[first+hint-ofs] < key <= a[first+hint-lastOfs]
            var maxOfs = hint + 1;

            while ( ( ofs < maxOfs ) && ( key.CompareTo( a[ ( first + hint ) - ofs ] ) <= 0 ) )
            {
                lastOfs = ofs;
                ofs     = ( ofs << 1 ) + 1;

                if ( ofs <= 0 ) // int overflow
                {
                    ofs = maxOfs;
                }
            }

            if ( ofs > maxOfs )
            {
                ofs = maxOfs;
            }

            // Make offsets relative to first
            var tmp = lastOfs;

            lastOfs = hint - ofs;
            ofs     = hint - tmp;
        }

#if ALLOW_ASSERTS
        DebugAssert( ( -1 <= lastOfs ) && ( lastOfs < ofs ) && ( ofs <= len ) );
#endif

        // Now a[first+lastOfs] < key <= a[first+ofs], so key belongs somewhere to the right
        // of lastOfs but no farther right than ofs. Do a binary search, with invariant
        // a[first + lastOfs - 1] < key <= a[first + ofs].
        lastOfs++;

        while ( lastOfs < ofs )
        {
            var m = lastOfs + ( ( ofs - lastOfs ) >>> 1 );

            if ( key.CompareTo( a[ first + m ] ) > 0 )
            {
                lastOfs = m + 1; // a[first + m] < key
            }
            else
            {
                ofs = m; // key <= a[first + m]
            }
        }

#if ALLOW_ASSERTS
        DebugAssert( lastOfs == ofs ); // so a[first + ofs - 1] < key <= a[first + ofs]
#endif

        return ofs;
    }

    /// <summary>
    /// Like gallopLeft, except that if the range contains an element equal to key,
    /// gallopRight returns the index after the rightmost equal element.
    /// </summary>
    /// <param name="key"> the key whose insertion point to search for </param>
    /// <param name="a"> the array in which to search </param>
    /// <param name="first"> the index of the first element in the range </param>
    /// <param name="len"> the length of the range; must be > 0 </param>
    /// <param name="hint">
    /// the index at which to begin the search, 0 &lt;= hint &lt; n. The closer hint is
    /// to the result, the faster this method will run.
    /// </param>
    /// <returns> the int k, 0 &lt;= k &lt;= n such that a[b + k - 1] &lt;= key &lt; a[b + k]  </returns>
    private static int GallopRight( IComparable< T > key, T[] a, int first, int len, int hint )
    {
        ArgumentNullException.ThrowIfNull( a );

#if ALLOW_ASSERTS
        DebugAssert( ( len > 0 ) && ( hint >= 0 ) && ( hint < len ) );
#endif

        var ofs     = 1;
        var lastOfs = 0;

        if ( key.CompareTo( a[ first + hint ] ) < 0 )
        {
            // Gallop left until a[b+hint - ofs] <= key < a[b+hint - lastOfs]
            var maxOfs = hint + 1;

            while ( ( ofs < maxOfs ) && ( key.CompareTo( a[ ( first + hint ) - ofs ] ) < 0 ) )
            {
                lastOfs = ofs;
                ofs     = ( ofs << 1 ) + 1;

                if ( ofs <= 0 ) // int overflow
                {
                    ofs = maxOfs;
                }
            }

            if ( ofs > maxOfs )
            {
                ofs = maxOfs;
            }

            // Make offsets relative to b
            var tmp = lastOfs;
            lastOfs = hint - ofs;
            ofs     = hint - tmp;
        }
        else
        {
            // a[b + hint] <= key
            // Gallop right until a[b+hint + lastOfs] <= key < a[b+hint + ofs]
            var maxOfs = len - hint;

            while ( ( ofs < maxOfs ) && ( key.CompareTo( a[ first + hint + ofs ] ) >= 0 ) )
            {
                lastOfs = ofs;
                ofs     = ( ofs << 1 ) + 1;

                if ( ofs <= 0 ) // int overflow
                {
                    ofs = maxOfs;
                }
            }

            if ( ofs > maxOfs )
            {
                ofs = maxOfs;
            }

            // Make offsets relative to b
            lastOfs += hint;
            ofs     += hint;
        }

#if ALLOW_ASSERTS
        DebugAssert( ( -1 <= lastOfs ) && ( lastOfs < ofs ) && ( ofs <= len ) );
#endif

        // Now a[b + lastOfs] <= key < a[b + ofs], so key belongs somewhere to the right of lastOfs but no farther right than ofs.
        // Do a binary search, with invariant a[b + lastOfs - 1] <= key < a[b + ofs].
        lastOfs++;

        while ( lastOfs < ofs )
        {
            var m = lastOfs + ( ( ofs - lastOfs ) >>> 1 );

            if ( key?.CompareTo( a[ first + m ] ) < 0 )
            {
                ofs = m; // key < a[b + m]
            }
            else
            {
                lastOfs = m + 1; // a[b + m] <= key
            }
        }

#if ALLOW_ASSERTS
        DebugAssert( lastOfs == ofs ); // so a[b + ofs - 1] <= key < a[b + ofs]
#endif

        return ofs;
    }

    /// <summary>
    /// Merges two adjacent runs in place, in a stable fashion. The first element of the first
    /// run must be greater than the first element of the second run (a[base1] > a[base2]), and
    /// the last element of the first run (a[base1 + len1-1]) must be greater than all elements
    /// of the second run.
    /// <para>
    /// For performance, this method should be called only when len1 &lt;= len2; its twin, mergeHi
    /// should be called if len1 >= len2. (Either method may be called if len1 == len2.)
    /// </para>
    /// </summary>
    /// <param name="base1"> index of first element in first run to be merged </param>
    /// <param name="len1"> length of first run to be merged (must be > 0) </param>
    /// <param name="base2"> index of first element in second run to be merged (must be aBase + aLen) </param>
    /// <param name="len2"> length of second run to be merged (must be > 0)  </param>
    private void MergeLo( int base1, int len1, int base2, int len2 )
    {
#if ALLOW_ASSERTS
        DebugAssert( ( len1 > 0 ) && ( len2 > 0 ) && ( ( base1 + len1 ) == base2 ) );
#endif

        // Copy first run into temp array
        T[]  a   = _a!; // For performance
        T?[] tmp = EnsureCapacity( len1 );

        Array.Copy( a, base1, tmp, 0, len1 );

        var cursor1 = 0;     // Indexes into tmp array
        var cursor2 = base2; // Indexes int a
        var dest    = base1; // Indexes int a

        // Move first element of second run and deal with degenerate cases
        a[ dest++ ] = a[ cursor2++ ];

        if ( --len2 == 0 )
        {
            Array.Copy( tmp, cursor1, a, dest, len1 );

            return;
        }

        if ( len1 == 1 )
        {
            Array.Copy( a, cursor2, a, dest, len2 );

            a[ dest + len2 ] = tmp[ cursor1 ]!; // Last elt of run 1 to end of merge

            return;
        }

        var minGallop = _minGallop; // Use local variable for performance

        while ( true )
        {
            var count1 = 0; // Number of times in a row that first run won
            var count2 = 0; // Number of times in a row that second run won

            // Do the straightforward thing until (if ever) one run starts winning consistently.
            do
            {
#if ALLOW_ASSERTS
                DebugAssert( ( len1 > 1 ) && ( len2 > 0 ) );
#endif

                if ( ( ( IComparable< T > ) a[ cursor2 ]! ).CompareTo( tmp[ cursor1 ] ) < 0 )
                {
                    a[ dest++ ] = a[ cursor2++ ];
                    count2++;
                    count1 = 0;

                    if ( --len2 == 0 )
                    {
                        goto outer;
                    }
                }
                else
                {
                    a[ dest++ ] = tmp[ cursor1++ ]!;
                    count1++;
                    count2 = 0;

                    if ( --len1 == 1 )
                    {
                        goto outer;
                    }
                }
            }
            while ( ( count1 | count2 ) < minGallop );

            // One run is winning so consistently that galloping may be a huge win. So
            // try that, and continue galloping until (if ever) neither run appears to
            // be winning consistently anymore.
            do
            {
#if ALLOW_ASSERTS
                DebugAssert( ( len1 > 1 ) && ( len2 > 0 ) );
#endif

                count1 = GallopRight( ( IComparable< T > ) a[ cursor2 ]!, tmp!, cursor1, len1, 0 );

                if ( count1 != 0 )
                {
                    Array.Copy( tmp, cursor1, a, dest, count1 );

                    dest    += count1;
                    cursor1 += count1;
                    len1    -= count1;

                    if ( len1 <= 1 ) // len1 == 1 || len1 == 0
                    {
                        goto outer;
                    }
                }

                a[ dest++ ] = a[ cursor2++ ];

                if ( --len2 == 0 )
                {
                    goto outer;
                }

                count2 = GallopLeft( ( IComparable< T > ) tmp[ cursor1 ]!, a, cursor2, len2, 0 );

                if ( count2 != 0 )
                {
                    Array.Copy( a, cursor2, a, dest, count2 );
                    dest    += count2;
                    cursor2 += count2;
                    len2    -= count2;

                    if ( len2 == 0 )
                    {
                        goto outer;
                    }
                }

                a[ dest++ ] = tmp[ cursor1++ ]!;

                if ( --len1 == 1 )
                {
                    goto outer;
                }

                minGallop--;
            }
            while ( ( count1 >= MIN_GALLOP ) || ( count2 >= MIN_GALLOP ) );

            if ( minGallop < 0 )
            {
                minGallop = 0;
            }

            minGallop += 2; // Penalize for leaving gallop mode
        }                   // End of "outer" loop

    outer:

        _minGallop = minGallop < 1 ? 1 : minGallop; // Write back to field

        switch ( len1 )
        {
            case 1:
            {
#if ALLOW_ASSERTS
                DebugAssert( len2 > 0 );
#endif

                Array.Copy( a, cursor2, a, dest, len2 );

                a[ dest + len2 ] = tmp[ cursor1 ]!; // Last elt of run 1 to end of merge

                break;
            }

            case 0:
            {
                throw new ArgumentException( "Comparison method violates its general contract!" );
            }

            default:
            {
#if ALLOW_ASSERTS
                DebugAssert( len2 == 0 );
                DebugAssert( len1 > 1 );
#endif

                Array.Copy( tmp, cursor1, a, dest, len1 );

                break;
            }
        }
    }

    /// <summary>
    /// Like mergeLo, except that this method should be called only if len1 >= len2
    /// mergeLo should be called if len1 &lt;= len2. (Either method may be called if len1 == len2.)
    /// </summary>
    /// <param name="base1"> index of first element in first run to be merged </param>
    /// <param name="len1"> length of first run to be merged (must be > 0) </param>
    /// <param name="base2"> index of first element in second run to be merged (must be aBase + aLen) </param>
    /// <param name="len2"> length of second run to be merged (must be > 0)  </param>
    private void MergeHi( int base1, int len1, int base2, int len2 )
    {
#if ALLOW_ASSERTS
        DebugAssert( ( len1 > 0 ) && ( len2 > 0 ) && ( ( base1 + len1 ) == base2 ) );
#endif

        // Copy second run into temp array
        T[]? a   = _a; // For performance
        T[]  tmp = EnsureCapacity( len2 );

        Array.Copy( a!, base2, tmp, 0, len2 );

        var cursor1 = ( base1 + len1 ) - 1; // Indexes into a
        var cursor2 = len2 - 1;             // Indexes into tmp array
        var dest    = ( base2 + len2 ) - 1; // Indexes into a

        // Move last element of first run and deal with degenerate cases
        a![ dest-- ] = a[ cursor1-- ];

        if ( --len1 == 0 )
        {
            Array.Copy( tmp, 0, a, dest - ( len2 - 1 ), len2 );

            return;
        }

        if ( len2 == 1 )
        {
            dest    -= len1;
            cursor1 -= len1;

            Array.Copy( a, cursor1 + 1, a, dest + 1, len1 );

            a[ dest ] = tmp[ cursor2 ]!;

            return;
        }

        var minGallop = _minGallop; // Use local variable for performance

        while ( true )
        {
            var count1 = 0; // Number of times in a row that first run won
            var count2 = 0; // Number of times in a row that second run won

            // Do the straightforward thing until (if ever) one run appears to win consistently.
            do
            {
#if ALLOW_ASSERTS
                DebugAssert( ( len1 > 0 ) && ( len2 > 1 ) );
#endif

                if ( ( ( IComparable< T >? ) tmp[ cursor2 ] )!.CompareTo( a[ cursor1 ] ) < 0 )
                {
                    a[ dest-- ] = a[ cursor1-- ];
                    count1++;
                    count2 = 0;

                    if ( --len1 == 0 )
                    {
                        goto outer;
                    }
                }
                else
                {
                    a[ dest-- ] = tmp[ cursor2-- ]!;
                    count2++;
                    count1 = 0;

                    if ( --len2 == 1 )
                    {
                        goto outer;
                    }
                }
            }
            while ( ( count1 | count2 ) < minGallop );

            // One run is winning so consistently that galloping may be a huge win. So
            // try that, and continue galloping until (if ever) neither run appears to
            // be winning consistently anymore.
            do
            {
#if ALLOW_ASSERTS
                DebugAssert( ( len1 > 0 ) && ( len2 > 1 ) );
#endif

                count1 = len1 - GallopRight( ( IComparable< T >? ) tmp[ cursor2 ]!, a, base1, len1, len1 - 1 );

                if ( count1 != 0 )
                {
                    dest    -= count1;
                    cursor1 -= count1;
                    len1    -= count1;

                    Array.Copy( a, cursor1 + 1, a, dest + 1, count1 );

                    if ( len1 == 0 )
                    {
                        goto outer;
                    }
                }

                a[ dest-- ] = tmp[ cursor2-- ];

                if ( --len2 == 1 )
                {
                    goto outer;
                }

                count2 = len2 - GallopLeft( ( IComparable< T > ) a[ cursor1 ]!, tmp, 0, len2, len2 - 1 );

                if ( count2 != 0 )
                {
                    dest    -= count2;
                    cursor2 -= count2;
                    len2    -= count2;
                    Array.Copy( tmp, cursor2 + 1, a, dest + 1, count2 );

                    if ( len2 <= 1 )
                    {
                        goto outer; // len2 == 1 || len2 == 0
                    }
                }

                a[ dest-- ] = a[ cursor1-- ];

                if ( --len1 == 0 )
                {
                    goto outer;
                }

                minGallop--;
            }
            while ( ( count1 >= MIN_GALLOP ) || ( count2 >= MIN_GALLOP ) );

            if ( minGallop < 0 )
            {
                minGallop = 0;
            }

            minGallop += 2; // Penalize for leaving gallop mode
        }                   // End of "outer" loop

    outer:

        _minGallop = minGallop < 1 ? 1 : minGallop; // Write back to field

        if ( len2 == 1 )
        {
#if ALLOW_ASSERTS
            DebugAssert( len1 > 0 );
#endif

            dest    -= len1;
            cursor1 -= len1;

            Array.Copy( a, cursor1 + 1, a, dest + 1, len1 );

            a[ dest ] = tmp[ cursor2 ]; // Move first elt of run2 to front of merge
        }
        else if ( len2 == 0 )
        {
            throw new ArgumentException( "Comparison method violates its general contract!" );
        }
        else
        {
#if ALLOW_ASSERTS
            DebugAssert( len1 == 0 );
            DebugAssert( len2 > 0 );
#endif

            Array.Copy( tmp, 0, a, dest - ( len2 - 1 ), len2 );
        }
    }

    /// <summary>
    /// Ensures that the external array tmp has at least the specified number of elements, increasing its size if
    /// necessary. The
    /// size increases exponentially to ensure amortized linear time complexity.
    /// </summary>
    /// <param name="minCapacity"> the minimum required capacity of the tmp array </param>
    /// <returns> tmp, whether or not it grew  </returns>
    private T[] EnsureCapacity( int minCapacity )
    {
        _tmpCount = Math.Max( _tmpCount, minCapacity );

        if ( _tmp.Length < minCapacity )
        {
            // Compute smallest power of 2 > minCapacity
            var newSize = minCapacity;

            newSize |= newSize >> 1;
            newSize |= newSize >> 2;
            newSize |= newSize >> 4;
            newSize |= newSize >> 8;
            newSize |= newSize >> 16;
            newSize++;

            newSize = newSize < 0 ? minCapacity : Math.Min( newSize, _a!.Length >>> 1 );

            var newArray = new T[ newSize ];

            _tmp = newArray;
        }

        return _tmp as T[];
    }

    /// <summary>
    /// Checks that fromIndex and toIndex are in range, and throws an appropriate exception if they aren't.
    /// </summary>
    /// <param name="arrayLen"> the length of the array </param>
    /// <param name="fromIndex"> the index of the first element of the range </param>
    /// <param name="toIndex"> the index after the last element of the range </param>
    /// <exception cref="ArgumentException"> if fromIndex > toIndex </exception>
    /// <exception cref="IndexOutOfRangeException"> if fromIndex &lt; 0 or toIndex > arrayLen  </exception>
    private static void RangeCheck( int arrayLen, int fromIndex, int toIndex )
    {
        if ( fromIndex > toIndex )
        {
            throw new ArgumentException( "fromIndex(" + fromIndex + ") > toIndex(" + toIndex + ")" );
        }

        if ( fromIndex < 0 )
        {
            throw new IndexOutOfRangeException( fromIndex.ToString() );
        }

        if ( toIndex > arrayLen )
        {
            throw new IndexOutOfRangeException( toIndex.ToString() );
        }
    }
}
