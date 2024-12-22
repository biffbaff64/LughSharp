// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.Cameras;
using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Maps.Tiled.Renderers;

/// <summary>
/// Renders ortho tiles by caching geometry on the GPU. How much is cached is controlled
/// by SetOverCache(float). When the view reaches the edge of the cached tiles, the cache
/// is rebuilt at the new view position. This class may have poor performance when tiles
/// are often changed dynamically, since the cache must be rebuilt after each change.
/// </summary>
[PublicAPI]
public class OrthoCachedTiledMapRenderer : ITiledMapRenderer, IDisposable
{
    private const    int            DEFAULT_CACHE_SIZE = 2000;
    private static   float          _tolerance         = 0.00001f;
    protected static int            NumVertices        = 20;
    protected        bool           Blending;
    protected        RectangleShape CacheBounds = new();
    protected        bool           Cached;
    protected        bool           CanCacheMoreE;
    protected        bool           CanCacheMoreN;
    protected        bool           CanCacheMoreS;
    protected        bool           CanCacheMoreW;
    protected        int            Count;

    protected TiledMap?      Map;
    protected float          MaxTileHeight;
    protected float          MaxTileWidth;
    protected SpriteCache?   SpriteCache;
    protected float          UnitScale;
    protected float[]        Vertices   = new float[ 20 ];
    protected RectangleShape ViewBounds = new();

    // ========================================================================

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="unitScale"></param>
    /// <param name="cacheSize">
    /// The maximum number of tiles that can be cached. The default size is 2000.
    /// </param>
    public OrthoCachedTiledMapRenderer( TiledMap map, float unitScale, int cacheSize = DEFAULT_CACHE_SIZE )
    {
        Map         = map;
        UnitScale   = unitScale;
        SpriteCache = new SpriteCache( cacheSize, true );
    }

    /// <summary>
    /// Sets the percentage of the view that is cached in each direction. Default is 0.5.
    /// <para>
    /// Eg, 0.75 will cache 75% of the width of the view to the left and right of the view,
    /// and 75% of the height of the view above and below the view.
    /// </para>
    /// </summary>
    public float OverCache { get; set; } = 0.50f;

    // <inheritdoc />
    public void Dispose()
    {
        SpriteCache?.Dispose();
    }

    public void SetView( OrthographicCamera camera )
    {
        GdxRuntimeException.ThrowIfNull( SpriteCache );

        SpriteCache.ProjectionMatrix = camera.Combined;

        var width  = ( camera.ViewportWidth * camera.Zoom ) + ( MaxTileWidth * 2 * UnitScale );
        var height = ( camera.ViewportHeight * camera.Zoom ) + ( MaxTileHeight * 2 * UnitScale );

        ViewBounds.Set( camera.Position.X - ( width / 2 ), camera.Position.Y - ( height / 2 ), width, height );

        if ( ( CanCacheMoreW && ( ViewBounds.X < ( CacheBounds.X - _tolerance ) ) )
          || ( CanCacheMoreS && ( ViewBounds.Y < ( CacheBounds.Y - _tolerance ) ) )
          || ( CanCacheMoreE && ( ( ViewBounds.X + ViewBounds.Width ) > ( CacheBounds.X + CacheBounds.Width + _tolerance ) ) )
          || ( CanCacheMoreN && ( ( ViewBounds.Y + ViewBounds.Height ) > ( CacheBounds.Y + CacheBounds.Height + _tolerance ) ) ) )
        {
            Cached = false;
        }
    }

