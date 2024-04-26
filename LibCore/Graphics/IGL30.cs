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


namespace LughSharp.LibCore.Graphics;

public interface IGL30_TBD : IGL20_TBD
{
    // ------------------------------------------------------------------------
    const int GL_CONTEXT_CORE_PROFILE_BIT          = 0x00000001;
    const int GL_VERTEX_SHADER_BIT                 = 0x00000001;
    const int GL_VERTEX_ATTRIB_ARRAY_BARRIER_BIT   = 0x00000001;
    const int GL_SYNC_FLUSH_COMMANDS_BIT           = 0x00000001;
    const int GL_CONTEXT_COMPATIBILITY_PROFILE_BIT = 0x00000002;
    const int GL_FRAGMENT_SHADER_BIT               = 0x00000002;
    const int GL_CONTEXT_FLAG_DEBUG_BIT            = 0x00000002;
    const int GL_ELEMENT_ARRAY_BARRIER_BIT         = 0x00000002;
    const int GL_GEOMETRY_SHADER_BIT               = 0x00000004;
    const int GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT    = 0x00000004;
    const int GL_UNIFORM_BARRIER_BIT               = 0x00000004;
    const int GL_TESS_CONTROL_SHADER_BIT           = 0x00000008;
    const int GL_CONTEXT_FLAG_NO_ERROR_BIT         = 0x00000008;
    const int GL_TEXTURE_FETCH_BARRIER_BIT         = 0x00000008;
    const int GL_TESS_EVALUATION_SHADER_BIT        = 0x00000010;
    const int GL_SHADER_IMAGE_ACCESS_BARRIER_BIT   = 0x00000020;
    const int GL_COMPUTE_SHADER_BIT                = 0x00000020;
    const int GL_COMMAND_BARRIER_BIT               = 0x00000040;
    const int GL_PIXEL_BUFFER_BARRIER_BIT          = 0x00000080;
    const int GL_TEXTURE_UPDATE_BARRIER_BIT        = 0x00000100;
    const int GL_BUFFER_UPDATE_BARRIER_BIT         = 0x00000200;
    const int GL_FRAMEBUFFER_BARRIER_BIT           = 0x00000400;
    const int GL_TRANSFORM_FEEDBACK_BARRIER_BIT    = 0x00000800;
    const int GL_ATOMIC_COUNTER_BARRIER_BIT        = 0x00001000;
    const int GL_SHADER_STORAGE_BARRIER_BIT        = 0x00002000;
    const int GL_CLIENT_MAPPED_BUFFER_BARRIER_BIT  = 0x00004000;
    const int GL_QUERY_BUFFER_BARRIER_BIT          = 0x00008000;

    // ------------------------------------------------------------------------
    const int GL_MAP_READ_BIT                        = 0x0001;
    const int GL_CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT = 0x0001;
    const int GL_MAP_WRITE_BIT                       = 0x0002;
    const int GL_MAP_INVALIDATE_RANGE_BIT            = 0x0004;
    const int GL_QUADS                               = 0x0007;
    const int GL_MAP_INVALIDATE_BUFFER_BIT           = 0x0008;
    const int GL_MAP_FLUSH_EXPLICIT_BIT              = 0x0010;
    const int GL_MAP_UNSYNCHRONIZED_BIT              = 0x0020;

    // ------------------------------------------------------------------------
    const int GL_READ_BUFFER        = 0x0C02;
    const int GL_UNPACK_ROW_LENGTH  = 0x0CF2;
    const int GL_UNPACK_SKIP_ROWS   = 0x0CF3;
    const int GL_UNPACK_SKIP_PIXELS = 0x0CF4;

    // ------------------------------------------------------------------------
    const int GL_PACK_ROW_LENGTH  = 0x0D02;
    const int GL_PACK_SKIP_ROWS   = 0x0D03;
    const int GL_PACK_SKIP_PIXELS = 0x0D04;

    // ------------------------------------------------------------------------
    const int GL_COLOR   = 0x1800;
    const int GL_DEPTH   = 0x1801;
    const int GL_STENCIL = 0x1802;
    const int GL_RED     = 0x1903;

    // ------------------------------------------------------------------------
    const int GL_MIN                   = 0x8007;
    const int GL_MAX                   = 0x8008;
    const int GL_RGB8                  = 0x8051;
    const int GL_RGBA8                 = 0x8058;
    const int GL_RGB10_A2              = 0x8059;
    const int GL_TEXTURE_BINDING_3D    = 0x806A;
    const int GL_UNPACK_SKIP_IMAGES    = 0x806D;
    const int GL_UNPACK_IMAGE_HEIGHT   = 0x806E;
    const int GL_TEXTURE_3D            = 0x806F;
    const int GL_TEXTURE_WRAP_R        = 0x8072;
    const int GL_MAX_3D_TEXTURE_SIZE   = 0x8073;
    const int GL_MAX_ELEMENTS_VERTICES = 0x80E8;
    const int GL_MAX_ELEMENTS_INDICES  = 0x80E9;

    // ------------------------------------------------------------------------
    const int GL_TEXTURE_MIN_LOD    = 0x813A;
    const int GL_TEXTURE_MAX_LOD    = 0x813B;
    const int GL_TEXTURE_BASE_LEVEL = 0x813C;
    const int GL_TEXTURE_MAX_LEVEL  = 0x813D;
    const int GL_DEPTH_COMPONENT24  = 0x81A6;

    // ------------------------------------------------------------------------
    const int GL_UNSIGNED_INT_2_10_10_10_REV = 0x8368;

    // ------------------------------------------------------------------------
    const int GL_MAX_TEXTURE_LOD_BIAS = 0x84FD;
    const int GL_DEPTH_STENCIL        = 0x84F9;
    const int GL_UNSIGNED_INT_24_8    = 0x84FA;

    // ------------------------------------------------------------------------
    const int GL_TEXTURE_COMPARE_MODE        = 0x884C;
    const int GL_TEXTURE_COMPARE_FUNC        = 0x884D;
    const int GL_CURRENT_QUERY               = 0x8865;
    const int GL_QUERY_RESULT                = 0x8866;
    const int GL_QUERY_RESULT_AVAILABLE      = 0x8867;
    const int GL_BUFFER_MAPPED               = 0x88BC;
    const int GL_BUFFER_MAP_POINTER          = 0x88BD;
    const int GL_STREAM_READ                 = 0x88E1;
    const int GL_STREAM_COPY                 = 0x88E2;
    const int GL_STATIC_READ                 = 0x88E5;
    const int GL_STATIC_COPY                 = 0x88E6;
    const int GL_DYNAMIC_READ                = 0x88E9;
    const int GL_DYNAMIC_COPY                = 0x88EA;
    const int GL_MAX_DRAW_BUFFERS            = 0x8824;
    const int GL_DRAW_BUFFER0                = 0x8825;
    const int GL_DRAW_BUFFER1                = 0x8826;
    const int GL_DRAW_BUFFER2                = 0x8827;
    const int GL_DRAW_BUFFER3                = 0x8828;
    const int GL_DRAW_BUFFER4                = 0x8829;
    const int GL_DRAW_BUFFER5                = 0x882A;
    const int GL_DRAW_BUFFER6                = 0x882B;
    const int GL_DRAW_BUFFER7                = 0x882C;
    const int GL_DRAW_BUFFER8                = 0x882D;
    const int GL_DRAW_BUFFER9                = 0x882E;
    const int GL_DRAW_BUFFER10               = 0x882F;
    const int GL_DRAW_BUFFER11               = 0x8830;
    const int GL_DRAW_BUFFER12               = 0x8831;
    const int GL_DRAW_BUFFER13               = 0x8832;
    const int GL_DRAW_BUFFER14               = 0x8833;
    const int GL_DRAW_BUFFER15               = 0x8834;
    const int GL_PIXEL_PACK_BUFFER           = 0x88EB;
    const int GL_PIXEL_UNPACK_BUFFER         = 0x88EC;
    const int GL_PIXEL_PACK_BUFFER_BINDING   = 0x88ED;
    const int GL_PIXEL_UNPACK_BUFFER_BINDING = 0x88EF;
    const int GL_COMPARE_REF_TO_TEXTURE      = 0x884E;
    const int GL_RGBA32_F                    = 0x8814;
    const int GL_RGB32_F                     = 0x8815;
    const int GL_RGBA16_F                    = 0x881A;
    const int GL_RGB16_F                     = 0x881B;
    const int GL_VERTEX_ATTRIB_ARRAY_INTEGER = 0x88FD;
    const int GL_DEPTH24_STENCIL8            = 0x88F0;
    const int GL_MAX_ARRAY_TEXTURE_LAYERS    = 0x88FF;

    // ------------------------------------------------------------------------
    const int GL_MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x8B49;
    const int GL_MAX_VERTEX_UNIFORM_COMPONENTS   = 0x8B4A;
    const int GL_SAMPLER_3D                      = 0x8B5F;
    const int GL_SAMPLER_2D_SHADOW               = 0x8B62;
    const int GL_FRAGMENT_SHADER_DERIVATIVE_HINT = 0x8B8B;
    const int GL_FLOAT_MAT2_X3                   = 0x8B65;
    const int GL_FLOAT_MAT2_X4                   = 0x8B66;
    const int GL_FLOAT_MAT3_X2                   = 0x8B67;
    const int GL_FLOAT_MAT3_X4                   = 0x8B68;
    const int GL_FLOAT_MAT4_X2                   = 0x8B69;
    const int GL_FLOAT_MAT4_X3                   = 0x8B6A;
    const int GL_MAX_VARYING_COMPONENTS          = 0x8B4B;

    // ------------------------------------------------------------------------
    const int GL_SRGB                                          = 0x8C40;
    const int GL_SRGB8                                         = 0x8C41;
    const int GL_SRGB8_ALPHA8                                  = 0x8C43;
    const int GL_TEXTURE_2D_ARRAY                              = 0x8C1A;
    const int GL_TEXTURE_BINDING_2D_ARRAY                      = 0x8C1D;
    const int GL_R11_F_G11_F_B10_F                             = 0x8C3A;
    const int GL_UNSIGNED_INT_10_F_11_F_11_F_REV               = 0x8C3B;
    const int GL_RGB9_E5                                       = 0x8C3D;
    const int GL_UNSIGNED_INT_5_9_9_9_REV                      = 0x8C3E;
    const int GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH         = 0x8C76;
    const int GL_TRANSFORM_FEEDBACK_BUFFER_MODE                = 0x8C7F;
    const int GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS    = 0x8C80;
    const int GL_TRANSFORM_FEEDBACK_VARYINGS                   = 0x8C83;
    const int GL_TRANSFORM_FEEDBACK_BUFFER_START               = 0x8C84;
    const int GL_TRANSFORM_FEEDBACK_BUFFER_SIZE                = 0x8C85;
    const int GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN         = 0x8C88;
    const int GL_RASTERIZER_DISCARD                            = 0x8C89;
    const int GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x8C8A;
    const int GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS       = 0x8C8B;
    const int GL_INTERLEAVED_ATTRIBS                           = 0x8C8C;
    const int GL_SEPARATE_ATTRIBS                              = 0x8C8D;
    const int GL_TRANSFORM_FEEDBACK_BUFFER                     = 0x8C8E;
    const int GL_TRANSFORM_FEEDBACK_BUFFER_BINDING             = 0x8C8F;
    const int GL_DEPTH_COMPONENT32_F                           = 0x8CAC;
    const int GL_DEPTH32_F_STENCIL8                            = 0x8CAD;
    const int GL_UNSIGNED_NORMALIZED                           = 0x8C17;
    const int GL_DRAW_FRAMEBUFFER_BINDING                      = 0x8CA6;
    const int GL_READ_FRAMEBUFFER                              = 0x8CA8;
    const int GL_DRAW_FRAMEBUFFER                              = 0x8CA9;
    const int GL_READ_FRAMEBUFFER_BINDING                      = 0x8CAA;
    const int GL_RENDERBUFFER_SAMPLES                          = 0x8CAB;
    const int GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER          = 0x8CD4;
    const int GL_MAX_COLOR_ATTACHMENTS                         = 0x8CDF;
    const int GL_COLOR_ATTACHMENT1                             = 0x8CE1;
    const int GL_COLOR_ATTACHMENT2                             = 0x8CE2;
    const int GL_COLOR_ATTACHMENT3                             = 0x8CE3;
    const int GL_COLOR_ATTACHMENT4                             = 0x8CE4;
    const int GL_COLOR_ATTACHMENT5                             = 0x8CE5;
    const int GL_COLOR_ATTACHMENT6                             = 0x8CE6;
    const int GL_COLOR_ATTACHMENT7                             = 0x8CE7;
    const int GL_COLOR_ATTACHMENT8                             = 0x8CE8;
    const int GL_COLOR_ATTACHMENT9                             = 0x8CE9;
    const int GL_COLOR_ATTACHMENT10                            = 0x8CEA;
    const int GL_COLOR_ATTACHMENT11                            = 0x8CEB;
    const int GL_COLOR_ATTACHMENT12                            = 0x8CEC;
    const int GL_COLOR_ATTACHMENT13                            = 0x8CED;
    const int GL_COLOR_ATTACHMENT14                            = 0x8CEE;
    const int GL_COLOR_ATTACHMENT15                            = 0x8CEF;

