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

using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Maths;

/// <summary>
/// Computes the convex hull of a set of points using the monotone
/// chain convex hull algorithm (aka Andrew's algorithm).
/// </summary>
[PublicAPI]
public class ConvexHull
{
    private readonly List< int >   _quicksortStack  = new();
    private readonly List< float > _hull            = new();
    private readonly List< int >   _indices         = new();
    private readonly List< short > _originalIndices = new();

    private float[]? _sortedPoints;

    public List< float > ComputePolygon( List< float > points, bool sorted )
    {
        return ComputePolygon( points.ToArray(), 0, points.Count, sorted );
    }

    public List< float > ComputePolygon( float[] polygon, bool sorted )
    {
        return ComputePolygon( polygon, 0, polygon.Length, sorted );
    }

    /// <summary>
    /// Returns the convex hull polygon for the given point cloud. </summary>
    /// <param name="points">
    /// x,y pairs describing points. Duplicate points will result in undefined behavior.
    /// </param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <param name="sorted">
    /// If false, the points will be sorted by the x coordinate then the y coordinate,
    /// which is required by the convex hull algorithm. If sorting is done the input
    /// array is not modified and count additional working memory is needed.
    /// </param>
    /// <returns>
    /// pairs of coordinates that describe the convex hull polygon in counterclockwise
    /// order. Note the returned array is reused for later calls to the same method.
    /// </returns>
    public List< float > ComputePolygon( float[] points, int offset, int count, bool sorted )
    {
        var end = offset + count;

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

        List< float > hull = this._hull;
        hull.Clear();

        // Lower hull.
        for ( var i = offset; i < end; i += 2 )
        {
            var x = points[ i ];
            var y = points[ i + 1 ];

            while ( ( hull.Count >= 4 ) && ( Ccw( x, y ) <= 0 ) )
            {
                hull.RemoveRange( hull.Count - 2, 2 );
            }

            hull.Add( x );
            hull.Add( y );
        }

        // Upper hull.
        for ( int i = end - 4, t = hull.Count + 2; i >= offset; i -= 2 )
        {
            var x = points[ i ];
            var y = points[ i + 1 ];

            while ( ( hull.Count >= t ) && ( Ccw( x, y ) <= 0 ) )
            {
                hull.RemoveRange( hull.Count - 2, 2 );
            }

            hull.Add( x );
            hull.Add( y );
        }

        return hull;
    }

    public List< int > ComputeIndices( List< float > points, bool sorted, bool yDown )
    {
        return ComputeIndices( points.ToArray(), 0, points.Count, sorted, yDown );
    }

    public List< int > ComputeIndices( float[] polygon, bool sorted, bool yDown )
    {
        return ComputeIndices( polygon, 0, polygon.Length, sorted, yDown );
    }

    /// <summary>
    /// Computes a hull the same as <see cref="ComputePolygon(float[], int, int, bool)"/>
    /// but returns indices of the specified points.
    /// </summary>
    //TODO: This method needs debugging / testing to make sure it works as expected
    public List< int > ComputeIndices( float[] points, int offset, int count, bool sorted, bool yDown )
    {
        if ( count > 32767 ) throw new ArgumentException( "count must be <= " + 32767 );

        var end = offset + count;

        if ( !sorted )
        {
            if ( ( _sortedPoints == null ) || ( _sortedPoints.Length < count ) )
            {
                _sortedPoints = new float[ count ];
            }

            Array.Copy( points, offset, _sortedPoints, 0, count );

            points = _sortedPoints;
            offset = 0;

            SortWithIndices( points, count, yDown );
        }

        List< int > indices = this._indices;
        indices.Clear();

        List< float > hull = this._hull;
        hull.Clear();

        // Lower hull.
        for ( int i = offset, index = i / 2; i < end; i += 2, index++ )
        {
            var x = points[ i ];
            var y = points[ i + 1 ];

            while ( ( hull.Count >= 4 ) && ( Ccw( x, y ) <= 0 ) )
            {
                hull.RemoveRange( hull.Count - 2, 2 );
                indices.RemoveAt( indices.Count - 1 );
            }

            hull.Add( x );
            hull.Add( y );
            indices.Add( index );
        }

        // Upper hull.
        for ( int i = end - 4, index = i / 2, t = hull.Count + 2; i >= offset; i -= 2, index-- )
        {
            var x = points[ i ];
            var y = points[ i + 1 ];

            while ( ( hull.Count >= t ) && ( Ccw( x, y ) <= 0 ) )
            {
                hull.RemoveRange( hull.Count - 2, 2 );
                indices.RemoveAt( indices.Count - 1 );
            }

            hull.Add( x );
            hull.Add( y );
            indices.Add( index );
        }

        // Convert sorted to unsorted indices.
        if ( !sorted )
        {
            for ( int i = 0, n = indices.Count; i < n; i++ )
            {
                indices[ i ] = _originalIndices[ indices[ i ] ];
            }
        }

        return indices;
    }

    /// <summary>
    /// Returns > 0 if the points are a counterclockwise turn, &lt; 0 if
    /// clockwise, and 0 if colinear.
    /// </summary>
    /// <param name="px"></param>
    /// <param name="py"></param>
    private float Ccw( float px, float py )
    {
        var size = this._hull.Count;
        var p1X  = this._hull[ size - 4 ];
        var p1Y  = this._hull[ size - 3 ];
        var p2X  = this._hull[ size - 2 ];
        var p2Y  = this._hull.Peek();

        return ( ( p2X - p1X ) * ( py - p1Y ) ) - ( ( p2Y - p1Y ) * ( px - p1X ) );
    }

