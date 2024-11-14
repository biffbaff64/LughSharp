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

using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Exceptions;
using Matrix4 = Corelib.LibCore.Maths.Matrix4;

namespace Corelib.LibCore.Graphics.G2D;

/// <summary>
/// CpuSpriteBatch behaves like SpriteBatch, except it doesn't flush automatically
/// whenever the transformation matrix changes. Instead, the vertices get adjusted
/// on subsequent draws to match the running batch. This can improve performance
/// through longer batches, for example when drawing Groups with transform enabled.
/// </summary>
/// <see cref="SpriteBatch.RenderCalls"/>
/// <see cref="Scenes.Scene2D.Group.Transform"/>
[PublicAPI]
public class CpuSpriteBatch : SpriteBatch
{
    private readonly Affine2 _adjustAffine  = new();
    private readonly Affine2 _tmpAffine     = new();
    private readonly Matrix4 _virtualMatrix = new();

    private bool _adjustNeeded;
    private bool _haveIdentityRealMatrix = true;

    // ========================================================================

    /// <summary>
    /// Constructs a CpuSpriteBatch with a size of 1000 and the default shader.
    /// </summary>
    /// <para>See also: <see cref="SpriteBatch"/></para>
    public CpuSpriteBatch() : this( 1000 )
    {
    }

    /// <summary>
    /// Constructs a CpuSpriteBatch with a custom shader.
    /// </summary>
    /// <para>See also: <see cref="SpriteBatch"/></para>
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

