// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
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

namespace Corelib.Lugh.Graphics.OpenGL;

[SuppressMessage( "ReSharper", "InconsistentNaming" )]
public partial interface IGLBindings
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    void MinSampleShading( float value );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="mode"></param>
    void BlendEquationi( uint buf, int mode );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="modeRGB"></param>
    /// <param name="modeAlpha"></param>
    void BlendEquationSeparatei( uint buf, int modeRGB, int modeAlpha );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    void BlendFunci( uint buf, int src, int dst );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="srcRGB"></param>
    /// <param name="dstRGB"></param>
    /// <param name="srcAlpha"></param>
    /// <param name="dstAlpha"></param>
    void BlendFuncSeparatei( uint buf, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="indirect"></param>
    unsafe void DrawArraysIndirect( int mode, void* indirect );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="indirect"></param>
    void DrawArraysIndirect( int mode, GLBindings.DrawArraysIndirectCommand indirect );

    unsafe void DrawElementsIndirect( int mode, int type, void* indirect );
    void DrawElementsIndirect( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect );
    void Uniform1d( int location, Double x );
    void Uniform2d( int location, Double x, Double y );
    void Uniform3d( int location, Double x, Double y, Double z );
    void Uniform4d( int location, Double x, Double y, Double z, Double w );
    unsafe void Uniform1dv( int location, int count, Double* value );
    void Uniform1dv( int location, Double[] value );
    unsafe void Uniform2dv( int location, int count, Double* value );
    void Uniform2dv( int location, Double[] value );
    unsafe void Uniform3dv( int location, int count, Double* value );
    void Uniform3dv( int location, Double[] value );
    unsafe void Uniform4dv( int location, int count, Double* value );
    void Uniform4dv( int location, Double[] value );
    unsafe void UniformMatrix2dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix2dv( int location, bool transpose, Double[] value );
    unsafe void UniformMatrix3dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix3dv( int location, bool transpose, Double[] value );
    unsafe void UniformMatrix4dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix4dv( int location, bool transpose, Double[] value );
    unsafe void UniformMatrix2x3dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix2x3dv( int location, bool transpose, Double[] value );
    unsafe void UniformMatrix2x4dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix2x4dv( int location, bool transpose, Double[] value );
    unsafe void UniformMatrix3x2dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix3x2dv( int location, bool transpose, Double[] value );
    unsafe void UniformMatrix3x4dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix3x4dv( int location, bool transpose, Double[] value );
    unsafe void UniformMatrix4x2dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix4x2dv( int location, bool transpose, Double[] value );
    unsafe void UniformMatrix4x3dv( int location, int count, bool transpose, Double* value );
    void UniformMatrix4x3dv( int location, bool transpose, Double[] value );
    unsafe void GetUniformdv( uint program, int location, Double* parameters );
    void GetUniformdv( uint program, int location, ref Double[] parameters );
    unsafe int GetSubroutineUniformLocation( uint program, int shadertype, Byte* name );
    int GetSubroutineUniformLocation( uint program, int shadertype, string name );
    unsafe uint GetSubroutineIndex( uint program, int shadertype, Byte* name );
    uint GetSubroutineIndex( uint program, int shadertype, string name );
    unsafe void GetActiveSubroutineUniformiv( uint program, int shadertype, uint index, int pname, int* values );
    void GetActiveSubroutineUniformiv( uint program, int shadertype, uint index, int pname, ref int[] values );
    unsafe void GetActiveSubroutineUniformName( uint program, int shadertype, uint index, int bufsize, int* length, Byte* name );
    string GetActiveSubroutineUniformName( uint program, int shadertype, uint index, int bufsize );
    unsafe void GetActiveSubroutineName( uint program, int shadertype, uint index, int bufsize, int* length, Byte* name );
    string GetActiveSubroutineName( uint program, int shadertype, uint index, int bufsize );
    unsafe void UniformSubroutinesuiv( int shadertype, int count, uint* indices );
    void UniformSubroutinesuiv( int shadertype, uint[] indices );
    unsafe void GetUniformSubroutineuiv( int shadertype, int location, uint* parameters );
    void GetUniformSubroutineuiv( int shadertype, int location, ref uint[] parameters );
    unsafe void GetProgramStageiv( uint program, int shadertype, int pname, int* values );
    void GetProgramStageiv( uint program, int shadertype, int pname, ref int[] values );
    void PatchParameteri( int pname, int value );
    unsafe void PatchParameterfv( int pname, float* values );
    void PatchParameterfv( int pname, float[] values );
    void BindTransformFeedback( int target, uint id );
    unsafe void DeleteTransformFeedbacks( int n, uint* ids );
    void DeleteTransformFeedbacks( params uint[] ids );
    unsafe void GenTransformFeedbacks( int n, uint* ids );
    uint[] GenTransformFeedbacks( int n );
    uint GenTransformFeedback();
    bool IsTransformFeedback( uint id );
    void PauseTransformFeedback();
    void ResumeTransformFeedback();
    void DrawTransformFeedback( int mode, uint id );
    void DrawTransformFeedbackStream( int mode, uint id, uint stream );
    void BeginQueryIndexed( int target, uint index, uint id );
    void EndQueryIndexed( int target, uint index );
    unsafe void GetQueryIndexediv( int target, uint index, int pname, int* parameters );
    void GetQueryIndexediv( int target, uint index, int pname, ref int[] parameters );
    void ReleaseShaderCompiler();
    unsafe void ShaderBinary( int count, uint* shaders, int binaryformat, void* binary, int length );
    void ShaderBinary( uint[] shaders, int binaryformat, byte[] binary );
    unsafe void GetShaderPrecisionFormat( int shaderType, int precisionType, int* range, int* precision );
    void GetShaderPrecisionFormat( int shaderType, int precisionType, ref int[] range, ref int precision );
    void DepthRangef( float n, float f );
    void ClearDepthf( float d );
    unsafe void GetProgramBinary( uint program, int bufSize, int* length, int* binaryFormat, void* binary );
    byte[] GetProgramBinary( uint program, int bufSize, out int binaryFormat );
    unsafe void ProgramBinary( uint program, int binaryFormat, void* binary, int length );
    void ProgramBinary( uint program, int binaryFormat, byte[] binary );
    void ProgramParameteri( uint program, int pname, int value );
    void UseProgramStages( uint pipeline, uint stages, uint program );
    void ActiveShaderProgram( uint pipeline, uint program );
    unsafe uint CreateShaderProgramv( int type, int count, Byte** strings );
    uint CreateShaderProgramv( int type, string[] strings );
    void BindProgramPipeline( uint pipeline );
    unsafe void DeleteProgramPipelines( int n, uint* pipelines );
    void DeleteProgramPipelines( params uint[] pipelines );
    unsafe void GenProgramPipelines( int n, uint* pipelines );
    uint[] GenProgramPipelines( int n );
    uint GenProgramPipeline();
    bool IsProgramPipeline( uint pipeline );
    unsafe void GetProgramPipelineiv( uint pipeline, int pname, int* param );
    void GetProgramPipelineiv( uint pipeline, int pname, ref int[] param );
    void ProgramUniform1i( uint program, int location, int v0 );
    unsafe void ProgramUniform1iv( uint program, int location, int count, int* value );
    void ProgramUniform1iv( uint program, int location, int[] value );
    void ProgramUniform1f( uint program, int location, float v0 );
    unsafe void ProgramUniform1fv( uint program, int location, int count, float* value );
    void ProgramUniform1fv( uint program, int location, float[] value );
    void ProgramUniform1d( uint program, int location, Double v0 );
    unsafe void ProgramUniform1dv( uint program, int location, int count, Double* value );
    void ProgramUniform1dv( uint program, int location, Double[] value );
    void ProgramUniform1ui( uint program, int location, uint v0 );
    unsafe void ProgramUniform1uiv( uint program, int location, int count, uint* value );
    void ProgramUniform1uiv( uint program, int location, uint[] value );
    void ProgramUniform2i( uint program, int location, int v0, int v1 );
    unsafe void ProgramUniform2iv( uint program, int location, int count, int* value );
    void ProgramUniform2iv( uint program, int location, int[] value );
    void ProgramUniform2f( uint program, int location, float v0, float v1 );
    unsafe void ProgramUniform2fv( uint program, int location, int count, float* value );
    void ProgramUniform2fv( uint program, int location, float[] value );
    void ProgramUniform2d( uint program, int location, Double v0, Double v1 );
    unsafe void ProgramUniform2dv( uint program, int location, int count, Double* value );
    void ProgramUniform2dv( uint program, int location, Double[] value );
    void ProgramUniform2ui( uint program, int location, uint v0, uint v1 );
    unsafe void ProgramUniform2uiv( uint program, int location, int count, uint* value );
    void ProgramUniform2uiv( uint program, int location, uint[] value );
    void ProgramUniform3i( uint program, int location, int v0, int v1, int v2 );
    unsafe void ProgramUniform3iv( uint program, int location, int count, int* value );
    void ProgramUniform3iv( uint program, int location, int[] value );
    void ProgramUniform3f( uint program, int location, float v0, float v1, float v2 );
    unsafe void ProgramUniform3fv( uint program, int location, int count, float* value );
    void ProgramUniform3fv( uint program, int location, float[] value );
    void ProgramUniform3d( uint program, int location, Double v0, Double v1, Double v2 );
    unsafe void ProgramUniform3dv( uint program, int location, int count, Double* value );
    void ProgramUniform3dv( uint program, int location, Double[] value );
    void ProgramUniform3ui( uint program, int location, uint v0, uint v1, uint v2 );
    unsafe void ProgramUniform3uiv( uint program, int location, int count, uint* value );
    void ProgramUniform3uiv( uint program, int location, uint[] value );
    void ProgramUniform4i( uint program, int location, int v0, int v1, int v2, int v3 );
    unsafe void ProgramUniform4iv( uint program, int location, int count, int* value );
    void ProgramUniform4iv( uint program, int location, int[] value );
    void ProgramUniform4f( uint program, int location, float v0, float v1, float v2, float v3 );
    unsafe void ProgramUniform4fv( uint program, int location, int count, float* value );
    void ProgramUniform4fv( uint program, int location, float[] value );
    void ProgramUniform4d( uint program, int location, Double v0, Double v1, Double v2, Double v3 );
    unsafe void ProgramUniform4dv( uint program, int location, int count, Double* value );
    void ProgramUniform4dv( uint program, int location, Double[] value );
    void ProgramUniform4ui( uint program, int location, uint v0, uint v1, uint v2, uint v3 );
    unsafe void ProgramUniform4uiv( uint program, int location, int count, uint* value );
    void ProgramUniform4uiv( uint program, int location, uint[] value );
    unsafe void ProgramUniformMatrix2fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix2fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix3fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix3fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix4fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix4fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix2dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix2dv( uint program, int location, bool transpose, Double[] value );
    unsafe void ProgramUniformMatrix3dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix3dv( uint program, int location, bool transpose, Double[] value );
    unsafe void ProgramUniformMatrix4dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix4dv( uint program, int location, bool transpose, Double[] value );
    unsafe void ProgramUniformMatrix2x3fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix2x3fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix3x2fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix3x2fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix2x4fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix2x4fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix4x2fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix4x2fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix3x4fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix3x4fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix4x3fv( uint program, int location, int count, bool transpose, float* value );
    void ProgramUniformMatrix4x3fv( uint program, int location, bool transpose, float[] value );
    unsafe void ProgramUniformMatrix2x3dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix2x3dv( uint program, int location, bool transpose, Double[] value );
    unsafe void ProgramUniformMatrix3x2dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix3x2dv( uint program, int location, bool transpose, Double[] value );
    unsafe void ProgramUniformMatrix2x4dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix2x4dv( uint program, int location, bool transpose, Double[] value );
    unsafe void ProgramUniformMatrix4x2dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix4x2dv( uint program, int location, bool transpose, Double[] value );
    unsafe void ProgramUniformMatrix3x4dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix3x4dv( uint program, int location, bool transpose, Double[] value );
    unsafe void ProgramUniformMatrix4x3dv( uint program, int location, int count, bool transpose, Double* value );
    void ProgramUniformMatrix4x3dv( uint program, int location, bool transpose, Double[] value );
    void ValidateProgramPipeline( uint pipeline );
    unsafe void GetProgramPipelineInfoLog( uint pipeline, int bufSize, int* length, Byte* infoLog );
    string GetProgramPipelineInfoLog( uint pipeline, int bufSize );
    void VertexAttribL1d( uint index, Double x );
    void VertexAttribL2d( uint index, Double x, Double y );
    void VertexAttribL3d( uint index, Double x, Double y, Double z );
    void VertexAttribL4d( uint index, Double x, Double y, Double z, Double w );
    unsafe void VertexAttribL1dv( uint index, Double* v );
    void VertexAttribL1dv( uint index, Double[] v );
    unsafe void VertexAttribL2dv( uint index, Double* v );
    void VertexAttribL2dv( uint index, Double[] v );
    unsafe void VertexAttribL3dv( uint index, Double* v );
    void VertexAttribL3dv( uint index, Double[] v );
    unsafe void VertexAttribL4dv( uint index, Double* v );
    void VertexAttribL4dv( uint index, Double[] v );
    unsafe void VertexAttribLPointer( uint index, int size, int type, int stride, void* pointer );
    void VertexAttribLPointer( uint index, int size, int type, int stride, int pointer );
    unsafe void GetVertexAttribLdv( uint index, int pname, Double* parameters );
    void GetVertexAttribLdv( uint index, int pname, ref Double[] parameters );
    unsafe void ViewportArrayv( uint first, int count, float* v );
    void ViewportArrayv( uint first, int count, params float[] v );
    void ViewportIndexedf( uint index, float x, float y, float w, float h );
    unsafe void ViewportIndexedfv( uint index, float* v );
    void ViewportIndexedfv( uint index, params float[] v );
    unsafe void ScissorArrayv( uint first, int count, int* v );
    void ScissorArrayv( uint first, int count, params int[] v );
    void ScissorIndexed( uint index, int left, int bottom, int width, int height );
    unsafe void ScissorIndexedv( uint index, int* v );
    void ScissorIndexedv( uint index, params int[] v );
    unsafe void DepthRangeArrayv( uint first, int count, Double* v );
    void DepthRangeArrayv( uint first, int count, params Double[] v );
    void DepthRangeIndexed( uint index, Double n, Double f );
    unsafe void GetFloati_v( int target, uint index, float* data );
    void GetFloati_v( int target, uint index, ref float[] data );
    unsafe void GetDoublei_v( int target, uint index, Double* data );
    void GetDoublei_v( int target, uint index, ref Double[] data );
    void DrawArraysInstancedBaseInstance( int mode, int first, int count, int instancecount, uint baseinstance );
    unsafe void DrawElementsInstancedBaseInstance( int mode, int count, int type, void* indices, int instancecount, uint baseinstance );
    void DrawElementsInstancedBaseInstance< T >( int mode, int count, int type, T[] indices, int instancecount, uint baseinstance ) where T : unmanaged, IUnsignedNumber< T >;

    unsafe void DrawElementsInstancedBaseVertexBaseInstance( int mode, int count, int type, void* indices,
                                                             int instancecount, int basevertex, uint baseinstance );

    void DrawElementsInstancedBaseVertexBaseInstance< T >( int mode, int count, int type, T[] indices,
                                                           int instancecount, int basevertex, uint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >;

    unsafe void GetInternalformativ( int target, int internalformat, int pname, int bufSize, int* parameters );
    void GetInternalformativ( int target, int internalformat, int pname, int bufSize, ref int[] parameters );
    unsafe void GetActiveAtomicCounterBufferiv( uint program, uint bufferIndex, int pname, int* parameters );
    void GetActiveAtomicCounterBufferiv( uint program, uint bufferIndex, int pname, ref int[] parameters );
    void BindImageTexture( uint unit, uint texture, int level, bool layered, int layer, int access, int format );
    void MemoryBarrier( uint barriers );
    void TexStorage1D( int target, int levels, int internalformat, int width );
    void TexStorage2D( int target, int levels, int internalformat, int width, int height );
    void TexStorage3D( int target, int levels, int internalformat, int width, int height, int depth );
    void DrawTransformFeedbackInstanced( int mode, uint id, int instancecount );
    void DrawTransformFeedbackStreamInstanced( int mode, uint id, uint stream, int instancecount );
    unsafe void GetPointerv( int pname, void** parameters );
    void GetPointerv( int pname, ref IntPtr[] parameters );
    unsafe void ClearBufferData( int target, int internalformat, int format, int type, void* data );
    void ClearBufferData< T >( int target, int internalformat, int format, int type, T[] data ) where T : unmanaged;
    unsafe void ClearBufferSubData( int target, int internalformat, int offset, int size, int format, int type, void* data );
    void ClearBufferSubData< T >( int target, int internalformat, int offset, int size, int format, int type, T[] data ) where T : unmanaged;
    void DispatchCompute( uint num_groups_x, uint num_groups_y, uint num_groups_z );
    unsafe void DispatchComputeIndirect( void* indirect );
    void DispatchComputeIndirect( GLBindings.DispatchIndirectCommand indirect );

    void CopyImageSubData( uint srcName, int srcTarget, int srcLevel, int srcX, int srcY, int srcZ,
                           uint dstName, int dstTarget, int dstLevel, int dstX, int dstY,
                           int dstZ, int srcWidth, int srcHeight, int srcDepth );

    void FramebufferParameteri( int target, int pname, int param );
    unsafe void GetFramebufferParameteriv( int target, int pname, int* parameters );
    void GetFramebufferParameteriv( int target, int pname, ref int[] parameters );
    unsafe void GetInternalformati64v( int target, int internalformat, int pname, int count, Int64* parameters );
    void GetInternalformati64v( int target, int internalformat, int pname, int count, ref Int64[] parameters );
    void InvalidateTexSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth );
    void InvalidateTexImage( uint texture, int level );
    void InvalidateBufferSubData( uint buffer, int offset, int length );
    void InvalidateBufferData( uint buffer );
    unsafe void InvalidateFramebuffer( int target, int numAttachments, int* attachments );
    void InvalidateFramebuffer( int target, int numAttachments, int[] attachments );
    unsafe void InvalidateSubFramebuffer( int target, int numAttachments, int* attachments, int x, int y, int width, int height );
    void InvalidateSubFramebuffer( int target, int numAttachments, int[] attachments, int x, int y, int width, int height );
    unsafe void MultiDrawArraysIndirect( int mode, void* indirect, int drawcount, int stride );
    void MultiDrawArraysIndirect( int mode, GLBindings.DrawArraysIndirectCommand indirect, int drawcount, int stride );
    unsafe void MultiDrawElementsIndirect( int mode, int type, void* indirect, int drawcount, int stride );
    void MultiDrawElementsIndirect( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect, int drawcount, int stride );
    unsafe void GetProgramInterfaceiv( uint program, int programInterface, int pname, int* parameters );
    void GetProgramInterfaceiv( uint program, int programInterface, int pname, ref int[] parameters );
    unsafe uint GetProgramResourceIndex( uint program, int programInterface, Byte* name );
    uint GetProgramResourceIndex( uint program, int programInterface, string name );
    unsafe void GetProgramResourceName( uint program, int programInterface, uint index, int bufSize, int* length, Byte* name );
    string GetProgramResourceName( uint program, int programInterface, uint index, int bufSize );

    unsafe void GetProgramResourceiv( uint program, int programInterface, uint index, int propCount,
                                      int* props, int bufSize, int* length, int* parameters );

    void GetProgramResourceiv( uint program, int programInterface, uint index, int[] props, int bufSize, ref int[] parameters );
    unsafe int GetProgramResourceLocation( uint program, int programInterface, Byte* name );
    int GetProgramResourceLocation( uint program, int programInterface, string name );
    unsafe int GetProgramResourceLocationIndex( uint program, int programInterface, Byte* name );
    int GetProgramResourceLocationIndex( uint program, int programInterface, string name );
    void ShaderStorageBlockBinding( uint program, uint storageBlockIndex, uint storageBlockBinding );
    void TexBufferRange( int target, int internalformat, uint buffer, int offset, int size );
    void TexStorage2DMultisample( int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations );

    void TexStorage3DMultisample( int target, int samples, int internalformat, int width,
                                  int height, int depth, bool fixedsamplelocations );

    void TextureView( uint texture,
                      int target,
                      uint origtexture,
                      int internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers );

    void BindVertexBuffer( uint bindingindex, uint buffer, int offset, int stride );
    void VertexAttribFormat( uint attribindex, int size, int type, bool normalized, uint relativeoffset );
    void VertexAttribIFormat( uint attribindex, int size, int type, uint relativeoffset );
    void VertexAttribLFormat( uint attribindex, int size, int type, uint relativeoffset );
    void VertexAttribBinding( uint attribindex, uint bindingindex );
    void VertexBindingDivisor( uint bindingindex, uint divisor );
    unsafe void DebugMessageControl( int source, int type, int severity, int count, uint* ids, bool enabled );
    void DebugMessageControl( int source, int type, int severity, uint[] ids, bool enabled );
    unsafe void DebugMessageInsert( int source, int type, uint id, int severity, int length, Byte* buf );
    void DebugMessageInsert( int source, int type, uint id, int severity, string buf );
    unsafe void DebugMessageCallback( GLBindings.GLDEBUGPROC callback, void* userParam );
    unsafe void DebugMessageCallback( GLBindings.GLDEBUGPROCSAFE callback, void* userParam );

    unsafe uint GetDebugMessageLog( uint count,
                                    int bufsize,
                                    int* sources,
                                    int* types,
                                    uint* ids,
                                    int* severities,
                                    int* lengths,
                                    Byte* messageLog );

    uint GetDebugMessageLog( uint count,
                             int bufSize,
                             out int[] sources,
                             out int[] types,
                             out uint[] ids,
                             out int[] severities,
                             out string[] messageLog );

    unsafe void PushDebugGroup( int source, uint id, int length, Byte* message );
    void PushDebugGroup( int source, uint id, string message );
    void PopDebugGroup();
    unsafe void ObjectLabel( int identifier, uint name, int length, Byte* label );
    void ObjectLabel( int identifier, uint name, string label );
    unsafe void GetObjectLabel( int identifier, uint name, int bufSize, int* length, Byte* label );
    string GetObjectLabel( int identifier, uint name, int bufSize );
    unsafe void ObjectPtrLabel( void* ptr, int length, Byte* label );
    void ObjectPtrLabel( IntPtr ptr, string label );
    unsafe void GetObjectPtrLabel( void* ptr, int bufSize, int* length, Byte* label );
    string GetObjectPtrLabel( IntPtr ptr, int bufSize );
    unsafe void BufferStorage( int target, int size, void* data, uint flags );
    void BufferStorage< T >( int target, T[] data, uint flags ) where T : unmanaged;
    unsafe void ClearTexImage( uint texture, int level, int format, int type, void* data );
    void ClearTexImage< T >( uint texture, int level, int format, int type, T[] data ) where T : unmanaged;
    unsafe void ClearTexSubImage( uint texture, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, void* data );

    void ClearTexSubImage< T >( uint texture, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, T[] data )
        where T : unmanaged;

    unsafe void BindBuffersBase( int target, uint first, int count, uint* buffers );
    void BindBuffersBase( int target, uint first, uint[] buffers );
    unsafe void BindBuffersRange( int target, uint first, int count, uint* buffers, int* offsets, int* sizes );
    void BindBuffersRange( int target, uint first, uint[] buffers, int[] offsets, int[] sizes );
    unsafe void BindTextures( uint first, int count, uint* textures );
    void BindTextures( uint first, uint[] textures );
    unsafe void BindSamplers( uint first, int count, uint* samplers );
    void BindSamplers( uint first, uint[] samplers );
    unsafe void BindImageTextures( uint first, int count, uint* textures );
    void BindImageTextures( uint first, uint[] textures );
    unsafe void BindVertexBuffers( uint first, int count, uint* buffers, int* offsets, int* strides );
    void BindVertexBuffers( uint first, uint[] buffers, int[] offsets, int[] strides );
    void ClipControl( int origin, int depth );
    unsafe void CreateTransformFeedbacks( int n, uint* ids );
    uint[] CreateTransformFeedbacks( int n );
    uint CreateTransformFeedbacks();
    void TransformFeedbackBufferBase( uint xfb, uint index, uint buffer );
    void TransformFeedbackBufferRange( uint xfb, uint index, uint buffer, int offset, int size );
    unsafe void GetTransformFeedbackiv( uint xfb, int pname, int* param );
    void GetTransformFeedbackiv( uint xfb, int pname, ref int[] param );
    unsafe void GetTransformFeedbacki_v( uint xfb, int pname, uint index, int* param );
    void GetTransformFeedbacki_v( uint xfb, int pname, uint index, ref int[] param );
    unsafe void GetTransformFeedbacki64_v( uint xfb, int pname, uint index, Int64* param );
    void GetTransformFeedbacki64_v( uint xfb, int pname, uint index, ref Int64[] param );
    unsafe void CreateBuffers( int n, uint* buffers );
    uint[] CreateBuffers( int n );
    uint CreateBuffer();
    unsafe void NamedBufferStorage( uint buffer, int size, void* data, uint flags );
    void NamedBufferStorage< T >( uint buffer, int size, T[] data, uint flags ) where T : unmanaged;
    unsafe void NamedBufferData( uint buffer, int size, void* data, int usage );
    void NamedBufferData< T >( uint buffer, int size, T[] data, int usage ) where T : unmanaged;
    unsafe void NamedBufferSubData( uint buffer, int offset, int size, void* data );
    void NamedBufferSubData< T >( uint buffer, int offset, int size, T[] data ) where T : unmanaged;
    void CopyNamedBufferSubData( uint readBuffer, uint writeBuffer, int readOffset, int writeOffset, int size );
    unsafe void ClearNamedBufferData( uint buffer, int internalformat, int format, int type, void* data );
    void ClearNamedBufferData< T >( uint buffer, int internalformat, int format, int type, T[] data ) where T : unmanaged;
    unsafe void ClearNamedBufferSubData( uint buffer, int internalformat, int offset, int size, int format, int type, void* data );
    void ClearNamedBufferSubData< T >( uint buffer, int internalformat, int offset, int size, int format, int type, T[] data ) where T : unmanaged;
    unsafe void* MapNamedBuffer( uint buffer, int access );
    System.Span< T > MapNamedBuffer< T >( uint buffer, int access ) where T : unmanaged;
    unsafe void* MapNamedBufferRange( uint buffer, int offset, int length, uint access );
    System.Span< T > MapNamedBufferRange< T >( uint buffer, int offset, int length, uint access ) where T : unmanaged;
    bool UnmapNamedBuffer( uint buffer );
    void FlushMappedNamedBufferRange( uint buffer, int offset, int length );
    unsafe void GetNamedBufferParameteriv( uint buffer, int pname, int* parameters );
    void GetNamedBufferParameteriv( uint buffer, int pname, ref int[] parameters );
    unsafe void GetNamedBufferParameteri64v( uint buffer, int pname, Int64* parameters );
    void GetNamedBufferParameteri64v( uint buffer, int pname, ref Int64[] parameters );
    unsafe void GetNamedBufferPointerv( uint buffer, int pname, void** parameters );
    void GetNamedBufferPointerv( uint buffer, int pname, ref IntPtr[] parameters );
    unsafe void GetNamedBufferSubData( uint buffer, int offset, int size, void* data );
    T[] GetNamedBufferSubData< T >( uint buffer, int offset, int size ) where T : unmanaged;
    unsafe void CreateFramebuffers( int n, uint* framebuffers );
    uint[] CreateFramebuffers( int n );
    uint CreateFramebuffer();
    void NamedFramebufferRenderbuffer( uint framebuffer, int attachment, int renderbuffertarget, uint renderbuffer );
    void NamedFramebufferParameteri( uint framebuffer, int pname, int param );
    void NamedFramebufferTexture( uint framebuffer, int attachment, uint texture, int level );
    void NamedFramebufferTextureLayer( uint framebuffer, int attachment, uint texture, int level, int layer );
    void NamedFramebufferDrawBuffer( uint framebuffer, int buf );
    unsafe void NamedFramebufferDrawBuffers( uint framebuffer, int n, int* bufs );
    void NamedFramebufferDrawBuffers( uint framebuffer, int[] bufs );
    void NamedFramebufferReadBuffer( uint framebuffer, int src );
    unsafe void InvalidateNamedFramebufferData( uint framebuffer, int numAttachments, int* attachments );
    void InvalidateNamedFramebufferData( uint framebuffer, int[] attachments );
    unsafe void InvalidateNamedFramebufferSubData( uint framebuffer, int numAttachments, int* attachments, int x, int y, int width, int height );
    void InvalidateNamedFramebufferSubData( uint framebuffer, int[] attachments, int x, int y, int width, int height );
    unsafe void ClearNamedFramebufferiv( uint framebuffer, int buffer, int drawbuffer, int* value );
    void ClearNamedFramebufferiv( uint framebuffer, int buffer, int drawbuffer, int[] value );
    unsafe void ClearNamedFramebufferuiv( uint framebuffer, int buffer, int drawbuffer, uint* value );
    void ClearNamedFramebufferuiv( uint framebuffer, int buffer, int drawbuffer, uint[] value );
    unsafe void ClearNamedFramebufferfv( uint framebuffer, int buffer, int drawbuffer, float* value );
    void ClearNamedFramebufferfv( uint framebuffer, int buffer, int drawbuffer, float[] value );
    void ClearNamedFramebufferfi( uint framebuffer, int buffer, float depth, int stencil );

    void BlitNamedFramebuffer( uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1,
                               uint mask, int filter );

    int CheckNamedFramebufferStatus( uint framebuffer, int target );
    unsafe void GetNamedFramebufferParameteriv( uint framebuffer, int pname, int* param );
    void GetNamedFramebufferParameteriv( uint framebuffer, int pname, ref int[] param );
    unsafe void GetNamedFramebufferAttachmentParameteriv( uint framebuffer, int attachment, int pname, int* params_ );
    void GetNamedFramebufferAttachmentParameteriv( uint framebuffer, int attachment, int pname, ref int[] params_ );
    unsafe void CreateRenderbuffers( int n, uint* renderbuffers );
    uint[] CreateRenderbuffers( int n );
    uint CreateRenderbuffer();
    void NamedRenderbufferStorage( uint renderbuffer, int internalformat, int width, int height );
    void NamedRenderbufferStorageMultisample( uint renderbuffer, int samples, int internalformat, int width, int height );
    unsafe void GetNamedRenderbufferParameteriv( uint renderbuffer, int pname, int* param );
    void GetNamedRenderbufferParameteriv( uint renderbuffer, int pname, ref int[] param );
    unsafe void CreateTextures( int target, int n, uint* textures );
    uint[] CreateTextures( int target, int n );
    uint CreateTexture( int target );
    void TextureBuffer( uint texture, int internalformat, uint buffer );
    void TextureBufferRange( uint texture, int internalformat, uint buffer, int offset, int size );
    void TextureStorage1D( uint texture, int levels, int internalformat, int width );
    void TextureStorage2D( uint texture, int levels, int internalformat, int width, int height );
    void TextureStorage3D( uint texture, int levels, int internalformat, int width, int height, int depth );
    void TextureStorage2DMultisample( uint texture, int samples, int internalformat, int width, int height, bool fixedsamplelocations );
    void TextureStorage3DMultisample( uint texture, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations );
    unsafe void TextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int type, void* pixels );
    void TextureSubImage1D< T >( uint texture, int level, int xoffset, int width, int format, int type, T[] pixels ) where T : unmanaged;
    unsafe void TextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels );
    void TextureSubImage2D< T >( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, T[] pixels ) where T : unmanaged;
    unsafe void TextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, void* pixels );

    void TextureSubImage3D< T >( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type,
                                 T[] pixels ) where T : unmanaged;

    unsafe void CompressedTextureSubImage1D( uint texture, int level, int xoffset, int width,
                                             int format, int imageSize, void* data );

    void CompressedTextureSubImage1D( uint texture, int level, int xoffset, int width,
                                      int format, int imageSize, byte[] data );

    unsafe void CompressedTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, void* data );
    void CompressedTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, byte[] data );

    unsafe void CompressedTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize,
                                             void* data );

    void CompressedTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize,
                                      byte[] data );

    void CopyTextureSubImage1D( uint texture, int level, int xoffset, int x, int y, int width );
    void CopyTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height );

    void CopyTextureSubImage3D( uint texture, int level, int xoffset, int yoffset,
                                int zoffset, int x, int y, int width, int height );

    void TextureParameterf( uint texture, int pname, float param );
    unsafe void TextureParameterfv( uint texture, int pname, float* param );
    void TextureParameterfv( uint texture, int pname, float[] param );
    void TextureParameteri( uint texture, int pname, int param );
    unsafe void TextureParameterIiv( uint texture, int pname, int* param );
    void TextureParameterIiv( uint texture, int pname, int[] param );
    unsafe void TextureParameterIuiv( uint texture, int pname, uint* param );
    void TextureParameterIuiv( uint texture, int pname, uint[] param );
    unsafe void TextureParameteriv( uint texture, int pname, int* param );
    void TextureParameteriv( uint texture, int pname, int[] param );
    void GenerateTextureMipmap( uint texture );
    void BindTextureUnit( uint unit, uint texture );
    unsafe void GetTextureImage( uint texture, int level, int format, int type, int bufSize, void* pixels );
    T[] GetTextureImage< T >( uint texture, int level, int format, int type, int bufSize ) where T : unmanaged;
    unsafe void GetCompressedTextureImage( uint texture, int level, int bufSize, void* pixels );
    byte[] GetCompressedTextureImage( uint texture, int level, int bufSize );
    unsafe void GetTextureLevelParameterfv( uint texture, int level, int pname, float* param );
    void GetTextureLevelParameterfv( uint texture, int level, int pname, ref float[] param );
    unsafe void GetTextureLevelParameteriv( uint texture, int level, int pname, int* param );
    void GetTextureLevelParameteriv( uint texture, int level, int pname, ref int[] param );
    unsafe void GetTextureParameterfv( uint texture, int pname, float* param );
    void GetTextureParameterfv( uint texture, int pname, ref float[] param );
    unsafe void GetTextureParameterIiv( uint texture, int pname, int* param );
    void GetTextureParameterIiv( uint texture, int pname, ref int[] param );
    unsafe void GetTextureParameterIuiv( uint texture, int pname, uint* param );
    void GetTextureParameterIuiv( uint texture, int pname, ref uint[] param );
    unsafe void GetTextureParameteriv( uint texture, int pname, int* param );
    void GetTextureParameteriv( uint texture, int pname, ref int[] param );
    unsafe void CreateVertexArrays( int n, uint* arrays );
    uint[] CreateVertexArrays( int n );
    uint CreateVertexArray();
    void DisableVertexArrayAttrib( uint vaobj, uint index );
    void EnableVertexArrayAttrib( uint vaobj, uint index );
    void VertexArrayElementBuffer( uint vaobj, uint buffer );
    void VertexArrayVertexBuffer( uint vaobj, uint bindingindex, uint buffer, int offset, int stride );
    unsafe void VertexArrayVertexBuffers( uint vaobj, uint first, int count, uint* buffers, int* offsets, int* strides );
    void VertexArrayVertexBuffers( uint vaobj, uint first, uint[] buffers, int[] offsets, int[] strides );
    void VertexArrayAttribBinding( uint vaobj, uint attribindex, uint bindingindex );
    void VertexArrayAttribFormat( uint vaobj, uint attribindex, int size, int type, bool normalized, uint relativeoffset );
    void VertexArrayAttribIFormat( uint vaobj, uint attribindex, int size, int type, uint relativeoffset );
    void VertexArrayAttribLFormat( uint vaobj, uint attribindex, int size, int type, uint relativeoffset );
    void VertexArrayBindingDivisor( uint vaobj, uint bindingindex, uint divisor );
    unsafe void GetVertexArrayiv( uint vaobj, int pname, int* param );
    void GetVertexArrayiv( uint vaobj, int pname, ref int[] param );
    unsafe void GetVertexArrayIndexediv( uint vaobj, uint index, int pname, int* param );
    void GetVertexArrayIndexediv( uint vaobj, uint index, int pname, ref int[] param );
    unsafe void GetVertexArrayIndexed64iv( uint vaobj, uint index, int pname, Int64* param );
    void GetVertexArrayIndexed64iv( uint vaobj, uint index, int pname, ref Int64[] param );
    unsafe void CreateSamplers( int n, uint* samplers );
    uint[] CreateSamplers( int n );
    uint CreateSamplers();
    unsafe void CreateProgramPipelines( int n, uint* pipelines );
    uint[] CreateProgramPipelines( int n );
    uint CreateProgramPipeline();
    unsafe void CreateQueries( int target, int n, uint* ids );
    uint[] CreateQueries( int target, int n );
    uint CreateQuery( int target );
    void GetQueryBufferObjecti64v( uint id, uint buffer, int pname, int offset );
    void GetQueryBufferObjectiv( uint id, uint buffer, int pname, int offset );
    void GetQueryBufferObjectui64v( uint id, uint buffer, int pname, int offset );
    void GetQueryBufferObjectuiv( uint id, uint buffer, int pname, int offset );
    void MemoryBarrierByRegion( uint barriers );

    unsafe void GetTextureSubImage( uint texture, int level, int xoffset, int yoffset,
                                    int zoffset, int width, int height, int depth,
                                    int format, int type, int bufSize, void* pixels );

    byte[] GetTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset,
                               int width, int height, int depth, int format, int type,
                               int bufSize );

    unsafe void GetCompressedTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, void* pixels );
    byte[] GetCompressedTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize );
    int GetGraphicsResetStatus();
    unsafe void GetnCompressedTexImage( int target, int lod, int bufSize, void* pixels );
    byte[] GetnCompressedTexImage( int target, int lod, int bufSize );
    unsafe void GetnTexImage( int target, int level, int format, int type, int bufSize, void* pixels );
    byte[] GetnTexImage( int target, int level, int format, int type, int bufSize );
    unsafe void GetnUniformdv( uint program, int location, int bufSize, Double* parameters );
    void GetnUniformdv( uint program, int location, int bufSize, ref Double[] parameters );
    unsafe void GetnUniformfv( uint program, int location, int bufSize, float* parameters );
    void GetnUniformfv( uint program, int location, int bufSize, ref float[] parameters );
    unsafe void GetnUniformiv( uint program, int location, int bufSize, int* parameters );
    void GetnUniformiv( uint program, int location, int bufSize, ref int[] parameters );
    unsafe void GetnUniformuiv( uint program, int location, int bufSize, uint* parameters );
    void GetnUniformuiv( uint program, int location, int bufSize, ref uint[] parameters );
    unsafe void ReadnPixels( int x, int y, int width, int height, int format, int type, int bufSize, void* data );
    byte[] ReadnPixels( int x, int y, int width, int height, int format, int type, int bufSize );
    void TextureBarrier();
    unsafe void SpecializeShader( uint shader, Byte* pEntryPoint, uint numSpecializationConstants, uint* pConstantIndex, uint* pConstantValue );
    void SpecializeShader( uint shader, string pEntryPoint, uint numSpecializationConstants, uint[] pConstantIndex, uint[] pConstantValue );
    unsafe void MultiDrawArraysIndirectCount( int mode, void* indirect, int drawcount, int maxdrawcount, int stride );
    void MultiDrawArraysIndirectCount( int mode, GLBindings.DrawArraysIndirectCommand indirect, int drawcount, int maxdrawcount, int stride );
    unsafe void MultiDrawElementsIndirectCount( int mode, int type, void* indirect, int drawcount, int maxdrawcount, int stride );
    void MultiDrawElementsIndirectCount( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect, int drawcount, int maxdrawcount, int stride );

    /// <summary>
    /// Controls the parameters of polygon offset.
    /// </summary>
    /// <param name="factor">Specifies a scale factor that is used to create a variable depth offset for each polygon. The initial value is 0.</param>
    /// <param name="units">Is multiplied by an implementation-specific value to create a constant depth offset. The initial value is 0.</param>
    /// <param name="clamp">Specifies the maximum (or minimum) depth clamping value. The initial value is 0.</param>
    void PolygonOffsetClamp( float factor, float units, float clamp );
}