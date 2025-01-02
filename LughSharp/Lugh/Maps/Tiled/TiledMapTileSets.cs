// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace LughSharp.Lugh.Maps.Tiled;

/// <summary>
/// A Collection of <see cref="TiledMapTileSet"/> objects.
/// </summary>
[PublicAPI]
public class TiledMapTileSets : IEnumerable< TiledMapTileSet >
{
    private readonly List< TiledMapTileSet > _tilesets;

    /// <summary>
    /// Creates an empty collection of tilesets.
    /// </summary>
    public TiledMapTileSets()
    {
        _tilesets = new List< TiledMapTileSet >();
    }

    /// <summary>
    /// Returns the desired <see cref="TiledMapTileSet"/> from this collection
    /// at the specified index.
    /// </summary>
    public virtual TiledMapTileSet GetTileSet( int index )
    {
        return _tilesets[ index ];
    }

    /// <summary>
    /// Gets the named tileset.
    /// </summary>
    /// <param name="name"> Name of the <see cref="TiledMapTileSet"/> to retrieve.</param>
    /// <returns> tileset with matching name, null if it doesn't exist  </returns>
    public virtual TiledMapTileSet? GetTileSet( string name )
    {
        foreach ( var tileset in _tilesets )
        {
            if ( name.Equals( tileset.Name ) )
            {
                return tileset;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds the specified tileset to the collection.
    /// </summary>
    /// <param name="tileset"> set to be added to the collection </param>
    public virtual void AddTileSet( TiledMapTileSet tileset )
    {
        _tilesets.Add( tileset );
    }

    /// <summary>
    /// Removes tileset at index
    /// </summary>
    /// <param name="index"> index at which to remove a tileset. </param>
    public virtual void RemoveTileSet( int index )
    {
        _tilesets.RemoveAt( index );
    }

    /// <summary>
    /// Removes the specified tileset from the collection.
    /// </summary>
    /// <param name="tileset"> set to be removed </param>
    public virtual void RemoveTileSet( TiledMapTileSet tileset )
    {
        _tilesets.Remove( tileset );
    }

    /// <summary>
    /// Returns the tile matching the specified ID by performing backward iteration
    /// through the collection of tilesets.
    /// <para>
    /// The purpose of backward iteration here is to maintain backwards compatibility with
    /// maps created with earlier versions of a shared tileset. The assumption is that the
    /// tilesets are in order of ascending firstgid, and by backward iterating precedence
    /// for conflicts is given to later tilesets in the list, which are likely to be the
    /// earlier version of any given gid.  
    /// </para>
    /// </summary>
    /// <param name="id"> id of the <see cref="ITiledMapTile"/> to get. </param>
    /// <returns> tile with matching id, null if it doesn't exist  </returns>
    public virtual ITiledMapTile? GetTile( int id )
    {
        for ( var i = _tilesets.Count - 1; i >= 0; i-- )
        {
            var tileset = _tilesets[ i ];
            var tile    = tileset.GetTile( id );

            if ( tile != null )
            {
                return tile;
            }
        }

        return null;
    }

    /// <inheritdoc />
    public virtual IEnumerator< TiledMapTileSet > GetEnumerator()
    {
        return _tilesets.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used
    /// to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
