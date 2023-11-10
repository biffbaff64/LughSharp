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

using LibGDXSharp.Core.Files.Buffers;

namespace LibGDXSharp.Graphics;

[PublicAPI]
public interface IGL31 : IGL30
{
    const int GL_VERTEX_SHADER_BIT   = 0x00000001;
    const int GL_FRAGMENT_SHADER_BIT = 0x00000002;
    const int GL_COMPUTE_SHADER_BIT  = 0x00000020;
    const int GL_ALL_SHADER_BITS     = -1; // 0xFFFFFFFF

    const int GL_VERTEX_ATTRIB_ARRAY_BARRIER_BIT = 0x00000001;
    const int GL_ELEMENT_ARRAY_BARRIER_BIT       = 0x00000002;
    const int GL_UNIFORM_BARRIER_BIT             = 0x00000004;
    const int GL_TEXTURE_FETCH_BARRIER_BIT       = 0x00000008;
    const int GL_SHADER_IMAGE_ACCESS_BARRIER_BIT = 0x00000020;
    const int GL_COMMAND_BARRIER_BIT             = 0x00000040;
    const int GL_PIXEL_BUFFER_BARRIER_BIT        = 0x00000080;
    const int GL_TEXTURE_UPDATE_BARRIER_BIT      = 0x00000100;
    const int GL_BUFFER_UPDATE_BARRIER_BIT       = 0x00000200;
    const int GL_FRAMEBUFFER_BARRIER_BIT         = 0x00000400;
    const int GL_TRANSFORM_FEEDBACK_BARRIER_BIT  = 0x00000800;
    const int GL_ATOMIC_COUNTER_BARRIER_BIT      = 0x00001000;
    const int GL_SHADER_STORAGE_BARRIER_BIT      = 0x00002000;
    const int GL_ALL_BARRIER_BITS                = -1; // 0xFFFFFFFF

