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

namespace LibGDXSharp.Maps.Tiled;

public class TiledMapTileLayer : MapLayer
{
    /// <summary>
    ///     Creates TiledMap layer
    /// </summary>
    /// <param name="width"> layer width in tiles </param>
    /// <param name="height"> layer height in tiles </param>
    /// <param name="tileWidth"> tile width in pixels </param>
    /// <param name="tileHeight"> tile height in pixels </param>
    public TiledMapTileLayer( int width, int height, int tileWidth, int tileHeight )
    {
        Width      = width;
        Height     = height;
        TileWidth  = tileWidth;
        TileHeight = tileHeight;
        Cells      = new Cell[ width, height ];
    }

    public int       Width      { get; }
    public int       Height     { get; }
    public int       TileWidth  { get; private set; }
    public int       TileHeight { get; private set; }
    public Cell[ , ] Cells      { get; }

    /// <summary>
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <returns> <see cref="Cell"/> at (x, y) </returns>
    public Cell? GetCell( int x, int y )
    {
        if ( ( x < 0 ) || ( x >= Width ) )
        {
            return null;
        }

        if ( ( y < 0 ) || ( y >= Height ) )
        {
            return null;
        }

        return Cells[ x, y ];
    }

    /// <summary>
    ///     Sets the <see cref="Cell"/> at the given coordinates.
    /// </summary>
    /// <param name="x"> X coordinate </param>
    /// <param name="y"> Y coordinate </param>
    /// <param name="cell"> the <see cref="Cell"/> to set at the given coordinates. </param>
    public void SetCell( int x, int y, Cell cell )
    {
        if ( ( x < 0 ) || ( x >= Width ) )
        {
            return;
        }

        if ( ( y < 0 ) || ( y >= Height ) )
        {
            return;
        }

        Cells[ x, y ] = cell;
    }

    /// <summary>
    ///     represents a cell in a TiledLayer: TiledMapTile, flip and rotation properties.
    /// </summary>
    public class Cell
    {
        public const int            ROTATE0   = 0;
        public const int            ROTATE90  = 1;
        public const int            ROTATE180 = 2;
        public const int            ROTATE270 = 3;
        private      bool           _flipHorizontally;
        private      bool           _flipVertically;
        private      int            _rotation;
        private      ITiledMapTile? _tile;

        /// <summary>
        ///     return The tile currently assigned to this cell.
        /// </summary>
        public ITiledMapTile? GetTile() => _tile;

        /// <summary>
        ///     Sets the tile to be used for this cell.
        /// </summary>
        /// <param name="tile"> the <see cref="TiledMapTile"/> to use for this cell. </param>
        /// <returns> this, for method chaining </returns>
        public Cell SetTile( ITiledMapTile tile )
        {
            _tile = tile;

            return this;
        }

        /// <summary>
        ///     Returns whether the tile should be flipped horizontally.
        /// </summary>
        public bool GetFlipHorizontally() => _flipHorizontally;

        /// <summary>
        ///     Sets whether to flip the tile horizontally.
        /// </summary>
        /// <param name="flipHorizontally"> whether or not to flip the tile horizontally. </param>
        /// <returns> this, for method chaining </returns>
        public Cell SetFlipHorizontally( bool flipHorizontally )
        {
            _flipHorizontally = flipHorizontally;

            return this;
        }

        /// <summary>
        ///     Returns whether the tile should be flipped vertically.
        /// </summary>
        public bool GetFlipVertically() => _flipVertically;

        /// <summary>
        ///     Sets whether to flip the tile vertically.
        /// </summary>
        /// <param name="flipVertically"> whether or not this tile should be flipped vertically. </param>
        /// <returns> this, for method chaining </returns>
        public Cell SetFlipVertically( bool flipVertically )
        {
            _flipVertically = flipVertically;

            return this;
        }

        /// <summary>
        ///     Returns the rotation of this cell, in 90 degree increments.
        /// </summary>
        public int GetRotation() => _rotation;

        /// <summary>
        ///     Sets the rotation of this cell, in 90 degree increments.
        /// </summary>
        /// <param name="rotation"> the rotation in 90 degree increments (see ints below). </param>
        /// <returns> this, for method chaining </returns>
        public Cell SetRotation( int rotation )
        {
            _rotation = rotation;

            return this;
        }
    }
}
