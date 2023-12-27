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

namespace LibGDXSharp.Graphics.G2D;

/// <summary>
/// Renders polygon filled with a repeating TextureRegion with specified
/// density without causing an additional flush or render call
/// </summary>
[PublicAPI]
public class RepeatablePolygonSprite
{
    public float X { get; set; } = 0;
    public float Y { get; set; } = 0;

    private TextureRegion?   _region;
    private float            _density;
    private List< float[]? > _parts    = new();
    private List< float[] >  _vertices = new();
    private List< short[] >  _indices  = new();
    private Color            _color    = Color.White;
    private Vector2          _offset   = new();
    private bool             _dirty    = true;
    private int              _cols;
    private int              _rows;
    private float            _gridWidth;
    private float            _gridHeight;

    /// <summary>
    /// Sets polygon with repeating texture region, the size of
    /// repeating grid is equal to region size
    /// </summary>
    /// <param name="region"> region to repeat </param>
    /// <param name="vertices"> cw vertices of polygon </param>
    public void SetPolygon( TextureRegion region, float[]? vertices )
    {
        SetPolygon( region, vertices, -1 );
    }

    /// <summary>
    /// Sets polygon with repeating texture region, the size of repeating
    /// grid is equal to region size
    /// </summary>
    /// <param name="region"> region to repeat </param>
    /// <param name="vertices"> cw vertices of polygon </param>
    /// <param name="density"> number of regions per polygon width bound </param>
    public void SetPolygon( TextureRegion region, float[]? vertices, float density )
    {
        this._region = region;

        vertices = Offset( vertices );

        var polygon          = new Polygon( vertices );
        var tmpPoly          = new Polygon();
        var intersectionPoly = new Polygon();
        var triangulator     = new EarClippingTriangulator();

        RectangleShape boundRect = polygon.BoundingRectangle;

        if ( density is -1 )
        {
            density = boundRect.Width / region.RegionWidth;
        }

        var regionAspectRatio = region.RegionHeight / region.RegionWidth;

        _cols       = ( int )( Math.Ceiling( density ) );
        _gridWidth  = boundRect.Width / density;
        _gridHeight = regionAspectRatio * _gridWidth;
        _rows       = ( int )Math.Ceiling( boundRect.Height / _gridHeight );

        for ( var col = 0; col < _cols; col++ )
        {
            for ( var row = 0; row < _rows; row++ )
            {
                var verts = new float[ 8 ];

                var idx = 0;

                verts[ idx++ ] = col * _gridWidth;
                verts[ idx++ ] = row * _gridHeight;
                verts[ idx++ ] = ( col ) * _gridWidth;
                verts[ idx++ ] = ( row + 1 ) * _gridHeight;
                verts[ idx++ ] = ( col + 1 ) * _gridWidth;
                verts[ idx++ ] = ( row + 1 ) * _gridHeight;
                verts[ idx++ ] = ( col + 1 ) * _gridWidth;
                verts[ idx ]   = ( row ) * _gridHeight;

                tmpPoly.Vertices = verts;

                Intersector.IntersectPolygons( polygon, tmpPoly, intersectionPoly );
                verts = intersectionPoly.Vertices;

                if ( verts?.Length > 0 )
                {
                    _parts.Add( SnapToGrid( verts ) );

                    List< short > arr = triangulator.ComputeTriangles( verts );

                    _indices.Add( arr.ToArray() );
                }
                else
                {
                    // adding null for key consistancy, needed to get col/row from key
                    // the other alternative is to make parts - IntMap<FloatArray>
                    _parts.Add( null );
                }
            }
        }

        BuildVertices();
    }