    const int GL_TEXTURE_WIDTH                              = 0x1000;
    const int GL_TEXTURE_HEIGHT                             = 0x1001;
    const int GL_TEXTURE_INTERNAL_FORMAT                    = 0x1003;
    const int GL_STENCIL_INDEX                              = 0x1901;
    const int GL_TEXTURE_RED_SIZE                           = 0x805C;
    const int GL_TEXTURE_GREEN_SIZE                         = 0x805D;
    const int GL_TEXTURE_BLUE_SIZE                          = 0x805E;
    const int GL_TEXTURE_ALPHA_SIZE                         = 0x805F;
    const int GL_TEXTURE_DEPTH                              = 0x8071;
    const int GL_PROGRAM_SEPARABLE                          = 0x8258;
    const int GL_ACTIVE_PROGRAM                             = 0x8259;
    const int GL_PROGRAM_PIPELINE_BINDING                   = 0x825A;
    const int GL_MAX_COMPUTE_SHARED_MEMORY_SIZE             = 0x8262;
    const int GL_MAX_COMPUTE_UNIFORM_COMPONENTS             = 0x8263;
    const int GL_MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS         = 0x8264;
    const int GL_MAX_COMPUTE_ATOMIC_COUNTERS                = 0x8265;
    const int GL_MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS    = 0x8266;
    const int GL_COMPUTE_WORK_GROUP_SIZE                    = 0x8267;
    const int GL_MAX_UNIFORM_LOCATIONS                      = 0x826E;
    const int GL_VERTEX_ATTRIB_BINDING                      = 0x82D4;
    const int GL_VERTEX_ATTRIB_RELATIVE_OFFSET              = 0x82D5;
    const int GL_VERTEX_BINDING_DIVISOR                     = 0x82D6;
    const int GL_VERTEX_BINDING_OFFSET                      = 0x82D7;
    const int GL_VERTEX_BINDING_STRIDE                      = 0x82D8;
    const int GL_MAX_VERTEX_ATTRIB_RELATIVE_OFFSET          = 0x82D9;
    const int GL_MAX_VERTEX_ATTRIB_BINDINGS                 = 0x82DA;
    const int GL_MAX_VERTEX_ATTRIB_STRIDE                   = 0x82E5;
    const int GL_TEXTURE_COMPRESSED                         = 0x86A1;
    const int GL_TEXTURE_DEPTH_SIZE                         = 0x884A;
    const int GL_READ_ONLY                                  = 0x88B8;
    const int GL_WRITE_ONLY                                 = 0x88B9;
    const int GL_READ_WRITE                                 = 0x88BA;
    const int GL_TEXTURE_STENCIL_SIZE                       = 0x88F1;
    const int GL_TEXTURE_RED_TYPE                           = 0x8C10;
    const int GL_TEXTURE_GREEN_TYPE                         = 0x8C11;
    const int GL_TEXTURE_BLUE_TYPE                          = 0x8C12;
    const int GL_TEXTURE_ALPHA_TYPE                         = 0x8C13;
    const int GL_TEXTURE_DEPTH_TYPE                         = 0x8C16;
    const int GL_TEXTURE_SHARED_SIZE                        = 0x8C3F;
    const int GL_SAMPLE_POSITION                            = 0x8E50;
    const int GL_SAMPLE_MASK                                = 0x8E51;
    const int GL_SAMPLE_MASK_VALUE                          = 0x8E52;
    const int GL_MAX_SAMPLE_MASK_WORDS                      = 0x8E59;
    const int GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET          = 0x8E5E;
    const int GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET          = 0x8E5F;
    const int GL_MAX_IMAGE_UNITS                            = 0x8F38;
    const int GL_MAX_COMBINED_SHADER_OUTPUT_RESOURCES       = 0x8F39;
    const int GL_IMAGE_BINDING_NAME                         = 0x8F3A;
    const int GL_IMAGE_BINDING_LEVEL                        = 0x8F3B;
    const int GL_IMAGE_BINDING_LAYERED                      = 0x8F3C;
    const int GL_IMAGE_BINDING_LAYER                        = 0x8F3D;
    const int GL_IMAGE_BINDING_ACCESS                       = 0x8F3E;
    const int GL_DRAW_INDIRECT_BUFFER                       = 0x8F3F;
    const int GL_DRAW_INDIRECT_BUFFER_BINDING               = 0x8F43;
    const int GL_VERTEX_BINDING_BUFFER                      = 0x8F4F;
    const int GL_IMAGE_2D                                   = 0x904D;
    const int GL_IMAGE_3D                                   = 0x904E;
    const int GL_IMAGE_CUBE                                 = 0x9050;
    const int GL_IMAGE_2D_ARRAY                             = 0x9053;
    const int GL_INT_IMAGE_2D                               = 0x9058;
    const int GL_INT_IMAGE_3D                               = 0x9059;
    const int GL_INT_IMAGE_CUBE                             = 0x905B;
    const int GL_INT_IMAGE_2D_ARRAY                         = 0x905E;
    const int GL_UNSIGNED_INT_IMAGE_2D                      = 0x9063;
    const int GL_UNSIGNED_INT_IMAGE_3D                      = 0x9064;
    const int GL_UNSIGNED_INT_IMAGE_CUBE                    = 0x9066;
    const int GL_UNSIGNED_INT_IMAGE_2D_ARRAY                = 0x9069;
    const int GL_IMAGE_BINDING_FORMAT                       = 0x906E;
    const int GL_IMAGE_FORMAT_COMPATIBILITY_TYPE            = 0x90C7;
    const int GL_IMAGE_FORMAT_COMPATIBILITY_BY_SIZE         = 0x90C8;
    const int GL_IMAGE_FORMAT_COMPATIBILITY_BY_CLASS        = 0x90C9;
    const int GL_MAX_VERTEX_IMAGE_UNIFORMS                  = 0x90CA;
    const int GL_MAX_FRAGMENT_IMAGE_UNIFORMS                = 0x90CE;
    const int GL_MAX_COMBINED_IMAGE_UNIFORMS                = 0x90CF;
    const int GL_SHADER_STORAGE_BUFFER                      = 0x90D2;
    const int GL_SHADER_STORAGE_BUFFER_BINDING              = 0x90D3;
    const int GL_SHADER_STORAGE_BUFFER_START                = 0x90D4;
    const int GL_SHADER_STORAGE_BUFFER_SIZE                 = 0x90D5;
    const int GL_MAX_VERTEX_SHADER_STORAGE_BLOCKS           = 0x90D6;
    const int GL_MAX_FRAGMENT_SHADER_STORAGE_BLOCKS         = 0x90DA;
    const int GL_MAX_COMPUTE_SHADER_STORAGE_BLOCKS          = 0x90DB;
    const int GL_MAX_COMBINED_SHADER_STORAGE_BLOCKS         = 0x90DC;
    const int GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS         = 0x90DD;
    const int GL_MAX_SHADER_STORAGE_BLOCK_SIZE              = 0x90DE;
    const int GL_SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT     = 0x90DF;
    const int GL_DEPTH_STENCIL_TEXTURE_MODE                 = 0x90EA;
    const int GL_MAX_COMPUTE_WORK_GROUP_INVOCATIONS         = 0x90EB;
    const int GL_DISPATCH_INDIRECT_BUFFER                   = 0x90EE;
    const int GL_DISPATCH_INDIRECT_BUFFER_BINDING           = 0x90EF;
    const int GL_TEXTURE_2D_MULTISAMPLE                     = 0x9100;
    const int GL_TEXTURE_BINDING_2D_MULTISAMPLE             = 0x9104;
    const int GL_TEXTURE_SAMPLES                            = 0x9106;
    const int GL_TEXTURE_FIXED_SAMPLE_LOCATIONS             = 0x9107;
    const int GL_SAMPLER_2D_MULTISAMPLE                     = 0x9108;
    const int GL_INT_SAMPLER_2D_MULTISAMPLE                 = 0x9109;
    const int GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE        = 0x910A;
    const int GL_MAX_COLOR_TEXTURE_SAMPLES                  = 0x910E;
    const int GL_MAX_DEPTH_TEXTURE_SAMPLES                  = 0x910F;
    const int GL_MAX_INTEGER_SAMPLES                        = 0x9110;
    const int GL_COMPUTE_SHADER                             = 0x91B9;
    const int GL_MAX_COMPUTE_UNIFORM_BLOCKS                 = 0x91BB;
    const int GL_MAX_COMPUTE_TEXTURE_IMAGE_UNITS            = 0x91BC;
    const int GL_MAX_COMPUTE_IMAGE_UNIFORMS                 = 0x91BD;
    const int GL_MAX_COMPUTE_WORK_GROUP_COUNT               = 0x91BE;
    const int GL_MAX_COMPUTE_WORK_GROUP_SIZE                = 0x91BF;
    const int GL_ATOMIC_COUNTER_BUFFER                      = 0x92C0;
    const int GL_ATOMIC_COUNTER_BUFFER_BINDING              = 0x92C1;
    const int GL_ATOMIC_COUNTER_BUFFER_START                = 0x92C2;
    const int GL_ATOMIC_COUNTER_BUFFER_SIZE                 = 0x92C3;
    const int GL_MAX_VERTEX_ATOMIC_COUNTER_BUFFERS          = 0x92CC;
    const int GL_MAX_FRAGMENT_ATOMIC_COUNTER_BUFFERS        = 0x92D0;
    const int GL_MAX_COMBINED_ATOMIC_COUNTER_BUFFERS        = 0x92D1;
    const int GL_MAX_VERTEX_ATOMIC_COUNTERS                 = 0x92D2;
    const int GL_MAX_FRAGMENT_ATOMIC_COUNTERS               = 0x92D6;
    const int GL_MAX_COMBINED_ATOMIC_COUNTERS               = 0x92D7;
    const int GL_MAX_ATOMIC_COUNTER_BUFFER_SIZE             = 0x92D8;
    const int GL_ACTIVE_ATOMIC_COUNTER_BUFFERS              = 0x92D9;
    const int GL_UNSIGNED_INT_ATOMIC_COUNTER                = 0x92DB;
    const int GL_MAX_ATOMIC_COUNTER_BUFFER_BINDINGS         = 0x92DC;
    const int GL_UNIFORM                                    = 0x92E1;
    const int GL_UNIFORM_BLOCK                              = 0x92E2;
    const int GL_PROGRAM_INPUT                              = 0x92E3;
    const int GL_PROGRAM_OUTPUT                             = 0x92E4;
    const int GL_BUFFER_VARIABLE                            = 0x92E5;
    const int GL_SHADER_STORAGE_BLOCK                       = 0x92E6;
    const int GL_TRANSFORM_FEEDBACK_VARYING                 = 0x92F4;
    const int GL_ACTIVE_RESOURCES                           = 0x92F5;
    const int GL_MAX_NAME_LENGTH                            = 0x92F6;
    const int GL_MAX_NUM_ACTIVE_VARIABLES                   = 0x92F7;
    const int GL_NAME_LENGTH                                = 0x92F9;
    const int GL_TYPE                                       = 0x92FA;
    const int GL_ARRAY_SIZE                                 = 0x92FB;
    const int GL_OFFSET                                     = 0x92FC;
    const int GL_BLOCK_INDEX                                = 0x92FD;
    const int GL_ARRAY_STRIDE                               = 0x92FE;
    const int GL_MATRIX_STRIDE                              = 0x92FF;
    const int GL_IS_ROW_MAJOR                               = 0x9300;
    const int GL_ATOMIC_COUNTER_BUFFER_INDEX                = 0x9301;
    const int GL_BUFFER_BINDING                             = 0x9302;
    const int GL_BUFFER_DATA_SIZE                           = 0x9303;
    const int GL_NUM_ACTIVE_VARIABLES                       = 0x9304;
    const int GL_ACTIVE_VARIABLES                           = 0x9305;
    const int GL_REFERENCED_BY_VERTEX_SHADER                = 0x9306;
    const int GL_REFERENCED_BY_FRAGMENT_SHADER              = 0x930A;
    const int GL_REFERENCED_BY_COMPUTE_SHADER               = 0x930B;
    const int GL_TOP_LEVEL_ARRAY_SIZE                       = 0x930C;
    const int GL_TOP_LEVEL_ARRAY_STRIDE                     = 0x930D;
    const int GL_LOCATION                                   = 0x930E;
    const int GL_FRAMEBUFFER_DEFAULT_WIDTH                  = 0x9310;
    const int GL_FRAMEBUFFER_DEFAULT_HEIGHT                 = 0x9311;
    const int GL_FRAMEBUFFER_DEFAULT_SAMPLES                = 0x9313;
    const int GL_FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS = 0x9314;
    const int GL_MAX_FRAMEBUFFER_WIDTH                      = 0x9315;
    const int GL_MAX_FRAMEBUFFER_HEIGHT                     = 0x9316;
    const int GL_MAX_FRAMEBUFFER_SAMPLES                    = 0x9318;