    public void SetView( Matrix4 projection, float x, float y, float width, float height )
    {
        GdxRuntimeException.ThrowIfNull( SpriteCache );

        SpriteCache.ProjectionMatrix = projection;

        x      -= MaxTileWidth * UnitScale;
        y      -= MaxTileHeight * UnitScale;
        width  += MaxTileWidth * 2 * UnitScale;
        height += MaxTileHeight * 2 * UnitScale;

        ViewBounds.Set( x, y, width, height );

        if ( ( CanCacheMoreW && ( ViewBounds.X < ( CacheBounds.X - _tolerance ) ) )
          || ( CanCacheMoreS && ( ViewBounds.Y < ( CacheBounds.Y - _tolerance ) ) )
          || ( CanCacheMoreE && ( ( ViewBounds.X + ViewBounds.Width ) > ( CacheBounds.X + CacheBounds.Width + _tolerance ) ) )
          || ( CanCacheMoreN && ( ( ViewBounds.Y + ViewBounds.Height ) > ( CacheBounds.Y + CacheBounds.Height + _tolerance ) ) ) )
        {
            Cached = false;
        }
    }

    public void Render()
    {
        GdxRuntimeException.ThrowIfNull( SpriteCache );

        if ( !Cached )
        {
            Cached = true;
            Count  = 0;
            SpriteCache.Clear();

            var extraWidth  = ViewBounds.Width * OverCache;
            var extraHeight = ViewBounds.Height * OverCache;

            CacheBounds.X      = ViewBounds.X - extraWidth;
            CacheBounds.Y      = ViewBounds.Y - extraHeight;
            CacheBounds.Width  = ViewBounds.Width + ( extraWidth * 2 );
            CacheBounds.Height = ViewBounds.Height + ( extraHeight * 2 );

            foreach ( var layer in Map!.Layers )
            {
                SpriteCache.BeginCache();

                if ( layer is TiledMapTileLayer tileLayer )
                {
                    RenderTileLayer( tileLayer );
                }
                else if ( layer is TiledMapImageLayer imageLayer )
                {
                    RenderImageLayer( imageLayer );
                }

                SpriteCache.EndCache();
            }
        }

        if ( Blending )
        {
            GdxApi.Bindings.Enable( IGL.GL_BLEND );
            GdxApi.Bindings.BlendFunc( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );
        }

        SpriteCache.Begin();

        var mapLayers = Map?.Layers;

        for ( int i = 0, j = mapLayers!.Size(); i < j; i++ )
        {
            var layer = mapLayers.Get( i );

            if ( layer.Visible )
            {
                SpriteCache.Draw( i );
                RenderObjects( layer );
            }
        }

        SpriteCache.End();

        if ( Blending )
        {
            GdxApi.Bindings.Disable( IGL.GL_BLEND );
        }
    }

    public void Render( int[] layers )
    {
        if ( !Cached )
        {
            Cached = true;
            Count  = 0;
            SpriteCache?.Clear();

            var extraWidth  = ViewBounds.Width * OverCache;
            var extraHeight = ViewBounds.Height * OverCache;

            CacheBounds.X      = ViewBounds.X - extraWidth;
            CacheBounds.Y      = ViewBounds.Y - extraHeight;
            CacheBounds.Width  = ViewBounds.Width + ( extraWidth * 2 );
            CacheBounds.Height = ViewBounds.Height + ( extraHeight * 2 );

            foreach ( var layer in Map!.Layers )
            {
                SpriteCache?.BeginCache();

                if ( layer is TiledMapTileLayer tileLayer )
                {
                    RenderTileLayer( tileLayer );
                }
                else if ( layer is TiledMapImageLayer imageLayer )
                {
                    RenderImageLayer( imageLayer );
                }

                SpriteCache?.EndCache();
            }
        }

        if ( Blending )
        {
            GdxApi.Bindings.Enable( IGL.GL_BLEND );
            GdxApi.Bindings.BlendFunc( IGL.GL_SRC_ALPHA, IGL.GL_ONE_MINUS_SRC_ALPHA );
        }

        SpriteCache?.Begin();

        foreach ( var i in layers )
        {
            var layer = Map?.Layers.Get( i );

            if ( layer!.Visible )
            {
                SpriteCache?.Draw( i );
                RenderObjects( layer );
            }
        }

        SpriteCache?.End();

        if ( Blending )
        {
            GdxApi.Bindings.Disable( IGL.GL_BLEND );
        }
    }

