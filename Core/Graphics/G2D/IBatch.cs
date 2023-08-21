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

using LibGDXSharp.Maths;

namespace LibGDXSharp.G2D;

/// <summary>
/// A Batch is used to draw 2D rectangles that reference a texture (region). The class will
/// batch the drawing commands and optimize them for processing by the GPU.
/// <para>
/// To draw something with a Batch one has to first call the <see cref="IBatch.Begin()"/>
/// method which will setup appropriate render states. When you are done with drawing you
/// have to call <see cref="IBatch.End()"/> which will actually draw the things you specified.
/// </para>
/// <para>
/// All drawing commands of the Batch operate in screen coordinates. The screen coordinate
/// system has an x-axis pointing to the right, an y-axis pointing upwards and the origin
/// is in the lower left corner of the screen. You can also provide your own transformation
/// and projection matrices if you so wish.
/// </para>
/// <para>
/// A Batch is managed. In case the OpenGL context is lost all OpenGL resources a Batch uses
/// internally get invalidated. A context is lost when a user switches to another application
/// or receives an incoming call on Android. A Batch will be automatically reloaded after the
/// OpenGL context is restored.
/// </para>
/// <para>
/// A Batch is a pretty heavy object so you should only ever have one in your program.
/// </para>
/// <para>
/// A Batch works with OpenGL ES 2.0. It will use its own custom shader to draw all provided
/// sprites. You can set your own custom shader via the <see cref="Shader"/> property.
/// </para>
/// <para>
/// A Batch has to be disposed if it is no longer used.
/// </para>
/// </summary>
public interface IBatch : IDisposable
{
    public const int X1 = 0;
    public const int Y1 = 1;
    public const int C1 = 2;
    public const int U1 = 3;
    public const int V1 = 4;
    public const int X2 = 5;
    public const int Y2 = 6;
    public const int C2 = 7;
    public const int U2 = 8;
    public const int V2 = 9;
    public const int X3 = 10;
    public const int Y3 = 11;
    public const int C3 = 12;
    public const int U3 = 13;
    public const int V3 = 14;
    public const int X4 = 15;
    public const int Y4 = 16;
    public const int C4 = 17;
    public const int U4 = 18;
    public const int V4 = 19;

    /// <summary>
    /// Sets up the Batch for drawing. This will disable depth buffer writing. It enables
    /// blending and texturing. If you have more texture units enabled than the first one
    /// you have to disable them before calling this. Uses a screen coordinate system by
    /// default where everything is given in pixels. You can specify your own projection
    /// and modelview matrices via <see cref="SetProjectionMatrix(Matrix4)"/> and
    /// <see cref="SetTransformMatrix(Matrix4)"/>. 
    /// </summary>
    public void Begin();

    /// <summary>
    /// Finishes off rendering. Enables depth writes, disables blending and texturing.
    /// Must always be called after a call to <see cref="Begin"/> 
    /// </summary>
    public void End();

    /// <returns>
    /// The rendering color of this Batch.
    /// </returns>
    public Color Color { get; set; }

    public void SetColor( float r, float g, float b, float a );

