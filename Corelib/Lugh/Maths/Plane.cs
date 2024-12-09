// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace Corelib.Lugh.Maths;

[PublicAPI]
public class Plane
{
    /// <summary>
    /// Enum specifying on which side a point lies respective to the plane and
    /// it's normal. <see cref="PlaneSide.Front"/> is the side to which the
    /// normal points.
    /// </summary>
    public enum PlaneSide
    {
        OnPlane,
        Back,
        Front,
    }

    /// <summary>
    /// Constructs a new plane with all values set to 0
    /// </summary>
    public Plane()
    {
    }

    /// <summary>
    /// Constructs a new plane based on the normal and distance to the origin.
    /// </summary>
    /// <param name="normal">The plane normal</param>
    /// <param name="dto">The distance to the origin</param>
    public Plane( Vector3 normal, float dto )
    {
        Normal.Set( normal ).Nor();
        DistanceToOrigin = dto;
    }

    /// <summary>
    /// Constructs a new plane based on the normal and a point on the plane.
    /// </summary>
    /// <param name="normal"> The normal </param>
    /// <param name="point"> The point on the plane  </param>
    public Plane( Vector3 normal, Vector3 point )
    {
        Normal.Set( normal ).Nor();
        DistanceToOrigin = -Normal.Dot( point );
    }

    /// <summary>
    /// Constructs a new plane out of the three given points that are considered to
    /// be on the plane. The normal is calculated via a cross product between
    /// ( point1 - point2 ) x ( point2 - point3 )
    /// </summary>
    /// <param name="point1"> The first point </param>
    /// <param name="point2"> The second point </param>
    /// <param name="point3"> The third point  </param>
    public Plane( Vector3 point1, Vector3 point2, Vector3 point3 )
    {
        Set( point1, point2, point3 );
    }

    /// <returns> The normal </returns>
    public Vector3 Normal { get; } = new();

    /// <summary>
    /// </summary>
    /// <returns>The distance to the origin</returns>
    public float DistanceToOrigin { get; private set; } = 0;

    /// <summary>
    /// Sets the plane normal and distance to the origin based on the three given
    /// points which are considered to be on the plane. The normal is calculated
    /// via a cross product between ( point1 - point2 ) x ( point2 - point3 )
    /// </summary>
    /// <param name="point1"> </param>
    /// <param name="point2"> </param>
    /// <param name="point3">  </param>
    public void Set( Vector3 point1, Vector3 point2, Vector3 point3 )
    {
        Normal.Set( point1 )
              .Sub( point2 )
              .Crs( point2.X - point3.X, point2.Y - point3.Y, point2.Z - point3.Z )
              .Nor();

        DistanceToOrigin = -point1.Dot( Normal );
    }

    /// <summary>
    /// Sets the plane normal and distance
    /// </summary>
    /// <param name="nx"> normal x-component </param>
    /// <param name="ny"> normal y-component </param>
    /// <param name="nz"> normal z-component </param>
    /// <param name="d"> distance to origin  </param>
    public void Set( float nx, float ny, float nz, float d )
    {
        Normal.Set( nx, ny, nz );
        DistanceToOrigin = d;
    }

    /// <summary>
    /// Calculates the shortest signed distance between the plane and the given point.
    /// </summary>
    /// <param name="point">The point</param>
    /// <returns>the shortest signed distance between the plane and the point</returns>
    public float Distance( Vector3 point )
    {
        return Normal.Dot( point ) + DistanceToOrigin;
    }

    /// <summary>
    /// Returns on which side the given point lies relative to the plane and its
    /// normal. PlaneSide.Front refers to the side the plane normal points to.
    /// </summary>
    /// <param name="point"> The point </param>
    /// <returns> The side the point lies relative to the plane  </returns>
    public PlaneSide TestPoint( Vector3 point )
    {
        var dist = Normal.Dot( point ) + DistanceToOrigin;

        if ( dist == 0 )
        {
            return PlaneSide.OnPlane;
        }

        return dist < 0 ? PlaneSide.Back : PlaneSide.Front;
    }

    /// <summary>
    /// Returns on which side the given point lies relative to the plane and its
    /// normal. PlaneSide.Front refers to the side the plane normal points to.
    /// </summary>
    /// <param name="x"> </param>
    /// <param name="y"> </param>
    /// <param name="z"> </param>
    /// <returns> The side the point lies relative to the plane  </returns>
    public PlaneSide TestPoint( float x, float y, float z )
    {
        var dist = Normal.Dot( x, y, z ) + DistanceToOrigin;

        if ( dist == 0 )
        {
            return PlaneSide.OnPlane;
        }

        return dist < 0 ? PlaneSide.Back : PlaneSide.Front;
    }

    /// <summary>
    /// Returns whether the plane is facing the direction vector. Think of the
    /// direction vector as the direction a camera looks in. This method will
    /// return true if the front side of the plane determined by its normal
    /// faces the camera.
    /// </summary>
    /// <param name="direction"> the direction </param>
    /// <returns> whether the plane is front facing  </returns>
    public bool IsFrontFacing( Vector3 direction )
    {
        return Normal.Dot( direction ) <= 0;
    }

    /// <summary>
    /// Sets the plane to the given point and normal.
    /// </summary>
    /// <param name="point">the point on the plane</param>
    /// <param name="norm">the normal of the plane</param>
    public void Set( Vector3 point, Vector3 norm )
    {
        Normal.Set( norm );
        DistanceToOrigin = -point.Dot( norm );
    }

    /// <summary>
    /// </summary>
    /// <param name="pointX"></param>
    /// <param name="pointY"></param>
    /// <param name="pointZ"></param>
    /// <param name="norX"></param>
    /// <param name="norY"></param>
    /// <param name="norZ"></param>
    public void Set( float pointX, float pointY, float pointZ, float norX, float norY, float norZ )
    {
        Normal.Set( norX, norY, norZ );
        DistanceToOrigin = -( ( pointX * norX ) + ( pointY * norY ) + ( pointZ * norZ ) );
    }

    /// <summary>
    /// Sets this plane from the given plane
    /// </summary>
    /// <param name="plane"> the plane  </param>
    public void Set( Plane plane )
    {
        Normal.Set( plane.Normal );
        DistanceToOrigin = plane.DistanceToOrigin;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Normal}, {DistanceToOrigin}";
    }
}
