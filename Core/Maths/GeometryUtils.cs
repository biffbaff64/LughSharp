namespace LibGDXSharp.Maths;

public sealed class GeometryUtils
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
	public static Vector2 ToBarycoord(Vector2 p, Vector2 a, Vector2 b, Vector2 c, Vector2 barycentricOut)
	{
		Vector2 v0 = tmp1.Set(b).Sub(a);
		Vector2 v1 = tmp2.Set(c).Sub(a);
		Vector2 v2 = tmp3.Set(p).Sub(a);
        
		var d00 = v0.Dot(v0);
		var d01 = v0.Dot(v1);
		var d11 = v1.Dot(v1);
		var d20 = v2.Dot(v0);
		var d21 = v2.Dot(v1);
		var denom = ( d00 * d11 ) - ( d01 * d01 );
		
        barycentricOut.X = (( d11 * d20 ) - ( d01 * d21 )) / denom;
		barycentricOut.Y = (( d00 * d21 ) - ( d01 * d20 )) / denom;
		
        return barycentricOut;
	}

	/// <summary>
	/// Returns true if the barycentric coordinates are inside the triangle.
	/// </summary>
	public static bool BarycoordInsideTriangle(Vector2 barycentric)
	{
		return barycentric is { X: >= 0, Y: >= 0 } && ( ( barycentric.X + barycentric.Y ) <= 1 );
	}

	/// <summary>
	/// Returns interpolated values given the barycentric coordinates of a point in a triangle and the values at each vertex. </summary>
	/// <returns> interpolatedOut  </returns>
	public static Vector2 FromBarycoord(Vector2 barycentric, Vector2 a, Vector2 b, Vector2 c, Vector2 interpolatedOut)
	{
		var u = 1 - barycentric.X - barycentric.Y;
		interpolatedOut.X = ( u * a.X ) + ( barycentric.X * b.X ) + ( barycentric.Y * c.X );
		interpolatedOut.Y = ( u * a.Y ) + ( barycentric.X * b.Y ) + ( barycentric.Y * c.Y );
		return interpolatedOut;
	}

	/// <summary>
	/// Returns an interpolated value given the barycentric coordinates of a point in a triangle and the values at each vertex. </summary>
	/// <returns> interpolatedOut  </returns>
	public static float FromBarycoord(Vector2 barycentric, float a, float b, float c)
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
	public static float LowestPositiveRoot(float a, float b, float c)
	{
		var det = ( b * b ) - ( 4 * a * c );
		
        if (det < 0) return float.NaN;

		var sqrtD = (float)Math.Sqrt(det);
		var invA = 1 / (2 * a);
		var r1 = (-b - sqrtD) * invA;
		var r2 = (-b + sqrtD) * invA;

		if (r1 > r2)
		{
			( r2, r1 ) = ( r1, r2 );
        }

		if (r1 > 0) return r1;
		
        return r2 > 0 ? r2 : float.NaN;

    }

	public static bool Colinear(float x1, float y1, float x2, float y2, float x3, float y3)
	{
		float dx21 = x2 - x1, dy21 = y2 - y1;
		float dx32 = x3 - x2, dy32 = y3 - y2;
		var det = ( dx32 * dy21 ) - ( dx21 * dy32 );

        return Math.Abs(det) < MathUtils.Float_Rounding_Error;
	}

	public static Vector2 TriangleCentroid(float x1, float y1, float x2, float y2, float x3, float y3, Vector2 centroid)
	{
		centroid.X = (x1 + x2 + x3) / 3;
		centroid.Y = (y1 + y2 + y3) / 3;
		return centroid;
	}
}