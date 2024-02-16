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


using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Utils;
using LibGDXSharp.LibCore.Utils.Buffers;

using Buffer = LibGDXSharp.LibCore.Utils.Buffers.Buffer;
using GLuint = System.UInt32;

namespace LibGDXSharp.Backends.DesktopGL.glGraphics;

[PublicAPI]
public class DesktopGL20 : IGL20
{
    private ByteBuffer?  _buffer      = null;
    private FloatBuffer? _floatBuffer = null;
    private IntBuffer?   _intBuffer   = null;

    /// <inheritdoc cref="GL.glBlendColor(float,float,float,float)" />
    public void GLBlendColor( float red, float green, float blue, float alpha )
    {
        GL.glBlendColor( red, green, blue, alpha );
    }

    /// <inheritdoc cref="GL.glClearColor(float,float,float,float)" />
    public void GLClearColor( float red, float green, float blue, float alpha )
    {
        GL.glClearColor( red, green, blue, alpha );
    }

    /// <inheritdoc cref="GL.glClearDepth(double)" />
    public void GLClearDepthf( double depth )
    {
        GL.glClearDepth( depth );
    }

    /// <inheritdoc cref="GL.glClearStencil(int)" />
    public void GLClearStencil( int s )
    {
        GL.glClearStencil( s );
    }

    /// <inheritdoc cref="GL.glColorMask(bool,bool,bool,bool)" />
    public void GLColorMask( bool red, bool green, bool blue, bool alpha )
    {
        GL.glColorMask( red, green, blue, alpha );
    }

    public void GLDepthMask( bool flag )
    {
        GL.glDepthMask( flag );
    }

    public void GLDepthRangef( float zNear, float zFar )
    {
        GL.glDepthRange( zNear, zFar );
    }

    /// <inheritdoc cref="GL.glFinish()" />
    public void GLFinish()
    {
        GL.glFinish();
    }

    /// <inheritdoc cref="GL.glFlush()" />
    public void GLFlush()
    {
        GL.glFlush();
    }

    /// <inheritdoc cref="GL.glLineWidth(float)" />
    public void GLLineWidth( float width )
    {
        GL.glLineWidth( width );
    }

    /// <inheritdoc cref="GL.glPolygonOffset(float,float)" />
    public void GLPolygonOffset( float factor, float units )
    {
        GL.glPolygonOffset( factor, units );
    }

