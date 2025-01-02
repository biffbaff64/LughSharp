// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team.
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using System.Numerics;

using static LughSharp.Lugh.Graphics.OpenGL.IGL;

namespace LughSharp.Lugh.Graphics.OpenGL;

[PublicAPI, SuppressMessage( "ReSharper", "InconsistentNaming" )]
public partial interface IGLBindings
{
    /// <summary>
    /// Returns a Tuple holding the version of OpenGL being used, obtained by
    /// calling <see cref="GetString"/> with the parameter IGL.GL_VERSION.
    /// </summary>
    ( int major, int minor ) GetOpenGLVersion();

    unsafe void MessageCallback( int source,
                                 int type,
                                 uint id,
                                 int severity,
                                 int length,
                                 string message,
                                 void* userParam );

    /// <summary>
    /// Specify whether front- or back-facing facets can be culled
    /// </summary>
    /// <param name="mode">
    /// Specifies whether front- or back-facing facets are candidates for culling. Symbolic constants
    /// <see cref="GL_FRONT"/>, <see cref="GL_BACK"/>, and <see cref="GL_FRONT_AND_BACK"/> are accepted.
    /// The initial value is <see cref="GL_BACK"/>.
    /// </param>
    void CullFace( int mode );

    /// <summary>
    /// Define front- and back-facing polygons
    /// </summary>
    /// <param name="mode">
    /// Specifies the orientation of front-facing polygons. Symbolic constants <see cref="GL_CW"/> and
    /// <see cref="GL_CCW"/> are accepted. The initial value is <see cref="GL_CCW"/>.
    /// </param>
    void FrontFace( int mode );

    /// <summary>
    /// Specify implementation-specific hints
    /// </summary>
    /// <param name="target">
    /// Specifies a symbolic constant indicating the behavior to be controlled.
    /// <see cref="GL_LINE_SMOOTH_HINT"/>, <see cref="GL_POLYGON_SMOOTH_HINT"/>,
    /// <see cref="GL_TEXTURE_COMPRESSION_HINT"/>, and <see cref="GL_FRAGMENT_SHADER_DERIVATIVE_HINT"/> are accepted.
    /// </param>
    /// <param name="mode">
    /// Specifies a symbolic constant indicating the desired behavior. <see cref="GL_FASTEST"/>,
    /// <see cref="GL_NICEST"/>, and <see cref="GL_DONT_CARE"/> are accepted.
    /// </param>
    void Hint( int target, int mode );

    /// <summary>
    /// Specify the width of rasterized lines
    /// </summary>
    /// <param name="width">Specifies the width of rasterized lines. The initial value is 1.0.</param>
    void LineWidth( float width );

    /// <summary>
    /// Specify the diameter of rasterized points
    /// </summary>
    /// <param name="size">Specifies the diameter of rasterized points. The initial value is 1.0.</param>
    void PointSize( float size );

    /// <summary>
    /// Select a polygon rasterization mode
    /// </summary>
    /// <param name="face">
    /// Specifies the polygons that mode applies to. Must be <see cref="GL_FRONT_AND_BACK"/> for both
    /// front- and back-facing polygons.
    /// </param>
    /// <param name="mode">
    /// Specifies how polygons will be rasterized. Accepted values are <see cref="GL_POINT"/>,
    /// <see cref="GL_LINE"/>, and <see cref="GL_FILL"/>. The initial value is <see cref="GL_FILL"/> for both front- and
    /// back-facing polygons.
    /// </param>
    void PolygonMode( int face, int mode );

    /// <summary>
    /// Define the scissor box
    /// </summary>
    /// <param name="x">Specify the lower left corner of the scissor box. Initially (0, 0).</param>
    /// <param name="y">Specify the lower left corner of the scissor box. Initially (0, 0).</param>
    /// <param name="width">Specify the width of the scissor box.</param>
    /// <param name="height">Specify the height of the scissor box.</param>
    void Scissor( int x, int y, int width, int height );

    /// <summary>
    /// Set texture parameters
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound for TexParameter functions. Must be one of
    /// <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>, or <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter.
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, and <see cref="GL_TEXTURE_WRAP_R"/> are
    /// accepted. For the vector commands (those that end in v), <see cref="GL_TEXTURE_BORDER_COLOR"/> or
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/> is also acceptable.
    /// </param>
    /// <param name="param">Specifies the value of pname.</param>
    void TexParameterf( int target, int pname, float param );

    /// <summary>
    /// Set texture parameters
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound for TexParameter functions. Must be one of
    /// <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>, or <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter.
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, and <see cref="GL_TEXTURE_WRAP_R"/> are
    /// accepted. For the vector commands (those that end in v), <see cref="GL_TEXTURE_BORDER_COLOR"/> or
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/> is also acceptable.
    /// </param>
    /// <param name="params">Specifies the values of pname.</param>
    unsafe void TexParameterfv( int target, int pname, float* parameters );

    /// <summary>
    /// Set texture parameters
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound for TexParameter functions. Must be one of
    /// <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>, or <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter.
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, and <see cref="GL_TEXTURE_WRAP_R"/> are
    /// accepted. For the vector commands (those that end in v), <see cref="GL_TEXTURE_BORDER_COLOR"/> or
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/> is also acceptable.
    /// </param>
    /// <param name="params">Specifies the values of pname.</param>
    unsafe void TexParameterfv( int target, int pname, float[] parameters );

    /// <summary>
    /// Set texture parameters
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound for TexParameter functions. Must be one of
    /// <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>, or <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter.
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, and <see cref="GL_TEXTURE_WRAP_R"/> are
    /// accepted. For the vector commands (those that end in v), <see cref="GL_TEXTURE_BORDER_COLOR"/> or
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/> is also acceptable.
    /// </param>
    /// <param name="param">Specifies the value of pname.</param>
    void TexParameteri( int target, int pname, int param );

    /// <summary>
    /// Set texture parameters
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound for TexParameter functions. Must be one of
    /// <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>, or <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter.
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, and <see cref="GL_TEXTURE_WRAP_R"/> are
    /// accepted. For the vector commands (those that end in v), <see cref="GL_TEXTURE_BORDER_COLOR"/> or
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/> is also acceptable.
    /// </param>
    /// <param name="params">Specifies the values of pname.</param>
    unsafe void TexParameteriv( int target, int pname, int* parameters );

    /// <summary>
    /// Set texture parameters
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound for TexParameter functions. Must be one of
    /// <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>, or <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter.
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, and <see cref="GL_TEXTURE_WRAP_R"/> are
    /// accepted. For the vector commands (those that end in v), <see cref="GL_TEXTURE_BORDER_COLOR"/> or
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/> is also acceptable.
    /// </param>
    /// <param name="params">Specifies the values of pname.</param>
    unsafe void TexParameteriv( int target, int pname, int[] parameters );

    /// <summary>
    /// Specify a one-dimensional texture image
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/> or
    /// <see cref="GL_PROXY_TEXTURE_1D"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the number of color components in the texture. Refer to
    /// <see href="https://docs.gl/gl4/glTexImage1D"/> for a list of possible values.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support texture images that are at
    /// least 1024 texels wide. The height of the 1D texture image is 1.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_BGRA"/>, <see cref="GL_RED_INTEGER"/>, <see cref="GL_RG_INTEGER"/>, <see cref="GL_RGB_INTEGER"/>,
    /// <see cref="GL_BGR_INTEGER"/>, <see cref="GL_RGBA_INTEGER"/>, <see cref="GL_BGRA_INTEGER"/>,
    /// <see cref="GL_STENCIL_INDEX"/>, <see cref="GL_DEPTH_COMPONENT"/>, <see cref="GL_DEPTH_STENCIL"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">Specifies a pointer to the image data in memory.</param>
    unsafe void TexImage1D( int target, int level, int internalformat, int width, int border, int format, int type, void* pixels );

    /// <summary>
    /// Specify a one-dimensional texture image
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/> or
    /// <see cref="GL_PROXY_TEXTURE_1D"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the number of color components in the texture. Refer to
    /// <see href="https://docs.gl/gl4/glTexImage1D"/> for a list of possible values.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support texture images that are at
    /// least 1024 texels wide. The height of the 1D texture image is 1.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_BGRA"/>, <see cref="GL_RED_INTEGER"/>, <see cref="GL_RG_INTEGER"/>, <see cref="GL_RGB_INTEGER"/>,
    /// <see cref="GL_BGR_INTEGER"/>, <see cref="GL_RGBA_INTEGER"/>, <see cref="GL_BGRA_INTEGER"/>,
    /// <see cref="GL_STENCIL_INDEX"/>, <see cref="GL_DEPTH_COMPONENT"/>, <see cref="GL_DEPTH_STENCIL"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">
    /// Specifies the pixel data as an array of values. Make sure to match the generic type with the
    /// <paramref name="type"/> parameter.
    /// </param>
    unsafe void TexImage1D< T >( int target, int level, int internalformat, int width, int border, int format, int type, T[] pixels )
        where T : unmanaged;

    /// <summary>
    /// Specify a two-dimensional texture image
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_RECTANGLE"/>, <see cref="GL_PROXY_TEXTURE_RECTANGLE"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP_ARRAY"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, or
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image. If <paramref name="target"/> is <see cref="GL_TEXTURE_RECTANGLE"/> or
    /// <see cref="GL_PROXY_TEXTURE_RECTANGLE"/>, <paramref name="level"/> must be 0.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the number of color components in the texture. Refer to
    /// <see href="https://docs.gl/gl4/glTexImage2D"/> for the list of possible values.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support texture images that are at
    /// least 1024 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture image, or the number of layers in a texture array, in the case
    /// of the <see cref="GL_TEXTURE_1D_ARRAY"/> and <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/> targets. ALl implementations
    /// support 2D texture images that are at least 1024 texels high, and texture arrays that are at least 256 layers deep.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_BGRA"/>, <see cref="GL_RED_INTEGER"/>, <see cref="GL_RG_INTEGER"/>, <see cref="GL_RGB_INTEGER"/>,
    /// <see cref="GL_BGR_INTEGER"/>, <see cref="GL_RGBA_INTEGER"/>, <see cref="GL_BGRA_INTEGER"/>,
    /// <see cref="GL_STENCIL_INDEX"/>, <see cref="GL_DEPTH_COMPONENT"/>, <see cref="GL_DEPTH_STENCIL"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">Specifies a pointer to the image data in memory.</param>
    unsafe void TexImage2D( int target,
                              int level,
                              int internalformat,
                              int width,
                              int height,
                              int border,
                              int format,
                              int type,
                              void* pixels );

    /// <summary>
    /// Specify a two-dimensional texture image
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_RECTANGLE"/>, <see cref="GL_PROXY_TEXTURE_RECTANGLE"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP_ARRAY"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, or
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image. If <paramref name="target"/> is <see cref="GL_TEXTURE_RECTANGLE"/> or
    /// <see cref="GL_PROXY_TEXTURE_RECTANGLE"/>, <paramref name="level"/> must be 0.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the number of color components in the texture. Refer to
    /// <see href="https://docs.gl/gl4/glTexImage2D"/> for the list of possible values.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support texture images that are at
    /// least 1024 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture image, or the number of layers in a texture array, in the case
    /// of the <see cref="GL_TEXTURE_1D_ARRAY"/> and <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/> targets. ALl implementations
    /// support 2D texture images that are at least 1024 texels high, and texture arrays that are at least 256 layers deep.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_BGRA"/>, <see cref="GL_RED_INTEGER"/>, <see cref="GL_RG_INTEGER"/>, <see cref="GL_RGB_INTEGER"/>,
    /// <see cref="GL_BGR_INTEGER"/>, <see cref="GL_RGBA_INTEGER"/>, <see cref="GL_BGRA_INTEGER"/>,
    /// <see cref="GL_STENCIL_INDEX"/>, <see cref="GL_DEPTH_COMPONENT"/>, <see cref="GL_DEPTH_STENCIL"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">
    /// Specifies the pixel data as an array of values. Make sure to match the <paramref name="format"/>
    /// and <paramref name="type"/> parameters.
    /// </param>
    unsafe void TexImage2D< T >( int target,
                                   int level,
                                   int internalformat,
                                   int width,
                                   int height,
                                   int border,
                                   int format,
                                   int type,
                                   T[] pixels ) where T : unmanaged;

    /// <summary>
    /// Specify which color buffers are to be drawn into.
    /// </summary>
    /// <param name="buf">
    /// Specifies up to four color buffers to be drawn into. Must be one of <see cref="GL_NONE"/>,
    /// <see cref="GL_FRONT_LEFT"/>, <see cref="GL_FRONT_RIGHT"/>, <see cref="GL_BACK_LEFT"/>,
    /// <see cref="GL_BACK_RIGHT"/>, <see cref="GL_FRONT"/>, <see cref="GL_BACK"/>, <see cref="GL_LEFT"/>,
    /// <see cref="GL_RIGHT"/>, <see cref="GL_FRONT_AND_BACK"/>. The initial value is <see cref="GL_FRONT"/> for single
    /// buffered contexts, and <see cref="GL_BACK"/> for double buffered contexts.
    /// </param>
    void DrawBuffer( int buf );

    /// <summary>
    /// Clear buffers to preset values.
    /// </summary>
    /// <param name="mask">
    /// Bitwise OR of masks that indicate the buffers to be cleared. The three masks are
    /// <see cref="GL_COLOR_BUFFER_BIT"/>, <see cref="GL_DEPTH_BUFFER_BIT"/>, and <see cref="GL_STENCIL_BUFFER_BIT"/>.
    /// </param>
    void Clear( uint mask );

    /// <summary>
    /// Specify clear values for the color buffers.
    /// </summary>
    /// <param name="red">Specifies the red value used when the color buffers are cleared. The initial value is 0.</param>
    /// <param name="green">Specifies the green value used when the color buffers are cleared. The initial value is 0.</param>
    /// <param name="blue">Specifies the blue value used when the color buffers are cleared. The initial value is 0.</param>
    /// <param name="alpha">Specifies the alpha value used when the color buffers are cleared. The initial value is 0.</param>
    void ClearColor( float red, float green, float blue, float alpha );

    /// <summary>
    /// Specify the clear value for the stencil buffer.
    /// </summary>
    /// <param name="s">Specifies the index used when the stencil buffer is cleared. The initial value is 0.</param>
    void ClearStencil( int s );

    /// <summary>
    /// Specify the clear value for the depth buffer.
    /// </summary>
    /// <param name="depth">Specifies the depth value used when the depth buffer is cleared. The initial value is 1.</param>
    void ClearDepth( double depth );

    /// <summary>
    /// Control the front and back writing of individual bits in the stencil planes.
    /// </summary>
    /// <param name="mask">
    /// Specifies a bit mask to enable and disable writing of individual bits in the stencil planes.
    /// Initially, the mask is all 1's.
    /// </param>
    void StencilMask( uint mask );

    /// <summary>
    /// Enable and disable writing of frame buffer color components.
    /// </summary>
    /// <param name="red">Specifies whether red can or cannot be written into the frame buffer.</param>
    /// <param name="green">Specifies whether green can or cannot be written into the frame buffer.</param>
    /// <param name="blue">Specifies whether blue can or cannot be written into the frame buffer.</param>
    /// <param name="alpha">Specifies whether alpha can or cannot be written into the frame buffer.</param>
    void ColorMask( bool red, bool green, bool blue, bool alpha );

    /// <summary>
    /// Enable or disable writing into the depth buffer.
    /// </summary>
    /// <param name="flag">
    /// Specifies whether depth buffer writing is enabled or disabled. If flag is <see langword="false"/>,
    /// depth buffer writing is disabled. Otherwise, it is enabled. Initially, depth buffer writing is enabled.
    /// </param>
    void DepthMask( bool flag );

    /// <summary>
    /// Disable  capabilities.
    /// </summary>
    /// <param name="cap">
    /// Specifies a symbolic constant indicating a  capability to be disabled. Refer to
    /// <see href="https://docs.gl/gl4/glEnable"/> for a list of possible values.
    /// </param>
    void Disable( int cap );

    /// <summary>
    /// Enable  capabilities.
    /// </summary>
    /// <param name="cap">
    /// Specifies a symbolic constant indicating a  capability to be enabled. Refer to
    /// <see href="https://docs.gl/gl4/glEnable"/> for a list of possible values.
    /// </param>
    void Enable( int cap );

    /// <summary>
    /// Block until all  execution is complete.
    /// </summary>
    void Finish();

    /// <summary>
    /// Force execution of  commands in finite time.
    /// </summary>
    void Flush();

    /// <summary>
    /// Specify pixel arithmetic.
    /// </summary>
    /// <param name="sfactor">
    /// Specifies how the red, green, blue, and alpha source blending factors are computed. The initial
    /// value is <see cref="GL_ONE"/>. Allowed values are <see cref="GL_ZERO"/>, <see cref="GL_ONE"/>,
    /// <see cref="GL_SRC_COLOR"/>, <see cref="GL_ONE_MINUS_SRC_COLOR"/>, <see cref="GL_DST_COLOR"/>,
    /// <see cref="GL_ONE_MINUS_DST_COLOR"/>, <see cref="GL_SRC_ALPHA"/>, <see cref="GL_ONE_MINUS_SRC_ALPHA"/>,
    /// <see cref="GL_DST_ALPHA"/>, <see cref="GL_ONE_MINUS_DST_ALPHA"/>, <see cref="GL_CONSTANT_COLOR"/>,
    /// <see cref="GL_ONE_MINUS_CONSTANT_COLOR"/>, <see cref="GL_CONSTANT_ALPHA"/>, and
    /// <see cref="GL_ONE_MINUS_CONSTANT_ALPHA"/>
    /// </param>
    /// <param name="dfactor">
    /// Specifies how the red, green, blue, and alpha destination blending factors are computed. The
    /// initial value is <see cref="GL_ZERO"/>. Allowed values are <see cref="GL_ZERO"/>, <see cref="GL_ONE"/>,
    /// <see cref="GL_SRC_COLOR"/>, <see cref="GL_ONE_MINUS_SRC_COLOR"/>, <see cref="GL_DST_COLOR"/>,
    /// <see cref="GL_ONE_MINUS_DST_COLOR"/>, <see cref="GL_SRC_ALPHA"/>, <see cref="GL_ONE_MINUS_SRC_ALPHA"/>,
    /// <see cref="GL_DST_ALPHA"/>, <see cref="GL_ONE_MINUS_DST_ALPHA"/>, <see cref="GL_CONSTANT_COLOR"/>,
    /// <see cref="GL_ONE_MINUS_CONSTANT_COLOR"/>, <see cref="GL_CONSTANT_ALPHA"/>, and
    /// <see cref="GL_ONE_MINUS_CONSTANT_ALPHA"/>.
    /// </param>
    void BlendFunc( int sfactor, int dfactor );

    /// <summary>
    /// Specify a logical pixel operation for rendering.
    /// </summary>
    /// <param name="opcode">
    /// Specifies a symbolic constant that selects a logical operation. The following symbols are
    /// accepted: <see cref="GL_CLEAR"/>, <see cref="GL_SET"/>, <see cref="GL_COPY"/>, <see cref="GL_COPY_INVERTED"/>,
    /// <see cref="GL_NOOP"/>, <see cref="GL_INVERT"/>, <see cref="GL_AND"/>, <see cref="GL_NAND"/>,
    /// <see cref="GL_OR"/>, <see cref="GL_NOR"/>, <see cref="GL_XOR"/>, <see cref="GL_EQUIV"/>,
    /// <see cref="GL_AND_REVERSE"/>, <see cref="GL_AND_INVERTED"/>, <see cref="GL_OR_REVERSE"/>, and
    /// <see cref="GL_OR_INVERTED"/>. The initial value is <see cref="GL_COPY"/>.
    /// </param>
    void LogicOp( int opcode );

    /// <summary>
    /// Set front and back function and reference value for stencil testing.
    /// </summary>
    /// <param name="func">
    /// Specifies the test function. Eight symbolic constants are accepted: <see cref="GL_NEVER"/>,
    /// <see cref="GL_LESS"/>, <see cref="GL_LEQUAL"/>, <see cref="GL_GREATER"/>, <see cref="GL_GEQUAL"/>,
    /// <see cref="GL_EQUAL"/>, <see cref="GL_NOTEQUAL"/>, and <see cref="GL_ALWAYS"/>. The initial value is
    /// <see cref="GL_ALWAYS"/>.
    /// </param>
    /// <param name="ref">
    /// Specifies the reference value for the stencil test. <paramref name="ref"/> is clamped to the range
    /// [0,(2^n) - 1], where n is the number of bitplanes in the stencil buffer. The initial value is 0.
    /// </param>
    /// <param name="mask">
    /// Specifies a mask that is ANDed with both the reference value and the stored stencil value when the
    /// test is done. The initial value is all 1's.
    /// </param>
    void StencilFunc( int func, int @ref, uint mask );

    /// <summary>
    /// Set front and back stencil test actions.
    /// </summary>
    /// <param name="fail">
    /// Specifies the action to take when the stencil test fails. Eight symbolic constants are accepted:
    /// <see cref="GL_KEEP"/>, <see cref="GL_ZERO"/>, <see cref="GL_REPLACE"/>, <see cref="GL_INCR"/>,
    /// <see cref="GL_INCR_WRAP"/>, <see cref="GL_DECR"/>, <see cref="GL_DECR_WRAP"/>, and <see cref="GL_INVERT"/>. The
    /// initial value is <see cref="GL_KEEP"/>.
    /// </param>
    /// <param name="zfail">
    /// Specifies the stencil action when the stencil test passes, but the depth test fails.
    /// <paramref name="zfail"/> accepts the same symbolic constants as <paramref name="fail"/>. The initial value is
    /// <see cref="GL_KEEP"/>.
    /// </param>
    /// <param name="zpass">
    /// Specifies the stencil action when both the stencil test and the depth test pass, or when the
    /// stencil test passes and either there is no depth buffer or depth testing is not enabled. <paramref name="zpass"/>
    /// accepts the same symbolic constants as <paramref name="fail"/>. The initial value is <see cref="GL_KEEP"/>.
    /// </param>
    void StencilOp( int fail, int zfail, int zpass );

    /// <summary>
    /// Specify the value used for depth buffer comparisons.
    /// </summary>
    /// <param name="func">
    /// Specifies the depth comparison function. Symbolic constants <see cref="GL_NEVER"/>,
    /// <see cref="GL_LESS"/>, <see cref="GL_EQUAL"/>, <see cref="GL_LEQUAL"/>, <see cref="GL_GREATER"/>,
    /// <see cref="GL_NOTEQUAL"/>, <see cref="GL_GEQUAL"/>, and <see cref="GL_ALWAYS"/> are accepted. The initial value
    /// is <see cref="GL_LESS"/>.
    /// </param>
    void DepthFunc( int func );

    /// <summary>
    /// Set pixel storage modes.
    /// </summary>
    /// <param name="pname">
    /// Specifies the symbolic name of the parameter to be set. The following values affect the packing of
    /// pixel data into memory: <see cref="GL_PACK_SWAP_BYTES"/>, <see cref="GL_PACK_LSB_FIRST"/>,
    /// <see cref="GL_PACK_ROW_LENGTH"/>, <see cref="GL_PACK_IMAGE_HEIGHT"/>, <see cref="GL_PACK_SKIP_PIXELS"/>,
    /// <see cref="GL_PACK_SKIP_ROWS"/>, <see cref="GL_PACK_SKIP_IMAGES"/>, and <see cref="GL_PACK_ALIGNMENT"/>. The
    /// following values affect the unpacking of pixel data from memory: <see cref="GL_UNPACK_SWAP_BYTES"/>,
    /// <see cref="GL_UNPACK_LSB_FIRST"/>, <see cref="GL_UNPACK_ROW_LENGTH"/>, <see cref="GL_UNPACK_IMAGE_HEIGHT"/>,
    /// <see cref="GL_UNPACK_SKIP_PIXELS"/>, <see cref="GL_UNPACK_SKIP_ROWS"/>, <see cref="GL_UNPACK_SKIP_IMAGES"/>, and
    /// <see cref="GL_UNPACK_ALIGNMENT"/>.
    /// </param>
    /// <param name="param">Specifies the value that <paramref name="pname"/> is set to.</param>
    void PixelStoref( int pname, float param );

    /// <summary>
    /// Set pixel storage modes.
    /// </summary>
    /// <param name="pname">
    /// Specifies the symbolic name of the parameter to be set. The following values affect the packing of
    /// pixel data into memory: <see cref="GL_PACK_SWAP_BYTES"/>, <see cref="GL_PACK_LSB_FIRST"/>,
    /// <see cref="GL_PACK_ROW_LENGTH"/>, <see cref="GL_PACK_IMAGE_HEIGHT"/>, <see cref="GL_PACK_SKIP_PIXELS"/>,
    /// <see cref="GL_PACK_SKIP_ROWS"/>, <see cref="GL_PACK_SKIP_IMAGES"/>, and <see cref="GL_PACK_ALIGNMENT"/>. The
    /// following values affect the unpacking of pixel data from memory: <see cref="GL_UNPACK_SWAP_BYTES"/>,
    /// <see cref="GL_UNPACK_LSB_FIRST"/>, <see cref="GL_UNPACK_ROW_LENGTH"/>, <see cref="GL_UNPACK_IMAGE_HEIGHT"/>,
    /// <see cref="GL_UNPACK_SKIP_PIXELS"/>, <see cref="GL_UNPACK_SKIP_ROWS"/>, <see cref="GL_UNPACK_SKIP_IMAGES"/>, and
    /// <see cref="GL_UNPACK_ALIGNMENT"/>.
    /// </param>
    /// <param name="param">Specifies the value that <paramref name="pname"/> is set to.</param>
    void PixelStorei( int pname, int param );

    /// <summary>
    /// Select a color buffer source for pixels.
    /// </summary>
    /// <param name="src">
    /// Specifies a color buffer. Accepted values are <see cref="GL_FRONT_LEFT"/>,
    /// <see cref="GL_FRONT_RIGHT"/>, <see cref="GL_BACK_LEFT"/>, <see cref="GL_BACK_RIGHT"/>, <see cref="GL_FRONT"/>,
    /// <see cref="GL_BACK"/>, <see cref="GL_LEFT"/>, <see cref="GL_RIGHT"/>, and the constants
    /// <see cref="GL_COLOR_ATTACHMENT0"/> through <see cref="GL_COLOR_ATTACHMENT31"/>.
    /// </param>
    void ReadBuffer( int src );

    /// <summary>
    /// Read a block of pixels from the frame buffer.
    /// </summary>
    /// <param name="x">
    /// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is
    /// the lower left corner of a rectangular block of pixels.
    /// </param>
    /// <param name="y">
    /// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is
    /// the lower left corner of a rectangular block of pixels.
    /// </param>
    /// <param name="width">
    /// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single
    /// pixel.
    /// </param>
    /// <param name="height">
    /// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single
    /// pixel.
    /// </param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_STENCIL_INDEX"/>, <see cref="GL_DEPTH_COMPONENT"/>, <see cref="GL_DEPTH_STENCIL"/>,
    /// <see cref="GL_RED"/>, <see cref="GL_GREEN"/>, <see cref="GL_BLUE"/>, <see cref="GL_RGB"/>,
    /// <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>, and <see cref="GL_BGRA"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_24_8"/>, <see cref="GL_UNSIGNED_INT_10F_11F_11F_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_5_9_9_9_REV"/>, and <see cref="GL_FLOAT_32_UNSIGNED_INT_24_8_REV"/>.
    /// </param>
    /// <param name="pixels">A pointer to somewhere in memory where the pixel data will be returned.</param>
    unsafe void ReadPixels( int x, int y, int width, int height, int format, int type, void* pixels );

    /// <summary>
    /// Read a block of pixels from the frame buffer.
    /// </summary>
    /// <param name="x">
    /// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is
    /// the lower left corner of a rectangular block of pixels.
    /// </param>
    /// <param name="y">
    /// Specify the window coordinates of the first pixel that is read from the frame buffer. This location is
    /// the lower left corner of a rectangular block of pixels.
    /// </param>
    /// <param name="width">
    /// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single
    /// pixel.
    /// </param>
    /// <param name="height">
    /// Specify the dimensions of the pixel rectangle. width and height of one correspond to a single
    /// pixel.
    /// </param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_STENCIL_INDEX"/>, <see cref="GL_DEPTH_COMPONENT"/>, <see cref="GL_DEPTH_STENCIL"/>,
    /// <see cref="GL_RED"/>, <see cref="GL_GREEN"/>, <see cref="GL_BLUE"/>, <see cref="GL_RGB"/>,
    /// <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>, and <see cref="GL_BGRA"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_24_8"/>, <see cref="GL_UNSIGNED_INT_10F_11F_11F_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_5_9_9_9_REV"/>, and <see cref="GL_FLOAT_32_UNSIGNED_INT_24_8_REV"/>.
    /// </param>
    /// <param name="pixels">
    /// A <see langword="ref"/> to an array of <typeparamref name="T"/>s where the pixel data will be
    /// returned. Make sure to match the type of the data with the type in <paramref name="type"/>.
    /// </param>
    unsafe void ReadPixels< T >( int x, int y, int width, int height, int format, int type, ref T[] pixels ) where T : unmanaged;

