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

using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Utils.Collections;

/// <summary>
/// An list that queues removal during iteration until the iteration has completed.
/// Queues any removals done after <see cref="Begin()"/> is called to occur once
/// <see cref="End()"/> is called. This can allow code out of your control to remove items
/// without affecting iteration. Between begin and end, most mutator methods will throw
/// IllegalStateException. Only <see cref="RemoveIndex(int)"/>, <see cref="RemoveValue"/>,
/// <see cref="RemoveRange(int, int)"/>, <see cref="Clear()"/>, and add methods are allowed.
/// <para>
/// Note that DelayedRemovalList is not for thread safety, only for removal during iteration.
/// </para>
/// <para>
/// Code using this class must not rely on items being removed immediately. Consider using
/// <see cref="SnapshotArray{T}"/> if this is a problem.
/// </para>
/// </summary>
[PublicAPI]
public class DelayedRemovalList< T > : List< T >
{
    private readonly List< int > _remove = [ ];

    private int _clear;
    private int _iterating;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new DelayedRemovalList from the supplied <see cref="IEnumerable{T}"/>
    /// </summary>
    public DelayedRemovalList( IEnumerable< T > list )
        : base( list )
    {
        Reset();
    }

    /// <summary>
    /// Creates a new DelayedRemovalList from the supplied T list.
    /// </summary>
    public DelayedRemovalList( T[] list )
        : base( list )
    {
        Reset();
    }

    /// <summary>
    /// Creates a new, empty, DelayedRemovalList with the specified initial
    /// capacity. The default capacity is 16.
    /// </summary>
    /// <param name="initialCapacity"></param>
    public DelayedRemovalList( int initialCapacity = 16 )
        : base( initialCapacity )
    {
        Reset();
    }

    /// <summary>
    /// Creates a new DelayedRemovalList from the supplied <see cref="IReadOnlyList{T}"/>.
    /// <paramref name="count"/> elements from the source list will be copied, starting
    /// at <paramref name="startIndex"/>.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="startIndex"></param>
    /// <param name="count"></param>
    public DelayedRemovalList( IReadOnlyList< T > list, int startIndex, int count )
    {
        for ( var i = 0; i < count; i++ )
        {
            Add( list[ startIndex + i ] );
        }

        Reset();
    }

    /// <summary>
    /// Begins the iteration process which delays removal until iteration
    /// has finished.
    /// <para>
    /// See the <see cref="DelayedRemovalList{T}"/> class description for
    /// a fuller description.
    /// </para>
    /// </summary>
    public void Begin()
    {
        _iterating++;
    }

    /// <summary>
    /// Ends the iteration process to allow element removal.
    /// <para>
    /// See the <see cref="DelayedRemovalList{T}"/> class description for
    /// a fuller description.
    /// </para>
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// Thrown if this method is called before calling <see cref="Begin()"/>.
    /// </exception>
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
    /// Removes the item at position <paramref name="index"/> from the list.
    /// </summary>
    /// <param name="index"> The zero-based index of the element to remove. </param>
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
    /// Removes the first occurance of the specified value from the list.
    /// </summary>
    /// <param name="value"> The value to remove. </param>
    /// <returns>
    /// true if item is successfully removed; otherwise, false. This method also
    /// returns false if item was not found in the list.
    /// </returns>
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
    /// <param name="index"> The zero-based index of the element to remove. </param>
    /// <returns></returns>
    public T RemoveIndex( int index )
    {
        if ( _iterating > 0 )
        {
            Remove( index );

            return this[ index ];
        }

        RemoveAt( index );

        return this[ index ];
    }

    /// <summary>
    /// Removes a range of items from the list, beginning at <paramref name="start"/>
    /// and ending at <paramref name="end"/>.
    /// Start index is zero-based.
    /// Positions are inclusive.
    /// </summary>
    /// <param name="start"> Start position. </param>
    /// <param name="end"> End position. </param>
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
    /// Clear this list.
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
    /// Sets the value at the supplied index.
    /// </summary>
    /// <param name="index"> The index. </param>
    /// <param name="value"> The Value. </param>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
    public void Set( int index, T value )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        this[ index ] = value;
    }

    /// <summary>
    /// Inserts an entry into the list at the specified index, and
    /// sets it to the supplied value.
    /// </summary>
    /// <param name="index"> The index. </param>
    /// <param name="value"> The Value. </param>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
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
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
    public void InsertRange( int index, int count )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        var insertItem = base[ index ];

        for ( var i = 0; i < count; i++ )
        {
            base.Insert( index + i, insertItem );
        }
    }

    /// <summary>
    /// Swaps the two values at indexes <paramref name="first"/> and <paramref name="second."/>
    /// </summary>
    /// <param name="first"> List position of the first element. </param>
    /// <param name="second"> List position of the second element. </param>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
    public void Swap( int first, int second )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        ( this[ first ], this[ second ] ) = ( this[ second ], this[ first ] );
    }

    /// <summary>
    /// Removes and returns the last item in this list.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
    public T Pop()
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        var t = base[ ^1 ];

        RemoveAt( Count - 1 );

        return t;
    }

    /// <summary>
    /// Wrapper for <see cref="List{T}.Sort()"/>, which allows for checking that this
    /// was NOT called between <see cref="Begin"/> and <see cref="End"/>.
    /// Sorts the elements in this list. Uses Array.Sort with the provided comparer.
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
    /// <inheritdoc cref="List{T}.Sort()"/>
    public new void Sort()
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        base.Sort();
    }

    /// <summary>
    /// Wrapper for <see cref="List{T}.Sort(IComparer{T})"/>, which allows for
    /// checking that this was NOT called between <see cref="Begin"/> and <see cref="End"/>.
    /// Sorts the elements in this list. Uses Array.Sort with the provided comparer.
    /// </summary>
    /// <param name="comparator">
    /// The <see cref="IComparer{T}"/> implementation to use when comparing elements,
    /// or null to use the default comparer Default.
    /// </param>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
    public new void Sort( IComparer< T > comparator )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        base.Sort( comparator );
    }

    /// <summary>
    /// Reverses the order of the elements in the list. Essentially a
    /// wrapper for <see cref="List{T}.Reverse()"/> with error checking.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
    public new void Reverse()
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        base.Reverse();
    }

    /// <summary>
    /// Shuffles the contents of this list.
    /// </summary>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle.
    /// </exception>
    public void Shuffle()
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        ListExtensions.Shuffle( this );
    }

    /// <summary>
    /// Truncates the list to the specified new size.
    /// </summary>
    /// <param name="newSize"> The new required size. </param>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle,
    /// or the requested new size is invalid.
    /// </exception>
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
    /// Sets the capacity for this list. Essentially a wrapper for
    /// <see cref="List{T}.EnsureCapacity"/> with error checking.
    /// </summary>
    /// <param name="newSize"> The new required capacity. </param>
    /// <returns> The new capacity </returns>
    /// <exception cref="GdxRuntimeException">
    /// If still between the <see cref="Begin"/> and <see cref="End"/> cycle,
    /// or the requested new size is invalid.
    /// </exception>
    public int SetSize( int newSize )
    {
        if ( _iterating > 0 )
        {
            throw new GdxRuntimeException( "Invalid between begin/end." );
        }

        if ( Count >= newSize )
        {
            throw new GdxRuntimeException( $"Invalid new size: {newSize} (current: {Count} )" );
        }

        return EnsureCapacity( newSize );
    }

    /// <summary>
    /// Resets the delayed removal task for this list.
    /// </summary>
    private void Reset()
    {
        _iterating = 0;
        _clear     = 0;
    }
}
