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


namespace Corelib.Lugh.Maps;

/// <summary>
/// A Collection of <see cref="MapObject"/> instances.
/// </summary>
[PublicAPI]
public class MapObjects : IEnumerable< MapObject >
{
    private readonly List< MapObject > _objects;

    /// <summary>
    /// Creates an empty set of MapObjects
    /// </summary>
    public MapObjects()
    {
        _objects = new List< MapObject >();
    }

    /// <inheritdoc />
    public IEnumerator< MapObject > GetEnumerator()
    {
        return _objects.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Gets the <see cref="MapObject"/> at the specified index.
    /// </summary>
    public MapObject Get( int index )
    {
        return _objects[ index ];
    }

    /// <summary>
    /// Searches for, and returns, the <see cref="MapObject"/> which has
    /// the same name as the one provided.
    /// </summary>
    public MapObject Get( string name )
    {
        for ( int i = 0, n = _objects.Count; i < n; i++ )
        {
            var obj = _objects[ i ];

            if ( name.Equals( obj.Name ) )
            {
                return obj;
            }
        }

        return null!;
    }

    /// <inheritdoc cref="List{MapObject}.IndexOf(MapObject)"/>
    public int GetIndex( string name )
    {
        return GetIndex( Get( name ) );
    }

    /// <inheritdoc cref="List{MapObject}.IndexOf(MapObject)"/>
    public int GetIndex( MapObject obj )
    {
        return _objects.IndexOf( obj );
    }

    /// <inheritdoc cref="List{MapObject}.Count"/>
    public int GetCount()
    {
        return _objects.Count;
    }

    /// <inheritdoc cref="List{MapObject}.Add"/>
    public virtual void Add( MapObject obj )
    {
        _objects.Add( obj );
    }

    /// <inheritdoc cref="List{MapObject}.RemoveAt"/>
    public void RemoveIndex( int index )
    {
        _objects.RemoveAt( index );
    }

    /// <inheritdoc cref="List{MapObject}.Remove"/>
    public void Remove( MapObject obj )
    {
        _objects.Remove( obj );
    }

    /// <summary>
    /// Returns a List of all <see cref="MapObject"/>s that match the
    /// specified type.
    /// </summary>
    /// <param name="type"> class of the objects we want to retrieve </param>
    /// <returns> array filled with all the objects in the collection matching type  </returns>
    public List< T > GetByType< T >( T type ) where T : MapObject
    {
        var fill = new List< T >();

        for ( int i = 0, n = _objects.Count; i < n; i++ )
        {
            var obj = _objects[ i ];

            if ( obj.GetType() == typeof( T ) )
            {
                fill.Add( ( T ) obj );
            }
        }

        return fill;
    }
}
