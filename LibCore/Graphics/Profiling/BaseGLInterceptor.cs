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


using LughSharp.LibCore.Maths;
using LughSharp.LibCore.Utils.Buffers;

using Buffer = LughSharp.LibCore.Utils.Buffers.Buffer;

namespace LughSharp.LibCore.Graphics.Profiling;

public abstract class BaseGLInterceptor : IGL20
{
    protected readonly GLProfiler glProfiler;

    protected BaseGLInterceptor( GLProfiler profiler )
    {
        glProfiler = profiler;
    }

    public int          Calls           { get; set; }
    public int          TextureBindings { get; set; }
    public int          DrawCalls       { get; set; }
    public int          ShaderSwitches  { get; set; }
    public FloatCounter VertexCount     { get; set; } = new( 0 );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public abstract void GLActiveTexture( int texture );

    /// <inheritdoc />
    public abstract void GLBindTexture( int target, int texture );

    /// <inheritdoc />
    public abstract void GLBlendFunc( int sfactor, int dfactor );

    /// <inheritdoc />
    public abstract void GLClear( int mask );

    /// <inheritdoc />
    public abstract void GLClearColor( float red, float green, float blue, float alpha );

    /// <inheritdoc />
    public abstract void GLClearDepthf( float depth );

    /// <inheritdoc />
    public abstract void GLClearStencil( int s );

    /// <inheritdoc />
    public abstract void GLColorMask( bool red, bool green, bool blue, bool alpha );

