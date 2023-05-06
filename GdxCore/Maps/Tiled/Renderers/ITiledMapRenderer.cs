namespace LibGDXSharp.Maps.Tiled;

public interface ITiledMapRenderer : IMapRenderer
{
    void RenderObjects( MapLayer layer );
    void RenderObject( MapObject mapObject );

    void RenderTileLayer( TiledMapTileLayer layer );
    void RenderImageLayer( TiledMapImageLayer layer );
}