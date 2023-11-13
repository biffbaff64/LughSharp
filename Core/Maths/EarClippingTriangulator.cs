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

    /** Triangulates the given (convex or concave) simple polygon to a list of triangle vertices.
     * @param vertices pairs describing vertices of the polygon, in either clockwise or counterclockwise order.
     * @return triples of triangle indices in clockwise order. Note the returned array is reused for later calls to the same
     *         method. */
    public List< short > ComputeTriangles( float[] vertices, int offset, int count )
    {
        this._vertices = vertices;
        int vertexCount  = this._vertexCount = count / 2;
        int vertexOffset = offset / 2;

        List< short > indicesArray = this._indicesArray;

        indicesArray.Clear();
        indicesArray.EnsureCapacity( vertexCount );
        indicesArray.Count = vertexCount;
        
        var indices = this._indices = indicesArray.ToArray();

        if ( GeometryUtils.IsClockwise( vertices, offset, count ) )
        {
            for ( short i = 0; i < vertexCount; i++ )
            {
                indices[ i ] = ( short )( vertexOffset + i );
            }
        }
        else
        {
            for ( int i = 0, n = vertexCount - 1; i < vertexCount; i++ )
            {
                indices[ i ] = ( short )( vertexOffset + n - i ); // Reversed.
            }
        }

        List< int > vertexTypes = this._vertexTypes;
        vertexTypes.Clear();
        vertexTypes.EnsureCapacity( vertexCount );

        for ( int i = 0, n = vertexCount; i < n; ++i )
        {
            vertexTypes.Add( classifyVertex( i ) );
        }

        // A polygon with n vertices has a triangulation of n-2 triangles.
        List< short > triangles = this.triangles;
        triangles.Clear();
        triangles.EnsureCapacity( Math.max( 0, vertexCount - 2 ) * 3 );
        triangulate();

        return triangles;
    }

    private void triangulate()
    {
        int[] vertexTypes = this.vertexTypes.items;

        while ( vertexCount > 3 )
        {
            int earTipIndex = findEarTip();
            cutEarTip( earTipIndex );

            // The type of the two vertices adjacent to the clipped vertex may have changed.
            int previousIndex = previousIndex( earTipIndex );
            int nextIndex     = earTipIndex == vertexCount ? 0 : earTipIndex;
            vertexTypes[ previousIndex ] = classifyVertex( previousIndex );
            vertexTypes[ nextIndex ]     = classifyVertex( nextIndex );
        }

        if ( vertexCount == 3 )
        {
            List< short > triangles = this.triangles;
            short[]    indices   = this.indices;
            triangles.Add( indices[ 0 ] );
            triangles.Add( indices[ 1 ] );
            triangles.Add( indices[ 2 ] );
        }
    }

    /** @return {@link #CONCAVE} or {@link #CONVEX} */
    private int classifyVertex( int index )
    {
        short[] indices  = this.indices;
        int     previous = indices[ previousIndex( index ) ] * 2;
        int     current  = indices[ index ] * 2;
        int     next     = indices[ nextIndex( index ) ] * 2;
        float[] vertices = this.vertices;

        return ComputeSpannedAreaSign( vertices[ previous ],
                                       vertices[ previous + 1 ],
                                       vertices[ current ],
                                       vertices[ current + 1 ],
                                       vertices[ next ],
                                       vertices[ next + 1 ] );
    }

    private int findEarTip()
    {
        int vertexCount = this.vertexCount;

        for ( int i = 0; i < vertexCount; i++ )
        {
            if ( isEarTip( i ) )
            {
                return i;
            }
        }

        // Desperate mode: if no vertex is an ear tip, we are dealing with a degenerate polygon (e.g. nearly collinear).
        // Note that the input was not necessarily degenerate, but we could have made it so by clipping some valid ears.

        // Idea taken from Martin Held, "FIST: Fast industrial-strength triangulation of polygons", Algorithmica (1998),
        // http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.115.291

        // Return a convex or tangential vertex if one exists.
        int[] vertexTypes = this.vertexTypes.items;

        for ( int i = 0; i < vertexCount; i++ )
        {
            if ( vertexTypes[ i ] != CONCAVE )
            {
                return i;
            }
        }

        return 0; // If all vertices are concave, just return the first one.
    }

    private boolean isEarTip( int earTipIndex )
    {
        int[] vertexTypes = this.vertexTypes.items;

        if ( vertexTypes[ earTipIndex ] == CONCAVE )
        {
            return false;
        }

        int     previousIndex = previousIndex( earTipIndex );
        int     nextIndex     = nextIndex( earTipIndex );
        short[] indices       = this.indices;
        int     p1            = indices[ previousIndex ] * 2;
        int     p2            = indices[ earTipIndex ] * 2;
        int     p3            = indices[ nextIndex ] * 2;
        float[] vertices      = this.vertices;
        float   p1x           = vertices[ p1 ], p1y = vertices[ p1 + 1 ];
        float   p2x           = vertices[ p2 ], p2y = vertices[ p2 + 1 ];
        float   p3x           = vertices[ p3 ], p3y = vertices[ p3 + 1 ];

        // Check if any point is inside the triangle formed by previous, current and next vertices.
        // Only consider vertices that are not part of this triangle, or else we'll always find one inside.
        for ( int i = nextIndex( nextIndex ); i != previousIndex; i = nextIndex( i ) )
        {
            // Concave vertices can obviously be inside the candidate ear, but so can tangential vertices
            // if they coincide with one of the triangle's vertices.
            if ( vertexTypes[ i ] != CONVEX )
            {
                int   v  = indices[ i ] * 2;
                float vx = vertices[ v ];
                float vy = vertices[ v + 1 ];

                // Because the polygon has clockwise winding order, the area sign will be positive if the point is strictly inside.
                // It will be 0 on the edge, which we want to include as well.
                // note: check the edge defined by p1->p3 first since this fails _far_ more then the other 2 checks.
                if ( ComputeSpannedAreaSign( p3x, p3y, p1x, p1y, vx, vy ) >= 0 )
                {
                    if ( ComputeSpannedAreaSign( p1x, p1y, p2x, p2y, vx, vy ) >= 0 )
                    {
                        if ( ComputeSpannedAreaSign( p2x, p2y, p3x, p3y, vx, vy ) >= 0 )
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private void cutEarTip( int earTipIndex )
    {
        short[]    indices   = this.indices;
        List< short > triangles = this.triangles;

        triangles.Add( indices[ previousIndex( earTipIndex ) ] );
        triangles.Add( indices[ earTipIndex ] );
        triangles.Add( indices[ nextIndex( earTipIndex ) ] );

        indicesArray.removeIndex( earTipIndex );
        vertexTypes.removeIndex( earTipIndex );
        vertexCount--;
    }

    private int PreviousIndex( int index )
    {
        return ( index == 0 ? vertexCount : index ) - 1;
    }

    private int NextIndex( int index )
    {
        return ( index + 1 ) % vertexCount;
    }

    private static int ComputeSpannedAreaSign( float p1X, float p1Y, float p2X, float p2Y, float p3X, float p3Y )
    {
        var area = p1X * ( p3Y - p2Y );

        area += p2X * ( p1Y - p3Y );
        area += p3X * ( p2Y - p1Y );

        return Math.Sign( area );
    }
}
