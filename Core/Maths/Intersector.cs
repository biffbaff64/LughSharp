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

/// <summary>
/// Class offering various static methods for intersection testing between different geometric objects.
/// </summary>
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
    public static bool IsPointInTriangle( float px, float py, float ax, float ay,
                                          float bx, float by, float cx, float cy )
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

    public static int PointLineSide( float linePoint1X, float linePoint1Y, float linePoint2X, float linePoint2Y, float pointX, float pointY )
    {
        return Math.Sign
            (
            ( ( linePoint2X - linePoint1X ) * ( pointY - linePoint1Y ) )
            - ( ( linePoint2Y - linePoint1Y ) * ( pointX - linePoint1X ) )
            );
    }
}
