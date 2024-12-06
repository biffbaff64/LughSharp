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

using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Maps.Objects;
using Corelib.Lugh.Maps.Tiled.Tiles;

namespace Corelib.Lugh.Maps.Tiled.Objects;

/// <summary>
/// A <see cref="MapObject"/> with a <see cref="ITiledMapTile"/>.
/// Can be both <see cref="StaticTiledMapTile"/> or <see cref="AnimatedTiledMapTile"/>.
/// For compatibility reasons, this extends <see cref="TextureMapObject"/>.
/// Use <see cref="ITiledMapTile.TextureRegion"/> instead of <see cref="TextureRegion"/>.
/// </summary>
[PublicAPI]
public class TiledMapTileMapObject : TextureMapObject
{
    /// <summary>
    /// Creates a new <see cref="MapObject"/> with an attached <see cref="ITiledMapTile"/>.
    /// </summary>
    /// <param name="tile"> The Tile to attach. </param>
    /// <param name="flipHorizontally"> True to flip this mapobject horizontally. </param>
    /// <param name="flipVertically"> True to flip this mapobject vertically. </param>
    public TiledMapTileMapObject( ITiledMapTile tile, bool flipHorizontally, bool flipVertically )
    {
        FlipHorizontally = flipHorizontally;
        FlipVertically   = flipVertically;
        Tile             = tile;

        var region = new TextureRegion( tile.TextureRegion );

        region.Flip( flipHorizontally, flipVertically );

        TextureRegion = region;
    }

    public ITiledMapTile Tile             { get; set; }
    public bool          FlipHorizontally { get; set; }
    public bool          FlipVertically   { get; set; }
}
