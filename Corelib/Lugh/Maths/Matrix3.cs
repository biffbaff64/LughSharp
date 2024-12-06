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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Maths;

[PublicAPI]
public class Matrix3
{
    public const int M00 = 0;
    public const int M01 = 3;
    public const int M02 = 6;
    public const int M10 = 1;
    public const int M11 = 4;
    public const int M12 = 7;
    public const int M20 = 2;
    public const int M21 = 5;
    public const int M22 = 8;

    private readonly float[] _tmp = new float[ 9 ];

    public readonly float[] Val = new float[ 9 ];

    // ========================================================================

    public Matrix3()
    {
        Idt();
    }

    public Matrix3( Matrix3 matrix )
    {
        Set( matrix );
    }

    /// <summary>
    /// Constructs a matrix from the given float array. The array must have at least 9
    /// elements; the first 9 will be copied.
    /// </summary>
    /// <param name="values"> The float array to copy. Remember that this matrix is in
    /// <a href="http://en.wikipedia.org/wiki/Row-major_order#Column-major_order"> column major </a>
    /// order. (The float array is not modified.)
    /// </param>
    public Matrix3( float[] values )
    {
        Set( values );
    }

    /// <summary>
    /// Sets this matrix to the identity matrix
    /// </summary>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 Idt()
    {
        Val[ M00 ] = 1;
        Val[ M10 ] = 0;
        Val[ M20 ] = 0;
        Val[ M01 ] = 0;
        Val[ M11 ] = 1;
        Val[ M21 ] = 0;
        Val[ M02 ] = 0;
        Val[ M12 ] = 0;
        Val[ M22 ] = 1;

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix with the provided matrix and stores the
    /// result in this matrix. For example:
    /// <para>
    ///     <tt>A.mul(B) results in A := AB</tt>
    /// </para>
    /// </summary>
    /// <param name="m"> Matrix to multiply by. </param>
    /// <returns> This matrix for chaining operations together. </returns>
    public Matrix3 Mul( Matrix3 m )
    {
        var v00 = ( Val[ M00 ] * m.Val[ M00 ] ) + ( Val[ M01 ] * m.Val[ M10 ] ) + ( Val[ M02 ] * m.Val[ M20 ] );
        var v01 = ( Val[ M00 ] * m.Val[ M01 ] ) + ( Val[ M01 ] * m.Val[ M11 ] ) + ( Val[ M02 ] * m.Val[ M21 ] );
        var v02 = ( Val[ M00 ] * m.Val[ M02 ] ) + ( Val[ M01 ] * m.Val[ M12 ] ) + ( Val[ M02 ] * m.Val[ M22 ] );

        var v10 = ( Val[ M10 ] * m.Val[ M00 ] ) + ( Val[ M11 ] * m.Val[ M10 ] ) + ( Val[ M12 ] * m.Val[ M20 ] );
        var v11 = ( Val[ M10 ] * m.Val[ M01 ] ) + ( Val[ M11 ] * m.Val[ M11 ] ) + ( Val[ M12 ] * m.Val[ M21 ] );
        var v12 = ( Val[ M10 ] * m.Val[ M02 ] ) + ( Val[ M11 ] * m.Val[ M12 ] ) + ( Val[ M12 ] * m.Val[ M22 ] );

        var v20 = ( Val[ M20 ] * m.Val[ M00 ] ) + ( Val[ M21 ] * m.Val[ M10 ] ) + ( Val[ M22 ] * m.Val[ M20 ] );
        var v21 = ( Val[ M20 ] * m.Val[ M01 ] ) + ( Val[ M21 ] * m.Val[ M11 ] ) + ( Val[ M22 ] * m.Val[ M21 ] );
        var v22 = ( Val[ M20 ] * m.Val[ M02 ] ) + ( Val[ M21 ] * m.Val[ M12 ] ) + ( Val[ M22 ] * m.Val[ M22 ] );

        Val[ M00 ] = v00;
        Val[ M10 ] = v10;
        Val[ M20 ] = v20;
        Val[ M01 ] = v01;
        Val[ M11 ] = v11;
        Val[ M21 ] = v21;
        Val[ M02 ] = v02;
        Val[ M12 ] = v12;
        Val[ M22 ] = v22;

        return this;
    }

    /// <summary>
    /// Premultiplies this matrix with the provided matrix and stores the
    /// result in this matrix. For example:
    /// <para>
    /// <c>A.mulLeft(B) results in A := BA</c>
    /// </para>
    /// </summary>
    /// <param name="m"> The other Matrix to multiply by </param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 MulLeft( Matrix3 m )
    {
        var v00 = ( m.Val[ M00 ] * Val[ M00 ] ) + ( m.Val[ M01 ] * Val[ M10 ] ) + ( m.Val[ M02 ] * Val[ M20 ] );
        var v01 = ( m.Val[ M00 ] * Val[ M01 ] ) + ( m.Val[ M01 ] * Val[ M11 ] ) + ( m.Val[ M02 ] * Val[ M21 ] );
        var v02 = ( m.Val[ M00 ] * Val[ M02 ] ) + ( m.Val[ M01 ] * Val[ M12 ] ) + ( m.Val[ M02 ] * Val[ M22 ] );

        var v10 = ( m.Val[ M10 ] * Val[ M00 ] ) + ( m.Val[ M11 ] * Val[ M10 ] ) + ( m.Val[ M12 ] * Val[ M20 ] );
        var v11 = ( m.Val[ M10 ] * Val[ M01 ] ) + ( m.Val[ M11 ] * Val[ M11 ] ) + ( m.Val[ M12 ] * Val[ M21 ] );
        var v12 = ( m.Val[ M10 ] * Val[ M02 ] ) + ( m.Val[ M11 ] * Val[ M12 ] ) + ( m.Val[ M12 ] * Val[ M22 ] );

        var v20 = ( m.Val[ M20 ] * Val[ M00 ] ) + ( m.Val[ M21 ] * Val[ M10 ] ) + ( m.Val[ M22 ] * Val[ M20 ] );
        var v21 = ( m.Val[ M20 ] * Val[ M01 ] ) + ( m.Val[ M21 ] * Val[ M11 ] ) + ( m.Val[ M22 ] * Val[ M21 ] );
        var v22 = ( m.Val[ M20 ] * Val[ M02 ] ) + ( m.Val[ M21 ] * Val[ M12 ] ) + ( m.Val[ M22 ] * Val[ M22 ] );

        Val[ M00 ] = v00;
        Val[ M10 ] = v10;
        Val[ M20 ] = v20;
        Val[ M01 ] = v01;
        Val[ M11 ] = v11;
        Val[ M21 ] = v21;
        Val[ M02 ] = v02;
        Val[ M12 ] = v12;
        Val[ M22 ] = v22;

        return this;
    }

    /// <summary>
    /// Sets this matrix to a rotation matrix that will rotate any vector in
    /// counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="degrees"> the angle in degrees.</param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToRotation( float degrees )
    {
        return SetToRotationRad( MathUtils.DEGREES_TO_RADIANS * degrees );
    }

    /// <summary>
    /// Sets this matrix to a rotation matrix that will rotate any vector in
    /// counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="radians"> the angle in radians.</param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToRotationRad( float radians )
    {
        var cos = ( float ) Math.Cos( radians );
        var sin = ( float ) Math.Sin( radians );

        Val[ M00 ] = cos;
        Val[ M10 ] = sin;
        Val[ M20 ] = 0;

        Val[ M01 ] = -sin;
        Val[ M11 ] = cos;
        Val[ M21 ] = 0;

        Val[ M02 ] = 0;
        Val[ M12 ] = 0;
        Val[ M22 ] = 1;

        return this;
    }

    public Matrix3 SetToRotation( Vector3 axis, float degrees )
    {
        return SetToRotation( axis, MathUtils.CosDeg( degrees ), MathUtils.SinDeg( degrees ) );
    }

    public Matrix3 SetToRotation( Vector3 axis, float cos, float sin )
    {
        var oc = 1.0f - cos;

        Val[ M00 ] = ( oc * axis.X * axis.X ) + cos;
        Val[ M01 ] = ( oc * axis.X * axis.Y ) - ( axis.Z * sin );
        Val[ M02 ] = ( oc * axis.Z * axis.X ) + ( axis.Y * sin );
        Val[ M10 ] = ( oc * axis.X * axis.Y ) + ( axis.Z * sin );
        Val[ M11 ] = ( oc * axis.Y * axis.Y ) + cos;
        Val[ M12 ] = ( oc * axis.Y * axis.Z ) - ( axis.X * sin );
        Val[ M20 ] = ( oc * axis.Z * axis.X ) - ( axis.Y * sin );
        Val[ M21 ] = ( oc * axis.Y * axis.Z ) + ( axis.X * sin );
        Val[ M22 ] = ( oc * axis.Z * axis.Z ) + cos;

        return this;
    }

    /// Sets this matrix to a translation matrix.
    /// <param name="x"> the translation in x</param>
    /// <param name="y"> the translation in y</param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToTranslation( float x, float y )
    {
        Val[ M00 ] = 1;
        Val[ M10 ] = 0;
        Val[ M20 ] = 0;

        Val[ M01 ] = 0;
        Val[ M11 ] = 1;
        Val[ M21 ] = 0;

        Val[ M02 ] = x;
        Val[ M12 ] = y;
        Val[ M22 ] = 1;

        return this;
    }

    /// Sets this matrix to a translation matrix.
    /// <param name="translation"> The translation vector.</param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToTranslation( Vector2 translation )
    {
        Val[ M00 ] = 1;
        Val[ M10 ] = 0;
        Val[ M20 ] = 0;

        Val[ M01 ] = 0;
        Val[ M11 ] = 1;
        Val[ M21 ] = 0;

        Val[ M02 ] = translation.X;
        Val[ M12 ] = translation.Y;
        Val[ M22 ] = 1;

        return this;
    }

    /// Sets this matrix to a scaling matrix.
    /// <param name="scaleX"> the scale in x </param>
    /// <param name="scaleY"> the scale in y </param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToScaling( float scaleX, float scaleY )
    {
        Val[ M00 ] = scaleX;
        Val[ M10 ] = 0;
        Val[ M20 ] = 0;
        Val[ M01 ] = 0;
        Val[ M11 ] = scaleY;
        Val[ M21 ] = 0;
        Val[ M02 ] = 0;
        Val[ M12 ] = 0;
        Val[ M22 ] = 1;

        return this;
    }

    /// Sets this matrix to a scaling matrix.
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 SetToScaling( Vector2 scale )
    {
        Val[ M00 ] = scale.X;
        Val[ M10 ] = 0;
        Val[ M20 ] = 0;
        Val[ M01 ] = 0;
        Val[ M11 ] = scale.Y;
        Val[ M21 ] = 0;
        Val[ M02 ] = 0;
        Val[ M12 ] = 0;
        Val[ M22 ] = 1;

        return this;
    }

    public override string ToString()
    {
        return $"[{Val[ M00 ]} | {Val[ M01 ]} | {Val[ M02 ]}]\n"
             + $"[{Val[ M10 ]} | {Val[ M11 ]} | {Val[ M12 ]}]\n"
             + $"[{Val[ M20 ]} | {Val[ M21 ]} | {Val[ M22 ]}]";
    }

    /// <returns> The determinant of this matrix </returns>
    public float Det()
    {
        return ( ( Val[ M00 ] * Val[ M11 ] * Val[ M22 ] )
               + ( Val[ M01 ] * Val[ M12 ] * Val[ M20 ] )
               + ( Val[ M02 ] * Val[ M10 ] * Val[ M21 ] ) )
             - ( Val[ M00 ] * Val[ M12 ] * Val[ M21 ] )
             - ( Val[ M01 ] * Val[ M10 ] * Val[ M22 ] )
             - ( Val[ M02 ] * Val[ M11 ] * Val[ M20 ] );
    }

    /// <summary>
    /// Inverts this matrix given that the determinant is != 0.
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

        _tmp[ M00 ] = ( Val[ M11 ] * Val[ M22 ] ) - ( Val[ M21 ] * Val[ M12 ] );
        _tmp[ M10 ] = ( Val[ M20 ] * Val[ M12 ] ) - ( Val[ M10 ] * Val[ M22 ] );
        _tmp[ M20 ] = ( Val[ M10 ] * Val[ M21 ] ) - ( Val[ M20 ] * Val[ M11 ] );
        _tmp[ M01 ] = ( Val[ M21 ] * Val[ M02 ] ) - ( Val[ M01 ] * Val[ M22 ] );
        _tmp[ M11 ] = ( Val[ M00 ] * Val[ M22 ] ) - ( Val[ M20 ] * Val[ M02 ] );
        _tmp[ M21 ] = ( Val[ M20 ] * Val[ M01 ] ) - ( Val[ M00 ] * Val[ M21 ] );
        _tmp[ M02 ] = ( Val[ M01 ] * Val[ M12 ] ) - ( Val[ M11 ] * Val[ M02 ] );
        _tmp[ M12 ] = ( Val[ M10 ] * Val[ M02 ] ) - ( Val[ M00 ] * Val[ M12 ] );
        _tmp[ M22 ] = ( Val[ M00 ] * Val[ M11 ] ) - ( Val[ M10 ] * Val[ M01 ] );

        Val[ M00 ] = invDet * _tmp[ M00 ];
        Val[ M10 ] = invDet * _tmp[ M10 ];
        Val[ M20 ] = invDet * _tmp[ M20 ];
        Val[ M01 ] = invDet * _tmp[ M01 ];
        Val[ M11 ] = invDet * _tmp[ M11 ];
        Val[ M21 ] = invDet * _tmp[ M21 ];
        Val[ M02 ] = invDet * _tmp[ M02 ];
        Val[ M12 ] = invDet * _tmp[ M12 ];
        Val[ M22 ] = invDet * _tmp[ M22 ];

        return this;
    }

