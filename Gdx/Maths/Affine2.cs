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


using LibGDXSharp.Gdx.Utils;

namespace LibGDXSharp.Gdx.Maths;

/// <summary>
///     A specialized 3x3 matrix that can represent sequences of 2D translations,
///     scales, flips, rotations, and shears. Affine transformations  preserve
///     straight lines, and parallel lines remain parallel after the transformation.
///     Operations on affine matrices are faster because the last row can always be
///     assumed (0, 0, 1).
/// </summary>
public class Affine2 // : ISerializable
{
    public float m00 = 1;
    public float m01 = 0;
    public float m02 = 0;
    public float m10 = 0;
    public float m11 = 1;
    public float m12 = 0;

    // constant: m21 = 0, m21 = 1, m22 = 1

    /// <summary>
    ///     Constructs an identity matrix.
    /// </summary>
    public Affine2()
    {
    }

    /// <summary>
    ///     Constructs a matrix from the given affine matrix.
    /// </summary>
    /// <param name="other">The affine matrix to copy. This matrix will not be modified.</param>
    public Affine2( Affine2 other ) => SetFrom( other );

    /// <summary>
    ///     Sets this matrix to the identity matrix.
    /// </summary>
    /// <returns>This matrix for the purpose of chaining operations.</returns>
    public Affine2 ToIdentityMatrix()
    {
        m00 = 1;
        m01 = 0;
        m02 = 0;
        m10 = 0;
        m11 = 1;
        m12 = 0;

        return this;
    }

    /// <summary>
    ///     Copies the values from the provided affine matrix to this matrix.
    /// </summary>
    /// <param name="other">The affine matrix to copy.</param>
    /// <returns>This matrix for the purposes of chaining.</returns>
    public Affine2 SetFrom( Affine2 other )
    {
        m00 = other.m00;
        m01 = other.m01;
        m02 = other.m02;
        m10 = other.m10;
        m11 = other.m11;
        m12 = other.m12;

        return this;
    }

    /// <summary>
    ///     Copies the values from the provided matrix to this matrix.
    /// </summary>
    /// <param name="matrix"> The matrix to copy, assumed to be an affine transformation. </param>
    /// <returns> This matrix for the purposes of chaining.  </returns>
    public Affine2 SetFrom( Matrix3 matrix )
    {
        var other = matrix.val;

        m00 = other[ Matrix3.M00 ];
        m01 = other[ Matrix3.M01 ];
        m02 = other[ Matrix3.M02 ];
        m10 = other[ Matrix3.M10 ];
        m11 = other[ Matrix3.M11 ];
        m12 = other[ Matrix3.M12 ];

        return this;
    }

