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

namespace LibGDXSharp.Backends.Desktop;

public class DesktopGL20 : IGL20
{
    void IGL20.GLActiveTexture( int texture )
    {
    }

    void IGL20.GLBindTexture( int target, int texture )
    {
    }

    void IGL20.GLBlendFunc( int sfactor, int dfactor )
    {
    }

    void IGL20.GLClear( int mask )
    {
    }

    void IGL20.GLClearColor( float red, float green, float blue, float alpha )
    {
    }

    void IGL20.GLClearDepthf( float depth )
    {
    }

    void IGL20.GLClearStencil( int s )
    {
    }

    void IGL20.GLColorMask( bool red, bool green, bool blue, bool alpha )
    {
    }

    void IGL20.GLCompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border,
                                       int imageSize, Buffer data )
    {
    }

    void IGL20.GLCompressedTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height,
                                          int format,
                                          int imageSize, Buffer data )
    {
    }

    void IGL20.GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height,
                                 int border )
    {
    }

    void IGL20.GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width,
                                    int height )
    {
    }

    void IGL20.GLCullFace( int mode )
    {
    }

    void IGL20.GLDeleteTextures( int n, IntBuffer textures )
    {
    }

    void IGL20.GLDeleteTexture( int texture )
    {
    }

    void IGL20.GLDepthFunc( int func )
    {
    }

    void IGL20.GLDepthMask( bool flag )
    {
    }

    void IGL20.GLDepthRangef( float zNear, float zFar )
    {
    }

    void IGL20.GLDisable( int cap )
    {
    }

    void IGL20.GLDrawArrays( int mode, int first, int count )
    {
    }

    void IGL20.GLDrawElements( int mode, int count, int type, Buffer indices )
    {
    }

    void IGL20.GLEnable( int cap )
    {
    }

    void IGL20.GLFinish()
    {
    }

    void IGL20.GLFlush()
    {
    }

    void IGL20.GLFrontFace( int mode )
    {
    }

    void IGL20.GLGenTextures( int n, IntBuffer textures )
    {
    }

    int IGL20.GLGenTexture()
    {
        return 0;
    }

    int IGL20.GLGetError()
    {
        return 0;
    }

    void IGL20.GLGetIntegerv( int pname, IntBuffer parameters )
    {
    }

    string IGL20.GLGetString( int name )
    {
        return null!;
    }

    void IGL20.GLHint( int target, int mode )
    {
    }

    void IGL20.GLLineWidth( float width )
    {
    }

    void IGL20.GLPixelStorei( int pname, int param )
    {
    }

    void IGL20.GLPolygonOffset( float factor, float units )
    {
    }

    void IGL20.GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels )
    {
    }

    void IGL20.GLScissor( int x, int y, int width, int height )
    {
    }

    void IGL20.GLStencilFunc( int func, int reference, int mask )
    {
    }

    void IGL20.GLStencilMask( int mask )
    {
    }

    void IGL20.GLStencilOp( int fail, int zfail, int zpass )
    {
    }

    void IGL20.GLTexImage2D( int target, int level, int internalformat, int width, int height, int border, int format,
                             int type,
                             Buffer pixels )
    {
    }

    void IGL20.GLTexParameterf( int target, int pname, float param )
    {
    }

    void IGL20.GLTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format,
                                int type,
                                Buffer pixels )
    {
    }

    void IGL20.GLViewport( int x, int y, int width, int height )
    {
    }

    void IGL20.GLAttachShader( int program, int shader )
    {
    }

    void IGL20.GLBindAttribLocation( int program, int index, string name )
    {
    }

    void IGL20.GLBindBuffer( int target, int buffer )
    {
    }

    void IGL20.GLBindFramebuffer( int target, int framebuffer )
    {
    }

    void IGL20.GLBindRenderbuffer( int target, int renderbuffer )
    {
    }

    void IGL20.GLBlendColor( float red, float green, float blue, float alpha )
    {
    }

    void IGL20.GLBlendEquation( int mode )
    {
    }

    void IGL20.GLBlendEquationSeparate( int modeRgb, int modeAlpha )
    {
    }

    void IGL20.GLBlendFuncSeparate( int srcRgb, int dstRgb, int srcAlpha, int dstAlpha )
    {
    }

    void IGL20.GLBufferData( int target, int size, Buffer data, int usage )
    {
    }

    void IGL20.GLBufferSubData( int target, int offset, int size, Buffer data )
    {
    }

    int IGL20.GLCheckFramebufferStatus( int target )
    {
        return 0;
    }

    void IGL20.GLCompileShader( int shader )
    {
    }

    int IGL20.GLCreateProgram()
    {
        return 0;
    }

    int IGL20.GLCreateShader( int type )
    {
        return 0;
    }

    void IGL20.GLDeleteBuffer( int buffer )
    {
    }

    void IGL20.GLDeleteBuffers( int n, IntBuffer buffers )
    {
    }

    void IGL20.GLDeleteFramebuffer( int framebuffer )
    {
    }

    void IGL20.GLDeleteFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    void IGL20.GLDeleteProgram( int program )
    {
    }

    void IGL20.GLDeleteRenderbuffer( int renderbuffer )
    {
    }

    void IGL20.GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    void IGL20.GLDeleteShader( int shader )
    {
    }

    void IGL20.GLDetachShader( int program, int shader )
    {
    }

    void IGL20.GLDisableVertexAttribArray( int index )
    {
    }

    void IGL20.GLDrawElements( int mode, int count, int type, int indices )
    {
    }

    void IGL20.GLEnableVertexAttribArray( int index )
    {
    }

    void IGL20.GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer )
    {
    }

    void IGL20.GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
    {
    }

    int IGL20.GLGenBuffer()
    {
        return 0;
    }

    void IGL20.GLGenBuffers( int n, IntBuffer buffers )
    {
    }

    void IGL20.GLGenerateMipmap( int target )
    {
    }

    int IGL20.GLGenFramebuffer()
    {
        return 0;
    }

    void IGL20.GLGenFramebuffers( int n, IntBuffer framebuffers )
    {
    }

    int IGL20.GLGenRenderbuffer()
    {
        return 0;
    }

    void IGL20.GLGenRenderbuffers( int n, IntBuffer renderbuffers )
    {
    }

    string IGL20.GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type )
    {
        return null!;
    }

    string IGL20.GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type )
    {
        return null!;
    }

    void IGL20.GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders )
    {
    }

    int IGL20.GLGetAttribLocation( int program, string name )
    {
        return 0;
    }

    void IGL20.GLGetBooleanv( int pname, Buffer parameters )
    {
    }

    void IGL20.GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    void IGL20.GLGetFloatv( int pname, FloatBuffer parameters )
    {
    }

    void IGL20.GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
    {
    }

    void IGL20.GLGetProgramiv( int program, int pname, IntBuffer parameters )
    {
    }

    string IGL20.GLGetProgramInfoLog( int program )
    {
        return null!;
    }

    void IGL20.GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    void IGL20.GLGetShaderiv( int shader, int pname, IntBuffer parameters )
    {
    }

    string IGL20.GLGetShaderInfoLog( int shader )
    {
        return null!;
    }

    void IGL20.GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
    {
    }

    void IGL20.GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
    }

    void IGL20.GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    void IGL20.GLGetUniformfv( int program, int location, FloatBuffer parameters )
    {
    }

    void IGL20.GLGetUniformiv( int program, int location, IntBuffer parameters )
    {
    }

    int IGL20.GLGetUniformLocation( int program, string name )
    {
        return 0;
    }

    void IGL20.GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
    {
    }

    void IGL20.GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
    {
    }

    void IGL20.GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
    {
    }

    bool IGL20.GLIsBuffer( int buffer )
    {
        return false;
    }

    bool IGL20.GLIsEnabled( int cap )
    {
        return false;
    }

    bool IGL20.GLIsFramebuffer( int framebuffer )
    {
        return false;
    }

    bool IGL20.GLIsProgram( int program )
    {
        return false;
    }

    bool IGL20.GLIsRenderbuffer( int renderbuffer )
    {
        return false;
    }

    bool IGL20.GLIsShader( int shader )
    {
        return false;
    }

    bool IGL20.GLIsTexture( int texture )
    {
        return false;
    }

    void IGL20.GLLinkProgram( int program )
    {
    }

    void IGL20.GLReleaseShaderCompiler()
    {
    }

    void IGL20.GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
    }

    void IGL20.GLSampleCoverage( float value, bool invert )
    {
    }

    void IGL20.GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
    {
    }

    void IGL20.GLShaderSource( int shader, string str )
    {
    }

    void IGL20.GLStencilFuncSeparate( int face, int func, int reference, int mask )
    {
    }

    void IGL20.GLStencilMaskSeparate( int face, int mask )
    {
    }

    void IGL20.GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
    }

    void IGL20.GLTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
    }

    void IGL20.GLTexParameteri( int target, int pname, int param )
    {
    }

    void IGL20.GLTexParameteriv( int target, int pname, IntBuffer parameters )
    {
    }

    void IGL20.GLUniform1F( int location, float x )
    {
    }

    void IGL20.GLUniform1Fv( int location, int count, FloatBuffer v )
    {
    }

    void IGL20.GLUniform1Fv( int location, int count, float[] v, int offset )
    {
    }

    void IGL20.GLUniform1I( int location, int x )
    {
    }

    void IGL20.GLUniform1Iv( int location, int count, IntBuffer v )
    {
    }

    void IGL20.GLUniform1Iv( int location, int count, int[] v, int offset )
    {
    }

    void IGL20.GLUniform2F( int location, float x, float y )
    {
    }

    void IGL20.GLUniform2Fv( int location, int count, FloatBuffer v )
    {
    }

    void IGL20.GLUniform2Fv( int location, int count, float[] v, int offset )
    {
    }

    void IGL20.GLUniform2I( int location, int x, int y )
    {
    }

    void IGL20.GLUniform2Iv( int location, int count, IntBuffer v )
    {
    }

    void IGL20.GLUniform2Iv( int location, int count, int[] v, int offset )
    {
    }

    void IGL20.GLUniform3F( int location, float x, float y, float z )
    {
    }

    void IGL20.GLUniform3Fv( int location, int count, FloatBuffer v )
    {
    }

    void IGL20.GLUniform3Fv( int location, int count, float[] v, int offset )
    {
    }

    void IGL20.GLUniform3I( int location, int x, int y, int z )
    {
    }

    void IGL20.GLUniform3Iv( int location, int count, IntBuffer v )
    {
    }

    void IGL20.GLUniform3Iv( int location, int count, int[] v, int offset )
    {
    }

    void IGL20.GLUniform4F( int location, float x, float y, float z, float w )
    {
    }

    void IGL20.GLUniform4Fv( int location, int count, FloatBuffer v )
    {
    }

    void IGL20.GLUniform4Fv( int location, int count, float[] v, int offset )
    {
    }

    void IGL20.GLUniform4I( int location, int x, int y, int z, int w )
    {
    }

    void IGL20.GLUniform4Iv( int location, int count, IntBuffer v )
    {
    }

    void IGL20.GLUniform4Iv( int location, int count, int[] v, int offset )
    {
    }

    void IGL20.GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    void IGL20.GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    void IGL20.GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    void IGL20.GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    void IGL20.GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
    }

    void IGL20.GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
    {
    }

    void IGL20.GLUseProgram( int program )
    {
    }

    void IGL20.GLValidateProgram( int program )
    {
    }

    void IGL20.GLVertexAttrib1F( int indx, float x )
    {
    }

    void IGL20.GLVertexAttrib1Fv( int indx, FloatBuffer values )
    {
    }

    void IGL20.GLVertexAttrib2F( int indx, float x, float y )
    {
    }

    void IGL20.GLVertexAttrib2Fv( int indx, FloatBuffer values )
    {
    }

    void IGL20.GLVertexAttrib3F( int indx, float x, float y, float z )
    {
    }

    void IGL20.GLVertexAttrib3Fv( int indx, FloatBuffer values )
    {
    }

    void IGL20.GLVertexAttrib4F( int indx, float x, float y, float z, float w )
    {
    }

    void IGL20.GLVertexAttrib4Fv( int indx, FloatBuffer values )
    {
    }

    /// In OpenGl core profiles (3.1+), passing a pointer to client memory is not valid.
    /// In 3.0 and later, use the other version of this function instead, pass a zero-based
    /// offset which references the buffer currently bound to GL_ARRAY_BUFFER.
    void IGL20.GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr )
    {
    }

    void IGL20.GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, int ptr )
    {
    }
}