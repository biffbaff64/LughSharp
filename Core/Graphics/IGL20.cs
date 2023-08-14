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

using LibGDXSharp.Utils.Buffers;

using Buffer = LibGDXSharp.Utils.Buffers.Buffer;

namespace LibGDXSharp.Graphics;

public interface IGL20
{
    public const int GL_ES_VERSION_2_0                               = 1;
    public const int GL_DEPTH_BUFFER_BIT                             = 0x00000100;
    public const int GL_STENCIL_BUFFER_BIT                           = 0x00000400;
    public const int GL_COLOR_BUFFER_BIT                             = 0x00004000;
    public const int GL_FALSE                                        = 0;
    public const int GL_TRUE                                         = 1;
    public const int GL_POINTS                                       = 0x0000;
    public const int GL_LINES                                        = 0x0001;
    public const int GL_LINE_LOOP                                    = 0x0002;
    public const int GL_LINE_STRIP                                   = 0x0003;
    public const int GL_TRIANGLES                                    = 0x0004;
    public const int GL_TRIANGLE_STRIP                               = 0x0005;
    public const int GL_TRIANGLE_FAN                                 = 0x0006;
    public const int GL_ZERO                                         = 0;
    public const int GL_ONE                                          = 1;
    public const int GL_SRC_COLOR                                    = 0x0300;
    public const int GL_ONE_MINUS_SRC_COLOR                          = 0x0301;
    public const int GL_SRC_ALPHA                                    = 0x0302;
    public const int GL_ONE_MINUS_SRC_ALPHA                          = 0x0303;
    public const int GL_DST_ALPHA                                    = 0x0304;
    public const int GL_ONE_MINUS_DST_ALPHA                          = 0x0305;
    public const int GL_DST_COLOR                                    = 0x0306;
    public const int GL_ONE_MINUS_DST_COLOR                          = 0x0307;
    public const int GL_SRC_ALPHA_SATURATE                           = 0x0308;
    public const int GL_FUNC_ADD                                     = 0x8006;
    public const int GL_BLEND_EQUATION                               = 0x8009;
    public const int GL_BLEND_EQUATION_RGB                           = 0x8009;
    public const int GL_BLEND_EQUATION_ALPHA                         = 0x883D;
    public const int GL_FUNC_SUBTRACT                                = 0x800A;
    public const int GL_FUNC_REVERSE_SUBTRACT                        = 0x800B;
    public const int GL_BLEND_DST_RGB                                = 0x80C8;
    public const int GL_BLEND_SRC_RGB                                = 0x80C9;
    public const int GL_BLEND_DST_ALPHA                              = 0x80CA;
    public const int GL_BLEND_SRC_ALPHA                              = 0x80CB;
    public const int GL_CONSTANT_COLOR                               = 0x8001;
    public const int GL_ONE_MINUS_CONSTANT_COLOR                     = 0x8002;
    public const int GL_CONSTANT_ALPHA                               = 0x8003;
    public const int GL_ONE_MINUS_CONSTANT_ALPHA                     = 0x8004;
    public const int GL_BLEND_COLOR                                  = 0x8005;
    public const int GL_ARRAY_BUFFER                                 = 0x8892;
    public const int GL_ELEMENT_ARRAY_BUFFER                         = 0x8893;
    public const int GL_ARRAY_BUFFER_BINDING                         = 0x8894;
    public const int GL_ELEMENT_ARRAY_BUFFER_BINDING                 = 0x8895;
    public const int GL_STREAM_DRAW                                  = 0x88E0;
    public const int GL_STATIC_DRAW                                  = 0x88E4;
    public const int GL_DYNAMIC_DRAW                                 = 0x88E8;
    public const int GL_BUFFER_SIZE                                  = 0x8764;
    public const int GL_BUFFER_USAGE                                 = 0x8765;
    public const int GL_CURRENT_VERTEX_ATTRIB                        = 0x8626;
    public const int GL_FRONT                                        = 0x0404;
    public const int GL_BACK                                         = 0x0405;
    public const int GL_FRONT_AND_BACK                               = 0x0408;
    public const int GL_TEXTURE_2D                                   = 0x0DE1;
    public const int GL_CULL_FACE                                    = 0x0B44;
    public const int GL_BLEND                                        = 0x0BE2;
    public const int GL_DITHER                                       = 0x0BD0;
    public const int GL_STENCIL_TEST                                 = 0x0B90;
    public const int GL_DEPTH_TEST                                   = 0x0B71;
    public const int GL_SCISSOR_TEST                                 = 0x0C11;
    public const int GL_POLYGON_OFFSET_FILL                          = 0x8037;
    public const int GL_SAMPLE_ALPHA_TO_COVERAGE                     = 0x809E;
    public const int GL_SAMPLE_COVERAGE                              = 0x80A0;
    public const int GL_NO_ERROR                                     = 0;
    public const int GL_INVALID_ENUM                                 = 0x0500;
    public const int GL_INVALID_VALUE                                = 0x0501;
    public const int GL_INVALID_OPERATION                            = 0x0502;
    public const int GL_OUT_OF_MEMORY                                = 0x0505;
    public const int GL_CW                                           = 0x0900;
    public const int GL_CCW                                          = 0x0901;
    public const int GL_LINE_WIDTH                                   = 0x0B21;
    public const int GL_ALIASED_POINT_SIZE_RANGE                     = 0x846D;
    public const int GL_ALIASED_LINE_WIDTH_RANGE                     = 0x846E;
    public const int GL_CULL_FACE_MODE                               = 0x0B45;
    public const int GL_FRONT_FACE                                   = 0x0B46;
    public const int GL_DEPTH_RANGE                                  = 0x0B70;
    public const int GL_DEPTH_WRITEMASK                              = 0x0B72;
    public const int GL_DEPTH_CLEAR_VALUE                            = 0x0B73;
    public const int GL_DEPTH_FUNC                                   = 0x0B74;
    public const int GL_STENCIL_CLEAR_VALUE                          = 0x0B91;
    public const int GL_STENCIL_FUNC                                 = 0x0B92;
    public const int GL_STENCIL_FAIL                                 = 0x0B94;
    public const int GL_STENCIL_PASS_DEPTH_FAIL                      = 0x0B95;
    public const int GL_STENCIL_PASS_DEPTH_PASS                      = 0x0B96;
    public const int GL_STENCIL_REF                                  = 0x0B97;
    public const int GL_STENCIL_VALUE_MASK                           = 0x0B93;
    public const int GL_STENCIL_WRITEMASK                            = 0x0B98;
    public const int GL_STENCIL_BACK_FUNC                            = 0x8800;
    public const int GL_STENCIL_BACK_FAIL                            = 0x8801;
    public const int GL_STENCIL_BACK_PASS_DEPTH_FAIL                 = 0x8802;
    public const int GL_STENCIL_BACK_PASS_DEPTH_PASS                 = 0x8803;
    public const int GL_STENCIL_BACK_REF                             = 0x8CA3;
    public const int GL_STENCIL_BACK_VALUE_MASK                      = 0x8CA4;
    public const int GL_STENCIL_BACK_WRITEMASK                       = 0x8CA5;
    public const int GL_VIEWPORT                                     = 0x0BA2;
    public const int GL_SCISSOR_BOX                                  = 0x0C10;
    public const int GL_COLOR_CLEAR_VALUE                            = 0x0C22;
    public const int GL_COLOR_WRITEMASK                              = 0x0C23;
    public const int GL_UNPACK_ALIGNMENT                             = 0x0CF5;
    public const int GL_PACK_ALIGNMENT                               = 0x0D05;
    public const int GL_MAX_TEXTURE_SIZE                             = 0x0D33;
    public const int GL_MAX_TEXTURE_UNITS                            = 0x84E2;
    public const int GL_MAX_VIEWPORT_DIMS                            = 0x0D3A;
    public const int GL_SUBPIXEL_BITS                                = 0x0D50;
    public const int GL_RED_BITS                                     = 0x0D52;
    public const int GL_GREEN_BITS                                   = 0x0D53;
    public const int GL_BLUE_BITS                                    = 0x0D54;
    public const int GL_ALPHA_BITS                                   = 0x0D55;
    public const int GL_DEPTH_BITS                                   = 0x0D56;
    public const int GL_STENCIL_BITS                                 = 0x0D57;
    public const int GL_POLYGON_OFFSET_UNITS                         = 0x2A00;
    public const int GL_POLYGON_OFFSET_FACTOR                        = 0x8038;
    public const int GL_TEXTURE_BINDING_2D                           = 0x8069;
    public const int GL_SAMPLE_BUFFERS                               = 0x80A8;
    public const int GL_SAMPLES                                      = 0x80A9;
    public const int GL_SAMPLE_COVERAGE_VALUE                        = 0x80AA;
    public const int GL_SAMPLE_COVERAGE_INVERT                       = 0x80AB;
    public const int GL_NUM_COMPRESSED_TEXTURE_FORMATS               = 0x86A2;
    public const int GL_COMPRESSED_TEXTURE_FORMATS                   = 0x86A3;
    public const int GL_DONT_CARE                                    = 0x1100;
    public const int GL_FASTEST                                      = 0x1101;
    public const int GL_NICEST                                       = 0x1102;
    public const int GL_GENERATE_MIPMAP                              = 0x8191;
    public const int GL_GENERATE_MIPMAP_HINT                         = 0x8192;
    public const int GL_BYTE                                         = 0x1400;
    public const int GL_UNSIGNED_BYTE                                = 0x1401;
    public const int GL_SHORT                                        = 0x1402;
    public const int GL_UNSIGNED_SHORT                               = 0x1403;
    public const int GL_INT                                          = 0x1404;
    public const int GL_UNSIGNED_INT                                 = 0x1405;
    public const int GL_FLOAT                                        = 0x1406;
    public const int GL_FIXED                                        = 0x140C;
    public const int GL_DEPTH_COMPONENT                              = 0x1902;
    public const int GL_ALPHA                                        = 0x1906;
    public const int GL_RGB                                          = 0x1907;
    public const int GL_RGBA                                         = 0x1908;
    public const int GL_LUMINANCE                                    = 0x1909;
    public const int GL_LUMINANCE_ALPHA                              = 0x190A;
    public const int GL_UNSIGNED_SHORT_4_4_4_4                       = 0x8033;
    public const int GL_UNSIGNED_SHORT_5_5_5_1                       = 0x8034;
    public const int GL_UNSIGNED_SHORT_5_6_5                         = 0x8363;
    public const int GL_FRAGMENT_SHADER                              = 0x8B30;
    public const int GL_VERTEX_SHADER                                = 0x8B31;
    public const int GL_MAX_VERTEX_ATTRIBS                           = 0x8869;
    public const int GL_MAX_VERTEX_UNIFORM_VECTORS                   = 0x8DFB;
    public const int GL_MAX_VARYING_VECTORS                          = 0x8DFC;
    public const int GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS             = 0x8B4D;
    public const int GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS               = 0x8B4C;
    public const int GL_MAX_TEXTURE_IMAGE_UNITS                      = 0x8872;
    public const int GL_MAX_FRAGMENT_UNIFORM_VECTORS                 = 0x8DFD;
    public const int GL_SHADER_TYPE                                  = 0x8B4F;
    public const int GL_DELETE_STATUS                                = 0x8B80;
    public const int GL_LINK_STATUS                                  = 0x8B82;
    public const int GL_VALIDATE_STATUS                              = 0x8B83;
    public const int GL_ATTACHED_SHADERS                             = 0x8B85;
    public const int GL_ACTIVE_UNIFORMS                              = 0x8B86;
    public const int GL_ACTIVE_UNIFORM_MAX_LENGTH                    = 0x8B87;
    public const int GL_ACTIVE_ATTRIBUTES                            = 0x8B89;
    public const int GL_ACTIVE_ATTRIBUTE_MAX_LENGTH                  = 0x8B8A;
    public const int GL_SHADING_LANGUAGE_VERSION                     = 0x8B8C;
    public const int GL_CURRENT_PROGRAM                              = 0x8B8D;
    public const int GL_NEVER                                        = 0x0200;
    public const int GL_LESS                                         = 0x0201;
    public const int GL_EQUAL                                        = 0x0202;
    public const int GL_LEQUAL                                       = 0x0203;
    public const int GL_GREATER                                      = 0x0204;
    public const int GL_NOTEQUAL                                     = 0x0205;
    public const int GL_GEQUAL                                       = 0x0206;
    public const int GL_ALWAYS                                       = 0x0207;
    public const int GL_KEEP                                         = 0x1E00;
    public const int GL_REPLACE                                      = 0x1E01;
    public const int GL_INCR                                         = 0x1E02;
    public const int GL_DECR                                         = 0x1E03;
    public const int GL_INVERT                                       = 0x150A;
    public const int GL_INCR_WRAP                                    = 0x8507;
    public const int GL_DECR_WRAP                                    = 0x8508;
    public const int GL_VENDOR                                       = 0x1F00;
    public const int GL_RENDERER                                     = 0x1F01;
    public const int GL_VERSION                                      = 0x1F02;
    public const int GL_EXTENSIONS                                   = 0x1F03;
    public const int GL_NEAREST                                      = 0x2600;
    public const int GL_LINEAR                                       = 0x2601;
    public const int GL_NEAREST_MIPMAP_NEAREST                       = 0x2700;
    public const int GL_LINEAR_MIPMAP_NEAREST                        = 0x2701;
    public const int GL_NEAREST_MIPMAP_LINEAR                        = 0x2702;
    public const int GL_LINEAR_MIPMAP_LINEAR                         = 0x2703;
    public const int GL_TEXTURE_MAG_FILTER                           = 0x2800;
    public const int GL_TEXTURE_MIN_FILTER                           = 0x2801;
    public const int GL_TEXTURE_WRAP_S                               = 0x2802;
    public const int GL_TEXTURE_WRAP_T                               = 0x2803;
    public const int GL_TEXTURE                                      = 0x1702;
    public const int GL_TEXTURE_CUBE_MAP                             = 0x8513;
    public const int GL_TEXTURE_BINDING_CUBE_MAP                     = 0x8514;
    public const int GL_TEXTURE_CUBE_MAP_POSITIVE_X                  = 0x8515;
    public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_X                  = 0x8516;
    public const int GL_TEXTURE_CUBE_MAP_POSITIVE_Y                  = 0x8517;
    public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_Y                  = 0x8518;
    public const int GL_TEXTURE_CUBE_MAP_POSITIVE_Z                  = 0x8519;
    public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_Z                  = 0x851A;
    public const int GL_MAX_CUBE_MAP_TEXTURE_SIZE                    = 0x851C;
    public const int GL_TEXTURE0                                     = 0x84C0;
    public const int GL_TEXTURE1                                     = 0x84C1;
    public const int GL_TEXTURE2                                     = 0x84C2;
    public const int GL_TEXTURE3                                     = 0x84C3;
    public const int GL_TEXTURE4                                     = 0x84C4;
    public const int GL_TEXTURE5                                     = 0x84C5;
    public const int GL_TEXTURE6                                     = 0x84C6;
    public const int GL_TEXTURE7                                     = 0x84C7;
    public const int GL_TEXTURE8                                     = 0x84C8;
    public const int GL_TEXTURE9                                     = 0x84C9;
    public const int GL_TEXTURE10                                    = 0x84CA;
    public const int GL_TEXTURE11                                    = 0x84CB;
    public const int GL_TEXTURE12                                    = 0x84CC;
    public const int GL_TEXTURE13                                    = 0x84CD;
    public const int GL_TEXTURE14                                    = 0x84CE;
    public const int GL_TEXTURE15                                    = 0x84CF;
    public const int GL_TEXTURE16                                    = 0x84D0;
    public const int GL_TEXTURE17                                    = 0x84D1;
    public const int GL_TEXTURE18                                    = 0x84D2;
    public const int GL_TEXTURE19                                    = 0x84D3;
    public const int GL_TEXTURE20                                    = 0x84D4;
    public const int GL_TEXTURE21                                    = 0x84D5;
    public const int GL_TEXTURE22                                    = 0x84D6;
    public const int GL_TEXTURE23                                    = 0x84D7;
    public const int GL_TEXTURE24                                    = 0x84D8;
    public const int GL_TEXTURE25                                    = 0x84D9;
    public const int GL_TEXTURE26                                    = 0x84DA;
    public const int GL_TEXTURE27                                    = 0x84DB;
    public const int GL_TEXTURE28                                    = 0x84DC;
    public const int GL_TEXTURE29                                    = 0x84DD;
    public const int GL_TEXTURE30                                    = 0x84DE;
    public const int GL_TEXTURE31                                    = 0x84DF;
    public const int GL_ACTIVE_TEXTURE                               = 0x84E0;
    public const int GL_REPEAT                                       = 0x2901;
    public const int GL_CLAMP_TO_EDGE                                = 0x812F;
    public const int GL_MIRRORED_REPEAT                              = 0x8370;
    public const int GL_FLOAT_VEC2                                   = 0x8B50;
    public const int GL_FLOAT_VEC3                                   = 0x8B51;
    public const int GL_FLOAT_VEC4                                   = 0x8B52;
    public const int GL_INT_VEC2                                     = 0x8B53;
    public const int GL_INT_VEC3                                     = 0x8B54;
    public const int GL_INT_VEC4                                     = 0x8B55;
    public const int GL_BOOL                                         = 0x8B56;
    public const int GL_BOOL_VEC2                                    = 0x8B57;
    public const int GL_BOOL_VEC3                                    = 0x8B58;
    public const int GL_BOOL_VEC4                                    = 0x8B59;
    public const int GL_FLOAT_MAT2                                   = 0x8B5A;
    public const int GL_FLOAT_MAT3                                   = 0x8B5B;
    public const int GL_FLOAT_MAT4                                   = 0x8B5C;
    public const int GL_SAMPLER_2D                                   = 0x8B5E;
    public const int GL_SAMPLER_CUBE                                 = 0x8B60;
    public const int GL_VERTEX_ATTRIB_ARRAY_ENABLED                  = 0x8622;
    public const int GL_VERTEX_ATTRIB_ARRAY_SIZE                     = 0x8623;
    public const int GL_VERTEX_ATTRIB_ARRAY_STRIDE                   = 0x8624;
    public const int GL_VERTEX_ATTRIB_ARRAY_TYPE                     = 0x8625;
    public const int GL_VERTEX_ATTRIB_ARRAY_NORMALIZED               = 0x886A;
    public const int GL_VERTEX_ATTRIB_ARRAY_POINTER                  = 0x8645;
    public const int GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING           = 0x889F;
    public const int GL_IMPLEMENTATION_COLOR_READ_TYPE               = 0x8B9A;
    public const int GL_IMPLEMENTATION_COLOR_READ_FORMAT             = 0x8B9B;
    public const int GL_COMPILE_STATUS                               = 0x8B81;
    public const int GL_INFO_LOG_LENGTH                              = 0x8B84;
    public const int GL_SHADER_SOURCE_LENGTH                         = 0x8B88;
    public const int GL_SHADER_COMPILER                              = 0x8DFA;
    public const int GL_SHADER_BINARY_FORMATS                        = 0x8DF8;
    public const int GL_NUM_SHADER_BINARY_FORMATS                    = 0x8DF9;
    public const int GL_LOW_FLOAT                                    = 0x8DF0;
    public const int GL_MEDIUM_FLOAT                                 = 0x8DF1;
    public const int GL_HIGH_FLOAT                                   = 0x8DF2;
    public const int GL_LOW_INT                                      = 0x8DF3;
    public const int GL_MEDIUM_INT                                   = 0x8DF4;
    public const int GL_HIGH_INT                                     = 0x8DF5;
    public const int GL_FRAMEBUFFER                                  = 0x8D40;
    public const int GL_RENDERBUFFER                                 = 0x8D41;
    public const int GL_RGBA4                                        = 0x8056;
    public const int GL_RGB5_A1                                      = 0x8057;
    public const int GL_RGB565                                       = 0x8D62;
    public const int GL_DEPTH_COMPONENT16                            = 0x81A5;
    public const int GL_STENCIL_INDEX                                = 0x1901;
    public const int GL_STENCIL_INDEX8                               = 0x8D48;
    public const int GL_RENDERBUFFER_WIDTH                           = 0x8D42;
    public const int GL_RENDERBUFFER_HEIGHT                          = 0x8D43;
    public const int GL_RENDERBUFFER_INTERNAL_FORMAT                 = 0x8D44;
    public const int GL_RENDERBUFFER_RED_SIZE                        = 0x8D50;
    public const int GL_RENDERBUFFER_GREEN_SIZE                      = 0x8D51;
    public const int GL_RENDERBUFFER_BLUE_SIZE                       = 0x8D52;
    public const int GL_RENDERBUFFER_ALPHA_SIZE                      = 0x8D53;
    public const int GL_RENDERBUFFER_DEPTH_SIZE                      = 0x8D54;
    public const int GL_RENDERBUFFER_STENCIL_SIZE                    = 0x8D55;
    public const int GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE           = 0x8CD0;
    public const int GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME           = 0x8CD1;
    public const int GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL         = 0x8CD2;
    public const int GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
    public const int GL_COLOR_ATTACHMENT0                            = 0x8CE0;
    public const int GL_DEPTH_ATTACHMENT                             = 0x8D00;
    public const int GL_STENCIL_ATTACHMENT                           = 0x8D20;
    public const int GL_NONE                                         = 0;
    public const int GL_FRAMEBUFFER_COMPLETE                         = 0x8CD5;
    public const int GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT            = 0x8CD6;
    public const int GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT    = 0x8CD7;
    public const int GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS            = 0x8CD9;
    public const int GL_FRAMEBUFFER_UNSUPPORTED                      = 0x8CDD;
    public const int GL_FRAMEBUFFER_BINDING                          = 0x8CA6;
    public const int GL_RENDERBUFFER_BINDING                         = 0x8CA7;
    public const int GL_MAX_RENDERBUFFER_SIZE                        = 0x84E8;
    public const int GL_INVALID_FRAMEBUFFER_OPERATION                = 0x0506;
    public const int GL_VERTEX_PROGRAM_POINT_SIZE                    = 0x8642;

