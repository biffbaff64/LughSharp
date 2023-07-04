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

namespace LibGDXSharp.Utils.Collections;

/// <summary>
/// An array that allows modification during iteration. Guarantees that
/// array entries provided by begin() between indexes 0 and size at the
/// time begin was called will not be modified until end() is called. If
/// modification of the SnapshotArray occurs between begin/end, the backing
/// array is copied prior to the modification, ensuring that the backing
/// array that was returned by begin() is unaffected. To avoid allocation,
/// an attempt is made to reuse any extra array created as a result of this
/// copy on subsequent copies.
/// <para>
/// Note that SnapshotArray is not for thread safety, only for modification
/// during iteration.
/// </para>
/// </summary>
/// <typeparam name="T"></typeparam>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class SnapshotArray<T> : Array< T >
{
    private T[]? _snapshot;
    private T[]? _recycled;
    private int  _snapshotCount;

    public SnapshotArray( int capacity = 0 ) : base( capacity: capacity )
    {
    }

    public SnapshotArray( Array< T > array ) : base( array )
    {
    }

    public SnapshotArray( bool ordered, int capacity )
        : base( ordered, capacity )
    {
    }

    public SnapshotArray( bool ordered, T[] array, int startIndex, int count )
        : base( ordered, array, startIndex, count )
    {
    }

    /// <summary>
    /// Takes a snapshot of the current array state and then
    /// returns the array. 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public T?[] Begin()
    {
        Modified();

        _snapshot = ToArray();

        _snapshotCount++;

        return ToArray();
    }

    /// <summary>
    /// </summary>
    public void End()
    {
        _snapshotCount = Math.Max( 0, _snapshotCount - 1 );

        if ( ( _snapshot != base.ToArray() ) && ( _snapshotCount == 0 ) )
        {
            // The backing array was copied, keep around the old array.
            _recycled = _snapshot;

            if ( _recycled != null )
            {
                for ( int i = 0, n = _recycled.Length; i < n; i++ )
                {
                    _recycled[ i ] = default!;
                }
            }
        }

        _snapshot = null!;
    }

    /// <summary>
    /// </summary>
    public void Modified()
    {
        if ( _snapshot != base.ToArray() ) return;

        // Snapshot is in use, copy backing array to recycled
        // array or create new backing array.
        if ( _recycled?.Length >= Size )
        {
            // Copy the contents of items[] to recycled
            for ( var i = 0; i < Size; i++ )
            {
                _recycled[ i ] = Items[ i ];
            }

            // 'recycled' now references nothing 
            _recycled = default!;
        }
        else
        {
            this.Resize( base.Size );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public new void Set( int index, T value )
    {
        Modified();

        base.Set( index, value );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public new void Insert( int index, T value )
    {
        Modified();
        base.Insert( index, value );
    }

    public new void Swap( int first, int second )
    {
        Modified();

        ( Items[ first ], Items[ second ] ) = ( Items[ second ], Items[ first ] );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Remove( T value )
    {
        Modified();

        return base.RemoveValue( value );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T RemoveAt( int index )
    {
        Modified();

        return this.RemoveIndex( index );
    }

    /// <summary>
    /// Removes a range of elements from the array.
    /// </summary>
    /// <param name="start">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="count">The number of elements to remove.</param>
    public new void RemoveRange( int start, int count )
    {
        Modified();
        base.RemoveRange( start, count );
    }

    /// <summary>
    /// Removes all the elements that match the conditions defined by the
    /// specified predicate.
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public bool RemoveAll<TT>( TT array ) where TT : Array<T>
    {
        Modified();

        return base.RemoveAll( array );
    }
}