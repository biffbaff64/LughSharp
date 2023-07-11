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

public interface IGL30 : IGL20
{
    // ------------------------------------------------

    public const int GL_Read_Buffer                                   = 0x0C02;
    public const int GL_Unpack_Row_Length                             = 0x0CF2;
    public const int GL_Unpack_Skip_Rows                              = 0x0CF3;
    public const int GL_Unpack_Skip_Pixels                            = 0x0CF4;
    public const int GL_Pack_Row_Length                               = 0x0D02;
    public const int GL_Pack_Skip_Rows                                = 0x0D03;
    public const int GL_Pack_Skip_Pixels                              = 0x0D04;
    public const int GL_Color                                         = 0x1800;
    public const int GL_Depth                                         = 0x1801;
    public const int GL_Stencil                                       = 0x1802;
    public const int GL_Red                                           = 0x1903;
    public const int GL_RGB8                                          = 0x8051;
    public const int GL_RGBA8                                         = 0x8058;
    public const int GL_RGB10_A2                                      = 0x8059;
    public const int GL_Texture_Binding_3D                            = 0x806A;
    public const int GL_Unpack_Skip_Images                            = 0x806D;
    public const int GL_Unpack_Image_Height                           = 0x806E;
    public const int GL_Texture_3D                                    = 0x806F;
    public const int GL_Texture_Wrap_R                                = 0x8072;
    public const int GL_Max_3D_Texture_Size                           = 0x8073;
    public const int GL_Unsigned_Int_2_10_10_10_Rev                   = 0x8368;
    public const int GL_Max_Elements_Vertices                         = 0x80E8;
    public const int GL_Max_Elements_Indices                          = 0x80E9;
    public const int GL_Texture_Min_Lod                               = 0x813A;
    public const int GL_Texture_Max_Lod                               = 0x813B;
    public const int GL_Texture_Base_Level                            = 0x813C;
    public const int GL_Texture_Max_Level                             = 0x813D;
    public const int GL_Min                                           = 0x8007;
    public const int GL_Max                                           = 0x8008;
    public const int GL_Depth_Component24                             = 0x81A6;
    public const int GL_Max_Texture_Lod_Bias                          = 0x84FD;
    public const int GL_Texture_Compare_Mode                          = 0x884C;
    public const int GL_Texture_Compare_Func                          = 0x884D;
    public const int GL_Current_Query                                 = 0x8865;
    public const int GL_Query_Result                                  = 0x8866;
    public const int GL_Query_Result_Available                        = 0x8867;
    public const int GL_Buffer_Mapped                                 = 0x88BC;
    public const int GL_Buffer_Map_Pointer                            = 0x88BD;
    public const int GL_Stream_Read                                   = 0x88E1;
    public const int GL_Stream_Copy                                   = 0x88E2;
    public const int GL_Static_Read                                   = 0x88E5;
    public const int GL_Static_Copy                                   = 0x88E6;
    public const int GL_Dynamic_Read                                  = 0x88E9;
    public const int GL_Dynamic_Copy                                  = 0x88EA;
    public const int GL_Max_Draw_Buffers                              = 0x8824;
    public const int GL_Draw_Buffer0                                  = 0x8825;
    public const int GL_Draw_Buffer1                                  = 0x8826;
    public const int GL_Draw_Buffer2                                  = 0x8827;
    public const int GL_Draw_Buffer3                                  = 0x8828;
    public const int GL_Draw_Buffer4                                  = 0x8829;
    public const int GL_Draw_Buffer5                                  = 0x882A;
    public const int GL_Draw_Buffer6                                  = 0x882B;
    public const int GL_Draw_Buffer7                                  = 0x882C;
    public const int GL_Draw_Buffer8                                  = 0x882D;
    public const int GL_Draw_Buffer9                                  = 0x882E;
    public const int GL_Draw_Buffer10                                 = 0x882F;
    public const int GL_Draw_Buffer11                                 = 0x8830;
    public const int GL_Draw_Buffer12                                 = 0x8831;
    public const int GL_Draw_Buffer13                                 = 0x8832;
    public const int GL_Draw_Buffer14                                 = 0x8833;
    public const int GL_Draw_Buffer15                                 = 0x8834;
    public const int GL_Max_Fragment_Uniform_Components               = 0x8B49;
    public const int GL_Max_Vertex_Uniform_Components                 = 0x8B4A;
    public const int GL_Sampler_3D                                    = 0x8B5F;
    public const int GL_Sampler_2D_Shadow                             = 0x8B62;
    public const int GL_Fragment_Shader_Derivative_Hint               = 0x8B8B;
    public const int GL_Pixel_Pack_Buffer                             = 0x88EB;
    public const int GL_Pixel_Unpack_Buffer                           = 0x88EC;
    public const int GL_Pixel_Pack_Buffer_Binding                     = 0x88ED;
    public const int GL_Pixel_Unpack_Buffer_Binding                   = 0x88EF;
    public const int GL_Float_Mat2X3                                  = 0x8B65;
    public const int GL_Float_Mat2X4                                  = 0x8B66;
    public const int GL_Float_Mat3X2                                  = 0x8B67;
    public const int GL_Float_Mat3X4                                  = 0x8B68;
    public const int GL_Float_Mat4X2                                  = 0x8B69;
    public const int GL_Float_Mat4X3                                  = 0x8B6A;
    public const int GL_Srgb                                          = 0x8C40;
    public const int GL_Srgb8                                         = 0x8C41;
    public const int GL_Srgb8_Alpha8                                  = 0x8C43;
    public const int GL_Compare_Ref_To_Texture                        = 0x884E;
    public const int GL_Major_Version                                 = 0x821B;
    public const int GL_Minor_Version                                 = 0x821C;
    public const int GL_Num_Extensions                                = 0x821D;
    public const int GL_RGBA32F                                       = 0x8814;
    public const int GL_RGB32F                                        = 0x8815;
    public const int GL_RGBA16F                                       = 0x881A;
    public const int GL_RGB16F                                        = 0x881B;
    public const int GL_Vertex_Attrib_Array_Integer                   = 0x88FD;
    public const int GL_Max_Array_Texture_Layers                      = 0x88FF;
    public const int GL_Min_Program_Texel_Offset                      = 0x8904;
    public const int GL_Max_Program_Texel_Offset                      = 0x8905;
    public const int GL_Max_Varying_Components                        = 0x8B4B;
    public const int GL_Texture_2D_Array                              = 0x8C1A;
    public const int GL_Texture_Binding_2D_Array                      = 0x8C1D;
    public const int GL_R11F_G11F_B10F                                = 0x8C3A;
    public const int GL_Unsigned_Int_10F_11F_11F_Rev                  = 0x8C3B;
    public const int GL_RGB9_E5                                       = 0x8C3D;
    public const int GL_Unsigned_Int_5_9_9_9_Rev                      = 0x8C3E;
    public const int GL_Transform_Feedback_Varying_Max_Length         = 0x8C76;
    public const int GL_Transform_Feedback_Buffer_Mode                = 0x8C7F;
    public const int GL_Max_Transform_Feedback_Separate_Components    = 0x8C80;
    public const int GL_Transform_Feedback_Varyings                   = 0x8C83;
    public const int GL_Transform_Feedback_Buffer_Start               = 0x8C84;
    public const int GL_Transform_Feedback_Buffer_Size                = 0x8C85;
    public const int GL_Transform_Feedback_Primitives_Written         = 0x8C88;
    public const int GL_Rasterizer_Discard                            = 0x8C89;
    public const int GL_Max_Transform_Feedback_Interleaved_Components = 0x8C8A;
    public const int GL_Max_Transform_Feedback_Separate_Attribs       = 0x8C8B;
    public const int GL_Interleaved_Attribs                           = 0x8C8C;
    public const int GL_Separate_Attribs                              = 0x8C8D;
    public const int GL_Transform_Feedback_Buffer                     = 0x8C8E;
    public const int GL_Transform_Feedback_Buffer_Binding             = 0x8C8F;
    public const int GL_RGBA32Ui                                      = 0x8D70;
    public const int GL_RGB32Ui                                       = 0x8D71;
    public const int GL_RGBA16Ui                                      = 0x8D76;
    public const int GL_RGB16Ui                                       = 0x8D77;
    public const int GL_RGBA8Ui                                       = 0x8D7C;
    public const int GL_RGB8Ui                                        = 0x8D7D;
    public const int GL_RGBA32I                                       = 0x8D82;
    public const int GL_RGB32I                                        = 0x8D83;
    public const int GL_RGBA16I                                       = 0x8D88;
    public const int GL_RGB16I                                        = 0x8D89;
    public const int GL_RGBA8I                                        = 0x8D8E;
    public const int GL_RGB8I                                         = 0x8D8F;
    public const int GL_Red_Integer                                   = 0x8D94;
    public const int GL_RGB_Integer                                   = 0x8D98;
    public const int GL_RGBA_Integer                                  = 0x8D99;
    public const int GL_Sampler_2D_Array                              = 0x8DC1;
    public const int GL_Sampler_2D_Array_Shadow                       = 0x8DC4;
    public const int GL_Sampler_Cube_Shadow                           = 0x8DC5;
    public const int GL_Unsigned_Int_Vec2                             = 0x8DC6;
    public const int GL_Unsigned_Int_Vec3                             = 0x8DC7;
    public const int GL_Unsigned_Int_Vec4                             = 0x8DC8;
    public const int GL_Int_Sampler_2D                                = 0x8DCA;
    public const int GL_Int_Sampler_3D                                = 0x8DCB;
    public const int GL_Int_Sampler_Cube                              = 0x8DCC;
    public const int GL_Int_Sampler_2D_Array                          = 0x8DCF;
    public const int GL_Unsigned_Int_Sampler_2D                       = 0x8DD2;
    public const int GL_Unsigned_Int_Sampler_3D                       = 0x8DD3;
    public const int GL_Unsigned_Int_Sampler_Cube                     = 0x8DD4;
    public const int GL_Unsigned_Int_Sampler_2D_Array                 = 0x8DD7;
    public const int GL_Buffer_Access_Flags                           = 0x911F;
    public const int GL_Buffer_Map_Length                             = 0x9120;
    public const int GL_Buffer_Map_Offset                             = 0x9121;
    public const int GL_Depth_Component32F                            = 0x8CAC;
    public const int GL_Depth32F_Stencil8                             = 0x8CAD;
    public const int GL_Float_32_Unsigned_Int_24_8_Rev                = 0x8DAD;
    public const int GL_Framebuffer_Attachment_Color_Encoding         = 0x8210;
    public const int GL_Framebuffer_Attachment_Component_Type         = 0x8211;
    public const int GL_Framebuffer_Attachment_Red_Size               = 0x8212;
    public const int GL_Framebuffer_Attachment_Green_Size             = 0x8213;
    public const int GL_Framebuffer_Attachment_Blue_Size              = 0x8214;
    public const int GL_Framebuffer_Attachment_Alpha_Size             = 0x8215;
    public const int GL_Framebuffer_Attachment_Depth_Size             = 0x8216;
    public const int GL_Framebuffer_Attachment_Stencil_Size           = 0x8217;
    public const int GL_Framebuffer_Default                           = 0x8218;
    public const int GL_Framebuffer_Undefined                         = 0x8219;
    public const int GL_Depth_Stencil_Attachment                      = 0x821A;
    public const int GL_Depth_Stencil                                 = 0x84F9;
    public const int GL_Unsigned_Int_24_8                             = 0x84FA;
    public const int GL_Depth24_Stencil8                              = 0x88F0;
    public const int GL_Unsigned_Normalized                           = 0x8C17;
    public const int GL_Draw_Framebuffer_Binding                      = GL_Framebuffer_Binding;
    public const int GL_Read_Framebuffer                              = 0x8CA8;
    public const int GL_Draw_Framebuffer                              = 0x8CA9;
    public const int GL_Read_Framebuffer_Binding                      = 0x8CAA;
    public const int GL_Renderbuffer_Samples                          = 0x8CAB;
    public const int GL_Framebuffer_Attachment_Texture_Layer          = 0x8CD4;
    public const int GL_Max_Color_Attachments                         = 0x8CDF;
    public const int GL_Color_Attachment1                             = 0x8CE1;
    public const int GL_Color_Attachment2                             = 0x8CE2;
    public const int GL_Color_Attachment3                             = 0x8CE3;
    public const int GL_Color_Attachment4                             = 0x8CE4;
    public const int GL_Color_Attachment5                             = 0x8CE5;
    public const int GL_Color_Attachment6                             = 0x8CE6;
    public const int GL_Color_Attachment7                             = 0x8CE7;
    public const int GL_Color_Attachment8                             = 0x8CE8;
    public const int GL_Color_Attachment9                             = 0x8CE9;
    public const int GL_Color_Attachment10                            = 0x8CEA;
    public const int GL_Color_Attachment11                            = 0x8CEB;
    public const int GL_Color_Attachment12                            = 0x8CEC;
    public const int GL_Color_Attachment13                            = 0x8CED;
    public const int GL_Color_Attachment14                            = 0x8CEE;
    public const int GL_Color_Attachment15                            = 0x8CEF;
    public const int GL_Framebuffer_Incomplete_Multisample            = 0x8D56;
    public const int GL_Max_Samples                                   = 0x8D57;
    public const int GL_Half_Float                                    = 0x140B;
    public const int GL_Map_Read_Bit                                  = 0x0001;
    public const int GL_Map_Write_Bit                                 = 0x0002;
    public const int GL_Map_Invalidate_Range_Bit                      = 0x0004;
    public const int GL_Map_Invalidate_Buffer_Bit                     = 0x0008;
    public const int GL_Map_Flush_Explicit_Bit                        = 0x0010;
    public const int GL_Map_Unsynchronized_Bit                        = 0x0020;
    public const int GL_Rg                                            = 0x8227;
    public const int GL_Rg_Integer                                    = 0x8228;
    public const int GL_R8                                            = 0x8229;
    public const int GL_Rg8                                           = 0x822B;
    public const int GL_R16F                                          = 0x822D;
    public const int GL_R32F                                          = 0x822E;
    public const int GL_Rg16F                                         = 0x822F;
    public const int GL_Rg32F                                         = 0x8230;
    public const int GL_R8I                                           = 0x8231;
    public const int GL_R8Ui                                          = 0x8232;
    public const int GL_R16I                                          = 0x8233;
    public const int GL_R16Ui                                         = 0x8234;
    public const int GL_R32I                                          = 0x8235;
    public const int GL_R32Ui                                         = 0x8236;
    public const int GL_Rg8I                                          = 0x8237;
    public const int GL_Rg8Ui                                         = 0x8238;
    public const int GL_Rg16I                                         = 0x8239;
    public const int GL_Rg16Ui                                        = 0x823A;
    public const int GL_Rg32I                                         = 0x823B;
    public const int GL_Rg32Ui                                        = 0x823C;
    public const int GL_Vertex_Array_Binding                          = 0x85B5;
    public const int GL_R8_Snorm                                      = 0x8F94;
    public const int GL_Rg8_Snorm                                     = 0x8F95;
    public const int GL_RGB8_Snorm                                    = 0x8F96;
    public const int GL_RGBA8_Snorm                                   = 0x8F97;
    public const int GL_Signed_Normalized                             = 0x8F9C;
    public const int GL_Primitive_Restart_Fixed_Index                 = 0x8D69;
    public const int GL_Copy_Read_Buffer                              = 0x8F36;
    public const int GL_Copy_Write_Buffer                             = 0x8F37;
    public const int GL_Copy_Read_Buffer_Binding                      = GL_Copy_Read_Buffer;
    public const int GL_Copy_Write_Buffer_Binding                     = GL_Copy_Write_Buffer;
    public const int GL_Uniform_Buffer                                = 0x8A11;
    public const int GL_Uniform_Buffer_Binding                        = 0x8A28;
    public const int GL_Uniform_Buffer_Start                          = 0x8A29;
    public const int GL_Uniform_Buffer_Size                           = 0x8A2A;
    public const int GL_Max_Vertex_Uniform_Blocks                     = 0x8A2B;
    public const int GL_Max_Fragment_Uniform_Blocks                   = 0x8A2D;
    public const int GL_Max_Combined_Uniform_Blocks                   = 0x8A2E;
    public const int GL_Max_Uniform_Buffer_Bindings                   = 0x8A2F;
    public const int GL_Max_Uniform_Block_Size                        = 0x8A30;
    public const int GL_Max_Combined_Vertex_Uniform_Components        = 0x8A31;
    public const int GL_Max_Combined_Fragment_Uniform_Components      = 0x8A33;
    public const int GL_Uniform_Buffer_Offset_Alignment               = 0x8A34;
    public const int GL_Active_Uniform_Block_Max_Name_Length          = 0x8A35;
    public const int GL_Active_Uniform_Blocks                         = 0x8A36;
    public const int GL_Uniform_Type                                  = 0x8A37;
    public const int GL_Uniform_Size                                  = 0x8A38;
    public const int GL_Uniform_Name_Length                           = 0x8A39;
    public const int GL_Uniform_Block_Index                           = 0x8A3A;
    public const int GL_Uniform_Offset                                = 0x8A3B;
    public const int GL_Uniform_Array_Stride                          = 0x8A3C;
    public const int GL_Uniform_Matrix_Stride                         = 0x8A3D;
    public const int GL_Uniform_Is_Row_Major                          = 0x8A3E;
    public const int GL_Uniform_Block_Binding                         = 0x8A3F;
    public const int GL_Uniform_Block_Data_Size                       = 0x8A40;
    public const int GL_Uniform_Block_Name_Length                     = 0x8A41;
    public const int GL_Uniform_Block_Active_Uniforms                 = 0x8A42;
    public const int GL_Uniform_Block_Active_Uniform_Indices          = 0x8A43;
    public const int GL_Uniform_Block_Referenced_By_Vertex_Shader     = 0x8A44;
    public const int GL_Uniform_Block_Referenced_By_Fragment_Shader   = 0x8A46;

