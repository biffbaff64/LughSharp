namespace LibGDXSharp.Maths;

/// <summary>
/// Computes the convex hull of a set of points using the monotone
/// chain convex hull algorithm (aka Andrew's algorithm).
/// </summary>
public class ConvexHull
{
    private List< int >   _quicksortStack  = new();
    private List< float > _hull            = new();
    private List< int >   _indices         = new();
    private List< short > _originalIndices = new();
    private float[]?      _sortedPoints;

    public List< float > ComputePolygon( List< float > points, bool sorted )
    {
        return ComputePolygon( points.items, 0, points.size, sorted );
    }

    public List< float > ComputePolygon( float[] polygon, bool sorted )
    {
        return ComputePolygon( polygon, 0, polygon.length, sorted );
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
        int end = offset + count;

        if ( !sorted )
        {
            if ( ( _sortedPoints == null ) || ( _sortedPoints.Length < count ) )
            {
                _sortedPoints = new float[ count ];
            }
            
            Array.Copy( points, offset, _sortedPoints, 0, count );

            points = _sortedPoints;
            offset = 0;
            sort( points, count );
        }

        List< float > hull = this._hull;
        hull.Clear();

        // Lower hull.
        for ( int i = offset; i < end; i += 2 )
        {
            float x = points[ i ];
            float y = points[ i + 1 ];

            while ( ( hull.Count >= 4 ) && ( ccw( x, y ) <= 0 ) )
            {
                hull.Count -= 2;
            }

            hull.Add( x );
            hull.Add( y );
        }

        // Upper hull.
        for ( int i = end - 4, t = hull.size + 2; i >= offset; i -= 2 )
        {
            float x = points[ i ];
            float y = points[ i + 1 ];

            while ( ( hull.size >= t ) && ( ccw( x, y ) <= 0 ) )
                hull.size -= 2;

            hull.add( x );
            hull.add( y );
        }

        return hull;
    }

    /** @see #computeIndices(float[], int, int, bool, bool) */
    public List< int > computeIndices( List< float > points, bool sorted, bool yDown )
    {
        return computeIndices( points.items, 0, points.size, sorted, yDown );
    }

    /** @see #computeIndices(float[], int, int, bool, bool) */
    public List< int > computeIndices( float[] polygon, bool sorted, bool yDown )
    {
        return computeIndices( polygon, 0, polygon.length, sorted, yDown );
    }

    /** Computes a hull the same as {@link #computePolygon(float[], int, int, bool)} but returns indices of the specified
	 * points. */
    public List< int > computeIndices( float[] points, int offset, int count, bool sorted, bool yDown )
    {
        if ( count > 32767 ) throw new IllegalArgumentException( "count must be <= " + 32767 );
        int end = offset + count;

        if ( !sorted )
        {
            if ( ( _sortedPoints == null ) || ( _sortedPoints.length < count ) ) _sortedPoints = new float[ count ];
            System.arraycopy( points, offset, _sortedPoints, 0, count );
            points = _sortedPoints;
            offset = 0;
            sortWithIndices( points, count, yDown );
        }

        List< int > indices = this._indices;
        indices.clear();

        List< float > hull = this._hull;
        hull.clear();

        // Lower hull.
        for ( int i = offset, index = i / 2; i < end; i += 2, index++ )
        {
            float x = points[ i ];
            float y = points[ i + 1 ];

            while ( ( hull.size >= 4 ) && ( ccw( x, y ) <= 0 ) )
            {
                hull.size -= 2;
                indices.size--;
            }

            hull.add( x );
            hull.add( y );
            indices.add( index );
        }

        // Upper hull.
        for ( int i = end - 4, index = i / 2, t = hull.size + 2; i >= offset; i -= 2, index-- )
        {
            float x = points[ i ];
            float y = points[ i + 1 ];

            while ( ( hull.size >= t ) && ( ccw( x, y ) <= 0 ) )
            {
                hull.size -= 2;
                indices.size--;
            }

            hull.add( x );
            hull.add( y );
            indices.add( index );
        }

        // Convert sorted to unsorted indices.
        if ( !sorted )
        {
            short[] originalIndicesArray = _originalIndices.items;
            int[]   indicesArray         = indices.items;

            for ( int i = 0, n = indices.size; i < n; i++ )
                indicesArray[ i ] = originalIndicesArray[ indicesArray[ i ] ];
        }

        return indices;
    }

    /** Returns > 0 if the points are a counterclockwise turn, < 0 if clockwise, and 0 if colinear. */
    private float ccw( float p3x, float p3y )
    {
        List< float > hull = this._hull;
        int           size = hull.size;
        float         p1x  = hull.get( size - 4 );
        float         p1y  = hull.get( size - 3 );
        float         p2x  = hull.get( size - 2 );
        float         p2y  = hull.peek();

        return ( ( p2x - p1x ) * ( p3y - p1y ) ) - ( ( p2y - p1y ) * ( p3x - p1x ) );
    }

