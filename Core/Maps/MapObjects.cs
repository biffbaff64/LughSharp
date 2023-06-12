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

using System.Collections;

namespace LibGDXSharp.Maps;

/// <summary>
/// A Collection of <see cref="MapObject"/> instances.
/// </summary>
public sealed class MapObjects : IEnumerable< MapObject >
{
    private readonly List< MapObject > _objects;

    /// <summary>
    /// Creates an empty set of MapObjects
    /// </summary>
    public MapObjects()
    {
        _objects = new List< MapObject >();
    }

    public MapObject Get( int index )
    {
        return _objects[ index ];
    }

    public MapObject Get( string name )
    {
        for ( int i = 0, n = _objects.Count; i < n; i++ )
        {
            MapObject obj = _objects[ i ];

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

    /// <param name="type"> class of the objects we want to retrieve </param>
    /// <returns> array filled with all the objects in the collection matching type  </returns>
    public List< T > GetByType<T>( T type ) where T : MapObject
    {
        return GetByType( type, new List< T >() );
    }

    /// <param name="type"> class of the objects we want to retrieve </param>
    /// <param name="fill"> collection to put the returned objects in </param>
    /// <returns> array filled with all the objects in the collection matching type  </returns>
    public List< T > GetByType<T>( T type, List< T > fill ) where T : MapObject
    {
        fill.Clear();

        for ( int i = 0, n = _objects.Count; i < n; i++ )
        {
            MapObject obj = _objects[ i ];

            if ( obj.GetType() == typeof(T) )
            {
                fill.Add( ( T )obj );
            }
        }

        return fill;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator{T}"/> object.</returns>
    public IEnumerator< MapObject > GetEnumerator()
    {
        return _objects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}