    // C function void glDispatchCompute ( GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z )

    void GLDispatchCompute( int num_groups_x, int num_groups_y, int num_groups_z );

    // C function void glDispatchComputeIndirect ( GLintptr indirect );

    void GLDispatchComputeIndirect( long indirect );

    // C function void glDrawArraysIndirect ( GLenum mode, const void *indirect );

    void GLDrawArraysIndirect( int mode, long indirect );

    // C function glDrawElementsIndirect ( GLenum mode, GLenum type, const void *indirect );

    void GLDrawElementsIndirect( int mode, int type, long indirect );

    // C function void glFramebufferParameteri ( GLenum target, GLenum pname, GLint param )

    void GLFramebufferParameteri( int target, int pname, int param );

    // C function void glGetFramebufferParameteriv ( GLenum target, GLenum pname, GLint *parameters )

// void glGetFramebufferParameteriv(
// int target,
// int pname,
// int[] parameters,
// int offset
// );

    // C function void glGetFramebufferParameteriv ( GLenum target, GLenum pname, GLint *parameters )

    void GLGetFramebufferParameteriv( int target, int pname, IntBuffer parameters );

    // C function void glGetProgramInterfaceiv ( GLuint program, GLenum programInterface, GLenum pname, GLint *parameters )

// void glGetProgramInterfaceiv(
// int program,
// int programInterface,
// int pname,
// int[] parameters,
// int offset
// );