    public void GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
        GL.glRenderbufferStorage( target, internalformat, width, height );
    }

    /// <inheritdoc cref="GL.glSampleCoverage(float,bool)" />
    public void GLSampleCoverage( float value, bool invert )
    {
        GL.glSampleCoverage( value, invert );
    }

    public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="GL.glScissor(int,int,int,int)" />
    public void GLScissor( int x, int y, int width, int height )
    {
        GL.glScissor( x, y, width, height );
    }

    public unsafe void GLTexSubImage2D( int target,
                                        int level,
                                        int xoffset,
                                        int yoffset,
                                        int width,
                                        int height,
                                        int format,
                                        int type,
                                        void* pixels )
    {
        GL.glTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
    }

    /// <inheritdoc cref="GL.glViewport(int,int,int,int)" />
    public void GLViewport( int x, int y, int width, int height )
    {
        GL.glViewport( x, y, width, height );
    }

    /// <inheritdoc cref="GL.glActiveTexture(int)" />
    public void GLActiveTexture( int texture )
    {
        GL.glActiveTexture( texture );
    }

    /// <inheritdoc cref="GL.glAttachShader(uint,uint)" />
    public void GLAttachShader( GLuint program, GLuint shader )
    {
        GL.glAttachShader( program, shader );
    }

    /// <inheritdoc cref="GL.glBindTexture(int,uint)" />
    public void GLBindTexture( int target, GLuint texture )
    {
        GL.glBindTexture( target, texture );
    }

    /// <inheritdoc cref="GL.glBlendFunc(int,int)" />
    public void GLBlendFunc( int sfactor, int dfactor )
    {
        GL.glBlendFunc( sfactor, dfactor );
    }

    /// <inheritdoc cref="GL.glCreateProgram()" />
    public GLuint GLCreateProgram()
    {
        return GL.glCreateProgram();
    }

    public unsafe void GLDeleteBuffer( int buffer, GLuint* buffers )
    {
        GL.glDeleteBuffers( buffer, buffers );
    }

    /// <inheritdoc cref="GL.glDeleteBuffers(uint[])" />
    public void GLDeleteBuffers( params GLuint[] buffers )
    {
        GL.glDeleteBuffers( buffers );
    }

    public void GLDeleteFramebuffer( int framebuffer )
    {
    }

    public void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    /// <inheritdoc cref="GL.glDeleteProgram(uint)" />
    public void GLDeleteProgram( GLuint program )
    {
        GL.glDeleteProgram( program );
    }

    /// <inheritdoc cref="GL.glDeleteTextures(uint[])" />
    public void GLDeleteTextures( params GLuint[] textures )
    {
        GL.glDeleteTextures( textures );
    }

    public void GLDeleteTexture( int texture )
    {
    }

    /// <inheritdoc cref="GL.glGenBuffer()" />
    public GLuint GLGenBuffer()
    {
        return GL.glGenBuffer();
    }

    /// <inheritdoc cref="GL.glGenTexture()" />
    public GLuint GLGenTexture()
    {
        return GL.glGenTexture();
    }

    /// <inheritdoc cref="GL.glGetAttribLocation(uint,string)" />
    public int GLGetAttribLocation( GLuint program, string name )
    {
        return GL.glGetAttribLocation( program, name );
    }

    public void GLGetBooleanv( int pname, Buffer buffer )
    {
    }

    public void GLGetBufferParameteriv( int target, int pname, IntBuffer buffer )
    {
    }

    public void GLGetFloatv( int pname, FloatBuffer buffer )
    {
    }

    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer buffer )
    {
    }

    public void GLGetProgramiv( int program, int pname, IntBuffer buffer )
    {
    }

    public string GLGetProgramInfoLog( GLuint program, int bufsize )
    {
        return GL.glGetProgramInfoLog( program, bufsize );
    }

    public void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer buffer )
    {
    }


    public void GLGetShaderiv( int shader, int pname, IntBuffer buffer )
    {
    }


    public string GLGetShaderInfoLog( int shader )
    {
        return null;
    }


    public void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
    {
    }


    public void GLGetTexParameterfv( int target, int pname, FloatBuffer buffer )
    {
    }


    public void GLGetTexParameteriv( int target, int pname, IntBuffer buffer )
    {
    }


    public void GLGetUniformfv( int program, int location, FloatBuffer buffer )
    {
    }


    public void GLGetUniformiv( int program, int location, IntBuffer buffer )
    {
    }

    /// <inheritdoc cref="GL.glGetError()" />
    public int GLGetError()
    {
        return GL.glGetError();
    }


    public void GLGetIntegerv( int pname, IntBuffer buffer )
    {
    }

    /// <inheritdoc cref="GL.glGetUniformLocation(uint,string)" />
    public int GLGetUniformLocation( GLuint program, string name )
    {
        return GL.glGetUniformLocation( program, name );
    }


    public void GLGetVertexAttribfv( int index, int pname, FloatBuffer buffer )
    {
    }


    public void GLGetVertexAttribiv( int index, int pname, IntBuffer buffer )
    {
    }


    public void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
    {
    }

    /// <inheritdoc cref="GL.glLinkProgram(uint)" />
    public void GLLinkProgram( GLuint program )
    {
        GL.glLinkProgram( program );
    }


    public void GLReleaseShaderCompiler()
    {
    }


    public void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    /// <inheritdoc cref="GL.glUseProgram(uint)" />
    public void GLUseProgram( GLuint program )
    {
        GL.glUseProgram( program );
    }

    /// <inheritdoc cref="GL.glBindAttribLocation(uint,uint,string)" />
    public void GLBindAttribLocation( GLuint program, GLuint index, string name )
    {
        GL.glBindAttribLocation( program, index, name );
    }

    /// <inheritdoc cref="GL.glBindBuffer(int,uint)" />
    public void GLBindBuffer( int target, GLuint buffer )
    {
        GL.glBindBuffer( target, buffer );
    }

    /// <inheritdoc cref="GL.glBindFramebuffer(int,uint)" />
    public void GLBindFramebuffer( int target, GLuint framebuffer )
    {
        GL.glBindFramebuffer( target, framebuffer );
    }

    /// <inheritdoc cref="GL.glBindRenderbuffer(int,uint)" />
    public void GLBindRenderbuffer( int target, GLuint renderbuffer )
    {
        GL.glBindRenderbuffer( target, renderbuffer );
    }

    /// <inheritdoc cref="GL.glBlendEquation(int)" />
    public void GLBlendEquation( int mode )
    {
        GL.glBlendEquation( mode );
    }

    /// <inheritdoc cref="GL.glBlendFuncSeparate(int,int,int,int)" />
    public void GLBlendEquationSeparate( int modeRGB, int modeAlpha )
    {
        GL.glBlendEquationSeparate( modeRGB, modeAlpha );
    }

    /// <inheritdoc cref="GL.glBlendFuncSeparate(int,int,int,int)" />
    public void GLBlendFuncSeparate( int srcRGB,
                                     int dstRGB,
                                     int srcAlpha,
                                     int dstAlpha )
    {
        GL.glBlendFuncSeparate( srcRGB, dstRGB, srcAlpha, dstAlpha );
    }

    public unsafe void GLBufferData( int target, int size, void* data, int usage )
    {
        GL.glBufferData( target, size, data, usage );
    }

    public unsafe void GLBufferSubData( int target, int offset, int size, void* data )
    {
        GL.glBufferSubData( target, offset, size, data );
    }

    public int GLCheckFramebufferStatus( int target )
    {
        throw new NotImplementedException();
    }

    //        return glCheckFramebufferStatus( target );
    /// <inheritdoc cref="GL.glClear(uint)" />
    public void GLClear( GLuint mask )
    {
        GL.glClear( mask );
    }

    /// <inheritdoc cref="GL.glCompileShader(uint)" />
    public void GLCompileShader( GLuint shader )
    {
        GL.glCompileShader( shader );
    }

    public unsafe void GLCompressedTexImage2D( int target,
                                               int level,
                                               int internalFormat,
                                               int width,
                                               int height,
                                               int border,
                                               int imageSize,
                                               void* data )
    {
        GL.glCompressedTexImage2D( target, level, internalFormat, width, height, border, imageSize, data );
    }

    public unsafe void GLCompressedTexSubImage2D( int target,
                                                  int level,
                                                  int xoffset,
                                                  int yoffset,
                                                  int width,
                                                  int height,
                                                  int format,
                                                  int imageSize,
                                                  void* data )
    {
        GL.glCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
    }

    /// <inheritdoc cref="GL.glCopyTexImage2D(int,int,int,int,int,int,int,int)" />
    public void GLCopyTexImage2D( int target,
                                  int level,
                                  int internalFormat,
                                  int x,
                                  int y,
                                  int width,
                                  int height,
                                  int border )
    {
        GL.glCopyTexImage2D( target, level, internalFormat, x, y, width, height, border );
    }

    /// <inheritdoc cref="GL.glCopyTexSubImage2D(int,int,int,int,int,int,int,int)" />
    public void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        GL.glCopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
    }

    /// <inheritdoc cref="GL.glCreateShader(int)" />
    public GLuint GLCreateShader( int type )
    {
        return GL.glCreateShader( type );
    }

    /// <inheritdoc cref="GL.glCullFace(int)" />
    public void GLCullFace( int mode )
    {
        GL.glCullFace( mode );
    }

    public unsafe void GLDeleteRenderbuffers( int n, GLuint* renderbuffers )
    {
        GL.glDeleteRenderbuffers( n, renderbuffers );
    }

    public void GLDeleteRenderbuffer( GLuint renderbuffer )
    {
        GL.glDeleteRenderbuffers( renderbuffer );
    }

    /// <inheritdoc cref="GL.glDeleteShader(uint)" />
    public void GLDeleteShader( GLuint shader )
    {
        GL.glDeleteShader( shader );
    }

    /// <inheritdoc cref="GL.glDepthFunc(int)" />
    public void GLDepthFunc( int func )
    {
        GL.glDepthFunc( func );
    }

    public void GLDetachShader( GLuint program, GLuint shader )
    {
        GL.glDetachShader( program, shader );
    }

    public void GLDisable( int cap )
    {
        GL.glDisable( cap );
    }

    public void GLDisableVertexAttribArray( GLuint index )
    {
        GL.glDisableVertexAttribArray( index );
    }

    public void GLDrawArrays( int mode, int first, int count )
    {
        GL.glDrawArrays( mode, first, count );
    }


    public void GLDrawElements( int mode, int count, int type, Buffer indices )
    {
    }

    public unsafe void GLDrawElements( int mode, int count, int type, void* indices )
    {
        GL.glDrawElements( mode, count, type, indices );
    }

    /// <inheritdoc cref="GL.glEnable(int)" />
    public void GLEnable( int cap )
    {
        GL.glEnable( cap );
    }

    /// <inheritdoc cref="GL.glEnableVertexAttribArray(uint)" />
    public void GLEnableVertexAttribArray( GLuint index )
    {
        GL.glEnableVertexAttribArray( index );
    }

    public void GLFramebufferRenderbuffer( int target, int attachment, int renderBufferTarget, GLuint renderbuffer )
    {
        GL.glFramebufferRenderbuffer( target, attachment, renderBufferTarget, renderbuffer );
    }

    public void GLFramebufferTexture2D( int target, int attachment, int textarget, GLuint texture, int level )
    {
        GL.glFramebufferTexture2D( target, attachment, textarget, texture, level );
    }

    /// <inheritdoc cref="GL.glFrontFace(int)" />
    public void GLFrontFace( int mode )
    {
        GL.glFrontFace( mode );
    }

    /// <inheritdoc cref="GL.glGenBuffers(int)" />
    public GLuint[] GLGenBuffers( int n )
    {
        return GL.glGenBuffers( n );
    }


    public void GLGenerateMipmap( int target )
    {
        GL.glGenerateMipmap( target );
    }


    public GLuint GLGenFramebuffer()
    {
        return GL.glGenFramebuffer();
    }


    public unsafe void GLGenFramebuffers( int n, GLuint* framebuffers )
    {
        GL.glGenFramebuffers( n, framebuffers );
    }


    public int GLGenRenderbuffer()
    {
        return 0;
    }


    public void GLGenRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    /// <inheritdoc cref="GL.glGenTextures(int)" />
    public GLuint[] GLGenTextures( int n )
    {
        return GL.glGenTextures( n );
    }

    /// <inheritdoc cref="GL.glGetActiveAttrib(uint,uint,int,int*,int*,int*,byte*)" />
    public unsafe void GLGetActiveAttrib( GLuint program,
                                          GLuint index,
                                          int bufSize,
                                          int* length,
                                          int* size,
                                          int* type,
                                          byte* name )
    {
        GL.glGetActiveAttrib( program, index, bufSize, length, size, type, name );
    }

    /// <inheritdoc cref="GL.glGetActiveUniform(uint,uint,int,int*,int*,int*,byte*)" />
    public unsafe void GLGetActiveUniform( GLuint program,
                                           GLuint index,
                                           int bufSize,
                                           int* length,
                                           int* size,
                                           int* type,
                                           byte* name )
    {
        GL.glGetActiveUniform( program, index, bufSize, length, size, type, name );
    }

    /// <inheritdoc cref="GL.glGetAttachedShaders(uint,int,int*,uint*)" />
    public unsafe void GLGetAttachedShaders( GLuint program, int maxCount, int* count, GLuint* shaders )
    {
        GL.glGetAttachedShaders( program, maxCount, count, shaders );
    }

    /// <inheritdoc cref="GL.glGetString(int)" />
    public unsafe string GLGetString( int name )
    {
        return GL.glGetString( name ) -> ToString();
    }

    /// <inheritdoc cref="GL.glHint(int,int)" />
    public void GLHint( int target, int mode )
    {
        GL.glHint( target, mode );
    }

    /// <inheritdoc cref="GL.glIsBuffer(uint)" />
    public bool GLIsBuffer( GLuint buffer )
    {
        return GL.glIsBuffer( buffer );
    }

    /// <inheritdoc cref="GL.glIsEnabled(int)" />
    public bool GLIsEnabled( int cap )
    {
        return GL.glIsEnabled( cap );
    }


    public bool GLIsFramebuffer( int framebuffer )
    {
        return false;
    }

    /// <inheritdoc cref="GL.glIsProgram(uint)" />
    public bool GLIsProgram( GLuint program )
    {
        return GL.glIsProgram( program );
    }


    public bool GLIsRenderbuffer( int renderbuffer )
    {
        return false;
    }

    /// <inheritdoc cref="GL.glIsShader(uint)" />
    public bool GLIsShader( GLuint shader )
    {
        return GL.glIsShader( shader );
    }

    /// <inheritdoc cref="GL.glIsTexture(uint)" />
    public bool GLIsTexture( GLuint texture )
    {
        return GL.glIsTexture( texture );
    }

    public void GLPixelStorei( int pname, int param )
    {
        GL.glPixelStorei( pname, param );
    }

    public unsafe void GLReadPixels( int x, int y, int width, int height, int format, int type, void* pixels )
    {
        GL.glReadPixels( x, y, width, height, format, type, pixels );
    }

    /// <inheritdoc cref="GL.glShaderSource(uint,string[])" />
    public void GLShaderSource( GLuint shader, params string[] str )
    {
        GL.glShaderSource( shader, str );
    }

    /// <inheritdoc cref="GL.glStencilFunc(int,int,uint)" />
    public void GLStencilFunc( int func, int reference, GLuint mask )
    {
        GL.glStencilFunc( func, reference, mask );
    }

    /// <inheritdoc cref="GL.glStencilFuncSeparate(int,int,int,uint)" />
    public void GLStencilFuncSeparate( int face, int func, int reference, GLuint mask )
    {
        GL.glStencilFuncSeparate( face, func, reference, mask );
    }

    /// <inheritdoc cref="GL.glStencilMask(uint)" />
    public void GLStencilMask( GLuint mask )
    {
        GL.glStencilMask( mask );
    }


    public void GLStencilOp( int fail, int zfail, int zpass )
    {
    }

    /// <inheritdoc cref="GL.glStencilMaskSeparate(int,uint)" />
    public void GLStencilMaskSeparate( int face, GLuint mask )
    {
        GL.glStencilMaskSeparate( face, mask );
    }


    public void GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
    }


    public void GLTexParameterfv( int target, int pname, FloatBuffer buffer )
    {
    }

    public unsafe void GLTexImage2D( int target,
                                     int level,
                                     int internalFormat,
                                     int width,
                                     int height,
                                     int border,
                                     int format,
                                     int type,
                                     void* pixels )
    {
        GL.glTexImage2D( target, level, internalFormat, width, height, border, format, type, pixels );
    }

    /// <inheritdoc cref="GL.glTexParameterf(int,int,float)" />
    public void GLTexParameterf( int target, int pname, float param )
    {
        GL.glTexParameterf( target, pname, param );
    }

    /// <inheritdoc cref="GL.glTexParameteri(int,int,int)" />
    public void GLTexParameteri( int target, int pname, int param )
    {
        GL.glTexParameteri( target, pname, param );
    }

    public void GLTexParameteriv( int target, int pname, IntBuffer buffer )
    {
    }

    /// <inheritdoc cref="GL.glUniform1f(int,float)" />
    public void GLUniform1F( int location, float value )
    {
        GL.glUniform1f( location, value );
    }

    public void GLUniform1Fv( int location, int count, FloatBuffer v )
    {
    }

    public void GLUniform1Fv( int location, int count, float[] v, int offset )
    {
    }

    /// <inheritdoc cref="GL.glUniform1i(int,int)" />
    public void GLUniform1I( int location, int value )
    {
        GL.glUniform1i( location, value );
    }

    public void GLUniform1Iv( int location, int count, IntBuffer v )
    {
    }

    public void GLUniform1Iv( int location, int count, int[] v, int offset )
    {
    }

    /// <inheritdoc cref="GL.glUniform2f(int,float,float)" />
    public void GLUniform2F( int location, float v0, float v1 )
    {
        GL.glUniform2f( location, v0, v1 );
    }

    public void GLUniform2Fv( int location, int count, FloatBuffer v )
    {
    }

    public void GLUniform2Fv( int location, int count, float[] v, int offset )
    {
    }

    /// <inheritdoc cref="GL.glUniform2i(int,int,int)" />
    public void GLUniform2I( int location, int v0, int v1 )
    {
        GL.glUniform2i( location, v0, v1 );
    }

    public void GLUniform2Iv( int location, int count, IntBuffer v )
    {
    }

    public void GLUniform2Iv( int location, int count, int[] v, int offset )
    {
    }

    /// <inheritdoc cref="GL.glUniform3f(int,float,float,float)" />
    public void GLUniform3F( int location, float v0, float v1, float v2 )
    {
        GL.glUniform3f( location, v0, v1, v2 );
    }

    public void GLUniform3Fv( int location, int count, FloatBuffer v )
    {
    }

    public void GLUniform3Fv( int location, int count, float[] v, int offset )
    {
    }

    /// <inheritdoc cref="GL.glUniform3i(int,int,int,int)" />
    public void GLUniform3I( int location, int v0, int v1, int v2 )
    {
        GL.glUniform3i( location, v0, v1, v2 );
    }

    public void GLUniform3Iv( int location, int count, IntBuffer v )
    {
    }

    public void GLUniform3Iv( int location, int count, int[] v, int offset )
    {
    }

    public void GLUniform4F( int location, float v0, float v1, float v2, float v3 )
    {
        GL.glUniform4f( location, v0, v1, v2, v3 );
    }

    public void GLUniform4Fv( int location, int count, FloatBuffer v )
    {
    }

    public void GLUniform4Fv( int location, int count, float[] v, int offset )
    {
    }

    public void GLUniform4I( int location, int v0, int v1, int v2, int v3 )
    {
        GL.glUniform4i( location, v0, v1, v2, v3 );
    }

    public void GLUniform4Iv( int location, int count, IntBuffer v )
    {
    }

    public void GLUniform4Iv( int location, int count, int[] v, int offset )
    {
    }

    public void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    public void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    public unsafe void GLUniformMatrix4Fv( int location, int count, bool transpose, float* value )
    {
        GL.glUniformMatrix4fv( location, count, transpose, value );
    }

    public void GLValidateProgram( GLuint program )
    {
        GL.glValidateProgram( program );
    }

    public void GLVertexAttrib1F( GLuint indx, float x )
    {
        GL.glVertexAttrib1f( indx, x );
    }

    public void GLVertexAttrib1Fv( GLuint indx, float x )
    {
        GL.glVertexAttrib1f( indx, x );
    }

    public void GLVertexAttrib2F( GLuint indx, float x, float y )
    {
        GL.glVertexAttrib2f( indx, x, y );
    }

    public void GLVertexAttrib2Fv( GLuint indx, float v0, float v1 )
    {
        GL.glVertexAttrib2f( indx, v0, v1 );
    }

    public void GLVertexAttrib3F( GLuint indx, float x, float y, float z )
    {
        GL.glVertexAttrib3f( indx, x, y, z );
    }

    public void GLVertexAttrib3Fv( GLuint indx, float v0, float v1, float v2 )
    {
        GL.glVertexAttrib3f( indx, v0, v1, v2 );
    }

    public void GLVertexAttrib4F( GLuint indx, float x, float y, float z, float w )
    {
        GL.glVertexAttrib4f( indx, x, y, z, w );
    }

    public void GLVertexAttrib4Fv( GLuint indx, float v0, float v1, float v2, float v3 )
    {
        GL.glVertexAttrib4f( indx, v0, v1, v2, v3 );
    }

    public void GLVertexAttribPointer( GLuint indx, int size, int type, bool normalized, int stride, GLuint ptr )
    {
        GL.glVertexAttribPointer( indx, size, type, normalized, stride, ptr );
    }

    public unsafe void GLVertexAttribPointer( GLuint indx,
                                              int size,
                                              int type,
                                              bool normalized,
                                              int stride,
                                              void* ptr )
    {
        GL.glVertexAttribPointer( indx, size, type, normalized, stride, ptr );
    }

    private void EnsureBufferCapacity( int numBytes )
    {
        if ( ( _buffer == null ) || ( _buffer.Capacity < numBytes ) )
        {
            _buffer      = BufferUtils.NewByteBuffer( numBytes );
            _floatBuffer = _buffer.AsFloatBuffer();
            _intBuffer   = _buffer.AsIntBuffer();
        }
    }

    private FloatBuffer ToFloatBuffer( float[] v, int offset, int count )
    {
        GdxRuntimeException.ThrowIfNull( _floatBuffer );

        EnsureBufferCapacity( count << 2 );

        _floatBuffer.Clear();
        _floatBuffer.Limit = count;
        _floatBuffer.Put( v, offset, count );
        _floatBuffer.Position = 0;

        return _floatBuffer;
    }

    private IntBuffer ToIntBuffer( int[] v, int offset, int count )
    {
        GdxRuntimeException.ThrowIfNull( _intBuffer );

        EnsureBufferCapacity( count << 2 );

        _intBuffer.Clear();
        _intBuffer.Limit = count;
        _intBuffer.Put( v, offset, count );
        _intBuffer.Position = 0;

        return _intBuffer;
    }
}
