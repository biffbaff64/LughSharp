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

namespace LibGDXSharp.Utils;

/// <summary>
/// Implementation of Tony Hoare's quickselect algorithm. Running time is generally O(n),
/// but worst case is O(n^2) Pivot choice is median of three method, providing better
/// performance than a random pivot for partially sorted data.
/// http://en.wikipedia.org/wiki/Quickselect
/// </summary>
[PublicAPI]
public class QuickSelect<T>
{
    private T[]            _array   = null!;
    private IComparer< T > _comp    = null!;

    public int Select( T[] items, IComparer< T > comp, int n, int size )
    {
        this._array = items;
        this._comp  = comp;

        return RecursiveSelect( 0, size - 1, n );
    }

    private int Partition( int left, int right, int pivot )
    {
        T pivotValue = _array[ pivot ];

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
        T   left   = _array[ leftIdx ];
        var midIdx = ( leftIdx + rightIdx ) / 2;
        T   mid    = _array[ midIdx ];
        T   right  = _array[ rightIdx ];

        // spaghetti median of three algorithm
        // does at most 3 comparisons
        if ( _comp.Compare( left, mid ) > 0 )
        {
            if ( _comp.Compare( mid, right ) > 0 )
            {
                return midIdx;
            }

            return ( _comp.Compare( left, right ) > 0 ) ? rightIdx : leftIdx;
        }

        if ( _comp.Compare( left, right ) > 0 )
        {
            return leftIdx;
        }
        
        return ( _comp.Compare( mid, right ) > 0 ) ? rightIdx : midIdx;
    }

    private void Swap( int left, int right )
    {
        ( _array[ left ], _array[ right ] ) = ( _array[ right ], _array[ left ] );
    }
}