    /// <summary>
    /// Returns a <see cref="Matrix4"/> holding the transform matrix.
    /// </summary>
    public virtual Matrix4 GetTransformMatrix()
    {
        return _adjustNeeded ? _virtualMatrix : TransformMatrix;
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
        if ( CheckEqual( TransformMatrix, transform ) )
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
                    _adjustAffine.SetFrom( TransformMatrix ).Invert().Mul( _tmpAffine );
                }
            }
            else
            {
                TransformMatrix.SetAsAffine( transform );
                _haveIdentityRealMatrix = CheckIdt( TransformMatrix );
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
    /// or <see cref="SpriteBatch.End"/>.
    /// </para>
    /// </summary>
    public virtual void SetTransformMatrix( Affine2 transform )
    {
        if ( CheckEqual( TransformMatrix, transform ) )
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
                    _adjustAffine.SetFrom( TransformMatrix ).Invert().Mul( transform );
                }
            }
            else
            {
                TransformMatrix.SetAsAffine( transform );
                _haveIdentityRealMatrix = CheckIdt( TransformMatrix );
            }
        }
    }

    /// <inheritdoc />
    public override void Draw( Texture? texture,
                               GRect region,
                               Point2D origin,
                               Point2D scale,
                               float rotation,
                               GRect src,
                               bool flipX,
                               bool flipY )
    {
        ArgumentNullException.ThrowIfNull( texture );

        if ( !_adjustNeeded )
        {
            base.Draw( texture, region, origin, scale, rotation, src, flipX, flipY );
        }
        else
        {
            DrawAdjusted( texture,
                          region.X,
                          region.Y,
                          region.Width,
                          region.Height,
                          origin.X,
                          origin.Y,
                          scale.X,
                          scale.Y,
                          rotation,
                          src.X,
                          src.Y,
                          src.Width,
                          src.Height,
                          flipX,
                          flipY );
        }
    }

    /// <inheritdoc />
    public override void Draw( Texture? texture,
                               GRect region,
                               GRect src,
                               bool flipX,
                               bool flipY )
    {
        ArgumentNullException.ThrowIfNull( texture );

        if ( !_adjustNeeded )
        {
            base.Draw( texture, region, src, flipX, flipY );
        }
        else
        {
            DrawAdjusted( texture,
                          region.X,
                          region.Y,
                          region.Width,
                          region.Height,
                          0,
                          0,
                          1,
                          1,
                          0,
                          src.X,
                          src.Y,
                          src.Width,
                          src.Height,
                          flipX,
                          flipY );
        }
    }

    /// <inheritdoc />
    public override void Draw( Texture? texture, float x, float y, GRect src )
    {
        ArgumentNullException.ThrowIfNull( texture );

        if ( !_adjustNeeded )
        {
            base.Draw( texture, x, y, src );
        }
        else
        {
            DrawAdjusted( texture,
                          x,
                          y,
                          src.Width,
                          src.Height,
                          0,
                          0,
                          1,
                          1,
                          0,
                          src.X,
                          src.Y,
                          src.Width,
                          src.Height,
                          false,
                          false );
        }
    }

    /// <inheritdoc />
    public override void Draw( Texture? texture,
                               GRect region,
                               float u,
                               float v,
                               float u2,
                               float v2 )
    {
        ArgumentNullException.ThrowIfNull( texture );

        if ( !_adjustNeeded )
        {
            base.Draw( texture, region, u, v, u2, v2 );
        }
        else
        {
            DrawAdjustedUV( texture,
                            region.X,
                            region.Y,
                            region.Width,
                            region.Height,
                            0,
                            0,
                            1,
                            1,
                            0,
                            u,
                            v,
                            u2,
                            v2,
                            false,
                            false );
        }
    }

    /// <inheritdoc />
    public override void Draw( Texture? texture, float x, float y )
    {
        ArgumentNullException.ThrowIfNull( texture );

        if ( !_adjustNeeded )
        {
            base.Draw( texture, x, y );
        }
        else
        {
            DrawAdjusted( texture, x, y, 0, 0, texture.Width, texture.Height, 1, 1, 0, 0, 1, 1, 0, false, false );
        }
    }

    /// <inheritdoc />
    public override void Draw( Texture? texture, float posX, float posY, int width, int height )
    {
        ArgumentNullException.ThrowIfNull( texture );

        if ( !_adjustNeeded )
        {
            base.Draw( texture, posX, posY, width, height );
        }
        else
        {
            DrawAdjusted( texture, posX, posY, 0, 0, width, height, 1, 1, 0, 0, 1, 1, 0, false, false );
        }
    }

    /// <inheritdoc />
    public override void Draw( TextureRegion? region, float x, float y )
    {
        ArgumentNullException.ThrowIfNull( region );

        if ( !_adjustNeeded )
        {
            base.Draw( region, x, y );
        }
        else
        {
            DrawAdjusted( region, x, y, 0, 0, region.RegionWidth, region.RegionHeight, 1, 1, 0 );
        }
    }

    /// <inheritdoc />
    public override void Draw( TextureRegion? region, float x, float y, float width, float height )
    {
        ArgumentNullException.ThrowIfNull( region );

        if ( !_adjustNeeded )
        {
            base.Draw( region, x, y, width, height );
        }
        else
        {
            DrawAdjusted( region, x, y, 0, 0, width, height, 1, 1, 0 );
        }
    }

    /// <inheritdoc />
    public override void Draw( TextureRegion? textureRegion,
                               GRect region,
                               Point2D origin,
                               Point2D scale,
                               float rotation )
    {
        ArgumentNullException.ThrowIfNull( textureRegion );

        if ( !_adjustNeeded )
        {
            base.Draw( textureRegion, region, origin, scale, rotation );
        }
        else
        {
            DrawAdjusted( textureRegion,
                          region.X,
                          region.Y,
                          origin.X,
                          origin.Y,
                          region.Width,
                          region.Height,
                          scale.X,
                          scale.Y,
                          rotation );
        }
    }

    /// <inheritdoc />
    public override void Draw( TextureRegion? textureRegion,
                               GRect region,
                               Point2D origin,
                               Point2D scale,
                               float rotation,
                               bool clockwise )
    {
        ArgumentNullException.ThrowIfNull( textureRegion );

        if ( !_adjustNeeded )
        {
            base.Draw( textureRegion, region, origin, scale, rotation, clockwise );
        }
        else
        {
            DrawAdjusted( textureRegion,
                          region.X,
                          region.Y,
                          origin.X,
                          origin.Y,
                          region.Width,
                          region.Height,
                          scale.X,
                          scale.Y,
                          rotation,
                          clockwise );
        }
    }

    /// <inheritdoc />
    public override void Draw( Texture? texture, float[] spriteVertices, int offset, int count )
    {
        ArgumentNullException.ThrowIfNull( texture );

        if ( ( count % Sprite.SPRITE_SIZE ) != 0 )
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

    /// <inheritdoc />
    public override void Draw( TextureRegion? region, float width, float height, Affine2 transform )
    {
        ArgumentNullException.ThrowIfNull( region );

        if ( !_adjustNeeded )
        {
            base.Draw( region, width, height, transform );
        }
        else
        {
            DrawAdjusted( region, width, height, transform );
        }
    }

    private void DrawAdjusted( TextureRegion? region,
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
        ArgumentNullException.ThrowIfNull( region );

        // v must be flipped
        DrawAdjustedUV( region.Texture,
                        x,
                        y,
                        originX,
                        originY,
                        width,
                        height,
                        scaleX,
                        scaleY,
                        rotation,
                        region.U,
                        region.V2,
                        region.U2,
                        region.V,
                        false,
                        false );
    }

    private void DrawAdjusted( Texture? texture,
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
        ArgumentNullException.ThrowIfNull( texture );

        var invWidth  = 1.0f / texture.Width;
        var invHeight = 1.0f / texture.Height;

        var u  = srcX * invWidth;
        var v  = ( srcY + srcHeight ) * invHeight;
        var u2 = ( srcX + srcWidth ) * invWidth;
        var v2 = srcY * invHeight;

        DrawAdjustedUV( texture,
                        x,
                        y,
                        originX,
                        originY,
                        width,
                        height,
                        scaleX,
                        scaleY,
                        rotation,
                        u,
                        v,
                        u2,
                        v2,
                        flipX,
                        flipY );
    }

    private void DrawAdjustedUV( Texture? texture,
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

        ArgumentNullException.ThrowIfNull( texture );
        GdxRuntimeException.ThrowIfNull( LastTexture );
        
        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( scaleX is not 1 || scaleY is not 1 )
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

        Vertices[ Idx + 0 ] = ( _adjustAffine.M00 * x1 ) + ( _adjustAffine.M01 * y1 ) + _adjustAffine.M02;
        Vertices[ Idx + 1 ] = ( _adjustAffine.M10 * x1 ) + ( _adjustAffine.M11 * y1 ) + _adjustAffine.M12;
        Vertices[ Idx + 2 ] = ColorPackedABGR;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = ( _adjustAffine.M00 * x2 ) + ( _adjustAffine.M01 * y2 ) + _adjustAffine.M02;
        Vertices[ Idx + 6 ] = ( _adjustAffine.M10 * x2 ) + ( _adjustAffine.M11 * y2 ) + _adjustAffine.M12;
        Vertices[ Idx + 7 ] = ColorPackedABGR;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = ( _adjustAffine.M00 * x3 ) + ( _adjustAffine.M01 * y3 ) + _adjustAffine.M02;
        Vertices[ Idx + 11 ] = ( _adjustAffine.M10 * x3 ) + ( _adjustAffine.M11 * y3 ) + _adjustAffine.M12;
        Vertices[ Idx + 12 ] = ColorPackedABGR;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = ( _adjustAffine.M00 * x4 ) + ( _adjustAffine.M01 * y4 ) + _adjustAffine.M02;
        Vertices[ Idx + 16 ] = ( _adjustAffine.M10 * x4 ) + ( _adjustAffine.M11 * y4 ) + _adjustAffine.M12;
        Vertices[ Idx + 17 ] = ColorPackedABGR;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx += Sprite.SPRITE_SIZE;
    }

    private void DrawAdjusted( TextureRegion? region,
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
            throw new GdxRuntimeException( "CpuSpriteBatch.begin must be called before draw." );
        }

        ArgumentNullException.ThrowIfNull( region );

        if ( region.Texture != LastTexture )
        {
            SwitchTexture( region.Texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        // bottom left and top right corner points relative to origin
        var worldOriginX = x + originX;
        var worldOriginY = y + originY;
        var fx           = -originX;
        var fy           = -originY;
        var fx2          = width - originX;
        var fy2          = height - originY;

        // scale
        if ( scaleX is not 1 || scaleY is not 1 )
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

        Vertices[ Idx + 0 ] = ( _adjustAffine.M00 * x1 ) + ( _adjustAffine.M01 * y1 ) + _adjustAffine.M02;
        Vertices[ Idx + 1 ] = ( _adjustAffine.M10 * x1 ) + ( _adjustAffine.M11 * y1 ) + _adjustAffine.M12;
        Vertices[ Idx + 2 ] = ColorPackedABGR;
        Vertices[ Idx + 3 ] = u1;
        Vertices[ Idx + 4 ] = v1;

        Vertices[ Idx + 5 ] = ( _adjustAffine.M00 * x2 ) + ( _adjustAffine.M01 * y2 ) + _adjustAffine.M02;
        Vertices[ Idx + 6 ] = ( _adjustAffine.M10 * x2 ) + ( _adjustAffine.M11 * y2 ) + _adjustAffine.M12;
        Vertices[ Idx + 7 ] = ColorPackedABGR;
        Vertices[ Idx + 8 ] = u2;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = ( _adjustAffine.M00 * x3 ) + ( _adjustAffine.M01 * y3 ) + _adjustAffine.M02;
        Vertices[ Idx + 11 ] = ( _adjustAffine.M10 * x3 ) + ( _adjustAffine.M11 * y3 ) + _adjustAffine.M12;
        Vertices[ Idx + 12 ] = ColorPackedABGR;
        Vertices[ Idx + 13 ] = u3;
        Vertices[ Idx + 14 ] = v3;

        Vertices[ Idx + 15 ] = ( _adjustAffine.M00 * x4 ) + ( _adjustAffine.M01 * y4 ) + _adjustAffine.M02;
        Vertices[ Idx + 16 ] = ( _adjustAffine.M10 * x4 ) + ( _adjustAffine.M11 * y4 ) + _adjustAffine.M12;
        Vertices[ Idx + 17 ] = ColorPackedABGR;
        Vertices[ Idx + 18 ] = u4;
        Vertices[ Idx + 19 ] = v4;

        Idx += Sprite.SPRITE_SIZE;
    }

    private void DrawAdjusted( TextureRegion? region, float width, float height, Affine2 transform )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "CpuSpriteBatch.begin must be called before draw." );
        }

        ArgumentNullException.ThrowIfNull( region );

        if ( region.Texture != LastTexture )
        {
            SwitchTexture( region.Texture );
        }
        else if ( Idx == Vertices.Length )
        {
            Flush();
        }

        // construct corner points
        var x1 = transform.M02;
        var y1 = transform.M12;
        var x2 = ( transform.M01 * height ) + transform.M02;
        var y2 = ( transform.M11 * height ) + transform.M12;
        var x3 = ( transform.M00 * width ) + ( transform.M01 * height ) + transform.M02;
        var y3 = ( transform.M10 * width ) + ( transform.M11 * height ) + transform.M12;
        var x4 = ( transform.M00 * width ) + transform.M02;
        var y4 = ( transform.M10 * width ) + transform.M12;

        // v must be flipped
        var u  = region.U;
        var v  = region.V2;
        var u2 = region.U2;
        var v2 = region.V;

        Vertices[ Idx + 0 ] = ( _adjustAffine.M00 * x1 ) + ( _adjustAffine.M01 * y1 ) + _adjustAffine.M02;
        Vertices[ Idx + 1 ] = ( _adjustAffine.M10 * x1 ) + ( _adjustAffine.M11 * y1 ) + _adjustAffine.M12;
        Vertices[ Idx + 2 ] = ColorPackedABGR;
        Vertices[ Idx + 3 ] = u;
        Vertices[ Idx + 4 ] = v;

        Vertices[ Idx + 5 ] = ( _adjustAffine.M00 * x2 ) + ( _adjustAffine.M01 * y2 ) + _adjustAffine.M02;
        Vertices[ Idx + 6 ] = ( _adjustAffine.M10 * x2 ) + ( _adjustAffine.M11 * y2 ) + _adjustAffine.M12;
        Vertices[ Idx + 7 ] = ColorPackedABGR;
        Vertices[ Idx + 8 ] = u;
        Vertices[ Idx + 9 ] = v2;

        Vertices[ Idx + 10 ] = ( _adjustAffine.M00 * x3 ) + ( _adjustAffine.M01 * y3 ) + _adjustAffine.M02;
        Vertices[ Idx + 11 ] = ( _adjustAffine.M10 * x3 ) + ( _adjustAffine.M11 * y3 ) + _adjustAffine.M12;
        Vertices[ Idx + 12 ] = ColorPackedABGR;
        Vertices[ Idx + 13 ] = u2;
        Vertices[ Idx + 14 ] = v2;

        Vertices[ Idx + 15 ] = ( _adjustAffine.M00 * x4 ) + ( _adjustAffine.M01 * y4 ) + _adjustAffine.M02;
        Vertices[ Idx + 16 ] = ( _adjustAffine.M10 * x4 ) + ( _adjustAffine.M11 * y4 ) + _adjustAffine.M12;
        Vertices[ Idx + 17 ] = ColorPackedABGR;
        Vertices[ Idx + 18 ] = u2;
        Vertices[ Idx + 19 ] = v;

        Idx += Sprite.SPRITE_SIZE;
    }

    private void DrawAdjusted( Texture? texture, float[] spriteVertices, int offset, int count )
    {
        if ( !IsDrawing )
        {
            throw new InvalidOperationException( "CpuSpriteBatch.begin must be called before draw." );
        }

        ArgumentNullException.ThrowIfNull( texture );

        if ( texture != LastTexture )
        {
            SwitchTexture( texture );
        }

        var copyCount = Math.Min( Vertices.Length - Idx, count );

        do
        {
            count -= copyCount;

            while ( copyCount > 0 )
            {
                var x = spriteVertices[ offset ];
                var y = spriteVertices[ offset + 1 ];

                Vertices[ Idx ]     = ( _adjustAffine.M00 * x ) + ( _adjustAffine.M01 * y ) + _adjustAffine.M02; // x
                Vertices[ Idx + 1 ] = ( _adjustAffine.M10 * x ) + ( _adjustAffine.M11 * y ) + _adjustAffine.M12; // y
                Vertices[ Idx + 2 ] = spriteVertices[ offset + 2 ];                                              // color
                Vertices[ Idx + 3 ] = spriteVertices[ offset + 3 ];                                              // u
                Vertices[ Idx + 4 ] = spriteVertices[ offset + 4 ];                                              // v

                Idx       += Sprite.VERTEX_SIZE;
                offset    += Sprite.VERTEX_SIZE;
                copyCount -= Sprite.VERTEX_SIZE;
            }

            if ( count > 0 )
            {
                Flush();
                copyCount = Math.Min( Vertices.Length, count );
            }
        } while ( count > 0 );
    }

    private static bool CheckEqual( Matrix4 a, Matrix4 b )
    {
        if ( a == b )
        {
            return true;
        }

        // matrices are assumed to be 2D transformations
        return a.Val[ Matrix4.M00 ].Equals( b.Val[ Matrix4.M00 ] )
            && a.Val[ Matrix4.M10 ].Equals( b.Val[ Matrix4.M10 ] )
            && a.Val[ Matrix4.M01 ].Equals( b.Val[ Matrix4.M01 ] )
            && a.Val[ Matrix4.M11 ].Equals( b.Val[ Matrix4.M11 ] )
            && a.Val[ Matrix4.M03 ].Equals( b.Val[ Matrix4.M03 ] )
            && a.Val[ Matrix4.M13 ].Equals( b.Val[ Matrix4.M13 ] );
    }

    private static bool CheckEqual( Matrix4 matrix, Affine2 affine )
    {
        var val = matrix.Values;

        // matrix is assumed to be 2D transformation
        return val[ Matrix4.M00 ].Equals( affine.M00 )
            && val[ Matrix4.M10 ].Equals( affine.M10 )
            && val[ Matrix4.M01 ].Equals( affine.M01 )
            && val[ Matrix4.M11 ].Equals( affine.M11 )
            && val[ Matrix4.M03 ].Equals( affine.M02 )
            && val[ Matrix4.M13 ].Equals( affine.M12 );
    }

    private static bool CheckIdt( Matrix4 matrix )
    {
        var val = matrix.Values;

        // matrix is assumed to be 2D transformation
        return val[ Matrix4.M00 ] is 1
            && val[ Matrix4.M10 ] is 0
            && val[ Matrix4.M01 ] is 0
            && val[ Matrix4.M11 ] is 1
            && val[ Matrix4.M03 ] is 0
            && val[ Matrix4.M13 ] is 0;
    }
}