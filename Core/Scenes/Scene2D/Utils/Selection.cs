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

using LibGDXSharp.Core.Utils.Collections.Extensions;
using LibGDXSharp.Utils.Collections.Extensions;
using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D.Utils;

/// <summary>
/// Manages selected objects. Optionally fires a <see cref="ChangeListener.ChangeEvent"/> on an actor.
/// Selection changes can be vetoed via <see cref="ChangeListener.ChangeEvent.Cancel()"/>.
/// </summary>
/// <seealso cref="SortedSet{T}"/>
public class Selection<T> : IDisableable
{
    public SortedSet< T > Selected                 { get; set; } = new();
    public bool           IsDisabled               { get; set; }
    public bool           Multiple                 { get; set; }
    public bool           Required                 { get; set; }
    public T?             LastSelected             { get; set; }
    public bool           Toggle                   { get; set; }
    public bool           ProgrammaticChangeEvents { get; set; } = true;

    private readonly SortedSet< T > _old = new();

    /// <summary>
    /// <param name="value">
    /// An actor to fire a <see cref="ChangeListener.ChangeEvent"/> on when the
    /// selection changes, or null.
    /// </param>
    /// </summary>
    public Actor? Actor { get; set; }

    /// <summary>
    /// Selects or deselects the specified item based on how the selection is
    /// configured, whether ctrl is currently pressed, etc.
    /// <para>
    /// This is typically invoked by user interaction. 
    /// </para>
    /// </summary>
    public void Choose( T item )
    {
        ArgumentNullException.ThrowIfNull( item );

        if ( IsDisabled ) return;

        Snapshot();

        try
        {
            if ( ( Toggle || UIUtils.Ctrl() ) && Selected.Contains( item ) )
            {
                if ( Required && ( Selected.Count == 1 ) ) return;

                Selected.Remove( item );
                LastSelected = default;
            }
            else
            {
                var modified = false;

                if ( !Multiple || ( !Toggle && !UIUtils.Ctrl() ) )
                {
                    if ( ( Selected.Count == 1 ) && Selected.Contains( item ) )
                    {
                        return;
                    }

                    modified = Selected.Count > 0;
                    Selected.Clear( 8 );
                }

                if ( !Selected.Add( item ) && !modified )
                {
                    return;
                }

                LastSelected = item;
            }

            if ( FireChangeEvent() )
            {
                Revert();
            }
            else
            {
                Changed();
            }
        }
        finally
        {
            Cleanup();
        }
    }

    /// <summary>
    /// Java LibGDX has this deprecated method. I have left it here for
    /// those coming from java LibGDX who may wonder what happened to it. 
    /// I am, however, tempted to 'un-obsolete' this as I like the clarity
    /// of the method name.
    /// </summary>
    [Obsolete( "Use NotEmpty() instead.", true )]
    public bool HasItems()
    {
        return Selected.Count > 0;
    }

    /// <summary>
    /// Returns TRUE if this set has items in it.
    /// </summary>
    public bool NotEmpty()
    {
        return !Empty;
    }

    public bool Empty => Selected.Count == 0;

    public int Size() => Selected.Count;

    public SortedSet< T > Items()
    {
        return Selected;
    }

    /// <summary>
    /// Returns the first selected item, or null.
    /// </summary>
    public T? First()
    {
        return Selected.Count == 0 ? default : Selected.First();
    }

    public void Snapshot()
    {
        _old.Clear( Selected.Count );
        _old.AddAll( Selected );
    }

    public void Revert()
    {
        Selected.Clear( _old.Count );
        Selected.AddAll( _old );
    }

    public void Cleanup()
    {
        _old.Clear( 32 );
    }

    /// <summary>
    /// Sets the selection to only the specified item.
    /// </summary>
    public void Set( T item )
    {
        ArgumentNullException.ThrowIfNull( item );

        if ( ( Selected.Count == 1 ) && ( Equals( Selected.First(), item ) ) ) return;

        Snapshot();
        Selected.Clear( 8 );
        Selected.Add( item );

        if ( ProgrammaticChangeEvents && FireChangeEvent() )
        {
            Revert();
        }
        else
        {
            LastSelected = item;
            Changed();
        }

        Cleanup();
    }

