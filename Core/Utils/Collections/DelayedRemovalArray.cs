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

using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Utils;

[PublicAPI]
public class DelayedRemovalArray<T> : List< T >
{
    private int _iterating = 0;
    private int _clear     = 0;

    private readonly List< int > _remove = new();

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    public DelayedRemovalArray( IEnumerable< T > array )
        : base( array )
    {
        Reset();
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    public DelayedRemovalArray( T[] array )
        : base( array )
    {
        Reset();
    }

    /// <summary>
    /// </summary>
    /// <param name="array"></param>
    /// <param name="startIndex"></param>
    /// <param name="count"></param>
    public DelayedRemovalArray( IReadOnlyList< T > array, int startIndex, int count )
    {
        for ( var i = 0; i < count; i++ )
        {
            Add( array[ startIndex + i ] );
        }

        Reset();
    }

    /// <summary>
    /// </summary>
    /// <param name="initialCapacity"></param>
    public DelayedRemovalArray( int initialCapacity = 16 )
        : base( initialCapacity )
    {
        Reset();
    }

    /// <summary>
    /// </summary>
    public void Begin()
    {
        _iterating++;
    }

    /// <summary>
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public void End()
    {
        if ( _iterating == 0 )
        {
            throw new GdxRuntimeException( "Begin() must be called before End()!" );
        }

        _iterating--;

        if ( _iterating == 0 )
        {
            if ( ( _clear > 0 ) && ( _clear == Count ) )
            {
                _remove.Clear();
                Clear();
            }
            else
            {
                for ( int i = 0, n = _remove.Count; i < n; i++ )
                {
                    var index = _remove.Pop();

                    if ( index >= _clear )
                    {
                        RemoveAt( index );
                    }
                }

                for ( var i = _clear - 1; i >= 0; i-- )
                {
                    RemoveAt( i );
                }
            }

            _clear = 0;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    public void Remove( int index )
    {
        if ( index < _clear )
        {
            return;
        }

        for ( int i = 0, n = _remove.Count; i < n; i++ )
        {
            var removeIndex = _remove[ i ];

            if ( index == removeIndex )
            {
                return;
            }

            if ( index < removeIndex )
            {
                _remove.Insert( i, index );

                return;
            }
        }

        _remove.Add( index );
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool RemoveValue( T value )
    {
        if ( _iterating > 0 )
        {
            var index = IndexOf( value );

            if ( index == -1 )
            {
                return false;
            }

            Remove( index );

            return true;
        }

        return base.Remove( value );
    }

    /// <summary>
    /// Removes the element at the specified index of the List.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <returns></returns>
    public T RemoveIndex( int index )
    {
        if ( _iterating > 0 )
        {
            Remove( index );

            return this[ index ];
        }

        base.RemoveAt( index );

        return this[ index ];
    }

    /// <summary>
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public new void RemoveRange( int start, int end )
    {
        if ( _iterating > 0 )
        {
            for ( var i = end; i >= start; i-- )
            {
                Remove( i );
            }
        }
        else
        {
            base.RemoveRange( start, end );
        }
    }

    /// <summary>
    /// </summary>
    public new void Clear()
    {
        if ( _iterating > 0 )
        {
            _clear = Count;

            return;
        }

        base.Clear();
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public void Set( int index, T value )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        this[ index ] = value;
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public new void Insert( int index, T value )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        base.Insert( index, value );
    }

    /// <summary>
    /// Inserts the specified number of items at the specified index.
    /// The new items will have values equal to the values at those indices
    /// before the insertion.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public void InsertRange( int index, int count )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        T insertItem = base[ index ];

        for ( var i = 0; i < count; i++ )
        {
            base.Insert( index + i, insertItem );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public void Swap( int first, int second )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        ( this[ first ], this[ second ] ) = ( this[ second ], this[ first ] );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public T Pop()
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        T t = base[ ^1 ];

        base.RemoveAt( Count - 1 );

        return t;
    }

    /// <summary>
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public new void Sort()
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        base.Sort();
    }

    /// <summary>
    /// </summary>
    /// <param name="comparator"></param>
    public new void Sort( IComparer< T > comparator )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        base.Sort( comparator );
    }

    /// <summary>
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public new void Reverse()
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        base.Reverse();
    }

    /// <summary>
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    public void Shuffle()
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        ListExtensions.Shuffle( this );
    }

    /// <summary>
    /// </summary>
    /// <param name="newSize"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public void Truncate( int newSize )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        if ( newSize < 0 )
        {
            throw new GdxRuntimeException( "New size must be >= 0: {newSize}" );
        }

        if ( Count < newSize )
        {
            return;
        }

        if ( newSize < Count )
        {
            base.RemoveRange( newSize + 1, Count - newSize );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns>The new capacity</returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public int SetSize( int newSize )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        if ( base.Count >= newSize )
        {
            throw new GdxRuntimeException( $"Invalid new size: {newSize} (current: {Count} )" );
        }

        return base.EnsureCapacity( newSize );
    }

    /// <summary>
    /// </summary>
    private void Reset()
    {
        _iterating = 0;
        _clear     = 0;
    }
}