    // Extensions
    public const int GL_COVERAGE_BUFFER_BIT_NV         = 0x8000;
    public const int GL_TEXTURE_MAX_ANISOTROPY_EXT     = 0x84FE;
    public const int GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT = 0x84FF;

    // ------------------------------------------------

    public void GLActiveTexture( int texture );

    public void GLBindTexture( int target, int texture );

    public void GLBlendFunc( int sfactor, int dfactor );

    public void GLClear( int mask );

    public void GLClearColor( float red, float green, float blue, float alpha );

    public void GLClearDepthf( float depth );

    public void GLClearStencil( int s );

    public void GLColorMask( bool red, bool green, bool blue, bool alpha );

    public void GLCompressedTexImage2D( int target,
                                        int level,
                                        int internalformat,
                                        int width,
                                        int height,
                                        int border,
                                        int imageSize,
                                        Buffer data );

    public void GLCompressedTexSubImage2D( int target,
                                           int level,
                                           int xoffset,
                                           int yoffset,
                                           int width,
                                           int height,
                                           int format,
                                           int imageSize,
                                           Buffer data );

    public void GLCopyTexImage2D( int target,
                                  int level,
                                  int internalformat,
                                  int x,
                                  int y,
                                  int width,
                                  int height,
                                  int border );

    public void GLCopyTexSubImage2D( int target,
                                     int level,
                                     int xoffset,
                                     int yoffset,
                                     int x,
                                     int y,
                                     int width,
                                     int height );

