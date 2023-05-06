using LibGDXSharp.G2D;

namespace LibGDXSharp.Maps.Tiled;

public interface ITiledMapTile
{
    public enum Blendmode
    {
        None,
        Alpha
    }

    int           ID            { get; set; }
    Blendmode     BlendMode     { get; set; }
    TextureRegion TextureRegion { get; set; }
    float         OffsetX       { get; set; }
    float         OffsetY       { get; set; }

    public MapProperties GetProperties();

    public MapObjects GetObjects();
}