    /// Copies the values from the provided matrix to this matrix.
    /// <param name="mat"> The matrix to copy. </param>
    /// <returns> This matrix for the purposes of chaining. </returns>
    public Matrix3 Set( Matrix3 mat )
    {
        Array.Copy( mat.Val, 0, Val, 0, Val.Length );

        return this;
    }

    /// <summary>
    /// Copies the values from the provided affine matrix to this matrix.
    /// The last row is set to (0, 0, 1).
    /// </summary>
    /// <param name="affine"> The affine matrix to copy. </param>
    /// <returns> This matrix for the purposes of chaining. </returns>
    public Matrix3 Set( Affine2 affine )
    {
        Val[ M00 ] = affine.M00;
        Val[ M10 ] = affine.M10;
        Val[ M20 ] = 0;
        Val[ M01 ] = affine.M01;
        Val[ M11 ] = affine.M11;
        Val[ M21 ] = 0;
        Val[ M02 ] = affine.M02;
        Val[ M12 ] = affine.M12;
        Val[ M22 ] = 1;

        return this;
    }

    /// Sets this 3x3 matrix to the top left 3x3 corner of the provided 4x4 matrix.
    /// <param name="mat">
    /// The matrix whose top left corner will be copied. This matrix will not be modified.
    /// </param>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    public Matrix3 Set( Matrix4 mat )
    {
        Val[ M00 ] = mat.Val[ Matrix4.M00 ];
        Val[ M10 ] = mat.Val[ Matrix4.M10 ];
        Val[ M20 ] = mat.Val[ Matrix4.M20 ];
        Val[ M01 ] = mat.Val[ Matrix4.M01 ];
        Val[ M11 ] = mat.Val[ Matrix4.M11 ];
        Val[ M21 ] = mat.Val[ Matrix4.M21 ];
        Val[ M02 ] = mat.Val[ Matrix4.M02 ];
        Val[ M12 ] = mat.Val[ Matrix4.M12 ];
        Val[ M22 ] = mat.Val[ Matrix4.M22 ];

        return this;
    }