    /** Sorts x,y pairs of values by the x value, then the y value.
	 * @param count Number of indices, must be even. */
    private void sort( float[] values, int count )
    {
        int         lower = 0;
        int         upper = count - 1;
        List< int > stack = _quicksortStack;
        stack.add( lower );
        stack.add( upper - 1 );

        while ( stack.size > 0 )
        {
            upper = stack.pop();
            lower = stack.pop();

            if ( upper <= lower ) continue;
            int i = quicksortPartition( values, lower, upper );

            if ( ( i - lower ) > ( upper - i ) )
            {
                stack.add( lower );
                stack.add( i - 2 );
            }

            stack.add( i + 2 );
            stack.add( upper );

            if ( ( upper - i ) >= ( i - lower ) )
            {
                stack.add( lower );
                stack.add( i - 2 );
            }
        }
    }

    private int quicksortPartition( float[] values, int lower, int upper )
    {
        float x    = values[ lower ];
        float y    = values[ lower + 1 ];
        int   up   = upper;
        int   down = lower;
        float temp;

        while ( down < up )
        {
            while ( ( down < up ) && ( values[ down ] <= x ) )
                down += 2;

            while ( ( values[ up ] > x ) || ( ( values[ up ] == x ) && ( values[ up + 1 ] < y ) ) )
                up -= 2;

            if ( down < up )
            {
                temp           = values[ down ];
                values[ down ] = values[ up ];
                values[ up ]   = temp;

                temp               = values[ down + 1 ];
                values[ down + 1 ] = values[ up + 1 ];
                values[ up + 1 ]   = temp;
            }
        }

        if ( ( x > values[ up ] ) || ( ( x == values[ up ] ) && ( y < values[ up + 1 ] ) ) )
        {
            values[ lower ] = values[ up ];
            values[ up ]    = x;

            values[ lower + 1 ] = values[ up + 1 ];
            values[ up + 1 ]    = y;
        }

        return up;
    }

    /** Sorts x,y pairs of values by the x value, then the y value and stores unsorted original indices.
	 * @param count Number of indices, must be even. */
    private void sortWithIndices( float[] values, int count, bool yDown )
    {
        int pointCount = count / 2;
        _originalIndices.clear();
        _originalIndices.ensureCapacity( pointCount );
        short[] originalIndicesArray = _originalIndices.items;

        for ( short i = 0; i < pointCount; i++ )
            originalIndicesArray[ i ] = i;

        int         lower = 0;
        int         upper = count - 1;
        List< int > stack = _quicksortStack;
        stack.add( lower );
        stack.add( upper - 1 );

        while ( stack.size > 0 )
        {
            upper = stack.pop();
            lower = stack.pop();

            if ( upper <= lower ) continue;
            int i = QuicksortPartitionWithIndices( values, lower, upper, yDown, originalIndicesArray );

            if ( ( i - lower ) > ( upper - i ) )
            {
                stack.add( lower );
                stack.add( i - 2 );
            }

            stack.add( i + 2 );
            stack.add( upper );

            if ( ( upper - i ) >= ( i - lower ) )
            {
                stack.add( lower );
                stack.add( i - 2 );
            }
        }
    }

    private int QuicksortPartitionWithIndices( float[] values, int lower, int upper, bool yDown,
                                               short[] originalIndices )
    {
        float x    = values[ lower ];
        float y    = values[ lower + 1 ];
        int   up   = upper;
        int   down = lower;
        short tempIndex;

        while ( down < up )
        {
            while ( ( down < up ) && ( values[ down ] <= x ) )
            {
                down += 2;
            }

            if ( yDown )
            {
                while ( ( values[ up ] > x ) || ( ( values[ up ].Equals( x ) ) && ( values[ up + 1 ] < y ) ) )
                {
                    up -= 2;
                }
            }
            else
            {
                while ( ( values[ up ] > x ) || ( ( values[ up ].Equals( x ) ) && ( values[ up + 1 ] > y ) ) )
                {
                    up -= 2;
                }
            }

            if ( down < up )
            {
                var temp = values[ down ];
                values[ down ] = values[ up ];
                values[ up ]   = temp;

                temp               = values[ down + 1 ];
                values[ down + 1 ] = values[ up + 1 ];
                values[ up + 1 ]   = temp;

                tempIndex                   = originalIndices[ down / 2 ];
                originalIndices[ down / 2 ] = originalIndices[ up / 2 ];
                originalIndices[ up / 2 ]   = tempIndex;
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

            tempIndex                    = originalIndices[ lower / 2 ];
            originalIndices[ lower / 2 ] = originalIndices[ up / 2 ];
            originalIndices[ up / 2 ]    = tempIndex;
        }

        return up;
    }
}
