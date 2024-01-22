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

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
///     A selection that supports range selection by knowing about the
///     array of items being selected.
/// </summary>
[PublicAPI]
public class ArraySelection<T> : Selection< T >
{
    private readonly List< T >? _array;
    private          T?         _rangeStart;

    public ArraySelection( List< T >? array )
    {
        _array      = array;
        _rangeStart = default( T? );
    }

    protected ArraySelection()
    {
    }

    public bool RangeSelect { get; set; } = true;

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

        if ( ( Selected.Count > 0 ) && UIUtils.Shift() )
        {
            var rangeStartIndex = _rangeStart == null ? -1 : _array!.IndexOf( _rangeStart, 0 );

            if ( rangeStartIndex != -1 )
            {
                T? oldRangeStart = _rangeStart;
                Snapshot();

                // Select new range.
                var start = rangeStartIndex;
                var end   = _array!.IndexOf( item, 0 );

                if ( start > end )
                {
                    ( end, start ) = ( start, end );
                }

                if ( !UIUtils.Ctrl() )
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
    ///     Called after the selection changes, clears the range start item.
    /// </summary>
    protected new void Changed() => _rangeStart = default( T? );

    /// <summary>
    ///     Removes objects from the selection that are no longer in the items
    ///     array. If <see cref="Selection{T}.Required" /> is true and there is
    ///     no selected item, the first item is selected.
    /// </summary>
    public void Validate()
    {
        if ( this._array?.Count == 0 )
        {
            Clear();

            return;
        }

        var changed = false;

        foreach ( T item in Items() )
        {
            if ( !this._array!.Contains( item ) )
            {
                Items().Remove( item );
                changed = true;
            }
        }

        if ( Required && ( Selected.Count == 0 ) )
        {
            Set( this._array!.First() );
        }
        else if ( changed )
        {
            Changed();
        }
    }
}
