// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Maths.Collision;
using LughSharp.LibCore.Utils.Collections.Extensions;

namespace LughSharp.LibCore.Maths;

/// <summary>
///     Class offering various static methods for intersection testing
///     between different geometric objects.
/// </summary>
public class Intersector
{
    private readonly static Vector3       V0          = new();
    private readonly static Vector3       V1          = new();
    private readonly static Vector3       V2          = new();
    private readonly static List< float > FloatArray  = new();
    private readonly static List< float > FloatArray2 = new();

    private readonly static Vector2 Ip  = new();
    private readonly static Vector2 Ep1 = new();
    private readonly static Vector2 Ep2 = new();
    private readonly static Vector2 S   = new();
    private readonly static Vector2 E   = new();

    private readonly static Vector2 V2A = new();
    private readonly static Vector2 V2B = new();
    private readonly static Vector2 V2C = new();
    private readonly static Vector2 V2D = new();

    private readonly static Plane   Plane = new( new Vector3(), 0 );
    private readonly static Vector3 Vec3  = new();

    private static Vector3 _dir   = new();
    private static Vector3 _start = new();

    private readonly static Vector3 Best = new();
    private readonly static Vector3 Tmp  = new();
    private readonly static Vector3 Tmp1 = new();
    private readonly static Vector3 Tmp2 = new();
    private readonly static Vector3 Tmp3 = new();

    //TODO:
    // This is a temporary fix for a problem caused by the above method.
    // As 'ref null' is not allowed i thought the best option would be to
    // create a dummy MinimumTranslationVector object and test for that as
    // well as null.
    // As soon as i determine whether or not 'ref' is actually needed, or if
    // there is a better solution then I will update.
    private static MinimumTranslationVector? _dummyMinimumTranslationVector = new();

    private readonly static Vector3 Intersection = new();

    private Intersector()
    {
    }

    /// <summary>
    ///     Returns whether the given point is inside the triangle. This assumes that
    ///     the point is on the plane of the triangle. No check is performed that this
    ///     is the case.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="t1"> the first vertex of the triangle </param>
    /// <param name="t2"> the second vertex of the triangle </param>
    /// <param name="t3"> the third vertex of the triangle </param>
    /// <returns> whether the point is in the triangle  </returns>
    public static bool IsPointInTriangle( Vector3 point, Vector3 t1, Vector3 t2, Vector3 t3 )
    {
        V0.Set( t1 ).Sub( point );
        V1.Set( t2 ).Sub( point );
        V2.Set( t3 ).Sub( point );

        var ab = V0.Dot( V1 );
        var ac = V0.Dot( V2 );
        var bc = V1.Dot( V2 );
        var cc = V2.Dot( V2 );

        if ( ( ( bc * ac ) - ( cc * ab ) ) < 0 )
        {
            return false;
        }

        var bb = V1.Dot( V1 );

        return !( ( ( ab * bc ) - ( ac * bb ) ) < 0 );
    }

    /// <summary>
    ///     Returns true if the given point is inside the triangle.
    /// </summary>
    public static bool IsPointInTriangle( Vector2 p, Vector2 a, Vector2 b, Vector2 c )
    {
        var px1    = p.X - a.X;
        var py1    = p.Y - a.Y;
        var side12 = ( ( ( b.X - a.X ) * py1 ) - ( ( b.Y - a.Y ) * px1 ) ) > 0;

        if ( ( ( ( ( c.X - a.X ) * py1 ) - ( ( c.Y - a.Y ) * px1 ) ) > 0 ) == side12 )
        {
            return false;
        }

        return ( ( ( ( c.X - b.X ) * ( p.Y - b.Y ) ) - ( ( c.Y - b.Y ) * ( p.X - b.X ) ) ) > 0 ) == side12;
    }

    /// <summary>
    ///     Returns true if the given point is inside the triangle.
    /// </summary>
    public static bool IsPointInTriangle( float px,
                                          float py,
                                          float ax,
                                          float ay,
                                          float bx,
                                          float by,
                                          float cx,
                                          float cy )
    {
        var px1    = px - ax;
        var py1    = py - ay;
        var side12 = ( ( ( bx - ax ) * py1 ) - ( ( by - ay ) * px1 ) ) > 0;

        if ( ( ( ( ( cx - ax ) * py1 ) - ( ( cy - ay ) * px1 ) ) > 0 ) == side12 )
        {
            return false;
        }

        return ( ( ( ( cx - bx ) * ( py - by ) ) - ( ( cy - by ) * ( px - bx ) ) ) > 0 ) == side12;
    }

    /// <summary>
    /// </summary>
    /// <param name="strt"></param>
    /// <param name="end"></param>
    /// <param name="plane"></param>
    /// <param name="intersection"></param>
    /// <returns></returns>
    public static bool IntersectSegmentPlane( Vector3 strt, Vector3 end, Plane plane, Vector3 intersection )
    {
        var dir   = V0.Set( end ).Sub( strt );
        var denom = dir.Dot( plane.Normal );

        if ( denom == 0f )
        {
            return false;
        }

        var t = -( strt.Dot( plane.Normal ) + plane.DistanceToOrigin ) / denom;

        if ( t is < 0 or > 1 )
        {
            return false;
        }

        intersection.Set( strt ).Add( dir.Scl( t ) );

        return true;
    }

    /// <summary>
    ///     Determines on which side of the given line the point is. Returns 1 if the point is on the left side of the line, 0
    ///     if the
    ///     point is on the line and -1 if the point is on the right side of the line. Left and right are relative to the lines
    ///     direction
    ///     which is linePoint1 to linePoint2.
    /// </summary>
    public static int PointLineSide( Vector2 linePoint1, Vector2 linePoint2, Vector2 point )
    {
        return Math.Sign
            (
             ( ( linePoint2.X - linePoint1.X ) * ( point.Y - linePoint1.Y ) )
           - ( ( linePoint2.Y - linePoint1.Y ) * ( point.X - linePoint1.X ) )
            );
    }

    /// <summary>
    /// </summary>
    /// <param name="linePoint1X"></param>
    /// <param name="linePoint1Y"></param>
    /// <param name="linePoint2X"></param>
    /// <param name="linePoint2Y"></param>
    /// <param name="pointX"></param>
    /// <param name="pointY"></param>
    /// <returns></returns>
    public static int PointLineSide( float linePoint1X, float linePoint1Y, float linePoint2X, float linePoint2Y, float pointX, float pointY )
    {
        return Math.Sign
            (
             ( ( linePoint2X - linePoint1X ) * ( pointY - linePoint1Y ) )
           - ( ( linePoint2Y - linePoint1Y ) * ( pointX - linePoint1X ) )
            );
    }

    /// <summary>
    ///     Checks whether the given point is in the polygon.
    /// </summary>
    /// <param name="polygon">The polygon vertices passed as an array.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point is in the polygon; otherwise, false.</returns>
    public static bool IsPointInPolygon( Vector2[] polygon, Vector2 point )
    {
        var last     = polygon[ ^1 ];
        var x        = point.X;
        var y        = point.Y;
        var oddNodes = false;

        foreach ( var vertex in polygon )
        {
            if ( ( ( vertex.Y < y ) && ( last.Y >= y ) ) || ( ( last.Y < y ) && ( vertex.Y >= y ) ) )
            {
                if ( ( vertex.X + ( ( ( y - vertex.Y ) / ( last.Y - vertex.Y ) ) * ( last.X - vertex.X ) ) ) < x )
                {
                    oddNodes = !oddNodes;
                }
            }

            last = vertex;
        }

        return oddNodes;
    }

    /// <summary>
    ///     Returns true if the specified point is in the polygon.
    /// </summary>
    /// <param name="polygon">The polygon vertices as an array of float values.</param>
    /// <param name="offset">Starting polygon index.</param>
    /// <param name="count">Number of array indices to use after offset.</param>
    /// <param name="x">X-coordinate of the point to check.</param>
    /// <param name="y">Y-coordinate of the point to check.</param>
    /// <returns>True if the point is in the polygon; otherwise, false.</returns>
    public static bool IsPointInPolygon( float[] polygon, int offset, int count, float x, float y )
    {
        var oddNodes = false;
        var sx       = polygon[ offset ];
        var sy       = polygon[ offset + 1 ];
        var y1       = sy;
        var yi       = offset + 3;

        for ( var n = offset + count; yi < n; yi += 2 )
        {
            var y2 = polygon[ yi ];

            if ( ( ( y2 < y ) && ( y1 >= y ) ) || ( ( y1 < y ) && ( y2 >= y ) ) )
            {
                var x2 = polygon[ yi - 1 ];

                if ( ( x2 + ( ( ( y - y2 ) / ( y1 - y2 ) ) * ( polygon[ yi - 3 ] - x2 ) ) ) < x )
                {
                    oddNodes = !oddNodes;
                }
            }

            y1 = y2;
        }

        if ( ( ( sy < y ) && ( y1 >= y ) ) || ( ( y1 < y ) && ( sy >= y ) ) )
        {
            if ( ( sx + ( ( ( y - sy ) / ( y1 - sy ) ) * ( polygon[ yi - 3 ] - sx ) ) ) < x )
            {
                oddNodes = !oddNodes;
            }
        }

        return oddNodes;
    }

