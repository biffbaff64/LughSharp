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

using LibGDXSharp.Scenes.Scene2D;

using Matrix4 = LibGDXSharp.Maths.Matrix4;

namespace LibGDXSharp.Graphics.G2D;

/// <summary>
/// CpuSpriteBatch behaves like SpriteBatch, except it doesn't flush automatically
/// whenever the transformation matrix changes. Instead, the vertices get adjusted
/// on subsequent draws to match the running batch. This can improve performance
/// through longer batches, for example when drawing Groups with transform enabled.
/// </summary>
/// <see cref="SpriteBatch.RenderCalls"/>
/// <see cref="Group.Transform"/>
[PublicAPI]
public class CpuSpriteBatch : SpriteBatch
{
    private readonly Matrix4 _virtualMatrix = new();
    private readonly Affine2 _adjustAffine  = new();
    private readonly Affine2 _tmpAffine     = new();

    private bool _adjustNeeded;
    private bool _haveIdentityRealMatrix = true;

    /// <summary>
    /// Constructs a CpuSpriteBatch with a size of 1000 and the default shader.
    /// </summary>
    /// <para>See also: <see cref="SpriteBatch()"/></para>
    public CpuSpriteBatch() : this( 1000 )
    {
    }

    /// <summary>
    /// Constructs a CpuSpriteBatch with a custom shader.
    /// </summary>
    /// <para>See also: <see cref="SpriteBatch()"/></para>
    public CpuSpriteBatch( int size, ShaderProgram defaultShader = null! )
        : base( size, defaultShader )
    {
    }

    /// <summary>
    /// <para>
    /// Flushes the batch and realigns the real matrix on the GPU. Subsequent
    /// draws won't need adjustment and will be slightly faster as long as the
    /// transform matrix is not changed by <see cref="SetTransformMatrix(Matrix4)"/>.
    /// </para>
    /// <para>
    /// Note: The real transform matrix <em>must</em> be invertible. If a singular
    /// matrix is detected, GdxRuntimeException will be thrown.
    /// </para>
    /// </summary>
    public virtual void FlushAndSyncTransformMatrix()
    {
        Flush();

        if ( _adjustNeeded )
        {
            // vertices flushed, safe now to replace matrix
            _haveIdentityRealMatrix = CheckIdt( _virtualMatrix );

            if ( !_haveIdentityRealMatrix && ( _virtualMatrix.Det() == 0 ) )
            {
                throw new GdxRuntimeException( "Transform matrix is singular, can't sync" );
            }

            _adjustNeeded = false;
            base.SetTransformMatrix( _virtualMatrix );
        }
    }

    public Matrix4 GetTransformMatrix()
    {
        return ( _adjustNeeded ? _virtualMatrix : base.TransformMatrix );
    }

    /// <summary>
    /// Sets the transform matrix to be used by this Batch. Even if this is called
    /// inside a <see cref="SpriteBatch.Begin"/>/<see cref="SpriteBatch.End"/> block,
    /// the current batch is <em>not</em> flushed to the GPU. Instead, for every
    /// subsequent draw() the vertices will be transformed on the CPU to match the
    /// original batch matrix. This adjustment must be performed until the matrices
    /// are realigned by restoring the original matrix, or by calling
    /// <see cref="FlushAndSyncTransformMatrix()"/>. 
    /// </summary>
    public override void SetTransformMatrix( Matrix4 transform )
    {
        Matrix4 realMatrix = base.TransformMatrix;

        if ( CheckEqual( realMatrix, transform ) )
        {
            _adjustNeeded = false;
        }
        else
        {
            if ( IsDrawing )
            {
                _virtualMatrix.SetAsAffine( transform );
                _adjustNeeded = true;

                // adjust = inverse(real) x virtual
                // real x adjust x vertex = virtual x vertex

                if ( _haveIdentityRealMatrix )
                {
                    _adjustAffine.SetFrom( transform );
                }
                else
                {
                    _tmpAffine.SetFrom( transform );
                    _adjustAffine.SetFrom( realMatrix ).Invert().Mul( _tmpAffine );
                }
            }
            else
            {
                realMatrix.SetAsAffine( transform );
                _haveIdentityRealMatrix = CheckIdt( realMatrix );
            }
        }
    }

