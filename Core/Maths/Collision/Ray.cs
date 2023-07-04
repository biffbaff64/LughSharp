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
/// Encapsulates a ray having a starting position and a unit length direction.
/// </summary>
[Serializable]
public class Ray
{
    public readonly Vector3 origin    = new Vector3();
    public readonly Vector3 direction = new Vector3();

    /// <summary>
    /// Constructor, sets the starting position of the ray and the direction.
    /// </summary>
    /// <param name="origin"> The starting position </param>
    /// <param name="direction"> The direction  </param>
    public Ray( Vector3 origin, Vector3 direction )
    {
        this.origin.Set( origin );
        this.direction.Set( direction ).Nor();
    }

    /// <summary>
    /// </summary>
    /// <returns> a copy of this ray. </returns>
    public virtual Ray Cpy()
    {
        return new Ray( this.origin, this.direction );
    }

    /// <summary>
    /// Returns the endpoint given the distance.
    /// This is calculated as startpoint + distance * direction.
    /// </summary>
    /// <param name="result">The vector to set to the result</param>
    /// <param name="distance">The distance from the end point to the start point.</param>
    /// <returns> The out param  </returns>
    public virtual Vector3 GetEndPoint( in Vector3 result, in float distance )
    {
        return result.Set( direction ).Scl( distance ).Add( origin );
    }

    private static Vector3 _tmp = new Vector3();

    /// <summary>
    /// Multiplies the ray by the given matrix.
    /// Use this to transform a ray into another coordinate system.
    /// </summary>
    /// <param name="matrix"> The matrix </param>
    /// <returns> This ray for chaining.  </returns>
    public virtual Ray Mul( Matrix4 matrix )
    {
        _tmp.Set( origin ).Add( direction );
        _tmp.Mul( matrix );

        origin.Mul( matrix );
        direction.Set( _tmp.Sub( origin ) ).Nor();

        return this;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "ray [" + origin + ":" + direction + "]";
    }

    /// <summary>
    /// Sets the starting position and the direction of this ray.
    /// </summary>
    /// <param name="orig"> The starting position </param>
    /// <param name="dir"> The direction </param>
    /// <returns> this ray for chaining  </returns>
    public virtual Ray Set( Vector3 orig, Vector3 dir )
    {
        this.origin.Set( orig );
        this.direction.Set( dir ).Nor();

        return this;
    }

    /// <summary>
    /// Sets this ray from the given starting position and direction.
    /// </summary>
    /// <param name="x"> The x-component of the starting position </param>
    /// <param name="y"> The y-component of the starting position </param>
    /// <param name="z"> The z-component of the starting position </param>
    /// <param name="dx"> The x-component of the direction </param>
    /// <param name="dy"> The y-component of the direction </param>
    /// <param name="dz"> The z-component of the direction </param>
    /// <returns> this ray for chaining  </returns>
    public virtual Ray Set( float x, float y, float z, float dx, float dy, float dz )
    {
        this.origin.Set( x, y, z );
        this.direction.Set( dx, dy, dz ).Nor();

        return this;
    }

    /// <summary>
    /// Sets the starting position and direction from the given ray
    /// </summary>
    /// <param name="ray"> The ray </param>
    /// <returns> This ray for chaining  </returns>
    public virtual Ray Set( Ray ray )
    {
        this.origin.Set( ray.origin );
        this.direction.Set( ray.direction ).Nor();

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public override bool Equals( object? o )
    {
        if ( o == this ) return true;

        if ( ( o == null ) || ( o.GetType() != this.GetType() ) ) return false;

        var r = ( Ray )o;

        return this.direction.Equals( r.direction ) && this.origin.Equals( r.origin );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        const int prime = 73;

        var result = prime + this.direction.GetHashCode();
        result = ( prime * result ) + this.origin.GetHashCode();

        return result;
    }
}