    /// <summary>
    ///     Intersects two convex polygons with clockwise vertices and sets the
    ///     overlap polygon resulting from the intersection.
    ///     Follows the Sutherland-Hodgman algorithm.
    /// </summary>
    /// <param name="p1"> The polygon that is being clipped </param>
    /// <param name="p2"> The clip polygon </param>
    /// <param name="overlap"> The intersection of the two polygons (can be null, if an intersection polygon is not needed) </param>
    /// <returns> Whether the two polygons intersect. </returns>
    public static bool IntersectPolygons( Polygon p1, Polygon p2, Polygon? overlap )
    {
        if ( ( p1.Vertices?.Length == 0 ) || ( p2.Vertices?.Length == 0 ) )
        {
            return false;
        }

        FloatArray.Clear();
        FloatArray2.Clear();
        FloatArray2.AddAll( p1.TransformedVertices!.ToList() );

        var vertices2 = p2.TransformedVertices;

        for ( int i = 0, last = vertices2!.Length - 2; i <= last; i += 2 )
        {
            Ep1.Set( vertices2[ i ], vertices2[ i + 1 ] );

            // wrap around to beginning of array if index points to end;
            if ( i < last )
            {
                Ep2.Set( vertices2[ i + 2 ], vertices2[ i + 3 ] );
            }
            else
            {
                Ep2.Set( vertices2[ 0 ], vertices2[ 1 ] );
            }

            if ( FloatArray2.Count == 0 )
            {
                return false;
            }

            S.Set( FloatArray2[ ^2 ], FloatArray2[ ^1 ] );

            for ( var j = 0; j < FloatArray2.Count; j += 2 )
            {
                E.Set( FloatArray2[ j ], FloatArray2[ j + 1 ] );

                // determine if point is inside clip edge
                var side = PointLineSide( Ep2, Ep1, S ) > 0;

                if ( PointLineSide( Ep2, Ep1, E ) > 0 )
                {
                    if ( !side )
                    {
                        IntersectLines( S, E, Ep1, Ep2, Ip );

                        if ( ( FloatArray.Count < 2 )
                          || !FloatArray[ ^2 ].Equals( Ip.X )
                          || !FloatArray[ ^1 ].Equals( Ip.Y ) )
                        {
                            FloatArray.Add( Ip.X );
                            FloatArray.Add( Ip.Y );
                        }
                    }

                    FloatArray.Add( E.X );
                    FloatArray.Add( E.Y );
                }
                else if ( side )
                {
                    IntersectLines( S, E, Ep1, Ep2, Ip );
                    FloatArray.Add( Ip.X );
                    FloatArray.Add( Ip.Y );
                }

                S.Set( E.X, E.Y );
            }

            FloatArray2.Clear();
            FloatArray2.AddAll( FloatArray );
            FloatArray.Clear();
        }

        if ( FloatArray2.Count != 0 )
        {
            if ( overlap != null )
            {
                if ( overlap.Vertices?.Length == FloatArray2.Count )
                {
                    Array.Copy( FloatArray2.ToArray(), 0, overlap.Vertices, 0, FloatArray2.Count );
                }
                else
                {
                    overlap.Vertices = FloatArray2.ToArray();
                }
            }

            return true;
        }

        return false;
    }

    /// <summary>
    ///     Returns true if the specified poygons intersect.
    /// </summary>
    public static bool IntersectPolygons( List< float > polygon1, List< float > polygon2 )
    {
        var p1Items = polygon1.ToArray();
        var p2Items = polygon2.ToArray();

        if ( IsPointInPolygon( p1Items,
                               0,
                               polygon1.Count,
                               p2Items[ 0 ],
                               p2Items[ 1 ]
                             ) )
        {
            return true;
        }

        return IsPointInPolygon( p2Items, 0, polygon2.Count, p1Items[ 0 ], p1Items[ 1 ] )
            || IntersectPolygonEdges( polygon1, polygon2 );
    }

    /// <summary>
    ///     Returns true if the lines of the specified poygons intersect.
    /// </summary>
    public static bool IntersectPolygonEdges( List< float > polygon1, List< float > polygon2 )
    {
        var last1 = polygon1.Count - 2;
        var last2 = polygon2.Count - 2;
        var p1    = polygon1.ToArray();
        var p2    = polygon2.ToArray();
        var x1    = p1[ last1 ];
        var y1    = p1[ last1 + 1 ];

        for ( var i = 0; i <= last1; i += 2 )
        {
            float x2 = p1[ i ],     y2 = p1[ i + 1 ];
            float x3 = p2[ last2 ], y3 = p2[ last2 + 1 ];

            for ( var j = 0; j <= last2; j += 2 )
            {
                float x4 = p2[ j ], y4 = p2[ j + 1 ];

                if ( IntersectSegments( x1, y1, x2, y2, x3, y3, x4, y4, null ) )
                {
                    return true;
                }

                x3 = x4;
                y3 = y4;
            }

            x1 = x2;
            y1 = y2;
        }

        return false;
    }

    /// <summary>
    ///     Returns the distance between the given line and point. Note the specified line is not a line segment.
    /// </summary>
    public static float DistanceLinePoint( float startX, float startY, float endX, float endY, float pointX, float pointY )
    {
        var normalLength = ( float ) Math.Sqrt( ( ( endX - startX ) * ( endX - startX ) ) + ( ( endY - startY ) * ( endY - startY ) ) );

        return Math.Abs( ( ( pointX - startX ) * ( endY - startY ) ) - ( ( pointY - startY ) * ( endX - startX ) ) ) / normalLength;
    }

    /// <summary>
    ///     Returns the distance between the given segment and point.
    /// </summary>
    public static float DistanceSegmentPoint( float startX, float startY, float endX, float endY, float pointX, float pointY )
    {
        return NearestSegmentPoint( startX, startY, endX, endY, pointX, pointY, V2A ).Dst( pointX, pointY );
    }

    /// <summary>
    ///     Returns the distance between the given segment and point.
    /// </summary>
    public static float DistanceSegmentPoint( Vector2 start, Vector2 end, Vector2 point )
    {
        return NearestSegmentPoint( start, end, point, V2A ).Dst( point );
    }

    /// <summary>
    ///     Returns a point on the segment nearest to the specified point.
    /// </summary>
    public static Vector2 NearestSegmentPoint( Vector2 start, Vector2 end, Vector2 point, Vector2 nearest )
    {
        var length2 = start.Dst2( end );

        if ( length2 == 0 )
        {
            return nearest.Set( start );
        }

        var t = ( ( ( point.X - start.X ) * ( end.X - start.X ) ) + ( ( point.Y - start.Y ) * ( end.Y - start.Y ) ) ) / length2;

        if ( t < 0 )
        {
            return nearest.Set( start );
        }

        return t > 1
                   ? nearest.Set( end )
                   : nearest.Set( start.X + ( t * ( end.X - start.X ) ),
                                  start.Y + ( t * ( end.Y - start.Y ) ) );
    }

    /// <summary>
    ///     Returns a point on the segment nearest to the specified point.
    /// </summary>
    public static Vector2 NearestSegmentPoint( float startX,
                                               float startY,
                                               float endX,
                                               float endY,
                                               float pointX,
                                               float pointY,
                                               Vector2 nearest )
    {
        var xDiff   = endX - startX;
        var yDiff   = endY - startY;
        var length2 = ( xDiff * xDiff ) + ( yDiff * yDiff );

        if ( length2 == 0 )
        {
            return nearest.Set( startX, startY );
        }

        var t = ( ( ( pointX - startX ) * ( endX - startX ) ) + ( ( pointY - startY ) * ( endY - startY ) ) ) / length2;

        if ( t < 0 )
        {
            return nearest.Set( startX, startY );
        }

        return t > 1
                   ? nearest.Set( endX, endY )
                   : nearest.Set( startX + ( t * ( endX - startX ) ),
                                  startY + ( t * ( endY - startY ) ) );
    }

    /// <summary>
    ///     Returns whether the given line segment intersects the given circle.
    /// </summary>
    /// <param name="start"> The start point of the line segment </param>
    /// <param name="end"> The end point of the line segment </param>
    /// <param name="center"> The center of the circle </param>
    /// <param name="squareRadius"> The squared Radius of the circle </param>
    /// <returns> Whether the line segment and the circle intersect </returns>
    public static bool IntersectSegmentCircle( Vector2 start, Vector2 end, Vector2 center, float squareRadius )
    {
        Tmp.Set( end.X - start.X, end.Y - start.Y, 0 );
        Tmp1.Set( center.X - start.X, center.Y - start.Y, 0 );
        var l = Tmp.Len();
        var u = Tmp1.Dot( Tmp.Nor() );

        if ( u <= 0 )
        {
            Tmp2.Set( start.X, start.Y, 0 );
        }
        else if ( u >= l )
        {
            Tmp2.Set( end.X, end.Y, 0 );
        }
        else
        {
            Tmp3.Set( Tmp.Scl( u ) ); // remember tmp is already normalized
            Tmp2.Set( Tmp3.X + start.X, Tmp3.Y + start.Y, 0 );
        }

        var x = center.X - Tmp2.X;
        var y = center.Y - Tmp2.Y;

        return ( ( x * x ) + ( y * y ) ) <= squareRadius;
    }

