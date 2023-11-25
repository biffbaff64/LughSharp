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

namespace LibGDXSharp.Maps.Tiled.Renderers;

[PublicAPI]
public class OrthoCachedTiledMapRenderer : ITiledMapRenderer, IDisposable
{
    private const   float TOLERANCE    = 0.00001f;
    protected const int   NUM_VERTICES = 20;

    public SpriteCache SpriteCache { get; private set; }
    public bool        IsCached    { get; private set; }
    public bool        Blending    { get; private set; }

    /// <summary>
    /// Sets the percentage of the view that is cached in each direction.
    /// Default is 0.5.
    /// <para>
    /// Eg, 0.75 will cache 75% of the width of the view to the left and
    /// right of the view, and 75% of the height of the view above and
    /// below the view.
    /// </para>
    /// </summary>
    public float OverCache { private get; set; } = 0.50f;
    
    private TiledMap       _map;
    private float[]        _vertices = new float[ 20 ];
    private float          _unitScale;
    private RectangleShape _viewBounds  = new();
    private RectangleShape _cacheBounds = new();
    private float          _maxTileWidth;
    private float          _maxTileHeight;
    private int            _count;
    private bool           _canCacheMoreN;
    private bool           _canCacheMoreE;
    private bool           _canCacheMoreW;
    private bool           _canCacheMoreS;

    /// <summary>
    /// Creates a renderer with the supplied unit scale and cache size.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="unitScale"></param>
    /// <param name="cacheSize"> The maximum number of tiles that can be cached. </param>
    public OrthoCachedTiledMapRenderer( TiledMap map, float unitScale = 1, int cacheSize = 2000 )
    {
        this._map       = map;
        this._unitScale = unitScale;
        SpriteCache     = new SpriteCache( cacheSize, true );
    }

    public void SetView( OrthographicCamera camera )
    {
        SpriteCache.ProjectionMatrix = camera.Combined;

        var width  = ( camera.ViewportWidth * camera.Zoom ) + ( _maxTileWidth * 2 * _unitScale );
        var height = ( camera.ViewportHeight * camera.Zoom ) + ( _maxTileHeight * 2 * _unitScale );

        _viewBounds.Set( camera.Position.X - ( width / 2 ),
                         camera.Position.Y - ( height / 2 ),
                         width,
                         height );

        if ( ( _canCacheMoreW && ( _viewBounds.X < ( _cacheBounds.X - TOLERANCE ) ) )
          || ( _canCacheMoreS && ( _viewBounds.Y < ( _cacheBounds.Y - TOLERANCE ) ) )
          || ( _canCacheMoreE && ( ( _viewBounds.X + _viewBounds.Width ) > ( _cacheBounds.X + _cacheBounds.Width + TOLERANCE ) ) )
          || ( _canCacheMoreN && ( ( _viewBounds.Y + _viewBounds.Height ) > ( _cacheBounds.Y + _cacheBounds.Height + TOLERANCE ) ) ) )
        {
            IsCached = false;
        }
    }

    public void SetView( Matrix4 projection, float x, float y, float width, float height )
    {
        SpriteCache.ProjectionMatrix = projection;

        x      -= _maxTileWidth * _unitScale;
        y      -= _maxTileHeight * _unitScale;
        width  += _maxTileWidth * 2 * _unitScale;
        height += _maxTileHeight * 2 * _unitScale;

        _viewBounds.Set( x, y, width, height );

        if ( ( _canCacheMoreW && ( _viewBounds.X < ( _cacheBounds.X - TOLERANCE ) ) )
          || ( _canCacheMoreS && ( _viewBounds.Y < ( _cacheBounds.Y - TOLERANCE ) ) )
          || ( _canCacheMoreE && ( ( _viewBounds.X + _viewBounds.Width ) > ( _cacheBounds.X + _cacheBounds.Width + TOLERANCE ) ) )
          || ( _canCacheMoreN && ( ( _viewBounds.Y + _viewBounds.Height ) > ( _cacheBounds.Y + _cacheBounds.Height + TOLERANCE ) ) ) )
        {
            IsCached = false;
        }
    }