    /// <summary>
    /// Return the boolean value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">A pointer to where the boolean value or values will be returned.</param>
    unsafe void GetBooleanv( int pname, bool* data );

    /// <summary>
    /// Return the boolean value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">
    /// A <see langword="ref"/> to an array of <see langword="bool"/>s where the boolean value or values
    /// will be returned.
    /// </param>
    unsafe void GetBooleanv( int pname, ref bool[] data );

    /// <summary>
    /// Return the double value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">A pointer to where the double value or values will be returned.</param>
    unsafe void GetDoublev( int pname, double* data );

    /// <summary>
    /// Return the double value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">
    /// A <see langword="ref"/> to an array of <see langword="double"/>s where the double value or values
    /// will be returned.
    /// </param>
    unsafe void GetDoublev( int pname, ref double[] data );

    /// <summary>
    /// Return error information.
    /// </summary>
    /// <returns>
    /// One of <see cref="GL_NO_ERROR"/>, <see cref="GL_INVALID_ENUM"/>, <see cref="GL_INVALID_VALUE"/>,
    /// <see cref="GL_INVALID_OPERATION"/>, <see cref="GL_INVALID_FRAMEBUFFER_OPERATION"/>,
    /// <see cref="GL_OUT_OF_MEMORY"/>, <see cref="GL_STACK_UNDERFLOW"/>, or <see cref="GL_STACK_OVERFLOW"/>.
    /// </returns>
    int GetError();

    /// <summary>
    /// Return the float value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">A pointer to where the float value or values will be returned.</param>
    unsafe void GetFloatv( int pname, float* data );

    /// <summary>
    /// Return the float value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    unsafe void GetFloatv( int pname, ref float[] data );

    /// <summary>
    /// Return the integer value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">A pointer to where the integer value or values will be returned.</param>
    unsafe void GetIntegerv( int pname, int* data );

    /// <summary>
    /// Return the integer value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">
    /// A <see langword="ref"/> to an array of <see langword="int"/>s where the integer value or values
    /// will be returned.
    /// </param>
    unsafe void GetIntegerv( int pname, ref int[] data );

    /// <summary>
    /// Return a string describing the current  connection.
    /// </summary>
    /// <param name="name">
    /// Specifies a symbolic constant, one of <see cref="GL_VENDOR"/>, <see cref="GL_RENDERER"/>,
    /// <see cref="GL_VERSION"/>, or <see cref="GL_SHADING_LANGUAGE_VERSION"/>. Additionally, <see cref="GLBindings.glGetStringi"/>
    /// accepts <see cref="GL_EXTENSIONS"/>.
    /// </param>
    /// <returns>The requested string as a <see cref="byte"/> pointer.</returns>
    unsafe Byte* GetString( int name );

    /// <summary>
    /// Return a string describing the current  connection.
    /// </summary>
    /// <param name="name">
    /// Specifies a symbolic constant, one of <see cref="GL_VENDOR"/>, <see cref="GL_RENDERER"/>,
    /// <see cref="GL_VERSION"/>, or <see cref="GL_SHADING_LANGUAGE_VERSION"/>. Additionally, <see cref="GLBindings.glGetStringi"/>
    /// accepts <see cref="GL_EXTENSIONS"/>.
    /// </param>
    /// <returns>The requested string as a managed string.</returns>
    unsafe string GetStringSafe( int name );

    /// <summary>
    /// Return a texture image.
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound for. <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_RECTANGLE"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> and
    /// <see cref="GL_TEXTURE_CUBE_MAP"/> are accepted.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number of the desired image. Level 0 is the base image level. Level n
    /// is the nth mipmap reduction image.
    /// </param>
    /// <param name="format">
    /// Specifies a pixel format for the returned data. The supported formats are
    /// <see cref="GL_STENCIL"/>, <see cref="GL_DEPTH_COMPONENT"/>, <see cref="GL_DEPTH_STENCIL"/>,
    /// <see cref="GL_RED"/>, <see cref="GL_GREEN"/>, <see cref="GL_BLUE"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>
    /// , <see cref="GL_RGBA"/>, <see cref="GL_BGR"/>, <see cref="GL_BGRA"/>, <see cref="GL_RED_INTEGER"/>,
    /// <see cref="GL_GREEN_INTEGER"/>, <see cref="GL_BLUE_INTEGER"/>, <see cref="GL_RG_INTEGER"/>,
    /// <see cref="GL_RGB_INTEGER"/>, <see cref="GL_RGBA_INTEGER"/>, <see cref="GL_BGR_INTEGER"/> and
    /// <see cref="GL_BGRA_INTEGER"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_24_8"/>, <see cref="GL_UNSIGNED_INT_10F_11F_11F_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_5_9_9_9_REV"/>, and <see cref="GL_FLOAT_32_UNSIGNED_INT_24_8_REV"/>.
    /// </param>
    /// <param name="pixels">A pointer to a memory location where the pixel data will be returned.</param>
    unsafe void GetTexImage( int target, int level, int format, int type, void* pixels );

    /// <summary>
    /// Return a texture image.
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound for. <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_RECTANGLE"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> and
    /// <see cref="GL_TEXTURE_CUBE_MAP"/> are accepted.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number of the desired image. Level 0 is the base image level. Level n
    /// is the nth mipmap reduction image.
    /// </param>
    /// <param name="format">
    /// Specifies a pixel format for the returned data. The supported formats are
    /// <see cref="GL_STENCIL"/>, <see cref="GL_DEPTH_COMPONENT"/>, <see cref="GL_DEPTH_STENCIL"/>,
    /// <see cref="GL_RED"/>, <see cref="GL_GREEN"/>, <see cref="GL_BLUE"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>
    /// , <see cref="GL_RGBA"/>, <see cref="GL_BGR"/>, <see cref="GL_BGRA"/>, <see cref="GL_RED_INTEGER"/>,
    /// <see cref="GL_GREEN_INTEGER"/>, <see cref="GL_BLUE_INTEGER"/>, <see cref="GL_RG_INTEGER"/>,
    /// <see cref="GL_RGB_INTEGER"/>, <see cref="GL_RGBA_INTEGER"/>, <see cref="GL_BGR_INTEGER"/> and
    /// <see cref="GL_BGRA_INTEGER"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_24_8"/>, <see cref="GL_UNSIGNED_INT_10F_11F_11F_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_5_9_9_9_REV"/>, and <see cref="GL_FLOAT_32_UNSIGNED_INT_24_8_REV"/>.
    /// </param>
    /// <param name="pixels">
    /// A <see langword="ref"/> to an array of <typeparamref name="T"/>s where the pixel data will be
    /// returned.
    /// </param>
    unsafe void GetTexImage< T >( int target, int level, int format, int type, ref T[] pixels ) where T : unmanaged;

    /// <summary>
    /// Return texture parameter (float) values.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> and <see cref="GL_TEXTURE_RECTANGLE"/> are accepted.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a texture parameter. <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>,
    /// <see cref="GL_IMAGE_FORMAT_COMPATIBILITY_TYPE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_BORDER_COLOR"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_IMMUTABLE_FORMAT"/>,
    /// <see cref="GL_TEXTURE_IMMUTABLE_LEVELS"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_MAX_LOD"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/>, <see cref="GL_TEXTURE_TARGET"/>, <see cref="GL_TEXTURE_VIEW_MIN_LAYER"/>,
    /// <see cref="GL_TEXTURE_VIEW_MIN_LEVEL"/>, <see cref="GL_TEXTURE_VIEW_NUM_LAYERS"/>,
    /// <see cref="GL_TEXTURE_VIEW_NUM_LEVELS"/>, <see cref="GL_TEXTURE_WRAP_R"/>, <see cref="GL_TEXTURE_WRAP_S"/>, and
    /// <see cref="GL_TEXTURE_WRAP_T"/> are accepted.
    /// </param>
    /// <param name="params">A pointer to a float array where the values will be returned.</param>
    unsafe void GetTexParameterfv( int target, int pname, float* parameters );

    /// <summary>
    /// Return texture parameter (float) values.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> and <see cref="GL_TEXTURE_RECTANGLE"/> are accepted.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a texture parameter. <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>,
    /// <see cref="GL_IMAGE_FORMAT_COMPATIBILITY_TYPE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_BORDER_COLOR"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_IMMUTABLE_FORMAT"/>,
    /// <see cref="GL_TEXTURE_IMMUTABLE_LEVELS"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_MAX_LOD"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/>, <see cref="GL_TEXTURE_TARGET"/>, <see cref="GL_TEXTURE_VIEW_MIN_LAYER"/>,
    /// <see cref="GL_TEXTURE_VIEW_MIN_LEVEL"/>, <see cref="GL_TEXTURE_VIEW_NUM_LAYERS"/>,
    /// <see cref="GL_TEXTURE_VIEW_NUM_LEVELS"/>, <see cref="GL_TEXTURE_WRAP_R"/>, <see cref="GL_TEXTURE_WRAP_S"/>, and
    /// <see cref="GL_TEXTURE_WRAP_T"/> are accepted.
    /// </param>
    /// <param name="params">A <see langword="ref"/> to a float array where the values will be returned.</param>
    unsafe void GetTexParameterfv( int target, int pname, ref float[] parameters );

    /// <summary>
    /// Return texture parameter (integer) values.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> and <see cref="GL_TEXTURE_RECTANGLE"/> are accepted.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a texture parameter. <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>,
    /// <see cref="GL_IMAGE_FORMAT_COMPATIBILITY_TYPE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_BORDER_COLOR"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_IMMUTABLE_FORMAT"/>,
    /// <see cref="GL_TEXTURE_IMMUTABLE_LEVELS"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_MAX_LOD"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/>, <see cref="GL_TEXTURE_TARGET"/>, <see cref="GL_TEXTURE_VIEW_MIN_LAYER"/>,
    /// <see cref="GL_TEXTURE_VIEW_MIN_LEVEL"/>, <see cref="GL_TEXTURE_VIEW_NUM_LAYERS"/>,
    /// <see cref="GL_TEXTURE_VIEW_NUM_LEVELS"/>, <see cref="GL_TEXTURE_WRAP_R"/>, <see cref="GL_TEXTURE_WRAP_S"/>, and
    /// <see cref="GL_TEXTURE_WRAP_T"/> are accepted.
    /// </param>
    /// <param name="params">A pointer to an integer array where the values will be returned.</param>
    unsafe void GetTexParameteriv( int target, int pname, int* parameters );

    /// <summary>
    /// Return texture parameter (integer) values.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> and <see cref="GL_TEXTURE_RECTANGLE"/> are accepted.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a texture parameter. <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>,
    /// <see cref="GL_IMAGE_FORMAT_COMPATIBILITY_TYPE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_BORDER_COLOR"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_IMMUTABLE_FORMAT"/>,
    /// <see cref="GL_TEXTURE_IMMUTABLE_LEVELS"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_MAX_LOD"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_RGBA"/>, <see cref="GL_TEXTURE_TARGET"/>, <see cref="GL_TEXTURE_VIEW_MIN_LAYER"/>,
    /// <see cref="GL_TEXTURE_VIEW_MIN_LEVEL"/>, <see cref="GL_TEXTURE_VIEW_NUM_LAYERS"/>,
    /// <see cref="GL_TEXTURE_VIEW_NUM_LEVELS"/>, <see cref="GL_TEXTURE_WRAP_R"/>, <see cref="GL_TEXTURE_WRAP_S"/>, and
    /// <see cref="GL_TEXTURE_WRAP_T"/> are accepted.
    /// </param>
    /// <param name="params">A <see langword="ref"/> to an integer array where the values will be returned.</param>
    unsafe void GetTexParameteriv( int target, int pname, ref int[] parameters );

    /// <summary>
    /// Return texture parameter (float) values for a specific level of detail.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_RECTANGLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>, <see cref="GL_PROXY_TEXTURE_1D"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D"/>, <see cref="GL_PROXY_TEXTURE_3D"/>, <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>, <see cref="GL_PROXY_TEXTURE_RECTANGLE"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP"/> or <see cref="GL_TEXTURE_BUFFER"/> are accepted.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="params">A pointer to a float array in which to place the returned parameter value(s).</param>
    unsafe void GetTexLevelParameterfv( int target, int level, int pname, float* parameters );

    /// <summary>
    /// Return texture parameter (float) values for a specific level of detail.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_RECTANGLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>, <see cref="GL_PROXY_TEXTURE_1D"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D"/>, <see cref="GL_PROXY_TEXTURE_3D"/>, <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>, <see cref="GL_PROXY_TEXTURE_RECTANGLE"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP"/> or <see cref="GL_TEXTURE_BUFFER"/> are accepted.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="params">A <see langword="ref"/> to a float array where the values will be returned.</param>
    unsafe void GetTexLevelParameterfv( int target, int level, int pname, ref float[] parameters );

    /// <summary>
    /// Return texture parameter (integer) values for a specific level of detail.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_RECTANGLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>, <see cref="GL_PROXY_TEXTURE_1D"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D"/>, <see cref="GL_PROXY_TEXTURE_3D"/>, <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>, <see cref="GL_PROXY_TEXTURE_RECTANGLE"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP"/> or <see cref="GL_TEXTURE_BUFFER"/> are accepted.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="params">A pointer to an integer array in which to place the returned parameter value(s).</param>
    unsafe void GetTexLevelParameteriv( int target, int level, int pname, int* parameters );

    /// <summary>
    /// Return texture parameter (integer) values for a specific level of detail.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_RECTANGLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>, <see cref="GL_PROXY_TEXTURE_1D"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D"/>, <see cref="GL_PROXY_TEXTURE_3D"/>, <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>, <see cref="GL_PROXY_TEXTURE_RECTANGLE"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP"/> or <see cref="GL_TEXTURE_BUFFER"/> are accepted.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="params">A <see langword="ref"/> to an integer array where the values will be returned.</param>
    unsafe void GetTexLevelParameteriv( int target, int level, int pname, ref int[] parameters );

    /// <summary>
    /// Test whether a capability is enabled.
    /// </summary>
    /// <param name="cap">
    /// Specifies a symbolic constant indicating a  capability. Refer to
    /// <see href="https://docs.gl/gl4/glIsEnabled"/> for a list of possible capabilities.
    /// </param>
    bool IsEnabled( int cap );

    /// <summary>
    /// Specify mapping of depth values from normalized device coordinates to window coordinates.
    /// </summary>
    /// <param name="near">Specifies the mapping of the near clipping plane to window coordinates. The initial value is 0.</param>
    /// <param name="far">Specifies the mapping of the far clipping plane to window coordinates. The initial value is 1.</param>
    void DepthRange( double near, double far );

    /// <summary>
    /// Set the viewport.
    /// </summary>
    /// <param name="x">Specify the lower left corner of the viewport rectangle, in pixels. The initial value is (0,0).</param>
    /// <param name="y">Specify the lower left corner of the viewport rectangle, in pixels. The initial value is (0,0).</param>
    /// <param name="width">
    /// Specify the width and height of the viewport. When a  context is first attached to a window,
    /// width and height are set to the dimensions of that window.
    /// </param>
    /// <param name="height">
    /// Specify the width and height of the viewport. When a  context is first attached to a window,
    /// width and height are set to the dimensions of that window.
    /// </param>
    void Viewport( int x, int y, int width, int height );

    /// <summary>
    /// Render primitives from array data.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>, <see cref="GL_PATCHES"/> are
    /// accepted.
    /// </param>
    /// <param name="first">Specifies the starting index in the enabled arrays.</param>
    /// <param name="count">Specifies the number of indices to be rendered.</param>
    void DrawArrays( int mode, int first, int count );

    /// <summary>
    /// Render primitives from array data.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>, <see cref="GL_PATCHES"/> are
    /// accepted.
    /// </param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in indices. Must be one of <see cref="GL_UNSIGNED_BYTE"/>,
    /// <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
    unsafe void DrawElements( int mode, int count, int type, void* indices );

    /// <summary>
    /// Render primitives from array data.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>, <see cref="GL_PATCHES"/> are
    /// accepted.
    /// </param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in indices. Must be one of <see cref="GL_UNSIGNED_BYTE"/>,
    /// <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">
    /// Specifies an array of indices to be rendererd. Make sure to match the type
    /// <typeparamref name="T"/> with the type specified by <paramref name="type"/>.
    /// </param>
    unsafe void DrawElements< T >( int mode, int count, int type, T[] indices ) where T : unmanaged, IUnsignedNumber< T >;

    /// <summary>
    /// Set the scale and units used to calculate depth values.
    /// </summary>
    /// <param name="factor">
    /// Specifies a scale factor that is used to create a variable depth offset for each polygon. The
    /// initial value is 0.
    /// </param>
    /// <param name="units">
    /// Is multiplied by an implementation-specific value to create a constant depth offset. The initial
    /// value is 0.
    /// </param>
    void PolygonOffset( float factor, float units );

    /// <summary>
    /// Copy pixels into a 1D texture image.
    /// </summary>
    /// <param name="target">Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/>.</param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the internal format of the texture. Refer to
    /// <see href="https://docs.gl/gl4/glCopyTexImage1D"/> for a list of supported formats.
    /// </param>
    /// <param name="x">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="y">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="width">Specifies the width of the texture image.</param>
    /// <param name="border">This value must be 0.</param>
    void CopyTexImage1D( int target, int level, int internalformat, int x, int y, int width, int border );

