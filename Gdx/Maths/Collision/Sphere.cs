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


namespace LibGDXSharp.Gdx.Maths.Collision;

/// <summary>
///     Encapsulates a 3D sphere with a center and a radius
/// </summary>
[PublicAPI, Serializable]
public class Sphere
{
    private const   float   PI43 = ( MathUtils.PI * 4f ) / 3f;
    public readonly Vector3 center; // the center of the sphere
    public readonly float   radius; // the radius of the sphere

    // ------------------------------------------------------------------------

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
        const int PRIME = 71;

        var result = PRIME + center.GetHashCode();
        result = ( PRIME * result ) + NumberUtils.FloatToRawIntBits( radius );

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
    ///     Returns the volume of this sphere.
    /// </summary>
    public virtual float Volume() => PI43 * radius * radius * radius;

    /// <summary>
    ///     Returns the surface area of this sphere.
    /// </summary>
    public virtual float SurfaceArea() => 4 * MathUtils.PI * radius * radius;
}