    /// <summary>
    /// Sorts x,y pairs of values by the x value, then the y value.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="count"> Number of indices, must be even. </param>
    private void Sort( float[] values, int count )
    {
        var lower = 0;
        var upper = count - 1;

        List< int > stack = _quicksortStack;

        stack.Add( lower );
        stack.Add( upper - 1 );

        while ( stack.Count > 0 )
        {
            upper = stack.Pop();
            lower = stack.Pop();

            if ( upper <= lower ) continue;
            var i = QuicksortPartition( values, lower, upper );

            if ( ( i - lower ) > ( upper - i ) )
            {
                stack.Add( lower );
                stack.Add( i - 2 );
            }

            stack.Add( i + 2 );
            stack.Add( upper );

            if ( ( upper - i ) >= ( i - lower ) )
            {
                stack.Add( lower );
                stack.Add( i - 2 );
            }
        }
    }

    private int QuicksortPartition( float[] values, int lower, int upper )
    {
        var x    = values[ lower ];
        var y    = values[ lower + 1 ];
        var up   = upper;
        var down = lower;

        while ( down < up )
        {
            while ( ( down < up ) && ( values[ down ] <= x ) )
            {
                down += 2;
            }

            while ( ( values[ up ] > x ) || ( ( values[ up ].Equals( x ) ) && ( values[ up + 1 ] < y ) ) )
            {
                up -= 2;
            }

            if ( down < up )
            {
                ( values[ up ], values[ down ] )         = ( values[ down ], values[ up ] );
                ( values[ up + 1 ], values[ down + 1 ] ) = ( values[ down + 1 ], values[ up + 1 ] );
            }
        }

        if ( ( x > values[ up ] ) || ( ( x.Equals( values[ up ] ) ) && ( y < values[ up + 1 ] ) ) )
        {
            values[ lower ] = values[ up ];
            values[ up ]    = x;

            values[ lower + 1 ] = values[ up + 1 ];
            values[ up + 1 ]    = y;
        }

        return up;
    }

    /// <summary>
    /// Sorts x,y pairs of values by the x value, then the y value and stores unsorted original indices.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="count"> Number of indices, must be even. </param>
    /// <param name="yDown"></param>
    private void SortWithIndices( float[] values, int count, bool yDown )
    {
        var pointCount = count / 2;

        _originalIndices.Clear();
        _originalIndices.EnsureCapacity( pointCount );

        for ( short i = 0; i < pointCount; i++ )
        {
            _originalIndices[ i ] = i;
        }

        var lower = 0;
        var upper = count - 1;

        List< int > stack = _quicksortStack;

        stack.Add( lower );
        stack.Add( upper - 1 );

        while ( stack.Count > 0 )
        {
            upper = stack.Pop();
            lower = stack.Pop();

            if ( upper <= lower ) continue;

            var i = QuicksortPartitionWithIndices( values, lower, upper, yDown, _originalIndices.ToArray() );

            if ( ( i - lower ) > ( upper - i ) )
            {
                stack.Add( lower );
                stack.Add( i - 2 );
            }

            stack.Add( i + 2 );
            stack.Add( upper );

            if ( ( upper - i ) >= ( i - lower ) )
            {
                stack.Add( lower );
                stack.Add( i - 2 );
            }
        }
    }

    private int QuicksortPartitionWithIndices( float[] values,
                                               int lower,
                                               int upper,
                                               bool yDown,
                                               short[] originalIndices )
    {
        var x    = values[ lower ];
        var y    = values[ lower + 1 ];
        var up   = upper;
        var down = lower;

        while ( down < up )
        {
            while ( ( down < up ) && ( values[ down ] <= x ) )
            {
                down += 2;
            }

            if ( yDown )
            {
                while ( ( values[ up ] > x )
                        || ( ( values[ up ].Equals( x ) ) && ( values[ up + 1 ] < y ) ) )
                {
                    up -= 2;
                }
            }
            else
            {
                while ( ( values[ up ] > x )
                        || ( ( values[ up ].Equals( x ) ) && ( values[ up + 1 ] > y ) ) )
                {
                    up -= 2;
                }
            }

            if ( down < up )
            {
                ( values[ up ], values[ down ] )         = ( values[ down ], values[ up ] );
                ( values[ up + 1 ], values[ down + 1 ] ) = ( values[ down + 1 ], values[ up + 1 ] );

                ( originalIndices[ down / 2 ], originalIndices[ up / 2 ] ) =
                    ( originalIndices[ up / 2 ], originalIndices[ down / 2 ] );
            }
        }

        if ( ( x > values[ up ] )
             || ( ( x.Equals( values[ up ] ) )
                  && ( yDown ? y < values[ up + 1 ] : y > values[ up + 1 ] ) ) )
        {
            values[ lower ] = values[ up ];
            values[ up ]    = x;

            values[ lower + 1 ] = values[ up + 1 ];
            values[ up + 1 ]    = y;

            ( originalIndices[ lower / 2 ], originalIndices[ up / 2 ] ) =
                ( originalIndices[ up / 2 ], originalIndices[ lower / 2 ] );
        }

        return up;
    }
}