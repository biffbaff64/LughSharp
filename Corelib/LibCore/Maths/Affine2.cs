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

using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Maths;

/// <summary>
/// A specialized 3x3 matrix that can represent sequences of 2D translations,
/// scales, flips, rotations, and shears. Affine transformations  preserve
/// straight lines, and parallel lines remain parallel after the transformation.
/// Operations on affine matrices are faster because the last row can always be
/// assumed (0, 0, 1).
/// </summary>
[PublicAPI]
public class Affine2
{
    public float M00 = 1;
    public float M01 = 0;
    public float M02 = 0;
    public float M10 = 0;
    public float M11 = 1;
    public float M12 = 0;

    // constant: m21 = 0, m21 = 1, m22 = 1

    /// <summary>
    /// Constructs an identity matrix.
    /// </summary>
    public Affine2()
    {
    }

    /// <summary>
    /// Constructs a matrix from the given affine matrix.
    /// </summary>
    /// <param name="other">The affine matrix to copy. This matrix will not be modified.</param>
    public Affine2( Affine2 other )
    {
        SetFrom( other );
    }

    /// <summary>
    /// Sets this matrix to the identity matrix.
    /// </summary>
    /// <returns>This matrix for the purpose of chaining operations.</returns>
    public Affine2 ToIdentityMatrix()
    {
        M00 = 1;
        M01 = 0;
        M02 = 0;
        M10 = 0;
        M11 = 1;
        M12 = 0;

        return this;
    }

    /// <summary>
    /// Copies the values from the provided affine matrix to this matrix.
    /// </summary>
    /// <param name="other">The affine matrix to copy.</param>
    /// <returns>This matrix for the purposes of chaining.</returns>
    public Affine2 SetFrom( Affine2 other )
    {
        M00 = other.M00;
        M01 = other.M01;
        M02 = other.M02;
        M10 = other.M10;
        M11 = other.M11;
        M12 = other.M12;

        return this;
    }

    /// <summary>
    /// Copies the values from the provided matrix to this matrix.
    /// </summary>
    /// <param name="matrix"> The matrix to copy, assumed to be an affine transformation. </param>
    /// <returns> This matrix for the purposes of chaining.  </returns>
    public Affine2 SetFrom( Matrix3 matrix )
    {
        var other = matrix.Val;

        M00 = other[ Matrix3.M00 ];
        M01 = other[ Matrix3.M01 ];
        M02 = other[ Matrix3.M02 ];
        M10 = other[ Matrix3.M10 ];
        M11 = other[ Matrix3.M11 ];
        M12 = other[ Matrix3.M12 ];

        return this;
    }

    /// <summary>
    /// Copies the 2D transformation components from the provided 4x4 matrix.
    /// The values are mapped as follows:
    /// <pre>
    /// [  M00  M01  M03  ]
    /// [  M10  M11  M13  ]
    /// [   0    0    1   ]
    /// </pre>
    /// </summary>
    /// <param name="matrix">
    /// The source matrix, assumed to be an affine transformation within XY plane.
    /// This matrix will not be modified.
    /// </param>
    /// <returns>This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetFrom( Matrix4 matrix )
    {
        var other = matrix.Val;

        M00 = other[ Matrix4.M00 ];
        M01 = other[ Matrix4.M01 ];
        M02 = other[ Matrix4.M03 ];
        M10 = other[ Matrix4.M10 ];
        M11 = other[ Matrix4.M11 ];
        M12 = other[ Matrix4.M13 ];

        return this;
    }

    /// <summary>
    /// Sets this matrix to a translation matrix.
    /// </summary>
    /// <param name="x"> The translation in x </param>
    /// <param name="y"> The translation in y </param>
    /// <returns> This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetToTranslation( float x, float y )
    {
        M00 = 1;
        M01 = 0;
        M02 = x;
        M10 = 0;
        M11 = 1;
        M12 = y;

        return this;
    }