    /// <summary>
    ///     Returns whether the given line segment intersects the given circle.
    /// </summary>
    /// <param name="start"> The start point of the line segment </param>
    /// <param name="end"> The end point of the line segment </param>
    /// <param name="circle"> The circle </param>
    /// <param name="mtv">
    ///     A Minimum Translation Vector to fill in the case of a collision, or null (optional).
    /// </param>
    /// <returns> Whether the line segment and the circle intersect </returns>
    public static bool IntersectSegmentCircle( Vector2 start, Vector2 end, Circle circle, MinimumTranslationVector? mtv )
    {
        V2A.Set( end ).Sub( start );
        V2B.Set( circle.X - start.X, circle.Y - start.Y );

        var len = V2A.Len();
        var u   = V2B.Dot( V2A.Nor() );

        if ( u <= 0 )
        {
            V2C.Set( start );
        }
        else if ( u >= len )
        {
            V2C.Set( end );
        }
        else
        {
            V2D.Set( V2A.Scl( u ) ); // remember v2a is already normalized
            V2C.Set( V2D ).Add( start );
        }

        V2A.Set( V2C.X - circle.X, V2C.Y - circle.Y );

        if ( mtv != null )
        {
            // Handle special case of segment containing circle center
            if ( V2A.Equals( Vector2.Zero ) )
            {
                V2D.Set( end.Y - start.Y, start.X - end.X );
                mtv.Normal.Set( V2D ).Nor();
                mtv.Depth = circle.Radius;
            }
            else
            {
                mtv.Normal.Set( V2A ).Nor();
                mtv.Depth = circle.Radius - V2A.Len();
            }
        }

        return V2A.Len2() <= ( circle.Radius * circle.Radius );
    }

    /// <summary>
    ///     Intersect two 2D rays and return the scalar parameter of the first ray at
    ///     the intersection point.
    ///     <para>
    ///         You can get the intersection point by:
    ///         <code>Vector2 point = direction1.Scl(scalar).Add(start1);</code>
    ///         For more information, check:
    ///         <a href="http://stackoverflow.com/a/565282/1091440">StackOverflow Post</a>.
    ///     </para>
    /// </summary>
    /// <param name="start1">The starting point of the first ray.</param>
    /// <param name="direction1">The direction the first ray is pointing.</param>
    /// <param name="start2">The starting point of the second ray.</param>
    /// <param name="direction2">The direction the second ray is pointing.</param>
    /// <returns>
    ///     The scalar parameter on the first ray describing the point where the
    ///     intersection happens.
    ///     May be negative.
    ///     In case the rays are collinear, <see cref="float.PositiveInfinity" />
    ///     will be returned.
    /// </returns>
    public static float IntersectRayRay( Vector2 start1, Vector2 direction1, Vector2 start2, Vector2 direction2 )
    {
        var difx  = start2.X - start1.X;
        var dify  = start2.Y - start1.Y;
        var d1Xd2 = ( direction1.X * direction2.Y ) - ( direction1.Y * direction2.X );

        if ( d1Xd2 == 0.0f )
        {
            return float.PositiveInfinity; // collinear
        }

        var d2Sx = direction2.X / d1Xd2;
        var d2Sy = direction2.Y / d1Xd2;

        return ( difx * d2Sy ) - ( dify * d2Sx );
    }

    /// <summary>
    ///     Intersects a <see cref="Ray" /> and a <see cref="Maths.Plane" />. The intersection
    ///     point is stored in intersection in case an intersection is present.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="plane"></param>
    /// <param name="intersection"> The vector the intersection point is written to (optional) </param>
    /// <returns> Whether an intersection is present. </returns>
    public static bool IntersectRayPlane( Ray ray, Plane plane, Vector3? intersection )
    {
        var denom = ray.direction.Dot( plane.Normal );

        if ( denom != 0 )
        {
            var t = -( ray.origin.Dot( plane.Normal ) + plane.DistanceToOrigin ) / denom;

            if ( t < 0 )
            {
                return false;
            }

            intersection?.Set( ray.origin ).Add( V0.Set( ray.direction ).Scl( t ) );

            return true;
        }

        if ( plane.TestPoint( ray.origin ) == Plane.PlaneSide.OnPlane )
        {
            intersection?.Set( ray.origin );

            return true;
        }

        return false;
    }

    /// <summary>
    ///     Intersects a line and a plane. The intersection is returned as the
    ///     distance from the first point to the plane. In case an intersection
    ///     happened, the return value is in the range [0,1]. The intersection
    ///     point can be recovered by <tt>point1 + t * (point2 - point1)</tt> where t is
    ///     the return value of this method.
    /// </summary>
    public static float IntersectLinePlane( float x,
                                            float y,
                                            float z,
                                            float x2,
                                            float y2,
                                            float z2,
                                            Plane plane,
                                            Vector3? intersection )
    {
        var direction = Tmp.Set( x2, y2, z2 ).Sub( x, y, z );
        var origin    = Tmp2.Set( x, y, z );
        var denom     = direction.Dot( plane.Normal );

        if ( denom != 0 )
        {
            var t = -( origin.Dot( plane.Normal ) + plane.DistanceToOrigin ) / denom;

            intersection?.Set( origin ).Add( direction.Scl( t ) );

            return t;
        }

        if ( plane.TestPoint( origin ) == Plane.PlaneSide.OnPlane )
        {
            intersection?.Set( origin );

            return 0;
        }

        return -1;
    }

    /// <summary>
    ///     Returns true if the three <see cref="Maths.Plane" />s intersect, setting the point
    ///     of intersection in <tt>intersection</tt>, if any.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="intersection"> The point where the three planes intersect </param>
    public static bool IntersectPlanes( Plane a, Plane b, Plane c, Vector3 intersection )
    {
        Tmp1.Set( a.Normal ).Crs( b.Normal );
        Tmp2.Set( b.Normal ).Crs( c.Normal );
        Tmp3.Set( c.Normal ).Crs( a.Normal );

        var f = -a.Normal.Dot( Tmp2 );

        if ( Math.Abs( f ) < MathUtils.FLOAT_ROUNDING_ERROR )
        {
            return false;
        }

        Tmp1.Scl( c.DistanceToOrigin );
        Tmp2.Scl( a.DistanceToOrigin );
        Tmp3.Scl( b.DistanceToOrigin );

        intersection.Set( Tmp1.X + Tmp2.X + Tmp3.X, Tmp1.Y + Tmp2.Y + Tmp3.Y, Tmp1.Z + Tmp2.Z + Tmp3.Z );
        intersection.Scl( 1 / f );

        return true;
    }

    /// <summary>
    ///     Intersect a <see cref="Ray" /> and a triangle, returning the intersection
    ///     point in intersection.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="t1"> The first vertex of the triangle </param>
    /// <param name="t2"> The second vertex of the triangle </param>
    /// <param name="t3"> The third vertex of the triangle </param>
    /// <param name="intersection"> The intersection point (optional) </param>
    /// <returns> True in case an intersection is present. </returns>
    public static bool IntersectRayTriangle( Ray ray, Vector3 t1, Vector3 t2, Vector3 t3, Vector3? intersection )
    {
        var edge1 = V0.Set( t2 ).Sub( t1 );
        var edge2 = V1.Set( t3 ).Sub( t1 );

        var pvec = V2.Set( ray.direction ).Crs( edge2 );
        var det  = edge1.Dot( pvec );

        if ( MathUtils.IsZero( det ) )
        {
            Plane.Set( t1, t2, t3 );

            if ( ( Plane.TestPoint( ray.origin ) == Plane.PlaneSide.OnPlane )
              && IsPointInTriangle( ray.origin, t1, t2, t3 ) )
            {
                intersection?.Set( ray.origin );

                return true;
            }

            return false;
        }

        det = 1.0f / det;

        var tvec = Vec3.Set( ray.origin ).Sub( t1 );
        var u    = tvec.Dot( pvec ) * det;

        if ( u is < 0.0f or > 1.0f )
        {
            return false;
        }

        var qvec = tvec.Crs( edge1 );
        var v    = ray.direction.Dot( qvec ) * det;

        if ( ( v < 0.0f ) || ( ( u + v ) > 1.0f ) )
        {
            return false;
        }

        var t = edge2.Dot( qvec ) * det;

        if ( t < 0 )
        {
            return false;
        }

        if ( intersection != null )
        {
            if ( t <= MathUtils.FLOAT_ROUNDING_ERROR )
            {
                intersection.Set( ray.origin );
            }
            else
            {
                ray.GetEndPoint( intersection, t );
            }
        }

        return true;
    }