    // GL_INVALID_INDEX is defined as 0xFFFFFFFFu in C.
    public const int GL_Invalid_Index                 = -1;
    public const int GL_Max_Vertex_Output_Components  = 0x9122;
    public const int GL_Max_Fragment_Input_Components = 0x9125;
    public const int GL_Max_Server_Wait_Timeout       = 0x9111;
    public const int GL_Object_Type                   = 0x9112;
    public const int GL_Sync_Condition                = 0x9113;
    public const int GL_Sync_Status                   = 0x9114;
    public const int GL_Sync_Flags                    = 0x9115;
    public const int GL_Sync_Fence                    = 0x9116;
    public const int GL_Sync_Gpu_Commands_Complete    = 0x9117;
    public const int GL_Unsignaled                    = 0x9118;
    public const int GL_Signaled                      = 0x9119;
    public const int GL_Already_Signaled              = 0x911A;
    public const int GL_Timeout_Expired               = 0x911B;
    public const int GL_Condition_Satisfied           = 0x911C;
    public const int GL_Wait_Failed                   = 0x911D;
    public const int GL_Sync_Flush_Commands_Bit       = 0x00000001;

    // GL_TIMEOUT_IGNORED is defined as 0xFFFFFFFFFFFFFFFFull in C.
    public const long GL_Timeout_Ignored                           = -1;
    public const int  GL_Vertex_Attrib_Array_Divisor               = 0x88FE;
    public const int  GL_Any_Samples_Passed                        = 0x8C2F;
    public const int  GL_Any_Samples_Passed_Conservative           = 0x8D6A;
    public const int  GL_Sampler_Binding                           = 0x8919;
    public const int  GL_RGB10_A2Ui                                = 0x906F;
    public const int  GL_Texture_Swizzle_R                         = 0x8E42;
    public const int  GL_Texture_Swizzle_G                         = 0x8E43;
    public const int  GL_Texture_Swizzle_B                         = 0x8E44;
    public const int  GL_Texture_Swizzle_A                         = 0x8E45;
    public const int  GL_Green                                     = 0x1904;
    public const int  GL_Blue                                      = 0x1905;
    public const int  GL_Int_2_10_10_10_Rev                        = 0x8D9F;
    public const int  GL_Transform_Feedback                        = 0x8E22;
    public const int  GL_Transform_Feedback_Paused                 = 0x8E23;
    public const int  GL_Transform_Feedback_Active                 = 0x8E24;
    public const int  GL_Transform_Feedback_Binding                = 0x8E25;
    public const int  GL_Program_Binary_Retrievable_Hint           = 0x8257;
    public const int  GL_Program_Binary_Length                     = 0x8741;
    public const int  GL_Num_Program_Binary_Formats                = 0x87FE;
    public const int  GL_Program_Binary_Formats                    = 0x87FF;
    public const int  GL_Compressed_R11_Eac                        = 0x9270;
    public const int  GL_Compressed_Signed_R11_Eac                 = 0x9271;
    public const int  GL_Compressed_Rg11_Eac                       = 0x9272;
    public const int  GL_Compressed_Signed_Rg11_Eac                = 0x9273;
    public const int  GL_Compressed_RGB8_ETC2                      = 0x9274;
    public const int  GL_Compressed_Srgb8_ETC2                     = 0x9275;
    public const int  GL_Compressed_RGB8_Punchthrough_Alpha1_ETC2  = 0x9276;
    public const int  GL_Compressed_Srgb8_Punchthrough_Alpha1_ETC2 = 0x9277;
    public const int  GL_Compressed_RGBA8_ETC2_Eac                 = 0x9278;
    public const int  GL_Compressed_Srgb8_Alpha8_ETC2_Eac          = 0x9279;
    public const int  GL_Texture_Immutable_Format                  = 0x912F;
    public const int  GL_Max_Element_Index                         = 0x8D6B;
    public const int  GL_Num_Sample_Counts                         = 0x9380;
    public const int  GL_Texture_Immutable_Levels                  = 0x82DF;