    public void Render()
    {
        if ( !IsCached )
        {
            IsCached = true;
            _count   = 0;
            SpriteCache.Clear();

            var extraWidth  = _viewBounds.Width * OverCache;
            var extraHeight = _viewBounds.Height * OverCache;

            _cacheBounds.X      = _viewBounds.X - extraWidth;
            _cacheBounds.Y      = _viewBounds.Y - extraHeight;
            _cacheBounds.Width  = _viewBounds.Width + ( extraWidth * 2 );
            _cacheBounds.Height = _viewBounds.Height + ( extraHeight * 2 );

            foreach ( MapLayer layer in _map.Layers )
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
            Gdx.GL.GLEnable( IGL20.GL_BLEND );
            Gdx.GL.GLBlendFunc( IGL20.GL_SRC_ALPHA, IGL20.GL_ONE_MINUS_SRC_ALPHA );
        }

        SpriteCache.Begin();

        MapLayers mapLayers = _map.Layers;

        for ( int i = 0, j = mapLayers.GetCount(); i < j; i++ )
        {
            MapLayer layer = mapLayers.Get( i );

            if ( layer.Visible )
            {
                SpriteCache.Draw( i );
                RenderObjects( layer );
            }
        }

        SpriteCache.End();

        if ( Blending )
        {
            Gdx.GL.GLDisable( IGL20.GL_BLEND );
        }
    }

    public void Render( int[] layers )
    {
        if ( !IsCached )
        {
            IsCached = true;
            _count   = 0;

            SpriteCache.Clear();

            var extraWidth  = _viewBounds.Width * OverCache;
            var extraHeight = _viewBounds.Height * OverCache;

            _cacheBounds.X      = _viewBounds.X - extraWidth;
            _cacheBounds.Y      = _viewBounds.Y - extraHeight;
            _cacheBounds.Width  = _viewBounds.Width + ( extraWidth * 2 );
            _cacheBounds.Height = _viewBounds.Height + ( extraHeight * 2 );

            foreach ( MapLayer layer in _map.Layers )
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
            Gdx.GL.GLEnable( IGL20.GL_BLEND );
            Gdx.GL.GLBlendFunc( IGL20.GL_SRC_ALPHA, IGL20.GL_ONE_MINUS_SRC_ALPHA );
        }

        SpriteCache.Begin();

        MapLayers mapLayers = _map.Layers;

        foreach ( var i in layers )
        {
            MapLayer layer = mapLayers.Get( i );

            if ( layer.Visible )
            {
                SpriteCache.Draw( i );
                RenderObjects( layer );
            }
        }

        SpriteCache.End();

        if ( Blending )
        {
            Gdx.GL.GLDisable( IGL20.GL_BLEND );
        }
    }

    public void RenderObjects( MapLayer layer )
    {
        foreach ( MapObject obj in layer.Objects )
        {
            RenderObject( obj );
        }
    }

    public void RenderObject( MapObject obj )
    {
    }

