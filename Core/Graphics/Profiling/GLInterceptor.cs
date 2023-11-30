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

using Buffer = LibGDXSharp.Core.Files.Buffers.Buffer;

namespace LibGDXSharp.Graphics.Profiling;

[PublicAPI]
public abstract class GLInterceptor : IGL20
{
    public int          Calls           { get; set; }
    public int          TextureBindings { get; set; }
    public int          DrawCalls       { get; set; }
    public int          ShaderSwitches  { get; set; }
    public FloatCounter VertexCount     { get; set; } = new( 0 );

    protected GLProfiler glProfiler;

    protected GLInterceptor( GLProfiler profiler )
    {
        this.glProfiler = profiler;
    }

    public static string ResolveErrorNumber( int error )
    {
        return error switch
               {
                   IGL20.GL_INVALID_VALUE                 => "GL_INVALID_VALUE",
                   IGL20.GL_INVALID_OPERATION             => "GL_INVALID_OPERATION",
                   IGL20.GL_INVALID_FRAMEBUFFER_OPERATION => "GL_INVALID_FRAMEBUFFER_OPERATION",
                   IGL20.GL_INVALID_ENUM                  => "GL_INVALID_ENUM",
                   IGL20.GL_OUT_OF_MEMORY                 => "GL_OUT_OF_MEMORY",
                   _ => throw new ArgumentOutOfRangeException( nameof( error ), error, null )
               };
    }

    public void Reset()
    {
        Calls           = 0;
        TextureBindings = 0;
        DrawCalls       = 0;
        ShaderSwitches  = 0;
        VertexCount.Reset();
    }

    public abstract void GLActiveTexture( uint texture );
    public abstract void GLBindTexture( int target, int texture );
    public abstract void GLBlendFunc( int sfactor, int dfactor );
    public abstract void GLClear( int mask );
    public abstract void GLClearColor( float red, float green, float blue, float alpha );
    public abstract void GLClearDepthf( float depth );
    public abstract void GLClearStencil( int s );
    public abstract void GLColorMask( bool red, bool green, bool blue, bool alpha );

    public abstract void GLCompressedTexImage2D( int target,
                                                 int level,
                                                 int internalformat,
                                                 int width,
                                                 int height,
                                                 int border,
                                                 int imageSize,
                                                 Buffer data );

    public abstract void GLCompressedTexSubImage2D( int target,
                                                    int level,
                                                    int xoffset,
                                                    int yoffset,
                                                    int width,
                                                    int height,
                                                    int format,
                                                    int imageSize,
                                                    Buffer data );