    /// <summary>
    ///     Intersects a {@link Ray} and a sphere, returning the intersection point in intersection.
    /// </summary>
    /// <param name="ray"> The ray, the direction component must be normalized before calling this method </param>
    /// <param name="center"> The center of the sphere </param>
    /// <param name="radius"> The radius of the sphere </param>
    /// <param name="intersection"> The intersection point (optional, can be null) </param>
    /// <returns> Whether an intersection is present. </returns>
    public static bool IntersectRaySphere( Ray ray, Vector3 center, float radius, Vector3? intersection )
    {
        var len = ray.direction.Dot( center.X - ray.origin.X, center.Y - ray.origin.Y, center.Z - ray.origin.Z );

        if ( len < 0.0f ) // behind the ray
        {
            return false;
        }

        var dst2 = center.Dst2(
                               ray.origin.X + ( ray.direction.X * len ),
                               ray.origin.Y + ( ray.direction.Y * len ),
                               ray.origin.Z + ( ray.direction.Z * len )
                              );

        var r2 = radius * radius;

        if ( dst2 > r2 )
        {
            return false;
        }

        intersection?.Set( ray.direction ).Scl( len - ( float ) Math.Sqrt( r2 - dst2 ) ).Add( ray.origin );

        return true;
    }

    /// <summary>
    ///     Intersects a <see cref="Ray" /> and a <see cref="BoundingBox" />, returning
    ///     the intersection point in <paramref name="intersection" />.
    ///     <para>
    ///         This intersection is defined as the point on the ray closest to the origin
    ///         which is within the specified bounds.
    ///     </para>
    ///     <para>
    ///         The returned intersection (if any) is guaranteed to be within the bounds of
    ///         the bounding box, but it can occasionally diverge slightly from the ray due
    ///         to small floating-point errors.
    ///     </para>
    ///     <para>
    ///         If the origin of the ray is inside the box, this method returns <tt>true</tt>
    ///         and the intersection point is set to the origin of the ray, accordingly to the
    ///         definition above.
    ///     </para>
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="box"></param>
    /// <param name="intersection">The intersection point (optional).</param>
    /// <returns>Whether an intersection is present.</returns>
    public static bool IntersectRayBounds( Ray ray, BoundingBox box, Vector3? intersection )
    {
        if ( box.Contains( ray.origin ) )
        {
            intersection?.Set( ray.origin );

            return true;
        }

        float lowest = 0, t;
        var   hit    = false;

        // min x
        if ( ( ray.origin.X <= box.Min.X ) && ( ray.direction.X > 0 ) )
        {
            t = ( box.Min.X - ray.origin.X ) / ray.direction.X;

            if ( t >= 0 )
            {
                V2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( V2.Y >= box.Min.Y ) && ( V2.Y <= box.Max.Y ) && ( V2.Z >= box.Min.Z ) && ( V2.Z <= box.Max.Z ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // max x
        if ( ( ray.origin.X >= box.Max.X ) && ( ray.direction.X < 0 ) )
        {
            t = ( box.Max.X - ray.origin.X ) / ray.direction.X;

            if ( t >= 0 )
            {
                V2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( V2.Y >= box.Min.Y ) && ( V2.Y <= box.Max.Y ) && ( V2.Z >= box.Min.Z ) && ( V2.Z <= box.Max.Z ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // min y
        if ( ( ray.origin.Y <= box.Min.Y ) && ( ray.direction.Y > 0 ) )
        {
            t = ( box.Min.Y - ray.origin.Y ) / ray.direction.Y;

            if ( t >= 0 )
            {
                V2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( V2.X >= box.Min.X ) && ( V2.X <= box.Max.X ) && ( V2.Z >= box.Min.Z ) && ( V2.Z <= box.Max.Z ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // max y
        if ( ( ray.origin.Y >= box.Max.Y ) && ( ray.direction.Y < 0 ) )
        {
            t = ( box.Max.Y - ray.origin.Y ) / ray.direction.Y;

            if ( t >= 0 )
            {
                V2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( V2.X >= box.Min.X ) && ( V2.X <= box.Max.X ) && ( V2.Z >= box.Min.Z ) && ( V2.Z <= box.Max.Z ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // min z
        if ( ( ray.origin.Z <= box.Min.Z ) && ( ray.direction.Z > 0 ) )
        {
            t = ( box.Min.Z - ray.origin.Z ) / ray.direction.Z;

            if ( t >= 0 )
            {
                V2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( V2.X >= box.Min.X )
                  && ( V2.X <= box.Max.X )
                  && ( V2.Y >= box.Min.Y )
                  && ( V2.Y <= box.Max.Y )
                  && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // max z
        if ( ( ray.origin.Z >= box.Max.Z ) && ( ray.direction.Z < 0 ) )
        {
            t = ( box.Max.Z - ray.origin.Z ) / ray.direction.Z;

            if ( t >= 0 )
            {
                V2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( V2.X >= box.Min.X )
                  && ( V2.X <= box.Max.X )
                  && ( V2.Y >= box.Min.Y )
                  && ( V2.Y <= box.Max.Y )
                  && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        if ( hit && ( intersection != null ) )
        {
            intersection.Set( ray.direction ).Scl( lowest ).Add( ray.origin );

            if ( intersection.X < box.Min.X )
            {
                intersection.X                                    = box.Min.X;
            }
            else if ( intersection.X > box.Max.X )
            {
                intersection.X = box.Max.X;
            }

            if ( intersection.Y < box.Min.Y )
            {
                intersection.Y                                    = box.Min.Y;
            }
            else if ( intersection.Y > box.Max.Y )
            {
                intersection.Y = box.Max.Y;
            }

            if ( intersection.Z < box.Min.Z )
            {
                intersection.Z                                    = box.Min.Z;
            }
            else if ( intersection.Z > box.Max.Z )
            {
                intersection.Z = box.Max.Z;
            }
        }

        return hit;
    }

    /// <summary>
    ///     Quick check whether the given <see cref="Ray" /> and <see cref="BoundingBox" /> intersect.
    /// </summary>
    /// <returns> Whether the ray and the bounding box intersect. </returns>
    public static bool IntersectRayBoundsFast( Ray ray, BoundingBox box )
    {
        return IntersectRayBoundsFast( ray, box.GetCenter( Tmp1 ), box.GetDimensions( Tmp2 ) );
    }

    /// <summary>
    ///     Quick check whether the given {@link Ray} and {@link BoundingBox} intersect.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="center"> The center of the bounding box </param>
    /// <param name="dimensions"> The dimensions (width, height and depth) of the bounding box </param>
    /// <returns> Whether the ray and the bounding box intersect. </returns>
    public static bool IntersectRayBoundsFast( Ray ray, Vector3 center, Vector3 dimensions )
    {
        var divX = 1f / ray.direction.X;
        var divY = 1f / ray.direction.Y;
        var divZ = 1f / ray.direction.Z;

        var minx = ( center.X - ( dimensions.X * 0.5f ) - ray.origin.X ) * divX;
        var maxx = ( ( center.X + ( dimensions.X * 0.5f ) ) - ray.origin.X ) * divX;

        if ( minx > maxx )
        {
            ( minx, maxx ) = ( maxx, minx );
        }

        var miny = ( center.Y - ( dimensions.Y * 0.5f ) - ray.origin.Y ) * divY;
        var maxy = ( ( center.Y + ( dimensions.Y * 0.5f ) ) - ray.origin.Y ) * divY;

        if ( miny > maxy )
        {
            ( miny, maxy ) = ( maxy, miny );
        }

        var minz = ( center.Z - ( dimensions.Z * 0.5f ) - ray.origin.Z ) * divZ;
        var maxz = ( ( center.Z + ( dimensions.Z * 0.5f ) ) - ray.origin.Z ) * divZ;

        if ( minz > maxz )
        {
            ( minz, maxz ) = ( maxz, minz );
        }

        var min = Math.Max( Math.Max( minx, miny ), minz );
        var max = Math.Min( Math.Min( maxx, maxy ), maxz );

        return ( max >= 0 ) && ( max >= min );
    }

    /// <summary>
    ///     Quick check whether the given <see cref="Ray" /> and Oriented
    ///     <see cref="BoundingBox" /> intersect.
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="bounds"></param>
    /// <param name="matrix"> The orientation of the bounding box </param>
    /// <returns> Whether the ray and the oriented bounding box intersect. </returns>
    public static bool IntersectRayOrientedBoundsFast( Ray ray, BoundingBox bounds, Matrix4 matrix )
    {
        var   tMin = 0.0f;
        var   tMax = float.MaxValue;
        float t1, t2;

        var oBBposition = matrix.GetTranslation( Tmp );
        var delta       = oBBposition.Sub( ray.origin );

        // Test intersection with the 2 planes perpendicular to the OBB's X axis
        var xaxis = Tmp1;

        Tmp1.Set( matrix.val[ Matrix4.M00 ], matrix.val[ Matrix4.M10 ], matrix.val[ Matrix4.M20 ] );

        var e = xaxis.Dot( delta );
        var f = ray.direction.Dot( xaxis );

        if ( Math.Abs( f ) > MathUtils.FLOAT_ROUNDING_ERROR )
        {
            // Standard case
            t1 = ( e + bounds.Min.X ) / f; // Intersection with the "left" plane
            t2 = ( e + bounds.Max.X ) / f; // Intersection with the "right" plane

            // t1 and t2 now contain distances betwen ray origin and ray-plane intersections

            // We want t1 to represent the nearest intersection,
            // so if it's not the case, invert t1 and t2
            if ( t1 > t2 )
            {
                ( t1, t2 ) = ( t2, t1 );
            }

            // tMax is the nearest "far" intersection (amongst the X,Y and Z planes pairs)
            if ( t2 < tMax )
            {
                tMax = t2;
            }

            // tMin is the farthest "near" intersection (amongst the X,Y and Z planes pairs)
            if ( t1 > tMin )
            {
                tMin = t1;
            }

            // And here's the trick :
            // If "far" is closer than "near", then there is NO intersection.
            // See the images in the tutorials for the visual explanation.
            if ( tMax < tMin )
            {
                return false;
            }

            // Rare case : the ray is almost parallel to the planes, so they don't have any "intersection"
        }
        else if ( ( ( -e + bounds.Min.X ) > 0.0f ) || ( ( -e + bounds.Max.X ) < 0.0f ) )
        {
            return false;
        }

        // Test intersection with the 2 planes perpendicular to the OBB's Y axis
        // Exactly the same thing than above.
        var yaxis = Tmp2;
        Tmp2.Set( matrix.val[ Matrix4.M01 ], matrix.val[ Matrix4.M11 ], matrix.val[ Matrix4.M21 ] );

        e = yaxis.Dot( delta );
        f = ray.direction.Dot( yaxis );

        if ( Math.Abs( f ) > MathUtils.FLOAT_ROUNDING_ERROR )
        {
            t1 = ( e + bounds.Min.Y ) / f;
            t2 = ( e + bounds.Max.Y ) / f;

            if ( t1 > t2 )
            {
                ( t1, t2 ) = ( t2, t1 );
            }

            if ( t2 < tMax )
            {
                tMax = t2;
            }

            if ( t1 > tMin )
            {
                tMin = t1;
            }

            if ( tMin > tMax )
            {
                return false;
            }
        }
        else if ( ( ( -e + bounds.Min.Y ) > 0.0f ) || ( ( -e + bounds.Max.Y ) < 0.0f ) )
        {
            return false;
        }

        // Test intersection with the 2 planes perpendicular to the OBB's Z axis
        // Exactly the same thing than above.
        var zaxis = Tmp3;
        Tmp3.Set( matrix.val[ Matrix4.M02 ], matrix.val[ Matrix4.M12 ], matrix.val[ Matrix4.M22 ] );

        e = zaxis.Dot( delta );
        f = ray.direction.Dot( zaxis );

        if ( Math.Abs( f ) > MathUtils.FLOAT_ROUNDING_ERROR )
        {
            t1 = ( e + bounds.Min.Z ) / f;
            t2 = ( e + bounds.Max.Z ) / f;

            if ( t1 > t2 )
            {
                ( t1, t2 ) = ( t2, t1 );
            }

            if ( t2 < tMax )
            {
                tMax = t2;
            }

            if ( t1 > tMin )
            {
                tMin = t1;
            }

            if ( tMin > tMax )
            {
                return false;
            }
        }
        else if ( ( ( -e + bounds.Min.Z ) > 0.0f ) || ( ( -e + bounds.Max.Z ) < 0.0f ) )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Intersects the given ray with list of triangles. Returns the nearest
    ///     intersection point in intersection
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="triangles">
    ///     The triangles, each successive 9 elements are the 3 vertices of a triangle,
    ///     a vertex is made of 3 successive floats (XYZ)
    /// </param>
    /// <param name="intersection"> The nearest intersection point (optional) </param>
    /// <returns> Whether the ray and the triangles intersect. </returns>
    public static bool IntersectRayTriangles( Ray ray, float[] triangles, Vector3? intersection )
    {
        var minDist = float.MaxValue;
        var hit     = false;

        if ( ( triangles.Length % 9 ) != 0 )
        {
            throw new GdxRuntimeException( "triangles array size is not a multiple of 9" );
        }

        for ( var i = 0; i < triangles.Length; i += 9 )
        {
            var result = IntersectRayTriangle(
                                              ray,
                                              Tmp1.Set( triangles[ i ], triangles[ i + 1 ], triangles[ i + 2 ] ),
                                              Tmp2.Set( triangles[ i + 3 ], triangles[ i + 4 ], triangles[ i + 5 ] ),
                                              Tmp3.Set( triangles[ i + 6 ], triangles[ i + 7 ], triangles[ i + 8 ] ),
                                              Tmp
                                             );

            if ( result )
            {
                var dist = ray.origin.Dst2( Tmp );

                if ( dist < minDist )
                {
                    minDist = dist;
                    Best.Set( Tmp );
                    hit = true;
                }
            }
        }

        if ( !hit )
        {
            return false;
        }

        intersection?.Set( Best );

        return true;
    }

    /// <summary>
    ///     Intersects the given ray with list of triangles. Returns the nearest intersection point in intersection
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="vertices"></param>
    /// <param name="indices"> the indices, each successive 3 shorts index the 3 vertices of a triangle </param>
    /// <param name="vertexSize"> the size of a vertex in floats </param>
    /// <param name="intersection"> The nearest intersection point (optional) </param>
    /// <returns> Whether the ray and the triangles intersect. </returns>
    public static bool IntersectRayTriangles( Ray ray,
                                              float[] vertices,
                                              short[] indices,
                                              int vertexSize,
                                              Vector3? intersection )
    {
        var minDist = float.MaxValue;
        var hit     = false;

        if ( ( indices.Length % 3 ) != 0 )
        {
            throw new GdxRuntimeException( "triangle list size is not a multiple of 3" );
        }

        for ( var i = 0; i < indices.Length; i += 3 )
        {
            var i1 = indices[ i ] * vertexSize;
            var i2 = indices[ i + 1 ] * vertexSize;
            var i3 = indices[ i + 2 ] * vertexSize;

            var result = IntersectRayTriangle(
                                              ray,
                                              Tmp1.Set( vertices[ i1 ], vertices[ i1 + 1 ], vertices[ i1 + 2 ] ),
                                              Tmp2.Set( vertices[ i2 ], vertices[ i2 + 1 ], vertices[ i2 + 2 ] ),
                                              Tmp3.Set( vertices[ i3 ], vertices[ i3 + 1 ], vertices[ i3 + 2 ] ),
                                              Tmp
                                             );

            if ( result )
            {
                var dist = ray.origin.Dst2( Tmp );

                if ( dist < minDist )
                {
                    minDist = dist;
                    Best.Set( Tmp );
                    hit = true;
                }
            }
        }

        if ( !hit )
        {
            return false;
        }

        intersection?.Set( Best );

        return true;
    }

    /// <summary>
    ///     Intersects the given ray with list of triangles. Returns the nearest intersection point in intersection
    /// </summary>
    /// <param name="ray"></param>
    /// <param name="triangles"> The triangles, each successive 3 elements are the 3 vertices of a triangle </param>
    /// <param name="intersection"> The nearest intersection point (optional) </param>
    /// <returns> Whether the ray and the triangles intersect. </returns>
    public static bool IntersectRayTriangles( Ray ray, List< Vector3 > triangles, Vector3? intersection )
    {
        var minDist = float.MaxValue;
        var hit     = false;

        if ( ( triangles.Count % 3 ) != 0 )
        {
            throw new GdxRuntimeException( "triangle list size is not a multiple of 3" );
        }

        for ( var i = 0; i < triangles.Count; i += 3 )
        {
            var result = IntersectRayTriangle( ray, triangles[ i ], triangles[ i + 1 ], triangles[ i + 2 ], Tmp );

            if ( result )
            {
                var dist = ray.origin.Dst2( Tmp );

                if ( dist < minDist )
                {
                    minDist = dist;
                    Best.Set( Tmp );
                    hit = true;
                }
            }
        }

        if ( !hit )
        {
            return false;
        }

        intersection?.Set( Best );

        return true;
    }

    /// <summary>
    ///     Quick check whether the given <see cref="BoundingBox" /> and <see cref="Maths.Plane" /> intersect.
    /// </summary>
    /// <returns> Whether the bounding box and the plane intersect. </returns>
    public static bool IntersectBoundsPlaneFast( BoundingBox box, Plane plane )
    {
        return IntersectBoundsPlaneFast( box.GetCenter( Tmp1 ),
                                         box.GetDimensions( Tmp2 ).Scl( 0.5f ),
                                         plane.Normal,
                                         plane.DistanceToOrigin );
    }

    /// <summary>
    ///     Quick check whether the given bounding box and a plane intersect. Code adapted from Christer
    ///     Ericson's Real Time Collision
    /// </summary>
    /// <param name="center"> The center of the bounding box </param>
    /// <param name="halfDimensions"> Half of the dimensions (width, height and depth) of the bounding box </param>
    /// <param name="normal"> The normal of the plane </param>
    /// <param name="distance"> The distance of the plane </param>
    /// <returns> Whether the bounding box and the plane intersect. </returns>
    public static bool IntersectBoundsPlaneFast( Vector3 center, Vector3 halfDimensions, Vector3 normal, float distance )
    {
        // Compute the projection interval Radius of b onto L(t) = b.c + t * p.n
        var radius = ( halfDimensions.X * Math.Abs( normal.X ) )
                   + ( halfDimensions.Y * Math.Abs( normal.Y ) )
                   + ( halfDimensions.Z * Math.Abs( normal.Z ) );

        // Compute distance of box center from plane
        var s = normal.Dot( center ) - distance;

        // Intersection occurs when plane distance falls within [-r,+r] interval
        return Math.Abs( s ) <= radius;
    }

    /// <summary>
    ///     Intersects the two lines and returns the intersection point in intersection.
    /// </summary>
    /// <param name="p1"> The first point of the first line </param>
    /// <param name="p2"> The second point of the first line </param>
    /// <param name="p3"> The first point of the second line </param>
    /// <param name="p4"> The second point of the second line </param>
    /// <param name="intersection"> The intersection point. May be null. </param>
    /// <returns> Whether the two lines intersect </returns>
    public static bool IntersectLines( Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2? intersection )
    {
        float x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y, x3 = p3.X, y3 = p3.Y, x4 = p4.X, y4 = p4.Y;

        var d = ( ( y4 - y3 ) * ( x2 - x1 ) ) - ( ( x4 - x3 ) * ( y2 - y1 ) );

        if ( d == 0 )
        {
            return false;
        }

        if ( intersection != null )
        {
            var ua = ( ( ( x4 - x3 ) * ( y1 - y3 ) ) - ( ( y4 - y3 ) * ( x1 - x3 ) ) ) / d;
            intersection.Set( x1 + ( ( x2 - x1 ) * ua ), y1 + ( ( y2 - y1 ) * ua ) );
        }

        return true;
    }

    /// <summary>
    ///     Intersects the two lines and returns the intersection point in intersection.
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <param name="x3"></param>
    /// <param name="y3"></param>
    /// <param name="x4"></param>
    /// <param name="y4"></param>
    /// <param name="intersection"> The intersection point, or null. </param>
    /// <returns> Whether the two lines intersect </returns>
    public static bool IntersectLines( float x1,
                                       float y1,
                                       float x2,
                                       float y2,
                                       float x3,
                                       float y3,
                                       float x4,
                                       float y4,
                                       Vector2? intersection )
    {
        var d = ( ( y4 - y3 ) * ( x2 - x1 ) ) - ( ( x4 - x3 ) * ( y2 - y1 ) );

        if ( d == 0 )
        {
            return false;
        }

        if ( intersection != null )
        {
            var ua = ( ( ( x4 - x3 ) * ( y1 - y3 ) ) - ( ( y4 - y3 ) * ( x1 - x3 ) ) ) / d;
            intersection.Set( x1 + ( ( x2 - x1 ) * ua ), y1 + ( ( y2 - y1 ) * ua ) );
        }

        return true;
    }

    /// <summary>
    ///     Check whether the given line and <see cref="Polygon" /> intersect.
    /// </summary>
    /// <param name="p1"> The first point of the line </param>
    /// <param name="p2"> The second point of the line </param>
    /// <param name="polygon"> The polygon </param>
    /// <returns> Whether polygon and line intersects </returns>
    public static bool IntersectLinePolygon( Vector2 p1, Vector2 p2, Polygon polygon )
    {
        var vertices = polygon.TransformedVertices;
        var n        = vertices!.Length;
        var x1       = p1.X;
        var y1       = p1.Y;
        var x2       = p2.X;
        var y2       = p2.Y;
        var x3       = vertices[ n - 2 ];
        var y3       = vertices[ n - 1 ];

        for ( var i = 0; i < n; i += 2 )
        {
            float x4 = vertices[ i ], y4 = vertices[ i + 1 ];
            var   d  = ( ( y4 - y3 ) * ( x2 - x1 ) ) - ( ( x4 - x3 ) * ( y2 - y1 ) );

            if ( d != 0 )
            {
                var yd = y1 - y3;
                var xd = x1 - x3;
                var ua = ( ( ( x4 - x3 ) * yd ) - ( ( y4 - y3 ) * xd ) ) / d;

                if ( ua is >= 0 and <= 1 )
                {
                    return true;
                }
            }

            x3 = x4;
            y3 = y4;
        }

        return false;
    }

    /// <summary>
    ///     Determines whether the given rectangles intersect and, if they do, sets the
    ///     supplied <tt>intersection</tt> rectangle to the area of overlap.
    /// </summary>
    /// <returns> Whether the rectangles intersect </returns>
    public static bool IntersectRectangles( RectangleShape rectangle1, RectangleShape rectangle2, RectangleShape intersection )
    {
        if ( rectangle1.Overlaps( rectangle2 ) )
        {
            intersection.X      = Math.Max( rectangle1.X, rectangle2.X );
            intersection.Width  = Math.Min( rectangle1.X + rectangle1.Width, rectangle2.X + rectangle2.Width ) - intersection.X;
            intersection.Y      = Math.Max( rectangle1.Y, rectangle2.Y );
            intersection.Height = Math.Min( rectangle1.Y + rectangle1.Height, rectangle2.Y + rectangle2.Height ) - intersection.Y;

            return true;
        }

        return false;
    }

    /// <summary>
    ///     Determines whether the given rectangle and segment intersect
    /// </summary>
    /// <param name="startX"> x-coordinate start of line segment </param>
    /// <param name="startY"> y-coordinate start of line segment </param>
    /// <param name="endX"> y-coordinate end of line segment </param>
    /// <param name="endY"> y-coordinate end of line segment </param>
    /// <param name="rectangle"> rectangle that is being tested for collision </param>
    /// <returns> whether the rectangle intersects with the line segment </returns>
    public static bool IntersectSegmentRectangle( float startX, float startY, float endX, float endY, RectangleShape rectangle )
    {
        var rectangleEndX = rectangle.X + rectangle.Width;
        var rectangleEndY = rectangle.Y + rectangle.Height;

        if ( IntersectSegments( startX, startY, endX, endY, rectangle.X, rectangle.Y, rectangle.X, rectangleEndY, null ) )
        {
            return true;
        }

        if ( IntersectSegments( startX, startY, endX, endY, rectangle.X, rectangle.Y, rectangleEndX, rectangle.Y, null ) )
        {
            return true;
        }

        if ( IntersectSegments( startX, startY, endX, endY, rectangleEndX, rectangle.Y, rectangleEndX, rectangleEndY, null ) )
        {
            return true;
        }

        return IntersectSegments( startX, startY, endX, endY, rectangle.X, rectangleEndY, rectangleEndX, rectangleEndY, null )
            || rectangle.Contains( startX, startY );
    }

    public static bool IntersectSegmentRectangle( Vector2 startvec, Vector2 endvec, RectangleShape rectangle )
    {
        return IntersectSegmentRectangle( startvec.X, startvec.Y, endvec.X, endvec.Y, rectangle );
    }

    /// <summary>
    ///     Check whether the given line segment and <see cref="Polygon" /> intersect.
    /// </summary>
    /// <param name="p1"> The first point of the segment </param>
    /// <param name="p2"> The second point of the segment </param>
    /// <param name="polygon"></param>
    /// <returns> Whether polygon and segment intersect </returns>
    public static bool IntersectSegmentPolygon( Vector2 p1, Vector2 p2, Polygon? polygon )
    {
        ArgumentNullException.ThrowIfNull( polygon );

        var vertices = polygon.TransformedVertices;
        var x1       = p1.X;
        var y1       = p1.Y;
        var x2       = p2.X;
        var y2       = p2.Y;
        var n        = vertices!.Length;
        var x3       = vertices[ n - 2 ];
        var y3       = vertices[ n - 1 ];

        for ( var i = 0; i < n; i += 2 )
        {
            float x4 = vertices[ i ], y4 = vertices[ i + 1 ];
            var   d  = ( ( y4 - y3 ) * ( x2 - x1 ) ) - ( ( x4 - x3 ) * ( y2 - y1 ) );

            if ( d != 0 )
            {
                var yd = y1 - y3;
                var xd = x1 - x3;
                var ua = ( ( ( x4 - x3 ) * yd ) - ( ( y4 - y3 ) * xd ) ) / d;

                if ( ua is >= 0 and <= 1 )
                {
                    var ub = ( ( ( x2 - x1 ) * yd ) - ( ( y2 - y1 ) * xd ) ) / d;

                    if ( ub is >= 0 and <= 1 )
                    {
                        return true;
                    }
                }
            }

            x3 = x4;
            y3 = y4;
        }

        return false;
    }

    /// <summary>
    ///     Intersects the two line segments and returns the intersection point in intersection.
    /// </summary>
    /// <param name="p1"> The first point of the first line segment </param>
    /// <param name="p2"> The second point of the first line segment </param>
    /// <param name="p3"> The first point of the second line segment </param>
    /// <param name="p4"> The second point of the second line segment </param>
    /// <param name="intersection"> The intersection point. May be null. </param>
    /// <returns> Whether the two line segments intersect </returns>
    public static bool IntersectSegments( Vector2 p1,
                                          Vector2 p2,
                                          Vector2 p3,
                                          Vector2 p4,
                                          Vector2? intersection )
    {
        float x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y, x3 = p3.X, y3 = p3.Y, x4 = p4.X, y4 = p4.Y;

        var d = ( ( y4 - y3 ) * ( x2 - x1 ) ) - ( ( x4 - x3 ) * ( y2 - y1 ) );

        if ( d == 0 )
        {
            return false;
        }

        var yd = y1 - y3;
        var xd = x1 - x3;
        var ua = ( ( ( x4 - x3 ) * yd ) - ( ( y4 - y3 ) * xd ) ) / d;

        if ( ua is < 0 or > 1 )
        {
            return false;
        }

        var ub = ( ( ( x2 - x1 ) * yd ) - ( ( y2 - y1 ) * xd ) ) / d;

        if ( ub is < 0 or > 1 )
        {
            return false;
        }

        intersection?.Set( x1 + ( ( x2 - x1 ) * ua ), y1 + ( ( y2 - y1 ) * ua ) );

        return true;
    }

    public static bool IntersectSegments( float x1,
                                          float y1,
                                          float x2,
                                          float y2,
                                          float x3,
                                          float y3,
                                          float x4,
                                          float y4,
                                          Vector2? intersection )
    {
        var d = ( ( y4 - y3 ) * ( x2 - x1 ) ) - ( ( x4 - x3 ) * ( y2 - y1 ) );

        if ( d == 0 )
        {
            return false;
        }

        var yd = y1 - y3;
        var xd = x1 - x3;
        var ua = ( ( ( x4 - x3 ) * yd ) - ( ( y4 - y3 ) * xd ) ) / d;

        if ( ua is < 0 or > 1 )
        {
            return false;
        }

        var ub = ( ( ( x2 - x1 ) * yd ) - ( ( y2 - y1 ) * xd ) ) / d;

        if ( ub is < 0 or > 1 )
        {
            return false;
        }

        intersection?.Set( x1 + ( ( x2 - x1 ) * ua ), y1 + ( ( y2 - y1 ) * ua ) );

        return true;
    }

    private static float Det( float a, float b, float c, float d )
    {
        return ( a * d ) - ( b * c );
    }

    private static double Detd( double a, double b, double c, double d )
    {
        return ( a * d ) - ( b * c );
    }

    public static bool Overlaps( Circle c1, Circle c2 )
    {
        return c1.Overlaps( c2 );
    }

    public static bool Overlaps( RectangleShape r1, RectangleShape r2 )
    {
        return r1.Overlaps( r2 );
    }

    public static bool Overlaps( Circle c, RectangleShape r )
    {
        var closestX = c.X;
        var closestY = c.Y;

        if ( c.X < r.X )
        {
            closestX                                 = r.X;
        }
        else if ( c.X > ( r.X + r.Width ) )
        {
            closestX = r.X + r.Width;
        }

        if ( c.Y < r.Y )
        {
            closestY                                  = r.Y;
        }
        else if ( c.Y > ( r.Y + r.Height ) )
        {
            closestY = r.Y + r.Height;
        }

        closestX -= c.X;
        closestX *= closestX;
        closestY -= c.Y;
        closestY *= closestY;

        return ( closestX + closestY ) < ( c.Radius * c.Radius );
    }

    /// <summary>
    ///     Check whether specified convex polygons overlap (clockwise or
    ///     counter-clockwise round doesn't matter).
    /// </summary>
    /// <param name="p1"> The first polygon. </param>
    /// <param name="p2"> The second polygon. </param>
    /// <returns> Whether polygons overlap. </returns>
    public static bool OverlapConvexPolygons( Polygon p1, Polygon p2 )
    {
        return OverlapConvexPolygons( p1, p2, ref _dummyMinimumTranslationVector );
    }

    /// <summary>
    ///     Check whether convex polygons overlap (clockwise or counter-clockwise
    ///     wound doesn't matter). If they do, optionally obtain a Minimum Translation
    ///     Vector indicating the minimum magnitude vector required to push the polygon
    ///     p1 out of collision with polygon p2.
    /// </summary>
    /// <param name="p1"> The first polygon. </param>
    /// <param name="p2"> The second polygon. </param>
    /// <param name="mtv">
    ///     A Minimum Translation Vector to fill in the case of a collision,
    ///     or null (optional).
    /// </param>
    /// <returns> Whether polygons overlap. </returns>
    public static bool OverlapConvexPolygons( Polygon p1, Polygon p2, ref MinimumTranslationVector? mtv )
    {
        ArgumentNullException.ThrowIfNull( p1.TransformedVertices );
        ArgumentNullException.ThrowIfNull( p2.TransformedVertices );

        return OverlapConvexPolygons( p1.TransformedVertices, p2.TransformedVertices, ref mtv );
    }

    public static bool OverlapConvexPolygons( float[] verts1, float[] verts2, ref MinimumTranslationVector? mtv )
    {
        return OverlapConvexPolygons( verts1, 0, verts1.Length, verts2, 0, verts2.Length, ref mtv );
    }

    /// <summary>
    ///     Check whether polygons defined by the given vertex arrays overlap
    ///     (clockwise or counter-clockwise wound doesn't matter). If they do,
    ///     optionally obtain a Minimum Translation Vector indicating the minimum
    ///     magnitude vector required to push the polygon defined by <paramref name="verts1" />
    ///     out of the collision with the polygon defined by <paramref name="verts2" />.
    /// </summary>
    /// <param name="verts1">Vertices of the first polygon.</param>
    /// <param name="offset1">The offset of the <paramref name="verts1" /> array.</param>
    /// <param name="count1">The amount that is added to the <paramref name="offset1" />.</param>
    /// <param name="verts2">Vertices of the second polygon.</param>
    /// <param name="offset2">The offset of the <paramref name="verts2" /> array.</param>
    /// <param name="count2">The amount that is added to the <paramref name="offset2" />.</param>
    /// <param name="mtv">
    ///     A Minimum Translation Vector to fill in the case of a collision,
    ///     or <see langword="null" /> (optional).
    /// </param>
    /// <returns>Whether polygons overlap.</returns>
    public static bool OverlapConvexPolygons( float[] verts1,
                                              int offset1,
                                              int count1,
                                              float[] verts2,
                                              int offset2,
                                              int count2,
                                              ref MinimumTranslationVector? mtv )
    {
        if ( ( mtv != null ) && ( mtv != _dummyMinimumTranslationVector ) )
        {
            mtv.Depth = float.MaxValue;
            mtv.Normal.SetZero();
        }

        var overlaps = OverlapsOnAxisOfShape( verts2,
                                              offset2,
                                              count2,
                                              verts1,
                                              offset1,
                                              count1,
                                              ref mtv!,
                                              true );

        if ( overlaps )
        {
            overlaps = OverlapsOnAxisOfShape( verts1,
                                              offset1,
                                              count1,
                                              verts2,
                                              offset2,
                                              count2,
                                              ref mtv,
                                              false );
        }

        if ( !overlaps )
        {
            mtv.Depth = 0;
            mtv.Normal.SetZero();

            return false;
        }

        return true;
    }

    /// <summary>
    ///     Implementation of the Separating Axis Theorem (SAT) algorithm.
    /// </summary>
    /// <param name="verts1"></param>
    /// <param name="offset1">Offset of vertices in the first shape.</param>
    /// <param name="count1">Count of vertices in the first shape.</param>
    /// <param name="verts2"></param>
    /// <param name="offset2">Offset of vertices in the second shape.</param>
    /// <param name="count2">Count of vertices in the second shape.</param>
    /// <param name="mtv">The minimum translation vector.</param>
    /// <param name="shapesShifted">
    ///     Indicates if shapes A and B are shifted, which is important for
    ///     calculating the axis translation for vertices in the first shape.
    /// </param>
    private static bool OverlapsOnAxisOfShape( float[] verts1,
                                               int offset1,
                                               int count1,
                                               float[] verts2,
                                               int offset2,
                                               int count2,
                                               ref MinimumTranslationVector mtv,
                                               bool shapesShifted )
    {
        var endA = offset1 + count1;
        var endB = offset2 + count2;

        // get axis of polygon A
        for ( var i = offset1; i < endA; i += 2 )
        {
            var x1 = verts1[ i ];
            var y1 = verts1[ i + 1 ];
            var x2 = verts1[ ( i + 2 ) % count1 ];
            var y2 = verts1[ ( i + 3 ) % count1 ];

            // Get the Axis for the 2 vertices
            var axisX = y1 - y2;
            var axisY = -( x1 - x2 );

            var len = ( float ) Math.Sqrt( ( axisX * axisX ) + ( axisY * axisY ) );

            // We got a normalized Vector
            axisX /= len;
            axisY /= len;

            var minA = float.MaxValue;
            var maxA = -float.MaxValue;

            // project shape a on axis
            for ( var v = offset1; v < endA; v += 2 )
            {
                var p = ( verts1[ v ] * axisX ) + ( verts1[ v + 1 ] * axisY );
                minA = Math.Min( minA, p );
                maxA = Math.Max( maxA, p );
            }

            var minB = float.MaxValue;
            var maxB = -float.MaxValue;

            // project shape b on axis
            for ( var v = offset2; v < endB; v += 2 )
            {
                var p = ( verts2[ v ] * axisX ) + ( verts2[ v + 1 ] * axisY );
                minB = Math.Min( minB, p );
                maxB = Math.Max( maxB, p );
            }

            // There is a gap
            if ( ( maxA < minB ) || ( maxB < minA ) )
            {
                return false;
            }

            if ( mtv != null )
            {
                var o          = Math.Min( maxA, maxB ) - Math.Max( minA, minB );
                var aContainsB = ( minA < minB ) && ( maxA > maxB );
                var bContainsA = ( minB < minA ) && ( maxB > maxA );

                // if it contains one or another
                float mins = 0;
                float maxs = 0;

                if ( aContainsB || bContainsA )
                {
                    mins =  Math.Abs( minA - minB );
                    maxs =  Math.Abs( maxA - maxB );
                    o    += Math.Min( mins, maxs );
                }

                if ( mtv.Depth > o )
                {
                    mtv.Depth = o;

                    if ( shapesShifted )
                    {
                        axisX = minA < minB ? axisX : -axisX;
                        axisY = minA < minB ? axisY : -axisY;
                    }
                    else
                    {
                        axisX = minA > minB ? axisX : -axisX;
                        axisY = minA > minB ? axisY : -axisY;
                    }

                    if ( aContainsB || bContainsA )
                    {
                        axisX = mins > maxs ? axisX : -axisX;
                        axisY = mins > maxs ? axisY : -axisY;
                    }

                    mtv.Normal.Set( axisX, axisY );
                }
            }
        }

        return true;
    }

    /// <summary>
    ///     Splits the triangle by the plane. The result is stored in the
    ///     <paramref name="split" /> instance.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Depending on where the triangle is relative to the plane, the result can be:
    ///     </para>
    ///     <para>
    ///         i) Triangle is fully in front/behind: <see cref="SplitTriangle.Front" /> or
    ///         <see cref="SplitTriangle.Back" /> will contain the original triangle, and
    ///         <see cref="SplitTriangle.Total" /> will be one.
    ///     </para>
    ///     <para>
    ///         ii) Triangle has two vertices in front, one behind: <see cref="SplitTriangle.Front" />
    ///         contains 2 triangles, <see cref="SplitTriangle.Back" /> contains 1 triangle, and
    ///         <see cref="SplitTriangle.Total" /> will be 3.
    ///     </para>
    ///     <para>
    ///         iii) Triangle has one vertex in front, two behind: <see cref="SplitTriangle.Front" />
    ///         contains 1 triangle, <see cref="SplitTriangle.Back" /> contains 2 triangles, and
    ///         <see cref="SplitTriangle.Total" /> will be 3.
    ///     </para>
    ///     <para>
    ///         The input triangle should have the form: x, y, z, x2, y2, z2, x3, y3, z3. You can
    ///         add additional attributes per vertex that will be interpolated if split, such as
    ///         texture coordinates or normals. Note that these additional attributes won't be
    ///         normalized, as might be necessary in case of normals.
    ///     </para>
    /// </remarks>
    /// <param name="triangle"></param>
    /// <param name="plane"></param>
    /// <param name="split">Output <see cref="SplitTriangle" /> to store the result of the split.</param>
    public static void DoSplitTriangle( float[] triangle, Plane plane, SplitTriangle split )
    {
        var stride = triangle.Length / 3;

        var r1 = plane.TestPoint( triangle[ 0 ], triangle[ 1 ], triangle[ 2 ] ) == Plane.PlaneSide.Back;

        var r2 = plane.TestPoint( triangle[ 0 + stride ], triangle[ 1 + stride ], triangle[ 2 + stride ] ) == Plane.PlaneSide.Back;

        var r3 = plane.TestPoint( triangle[ 0 + ( stride * 2 ) ], triangle[ 1 + ( stride * 2 ) ], triangle[ 2 + ( stride * 2 ) ] ) == Plane.PlaneSide.Back;

        split.Reset();

        // easy case, triangle is on one side (point on plane means front).
        if ( ( r1 == r2 ) && ( r2 == r3 ) )
        {
            split.Total = 1;

            if ( r1 )
            {
                split.NumBack = 1;
                Array.Copy( triangle, 0, split.Back, 0, triangle.Length );
            }
            else
            {
                split.NumFront = 1;
                Array.Copy( triangle, 0, split.Front, 0, triangle.Length );
            }

            return;
        }

        // set number of triangles
        split.Total    = 3;
        split.NumFront = ( r1 ? 0 : 1 ) + ( r2 ? 0 : 1 ) + ( r3 ? 0 : 1 );
        split.NumBack  = split.Total - split.NumFront;

        // hard case, split the three edges on the plane
        // determine which array to fill first, front or back, flip if we
        // cross the plane
        split.FrontCurrent = !r1;

        // split first edge
        var first  = 0;
        var second = stride;

        if ( r1 != r2 )
        {
            // split the edge
            SplitEdge( triangle, first, second, stride, plane, split.EdgeSplit, 0 );

            // Add first edge vertex and new vertex to current side
            split.Add( triangle, first, stride );
            split.Add( split.EdgeSplit, 0, stride );

            // flip side and Add new vertex and second edge vertex to current side
            split.FrontCurrent = !split.FrontCurrent;
            split.Add( split.EdgeSplit, 0, stride );
        }
        else
        {
            // Add both vertices
            split.Add( triangle, first, stride );
        }

        // split second edge
        first  = stride;
        second = stride + stride;

        if ( r2 != r3 )
        {
            // split the edge
            SplitEdge( triangle, first, second, stride, plane, split.EdgeSplit, 0 );

            // Add first edge vertex and new vertex to current side
            split.Add( triangle, first, stride );
            split.Add( split.EdgeSplit, 0, stride );

            // flip side and Add new vertex and second edge vertex to current side
            split.FrontCurrent = !split.FrontCurrent;
            split.Add( split.EdgeSplit, 0, stride );
        }
        else
        {
            // Add both vertices
            split.Add( triangle, first, stride );
        }

        // split third edge
        first  = stride + stride;
        second = 0;

        if ( r3 != r1 )
        {
            // split the edge
            SplitEdge( triangle, first, second, stride, plane, split.EdgeSplit, 0 );

            // Add first edge vertex and new vertex to current side
            split.Add( triangle, first, stride );
            split.Add( split.EdgeSplit, 0, stride );

            // flip side and Add new vertex and second edge vertex to current side
            split.FrontCurrent = !split.FrontCurrent;
            split.Add( split.EdgeSplit, 0, stride );
        }
        else
        {
            // Add both vertices
            split.Add( triangle, first, stride );
        }

        // triangulate the side with 2 triangles
        if ( split.NumFront == 2 )
        {
            Array.Copy( split.Front, stride * 2, split.Front, stride * 3, stride * 2 );
            Array.Copy( split.Front, 0, split.Front, stride * 5, stride );
        }
        else
        {
            Array.Copy( split.Back, stride * 2, split.Back, stride * 3, stride * 2 );
            Array.Copy( split.Back, 0, split.Back, stride * 5, stride );
        }
    }

    private static void SplitEdge( float[] vertices,
                                   int s,
                                   int e,
                                   int stride,
                                   Plane plane,
                                   float[] split,
                                   int offset )
    {
        var t = IntersectLinePlane( vertices[ s ],
                                    vertices[ s + 1 ],
                                    vertices[ s + 2 ],
                                    vertices[ e ],
                                    vertices[ e + 1 ],
                                    vertices[ e + 2 ],
                                    plane,
                                    Intersection );

        split[ offset + 0 ] = Intersection.X;
        split[ offset + 1 ] = Intersection.Y;
        split[ offset + 2 ] = Intersection.Z;

        for ( var i = 3; i < stride; i++ )
        {
            var a = vertices[ s + i ];
            var b = vertices[ e + i ];
            split[ offset + i ] = a + ( t * ( b - a ) );
        }
    }


    public class SplitTriangle
    {
        /// <summary>
        ///     Creates a new instance, assuming numAttributes attributes
        ///     per triangle vertex.
        /// </summary>
        /// <param name="numAttributes"> must be &gt;= 3 </param>
        public SplitTriangle( int numAttributes )
        {
            if ( numAttributes < 3 )
            {
                throw new ArgumentException( "numAttributes must be >= 3!" );
            }

            Front     = new float[ numAttributes * 3 * 2 ];
            Back      = new float[ numAttributes * 3 * 2 ];
            EdgeSplit = new float[ numAttributes ];
        }

        public int     NumBack      { get; set; }
        public int     NumFront     { get; set; }
        public int     Total        { get; set; }
        public float[] Front        { get; set; }
        public float[] Back         { get; set; }
        public bool    FrontCurrent { get; set; } = false;
        public float[] EdgeSplit    { get; set; }
        public int     FrontOffset  { get; set; } = 0;
        public int     BackOffset   { get; set; } = 0;

        public void Add( float[] vertex, int offset, int stride )
        {
            if ( FrontCurrent )
            {
                Array.Copy( vertex, offset, Front, FrontOffset, stride );
                FrontOffset += stride;
            }
            else
            {
                Array.Copy( vertex, offset, Back, BackOffset, stride );
                BackOffset += stride;
            }
        }

        public void Reset()
        {
            FrontCurrent = false;
            FrontOffset  = 0;
            BackOffset   = 0;
            NumFront     = 0;
            NumBack      = 0;
            Total        = 0;
        }

        public override string ToString()
        {
            return $"SplitTriangle [front={Front}, back={Back}, "
                 + $"numFront={NumFront}, numBack={{numBack}}, total={{total}}]";
        }
    }

    /// <summary>
    ///     Minimum translation required to separate two polygons.
    /// </summary>
    public record MinimumTranslationVector
    {
        // Distance of the translation required for the separation
        public float Depth { get; set; } = 0;

        // Unit length vector that indicates the direction for the separation
        public Vector2 Normal { get; set; } = new();
    }
}