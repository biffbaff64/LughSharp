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

using LibGDXSharp.G2D;
using LibGDXSharp.Maps.Tiled.Tiles;
using LibGDXSharp.Maths;

namespace LibGDXSharp.Maps.Tiled.Renderers;

public class BatchTileMapRenderer : ITiledMapRenderer
{
    protected const int NumVertices = 20;

    public TiledMap       TiledMap    { get; set; }
    public bool           OwnsBatch   { get; set; }
    public RectangleShape ImageBounds { get; set; } = new RectangleShape();

    protected IBatch         Batch      { get; set; }
    protected RectangleShape ViewBounds { get; set; }
    protected float          UnitScale  { get; set; }
    protected float[]        Vertices   { get; set; } = new float[ NumVertices ];

    public BatchTileMapRenderer() : this( new TiledMap(), 1.0f )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="batch"></param>
    protected BatchTileMapRenderer( TiledMap map, IBatch batch )
        : this( map, 1.0f, batch )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="unitScale"></param>
    protected BatchTileMapRenderer( TiledMap map, float unitScale = 1.0f )
        : this( map, unitScale, new SpriteBatch(), true )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="unitScale"></param>
    /// <param name="batch"></param>
    /// <param name="ownsBatch"></param>
    protected BatchTileMapRenderer( TiledMap map, float unitScale, IBatch batch, bool ownsBatch = false )
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

        var w = ( width * Math.Abs( camera.Up.Y ) ) + ( height * Math.Abs( camera.Up.X ) );
        var h = ( height * Math.Abs( camera.Up.Y ) ) + ( width * Math.Abs( camera.Up.X ) );

        ViewBounds.Set( camera.Position.X - ( w / 2 ), camera.Position.Y - ( h / 2 ), w, h );
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

        if ( layer is MapGroupLayer groupLayer )
        {
            MapLayers childLayers = groupLayer.Layers;

            for ( int i = 0; i < childLayers.Size(); i++ )
            {
                MapLayer childLayer = childLayers.Get( i );

                if ( !childLayer.Visible ) continue;

                RenderMapLayer( childLayer );
            }
        }
        else
        {
            if ( layer is TiledMapTileLayer tileLayer )
            {
                RenderTileLayer( tileLayer );
            }
            else if ( layer is TiledMapImageLayer imageLayer )
            {
                RenderImageLayer( imageLayer );
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
        var x2 = x1 + ( region.RegionWidth * UnitScale );
        var y2 = y1 + ( region.RegionHeight * UnitScale );

        ImageBounds.Set( x1, y1, x2 - x1, y2 - y1 );

        if ( ViewBounds.Contains( ImageBounds ) || ViewBounds.Overlaps( ImageBounds ) )
        {
            float u1 = region.U;
            float v1 = region.V2;
            float u2 = region.U2;
            float v2 = region.V;

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