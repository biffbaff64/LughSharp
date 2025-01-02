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
/// A set of <see cref="ITiledMapTile"/> instances used to compose a TiledMapLayer
/// </summary>
[PublicAPI]
public class TiledMapTileSet : IEnumerable< ITiledMapTile >
{
    /// <summary>
    /// The Tileset name.
    /// </summary>
    public string?       Name       { get; set; }
    
    /// <summary>
    /// A Property Set for this tileset. 
    /// </summary>
    /// <inheritdoc cref="MapProperties"/>
    public MapProperties Properties { get; private set; }

    /// <summary>
    /// A Collection holding the individual tiles for this tileset.
    /// </summary>
    private readonly Dictionary< int, ITiledMapTile > _tiles;

    // ========================================================================
    // ========================================================================
    
    /// <summary>
    /// Creates empty tileset
    /// </summary>
    public TiledMapTileSet()
    {
        Name       = string.Empty;
        _tiles     = new Dictionary< int, ITiledMapTile >();
        Properties = new MapProperties();
    }

    /// <summary>
    /// Gets the <see cref="ITiledMapTile"/> that has the given id.
    /// </summary>
    /// <param name="id"> the id of the <see cref="ITiledMapTile"/> to retrieve. </param>
    /// <returns> tile matching ID, null if it doesn't exist  </returns>
    public ITiledMapTile? GetTile( int id )
    {
        return _tiles[ id ];
    }

    /// <summary>
    /// Adds or replaces tile with the given ID.
    /// </summary>
    /// <param name="id"> the id of the <see cref="ITiledMapTile"/> to add or replace. </param>
    /// <param name="tile"> the <see cref="ITiledMapTile"/> to add or replace. </param>
    public void PutTile( int id, ITiledMapTile tile )
    {
        _tiles[ id ] = tile;
    }

    /// <summary>
    /// Removes a tile from this tileset.
    /// </summary>
    /// <param name="id"> The ID of the tile to be removed </param>
    public void RemoveTile( int id )
    {
        _tiles.Remove( id );
    }

    /// <summary>
    /// Returns the size of this TiledMapTileSet, as in the number of tiles.
    /// </summary>
    public int Size()
    {
        return _tiles.Count;
    }

    /// <inheritdoc />
    public IEnumerator< ITiledMapTile > GetEnumerator()
    {
        return _tiles.Values.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