    // ------------------------------------------------------------------------
    const int GL_MAJOR_VERSION                         = 0x821B;
    const int GL_MINOR_VERSION                         = 0x821C;
    const int GL_NUM_EXTENSIONS                        = 0x821D;
    const int GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x8210;
    const int GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x8211;
    const int GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE       = 0x8212;
    const int GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE     = 0x8213;
    const int GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE      = 0x8214;
    const int GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE     = 0x8215;
    const int GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE     = 0x8216;
    const int GL_FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE   = 0x8217;
    const int GL_FRAMEBUFFER_DEFAULT                   = 0x8218;
    const int GL_FRAMEBUFFER_UNDEFINED                 = 0x8219;
    const int GL_DEPTH_STENCIL_ATTACHMENT              = 0x821A;
    const int GL_RG                                    = 0x8227;
    const int GL_RG_INTEGER                            = 0x8228;
    const int GL_R8                                    = 0x8229;
    const int GL_RG8                                   = 0x822B;
    const int GL_R16_F                                 = 0x822D;
    const int GL_R32_F                                 = 0x822E;
    const int GL_RG16_F                                = 0x822F;
    const int GL_RG32_F                                = 0x8230;
    const int GL_R8_I                                  = 0x8231;
    const int GL_R8_UI                                 = 0x8232;
    const int GL_R16_I                                 = 0x8233;
    const int GL_R16_UI                                = 0x8234;
    const int GL_R32_I                                 = 0x8235;
    const int GL_R32_UI                                = 0x8236;
    const int GL_RG8_I                                 = 0x8237;
    const int GL_RG8_UI                                = 0x8238;
    const int GL_RG16_I                                = 0x8239;
    const int GL_RG16_UI                               = 0x823A;
    const int GL_RG32_I                                = 0x823B;
    const int GL_RG32_UI                               = 0x823C;

    // ------------------------------------------------------------------------
    const int GL_MIN_PROGRAM_TEXEL_OFFSET = 0x8904;
    const int GL_MAX_PROGRAM_TEXEL_OFFSET = 0x8905;

    // ------------------------------------------------------------------------
    const int GL_RGBA32_UI                      = 0x8D70;
    const int GL_RGB32_UI                       = 0x8D71;
    const int GL_RGBA16_UI                      = 0x8D76;
    const int GL_RGB16_UI                       = 0x8D77;
    const int GL_RGBA8_UI                       = 0x8D7C;
    const int GL_RGB8_UI                        = 0x8D7D;
    const int GL_RGBA32_I                       = 0x8D82;
    const int GL_RGB32_I                        = 0x8D83;
    const int GL_RGBA16_I                       = 0x8D88;
    const int GL_RGB16_I                        = 0x8D89;
    const int GL_RGBA8_I                        = 0x8D8E;
    const int GL_RGB8_I                         = 0x8D8F;
    const int GL_RED_INTEGER                    = 0x8D94;
    const int GL_RGB_INTEGER                    = 0x8D98;
    const int GL_RGBA_INTEGER                   = 0x8D99;
    const int GL_SAMPLER_2D_ARRAY               = 0x8DC1;
    const int GL_SAMPLER_2D_ARRAY_SHADOW        = 0x8DC4;
    const int GL_SAMPLER_CUBE_SHADOW            = 0x8DC5;
    const int GL_UNSIGNED_INT_VEC2              = 0x8DC6;
    const int GL_UNSIGNED_INT_VEC3              = 0x8DC7;
    const int GL_UNSIGNED_INT_VEC4              = 0x8DC8;
    const int GL_INT_SAMPLER_2D                 = 0x8DCA;
    const int GL_INT_SAMPLER_3D                 = 0x8DCB;
    const int GL_INT_SAMPLER_CUBE               = 0x8DCC;
    const int GL_INT_SAMPLER_2D_ARRAY           = 0x8DCF;
    const int GL_UNSIGNED_INT_SAMPLER_2D        = 0x8DD2;
    const int GL_UNSIGNED_INT_SAMPLER_3D        = 0x8DD3;
    const int GL_UNSIGNED_INT_SAMPLER_CUBE      = 0x8DD4;
    const int GL_UNSIGNED_INT_SAMPLER_2D_ARRAY  = 0x8DD7;
    const int GL_FLOAT_32_UNSIGNED_INT_24_8_REV = 0x8DAD;

    // ------------------------------------------------------------------------
    const int GL_BUFFER_ACCESS_FLAGS = 0x911F;
    const int GL_BUFFER_MAP_LENGTH   = 0x9120;
    const int GL_BUFFER_MAP_OFFSET   = 0x9121;

    // ------------------------------------------------------------------------
    const int GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
    const int GL_MAX_SAMPLES                        = 0x8D57;

    const int GL_HALF_FLOAT = 0x140B;


    const int GL_VERTEX_ARRAY_BINDING          = 0x85B5;
    const int GL_R8_SNORM                      = 0x8F94;
    const int GL_RG8_SNORM                     = 0x8F95;
    const int GL_RGB8_SNORM                    = 0x8F96;
    const int GL_RGBA8_SNORM                   = 0x8F97;
    const int GL_SIGNED_NORMALIZED             = 0x8F9C;
    const int GL_PRIMITIVE_RESTART_FIXED_INDEX = 0x8D69;
    const int GL_COPY_READ_BUFFER              = 0x8F36;
    const int GL_COPY_WRITE_BUFFER             = 0x8F37;
    const int GL_COPY_READ_BUFFER_BINDING      = GL_COPY_READ_BUFFER;
    const int GL_COPY_WRITE_BUFFER_BINDING     = GL_COPY_WRITE_BUFFER;

    // ------------------------------------------------------------------------
    const int GL_UNIFORM_BUFFER                              = 0x8A11;
    const int GL_UNIFORM_BUFFER_BINDING                      = 0x8A28;
    const int GL_UNIFORM_BUFFER_START                        = 0x8A29;
    const int GL_UNIFORM_BUFFER_SIZE                         = 0x8A2A;
    const int GL_MAX_VERTEX_UNIFORM_BLOCKS                   = 0x8A2B;
    const int GL_MAX_FRAGMENT_UNIFORM_BLOCKS                 = 0x8A2D;
    const int GL_MAX_COMBINED_UNIFORM_BLOCKS                 = 0x8A2E;
    const int GL_MAX_UNIFORM_BUFFER_BINDINGS                 = 0x8A2F;
    const int GL_MAX_UNIFORM_BLOCK_SIZE                      = 0x8A30;
    const int GL_MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS      = 0x8A31;
    const int GL_MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS    = 0x8A33;
    const int GL_UNIFORM_BUFFER_OFFSET_ALIGNMENT             = 0x8A34;
    const int GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH        = 0x8A35;
    const int GL_ACTIVE_UNIFORM_BLOCKS                       = 0x8A36;
    const int GL_UNIFORM_TYPE                                = 0x8A37;
    const int GL_UNIFORM_SIZE                                = 0x8A38;
    const int GL_UNIFORM_NAME_LENGTH                         = 0x8A39;
    const int GL_UNIFORM_BLOCK_INDEX                         = 0x8A3A;
    const int GL_UNIFORM_OFFSET                              = 0x8A3B;
    const int GL_UNIFORM_ARRAY_STRIDE                        = 0x8A3C;
    const int GL_UNIFORM_MATRIX_STRIDE                       = 0x8A3D;
    const int GL_UNIFORM_IS_ROW_MAJOR                        = 0x8A3E;
    const int GL_UNIFORM_BLOCK_BINDING                       = 0x8A3F;
    const int GL_UNIFORM_BLOCK_DATA_SIZE                     = 0x8A40;
    const int GL_UNIFORM_BLOCK_NAME_LENGTH                   = 0x8A41;
    const int GL_UNIFORM_BLOCK_ACTIVE_UNIFORMS               = 0x8A42;
    const int GL_UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES        = 0x8A43;
    const int GL_UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER   = 0x8A44;
    const int GL_UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER = 0x8A46;

    // ------------------------------------------------------------------------
    // GL_INVALID_INDEX is defined as 0xFFFFFFFFu in C.
    const int GL_INVALID_INDEX                 = -1;
    const int GL_MAX_VERTEX_OUTPUT_COMPONENTS  = 0x9122;
    const int GL_MAX_FRAGMENT_INPUT_COMPONENTS = 0x9125;
    const int GL_MAX_SERVER_WAIT_TIMEOUT       = 0x9111;
    const int GL_OBJECT_TYPE                   = 0x9112;
    const int GL_SYNC_CONDITION                = 0x9113;
    const int GL_SYNC_STATUS                   = 0x9114;
    const int GL_SYNC_FLAGS                    = 0x9115;
    const int GL_SYNC_FENCE                    = 0x9116;
    const int GL_SYNC_GPU_COMMANDS_COMPLETE    = 0x9117;
    const int GL_UNSIGNALED                    = 0x9118;
    const int GL_SIGNALED                      = 0x9119;
    const int GL_ALREADY_SIGNALED              = 0x911A;
    const int GL_TIMEOUT_EXPIRED               = 0x911B;
    const int GL_CONDITION_SATISFIED           = 0x911C;
    const int GL_WAIT_FAILED                   = 0x911D;

    // GL_TIMEOUT_IGNORED is defined as 0xFFFFFFFFFFFFFFFFull in C.
    const long GL_TIMEOUT_IGNORED                           = -1;
    const int  GL_VERTEX_ATTRIB_ARRAY_DIVISOR               = 0x88FE;
    const int  GL_ANY_SAMPLES_PASSED                        = 0x8C2F;
    const int  GL_ANY_SAMPLES_PASSED_CONSERVATIVE           = 0x8D6A;
    const int  GL_SAMPLER_BINDING                           = 0x8919;
    const int  GL_RGB10_A2_UI                               = 0x906F;
    const int  GL_TEXTURE_SWIZZLE_R                         = 0x8E42;
    const int  GL_TEXTURE_SWIZZLE_G                         = 0x8E43;
    const int  GL_TEXTURE_SWIZZLE_B                         = 0x8E44;
    const int  GL_TEXTURE_SWIZZLE_A                         = 0x8E45;
    const int  GL_GREEN                                     = 0x1904;
    const int  GL_BLUE                                      = 0x1905;
    const int  GL_INT_2_10_10_10_REV                        = 0x8D9F;
    const int  GL_TRANSFORM_FEEDBACK                        = 0x8E22;
    const int  GL_TRANSFORM_FEEDBACK_PAUSED                 = 0x8E23;
    const int  GL_TRANSFORM_FEEDBACK_ACTIVE                 = 0x8E24;
    const int  GL_TRANSFORM_FEEDBACK_BINDING                = 0x8E25;
    const int  GL_PROGRAM_BINARY_RETRIEVABLE_HINT           = 0x8257;
    const int  GL_PROGRAM_BINARY_LENGTH                     = 0x8741;
    const int  GL_NUM_PROGRAM_BINARY_FORMATS                = 0x87FE;
    const int  GL_PROGRAM_BINARY_FORMATS                    = 0x87FF;
    const int  GL_COMPRESSED_R11_EAC                        = 0x9270;
    const int  GL_COMPRESSED_SIGNED_R11_EAC                 = 0x9271;
    const int  GL_COMPRESSED_RG11_EAC                       = 0x9272;
    const int  GL_COMPRESSED_SIGNED_RG11_EAC                = 0x9273;
    const int  GL_COMPRESSED_RGB8_ETC2                      = 0x9274;
    const int  GL_COMPRESSED_SRGB8_ETC2                     = 0x9275;
    const int  GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2  = 0x9276;
    const int  GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9277;
    const int  GL_COMPRESSED_RGBA8_ETC2_EAC                 = 0x9278;
    const int  GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC          = 0x9279;
    const int  GL_TEXTURE_IMMUTABLE_FORMAT                  = 0x912F;
    const int  GL_MAX_ELEMENT_INDEX                         = 0x8D6B;
    const int  GL_NUM_SAMPLE_COUNTS                         = 0x9380;
    const int  GL_TEXTURE_IMMUTABLE_LEVELS                  = 0x82DF;

