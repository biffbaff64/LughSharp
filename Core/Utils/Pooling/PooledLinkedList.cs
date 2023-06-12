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

using System.Diagnostics;

namespace LibGDXSharp.Utils.Pooling;

/// <summary>
/// A simple linked list that pools its nodes.
/// </summary>
public class PooledLinkedList<T>
{
    internal sealed record Item<TT>
    {
        internal TT?         payload;
        internal Item< TT >? next;
        internal Item< TT >? prev;
    }

    public int Size { get; set; } = 0;

    private Item< T >? _head;
    private Item< T >? _tail;
    private Item< T >? _iter;
    private Item< T >? _curr;

    private readonly Pool< Item< T > > _pool;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxPoolSize"></param>
    public PooledLinkedList( int maxPoolSize )
    {
        this._pool = new Pool< Item< T > >( 16, maxPoolSize )
        {
            NewObject = NewObjectImplementation
        };
    }

    /// <summary>
    /// Adds the specified object to the end of the list regardless of iteration status
    /// </summary>
    public void Add( T obj )
    {
        Item< T > item = _pool.Obtain();

        item.payload = obj;
        item.next    = null;
        item.prev    = null;

        if ( _head == null )
        {
            _head = item;
            _tail = item;

            Size++;

            return;
        }

        item.prev = _tail;

        _tail      = item;
        _tail.next = item;

        Size++;
    }

    /// <summary>
    /// Adds the specified object to the head of the list regardless of iteration status
    /// </summary>
    public void AddFirst( T obj )
    {
        Item< T > item = _pool.Obtain();

        item.payload = obj;
        item.next    = _head;
        item.prev    = null;

        if ( _head != null )
        {
            _head.prev = item;
        }
        else
        {
            _tail = item;
        }

        _head = item;

        Size++;
    }

    /// <summary>
    /// Starts iterating over the list's items from the head of the list
    /// </summary>
    protected void Iter()
    {
        _iter = _head;
    }

    /// <summary>
    /// Starts iterating over the list's items from the tail of the list
    /// </summary>
    public void IterReverse()
    {
        _iter = _tail;
    }

    /// <summary>
    /// Gets the next item in the list
    /// </summary>
    /// <returns> the next item in the list or null if there are no more items</returns>
    protected T? Next()
    {
        if ( _iter == null ) return default;

        T? payload = _iter.payload;

        _curr = _iter;
        _iter = _iter.next;

        return payload;
    }

    /// <summary>
    /// Gets the previous item in the list
    /// </summary>
    /// <returns> the previous item in the list or null if there are no more items  </returns>
    public T? Previous()
    {
        if ( _iter == null ) return default;

        T? payload = _iter.payload;

        _curr = _iter;
        _iter = _iter.prev;

        return payload;
    }

    /// <summary>
    /// Removes the current list item based on the iterator position.
    /// </summary>
    protected void Remove()
    {
        if ( _curr == null ) return;

        Size--;

        Item< T >? c = _curr;
        Item< T >? n = _curr.next;
        Item< T >? p = _curr.prev;

        Debug.Assert( c != null, nameof( c ) + " != null" );
        Debug.Assert( n != null, nameof( n ) + " != null" );
        Debug.Assert( p != null, nameof( p ) + " != null" );

        _pool.Free( _curr );
        _curr = null;

        if ( Size == 0 )
        {
            _head = null;
            _tail = null;

            return;
        }

        if ( c == _head )
        {
            n.prev = null;
            _head  = n;

            return;
        }

        if ( c == _tail )
        {
            p.next = null;
            _tail  = p;

            return;
        }

        p.next = n;
        n.prev = p;
    }

    /// <summary>
    /// Removes the tail of the list regardless of iteration status
    /// </summary>
    public T? RemoveLast()
    {
        if ( _tail == null ) return default;

        T? payload = _tail.payload;

        Size--;

        Item< T >? p = _tail.prev;

        _pool.Free( _tail );

        if ( Size == 0 )
        {
            _head = null;
            _tail = null;
        }
        else
        {
            _tail = p;

            if ( _tail != null )
            {
                _tail.next = null;
            }
        }

        return payload;
    }

    /// <summary>
    /// </summary>
    public void Clear()
    {
        Iter();

        while ( Next() != null )
        {
            Remove();
        }
    }

    // ------------------------------------------------

    private static Item< T > NewObjectImplementation()
    {
        return new Item< T >();
    }
}