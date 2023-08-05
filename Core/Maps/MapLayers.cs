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

public class MapLayers : IEnumerable< MapLayer >
{
    private readonly List< MapLayer > _layers = new();

    /// <summary>
    /// Returns the <see cref="MapLayer"/> at the specified index.
    /// </summary>
    public MapLayer Get( int index ) => _layers[ index ];

    /// <summary>
    /// Returns the first layer having the specified name, if one exists, otherwise null
    /// </summary>
    public MapLayer? Get( string name )
    {
        if ( name.Equals( string.Empty ) ) return null;
            
        for ( int i = 0, n = _layers.Count; i < n; i++ )
        {
            if ( name.Equals( _layers[ i ].Name ) )
            {
                return _layers[ i ];
            }
        }

        return null;
    }

    /// <summary>
    /// Get the index of the layer having the specified name, or -1 if no such layer exists.
    /// </summary>
    public int GetIndex( string name )
    {
        MapLayer? layer = Get( name );
            
        return layer != null ? GetIndex( layer ) : -1;
    }

    /// <summary>
    /// Get the index of the layer in the collection, or -1 if no such layer exists.
    /// </summary>
    public int GetIndex( MapLayer layer ) => _layers.IndexOf( layer );

    /// <summary>
    /// Adds a layer to this collection
    /// </summary>
    public void Add( MapLayer layer )
    {
        this._layers.Add( layer );
    }

    public int GetCount() => _layers.Count;

    public void Remove( int index ) => _layers.RemoveAt( index );

    public void Remove( MapLayer layer ) => _layers.Remove( layer );

    public int Size() => _layers.Count;

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List< T > GetByType<T>() where T : MapLayer
    {
        return GetByType( new List< T >() );
    }

    /// <summary>
    /// Returns a list of layers which match the requested type in <b>T</b>
    /// </summary>
    /// <param name="fill">Ther list in which to store the results</param>
    /// <typeparam name="T">The requested type.</typeparam>
    /// <returns></returns>
    public List< T > GetByType<T>( List< T > fill ) where T : MapLayer
    {
        fill.Clear();
        fill.AddRange( _layers.OfType<T>() );

        return fill;
    }

    public IEnumerator< MapLayer > GetEnumerator()
    {
        return _layers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}