    /// <summary>
    /// Sets the transform matrix to be used by this Batch. Even if this is calle
    /// inside a <see cref="SpriteBatch.Begin"/>/<see cref="SpriteBatch.End"/> block,
    /// the current batch is <em>not</em> flushed to the GPU. Instead, for every
    /// subsequent draw() the vertices will be transformed on the CPU to match the
    /// original batch matrix.
    /// <para>
    /// This adjustment must be performed until the matrices are realigned by restoring
    /// the original matrix, or by calling <see cref="FlushAndSyncTransformMatrix()"/>
    /// or <seealso cref="SpriteBatch.End"/>. 
    /// </para>
    /// </summary>
    public virtual void SetTransformMatrix( Affine2 transform )
    {
        Matrix4 realMatrix = base.TransformMatrix;

        if ( CheckEqual( realMatrix, transform ) )
        {
            _adjustNeeded = false;
        }
        else
        {
            _virtualMatrix.SetAsAffine( transform );

            if ( IsDrawing )
            {
                _adjustNeeded = true;

                // adjust = inverse(real) x virtual
                // real x adjust x vertex = virtual x vertex

                if ( _haveIdentityRealMatrix )
                {
                    _adjustAffine.SetFrom( transform );
                }
                else
                {
                    _adjustAffine.SetFrom( realMatrix ).Invert().Mul( transform );
                }
            }
            else
            {
                realMatrix.SetAsAffine( transform );
                _haveIdentityRealMatrix = CheckIdt( realMatrix );
            }
        }
    }

    public override void Draw( Texture texture,
                               float x,
                               float y,
                               float originX,
                               float originY,
                               float width,
                               float height,
                               float scaleX,
                               float scaleY,
                               float rotation,
                               int srcX,
                               int srcY,
                               int srcWidth,
                               int srcHeight,
                               bool flipX,
                               bool flipY )
    {
        if ( !_adjustNeeded )
        {
            base.Draw
                (
                texture, x, y, originX, originY, width, height, scaleX, scaleY,
                rotation, srcX, srcY, srcWidth, srcHeight, flipX, flipY
                );
        }
        else
        {
            DrawAdjusted
                (
                texture, x, y, originX, originY, width, height, scaleX, scaleY,
                rotation, srcX, srcY, srcWidth, srcHeight, flipX, flipY
                );
        }
    }

