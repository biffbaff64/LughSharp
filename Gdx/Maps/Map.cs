namespace LibGDXSharp.Maps
{
    /// <summary>
    /// This is a base type so cannot be a record or struct.
    /// </summary>
    public class Map
    {
        public MapLayers     Layers     { get; } = new MapLayers();
        public MapProperties Properties { get; } = new MapProperties();
    }
}
