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


using LibGDXSharp.LibCore.Utils;

namespace LibGDXSharp.LibCore.Maths;

public class Matrix3
{
    public const     int     M00  = 0;
    public const     int     M01  = 3;
    public const     int     M02  = 6;
    public const     int     M10  = 1;
    public const     int     M11  = 4;
    public const     int     M12  = 7;
    public const     int     M20  = 2;
    public const     int     M21  = 5;
    public const     int     M22  = 8;
    private readonly float[] _tmp = new float[ 9 ];

    public readonly float[] val = new float[ 9 ];

    public Matrix3() => Idt();

    public Matrix3( Matrix3 matrix ) => Set( matrix );

    /// <summary>
    ///     Constructs a matrix from the given float array. The array must have at
    ///     least 9 elements; the first 9 will be copied.
    /// </summary>
    /// <param name="values">
    ///     The float array to copy.
    ///     Remember that this matrix is in
    ///     <a href="http://en.wikipedia.org/wiki/Row-major_order#Column-major_order">
    ///         column major
    ///     </a>
    ///     order. (The float array is not modified.)
    /// </param>
    public Matrix3( float[] values ) => Set( values );

    /// <summary>
    ///     Sets this matrix to the identity matrix
    /// </summary>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 Idt()
    {
        val[ M00 ] = 1;
        val[ M10 ] = 0;
        val[ M20 ] = 0;
        val[ M01 ] = 0;
        val[ M11 ] = 1;
        val[ M21 ] = 0;
        val[ M02 ] = 0;
        val[ M12 ] = 0;
        val[ M22 ] = 1;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with the provided matrix and stores the
    ///     result in this matrix. For example:
    ///     <para>
    ///         <tt>A.mul(B) results in A := AB</tt>
    ///     </para>
    /// </summary>
    /// <param name="m"> Matrix to multiply by. </param>
    /// <returns> This matrix for chaining operations together. </returns>
    public Matrix3 Mul( Matrix3 m )
    {
        var v00 = ( val[ M00 ] * m.val[ M00 ] ) + ( val[ M01 ] * m.val[ M10 ] ) + ( val[ M02 ] * m.val[ M20 ] );
        var v01 = ( val[ M00 ] * m.val[ M01 ] ) + ( val[ M01 ] * m.val[ M11 ] ) + ( val[ M02 ] * m.val[ M21 ] );
        var v02 = ( val[ M00 ] * m.val[ M02 ] ) + ( val[ M01 ] * m.val[ M12 ] ) + ( val[ M02 ] * m.val[ M22 ] );

        var v10 = ( val[ M10 ] * m.val[ M00 ] ) + ( val[ M11 ] * m.val[ M10 ] ) + ( val[ M12 ] * m.val[ M20 ] );
        var v11 = ( val[ M10 ] * m.val[ M01 ] ) + ( val[ M11 ] * m.val[ M11 ] ) + ( val[ M12 ] * m.val[ M21 ] );
        var v12 = ( val[ M10 ] * m.val[ M02 ] ) + ( val[ M11 ] * m.val[ M12 ] ) + ( val[ M12 ] * m.val[ M22 ] );

        var v20 = ( val[ M20 ] * m.val[ M00 ] ) + ( val[ M21 ] * m.val[ M10 ] ) + ( val[ M22 ] * m.val[ M20 ] );
        var v21 = ( val[ M20 ] * m.val[ M01 ] ) + ( val[ M21 ] * m.val[ M11 ] ) + ( val[ M22 ] * m.val[ M21 ] );
        var v22 = ( val[ M20 ] * m.val[ M02 ] ) + ( val[ M21 ] * m.val[ M12 ] ) + ( val[ M22 ] * m.val[ M22 ] );

        val[ M00 ] = v00;
        val[ M10 ] = v10;
        val[ M20 ] = v20;
        val[ M01 ] = v01;
        val[ M11 ] = v11;
        val[ M21 ] = v21;
        val[ M02 ] = v02;
        val[ M12 ] = v12;
        val[ M22 ] = v22;

        return this;
    }

    /// <summary>
    ///     Premultiplies this matrix with the provided matrix and stores the
    ///     result in this matrix. For example:
    ///     <para>
    ///         <tt>A.mulLeft(B) results in A := BA</tt>
    ///     </para>
    /// </summary>
    /// <param name="m"> The other Matrix to multiply by </param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 MulLeft( Matrix3 m )
    {
        var v00 = ( m.val[ M00 ] * val[ M00 ] ) + ( m.val[ M01 ] * val[ M10 ] ) + ( m.val[ M02 ] * val[ M20 ] );
        var v01 = ( m.val[ M00 ] * val[ M01 ] ) + ( m.val[ M01 ] * val[ M11 ] ) + ( m.val[ M02 ] * val[ M21 ] );
        var v02 = ( m.val[ M00 ] * val[ M02 ] ) + ( m.val[ M01 ] * val[ M12 ] ) + ( m.val[ M02 ] * val[ M22 ] );

        var v10 = ( m.val[ M10 ] * val[ M00 ] ) + ( m.val[ M11 ] * val[ M10 ] ) + ( m.val[ M12 ] * val[ M20 ] );
        var v11 = ( m.val[ M10 ] * val[ M01 ] ) + ( m.val[ M11 ] * val[ M11 ] ) + ( m.val[ M12 ] * val[ M21 ] );
        var v12 = ( m.val[ M10 ] * val[ M02 ] ) + ( m.val[ M11 ] * val[ M12 ] ) + ( m.val[ M12 ] * val[ M22 ] );

        var v20 = ( m.val[ M20 ] * val[ M00 ] ) + ( m.val[ M21 ] * val[ M10 ] ) + ( m.val[ M22 ] * val[ M20 ] );
        var v21 = ( m.val[ M20 ] * val[ M01 ] ) + ( m.val[ M21 ] * val[ M11 ] ) + ( m.val[ M22 ] * val[ M21 ] );
        var v22 = ( m.val[ M20 ] * val[ M02 ] ) + ( m.val[ M21 ] * val[ M12 ] ) + ( m.val[ M22 ] * val[ M22 ] );

        val[ M00 ] = v00;
        val[ M10 ] = v10;
        val[ M20 ] = v20;
        val[ M01 ] = v01;
        val[ M11 ] = v11;
        val[ M21 ] = v21;
        val[ M02 ] = v02;
        val[ M12 ] = v12;
        val[ M22 ] = v22;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a rotation matrix that will rotate any vector in
    ///     counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="degrees"> the angle in degrees.</param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToRotation( float degrees ) => SetToRotationRad( MathUtils.DEGREES_TO_RADIANS * degrees );

    /// <summary>
    ///     Sets this matrix to a rotation matrix that will rotate any vector in
    ///     counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="radians"> the angle in radians.</param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToRotationRad( float radians )
    {
        var cos = ( float )Math.Cos( radians );
        var sin = ( float )Math.Sin( radians );

        val[ M00 ] = cos;
        val[ M10 ] = sin;
        val[ M20 ] = 0;

        val[ M01 ] = -sin;
        val[ M11 ] = cos;
        val[ M21 ] = 0;

        val[ M02 ] = 0;
        val[ M12 ] = 0;
        val[ M22 ] = 1;

        return this;
    }

    public Matrix3 SetToRotation( Vector3 axis, float degrees ) => SetToRotation( axis, MathUtils.CosDeg( degrees ), MathUtils.SinDeg( degrees ) );

    public Matrix3 SetToRotation( Vector3 axis, float cos, float sin )
    {
        var oc = 1.0f - cos;

        val[ M00 ] = ( oc * axis.X * axis.X ) + cos;
        val[ M01 ] = ( oc * axis.X * axis.Y ) - ( axis.Z * sin );
        val[ M02 ] = ( oc * axis.Z * axis.X ) + ( axis.Y * sin );
        val[ M10 ] = ( oc * axis.X * axis.Y ) + ( axis.Z * sin );
        val[ M11 ] = ( oc * axis.Y * axis.Y ) + cos;
        val[ M12 ] = ( oc * axis.Y * axis.Z ) - ( axis.X * sin );
        val[ M20 ] = ( oc * axis.Z * axis.X ) - ( axis.Y * sin );
        val[ M21 ] = ( oc * axis.Y * axis.Z ) + ( axis.X * sin );
        val[ M22 ] = ( oc * axis.Z * axis.Z ) + cos;

        return this;
    }

    /// Sets this matrix to a translation matrix.
    /// <param name="x"> the translation in x</param>
    /// <param name="y"> the translation in y</param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToTranslation( float x, float y )
    {
        val[ M00 ] = 1;
        val[ M10 ] = 0;
        val[ M20 ] = 0;

        val[ M01 ] = 0;
        val[ M11 ] = 1;
        val[ M21 ] = 0;

        val[ M02 ] = x;
        val[ M12 ] = y;
        val[ M22 ] = 1;

        return this;
    }

    /// Sets this matrix to a translation matrix.
    /// <param name="translation"> The translation vector.</param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToTranslation( Vector2 translation )
    {
        val[ M00 ] = 1;
        val[ M10 ] = 0;
        val[ M20 ] = 0;

        val[ M01 ] = 0;
        val[ M11 ] = 1;
        val[ M21 ] = 0;

        val[ M02 ] = translation.X;
        val[ M12 ] = translation.Y;
        val[ M22 ] = 1;

        return this;
    }

    /// Sets this matrix to a scaling matrix.
    /// <param name="scaleX"> the scale in x </param>
    /// <param name="scaleY"> the scale in y </param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToScaling( float scaleX, float scaleY )
    {
        val[ M00 ] = scaleX;
        val[ M10 ] = 0;
        val[ M20 ] = 0;
        val[ M01 ] = 0;
        val[ M11 ] = scaleY;
        val[ M21 ] = 0;
        val[ M02 ] = 0;
        val[ M12 ] = 0;
        val[ M22 ] = 1;

        return this;
    }

    /// Sets this matrix to a scaling matrix.
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToScaling( Vector2 scale )
    {
        val[ M00 ] = scale.X;
        val[ M10 ] = 0;
        val[ M20 ] = 0;
        val[ M01 ] = 0;
        val[ M11 ] = scale.Y;
        val[ M21 ] = 0;
        val[ M02 ] = 0;
        val[ M12 ] = 0;
        val[ M22 ] = 1;

        return this;
    }

    public override string ToString() => $"[{val[ M00 ]} | {val[ M01 ]} | {val[ M02 ]}]\n"
                                       + $"[{val[ M10 ]} | {val[ M11 ]} | {val[ M12 ]}]\n"
                                       + $"[{val[ M20 ]} | {val[ M21 ]} | {val[ M22 ]}]";

    /// <returns> The determinant of this matrix </returns>
    public float Det() => ( ( val[ M00 ] * val[ M11 ] * val[ M22 ] )
                          + ( val[ M01 ] * val[ M12 ] * val[ M20 ] )
                          + ( val[ M02 ] * val[ M10 ] * val[ M21 ] ) )
                        - ( val[ M00 ] * val[ M12 ] * val[ M21 ] )
                        - ( val[ M01 ] * val[ M10 ] * val[ M22 ] )
                        - ( val[ M02 ] * val[ M11 ] * val[ M20 ] );

    /// <summary>
    ///     Inverts this matrix given that the determinant is != 0.
    /// </summary>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    /// <exception cref="GdxRuntimeException"> if the matrix is singular (not invertible) </exception>
    public Matrix3 Inv()
    {
        var det = Det();

        if ( det == 0 )
        {
            throw new GdxRuntimeException( "Can't invert a singular matrix" );
        }

        var invDet = 1.0f / det;

        _tmp[ M00 ] = ( val[ M11 ] * val[ M22 ] ) - ( val[ M21 ] * val[ M12 ] );
        _tmp[ M10 ] = ( val[ M20 ] * val[ M12 ] ) - ( val[ M10 ] * val[ M22 ] );
        _tmp[ M20 ] = ( val[ M10 ] * val[ M21 ] ) - ( val[ M20 ] * val[ M11 ] );
        _tmp[ M01 ] = ( val[ M21 ] * val[ M02 ] ) - ( val[ M01 ] * val[ M22 ] );
        _tmp[ M11 ] = ( val[ M00 ] * val[ M22 ] ) - ( val[ M20 ] * val[ M02 ] );
        _tmp[ M21 ] = ( val[ M20 ] * val[ M01 ] ) - ( val[ M00 ] * val[ M21 ] );
        _tmp[ M02 ] = ( val[ M01 ] * val[ M12 ] ) - ( val[ M11 ] * val[ M02 ] );
        _tmp[ M12 ] = ( val[ M10 ] * val[ M02 ] ) - ( val[ M00 ] * val[ M12 ] );
        _tmp[ M22 ] = ( val[ M00 ] * val[ M11 ] ) - ( val[ M10 ] * val[ M01 ] );

        val[ M00 ] = invDet * _tmp[ M00 ];
        val[ M10 ] = invDet * _tmp[ M10 ];
        val[ M20 ] = invDet * _tmp[ M20 ];
        val[ M01 ] = invDet * _tmp[ M01 ];
        val[ M11 ] = invDet * _tmp[ M11 ];
        val[ M21 ] = invDet * _tmp[ M21 ];
        val[ M02 ] = invDet * _tmp[ M02 ];
        val[ M12 ] = invDet * _tmp[ M12 ];
        val[ M22 ] = invDet * _tmp[ M22 ];

        return this;
    }

    /// Copies the values from the provided matrix to this matrix.
    /// <param name="mat"> The matrix to copy. </param>
    /// <returns> This matrix for the purposes of chaining. </returns>
    public Matrix3 Set( Matrix3 mat )
    {
        Array.Copy( mat.val, 0, val, 0, val.Length );

        return this;
    }

    /// <summary>
    ///     Copies the values from the provided affine matrix to this matrix.
    ///     The last row is set to (0, 0, 1).
    /// </summary>
    /// <param name="affine"> The affine matrix to copy. </param>
    /// <returns> This matrix for the purposes of chaining. </returns>
    public Matrix3 Set( Affine2 affine )
    {
        val[ M00 ] = affine.m00;
        val[ M10 ] = affine.m10;
        val[ M20 ] = 0;
        val[ M01 ] = affine.m01;
        val[ M11 ] = affine.m11;
        val[ M21 ] = 0;
        val[ M02 ] = affine.m02;
        val[ M12 ] = affine.m12;
        val[ M22 ] = 1;

        return this;
    }

    /// Sets this 3x3 matrix to the top left 3x3 corner of the provided 4x4 matrix.
    /// <param name="mat">
    ///     The matrix whose top left corner will be copied. This matrix will not be modified.
    /// </param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 Set( Matrix4 mat )
    {
        val[ M00 ] = mat.val[ Matrix4.M00 ];
        val[ M10 ] = mat.val[ Matrix4.M10 ];
        val[ M20 ] = mat.val[ Matrix4.M20 ];
        val[ M01 ] = mat.val[ Matrix4.M01 ];
        val[ M11 ] = mat.val[ Matrix4.M11 ];
        val[ M21 ] = mat.val[ Matrix4.M21 ];
        val[ M02 ] = mat.val[ Matrix4.M02 ];
        val[ M12 ] = mat.val[ Matrix4.M12 ];
        val[ M22 ] = mat.val[ Matrix4.M22 ];

        return this;
    }

    /// <summary>
    ///     Sets the matrix to the given matrix as a float array. The float array
    ///     must have at least 9 elements; the first 9 will be copied.
    /// </summary>
    /// <param name="values">
    ///     The matrix, in float form, that is to be copied. Remember that this matrix is in
    ///     <a href="http://en.wikipedia.org/wiki/Row-major_order#Column-major_order">column major</a>
    ///     order.
    /// </param>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    public Matrix3 Set( float[] values )
    {
        Array.Copy( values, 0, val, 0, val.Length );

        return this;
    }

    /// <summary>
    ///     Adds a translational component to the matrix in the 3rd column. The other columns are untouched.
    /// </summary>
    /// <param name="vector"> The translation vector. </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Trn( Vector2 vector )
    {
        val[ M02 ] += vector.X;
        val[ M12 ] += vector.Y;

        return this;
    }

    /// <summary>
    ///     Adds a translational component to the matrix in the 3rd column. The other columns are untouched.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Trn( float x, float y )
    {
        val[ M02 ] += x;
        val[ M12 ] += y;

        return this;
    }

    /// <summary>
    ///     Adds a translational component to the matrix in the 3rd column. The other columns are untouched.
    /// </summary>
    /// <param name="vector">
    ///     The translation vector. (The z-component of the vector is ignored because this is a 3x3 matrix)
    /// </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Trn( Vector3 vector )
    {
        val[ M02 ] += vector.X;
        val[ M12 ] += vector.Y;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix by a translation matrix. Postmultiplication is also used by OpenGL ES' 1.x
    ///     glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Translate( float x, float y )
    {
        _tmp[ M00 ] = 1;
        _tmp[ M10 ] = 0;
        _tmp[ M20 ] = 0;

        _tmp[ M01 ] = 0;
        _tmp[ M11 ] = 1;
        _tmp[ M21 ] = 0;

        _tmp[ M02 ] = x;
        _tmp[ M12 ] = y;
        _tmp[ M22 ] = 1;

        Mul( val, _tmp );

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix by a translation matrix. Postmultiplication is also used by OpenGL ES' 1.x
    ///     glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="translation"> The translation vector. </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Translate( Vector2 translation )
    {
        _tmp[ M00 ] = 1;
        _tmp[ M10 ] = 0;
        _tmp[ M20 ] = 0;

        _tmp[ M01 ] = 0;
        _tmp[ M11 ] = 1;
        _tmp[ M21 ] = 0;

        _tmp[ M02 ] = translation.X;
        _tmp[ M12 ] = translation.Y;
        _tmp[ M22 ] = 1;

        Mul( val, _tmp );

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix. Postmultiplication is also used by OpenGL
    ///     ES' 1.x
    ///     glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="degrees"> The angle in degrees </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Rotate( float degrees ) => RotateRad( MathUtils.DEGREES_TO_RADIANS * degrees );

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    ///     Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="radians"> The angle in radians </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 RotateRad( float radians )
    {
        if ( radians == 0 )
        {
            return this;
        }

        var cos = ( float )Math.Cos( radians );
        var sin = ( float )Math.Sin( radians );

        _tmp[ M00 ] = cos;
        _tmp[ M10 ] = sin;
        _tmp[ M20 ] = 0;

        _tmp[ M01 ] = -sin;
        _tmp[ M11 ] = cos;
        _tmp[ M21 ] = 0;

        _tmp[ M02 ] = 0;
        _tmp[ M12 ] = 0;
        _tmp[ M22 ] = 1;

        Mul( val, _tmp );

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with a scale matrix. Postmultiplication is also
    ///     used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="scaleX"> The scale in the x-axis. </param>
    /// <param name="scaleY"> The scale in the y-axis. </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Scale( float scaleX, float scaleY )
    {
        _tmp[ M00 ] = scaleX;
        _tmp[ M10 ] = 0;
        _tmp[ M20 ] = 0;
        _tmp[ M01 ] = 0;
        _tmp[ M11 ] = scaleY;
        _tmp[ M21 ] = 0;
        _tmp[ M02 ] = 0;
        _tmp[ M12 ] = 0;
        _tmp[ M22 ] = 1;

        Mul( val, _tmp );

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with a scale matrix. Postmultiplication is also
    ///     used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="scale"> The vector to scale the matrix by. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Matrix3 Scale( Vector2 scale )
    {
        _tmp[ M00 ] = scale.X;
        _tmp[ M10 ] = 0;
        _tmp[ M20 ] = 0;
        _tmp[ M01 ] = 0;
        _tmp[ M11 ] = scale.Y;
        _tmp[ M21 ] = 0;
        _tmp[ M02 ] = 0;
        _tmp[ M12 ] = 0;
        _tmp[ M22 ] = 1;

        Mul( val, _tmp );

        return this;
    }

    /// <summary>
    ///     Get the values in this matrix.
    /// </summary>
    /// <returns> The float values that make up this matrix in column-major order. </returns>
    public float[] GetValues() => val;

    public Vector2 GetTranslation( Vector2 position )
    {
        position.X = val[ M02 ];
        position.Y = val[ M12 ];

        return position;
    }

    /// <param name="scale">
    ///     The vector which will receive the (non-negative) scale components on each axis.
    /// </param>
    /// <returns> The provided vector for chaining. </returns>
    public Vector2 GetScale( Vector2 scale )
    {
        scale.X = ( float )Math.Sqrt( ( val[ M00 ] * val[ M00 ] ) + ( val[ M01 ] * val[ M01 ] ) );
        scale.Y = ( float )Math.Sqrt( ( val[ M10 ] * val[ M10 ] ) + ( val[ M11 ] * val[ M11 ] ) );

        return scale;
    }

    public float GetRotation() => MathUtils.RADIANS_TO_DEGREES * ( float )Math.Atan2( val[ M10 ], val[ M00 ] );

    public float GetRotationRad() => ( float )Math.Atan2( val[ M10 ], val[ M00 ] );

    /// <summary>
    ///     Scale the matrix in the both the x and y components by the scalar value.
    /// </summary>
    /// <param name="scale"> The single value that will be used to scale both the x and y components. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix3 Scl( float scale )
    {
        val[ M00 ] *= scale;
        val[ M11 ] *= scale;

        return this;
    }

    /// <summary>
    ///     Scale this matrix using the x and y components of the vector
    ///     but leave the rest of the matrix alone.
    /// </summary>
    /// <param name="scale"> The <see cref="Vector3" /> to use to scale this matrix. </param>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    public Matrix3 Scl( Vector2 scale )
    {
        val[ M00 ] *= scale.X;
        val[ M11 ] *= scale.Y;

        return this;
    }

    /// <summary>
    ///     Scale this matrix using the x and y components of the vector but
    ///     leave the rest of the matrix alone.
    /// </summary>
    /// <param name="scale">
    ///     The <see cref="Vector3" /> to use to scale this matrix.
    ///     The z component will be ignored.
    /// </param>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    public Matrix3 Scl( Vector3 scale )
    {
        val[ M00 ] *= scale.X;
        val[ M11 ] *= scale.Y;

        return this;
    }

    /// <summary>
    ///     Transposes the current matrix.
    /// </summary>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    public Matrix3 Transpose()
    {
        ( val[ M01 ], val[ M10 ] ) = ( val[ M10 ], val[ M01 ] );
        ( val[ M02 ], val[ M20 ] ) = ( val[ M20 ], val[ M02 ] );
        ( val[ M12 ], val[ M21 ] ) = ( val[ M12 ], val[ M21 ] );

        return this;
    }

    /// Multiplies matrix a with matrix b in the following manner:
    /// <para>
    ///     <tt>mul(A, B) => A := AB</tt>
    /// </para>
    /// <param name="mata">
    ///     The float array representing the first matrix. Must have at least 9 elements.
    /// </param>
    /// <param name="matb">
    ///     The float array representing the second matrix. Must have at least 9 elements.
    /// </param>
    private static void Mul( float[] mata, float[] matb )
    {
        var v00 = ( mata[ M00 ] * matb[ M00 ] ) + ( mata[ M01 ] * matb[ M10 ] ) + ( mata[ M02 ] * matb[ M20 ] );
        var v01 = ( mata[ M00 ] * matb[ M01 ] ) + ( mata[ M01 ] * matb[ M11 ] ) + ( mata[ M02 ] * matb[ M21 ] );
        var v02 = ( mata[ M00 ] * matb[ M02 ] ) + ( mata[ M01 ] * matb[ M12 ] ) + ( mata[ M02 ] * matb[ M22 ] );

        var v10 = ( mata[ M10 ] * matb[ M00 ] ) + ( mata[ M11 ] * matb[ M10 ] ) + ( mata[ M12 ] * matb[ M20 ] );
        var v11 = ( mata[ M10 ] * matb[ M01 ] ) + ( mata[ M11 ] * matb[ M11 ] ) + ( mata[ M12 ] * matb[ M21 ] );
        var v12 = ( mata[ M10 ] * matb[ M02 ] ) + ( mata[ M11 ] * matb[ M12 ] ) + ( mata[ M12 ] * matb[ M22 ] );

        var v20 = ( mata[ M20 ] * matb[ M00 ] ) + ( mata[ M21 ] * matb[ M10 ] ) + ( mata[ M22 ] * matb[ M20 ] );
        var v21 = ( mata[ M20 ] * matb[ M01 ] ) + ( mata[ M21 ] * matb[ M11 ] ) + ( mata[ M22 ] * matb[ M21 ] );
        var v22 = ( mata[ M20 ] * matb[ M02 ] ) + ( mata[ M21 ] * matb[ M12 ] ) + ( mata[ M22 ] * matb[ M22 ] );

        mata[ M00 ] = v00;
        mata[ M10 ] = v10;
        mata[ M20 ] = v20;
        mata[ M01 ] = v01;
        mata[ M11 ] = v11;
        mata[ M21 ] = v21;
        mata[ M02 ] = v02;
        mata[ M12 ] = v12;
        mata[ M22 ] = v22;
    }
}
