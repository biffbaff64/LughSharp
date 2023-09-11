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

namespace LibGDXSharp.Maps.Tiled.Renderers;

[PublicAPI]
public class HexagonalTiledMapRenderer : BatchTileMapRenderer
{
    /// <summary>
    /// true for X-Axis, false for Y-Axis
    /// </summary>
    private bool _staggerAxisX = true;

    /// <summary>
    /// true for even StaggerIndex, false for odd
    /// </summary>
    private bool _staggerIndexEven = false;

    /// <summary>
    /// the parameter defining the shape of the hexagon from tiled. more
    /// specifically it represents the length of the sides that are parallel
    /// to the stagger axis. e.g. with respect to the stagger axis a value
    /// of 0 results in a rhombus shape, while a value equal to the tile
    /// length/height represents a square shape and a value of 0.5 represents
    /// a regular hexagon if tile length equals tile height 
    /// </summary>
    private float _hexSideLength = 0f;

    public HexagonalTiledMapRenderer( TiledMap map ) : base( map )
    {
        Init( map );
    }

    public HexagonalTiledMapRenderer( TiledMap map, float unitScale )
        : base( map, unitScale )
    {
        Init( map );
    }

    public HexagonalTiledMapRenderer( TiledMap map, IBatch batch )
        : base( map, batch )
    {
        Init( map );
    }

    public HexagonalTiledMapRenderer( TiledMap map, float unitScale, IBatch batch )
        : base( map, unitScale, batch )
    {
        Init( map );
    }

    private void Init( TiledMap map )
    {
        var axis = map.Properties.Get< string >( "staggeraxis" );

        if ( !string.ReferenceEquals( axis, null ) )
        {
            _staggerAxisX = axis.Equals( "x" );
        }

        var index = map.Properties.Get< string >( "staggerindex" );

        if ( !string.ReferenceEquals( index, null ) )
        {
            _staggerIndexEven = index.Equals( "even" );
        }

        int? length = map.Properties.Get< int >( "hexsidelength" );

        if ( length != null )
        {
            _hexSideLength = length.Value;
        }
        else
        {
            if ( _staggerAxisX )
            {
                length = map.Properties.Get< int >( "tilewidth" );

                if ( length != null )
                {
                    _hexSideLength = 0.5f * length.Value;
                }
                else
                {
                    var tmtl = ( TiledMapTileLayer )map.Layers.Get( 0 );
                    _hexSideLength = 0.5f * tmtl.TileWidth;
                }
            }
            else
            {
                length = map.Properties.Get< int >( "tileheight" );

                if ( length != null )
                {
                    _hexSideLength = 0.5f * length.Value;
                }
                else
                {
                    var tmtl = ( TiledMapTileLayer )map.Layers.Get( 0 );
                    _hexSideLength = 0.5f * tmtl.TileHeight;
                }
            }
        }
    }

