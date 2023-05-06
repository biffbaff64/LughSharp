using LibGDXSharp.G2D;
using LibGDXSharp.Maps.Tiled.Tiles;
using LibGDXSharp.Maths;

namespace LibGDXSharp.Maps.Tiled.Renderers;

public class BatchTileMapRenderer : ITiledMapRenderer
{
    private const int NumVertices = 20;

    public TiledMap       TiledMap    { get; set; }
    public IBatch         Batch       { get; set; }
    public RectangleShape ViewBounds  { get; set; }
    public float          UnitScale   { get; set; }
    public RectangleShape ImageBounds { get; set; } = new RectangleShape();
    public bool           OwnsBatch   { get; set; }
    public float[]        Vertices    { get; set; } = new float[ NumVertices ];

    public BatchTileMapRenderer() : this( new TiledMap(), 1.0f )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    public BatchTileMapRenderer( TiledMap map ) : this( map, 1.0f )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="batch"></param>
    public BatchTileMapRenderer( TiledMap map, IBatch batch ) : this( map, 1.0f, batch )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="unitScale"></param>
    public BatchTileMapRenderer( TiledMap map, float unitScale )
        : this( map, unitScale, new SpriteBatch(), true )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="unitScale"></param>
    /// <param name="batch"></param>
    /// <param name="ownsBatch"></param>
    public BatchTileMapRenderer( TiledMap map, float unitScale, IBatch batch, bool ownsBatch = false )
    {
        this.TiledMap   = map;
        this.UnitScale  = unitScale;
        this.ViewBounds = new RectangleShape();
        this.Batch      = batch;
        this.OwnsBatch  = ownsBatch;
    }

    /// <summary>
    /// </summary>
    public void Render()
    {
        BeginRender();

        foreach ( MapLayer layer in TiledMap.Layers )
        {
            RenderMapLayer( layer );
        }

        EndRender();
    }

    /// <summary>
    /// </summary>
    /// <param name="layers"></param>
    public void Render( int[] layers )
    {
        BeginRender();

        foreach ( var layerIdx in layers )
        {
            MapLayer layer = TiledMap.Layers.Get( layerIdx );
            RenderMapLayer( layer );
        }

        EndRender();
    }

    /// <summary>
    /// </summary>
    /// <param name="camera"></param>
    public void SetView( OrthographicCamera camera )
    {
        Batch.SetProjectionMatrix( camera.Combined );

        var width  = camera.ViewportWidth * camera.Zoom;
        var height = camera.ViewportHeight * camera.Zoom;

        var w = width * Math.Abs( camera.Up.Y ) + height * Math.Abs( camera.Up.X );
        var h = height * Math.Abs( camera.Up.Y ) + width * Math.Abs( camera.Up.X );

        ViewBounds.Set( camera.Position.X - w / 2, camera.Position.Y - h / 2, w, h );
    }

    /// <summary>
    /// </summary>
    /// <param name="projection"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetView( Matrix4 projection, float x, float y, float width, float height )
    {
        Batch.SetProjectionMatrix( projection );
        ViewBounds.Set( x, y, width, height );
    }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    protected void RenderMapLayer( MapLayer layer )
    {
        if ( !layer.Visible ) return;

        if ( layer is MapGroupLayer )
        {
            MapLayers childLayers = ( ( MapGroupLayer )layer ).GetLayers();

            for ( int i = 0; i < childLayers.Size(); i++ )
            {
                MapLayer childLayer = childLayers.Get( i );

                if ( !childLayer.Visible ) continue;

                RenderMapLayer( childLayer );
            }
        }
        else
        {
            if ( layer is TiledMapTileLayer )
            {
                RenderTileLayer( ( TiledMapTileLayer )layer );
            }
            else if ( layer is TiledMapImageLayer )
            {
                RenderImageLayer( ( TiledMapImageLayer )layer );
            }
            else
            {
                RenderObjects( layer );
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    public void RenderObjects( MapLayer layer )
    {
        foreach ( MapObject obj in layer.Objects )
        {
            RenderObject( obj );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    public void RenderObject( MapObject obj )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    public void RenderTileLayer( TiledMapTileLayer layer )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    public void RenderImageLayer( TiledMapImageLayer layer )
    {
        Color batchColor = Batch.GetColor();

        var color = Color.ToFloatBits
            (
             batchColor.R,
             batchColor.G,
             batchColor.B,
             batchColor.A * layer.Opacity
            );

        TextureRegion? region = layer.Region;

        if ( region == null ) return;

        var x  = layer.X;
        var y  = layer.Y;
        var x1 = x * UnitScale;
        var y1 = y * UnitScale;
        var x2 = x1 + region.RegionWidth * UnitScale;
        var y2 = y1 + region.RegionHeight * UnitScale;

        ImageBounds.Set( x1, y1, x2 - x1, y2 - y1 );

        if ( ViewBounds.Contains( ImageBounds ) || ViewBounds.Overlaps( ImageBounds ) )
        {
            float u1 = region.GetU();
            float v1 = region.GetV2();
            float u2 = region.GetU2();
            float v2 = region.GetV();

            Vertices[ IBatch.X1 ] = x1;
            Vertices[ IBatch.Y1 ] = y1;
            Vertices[ IBatch.C1 ] = color;
            Vertices[ IBatch.U1 ] = u1;
            Vertices[ IBatch.V1 ] = v1;

            Vertices[ IBatch.X2 ] = x1;
            Vertices[ IBatch.Y2 ] = y2;
            Vertices[ IBatch.C2 ] = color;
            Vertices[ IBatch.U2 ] = u1;
            Vertices[ IBatch.V2 ] = v2;
                                  
            Vertices[ IBatch.X3 ] = x2;
            Vertices[ IBatch.Y3 ] = y2;
            Vertices[ IBatch.C3 ] = color;
            Vertices[ IBatch.U3 ] = u2;
            Vertices[ IBatch.V3 ] = v2;

            Vertices[ IBatch.X4 ] = x2;
            Vertices[ IBatch.Y4 ] = y1;
            Vertices[ IBatch.C4 ] = color;
            Vertices[ IBatch.U4 ] = u2;
            Vertices[ IBatch.V4 ] = v1;

            Batch.Draw( region.Texture, Vertices, 0, NumVertices );
        }
    }

    /// <summary>
    /// Called before the rendering of all layers starts.
    /// </summary>
    protected void BeginRender()
    {
        AnimatedTiledMapTile.UpdateAnimationBaseTime();
        Batch.Begin();
    }

    /// <summary>
    /// Called after the rendering of all layers ended.
    /// </summary>
    protected void EndRender()
    {
        Batch.End();
    }
}