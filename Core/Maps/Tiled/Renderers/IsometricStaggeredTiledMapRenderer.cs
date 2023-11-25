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
public class IsometricStaggeredTiledMapRenderer : BatchTileMapRenderer
{
    public IsometricStaggeredTiledMapRenderer( TiledMap map )
        : base( map )
    {
    }

    public IsometricStaggeredTiledMapRenderer( TiledMap map, IBatch batch )
        : base( map, batch )
    {
    }

    public IsometricStaggeredTiledMapRenderer( TiledMap map, float unitScale )
        : base( map, unitScale )
    {
    }

    public IsometricStaggeredTiledMapRenderer( TiledMap map, float unitScale, IBatch batch )
        : base( map, unitScale, batch )
    {
    }

    public override void RenderTileLayer( TiledMapTileLayer layer )
    {
        var color = Color.ToFloatBits( Batch.Color.R, Batch.Color.G, Batch.Color.B, Batch.Color.A * layer.Opacity );

        var layerOffsetX = layer.RenderOffsetX * UnitScale;

        // offset in tiled is y down, so we flip it
        var layerOffsetY = -layer.RenderOffsetY * UnitScale;

        var layerTileWidth  = layer.TileWidth * UnitScale;
        var layerTileHeight = layer.TileHeight * UnitScale;

        var layerTileWidth50  = layerTileWidth * 0.50f;
        var layerTileHeight50 = layerTileHeight * 0.50f;

        var minX = Math.Max( 0, ( int )( ( ( ViewBounds.X - layerTileWidth50 - layerOffsetX ) / layerTileWidth ) ) );

        var maxX = Math.Min( layer.Width,
                             ( int )( ( ( ViewBounds.X + ViewBounds.Width + layerTileWidth + layerTileWidth50 ) - layerOffsetX ) / layerTileWidth ) );

        var minY = Math.Max( 0, ( int )( ( ( ViewBounds.Y - layerTileHeight - layerOffsetY ) / layerTileHeight ) ) );

        var maxY = Math.Min( layer.Height,
                             ( int )( ( ( ViewBounds.Y + ViewBounds.Height + layerTileHeight ) - layerOffsetY ) / layerTileHeight50 ) );

        for ( var y = maxY - 1; y >= minY; y-- )
        {
            var offsetX = ( ( y % 2 ) == 1 ) ? layerTileWidth50 : 0;

            for ( var x = maxX - 1; x >= minX; x-- )
            {
                TiledMapTileLayer.Cell? cell = layer.GetCell( x, y );

                if ( cell == null )
                {
                    continue;
                }

                ITiledMapTile? tile = cell.GetTile();

                if ( tile != null )
                {
                    var flipX     = cell.GetFlipHorizontally();
                    var flipY     = cell.GetFlipVertically();
                    var rotations = cell.GetRotation();

                    var x1 = ( ( x * layerTileWidth ) - offsetX ) + ( tile.OffsetX * UnitScale ) + layerOffsetX;
                    var y1 = ( y * layerTileHeight50 ) + ( tile.OffsetY * UnitScale ) + layerOffsetY;
                    var x2 = x1 + ( tile.TextureRegion.RegionWidth * UnitScale );
                    var y2 = y1 + ( tile.TextureRegion.RegionHeight * UnitScale );

                    Vertices[ IBatch.X1 ] = x1;
                    Vertices[ IBatch.Y1 ] = y1;
                    Vertices[ IBatch.C1 ] = color;
                    Vertices[ IBatch.U1 ] = tile.TextureRegion.U;
                    Vertices[ IBatch.V1 ] = tile.TextureRegion.V2;

                    Vertices[ IBatch.X2 ] = x1;
                    Vertices[ IBatch.Y2 ] = y2;
                    Vertices[ IBatch.C2 ] = color;
                    Vertices[ IBatch.U2 ] = tile.TextureRegion.U;
                    Vertices[ IBatch.V2 ] = tile.TextureRegion.V;

                    Vertices[ IBatch.X3 ] = x2;
                    Vertices[ IBatch.Y3 ] = y2;
                    Vertices[ IBatch.C3 ] = color;
                    Vertices[ IBatch.U3 ] = tile.TextureRegion.U2;
                    Vertices[ IBatch.V3 ] = tile.TextureRegion.V;

                    Vertices[ IBatch.X4 ] = x2;
                    Vertices[ IBatch.Y4 ] = y1;
                    Vertices[ IBatch.C4 ] = color;
                    Vertices[ IBatch.U4 ] = tile.TextureRegion.U2;
                    Vertices[ IBatch.V4 ] = tile.TextureRegion.V2;

                    if ( flipX )
                    {
                        ( Vertices[ IBatch.U1 ], Vertices[ IBatch.U3 ] ) = ( Vertices[ IBatch.U3 ], Vertices[ IBatch.U1 ] );
                        ( Vertices[ IBatch.U2 ], Vertices[ IBatch.U4 ] ) = ( Vertices[ IBatch.U4 ], Vertices[ IBatch.U2 ] );
                    }

                    if ( flipY )
                    {
                        ( Vertices[ IBatch.V1 ], Vertices[ IBatch.V3 ] ) = ( Vertices[ IBatch.V3 ], Vertices[ IBatch.V1 ] );
                        ( Vertices[ IBatch.V2 ], Vertices[ IBatch.V4 ] ) = ( Vertices[ IBatch.V4 ], Vertices[ IBatch.V2 ] );
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

                    Batch.Draw( tile.TextureRegion.Texture, Vertices, 0, NUM_VERTICES );
                }
            }
        }
    }
}