    /// <summary>
    /// Sets this matrix to a translation matrix.
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetToTranslation( Vector2 trn )
    {
        return SetToTranslation( trn.X, trn.Y );
    }

    /// <summary>
    /// Sets this matrix to a scaling matrix.
    /// </summary>
    /// <param name="scaleX"> The scale in x. </param>
    /// <param name="scaleY"> The scale in y. </param>
    /// <returns> This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetToScaling( float scaleX, float scaleY )
    {
        M00 = scaleX;
        M01 = 0;
        M02 = 0;
        M10 = 0;
        M11 = scaleY;
        M12 = 0;

        return this;
    }

    /// <summary>
    /// Sets this matrix to a scaling matrix.
    /// </summary>
    /// <param name="scale">The scale vector.</param>
    /// <returns>This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetToScaling( Vector2 scale )
    {
        return SetToScaling( scale.X, scale.Y );
    }

    /// <summary>
    /// Sets this matrix to a rotation matrix that will rotate any vector in
    /// counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="degrees"> The angle in degrees. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToRotation( float degrees )
    {
        var cos = MathUtils.CosDeg( degrees );
        var sin = MathUtils.SinDeg( degrees );

        M00 = cos;
        M01 = -sin;
        M02 = 0;
        M10 = sin;
        M11 = cos;
        M12 = 0;

        return this;
    }

    /// <summary>
    /// Sets this matrix to a rotation matrix that will rotate any vector in
    /// counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="radians"> The angle in radians. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToRotationRad( float radians )
    {
        var cos = MathUtils.Cos( radians );
        var sin = MathUtils.Sin( radians );

        M00 = cos;
        M01 = -sin;
        M02 = 0;
        M10 = sin;
        M11 = cos;
        M12 = 0;

        return this;
    }

    /// <summary>
    /// Sets this matrix to a rotation matrix that will rotate any vector in
    /// counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="cos"> The angle cosine. </param>
    /// <param name="sin"> The angle sine. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToRotation( float cos, float sin )
    {
        M00 = cos;
        M01 = -sin;
        M02 = 0;
        M10 = sin;
        M11 = cos;
        M12 = 0;

        return this;
    }

    /// <summary>
    /// Sets this matrix to a shearing matrix.
    /// </summary>
    /// <param name="shearX"> The shear in x direction. </param>
    /// <param name="shearY"> The shear in y direction. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToShearing( float shearX, float shearY )
    {
        M00 = 1;
        M01 = shearX;
        M02 = 0;
        M10 = shearY;
        M11 = 1;
        M12 = 0;

        return this;
    }

    /// <summary>
    /// Sets this matrix to a shearing matrix.
    /// </summary>
    /// <param name="shear"> The shear vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToShearing( Vector2 shear )
    {
        return SetToShearing( shear.X, shear.Y );
    }

    /// <summary>
    /// Sets this matrix to a concatenation of translation, rotation and scale.
    /// It is a more efficient form for:
    /// <code>idt().translate(x, y).rotate(degrees).scale(scaleX, scaleY)</code>
    /// </summary>
    /// <param name="x"> The translation in x. </param>
    /// <param name="y"> The translation in y. </param>
    /// <param name="degrees"> The angle in degrees. </param>
    /// <param name="scaleX"> The scale in y. </param>
    /// <param name="scaleY"> The scale in x. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnRotScl( float x, float y, float degrees, float scaleX, float scaleY )
    {
        M02 = x;
        M12 = y;

        if ( degrees == 0 )
        {
            M00 = scaleX;
            M01 = 0;
            M10 = 0;
            M11 = scaleY;
        }
        else
        {
            var sin = MathUtils.SinDeg( degrees );
            var cos = MathUtils.CosDeg( degrees );

            M00 = cos * scaleX;
            M01 = -sin * scaleY;
            M10 = sin * scaleX;
            M11 = cos * scaleY;
        }

        return this;
    }