    // C function void glGetProgramInterfaceiv ( GLuint program, GLenum programInterface, GLenum pname, GLint *parameters )

    void GLGetProgramInterfaceiv( int program, int programInterface, int pname, IntBuffer parameters );

    // C function GLuint glGetProgramResourceIndex ( GLuint program, GLenum programInterface, const GLchar *name )

    int GLGetProgramResourceIndex( int program, int programInterface, string name );

    // C function void glGetProgramResourceName ( GLuint program, GLenum programInterface, GLuint index, GLsizei bufSize, GLsizei
    // *length, GLchar *name )

    string GLGetProgramResourceName( int program, int programInterface, int index );

    // C function void glGetProgramResourceiv ( GLuint program, GLenum programInterface, GLuint index, GLsizei propCount, const
    // GLenum *props, GLsizei bufSize, GLsizei *length, GLint *parameters )

// void glGetProgramResourceiv(
// int program,
// int programInterface,
// int index,
// int propCount,
// int[] props,
// int propsOffset,
// int bufSize,
// int[] length,
// int lengthOffset,
// int[] parameters,
// int paramsOffset
// );

    // C function void glGetProgramResourceiv ( GLuint program, GLenum programInterface, GLuint index, GLsizei propCount, const
    // GLenum *props, GLsizei bufSize, GLsizei *length, GLint *parameters )

