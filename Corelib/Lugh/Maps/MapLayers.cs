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
/// Ordered list of <see cref="MapLayer"/> instances owned by a <see cref="Map"/>.
/// </summary>
[PublicAPI]
public class MapLayers : IEnumerable< MapLayer >
{
    private readonly List< MapLayer > _layers = new();

    /// <inheritdoc />
    public IEnumerator< MapLayer > GetEnumerator()
    {
        return _layers.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns the <see cref="MapLayer"/> at the specified index.
    /// </summary>
    public MapLayer Get( int index )
    {
        return _layers[ index ];
    }

    /// <summary>
    /// Returns the first layer having the specified name, if one exists, otherwise null
    /// </summary>
    public MapLayer? Get( string name )
    {
        if ( name.Equals( string.Empty ) )
        {
            return null;
        }

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
        var layer = Get( name );

        return layer != null ? GetIndex( layer ) : -1;
    }

    /// <summary>
    /// Get the index of the layer in the collection, or -1 if no such layer exists.
    /// </summary>
    public int GetIndex( MapLayer layer )
    {
        return _layers.IndexOf( layer );
    }

    /// <summary>
    /// Adds a layer to this collection
    /// </summary>
    public virtual void Add( MapLayer layer )
    {
        _layers.Add( layer );
    }

    /// <summary>
    /// Removes the <see cref="MapLayer"/> at the specified index.
    /// </summary>
    public virtual void Remove( int index )
    {
        _layers.RemoveAt( index );
    }

    /// <summary>
    /// Removes the requested <see cref="MapLayer"/>
    /// </summary>
    public virtual void Remove( MapLayer layer )
    {
        _layers.Remove( layer );
    }

    /// <summary>
    /// Returns the number of layers.
    /// </summary>
    public int Size()
    {
        return _layers.Count;
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List< T > GetByType< T >() where T : MapLayer
    {
        return GetByType( new List< T >() );
    }

    /// <summary>
    /// Returns a list of layers which match the requested type in <b>T</b>
    /// </summary>
    /// <param name="fill">The list in which to store the results</param>
    /// <typeparam name="T">The requested type.</typeparam>
    /// <returns></returns>
    public List< T > GetByType< T >( List< T > fill ) where T : MapLayer
    {
        fill.Clear();
        fill.AddRange( _layers.OfType< T >() );

        return fill;
    }
}
