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

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class GeometryUtils
{
    private readonly static Vector2 tmp1 = new();
    private readonly static Vector2 tmp2 = new();
    private readonly static Vector2 tmp3 = new();

    /// <summary>
    /// Computes the barycentric coordinates v,w for the specified point in the triangle.
    /// <example>
    /// The point is inside the triangle if the following is true,
    /// <code>
    ///     barycentric.x >= 0 &amp;&amp; barycentric.y >= 0 &amp;&amp; barycentric.x + barycentric.y &lt;= 1;
    /// </code>
    /// </example>
    /// <example>
    /// If vertices a,b,c have values aa,bb,cc then to get an interpolated value at point p -
    /// <code>
    ///     GeometryUtils.barycentric(p, a, b, c, barycentric);
    ///     float u = 1.f - barycentric.x - barycentric.y;
    ///     float x = u * aa.x + barycentric.x * bb.x + barycentric.y * cc.x;
    ///     float y = u * aa.y + barycentric.x * bb.y + barycentric.y * cc.y;
    /// </code>
    /// </example>
    /// </summary>
    /// <returns> barycentricOut </returns>
    public static Vector2 ToBarycoord( Vector2 p, Vector2 a, Vector2 b, Vector2 c, Vector2 barycentricOut )
    {
        Vector2 v0 = tmp1.Set( b ).Sub( a );
        Vector2 v1 = tmp2.Set( c ).Sub( a );
        Vector2 v2 = tmp3.Set( p ).Sub( a );

        var d00   = v0.Dot( v0 );
        var d01   = v0.Dot( v1 );
        var d11   = v1.Dot( v1 );
        var d20   = v2.Dot( v0 );
        var d21   = v2.Dot( v1 );
        var denom = ( d00 * d11 ) - ( d01 * d01 );

        barycentricOut.X = ( ( d11 * d20 ) - ( d01 * d21 ) ) / denom;
        barycentricOut.Y = ( ( d00 * d21 ) - ( d01 * d20 ) ) / denom;

        return barycentricOut;
    }

    /// <summary>
    /// Returns true if the barycentric coordinates are inside the triangle.
    /// </summary>
    public static bool BarycoordInsideTriangle( Vector2 barycentric )
    {
        return barycentric is { X: >= 0, Y: >= 0 } && ( ( barycentric.X + barycentric.Y ) <= 1 );
    }

    /// <summary>
    /// Returns interpolated values given the barycentric coordinates of a point in a triangle and the values at each vertex. </summary>
    /// <returns> interpolatedOut  </returns>
    public static Vector2 FromBarycoord( Vector2 barycentric, Vector2 a, Vector2 b, Vector2 c, Vector2 interpolatedOut )
    {
        var u = 1 - barycentric.X - barycentric.Y;
        interpolatedOut.X = ( u * a.X ) + ( barycentric.X * b.X ) + ( barycentric.Y * c.X );
        interpolatedOut.Y = ( u * a.Y ) + ( barycentric.X * b.Y ) + ( barycentric.Y * c.Y );

        return interpolatedOut;
    }

    /// <summary>
    /// Returns an interpolated value given the barycentric coordinates of a point in a triangle and the values at each vertex. </summary>
    /// <returns> interpolatedOut  </returns>
    public static float FromBarycoord( Vector2 barycentric, float a, float b, float c )
    {
        var u = 1 - barycentric.X - barycentric.Y;

        return ( u * a ) + ( barycentric.X * b ) + ( barycentric.Y * c );
    }

    /// <summary>
    /// Returns the lowest positive root of the quadric equation given by a* x * x + b * x + c = 0. If no solution is given
    /// Float.Nan is returned. </summary>
    /// <param name="a"> the first coefficient of the quadric equation </param>
    /// <param name="b"> the second coefficient of the quadric equation </param>
    /// <param name="c"> the third coefficient of the quadric equation </param>
    /// <returns> the lowest positive root or Float.Nan  </returns>
    public static float LowestPositiveRoot( float a, float b, float c )
    {
        var det = ( b * b ) - ( 4 * a * c );

        if ( det < 0 ) return float.NaN;

        var sqrtD = ( float )Math.Sqrt( det );
        var invA  = 1 / ( 2 * a );
        var r1    = ( -b - sqrtD ) * invA;
        var r2    = ( -b + sqrtD ) * invA;

        if ( r1 > r2 )
        {
            ( r2, r1 ) = ( r1, r2 );
        }

        if ( r1 > 0 ) return r1;

        return r2 > 0 ? r2 : float.NaN;

    }

    public static bool Colinear( float x1, float y1, float x2, float y2, float x3, float y3 )
    {
        float dx21 = x2 - x1, dy21 = y2 - y1;
        float dx32 = x3 - x2, dy32 = y3 - y2;
        var   det  = ( dx32 * dy21 ) - ( dx21 * dy32 );

        return Math.Abs( det ) < MathUtils.Float_Rounding_Error;
    }

    public static Vector2 TriangleCentroid( float x1,
                                            float y1,
                                            float x2,
                                            float y2,
                                            float x3,
                                            float y3,
                                            Vector2 centroid )
    {
        centroid.X = ( x1 + x2 + x3 ) / 3;
        centroid.Y = ( y1 + y2 + y3 ) / 3;

        return centroid;
    }

    /// <summary>
    /// Returns the circumcenter of the triangle. The input points must not be colinear.
    /// </summary>
    public static Vector2 TriangleCircumcenter( float x1,
                                                float y1,
                                                float x2,
                                                float y2,
                                                float x3,
                                                float y3,
                                                Vector2 circumcenter )
    {
        float dx21 = x2 - x1, dy21 = y2 - y1;
        float dx32 = x3 - x2, dy32 = y3 - y2;
        float dx13 = x1 - x3, dy13 = y1 - y3;
        var   det  = ( dx32 * dy21 ) - ( dx21 * dy32 );

        if ( Math.Abs( det ) < MathUtils.Float_Rounding_Error )
        {
            throw new ArgumentException( "Triangle points must not be colinear." );
        }

        det *= 2;
        float sqr1 = ( x1 * x1 ) + ( y1 * y1 ), sqr2 = ( x2 * x2 ) + ( y2 * y2 ), sqr3 = ( x3 * x3 ) + ( y3 * y3 );

        circumcenter.Set
            (
             ( ( sqr1 * dy32 ) + ( sqr2 * dy13 ) + ( sqr3 * dy21 ) ) / det,
             -( ( sqr1 * dx32 ) + ( sqr2 * dx13 ) + ( sqr3 * dx21 ) ) / det
            );

        return circumcenter;
    }

    public static float TriangleCircumradius( float x1, float y1, float x2, float y2, float x3, float y3 )
    {
        float m1, m2, mx1, mx2, my1, my2, x, y;

        if ( Math.Abs( y2 - y1 ) < MathUtils.Float_Rounding_Error )
        {
            m2  = -( x3 - x2 ) / ( y3 - y2 );
            mx2 = ( x2 + x3 ) / 2;
            my2 = ( y2 + y3 ) / 2;
            x   = ( x2 + x1 ) / 2;
            y   = ( m2 * ( x - mx2 ) ) + my2;
        }
        else if ( Math.Abs( y3 - y2 ) < MathUtils.Float_Rounding_Error )
        {
            m1  = -( x2 - x1 ) / ( y2 - y1 );
            mx1 = ( x1 + x2 ) / 2;
            my1 = ( y1 + y2 ) / 2;
            x   = ( x3 + x2 ) / 2;
            y   = ( m1 * ( x - mx1 ) ) + my1;
        }
        else
        {
            m1  = -( x2 - x1 ) / ( y2 - y1 );
            m2  = -( x3 - x2 ) / ( y3 - y2 );
            mx1 = ( x1 + x2 ) / 2;
            mx2 = ( x2 + x3 ) / 2;
            my1 = ( y1 + y2 ) / 2;
            my2 = ( y2 + y3 ) / 2;
            x   = ( ( ( ( m1 * mx1 ) - ( m2 * mx2 ) ) + my2 ) - my1 ) / ( m1 - m2 );
            y   = ( m1 * ( x - mx1 ) ) + my1;
        }

        float dx = x1 - x, dy = y1 - y;

        return ( float )Math.Sqrt( ( dx * dx ) + ( dy * dy ) );
    }

    /// <summary>
    /// Ratio of circumradius to shortest edge as a measure of triangle quality.
    /// </summary>
    public static float TriangleQuality( float x1, float y1, float x2, float y2, float x3, float y3 )
    {
        var length1 = ( float )Math.Sqrt( ( x1 * x1 ) + ( y1 * y1 ) );
        var length2 = ( float )Math.Sqrt( ( x2 * x2 ) + ( y2 * y2 ) );
        var length3 = ( float )Math.Sqrt( ( x3 * x3 ) + ( y3 * y3 ) );

        return Math.Min( length1, Math.Min( length2, length3 ) ) / TriangleCircumradius( x1, y1, x2, y2, x3, y3 );
    }

    public static float TriangleArea( float x1, float y1, float x2, float y2, float x3, float y3 )
    {
        return Math.Abs( ( ( x1 - x3 ) * ( y2 - y1 ) ) - ( ( x1 - x2 ) * ( y3 - y1 ) ) ) * 0.5f;
    }

    public static Vector2 QuadrilateralCentroid( float x1,
                                                 float y1,
                                                 float x2,
                                                 float y2,
                                                 float x3,
                                                 float y3,
                                                 float x4,
                                                 float y4,
                                                 Vector2 centroid )
    {
        var avgX1 = ( x1 + x2 + x3 ) / 3;
        var avgY1 = ( y1 + y2 + y3 ) / 3;
        var avgX2 = ( x1 + x4 + x3 ) / 3;
        var avgY2 = ( y1 + y4 + y3 ) / 3;

        centroid.X = avgX1 - ( ( avgX1 - avgX2 ) / 2 );
        centroid.Y = avgY1 - ( ( avgY1 - avgY2 ) / 2 );

        return centroid;
    }

    /// <summary>
    /// Returns the centroid for the specified non-self-intersecting polygon.
    /// </summary>
    public static Vector2 PolygonCentroid( float[] polygon, int offset, int count, Vector2 centroid )
    {
        if ( count < 6 )
        {
            throw new ArgumentException( "A polygon must have 3 or more coordinate pairs." );
        }

        float area = 0, x = 0, y = 0;
        var   last = ( offset + count ) - 2;
        float x1   = polygon[ last ], y1 = polygon[ last + 1 ];

        for ( var i = offset; i <= last; i += 2 )
        {
            float x2 = polygon[ i ], y2 = polygon[ i + 1 ];
            var   a  = ( x1 * y2 ) - ( x2 * y1 );

            area += a;

            x  += ( x1 + x2 ) * a;
            y  += ( y1 + y2 ) * a;
            x1 =  x2;
            y1 =  y2;
        }

        if ( area == 0 )
        {
            centroid.X = 0;
            centroid.Y = 0;
        }
        else
        {
            area       *= 0.5f;
            centroid.X =  x / ( 6 * area );
            centroid.Y =  y / ( 6 * area );
        }

        return centroid;
    }

    /// <summary>
    /// Computes the area for a convex polygon.
    /// </summary>
    public static float PolygonArea( float[] polygon, int offset, int count )
    {
        float area = 0;
        var   last = ( offset + count ) - 2;
        float x1   = polygon[ last ], y1 = polygon[ last + 1 ];

        for ( var i = offset; i <= last; i += 2 )
        {
            float x2 = polygon[ i ], y2 = polygon[ i + 1 ];
            area += ( x1 * y2 ) - ( x2 * y1 );
            x1   =  x2;
            y1   =  y2;
        }

        return area * 0.5f;
    }

    public static void EnsureCcw( float[] polygon )
    {
        EnsureCcw( polygon, 0, polygon.Length );
    }

    public static void EnsureCcw( float[] polygon, int offset, int count )
    {
        if ( !IsClockwise( polygon, offset, count ) ) return;

        var lastX = ( offset + count ) - 2;

        for ( int i = offset, n = offset + ( count / 2 ); i < n; i += 2 )
        {
            var other = lastX - i;
            var x     = polygon[ i ];
            var y     = polygon[ i + 1 ];

            polygon[ i ]         = polygon[ other ];
            polygon[ i + 1 ]     = polygon[ other + 1 ];
            polygon[ other ]     = x;
            polygon[ other + 1 ] = y;
        }
    }

    public static bool IsClockwise( float[] polygon, int offset, int count )
    {
        if ( count <= 2 ) return false;

        float area = 0;

        var   last = ( offset + count ) - 2;
        float x1   = polygon[ last ], y1 = polygon[ last + 1 ];

        for ( var i = offset; i <= last; i += 2 )
        {
            float x2 = polygon[ i ], y2 = polygon[ i + 1 ];
            area += ( x1 * y2 ) - ( x2 * y1 );
            x1   =  x2;
            y1   =  y2;
        }

        return area < 0;
    }
}