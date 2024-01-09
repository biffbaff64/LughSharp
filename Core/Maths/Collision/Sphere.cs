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

namespace LibGDXSharp.Maths.Collision;

/// <summary>
///     Encapsulates a 3D sphere with a center and a radius
/// </summary>
[Serializable]
public class Sphere
{
    private readonly static float   PI43 = ( MathUtils.PI * 4f ) / 3f;
    public readonly         Vector3 center; // the center of the sphere
    public readonly         float   radius; // the radius of the sphere

    /// <summary>
    ///     Constructs a sphere with the given center and radius
    /// </summary>
    /// <param name="center"> The center </param>
    /// <param name="radius"> The radius  </param>
    public Sphere( Vector3 center, float radius )
    {
        this.center = new Vector3( center );
        this.radius = radius;
    }

    /// <summary>
    /// </summary>
    /// <param name="sphere"> the other sphere </param>
    /// <returns> whether this and the other sphere overlap  </returns>
    public virtual bool Overlaps( Sphere sphere ) => center.Dst2( sphere.center ) < ( ( radius + sphere.radius ) * ( radius + sphere.radius ) );

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        const int prime = 71;

        var result = prime + center.GetHashCode();
        result = ( prime * result ) + NumberUtils.FloatToRawIntBits( radius );

        return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public override bool Equals( object? o )
    {
        if ( this == o )
        {
            return true;
        }

        if ( ( o == null ) || ( o.GetType() != GetType() ) )
        {
            return false;
        }

        var s = ( Sphere )o;

        return MathUtils.IsEqual( radius, radius ) && center.Equals( s.center );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual float Volume() => PI43 * radius * radius * radius;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual float SurfaceArea() => 4 * MathUtils.PI * radius * radius;
}
