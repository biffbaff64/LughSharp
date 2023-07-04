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
/// A stable, adaptive, iterative mergesort that requires far fewer than n lg(n)
/// comparisons when running on partially sorted arrays, while offering performance
/// comparable to a traditional mergesort when run on random arrays.
/// <p>
/// Like all proper mergesorts, this sort is stable and runs O(n log n) time (worst case).
/// In the worst case, this sort requires temporary storage space for n/2 object
/// references; in the best case, it requires only a small constant amount of space.
/// </p>
/// <p>
/// This implementation was adapted from Tim Peters's list sort for Python, which is
/// described in detail here:
/// </p>
/// <p>
/// http://svn.python.org/projects/python/trunk/Objects/listsort.txt
/// </p>
/// <p>Tim's C code may be found here:</p>
/// <p>
/// http://svn.python.org/projects/python/trunk/Objects/listobject.c
/// </p>
/// <p>
/// The underlying techniques are described in this paper (and may have even earlier origins):
/// "Optimistic Sorting and Information Theoretic Complexity" Peter McIlroy SODA (Fourth Annual
/// ACM-SIAM Symposium on Discrete Algorithms), pp 467-474, Austin, Texas, 25-27 January 1993.
/// </p>
/// <p>
/// While the API to this class consists solely of static methods, it is (privately) instantiable;
/// a TimSort instance holds the state of an ongoing sort, assuming the input array is large enough
/// to warrant the full-blown TimSort. Small arrays are sorted in place, using a binary insertion sort.
/// </p>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class TimSort<T>
{
    /// <summary>
    /// This is the minimum sized sequence that will be merged. Shorter sequences will
    /// be lengthened by calling binarySort. If the entire array is less than this length,
    /// no merges will be performed.
    /// <p>
    /// This constant should be a power of two. It was 64 in Tim Peter's C implementation,
    /// but 32 was empirically determined to work better in this implementation. In the
    /// unlikely event that you set this constant to be a number that's not a power of two,
    /// you'll need to change the <see cref="MinRunLength"/> computation.
    /// </p>
    /// <p>
    /// If you decrease this constant, you must change the stackLen computation in the
    /// TimSort constructor, or you risk an ArrayOutOfBounds exception. See listsort.txt
    /// for a discussion of the minimum stack length required as a function of the
    /// length of the array being sorted and the minimum merge sequence length. 
    /// </p>
    /// </summary>
    private const int MinMerge = 32;

    /// <summary>
    /// The array being sorted.
    /// </summary>
    private T[]? _a;

    /// <summary>
    /// The comparator for this sort.
    /// </summary>
    private IComparer< T >? _c;

    /// <summary>
    /// When we get into galloping mode, we stay there until both runs win less
    /// often than MIN_GALLOP consecutive times.
    /// </summary>
    private const int MinGallop = 7;

    /// <summary>
    /// This controls when we get *into* galloping mode. It is initialized to MIN_GALLOP.
    /// The mergeLo and mergeHi methods nudge it higher for random data, and lower for
    /// highly structured data. 
    /// </summary>
    private int _minGallop = MinGallop;

    /// <summary>
    /// Maximum initial size of tmp array, which is used for merging.
    /// The array can grow to accommodate demand.
    /// 
    /// Unlike Tim's original C version, we do not allocate this much storage when sorting
    /// smaller arrays. This change was required for performance. 
    /// </summary>
    private const int InitialTmpStorageLength = 256;

    /// <summary>
    /// Temp storage for merges.
    /// </summary>
    private T[] _tmp; // Actual runtime type will be Object[], regardless of T
    private int _tmpCount;

    /// <summary>
    /// A stack of pending runs yet to be merged.
    /// Run i starts at address base[i] and extends for len[i] elements.
    /// <p>
    /// It's always true (so long as the indices are in bounds) that:
    /// </p>
    /// <code>
    /// runBase[i] + runLen[i] == runBase[i + 1]
    /// </code>
    /// <p>
    /// so we could cut the storage for this, but it's a minor amount, and keeping all
    /// the info explicit simplifies the code.
    /// </p>
    /// </summary>
    private int _stackSize = 0; // Number of pending runs on stack
        
    private readonly int[] _runBase;
    private readonly int[] _runLen;

    /// <summary>
    /// Asserts have been placed in if-statements for performance.
    /// To enable them, set this field to true.
    /// If you modify this class, please do test the asserts! 
    /// </summary>
    private const bool AllowAsserts = true; //false;

    /// <summary>
    /// </summary>
    public TimSort()
    {
        _tmp     = new T[ InitialTmpStorageLength ];
        _runBase = new int[ 40 ];
        _runLen  = new int[ 40 ];
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <param name="lo"></param>
    /// <param name="hi"></param>
    public void DoSort( T[] a, IComparer< T > c, int lo, int hi )
    {
        _stackSize = 0;
            
        RangeCheck( a.Length, lo, hi );

        var remaining = hi - lo;

        // Arrays of size 0 and 1 are always sorted
        if ( remaining < 2 ) return;

        // If array is small, do a "mini-TimSort" with no merges
        if ( remaining < MinMerge )
        {
            var initRunLen = CountRunAndMakeAscending( a, lo, hi, c );

            BinarySort( a, lo, hi, lo + initRunLen, c );

            return;
        }

        this._a   = a;
        this._c   = c;
        _tmpCount = 0;

        // March over the array once, left to right, finding natural runs, extending
        // short natural runs to minRun elements, and  merging runs to maintain stack
        // invariant. 
        var minRun = MinRunLength( remaining );

        do
        {
            // Identify next run
            var runLen = CountRunAndMakeAscending( a, lo, hi, c );

            // If run is short, extend to min(minRun, nRemaining)
            if ( runLen < minRun )
            {
                var force = remaining <= minRun ? remaining : minRun;

                BinarySort( a, lo, lo + force, lo + runLen, c );
                    
                runLen = force;
            }

            // Push run onto pending-run stack, and maybe merge
            PushRun( lo, runLen );
            MergeCollapse();

            // Advance to find next run
            lo        += runLen;
            remaining -= runLen;
        }
        while ( remaining != 0 );

        // Merge all remaining runs to complete sort
        if ( AllowAsserts ) Debug.Assert( lo == hi );
            
        MergeForceCollapse();
            
        if ( AllowAsserts ) Debug.Assert( _stackSize == 1 );

        this._a = null;
        this._c = null;

        for ( int i = 0, n = _tmpCount; i < n; i++ )
        {
            _tmp[ i ] = default!;
        }
    }

    /// <summary>
    /// Creates a TimSort instance to maintain the state of an ongoing sort.
    /// </summary>
    /// <param name="a"> the array to be sorted </param>
    /// <param name="c"> the comparator to determine the order of the sort</param>
    private TimSort( T[] a, IComparer< T > c )
    {
        this._a = a;
        this._c = c;

        // Allocate temp storage (which may be increased later if necessary)
        var len = a.Length;
            
        _tmp = new T[ len < ( 2 * InitialTmpStorageLength ) ? len >>> 1 : InitialTmpStorageLength ];

        // Allocate runs-to-be-merged stack (which cannot be expanded). The stack length
        // requirements are described in listsort.txt. The C version always uses the same
        // stack length (85), but this was measured to be too expensive when sorting "mid-sized"
        // arrays (e.g., 100 elements) in the Java code this library is converted from.
        // Therefore, we use smaller (but sufficiently large) stack lengths for smaller arrays.
        // The "magic numbers" in the computation below must be changed if MinMerge is decreased.
        // See the MinMerge declaration above for more information.
        var stackLen = ( len < 120 ? 5 : len < 1542 ? 10 : len < 119151 ? 19 : 40 );
            
        _runBase = new int[ stackLen ];
        _runLen  = new int[ stackLen ];
    }

    // The next two methods (which are package private and static) constitute the entire
    // API of this class. Each of these methods obeys the contract of the public method
    // with the same signature in java.util.Arrays.

    internal static void Sort( T[] a, IComparer< T > c )
    {
        Sort( a, 0, a.Length, c );
    }

    internal static void Sort( T[] a, int lo, int hi, IComparer< T >? c )
    {
        if ( c == null )
        {
            Array.Sort( a, lo, hi );

            return;
        }

        RangeCheck( a.Length, lo, hi );
            
        var nRemaining = hi - lo;

        if ( nRemaining < 2 ) return; // Arrays of size 0 and 1 are always sorted

        // If array is small, do a "mini-TimSort" with no merges
        if ( nRemaining < MinMerge )
        {
            var initRunLen = CountRunAndMakeAscending( a, lo, hi, c );

            BinarySort( a, lo, hi, lo + initRunLen, c );

            return;
        }

        // March over the array once, left to right, finding natural runs, extending short
        // natural runs to minRun elements, and merging runs to maintain stack invariant.
        var ts     = new TimSort< T >( a, c );
        var minRun = MinRunLength( nRemaining );

        do
        {
            // Identify next run
            var runLen = CountRunAndMakeAscending( a, lo, hi, c );

            // If run is short, extend to min(minRun, nRemaining)
            if ( runLen < minRun )
            {
                var force = nRemaining <= minRun ? nRemaining : minRun;
                BinarySort( a, lo, lo + force, lo + runLen, c );
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

        // Merge all remaining runs to complete sort
        if ( AllowAsserts ) Debug.Assert( lo == hi );
            
        ts.MergeForceCollapse();
            
        if ( AllowAsserts ) Debug.Assert( ts._stackSize == 1 );
    }

    /// <summary>
    /// Sorts the specified portion of the specified array using a binary insertion sort.
    /// This is the best method for sorting small numbers of elements. It requires O(n log n)
    /// compares, but O(n^2) data movement (worst case).
    /// <p>
    /// If the initial part of the specified range is already sorted, this method can take
    /// advantage of it: the method assumes that the elements from index lo, inclusive, to
    /// start, exclusive are already sorted.
    /// </p>
    /// </summary>
    /// <param name="a"> the array in which a range is to be sorted </param>
    /// <param name="lo"> the index of the first element in the range to be sorted </param>
    /// <param name="hi"> the index after the last element in the range to be sorted </param>
    /// <param name="start">
    /// the index of the first element in the range that is not already known to be sorted
    /// <code> lo &lt;= start &lt;= hi</code>
    /// </param>
    /// <param name="c"> comparator to used for the sort</param>
    private static void BinarySort( T[] a, int lo, int hi, int start, IComparer< T > c )
    {
        if ( AllowAsserts ) Debug.Assert( ( lo <= start ) && ( start <= hi ) );

        if ( start == lo ) start++;

        for ( ; start < hi; start++ )
        {
            T pivot = a[ start ];

            // Set left (and right) to the index where a[start] (pivot) belongs
            var left  = lo;
            var right = start;
                
            if ( AllowAsserts ) Debug.Assert( left <= right );

            // Invariants: pivot >= all in [lo, left). pivot < all in [right, start).
            while ( left < right )
            {
                var mid = ( left + right ) >>> 1;

                if ( c.Compare( pivot, a[ mid ] ) < 0 )
                {
                    right = mid;
                }
                else
                {
                    left = mid + 1;
                }
            }

            if ( AllowAsserts ) Debug.Assert( left == right );

            // The invariants still hold: pivot >= all in [lo, left) and pivot < all in
            // [left, start), so pivot belongs at left.
            // Note that if there are elements equal to pivot, left points to the first
            // slot after them -- that's why this sort is stable.
                
            // Slide elements over to make room for pivot.
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
                Array.Copy( a, left, 
                            a, left + 1, n );
            }

            a[ left ] = pivot;
        }
    }

    /// <summary>
    /// Returns the length of the run beginning at the specified position in the specified array and reverses the run if it is
    /// descending (ensuring that the run will always be ascending when the method returns).
    /// <p>
    /// A run is the longest ascending sequence with:
    /// </p>
    /// <p>
    /// a[lo] &lt;= a[lo + 1] &lt;= a[lo + 2] &lt;= ...
    /// </p>
    /// <p>
    /// or the longest descending sequence with:
    /// </p>
    /// <p>
    /// a[lo] > a[lo + 1] > a[lo + 2] > ...
    /// </p>
    /// <p>
    /// For its intended use in a stable mergesort, the strictness of the definition of
    /// "descending" is needed so that the call can safely reverse a descending sequence
    /// without violating stability.
    /// </p>
    /// </summary>
    /// <param name="a"> the array in which a run is to be counted and possibly reversed </param>
    /// <param name="lo"> index of the first element in the run </param>
    /// <param name="hi">
    /// index after the last element that may be contained in the run.
    /// It is required that <code>lo &lt; hi</code>
    /// </param>
    /// <param name="c"> the comparator to used for the sort </param>
    /// <returns> the length of the run beginning at the specified position in the specified array  </returns>
    private static int CountRunAndMakeAscending( T[] a, int lo, int hi, IComparer< T > c )
    {
        if ( AllowAsserts ) Debug.Assert( lo < hi );

        var runHi = lo + 1;

        if ( runHi == hi ) return 1;

        // Find end of run, and reverse range if descending
        if ( c.Compare( a[ runHi++ ], a[ lo ] ) < 0 )
        {
            // Descending
            while ( ( runHi < hi ) && ( c.Compare( a[ runHi ], a[ runHi - 1 ] ) < 0 ) )
            {
                runHi++;
            }

            ReverseRange( a, lo, runHi );
        }
        else
        {
            // Ascending
            while ( ( runHi < hi ) && ( c.Compare( a[ runHi ], a[ runHi - 1 ] ) >= 0 ) )
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
            T t = a[ lo ];

            a[ lo++ ] = a[ hi ];
            a[ hi-- ] = t;
        }
    }

    /// <summary>
    /// Returns the minimum acceptable run length for an array of the specified length.
    /// Natural runs shorter than this will be extended with <see cref="BinarySort"/>.
    /// <p>
    /// Roughly speaking, the computation is:
    /// </p>
    /// <p>If n &lt; MinMerge, return n (it's too small to bother with fancy stuff).</p>
    /// <p>Else if n is an exact power of 2, return MinMerge/2.</p>
    /// <p>Else return an int k, MinMerge/2 &lt;= k &lt;= MinMerge, such that n/k is close to,
    /// but strictly less than, an exact power of 2.</p>
    /// <p>For the rationale, see listsort.txt.</p>
    /// </summary>
    /// <param name="n"> the length of the array to be sorted </param>
    /// <returns> the length of the minimum run to be merged  </returns>
    private static int MinRunLength( int n )
    {
        if ( AllowAsserts ) Debug.Assert( n >= 0 );

        var r = 0; // Becomes 1 if any 1 bits are shifted off

        while ( n >= MinMerge )
        {
            r |=  ( n & 1 );
            n >>= 1;
        }

        return n + r;
    }

    /// <summary>
    /// Pushes the specified run onto the pending-run stack.
    /// </summary>
    /// <param name="runBase">index of the first element in the run</param>
    ///	<param name="runLen">the number of elements in the run</param>
    private void PushRun( int runBase, int runLen )
    {
        this._runBase[ _stackSize ] = runBase;
        this._runLen[ _stackSize ]  = runLen;

        _stackSize++;
    }

    /// <summary>
    /// Examines the stack of runs waiting to be merged and merges adjacent runs until the
    /// stack invariants are reestablished:
    /// <p>
    /// 1. runLen[n - 2] &gt; runLen[n - 1] + runLen[n] 2. runLen[n - 1] &gt; runLen[n]
    /// </p>
    /// where n is the index of the last run in runLen.
    /// 
    /// This method has been formally verified to be correct after checking the last 4 runs.
    /// Checking for 3 runs results in an exception for large arrays.
    /// <p>
    /// (Source: http://envisage-project.eu/proving-android-java-and-python-sorting-algorithm-is-broken-and-how-to-fix-it/)
    /// </p>
    /// This method is called each time a new run is pushed onto the stack, so the invariants
    /// are guaranteed to hold for i &lt; stackSize upon entry to the method. 
    /// </summary>
    private void MergeCollapse()
    {
        while ( _stackSize > 1 )
        {
            var n = _stackSize - 2;

            if ( ( ( n >= 1 ) && ( _runLen[ n - 1 ] <= ( _runLen[ n ] + _runLen[ n + 1 ] ) ) )
                 || ( ( n >= 2 ) && ( _runLen[ n - 2 ] <= ( _runLen[ n ] + _runLen[ n - 1 ] ) ) ) )
            {
                if ( _runLen[ n - 1 ] < _runLen[ n + 1 ] ) n--;
            }
            else if ( _runLen[ n ] > _runLen[ n + 1 ] )
            {
                break; // Invariant is established
            }

            MergeAt( n );
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
    /// <param name="i">stack index of the first of the two runs to merge</param>
    private void MergeAt( int i )
    {
        if ( AllowAsserts ) Debug.Assert( _stackSize >= 2 );
        if ( AllowAsserts ) Debug.Assert( i >= 0 );
        if ( AllowAsserts ) Debug.Assert( ( i == ( _stackSize - 2 ) ) || ( i == ( _stackSize - 3 ) ) );

        var base1 = _runBase[ i ];
        var len1  = _runLen[ i ];
        var base2 = _runBase[ i + 1 ];
        var len2  = _runLen[ i + 1 ];

        if ( AllowAsserts ) Debug.Assert( ( len1 > 0 ) && ( len2 > 0 ) );
        if ( AllowAsserts ) Debug.Assert( ( base1 + len1 ) == base2 );

        // Record the length of the combined runs; if i is the 3rd-last run now, also
        // slide over the last run (which isn't involved in this merge).
        // The current run (i+1) goes away in any case.
        _runLen[ i ] = len1 + len2;

        if ( i == ( _stackSize - 3 ) )
        {
            _runBase[ i + 1 ] = _runBase[ i + 2 ];
            _runLen[ i + 1 ]  = _runLen[ i + 2 ];
        }

        _stackSize--;

        // Find where the first element of run2 goes in run1. Prior elements in run1 can
        // be ignored (because they're already in place).
        var k = GallopRight( _a![ base2 ], _a, base1, len1, 0, _c );

        if ( AllowAsserts ) Debug.Assert( k >= 0 );

        base1 += k;
        len1  -= k;

        if ( len1 == 0 ) return;

        // Find where the last element of run1 goes in run2. Subsequent elements in run2
        // can be ignored (because they're already in place).
        len2 = GallopLeft( _a[ ( base1 + len1 ) - 1 ], _a, base2, len2, len2 - 1, _c );

        if ( AllowAsserts ) Debug.Assert( len2 >= 0 );

        if ( len2 == 0 ) return;

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
    /// Locates the position at which to insert the specified key into the specified sorted range;
    /// if the range contains an element equal to key, returns the index of the leftmost equal element.
    /// </summary>
    /// <param name="key">the key's insertion point to search for.</param>
    /// <param name="a"> the array in which to search. </param>
    /// <param name="baseIndex"> the index of the first element in the range </param>
    /// <param name="len"> the length of the range; must be > 0 </param>
    /// <param name="hint">
    /// the index at which to begin the search, 0 &lt;= hint &lt; n.
    /// The closer hint is to the result, the faster this method will run.
    /// </param>
    /// <param name="c"> the comparator used to order the range, and to search </param>
    /// <returns>
    /// the int k, 0 &lt;= k &lt;= n such that a[b + k - 1] &lt; key &lt;= a[b + k], pretending
    /// that a[b - 1] is minus infinity and a[b + n] is infinity.
    /// <p>In other words, key belongs at index b + k;</p>
    /// <p>
    /// or in other words, the first k elements of a should precede key, and the
    /// last n - k should follow it.
    /// </p>
    /// </returns>
    private static int GallopLeft( T? key, IReadOnlyList< T > a, int baseIndex, int len, int hint, IComparer< T >? c )
    {
        if ( AllowAsserts ) Debug.Assert( ( len > 0 ) && ( hint >= 0 ) && ( hint < len ) );

        var lastOfs = 0;
        var ofs     = 1;

        if ( c?.Compare( key, a[ baseIndex + hint ] ) > 0 )
        {
            // Gallop right until a[base+hint+lastOfs] < key <= a[base+hint+ofs]
            var maxOfs = len - hint;

            while ( ( ofs < maxOfs ) && ( c.Compare( key, a[ baseIndex + hint + ofs ] ) > 0 ) )
            {
                lastOfs = ofs;
                ofs     = ( ofs << 1 ) + 1;

                if ( ofs <= 0 ) // int overflow
                {
                    ofs = maxOfs;
                }
            }

            if ( ofs > maxOfs ) ofs = maxOfs;

            // Make offsets relative to base
            lastOfs += hint;
            ofs     += hint;
        }
        else
        {
            // key <= a[base + hint]
            // Gallop left until a[base+hint-ofs] < key <= a[base+hint-lastOfs]
            var maxOfs = hint + 1;

            while ( ( ofs < maxOfs ) && ( c?.Compare( key, a[ ( baseIndex + hint ) - ofs ] ) <= 0 ) )
            {
                lastOfs = ofs;
                ofs     = ( ofs << 1 ) + 1;

                if ( ofs <= 0 ) // int overflow
                {
                    ofs = maxOfs;
                }
            }

            if ( ofs > maxOfs ) ofs = maxOfs;

            // Make offsets relative to base
            var tmp = lastOfs;

            lastOfs = hint - ofs;
            ofs     = hint - tmp;
        }

        if ( AllowAsserts ) Debug.Assert( ( -1 <= lastOfs ) && ( lastOfs < ofs ) && ( ofs <= len ) );

        // Now a[base+lastOfs] < key <= a[base+ofs], so key belongs somewhere to the right
        // of lastOfs but no farther right than ofs.
        // Do a binary search, with invariant a[base + lastOfs - 1] < key <= a[base + ofs].
        lastOfs++;

        while ( lastOfs < ofs )
        {
            var m = lastOfs + ( ( ofs - lastOfs ) >>> 1 );

            if ( c?.Compare( key, a[ baseIndex + m ] ) > 0 )
            {
                lastOfs = m + 1; // a[base + m] < key
            }
            else
            {
                ofs = m; // key <= a[base + m]
            }
        }

        if ( AllowAsserts ) Debug.Assert( lastOfs == ofs ); // so a[base + ofs - 1] < key <= a[base + ofs]

        return ofs;
    }

    /// <summary>
    /// Like GallopLeft, except that if the range contains an element equal to key,
    /// GallopRight returns the index after the rightmost equal element.
    /// </summary>
    /// <param name="key"> the key whose insertion point to search for </param>
    /// <param name="a"> the array in which to search </param>
    /// <param name="baseIndex"> the index of the first element in the range </param>
    /// <param name="len"> the length of the range; must be > 0 </param>
    /// <param name="hint">
    /// the index at which to begin the search, 0 &lt;= hint &lt; n. The closer hint is
    /// to the result, the faster this method will run.
    /// </param>
    /// <param name="c"> the comparator used to order the range, and to search </param>
    /// <returns>
    /// the int k, 0 &lt;= k &lt;= n such that a[b + k - 1] &lt;= key &lt; a[b + k].
    /// </returns>
    private static int GallopRight( T? key, IReadOnlyList< T >? a, int baseIndex, int len, int hint, IComparer< T >? c )
    {
        if ( AllowAsserts ) Debug.Assert( ( len > 0 ) && ( hint >= 0 ) && ( hint < len ) );

        var ofs     = 1;
        var lastOfs = 0;

        if ( c?.Compare( key, a![ baseIndex + hint ] ) < 0 )
        {
            // Gallop left until a[b+hint - ofs] <= key < a[b+hint - lastOfs]
            var maxOfs = hint + 1;

            while ( ( ofs < maxOfs ) && ( c.Compare( key, a![ ( baseIndex + hint ) - ofs ] ) < 0 ) )
            {
                lastOfs = ofs;
                ofs     = ( ofs << 1 ) + 1;

                if ( ofs <= 0 ) // int overflow
                {
                    ofs = maxOfs;
                }
            }

            if ( ofs > maxOfs ) ofs = maxOfs;

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

            while ( ( ofs < maxOfs ) && ( c?.Compare( key, a![ baseIndex + hint + ofs ] ) >= 0 ) )
            {
                lastOfs = ofs;
                ofs     = ( ofs << 1 ) + 1;

                if ( ofs <= 0 ) // int overflow
                {
                    ofs = maxOfs;
                }
            }

            if ( ofs > maxOfs ) ofs = maxOfs;

            // Make offsets relative to b
            lastOfs += hint;
            ofs     += hint;
        }

        if ( AllowAsserts ) Debug.Assert( ( -1 <= lastOfs ) && ( lastOfs < ofs ) && ( ofs <= len ) );

        // Now a[b + lastOfs] <= key < a[b + ofs], so key belongs somewhere to the right
        // of lastOfs but no farther right than ofs.
        // Do a binary search, with invariant a[b + lastOfs - 1] <= key < a[b + ofs].
        lastOfs++;

        while ( lastOfs < ofs )
        {
            var m = lastOfs + ( ( ofs - lastOfs ) >>> 1 );

            if ( c?.Compare( key, a![ baseIndex + m ] ) < 0 )
            {
                ofs = m; // key < a[b + m]
            }
            else
            {
                lastOfs = m + 1; // a[b + m] <= key
            }
        }

        if ( AllowAsserts ) Debug.Assert( lastOfs == ofs ); // so a[b + ofs - 1] <= key < a[b + ofs]

        return ofs;
    }

    /// <summary>
    /// Merges two adjacent runs in place, in a stable fashion.
    /// <p>
    /// The first element of the first run must be greater than the first element of the
    /// second run (a[base1] > a[base2]), and the last element of the first run
    /// (a[base1 + len1-1]) must be greater than all elements of the second run.
    /// </p>
    /// <p>
    /// For performance, this method should be called only when len1 &lt;= len2;
    /// its twin, MergeHi should be called if len1 >= len2.
    /// (Either method may be called if len1 == len2.)
    /// </p>
    /// </summary>
    /// <param name="base1"> index of first element in first run to be merged </param>
    /// <param name="len1"> length of first run to be merged (must be > 0) </param>
    /// <param name="base2"> index of first element in second run to be merged (must be aBase + aLen) </param>
    /// <param name="len2"> length of second run to be merged (must be > 0)  </param>
    private void MergeLo( int base1, int len1, int base2, int len2 )
    {
        if ( AllowAsserts ) Debug.Assert( ( len1 > 0 ) && ( len2 > 0 ) && ( ( base1 + len1 ) == base2 ) );

        // Copy first run into temp array
        var a   = this._a; // For performance
        var tmp = EnsureCapacity( len1 );

        if ( a == null ) throw new NullReferenceException();
            
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

            a[ dest + len2 ] = tmp[ cursor1 ]; // Last elt of run 1 to end of merge

            return;
        }

        var c         = this._c;         // Use local variable for performance
        var minGallop = this._minGallop; // "    " "     " "

        while ( true )
        {
            var count1 = 0; // Number of times in a row that first run won
            var count2 = 0; // Number of times in a row that second run won

            // Do the straightforward thing until (if ever) one run starts winning consistently.
            do
            {
                if ( AllowAsserts ) Debug.Assert( ( len1 > 1 ) && ( len2 > 0 ) );

                if ( c?.Compare( a[ cursor2 ], tmp[ cursor1 ] ) < 0 )
                {
                    a[ dest++ ] = a[ cursor2++ ];
                    count2++;
                    count1 = 0;

                    if ( --len2 == 0 ) goto outer;
                }
                else
                {
                    a[ dest++ ] = tmp[ cursor1++ ];
                    count1++;
                    count2 = 0;

                    if ( --len1 == 1 ) goto outer;
                }
            }
            while ( ( count1 | count2 ) < minGallop );

            // One run is winning so consistently that galloping may be a huge win.
            // So try that, and continue galloping until (if ever) neither run
            // appears to be winning consistently anymore.
            do
            {
                if ( AllowAsserts ) Debug.Assert( ( len1 > 1 ) && ( len2 > 0 ) );

                count1 = GallopRight( a[ cursor2 ], tmp, cursor1, len1, 0, c );

                if ( count1 != 0 )
                {
                    Array.Copy( tmp, cursor1, a, dest, count1 );

                    dest    += count1;
                    cursor1 += count1;
                    len1    -= count1;

                    if ( len1 <= 1 ) goto outer;
                }

                a[ dest++ ] = a[ cursor2++ ];

                if ( --len2 == 0 ) goto outer;

                count2 = GallopLeft( tmp[ cursor1 ], a, cursor2, len2, 0, c );

                if ( count2 != 0 )
                {
                    Array.Copy( a, cursor2, a, dest, count2 );

                    dest    += count2;
                    cursor2 += count2;
                    len2    -= count2;

                    if ( len2 == 0 ) goto outer;
                }

                a[ dest++ ] = tmp[ cursor1++ ];

                if ( --len1 == 1 ) goto outer;

                minGallop--;
            }
            while ( ( count1 >= MinGallop ) || ( count2 >= MinGallop ) );

            if ( minGallop < 0 ) minGallop = 0;

            minGallop += 2; // Penalize for leaving gallop mode
        }                   // End of "outer" loop

        outer:

        this._minGallop = minGallop < 1 ? 1 : minGallop; // Write back to field

        if ( len1 == 1 )
        {
            if ( AllowAsserts ) Debug.Assert( len2 > 0 );

            Array.Copy( a, cursor2, a, dest, len2 );

            a[ dest + len2 ] = tmp[ cursor1 ]; // Last elt of run 1 to end of merge
        }
        else if ( len1 == 0 )
        {
            throw new ArgumentException( "Comparison method violates its general contract!" );
        }
        else
        {
            if ( AllowAsserts ) Debug.Assert( len2 == 0 );
            if ( AllowAsserts ) Debug.Assert( len1 > 1 );

            Array.Copy( tmp, cursor1, a, dest, len1 );
        }
    }

    /// <summary>
    /// Like mergeLo, except that this method should be called only if len1 &gt;= len2;
    /// mergeLo should be called if len1 &lt;= len2.
    /// (Either method may be called if len1 == len2.)
    /// </summary>
    /// <param name="base1"> index of first element in first run to be merged </param>
    /// <param name="len1"> length of first run to be merged (must be > 0) </param>
    /// <param name="base2"> index of first element in second run to be merged (must be aBase + aLen) </param>
    /// <param name="len2"> length of second run to be merged (must be > 0)  </param>
    private void MergeHi( int base1, int len1, int base2, int len2 )
    {
        if ( AllowAsserts ) Debug.Assert( ( len1 > 0 ) && ( len2 > 0 ) && ( ( base1 + len1 ) == base2 ) );

        // Copy second run into temp array
        var a   = this._a; // For performance
        var tmp = EnsureCapacity( len2 );

        if ( a == null ) throw new NullReferenceException();
            
        Array.Copy( a, base2, tmp, 0, len2 );

        var cursor1 = ( base1 + len1 ) - 1; // Indexes into a
        var cursor2 = len2 - 1;         // Indexes into tmp array
        var dest    = ( base2 + len2 ) - 1; // Indexes into a

        // Move last element of first run and deal with degenerate cases
        a[ dest-- ] = a[ cursor1-- ];

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

            a[ dest ] = tmp[ cursor2 ];

            return;
        }

        var c         = this._c;         // Use local variable for performance
        var minGallop = this._minGallop; // "    " "     " "

        while ( true )
        {
            var count1 = 0; // Number of times in a row that first run won
            var count2 = 0; // Number of times in a row that second run won

            /*
             * Do the straightforward thing until (if ever) one run appears to win consistently.
             */
            do
            {
                if ( AllowAsserts ) Debug.Assert( ( len1 > 0 ) && ( len2 > 1 ) );

                if ( c?.Compare( tmp[ cursor2 ], a[ cursor1 ] ) < 0 )
                {
                    a[ dest-- ] = a[ cursor1-- ];

                    count1++;
                    count2 = 0;

                    if ( --len1 == 0 ) goto outer;
                }
                else
                {
                    a[ dest-- ] = tmp[ cursor2-- ];

                    count2++;
                    count1 = 0;

                    if ( --len2 == 1 ) goto outer;
                }
            }
            while ( ( count1 | count2 ) < minGallop );

            // One run is winning so consistently that galloping may be a huge win.
            // So try that, and continue galloping until (if ever) neither run appears
            // to be winning consistently anymore.
            do
            {
                if ( AllowAsserts ) Debug.Assert( ( len1 > 0 ) && ( len2 > 1 ) );

                count1 = len1 - GallopRight( tmp[ cursor2 ], a, base1, len1, len1 - 1, c );

                if ( count1 != 0 )
                {
                    dest    -= count1;
                    cursor1 -= count1;
                    len1    -= count1;

                    Array.Copy( a, cursor1 + 1, a, dest + 1, count1 );

                    if ( len1 == 0 ) goto outer;
                }

                a[ dest-- ] = tmp[ cursor2-- ];

                if ( --len2 == 1 ) goto outer;

                count2 = len2 - GallopLeft( a[ cursor1 ], tmp, 0, len2, len2 - 1, c );

                if ( count2 != 0 )
                {
                    dest    -= count2;
                    cursor2 -= count2;
                    len2    -= count2;

                    Array.Copy( tmp, cursor2 + 1, a, dest + 1, count2 );

                    if ( len2 <= 1 ) goto outer;
                }

                a[ dest-- ] = a[ cursor1-- ];

                if ( --len1 == 0 ) goto outer;

                minGallop--;
            }
            while ( ( count1 >= MinGallop ) || ( count2 >= MinGallop ) );

            if ( minGallop < 0 ) minGallop = 0;

            minGallop += 2; // Penalize for leaving gallop mode
        }                   // End of "outer" loop

        outer:

        this._minGallop = minGallop < 1 ? 1 : minGallop; // Write back to field

        if ( len2 == 1 )
        {
            if ( AllowAsserts ) Debug.Assert( len1 > 0 );

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
            if ( AllowAsserts ) Debug.Assert( len1 == 0 );
            if ( AllowAsserts ) Debug.Assert( len2 > 0 );

            Array.Copy( tmp, 0, a, dest - ( len2 - 1 ), len2 );
        }
    }

    /// <summary>
    /// Ensures that the external array tmp has at least the specified number of elements, increasing its size if necessary. The
    /// size increases exponentially to ensure amortized linear time complexity.
    ///
    /// @param minCapacity the minimum required capacity of the tmp array
    /// @return tmp, whether or not it grew
    /// </summary>
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

            if ( newSize < 0 )
            {
                newSize = minCapacity;
            }
            else
            {
                if ( _a != null )
                {
                    newSize = Math.Min( newSize, _a.Length >>> 1 );
                }
            }

            var newArray = new T[ newSize ];

            _tmp = newArray;
        }

        return _tmp;
    }

    /// <summary>
    /// Checks that fromIndex and toIndex are in range, and throws an appropriate exception if they aren't.
    /// </summary>
    /// <param name="arrayLen"> the length of the array </param>
    /// <param name="fromIndex"> the index of the first element of the range </param>
    /// <param name="toIndex"> the index after the last element of the range </param>
    /// <exception cref="ArgumentException"> if fromIndex &gt; toIndex</exception>
    /// <exception cref="IndexOutOfRangeException"> if fromIndex &lt; 0 or toIndex &gt; arrayLen </exception>
    private static void RangeCheck( int arrayLen, int fromIndex, int toIndex )
    {
        if ( fromIndex > toIndex ) throw new ArgumentException( "fromIndex(" + fromIndex + ") > toIndex(" + toIndex + ")" );
        if ( fromIndex < 0 ) throw new IndexOutOfRangeException( fromIndex.ToString() );
        if ( toIndex > arrayLen ) throw new IndexOutOfRangeException( toIndex.ToString() );
    }
}