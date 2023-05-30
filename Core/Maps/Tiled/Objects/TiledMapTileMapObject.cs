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