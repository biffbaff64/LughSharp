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

namespace LibGDXSharp.Maths;

[PublicAPI]
public interface IPath<T>
{
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