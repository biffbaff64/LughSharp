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


namespace Corelib.LibCore.Maths;

/// <summary>
/// Encapsulates a general vector. Allows chaining operations by returning
/// a reference to itself in all modification methods. See Vector2 and
/// Vector3 for specific implementations.
/// </summary>
[PublicAPI]
public interface IVector< T > where T : IVector< T >
{
    /// <summary>
    /// Returns a copy of this vector
    /// </summary>
    T Cpy();

    /// <summary>
    /// Returns the euclidean length.
    /// </summary>
    float Len();

    /// <summary>
    /// This method is faster than IVector.Len() because it avoids calculating
    /// a square root. It is useful for comparisons, but not for getting exact
    /// lengths, as the return value is the square of the actual length.
    /// </summary>
    /// <returns> The squared euclidean length  </returns>
    float Len2();

    /// <summary>
    /// Limits the length of this vector, based on the desired maximum length.
    /// </summary>
    /// <param name="limit"> desired maximum length for this vector </param>
    /// <returns> this vector for chaining  </returns>
    T Limit( float limit );

    /// <summary>
    /// Limits the length of this vector, based on the desired maximum length squared.
    /// This method is slightly faster than Limit().
    /// </summary>
    /// <param name="limit2"> squared desired maximum length for this vector </param>
    /// <returns> this vector for chaining </returns>
    /// <see cref="Len2() "/>
    T Limit2( float limit2 );

    /// <summary>
    /// Sets the length of this vector. Does nothing if this vector is zero.
    /// </summary>
    /// <param name="len"> desired length for this vector </param>
    /// <returns> this vector for chaining  </returns>
    T SetLength( float len );

    /// <summary>
    /// Sets the length of this vector, based on the square of the desired length.
    /// Does nothing if this vector is zero.
    /// This method is slightly faster than setLength().
    /// </summary>
    /// <param name="len2"> desired square of the length for this vector </param>
    /// <returns> this vector for chaining </returns>
    T SetLength2( float len2 );

    /// <summary>
    /// Clamps this vector's length to given min and max values
    /// </summary>
    /// <param name="min"> Min length </param>
    /// <param name="max"> Max length </param>
    /// <returns> This vector for chaining  </returns>
    T Clamp( float min, float max );

    /// <summary>
    /// Sets this vector from the given vector
    /// </summary>
    /// <param name="v"> The vector </param>
    /// <returns> This vector for chaining  </returns>
    T Set( T v );

    /// <summary>
    /// Subtracts the given vector from this vector.
    /// </summary>
    /// <param name="v"> The vector </param>
    /// <returns> This vector for chaining  </returns>
    T Sub( T v );

    /// <summary>
    /// Normalizes this vector. Does nothing if it is zero.
    /// </summary>
    /// <returns> This vector for chaining  </returns>
    T Nor();

    /// <summary>
    /// Adds the given vector to this vector
    /// </summary>
    /// <param name="v"> The vector </param>
    /// <returns> This vector for chaining  </returns>
    T Add( T v );

    /// <summary>
    /// Returns the Dot product between this vector and the supplied vector.
    /// </summary>
    float Dot( T v );

    /// <summary>
    /// Scales this vector by a scalar
    /// </summary>
    /// <param name="scalar"> The scalar </param>
    /// <returns> This vector for chaining  </returns>
    T Scale( float scalar );

    /// <summary>
    /// Scales this vector by another vector
    /// </summary>
    /// <returns> This vector for chaining  </returns>
    T Scale( T v );

    /// <summary>
    /// Returns the Distance between this vector and the supplied vector.
    /// </summary>
    float Distance( T v );

    /// <summary>
    /// This method is faster than <see cref="Distance"/> because it
    /// avoids calculating a square root. It is useful for comparisons, but not for
    /// getting accurate distances, as the return value is the square of the actual
    /// distance.
    /// </summary>
    /// <param name="v"> The other vector </param>
    /// <returns> the squared distance between this and the other vector  </returns>
    float Distance2( T v );

