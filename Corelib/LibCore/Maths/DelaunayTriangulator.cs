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

using Corelib.LibCore.Utils.Collections;

namespace Corelib.LibCore.Maths;

/// <summary>
/// Delaunay triangulation. Adapted from Paul Bourke's triangulate:
/// http://paulbourke.net/papers/triangulate/
/// </summary>
[PublicAPI]
public class DelaunayTriangulator
{
    private const float EPSILON    = 0.000001f;
    private const int   INSIDE     = 0;
    private const int   COMPLETE   = 1;
    private const int   INCOMPLETE = 2;

    // ========================================================================

    private readonly Vector2      _centroid        = new();
    private readonly List< bool > _complete        = new( 16 );
    private readonly List< int >  _edges           = [ ];
    private readonly List< int >  _originalIndices = [ ];
    private readonly List< int >  _quicksortStack  = [ ];
    private readonly float[]      _superTriangle   = new float[ 6 ];
    private readonly List< int >  _triangles       = new( 16 );

    private float[]? _sortedPoints;

    // ========================================================================

    public List< int > ComputeTriangles( List< float > points, bool sorted )
    {
        return ComputeTriangles( points.ToArray(), 0, points.Count, sorted );
    }

    public List< int > ComputeTriangles( float[] polygon, bool sorted )
    {
        return ComputeTriangles( polygon, 0, polygon.Length, sorted );
    }

    // ========================================================================

