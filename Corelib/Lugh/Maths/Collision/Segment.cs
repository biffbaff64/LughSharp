// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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
/// A Segment is a line in 3-space having a starting and an ending position.
/// </summary>
[Serializable]
public class Segment
{
    // ========================================================================

    private readonly Vector3 _vecA = new();
    private readonly Vector3 _vecB = new();

    // ========================================================================

    /// <summary>
    /// Constructs a new Segment from the two points given.
    /// </summary>
    /// <param name="a">the first point</param>
    /// <param name="b">the second point</param>
    public Segment( Vector3 a, Vector3 b )
    {
        VectorA.Set( a );
        VectorB.Set( b );

        _vecA.Set( a );
        _vecB.Set( b );
    }

    /// <summary>
    /// Constructs a new Segment from the two points given.
    /// </summary>
    /// <param name="aX"> the x-coordinate of the first point </param>
    /// <param name="aY"> the y-coordinate of the first point </param>
    /// <param name="aZ"> the z-coordinate of the first point </param>
    /// <param name="bX"> the x-coordinate of the second point </param>
    /// <param name="bY"> the y-coordinate of the second point </param>
    /// <param name="bZ"> the z-coordinate of the second point  </param>
    public Segment( float aX, float aY, float aZ, float bX, float bY, float bZ )
    {
        VectorA.Set( aX, aY, aZ );
        VectorB.Set( bX, bY, bZ );
    }

    public Vector3 VectorA { get; private set; } = new(); // the starting position
    public Vector3 VectorB { get; private set; } = new(); // the ending position

    public virtual float Len()
    {
        return VectorA.Distance( VectorB );
    }

    public virtual float Len2()
    {
        return VectorA.Distance2( VectorB );
    }

    /// <summary>
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
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

        var s = ( Segment ) o;

        return VectorA.Equals( s.VectorA ) && VectorB.Equals( s.VectorB );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        const int PRIME = 71;

        var result = PRIME + _vecA.GetHashCode();
        result = ( PRIME * result ) + _vecB.GetHashCode();

        return result;
    }
}
