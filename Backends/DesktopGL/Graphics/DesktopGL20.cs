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


using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Utils.Buffers;

using Buffer = LughSharp.LibCore.Utils.Buffers.Buffer;

namespace LughSharp.Backends.DesktopGL.Graphics;

public class DesktopGL20 : IGL20
{
    /// <inheritdoc />
    public void GLActiveTexture( int texture )
    {
    }

    /// <inheritdoc />
    public void GLBindTexture( int target, int texture )
    {
    }

    /// <inheritdoc />
    public void GLBlendFunc( int sfactor, int dfactor )
    {
    }

    /// <inheritdoc />
    public void GLClear( int mask )
    {
    }

    /// <inheritdoc />
    public void GLClearColor( float red, float green, float blue, float alpha )
    {
    }

    /// <inheritdoc />
    public void GLClearDepthf( float depth )
    {
    }

    /// <inheritdoc />
    public void GLClearStencil( int s )
    {
    }

    /// <inheritdoc />
    public void GLColorMask( bool red, bool green, bool blue, bool alpha )
    {
    }

    /// <inheritdoc />
    public void GLCompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, int imageSize, Buffer data )
    {
    }

    /// <inheritdoc />
    public void GLCompressedTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, Buffer data )
    {
    }

    /// <inheritdoc />
    public void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border )
    {
    }

    /// <inheritdoc />
    public void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
    }

    /// <inheritdoc />
    public void GLCullFace( int mode )
    {
    }

    /// <inheritdoc />
    public void GLDeleteTextures( int n, IntBuffer textures )
    {
    }

    /// <inheritdoc />
    public void GLDeleteTexture( int texture )
    {
    }

    /// <inheritdoc />
    public void GLDepthFunc( int func )
    {
    }

    /// <inheritdoc />
    public void GLDepthMask( bool flag )
    {
    }

    /// <inheritdoc />
    public void GLDepthRangef( float zNear, float zFar )
    {
    }

    /// <inheritdoc />
    public void GLDisable( int cap )
    {
    }

    /// <inheritdoc />
    public void GLDrawArrays( int mode, int first, int count )
    {
    }

    /// <inheritdoc />
    public void GLDrawElements( int mode, int count, int type, Buffer indices )
    {
    }

    /// <inheritdoc />
    public void GLEnable( int cap )
    {
    }

    /// <inheritdoc />
    public void GLFinish()
    {
    }

    /// <inheritdoc />
    public void GLFlush()
    {
    }

    /// <inheritdoc />
    public void GLFrontFace( int mode )
    {
    }

    /// <inheritdoc />
    public void GLGenTextures( int n, IntBuffer textures )
    {
    }

    /// <inheritdoc />
    public int GLGenTexture()
    {
        return 0;
    }

    /// <inheritdoc />
    public int GLGetError()
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLGetIntegerv( int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public string GLGetString( int name )
    {
        return null!;
    }

    /// <inheritdoc />
    public void GLHint( int target, int mode )
    {
    }

    /// <inheritdoc />
    public void GLLineWidth( float width )
    {
    }

    /// <inheritdoc />
    public void GLPixelStorei( int pname, int param )
    {
    }

    /// <inheritdoc />
    public void GLPolygonOffset( float factor, float units )
    {
    }

    /// <inheritdoc />
    public void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels )
    {
    }

    /// <inheritdoc />
    public void GLScissor( int x, int y, int width, int height )
    {
    }

    /// <inheritdoc />
    public void GLStencilFunc( int func, int reference, int mask )
    {
    }

    /// <inheritdoc />
    public void GLStencilMask( int mask )
    {
    }

    /// <inheritdoc />
    public void GLStencilOp( int fail, int zfail, int zpass )
    {
    }

    /// <inheritdoc />
    public void GLTexImage2D( int target, int level, int internalformat, int width, int height, int border, int format, int type, Buffer pixels )
    {
    }

    /// <inheritdoc />
    public void GLTexParameterf( int target, int pname, float param )
    {
    }

    /// <inheritdoc />
    public void GLTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, Buffer pixels )
    {
    }

    /// <inheritdoc />
    public void GLViewport( int x, int y, int width, int height )
    {
    }

    /// <inheritdoc />
    public void GLAttachShader( int program, int shader )
    {
    }

    /// <inheritdoc />
    public void GLBindAttribLocation( int program, int index, string name )
    {
    }

    /// <inheritdoc />
    public void GLBindBuffer( int target, int buffer )
    {
    }

    /// <inheritdoc />
    public void GLBindFramebuffer( int target, int framebuffer )
    {
    }

    /// <inheritdoc />
    public void GLBindRenderbuffer( int target, int renderbuffer )
    {
    }

    /// <inheritdoc />
    public void GLBlendColor( float red, float green, float blue, float alpha )
    {
    }

    /// <inheritdoc />
    public void GLBlendEquation( int mode )
    {
    }

    /// <inheritdoc />
    public void GLBlendEquationSeparate( int modeRGB, int modeAlpha )
    {
    }

    /// <inheritdoc />
    public void GLBlendFuncSeparate( int srcRGB, int dstRGB, int srcAlpha, int dstAlpha )
    {
    }

    /// <inheritdoc />
    public void GLBufferData( int target, int size, Buffer data, int usage )
    {
    }

    /// <inheritdoc />
    public void GLBufferSubData( int target, int offset, int size, Buffer data )
    {
    }

    /// <inheritdoc />
    public int GLCheckFramebufferStatus( int target )
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLCompileShader( int shader )
    {
    }

    /// <inheritdoc />
    public int GLCreateProgram()
    {
        return 0;
    }

    /// <inheritdoc />
    public int GLCreateShader( int type )
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLDeleteBuffer( int buffer )
    {
    }

    /// <inheritdoc />
    public void GLDeleteBuffers( int n, IntBuffer buffers )
    {
    }

    /// <inheritdoc />
    public void GLDeleteFramebuffer( int framebuffer )
    {
    }

    /// <inheritdoc />
    public void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    /// <inheritdoc />
    public void GLDeleteProgram( int program )
    {
    }

    /// <inheritdoc />
    public void GLDeleteRenderbuffer( int renderbuffer )
    {
    }

    /// <inheritdoc />
    public void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    /// <inheritdoc />
    public void GLDeleteShader( int shader )
    {
    }

    /// <inheritdoc />
    public void GLDetachShader( int program, int shader )
    {
    }

    /// <inheritdoc />
    public void GLDisableVertexAttribArray( int index )
    {
    }

    /// <inheritdoc />
    public void GLDrawElements( int mode, int count, int type, int indices )
    {
    }

    /// <inheritdoc />
    public void GLEnableVertexAttribArray( int index )
    {
    }

    /// <inheritdoc />
    public void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer )
    {
    }

    /// <inheritdoc />
    public void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
    {
    }

    /// <inheritdoc />
    public int GLGenBuffer()
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLGenBuffers( int n, IntBuffer buffers )
    {
    }

    /// <inheritdoc />
    public void GLGenerateMipmap( int target )
    {
    }

    /// <inheritdoc />
    public int GLGenFramebuffer()
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLGenFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    /// <inheritdoc />
    public int GLGenRenderbuffer()
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLGenRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    /// <inheritdoc />
    public string GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type )
    {
        return null!;
    }

    /// <inheritdoc />
    public string GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type )
    {
        return null!;
    }

    /// <inheritdoc />
    public void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders )
    {
    }

    /// <inheritdoc />
    public int GLGetAttribLocation( int program, string name )
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLGetBooleanv( int pname, Buffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetFloatv( int pname, FloatBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetProgramiv( int program, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public string GLGetProgramInfoLog( int program )
    {
        return null!;
    }

    /// <inheritdoc />
    public void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetShaderiv( int shader, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public string GLGetShaderInfoLog( int shader )
    {
        return null!;
    }

    /// <inheritdoc />
    public void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
    {
    }

    /// <inheritdoc />
    public void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetUniformfv( int program, int location, FloatBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetUniformiv( int program, int location, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public int GLGetUniformLocation( int program, string name )
    {
        return 0;
    }

    /// <inheritdoc />
    public void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
    {
    }

    /// <inheritdoc />
    public bool GLIsBuffer( int buffer )
    {
        return false;
    }

    /// <inheritdoc />
    public bool GLIsEnabled( int cap )
    {
        return false;
    }

    /// <inheritdoc />
    public bool GLIsFramebuffer( int framebuffer )
    {
        return false;
    }

    /// <inheritdoc />
    public bool GLIsProgram( int program )
    {
        return false;
    }

    /// <inheritdoc />
    public bool GLIsRenderbuffer( int renderbuffer )
    {
        return false;
    }

    /// <inheritdoc />
    public bool GLIsShader( int shader )
    {
        return false;
    }

    /// <inheritdoc />
    public bool GLIsTexture( int texture )
    {
        return false;
    }

    /// <inheritdoc />
    public void GLLinkProgram( int program )
    {
    }

    /// <inheritdoc />
    public void GLReleaseShaderCompiler()
    {
    }

    /// <inheritdoc />
    public void GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
    }

    /// <inheritdoc />
    public void GLSampleCoverage( float value, bool invert )
    {
    }

    /// <inheritdoc />
    public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
    {
    }

    /// <inheritdoc />
    public void GLShaderSource( int shader, string str )
    {
    }

    /// <inheritdoc />
    public void GLStencilFuncSeparate( int face, int func, int reference, int mask )
    {
    }

    /// <inheritdoc />
    public void GLStencilMaskSeparate( int face, int mask )
    {
    }

    /// <inheritdoc />
    public void GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
    }

    /// <inheritdoc />
    public void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLTexParameteri( int target, int pname, int param )
    {
    }

    /// <inheritdoc />
    public void GLTexParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    /// <inheritdoc />
    public void GLUniform1F( int location, float x )
    {
    }

    /// <inheritdoc />
    public void GLUniform1Fv( int location, int count, FloatBuffer v )
    {
    }

    /// <inheritdoc />
    public void GLUniform1Fv( int location, int count, float[] v, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniform1I( int location, int x )
    {
    }

    /// <inheritdoc />
    public void GLUniform1Iv( int location, int count, IntBuffer v )
    {
    }

    /// <inheritdoc />
    public void GLUniform1Iv( int location, int count, int[] v, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniform2F( int location, float x, float y )
    {
    }

    /// <inheritdoc />
    public void GLUniform2Fv( int location, int count, FloatBuffer v )
    {
    }

    /// <inheritdoc />
    public void GLUniform2Fv( int location, int count, float[] v, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniform2I( int location, int x, int y )
    {
    }

    /// <inheritdoc />
    public void GLUniform2Iv( int location, int count, IntBuffer v )
    {
    }

    /// <inheritdoc />
    public void GLUniform2Iv( int location, int count, int[] v, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniform3F( int location, float x, float y, float z )
    {
    }

    /// <inheritdoc />
    public void GLUniform3Fv( int location, int count, FloatBuffer v )
    {
    }

    /// <inheritdoc />
    public void GLUniform3Fv( int location, int count, float[] v, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniform3I( int location, int x, int y, int z )
    {
    }

    /// <inheritdoc />
    public void GLUniform3Iv( int location, int count, IntBuffer v )
    {
    }

    /// <inheritdoc />
    public void GLUniform3Iv( int location, int count, int[] v, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniform4F( int location, float x, float y, float z, float w )
    {
    }

    /// <inheritdoc />
    public void GLUniform4Fv( int location, int count, FloatBuffer v )
    {
    }

    /// <inheritdoc />
    public void GLUniform4Fv( int location, int count, float[] v, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniform4I( int location, int x, int y, int z, int w )
    {
    }

    /// <inheritdoc />
    public void GLUniform4Iv( int location, int count, IntBuffer v )
    {
    }

    /// <inheritdoc />
    public void GLUniform4Iv( int location, int count, int[] v, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    /// <inheritdoc />
    public void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    /// <inheritdoc />
    public void GLUseProgram( int program )
    {
    }

    /// <inheritdoc />
    public void GLValidateProgram( int program )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttrib1F( int indx, float x )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttrib1Fv( int indx, FloatBuffer values )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttrib2F( int indx, float x, float y )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttrib2Fv( int indx, FloatBuffer values )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttrib3F( int indx, float x, float y, float z )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttrib3Fv( int indx, FloatBuffer values )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttrib4Fv( int indx, FloatBuffer values )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr )
    {
    }

    /// <inheritdoc />
    public void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, int ptr )
    {
    }
}
