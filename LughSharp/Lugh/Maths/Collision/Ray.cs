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


namespace LughSharp.Lugh.Maths.Collision;

/// <summary>
/// Encapsulates a ray having a starting position and a unit length direction.
/// </summary>
[Serializable]
public class Ray
{
    private static  Vector3 _tmp      = new();
    public readonly Vector3 Direction = new();
    public readonly Vector3 Origin    = new();

    /// <summary>
    /// Constructor, sets the starting position of the ray and the direction.
    /// </summary>
    /// <param name="origin"> The starting position </param>
    /// <param name="direction"> The direction  </param>
    public Ray( Vector3 origin, Vector3 direction )
    {
        this.Origin.Set( origin );
        this.Direction.Set( direction ).Nor();
    }

    /// <summary>
    /// Returns a copy of this ray.
    /// </summary>
    public virtual Ray Copy()
    {
        return new Ray( Origin, Direction );
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
        return result.Set( Direction ).Scale( distance ).Add( Origin );
    }

    /// <summary>
    /// Multiplies the ray by the given matrix.
    /// Use this to transform a ray into another coordinate system.
    /// </summary>
    /// <param name="matrix"> The matrix </param>
    /// <returns> This ray for chaining.  </returns>
    public virtual Ray Multiply( Matrix4 matrix )
    {
        _tmp.Set( Origin ).Add( Direction );
        _tmp.Mul( matrix );

        Origin.Mul( matrix );
        Direction.Set( _tmp.Sub( Origin ) ).Nor();

        return this;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "ray [" + Origin + ":" + Direction + "]";
    }

    /// <summary>
    /// Sets the starting position and the direction of this ray.
    /// </summary>
    /// <param name="orig"> The starting position </param>
    /// <param name="dir"> The direction </param>
    /// <returns> this ray for chaining  </returns>
    public virtual Ray Set( Vector3 orig, Vector3 dir )
    {
        Origin.Set( orig );
        Direction.Set( dir ).Nor();

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
        Origin.Set( x, y, z );
        Direction.Set( dx, dy, dz ).Nor();

        return this;
    }

    /// <summary>
    /// Sets the starting position and direction from the given ray
    /// </summary>
    /// <param name="ray"> The ray </param>
    /// <returns> This ray for chaining  </returns>
    public virtual Ray Set( Ray ray )
    {
        Origin.Set( ray.Origin );
        Direction.Set( ray.Direction ).Nor();

        return this;
    }

    /// <inheritdoc />
    public override bool Equals( object? o )
    {
        if ( o == this )
        {
            return true;
        }

        if ( ( o == null ) || ( o.GetType() != GetType() ) )
        {
            return false;
        }

        var r = ( Ray ) o;

        return Direction.Equals( r.Direction ) && Origin.Equals( r.Origin );
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        const int PRIME = 73;

        var result = PRIME + Direction.GetHashCode();
        result = ( PRIME * result ) + Origin.GetHashCode();

        return result;
    }
}
