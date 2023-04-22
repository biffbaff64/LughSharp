using LibGDXSharp.G2D;
using LibGDXSharp.Maps.Objects;
using LibGDXSharp.Maps.Tiled.Tiles;

namespace LibGDXSharp.Maps.Tiled.Objects
{
    /// <summary>
    /// A <see cref="MapObject"/> with a <see cref="ITiledMapTile"/>.
    /// Can be both <see cref="StaticTiledMapTile"/> or <see cref="AnimatedTiledMapTile"/>.
    /// For compatibility reasons, this extends <see cref="TextureMapObject"/>.
    /// Use <see cref="ITiledMapTile.TextureRegion"/> instead of <see cref="TextureRegion"/>.
    /// </summary>
    public class TiledMapTileMapObject : TextureMapObject
    {
        public ITiledMapTile Tile { get; set; }

        private bool _flipHorizontally;
        private bool _flipVertically;

        /// <summary>
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="flipHorizontally"></param>
        /// <param name="flipVertically"></param>
        public TiledMapTileMapObject( ITiledMapTile tile, bool flipHorizontally, bool flipVertically )
        {
            this._flipHorizontally  = flipHorizontally;
            this._flipVertically    = flipVertically;
            this.Tile = tile;

            TextureRegion region = new TextureRegion( tile.TextureRegion );

            region.Flip( flipHorizontally, flipVertically );

            TextureRegion = region;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public bool isFlipHorizontally()
        {
            return _flipHorizontally;
        }

        /// <summary>
        /// </summary>
        /// <param name="flipHorizontally"></param>
        public void setFlipHorizontally( bool flipHorizontally )
        {
            this._flipHorizontally = flipHorizontally;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public bool isFlipVertically()
        {
            return _flipVertically;
        }

        /// <summary>
        /// </summary>
        /// <param name="flipVertically"></param>
        public void setFlipVertically( bool flipVertically )
        {
            this._flipVertically = flipVertically;
        }
    }
}