    /// <summary>
    /// Sets this matrix to a concatenation of translation, rotation and scale.
    /// It is a more efficient form for:
    /// <code>idt().translate(trn).rotate(degrees).scale(scale)</code>
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <param name="degrees"> The angle in degrees. </param>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnRotScl( Vector2 trn, float degrees, Vector2 scale )
    {
        return SetToTrnRotScl( trn.X, trn.Y, degrees, scale.X, scale.Y );
    }

    /// <summary>
    /// Sets this matrix to a concatenation of translation, rotation and scale.
    /// It is a more efficient form for:
    /// <code>idt().translate(x, y).rotateRad(radians).scale(scaleX, scaleY)</code>
    /// </summary>
    /// <param name="x"> The translation in x. </param>
    /// <param name="y"> The translation in y. </param>
    /// <param name="radians"> The angle in radians. </param>
    /// <param name="scaleX"> The scale in y. </param>
    /// <param name="scaleY"> The scale in x. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnRotRadScl( float x, float y, float radians, float scaleX, float scaleY )
    {
        M02 = x;
        M12 = y;

        if ( radians == 0 )
        {
            M00 = scaleX;
            M01 = 0;
            M10 = 0;
            M11 = scaleY;
        }
        else
        {
            var sin = MathUtils.Sin( radians );
            var cos = MathUtils.Cos( radians );

            M00 = cos * scaleX;
            M01 = -sin * scaleY;
            M10 = sin * scaleX;
            M11 = cos * scaleY;
        }

        return this;
    }

    /// <summary>
    /// Sets this matrix to a concatenation of translation, rotation and scale. It is a more efficient form for:
    /// <code>idt().translate(trn).rotateRad(radians).scale(scale)</code>
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <param name="radians"> The angle in radians. </param>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnRotRadScl( Vector2 trn, float radians, Vector2 scale )
    {
        return SetToTrnRotRadScl( trn.X, trn.Y, radians, scale.X, scale.Y );
    }

    /// <summary>
    /// Sets this matrix to a concatenation of translation and scale. It is a more efficient form for:
    /// <code>idt().translate(x, y).scale(scaleX, scaleY)</code>
    /// </summary>
    /// <param name="x"> The translation in x. </param>
    /// <param name="y"> The translation in y. </param>
    /// <param name="scaleX"> The scale in y. </param>
    /// <param name="scaleY"> The scale in x. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnScl( float x, float y, float scaleX, float scaleY )
    {
        M00 = scaleX;
        M01 = 0;
        M02 = x;
        M10 = 0;
        M11 = scaleY;
        M12 = y;

        return this;
    }

    /// <summary>
    /// Sets this matrix to a concatenation of translation and scale. It is a more efficient form for:
    /// <code>idt().translate(trn).scale(scale)</code>
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnScl( Vector2 trn, Vector2 scale )
    {
        return SetToTrnScl( trn.X, trn.Y, scale.X, scale.Y );
    }

    /// <summary>
    /// Sets this matrix to the product of two matrices.
    /// </summary>
    /// <param name="l"> Left matrix. </param>
    /// <param name="r"> Right matrix. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToProduct( Affine2 l, Affine2 r )
    {
        M00 = ( l.M00 * r.M00 ) + ( l.M01 * r.M10 );
        M01 = ( l.M00 * r.M01 ) + ( l.M01 * r.M11 );
        M02 = ( l.M00 * r.M02 ) + ( l.M01 * r.M12 ) + l.M02;
        M10 = ( l.M10 * r.M00 ) + ( l.M11 * r.M10 );
        M11 = ( l.M10 * r.M01 ) + ( l.M11 * r.M11 );
        M12 = ( l.M10 * r.M02 ) + ( l.M11 * r.M12 ) + l.M12;

        return this;
    }