    /// <returns>
    /// the rendering color of this Batch in vertex format (alpha compressed to 0-254)
    /// </returns>
    public float PackedColor { get; set; }

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y having the given width and height in
    /// pixels. The rectangle is offset by originX, originY relative to the origin. Scale specifies
    /// the scaling factor by which the rectangle should be scaled around originX, originY. Rotation
    /// specifies the angle of counter clockwise rotation of the rectangle around originX, originY. The
    /// portion of the <see cref="Texture"/> given by srcX, srcY and srcWidth, srcHeight is used.
    /// These coordinates and sizes are given in texels. FlipX and FlipY specify whether the texture
    /// portion should be flipped horizontally or vertically.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="x"> the x-coordinate in screen space </param>
    /// <param name="y"> the y-coordinate in screen space </param>
    /// <param name="originX">
    /// the x-coordinate of the scaling and rotation origin relative to the screen space coordinates
    /// </param>
    /// <param name="originY">
    /// the y-coordinate of the scaling and rotation origin relative to the screen space coordinates
    /// </param>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels </param>
    /// <param name="scaleX"> the scale of the rectangle around originX/originY in x </param>
    /// <param name="scaleY"> the scale of the rectangle around originX/originY in y </param>
    /// <param name="rotation">
    /// the angle of counter clockwise rotation of the rectangle around originX/originY
    /// </param>
    /// <param name="srcX"> the x-coordinate in texel space </param>
    /// <param name="srcY"> the y-coordinate in texel space </param>
    /// <param name="srcWidth"> the source with in texels </param>
    /// <param name="srcHeight"> the source height in texels </param>
    /// <param name="flipX"> whether to flip the sprite horizontally </param>
    /// <param name="flipY"> whether to flip the sprite vertically  </param>
    public void Draw( Texture texture,
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
                      bool flipY );

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y having the given width and height
    /// in pixels. The portion of the <see cref="Texture"/> given by srcX, srcY and srcWidth,
    /// srcHeight is used. These coordinates and sizes are given in texels. FlipX and flipY
    /// specify whether the texture portion should be flipped horizontally or vertically.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="x"> the x-coordinate in screen space </param>
    /// <param name="y"> the y-coordinate in screen space </param>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels </param>
    /// <param name="srcX"> the x-coordinate in texel space </param>
    /// <param name="srcY"> the y-coordinate in texel space </param>
    /// <param name="srcWidth"> the source with in texels </param>
    /// <param name="srcHeight"> the source height in texels </param>
    /// <param name="flipX"> whether to flip the sprite horizontally </param>
    /// <param name="flipY"> whether to flip the sprite vertically  </param>
    public void Draw( Texture texture,
                      float x,
                      float y,
                      float width,
                      float height,
                      int srcX,
                      int srcY,
                      int srcWidth,
                      int srcHeight,
                      bool flipX,
                      bool flipY );

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y having the given width and height
    /// in pixels. The portion of the <see cref="Texture"/> given by srcX, srcY and srcWidth,
    /// srcHeight are used. These coordinates and sizes are given in texels.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="x"> the x-coordinate in screen space </param>
    /// <param name="y"> the y-coordinate in screen space </param>
    /// <param name="srcX"> the x-coordinate in texel space </param>
    /// <param name="srcY"> the y-coordinate in texel space </param>
    /// <param name="srcWidth"> the source with in texels </param>
    /// <param name="srcHeight"> the source height in texels  </param>
    public void Draw( Texture texture, float x, float y, int srcX, int srcY, int srcWidth, int srcHeight );

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y having the given width and height
    /// in pixels. The portion of the <see cref="Texture"/> given by u, v and u2, v2 are used.
    /// These coordinates and sizes are given in texture size percentage. The rectangle will
    /// have the given tint <see cref="Color"/>.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="x"> the x-coordinate in screen space </param>
    /// <param name="y"> the y-coordinate in screen space </param>
    /// <param name="width"> the width in pixels </param>
    /// <param name="height"> the height in pixels  </param>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <param name="u2"></param>
    /// <param name="v2"></param>
    public void Draw( Texture texture, float x, float y, float width, float height, float u, float v, float u2, float v2 );

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y having the width and
    /// height of the texture.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="x"> the x-coordinate in screen space </param>
    /// <param name="y"> the y-coordinate in screen space  </param>
    public void Draw( Texture texture, float x, float y );

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y and stretching the region
    /// to cover the given width and height.
    /// </summary>
    public void Draw( Texture texture, float x, float y, float width, float height );

    /// <summary>
    /// Draws a rectangle using the given vertices. There must be 4 vertices, each made
    /// up of 5 elements in this order: x, y, color, u, v. The <see cref="Color"/>
    /// from the Batch is not applied. 
    /// </summary>
    public void Draw( Texture texture, float[] spriteVertices, int offset, int count );

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y having the width and
    /// height of the region.
    /// </summary>
    public void Draw( TextureRegion region, float x, float y );

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y and stretching the region to cover
    /// the given width and height.
    /// </summary>
    public void Draw( TextureRegion region, float x, float y, float width, float height );

    /// <summary>
    /// Draws a rectangle with the bottom left corner at x,y and stretching the region to
    /// cover the given width and height. The rectangle is offset by originX, originY relative
    /// to the origin. Scale specifies the scaling factor by which the rectangle should be scaled
    /// around originX, originY. Rotation specifies the angle of counter clockwise rotation
    /// of the rectangle around originX, originY. 
    /// </summary>
    public void Draw( TextureRegion region,
                      float x,
                      float y,
                      float originX,
                      float originY,
                      float width,
                      float height,
                      float scaleX,
                      float scaleY,
                      float rotation );

    /// <summary>
    /// Draws a rectangle with the texture coordinates rotated 90 degrees. The bottom left corner
    /// at x,y and stretching the region to cover the given width and height. The rectangle is
    /// offset by originX, originY relative to the origin. Scale specifies the scaling factor by
    /// which the rectangle should be scaled around originX, originY. Rotation specifies the angle
    /// of counter clockwise rotation of the rectangle around originX, originY.
    /// </summary>
    /// <param name="region"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="originX"></param>
    /// <param name="originY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <param name="rotation"></param>
    /// <param name="clockwise">
    /// If true, the texture coordinates are rotated 90 degrees clockwise.
    /// If false, they are rotated 90 degrees counter clockwise.
    /// </param>
    public void Draw( TextureRegion region,
                      float x,
                      float y,
                      float originX,
                      float originY,
                      float width,
                      float height,
                      float scaleX,
                      float scaleY,
                      float rotation,
                      bool clockwise );