    /// <summary>
    /// Copy pixels into a 2D texture image.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/> or <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the internal format of the texture. Refer to
    /// <see href="https://docs.gl/gl4/glCopyTexImage2D"/> for a list of supported formats.
    /// </param>
    /// <param name="x">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="y">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="width">Specifies the width of the texture image.</param>
    /// <param name="height">Specifies the height of the texture image.</param>
    /// <param name="border">This value must be 0.</param>
    void CopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border );

    /// <summary>
    /// Copy a one-dimensional texture subimage.
    /// </summary>
    /// <param name="target">Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/>.</param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="x">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="y">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    void CopyTexSubImage1D( int target, int level, int xoffset, int x, int y, int width );

    /// <summary>
    /// Copy a two-dimensional texture subimage.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> or <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="x">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="y">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    /// <param name="height">Specifies the height of the texture subimage.</param>
    void CopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height );

    /// <summary>
    /// Specify a one-dimensional texture subimage.
    /// </summary>
    /// <param name="target">Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/>.</param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_DEPTH_COMPONENT"/> and <see cref="GL_STENCIL_INDEX"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/> and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">Specifies a pointer to the image data in memory.</param>
    unsafe void TexSubImage1D( int target, int level, int xoffset, int width, int format, int type, void* pixels );

    /// <summary>
    /// Specify a one-dimensional texture subimage.
    /// </summary>
    /// <param name="target">Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/>.</param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_DEPTH_COMPONENT"/> and <see cref="GL_STENCIL_INDEX"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/> and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">
    /// Specifies an array of <typeparamref name="T"/>s containing the image data. Make sure to match
    /// this with the type specified in <paramref name="type"/>.
    /// </param>
    unsafe void TexSubImage1D< T >( int target, int level, int xoffset, int width, int format, int type, T[] pixels )
        where T : unmanaged;

    /// <summary>
    /// Specify a two-dimensional texture subimage.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be one of <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> or
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    /// <param name="height">Specifies the height of the texture subimage.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_DEPTH_COMPONENT"/> and <see cref="GL_STENCIL_INDEX"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/> and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">Specifies a pointer to the image data in memory.</param>
    unsafe void TexSubImage2D( int target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int width,
                                 int height,
                                 int format,
                                 int type,
                                 void* pixels );

    /// <summary>
    /// Specify a two-dimensional texture subimage.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be one of <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> or
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    /// <param name="height">Specifies the height of the texture subimage.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_DEPTH_COMPONENT"/> and <see cref="GL_STENCIL_INDEX"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/> and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">
    /// Specifies an array of <typeparamref name="T"/>s containing the image data. Make sure to match
    /// this with the type specified in <paramref name="type"/>.
    /// </param>
    unsafe void TexSubImage2D< T >( int target,
                                      int level,
                                      int xoffset,
                                      int yoffset,
                                      int width,
                                      int height,
                                      int format,
                                      int type,
                                      T[] pixels ) where T : unmanaged;

    /// <summary>
    /// Bind a named texture to a texturing target.
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture is bound. Must be one of <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>, <see cref="GL_TEXTURE_RECTANGLE"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/> or <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>.
    /// </param>
    /// <param name="texture">Specifies the name of a texture.</param>
    void BindTexture( int target, uint texture );

    /// <summary>
    /// Delete named textures.
    /// </summary>
    /// <param name="n">Specifies the number of textures to be deleted.</param>
    /// <param name="textures">Specifies a pointer to an array of textures to be deleted.</param>
    unsafe void DeleteTextures( int n, uint* textures );

    /// <summary>
    /// Delete named textures.
    /// </summary>
    /// <param name="textures">Specifies an array of textures to be deleted.</param>
    unsafe void DeleteTextures( params uint[] textures );

    /// <summary>
    /// Generate texture names.
    /// </summary>
    /// <param name="n">Specifies the number of texture names to be generated.</param>
    /// <param name="textures">Specifies an array in which the generated texture names are stored.</param>
    unsafe void GenTextures( int n, uint* textures );

    /// <summary>
    /// Generate texture names.
    /// </summary>
    /// <param name="n">Specifies the number of texture names to be generated.</param>
    /// <returns>An array containing the generated texture names.</returns>
    unsafe uint[] GenTextures( int n );

    /// <summary>
    /// Generate a single texture name.
    /// </summary>
    /// <returns>The generated texture name.</returns>
    uint GenTexture();

    /// <summary>
    /// Determine if a name corresponds to a texture.
    /// </summary>
    /// <param name="texture">Specifies a value that may be the name of a texture.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="texture"/> is currently the name of a texture. Otherwise,
    /// <see langword="false"/> is returned.
    /// </returns>
    bool IsTexture( uint texture );

    /// <summary>
    /// Render primitives from array data.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_TRIANGLE_STRIP"/>,
    /// <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>, <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLES_ADJACENCY"/>, and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="start">Specifies the minimum array index contained in <paramref name="indices"/>.</param>
    /// <param name="end">Specifies the maximum array index contained in <paramref name="indices"/>.</param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">Specifies the type of the values in <paramref name="indices"/>.</param>
    /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
    unsafe void DrawRangeElements( int mode, uint start, uint end, int count, int type, void* indices );

    /// <summary>
    /// Render primitives from array data.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_TRIANGLE_STRIP"/>,
    /// <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>, <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLES_ADJACENCY"/>, and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="start">Specifies the minimum array index contained in <paramref name="indices"/>.</param>
    /// <param name="end">Specifies the maximum array index contained in <paramref name="indices"/>.</param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">Specifies the type of the values in <paramref name="indices"/>.</param>
    /// <param name="indices">
    /// An array of indices to render. Make sure to match the type <typeparamref name="T"/> with the
    /// type specified for <paramref name="type"/>.
    /// </param>
    unsafe void DrawRangeElements< T >( int mode, uint start, uint end, int count, int type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >;

    /// <summary>
    /// Specify a three-dimensional texture image.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/>,
    /// <see cref="GL_PROXY_TEXTURE_3D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/> or <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the number of color components in the texture. Refer to
    /// <see href="https://docs.gl/gl4/glTexImage3D"/> for the list of possible values.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support 3D texture images that are at
    /// least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture image. All implementations support 3D texture images that are
    /// at least 256 texels high.
    /// </param>
    /// <param name="depth">
    /// Specifies the depth of the texture image, or the number of layers in a texture array. All
    /// implementations support 3D texture images that are at least 256 texels deep, and texture arrays that are at least
    /// 256 layers deep.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_BGRA"/>, <see cref="GL_RED_INTEGER"/>, <see cref="GL_RG_INTEGER"/>, <see cref="GL_RGB_INTEGER"/>,
    /// <see cref="GL_BGR_INTEGER"/>, <see cref="GL_RGBA_INTEGER"/>, <see cref="GL_BGRA_INTEGER"/>,
    /// <see cref="GL_STENCIL_INDEX"/>, <see cref="GL_DEPTH_COMPONENT"/> or <see cref="GL_DEPTH_STENCIL"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">Specifies a pointer to the image data in memory.</param>
    unsafe void TexImage3D( int target,
                              int level,
                              int internalformat,
                              int width,
                              int height,
                              int depth,
                              int border,
                              int format,
                              int type,
                              void* pixels );

    /// <summary>
    /// Specify a three-dimensional texture image.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/>,
    /// <see cref="GL_PROXY_TEXTURE_3D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/> or <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the number of color components in the texture. Refer to
    /// <see href="https://docs.gl/gl4/glTexImage3D"/> for the list of possible values.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support 3D texture images that are at
    /// least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture image. All implementations support 3D texture images that are
    /// at least 256 texels high.
    /// </param>
    /// <param name="depth">
    /// Specifies the depth of the texture image, or the number of layers in a texture array. All
    /// implementations support 3D texture images that are at least 256 texels deep, and texture arrays that are at least
    /// 256 layers deep.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_BGRA"/>, <see cref="GL_RED_INTEGER"/>, <see cref="GL_RG_INTEGER"/>, <see cref="GL_RGB_INTEGER"/>,
    /// <see cref="GL_BGR_INTEGER"/>, <see cref="GL_RGBA_INTEGER"/>, <see cref="GL_BGRA_INTEGER"/>,
    /// <see cref="GL_STENCIL_INDEX"/>, <see cref="GL_DEPTH_COMPONENT"/> or <see cref="GL_DEPTH_STENCIL"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">
    /// Specifies an array of <typeparamref name="T"/> containing the image data. Make sure to match the
    /// <paramref name="type"/> parameter with <typeparamref name="T"/>.
    /// </param>
    unsafe void TexImage3D< T >( int target,
                                   int level,
                                   int internalformat,
                                   int width,
                                   int height,
                                   int depth,
                                   int border,
                                   int format,
                                   int type,
                                   T[] pixels ) where T : unmanaged;

    /// <summary>
    /// Specify a three-dimensional texture subimage.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/> or
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="zoffset">Specifies a texel offset in the z direction within the texture array.</param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    /// <param name="height">Specifies the height of the texture subimage.</param>
    /// <param name="depth">Specifies the depth of the texture subimage.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_DEPTH_COMPONENT"/> and <see cref="GL_STENCIL_INDEX"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">Specifies a pointer to the image data in memory.</param>
    unsafe void TexSubImage3D( int target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int zoffset,
                                 int width,
                                 int height,
                                 int depth,
                                 int format,
                                 int type,
                                 void* pixels );

    /// <summary>
    /// Specify a three-dimensional texture subimage.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/> or
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="zoffset">Specifies a texel offset in the z direction within the texture array.</param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    /// <param name="height">Specifies the height of the texture subimage.</param>
    /// <param name="depth">Specifies the depth of the texture subimage.</param>
    /// <param name="format">
    /// Specifies the format of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_RED"/>, <see cref="GL_RG"/>, <see cref="GL_RGB"/>, <see cref="GL_BGR"/>, <see cref="GL_RGBA"/>,
    /// <see cref="GL_DEPTH_COMPONENT"/> and <see cref="GL_STENCIL_INDEX"/>.
    /// </param>
    /// <param name="type">
    /// Specifies the data type of the pixel data. The following symbolic values are accepted:
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_SHORT"/>,
    /// <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_INT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_UNSIGNED_BYTE_3_3_2"/>, <see cref="GL_UNSIGNED_BYTE_2_3_3_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_6_5"/>, <see cref="GL_UNSIGNED_SHORT_5_6_5_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_4_4_4_4"/>, <see cref="GL_UNSIGNED_SHORT_4_4_4_4_REV"/>,
    /// <see cref="GL_UNSIGNED_SHORT_5_5_5_1"/>, <see cref="GL_UNSIGNED_SHORT_1_5_5_5_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_8_8_8_8"/>, <see cref="GL_UNSIGNED_INT_8_8_8_8_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_10_10_10_2"/>, and <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>.
    /// </param>
    /// <param name="pixels">
    /// Specifies an array of <typeparamref name="T"/> containing the image data. Make sure to match the
    /// <paramref name="type"/> parameter with <typeparamref name="T"/>.
    /// </param>
    unsafe void TexSubImage3D< T >( int target,
                                      int level,
                                      int xoffset,
                                      int yoffset,
                                      int zoffset,
                                      int width,
                                      int height,
                                      int depth,
                                      int format,
                                      int type,
                                      T[] pixels ) where T : unmanaged;

    /// <summary>
    /// Copy a three-dimensional texture subimage.
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/> or
    /// <see cref="GL_TEXTURE_2D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="zoffset">Specifies a texel offset in the z direction within the texture array.</param>
    /// <param name="x">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="y">
    /// Specify the window coordinates of the lower left corner of the rectangular region of pixels to be
    /// copied.
    /// </param>
    /// <param name="width">Specifies the width of the texture subimage.</param>
    /// <param name="height">Specifies the height of the texture subimage.</param>
    void CopyTexSubImage3D( int target,
                              int level,
                              int xoffset,
                              int yoffset,
                              int zoffset,
                              int x,
                              int y,
                              int width,
                              int height );

    /// <summary>
    /// Select active texture unit
    /// </summary>
    /// <param name="texture">
    /// Specifies which texture unit to make active. The number of texture units is implementation
    /// dependent, but must be at least 80. <paramref name="texture"/> must be one of <see cref="GL_TEXTURE0"/>+i, where
    /// i ranges from 0 to the value of <see cref="GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS"/> minus one. The initial value is
    /// <see cref="GL_TEXTURE0"/>.
    /// </param>
    void ActiveTexture( int texture );

    /// <summary>
    /// Specify multisample coverage parameters
    /// </summary>
    /// <param name="value">
    /// Specify a single floating-point sample coverage value. The value is clamped to the range [0,1]. The
    /// initial value is 1.
    /// </param>
    /// <param name="invert">
    /// Specify a single boolean value representing if the coverage masks should be inverted. The intial
    /// value is <see langword="false"/>.
    /// </param>
    void SampleCoverage( float value, bool invert );

    /// <summary>
    /// Specify a three-dimensional texture image in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/>,
    /// <see cref="GL_PROXY_TEXTURE_3D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/> or <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the color components in the texture. Must be one of
    /// <see cref="GL_COMPRESSED_RED"/>, <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>,
    /// <see cref="GL_COMPRESSED_RGBA"/>, <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support 3D texture images that are at
    /// least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture image. All implementations support 3D texture images that are
    /// at least 16 texels high.
    /// </param>
    /// <param name="depth">
    /// Specifies the depth of the texture image. All implementations support 3D texture images that are at
    /// least 16 texels deep.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="imageSize">
    /// Specifies the number of unsigned bytes of image data starting at the address specified by
    /// <paramref name="data"/>.
    /// </param>
    /// <param name="data">Specifies a pointer to the compressed image data in memory.</param>
    unsafe void CompressedTexImage3D( int target,
                                        int level,
                                        int internalformat,
                                        int width,
                                        int height,
                                        int depth,
                                        int border,
                                        int imageSize,
                                        void* data );

    /// <summary>
    /// Specify a three-dimensional texture image in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/>,
    /// <see cref="GL_PROXY_TEXTURE_3D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/> or <see cref="GL_PROXY_TEXTURE_2D_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the color components in the texture. Must be one of
    /// <see cref="GL_COMPRESSED_RED"/>, <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>,
    /// <see cref="GL_COMPRESSED_RGBA"/>, <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support 3D texture images that are at
    /// least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture image. All implementations support 3D texture images that are
    /// at least 16 texels high.
    /// </param>
    /// <param name="depth">
    /// Specifies the depth of the texture image. All implementations support 3D texture images that are at
    /// least 16 texels deep.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="data">Specifies an array of bytes containing the compressed image data.</param>
    unsafe void CompressedTexImage3D( int target,
                                        int level,
                                        int internalformat,
                                        int width,
                                        int height,
                                        int depth,
                                        int border,
                                        byte[] data );

    /// <summary>
    /// Specify a two-dimensional texture image in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> or
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the color components in the texture. Must be one of
    /// <see cref="GL_COMPRESSED_RED"/>, <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>,
    /// <see cref="GL_COMPRESSED_RGBA"/>, <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support 2D texture images that are at
    /// least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture image. All implementations support 2D texture images that are
    /// at least 16 texels high.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="imageSize">
    /// Specifies the number of unsigned bytes of image data starting at the address specified by
    /// <paramref name="data"/>.
    /// </param>
    /// <param name="data">Specifies a pointer to the compressed image data in memory.</param>
    unsafe void CompressedTexImage2D( int target,
                                        int level,
                                        int internalformat,
                                        int width,
                                        int height,
                                        int border,
                                        int imageSize,
                                        void* data );

    /// <summary>
    /// Specify a two-dimensional texture image in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_PROXY_TEXTURE_2D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_PROXY_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> or
    /// <see cref="GL_PROXY_TEXTURE_CUBE_MAP"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the color components in the texture. Must be one of
    /// <see cref="GL_COMPRESSED_RED"/>, <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>,
    /// <see cref="GL_COMPRESSED_RGBA"/>, <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support 2D texture images that are at
    /// least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture image. All implementations support 2D texture images that are
    /// at least 16 texels high.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="data">Specifies an array of bytes containing the compressed image data.</param>
    unsafe void CompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, byte[] data );

    /// <summary>
    /// Specify a one-dimensional texture image in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/> or
    /// <see cref="GL_PROXY_TEXTURE_1D"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the color components in the texture. Must be one of
    /// <see cref="GL_COMPRESSED_RED"/>, <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>,
    /// <see cref="GL_COMPRESSED_RGBA"/>, <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support 2D texture images that are at
    /// least 64 texels wide.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="imageSize">
    /// Specifies the number of unsigned bytes of image data starting at the address specified by
    /// <paramref name="data"/>.
    /// </param>
    /// <param name="data">Specifies a pointer to the compressed image data in memory.</param>
    unsafe void CompressedTexImage1D( int target, int level, int internalformat, int width, int border, int imageSize, void* data );

    /// <summary>
    /// Specify a one-dimensional texture image in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/> or
    /// <see cref="GL_PROXY_TEXTURE_1D"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="internalformat">
    /// Specifies the color components in the texture. Must be one of
    /// <see cref="GL_COMPRESSED_RED"/>, <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>,
    /// <see cref="GL_COMPRESSED_RGBA"/>, <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="width">
    /// Specifies the width of the texture image. All implementations support 2D texture images that are at
    /// least 64 texels wide.
    /// </param>
    /// <param name="border">This value must be 0.</param>
    /// <param name="data">Specifies an array of bytes containing the compressed image data.</param>
    unsafe void CompressedTexImage1D( int target, int level, int internalformat, int width, int border, byte[] data );

    /// <summary>
    /// Specify a three-dimensional texture subimage in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/> or <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="zoffset">Specifies a texel offset in the z direction within the texture array.</param>
    /// <param name="width">
    /// Specifies the width of the texture subimage. All implementations support 3D texture subimages that
    /// are at least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture subimage. All implementations support 3D texture subimages
    /// that are at least 16 texels high.
    /// </param>
    /// <param name="depth">
    /// Specifies the depth of the texture subimage. All implementations support 3D texture subimages that
    /// are at least 16 texels deep.
    /// </param>
    /// <param name="format">
    /// Specifies the color components in the texture. Must be one of <see cref="GL_COMPRESSED_RED"/>,
    /// <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>, <see cref="GL_COMPRESSED_RGBA"/>,
    /// <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="imageSize">
    /// Specifies the number of unsigned bytes of image data starting at the address specified by
    /// <paramref name="data"/>.
    /// </param>
    /// <param name="data">Specifies a pointer to the compressed image data in memory.</param>
    unsafe void CompressedTexSubImage3D( int target,
                                           int level,
                                           int xoffset,
                                           int yoffset,
                                           int zoffset,
                                           int width,
                                           int height,
                                           int depth,
                                           int format,
                                           int imageSize,
                                           void* data );

    /// <summary>
    /// Specify a three-dimensional texture subimage in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_3D"/>,
    /// <see cref="GL_TEXTURE_2D_ARRAY"/> or <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="zoffset">Specifies a texel offset in the z direction within the texture array.</param>
    /// <param name="width">
    /// Specifies the width of the texture subimage. All implementations support 3D texture subimages that
    /// are at least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture subimage. All implementations support 3D texture subimages
    /// that are at least 16 texels high.
    /// </param>
    /// <param name="depth">
    /// Specifies the depth of the texture subimage. All implementations support 3D texture subimages that
    /// are at least 16 texels deep.
    /// </param>
    /// <param name="format">
    /// Specifies the color components in the texture. Must be one of <see cref="GL_COMPRESSED_RED"/>,
    /// <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>, <see cref="GL_COMPRESSED_RGBA"/>,
    /// <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="data">Specifies an array of bytes containing the compressed image data.</param>
    unsafe void CompressedTexSubImage3D( int target,
                                           int level,
                                           int xoffset,
                                           int yoffset,
                                           int zoffset,
                                           int width,
                                           int height,
                                           int depth,
                                           int format,
                                           byte[] data );

    /// <summary>
    /// Specify a two-dimensional texture subimage in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/> or
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="width">
    /// Specifies the width of the texture subimage. All implementations support 2D texture subimages that
    /// are at least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture subimage. All implementations support 2D texture subimages
    /// that are at least 16 texels high.
    /// </param>
    /// <param name="format">
    /// Specifies the color components in the texture. Must be one of <see cref="GL_COMPRESSED_RED"/>,
    /// <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>, <see cref="GL_COMPRESSED_RGBA"/>,
    /// <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="imageSize">
    /// Specifies the number of unsigned bytes of image data starting at the address specified by
    /// <paramref name="data"/>.
    /// </param>
    /// <param name="data">Specifies a pointer to the compressed image data in memory.</param>
    unsafe void CompressedTexSubImage2D( int target,
                                           int level,
                                           int xoffset,
                                           int yoffset,
                                           int width,
                                           int height,
                                           int format,
                                           int imageSize,
                                           void* data );

    /// <summary>
    /// Specify a two-dimensional texture subimage in a compressed format
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/> or
    /// <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="yoffset">Specifies a texel offset in the y direction within the texture array.</param>
    /// <param name="width">
    /// Specifies the width of the texture subimage. All implementations support 2D texture subimages that
    /// are at least 16 texels wide.
    /// </param>
    /// <param name="height">
    /// Specifies the height of the texture subimage. All implementations support 2D texture subimages
    /// that are at least 16 texels high.
    /// </param>
    /// <param name="format">
    /// Specifies the color components in the texture. Must be one of <see cref="GL_COMPRESSED_RED"/>,
    /// <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>, <see cref="GL_COMPRESSED_RGBA"/>,
    /// <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="data">Specifies an array of bytes containing the compressed image data.</param>
    unsafe void CompressedTexSubImage2D( int target,
                                           int level,
                                           int xoffset,
                                           int yoffset,
                                           int width,
                                           int height,
                                           int format,
                                           byte[] data );

    /// <summary>
    /// Specify a one-dimensional texture subimage in a compressed format
    /// </summary>
    /// <param name="target">Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/>.</param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="width">
    /// Specifies the width of the texture subimage. All implementations support 1D texture subimages that
    /// are at least 16 texels wide.
    /// </param>
    /// <param name="format">
    /// Specifies the color components in the texture. Must be one of <see cref="GL_COMPRESSED_RED"/>,
    /// <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>, <see cref="GL_COMPRESSED_RGBA"/>,
    /// <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="imageSize">
    /// Specifies the number of unsigned bytes of image data starting at the address specified by
    /// <paramref name="data"/>.
    /// </param>
    /// <param name="data">Specifies a pointer to the compressed image data in memory.</param>
    unsafe void CompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, int imageSize, void* data );

    /// <summary>
    /// Specify a one-dimensional texture subimage in a compressed format
    /// </summary>
    /// <param name="target">Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/>.</param>
    /// <param name="level">
    /// Specifies the level-of-detail number. Level 0 is the base image level. Level n is the nth mipmap
    /// reduction image.
    /// </param>
    /// <param name="xoffset">Specifies a texel offset in the x direction within the texture array.</param>
    /// <param name="width">
    /// Specifies the width of the texture subimage. All implementations support 1D texture subimages that
    /// are at least 16 texels wide.
    /// </param>
    /// <param name="format">
    /// Specifies the color components in the texture. Must be one of <see cref="GL_COMPRESSED_RED"/>,
    /// <see cref="GL_COMPRESSED_RG"/>, <see cref="GL_COMPRESSED_RGB"/>, <see cref="GL_COMPRESSED_RGBA"/>,
    /// <see cref="GL_COMPRESSED_SRGB"/>, <see cref="GL_COMPRESSED_SRGB_ALPHA"/>.
    /// </param>
    /// <param name="data">Specifies an array of bytes containing the compressed image data.</param>
    unsafe void CompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, byte[] data );

    /// <summary>
    /// Return a compressed texture image
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number of the desired image. Level 0 is the base image level. Level n
    /// is the nth mipmap reduction image.
    /// </param>
    /// <param name="img">Specifies a pointer to a buffer into which the compressed image data will be placed.</param>
    unsafe void GetCompressedTexImage( int target, int level, void* img );

    /// <summary>
    /// Return a compressed texture image
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture. Must be <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_X"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_X"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Y"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Y"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP_POSITIVE_Z"/>, <see cref="GL_TEXTURE_CUBE_MAP_NEGATIVE_Z"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="level">
    /// Specifies the level-of-detail number of the desired image. Level 0 is the base image level. Level n
    /// is the nth mipmap reduction image.
    /// </param>
    /// <param name="img">Specifies a <see langword="ref"/> byte array into which the compressed image data will be placed.</param>
    unsafe void GetCompressedTexImage( int target, int level, ref byte[] img );

    /// <summary>
    /// Specify pixel arithmetic for RGB and alpha components separately. Refer to
    /// <see href="https://docs.gl/gl4/glBlendFuncSeparate"/> for all possible values for the functions.
    /// </summary>
    /// <param name="sfactorRGB">
    /// Specifies how the red, green, and blue blending factors are computed. The initial value is
    /// <see cref="GL_ONE"/>.
    /// </param>
    /// <param name="dfactorRGB">
    /// Specifies how the red, green, and blue destination blending factors are computed. The initial
    /// value is <see cref="GL_ZERO"/>.
    /// </param>
    /// <param name="sfactorAlpha">
    /// Specifies how the alpha source blending factor is computed. The initial value is
    /// <see cref="GL_ONE"/>.
    /// </param>
    /// <param name="dfactorAlpha">
    /// Specifies how the alpha destination blending factor is computed. The initial value is
    /// <see cref="GL_ZERO"/>.
    /// </param>
    void BlendFuncSeparate( int sfactorRGB, int dfactorRGB, int sfactorAlpha, int dfactorAlpha );

    /// <summary>
    /// Render multiple sets of primitives from array data.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_TRIANGLE_STRIP"/>,
    /// <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>, <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLES_ADJACENCY"/>, and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="first">Specifies an array of starting indices in the enabled arrays.</param>
    /// <param name="count">Specifies an array of the number of indices to be rendered.</param>
    /// <param name="drawcount">Specifies the size of the <paramref name="first"/> and <paramref name="count"/> arrays.</param>
    unsafe void MultiDrawArrays( int mode, int* first, int* count, int drawcount );

    /// <summary>
    /// Render multiple sets of primitives from array data.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_TRIANGLE_STRIP"/>,
    /// <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>, <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLES_ADJACENCY"/>, and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="first">Specifies an array of starting indices in the enabled arrays.</param>
    /// <param name="count">Specifies an array of the number of indices to be rendered.</param>
    unsafe void MultiDrawArrays( int mode, int[] first, int[] count );

    /// <summary>
    /// Render multiple sets of primitives by specifying indices of array data elements.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_TRIANGLE_STRIP"/>,
    /// <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>, <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLES_ADJACENCY"/>, and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="count">Specifies an array of the elements counts.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
    /// <param name="drawcount">Specifies the size of the <paramref name="count"/> and <paramref name="indices"/> arrays.</param>
    unsafe void MultiDrawElements( int mode, int* count, int type, void** indices, int drawcount );

    /// <summary>
    /// Render multiple sets of primitives by specifying indices of array data elements.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_TRIANGLE_STRIP"/>,
    /// <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>, <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLES_ADJACENCY"/>, and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="count">Specifies an array of the elements counts.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">Specifies a two-dimensional array of indices of the vertices that are to be rendered.</param>
    unsafe void MultiDrawElements< T >( int mode, int[] count, int type, T[][] indices ) where T : unmanaged, IUnsignedNumber< T >;

    /// <summary>
    /// Specify point parameters.
    /// </summary>
    /// <param name="pname">
    /// Specifies the symbolic name of the parameter to be set. <see cref="GL_POINT_FADE_THRESHOLD_SIZE"/>
    /// and <see cref="GL_POINT_SPRITE_COORD_ORIGIN"/> are accepted.
    /// </param>
    /// <param name="param">Specifies the value that parameter <paramref name="pname"/> will be set to.</param>
    void PointParameterf( int pname, float param );

    /// <summary>
    /// Specify point parameters.
    /// </summary>
    /// <param name="pname">
    /// Specifies the symbolic name of the parameter to be set. <see cref="GL_POINT_FADE_THRESHOLD_SIZE"/>
    /// and <see cref="GL_POINT_SPRITE_COORD_ORIGIN"/> are accepted.
    /// </param>
    /// <param name="params">
    /// Specifies a pointer to an array where the value or values to be assigned to
    /// <paramref name="pname"/> are currently stored.
    /// </param>
    unsafe void PointParameterfv( int pname, float* parameters );

    /// <summary>
    /// Specify point parameters.
    /// </summary>
    /// <param name="pname">
    /// Specifies the symbolic name of the parameter to be set. <see cref="GL_POINT_FADE_THRESHOLD_SIZE"/>
    /// and <see cref="GL_POINT_SPRITE_COORD_ORIGIN"/> are accepted.
    /// </param>
    /// <param name="params">Specifies an array of values that will be used to update the current point parameters.</param>
    unsafe void PointParameterfv( int pname, float[] parameters );

    /// <summary>
    /// Specify point parameters.
    /// </summary>
    /// <param name="pname">
    /// Specifies the symbolic name of the parameter to be set. <see cref="GL_POINT_FADE_THRESHOLD_SIZE"/>
    /// and <see cref="GL_POINT_SPRITE_COORD_ORIGIN"/> are accepted.
    /// </param>
    /// <param name="param">Specifies the value that parameter <paramref name="pname"/> will be set to.</param>
    void PointParameteri( int pname, int param );

    /// <summary>
    /// Specify point parameters.
    /// </summary>
    /// <param name="pname">
    /// Specifies the symbolic name of the parameter to be set. <see cref="GL_POINT_FADE_THRESHOLD_SIZE"/>
    /// and <see cref="GL_POINT_SPRITE_COORD_ORIGIN"/> are accepted.
    /// </param>
    /// <param name="params">
    /// Specifies a pointer to an array where the value or values to be assigned to
    /// <paramref name="pname"/> are currently stored.
    /// </param>
    unsafe void PointParameteriv( int pname, int* parameters );

    /// <summary>
    /// Specify point parameters.
    /// </summary>
    /// <param name="pname">
    /// Specifies the symbolic name of the parameter to be set. <see cref="GL_POINT_FADE_THRESHOLD_SIZE"/>
    /// and <see cref="GL_POINT_SPRITE_COORD_ORIGIN"/> are accepted.
    /// </param>
    /// <param name="params">Specifies an array of values that will be used to update the current point parameters.</param>
    unsafe void PointParameteriv( int pname, int[] parameters );

    /// <summary>
    /// Set the blend color.
    /// </summary>
    /// <param name="red">Specify the red value to use as the blend color.</param>
    /// <param name="green">Specify the green value to use as the blend color.</param>
    /// <param name="blue">Specify the blue value to use as the blend color.</param>
    /// <param name="alpha">Specify the alpha value to use as the blend color.</param>
    void BlendColor( float red, float green, float blue, float alpha );

    /// <summary>
    /// Specify the equation used for both the RGB blend equation and the Alpha blend equation.
    /// </summary>
    /// <param name="mode">
    /// Specifies how source and destination colors are combined. Must be <see cref="GL_FUNC_ADD"/>,
    /// <see cref="GL_FUNC_SUBTRACT"/>, <see cref="GL_FUNC_REVERSE_SUBTRACT"/>, <see cref="GL_MIN"/>,
    /// <see cref="GL_MAX"/>.
    /// </param>
    void BlendEquation( int mode );

    /// <summary>
    /// Generate query object names.
    /// </summary>
    /// <param name="n">Specifies the number of query object names to generate.</param>
    /// <param name="ids">Specifies an array in which the generated query object names are to be stored.</param>
    unsafe void GenQueries( int n, uint* ids );

    /// <summary>
    /// Generate query object names.
    /// </summary>
    /// <param name="n">Specifies the number of query object names to generate.</param>
    /// <returns>Array of generated query object names.</returns>
    unsafe uint[] GenQueries( int n );

    /// <summary>
    /// Generate a single query object name.
    /// </summary>
    /// <returns>Generated query object name.</returns>
    uint GenQuery();

    /// <summary>
    /// Delete named query objects.
    /// </summary>
    /// <param name="n">Specifies the number of query objects to be deleted.</param>
    /// <param name="ids">Specifies an array of query objects to be deleted.</param>
    unsafe void DeleteQueries( int n, uint* ids );

    /// <summary>
    /// Delete named query objects.
    /// </summary>
    /// <param name="ids">Specifies an array of query objects to be deleted.</param>
    unsafe void DeleteQueries( params uint[] ids );

    /// <summary>
    /// Determine if a name corresponds to a query object.
    /// </summary>
    /// <param name="id">Specifies a value that may be the name of a query object.</param>
    /// <returns><see langword="true"/> if <paramref name="id"/> is query object name, otherwise <see langword="false"/>.</returns>
    bool IsQuery( uint id );

    /// <summary>
    /// Delimit the boundaries of a query object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target type of query object established between <see cref="GLBindings.glBeginQuery"/> and the
    /// subsequent <see cref="GLBindings.glEndQuery"/>. The symbol constant must be one of <see cref="GL_SAMPLES_PASSED"/>,
    /// <see cref="GL_ANY_SAMPLES_PASSED"/>, <see cref="GL_ANY_SAMPLES_PASSED_CONSERVATIVE"/>,
    /// <see cref="GL_PRIMITIVES_GENERATED"/>, <see cref="GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN"/> or
    /// <see cref="GL_TIME_ELAPSED"/>.
    /// </param>
    /// <param name="id">Specifies the name of a query object.</param>
    void BeginQuery( int target, uint id );

    /// <summary>
    /// Delimit the boundaries of a query object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target type of query object established between <see cref="GLBindings.glBeginQuery"/> and the
    /// subsequent <see cref="GLBindings.glEndQuery"/>. The symbol constant must be one of <see cref="GL_SAMPLES_PASSED"/>,
    /// <see cref="GL_ANY_SAMPLES_PASSED"/>, <see cref="GL_ANY_SAMPLES_PASSED_CONSERVATIVE"/>,
    /// <see cref="GL_PRIMITIVES_GENERATED"/>, <see cref="GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN"/> or
    /// <see cref="GL_TIME_ELAPSED"/>.
    /// </param>
    void EndQuery( int target );

    /// <summary>
    /// Return parameters of a query object target.
    /// </summary>
    /// <param name="target">
    /// Specifies the target parameter of the query object being queried. The symbolic constant must be
    /// one of <see cref="GL_SAMPLES_PASSED"/>, <see cref="GL_ANY_SAMPLES_PASSED"/>,
    /// <see cref="GL_ANY_SAMPLES_PASSED_CONSERVATIVE"/>, <see cref="GL_PRIMITIVES_GENERATED"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN"/>, <see cref="GL_TIME_ELAPSED"/> or
    /// <see cref="GL_TIMESTAMP"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a query object target parameter. <see cref="GL_CURRENT_QUERY"/> and
    /// <see cref="GL_QUERY_COUNTER_BITS"/> are accepted.
    /// </param>
    /// <param name="params">A pointer to the location where the integer value or values are to be returned.</param>
    unsafe void GetQueryiv( int target, int pname, int* parameters );

    /// <summary>
    /// Return parameters of a query object target.
    /// </summary>
    /// <param name="target">
    /// Specifies the target parameter of the query object being queried. The symbolic constant must be
    /// one of <see cref="GL_SAMPLES_PASSED"/>, <see cref="GL_ANY_SAMPLES_PASSED"/>,
    /// <see cref="GL_ANY_SAMPLES_PASSED_CONSERVATIVE"/>, <see cref="GL_PRIMITIVES_GENERATED"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN"/>, <see cref="GL_TIME_ELAPSED"/> or
    /// <see cref="GL_TIMESTAMP"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a query object target parameter. <see cref="GL_CURRENT_QUERY"/> and
    /// <see cref="GL_QUERY_COUNTER_BITS"/> are accepted.
    /// </param>
    /// <param name="params">A <see langword="ref"/> to an integer array where the integer value or values are to be returned.</param>
    unsafe void GetQueryiv( int target, int pname, ref int[] parameters );

    /// <summary>
    /// Return parameters of a query object.
    /// </summary>
    /// <param name="id">Specifies the name of a query object.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a query object parameter. <see cref="GL_QUERY_RESULT"/>,
    /// <see cref="GL_QUERY_RESULT_NO_WAIT"/> or <see cref="GL_QUERY_RESULT_AVAILABLE"/> are accepted.
    /// </param>
    /// <param name="params">A pointer to the location where the integer value or values are to be returned.</param>
    unsafe void GetQueryObjectiv( uint id, int pname, int* parameters );

    /// <summary>
    /// Return parameters of a query object.
    /// </summary>
    /// <param name="id">Specifies the name of a query object.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a query object parameter. <see cref="GL_QUERY_RESULT"/>,
    /// <see cref="GL_QUERY_RESULT_NO_WAIT"/> or <see cref="GL_QUERY_RESULT_AVAILABLE"/> are accepted.
    /// </param>
    /// <param name="params">A <see langword="ref"/> to an integer array where the integer value or values are to be returned.</param>
    unsafe void GetQueryObjectiv( uint id, int pname, ref int[] parameters );

    /// <summary>
    /// Return parameters of a query object.
    /// </summary>
    /// <param name="id">Specifies the name of a query object.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a query object parameter. <see cref="GL_QUERY_RESULT"/>,
    /// <see cref="GL_QUERY_RESULT_NO_WAIT"/> or <see cref="GL_QUERY_RESULT_AVAILABLE"/> are accepted.
    /// </param>
    /// <param name="params">A pointer to the location where the unsigned integer value or values are to be returned.</param>
    unsafe void GetQueryObjectuiv( uint id, int pname, uint* parameters );

    /// <summary>
    /// Return parameters of a query object.
    /// </summary>
    /// <param name="id">Specifies the name of a query object.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a query object parameter. <see cref="GL_QUERY_RESULT"/>,
    /// <see cref="GL_QUERY_RESULT_NO_WAIT"/> or <see cref="GL_QUERY_RESULT_AVAILABLE"/> are accepted.
    /// </param>
    /// <param name="params">
    /// A <see langword="ref"/> to an unsigned integer array where the integer value or values are to be
    /// returned.
    /// </param>
    unsafe void GetQueryObjectuiv( uint id, int pname, ref uint[] parameters );

    /// <summary>
    /// Bind a named buffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the buffer object is bound. The symbolic constant must be
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DRAW_INDIRECT_BUFFER"/>, <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>
    /// , <see cref="GL_PIXEL_UNPACK_BUFFER"/>, <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="buffer">Specifies the name of a buffer object.</param>
    void BindBuffer( int target, uint buffer );

    /// <summary>
    /// Delete named buffer objects.
    /// </summary>
    /// <param name="n">Specifies the number of buffer objects to be deleted.</param>
    /// <param name="buffers">A pointer to an array of buffer objects to be deleted.</param>
    unsafe void DeleteBuffers( int n, uint* buffers );

    /// <summary>
    /// Delete named buffer objects.
    /// </summary>
    /// <param name="buffers">An array of buffer objects to be deleted.</param>
    unsafe void DeleteBuffers( params uint[] buffers );

    /// <summary>
    /// Generate buffer object names.
    /// </summary>
    /// <param name="n">Specifies the number of buffer object names to generate.</param>
    /// <param name="buffers">A pointer to an array in which the generated buffer object names are to be stored.</param>
    unsafe void GenBuffers( int n, uint* buffers );

    /// <summary>
    /// Generate buffer object names.
    /// </summary>
    /// <param name="n">Specifies the number of buffer object names to generate.</param>
    /// <returns>An array of generated buffer object names.</returns>
    unsafe uint[] GenBuffers( int n );

    /// <summary>
    /// Generate a single buffer object name.
    /// </summary>
    /// <returns>The generated buffer object name.</returns>
    uint GenBuffer();

    /// <summary>
    /// Determine if a name corresponds to a buffer object.
    /// </summary>
    /// <param name="buffer">A value that may be the name of a buffer object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="buffer"/> is a buffer object name. <see langword="false"/>
    /// otherwise.
    /// </returns>
    bool IsBuffer( uint buffer );

    /// <summary>
    /// Create and initialize a buffer object's data store.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="size">Specifies the size in bytes of the buffer object's new data store.</param>
    /// <param name="data">
    /// Specifies a pointer to data that will be copied into the data store for initialization, or
    /// <see cref="GLBindings.NULL"/> if no data is to be copied.
    /// </param>
    /// <param name="usage">
    /// Specifies the expected usage pattern of the data store. The symbolic constant must be
    /// <see cref="GL_STREAM_DRAW"/>, <see cref="GL_STREAM_READ"/>, <see cref="GL_STREAM_COPY"/>,
    /// <see cref="GL_STATIC_DRAW"/>, <see cref="GL_STATIC_READ"/>, <see cref="GL_STATIC_COPY"/>,
    /// <see cref="GL_DYNAMIC_DRAW"/>, <see cref="GL_DYNAMIC_READ"/> or <see cref="GL_DYNAMIC_COPY"/>.
    /// </param>
    unsafe void BufferData( int target, int size, void* data, int usage );

    /// <summary>
    /// Create and initialize a buffer object's data store.
    /// </summary>
    /// <typeparam name="T">The type of the data to be copied.</typeparam>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="data">An array of <typeparamref name="T"/>s that will be copied into the data store for initialization.</param>
    /// <param name="usage">
    /// Specifies the expected usage pattern of the data store. The symbolic constant must be
    /// <see cref="GL_STREAM_DRAW"/>, <see cref="GL_STREAM_READ"/>, <see cref="GL_STREAM_COPY"/>,
    /// <see cref="GL_STATIC_DRAW"/>, <see cref="GL_STATIC_READ"/>, <see cref="GL_STATIC_COPY"/>,
    /// <see cref="GL_DYNAMIC_DRAW"/>, <see cref="GL_DYNAMIC_READ"/> or <see cref="GL_DYNAMIC_COPY"/>.
    /// </param>
    unsafe void BufferData< T >( int target, T[] data, int usage ) where T : unmanaged;

    /// <summary>
    /// Update a subset of a buffer object's data store.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="offset">
    /// Specifies the offset into the buffer object's data store where data replacement will begin,
    /// measured in bytes.
    /// </param>
    /// <param name="size">Specifies the size in bytes of the data store region being replaced.</param>
    /// <param name="data">Specifies a pointer to the new data that will be copied into the data store.</param>
    unsafe void BufferSubData( int target, int offset, int size, void* data );

    /// <summary>
    /// Update a subset of a buffer object's data store.
    /// </summary>
    /// <typeparam name="T">The type of the data to be copied.</typeparam>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="offsetCount">Specifies the offset into the buffer object's data store where data replacement will begin.</param>
    /// <param name="data">An array of <typeparamref name="T"/>s that will be copied into the data store for replacement.</param>
    unsafe void BufferSubData< T >( int target, int offsetCount, T[] data ) where T : unmanaged;

    /// <summary>
    /// Return a subset of a buffer object's data store.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="offset">
    /// Specifies the offset into the buffer object's data store from which data will be returned,
    /// measured in bytes.
    /// </param>
    /// <param name="size">Specifies the size in bytes of the data store region being returned.</param>
    /// <param name="data">Specifies a pointer to the location where buffer object data is returned.</param>
    unsafe void GetBufferSubData( int target, int offset, int size, void* data );

    /// <summary>
    /// Return a subset of a buffer object's data store.
    /// </summary>
    /// <typeparam name="T">The type of the data to be returned.</typeparam>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="offsetCount">Specifies the offset into the buffer object's data store from which data will be returned.</param>
    /// <param name="count">Specifies the number of <typeparamref name="T"/>s to be returned.</param>
    /// <param name="data">An array of <typeparamref name="T"/>s that will be filled with the data from the buffer object.</param>
    unsafe void GetBufferSubData< T >( int target, int offsetCount, int count, ref T[] data ) where T : unmanaged;

    /// <summary>
    /// Map a buffer object's data store into the client's address space.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="access">
    /// Specifies a combination of access flags indicating the desired access to the range of the buffer
    /// object's data store. One of <see cref="GL_READ_ONLY"/>, <see cref="GL_WRITE_ONLY"/> or
    /// <see cref="GL_READ_WRITE"/>.
    /// </param>
    /// <returns>Returns a pointer to the beginning of the mapped range.</returns>
    unsafe void* MapBuffer( int target, int access );

    /// <summary>
    /// Map a buffer object's data store into the client's address space.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="access">
    /// Specifies a combination of access flags indicating the desired access to the range of the buffer
    /// object's data store. One of <see cref="GL_READ_ONLY"/>, <see cref="GL_WRITE_ONLY"/> or
    /// <see cref="GL_READ_WRITE"/>.
    /// </param>
    /// <returns>Returns a type-safe and memory-safe <see cref="System.Span{T}"/> of the buffers data.</returns>
    unsafe Span< T > MapBuffer< T >( int target, int access ) where T : unmanaged;

    /// <summary>
    /// Release a mapped buffer object's data store.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object being unmapped. The symbolic constant must be
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DRAW_INDIRECT_BUFFER"/>, <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>
    /// , <see cref="GL_PIXEL_UNPACK_BUFFER"/>, <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> unless the data store contents have become corrupt during the time the data store was
    /// mapped. This can occur for system-specific reasons that affect the availability of graphics memory, such as screen
    /// mode changes. In such situations, <see cref="GLBindings.glUnmapBuffer"/> may return <see langword="false"/> to indicate that
    /// the contents of the buffer have become corrupt and should be considered undefined. An application must detect this
    /// rare condition and reinitialize the data store.
    /// </returns>
    bool UnmapBuffer( int target );

    /// <summary>
    /// Return parameters of a buffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a buffer object parameter. Accepted values are
    /// <see cref="GL_BUFFER_ACCESS"/>, <see cref="GL_BUFFER_MAPPED"/>, <see cref="GL_BUFFER_SIZE"/>,
    /// <see cref="GL_BUFFER_USAGE"/>.
    /// </param>
    /// <param name="params">A pointer to a memory location where the returned data will be placed.</param>
    unsafe void GetBufferParameteriv( int target, int pname, int* parameters );

    /// <summary>
    /// Return parameters of a buffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object. The symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a buffer object parameter. Accepted values are
    /// <see cref="GL_BUFFER_ACCESS"/>, <see cref="GL_BUFFER_MAPPED"/>, <see cref="GL_BUFFER_SIZE"/>,
    /// <see cref="GL_BUFFER_USAGE"/>.
    /// </param>
    /// <param name="params">A <see langword="ref"/> to an integer array where the returned data will be placed.</param>
    unsafe void GetBufferParameteriv( int target, int pname, ref int[] parameters );

    /// <summary>
    /// Return the pointer to a mapped buffer object's data store.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object being mapped. The symbolic constant must be
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DRAW_INDIRECT_BUFFER"/>, <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>
    /// , <see cref="GL_PIXEL_UNPACK_BUFFER"/>, <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="pname">Specifies the pointer to be returned. Accepted values are <see cref="GL_BUFFER_MAP_POINTER"/>.</param>
    /// <param name="params">A pointer to a memory location where the returned data will be placed.</param>
    unsafe void GetBufferPointerv( int target, int pname, void** parameters );

    /// <summary>
    /// Return the pointer to a mapped buffer object's data store.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object being mapped. The symbolic constant must be
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DRAW_INDIRECT_BUFFER"/>, <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>
    /// , <see cref="GL_PIXEL_UNPACK_BUFFER"/>, <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="pname">Specifies the pointer to be returned. Accepted values are <see cref="GL_BUFFER_MAP_POINTER"/>.</param>
    /// <param name="params">
    /// A <see langword="ref"/> to an array of <see cref="IntPtr"/>s where the returned pointer(s) will
    /// be placed.
    /// </param>
    unsafe void GetBufferPointerv( int target, int pname, ref IntPtr[] parameters );

    /// <summary>
    /// Set the RGB blend equation and alpha blend equation separately
    /// </summary>
    /// <param name="modeRGB">
    /// Specifies the RGB blend equation, how the red, green, and blue components of the source and
    /// destination colors are combined. Must be <see cref="GL_FUNC_ADD"/>, <see cref="GL_FUNC_SUBTRACT"/>,
    /// <see cref="GL_FUNC_REVERSE_SUBTRACT"/>, <see cref="GL_MIN"/> or <see cref="GL_MAX"/>.
    /// </param>
    /// <param name="modeAlpha">
    /// Specifies the alpha blend equation, how the alpha component of the source and destination
    /// colors are combined. Must be <see cref="GL_FUNC_ADD"/>, <see cref="GL_FUNC_SUBTRACT"/>,
    /// <see cref="GL_FUNC_REVERSE_SUBTRACT"/>, <see cref="GL_MIN"/> or <see cref="GL_MAX"/>.
    /// </param>
    void BlendEquationSeparate( int modeRGB, int modeAlpha );

    /// <summary>
    /// Specify a list of color buffers to be drawn into
    /// </summary>
    /// <param name="n">Specifies the number of buffers in the list that follows.</param>
    /// <param name="bufs">
    /// Specifies a pointer to an array of symbolic constants specifying the buffers into which fragment
    /// colors or data values will be written. The symbolic constants must be <see cref="GL_NONE"/>,
    /// <see cref="GL_FRONT_LEFT"/>, <see cref="GL_FRONT_RIGHT"/>, <see cref="GL_BACK_LEFT"/>,
    /// <see cref="GL_BACK_RIGHT"/>, <see cref="GL_COLOR_ATTACHMENT0"/> through <see cref="GL_COLOR_ATTACHMENT31"/>.
    /// </param>
    unsafe void DrawBuffers( int n, int* bufs );

    /// <summary>
    /// Specify a list of color buffers to be drawn into
    /// </summary>
    /// <param name="bufs">
    /// Specifies an array of symbol constants specifying the buffers into which the fragment colors or data
    /// values will be written. The symbolic constants must be <see cref="GL_NONE"/>, <see cref="GL_FRONT_LEFT"/>,
    /// <see cref="GL_FRONT_RIGHT"/>, <see cref="GL_BACK_LEFT"/>, <see cref="GL_BACK_RIGHT"/>,
    /// <see cref="GL_COLOR_ATTACHMENT0"/> through <see cref="GL_COLOR_ATTACHMENT31"/>.
    /// </param>
    unsafe void DrawBuffers( params int[] bufs );

    /// <summary>
    /// Set front and back stencil test actions
    /// </summary>
    /// <param name="face">
    /// Specifies whether the stencil test applies to front and/or back-facing polygons, or both. Must be
    /// <see cref="GL_FRONT"/>, <see cref="GL_BACK"/> or <see cref="GL_FRONT_AND_BACK"/>.
    /// </param>
    /// <param name="sfail">
    /// Specifies the action to take when the stencil test fails. Must be <see cref="GL_KEEP"/>,
    /// <see cref="GL_ZERO"/>, <see cref="GL_REPLACE"/>, <see cref="GL_INCR"/>, <see cref="GL_INCR_WRAP"/>,
    /// <see cref="GL_DECR"/>, <see cref="GL_DECR_WRAP"/> or <see cref="GL_INVERT"/>.
    /// </param>
    /// <param name="dpfail">
    /// Specifies the stencil action when the stencil test passes, but the depth test fails.
    /// <paramref name="dpfail"/> accepts the same symbolic constants as <paramref name="sfail"/>.
    /// </param>
    /// <param name="dppass">
    /// Specifies the stencil action when both the stencil test and the depth test pass, or when the
    /// stencil test passes and either there is no depth buffer or depth testing is not enabled. <paramref name="dppass"/>
    /// accepts the same symbolic constants as <paramref name="sfail"/>.
    /// </param>
    void StencilOpSeparate( int face, int sfail, int dpfail, int dppass );

    /// <summary>
    /// Set front and/or back function and reference value for stencil testing
    /// </summary>
    /// <param name="face">
    /// Specifies whether the stencil test applies to front and/or back-facing polygons, or both. Must be
    /// <see cref="GL_FRONT"/>, <see cref="GL_BACK"/> or <see cref="GL_FRONT_AND_BACK"/>.
    /// </param>
    /// <param name="func">
    /// Specifies the test function. Eight symbolic constants are accepted: <see cref="GL_NEVER"/>,
    /// <see cref="GL_LESS"/>, <see cref="GL_LEQUAL"/>, <see cref="GL_GREATER"/>, <see cref="GL_GEQUAL"/>,
    /// <see cref="GL_EQUAL"/>, <see cref="GL_NOTEQUAL"/> and <see cref="GL_ALWAYS"/>. The initial value is
    /// <see cref="GL_ALWAYS"/>.
    /// </param>
    /// <param name="ref">
    /// Specifies the reference value for the stencil test. <paramref name="ref"/> is clamped to the range
    /// [0, 2^n - 1], where n is the number of bitplanes in the stencil buffer. The initial value is 0.
    /// </param>
    /// <param name="mask">
    /// Specifies a mask that is ANDed with both the reference value and the stored stencil value when the
    /// test is done. The initial value is all 1's.
    /// </param>
    void StencilFuncSeparate( int face, int func, int @ref, uint mask );

    /// <summary>
    /// Control the front and back writing of individual bits in the stencil planes
    /// </summary>
    /// <param name="face">
    /// Specifies whether the stencil writemask applies to front and/or back-facing polygons, or both. Must
    /// be <see cref="GL_FRONT"/>, <see cref="GL_BACK"/> or <see cref="GL_FRONT_AND_BACK"/>.
    /// </param>
    /// <param name="mask">
    /// Specifies a bit mask to enable and disable writing of individual bits in the stencil planes.
    /// Initially, the mask is all 1's.
    /// </param>
    void StencilMaskSeparate( int face, uint mask );

    /// <summary>
    /// Attaches a shader object to a program object
    /// </summary>
    /// <param name="program">Specifies the program object to which a shader object will be attached.</param>
    /// <param name="shader">Specifies the shader object that is to be attached.</param>
    void AttachShader( uint program, uint shader );

    /// <summary>
    /// Associates a generic vertex attribute index with a named attribute variable. This is typically replaced with the
    /// <c>location</c> layout qualifier in the vertex shader.
    /// </summary>
    /// <param name="program">Specifies the program object in which the association is to be made.</param>
    /// <param name="index">Specifies the index of the generic vertex attribute to be bound.</param>
    /// <param name="name">
    /// Specifies a null terminated string containing the name of the vertex shader attribute variable to
    /// which index is to be bound.
    /// </param>
    unsafe void BindAttribLocation( uint program, uint index, Byte* name );

    /// <summary>
    /// Associates a generic vertex attribute index with a named attribute variable. This is typically replaced with the
    /// <c>location</c> layout qualifier in the vertex shader.
    /// </summary>
    /// <param name="program">Specifies the program object in which the association is to be made.</param>
    /// <param name="index">Specifies the index of the generic vertex attribute to be bound.</param>
    /// <param name="name">
    /// Specifies a string containing the name of the vertex shader attribute variable to which index is to
    /// be bound.
    /// </param>
    unsafe void BindAttribLocation( uint program, uint index, string name );

    /// <summary>
    /// Compiles a shader object
    /// </summary>
    /// <param name="shader">Specifies the shader object to be compiled.</param>
    void CompileShader( uint shader );

    /// <summary>
    /// Creates a program object
    /// </summary>
    /// <returns>The name of the program object created.</returns>
    uint CreateProgram();

    /// <summary>
    /// Creates a shader object
    /// </summary>
    /// <param name="type">
    /// Specifies the type of shader to be created. Must be <see cref="GL_COMPUTE_SHADER"/>,
    /// <see cref="GL_VERTEX_SHADER"/>, <see cref="GL_TESS_CONTROL_SHADER"/>, <see cref="GL_TESS_EVALUATION_SHADER"/>,
    /// <see cref="GL_GEOMETRY_SHADER"/> or <see cref="GL_FRAGMENT_SHADER"/>.
    /// </param>
    /// <returns>The name of the shader object created.</returns>
    uint CreateShader( int type );

    /// <summary>
    /// Deletes a program object
    /// </summary>
    /// <param name="program">Specifies the program object to be deleted.</param>
    void DeleteProgram( uint program );

    /// <summary>
    /// Deletes a shader object
    /// </summary>
    /// <param name="shader">Specifies the shader object to be deleted.</param>
    void DeleteShader( uint shader );

    /// <summary>
    /// Detaches a shader object from a program object
    /// </summary>
    /// <param name="program">Specifies the program object from which to detach the shader object.</param>
    /// <param name="shader">Specifies the shader object to be detached.</param>
    void DetachShader( uint program, uint shader );

    /// <summary>
    /// Disables a generic vertex attribute array
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be disabled.</param>
    void DisableVertexAttribArray( uint index );

    /// <summary>
    /// Enables a generic vertex attribute array
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be enabled.</param>
    void EnableVertexAttribArray( uint index );

    /// <summary>
    /// Returns information about an active attribute variable for the specified program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="index">Specifies the index of the attribute variable to be queried.</param>
    /// <param name="bufSize">
    /// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer
    /// indicated by name (excluding the null terminator) when information about the variable name is returned.
    /// </param>
    /// <param name="length">
    /// Returns the number of characters actually written by OpenGL in the string indicated by name
    /// (excluding the null terminator) if a value other than NULL is passed.
    /// </param>
    /// <param name="size">
    /// Returns the size of the attribute variable that is written into size if a value other than NULL is
    /// passed.
    /// </param>
    /// <param name="type">
    /// Returns the data type of the attribute variable that is written into type if a value other than NULL
    /// is passed.
    /// </param>
    /// <param name="name">Returns a null-terminated string containing the name of the attribute variable.</param>
    unsafe void GetActiveAttrib( uint program, uint index, int bufSize, int* length, int* size, int* type, Byte* name );

    /// <summary>
    /// Returns information about an active attribute variable for the specified program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="index">Specifies the index of the attribute variable to be queried.</param>
    /// <param name="bufSize">
    /// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer
    /// indicated by name (excluding the null terminator) when information about the variable name is returned.
    /// </param>
    /// <param name="size">Returns the size of the attribute variable.</param>
    /// <param name="type">Returns the data type of the attribute variable.</param>
    /// <returns>Returns a managed string containing the name of the attribute variable.</returns>
    unsafe string GetActiveAttrib( uint program, uint index, int bufSize, out int size, out int type );

    /// <summary>
    /// Returns information about an active uniform variable for the specified program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="index">Specifies the index of the uniform variable to be queried.</param>
    /// <param name="bufSize">
    /// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer
    /// indicated by name (excluding the null terminator) when information about the variable name is returned.
    /// </param>
    /// <param name="length">
    /// Returns the number of characters actually written by OpenGL in the string indicated by name
    /// (excluding the null terminator) if a value other than NULL is passed.
    /// </param>
    /// <param name="size">
    /// Returns the size of the uniform variable that is written into size if a value other than NULL is
    /// passed.
    /// </param>
    /// <param name="type">
    /// Returns the data type of the uniform variable that is written into type if a value other than NULL
    /// is passed.
    /// </param>
    /// <param name="name">Returns a null-terminated string containing the name of the uniform variable.</param>
    unsafe void GetActiveUniform( uint program, uint index, int bufSize, int* length, int* size, int* type, Byte* name );

    /// <summary>
    /// Returns information about an active uniform variable for the specified program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="index">Specifies the index of the uniform variable to be queried.</param>
    /// <param name="bufSize">
    /// Specifies the maximum number of characters OpenGL is allowed to write in the character buffer
    /// indicated by name (excluding the null terminator) when information about the variable name is returned.
    /// </param>
    /// <param name="size">Returns the size of the uniform variable.</param>
    /// <param name="type">Returns the data type of the uniform variable.</param>
    /// <returns>Returns a managed string containing the name of the uniform variable.</returns>
    unsafe string GetActiveUniform( uint program, uint index, int bufSize, out int size, out int type );

    /// <summary>
    /// Returns the shader objects attached to program
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="maxCount">Specifies the size of the array for storing object names.</param>
    /// <param name="count">Returns the number of names actually returned in shaders.</param>
    /// <param name="shaders">Returns the names of the shader objects attached to program.</param>
    unsafe void GetAttachedShaders( uint program, int maxCount, int* count, uint* shaders );

    /// <summary>
    /// Returns the shader objects attached to program
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="maxCount">Specifies a maximum amount of shaders to return.</param>
    /// <returns>Returns an array of shader objects attached to program resized to the amount of shaders actually attached.</returns>
    unsafe uint[] GetAttachedShaders( uint program, int maxCount );

    /// <summary>
    /// Returns the location of an attribute variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="name">Specifies the name of the attribute variable whose location is to be queried.</param>
    /// <returns>
    /// Returns the location of the attribute variable name if it is found in program. If name starts with the
    /// reserved prefix "gl_", a location of -1 is returned.
    /// </returns>
    unsafe int GetAttribLocation( uint program, Byte* name );

    /// <summary>
    /// Returns the location of an attribute variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="name">Specifies the name of the attribute variable whose location is to be queried.</param>
    /// <returns>
    /// Returns the location of the attribute variable name if it is found in program. If name starts with the
    /// reserved prefix "gl_", a location of -1 is returned.
    /// </returns>
    unsafe int GetAttribLocation( uint program, string name );

    /// <summary>
    /// Returns a parameter from a program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="pname">
    /// Specifies the object parameter to query. Refer to <see href="https://docs.gl/gl4/glGetProgram"/>
    /// for a list of possible values.
    /// </param>
    /// <param name="parameters">Returns the requested object parameter.</param>
    unsafe void GetProgramiv( uint program, int pname, int* parameters );

    /// <summary>
    /// Returns a parameter from a program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="pname">
    /// Specifies the object parameter to query. Refer to <see href="https://docs.gl/gl4/glGetProgram"/>
    /// for a list of possible values.
    /// </param>
    /// <param name="parameters">A <see langword="ref"/> to an integer array where the returned value(s) will be placed.</param>
    unsafe void GetProgramiv( uint program, int pname, ref int[] parameters );

    /// <summary>
    /// Returns the information log for a program object
    /// </summary>
    /// <param name="program">Specifies the program object whose information log is to be queried.</param>
    /// <param name="bufSize">Specifies the size of the character buffer for storing the returned information log.</param>
    /// <param name="length">Returns the length of the string returned in infoLog (excluding the null terminator).</param>
    /// <param name="infoLog">Specifies an array of characters that is used to return the information log.</param>
    unsafe void GetProgramInfoLog( uint program, int bufSize, int* length, Byte* infoLog );

    /// <summary>
    /// Returns the information log for a program object
    /// </summary>
    /// <param name="program">Specifies the program object whose information log is to be queried.</param>
    /// <param name="bufSize">Specifies the size of the character buffer for storing the returned information log.</param>
    /// <returns>Returns the information log for <paramref name="program"/>, resized to the correct length.</returns>
    unsafe string GetProgramInfoLog( uint program, int bufSize );

    /// <summary>
    /// Returns a parameter from a shader object
    /// </summary>
    /// <param name="shader">Specifies the shader object to be queried.</param>
    /// <param name="pname">
    /// Specifies the object parameter to query. Accepted symbolic names are <see cref="GL_SHADER_TYPE"/>,
    /// <see cref="GL_DELETE_STATUS"/>, <see cref="GL_COMPILE_STATUS"/>, <see cref="GL_INFO_LOG_LENGTH"/>,
    /// <see cref="GL_SHADER_SOURCE_LENGTH"/>.
    /// </param>
    /// <param name="params">Returns the requested object parameter.</param>
    unsafe void GetShaderiv( uint shader, int pname, int* parameters );

    /// <summary>
    /// Returns a parameter from a shader object
    /// </summary>
    /// <param name="shader">Specifies the shader object to be queried.</param>
    /// <param name="pname">
    /// Specifies the object parameter to query. Accepted symbolic names are <see cref="GL_SHADER_TYPE"/>,
    /// <see cref="GL_DELETE_STATUS"/>, <see cref="GL_COMPILE_STATUS"/>, <see cref="GL_INFO_LOG_LENGTH"/>,
    /// <see cref="GL_SHADER_SOURCE_LENGTH"/>.
    /// </param>
    /// <param name="params">A <see langword="ref"/> to an integer array where the returned value(s) will be placed.</param>
    unsafe void GetShaderiv( uint shader, int pname, ref int[] parameters );

    /// <summary>
    /// Returns the information log for a shader object
    /// </summary>
    /// <param name="shader">Specifies the shader object whose information log is to be queried.</param>
    /// <param name="bufSize">Specifies the size of the character buffer for storing the returned information log.</param>
    /// <param name="length">Returns the length of the string returned in infoLog (excluding the null terminator).</param>
    /// <param name="infoLog">Specifies an array of characters that is used to return the information log.</param>
    unsafe void GetShaderInfoLog( uint shader, int bufSize, int* length, Byte* infoLog );

    /// <summary>
    /// Returns the information log for a shader object
    /// </summary>
    /// <param name="shader">Specifies the shader object whose information log is to be queried.</param>
    /// <param name="bufSize">Specifies the size of the character buffer for storing the returned information log.</param>
    /// <returns>Returns the information log for <paramref name="shader"/>, resized to the correct length.</returns>
    unsafe string GetShaderInfoLog( uint shader, int bufSize );

    /// <summary>
    /// Returns the source code string from a shader object
    /// </summary>
    /// <param name="shader">Specifies the shader object to be queried.</param>
    /// <param name="bufSize">Specifies the size of the character buffer for storing the returned source code string.</param>
    /// <param name="length">Returns the length of the string returned in source (excluding the null terminator).</param>
    /// <param name="source">Specifies an array of characters that is used to return the source code string.</param>
    unsafe void GetShaderSource( uint shader, int bufSize, int* length, Byte* source );

    /// <summary>
    /// Returns the source code string from a shader object
    /// </summary>
    /// <param name="shader">Specifies the shader object to be queried.</param>
    /// <param name="bufSize">Specifies the size of the character buffer for storing the returned source code string.</param>
    /// <returns>Returns the source code string for <paramref name="shader"/>, resized to the correct length.</returns>
    unsafe string GetShaderSource( uint shader, int bufSize = 4096 );

    /// <summary>
    /// Returns the location of a uniform variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="name">
    /// Points to a null terminated string containing the name of the uniform variable whose location is to
    /// be queried.
    /// </param>
    unsafe int GetUniformLocation( uint program, Byte* name );

    /// <summary>
    /// Returns the location of a uniform variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="name">A string containing the name of the uniform variable whose location is to be queried.</param>
    unsafe int GetUniformLocation( uint program, string name );

    /// <summary>
    /// Returns the value of a uniform variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="location">Specifies the location of the uniform variable to be queried.</param>
    /// <param name="params">Returns the value of the uniform variable at the location specified by location.</param>
    unsafe void GetUniformfv( uint program, int location, float* parameters );

    /// <summary>
    /// Returns the value of a uniform variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="location">Specifies the location of the uniform variable to be queried.</param>
    /// <param name="params">
    /// A <see langword="ref"/> to an array to receive the value of the uniform variable at the location
    /// specified by location.
    /// </param>
    unsafe void GetUniformfv( uint program, int location, ref float[] parameters );

    /// <summary>
    /// Returns the value of a uniform variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="location">Specifies the location of the uniform variable to be queried.</param>
    /// <param name="params">Returns the value of the uniform variable at the location specified by location.</param>
    unsafe void GetUniformiv( uint program, int location, int* parameters );

    /// <summary>
    /// Returns the value of a uniform variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="location">Specifies the location of the uniform variable to be queried.</param>
    /// <param name="params">
    /// A <see langword="ref"/> to an array to receive the value of the uniform variable at the location
    /// specified by location.
    /// </param>
    unsafe void GetUniformiv( uint program, int location, ref int[] parameters );

    /// <summary>
    /// Returns the value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute parameter to be queried.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are:
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>
    /// </param>
    /// <param name="params">
    /// Returns the value of the generic vertex attribute parameter specified by pname for the vertex
    /// attribute specified by index.
    /// </param>
    unsafe void GetVertexAttribdv( uint index, int pname, double* parameters );

    /// <summary>
    /// Returns the value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute parameter to be queried.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are:
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>
    /// </param>
    /// <param name="params">
    /// A <see langword="ref"/> to an array to receive the value of the generic vertex attribute
    /// parameter specified by pname for the vertex attribute specified by index.
    /// </param>
    unsafe void GetVertexAttribdv( uint index, int pname, ref double[] parameters );

    /// <summary>
    /// Returns the value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute parameter to be queried.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are:
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>
    /// </param>
    /// <param name="params">
    /// Returns the value of the generic vertex attribute parameter specified by pname for the vertex
    /// attribute specified by index.
    /// </param>
    unsafe void GetVertexAttribfv( uint index, int pname, float* parameters );

    /// <summary>
    /// Returns the value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute parameter to be queried.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are:
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>
    /// </param>
    /// <param name="params">
    /// A <see langword="ref"/> to an array to receive the value of the generic vertex attribute
    /// parameter specified by pname for the vertex attribute specified by index.
    /// </param>
    unsafe void GetVertexAttribfv( uint index, int pname, ref float[] parameters );

    /// <summary>
    /// Returns the value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute parameter to be queried.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are:
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>
    /// </param>
    /// <param name="params">
    /// Returns the value of the generic vertex attribute parameter specified by pname for the vertex
    /// attribute specified by index.
    /// </param>
    unsafe void GetVertexAttribiv( uint index, int pname, int* parameters );

    /// <summary>
    /// Returns the value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute parameter to be queried.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the vertex attribute parameter to be queried. Accepted values are:
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>
    /// </param>
    /// <param name="params">
    /// A <see langword="ref"/> to an array to receive the value of the generic vertex attribute
    /// parameter specified by pname for the vertex attribute specified by index.
    /// </param>
    unsafe void GetVertexAttribiv( uint index, int pname, ref int[] parameters );

    /// <summary>
    /// Return the address of the specified generic vertex attribute pointer
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute pointer to be returned.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the generic vertex attribute pointer to be returned. Accepted values
    /// are: <see cref="GL_VERTEX_ATTRIB_ARRAY_POINTER"/>
    /// </param>
    /// <param name="pointer">Returns the address of the specified generic vertex attribute pointer.</param>
    unsafe void GetVertexAttribPointerv( uint index, int pname, void** pointer );

    /// <summary>
    /// Return the address of the specified generic vertex attribute pointer
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute pointer to be returned.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the generic vertex attribute pointer to be returned. Accepted values
    /// are: <see cref="GL_VERTEX_ATTRIB_ARRAY_POINTER"/>
    /// </param>
    /// <param name="pointer">
    /// A <see langword="ref"/> to an array to receive the address of the specified generic vertex
    /// attribute pointer.
    /// </param>
    unsafe void GetVertexAttribPointerv( uint index, int pname, ref uint[] pointer );

    /// <summary>
    /// Determines if a name corresponds to a program object
    /// </summary>
    /// <param name="program">Specifies a potential program object.</param>
    /// <returns>
    /// <see langword="true"/> if program is currently the name of a program object. <see langword="false"/>
    /// otherwise.
    /// </returns>
    bool IsProgram( uint program );

    /// <summary>
    /// Determines if a name corresponds to a shader object
    /// </summary>
    /// <param name="shader">Specifies a potential shader object.</param>
    /// <returns>
    /// <see langword="true"/> if shader is currently the name of a shader object. <see langword="false"/>
    /// otherwise.
    /// </returns>
    bool IsShader( uint shader );

    /// <summary>
    /// Links a program object
    /// </summary>
    /// <param name="program">Specifies the handle of the program object to be linked.</param>
    void LinkProgram( uint program );

    /// <summary>
    /// Replaces the source code in a shader object
    /// </summary>
    /// <param name="shader">Specifies the handle of the shader object whose source code is to be replaced.</param>
    /// <param name="count">
    /// Specifies the number of elements in the <paramref name="str"/> and <paramref name="length"/>
    /// arrays.
    /// </param>
    /// <param name="str">Specifies an array of pointers to strings containing the source code to be loaded into the shader.</param>
    /// <param name="length">Specifies an array of string lengths.</param>
    unsafe void ShaderSource( uint shader, int count, Byte** str, int* length );

    /// <summary>
    /// Replaces the source code in a shader object
    /// </summary>
    /// <param name="shader">Specifies the handle of the shader object whose source code is to be replaced.</param>
    /// <param name="string">Specifies an array of strings containing the source code to be loaded into the shader.</param>
    unsafe void ShaderSource( uint shader, params string[] @string );

    /// <summary>
    /// Installs a program object as part of current rendering state
    /// </summary>
    /// <param name="program">
    /// Specifies the handle of the program object whose executables are to be used as part of current
    /// rendering state.
    /// </param>
    void UseProgram( uint program );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">Specifies the new value to be used for the uniform variable at location <paramref name="location"/>.</param>
    void Uniform1f( int location, float v0 );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">
    /// Specifies the first new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v1">
    /// Specifies the second new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    void Uniform2f( int location, float v0, float v1 );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">
    /// Specifies the first new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v1">
    /// Specifies the second new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v2">
    /// Specifies the third new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    void Uniform3f( int location, float v0, float v1, float v2 );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">
    /// Specifies the first new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v1">
    /// Specifies the second new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v2">
    /// Specifies the third new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v3">
    /// Specifies the fourth new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    void Uniform4f( int location, float v0, float v1, float v2, float v3 );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">Specifies the new value to be used for the uniform variable at location <paramref name="location"/>.</param>
    void Uniform1i( int location, int v0 );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">
    /// Specifies the first new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v1">
    /// Specifies the second new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    void Uniform2i( int location, int v0, int v1 );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">
    /// Specifies the first new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v1">
    /// Specifies the second new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v2">
    /// Specifies the third new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    void Uniform3i( int location, int v0, int v1, int v2 );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="v0">
    /// Specifies the first new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v1">
    /// Specifies the second new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v2">
    /// Specifies the third new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    /// <param name="v3">
    /// Specifies the fourth new value to be used for the uniform variable at location
    /// <paramref name="location"/>.
    /// </param>
    void Uniform4i( int location, int v0, int v1, int v2, int v3 );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform1fv( int location, int count, float* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    unsafe void Uniform1fv( int location, params float[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform2fv( int location, int count, float* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform2fv( int location, params float[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform3fv( int location, int count, float* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform3fv( int location, params float[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform4fv( int location, int count, float* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform4fv( int location, params float[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform1iv( int location, int count, int* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform1iv( int location, params int[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform2iv( int location, int count, int* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform2iv( int location, params int[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform3iv( int location, int count, int* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform3iv( int location, params int[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform4iv( int location, int count, int* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform4iv( int location, params int[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of matrices that are to be modified.</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void UniformMatrix2fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable.</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. Needs 4
    /// values per matrix.
    /// </param>
    unsafe void UniformMatrix2fv( int location, bool transpose, params float[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of matrices that are to be modified.</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void UniformMatrix3fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable.</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. Needs 9
    /// values per matrix.
    /// </param>
    unsafe void UniformMatrix3fv( int location, bool transpose, params float[] value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="count">Specifies the number of matrices that are to be modified.</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void UniformMatrix4fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specifies the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform value to be modified.</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable.</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. Needs 16
    /// values per matrix.
    /// </param>
    unsafe void UniformMatrix4fv( int location, bool transpose, params float[] value );

//TODO: Unsupported method    
//    unsafe void UniformMatrix4fv( int location, int count, bool transpose, Buffer buffer );

    /// <summary>
    /// Validates a program object
    /// </summary>
    /// <param name="program">Specifies the handle of the program object to be validated</param>
    /// <returns><c>true</c> if validation is successful, <c>false</c> otherwise</returns>
    bool ValidateProgram( uint program );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the new value for the generic vertex attribute</param>
    void VertexAttrib1d( uint index, double x );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib1dv( uint index, double* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib1dv( uint index, params double[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the new value for the generic vertex attribute</param>
    void VertexAttrib1f( uint index, float x );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib1fv( uint index, float* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib1fv( uint index, params float[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the new value for the generic vertex attribute</param>
    void VertexAttrib1s( uint index, Int16 x );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib1sv( uint index, Int16* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib1sv( uint index, params Int16[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the first component of the generic vertex attribute</param>
    /// <param name="y">Specifies the second component of the generic vertex attribute</param>
    void VertexAttrib2d( uint index, double x, double y );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib2dv( uint index, double* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib2dv( uint index, params double[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the first component of the generic vertex attribute</param>
    /// <param name="y">Specifies the second component of the generic vertex attribute</param>
    void VertexAttrib2f( uint index, float x, float y );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib2fv( uint index, float* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib2fv( uint index, params float[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the first component of the generic vertex attribute</param>
    /// <param name="y">Specifies the second component of the generic vertex attribute</param>
    void VertexAttrib2s( uint index, Int16 x, Int16 y );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib2sv( uint index, Int16* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib2sv( uint index, params Int16[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the first component of the generic vertex attribute</param>
    /// <param name="y">Specifies the second component of the generic vertex attribute</param>
    /// <param name="z">Specifies the third component of the generic vertex attribute</param>
    void VertexAttrib3d( uint index, double x, double y, double z );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib3dv( uint index, double* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib3dv( uint index, params double[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the first component of the generic vertex attribute</param>
    /// <param name="y">Specifies the second component of the generic vertex attribute</param>
    /// <param name="z">Specifies the third component of the generic vertex attribute</param>
    void VertexAttrib3f( uint index, float x, float y, float z );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib3fv( uint index, float* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib3fv( uint index, params float[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the first component of the generic vertex attribute</param>
    /// <param name="y">Specifies the second component of the generic vertex attribute</param>
    /// <param name="z">Specifies the third component of the generic vertex attribute</param>
    void VertexAttrib3s( uint index, Int16 x, Int16 y, Int16 z );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib3sv( uint index, Int16* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib3sv( uint index, params Int16[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nbv( uint index, SByte* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nbv( uint index, params SByte[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Niv( uint index, int* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Niv( uint index, params int[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nsv( uint index, Int16* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nsv( uint index, params Int16[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">Specifies the first component of the generic vertex attribute</param>
    /// <param name="y">Specifies the second component of the generic vertex attribute</param>
    /// <param name="z">Specifies the third component of the generic vertex attribute</param>
    /// <param name="w">Specifies the fourth component of the generic vertex attribute</param>
    void VertexAttrib4Nub( uint index, Byte x, Byte y, Byte z, Byte w );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nubv( uint index, Byte* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nubv( uint index, params Byte[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nuiv( uint index, uint* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nuiv( uint index, params uint[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nusv( uint index, UInt16* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4Nusv( uint index, params UInt16[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4bv( uint index, SByte* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4bv( uint index, params SByte[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">
    /// Specifies the first component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="y">
    /// Specifies the second component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="z">
    /// Specifies the third component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="w">
    /// Specifies the fourth component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    void VertexAttrib4d( uint index, double x, double y, double z, double w );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4dv( uint index, double* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4dv( uint index, params double[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">
    /// Specifies the first component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="y">
    /// Specifies the second component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="z">
    /// Specifies the third component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="w">
    /// Specifies the fourth component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    void VertexAttrib4f( uint index, float x, float y, float z, float w );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4fv( uint index, float* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4fv( uint index, params float[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4iv( uint index, int* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4iv( uint index, params int[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="x">
    /// Specifies the first component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="y">
    /// Specifies the second component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="z">
    /// Specifies the third component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    /// <param name="w">
    /// Specifies the fourth component of the vector to be used when updating the current value of the generic
    /// vertex attribute
    /// </param>
    void VertexAttrib4s( uint index, Int16 x, Int16 y, Int16 z, Int16 w );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4sv( uint index, Int16* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4sv( uint index, params Int16[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4ubv( uint index, Byte* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4ubv( uint index, params Byte[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4uiv( uint index, uint* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4uiv( uint index, params uint[] v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies a pointer to an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4usv( uint index, UInt16* v );

    /// <summary>
    /// Specifies the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="v">Specifies an array that contains the new values for the generic vertex attribute</param>
    unsafe void VertexAttrib4usv( uint index, params UInt16[] v );

    /// <summary>
    /// Define an array of generic vertex attribute data
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="size">Specifies the number of components per generic vertex attribute. Must be 1,2,3 or 4.</param>
    /// <param name="type">
    /// Specifies the data type of each component in the array. The symbolic constants
    /// <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_SHORT"/>, <see cref="GL_UNSIGNED_SHORT"/>,
    /// <see cref="GL_INT"/>, <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_HALF_FLOAT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_DOUBLE"/>, <see cref="GL_FIXED"/>, <see cref="GL_INT_2_10_10_10_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>, and <see cref="GL_UNSIGNED_INT_10F_11F_11F_REV"/> are accepted. The
    /// initial value is <see cref="GL_FLOAT"/>.
    /// </param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized <see langword="true"/> or
    /// converted directly as fixed-point values <see langword="false"/> when they are accessed.
    /// </param>
    /// <param name="stride">
    /// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the
    /// generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
    /// </param>
    /// <param name="pointer">
    /// Specifies an offset of the first component in the first generic vertex attribute in the array in
    /// the data store of the buffer currently bound to the <see cref="GL_ARRAY_BUFFER"/> target. The initial value is 0.
    /// </param>
    unsafe void VertexAttribPointer( uint index, int size, int type, bool normalized, int stride, void* pointer );

    /// <summary>
    /// Define an array of generic vertex attribute data
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified</param>
    /// <param name="size">Specifies the number of components per generic vertex attribute. Must be 1,2,3 or 4.</param>
    /// <param name="type">
    /// Specifies the data type of each component in the array. The symbolic constants
    /// <see cref="GL_BYTE"/>, <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_SHORT"/>, <see cref="GL_UNSIGNED_SHORT"/>,
    /// <see cref="GL_INT"/>, <see cref="GL_UNSIGNED_INT"/>, <see cref="GL_HALF_FLOAT"/>, <see cref="GL_FLOAT"/>,
    /// <see cref="GL_DOUBLE"/>, <see cref="GL_FIXED"/>, <see cref="GL_INT_2_10_10_10_REV"/>,
    /// <see cref="GL_UNSIGNED_INT_2_10_10_10_REV"/>, and <see cref="GL_UNSIGNED_INT_10F_11F_11F_REV"/> are accepted. The
    /// initial value is <see cref="GL_FLOAT"/>.
    /// </param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized <see langword="true"/> or
    /// converted directly as fixed-point values <see langword="false"/> when they are accessed.
    /// </param>
    /// <param name="stride">
    /// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the
    /// generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.
    /// </param>
    /// <param name="pointer">
    /// Specifies an offset of the first component in the first generic vertex attribute in the array in
    /// the data store of the buffer currently bound to the <see cref="GL_ARRAY_BUFFER"/> target. The initial value is 0.
    /// </param>
    unsafe void VertexAttribPointer( uint index, int size, int type, bool normalized, int stride, uint pointer );

//TODO: Unsupported method    
//    unsafe void VertexAttribPointer( int location, int size, int type, bool normalized, int stride, Buffer buffer );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="count">
    /// Specifies the number of matrices that are to be modified. This should be 1 if the targeted uniform
    /// variable is not an array of matrices, and 1 or more if it is an array of matrices
    /// </param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies a pointer to an array of values that will be used to update the specified uniform
    /// variable
    /// </param>
    unsafe void UniformMatrix2x3fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. 6 values
    /// per matrix.
    /// </param>
    unsafe void UniformMatrix2x3fv( int location, bool transpose, params float[] value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="count">
    /// Specifies the number of matrices that are to be modified. This should be 1 if the targeted uniform
    /// variable is not an array of matrices, and 1 or more if it is an array of matrices
    /// </param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies a pointer to an array of values that will be used to update the specified uniform
    /// variable
    /// </param>
    unsafe void UniformMatrix3x2fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. 6 values
    /// per matrix.
    /// </param>
    unsafe void UniformMatrix3x2fv( int location, bool transpose, params float[] value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="count">
    /// Specifies the number of matrices that are to be modified. This should be 1 if the targeted uniform
    /// variable is not an array of matrices, and 1 or more if it is an array of matrices
    /// </param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies a pointer to an array of values that will be used to update the specified uniform
    /// variable
    /// </param>
    unsafe void UniformMatrix2x4fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. 8 values
    /// per matrix.
    /// </param>
    unsafe void UniformMatrix2x4fv( int location, bool transpose, params float[] value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="count">
    /// Specifies the number of matrices that are to be modified. This should be 1 if the targeted uniform
    /// variable is not an array of matrices, and 1 or more if it is an array of matrices
    /// </param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies a pointer to an array of values that will be used to update the specified uniform
    /// variable
    /// </param>
    unsafe void UniformMatrix4x2fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. 8 values
    /// per matrix.
    /// </param>
    unsafe void UniformMatrix4x2fv( int location, bool transpose, params float[] value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="count">
    /// Specifies the number of matrices that are to be modified. This should be 1 if the targeted uniform
    /// variable is not an array of matrices, and 1 or more if it is an array of matrices
    /// </param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies a pointer to an array of values that will be used to update the specified uniform
    /// variable
    /// </param>
    unsafe void UniformMatrix3x4fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. 12 values
    /// per matrix.
    /// </param>
    unsafe void UniformMatrix3x4fv( int location, bool transpose, params float[] value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="count">
    /// Specifies the number of matrices that are to be modified. This should be 1 if the targeted uniform
    /// variable is not an array of matrices, and 1 or more if it is an array of matrices
    /// </param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies a pointer to an array of values that will be used to update the specified uniform
    /// variable
    /// </param>
    unsafe void UniformMatrix4x3fv( int location, int count, bool transpose, float* value );

    /// <summary>
    /// Specify the value of a uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified</param>
    /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable</param>
    /// <param name="value">
    /// Specifies an array of values that will be used to update the specified uniform variable. 12 values
    /// per matrix.
    /// </param>
    unsafe void UniformMatrix4x3fv( int location, bool transpose, params float[] value );

    /// <summary>
    /// Enable and disable writing of frame buffer color components
    /// </summary>
    /// <param name="index">Specifies the index of the draw buffer for which to modify the color mask.</param>
    /// <param name="r">Specifies whether red can or cannot be written into the frame buffer.</param>
    /// <param name="g">Specifies whether green can or cannot be written into the frame buffer.</param>
    /// <param name="b">Specifies whether blue can or cannot be written into the frame buffer.</param>
    /// <param name="a">Specifies whether alpha can or cannot be written into the frame buffer.</param>
    void ColorMaski( uint index, bool r, bool g, bool b, bool a );

    /// <summary>
    /// Return the boolean value of a selected indexed state variable
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the query. Refer to <see href="https://docs.gl/gl4/glGet"/> for a list of
    /// possible targets.
    /// </param>
    /// <param name="index">Specifies the index of the indexed state variable to be queried.</param>
    /// <param name="data">Returns the requested data.</param>
    unsafe void GetBooleani_v( int target, uint index, bool* data );

    /// <summary>
    /// Return the boolean value of a selected indexed state variable
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the query. Refer to <see href="https://docs.gl/gl4/glGet"/> for a list of
    /// possible targets.
    /// </param>
    /// <param name="index">Specifies the index of the indexed state variable to be queried.</param>
    /// <param name="data">A <see langword="ref"/> to an array to receive the data.</param>
    unsafe void GetBooleani_v( int target, uint index, ref bool[] data );

    /// <summary>
    /// Return the integer value of a selected indexed state variable
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the query. Refer to <see href="https://docs.gl/gl4/glGet"/> for a list of
    /// possible targets.
    /// </param>
    /// <param name="index">Specifies the index of the indexed state variable to be queried.</param>
    /// <param name="data">Returns the requested data.</param>
    unsafe void GetIntegeri_v( int target, uint index, int* data );

    /// <summary>
    /// Return the integer value of a selected indexed state variable
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the query. Refer to <see href="https://docs.gl/gl4/glGet"/> for a list of
    /// possible targets.
    /// </param>
    /// <param name="index">Specifies the index of the indexed state variable to be queried.</param>
    /// <param name="data">A <see langword="ref"/> to an array to receive the data.</param>
    unsafe void GetIntegeri_v( int target, uint index, ref int[] data );

    /// <summary>
    /// Enable capabilities for a specific indexed target
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the capability to enable. Refer to
    /// <see href="https://docs.gl/gl4/glEnable"/> for a list of possible capabilities.
    /// </param>
    /// <param name="index">Specifies the index of the target to enable or disable.</param>
    void Enablei( int target, uint index );

    /// <summary>
    /// Disable capabilities for a specific indexed target
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the capability to enable. Refer to
    /// <see href="https://docs.gl/gl4/glEnable"/> for a list of possible capabilities.
    /// </param>
    /// <param name="index">Specifies the index of the target to enable or disable.</param>
    void Disablei( int target, uint index );

    /// <summary>
    /// Test whether a specific indexed capability is enabled
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the capability to enable. Refer to
    /// <see href="https://docs.gl/gl4/glEnable"/> for a list of possible capabilities.
    /// </param>
    /// <param name="index">Specifies the index of the target to enable or disable.</param>
    bool IsEnabledi( int target, uint index );

    /// <summary>
    /// Start transform feedback operations
    /// </summary>
    /// <param name="primitiveMode">
    /// Specifies the mode used to capture vertex data. Symbolic constants <see cref="GL_POINTS"/>
    /// , <see cref="GL_LINES"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINE_STRIP"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES_ADJACENCY"/> and
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> are accepted. Refer to
    /// <see href="https://docs.gl/gl4/glBeginTransformFeedback"/> for some quirks regarding this parameter.
    /// </param>
    void BeginTransformFeedback( int primitiveMode );

    /// <summary>
    /// End transform feedback operations
    /// </summary>
    void EndTransformFeedback();

    /// <summary>
    /// Bind a range of a buffer object to an indexed buffer target
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the bind operation. <paramref name="target"/> must be one of
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/>,
    /// <see cref="GL_UNIFORM_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>.
    /// </param>
    /// <param name="index">Specifies the index of the binding point within the array specified by <paramref name="target"/>.</param>
    /// <param name="buffer">Specifies the name of a buffer object whose storage to bind to the specified binding point.</param>
    /// <param name="offset">Specifies the starting offset within the buffer of the range to bind.</param>
    /// <param name="size">
    /// Specifies the amount of data in bytes from the buffer object that is to be made available for
    /// reading.
    /// </param>
    void BindBufferRange( int target, uint index, uint buffer, int offset, int size );

    /// <summary>
    /// Bind a buffer object to an indexed buffer target
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the bind operation. <paramref name="target"/> must be one of
    /// <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/>,
    /// <see cref="GL_UNIFORM_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>.
    /// </param>
    /// <param name="index">Specifies the index of the binding point within the array specified by <paramref name="target"/>.</param>
    /// <param name="buffer">Specifies the name of a buffer object whose storage to bind to the specified binding point.</param>
    void BindBufferBase( int target, uint index, uint buffer );

    /// <summary>
    /// Specify values to record in transform feedback buffers
    /// </summary>
    /// <param name="program">Specifies the name of the program object whose transform feedback varyings are to be specified.</param>
    /// <param name="count">
    /// Specifies the number of transform feedback varyings to capture when transform feedback is active.
    /// This number may be zero.
    /// </param>
    /// <param name="varyings">
    /// Specifies an array of pointers to buffers into which to place the names of the transform
    /// feedback varyings for the program named by <paramref name="program"/>.
    /// </param>
    /// <param name="bufferMode">
    /// Specifies the mode used to capture the varying variables when transform feedback is active.
    /// <paramref name="bufferMode"/> must be one of <see cref="GL_INTERLEAVED_ATTRIBS"/> or
    /// <see cref="GL_SEPARATE_ATTRIBS"/>.
    /// </param>
    unsafe void TransformFeedbackVaryings( uint program, int count, Byte** varyings, int bufferMode );

    /// <summary>
    /// Specify values to record in transform feedback buffers
    /// </summary>
    /// <param name="program">Specifies the name of the program object whose transform feedback varyings are to be specified.</param>
    /// <param name="varyings">Specifies an array of names of varying variables to use for transform feedback.</param>
    /// <param name="bufferMode">
    /// Specifies the mode used to capture the varying variables when transform feedback is active.
    /// <paramref name="bufferMode"/> must be one of <see cref="GL_INTERLEAVED_ATTRIBS"/> or
    /// <see cref="GL_SEPARATE_ATTRIBS"/>.
    /// </param>
    unsafe void TransformFeedbackVaryings( uint program, string[] varyings, int bufferMode );

    /// <summary>
    /// Retrieve information about a varying variable from a program object's active transform feedback varyings
    /// </summary>
    /// <param name="program">Specifies the name of the program containing the queried varying variable.</param>
    /// <param name="index">Specifies the index of the varying variable to query.</param>
    /// <param name="bufSize">
    /// Specifies the maximum number of characters OpenGL is allowed to write into
    /// <paramref name="name"/>.
    /// </param>
    /// <param name="length">Returns the number of characters actually written by OpenGL into <paramref name="name"/>.</param>
    /// <param name="size">Returns the size of the requested varying variable.</param>
    /// <param name="type">Returns the data type of the requested varying variable.</param>
    /// <param name="name">Returns a null-terminated string containing the name of the requested varying variable.</param>
    unsafe void GetTransformFeedbackVarying( uint program,
                                               uint index,
                                               int bufSize,
                                               int* length,
                                               int* size,
                                               int* type,
                                               Byte* name );

    /// <summary>
    /// Retrieve information about a varying variable from a program object's active transform feedback varyings
    /// </summary>
    /// <param name="program">Specifies the name of the program containing the queried varying variable.</param>
    /// <param name="index">Specifies the index of the varying variable to query.</param>
    /// <param name="bufSize">
    /// Specifies the maximum number of characters OpenGL is allowed to write into
    /// <paramref name="name"/>.
    /// </param>
    /// <param name="size">Returns the size of the requested varying variable.</param>
    /// <param name="type">Returns the data type of the requested varying variable.</param>
    /// <returns>Returns a managed string containing the name of the requested varying variable.</returns>
    unsafe string GetTransformFeedbackVarying( uint program, uint index, int bufSize, out int size, out int type );

    /// <summary>
    /// Specify whether data read via <see cref="GLBindings.glReadPixels"/> should be clamped.
    /// </summary>
    /// <param name="target">Specifies the target to be clamped. Must be <see cref="GL_CLAMP_READ_COLOR"/>.</param>
    /// <param name="clamp">
    /// Specifies whether to apply color clamping. <see langword="true"/> specifies that clamping is
    /// enabled, <see langword="false"/> specifies that clamping is disabled.
    /// </param>
    void ClampColor( int target, int clamp );

    /// <summary>
    /// Start conditional rendering
    /// </summary>
    /// <param name="id">
    /// Specifies the name of an occlusion query object whose results are used to determine if the rendering
    /// commands are discarded or not.
    /// </param>
    /// <param name="mode">
    /// Specifies how <see cref="glglBeginConditionalRender"/> interprets the results of the occlusion
    /// query.
    /// </param>
    void BeginConditionalRender( uint id, int mode );

    /// <summary>
    /// End conditional rendering
    /// </summary>
    void EndConditionalRender();

    /// <summary>
    /// Define an array of generic vertex attribute data
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="size">Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4.</param>
    /// <param name="type">
    /// Specifies the data type of each component in the array. Must be one of <see cref="GL_BYTE"/>,
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_SHORT"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_INT"/>,
    /// <see cref="GL_UNSIGNED_INT"/> or <see cref="GL_DOUBLE"/>.
    /// </param>
    /// <param name="stride">
    /// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the
    /// generic vertex attributes are understood to be tightly packed in the array.
    /// </param>
    /// <param name="pointer">
    /// Specifies an offset of the first component of the first generic vertex attribute in the array in
    /// the data store of the buffer currently bound to the <see cref="GL_ARRAY_BUFFER"/> target. The initial value is 0.
    /// </param>
    unsafe void VertexAttribIPointer( uint index, int size, int type, int stride, void* pointer );

    /// <summary>
    /// Define an array of generic vertex attribute data
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="size">Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4.</param>
    /// <param name="type">
    /// Specifies the data type of each component in the array. Must be one of <see cref="GL_BYTE"/>,
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_SHORT"/>, <see cref="GL_UNSIGNED_SHORT"/>, <see cref="GL_INT"/>,
    /// <see cref="GL_UNSIGNED_INT"/> or <see cref="GL_DOUBLE"/>.
    /// </param>
    /// <param name="stride">
    /// Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the
    /// generic vertex attributes are understood to be tightly packed in the array.
    /// </param>
    /// <param name="pointer">
    /// Specifies an offset of the first component of the first generic vertex attribute in the array in
    /// the data store of the buffer currently bound to the <see cref="GL_ARRAY_BUFFER"/> target. The initial value is 0.
    /// </param>
    unsafe void VertexAttribIPointer( uint index, int size, int type, int stride, uint pointer );

    /// <summary>
    /// Return the integer value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the generic vertex attribute parameter to be queried. Must be one of
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>.
    /// </param>
    /// <param name="parameters">Returns the requested parameter.</param>
    unsafe void GetVertexAttribIiv( uint index, int pname, int* parameters );

    /// <summary>
    /// Return the integer value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the generic vertex attribute parameter to be queried. Must be one of
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>.
    /// </param>
    /// <param name="parameters">A <see langword="ref"/> to an array into which the returned values will be placed.</param>
    unsafe void GetVertexAttribIiv( uint index, int pname, ref int[] parameters );

    /// <summary>
    /// Return the unsigned integer value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the generic vertex attribute parameter to be queried. Must be one of
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>.
    /// </param>
    /// <param name="parameters">Returns the requested parameter.</param>
    unsafe void GetVertexAttribIuiv( uint index, int pname, uint* parameters );

    /// <summary>
    /// Return the unsigned integer value of a generic vertex attribute parameter
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of the generic vertex attribute parameter to be queried. Must be one of
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_ENABLED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_SIZE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_STRIDE"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_TYPE"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_NORMALIZED"/>,
    /// <see cref="GL_VERTEX_ATTRIB_ARRAY_INTEGER"/>, <see cref="GL_VERTEX_ATTRIB_ARRAY_DIVISOR"/>,
    /// <see cref="GL_CURRENT_VERTEX_ATTRIB"/>.
    /// </param>
    /// <param name="parameters">A <see langword="ref"/> to an array into which the returned values will be placed.</param>
    unsafe void GetVertexAttribIuiv( uint index, int pname, ref uint[] parameters );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="x">Specifies the first component of the vertex attribute.</param>
    void VertexAttribI1i( uint index, int x );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="x">Specifies the first component of the vertex attribute.</param>
    /// <param name="y">Specifies the second component of the vertex attribute.</param>
    void VertexAttribI2i( uint index, int x, int y );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="x">Specifies the first component of the vertex attribute.</param>
    /// <param name="y">Specifies the second component of the vertex attribute.</param>
    /// <param name="z">Specifies the third component of the vertex attribute.</param>
    void VertexAttribI3i( uint index, int x, int y, int z );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="x">Specifies the first component of the vertex attribute.</param>
    /// <param name="y">Specifies the second component of the vertex attribute.</param>
    /// <param name="z">Specifies the third component of the vertex attribute.</param>
    /// <param name="w">Specifies the fourth component of the vertex attribute.</param>
    void VertexAttribI4i( uint index, int x, int y, int z, int w );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="x">Specifies the first component of the vertex attribute.</param>
    void VertexAttribI1ui( uint index, uint x );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="x">Specifies the first component of the vertex attribute.</param>
    /// <param name="y">Specifies the second component of the vertex attribute.</param>
    void VertexAttribI2ui( uint index, uint x, uint y );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="x">Specifies the first component of the vertex attribute.</param>
    /// <param name="y">Specifies the second component of the vertex attribute.</param>
    /// <param name="z">Specifies the third component of the vertex attribute.</param>
    void VertexAttribI3ui( uint index, uint x, uint y, uint z );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="x">Specifies the first component of the vertex attribute.</param>
    /// <param name="y">Specifies the second component of the vertex attribute.</param>
    /// <param name="z">Specifies the third component of the vertex attribute.</param>
    /// <param name="w">Specifies the fourth component of the vertex attribute.</param>
    void VertexAttribI4ui( uint index, uint x, uint y, uint z, uint w );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI1iv( uint index, int* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI1iv( uint index, int[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI2iv( uint index, int* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI2iv( uint index, int[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI3iv( uint index, int* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI3iv( uint index, int[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4iv( uint index, int* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4iv( uint index, int[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI1uiv( uint index, uint* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI1uiv( uint index, uint[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI2uiv( uint index, uint* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI2uiv( uint index, uint[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI3uiv( uint index, uint* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI3uiv( uint index, uint[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4uiv( uint index, uint* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4uiv( uint index, uint[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4bv( uint index, SByte* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4bv( uint index, SByte[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4sv( uint index, Int16* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4sv( uint index, Int16[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4ubv( uint index, Byte* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4ubv( uint index, Byte[] v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies the address of an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4usv( uint index, UInt16* v );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="v">Specifies an array that contains the new values for the vertex attribute.</param>
    unsafe void VertexAttribI4usv( uint index, UInt16[] v );

    /// <summary>
    /// Return the value of a uniform variable of type unsigned int
    /// </summary>
    /// <param name="program">Specifies the program object containing the uniform variable to be queried.</param>
    /// <param name="location">Specifies the location of the uniform variable to be queried.</param>
    /// <param name="params">Returns the value of the specified uniform variable.</param>
    unsafe void GetUniformuiv( uint program, int location, uint* parameters );

    /// <summary>
    /// Return the value of a uniform variable of type unsigned int
    /// </summary>
    /// <param name="program">Specifies the program object containing the uniform variable to be queried.</param>
    /// <param name="location">Specifies the location of the uniform variable to be queried.</param>
    /// <param name="params">A <see langword="ref"/> to an array to receive the value of the specified uniform variable.</param>
    unsafe void GetUniformuiv( uint program, int location, ref uint[] parameters );

    /// <summary>
    /// Bind a user-defined varying out variable to a fragment shader color number
    /// </summary>
    /// <param name="program">Specifies the program object in which the binding is to occur.</param>
    /// <param name="color">Specifies the color number to which the user-defined varying out variable is to be bound.</param>
    /// <param name="name">Specifies the name of the user-defined varying out variable to whose bound location to set.</param>
    unsafe void BindFragDataLocation( uint program, uint color, Byte* name );

    /// <summary>
    /// Bind a user-defined varying out variable to a fragment shader color number
    /// </summary>
    /// <param name="program">Specifies the program object in which the binding is to occur.</param>
    /// <param name="color">Specifies the color number to which the user-defined varying out variable is to be bound.</param>
    /// <param name="name">Specifies the name of the user-defined varying out variable to whose bound location to set.</param>
    unsafe void BindFragDataLocation( uint program, uint color, string name );

    /// <summary>
    /// Return the location of a user-defined varying out variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="name">Specifies the name of the user-defined varying out variable whose location is to be queried.</param>
    /// <returns>The location of the user-defined varying out variable specified by <paramref name="name"/> is returned.</returns>
    unsafe int GetFragDataLocation( uint program, Byte* name );

    /// <summary>
    /// Return the location of a user-defined varying out variable
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="name">Specifies the name of the user-defined varying out variable whose location is to be queried.</param>
    /// <returns>The location of the user-defined varying out variable specified by <paramref name="name"/> is returned.</returns>
    unsafe int GetFragDataLocation( uint program, string name );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="v0">Specifies the new value to be used for the specified uniform variable.</param>
    void Uniform1ui( int location, uint v0 );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="v0">Specifies the first value to be used for the specified uniform variable.</param>
    /// <param name="v1">Specifies the second value to be used for the specified uniform variable.</param>
    void Uniform2ui( int location, uint v0, uint v1 );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="v0">Specifies the first value to be used for the specified uniform variable.</param>
    /// <param name="v1">Specifies the second value to be used for the specified uniform variable.</param>
    /// <param name="v2">Specifies the third value to be used for the specified uniform variable.</param>
    void Uniform3ui( int location, uint v0, uint v1, uint v2 );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="v0">Specifies the first value to be used for the specified uniform variable.</param>
    /// <param name="v1">Specifies the second value to be used for the specified uniform variable.</param>
    /// <param name="v2">Specifies the third value to be used for the specified uniform variable.</param>
    /// <param name="v3">Specifies the fourth value to be used for the specified uniform variable.</param>
    void Uniform4ui( int location, uint v0, uint v1, uint v2, uint v3 );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform1uiv( int location, int count, uint* value );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform1uiv( int location, uint[] value );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform2uiv( int location, int count, uint* value );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform2uiv( int location, uint[] value );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform3uiv( int location, int count, uint* value );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform3uiv( int location, uint[] value );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="count">Specifies the number of elements that are to be modified.</param>
    /// <param name="value">
    /// Specifies a pointer to an array of <paramref name="count"/> values that will be used to update the
    /// specified uniform variable.
    /// </param>
    unsafe void Uniform4uiv( int location, int count, uint* value );

    /// <summary>
    /// Specify the value of an unsigned int uniform variable for the current program object
    /// </summary>
    /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
    /// <param name="value">Specifies an array of values that will be used to update the specified uniform variable.</param>
    unsafe void Uniform4uiv( int location, uint[] value );

    /// <summary>
    /// Set the value of a texture parameter for the current texture unit, with integer values
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture, which must be either <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter. <paramref name="pname"/> can be
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/> or <see cref="GL_TEXTURE_WRAP_R"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void TexParameterIiv( int target, int pname, int* param );

    /// <summary>
    /// Set the value of a texture parameter for the current texture unit, with integer values
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture, which must be either <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter. <paramref name="pname"/> can be
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/> or <see cref="GL_TEXTURE_WRAP_R"/>.
    /// </param>
    /// <param name="param">Specifies an array of values that will be used to update the specified texture parameter.</param>
    unsafe void TexParameterIiv( int target, int pname, int[] param );

    /// <summary>
    /// Set the value of a texture parameter for the current texture unit, with unsigned integer values
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture, which must be either <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter. <paramref name="pname"/> can be
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/> or <see cref="GL_TEXTURE_WRAP_R"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void TexParameterIuiv( int target, int pname, uint* param );

    /// <summary>
    /// Set the value of a texture parameter for the current texture unit, with unsigned integer values
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture, which must be either <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter. <paramref name="pname"/> can be
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/> or <see cref="GL_TEXTURE_WRAP_R"/>.
    /// </param>
    /// <param name="param">Specifies an array of values that will be used to update the specified texture parameter.</param>
    unsafe void TexParameterIuiv( int target, int pname, uint[] param );

    /// <summary>
    /// Get the value of a texture parameter for the current texture unit, with signed integer values
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture, which must be either <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter. <paramref name="pname"/> can be
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/> or <see cref="GL_TEXTURE_WRAP_R"/>.
    /// </param>
    /// <param name="parameters">Returns the texture parameter value.</param>
    unsafe void GetTexParameterIiv( int target, int pname, int* parameters );

    /// <summary>
    /// Get the value of a texture parameter for the current texture unit, with signed integer values
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture, which must be either <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter. <paramref name="pname"/> can be
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/> or <see cref="GL_TEXTURE_WRAP_R"/>.
    /// </param>
    /// <param name="parameters">A <see langword="ref"/> to an array to receive the texture parameter value.</param>
    unsafe void GetTexParameterIiv( int target, int pname, ref int[] parameters );

    /// <summary>
    /// Get the value of a texture parameter for the current texture unit, with unsigned integer values
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture, which must be either <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter. <paramref name="pname"/> can be
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/> or <see cref="GL_TEXTURE_WRAP_R"/>.
    /// </param>
    /// <param name="parameters">Returns the texture parameter value.</param>
    unsafe void GetTexParameterIuiv( int target, int pname, uint* parameters );

    /// <summary>
    /// Get the value of a texture parameter for the current texture unit, with unsigned integer values
    /// </summary>
    /// <param name="target">
    /// Specifies the target texture, which must be either <see cref="GL_TEXTURE_1D"/>,
    /// <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_2D_MULTISAMPLE"/>, <see cref="GL_TEXTURE_2D_MULTISAMPLE_ARRAY"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/> or
    /// <see cref="GL_TEXTURE_RECTANGLE"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued texture parameter. <paramref name="pname"/> can be
    /// <see cref="GL_DEPTH_STENCIL_TEXTURE_MODE"/>, <see cref="GL_TEXTURE_BASE_LEVEL"/>,
    /// <see cref="GL_TEXTURE_COMPARE_FUNC"/>, <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_MIN_LOD"/>,
    /// <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_MAX_LEVEL"/>, <see cref="GL_TEXTURE_SWIZZLE_R"/>,
    /// <see cref="GL_TEXTURE_SWIZZLE_G"/>, <see cref="GL_TEXTURE_SWIZZLE_B"/>, <see cref="GL_TEXTURE_SWIZZLE_A"/>,
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/> or <see cref="GL_TEXTURE_WRAP_R"/>.
    /// </param>
    /// <param name="parameters">A <see langword="ref"/> to an array to receive the texture parameter value.</param>
    unsafe void GetTexParameterIuiv( int target, int pname, ref uint[] parameters );

    /// <summary>
    /// Clear a buffer to an integer value. Refer to <see href="https://docs.gl/gl4/glClearBuffer"/> for details on how to
    /// use this function.
    /// </summary>
    /// <param name="buffer">
    /// Specifies the buffer to clear. <paramref name="buffer"/> must be one of <see cref="GL_COLOR"/>,
    /// <see cref="GL_DEPTH"/>, <see cref="GL_STENCIL"/>.
    /// </param>
    /// <param name="drawbuffer">Specify a partical draw buffer to clear.</param>
    /// <param name="value">Specifies the value to clear the buffer to.</param>
    unsafe void ClearBufferiv( int buffer, int drawbuffer, int* value );

    /// <summary>
    /// Clear a buffer to an integer value. Refer to <see href="https://docs.gl/gl4/glClearBuffer"/> for details on how to
    /// use this function.
    /// </summary>
    /// <param name="buffer">
    /// Specifies the buffer to clear. <paramref name="buffer"/> must be one of <see cref="GL_COLOR"/>,
    /// <see cref="GL_DEPTH"/>, <see cref="GL_STENCIL"/>.
    /// </param>
    /// <param name="drawbuffer">Specify a partical draw buffer to clear.</param>
    /// <param name="value">Specifies the value to clear the buffer to.</param>
    unsafe void ClearBufferiv( int buffer, int drawbuffer, int[] value );

    /// <summary>
    /// Clear a buffer to an unsigned integer value. Refer to <see href="https://docs.gl/gl4/glClearBuffer"/> for details
    /// on how to use this function.
    /// </summary>
    /// <param name="buffer">
    /// Specifies the buffer to clear. <paramref name="buffer"/> must be one of <see cref="GL_COLOR"/>,
    /// <see cref="GL_DEPTH"/>, <see cref="GL_STENCIL"/>.
    /// </param>
    /// <param name="drawbuffer">Specify a partical draw buffer to clear.</param>
    /// <param name="value">Specifies the value to clear the buffer to.</param>
    unsafe void ClearBufferuiv( int buffer, int drawbuffer, uint* value );

    /// <summary>
    /// Clear a buffer to an unsigned integer value. Refer to <see href="https://docs.gl/gl4/glClearBuffer"/> for details
    /// on how to use this function.
    /// </summary>
    /// <param name="buffer">
    /// Specifies the buffer to clear. <paramref name="buffer"/> must be one of <see cref="GL_COLOR"/>,
    /// <see cref="GL_DEPTH"/>, <see cref="GL_STENCIL"/>.
    /// </param>
    /// <param name="drawbuffer">Specify a partical draw buffer to clear.</param>
    /// <param name="value">Specifies the value to clear the buffer to.</param>
    unsafe void ClearBufferuiv( int buffer, int drawbuffer, uint[] value );

    /// <summary>
    /// Clear a buffer to a floating point value. Refer to <see href="https://docs.gl/gl4/glClearBuffer"/> for details on
    /// how to use this function.
    /// </summary>
    /// <param name="buffer">
    /// Specifies the buffer to clear. <paramref name="buffer"/> must be one of <see cref="GL_COLOR"/>,
    /// <see cref="GL_DEPTH"/>, <see cref="GL_STENCIL"/>.
    /// </param>
    /// <param name="drawbuffer">Specify a partical draw buffer to clear.</param>
    /// <param name="value">Specifies the value to clear the buffer to.</param>
    unsafe void ClearBufferfv( int buffer, int drawbuffer, float* value );

    /// <summary>
    /// Clear a buffer to a floating point value. Refer to <see href="https://docs.gl/gl4/glClearBuffer"/> for details on
    /// how to use this function.
    /// </summary>
    /// <param name="buffer">
    /// Specifies the buffer to clear. <paramref name="buffer"/> must be one of <see cref="GL_COLOR"/>,
    /// <see cref="GL_DEPTH"/>, <see cref="GL_STENCIL"/>.
    /// </param>
    /// <param name="drawbuffer">Specify a partical draw buffer to clear.</param>
    /// <param name="value">Specifies the value to clear the buffer to.</param>
    unsafe void ClearBufferfv( int buffer, int drawbuffer, float[] value );

    /// <summary>
    /// Clear a buffer to a floating point value. Refer to <see href="https://docs.gl/gl4/glClearBuffer"/> for details on
    /// how to use this function.
    /// </summary>
    /// <param name="buffer">
    /// Specifies the buffer to clear. <paramref name="buffer"/> must be one of
    /// <see cref="GL_DEPTH_STENCIL"/>.
    /// </param>
    /// <param name="drawbuffer">Must be zero.</param>
    /// <param name="depth">Specifies the value to clear the depth buffer to.</param>
    /// <param name="stencil">Specifies the value to clear the stencil buffer to.</param>
    void ClearBufferfi( int buffer, int drawbuffer, float depth, int stencil );

    /// <summary>
    /// Returns a string describing the current  connection.
    /// </summary>
    /// <param name="name">
    /// Specifies a symbolic constant, one of <see cref="GL_VENDOR"/>, <see cref="GL_RENDERER"/>,
    /// <see cref="GL_VERSION"/>, <see cref="GL_SHADING_LANGUAGE_VERSION"/> or <see cref="GL_EXTENSIONS"/>.
    /// </param>
    /// <param name="index">Specifies the index of the string to return.</param>
    /// <returns>The requested string.</returns>
    unsafe Byte* GetStringi( int name, uint index );

    /// <summary>
    /// Returns a string describing the current  connection.
    /// </summary>
    /// <param name="name">
    /// Specifies a symbolic constant, one of <see cref="GL_VENDOR"/>, <see cref="GL_RENDERER"/>,
    /// <see cref="GL_VERSION"/>, <see cref="GL_SHADING_LANGUAGE_VERSION"/> or <see cref="GL_EXTENSIONS"/>.
    /// </param>
    /// <param name="index">Specifies the index of the string to return.</param>
    /// <returns>The requested string.</returns>
    unsafe string GetStringiSafe( int name, uint index );

    /// <summary>
    /// Determine if a name corresponds to a renderbuffer object.
    /// </summary>
    /// <param name="renderbuffer">Specifies a value that may be the name of a renderbuffer object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="renderbuffer"/> is the name of a renderbuffer object.
    /// <see langword="false"/> otherwise.
    /// </returns>
    bool IsRenderbuffer( uint renderbuffer );

    /// <summary>
    /// Bind a named renderbuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the renderbuffer target of the binding operation. <paramref name="target"/> must be
    /// <see cref="GL_RENDERBUFFER"/>.
    /// </param>
    /// <param name="renderbuffer">Specifies the name of the renderbuffer object to bind.</param>
    void BindRenderbuffer( int target, uint renderbuffer );

    /// <summary>
    /// Delete named renderbuffer objects.
    /// </summary>
    /// <param name="n">Specifies the number of renderbuffer objects to be deleted.</param>
    /// <param name="renderbuffers">
    /// Specifies an array of <paramref name="n"/> values, each of which contains a renderbuffer
    /// object name to be deleted.
    /// </param>
    unsafe void DeleteRenderbuffers( int n, uint* renderbuffers );

    /// <summary>
    /// Delete named renderbuffer objects.
    /// </summary>
    /// <param name="renderbuffers">Specifies an array of renderbuffer object names to be deleted.</param>
    unsafe void DeleteRenderbuffers( params uint[] renderbuffers );

    /// <summary>
    /// Generate renderbuffer object names.
    /// </summary>
    /// <param name="n">Specifies the number of renderbuffer object names to generate.</param>
    /// <param name="renderbuffers">Specifies an array in which the generated renderbuffer object names are to be stored.</param>
    unsafe void GenRenderbuffers( int n, uint* renderbuffers );

    /// <summary>
    /// Generate renderbuffer object names.
    /// </summary>
    /// <param name="n">Specifies the number of renderbuffer object names to generate.</param>
    /// <returns>Array of generated renderbuffer object names.</returns>
    unsafe uint[] GenRenderbuffers( int n );

    /// <summary>
    /// Generate a single renderbuffer object name.
    /// </summary>
    /// <returns>The generated renderbuffer object name.</returns>
    uint GenRenderbuffer();

    /// <summary>
    /// Establish data storage, format and dimensions of a renderbuffer object's image.
    /// </summary>
    /// <param name="target">
    /// Specifies a binding to which the target of the allocation and must be
    /// <see cref="GL_RENDERBUFFER"/>.
    /// </param>
    /// <param name="internalformat">Specifies the internal format to use for the renderbuffer object's image.</param>
    /// <param name="width">Specifies the width of the renderbuffer, in pixels.</param>
    /// <param name="height">Specifies the height of the renderbuffer, in pixels.</param>
    void RenderbufferStorage( int target, int internalformat, int width, int height );

    /// <summary>
    /// Return renderbuffer object parameter values.
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the query operation. <paramref name="target"/> must be
    /// <see cref="GL_RENDERBUFFER"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a renderbuffer object parameter. <paramref name="pname"/> can be
    /// one of the following: <see cref="GL_RENDERBUFFER_WIDTH"/>, <see cref="GL_RENDERBUFFER_HEIGHT"/>,
    /// <see cref="GL_RENDERBUFFER_INTERNAL_FORMAT"/>, <see cref="GL_RENDERBUFFER_SAMPLES"/>,
    /// <see cref="GL_RENDERBUFFER_RED_SIZE"/>, <see cref="GL_RENDERBUFFER_GREEN_SIZE"/>,
    /// <see cref="GL_RENDERBUFFER_BLUE_SIZE"/>, <see cref="GL_RENDERBUFFER_ALPHA_SIZE"/>,
    /// <see cref="GL_RENDERBUFFER_DEPTH_SIZE"/>, or <see cref="GL_RENDERBUFFER_STENCIL_SIZE"/>.
    /// </param>
    /// <param name="parameters">Specifies the address of a variable to receive the value of the queried parameter.</param>
    unsafe void GetRenderbufferParameteriv( int target, int pname, int* parameters );

    /// <summary>
    /// Return renderbuffer object parameter values.
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the query operation. <paramref name="target"/> must be
    /// <see cref="GL_RENDERBUFFER"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a renderbuffer object parameter. <paramref name="pname"/> can be
    /// one of the following: <see cref="GL_RENDERBUFFER_WIDTH"/>, <see cref="GL_RENDERBUFFER_HEIGHT"/>,
    /// <see cref="GL_RENDERBUFFER_INTERNAL_FORMAT"/>, <see cref="GL_RENDERBUFFER_SAMPLES"/>,
    /// <see cref="GL_RENDERBUFFER_RED_SIZE"/>, <see cref="GL_RENDERBUFFER_GREEN_SIZE"/>,
    /// <see cref="GL_RENDERBUFFER_BLUE_SIZE"/>, <see cref="GL_RENDERBUFFER_ALPHA_SIZE"/>,
    /// <see cref="GL_RENDERBUFFER_DEPTH_SIZE"/>, or <see cref="GL_RENDERBUFFER_STENCIL_SIZE"/>.
    /// </param>
    /// <param name="parameters">A <see langword="ref"/> to an array which will receive the value of the queried parameter.</param>
    unsafe void GetRenderbufferParameteriv( int target, int pname, ref int[] parameters );

    /// <summary>
    /// Determine if a name corresponds to a framebuffer object.
    /// </summary>
    /// <param name="framebuffer">Specifies a value that may be the name of a framebuffer object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="framebuffer"/> is the name of a framebuffer object.
    /// <see langword="false"/> otherwise.
    /// </returns>
    bool IsFramebuffer( uint framebuffer );

    /// <summary>
    /// Bind a named framebuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the bind operation. <paramref name="target"/> must be one of
    /// <see cref="GL_FRAMEBUFFER"/>, <see cref="GL_DRAW_FRAMEBUFFER"/> or <see cref="GL_READ_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="framebuffer">Specifies the name of a framebuffer object.</param>
    void BindFramebuffer( int target, uint framebuffer );

    /// <summary>
    /// Delete named framebuffer objects.
    /// </summary>
    /// <param name="n">Specifies the number of framebuffer objects to be deleted.</param>
    /// <param name="framebuffers">Specifies an array of framebuffer objects to be deleted.</param>
    unsafe void DeleteFramebuffers( int n, uint* framebuffers );

    /// <summary>
    /// Delete named framebuffer objects.
    /// </summary>
    /// <param name="framebuffers">Specifies an array of framebuffer objects to be deleted.</param>
    unsafe void DeleteFramebuffers( params uint[] framebuffers );

    /// <summary>
    /// Generate framebuffer object names.
    /// </summary>
    /// <param name="n">Specifies the number of framebuffer object names to generate.</param>
    /// <param name="framebuffers">Specifies an array in which the generated framebuffer object names are stored.</param>
    unsafe void GenFramebuffers( int n, uint* framebuffers );

    /// <summary>
    /// Generate framebuffer object names.
    /// </summary>
    /// <param name="n">Specifies the number of framebuffer object names to generate.</param>
    /// <returns>An array in which the generated framebuffer object names are stored.</returns>
    unsafe uint[] GenFramebuffers( int n );

    /// <summary>
    /// Generate a single framebuffer object name.
    /// </summary>
    /// <returns>The generated framebuffer object name.</returns>
    uint GenFramebuffer();

    /// <summary>
    /// Check the completeness status of a framebuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the framebuffer completeness check. <paramref name="target"/> must be one
    /// of <see cref="GL_FRAMEBUFFER"/>, <see cref="GL_DRAW_FRAMEBUFFER"/> or <see cref="GL_READ_FRAMEBUFFER"/>.
    /// </param>
    /// <returns>
    /// The framebuffer completeness status of <paramref name="target"/>. Refer to
    /// <see href="https://docs.gl/gl4/glCheckFramebufferStatus"/> for a list of all possible values.
    /// </returns>
    int CheckFramebufferStatus( int target );

    /// <summary>
    /// Attach a level of a 1D texture object as a logical buffer to the currently bound framebuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the framebuffer target. <paramref name="target"/> must be one of
    /// <see cref="GL_FRAMEBUFFER"/>, <see cref="GL_DRAW_FRAMEBUFFER"/> or <see cref="GL_READ_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="attachment">
    /// Specifies the attachment point of the framebuffer. One of <see cref="GL_COLOR_ATTACHMENT0"/>
    /// through <see cref="GL_COLOR_ATTACHMENT31"/>, <see cref="GL_DEPTH_ATTACHMENT"/>,
    /// <see cref="GL_STENCIL_ATTACHMENT"/>, <see cref="GL_DEPTH_STENCIL_ATTACHMENT"/>.
    /// </param>
    /// <param name="textarget">Specifies the type of texture.</param>
    /// <param name="texture">Specifies the name of an existing 1D texture object.</param>
    /// <param name="level">Specifies the mipmap level of the texture object to attach.</param>
    void FramebufferTexture1D( int target, int attachment, int textarget, uint texture, int level );

    /// <summary>
    /// Attach a level of a 2D texture object as a logical buffer to the currently bound framebuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the framebuffer target. <paramref name="target"/> must be one of
    /// <see cref="GL_FRAMEBUFFER"/>, <see cref="GL_DRAW_FRAMEBUFFER"/> or <see cref="GL_READ_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="attachment">
    /// Specifies the attachment point of the framebuffer. One of <see cref="GL_COLOR_ATTACHMENT0"/>
    /// through <see cref="GL_COLOR_ATTACHMENT31"/>, <see cref="GL_DEPTH_ATTACHMENT"/>,
    /// <see cref="GL_STENCIL_ATTACHMENT"/>, <see cref="GL_DEPTH_STENCIL_ATTACHMENT"/>.
    /// </param>
    /// <param name="textarget">Specifies the type of texture.</param>
    /// <param name="texture">Specifies the name of an existing 2D texture object.</param>
    /// <param name="level">Specifies the mipmap level of the texture object to attach.</param>
    void FramebufferTexture2D( int target, int attachment, int textarget, uint texture, int level );

    /// <summary>
    /// Attach a level of a 3D texture object as a logical buffer to the currently bound framebuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the framebuffer target. <paramref name="target"/> must be one of
    /// <see cref="GL_FRAMEBUFFER"/>, <see cref="GL_DRAW_FRAMEBUFFER"/> or <see cref="GL_READ_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="attachment">
    /// Specifies the attachment point of the framebuffer. One of <see cref="GL_COLOR_ATTACHMENT0"/>
    /// through <see cref="GL_COLOR_ATTACHMENT31"/>, <see cref="GL_DEPTH_ATTACHMENT"/>,
    /// <see cref="GL_STENCIL_ATTACHMENT"/>, <see cref="GL_DEPTH_STENCIL_ATTACHMENT"/>.
    /// </param>
    /// <param name="textarget">Specifies the type of texture.</param>
    /// <param name="texture">Specifies the name of an existing 3D texture object.</param>
    /// <param name="level">Specifies the mipmap level of the texture object to attach.</param>
    /// <param name="zoffset">Specifies the zoffset texel to be used as the framebuffer attachment point.</param>
    void FramebufferTexture3D( int target, int attachment, int textarget, uint texture, int level, int zoffset );

    /// <summary>
    /// Attach a renderbuffer as a logical buffer to the currently bound framebuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the framebuffer target. <paramref name="target"/> must be one of
    /// <see cref="GL_FRAMEBUFFER"/>, <see cref="GL_DRAW_FRAMEBUFFER"/> or <see cref="GL_READ_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="attachment">
    /// Specifies the attachment point of the framebuffer. One of <see cref="GL_COLOR_ATTACHMENT0"/>
    /// through <see cref="GL_COLOR_ATTACHMENT31"/>, <see cref="GL_DEPTH_ATTACHMENT"/>,
    /// <see cref="GL_STENCIL_ATTACHMENT"/>, <see cref="GL_DEPTH_STENCIL_ATTACHMENT"/>.
    /// </param>
    /// <param name="renderbuffertarget">
    /// Specifies the renderbuffer target. <paramref name="renderbuffertarget"/> must be
    /// <see cref="GL_RENDERBUFFER"/>.
    /// </param>
    /// <param name="renderbuffer">
    /// Specifies the name of an existing renderbuffer object of type
    /// <paramref name="renderbuffertarget"/>.
    /// </param>
    void FramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, uint renderbuffer );

    /// <summary>
    /// Return parameters of a framebuffer attachment.
    /// </summary>
    /// <param name="target">
    /// Specifies the framebuffer target. <paramref name="target"/> must be one of
    /// <see cref="GL_FRAMEBUFFER"/>, <see cref="GL_DRAW_FRAMEBUFFER"/> or <see cref="GL_READ_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="attachment">
    /// Specifies the attachment point of the framebuffer. One of <see cref="GL_COLOR_ATTACHMENT0"/>
    /// through <see cref="GL_COLOR_ATTACHMENT31"/>, <see cref="GL_DEPTH_ATTACHMENT"/>,
    /// <see cref="GL_STENCIL_ATTACHMENT"/>, <see cref="GL_DEPTH_STENCIL_ATTACHMENT"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the parameter of <paramref name="attachment"/> to query. Refer to
    /// <see href="https://docs.gl/gl4/glGetFramebufferAttachmentParameter"/> for details.
    /// </param>
    /// <param name="parameters">Specifies the address of a variable to receive the value of the queried parameter.</param>
    unsafe void GetFramebufferAttachmentParameteriv( int target, int attachment, int pname, int* parameters );

    /// <summary>
    /// Return parameters of a framebuffer attachment.
    /// </summary>
    /// <param name="target">
    /// Specifies the framebuffer target. <paramref name="target"/> must be one of
    /// <see cref="GL_FRAMEBUFFER"/>, <see cref="GL_DRAW_FRAMEBUFFER"/> or <see cref="GL_READ_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="attachment">
    /// Specifies the attachment point of the framebuffer. One of <see cref="GL_COLOR_ATTACHMENT0"/>
    /// through <see cref="GL_COLOR_ATTACHMENT31"/>, <see cref="GL_DEPTH_ATTACHMENT"/>,
    /// <see cref="GL_STENCIL_ATTACHMENT"/>, <see cref="GL_DEPTH_STENCIL_ATTACHMENT"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the parameter of <paramref name="attachment"/> to query. Refer to
    /// <see href="https://docs.gl/gl4/glGetFramebufferAttachmentParameter"/> for details.
    /// </param>
    /// <param name="parameters">A <see langword="ref"/> to an array which will receive the returned value(s).</param>
    unsafe void GetFramebufferAttachmentParameteriv( int target, int attachment, int pname, ref int[] parameters );

    /// <summary>
    /// Generate mipmaps for a specified texture object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture whose mimaps to generate is bound.
    /// <paramref name="target"/> must be one of <see cref="GL_TEXTURE_1D"/>, <see cref="GL_TEXTURE_2D"/>,
    /// <see cref="GL_TEXTURE_3D"/>, <see cref="GL_TEXTURE_1D_ARRAY"/>, <see cref="GL_TEXTURE_2D_ARRAY"/>,
    /// <see cref="GL_TEXTURE_CUBE_MAP"/>, <see cref="GL_TEXTURE_CUBE_MAP_ARRAY"/>.
    /// </param>
    void GenerateMipmap( int target );

    /// <summary>
    /// Copy a block of pixels from the read framebuffer to the draw framebuffer.
    /// </summary>
    /// <param name="srcX0">Specify the left pixel coordinate of the source rectangle.</param>
    /// <param name="srcY0">Specify the bottom pixel coordinate of the source rectangle.</param>
    /// <param name="srcX1">Specify the right pixel coordinate of the source rectangle.</param>
    /// <param name="srcY1">Specify the top pixel coordinate of the source rectangle.</param>
    /// <param name="dstX0">Specify the left pixel coordinate of the destination rectangle.</param>
    /// <param name="dstY0">Specify the bottom pixel coordinate of the destination rectangle.</param>
    /// <param name="dstX1">Specify the right pixel coordinate of the destination rectangle.</param>
    /// <param name="dstY1">Specify the top pixel coordinate of the destination rectangle.</param>
    /// <param name="mask">
    /// Specifies the bitwise OR of the flags indicating which buffers are to be copied. The allowed flags
    /// are <see cref="GL_COLOR_BUFFER_BIT"/>, <see cref="GL_DEPTH_BUFFER_BIT"/> and <see cref="GL_STENCIL_BUFFER_BIT"/>
    /// .
    /// </param>
    /// <param name="filter">
    /// Specifies the interpolation to be applied if the image is stretched. Must be one of
    /// <see cref="GL_NEAREST"/> or <see cref="GL_LINEAR"/>.
    /// </param>
    void BlitFramebuffer( int srcX0,
                            int srcY0,
                            int srcX1,
                            int srcY1,
                            int dstX0,
                            int dstY0,
                            int dstX1,
                            int dstY1,
                            uint mask,
                            int filter );

    /// <summary>
    /// Establish data storage, format and dimensions of a renderbuffer object's image.
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the renderbuffer object is bound for storage. Must be
    /// <see cref="GL_RENDERBUFFER"/>.
    /// </param>
    /// <param name="samples">Specifies the number of samples to be used for the renderbuffer object's storage.</param>
    /// <param name="internalformat">Specifies the internal format to be used for the renderbuffer object's image.</param>
    /// <param name="width">Specifies the width of the renderbuffer, in pixels.</param>
    /// <param name="height">Specifies the height of the renderbuffer, in pixels.</param>
    void RenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height );

    /// <summary>
    /// Attach a single layer of a texture object as a logical buffer to the currently bound framebuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the texture should be attached. Must be
    /// <see cref="GL_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="attachment">Specifies the attachment point of the framebuffer.</param>
    /// <param name="texture">Specifies the name of an existing texture object.</param>
    /// <param name="level">Specifies the mipmap level of the texture image to be attached.</param>
    /// <param name="layer">
    /// Specifies the layer of a 3D texture that is to be attached, if any. Must be a number in the range 0
    /// to the value of <see cref="GL_MAX_ARRAY_TEXTURE_LAYERS"/> minus 1.
    /// </param>
    void FramebufferTextureLayer( int target, int attachment, uint texture, int level, int layer );

    /// <summary>
    /// Map all or part of a buffer object's data store into the client's address space.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object being mapped. The symbolic constant must be
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="offset">Specifies the starting offset within the buffer of the range to be mapped.</param>
    /// <param name="length">Specifies the length of the range to be mapped.</param>
    /// <param name="access">Specifies a combination of access flags indicating the desired access to the range.</param>
    /// <returns>Returns a pointer to the beginning of the mapped range.</returns>
    unsafe void* MapBufferRange( int target, int offset, int length, uint access );

    /// <summary>
    /// Map all or part of a buffer object's data store into the client's address space.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object being mapped. The symbolic constant must be
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="offset">Specifies the starting offset within the buffer of the range to be mapped.</param>
    /// <param name="length">Specifies the length of the range to be mapped.</param>
    /// <param name="access">Specifies a combination of access flags indicating the desired access to the range.</param>
    /// <returns>Returns a type-safe and memory-safe <see cref="System.Span{T}"/> of the entire mapped memory.</returns>
    unsafe Span< T > MapBufferRange< T >( int target, int offset, int length, uint access ) where T : unmanaged;

    /// <summary>
    /// Invalidate portions of the buffer object's data store.
    /// </summary>
    /// <param name="target">
    /// Specifies the target buffer object whose data store is to be flushed and/or invalidated. The
    /// symbolic constant must be <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>,
    /// <see cref="GL_COPY_READ_BUFFER"/>, <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>, <see cref="GL_DRAW_INDIRECT_BUFFER"/>,
    /// <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>, <see cref="GL_PIXEL_UNPACK_BUFFER"/>,
    /// <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>, <see cref="GL_TEXTURE_BUFFER"/>,
    /// <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/> or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="offset">
    /// Specifies the offset within the buffer object's data store of the first byte to be flushed and/or
    /// invalidated.
    /// </param>
    /// <param name="length">Specifies the length of the range of bytes to be flushed and/or invalidated.</param>
    void FlushMappedBufferRange( int target, int offset, int length );

    /// <summary>
    /// Bind a vertex array object.
    /// </summary>
    /// <param name="array">Specifies the name of the vertex array to bind.</param>
    void BindVertexArray( uint array );

    /// <summary>
    /// Delete vertex array objects.
    /// </summary>
    /// <param name="n">Specifies the number of vertex array objects to be deleted.</param>
    /// <param name="arrays">Specifies an array of <paramref name="n"/> names of vertex array objects to be deleted.</param>
    unsafe void DeleteVertexArrays( int n, uint* arrays );

    /// <summary>
    /// Delete vertex array objects.
    /// </summary>
    /// <param name="arrays">Specifies an array of vertex array objects to be deleted.</param>
    unsafe void DeleteVertexArrays( params uint[] arrays );

    /// <summary>
    /// Generate vertex array object names.
    /// </summary>
    /// <param name="n">Specifies the number of vertex array object names to generate.</param>
    /// <param name="arrays">Specifies an array in which the generated vertex array object names are stored.</param>
    unsafe void GenVertexArrays( int n, uint* arrays );

    /// <summary>
    /// Generate vertex array object names.
    /// </summary>
    /// <param name="n">Specifies the number of vertex array object names to generate.</param>
    /// <returns>Returns an array of <paramref name="n"/> generated vertex array object names.</returns>
    unsafe uint[] GenVertexArrays( int n );

    /// <summary>
    /// Generate a single vertex array object name.
    /// </summary>
    /// <returns>Returns a generated vertex array object name.</returns>
    unsafe uint GenVertexArray();

    /// <summary>
    /// Determine if a name corresponds to a vertex array object.
    /// </summary>
    /// <param name="array">Specifies a value that may be the name of a vertex array object.</param>
    /// <returns>
    /// Returns <see langword="true"/> if <paramref name="array"/> is the name of a vertex array object. Otherwise,
    /// returns <see langword="false"/>.
    /// </returns>
    bool IsVertexArray( uint array );

    /// <summary>
    /// Draw multiple instances of a range of elements
    /// </summary>
    /// <param mode="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param first="first">Specifies the starting index in the enabled arrays.</param>
    /// <param count="count">Specifies the number of indices to be rendered.</param>
    /// <param instancecount="instancecount">
    /// Specifies the number of instances of the specified range of indices to be
    /// rendered.
    /// </param>
    void DrawArraysInstanced( int mode, int first, int count, int instancecount );

    /// <summary>
    /// Draw multiple instances of a set of elements
    /// </summary>
    /// <param mode="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param count="count">Specifies the number of elements to be rendered.</param>
    /// <param type="type">
    /// Specifies the type of the values in indices. Must be one of <see cref="GL_UNSIGNED_BYTE"/>,
    /// <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param indices="indices">Specifies a pointer to the location where the indices are stored.</param>
    /// <param instancecount="instancecount">
    /// Specifies the number of instances of the specified range of indices to be
    /// rendered.
    /// </param>
    unsafe void DrawElementsInstanced( int mode, int count, int type, void* indices, int instancecount );

    /// <summary>
    /// Draw multiple instances of a set of elements
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in indices. Must be one of <see cref="GL_UNSIGNED_BYTE"/>,
    /// <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">
    /// Specifies an array containin the indices. Make sure to match <typeparamref name="T"/> to the
    /// type of the indices.
    /// </param>
    /// <param name="instancecount">Specifies the number of instances of the specified range of indices to be rendered.</param>
    unsafe void DrawElementsInstanced< T >( int mode, int count, int type, T[] indices, int instancecount )
        where T : unmanaged, IUnsignedNumber< T >;

    /// <summary>
    /// Attach a buffer object's data store to a buffer texture object
    /// </summary>
    /// <param name="target">
    /// Specifies the target to which the buffer object's data store is attached for the purposes of the
    /// specified buffer texture object. target​ must be <see cref="GL_TEXTURE_BUFFER"/>.
    /// </param>
    /// <param name="internalformat">Specifies the internal format of the data in the store belonging to buffer.</param>
    /// <param name="buffer">
    /// Specifies the name of an existing buffer object whose storage to attach to the specified buffer
    /// texture object.
    /// </param>
    void TexBuffer( int target, int internalformat, uint buffer );

    /// <summary>
    /// Specify the primitive restart index
    /// </summary>
    /// <param name="index">Specifies the value to be interpreted as the primitive restart index.</param>
    void PrimitiveRestartIndex( uint index );

    /// <summary>
    /// Copy part of a buffer object's data store to the the data store of another buffer object.
    /// </summary>
    /// <param name="readTarget">
    /// Specifies the target from which the data will be copied. The symbolic constant must be
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DRAW_INDIRECT_BUFFER"/>, <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>
    /// , <see cref="GL_PIXEL_UNPACK_BUFFER"/>, <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/>, or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="writeTarget">
    /// Specifies the target to which the data will be copied. The symbolic constant must be
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DRAW_INDIRECT_BUFFER"/>, <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>
    /// , <see cref="GL_PIXEL_UNPACK_BUFFER"/>, <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/>, or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="readOffset">Specifies the offset into the data store of the buffer object containing the data to copy.</param>
    /// <param name="writeOffset">Specifies the offset into the data store of the buffer object into which data will be copied.</param>
    /// <param name="size">Specifies the size in bytes of the data to be copied.</param>
    void CopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size );

    /// <summary>
    /// Retrieve the indices of a number of uniforms within a program object
    /// </summary>
    /// <param name="program">Specifies the name of a program containing uniforms whose indices to retrieve.</param>
    /// <param name="uniformCount">Specifies the number of uniforms whose indices to retrieve.</param>
    /// <param name="uniformNames">Specifies an array of pointers to strings containing the names of the queried uniforms.</param>
    /// <param name="uniformIndices">Specifies an array to receive the indices of the uniforms specified in uniformNames.</param>
    unsafe void GetUniformIndices( uint program, int uniformCount, Byte** uniformNames, uint* uniformIndices );

    /// <summary>
    /// Retrieve the indices of a number of uniforms within a program object
    /// </summary>
    /// <param name="program">Specifies the name of a program containing uniforms whose indices to retrieve.</param>
    /// <param name="uniformNames">Specifies an array of strings containing the names of the queried uniforms.</param>
    /// <returns>An array of indices of the uniforms specified in uniformNames.</returns>
    unsafe uint[] GetUniformIndices( uint program, params string[] uniformNames );

    /// <summary>
    /// Returns information about several active uniform variables for the specified program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformCount">Specifies the number of elements in the array of indices <paramref name="uniformIndices"/>.</param>
    /// <param name="uniformIndices">
    /// Specifies an array of <paramref name="uniformCount"/> integers containing the indices of
    /// the uniform variables to be queried.
    /// </param>
    /// <param name="pname">
    /// Specifies the information to be queried about each uniform variable specified in
    /// <paramref name="uniformIndices"/>.
    /// </param>
    /// <param name="parameters">
    /// Specifies an array of <paramref name="uniformCount"/> integers to receive the information
    /// requested about each uniform variable specified in <paramref name="uniformIndices"/>.
    /// </param>
    unsafe void GetActiveUniformsiv( uint program, int uniformCount, uint* uniformIndices, int pname, int* parameters );

    /// <summary>
    /// Returns information about several active uniform variables for the specified program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="pname">
    /// Specifies the information to be queried about each uniform variable specified in
    /// <paramref name="uniformIndices"/>.
    /// </param>
    /// <param name="uniformIndices">
    /// Specifies an array of integers containing the indices of the uniform variables to be
    /// queried.
    /// </param>
    /// <returns>
    /// An array of integers to receive the information requested about each uniform variable specified in
    /// <paramref name="uniformIndices"/>.
    /// </returns>
    unsafe int[] GetActiveUniformsiv( uint program, int pname, params uint[] uniformIndices );

    /// <summary>
    /// Returns the name of an active uniform variable at the specified index within a program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformIndex">Specifies the index of the uniform variable to be queried.</param>
    /// <param name="bufSize">
    /// Specifies the size of the buffer whose address is specified by <paramref name="uniformName"/>,
    /// in characters.
    /// </param>
    /// <param name="length">
    /// Returns the number of characters actually written into the buffer indicated by
    /// <paramref name="uniformName"/>.
    /// </param>
    /// <param name="uniformName">
    /// Returns the name of the uniform variable at the specified index in the program object
    /// specified by <paramref name="program"/>.
    /// </param>
    unsafe void GetActiveUniformName( uint program, uint uniformIndex, int bufSize, int* length, Byte* uniformName );

    /// <summary>
    /// Returns the name of an active uniform variable at the specified index within a program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformIndex">Specifies the index of the uniform variable to be queried.</param>
    /// <param name="bufSize">
    /// Specifies the size of the buffer whose address is specified by <paramref name="uniformName"/>,
    /// in characters.
    /// </param>
    /// <returns>
    /// The name of the uniform variable at the specified index in the program object specified by
    /// <paramref name="program"/>, in the correct size.
    /// </returns>
    unsafe string GetActiveUniformName( uint program, uint uniformIndex, int bufSize );

    /// <summary>
    /// Returns the index of a uniform block within a program
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformBlockName">
    /// Points to a null terminated string containing the name of the uniform block whose index
    /// to query.
    /// </param>
    /// <returns>
    /// The index of the uniform block named <paramref name="uniformBlockName"/> within the program object
    /// <paramref name="program"/>.
    /// </returns>
    unsafe uint GetUniformBlockIndex( uint program, Byte* uniformBlockName );

    /// <summary>
    /// Returns the index of a uniform block within a program
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformBlockName">Contains the name of the uniform block whose index to query.</param>
    unsafe uint GetUniformBlockIndex( uint program, string uniformBlockName );

    /// <summary>
    /// Returns information about an active uniform block
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformBlockIndex">
    /// Specifies the index of the uniform block within <paramref name="program"/> whose
    /// information to query.
    /// </param>
    /// <param name="pname">Specifies the specific information to query about the active uniform block.</param>
    /// <param name="parameters">Returns the requested information about the uniform block.</param>
    unsafe void GetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, int* parameters );

    /// <summary>
    /// Returns information about an active uniform block
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformBlockIndex">
    /// Specifies the index of the uniform block within <paramref name="program"/> whose
    /// information to query.
    /// </param>
    /// <param name="pname">Specifies the specific information to query about the active uniform block.</param>
    /// <param name="parameters">Returns the requested information about the uniform block.</param>
    unsafe void GetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, ref int[] parameters );

    /// <summary>
    /// Returns the name of an active uniform block at the specified index within a program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformBlockIndex">
    /// Specifies the index of the uniform block within <paramref name="program"/> whose name
    /// to retrieve.
    /// </param>
    /// <param name="bufSize">
    /// Specifies the size of the buffer whose address is specified by
    /// <paramref name="uniformBlockName"/>, in characters.
    /// </param>
    /// <param name="length">Returns the length of the uniform block name.</param>
    /// <param name="uniformBlockName">
    /// Returns the name of the uniform block at the specified index in the program object
    /// specified by <paramref name="program"/>.
    /// </param>
    unsafe void GetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize, int* length, Byte* uniformBlockName );

    /// <summary>
    /// Returns the name of an active uniform block at the specified index within a program object
    /// </summary>
    /// <param name="program">Specifies the program object to be queried.</param>
    /// <param name="uniformBlockIndex">
    /// Specifies the index of the uniform block within <paramref name="program"/> whose name
    /// to retrieve.
    /// </param>
    /// <param name="bufSize">Specifies a maximum amount of characters OpenGL is allowed to write in the character buffer.</param>
    /// <param name="length">Returns the length of the uniform block name.</param>
    unsafe string GetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize );

    /// <summary>
    /// Assigns a binding point to an active uniform block
    /// </summary>
    /// <param name="program">Specifies the program object containing the active uniform block whose binding to assign.</param>
    /// <param name="uniformBlockIndex">
    /// Specifies the index of the active uniform block within <paramref name="program"/>
    /// whose binding to assign.
    /// </param>
    /// <param name="uniformBlockBinding">
    /// Specifies the binding point to which to bind the uniform block with index
    /// <paramref name="uniformBlockIndex"/> within the program object <paramref name="program"/>.
    /// </param>
    void UniformBlockBinding( uint program, uint uniformBlockIndex, uint uniformBlockBinding );

    /// <summary>
    /// Render primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
    /// <param name="basevertex">
    /// Specifies a constant that should be added to each element of <paramref name="indices"/> when
    /// choosing elements from the enabled vertex arrays.
    /// </param>
    unsafe void DrawElementsBaseVertex( int mode, int count, int type, void* indices, int basevertex );

    /// <summary>
    /// Render primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">
    /// Specifies an array of indices. Make sure to match the type <typeparamref name="T"/> with
    /// <paramref name="type"/>.
    /// </param>
    /// <param name="basevertex">
    /// Specifies a constant that should be added to each element of <paramref name="indices"/> when
    /// choosing elements from the enabled vertex arrays.
    /// </param>
    unsafe void DrawElementsBaseVertex< T >( int mode, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >;

    /// <summary>
    /// Render primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="start">Specifies the minimum array index contained in <paramref name="indices"/>.</param>
    /// <param name="end">Specifies the maximum array index contained in <paramref name="indices"/>.</param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
    /// <param name="basevertex">
    /// Specifies a constant that should be added to each element of <paramref name="indices"/> when
    /// choosing elements from the enabled vertex arrays.
    /// </param>
    unsafe void DrawRangeElementsBaseVertex( int mode, uint start, uint end, int count, int type, void* indices, int basevertex );

    /// <summary>
    /// Render primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="start">Specifies the minimum array index contained in <paramref name="indices"/>.</param>
    /// <param name="end">Specifies the maximum array index contained in <paramref name="indices"/>.</param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">
    /// Specifies an array of indices. Make sure to match the type <typeparamref name="T"/> with
    /// <paramref name="type"/>.
    /// </param>
    /// <param name="basevertex">
    /// Specifies a constant that should be added to each element of <paramref name="indices"/> when
    /// choosing elements from the enabled vertex arrays.
    /// </param>
    unsafe void DrawRangeElementsBaseVertex< T >( int mode, uint start, uint end, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >;

    /// <summary>
    /// Render multiple instances of a set of primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
    /// <param name="instancecount">Specifies the number of instances of the indexed geometry that should be drawn.</param>
    /// <param name="basevertex">
    /// Specifies a constant that should be added to each element of <paramref name="indices"/> when
    /// choosing elements from the enabled vertex arrays.
    /// </param>
    unsafe void DrawElementsInstancedBaseVertex( int mode, int count, int type, void* indices, int instancecount, int basevertex );

    /// <summary>
    /// Render multiple instances of a set of primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="count">Specifies the number of elements to be rendered.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">
    /// Specifies an array of indices. Make sure to match the type <typeparamref name="T"/> with
    /// <paramref name="type"/>.
    /// </param>
    /// <param name="instancecount">Specifies the number of instances of the indexed geometry that should be drawn.</param>
    /// <param name="basevertex">
    /// Specifies a constant that should be added to each element of <paramref name="indices"/> when
    /// choosing elements from the enabled vertex arrays.
    /// </param>
    unsafe void DrawElementsInstancedBaseVertex< T >( int mode, int count, int type, T[] indices, int instancecount, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >;

    /// <summary>
    /// Render multiple sets of primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="count">Specifies an array of the elements counts.</param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">Specifies an array of pointers to the location where the indices are stored.</param>
    /// <param name="drawcount">Specifies the size of the <paramref name="count"/> and <paramref name="basevertex"/> arrays.</param>
    /// <param name="basevertex">
    /// Specifies an array of the constants that should be added to each element of
    /// <paramref name="indices"/> when choosing elements from the enabled vertex arrays.
    /// </param>
    unsafe void MultiDrawElementsBaseVertex( int mode, int* count, int type, void** indices, int drawcount, int* basevertex );

    /// <summary>
    /// Render multiple sets of primitives from array data with a per-element offset.
    /// </summary>
    /// <param name="mode">
    /// Specifies what kind of primitives to render. Symbolic constants <see cref="GL_POINTS"/>,
    /// <see cref="GL_LINE_STRIP"/>, <see cref="GL_LINE_LOOP"/>, <see cref="GL_LINES"/>,
    /// <see cref="GL_TRIANGLE_STRIP"/>, <see cref="GL_TRIANGLE_FAN"/>, <see cref="GL_TRIANGLES"/>,
    /// <see cref="GL_LINES_ADJACENCY"/>, <see cref="GL_LINE_STRIP_ADJACENCY"/>, <see cref="GL_TRIANGLES_ADJACENCY"/>,
    /// <see cref="GL_TRIANGLE_STRIP_ADJACENCY"/> and <see cref="GL_PATCHES"/> are accepted.
    /// </param>
    /// <param name="type">
    /// Specifies the type of the values in <paramref name="indices"/>. Must be one of
    /// <see cref="GL_UNSIGNED_BYTE"/>, <see cref="GL_UNSIGNED_SHORT"/>, or <see cref="GL_UNSIGNED_INT"/>.
    /// </param>
    /// <param name="indices">
    /// Specifies an array of arrays of indices. Make sure to match the type <typeparamref name="T"/>
    /// with <paramref name="type"/>.
    /// </param>
    /// <param name="basevertex">
    /// Specifies an array of the constants that should be added to each element of
    /// <paramref name="indices"/> when choosing elements from the enabled vertex arrays.
    /// </param>
    unsafe void MultiDrawElementsBaseVertex< T >( int mode, int type, T[][] indices, int[] basevertex ) where T : unmanaged, IUnsignedNumber< T >;

    /// <summary>
    /// Specify the vertex to be used as the source of data for flat shaded varyings.
    /// </summary>
    /// <param name="mode">
    /// Specifies the vertex to be used as the source of data for flat shaded varyings. Must be
    /// <see cref="GL_FIRST_VERTEX_CONVENTION"/> or <see cref="GL_LAST_VERTEX_CONVENTION"/>.
    /// </param>
    void ProvokingVertex( int mode );

    /// <summary>
    /// Creates a new sync object and inserts it into the  command stream.
    /// </summary>
    /// <param name="condition">
    /// Specifies the condition that must be met to set the sync object's state to signaled. Must be
    /// <see cref="GL_SYNC_GPU_COMMANDS_COMPLETE"/>.
    /// </param>
    /// <param name="flags">
    /// Specifies a bitwise combination of flags controlling the behavior of the sync object. No flags are
    /// presently defined for this operation and <paramref name="flags"/> must be zero.
    /// </param>
    /// <returns>The sync object.</returns>
    unsafe void* FenceSync( int condition, uint flags );

    /// <summary>
    /// Creates a new sync object and inserts it into the  command stream.
    /// </summary>
    /// <param name="condition">
    /// Specifies the condition that must be met to set the sync object's state to signaled. Must be
    /// <see cref="GL_SYNC_GPU_COMMANDS_COMPLETE"/>.
    /// </param>
    /// <param name="flags">
    /// Specifies a bitwise combination of flags controlling the behavior of the sync object. No flags are
    /// presently defined for this operation and <paramref name="flags"/> must be zero.
    /// </param>
    /// <returns>The sync object.</returns>
    unsafe IntPtr FenceSyncSafe( int condition, uint flags );

    /// <summary>
    /// Determines if a name corresponds to a sync object.
    /// </summary>
    /// <param name="sync">Specifies a value that may be the name of a sync object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="sync"/> is a name of a sync object. Otherwise,
    /// <see langword="false"/>.
    /// </returns>
    unsafe bool IsSync( void* sync );

    /// <summary>
    /// Determines if a name corresponds to a sync object.
    /// </summary>
    /// <param name="sync">Specifies a value that may be the name of a sync object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="sync"/> is a name of a sync object. Otherwise,
    /// <see langword="false"/>.
    /// </returns>
    unsafe bool IsSyncSafe( IntPtr sync );

    /// <summary>
    /// Deletes a sync object.
    /// </summary>
    /// <param name="sync">Specifies the sync object to be deleted.</param>
    unsafe void DeleteSync( void* sync );

    /// <summary>
    /// Deletes a sync object.
    /// </summary>
    /// <param name="sync">Specifies the sync object to be deleted.</param>
    unsafe void DeleteSyncSafe( IntPtr sync );

    /// <summary>
    /// Causes the client to block and wait for a sync object to become signaled.
    /// </summary>
    /// <param name="sync">Specifies the sync object whose status to wait on.</param>
    /// <param name="flags">
    /// A bitfield controlling the command flushing behavior. <paramref name="flags"/> may be
    /// <see cref="GL_SYNC_FLUSH_COMMANDS_BIT"/> or zero.
    /// </param>
    /// <param name="timeout">
    /// The timeout, specified in nanoseconds, for which the implementation should wait for
    /// <paramref name="sync"/> to become signaled.
    /// </param>
    /// <returns>
    /// One of <see cref="GL_ALREADY_SIGNALED"/>, <see cref="GL_TIMEOUT_EXPIRED"/>,
    /// <see cref="GL_CONDITION_SATISFIED"/>, or <see cref="GL_WAIT_FAILED"/>.
    /// </returns>
    unsafe int ClientWaitSync( void* sync, uint flags, UInt64 timeout );

    /// <summary>
    /// Causes the client to block and wait for a sync object to become signaled.
    /// </summary>
    /// <param name="sync">Specifies the sync object whose status to wait on.</param>
    /// <param name="flags">
    /// A bitfield controlling the command flushing behavior. <paramref name="flags"/> may be
    /// <see cref="GL_SYNC_FLUSH_COMMANDS_BIT"/> or zero.
    /// </param>
    /// <param name="timeout">
    /// The timeout, specified in nanoseconds, for which the implementation should wait for
    /// <paramref name="sync"/> to become signaled.
    /// </param>
    /// <returns>
    /// One of <see cref="GL_ALREADY_SIGNALED"/>, <see cref="GL_TIMEOUT_EXPIRED"/>,
    /// <see cref="GL_CONDITION_SATISFIED"/>, or <see cref="GL_WAIT_FAILED"/>.
    /// </returns>
    unsafe int ClientWaitSyncSafe( IntPtr sync, uint flags, UInt64 timeout );

    /// <summary>
    /// Causes the server to block and wait for a sync object to become signaled.
    /// </summary>
    /// <param name="sync">Specifies the sync object whose status to wait on.</param>
    /// <param name="flags">A bitfield controlling the command flushing behavior. <paramref name="flags"/> must be zero.</param>
    /// <param name="timeout">
    /// The timeout, specified in nanoseconds, for which the implementation should wait for
    /// <paramref name="sync"/> to become signaled.
    /// </param>
    unsafe void WaitSync( void* sync, uint flags, UInt64 timeout );

    /// <summary>
    /// Causes the server to block and wait for a sync object to become signaled.
    /// </summary>
    /// <param name="sync">Specifies the sync object whose status to wait on.</param>
    /// <param name="flags">A bitfield controlling the command flushing behavior. <paramref name="flags"/> must be zero.</param>
    /// <param name="timeout">
    /// The timeout, specified in nanoseconds, for which the implementation should wait for
    /// <paramref name="sync"/> to become signaled.
    /// </param>
    unsafe void WaitSyncSafe( IntPtr sync, uint flags, UInt64 timeout );

    /// <summary>
    /// Returns the 64bit integer value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">Returns the value or values of the specified parameter.</param>
    unsafe void GetInteger64v( int pname, Int64* data );

    /// <summary>
    /// Returns the 64bit integer value or values of a selected parameter.
    /// </summary>
    /// <param name="pname">
    /// Specifies the parameter value to be returned. Refer to <see href="https://docs.gl/gl4/glGet"/> for
    /// a list of possible values.
    /// </param>
    /// <param name="data">Returns the value or values of the specified parameter.</param>
    unsafe void GetInteger64v( int pname, ref Int64[] data );

    /// <summary>
    /// Query the properties of a sync object.
    /// </summary>
    /// <param name="sync">Specifies the sync object whose properties to query.</param>
    /// <param name="pname">
    /// Specifies the parameter whose value to retrieve from the sync object indicated by
    /// <paramref name="sync"/>. Allowed values are <see cref="GL_OBJECT_TYPE"/>, <see cref="GL_SYNC_STATUS"/>,
    /// <see cref="GL_SYNC_CONDITION"/>, <see cref="GL_SYNC_FLAGS"/>.
    /// </param>
    /// <param name="bufSize">Specifies the size of the buffer whose address is given by <paramref name="values"/>.</param>
    /// <param name="length">Returns the number of integers placed in <paramref name="values"/>.</param>
    /// <param name="values">Returns the requested parameter.</param>
    unsafe void GetSynciv( void* sync, int pname, int bufSize, int* length, int* values );

    /// <summary>
    /// Query the properties of a sync object.
    /// </summary>
    /// <param name="sync">Specifies the sync object whose properties to query.</param>
    /// <param name="pname">
    /// Specifies the parameter whose value to retrieve from the sync object indicated by
    /// <paramref name="sync"/>. Allowed values are <see cref="GL_OBJECT_TYPE"/>, <see cref="GL_SYNC_STATUS"/>,
    /// <see cref="GL_SYNC_CONDITION"/>, <see cref="GL_SYNC_FLAGS"/>.
    /// </param>
    /// <param name="bufSize">Specifies the size of the buffer whose address is given by <paramref name="values"/>.</param>
    /// <returns>Returns the requested parameter(s).</returns>
    unsafe int[] GetSynciv( IntPtr sync, int pname, int bufSize );

    /// <summary>
    /// Returns the 64bit integer value or values of a selected parameter.
    /// </summary>
    /// <param name="target">
    /// Specifies the target of which the indexed parameter <paramref name="index"/> is to be returned.
    /// Refer to <see href="https://docs.gl/gl4/glGet"/> for a list of possible values.
    /// </param>
    /// <param name="index">Specifies the index of the value to be returned.</param>
    /// <param name="data">Returns the value or values of the specified parameter.</param>
    unsafe void GetInteger64i_v( int target, uint index, Int64* data );

    /// <summary>
    /// Returns the 64bit integer value or values of a selected parameter.
    /// </summary>
    /// <param name="target">
    /// Specifies the target of which the indexed parameter <paramref name="index"/> is to be returned.
    /// Refer to <see href="https://docs.gl/gl4/glGet"/> for a list of possible values.
    /// </param>
    /// <param name="index">Specifies the index of the value to be returned.</param>
    /// <param name="data">Returns the value or values of the specified parameter.</param>
    unsafe void GetInteger64i_v( int target, uint index, ref Int64[] data );

    /// <summary>
    /// Returns the value or values of a selected parameter.
    /// </summary>
    /// <param name="target">
    /// . Specifies the target to which the buffer object is bound. Must be one of
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DRAW_INDIRECT_BUFFER"/>, <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>
    /// , <see cref="GL_PIXEL_UNPACK_BUFFER"/>, <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/>, or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a buffer object parameter. Accepted values are
    /// <see cref="GL_BUFFER_ACCESS"/>, <see cref="GL_BUFFER_ACCESS_FLAGS"/>, <see cref="GL_BUFFER_IMMUTABLE_STORAGE"/>,
    /// <see cref="GL_BUFFER_MAPPED"/>, <see cref="GL_BUFFER_MAP_LENGTH"/>, <see cref="GL_BUFFER_MAP_OFFSET"/>,
    /// <see cref="GL_BUFFER_SIZE"/>, <see cref="GL_BUFFER_STORAGE_FLAGS"/>, <see cref="GL_BUFFER_USAGE"/>.
    /// </param>
    /// <param name="parameters">Returns the requested parameter.</param>
    unsafe void GetBufferParameteri64v( int target, int pname, Int64* parameters );

    /// <summary>
    /// Returns the value or values of a selected parameter.
    /// </summary>
    /// <param name="target">
    /// . Specifies the target to which the buffer object is bound. Must be one of
    /// <see cref="GL_ARRAY_BUFFER"/>, <see cref="GL_ATOMIC_COUNTER_BUFFER"/>, <see cref="GL_COPY_READ_BUFFER"/>,
    /// <see cref="GL_COPY_WRITE_BUFFER"/>, <see cref="GL_DISPATCH_INDIRECT_BUFFER"/>,
    /// <see cref="GL_DRAW_INDIRECT_BUFFER"/>, <see cref="GL_ELEMENT_ARRAY_BUFFER"/>, <see cref="GL_PIXEL_PACK_BUFFER"/>
    /// , <see cref="GL_PIXEL_UNPACK_BUFFER"/>, <see cref="GL_QUERY_BUFFER"/>, <see cref="GL_SHADER_STORAGE_BUFFER"/>,
    /// <see cref="GL_TEXTURE_BUFFER"/>, <see cref="GL_TRANSFORM_FEEDBACK_BUFFER"/>, or <see cref="GL_UNIFORM_BUFFER"/>.
    /// </param>
    /// <param name="pname">
    /// Specifies the symbolic name of a buffer object parameter. Accepted values are
    /// <see cref="GL_BUFFER_ACCESS"/>, <see cref="GL_BUFFER_ACCESS_FLAGS"/>, <see cref="GL_BUFFER_IMMUTABLE_STORAGE"/>,
    /// <see cref="GL_BUFFER_MAPPED"/>, <see cref="GL_BUFFER_MAP_LENGTH"/>, <see cref="GL_BUFFER_MAP_OFFSET"/>,
    /// <see cref="GL_BUFFER_SIZE"/>, <see cref="GL_BUFFER_STORAGE_FLAGS"/>, <see cref="GL_BUFFER_USAGE"/>.
    /// </param>
    /// <param name="parameters">Returns the requested parameter.</param>
    unsafe void GetBufferParameteri64v( int target, int pname, ref Int64[] parameters );

    /// <summary>
    /// Attaches a level of a texture object as a logical buffer to the currently bound framebuffer object.
    /// </summary>
    /// <param name="target">
    /// Specifies the framebuffer target. Must be <see cref="GL_DRAW_FRAMEBUFFER"/>,
    /// <see cref="GL_READ_FRAMEBUFFER"/> or <see cref="GL_FRAMEBUFFER"/>.
    /// </param>
    /// <param name="attachment">
    /// Specifies the attachment point of the framebuffer. Must be one of
    /// <see cref="GL_COLOR_ATTACHMENT0"/> through <see cref="GL_COLOR_ATTACHMENT31"/>,
    /// <see cref="GL_DEPTH_ATTACHMENT"/>, <see cref="GL_STENCIL_ATTACHMENT"/>,
    /// <see cref="GL_DEPTH_STENCIL_ATTACHMENT"/>.
    /// </param>
    /// <param name="texture">Specifies the texture object whose image is to be attached.</param>
    /// <param name="level">Specifies the mipmap level of the texture object to be attached.</param>
    void FramebufferTexture( int target, int attachment, uint texture, int level );

    /// <summary>
    /// Establishes the data storage, format, dimensions, and number of samples of a multisample texture's image.
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the operation. Must be <see cref="GL_TEXTURE_2D_MULTISAMPLE"/> or
    /// <see cref="GL_PROXY_TEXTURE_2D_MULTISAMPLE"/>.
    /// </param>
    /// <param name="samples">Specifies the number of samples in the multisample texture's image.</param>
    /// <param name="internalformat">Specifies the internal format to be used to store texture image data.</param>
    /// <param name="width">Specifies the width of the multisample texture's image, in texels.</param>
    /// <param name="height">Specifies the height of the multisample texture's image, in texels.</param>
    /// <param name="fixedsamplelocations">
    /// Specifies whether the image will use identical sample locations and the same number
    /// of samples for all texels in the image, and the sample locations will not depend on the internal format or size of
    /// the image.
    /// </param>
    void TexImage2DMultisample( int target,
                                  int samples,
                                  int internalformat,
                                  int width,
                                  int height,
                                  bool fixedsamplelocations );

    /// <summary>
    /// Establishes the data storage, format, dimensions, and number of samples of a multisample texture's image.
    /// </summary>
    /// <param name="target">
    /// Specifies the target of the operation. Must be <see cref="GL_TEXTURE_3D_MULTISAMPLE"/> or
    /// <see cref="GL_PROXY_TEXTURE_3D_MULTISAMPLE"/>.
    /// </param>
    /// <param name="samples">Specifies the number of samples in the multisample texture's image.</param>
    /// <param name="internalformat">Specifies the internal format to be used to store texture image data.</param>
    /// <param name="width">Specifies the width of the multisample texture's image, in texels.</param>
    /// <param name="height">Specifies the height of the multisample texture's image, in texels.</param>
    /// <param name="depth">Specifies the depth of the multisample texture's image, in texels.</param>
    /// <param name="fixedsamplelocations">
    /// Specifies whether the image will use identical sample locations and the same number
    /// of samples for all texels in the image, and the sample locations will not depend on the internal format or size of
    /// the image.
    /// </param>
    void TexImage3DMultisample( int target,
                                  int samples,
                                  int internalformat,
                                  int width,
                                  int height,
                                  int depth,
                                  bool fixedsamplelocations );

    /// <summary>
    /// Returns the location of a sample.
    /// </summary>
    /// <param name="pname">Specifies the sample parameter to query. Must be <see cref="GL_SAMPLE_POSITION"/>.</param>
    /// <param name="index">Specifies the index of the sample.</param>
    /// <param name="val">Specifies the address of an array to receive the location of the sample.</param>
    unsafe void GetMultisamplefv( int pname, uint index, float* val );

    /// <summary>
    /// Returns the location of a sample.
    /// </summary>
    /// <param name="pname">Specifies the sample parameter to query. Must be <see cref="GL_SAMPLE_POSITION"/>.</param>
    /// <param name="index">Specifies the index of the sample.</param>
    /// <param name="val">Specifies the address of an array to receive the location of the sample.</param>
    unsafe void GetMultisamplefvSafe( int pname, uint index, ref float[] val );

    /// <summary>
    /// Controls the writing of individual bits in a logical multisample color sample.
    /// </summary>
    /// <param name="maskNumber">Specifies which 32-bit sub-word of the sample mask to update.</param>
    /// <param name="mask">Specifies a mask to enable and disable sample coverage.</param>
    void SampleMaski( uint maskNumber, uint mask );

    /// <summary>
    /// Bind a user-defined varying out variable to a fragment shader color number and index.
    /// </summary>
    /// <param name="program">Specifies the name of the program containing varying out variable whose binding to modify.</param>
    /// <param name="colorNumber">Specifies the color number to bind the user-defined varying out variable to.</param>
    /// <param name="index">Specifies the index of the color number to bind the user-defined varying out variable to.</param>
    /// <param name="name">Specifies the name of the user-defined varying out variable whose binding to modify.</param>
    unsafe void BindFragDataLocationIndexed( uint program, uint colorNumber, uint index, Byte* name );

    /// <summary>
    /// Bind a user-defined varying out variable to a fragment shader color number and index.
    /// </summary>
    /// <param name="program">Specifies the name of the program containing varying out variable whose binding to modify.</param>
    /// <param name="colorNumber">Specifies the color number to bind the user-defined varying out variable to.</param>
    /// <param name="index">Specifies the index of the color number to bind the user-defined varying out variable to.</param>
    /// <param name="name">Specifies the name of the user-defined varying out variable whose binding to modify.</param>
    unsafe void BindFragDataLocationIndexed( uint program, uint colorNumber, uint index, string name );

    /// <summary>
    /// Return the index of a user-defined varying out variable.
    /// </summary>
    /// <param name="program">Specifies the name of the program containing varying out variable whose index to query.</param>
    /// <param name="name">Specifies the name of the user-defined varying out variable whose index to query.</param>
    /// <returns>The index of the user-defined varying out variable.</returns>
    unsafe int GetFragDataIndex( uint program, Byte* name );

    /// <summary>
    /// Return the index of a user-defined varying out variable.
    /// </summary>
    /// <param name="program">Specifies the name of the program containing varying out variable whose index to query.</param>
    /// <param name="name">Specifies the name of the user-defined varying out variable whose index to query.</param>
    /// <returns>The index of the user-defined varying out variable.</returns>
    unsafe int GetFragDataIndex( uint program, string name );

    /// <summary>
    /// Generate sampler object names.
    /// </summary>
    /// <param name="count">Specifies the number of sampler object names to generate.</param>
    /// <param name="samplers">Specifies an array in which the generated sampler object names are stored.</param>
    unsafe void GenSamplers( int count, uint* samplers );

    /// <summary>
    /// Generate sampler object names.
    /// </summary>
    /// <param name="count">Specifies the number of sampler object names to generate.</param>
    /// <returns>An array in which the generated sampler object names are stored.</returns>
    unsafe uint[] GenSamplers( int count );

    /// <summary>
    /// Generate a single sampler object name.
    /// </summary>
    /// <returns>The generated sampler object name.</returns>
    uint GenSampler();

    /// <summary>
    /// Delete named sampler objects.
    /// </summary>
    /// <param name="count">Specifies the number of sampler objects to be deleted.</param>
    /// <param name="samplers">Specifies an array of sampler objects to be deleted.</param>
    unsafe void DeleteSamplers( int count, uint* samplers );

    /// <summary>
    /// Delete named sampler objects.
    /// </summary>
    /// <param name="samplers">Specifies an array of sampler objects to be deleted.</param>
    unsafe void DeleteSamplers( params uint[] samplers );

    /// <summary>
    /// Determine if a name corresponds to a sampler object.
    /// </summary>
    /// <param name="sampler">Specifies a value that may be the name of a sampler object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="sampler"/> is a value generated by OpenGL; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    bool IsSampler( uint sampler );

    /// <summary>
    /// Bind a named sampler to a texturing unit.
    /// </summary>
    /// <param name="unit">Specifies the index of the texture unit to which the sampler is bound.</param>
    /// <param name="sampler">Specifies the name of a sampler.</param>
    void BindSampler( uint unit, uint sampler );

    /// <summary>
    /// Set the integer value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    void SamplerParameteri( uint sampler, int pname, int param );

    /// <summary>
    /// Set the integer value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void SamplerParameteriv( uint sampler, int pname, int* param );

    /// <summary>
    /// Set the integer value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void SamplerParameteriv( uint sampler, int pname, int[] param );

    /// <summary>
    /// Set the float value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    void SamplerParameterf( uint sampler, int pname, float param );

    /// <summary>
    /// Set the float value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void SamplerParameterfv( uint sampler, int pname, float* param );

    /// <summary>
    /// Set the float value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void SamplerParameterfv( uint sampler, int pname, float[] param );

    /// <summary>
    /// Set the integer value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void SamplerParameterIiv( uint sampler, int pname, int* param );

    /// <summary>
    /// Set the integer value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void SamplerParameterIiv( uint sampler, int pname, int[] param );

    /// <summary>
    /// Set the unsigned integer value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void SamplerParameterIuiv( uint sampler, int pname, uint* param );

    /// <summary>
    /// Set the unsigned integer value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to modify.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Specifies the value of <paramref name="pname"/>.</param>
    unsafe void SamplerParameterIuiv( uint sampler, int pname, uint[] param );

    /// <summary>
    /// Return the integer value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to query.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Returns the value of <paramref name="pname"/>.</param>
    unsafe void GetSamplerParameteriv( uint sampler, int pname, int* param );

    /// <summary>
    /// Return the value of a sampler parameter.
    /// </summary>
    /// <param name="sampler">Specifies the name of the sampler object whose parameter to query.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a single-valued sampler parameter. One of
    /// <see cref="GL_TEXTURE_WRAP_S"/>, <see cref="GL_TEXTURE_WRAP_T"/>, <see cref="GL_TEXTURE_WRAP_R"/>,
    /// <see cref="GL_TEXTURE_MIN_FILTER"/>, <see cref="GL_TEXTURE_MAG_FILTER"/>, <see cref="GL_TEXTURE_BORDER_COLOR"/>,
    /// <see cref="GL_TEXTURE_MIN_LOD"/>, <see cref="GL_TEXTURE_MAX_LOD"/>, <see cref="GL_TEXTURE_LOD_BIAS"/>,
    /// <see cref="GL_TEXTURE_COMPARE_MODE"/>, <see cref="GL_TEXTURE_COMPARE_FUNC"/>.
    /// </param>
    /// <param name="param">Returns the value of <paramref name="pname"/>.</param>
    unsafe void GetSamplerParameteriv( uint sampler, int pname, ref int[] param );

    /// <inheritdoc cref="GLBindings.glGetSamplerParameteriv(GLuint,GLenum,GLint*)"/>
    unsafe void GetSamplerParameterIiv( uint sampler, int pname, int* param );

    /// <inheritdoc cref="GLBindings.glGetSamplerParameteriv(GLuint,GLenum,ref int[])"/>
    unsafe void GetSamplerParameterIiv( uint sampler, int pname, ref int[] param );

    /// <inheritdoc cref="GLBindings.glGetSamplerParameteriv(GLuint,GLenum,GLint*)"/>
    unsafe void GetSamplerParameterfv( uint sampler, int pname, float* param );

    /// <inheritdoc cref="GLBindings.glGetSamplerParameteriv(GLuint,GLenum,ref int[])"/>
    unsafe void GetSamplerParameterfv( uint sampler, int pname, ref float[] param );

    /// <inheritdoc cref="GLBindings.glGetSamplerParameteriv(GLuint,GLenum,GLint*)"/>
    unsafe void GetSamplerParameterIuiv( uint sampler, int pname, uint* param );

    /// <inheritdoc cref="GLBindings.glGetSamplerParameteriv(GLuint,GLenum,ref int[])"/>
    unsafe void GetSamplerParameterIuiv( uint sampler, int pname, ref uint[] param );

    /// <summary>
    /// Record the  time into a query object after all previous commands have reached the  server but have not yet
    /// necessarily executed.
    /// </summary>
    /// <param name="id">Specifies the name of a query object into which to record the  time.</param>
    /// <param name="target">Specifies the counter to use as the source of the time.</param>
    void QueryCounter( uint id, int target );

    /// <summary>
    /// Return the 64bit integer value of a query object parameter.
    /// </summary>
    /// <param name="id">Specifies the name of a query object.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a query object parameter. One of <see cref="GL_QUERY_RESULT"/>,
    /// <see cref="GL_QUERY_RESULT_NO_WAIT"/> or <see cref="GL_QUERY_RESULT_AVAILABLE"/>.
    /// </param>
    /// <param name="param">Returns the value of <paramref name="pname"/>.</param>
    unsafe void GetQueryObjecti64v( uint id, int pname, Int64* param );

    /// <inheritdoc cref="GLBindings.glGetQueryObjecti64v(GLuint,GLenum,GLint64*)"/>
    unsafe void GetQueryObjecti64v( uint id, int pname, ref Int64[] param );

    /// <summary>
    /// Return the 64bit unsigned integer value of a query object parameter.
    /// </summary>
    /// <param name="id">Specifies the name of a query object.</param>
    /// <param name="pname">
    /// Specifies the symbolic name of a query object parameter. One of <see cref="GL_QUERY_RESULT"/>,
    /// <see cref="GL_QUERY_RESULT_NO_WAIT"/> or <see cref="GL_QUERY_RESULT_AVAILABLE"/>.
    /// </param>
    /// <param name="param">Returns the value of <paramref name="pname"/>.</param>
    unsafe void GetQueryObjectui64v( uint id, int pname, UInt64* param );

    /// <inheritdoc cref="GLBindings.glGetQueryObjectui64v(GLuint,GLenum,GLuint64*)"/>
    unsafe void GetQueryObjectui64v( uint id, int pname, ref UInt64[] param );

    /// <summary>
    /// Modify the reate at which generic vertex attributes advance during instanced rendering
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute.</param>
    /// <param name="divisor">
    /// Specifies the number of instances that will pass between updates of the generic attribute at slot
    /// <paramref name="index"/>.
    /// </param>
    void VertexAttribDivisor( uint index, uint divisor );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies the value of the vertex attribute.</param>
    void VertexAttribP1ui( uint index, int type, bool normalized, uint value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies a pointer to the value of the vertex attribute.</param>
    unsafe void VertexAttribP1uiv( uint index, int type, bool normalized, uint* value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies the value of the vertex attribute.</param>
    unsafe void VertexAttribP1uiv( uint index, int type, bool normalized, uint[] value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies the value of the vertex attribute.</param>
    void VertexAttribP2ui( uint index, int type, bool normalized, uint value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies a pointer to the value of the vertex attribute.</param>
    unsafe void VertexAttribP2uiv( uint index, int type, bool normalized, uint* value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies the value of the vertex attribute.</param>
    unsafe void VertexAttribP2uiv( uint index, int type, bool normalized, uint[] value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies the value of the vertex attribute.</param>
    void VertexAttribP3ui( uint index, int type, bool normalized, uint value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies a pointer to the value of the vertex attribute.</param>
    unsafe void VertexAttribP3uiv( uint index, int type, bool normalized, uint* value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies the value of the vertex attribute.</param>
    unsafe void VertexAttribP3uiv( uint index, int type, bool normalized, uint[] value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies the value of the vertex attribute.</param>
    void VertexAttribP4ui( uint index, int type, bool normalized, uint value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies a pointer to the value of the vertex attribute.</param>
    unsafe void VertexAttribP4uiv( uint index, int type, bool normalized, uint* value );

    /// <summary>
    /// Specify the value of a generic vertex attribute
    /// </summary>
    /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
    /// <param name="type">Specifies the data type of the vertex attribute value.</param>
    /// <param name="normalized">
    /// Specifies whether fixed-point data values should be normalized (<see langword="true"/>), or
    /// converted directly as fixed-point values (<see langword="false"/>).
    /// </param>
    /// <param name="value">Specifies the value of the vertex attribute.</param>
    unsafe void VertexAttribP4uiv( uint index, int type, bool normalized, uint[] value );

    /// <summary>
    /// Loads OpenGL functions using the specified loader delegate.
    /// </summary>
    public delegate IntPtr GetProcAddressDelegate( string funcName );
}