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

namespace LibGDXSharp.Graphics.Profiling;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class GL30Interceptor : GLInterceptor, IGL30
{
    public IGL30 GL30 { get; set; }
    
    public GL30Interceptor( GLProfiler glProfiler, IGL30 gl30 )
        : base( glProfiler )
    {
    }

    public override void GLActiveTexture( int texture )
    {
    }

    public override void GLBindTexture( int target, int texture )
    {
    }

    public override void GLBlendFunc( int sfactor, int dfactor )
    {
    }

    public override void GLClear( int mask )
    {
    }

    public override void GLClearColor( float red, float green, float blue, float alpha )
    {
    }

    public override void GLClearDepthf( float depth )
    {
    }

    public override void GLClearStencil( int s )
    {
    }

    public override void GLColorMask( bool red, bool green, bool blue, bool alpha )
    {
    }

    public override void GLCompressedTexImage2D( int target,
                                                 int level,
                                                 int internalformat,
                                                 int width,
                                                 int height,
                                                 int border,
                                                 int imageSize,
                                                 Buffer data )
    {
    }

    public override void GLCompressedTexSubImage2D( int target,
                                                    int level,
                                                    int xoffset,
                                                    int yoffset,
                                                    int width,
                                                    int height,
                                                    int format,
                                                    int imageSize,
                                                    Buffer data )
    {
    }