    /// <summary>
    /// This is garbage, due to Intersector returning values slightly
    /// different then the grid values Snapping exactly to grid is important,
    /// so that during bulidVertices method, it can be figured out if points
    /// is on the wall of it's own grid box or not, to set u/v properly.
    /// Any other implementations are welcome
    /// </summary>
    private float[]? SnapToGrid( float[]? vertices )
    {
        for ( var i = 0; i < vertices?.Length; i += 2 )
        {
            var numX = ( vertices[ i ] / _gridWidth ) % 1;
            var numY = ( vertices[ i + 1 ] / _gridHeight ) % 1;

            if ( ( numX > 0.99f ) || ( numX < 0.01f ) )
            {
                vertices[ i ] = ( float )( _gridWidth * Math.Round( vertices[ i ] / _gridWidth ) );
            }

            if ( ( numY > 0.99f ) || ( numY < 0.01f ) )
            {
                vertices[ i + 1 ] = ( float )( _gridHeight * Math.Round( vertices[ i + 1 ] / _gridHeight ) );
            }
        }

        return vertices;
    }

    /// <summary>
    /// Offsets polygon to 0 coordinate for ease of calculations, later
    /// offset is put back on final render.
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns> offsetted vertices </returns>
    private float[] Offset( float[]? vertices )
    {
        ArgumentNullException.ThrowIfNull( vertices );

        _offset.Set( vertices[ 0 ], vertices[ 1 ] );

        for ( var i = 0; i < ( vertices.Length - 1 ); i += 2 )
        {
            if ( _offset.X > vertices[ i ] )
            {
                _offset.X = vertices[ i ];
            }

            if ( _offset.Y > vertices[ i + 1 ] )
            {
                _offset.Y = vertices[ i + 1 ];
            }
        }

        for ( var i = 0; i < vertices.Length; i += 2 )
        {
            vertices[ i ]     -= _offset.X;
            vertices[ i + 1 ] -= _offset.Y;
        }

        return vertices;
    }

    /// <summary>
    /// Builds final vertices with vertex attributes like coordinates,
    /// color and region u/v
    /// </summary>
    private void BuildVertices()
    {
        GdxRuntimeException.ThrowIfNull( _region );

        _vertices.Clear();

        for ( var i = 0; i < _parts.Count; i++ )
        {
            var verts = _parts[ i ];

            if ( verts == null )
            {
                continue;
            }

            var fullVerts = new float[ ( 5 * verts.Length ) / 2 ];
            var idx       = 0;

            var col = i / _rows;
            var row = i % _rows;

            for ( var j = 0; j < verts.Length; j += 2 )
            {
                fullVerts[ idx++ ] = verts[ j ] + _offset.X + X;
                fullVerts[ idx++ ] = verts[ j + 1 ] + _offset.Y + Y;

                fullVerts[ idx++ ] = _color.ToFloatBits();

                var u = ( verts[ j ] % _gridWidth ) / _gridWidth;
                var v = ( verts[ j + 1 ] % _gridHeight ) / _gridHeight;

                if ( verts[ j ].Equals( col * _gridWidth ) )
                {
                    u = 0f;
                }

                if ( verts[ j ].Equals( ( col + 1 ) * _gridWidth ) )
                {
                    u = 1f;
                }

                if ( verts[ j + 1 ].Equals( row * _gridHeight ) )
                {
                    v = 0f;
                }

                if ( verts[ j + 1 ].Equals( ( row + 1 ) * _gridHeight ) )
                {
                    v = 1f;
                }

                u = _region.U + ( ( _region.U2 - _region.U ) * u );
                v = _region.V + ( ( _region.V2 - _region.V ) * v );

                fullVerts[ idx++ ] = u;
                fullVerts[ idx++ ] = v;
            }

            _vertices.Add( fullVerts );
        }

        _dirty = false;
    }

    public void Draw( PolygonSpriteBatch batch )
    {
        GdxRuntimeException.ThrowIfNull( _region );
        
        if ( _dirty )
        {
            BuildVertices();
        }

        for ( var i = 0; i < _vertices.Count; i++ )
        {
            batch.Draw
                (
                 _region.Texture,
                 _vertices[ i ],
                 0,
                 _vertices[ i ].Length,
                 _indices[ i ],
                 0,
                 _indices[ i ].Length
                );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="color"> Tint color to be applied to entire polygon </param>
    public void SetColor( Color color )
    {
        this._color = color;
        _dirty      = true;
    }

    public void SetPosition( float x, float y )
    {
        this.X = x;
        this.Y = y;
        _dirty = true;
    }
}
