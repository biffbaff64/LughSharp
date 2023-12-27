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

using LibGDXSharp.Files.Buffers;

using Buffer = LibGDXSharp.Files.Buffers.Buffer;
using ErrorCode = OpenGL.ErrorCode;

namespace LibGDXSharp.Graphics.Profiling;

[PublicAPI]
public class GL20Interceptor : GLInterceptor, IGL20
{
    public IGL20 GL20 { get; set; }

    public GL20Interceptor( GLProfiler glProfiler, IGL20 gl20 )
        : base( glProfiler )
    {
        this.GL20 = gl20;
    }

    private void Check()
    {
        ErrorCode error = GL20.GLGetError();

        while ( error != IGL20.GL_NO_ERROR )
        {
            glProfiler.Listener.OnError( error );
            error = GL20.GLGetError();
        }
    }

    public override void GLActiveTexture( int texture )
    {
        Calls++;
        GL20.GLActiveTexture( texture );
        Check();
    }

    public override void GLBindTexture( int target, int texture )
    {
        TextureBindings++;
        Calls++;
        GL20.GLBindTexture( target, texture );
        Check();
    }

    public override void GLBlendFunc( int sfactor, int dfactor )
    {
        Calls++;
        GL20.GLBlendFunc( sfactor, dfactor );
        Check();
    }

    public override void GLClear( int mask )
    {
        Calls++;
        GL20.GLClear( mask );
        Check();
    }

