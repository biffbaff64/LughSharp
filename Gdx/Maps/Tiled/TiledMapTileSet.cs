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

namespace LibGDXSharp.Maps.Tiled;

public class TiledMapTileSet : IEnumerable< ITiledMapTile >
{
    private readonly Dictionary< int, ITiledMapTile > _tiles;

    /// <summary>
    ///     Creates empty tileset
    /// </summary>
    public TiledMapTileSet()
    {
        Name       = string.Empty;
        _tiles     = new Dictionary< int, ITiledMapTile >();
        Properties = new MapProperties();
    }

    public string? Name { get; set; }

    /// <summary>
    /// </summary>
    /// <returns>tileset's properties set.</returns>
    public MapProperties Properties { get; private set; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerator< ITiledMapTile > GetEnumerator() => _tiles.Values.GetEnumerator();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    ///     Gets the <see cref="ITiledMapTile" /> that has the given id.
    /// </summary>
    /// <param name="id"> the id of the <see cref="ITiledMapTile" /> to retrieve. </param>
    /// <returns> tile matching id, null if it doesn't exist  </returns>
    public ITiledMapTile? GetTile( int id ) => _tiles[ id ];

    /// <summary>
    ///     Adds or replaces tile with that id
    /// </summary>
    /// <param name="id"> the id of the <see cref="ITiledMapTile" /> to add or replace. </param>
    /// <param name="tile"> the <see cref="ITiledMapTile" /> to add or replace. </param>
    public void PutTile( int id, ITiledMapTile tile ) => _tiles[ id ] = tile;

    /// <summary>
    /// </summary>
    /// <param name="id"> tile's id to be removed </param>
    public void RemoveTile( int id ) => _tiles.Remove( id );

    /// <summary>
    /// </summary>
    /// <returns> the size of this TiledMapTileSet. </returns>
    public int Size() => _tiles.Count;
}