    // ------------------------------------------------

    //@formatter:off
    
	public void GLReadBuffer( int mode );

	public void GLDrawRangeElements( int mode, int start, int end, int count, int type, Buffer indices );

	public void GLDrawRangeElements( int mode, int start, int end, int count, int type, int offset );

	public void GLTexImage3D( int target, int level, int internalformat, int width, int height,
	                          int depth, int border, int format, int type, Buffer pixels );
	
	public void GLTexImage3D( int target, int level, int internalformat, int width, int height,
	                          int depth, int border, int format, int type, int offset );

	public void GLTexSubImage3D( int target, int level, int xoffset, int yoffset, int zoffset, int width,
	                             int height, int depth, int format, int type, Buffer pixels );

	public void GLTexSubImage3D( int target, int level, int xoffset, int yoffset, int zoffset, int width,
	                             int height, int depth, int format, int type, int offset );

	public void GLCopyTexSubImage3D( int target, int level, int xoffset, int yoffset, int zoffset,
	                                 int x, int y, int width, int height );
	
	public void GLGenQueries( int n, int[] ids, int offset );

	public void GLGenQueries( int n, IntBuffer ids );

	public void GLDeleteQueries( int n, int[] ids, int offset );

	public void GLDeleteQueries( int n, IntBuffer ids );

