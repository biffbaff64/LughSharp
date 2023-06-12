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

using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;
using LibGDXSharp.Maps.Objects;
using LibGDXSharp.Maps.Tiled.Tiles;

namespace LibGDXSharp.Maps.Tiled.Objects;

/// <summary>
/// A <see cref="MapObject"/> with a <see cref="ITiledMapTile"/>.
/// Can be both <see cref="StaticTiledMapTile"/> or <see cref="AnimatedTiledMapTile"/>.
/// For compatibility reasons, this extends <see cref="TextureMapObject"/>.
/// Use <see cref="ITiledMapTile.TextureRegion"/> instead of <see cref="TextureRegion"/>.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class TiledMapTileMapObject : TextureMapObject
{
    public ITiledMapTile Tile             { get; set; }
    public bool          FlipHorizontally { get; set; }
    public bool          FlipVertically   { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="flipHorizontally"></param>
    /// <param name="flipVertically"></param>
    public TiledMapTileMapObject( ITiledMapTile tile, bool flipHorizontally, bool flipVertically )
    {
        this.FlipHorizontally = flipHorizontally;
        this.FlipVertically   = flipVertically;
        this.Tile              = tile;

        TextureRegion region = new TextureRegion( tile.TextureRegion );

        region.Flip( flipHorizontally, flipVertically );

        TextureRegion = region;
    }
}