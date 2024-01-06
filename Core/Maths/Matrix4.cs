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

public class Matrix4
{
    /// <summary>
    ///     XX: Typically the unrotated X component for scaling, also the cosine
    ///     of the angle when rotated on the Y and/or Z axis. On Vector3 multiplication
    ///     this value is multiplied with the source X component and added to the
    ///     target X component.
    /// </summary>
    public const int M00 = 0;
    /// <summary>
    ///     XY: Typically the negative sine of the angle when rotated on the Z axis.
    ///     On Vector3 multiplication this value is multiplied with the source Y
    ///     component and added to the target X component.
    /// </summary>
    public const int M01 = 4;
    /// <summary>
    ///     XZ: Typically the sine of the angle when rotated on the Y axis.
    ///     On Vector3 multiplication this value is multiplied with the
    ///     source Z component and added to the target X component.
    /// </summary>
    public const int M02 = 8;
    /// <summary>
    ///     XW: Typically the translation of the X component. On Vector3 multiplication
    ///     this value is added to the target X component.
    /// </summary>
    public const int M03 = 12;
    /// <summary>
    ///     YX: Typically the sine of the angle when rotated on the Z axis. On Vector3
    ///     multiplication this value is multiplied with the source X component and
    ///     added to the target Y component.
    /// </summary>
    public const int M10 = 1;
    /// <summary>
    ///     YY: Typically the unrotated Y component for scaling, also the cosine of the
    ///     angle when rotated on the X and/or Z axis. On Vector3 multiplication this value
    ///     is multiplied with the source Y component and added to the target Y component.
    /// </summary>
    public const int M11 = 5;
    /// <summary>
    ///     YZ: Typically the negative sine of the angle when rotated on the X axis.
    ///     On Vector3 multiplication this value is multiplied with the source Z component
    ///     and added to the target Y component.
    /// </summary>
    public const int M12 = 9;
    /// <summary>
    ///     YW: Typically the translation of the Y component.
    ///     On Vector3 multiplication this value is added to the target Y component.
    /// </summary>
    public const int M13 = 13;
    /// <summary>
    ///     ZX: Typically the negative sine of the angle when rotated on the Y axis.
    ///     On Vector3 multiplication this value is multiplied with the source X component
    ///     and added to the target Z component.
    /// </summary>
    public const int M20 = 2;
    /// <summary>
    ///     ZY: Typically the sine of the angle when rotated on the X axis.
    ///     On Vector3 multiplication this value is multiplied with the source Y component
    ///     and added to the target Z component.
    /// </summary>
    public const int M21 = 6;
    /// <summary>
    ///     ZZ: Typically the unrotated Z component for scaling, also the cosine of the angle
    ///     when rotated on the X and/or Y axis. On Vector3 multiplication this value is
    ///     multiplied with the source Z component and added to the target Z component.
    /// </summary>
    public const int M22 = 10;
    /// <summary>
    ///     ZW: Typically the translation of the Z component. On Vector3 multiplication
    ///     this value is added to the target Z component.
    /// </summary>
    public const int M23 = 14;
    /// <summary>
    ///     WX: Typically the value zero. On Vector3 multiplication this value is ignored.
    /// </summary>
    public const int M30 = 3;
    /// <summary>
    ///     WY: Typically the value zero. On Vector3 multiplication this value is ignored.
    /// </summary>
    public const int M31 = 7;
    /// <summary>
    ///     WZ: Typically the value zero. On Vector3 multiplication this value is ignored.
    /// </summary>
    public const int M32 = 11;
    /// <summary>
    ///     WW: Typically the value one. On Vector3 multiplication this value is ignored.
    /// </summary>
    public const int M33 = 15;

    public readonly static Quaternion Quat       = new();
    public readonly static Quaternion Quat2      = new();
    public readonly static Vector3    LVez       = new();
    public readonly static Vector3    LVex       = new();
    public readonly static Vector3    LVey       = new();
    public readonly static Vector3    TmpVec     = new();
    public readonly static Matrix4    TmpMat     = new();
    public readonly static Vector3    Right      = new();
    public readonly static Vector3    TmpForward = new();
    public readonly static Vector3    TmpUp      = new();

    public readonly float[] val = new float[ 16 ];

    /// <summary>
    ///     Constructs an identity matrix
    /// </summary>
    public Matrix4()
    {
        val[ M00 ] = 1f;
        val[ M11 ] = 1f;
        val[ M22 ] = 1f;
        val[ M33 ] = 1f;
    }

    /// <summary>
    ///     Constructs a matrix from the given matrix.
    /// </summary>
    /// <param name="matrix">
    ///     The matrix to copy. (This matrix is not modified)
    /// </param>
    public Matrix4( Matrix4 matrix ) => Set( matrix );

    /// <summary>
    ///     Constructs a matrix from the given float array. The array must have at
    ///     least 16 elements; the first 16 will be copied.
    /// </summary>
    /// <param name="values">
    ///     The float array to copy. Remember that this matrix is in column-major order.
    ///     (The float array is not modified.)
    ///     <para>
    ///         See here:
    ///         <a href="http://en.wikipedia.org/wiki/Row-major_order">wikipedia.org/wiki/Row-major_order</a>
    ///     </para>
    /// </param>
    public Matrix4( float[] values ) => Set( values );

    /// <summary>
    ///     Constructs a rotation matrix from the given <see cref="Quaternion" />.
    /// </summary>
    /// <param name="quaternion">The quaternion to be copied. (The quaternion is not modified)</param>
    public Matrix4( Quaternion quaternion ) => Set( quaternion );

    /// <summary>
    ///     Construct a matrix from the given translation, rotation and scale.
    /// </summary>
    /// <param name="position"> The translation </param>
    /// <param name="rotation"> The rotation, must be normalized </param>
    /// <param name="scale"> The scale</param>
    public Matrix4( Vector3 position, Quaternion rotation, Vector3 scale ) => Set( position, rotation, scale );

    /// <summary>
    /// </summary>
    /// <returns> the backing float array </returns>
    public float[] Values => val;

    /// <summary>
    ///     Sets the matrix to the given matrix.
    /// </summary>
    /// <param name="matrix"> The matrix that is to be copied.(The given matrix is not modified)</param>
    /// <returns> This matrix for the purpose of chaining methods together.</returns>
    public Matrix4 Set( Matrix4 matrix ) => Set( matrix.val );

    /// <summary>
    ///     Sets the matrix to the given matrix as a float array. The float array must
    ///     have at least 16 elements; the first 16 will be copied.
    /// </summary>
    /// <param name="values">
    ///     The matrix, in float form, that is to be copied. Remember that this matrix is in
    ///     Column-Major order.
    ///     <p>
    ///         See here:
    ///         <a href="http://en.wikipedia.org/wiki/Row-major_order">wikipedia.org/wiki/Row-major_order</a>
    ///     </p>
    /// </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Set( float[] values )
    {
        Array.Copy( values, 0, val, 0, val.Length );

        return this;
    }

    /// <summary>
    ///     Sets the matrix to a rotation matrix representing the quaternion.
    /// </summary>
    /// <param name="quaternion"> The quaternion that is to be used to Set this matrix. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Set( Quaternion quaternion ) => Set( quaternion.X, quaternion.Y, quaternion.Z, quaternion.W );

    /// <summary>
    ///     Sets the matrix to a rotation matrix representing the quaternion.
    /// </summary>
    /// <param name="quaternionX"> The X component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionY"> The Y component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionZ"> The Z component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionW"> The W component of the quaternion that is to be used to Set this matrix. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Set( float quaternionX, float quaternionY, float quaternionZ, float quaternionW )
        => Set( 0f, 0f, 0f, quaternionX, quaternionY, quaternionZ, quaternionW );

    /// <summary>
    ///     Set this matrix to the specified translation and rotation.
    /// </summary>
    /// <param name="position"> The translation </param>
    /// <param name="orientation"> The rotation, must be normalized </param>
    /// <returns> This matrix for chaining  </returns>
    public Matrix4 Set( Vector3 position, Quaternion orientation )
        => Set( position.X, position.Y, position.Z, orientation.X, orientation.Y, orientation.Z, orientation.W );

