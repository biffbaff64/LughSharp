// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.Cameras;
using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Maps.Tiled.Tiles;
using Corelib.LibCore.Maths;
using Matrix4 = Corelib.LibCore.Maths.Matrix4;

namespace Corelib.LibCore.Maps.Tiled.Renderers;

[PublicAPI]
public class BatchTileMapRenderer : ITiledMapRenderer
{
    public TiledMap       TiledMap    { get; set; }
    public bool           OwnsBatch   { get; set; }
    public RectangleShape ImageBounds { get; set; } = new();

    protected IBatch         Batch      { get; set; }
    protected RectangleShape ViewBounds { get; set; }
    protected float          UnitScale  { get; set; }
    protected float[]        Vertices   { get; set; } = new float[ NUM_VERTICES ];

    protected const int NUM_VERTICES = 20;

    // ------------------------------------------------------------------------

    public BatchTileMapRenderer()
        : this( new TiledMap() )
    {
    }

    /// <summary>
    /// Creates a new Renderer using the supplied <see cref="TiledMap"/>
    /// and <see cref="IBatch"/>
    /// </summary>
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

    /// <summary>
    /// Draws all layers in the default <see cref="TiledMap"/>.
    /// This is the map supplied on creation, or supplied by any
    /// extending classes.
    /// </summary>
    public void Render()
    {
        BeginRender();

        foreach ( var layer in TiledMap.Layers )
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
            var layer = TiledMap.Layers.Get( layerIdx );
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
        foreach ( var obj in layer.Objects )
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
        var color = Color.ToFloatBitsABGR( Batch.Color.R,
                                       Batch.Color.G,
                                       Batch.Color.B,
                                       Batch.Color.A * layer.Opacity );

        var region = layer.Region;

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
    /// Renders the specified <see cref="MapLayer"/>.
    /// </summary>
    protected void RenderMapLayer( MapLayer layer )
    {
        if ( !layer.Visible )
        {
            return;
        }

        switch ( layer )
        {
            case MapGroupLayer groupLayer:
            {
                RenderGroupLayerChildren( groupLayer );

                break;
            }

            case TiledMapTileLayer tileLayer:
            {
                RenderTileLayer( tileLayer );

                break;
            }

            case TiledMapImageLayer imageLayer:
            {
                RenderImageLayer( imageLayer );

                break;
            }

            default:
            {
                RenderObjects( layer );

                break;
            }
        }
    }

    /// <summary>
    /// Rendes the child layers of a <see cref="MapGroupLayer"/>.
    /// </summary>
    private void RenderGroupLayerChildren( MapGroupLayer groupLayer )
    {
        var childLayers = groupLayer.Layers;

        for ( var i = 0; i < childLayers.Size(); i++ )
        {
            var childLayer = childLayers.Get( i );

            if ( !childLayer.Visible )
            {
                continue;
            }

            RenderMapLayer( childLayer );
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