    /// <summary>
    /// Inverts this matrix given that the determinant is != 0.
    /// </summary>
    /// <returns> This matrix for the purpose of chaining operations. </returns>
    /// <exception cref="GdxRuntimeException"> if the matrix is singular (not invertible)  </exception>
    public Affine2 Invert()
    {
        var det = GetDeterminant();

        if ( det == 0 )
        {
            throw new GdxRuntimeException( "Can't invert a singular affine matrix" );
        }

        var invDet = 1.0f / det;

        var tmp00 = M11;
        var tmp01 = -M01;
        var tmp02 = ( M01 * M12 ) - ( M11 * M02 );
        var tmp10 = -M10;
        var tmp11 = M00;
        var tmp12 = ( M10 * M02 ) - ( M00 * M12 );

        M00 = invDet * tmp00;
        M01 = invDet * tmp01;
        M02 = invDet * tmp02;
        M10 = invDet * tmp10;
        M11 = invDet * tmp11;
        M12 = invDet * tmp12;

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix with the provided matrix and stores the result
    /// in this matrix. For example: A.mul(B) results in A := AB
    /// </summary>
    /// <param name="other"> Matrix to multiply by. </param>
    /// <returns> This matrix for the purpose of chaining operations together.</returns>
    public Affine2 Mul( Affine2 other )
    {
        var tmp00 = ( M00 * other.M00 ) + ( M01 * other.M10 );
        var tmp01 = ( M00 * other.M01 ) + ( M01 * other.M11 );
        var tmp02 = ( M00 * other.M02 ) + ( M01 * other.M12 ) + M02;
        var tmp10 = ( M10 * other.M00 ) + ( M11 * other.M10 );
        var tmp11 = ( M10 * other.M01 ) + ( M11 * other.M11 );
        var tmp12 = ( M10 * other.M02 ) + ( M11 * other.M12 ) + M12;

        M00 = tmp00;
        M01 = tmp01;
        M02 = tmp02;
        M10 = tmp10;
        M11 = tmp11;
        M12 = tmp12;

        return this;
    }

    /// <summary>
    /// Premultiplies this matrix with the provided matrix and stores the result in this matrix. For example:
    /// <pre>
    /// A.preMul(B) results in A := BA
    /// </pre>
    /// </summary>
    /// <param name="other"> The other Matrix to multiply by </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 PreMul( Affine2 other )
    {
        var tmp00 = ( other.M00 * M00 ) + ( other.M01 * M10 );
        var tmp01 = ( other.M00 * M01 ) + ( other.M01 * M11 );
        var tmp02 = ( other.M00 * M02 ) + ( other.M01 * M12 ) + other.M02;
        var tmp10 = ( other.M10 * M00 ) + ( other.M11 * M10 );
        var tmp11 = ( other.M10 * M01 ) + ( other.M11 * M11 );
        var tmp12 = ( other.M10 * M02 ) + ( other.M11 * M12 ) + other.M12;

        M00 = tmp00;
        M01 = tmp01;
        M02 = tmp02;
        M10 = tmp10;
        M11 = tmp11;
        M12 = tmp12;

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix by a translation matrix.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Translate( float x, float y )
    {
        M02 += ( M00 * x ) + ( M01 * y );
        M12 += ( M10 * x ) + ( M11 * y );

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix by a translation matrix.
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Translate( Vector2 trn )
    {
        return Translate( trn.X, trn.Y );
    }

    /// <summary>
    /// Premultiplies this matrix by a translation matrix.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreTranslate( float x, float y )
    {
        M02 += x;
        M12 += y;

        return this;
    }

    /// <summary>
    /// Premultiplies this matrix by a translation matrix.
    /// <param name="trn">The translation vector.</param>
    /// <returns>This matrix for the purpose of chaining.</returns>
    /// </summary>
    public Affine2 PreTranslate( Vector2 trn )
    {
        return PreTranslate( trn.X, trn.Y );
    }

    /// <summary>
    /// Postmultiplies this matrix with a scale matrix.
    /// </summary>
    /// <param name="scaleX"> The scale in the x-axis. </param>
    /// <param name="scaleY"> The scale in the y-axis. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Scale( float scaleX, float scaleY )
    {
        M00 *= scaleX;
        M01 *= scaleY;
        M10 *= scaleX;
        M11 *= scaleY;

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix with a scale matrix.
    /// </summary>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Scale( Vector2 scale )
    {
        return Scale( scale.X, scale.Y );
    }

    /// <summary>
    /// Premultiplies this matrix with a scale matrix.
    /// </summary>
    /// <param name="scaleX"> The scale in the x-axis. </param>
    /// <param name="scaleY"> The scale in the y-axis. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreScale( float scaleX, float scaleY )
    {
        M00 *= scaleX;
        M01 *= scaleX;
        M02 *= scaleX;
        M10 *= scaleY;
        M11 *= scaleY;
        M12 *= scaleY;

        return this;
    }

    /// <summary>
    /// Premultiplies this matrix with a scale matrix.
    /// </summary>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreScale( Vector2 scale )
    {
        return PreScale( scale.X, scale.Y );
    }

    /// <summary>
    /// Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    /// </summary>
    /// <param name="degrees"> The angle in degrees </param>
    /// <returns>
    /// This matrix for the purpose of chaining.
    /// </returns>
    public Affine2 Rotate( float degrees )
    {
        if ( degrees == 0 )
        {
            return this;
        }

        var cos = MathUtils.CosDeg( degrees );
        var sin = MathUtils.SinDeg( degrees );

        var tmp00 = ( M00 * cos ) + ( M01 * sin );
        var tmp01 = ( M00 * -sin ) + ( M01 * cos );
        var tmp10 = ( M10 * cos ) + ( M11 * sin );
        var tmp11 = ( M10 * -sin ) + ( M11 * cos );

        M00 = tmp00;
        M01 = tmp01;
        M10 = tmp10;
        M11 = tmp11;

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    /// </summary>
    /// <param name="radians"> The angle in radians </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 RotateRad( float radians )
    {
        if ( radians == 0 )
        {
            return this;
        }

        var cos = MathUtils.Cos( radians );
        var sin = MathUtils.Sin( radians );

        var tmp00 = ( M00 * cos ) + ( M01 * sin );
        var tmp01 = ( M00 * -sin ) + ( M01 * cos );
        var tmp10 = ( M10 * cos ) + ( M11 * sin );
        var tmp11 = ( M10 * -sin ) + ( M11 * cos );

        M00 = tmp00;
        M01 = tmp01;
        M10 = tmp10;
        M11 = tmp11;

        return this;
    }

    /// <summary>
    /// Premultiplies this matrix with a (counter-clockwise) rotation matrix.
    /// </summary>
    /// <param name="degrees"> The angle in degrees </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreRotate( float degrees )
    {
        if ( degrees == 0 )
        {
            return this;
        }

        var cos = MathUtils.CosDeg( degrees );
        var sin = MathUtils.SinDeg( degrees );

        var tmp00 = ( cos * M00 ) - ( sin * M10 );
        var tmp01 = ( cos * M01 ) - ( sin * M11 );
        var tmp02 = ( cos * M02 ) - ( sin * M12 );
        var tmp10 = ( sin * M00 ) + ( cos * M10 );
        var tmp11 = ( sin * M01 ) + ( cos * M11 );
        var tmp12 = ( sin * M02 ) + ( cos * M12 );

        M00 = tmp00;
        M01 = tmp01;
        M02 = tmp02;
        M10 = tmp10;
        M11 = tmp11;
        M12 = tmp12;

        return this;
    }

    /// <summary>
    /// Premultiplies this matrix with a (counter-clockwise) rotation matrix.
    /// </summary>
    /// <param name="radians"> The angle in radians </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreRotateRad( float radians )
    {
        if ( radians == 0 )
        {
            return this;
        }

        var cos = MathUtils.Cos( radians );
        var sin = MathUtils.Sin( radians );

        var tmp00 = ( cos * M00 ) - ( sin * M10 );
        var tmp01 = ( cos * M01 ) - ( sin * M11 );
        var tmp02 = ( cos * M02 ) - ( sin * M12 );
        var tmp10 = ( sin * M00 ) + ( cos * M10 );
        var tmp11 = ( sin * M01 ) + ( cos * M11 );
        var tmp12 = ( sin * M02 ) + ( cos * M12 );

        M00 = tmp00;
        M01 = tmp01;
        M02 = tmp02;
        M10 = tmp10;
        M11 = tmp11;
        M12 = tmp12;

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix by a shear matrix.
    /// </summary>
    /// <param name="shearX"> The shear in x direction.</param>
    /// <param name="shearY"> The shear in y direction.</param>
    /// <returns> This matrix for the purpose of chaining.</returns>
    public Affine2 Shear( float shearX, float shearY )
    {
        var tmp0 = M00 + ( shearY * M01 );
        var tmp1 = M01 + ( shearX * M00 );

        M00 = tmp0;
        M01 = tmp1;

        tmp0 = M10 + ( shearY * M11 );
        tmp1 = M11 + ( shearX * M10 );

        M10 = tmp0;
        M11 = tmp1;

        return this;
    }

    /// <summary>
    /// Postmultiplies this matrix by a shear matrix.
    /// </summary>
    /// <param name="shear"> The shear vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Shear( Vector2 shear )
    {
        return Shear( shear.X, shear.Y );
    }

    /// <summary>
    /// Premultiplies this matrix by a shear matrix.
    /// </summary>
    /// <param name="shearX"> The shear in x direction. </param>
    /// <param name="shearY"> The shear in y direction. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreShear( float shearX, float shearY )
    {
        var tmp00 = M00 + ( shearX * M10 );
        var tmp01 = M01 + ( shearX * M11 );
        var tmp02 = M02 + ( shearX * M12 );
        var tmp10 = M10 + ( shearY * M00 );
        var tmp11 = M11 + ( shearY * M01 );
        var tmp12 = M12 + ( shearY * M02 );

        M00 = tmp00;
        M01 = tmp01;
        M02 = tmp02;
        M10 = tmp10;
        M11 = tmp11;
        M12 = tmp12;

        return this;
    }

    /// <summary>
    /// Premultiplies this matrix by a shear matrix.
    /// </summary>
    /// <param name="shear"> The shear vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreShear( Vector2 shear )
    {
        return PreShear( shear.X, shear.Y );
    }

    /// <summary>
    /// Calculates the determinant of the matrix.
    /// </summary>
    /// <returns> The determinant of this matrix.</returns>
    public float GetDeterminant()
    {
        return ( M00 * M11 ) - ( M01 * M10 );
    }

    /// <summary>
    /// Get the x-y translation component of the matrix.
    /// </summary>
    /// <param name="position"> Output vector. </param>
    /// <returns> Filled position.  </returns>
    public Vector2 GetTranslation( Vector2 position )
    {
        position.X = M02;
        position.Y = M12;

        return position;
    }

    /// <summary>
    /// Check if the this is a plain translation matrix.
    /// </summary>
    /// <returns> True if scale is 1 and rotation is 0.</returns>
    public bool IsTranslation()
    {
        return M00 is 1f
            && M11 is 1
            && M01 is 0
            && M10 is 0;
    }

    /// <summary>
    /// Check if this is an indentity matrix.
    /// </summary>
    /// <returns> True if scale is 1 and rotation is 0.  </returns>
    public bool IsIdentityMatrix()
    {
        return M00 is 1
            && M02 is 0
            && M12 is 0
            && M11 is 1
            && M01 is 0
            && M10 is 0;
    }

    /// <summary>
    /// Applies the affine transformation on a vector.
    /// </summary>
    public void ApplyTo( Vector2 point )
    {
        var x = point.X;
        var y = point.Y;

        point.X = ( M00 * x ) + ( M01 * y ) + M02;
        point.Y = ( M10 * x ) + ( M11 * y ) + M12;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "[" + M00 + "|" + M01 + "|" + M02 + "]\n[" + M10 + "|" + M11 + "|" + M12 + "]\n[0.0|0.0|0.1]";
    }
}
