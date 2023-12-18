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
using ErrorCode = OpenGL.ErrorCode;

namespace LibGDXSharp.Graphics.Profiling;

[PublicAPI]
public class GL30Interceptor : GLInterceptor, IGL30
{
    public IGL30 GL30 { get; set; }

    public GL30Interceptor( GLProfiler glProfiler, IGL30 gl30 )
        : base( glProfiler )
    {
        this.GL30 = gl30;
    }

    private void Check()
    {
        ErrorCode error = GL30.GLGetError();

        while ( error != IGL20.GL_NO_ERROR )
        {
            glProfiler.Listener.OnError( error );
            error = GL30.GLGetError();
        }
    }

    public override void GLActiveTexture( int texture )
    {
        Calls++;
        GL30.GLActiveTexture( texture );
        Check();
    }

    public override void GLBindTexture( int target, int texture )
    {
        TextureBindings++;
        Calls++;
        GL30.GLBindTexture( target, texture );
        Check();
    }

    public override void GLBlendFunc( int sfactor, int dfactor )
    {
        Calls++;
        GL30.GLBlendFunc( sfactor, dfactor );
        Check();
    }

    public override void GLClear( int mask )
    {
        Calls++;
        GL30.GLClear( mask );
        Check();
    }

    public override void GLClearColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        GL30.GLClearColor( red, green, blue, alpha );
        Check();
    }

    public override void GLClearDepthf( float depth )
    {
        Calls++;
        GL30.GLClearDepthf( depth );
        Check();
    }

    public override void GLClearStencil( int s )
    {
        Calls++;
        GL30.GLClearStencil( s );
        Check();
    }

    public override void GLColorMask( bool red, bool green, bool blue, bool alpha )
    {
        Calls++;
        GL30.GLColorMask( red, green, blue, alpha );
        Check();
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
        Calls++;
        GL30.GLCompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, data );
        Check();
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
        Calls++;
        GL30.GLCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
        Check();
    }

    public override void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border )
    {
        Calls++;
        GL30.GLCopyTexImage2D( target, level, internalformat, x, y, width, height, border );
        Check();
    }

    public override void GLCopyTexSubImage2D( TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Calls++;
        GL30.GLCopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
        Check();
    }

    public override void GLCullFace( CullFaceMode mode )
    {
        Calls++;
        GL30.GLCullFace( mode );
        Check();
    }

    public override void GLDeleteTextures( int n, int textures )
    {
        Calls++;
        GL30.GLDeleteTextures( n, textures );
        Check();
    }

    public override void GLDeleteTexture( int texture )
    {
        Calls++;
        GL30.GLDeleteTexture( texture );
        Check();
    }

    public override void GLDepthFunc( DepthFunction func )
    {
        Calls++;
        GL30.GLDepthFunc( func );
        Check();
    }

    public override void GLDepthMask( bool flag )
    {
        Calls++;
        GL30.GLDepthMask( flag );
        Check();
    }

    public override void GLDepthRangef( float zNear, float zFar )
    {
        Calls++;
        GL30.GLDepthRangef( zNear, zFar );
        Check();
    }

    public override void GLDisable( EnableCap cap )
    {
        Calls++;
        GL30.GLDisable( cap );
        Check();
    }

    public override void GLDrawArrays( PrimitiveType mode, int first, int count )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL30.GLDrawArrays( mode, first, count );
        Check();
    }

    public override void GLDrawElements( PrimitiveType mode, int count, DrawElementsType type, Buffer indices )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL30.GLDrawElements( mode, count, type, indices );
        Check();
    }

    public override void GLEnable( EnableCap cap )
    {
        Calls++;
        GL30.GLEnable( cap );
        Check();
    }

    public override void GLFinish()
    {
        Calls++;
        GL30.GLFinish();
        Check();
    }

    public override void GLFlush()
    {
        Calls++;
        GL30.GLFlush();
        Check();
    }

    public override void GLFrontFace( FrontFaceDirection mode )
    {
        Calls++;
        GL30.GLFrontFace( mode );
        Check();
    }

    public override void GLGenTextures( int n, int[] textures )
    {
        Calls++;
        GL30.GLGenTextures( n, textures );
        Check();
    }

    public override int GLGenTexture()
    {
        Calls++;
        var result = GL30.GLGenTexture();
        Check();

        return result;
    }

    public override ErrorCode GLGetError()
    {
        Calls++;

        return GL30.GLGetError();
    }

    public override void GLGetIntegerv( int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetIntegerv( pname, parameters );
        Check();
    }

    public override string GLGetString( int name )
    {
        Calls++;
        var result = GL30.GLGetString( name );
        Check();

        return result;
    }

    public override void GLHint( int target, int mode )
    {
        Calls++;
        GL30.GLHint( target, mode );
        Check();
    }

    public override void GLLineWidth( float width )
    {
        Calls++;
        GL30.GLLineWidth( width );
        Check();
    }

    public override void GLPixelStorei( int pname, int param )
    {
        Calls++;
        GL30.GLPixelStorei( pname, param );
        Check();
    }

    public override void GLPolygonOffset( float factor, float units )
    {
        Calls++;
        GL30.GLPolygonOffset( factor, units );
        Check();
    }

    public override void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels )
    {
        Calls++;
        GL30.GLReadPixels( x, y, width, height, format, type, pixels );
        Check();
    }

    public override void GLScissor( int x, int y, int width, int height )
    {
        Calls++;
        GL30.GLScissor( x, y, width, height );
        Check();
    }

    public override void GLStencilFunc( int func, int r, int mask )
    {
        Calls++;
        GL30.GLStencilFunc( func, r, mask );
        Check();
    }

    public override void GLStencilMask( int mask )
    {
        Calls++;
        GL30.GLStencilMask( mask );
        Check();
    }

    public override void GLStencilOp( int fail, int zfail, int zpass )
    {
        Calls++;
        GL30.GLStencilOp( fail, zfail, zpass );
        Check();
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
        Calls++;
        GL30.GLTexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        Check();
    }

    public override void GLTexParameterf( int target, int pname, float param )
    {
        Calls++;
        GL30.GLTexParameterf( target, pname, param );
        Check();
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
        Calls++;
        GL30.GLTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
        Check();
    }

    public override void GLViewport( int x, int y, int width, int height )
    {
        Calls++;
        GL30.GLViewport( x, y, width, height );
        Check();
    }

    public override void GLAttachShader( int program, int shader )
    {
        Calls++;
        GL30.GLAttachShader( program, shader );
        Check();
    }

    public override void GLBindAttribLocation( int program, int index, string name )
    {
        Calls++;
        GL30.GLBindAttribLocation( program, index, name );
        Check();
    }

    public override void GLBindBuffer( int target, int buffer )
    {
        Calls++;
        GL30.GLBindBuffer( target, buffer );
        Check();
    }

    public override void GLBindFramebuffer( int target, int framebuffer )
    {
        Calls++;
        GL30.GLBindFramebuffer( target, framebuffer );
        Check();
    }

    public override void GLBindRenderbuffer( int target, int renderbuffer )
    {
        Calls++;
        GL30.GLBindRenderbuffer( target, renderbuffer );
        Check();
    }

    public override void GLBlendColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        GL30.GLBlendColor( red, green, blue, alpha );
        Check();
    }

    public override void GLBlendEquation( int mode )
    {
        Calls++;
        GL30.GLBlendEquation( mode );
        Check();
    }

    public override void GLBlendEquationSeparate( int modeRGB, int modeAlpha )
    {
        Calls++;
        GL30.GLBlendEquationSeparate( modeRGB, modeAlpha );
        Check();
    }

    public override void GLBlendFuncSeparate( int srcRGB, int dstRGB, int srcAlpha, int dstAlpha )
    {
        Calls++;
        GL30.GLBlendFuncSeparate( srcRGB, dstRGB, srcAlpha, dstAlpha );
        Check();
    }

    public override void GLBufferData( int target, int size, Buffer data, int usage )
    {
        Calls++;
        GL30.GLBufferData( target, size, data, usage );
        Check();
    }

    public override void GLBufferSubData( int target, int offset, int size, Buffer data )
    {
        Calls++;
        GL30.GLBufferSubData( target, offset, size, data );
        Check();
    }

    public override int GLCheckFramebufferStatus( int target )
    {
        Calls++;
        var result = GL30.GLCheckFramebufferStatus( target );
        Check();

        return result;
    }

    public override void GLCompileShader( int shader )
    {
        Calls++;
        GL30.GLCompileShader( shader );
        Check();
    }

    public override int GLCreateProgram()
    {
        Calls++;
        var result = GL30.GLCreateProgram();
        Check();

        return result;
    }

    public override int GLCreateShader( ShaderType type )
    {
        Calls++;
        var result = GL30.GLCreateShader( type );
        Check();

        return result;
    }

    public override void GLDeleteBuffer( int buffer )
    {
        Calls++;
        GL30.GLDeleteBuffer( buffer );
        Check();
    }

    public override void GLDeleteBuffers( int n, int buffers )
    {
        Calls++;
        GL30.GLDeleteBuffers( n, buffers );
        Check();
    }

    public override void GLDeleteFramebuffer( int framebuffer )
    {
        Calls++;
        GL30.GLDeleteFramebuffer( framebuffer );
        Check();
    }

    public override void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
    {
        Calls++;
        GL30.GLDeleteFramebuffers( n, framebuffers );
        Check();
    }

    public override void GLDeleteProgram( int program )
    {
        Calls++;
        GL30.GLDeleteProgram( program );
        Check();
    }

    public override void GLDeleteRenderbuffer( int renderbuffer )
    {
        Calls++;
        GL30.GLDeleteRenderbuffer( renderbuffer );
        Check();
    }

    public override void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
    {
        Calls++;
        GL30.GLDeleteRenderbuffers( n, renderbuffers );
        Check();
    }

    public override void GLDeleteShader( int shader )
    {
        Calls++;
        GL30.GLDeleteShader( shader );
        Check();
    }

    public override void GLDetachShader( int program, int shader )
    {
        Calls++;
        GL30.GLDetachShader( program, shader );
        Check();
    }

    public override void GLDisableVertexAttribArray( int index )
    {
        Calls++;
        GL30.GLDisableVertexAttribArray( index );
        Check();
    }

    public override void GLDrawElements( PrimitiveType mode, int count, DrawElementsType type, int indices )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL30.GLDrawElements( mode, count, type, indices );
        Check();
    }

    public override void GLEnableVertexAttribArray( int index )
    {
        Calls++;
        GL30.GLEnableVertexAttribArray( index );
        Check();
    }

    public override void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer )
    {
        Calls++;
        GL30.GLFramebufferRenderbuffer( target, attachment, renderbuffertarget, renderbuffer );
        Check();
    }

    public override void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
    {
        Calls++;
        GL30.GLFramebufferTexture2D( target, attachment, textarget, texture, level );
        Check();
    }

    public override int GLGenBuffer()
    {
        Calls++;
        var result = GL30.GLGenBuffer();
        Check();

        return result;
    }

    public override void GLGenBuffers( int n, int[] buffers )
    {
        Calls++;
        GL30.GLGenBuffers( n, buffers );
        Check();
    }

    public override void GLGenerateMipmap( int target )
    {
        Calls++;
        GL30.GLGenerateMipmap( target );
        Check();
    }

    public override int GLGenFramebuffer()
    {
        Calls++;
        var result = GL30.GLGenFramebuffer();
        Check();

        return result;
    }

    public override void GLGenFramebuffers( int n, IntBuffer framebuffers )
    {
        Calls++;
        GL30.GLGenFramebuffers( n, framebuffers );
        Check();
    }


    public override int GLGenRenderbuffer()
    {
        Calls++;
        var result = GL30.GLGenRenderbuffer();
        Check();

        return result;
    }


    public override void GLGenRenderbuffers( int n, IntBuffer renderbuffers )
    {
        Calls++;
        GL30.GLGenRenderbuffers( n, renderbuffers );
        Check();
    }


    public override string GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type )
    {
        Calls++;
        var result = GL30.GLGetActiveAttrib( program, index, size, type );
        Check();

        return result;
    }


    public override string GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type )
    {
        Calls++;
        var result = GL30.GLGetActiveUniform( program, index, size, type );
        Check();

        return result;
    }

    public override void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders )
    {
        Calls++;
        GL30.GLGetAttachedShaders( program, maxcount, count, shaders );
        Check();
    }

    public override int GLGetAttribLocation( int program, string name )
    {
        Calls++;
        var result = GL30.GLGetAttribLocation( program, name );
        Check();

        return result;
    }

    public override void GLGetBooleanv( int pname, Buffer parameters )
    {
        Calls++;
        GL30.GLGetBooleanv( pname, parameters );
        Check();
    }

    public override void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetBufferParameteriv( target, pname, parameters );
        Check();
    }

    public override void GLGetFloatv( int pname, FloatBuffer parameters )
    {
        Calls++;
        GL30.GLGetFloatv( pname, parameters );
        Check();
    }

    public override void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetFramebufferAttachmentParameteriv( target, attachment, pname, parameters );
        Check();
    }

    public override void GLGetProgramiv( int program, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetProgramiv( program, pname, parameters );
        Check();
    }

    public override string GLGetProgramInfoLog( int program )
    {
        Calls++;
        var result = GL30.GLGetProgramInfoLog( program );
        Check();

        return result;
    }

    public override void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetRenderbufferParameteriv( target, pname, parameters );
        Check();
    }

    public override void GLGetShaderiv( int shader, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetShaderiv( shader, pname, parameters );
        Check();
    }

    public override string GLGetShaderInfoLog( int shader )
    {
        Calls++;
        var result = GL30.GLGetShaderInfoLog( shader );
        Check();

        return result;
    }

    public override void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
    {
        Calls++;
        GL30.GLGetShaderPrecisionFormat( shadertype, precisiontype, range, precision );
        Check();
    }

    public override void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
        Calls++;
        GL30.GLGetTexParameterfv( target, pname, parameters );
        Check();
    }

    public override void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetTexParameteriv( target, pname, parameters );
        Check();
    }

    public override void GLGetUniformfv( int program, int location, FloatBuffer parameters )
    {
        Calls++;
        GL30.GLGetUniformfv( program, location, parameters );
        Check();
    }

    public override void GLGetUniformiv( int program, int location, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetUniformiv( program, location, parameters );
        Check();
    }

    public override int GLGetUniformLocation( int program, string name )
    {
        Calls++;
        var result = GL30.GLGetUniformLocation( program, name );
        Check();

        return result;
    }

    public override void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
    {
        Calls++;
        GL30.GLGetVertexAttribfv( index, pname, parameters );
        Check();
    }

    public override void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetVertexAttribiv( index, pname, parameters );
        Check();
    }

    public override void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
    {
        Calls++;
        GL30.GLGetVertexAttribPointerv( index, pname, pointer );
        Check();
    }

    public override bool GLIsBuffer( int buffer )
    {
        Calls++;
        var result = GL30.GLIsBuffer( buffer );
        Check();

        return result;
    }

    public override bool GLIsEnabled( int cap )
    {
        Calls++;
        var result = GL30.GLIsEnabled( cap );
        Check();

        return result;
    }

    public override bool GLIsFramebuffer( int framebuffer )
    {
        Calls++;
        var result = GL30.GLIsFramebuffer( framebuffer );
        Check();

        return result;
    }

    public override bool GLIsProgram( int program )
    {
        Calls++;
        var result = GL30.GLIsProgram( program );
        Check();

        return result;
    }

    public override bool GLIsRenderbuffer( int renderbuffer )
    {
        Calls++;
        var result = GL30.GLIsRenderbuffer( renderbuffer );
        Check();

        return result;
    }

    public override bool GLIsShader( int shader )
    {
        Calls++;
        var result = GL30.GLIsShader( shader );
        Check();

        return result;
    }

    public override bool GLIsTexture( int texture )
    {
        Calls++;
        var result = GL30.GLIsTexture( texture );
        Check();

        return result;
    }

    public override void GLLinkProgram( int program )
    {
        Calls++;
        GL30.GLLinkProgram( program );
        Check();
    }

    public override void GLReleaseShaderCompiler()
    {
        Calls++;
        GL30.GLReleaseShaderCompiler();
        Check();
    }

    public override void GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
        Calls++;
        GL30.GLRenderbufferStorage( target, internalformat, width, height );
        Check();
    }

    public override void GLSampleCoverage( float value, bool invert )
    {
        Calls++;
        GL30.GLSampleCoverage( value, invert );
        Check();
    }

    public override void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
    {
        Calls++;
        GL30.GLShaderBinary( n, shaders, binaryformat, binary, length );
        Check();
    }

    public override void GLShaderSource( int shader, string str )
    {
        Calls++;
        GL30.GLShaderSource( shader, str );
        Check();
    }

    public override void GLStencilFuncSeparate( int face, int func, int r, int mask )
    {
        Calls++;
        GL30.GLStencilFuncSeparate( face, func, r, mask );
        Check();
    }

    public override void GLStencilMaskSeparate( int face, int mask )
    {
        Calls++;
        GL30.GLStencilMaskSeparate( face, mask );
        Check();
    }

    public override void GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
        Calls++;
        GL30.GLStencilOpSeparate( face, fail, zfail, zpass );
        Check();
    }

    public override void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
        Calls++;
        GL30.GLTexParameterfv( target, pname, parameters );
        Check();
    }

    public override void GLTexParameteri( int target, int pname, int param )
    {
        Calls++;
        GL30.GLTexParameteri( target, pname, param );
        Check();
    }

    public override void GLTexParameteriv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLTexParameteriv( target, pname, parameters );
        Check();
    }

    public override void GLUniform1F( int location, float x )
    {
        Calls++;
        GL30.GLUniform1F( location, x );
        Check();
    }

    public override void GLUniform1Fv( int location, int count, FloatBuffer v )
    {
        Calls++;
        GL30.GLUniform1Fv( location, count, v );
        Check();
    }

    public override void GLUniform1Fv( int location, int count, float[] v, int offset )
    {
        Calls++;
        GL30.GLUniform1Fv( location, count, v, offset );
        Check();
    }

    public override void GLUniform1I( int location, int x )
    {
        Calls++;
        GL30.GLUniform1I( location, x );
        Check();
    }

    public override void GLUniform1Iv( int location, int count, IntBuffer v )
    {
        Calls++;
        GL30.GLUniform1Iv( location, count, v );
        Check();
    }

    public override void GLUniform1Iv( int location, int count, int[] v, int offset )
    {
        Calls++;
        GL30.GLUniform1Iv( location, count, v, offset );
        Check();
    }

    public override void GLUniform2F( int location, float x, float y )
    {
        Calls++;
        GL30.GLUniform2F( location, x, y );
        Check();
    }

    public override void GLUniform2Fv( int location, int count, FloatBuffer v )
    {
        Calls++;
        GL30.GLUniform2Fv( location, count, v );
        Check();
    }

    public override void GLUniform2Fv( int location, int count, float[] v, int offset )
    {
        Calls++;
        GL30.GLUniform2Fv( location, count, v, offset );
        Check();
    }

    public override void GLUniform2I( int location, int x, int y )
    {
        Calls++;
        GL30.GLUniform2I( location, x, y );
        Check();
    }

    public override void GLUniform2Iv( int location, int count, IntBuffer v )
    {
        Calls++;
        GL30.GLUniform2Iv( location, count, v );
        Check();
    }

    public override void GLUniform2Iv( int location, int count, int[] v, int offset )
    {
        Calls++;
        GL30.GLUniform2Iv( location, count, v, offset );
        Check();
    }

    public override void GLUniform3F( int location, float x, float y, float z )
    {
        Calls++;
        GL30.GLUniform3F( location, x, y, z );
        Check();
    }

    public override void GLUniform3Fv( int location, int count, FloatBuffer v )
    {
        Calls++;
        GL30.GLUniform3Fv( location, count, v );
        Check();
    }

    public override void GLUniform3Fv( int location, int count, float[] v, int offset )
    {
        Calls++;
        GL30.GLUniform3Fv( location, count, v, offset );
        Check();
    }

    public override void GLUniform3I( int location, int x, int y, int z )
    {
        Calls++;
        GL30.GLUniform3I( location, x, y, z );
        Check();
    }

    public override void GLUniform3Iv( int location, int count, IntBuffer v )
    {
        Calls++;
        GL30.GLUniform3Iv( location, count, v );
        Check();
    }

    public override void GLUniform3Iv( int location, int count, int[] v, int offset )
    {
        Calls++;
        GL30.GLUniform3Iv( location, count, v, offset );
        Check();
    }

    public override void GLUniform4F( int location, float x, float y, float z, float w )
    {
        Calls++;
        GL30.GLUniform4F( location, x, y, z, w );
        Check();
    }

    public override void GLUniform4Fv( int location, int count, FloatBuffer v )
    {
        Calls++;
        GL30.GLUniform4Fv( location, count, v );
        Check();
    }

    public override void GLUniform4Fv( int location, int count, float[] v, int offset )
    {
        Calls++;
        GL30.GLUniform4Fv( location, count, v, offset );
        Check();
    }

    public override void GLUniform4I( int location, int x, int y, int z, int w )
    {
        Calls++;
        GL30.GLUniform4I( location, x, y, z, w );
        Check();
    }

    public override void GLUniform4Iv( int location, int count, IntBuffer v )
    {
        Calls++;
        GL30.GLUniform4Iv( location, count, v );
        Check();
    }

    public override void GLUniform4Iv( int location, int count, int[] v, int offset )
    {
        Calls++;
        GL30.GLUniform4Iv( location, count, v, offset );
        Check();
    }

    public override void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix2Fv( location, count, transpose, value );
        Check();
    }

    public override void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset )
    {
        Calls++;
        GL30.GLUniformMatrix2Fv( location, count, transpose, value, offset );
        Check();
    }

    public override void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix3Fv( location, count, transpose, value );
        Check();
    }

    public override void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset )
    {
        Calls++;
        GL30.GLUniformMatrix3Fv( location, count, transpose, value, offset );
        Check();
    }

    public override void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix4Fv( location, count, transpose, value );
        Check();
    }

    public override void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
    {
        Calls++;
        GL30.GLUniformMatrix4Fv( location, count, transpose, value, offset );
        Check();
    }

    public override void GLUseProgram( int program )
    {
        ShaderSwitches++;
        Calls++;
        GL30.GLUseProgram( program );
        Check();
    }

    public override void GLValidateProgram( int program )
    {
        Calls++;
        GL30.GLValidateProgram( program );
        Check();
    }

    public override void GLVertexAttrib1F( int indx, float x )
    {
        Calls++;
        GL30.GLVertexAttrib1F( indx, x );
        Check();
    }

    public override void GLVertexAttrib1Fv( int indx, FloatBuffer values )
    {
        Calls++;
        GL30.GLVertexAttrib1Fv( indx, values );
        Check();
    }

    public override void GLVertexAttrib2F( int indx, float x, float y )
    {
        Calls++;
        GL30.GLVertexAttrib2F( indx, x, y );
        Check();
    }

    public override void GLVertexAttrib2Fv( int indx, FloatBuffer values )
    {
        Calls++;
        GL30.GLVertexAttrib2Fv( indx, values );
        Check();
    }

    public override void GLVertexAttrib3F( int indx, float x, float y, float z )
    {
        Calls++;
        GL30.GLVertexAttrib3F( indx, x, y, z );
        Check();
    }

    public override void GLVertexAttrib3Fv( int indx, FloatBuffer values )
    {
        Calls++;
        GL30.GLVertexAttrib3Fv( indx, values );
        Check();
    }

    public override void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
    {
        Calls++;
        GL30.GLVertexAttrib4F( indx, x, y, z, w );
        Check();
    }

    public override void GLVertexAttrib4Fv( int indx, FloatBuffer values )
    {
        Calls++;
        GL30.GLVertexAttrib4Fv( indx, values );
        Check();
    }

    public override void GLVertexAttribPointer( int indx, int size, VertexAttribType type, bool normalized, int stride, Buffer ptr )
    {
        Calls++;
        GL30.GLVertexAttribPointer( indx, size, type, normalized, stride, ptr );
        Check();
    }

    public override void GLVertexAttribPointer( int indx, int size, VertexAttribType type, bool normalized, int stride, int ptr )
    {
        Calls++;
        GL30.GLVertexAttribPointer( indx, size, type, normalized, stride, ptr );
        Check();
    }

    // GL30 Unique

    public void GLReadBuffer( int mode )
    {
        Calls++;
        GL30.GLReadBuffer( mode );
        Check();
    }

    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, Buffer indices )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL30.GLDrawRangeElements( mode, start, end, count, type, indices );
        Check();
    }

    public void GLDrawRangeElements( int mode, int start, int end, int count, int type, int offset )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL30.GLDrawRangeElements( mode, start, end, count, type, offset );
        Check();
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
        Calls++;
        GL30.GLTexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
        Check();
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
        Calls++;
        GL30.GLTexImage3D( target, level, internalformat, width, height, depth, border, format, type, offset );
        Check();
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
        Calls++;
        GL30.GLTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        Check();
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
        Calls++;
        GL30.GLTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, offset );
        Check();
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
        Calls++;
        GL30.GLCopyTexSubImage3D( target, level, xoffset, yoffset, zoffset, x, y, width, height );
        Check();
    }

    public void GLGenQueries( int n, int[] ids, int offset )
    {
        Calls++;
        GL30.GLGenQueries( n, ids, offset );
        Check();
    }

    public void GLGenQueries( int n, IntBuffer ids )
    {
        Calls++;
        GL30.GLGenQueries( n, ids );
        Check();
    }

    public void GLDeleteQueries( int n, int[] ids, int offset )
    {
        Calls++;
        GL30.GLDeleteQueries( n, ids, offset );
        Check();
    }

    public void GLDeleteQueries( int n, IntBuffer ids )
    {
        Calls++;
        GL30.GLDeleteQueries( n, ids );
        Check();
    }

    public bool GLIsQuery( int id )
    {
        Calls++;
        var result = GL30.GLIsQuery( id );
        Check();

        return result;
    }

    public void GLBeginQuery( int target, int id )
    {
        Calls++;
        GL30.GLBeginQuery( target, id );
        Check();
    }

    public void GLEndQuery( int target )
    {
        Calls++;
        GL30.GLEndQuery( target );
        Check();
    }

    public void GLGetQueryiv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetQueryiv( target, pname, parameters );
        Check();
    }

    public void GLGetQueryObjectuiv( int id, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetQueryObjectuiv( id, pname, parameters );
        Check();
    }


    public bool GLUnmapBuffer( int target )
    {
        Calls++;
        var result = GL30.GLUnmapBuffer( target );
        Check();

        return result;
    }


    public Buffer GLGetBufferPointerv( int target, int pname )
    {
        Calls++;
        Buffer result = GL30.GLGetBufferPointerv( target, pname );
        Check();

        return result;
    }


    public void GLDrawBuffers( int n, IntBuffer bufs )
    {
        DrawCalls++;
        Calls++;
        GL30.GLDrawBuffers( n, bufs );
        Check();
    }


    public void GLUniformMatrix2X3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix2X3Fv( location, count, transpose, value );
        Check();
    }


    public void GLUniformMatrix3X2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix3X2Fv( location, count, transpose, value );
        Check();
    }


    public void GLUniformMatrix2X4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix2X4Fv( location, count, transpose, value );
        Check();
    }


    public void GLUniformMatrix4X2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix4X2Fv( location, count, transpose, value );
        Check();
    }


    public void GLUniformMatrix3X4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix3X4Fv( location, count, transpose, value );
        Check();
    }


    public void GLUniformMatrix4X3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL30.GLUniformMatrix4X3Fv( location, count, transpose, value );
        Check();
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
        Calls++;
        GL30.GLBlitFramebuffer( srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
        Check();
    }


    public void GLRenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height )
    {
        Calls++;
        GL30.GLRenderbufferStorageMultisample( target, samples, internalformat, width, height );
        Check();
    }


    public void GLFramebufferTextureLayer( int target, int attachment, int texture, int level, int layer )
    {
        Calls++;
        GL30.GLFramebufferTextureLayer( target, attachment, texture, level, layer );
        Check();
    }


    public Buffer GLMapBufferRange( int target, int offset, int length, int access )
    {
        Calls++;
        Buffer result = GL30.GLMapBufferRange( target, offset, length, access );
        Check();

        return result;
    }


    public void GLFlushMappedBufferRange( int target, int offset, int length )
    {
        Calls++;
        GL30.GLFlushMappedBufferRange( target, offset, length );
        Check();
    }


    public void GLBindVertexArray( int array )
    {
        Calls++;
        GL30.GLBindVertexArray( array );
        Check();
    }


    public void GLDeleteVertexArrays( int n, int[] arrays, int offset )
    {
        Calls++;
        GL30.GLDeleteVertexArrays( n, arrays, offset );
        Check();
    }


    public void GLDeleteVertexArrays( int n, IntBuffer arrays )
    {
        Calls++;
        GL30.GLDeleteVertexArrays( n, arrays );
        Check();
    }


    public void GLGenVertexArrays( int n, int[] arrays, int offset )
    {
        Calls++;
        GL30.GLGenVertexArrays( n, arrays, offset );
        Check();
    }


    public void GLGenVertexArrays( int n, IntBuffer arrays )
    {
        Calls++;
        GL30.GLGenVertexArrays( n, arrays );
        Check();
    }


    public bool GLIsVertexArray( int array )
    {
        Calls++;
        var result = GL30.GLIsVertexArray( array );
        Check();

        return result;
    }


    public void GLBeginTransformFeedback( int primitiveMode )
    {
        Calls++;
        GL30.GLBeginTransformFeedback( primitiveMode );
        Check();
    }


    public void GLEndTransformFeedback()
    {
        Calls++;
        GL30.GLEndTransformFeedback();
        Check();
    }


    public void GLBindBufferRange( int target, int index, int buffer, int offset, int size )
    {
        Calls++;
        GL30.GLBindBufferRange( target, index, buffer, offset, size );
        Check();
    }


    public void GLBindBufferBase( int target, int index, int buffer )
    {
        Calls++;
        GL30.GLBindBufferBase( target, index, buffer );
        Check();
    }


    public void GLTransformFeedbackVaryings( int program, string[] varyings, int bufferMode )
    {
        Calls++;
        GL30.GLTransformFeedbackVaryings( program, varyings, bufferMode );
        Check();
    }


    public void GLVertexAttribIPointer( int index, int size, int type, int stride, int offset )
    {
        Calls++;
        GL30.GLVertexAttribIPointer( index, size, type, stride, offset );
        Check();
    }


    public void GLGetVertexAttribIiv( int index, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetVertexAttribIiv( index, pname, parameters );
        Check();
    }


    public void GLGetVertexAttribIuiv( int index, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetVertexAttribIuiv( index, pname, parameters );
        Check();
    }


    public void GLVertexAttribI4I( int index, int x, int y, int z, int w )
    {
        Calls++;
        GL30.GLVertexAttribI4I( index, x, y, z, w );
        Check();
    }


    public void GLVertexAttribI4Ui( int index, int x, int y, int z, int w )
    {
        Calls++;
        GL30.GLVertexAttribI4Ui( index, x, y, z, w );
        Check();
    }


    public void GLGetUniformuiv( int program, int location, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetUniformuiv( program, location, parameters );
        Check();
    }


    public int GLGetFragDataLocation( int program, string name )
    {
        Calls++;
        var result = GL30.GLGetFragDataLocation( program, name );
        Check();

        return result;
    }


    public void GLUniform1Uiv( int location, int count, IntBuffer value )
    {
        Calls++;
        GL30.GLUniform1Uiv( location, count, value );
        Check();
    }


    public void GLUniform3Uiv( int location, int count, IntBuffer value )
    {
        Calls++;
        GL30.GLUniform3Uiv( location, count, value );
        Check();
    }


    public void GLUniform4Uiv( int location, int count, IntBuffer value )
    {
        Calls++;
        GL30.GLUniform4Uiv( location, count, value );
        Check();
    }


    public void GLClearBufferiv( int buffer, int drawbuffer, IntBuffer value )
    {
        Calls++;
        GL30.GLClearBufferiv( buffer, drawbuffer, value );
        Check();
    }


    public void GLClearBufferuiv( int buffer, int drawbuffer, IntBuffer value )
    {
        Calls++;
        GL30.GLClearBufferuiv( buffer, drawbuffer, value );
        Check();
    }


    public void GLClearBufferfv( int buffer, int drawbuffer, FloatBuffer value )
    {
        Calls++;
        GL30.GLClearBufferfv( buffer, drawbuffer, value );
        Check();
    }


    public void GLClearBufferfi( int buffer, int drawbuffer, float depth, int stencil )
    {
        Calls++;
        GL30.GLClearBufferfi( buffer, drawbuffer, depth, stencil );
        Check();
    }


    public string GLGetStringi( int name, int index )
    {
        Calls++;
        var result = GL30.GLGetStringi( name, index );
        Check();

        return result;
    }


    public void GLCopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size )
    {
        Calls++;
        GL30.GLCopyBufferSubData( readTarget, writeTarget, readOffset, writeOffset, size );
        Check();
    }


    public void GLGetUniformIndices( int program, string[] uniformNames, IntBuffer uniformIndices )
    {
        Calls++;
        GL30.GLGetUniformIndices( program, uniformNames, uniformIndices );
        Check();
    }


    public void GLGetActiveUniformsiv( int program, int uniformCount, IntBuffer uniformIndices, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetActiveUniformsiv( program, uniformCount, uniformIndices, pname, parameters );
        Check();
    }


    public int GLGetUniformBlockIndex( int program, string uniformBlockName )
    {
        Calls++;
        var result = GL30.GLGetUniformBlockIndex( program, uniformBlockName );
        Check();

        return result;
    }


    public void GLGetActiveUniformBlockiv( int program, int uniformBlockIndex, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetActiveUniformBlockiv( program, uniformBlockIndex, pname, parameters );
        Check();
    }


    public void GLGetActiveUniformBlockName( int program, int uniformBlockIndex, Buffer length, Buffer uniformBlockName )
    {
        Calls++;
        GL30.GLGetActiveUniformBlockName( program, uniformBlockIndex, length, uniformBlockName );
        Check();
    }


    public string GLGetActiveUniformBlockName( int program, int uniformBlockIndex )
    {
        Calls++;
        var result = GL30.GLGetActiveUniformBlockName( program, uniformBlockIndex );
        Check();

        return result;
    }


    public void GLUniformBlockBinding( int program, int uniformBlockIndex, int uniformBlockBinding )
    {
        Calls++;
        GL30.GLUniformBlockBinding( program, uniformBlockIndex, uniformBlockBinding );
        Check();
    }


    public void GLDrawArraysInstanced( PrimitiveType mode, int first, int count, int instanceCount )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL30.GLDrawArraysInstanced( mode, first, count, instanceCount );
        Check();
    }

    public void GLDrawElementsInstanced( PrimitiveType mode, int count, int type, int indicesOffset, int instanceCount )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL30.GLDrawElementsInstanced( mode, count, type, indicesOffset, instanceCount );
        Check();
    }

    public void GLGetInteger64V( int pname, LongBuffer parameters )
    {
        Calls++;
        GL30.GLGetInteger64V( pname, parameters );
        Check();
    }


    public void GLGetBufferParameteri64V( int target, int pname, LongBuffer parameters )
    {
        Calls++;
        GL30.GLGetBufferParameteri64V( target, pname, parameters );
        Check();
    }

    public void GLGenSamplers( int count, int[] samplers, int offset )
    {
        Calls++;
        GL30.GLGenSamplers( count, samplers, offset );
        Check();
    }

    public void GLGenSamplers( int count, IntBuffer samplers )
    {
        Calls++;
        GL30.GLGenSamplers( count, samplers );
        Check();
    }

    public void GLDeleteSamplers( int count, int[] samplers, int offset )
    {
        Calls++;
        GL30.GLDeleteSamplers( count, samplers, offset );
        Check();
    }

    public void GLDeleteSamplers( int count, IntBuffer samplers )
    {
        Calls++;
        GL30.GLDeleteSamplers( count, samplers );
        Check();
    }

    public bool GLIsSampler( int sampler )
    {
        Calls++;
        var result = GL30.GLIsSampler( sampler );
        Check();

        return result;
    }

    public void GLBindSampler( int unit, int sampler )
    {
        Calls++;
        GL30.GLBindSampler( unit, sampler );
        Check();
    }

    public void GLSamplerParameteri( int sampler, int pname, int param )
    {
        Calls++;
        GL30.GLSamplerParameteri( sampler, pname, param );
        Check();
    }

    public void GLSamplerParameteriv( int sampler, int pname, IntBuffer param )
    {
        Calls++;
        GL30.GLSamplerParameteriv( sampler, pname, param );
        Check();
    }

    public void GLSamplerParameterf( int sampler, int pname, float param )
    {
        Calls++;
        GL30.GLSamplerParameterf( sampler, pname, param );
        Check();
    }

    public void GLSamplerParameterfv( int sampler, int pname, FloatBuffer param )
    {
        Calls++;
        GL30.GLSamplerParameterfv( sampler, pname, param );
        Check();
    }

    public void GLGetSamplerParameteriv( int sampler, int pname, IntBuffer parameters )
    {
        Calls++;
        GL30.GLGetSamplerParameteriv( sampler, pname, parameters );
        Check();
    }

    public void GLGetSamplerParameterfv( int sampler, int pname, FloatBuffer parameters )
    {
        Calls++;
        GL30.GLGetSamplerParameterfv( sampler, pname, parameters );
        Check();
    }

    public void GLVertexAttribDivisor( int index, int divisor )
    {
        Calls++;
        GL30.GLVertexAttribDivisor( index, divisor );
        Check();
    }

    public void GLBindTransformFeedback( int target, int id )
    {
        Calls++;
        GL30.GLBindTransformFeedback( target, id );
        Check();
    }

    public void GLDeleteTransformFeedbacks( int n, int[] ids, int offset )
    {
        Calls++;
        GL30.GLDeleteTransformFeedbacks( n, ids, offset );
        Check();
    }

    public void GLDeleteTransformFeedbacks( int n, IntBuffer ids )
    {
        Calls++;
        GL30.GLDeleteTransformFeedbacks( n, ids );
        Check();
    }

    public void GLGenTransformFeedbacks( int n, int[] ids, int offset )
    {
        Calls++;
        GL30.GLGenTransformFeedbacks( n, ids, offset );
        Check();
    }

    public void GLGenTransformFeedbacks( int n, IntBuffer ids )
    {
        Calls++;
        GL30.GLGenTransformFeedbacks( n, ids );
        Check();
    }

    public bool GLIsTransformFeedback( int id )
    {
        Calls++;
        var result = GL30.GLIsTransformFeedback( id );
        Check();

        return result;
    }

    public void GLPauseTransformFeedback()
    {
        Calls++;
        GL30.GLPauseTransformFeedback();
        Check();
    }

    public void GLResumeTransformFeedback()
    {
        Calls++;
        GL30.GLResumeTransformFeedback();
        Check();
    }

    public void GLProgramParameteri( int program, int pname, int value )
    {
        Calls++;
        GL30.GLProgramParameteri( program, pname, value );
        Check();
    }

    public void GLInvalidateFramebuffer( int target, int numAttachments, IntBuffer attachments )
    {
        Calls++;
        GL30.GLInvalidateFramebuffer( target, numAttachments, attachments );
        Check();
    }

    public void GLInvalidateSubFramebuffer( int target,
                                            int numAttachments,
                                            IntBuffer attachments,
                                            int x,
                                            int y,
                                            int width,
                                            int height )
    {
        Calls++;
        GL30.GLInvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
        Check();
    }
}