    /// <summary>
    ///     Sets the matrix to a rotation matrix representing the translation and quaternion.
    /// </summary>
    /// <param name="translationX"> The X component of the translation that is to be used to Set this matrix. </param>
    /// <param name="translationY"> The Y component of the translation that is to be used to Set this matrix. </param>
    /// <param name="translationZ"> The Z component of the translation that is to be used to Set this matrix. </param>
    /// <param name="quaternionX"> The X component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionY"> The Y component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionZ"> The Z component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionW"> The W component of the quaternion that is to be used to Set this matrix. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Set( float translationX,
                        float translationY,
                        float translationZ,
                        float quaternionX,
                        float quaternionY,
                        float quaternionZ,
                        float quaternionW )
    {
        float xs = quaternionX * 2f, ys = quaternionY * 2f, zs = quaternionZ * 2f;
        float wx = quaternionW * xs, wy = quaternionW * ys, wz = quaternionW * zs;
        float xx = quaternionX * xs, xy = quaternionX * ys, xz = quaternionX * zs;
        float yy = quaternionY * ys, yz = quaternionY * zs, zz = quaternionZ * zs;

        val[ M00 ] = 1f - ( yy + zz );
        val[ M01 ] = xy - wz;
        val[ M02 ] = xz + wy;
        val[ M03 ] = translationX;

        val[ M10 ] = xy + wz;
        val[ M11 ] = 1f - ( xx + zz );
        val[ M12 ] = yz - wx;
        val[ M13 ] = translationY;

        val[ M20 ] = xz - wy;
        val[ M21 ] = yz + wx;
        val[ M22 ] = 1f - ( xx + yy );
        val[ M23 ] = translationZ;

        val[ M30 ] = 0f;
        val[ M31 ] = 0f;
        val[ M32 ] = 0f;
        val[ M33 ] = 1f;

        return this;
    }

    /// <summary>
    ///     Set this matrix to the specified translation, rotation and scale.
    /// </summary>
    /// <param name="position"> The translation </param>
    /// <param name="orientation"> The rotation, must be normalized </param>
    /// <param name="scale"> The scale </param>
    /// <returns> This matrix for chaining  </returns>
    public Matrix4 Set( Vector3 position, Quaternion orientation, Vector3 scale ) => Set(
        position.X,
        position.Y,
        position.Z,
        orientation.X,
        orientation.Y,
        orientation.Z,
        orientation.W,
        scale.X,
        scale.Y,
        scale.Z
        );

    /// <summary>
    ///     Sets the matrix to a rotation matrix representing the translation and quaternion.
    /// </summary>
    /// <param name="translationX"> The X component of the translation that is to be used to Set this matrix. </param>
    /// <param name="translationY"> The Y component of the translation that is to be used to Set this matrix. </param>
    /// <param name="translationZ"> The Z component of the translation that is to be used to Set this matrix. </param>
    /// <param name="quaternionX"> The X component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionY"> The Y component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionZ"> The Z component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="quaternionW"> The W component of the quaternion that is to be used to Set this matrix. </param>
    /// <param name="scaleX"> The X component of the scaling that is to be used to Set this matrix. </param>
    /// <param name="scaleY"> The Y component of the scaling that is to be used to Set this matrix. </param>
    /// <param name="scaleZ"> The Z component of the scaling that is to be used to Set this matrix. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Set( float translationX,
                        float translationY,
                        float translationZ,
                        float quaternionX,
                        float quaternionY,
                        float quaternionZ,
                        float quaternionW,
                        float scaleX,
                        float scaleY,
                        float scaleZ )
    {
        float xs = quaternionX * 2f, ys = quaternionY * 2f, zs = quaternionZ * 2f;
        float wx = quaternionW * xs, wy = quaternionW * ys, wz = quaternionW * zs;
        float xx = quaternionX * xs, xy = quaternionX * ys, xz = quaternionX * zs;
        float yy = quaternionY * ys, yz = quaternionY * zs, zz = quaternionZ * zs;

        val[ M00 ] = scaleX * ( 1.0f - ( yy + zz ) );
        val[ M01 ] = scaleY * ( xy - wz );
        val[ M02 ] = scaleZ * ( xz + wy );
        val[ M03 ] = translationX;

        val[ M10 ] = scaleX * ( xy + wz );
        val[ M11 ] = scaleY * ( 1.0f - ( xx + zz ) );
        val[ M12 ] = scaleZ * ( yz - wx );
        val[ M13 ] = translationY;

        val[ M20 ] = scaleX * ( xz - wy );
        val[ M21 ] = scaleY * ( yz + wx );
        val[ M22 ] = scaleZ * ( 1.0f - ( xx + yy ) );
        val[ M23 ] = translationZ;

        val[ M30 ] = 0f;
        val[ M31 ] = 0f;
        val[ M32 ] = 0f;
        val[ M33 ] = 1f;

        return this;
    }

    /// <summary>
    ///     Sets the four columns of the matrix which correspond to the x-, y- and z-axis of the vector space this matrix
    ///     creates as
    ///     well as the 4th column representing the translation of any point that is multiplied by this matrix.
    /// </summary>
    /// <param name="xAxis"> The x-axis. </param>
    /// <param name="yAxis"> The y-axis. </param>
    /// <param name="zAxis"> The z-axis. </param>
    /// <param name="pos"> The translation vector.  </param>
    public Matrix4 Set( Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, Vector3 pos )
    {
        val[ M00 ] = xAxis.X;
        val[ M01 ] = xAxis.Y;
        val[ M02 ] = xAxis.Z;
        val[ M10 ] = yAxis.X;
        val[ M11 ] = yAxis.Y;
        val[ M12 ] = yAxis.Z;
        val[ M20 ] = zAxis.X;
        val[ M21 ] = zAxis.Y;
        val[ M22 ] = zAxis.Z;
        val[ M03 ] = pos.X;
        val[ M13 ] = pos.Y;
        val[ M23 ] = pos.Z;
        val[ M30 ] = 0f;
        val[ M31 ] = 0f;
        val[ M32 ] = 0f;
        val[ M33 ] = 1f;

        return this;
    }

    /// <returns> a copy of this matrix </returns>
    public Matrix4 Cpy() => new( this );

    /// <summary>
    ///     Adds a translational component to the matrix in the 4th column.
    ///     The other columns are untouched.
    /// </summary>
    /// <param name="vector">
    ///     The translation vector to add to the current matrix. (This vector is not modified)
    /// </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Trn( Vector3 vector )
    {
        val[ M03 ] += vector.X;
        val[ M13 ] += vector.Y;
        val[ M23 ] += vector.Z;

        return this;
    }

    /// <summary>
    ///     Adds a translational component to the matrix in the 4th column.
    ///     The other columns are untouched.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <param name="z"> The z-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Trn( float x, float y, float z )
    {
        val[ M03 ] += x;
        val[ M13 ] += y;
        val[ M23 ] += z;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with the given matrix, storing the result in this matrix. For example:
    ///     <code>
    /// A.mul(B) results in A := AB.
    /// </code>
    /// </summary>
    /// <param name="matrix"> The other matrix to multiply by. </param>
    /// <returns> This matrix for the purpose of chaining operations together.  </returns>
    public Matrix4 Mul( Matrix4 matrix )
    {
        Mul( val, matrix.val );

        return this;
    }

    /// <summary>
    ///     Premultiplies this matrix with the given matrix, storing the result in this matrix. For example:
    ///     <para>
    ///         <tt>A.mulLeft(B) results in A := BA.</tt>
    ///     </para>
    /// </summary>
    /// <param name="matrix"> The other matrix to multiply by. </param>
    /// <returns> This matrix for the purpose of chaining operations together.  </returns>
    public Matrix4 MulLeft( Matrix4 matrix )
    {
        TmpMat.Set( matrix );

        Mul( TmpMat.val, val );

        return Set( TmpMat );
    }

    /// <summary>
    ///     Transposes the matrix.
    /// </summary>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Tra()
    {
        var m01 = val[ M01 ];
        var m02 = val[ M02 ];
        var m03 = val[ M03 ];
        var m12 = val[ M12 ];
        var m13 = val[ M13 ];
        var m23 = val[ M23 ];

        val[ M01 ] = val[ M10 ];
        val[ M02 ] = val[ M20 ];
        val[ M03 ] = val[ M30 ];
        val[ M10 ] = m01;
        val[ M12 ] = val[ M21 ];
        val[ M13 ] = val[ M31 ];
        val[ M20 ] = m02;
        val[ M21 ] = m12;
        val[ M23 ] = val[ M32 ];
        val[ M30 ] = m03;
        val[ M31 ] = m13;
        val[ M32 ] = m23;

        return this;
    }

    /// <summary>
    ///     Sets the matrix to an identity matrix.
    /// </summary>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Idt()
    {
        val[ M00 ] = 1f;
        val[ M01 ] = 0f;
        val[ M02 ] = 0f;
        val[ M03 ] = 0f;
        val[ M10 ] = 0f;
        val[ M11 ] = 1f;
        val[ M12 ] = 0f;
        val[ M13 ] = 0f;
        val[ M20 ] = 0f;
        val[ M21 ] = 0f;
        val[ M22 ] = 1f;
        val[ M23 ] = 0f;
        val[ M30 ] = 0f;
        val[ M31 ] = 0f;
        val[ M32 ] = 0f;
        val[ M33 ] = 1f;

        return this;
    }

    /// <summary>
    ///     Inverts the matrix. Stores the result in this matrix.
    /// </summary>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    /// <exception cref="GdxRuntimeException"> if the matrix is singular (not invertible)  </exception>
    public Matrix4 Inv()
    {
        var lDet = ( ( ( ( ( ( ( ( ( ( ( ( val[ M30 ] * val[ M21 ] * val[ M12 ] * val[ M03 ] )
                                       - ( val[ M20 ] * val[ M31 ] * val[ M12 ] * val[ M03 ] )
                                       - ( val[ M30 ]
                                         * val[ M11 ]
                                         * val[ M22 ]
                                         * val[ M03 ] ) )
                                     + ( val[ M10 ] * val[ M31 ] * val[ M22 ] * val[ M03 ] )
                                     + ( val[ M20 ] * val[ M11 ] * val[ M32 ] * val[ M03 ] ) )
                                   - ( val[ M10 ]
                                     * val[ M21 ]
                                     * val[ M32 ]
                                     * val[ M03 ] )
                                   - ( val[ M30 ] * val[ M21 ] * val[ M02 ] * val[ M13 ] ) )
                                 + ( val[ M20 ] * val[ M31 ] * val[ M02 ] * val[ M13 ] )
                                 + ( val[ M30 ] * val[ M01 ] * val[ M22 ] * val[ M13 ] ) )
                               - ( val[ M00 ] * val[ M31 ] * val[ M22 ] * val[ M13 ] )
                               - ( val[ M20 ]
                                 * val[ M01 ]
                                 * val[ M32 ]
                                 * val[ M13 ] ) )
                             + ( val[ M00 ] * val[ M21 ] * val[ M32 ] * val[ M13 ] )
                             + ( val[ M30 ] * val[ M11 ] * val[ M02 ] * val[ M23 ] ) )
                           - ( val[ M10 ]
                             * val[ M31 ]
                             * val[ M02 ]
                             * val[ M23 ] )
                           - ( val[ M30 ] * val[ M01 ] * val[ M12 ] * val[ M23 ] ) )
                         + ( val[ M00 ] * val[ M31 ] * val[ M12 ] * val[ M23 ] )
                         + ( val[ M10 ]
                           * val[ M01 ]
                           * val[ M32 ]
                           * val[ M23 ] ) )
                       - ( val[ M00 ] * val[ M11 ] * val[ M32 ] * val[ M23 ] )
                       - ( val[ M20 ] * val[ M11 ] * val[ M02 ] * val[ M33 ] ) )
                     + ( val[ M10 ] * val[ M21 ] * val[ M02 ] * val[ M33 ] )
                     + ( val[ M20 ] * val[ M01 ] * val[ M12 ] * val[ M33 ] ) )
                   - ( val[ M00 ]
                     * val[ M21 ]
                     * val[ M12 ]
                     * val[ M33 ] )
                   - ( val[ M10 ] * val[ M01 ] * val[ M22 ] * val[ M33 ] ) )
                 + ( val[ M00 ] * val[ M11 ] * val[ M22 ] * val[ M33 ] );

        if ( lDet == 0f )
        {
            throw new GdxRuntimeException( "non-invertible matrix" );
        }

        var m00 = ( ( ( ( val[ M12 ] * val[ M23 ] * val[ M31 ] )
                      - ( val[ M13 ] * val[ M22 ] * val[ M31 ] ) )
                    + ( val[ M13 ] * val[ M21 ] * val[ M32 ] ) )
                  - ( val[ M11 ] * val[ M23 ] * val[ M32 ] )
                  - ( val[ M12 ] * val[ M21 ] * val[ M33 ] ) )
                + ( val[ M11 ] * val[ M22 ] * val[ M33 ] );

        var m01 = ( ( ( val[ M03 ] * val[ M22 ] * val[ M31 ] )
                    - ( val[ M02 ] * val[ M23 ] * val[ M31 ] )
                    - ( val[ M03 ] * val[ M21 ] * val[ M32 ] ) )
                  + ( val[ M01 ] * val[ M23 ] * val[ M32 ] )
                  + ( val[ M02 ] * val[ M21 ] * val[ M33 ] ) )
                - ( val[ M01 ] * val[ M22 ] * val[ M33 ] );

        var m02 = ( ( ( ( val[ M02 ] * val[ M13 ] * val[ M31 ] )
                      - ( val[ M03 ] * val[ M12 ] * val[ M31 ] ) )
                    + ( val[ M03 ] * val[ M11 ] * val[ M32 ] ) )
                  - ( val[ M01 ] * val[ M13 ] * val[ M32 ] )
                  - ( val[ M02 ] * val[ M11 ] * val[ M33 ] ) )
                + ( val[ M01 ] * val[ M12 ] * val[ M33 ] );

        var m03 = ( ( ( val[ M03 ] * val[ M12 ] * val[ M21 ] )
                    - ( val[ M02 ] * val[ M13 ] * val[ M21 ] )
                    - ( val[ M03 ] * val[ M11 ] * val[ M22 ] ) )
                  + ( val[ M01 ] * val[ M13 ] * val[ M22 ] )
                  + ( val[ M02 ] * val[ M11 ] * val[ M23 ] ) )
                - ( val[ M01 ] * val[ M12 ] * val[ M23 ] );

        var m10 = ( ( ( val[ M13 ] * val[ M22 ] * val[ M30 ] )
                    - ( val[ M12 ] * val[ M23 ] * val[ M30 ] )
                    - ( val[ M13 ] * val[ M20 ] * val[ M32 ] ) )
                  + ( val[ M10 ] * val[ M23 ] * val[ M32 ] )
                  + ( val[ M12 ] * val[ M20 ] * val[ M33 ] ) )
                - ( val[ M10 ] * val[ M22 ] * val[ M33 ] );

        var m11 = ( ( ( ( val[ M02 ] * val[ M23 ] * val[ M30 ] )
                      - ( val[ M03 ] * val[ M22 ] * val[ M30 ] ) )
                    + ( val[ M03 ] * val[ M20 ] * val[ M32 ] ) )
                  - ( val[ M00 ] * val[ M23 ] * val[ M32 ] )
                  - ( val[ M02 ] * val[ M20 ] * val[ M33 ] ) )
                + ( val[ M00 ] * val[ M22 ] * val[ M33 ] );

        var m12 = ( ( ( val[ M03 ] * val[ M12 ] * val[ M30 ] )
                    - ( val[ M02 ] * val[ M13 ] * val[ M30 ] )
                    - ( val[ M03 ] * val[ M10 ] * val[ M32 ] ) )
                  + ( val[ M00 ] * val[ M13 ] * val[ M32 ] )
                  + ( val[ M02 ] * val[ M10 ] * val[ M33 ] ) )
                - ( val[ M00 ] * val[ M12 ] * val[ M33 ] );

        var m13 = ( ( ( ( val[ M02 ] * val[ M13 ] * val[ M20 ] )
                      - ( val[ M03 ] * val[ M12 ] * val[ M20 ] ) )
                    + ( val[ M03 ] * val[ M10 ] * val[ M22 ] ) )
                  - ( val[ M00 ] * val[ M13 ] * val[ M22 ] )
                  - ( val[ M02 ] * val[ M10 ] * val[ M23 ] ) )
                + ( val[ M00 ] * val[ M12 ] * val[ M23 ] );

        var m20 = ( ( ( ( val[ M11 ] * val[ M23 ] * val[ M30 ] )
                      - ( val[ M13 ] * val[ M21 ] * val[ M30 ] ) )
                    + ( val[ M13 ] * val[ M20 ] * val[ M31 ] ) )
                  - ( val[ M10 ] * val[ M23 ] * val[ M31 ] )
                  - ( val[ M11 ] * val[ M20 ] * val[ M33 ] ) )
                + ( val[ M10 ] * val[ M21 ] * val[ M33 ] );

        var m21 = ( ( ( val[ M03 ] * val[ M21 ] * val[ M30 ] )
                    - ( val[ M01 ] * val[ M23 ] * val[ M30 ] )
                    - ( val[ M03 ] * val[ M20 ] * val[ M31 ] ) )
                  + ( val[ M00 ] * val[ M23 ] * val[ M31 ] )
                  + ( val[ M01 ] * val[ M20 ] * val[ M33 ] ) )
                - ( val[ M00 ] * val[ M21 ] * val[ M33 ] );

        var m22 = ( ( ( ( val[ M01 ] * val[ M13 ] * val[ M30 ] )
                      - ( val[ M03 ] * val[ M11 ] * val[ M30 ] ) )
                    + ( val[ M03 ] * val[ M10 ] * val[ M31 ] ) )
                  - ( val[ M00 ] * val[ M13 ] * val[ M31 ] )
                  - ( val[ M01 ] * val[ M10 ] * val[ M33 ] ) )
                + ( val[ M00 ] * val[ M11 ] * val[ M33 ] );

        var m23 = ( ( ( val[ M03 ] * val[ M11 ] * val[ M20 ] )
                    - ( val[ M01 ] * val[ M13 ] * val[ M20 ] )
                    - ( val[ M03 ] * val[ M10 ] * val[ M21 ] ) )
                  + ( val[ M00 ] * val[ M13 ] * val[ M21 ] )
                  + ( val[ M01 ] * val[ M10 ] * val[ M23 ] ) )
                - ( val[ M00 ] * val[ M11 ] * val[ M23 ] );

        var m30 = ( ( ( val[ M12 ] * val[ M21 ] * val[ M30 ] )
                    - ( val[ M11 ] * val[ M22 ] * val[ M30 ] )
                    - ( val[ M12 ] * val[ M20 ] * val[ M31 ] ) )
                  + ( val[ M10 ] * val[ M22 ] * val[ M31 ] )
                  + ( val[ M11 ] * val[ M20 ] * val[ M32 ] ) )
                - ( val[ M10 ] * val[ M21 ] * val[ M32 ] );

        var m31 = ( ( ( ( val[ M01 ] * val[ M22 ] * val[ M30 ] )
                      - ( val[ M02 ] * val[ M21 ] * val[ M30 ] ) )
                    + ( val[ M02 ] * val[ M20 ] * val[ M31 ] ) )
                  - ( val[ M00 ] * val[ M22 ] * val[ M31 ] )
                  - ( val[ M01 ] * val[ M20 ] * val[ M32 ] ) )
                + ( val[ M00 ] * val[ M21 ] * val[ M32 ] );

        var m32 = ( ( ( val[ M02 ] * val[ M11 ] * val[ M30 ] )
                    - ( val[ M01 ] * val[ M12 ] * val[ M30 ] )
                    - ( val[ M02 ] * val[ M10 ] * val[ M31 ] ) )
                  + ( val[ M00 ] * val[ M12 ] * val[ M31 ] )
                  + ( val[ M01 ] * val[ M10 ] * val[ M32 ] ) )
                - ( val[ M00 ] * val[ M11 ] * val[ M32 ] );

        var m33 = ( ( ( ( val[ M01 ] * val[ M12 ] * val[ M20 ] )
                      - ( val[ M02 ] * val[ M11 ] * val[ M20 ] ) )
                    + ( val[ M02 ] * val[ M10 ] * val[ M21 ] ) )
                  - ( val[ M00 ] * val[ M12 ] * val[ M21 ] )
                  - ( val[ M01 ] * val[ M10 ] * val[ M22 ] ) )
                + ( val[ M00 ] * val[ M11 ] * val[ M22 ] );

        var invDet = 1.0f / lDet;

        val[ M00 ] = m00 * invDet;
        val[ M10 ] = m10 * invDet;
        val[ M20 ] = m20 * invDet;
        val[ M30 ] = m30 * invDet;
        val[ M01 ] = m01 * invDet;
        val[ M11 ] = m11 * invDet;
        val[ M21 ] = m21 * invDet;
        val[ M31 ] = m31 * invDet;
        val[ M02 ] = m02 * invDet;
        val[ M12 ] = m12 * invDet;
        val[ M22 ] = m22 * invDet;
        val[ M32 ] = m32 * invDet;
        val[ M03 ] = m03 * invDet;
        val[ M13 ] = m13 * invDet;
        val[ M23 ] = m23 * invDet;
        val[ M33 ] = m33 * invDet;

        return this;
    }

    public float Det() => ( ( ( ( ( ( ( ( ( ( ( ( val[ M30 ] * val[ M21 ] * val[ M12 ] * val[ M03 ] )
                                              - ( val[ M20 ] * val[ M31 ] * val[ M12 ] * val[ M03 ] )
                                              - ( val[ M30 ]
                                                * val[ M11 ]
                                                * val[ M22 ]
                                                * val[ M03 ] ) )
                                            + ( val[ M10 ] * val[ M31 ] * val[ M22 ] * val[ M03 ] )
                                            + ( val[ M20 ] * val[ M11 ] * val[ M32 ] * val[ M03 ] ) )
                                          - ( val[ M10 ]
                                            * val[ M21 ]
                                            * val[ M32 ]
                                            * val[ M03 ] )
                                          - ( val[ M30 ] * val[ M21 ] * val[ M02 ] * val[ M13 ] ) )
                                        + ( val[ M20 ] * val[ M31 ] * val[ M02 ] * val[ M13 ] )
                                        + ( val[ M30 ] * val[ M01 ] * val[ M22 ] * val[ M13 ] ) )
                                      - ( val[ M00 ] * val[ M31 ] * val[ M22 ] * val[ M13 ] )
                                      - ( val[ M20 ]
                                        * val[ M01 ]
                                        * val[ M32 ]
                                        * val[ M13 ] ) )
                                    + ( val[ M00 ] * val[ M21 ] * val[ M32 ] * val[ M13 ] )
                                    + ( val[ M30 ] * val[ M11 ] * val[ M02 ] * val[ M23 ] ) )
                                  - ( val[ M10 ]
                                    * val[ M31 ]
                                    * val[ M02 ]
                                    * val[ M23 ] )
                                  - ( val[ M30 ] * val[ M01 ] * val[ M12 ] * val[ M23 ] ) )
                                + ( val[ M00 ] * val[ M31 ] * val[ M12 ] * val[ M23 ] )
                                + ( val[ M10 ]
                                  * val[ M01 ]
                                  * val[ M32 ]
                                  * val[ M23 ] ) )
                              - ( val[ M00 ] * val[ M11 ] * val[ M32 ] * val[ M23 ] )
                              - ( val[ M20 ] * val[ M11 ] * val[ M02 ] * val[ M33 ] ) )
                            + ( val[ M10 ] * val[ M21 ] * val[ M02 ] * val[ M33 ] )
                            + ( val[ M20 ] * val[ M01 ] * val[ M12 ] * val[ M33 ] ) )
                          - ( val[ M00 ]
                            * val[ M21 ]
                            * val[ M12 ]
                            * val[ M33 ] )
                          - ( val[ M10 ] * val[ M01 ] * val[ M22 ] * val[ M33 ] ) )
                        + ( val[ M00 ] * val[ M11 ] * val[ M22 ] * val[ M33 ] );

    /// <returns> The determinant of the 3x3 upper left matrix </returns>
    public float Det3X3() => ( ( val[ M00 ] * val[ M11 ] * val[ M22 ] )
                             + ( val[ M01 ] * val[ M12 ] * val[ M20 ] )
                             + ( val[ M02 ] * val[ M10 ] * val[ M21 ] ) )
                           - ( val[ M00 ]
                             * val[ M12 ]
                             * val[ M21 ] )
                           - ( val[ M01 ] * val[ M10 ] * val[ M22 ] )
                           - ( val[ M02 ] * val[ M11 ] * val[ M20 ] );

    /// <summary>
    ///     Sets the matrix to a projection matrix with a near- and far plane, a field
    ///     of view in degrees and an aspect ratio. Note that the field of view specified
    ///     is the angle in degrees for the height, the field of view for the width will
    ///     be calculated according to the aspect ratio.
    /// </summary>
    /// <param name="near"> The near plane </param>
    /// <param name="far"> The far plane </param>
    /// <param name="fovy"> The field of view of the height in degrees </param>
    /// <param name="aspectRatio"> The "width over height" aspect ratio </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToProjection( float near, float far, float fovy, float aspectRatio )
    {
        Idt();

        var lFd = ( float )( 1.0 / Math.Tan( ( fovy * ( Math.PI / 180 ) ) / 2.0 ) );
        var lA1 = ( far + near ) / ( near - far );
        var lA2 = ( 2 * far * near ) / ( near - far );

        val[ M00 ] = lFd / aspectRatio;
        val[ M10 ] = 0;
        val[ M20 ] = 0;
        val[ M30 ] = 0;
        val[ M01 ] = 0;
        val[ M11 ] = lFd;
        val[ M21 ] = 0;
        val[ M31 ] = 0;
        val[ M02 ] = 0;
        val[ M12 ] = 0;
        val[ M22 ] = lA1;
        val[ M32 ] = -1;
        val[ M03 ] = 0;
        val[ M13 ] = 0;
        val[ M23 ] = lA2;
        val[ M33 ] = 0;

        return this;
    }

    /// <summary>
    ///     Sets the matrix to a projection matrix with a near/far plane, and left, bottom,
    ///     right and top specifying the points on the near plane that are mapped to the lower
    ///     left and upper right corners of the viewport. This allows to create projection
    ///     matrix with off-center vanishing point.
    /// </summary>
    /// <param name="left"> </param>
    /// <param name="right"> </param>
    /// <param name="bottom"> </param>
    /// <param name="top"> </param>
    /// <param name="near"> The near plane </param>
    /// <param name="far"> The far plane </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToProjection( float left, float right, float bottom, float top, float near, float far )
    {
        var x   = ( 2.0f * near ) / ( right - left );
        var y   = ( 2.0f * near ) / ( top - bottom );
        var a   = ( right + left ) / ( right - left );
        var b   = ( top + bottom ) / ( top - bottom );
        var lA1 = ( far + near ) / ( near - far );
        var lA2 = ( 2 * far * near ) / ( near - far );

        val[ M00 ] = x;
        val[ M10 ] = 0;
        val[ M20 ] = 0;
        val[ M30 ] = 0;
        val[ M01 ] = 0;
        val[ M11 ] = y;
        val[ M21 ] = 0;
        val[ M31 ] = 0;
        val[ M02 ] = a;
        val[ M12 ] = b;
        val[ M22 ] = lA1;
        val[ M32 ] = -1;
        val[ M03 ] = 0;
        val[ M13 ] = 0;
        val[ M23 ] = lA2;
        val[ M33 ] = 0;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to an orthographic projection matrix with the origin at (x,y) extending by width and height. The
    ///     near plane
    ///     is Set to 0, the far plane is Set to 1.
    /// </summary>
    /// <param name="x"> The x-coordinate of the origin </param>
    /// <param name="y"> The y-coordinate of the origin </param>
    /// <param name="width"> The width </param>
    /// <param name="height"> The height </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToOrtho2D( float x, float y, float width, float height )
    {
        SetToOrtho( x, x + width, y, y + height, 0, 1 );

        return this;
    }

    /// <summary>
    ///     Sets this matrix to an orthographic projection matrix with the origin at (x,y) extending by width and height,
    ///     having a near
    ///     and far plane.
    /// </summary>
    /// <param name="x"> The x-coordinate of the origin </param>
    /// <param name="y"> The y-coordinate of the origin </param>
    /// <param name="width"> The width </param>
    /// <param name="height"> The height </param>
    /// <param name="near"> The near plane </param>
    /// <param name="far"> The far plane </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToOrtho2D( float x, float y, float width, float height, float near, float far )
    {
        SetToOrtho( x, x + width, y, y + height, near, far );

        return this;
    }

    /// <summary>
    ///     Sets the matrix to an orthographic projection like glOrtho (http://www.opengl.org/sdk/docs/man/xhtml/glOrtho.xml)
    ///     following
    ///     the OpenGL equivalent
    /// </summary>
    /// <param name="left"> The left clipping plane </param>
    /// <param name="right"> The right clipping plane </param>
    /// <param name="bottom"> The bottom clipping plane </param>
    /// <param name="top"> The top clipping plane </param>
    /// <param name="near"> The near clipping plane </param>
    /// <param name="far"> The far clipping plane </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToOrtho( float left, float right, float bottom, float top, float near, float far )
    {
        var xOrth = 2 / ( right - left );
        var yOrth = 2 / ( top - bottom );
        var zOrth = -2 / ( far - near );

        var tx = -( right + left ) / ( right - left );
        var ty = -( top + bottom ) / ( top - bottom );
        var tz = -( far + near ) / ( far - near );

        val[ M00 ] = xOrth;
        val[ M10 ] = 0;
        val[ M20 ] = 0;
        val[ M30 ] = 0;
        val[ M01 ] = 0;
        val[ M11 ] = yOrth;
        val[ M21 ] = 0;
        val[ M31 ] = 0;
        val[ M02 ] = 0;
        val[ M12 ] = 0;
        val[ M22 ] = zOrth;
        val[ M32 ] = 0;
        val[ M03 ] = tx;
        val[ M13 ] = ty;
        val[ M23 ] = tz;
        val[ M33 ] = 1;

        return this;
    }

    /// <summary>
    ///     Sets the 4th column to the translation vector.
    /// </summary>
    /// <param name="vector"> The translation vector </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetTranslation( Vector3 vector )
    {
        val[ M03 ] = vector.X;
        val[ M13 ] = vector.Y;
        val[ M23 ] = vector.Z;

        return this;
    }

    /// <summary>
    ///     Sets the 4th column to the translation vector.
    /// </summary>
    /// <param name="x"> The X coordinate of the translation vector </param>
    /// <param name="y"> The Y coordinate of the translation vector </param>
    /// <param name="z"> The Z coordinate of the translation vector </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetTranslation( float x, float y, float z )
    {
        val[ M03 ] = x;
        val[ M13 ] = y;
        val[ M23 ] = z;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a translation matrix, overwriting it first by an identity matrix and then setting the 4th
    ///     column to the
    ///     translation vector.
    /// </summary>
    /// <param name="vector"> The translation vector </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToTranslation( Vector3 vector )
    {
        Idt();

        val[ M03 ] = vector.X;
        val[ M13 ] = vector.Y;
        val[ M23 ] = vector.Z;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a translation matrix, overwriting it first by an identity matrix and then setting the 4th
    ///     column to the
    ///     translation vector.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <param name="z"> The z-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToTranslation( float x, float y, float z )
    {
        Idt();

        val[ M03 ] = x;
        val[ M13 ] = y;
        val[ M23 ] = z;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a translation and scaling matrix by first overwriting it with an identity and then setting the
    ///     translation vector in the 4th column and the scaling vector in the diagonal.
    /// </summary>
    /// <param name="translation"> The translation vector </param>
    /// <param name="scaling"> The scaling vector </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToTranslationAndScaling( Vector3 translation, Vector3 scaling )
    {
        Idt();

        val[ M03 ] = translation.X;
        val[ M13 ] = translation.Y;
        val[ M23 ] = translation.Z;
        val[ M00 ] = scaling.X;
        val[ M11 ] = scaling.Y;
        val[ M22 ] = scaling.Z;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a translation and scaling matrix by first overwriting it with an identity and then setting the
    ///     translation vector in the 4th column and the scaling vector in the diagonal.
    /// </summary>
    /// <param name="translationX"> The x-component of the translation vector </param>
    /// <param name="translationY"> The y-component of the translation vector </param>
    /// <param name="translationZ"> The z-component of the translation vector </param>
    /// <param name="scalingX"> The x-component of the scaling vector </param>
    /// <param name="scalingY"> The x-component of the scaling vector </param>
    /// <param name="scalingZ"> The x-component of the scaling vector </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToTranslationAndScaling( float translationX,
                                               float translationY,
                                               float translationZ,
                                               float scalingX,
                                               float scalingY,
                                               float scalingZ )
    {
        Idt();

        val[ M03 ] = translationX;
        val[ M13 ] = translationY;
        val[ M23 ] = translationZ;
        val[ M00 ] = scalingX;
        val[ M11 ] = scalingY;
        val[ M22 ] = scalingZ;

        return this;
    }

    /// <summary>
    ///     Sets the matrix to a rotation matrix around the given axis.
    /// </summary>
    /// <param name="axis"> The axis </param>
    /// <param name="degrees"> The angle in degrees </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToRotation( Vector3 axis, float degrees )
    {
        if ( degrees == 0 )
        {
            Idt();

            return this;
        }

        return Set( Quat.Set( axis, degrees ) );
    }

    /// <summary>
    ///     Sets the matrix to a rotation matrix around the given axis.
    /// </summary>
    /// <param name="axis"> The axis </param>
    /// <param name="radians"> The angle in radians </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToRotationRad( Vector3 axis, float radians )
    {
        if ( radians == 0 )
        {
            Idt();

            return this;
        }

        return Set( Quat.SetFromAxisRad( axis, radians ) );
    }

    /// <summary>
    ///     Sets the matrix to a rotation matrix around the given axis.
    /// </summary>
    /// <param name="axisX"> The x-component of the axis </param>
    /// <param name="axisY"> The y-component of the axis </param>
    /// <param name="axisZ"> The z-component of the axis </param>
    /// <param name="degrees"> The angle in degrees </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToRotation( float axisX, float axisY, float axisZ, float degrees )
    {
        if ( degrees == 0 )
        {
            Idt();

            return this;
        }

        return Set( Quat.SetFromAxis( axisX, axisY, axisZ, degrees ) );
    }

    /// <summary>
    ///     Sets the matrix to a rotation matrix around the given axis.
    /// </summary>
    /// <param name="axisX"> The x-component of the axis </param>
    /// <param name="axisY"> The y-component of the axis </param>
    /// <param name="axisZ"> The z-component of the axis </param>
    /// <param name="radians"> The angle in radians </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToRotationRad( float axisX, float axisY, float axisZ, float radians )
    {
        if ( radians == 0 )
        {
            Idt();

            return this;
        }

        return Set( Quat.SetFromAxisRad( axisX, axisY, axisZ, radians ) );
    }

    /// <summary>
    ///     Set the matrix to a rotation matrix between two vectors.
    /// </summary>
    /// <param name="v1"> The base vector </param>
    /// <param name="v2"> The target vector </param>
    /// <returns> This matrix for the purpose of chaining methods together  </returns>
    public Matrix4 SetToRotation( Vector3 v1, Vector3 v2 ) => Set( Quat.SetFromCross( v1, v2 ) );

    /// <summary>
    ///     Set the matrix to a rotation matrix between two vectors.
    /// </summary>
    /// <param name="x1"> The base vectors x value </param>
    /// <param name="y1"> The base vectors y value </param>
    /// <param name="z1"> The base vectors z value </param>
    /// <param name="x2"> The target vector x value </param>
    /// <param name="y2"> The target vector y value </param>
    /// <param name="z2"> The target vector z value </param>
    /// <returns> This matrix for the purpose of chaining methods together  </returns>
    public Matrix4 SetToRotation( float x1, float y1, float z1, float x2, float y2, float z2 ) => Set( Quat.SetFromCross( x1, y1, z1, x2, y2, z2 ) );

    /// <summary>
    ///     Sets this matrix to a rotation matrix from the given euler angles.
    /// </summary>
    /// <param name="yaw"> the yaw in degrees </param>
    /// <param name="pitch"> the pitch in degrees </param>
    /// <param name="roll"> the roll in degrees </param>
    /// <returns> This matrix  </returns>
    public Matrix4 SetFromEulerAngles( float yaw, float pitch, float roll )
    {
        Quat.SetEulerAngles( yaw, pitch, roll );

        return Set( Quat );
    }

    /// <summary>
    ///     Sets this matrix to a rotation matrix from the given euler angles.
    /// </summary>
    /// <param name="yaw"> the yaw in radians </param>
    /// <param name="pitch"> the pitch in radians </param>
    /// <param name="roll"> the roll in radians </param>
    /// <returns> This matrix  </returns>
    public Matrix4 SetFromEulerAnglesRad( float yaw, float pitch, float roll )
    {
        Quat.SetEulerAnglesRad( yaw, pitch, roll );

        return Set( Quat );
    }

    /// <summary>
    ///     Sets this matrix to a scaling matrix
    /// </summary>
    /// <param name="vector"> The scaling vector </param>
    /// <returns> This matrix for chaining.  </returns>
    public Matrix4 SetToScaling( Vector3 vector )
    {
        Idt();

        val[ M00 ] = vector.X;
        val[ M11 ] = vector.Y;
        val[ M22 ] = vector.Z;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a scaling matrix
    /// </summary>
    /// <param name="x"> The x-component of the scaling vector </param>
    /// <param name="y"> The y-component of the scaling vector </param>
    /// <param name="z"> The z-component of the scaling vector </param>
    /// <returns> This matrix for chaining.  </returns>
    public Matrix4 SetToScaling( float x, float y, float z )
    {
        Idt();

        val[ M00 ] = x;
        val[ M11 ] = y;
        val[ M22 ] = z;

        return this;
    }


    /// <summary>
    ///     Sets the matrix to a look at matrix with a direction and an up vector. Multiply
    ///     with a translation matrix to get a camera model view matrix.
    /// </summary>
    /// <param name="direction"> The direction vector </param>
    /// <param name="up"> The up vector </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 SetToLookAt( Vector3 direction, Vector3 up )
    {
        LVez.Set( direction ).Nor();
        LVex.Set( direction ).Crs( up ).Nor();
        LVey.Set( LVex ).Crs( LVez ).Nor();

        Idt();

        val[ M00 ] = LVex.X;
        val[ M01 ] = LVex.Y;
        val[ M02 ] = LVex.Z;
        val[ M10 ] = LVey.X;
        val[ M11 ] = LVey.Y;
        val[ M12 ] = LVey.Z;
        val[ M20 ] = -LVez.X;
        val[ M21 ] = -LVez.Y;
        val[ M22 ] = -LVez.Z;

        return this;
    }


    /// <summary>
    ///     Sets this matrix to a look at matrix with the given position, target and up vector.
    /// </summary>
    /// <param name="position"> the position </param>
    /// <param name="target"> the target </param>
    /// <param name="up"> the up vector </param>
    /// <returns> This matrix  </returns>
    public Matrix4 SetToLookAt( Vector3 position, Vector3 target, Vector3 up )
    {
        TmpVec.Set( target ).Sub( position );
        SetToLookAt( TmpVec, up );
        Mul( TmpMat.SetToTranslation( -position.X, -position.Y, -position.Z ) );

        return this;
    }


    public Matrix4 SetToWorld( Vector3 position, Vector3 forward, Vector3 up )
    {
        TmpForward.Set( forward ).Nor();
        Right.Set( TmpForward ).Crs( up ).Nor();
        TmpUp.Set( Right ).Crs( TmpForward ).Nor();
        Set( Right, TmpUp, TmpForward.Scl( -1 ), position );

        return this;
    }

    /// <summary>
    ///     Linearly interpolates between this matrix and the given matrix mixing by alpha
    /// </summary>
    /// <param name="matrix"> the matrix </param>
    /// <param name="alpha"> the alpha value in the range [0,1] </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Lerp( Matrix4 matrix, float alpha )
    {
        for ( var i = 0; i < 16; i++ )
        {
            val[ i ] = ( val[ i ] * ( 1 - alpha ) ) + ( matrix.val[ i ] * alpha );
        }

        return this;
    }

    /// <summary>
    ///     Averages the given transform with this one and stores the result in this matrix. Translations and scales are lerped
    ///     while
    ///     rotations are slerped.
    /// </summary>
    /// <param name="other"> The other transform </param>
    /// <param name="w"> Weight of this transform; weight of the other transform is (1 - w) </param>
    /// <returns> This matrix for chaining  </returns>
    public Matrix4 Avg( Matrix4 other, float w )
    {
        GetScale( TmpVec );
        other.GetScale( TmpForward );

        GetRotation( Quat );
        other.GetRotation( Quat2 );

        GetTranslation( TmpUp );
        other.GetTranslation( Right );

        SetToScaling( TmpVec.Scl( w ).Add( TmpForward.Scl( 1 - w ) ) );
        Rotate( Quat.Slerp( Quat2, 1 - w ) );
        SetTranslation( TmpUp.Scl( w ).Add( Right.Scl( 1 - w ) ) );

        return this;
    }

    /// <summary>
    ///     Averages the given transforms and stores the result in this matrix. Translations
    ///     and scales are lerped while rotations are slerped. Does not destroy the data
    ///     contained in t.
    /// </summary>
    /// <param name="t"> List of transforms </param>
    /// <returns> This matrix for chaining  </returns>
    public Matrix4 Avg( Matrix4[] t )
    {
        var w = 1.0f / t.Length;

        TmpVec.Set( t[ 0 ].GetScale( TmpUp ).Scl( w ) );
        Quat.Set( t[ 0 ].GetRotation( Quat2 ).Exp( w ) );
        TmpForward.Set( t[ 0 ].GetTranslation( TmpUp ).Scl( w ) );

        for ( var i = 1; i < t.Length; i++ )
        {
            TmpVec.Add( t[ i ].GetScale( TmpUp ).Scl( w ) );
            Quat.Mul( t[ i ].GetRotation( Quat2 ).Exp( w ) );
            TmpForward.Add( t[ i ].GetTranslation( TmpUp ).Scl( w ) );
        }

        Quat.Nor();

        SetToScaling( TmpVec );
        Rotate( Quat );
        SetTranslation( TmpForward );

        return this;
    }

    /// <summary>
    ///     Averages the given transforms with the given weights and stores the result
    ///     in this matrix. Translations and scales are lerped while rotations are slerped.
    ///     Does not destroy the data contained in t or w; Sum of w_i must be equal to 1, or
    ///     unexpected results will occur.
    /// </summary>
    /// <param name="t"> List of transforms </param>
    /// <param name="w"> List of weights </param>
    /// <returns> This matrix for chaining  </returns>
    public Matrix4 Avg( Matrix4[] t, float[] w )
    {
        TmpVec.Set( t[ 0 ].GetScale( TmpUp ).Scl( w[ 0 ] ) );
        Quat.Set( t[ 0 ].GetRotation( Quat2 ).Exp( w[ 0 ] ) );
        TmpForward.Set( t[ 0 ].GetTranslation( TmpUp ).Scl( w[ 0 ] ) );

        for ( var i = 1; i < t.Length; i++ )
        {
            TmpVec.Add( t[ i ].GetScale( TmpUp ).Scl( w[ i ] ) );
            Quat.Mul( t[ i ].GetRotation( Quat2 ).Exp( w[ i ] ) );
            TmpForward.Add( t[ i ].GetTranslation( TmpUp ).Scl( w[ i ] ) );
        }

        Quat.Nor();

        SetToScaling( TmpVec );
        Rotate( Quat );
        SetTranslation( TmpForward );

        return this;
    }

    /// <summary>
    ///     Sets this matrix to the given 3x3 matrix.
    ///     The third column of this matrix is Set to ( 0, 0, 1, 0 ).
    /// </summary>
    /// <param name="mat"> the matrix </param>
    public Matrix4 Set( Matrix3 mat )
    {
        val[ 0 ]  = mat.val[ 0 ];
        val[ 1 ]  = mat.val[ 1 ];
        val[ 2 ]  = mat.val[ 2 ];
        val[ 3 ]  = 0;
        val[ 4 ]  = mat.val[ 3 ];
        val[ 5 ]  = mat.val[ 4 ];
        val[ 6 ]  = mat.val[ 5 ];
        val[ 7 ]  = 0;
        val[ 8 ]  = 0;
        val[ 9 ]  = 0;
        val[ 10 ] = 1;
        val[ 11 ] = 0;
        val[ 12 ] = mat.val[ 6 ];
        val[ 13 ] = mat.val[ 7 ];
        val[ 14 ] = 0;
        val[ 15 ] = mat.val[ 8 ];

        return this;
    }

    /// <summary>
    ///     Sets this matrix to the given affine matrix. The values are mapped as follows:
    ///     <para></para>
    ///     <para>     [  M00  M01  ___  M02  ]</para>
    ///     <para>     [  M10  M11  ___  M12  ]</para>
    ///     <para>     [  ___  ___  ___  ___  ]</para>
    ///     <para>     [  ___  ___  ___  ___  ]</para>
    ///     <para></para>
    /// </summary>
    /// <param name="affine"> the source matrix </param>
    /// <returns> This matrix for chaining </returns>
    public Matrix4 Set( Affine2 affine )
    {
        val[ M00 ] = affine.m00;
        val[ M10 ] = affine.m10;
        val[ M20 ] = 0;
        val[ M30 ] = 0;
        val[ M01 ] = affine.m01;
        val[ M11 ] = affine.m11;
        val[ M21 ] = 0;
        val[ M31 ] = 0;
        val[ M02 ] = 0;
        val[ M12 ] = 0;
        val[ M22 ] = 1;
        val[ M32 ] = 0;
        val[ M03 ] = affine.m02;
        val[ M13 ] = affine.m12;
        val[ M23 ] = 0;
        val[ M33 ] = 1;

        return this;
    }

    /// <summary>
    ///     Assumes that both matrices are 2D affine transformations, copying only
    ///     the relevant components. The copied are mapped as follows:
    ///     <para></para>
    ///     <para>     [  M00  M01  ___  M02  ]</para>
    ///     <para>     [  M10  M11  ___  M12  ]</para>
    ///     <para>     [  ___  ___  ___  ___  ]</para>
    ///     <para>     [  ___  ___  ___  ___  ]</para>
    ///     <para></para>
    /// </summary>
    /// <param name="affine"> the source matrix </param>
    /// <returns> This matrix for chaining </returns>
    public Matrix4 SetAsAffine( Affine2 affine )
    {
        val[ M00 ] = affine.m00;
        val[ M10 ] = affine.m10;
        val[ M01 ] = affine.m01;
        val[ M11 ] = affine.m11;
        val[ M03 ] = affine.m02;
        val[ M13 ] = affine.m12;

        return this;
    }

    /// <summary>
    ///     Assumes that both matrices are 2D affine transformations, copying only
    ///     the relevant components. The copied values are:
    ///     <para></para>
    ///     <para>     [  M00  M01  ___  M03  ]</para>
    ///     <para>     [  M10  M11  ___  M13  ]</para>
    ///     <para>     [  ___  ___  ___  ___  ]</para>
    ///     <para>     [  ___  ___  ___  ___  ]</para>
    ///     <para></para>
    /// </summary>
    /// <param name="mat"> the source matrix </param>
    /// <returns> This matrix for chaining </returns>
    public Matrix4 SetAsAffine( Matrix4 mat )
    {
        val[ M00 ] = mat.val[ M00 ];
        val[ M10 ] = mat.val[ M10 ];
        val[ M01 ] = mat.val[ M01 ];
        val[ M11 ] = mat.val[ M11 ];
        val[ M03 ] = mat.val[ M03 ];
        val[ M13 ] = mat.val[ M13 ];

        return this;
    }

    public Matrix4 Scl( Vector3 scale )
    {
        val[ M00 ] *= scale.X;
        val[ M11 ] *= scale.Y;
        val[ M22 ] *= scale.Z;

        return this;
    }

    public Matrix4 Scl( float x, float y, float z )
    {
        val[ M00 ] *= x;
        val[ M11 ] *= y;
        val[ M22 ] *= z;

        return this;
    }

    public Matrix4 Scl( float scale )
    {
        val[ M00 ] *= scale;
        val[ M11 ] *= scale;
        val[ M22 ] *= scale;

        return this;
    }

    public Vector3 GetTranslation( Vector3 position )
    {
        position.X = val[ M03 ];
        position.Y = val[ M13 ];
        position.Z = val[ M23 ];

        return position;
    }

    /// <summary>
    ///     Gets the rotation of this matrix.
    /// </summary>
    /// <param name="rotation"> The <see cref="Quaternion" /> to receive the rotation </param>
    /// <param name="normalizeAxes"> True to normalize the axes, necessary when the matrix might also include scaling. </param>
    /// <returns> The provided <see cref="Quaternion" /> for chaining.  </returns>
    public Quaternion GetRotation( Quaternion rotation, bool normalizeAxes ) => rotation.SetFromMatrix( normalizeAxes, this );

    /// <summary>
    ///     Gets the rotation of this matrix.
    /// </summary>
    /// <param name="rotation"> The <see cref="Quaternion" /> to receive the rotation </param>
    /// <returns> The provided <see cref="Quaternion" /> for chaining.  </returns>
    public Quaternion GetRotation( Quaternion rotation ) => rotation.SetFromMatrix( this );

    /// <summary>
    /// </summary>
    /// <returns> the squared scale factor on the X axis </returns>
    public float GetScaleXSquared() => ( val[ M00 ] * val[ M00 ] ) + ( val[ M01 ] * val[ M01 ] ) + ( val[ M02 ] * val[ M02 ] );

    /// <summary>
    /// </summary>
    /// <returns> the squared scale factor on the Y axis </returns>
    public float GetScaleYSquared() => ( val[ M10 ] * val[ M10 ] ) + ( val[ M11 ] * val[ M11 ] ) + ( val[ M12 ] * val[ M12 ] );

    /// <summary>
    /// </summary>
    /// <returns> the squared scale factor on the Z axis </returns>
    public float GetScaleZSquared() => ( val[ M20 ] * val[ M20 ] ) + ( val[ M21 ] * val[ M21 ] ) + ( val[ M22 ] * val[ M22 ] );

    /// <summary>
    /// </summary>
    /// <returns> the scale factor on the X axis (non-negative) </returns>
    public float GetScaleX() => MathUtils.IsZero( val[ M01 ] ) && MathUtils.IsZero( val[ M02 ] )
        ? Math.Abs( val[ M00 ] )
        : ( float )Math.Sqrt( GetScaleXSquared() );

    /// <summary>
    /// </summary>
    /// <returns> the scale factor on the Y axis (non-negative) </returns>
    public float GetScaleY() => MathUtils.IsZero( val[ M10 ] ) && MathUtils.IsZero( val[ M12 ] )
        ? Math.Abs( val[ M11 ] )
        : ( float )Math.Sqrt( GetScaleYSquared() );

    /// <summary>
    /// </summary>
    /// <returns> the scale factor on the Z axis (non-negative) </returns>
    public float GetScaleZ() => MathUtils.IsZero( val[ M20 ] ) && MathUtils.IsZero( val[ M21 ] )
        ? Math.Abs( val[ M22 ] )
        : ( float )Math.Sqrt( GetScaleZSquared() );

    /// <summary>
    ///     <param name="scale"> The vector which will receive the (non-negative) scale components on each axis. </param>
    ///     <returns> The provided vector for chaining.  </returns>
    /// </summary>
    public Vector3 GetScale( Vector3 scale ) => scale.Set( GetScaleX(), GetScaleY(), GetScaleZ() );

    /// <summary>
    ///     removes the translational part and transposes the matrix.
    /// </summary>
    public Matrix4 ToNormalMatrix()
    {
        val[ M03 ] = 0;
        val[ M13 ] = 0;
        val[ M23 ] = 0;

        return Inv().Tra();
    }

    public new string ToString() => $"[{val[ M00 ]}|{val[ M01 ]}|{val[ M02 ]}|{val[ M03 ]}]\n[{val[ M10 ]}"
                                  + $"|{val[ M11 ]}|{val[ M12 ]}|{val[ M13 ]}]\n[{val[ M20 ]}|{val[ M21 ]}"
                                  + $"|{val[ M22 ]}|{val[ M23 ]}]\n[{val[ M30 ]}|{val[ M31 ]}|{val[ M32 ]}"
                                  + $"|{val[ M33 ]}]\n";

    /// <summary>
    ///     Multiplies the vectors with the given matrix. The matrix array is assumed to
    ///     hold a 4x4 column major matrix as you can get from <see cref="Matrix4.val" />.
    ///     The vectors array is assumed to hold 3-component vectors. Offset specifies
    ///     the offset into the array where the x-component of the first vector is located.
    ///     The numVecs parameter specifies the number of vectors stored in the vectors
    ///     array. The stride parameter specifies the number of floats between subsequent
    ///     vectors and must be >= 3. This is the same as <see cref="Vector3.Mul(Matrix4)" />
    ///     applied to multiple vectors.
    /// </summary>
    /// <param name="mat"> the matrix </param>
    /// <param name="vecs"> the vectors </param>
    /// <param name="offset"> the offset into the vectors array </param>
    /// <param name="numVecs"> the number of vectors </param>
    /// <param name="stride"> the stride between vectors in floats  </param>
    public static void MulVec( float[] mat, float[] vecs, int offset, int numVecs, int stride )
    {
        var vecPtr = vecs.Skip( offset ).ToArray();

        for ( var i = 0; i < numVecs; i++ )
        {
            MulVec( mat, vecPtr );
            vecPtr = vecPtr.Skip( stride ).ToArray();
        }
    }

    /*-{ }-*/ /*
        float* vecPtr = vecs + offset;
        for(int i = 0; i < numVecs; i++) {
            matrix4_mulVec(mat, vecPtr);
            vecPtr += stride;
        }
    */

    /// <summary>
    ///     Multiplies the vectors with the given matrix, performing a division by w.
    ///     The matrix array is assumed to hold a 4x4 column major matrix as you can
    ///     get from <see cref="Matrix4.val" />. The vectors array is assumed to hold
    ///     3-component vectors. Offset specifies the offset into the array where the
    ///     x-component of the first vector is located. The numVecs parameter specifies
    ///     the number of vectors stored in the vectors array. The stride parameter
    ///     specifies the number of floats between subsequent vectors and must be >= 3.
    ///     This is the same as <see cref="Vector3.Prj(Matrix4)" /> applied to multiple
    ///     vectors.
    /// </summary>
    /// <param name="mat"> the matrix </param>
    /// <param name="vecs"> the vectors </param>
    /// <param name="offset"> the offset into the vectors array </param>
    /// <param name="numVecs"> the number of vectors </param>
    /// <param name="stride"> the stride between vectors in floats  </param>
    public static void Prj( float[] mat, float[] vecs, int offset, int numVecs, int stride )
    {
        var vecPtr = vecs.Skip( offset ).ToArray();

        for ( var i = 0; i < numVecs; i++ )
        {
            Prj( mat, vecPtr );
            vecPtr = vecPtr.Skip( stride ).ToArray();
        }
    }
    /*-{ }-*/ /*
        float* vecPtr = vecs + offset;
        for(int i = 0; i < numVecs; i++) {
            matrix4_proj(mat, vecPtr);
            vecPtr += stride;
        }
    */

    /// <summary>
    ///     Multiplies the vectors with the top most 3x3 sub-matrix of the given matrix.
    ///     The matrix array is assumed to hold a 4x4 column major matrix as you can get
    ///     from <see cref="Matrix4.val" />. The vectors array is assumed to hold
    ///     3-component vectors. Offset specifies the offset into the array where the
    ///     x-component of the first vector is located. The numVecs parameter specifies
    ///     the number of vectors stored in the vectors array. The stride parameter
    ///     specifies the number of floats between subsequent vectors and must be >= 3.
    ///     This is the same as <see cref="Vector3.Rot(Matrix4)" /> applied to multiple vectors.
    /// </summary>
    /// <param name="mat"> the matrix </param>
    /// <param name="vecs"> the vectors </param>
    /// <param name="offset"> the offset into the vectors array </param>
    /// <param name="numVecs"> the number of vectors </param>
    /// <param name="stride"> the stride between vectors in floats  </param>
    public static void Rot( float[] mat, float[] vecs, int offset, int numVecs, int stride )
    {
        var vecPtr = vecs.Skip( offset ).ToArray();

        for ( var i = 0; i < numVecs; i++ )
        {
            Rot( mat, vecPtr );
            vecPtr = vecPtr.Skip( stride ).ToArray();
        }
    }

    /*-{ }-*/ /*
        float* vecPtr = vecs + offset;
        for(int i = 0; i < numVecs; i++) {
            matrix4_rot(mat, vecPtr);
            vecPtr += stride;
        }
    */
    // @on

    /// <summary>
    ///     Multiplies the matrix mata with matrix matb, storing the result in mata.
    ///     The arrays are assumed to hold 4x4 column major matrices as you can get
    ///     from <see cref="Matrix4.val" />.
    ///     This is the same as <see cref="Matrix4.Mul(Matrix4)" />.
    /// </summary>
    /// <param name="mata"> the first matrix. </param>
    /// <param name="matb"> the second matrix.  </param>
    public static void Mul( float[] mata, float[] matb )
    {
        var m00 = ( mata[ M00 ] * matb[ M00 ] )
                + ( mata[ M01 ] * matb[ M10 ] )
                + ( mata[ M02 ] * matb[ M20 ] )
                + ( mata[ M03 ] * matb[ M30 ] );

        var m01 = ( mata[ M00 ] * matb[ M01 ] )
                + ( mata[ M01 ] * matb[ M11 ] )
                + ( mata[ M02 ] * matb[ M21 ] )
                + ( mata[ M03 ] * matb[ M31 ] );

        var m02 = ( mata[ M00 ] * matb[ M02 ] )
                + ( mata[ M01 ] * matb[ M12 ] )
                + ( mata[ M02 ] * matb[ M22 ] )
                + ( mata[ M03 ] * matb[ M32 ] );

        var m03 = ( mata[ M00 ] * matb[ M03 ] )
                + ( mata[ M01 ] * matb[ M13 ] )
                + ( mata[ M02 ] * matb[ M23 ] )
                + ( mata[ M03 ] * matb[ M33 ] );

        var m10 = ( mata[ M10 ] * matb[ M00 ] )
                + ( mata[ M11 ] * matb[ M10 ] )
                + ( mata[ M12 ] * matb[ M20 ] )
                + ( mata[ M13 ] * matb[ M30 ] );

        var m11 = ( mata[ M10 ] * matb[ M01 ] )
                + ( mata[ M11 ] * matb[ M11 ] )
                + ( mata[ M12 ] * matb[ M21 ] )
                + ( mata[ M13 ] * matb[ M31 ] );

        var m12 = ( mata[ M10 ] * matb[ M02 ] )
                + ( mata[ M11 ] * matb[ M12 ] )
                + ( mata[ M12 ] * matb[ M22 ] )
                + ( mata[ M13 ] * matb[ M32 ] );

        var m13 = ( mata[ M10 ] * matb[ M03 ] )
                + ( mata[ M11 ] * matb[ M13 ] )
                + ( mata[ M12 ] * matb[ M23 ] )
                + ( mata[ M13 ] * matb[ M33 ] );

        var m20 = ( mata[ M20 ] * matb[ M00 ] )
                + ( mata[ M21 ] * matb[ M10 ] )
                + ( mata[ M22 ] * matb[ M20 ] )
                + ( mata[ M23 ] * matb[ M30 ] );

        var m21 = ( mata[ M20 ] * matb[ M01 ] )
                + ( mata[ M21 ] * matb[ M11 ] )
                + ( mata[ M22 ] * matb[ M21 ] )
                + ( mata[ M23 ] * matb[ M31 ] );

        var m22 = ( mata[ M20 ] * matb[ M02 ] )
                + ( mata[ M21 ] * matb[ M12 ] )
                + ( mata[ M22 ] * matb[ M22 ] )
                + ( mata[ M23 ] * matb[ M32 ] );

        var m23 = ( mata[ M20 ] * matb[ M03 ] )
                + ( mata[ M21 ] * matb[ M13 ] )
                + ( mata[ M22 ] * matb[ M23 ] )
                + ( mata[ M23 ] * matb[ M33 ] );

        var m30 = ( mata[ M30 ] * matb[ M00 ] )
                + ( mata[ M31 ] * matb[ M10 ] )
                + ( mata[ M32 ] * matb[ M20 ] )
                + ( mata[ M33 ] * matb[ M30 ] );

        var m31 = ( mata[ M30 ] * matb[ M01 ] )
                + ( mata[ M31 ] * matb[ M11 ] )
                + ( mata[ M32 ] * matb[ M21 ] )
                + ( mata[ M33 ] * matb[ M31 ] );

        var m32 = ( mata[ M30 ] * matb[ M02 ] )
                + ( mata[ M31 ] * matb[ M12 ] )
                + ( mata[ M32 ] * matb[ M22 ] )
                + ( mata[ M33 ] * matb[ M32 ] );

        var m33 = ( mata[ M30 ] * matb[ M03 ] )
                + ( mata[ M31 ] * matb[ M13 ] )
                + ( mata[ M32 ] * matb[ M23 ] )
                + ( mata[ M33 ] * matb[ M33 ] );

        mata[ M00 ] = m00;
        mata[ M10 ] = m10;
        mata[ M20 ] = m20;
        mata[ M30 ] = m30;
        mata[ M01 ] = m01;
        mata[ M11 ] = m11;
        mata[ M21 ] = m21;
        mata[ M31 ] = m31;
        mata[ M02 ] = m02;
        mata[ M12 ] = m12;
        mata[ M22 ] = m22;
        mata[ M32 ] = m32;
        mata[ M03 ] = m03;
        mata[ M13 ] = m13;
        mata[ M23 ] = m23;
        mata[ M33 ] = m33;
    }

    /// <summary>
    ///     Multiplies the vector with the given matrix. The matrix array is assumed
    ///     to hold a 4x4 column major matrix as you can get from <see cref="Matrix4.val" />.
    ///     The vector array is assumed to hold a 3-component vector, with x being the
    ///     first element, y being the second and z being the last component. The result
    ///     is stored in the vector array. This is the same as <see cref="Vector3.Mul(Matrix4)" />.
    /// </summary>
    /// <param name="mat"> the matrix </param>
    /// <param name="vec"> the vector.  </param>
    public static void MulVec( float[] mat, float[] vec )
    {
        var x = ( vec[ 0 ] * mat[ M00 ] ) + ( vec[ 1 ] * mat[ M01 ] ) + ( vec[ 2 ] * mat[ M02 ] ) + mat[ M03 ];
        var y = ( vec[ 0 ] * mat[ M10 ] ) + ( vec[ 1 ] * mat[ M11 ] ) + ( vec[ 2 ] * mat[ M12 ] ) + mat[ M13 ];
        var z = ( vec[ 0 ] * mat[ M20 ] ) + ( vec[ 1 ] * mat[ M21 ] ) + ( vec[ 2 ] * mat[ M22 ] ) + mat[ M23 ];

        vec[ 0 ] = x;
        vec[ 1 ] = y;
        vec[ 2 ] = z;
    }

    /// <summary>
    ///     Multiplies the vector with the given matrix, performing a division by w. The
    ///     matrix array is assumed to hold a 4x4 column major matrix as you can get from
    ///     <see cref="Matrix4.val" />. The vector array is assumed to hold a 3-component
    ///     vector, with x being the first element, y being the second and z being the last
    ///     component. The result is stored in the vector array. This is the same as
    ///     <see cref="Vector3.Prj(Matrix4)" />.
    /// </summary>
    /// <param name="mat"> the matrix </param>
    /// <param name="vec"> the vector.  </param>
    public static void Prj( float[] mat, float[] vec )
    {
        var invW = 1.0f
                 / ( ( vec[ 0 ] * mat[ M30 ] ) + ( vec[ 1 ] * mat[ M31 ] ) + ( vec[ 2 ] * mat[ M32 ] ) + mat[ M33 ] );

        var x = ( ( vec[ 0 ] * mat[ M00 ] ) + ( vec[ 1 ] * mat[ M01 ] ) + ( vec[ 2 ] * mat[ M02 ] ) + mat[ M03 ] )
              * invW;

        var y = ( ( vec[ 0 ] * mat[ M10 ] ) + ( vec[ 1 ] * mat[ M11 ] ) + ( vec[ 2 ] * mat[ M12 ] ) + mat[ M13 ] )
              * invW;

        var z = ( ( vec[ 0 ] * mat[ M20 ] ) + ( vec[ 1 ] * mat[ M21 ] ) + ( vec[ 2 ] * mat[ M22 ] ) + mat[ M23 ] )
              * invW;

        vec[ 0 ] = x;
        vec[ 1 ] = y;
        vec[ 2 ] = z;
    }

    /// <summary>
    ///     Multiplies the vector with the top most 3x3 sub-matrix of the given matrix.
    ///     The matrix array is assumed to hold a 4x4 column major matrix as you can get
    ///     from <see cref="Matrix4.val" />. The vector array is assumed to hold a
    ///     3-component vector, with x being the first element, y being the second and z
    ///     being the last component. The result is stored in the vector array. This is the
    ///     same as <see cref="Vector3.Rot(Matrix4)" />.
    /// </summary>
    /// <param name="mat"> the matrix </param>
    /// <param name="vec"> the vector.  </param>
    public static void Rot( float[] mat, float[] vec )
    {
        var x = ( vec[ 0 ] * mat[ M00 ] ) + ( vec[ 1 ] * mat[ M01 ] ) + ( vec[ 2 ] * mat[ M02 ] );
        var y = ( vec[ 0 ] * mat[ M10 ] ) + ( vec[ 1 ] * mat[ M11 ] ) + ( vec[ 2 ] * mat[ M12 ] );
        var z = ( vec[ 0 ] * mat[ M20 ] ) + ( vec[ 1 ] * mat[ M21 ] ) + ( vec[ 2 ] * mat[ M22 ] );
        vec[ 0 ] = x;
        vec[ 1 ] = y;
        vec[ 2 ] = z;
    }

    /// <summary>
    ///     Computes the inverse of the given matrix. The matrix array is assumed to
    ///     hold a 4x4 column major matrix as you can get from <see cref="Matrix4.val" />.
    /// </summary>
    /// <param name="values"> the matrix values. </param>
    /// <returns> false in case the inverse could not be calculated, true otherwise.  </returns>
    public static bool Inv( float[] values )
    {
        var lDet = Det( values );

        if ( lDet == 0 )
        {
            return false;
        }

        var m00 = ( ( ( ( values[ M12 ] * values[ M23 ] * values[ M31 ] )
                      - ( values[ M13 ] * values[ M22 ] * values[ M31 ] ) )
                    + ( values[ M13 ] * values[ M21 ] * values[ M32 ] ) )
                  - ( values[ M11 ] * values[ M23 ] * values[ M32 ] )
                  - ( values[ M12 ] * values[ M21 ] * values[ M33 ] ) )
                + ( values[ M11 ] * values[ M22 ] * values[ M33 ] );

        var m01 = ( ( ( values[ M03 ] * values[ M22 ] * values[ M31 ] )
                    - ( values[ M02 ] * values[ M23 ] * values[ M31 ] )
                    - ( values[ M03 ] * values[ M21 ] * values[ M32 ] ) )
                  + ( values[ M01 ] * values[ M23 ] * values[ M32 ] )
                  + ( values[ M02 ] * values[ M21 ] * values[ M33 ] ) )
                - ( values[ M01 ] * values[ M22 ] * values[ M33 ] );

        var m02 = ( ( ( ( values[ M02 ] * values[ M13 ] * values[ M31 ] )
                      - ( values[ M03 ] * values[ M12 ] * values[ M31 ] ) )
                    + ( values[ M03 ] * values[ M11 ] * values[ M32 ] ) )
                  - ( values[ M01 ] * values[ M13 ] * values[ M32 ] )
                  - ( values[ M02 ] * values[ M11 ] * values[ M33 ] ) )
                + ( values[ M01 ] * values[ M12 ] * values[ M33 ] );

        var m03 = ( ( ( values[ M03 ] * values[ M12 ] * values[ M21 ] )
                    - ( values[ M02 ] * values[ M13 ] * values[ M21 ] )
                    - ( values[ M03 ] * values[ M11 ] * values[ M22 ] ) )
                  + ( values[ M01 ] * values[ M13 ] * values[ M22 ] )
                  + ( values[ M02 ] * values[ M11 ] * values[ M23 ] ) )
                - ( values[ M01 ] * values[ M12 ] * values[ M23 ] );

        var m10 = ( ( ( values[ M13 ] * values[ M22 ] * values[ M30 ] )
                    - ( values[ M12 ] * values[ M23 ] * values[ M30 ] )
                    - ( values[ M13 ] * values[ M20 ] * values[ M32 ] ) )
                  + ( values[ M10 ] * values[ M23 ] * values[ M32 ] )
                  + ( values[ M12 ] * values[ M20 ] * values[ M33 ] ) )
                - ( values[ M10 ] * values[ M22 ] * values[ M33 ] );

        var m11 = ( ( ( ( values[ M02 ] * values[ M23 ] * values[ M30 ] )
                      - ( values[ M03 ] * values[ M22 ] * values[ M30 ] ) )
                    + ( values[ M03 ] * values[ M20 ] * values[ M32 ] ) )
                  - ( values[ M00 ] * values[ M23 ] * values[ M32 ] )
                  - ( values[ M02 ] * values[ M20 ] * values[ M33 ] ) )
                + ( values[ M00 ] * values[ M22 ] * values[ M33 ] );

        var m12 = ( ( ( values[ M03 ] * values[ M12 ] * values[ M30 ] )
                    - ( values[ M02 ] * values[ M13 ] * values[ M30 ] )
                    - ( values[ M03 ] * values[ M10 ] * values[ M32 ] ) )
                  + ( values[ M00 ] * values[ M13 ] * values[ M32 ] )
                  + ( values[ M02 ] * values[ M10 ] * values[ M33 ] ) )
                - ( values[ M00 ] * values[ M12 ] * values[ M33 ] );

        var m13 = ( ( ( ( values[ M02 ] * values[ M13 ] * values[ M20 ] )
                      - ( values[ M03 ] * values[ M12 ] * values[ M20 ] ) )
                    + ( values[ M03 ] * values[ M10 ] * values[ M22 ] ) )
                  - ( values[ M00 ] * values[ M13 ] * values[ M22 ] )
                  - ( values[ M02 ] * values[ M10 ] * values[ M23 ] ) )
                + ( values[ M00 ] * values[ M12 ] * values[ M23 ] );

        var m20 = ( ( ( ( values[ M11 ] * values[ M23 ] * values[ M30 ] )
                      - ( values[ M13 ] * values[ M21 ] * values[ M30 ] ) )
                    + ( values[ M13 ] * values[ M20 ] * values[ M31 ] ) )
                  - ( values[ M10 ] * values[ M23 ] * values[ M31 ] )
                  - ( values[ M11 ] * values[ M20 ] * values[ M33 ] ) )
                + ( values[ M10 ] * values[ M21 ] * values[ M33 ] );

        var m21 = ( ( ( values[ M03 ] * values[ M21 ] * values[ M30 ] )
                    - ( values[ M01 ] * values[ M23 ] * values[ M30 ] )
                    - ( values[ M03 ] * values[ M20 ] * values[ M31 ] ) )
                  + ( values[ M00 ] * values[ M23 ] * values[ M31 ] )
                  + ( values[ M01 ] * values[ M20 ] * values[ M33 ] ) )
                - ( values[ M00 ] * values[ M21 ] * values[ M33 ] );

        var m22 = ( ( ( ( values[ M01 ] * values[ M13 ] * values[ M30 ] )
                      - ( values[ M03 ] * values[ M11 ] * values[ M30 ] ) )
                    + ( values[ M03 ] * values[ M10 ] * values[ M31 ] ) )
                  - ( values[ M00 ] * values[ M13 ] * values[ M31 ] )
                  - ( values[ M01 ] * values[ M10 ] * values[ M33 ] ) )
                + ( values[ M00 ] * values[ M11 ] * values[ M33 ] );

        var m23 = ( ( ( values[ M03 ] * values[ M11 ] * values[ M20 ] )
                    - ( values[ M01 ] * values[ M13 ] * values[ M20 ] )
                    - ( values[ M03 ] * values[ M10 ] * values[ M21 ] ) )
                  + ( values[ M00 ] * values[ M13 ] * values[ M21 ] )
                  + ( values[ M01 ] * values[ M10 ] * values[ M23 ] ) )
                - ( values[ M00 ] * values[ M11 ] * values[ M23 ] );

        var m30 = ( ( ( values[ M12 ] * values[ M21 ] * values[ M30 ] )
                    - ( values[ M11 ] * values[ M22 ] * values[ M30 ] )
                    - ( values[ M12 ] * values[ M20 ] * values[ M31 ] ) )
                  + ( values[ M10 ] * values[ M22 ] * values[ M31 ] )
                  + ( values[ M11 ] * values[ M20 ] * values[ M32 ] ) )
                - ( values[ M10 ] * values[ M21 ] * values[ M32 ] );

        var m31 = ( ( ( ( values[ M01 ] * values[ M22 ] * values[ M30 ] )
                      - ( values[ M02 ] * values[ M21 ] * values[ M30 ] ) )
                    + ( values[ M02 ] * values[ M20 ] * values[ M31 ] ) )
                  - ( values[ M00 ] * values[ M22 ] * values[ M31 ] )
                  - ( values[ M01 ] * values[ M20 ] * values[ M32 ] ) )
                + ( values[ M00 ] * values[ M21 ] * values[ M32 ] );

        var m32 = ( ( ( values[ M02 ] * values[ M11 ] * values[ M30 ] )
                    - ( values[ M01 ] * values[ M12 ] * values[ M30 ] )
                    - ( values[ M02 ] * values[ M10 ] * values[ M31 ] ) )
                  + ( values[ M00 ] * values[ M12 ] * values[ M31 ] )
                  + ( values[ M01 ] * values[ M10 ] * values[ M32 ] ) )
                - ( values[ M00 ] * values[ M11 ] * values[ M32 ] );

        var m33 = ( ( ( ( values[ M01 ] * values[ M12 ] * values[ M20 ] )
                      - ( values[ M02 ] * values[ M11 ] * values[ M20 ] ) )
                    + ( values[ M02 ] * values[ M10 ] * values[ M21 ] ) )
                  - ( values[ M00 ] * values[ M12 ] * values[ M21 ] )
                  - ( values[ M01 ] * values[ M10 ] * values[ M22 ] ) )
                + ( values[ M00 ] * values[ M11 ] * values[ M22 ] );

        var invDet = 1.0f / lDet;
        values[ M00 ] = m00 * invDet;
        values[ M10 ] = m10 * invDet;
        values[ M20 ] = m20 * invDet;
        values[ M30 ] = m30 * invDet;
        values[ M01 ] = m01 * invDet;
        values[ M11 ] = m11 * invDet;
        values[ M21 ] = m21 * invDet;
        values[ M31 ] = m31 * invDet;
        values[ M02 ] = m02 * invDet;
        values[ M12 ] = m12 * invDet;
        values[ M22 ] = m22 * invDet;
        values[ M32 ] = m32 * invDet;
        values[ M03 ] = m03 * invDet;
        values[ M13 ] = m13 * invDet;
        values[ M23 ] = m23 * invDet;
        values[ M33 ] = m33 * invDet;

        return true;
    }

    /// <summary>
    ///     Computes the determinante of the given matrix. The matrix array is assumed
    ///     to hold a 4x4 column major matrix as you can get from <see cref="Matrix4.val" />.
    /// </summary>
    /// <param name="values"> the matrix values. </param>
    /// <returns> the determinante.  </returns>
    public static float Det( float[] values ) => ( ( ( ( ( ( ( ( ( ( ( ( values[ M30 ] * values[ M21 ] * values[ M12 ] * values[ M03 ] )
                                                                     - ( values[ M20 ] * values[ M31 ] * values[ M12 ] * values[ M03 ] )
                                                                     - ( values[ M30 ]
                                                                       * values[ M11 ]
                                                                       * values[ M22 ]
                                                                       * values[ M03 ] ) )
                                                                   + ( values[ M10 ] * values[ M31 ] * values[ M22 ] * values[ M03 ] )
                                                                   + ( values[ M20 ] * values[ M11 ] * values[ M32 ] * values[ M03 ] ) )
                                                                 - ( values[ M10 ]
                                                                   * values[ M21 ]
                                                                   * values[ M32 ]
                                                                   * values[ M03 ] )
                                                                 - ( values[ M30 ] * values[ M21 ] * values[ M02 ] * values[ M13 ] ) )
                                                               + ( values[ M20 ] * values[ M31 ] * values[ M02 ] * values[ M13 ] )
                                                               + ( values[ M30 ] * values[ M01 ] * values[ M22 ] * values[ M13 ] ) )
                                                             - ( values[ M00 ] * values[ M31 ] * values[ M22 ] * values[ M13 ] )
                                                             - ( values[ M20 ]
                                                               * values[ M01 ]
                                                               * values[ M32 ]
                                                               * values[ M13 ] ) )
                                                           + ( values[ M00 ] * values[ M21 ] * values[ M32 ] * values[ M13 ] )
                                                           + ( values[ M30 ] * values[ M11 ] * values[ M02 ] * values[ M23 ] ) )
                                                         - ( values[ M10 ]
                                                           * values[ M31 ]
                                                           * values[ M02 ]
                                                           * values[ M23 ] )
                                                         - ( values[ M30 ] * values[ M01 ] * values[ M12 ] * values[ M23 ] ) )
                                                       + ( values[ M00 ] * values[ M31 ] * values[ M12 ] * values[ M23 ] )
                                                       + ( values[ M10 ]
                                                         * values[ M01 ]
                                                         * values[ M32 ]
                                                         * values[ M23 ] ) )
                                                     - ( values[ M00 ] * values[ M11 ] * values[ M32 ] * values[ M23 ] )
                                                     - ( values[ M20 ] * values[ M11 ] * values[ M02 ] * values[ M33 ] ) )
                                                   + ( values[ M10 ] * values[ M21 ] * values[ M02 ] * values[ M33 ] )
                                                   + ( values[ M20 ] * values[ M01 ] * values[ M12 ] * values[ M33 ] ) )
                                                 - ( values[ M00 ]
                                                   * values[ M21 ]
                                                   * values[ M12 ]
                                                   * values[ M33 ] )
                                                 - ( values[ M10 ] * values[ M01 ] * values[ M22 ] * values[ M33 ] ) )
                                               + ( values[ M00 ] * values[ M11 ] * values[ M22 ] * values[ M33 ] );

    /// <summary>
    ///     Postmultiplies this matrix by a translation matrix.
    ///     Postmultiplication is also used by OpenGL ES' glTranslate/glRotate/glScale
    /// </summary>
    /// <param name="translation"> </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Translate( Vector3 translation ) => Translate( translation.X, translation.Y, translation.Z );

    /// <summary>
    ///     Postmultiplies this matrix by a translation matrix.
    ///     Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="x"> Translation in the x-axis. </param>
    /// <param name="y"> Translation in the y-axis. </param>
    /// <param name="z"> Translation in the z-axis. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Translate( float x, float y, float z )
    {
        val[ M03 ] += ( val[ M00 ] * x ) + ( val[ M01 ] * y ) + ( val[ M02 ] * z );
        val[ M13 ] += ( val[ M10 ] * x ) + ( val[ M11 ] * y ) + ( val[ M12 ] * z );
        val[ M23 ] += ( val[ M20 ] * x ) + ( val[ M21 ] * y ) + ( val[ M22 ] * z );
        val[ M33 ] += ( val[ M30 ] * x ) + ( val[ M31 ] * y ) + ( val[ M32 ] * z );

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    ///     Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="axis"> The vector axis to rotate around. </param>
    /// <param name="degrees"> The angle in degrees. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Rotate( Vector3 axis, float degrees )
    {
        if ( degrees == 0 )
        {
            return this;
        }

        Quat.Set( axis, degrees );

        return Rotate( Quat );
    }

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    ///     Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="axis"> The vector axis to rotate around. </param>
    /// <param name="radians"> The angle in radians. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 RotateRad( Vector3 axis, float radians )
    {
        if ( radians == 0 )
        {
            return this;
        }

        Quat.SetFromAxisRad( axis, radians );

        return Rotate( Quat );
    }

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    ///     Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale
    /// </summary>
    /// <param name="axisX"> The x-axis component of the vector to rotate around. </param>
    /// <param name="axisY"> The y-axis component of the vector to rotate around. </param>
    /// <param name="axisZ"> The z-axis component of the vector to rotate around. </param>
    /// <param name="degrees"> The angle in degrees </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Rotate( float axisX, float axisY, float axisZ, float degrees )
    {
        if ( degrees == 0 )
        {
            return this;
        }

        Quat.SetFromAxis( axisX, axisY, axisZ, degrees );

        return Rotate( Quat );
    }

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    ///     Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale
    /// </summary>
    /// <param name="axisX"> The x-axis component of the vector to rotate around. </param>
    /// <param name="axisY"> The y-axis component of the vector to rotate around. </param>
    /// <param name="axisZ"> The z-axis component of the vector to rotate around. </param>
    /// <param name="radians"> The angle in radians </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 RotateRad( float axisX, float axisY, float axisZ, float radians )
    {
        if ( radians == 0 )
        {
            return this;
        }

        Quat.SetFromAxisRad( axisX, axisY, axisZ, radians );

        return Rotate( Quat );
    }

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    ///     Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="rotation"> </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Rotate( Quaternion rotation )
    {
        float x  = rotation.X, y = rotation.Y, z = rotation.Z, w = rotation.W;
        var   xx = x * x;
        var   xy = x * y;
        var   xz = x * z;
        var   xw = x * w;
        var   yy = y * y;
        var   yz = y * z;
        var   yw = y * w;
        var   zz = z * z;
        var   zw = z * w;

        // Set matrix from quaternion
        var r00 = 1 - ( 2 * ( yy + zz ) );
        var r01 = 2 * ( xy - zw );
        var r02 = 2 * ( xz + yw );
        var r10 = 2 * ( xy + zw );
        var r11 = 1 - ( 2 * ( xx + zz ) );
        var r12 = 2 * ( yz - xw );
        var r20 = 2 * ( xz - yw );
        var r21 = 2 * ( yz + xw );
        var r22 = 1 - ( 2 * ( xx + yy ) );
        var m00 = ( val[ M00 ] * r00 ) + ( val[ M01 ] * r10 ) + ( val[ M02 ] * r20 );
        var m01 = ( val[ M00 ] * r01 ) + ( val[ M01 ] * r11 ) + ( val[ M02 ] * r21 );
        var m02 = ( val[ M00 ] * r02 ) + ( val[ M01 ] * r12 ) + ( val[ M02 ] * r22 );
        var m10 = ( val[ M10 ] * r00 ) + ( val[ M11 ] * r10 ) + ( val[ M12 ] * r20 );
        var m11 = ( val[ M10 ] * r01 ) + ( val[ M11 ] * r11 ) + ( val[ M12 ] * r21 );
        var m12 = ( val[ M10 ] * r02 ) + ( val[ M11 ] * r12 ) + ( val[ M12 ] * r22 );
        var m20 = ( val[ M20 ] * r00 ) + ( val[ M21 ] * r10 ) + ( val[ M22 ] * r20 );
        var m21 = ( val[ M20 ] * r01 ) + ( val[ M21 ] * r11 ) + ( val[ M22 ] * r21 );
        var m22 = ( val[ M20 ] * r02 ) + ( val[ M21 ] * r12 ) + ( val[ M22 ] * r22 );
        var m30 = ( val[ M30 ] * r00 ) + ( val[ M31 ] * r10 ) + ( val[ M32 ] * r20 );
        var m31 = ( val[ M30 ] * r01 ) + ( val[ M31 ] * r11 ) + ( val[ M32 ] * r21 );
        var m32 = ( val[ M30 ] * r02 ) + ( val[ M31 ] * r12 ) + ( val[ M32 ] * r22 );
        val[ M00 ] = m00;
        val[ M10 ] = m10;
        val[ M20 ] = m20;
        val[ M30 ] = m30;
        val[ M01 ] = m01;
        val[ M11 ] = m11;
        val[ M21 ] = m21;
        val[ M31 ] = m31;
        val[ M02 ] = m02;
        val[ M12 ] = m12;
        val[ M22 ] = m22;
        val[ M32 ] = m32;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix by the rotation between two vectors.
    /// </summary>
    /// <param name="v1"> The base vector </param>
    /// <param name="v2"> The target vector </param>
    /// <returns> This matrix for the purpose of chaining methods together  </returns>
    public Matrix4 Rotate( Vector3 v1, Vector3 v2 ) => Rotate( Quat.SetFromCross( v1, v2 ) );

    /// <summary>
    ///     Post-multiplies this matrix by a rotation toward a direction.
    /// </summary>
    /// <param name="direction"> direction to rotate toward </param>
    /// <param name="up"> up vector </param>
    /// <returns> This matrix for chaining  </returns>
    public Matrix4 RotateTowardDirection( Vector3 direction, Vector3 up )
    {
        LVez.Set( direction ).Nor();
        LVex.Set( direction ).Crs( up ).Nor();
        LVey.Set( LVex ).Crs( LVez ).Nor();

        var m00 = ( val[ M00 ] * LVex.X ) + ( val[ M01 ] * LVex.Y ) + ( val[ M02 ] * LVex.Z );
        var m01 = ( val[ M00 ] * LVey.X ) + ( val[ M01 ] * LVey.Y ) + ( val[ M02 ] * LVey.Z );
        var m02 = ( val[ M00 ] * -LVez.X ) + ( val[ M01 ] * -LVez.Y ) + ( val[ M02 ] * -LVez.Z );
        var m10 = ( val[ M10 ] * LVex.X ) + ( val[ M11 ] * LVex.Y ) + ( val[ M12 ] * LVex.Z );
        var m11 = ( val[ M10 ] * LVey.X ) + ( val[ M11 ] * LVey.Y ) + ( val[ M12 ] * LVey.Z );
        var m12 = ( val[ M10 ] * -LVez.X ) + ( val[ M11 ] * -LVez.Y ) + ( val[ M12 ] * -LVez.Z );
        var m20 = ( val[ M20 ] * LVex.X ) + ( val[ M21 ] * LVex.Y ) + ( val[ M22 ] * LVex.Z );
        var m21 = ( val[ M20 ] * LVey.X ) + ( val[ M21 ] * LVey.Y ) + ( val[ M22 ] * LVey.Z );
        var m22 = ( val[ M20 ] * -LVez.X ) + ( val[ M21 ] * -LVez.Y ) + ( val[ M22 ] * -LVez.Z );
        var m30 = ( val[ M30 ] * LVex.X ) + ( val[ M31 ] * LVex.Y ) + ( val[ M32 ] * LVex.Z );
        var m31 = ( val[ M30 ] * LVey.X ) + ( val[ M31 ] * LVey.Y ) + ( val[ M32 ] * LVey.Z );
        var m32 = ( val[ M30 ] * -LVez.X ) + ( val[ M31 ] * -LVez.Y ) + ( val[ M32 ] * -LVez.Z );

        val[ M00 ] = m00;
        val[ M10 ] = m10;
        val[ M20 ] = m20;
        val[ M30 ] = m30;
        val[ M01 ] = m01;
        val[ M11 ] = m11;
        val[ M21 ] = m21;
        val[ M31 ] = m31;
        val[ M02 ] = m02;
        val[ M12 ] = m12;
        val[ M22 ] = m22;
        val[ M32 ] = m32;

        return this;
    }

    /// <summary>
    ///     Post-multiplies this matrix by a rotation toward a target.
    /// </summary>
    /// <param name="target"> the target to rotate to </param>
    /// <param name="up"> the up vector </param>
    /// <returns> This matrix for chaining  </returns>
    public Matrix4 RotateTowardTarget( Vector3 target, Vector3 up )
    {
        TmpVec.Set( target.X - val[ M03 ], target.Y - val[ M13 ], target.Z - val[ M23 ] );

        return RotateTowardDirection( TmpVec, up );
    }

    /// <summary>
    ///     Postmultiplies this matrix with a scale matrix.
    ///     Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="scaleX"> The scale in the x-axis. </param>
    /// <param name="scaleY"> The scale in the y-axis. </param>
    /// <param name="scaleZ"> The scale in the z-axis. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix4 Scale( float scaleX, float scaleY, float scaleZ )
    {
        val[ M00 ] *= scaleX;
        val[ M01 ] *= scaleY;
        val[ M02 ] *= scaleZ;
        val[ M10 ] *= scaleX;
        val[ M11 ] *= scaleY;
        val[ M12 ] *= scaleZ;
        val[ M20 ] *= scaleX;
        val[ M21 ] *= scaleY;
        val[ M22 ] *= scaleZ;
        val[ M30 ] *= scaleX;
        val[ M31 ] *= scaleY;
        val[ M32 ] *= scaleZ;

        return this;
    }

    /// <summary>
    ///     Copies the 4x3 upper-left sub-matrix into float array. The destination
    ///     array is supposed to be a column major matrix.
    /// </summary>
    /// <param name="dst"> the destination matrix </param>
    public void Extract4X3Matrix( float[] dst )
    {
        dst[ 0 ]  = val[ M00 ];
        dst[ 1 ]  = val[ M10 ];
        dst[ 2 ]  = val[ M20 ];
        dst[ 3 ]  = val[ M01 ];
        dst[ 4 ]  = val[ M11 ];
        dst[ 5 ]  = val[ M21 ];
        dst[ 6 ]  = val[ M02 ];
        dst[ 7 ]  = val[ M12 ];
        dst[ 8 ]  = val[ M22 ];
        dst[ 9 ]  = val[ M03 ];
        dst[ 10 ] = val[ M13 ];
        dst[ 11 ] = val[ M23 ];
    }

    /// <summary>
    /// </summary>
    /// <returns>True if this matrix has any rotation or scaling, false otherwise </returns>
    public bool HasRotationOrScaling() => !( MathUtils.IsEqual( val[ M00 ], 1 )
                                          && MathUtils.IsEqual( val[ M11 ], 1 )
                                          && MathUtils.IsEqual( val[ M22 ], 1 )
                                          && MathUtils.IsZero( val[ M01 ] )
                                          && MathUtils.IsZero( val[ M02 ] )
                                          && MathUtils.IsZero( val[ M10 ] )
                                          && MathUtils.IsZero( val[ M12 ] )
                                          && MathUtils.IsZero( val[ M20 ] )
                                          && MathUtils.IsZero( val[ M21 ] ) );
}
