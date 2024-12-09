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


namespace Corelib.Lugh.Maths.Collision;

/// <summary>
/// Encapsulates a 3D sphere with a center and a radius
/// </summary>
[PublicAPI]
public class Sphere
{
    private const   float   PI43 = ( MathUtils.PI * 4f ) / 3f;
    public readonly Vector3 Center; // the center of the sphere
    public readonly float   Radius; // the radius of the sphere

    // ========================================================================

    /// <summary>
    /// Constructs a sphere with the given center and radius
    /// </summary>
    /// <param name="center"> The center </param>
    /// <param name="radius"> The radius  </param>
    public Sphere( Vector3 center, float radius )
    {
        Center = new Vector3( center );
        Radius = radius;
    }

    /// <summary>
    /// </summary>
    /// <param name="sphere"> the other sphere </param>
    /// <returns> whether this and the other sphere overlap  </returns>
    public virtual bool Overlaps( Sphere sphere )
    {
        return Center.Distance2( sphere.Center ) < ( ( Radius + sphere.Radius ) * ( Radius + sphere.Radius ) );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        const int PRIME = 71;

        var result = PRIME + Center.GetHashCode();
        result = ( PRIME * result ) + NumberUtils.FloatToRawIntBits( Radius );

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

        var s = ( Sphere ) o;

        return MathUtils.IsEqual( Radius, Radius ) && Center.Equals( s.Center );
    }

    /// <summary>
    /// Returns the volume of this sphere.
    /// </summary>
    public virtual float Volume()
    {
        return PI43 * Radius * Radius * Radius;
    }

    /// <summary>
    /// Returns the surface area of this sphere.
    /// </summary>
    public virtual float SurfaceArea()
    {
        return 4 * MathUtils.PI * Radius * Radius;
    }
}