    public override void RenderTileLayer( TiledMapTileLayer layer )
    {
        var color = Color.ToFloatBits( Batch.Color.R, Batch.Color.G, Batch.Color.B, Batch.Color.A * layer.Opacity );

        var layerWidth  = layer.Width;
        var layerHeight = layer.Height;

        var layerTileWidth  = layer.TileWidth * UnitScale;
        var layerTileHeight = layer.TileHeight * UnitScale;

        var layerOffsetX = layer.RenderOffsetX * UnitScale;

        // offset in tiled is y down, so we flip it
        var layerOffsetY = -layer.RenderOffsetY * UnitScale;

        var layerHexLength = _hexSideLength * UnitScale;

        if ( _staggerAxisX )
        {
            var tileWidthLowerCorner = ( layerTileWidth - layerHexLength ) / 2;
            var tileWidthUpperCorner = ( layerTileWidth + layerHexLength ) / 2;
            var layerTileHeight50    = layerTileHeight * 0.50f;

            var row1 = Math.Max( 0, ( int )( ( ViewBounds.Y - layerTileHeight50 - layerOffsetX ) / layerTileHeight ) );

            var row2 = Math.Min
                (
                layerHeight,
                ( int )( ( ( ViewBounds.Y + ViewBounds.Height + layerTileHeight ) - layerOffsetX ) / layerTileHeight )
                );

            var col1 = Math.Max( 0, ( int )( ( ( ViewBounds.X - tileWidthLowerCorner - layerOffsetY ) / tileWidthUpperCorner ) ) );

            var col2 = Math.Min
                (
                layerWidth,
                ( int )( ( ( ViewBounds.X + ViewBounds.Width + tileWidthUpperCorner ) - layerOffsetY )
                       / tileWidthUpperCorner )
                );

            // depending on the stagger index either draw all even before the odd or vice versa
            var colA = ( _staggerIndexEven == ( ( col1 % 2 ) == 0 ) ) ? col1 + 1 : col1;
            var colB = ( _staggerIndexEven == ( ( col1 % 2 ) == 0 ) ) ? col1 : col1 + 1;

            for ( var row = row2 - 1; row >= row1; row-- )
            {
                for ( var col = colA; col < col2; col += 2 )
                {
                    RenderCell
                        (
                        layer.GetCell( col, row ),
                        ( tileWidthUpperCorner * col ) + layerOffsetX,
                        layerTileHeight50 + ( layerTileHeight * row ) + layerOffsetY,
                        color
                        );
                }

                for ( var col = colB; col < col2; col += 2 )
                {
                    RenderCell
                        (
                        layer.GetCell( col, row ),
                        ( tileWidthUpperCorner * col ) + layerOffsetX,
                        ( layerTileHeight * row ) + layerOffsetY,
                        color
                        );
                }
            }
        }
        else
        {
            var tileHeightLowerCorner = ( layerTileHeight - layerHexLength ) / 2;
            var tileHeightUpperCorner = ( layerTileHeight + layerHexLength ) / 2;
            var layerTileWidth50      = layerTileWidth * 0.50f;

            var row1 = Math.Max
                ( 0, ( int )( ( ( ViewBounds.Y - tileHeightLowerCorner - layerOffsetX ) / tileHeightUpperCorner ) ) );

            var row2 = Math.Min
                (
                layerHeight,
                ( int )( ( ( ViewBounds.Y + ViewBounds.Height + tileHeightUpperCorner ) - layerOffsetX )
                       / tileHeightUpperCorner )
                );

            var col1 = Math.Max
                ( 0, ( int )( ( ( ViewBounds.X - layerTileWidth50 - layerOffsetY ) / layerTileWidth ) ) );

            var col2 = Math.Min
                (
                layerWidth,
                ( int )( ( ( ViewBounds.X + ViewBounds.Width + layerTileWidth ) - layerOffsetY ) / layerTileWidth )
                );

            for ( var row = row2 - 1; row >= row1; row-- )
            {
                // depending on the stagger index either shift for even or uneven indexes
                var shiftX = ( ( row % 2 ) == 0 ) == _staggerIndexEven ? layerTileWidth50 : 0;

                for ( var col = col1; col < col2; col++ )
                {
                    RenderCell
                        (
                        layer.GetCell( col, row ),
                        ( layerTileWidth * col ) + shiftX + layerOffsetX,
                        ( tileHeightUpperCorner * row ) + layerOffsetY,
                        color
                        );
                }
            }
        }
    }

    private void RenderCell( in TiledMapTileLayer.Cell? cell, in float x, in float y, in float color )
    {
        ArgumentNullException.ThrowIfNull( cell );

        ITiledMapTile? tile = cell.GetTile();

        if ( tile != null )
        {
            if ( tile is AnimatedTiledMapTile )
            {
                return;
            }

            var flipX     = cell.GetFlipHorizontally();
            var flipY     = cell.GetFlipVertically();
            var rotations = cell.GetRotation();

            TextureRegion region = tile.TextureRegion;

            var x1 = x + ( tile.OffsetX * UnitScale );
            var y1 = y + ( tile.OffsetY * UnitScale );
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

            if ( flipX )
            {
                ( Vertices[ IBatch.U1 ], Vertices[ IBatch.U3 ] )
                    = ( Vertices[ IBatch.U3 ], Vertices[ IBatch.U1 ] );

                ( Vertices[ IBatch.U2 ], Vertices[ IBatch.U4 ] )
                    = ( Vertices[ IBatch.U4 ], Vertices[ IBatch.U2 ] );
            }

            if ( flipY )
            {
                ( Vertices[ IBatch.V1 ], Vertices[ IBatch.V3 ] )
                    = ( Vertices[ IBatch.V3 ], Vertices[ IBatch.V1 ] );

                ( Vertices[ IBatch.V2 ], Vertices[ IBatch.V4 ] )
                    = ( Vertices[ IBatch.V4 ], Vertices[ IBatch.V2 ] );
            }

            if ( rotations == 2 )
            {
                ( Vertices[ IBatch.U1 ], Vertices[ IBatch.U3 ] )
                    = ( Vertices[ IBatch.U3 ], Vertices[ IBatch.U1 ] );

                ( Vertices[ IBatch.U2 ], Vertices[ IBatch.U4 ] )
                    = ( Vertices[ IBatch.U4 ], Vertices[ IBatch.U2 ] );

                ( Vertices[ IBatch.V1 ], Vertices[ IBatch.V3 ] )
                    = ( Vertices[ IBatch.V3 ], Vertices[ IBatch.V1 ] );

                ( Vertices[ IBatch.V2 ], Vertices[ IBatch.V4 ] )
                    = ( Vertices[ IBatch.V4 ], Vertices[ IBatch.V2 ] );
            }

            Batch.Draw( region.Texture, Vertices, 0, NUM_VERTICES );
        }
    }
}