    public void RenderObjects( MapLayer layer )
    {
        foreach ( var mapObject in layer.Objects )
        {
            RenderObject( mapObject );
        }
    }

    public void RenderObject( MapObject mapObject )
    {
    }

    public void RenderTileLayer( TiledMapTileLayer layer )
    {
        var color = Color.ToFloatBitsABGR( 1, 1, 1, layer.Opacity );

        var layerWidth  = layer.Width;
        var layerHeight = layer.Height;

        var layerTileWidth  = layer.TileWidth * UnitScale;
        var layerTileHeight = layer.TileHeight * UnitScale;
        var layerOffsetX    = layer.RenderOffsetX * UnitScale;

        // offset in tiled is y down, so we flip it
        var layerOffsetY = -layer.RenderOffsetY * UnitScale;

        var col1 = Math.Max( 0, ( int ) ( ( CacheBounds.X - layerOffsetX ) / layerTileWidth ) );
        var col2 = Math.Min( layerWidth,
                             ( int ) ( ( ( CacheBounds.X + CacheBounds.Width + layerTileWidth ) - layerOffsetX ) / layerTileWidth ) );

        var row1 = Math.Max( 0, ( int ) ( ( CacheBounds.Y - layerOffsetY ) / layerTileHeight ) );
        var row2 = Math.Min( layerHeight,
                             ( int ) ( ( ( CacheBounds.Y + CacheBounds.Height + layerTileHeight ) - layerOffsetY ) / layerTileHeight ) );

        CanCacheMoreN = row2 < layerHeight;
        CanCacheMoreE = col2 < layerWidth;
        CanCacheMoreW = col1 > 0;
        CanCacheMoreS = row1 > 0;

        for ( var row = row2; row >= row1; row-- )
        {
            for ( var col = col1; col < col2; col++ )
            {
                var cell = layer.GetCell( col, row );

                if ( cell == null ) continue;

                var tile = cell.GetTile();

                if ( tile == null ) continue;

                Count++;

                var flipX     = cell.GetFlipHorizontally();
                var flipY     = cell.GetFlipVertically();
                var rotations = cell.GetRotation();

                var region  = tile.TextureRegion;
                var texture = region.Texture;

                var x1 = ( col * layerTileWidth ) + ( tile.OffsetX * UnitScale ) + layerOffsetX;
                var y1 = ( row * layerTileHeight ) + ( tile.OffsetY * UnitScale ) + layerOffsetY;
                var x2 = x1 + ( region.RegionWidth * UnitScale );
                var y2 = y1 + ( region.RegionHeight * UnitScale );

                var adjustX = 0.5f / texture.Width;
                var adjustY = 0.5f / texture.Height;
                var u1      = region.U + adjustX;
                var v1      = region.V2 - adjustY;
                var u2      = region.U2 - adjustX;
                var v2      = region.V + adjustY;

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

                if ( flipX )
                {
                    var temp = Vertices[ IBatch.U1 ];
                    Vertices[ IBatch.U1 ] = Vertices[ IBatch.U3 ];
                    Vertices[ IBatch.U3 ] = temp;
                    temp                  = Vertices[ IBatch.U2 ];
                    Vertices[ IBatch.U2 ] = Vertices[ IBatch.U4 ];
                    Vertices[ IBatch.U4 ] = temp;
                }

                if ( flipY )
                {
                    var temp = Vertices[ IBatch.V1 ];
                    Vertices[ IBatch.V1 ] = Vertices[ IBatch.V3 ];
                    Vertices[ IBatch.V3 ] = temp;
                    temp                  = Vertices[ IBatch.V2 ];
                    Vertices[ IBatch.V2 ] = Vertices[ IBatch.V4 ];
                    Vertices[ IBatch.V4 ] = temp;
                }

                if ( rotations != 0 )
                {
                    switch ( rotations )
                    {
                        case TiledMapTileLayer.Cell.ROTATE90:
                        {
                            var tempV = Vertices[ IBatch.V1 ];
                            Vertices[ IBatch.V1 ] = Vertices[ IBatch.V2 ];
                            Vertices[ IBatch.V2 ] = Vertices[ IBatch.V3 ];
                            Vertices[ IBatch.V3 ] = Vertices[ IBatch.V4 ];
                            Vertices[ IBatch.V4 ] = tempV;

                            var tempU = Vertices[ IBatch.U1 ];
                            Vertices[ IBatch.U1 ] = Vertices[ IBatch.U2 ];
                            Vertices[ IBatch.U2 ] = Vertices[ IBatch.U3 ];
                            Vertices[ IBatch.U3 ] = Vertices[ IBatch.U4 ];
                            Vertices[ IBatch.U4 ] = tempU;

                            break;
                        }

                        case TiledMapTileLayer.Cell.ROTATE180:
                        {
                            var tempU = Vertices[ IBatch.U1 ];
                            Vertices[ IBatch.U1 ] = Vertices[ IBatch.U3 ];
                            Vertices[ IBatch.U3 ] = tempU;
                            tempU                 = Vertices[ IBatch.U2 ];
                            Vertices[ IBatch.U2 ] = Vertices[ IBatch.U4 ];
                            Vertices[ IBatch.U4 ] = tempU;

                            var tempV = Vertices[ IBatch.V1 ];
                            Vertices[ IBatch.V1 ] = Vertices[ IBatch.V3 ];
                            Vertices[ IBatch.V3 ] = tempV;
                            tempV                 = Vertices[ IBatch.V2 ];
                            Vertices[ IBatch.V2 ] = Vertices[ IBatch.V4 ];
                            Vertices[ IBatch.V4 ] = tempV;

                            break;
                        }

                        case TiledMapTileLayer.Cell.ROTATE270:
                        {
                            var tempV = Vertices[ IBatch.V1 ];
                            Vertices[ IBatch.V1 ] = Vertices[ IBatch.V4 ];
                            Vertices[ IBatch.V4 ] = Vertices[ IBatch.V3 ];
                            Vertices[ IBatch.V3 ] = Vertices[ IBatch.V2 ];
                            Vertices[ IBatch.V2 ] = tempV;

                            var tempU = Vertices[ IBatch.U1 ];
                            Vertices[ IBatch.U1 ] = Vertices[ IBatch.U4 ];
                            Vertices[ IBatch.U4 ] = Vertices[ IBatch.U3 ];
                            Vertices[ IBatch.U3 ] = Vertices[ IBatch.U2 ];
                            Vertices[ IBatch.U2 ] = tempU;

                            break;
                        }
                    }
                }

                SpriteCache?.Add( texture, Vertices, 0, NumVertices );
            }
        }
    }