	public bool GLIsQuery( int id );

	public void GLBeginQuery( int target, int id );

	public void GLEndQuery( int target );

	public void GLGetQueryiv( int target, int pname, IntBuffer param );

	public void GLGetQueryObjectuiv( int id, int pname, IntBuffer param );

	public bool GLUnmapBuffer( int target );

	public Buffer GLGetBufferPointerv( int target, int pname );

	public void GLDrawBuffers( int n, IntBuffer bufs );

	public void GLUniformMatrix2X3Fv( int location, int count, bool transpose, FloatBuffer value );

	public void GLUniformMatrix3X2Fv( int location, int count, bool transpose, FloatBuffer value );

	public void GLUniformMatrix2X4Fv( int location, int count, bool transpose, FloatBuffer value );

	public void GLUniformMatrix4X2Fv( int location, int count, bool transpose, FloatBuffer value );

	public void GLUniformMatrix3X4Fv( int location, int count, bool transpose, FloatBuffer value );

	public void GLUniformMatrix4X3Fv( int location, int count, bool transpose, FloatBuffer value );

	public void GLBlitFramebuffer( int srcX0, int srcY0, int srcX1, int srcY1,
	                               int dstX0, int dstY0, int dstX1, int dstY1,
	                               int mask, int filter );

	public void GLRenderbufferStorageMultisample( int target, int samples,
	                                              int internalformat, int width, int height );