    // ------------------------------------------------------------------------
    //
    // ------------------------------------------------------------------------


    // ------------------------------------------------------------------------
    const int GL_FRONT_LEFT  = 0x0400;
    const int GL_FRONT_RIGHT = 0x0401;
    const int GL_BACK_LEFT   = 0x0402;
    const int GL_BACK_RIGHT  = 0x0403;
    const int GL_LEFT        = 0x0406;
    const int GL_RIGHT       = 0x0407;

    // ------------------------------------------------------------------------
    const int GL_POINT_SIZE             = 0x0B11;
    const int GL_POINT_SIZE_RANGE       = 0x0B12;
    const int GL_POINT_SIZE_GRANULARITY = 0x0B13;
    const int GL_LINE_SMOOTH            = 0x0B20;
    const int GL_LINE_WIDTH_RANGE       = 0x0B22;
    const int GL_LINE_WIDTH_GRANULARITY = 0x0B23;
    const int GL_POLYGON_MODE           = 0x0B40;
    const int GL_POLYGON_SMOOTH         = 0x0B41;
    const int GL_BLEND_DST              = 0x0BE0;
    const int GL_BLEND_SRC              = 0x0BE1;
    const int GL_LOGIC_OP_MODE          = 0x0BF0;

    // ------------------------------------------------------------------------
    const int GL_DRAW_BUFFER         = 0x0C01;
    const int GL_DOUBLEBUFFER        = 0x0C32;
    const int GL_STEREO              = 0x0C33;
    const int GL_LINE_SMOOTH_HINT    = 0x0C52;
    const int GL_POLYGON_SMOOTH_HINT = 0x0C53;
    const int GL_UNPACK_SWAP_BYTES   = 0x0CF0;
    const int GL_UNPACK_LSB_FIRST    = 0x0CF1;

    // ------------------------------------------------------------------------
    const int GL_PACK_SWAP_BYTES = 0x0D00;
    const int GL_PACK_LSB_FIRST  = 0x0D01;
    const int GL_TEXTURE_1D      = 0x0DE0;

    // ------------------------------------------------------------------------
    const int GL_TEXTURE_WIDTH        = 0x1000;
    const int GL_TEXTURE_HEIGHT       = 0x1001;
    const int GL_TEXTURE_BORDER_COLOR = 0x1004;

    // ------------------------------------------------------------------------
    const int GL_STACK_OVERFLOW  = 0x0503;
    const int GL_STACK_UNDERFLOW = 0x0504;

    // ------------------------------------------------------------------------
    const int GL_CLEAR         = 0x1500;
    const int GL_AND           = 0x1501;
    const int GL_AND_REVERSE   = 0x1502;
    const int GL_COPY          = 0x1503;
    const int GL_AND_INVERTED  = 0x1504;
    const int GL_NOOP          = 0x1505;
    const int GL_XOR           = 0x1506;
    const int GL_OR            = 0x1507;
    const int GL_NOR           = 0x1508;
    const int GL_EQUIV         = 0x1509;
    const int GL_OR_REVERSE    = 0x150B;
    const int GL_COPY_INVERTED = 0x150C;
    const int GL_OR_INVERTED   = 0x150D;
    const int GL_NAND          = 0x150E;
    const int GL_SET           = 0x150F;

    // ------------------------------------------------------------------------
    const int GL_POINT = 0x1B00;
    const int GL_LINE  = 0x1B01;
    const int GL_FILL  = 0x1B02;

    // ------------------------------------------------------------------------
    const int GL_COLOR_LOGIC_OP = 0x0BF2;

    // ------------------------------------------------------------------------
    const int GL_POLYGON_OFFSET_POINT = 0x2A01;
    const int GL_POLYGON_OFFSET_LINE  = 0x2A02;

    // ------------------------------------------------------------------------
    const int GL_TEXTURE_BINDING_1D      = 0x8068;
    const int GL_TEXTURE_RED_SIZE        = 0x805C;
    const int GL_TEXTURE_GREEN_SIZE      = 0x805D;
    const int GL_TEXTURE_BLUE_SIZE       = 0x805E;
    const int GL_TEXTURE_ALPHA_SIZE      = 0x805F;
    const int GL_PROXY_TEXTURE_1D        = 0x8063;
    const int GL_PROXY_TEXTURE_2D        = 0x8064;
    const int GL_RGB4                    = 0x804F;
    const int GL_RGB5                    = 0x8050;
    const int GL_RGB10                   = 0x8052;
    const int GL_RGB12                   = 0x8053;
    const int GL_RGB16                   = 0x8054;
    const int GL_RGBA2                   = 0x8055;
    const int GL_RGBA12                  = 0x805A;
    const int GL_RGBA16                  = 0x805B;
    const int GL_VERTEX_ARRAY            = 0x8074;
    const int GL_UNSIGNED_BYTE_3_3_2     = 0x8032;
    const int GL_UNSIGNED_INT_8_8_8_8    = 0x8035;
    const int GL_UNSIGNED_INT_10_10_10_2 = 0x8036;
    const int GL_PACK_SKIP_IMAGES        = 0x806B;
    const int GL_PACK_IMAGE_HEIGHT       = 0x806C;
    const int GL_PROXY_TEXTURE_3D        = 0x8070;
    const int GL_TEXTURE_DEPTH           = 0x8071;
    const int GL_BGR                     = 0x80E0;
    const int GL_BGRA                    = 0x80E1;

    // ------------------------------------------------------------------------
    const int GL_TEXTURE_INTERNAL_FORMAT       = 0x1003;
    const int GL_DOUBLE                        = 0x140A;
    const int GL_R3_G3_B2                      = 0x2A10;
    const int GL_UNSIGNED_BYTE_2_3_3_REV       = 0x8362;
    const int GL_UNSIGNED_SHORT_5_6_5_REV      = 0x8364;
    const int GL_UNSIGNED_SHORT_4_4_4_4_REV    = 0x8365;
    const int GL_UNSIGNED_SHORT_1_5_5_5_REV    = 0x8366;
    const int GL_UNSIGNED_INT_8_8_8_8_REV      = 0x8367;
    const int GL_SMOOTH_POINT_SIZE_RANGE       = 0x0B12;
    const int GL_SMOOTH_POINT_SIZE_GRANULARITY = 0x0B13;
    const int GL_SMOOTH_LINE_WIDTH_RANGE       = 0x0B22;
    const int GL_SMOOTH_LINE_WIDTH_GRANULARITY = 0x0B23;
    const int GL_MULTISAMPLE                   = 0x809D;
    const int GL_SAMPLE_ALPHA_TO_ONE           = 0x809F;
    const int GL_PROXY_TEXTURE_CUBE_MAP        = 0x851B;
    const int GL_COMPRESSED_RGB                = 0x84ED;
    const int GL_COMPRESSED_RGBA               = 0x84EE;
    const int GL_TEXTURE_COMPRESSION_HINT      = 0x84EF;
    const int GL_TEXTURE_COMPRESSED_IMAGE_SIZE = 0x86A0;
    const int GL_TEXTURE_COMPRESSED            = 0x86A1;
    const int GL_CLAMP_TO_BORDER               = 0x812D;
    const int GL_POINT_FADE_THRESHOLD_SIZE     = 0x8128;
    const int GL_DEPTH_COMPONENT32             = 0x81A7;
    const int GL_TEXTURE_LOD_BIAS              = 0x8501;
    const int GL_TEXTURE_DEPTH_SIZE            = 0x884A;
    const int GL_QUERY_COUNTER_BITS            = 0x8864;
    const int GL_READ_ONLY                     = 0x88B8;
    const int GL_WRITE_ONLY                    = 0x88B9;
    const int GL_READ_WRITE                    = 0x88BA;
    const int GL_BUFFER_ACCESS                 = 0x88BB;
    const int GL_SAMPLES_PASSED                = 0x8914;
    const int GL_SRC1_ALPHA                    = 0x8589;

    // ------------------------------------------------------------------------
    const int GL_MAX_VARYING_FLOATS = 0x8B4B;
    const int GL_SAMPLER_1D         = 0x8B5D;
    const int GL_SAMPLER_1D_SHADOW  = 0x8B61;

    // ------------------------------------------------------------------------
    const int GL_POINT_SPRITE_COORD_ORIGIN = 0x8CA0;
    const int GL_LOWER_LEFT                = 0x8CA1;
    const int GL_UPPER_LEFT                = 0x8CA2;
    const int GL_SRGB_ALPHA                = 0x8C42;
    const int GL_COMPRESSED_SRGB           = 0x8C48;
    const int GL_COMPRESSED_SRGB_ALPHA     = 0x8C49;

    // ------------------------------------------------------------------------
    const int GL_CLIP_DISTANCE0 = 0x3000;
    const int GL_CLIP_DISTANCE1 = 0x3001;
    const int GL_CLIP_DISTANCE2 = 0x3002;
    const int GL_CLIP_DISTANCE3 = 0x3003;
    const int GL_CLIP_DISTANCE4 = 0x3004;
    const int GL_CLIP_DISTANCE5 = 0x3005;
    const int GL_CLIP_DISTANCE6 = 0x3006;
    const int GL_CLIP_DISTANCE7 = 0x3007;

