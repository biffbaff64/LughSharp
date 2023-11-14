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

namespace LibGDXSharp.Maths;

[PublicAPI]
public class EarClippingTriangulator
{
    private const int CONCAVE = -1;
    private const int CONVEX  = 1;

    private List< short > _indicesArray = new();
    private List< int >   _vertexTypes  = new();
    private List< short > _triangles    = new();
    private short[]?      _indices;
    private float[]?      _vertices;
    private int           _vertexCount;

    public List< short > ComputeTriangles( List< float > vertices )
    {
        return ComputeTriangles( vertices.ToArray(), 0, vertices.Count );
    }

    public List< short > ComputeTriangles( float[] vertices )
    {
        return ComputeTriangles( vertices, 0, vertices.Length );
    }

    /// <summary>
    /// Triangulates the given (convex or concave) simple polygon to a
    /// list of triangle vertices.
    /// </summary>
    /// <param name="vertices">
    /// pairs describing vertices of the polygon, in either clockwise
    /// or counterclockwise order.
    /// </param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns>
    /// triples of triangle indices in clockwise order. Note the returned
    /// array is reused for later calls to the same method.
    /// </returns>
    public List< short > ComputeTriangles( float[] vertices, int offset, int count )
    {
        this._vertices = vertices;

        var vertexCount  = this._vertexCount = count / 2;
        var vertexOffset = offset / 2;

        this._indicesArray.Clear();
        this._indicesArray.EnsureCapacity( vertexCount );

        if ( GeometryUtils.IsClockwise( vertices, offset, count ) )
        {
            for ( short i = 0; i < vertexCount; i++ )
            {
                this._indicesArray[ i ] = ( short )( vertexOffset + i );
            }
        }
        else
        {
            for ( int i = 0, n = vertexCount - 1; i < vertexCount; i++ )
            {
                this._indicesArray[ i ] = ( short )( ( vertexOffset + n ) - i ); // Reversed.
            }
        }

        this._indices = this._indicesArray.ToArray();

        this._vertexTypes.Clear();
        this._vertexTypes.EnsureCapacity( vertexCount );

        for ( int i = 0, n = vertexCount; i < n; ++i )
        {
            this._vertexTypes.Add( ClassifyVertex( i ) );
        }

        // A polygon with n vertices has a triangulation of n-2 triangles.
        List< short > triangles = this._triangles;
        triangles.Clear();
        triangles.EnsureCapacity( Math.Max( 0, vertexCount - 2 ) * 3 );

        Triangulate();

        return triangles;
    }

    private void Triangulate()
    {
//        var vertexTypes = this._vertexTypes.ToArray();

        while ( _vertexCount > 3 )
        {
            var earTipIndex = FindEarTip();
            CutEarTip( earTipIndex );

            // The type of the two vertices adjacent to the clipped vertex may have changed.
            var previousIndex = PreviousIndex( earTipIndex );
            var nextIndex     = earTipIndex == _vertexCount ? 0 : earTipIndex;

            this._vertexTypes[ previousIndex ] = ClassifyVertex( previousIndex );
            this._vertexTypes[ nextIndex ]     = ClassifyVertex( nextIndex );
        }

        if ( _vertexCount == 3 )
        {
            this._triangles.Add( this._indices![ 0 ] );
            this._triangles.Add( this._indices[ 1 ] );
            this._triangles.Add( this._indices[ 2 ] );
        }
    }

    /** @return {@link #CONCAVE} or {@link #CONVEX} */
    private int ClassifyVertex( int index )
    {
        var previous = this._indices![ PreviousIndex( index ) ] * 2;
        var current  = this._indices[ index ] * 2;
        var next     = this._indices[ NextIndex( index ) ] * 2;

        return ComputeSpannedAreaSign( this._vertices![ previous ],
                                       this._vertices[ previous + 1 ],
                                       this._vertices[ current ],
                                       this._vertices[ current + 1 ],
                                       this._vertices[ next ],
                                       this._vertices[ next + 1 ] );
    }