    /// <summary>
    /// Linearly interpolates between this vector and the target vector by alpha
    /// which is in the range [0,1]. The result is stored in this vector.
    /// </summary>
    /// <param name="target"> The target vector </param>
    /// <param name="alpha"> The interpolation coefficient </param>
    /// <returns> This vector for chaining.  </returns>
    T Lerp( T target, float alpha );

    /// <summary>
    /// Interpolates between this vector and the given target vector by alpha
    /// (within range [0,1]) using the given Interpolation method. the result
    /// is stored in this vector.
    /// </summary>
    /// <param name="target"> The target vector </param>
    /// <param name="alpha"> The interpolation coefficient </param>
    /// <param name="interpolator"> An Interpolation object describing the used interpolation method </param>
    /// <returns> This vector for chaining.  </returns>
    T Interpolate( T target, float alpha, IInterpolation interpolator );

    /// <summary>
    /// Sets this vector to the unit vector with a random direction
    /// </summary>
    /// <returns> This vector for chaining  </returns>
    T SetToRandomDirection();

    /// <summary>
    /// </summary>
    /// <returns> Whether this vector is a unit length vector within the given margin. </returns>
    bool IsUnit( float margin );

    /// <summary>
    /// </summary>
    /// <returns> Whether this vector is a zero vector </returns>
    bool IsZero();

    /// <summary>
    /// </summary>
    /// <returns> Whether the length of this vector is smaller than the given margin </returns>
    bool IsZero( float margin );

    /// <summary>
    /// </summary>
    /// <returns>
    /// true if this vector is in line with the other vector (either in the same
    /// or the opposite direction)
    /// </returns>
    bool IsOnLine( T other, float epsilon );

    /// <summary>
    /// </summary>
    /// <returns>
    /// true if this vector is in line with the other vector (either in the
    /// same or the opposite direction)
    /// </returns>
    bool IsOnLine( T other );

    /// <summary>
    /// </summary>
    /// <returns>true if this vector is collinear with the other vector</returns>
    bool IsCollinear( T other, float epsilon );

    /// <summary>
    /// </summary>
    /// <returns> true if this vector is collinear with the other vector</returns>
    bool IsCollinear( T other );

    /// <summary>
    /// </summary>
    /// <returns> true if this vector is opposite collinear with the other vector</returns>
    bool IsCollinearOpposite( T other, float epsilon );

    /// <summary>
    /// Returns true if this vector is opposite collinear with the other vector.
    /// </summary>
    bool IsCollinearOpposite( T other );

    /// <summary>
    /// Returns whether this vector is perpendicular with the other vector.
    /// True if the dot product is 0.
    /// </summary>
    bool IsPerpendicular( T other );

    /// <summary>
    /// Whether this vector is perpendicular with the other vector.
    /// True if the dot product is 0.
    /// </summary>
    bool IsPerpendicular( T other, float epsilon );

    /// <summary>
    /// Returns whether this vector has similar direction compared to the other vector.
    /// True if the normalized dot product is > 0.
    /// </summary>
    bool HasSameDirection( T other );

    /// <summary>
    /// Returns whether this vector has opposite direction compared to the
    /// other vector. True if the normalized dot product is less than 0.
    /// </summary>
    bool HasOppositeDirection( T other );

    /// <summary>
    /// Compares this vector with the other vector, using the supplied
    /// epsilon for fuzzy equality testing.
    /// </summary>
    /// <param name="other"> </param>
    /// <param name="epsilon"> </param>
    /// <returns> whether the vectors have fuzzy equality.  </returns>
    bool EpsilonEquals( T other, float epsilon );

    /// <summary>
    /// First scale a supplied vector, then add it to this vector.
    /// </summary>
    /// <param name="v"> addition vector </param>
    /// <param name="scalar"> for scaling the addition vector  </param>
    T MulAdd( T v, float scalar );

    /// <summary>
    /// First scale a supplied vector, then add it to this vector.
    /// </summary>
    /// <param name="v"> addition vector </param>
    /// <param name="mulVec"> vector by whose values the addition vector will be scaled  </param>
    T MulAdd( T v, T mulVec );

    /// <summary>
    /// Sets the components of this vector to 0
    /// </summary>
    /// <returns> This vector for chaining  </returns>
    T SetZero();
}