    // ------------------------------------------------------------------------
    const int GL_MAX_CLIP_DISTANCES                                         = 0x0D32;
    const int GL_CONTEXT_FLAGS                                              = 0x821E;
    const int GL_COMPRESSED_RED                                             = 0x8225;
    const int GL_COMPRESSED_RG                                              = 0x8226;
    const int GL_CLAMP_READ_COLOR                                           = 0x891C;
    const int GL_FIXED_ONLY                                                 = 0x891D;
    const int GL_TEXTURE_1D_ARRAY                                           = 0x8C18;
    const int GL_PROXY_TEXTURE_1D_ARRAY                                     = 0x8C19;
    const int GL_PROXY_TEXTURE_2D_ARRAY                                     = 0x8C1B;
    const int GL_TEXTURE_BINDING_1D_ARRAY                                   = 0x8C1C;
    const int GL_TEXTURE_SHARED_SIZE                                        = 0x8C3F;
    const int GL_PRIMITIVES_GENERATED                                       = 0x8C87;
    const int GL_GREEN_INTEGER                                              = 0x8D95;
    const int GL_BLUE_INTEGER                                               = 0x8D96;
    const int GL_BGR_INTEGER                                                = 0x8D9A;
    const int GL_BGRA_INTEGER                                               = 0x8D9B;
    const int GL_SAMPLER_1D_ARRAY                                           = 0x8DC0;
    const int GL_SAMPLER_1D_ARRAY_SHADOW                                    = 0x8DC3;
    const int GL_INT_SAMPLER_1D                                             = 0x8DC9;
    const int GL_INT_SAMPLER_1D_ARRAY                                       = 0x8DCE;
    const int GL_UNSIGNED_INT_SAMPLER_1D                                    = 0x8DD1;
    const int GL_UNSIGNED_INT_SAMPLER_1D_ARRAY                              = 0x8DD6;
    const int GL_QUERY_WAIT                                                 = 0x8E13;
    const int GL_QUERY_NO_WAIT                                              = 0x8E14;
    const int GL_QUERY_BY_REGION_WAIT                                       = 0x8E15;
    const int GL_QUERY_BY_REGION_NO_WAIT                                    = 0x8E16;
    const int GL_TEXTURE_STENCIL_SIZE                                       = 0x88F1;
    const int GL_TEXTURE_RED_TYPE                                           = 0x8C10;
    const int GL_TEXTURE_GREEN_TYPE                                         = 0x8C11;
    const int GL_TEXTURE_BLUE_TYPE                                          = 0x8C12;
    const int GL_TEXTURE_ALPHA_TYPE                                         = 0x8C13;
    const int GL_TEXTURE_DEPTH_TYPE                                         = 0x8C16;
    const int GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER                         = 0x8CDB;
    const int GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER                         = 0x8CDC;
    const int GL_COLOR_ATTACHMENT16                                         = 0x8CF0;
    const int GL_COLOR_ATTACHMENT17                                         = 0x8CF1;
    const int GL_COLOR_ATTACHMENT18                                         = 0x8CF2;
    const int GL_COLOR_ATTACHMENT19                                         = 0x8CF3;
    const int GL_COLOR_ATTACHMENT20                                         = 0x8CF4;
    const int GL_COLOR_ATTACHMENT21                                         = 0x8CF5;
    const int GL_COLOR_ATTACHMENT22                                         = 0x8CF6;
    const int GL_COLOR_ATTACHMENT23                                         = 0x8CF7;
    const int GL_COLOR_ATTACHMENT24                                         = 0x8CF8;
    const int GL_COLOR_ATTACHMENT25                                         = 0x8CF9;
    const int GL_COLOR_ATTACHMENT26                                         = 0x8CFA;
    const int GL_COLOR_ATTACHMENT27                                         = 0x8CFB;
    const int GL_COLOR_ATTACHMENT28                                         = 0x8CFC;
    const int GL_COLOR_ATTACHMENT29                                         = 0x8CFD;
    const int GL_COLOR_ATTACHMENT30                                         = 0x8CFE;
    const int GL_COLOR_ATTACHMENT31                                         = 0x8CFF;
    const int GL_STENCIL_INDEX1                                             = 0x8D46;
    const int GL_STENCIL_INDEX4                                             = 0x8D47;
    const int GL_STENCIL_INDEX16                                            = 0x8D49;
    const int GL_FRAMEBUFFER_SRGB                                           = 0x8DB9;
    const int GL_COMPRESSED_RED_RGTC1                                       = 0x8DBB;
    const int GL_COMPRESSED_SIGNED_RED_RGTC1                                = 0x8DBC;
    const int GL_COMPRESSED_RG_RGTC2                                        = 0x8DBD;
    const int GL_COMPRESSED_SIGNED_RG_RGTC2                                 = 0x8DBE;
    const int GL_R16                                                        = 0x822A;
    const int GL_RG16                                                       = 0x822C;
    const int GL_SAMPLER_2D_RECT                                            = 0x8B63;
    const int GL_SAMPLER_2D_RECT_SHADOW                                     = 0x8B64;
    const int GL_SAMPLER_BUFFER                                             = 0x8DC2;
    const int GL_INT_SAMPLER_2D_RECT                                        = 0x8DCD;
    const int GL_INT_SAMPLER_BUFFER                                         = 0x8DD0;
    const int GL_UNSIGNED_INT_SAMPLER_2D_RECT                               = 0x8DD5;
    const int GL_UNSIGNED_INT_SAMPLER_BUFFER                                = 0x8DD8;
    const int GL_TEXTURE_BUFFER                                             = 0x8C2A;
    const int GL_MAX_TEXTURE_BUFFER_SIZE                                    = 0x8C2B;
    const int GL_TEXTURE_BINDING_BUFFER                                     = 0x8C2C;
    const int GL_TEXTURE_BUFFER_DATA_STORE_BINDING                          = 0x8C2D;
    const int GL_TEXTURE_RECTANGLE                                          = 0x84F5;
    const int GL_TEXTURE_BINDING_RECTANGLE                                  = 0x84F6;
    const int GL_PROXY_TEXTURE_RECTANGLE                                    = 0x84F7;
    const int GL_MAX_RECTANGLE_TEXTURE_SIZE                                 = 0x84F8;
    const int GL_R16_SNORM                                                  = 0x8F98;
    const int GL_RG16_SNORM                                                 = 0x8F99;
    const int GL_RGB16_SNORM                                                = 0x8F9A;
    const int GL_RGBA16_SNORM                                               = 0x8F9B;
    const int GL_PRIMITIVE_RESTART                                          = 0x8F9D;
    const int GL_PRIMITIVE_RESTART_INDEX                                    = 0x8F9E;
    const int GL_MAX_GEOMETRY_UNIFORM_BLOCKS                                = 0x8A2C;
    const int GL_MAX_COMBINED_GEOMETRY_UNIFORM_COMPONENTS                   = 0x8A32;
    const int GL_UNIFORM_BLOCK_REFERENCED_BY_GEOMETRY_SHADER                = 0x8A45;
    const int GL_LINES_ADJACENCY                                            = 0x000A;
    const int GL_LINE_STRIP_ADJACENCY                                       = 0x000B;
    const int GL_TRIANGLES_ADJACENCY                                        = 0x000C;
    const int GL_TRIANGLE_STRIP_ADJACENCY                                   = 0x000D;
    const int GL_PROGRAM_POINT_SIZE                                         = 0x8642;
    const int GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS                           = 0x8C29;
    const int GL_FRAMEBUFFER_ATTACHMENT_LAYERED                             = 0x8DA7;
    const int GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS                       = 0x8DA8;
    const int GL_GEOMETRY_SHADER                                            = 0x8DD9;
    const int GL_GEOMETRY_VERTICES_OUT                                      = 0x8916;
    const int GL_GEOMETRY_INPUT_TYPE                                        = 0x8917;
    const int GL_GEOMETRY_OUTPUT_TYPE                                       = 0x8918;
    const int GL_MAX_GEOMETRY_UNIFORM_COMPONENTS                            = 0x8DDF;
    const int GL_MAX_GEOMETRY_OUTPUT_VERTICES                               = 0x8DE0;
    const int GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS                       = 0x8DE1;
    const int GL_MAX_GEOMETRY_INPUT_COMPONENTS                              = 0x9123;
    const int GL_MAX_GEOMETRY_OUTPUT_COMPONENTS                             = 0x9124;
    const int GL_CONTEXT_PROFILE_MASK                                       = 0x9126;
    const int GL_DEPTH_CLAMP                                                = 0x864F;
    const int GL_QUADS_FOLLOW_PROVOKING_VERTEX_CONVENTION                   = 0x8E4C;
    const int GL_FIRST_VERTEX_CONVENTION                                    = 0x8E4D;
    const int GL_LAST_VERTEX_CONVENTION                                     = 0x8E4E;
    const int GL_PROVOKING_VERTEX                                           = 0x8E4F;
    const int GL_TEXTURE_CUBE_MAP_SEAMLESS                                  = 0x884F;
    const int GL_SAMPLE_POSITION                                            = 0x8E50;
    const int GL_SAMPLE_MASK                                                = 0x8E51;
    const int GL_SAMPLE_MASK_VALUE                                          = 0x8E52;
    const int GL_MAX_SAMPLE_MASK_WORDS                                      = 0x8E59;
    const int GL_TEXTURE_2D_MULTISAMPLE                                     = 0x9100;
    const int GL_PROXY_TEXTURE_2D_MULTISAMPLE                               = 0x9101;
    const int GL_TEXTURE_2D_MULTISAMPLE_ARRAY                               = 0x9102;
    const int GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY                         = 0x9103;
    const int GL_TEXTURE_BINDING_2D_MULTISAMPLE                             = 0x9104;
    const int GL_TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY                       = 0x9105;
    const int GL_TEXTURE_SAMPLES                                            = 0x9106;
    const int GL_TEXTURE_FIXED_SAMPLE_LOCATIONS                             = 0x9107;
    const int GL_SAMPLER_2D_MULTISAMPLE                                     = 0x9108;
    const int GL_INT_SAMPLER_2D_MULTISAMPLE                                 = 0x9109;
    const int GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE                        = 0x910A;
    const int GL_SAMPLER_2D_MULTISAMPLE_ARRAY                               = 0x910B;
    const int GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY                           = 0x910C;
    const int GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY                  = 0x910D;
    const int GL_MAX_COLOR_TEXTURE_SAMPLES                                  = 0x910E;
    const int GL_MAX_DEPTH_TEXTURE_SAMPLES                                  = 0x910F;
    const int GL_MAX_INTEGER_SAMPLES                                        = 0x9110;
    const int GL_SRC1_COLOR                                                 = 0x88F9;
    const int GL_ONE_MINUS_SRC1_COLOR                                       = 0x88FA;
    const int GL_ONE_MINUS_SRC1_ALPHA                                       = 0x88FB;
    const int GL_MAX_DUAL_SOURCE_DRAW_BUFFERS                               = 0x88FC;
    const int GL_TEXTURE_SWIZZLE_RGBA                                       = 0x8E46;
    const int GL_TIME_ELAPSED                                               = 0x88BF;
    const int GL_TIMESTAMP                                                  = 0x8E28;
    const int GL_SAMPLE_SHADING                                             = 0x8C36;
    const int GL_MIN_SAMPLE_SHADING_VALUE                                   = 0x8C37;
    const int GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET                          = 0x8E5E;
    const int GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET                          = 0x8E5F;
    const int GL_TEXTURE_CUBE_MAP_ARRAY                                     = 0x9009;
    const int GL_TEXTURE_BINDING_CUBE_MAP_ARRAY                             = 0x900A;
    const int GL_PROXY_TEXTURE_CUBE_MAP_ARRAY                               = 0x900B;
    const int GL_SAMPLER_CUBE_MAP_ARRAY                                     = 0x900C;
    const int GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW                              = 0x900D;
    const int GL_INT_SAMPLER_CUBE_MAP_ARRAY                                 = 0x900E;
    const int GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY                        = 0x900F;
    const int GL_DRAW_INDIRECT_BUFFER                                       = 0x8F3F;
    const int GL_DRAW_INDIRECT_BUFFER_BINDING                               = 0x8F43;
    const int GL_GEOMETRY_SHADER_INVOCATIONS                                = 0x887F;
    const int GL_MAX_GEOMETRY_SHADER_INVOCATIONS                            = 0x8E5A;
    const int GL_MIN_FRAGMENT_INTERPOLATION_OFFSET                          = 0x8E5B;
    const int GL_MAX_FRAGMENT_INTERPOLATION_OFFSET                          = 0x8E5C;
    const int GL_FRAGMENT_INTERPOLATION_OFFSET_BITS                         = 0x8E5D;
    const int GL_MAX_VERTEX_STREAMS                                         = 0x8E71;
    const int GL_DOUBLE_VEC2                                                = 0x8FFC;
    const int GL_DOUBLE_VEC3                                                = 0x8FFD;
    const int GL_DOUBLE_VEC4                                                = 0x8FFE;
    const int GL_DOUBLE_MAT2                                                = 0x8F46;
    const int GL_DOUBLE_MAT3                                                = 0x8F47;
    const int GL_DOUBLE_MAT4                                                = 0x8F48;
    const int GL_DOUBLE_MAT2_X3                                             = 0x8F49;
    const int GL_DOUBLE_MAT2_X4                                             = 0x8F4A;
    const int GL_DOUBLE_MAT3_X2                                             = 0x8F4B;
    const int GL_DOUBLE_MAT3_X4                                             = 0x8F4C;
    const int GL_DOUBLE_MAT4_X2                                             = 0x8F4D;
    const int GL_DOUBLE_MAT4_X3                                             = 0x8F4E;
    const int GL_ACTIVE_SUBROUTINES                                         = 0x8DE5;
    const int GL_ACTIVE_SUBROUTINE_UNIFORMS                                 = 0x8DE6;
    const int GL_ACTIVE_SUBROUTINE_UNIFORM_LOCATIONS                        = 0x8E47;
    const int GL_ACTIVE_SUBROUTINE_MAX_LENGTH                               = 0x8E48;
    const int GL_ACTIVE_SUBROUTINE_UNIFORM_MAX_LENGTH                       = 0x8E49;
    const int GL_MAX_SUBROUTINES                                            = 0x8DE7;
    const int GL_MAX_SUBROUTINE_UNIFORM_LOCATIONS                           = 0x8DE8;
    const int GL_NUM_COMPATIBLE_SUBROUTINES                                 = 0x8E4A;
    const int GL_COMPATIBLE_SUBROUTINES                                     = 0x8E4B;
    const int GL_PATCHES                                                    = 0x000E;
    const int GL_PATCH_VERTICES                                             = 0x8E72;
    const int GL_PATCH_DEFAULT_INNER_LEVEL                                  = 0x8E73;
    const int GL_PATCH_DEFAULT_OUTER_LEVEL                                  = 0x8E74;
    const int GL_TESS_CONTROL_OUTPUT_VERTICES                               = 0x8E75;
    const int GL_TESS_GEN_MODE                                              = 0x8E76;
    const int GL_TESS_GEN_SPACING                                           = 0x8E77;
    const int GL_TESS_GEN_VERTEX_ORDER                                      = 0x8E78;
    const int GL_TESS_GEN_POINT_MODE                                        = 0x8E79;
    const int GL_ISOLINES                                                   = 0x8E7A;
    const int GL_FRACTIONAL_ODD                                             = 0x8E7B;
    const int GL_FRACTIONAL_EVEN                                            = 0x8E7C;
    const int GL_MAX_PATCH_VERTICES                                         = 0x8E7D;
    const int GL_MAX_TESS_GEN_LEVEL                                         = 0x8E7E;
    const int GL_MAX_TESS_CONTROL_UNIFORM_COMPONENTS                        = 0x8E7F;
    const int GL_MAX_TESS_EVALUATION_UNIFORM_COMPONENTS                     = 0x8E80;
    const int GL_MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS                       = 0x8E81;
    const int GL_MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS                    = 0x8E82;
    const int GL_MAX_TESS_CONTROL_OUTPUT_COMPONENTS                         = 0x8E83;
    const int GL_MAX_TESS_PATCH_COMPONENTS                                  = 0x8E84;
    const int GL_MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS                   = 0x8E85;
    const int GL_MAX_TESS_EVALUATION_OUTPUT_COMPONENTS                      = 0x8E86;
    const int GL_MAX_TESS_CONTROL_UNIFORM_BLOCKS                            = 0x8E89;
    const int GL_MAX_TESS_EVALUATION_UNIFORM_BLOCKS                         = 0x8E8A;
    const int GL_MAX_TESS_CONTROL_INPUT_COMPONENTS                          = 0x886C;
    const int GL_MAX_TESS_EVALUATION_INPUT_COMPONENTS                       = 0x886D;
    const int GL_MAX_COMBINED_TESS_CONTROL_UNIFORM_COMPONENTS               = 0x8E1E;
    const int GL_MAX_COMBINED_TESS_EVALUATION_UNIFORM_COMPONENTS            = 0x8E1F;
    const int GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_CONTROL_SHADER            = 0x84F0;
    const int GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_EVALUATION_SHADER         = 0x84F1;
    const int GL_TESS_EVALUATION_SHADER                                     = 0x8E87;
    const int GL_TESS_CONTROL_SHADER                                        = 0x8E88;
    const int GL_TRANSFORM_FEEDBACK_BUFFER_PAUSED                           = 0x8E23;
    const int GL_TRANSFORM_FEEDBACK_BUFFER_ACTIVE                           = 0x8E24;
    const int GL_MAX_TRANSFORM_FEEDBACK_BUFFERS                             = 0x8E70;
    const int GL_ALL_SHADER_BITS                                            = unchecked( ( int ) 0xFFFFFFFF );
    const int GL_PROGRAM_SEPARABLE                                          = 0x8258;
    const int GL_ACTIVE_PROGRAM                                             = 0x8259;
    const int GL_PROGRAM_PIPELINE_BINDING                                   = 0x825A;
    const int GL_MAX_VIEWPORTS                                              = 0x825B;
    const int GL_VIEWPORT_SUBPIXEL_BITS                                     = 0x825C;
    const int GL_VIEWPORT_BOUNDS_RANGE                                      = 0x825D;
    const int GL_LAYER_PROVOKING_VERTEX                                     = 0x825E;
    const int GL_VIEWPORT_INDEX_PROVOKING_VERTEX                            = 0x825F;
    const int GL_UNDEFINED_VERTEX                                           = 0x8260;
    const int GL_UNPACK_COMPRESSED_BLOCK_WIDTH                              = 0x9127;
    const int GL_UNPACK_COMPRESSED_BLOCK_HEIGHT                             = 0x9128;
    const int GL_UNPACK_COMPRESSED_BLOCK_DEPTH                              = 0x9129;
    const int GL_UNPACK_COMPRESSED_BLOCK_SIZE                               = 0x912A;
    const int GL_PACK_COMPRESSED_BLOCK_WIDTH                                = 0x912B;
    const int GL_PACK_COMPRESSED_BLOCK_HEIGHT                               = 0x912C;
    const int GL_PACK_COMPRESSED_BLOCK_DEPTH                                = 0x912D;
    const int GL_PACK_COMPRESSED_BLOCK_SIZE                                 = 0x912E;
    const int GL_MIN_MAP_BUFFER_ALIGNMENT                                   = 0x90BC;
    const int GL_ATOMIC_COUNTER_BUFFER                                      = 0x92C0;
    const int GL_ATOMIC_COUNTER_BUFFER_BINDING                              = 0x92C1;
    const int GL_ATOMIC_COUNTER_BUFFER_START                                = 0x92C2;
    const int GL_ATOMIC_COUNTER_BUFFER_SIZE                                 = 0x92C3;
    const int GL_ATOMIC_COUNTER_BUFFER_DATA_SIZE                            = 0x92C4;
    const int GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTERS               = 0x92C5;
    const int GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTER_INDICES        = 0x92C6;
    const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_VERTEX_SHADER          = 0x92C7;
    const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_CONTROL_SHADER    = 0x92C8;
    const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_EVALUATION_SHADER = 0x92C9;
    const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_GEOMETRY_SHADER        = 0x92CA;
    const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_FRAGMENT_SHADER        = 0x92CB;
    const int GL_MAX_VERTEX_ATOMIC_COUNTER_BUFFERS                          = 0x92CC;
    const int GL_MAX_TESS_CONTROL_ATOMIC_COUNTER_BUFFERS                    = 0x92CD;
    const int GL_MAX_TESS_EVALUATION_ATOMIC_COUNTER_BUFFERS                 = 0x92CE;
    const int GL_MAX_GEOMETRY_ATOMIC_COUNTER_BUFFERS                        = 0x92CF;
    const int GL_MAX_FRAGMENT_ATOMIC_COUNTER_BUFFERS                        = 0x92D0;
    const int GL_MAX_COMBINED_ATOMIC_COUNTER_BUFFERS                        = 0x92D1;
    const int GL_MAX_VERTEX_ATOMIC_COUNTERS                                 = 0x92D2;
    const int GL_MAX_TESS_CONTROL_ATOMIC_COUNTERS                           = 0x92D3;
    const int GL_MAX_TESS_EVALUATION_ATOMIC_COUNTERS                        = 0x92D4;
    const int GL_MAX_GEOMETRY_ATOMIC_COUNTERS                               = 0x92D5;
    const int GL_MAX_FRAGMENT_ATOMIC_COUNTERS                               = 0x92D6;
    const int GL_MAX_COMBINED_ATOMIC_COUNTERS                               = 0x92D7;
    const int GL_MAX_ATOMIC_COUNTER_BUFFER_SIZE                             = 0x92D8;
    const int GL_MAX_ATOMIC_COUNTER_BUFFER_BINDINGS                         = 0x92DC;
    const int GL_ACTIVE_ATOMIC_COUNTER_BUFFERS                              = 0x92D9;
    const int GL_UNIFORM_ATOMIC_COUNTER_BUFFER_INDEX                        = 0x92DA;
    const int GL_UNSIGNED_INT_ATOMIC_COUNTER                                = 0x92DB;
    const int GL_ALL_BARRIER_BITS                                           = unchecked( ( int ) 0xFFFFFFFF );
    const int GL_MAX_IMAGE_UNITS                                            = 0x8F38;
    const int GL_MAX_COMBINED_IMAGE_UNITS_AND_FRAGMENT_OUTPUTS              = 0x8F39;
    const int GL_IMAGE_BINDING_NAME                                         = 0x8F3A;
    const int GL_IMAGE_BINDING_LEVEL                                        = 0x8F3B;
    const int GL_IMAGE_BINDING_LAYERED                                      = 0x8F3C;
    const int GL_IMAGE_BINDING_LAYER                                        = 0x8F3D;
    const int GL_IMAGE_BINDING_ACCESS                                       = 0x8F3E;
    const int GL_IMAGE_1D                                                   = 0x904C;
    const int GL_IMAGE_2D                                                   = 0x904D;
    const int GL_IMAGE_3D                                                   = 0x904E;
    const int GL_IMAGE_2D_RECT                                              = 0x904F;
    const int GL_IMAGE_CUBE                                                 = 0x9050;
    const int GL_IMAGE_BUFFER                                               = 0x9051;
    const int GL_IMAGE_1D_ARRAY                                             = 0x9052;
    const int GL_IMAGE_2D_ARRAY                                             = 0x9053;
    const int GL_IMAGE_CUBE_MAP_ARRAY                                       = 0x9054;
    const int GL_IMAGE_2D_MULTISAMPLE                                       = 0x9055;
    const int GL_IMAGE_2D_MULTISAMPLE_ARRAY                                 = 0x9056;
    const int GL_INT_IMAGE_1D                                               = 0x9057;
    const int GL_INT_IMAGE_2D                                               = 0x9058;
    const int GL_INT_IMAGE_3D                                               = 0x9059;
    const int GL_INT_IMAGE_2D_RECT                                          = 0x905A;
    const int GL_INT_IMAGE_CUBE                                             = 0x905B;
    const int GL_INT_IMAGE_BUFFER                                           = 0x905C;
    const int GL_INT_IMAGE_1D_ARRAY                                         = 0x905D;
    const int GL_INT_IMAGE_2D_ARRAY                                         = 0x905E;
    const int GL_INT_IMAGE_CUBE_MAP_ARRAY                                   = 0x905F;
    const int GL_INT_IMAGE_2D_MULTISAMPLE                                   = 0x9060;
    const int GL_INT_IMAGE_2D_MULTISAMPLE_ARRAY                             = 0x9061;
    const int GL_UNSIGNED_INT_IMAGE_1D                                      = 0x9062;
    const int GL_UNSIGNED_INT_IMAGE_2D                                      = 0x9063;
    const int GL_UNSIGNED_INT_IMAGE_3D                                      = 0x9064;
    const int GL_UNSIGNED_INT_IMAGE_2D_RECT                                 = 0x9065;
    const int GL_UNSIGNED_INT_IMAGE_CUBE                                    = 0x9066;
    const int GL_UNSIGNED_INT_IMAGE_BUFFER                                  = 0x9067;
    const int GL_UNSIGNED_INT_IMAGE_1D_ARRAY                                = 0x9068;
    const int GL_UNSIGNED_INT_IMAGE_2D_ARRAY                                = 0x9069;
    const int GL_UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY                          = 0x906A;
    const int GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE                          = 0x906B;
    const int GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY                    = 0x906C;
    const int GL_MAX_IMAGE_SAMPLES                                          = 0x906D;
    const int GL_IMAGE_BINDING_FORMAT                                       = 0x906E;
    const int GL_IMAGE_FORMAT_COMPATIBILITY_TYPE                            = 0x90C7;
    const int GL_IMAGE_FORMAT_COMPATIBILITY_BY_SIZE                         = 0x90C8;
    const int GL_IMAGE_FORMAT_COMPATIBILITY_BY_CLASS                        = 0x90C9;
    const int GL_MAX_VERTEX_IMAGE_UNIFORMS                                  = 0x90CA;
    const int GL_MAX_TESS_CONTROL_IMAGE_UNIFORMS                            = 0x90CB;
    const int GL_MAX_TESS_EVALUATION_IMAGE_UNIFORMS                         = 0x90CC;
    const int GL_MAX_GEOMETRY_IMAGE_UNIFORMS                                = 0x90CD;
    const int GL_MAX_FRAGMENT_IMAGE_UNIFORMS                                = 0x90CE;
    const int GL_MAX_COMBINED_IMAGE_UNIFORMS                                = 0x90CF;
    const int GL_COMPRESSED_RGBA_BPTC_UNORM                                 = 0x8E8C;
    const int GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM                           = 0x8E8D;
    const int GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT                           = 0x8E8E;
    const int GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT                         = 0x8E8F;
    const int GL_NUM_SHADING_LANGUAGE_VERSIONS                              = 0x82E9;
    const int GL_VERTEX_ATTRIB_ARRAY_LONG                                   = 0x874E;
    const int GL_COMPUTE_SHADER                                             = 0x91B9;
    const int GL_MAX_COMPUTE_UNIFORM_BLOCKS                                 = 0x91BB;
    const int GL_MAX_COMPUTE_TEXTURE_IMAGE_UNITS                            = 0x91BC;
    const int GL_MAX_COMPUTE_IMAGE_UNIFORMS                                 = 0x91BD;
    const int GL_MAX_COMPUTE_SHARED_MEMORY_SIZE                             = 0x8262;
    const int GL_MAX_COMPUTE_UNIFORM_COMPONENTS                             = 0x8263;
    const int GL_MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS                         = 0x8264;
    const int GL_MAX_COMPUTE_ATOMIC_COUNTERS                                = 0x8265;
    const int GL_MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS                    = 0x8266;
    const int GL_MAX_COMPUTE_WORK_GROUP_INVOCATIONS                         = 0x90EB;
    const int GL_MAX_COMPUTE_WORK_GROUP_COUNT                               = 0x91BE;
    const int GL_MAX_COMPUTE_WORK_GROUP_SIZE                                = 0x91BF;
    const int GL_COMPUTE_WORK_GROUP_SIZE                                    = 0x8267;
    const int GL_UNIFORM_BLOCK_REFERENCED_BY_COMPUTE_SHADER                 = 0x90EC;
    const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_COMPUTE_SHADER         = 0x90ED;
    const int GL_DISPATCH_INDIRECT_BUFFER                                   = 0x90EE;
    const int GL_DISPATCH_INDIRECT_BUFFER_BINDING                           = 0x90EF;
    const int GL_DEBUG_OUTPUT_SYNCHRONOUS                                   = 0x8242;
    const int GL_DEBUG_NEXT_LOGGED_MESSAGE_LENGTH                           = 0x8243;
    const int GL_DEBUG_CALLBACK_FUNCTION                                    = 0x8244;
    const int GL_DEBUG_CALLBACK_USER_PARAM                                  = 0x8245;
    const int GL_DEBUG_SOURCE_API                                           = 0x8246;
    const int GL_DEBUG_SOURCE_WINDOW_SYSTEM                                 = 0x8247;
    const int GL_DEBUG_SOURCE_SHADER_COMPILER                               = 0x8248;
    const int GL_DEBUG_SOURCE_THIRD_PARTY                                   = 0x8249;
    const int GL_DEBUG_SOURCE_APPLICATION                                   = 0x824A;
    const int GL_DEBUG_SOURCE_OTHER                                         = 0x824B;
    const int GL_DEBUG_TYPE_ERROR                                           = 0x824C;
    const int GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR                             = 0x824D;
    const int GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR                              = 0x824E;
    const int GL_DEBUG_TYPE_PORTABILITY                                     = 0x824F;
    const int GL_DEBUG_TYPE_PERFORMANCE                                     = 0x8250;
    const int GL_DEBUG_TYPE_OTHER                                           = 0x8251;
    const int GL_MAX_DEBUG_MESSAGE_LENGTH                                   = 0x9143;
    const int GL_MAX_DEBUG_LOGGED_MESSAGES                                  = 0x9144;
    const int GL_DEBUG_LOGGED_MESSAGES                                      = 0x9145;
    const int GL_DEBUG_SEVERITY_HIGH                                        = 0x9146;
    const int GL_DEBUG_SEVERITY_MEDIUM                                      = 0x9147;
    const int GL_DEBUG_SEVERITY_LOW                                         = 0x9148;
    const int GL_DEBUG_TYPE_MARKER                                          = 0x8268;
    const int GL_DEBUG_TYPE_PUSH_GROUP                                      = 0x8269;
    const int GL_DEBUG_TYPE_POP_GROUP                                       = 0x826A;
    const int GL_DEBUG_SEVERITY_NOTIFICATION                                = 0x826B;
    const int GL_MAX_DEBUG_GROUP_STACK_DEPTH                                = 0x826C;
    const int GL_DEBUG_GROUP_STACK_DEPTH                                    = 0x826D;
    const int GL_BUFFER                                                     = 0x82E0;
    const int GL_SHADER                                                     = 0x82E1;
    const int GL_PROGRAM                                                    = 0x82E2;
    const int GL_QUERY                                                      = 0x82E3;
    const int GL_PROGRAM_PIPELINE                                           = 0x82E4;
    const int GL_SAMPLER                                                    = 0x82E6;
    const int GL_MAX_LABEL_LENGTH                                           = 0x82E8;
    const int GL_DEBUG_OUTPUT                                               = 0x92E0;
    const int GL_MAX_UNIFORM_LOCATIONS                                      = 0x826E;
    const int GL_FRAMEBUFFER_DEFAULT_WIDTH                                  = 0x9310;
    const int GL_FRAMEBUFFER_DEFAULT_HEIGHT                                 = 0x9311;
    const int GL_FRAMEBUFFER_DEFAULT_LAYERS                                 = 0x9312;
    const int GL_FRAMEBUFFER_DEFAULT_SAMPLES                                = 0x9313;
    const int GL_FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS                 = 0x9314;
    const int GL_MAX_FRAMEBUFFER_WIDTH                                      = 0x9315;
    const int GL_MAX_FRAMEBUFFER_HEIGHT                                     = 0x9316;
    const int GL_MAX_FRAMEBUFFER_LAYERS                                     = 0x9317;
    const int GL_MAX_FRAMEBUFFER_SAMPLES                                    = 0x9318;
    const int GL_INTERNALFORMAT_SUPPORTED                                   = 0x826F;
    const int GL_INTERNALFORMAT_PREFERRED                                   = 0x8270;
    const int GL_INTERNALFORMAT_RED_SIZE                                    = 0x8271;
    const int GL_INTERNALFORMAT_GREEN_SIZE                                  = 0x8272;
    const int GL_INTERNALFORMAT_BLUE_SIZE                                   = 0x8273;
    const int GL_INTERNALFORMAT_ALPHA_SIZE                                  = 0x8274;
    const int GL_INTERNALFORMAT_DEPTH_SIZE                                  = 0x8275;
    const int GL_INTERNALFORMAT_STENCIL_SIZE                                = 0x8276;
    const int GL_INTERNALFORMAT_SHARED_SIZE                                 = 0x8277;
    const int GL_INTERNALFORMAT_RED_TYPE                                    = 0x8278;
    const int GL_INTERNALFORMAT_GREEN_TYPE                                  = 0x8279;
    const int GL_INTERNALFORMAT_BLUE_TYPE                                   = 0x827A;
    const int GL_INTERNALFORMAT_ALPHA_TYPE                                  = 0x827B;
    const int GL_INTERNALFORMAT_DEPTH_TYPE                                  = 0x827C;
    const int GL_INTERNALFORMAT_STENCIL_TYPE                                = 0x827D;
    const int GL_MAX_WIDTH                                                  = 0x827E;
    const int GL_MAX_HEIGHT                                                 = 0x827F;
    const int GL_MAX_DEPTH                                                  = 0x8280;
    const int GL_MAX_LAYERS                                                 = 0x8281;
    const int GL_MAX_COMBINED_DIMENSIONS                                    = 0x8282;
    const int GL_COLOR_COMPONENTS                                           = 0x8283;
    const int GL_DEPTH_COMPONENTS                                           = 0x8284;
    const int GL_STENCIL_COMPONENTS                                         = 0x8285;
    const int GL_COLOR_RENDERABLE                                           = 0x8286;
    const int GL_DEPTH_RENDERABLE                                           = 0x8287;
    const int GL_STENCIL_RENDERABLE                                         = 0x8288;
    const int GL_FRAMEBUFFER_RENDERABLE                                     = 0x8289;
    const int GL_FRAMEBUFFER_RENDERABLE_LAYERED                             = 0x828A;
    const int GL_FRAMEBUFFER_BLEND                                          = 0x828B;
    const int GL_READ_PIXELS                                                = 0x828C;
    const int GL_READ_PIXELS_FORMAT                                         = 0x828D;
    const int GL_READ_PIXELS_TYPE                                           = 0x828E;
    const int GL_TEXTURE_IMAGE_FORMAT                                       = 0x828F;
    const int GL_TEXTURE_IMAGE_TYPE                                         = 0x8290;
    const int GL_GET_TEXTURE_IMAGE_FORMAT                                   = 0x8291;
    const int GL_GET_TEXTURE_IMAGE_TYPE                                     = 0x8292;
    const int GL_MIPMAP                                                     = 0x8293;
    const int GL_MANUAL_GENERATE_MIPMAP                                     = 0x8294;
    const int GL_AUTO_GENERATE_MIPMAP                                       = 0x8295;
    const int GL_COLOR_ENCODING                                             = 0x8296;
    const int GL_SRGB_READ                                                  = 0x8297;
    const int GL_SRGB_WRITE                                                 = 0x8298;
    const int GL_FILTER                                                     = 0x829A;
    const int GL_VERTEX_TEXTURE                                             = 0x829B;
    const int GL_TESS_CONTROL_TEXTURE                                       = 0x829C;
    const int GL_TESS_EVALUATION_TEXTURE                                    = 0x829D;
    const int GL_GEOMETRY_TEXTURE                                           = 0x829E;
    const int GL_FRAGMENT_TEXTURE                                           = 0x829F;
    const int GL_COMPUTE_TEXTURE                                            = 0x82A0;
    const int GL_TEXTURE_SHADOW                                             = 0x82A1;
    const int GL_TEXTURE_GATHER                                             = 0x82A2;
    const int GL_TEXTURE_GATHER_SHADOW                                      = 0x82A3;
    const int GL_SHADER_IMAGE_LOAD                                          = 0x82A4;
    const int GL_SHADER_IMAGE_STORE                                         = 0x82A5;
    const int GL_SHADER_IMAGE_ATOMIC                                        = 0x82A6;
    const int GL_IMAGE_TEXEL_SIZE                                           = 0x82A7;
    const int GL_IMAGE_COMPATIBILITY_CLASS                                  = 0x82A8;
    const int GL_IMAGE_PIXEL_FORMAT                                         = 0x82A9;
    const int GL_IMAGE_PIXEL_TYPE                                           = 0x82AA;
    const int GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST                        = 0x82AC;
    const int GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST                      = 0x82AD;
    const int GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE                       = 0x82AE;
    const int GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE                     = 0x82AF;
    const int GL_TEXTURE_COMPRESSED_BLOCK_WIDTH                             = 0x82B1;
    const int GL_TEXTURE_COMPRESSED_BLOCK_HEIGHT                            = 0x82B2;
    const int GL_TEXTURE_COMPRESSED_BLOCK_SIZE                              = 0x82B3;
    const int GL_CLEAR_BUFFER                                               = 0x82B4;
    const int GL_TEXTURE_VIEW                                               = 0x82B5;
    const int GL_VIEW_COMPATIBILITY_CLASS                                   = 0x82B6;
    const int GL_FULL_SUPPORT                                               = 0x82B7;
    const int GL_CAVEAT_SUPPORT                                             = 0x82B8;
    const int GL_IMAGE_CLASS_4_X_32                                         = 0x82B9;
    const int GL_IMAGE_CLASS_2_X_32                                         = 0x82BA;
    const int GL_IMAGE_CLASS_1_X_32                                         = 0x82BB;
    const int GL_IMAGE_CLASS_4_X_16                                         = 0x82BC;
    const int GL_IMAGE_CLASS_2_X_16                                         = 0x82BD;
    const int GL_IMAGE_CLASS_1_X_16                                         = 0x82BE;
    const int GL_IMAGE_CLASS_4_X_8                                          = 0x82BF;
    const int GL_IMAGE_CLASS_2_X_8                                          = 0x82C0;
    const int GL_IMAGE_CLASS_1_X_8                                          = 0x82C1;
    const int GL_IMAGE_CLASS_11_11_10                                       = 0x82C2;
    const int GL_IMAGE_CLASS_10_10_10_2                                     = 0x82C3;
    const int GL_VIEW_CLASS_128_BITS                                        = 0x82C4;
    const int GL_VIEW_CLASS_96_BITS                                         = 0x82C5;
    const int GL_VIEW_CLASS_64_BITS                                         = 0x82C6;
    const int GL_VIEW_CLASS_48_BITS                                         = 0x82C7;
    const int GL_VIEW_CLASS_32_BITS                                         = 0x82C8;
    const int GL_VIEW_CLASS_24_BITS                                         = 0x82C9;
    const int GL_VIEW_CLASS_16_BITS                                         = 0x82CA;
    const int GL_VIEW_CLASS_8_BITS                                          = 0x82CB;
    const int GL_VIEW_CLASS_S3_TC_DXT1_RGB                                  = 0x82CC;
    const int GL_VIEW_CLASS_S3_TC_DXT1_RGBA                                 = 0x82CD;
    const int GL_VIEW_CLASS_S3_TC_DXT3_RGBA                                 = 0x82CE;
    const int GL_VIEW_CLASS_S3_TC_DXT5_RGBA                                 = 0x82CF;
    const int GL_VIEW_CLASS_RGTC1_RED                                       = 0x82D0;
    const int GL_VIEW_CLASS_RGTC2_RG                                        = 0x82D1;
    const int GL_VIEW_CLASS_BPTC_UNORM                                      = 0x82D2;
    const int GL_VIEW_CLASS_BPTC_FLOAT                                      = 0x82D3;
    const int GL_UNIFORM                                                    = 0x92E1;
    const int GL_UNIFORM_BLOCK                                              = 0x92E2;
    const int GL_PROGRAM_INPUT                                              = 0x92E3;
    const int GL_PROGRAM_OUTPUT                                             = 0x92E4;
    const int GL_BUFFER_VARIABLE                                            = 0x92E5;
    const int GL_SHADER_STORAGE_BLOCK                                       = 0x92E6;
    const int GL_VERTEX_SUBROUTINE                                          = 0x92E8;
    const int GL_TESS_CONTROL_SUBROUTINE                                    = 0x92E9;
    const int GL_TESS_EVALUATION_SUBROUTINE                                 = 0x92EA;
    const int GL_GEOMETRY_SUBROUTINE                                        = 0x92EB;
    const int GL_FRAGMENT_SUBROUTINE                                        = 0x92EC;
    const int GL_COMPUTE_SUBROUTINE                                         = 0x92ED;
    const int GL_VERTEX_SUBROUTINE_UNIFORM                                  = 0x92EE;
    const int GL_TESS_CONTROL_SUBROUTINE_UNIFORM                            = 0x92EF;
    const int GL_TESS_EVALUATION_SUBROUTINE_UNIFORM                         = 0x92F0;
    const int GL_GEOMETRY_SUBROUTINE_UNIFORM                                = 0x92F1;
    const int GL_FRAGMENT_SUBROUTINE_UNIFORM                                = 0x92F2;
    const int GL_COMPUTE_SUBROUTINE_UNIFORM                                 = 0x92F3;
    const int GL_TRANSFORM_FEEDBACK_VARYING                                 = 0x92F4;
    const int GL_ACTIVE_RESOURCES                                           = 0x92F5;
    const int GL_MAX_NAME_LENGTH                                            = 0x92F6;
    const int GL_MAX_NUM_ACTIVE_VARIABLES                                   = 0x92F7;
    const int GL_MAX_NUM_COMPATIBLE_SUBROUTINES                             = 0x92F8;
    const int GL_NAME_LENGTH                                                = 0x92F9;
    const int GL_TYPE                                                       = 0x92FA;
    const int GL_ARRAY_SIZE                                                 = 0x92FB;
    const int GL_OFFSET                                                     = 0x92FC;
    const int GL_BLOCK_INDEX                                                = 0x92FD;
    const int GL_ARRAY_STRIDE                                               = 0x92FE;
    const int GL_MATRIX_STRIDE                                              = 0x92FF;
    const int GL_IS_ROW_MAJOR                                               = 0x9300;
    const int GL_ATOMIC_COUNTER_BUFFER_INDEX                                = 0x9301;
    const int GL_BUFFER_BINDING                                             = 0x9302;
    const int GL_BUFFER_DATA_SIZE                                           = 0x9303;
    const int GL_NUM_ACTIVE_VARIABLES                                       = 0x9304;
    const int GL_ACTIVE_VARIABLES                                           = 0x9305;
    const int GL_REFERENCED_BY_VERTEX_SHADER                                = 0x9306;
    const int GL_REFERENCED_BY_TESS_CONTROL_SHADER                          = 0x9307;
    const int GL_REFERENCED_BY_TESS_EVALUATION_SHADER                       = 0x9308;
    const int GL_REFERENCED_BY_GEOMETRY_SHADER                              = 0x9309;
    const int GL_REFERENCED_BY_FRAGMENT_SHADER                              = 0x930A;
    const int GL_REFERENCED_BY_COMPUTE_SHADER                               = 0x930B;
    const int GL_TOP_LEVEL_ARRAY_SIZE                                       = 0x930C;
    const int GL_TOP_LEVEL_ARRAY_STRIDE                                     = 0x930D;
    const int GL_LOCATION                                                   = 0x930E;
    const int GL_LOCATION_INDEX                                             = 0x930F;
    const int GL_IS_PER_PATCH                                               = 0x92E7;
    const int GL_SHADER_STORAGE_BUFFER                                      = 0x90D2;
    const int GL_SHADER_STORAGE_BUFFER_BINDING                              = 0x90D3;
    const int GL_SHADER_STORAGE_BUFFER_START                                = 0x90D4;
    const int GL_SHADER_STORAGE_BUFFER_SIZE                                 = 0x90D5;
    const int GL_MAX_VERTEX_SHADER_STORAGE_BLOCKS                           = 0x90D6;
    const int GL_MAX_GEOMETRY_SHADER_STORAGE_BLOCKS                         = 0x90D7;
    const int GL_MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS                     = 0x90D8;
    const int GL_MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS                  = 0x90D9;
    const int GL_MAX_FRAGMENT_SHADER_STORAGE_BLOCKS                         = 0x90DA;
    const int GL_MAX_COMPUTE_SHADER_STORAGE_BLOCKS                          = 0x90DB;
    const int GL_MAX_COMBINED_SHADER_STORAGE_BLOCKS                         = 0x90DC;
    const int GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS                         = 0x90DD;
    const int GL_MAX_SHADER_STORAGE_BLOCK_SIZE                              = 0x90DE;
    const int GL_SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT                     = 0x90DF;
    const int GL_MAX_COMBINED_SHADER_OUTPUT_RESOURCES                       = 0x8F39;
    const int GL_DEPTH_STENCIL_TEXTURE_MODE                                 = 0x90EA;
    const int GL_TEXTURE_BUFFER_OFFSET                                      = 0x919D;
    const int GL_TEXTURE_BUFFER_SIZE                                        = 0x919E;
    const int GL_TEXTURE_BUFFER_OFFSET_ALIGNMENT                            = 0x919F;
    const int GL_TEXTURE_VIEW_MIN_LEVEL                                     = 0x82DB;
    const int GL_TEXTURE_VIEW_NUM_LEVELS                                    = 0x82DC;
    const int GL_TEXTURE_VIEW_MIN_LAYER                                     = 0x82DD;
    const int GL_TEXTURE_VIEW_NUM_LAYERS                                    = 0x82DE;
    const int GL_VERTEX_ATTRIB_BINDING                                      = 0x82D4;
    const int GL_VERTEX_ATTRIB_RELATIVE_OFFSET                              = 0x82D5;
    const int GL_VERTEX_BINDING_DIVISOR                                     = 0x82D6;
    const int GL_VERTEX_BINDING_OFFSET                                      = 0x82D7;
    const int GL_VERTEX_BINDING_STRIDE                                      = 0x82D8;
    const int GL_MAX_VERTEX_ATTRIB_RELATIVE_OFFSET                          = 0x82D9;
    const int GL_MAX_VERTEX_ATTRIB_BINDINGS                                 = 0x82DA;
    const int GL_VERTEX_BINDING_BUFFER                                      = 0x8F4F;
    const int GL_MAX_VERTEX_ATTRIB_STRIDE                                   = 0x82E5;
    const int GL_PRIMITIVE_RESTART_FOR_PATCHES_SUPPORTED                    = 0x8221;
    const int GL_TEXTURE_BUFFER_BINDING                                     = 0x8C2A;
    const int GL_MAP_PERSISTENT_BIT                                         = 0x0040;
    const int GL_MAP_COHERENT_BIT                                           = 0x0080;
    const int GL_DYNAMIC_STORAGE_BIT                                        = 0x0100;
    const int GL_CLIENT_STORAGE_BIT                                         = 0x0200;
    const int GL_BUFFER_IMMUTABLE_STORAGE                                   = 0x821F;
    const int GL_BUFFER_STORAGE_FLAGS                                       = 0x8220;
    const int GL_CLEAR_TEXTURE                                              = 0x9365;
    const int GL_LOCATION_COMPONENT                                         = 0x934A;
    const int GL_TRANSFORM_FEEDBACK_BUFFER_INDEX                            = 0x934B;
    const int GL_TRANSFORM_FEEDBACK_BUFFER_STRIDE                           = 0x934C;
    const int GL_QUERY_BUFFER                                               = 0x9192;
    const int GL_QUERY_BUFFER_BINDING                                       = 0x9193;
    const int GL_QUERY_RESULT_NO_WAIT                                       = 0x9194;
    const int GL_MIRROR_CLAMP_TO_EDGE                                       = 0x8743;
    const int GL_CONTEXT_LOST                                               = 0x0507;
    const int GL_NEGATIVE_ONE_TO_ONE                                        = 0x935E;
    const int GL_ZERO_TO_ONE                                                = 0x935F;
    const int GL_CLIP_ORIGIN                                                = 0x935C;
    const int GL_CLIP_DEPTH_MODE                                            = 0x935D;
    const int GL_QUERY_WAIT_INVERTED                                        = 0x8E17;
    const int GL_QUERY_NO_WAIT_INVERTED                                     = 0x8E18;
    const int GL_QUERY_BY_REGION_WAIT_INVERTED                              = 0x8E19;
    const int GL_QUERY_BY_REGION_NO_WAIT_INVERTED                           = 0x8E1A;
    const int GL_MAX_CULL_DISTANCES                                         = 0x82F9;
    const int GL_MAX_COMBINED_CLIP_AND_CULL_DISTANCES                       = 0x82FA;
    const int GL_TEXTURE_TARGET                                             = 0x1006;
    const int GL_QUERY_TARGET                                               = 0x82EA;
    const int GL_GUILTY_CONTEXT_RESET                                       = 0x8253;
    const int GL_INNOCENT_CONTEXT_RESET                                     = 0x8254;
    const int GL_UNKNOWN_CONTEXT_RESET                                      = 0x8255;
    const int GL_RESET_NOTIFICATION_STRATEGY                                = 0x8256;
    const int GL_LOSE_CONTEXT_ON_RESET                                      = 0x8252;
    const int GL_NO_RESET_NOTIFICATION                                      = 0x8261;
    const int GL_CONTEXT_RELEASE_BEHAVIOR                                   = 0x82FB;
    const int GL_CONTEXT_RELEASE_BEHAVIOR_FLUSH                             = 0x82FC;
    const int GL_SHADER_BINARY_FORMAT_SPIR_V                                = 0x9551;
    const int GL_SPIR_V_BINARY                                              = 0x9552;
    const int GL_PARAMETER_BUFFER                                           = 0x80EE;
    const int GL_PARAMETER_BUFFER_BINDING                                   = 0x80EF;
    const int GL_VERTICES_SUBMITTED                                         = 0x82EE;
    const int GL_PRIMITIVES_SUBMITTED                                       = 0x82EF;
    const int GL_VERTEX_SHADER_INVOCATIONS                                  = 0x82F0;
    const int GL_TESS_CONTROL_SHADER_PATCHES                                = 0x82F1;
    const int GL_TESS_EVALUATION_SHADER_INVOCATIONS                         = 0x82F2;
    const int GL_GEOMETRY_SHADER_PRIMITIVES_EMITTED                         = 0x82F3;
    const int GL_FRAGMENT_SHADER_INVOCATIONS                                = 0x82F4;
    const int GL_COMPUTE_SHADER_INVOCATIONS                                 = 0x82F5;
    const int GL_CLIPPING_INPUT_PRIMITIVES                                  = 0x82F6;
    const int GL_CLIPPING_OUTPUT_PRIMITIVES                                 = 0x82F7;
    const int GL_POLYGON_OFFSET_CLAMP                                       = 0x8E1B;
    const int GL_SPIR_V_EXTENSIONS                                          = 0x9553;
    const int GL_NUM_SPIR_V_EXTENSIONS                                      = 0x9554;
    const int GL_TEXTURE_MAX_ANISOTROPY                                     = 0x84FE;
    const int GL_MAX_TEXTURE_MAX_ANISOTROPY                                 = 0x84FF;
    const int GL_TRANSFORM_FEEDBACK_OVERFLOW                                = 0x82EC;
    const int GL_TRANSFORM_FEEDBACK_STREAM_OVERFLOW                         = 0x82ED;

