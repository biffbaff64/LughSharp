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

using Buffer = LibGDXSharp.LibCore.Utils.Buffers.Buffer;

namespace LibGDXSharp.Backends.DesktopGL.Graphics;

[PublicAPI]
public unsafe class DesktopGL20 : IGL20
{
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

    /// <inheritdoc cref="GL.glDepthMask(bool)" />
    public void GLDepthMask( bool flag )
    {
        GL.glDepthMask( flag );
    }

    /// <inheritdoc cref="GL.glDepthRange(double,double)" />
    public void GLDepthRangef( double zNear, double zFar )
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

    /// <inheritdoc cref="GL.glRenderbufferStorage(int,int,int,int)" />
    public void GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
        GL.glRenderbufferStorage( target, internalformat, width, height );
    }

    /// <inheritdoc cref="GL.glSampleCoverage(float,bool)" />
    public void GLSampleCoverage( float value, bool invert )
    {
        GL.glSampleCoverage( value, invert );
    }

    /// <inheritdoc cref="GL.glScissor(int,int,int,int)" />
    public void GLScissor( int x, int y, int width, int height )
    {
        GL.glScissor( x, y, width, height );
    }

    public void GLTexSubImage2D( int target,
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
    public void GLAttachShader( uint program, uint shader )
    {
        GL.glAttachShader( program, shader );
    }

    /// <inheritdoc cref="GL.glBindTexture(int,uint)" />
    public void GLBindTexture( int target, uint texture )
    {
        GL.glBindTexture( target, texture );
    }

    /// <inheritdoc cref="GL.glBlendFunc(int,int)" />
    public void GLBlendFunc( int sfactor, int dfactor )
    {
        GL.glBlendFunc( sfactor, dfactor );
    }

    /// <inheritdoc cref="GL.glCreateProgram()" />
    public uint GLCreateProgram()
    {
        return GL.glCreateProgram();
    }

    public void GLDeleteBuffer( int buffer, uint* buffers )
    {
        GL.glDeleteBuffers( buffer, buffers );
    }

    /// <inheritdoc cref="GL.glDeleteBuffers(uint[])" />
    public void GLDeleteBuffers( params uint[] buffers )
    {
        GL.glDeleteBuffers( buffers );
    }

    public void GLDeleteFramebuffers( int n, uint* framebuffers )
    {
        GL.glDeleteFramebuffers( n, framebuffers );
    }

    /// <inheritdoc cref="GL.glDeleteProgram(uint)" />
    public void GLDeleteProgram( uint program )
    {
        GL.glDeleteProgram( program );
    }

    /// <inheritdoc cref="GL.glDeleteTextures(uint[])" />
    public void GLDeleteTextures( params uint[] textures )
    {
        GL.glDeleteTextures( textures );
    }

    /// <inheritdoc cref="GL.glGenBuffer()" />
    public uint GLGenBuffer()
    {
        return GL.glGenBuffer();
    }

    /// <inheritdoc cref="GL.glGenTexture()" />
    public uint GLGenTexture()
    {
        return GL.glGenTexture();
    }

    /// <inheritdoc cref="GL.glGetAttribLocation(uint,string)" />
    public int GLGetAttribLocation( uint program, string name )
    {
        return GL.glGetAttribLocation( program, name );
    }

    public void GLGetBooleanv( int pname, bool* buffer )
    {
        GL.glGetBooleanv( pname, buffer );
    }

    public void GLGetBufferParameteriv( int target, int pname, int* buffer )
    {
        GL.glGetBufferParameteriv( target, pname, buffer );
    }

    public void GLGetFloatv( int pname, float* buffer )
    {
        GL.glGetFloatv( pname, buffer );
    }

    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, int* buffer )
    {
        GL.glGetFramebufferAttachmentParameteriv( target, attachment, pname, buffer );
    }

    public void GLGetProgramiv( uint program, int pname, int* buffer )
    {
        GL.glGetProgramiv( program, pname, buffer );
    }

    public string GLGetProgramInfoLog( uint program, int bufsize )
    {
        return GL.glGetProgramInfoLog( program, bufsize );
    }

    public void GLGetRenderbufferParameteriv( int target, int pname, int* buffer )
    {
        GL.glGetRenderbufferParameteriv( target, pname, buffer );
    }

    public void GLGetShaderiv( uint shader, int pname, int* buffer )
    {
        GL.glGetShaderiv( shader, pname, buffer );
    }

    public string GLGetShaderInfoLog( uint shader, int size )
    {
        return GL.glGetShaderInfoLog( shader, size );
    }

    public void GLGetTexParameterfv( int target, int pname, float* buffer )
    {
        GL.glGetTexParameterfv( target, pname, buffer );
    }

    public void GLGetTexParameteriv( int target, int pname, int* buffer )
    {
        GL.glGetTexParameteriv( target, pname, buffer );
    }

    public void GLGetUniformfv( uint program, int location, float* buffer )
    {
        GL.glGetUniformfv( program, location, buffer );
    }

    public void GLGetUniformiv( uint program, int location, int* buffer )
    {
        GL.glGetUniformiv( program, location, buffer );
    }

    /// <inheritdoc cref="GL.glGetError()" />
    public int GLGetError()
    {
        return GL.glGetError();
    }

    public void GLGetIntegerv( int pname, int* buffer )
    {
        GL.glGetIntegerv( pname, buffer );
    }

    /// <inheritdoc cref="GL.glGetUniformLocation(uint,string)" />
    public int GLGetUniformLocation( uint program, string name )
    {
        return GL.glGetUniformLocation( program, name );
    }

    public void GLGetVertexAttribfv( uint index, int pname, float* buffer )
    {
        GL.glGetVertexAttribfv( index, pname, buffer );
    }

    public void GLGetVertexAttribiv( uint index, int pname, int* buffer )
    {
        GL.glGetVertexAttribiv( index, pname, buffer );
    }

    public void GLGetVertexAttribPointerv( uint index, int pname, void** pointer )
    {
        GL.glGetVertexAttribPointerv( index, pname, pointer );
    }

    /// <inheritdoc cref="GL.glLinkProgram(uint)" />
    public void GLLinkProgram( uint program )
    {
        GL.glLinkProgram( program );
    }

    public void GLUniformMatrix4Fv( int location, bool b, float transpose, float value, int offset )
    {
        GL.glUniformMatrix4fv( location, b, transpose, value, offset );
    }

    /// <inheritdoc cref="GL.glUseProgram(uint)" />
    public void GLUseProgram( uint program )
    {
        GL.glUseProgram( program );
    }

    /// <inheritdoc cref="GL.glBindAttribLocation(uint,uint,string)" />
    public void GLBindAttribLocation( uint program, uint index, string name )
    {
        GL.glBindAttribLocation( program, index, name );
    }

    /// <inheritdoc cref="GL.glBindBuffer(int,uint)" />
    public void GLBindBuffer( int target, uint buffer )
    {
        GL.glBindBuffer( target, buffer );
    }

    /// <inheritdoc cref="GL.glBindFramebuffer(int,uint)" />
    public void GLBindFramebuffer( int target, uint framebuffer )
    {
        GL.glBindFramebuffer( target, framebuffer );
    }

    /// <inheritdoc cref="GL.glBindRenderbuffer(int,uint)" />
    public void GLBindRenderbuffer( int target, uint renderbuffer )
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

    public void GLBufferData( int target, int size, void* data, int usage )
    {
        GL.glBufferData( target, size, data, usage );
    }

    public void GLBufferSubData( int target, int offset, int size, void* data )
    {
        GL.glBufferSubData( target, offset, size, data );
    }

    /// <inheritdoc cref="GL.glClear(uint)" />
    public void GLClear( uint mask )
    {
        GL.glClear( mask );
    }

    /// <inheritdoc cref="GL.glCompileShader(uint)" />
    public void GLCompileShader( uint shader )
    {
        GL.glCompileShader( shader );
    }

    public void GLCompressedTexImage2D( int target,
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

    public void GLCompressedTexSubImage2D( int target,
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
    public uint GLCreateShader( int type )
    {
        return GL.glCreateShader( type );
    }

    /// <inheritdoc cref="GL.glCullFace(int)" />
    public void GLCullFace( int mode )
    {
        GL.glCullFace( mode );
    }

    public void GLDeleteRenderbuffers( int n, uint* renderbuffers )
    {
        GL.glDeleteRenderbuffers( n, renderbuffers );
    }

    public void GLDeleteRenderbuffer( uint renderbuffer )
    {
        GL.glDeleteRenderbuffers( renderbuffer );
    }

    /// <inheritdoc cref="GL.glDeleteShader(uint)" />
    public void GLDeleteShader( uint shader )
    {
        GL.glDeleteShader( shader );
    }

    /// <inheritdoc cref="GL.glDepthFunc(int)" />
    public void GLDepthFunc( int func )
    {
        GL.glDepthFunc( func );
    }

    public void GLDetachShader( uint program, uint shader )
    {
        GL.glDetachShader( program, shader );
    }

    public void GLDisable( int cap )
    {
        GL.glDisable( cap );
    }

    public void GLDisableVertexAttribArray( uint index )
    {
        GL.glDisableVertexAttribArray( index );
    }

    public void GLDrawArrays( int mode, int first, int count )
    {
        GL.glDrawArrays( mode, first, count );
    }

    public void GLDrawElements( int mode, int count, int type, void* indices )
    {
        GL.glDrawElements( mode, count, type, indices );
    }

    /// <inheritdoc cref="GL.glEnable(int)" />
    public void GLEnable( int cap )
    {
        GL.glEnable( cap );
    }

    /// <inheritdoc cref="GL.glEnableVertexAttribArray(uint)" />
    public void GLEnableVertexAttribArray( uint index )
    {
        GL.glEnableVertexAttribArray( index );
    }

    public void GLFramebufferRenderbuffer( int target, int attachment, int renderBufferTarget, uint renderbuffer )
    {
        GL.glFramebufferRenderbuffer( target, attachment, renderBufferTarget, renderbuffer );
    }

    public void GLFramebufferTexture2D( int target, int attachment, int textarget, uint texture, int level )
    {
        GL.glFramebufferTexture2D( target, attachment, textarget, texture, level );
    }

    /// <inheritdoc cref="GL.glFrontFace(int)" />
    public void GLFrontFace( int mode )
    {
        GL.glFrontFace( mode );
    }

    /// <inheritdoc cref="GL.glGenBuffers(int)" />
    public uint[] GLGenBuffers( int n )
    {
        return GL.glGenBuffers( n );
    }

    public void GLGenerateMipmap( int target )
    {
        GL.glGenerateMipmap( target );
    }

    public uint GLGenFramebuffer()
    {
        return GL.glGenFramebuffer();
    }

    public void GLGenFramebuffers( int n, uint* framebuffers )
    {
        GL.glGenFramebuffers( n, framebuffers );
    }

    public uint GLGenRenderbuffer()
    {
        return GL.glGenRenderbuffer();
    }

    public void GLGenRenderbuffers( int n, uint* renderbuffers )
    {
        GL.glGenRenderbuffers( n, renderbuffers );
    }

    /// <inheritdoc cref="GL.glGenTextures(int)" />
    public uint[] GLGenTextures( int n )
    {
        return GL.glGenTextures( n );
    }

    /// <inheritdoc cref="GL.glGetActiveAttrib(uint,uint,int,int*,int*,int*,byte*)" />
    public void GLGetActiveAttrib( uint program,
                                   uint index,
                                   int bufSize,
                                   int* length,
                                   int* size,
                                   int* type,
                                   byte* name )
    {
        GL.glGetActiveAttrib( program, index, bufSize, length, size, type, name );
    }

    /// <inheritdoc cref="GL.glGetActiveUniform(uint,uint,int,int*,int*,int*,byte*)" />
    public void GLGetActiveUniform( uint program,
                                    uint index,
                                    int bufSize,
                                    int* length,
                                    int* size,
                                    int* type,
                                    byte* name )
    {
        GL.glGetActiveUniform( program, index, bufSize, length, size, type, name );
    }

    /// <inheritdoc cref="GL.glGetAttachedShaders(uint,int,int*,uint*)" />
    public void GLGetAttachedShaders( uint program, int maxCount, int* count, uint* shaders )
    {
        GL.glGetAttachedShaders( program, maxCount, count, shaders );
    }

    /// <inheritdoc cref="GL.glGetString(int)" />
    public string GLGetString( int name )
    {
        return GL.glGetString( name ) -> ToString();
    }

    /// <inheritdoc cref="GL.glHint(int,int)" />
    public void GLHint( int target, int mode )
    {
        GL.glHint( target, mode );
    }

    /// <inheritdoc cref="GL.glIsBuffer(uint)" />
    public bool GLIsBuffer( uint buffer )
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
    public bool GLIsProgram( uint program )
    {
        return GL.glIsProgram( program );
    }

    public bool GLIsRenderbuffer( int renderbuffer )
    {
        return false;
    }

    /// <inheritdoc cref="GL.glIsShader(uint)" />
    public bool GLIsShader( uint shader )
    {
        return GL.glIsShader( shader );
    }

    /// <inheritdoc cref="GL.glIsTexture(uint)" />
    public bool GLIsTexture( uint texture )
    {
        return GL.glIsTexture( texture );
    }

    public void GLPixelStorei( int pname, int param )
    {
        GL.glPixelStorei( pname, param );
    }

    public void GLReadPixels( int x, int y, int width, int height, int format, int type, void* pixels )
    {
        GL.glReadPixels( x, y, width, height, format, type, pixels );
    }

    /// <inheritdoc cref="GL.glShaderSource(uint,string[])" />
    public void GLShaderSource( uint shader, params string[] str )
    {
        GL.glShaderSource( shader, str );
    }

    /// <inheritdoc cref="GL.glStencilFunc(int,int,uint)" />
    public void GLStencilFunc( int func, int reference, uint mask )
    {
        GL.glStencilFunc( func, reference, mask );
    }

    /// <inheritdoc cref="GL.glStencilFuncSeparate(int,int,int,uint)" />
    public void GLStencilFuncSeparate( int face, int func, int reference, uint mask )
    {
        GL.glStencilFuncSeparate( face, func, reference, mask );
    }

    /// <inheritdoc cref="GL.glStencilMask(uint)" />
    public void GLStencilMask( uint mask )
    {
        GL.glStencilMask( mask );
    }

    public void GLStencilOp( int fail, int zfail, int zpass )
    {
        GL.glStencilOp( fail, zfail, zpass );
    }

    /// <inheritdoc cref="GL.glStencilMaskSeparate(int,uint)" />
    public void GLStencilMaskSeparate( int face, uint mask )
    {
        GL.glStencilMaskSeparate( face, mask );
    }

    public void GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
        GL.glStencilOpSeparate( face, fail, zfail, zpass );
    }

    public void GLTexParameterfv( int target, int pname, float* buffer )
    {
        GL.glTexParameterfv( target, pname, buffer );
    }

    public void GLTexImage2D( int target,
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

    public void GLTexParameteriv( int target, int pname, int* buffer )
    {
        GL.glTexParameteriv( target, pname, buffer );
    }

    /// <inheritdoc cref="GL.glUniform1f(int,float)" />
    public void GLUniform1F( int location, float value )
    {
        GL.glUniform1f( location, value );
    }

    public void GLUniform1Fv( int location, int count, float* v )
    {
        GL.glUniform1fv( location, count, v );
    }

    public void GLUniform1Fv( int location, int count, float v, int offset )
    {
        GL.glUniform1fv( location, count, v, offset );
    }

    /// <inheritdoc cref="GL.glUniform1i(int,int)" />
    public void GLUniform1I( int location, int value )
    {
        GL.glUniform1i( location, value );
    }

    public void GLUniform1Iv( int location, int count, int* v )
    {
        GL.glUniform1iv( location, count, v );
    }

    public void GLUniform1Iv( int location, int count, int v, int offset )
    {
        GL.glUniform1iv( location, count, v, offset );
    }

    /// <inheritdoc cref="GL.glUniform2f(int,float,float)" />
    public void GLUniform2F( int location, float v0, float v1 )
    {
        GL.glUniform2f( location, v0, v1 );
    }

    public void GLUniform2Fv( int location, int count, float* v )
    {
        GL.glUniform2fv( location, count, v );
    }

    public void GLUniform2Fv( int location, int count, float v, int offset )
    {
        GL.glUniform2fv( location, count, v, offset );
    }

    /// <inheritdoc cref="GL.glUniform2i(int,int,int)" />
    public void GLUniform2I( int location, int v0, int v1 )
    {
        GL.glUniform2i( location, v0, v1 );
    }

    public void GLUniform2Iv( int location, int count, int* v )
    {
        GL.glUniform2iv( location, count, v );
    }

    public void GLUniform2Iv( int location, int count, int v, int offset )
    {
        GL.glUniform2iv( location, count, v, offset );
    }

    /// <inheritdoc cref="GL.glUniform3f(int,float,float,float)" />
    public void GLUniform3F( int location, float v0, float v1, float v2 )
    {
        GL.glUniform3f( location, v0, v1, v2 );
    }

    public void GLUniform3Fv( int location, int count, float* v )
    {
        GL.glUniform3fv( location, count, v );
    }

    public void GLUniform3Fv( int location, int count, float v, int offset )
    {
        GL.glUniform3fv( location, count, v, offset );
    }

    /// <inheritdoc cref="GL.glUniform3i(int,int,int,int)" />
    public void GLUniform3I( int location, int v0, int v1, int v2 )
    {
        GL.glUniform3i( location, v0, v1, v2 );
    }

    public void GLUniform3Iv( int location, int count, int* v )
    {
        GL.glUniform3iv( location, count, v );
    }

    public void GLUniform3Iv( int location, int count, int v, int offset )
    {
        GL.glUniform3iv( location, count, v, offset );
    }

    public void GLUniform4F( int location, float v0, float v1, float v2, float v3 )
    {
        GL.glUniform4f( location, v0, v1, v2, v3 );
    }

    public void GLUniform4Fv( int location, int count, float* v )
    {
        GL.glUniform4fv( location, count, v );
    }

    public void GLUniform4Fv( int location, int count, float v, int offset )
    {
        GL.glUniform4fv( location, count, v, offset );
    }

    public void GLUniform4I( int location, int v0, int v1, int v2, int v3 )
    {
        GL.glUniform4i( location, v0, v1, v2, v3 );
    }

    public void GLUniform4Iv( int location, int count, int* v )
    {
        GL.glUniform4iv( location, count, v );
    }

    public void GLUniform4Iv( int location, int count, int v, int offset )
    {
        GL.glUniform4iv( location, count, v, offset );
    }

    public void GLUniformMatrix2Fv( int location, int count, bool transpose, float* value )
    {
        GL.glUniformMatrix2fv( location, count, transpose, value );
    }

    /// <inheritdoc cref="GL.glUniformMatrix2fv(int,bool,float[])" />
    public void GLUniformMatrix2Fv( int location, bool transpose, float[] value )
    {
        GL.glUniformMatrix2fv( location, transpose, value );
    }

    public void GLUniformMatrix3Fv( int location, int count, bool transpose, float* value )
    {
        GL.glUniformMatrix3fv( location, count, transpose, value );
    }

    public void GLUniformMatrix3Fv( int location, bool transpose, float[] value )
    {
        GL.glUniformMatrix3fv( location, transpose, value );
    }

    public void GLUniformMatrix4Fv( int location, int count, bool transpose, float* value )
    {
        GL.glUniformMatrix4fv( location, count, transpose, value );
    }

    /// <inheritdoc cref="GL.glValidateProgram(uint)" />
    public void GLValidateProgram( uint program )
    {
        GL.glValidateProgram( program );
    }

    /// <inheritdoc cref="GL.glVertexAttrib1f(uint,float)" />
    public void GLVertexAttrib1F( uint indx, float x )
    {
        GL.glVertexAttrib1f( indx, x );
    }

    public void GLVertexAttrib1Fv( uint indx, float x )
    {
        GL.glVertexAttrib1f( indx, x );
    }

    public void GLVertexAttrib2F( uint indx, float x, float y )
    {
        GL.glVertexAttrib2f( indx, x, y );
    }

    public void GLVertexAttrib2Fv( uint indx, float v0, float v1 )
    {
        GL.glVertexAttrib2f( indx, v0, v1 );
    }

    public void GLVertexAttrib3F( uint indx, float x, float y, float z )
    {
        GL.glVertexAttrib3f( indx, x, y, z );
    }

    public void GLVertexAttrib3Fv( uint indx, float v0, float v1, float v2 )
    {
        GL.glVertexAttrib3f( indx, v0, v1, v2 );
    }

    public void GLVertexAttrib4F( uint indx, float x, float y, float z, float w )
    {
        GL.glVertexAttrib4f( indx, x, y, z, w );
    }

    public void GLVertexAttrib4Fv( uint indx, float v0, float v1, float v2, float v3 )
    {
        GL.glVertexAttrib4f( indx, v0, v1, v2, v3 );
    }

    public void GLVertexAttribPointer( uint indx, int size, int type, bool normalized, int stride, uint ptr )
    {
        GL.glVertexAttribPointer( indx, size, type, normalized, stride, ptr );
    }

    public void GLVertexAttribPointer( uint indx,
                                       int size,
                                       int type,
                                       bool normalized,
                                       int stride,
                                       void* ptr )
    {
        GL.glVertexAttribPointer( indx, size, type, normalized, stride, ptr );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public void GLShaderBinary( int n, int* shaders, int binaryformat, Buffer binary, int length )
    {
        throw new NotImplementedException();
    }

    public void GLDeleteFramebuffer( int framebuffer )
    {
        throw new NotImplementedException();
    }

    public int GLCheckFramebufferStatus( int target )
    {
        throw new NotImplementedException();
    }

    public void GLDeleteTexture( int texture )
    {
        throw new NotImplementedException();
    }
}
