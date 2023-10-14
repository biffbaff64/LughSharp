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

namespace LibGDXSharp.Backends.Desktop;

public class GL20 : IGL20
{
    public void GLActiveTexture( int texture )
    {
    }

    public void GLBindTexture( int target, int texture )
    {
    }

    public void GLBlendFunc( int sfactor, int dfactor )
    {
    }

    public void GLClear( int mask )
    {
    }

    public void GLClearColor( float red, float green, float blue, float alpha )
    {
    }

    public void GLClearDepthf( float depth )
    {
    }

    public void GLClearStencil( int s )
    {
    }

    public void GLColorMask( bool red, bool green, bool blue, bool alpha )
    {
    }

    public void GLCompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border,
                                        int imageSize, Buffer data )
    {
    }

    public void GLCompressedTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height,
                                           int format,
                                           int imageSize, Buffer data )
    {
    }

    public void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height,
                                  int border )
    {
    }

    public void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width,
                                     int height )
    {
    }

    public void GLCullFace( int mode )
    {
    }

    public void GLDeleteTextures( int n, IntBuffer textures )
    {
    }

    public void GLDeleteTexture( int texture )
    {
    }

    public void GLDepthFunc( int func )
    {
    }

    public void GLDepthMask( bool flag )
    {
    }

    public void GLDepthRangef( float zNear, float zFar )
    {
    }

    public void GLDisable( int cap )
    {
    }

    public void GLDrawArrays( int mode, int first, int count )
    {
    }

    public void GLDrawElements( int mode, int count, int type, Buffer indices )
    {
    }

    public void GLEnable( int cap )
    {
    }

    public void GLFinish()
    {
    }

    public void GLFlush()
    {
    }

    public void GLFrontFace( int mode )
    {
    }

    public void GLGenTextures( int n, IntBuffer textures )
    {
    }

    public int GLGenTexture() => 0;

    public int GLGetError() => 0;

    public void GLGetIntegerv( int pname, IntBuffer parameters )
    {
    }

    public string GLGetString( int name ) => null;

    public void GLHint( int target, int mode )
    {
    }

    public void GLLineWidth( float width )
    {
    }

    public void GLPixelStorei( int pname, int param )
    {
    }

    public void GLPolygonOffset( float factor, float units )
    {
    }

    public void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels )
    {
    }

    public void GLScissor( int x, int y, int width, int height )
    {
    }

    public void GLStencilFunc( int func, int reference, int mask )
    {
    }

    public void GLStencilMask( int mask )
    {
    }

    public void GLStencilOp( int fail, int zfail, int zpass )
    {
    }

    public void GLTexImage2D( int target, int level, int internalformat, int width, int height, int border, int format,
                              int type,
                              Buffer pixels )
    {
    }

    public void GLTexParameterf( int target, int pname, float param )
    {
    }

    public void GLTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format,
                                 int type,
                                 Buffer pixels )
    {
    }

    public void GLViewport( int x, int y, int width, int height )
    {
    }

    public void GLAttachShader( int program, int shader )
    {
    }

    public void GLBindAttribLocation( int program, int index, string name )
    {
    }

    public void GLBindBuffer( int target, int buffer )
    {
    }

    public void GLBindFramebuffer( int target, int framebuffer )
    {
    }

    public void GLBindRenderbuffer( int target, int renderbuffer )
    {
    }

    public void GLBlendColor( float red, float green, float blue, float alpha )
    {
    }

    public void GLBlendEquation( int mode )
    {
    }

    public void GLBlendEquationSeparate( int modeRgb, int modeAlpha )
    {
    }

    public void GLBlendFuncSeparate( int srcRgb, int dstRgb, int srcAlpha, int dstAlpha )
    {
    }

    public void GLBufferData( int target, int size, Buffer data, int usage )
    {
    }

    public void GLBufferSubData( int target, int offset, int size, Buffer data )
    {
    }

    public int GLCheckFramebufferStatus( int target ) => 0;

    public void GLCompileShader( int shader )
    {
    }

    public int GLCreateProgram() => 0;

    public int GLCreateShader( int type ) => 0;

    public void GLDeleteBuffer( int buffer )
    {
    }

    public void GLDeleteBuffers( int n, IntBuffer buffers )
    {
    }

    public void GLDeleteFramebuffer( int framebuffer )
    {
    }

    public void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    public void GLDeleteProgram( int program )
    {
    }

    public void GLDeleteRenderbuffer( int renderbuffer )
    {
    }

    public void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    public void GLDeleteShader( int shader )
    {
    }

    public void GLDetachShader( int program, int shader )
    {
    }

    public void GLDisableVertexAttribArray( int index )
    {
    }

    public void GLDrawElements( int mode, int count, int type, int indices )
    {
    }

    public void GLEnableVertexAttribArray( int index )
    {
    }

    public void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer )
    {
    }

    public void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
    {
    }

    public int GLGenBuffer() => 0;

    public void GLGenBuffers( int n, IntBuffer buffers )
    {
    }

    public void GLGenerateMipmap( int target )
    {
    }

    public int GLGenFramebuffer() => 0;

    public void GLGenFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    public int GLGenRenderbuffer() => 0;

    public void GLGenRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    public string GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type ) => null;

    public string GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type ) => null;

    public void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders )
    {
    }

    public int GLGetAttribLocation( int program, string name ) => 0;

    public void GLGetBooleanv( int pname, Buffer parameters )
    {
    }

    public void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    public void GLGetFloatv( int pname, FloatBuffer parameters )
    {
    }

    public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
    {
    }

    public void GLGetProgramiv( int program, int pname, IntBuffer parameters )
    {
    }

    public string GLGetProgramInfoLog( int program ) => null;

    public void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    public void GLGetShaderiv( int shader, int pname, IntBuffer parameters )
    {
    }

    public string GLGetShaderInfoLog( int shader ) => null;

    public void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
    {
    }

    public void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
    }

    public void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    public void GLGetUniformfv( int program, int location, FloatBuffer parameters )
    {
    }

    public void GLGetUniformiv( int program, int location, IntBuffer parameters )
    {
    }

    public int GLGetUniformLocation( int program, string name ) => 0;

    public void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
    {
    }

    public void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
    {
    }

    public void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
    {
    }

    public bool GLIsBuffer( int buffer ) => false;

    public bool GLIsEnabled( int cap ) => false;

    public bool GLIsFramebuffer( int framebuffer ) => false;

    public bool GLIsProgram( int program ) => false;

    public bool GLIsRenderbuffer( int renderbuffer ) => false;

    public bool GLIsShader( int shader ) => false;

    public bool GLIsTexture( int texture ) => false;

    public void GLLinkProgram( int program )
    {
    }

    public void GLReleaseShaderCompiler()
    {
    }

    public void GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
    }

    public void GLSampleCoverage( float value, bool invert )
    {
    }

    public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
    {
    }

    public void GLShaderSource( int shader, string str )
    {
    }

    public void GLStencilFuncSeparate( int face, int func, int reference, int mask )
    {
    }

    public void GLStencilMaskSeparate( int face, int mask )
    {
    }

    public void GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
    }

    public void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
    }

    public void GLTexParameteri( int target, int pname, int param )
    {
    }

    public void GLTexParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    public void GLUniform1F( int location, float x )
    {
    }

    public void GLUniform1Fv( int location, int count, FloatBuffer v )
    {
    }

    public void GLUniform1Fv( int location, int count, float[] v, int offset )
    {
    }

    public void GLUniform1I( int location, int x )
    {
    }

    public void GLUniform1Iv( int location, int count, IntBuffer v )
    {
    }

    public void GLUniform1Iv( int location, int count, int[] v, int offset )
    {
    }

    public void GLUniform2F( int location, float x, float y )
    {
    }

    public void GLUniform2Fv( int location, int count, FloatBuffer v )
    {
    }

    public void GLUniform2Fv( int location, int count, float[] v, int offset )
    {
    }

    public void GLUniform2I( int location, int x, int y )
    {
    }

    public void GLUniform2Iv( int location, int count, IntBuffer v )
    {
    }

    public void GLUniform2Iv( int location, int count, int[] v, int offset )
    {
    }

    public void GLUniform3F( int location, float x, float y, float z )
    {
    }

    public void GLUniform3Fv( int location, int count, FloatBuffer v )
    {
    }

    public void GLUniform3Fv( int location, int count, float[] v, int offset )
    {
    }

    public void GLUniform3I( int location, int x, int y, int z )
    {
    }

    public void GLUniform3Iv( int location, int count, IntBuffer v )
    {
    }

    public void GLUniform3Iv( int location, int count, int[] v, int offset )
    {
    }

    public void GLUniform4F( int location, float x, float y, float z, float w )
    {
    }

    public void GLUniform4Fv( int location, int count, FloatBuffer v )
    {
    }

    public void GLUniform4Fv( int location, int count, float[] v, int offset )
    {
    }

    public void GLUniform4I( int location, int x, int y, int z, int w )
    {
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

    public void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    public void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    public void GLUseProgram( int program )
    {
    }

    public void GLValidateProgram( int program )
    {
    }

    public void GLVertexAttrib1F( int indx, float x )
    {
    }

    public void GLVertexAttrib1Fv( int indx, FloatBuffer values )
    {
    }

    public void GLVertexAttrib2F( int indx, float x, float y )
    {
    }

    public void GLVertexAttrib2Fv( int indx, FloatBuffer values )
    {
    }

    public void GLVertexAttrib3F( int indx, float x, float y, float z )
    {
    }

    public void GLVertexAttrib3Fv( int indx, FloatBuffer values )
    {
    }

    public void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
    {
    }

    public void GLVertexAttrib4Fv( int indx, FloatBuffer values )
    {
    }

    /// In OpenGl core profiles (3.1+), passing a pointer to client memory is not valid.
    /// In 3.0 and later, use the other version of this function instead, pass a zero-based
    /// offset which references the buffer currently bound to GL_ARRAY_BUFFER.
    public void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr )
    {
    }

    public void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, int ptr )
    {
    }
}