	public void GLFramebufferTextureLayer( int target, int attachment, int texture, int level, int layer );

	public Buffer GLMapBufferRange( int target, int offset, int length, int access );

	public void GLFlushMappedBufferRange( int target, int offset, int length );

	public void GLBindVertexArray( int array );

	public void GLDeleteVertexArrays( int n, int[] arrays, int offset );

	public void GLDeleteVertexArrays( int n, IntBuffer arrays );

	public void GLGenVertexArrays( int n, int[] arrays, int offset );

	public void GLGenVertexArrays( int n, IntBuffer arrays );

	public bool GLIsVertexArray( int array );

	public void GLBeginTransformFeedback( int primitiveMode );

	public void GLEndTransformFeedback();

	public void GLBindBufferRange( int target, int index, int buffer, int offset, int size );

	public void GLBindBufferBase( int target, int index, int buffer );

	public void GLTransformFeedbackVaryings( int program, string[] varyings, int bufferMode );

	public void GLVertexAttribIPointer( int index, int size, int type, int stride, int offset );

	public void GLGetVertexAttribIiv( int index, int pname, IntBuffer param );

	public void GLGetVertexAttribIuiv( int index, int pname, IntBuffer parameters );

	public void GLVertexAttribI4I( int index, int x, int y, int z, int w );