    public void RenderTileLayer( TiledMapTileLayer layer )
    {
        var color = Color.ToFloatBits( 1, 1, 1, layer.Opacity );

        var layerWidth  = layer.Width;
        var layerHeight = layer.Height;

        var layerTileWidth  = layer.TileWidth * _unitScale;
        var layerTileHeight = layer.TileHeight * _unitScale;

        var layerOffsetX = layer.RenderOffsetX * _unitScale;

        // offset in tiled is y down, so we flip it
        var layerOffsetY = -layer.RenderOffsetY * _unitScale;

        var col1 = Math.Max( 0, ( int )( ( _cacheBounds.X - layerOffsetX ) / layerTileWidth ) );

        var col2 = Math.Min( layerWidth,
                             ( int )( ( ( _cacheBounds.X + _cacheBounds.Width + layerTileWidth )
                                      - layerOffsetX )
                                    / layerTileWidth ) );

        var row1 = Math.Max( 0, ( int )( ( _cacheBounds.Y - layerOffsetY ) / layerTileHeight ) );

        var row2 = Math.Min( layerHeight,
                             ( int )( ( ( _cacheBounds.Y + _cacheBounds.Height + layerTileHeight )
                                      - layerOffsetY )
                                    / layerTileHeight ) );

        _canCacheMoreN = row2 < layerHeight;
        _canCacheMoreE = col2 < layerWidth;
        _canCacheMoreW = col1 > 0;
        _canCacheMoreS = row1 > 0;

        for ( var row = row2; row >= row1; row-- )
        {
            for ( var col = col1; col < col2; col++ )
            {
                TiledMapTileLayer.Cell? cell = layer.GetCell( col, row );

                if ( cell == null )
                {
                    continue;
                }

                ITiledMapTile? tile = cell.GetTile();

                if ( tile == null )
                {
                    continue;
                }

                _count++;

                var flipX     = cell.GetFlipHorizontally();
                var flipY     = cell.GetFlipVertically();
                var rotations = cell.GetRotation();

                var x1 = ( col * layerTileWidth ) + ( tile.OffsetX * _unitScale ) + layerOffsetX;
                var y1 = ( row * layerTileHeight ) + ( tile.OffsetY * _unitScale ) + layerOffsetY;
                var x2 = x1 + ( tile.TextureRegion.RegionWidth * _unitScale );
                var y2 = y1 + ( tile.TextureRegion.RegionHeight * _unitScale );

                var adjustX = 0.5f / tile.TextureRegion.Texture.Width;
                var adjustY = 0.5f / tile.TextureRegion.Texture.Height;
                var u1      = tile.TextureRegion.U + adjustX;
                var v1      = tile.TextureRegion.V2 - adjustY;
                var u2      = tile.TextureRegion.U2 - adjustX;
                var v2      = tile.TextureRegion.V + adjustY;

                this._vertices[ IBatch.X1 ] = x1;
                this._vertices[ IBatch.Y1 ] = y1;
                this._vertices[ IBatch.C1 ] = color;
                this._vertices[ IBatch.U1 ] = u1;
                this._vertices[ IBatch.V1 ] = v1;

                this._vertices[ IBatch.X2 ] = x1;
                this._vertices[ IBatch.Y2 ] = y2;
                this._vertices[ IBatch.C2 ] = color;
                this._vertices[ IBatch.U2 ] = u1;
                this._vertices[ IBatch.V2 ] = v2;

                this._vertices[ IBatch.X3 ] = x2;
                this._vertices[ IBatch.Y3 ] = y2;
                this._vertices[ IBatch.C3 ] = color;
                this._vertices[ IBatch.U3 ] = u2;
                this._vertices[ IBatch.V3 ] = v2;

                this._vertices[ IBatch.X4 ] = x2;
                this._vertices[ IBatch.Y4 ] = y1;
                this._vertices[ IBatch.C4 ] = color;
                this._vertices[ IBatch.U4 ] = u2;
                this._vertices[ IBatch.V4 ] = v1;

                if ( flipX )
                {
                    var temp = this._vertices[ IBatch.U1 ];
                    this._vertices[ IBatch.U1 ] = this._vertices[ IBatch.U3 ];
                    this._vertices[ IBatch.U3 ] = temp;

                    temp                        = this._vertices[ IBatch.U2 ];
                    this._vertices[ IBatch.U2 ] = this._vertices[ IBatch.U4 ];
                    this._vertices[ IBatch.U4 ] = temp;
                }

                if ( flipY )
                {
                    var temp = this._vertices[ IBatch.V1 ];
                    this._vertices[ IBatch.V1 ] = this._vertices[ IBatch.V3 ];
                    this._vertices[ IBatch.V3 ] = temp;

                    temp                        = this._vertices[ IBatch.V2 ];
                    this._vertices[ IBatch.V2 ] = this._vertices[ IBatch.V4 ];
                    this._vertices[ IBatch.V4 ] = temp;
                }

                if ( rotations != 0 )
                {
                    switch ( rotations )
                    {
                        case TiledMapTileLayer.Cell.ROTATE90:
                        {
                            var tempV = this._vertices[ IBatch.V1 ];
                            this._vertices[ IBatch.V1 ] = this._vertices[ IBatch.V2 ];
                            this._vertices[ IBatch.V2 ] = this._vertices[ IBatch.V3 ];
                            this._vertices[ IBatch.V3 ] = this._vertices[ IBatch.V4 ];
                            this._vertices[ IBatch.V4 ] = tempV;

                            var tempU = this._vertices[ IBatch.U1 ];
                            this._vertices[ IBatch.U1 ] = this._vertices[ IBatch.U2 ];
                            this._vertices[ IBatch.U2 ] = this._vertices[ IBatch.U3 ];
                            this._vertices[ IBatch.U3 ] = this._vertices[ IBatch.U4 ];
                            this._vertices[ IBatch.U4 ] = tempU;

                            break;
                        }

                        case TiledMapTileLayer.Cell.ROTATE180:
                        {
                            var tempU = this._vertices[ IBatch.U1 ];
                            this._vertices[ IBatch.U1 ] = this._vertices[ IBatch.U3 ];
                            this._vertices[ IBatch.U3 ] = tempU;

                            tempU                       = this._vertices[ IBatch.U2 ];
                            this._vertices[ IBatch.U2 ] = this._vertices[ IBatch.U4 ];
                            this._vertices[ IBatch.U4 ] = tempU;

                            var tempV = this._vertices[ IBatch.V1 ];
                            this._vertices[ IBatch.V1 ] = this._vertices[ IBatch.V3 ];
                            this._vertices[ IBatch.V3 ] = tempV;

                            tempV                       = this._vertices[ IBatch.V2 ];
                            this._vertices[ IBatch.V2 ] = this._vertices[ IBatch.V4 ];
                            this._vertices[ IBatch.V4 ] = tempV;

                            break;
                        }

                        case TiledMapTileLayer.Cell.ROTATE270:
                        {
                            var tempV = this._vertices[ IBatch.V1 ];
                            this._vertices[ IBatch.V1 ] = this._vertices[ IBatch.V4 ];
                            this._vertices[ IBatch.V4 ] = this._vertices[ IBatch.V3 ];
                            this._vertices[ IBatch.V3 ] = this._vertices[ IBatch.V2 ];
                            this._vertices[ IBatch.V2 ] = tempV;

                            var tempU = this._vertices[ IBatch.U1 ];
                            this._vertices[ IBatch.U1 ] = this._vertices[ IBatch.U4 ];
                            this._vertices[ IBatch.U4 ] = this._vertices[ IBatch.U3 ];
                            this._vertices[ IBatch.U3 ] = this._vertices[ IBatch.U2 ];
                            this._vertices[ IBatch.U2 ] = tempU;

                            break;
                        }
                    }
                }

                SpriteCache.Add( tile.TextureRegion.Texture, this._vertices, 0, NUM_VERTICES );
            }
        }
    }

