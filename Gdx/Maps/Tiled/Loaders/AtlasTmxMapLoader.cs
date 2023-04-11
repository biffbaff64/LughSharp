namespace LibGDXSharp.Maps.Tiled
{
    /// <summary>
    /// A TiledMap Loader which loads tiles from a TextureAtlas instead of separate images.
    /// It requires a map-level property called 'atlas' with its value being the relative
    /// path to the TextureAtlas.
    /// <p>
    /// The atlas must have in it indexed regions named after the tilesets used in the map.
    /// The indexes shall be local to the tileset (not the global id). Strip whitespace and
    /// rotation should not be used when creating the atlas.
    /// </p>
    /// </summary>
    public class AtlasTmxMapLoader : BaseTmxMapLoader<AtlasTmxMapLoader.AtlasTiledMapLoaderParameters>
    {
        public class AtlasTiledMapLoaderParameters : BaseTmxMapLoader.Parameters
        {
            public bool ForceTextureFilters { get; set; } = false;
        }

    }
}