	public void GLVertexAttribI4Ui( int index, int x, int y, int z, int w );
	
	public void GLGetUniformuiv( int program, int location, IntBuffer parameters );

	public int GLGetFragDataLocation( int program, String name );

	public void GLUniform1Uiv( int location, int count, IntBuffer value );

	public void GLUniform3Uiv( int location, int count, IntBuffer value );

	public void GLUniform4Uiv( int location, int count, IntBuffer value );

	public void GLClearBufferiv( int buffer, int drawbuffer, IntBuffer value );

	public void GLClearBufferuiv( int buffer, int drawbuffer, IntBuffer value );

	public void GLClearBufferfv( int buffer, int drawbuffer, FloatBuffer value );

	public void GLClearBufferfi( int buffer, int drawbuffer, float depth, int stencil );

	public string GLGetStringi( int name, int index );

	public void GLCopyBufferSubData( int readTarget, int writeTarget, int readOffset,
	                                 int writeOffset, int size );

	public void GLGetUniformIndices( int program, string[] uniformNames, IntBuffer uniformIndices );

	public void GLGetActiveUniformsiv( int program, int uniformCount, IntBuffer uniformIndices,
	                                   int pname, IntBuffer parameters );

	public int GLGetUniformBlockIndex( int program, string uniformBlockName );