    /// <summary>
    /// Triangulates the given point cloud to a list of triangle indices that
    /// make up the Delaunay triangulation.
    /// </summary>
    /// <param name="points">
    /// x,y pairs describing points. Duplicate points will result in undefined behavior.
    /// </param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <param name="sorted">
    /// If false, the points will be sorted by the x coordinate, which is required
    /// by the triangulation algorithm. If sorting is done the input array is not
    /// modified, the returned indices are for the input array, and count*2
    /// additional working memory is needed.
    /// </param>
    /// <returns>
    /// triples of indices into the points that describe the triangles in clockwise
    /// order. Note the returned array is reused for later calls to the same method.
    /// </returns>
    public List< int > ComputeTriangles( float[] points, int offset, int count, bool sorted )
    {
        if ( count > 32767 )
        {
            throw new ArgumentException( "count must be <= " + 32767 );
        }

        List< int > triangles = _triangles;
        triangles.Clear();

        if ( count < 6 )
        {
            return triangles;
        }

        triangles.EnsureCapacity( count );

        if ( !sorted )
        {
            if ( ( _sortedPoints == null ) || ( _sortedPoints.Length < count ) )
            {
                _sortedPoints = new float[ count ];
            }

            Array.Copy( points, offset, _sortedPoints, 0, count );

            points = _sortedPoints;
            offset = 0;

            Sort( points, count );
        }

        var end = offset + count;

        // Determine bounds for super triangle.
        float xmin = points[ 0 ], ymin = points[ 1 ];
        float xmax = xmin,        ymax = ymin;

        for ( var i = offset + 2; i < end; i++ )
        {
            var value = points[ i ];

            if ( value < xmin )
            {
                xmin = value;
            }

            if ( value > xmax )
            {
                xmax = value;
            }

            i++;
            value = points[ i ];

            if ( value < ymin )
            {
                ymin = value;
            }

            if ( value > ymax )
            {
                ymax = value;
            }
        }

        float dx   = xmax - xmin, dy = ymax - ymin;
        var   dmax = ( dx > dy ? dx : dy ) * 20f;
        float xmid = ( xmax + xmin ) / 2f, ymid = ( ymax + ymin ) / 2f;

        // Setup the super triangle, which contains all points.
        var superTriangle = _superTriangle;

        superTriangle[ 0 ] = xmid - dmax;
        superTriangle[ 1 ] = ymid - dmax;
        superTriangle[ 2 ] = xmid;
        superTriangle[ 3 ] = ymid + dmax;
        superTriangle[ 4 ] = xmid + dmax;
        superTriangle[ 5 ] = ymid - dmax;

        _edges.EnsureCapacity( count / 2 );

        _complete.Clear();
        _complete.EnsureCapacity( count );

        // Add super triangle.
        triangles.Add( end );
        triangles.Add( end + 2 );
        triangles.Add( end + 4 );
        _complete.Add( false );

        int[] trianglesArray;

        // Include each point one at a time into the existing mesh.
        for ( var pointIndex = offset; pointIndex < end; pointIndex += 2 )
        {
            float x = points[ pointIndex ], y = points[ pointIndex + 1 ];

            // If x,y lies inside the circumcircle of a triangle, the edges
            // are stored and the triangle removed.
            trianglesArray = triangles.ToArray();
            var completeArray = _complete.ToArray();

            for ( var triangleIndex = triangles.Count - 1; triangleIndex >= 0; triangleIndex -= 3 )
            {
                var completeIndex = triangleIndex / 3;

                if ( completeArray[ completeIndex ] )
                {
                    continue;
                }

                var p1 = trianglesArray[ triangleIndex - 2 ];
                var p2 = trianglesArray[ triangleIndex - 1 ];
                var p3 = trianglesArray[ triangleIndex ];

                float x1;
                float y1;
                float x2;
                float y2;
                float x3;
                float y3;

                if ( p1 >= end )
                {
                    var i = p1 - end;
                    x1 = superTriangle[ i ];
                    y1 = superTriangle[ i + 1 ];
                }
                else
                {
                    x1 = points[ p1 ];
                    y1 = points[ p1 + 1 ];
                }

                if ( p2 >= end )
                {
                    var i = p2 - end;
                    x2 = superTriangle[ i ];
                    y2 = superTriangle[ i + 1 ];
                }
                else
                {
                    x2 = points[ p2 ];
                    y2 = points[ p2 + 1 ];
                }

                if ( p3 >= end )
                {
                    var i = p3 - end;
                    x3 = superTriangle[ i ];
                    y3 = superTriangle[ i + 1 ];
                }
                else
                {
                    x3 = points[ p3 ];
                    y3 = points[ p3 + 1 ];
                }

                switch ( CircumCircle( x, y, x1, y1, x2, y2, x3, y3 ) )
                {
                    case COMPLETE:
                        completeArray[ completeIndex ] = true;

                        break;

                    case INSIDE:
                        _edges.AddAll( p1, p2, p2, p3 );
                        _edges.AddAll( p3, p1 );

                        triangles.RemoveRange( triangleIndex - 2, triangleIndex );
                        _complete.RemoveAt( completeIndex );

                        break;
                }
            }

            var edgesArray = _edges.ToArray();

            for ( int i = 0, n = _edges.Count; i < n; i += 2 )
            {
                // Skip multiple edges. If all triangles are anticlockwise then
                // all interior edges are opposite pointing in direction.
                var p1 = edgesArray[ i ];

                if ( p1 == -1 )
                {
                    continue;
                }

                var p2   = edgesArray[ i + 1 ];
                var skip = false;

                for ( var ii = i + 2; ii < n; ii += 2 )
                {
                    if ( ( p1 == edgesArray[ ii + 1 ] ) && ( p2 == edgesArray[ ii ] ) )
                    {
                        skip             = true;
                        edgesArray[ ii ] = -1;
                    }
                }

                if ( skip )
                {
                    continue;
                }

                // Form new triangles for the current point. Edges are
                // arranged in clockwise order.
                triangles.Add( p1 );
                triangles.Add( edgesArray[ i + 1 ] );
                triangles.Add( pointIndex );
                _complete.Add( false );
            }

            _edges.Clear();
        }

        // Remove triangles with super triangle vertices.
        trianglesArray = triangles.ToArray();

        for ( var i = triangles.Count - 1; i >= 0; i -= 3 )
        {
            if ( ( trianglesArray[ i ] >= end )
              || ( trianglesArray[ i - 1 ] >= end )
              || ( trianglesArray[ i - 2 ] >= end ) )
            {
                triangles.RemoveAt( i );
                triangles.RemoveAt( i - 1 );
                triangles.RemoveAt( i - 2 );
            }
        }

        // Convert sorted to unsorted indices.
        if ( !sorted )
        {
            var originalIndicesArray = _originalIndices.ToArray();

            for ( int i = 0, n = triangles.Count; i < n; i++ )
            {
                trianglesArray[ i ] = ( short ) ( originalIndicesArray[ trianglesArray[ i ] / 2 ] * 2 );
            }
        }

        // Adjust triangles to start from zero and count by 1,
        // not by vertex x,y coordinate pairs.
        if ( offset == 0 )
        {
            for ( int i = 0, n = triangles.Count; i < n; i++ )
            {
                trianglesArray[ i ] = ( short ) ( trianglesArray[ i ] / 2 );
            }
        }
        else
        {
            for ( int i = 0, n = triangles.Count; i < n; i++ )
            {
                trianglesArray[ i ] = ( short ) ( ( trianglesArray[ i ] - offset ) / 2 );
            }
        }

        return triangles;
    }