    public override void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border )
    {
    }

    public override void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
    }

    public override void GLCullFace( int mode )
    {
    }

    public override void GLDeleteTextures( int n, IntBuffer textures )
    {
    }

    public override void GLDeleteTexture( int texture )
    {
    }

    public override void GLDepthFunc( int func )
    {
    }

    public override void GLDepthMask( bool flag )
    {
    }

    public override void GLDepthRangef( float zNear, float zFar )
    {
    }

    public override void GLDisable( int cap )
    {
    }

    public override void GLDrawArrays( int mode, int first, int count )
    {
    }

    public override void GLDrawElements( int mode, int count, int type, Buffer indices )
    {
    }

    public override void GLEnable( int cap )
    {
    }

    public override void GLFinish()
    {
    }

    public override void GLFlush()
    {
    }

    public override void GLFrontFace( int mode )
    {
    }

    public override void GLGenTextures( int n, IntBuffer textures )
    {
    }

    public override int GLGenTexture() => 0;

    public override int GLGetError() => 0;

    public override void GLGetIntegerv( int pname, IntBuffer parameters )
    {
    }

    public override string GLGetString( int name ) => null;

    public override void GLHint( int target, int mode )
    {
    }

    public override void GLLineWidth( float width )
    {
    }

    public override void GLPixelStorei( int pname, int param )
    {
    }

    public override void GLPolygonOffset( float factor, float units )
    {
    }

    public override void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels )
    {
    }

    public override void GLScissor( int x, int y, int width, int height )
    {
    }

    public override void GLStencilFunc( int func, int reference, int mask )
    {
    }

    public override void GLStencilMask( int mask )
    {
    }

    public override void GLStencilOp( int fail, int zfail, int zpass )
    {
    }

    public override void GLTexImage2D( int target,
                                       int level,
                                       int internalformat,
                                       int width,
                                       int height,
                                       int border,
                                       int format,
                                       int type,
                                       Buffer pixels )
    {
    }

    public override void GLTexParameterf( int target, int pname, float param )
    {
    }

    public override void GLTexSubImage2D( int target,
                                          int level,
                                          int xoffset,
                                          int yoffset,
                                          int width,
                                          int height,
                                          int format,
                                          int type,
                                          Buffer pixels )
    {
    }

    public override void GLViewport( int x, int y, int width, int height )
    {
    }

    public override void GLAttachShader( int program, int shader )
    {
    }

    public override void GLBindAttribLocation( int program, int index, string name )
    {
    }

    public override void GLBindBuffer( int target, int buffer )
    {
    }

    public override void GLBindFramebuffer( int target, int framebuffer )
    {
    }

    public override void GLBindRenderbuffer( int target, int renderbuffer )
    {
    }

    public override void GLBlendColor( float red, float green, float blue, float alpha )
    {
    }

    public override void GLBlendEquation( int mode )
    {
    }

    public override void GLBlendEquationSeparate( int modeRgb, int modeAlpha )
    {
    }

    public override void GLBlendFuncSeparate( int srcRgb, int dstRgb, int srcAlpha, int dstAlpha )
    {
    }

    public override void GLBufferData( int target, int size, Buffer data, int usage )
    {
    }

    public override void GLBufferSubData( int target, int offset, int size, Buffer data )
    {
    }

    public override int GLCheckFramebufferStatus( int target ) => 0;

    public override void GLCompileShader( int shader )
    {
    }

    public override int GLCreateProgram() => 0;

    public override int GLCreateShader( int type ) => 0;

    public override void GLDeleteBuffer( int buffer )
    {
    }

    public override void GLDeleteBuffers( int n, IntBuffer buffers )
    {
    }

    public override void GLDeleteFramebuffer( int framebuffer )
    {
    }

    public override void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    public override void GLDeleteProgram( int program )
    {
    }

    public override void GLDeleteRenderbuffer( int renderbuffer )
    {
    }

    public override void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    public override void GLDeleteShader( int shader )
    {
    }

    public override void GLDetachShader( int program, int shader )
    {
    }

    public override void GLDisableVertexAttribArray( int index )
    {
    }

    public override void GLDrawElements( int mode, int count, int type, int indices )
    {
    }

    public override void GLEnableVertexAttribArray( int index )
    {
    }

    public override void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer )
    {
    }

    public override void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
    {
    }

    public override int GLGenBuffer() => 0;

    public override void GLGenBuffers( int n, IntBuffer buffers )
    {
    }

    public override void GLGenerateMipmap( int target )
    {
    }

    public override int GLGenFramebuffer() => 0;

    public override void GLGenFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    public override int GLGenRenderbuffer() => 0;

    public override void GLGenRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    public override string GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type ) => null;

    public override string GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type ) => null;

    public override void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders )
    {
    }

    public override int GLGetAttribLocation( int program, string name ) => 0;

    public override void GLGetboolv( int pname, Buffer parameters )
    {
    }

    public override void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    public override void GLGetFloatv( int pname, FloatBuffer parameters )
    {
    }

    public override void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
    {
    }

    public override void GLGetProgramiv( int program, int pname, IntBuffer parameters )
    {
    }

    public override string GLGetProgramInfoLog( int program ) => null;

    public override void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    public override void GLGetShaderiv( int shader, int pname, IntBuffer parameters )
    {
    }

    public override string GLGetShaderInfoLog( int shader ) => null;

    public override void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
    {
    }

    public override void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
    }

    public override void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    public override void GLGetUniformfv( int program, int location, FloatBuffer parameters )
    {
    }

    public override void GLGetUniformiv( int program, int location, IntBuffer parameters )
    {
    }

    public override int GLGetUniformLocation( int program, string name ) => 0;

    public override void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
    {
    }

    public override void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
    {
    }

    public override void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
    {
    }

    public override bool GLIsBuffer( int buffer ) => false;

    public override bool GLIsEnabled( int cap ) => false;

    public override bool GLIsFramebuffer( int framebuffer ) => false;

    public override bool GLIsProgram( int program ) => false;

    public override bool GLIsRenderbuffer( int renderbuffer ) => false;

    public override bool GLIsShader( int shader ) => false;

    public override bool GLIsTexture( int texture ) => false;

    public override void GLLinkProgram( int program )
    {
    }

    public override void GLReleaseShaderCompiler()
    {
    }

    public override void GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
    }

    public override void GLSampleCoverage( float value, bool invert )
    {
    }

    public override void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
    {
    }

    public override void GLShaderSource( int shader, string str )
    {
    }

    public override void GLStencilFuncSeparate( int face, int func, int reference, int mask )
    {
    }

    public override void GLStencilMaskSeparate( int face, int mask )
    {
    }

    public override void GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
    }

    public override void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
    }

    public override void GLTexParameteri( int target, int pname, int param )
    {
    }

    public override void GLTexParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    public override void GLUniform1F( int location, float x )
    {
    }

    public override void GLUniform1Fv( int location, int count, FloatBuffer v )
    {
    }

    public override void GLUniform1Fv( int location, int count, float[] v, int offset )
    {
    }

    public override void GLUniform1I( int location, int x )
    {
    }

    public override void GLUniform1Iv( int location, int count, IntBuffer v )
    {
    }

    public override void GLUniform1Iv( int location, int count, int[] v, int offset )
    {
    }

    public override void GLUniform2F( int location, float x, float y )
    {
    }

    public override void GLUniform2Fv( int location, int count, FloatBuffer v )
    {
    }

    public override void GLUniform2Fv( int location, int count, float[] v, int offset )
    {
    }

    public override void GLUniform2I( int location, int x, int y )
    {
    }

    public override void GLUniform2Iv( int location, int count, IntBuffer v )
    {
    }

    public override void GLUniform2Iv( int location, int count, int[] v, int offset )
    {
    }

    public override void GLUniform3F( int location, float x, float y, float z )
    {
    }

    public override void GLUniform3Fv( int location, int count, FloatBuffer v )
    {
    }

    public override void GLUniform3Fv( int location, int count, float[] v, int offset )
    {
    }

    public override void GLUniform3I( int location, int x, int y, int z )
    {
    }

    public override void GLUniform3Iv( int location, int count, IntBuffer v )
    {
    }

    public override void GLUniform3Iv( int location, int count, int[] v, int offset )
    {
    }

    public override void GLUniform4F( int location, float x, float y, float z, float w )
    {
    }

    public override void GLUniform4Fv( int location, int count, FloatBuffer v )
    {
    }

    public override void GLUniform4Fv( int location, int count, float[] v, int offset )
    {
    }

    public override void GLUniform4I( int location, int x, int y, int z, int w )
    {
    }

    public override void GLUniform4Iv( int location, int count, IntBuffer v )
    {
    }

    public override void GLUniform4Iv( int location, int count, int[] v, int offset )
    {
    }

    public override void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public override void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    public override void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public override void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    public override void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public override void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    public override void GLUseProgram( int program )
    {
    }

    public override void GLValidateProgram( int program )
    {
    }

    public override void GLVertexAttrib1F( int indx, float x )
    {
    }

    public override void GLVertexAttrib1Fv( int indx, FloatBuffer values )
    {
    }

    public override void GLVertexAttrib2F( int indx, float x, float y )
    {
    }

    public override void GLVertexAttrib2Fv( int indx, FloatBuffer values )
    {
    }

    public override void GLVertexAttrib3F( int indx, float x, float y, float z )
    {
    }

    public override void GLVertexAttrib3Fv( int indx, FloatBuffer values )
    {
    }

    public override void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
    {
    }

    public override void GLVertexAttrib4Fv( int indx, FloatBuffer values )
    {
    }

    public void GLReadBuffer( int mode )
    {
    }

    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, Buffer indices )
    {
    }

    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, int offset )
    {
    }

    public void GLTexImage3D( int target,
                              int level,
                              int internalformat,
                              int width,
                              int height,
                              int depth,
                              int border,
                              int format,
                              int type,
                              Buffer pixels )
    {
    }

    public void GLTexImage3D( int target,
                              int level,
                              int internalformat,
                              int width,
                              int height,
                              int depth,
                              int border,
                              int format,
                              int type,
                              int offset )
    {
    }

    public void GLTexSubImage3D( int target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int zoffset,
                                 int width,
                                 int height,
                                 int depth,
                                 int format,
                                 int type,
                                 Buffer pixels )
    {
    }

    public void GLTexSubImage3D( int target,
                                 int level,
                                 int xoffset,
                                 int yoffset,
                                 int zoffset,
                                 int width,
                                 int height,
                                 int depth,
                                 int format,
                                 int type,
                                 int offset )
    {
    }

    public void GLCopyTexSubImage3D( int target,
                                     int level,
                                     int xoffset,
                                     int yoffset,
                                     int zoffset,
                                     int x,
                                     int y,
                                     int width,
                                     int height )
    {
    }

    public void GLGenQueries( int n, int[] ids, int offset )
    {
    }

    public void GLGenQueries( int n, IntBuffer ids )
    {
    }

    public void GLDeleteQueries( int n, int[] ids, int offset )
    {
    }

    public void GLDeleteQueries( int n, IntBuffer ids )
    {
    }

    public bool GLIsQuery( int id ) => false;

    public void GLBeginQuery( int target, int id )
    {
    }

    public void GLEndQuery( int target )
    {
    }

    public void GLGetQueryiv( int target, int pname, IntBuffer param )
    {
    }

    public void GLGetQueryObjectuiv( int id, int pname, IntBuffer param )
    {
    }

    public bool GLUnmapBuffer( int target ) => false;

    public Buffer GLGetBufferPointerv( int target, int pname ) => null;

    public void GLDrawBuffers( int n, IntBuffer bufs )
    {
    }

    public void GLUniformMatrix2X3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix3X2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix2X4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix4X2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix3X4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix4X3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLBlitFramebuffer( int srcX0,
                                   int srcY0,
                                   int srcX1,
                                   int srcY1,
                                   int dstX0,
                                   int dstY0,
                                   int dstX1,
                                   int dstY1,
                                   int mask,
                                   int filter )
    {
    }

    public void GLRenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height )
    {
    }

    public void GLFramebufferTextureLayer( int target, int attachment, int texture, int level, int layer )
    {
    }

    public Buffer GLMapBufferRange( int target, int offset, int length, int access ) => null;

    public void GLFlushMappedBufferRange( int target, int offset, int length )
    {
    }

    public void GLBindVertexArray( int array )
    {
    }

    public void GLDeleteVertexArrays( int n, int[] arrays, int offset )
    {
    }

    public void GLDeleteVertexArrays( int n, IntBuffer arrays )
    {
    }

    public void GLGenVertexArrays( int n, int[] arrays, int offset )
    {
    }

    public void GLGenVertexArrays( int n, IntBuffer arrays )
    {
    }

    public bool GLIsVertexArray( int array ) => false;

    public void GLBeginTransformFeedback( int primitiveMode )
    {
    }

    public void GLEndTransformFeedback()
    {
    }

    public void GLBindBufferRange( int target, int index, int buffer, int offset, int size )
    {
    }

    public void GLBindBufferBase( int target, int index, int buffer )
    {
    }

    public void GLTransformFeedbackVaryings( int program, string[] varyings, int bufferMode )
    {
    }

    public void GLVertexAttribIPointer( int index, int size, int type, int stride, int offset )
    {
    }

    public void GLGetVertexAttribIiv( int index, int pname, IntBuffer param )
    {
    }

    public void GLGetVertexAttribIuiv( int index, int pname, IntBuffer parameters )
    {
    }

    public void GLVertexAttribI4I( int index, int x, int y, int z, int w )
    {
    }

    public void GLVertexAttribI4Ui( int index, int x, int y, int z, int w )
    {
    }

    public void GLGetUniformuiv( int program, int location, IntBuffer parameters )
    {
    }

    public int GLGetFragDataLocation( int program, string name ) => 0;

    public void GLUniform1Uiv( int location, int count, IntBuffer value )
    {
    }

    public void GLUniform3Uiv( int location, int count, IntBuffer value )
    {
    }

    public void GLUniform4Uiv( int location, int count, IntBuffer value )
    {
    }

    public void GLClearBufferiv( int buffer, int drawbuffer, IntBuffer value )
    {
    }

    public void GLClearBufferuiv( int buffer, int drawbuffer, IntBuffer value )
    {
    }

    public void GLClearBufferfv( int buffer, int drawbuffer, FloatBuffer value )
    {
    }

    public void GLClearBufferfi( int buffer, int drawbuffer, float depth, int stencil )
    {
    }

    public string GLGetStringi( int name, int index ) => null;

    public void GLCopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size )
    {
    }

    public void GLGetUniformIndices( int program, string[] uniformNames, IntBuffer uniformIndices )
    {
    }

    public void GLGetActiveUniformsiv( int program, int uniformCount, IntBuffer uniformIndices, int pname, IntBuffer parameters )
    {
    }

    public int GLGetUniformBlockIndex( int program, string uniformBlockName ) => 0;

    public void GLGetActiveUniformBlockiv( int program, int uniformBlockIndex, int pname, IntBuffer parameters )
    {
    }

    public void GLGetActiveUniformBlockName( int program, int uniformBlockIndex, Buffer length, Buffer uniformBlockName )
    {
    }

    public string GLGetActiveUniformBlockName( int program, int uniformBlockIndex ) => null;

    public void GLUniformBlockBinding( int program, int uniformBlockIndex, int uniformBlockBinding )
    {
    }

    public void GLDrawArraysInstanced( int mode, int first, int count, int instanceCount )
    {
    }

    public void GLDrawElementsInstanced( int mode, int count, int type, int indicesOffset, int instanceCount )
    {
    }

    public void GLGetInteger64V( int pname, LongBuffer parameters )
    {
    }

    public void GLGetBufferParameteri64V( int target, int pname, LongBuffer parameters )
    {
    }

    public void GLGenSamplers( int count, int[] samplers, int offset )
    {
    }

    public void GLGenSamplers( int count, IntBuffer samplers )
    {
    }

    public void GLDeleteSamplers( int count, int[] samplers, int offset )
    {
    }

    public void GLDeleteSamplers( int count, IntBuffer samplers )
    {
    }

    public bool GLIsSampler( int sampler ) => false;

    public void GLBindSampler( int unit, int sampler )
    {
    }

    public void GLSamplerParameteri( int sampler, int pname, int param )
    {
    }

    public void GLSamplerParameteriv( int sampler, int pname, IntBuffer param )
    {
    }

    public void GLSamplerParameterf( int sampler, int pname, float param )
    {
    }

    public void GLSamplerParameterfv( int sampler, int pname, FloatBuffer param )
    {
    }

    public void GLGetSamplerParameteriv( int sampler, int pname, IntBuffer parameters )
    {
    }

    public void GLGetSamplerParameterfv( int sampler, int pname, FloatBuffer parameters )
    {
    }

    public void GLVertexAttribDivisor( int index, int divisor )
    {
    }

    public void GLBindTransformFeedback( int target, int id )
    {
    }

    public void GLDeleteTransformFeedbacks( int n, int[] ids, int offset )
    {
    }

    public void GLDeleteTransformFeedbacks( int n, IntBuffer ids )
    {
    }

    public void GLGenTransformFeedbacks( int n, int[] ids, int offset )
    {
    }

    public void GLGenTransformFeedbacks( int n, IntBuffer ids )
    {
    }

    public bool GLIsTransformFeedback( int id ) => false;

    public void GLPauseTransformFeedback()
    {
    }

    public void GLResumeTransformFeedback()
    {
    }

    public void GLProgramParameteri( int program, int pname, int value )
    {
    }

    public void GLInvalidateFramebuffer( int target, int numAttachments, IntBuffer attachments )
    {
    }

    public void GLInvalidateSubFramebuffer( int target,
                                            int numAttachments,
                                            IntBuffer attachments,
                                            int x,
                                            int y,
                                            int width,
                                            int height )
    {
    }

    /// In OpenGl core profiles (3.1+), passing a pointer to client memory is not valid.
    /// In 3.0 and later, use the other version of this function instead, pass a zero-based
    /// offset which references the buffer currently bound to GL_ARRAY_BUFFER.
    public override void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr )
    {
    }

    public override void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, int ptr )
    {
    }
}