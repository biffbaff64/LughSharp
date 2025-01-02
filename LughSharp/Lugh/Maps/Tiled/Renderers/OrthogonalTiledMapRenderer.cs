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

using LughSharp.Lugh.Graphics;
using LughSharp.Lugh.Graphics.G2D;

namespace LughSharp.Lugh.Maps.Tiled.Renderers;

[PublicAPI]
public class OrthogonalTiledMapRenderer : BatchTileMapRenderer
{
    public OrthogonalTiledMapRenderer( TiledMap map )
        : base( map )
    {
    }

    public OrthogonalTiledMapRenderer( TiledMap map, IBatch batch )
        : base( map, batch )
    {
    }

    public OrthogonalTiledMapRenderer( TiledMap map, float unitScale )
        : base( map, unitScale )
    {
    }

    public OrthogonalTiledMapRenderer( TiledMap map, float unitScale, IBatch batch )
        : base( map, unitScale, batch )
    {
    }

    public override void RenderTileLayer( TiledMapTileLayer layer )
    {
        var batchColor = Batch.Color;
        var color      = Color.ToFloatBitsABGR( batchColor.R, batchColor.G, batchColor.B, batchColor.A * layer.Opacity );

        var layerWidth  = layer.Width;
        var layerHeight = layer.Height;

        var layerTileWidth  = layer.TileWidth * UnitScale;
        var layerTileHeight = layer.TileHeight * UnitScale;
        var layerOffsetX    = layer.RenderOffsetX * UnitScale;

        // offset in tiled is y down, so we flip it
        var layerOffsetY = -layer.RenderOffsetY * UnitScale;

        var col1 = Math.Max( 0, ( int ) ( ( ViewBounds.X - layerOffsetX ) / layerTileWidth ) );

        var col2 = Math.Min( layerWidth,
                             ( int ) ( ( ( ViewBounds.X + ViewBounds.Width + layerTileWidth ) - layerOffsetX ) / layerTileWidth ) );

        var row1 = Math.Max( 0, ( int ) ( ( ViewBounds.Y - layerOffsetY ) / layerTileHeight ) );

        var row2 = Math.Min( layerHeight,
                             ( int ) ( ( ( ViewBounds.Y + ViewBounds.Height + layerTileHeight ) - layerOffsetY ) / layerTileHeight ) );

        var y      = ( row2 * layerTileHeight ) + layerOffsetY;
        var xStart = ( col1 * layerTileWidth ) + layerOffsetX;

        for ( var row = row2; row >= row1; row-- )
        {
            var x = xStart;

            for ( var col = col1; col < col2; col++ )
            {
                var cell = layer?.GetCell( col, row );

                if ( cell == null )
                {
                    x += layerTileWidth;

                    continue;
                }

                var tile = cell.GetTile();

                if ( tile != null )
                {
                    var flipX     = cell.GetFlipHorizontally();
                    var flipY     = cell.GetFlipVertically();
                    var rotations = cell.GetRotation();

                    var region = tile.TextureRegion;

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
                        ( Vertices[ IBatch.U1 ], Vertices[ IBatch.U3 ] ) =
                            ( Vertices[ IBatch.U3 ], Vertices[ IBatch.U1 ] );

                        ( Vertices[ IBatch.U2 ], Vertices[ IBatch.U4 ] ) =
                            ( Vertices[ IBatch.U4 ], Vertices[ IBatch.U2 ] );
                    }

                    if ( flipY )
                    {
                        ( Vertices[ IBatch.V1 ], Vertices[ IBatch.V3 ] ) =
                            ( Vertices[ IBatch.V3 ], Vertices[ IBatch.V1 ] );

                        ( Vertices[ IBatch.V2 ], Vertices[ IBatch.V4 ] ) =
                            ( Vertices[ IBatch.V4 ], Vertices[ IBatch.V2 ] );
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
                                ( Vertices[ IBatch.U1 ], Vertices[ IBatch.U3 ] )
                                    = ( Vertices[ IBatch.U3 ], Vertices[ IBatch.U1 ] );

                                ( Vertices[ IBatch.U2 ], Vertices[ IBatch.U4 ] )
                                    = ( Vertices[ IBatch.U4 ], Vertices[ IBatch.U2 ] );

                                ( Vertices[ IBatch.V1 ], Vertices[ IBatch.V3 ] )
                                    = ( Vertices[ IBatch.V3 ], Vertices[ IBatch.V1 ] );

                                ( Vertices[ IBatch.V2 ], Vertices[ IBatch.V4 ] )
                                    = ( Vertices[ IBatch.V4 ], Vertices[ IBatch.V2 ] );

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

                    Batch.Draw( region.Texture, Vertices, 0, NUM_VERTICES );
                }

                x += layerTileWidth;
            }

            y -= layerTileHeight;
        }
    }
}