    public void RenderImageLayer( TiledMapImageLayer layer )
    {
        var color = Color.ToFloatBits( 1.0f, 1.0f, 1.0f, layer.Opacity );

        if ( layer.TextureRegion == null )
        {
            return;
        }

        var x  = layer.X;
        var y  = layer.Y;
        var x1 = x * _unitScale;
        var y1 = y * _unitScale;
        var x2 = x1 + ( layer.TextureRegion.RegionWidth * _unitScale );
        var y2 = y1 + ( layer.TextureRegion.RegionHeight * _unitScale );

        var u1 = layer.TextureRegion.U;
        var v1 = layer.TextureRegion.V2;
        var u2 = layer.TextureRegion.U2;
        var v2 = layer.TextureRegion.V;

        this._vertices[ IBatch.X1 ] = x1;
        this._vertices[ IBatch.Y1 ] = y1;
        this._vertices[ IBatch.C1 ] = color;
        this._vertices[ IBatch.U1 ] = u1;
        this._vertices[ IBatch.V1 ] = v1;

        this._vertices[ IBatch.X2 ] = x1;
        this._vertices[ IBatch.Y2 ] = y2;
        this._vertices[ IBatch.C2 ] = color;
        this._vertices[ IBatch.U2 ] = u1;
        this._vertices[ IBatch.V2 ] = v2;

        this._vertices[ IBatch.X3 ] = x2;
        this._vertices[ IBatch.Y3 ] = y2;
        this._vertices[ IBatch.C3 ] = color;
        this._vertices[ IBatch.U3 ] = u2;
        this._vertices[ IBatch.V3 ] = v2;

        this._vertices[ IBatch.X4 ] = x2;
        this._vertices[ IBatch.Y4 ] = y1;
        this._vertices[ IBatch.C4 ] = color;
        this._vertices[ IBatch.U4 ] = u2;
        this._vertices[ IBatch.V4 ] = v1;

        SpriteCache.Add( layer.TextureRegion.Texture, this._vertices, 0, NUM_VERTICES );
    }

    /// <summary>
    /// Causes the cache to be rebuilt the next time it is rendered.
    /// </summary>
    public void InvalidateCache()
    {
        IsCached = false;
    }

    /// <summary>
    /// Expands the view size in each direction, ensuring that tiles of this
    /// size or smaller are never culled from the visible portion of the view.
    /// Default is 0,0.
    /// <para>
    /// The amount of tiles cached is computed using:-
    /// <code>(view size + max tile size) * overCache</code>
    /// meaning the max tile size increases the amount cached and possibly
    /// <see cref="OverCache"/>" can be reduced.
    /// </para>
    /// <para>
    /// If the view size and {@link #setOverCache(float)} are configured so the
    /// size of the cached tiles is always larger than the largest tile size,
    /// this setting is not needed.
    /// </para>
    /// </summary>
    public void SetMaxTileSize( float maxPixelWidth, float maxPixelHeight )
    {
        this._maxTileWidth  = maxPixelWidth;
        this._maxTileHeight = maxPixelHeight;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources, IF the value
    /// of <paramref name="disposing"/> is TRUE.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            SpriteCache.Dispose();
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }
}
