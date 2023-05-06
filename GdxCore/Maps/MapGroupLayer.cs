namespace LibGDXSharp.Maps;

/// <summary>
/// Map layer containing a set of MapLayers, objects and properties.
/// </summary>
public sealed class MapGroupLayer : MapLayer
{
    public MapLayers Layers { get; private set; } = new MapLayers();

    public new void InvalidateRenderOffset()
    {
        base.InvalidateRenderOffset();

        for ( var i = 0; i < Layers.Size(); i++ )
        {
            MapLayer child = Layers.Get( i );
            child.InvalidateRenderOffset();
        }
    }
}