    /// <summary>
    /// Returns INSIDE if point xp,yp is inside the circumcircle made up of
    /// the points x1,y1, x2,y2, x3,y3. Returns COMPLETE if xp is to the right
    /// of the entire circumcircle. Otherwise returns INCOMPLETE.
    /// <para>
    /// Note: a point on the circumcircle edge is considered inside.
    /// </para>
    /// </summary>
    private int CircumCircle( float xp,
                              float yp,
                              float x1,
                              float y1,
                              float x2,
                              float y2,
                              float x3,
                              float y3 )
    {
        float xc;
        float yc;

        var y1Y2 = Math.Abs( y1 - y2 );
        var y2Y3 = Math.Abs( y2 - y3 );

        if ( y1Y2 < EPSILON )
        {
            if ( y2Y3 < EPSILON )
            {
                return INCOMPLETE;
            }

            var m2  = -( x3 - x2 ) / ( y3 - y2 );
            var mx2 = ( x2 + x3 ) / 2f;
            var my2 = ( y2 + y3 ) / 2f;

            xc = ( x2 + x1 ) / 2f;
            yc = ( m2 * ( xc - mx2 ) ) + my2;
        }
        else
        {
            var m1  = -( x2 - x1 ) / ( y2 - y1 );
            var mx1 = ( x1 + x2 ) / 2f;
            var my1 = ( y1 + y2 ) / 2f;

            if ( y2Y3 < EPSILON )
            {
                xc = ( x3 + x2 ) / 2f;
            }
            else
            {
                var m2  = -( x3 - x2 ) / ( y3 - y2 );
                var mx2 = ( x2 + x3 ) / 2f;
                var my2 = ( y2 + y3 ) / 2f;

                xc = ( ( ( ( m1 * mx1 ) - ( m2 * mx2 ) ) + my2 ) - my1 ) / ( m1 - m2 );
            }
            
            yc = ( m1 * ( xc - mx1 ) ) + my1;
        }

        var dx   = x2 - xc;
        var dy   = y2 - yc;
        var rsqr = ( dx * dx ) + ( dy * dy );

        dx =  xp - xc;
        dx *= dx;
        dy =  yp - yc;

        if ( ( ( dx + ( dy * dy ) ) - rsqr ) <= EPSILON )
        {
            return INSIDE;
        }

        return ( xp > xc ) && ( dx > rsqr ) ? COMPLETE : INCOMPLETE;
    }

    /// <summary>
    /// Sorts x,y pairs of values by the x value.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="count"> Number of indices, must be even. </param>
    private void Sort( float[] values, int count )
    {
        var pointCount = count / 2;

        _originalIndices.Clear();
        _originalIndices.EnsureCapacity( pointCount );

        var originalIndicesArray = _originalIndices.ToArray();

        for ( short i = 0; i < pointCount; i++ )
        {
            originalIndicesArray[ i ] = i;
        }

        var lower = 0;
        var upper = count - 1;

        _quicksortStack.Add( lower );
        _quicksortStack.Add( upper - 1 );

        while ( _quicksortStack.Count > 0 )
        {
            upper = _quicksortStack.Pop();
            lower = _quicksortStack.Pop();

            if ( upper <= lower )
            {
                continue;
            }

            var i = QuicksortPartition( values, lower, upper, originalIndicesArray );

            if ( ( i - lower ) > ( upper - i ) )
            {
                _quicksortStack.Add( lower );
                _quicksortStack.Add( i - 2 );
            }

            _quicksortStack.Add( i + 2 );
            _quicksortStack.Add( upper );

            if ( ( upper - i ) >= ( i - lower ) )
            {
                _quicksortStack.Add( lower );
                _quicksortStack.Add( i - 2 );
            }
        }
    }

    private int QuicksortPartition( float[] values, int lower, int upper, int[] originalIndices )
    {
        var value = values[ lower ];
        var up    = upper;
        var down  = lower + 2;

        while ( down < up )
        {
            while ( ( down < up ) && ( values[ down ] <= value ) )
            {
                down += 2;
            }

            while ( values[ up ] > value )
            {
                up -= 2;
            }

            if ( down < up )
            {
                ( values[ down ], values[ up ] )         = ( values[ up ], values[ down ] );
                ( values[ down + 1 ], values[ up + 1 ] ) = ( values[ up + 1 ], values[ down + 1 ] );

                ( originalIndices[ down / 2 ], originalIndices[ up / 2 ] )
                    = ( originalIndices[ up / 2 ], originalIndices[ down / 2 ] );
            }
        }

        if ( value > values[ up ] )
        {
            values[ lower ] = values[ up ];
            values[ up ]    = value;

            ( values[ lower + 1 ], values[ up + 1 ] ) = ( values[ up + 1 ], values[ lower + 1 ] );

            ( originalIndices[ lower / 2 ], originalIndices[ up / 2 ] )
                = ( originalIndices[ up / 2 ], originalIndices[ lower / 2 ] );
        }

        return up;
    }

    /// <summary>
    /// Removes all triangles with a centroid outside the specified hull, which
    /// may be concave. Note some triangulations may have triangles whose centroid
    /// is inside the hull but a portion is outside.
    /// </summary>
    public void Trim( List< short > triangles, float[] points, float[] hull, int offset, int count )
    {
        var trianglesArray = triangles.ToArray();

        for ( var i = triangles.Count - 1; i >= 0; i -= 3 )
        {
            var p1 = trianglesArray[ i - 2 ] * 2;
            var p2 = trianglesArray[ i - 1 ] * 2;
            var p3 = trianglesArray[ i ] * 2;

            GeometryUtils.TriangleCentroid( points[ p1 ],
                                            points[ p1 + 1 ],
                                            points[ p2 ],
                                            points[ p2 + 1 ],
                                            points[ p3 ],
                                            points[ p3 + 1 ],
                                            _centroid );

            if ( !Intersector.IsPointInPolygon( hull, offset, count, _centroid.X, _centroid.Y ) )
            {
                triangles.RemoveAt( i );
                triangles.RemoveAt( i - 1 );
                triangles.RemoveAt( i - 2 );
            }
        }
    }
}
