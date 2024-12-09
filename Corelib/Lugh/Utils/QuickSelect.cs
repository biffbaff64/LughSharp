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
/// Implementation of Tony Hoare's quickselect algorithm. Running time is generally
/// O(n), but worst case is O(n^2) Pivot choice is median of three method, providing
/// better performance than a random pivot for partially sorted data.
/// <para> http://en.wikipedia.org/wiki/Quickselect </para>
/// </summary>
[PublicAPI]
public class QuickSelect< T >
{
    private T[]            _array = null!;
    private IComparer< T > _comp  = null!;

    public int Select( T[] items, IComparer< T > comp, int n, int size )
    {
        _array = items;
        _comp  = comp;

        return RecursiveSelect( 0, size - 1, n );
    }

    private int Partition( int left, int right, int pivot )
    {
        var pivotValue = _array[ pivot ];

        Swap( right, pivot );

        var storage = left;

        for ( var i = left; i < right; i++ )
        {
            if ( _comp.Compare( _array[ i ], pivotValue ) < 0 )
            {
                Swap( storage, i );
                storage++;
            }
        }

        Swap( right, storage );

        return storage;
    }

    private int RecursiveSelect( int left, int right, int k )
    {
        if ( left == right )
        {
            return left;
        }

        var pivotIndex    = MedianOfThreePivot( left, right );
        var pivotNewIndex = Partition( left, right, pivotIndex );
        var pivotDist     = ( pivotNewIndex - left ) + 1;

        int result;

        if ( pivotDist == k )
        {
            result = pivotNewIndex;
        }
        else if ( k < pivotDist )
        {
            result = RecursiveSelect( left, pivotNewIndex - 1, k );
        }
        else
        {
            result = RecursiveSelect( pivotNewIndex + 1, right, k - pivotDist );
        }

        return result;
    }

    /// <summary>
    /// Median of Three has the potential to outperform a random pivot, especially
    /// for partially sorted arrays
    /// </summary>
    private int MedianOfThreePivot( int leftIdx, int rightIdx )
    {
        var left   = _array[ leftIdx ];
        var midIdx = ( leftIdx + rightIdx ) / 2;
        var mid    = _array[ midIdx ];
        var right  = _array[ rightIdx ];

        // spaghetti median of three algorithm
        // does at most 3 comparisons
        if ( _comp.Compare( left, mid ) > 0 )
        {
            if ( _comp.Compare( mid, right ) > 0 )
            {
                return midIdx;
            }

            return _comp.Compare( left, right ) > 0 ? rightIdx : leftIdx;
        }

        if ( _comp.Compare( left, right ) > 0 )
        {
            return leftIdx;
        }

        return _comp.Compare( mid, right ) > 0 ? rightIdx : midIdx;
    }

    private void Swap( int left, int right )
    {
        ( _array[ left ], _array[ right ] ) = ( _array[ right ], _array[ left ] );
    }
}