    public void GLCullFace( int mode );

    public void GLDeleteTextures( int n, IntBuffer textures );

    public void GLDeleteTexture( int texture );

    public void GLDepthFunc( int func );

    public void GLDepthMask( bool flag );

    public void GLDepthRangef( float zNear, float zFar );

    public void GLDisable( int cap );

    public void GLDrawArrays( int mode, int first, int count );

    public void GLDrawElements( int mode, int count, int type, Buffer indices );

    public void GLEnable( int cap );

    public void GLFinish();

    public void GLFlush();

    public void GLFrontFace( int mode );

    public void GLGenTextures( int n, IntBuffer textures );

    public int GLGenTexture();

    public int GLGetError();

    public void GLGetIntegerv( int pname, IntBuffer parameters );

    public string GLGetString( int name );

    public void GLHint( int target, int mode );

    public void GLLineWidth( float width );

    public void GLPixelStorei( int pname, int param );

    public void GLPolygonOffset( float factor, float units );

    public void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels );

    public void GLScissor( int x, int y, int width, int height );

    public void GLStencilFunc( int func, int reference, int mask );

    public void GLStencilMask( int mask );

    public void GLStencilOp( int fail, int zfail, int zpass );

    public void GLTexImage2D( int target,
                              int level,
                              int internalformat,
                              int width,
                              int height,
                              int border,
                              int format,
                              int type,
                              Buffer pixels );

    public void GLTexParameterf( int target, int pname, float param );

    public void GLTexSubImage2D( int target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int width,
                                 int height,
                                 int format,
                                 int type,
                                 Buffer pixels );

    public void GLViewport( int x, int y, int width, int height );

    public void GLAttachShader( int program, int shader );

    public void GLBindAttribLocation( int program, int index, string name );

    public void GLBindBuffer( int target, int buffer );

    public void GLBindFramebuffer( int target, int framebuffer );

    public void GLBindRenderbuffer( int target, int renderbuffer );

    public void GLBlendColor( float red, float green, float blue, float alpha );

    public void GLBlendEquation( int mode );

    public void GLBlendEquationSeparate( int modeRgb, int modeAlpha );

    public void GLBlendFuncSeparate( int srcRgb, int dstRgb, int srcAlpha, int dstAlpha );

    public void GLBufferData( int target, int size, Buffer data, int usage );

    public void GLBufferSubData( int target, int offset, int size, Buffer data );

    public int GLCheckFramebufferStatus( int target );

    public void GLCompileShader( int shader );

    public int GLCreateProgram();

    public int GLCreateShader( int type );

    public void GLDeleteBuffer( int buffer );

    public void GLDeleteBuffers( int n, IntBuffer buffers );

    public void GLDeleteFramebuffer( int framebuffer );

    public void GLDeleteFramebuffers( int n, IntBuffer framebuffers );

    public void GLDeleteProgram( int program );

    public void GLDeleteRenderbuffer( int renderbuffer );

    public void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers );

    public void GLDeleteShader( int shader );

    public void GLDetachShader( int program, int shader );

    public void GLDisableVertexAttribArray( int index );

    public void GLDrawElements( int mode, int count, int type, int indices );

    public void GLEnableVertexAttribArray( int index );

    public void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer );

    public void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level );

    public int GLGenBuffer();

    public void GLGenBuffers( int n, IntBuffer buffers );

    public void GLGenerateMipmap( int target );

    public int GLGenFramebuffer();

    public void GLGenFramebuffers( int n, IntBuffer framebuffers );

    public int GLGenRenderbuffer();

    public void GLGenRenderbuffers( int n, IntBuffer renderbuffers );

    // deviates
    public string GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type );

    // deviates
    public string GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type );

    public void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders );

    public int GLGetAttribLocation( int program, string name );

    public void GLGetboolv( int pname, Buffer parameters );

    public void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters );

    public void GLGetFloatv( int pname, FloatBuffer parameters );

    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters );

    public void GLGetProgramiv( int program, int pname, IntBuffer parameters );

    public string GLGetProgramInfoLog( int program );

    public void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters );

    public void GLGetShaderiv( int shader, int pname, IntBuffer parameters );

    public string GLGetShaderInfoLog( int shader );

    public void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision );

    public void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters );

    public void GLGetTexParameteriv( int target, int pname, IntBuffer parameters );

    public void GLGetUniformfv( int program, int location, FloatBuffer parameters );

    public void GLGetUniformiv( int program, int location, IntBuffer parameters );

    public int GLGetUniformLocation( int program, string name );

    public void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters );

    public void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters );

    public void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer );

    public bool GLIsBuffer( int buffer );

    public bool GLIsEnabled( int cap );

    public bool GLIsFramebuffer( int framebuffer );

    public bool GLIsProgram( int program );

    public bool GLIsRenderbuffer( int renderbuffer );

    public bool GLIsShader( int shader );

    public bool GLIsTexture( int texture );

    public void GLLinkProgram( int program );

    public void GLReleaseShaderCompiler();

    public void GLRenderbufferStorage( int target, int internalformat, int width, int height );

    public void GLSampleCoverage( float value, bool invert );

    public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length );

    public void GLShaderSource( int shader, string str );

    public void GLStencilFuncSeparate( int face, int func, int reference, int mask );

    public void GLStencilMaskSeparate( int face, int mask );

    public void GLStencilOpSeparate( int face, int fail, int zfail, int zpass );

    public void GLTexParameterfv( int target, int pname, FloatBuffer parameters );

    public void GLTexParameteri( int target, int pname, int param );

    public void GLTexParameteriv( int target, int pname, IntBuffer parameters );

    public void GLUniform1F( int location, float x );

    public void GLUniform1Fv( int location, int count, FloatBuffer v );

    public void GLUniform1Fv( int location, int count, float[] v, int offset );

    public void GLUniform1I( int location, int x );

    public void GLUniform1Iv( int location, int count, IntBuffer v );

    public void GLUniform1Iv( int location, int count, int[] v, int offset );

    public void GLUniform2F( int location, float x, float y );

    public void GLUniform2Fv( int location, int count, FloatBuffer v );

    public void GLUniform2Fv( int location, int count, float[] v, int offset );

    public void GLUniform2I( int location, int x, int y );

    public void GLUniform2Iv( int location, int count, IntBuffer v );

    public void GLUniform2Iv( int location, int count, int[] v, int offset );

    public void GLUniform3F( int location, float x, float y, float z );

    public void GLUniform3Fv( int location, int count, FloatBuffer v );

    public void GLUniform3Fv( int location, int count, float[] v, int offset );

    public void GLUniform3I( int location, int x, int y, int z );

    public void GLUniform3Iv( int location, int count, IntBuffer v );

    public void GLUniform3Iv( int location, int count, int[] v, int offset );

    public void GLUniform4F( int location, float x, float y, float z, float w );

    public void GLUniform4Fv( int location, int count, FloatBuffer v );

    public void GLUniform4Fv( int location, int count, float[] v, int offset );

    public void GLUniform4I( int location, int x, int y, int z, int w );

    public void GLUniform4Iv( int location, int count, IntBuffer v );

    public void GLUniform4Iv( int location, int count, int[] v, int offset );

    public void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value );

    public void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset );

    public void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value );

    public void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset );

    public void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value );

    public void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset );

    public void GLUseProgram( int program );

    public void GLValidateProgram( int program );

    public void GLVertexAttrib1F( int indx, float x );

    public void GLVertexAttrib1Fv( int indx, FloatBuffer values );

    public void GLVertexAttrib2F( int indx, float x, float y );

    public void GLVertexAttrib2Fv( int indx, FloatBuffer values );

    public void GLVertexAttrib3F( int indx, float x, float y, float z );

    public void GLVertexAttrib3Fv( int indx, FloatBuffer values );

    public void GLVertexAttrib4F( int indx, float x, float y, float z, float w );

    public void GLVertexAttrib4Fv( int indx, FloatBuffer values );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    public void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr );

    public void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, int ptr );
}