    public void RenderImageLayer( TiledMapImageLayer layer )
    {
        var color = Color.ToFloatBitsABGR( 1.0f, 1.0f, 1.0f, layer.Opacity );

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

        SpriteCache?.Add( region.Texture, Vertices, 0, NumVertices );
    }

    /// <summary>
    /// Causes the cache to be rebuilt the next time it is rendered.
    /// </summary>
    public void InvalidateCache()
    {
        Cached = false;
    }

    /// <summary>
    /// Returns true if tiles are currently cached.
    /// </summary>
    public bool IsCached()
    {
        return Cached;
    }

    /// <summary>
    /// Expands the view size in each direction, ensuring that tiles of this size or
    /// smaller are never culled from the visible portion of the view. Default is 0,0.
    /// <para>
    /// The amount of tiles cached is computed using <tt>(view size + max tile size) * overCache</tt>,
    /// meaning the max tile size increases the amount cached and possibly
    /// <see cref="OverCache"/> can be reduced.
    /// </para>
    /// <para>
    /// If the view size and <see cref="OverCache"/> are configured so the size of the
    /// cached tiles is always larger than the largest tile size, this setting is not needed.
    /// </para>
    /// </summary>
    public void SetMaxTileSize( float maxPixelWidth, float maxPixelHeight )
    {
        MaxTileWidth  = maxPixelWidth;
        MaxTileHeight = maxPixelHeight;
    }
}
