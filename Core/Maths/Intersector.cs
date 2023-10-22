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

using System.Drawing;

using LibGDXSharp.Core.Utils.Collections;
using LibGDXSharp.Maths.Collision;

namespace LibGDXSharp.Maths;

/// <summary>
/// Class offering various static methods for intersection testing between different geometric objects.
/// </summary>
[PublicAPI]
public class Intersector
{
    private Intersector()
    {
    }

    private readonly static Vector3       V0          = new();
    private readonly static Vector3       V1          = new();
    private readonly static Vector3       V2          = new();
    private readonly static List< float > FloatArray  = new();
    private readonly static List< float > FloatArray2 = new();

    /// <summary>
    /// Returns whether the given point is inside the triangle. This assumes that
    /// the point is on the plane of the triangle. No check is performed that this
    /// is the case.
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
    /// Returns true if the given point is inside the triangle.
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
    /// Returns true if the given point is inside the triangle.
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
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="plane"></param>
    /// <param name="intersection"></param>
    /// <returns></returns>
    public static bool IntersectSegmentPlane( Vector3 start, Vector3 end, Plane plane, Vector3 intersection )
    {
        Vector3 dir   = V0.Set( end ).Sub( start );
        var     denom = dir.Dot( plane.Normal );

        if ( denom == 0f )
        {
            return false;
        }

        var t = -( start.Dot( plane.Normal ) + plane.DistanceToOrigin ) / denom;

        if ( t is < 0 or > 1 )
        {
            return false;
        }

        intersection.Set( start ).Add( dir.Scl( t ) );

        return true;
    }

    /// <summary>
    /// Determines on which side of the given line the point is. Returns 1 if the point is on the left side of the line, 0 if the
    /// point is on the line and -1 if the point is on the right side of the line. Left and right are relative to the lines direction
    /// which is linePoint1 to linePoint2. 
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

    private static Vector2 _ip  = new();
    private static Vector2 _ep1 = new();
    private static Vector2 _ep2 = new();
    private static Vector2 _s   = new();
    private static Vector2 _e   = new();

    /// <summary>
    /// Intersects two convex polygons with clockwise vertices and sets the overlap polygon resulting from the intersection.
    /// Follows the Sutherland-Hodgman algorithm.
    /// </summary>
    /// @param p1 The polygon that is being clipped
    /// @param p2 The clip polygon
    /// @param overlap The intersection of the two polygons (can be null, if an intersection polygon is not needed)
    /// @return Whether the two polygons intersect. */
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
            _ep1.Set( vertices2[ i ], vertices2[ i + 1 ] );

            // wrap around to beginning of array if index points to end;
            if ( i < last )
            {
                _ep2.Set( vertices2[ i + 2 ], vertices2[ i + 3 ] );
            }
            else
            {
                _ep2.Set( vertices2[ 0 ], vertices2[ 1 ] );
            }

            if ( FloatArray2.Count == 0 )
            {
                return false;
            }

            _s.Set( FloatArray2[ FloatArray2.Count - 2 ], FloatArray2[ FloatArray2.Count - 1 ] );