    // ------------------------------------------------

    // These are to be replaced by equivalent methods in GL.cs

//    public void GLReadBuffer( int mode );
//
//    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, Buffer indices );
//
//    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, int offset );
//
//    public void GLTexImage3D( int target,
//                              int level,
//                              int internalformat,
//                              int width,
//                              int height,
//                              int depth,
//                              int border,
//                              int format,
//                              int type,
//                              Buffer pixels );
//
//    public void GLTexImage3D( int target,
//                              int level,
//                              int internalformat,
//                              int width,
//                              int height,
//                              int depth,
//                              int border,
//                              int format,
//                              int type,
//                              int offset );
//
//    public void GLTexSubImage3D( int target,
//                                 int level,
//                                 int xoffset,
//                                 int yoffset,
//                                 int zoffset,
//                                 int width,
//                                 int height,
//                                 int depth,
//                                 int format,
//                                 int type,
//                                 Buffer pixels );
//
//    public void GLTexSubImage3D( int target,
//                                 int level,
//                                 int xoffset,
//                                 int yoffset,
//                                 int zoffset,
//                                 int width,
//                                 int height,
//                                 int depth,
//                                 int format,
//                                 int type,
//                                 int offset );
//
//    public void GLCopyTexSubImage3D( int target,
//                                     int level,
//                                     int xoffset,
//                                     int yoffset,
//                                     int zoffset,
//                                     int x,
//                                     int y,
//                                     int width,
//                                     int height );
//
//    public void GLGenQueries( int n, int[] ids, int offset );
//
//    public void GLGenQueries( int n, IntBuffer ids );
//
//    public void GLDeleteQueries( int n, int[] ids, int offset );
//
//    public void GLDeleteQueries( int n, IntBuffer ids );
//
//    public bool GLIsQuery( int id );
//
//    public void GLBeginQuery( int target, int id );
//
//    public void GLEndQuery( int target );
//
//    public void GLGetQueryiv( int target, int pname, IntBuffer param );
//
//    public void GLGetQueryObjectuiv( int id, int pname, IntBuffer param );
//
//    public bool GLUnmapBuffer( int target );
//
//    public Buffer GLGetBufferPointerv( int target, int pname );
//
//    public void GLDrawBuffers( int n, IntBuffer bufs );
//
//    public void GLUniformMatrix2X3Fv( int location, int count, bool transpose, FloatBuffer value );
//
//    public void GLUniformMatrix3X2Fv( int location, int count, bool transpose, FloatBuffer value );
//
//    public void GLUniformMatrix2X4Fv( int location, int count, bool transpose, FloatBuffer value );
//
//    public void GLUniformMatrix4X2Fv( int location, int count, bool transpose, FloatBuffer value );
//
//    public void GLUniformMatrix3X4Fv( int location, int count, bool transpose, FloatBuffer value );
//
//    public void GLUniformMatrix4X3Fv( int location, int count, bool transpose, FloatBuffer value );
//
//    public void GLBlitFramebuffer( int srcX0,
//                                   int srcY0,
//                                   int srcX1,
//                                   int srcY1,
//                                   int dstX0,
//                                   int dstY0,
//                                   int dstX1,
//                                   int dstY1,
//                                   int mask,
//                                   int filter );
//
//    public void GLRenderbufferStorageMultisample( int target,
//                                                  int samples,
//                                                  int internalformat,
//                                                  int width,
//                                                  int height );
//
//    public void GLFramebufferTextureLayer( int target, int attachment, int texture, int level, int layer );
//
//    public Buffer GLMapBufferRange( int target, int offset, int length, int access );
//
//    public void GLFlushMappedBufferRange( int target, int offset, int length );
//
//    public void GLBindVertexArray( int array );
//
//    public void GLDeleteVertexArrays( int n, int[] arrays, int offset );
//
//    public void GLDeleteVertexArrays( int n, IntBuffer arrays );
//
//    public void GLGenVertexArrays( int n, int[] arrays, int offset );
//
//    public void GLGenVertexArrays( int n, IntBuffer arrays );
//
//    public bool GLIsVertexArray( int array );
//
//    public void GLBeginTransformFeedback( int primitiveMode );
//
//    public void GLEndTransformFeedback();
//
//    public void GLBindBufferRange( int target, int index, int buffer, int offset, int size );
//
//    public void GLBindBufferBase( int target, int index, int buffer );
//
//    public void GLTransformFeedbackVaryings( int program, string[] varyings, int bufferMode );
//
//    public void GLVertexAttribIPointer( int index, int size, int type, int stride, int offset );
//
//    public void GLGetVertexAttribIiv( int index, int pname, IntBuffer param );
//
//    public void GLGetVertexAttribIuiv( int index, int pname, IntBuffer parameters );
//
//    public void GLVertexAttribI4I( int index, int x, int y, int z, int w );
//
//    public void GLVertexAttribI4Ui( int index, int x, int y, int z, int w );
//
//    public void GLGetUniformuiv( int program, int location, IntBuffer parameters );
//
//    public int GLGetFragDataLocation( int program, String name );
//
//    public void GLUniform1Uiv( int location, int count, IntBuffer value );
//
//    public void GLUniform3Uiv( int location, int count, IntBuffer value );
//
//    public void GLUniform4Uiv( int location, int count, IntBuffer value );
//
//    public void GLClearBufferiv( int buffer, int drawbuffer, IntBuffer value );
//
//    public void GLClearBufferuiv( int buffer, int drawbuffer, IntBuffer value );
//
//    public void GLClearBufferfv( int buffer, int drawbuffer, FloatBuffer value );
//
//    public void GLClearBufferfi( int buffer, int drawbuffer, float depth, int stencil );
//
//    public string GLGetStringi( int name, int index );
//
//    public void GLCopyBufferSubData( int readTarget,
//                                     int writeTarget,
//                                     int readOffset,
//                                     int writeOffset,
//                                     int size );
//
//    public void GLGetUniformIndices( int program, string[] uniformNames, IntBuffer uniformIndices );
//
//    public void GLGetActiveUniformsiv( int program,
//                                       int uniformCount,
//                                       IntBuffer uniformIndices,
//                                       int pname,
//                                       IntBuffer parameters );
//
//    public int GLGetUniformBlockIndex( int program, string uniformBlockName );
//
//    public void GLGetActiveUniformBlockiv( int program,
//                                           int uniformBlockIndex,
//                                           int pname,
//                                           IntBuffer parameters );
//
//    public void GLGetActiveUniformBlockName( int program,
//                                             int uniformBlockIndex,
//                                             Buffer length,
//                                             Buffer uniformBlockName );
//
//    public string GLGetActiveUniformBlockName( int program, int uniformBlockIndex );
//
//    public void GLUniformBlockBinding( int program, int uniformBlockIndex, int uniformBlockBinding );
//
//    public void GLDrawArraysInstanced( int mode, int first, int count, int instanceCount );
//
//    public void GLDrawElementsInstanced( int mode,
//                                         int count,
//                                         int type,
//                                         int indicesOffset,
//                                         int instanceCount );
//
//    public void GLGetInteger64V( int pname, LongBuffer parameters );
//
//    public void GLGetBufferParameteri64V( int target, int pname, LongBuffer parameters );
//
//    public void GLGenSamplers( int count, int[] samplers, int offset );
//
//    public void GLGenSamplers( int count, IntBuffer samplers );
//
//    public void GLDeleteSamplers( int count, int[] samplers, int offset );
//
//    public void GLDeleteSamplers( int count, IntBuffer samplers );
//
//    public bool GLIsSampler( int sampler );
//
//    public void GLBindSampler( int unit, int sampler );
//
//    public void GLSamplerParameteri( int sampler, int pname, int param );
//
//    public void GLSamplerParameteriv( int sampler, int pname, IntBuffer param );
//
//    public void GLSamplerParameterf( int sampler, int pname, float param );
//
//    public void GLSamplerParameterfv( int sampler, int pname, FloatBuffer param );
//
//    public void GLGetSamplerParameteriv( int sampler, int pname, IntBuffer parameters );
//
//    public void GLGetSamplerParameterfv( int sampler, int pname, FloatBuffer parameters );
//
//    public void GLVertexAttribDivisor( int index, int divisor );
//
//    public void GLBindTransformFeedback( int target, int id );
//
//    public void GLDeleteTransformFeedbacks( int n, int[] ids, int offset );
//
//    public void GLDeleteTransformFeedbacks( int n, IntBuffer ids );
//
//    public void GLGenTransformFeedbacks( int n, int[] ids, int offset );
//
//    public void GLGenTransformFeedbacks( int n, IntBuffer ids );
//
//    public bool GLIsTransformFeedback( int id );
//
//    public void GLPauseTransformFeedback();
//
//    public void GLResumeTransformFeedback();
//
//    public void GLProgramParameteri( int program, int pname, int value );
//
//    public void GLInvalidateFramebuffer( int target, int numAttachments, IntBuffer attachments );
//
//    public void GLInvalidateSubFramebuffer( int target,
//                                            int numAttachments,
//                                            IntBuffer attachments,
//                                            int x,
//                                            int y,
//                                            int width,
//                                            int height );
}