    void GLGetProgramResourceiv( int program,
                                 int programInterface,
                                 int index,
                                // int propCount,
                                 IntBuffer props,
                                // int bufSize,
                                 IntBuffer length,
                                 IntBuffer parameters );

    // C function GLint glGetProgramResourceLocation ( GLuint program, GLenum programInterface, const GLchar *name )

    int GLGetProgramResourceLocation( int program, int programInterface, string name );

    // C function void glUseProgramStages ( GLuint pipeline, GLbitfield stages, GLuint program )

    void GLUseProgramStages( int pipeline, int stages, int program );

    // C function void glActiveShaderProgram ( GLuint pipeline, GLuint program )

    void GLActiveShaderProgram( int pipeline, int program );

    // C function GLuint glCreateShaderProgramv ( GLenum type, GLsizei count, const GLchar *const *strings )

    int GLCreateShaderProgramv( int type, string[] strings );

    // C function void glBindProgramPipeline ( GLuint pipeline )

    void GLBindProgramPipeline( int pipeline );

    // C function void glDeleteProgramPipelines ( GLsizei n, const GLuint *pipelines )

// void glDeleteProgramPipelines(
// int n,
// int[] pipelines,
// int offset
// );

    // C function void glDeleteProgramPipelines ( GLsizei n, const GLuint *pipelines )

    void GLDeleteProgramPipelines( int n, IntBuffer pipelines );

    // C function void glGenProgramPipelines ( GLsizei n, GLuint *pipelines )

// void glGenProgramPipelines(
// int n,
// int[] pipelines,
// int offset
// );

    // C function void glGenProgramPipelines ( GLsizei n, GLuint *pipelines )

    void GLGenProgramPipelines( int n, IntBuffer pipelines );

    // C function GLboolean glIsProgramPipeline ( GLuint pipeline )

    bool GLIsProgramPipeline( int pipeline );

    // C function void glGetProgramPipelineiv ( GLuint pipeline, GLenum pname, GLint *parameters )

// void glGetProgramPipelineiv(
// int pipeline,
// int pname,
// int[] parameters,
// int offset
// );

    // C function void glGetProgramPipelineiv ( GLuint pipeline, GLenum pname, GLint *parameters )

    void GLGetProgramPipelineiv( int pipeline, int pname, IntBuffer parameters );

    // C function void glProgramUniform1i ( GLuint program, GLint location, GLint v0 )

    void GLProgramUniform1I( int program, int location, int v0 );

    // C function void glProgramUniform2i ( GLuint program, GLint location, GLint v0, GLint v1 )

    void GLProgramUniform2I( int program, int location, int v0, int v1 );

    // C function void glProgramUniform3i ( GLuint program, GLint location, GLint v0, GLint v1, GLint v2 )

    void GLProgramUniform3I( int program, int location, int v0, int v1, int v2 );

    // C function void glProgramUniform4i ( GLuint program, GLint location, GLint v0, GLint v1, GLint v2, GLint v3 )

    void GLProgramUniform4I( int program, int location, int v0, int v1, int v2, int v3 );

    // C function void glProgramUniform1ui ( GLuint program, GLint location, GLuint v0 )