    public abstract void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border );
    public abstract void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height );
    public abstract void GLCullFace( int mode );
    public abstract void GLDeleteTextures( int n, IntBuffer textures );
    public abstract void GLDeleteTexture( int texture );
    public abstract void GLDepthFunc( int func );
    public abstract void GLDepthMask( bool flag );
    public abstract void GLDepthRangef( float zNear, float zFar );
    public abstract void GLDisable( int cap );
    public abstract void GLDrawArrays( int mode, int first, int count );
    public abstract void GLDrawElements( int mode, int count, int type, Buffer indices );
    public abstract void GLEnable( int cap );
    public abstract void GLFinish();
    public abstract void GLFlush();
    public abstract void GLFrontFace( int mode );
    public abstract void GLGenTextures( int n, IntBuffer textures );
    public abstract int GLGenTexture();
    public abstract int GLGetError();
    public abstract void GLGetIntegerv( int pname, IntBuffer parameters );
    public abstract string GLGetString( int name );
    public abstract void GLHint( int target, int mode );
    public abstract void GLLineWidth( float width );
    public abstract void GLPixelStorei( int pname, int param );
    public abstract void GLPolygonOffset( float factor, float units );
    public abstract void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels );
    public abstract void GLScissor( int x, int y, int width, int height );
    public abstract void GLStencilFunc( int func, int reference, int mask );
    public abstract void GLStencilMask( int mask );
    public abstract void GLStencilOp( int fail, int zfail, int zpass );

    public abstract void GLTexImage2D( int target,
                                       int level,
                                       int internalformat,
                                       int width,
                                       int height,
                                       int border,
                                       int format,
                                       int type,
                                       Buffer pixels );

    public abstract void GLTexParameterf( int target, int pname, float param );

    public abstract void GLTexSubImage2D( int target,
                                          int level,
                                          int xoffset,
                                          int yoffset,
                                          int width,
                                          int height,
                                          int format,
                                          int type,
                                          Buffer pixels );

    public abstract void GLViewport( int x, int y, int width, int height );
    public abstract void GLAttachShader( int program, int shader );
    public abstract void GLBindAttribLocation( int program, int index, string name );
    public abstract void GLBindBuffer( int target, int buffer );
    public abstract void GLBindFramebuffer( int target, int framebuffer );
    public abstract void GLBindRenderbuffer( int target, int renderbuffer );
    public abstract void GLBlendColor( float red, float green, float blue, float alpha );
    public abstract void GLBlendEquation( int mode );
    public abstract void GLBlendEquationSeparate( int modeRgb, int modeAlpha );
    public abstract void GLBlendFuncSeparate( int srcRgb, int dstRgb, int srcAlpha, int dstAlpha );
    public abstract void GLBufferData( int target, int size, Buffer data, int usage );
    public abstract void GLBufferSubData( int target, int offset, int size, Buffer data );
    public abstract int GLCheckFramebufferStatus( int target );
    public abstract void GLCompileShader( int shader );
    public abstract int GLCreateProgram();
    public abstract int GLCreateShader( int type );
    public abstract void GLDeleteBuffer( int buffer );
    public abstract void GLDeleteBuffers( int n, IntBuffer buffers );
    public abstract void GLDeleteFramebuffer( int framebuffer );
    public abstract void GLDeleteFramebuffers( int n, IntBuffer framebuffers );
    public abstract void GLDeleteProgram( int program );
    public abstract void GLDeleteRenderbuffer( int renderbuffer );
    public abstract void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers );
    public abstract void GLDeleteShader( int shader );
    public abstract void GLDetachShader( int program, int shader );
    public abstract void GLDisableVertexAttribArray( int index );
    public abstract void GLDrawElements( int mode, int count, int type, int indices );
    public abstract void GLEnableVertexAttribArray( int index );
    public abstract void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer );
    public abstract void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level );
    public abstract int GLGenBuffer();
    public abstract void GLGenBuffers( int n, IntBuffer buffers );
    public abstract void GLGenerateMipmap( int target );
    public abstract int GLGenFramebuffer();
    public abstract void GLGenFramebuffers( int n, IntBuffer framebuffers );
    public abstract int GLGenRenderbuffer();
    public abstract void GLGenRenderbuffers( int n, IntBuffer renderbuffers );
    public abstract string GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type );
    public abstract string GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type );
    public abstract void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders );
    public abstract int GLGetAttribLocation( int program, string name );
    public abstract void GLGetBooleanv( int pname, Buffer parameters );
    public abstract void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters );
    public abstract void GLGetFloatv( int pname, FloatBuffer parameters );
    public abstract void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters );
    public abstract void GLGetProgramiv( int program, int pname, IntBuffer parameters );
    public abstract string GLGetProgramInfoLog( int program );
    public abstract void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters );
    public abstract void GLGetShaderiv( int shader, int pname, IntBuffer parameters );
    public abstract string GLGetShaderInfoLog( int shader );
    public abstract void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision );
    public abstract void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters );
    public abstract void GLGetTexParameteriv( int target, int pname, IntBuffer parameters );
    public abstract void GLGetUniformfv( int program, int location, FloatBuffer parameters );
    public abstract void GLGetUniformiv( int program, int location, IntBuffer parameters );
    public abstract int GLGetUniformLocation( int program, string name );
    public abstract void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters );
    public abstract void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters );
    public abstract void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer );
    public abstract bool GLIsBuffer( int buffer );
    public abstract bool GLIsEnabled( int cap );
    public abstract bool GLIsFramebuffer( int framebuffer );
    public abstract bool GLIsProgram( int program );
    public abstract bool GLIsRenderbuffer( int renderbuffer );
    public abstract bool GLIsShader( int shader );
    public abstract bool GLIsTexture( int texture );
    public abstract void GLLinkProgram( int program );
    public abstract void GLReleaseShaderCompiler();
    public abstract void GLRenderbufferStorage( int target, int internalformat, int width, int height );
    public abstract void GLSampleCoverage( float value, bool invert );
    public abstract void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length );
    public abstract void GLShaderSource( int shader, string str );
    public abstract void GLStencilFuncSeparate( int face, int func, int reference, int mask );
    public abstract void GLStencilMaskSeparate( int face, int mask );
    public abstract void GLStencilOpSeparate( int face, int fail, int zfail, int zpass );
    public abstract void GLTexParameterfv( int target, int pname, FloatBuffer parameters );
    public abstract void GLTexParameteri( int target, int pname, int param );
    public abstract void GLTexParameteriv( int target, int pname, IntBuffer parameters );
    public abstract void GLUniform1F( int location, float x );
    public abstract void GLUniform1Fv( int location, int count, FloatBuffer v );
    public abstract void GLUniform1Fv( int location, int count, float[] v, int offset );
    public abstract void GLUniform1I( int location, int x );
    public abstract void GLUniform1Iv( int location, int count, IntBuffer v );
    public abstract void GLUniform1Iv( int location, int count, int[] v, int offset );
    public abstract void GLUniform2F( int location, float x, float y );
    public abstract void GLUniform2Fv( int location, int count, FloatBuffer v );
    public abstract void GLUniform2Fv( int location, int count, float[] v, int offset );
    public abstract void GLUniform2I( int location, int x, int y );
    public abstract void GLUniform2Iv( int location, int count, IntBuffer v );
    public abstract void GLUniform2Iv( int location, int count, int[] v, int offset );
    public abstract void GLUniform3F( int location, float x, float y, float z );
    public abstract void GLUniform3Fv( int location, int count, FloatBuffer v );
    public abstract void GLUniform3Fv( int location, int count, float[] v, int offset );
    public abstract void GLUniform3I( int location, int x, int y, int z );
    public abstract void GLUniform3Iv( int location, int count, IntBuffer v );
    public abstract void GLUniform3Iv( int location, int count, int[] v, int offset );
    public abstract void GLUniform4F( int location, float x, float y, float z, float w );
    public abstract void GLUniform4Fv( int location, int count, FloatBuffer v );
    public abstract void GLUniform4Fv( int location, int count, float[] v, int offset );
    public abstract void GLUniform4I( int location, int x, int y, int z, int w );
    public abstract void GLUniform4Iv( int location, int count, IntBuffer v );
    public abstract void GLUniform4Iv( int location, int count, int[] v, int offset );
    public abstract void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value );
    public abstract void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset );
    public abstract void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value );
    public abstract void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset );
    public abstract void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value );
    public abstract void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset );
    public abstract void GLUseProgram( int program );
    public abstract void GLValidateProgram( int program );
    public abstract void GLVertexAttrib1F( int indx, float x );
    public abstract void GLVertexAttrib1Fv( int indx, FloatBuffer values );
    public abstract void GLVertexAttrib2F( int indx, float x, float y );
    public abstract void GLVertexAttrib2Fv( int indx, FloatBuffer values );
    public abstract void GLVertexAttrib3F( int indx, float x, float y, float z );
    public abstract void GLVertexAttrib3Fv( int indx, FloatBuffer values );
    public abstract void GLVertexAttrib4F( int indx, float x, float y, float z, float w );
    public abstract void GLVertexAttrib4Fv( int indx, FloatBuffer values );

    /// In OpenGl core profiles (3.1+), passing a pointer to client memory is not valid.
    /// In 3.0 and later, use the other version of this function instead, pass a zero-based
    /// offset which references the buffer currently bound to GL_ARRAY_BUFFER.
    public abstract void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr );
    public abstract void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, int ptr );
}