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
using ErrorCode = OpenGL.ErrorCode;

namespace LibGDXSharp.Graphics.Profiling;

public abstract class GLInterceptor : IGL20
{
    protected readonly GLProfiler glProfiler;

    protected GLInterceptor( GLProfiler profiler ) => glProfiler = profiler;

    public int          Calls           { get; set; }
    public int          TextureBindings { get; set; }
    public int          DrawCalls       { get; set; }
    public int          ShaderSwitches  { get; set; }
    public FloatCounter VertexCount     { get; set; } = new( 0 );

    /// <inheritdoc />
    public abstract void GLActiveTexture( TextureUnit texture );

    /// <inheritdoc />
    public abstract void GLAttachShader( uint program, uint shader );

    /// <inheritdoc />
    public abstract void GLBindTexture( TextureTarget target, uint texture );

    /// <inheritdoc />
    public abstract void GLBlendFunc( BlendingFactor sfactor, BlendingFactor dfactor );

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
    public abstract void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border );

    /// <inheritdoc />
    public abstract void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height );

    /// <inheritdoc />
    public abstract void GLCullFace( int mode );

    /// <inheritdoc />
    public abstract void GLDeleteTextures( params uint[] textures );

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
    public abstract void GLGenTextures( int[] textures );

    /// <inheritdoc />
    public abstract uint GLGenTexture();

    /// <inheritdoc />
    public abstract ErrorCode GLGetError();

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
    public abstract void GLInt( int fail, int zfail, int zpass );

    /// <inheritdoc />
    public abstract void GLTexImage2D( int target, int level, int internalformat, int width, int height, int border, int format, int type, Buffer pixels );

    /// <inheritdoc />
    public abstract void GLTexSubImage2D( TextureTarget target,
                                          int level,
                                          int xoffset,
                                          int yoffset,
                                          int width,
                                          int height,
                                          int format,
                                          int type,
                                          Buffer pixels );

    /// <inheritdoc />
    public abstract void GLViewport( int x, int y, int width, int height );

    /// <inheritdoc />
    public abstract void GLBindAttribLocation( int program, int index, string name );

    /// <inheritdoc />
    public abstract void GLBindBuffer( int target, uint buffer );

    /// <inheritdoc />
    public abstract void GLBindFramebuffer( int target, int framebuffer );

    /// <inheritdoc />
    public abstract void GLBindRenderbuffer( int target, int renderbuffer );

    /// <inheritdoc />
    public abstract void GLBlendColor( float red, float green, float blue, float alpha );

    /// <inheritdoc />
    public abstract void GLBlendEquation( int mode );

    /// <inheritdoc />
    public abstract void GLBlendEquationSeparate( int modeRgb, int modeAlpha );

    /// <inheritdoc />
    public abstract void GLBlendFuncSeparate( int srcRgb, int dstRgb, int srcAlpha, int dstAlpha );

    /// <inheritdoc />
    public abstract void GLBufferData( int target, int size, Buffer? data, int usage );

    /// <inheritdoc />
    public abstract void GLBufferSubData( int target, int offset, int size, Buffer data );

    /// <inheritdoc />
    public abstract void GLCompileShader( int shader );

    /// <inheritdoc />
    public abstract uint GLCreateProgram();

    /// <inheritdoc />
    public abstract int GLCreateShader( int type );

    /// <inheritdoc />
    public abstract void GLDeleteBuffers( params uint[] buffer );

    /// <inheritdoc />
    public abstract void GLDeleteProgram( uint program );

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
    public abstract uint GLGenBuffer();

    /// <inheritdoc />
    public abstract void GLGenBuffers( Buffer buffers );

    /// <inheritdoc />
    public abstract string GLGetActiveAttrib( uint program, int index, Buffer size, Buffer type );

    /// <inheritdoc />
    public abstract string GLGetActiveUniform( uint program, int index, Buffer size, Buffer type );

    /// <inheritdoc />
    public abstract void GLGetAttachedShaders( int program, int maxCount, Buffer count, IntBuffer shaders );

    /// <inheritdoc />
    public abstract int GLGetAttribLocation( uint program, string name );

    /// <inheritdoc />
    public abstract int GLGetUniformLocation( uint program, string name );

    /// <inheritdoc />
    public abstract bool GLIsBuffer( int buffer );

    /// <inheritdoc />
    public abstract bool GLIsEnabled( int cap );

    /// <inheritdoc />
    public abstract bool GLIsProgram( int program );

    /// <inheritdoc />
    public abstract bool GLIsShader( int shader );

    /// <inheritdoc />
    public abstract bool GLIsTexture( int texture );

    /// <inheritdoc />
    public abstract void GLLinkProgram( uint program );

    /// <inheritdoc />
    public abstract void GLSampleCoverage( float value, bool invert );

    /// <inheritdoc />
    public abstract void GLShaderSource( int shader, string str );

    /// <inheritdoc />
    public abstract void GLStencilFuncSeparate( int face, int func, int reference, int mask );

    /// <inheritdoc />
    public abstract void GLStencilMaskSeparate( int face, int mask );

    /// <inheritdoc />
    public abstract void GLStencilOp( int fail, int zfail, int zpass );

    /// <inheritdoc />
    public abstract void GLStencilOpSeparate( int face, int fail, int zfail, int zpass );

    /// <inheritdoc />
    public abstract void GLIntSeparate( int face, int fail, int zfail, int zpass );

    /// <inheritdoc />
    public abstract void GLUseProgram( uint program );

    /// <inheritdoc />
    public abstract void GLValidateProgram( int program );

    /// <inheritdoc />
    public abstract void GLTexParameterf( int target, int pname, float param );

    /// <inheritdoc />
    public abstract void GLGetFloatv( int pname, FloatBuffer parameters );

    /// <inheritdoc />
    public abstract void GLTexParameteri( TextureTarget target, int pname, int param );

    /// <inheritdoc />
    public abstract void GLUniform1F( int location, float x );

    /// <inheritdoc />
    public abstract void GLUniform1I( int location, int value );

    /// <inheritdoc />
    public abstract void GLUniform2F( int location, float x, float y );

    /// <inheritdoc />
    public abstract void GLUniform2I( int location, int count, int value );

    /// <inheritdoc />
    public abstract void GLUniform3F( int location, float x, float y, float z );

    /// <inheritdoc />
    public abstract void GLUniform3I( int location, int x, int y, int z );

    /// <inheritdoc />
    public abstract void GLUniform4F( int location, float x, float y, float z, float w );

    /// <inheritdoc />
    public abstract void GLUniform4I( int location, int x, int y, int z, int w );

    /// <inheritdoc />
    public abstract void GLVertexAttrib1F( int indx, float x );

    /// <inheritdoc />
    public abstract void GLVertexAttrib1Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLVertexAttrib2Fv( int location, int count, FloatBuffer values );

    /// <inheritdoc />
    public abstract void GLVertexAttrib3Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLVertexAttrib4Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr );

    /// <inheritdoc />
    public abstract void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, IntPtr ptr );

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
    public abstract void GLGetIntegerv( int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract int GLCheckFramebufferStatus( int target );

    /// <inheritdoc />
    public abstract void GLDeleteFramebuffer( int framebuffer );

    /// <inheritdoc />
    public abstract void GLDeleteFramebuffers( int n, IntBuffer framebuffers );

    /// <inheritdoc />
    public abstract void GLDeleteRenderbuffer( int renderbuffer );

    /// <inheritdoc />
    public abstract void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers );

    /// <inheritdoc />
    public abstract void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer );

    /// <inheritdoc />
    public abstract void GLFramebufferTexture2D( int target, int attachment, int textarget, uint texture, int level );

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
    public abstract void GLGetBooleanv( int pname, Buffer parameters );

    /// <inheritdoc />
    public abstract void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetProgramiv( uint program, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract string GLGetProgramInfoLog( uint program );

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
    public abstract void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer );

    /// <inheritdoc />
    public abstract bool GLIsFramebuffer( int framebuffer );

    /// <inheritdoc />
    public abstract bool GLIsRenderbuffer( int renderbuffer );

    /// <inheritdoc />
    public abstract void GLReleaseShaderCompiler();

    /// <inheritdoc />
    public abstract void GLRenderbufferStorage( int target, int internalformat, int width, int height );

    /// <inheritdoc />
    public abstract void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length );

    /// <inheritdoc />
    public abstract void GLTexParameterfv( int target, int pname, FloatBuffer parameters );

    /// <inheritdoc />
    public abstract void GLTexParameteriv( int target, int pname, IntBuffer parameters );

    /// <inheritdoc />
    public abstract void GLUniform1Fv( int location, int count, FloatBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform1Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform1Iv( int location, int count, IntBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform1Iv( int location, int count, int[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform2Fv( int location, int count, FloatBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform2Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform2Iv( int location, int count, IntBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform2Iv( int location, int count, int[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform3Fv( int location, int count, FloatBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform3Fv( int location, int count, float[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform3Iv( int location, int count, IntBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform3Iv( int location, int count, int[] v, int offset );

    /// <inheritdoc />
    public abstract void GLUniform4Fv( int location, int count, FloatBuffer v );

    /// <inheritdoc />
    public abstract void GLUniform4Fv( int location, int count, float[] v, int offset );

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
    public abstract void GLVertexAttrib2F( int indx, float x, float y );

    /// <inheritdoc />
    public abstract void GLVertexAttrib3F( int indx, float x, float y, float z );

    /// <inheritdoc />
    public abstract void GLVertexAttrib4F( int indx, float x, float y, float z, float w );

    public static string ResolveErrorNumber( ErrorCode error )
    {
        return error switch
               {
                   ErrorCode.InvalidValue                => "InvalidValue",
                   ErrorCode.InvalidOperation            => "InvalidOperation",
                   ErrorCode.InvalidFramebufferOperation => "InvalidFramebufferOperation",
                   ErrorCode.InvalidEnum                 => "InvalidEnum",
                   ErrorCode.OutOfMemory                 => "OutOfMemory",
                   ErrorCode.StackOverflow               => "StackOverflow",
                   ErrorCode.StackUnderflow              => "StackUnderflow",
                   ErrorCode.TableTooLarge               => "TableTooLarge",
                   ErrorCode.TextureTooLargeExt          => "TextureTooLarge",
                   ErrorCode.NoError                     => "NoError",
                   _                                     => throw new ArgumentOutOfRangeException( nameof( error ), error, null )
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
