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


namespace LughSharp.Lugh.Maths;

[PublicAPI]
public interface IPath< T >
{
    /// <summary>
    /// </summary>
    /// <param name="outParam"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    T DerivativeAt( in T outParam, in float t );

    /// <summary>
    /// </summary>
    /// <returns>The value of the path at t where <tt>0 &lt;= t &lt;= 1.</tt></returns>
    T ValueAt( in T value, in float t );

    /// <summary>
    /// </summary>
    /// <returns>
    /// The approximated value (between 0 and 1) on the path which is closest
    /// to the specified value. Note that the implementation of this method
    /// might be optimized for speed against precision.
    /// See <see cref="Locate"/> for a more precise (but more
    /// intensive) method.
    /// </returns>
    float Approximate( in T v );

    /// <summary>
    /// </summary>
    /// <returns>
    /// The precise location (between 0 and 1) on the path which is closest
    /// to the specified value. Note that the implementation of this method
    /// might be CPU intensive. see <see cref="Approximate"/> for a
    /// faster (but less precise) method.
    /// </returns>
    float Locate( T v );

    /// <summary>
    /// </summary>
    /// <param name="samples">
    /// The amount of divisions used to approximate length. Higher values
    /// will produce more precise results but will be more CPU intensive.
    /// </param>
    /// <returns>
    /// An approximated length of the spline through sampling the curve and
    /// accumulating the euclidean distances between the sample points.
    /// </returns>
    float ApproxLength( int samples );
}
