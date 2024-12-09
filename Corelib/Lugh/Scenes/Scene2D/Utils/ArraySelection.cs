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

using Corelib.Lugh.Input;

namespace Corelib.Lugh.Scenes.Scene2D.Utils;

/// <summary>
/// A selection that supports range selection by knowing about the
/// array of items being selected.
/// </summary>
[PublicAPI]
public class ArraySelection< T > : Selection< T >
{
    private readonly List< T >? _array;
    private          T?         _rangeStart;

    // ========================================================================

    public ArraySelection( List< T >? array )
    {
        _array      = array;
        _rangeStart = default( T? );
    }

    protected ArraySelection()
    {
    }

    public bool RangeSelect { get; set; } = true;

    /// <inheritdoc />
    public override void Choose( T item )
    {
        ArgumentNullException.ThrowIfNull( item );

        if ( IsDisabled )
        {
            return;
        }

        if ( !RangeSelect || !Multiple )
        {
            base.Choose( item );

            return;
        }

        if ( ( Selected.Count > 0 ) && InputUtils.ShiftKey() )
        {
            var rangeStartIndex = _rangeStart == null ? -1 : _array!.IndexOf( _rangeStart, 0 );

            if ( rangeStartIndex != -1 )
            {
                var oldRangeStart = _rangeStart;
                Snapshot();

                // Select new range.
                var start = rangeStartIndex;
                var end   = _array!.IndexOf( item, 0 );

                if ( start > end )
                {
                    ( end, start ) = ( start, end );
                }

                if ( !InputUtils.CtrlKey() )
                {
                    Selected.Clear(); // Clear( 8 );
                }

                for ( var i = start; i <= end; i++ )
                {
                    Selected.Add( _array[ i ] );
                }

                if ( FireChangeEvent() )
                {
                    Revert();
                }
                else
                {
                    Changed();
                }

                _rangeStart = oldRangeStart;

                Cleanup();

                return;
            }
        }

        base.Choose( item );

        _rangeStart = item;
    }

    /// <summary>
    /// Called after the selection changes, clears the range start item.
    /// </summary>
    protected override void Changed()
    {
        _rangeStart = default( T? );
    }

    /// <summary>
    /// Removes objects from the selection that are no longer in the items
    /// array. If <see cref="Selection{T}.Required"/> is true and there is
    /// no selected item, the first item is selected.
    /// </summary>
    public virtual void Validate()
    {
        if ( _array?.Count == 0 )
        {
            Clear();

            return;
        }

        var changed = false;

        foreach ( var item in Items() )
        {
            if ( !_array!.Contains( item ) )
            {
                Items().Remove( item );
                changed = true;
            }
        }

        if ( Required && ( Selected.Count == 0 ) )
        {
            Set( _array!.First() );
        }
        else if ( changed )
        {
            Changed();
        }
    }
}
