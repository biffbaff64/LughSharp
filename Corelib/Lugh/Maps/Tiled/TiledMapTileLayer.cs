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


namespace Corelib.Lugh.Maps.Tiled;

[PublicAPI]
public class TiledMapTileLayer : MapLayer
{
    public int       Width      { get; }
    public int       Height     { get; }
    public int       TileWidth  { get; private set; }
    public int       TileHeight { get; private set; }
    public Cell[ , ] Cells      { get; }

    // ========================================================================

    /// <summary>
    /// Creates TiledMap layer
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

    /// <summary>
    /// Gets the <see cref="Cell"/> at the given X, Y coordinates.
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
    /// Sets the <see cref="Cell"/> at the given coordinates.
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
    /// represents a cell in a TiledLayer: ITiledMapTile, flip and rotation properties.
    /// </summary>
    [PublicAPI]
    public class Cell
    {
        public const int ROTATE0   = 0;
        public const int ROTATE90  = 1;
        public const int ROTATE180 = 2;
        public const int ROTATE270 = 3;

        private bool           _flipHorizontally;
        private bool           _flipVertically;
        private int            _rotation;
        private ITiledMapTile? _tile;

        /// <summary>
        /// return The tile currently assigned to this cell.
        /// </summary>
        public ITiledMapTile? GetTile()
        {
            return _tile;
        }

        /// <summary>
        /// Sets the tile to be used for this cell.
        /// </summary>
        /// <param name="tile"> the <see cref="ITiledMapTile"/> to use for this cell. </param>
        /// <returns> this, for method chaining </returns>
        public Cell SetTile( ITiledMapTile tile )
        {
            _tile = tile;

            return this;
        }

        /// <summary>
        /// Returns whether the tile should be flipped horizontally.
        /// </summary>
        public bool GetFlipHorizontally()
        {
            return _flipHorizontally;
        }

        /// <summary>
        /// Sets whether to flip the tile horizontally.
        /// </summary>
        /// <param name="flipHorizontally"> whether or not to flip the tile horizontally. </param>
        /// <returns> this, for method chaining </returns>
        public Cell SetFlipHorizontally( bool flipHorizontally )
        {
            _flipHorizontally = flipHorizontally;

            return this;
        }

        /// <summary>
        /// Returns whether the tile should be flipped vertically.
        /// </summary>
        public bool GetFlipVertically()
        {
            return _flipVertically;
        }

        /// <summary>
        /// Sets whether to flip the tile vertically.
        /// </summary>
        /// <param name="flipVertically"> whether or not this tile should be flipped vertically. </param>
        /// <returns> this, for method chaining </returns>
        public Cell SetFlipVertically( bool flipVertically )
        {
            _flipVertically = flipVertically;

            return this;
        }

        /// <summary>
        /// Returns the rotation of this cell, in 90 degree increments.
        /// </summary>
        public int GetRotation()
        {
            return _rotation;
        }

        /// <summary>
        /// Sets the rotation of this cell, in 90 degree increments.
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
