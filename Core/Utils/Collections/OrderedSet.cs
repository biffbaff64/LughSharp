// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Drawing;
using System.Text;

using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Core.Utils.Collections;

[PublicAPI]
public class OrderedSet<T> : ObjectSet< T >
{
    public List< T > Items { get; set; } = new();

    private OrderedSetIterator< T >? _iterator1;
    private OrderedSetIterator< T >? _iterator2;

    public OrderedSet( int initialCapacity, float loadFactor )
        : base( initialCapacity, loadFactor )
    {
        Items = new List< T >( initialCapacity );
    }

    public OrderedSet( int initialCapacity )
        : base( initialCapacity )
    {
        Items = new List< T >( initialCapacity );
    }

    public OrderedSet( OrderedSet< T > set )
        : base( set )
    {
        Items = new List< T >( set.Items );
    }

    public bool Add( T key )
    {
        if ( !base.Add( key ) )
        {
            return false;
        }

        Items.Add( key );

        return true;
    }

    /// <summary>
    /// Sets the key at the specfied index. Returns true if the key was not already
    /// in the set. If this set already contains the key, the existing key's index
    /// is changed if needed and false is returned.
    /// </summary>
    public bool Add( T key, int index )
    {
        if ( !base.Add( key ) )
        {
            var oldIndex = Items.IndexOf( key );

            if ( oldIndex != index )
            {
                Items.Insert( index, Items.RemoveIndex( oldIndex ) );
            }

            return false;
        }

        Items.Insert( index, key );

        return true;
    }

    public void AddAll( OrderedSet< T > set )
    {
        EnsureCapacity( set.Size );

        T[] keys = set.Items.ToArray();

        for ( int i = 0, n = set.Items.Size; i < n; i++ )
        {
            Add( keys[ i ] );
        }
    }

    public bool Remove( T key )
    {
        if ( !base.Remove( key ) )
        {
            return false;
        }

        Items.Remove( key );

        return true;
    }

    public T RemoveIndex( int index )
    {
        T key = Items.RemoveIndex( index );
        base.Remove( key );

        return key;
    }

    /// <summary>
    /// Changes the item <paramref name="before"/> to <paramref name="after"/>
    /// without changing its position in the order.
    /// </summary>
    /// <remarks>
    /// Returns <tt>true</tt> if <paramref name="after"/> has been added to the
    /// <see cref="OrderedSet{T}"/> and <paramref name="before"/> has been removed;
    /// returns <tt>false</tt> if <paramref name="after"/> is already present or
    /// <paramref name="before"/> is not present.
    /// <para>
    /// If you are iterating over an <see cref="OrderedSet{T}"/> and have an index,
    /// you should prefer <see cref="AlterIndex(int, T)"/>, which doesn't need to
    /// search for an index like this does and can be faster.
    /// </para>
    /// </remarks>
    /// <param name="before">An item that must be present for this to succeed.</param>
    /// <param name="after">An item that must not be in this set for this to succeed.</param>
    /// <returns>
    /// <tt>true</tt> if <paramref name="before"/> was removed and <paramref name="after"/>
    /// was added; <tt>false</tt> otherwise.
    /// </returns>
    public bool Alter( T before, T after )
    {
        if ( Contains( after ) )
        {
            return false;
        }

        if ( !base.Remove( before ) )
        {
            return false;
        }

        base.Add( after );
        Items.Set( Items.IndexOf( before ), after );

        return true;
    }

    /// <summary>
    /// Changes the item at the given <paramref name="index"/> in the order to
    /// <paramref name="after"/>, without changing the ordering of other items.
    /// If <paramref name="after"/> is already present, this returns <tt>false</tt>;
    /// it will also return <tt>false</tt> if <paramref name="index"/> is invalid
    /// for the size of this set. Otherwise, it returns <tt>true</tt>.
    /// </summary>
    /// <param name="index">
    /// The index in the order of the item to change; must be non-negative
    /// and less than <see cref="Size"/>.
    /// </param>
    /// <param name="after">The item that will replace the contents at
    /// <paramref name="index"/>; this item must not be present for this to succeed.
    /// </param>
    /// <returns>
    /// <tt>true</tt> if <paramref name="after"/> successfully replaced the contents
    /// at <paramref name="index"/>, <tt>false</tt> otherwise.
    /// </returns>
    public bool AlterIndex( int index, T after )
    {
        if ( ( index < 0 ) || ( index >= Size ) || Contains( after ) )
        {
            return false;
        }

        base.Remove( Items[ index ] );
        base.Add( after );
        Items.Set( index, after );

        return true;
    }

    public void Clear( int maximumCapacity )
    {
        Items.Clear();
        base.Clear( maximumCapacity );
    }

    public void Clear()
    {
        Items.Clear();
        base.Clear();
    }

    public OrderedSetIterator< T > Iterator()
    {
        if ( CollectionsData.AllocateIterators )
        {
            return new OrderedSetIterator( this );
        }

        if ( _iterator1 == null )
        {
            _iterator1 = new OrderedSetIterator( this );
            _iterator2 = new OrderedSetIterator( this );
        }

        if ( !_iterator1.Valid )
        {
            _iterator1.Reset();
            _iterator1.Valid = true;
            _iterator2.Valid = false;

            return _iterator1;
        }

        _iterator2.Reset();
        _iterator2.Valid = true;
        _iterator1.Valid = false;

        return _iterator2;
    }

    public override string ToString()
    {
        if ( Size == 0 )
        {
            return "{}";
        }

        T[] items = this.Items.ToArray();

        var buffer = new StringBuilder( 32 );

        buffer.Append( '{' );
        buffer.Append( items[ 0 ] );

        for ( var i = 1; i < Size; i++ )
        {
            buffer.Append( ", " );
            buffer.Append( items[ i ] );
        }

        buffer.Append( '}' );

        return buffer.ToString();
    }

    internal class OrderedSetIterator<TT>
    {
    }
}
