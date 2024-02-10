// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Graphics.G2D;
using LibGDXSharp.LibCore.Maps.Tiled.Tiles;
using LibGDXSharp.LibCore.Maths;

using Matrix4 = LibGDXSharp.LibCore.Maths.Matrix4;

namespace LibGDXSharp.LibCore.Maps.Tiled.Renderers;

public class BatchTileMapRenderer : ITiledMapRenderer
{
    protected const int NUM_VERTICES = 20;

    public BatchTileMapRenderer() : this( new TiledMap() )
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
        TiledMap   = map;
        UnitScale  = unitScale;
        ViewBounds = new RectangleShape();
        Batch      = batch;
        OwnsBatch  = ownsBatch;
    }

    public TiledMap       TiledMap    { get; set; }
    public bool           OwnsBatch   { get; set; }
    public RectangleShape ImageBounds { get; set; } = new();

    protected IBatch         Batch      { get; set; }
    protected RectangleShape ViewBounds { get; set; }
    protected float          UnitScale  { get; set; }
    protected float[]        Vertices   { get; set; } = new float[ NUM_VERTICES ];

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
    public virtual void RenderTileLayer( TiledMapTileLayer layer )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    public void RenderImageLayer( TiledMapImageLayer layer )
    {
        Color batchColor = Batch.Color;

        var color = Color.ToFloatBits( batchColor.R,
                                       batchColor.G,
                                       batchColor.B,
                                       batchColor.A * layer.Opacity );

        TextureRegion? region = layer.Region;

        if ( region == null )
        {
            return;
        }

        var x  = layer.X;
        var y  = layer.Y;
        var x1 = x * UnitScale;
        var y1 = y * UnitScale;
        var x2 = x1 + ( region.RegionWidth * UnitScale );
        var y2 = y1 + ( region.RegionHeight * UnitScale );

        ImageBounds.Set( x1, y1, x2 - x1, y2 - y1 );

        if ( ViewBounds.Contains( ImageBounds ) || ViewBounds.Overlaps( ImageBounds ) )
        {
            var u1 = region.U;
            var v1 = region.V2;
            var u2 = region.U2;
            var v2 = region.V;

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

            Batch.Draw( region.Texture, Vertices, 0, NUM_VERTICES );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    protected void RenderMapLayer( MapLayer layer )
    {
        if ( !layer.Visible )
        {
            return;
        }

        if ( layer is MapGroupLayer groupLayer )
        {
            MapLayers childLayers = groupLayer.Layers;

            for ( var i = 0; i < childLayers.Size(); i++ )
            {
                MapLayer childLayer = childLayers.Get( i );

                if ( !childLayer.Visible )
                {
                    continue;
                }

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
    ///     Called before the rendering of all layers starts.
    /// </summary>
    protected void BeginRender()
    {
        AnimatedTiledMapTile.UpdateAnimationBaseTime();
        Batch.Begin();
    }

    /// <summary>
    ///     Called after the rendering of all layers ended.
    /// </summary>
    protected void EndRender() => Batch.End();
}
