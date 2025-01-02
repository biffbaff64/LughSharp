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

using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Utils.Collections;

[PublicAPI]
public static class ListExtensions
{
    public static T[] Resize< T >( this List< T > ts, int newSize )
    {
        var newItems = new T[ newSize ];

        Array.Copy( ts.ToArray(), newItems, newSize );

        return newItems;
    }

    /// <summary>
    /// Returns the element found at a random position
    /// within the list.
    /// </summary>
    /// <param name="list"> This list </param>
    /// <typeparam name="T"> This list type </typeparam>
    /// <returns></returns>
    public static T Random< T >( this List< T > list )
    {
        return list[ MathUtils.Random( list.Count - 1 ) ];
    }

    /// <summary>
    /// Returns a new List of the required type.
    /// </summary>
    public static List< T > New< T >( T t )
    {
        return [ t ];
    }

    /// <summary>
    /// Adds <paramref name="count"/> elements in the source Array, starting at position
    /// <paramref name="start"/> to the target List.
    /// </summary>
    public static void AddAll< T >( this List< T > target, T[] source, int start, int count )
    {
        for ( var i = start; i < count; i++ )
        {
            target.Add( source[ i ] );
        }
    }

    /// <summary>
    /// Adds <paramref name="count"/> elements in the source List, starting at position
    /// <paramref name="start"/> to the target List.
    /// </summary>
    public static void AddAll< T >( this List< T > target, List< T > source, int start, int count )
    {
        for ( var i = start; i < count; i++ )
        {
            target.Add( source[ i ] );
        }
    }

    /// <summary>
    /// Adds all elements in the source List to the target List.
    /// </summary>
    public static void AddAll< T >( this List< T > target, List< T > source )
    {
        foreach ( var tex in source )
        {
            target.Add( tex );
        }
    }

    /// <summary>
    /// Adds all elements in the array 'items' to the target List.
    /// </summary>
    public static void AddAll< T >( this List< T > target, params T[] items )
    {
        foreach ( var item in items )
        {
            target.Add( item );
        }
    }

    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle< T >( this List< T > ts )
    {
        var count = ts.Count;
        var last  = count - 1;

        var random = new Random();

        for ( var i = 0; i < last; ++i )
        {
            var r = random.Next( i, count );

            ( ts[ i ], ts[ r ] ) = ( ts[ r ], ts[ i ] );
        }
    }

    /// <summary>
    /// Reduces the size of the array to the specified size. If the array is
    /// already smaller than the specified size, no action is taken.
    /// </summary>
    public static void Truncate< T >( this List< T > ts, int newSize )
    {
        if ( ts.Count > newSize )
        {
            ts.RemoveRange( newSize, ts.Count - newSize );
        }
    }

    /// <summary>
    /// Removes and returns the last item in the list.
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static T Pop< T >( this List< T > list )
    {
        if ( list.Count == 0 )
        {
            throw new GdxRuntimeException( "List is empty." );
        }

        var item = list[ ^1 ];

        list.RemoveAt( list.Count - 1 );

        return item;
    }

    /// <summary>
    /// Returns the last item in a list.
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Peek< T >( this List< T > list )
    {
        return list[ ^1 ];
    }

    /// <summary>
    /// Removes and returns the item at the specified index.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T RemoveIndex< T >( this List< T > list, int index )
    {
        if ( index >= list.Count )
        {
            throw new IndexOutOfRangeException( $"index can't be >= size: {index} >= {list.Count}" );
        }

        var value = list[ index ];

        list.RemoveAt( index );

        return value;
    }
}