    /// <summary>
    /// Draws a rectangle transformed by the given matrix.
    /// </summary>
    public void Draw( TextureRegion region, float width, float height, Affine2 transform );

    /// <summary>
    /// Causes any pending sprites to be rendered, without ending the Batch.
    /// </summary>
    public void Flush();

    /// <summary>
    /// Disables blending for drawing sprites.
    /// Calling this within <see cref="Begin()"/> / <see cref="End()"/> will flush the batch.
    /// </summary>
    public void DisableBlending();

    /// <summary>
    /// Enables blending for drawing sprites.
    /// Calling this within <see cref="Begin()"/> / <see cref="End()"/> will flush the batch.
    /// </summary>
    public void EnableBlending();

    /// <summary>
    /// Sets the blending function to be used when rendering sprites.
    /// </summary>
    /// <param name="srcFunc">
    /// the source function, e.g. GL20.GL_SRC_ALPHA.
    /// If set to -1, Batch won't change the blending function.
    /// </param>
    /// <param name="dstFunc"> the destination function, e.g. GL20.GL_ONE_MINUS_SRC_ALPHA </param>
    public void SetBlendFunction( int srcFunc, int dstFunc );

    /// <summary>
    /// Sets separate (color/alpha) blending function to be used when rendering sprites.
    /// </summary>
    /// <param name="srcFuncColor">
    /// the source color function, e.g. GL20.GL_SRC_ALPHA.
    /// If set to -1, Batch won't change the blending function.
    /// </param>
    /// <param name="dstFuncColor"> the destination color function, e.g. GL20.GL_ONE_MINUS_SRC_ALPHA. </param>
    /// <param name="srcFuncAlpha"> the source alpha function, e.g. GL20.GL_SRC_ALPHA. </param>
    /// <param name="dstFuncAlpha"> the destination alpha function, e.g. GL20.GL_ONE_MINUS_SRC_ALPHA.
    ///  </param>
    public void SetBlendFunctionSeparate( int srcFuncColor, int dstFuncColor, int srcFuncAlpha, int dstFuncAlpha );

    public int BlendSrcFunc { get; }

    public int BlendDstFunc { get; }

    public int BlendSrcFuncAlpha { get; }

    public int BlendDstFuncAlpha { get; }

    /// <summary>
    /// Returns the current projection matrix.
    /// Changing this within <see cref="Begin()"/> / <see cref="End()"/> results in undefined behaviour.
    /// </summary>
    public Matrix4 ProjectionMatrix { get; }

    /// <summary>
    /// Returns the current transform matrix.
    /// Changing this within <see cref="Begin()"/> / <see cref="End()"/> results in undefined behaviour.
    /// </summary>
    public Matrix4 TransformMatrix { get; }

    /// <summary>
    /// Sets the projection matrix to be used by this Batch.
    /// If this is called inside a <see cref="Begin()"/> / <see cref="End()"/> block, the
    /// current batch is flushed to the gpu. 
    /// </summary>
    public void SetProjectionMatrix( Matrix4 projection );

    /// <summary>
    /// Sets the transform matrix to be used by this Batch.
    /// </summary>
    public void SetTransformMatrix( Matrix4 transform );

    /// <summary>
    /// Sets the shader to be used in a GLES 2.0 environment. Vertex position attribute is
    /// called "a_position", the texture coordinates attribute is called "a_texCoord0", the
    /// color attribute is called "a_color".
    /// <p>
    /// See <see cref="ShaderProgram.POSITION_ATTRIBUTE"/>, <see cref="ShaderProgram.COLOR_ATTRIBUTE"/>
    /// and <seealso cref="ShaderProgram.TEXCOORD_ATTRIBUTE"/> which gets "0" appended to indicate
    /// the use of the first texture unit.
    /// </p>
    /// <p>
    /// The combined transform and projection matrx is uploaded via a mat4 uniform called "u_projTrans".
    /// The texture sampler is passed via a uniform called "u_texture".
    /// </p>
    /// <para>
    /// Call this method with a null argument to use the default shader.
    /// </para>
    /// <para>
    /// This method will flush the batch before setting the new shader.
    /// It can be called inbetween <see cref="Begin()"/> and <see cref="End()"/>.
    /// </para>
    /// </summary>
    public ShaderProgram? Shader { get; set; }

    /// <returns> true if currently between begin and end. </returns>
    public bool IsDrawing { get; set; }
}