    void GLProgramUniform1UI( int program, int location, int v0 );

    // C function void glProgramUniform2ui ( GLuint program, GLint location, GLuint v0, GLuint v1 )

    void GLProgramUniform2UI( int program, int location, int v0, int v1 );

    // C function void glProgramUniform3ui ( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2 )

    void GLProgramUniform3UI( int program, int location, int v0, int v1, int v2 );

    // C function void glProgramUniform4ui ( GLuint program, GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3 )

    void GLProgramUniform4UI( int program, int location, int v0, int v1, int v2, int v3 );

    // C function void glProgramUniform1f ( GLuint program, GLint location, GLfloat v0 )

    void GLProgramUniform1F( int program, int location, float v0 );

    // C function void glProgramUniform2f ( GLuint program, GLint location, GLfloat v0, GLfloat v1 )

    void GLProgramUniform2F( int program, int location, float v0, float v1 );

    // C function void glProgramUniform3f ( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2 )

    void GLProgramUniform3F( int program, int location, float v0, float v1, float v2 );

    // C function void glProgramUniform4f ( GLuint program, GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3 )

    void GLProgramUniform4F( int program, int location, float v0, float v1, float v2, float v3 );

    // C function void glProgramUniform1iv ( GLuint program, GLint location, GLsizei count, const GLint *value )

// void glProgramUniform1iv(
// int program,
// int location,
// int count,
// int[] value,
// int offset
// );

    // C function void glProgramUniform1iv ( GLuint program, GLint location, GLsizei count, const GLint *value )

    void GLProgramUniform1Iv( int program,
                              int location, 
                              // int count,
                              IntBuffer value );

    // C function void glProgramUniform2iv ( GLuint program, GLint location, GLsizei count, const GLint *value )

// void glProgramUniform2iv(
// int program,
// int location,
// int count,
// int[] value,
// int offset
// );

    // C function void glProgramUniform2iv ( GLuint program, GLint location, GLsizei count, const GLint *value )

    void GLProgramUniform2Iv( int program,
                              int location,
                              // int count,
                              IntBuffer value );

    // C function void glProgramUniform3iv ( GLuint program, GLint location, GLsizei count, const GLint *value )

// void glProgramUniform3iv(
// int program,
// int location,
// int count,
// int[] value,
// int offset
// );

    // C function void glProgramUniform3iv ( GLuint program, GLint location, GLsizei count, const GLint *value )

    void GLProgramUniform3Iv( int program,
                              int location,
                              // int count,
                              IntBuffer value );

    // C function void glProgramUniform4iv ( GLuint program, GLint location, GLsizei count, const GLint *value )

// void glProgramUniform4iv(
// int program,
// int location,
// int count,
// int[] value,
// int offset
// );

    // C function void glProgramUniform4iv ( GLuint program, GLint location, GLsizei count, const GLint *value )

    void GLProgramUniform4Iv( int program,
                              int location,
                              // int count,
                              IntBuffer value );

    // C function void glProgramUniform1uiv ( GLuint program, GLint location, GLsizei count, const GLuint *value )

// void glProgramUniform1uiv(
// int program,
// int location,
// int count,
// int[] value,
// int offset
// );

    // C function void glProgramUniform1uiv ( GLuint program, GLint location, GLsizei count, const GLuint *value )

    void GLProgramUniform1Uiv( int program,
                               int location, 
                               // int count,
                               IntBuffer value );

    // C function void glProgramUniform2uiv ( GLuint program, GLint location, GLsizei count, const GLuint *value )

// void glProgramUniform2uiv(
// int program,
// int location,
// int count,
// int[] value,
// int offset
// );

    // C function void glProgramUniform2uiv ( GLuint program, GLint location, GLsizei count, const GLuint *value )

    void GLProgramUniform2Uiv( int program,
                               int location, 
                               // int count,
                               IntBuffer value );

    // C function void glProgramUniform3uiv ( GLuint program, GLint location, GLsizei count, const GLuint *value )

// void glProgramUniform3uiv(
// int program,
// int location,
// int count,
// int[] value,
// int offset
// );

    // C function void glProgramUniform3uiv ( GLuint program, GLint location, GLsizei count, const GLuint *value )