    public override void GLClearColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        GL20.GLClearColor( red, green, blue, alpha );
        Check();
    }

    public override void GLClearDepthf( float depth )
    {
        Calls++;
        GL20.GLClearDepthf( depth );
        Check();
    }

    public override void GLClearStencil( int s )
    {
        Calls++;
        GL20.GLClearStencil( s );
        Check();
    }

    public override void GLColorMask( bool red, bool green, bool blue, bool alpha )
    {
        Calls++;
        GL20.GLColorMask( red, green, blue, alpha );
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
        GL20.GLCompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, data );
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
        GL20.GLCompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
        Check();
    }

    public override void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border )
    {
        Calls++;
        GL20.GLCopyTexImage2D( target, level, internalformat, x, y, width, height, border );
        Check();
    }

    public override void GLCopyTexSubImage2D( TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Calls++;
        GL20.GLCopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
        Check();
    }

    public override void GLCullFace( CullFaceMode mode )
    {
        Calls++;
        GL20.GLCullFace( mode );
        Check();
    }

    public override void GLDeleteTextures( int n, int textures )
    {
        Calls++;
        GL20.GLDeleteTextures( n, textures );
        Check();
    }

    public override void GLDeleteTexture( int texture )
    {
        Calls++;
        GL20.GLDeleteTexture( texture );
        Check();
    }

    public override void GLDepthFunc( DepthFunction func )
    {
        Calls++;
        GL20.GLDepthFunc( func );
        Check();
    }

    public override void GLDepthMask( bool flag )
    {
        Calls++;
        GL20.GLDepthMask( flag );
        Check();
    }

    public override void GLDepthRangef( float zNear, float zFar )
    {
        Calls++;
        GL20.GLDepthRangef( zNear, zFar );
        Check();
    }

    public override void GLDisable( EnableCap cap )
    {
        Calls++;
        GL20.GLDisable( cap );
        Check();
    }

    public override void GLDrawArrays( PrimitiveType mode, int first, int count )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL20.GLDrawArrays( mode, first, count );
        Check();
    }

    public override void GLDrawElements( PrimitiveType mode, int count, DrawElementsType type, Buffer indices )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL20.GLDrawElements( mode, count, type, indices );
        Check();
    }

    public override void GLEnable( EnableCap cap )
    {
        Calls++;
        GL20.GLEnable( cap );
        Check();
    }

    public override void GLFinish()
    {
        Calls++;
        GL20.GLFinish();
        Check();
    }

    public override void GLFlush()
    {
        Calls++;
        GL20.GLFlush();
        Check();
    }

    public override void GLFrontFace( FrontFaceDirection mode )
    {
        Calls++;
        GL20.GLFrontFace( mode );
        Check();
    }

    public override void GLGenTextures( int n, int[] textures )
    {
        Calls++;
        GL20.GLGenTextures( n, textures );
        Check();
    }

    public override int GLGenTexture()
    {
        Calls++;
        var result = GL20.GLGenTexture();
        Check();

        return result;
    }

    public override ErrorCode GLGetError()
    {
        Calls++;

        return GL20.GLGetError();
    }

    public override void GLGetIntegerv( int pname, IntBuffer parameters )
    {
    }

    public override string GLGetString( int name )
    {
        Calls++;
        var result = GL20.GLGetString( name );
        Check();

        return result;
    }

    public override void GLHint( int target, int mode )
    {
        Calls++;
        GL20.GLHint( target, mode );
        Check();
    }

    public override void GLLineWidth( float width )
    {
        Calls++;
        GL20.GLLineWidth( width );
        Check();
    }

    public override void GLPixelStorei( int pname, int param )
    {
        Calls++;
        GL20.GLPixelStorei( pname, param );
        Check();
    }

    public override void GLPolygonOffset( float factor, float units )
    {
        Calls++;
        GL20.GLPolygonOffset( factor, units );
        Check();
    }

    public override void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels )
    {
        Calls++;
        GL20.GLReadPixels( x, y, width, height, format, type, pixels );
        Check();
    }

    public override void GLScissor( int x, int y, int width, int height )
    {
        Calls++;
        GL20.GLScissor( x, y, width, height );
        Check();
    }

    public override void GLStencilFunc( int func, int reference, int mask )
    {
        Calls++;
        GL20.GLStencilFunc( func, reference, mask );
        Check();
    }

    public override void GLStencilMask( int mask )
    {
        Calls++;
        GL20.GLStencilMask( mask );
        Check();
    }

    public override void GLStencilOp( int fail, int zfail, int zpass )
    {
        Calls++;
        GL20.GLStencilOp( fail, zfail, zpass );
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
        GL20.GLTexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        Check();
    }

    public override void GLTexParameterf( int target, int pname, float param )
    {
        Calls++;
        GL20.GLTexParameterf( target, pname, param );
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
        GL20.GLTexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
        Check();
    }

    public override void GLViewport( int x, int y, int width, int height )
    {
        Calls++;
        GL20.GLViewport( x, y, width, height );
        Check();
    }

    public override void GLAttachShader( int program, int shader )
    {
        Calls++;
        GL20.GLAttachShader( program, shader );
        Check();
    }

    public override void GLBindAttribLocation( int program, int index, string name )
    {
        Calls++;
        GL20.GLBindAttribLocation( program, index, name );
        Check();
    }

    public override void GLBindBuffer( int target, int buffer )
    {
        Calls++;
        GL20.GLBindBuffer( target, buffer );
        Check();
    }

    public override void GLBindFramebuffer( int target, int framebuffer )
    {
        Calls++;
        GL20.GLBindFramebuffer( target, framebuffer );
        Check();
    }

    public override void GLBindRenderbuffer( int target, int renderbuffer )
    {
        Calls++;
        GL20.GLBindRenderbuffer( target, renderbuffer );
        Check();
    }

    public override void GLBlendColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        GL20.GLBlendColor( red, green, blue, alpha );
        Check();
    }

    public override void GLBlendEquation( int mode )
    {
        Calls++;
        GL20.GLBlendEquation( mode );
        Check();
    }

    public override void GLBlendEquationSeparate( int modeRgb, int modeAlpha )
    {
        Calls++;
        GL20.GLBlendEquationSeparate( modeRgb, modeAlpha );
        Check();
    }

    public override void GLBlendFuncSeparate( int srcRgb, int dstRgb, int srcAlpha, int dstAlpha )
    {
        Calls++;
        GL20.GLBlendFuncSeparate( srcRgb, dstRgb, srcAlpha, dstAlpha );
        Check();
    }

    public override void GLBufferData( int target, int size, Buffer data, int usage )
    {
        Calls++;
        GL20.GLBufferData( target, size, data, usage );
        Check();
    }

    public override void GLBufferSubData( int target, int offset, int size, Buffer data )
    {
        Calls++;
        GL20.GLBufferSubData( target, offset, size, data );
        Check();
    }

    public override int GLCheckFramebufferStatus( int target )
    {
        Calls++;
        var result = GL20.GLCheckFramebufferStatus( target );
        Check();

        return result;
    }

    public override void GLCompileShader( int shader )
    {
        Calls++;
        GL20.GLCompileShader( shader );
        Check();
    }

    public override int GLCreateProgram()
    {
        Calls++;
        var result = GL20.GLCreateProgram();
        Check();

        return result;
    }

    public override int GLCreateShader( ShaderType type )
    {
        Calls++;
        var result = GL20.GLCreateShader( type );
        Check();

        return result;
    }

    public override void GLDeleteBuffer( int buffer )
    {
        Calls++;
        GL20.GLDeleteBuffer( buffer );
        Check();
    }

    public override void GLDeleteBuffers( int n, int buffers )
    {
        Calls++;
        GL20.GLDeleteBuffers( n, buffers );
        Check();
    }

    public override void GLDeleteFramebuffer( int framebuffer )
    {
        Calls++;
        GL20.GLDeleteFramebuffer( framebuffer );
        Check();
    }

    public override void GLDeleteFramebuffers( int n, IntBuffer framebuffers )
    {
        Calls++;
        GL20.GLDeleteFramebuffers( n, framebuffers );
        Check();
    }

    public override void GLDeleteProgram( int program )
    {
        Calls++;
        GL20.GLDeleteProgram( program );
        Check();
    }

    public override void GLDeleteRenderbuffer( int renderbuffer )
    {
        Calls++;
        GL20.GLDeleteRenderbuffer( renderbuffer );
        Check();
    }

    public override void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers )
    {
        Calls++;
        GL20.GLDeleteRenderbuffers( n, renderbuffers );
        Check();
    }

    public override void GLDeleteShader( int shader )
    {
        Calls++;
        GL20.GLDeleteShader( shader );
        Check();
    }

    public override void GLDetachShader( int program, int shader )
    {
        Calls++;
        GL20.GLDetachShader( program, shader );
        Check();
    }

    public override void GLDisableVertexAttribArray( int index )
    {
        Calls++;
        GL20.GLDisableVertexAttribArray( index );
        Check();
    }

    public override void GLDrawElements( PrimitiveType mode, int count, DrawElementsType type, int indices )
    {
        VertexCount.Put( count );
        DrawCalls++;
        Calls++;
        GL20.GLDrawElements( mode, count, type, indices );
        Check();
    }

    public override void GLEnableVertexAttribArray( int index )
    {
        Calls++;
        GL20.GLEnableVertexAttribArray( index );
        Check();
    }

    public override void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer )
    {
        Calls++;
        GL20.GLFramebufferRenderbuffer( target, attachment, renderbuffertarget, renderbuffer );
        Check();
    }

    public override void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level )
    {
        Calls++;
        GL20.GLFramebufferTexture2D( target, attachment, textarget, texture, level );
        Check();
    }

    public override int GLGenBuffer()
    {
        Calls++;
        var result = GL20.GLGenBuffer();
        Check();

        return result;
    }

    public override void GLGenBuffers( int n, int[] buffers )
    {
        Calls++;
        GL20.GLGenBuffers( n, buffers );
        Check();
    }

    public override void GLGenerateMipmap( int target )
    {
        Calls++;
        GL20.GLGenerateMipmap( target );
        Check();
    }

    public override int GLGenFramebuffer()
    {
        Calls++;
        var result = GL20.GLGenFramebuffer();
        Check();

        return result;
    }

    public override void GLGenFramebuffers( int n, IntBuffer framebuffers )
    {
        Calls++;
        GL20.GLGenFramebuffers( n, framebuffers );
        Check();
    }

    public override int GLGenRenderbuffer()
    {
        Calls++;
        var result = GL20.GLGenFramebuffer();
        Check();

        return result;
    }

    public override void GLGenRenderbuffers( int n, IntBuffer renderbuffers )
    {
        Calls++;
        GL20.GLGenRenderbuffers( n, renderbuffers );
        Check();
    }

    public override string GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type )
    {
        Calls++;
        var result = GL20.GLGetActiveAttrib( program, index, size, type );
        Check();

        return result;
    }

    public override string GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type )
    {
        Calls++;
        var result = GL20.GLGetActiveUniform( program, index, size, type );
        Check();

        return result;
    }

    public override void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders )
    {
        Calls++;
        GL20.GLGetAttachedShaders( program, maxcount, count, shaders );
        Check();
    }

    public override int GLGetAttribLocation( int program, string name )
    {
        Calls++;
        var result = GL20.GLGetAttribLocation( program, name );
        Check();

        return result;
    }

    public override void GLGetBooleanv( int pname, Buffer parameters )
    {
        Calls++;
        GL20.GLGetBooleanv( pname, parameters );
        Check();
    }

    public override void GLGetBufferParameteriv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL20.GLGetBufferParameteriv( target, pname, parameters );
        Check();
    }

    public override void GLGetFloatv( int pname, FloatBuffer parameters )
    {
        Calls++;
        GL20.GLGetFloatv( pname, parameters );
        Check();
    }

    public override void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer parameters )
    {
        Calls++;
        GL20.GLGetFramebufferAttachmentParameteriv( target, attachment, pname, parameters );
        Check();
    }

    public override void GLGetProgramiv( int program, int pname, IntBuffer parameters )
    {
        Calls++;
        GL20.GLGetProgramiv( program, pname, parameters );
        Check();
    }

    public override string GLGetProgramInfoLog( int program )
    {
        Calls++;
        var result = GL20.GLGetProgramInfoLog( program );
        Check();

        return result;
    }

    public override void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL20.GLGetRenderbufferParameteriv( target, pname, parameters );
        Check();
    }

    public override void GLGetShaderiv( int shader, int pname, IntBuffer parameters )
    {
        Calls++;
        GL20.GLGetShaderiv( shader, pname, parameters );
        Check();
    }

    public override string GLGetShaderInfoLog( int shader )
    {
        Calls++;
        var result = GL20.GLGetShaderInfoLog( shader );
        Check();

        return result;
    }

    public override void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision )
    {
        Calls++;
        GL20.GLGetShaderPrecisionFormat( shadertype, precisiontype, range, precision );
        Check();
    }

    public override void GLGetTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
        Calls++;
        GL20.GLGetTexParameterfv( target, pname, parameters );
        Check();
    }

    public override void GLGetTexParameteriv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL20.GLGetTexParameteriv( target, pname, parameters );
        Check();
    }

    public override void GLGetUniformfv( int program, int location, FloatBuffer parameters )
    {
        Calls++;
        GL20.GLGetUniformfv( program, location, parameters );
        Check();
    }

    public override void GLGetUniformiv( int program, int location, IntBuffer parameters )
    {
        Calls++;
        GL20.GLGetUniformiv( program, location, parameters );
        Check();
    }

    public override int GLGetUniformLocation( int program, string name )
    {
        Calls++;
        var result = GL20.GLGetUniformLocation( program, name );
        Check();

        return result;
    }

    public override void GLGetVertexAttribfv( int index, int pname, FloatBuffer parameters )
    {
        Calls++;
        GL20.GLGetVertexAttribfv( index, pname, parameters );
        Check();
    }

    public override void GLGetVertexAttribiv( int index, int pname, IntBuffer parameters )
    {
        Calls++;
        GL20.GLGetVertexAttribiv( index, pname, parameters );
        Check();
    }

    public override void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer )
    {
        Calls++;
        GL20.GLGetVertexAttribPointerv( index, pname, pointer );
        Check();
    }

    public override bool GLIsBuffer( int buffer )
    {
        Calls++;
        var result = GL20.GLIsBuffer( buffer );
        Check();

        return result;
    }

    public override bool GLIsEnabled( int cap )
    {
        Calls++;
        var result = GL20.GLIsEnabled( cap );
        Check();

        return result;
    }

    public override bool GLIsFramebuffer( int framebuffer )
    {
        Calls++;
        var result = GL20.GLIsFramebuffer( framebuffer );
        Check();

        return result;
    }

    public override bool GLIsProgram( int program )
    {
        Calls++;
        var result = GL20.GLIsProgram( program );
        Check();

        return result;
    }

    public override bool GLIsRenderbuffer( int renderbuffer )
    {
        Calls++;
        var result = GL20.GLIsRenderbuffer( renderbuffer );
        Check();

        return result;
    }

    public override bool GLIsShader( int shader )
    {
        Calls++;
        var result = GL20.GLIsShader( shader );
        Check();

        return result;
    }

    public override bool GLIsTexture( int texture )
    {
        Calls++;
        var result = GL20.GLIsTexture( texture );
        Check();

        return result;
    }

    public override void GLLinkProgram( int program )
    {
        Calls++;
        GL20.GLLinkProgram( program );
        Check();
    }

    public override void GLReleaseShaderCompiler()
    {
        Calls++;
        GL20.GLReleaseShaderCompiler();
        Check();
    }

    public override void GLRenderbufferStorage( int target, int internalformat, int width, int height )
    {
        Calls++;
        GL20.GLRenderbufferStorage( target, internalformat, width, height );
        Check();
    }

    public override void GLSampleCoverage( float value, bool invert )
    {
        Calls++;
        GL20.GLSampleCoverage( value, invert );
        Check();
    }

    public override void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length )
    {
        Calls++;
        GL20.GLShaderBinary( n, shaders, binaryformat, binary, length );
        Check();
    }

    public override void GLShaderSource( int shader, string str )
    {
        Calls++;
        GL20.GLShaderSource( shader, str );
        Check();
    }

    public override void GLStencilFuncSeparate( int face, int func, int reference, int mask )
    {
        Calls++;
        GL20.GLStencilFuncSeparate( face, func, reference, mask );
        Check();
    }

    public override void GLStencilMaskSeparate( int face, int mask )
    {
        Calls++;
        GL20.GLStencilMaskSeparate( face, mask );
        Check();
    }

    public override void GLStencilOpSeparate( int face, int fail, int zfail, int zpass )
    {
        Calls++;
        GL20.GLStencilOpSeparate( face, fail, zfail, zpass );
        Check();
    }

    public override void GLTexParameterfv( int target, int pname, FloatBuffer parameters )
    {
        Calls++;
        GL20.GLTexParameterfv( target, pname, parameters );
        Check();
    }

    public override void GLTexParameteri( int target, int pname, int param )
    {
        Calls++;
        GL20.GLTexParameteri( target, pname, param );
        Check();
    }

    public override void GLTexParameteriv( int target, int pname, IntBuffer parameters )
    {
        Calls++;
        GL20.GLTexParameteriv( target, pname, parameters );
        Check();
    }

    public override void GLUniform1F( int location, float x )
    {
        Calls++;
        GL20.GLUniform1F( location, x );
        Check();
    }

    public override void GLUniform1Fv( int location, int count, FloatBuffer v )
    {
        Calls++;
        GL20.GLUniform1Fv( location, count, v );
        Check();
    }

    public override void GLUniform1Fv( int location, int count, float[] v, int offset )
    {
        Calls++;
        GL20.GLUniform1Fv( location, count, v, offset );
        Check();
    }

    public override void GLUniform1I( int location, int x )
    {
        Calls++;
        GL20.GLUniform1I( location, x );
        Check();
    }

    public override void GLUniform1Iv( int location, int count, IntBuffer v )
    {
        Calls++;
        GL20.GLUniform1Iv( location, count, v );
        Check();
    }

    public override void GLUniform1Iv( int location, int count, int[] v, int offset )
    {
        Calls++;
        GL20.GLUniform1Iv( location, count, v, offset );
        Check();
    }

    public override void GLUniform2F( int location, float x, float y )
    {
        Calls++;
        GL20.GLUniform2F( location, x, y );
        Check();
    }

    public override void GLUniform2Fv( int location, int count, FloatBuffer v )
    {
        Calls++;
        GL20.GLUniform2Fv( location, count, v );
        Check();
    }

    public override void GLUniform2Fv( int location, int count, float[] v, int offset )
    {
        Calls++;
        GL20.GLUniform2Fv( location, count, v, offset );
        Check();
    }

    public override void GLUniform2I( int location, int x, int y )
    {
        Calls++;
        GL20.GLUniform2I( location, x, y );
        Check();
    }

    public override void GLUniform2Iv( int location, int count, IntBuffer v )
    {
        Calls++;
        GL20.GLUniform2Iv( location, count, v );
        Check();
    }

    public override void GLUniform2Iv( int location, int count, int[] v, int offset )
    {
        Calls++;
        GL20.GLUniform2Iv( location, count, v, offset );
        Check();
    }

    public override void GLUniform3F( int location, float x, float y, float z )
    {
        Calls++;
        GL20.GLUniform3F( location, x, y, z );
        Check();
    }

    public override void GLUniform3Fv( int location, int count, FloatBuffer v )
    {
        Calls++;
        GL20.GLUniform3Fv( location, count, v );
        Check();
    }

    public override void GLUniform3Fv( int location, int count, float[] v, int offset )
    {
        Calls++;
        GL20.GLUniform3Fv( location, count, v, offset );
        Check();
    }

    public override void GLUniform3I( int location, int x, int y, int z )
    {
        Calls++;
        GL20.GLUniform3I( location, x, y, z );
        Check();
    }

    public override void GLUniform3Iv( int location, int count, IntBuffer v )
    {
        Calls++;
        GL20.GLUniform3Iv( location, count, v );
        Check();
    }

    public override void GLUniform3Iv( int location, int count, int[] v, int offset )
    {
        Calls++;
        GL20.GLUniform3Iv( location, count, v, offset );
        Check();
    }

    public override void GLUniform4F( int location, float x, float y, float z, float w )
    {
        Calls++;
        GL20.GLUniform4F( location, x, y, z, w );
        Check();
    }

    public override void GLUniform4Fv( int location, int count, FloatBuffer v )
    {
        Calls++;
        GL20.GLUniform4Fv( location, count, v );
        Check();
    }

    public override void GLUniform4Fv( int location, int count, float[] v, int offset )
    {
        Calls++;
        GL20.GLUniform4Fv( location, count, v, offset );
        Check();
    }

    public override void GLUniform4I( int location, int x, int y, int z, int w )
    {
        Calls++;
        GL20.GLUniform4I( location, x, y, z, w );
        Check();
    }

    public override void GLUniform4Iv( int location, int count, IntBuffer v )
    {
        Calls++;
        GL20.GLUniform4Iv( location, count, v );
        Check();
    }

    public override void GLUniform4Iv( int location, int count, int[] v, int offset )
    {
        Calls++;
        GL20.GLUniform4Iv( location, count, v, offset );
        Check();
    }

    public override void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL20.GLUniformMatrix2Fv( location, count, transpose, value );
        Check();
    }

    public override void GLUniformMatrix2Fv( int location, int count, bool transpose, float[] value, int offset )
    {
        Calls++;
        GL20.GLUniformMatrix2Fv( location, count, transpose, value, offset );
        Check();
    }

    public override void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL20.GLUniformMatrix3Fv( location, count, transpose, value );
        Check();
    }

    public override void GLUniformMatrix3Fv( int location, int count, bool transpose, float[] value, int offset )
    {
        Calls++;
        GL20.GLUniformMatrix3Fv( location, count, transpose, value, offset );
        Check();
    }

    public override void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value )
    {
        Calls++;
        GL20.GLUniformMatrix4Fv( location, count, transpose, value );
        Check();
    }

    public override void GLUniformMatrix4Fv( int location, int count, bool transpose, float[] value, int offset )
    {
        Calls++;
        GL20.GLUniformMatrix4Fv( location, count, transpose, value, offset );
        Check();
    }

    public override void GLUseProgram( int program )
    {
        Calls++;
        GL20.GLUseProgram( program );
        Check();
    }

    public override void GLValidateProgram( int program )
    {
        Calls++;
        GL20.GLValidateProgram( program );
        Check();
    }

    public override void GLVertexAttrib1F( int indx, float x )
    {
        Calls++;
        GL20.GLVertexAttrib1F( indx, x );
        Check();
    }

    public override void GLVertexAttrib1Fv( int indx, FloatBuffer values )
    {
        Calls++;
        GL20.GLVertexAttrib1Fv( indx, values );
        Check();
    }

    public override void GLVertexAttrib2F( int indx, float x, float y )
    {
        Calls++;
        GL20.GLVertexAttrib2F( indx, x, y );
        Check();
    }

    public override void GLVertexAttrib2Fv( int indx, FloatBuffer values )
    {
        Calls++;
        GL20.GLVertexAttrib2Fv( indx, values );
        Check();
    }

    public override void GLVertexAttrib3F( int indx, float x, float y, float z )
    {
        Calls++;
        GL20.GLVertexAttrib3F( indx, x, y, z );
        Check();
    }

    public override void GLVertexAttrib3Fv( int indx, FloatBuffer values )
    {
        Calls++;
        GL20.GLVertexAttrib3Fv( indx, values );
        Check();
    }

    public override void GLVertexAttrib4F( int indx, float x, float y, float z, float w )
    {
        Calls++;
        GL20.GLVertexAttrib4F( indx, x, y, z, w );
        Check();
    }

    public override void GLVertexAttrib4Fv( int indx, FloatBuffer values )
    {
        Calls++;
        GL20.GLVertexAttrib4Fv( indx, values );
        Check();
    }

    public override void GLVertexAttribPointer( int indx, int size, VertexAttribType type, bool normalized, int stride, Buffer ptr )
    {
        Calls++;
        GL20.GLVertexAttribPointer( indx, size, type, normalized, stride, ptr );
        Check();
    }

    public override void GLVertexAttribPointer( int indx, int size, VertexAttribType type, bool normalized, int stride, int ptr )
    {
        Calls++;
        GL20.GLVertexAttribPointer( indx, size, type, normalized, stride, ptr );
        Check();
    }
}