    public override void Draw( Texture texture,
                               float x,
                               float y,
                               float width,
                               float height,
                               int srcX,
                               int srcY,
                               int srcWidth,
                               int srcHeight,
                               bool flipX,
                               bool flipY )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( texture, x, y, width, height, srcX, srcY, srcWidth, srcHeight, flipX, flipY );
        }
        else
        {
            DrawAdjusted
                (
                texture, x, y, 0, 0, width, height, 1, 1, 0,
                srcX, srcY, srcWidth, srcHeight, flipX, flipY
                );
        }
    }

    public override void Draw( Texture texture, float x, float y, int srcX, int srcY, int srcWidth, int srcHeight )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( texture, x, y, srcX, srcY, srcWidth, srcHeight );
        }
        else
        {
            DrawAdjusted
                (
                texture, x, y, 0, 0, srcWidth, srcHeight, 1, 1, 0,
                srcX, srcY, srcWidth, srcHeight, false, false
                );
        }
    }

    public override void Draw( Texture texture,
                               float x,
                               float y,
                               float width,
                               float height,
                               float u,
                               float v,
                               float u2,
                               float v2 )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( texture, x, y, width, height, u, v, u2, v2 );
        }
        else
        {
            DrawAdjustedUV( texture, x, y, 0, 0, width, height, 1, 1, 0, u, v, u2, v2, false, false );
        }
    }

    public override void Draw( Texture texture, float x, float y )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( texture, x, y );
        }
        else
        {
            DrawAdjusted
                (
                texture, x, y, 0, 0, texture.Width, texture.Height,
                1, 1, 0, 0, 1, 1, 0, false, false
                );
        }
    }

    public override void Draw( Texture texture, float x, float y, float width, float height )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( texture, x, y, width, height );
        }
        else
        {
            DrawAdjusted( texture, x, y, 0, 0, width, height, 1, 1, 0, 0, 1, 1, 0, false, false );
        }
    }

    public override void Draw( TextureRegion region, float x, float y )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( region, x, y );
        }
        else
        {
            DrawAdjusted( region, x, y, 0, 0, region.RegionWidth, region.RegionHeight, 1, 1, 0 );
        }
    }

    public override void Draw( TextureRegion region, float x, float y, float width, float height )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( region, x, y, width, height );
        }
        else
        {
            DrawAdjusted( region, x, y, 0, 0, width, height, 1, 1, 0 );
        }
    }

    public override void Draw( TextureRegion region,
                               float x,
                               float y,
                               float originX,
                               float originY,
                               float width,
                               float height,
                               float scaleX,
                               float scaleY,
                               float rotation )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( region, x, y, originX, originY, width, height, scaleX, scaleY, rotation );
        }
        else
        {
            DrawAdjusted( region, x, y, originX, originY, width, height, scaleX, scaleY, rotation );
        }
    }

    public override void Draw( TextureRegion region,
                               float x,
                               float y,
                               float originX,
                               float originY,
                               float width,
                               float height,
                               float scaleX,
                               float scaleY,
                               float rotation,
                               bool clockwise )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( region, x, y, originX, originY, width, height, scaleX, scaleY, rotation, clockwise );
        }
        else
        {
            DrawAdjusted
                (
                region, x, y, originX, originY, width, height,
                scaleX, scaleY, rotation, clockwise
                );
        }
    }

    public override void Draw( Texture texture, float[] spriteVertices, int offset, int count )
    {
        if ( ( count % Sprite.SpriteSize ) != 0 )
        {
            throw new GdxRuntimeException( "invalid vertex count" );
        }

        if ( !_adjustNeeded )
        {
            base.Draw( texture, spriteVertices, offset, count );
        }
        else
        {
            DrawAdjusted( texture, spriteVertices, offset, count );
        }
    }

    public override void Draw( TextureRegion region, float width, float height, Affine2 transform )
    {
        if ( !_adjustNeeded )
        {
            base.Draw( region, width, height, transform );
        }
        else
        {
            DrawAdjusted( region, width, height, transform );
        }
    }

    private void DrawAdjusted( TextureRegion region,
                               float x,
                               float y,
                               float originX,
                               float originY,
                               float width,
                               float height,
                               float scaleX,
                               float scaleY,
                               float rotation )
    {
        // v must be flipped
        DrawAdjustedUV
            (
            region.Texture, x, y, originX, originY, width, height, scaleX,
            scaleY, rotation, region.U, region.V2, region.U2, region.V, false, false
            );
    }

    private void DrawAdjusted( Texture texture,
                               float x,
                               float y,
                               float originX,
                               float originY,
                               float width,
                               float height,
                               float scaleX,
                               float scaleY,
                               float rotation,
                               int srcX,
                               int srcY,
                               int srcWidth,
                               int srcHeight,
                               bool flipX,
                               bool flipY )
    {
        var invWidth  = 1.0f / texture.Width;
        var invHeight = 1.0f / texture.Height;

        var u  = srcX * invWidth;
        var v  = ( srcY + srcHeight ) * invHeight;
        var u2 = ( srcX + srcWidth ) * invWidth;
        var v2 = srcY * invHeight;

        DrawAdjustedUV
            (
            texture, x, y, originX, originY, width, height, scaleX,
            scaleY, rotation, u, v, u2, v2, flipX, flipY
            );
    }

    private void DrawAdjustedUV( Texture texture,
                                 float x,
                                 float y,
                                 float originX,
                                 float originY,
                                 float width,
                                 float height,
                                 float scaleX,
                                 float scaleY,
                                 float rotation,
                                 float u,
                                 float v,
                                 float u2,
                                 float v2,
                                 bool flipX,
                                 bool flipY )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "CpuSpriteBatch.begin must be called before draw." );
        }

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( idx == Vertices.Length )
        {
            base.Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( ( scaleX is not 1 ) || ( scaleY is not 1 ) )
        {
            fx  *= scaleX;
            fy  *= scaleY;
            fx2 *= scaleX;
            fy2 *= scaleY;
        }

        // construct corner points, start from top left and go counter clockwise
        var p1X = fx;
        var p1Y = fy;
        var p2X = fx;
        var p2Y = fy2;
        var p3X = fx2;
        var p3Y = fy2;
        var p4X = fx2;
        var p4Y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if ( rotation != 0 )
        {
            var cos = MathUtils.CosDeg( rotation );
            var sin = MathUtils.SinDeg( rotation );

            x1 = ( cos * p1X ) - ( sin * p1Y );
            y1 = ( sin * p1X ) + ( cos * p1Y );

            x2 = ( cos * p2X ) - ( sin * p2Y );
            y2 = ( sin * p2X ) + ( cos * p2Y );

            x3 = ( cos * p3X ) - ( sin * p3Y );
            y3 = ( sin * p3X ) + ( cos * p3Y );

            x4 = x1 + ( x3 - x2 );
            y4 = y3 - ( y2 - y1 );
        }
        else
        {
            x1 = p1X;
            y1 = p1Y;

            x2 = p2X;
            y2 = p2Y;

            x3 = p3X;
            y3 = p3Y;

            x4 = p4X;
            y4 = p4Y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        if ( flipX )
        {
            ( u, u2 ) = ( u2, u );
        }

        if ( flipY )
        {
            ( v, v2 ) = ( v2, v );
        }

        Affine2 t = _adjustAffine;

        Vertices[ idx + 0 ] = ( t.m00 * x1 ) + ( t.m01 * y1 ) + t.m02;
        Vertices[ idx + 1 ] = ( t.m10 * x1 ) + ( t.m11 * y1 ) + t.m12;
        Vertices[ idx + 2 ] = colorPacked;
        Vertices[ idx + 3 ] = u;
        Vertices[ idx + 4 ] = v;

        Vertices[ idx + 5 ] = ( t.m00 * x2 ) + ( t.m01 * y2 ) + t.m02;
        Vertices[ idx + 6 ] = ( t.m10 * x2 ) + ( t.m11 * y2 ) + t.m12;
        Vertices[ idx + 7 ] = colorPacked;
        Vertices[ idx + 8 ] = u;
        Vertices[ idx + 9 ] = v2;

        Vertices[ idx + 10 ] = ( t.m00 * x3 ) + ( t.m01 * y3 ) + t.m02;
        Vertices[ idx + 11 ] = ( t.m10 * x3 ) + ( t.m11 * y3 ) + t.m12;
        Vertices[ idx + 12 ] = colorPacked;
        Vertices[ idx + 13 ] = u2;
        Vertices[ idx + 14 ] = v2;

        Vertices[ idx + 15 ] = ( t.m00 * x4 ) + ( t.m01 * y4 ) + t.m02;
        Vertices[ idx + 16 ] = ( t.m10 * x4 ) + ( t.m11 * y4 ) + t.m12;
        Vertices[ idx + 17 ] = colorPacked;
        Vertices[ idx + 18 ] = u2;
        Vertices[ idx + 19 ] = v;

        idx += Sprite.SpriteSize;
    }

    private void DrawAdjusted( TextureRegion region,
                               float x,
                               float y,
                               float originX,
                               float originY,
                               float width,
                               float height,
                               float scaleX,
                               float scaleY,
                               float rotation,
                               bool clockwise )
    {
        if ( !IsDrawing )
        {
            throw new IllegalStateException( "CpuSpriteBatch.begin must be called before draw." );
        }

        if ( region.Texture != LastTexture )
        {
            SwitchTexture( region.Texture );
        }
        else if ( idx == Vertices.Length )
        {
            base.Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( ( scaleX is not 1 ) || ( scaleY is not 1 ) )
        {
            fx  *= scaleX;
            fy  *= scaleY;
            fx2 *= scaleX;
            fy2 *= scaleY;
        }

        // construct corner points, start from top left and go counter clockwise
        var p1X = fx;
        var p1Y = fy;
        var p2X = fx;
        var p2Y = fy2;
        var p3X = fx2;
        var p3Y = fy2;
        var p4X = fx2;
        var p4Y = fy;

        float x1;
        float y1;
        float x2;
        float y2;
        float x3;
        float y3;
        float x4;
        float y4;

        // rotate
        if ( rotation != 0 )
        {
            var cos = MathUtils.CosDeg( rotation );
            var sin = MathUtils.SinDeg( rotation );

            x1 = ( cos * p1X ) - ( sin * p1Y );
            y1 = ( sin * p1X ) + ( cos * p1Y );

            x2 = ( cos * p2X ) - ( sin * p2Y );
            y2 = ( sin * p2X ) + ( cos * p2Y );

            x3 = ( cos * p3X ) - ( sin * p3Y );
            y3 = ( sin * p3X ) + ( cos * p3Y );

            x4 = x1 + ( x3 - x2 );
            y4 = y3 - ( y2 - y1 );
        }
        else
        {
            x1 = p1X;
            y1 = p1Y;

            x2 = p2X;
            y2 = p2Y;

            x3 = p3X;
            y3 = p3Y;

            x4 = p4X;
            y4 = p4Y;
        }

        x1 += worldOriginX;
        y1 += worldOriginY;
        x2 += worldOriginX;
        y2 += worldOriginY;
        x3 += worldOriginX;
        y3 += worldOriginY;
        x4 += worldOriginX;
        y4 += worldOriginY;

        float u1, v1, u2, v2, u3, v3, u4, v4;

        if ( clockwise )
        {
            u1 = region.U2;
            v1 = region.V2;
            u2 = region.U;
            v2 = region.V2;
            u3 = region.U;
            v3 = region.V;
            u4 = region.U2;
            v4 = region.V;
        }
        else
        {
            u1 = region.U;
            v1 = region.V;
            u2 = region.U2;
            v2 = region.V;
            u3 = region.U2;
            v3 = region.V2;
            u4 = region.U;
            v4 = region.V2;
        }

        Vertices[ idx + 0 ] = ( _adjustAffine.m00 * x1 ) + ( _adjustAffine.m01 * y1 ) + _adjustAffine.m02;
        Vertices[ idx + 1 ] = ( _adjustAffine.m10 * x1 ) + ( _adjustAffine.m11 * y1 ) + _adjustAffine.m12;
        Vertices[ idx + 2 ] = colorPacked;
        Vertices[ idx + 3 ] = u1;
        Vertices[ idx + 4 ] = v1;

        Vertices[ idx + 5 ] = ( _adjustAffine.m00 * x2 ) + ( _adjustAffine.m01 * y2 ) + _adjustAffine.m02;
        Vertices[ idx + 6 ] = ( _adjustAffine.m10 * x2 ) + ( _adjustAffine.m11 * y2 ) + _adjustAffine.m12;
        Vertices[ idx + 7 ] = colorPacked;
        Vertices[ idx + 8 ] = u2;
        Vertices[ idx + 9 ] = v2;

        Vertices[ idx + 10 ] = ( _adjustAffine.m00 * x3 ) + ( _adjustAffine.m01 * y3 ) + _adjustAffine.m02;
        Vertices[ idx + 11 ] = ( _adjustAffine.m10 * x3 ) + ( _adjustAffine.m11 * y3 ) + _adjustAffine.m12;
        Vertices[ idx + 12 ] = colorPacked;
        Vertices[ idx + 13 ] = u3;
        Vertices[ idx + 14 ] = v3;

        Vertices[ idx + 15 ] = ( _adjustAffine.m00 * x4 ) + ( _adjustAffine.m01 * y4 ) + _adjustAffine.m02;
        Vertices[ idx + 16 ] = ( _adjustAffine.m10 * x4 ) + ( _adjustAffine.m11 * y4 ) + _adjustAffine.m12;
        Vertices[ idx + 17 ] = colorPacked;
        Vertices[ idx + 18 ] = u4;
        Vertices[ idx + 19 ] = v4;

        idx += Sprite.SpriteSize;
    }

    private void DrawAdjusted( TextureRegion region, float width, float height, Affine2 transform )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "CpuSpriteBatch.begin must be called before draw." );
        }

        if ( region.Texture != LastTexture )
        {
            SwitchTexture( region.Texture );
        }
        else if ( idx == Vertices.Length )
        {
            base.Flush();
        }

        // construct corner points
        var x1 = transform.m02;
        var y1 = transform.m12;
        var x2 = ( transform.m01 * height ) + transform.m02;
        var y2 = ( transform.m11 * height ) + transform.m12;
        var x3 = ( transform.m00 * width ) + ( transform.m01 * height ) + transform.m02;
        var y3 = ( transform.m10 * width ) + ( transform.m11 * height ) + transform.m12;
        var x4 = ( transform.m00 * width ) + transform.m02;
        var y4 = ( transform.m10 * width ) + transform.m12;

        // v must be flipped
        var u  = region.U;
        var v  = region.V2;
        var u2 = region.U2;
        var v2 = region.V;

        Vertices[ idx + 0 ] = ( _adjustAffine.m00 * x1 ) + ( _adjustAffine.m01 * y1 ) + _adjustAffine.m02;
        Vertices[ idx + 1 ] = ( _adjustAffine.m10 * x1 ) + ( _adjustAffine.m11 * y1 ) + _adjustAffine.m12;
        Vertices[ idx + 2 ] = colorPacked;
        Vertices[ idx + 3 ] = u;
        Vertices[ idx + 4 ] = v;

        Vertices[ idx + 5 ] = ( _adjustAffine.m00 * x2 ) + ( _adjustAffine.m01 * y2 ) + _adjustAffine.m02;
        Vertices[ idx + 6 ] = ( _adjustAffine.m10 * x2 ) + ( _adjustAffine.m11 * y2 ) + _adjustAffine.m12;
        Vertices[ idx + 7 ] = colorPacked;
        Vertices[ idx + 8 ] = u;
        Vertices[ idx + 9 ] = v2;

        Vertices[ idx + 10 ] = ( _adjustAffine.m00 * x3 ) + ( _adjustAffine.m01 * y3 ) + _adjustAffine.m02;
        Vertices[ idx + 11 ] = ( _adjustAffine.m10 * x3 ) + ( _adjustAffine.m11 * y3 ) + _adjustAffine.m12;
        Vertices[ idx + 12 ] = colorPacked;
        Vertices[ idx + 13 ] = u2;
        Vertices[ idx + 14 ] = v2;

        Vertices[ idx + 15 ] = ( _adjustAffine.m00 * x4 ) + ( _adjustAffine.m01 * y4 ) + _adjustAffine.m02;
        Vertices[ idx + 16 ] = ( _adjustAffine.m10 * x4 ) + ( _adjustAffine.m11 * y4 ) + _adjustAffine.m12;
        Vertices[ idx + 17 ] = colorPacked;
        Vertices[ idx + 18 ] = u2;
        Vertices[ idx + 19 ] = v;

        idx += Sprite.SpriteSize;
    }

    private void DrawAdjusted( Texture texture, float[] spriteVertices, int offset, int count )
    {
        if ( !IsDrawing )
        {
            throw new System.InvalidOperationException( "CpuSpriteBatch.begin must be called before draw." );
        }

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }

        var copyCount = Math.Min( Vertices.Length - idx, count );

        do
        {
            count -= copyCount;

            while ( copyCount > 0 )
            {
                var x = spriteVertices[ offset ];
                var y = spriteVertices[ offset + 1 ];

                Vertices[ idx ]     = ( _adjustAffine.m00 * x ) + ( _adjustAffine.m01 * y ) + _adjustAffine.m02; // x
                Vertices[ idx + 1 ] = ( _adjustAffine.m10 * x ) + ( _adjustAffine.m11 * y ) + _adjustAffine.m12; // y
                Vertices[ idx + 2 ] = spriteVertices[ offset + 2 ];                                              // color
                Vertices[ idx + 3 ] = spriteVertices[ offset + 3 ];                                              // u
                Vertices[ idx + 4 ] = spriteVertices[ offset + 4 ];                                              // v

                idx       += Sprite.VertexSize;
                offset    += Sprite.VertexSize;
                copyCount -= Sprite.VertexSize;
            }

            if ( count > 0 )
            {
                base.Flush();
                copyCount = Math.Min( Vertices.Length, count );
            }
        }
        while ( count > 0 );
    }

    private static bool CheckEqual( Matrix4 a, Matrix4 b )
    {
        if ( a == b )
        {
            return true;
        }

        // matrices are assumed to be 2D transformations
        return ( ( a.val[ Matrix4.M00 ].Equals( b.val[ Matrix4.M00 ] ) )
              && ( a.val[ Matrix4.M10 ].Equals( b.val[ Matrix4.M10 ] ) )
              && ( a.val[ Matrix4.M01 ].Equals( b.val[ Matrix4.M01 ] ) )
              && ( a.val[ Matrix4.M11 ].Equals( b.val[ Matrix4.M11 ] ) )
              && ( a.val[ Matrix4.M03 ].Equals( b.val[ Matrix4.M03 ] ) )
              && ( a.val[ Matrix4.M13 ].Equals( b.val[ Matrix4.M13 ] ) ) );
    }

    private static bool CheckEqual( Matrix4 matrix, Affine2 affine )
    {
        var val = matrix.Values;

        // matrix is assumed to be 2D transformation
        return ( ( val[ Matrix4.M00 ].Equals( affine.m00 ) )
              && ( val[ Matrix4.M10 ].Equals( affine.m10 ) )
              && ( val[ Matrix4.M01 ].Equals( affine.m01 ) )
              && ( val[ Matrix4.M11 ].Equals( affine.m11 ) )
              && ( val[ Matrix4.M03 ].Equals( affine.m02 ) )
              && ( val[ Matrix4.M13 ].Equals( affine.m12 ) ) );
    }

    private static bool CheckIdt( Matrix4 matrix )
    {
        var val = matrix.Values;

        // matrix is assumed to be 2D transformation
        return ( ( val[ Matrix4.M00 ] is 1 )
              && ( val[ Matrix4.M10 ] is 0 )
              && ( val[ Matrix4.M01 ] is 0 )
              && ( val[ Matrix4.M11 ] is 1 )
              && ( val[ Matrix4.M03 ] is 0 )
              && ( val[ Matrix4.M13 ] is 0 ) );
    }
}