    public void SetAll( List< T > items )
    {
        var added = false;

        Snapshot();

        LastSelected = default;

        Selected.Clear( items.Count );

        for ( int i = 0, n = items.Count; i < n; i++ )
        {
            T item = items[ i ];

            if ( item == null ) throw new ArgumentException( "item cannot be null." );
            
            if ( Selected.Add( item ) ) added = true;
        }

        if ( added )
        {
            if ( ProgrammaticChangeEvents && FireChangeEvent() )
            {
                Revert();
            }
            else if ( items.Count > 0 )
            {
                LastSelected = items.Peek();
                Changed();
            }
        }

        Cleanup();
    }

    /// <summary>
    /// Adds the item to the selection.
    /// </summary>
    public void Add( T item )
    {
        if ( item == null ) throw new ArgumentException( "item cannot be null." );

        if ( !Selected.Add( item ) ) return;

        if ( ProgrammaticChangeEvents && FireChangeEvent() )
        {
            Selected.Remove( item );
        }
        else
        {
            LastSelected = item;
            Changed();
        }
    }

    /// <summary>
    /// Adds all items from the supplied list to the selection.
    /// </summary>
    public void AddAll( List< T > items )
    {
        var added = false;

        Snapshot();

        for ( int i = 0, n = items.Count; i < n; i++ )
        {
            T item = items[ i ];

            if ( item == null ) throw new ArgumentException( "item cannot be null." );

            if ( Selected.Add( item ) ) added = true;
        }

        if ( added )
        {
            if ( ProgrammaticChangeEvents && FireChangeEvent() )
            {
                Revert();
            }
            else
            {
                LastSelected = items.Peek();
                Changed();
            }
        }

        Cleanup();
    }

    public void Remove( T item )
    {
        if ( item == null ) throw new ArgumentException( "item cannot be null." );

        if ( !Selected.Remove( item ) ) return;

        if ( ProgrammaticChangeEvents && FireChangeEvent() )
        {
            Selected.Add( item );
        }
        else
        {
            LastSelected = default;
            Changed();
        }
    }

    public void RemoveAll( List< T > items )
    {
        var removed = false;

        Snapshot();

        for ( int i = 0, n = items.Count; i < n; i++ )
        {
            T item = items[ i ];

            if ( item == null ) throw new ArgumentException( "item cannot be null." );

            if ( Selected.Remove( item ) ) removed = true;
        }

        if ( removed )
        {
            if ( ProgrammaticChangeEvents && FireChangeEvent() )
            {
                Revert();
            }
            else
            {
                LastSelected = default;
                Changed();
            }
        }

        Cleanup();
    }

    public void Clear()
    {
        if ( Selected.Count == 0 ) return;

        Snapshot();

        Selected.Clear( 8 );

        if ( ProgrammaticChangeEvents && FireChangeEvent() )
        {
            Revert();
        }
        else
        {
            LastSelected = default;
            Changed();
        }

        Cleanup();
    }

    /// <summary>
    /// Called after the selection changes. The default implementation does nothing.
    /// </summary>
    protected void Changed()
    {
    }

    /// <summary>
    /// Fires a change event on the selection's actor, if any. Called internally when
    /// the selection changes, depending on <see cref="ProgrammaticChangeEvents"/>.
    /// </summary>
	/// <returns> true if the change should be undone. </returns>
    public bool FireChangeEvent()
    {
        if ( Actor == null ) return false;

        ChangeListener.ChangeEvent changeEvent = Pools< ChangeListener.ChangeEvent >.Obtain();

        try
        {
            return Actor.Fire( changeEvent );
        }
        finally
        {
            Pools< ChangeListener.ChangeEvent >.Free( changeEvent );
        }
    }

    public bool Contains( T? item )
    {
        if ( item == null ) return false;

        return Selected.Contains( item );
    }

    /// <summary>
    /// Makes a best effort to return the last item selected, else returns an
    /// arbitrary item or null if the selection is empty.
    /// </summary>
    public T? GetLastSelected()
    {
        if ( LastSelected != null ) return LastSelected;

        if ( Selected.Count > 0 ) return Selected.First();

        return default;
    }

    public IEnumerator< T > Iterator() => Selected.GetEnumerator();

    public List< T > ToArray() => Selected.ToList();

    public List< T > ToArray( List< T > array )
    {
        List< T > list = Selected.ToList();

        list.AddRange( array );

        return list;
    }

    public new string? ToString() => Selected.ToString();
}