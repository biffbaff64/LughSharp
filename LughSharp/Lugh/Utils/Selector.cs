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

using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Utils;

/// <summary>
/// This class is for selecting a ranked element (kth ordered statistic) from an
/// unordered list in faster time than sorting the whole array. Typical applications
/// include finding the nearest enemy unit(s), and other operations which are likely
/// to run as often as every x frames. Certain values of k will result in a partial
/// sorting of the Array.
/// The lowest ranking element starts at 1, not 0. 1 = first, 2 = second, 3 = third,
/// etc. Calling with a value of zero will result in a GdxRuntimeException
/// This class uses very minimal extra memory, as it makes no copies of the array.
/// The underlying algorithms used are a naive single-pass for k = min and k =max, and
/// Hoare's quickselect for values in between.
/// </summary>
[PublicAPI]
public class Selector< T >
{
    private       QuickSelect< T >? _quickSelect;
    public static Selector< T >     Instance { get; } = new();

    public T Select( T[] items, IComparer< T > comp, int kthLowest, int size )
    {
        var idx = SelectIndex( items, comp, kthLowest, size );

        return items[ idx ];
    }

    public int SelectIndex( T[] items, IComparer< T > comp, int kthLowest, int size )
    {
        if ( size < 1 )
        {
            throw new GdxRuntimeException( "cannot select from empty array (size < 1)" );
        }

        if ( kthLowest > size )
        {
            throw new GdxRuntimeException
                ( $"Kth rank is larger than size. k: {kthLowest}, size: {size}" );
        }

        int idx;

        // naive partial selection sort almost certain to outperform
        // quickselect where n is min or max
        if ( kthLowest == 1 )
        {
            // find min
            idx = FastMin( items, comp, size );
        }
        else if ( kthLowest == size )
        {
            // find max
            idx = FastMax( items, comp, size );
        }
        else
        {
            // quickselect a better choice for cases of k between min and max
            _quickSelect ??= new QuickSelect< T >();

            idx = _quickSelect.Select( items, comp, kthLowest, size );
        }

        return idx;
    }

    /// <summary>
    /// Faster than <see cref="QuickSelect{T}"/> for n = min
    /// </summary>
    private static int FastMin( T[] items, IComparer< T > comp, int size )
    {
        var lowestIdx = 0;

        for ( var i = 1; i < size; i++ )
        {
            var comparison = comp.Compare( items[ i ], items[ lowestIdx ] );

            if ( comparison < 0 )
            {
                lowestIdx = i;
            }
        }

        return lowestIdx;
    }

    /// <summary>
    /// Faster than <see cref="QuickSelect{T}"/> for n = max
    /// </summary>
    private static int FastMax( T[] items, IComparer< T > comp, int size )
    {
        var highestIdx = 0;

        for ( var i = 1; i < size; i++ )
        {
            var comparison = comp.Compare( items[ i ], items[ highestIdx ] );

            if ( comparison > 0 )
            {
                highestIdx = i;
            }
        }

        return highestIdx;
    }
}
