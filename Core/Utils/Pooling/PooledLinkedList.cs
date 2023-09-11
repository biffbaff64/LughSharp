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

namespace LibGDXSharp.Utils.Pooling;

/// <summary>
/// A simple linked list that pools its nodes.
/// </summary>
[PublicAPI]
public class PooledLinkedList<T>
{
    public record Item<TT>
    {
        internal TT?         Payload { get; set; }
        internal Item< TT >? Next    { get; set; }
        internal Item< TT >? Prev    { get; set; }
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
            newObject = GetNewObject
        };
    }

    /// <summary>
    /// Adds the specified object to the end of the list regardless of iteration status
    /// </summary>
    public void Add( T obj )
    {
        Item< T >? item;

        if ( ( item = _pool.Obtain() ) == null )
        {
            return;
        }

        item.Payload = obj;
        item.Next    = null;
        item.Prev    = null;

        if ( _head == null )
        {
            _head = item;
            _tail = item;

            Size++;

            return;
        }

        item.Prev = _tail;

        _tail      = item;
        _tail.Next = item;

        Size++;
    }

    /// <summary>
    /// Adds the specified object to the head of the list regardless of iteration status
    /// </summary>
    public void AddFirst( T obj )
    {
        Item< T >? item;

        if ( ( item = _pool.Obtain() ) == null )
        {
            return;
        }

        item.Payload = obj;
        item.Next    = _head;
        item.Prev    = null;

        if ( _head != null )
        {
            _head.Prev = item;
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
        if ( _iter == null )
        {
            return default( T? );
        }

        T? payload = _iter.Payload;

        _curr = _iter;
        _iter = _iter.Next;

        return payload;
    }

    /// <summary>
    /// Gets the previous item in the list
    /// </summary>
    /// <returns> the previous item in the list or null if there are no more items </returns>
    public T? Previous()
    {
        if ( _iter == null )
        {
            return default( T? );
        }

        T? payload = _iter.Payload;

        _curr = _iter;
        _iter = _iter.Prev;

        return payload;
    }

    /// <summary>
    /// Removes the current list item based on the iterator position.
    /// </summary>
    protected void Remove()
    {
        if ( _curr == null )
        {
            return;
        }

        Size--;

        Item< T >? c = _curr;
        Item< T >? n = _curr.Next;
        Item< T >? p = _curr.Prev;

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
            n.Prev = null;
            _head  = n;

            return;
        }

        if ( c == _tail )
        {
            p.Next = null;
            _tail  = p;

            return;
        }

        p.Next = n;
        n.Prev = p;
    }

    /// <summary>
    /// Removes the tail of the list regardless of iteration status
    /// </summary>
    public T? RemoveLast()
    {
        if ( _tail == null )
        {
            return default( T? );
        }

        T? payload = _tail.Payload;

        Size--;

        Item< T >? p = _tail.Prev;

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
                _tail.Next = null;
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

    public Item< T > GetNewObject()
    {
        return new Item< T >();
    }
}