    /// <inheritdoc />
    public abstract void GLCompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, int imageSize, Buffer data );

    /// <inheritdoc />
    public abstract void GLCompressedTexSubImage2D( int target,
                                                    int level,
                                                    int xoffset,
                                                    int yoffset,
                                                    int width,
                                                    int height,
                                                    int format,
                                                    int imageSize,
                                                    Buffer data );

    /// <inheritdoc />
    public abstract void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border );

    /// <inheritdoc />
    public abstract void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height );

    /// <inheritdoc />
    public abstract void GLCullFace( int mode );

    /// <inheritdoc />
    public abstract void GLDeleteTextures( int n, IntBuffer textures );

    /// <inheritdoc />
    public abstract void GLDeleteTexture( int texture );

    /// <inheritdoc />
    public abstract void GLDepthFunc( int func );

    /// <inheritdoc />
    public abstract void GLDepthMask( bool flag );

    /// <inheritdoc />
    public abstract void GLDepthRangef( float zNear, float zFar );

    /// <inheritdoc />
    public abstract void GLDisable( int cap );

    /// <inheritdoc />
    public abstract void GLDrawArrays( int mode, int first, int count );

    /// <inheritdoc />
    public abstract void GLDrawElements( int mode, int count, int type, Buffer indices );

    /// <inheritdoc />
    public abstract void GLEnable( int cap );

    /// <inheritdoc />
    public abstract void GLFinish();

    /// <inheritdoc />
    public abstract void GLFlush();

    /// <inheritdoc />
    public abstract void GLFrontFace( int mode );

    /// <inheritdoc />
    public abstract void GLGenTextures( int n, IntBuffer textures );

    /// <inheritdoc />
    public abstract int GLGenTexture();

    /// <inheritdoc />
    public abstract int GLGetError();

    /// <inheritdoc />
    public abstract void GLGetIntegerv( int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract string GLGetString( int name );

    /// <inheritdoc />
    public abstract void GLHint( int target, int mode );

    /// <inheritdoc />
    public abstract void GLLineWidth( float width );

    /// <inheritdoc />
    public abstract void GLPixelStorei( int pname, int param );

    /// <inheritdoc />
    public abstract void GLPolygonOffset( float factor, float units );

    /// <inheritdoc />
    public abstract void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels );

    /// <inheritdoc />
    public abstract void GLScissor( int x, int y, int width, int height );

    /// <inheritdoc />
    public abstract void GLStencilFunc( int func, int reference, int mask );

    /// <inheritdoc />
    public abstract void GLStencilMask( int mask );

    /// <inheritdoc />
    public abstract void GLStencilOp( int fail, int zfail, int zpass );

    /// <inheritdoc />
    public abstract void GLTexImage2D( int target, int level, int internalformat, int width, int height, int border, int format, int type, Buffer pixels );

    /// <inheritdoc />
    public abstract void GLTexParameterf( int target, int pname, float param );

    /// <inheritdoc />
    public abstract void GLTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, Buffer pixels );

    /// <inheritdoc />
    public abstract void GLViewport( int x, int y, int width, int height );

    /// <inheritdoc />
    public abstract void GLAttachShader( int program, int shader );

    /// <inheritdoc />
    public abstract void GLBindAttribLocation( int program, int index, string name );

    /// <inheritdoc />
    public abstract void GLBindBuffer( int target, int buffer );

    /// <inheritdoc />
    public abstract void GLBindFramebuffer( int target, int framebuffer );

    /// <inheritdoc />
    public abstract void GLBindRenderbuffer( int target, int renderbuffer );

    /// <inheritdoc />
    public abstract void GLBlendColor( float red, float green, float blue, float alpha );

    /// <inheritdoc />
    public abstract void GLBlendEquation( int mode );

    /// <inheritdoc />
    public abstract void GLBlendEquationSeparate( int modeRGB, int modeAlpha );

    /// <inheritdoc />
    public abstract void GLBlendFuncSeparate( int srcRGB, int dstRGB, int srcAlpha, int dstAlpha );

    /// <inheritdoc />
    public abstract void GLBufferData( int target, int size, Buffer data, int usage );

    /// <inheritdoc />
    public abstract void GLBufferSubData( int target, int offset, int size, Buffer data );

    /// <inheritdoc />
    public abstract int GLCheckFramebufferStatus( int target );

    /// <inheritdoc />
    public abstract void GLCompileShader( int shader );

    /// <inheritdoc />
    public abstract int GLCreateProgram();

    /// <inheritdoc />
    public abstract int GLCreateShader( int type );

    /// <inheritdoc />
    public abstract void GLDeleteBuffer( int buffer );

    /// <inheritdoc />
    public abstract void GLDeleteBuffers( int n, IntBuffer buffers );

    /// <inheritdoc />
    public abstract void GLDeleteFramebuffer( int framebuffer );

    /// <inheritdoc />
    public abstract void GLDeleteFramebuffers( int n, IntBuffer framebuffers );

    /// <inheritdoc />
    public abstract void GLDeleteProgram( int program );

    /// <inheritdoc />
    public abstract void GLDeleteRenderbuffer( int renderbuffer );

    /// <inheritdoc />
    public abstract void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers );

    /// <inheritdoc />
    public abstract void GLDeleteShader( int shader );

    /// <inheritdoc />
    public abstract void GLDetachShader( int program, int shader );

    /// <inheritdoc />
    public abstract void GLDisableVertexAttribArray( int index );

    /// <inheritdoc />
    public abstract void GLDrawElements( int mode, int count, int type, int indices );

    /// <inheritdoc />
    public abstract void GLEnableVertexAttribArray( int index );

    /// <inheritdoc />
    public abstract void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer );

    /// <inheritdoc />
    public abstract void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level );

    /// <inheritdoc />
    public abstract int GLGenBuffer();

    /// <inheritdoc />
    public abstract void GLGenBuffers( int n, IntBuffer buffers );

    /// <inheritdoc />
    public abstract void GLGenerateMipmap( int target );

    /// <inheritdoc />
    public abstract int GLGenFramebuffer();

    /// <inheritdoc />
    public abstract void GLGenFramebuffers( int n, IntBuffer framebuffers );

    /// <inheritdoc />
    public abstract int GLGenRenderbuffer();

    /// <inheritdoc />
    public abstract void GLGenRenderbuffers( int n, IntBuffer renderbuffers );

    /// <inheritdoc />
    public abstract string GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type );

    /// <inheritdoc />
    public abstract string GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type );

    /// <inheritdoc />
    public abstract void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders );

    /// <inheritdoc />
    public abstract int GLGetAttribLocation( int program, string name );

    /// <inheritdoc />
    public abstract void GLGetBooleanv( int pname, Buffer parameters );

    /// <inheritdoc />
    public abstract void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetFloatv( int pname, FloatBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetProgramiv( int program, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract string GLGetProgramInfoLog( int program );

    /// <inheritdoc />
    public abstract void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetShaderiv( int shader, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract string GLGetShaderInfoLog( int shader );

    /// <inheritdoc />
    public abstract void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision );

    /// <inheritdoc />
    public abstract void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetTexParameteriv( int target, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetUniformfv( int program, int location, FloatBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetUniformiv( int program, int location, IntBuffer parameters );

    /// <inheritdoc />
    public abstract int GLGetUniformLocation( int program, string name );

    /// <inheritdoc />
    public abstract void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer );

    /// <inheritdoc />
    public abstract bool GLIsBuffer( int buffer );

    /// <inheritdoc />
    public abstract bool GLIsEnabled( int cap );

    /// <inheritdoc />
    public abstract bool GLIsFramebuffer( int framebuffer );

    /// <inheritdoc />
    public abstract bool GLIsProgram( int program );

    /// <inheritdoc />
    public abstract bool GLIsRenderbuffer( int renderbuffer );

    /// <inheritdoc />
    public abstract bool GLIsShader( int shader );

    /// <inheritdoc />
    public abstract bool GLIsTexture( int texture );

    /// <inheritdoc />
    public abstract void GLLinkProgram( int program );

    /// <inheritdoc />
    public abstract void GLReleaseShaderCompiler();

    /// <inheritdoc />
    public abstract void GLRenderbufferStorage( int target, int internalformat, int width, int height );

    /// <inheritdoc />
    public abstract void GLSampleCoverage( float value, bool invert );

    /// <inheritdoc />
    public abstract void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length );

    /// <inheritdoc />
    public abstract void GLShaderSource( int shader, string str );

    /// <inheritdoc />
    public abstract void GLStencilFuncSeparate( int face, int func, int reference, int mask );

    /// <inheritdoc />
    public abstract void GLStencilMaskSeparate( int face, int mask );

    /// <inheritdoc />
    public abstract void GLStencilOpSeparate( int face, int fail, int zfail, int zpass );

    /// <inheritdoc />
    public abstract void GLTexParameterfv( int target, int pname, FloatBuffer parameters );

    /// <inheritdoc />
    public abstract void GLTexParameteri( int target, int pname, int param );

    /// <inheritdoc />
    public abstract void GLTexParameteriv( int target, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLUniform1F( int location, float x );

    /// <inheritdoc />
    public abstract void GLUniform1Fv( int location, int count, FloatBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform1Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform1I( int location, int x );

    /// <inheritdoc />
    public abstract void GLUniform1Iv( int location, int count, IntBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform1Iv( int location, int count, int[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform2F( int location, float x, float y );

    /// <inheritdoc />
    public abstract void GLUniform2Fv( int location, int count, FloatBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform2Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform2I( int location, int x, int y );

    /// <inheritdoc />
    public abstract void GLUniform2Iv( int location, int count, IntBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform2Iv( int location, int count, int[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform3F( int location, float x, float y, float z );

    /// <inheritdoc />
    public abstract void GLUniform3Fv( int location, int count, FloatBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform3Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform3I( int location, int x, int y, int z );

    /// <inheritdoc />
    public abstract void GLUniform3Iv( int location, int count, IntBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform3Iv( int location, int count, int[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform4F( int location, float x, float y, float z, float w );

    /// <inheritdoc />
    public abstract void GLUniform4Fv( int location, int count, FloatBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform4Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform4I( int location, int x, int y, int z, int w );

    /// <inheritdoc />
    public abstract void GLUniform4Iv( int location, int count, IntBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform4Iv( int location, int count, int[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value );

    /// <inheritdoc />
    public abstract void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset );

    /// <inheritdoc />
    public abstract void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value );

    /// <inheritdoc />
    public abstract void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset );

    /// <inheritdoc />
    public abstract void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value );

    /// <inheritdoc />
    public abstract void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset );

    /// <inheritdoc />
    public abstract void GLUseProgram( int program );

    /// <inheritdoc />
    public abstract void GLValidateProgram( int program );

    /// <inheritdoc />
    public abstract void GLVertexAttrib1F( int indx, float x );

    /// <inheritdoc />
    public abstract void GLVertexAttrib1Fv( int indx, FloatBuffer values );

    /// <inheritdoc />
    public abstract void GLVertexAttrib2F( int indx, float x, float y );

    /// <inheritdoc />
    public abstract void GLVertexAttrib2Fv( int indx, FloatBuffer values );

    /// <inheritdoc />
    public abstract void GLVertexAttrib3F( int indx, float x, float y, float z );

    /// <inheritdoc />
    public abstract void GLVertexAttrib3Fv( int indx, FloatBuffer values );

    /// <inheritdoc />
    public abstract void GLVertexAttrib4F( int indx, float x, float y, float z, float w );

    /// <inheritdoc />
    public abstract void GLVertexAttrib4Fv( int indx, FloatBuffer values );

    /// <inheritdoc />
    public abstract void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr );

    /// <inheritdoc />
    public abstract void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, int ptr );

    public static string ResolveErrorNumber( int error )
    {
        return error switch
               {
                   IGL20.GL_INVALID_VALUE                 => "InvalidValue",
                   IGL20.GL_INVALID_OPERATION             => "InvalidOperation",
                   IGL20.GL_INVALID_FRAMEBUFFER_OPERATION => "InvalidFramebufferOperation",
                   IGL20.GL_INVALID_ENUM                  => "InvalidEnum",
                   IGL20.GL_OUT_OF_MEMORY                 => "OutOfMemory",
                   IGL20.GL_NO_ERROR                      => "NoError",
                   _                                      => throw new ArgumentOutOfRangeException( nameof( error ), error, null )
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
}
