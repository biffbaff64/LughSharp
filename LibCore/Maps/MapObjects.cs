// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using System.Collections;

namespace LughSharp.LibCore.Maps;

/// <summary>
///     A Collection of <see cref="MapObject" /> instances.
/// </summary>
public class MapObjects : IEnumerable< MapObject >
{
    private readonly List< MapObject > _objects;

    /// <summary>
    ///     Creates an empty set of MapObjects
    /// </summary>
    public MapObjects()
    {
        _objects = new List< MapObject >();
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator{T}" /> object.</returns>
    public IEnumerator< MapObject > GetEnumerator()
    {
        return _objects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public MapObject Get( int index )
    {
        return _objects[ index ];
    }

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

    public int GetIndex( string name )
    {
        return GetIndex( Get( name ) );
    }

    public int GetIndex( MapObject obj )
    {
        return _objects.IndexOf( obj );
    }

    public int GetCount()
    {
        return _objects.Count;
    }

    public void Add( MapObject obj )
    {
        _objects.Add( obj );
    }

    public void RemoveIndex( int index )
    {
        _objects.RemoveAt( index );
    }

    public void Remove( MapObject obj )
    {
        _objects.Remove( obj );
    }

    /// <summary>
    /// </summary>
    /// <param name="type"> class of the objects we want to retrieve </param>
    /// <returns> array filled with all the objects in the collection matching type  </returns>
    public List< T > GetByType< T >( T type ) where T : MapObject
    {
        return GetByType( type, new List< T >() );
    }

    /// <summary>
    /// </summary>
    /// <param name="type"> class of the objects we want to retrieve </param>
    /// <param name="fill"> collection to put the returned objects in </param>
    /// <returns> array filled with all the objects in the collection matching type  </returns>
    public List< T > GetByType< T >( T type, List< T > fill ) where T : MapObject
    {
        fill.Clear();

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