            for ( var j = 0; j < FloatArray2.Count; j += 2 )
            {
                _e.Set( FloatArray2[ j ], FloatArray2[ j + 1 ] );

                // determine if point is inside clip edge
                var side = PointLineSide( _ep2, _ep1, _s ) > 0;

                if ( Intersector.PointLineSide( _ep2, _ep1, _e ) > 0 )
                {
                    if ( !side )
                    {
                        Intersector.IntersectLines( _s, _e, _ep1, _ep2, _ip );

                        if ( ( FloatArray.Count < 2 )
                          || ( !FloatArray[ FloatArray.Count - 2 ].Equals( _ip.X ) )
                          || ( !FloatArray[ FloatArray.Count - 1 ].Equals( _ip.Y ) ) )
                        {
                            FloatArray.Add( _ip.X );
                            FloatArray.Add( _ip.Y );
                        }
                    }

                    FloatArray.Add( _e.X );
                    FloatArray.Add( _e.Y );
                }
                else if ( side )
                {
                    Intersector.IntersectLines( _s, _e, _ep1, _ep2, _ip );
                    FloatArray.Add( _ip.X );
                    FloatArray.Add( _ip.Y );
                }

                _s.Set( _e.X, _e.Y );
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
    /// Returns true if the specified poygons intersect.
    /// </summary>
    public static bool IntersectPolygons( List< float > polygon1, List< float > polygon2 )
    {
        if ( Intersector.IsPointInPolygon
                (
                 polygon1.items,
                 0,
                 polygon1.size,
                 polygon2.items[ 0 ],
                 polygon2.items[ 1 ]
                ) )
        {
            return true;
        }

        if ( Intersector.IsPointInPolygon
                (
                 polygon2.items,
                 0,
                 polygon2.size,
                 polygon1.items[ 0 ],
                 polygon1.items[ 1 ]
                ) )
        {
            return true;
        }

        return IntersectPolygonEdges( polygon1, polygon2 );
    }

    /// <summary>
    /// Returns true if the lines of the specified poygons intersect.
    /// </summary>
    public static bool IntersectPolygonEdges( List< float > polygon1, List< float > polygon2 )
    {
        int     last1 = polygon1.size - 2, last2 = polygon2.size - 2;
        float[] p1    = polygon1.items,    p2    = polygon2.items;
        float   x1    = p1[ last1 ],       y1    = p1[ last1 + 1 ];

        for ( var i = 0; i <= last1; i += 2 )
        {
            float x2 = p1[ i ],     y2 = p1[ i + 1 ];
            float x3 = p2[ last2 ], y3 = p2[ last2 + 1 ];

            for ( var j = 0; j <= last2; j += 2 )
            {
                float x4 = p2[ j ], y4 = p2[ j + 1 ];

                if ( intersectSegments( x1, y1, x2, y2, x3, y3, x4, y4, null ) )
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

    private static Vector2 _v2A = new();
    private static Vector2 _v2B = new();
    private static Vector2 _v2C = new();
    private static Vector2 _v2d = new();

    /** Returns the distance between the given line and point. Note the specified line is not a line segment. */
    public static float distanceLinePoint( float startX, float startY, float endX, float endY, float pointX, float pointY )
    {
        var normalLength = ( float )Math.Sqrt( ( ( endX - startX ) * ( endX - startX ) ) + ( ( endY - startY ) * ( endY - startY ) ) );

        return Math.Abs( ( ( pointX - startX ) * ( endY - startY ) ) - ( ( pointY - startY ) * ( endX - startX ) ) ) / normalLength;
    }

    /** Returns the distance between the given segment and point. */
    public static float distanceSegmentPoint( float startX, float startY, float endX, float endY, float pointX, float pointY )
    {
        return nearestSegmentPoint( startX, startY, endX, endY, pointX, pointY, _v2A ).dst( pointX, pointY );
    }

    /** Returns the distance between the given segment and point. */
    public static float distanceSegmentPoint( Vector2 start, Vector2 end, Vector2 point )
    {
        return nearestSegmentPoint( start, end, point, _v2A ).dst( point );
    }

    /** Returns a point on the segment nearest to the specified point. */
    public static Vector2 nearestSegmentPoint( Vector2 start, Vector2 end, Vector2 point, Vector2 nearest )
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

        if ( t > 1 )
        {
            return nearest.Set( end );
        }

        return nearest.Set( start.X + ( t * ( end.X - start.X ) ), start.Y + ( t * ( end.Y - start.Y ) ) );
    }

    /** Returns a point on the segment nearest to the specified point. */
    public static Vector2 nearestSegmentPoint( float startX,
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

        if ( t > 1 )
        {
            return nearest.Set( endX, endY );
        }

        return nearest.Set( startX + ( t * ( endX - startX ) ), startY + ( t * ( endY - startY ) ) );
    }

    /** Returns whether the given line segment intersects the given circle.
     * @param start The start point of the line segment
     * @param end The end point of the line segment
     * @param center The center of the circle
     * @param squareRadius The squared Radius of the circle
     * @return Whether the line segment and the circle intersect */
    public static bool intersectSegmentCircle( Vector2 start, Vector2 end, Vector2 center, float squareRadius )
    {
        tmp.Set( end.X - start.X, end.Y - start.Y, 0 );
        tmp1.Set( center.X - start.X, center.Y - start.Y, 0 );
        var l = tmp.Len();
        var u = tmp1.Dot( tmp.Nor() );

        if ( u <= 0 )
        {
            tmp2.Set( start.X, start.Y, 0 );
        }
        else if ( u >= l )
        {
            tmp2.Set( end.X, end.Y, 0 );
        }
        else
        {
            tmp3.Set( tmp.Scl( u ) ); // remember tmp is already normalized
            tmp2.Set( tmp3.X + start.X, tmp3.Y + start.Y, 0 );
        }

        var x = center.X - tmp2.X;
        var y = center.Y - tmp2.Y;

        return ( ( x * x ) + ( y * y ) ) <= squareRadius;
    }

    /** Returns whether the given line segment intersects the given circle.
     * @param start The start point of the line segment
     * @param end The end point of the line segment
     * @param circle The circle
     * @param mtv A Minimum Translation Vector to fill in the case of a collision, or null (optional).
     * @return Whether the line segment and the circle intersect */
    public static bool intersectSegmentCircle( Vector2 start, Vector2 end, Circle circle, MinimumTranslationVector mtv )
    {
        _v2A.Set( end ).Sub( start );
        _v2B.Set( circle.X - start.X, circle.Y - start.Y );
        var Len = _v2A.Len();
        var u   = _v2B.Dot( _v2A.Nor() );

        if ( u <= 0 )
        {
            _v2C.Set( start );
        }
        else if ( u >= Len )
        {
            _v2C.Set( end );
        }
        else
        {
            _v2d.Set( _v2A.Scl( u ) ); // remember v2a is already normalized
            _v2C.Set( _v2d ).Add( start );
        }

        _v2A.Set( _v2C.X - circle.X, _v2C.Y - circle.Y );

        if ( mtv != null )
        {
            // Handle special case of segment containing circle center
            if ( _v2A.Equals( Vector2.Zero ) )
            {
                _v2d.Set( end.Y - start.Y, start.X - end.X );
                mtv.normal.Set( _v2d ).Nor();
                mtv.depth = circle.Radius;
            }
            else
            {
                mtv.normal.Set( _v2A ).Nor();
                mtv.depth = circle.Radius - _v2A.Len();
            }
        }

        return _v2A.len2() <= ( circle.Radius * circle.Radius );
    }

    /** Intersect two 2D Rays and return the scalar parameter of the first ray at the intersection point. You can get the
     * intersection point by: Vector2 point(direction1).Scl(scalar).Add(start1); For more information, check:
     * http://stackoverflow.com/a/565282/1091440
     * @param start1 Where the first ray start
     * @param direction1 The direction the first ray is pointing
     * @param start2 Where the second ray start
     * @param direction2 The direction the second ray is pointing
     * @return scalar parameter on the first ray describing the point where the intersection happens. May be negative. In case the
     *         rays are collinear, Float.POSITIVE_INFINITY will be returned. */
    public static float intersectRayRay( Vector2 start1, Vector2 direction1, Vector2 start2, Vector2 direction2 )
    {
        var difx  = start2.X - start1.X;
        var dify  = start2.Y - start1.Y;
        var d1xd2 = ( direction1.X * direction2.Y ) - ( direction1.Y * direction2.X );

        if ( d1xd2 == 0.0f )
        {
            return Float.POSITIVE_INFINITY; // collinear
        }

        var d2sx = direction2.X / d1xd2;
        var d2sy = direction2.Y / d1xd2;

        return ( difx * d2sy ) - ( dify * d2sx );
    }

    /** Intersects a {@link Ray} and a {@link Plane}. The intersection point is stored in intersection in case an intersection is
     * present.
     * @param intersection The vector the intersection point is written to (optional)
     * @return Whether an intersection is present. */
    public static bool intersectRayPlane( Ray ray, Plane plane, Vector3? intersection )
    {
        var denom = ray.direction.Dot( plane.getNormal() );

        if ( denom != 0 )
        {
            var t = -( ray.origin.Dot( plane.getNormal() ) + plane.getD() ) / denom;

            if ( t < 0 )
            {
                return false;
            }

            if ( intersection != null )
            {
                intersection.Set( ray.origin ).Add( v0.Set( ray.direction ).Scl( t ) );
            }

            return true;
        }
        else if ( plane.TestPoint( ray.origin ) == Plane.PlaneSide.OnPlane )
        {
            if ( intersection != null )
            {
                intersection.Set( ray.origin );
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    /** Intersects a line and a plane. The intersection is returned as the distance from the first point to the plane. In case an
     * intersection happened, the return value is in the range [0,1]. The intersection point can be recovered by point1 + t *
     * (point2 - point1) where t is the return value of this method. */
    public static float intersectLinePlane( float x,
                                            float y,
                                            float z,
                                            float x2,
                                            float y2,
                                            float z2,
                                            Plane plane,
                                            Vector3 intersection )
    {
        Vector3 direction = tmp.Set( x2, y2, z2 ).Sub( x, y, z );
        Vector3 origin    = tmp2.Set( x, y, z );
        var     denom     = direction.Dot( plane.getNormal() );

        if ( denom != 0 )
        {
            var t = -( origin.Dot( plane.getNormal() ) + plane.getD() ) / denom;

            if ( intersection != null )
            {
                intersection.Set( origin ).Add( direction.Scl( t ) );
            }

            return t;
        }
        else if ( plane.testPoint( origin ) == Plane.PlaneSide.OnPlane )
        {
            if ( intersection != null )
            {
                intersection.Set( origin );
            }

            return 0;
        }

        return -1;
    }

    /** Returns true if the three {@link Plane planes} intersect, setting the point of intersection in {@code intersection}, if any.
     * @param intersection The point where the three planes intersect */
    public static bool intersectPlanes( Plane a, Plane b, Plane c, Vector3 intersection )
    {
        tmp1.Set( a.normal ).crs( b.normal );
        tmp2.Set( b.normal ).crs( c.normal );
        tmp3.Set( c.normal ).crs( a.normal );

        float f = -a.normal.Dot( tmp2 );

        if ( Math.Abs( f ) < MathUtils.FLOAT_ROUNDING_ERROR )
        {
            return false;
        }

        tmp1.Scl( c.d );
        tmp2.Scl( a.d );
        tmp3.Scl( b.d );

        intersection.Set( tmp1.X + tmp2.X + tmp3.X, tmp1.Y + tmp2.Y + tmp3.Y, tmp1.z + tmp2.z + tmp3.z );
        intersection.Scl( 1 / f );

        return true;
    }

    private static Plane   _plane = new( new Vector3(), 0 );
    private static Vector3 _vec3  = new();

    /** Intersect a {@link Ray} and a triangle, returning the intersection point in intersection.
     * @param t1 The first vertex of the triangle
     * @param t2 The second vertex of the triangle
     * @param t3 The third vertex of the triangle
     * @param intersection The intersection point (optional)
     * @return True in case an intersection is present. */
    public static bool IntersectRayTriangle( Ray ray, Vector3 t1, Vector3 t2, Vector3 t3, Vector3 intersection )
    {
        Vector3 edge1 = v0.Set( t2 ).Sub( t1 );
        Vector3 edge2 = v1.Set( t3 ).Sub( t1 );

        Vector3 pvec = v2.Set( ray.direction ).crs( edge2 );
        var     det  = edge1.Dot( pvec );

        if ( MathUtils.IsZero( det ) )
        {
            p.Set( t1, t2, t3 );

            if ( ( p.testPoint( ray.origin ) == Maths.Plane.PlaneSide.OnPlane ) && Intersector.isPointInTriangle( ray.origin, t1, t2, t3 ) )
            {
                if ( intersection != null )
                {
                    intersection.Set( ray.origin );
                }

                return true;
            }

            return false;
        }

        det = 1.0f / det;

        Vector3 tvec = i.Set( ray.origin ).Sub( t1 );
        var     u    = tvec.Dot( pvec ) * det;

        if ( ( u < 0.0f ) || ( u > 1.0f ) )
        {
            return false;
        }

        Vector3 qvec = tvec.crs( edge1 );
        var     v    = ray.direction.Dot( qvec ) * det;

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
                ray.getEndPoint( intersection, t );
            }
        }

        return true;
    }

    private static Vector3 dir   = new Vector3();
    private static Vector3 start = new Vector3();

    /** Intersects a {@link Ray} and a sphere, returning the intersection point in intersection.
     * @param ray The ray, the direction component must be normalized before calling this method
     * @param center The center of the sphere
     * @param Radius The Radius of the sphere
     * @param intersection The intersection point (optional, can be null)
     * @return Whether an intersection is present. */
    public static bool intersectRaySphere( Ray ray, Vector3 center, float Radius, Vector3 intersection )
    {
        var Len = ray.direction.Dot( center.X - ray.origin.X, center.Y - ray.origin.Y, center.z - ray.origin.z );

        if ( Len < 0.f ) // behind the ray
        {
            return false;
        }

        var Dst2 = center.Dst2
            (
             ray.origin.X + ( ray.direction.X * Len ), ray.origin.Y + ( ray.direction.Y * Len ),
             ray.origin.z + ( ray.direction.z * Len )
            );

        var r2 = Radius * Radius;

        if ( Dst2 > r2 )
        {
            return false;
        }

        if ( intersection != null )
        {
            intersection.Set( ray.direction ).Scl( Len - ( float )Math.Sqrt( r2 - Dst2 ) ).Add( ray.origin );
        }

        return true;
    }

    /** Intersects a {@link Ray} and a {@link BoundingBox}, returning the intersection point in intersection. This intersection is
     * defined as the point on the ray closest to the origin which is within the specified bounds.
     *
     * <p>
     * The returned intersection (if any) is guaranteed to be within the bounds of the bounding box, but it can occasionally
     * diverge slightly from ray, due to small floating-point errors.
     * </p>
     *
     * <p>
     * If the origin of the ray is inside the box, this method returns true and the intersection point is set to the origin of the
     * ray, accordingly to the definition above.
     * </p>
     * @param intersection The intersection point (optional)
     * @return Whether an intersection is present. */
    public static bool intersectRayBounds( Ray ray, BoundingBox box, Vector3 intersection )
    {
        if ( box.contains( ray.origin ) )
        {
            if ( intersection != null )
            {
                intersection.Set( ray.origin );
            }

            return true;
        }

        float lowest = 0, t;
        var   hit    = false;

        // min x
        if ( ( ray.origin.X <= box.min.X ) && ( ray.direction.X > 0 ) )
        {
            t = ( box.min.X - ray.origin.X ) / ray.direction.X;

            if ( t >= 0 )
            {
                v2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( v2.Y >= box.min.Y ) && ( v2.Y <= box.max.Y ) && ( v2.z >= box.min.z ) && ( v2.z <= box.max.z ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // max x
        if ( ( ray.origin.X >= box.max.X ) && ( ray.direction.X < 0 ) )
        {
            t = ( box.max.X - ray.origin.X ) / ray.direction.X;

            if ( t >= 0 )
            {
                v2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( v2.Y >= box.min.Y ) && ( v2.Y <= box.max.Y ) && ( v2.z >= box.min.z ) && ( v2.z <= box.max.z ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // min y
        if ( ( ray.origin.Y <= box.min.Y ) && ( ray.direction.Y > 0 ) )
        {
            t = ( box.min.Y - ray.origin.Y ) / ray.direction.Y;

            if ( t >= 0 )
            {
                v2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( v2.X >= box.min.X ) && ( v2.X <= box.max.X ) && ( v2.z >= box.min.z ) && ( v2.z <= box.max.z ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // max y
        if ( ( ray.origin.Y >= box.max.Y ) && ( ray.direction.Y < 0 ) )
        {
            t = ( box.max.Y - ray.origin.Y ) / ray.direction.Y;

            if ( t >= 0 )
            {
                v2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( v2.X >= box.min.X ) && ( v2.X <= box.max.X ) && ( v2.z >= box.min.z ) && ( v2.z <= box.max.z ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // min z
        if ( ( ray.origin.z <= box.min.z ) && ( ray.direction.z > 0 ) )
        {
            t = ( box.min.z - ray.origin.z ) / ray.direction.z;

            if ( t >= 0 )
            {
                v2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( v2.X >= box.min.X ) && ( v2.X <= box.max.X ) && ( v2.Y >= box.min.Y ) && ( v2.Y <= box.max.Y ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        // max z
        if ( ( ray.origin.z >= box.max.z ) && ( ray.direction.z < 0 ) )
        {
            t = ( box.max.z - ray.origin.z ) / ray.direction.z;

            if ( t >= 0 )
            {
                v2.Set( ray.direction ).Scl( t ).Add( ray.origin );

                if ( ( v2.X >= box.min.X ) && ( v2.X <= box.max.X ) && ( v2.Y >= box.min.Y ) && ( v2.Y <= box.max.Y ) && ( !hit || ( t < lowest ) ) )
                {
                    hit    = true;
                    lowest = t;
                }
            }
        }

        if ( hit && ( intersection != null ) )
        {
            intersection.Set( ray.direction ).Scl( lowest ).Add( ray.origin );

            if ( intersection.X < box.min.X )
            {
                intersection.X = box.min.X;
            }
            else if ( intersection.X > box.max.X )
            {
                intersection.X = box.max.X;
            }

            if ( intersection.Y < box.min.Y )
            {
                intersection.Y = box.min.Y;
            }
            else if ( intersection.Y > box.max.Y )
            {
                intersection.Y = box.max.Y;
            }

            if ( intersection.z < box.min.z )
            {
                intersection.z = box.min.z;
            }
            else if ( intersection.z > box.max.z )
            {
                intersection.z = box.max.z;
            }
        }

        return hit;
    }

    /** Quick check whether the given {@link Ray} and {@link BoundingBox} intersect.
     * @return Whether the ray and the bounding box intersect. */
    public static bool intersectRayBoundsFast( Ray ray, BoundingBox box )
    {
        return intersectRayBoundsFast( ray, box.getCenter( tmp1 ), box.getDimensions( tmp2 ) );
    }

    /** Quick check whether the given {@link Ray} and {@link BoundingBox} intersect.
     * @param center The center of the bounding box
     * @param dimensions The dimensions (width, height and depth) of the bounding box
     * @return Whether the ray and the bounding box intersect. */
    public static bool intersectRayBoundsFast( Ray ray, Vector3 center, Vector3 dimensions )
    {
        var divX = 1f / ray.direction.X;
        var divY = 1f / ray.direction.Y;
        var divZ = 1f / ray.direction.z;

        var minx = ( ( center.X - ( dimensions.X * 0.5f ) ) - ray.origin.X ) * divX;
        var maxx = ( ( center.X + ( dimensions.X * 0.5f ) ) - ray.origin.X ) * divX;

        if ( minx > maxx )
        {
            var t = minx;
            minx = maxx;
            maxx = t;
        }

        var miny = ( ( center.Y - ( dimensions.Y * 0.5f ) ) - ray.origin.Y ) * divY;
        var maxy = ( ( center.Y + ( dimensions.Y * 0.5f ) ) - ray.origin.Y ) * divY;

        if ( miny > maxy )
        {
            var t = miny;
            miny = maxy;
            maxy = t;
        }

        var minz = ( ( center.z - ( dimensions.z * 0.5f ) ) - ray.origin.z ) * divZ;
        var maxz = ( ( center.z + ( dimensions.z * 0.5f ) ) - ray.origin.z ) * divZ;

        if ( minz > maxz )
        {
            var t = minz;
            minz = maxz;
            maxz = t;
        }

        float min = Math.max( Math.max( minx, miny ), minz );
        float max = Math.min( Math.min( maxx, maxy ), maxz );

        return ( max >= 0 ) && ( max >= min );
    }

    /** Quick check whether the given {@link Ray} and Oriented {@link BoundingBox} intersect.
     *
     * Based on code at: https://github.com/opengl-tutorials/ogl/blob/master/misc05_picking/misc05_picking_custom.cpp#L83
     * @param matrix The orientation of the bounding box
     * @return Whether the ray and the oriented bounding box intersect. */
    public static bool intersectRayOrientedBoundsFast( Ray ray, BoundingBox bounds, Matrix4 matrix )
    {
        var   tMin = 0.0f;
        float tMax = Float.MAX_VALUE;
        float t1, t2;

        Vector3 oBBposition = matrix.getTranslation( tmp );
        Vector3 delta       = oBBposition.Sub( ray.origin );

        // Test intersection with the 2 planes perpendicular to the OBB's X axis
        Vector3 xaxis = tmp1;
        tmp1.Set( matrix.val[ Matrix4.M00 ], matrix.val[ Matrix4.M10 ], matrix.val[ Matrix4.M20 ] );
        var e = xaxis.Dot( delta );
        var f = ray.direction.Dot( xaxis );

        if ( Math.Abs( f ) > MathUtils.FLOAT_ROUNDING_ERROR )
        {
            // Standard case
            t1 = ( e + bounds.min.X ) / f; // Intersection with the "left" plane
            t2 = ( e + bounds.max.X ) / f; // Intersection with the "right" plane

            // t1 and t2 now contain distances betwen ray origin and ray-plane intersections

            // We want t1 to represent the nearest intersection,
            // so if it's not the case, invert t1 and t2
            if ( t1 > t2 )
            {
                var w = t1;
                t1 = t2;
                t2 = w;
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
        else if ( ( ( -e + bounds.min.X ) > 0.0f ) || ( ( -e + bounds.max.X ) < 0.0f ) )
        {
            return false;
        }

        // Test intersection with the 2 planes perpendicular to the OBB's Y axis
        // Exactly the same thing than above.
        Vector3 yaxis = tmp2;
        tmp2.Set( matrix.val[ Matrix4.M01 ], matrix.val[ Matrix4.M11 ], matrix.val[ Matrix4.M21 ] );

        e = yaxis.Dot( delta );
        f = ray.direction.Dot( yaxis );

        if ( Math.Abs( f ) > MathUtils.FLOAT_ROUNDING_ERROR )
        {
            t1 = ( e + bounds.min.Y ) / f;
            t2 = ( e + bounds.max.Y ) / f;

            if ( t1 > t2 )
            {
                var w = t1;
                t1 = t2;
                t2 = w;
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
        else if ( ( ( -e + bounds.min.Y ) > 0.0f ) || ( ( -e + bounds.max.Y ) < 0.0f ) )
        {
            return false;
        }

        // Test intersection with the 2 planes perpendicular to the OBB's Z axis
        // Exactly the same thing than above.
        Vector3 zaxis = tmp3;
        tmp3.Set( matrix.val[ Matrix4.M02 ], matrix.val[ Matrix4.M12 ], matrix.val[ Matrix4.M22 ] );

        e = zaxis.Dot( delta );
        f = ray.direction.Dot( zaxis );

        if ( Math.Abs( f ) > MathUtils.FLOAT_ROUNDING_ERROR )
        {
            t1 = ( e + bounds.min.z ) / f;
            t2 = ( e + bounds.max.z ) / f;

            if ( t1 > t2 )
            {
                var w = t1;
                t1 = t2;
                t2 = w;
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
        else if ( ( ( -e + bounds.min.z ) > 0.0f ) || ( ( -e + bounds.max.z ) < 0.0f ) )
        {
            return false;
        }

        return true;
    }

    static Vector3 best = new Vector3();
    static Vector3 tmp  = new Vector3();
    static Vector3 tmp1 = new Vector3();
    static Vector3 tmp2 = new Vector3();
    static Vector3 tmp3 = new Vector3();

    /** Intersects the given ray with list of triangles. Returns the nearest intersection point in intersection
     * @param triangles The triangles, each successive 9 elements are the 3 vertices of a triangle, a vertex is made of 3
     *           successive floats (XYZ)
     * @param intersection The nearest intersection point (optional)
     * @return Whether the ray and the triangles intersect. */
    public static bool intersectRayTriangles( Ray ray, float[] triangles, Vector3 intersection )
    {
        float min_dist = Float.MAX_VALUE;
        var   hit      = false;

        if ( ( triangles.length % 9 ) != 0 )
        {
            throw new RuntimeException( "triangles array size is not a multiple of 9" );
        }

        for ( var i = 0; i < triangles.length; i += 9 )
        {
            var result = intersectRayTriangle
                (
                 ray, tmp1.Set( triangles[ i ], triangles[ i + 1 ], triangles[ i + 2 ] ),
                 tmp2.Set( triangles[ i + 3 ], triangles[ i + 4 ], triangles[ i + 5 ] ),
                 tmp3.Set( triangles[ i + 6 ], triangles[ i + 7 ], triangles[ i + 8 ] ), tmp
                );

            if ( result )
            {
                var dist = ray.origin.Dst2( tmp );

                if ( dist < min_dist )
                {
                    min_dist = dist;
                    best.Set( tmp );
                    hit = true;
                }
            }
        }

        if ( !hit )
        {
            return false;
        }
        else
        {
            if ( intersection != null )
            {
                intersection.Set( best );
            }

            return true;
        }
    }

    /** Intersects the given ray with list of triangles. Returns the nearest intersection point in intersection
     * @param indices the indices, each successive 3 shorts index the 3 vertices of a triangle
     * @param vertexSize the size of a vertex in floats
     * @param intersection The nearest intersection point (optional)
     * @return Whether the ray and the triangles intersect. */
    public static bool intersectRayTriangles( Ray ray,
                                              float[] vertices,
                                              short[] indices,
                                              int vertexSize,
                                              Vector3 intersection )
    {
        float min_dist = Float.MAX_VALUE;
        var   hit      = false;

        if ( ( indices.length % 3 ) != 0 )
        {
            throw new RuntimeException( "triangle list size is not a multiple of 3" );
        }

        for ( var i = 0; i < indices.length; i += 3 )
        {
            var i1 = indices[ i ] * vertexSize;
            var i2 = indices[ i + 1 ] * vertexSize;
            var i3 = indices[ i + 2 ] * vertexSize;

            var result = intersectRayTriangle
                (
                 ray, tmp1.Set( vertices[ i1 ], vertices[ i1 + 1 ], vertices[ i1 + 2 ] ),
                 tmp2.Set( vertices[ i2 ], vertices[ i2 + 1 ], vertices[ i2 + 2 ] ),
                 tmp3.Set( vertices[ i3 ], vertices[ i3 + 1 ], vertices[ i3 + 2 ] ), tmp
                );

            if ( result )
            {
                var dist = ray.origin.Dst2( tmp );

                if ( dist < min_dist )
                {
                    min_dist = dist;
                    best.Set( tmp );
                    hit = true;
                }
            }
        }

        if ( !hit )
        {
            return false;
        }
        else
        {
            if ( intersection != null )
            {
                intersection.Set( best );
            }

            return true;
        }
    }

    /** Intersects the given ray with list of triangles. Returns the nearest intersection point in intersection
     * @param triangles The triangles, each successive 3 elements are the 3 vertices of a triangle
     * @param intersection The nearest intersection point (optional)
     * @return Whether the ray and the triangles intersect. */
    public static bool intersectRayTriangles( Ray ray, List< Vector3 > triangles, Vector3 intersection )
    {
        float min_dist = Float.MAX_VALUE;
        var   hit      = false;

        if ( ( triangles.size() % 3 ) != 0 )
        {
            throw new RuntimeException( "triangle list size is not a multiple of 3" );
        }

        for ( var i = 0; i < triangles.size(); i += 3 )
        {
            var result = intersectRayTriangle( ray, triangles.get( i ), triangles.get( i + 1 ), triangles.get( i + 2 ), tmp );

            if ( result )
            {
                var dist = ray.origin.Dst2( tmp );

                if ( dist < min_dist )
                {
                    min_dist = dist;
                    best.Set( tmp );
                    hit = true;
                }
            }
        }

        if ( !hit )
        {
            return false;
        }
        else
        {
            if ( intersection != null )
            {
                intersection.Set( best );
            }

            return true;
        }
    }

    /** Quick check whether the given {@link BoundingBox} and {@link Plane} intersect.
     * @return Whether the bounding box and the plane intersect. */
    public static bool intersectBoundsPlaneFast( BoundingBox box, Plane plane )
    {
        return intersectBoundsPlaneFast( box.getCenter( tmp1 ), box.getDimensions( tmp2 ).Scl( 0.5f ), plane.normal, plane.d );
    }

    /** Quick check whether the given bounding box and a plane intersect. Code adapted from Christer Ericson's Real Time Collision
     * @param center The center of the bounding box
     * @param halfDimensions Half of the dimensions (width, height and depth) of the bounding box
     * @param normal The normal of the plane
     * @param distance The distance of the plane
     * @return Whether the bounding box and the plane intersect. */
    public static bool intersectBoundsPlaneFast( Vector3 center, Vector3 halfDimensions, Vector3 normal, float distance )
    {
        // Compute the projection interval Radius of b onto L(t) = b.c + t * p.n
        var Radius = ( halfDimensions.X * Math.Abs( normal.X ) )
                   + ( halfDimensions.Y * Math.Abs( normal.Y ) )
                   + ( halfDimensions.z * Math.Abs( normal.z ) );

        // Compute distance of box center from plane
        var s = normal.Dot( center ) - distance;

        // Intersection occurs when plane distance falls within [-r,+r] interval
        return Math.Abs( s ) <= Radius;
    }

    /** Intersects the two lines and returns the intersection point in intersection.
     * @param p1 The first point of the first line
     * @param p2 The second point of the first line
     * @param p3 The first point of the second line
     * @param p4 The second point of the second line
     * @param intersection The intersection point. May be null.
     * @return Whether the two lines intersect */
    public static bool IntersectLines( Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 intersection )
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

    /** Intersects the two lines and returns the intersection point in intersection.
     * @param intersection The intersection point, or null.
     * @return Whether the two lines intersect */
    public static bool IntersectLines( float x1,
                                       float y1,
                                       float x2,
                                       float y2,
                                       float x3,
                                       float y3,
                                       float x4,
                                       float y4,
                                       Vector2 intersection )
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

    /** Check whether the given line and {@link Polygon} intersect.
     * @param p1 The first point of the line
     * @param p2 The second point of the line
     * @return Whether polygon and line intersects */
    public static bool IntersectLinePolygon( Vector2 p1, Vector2 p2, Polygon polygon )
    {
        float[] vertices = polygon.getTransformedVertices();
        float   x1       = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y;
        int     n        = vertices.length;
        float   x3       = vertices[ n - 2 ], y3 = vertices[ n - 1 ];

        for ( var i = 0; i < n; i += 2 )
        {
            float x4 = vertices[ i ], y4 = vertices[ i + 1 ];
            var   d  = ( ( y4 - y3 ) * ( x2 - x1 ) ) - ( ( x4 - x3 ) * ( y2 - y1 ) );

            if ( d != 0 )
            {
                var yd = y1 - y3;
                var xd = x1 - x3;
                var ua = ( ( ( x4 - x3 ) * yd ) - ( ( y4 - y3 ) * xd ) ) / d;

                if ( ( ua >= 0 ) && ( ua <= 1 ) )
                {
                    return true;
                }
            }

            x3 = x4;
            y3 = y4;
        }

        return false;
    }

    /** Determines whether the given rectangles intersect and, if they do, sets the supplied {@code intersection} rectangle to the
     * area of overlap.
     * @return Whether the rectangles intersect */
    public static bool IntersectRectangles( RectangleShape rectangle1, RectangleShape rectangle2, RectangleShape intersection )
    {
        if ( rectangle1.overlaps( rectangle2 ) )
        {
            intersection.X      = Math.max( rectangle1.X, rectangle2.X );
            intersection.width  = Math.min( rectangle1.X + rectangle1.width, rectangle2.X + rectangle2.width ) - intersection.X;
            intersection.Y      = Math.max( rectangle1.Y, rectangle2.Y );
            intersection.height = Math.min( rectangle1.Y + rectangle1.height, rectangle2.Y + rectangle2.height ) - intersection.Y;

            return true;
        }

        return false;
    }

    /** Determines whether the given rectangle and segment intersect
     * @param startX x-coordinate start of line segment
     * @param startY y-coordinate start of line segment
     * @param endX y-coordinate end of line segment
     * @param endY y-coordinate end of line segment
     * @param rectangle rectangle that is being tested for collision
     * @return whether the rectangle intersects with the line segment */
    public static bool intersectSegmentRectangle( float startX, float startY, float endX, float endY, RectangleShape rectangle )
    {
        var rectangleEndX = rectangle.X + rectangle.width;
        var rectangleEndY = rectangle.Y + rectangle.height;

        if ( intersectSegments( startX, startY, endX, endY, rectangle.X, rectangle.Y, rectangle.X, rectangleEndY, null ) )
        {
            return true;
        }

        if ( intersectSegments( startX, startY, endX, endY, rectangle.X, rectangle.Y, rectangleEndX, rectangle.Y, null ) )
        {
            return true;
        }

        if ( intersectSegments( startX, startY, endX, endY, rectangleEndX, rectangle.Y, rectangleEndX, rectangleEndY, null ) )
        {
            return true;
        }

        if ( intersectSegments( startX, startY, endX, endY, rectangle.X, rectangleEndY, rectangleEndX, rectangleEndY, null ) )
        {
            return true;
        }

        return rectangle.contains( startX, startY );
    }

    /** {@link #intersectSegmentRectangle(float, float, float, float, RectangleShape)} */
    public static bool IntersectSegmentRectangle( Vector2 start, Vector2 end, RectangleShape rectangle )
    {
        return intersectSegmentRectangle( start.X, start.Y, end.X, end.Y, rectangle );
    }

    /// <summary>
    /// Check whether the given line segment and <see cref="Polygon"/> intersect.
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
    /// Intersects the two line segments and returns the intersection point in intersection.
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
                                          Vector2 intersection )
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

        if ( ( ua < 0 ) || ( ua > 1 ) )
        {
            return false;
        }

        var ub = ( ( ( x2 - x1 ) * yd ) - ( ( y2 - y1 ) * xd ) ) / d;

        if ( ( ub < 0 ) || ( ub > 1 ) )
        {
            return false;
        }

        if ( intersection != null )
        {
            intersection.Set( x1 + ( ( x2 - x1 ) * ua ), y1 + ( ( y2 - y1 ) * ua ) );
        }

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

        if ( ( ua < 0 ) || ( ua > 1 ) )
        {
            return false;
        }

        var ub = ( ( ( x2 - x1 ) * yd ) - ( ( y2 - y1 ) * xd ) ) / d;

        if ( ( ub < 0 ) || ( ub > 1 ) )
        {
            return false;
        }

        if ( intersection != null )
        {
            intersection.Set( x1 + ( ( x2 - x1 ) * ua ), y1 + ( ( y2 - y1 ) * ua ) );
        }

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
            closestX = r.X;
        }
        else if ( c.X > ( r.X + r.Width ) )
        {
            closestX = r.X + r.Width;
        }

        if ( c.Y < r.Y )
        {
            closestY = r.Y;
        }
        else if ( c.Y > ( r.Y + r.Height ) )
        {
            closestY = r.Y + r.Height;
        }

        closestX =  closestX - c.X;
        closestX *= closestX;
        closestY =  closestY - c.Y;
        closestY *= closestY;

        return ( closestX + closestY ) < ( c.Radius * c.Radius );
    }

    /// <summary>
    /// Check whether specified convex polygons overlap (clockwise or
    /// counter-clockwise round doesn't matter).
    /// </summary>
    /// <param name="p1"> The first polygon. </param>
    /// <param name="p2"> The second polygon. </param>
    /// <returns> Whether polygons overlap. </returns>
    public static bool OverlapConvexPolygons( Polygon p1, Polygon p2 )
    {
        return OverlapConvexPolygons( p1, p2, ref _dummyMinimumTranslationVector );
    }

    //TODO:
    // This is a temporary fix for a problem caused by the above method.
    // As 'ref null' is not allowed i thought the best option would be to
    // create a dummy MinimumTranslationVector object and test for that as
    // well as null.
    // As soon as i determine whether or not 'ref' is actually needed, or if
    // there is a better solution then I will update.
    private static MinimumTranslationVector? _dummyMinimumTranslationVector = new();

    /// <summary>
    /// Check whether convex polygons overlap (clockwise or counter-clockwise
    /// wound doesn't matter). If they do, optionally obtain a Minimum Translation
    /// Vector indicating the minimum magnitude vector required to push the polygon
    /// p1 out of collision with polygon p2.
    /// </summary>
    /// <param name="p1"> The first polygon. </param>
    /// <param name="p2"> The second polygon. </param>
    /// <param name="mtv">
    /// A Minimum Translation Vector to fill in the case of a collision,
    /// or null (optional).
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
    /// Check whether polygons defined by the given vertex arrays overlap
    /// (clockwise or counter-clockwise wound doesn't matter). If they do,
    /// optionally obtain a Minimum Translation Vector indicating the minimum
    /// magnitude vector required to push the polygon defined by <paramref name="verts1"/>
    /// out of the collision with the polygon defined by <paramref name="verts2"/>.
    /// </summary>
    /// <param name="verts1">Vertices of the first polygon.</param>
    /// <param name="offset1">The offset of the <paramref name="verts1"/> array.</param>
    /// <param name="count1">The amount that is added to the <paramref name="offset1"/>.</param>
    /// <param name="verts2">Vertices of the second polygon.</param>
    /// <param name="offset2">The offset of the <paramref name="verts2"/> array.</param>
    /// <param name="count2">The amount that is added to the <paramref name="offset2"/>.</param>
    /// <param name="mtv">
    /// A Minimum Translation Vector to fill in the case of a collision,
    /// or <see langword="null"/> (optional).
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
            mtv.depth = float.MaxValue;
            mtv.normal.SetZero();
        }

        var overlaps = OverlapsOnAxisOfShape
            (
             verts2,
             offset2,
             count2,
             verts1,
             offset1,
             count1,
             ref mtv!,
             true
            );

        if ( overlaps )
        {
            overlaps = OverlapsOnAxisOfShape
                (
                 verts1,
                 offset1,
                 count1,
                 verts2,
                 offset2,
                 count2,
                 ref mtv,
                 false
                );
        }

        if ( !overlaps )
        {
            mtv.depth = 0;
            mtv.normal.SetZero();

            return false;
        }

        return true;
    }

    /// <summary>
    /// Implementation of the Separating Axis Theorem (SAT) algorithm.
    /// </summary>
    /// <param name="verts1"></param>
    /// <param name="offset1">Offset of vertices in the first shape.</param>
    /// <param name="count1">Count of vertices in the first shape.</param>
    /// <param name="verts2"></param>
    /// <param name="offset2">Offset of vertices in the second shape.</param>
    /// <param name="count2">Count of vertices in the second shape.</param>
    /// <param name="mtv">The minimum translation vector.</param>
    /// <param name="shapesShifted">
    /// Indicates if shapes A and B are shifted, which is important for
    /// calculating the axis translation for vertices in the first shape.
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

            var len = ( float )Math.Sqrt( ( axisX * axisX ) + ( axisY * axisY ) );

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
            else
            {
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

                    if ( mtv.depth > o )
                    {
                        mtv.depth = o;
                        bool condition;

                        if ( shapesShifted )
                        {
                            condition = minA < minB;
                            axisX     = condition ? axisX : -axisX;
                            axisY     = condition ? axisY : -axisY;
                        }
                        else
                        {
                            condition = minA > minB;
                            axisX     = condition ? axisX : -axisX;
                            axisY     = condition ? axisY : -axisY;
                        }

                        if ( aContainsB || bContainsA )
                        {
                            condition = mins > maxs;
                            axisX     = condition ? axisX : -axisX;
                            axisY     = condition ? axisY : -axisY;
                        }

                        mtv.normal.Set( axisX, axisY );
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Splits the triangle by the plane. The result is stored in the
    /// <paramref name="split"/> instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Depending on where the triangle is relative to the plane, the result can be:
    /// </para>
    /// <para>
    /// i) Triangle is fully in front/behind: <see cref="SplitTriangle.Front"/> or
    /// <see cref="SplitTriangle.Back"/> will contain the original triangle, and
    /// <see cref="SplitTriangle.total"/> will be one.
    /// </para>
    /// <para>
    /// ii) Triangle has two vertices in front, one behind: <see cref="SplitTriangle.Front"/>
    /// contains 2 triangles, <see cref="SplitTriangle.Back"/> contains 1 triangle, and
    /// <see cref="SplitTriangle.total"/> will be 3.
    /// </para>
    /// <para>
    /// iii) Triangle has one vertex in front, two behind: <see cref="SplitTriangle.Front"/>
    /// contains 1 triangle, <see cref="SplitTriangle.Back"/> contains 2 triangles, and
    /// <see cref="SplitTriangle.total"/> will be 3.
    /// </para>
    /// <para>
    /// The input triangle should have the form: x, y, z, x2, y2, z2, x3, y3, z3. You can
    /// add additional attributes per vertex that will be interpolated if split, such as
    /// texture coordinates or normals. Note that these additional attributes won't be
    /// normalized, as might be necessary in case of normals.
    /// </para>
    /// </remarks>
    /// <param name="triangle"></param>
    /// <param name="plane"></param>
    /// <param name="split">Output <see cref="SplitTriangle"/> to store the result of the split.</param>
    public static void DoSplitTriangle( float[] triangle, Plane plane, SplitTriangle split )
    {
        var stride = triangle.Length / 3;

        var r1 = plane.TestPoint
                     (
                      triangle[ 0 ],
                      triangle[ 1 ],
                      triangle[ 2 ]
                     )
              == Maths.Plane.PlaneSide.Back;

        var r2 = plane.TestPoint
                     (
                      triangle[ 0 + stride ],
                      triangle[ 1 + stride ],
                      triangle[ 2 + stride ]
                     )
              == Maths.Plane.PlaneSide.Back;

        var r3 = plane.TestPoint
                     (
                      triangle[ 0 + ( stride * 2 ) ],
                      triangle[ 1 + ( stride * 2 ) ],
                      triangle[ 2 + ( stride * 2 ) ]
                     )
              == Maths.Plane.PlaneSide.Back;

        split.Reset();

        // easy case, triangle is on one side (point on plane means front).
        if ( ( r1 == r2 ) && ( r2 == r3 ) )
        {
            split.total = 1;

            if ( r1 )
            {
                split.numBack = 1;
                Array.Copy( triangle, 0, split.Back, 0, triangle.Length );
            }
            else
            {
                split.numFront = 1;
                Array.Copy( triangle, 0, split.Front, 0, triangle.Length );
            }

            return;
        }

        // set number of triangles
        split.total    = 3;
        split.numFront = ( r1 ? 0 : 1 ) + ( r2 ? 0 : 1 ) + ( r3 ? 0 : 1 );
        split.numBack  = split.total - split.numFront;

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
        if ( split.numFront == 2 )
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

    private static Vector3 _intersection = new();

    private static void SplitEdge( float[] vertices,
                                   int s,
                                   int e,
                                   int stride,
                                   Plane plane,
                                   float[] split,
                                   int offset )
    {
        var t = intersectLinePlane
            (
             vertices[ s ], vertices[ s + 1 ],
             vertices[ s + 2 ], vertices[ e ],
             vertices[ e + 1 ], vertices[ e + 2 ],
             plane,
             _intersection
            );

        split[ offset + 0 ] = _intersection.X;
        split[ offset + 1 ] = _intersection.Y;
        split[ offset + 2 ] = _intersection.Z;

        for ( var i = 3; i < stride; i++ )
        {
            var a = vertices[ s + i ];
            var b = vertices[ e + i ];
            split[ offset + i ] = a + ( t * ( b - a ) );
        }
    }

    [PublicAPI]
    public class SplitTriangle
    {
        public float[] Front        { get; set; }
        public float[] Back         { get; set; }
        public bool    FrontCurrent { get; set; } = false;
        public float[] EdgeSplit    { get; set; }
        public int     FrontOffset  { get; set; } = 0;
        public int     BackOffset   { get; set; } = 0;

        public int numFront;
        public int numBack;
        public int total;

        /// <summary>
        /// Creates a new instance, assuming numAttributes attributes
        /// per triangle vertex.
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

        public override string ToString()
        {
            return "SplitTriangle [front="
                 + Front
                 + ", back="
                 + Back
                 + ", numFront="
                 + numFront
                 + ", numBack="
                 + numBack
                 + ", total="
                 + total
                 + "]";
        }

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
            numFront     = 0;
            numBack      = 0;
            total        = 0;
        }
    }

    /// <summary>
    /// Minimum translation required to separate two polygons.
    /// </summary>
    [PublicAPI]
    public record MinimumTranslationVector
    {
        // Unit length vector that indicates the direction for the separation
        public Vector2 normal = new();

        // Distance of the translation required for the separation
        public float depth = 0;
    }
}
