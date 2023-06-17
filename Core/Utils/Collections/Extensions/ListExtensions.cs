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

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils.Collections.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public static class ListExtensions
{
    public static T[] Resize<T>( this List< T > ts, int newSize )
    {
        var newItems = new T[ newSize ];
            
        Array.Copy( ts.ToArray(), newItems, newSize  );

        // TODO
        
//            if ( newSize < ts. )
            
//            if ( newSize < ts.Capacity )
//            {
//                ts.RemoveRange
//            }
                
//            T[] items    = this.items;
//            T[] newItems = ( T[] )ArrayReflection.newInstance( items.getClass().getComponentType(), newSize );
//            System.arraycopy( items, 0, newItems, 0, Math.min( size, newItems.length ) );
//            this.items = newItems;

        return newItems;
    }

    /// <summary>
    /// </summary>
    /// <param name="t"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List< T > With<T>( T t )
    {
        return new List< T > { t };
    }

    public static void AddAll<T>( this List< T > ts, T[] array, int start, int count )
    {
        for ( var i = start; i < count; i++ )
        {
            ts.Add( array[ i ] );
        }
    }

    public static void AddAll<T>( this List< T > ts, List<T> array, int start, int count )
    {
        for ( var i = start; i < count; i++ )
        {
            ts.Add( array[ i ] );
        }
    }

    public static void AddAll<T>( this List< T > ts, List<T> array )
    {
        foreach ( T tex in array )
        {
            ts.Add( tex );
        }
    }
        
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>( this List< T > ts )
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
    public static void Truncate<T>( this List< T > ts, int newSize )
    {
        if ( ts.Count > newSize )
        {
            ts.RemoveRange( newSize, ts.Count - newSize );
        }
    }
    
    /// <summary>
    /// Removes and returns the last item in the list.
    /// </summary>
    /// <param name="ts"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="IllegalStateException"></exception>
    public static T Pop<T>( this List< T > ts )
    {
        if ( ts.Count == 0 ) throw new IllegalStateException( "Array is empty." );

        T item = ts[ ^1 ];

        ts.RemoveAt( ts.Count - 1 );

        return item;
    }

    /// <summary>
    /// Returns the last item in a list.
    /// </summary>
    /// <param name="ts"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Peek<T>( this List< T > ts )
    {
        return ts[ ^1 ];
    }
    
    /// <summary>
    /// Removes and returns the item at the specified index.
    /// </summary>
    /// <param name="ts"></param>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T RemoveIndex<T>( this List< T > ts, int index )
    {
        if ( index >= ts.Count )
        {
            throw new IndexOutOfRangeException( "index can't be >= size: " + index + " >= " + ts.Count );
        }

        T value = ts[ index ];

        ts.RemoveAt( index );

        return value;
    }
}