    /// <summary>
    ///     Copies the 2D transformation components from the provided 4x4 matrix.
    ///     The values are mapped as follows:
    ///     <pre>
    ///         [  M00  M01  M03  ]
    ///         [  M10  M11  M13  ]
    ///         [   0    0    1   ]
    ///     </pre>
    /// </summary>
    /// <param name="matrix">
    ///     The source matrix, assumed to be an affine transformation within XY plane.
    ///     This matrix will not be modified.
    /// </param>
    /// <returns>This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetFrom( Matrix4 matrix )
    {
        var other = matrix.val;

        m00 = other[ Matrix4.M00 ];
        m01 = other[ Matrix4.M01 ];
        m02 = other[ Matrix4.M03 ];
        m10 = other[ Matrix4.M10 ];
        m11 = other[ Matrix4.M11 ];
        m12 = other[ Matrix4.M13 ];

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a translation matrix.
    /// </summary>
    /// <param name="x"> The translation in x </param>
    /// <param name="y"> The translation in y </param>
    /// <returns> This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetToTranslation( float x, float y )
    {
        m00 = 1;
        m01 = 0;
        m02 = x;
        m10 = 0;
        m11 = 1;
        m12 = y;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a translation matrix.
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetToTranslation( Vector2 trn ) => SetToTranslation( trn.X, trn.Y );

    /// <summary>
    ///     Sets this matrix to a scaling matrix.
    /// </summary>
    /// <param name="scaleX"> The scale in x. </param>
    /// <param name="scaleY"> The scale in y. </param>
    /// <returns> This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetToScaling( float scaleX, float scaleY )
    {
        m00 = scaleX;
        m01 = 0;
        m02 = 0;
        m10 = 0;
        m11 = scaleY;
        m12 = 0;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a scaling matrix.
    /// </summary>
    /// <param name="scale">The scale vector.</param>
    /// <returns>This matrix for the purpose of chaining operations.</returns>
    public Affine2 SetToScaling( Vector2 scale ) => SetToScaling( scale.X, scale.Y );

    /// <summary>
    ///     Sets this matrix to a rotation matrix that will rotate any vector in
    ///     counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="degrees"> The angle in degrees. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToRotation( float degrees )
    {
        var cos = MathUtils.CosDeg( degrees );
        var sin = MathUtils.SinDeg( degrees );

        m00 = cos;
        m01 = -sin;
        m02 = 0;
        m10 = sin;
        m11 = cos;
        m12 = 0;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a rotation matrix that will rotate any vector in
    ///     counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="radians"> The angle in radians. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToRotationRad( float radians )
    {
        var cos = MathUtils.Cos( radians );
        var sin = MathUtils.Sin( radians );

        m00 = cos;
        m01 = -sin;
        m02 = 0;
        m10 = sin;
        m11 = cos;
        m12 = 0;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a rotation matrix that will rotate any vector in
    ///     counter-clockwise direction around the z-axis.
    /// </summary>
    /// <param name="cos"> The angle cosine. </param>
    /// <param name="sin"> The angle sine. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToRotation( float cos, float sin )
    {
        m00 = cos;
        m01 = -sin;
        m02 = 0;
        m10 = sin;
        m11 = cos;
        m12 = 0;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a shearing matrix.
    /// </summary>
    /// <param name="shearX"> The shear in x direction. </param>
    /// <param name="shearY"> The shear in y direction. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToShearing( float shearX, float shearY )
    {
        m00 = 1;
        m01 = shearX;
        m02 = 0;
        m10 = shearY;
        m11 = 1;
        m12 = 0;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a shearing matrix.
    /// </summary>
    /// <param name="shear"> The shear vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToShearing( Vector2 shear ) => SetToShearing( shear.X, shear.Y );

    /// <summary>
    ///     Sets this matrix to a concatenation of translation, rotation and scale.
    ///     It is a more efficient form for:
    ///     <code>idt().translate(x, y).rotate(degrees).scale(scaleX, scaleY)</code>
    /// </summary>
    /// <param name="x"> The translation in x. </param>
    /// <param name="y"> The translation in y. </param>
    /// <param name="degrees"> The angle in degrees. </param>
    /// <param name="scaleX"> The scale in y. </param>
    /// <param name="scaleY"> The scale in x. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnRotScl( float x, float y, float degrees, float scaleX, float scaleY )
    {
        m02 = x;
        m12 = y;

        if ( degrees == 0 )
        {
            m00 = scaleX;
            m01 = 0;
            m10 = 0;
            m11 = scaleY;
        }
        else
        {
            var sin = MathUtils.SinDeg( degrees );
            var cos = MathUtils.CosDeg( degrees );

            m00 = cos * scaleX;
            m01 = -sin * scaleY;
            m10 = sin * scaleX;
            m11 = cos * scaleY;
        }

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a concatenation of translation, rotation and scale.
    ///     It is a more efficient form for:
    ///     <code>idt().translate(trn).rotate(degrees).scale(scale)</code>
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <param name="degrees"> The angle in degrees. </param>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnRotScl( Vector2 trn, float degrees, Vector2 scale ) => SetToTrnRotScl( trn.X, trn.Y, degrees, scale.X, scale.Y );

    /// <summary>
    ///     Sets this matrix to a concatenation of translation, rotation and scale.
    ///     It is a more efficient form for:
    ///     <code>idt().translate(x, y).rotateRad(radians).scale(scaleX, scaleY)</code>
    /// </summary>
    /// <param name="x"> The translation in x. </param>
    /// <param name="y"> The translation in y. </param>
    /// <param name="radians"> The angle in radians. </param>
    /// <param name="scaleX"> The scale in y. </param>
    /// <param name="scaleY"> The scale in x. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnRotRadScl( float x, float y, float radians, float scaleX, float scaleY )
    {
        m02 = x;
        m12 = y;

        if ( radians == 0 )
        {
            m00 = scaleX;
            m01 = 0;
            m10 = 0;
            m11 = scaleY;
        }
        else
        {
            var sin = MathUtils.Sin( radians );
            var cos = MathUtils.Cos( radians );

            m00 = cos * scaleX;
            m01 = -sin * scaleY;
            m10 = sin * scaleX;
            m11 = cos * scaleY;
        }

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a concatenation of translation, rotation and scale. It is a more efficient form for:
    ///     <code>idt().translate(trn).rotateRad(radians).scale(scale)</code>
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <param name="radians"> The angle in radians. </param>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnRotRadScl( Vector2 trn, float radians, Vector2 scale ) => SetToTrnRotRadScl( trn.X, trn.Y, radians, scale.X, scale.Y );

    /// <summary>
    ///     Sets this matrix to a concatenation of translation and scale. It is a more efficient form for:
    ///     <code>idt().translate(x, y).scale(scaleX, scaleY)</code>
    /// </summary>
    /// <param name="x"> The translation in x. </param>
    /// <param name="y"> The translation in y. </param>
    /// <param name="scaleX"> The scale in y. </param>
    /// <param name="scaleY"> The scale in x. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnScl( float x, float y, float scaleX, float scaleY )
    {
        m00 = scaleX;
        m01 = 0;
        m02 = x;
        m10 = 0;
        m11 = scaleY;
        m12 = y;

        return this;
    }

    /// <summary>
    ///     Sets this matrix to a concatenation of translation and scale. It is a more efficient form for:
    ///     <code>idt().translate(trn).scale(scale)</code>
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToTrnScl( Vector2 trn, Vector2 scale ) => SetToTrnScl( trn.X, trn.Y, scale.X, scale.Y );

    /// <summary>
    ///     Sets this matrix to the product of two matrices.
    /// </summary>
    /// <param name="l"> Left matrix. </param>
    /// <param name="r"> Right matrix. </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 SetToProduct( Affine2 l, Affine2 r )
    {
        m00 = ( l.m00 * r.m00 ) + ( l.m01 * r.m10 );
        m01 = ( l.m00 * r.m01 ) + ( l.m01 * r.m11 );
        m02 = ( l.m00 * r.m02 ) + ( l.m01 * r.m12 ) + l.m02;
        m10 = ( l.m10 * r.m00 ) + ( l.m11 * r.m10 );
        m11 = ( l.m10 * r.m01 ) + ( l.m11 * r.m11 );
        m12 = ( l.m10 * r.m02 ) + ( l.m11 * r.m12 ) + l.m12;

        return this;
    }

    /// <summary>
    ///     Inverts this matrix given that the determinant is != 0.
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

        var tmp00 = m11;
        var tmp01 = -m01;
        var tmp02 = ( m01 * m12 ) - ( m11 * m02 );
        var tmp10 = -m10;
        var tmp11 = m00;
        var tmp12 = ( m10 * m02 ) - ( m00 * m12 );

        m00 = invDet * tmp00;
        m01 = invDet * tmp01;
        m02 = invDet * tmp02;
        m10 = invDet * tmp10;
        m11 = invDet * tmp11;
        m12 = invDet * tmp12;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with the provided matrix and stores the result
    ///     in this matrix. For example: A.mul(B) results in A := AB
    /// </summary>
    /// <param name="other"> Matrix to multiply by. </param>
    /// <returns> This matrix for the purpose of chaining operations together.</returns>
    public Affine2 Mul( Affine2 other )
    {
        var tmp00 = ( m00 * other.m00 ) + ( m01 * other.m10 );
        var tmp01 = ( m00 * other.m01 ) + ( m01 * other.m11 );
        var tmp02 = ( m00 * other.m02 ) + ( m01 * other.m12 ) + m02;
        var tmp10 = ( m10 * other.m00 ) + ( m11 * other.m10 );
        var tmp11 = ( m10 * other.m01 ) + ( m11 * other.m11 );
        var tmp12 = ( m10 * other.m02 ) + ( m11 * other.m12 ) + m12;

        m00 = tmp00;
        m01 = tmp01;
        m02 = tmp02;
        m10 = tmp10;
        m11 = tmp11;
        m12 = tmp12;

        return this;
    }

    /// <summary>
    ///     Premultiplies this matrix with the provided matrix and stores the result in this matrix. For example:
    ///     <pre>
    ///         A.preMul(B) results in A := BA
    ///     </pre>
    /// </summary>
    /// <param name="other"> The other Matrix to multiply by </param>
    /// <returns> This matrix for the purpose of chaining operations.  </returns>
    public Affine2 PreMul( Affine2 other )
    {
        var tmp00 = ( other.m00 * m00 ) + ( other.m01 * m10 );
        var tmp01 = ( other.m00 * m01 ) + ( other.m01 * m11 );
        var tmp02 = ( other.m00 * m02 ) + ( other.m01 * m12 ) + other.m02;
        var tmp10 = ( other.m10 * m00 ) + ( other.m11 * m10 );
        var tmp11 = ( other.m10 * m01 ) + ( other.m11 * m11 );
        var tmp12 = ( other.m10 * m02 ) + ( other.m11 * m12 ) + other.m12;

        m00 = tmp00;
        m01 = tmp01;
        m02 = tmp02;
        m10 = tmp10;
        m11 = tmp11;
        m12 = tmp12;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix by a translation matrix.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Translate( float x, float y )
    {
        m02 += ( m00 * x ) + ( m01 * y );
        m12 += ( m10 * x ) + ( m11 * y );

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix by a translation matrix.
    /// </summary>
    /// <param name="trn"> The translation vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Translate( Vector2 trn ) => Translate( trn.X, trn.Y );

    /// <summary>
    ///     Premultiplies this matrix by a translation matrix.
    /// </summary>
    /// <param name="x"> The x-component of the translation vector. </param>
    /// <param name="y"> The y-component of the translation vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreTranslate( float x, float y )
    {
        m02 += x;
        m12 += y;

        return this;
    }

    /// <summary>
    ///     Premultiplies this matrix by a translation matrix.
    ///     <param name="trn">The translation vector.</param>
    ///     <returns>This matrix for the purpose of chaining.</returns>
    /// </summary>
    public Affine2 PreTranslate( Vector2 trn ) => PreTranslate( trn.X, trn.Y );

    /// <summary>
    ///     Postmultiplies this matrix with a scale matrix.
    /// </summary>
    /// <param name="scaleX"> The scale in the x-axis. </param>
    /// <param name="scaleY"> The scale in the y-axis. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Scale( float scaleX, float scaleY )
    {
        m00 *= scaleX;
        m01 *= scaleY;
        m10 *= scaleX;
        m11 *= scaleY;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with a scale matrix.
    /// </summary>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Scale( Vector2 scale ) => Scale( scale.X, scale.Y );

    /// <summary>
    ///     Premultiplies this matrix with a scale matrix.
    /// </summary>
    /// <param name="scaleX"> The scale in the x-axis. </param>
    /// <param name="scaleY"> The scale in the y-axis. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreScale( float scaleX, float scaleY )
    {
        m00 *= scaleX;
        m01 *= scaleX;
        m02 *= scaleX;
        m10 *= scaleY;
        m11 *= scaleY;
        m12 *= scaleY;

        return this;
    }

    /// <summary>
    ///     Premultiplies this matrix with a scale matrix.
    /// </summary>
    /// <param name="scale"> The scale vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreScale( Vector2 scale ) => PreScale( scale.X, scale.Y );

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
    /// </summary>
    /// <param name="degrees"> The angle in degrees </param>
    /// <returns>
    ///     This matrix for the purpose of chaining.
    /// </returns>
    public Affine2 Rotate( float degrees )
    {
        if ( degrees == 0 )
        {
            return this;
        }

        var cos = MathUtils.CosDeg( degrees );
        var sin = MathUtils.SinDeg( degrees );

        var tmp00 = ( m00 * cos ) + ( m01 * sin );
        var tmp01 = ( m00 * -sin ) + ( m01 * cos );
        var tmp10 = ( m10 * cos ) + ( m11 * sin );
        var tmp11 = ( m10 * -sin ) + ( m11 * cos );

        m00 = tmp00;
        m01 = tmp01;
        m10 = tmp10;
        m11 = tmp11;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix with a (counter-clockwise) rotation matrix.
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

        var tmp00 = ( m00 * cos ) + ( m01 * sin );
        var tmp01 = ( m00 * -sin ) + ( m01 * cos );
        var tmp10 = ( m10 * cos ) + ( m11 * sin );
        var tmp11 = ( m10 * -sin ) + ( m11 * cos );

        m00 = tmp00;
        m01 = tmp01;
        m10 = tmp10;
        m11 = tmp11;

        return this;
    }

    /// <summary>
    ///     Premultiplies this matrix with a (counter-clockwise) rotation matrix.
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

        var tmp00 = ( cos * m00 ) - ( sin * m10 );
        var tmp01 = ( cos * m01 ) - ( sin * m11 );
        var tmp02 = ( cos * m02 ) - ( sin * m12 );
        var tmp10 = ( sin * m00 ) + ( cos * m10 );
        var tmp11 = ( sin * m01 ) + ( cos * m11 );
        var tmp12 = ( sin * m02 ) + ( cos * m12 );

        m00 = tmp00;
        m01 = tmp01;
        m02 = tmp02;
        m10 = tmp10;
        m11 = tmp11;
        m12 = tmp12;

        return this;
    }

    /// <summary>
    ///     Premultiplies this matrix with a (counter-clockwise) rotation matrix.
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

        var tmp00 = ( cos * m00 ) - ( sin * m10 );
        var tmp01 = ( cos * m01 ) - ( sin * m11 );
        var tmp02 = ( cos * m02 ) - ( sin * m12 );
        var tmp10 = ( sin * m00 ) + ( cos * m10 );
        var tmp11 = ( sin * m01 ) + ( cos * m11 );
        var tmp12 = ( sin * m02 ) + ( cos * m12 );

        m00 = tmp00;
        m01 = tmp01;
        m02 = tmp02;
        m10 = tmp10;
        m11 = tmp11;
        m12 = tmp12;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix by a shear matrix.
    /// </summary>
    /// <param name="shearX"> The shear in x direction.</param>
    /// <param name="shearY"> The shear in y direction.</param>
    /// <returns> This matrix for the purpose of chaining.</returns>
    public Affine2 Shear( float shearX, float shearY )
    {
        var tmp0 = m00 + ( shearY * m01 );
        var tmp1 = m01 + ( shearX * m00 );

        m00 = tmp0;
        m01 = tmp1;

        tmp0 = m10 + ( shearY * m11 );
        tmp1 = m11 + ( shearX * m10 );

        m10 = tmp0;
        m11 = tmp1;

        return this;
    }

    /// <summary>
    ///     Postmultiplies this matrix by a shear matrix.
    /// </summary>
    /// <param name="shear"> The shear vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 Shear( Vector2 shear ) => Shear( shear.X, shear.Y );

    /// <summary>
    ///     Premultiplies this matrix by a shear matrix.
    /// </summary>
    /// <param name="shearX"> The shear in x direction. </param>
    /// <param name="shearY"> The shear in y direction. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreShear( float shearX, float shearY )
    {
        var tmp00 = m00 + ( shearX * m10 );
        var tmp01 = m01 + ( shearX * m11 );
        var tmp02 = m02 + ( shearX * m12 );
        var tmp10 = m10 + ( shearY * m00 );
        var tmp11 = m11 + ( shearY * m01 );
        var tmp12 = m12 + ( shearY * m02 );

        m00 = tmp00;
        m01 = tmp01;
        m02 = tmp02;
        m10 = tmp10;
        m11 = tmp11;
        m12 = tmp12;

        return this;
    }

    /// <summary>
    ///     Premultiplies this matrix by a shear matrix.
    /// </summary>
    /// <param name="shear"> The shear vector. </param>
    /// <returns> This matrix for the purpose of chaining.  </returns>
    public Affine2 PreShear( Vector2 shear ) => PreShear( shear.X, shear.Y );

    /// <summary>
    ///     Calculates the determinant of the matrix.
    /// </summary>
    /// <returns> The determinant of this matrix.</returns>
    public float GetDeterminant() => ( m00 * m11 ) - ( m01 * m10 );

    /// <summary>
    ///     Get the x-y translation component of the matrix.
    /// </summary>
    /// <param name="position"> Output vector. </param>
    /// <returns> Filled position.  </returns>
    public Vector2 GetTranslation( Vector2 position )
    {
        position.X = m02;
        position.Y = m12;

        return position;
    }

    /// <summary>
    ///     Check if the this is a plain translation matrix.
    /// </summary>
    /// <returns> True if scale is 1 and rotation is 0.</returns>
    public bool IsTranslation() => ( m00 == 1 ) && ( m11 == 1 ) && ( m01 == 0 ) && ( m10 == 0 );

    /// <summary>
    ///     Check if this is an indentity matrix.
    /// </summary>
    /// <returns> True if scale is 1 and rotation is 0.  </returns>
    public bool IsIdentityMatrix() => m00 is 1
                                   && m02 is 0
                                   && m12 is 0
                                   && m11 is 1
                                   && m01 is 0
                                   && m10 is 0;

    /// <summary>
    ///     Applies the affine transformation on a vector.
    /// </summary>
    public void ApplyTo( Vector2 point )
    {
        var x = point.X;
        var y = point.Y;

        point.X = ( m00 * x ) + ( m01 * y ) + m02;
        point.Y = ( m10 * x ) + ( m11 * y ) + m12;
    }

    /// <summary>
    /// </summary>
    public override string ToString() => "[" + m00 + "|" + m01 + "|" + m02 + "]\n[" + m10 + "|" + m11 + "|" + m12 + "]\n[0.0|0.0|0.1]";
}