    void GLProgramUniform3Uiv( int program,
                               int location, 
                               // int count,
                               IntBuffer value );

    // C function void glProgramUniform4uiv ( GLuint program, GLint location, GLsizei count, const GLuint *value )

// void glProgramUniform4uiv(
// int program,
// int location,
// int count,
// int[] value,
// int offset
// );

    // C function void glProgramUniform4uiv ( GLuint program, GLint location, GLsizei count, const GLuint *value )

    void GLProgramUniform4Uiv( int program,
                               int location, 
                               // int count,
                               IntBuffer value );

    // C function void glProgramUniform1fv ( GLuint program, GLint location, GLsizei count, const GLfloat *value )

// void glProgramUniform1fv(
// int program,
// int location,
// int count,
// float[] value,
// int offset
// );

    // C function void glProgramUniform1fv ( GLuint program, GLint location, GLsizei count, const GLfloat *value )

    void GLProgramUniform1Fv( int program,
                              int location, 
                              // int count,
                              FloatBuffer value );

    // C function void glProgramUniform2fv ( GLuint program, GLint location, GLsizei count, const GLfloat *value )

// void glProgramUniform2fv(
// int program,
// int location,
// int count,
// float[] value,
// int offset
// );

    // C function void glProgramUniform2fv ( GLuint program, GLint location, GLsizei count, const GLfloat *value )

    void GLProgramUniform2Fv( int program,
                              int location, 
                              // int count,
                              FloatBuffer value );

    // C function void glProgramUniform3fv ( GLuint program, GLint location, GLsizei count, const GLfloat *value )

// void glProgramUniform3fv(
// int program,
// int location,
// int count,
// float[] value,
// int offset
// );

    // C function void glProgramUniform3fv ( GLuint program, GLint location, GLsizei count, const GLfloat *value )

    void GLProgramUniform3Fv( int program,
                              int location, 
                              // int count,
                              FloatBuffer value );

    // C function void glProgramUniform4fv ( GLuint program, GLint location, GLsizei count, const GLfloat *value )

// void glProgramUniform4fv(
// int program,
// int location,
// int count,
// float[] value,
// int offset
// );

    // C function void glProgramUniform4fv ( GLuint program, GLint location, GLsizei count, const GLfloat *value )

    void GLProgramUniform4Fv( int program,
                              int location, 
                              // int count,
                              FloatBuffer value );

    // C function void glProgramUniformMatrix2fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix2fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix2fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix2Fv( int program,
                                    int location, 
                                    // int count,
                                    bool transpose,
                                    FloatBuffer value );

    // C function void glProgramUniformMatrix3fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix3fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix3fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix3Fv( int program,
                                    int location, 
                                    // int count,
                                    bool transpose,
                                    FloatBuffer value );

    // C function void glProgramUniformMatrix4fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix4fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix4fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix4Fv( int program,
                                    int location, 
                                    // int count,
                                    bool transpose,
                                    FloatBuffer value );

    // C function void glProgramUniformMatrix2x3fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix2x3fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix2x3fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix2X3Fv( int program,
                                      int location, 
                                      // int count,
                                      bool transpose,
                                      FloatBuffer value );

    // C function void glProgramUniformMatrix3x2fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix3x2fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix3x2fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix3X2Fv( int program,
                                      int location, 
                                      // int count,
                                      bool transpose,
                                      FloatBuffer value );

    // C function void glProgramUniformMatrix2x4fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix2x4fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix2x4fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix2X4Fv( int program,
                                      int location, 
                                      // int count,
                                      bool transpose,
                                      FloatBuffer value );

    // C function void glProgramUniformMatrix4x2fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix4x2fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix4x2fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix4X2Fv( int program,
                                      int location, 
                                      // int count,
                                      bool transpose,
                                      FloatBuffer value );

    // C function void glProgramUniformMatrix3x4fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix3x4fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix3x4fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix3X4Fv( int program,
                                      int location, 
                                      // int count,
                                      bool transpose,
                                      FloatBuffer value );

    // C function void glProgramUniformMatrix4x3fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

