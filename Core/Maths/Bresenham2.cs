namespace LibGDXSharp.Maths;

/// <summary>
/// Returns a list of points at integer coordinates for a line on a 2D grid,
/// using the Bresenham algorithm.
/// <para>
/// Instances of this class own the returned array of points and the points
/// themselves to avoid garbage collection as much as possible. Calling any of
/// the methods will result in the reuse of the previously returned array and
/// vectors.
/// </para>
/// </summary>
public class Bresenham2
{
    private readonly List< GridPoint2 > _points = new List< GridPoint2 >();
    private readonly Pool< GridPoint2 > _pool = new Pool< GridPoint2 >
    {
        NewObject = NewGridPointGenerator
    };

    private static GridPoint2 NewGridPointGenerator()
    {
        return new GridPoint2();
    }

    /// <summary>
    /// Returns a list of <see cref="GridPoint2"/> instances along the given line,
    /// at integer coordinates.
    /// </summary>
    /// <param name="start"> the start of the line </param>
    /// <param name="end"> the end of the line </param>
    /// <returns> the list of points on the line at integer coordinates  </returns>
    public virtual List< GridPoint2 > Line( GridPoint2 start, GridPoint2 end )
    {
        return Line( start.X, start.Y, end.X, end.Y );
    }

    /// <summary>
    /// Returns a list of <see cref="GridPoint2"/> instances along the given line,
    /// at integer coordinates.
    /// </summary>
    /// <param name="startX"> the start x coordinate of the line </param>
    /// <param name="startY"> the start y coordinate of the line </param>
    /// <param name="endX"> the end x coordinate of the line </param>
    /// <param name="endY"> the end y coordinate of the line </param>
    /// <returns> the list of points on the line at integer coordinates  </returns>
    public virtual List< GridPoint2 > Line( int startX, int startY, int endX, int endY )
    {
        _pool.FreeAll( _points );
        _points.Clear();

        return Line( startX, startY, endX, endY, _pool, _points );
    }

    /// <summary>
    /// Returns a list of <see cref="GridPoint2"/> instances along the given line,
    /// at integer coordinates.
    /// </summary>
    /// <param name="startX"> the start x coordinate of the line </param>
    /// <param name="startY"> the start y coordinate of the line </param>
    /// <param name="endX"> the end x coordinate of the line </param>
    /// <param name="endY"> the end y coordinate of the line </param>
    /// <param name="pool"> the pool from which GridPoint2 instances are fetched </param>
    /// <param name="output"> the output array, will be cleared in this method </param>
    /// <returns> the list of points on the line at integer coordinates  </returns>
    public virtual List< GridPoint2 > Line( int startX,
                                            int startY,
                                            int endX,
                                            int endY,
                                            Pool< GridPoint2 > pool,
                                            List< GridPoint2 > output )
    {

        var w   = endX - startX;
        var h   = endY - startY;
        var dx1 = 0;
        var dy1 = 0;
        var dx2 = 0;
        var dy2 = 0;

        if ( w < 0 )
        {
            dx1 = -1;
            dx2 = -1;
        }
        else if ( w > 0 )
        {
            dx1 = 1;
            dx2 = 1;
        }

        if ( h < 0 )
        {
            dy1 = -1;
        }
        else if ( h > 0 )
        {
            dy1 = 1;
        }

        var longest  = Math.Abs( w );
        var shortest = Math.Abs( h );

        if ( longest < shortest )
        {
            longest  = Math.Abs( h );
            shortest = Math.Abs( w );

            if ( h < 0 )
            {
                dy2 = -1;
            }
            else if ( h > 0 )
            {
                dy2 = 1;
            }

            dx2 = 0;
        }

        var shortest2 = shortest << 1;
        var longest2  = longest << 1;
        var numerator = 0;

        for ( int i = 0; i <= longest; i++ )
        {
            GridPoint2 point = pool.Obtain();
            point.Set( startX, startY );
            output.Add( point );
            numerator += shortest2;

            if ( numerator > longest )
            {
                numerator -= longest2;
                startX    += dx1;
                startY    += dy1;
            }
            else
            {
                startX += dx2;
                startY += dy2;
            }
        }

        return output;
    }
}