    private int FindEarTip()
    {
        for ( var i = 0; i < this._vertexCount; i++ )
        {
            if ( IsEarTip( i ) )
            {
                return i;
            }
        }

        // Desperate mode: if no vertex is an ear tip, we are dealing with a degenerate
        // polygon (e.g. nearly collinear).
        // Note that the input was not necessarily degenerate, but we could have made
        // it so by clipping some valid ears.

        // Idea taken from Martin Held, "FIST: Fast industrial-strength triangulation
        // of polygons", Algorithmica (1998),
        // http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.115.291

        // Return a convex or tangential vertex if one exists.
        for ( var i = 0; i < this._vertexCount; i++ )
        {
            if ( this._vertexTypes[ i ] != CONCAVE )
            {
                return i;
            }
        }

        return 0; // If all vertices are concave, just return the first one.
    }

    private bool IsEarTip( int earTipIndex )
    {
        if ( this._vertexTypes[ earTipIndex ] == CONCAVE )
        {
            return false;
        }

        var previousIndex = PreviousIndex( earTipIndex );
        var nextIndex     = NextIndex( earTipIndex );
        var p1            = this._indices![ previousIndex ] * 2;
        var p2            = this._indices[ earTipIndex ] * 2;
        var p3            = this._indices[ nextIndex ] * 2;
        var p1X           = this._vertices![ p1 ];
        var p1Y           = this._vertices[ p1 + 1 ];
        var p2X           = this._vertices[ p2 ];
        var p2Y           = this._vertices[ p2 + 1 ];
        var p3X           = this._vertices[ p3 ];
        var p3Y           = this._vertices[ p3 + 1 ];

        // Check if any point is inside the triangle formed by previous, current and next vertices.
        // Only consider vertices that are not part of this triangle, or else we'll always find one inside.
        for ( var i = NextIndex( nextIndex ); i != previousIndex; i = NextIndex( i ) )
        {
            // Concave vertices can obviously be inside the candidate ear, but so can tangential vertices
            // if they coincide with one of the triangle's vertices.
            if ( this._vertexTypes[ i ] != CONVEX )
            {
                var v  = this._indices[ i ] * 2;
                var vx = this._vertices[ v ];
                var vy = this._vertices[ v + 1 ];

                // Because the polygon has clockwise winding order, the area sign will be positive if the point is strictly inside.
                // It will be 0 on the edge, which we want to include as well.
                // note: check the edge defined by p1->p3 first since this fails _far_ more then the other 2 checks.
                if ( ComputeSpannedAreaSign( p3X, p3Y, p1X, p1Y, vx, vy ) >= 0 )
                {
                    if ( ComputeSpannedAreaSign( p1X, p1Y, p2X, p2Y, vx, vy ) >= 0 )
                    {
                        if ( ComputeSpannedAreaSign( p2X, p2Y, p3X, p3Y, vx, vy ) >= 0 )
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private void CutEarTip( int earTipIndex )
    {
        this._triangles.Add( this._indices![ PreviousIndex( earTipIndex ) ] );
        this._triangles.Add( this._indices[ earTipIndex ] );
        this._triangles.Add( this._indices[ NextIndex( earTipIndex ) ] );

        this._indicesArray.RemoveAt( earTipIndex );
        this._vertexTypes.RemoveAt( earTipIndex );
        this._vertexCount--;
    }

    private int PreviousIndex( int index )
    {
        return ( index == 0 ? this._vertexCount : index ) - 1;
    }

    private int NextIndex( int index )
    {
        return ( index + 1 ) % this._vertexCount;
    }

    private static int ComputeSpannedAreaSign( float p1X,
                                               float p1Y,
                                               float p2X,
                                               float p2Y,
                                               float p3X,
                                               float p3Y )
    {
        var area = p1X * ( p3Y - p2Y );

        area += p2X * ( p1Y - p3Y );
        area += p3X * ( p2Y - p1Y );

        return Math.Sign( area );
    }
}