    /// <summary>
    /// Sets the matrix to the given matrix as a float array. The float array
    /// must have at least 9 elements; the first 9 will be copied.
    /// </summary>
    /// <param name="values">
    /// The matrix, in float form, that is to be copied. Remember that this matrix is in
    /// <a href="http://en.wikipedia.org/wiki/Row-major_order#Column-major_order">column major</a>
    /// order.
    /// </param>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    public Matrix3 Set( float[] values )
    {
        Array.Copy( values, 0, Val, 0, Val.Length );

        return this;
    }

    /// <summary>
    /// Adds a translational component to the matrix in the 3rd column. The other columns are untouched.
    /// </summary>
    /// <param name="vector"> The translation vector. </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Trn( Vector2 vector )
    {
        Val[ M02 ] += vector.X;
        Val[ M12 ] += vector.Y;

        return this;
    }

    /// <summary>
    /// Adds a translational component to the matrix in the 3rd column. The other columns are untouched.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Trn( float x, float y )
    {
        Val[ M02 ] += x;
        Val[ M12 ] += y;

        return this;
    }

    /// <summary>
    /// Adds a translational component to the matrix in the 3rd column. The other columns are untouched.
    /// </summary>
    /// <param name="vector">
    /// The translation vector. (The z-component of the vector is ignored because this is a 3x3 matrix)
    /// </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Trn( Vector3 vector )
    {
        Val[ M02 ] += vector.X;
        Val[ M12 ] += vector.Y;

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix by a translation matrix. Postmultiplication is also used by OpenGL ES' 1.x
    /// glTranslate/glRotate/glScale.
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

        Mul( Val, _tmp );

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix by a translation matrix. Postmultiplication is also used by OpenGL ES' 1.x
    /// glTranslate/glRotate/glScale.
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

        Mul( Val, _tmp );

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix with a (counter-clockwise) rotation matrix. Postmultiplication is also used by OpenGL
    /// ES' 1.x
    /// glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="degrees"> The angle in degrees </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 Rotate( float degrees )
    {
        return RotateRad( MathUtils.DEGREES_TO_RADIANS * degrees );
    }

    /// <summary>
    /// Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    /// Postmultiplication is also used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
    /// </summary>
    /// <param name="radians"> The angle in radians </param>
    /// <returns> This matrix for the purpose of chaining. </returns>
    public Matrix3 RotateRad( float radians )
    {
        if ( radians == 0 )
        {
            return this;
        }

        var cos = ( float ) Math.Cos( radians );
        var sin = ( float ) Math.Sin( radians );

        _tmp[ M00 ] = cos;
        _tmp[ M10 ] = sin;
        _tmp[ M20 ] = 0;

        _tmp[ M01 ] = -sin;
        _tmp[ M11 ] = cos;
        _tmp[ M21 ] = 0;

        _tmp[ M02 ] = 0;
        _tmp[ M12 ] = 0;
        _tmp[ M22 ] = 1;

        Mul( Val, _tmp );

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix with a scale matrix. Postmultiplication is also
    /// used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
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

        Mul( Val, _tmp );

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix with a scale matrix. Postmultiplication is also
    /// used by OpenGL ES' 1.x glTranslate/glRotate/glScale.
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

        Mul( Val, _tmp );

        return this;
    }

    /// <summary>
    /// Get the values in this matrix.
    /// </summary>
    /// <returns> The float values that make up this matrix in column-major order. </returns>
    public float[] GetValues()
    {
        return Val;
    }

    public Vector2 GetTranslation( Vector2 position )
    {
        position.X = Val[ M02 ];
        position.Y = Val[ M12 ];

        return position;
    }

    /// <param name="scale">
    /// The vector which will receive the (non-negative) scale components on each axis.
    /// </param>
    /// <returns> The provided vector for chaining. </returns>
    public Vector2 GetScale( Vector2 scale )
    {
        scale.X = ( float ) Math.Sqrt( ( Val[ M00 ] * Val[ M00 ] ) + ( Val[ M01 ] * Val[ M01 ] ) );
        scale.Y = ( float ) Math.Sqrt( ( Val[ M10 ] * Val[ M10 ] ) + ( Val[ M11 ] * Val[ M11 ] ) );

        return scale;
    }

    public float GetRotation()
    {
        return MathUtils.RADIANS_TO_DEGREES * ( float ) Math.Atan2( Val[ M10 ], Val[ M00 ] );
    }

    public float GetRotationRad()
    {
        return ( float ) Math.Atan2( Val[ M10 ], Val[ M00 ] );
    }

    /// <summary>
    /// Scale the matrix in the both the x and y components by the scalar value.
    /// </summary>
    /// <param name="scale"> The single value that will be used to scale both the x and y components. </param>
    /// <returns> This matrix for the purpose of chaining methods together.  </returns>
    public Matrix3 Scl( float scale )
    {
        Val[ M00 ] *= scale;
        Val[ M11 ] *= scale;

        return this;
    }

    /// <summary>
    /// Scale this matrix using the x and y components of the vector
    /// but leave the rest of the matrix alone.
    /// </summary>
    /// <param name="scale"> The <see cref="Vector3"/> to use to scale this matrix. </param>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    public Matrix3 Scl( Vector2 scale )
    {
        Val[ M00 ] *= scale.X;
        Val[ M11 ] *= scale.Y;

        return this;
    }

    /// <summary>
    /// Scale this matrix using the x and y components of the vector but
    /// leave the rest of the matrix alone.
    /// </summary>
    /// <param name="scale">
    /// The <see cref="Vector3"/> to use to scale this matrix.
    /// The z component will be ignored.
    /// </param>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    public Matrix3 Scl( Vector3 scale )
    {
        Val[ M00 ] *= scale.X;
        Val[ M11 ] *= scale.Y;

        return this;
    }

    /// <summary>
    /// Transposes the current matrix.
    /// </summary>
    /// <returns> This matrix for the purpose of chaining methods together. </returns>
    public Matrix3 Transpose()
    {
        ( Val[ M01 ], Val[ M10 ] ) = ( Val[ M10 ], Val[ M01 ] );
        ( Val[ M02 ], Val[ M20 ] ) = ( Val[ M20 ], Val[ M02 ] );
        ( Val[ M12 ], Val[ M21 ] ) = ( Val[ M12 ], Val[ M21 ] );

        return this;
    }

    /// Multiplies matrix a with matrix b in the following manner:
    /// <para>
    ///     <tt>mul(A, B) => A := AB</tt>
    /// </para>
    /// <param name="mata">
    /// The float array representing the first matrix. Must have at least 9 elements.
    /// </param>
    /// <param name="matb">
    /// The float array representing the second matrix. Must have at least 9 elements.
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