// void glProgramUniformMatrix4x3fv(
// int program,
// int location,
// int count,
// bool transpose,
// float[] value,
// int offset
// );

    // C function void glProgramUniformMatrix4x3fv ( GLuint program, GLint location, GLsizei count, GLboolean transpose, const
    // GLfloat *value )

    void GLProgramUniformMatrix4X3Fv( int program,
                                      int location, 
                                      // int count,
                                      bool transpose,
                                      FloatBuffer value );

    // C function void glValidateProgramPipeline ( GLuint pipeline )

    void GLValidateProgramPipeline( int pipeline );

    // C function void glGetProgramPipelineInfoLog( GLuint program, GLsizei maxLength, GLsizei * length, GLchar * infoLog);

    string GLGetProgramPipelineInfoLog( int program );

    // C function void glBindImageTexture ( GLuint unit, GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum
    // access, GLenum format )

    void GLBindImageTexture( int unit, int texture, int level, bool layered, int layer, int access, int format );

    // C function void glGetBooleani_v ( GLenum target, GLuint index, GLboolean *data )

// void glGetBooleani_v(
// int target,
// int index,
// bool[] data,
// int offset
// );

    // C function void glGetBooleani_v ( GLenum target, GLuint index, GLboolean *data )

    void glGetBooleani_v( int target, int index, IntBuffer data );

    // C function void glMemoryBarrier ( GLbitfield barriers )

    void GLMemoryBarrier( int barriers );

    // C function void glMemoryBarrierByRegion ( GLbitfield barriers )

    void GLMemoryBarrierByRegion( int barriers );

    // C function void glTexStorage2DMultisample ( GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei
    // height, GLboolean fixedsamplelocations )

    void GLTexStorage2DMultisample( int target,
                                    int samples,
                                    int internalformat,
                                    int width,
                                    int height,
                                    bool fixedsamplelocations );

    // C function void glGetMultisamplefv ( GLenum pname, GLuint index, GLfloat *val )

// void glGetMultisamplefv(
// int pname,
// int index,
// float[] val,
// int offset
// );

    // C function void glGetMultisamplefv ( GLenum pname, GLuint index, GLfloat *val )

    void GLGetMultisamplefv( int pname, int index, FloatBuffer val );

    // C function void glSampleMaski ( GLuint maskNumber, GLbitfield mask )

    void GLSampleMaski( int maskNumber, int mask );

    // C function void glGetTexLevelParameteriv ( GLenum target, GLint level, GLenum pname, GLint *parameters )

// void glGetTexLevelParameteriv(
// int target,
// int level,
// int pname,
// int[] parameters,
// int offset
// );

    // C function void glGetTexLevelParameteriv ( GLenum target, GLint level, GLenum pname, GLint *parameters )

    void GLGetTexLevelParameteriv( int target, int level, int pname, IntBuffer parameters );

    // C function void glGetTexLevelParameterfv ( GLenum target, GLint level, GLenum pname, GLfloat *parameters )

// void glGetTexLevelParameterfv(
// int target,
// int level,
// int pname,
// float[] parameters,
// int offset
// );

    // C function void glGetTexLevelParameterfv ( GLenum target, GLint level, GLenum pname, GLfloat *parameters )

    void GLGetTexLevelParameterfv( int target, int level, int pname, FloatBuffer parameters );

    // C function void glBindVertexBuffer ( GLuint bindingindex, GLuint buffer, GLintptr offset, GLsizei stride )

    void GLBindVertexBuffer( int bindingindex, int buffer, long offset, int stride );

    // C function void glVertexAttribFormat ( GLuint attribindex, GLint size, GLenum type, GLboolean normalized, GLuint
    // relativeoffset )

    void GLVertexAttribFormat( int attribindex, int size, int type, bool normalized, int relativeoffset );

    // C function void glVertexAttribIFormat ( GLuint attribindex, GLint size, GLenum type, GLuint relativeoffset )

    void GLVertexAttribIFormat( int attribindex, int size, int type, int relativeoffset );

    // C function void glVertexAttribBinding ( GLuint attribindex, GLuint bindingindex )

    void GLVertexAttribBinding( int attribindex, int bindingindex );

    // C function void glVertexBindingDivisor ( GLuint bindingindex, GLuint divisor )

    void GLVertexBindingDivisor( int bindingindex, int divisor );
}
