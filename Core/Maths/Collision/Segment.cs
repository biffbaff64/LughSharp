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
/// A Segment is a line in 3-space having a starting and an ending position.
/// </summary>
[Serializable]
public class Segment
{
    public readonly Vector3 vectorA = new Vector3(); // the starting position
    public readonly Vector3 vectorB = new Vector3(); // the ending position

    /// <summary>
    /// Constructs a new Segment from the two points given.
    /// </summary>
    /// <param name="a">the first point</param>
    /// <param name="b">the second point</param>
    public Segment( Vector3 a, Vector3 b )
    {
        this.vectorA.Set( a );
        this.vectorB.Set( b );
    }

    /// <summary>
    /// Constructs a new Segment from the two points given. </summary>
    /// <param name="aX"> the x-coordinate of the first point </param>
    /// <param name="aY"> the y-coordinate of the first point </param>
    /// <param name="aZ"> the z-coordinate of the first point </param>
    /// <param name="bX"> the x-coordinate of the second point </param>
    /// <param name="bY"> the y-coordinate of the second point </param>
    /// <param name="bZ"> the z-coordinate of the second point  </param>
    public Segment( float aX, float aY, float aZ, float bX, float bY, float bZ )
    {
        this.vectorA.Set( aX, aY, aZ );
        this.vectorB.Set( bX, bY, bZ );
    }

    public virtual float Len()
    {
        return vectorA.Dst( vectorB );
    }

    public virtual float Len2()
    {
        return vectorA.Dst2( vectorB );
    }

    /// <summary>
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public override bool Equals( object? o )
    {
        if ( o == this ) return true;
        if ( ( o == null ) || ( o.GetType() != this.GetType() ) ) return false;

        var s = ( Segment )o;

        return this.vectorA.Equals( s.vectorA ) && this.vectorB.Equals( s.vectorB );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        const int prime = 71;

        var result = prime + this.vectorA.GetHashCode();
        result = ( prime * result ) + this.vectorB.GetHashCode();

        return result;
    }
}