	public void GLGetActiveUniformBlockiv( int program, int uniformBlockIndex,
	                                       int pname, IntBuffer parameters );

	public void GLGetActiveUniformBlockName( int program, int uniformBlockIndex,
	                                         Buffer length, Buffer uniformBlockName );

	public string GLGetActiveUniformBlockName( int program, int uniformBlockIndex );

	public void GLUniformBlockBinding( int program, int uniformBlockIndex, int uniformBlockBinding );

	public void GLDrawArraysInstanced( int mode, int first, int count, int instanceCount );

	public void GLDrawElementsInstanced( int mode, int count, int type,
	                                     int indicesOffset, int instanceCount );

	public void GLGetInteger64V( int pname, LongBuffer parameters );

	public void GLGetBufferParameteri64V( int target, int pname, LongBuffer parameters);

	public void GLGenSamplers( int count, int[] samplers, int offset );

	public void GLGenSamplers( int count, IntBuffer samplers );

	public void GLDeleteSamplers( int count, int[] samplers, int offset );

	public void GLDeleteSamplers( int count, IntBuffer samplers );

	public bool GLIsSampler( int sampler );

	public void GLBindSampler( int unit, int sampler );

	public void GLSamplerParameteri( int sampler, int pname, int param );

	public void GLSamplerParameteriv( int sampler, int pname, IntBuffer param );

	public void GLSamplerParameterf( int sampler, int pname, float param );

	public void GLSamplerParameterfv( int sampler, int pname, FloatBuffer param );

	public void GLGetSamplerParameteriv( int sampler, int pname, IntBuffer parameters );

	public void GLGetSamplerParameterfv( int sampler, int pname, FloatBuffer parameters );

	public void GLVertexAttribDivisor( int index, int divisor );

	public void GLBindTransformFeedback( int target, int id );

	public void GLDeleteTransformFeedbacks( int n, int[] ids, int offset );

	public void GLDeleteTransformFeedbacks( int n, IntBuffer ids );

	public void GLGenTransformFeedbacks( int n, int[] ids, int offset );

	public void GLGenTransformFeedbacks( int n, IntBuffer ids );

	public bool GLIsTransformFeedback( int id );

	public void GLPauseTransformFeedback();

	public void GLResumeTransformFeedback();

	public void GLProgramParameteri( int program, int pname, int value );

	public void GLInvalidateFramebuffer( int target, int numAttachments, IntBuffer attachments );

	public void GLInvalidateSubFramebuffer( int target, int numAttachments, IntBuffer attachments,
	                                        int x, int y, int width, int height );

	/// <summary>
	/// In OpenGl core profiles (3.1+), passing a pointer to client memory is
	/// not valid. Use the other version of this function instead, pass a
	/// zero-based offset which references the buffer currently bound to GL_Array_Buffer.
	/// </summary>
	public new void GLVertexAttribPointer( int indx, int size, int type,
	                                       bool normalized, int stride, Buffer ptr );

    //@formatter:on
}
